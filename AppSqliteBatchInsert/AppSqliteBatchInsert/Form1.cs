namespace AppSqliteBatchInsert
{
    public partial class Form1 : Form
    {
        private readonly string _dbFilePath = "industrial_data.db";
        public Form1()
        {
            InitializeComponent();
            progressBarInsert.Style = ProgressBarStyle.Continuous;
            numericUpDownCount.Value = 10000; // Ĭ�ϲ����������ɵ�����
        }
        private async void btnCreateDb_Click(object sender, EventArgs e)
        {
            try
            {
                DbHelper.CreateDatabaseIfNotExists(_dbFilePath);
                DbHelper.CreateSampleTable(_dbFilePath);
                MessageBox.Show("���ݿ��������ɡ�", "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�������ݿ�/��ʧ�ܣ�{ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            lblStatus.Text = "׼������...";

            try
            {
                // ��һ���߳���ִ�����������Է�ֹ����UI
                await Task.Run(() =>
                {
                    DbHelper.BulkInsert(_dbFilePath, total, batchSize, progress =>
                    {
                        // ���ڴӺ�̨�̵߳��� UI����Ҫ Invoke
                        this.Invoke((Action)(() =>
                        {
                            progressBarInsert.Value = progress;
                            lblStatus.Text = $"�Ѳ��� {progress}/{total}";
                        }));
                    });
                });

                MessageBox.Show("����������ɡ�", "�ɹ�", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��������ʧ�ܣ�{ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnInsert.Enabled = true;
                btnCreateDb.Enabled = true;
                lblStatus.Text = "����";
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                DbHelper.TruncateTable(_dbFilePath);
                MessageBox.Show("������ա�", "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��ձ�ʧ�ܣ�{ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
