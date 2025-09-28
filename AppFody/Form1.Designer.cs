namespace AppFody
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
            this.grpConnection = new System.Windows.Forms.GroupBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.grpStatus = new System.Windows.Forms.GroupBox();
            this.lblDeviceStatusTitle = new System.Windows.Forms.Label();
            this.lblDeviceStatus = new System.Windows.Forms.Label();
            this.lblTemperatureTitle = new System.Windows.Forms.Label();
            this.lblTemperature = new System.Windows.Forms.Label();
            this.lblPressureTitle = new System.Windows.Forms.Label();
            this.lblPressure = new System.Windows.Forms.Label();
            this.lblMotorSpeedTitle = new System.Windows.Forms.Label();
            this.lblMotorSpeed = new System.Windows.Forms.Label();
            this.lblLastUpdateTitle = new System.Windows.Forms.Label();
            this.lblLastUpdate = new System.Windows.Forms.Label();
            this.lblAlarmStatusTitle = new System.Windows.Forms.Label();
            this.lblAlarmStatus = new System.Windows.Forms.Label();

            this.grpControl = new System.Windows.Forms.GroupBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();

            this.grpAdvancedParams = new System.Windows.Forms.GroupBox();
            this.lblVoltageTitle = new System.Windows.Forms.Label();
            this.lblVoltage = new System.Windows.Forms.Label();
            this.lblCurrentTitle = new System.Windows.Forms.Label();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.lblPowerTitle = new System.Windows.Forms.Label();
            this.lblPower = new System.Windows.Forms.Label();
            this.lblVibrationTitle = new System.Windows.Forms.Label();
            this.lblVibration = new System.Windows.Forms.Label();
            this.lblProductCountTitle = new System.Windows.Forms.Label();
            this.lblProductCount = new System.Windows.Forms.Label();
            this.lblEfficiencyTitle = new System.Windows.Forms.Label();
            this.lblEfficiency = new System.Windows.Forms.Label();

            this.grpDataCollection = new System.Windows.Forms.GroupBox();
            this.btnStartDataCollection = new System.Windows.Forms.Button();
            this.btnStopDataCollection = new System.Windows.Forms.Button();
            this.lblDataCollectionStatusTitle = new System.Windows.Forms.Label();
            this.lblDataCollectionStatus = new System.Windows.Forms.Label();
            this.lblCollectionIntervalTitle = new System.Windows.Forms.Label();
            this.nudCollectionInterval = new System.Windows.Forms.NumericUpDown();

            this.lblErrorMessage = new System.Windows.Forms.Label();

            this.grpConnection.SuspendLayout();
            this.grpStatus.SuspendLayout();
            this.grpControl.SuspendLayout();
            this.grpAdvancedParams.SuspendLayout();
            this.grpDataCollection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCollectionInterval)).BeginInit();
            this.SuspendLayout();

            // grpConnection
            this.grpConnection.Controls.Add(this.btnDisconnect);
            this.grpConnection.Controls.Add(this.btnConnect);
            this.grpConnection.Location = new System.Drawing.Point(12, 12);
            this.grpConnection.Name = "grpConnection";
            this.grpConnection.Size = new System.Drawing.Size(200, 80);
            this.grpConnection.TabIndex = 0;
            this.grpConnection.TabStop = false;
            this.grpConnection.Text = "连接控制";

            // btnConnect
            this.btnConnect.Location = new System.Drawing.Point(15, 25);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 35);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;

            // btnDisconnect
            this.btnDisconnect.Location = new System.Drawing.Point(105, 25);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 35);
            this.btnDisconnect.TabIndex = 1;
            this.btnDisconnect.Text = "断开";
            this.btnDisconnect.UseVisualStyleBackColor = true;

            // grpStatus
            this.grpStatus.Controls.Add(this.lblAlarmStatus);
            this.grpStatus.Controls.Add(this.lblAlarmStatusTitle);
            this.grpStatus.Controls.Add(this.lblLastUpdate);
            this.grpStatus.Controls.Add(this.lblLastUpdateTitle);
            this.grpStatus.Controls.Add(this.lblMotorSpeed);
            this.grpStatus.Controls.Add(this.lblMotorSpeedTitle);
            this.grpStatus.Controls.Add(this.lblPressure);
            this.grpStatus.Controls.Add(this.lblPressureTitle);
            this.grpStatus.Controls.Add(this.lblTemperature);
            this.grpStatus.Controls.Add(this.lblTemperatureTitle);
            this.grpStatus.Controls.Add(this.lblDeviceStatus);
            this.grpStatus.Controls.Add(this.lblDeviceStatusTitle);
            this.grpStatus.Location = new System.Drawing.Point(12, 110);
            this.grpStatus.Name = "grpStatus";
            this.grpStatus.Size = new System.Drawing.Size(350, 200);
            this.grpStatus.TabIndex = 1;
            this.grpStatus.TabStop = false;
            this.grpStatus.Text = "设备状态";

            // 基础参数标签
            this.lblDeviceStatusTitle.AutoSize = true;
            this.lblDeviceStatusTitle.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblDeviceStatusTitle.Location = new System.Drawing.Point(15, 25);
            this.lblDeviceStatusTitle.Name = "lblDeviceStatusTitle";
            this.lblDeviceStatusTitle.Size = new System.Drawing.Size(68, 17);
            this.lblDeviceStatusTitle.TabIndex = 0;
            this.lblDeviceStatusTitle.Text = "设备状态：";

            this.lblDeviceStatus.AutoSize = true;
            this.lblDeviceStatus.ForeColor = System.Drawing.Color.Blue;
            this.lblDeviceStatus.Location = new System.Drawing.Point(90, 25);
            this.lblDeviceStatus.Name = "lblDeviceStatus";
            this.lblDeviceStatus.Size = new System.Drawing.Size(29, 12);
            this.lblDeviceStatus.TabIndex = 1;
            this.lblDeviceStatus.Text = "离线";

            this.lblTemperatureTitle.AutoSize = true;
            this.lblTemperatureTitle.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblTemperatureTitle.Location = new System.Drawing.Point(15, 50);
            this.lblTemperatureTitle.Name = "lblTemperatureTitle";
            this.lblTemperatureTitle.Size = new System.Drawing.Size(68, 17);
            this.lblTemperatureTitle.TabIndex = 2;
            this.lblTemperatureTitle.Text = "温度(°C)：";

            this.lblTemperature.AutoSize = true;
            this.lblTemperature.ForeColor = System.Drawing.Color.Blue;
            this.lblTemperature.Location = new System.Drawing.Point(90, 50);
            this.lblTemperature.Name = "lblTemperature";
            this.lblTemperature.Size = new System.Drawing.Size(23, 12);
            this.lblTemperature.TabIndex = 3;
            this.lblTemperature.Text = "0.0";

            this.lblPressureTitle.AutoSize = true;
            this.lblPressureTitle.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblPressureTitle.Location = new System.Drawing.Point(15, 75);
            this.lblPressureTitle.Name = "lblPressureTitle";
            this.lblPressureTitle.Size = new System.Drawing.Size(70, 17);
            this.lblPressureTitle.TabIndex = 4;
            this.lblPressureTitle.Text = "压力(MPa)：";

            this.lblPressure.AutoSize = true;
            this.lblPressure.ForeColor = System.Drawing.Color.Green;
            this.lblPressure.Location = new System.Drawing.Point(90, 75);
            this.lblPressure.Name = "lblPressure";
            this.lblPressure.Size = new System.Drawing.Size(23, 12);
            this.lblPressure.TabIndex = 5;
            this.lblPressure.Text = "0.0";

            this.lblMotorSpeedTitle.AutoSize = true;
            this.lblMotorSpeedTitle.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblMotorSpeedTitle.Location = new System.Drawing.Point(15, 100);
            this.lblMotorSpeedTitle.Name = "lblMotorSpeedTitle";
            this.lblMotorSpeedTitle.Size = new System.Drawing.Size(68, 17);
            this.lblMotorSpeedTitle.TabIndex = 6;
            this.lblMotorSpeedTitle.Text = "转速(rpm)：";

            this.lblMotorSpeed.AutoSize = true;
            this.lblMotorSpeed.ForeColor = System.Drawing.Color.Purple;
            this.lblMotorSpeed.Location = new System.Drawing.Point(90, 100);
            this.lblMotorSpeed.Name = "lblMotorSpeed";
            this.lblMotorSpeed.Size = new System.Drawing.Size(11, 12);
            this.lblMotorSpeed.TabIndex = 7;
            this.lblMotorSpeed.Text = "0";

            this.lblAlarmStatusTitle.AutoSize = true;
            this.lblAlarmStatusTitle.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblAlarmStatusTitle.Location = new System.Drawing.Point(15, 125);
            this.lblAlarmStatusTitle.Name = "lblAlarmStatusTitle";
            this.lblAlarmStatusTitle.Size = new System.Drawing.Size(68, 17);
            this.lblAlarmStatusTitle.TabIndex = 8;
            this.lblAlarmStatusTitle.Text = "报警状态：";

            this.lblAlarmStatus.AutoSize = true;
            this.lblAlarmStatus.ForeColor = System.Drawing.Color.Green;
            this.lblAlarmStatus.Location = new System.Drawing.Point(90, 125);
            this.lblAlarmStatus.Name = "lblAlarmStatus";
            this.lblAlarmStatus.Size = new System.Drawing.Size(29, 12);
            this.lblAlarmStatus.TabIndex = 9;
            this.lblAlarmStatus.Text = "正常";

            this.lblLastUpdateTitle.AutoSize = true;
            this.lblLastUpdateTitle.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblLastUpdateTitle.Location = new System.Drawing.Point(15, 170);
            this.lblLastUpdateTitle.Name = "lblLastUpdateTitle";
            this.lblLastUpdateTitle.Size = new System.Drawing.Size(68, 17);
            this.lblLastUpdateTitle.TabIndex = 10;
            this.lblLastUpdateTitle.Text = "最后更新：";

            this.lblLastUpdate.AutoSize = true;
            this.lblLastUpdate.ForeColor = System.Drawing.Color.Gray;
            this.lblLastUpdate.Location = new System.Drawing.Point(90, 170);
            this.lblLastUpdate.Name = "lblLastUpdate";
            this.lblLastUpdate.Size = new System.Drawing.Size(65, 12);
            this.lblLastUpdate.TabIndex = 11;
            this.lblLastUpdate.Text = "00:00:00.000";

            // grpControl
            this.grpControl.Controls.Add(this.btnReset);
            this.grpControl.Controls.Add(this.btnStop);
            this.grpControl.Controls.Add(this.btnStart);
            this.grpControl.Location = new System.Drawing.Point(230, 12);
            this.grpControl.Name = "grpControl";
            this.grpControl.Size = new System.Drawing.Size(250, 80);
            this.grpControl.TabIndex = 2;
            this.grpControl.TabStop = false;
            this.grpControl.Text = "设备控制";

            this.btnStart.BackColor = System.Drawing.Color.LightGreen;
            this.btnStart.Location = new System.Drawing.Point(15, 25);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(65, 35);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "启动";
            this.btnStart.UseVisualStyleBackColor = false;

            this.btnStop.BackColor = System.Drawing.Color.LightCoral;
            this.btnStop.Location = new System.Drawing.Point(95, 25);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(65, 35);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = false;

            this.btnReset.BackColor = System.Drawing.Color.LightYellow;
            this.btnReset.Location = new System.Drawing.Point(175, 25);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(65, 35);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "重置";
            this.btnReset.UseVisualStyleBackColor = false;

            // grpAdvancedParams
            this.grpAdvancedParams.Controls.Add(this.lblEfficiency);
            this.grpAdvancedParams.Controls.Add(this.lblEfficiencyTitle);
            this.grpAdvancedParams.Controls.Add(this.lblProductCount);
            this.grpAdvancedParams.Controls.Add(this.lblProductCountTitle);
            this.grpAdvancedParams.Controls.Add(this.lblVibration);
            this.grpAdvancedParams.Controls.Add(this.lblVibrationTitle);
            this.grpAdvancedParams.Controls.Add(this.lblPower);
            this.grpAdvancedParams.Controls.Add(this.lblPowerTitle);
            this.grpAdvancedParams.Controls.Add(this.lblCurrent);
            this.grpAdvancedParams.Controls.Add(this.lblCurrentTitle);
            this.grpAdvancedParams.Controls.Add(this.lblVoltage);
            this.grpAdvancedParams.Controls.Add(this.lblVoltageTitle);
            this.grpAdvancedParams.Location = new System.Drawing.Point(380, 110);
            this.grpAdvancedParams.Name = "grpAdvancedParams";
            this.grpAdvancedParams.Size = new System.Drawing.Size(280, 200);
            this.grpAdvancedParams.TabIndex = 3;
            this.grpAdvancedParams.TabStop = false;
            this.grpAdvancedParams.Text = "高级参数";

            // 高级参数标签
            this.lblVoltageTitle.AutoSize = true;
            this.lblVoltageTitle.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblVoltageTitle.Location = new System.Drawing.Point(15, 25);
            this.lblVoltageTitle.Name = "lblVoltageTitle";
            this.lblVoltageTitle.Size = new System.Drawing.Size(55, 17);
            this.lblVoltageTitle.TabIndex = 0;
            this.lblVoltageTitle.Text = "电压(V)：";

            this.lblVoltage.AutoSize = true;
            this.lblVoltage.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblVoltage.Location = new System.Drawing.Point(80, 25);
            this.lblVoltage.Name = "lblVoltage";
            this.lblVoltage.Size = new System.Drawing.Size(23, 12);
            this.lblVoltage.TabIndex = 1;
            this.lblVoltage.Text = "0.0";

            this.lblCurrentTitle.AutoSize = true;
            this.lblCurrentTitle.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblCurrentTitle.Location = new System.Drawing.Point(15, 50);
            this.lblCurrentTitle.Name = "lblCurrentTitle";
            this.lblCurrentTitle.Size = new System.Drawing.Size(55, 17);
            this.lblCurrentTitle.TabIndex = 2;
            this.lblCurrentTitle.Text = "电流(A)：";

            this.lblCurrent.AutoSize = true;
            this.lblCurrent.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblCurrent.Location = new System.Drawing.Point(80, 50);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(23, 12);
            this.lblCurrent.TabIndex = 3;
            this.lblCurrent.Text = "0.0";

            this.lblPowerTitle.AutoSize = true;
            this.lblPowerTitle.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblPowerTitle.Location = new System.Drawing.Point(15, 75);
            this.lblPowerTitle.Name = "lblPowerTitle";
            this.lblPowerTitle.Size = new System.Drawing.Size(60, 17);
            this.lblPowerTitle.TabIndex = 4;
            this.lblPowerTitle.Text = "功率(kW)：";

            this.lblPower.AutoSize = true;
            this.lblPower.ForeColor = System.Drawing.Color.DarkRed;
            this.lblPower.Location = new System.Drawing.Point(80, 75);
            this.lblPower.Name = "lblPower";
            this.lblPower.Size = new System.Drawing.Size(29, 12);
            this.lblPower.TabIndex = 5;
            this.lblPower.Text = "0.00";

            this.lblVibrationTitle.AutoSize = true;
            this.lblVibrationTitle.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblVibrationTitle.Location = new System.Drawing.Point(15, 100);
            this.lblVibrationTitle.Name = "lblVibrationTitle";
            this.lblVibrationTitle.Size = new System.Drawing.Size(80, 17);
            this.lblVibrationTitle.TabIndex = 6;
            this.lblVibrationTitle.Text = "振动(mm/s)：";

            this.lblVibration.AutoSize = true;
            this.lblVibration.ForeColor = System.Drawing.Color.Orange;
            this.lblVibration.Location = new System.Drawing.Point(100, 100);
            this.lblVibration.Name = "lblVibration";
            this.lblVibration.Size = new System.Drawing.Size(29, 12);
            this.lblVibration.TabIndex = 7;
            this.lblVibration.Text = "0.00";

            this.lblProductCountTitle.AutoSize = true;
            this.lblProductCountTitle.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblProductCountTitle.Location = new System.Drawing.Point(15, 125);
            this.lblProductCountTitle.Name = "lblProductCountTitle";
            this.lblProductCountTitle.Size = new System.Drawing.Size(68, 17);
            this.lblProductCountTitle.TabIndex = 8;
            this.lblProductCountTitle.Text = "产品计数：";

            this.lblProductCount.AutoSize = true;
            this.lblProductCount.ForeColor = System.Drawing.Color.DarkMagenta;
            this.lblProductCount.Location = new System.Drawing.Point(90, 125);
            this.lblProductCount.Name = "lblProductCount";
            this.lblProductCount.Size = new System.Drawing.Size(11, 12);
            this.lblProductCount.TabIndex = 9;
            this.lblProductCount.Text = "0";

            this.lblEfficiencyTitle.AutoSize = true;
            this.lblEfficiencyTitle.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
            this.lblEfficiencyTitle.Location = new System.Drawing.Point(15, 150);
            this.lblEfficiencyTitle.Name = "lblEfficiencyTitle";
            this.lblEfficiencyTitle.Size = new System.Drawing.Size(60, 17);
            this.lblEfficiencyTitle.TabIndex = 10;
            this.lblEfficiencyTitle.Text = "效率(%)：";

            this.lblEfficiency.AutoSize = true;
            this.lblEfficiency.ForeColor = System.Drawing.Color.DarkCyan;
            this.lblEfficiency.Location = new System.Drawing.Point(80, 150);
            this.lblEfficiency.Name = "lblEfficiency";
            this.lblEfficiency.Size = new System.Drawing.Size(23, 12);
            this.lblEfficiency.TabIndex = 11;
            this.lblEfficiency.Text = "0.0";

            // grpDataCollection
            this.grpDataCollection.Controls.Add(this.nudCollectionInterval);
            this.grpDataCollection.Controls.Add(this.lblCollectionIntervalTitle);
            this.grpDataCollection.Controls.Add(this.lblDataCollectionStatus);
            this.grpDataCollection.Controls.Add(this.lblDataCollectionStatusTitle);
            this.grpDataCollection.Controls.Add(this.btnStopDataCollection);
            this.grpDataCollection.Controls.Add(this.btnStartDataCollection);
            this.grpDataCollection.Location = new System.Drawing.Point(500, 12);
            this.grpDataCollection.Name = "grpDataCollection";
            this.grpDataCollection.Size = new System.Drawing.Size(280, 80);
            this.grpDataCollection.TabIndex = 4;
            this.grpDataCollection.TabStop = false;
            this.grpDataCollection.Text = "数据采集控制";

            this.btnStartDataCollection.BackColor = System.Drawing.Color.LightBlue;
            this.btnStartDataCollection.Location = new System.Drawing.Point(15, 20);
            this.btnStartDataCollection.Name = "btnStartDataCollection";
            this.btnStartDataCollection.Size = new System.Drawing.Size(80, 25);
            this.btnStartDataCollection.TabIndex = 0;
            this.btnStartDataCollection.Text = "开始采集";
            this.btnStartDataCollection.UseVisualStyleBackColor = false;

            this.btnStopDataCollection.BackColor = System.Drawing.Color.LightGray;
            this.btnStopDataCollection.Location = new System.Drawing.Point(105, 20);
            this.btnStopDataCollection.Name = "btnStopDataCollection";
            this.btnStopDataCollection.Size = new System.Drawing.Size(80, 25);
            this.btnStopDataCollection.TabIndex = 1;
            this.btnStopDataCollection.Text = "停止采集";
            this.btnStopDataCollection.UseVisualStyleBackColor = false;

            this.lblDataCollectionStatusTitle.AutoSize = true;
            this.lblDataCollectionStatusTitle.Font = new System.Drawing.Font("Microsoft YaHei", 8F, System.Drawing.FontStyle.Bold);
            this.lblDataCollectionStatusTitle.Location = new System.Drawing.Point(15, 55);
            this.lblDataCollectionStatusTitle.Name = "lblDataCollectionStatusTitle";
            this.lblDataCollectionStatusTitle.Size = new System.Drawing.Size(60, 16);
            this.lblDataCollectionStatusTitle.TabIndex = 2;
            this.lblDataCollectionStatusTitle.Text = "采集状态：";

            this.lblDataCollectionStatus.AutoSize = true;
            this.lblDataCollectionStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblDataCollectionStatus.Location = new System.Drawing.Point(80, 55);
            this.lblDataCollectionStatus.Name = "lblDataCollectionStatus";
            this.lblDataCollectionStatus.Size = new System.Drawing.Size(41, 12);
            this.lblDataCollectionStatus.TabIndex = 3;
            this.lblDataCollectionStatus.Text = "未启动";

            this.lblCollectionIntervalTitle.AutoSize = true;
            this.lblCollectionIntervalTitle.Font = new System.Drawing.Font("Microsoft YaHei", 8F, System.Drawing.FontStyle.Bold);
            this.lblCollectionIntervalTitle.Location = new System.Drawing.Point(195, 25);
            this.lblCollectionIntervalTitle.Name = "lblCollectionIntervalTitle";
            this.lblCollectionIntervalTitle.Size = new System.Drawing.Size(70, 16);
            this.lblCollectionIntervalTitle.TabIndex = 4;
            this.lblCollectionIntervalTitle.Text = "间隔(ms)：";

            this.nudCollectionInterval.Location = new System.Drawing.Point(195, 45);
            this.nudCollectionInterval.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.nudCollectionInterval.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            this.nudCollectionInterval.Name = "nudCollectionInterval";
            this.nudCollectionInterval.Size = new System.Drawing.Size(70, 21);
            this.nudCollectionInterval.TabIndex = 5;
            this.nudCollectionInterval.Value = new decimal(new int[] { 1000, 0, 0, 0 });

            // lblErrorMessage
            this.lblErrorMessage.ForeColor = System.Drawing.Color.Red;
            this.lblErrorMessage.Location = new System.Drawing.Point(12, 330);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(770, 30);
            this.lblErrorMessage.TabIndex = 5;
            this.lblErrorMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 371);
            this.Controls.Add(this.lblErrorMessage);
            this.Controls.Add(this.grpDataCollection);
            this.Controls.Add(this.grpAdvancedParams);
            this.Controls.Add(this.grpControl);
            this.Controls.Add(this.grpStatus);
            this.Controls.Add(this.grpConnection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MVVM设备监控";
            this.grpConnection.ResumeLayout(false);
            this.grpStatus.ResumeLayout(false);
            this.grpStatus.PerformLayout();
            this.grpControl.ResumeLayout(false);
            this.grpAdvancedParams.ResumeLayout(false);
            this.grpAdvancedParams.PerformLayout();
            this.grpDataCollection.ResumeLayout(false);
            this.grpDataCollection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCollectionInterval)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.GroupBox grpConnection;
        private System.Windows.Forms.GroupBox grpStatus;
        private System.Windows.Forms.GroupBox grpControl;
        private System.Windows.Forms.GroupBox grpDataCollection;
        private System.Windows.Forms.GroupBox grpAdvancedParams;

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnStartDataCollection;
        private System.Windows.Forms.Button btnStopDataCollection;

        private System.Windows.Forms.Label lblDeviceStatusTitle;
        private System.Windows.Forms.Label lblDeviceStatus;
        private System.Windows.Forms.Label lblTemperatureTitle;
        private System.Windows.Forms.Label lblTemperature;
        private System.Windows.Forms.Label lblPressureTitle;
        private System.Windows.Forms.Label lblPressure;
        private System.Windows.Forms.Label lblMotorSpeedTitle;
        private System.Windows.Forms.Label lblMotorSpeed;
        private System.Windows.Forms.Label lblLastUpdateTitle;
        private System.Windows.Forms.Label lblLastUpdate;

        // 新增参数标签  
        private System.Windows.Forms.Label lblVoltageTitle;
        private System.Windows.Forms.Label lblVoltage;
        private System.Windows.Forms.Label lblCurrentTitle;
        private System.Windows.Forms.Label lblCurrent;
        private System.Windows.Forms.Label lblPowerTitle;
        private System.Windows.Forms.Label lblPower;
        private System.Windows.Forms.Label lblVibrationTitle;
        private System.Windows.Forms.Label lblVibration;
        private System.Windows.Forms.Label lblProductCountTitle;
        private System.Windows.Forms.Label lblProductCount;
        private System.Windows.Forms.Label lblEfficiencyTitle;
        private System.Windows.Forms.Label lblEfficiency;
        private System.Windows.Forms.Label lblAlarmStatusTitle;
        private System.Windows.Forms.Label lblAlarmStatus;

        // 数据采集控件  
        private System.Windows.Forms.Label lblDataCollectionStatusTitle;
        private System.Windows.Forms.Label lblDataCollectionStatus;
        private System.Windows.Forms.Label lblCollectionIntervalTitle;
        private System.Windows.Forms.NumericUpDown nudCollectionInterval;

        private System.Windows.Forms.Label lblErrorMessage;
    }
}
