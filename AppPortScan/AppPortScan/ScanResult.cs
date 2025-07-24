using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppPortScan
{
    public class ScanResult
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool IsOpen { get; set; }
        public string Service { get; set; }
        public DateTime ScanTime { get; set; }
    }
}
