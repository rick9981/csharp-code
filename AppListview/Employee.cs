using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppListview
{
    // 员工信息模型 - 实现INotifyPropertyChanged接口支持数据绑定
    public class Employee : INotifyPropertyChanged
    {
        private string _name;
        private string _department;
        private decimal _salary;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(); // 属性变化时通知UI更新
            }
        }

        public string Department
        {
            get => _department;
            set
            {
                _department = value;
                OnPropertyChanged();
            }
        }

        public decimal Salary
        {
            get => _salary;
            set
            {
                _salary = value;
                OnPropertyChanged();
            }
        }

        // 属性变化通知事件
        public event PropertyChangedEventHandler PropertyChanged;

        // 触发属性变化通知的方法
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
