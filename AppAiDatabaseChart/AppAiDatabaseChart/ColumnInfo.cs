using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAiDatabaseChart
{
    /// <summary>
    /// 列信息类
    /// </summary>
    public class ColumnInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsNotNull { get; set; }
        public bool IsPrimaryKey { get; set; }
    }

    /// <summary>
    /// 外键信息类
    /// </summary>
    public class ForeignKeyInfo
    {
        public string Column { get; set; }
        public string RefTable { get; set; }
        public string RefColumn { get; set; }
    }
}
