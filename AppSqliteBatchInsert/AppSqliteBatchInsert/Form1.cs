namespace AppSqliteBatchInsert
{
    public partial class Form1 : Form
    {
        private readonly string _dbFilePath = "industrial_data.db";
        public Form1()
        {
            InitializeComponent();
            progressBarInsert.Style = ProgressBarStyle.Continuous;
            numericUpDownCount.Value = 10000; // 默认插入数量（可调整）
        }
        private async void btnCreateDb_Click(object sender, EventArgs e)
        {
            try
            {
                DbHelper.CreateDatabaseIfNotExists(_dbFilePath);
                DbHelper.CreateSampleTable(_dbFilePath);
                MessageBox.Show("数据库与表创建完成。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"创建数据库/表失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnInsert_Click(object sender, EventArgs e)
        {
            btnInsert.Enabled = false;
            btnCreateDb.Enabled = false;
            int total = (int)numericUpDownCount.Value;
            int batchSize = (int)numericUpDownBatch.Value;
            progressBarInsert.Value = 0;
            progressBarInsert.Maximum = total;
            lblStatus.Text = "准备插入...";

            try
            {
                // 在一个线程中执行批量插入以防止阻塞UI
                await Task.Run(() =>
                {
                    DbHelper.BulkInsert(_dbFilePath, total, batchSize, progress =>
                    {
                        // 由于从后台线程调用 UI，需要 Invoke
                        this.Invoke((Action)(() =>
                        {
                            progressBarInsert.Value = progress;
                            lblStatus.Text = $"已插入 {progress}/{total}";
                        }));
                    });
                });

                MessageBox.Show("批量插入完成。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"批量插入失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnInsert.Enabled = true;
                btnCreateDb.Enabled = true;
                lblStatus.Text = "就绪";
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                DbHelper.TruncateTable(_dbFilePath);
                MessageBox.Show("表已清空。", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"清空表失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
