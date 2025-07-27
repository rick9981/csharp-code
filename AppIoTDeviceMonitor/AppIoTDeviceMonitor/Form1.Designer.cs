namespace AppIoTDeviceMonitor
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvDevices = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExportData = new System.Windows.Forms.Button();
            this.btnResetChart = new System.Windows.Forms.Button();
            this.btnStopMonitoring = new System.Windows.Forms.Button();
            this.btnStartMonitoring = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.formsPlot = new ScottPlot.WinForms.FormsPlot();
            this.panelChartControls = new System.Windows.Forms.Panel();
            this.btnSaveChart = new System.Windows.Forms.Button();
            this.btnAutoScale = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTotalDevices = new System.Windows.Forms.Label();
            this.lblOnlineDevices = new System.Windows.Forms.Label();
            this.lblErrorDevices = new System.Windows.Forms.Label();
            this.lblMemoryUsage = new System.Windows.Forms.Label();
            this.lblCpuUsage = new System.Windows.Forms.Label();
            this.lblDataRate = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevices)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panelChartControls.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(1200, 700);
            this.splitContainer1.SplitterDistance = 950;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Size = new System.Drawing.Size(950, 700);
            this.splitContainer2.SplitterDistance = 350;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvDevices);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(950, 350);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设备状态监控";
            // 
            // dgvDevices
            // 
            this.dgvDevices.AllowUserToAddRows = false;
            this.dgvDevices.AllowUserToDeleteRows = false;
            this.dgvDevices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDevices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDevices.Location = new System.Drawing.Point(3, 46);
            this.dgvDevices.Name = "dgvDevices";
            this.dgvDevices.ReadOnly = true;
            this.dgvDevices.RowHeadersVisible = false;
            this.dgvDevices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDevices.Size = new System.Drawing.Size(944, 301);
            this.dgvDevices.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExportData);
            this.panel1.Controls.Add(this.btnResetChart);
            this.panel1.Controls.Add(this.btnStopMonitoring);
            this.panel1.Controls.Add(this.btnStartMonitoring);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(944, 29);
            this.panel1.TabIndex = 1;
            // 
            // btnExportData
            // 
            this.btnExportData.Location = new System.Drawing.Point(264, 3);
            this.btnExportData.Name = "btnExportData";
            this.btnExportData.Size = new System.Drawing.Size(80, 23);
            this.btnExportData.TabIndex = 3;
            this.btnExportData.Text = "导出数据";
            this.btnExportData.UseVisualStyleBackColor = true;
            this.btnExportData.Click += new System.EventHandler(this.btnExportData_Click);
            // 
            // btnResetChart
            // 
            this.btnResetChart.Location = new System.Drawing.Point(178, 3);
            this.btnResetChart.Name = "btnResetChart";
            this.btnResetChart.Size = new System.Drawing.Size(80, 23);
            this.btnResetChart.TabIndex = 2;
            this.btnResetChart.Text = "重置图表";
            this.btnResetChart.UseVisualStyleBackColor = true;
            this.btnResetChart.Click += new System.EventHandler(this.btnResetChart_Click);
            // 
            // btnStopMonitoring
            // 
            this.btnStopMonitoring.Location = new System.Drawing.Point(92, 3);
            this.btnStopMonitoring.Name = "btnStopMonitoring";
            this.btnStopMonitoring.Size = new System.Drawing.Size(80, 23);
            this.btnStopMonitoring.TabIndex = 1;
            this.btnStopMonitoring.Text = "停止监控";
            this.btnStopMonitoring.UseVisualStyleBackColor = true;
            this.btnStopMonitoring.Click += new System.EventHandler(this.btnStopMonitoring_Click);
            // 
            // btnStartMonitoring
            // 
            this.btnStartMonitoring.Location = new System.Drawing.Point(6, 3);
            this.btnStartMonitoring.Name = "btnStartMonitoring";
            this.btnStartMonitoring.Size = new System.Drawing.Size(80, 23);
            this.btnStartMonitoring.TabIndex = 0;
            this.btnStartMonitoring.Text = "开始监控";
            this.btnStartMonitoring.UseVisualStyleBackColor = true;
            this.btnStartMonitoring.Click += new System.EventHandler(this.btnStartMonitoring_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.formsPlot);
            this.groupBox2.Controls.Add(this.panelChartControls);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(950, 346);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "实时数据图表 (ScottPlot)";
            // 
            // formsPlot
            // 
            this.formsPlot.DisplayScale = 1F;
            this.formsPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlot.Location = new System.Drawing.Point(3, 46);
            this.formsPlot.Name = "formsPlot";
            this.formsPlot.Size = new System.Drawing.Size(944, 297);
            this.formsPlot.TabIndex = 0;
            // 
            // panelChartControls
            // 
            this.panelChartControls.Controls.Add(this.btnSaveChart);
            this.panelChartControls.Controls.Add(this.btnAutoScale);
            this.panelChartControls.Controls.Add(this.btnZoomOut);
            this.panelChartControls.Controls.Add(this.btnZoomIn);
            this.panelChartControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelChartControls.Location = new System.Drawing.Point(3, 17);
            this.panelChartControls.Name = "panelChartControls";
            this.panelChartControls.Size = new System.Drawing.Size(944, 29);
            this.panelChartControls.TabIndex = 1;
            // 
            // btnSaveChart
            // 
            this.btnSaveChart.Location = new System.Drawing.Point(264, 3);
            this.btnSaveChart.Name = "btnSaveChart";
            this.btnSaveChart.Size = new System.Drawing.Size(80, 23);
            this.btnSaveChart.TabIndex = 3;
            this.btnSaveChart.Text = "保存图表";
            this.btnSaveChart.UseVisualStyleBackColor = true;
            this.btnSaveChart.Click += new System.EventHandler(this.btnSaveChart_Click);
            // 
            // btnAutoScale
            // 
            this.btnAutoScale.Location = new System.Drawing.Point(178, 3);
            this.btnAutoScale.Name = "btnAutoScale";
            this.btnAutoScale.Size = new System.Drawing.Size(80, 23);
            this.btnAutoScale.TabIndex = 2;
            this.btnAutoScale.Text = "自动缩放";
            this.btnAutoScale.UseVisualStyleBackColor = true;
            this.btnAutoScale.Click += new System.EventHandler(this.btnAutoScale_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Location = new System.Drawing.Point(92, 3);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(80, 23);
            this.btnZoomOut.TabIndex = 1;
            this.btnZoomOut.Text = "缩小";
            this.btnZoomOut.UseVisualStyleBackColor = true;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Location = new System.Drawing.Point(6, 3);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(80, 23);
            this.btnZoomIn.TabIndex = 0;
            this.btnZoomIn.Text = "放大";
            this.btnZoomIn.UseVisualStyleBackColor = true;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(246, 700);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "系统状态";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblTotalDevices, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblOnlineDevices, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblErrorDevices, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblMemoryUsage, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblCpuUsage, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblDataRate, 0, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(240, 680);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblTotalDevices
            // 
            this.lblTotalDevices.AutoSize = true;
            this.lblTotalDevices.BackColor = System.Drawing.Color.LightBlue;
            this.lblTotalDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalDevices.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTotalDevices.Location = new System.Drawing.Point(3, 3);
            this.lblTotalDevices.Margin = new System.Windows.Forms.Padding(3);
            this.lblTotalDevices.Name = "lblTotalDevices";
            this.lblTotalDevices.Size = new System.Drawing.Size(234, 91);
            this.lblTotalDevices.TabIndex = 0;
            this.lblTotalDevices.Text = "总设备数: 0";
            this.lblTotalDevices.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOnlineDevices
            // 
            this.lblOnlineDevices.AutoSize = true;
            this.lblOnlineDevices.BackColor = System.Drawing.Color.LightGreen;
            this.lblOnlineDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOnlineDevices.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblOnlineDevices.Location = new System.Drawing.Point(3, 100);
            this.lblOnlineDevices.Margin = new System.Windows.Forms.Padding(3);
            this.lblOnlineDevices.Name = "lblOnlineDevices";
            this.lblOnlineDevices.Size = new System.Drawing.Size(234, 91);
            this.lblOnlineDevices.TabIndex = 1;
            this.lblOnlineDevices.Text = "在线设备: 0";
            this.lblOnlineDevices.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblErrorDevices
            // 
            this.lblErrorDevices.AutoSize = true;
            this.lblErrorDevices.BackColor = System.Drawing.Color.LightCoral;
            this.lblErrorDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblErrorDevices.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblErrorDevices.Location = new System.Drawing.Point(3, 197);
            this.lblErrorDevices.Margin = new System.Windows.Forms.Padding(3);
            this.lblErrorDevices.Name = "lblErrorDevices";
            this.lblErrorDevices.Size = new System.Drawing.Size(234, 91);
            this.lblErrorDevices.TabIndex = 2;
            this.lblErrorDevices.Text = "故障设备: 0";
            this.lblErrorDevices.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMemoryUsage
            // 
            this.lblMemoryUsage.AutoSize = true;
            this.lblMemoryUsage.BackColor = System.Drawing.Color.LightYellow;
            this.lblMemoryUsage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMemoryUsage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMemoryUsage.Location = new System.Drawing.Point(3, 294);
            this.lblMemoryUsage.Margin = new System.Windows.Forms.Padding(3);
            this.lblMemoryUsage.Name = "lblMemoryUsage";
            this.lblMemoryUsage.Size = new System.Drawing.Size(234, 91);
            this.lblMemoryUsage.TabIndex = 3;
            this.lblMemoryUsage.Text = "内存使用: 0 MB";
            this.lblMemoryUsage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCpuUsage
            // 
            this.lblCpuUsage.AutoSize = true;
            this.lblCpuUsage.BackColor = System.Drawing.Color.LightSalmon;
            this.lblCpuUsage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCpuUsage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCpuUsage.Location = new System.Drawing.Point(3, 391);
            this.lblCpuUsage.Margin = new System.Windows.Forms.Padding(3);
            this.lblCpuUsage.Name = "lblCpuUsage";
            this.lblCpuUsage.Size = new System.Drawing.Size(234, 91);
            this.lblCpuUsage.TabIndex = 4;
            this.lblCpuUsage.Text = "CPU使用率: 0%";
            this.lblCpuUsage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDataRate
            // 
            this.lblDataRate.AutoSize = true;
            this.lblDataRate.BackColor = System.Drawing.Color.LightCyan;
            this.lblDataRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDataRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDataRate.Location = new System.Drawing.Point(3, 488);
            this.lblDataRate.Margin = new System.Windows.Forms.Padding(3);
            this.lblDataRate.Name = "lblDataRate";
            this.lblDataRate.Size = new System.Drawing.Size(234, 91);
            this.lblDataRate.TabIndex = 5;
            this.lblDataRate.Text = "数据采集速率: 0 条/秒";
            this.lblDataRate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IoT设备监控系统 - ScottPlot5";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevices)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panelChartControls.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvDevices;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExportData;
        private System.Windows.Forms.Button btnResetChart;
        private System.Windows.Forms.Button btnStopMonitoring;
        private System.Windows.Forms.Button btnStartMonitoring;
        private System.Windows.Forms.GroupBox groupBox2;
        private ScottPlot.WinForms.FormsPlot formsPlot;
        private System.Windows.Forms.Panel panelChartControls;
        private System.Windows.Forms.Button btnSaveChart;
        private System.Windows.Forms.Button btnAutoScale;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblTotalDevices;
        private System.Windows.Forms.Label lblOnlineDevices;
        private System.Windows.Forms.Label lblErrorDevices;
        private System.Windows.Forms.Label lblMemoryUsage;
        private System.Windows.Forms.Label lblCpuUsage;
        private System.Windows.Forms.Label lblDataRate;
    }
}
