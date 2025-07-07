using System.Data;
using System.Windows.Forms;

namespace AppMergeGrid
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(800, 600);
            this.Text = "�ɺϲ���Ԫ���DataGridView";
            this.StartPosition = FormStartPosition.CenterScreen;

        }

        private void LoadSampleData()
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("��Ʒ���", typeof(string));
            dt.Columns.Add("��Ʒ����", typeof(string));
            dt.Columns.Add("����", typeof(int));
            dt.Columns.Add("����", typeof(decimal));
            dt.Columns.Add("�ܼ�", typeof(decimal));

            dt.Rows.Add("���Ӳ�Ʒ", "�ʼǱ�����", 10, 5000, 50000);
            dt.Rows.Add("���Ӳ�Ʒ", "̨ʽ����", 5, 3000, 15000);
            dt.Rows.Add("���Ӳ�Ʒ", "��ʾ��", 20, 1000, 20000);
            dt.Rows.Add("�칫��Ʒ", "��ӡ��", 3, 2000, 6000);
            dt.Rows.Add("�칫��Ʒ", "��ӡ��", 2, 8000, 16000);
            dt.Rows.Add("�Ҿ�", "�칫��", 15, 800, 12000);
            dt.Rows.Add("�Ҿ�", "�칫��", 25, 800, 12000);
            dt.Rows.Add("�Ҿ�", "�칫��", 20, 500, 10000);

            mergeableDataGridView1.DataSource = dt;

            // �����п�
            mergeableDataGridView1.Columns[0].Width = 100;
            mergeableDataGridView1.Columns[1].Width = 150;
            mergeableDataGridView1.Columns[2].Width = 80;
            mergeableDataGridView1.Columns[3].Width = 100;
            mergeableDataGridView1.Columns[4].Width = 100;

            MessageBox.Show("���ݼ�����ɣ����'�Զ��ϲ���ͬ��'��ť�鿴Ч����");
        }

        private void AutoMergeSameValues()
        {
            mergeableDataGridView1.ClearAllMerges();
            mergeableDataGridView1.AutoMergeColumn(0); // �ϲ���һ��
            mergeableDataGridView1.AutoMergeColumn(1, StringAlignment.Near, StringAlignment.Center); // �ϲ��ڶ���
            MessageBox.Show("���Զ��ϲ���Ʒ����е���ͬ�");
        }

        private void btnClearMerge_Click(object sender, EventArgs e)
        {
            mergeableDataGridView1.ClearAllMerges();
            MessageBox.Show("��������кϲ���");
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            LoadSampleData();
        }

        private void btnAutoMerge_Click(object sender, EventArgs e)
        {
            AutoMergeSameValues();
        }
    }
}
