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
using AppIotEvent.Services;

namespace AppIotEvent.Forms
{
    public partial class FrmMain : Form
    {
        private readonly IEventBus _eventBus;
        private readonly DeviceManager _deviceManager;
        private readonly DataLogger _dataLogger;
        private readonly Dictionary<string, double> _latestDeviceData;
        public FrmMain()
        {
            InitializeComponent();
            _eventBus = new Events.EventBus();
            _deviceManager = new DeviceManager(_eventBus);
            _dataLogger = new DataLogger(_eventBus);
            _latestDeviceData = new Dictionary<string, double>();

            SubscribeToEvents();
            InitializeDevices();
            SetupUI();

        }

        private void SubscribeToEvents()
        {
            _eventBus.Subscribe<DeviceDataUpdatedEvent>(OnDeviceDataUpdated);
            _eventBus.Subscribe<DeviceConnectionChangedEvent>(OnDeviceConnectionChanged);
            _eventBus.Subscribe<DeviceAlarmEvent>(OnDeviceAlarm);
            _eventBus.Subscribe<SystemLogEvent>(OnSystemLog);
        }

        private void InitializeDevices()
        {
            // 添加温度传感器
            var tempSensor = new TemperatureSensor("TEMP_001", "车间温度传感器");
            _deviceManager.AddDevice(tempSensor);

            // 添加湿度传感器
            var humiditySensor = new HumiditySensor("HUMI_001", "车间湿度传感器");
            _deviceManager.AddDevice(humiditySensor);

            UpdateDeviceList();
        }

        private void SetupUI()
        {
            // 设置状态栏
            statusLabel.Text = "系统就绪";

            // 设置设备列表列头
            deviceListView.View = View.Details;
            deviceListView.FullRowSelect = true;
            deviceListView.GridLines = true;
        }

        private void UpdateDeviceList()
        {
            deviceListView.Items.Clear();

            foreach (var device in _deviceManager.Devices)
            {
                var item = new ListViewItem(device.DeviceId);
                item.SubItems.Add(device.DeviceName);
                item.SubItems.Add(device.IsConnected ? "已连接" : "未连接");
                item.SubItems.Add(device.IsRunning ? "运行中" : "已停止");
                item.SubItems.Add(_latestDeviceData.ContainsKey(device.DeviceId)
                    ? _latestDeviceData[device.DeviceId].ToString("F1")
                    : "无数据");

                item.BackColor = device.IsConnected ? Color.LightGreen : Color.LightGray;
                item.Tag = device;

                deviceListView.Items.Add(item);
            }
        }

        private void OnDeviceDataUpdated(DeviceDataUpdatedEvent eventData)
        {
            _latestDeviceData[eventData.DeviceId] = eventData.Value;
            UpdateDeviceList();

            // 添加到数据日志
            var logItem = new ListViewItem(eventData.Timestamp.ToString("HH:mm:ss"));
            logItem.SubItems.Add(eventData.DeviceName);
            logItem.SubItems.Add(eventData.DataType);
            logItem.SubItems.Add($"{eventData.Value:F1} {eventData.Unit}");

            dataLogListView.Items.Insert(0, logItem);

            // 限制显示条数
            while (dataLogListView.Items.Count > 100)
            {
                dataLogListView.Items.RemoveAt(dataLogListView.Items.Count - 1);
            }
        }

        private void OnDeviceConnectionChanged(DeviceConnectionChangedEvent eventData)
        {
            UpdateDeviceList();
            var message = $"设备 {eventData.DeviceName} {(eventData.IsConnected ? "已连接" : "已断开")}";
            statusLabel.Text = message;
        }

        private void OnDeviceAlarm(DeviceAlarmEvent eventData)
        {
            var logItem = new ListViewItem(eventData.Timestamp.ToString("HH:mm:ss"));
            logItem.SubItems.Add(eventData.DeviceName);
            logItem.SubItems.Add(eventData.Level.ToString());
            logItem.SubItems.Add(eventData.AlarmMessage);

            // 根据报警级别设置颜色
            logItem.BackColor = eventData.Level switch
            {
                AlarmLevel.Warning => Color.Yellow,
                AlarmLevel.Error => Color.Orange,
                AlarmLevel.Critical => Color.Red,
                _ => Color.White
            };

            alarmListView.Items.Insert(0, logItem);

            // 限制显示条数
            while (alarmListView.Items.Count > 50)
            {
                alarmListView.Items.RemoveAt(alarmListView.Items.Count - 1);
            }

            // 显示气泡提示
            notifyIcon.ShowBalloonTip(3000, "设备报警",
                $"{eventData.DeviceName}: {eventData.AlarmMessage}",
                ToolTipIcon.Warning);
        }

        private void OnSystemLog(SystemLogEvent eventData)
        {
            var logItem = new ListViewItem(eventData.Timestamp.ToString("HH:mm:ss.fff"));
            logItem.SubItems.Add(eventData.Level.ToString());
            logItem.SubItems.Add(eventData.Message);

            systemLogListView.Items.Insert(0, logItem);

            // 限制显示条数
            while (systemLogListView.Items.Count > 200)
            {
                systemLogListView.Items.RemoveAt(systemLogListView.Items.Count - 1);
            }
        }

        private void startAllButton_Click(object sender, EventArgs e)
        {
            _deviceManager.StartAllDevices();
            _eventBus.Publish(new UserActionEvent("StartAllDevices", "DeviceManager"));
        }

        private void stopAllButton_Click(object sender, EventArgs e)
        {
            _deviceManager.StopAllDevices();
            _eventBus.Publish(new UserActionEvent("StopAllDevices", "DeviceManager"));
        }

        private void deviceControlButton_Click(object sender, EventArgs e)
        {
            if (deviceListView.SelectedItems.Count > 0)
            {
                var device = (IDevice)deviceListView.SelectedItems[0].Tag;
                var controlForm = new FrmDeviceControl(device, _eventBus);
                controlForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("请先选择一个设备", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void clearLogsButton_Click(object sender, EventArgs e)
        {
            dataLogListView.Items.Clear();
            alarmListView.Items.Clear();
            systemLogListView.Items.Clear();
            _eventBus.Publish(new UserActionEvent("ClearLogs", "MainForm"));
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _deviceManager?.Dispose();
            _dataLogger?.Dispose();
            _eventBus?.Clear();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon.ShowBalloonTip(2000, "IoT监控系统", "程序已最小化到系统托盘", ToolTipIcon.Info);
            }
        }
    }
}
