using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFlowChart
{
    public class FlowChartEditor
    {
        public enum NodeType
        {
            Rectangle,
            Ellipse,
            Diamond
        }

        public class FlowChartNode
        {
            public NodeType NodeType { get; set; }
            public Rectangle Bounds { get; set; }
            public string Text { get; set; }
            public Color BackColor { get; set; }

            public FlowChartNode()
            {
                NodeType = NodeType.Rectangle;
                Bounds = new Rectangle(0, 0, 100, 60);
                Text = "新节点";
                BackColor = Color.LightBlue;
            }
        }
    }
}
