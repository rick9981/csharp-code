using System.Data;
using System.Reflection;
using Timer = System.Windows.Forms.Timer;

namespace AppCollapsibleDataGrid
{
    public partial class Form1 : Form
    {
        private DataTable dataTable;
        private Timer salaryUpdateTimer;
        private Random random;

        public Form1()
        {
            InitializeComponent();
            SetupControls();
            InitializeSalaryUpdater();
            LoadSampleData();
        }

        private void SetupControls()
        {
            this.Size = new Size(800, 600);
            this.Text = "可折叠DataGridView示例";
        }

        private void InitializeSalaryUpdater()
        {
            random = new Random();

            salaryUpdateTimer = new Timer();
            salaryUpdateTimer.Interval = 500; // 增加到500毫秒，减少更新频率
            salaryUpdateTimer.Tick += SalaryUpdateTimer_Tick;
            salaryUpdateTimer.Start();
        }

        private void SalaryUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                // 开始批量更新以减少重绘次数
                collapsibleGrid.BeginBatchUpdate();

                try
                {
                    // 更新设备监控数据
                    foreach (DataRow row in dataTable.Rows)
                    {
                        // 模拟温度变化
                        decimal currentTemp = Convert.ToDecimal(row["温度"]);
                        decimal tempChange = (decimal)(random.NextDouble() * 10 - 5); // -5到+5度变化
                        decimal newTemp = Math.Max(0, currentTemp + tempChange);
                        row["温度"] = Math.Round(newTemp, 1);

                        // 模拟压力变化（只有运行中的设备才有压力）
                        if (row["运行状态"].ToString() == "运行中")
                        {
                            decimal currentPressure = Convert.ToDecimal(row["压力"]);
                            decimal pressureChange = (decimal)(random.NextDouble() * 2 - 1); // -1到+1变化
                            decimal newPressure = Math.Max(0, currentPressure + pressureChange);
                            row["压力"] = Math.Round(newPressure, 1);
                        }

                        // 模拟电流变化
                        decimal currentCurrent = Convert.ToDecimal(row["电流"]);
                        decimal currentChange = (decimal)(random.NextDouble() * 6 - 3); // -3到+3变化
                        decimal newCurrent = Math.Max(0, currentCurrent + currentChange);
                        row["电流"] = Math.Round(newCurrent, 1);

                        // 更新界面显示
                        int id = Convert.ToInt32(row["ID"]);
                        collapsibleGrid.UpdateCellValueById(id, "温度", Math.Round(newTemp, 1));
                        if (row["运行状态"].ToString() == "运行中")
                        {
                            collapsibleGrid.UpdateCellValueById(id, "压力", Math.Round(Convert.ToDecimal(row["压力"]), 1));
                        }
                        collapsibleGrid.UpdateCellValueById(id, "电流", Math.Round(newCurrent, 1));
                    }

                    // 更新分组汇总文字为当前监控时间
                    var groupNames = collapsibleGrid.GetGroupNames();
                    foreach (string groupName in groupNames)
                    {
                        string timeText = DateTime.Now.ToString("HH:mm:ss");
                        collapsibleGrid.UpdateGroupCustomText(groupName, $"{timeText}");
                    }
                }
                finally
                {
                    // 结束批量更新，统一刷新UI
                    collapsibleGrid.EndBatchUpdate();
                }
            }
        }

        private void LoadSampleData()
        {
            dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("设备名称", typeof(string));
            dataTable.Columns.Add("车间", typeof(string));
            dataTable.Columns.Add("设备类型", typeof(string));
            dataTable.Columns.Add("运行状态", typeof(string));
            dataTable.Columns.Add("温度", typeof(decimal));
            dataTable.Columns.Add("压力", typeof(decimal));
            dataTable.Columns.Add("电流", typeof(decimal));

            // 生产车间设备
            dataTable.Rows.Add(1, "注塑机-001", "生产车间", "注塑设备", "运行中", 85.2, 12.5, 45.8);
            dataTable.Rows.Add(2, "注塑机-002", "生产车间", "注塑设备", "待机", 42.1, 0.0, 2.1);
            dataTable.Rows.Add(3, "冲压机-001", "生产车间", "冲压设备", "运行中", 78.9, 15.2, 52.3);
            dataTable.Rows.Add(4, "装配线-A", "生产车间", "装配设备", "运行中", 25.4, 6.8, 28.7);
            dataTable.Rows.Add(5, "质检台-001", "生产车间", "检测设备", "运行中", 22.1, 0.5, 15.2);

            // 加工车间设备
            dataTable.Rows.Add(6, "数控机床-001", "加工车间", "机床设备", "运行中", 65.8, 8.9, 38.4);
            dataTable.Rows.Add(7, "数控机床-002", "加工车间", "机床设备", "维护中", 35.2, 0.0, 0.0);
            dataTable.Rows.Add(8, "磨床-001", "加工车间", "磨削设备", "运行中", 58.7, 5.6, 32.1);
            dataTable.Rows.Add(9, "车床-001", "加工车间", "车削设备", "运行中", 72.3, 7.2, 41.9);
            dataTable.Rows.Add(10, "铣床-001", "加工车间", "铣削设备", "待机", 28.9, 0.0, 3.5);

            // 包装车间设备
            dataTable.Rows.Add(11, "包装机-001", "包装车间", "包装设备", "运行中", 32.4, 4.2, 22.8);
            dataTable.Rows.Add(12, "封箱机-001", "包装车间", "封装设备", "运行中", 29.7, 3.8, 18.5);
            dataTable.Rows.Add(13, "码垛机-001", "包装车间", "码垛设备", "故障", 45.6, 0.0, 0.0);
            dataTable.Rows.Add(14, "贴标机-001", "包装车间", "贴标设备", "运行中", 26.8, 2.1, 12.4);

            // 动力车间设备
            dataTable.Rows.Add(15, "锅炉-001", "动力车间", "供热设备", "运行中", 285, 18.5, 125.6);
            dataTable.Rows.Add(16, "空压机-001", "动力车间", "压缩设备", "运行中", 68.9, 8.2, 78.4);
            dataTable.Rows.Add(17, "冷却塔-001", "动力车间", "冷却设备", "运行中", 35.2, 2.5, 45.2);
            dataTable.Rows.Add(18, "变压器-001", "动力车间", "电力设备", "运行中", 65.8, 0.0, 185.7);

            collapsibleGrid.SetDataSource(dataTable, "车间");
        }

        private void BtnLoadData_Click(object sender, EventArgs e)
        {
            LoadSampleData();
        }

        private void BtnExpandAll_Click(object sender, EventArgs e)
        {
            collapsibleGrid.ExpandAll();
        }

        private void BtnCollapseAll_Click(object sender, EventArgs e)
        {
            collapsibleGrid.CollapseAll();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            salaryUpdateTimer?.Stop();
            salaryUpdateTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}