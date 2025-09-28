namespace AppEdgeHostedService.Services
{
    public class PollingOptions
    {
        public int PollIntervalMs { get; set; } = 1000;
        public int Concurrency { get; set; } = 4;
        public int MaxRetries { get; set; } = 3;
        public string ConnectionString { get; set; } = "192.168.1.100:502";
        public int MaxBufferSize { get; set; } = 10000;
        public int BatchSize { get; set; } = 100;
        public int TimeoutMs { get; set; } = 5000;
    }
}