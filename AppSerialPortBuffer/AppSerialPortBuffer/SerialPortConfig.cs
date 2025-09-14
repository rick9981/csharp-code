using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSerialPortBuffer
{
    public class SerialPortConfig
    {
        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public Parity Parity { get; set; }
        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }
        public int ReadBufferSize { get; set; } = 4096;
        public int WriteBufferSize { get; set; } = 4096;
        public int ReadTimeout { get; set; } = 500;
        public int WriteTimeout { get; set; } = 500;
    }
}
