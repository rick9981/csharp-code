namespace AppWinStyle1
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
            sidebarPanel = new Panel();
            panel1 = new Panel();
            btnExit = new Button();
            btnSettings = new Button();
            btnReports = new Button();
            btnOrders = new Button();
            btnProducts = new Button();
            btnUsers = new Button();
            btnDashboard = new Button();
            panel2 = new Panel();
            btnMenuToggle = new Button();
            topPanel = new Panel();
            btnClose = new Button();
            btnMaximize = new Button();
            btnMinimize = new Button();
            lblTopSystemName = new Label();
            mainContentPanel = new Panel();
            lblPageTitle = new Label();
            contentHeaderPanel = new Panel();
            sidebarPanel.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            topPanel.SuspendLayout();
            contentHeaderPanel.SuspendLayout();
            SuspendLayout();
            // 
            // sidebarPanel
            // 
            sidebarPanel.BackColor = Color.FromArgb(45, 52, 67);
            sidebarPanel.Controls.Add(panel1);
            sidebarPanel.Controls.Add(panel2);
            sidebarPanel.Dock = DockStyle.Left;
            sidebarPanel.Location = new Point(0, 47);
            sidebarPanel.Name = "sidebarPanel";
            sidebarPanel.Size = new Size(210, 797);
            sidebarPanel.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnExit);
            panel1.Controls.Add(btnSettings);
            panel1.Controls.Add(btnReports);
            panel1.Controls.Add(btnOrders);
            panel1.Controls.Add(btnProducts);
            panel1.Controls.Add(btnUsers);
            panel1.Controls.Add(btnDashboard);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 54);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(0, 0, 0, 20);
            panel1.Size = new Size(210, 743);
            panel1.TabIndex = 3;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.Transparent;
            btnExit.Dock = DockStyle.Bottom;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.FlatAppearance.MouseDownBackColor = Color.FromArgb(67, 83, 105);
            btnExit.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 67, 84);
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Microsoft YaHei", 10F);
            btnExit.ForeColor = Color.FromArgb(189, 195, 199);
            btnExit.Location = new Point(0, 676);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(210, 47);
            btnExit.TabIndex = 14;
            btnExit.Text = "⚙️  退出系统";
            btnExit.TextAlign = ContentAlignment.MiddleLeft;
            btnExit.UseVisualStyleBackColor = false;
            // 
            // btnSettings
            // 
            btnSettings.BackColor = Color.Transparent;
            btnSettings.Dock = DockStyle.Top;
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.FlatAppearance.MouseDownBackColor = Color.FromArgb(67, 83, 105);
            btnSettings.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 67, 84);
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Font = new Font("Microsoft YaHei", 10F);
            btnSettings.ForeColor = Color.FromArgb(189, 195, 199);
            btnSettings.Location = new Point(0, 235);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(210, 47);
            btnSettings.TabIndex = 13;
            btnSettings.Text = "⚙️  系统设置";
            btnSettings.TextAlign = ContentAlignment.MiddleLeft;
            btnSettings.UseVisualStyleBackColor = false;
            // 
            // btnReports
            // 
            btnReports.BackColor = Color.Transparent;
            btnReports.Dock = DockStyle.Top;
            btnReports.FlatAppearance.BorderSize = 0;
            btnReports.FlatAppearance.MouseDownBackColor = Color.FromArgb(67, 83, 105);
            btnReports.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 67, 84);
            btnReports.FlatStyle = FlatStyle.Flat;
            btnReports.Font = new Font("Microsoft YaHei", 10F);
            btnReports.ForeColor = Color.FromArgb(189, 195, 199);
            btnReports.Location = new Point(0, 188);
            btnReports.Name = "btnReports";
            btnReports.Size = new Size(210, 47);
            btnReports.TabIndex = 12;
            btnReports.Text = "📈  报表分析";
            btnReports.TextAlign = ContentAlignment.MiddleLeft;
            btnReports.UseVisualStyleBackColor = false;
            // 
            // btnOrders
            // 
            btnOrders.BackColor = Color.Transparent;
            btnOrders.Dock = DockStyle.Top;
            btnOrders.FlatAppearance.BorderSize = 0;
            btnOrders.FlatAppearance.MouseDownBackColor = Color.FromArgb(67, 83, 105);
            btnOrders.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 67, 84);
            btnOrders.FlatStyle = FlatStyle.Flat;
            btnOrders.Font = new Font("Microsoft YaHei", 10F);
            btnOrders.ForeColor = Color.FromArgb(189, 195, 199);
            btnOrders.Location = new Point(0, 141);
            btnOrders.Name = "btnOrders";
            btnOrders.Size = new Size(210, 47);
            btnOrders.TabIndex = 11;
            btnOrders.Text = "📋  订单管理";
            btnOrders.TextAlign = ContentAlignment.MiddleLeft;
            btnOrders.UseVisualStyleBackColor = false;
            // 
            // btnProducts
            // 
            btnProducts.BackColor = Color.Transparent;
            btnProducts.Dock = DockStyle.Top;
            btnProducts.FlatAppearance.BorderSize = 0;
            btnProducts.FlatAppearance.MouseDownBackColor = Color.FromArgb(67, 83, 105);
            btnProducts.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 67, 84);
            btnProducts.FlatStyle = FlatStyle.Flat;
            btnProducts.Font = new Font("Microsoft YaHei", 10F);
            btnProducts.ForeColor = Color.FromArgb(189, 195, 199);
            btnProducts.Location = new Point(0, 94);
            btnProducts.Name = "btnProducts";
            btnProducts.Size = new Size(210, 47);
            btnProducts.TabIndex = 10;
            btnProducts.Text = "📦  产品管理";
            btnProducts.TextAlign = ContentAlignment.MiddleLeft;
            btnProducts.UseVisualStyleBackColor = false;
            // 
            // btnUsers
            // 
            btnUsers.BackColor = Color.Transparent;
            btnUsers.Dock = DockStyle.Top;
            btnUsers.FlatAppearance.BorderSize = 0;
            btnUsers.FlatAppearance.MouseDownBackColor = Color.FromArgb(67, 83, 105);
            btnUsers.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 67, 84);
            btnUsers.FlatStyle = FlatStyle.Flat;
            btnUsers.Font = new Font("Microsoft YaHei", 10F);
            btnUsers.ForeColor = Color.FromArgb(189, 195, 199);
            btnUsers.Location = new Point(0, 47);
            btnUsers.Name = "btnUsers";
            btnUsers.Size = new Size(210, 47);
            btnUsers.TabIndex = 9;
            btnUsers.Text = "👥  用户管理";
            btnUsers.TextAlign = ContentAlignment.MiddleLeft;
            btnUsers.UseVisualStyleBackColor = false;
            // 
            // btnDashboard
            // 
            btnDashboard.BackColor = Color.FromArgb(52, 152, 219);
            btnDashboard.Dock = DockStyle.Top;
            btnDashboard.FlatAppearance.BorderSize = 0;
            btnDashboard.FlatAppearance.MouseDownBackColor = Color.FromArgb(41, 128, 185);
            btnDashboard.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 134, 193);
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.Font = new Font("Microsoft YaHei", 10F, FontStyle.Bold);
            btnDashboard.ForeColor = Color.White;
            btnDashboard.Location = new Point(0, 0);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Size = new Size(210, 47);
            btnDashboard.TabIndex = 8;
            btnDashboard.Text = "📊  仪表板";
            btnDashboard.TextAlign = ContentAlignment.MiddleLeft;
            btnDashboard.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnMenuToggle);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(210, 54);
            panel2.TabIndex = 2;
            // 
            // btnMenuToggle
            // 
            btnMenuToggle.BackColor = Color.Transparent;
            btnMenuToggle.FlatAppearance.BorderSize = 0;
            btnMenuToggle.FlatAppearance.MouseDownBackColor = Color.FromArgb(67, 83, 105);
            btnMenuToggle.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 67, 84);
            btnMenuToggle.FlatStyle = FlatStyle.Flat;
            btnMenuToggle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnMenuToggle.ForeColor = Color.White;
            btnMenuToggle.Location = new Point(3, 6);
            btnMenuToggle.Name = "btnMenuToggle";
            btnMenuToggle.Size = new Size(39, 39);
            btnMenuToggle.TabIndex = 0;
            btnMenuToggle.Text = "☰";
            btnMenuToggle.UseVisualStyleBackColor = false;
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.FromArgb(52, 73, 94);
            topPanel.Controls.Add(lblTopSystemName);
            topPanel.Controls.Add(btnClose);
            topPanel.Controls.Add(btnMaximize);
            topPanel.Controls.Add(btnMinimize);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(1225, 47);
            topPanel.TabIndex = 1;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.Transparent;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(192, 57, 43);
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(231, 76, 60);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 12F);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(1190, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(35, 47);
            btnClose.TabIndex = 2;
            btnClose.Text = "✕";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // btnMaximize
            // 
            btnMaximize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMaximize.BackColor = Color.Transparent;
            btnMaximize.FlatAppearance.BorderSize = 0;
            btnMaximize.FlatAppearance.MouseDownBackColor = Color.FromArgb(67, 83, 105);
            btnMaximize.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 67, 84);
            btnMaximize.FlatStyle = FlatStyle.Flat;
            btnMaximize.Font = new Font("Segoe UI", 12F);
            btnMaximize.ForeColor = Color.White;
            btnMaximize.Location = new Point(1159, 0);
            btnMaximize.Name = "btnMaximize";
            btnMaximize.Size = new Size(31, 47);
            btnMaximize.TabIndex = 1;
            btnMaximize.Text = "🗖";
            btnMaximize.UseVisualStyleBackColor = false;
            // 
            // btnMinimize
            // 
            btnMinimize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMinimize.BackColor = Color.Transparent;
            btnMinimize.FlatAppearance.BorderSize = 0;
            btnMinimize.FlatAppearance.MouseDownBackColor = Color.FromArgb(67, 83, 105);
            btnMinimize.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 67, 84);
            btnMinimize.FlatStyle = FlatStyle.Flat;
            btnMinimize.Font = new Font("Segoe UI", 12F);
            btnMinimize.ForeColor = Color.White;
            btnMinimize.Location = new Point(1129, 0);
            btnMinimize.Name = "btnMinimize";
            btnMinimize.Size = new Size(31, 47);
            btnMinimize.TabIndex = 0;
            btnMinimize.Text = "─";
            btnMinimize.UseVisualStyleBackColor = false;
            // 
            // lblTopSystemName
            // 
            lblTopSystemName.AutoSize = true;
            lblTopSystemName.Font = new Font("Microsoft YaHei", 14F, FontStyle.Bold);
            lblTopSystemName.ForeColor = Color.White;
            lblTopSystemName.Location = new Point(3, 10);
            lblTopSystemName.Name = "lblTopSystemName";
            lblTopSystemName.Size = new Size(134, 26);
            lblTopSystemName.TabIndex = 3;
            lblTopSystemName.Text = "⚡ 管理系统 v1.0";
            lblTopSystemName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // mainContentPanel
            // 
            mainContentPanel.BackColor = Color.FromArgb(248, 249, 250);
            mainContentPanel.Dock = DockStyle.Fill;
            mainContentPanel.Location = new Point(210, 122);
            mainContentPanel.Name = "mainContentPanel";
            mainContentPanel.Size = new Size(1015, 722);
            mainContentPanel.TabIndex = 3;
            // 
            // lblPageTitle
            // 
            lblPageTitle.AutoSize = true;
            lblPageTitle.Font = new Font("Microsoft YaHei", 18F, FontStyle.Bold);
            lblPageTitle.ForeColor = Color.FromArgb(44, 62, 80);
            lblPageTitle.Location = new Point(26, 23);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.Size = new Size(86, 31);
            lblPageTitle.TabIndex = 0;
            lblPageTitle.Text = "仪表板";
            // 
            // contentHeaderPanel
            // 
            contentHeaderPanel.BackColor = Color.White;
            contentHeaderPanel.Controls.Add(lblPageTitle);
            contentHeaderPanel.Dock = DockStyle.Top;
            contentHeaderPanel.Location = new Point(210, 47);
            contentHeaderPanel.Name = "contentHeaderPanel";
            contentHeaderPanel.Size = new Size(1015, 75);
            contentHeaderPanel.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 244, 248);
            ClientSize = new Size(1225, 844);
            Controls.Add(mainContentPanel);
            Controls.Add(contentHeaderPanel);
            Controls.Add(sidebarPanel);
            Controls.Add(topPanel);
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(1050, 656);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "管理系统";
            sidebarPanel.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            contentHeaderPanel.ResumeLayout(false);
            contentHeaderPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel sidebarPanel;
        private Panel topPanel;
        private Panel mainContentPanel;
        private Button btnMenuToggle;
        private Label lblPageTitle;
        private Button btnMinimize;
        private Button btnMaximize;
        private Button btnClose;
        private Panel contentHeaderPanel;
        private Panel panel2;
        private Panel panel1;
        private Button btnSettings;
        private Button btnReports;
        private Button btnOrders;
        private Button btnProducts;
        private Button btnUsers;
        private Button btnDashboard;
        private Button btnExit;
        private Label lblTopSystemName;

    }
}
