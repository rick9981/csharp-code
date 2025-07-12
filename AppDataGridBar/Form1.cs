using System.Data;
using System.Windows.Forms;

namespace AppDataGridBar
{
    public partial class Form1 : Form
    {
        private DataTable dataTable;
        public Form1()
        {
            InitializeComponent();
            InitializeDataGridView();
        }
        private void InitializeDataGridView()
        {
            // 创建数据源
            dataTable = new DataTable();
            dataTable.Columns.Add("任务名称", typeof(string));
            dataTable.Columns.Add("进度", typeof(int)); // 存储0-100的进度值
            dataTable.Columns.Add("状态", typeof(string));

            // 添加示例数据
            dataTable.Rows.Add("文件下载", 75, "进行中");
            dataTable.Rows.Add("数据同步", 45, "进行中");
            dataTable.Rows.Add("报表生成", 100, "已完成");
            dataTable.Rows.Add("邮件发送", 30, "进行中");

            // 绑定数据源
            dataGridView1.DataSource = dataTable;

            // 设置列属性
            dataGridView1.Columns["任务名称"].Width = 120;
            dataGridView1.Columns["进度"].Width = 200;
            dataGridView1.Columns["状态"].Width = 80;

            // 关键：绑定CellPainting事件
            dataGridView1.CellPainting += DataGridView1_CellPainting;

            // 优化显示效果
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void DataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // 只对"进度"列进行自定义绘制
            if (e.ColumnIndex == 1 && e.RowIndex >= 0) // 进度列的索引为1
            {
                // 获取进度值
                if (int.TryParse(e.Value?.ToString(), out int progressValue))
                {
                    // 确保进度值在0-100范围内
                    progressValue = Math.Max(0, Math.Min(100, progressValue));

                    // 绘制背景
                    e.PaintBackground(e.CellBounds, true);

                    // 计算进度条区域（留出边距）
                    Rectangle progressRect = new Rectangle(
                        e.CellBounds.X + 2,
                        e.CellBounds.Y + 2,
                        e.CellBounds.Width - 4,
                        e.CellBounds.Height - 4
                    );

                    // 绘制进度条背景（灰色）
                    using (Brush backgroundBrush = new SolidBrush(Color.LightGray))
                    {
                        e.Graphics.FillRectangle(backgroundBrush, progressRect);
                    }

                    // 计算填充宽度
                    int fillWidth = (int)(progressRect.Width * (progressValue / 100.0));

                    if (fillWidth > 0)
                    {
                        Rectangle fillRect = new Rectangle(
                            progressRect.X,
                            progressRect.Y,
                            fillWidth,
                            progressRect.Height
                        );

                        // 根据进度值选择颜色
                        Color fillColor = GetProgressColor(progressValue);

                        using (Brush fillBrush = new SolidBrush(fillColor))
                        {
                            e.Graphics.FillRectangle(fillBrush, fillRect);
                        }
                    }

                    // 绘制文字（进度百分比）
                    string text = $"{progressValue}%";
                    using (Brush textBrush = new SolidBrush(Color.Black))
                    {
                        StringFormat sf = new StringFormat();
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;

                        e.Graphics.DrawString(text, e.CellStyle.Font, textBrush,
                            e.CellBounds, sf);
                    }

                    // 绘制边框
                    using (Pen borderPen = new Pen(Color.Gray))
                    {
                        e.Graphics.DrawRectangle(borderPen, progressRect);
                    }

                    // 标记为已处理，避免默认绘制
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// 根据进度值返回对应颜色
        /// </summary>
        private Color GetProgressColor(int progress)
        {
            if (progress < 30)
                return Color.FromArgb(220, 53, 69); // 红色
            else if (progress < 70)
                return Color.FromArgb(255, 193, 7);  // 黄色
            else if (progress < 100)
                return Color.FromArgb(40, 167, 69);  // 绿色
            else
                return Color.FromArgb(23, 162, 184); // 蓝色（完成）
        }
    }
}
