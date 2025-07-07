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
            this.Text = "���۵�DataGridViewʾ��";
        }

        private void InitializeSalaryUpdater()
        {
            random = new Random();

            salaryUpdateTimer = new Timer();
            salaryUpdateTimer.Interval = 500; // ���ӵ�500���룬���ٸ���Ƶ��
            salaryUpdateTimer.Tick += SalaryUpdateTimer_Tick;
            salaryUpdateTimer.Start();
        }

        private void SalaryUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                // ��ʼ���������Լ����ػ����
                collapsibleGrid.BeginBatchUpdate();

                try
                {
                    // �����豸�������
                    foreach (DataRow row in dataTable.Rows)
                    {
                        // ģ���¶ȱ仯
                        decimal currentTemp = Convert.ToDecimal(row["�¶�"]);
                        decimal tempChange = (decimal)(random.NextDouble() * 10 - 5); // -5��+5�ȱ仯
                        decimal newTemp = Math.Max(0, currentTemp + tempChange);
                        row["�¶�"] = Math.Round(newTemp, 1);

                        // ģ��ѹ���仯��ֻ�������е��豸����ѹ����
                        if (row["����״̬"].ToString() == "������")
                        {
                            decimal currentPressure = Convert.ToDecimal(row["ѹ��"]);
                            decimal pressureChange = (decimal)(random.NextDouble() * 2 - 1); // -1��+1�仯
                            decimal newPressure = Math.Max(0, currentPressure + pressureChange);
                            row["ѹ��"] = Math.Round(newPressure, 1);
                        }

                        // ģ������仯
                        decimal currentCurrent = Convert.ToDecimal(row["����"]);
                        decimal currentChange = (decimal)(random.NextDouble() * 6 - 3); // -3��+3�仯
                        decimal newCurrent = Math.Max(0, currentCurrent + currentChange);
                        row["����"] = Math.Round(newCurrent, 1);

                        // ���½�����ʾ
                        int id = Convert.ToInt32(row["ID"]);
                        collapsibleGrid.UpdateCellValueById(id, "�¶�", Math.Round(newTemp, 1));
                        if (row["����״̬"].ToString() == "������")
                        {
                            collapsibleGrid.UpdateCellValueById(id, "ѹ��", Math.Round(Convert.ToDecimal(row["ѹ��"]), 1));
                        }
                        collapsibleGrid.UpdateCellValueById(id, "����", Math.Round(newCurrent, 1));
                    }

                    // ���·����������Ϊ��ǰ���ʱ��
                    var groupNames = collapsibleGrid.GetGroupNames();
                    foreach (string groupName in groupNames)
                    {
                        string timeText = DateTime.Now.ToString("HH:mm:ss");
                        collapsibleGrid.UpdateGroupCustomText(groupName, $"{timeText}");
                    }
                }
                finally
                {
                    // �����������£�ͳһˢ��UI
                    collapsibleGrid.EndBatchUpdate();
                }
            }
        }

        private void LoadSampleData()
        {
            dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("�豸����", typeof(string));
            dataTable.Columns.Add("����", typeof(string));
            dataTable.Columns.Add("�豸����", typeof(string));
            dataTable.Columns.Add("����״̬", typeof(string));
            dataTable.Columns.Add("�¶�", typeof(decimal));
            dataTable.Columns.Add("ѹ��", typeof(decimal));
            dataTable.Columns.Add("����", typeof(decimal));

            // ���������豸
            dataTable.Rows.Add(1, "ע�ܻ�-001", "��������", "ע���豸", "������", 85.2, 12.5, 45.8);
            dataTable.Rows.Add(2, "ע�ܻ�-002", "��������", "ע���豸", "����", 42.1, 0.0, 2.1);
            dataTable.Rows.Add(3, "��ѹ��-001", "��������", "��ѹ�豸", "������", 78.9, 15.2, 52.3);
            dataTable.Rows.Add(4, "װ����-A", "��������", "װ���豸", "������", 25.4, 6.8, 28.7);
            dataTable.Rows.Add(5, "�ʼ�̨-001", "��������", "����豸", "������", 22.1, 0.5, 15.2);

            // �ӹ������豸
            dataTable.Rows.Add(6, "���ػ���-001", "�ӹ�����", "�����豸", "������", 65.8, 8.9, 38.4);
            dataTable.Rows.Add(7, "���ػ���-002", "�ӹ�����", "�����豸", "ά����", 35.2, 0.0, 0.0);
            dataTable.Rows.Add(8, "ĥ��-001", "�ӹ�����", "ĥ���豸", "������", 58.7, 5.6, 32.1);
            dataTable.Rows.Add(9, "����-001", "�ӹ�����", "�����豸", "������", 72.3, 7.2, 41.9);
            dataTable.Rows.Add(10, "ϳ��-001", "�ӹ�����", "ϳ���豸", "����", 28.9, 0.0, 3.5);

            // ��װ�����豸
            dataTable.Rows.Add(11, "��װ��-001", "��װ����", "��װ�豸", "������", 32.4, 4.2, 22.8);
            dataTable.Rows.Add(12, "�����-001", "��װ����", "��װ�豸", "������", 29.7, 3.8, 18.5);
            dataTable.Rows.Add(13, "����-001", "��װ����", "����豸", "����", 45.6, 0.0, 0.0);
            dataTable.Rows.Add(14, "�����-001", "��װ����", "�����豸", "������", 26.8, 2.1, 12.4);

            // ���������豸
            dataTable.Rows.Add(15, "��¯-001", "��������", "�����豸", "������", 285, 18.5, 125.6);
            dataTable.Rows.Add(16, "��ѹ��-001", "��������", "ѹ���豸", "������", 68.9, 8.2, 78.4);
            dataTable.Rows.Add(17, "��ȴ��-001", "��������", "��ȴ�豸", "������", 35.2, 2.5, 45.2);
            dataTable.Rows.Add(18, "��ѹ��-001", "��������", "�����豸", "������", 65.8, 0.0, 185.7);

            collapsibleGrid.SetDataSource(dataTable, "����");
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