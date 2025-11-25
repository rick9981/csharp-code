namespace PrinterLibrary
{
    public class Row
    {
        public Row()
        {
        }
        public Row(int height)
        {
            this.Height = height;
        }
        public int Height { get; set; } = 0;
    }
}
