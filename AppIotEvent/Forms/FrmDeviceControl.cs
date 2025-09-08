using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppIotEvent.Devices;
using AppIotEvent.EventBus;
using AppIotEvent.Events;

namespace AppIotEvent.Forms
{
    public partial class FrmDeviceControl : Form
    {
        private readonly IDevice _device;
        private readonly IEventBus _eventBus;
        public FrmDeviceControl(IDevice device, IEventBus eventBus)
        {
            InitializeComponent();
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

            InitializeForm();
        }
        private void InitializeForm()
        {
            this.Text = $"设备控制 - {_device.DeviceName}";
            deviceIdLabel.Text = _device.DeviceId;
            deviceNameLabel.Text = _device.DeviceName;
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            connectionStatusLabel.Text = _device.IsConnected ? "已连接" : "未连接";
            connectionStatusLabel.ForeColor = _device.IsConnected ? System.Drawing.Color.Green : System.Drawing.Color.Red;

            runningStatusLabel.Text = _device.IsRunning ? "运行中" : "已停止";
            runningStatusLabel.ForeColor = _device.IsRunning ? System.Drawing.Color.Green : System.Drawing.Color.Red;

            connectButton.Enabled = !_device.IsConnected;
            disconnectButton.Enabled = _device.IsConnected;
            startButton.Enabled = _device.IsConnected && !_device.IsRunning;
            stopButton.Enabled = _device.IsRunning;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            try
            {
                _device.Connect();
                _eventBus.Publish(new UserActionEvent("ConnectDevice", _device.DeviceId, _device.DeviceName));
                UpdateStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"连接设备失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                _device.Disconnect();
                _eventBus.Publish(new UserActionEvent("DisconnectDevice", _device.DeviceId, _device.DeviceName));
                UpdateStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"断开设备失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                _device.Start();
                _eventBus.Publish(new UserActionEvent("StartDevice", _device.DeviceId, _device.DeviceName));
                UpdateStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动设备失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            try
            {
                _device.Stop();
                _eventBus.Publish(new UserActionEvent("StopDevice", _device.DeviceId, _device.DeviceName));
                UpdateStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止设备失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            UpdateStatus();
            _eventBus.Publish(new UserActionEvent("RefreshDeviceStatus", _device.DeviceId, _device.DeviceName));
        }
    }
}
