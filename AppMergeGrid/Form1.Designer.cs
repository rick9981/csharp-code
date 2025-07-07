namespace AppMergeGrid
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
            btnLoadData = new Button();
            btnAutoMerge = new Button();
            btnClearMerge = new Button();
            mergeableDataGridView1 = new MergeableDataGridView();
            ((System.ComponentModel.ISupportInitialize)mergeableDataGridView1).BeginInit();
            SuspendLayout();
            // 
            // btnLoadData
            // 
            btnLoadData.Location = new Point(12, 403);
            btnLoadData.Name = "btnLoadData";
            btnLoadData.Size = new Size(75, 23);
            btnLoadData.TabIndex = 0;
            btnLoadData.Text = "加载示例数据";
            btnLoadData.UseVisualStyleBackColor = true;
            btnLoadData.Click += btnLoadData_Click;
            // 
            // btnAutoMerge
            // 
            btnAutoMerge.Location = new Point(102, 403);
            btnAutoMerge.Name = "btnAutoMerge";
            btnAutoMerge.Size = new Size(75, 23);
            btnAutoMerge.TabIndex = 1;
            btnAutoMerge.Text = "自动合并相同项";
            btnAutoMerge.UseVisualStyleBackColor = true;
            btnAutoMerge.Click += btnAutoMerge_Click;
            // 
            // btnClearMerge
            // 
            btnClearMerge.Location = new Point(195, 403);
            btnClearMerge.Name = "btnClearMerge";
            btnClearMerge.Size = new Size(75, 23);
            btnClearMerge.TabIndex = 2;
            btnClearMerge.Text = "清除所有合并";
            btnClearMerge.UseVisualStyleBackColor = true;
            btnClearMerge.Click += btnClearMerge_Click;
            // 
            // mergeableDataGridView1
            // 
            mergeableDataGridView1.AutoRefreshMergeOnSort = true;
            mergeableDataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            mergeableDataGridView1.Location = new Point(12, 12);
            mergeableDataGridView1.Name = "mergeableDataGridView1";
            mergeableDataGridView1.Size = new Size(776, 376);
            mergeableDataGridView1.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(mergeableDataGridView1);
            Controls.Add(btnClearMerge);
            Controls.Add(btnAutoMerge);
            Controls.Add(btnLoadData);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)mergeableDataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnLoadData;
        private Button btnAutoMerge;
        private Button btnClearMerge;
        private MergeableDataGridView mergeableDataGridView1;
    }
}
