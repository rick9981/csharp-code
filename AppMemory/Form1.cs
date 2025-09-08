using Microsoft.Data.SqlClient;
using OpenTK.Graphics.OpenGL;
using ScottPlot;
using System;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace AppMemory
{
    public partial class Form1 : Form
    {
        private DateTime[] timestamps = new DateTime[100]; 
        private double[] values = new double[100]; 
        private int index = 0; 
        private SqlConnection sqlConnection;

        public Form1()
        {
            InitializeComponent();
            InitializeDatabase();

            StartTimer();
        }

        private void InitializeDatabase()
        {
            string connectionString = "Server=localhost;Database=dbtest;User Id=sa;Password=123;TrustServerCertificate=True;";
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }

        private void StartTimer()
        {
            Timer timer = new Timer();
            timer.Interval = 1000; // Update every second
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            double newValue = GetNewValue();
            InsertDataIntoDatabase(newValue);
            GetDataFromDatabase();
            UpdatePlot();
        }

        private double GetNewValue()
        {
            return new Random().NextDouble() * 100; // Simulated random value
        }

        private void InsertDataIntoDatabase(double value)
        {
            using (SqlCommand cmd = new SqlCommand("INSERT INTO dbo.MonitorData (Timestamp, Value) VALUES (@Timestamp, @Value);", sqlConnection))
            {
                cmd.Parameters.AddWithValue("@Timestamp", DateTime.Now);
                cmd.Parameters.AddWithValue("@Value", value);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database insertion error: " + ex.Message);
                }
            }
        }

        private void GetDataFromDatabase()
        {
            using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Timestamp, Value FROM dbo.MonitorData ORDER BY Timestamp DESC;", sqlConnection))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    DateTime timestamp = reader.GetDateTime(0);
                    double value = (double)(decimal)reader.GetDecimal(1);

                    // Store timestamp directly instead of the elapsed time
                    timestamps[index] = timestamp;
                    values[index] = value;

                    index++;
                    if (index >= timestamps.Length)
                    {
                        index = 0; // Reset index
                    }
                }
                reader.Close();
            }
        }

        private void UpdatePlot()
        {
            // 清除当前绘图
            formsPlot1.Plot.Clear();

            // 创建一个与时间戳数组相同长度的 double 数组
            double[] dateDoubles = new double[index];
            for (int i = 0; i < index; i++)
            {
                // 将时间戳转换为 OLE 自动日期（用于绘图）
                dateDoubles[i] = timestamps[i].ToOADate();
            }

            // 自定义折线样式
            // 添加散点图到绘图中，使用时间和对应的值
            var sp = formsPlot1.Plot.Add.Scatter(dateDoubles, values[0..index]);
            sp.LineWidth = 3; // 设置线条宽度为3（加粗）
            sp.MarkerSize = 4; // 设置标记点的大小
            sp.MarkerShape = MarkerShape.FilledCircle; // 设置标记点形状为实心圆
            sp.Color = ScottPlot.Color.FromHtml("#007BFF"); // 设置线条颜色为蓝色（#007BFF）

            // 添加一条水平参考线
            var horizontalLine = formsPlot1.Plot.Add.HorizontalLine(50, color: ScottPlot.Color.FromHtml("#FF0000"));
            horizontalLine.Text = "50% threshold"; // 设置参考线的文本
            horizontalLine.LabelFontColor = ScottPlot.Color.FromHtml("#FF0000"); // 设置文本颜色为红色
            horizontalLine.LabelBackgroundColor = ScottPlot.Color.FromHtml("#FFFFFF"); // 设置文本背景为白色

            // 设置绘图的标题和轴标签，并调整样式
            formsPlot1.Plot.Title("实时监控数据"); // 设置标题
            formsPlot1.Plot.XLabel("时间 / seconds"); // 设置 X 轴标签
            formsPlot1.Plot.YLabel("监测值 / value"); // 设置 Y 轴标签

            // 设置主网格线的颜色和宽度
            formsPlot1.Plot.Grid.MajorLineColor = Colors.Green.WithOpacity(.3); // 主网格线颜色（绿色）
            formsPlot1.Plot.Grid.MajorLineWidth = 2; // 主网格线宽度

            // 设置次网格线的颜色和宽度
            formsPlot1.Plot.Grid.MinorLineColor = Colors.Gray.WithOpacity(.1); // 次网格线颜色（灰色）
            formsPlot1.Plot.Grid.MinorLineWidth = 1; // 次网格线宽度

            // 对当前的绘图对象设置字体
            ScottPlot.Plot myPlot = formsPlot1.Plot;
            myPlot.Font.Set("SimSun"); // 设置字体为 SimSun （宋体）

            // 将 X 轴设置为时间格式的刻度
            myPlot.Axes.DateTimeTicksBottom();

            // 刷新绘图以更新显示
            formsPlot1.Refresh();
        }


        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            sqlConnection.Close();
            base.OnFormClosed(e);
        }
    }
}