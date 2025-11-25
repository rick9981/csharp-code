using System.Drawing.Printing;
using PrinterLibrary;


namespace AppTestPrinter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Test1 test1 = new Test1();
            test1.Print();
            //Test2 test2 = new Test2();
            //test2.Print();
        }
    }

    /// <summary>
    /// 普通标签打印
    /// </summary>
    class Test1
    {
        PrintDocument printDocument = new PrintDocument();
        TableConfig tableConfig;
        TableDocument tableDocument;
        dynamic dto;
        public void Print()
        {
            tableDocument = new TableDocument();
            tableConfig = System.Text.Json.JsonSerializer.Deserialize<TableConfig>(tableDocument.RemoveCommentsFromJson(File.ReadAllText("templates/原材料码单.json")));
            var print = tableConfig.Printer;
            Utils.PrinterSettings(printDocument, print);
            dto = new
            {
                barcode = "A0001",
                supplier = "中国",
                part_no = "A000001",
                part_description = "测试物料",
                unit = "EA",
                qty = 10,
                batch_no = "A01"
            };
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
            try
            {
                printDocument.Print();
            }
            catch (Exception ex)
            {
            }
        }

        private async void PrintDocument_PrintPage(object sender, PrintPageEventArgs ev)
        {
            var graphics = ev.Graphics;
            await tableDocument.DrawTableAsync(graphics, dto, null, tableConfig);
        }
    }


    /// <summary>
    /// 列表打印
    /// </summary>
    class Test2
    {
        PrintDocument printDocument = new PrintDocument();
        TableConfig tableConfig;
        TableDocument tableDocument;
        dynamic dto;
        public void Print()
        {
            tableDocument = new TableDocument();
            tableConfig = System.Text.Json.JsonSerializer.Deserialize<TableConfig>(tableDocument.RemoveCommentsFromJson(File.ReadAllText("templates/产品采购单.json")));
            var print = tableConfig.Printer;
            Utils.PrinterSettings(printDocument, print);
            dto = new
            {
                main = new
                {
                    purchase_no = "P0001",
                    supplier = "中国A",
                    requested_arrival_time = DateTime.Now,
                    CreateUserName = "Rick",
                    remark = "备注A0001"
                },
                items = new List<dynamic>
                {
                    new {
                        part_no = "PN001",
                        part_description = "螺丝M6x20",
                        unit = "个",
                        qty = "100",
                        remark = "紧急采购"
                    },
                    new {
                        part_no = "PN002",
                        part_description = "垫片φ10",
                        unit = "片",
                        qty = "200",
                        remark = "标准件"
                    },
                    new {
                        part_no = "PN003",
                        part_description = "轴承6200",
                        unit = "个",
                        qty = "50",
                        remark = "高精度"
                    },
                    new {
                        part_no = "PN004",
                        part_description = "钢管φ20x2",
                        unit = "米",
                        qty = "10",
                        remark = "无缝钢管"
                    },
                    new {
                        part_no = "PN005",
                        part_description = "电缆线2.5mm²",
                        unit = "米",
                        qty = "100",
                        remark = "阻燃型"
                    }
                }
            };
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
            try
            {
                printDocument.Print();
            }
            catch (Exception ex)
            {
            }
        }

        private async void PrintDocument_PrintPage(object sender, PrintPageEventArgs ev)
        {
            var graphics = ev.Graphics;
            await tableDocument.DrawTableAsync(graphics, dto.main, dto.items, tableConfig);
        }
    }
}
