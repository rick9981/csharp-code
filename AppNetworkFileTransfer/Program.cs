using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using AppNetworkFileTransfer;

namespace AppNetworkFileTransfer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
}