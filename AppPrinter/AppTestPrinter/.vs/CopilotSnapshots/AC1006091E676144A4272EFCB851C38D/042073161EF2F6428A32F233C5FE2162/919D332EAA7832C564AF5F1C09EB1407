using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterLibrary
{
    public class TableConfig
    {
        public string Print_Location { get; set; }
        public Print Printer { get; set; }

        public Point StartPosition { get; set; }
        public int RowsCount { get; set; }
        public int ColumnsCount { get; set; }
        public List<Row> Rows { get; set; }
        public List<int> ColumnsWidths { get; set; }
        public List<BarCode> BarCodes { get; set; }

        public class Row
        {
            public string RowIndex { get; set; }
            public List<Cell> Cells { get; set; }
            public bool Is_Items { get; set; }
            public string? Height { get; set; }
            public List<Border> Borders { get; set; } = new List<Border>();
        }

        public class Cell
        {
            public string Title { get; set; }
            public string ColumnIndex { get; set; }
            public string[] Merge { get; set; }
            public string Alignment { get; set; }
            public string LineAlignment { get; set; }
            public string? Width { get; set; }
            public CFont? Font { get; set; }
            public List<Border> Borders { get; set; } = new List<Border>();
        }

        public class BarCode
        {
            public string RowIndex { get; set; }
            public string ColumnIndex { get; set; }
            public string Tilte { get; set; }
            public string BarcodeType { get; set; }
            public Size? Sizes { get; set; }
        }

        public class Print
        {
            public string Name { get; set; }
            public string PaperSize { get; set; }
            public bool? Landscape { get; set; }
            public string? CustomPaperSize { get; set; }
        }

        public class CFont
        {
            public string Name { get; set; }
            public float Size { get; set; }
            public bool Bold { get; set; }
            public bool Italic { get; set; }
        }

        public class Border
        {
            public string Direction { get; set; }
            public int Width { get; set; } = 1;
            public string Style { get; set; } = "Solid";
            public string Color { get; set; } = "#000000";
        }
    }
}
