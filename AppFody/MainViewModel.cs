using PropertyChanged;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Threading;

namespace AppFody
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel : INotifyPropertyChanged
    {
        // 同步上下文，用于线程安全的属性更新
        private readonly SynchronizationContext _syncContext;
        private CancellationTokenSource _cancellationTokenSource;

        // 设备状态属性
        public string DeviceStatus { get; set; } = "离线";
        public double Temperature { get; set; } = 25.0;
        public double Pressure { get; set; } = 0.0;
        public int MotorSpeed { get; set; } = 0;
        public bool IsRunning { get; set; } = false;
        public bool IsConnected { get; set; } = false;
        public string ErrorMessage { get; set; } = "";
        public DateTime LastUpdate { get; set; } = DateTime.Now;


        public double Voltage { get; set; } = 0.0; 
        public double Current { get; set; } = 0.0;  
        public double Power { get; set; } = 0.0;  
        public double Vibration { get; set; } = 0.0;
        public int ProductCount { get; set; } = 0;   
        public double Efficiency { get; set; } = 0.0;   
        public string AlarmStatus { get; set; } = "正常";  

        // 按钮启用状态属性
        public bool CanConnect { get; set; } = true;
        public bool CanDisconnect { get; set; } = false;
        public bool CanStart { get; set; } = false;
        public bool CanStop { get; set; } = false;
        public bool CanReset { get; set; } = false;
        public bool CanStartDataCollection { get; set; } = false;
        public bool CanStopDataCollection { get; set; } = false;

        // 数据采集状态
        public bool IsDataCollecting { get; set; } = false;
        public string DataCollectionStatus { get; set; } = "未启动";
        public int CollectionInterval { get; set; } = 1000; 

        // 私有命令字段
        private RelayCommand _connectCmd;
        private RelayCommand _disconnectCmd;
        private RelayCommand _startCmd;
        private RelayCommand _stopCmd;
        private RelayCommand _resetCmd;
        private RelayCommand _startDataCollectionCmd;
        private RelayCommand _stopDataCollectionCmd;

        // 命令属性
        public ICommand ConnectCommand => _connectCmd;
        public ICommand DisconnectCommand => _disconnectCmd;
        public ICommand StartCommand => _startCmd;
        public ICommand StopCommand => _stopCmd;
        public ICommand ResetCommand => _resetCmd;
        public ICommand StartDataCollectionCommand => _startDataCollectionCmd;
        public ICommand StopDataCollectionCommand => _stopDataCollectionCmd;

        // PropertyChanged 事件
        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            // 捕获当前同步上下文（通常是UI线程的同步上下文）
            _syncContext = SynchronizationContext.Current ?? new SynchronizationContext();
            _cancellationTokenSource = new CancellationTokenSource();

            InitializeCommands();
            UpdateButtonStates();
        }

        private void InitializeCommands()
        {
            _connectCmd = new RelayCommand(
                execute: _ => ConnectToDevice(),
                canExecute: _ => CanConnect
            );

            _disconnectCmd = new RelayCommand(
                execute: _ => DisconnectFromDevice(),
                canExecute: _ => CanDisconnect
            );

            _startCmd = new RelayCommand(
                execute: _ => StartDevice(),
                canExecute: _ => CanStart
            );

            _stopCmd = new RelayCommand(
                execute: _ => StopDevice(),
                canExecute: _ => CanStop
            );

            _resetCmd = new RelayCommand(
                execute: _ => ResetDevice(),
                canExecute: _ => CanReset
            );

            _startDataCollectionCmd = new RelayCommand(
                execute: _ => StartDataCollection(),
                canExecute: _ => CanStartDataCollection
            );

            _stopDataCollectionCmd = new RelayCommand(
                execute: _ => StopDataCollection(),
                canExecute: _ => CanStopDataCollection
            );
        }

        // 线程安全的属性更新方法
        private void UpdatePropertySafe<T>(Action<T> updateAction, T value)
        {
            if (_syncContext != null)
            {
                _syncContext.Post(_ => updateAction(value), null);
            }
            else
            {
                updateAction(value);
            }
        }

        // 线程安全的多属性更新方法
        private void UpdatePropertiesSafe(Action updateAction)
        {
            if (_syncContext != null)
            {
                _syncContext.Post(_ => updateAction(), null);
            }
            else
            {
                updateAction();
            }
        }

        private async void ConnectToDevice()
        {
            try
            {
                DeviceStatus = "连接中...";
                ErrorMessage = "";
                CanConnect = false;
                UpdateButtonStates();

                await Task.Delay(2000);

                UpdatePropertiesSafe(() =>
                {
                    IsConnected = true;
                    DeviceStatus = "已连接";
                    Temperature = 25.5;
                    Pressure = 1.2;
                    Voltage = 220.0;
                    Current = 0.0;
                    Power = 0.0;
                    Vibration = 0.1;
                    AlarmStatus = "正常";
                    LastUpdate = DateTime.Now;
                    UpdateButtonStates();
                });

                // 开始基础数据更新
                _ = Task.Run(() => SimulateBasicDataUpdate(_cancellationTokenSource.Token));
            }
            catch (Exception ex)
            {
                UpdatePropertiesSafe(() =>
                {
                    ErrorMessage = $"连接失败: {ex.Message}";
                    DeviceStatus = "连接失败";
                    IsConnected = false;
                    UpdateButtonStates();
                });
            }
        }

        private void DisconnectFromDevice()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            IsConnected = false;
            IsRunning = false;
            IsDataCollecting = false;
            DeviceStatus = "离线";
            DataCollectionStatus = "未启动";
            ResetAllParameters();
            UpdateButtonStates();
        }

        private async void StartDevice()
        {
            if (!IsConnected) return;

            DeviceStatus = "启动中...";
            UpdateButtonStates();

            await Task.Delay(1000);

            UpdatePropertiesSafe(() =>
            {
                IsRunning = true;
                DeviceStatus = "运行中";
                MotorSpeed = 1500;
                Current = 15.5;
                Power = Current * Voltage / 1000; // kW
                Efficiency = 85.0;
                UpdateButtonStates();
            });
        }

        private async void StopDevice()
        {
            if (!IsConnected) return;

            DeviceStatus = "停止中...";
            UpdateButtonStates();

            await Task.Delay(1000);

            UpdatePropertiesSafe(() =>
            {
                IsRunning = false;
                DeviceStatus = "已停止";
                MotorSpeed = 0;
                Current = 0.0;
                Power = 0.0;
                Efficiency = 0.0;
                UpdateButtonStates();
            });
        }

        private void ResetDevice()
        {
            if (!IsConnected) return;

            IsRunning = false;
            IsDataCollecting = false;
            DataCollectionStatus = "未启动";
            ResetAllParameters();
            DeviceStatus = "已重置";
            ErrorMessage = "";

            UpdateButtonStates();
        }

        private void StartDataCollection()
        {
            if (!IsConnected) return;

            IsDataCollecting = true;
            DataCollectionStatus = "采集中";
            UpdateButtonStates();

            // 开始高频数据采集
            _ = Task.Run(() => SimulateDataCollection(_cancellationTokenSource.Token));
        }

        private void StopDataCollection()
        {
            IsDataCollecting = false;
            DataCollectionStatus = "已停止";
            UpdateButtonStates();
        }

        private void ResetAllParameters()
        {
            Temperature = 0;
            Pressure = 0;
            MotorSpeed = 0;
            Voltage = 0;
            Current = 0;
            Power = 0;
            Vibration = 0;
            ProductCount = 0;
            Efficiency = 0;
            AlarmStatus = "正常";
        }

        private void UpdateButtonStates()
        {
            CanConnect = !IsConnected;
            CanDisconnect = IsConnected;
            CanStart = IsConnected && !IsRunning;
            CanStop = IsConnected && IsRunning;
            CanReset = IsConnected;
            CanStartDataCollection = IsConnected && !IsDataCollecting;
            CanStopDataCollection = IsConnected && IsDataCollecting;

            // 触发命令的 CanExecuteChanged 事件
            _connectCmd?.RaiseCanExecuteChanged();
            _disconnectCmd?.RaiseCanExecuteChanged();
            _startCmd?.RaiseCanExecuteChanged();
            _stopCmd?.RaiseCanExecuteChanged();
            _resetCmd?.RaiseCanExecuteChanged();
            _startDataCollectionCmd?.RaiseCanExecuteChanged();
            _stopDataCollectionCmd?.RaiseCanExecuteChanged();
        }

        private async Task SimulateBasicDataUpdate(CancellationToken cancellationToken)
        {
            var random = new Random();

            while (!cancellationToken.IsCancellationRequested && IsConnected)
            {
                try
                {
                    if (!IsDataCollecting)
                    {
                        UpdatePropertiesSafe(() =>
                        {
                            if (IsRunning)
                            {
                                Temperature = 25 + random.NextDouble() * 50;
                                Pressure = 1.0 + random.NextDouble() * 2.0;
                                MotorSpeed = 1450 + random.Next(0, 100);
                                Voltage = 220 + random.NextDouble() * 10 - 5;
                                Current = 15 + random.NextDouble() * 5;
                                Power = Current * Voltage / 1000;
                                Vibration = 0.5 + random.NextDouble() * 2.0;
                                Efficiency = 80 + random.NextDouble() * 15;
                            }

                            LastUpdate = DateTime.Now;
                        });
                    }

                    await Task.Delay(2000, cancellationToken); // 基础更新间隔2秒
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    UpdatePropertySafe<string>(msg => ErrorMessage = msg, $"基础数据更新错误: {ex.Message}");
                    break;
                }
            }
        }

        private async Task SimulateDataCollection(CancellationToken cancellationToken)
        {
            var random = new Random();

            while (!cancellationToken.IsCancellationRequested && IsConnected && IsDataCollecting)
            {
                try
                {
                    UpdatePropertiesSafe(() =>
                    {
                        if (IsRunning)
                        {
                            // 高频数据采集 - 使用时间函数模拟真实波动
                            var timeMs = DateTime.Now.Millisecond;
                            var timeSec = DateTime.Now.Second;

                            Temperature = 30 + Math.Sin(timeMs / 1000.0 * Math.PI) * 20 + random.NextDouble() * 5;
                            Pressure = 1.5 + Math.Cos(timeMs / 800.0 * Math.PI) * 0.8 + random.NextDouble() * 0.2;
                            MotorSpeed = 1500 + (int)(Math.Sin(timeMs / 600.0 * Math.PI) * 50) + random.Next(-20, 20);

                            Voltage = 220 + Math.Sin(timeMs / 1200.0 * Math.PI) * 8 + random.NextDouble() * 3;
                            Current = 15 + Math.Cos(timeMs / 900.0 * Math.PI) * 3 + random.NextDouble() * 1;
                            Power = Current * Voltage / 1000;

                            Vibration = 0.8 + Math.Abs(Math.Sin(timeMs / 300.0 * Math.PI)) * 1.5 + random.NextDouble() * 0.3;
                            Efficiency = 85 + Math.Sin(timeSec / 10.0 * Math.PI) * 10 + random.NextDouble() * 3;

                            // 产品计数
                            if (timeMs % 100 < 50 && random.NextDouble() > 0.7)
                            {
                                ProductCount++;
                            }

                            // 报警模拟
                            if (Temperature > 70)
                                AlarmStatus = "温度报警";
                            else if (Vibration > 2.0)
                                AlarmStatus = "振动报警";
                            else if (Pressure > 2.5)
                                AlarmStatus = "压力报警";
                            else
                                AlarmStatus = "正常";
                        }
                        else
                        {
                            // 设备停止时的数据
                            Temperature = Math.Max(25, Temperature - 0.5);
                            Pressure = Math.Max(0, Pressure - 0.1);
                            MotorSpeed = 0;
                            Current = 0;
                            Power = 0;
                            Vibration = Math.Max(0.1, Vibration - 0.05);
                            Efficiency = 0;
                            AlarmStatus = "正常";
                        }

                        LastUpdate = DateTime.Now;
                    });

                    await Task.Delay(CollectionInterval, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    UpdatePropertySafe<string>(msg => ErrorMessage = msg, $"数据采集错误: {ex.Message}");
                    break;
                }
            }

            UpdatePropertySafe<string>(status => DataCollectionStatus = status, "已停止");
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // 释放资源
        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}