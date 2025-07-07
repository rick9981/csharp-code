using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMergeGrid
{
    public class MergeableDataGridView : DataGridView
    {
        private List<MergeArea> mergeAreas;
        private Dictionary<int, object> originalRowData; // 存储原始行数据用于排序后重新匹配
        private bool autoRefreshMergeOnSort = true; // 是否在排序后自动刷新合并

        public MergeableDataGridView()
        {
            mergeAreas = new List<MergeArea>();
            originalRowData = new Dictionary<int, object>();

            // 启用双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.DoubleBuffer |
                         ControlStyles.ResizeRedraw, true);

            this.UpdateStyles();

            // 监听排序事件
            this.Sorted += OnDataGridViewSorted;
            this.DataSourceChanged += OnDataSourceChanged;
        }

        /// <summary>
        /// 是否在排序后自动刷新合并区域
        /// </summary>
        public bool AutoRefreshMergeOnSort
        {
            get { return autoRefreshMergeOnSort; }
            set { autoRefreshMergeOnSort = value; }
        }

        /// <summary>
        /// 合并指定区域的单元格
        /// </summary>
        public void MergeCells(int startRow, int startCol, int endRow, int endCol, string text = "",
            StringAlignment horizontalAlign = StringAlignment.Center,
            StringAlignment verticalAlign = StringAlignment.Center)
        {
            if (startRow > endRow || startCol > endCol)
                throw new ArgumentException("起始位置不能大于结束位置");

            if (startRow < 0 || endRow >= this.Rows.Count ||
                startCol < 0 || endCol >= this.Columns.Count)
                throw new ArgumentException("行列索引超出范围");

            // 检查是否与现有合并区域重叠
            RemoveOverlappingMerges(startRow, startCol, endRow, endCol);

            var mergeArea = new MergeArea
            {
                StartRow = startRow,
                StartColumn = startCol,
                EndRow = endRow,
                EndColumn = endCol,
                Text = text,
                HorizontalAlignment = horizontalAlign,
                VerticalAlignment = verticalAlign,
                OriginalRowKeys = GetRowKeys(startRow, endRow) // 存储原始行标识
            };

            mergeAreas.Add(mergeArea);

            // 重绘整个控件
            this.Invalidate();
        }

        /// <summary>
        /// 获取行的唯一标识键
        /// </summary>
        private List<string> GetRowKeys(int startRow, int endRow)
        {
            var keys = new List<string>();
            for (int i = startRow; i <= endRow; i++)
            {
                if (i < this.Rows.Count)
                {
                    // 使用行的所有列值组成唯一键
                    var rowKey = string.Join("|",
                        this.Columns.Cast<DataGridViewColumn>()
                        .Select(col => this.Rows[i].Cells[col.Index].Value?.ToString() ?? ""));
                    keys.Add(rowKey);
                }
            }
            return keys;
        }

        /// <summary>
        /// 根据行键查找当前行索引
        /// </summary>
        private int FindRowByKey(string rowKey)
        {
            for (int i = 0; i < this.Rows.Count; i++)
            {
                var currentKey = string.Join("|",
                    this.Columns.Cast<DataGridViewColumn>()
                    .Select(col => this.Rows[i].Cells[col.Index].Value?.ToString() ?? ""));

                if (currentKey == rowKey)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 数据源改变时的处理
        /// </summary>
        private void OnDataSourceChanged(object sender, EventArgs e)
        {
            // 数据源改变时清除所有合并
            ClearAllMerges();
        }

        /// <summary>
        /// 排序完成后的处理
        /// </summary>
        private void OnDataGridViewSorted(object sender, EventArgs e)
        {
            if (autoRefreshMergeOnSort)
            {
                RefreshMergeAreasAfterSort();
            }
        }

        /// <summary>
        /// 排序后刷新合并区域
        /// </summary>
        private void RefreshMergeAreasAfterSort()
        {
            var updatedMergeAreas = new List<MergeArea>();

            foreach (var mergeArea in mergeAreas.ToList())
            {
                // 根据原始行键重新查找行位置
                var newRowIndexes = new List<int>();

                foreach (var rowKey in mergeArea.OriginalRowKeys)
                {
                    int newIndex = FindRowByKey(rowKey);
                    if (newIndex >= 0)
                    {
                        newRowIndexes.Add(newIndex);
                    }
                }

                // 如果找到了所有行且它们是连续的，则更新合并区域
                if (newRowIndexes.Count == mergeArea.OriginalRowKeys.Count)
                {
                    newRowIndexes.Sort();

                    // 检查是否连续
                    bool isContinuous = true;
                    for (int i = 1; i < newRowIndexes.Count; i++)
                    {
                        if (newRowIndexes[i] != newRowIndexes[i - 1] + 1)
                        {
                            isContinuous = false;
                            break;
                        }
                    }

                    if (isContinuous)
                    {
                        // 更新合并区域的行索引
                        var updatedMergeArea = new MergeArea
                        {
                            StartRow = newRowIndexes.First(),
                            StartColumn = mergeArea.StartColumn,
                            EndRow = newRowIndexes.Last(),
                            EndColumn = mergeArea.EndColumn,
                            Text = mergeArea.Text,
                            HorizontalAlignment = mergeArea.HorizontalAlignment,
                            VerticalAlignment = mergeArea.VerticalAlignment,
                            OriginalRowKeys = mergeArea.OriginalRowKeys
                        };

                        updatedMergeAreas.Add(updatedMergeArea);
                    }
                }
            }

            // 更新合并区域列表
            mergeAreas = updatedMergeAreas;

            // 重绘
            this.Invalidate();
        }

        /// <summary>
        /// 手动刷新合并区域（用于排序后）
        /// </summary>
        public void RefreshMergeAreas()
        {
            RefreshMergeAreasAfterSort();
        }

        /// <summary>
        /// 基于列值自动重新合并（排序后使用）
        /// </summary>
        public void AutoReMergeAfterSort()
        {
            // 存储当前的合并配置
            var mergeConfigs = new List<ColumnMergeConfig>();

            foreach (var mergeArea in mergeAreas)
            {
                if (mergeArea.StartColumn == mergeArea.EndColumn) // 只处理单列合并
                {
                    mergeConfigs.Add(new ColumnMergeConfig
                    {
                        ColumnIndex = mergeArea.StartColumn,
                        HorizontalAlign = mergeArea.HorizontalAlignment,
                        VerticalAlign = mergeArea.VerticalAlignment
                    });
                }
            }

            // 清除现有合并
            ClearAllMerges();

            // 重新自动合并
            foreach (var config in mergeConfigs.Distinct())
            {
                this.AutoMergeColumn(config.ColumnIndex, config.HorizontalAlign, config.VerticalAlign);
            }
        }

        /// <summary>
        /// 移除重叠的合并区域
        /// </summary>
        private void RemoveOverlappingMerges(int startRow, int startCol, int endRow, int endCol)
        {
            mergeAreas.RemoveAll(area =>
                !(endRow < area.StartRow || startRow > area.EndRow ||
                  endCol < area.StartColumn || startCol > area.EndColumn));
        }

        /// <summary>
        /// 清除所有合并
        /// </summary>
        public void ClearAllMerges()
        {
            mergeAreas.Clear();
            this.Invalidate();
        }

        /// <summary>
        /// 获取指定单元格所属的合并区域
        /// </summary>
        private MergeArea GetMergeArea(int rowIndex, int columnIndex)
        {
            return mergeAreas.FirstOrDefault(area =>
                rowIndex >= area.StartRow && rowIndex <= area.EndRow &&
                columnIndex >= area.StartColumn && columnIndex <= area.EndColumn);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // 先绘制基础的DataGridView
            base.OnPaint(e);

            // 然后绘制合并区域
            DrawMergedAreas(e.Graphics);
        }

        /// <summary>
        /// 绘制所有合并区域
        /// </summary>
        private void DrawMergedAreas(Graphics g)
        {
            foreach (var mergeArea in mergeAreas)
            {
                DrawSingleMergeArea(g, mergeArea);
            }
        }

        /// <summary>
        /// 绘制单个合并区域
        /// </summary>
        private void DrawSingleMergeArea(Graphics g, MergeArea mergeArea)
        {
            try
            {
                Rectangle mergeRect = GetMergeAreaRectangle(mergeArea);
                if (mergeRect.IsEmpty || mergeRect.Width <= 0 || mergeRect.Height <= 0)
                    return;

                // 检查合并区域是否在可见区域内
                Rectangle visibleRect = this.DisplayRectangle;
                if (!mergeRect.IntersectsWith(visibleRect))
                    return;

                // 设置高质量绘制
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                // 判断是否被选中
                bool isSelected = IsMergeAreaSelected(mergeArea);

                // 绘制背景
                Color backColor = isSelected ? this.DefaultCellStyle.SelectionBackColor : this.DefaultCellStyle.BackColor;
                using (Brush backBrush = new SolidBrush(backColor))
                {
                    g.FillRectangle(backBrush, mergeRect);
                }

                // 绘制边框
                using (Pen borderPen = new Pen(this.GridColor, 1))
                {
                    g.DrawRectangle(borderPen, mergeRect);
                }

                // 绘制文本
                if (!string.IsNullOrEmpty(mergeArea.Text))
                {
                    Color textColor = isSelected ? this.DefaultCellStyle.SelectionForeColor : this.DefaultCellStyle.ForeColor;
                    using (Brush textBrush = new SolidBrush(textColor))
                    {
                        // 计算文本区域，留出边距
                        Rectangle textRect = new Rectangle(
                            mergeRect.X + 3,
                            mergeRect.Y + 3,
                            mergeRect.Width - 6,
                            mergeRect.Height - 6);

                        StringFormat sf = new StringFormat
                        {
                            Alignment = mergeArea.HorizontalAlignment,
                            LineAlignment = mergeArea.VerticalAlignment,
                            Trimming = StringTrimming.EllipsisCharacter,
                            FormatFlags = StringFormatFlags.NoWrap
                        };

                        Font font = this.DefaultCellStyle.Font ?? this.Font;
                        g.DrawString(mergeArea.Text, font, textBrush, textRect, sf);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"绘制合并区域出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 判断合并区域是否被选中
        /// </summary>
        private bool IsMergeAreaSelected(MergeArea mergeArea)
        {
            if (this.CurrentCell == null) return false;

            return this.CurrentCell.RowIndex >= mergeArea.StartRow &&
                   this.CurrentCell.RowIndex <= mergeArea.EndRow &&
                   this.CurrentCell.ColumnIndex >= mergeArea.StartColumn &&
                   this.CurrentCell.ColumnIndex <= mergeArea.EndColumn;
        }

        /// <summary>
        /// 获取合并区域的矩形
        /// </summary>
        private Rectangle GetMergeAreaRectangle(MergeArea mergeArea)
        {
            try
            {
                // 检查索引有效性
                if (mergeArea.StartRow >= this.Rows.Count ||
                    mergeArea.EndRow >= this.Rows.Count ||
                    mergeArea.StartColumn >= this.Columns.Count ||
                    mergeArea.EndColumn >= this.Columns.Count)
                {
                    return Rectangle.Empty;
                }

                // 计算列的位置
                int left = 0;
                for (int i = 0; i < mergeArea.StartColumn; i++)
                {
                    if (this.Columns[i].Visible)
                        left += this.Columns[i].Width;
                }
                left += this.RowHeadersWidth;

                int width = 0;
                for (int i = mergeArea.StartColumn; i <= mergeArea.EndColumn; i++)
                {
                    if (this.Columns[i].Visible)
                        width += this.Columns[i].Width;
                }

                // 计算行的位置
                int top = this.ColumnHeadersHeight;
                for (int i = 0; i < mergeArea.StartRow; i++)
                {
                    top += this.Rows[i].Height;
                }

                int height = 0;
                for (int i = mergeArea.StartRow; i <= mergeArea.EndRow; i++)
                {
                    height += this.Rows[i].Height;
                }

                // 考虑滚动偏移
                left -= this.HorizontalScrollingOffset;
                top -= this.VerticalScrollingOffset;

                return new Rectangle(left, top, width, height);
            }
            catch
            {
                return Rectangle.Empty;
            }
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var mergeArea = GetMergeArea(e.RowIndex, e.ColumnIndex);
                if (mergeArea != null)
                {
                    // 对于合并区域内的单元格，不绘制内容，只绘制背景
                    e.PaintBackground(e.CellBounds, false);
                    e.Handled = true;
                    return;
                }
            }

            base.OnCellPainting(e);
        }

        protected override void OnCellClick(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var mergeArea = GetMergeArea(e.RowIndex, e.ColumnIndex);
                if (mergeArea != null)
                {
                    this.CurrentCell = this[mergeArea.StartColumn, mergeArea.StartRow];
                    return;
                }
            }

            base.OnCellClick(e);
        }

        protected override void OnSelectionChanged(EventArgs e)
        {
            base.OnSelectionChanged(e);
            this.Invalidate();
        }

        protected override void OnScroll(ScrollEventArgs e)
        {
            base.OnScroll(e);
            this.Invalidate();
        }
    }

    /// <summary>
    /// 合并区域信息
    /// </summary>
    public class MergeArea
    {
        public int StartRow { get; set; }
        public int StartColumn { get; set; }
        public int EndRow { get; set; }
        public int EndColumn { get; set; }
        public string Text { get; set; }
        public StringAlignment HorizontalAlignment { get; set; } = StringAlignment.Center;
        public StringAlignment VerticalAlignment { get; set; } = StringAlignment.Center;
        public List<string> OriginalRowKeys { get; set; } = new List<string>(); // 存储原始行键
    }

    /// <summary>
    /// 列合并配置
    /// </summary>
    public class ColumnMergeConfig
    {
        public int ColumnIndex { get; set; }
        public StringAlignment HorizontalAlign { get; set; }
        public StringAlignment VerticalAlign { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ColumnMergeConfig config && ColumnIndex == config.ColumnIndex;
        }

        public override int GetHashCode()
        {
            return ColumnIndex.GetHashCode();
        }
    }
}
