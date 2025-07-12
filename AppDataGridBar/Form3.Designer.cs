namespace AppDataGridBar
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            buttonPanel = new Panel();
            btnStart = new Button();
            btnStop = new Button();
            btnReset = new Button();
            btnAddTask = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(800, 400);
            dataGridView1.TabIndex = 0;
            // 
            // buttonPanel
            // 
            buttonPanel.Controls.Add(btnAddTask);
            buttonPanel.Controls.Add(btnReset);
            buttonPanel.Controls.Add(btnStop);
            buttonPanel.Controls.Add(btnStart);
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Height = 60;
            buttonPanel.Location = new Point(0, 390);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new Size(800, 60);
            buttonPanel.TabIndex = 1;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(15, 12);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(100, 35);
            btnStart.TabIndex = 0;
            btnStart.Text = "开始模拟";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += BtnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(125, 12);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(100, 35);
            btnStop.TabIndex = 1;
            btnStop.Text = "停止模拟";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += BtnStop_Click;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(235, 12);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(100, 35);
            btnReset.TabIndex = 2;
            btnReset.Text = "重置进度";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += BtnReset_Click;
            // 
            // btnAddTask
            // 
            btnAddTask.Location = new Point(345, 12);
            btnAddTask.Name = "btnAddTask";
            btnAddTask.Size = new Size(100, 35);
            btnAddTask.TabIndex = 3;
            btnAddTask.Text = "添加任务";
            btnAddTask.UseVisualStyleBackColor = true;
            btnAddTask.Click += BtnAddTask_Click;
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dataGridView1);
            Controls.Add(buttonPanel);
            Name = "Form3";
            Text = "进度条示例 - DataGridView";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            buttonPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private Panel buttonPanel;
        private Button btnStart;
        private Button btnStop;
        private Button btnReset;
        private Button btnAddTask;
    }
}