namespace AppSqlServerSynTool
{
    partial class FrmMain
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
            groupBoxSource = new GroupBox();
            btnTestSourceConnection = new Button();
            txtSourceConnection = new TextBox();
            labelSourceServer = new Label();
            groupBoxTarget = new GroupBox();
            groupBoxTables = new GroupBox();
            btnSelectNone = new Button();
            btnSelectAll = new Button();
            btnRefreshTables = new Button();
            checkedListBoxTables = new CheckedListBox();
            groupBoxOptions = new GroupBox();
            labelBatchSize = new Label();
            numericBatchSize = new NumericUpDown();
            chkCreateTargetTables = new CheckBox();
            chkTruncateTarget = new CheckBox();
            chkSyncData = new CheckBox();
            chkSyncStructure = new CheckBox();
            groupBoxProgress = new GroupBox();
            richTextBoxLog = new RichTextBox();
            labelProgress = new Label();
            progressBarMain = new ProgressBar();
            btnStartSync = new Button();
            btnCancel = new Button();
            btnExit = new Button();
            btnSaveConfig = new Button();
            btnLoadConfig = new Button();
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            toolStripProgressBar = new ToolStripProgressBar();
            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            saveConfigToolStripMenuItem = new ToolStripMenuItem();
            loadConfigToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            btnTestTargetConnection = new Button();
            txtTargetConnection = new TextBox();
            label1 = new Label();
            groupBoxSource.SuspendLayout();
            groupBoxTarget.SuspendLayout();
            groupBoxTables.SuspendLayout();
            groupBoxOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericBatchSize).BeginInit();
            groupBoxProgress.SuspendLayout();
            statusStrip.SuspendLayout();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxSource
            // 
            groupBoxSource.Controls.Add(btnTestSourceConnection);
            groupBoxSource.Controls.Add(txtSourceConnection);
            groupBoxSource.Controls.Add(labelSourceServer);
            groupBoxSource.Location = new Point(14, 44);
            groupBoxSource.Margin = new Padding(4, 4, 4, 4);
            groupBoxSource.Name = "groupBoxSource";
            groupBoxSource.Padding = new Padding(4, 4, 4, 4);
            groupBoxSource.Size = new Size(560, 126);
            groupBoxSource.TabIndex = 1;
            groupBoxSource.TabStop = false;
            groupBoxSource.Text = "源数据库连接";
            // 
            // btnTestSourceConnection
            // 
            btnTestSourceConnection.Location = new Point(18, 81);
            btnTestSourceConnection.Margin = new Padding(4, 4, 4, 4);
            btnTestSourceConnection.Name = "btnTestSourceConnection";
            btnTestSourceConnection.Size = new Size(88, 29);
            btnTestSourceConnection.TabIndex = 9;
            btnTestSourceConnection.Text = "测试连接";
            btnTestSourceConnection.UseVisualStyleBackColor = true;
            // 
            // txtSourceConnection
            // 
            txtSourceConnection.Location = new Point(18, 50);
            txtSourceConnection.Margin = new Padding(4, 4, 4, 4);
            txtSourceConnection.Name = "txtSourceConnection";
            txtSourceConnection.Size = new Size(534, 23);
            txtSourceConnection.TabIndex = 1;
            // 
            // labelSourceServer
            // 
            labelSourceServer.AutoSize = true;
            labelSourceServer.Location = new Point(18, 31);
            labelSourceServer.Margin = new Padding(4, 0, 4, 0);
            labelSourceServer.Name = "labelSourceServer";
            labelSourceServer.Size = new Size(88, 15);
            labelSourceServer.TabIndex = 0;
            labelSourceServer.Text = "链接字符串： ";
            // 
            // groupBoxTarget
            // 
            groupBoxTarget.Controls.Add(btnTestTargetConnection);
            groupBoxTarget.Controls.Add(txtTargetConnection);
            groupBoxTarget.Controls.Add(label1);
            groupBoxTarget.Location = new Point(593, 44);
            groupBoxTarget.Margin = new Padding(4, 4, 4, 4);
            groupBoxTarget.Name = "groupBoxTarget";
            groupBoxTarget.Padding = new Padding(4, 4, 4, 4);
            groupBoxTarget.Size = new Size(560, 126);
            groupBoxTarget.TabIndex = 2;
            groupBoxTarget.TabStop = false;
            groupBoxTarget.Text = "目标数据库连接";
            // 
            // groupBoxTables
            // 
            groupBoxTables.Controls.Add(btnSelectNone);
            groupBoxTables.Controls.Add(btnSelectAll);
            groupBoxTables.Controls.Add(btnRefreshTables);
            groupBoxTables.Controls.Add(checkedListBoxTables);
            groupBoxTables.Location = new Point(14, 178);
            groupBoxTables.Margin = new Padding(4, 4, 4, 4);
            groupBoxTables.Name = "groupBoxTables";
            groupBoxTables.Padding = new Padding(4, 4, 4, 4);
            groupBoxTables.Size = new Size(408, 312);
            groupBoxTables.TabIndex = 3;
            groupBoxTables.TabStop = false;
            groupBoxTables.Text = "选择要同步的表";
            // 
            // btnSelectNone
            // 
            btnSelectNone.Location = new Point(321, 110);
            btnSelectNone.Margin = new Padding(4, 4, 4, 4);
            btnSelectNone.Name = "btnSelectNone";
            btnSelectNone.Size = new Size(76, 29);
            btnSelectNone.TabIndex = 3;
            btnSelectNone.Text = "全不选";
            btnSelectNone.UseVisualStyleBackColor = true;
            // 
            // btnSelectAll
            // 
            btnSelectAll.Location = new Point(321, 69);
            btnSelectAll.Margin = new Padding(4, 4, 4, 4);
            btnSelectAll.Name = "btnSelectAll";
            btnSelectAll.Size = new Size(76, 29);
            btnSelectAll.TabIndex = 2;
            btnSelectAll.Text = "全选";
            btnSelectAll.UseVisualStyleBackColor = true;
            // 
            // btnRefreshTables
            // 
            btnRefreshTables.Location = new Point(321, 28);
            btnRefreshTables.Margin = new Padding(4, 4, 4, 4);
            btnRefreshTables.Name = "btnRefreshTables";
            btnRefreshTables.Size = new Size(76, 29);
            btnRefreshTables.TabIndex = 1;
            btnRefreshTables.Text = "刷新";
            btnRefreshTables.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxTables
            // 
            checkedListBoxTables.CheckOnClick = true;
            checkedListBoxTables.FormattingEnabled = true;
            checkedListBoxTables.Location = new Point(18, 28);
            checkedListBoxTables.Margin = new Padding(4, 4, 4, 4);
            checkedListBoxTables.Name = "checkedListBoxTables";
            checkedListBoxTables.Size = new Size(291, 238);
            checkedListBoxTables.TabIndex = 0;
            // 
            // groupBoxOptions
            // 
            groupBoxOptions.Controls.Add(labelBatchSize);
            groupBoxOptions.Controls.Add(numericBatchSize);
            groupBoxOptions.Controls.Add(chkCreateTargetTables);
            groupBoxOptions.Controls.Add(chkTruncateTarget);
            groupBoxOptions.Controls.Add(chkSyncData);
            groupBoxOptions.Controls.Add(chkSyncStructure);
            groupBoxOptions.Location = new Point(441, 178);
            groupBoxOptions.Margin = new Padding(4, 4, 4, 4);
            groupBoxOptions.Name = "groupBoxOptions";
            groupBoxOptions.Padding = new Padding(4, 4, 4, 4);
            groupBoxOptions.Size = new Size(350, 188);
            groupBoxOptions.TabIndex = 4;
            groupBoxOptions.TabStop = false;
            groupBoxOptions.Text = "同步选项";
            // 
            // labelBatchSize
            // 
            labelBatchSize.AutoSize = true;
            labelBatchSize.Location = new Point(18, 100);
            labelBatchSize.Margin = new Padding(4, 0, 4, 0);
            labelBatchSize.Name = "labelBatchSize";
            labelBatchSize.Size = new Size(62, 15);
            labelBatchSize.TabIndex = 4;
            labelBatchSize.Text = "批次大小:";
            // 
            // numericBatchSize
            // 
            numericBatchSize.Location = new Point(99, 96);
            numericBatchSize.Margin = new Padding(4, 4, 4, 4);
            numericBatchSize.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numericBatchSize.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numericBatchSize.Name = "numericBatchSize";
            numericBatchSize.Size = new Size(93, 23);
            numericBatchSize.TabIndex = 5;
            numericBatchSize.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // chkCreateTargetTables
            // 
            chkCreateTargetTables.AutoSize = true;
            chkCreateTargetTables.Checked = true;
            chkCreateTargetTables.CheckState = CheckState.Checked;
            chkCreateTargetTables.Location = new Point(175, 62);
            chkCreateTargetTables.Margin = new Padding(4, 4, 4, 4);
            chkCreateTargetTables.Name = "chkCreateTargetTables";
            chkCreateTargetTables.Size = new Size(91, 19);
            chkCreateTargetTables.TabIndex = 3;
            chkCreateTargetTables.Text = "创建目标表";
            chkCreateTargetTables.UseVisualStyleBackColor = true;
            // 
            // chkTruncateTarget
            // 
            chkTruncateTarget.AutoSize = true;
            chkTruncateTarget.Location = new Point(175, 31);
            chkTruncateTarget.Margin = new Padding(4, 4, 4, 4);
            chkTruncateTarget.Name = "chkTruncateTarget";
            chkTruncateTarget.Size = new Size(117, 19);
            chkTruncateTarget.TabIndex = 2;
            chkTruncateTarget.Text = "清空目标表数据";
            chkTruncateTarget.UseVisualStyleBackColor = true;
            // 
            // chkSyncData
            // 
            chkSyncData.AutoSize = true;
            chkSyncData.Checked = true;
            chkSyncData.CheckState = CheckState.Checked;
            chkSyncData.Location = new Point(18, 62);
            chkSyncData.Margin = new Padding(4, 4, 4, 4);
            chkSyncData.Name = "chkSyncData";
            chkSyncData.Size = new Size(78, 19);
            chkSyncData.TabIndex = 1;
            chkSyncData.Text = "同步数据";
            chkSyncData.UseVisualStyleBackColor = true;
            // 
            // chkSyncStructure
            // 
            chkSyncStructure.AutoSize = true;
            chkSyncStructure.Checked = true;
            chkSyncStructure.CheckState = CheckState.Checked;
            chkSyncStructure.Location = new Point(18, 31);
            chkSyncStructure.Margin = new Padding(4, 4, 4, 4);
            chkSyncStructure.Name = "chkSyncStructure";
            chkSyncStructure.Size = new Size(78, 19);
            chkSyncStructure.TabIndex = 0;
            chkSyncStructure.Text = "同步结构";
            chkSyncStructure.UseVisualStyleBackColor = true;
            // 
            // groupBoxProgress
            // 
            groupBoxProgress.Controls.Add(richTextBoxLog);
            groupBoxProgress.Controls.Add(labelProgress);
            groupBoxProgress.Controls.Add(progressBarMain);
            groupBoxProgress.Location = new Point(14, 498);
            groupBoxProgress.Margin = new Padding(4, 4, 4, 4);
            groupBoxProgress.Name = "groupBoxProgress";
            groupBoxProgress.Padding = new Padding(4, 4, 4, 4);
            groupBoxProgress.Size = new Size(1139, 225);
            groupBoxProgress.TabIndex = 5;
            groupBoxProgress.TabStop = false;
            groupBoxProgress.Text = "同步进度与日志";
            // 
            // richTextBoxLog
            // 
            richTextBoxLog.Location = new Point(18, 69);
            richTextBoxLog.Margin = new Padding(4, 4, 4, 4);
            richTextBoxLog.Name = "richTextBoxLog";
            richTextBoxLog.ReadOnly = true;
            richTextBoxLog.Size = new Size(1102, 143);
            richTextBoxLog.TabIndex = 2;
            richTextBoxLog.Text = "";
            // 
            // labelProgress
            // 
            labelProgress.AutoSize = true;
            labelProgress.Location = new Point(962, 35);
            labelProgress.Margin = new Padding(4, 0, 4, 0);
            labelProgress.Name = "labelProgress";
            labelProgress.Size = new Size(24, 15);
            labelProgress.TabIndex = 1;
            labelProgress.Text = "0/0";
            // 
            // progressBarMain
            // 
            progressBarMain.Location = new Point(18, 28);
            progressBarMain.Margin = new Padding(4, 4, 4, 4);
            progressBarMain.Name = "progressBarMain";
            progressBarMain.Size = new Size(933, 29);
            progressBarMain.TabIndex = 0;
            // 
            // btnStartSync
            // 
            btnStartSync.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 134);
            btnStartSync.Location = new Point(817, 240);
            btnStartSync.Margin = new Padding(4, 4, 4, 4);
            btnStartSync.Name = "btnStartSync";
            btnStartSync.Size = new Size(105, 44);
            btnStartSync.TabIndex = 6;
            btnStartSync.Text = "开始同步";
            btnStartSync.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Enabled = false;
            btnCancel.Location = new Point(817, 297);
            btnCancel.Margin = new Padding(4, 4, 4, 4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(105, 38);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "取消";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            btnExit.Location = new Point(1062, 297);
            btnExit.Margin = new Padding(4, 4, 4, 4);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(88, 38);
            btnExit.TabIndex = 10;
            btnExit.Text = "退出";
            btnExit.UseVisualStyleBackColor = true;
            // 
            // btnSaveConfig
            // 
            btnSaveConfig.Location = new Point(945, 240);
            btnSaveConfig.Margin = new Padding(4, 4, 4, 4);
            btnSaveConfig.Name = "btnSaveConfig";
            btnSaveConfig.Size = new Size(93, 38);
            btnSaveConfig.TabIndex = 8;
            btnSaveConfig.Text = "保存配置";
            btnSaveConfig.UseVisualStyleBackColor = true;
            // 
            // btnLoadConfig
            // 
            btnLoadConfig.Location = new Point(945, 297);
            btnLoadConfig.Margin = new Padding(4, 4, 4, 4);
            btnLoadConfig.Name = "btnLoadConfig";
            btnLoadConfig.Size = new Size(93, 38);
            btnLoadConfig.TabIndex = 9;
            btnLoadConfig.Text = "加载配置";
            btnLoadConfig.UseVisualStyleBackColor = true;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel, toolStripProgressBar });
            statusStrip.Location = new Point(0, 727);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 16, 0);
            statusStrip.Size = new Size(1167, 22);
            statusStrip.TabIndex = 11;
            statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(33, 17);
            toolStripStatusLabel.Text = "就绪";
            // 
            // toolStripProgressBar
            // 
            toolStripProgressBar.Name = "toolStripProgressBar";
            toolStripProgressBar.Size = new Size(233, 20);
            toolStripProgressBar.Visible = false;
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, helpToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(7, 2, 0, 2);
            menuStrip.Size = new Size(1167, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveConfigToolStripMenuItem, loadConfigToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(45, 20);
            fileToolStripMenuItem.Text = "文件";
            // 
            // saveConfigToolStripMenuItem
            // 
            saveConfigToolStripMenuItem.Name = "saveConfigToolStripMenuItem";
            saveConfigToolStripMenuItem.Size = new Size(126, 22);
            saveConfigToolStripMenuItem.Text = "保存配置";
            // 
            // loadConfigToolStripMenuItem
            // 
            loadConfigToolStripMenuItem.Name = "loadConfigToolStripMenuItem";
            loadConfigToolStripMenuItem.Size = new Size(126, 22);
            loadConfigToolStripMenuItem.Text = "加载配置";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(126, 22);
            exitToolStripMenuItem.Text = "退出";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(45, 20);
            helpToolStripMenuItem.Text = "帮助";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(100, 22);
            aboutToolStripMenuItem.Text = "关于";
            // 
            // btnTestTargetConnection
            // 
            btnTestTargetConnection.Location = new Point(18, 81);
            btnTestTargetConnection.Margin = new Padding(4);
            btnTestTargetConnection.Name = "btnTestTargetConnection";
            btnTestTargetConnection.Size = new Size(88, 29);
            btnTestTargetConnection.TabIndex = 12;
            btnTestTargetConnection.Text = "测试连接";
            btnTestTargetConnection.UseVisualStyleBackColor = true;
            // 
            // txtTargetConnection
            // 
            txtTargetConnection.Location = new Point(18, 50);
            txtTargetConnection.Margin = new Padding(4);
            txtTargetConnection.Name = "txtTargetConnection";
            txtTargetConnection.Size = new Size(534, 23);
            txtTargetConnection.TabIndex = 11;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 31);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(88, 15);
            label1.TabIndex = 10;
            label1.Text = "链接字符串： ";
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1167, 749);
            Controls.Add(statusStrip);
            Controls.Add(btnExit);
            Controls.Add(btnLoadConfig);
            Controls.Add(btnSaveConfig);
            Controls.Add(btnCancel);
            Controls.Add(btnStartSync);
            Controls.Add(groupBoxProgress);
            Controls.Add(groupBoxOptions);
            Controls.Add(groupBoxTables);
            Controls.Add(groupBoxTarget);
            Controls.Add(groupBoxSource);
            Controls.Add(menuStrip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip;
            Margin = new Padding(4, 4, 4, 4);
            MaximizeBox = false;
            Name = "FrmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SQL Server 数据库同步工具 v1.0";
            groupBoxSource.ResumeLayout(false);
            groupBoxSource.PerformLayout();
            groupBoxTarget.ResumeLayout(false);
            groupBoxTarget.PerformLayout();
            groupBoxTables.ResumeLayout(false);
            groupBoxOptions.ResumeLayout(false);
            groupBoxOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericBatchSize).EndInit();
            groupBoxProgress.ResumeLayout(false);
            groupBoxProgress.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveConfigToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadConfigToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;

        private System.Windows.Forms.GroupBox groupBoxSource;
        private System.Windows.Forms.TextBox txtSourceConnection;
        private System.Windows.Forms.Button btnTestSourceConnection;
        private System.Windows.Forms.Label labelSourceServer;

        private System.Windows.Forms.GroupBox groupBoxTarget;

        private System.Windows.Forms.GroupBox groupBoxTables;
        private System.Windows.Forms.CheckedListBox checkedListBoxTables;
        private System.Windows.Forms.Button btnRefreshTables;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectNone;

        private System.Windows.Forms.GroupBox groupBoxOptions;
        private System.Windows.Forms.CheckBox chkSyncStructure;
        private System.Windows.Forms.CheckBox chkSyncData;
        private System.Windows.Forms.CheckBox chkTruncateTarget;
        private System.Windows.Forms.CheckBox chkCreateTargetTables;
        private System.Windows.Forms.NumericUpDown numericBatchSize;
        private System.Windows.Forms.Label labelBatchSize;

        private System.Windows.Forms.GroupBox groupBoxProgress;
        private System.Windows.Forms.ProgressBar progressBarMain;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.RichTextBox richTextBoxLog;

        private System.Windows.Forms.Button btnStartSync;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.Button btnLoadConfig;
        private System.Windows.Forms.Button btnExit;

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private Button btnTestTargetConnection;
        private TextBox txtTargetConnection;
        private Label label1;
    }
}
