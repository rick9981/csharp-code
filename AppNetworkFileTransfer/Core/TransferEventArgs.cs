namespace AppNetworkFileTransfer.Models
{
    public class TransferEventArgs : EventArgs
    {
        public long TransferredBytes { get; set; }
        public long TotalBytes { get; set; }
        public string FileName { get; set; }
        public string Message { get; set; }
        public bool IsError { get; set; }

        // 原有构造函数
        public TransferEventArgs(string message, bool isError = false)
        {
            Message = message;
            IsError = isError;
        }

        public TransferEventArgs(long transferredBytes, long totalBytes, string fileName)
        {
            TransferredBytes = transferredBytes;
            TotalBytes = totalBytes;
            FileName = fileName;
        }

        // 用于传输开始事件
        public TransferEventArgs(long transferredBytes, long totalBytes, string fileName, string message)
        {
            TransferredBytes = transferredBytes;
            TotalBytes = totalBytes;
            FileName = fileName;
            Message = message;
        }
    }
}