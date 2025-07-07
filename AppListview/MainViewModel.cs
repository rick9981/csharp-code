using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace AppListview
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<AdvancedEmployee> Employees { get; set; }

        // 编辑命令
        public ICommand EditCommand { get; }

        // 删除命令
        public ICommand DeleteCommand { get; }

        public MainViewModel()
        {
            LoadEmployees();

            // 初始化命令
            EditCommand = new RelayCommand<AdvancedEmployee>(EditEmployee);
            DeleteCommand = new RelayCommand<AdvancedEmployee>(DeleteEmployee);
        }

        private void EditEmployee(AdvancedEmployee employee)
        {
            // 编辑员工信息的逻辑
            MessageBox.Show($"编辑员工：{employee.Name}");
        }

        private void DeleteEmployee(AdvancedEmployee employee)
        {
            // 删除确认
            var result = MessageBox.Show($"确定要删除员工 {employee.Name} 吗？",
                                       "确认删除",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Employees.Remove(employee);
            }
        }

        private void LoadEmployees()
        {
            Employees = new ObservableCollection<AdvancedEmployee>
        {
            new AdvancedEmployee { Name = "张三", Department = "技术部", Salary = 15000, Progress = 85, IsActive = true },
            new AdvancedEmployee { Name = "李四", Department = "销售部", Salary = 12000, Progress = 92, IsActive = true },
            new AdvancedEmployee { Name = "王五", Department = "人事部", Salary = 10000, Progress = 78, IsActive = false }
        };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // 简单的命令实现类
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke((T)parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
