using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppIndustrialAop
{
    public interface IPerformanceLogger
    {
        void LogPerformance(string methodName, long elapsedMs);
        void LogWarning(string methodName, long elapsedMs);
        void LogError(string methodName, Exception ex);
        ObservableCollection<PerformanceRecord> GetPerformanceRecords();
        List<PerformanceMetrics> GetPerformanceMetrics();
        List<MethodPerformanceStats> GetMethodStats();
    }
}
