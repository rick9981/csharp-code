namespace AppEdgeHostedService.Models
{
    public class CollectionData
    {
        public DateTime Timestamp { get; set; }
        public int Address { get; set; }
        public string Value { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Quality { get; set; } = string.Empty;
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        public override string ToString()
        {
            return $"[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] {Address}={Value} ({DataType}) [{Status}]";
        }
    }
}