using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppNetworkFileTransfer.Core;
using AppNetworkFileTransfer.Models;

namespace AppNetworkFileTransfer.Core
{
    public class FileTransferServer
    {
        private TcpListener _listener;
        private TcpClient _client;
        private NetworkStream _stream;
        private bool _isRunning = false;
        private bool _isClientConnected = false;
        private CancellationTokenSource _cancellationTokenSource;
        private const int BufferSize = 8192;
        private const int HeaderSize = 1024;

        // 添加保存目录属性
        public string SaveDirectory { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public event EventHandler<TransferEventArgs> ProgressChanged;
        public event EventHandler<TransferEventArgs> StatusChanged;
        public event EventHandler<TransferEventArgs> ClientConnected;
        public event EventHandler<TransferEventArgs> TransferStarted;
        public async Task StartListening(int port)
        {
            try
            {
                _listener = new TcpListener(IPAddress.Any, port);
                _listener.Start();
                _isRunning = true;
                _cancellationTokenSource = new CancellationTokenSource();

                OnStatusChanged($"服务器启动，监听端口 {port}，文件保存目录: {SaveDirectory}");

                // 在后台异步等待客户端连接
                _ = Task.Run(async () => await WaitForClientAsync());
            }
            catch (Exception ex)
            {
                OnStatusChanged($"服务器启动失败: {ex.Message}", true);
                throw;
            }
        }

        private async Task WaitForClientAsync()
        {
            try
            {
                while (_isRunning && !_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    _client = await _listener.AcceptTcpClientAsync();
                    _stream = _client.GetStream();
                    _isClientConnected = true;

                    OnStatusChanged($"客户端已连接: {_client.Client.RemoteEndPoint}");
                    ClientConnected?.Invoke(this, new TransferEventArgs("客户端已连接"));

                    // 开始处理客户端请求
                    _ = Task.Run(async () => await HandleClientAsync());
                    break;
                }
            }
            catch (ObjectDisposedException)
            {
                // 服务器已停止，正常情况
            }
            catch (Exception ex)
            {
                OnStatusChanged($"等待客户端连接时出错: {ex.Message}", true);
            }
        }

        private async Task HandleClientAsync()
        {
            try
            {
                while (_isRunning && _isClientConnected && !_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    // 等待接收文件头信息
                    var headerBuffer = new byte[HeaderSize];
                    var totalRead = 0;

                    while (totalRead < HeaderSize)
                    {
                        var bytesRead = await _stream.ReadAsync(headerBuffer, totalRead, HeaderSize - totalRead, _cancellationTokenSource.Token);
                        if (bytesRead == 0)
                        {
                            OnStatusChanged("客户端断开连接");
                            _isClientConnected = false;
                            return;
                        }
                        totalRead += bytesRead;
                    }

                    var headerInfo = ParseFileHeader(headerBuffer);

                    if (headerInfo.Command == "SEND")
                    {
                        await ReceiveFileFromClientAsync(headerInfo);
                    }
                    else
                    {
                        OnStatusChanged($"收到未知命令: {headerInfo.Command}", true);
                        // 发送错误响应
                        var errorBytes = Encoding.UTF8.GetBytes("ERR");
                        await _stream.WriteAsync(errorBytes, 0, errorBytes.Length, _cancellationTokenSource.Token);
                    }
                }
            }
            catch (Exception ex)
            {
                OnStatusChanged($"处理客户端请求时出错: {ex.Message}", true);
                _isClientConnected = false;
            }
        }

        private async Task ReceiveFileFromClientAsync((string Command, string FileName, long FileSize, DateTime Timestamp) headerInfo)
        {
            try
            {
                // 确保保存目录存在
                if (!Directory.Exists(SaveDirectory))
                {
                    Directory.CreateDirectory(SaveDirectory);
                }

                // 构造完整的文件路径
                var localFilePath = Path.Combine(SaveDirectory, headerInfo.FileName);

                // 如果文件已存在，添加时间戳后缀
                if (File.Exists(localFilePath))
                {
                    var fileInfo = new FileInfo(localFilePath);
                    var nameWithoutExt = Path.GetFileNameWithoutExtension(fileInfo.Name);
                    var extension = fileInfo.Extension;
                    var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    localFilePath = Path.Combine(SaveDirectory, $"{nameWithoutExt}_{timestamp}{extension}");
                }

                OnStatusChanged($"开始接收文件: {headerInfo.FileName} ({FormatBytes(headerInfo.FileSize)})");

                // 发送确认响应
                var confirmBytes = Encoding.UTF8.GetBytes("OK");
                await _stream.WriteAsync(confirmBytes, 0, confirmBytes.Length, _cancellationTokenSource.Token);

                // 在开始接收前触发开始事件，让UI设置开始时间
                OnTransferStarted(headerInfo.FileName, headerInfo.FileSize);

                // 接收文件内容
                using (var fileStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write))
                {
                    var buffer = new byte[BufferSize];
                    long totalReceived = 0;
                    int bytesRead;

                    while (totalReceived < headerInfo.FileSize)
                    {
                        var remainingBytes = headerInfo.FileSize - totalReceived;
                        var bytesToRead = (int)Math.Min(buffer.Length, remainingBytes);

                        bytesRead = await _stream.ReadAsync(buffer, 0, bytesToRead, _cancellationTokenSource.Token);
                        if (bytesRead == 0) break;

                        await fileStream.WriteAsync(buffer, 0, bytesRead, _cancellationTokenSource.Token);
                        totalReceived += bytesRead;

                        OnProgressChanged(totalReceived, headerInfo.FileSize, headerInfo.FileName);

                        if (_cancellationTokenSource.Token.IsCancellationRequested)
                            break;
                    }
                }

                OnStatusChanged($"文件接收完成: {Path.GetFileName(localFilePath)} -> {localFilePath}");
            }
            catch (Exception ex)
            {
                OnStatusChanged($"接收文件失败: {ex.Message}", true);

                // 发送错误响应
                try
                {
                    var errorBytes = Encoding.UTF8.GetBytes("ERR");
                    await _stream.WriteAsync(errorBytes, 0, errorBytes.Length, _cancellationTokenSource.Token);
                }
                catch { }
            }
        }
        public void Stop()
        {
            try
            {
                _isRunning = false;
                _isClientConnected = false;
                _cancellationTokenSource?.Cancel();

                _stream?.Close();
                _client?.Close();
                _listener?.Stop();

                OnStatusChanged("服务器已停止");
            }
            catch (Exception ex)
            {
                OnStatusChanged($"停止服务器时出错: {ex.Message}", true);
            }
        }

        private (string Command, string FileName, long FileSize, DateTime Timestamp) ParseFileHeader(byte[] header)
        {
            var headerString = Encoding.UTF8.GetString(header).TrimEnd('\0');
            var parts = headerString.Split('|');

            if (parts.Length >= 4)
            {
                return (
                    Command: parts[0],
                    FileName: parts[1],
                    FileSize: long.TryParse(parts[2], out var size) ? size : 0,
                    Timestamp: DateTime.TryParse(parts[3], out var time) ? time : DateTime.Now
                );
            }

            return ("UNKNOWN", "", 0, DateTime.Now);
        }

        private void OnTransferStarted(string fileName, long totalBytes)
        {
            TransferStarted?.Invoke(this, new TransferEventArgs(0, totalBytes, fileName));
        }

        private void OnProgressChanged(long transferred, long total, string fileName)
        {
            ProgressChanged?.Invoke(this, new TransferEventArgs(transferred, total, fileName));
        }

        private void OnStatusChanged(string message, bool isError = false)
        {
            StatusChanged?.Invoke(this, new TransferEventArgs(message, isError));
        }

        private string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}