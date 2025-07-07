namespace AppCollapsibleDataGrid
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
            btnExpandAll = new Button();
            btnLoadData = new Button();
            btnCollapseAll = new Button();
            collapsibleGrid = new CollapsibleDataGridView();
            SuspendLayout();
            // 
            // btnExpandAll
            // 
            btnExpandAll.Location = new Point(94, 12);
            btnExpandAll.Name = "btnExpandAll";
            btnExpandAll.Size = new Size(75, 23);
            btnExpandAll.TabIndex = 0;
            btnExpandAll.Text = "展开全部";
            btnExpandAll.UseVisualStyleBackColor = true;
            btnExpandAll.Click += BtnExpandAll_Click;
            // 
            // btnLoadData
            // 
            btnLoadData.Location = new Point(13, 12);
            btnLoadData.Name = "btnLoadData";
            btnLoadData.Size = new Size(75, 23);
            btnLoadData.TabIndex = 1;
            btnLoadData.Text = "加载数据";
            btnLoadData.UseVisualStyleBackColor = true;
            btnLoadData.Click += BtnLoadData_Click;
            // 
            // btnCollapseAll
            // 
            btnCollapseAll.Location = new Point(175, 12);
            btnCollapseAll.Name = "btnCollapseAll";
            btnCollapseAll.Size = new Size(75, 23);
            btnCollapseAll.TabIndex = 2;
            btnCollapseAll.Text = "折叠全部";
            btnCollapseAll.UseVisualStyleBackColor = true;
            btnCollapseAll.Click += BtnCollapseAll_Click;
            // 
            // collapsibleGrid
            // 
            collapsibleGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            collapsibleGrid.GroupColumn = null;
            collapsibleGrid.Location = new Point(13, 41);
            collapsibleGrid.Name = "collapsibleGrid";
            collapsibleGrid.ShowGroupHeaders = true;
            collapsibleGrid.Size = new Size(775, 397);
            collapsibleGrid.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(collapsibleGrid);
            Controls.Add(btnCollapseAll);
            Controls.Add(btnLoadData);
            Controls.Add(btnExpandAll);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button btnExpandAll;
        private Button btnLoadData;
        private Button btnCollapseAll;
        private CollapsibleDataGridView collapsibleGrid;
    }
}
