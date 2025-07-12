using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDataGridBar
{
    // 自定义进度条单元格类
    public class DataGridViewProgressCell : DataGridViewTextBoxCell
    {
        public override Type ValueType => typeof(int);

        protected override void Paint(Graphics graphics, Rectangle clipBounds,
            Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState,
            object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle borderStyle,
            DataGridViewPaintParts paintParts)
        {
            // 获取进度值
            int progressVal = 0;
            if (value != null && int.TryParse(value.ToString(), out int tempVal))
            {
                progressVal = Math.Max(0, Math.Min(100, tempVal));
            }

            // 绘制单元格背景
            if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
            {
                using (var backColorBrush = new SolidBrush(cellStyle.BackColor))
                {
                    graphics.FillRectangle(backColorBrush, cellBounds);
                }
            }

            // 绘制进度条
            if ((paintParts & DataGridViewPaintParts.ContentForeground) == DataGridViewPaintParts.ContentForeground)
            {
                Rectangle progressRect = new Rectangle(
                    cellBounds.X + 2,
                    cellBounds.Y + 2,
                    cellBounds.Width - 4,
                    cellBounds.Height - 4
                );

                // 进度条背景
                using (var progressBackBrush = new SolidBrush(Color.FromArgb(240, 240, 240)))
                {
                    graphics.FillRectangle(progressBackBrush, progressRect);
                }

                // 进度条填充
                if (progressVal > 0)
                {
                    int fillWidth = (int)(progressRect.Width * (progressVal / 100.0));
                    Rectangle fillRect = new Rectangle(progressRect.X, progressRect.Y,
                        fillWidth, progressRect.Height);

                    // 使用线性渐变效果
                    using (var fillBrush = new LinearGradientBrush(fillRect,
                        GetProgressStartColor(progressVal), GetProgressEndColor(progressVal),
                        LinearGradientMode.Horizontal))
                    {
                        graphics.FillRectangle(fillBrush, fillRect);
                    }
                }

                // 绘制进度文字
                string progressText = $"{progressVal}%";
                using (var textBrush = new SolidBrush(Color.Black))
                {
                    StringFormat stringFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    graphics.DrawString(progressText, cellStyle.Font, textBrush,
                        cellBounds, stringFormat);
                }

                // 绘制边框
                using (var borderPen = new Pen(Color.FromArgb(200, 200, 200)))
                {
                    graphics.DrawRectangle(borderPen, progressRect);
                }
            }

            // 绘制单元格边框
            if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
            {
                PaintBorder(graphics, clipBounds, cellBounds, cellStyle, borderStyle);
            }
        }

        private Color GetProgressStartColor(int progress)
        {
            if (progress < 30) return Color.FromArgb(255, 99, 132);
            if (progress < 70) return Color.FromArgb(255, 205, 86);
            if (progress < 100) return Color.FromArgb(75, 192, 192);
            return Color.FromArgb(54, 162, 235);
        }

        private Color GetProgressEndColor(int progress)
        {
            if (progress < 30) return Color.FromArgb(255, 159, 164);
            if (progress < 70) return Color.FromArgb(255, 226, 139);
            if (progress < 100) return Color.FromArgb(129, 210, 210);
            return Color.FromArgb(116, 185, 255);
        }
    }

    // 自定义进度条列类
    public class DataGridViewProgressColumn : DataGridViewColumn
    {
        public DataGridViewProgressColumn() : base(new DataGridViewProgressCell())
        {

        }

        public override DataGridViewCell CellTemplate
        {
            get => base.CellTemplate;
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewProgressCell)))
                {
                    throw new InvalidCastException("必须是DataGridViewProgressCell类型");
                }
                base.CellTemplate = value;
            }
        }
    }
}
