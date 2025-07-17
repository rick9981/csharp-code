using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSqlserverToMySql
{
    #region 辅助类定义

    /// <summary>
    /// 同步选项
    /// </summary>
    public class SyncOptions
    {
        public string[] Tables { get; set; }
        public bool SyncStructure { get; set; }
        public bool SyncData { get; set; }
    }

    /// <summary>
    /// 同步进度信息
    /// </summary>
    public class SyncProgress
    {
        public string CurrentTable { get; set; }
        public string Message { get; set; }
        public string Phase { get; set; }
        public bool Success { get; set; }
    }

    #endregion
}
