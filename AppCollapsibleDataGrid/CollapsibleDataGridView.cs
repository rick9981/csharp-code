using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppCollapsibleDataGrid
{
    public partial class CollapsibleDataGridView : UserControl
    {
        private DataGridView dataGridView;
        private List<GroupInfo> groups;
        private DataTable originalDataTable;
        private string groupColumnName;
        private bool showGroupHeaders = true;
        private const int GROUP_HEADER_HEIGHT = 25;
        private Dictionary<string, string> groupCustomTexts;
        private bool isBatchUpdating = false;

        public CollapsibleDataGridView()
        {
            InitializeComponent();
            InitializeDataGridView();
            groups = new List<GroupInfo>();
            groupCustomTexts = new Dictionary<string, string>();
        }

        private void InitializeDataGridView()
        {
            dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.ReadOnly = true;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.RowHeadersVisible = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.BackgroundColor = Color.White;
            dataGridView.GridColor = Color.LightGray;

            dataGridView.RowTemplate.Height = 22;
            dataGridView.AllowUserToResizeRows = false;

            // 启用双缓冲以减少闪烁
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, dataGridView, new object[] { true });

            dataGridView.CellPainting += DataGridView_CellPainting;
            dataGridView.CellClick += DataGridView_CellClick;
            dataGridView.RowPrePaint += DataGridView_RowPrePaint;
            dataGridView.CellFormatting += DataGridView_CellFormatting;

            this.Controls.Add(dataGridView);
        }

        // 公共属性
        [Category("Collapsible")]
        [Description("获取或设置用于分组的列名")]
        public string GroupColumn
        {
            get { return groupColumnName; }
            set
            {
                groupColumnName = value;
                if (originalDataTable != null)
                {
                    RefreshGroups();
                }
            }
        }

        [Category("Collapsible")]
        [Description("获取或设置是否显示分组标题")]
        public bool ShowGroupHeaders
        {
            get { return showGroupHeaders; }
            set
            {
                showGroupHeaders = value;
                RefreshDisplay();
            }
        }

        [Category("Collapsible")]
        [Description("获取内部DataGridView控件")]
        public DataGridView InnerDataGridView
        {
            get { return dataGridView; }
        }

        // 设置数据源
        public void SetDataSource(DataTable dataTable, string groupByColumn)
        {
            originalDataTable = dataTable.Copy();
            groupColumnName = groupByColumn;
            RefreshGroups();
        }

        // 刷新分组
        private void RefreshGroups()
        {
            if (originalDataTable == null || string.IsNullOrEmpty(groupColumnName))
                return;

            groups.Clear();

            var groupedData = originalDataTable.AsEnumerable()
                .GroupBy(row => row[groupColumnName]?.ToString() ?? "")
                .OrderBy(g => g.Key);

            foreach (var group in groupedData)
            {
                var groupInfo = new GroupInfo
                {
                    GroupName = group.Key,
                    IsExpanded = true,
                    Rows = group.ToList()
                };
                groups.Add(groupInfo);

                // 初始化分组自定义文字为当前时间
                if (!groupCustomTexts.ContainsKey(group.Key))
                {
                    groupCustomTexts[group.Key] = DateTime.Now.ToString("HH:mm:ss");
                }
            }

            RefreshDisplay();
        }

        // 刷新显示
        private void RefreshDisplay()
        {
            if (originalDataTable == null)
                return;

            // 暂停绘制以减少闪烁
            dataGridView.SuspendLayout();
            try
            {
                DataTable displayTable = originalDataTable.Clone();
                displayTable.Columns.Add("__IsGroupHeader", typeof(bool));
                displayTable.Columns.Add("__GroupName", typeof(string));
                displayTable.Columns.Add("__IsExpanded", typeof(bool));
                displayTable.Columns.Add("__GroupRowCount", typeof(int));

                foreach (var group in groups)
                {
                    if (showGroupHeaders)
                    {
                        DataRow headerRow = displayTable.NewRow();
                        for (int i = 0; i < originalDataTable.Columns.Count; i++)
                        {
                            headerRow[i] = DBNull.Value;
                        }

                        headerRow["__IsGroupHeader"] = true;
                        headerRow["__GroupName"] = group.GroupName;
                        headerRow["__IsExpanded"] = group.IsExpanded;
                        headerRow["__GroupRowCount"] = group.Rows.Count;

                        displayTable.Rows.Add(headerRow);
                    }

                    if (group.IsExpanded)
                    {
                        foreach (var row in group.Rows)
                        {
                            DataRow newRow = displayTable.NewRow();
                            for (int i = 0; i < originalDataTable.Columns.Count; i++)
                            {
                                newRow[i] = row[i];
                            }
                            newRow["__IsGroupHeader"] = false;
                            newRow["__GroupName"] = group.GroupName;
                            newRow["__IsExpanded"] = group.IsExpanded;
                            newRow["__GroupRowCount"] = 0;
                            displayTable.Rows.Add(newRow);
                        }
                    }
                }

                dataGridView.DataSource = displayTable;
                HideHelperColumns();
            }
            finally
            {
                dataGridView.ResumeLayout();
            }
        }

        private void HideHelperColumns()
        {
            string[] helperColumns = { "__IsGroupHeader", "__GroupName", "__IsExpanded", "__GroupRowCount" };
            foreach (string colName in helperColumns)
            {
                if (dataGridView.Columns.Contains(colName))
                {
                    dataGridView.Columns[colName].Visible = false;
                }
            }
        }

        private void DataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView.Rows.Count)
                return;

            DataGridViewRow row = dataGridView.Rows[e.RowIndex];
            DataRowView rowView = row.DataBoundItem as DataRowView;

            if (rowView != null && Convert.ToBoolean(rowView["__IsGroupHeader"]))
            {
                row.Height = GROUP_HEADER_HEIGHT;
                row.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                row.DefaultCellStyle.Font = new Font(dataGridView.Font, FontStyle.Bold);
            }
            else
            {
                row.Height = 22;
                row.DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView.Rows.Count)
                return;

            DataGridViewRow row = dataGridView.Rows[e.RowIndex];
            DataRowView rowView = row.DataBoundItem as DataRowView;

            if (rowView != null && Convert.ToBoolean(rowView["__IsGroupHeader"]))
            {
                if (e.ColumnIndex == 0)
                {
                    bool isExpanded = Convert.ToBoolean(rowView["__IsExpanded"]);
                    string groupName = rowView["__GroupName"].ToString();
                    int count = Convert.ToInt32(rowView["__GroupRowCount"]);

                    string icon = isExpanded ? "▼" : "▶";
                    e.Value = $"{icon} {groupName} ({count} 项)";
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dataGridView.Columns.Count - 5)
                {
                    string groupName = rowView["__GroupName"].ToString();
                    e.Value = GetGroupSummaryText(groupName);
                    e.FormattingApplied = true;
                }
                else
                {
                    e.Value = "";
                    e.FormattingApplied = true;
                }
            }
        }

        private void DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            DataGridView dgv = sender as DataGridView;
            DataRowView rowView = dgv.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView != null && Convert.ToBoolean(rowView["__IsGroupHeader"]))
            {
                using (var brush = new SolidBrush(Color.FromArgb(230, 235, 245)))
                {
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                }

                using (var pen = new Pen(Color.FromArgb(200, 200, 200)))
                {
                    e.Graphics.DrawRectangle(pen, e.CellBounds);
                }

                if (e.ColumnIndex == 0)
                {
                    bool isExpanded = Convert.ToBoolean(rowView["__IsExpanded"]);
                    string groupName = rowView["__GroupName"].ToString();
                    int count = Convert.ToInt32(rowView["__GroupRowCount"]);

                    string icon = isExpanded ? "▼" : "▶";
                    string text = $"{icon} {groupName} ({count} 项)";

                    using (var brush = new SolidBrush(Color.FromArgb(50, 50, 50)))
                    using (var font = new Font(dgv.Font, FontStyle.Bold))
                    {
                        var textRect = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + 4,
                                                   e.CellBounds.Width - 16, e.CellBounds.Height - 8);
                        e.Graphics.DrawString(text, font, brush, textRect,
                                            new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    }
                }
                else if (e.ColumnIndex == dgv.Columns.Count - 5)
                {
                    string groupName = rowView["__GroupName"].ToString();
                    string text = GetGroupSummaryText(groupName);

                    using (var brush = new SolidBrush(Color.FromArgb(80, 80, 80)))
                    using (var font = new Font(dgv.Font, FontStyle.Regular))
                    {
                        var textRect = new Rectangle(e.CellBounds.X + 2, e.CellBounds.Y + 4,
                                                   e.CellBounds.Width - 16, e.CellBounds.Height - 8);
                        e.Graphics.DrawString(text, font, brush, textRect,
                                            new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    }
                }

                e.Handled = true;
            }
        }

        private string GetGroupSummaryText(string groupName)
        {
            return groupCustomTexts.ContainsKey(groupName) ? groupCustomTexts[groupName] : DateTime.Now.ToString("HH:mm:ss");
        }

        // 批量更新开始
        public void BeginBatchUpdate()
        {
            isBatchUpdating = true;
        }

        // 批量更新结束
        public void EndBatchUpdate()
        {
            isBatchUpdating = false;
            RefreshGroupHeaders();
        }

        public void UpdateGroupCustomText(string groupName, string customText)
        {
            if (groupCustomTexts.ContainsKey(groupName))
            {
                groupCustomTexts[groupName] = customText;
                if (!isBatchUpdating)
                {
                    RefreshGroupHeaders();
                }
            }
        }

        private void RefreshGroupHeaders()
        {
            if (dataGridView.DataSource is DataTable)
            {
                this.BeginInvoke(new Action(() =>
                {
                    for (int i = 0; i < dataGridView.Rows.Count; i++)
                    {
                        DataRowView rowView = dataGridView.Rows[i].DataBoundItem as DataRowView;
                        if (rowView != null && Convert.ToBoolean(rowView["__IsGroupHeader"]))
                        {
                            // 只重绘特定的单元格而不是整行
                            int lastColumnIndex = dataGridView.Columns.Count - 5;
                            if (lastColumnIndex >= 0 && lastColumnIndex < dataGridView.Columns.Count)
                            {
                                dataGridView.InvalidateCell(lastColumnIndex, i);
                            }
                        }
                    }
                }));
            }
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridView dgv = sender as DataGridView;
            DataRowView rowView = dgv.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView != null && Convert.ToBoolean(rowView["__IsGroupHeader"]))
            {
                string groupName = rowView["__GroupName"].ToString();
                var group = groups.FirstOrDefault(g => g.GroupName == groupName);

                if (group != null)
                {
                    group.IsExpanded = !group.IsExpanded;
                    RefreshDisplay();
                }
            }
        }

        public void ExpandAll()
        {
            foreach (var group in groups)
            {
                group.IsExpanded = true;
            }
            RefreshDisplay();
        }

        public void CollapseAll()
        {
            foreach (var group in groups)
            {
                group.IsExpanded = false;
            }
            RefreshDisplay();
        }

        public void ExpandGroup(string groupName)
        {
            var group = groups.FirstOrDefault(g => g.GroupName == groupName);
            if (group != null)
            {
                group.IsExpanded = true;
                RefreshDisplay();
            }
        }

        public void CollapseGroup(string groupName)
        {
            var group = groups.FirstOrDefault(g => g.GroupName == groupName);
            if (group != null)
            {
                group.IsExpanded = false;
                RefreshDisplay();
            }
        }

        public List<string> GetGroupNames()
        {
            return groups.Select(g => g.GroupName).ToList();
        }

        public bool IsGroupExpanded(string groupName)
        {
            var group = groups.FirstOrDefault(g => g.GroupName == groupName);
            return group?.IsExpanded ?? false;
        }

        public void UpdateCellValue(int originalRowIndex, string columnName, object newValue)
        {
            if (dataGridView.DataSource is DataTable displayTable)
            {
                int displayRowIndex = -1;
                int dataRowCount = 0;

                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    DataRowView rowView = dataGridView.Rows[i].DataBoundItem as DataRowView;
                    if (rowView != null && !Convert.ToBoolean(rowView["__IsGroupHeader"]))
                    {
                        if (dataRowCount == originalRowIndex)
                        {
                            displayRowIndex = i;
                            break;
                        }
                        dataRowCount++;
                    }
                }

                if (displayRowIndex >= 0 && dataGridView.Columns.Contains(columnName))
                {
                    // 直接更新单元格值，避免触发整行重绘
                    var cell = dataGridView.Rows[displayRowIndex].Cells[columnName];
                    if (!cell.Value?.Equals(newValue) == true)
                    {
                        cell.Value = newValue;
                    }
                }
            }
        }

        public void UpdateCellValueById(int id, string columnName, object newValue)
        {
            if (dataGridView.DataSource is DataTable displayTable)
            {
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    DataRowView rowView = dataGridView.Rows[i].DataBoundItem as DataRowView;
                    if (rowView != null && !Convert.ToBoolean(rowView["__IsGroupHeader"]))
                    {
                        if (Convert.ToInt32(rowView["ID"]) == id && dataGridView.Columns.Contains(columnName))
                        {
                            var cell = dataGridView.Rows[i].Cells[columnName];
                            if (!cell.Value?.Equals(newValue) == true)
                            {
                                cell.Value = newValue;
                            }
                            break;
                        }
                    }
                }
            }
        }
    }

    internal class GroupInfo
    {
        public string GroupName { get; set; }
        public bool IsExpanded { get; set; }
        public List<DataRow> Rows { get; set; }

        public GroupInfo()
        {
            Rows = new List<DataRow>();
        }
    }
}