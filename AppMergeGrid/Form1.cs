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
            this.Text = "可合并单元格的DataGridView";
            this.StartPosition = FormStartPosition.CenterScreen;

        }

        private void LoadSampleData()
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("产品类别", typeof(string));
            dt.Columns.Add("产品名称", typeof(string));
            dt.Columns.Add("数量", typeof(int));
            dt.Columns.Add("单价", typeof(decimal));
            dt.Columns.Add("总价", typeof(decimal));

            dt.Rows.Add("电子产品", "笔记本电脑", 10, 5000, 50000);
            dt.Rows.Add("电子产品", "台式电脑", 5, 3000, 15000);
            dt.Rows.Add("电子产品", "显示器", 20, 1000, 20000);
            dt.Rows.Add("办公用品", "打印机", 3, 2000, 6000);
            dt.Rows.Add("办公用品", "复印机", 2, 8000, 16000);
            dt.Rows.Add("家具", "办公桌", 15, 800, 12000);
            dt.Rows.Add("家具", "办公桌", 25, 800, 12000);
            dt.Rows.Add("家具", "办公椅", 20, 500, 10000);

            mergeableDataGridView1.DataSource = dt;

            // 设置列宽
            mergeableDataGridView1.Columns[0].Width = 100;
            mergeableDataGridView1.Columns[1].Width = 150;
            mergeableDataGridView1.Columns[2].Width = 80;
            mergeableDataGridView1.Columns[3].Width = 100;
            mergeableDataGridView1.Columns[4].Width = 100;

            MessageBox.Show("数据加载完成！点击'自动合并相同项'按钮查看效果。");
        }

        private void AutoMergeSameValues()
        {
            mergeableDataGridView1.ClearAllMerges();
            mergeableDataGridView1.AutoMergeColumn(0); // 合并第一列
            mergeableDataGridView1.AutoMergeColumn(1, StringAlignment.Near, StringAlignment.Center); // 合并第二列
            MessageBox.Show("已自动合并产品类别列的相同项！");
        }

        private void btnClearMerge_Click(object sender, EventArgs e)
        {
            mergeableDataGridView1.ClearAllMerges();
            MessageBox.Show("已清除所有合并！");
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
