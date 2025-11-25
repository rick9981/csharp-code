using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterLibrary
{
    public class Position
    {
        public int RowIndex { get; set; } = 0;
        public int ColumnIndex { get; set; } = 0;
        public Point Point { get; set; }

        public Position(int rowIndex, int columnIndex, Point point)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Point = point;
        }

        public Position(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }
    }
}
