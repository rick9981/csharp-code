
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AppWinStyle1
{
    public partial class Form1 : Form
    {
        #region Windows API for Form Movement
        const int WM_NCLBUTTONDOWN = 0xA1;
        const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern bool ReleaseCapture();
        #endregion

        private Button currentActiveButton = null;
        private bool isCollapsed = false;

        public Form1()
        {
            InitializeComponent();
            InitializeEventHandlers();
            SetActiveButton(btnDashboard); // 默认选中仪表板
        }

        private void InitializeEventHandlers()
        {

            btnDashboard.Click += (s, e) => { LoadContent("仪表板"); SetActiveButton(btnDashboard); };
            btnUsers.Click += (s, e) => { LoadContent("用户管理"); SetActiveButton(btnUsers); };
            btnProducts.Click += (s, e) => { LoadContent("产品管理"); SetActiveButton(btnProducts); };
            btnOrders.Click += (s, e) => { LoadContent("订单管理"); SetActiveButton(btnOrders); };
            btnReports.Click += (s, e) => { LoadContent("报表分析"); SetActiveButton(btnReports); };
            btnSettings.Click += (s, e) => { LoadContent("系统设置"); SetActiveButton(btnSettings); };
            btnExit.Click += (s, e) =>
            {
                if (MessageBox.Show("您确认退出系统", "信息", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Application.Exit();
                }
            };

            btnMinimize.Click += (s, e) => WindowState = FormWindowState.Minimized;
            btnMaximize.Click += BtnMaximize_Click;
            btnClose.Click += (s, e) => Close();

            btnMenuToggle.Click += (s, e) => ToggleSidebar();

            topPanel.MouseDown += TopPanel_MouseDown;

            AddHoverEffects();
        }

        private void BtnMaximize_Click(object sender, EventArgs e)
        {
            ToggleWindowState();
        }

        private void ToggleWindowState()
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
                btnMaximize.Text = "🗖";
            }
            else
            {
                WindowState = FormWindowState.Maximized;
                btnMaximize.Text = "🗗";
            }
        }

        private void AddHoverEffects()
        {
            var menuButtons = new[] { btnDashboard, btnUsers, btnProducts, btnOrders, btnReports, btnSettings };

            foreach (var button in menuButtons)
            {
                button.MouseEnter += MenuButton_MouseEnter;
                button.MouseLeave += MenuButton_MouseLeave;
            }
        }

        private void MenuButton_MouseEnter(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != currentActiveButton)
            {
                button.BackColor = Color.FromArgb(58, 67, 84);
                button.ForeColor = Color.White;
            }
        }

        private void MenuButton_MouseLeave(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != currentActiveButton)
            {
                button.BackColor = Color.Transparent;
                button.ForeColor = Color.FromArgb(189, 195, 199);
            }
        }

        private void LoadContent(string content)
        {
            lblPageTitle.Text = content;
            mainContentPanel.Controls.Clear();

            switch (content)
            {
                case "仪表板":
                    LoadDashboard();
                    break;
                case "用户管理":
                    LoadUserManagement();
                    break;
                case "产品管理":
                    LoadProductManagement();
                    break;
                case "订单管理":
                    LoadOrderManagement();
                    break;
                case "报表分析":
                    LoadReports();
                    break;
                case "系统设置":
                    LoadSettings();
                    break;
            }
        }

        private void SetActiveButton(Button activeButton)
        {
            ResetButtonStyles();
            currentActiveButton = activeButton;
            activeButton.BackColor = Color.FromArgb(52, 152, 219);
            activeButton.ForeColor = Color.White;
            activeButton.Font = new Font("Microsoft YaHei", 10F, FontStyle.Bold);
        }

        private void ResetButtonStyles()
        {
            var menuButtons = new[] { btnDashboard, btnUsers, btnProducts, btnOrders, btnReports, btnSettings, btnExit };

            foreach (var button in menuButtons)
            {
                button.BackColor = Color.Transparent;
                button.ForeColor = Color.FromArgb(189, 195, 199);
                button.Font = new Font("Microsoft YaHei", 10F, FontStyle.Regular);
            }
        }

        private void ToggleSidebar()
        {
            if (!isCollapsed)
            {
                CollapseSidebar();
            }
            else
            {
                ExpandSidebar();
            }
            isCollapsed = !isCollapsed;
        }

        private void CollapseSidebar()
        {
            sidebarPanel.Width = 60;

            // 调整菜单切换按钮
            btnMenuToggle.Size = new Size(60, 45);
            btnMenuToggle.Location = new Point(0, 6);
            btnMenuToggle.Text = "☰";
            btnMenuToggle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            // 调整所有菜单按钮为图标模式
            var buttons = new[]
            {
                new { btn = btnDashboard, icon = "📊" },
                new { btn = btnUsers, icon = "👥" },
                new { btn = btnProducts, icon = "📦" },
                new { btn = btnOrders, icon = "📋" },
                new { btn = btnReports, icon = "📈" },
                new { btn = btnSettings, icon = "⚙️" },
                new { btn = btnExit, icon = "🚪"}
            };

            foreach (var item in buttons)
            {
                item.btn.Size = new Size(60, 47);
                item.btn.Text = item.icon;
                item.btn.Font = new Font("Segoe UI", 16F);
                item.btn.TextAlign = ContentAlignment.MiddleCenter;
                item.btn.FlatAppearance.BorderSize = 0;
            }

            // 调整内容区域
            AdjustContentArea(60);
        }

        private void ExpandSidebar()
        {
            sidebarPanel.Width = 210;

            // 恢复菜单切换按钮
            btnMenuToggle.Size = new Size(60, 45);
            btnMenuToggle.Location = new Point(3, 6);
            btnMenuToggle.Text = "☰";
            btnMenuToggle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            // 恢复所有菜单按钮的文字和位置
            var buttons = new[]
            {
                new { btn = btnDashboard, text = "📊  仪表板" },
                new { btn = btnUsers, text = "👥  用户管理"},
                new { btn = btnProducts, text = "📦  产品管理" },
                new { btn = btnOrders, text = "📋  订单管理"},
                new { btn = btnReports, text = "📈  报表分析" },
                new { btn = btnSettings, text = "⚙️  系统设置"},
                new { btn = btnExit, text = "🚪  退出系统"}
            };

            foreach (var item in buttons)
            {
                item.btn.Size = new Size(198, 47);
                item.btn.Text = item.text;
                item.btn.Font = new Font("Microsoft YaHei", 10F);
                item.btn.TextAlign = ContentAlignment.MiddleLeft;
                item.btn.FlatAppearance.BorderSize = 0;
            }

            if (currentActiveButton != null)
            {
                SetActiveButton(currentActiveButton);
            }

            // 调整内容区域
            AdjustContentArea(210);
        }

        private void AdjustContentArea(int sidebarWidth)
        {
            // 调整内容标题区域
            contentHeaderPanel.Location = new Point(sidebarWidth, contentHeaderPanel.Location.Y);
            contentHeaderPanel.Width = this.Width - sidebarWidth;

            // 调整主内容区域
            mainContentPanel.Location = new Point(sidebarWidth, mainContentPanel.Location.Y);
            mainContentPanel.Width = this.Width - sidebarWidth;
        }

        private void TopPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 2)
            {
                ToggleWindowState();
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #region Content Loading Methods
        private void LoadDashboard()
        {

        }


        private Panel CreateContentPanel()
        {
            return new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(20)
            };
        }

        private void LoadUserManagement()
        {
            var userPanel = CreateContentPanel();
            var titleLabel = CreateModuleTitle("用户管理模块");

            // 添加一些示例按钮
            var btnAddUser = CreateActionButton("添加用户", 30, 80);
            var btnEditUser = CreateActionButton("编辑用户", 150, 80);
            var btnDeleteUser = CreateActionButton("删除用户", 270, 80);

            userPanel.Controls.AddRange(new Control[] { titleLabel, btnAddUser, btnEditUser, btnDeleteUser });
            mainContentPanel.Controls.Add(userPanel);
        }

        private void LoadProductManagement()
        {
            var productPanel = CreateContentPanel();
            var titleLabel = CreateModuleTitle("产品管理模块");

            var btnAddProduct = CreateActionButton("添加产品", 30, 80);
            var btnEditProduct = CreateActionButton("编辑产品", 150, 80);
            var btnDeleteProduct = CreateActionButton("删除产品", 270, 80);

            productPanel.Controls.AddRange(new Control[] { titleLabel, btnAddProduct, btnEditProduct, btnDeleteProduct });
            mainContentPanel.Controls.Add(productPanel);
        }

        private void LoadOrderManagement()
        {
            var orderPanel = CreateContentPanel();
            var titleLabel = CreateModuleTitle("订单管理模块");

            var btnViewOrders = CreateActionButton("查看订单", 30, 80);
            var btnProcessOrder = CreateActionButton("处理订单", 150, 80);
            var btnOrderHistory = CreateActionButton("订单历史", 270, 80);

            orderPanel.Controls.AddRange(new Control[] { titleLabel, btnViewOrders, btnProcessOrder, btnOrderHistory });
            mainContentPanel.Controls.Add(orderPanel);
        }

        private void LoadReports()
        {
            var reportPanel = CreateContentPanel();
            var titleLabel = CreateModuleTitle("报表分析模块");

            var btnSalesReport = CreateActionButton("销售报表", 30, 80);
            var btnUserReport = CreateActionButton("用户报表", 150, 80);
            var btnFinanceReport = CreateActionButton("财务报表", 270, 80);

            reportPanel.Controls.AddRange(new Control[] { titleLabel, btnSalesReport, btnUserReport, btnFinanceReport });
            mainContentPanel.Controls.Add(reportPanel);
        }

        private void LoadSettings()
        {
            var settingsPanel = CreateContentPanel();
            var titleLabel = CreateModuleTitle("系统设置模块");

            var btnSystemConfig = CreateActionButton("系统配置", 30, 80);
            var btnUserSettings = CreateActionButton("用户设置", 150, 80);
            var btnBackup = CreateActionButton("数据备份", 270, 80);

            settingsPanel.Controls.AddRange(new Control[] { titleLabel, btnSystemConfig, btnUserSettings, btnBackup });
            mainContentPanel.Controls.Add(settingsPanel);
        }

        private Label CreateModuleTitle(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Microsoft YaHei", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(30, 20),
                AutoSize = true
            };
        }

        private Button CreateActionButton(string text, int x, int y)
        {
            var button = new Button
            {
                Text = text,
                Font = new Font("Microsoft YaHei", 9F),
                Size = new Size(100, 35),
                Location = new Point(x, y),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                UseVisualStyleBackColor = false
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(41, 128, 185);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(37, 116, 169);

            return button;
        }
        #endregion

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (contentHeaderPanel != null && mainContentPanel != null)
            {
                int sidebarWidth = sidebarPanel?.Width ?? 219;
                AdjustContentArea(sidebarWidth);
            }
        }
    }
}