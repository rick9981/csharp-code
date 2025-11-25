using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppTestPrinter
{
    public static class Utils
    {
       

        /// <summary>
        /// 英寸转换为毫米
        /// </summary>
        /// <param name="inches"></param>
        /// <returns></returns>
        public static double InchesToMillimeters(double inches)
        {
            return inches * 25.4;
        }

        /// <summary>
        /// 毫米转换为百分之一英寸
        /// </summary>
        /// <param name="millimeters"></param>
        /// <returns></returns>
        public static int MillimetersToHundredthsOfAnInch(double millimeters)
        {
            return (int)Math.Round(millimeters / 25.4 * 100);
        }

        /// <summary>
        /// 设置打印机
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="print"></param>
        public static void PrinterSettings(PrintDocument doc, PrinterLibrary.TableConfig.Print print, int inheight = 0)
        {
            doc.PrinterSettings.PrinterName = print.Name;

            // 处理自定义纸张大小或从打印机支持的纸张中选择
            if (!string.IsNullOrEmpty(print.CustomPaperSize))
            {
                var sizeParts = print.CustomPaperSize.Split('x');
                if (sizeParts.Length == 2 && double.TryParse(sizeParts[0], out double width)
                    && double.TryParse(sizeParts[1], out double height))
                {
                    int widthInHundredthsOfAnInch = Utils.MillimetersToHundredthsOfAnInch(width);
                    int heightInHundredthsOfAnInch = 0;
                    if (inheight == 0)
                    {
                        heightInHundredthsOfAnInch = Utils.MillimetersToHundredthsOfAnInch(height);
                    }
                    else
                    {
                        heightInHundredthsOfAnInch = Utils.MillimetersToHundredthsOfAnInch(inheight);
                    }
                    // 创建自定义纸张大小
                    PaperSize customSize = new PaperSize("Custom", widthInHundredthsOfAnInch, heightInHundredthsOfAnInch);
                    doc.DefaultPageSettings.PaperSize = customSize;
                }
            }
            else
            {
                foreach (PaperSize size in doc.PrinterSettings.PaperSizes)
                {
                    if (size.PaperName == print.PaperSize)
                    {
                        doc.DefaultPageSettings.PaperSize = size;
                        break;
                    }
                }
            }

            // 设置打印文档的边距（以百分之一英寸为单位）
            int leftMargin = 0; // 例如，设置左边距为1英寸
            int topMargin = 0; // 例如，设置上边距为1英寸
            int rightMargin = 0; // 例如，设置右边距为1英寸
            int bottomMargin = 0; // 例如，设置下边距为1英寸
            Margins margins = new Margins(leftMargin, rightMargin, topMargin, bottomMargin);
            doc.DefaultPageSettings.Margins = margins;

            if (print.Landscape!=null && print.Landscape.Value)
            {
                // 设置打印方向为横向
                doc.DefaultPageSettings.Landscape = true;
            }
        }
    }
}
