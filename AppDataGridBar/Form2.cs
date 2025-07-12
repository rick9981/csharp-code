using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppDataGridBar
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            InitializeCustomProgressColumn();
        }

        private void InitializeCustomProgressColumn()
        {
            // 清除自动生成的列
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            // 添加普通列 - 关键是要设置DataPropertyName
            DataGridViewTextBoxColumn taskNameColumn = new DataGridViewTextBoxColumn
            {
                Name = "TaskName",
                HeaderText = "任务名称",
                DataPropertyName = "TaskName" // 绑定到TaskInfo.TaskName属性
            };
            dataGridView1.Columns.Add(taskNameColumn);

            // 如果要添加进度条列，取消注释这部分
            DataGridViewProgressColumn progressColumn = new DataGridViewProgressColumn
            {
                Name = "Progress",
                HeaderText = "完成进度",
                Width = 200,
                DataPropertyName = "Progress"
            };
            dataGridView1.Columns.Add(progressColumn);

            // 添加状态列
            DataGridViewTextBoxColumn statusColumn = new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "状态",
                DataPropertyName = "Status" // 绑定到TaskInfo.Status属性
            };
            dataGridView1.Columns.Add(statusColumn);

            // 绑定数据源
            List<TaskInfo> tasks = new List<TaskInfo>
            {
                new TaskInfo { TaskName = "系统备份", Progress = 85, Status = "进行中" },
                new TaskInfo { TaskName = "数据清理", Progress = 60, Status = "进行中" },
                new TaskInfo { TaskName = "日志归档", Progress = 100, Status = "已完成" }
            };

            dataGridView1.DataSource = tasks;
        }

        // 数据模型类
        public class TaskInfo
        {
            public string TaskName { get; set; }
            public int Progress { get; set; }
            public string Status { get; set; }
        }
    }
}
