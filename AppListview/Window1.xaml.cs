using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppListview
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public ObservableCollection<Employee> Employees { get; set; }
        public Window1()
        {
            InitializeComponent();
            LoadEmployees();

            // 设置数据上下文，支持XAML中的数据绑定
            DataContext = this;
        }

        private void LoadEmployees()
        {
            // 初始化员工数据
            Employees = new ObservableCollection<Employee>
        {
            new Employee { Name = "张三", Department = "技术部", Salary = 15000 },
            new Employee { Name = "李四", Department = "销售部", Salary = 12000 },
            new Employee { Name = "王五", Department = "人事部", Salary = 10000 }
        };
        }
    }
}
