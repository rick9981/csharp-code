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
            // �����ǰ��ͼ
            formsPlot1.Plot.Clear();

            // ����һ����ʱ���������ͬ���ȵ� double ����
            double[] dateDoubles = new double[index];
            for (int i = 0; i < index; i++)
            {
                // ��ʱ���ת��Ϊ OLE �Զ����ڣ����ڻ�ͼ��
                dateDoubles[i] = timestamps[i].ToOADate();
            }

            // �Զ���������ʽ
            // ���ɢ��ͼ����ͼ�У�ʹ��ʱ��Ͷ�Ӧ��ֵ
            var sp = formsPlot1.Plot.Add.Scatter(dateDoubles, values[0..index]);
            sp.LineWidth = 3; // �����������Ϊ3���Ӵ֣�
            sp.MarkerSize = 4; // ���ñ�ǵ�Ĵ�С
            sp.MarkerShape = MarkerShape.FilledCircle; // ���ñ�ǵ���״Ϊʵ��Բ
            sp.Color = ScottPlot.Color.FromHtml("#007BFF"); // ����������ɫΪ��ɫ��#007BFF��

            // ���һ��ˮƽ�ο���
            var horizontalLine = formsPlot1.Plot.Add.HorizontalLine(50, color: ScottPlot.Color.FromHtml("#FF0000"));
            horizontalLine.Text = "50% threshold"; // ���òο��ߵ��ı�
            horizontalLine.LabelFontColor = ScottPlot.Color.FromHtml("#FF0000"); // �����ı���ɫΪ��ɫ
            horizontalLine.LabelBackgroundColor = ScottPlot.Color.FromHtml("#FFFFFF"); // �����ı�����Ϊ��ɫ

            // ���û�ͼ�ı�������ǩ����������ʽ
            formsPlot1.Plot.Title("ʵʱ�������"); // ���ñ���
            formsPlot1.Plot.XLabel("ʱ�� / seconds"); // ���� X ���ǩ
            formsPlot1.Plot.YLabel("���ֵ / value"); // ���� Y ���ǩ

            // �����������ߵ���ɫ�Ϳ��
            formsPlot1.Plot.Grid.MajorLineColor = Colors.Green.WithOpacity(.3); // ����������ɫ����ɫ��
            formsPlot1.Plot.Grid.MajorLineWidth = 2; // �������߿��

            // ���ô������ߵ���ɫ�Ϳ��
            formsPlot1.Plot.Grid.MinorLineColor = Colors.Gray.WithOpacity(.1); // ����������ɫ����ɫ��
            formsPlot1.Plot.Grid.MinorLineWidth = 1; // �������߿��

            // �Ե�ǰ�Ļ�ͼ������������
            ScottPlot.Plot myPlot = formsPlot1.Plot;
            myPlot.Font.Set("SimSun"); // ��������Ϊ SimSun �����壩

            // �� X ������Ϊʱ���ʽ�Ŀ̶�
            myPlot.Axes.DateTimeTicksBottom();

            // ˢ�»�ͼ�Ը�����ʾ
            formsPlot1.Refresh();
        }


        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            sqlConnection.Close();
            base.OnFormClosed(e);
        }
    }
}