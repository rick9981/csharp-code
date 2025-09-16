using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFlowChart
{
    public enum ConnectionDirection
    {
        Forward,    // 正向箭头 (起始->结束)  
        Backward,   // 反向箭头 (结束->起始)   
        Both,       // 双向箭头  
        None        // 无箭头  
    }
}
