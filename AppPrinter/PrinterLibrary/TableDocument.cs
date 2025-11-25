using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZXing;
using ZXing.Windows.Compatibility;

namespace PrinterLibrary
{
    public class TableDocument
    {
        public List<Column> Columns { get; } = new();
        public List<Row> Rows { get; } = new();
        public List<Position> Positions { get; } = new(); // 单元格位置索引
        public Dictionary<Position, Position> MergeCell { get; } = new(); // 合并单元格信息

        private readonly Dictionary<(int Row, int Col), Position> _positionMap = new();
        private readonly Dictionary<(string? h, string? v), StringFormat> _stringFormatCache = new();

        public void ClearState()
        {
            Columns.Clear();
            Rows.Clear();
            Positions.Clear();
            MergeCell.Clear();
            _positionMap.Clear();
            _stringFormatCache.Clear();
        }

        public Position? GetPosition(int rowIndex, int columnIndex)
        {
            _positionMap.TryGetValue((rowIndex, columnIndex), out var p);
            return p;
        }

        public int GetColumnWidth(int columnIndex) => Columns[columnIndex].Width;
        public int GetRowHeight(int rowIndex) => Rows[rowIndex].Height;

        public void DrawImageInCell(Graphics graphics, Position position, Image image, Point offset)
        {
            var rect = new Rectangle(position.Point.X + offset.X, position.Point.Y + offset.Y, image.Width, image.Height);
            graphics.DrawImage(image, rect);
        }

        public Image ResizeImage(Image image, int width, int height)
        {
            var resized = new Bitmap(width, height);
            using var g = Graphics.FromImage(resized);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawImage(image, new Rectangle(0, 0, width, height));
            return resized;
        }

        public void DrawQRCode(Graphics graphics, string code, Position position, Point offset, int width = 64, int height = 64)
        {
            var writer = new BarcodeWriter
            {
                Options = new ZXing.Common.EncodingOptions { Width = width, Height = height, Margin = 0, PureBarcode = true },
                Format = ZXing.BarcodeFormat.QR_CODE
            };
            using var bitmap = writer.Write(code);
            DrawImageInCell(graphics, position, bitmap, offset);
        }

        public void Draw128Code(Graphics graphics, string code, Position position, int width, int height, Point offset, bool pureBarcode = false)
        {
            var writer = new BarcodeWriter
            {
                Options = new ZXing.Common.EncodingOptions { Width = width, Height = height, Margin = 0, PureBarcode = pureBarcode },
                Format = ZXing.BarcodeFormat.CODE_128
            };
            using var bitmap = writer.Write(code);
            DrawImageInCell(graphics, position, bitmap, offset);
        }

        private void DrawCellText(Graphics graphics, Position pos, string text, Font font, Brush brush,
            int mergeRowSpan, int mergeColSpan, StringFormat stringFormat)
        {
            int rectHeight = GetRowHeight(pos.RowIndex);
            for (int i = 0; i < mergeRowSpan; i++) rectHeight += GetRowHeight(pos.RowIndex + i);

            int rectWidth = GetColumnWidth(pos.ColumnIndex);
            for (int i = 1; i < mergeColSpan; i++) rectWidth += GetColumnWidth(pos.ColumnIndex + i);

            var layout = new RectangleF(pos.Point.X, pos.Point.Y, rectWidth, rectHeight);
            graphics.DrawString(text, font, brush, layout, stringFormat);
        }

        public static SizeF MeasureTextSize(Graphics graphics, string text, Font font) => graphics.MeasureString(text, font);

        public void Draw(Graphics graphics, TableConfig config)
        {
            graphics.SmoothingMode = SmoothingMode.None;
            int startX = config.StartPosition.X;
            int startY = config.StartPosition.Y;

            using var pen = new Pen(Color.Black, 1);

            // 构建坐标(Position)而不立即绘制格子
            int currentY = startY;
            for (int r = 0; r < Rows.Count; r++)
            {
                int currentX = startX;
                for (int c = 0; c < Columns.Count; c++)
                {
                    var pos = new Position(r, c, new Point(currentX, currentY));
                    Positions.Add(pos);
                    _positionMap[(r, c)] = pos;
                    currentX += Columns[c].Width;
                }
                currentY += Rows[r].Height;
            }

            // 计算合并区域矩形（用于跳过内部边界线）
            var mergedRects = new List<Rectangle>();
            foreach (var kv in MergeCell)
            {
                int startRowZero = kv.Key.RowIndex - 1;
                int startColZero = kv.Key.ColumnIndex - 1;
                int endRow = kv.Value.RowIndex - 1;     // inclusive zero-based
                int endCol = kv.Value.ColumnIndex - 1;  // inclusive zero-based
                var startPos = GetPosition(startRowZero, startColZero);
                if (startPos == null) continue;
                int w = 0; int h = 0;
                for (int c = startColZero; c <= endCol; c++) w += GetColumnWidth(c);
                for (int r = startRowZero; r <= endRow; r++) h += GetRowHeight(r);
                mergedRects.Add(new Rectangle(startPos.Point.X, startPos.Point.Y, w, h));
            }

            // 列边界坐标
            var colBoundaries = new int[Columns.Count + 1];
            colBoundaries[0] = startX;
            for (int i = 0; i < Columns.Count; i++) colBoundaries[i + 1] = colBoundaries[i] + Columns[i].Width;
            // 行边界坐标
            var rowBoundaries = new int[Rows.Count + 1];
            rowBoundaries[0] = startY;
            for (int i = 0; i < Rows.Count; i++) rowBoundaries[i + 1] = rowBoundaries[i] + Rows[i].Height;

            int totalWidth = colBoundaries[^1] - startX;
            int totalHeight = rowBoundaries[^1] - startY;

            // 绘制外框
            graphics.DrawRectangle(pen, startX, startY, totalWidth, totalHeight);

            // 垂直线（内部列分隔）
            for (int ci = 1; ci < colBoundaries.Length - 1; ci++)
            {
                int x = colBoundaries[ci];
                // 分段绘制，遇到合并区域内部则跳过
                int segmentStartY = startY;
                for (int ri = 0; ri < rowBoundaries.Length - 1; ri++)
                {
                    int yTop = rowBoundaries[ri];
                    int yBottom = rowBoundaries[ri + 1];

                    if (IsVerticalLineInsideMerged(x, yTop, yBottom, mergedRects))
                    {
                        // 跳过该段
                        continue;
                    }
                    graphics.DrawLine(pen, x, yTop, x, yBottom);
                }
            }

            // 水平线（内部行分隔）
            for (int ri = 1; ri < rowBoundaries.Length - 1; ri++)
            {
                int y = rowBoundaries[ri];
                for (int ci = 0; ci < colBoundaries.Length - 1; ci++)
                {
                    int xLeft = colBoundaries[ci];
                    int xRight = colBoundaries[ci + 1];
                    if (IsHorizontalLineInsideMerged(y, xLeft, xRight, mergedRects))
                    {
                        continue;
                    }
                    graphics.DrawLine(pen, xLeft, y, xRight, y);
                }
            }

            // 行与单元格边框
            foreach (var row in config.Rows)
            {
                if (!int.TryParse(row.RowIndex, out int intRowIndex)) continue;
                if (row.Borders != null)
                {
                    foreach (var br in row.Borders)
                    {
                        using var penBorder = CreatePen(br);
                        switch (br.Direction)
                        {
                            case "Top":
                                {
                                    var leftX = startX;
                                    var rightX = colBoundaries[^1];
                                    int y = rowBoundaries[intRowIndex - 1];
                                    graphics.DrawLine(penBorder, leftX, y, rightX, y);
                                    break;
                                }
                            case "Bottom":
                                {
                                    var leftX = startX;
                                    var rightX = colBoundaries[^1];
                                    int y = rowBoundaries[intRowIndex];
                                    graphics.DrawLine(penBorder, leftX, y, rightX, y);
                                    break;
                                }
                            case "Left":
                                {
                                    int x = startX;
                                    int y1 = rowBoundaries[intRowIndex - 1];
                                    int y2 = rowBoundaries[intRowIndex];
                                    graphics.DrawLine(penBorder, x, y1, x, y2);
                                    break;
                                }
                            case "Right":
                                {
                                    int x = colBoundaries[^1];
                                    int y1 = rowBoundaries[intRowIndex - 1];
                                    int y2 = rowBoundaries[intRowIndex];
                                    graphics.DrawLine(penBorder, x, y1, x, y2);
                                    break;
                                }
                        }
                    }
                }

                foreach (var cell in row.Cells)
                {
                    if (cell.Borders == null || cell.Borders.Count == 0) continue;
                    if (!int.TryParse(cell.ColumnIndex, out int colIndex)) continue;
                    int cellLeft = colBoundaries[colIndex - 1];
                    int cellRight = colBoundaries[colIndex];
                    int cellTop = rowBoundaries[intRowIndex - 1];
                    int cellBottom = rowBoundaries[intRowIndex];
                    foreach (var br in cell.Borders)
                    {
                        if (br.Direction != "Left" && br.Direction != "Right") continue;
                        using var penBorder = CreatePen(br);
                        if (br.Direction == "Left")
                        {
                            graphics.DrawLine(penBorder, cellLeft, cellTop, cellLeft, cellBottom);
                        }
                        else
                        {
                            graphics.DrawLine(penBorder, cellRight, cellTop, cellRight, cellBottom);
                        }
                    }
                }
            }
        }

        private bool IsVerticalLineInsideMerged(int x, int yTop, int yBottom, List<Rectangle> mergedRects)
        {
            foreach (var rect in mergedRects)
            {
                if (x > rect.Left && x < rect.Right && yTop >= rect.Top && yBottom <= rect.Bottom)
                {
                    return true; // 该垂直线段位于合并区域内部
                }
            }
            return false;
        }

        private bool IsHorizontalLineInsideMerged(int y, int xLeft, int xRight, List<Rectangle> mergedRects)
        {
            foreach (var rect in mergedRects)
            {
                if (y > rect.Top && y < rect.Bottom && xLeft >= rect.Left && xRight <= rect.Right)
                {
                    return true; // 该水平线段位于合并区域内部
                }
            }
            return false;
        }

        private static Pen CreatePen(TableConfig.Border br)
        {
            Brush brush = br.Style switch
            {
                "Dot" => new HatchBrush(HatchStyle.DashedHorizontal, ColorTranslator.FromHtml(br.Color), Color.White),
                _ => new SolidBrush(ColorTranslator.FromHtml(br.Color))
            };
            if (br.Width == 0)
            {
                brush.Dispose();
                return new Pen(Color.White, 2);
            }
            return new Pen(brush, br.Width);
        }

        public async Task DrawTableAsync(Graphics graphics, object dy, IEnumerable<object> items, TableConfig config)
        {
            dy ??= new object();
            items ??= Enumerable.Empty<object>();
            int itemCount = items.Count();
            ClearState();
            await OrganizeConfigAsync(config, itemCount);
            DrawStructure(config, itemCount);
            Draw(graphics, config);

            var headerType = dy.GetType();
            var headerProps = headerType.GetProperties().ToDictionary(p => p.Name, p => p);
            var itemPropCache = new Dictionary<Type, Dictionary<string, System.Reflection.PropertyInfo>>();

            int currentDynamicCursor = 0;
            foreach (var row in config.Rows.OrderBy(r => int.Parse(r.RowIndex)))
            {
                int rowIndex = int.Parse(row.RowIndex);
                if (row.Is_Items)
                {
                    currentDynamicCursor = rowIndex;
                    int seq = 1;
                    foreach (var it in items)
                    {
                        var t = it.GetType();
                        if (!itemPropCache.TryGetValue(t, out var dict))
                        {
                            dict = t.GetProperties().ToDictionary(p => p.Name, p => p);
                            itemPropCache[t] = dict;
                        }
                        foreach (var cell in row.Cells)
                        {
                            if (!int.TryParse(cell.ColumnIndex, out int cIndex)) continue;
                            var pos = GetPosition(currentDynamicCursor - 1, cIndex - 1);
                            if (pos == null) continue;
                            string title = cell.Title ?? string.Empty;
                            var fmt = GetCachedStringFormat(cell.Alignment, cell.LineAlignment);
                            title = ReplacePlaceHolderForItem(title, it, dict, seq);
                            var (mergeRowSpan, mergeColSpan) = GetMergeSpan(cell.Merge, seq);
                            DrawCellText(graphics, pos, title, CellFont(cell), Brushes.Black, mergeRowSpan, mergeColSpan, fmt);
                        }
                        currentDynamicCursor++;
                        seq++;
                    }
                }
                else
                {
                    foreach (var cell in row.Cells)
                    {
                        if (!int.TryParse(cell.ColumnIndex, out int cIndex)) continue;
                        var pos = GetPosition(rowIndex - 1, cIndex - 1);
                        if (pos == null) continue;
                        string title = cell.Title ?? string.Empty;
                        var fmt = GetCachedStringFormat(cell.Alignment, cell.LineAlignment);
                        title = ReplacePlaceHolderForHeader(title, dy, headerProps);
                        var (mergeRowSpan, mergeColSpan) = GetMergeSpan(cell.Merge, null);
                        DrawCellText(graphics, pos, title, CellFont(cell), Brushes.Black, mergeRowSpan, mergeColSpan, fmt);
                    }
                }
            }

            foreach (var barcode in config.BarCodes ?? Enumerable.Empty<TableConfig.BarCode>())
            {
                if (!int.TryParse(barcode.RowIndex, out int rIdx) || !int.TryParse(barcode.ColumnIndex, out int cIdx)) continue;
                var pos = GetPosition(rIdx - 1, cIdx - 1);
                if (pos == null) continue;
                string raw = barcode.Tilte ?? string.Empty;
                string replaced = ReplacePlaceHolderForHeader(raw, dy, headerProps);
                barcode.Sizes ??= new Size(64, 64);
                if (barcode.BarcodeType == "QRCODE")
                {
                    DrawQRCode(graphics, replaced, pos, new Point(15, 5), barcode.Sizes.Value.Width, barcode.Sizes.Value.Height);
                }
                else if (barcode.BarcodeType == "CODE128")
                {
                    Draw128Code(graphics, replaced, pos, barcode.Sizes.Value.Width, barcode.Sizes.Value.Height, new Point(2, 2));
                }
            }
        }

        private (int rowSpan, int colSpan) GetMergeSpan(string[]? merge, int? seqNo)
        {
            if (merge == null || merge.Length < 4) return (0, 0);
            string startRow = merge[0];
            string startCol = merge[1];
            string endRow = merge[2];
            string endCol = merge[3];
            if (seqNo.HasValue)
            {
                if (startRow.Contains("+")) startRow = Calculate(startRow.Replace("seq_no", seqNo.Value.ToString())).ToString();
                if (endRow.Contains("+")) endRow = Calculate(endRow.Replace("seq_no", seqNo.Value.ToString())).ToString();
            }
            int sr = int.Parse(startRow);
            int er = int.Parse(endRow);
            int sc = int.Parse(startCol);
            int ec = int.Parse(endCol);
            int rowSpan = er - sr;
            int colSpan = ec - sc + 1;
            return (rowSpan, colSpan);
        }

        private string ReplacePlaceHolderForItem(string title, object item, Dictionary<string, System.Reflection.PropertyInfo> props, int seq)
        {
            var matches = Regex.Matches(title, @"\$(.*?)\$");
            foreach (Match m in matches.Cast<Match>())
            {
                string expr = m.Groups[1].Value;
                if (expr == "seq_no")
                {
                    title = title.Replace($"${expr}$", seq.ToString());
                    continue;
                }
                var parts = expr.Split(',');
                string propName = parts[0];
                if (!props.TryGetValue(propName, out var pi)) continue;
                var valueObj = pi.GetValue(item);
                string value = valueObj?.ToString() ?? string.Empty;
                if (parts.Length > 1 && parts[1] == "date" && parts.Length > 2)
                {
                    if (DateTime.TryParse(value, out var dt)) value = dt.ToString(parts[2]);
                }
                title = title.Replace($"${expr}$", value);
            }
            return title;
        }

        private string ReplacePlaceHolderForHeader(string title, object header, Dictionary<string, System.Reflection.PropertyInfo> props)
        {
            var matches = Regex.Matches(title, @"\$(.*?)\$");
            foreach (Match m in matches.Cast<Match>())
            {
                string expr = m.Groups[1].Value;
                var parts = expr.Split(',');
                string propName = parts[0];
                if (!props.TryGetValue(propName, out var pi)) continue;
                var valueObj = pi.GetValue(header);
                string value = valueObj?.ToString() ?? string.Empty;
                if (parts.Length > 1 && parts[1] == "date" && parts.Length > 2)
                {
                    if (DateTime.TryParse(value, out var dt)) value = dt.ToString(parts[2]);
                }
                title = title.Replace($"${expr}$", value);
            }
            return title;
        }

        private Font CellFont(TableConfig.Cell cell)
        {
            if (cell.Font == null) return new Font("SimHei", 12, FontStyle.Regular);
            FontStyle style = FontStyle.Regular;
            if (cell.Font.Bold) style |= FontStyle.Bold;
            if (cell.Font.Italic) style |= FontStyle.Italic;
            return new Font(cell.Font.Name, cell.Font.Size, style);
        }

        public void DrawStructure(TableConfig config, int itemCount)
        {
            // 行构建
            for (int i = 0; i < config.RowsCount + itemCount; i++)
            {
                var found = config.Rows.FirstOrDefault(x => x.RowIndex == (i + 1).ToString());
                if (found != null)
                {
                    int h = 35;
                    if (!string.IsNullOrEmpty(found.Height) && int.TryParse(found.Height, out int parsed)) h = parsed;
                    Rows.Add(new Row(h));
                }
                else
                {
                    Rows.Add(new Row(35));
                }
            }
            // 列构建
            foreach (var w in config.ColumnsWidths) Columns.Add(new Column(w));

            // 追加动态行
            var dynamicTemplate = config.Rows.FirstOrDefault(x => x.Is_Items);
            if (dynamicTemplate != null)
            {
                for (int i = 0; i < itemCount - 1; i++)
                {
                    var clone = JsonSerializer.Deserialize<TableConfig.Row>(JsonSerializer.Serialize(dynamicTemplate));
                    if (clone != null) config.Rows.Add(clone);
                }
            }

            int dynSeq = 0;
            foreach (var row in config.Rows)
            {
                var merges = row.Cells.Where(c => c.Merge != null).ToList();
                foreach (var merge in merges)
                {
                    if (merge.Merge.Length < 4) continue;
                    string startRowStr = merge.Merge[0];
                    string startColStr = merge.Merge[1];
                    string endRowStr = merge.Merge[2];
                    string endColStr = merge.Merge[3];
                    if (row.Is_Items)
                    {
                        if (startRowStr.Contains("+")) startRowStr = Calculate(startRowStr.Replace("seq_no", dynSeq.ToString())).ToString();
                        if (endRowStr.Contains("+")) endRowStr = Calculate(endRowStr.Replace("seq_no", dynSeq.ToString())).ToString();
                    }
                    int startRowIndex = int.Parse(startRowStr);
                    int startColumnIndex = int.Parse(startColStr);
                    int endRowIndex = int.Parse(endRowStr);
                    int endColumnIndex = int.Parse(endColStr);
                    MergeCell[new Position(startRowIndex, startColumnIndex)] = new Position(endRowIndex, endColumnIndex);
                }
                if (row.Is_Items) dynSeq++;
            }
        }

        public async Task OrganizeConfigAsync(TableConfig config, int itemCount)
        {
            foreach (var row in config.Rows)
            {
                if (row.RowIndex.Contains("Items.Count"))
                {
                    string expr = row.RowIndex.Replace("Items.Count", itemCount.ToString());
                    row.RowIndex = (await CSharpScript.EvaluateAsync(expr)).ToString();
                }
            }
            foreach (var row in config.Rows)
            {
                foreach (var cell in row.Cells.Where(c => c.Merge != null))
                {
                    if (cell.Merge[0].Contains("Items.Count"))
                    {
                        string expr = cell.Merge[0].Replace("Items.Count", itemCount.ToString());
                        cell.Merge[0] = (await CSharpScript.EvaluateAsync(expr)).ToString();
                    }
                    if (cell.Merge[2].Contains("Items.Count"))
                    {
                        string expr = cell.Merge[2].Replace("Items.Count", itemCount.ToString());
                        cell.Merge[2] = (await CSharpScript.EvaluateAsync(expr)).ToString();
                    }
                }
            }
            foreach (var bc in config.BarCodes ?? Enumerable.Empty<TableConfig.BarCode>())
            {
                if (bc.RowIndex != null && bc.RowIndex.Contains("Items.Count"))
                {
                    string expr = bc.RowIndex.Replace("Items.Count", itemCount.ToString());
                    bc.RowIndex = (await CSharpScript.EvaluateAsync(expr)).ToString();
                }
            }
        }

        private StringFormat GetCachedStringFormat(string? alignment, string? lineAlignment)
        {
            var key = (alignment, lineAlignment);
            if (_stringFormatCache.TryGetValue(key, out var sf)) return sf;
            sf = new StringFormat
            {
                Alignment = alignment switch { "Left" => StringAlignment.Near, "Right" => StringAlignment.Far, _ => StringAlignment.Center },
                LineAlignment = lineAlignment switch { "Top" => StringAlignment.Near, "Bottom" => StringAlignment.Far, _ => StringAlignment.Center }
            };
            _stringFormatCache[key] = sf;
            return sf;
        }

        public string RemoveCommentsFromJson(string json)
        {
            string singleLine = @"(//.*?$)";
            string multiLine = @"/\*[\s\S]*?\*/";
            return Regex.Replace(json, $"{singleLine}|{multiLine}", string.Empty, RegexOptions.Multiline);
        }

        private int Calculate(string formula)
        {
            try
            {
                var table = new DataTable();
                table.Columns.Add("expression", typeof(string), formula);
                var row = table.NewRow();
                table.Rows.Add(row);
                return Convert.ToInt32(row["expression"]);
            }
            catch
            {
                return 0;
            }
        }
    }
}
