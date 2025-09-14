using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSerialPortBuffer
{
    public class OptimizedSerialPortHandler : IDisposable
    {
        private SerialPort _serialPort;
        private CancellationTokenSource _cancellationTokenSource;
        private byte[] _receiveBuffer;
        private bool _disposed = false;
        private bool _isConnected = false;
        private readonly object _lockObject = new object();

        public event Action<byte[]> DataReceived;
        public event Action<string> ErrorOccurred;

        public bool IsConnected => _isConnected && (_serialPort?.IsOpen ?? false);

        public async Task<bool> ConnectAsync(SerialPortConfig config)
        {
            try
            {
                lock (_lockObject)
                {
                    if (_serialPort?.IsOpen == true)
                    {
                        _serialPort.Close();
                        _isConnected = false;
                    }

                    _serialPort = new SerialPort(config.PortName, config.BaudRate, config.Parity, config.DataBits, config.StopBits)
                    {
                        ReadBufferSize = config.ReadBufferSize,
                        WriteBufferSize = config.WriteBufferSize,
                        ReadTimeout = config.ReadTimeout,
                        WriteTimeout = config.WriteTimeout,
                        Handshake = Handshake.None
                    };

                    _receiveBuffer = new byte[config.ReadBufferSize];

                    _serialPort.DataReceived += SerialPort_DataReceived;
                    _serialPort.ErrorReceived += SerialPort_ErrorReceived;
                }

                await Task.Run(() => _serialPort.Open());

                _cancellationTokenSource = new CancellationTokenSource();
                _isConnected = true; // 连接成功后设置标志

                return true;
            }
            catch (Exception ex)
            {
                _isConnected = false; // 连接失败时确保标志为false
                ErrorOccurred?.Invoke($"连接失败: {ex.Message}");
                return false;
            }
        }


        public void Disconnect()
        {
            try
            {
                _isConnected = false; // 立即设置连接状态为false

                _cancellationTokenSource?.Cancel();

                lock (_lockObject)
                {
                    if (_serialPort?.IsOpen == true)
                    {
                        _serialPort.DataReceived -= SerialPort_DataReceived;
                        _serialPort.ErrorReceived -= SerialPort_ErrorReceived;
                        _serialPort.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"断开连接时出错: {ex.Message}");
            }
            finally
            {
                _isConnected = false; // 确保在任何情况下都设置为false
            }
        }

        // 同步发送
        public void SendData(byte[] data)
        {
            if (!IsConnected) return;

            try
            {
                lock (_lockObject)
                {
                    if (_serialPort.IsOpen)
                    {
                        _serialPort.Write(data, 0, data.Length);
                    }
                }
            }
            catch (TimeoutException ex)
            {
                ErrorOccurred?.Invoke($"发送超时: {ex.Message}");
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"发送失败: {ex.Message}");
            }
        }

        // 异步发送
        public async Task<bool> SendDataAsync(byte[] data, int timeoutMilliseconds = 1000)
        {
            if (!IsConnected) return false;

            var timeoutCts = new CancellationTokenSource(timeoutMilliseconds);

            try
            {
                await Task.Run(() =>
                {
                    lock (_lockObject)
                    {
                        if (_serialPort.IsOpen)
                        {
                            _serialPort.Write(data, 0, data.Length);
                        }
                    }
                }, timeoutCts.Token);

                return true;
            }
            catch (OperationCanceledException)
            {
                ErrorOccurred?.Invoke("发送操作超时");
                return false;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"异步发送失败: {ex.Message}");
                return false;
            }
        }

        private async void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                // 异步读取数据，提高响应速度  
                byte[] data = await ReadDataAsync();
                if (data.Length > 0)
                {
                    DataReceived?.Invoke(data);
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"数据接收异常: {ex.Message}");
            }
        }

        // 异步读取数据 - 使用内存流优化  
        private async Task<byte[]> ReadDataAsync()
        {
            using (var ms = new MemoryStream())
            {
                try
                {
                    int bytesRead;
                    do
                    {
                        bytesRead = await _serialPort.BaseStream.ReadAsync(_receiveBuffer, 0, _receiveBuffer.Length);
                        if (bytesRead > 0)
                        {
                            ms.Write(_receiveBuffer, 0, bytesRead);
                        }
                    } while (bytesRead > 0 && _serialPort.BytesToRead > 0);

                    return ms.ToArray();
                }
                catch (Exception ex)
                {
                    ErrorOccurred?.Invoke($"读取数据失败: {ex.Message}");
                    return new byte[0];
                }
            }
        }

        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            ErrorOccurred?.Invoke($"串口错误: {e.EventType}");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源  
                    _cancellationTokenSource?.Cancel();
                    _cancellationTokenSource?.Dispose();
                    _serialPort?.Close();
                    _serialPort?.Dispose();
                }

                // 释放非托管资源  
                _receiveBuffer = null;
                _disposed = true;
            }
        }
    }
}
