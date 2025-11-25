using System;

namespace AppNetworkFileTransfer.Models
{
    public class FileTransferInfo
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FilePath { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string CheckSum { get; set; }
        public TransferDirection Direction { get; set; }
        public TransferStatus Status { get; set; }
        public long TransferredBytes { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string ErrorMessage { get; set; }

        public double ProgressPercentage => FileSize > 0 ? (double)TransferredBytes / FileSize * 100 : 0;

        public TimeSpan? Duration => EndTime.HasValue ? EndTime.Value - StartTime : (TimeSpan?)null;

        public long TransferSpeed => Duration.HasValue && Duration.Value.TotalSeconds > 0
            ? (long)(TransferredBytes / Duration.Value.TotalSeconds) : 0;
    }

    public enum TransferDirection
    {
        Upload,
        Download
    }

    public enum TransferStatus
    {
        Pending,
        InProgress,
        Completed,
        Failed,
        Cancelled
    }
}