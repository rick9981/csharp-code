using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppNetworkFileTransfer.Core;
using AppNetworkFileTransfer.Models;

namespace AppNetworkFileTransfer.Core
{
    public class FileTransferClient
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private bool _isConnected = false;
        private CancellationTokenSource _cancellationTokenSource;
        private const int BufferSize = 8192;
        private const int HeaderSize = 1024;

        public event EventHandler<TransferEventArgs> ProgressChanged;
        public event EventHandler<TransferEventArgs> StatusChanged;
        public event EventHandler<TransferEventArgs> TransferStarted;
        public async Task ConnectAsync(string serverIP, int port)
        {
            try
            {
                _client = new TcpClient();
                _cancellationTokenSource = new CancellationTokenSource();

                await _client.ConnectAsync(serverIP, port);
                _stream = _client.GetStream();
                _isConnected = true;

                OnStatusChanged($"已连接到服务器 {serverIP}:{port}");
            }
            catch (Exception ex)
            {
                OnStatusChanged($"连接服务器失败: {ex.Message}", true);
                throw;
            }
        }

        public void Disconnect()
        {
            try
            {
                _isConnected = false;
                _cancellationTokenSource?.Cancel();

                _stream?.Close();
                _client?.Close();

                OnStatusChanged("已断开连接");
            }
            catch (Exception ex)
            {
                OnStatusChanged($"断开连接时出错: {ex.Message}", true);
            }
        }

        public async Task SendFileAsync(string localFilePath)
        {
            if (!_isConnected || _stream == null)
                throw new InvalidOperationException("未连接到服务器");

            if (!File.Exists(localFilePath))
                throw new FileNotFoundException($"文件不存在: {localFilePath}");

            try
            {
                var fileInfo = new FileInfo(localFilePath);
                var fileName = fileInfo.Name;

                OnStatusChanged($"开始发送文件: {fileName} ({FormatBytes(fileInfo.Length)})");

                // 触发传输开始事件
                OnTransferStarted(fileName, fileInfo.Length);

                // 发送文件头信息
                var header = CreateFileHeader("SEND", fileName, fileInfo.Length);
                await _stream.WriteAsync(header, 0, header.Length, _cancellationTokenSource.Token);

                // 等待服务器确认
                var response = new byte[4];
                await _stream.ReadAsync(response, 0, 4, _cancellationTokenSource.Token);
                var responseStr = Encoding.UTF8.GetString(response).TrimEnd('\0');

                if (responseStr != "OK")
                {
                    throw new Exception($"服务器响应错误: {responseStr}");
                }

                // 发送文件内容
                using (var fileStream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read))
                {
                    var buffer = new byte[BufferSize];
                    long totalSent = 0;
                    int bytesRead;

                    while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length, _cancellationTokenSource.Token)) > 0)
                    {
                        await _stream.WriteAsync(buffer, 0, bytesRead, _cancellationTokenSource.Token);
                        totalSent += bytesRead;

                        OnProgressChanged(totalSent, fileInfo.Length, fileName);

                        if (_cancellationTokenSource.Token.IsCancellationRequested)
                            break;
                    }
                }

                OnStatusChanged($"文件发送完成: {fileName}");
            }
            catch (Exception ex)
            {
                OnStatusChanged($"发送文件失败: {ex.Message}", true);
                throw;
            }
        }

        private byte[] CreateFileHeader(string command, string fileName, long fileSize)
        {
            var header = new byte[HeaderSize];
            var headerString = $"{command}|{fileName}|{fileSize}|{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            var headerBytes = Encoding.UTF8.GetBytes(headerString);

            Array.Copy(headerBytes, 0, header, 0, Math.Min(headerBytes.Length, HeaderSize));
            return header;
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