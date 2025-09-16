using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppFlowChart.FlowChartEditor;

namespace AppFlowChart
{
    public class Connection
    {
        public FlowChartNode StartNode { get; set; }
        public FlowChartNode EndNode { get; set; }
        public ConnectionDirection Direction { get; set; } // 新增方向属性

        public Connection(FlowChartNode startNode, FlowChartNode endNode)
            : this(startNode, endNode, ConnectionDirection.Forward)
        {
        }

        public Connection(FlowChartNode startNode, FlowChartNode endNode, ConnectionDirection direction)
        {
            StartNode = startNode;
            EndNode = endNode;
            Direction = direction;
        }
    }
}
