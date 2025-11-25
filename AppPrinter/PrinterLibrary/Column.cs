using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterLibrary
{
    public class Column
    {
        public Column() { }
        public Column(int width)
        {
            this.Width = width;
        }
        public int Width { get; set; } = 0;
    }
}
