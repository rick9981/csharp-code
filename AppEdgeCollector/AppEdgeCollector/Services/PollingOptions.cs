using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEdgeCollector.Services
{
    public class PollingOptions
    {
        public int Concurrency { get; set; } = 4;
        public int PollIntervalMs { get; set; } = 1000;
    }
}
