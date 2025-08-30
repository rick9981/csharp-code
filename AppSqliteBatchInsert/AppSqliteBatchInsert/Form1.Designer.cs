namespace AppSqliteBatchInsert
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelTop = new Panel();
            lblTitle = new Label();
            btnCreateDb = new Button();
            btnClear = new Button();
            groupBoxControls = new GroupBox();
            label1 = new Label();
            numericUpDownCount = new NumericUpDown();
            label2 = new Label();
            numericUpDownBatch = new NumericUpDown();
            btnInsert = new Button();
            progressBarInsert = new ProgressBar();
            lblStatus = new Label();
            panelTop.SuspendLayout();
            groupBoxControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownCount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownBatch).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(33, 150, 243);
            panelTop.Controls.Add(lblTitle);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(524, 64);
            panelTop.TabIndex = 5;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(16, 16);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(230, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "SQLite 批量插入演示";
            // 
            // btnCreateDb
            // 
            btnCreateDb.BackColor = Color.FromArgb(76, 175, 80);
            btnCreateDb.FlatStyle = FlatStyle.Flat;
            btnCreateDb.ForeColor = Color.White;
            btnCreateDb.Location = new Point(20, 88);
            btnCreateDb.Name = "btnCreateDb";
            btnCreateDb.Size = new Size(140, 38);
            btnCreateDb.TabIndex = 4;
            btnCreateDb.Text = "创建数据库/表";
            btnCreateDb.UseVisualStyleBackColor = false;
            btnCreateDb.Click += btnCreateDb_Click;
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.FromArgb(244, 67, 54);
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.ForeColor = Color.White;
            btnClear.Location = new Point(176, 88);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(140, 38);
            btnClear.TabIndex = 3;
            btnClear.Text = "清空表（慎用）";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += btnClear_Click;
            // 
            // groupBoxControls
            // 
            groupBoxControls.Controls.Add(label1);
            groupBoxControls.Controls.Add(numericUpDownCount);
            groupBoxControls.Controls.Add(label2);
            groupBoxControls.Controls.Add(numericUpDownBatch);
            groupBoxControls.Controls.Add(btnInsert);
            groupBoxControls.Location = new Point(20, 140);
            groupBoxControls.Name = "groupBoxControls";
            groupBoxControls.Size = new Size(480, 120);
            groupBoxControls.TabIndex = 2;
            groupBoxControls.TabStop = false;
            groupBoxControls.Text = "批量插入设置";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 28);
            label1.Name = "label1";
            label1.Size = new Size(72, 15);
            label1.TabIndex = 0;
            label1.Text = "插入数量：";
            // 
            // numericUpDownCount
            // 
            numericUpDownCount.Location = new Point(119, 24);
            numericUpDownCount.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numericUpDownCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownCount.Name = "numericUpDownCount";
            numericUpDownCount.Size = new Size(120, 23);
            numericUpDownCount.TabIndex = 1;
            numericUpDownCount.Value = new decimal(new int[] { 10000, 0, 0, 0 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 64);
            label2.Name = "label2";
            label2.Size = new Size(98, 15);
            label2.TabIndex = 2;
            label2.Text = "事务批次大小：";
            // 
            // numericUpDownBatch
            // 
            numericUpDownBatch.Location = new Point(119, 60);
            numericUpDownBatch.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numericUpDownBatch.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownBatch.Name = "numericUpDownBatch";
            numericUpDownBatch.Size = new Size(120, 23);
            numericUpDownBatch.TabIndex = 3;
            numericUpDownBatch.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // btnInsert
            // 
            btnInsert.BackColor = Color.FromArgb(33, 150, 243);
            btnInsert.FlatStyle = FlatStyle.Flat;
            btnInsert.ForeColor = Color.White;
            btnInsert.Location = new Point(320, 36);
            btnInsert.Name = "btnInsert";
            btnInsert.Size = new Size(140, 48);
            btnInsert.TabIndex = 4;
            btnInsert.Text = "开始批量插入";
            btnInsert.UseVisualStyleBackColor = false;
            btnInsert.Click += btnInsert_Click;
            // 
            // progressBarInsert
            // 
            progressBarInsert.Location = new Point(20, 280);
            progressBarInsert.Name = "progressBarInsert";
            progressBarInsert.Size = new Size(480, 24);
            progressBarInsert.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(20, 312);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(33, 15);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "就绪";
            // 
            // Form1
            // 
            ClientSize = new Size(524, 360);
            Controls.Add(lblStatus);
            Controls.Add(progressBarInsert);
            Controls.Add(groupBoxControls);
            Controls.Add(btnClear);
            Controls.Add(btnCreateDb);
            Controls.Add(panelTop);
            Font = new Font("Segoe UI", 9F);
            Name = "Form1";
            Text = "SQLite 批量插入优化示例";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            groupBoxControls.ResumeLayout(false);
            groupBoxControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownCount).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownBatch).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnCreateDb;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox groupBoxControls;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownBatch;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.ProgressBar progressBarInsert;
        private System.Windows.Forms.Label lblStatus;
    }
}
