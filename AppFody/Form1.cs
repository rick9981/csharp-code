using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AppFody
{
    public partial class Form1 : Form
    {
        private MainViewModel _viewModel;

        public Form1()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            SetupDataBindings();
            SetupEventHandlers();
        }

        private void SetupDataBindings()
        {
            // 基础状态绑定  
            lblDeviceStatus.DataBindings.Add("Text", _viewModel, nameof(_viewModel.DeviceStatus),
                false, DataSourceUpdateMode.OnPropertyChanged);

            lblTemperature.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Temperature),
                true, DataSourceUpdateMode.OnPropertyChanged, "0", "N1");

            lblPressure.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Pressure),
                true, DataSourceUpdateMode.OnPropertyChanged, "0", "N2");

            lblMotorSpeed.DataBindings.Add("Text", _viewModel, nameof(_viewModel.MotorSpeed),
                false, DataSourceUpdateMode.OnPropertyChanged);

            lblLastUpdate.DataBindings.Add("Text", _viewModel, nameof(_viewModel.LastUpdate),
                true, DataSourceUpdateMode.OnPropertyChanged, null, "HH:mm:ss.fff");

            lblErrorMessage.DataBindings.Add("Text", _viewModel, nameof(_viewModel.ErrorMessage),
                false, DataSourceUpdateMode.OnPropertyChanged);

            // 新增参数绑定  
            lblVoltage.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Voltage),
                true, DataSourceUpdateMode.OnPropertyChanged, "0", "N1");

            lblCurrent.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Current),
                true, DataSourceUpdateMode.OnPropertyChanged, "0", "N1");

            lblPower.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Power),
                true, DataSourceUpdateMode.OnPropertyChanged, "0", "N2");

            lblVibration.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Vibration),
                true, DataSourceUpdateMode.OnPropertyChanged, "0", "N2");

            lblProductCount.DataBindings.Add("Text", _viewModel, nameof(_viewModel.ProductCount),
                false, DataSourceUpdateMode.OnPropertyChanged);

            lblEfficiency.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Efficiency),
                true, DataSourceUpdateMode.OnPropertyChanged, "0", "N1");

            lblAlarmStatus.DataBindings.Add("Text", _viewModel, nameof(_viewModel.AlarmStatus),
                false, DataSourceUpdateMode.OnPropertyChanged);

            // 数据采集状态绑定  
            lblDataCollectionStatus.DataBindings.Add("Text", _viewModel, nameof(_viewModel.DataCollectionStatus),
                false, DataSourceUpdateMode.OnPropertyChanged);

            nudCollectionInterval.DataBindings.Add("Value", _viewModel, nameof(_viewModel.CollectionInterval),
                false, DataSourceUpdateMode.OnPropertyChanged);

            // 按钮状态绑定  
            btnConnect.DataBindings.Add("Enabled", _viewModel, nameof(_viewModel.CanConnect),
                false, DataSourceUpdateMode.OnPropertyChanged);

            btnDisconnect.DataBindings.Add("Enabled", _viewModel, nameof(_viewModel.CanDisconnect),
                false, DataSourceUpdateMode.OnPropertyChanged);

            btnStart.DataBindings.Add("Enabled", _viewModel, nameof(_viewModel.CanStart),
                false, DataSourceUpdateMode.OnPropertyChanged);

            btnStop.DataBindings.Add("Enabled", _viewModel, nameof(_viewModel.CanStop),
                false, DataSourceUpdateMode.OnPropertyChanged);

            btnReset.DataBindings.Add("Enabled", _viewModel, nameof(_viewModel.CanReset),
                false, DataSourceUpdateMode.OnPropertyChanged);

            btnStartDataCollection.DataBindings.Add("Enabled", _viewModel, nameof(_viewModel.CanStartDataCollection),
                false, DataSourceUpdateMode.OnPropertyChanged);

            btnStopDataCollection.DataBindings.Add("Enabled", _viewModel, nameof(_viewModel.CanStopDataCollection),
                false, DataSourceUpdateMode.OnPropertyChanged);

            // 订阅 PropertyChanged 事件以更新 UI 样式  
            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // 确保在UI线程上执行  
            if (InvokeRequired)
            {
                // 使用BeginInvoke提高性能，避免阻塞  
                BeginInvoke(new Action<object, PropertyChangedEventArgs>(OnViewModelPropertyChanged), sender, e);
                return;
            }

            try
            {
                switch (e.PropertyName)
                {
                    case nameof(_viewModel.DeviceStatus):
                        UpdateStatusColor();
                        break;
                    case nameof(_viewModel.IsRunning):
                        UpdateRunningIndicator();
                        break;
                    case nameof(_viewModel.Temperature):
                        UpdateTemperatureColor();
                        break;
                    case nameof(_viewModel.AlarmStatus):
                        UpdateAlarmColor();
                        break;
                    case nameof(_viewModel.IsDataCollecting):
                        UpdateDataCollectionIndicator();
                        break;
                    case nameof(_viewModel.Vibration):
                        UpdateVibrationColor();
                        break;
                }
            }
            catch (Exception ex)
            {
                // 防止UI更新错误影响整个应用  
                Console.WriteLine($"UI更新错误: {ex.Message}");
            }
        }

        private void UpdateStatusColor()
        {
            if (lblDeviceStatus.IsDisposed) return;

            switch (_viewModel.DeviceStatus)
            {
                case "离线":
                case "连接失败":
                    lblDeviceStatus.ForeColor = Color.Gray;
                    break;
                case "连接中...":
                case "启动中...":
                case "停止中...":
                    lblDeviceStatus.ForeColor = Color.Orange;
                    break;
                case "已连接":
                case "已停止":
                case "已重置":
                    lblDeviceStatus.ForeColor = Color.Blue;
                    break;
                case "运行中":
                    lblDeviceStatus.ForeColor = Color.Green;
                    break;
                default:
                    lblDeviceStatus.ForeColor = Color.Black;
                    break;
            }
        }

        private void UpdateRunningIndicator()
        {
            if (this.IsDisposed) return;

            if (_viewModel.IsRunning)
            {
                this.Text = "MVVM设备监控 v1.0 - 运行中";
            }
            else
            {
                this.Text = "MVVM设备监控 v1.0";
            }
        }

        private void UpdateTemperatureColor()
        {
            if (lblTemperature.IsDisposed) return;

            if (_viewModel.Temperature > 70)
                lblTemperature.ForeColor = Color.Red;
            else if (_viewModel.Temperature > 50)
                lblTemperature.ForeColor = Color.Orange;
            else if (_viewModel.Temperature > 30)
                lblTemperature.ForeColor = Color.Green;
            else
                lblTemperature.ForeColor = Color.Blue;
        }

        private void UpdateAlarmColor()
        {
            if (lblAlarmStatus.IsDisposed) return;

            switch (_viewModel.AlarmStatus)
            {
                case "正常":
                    lblAlarmStatus.ForeColor = Color.Green;
                    lblAlarmStatus.Font = new Font(lblAlarmStatus.Font, FontStyle.Regular);
                    break;
                case "温度报警":
                case "振动报警":
                case "压力报警":
                    lblAlarmStatus.ForeColor = Color.Red;
                    lblAlarmStatus.Font = new Font(lblAlarmStatus.Font, FontStyle.Bold);
                    break;
                default:
                    lblAlarmStatus.ForeColor = Color.Orange;
                    lblAlarmStatus.Font = new Font(lblAlarmStatus.Font, FontStyle.Regular);
                    break;
            }
        }

        private void UpdateDataCollectionIndicator()
        {
            if (lblDataCollectionStatus.IsDisposed || btnStartDataCollection.IsDisposed || btnStopDataCollection.IsDisposed)
                return;

            if (_viewModel.IsDataCollecting)
            {
                lblDataCollectionStatus.ForeColor = Color.Green;
                lblDataCollectionStatus.Font = new Font(lblDataCollectionStatus.Font, FontStyle.Bold);
                btnStartDataCollection.BackColor = Color.LightGray;
                btnStopDataCollection.BackColor = Color.LightCoral;
            }
            else
            {
                lblDataCollectionStatus.ForeColor = Color.Gray;
                lblDataCollectionStatus.Font = new Font(lblDataCollectionStatus.Font, FontStyle.Regular);
                btnStartDataCollection.BackColor = Color.LightBlue;
                btnStopDataCollection.BackColor = Color.LightGray;
            }
        }

        private void UpdateVibrationColor()
        {
            if (lblVibration.IsDisposed) return;

            if (_viewModel.Vibration > 2.0)
                lblVibration.ForeColor = Color.Red;
            else if (_viewModel.Vibration > 1.5)
                lblVibration.ForeColor = Color.Orange;
            else
                lblVibration.ForeColor = Color.Green;
        }

        private void SetupEventHandlers()
        {
            btnConnect.Click += (s, e) => ExecuteCommand(_viewModel.ConnectCommand);
            btnDisconnect.Click += (s, e) => ExecuteCommand(_viewModel.DisconnectCommand);
            btnStart.Click += (s, e) => ExecuteCommand(_viewModel.StartCommand);
            btnStop.Click += (s, e) => ExecuteCommand(_viewModel.StopCommand);
            btnReset.Click += (s, e) => ExecuteCommand(_viewModel.ResetCommand);
            btnStartDataCollection.Click += (s, e) => ExecuteCommand(_viewModel.StartDataCollectionCommand);
            btnStopDataCollection.Click += (s, e) => ExecuteCommand(_viewModel.StopDataCollectionCommand);

            // 采集间隔变化事件
            nudCollectionInterval.ValueChanged += (s, e) =>
            {
                if (!nudCollectionInterval.IsDisposed)
                {
                    _viewModel.CollectionInterval = (int)nudCollectionInterval.Value;
                }
            };
        }

        private void ExecuteCommand(System.Windows.Input.ICommand command)
        {
            try
            {
                if (command?.CanExecute(null) == true)
                {
                    command.Execute(null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"命令执行错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                if (_viewModel != null)
                {
                    _viewModel.PropertyChanged -= OnViewModelPropertyChanged;

                    if (_viewModel.IsConnected)
                    {
                        _viewModel.DisconnectCommand?.Execute(null);
                    }

                    // 释放ViewModel资源
                    _viewModel.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"窗体关闭错误: {ex.Message}");
            }

            base.OnFormClosing(e);
        }
    }
}