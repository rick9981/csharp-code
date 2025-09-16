using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFlowChart
{
    public class CustomPanel : Panel
    {
        public CustomPanel()
        {
            // 启用双缓冲和自定义绘制
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.DoubleBuffer |
                         ControlStyles.ResizeRedraw, true);

            this.UpdateStyles();
        }
    }
}
