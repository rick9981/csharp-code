using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace AppDataGridBar
{
    public partial class Form3 : Form
    {
        private DataTable dataTable;
        private Timer updateTimer;
        private Random random = new Random();

        public Form3()
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
            dataTable.Rows.Add("备份数据", 0, "待开始");

            // 绑定数据源
            dataGridView1.DataSource = dataTable;

            // 设置列属性
            dataGridView1.Columns["任务名称"].Width = 120;
            dataGridView1.Columns["进度"].Width = 250; // 增加宽度以容纳进度条
            dataGridView1.Columns["状态"].Width = 80;

            // 关键：绑定CellPainting事件
            dataGridView1.CellPainting += DataGridView1_CellPainting;

            // 优化显示效果
            dataGridView1.RowTemplate.Height = 30;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // 设置表格样式
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(64, 64, 64);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersHeight = 35;

            // 设置行样式
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 122, 183);
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
        }

        // 关键的绘制方法
        private void DataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // 只处理进度列（第1列，索引为1）
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                // 获取进度值
                int progress = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["进度"].Value);

                // 绘制背景
                e.PaintBackground(e.CellBounds, true);

                // 计算进度条区域
                Rectangle progressRect = e.CellBounds;
                progressRect.Inflate(-3, -3); // 留出边距

                // 绘制进度条背景
                using (SolidBrush backgroundBrush = new SolidBrush(Color.FromArgb(230, 230, 230)))
                {
                    e.Graphics.FillRectangle(backgroundBrush, progressRect);
                }

                // 绘制进度条边框
                using (Pen borderPen = new Pen(Color.FromArgb(180, 180, 180)))
                {
                    e.Graphics.DrawRectangle(borderPen, progressRect);
                }

                // 计算进度条填充区域
                int fillWidth = (int)(progressRect.Width * progress / 100.0);
                Rectangle fillRect = new Rectangle(progressRect.X, progressRect.Y, fillWidth, progressRect.Height);

                // 根据进度选择颜色
                Color progressColor = GetProgressColor(progress);

                // 绘制进度填充
                if (fillWidth > 0)
                {
                    using (LinearGradientBrush fillBrush = new LinearGradientBrush(
                        fillRect,
                        Color.FromArgb(255, progressColor.R, progressColor.G, progressColor.B),
                        Color.FromArgb(200, progressColor.R, progressColor.G, progressColor.B),
                        LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillRectangle(fillBrush, fillRect);
                    }
                }

                // 绘制进度文本
                string progressText = $"{progress}%";
                using (Font font = new Font("Microsoft YaHei", 9, FontStyle.Bold))
                {
                    SizeF textSize = e.Graphics.MeasureString(progressText, font);
                    PointF textPoint = new PointF(
                        progressRect.X + (progressRect.Width - textSize.Width) / 2,
                        progressRect.Y + (progressRect.Height - textSize.Height) / 2
                    );

                    // 根据背景选择文字颜色
                    Color textColor = progress > 50 ? Color.White : Color.Black;
                    using (SolidBrush textBrush = new SolidBrush(textColor))
                    {
                        e.Graphics.DrawString(progressText, font, textBrush, textPoint);
                    }
                }

                e.Handled = true;
            }
        }

        // 根据进度值返回对应的颜色
        private Color GetProgressColor(int progress)
        {
            if (progress == 100)
                return Color.FromArgb(40, 167, 69); // 绿色-完成
            else if (progress >= 70)
                return Color.FromArgb(23, 162, 184); // 蓝色-接近完成
            else if (progress >= 40)
                return Color.FromArgb(255, 193, 7); // 黄色-进行中
            else if (progress > 0)
                return Color.FromArgb(255, 87, 34); // 橙色-刚开始
            else
                return Color.FromArgb(108, 117, 125); // 灰色-未开始
        }

 

        // 按钮事件处理
        private void BtnStart_Click(object sender, EventArgs e)
        {
            StartProgressSimulation();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            StopProgressSimulation();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetProgress();
        }

        private void BtnAddTask_Click(object sender, EventArgs e)
        {
            AddNewTask();
        }

        private void StartProgressSimulation()
        {
            if (updateTimer == null)
            {
                // 创建定时器，每200毫秒更新一次进度
                updateTimer = new Timer();
                updateTimer.Interval = 200;
                updateTimer.Tick += UpdateTimer_Tick;
            }

            if (!updateTimer.Enabled)
            {
                updateTimer.Start();
            }
        }

        private void StopProgressSimulation()
        {
            if (updateTimer != null && updateTimer.Enabled)
            {
                updateTimer.Stop();
            }
        }

        private void ResetProgress()
        {
            StopProgressSimulation();

            // 重置所有进度
            foreach (DataRow row in dataTable.Rows)
            {
                row["进度"] = 0;
                row["状态"] = "待开始";
            }

            dataGridView1.Invalidate();
        }

        private void AddNewTask()
        {
            string taskName = $"新任务{dataTable.Rows.Count + 1}";
            dataTable.Rows.Add(taskName, 0, "待开始");
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            // 模拟进度更新
            bool hasUpdates = false;

            foreach (DataRow row in dataTable.Rows)
            {
                int currentProgress = Convert.ToInt32(row["进度"]);

                // 如果未完成，随机增加进度
                if (currentProgress < 100)
                {
                    int increment = random.Next(1, 8); // 随机增加1-7
                    int newProgress = Math.Min(100, currentProgress + increment);
                    row["进度"] = newProgress;
                    hasUpdates = true;

                    // 更新状态
                    if (newProgress == 100)
                    {
                        row["状态"] = "已完成";
                    }
                    else if (newProgress > 0)
                    {
                        row["状态"] = "进行中";
                    }
                }
            }

            // 只在有更新时刷新
            if (hasUpdates)
            {
                dataGridView1.InvalidateColumn(1); // 只刷新进度列
                dataGridView1.InvalidateColumn(2); // 刷新状态列
            }

            // 检查是否所有任务都完成
            bool allCompleted = true;
            foreach (DataRow row in dataTable.Rows)
            {
                if (Convert.ToInt32(row["进度"]) < 100)
                {
                    allCompleted = false;
                    break;
                }
            }

            if (allCompleted)
            {
                updateTimer.Stop();
                MessageBox.Show("所有任务已完成！", "提示", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        // 手动更新特定行的进度
        public void UpdateProgress(int rowIndex, int newProgress)
        {
            if (rowIndex >= 0 && rowIndex < dataTable.Rows.Count)
            {
                int clampedProgress = Math.Max(0, Math.Min(100, newProgress));
                dataTable.Rows[rowIndex]["进度"] = clampedProgress;

                // 更新状态
                if (clampedProgress == 100)
                    dataTable.Rows[rowIndex]["状态"] = "已完成";
                else if (clampedProgress > 0)
                    dataTable.Rows[rowIndex]["状态"] = "进行中";
                else
                    dataTable.Rows[rowIndex]["状态"] = "待开始";

                // 只刷新指定行
                dataGridView1.InvalidateRow(rowIndex);
            }
        }

        // 批量更新进度
        public void UpdateMultipleProgress(Dictionary<int, int> progressUpdates)
        {
            foreach (var update in progressUpdates)
            {
                UpdateProgress(update.Key, update.Value);
            }
        }

        // 获取所有任务的进度信息
        public List<TaskProgress> GetAllProgress()
        {
            List<TaskProgress> progressList = new List<TaskProgress>();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                progressList.Add(new TaskProgress
                {
                    TaskName = row["任务名称"].ToString(),
                    Progress = Convert.ToInt32(row["进度"]),
                    Status = row["状态"].ToString(),
                    RowIndex = i
                });
            }

            return progressList;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopProgressSimulation();
            base.OnFormClosing(e);
        }
    }

    // 任务进度信息类
    public class TaskProgress
    {
        public string TaskName { get; set; }
        public int Progress { get; set; }
        public string Status { get; set; }
        public int RowIndex { get; set; }
    }
}