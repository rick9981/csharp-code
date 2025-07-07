using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMergeGrid
{
    /// <summary>
    /// DataGridView合并扩展方法
    /// </summary>
    public static class MergeableDataGridViewExtensions
    {
        /// <summary>
        /// 自动合并指定列的相同值 - 支持对齐方式
        /// </summary>
        /// <param name="dgv">DataGridView控件</param>
        /// <param name="columnIndex">要合并的列索引</param>
        /// <param name="horizontalAlign">水平对齐方式</param>
        /// <param name="verticalAlign">垂直对齐方式</param>
        public static void AutoMergeColumn(this MergeableDataGridView dgv, int columnIndex,
            StringAlignment horizontalAlign = StringAlignment.Center,
            StringAlignment verticalAlign = StringAlignment.Center)
        {
            if (dgv.Rows.Count == 0) return;

            int startRow = 0;
            object currentValue = dgv[columnIndex, 0].Value;

            for (int i = 1; i < dgv.Rows.Count; i++)
            {
                object nextValue = dgv[columnIndex, i].Value;

                if (!Equals(currentValue, nextValue))
                {
                    // 值不同，合并前面的连续相同值
                    if (i - startRow > 1)
                    {
                        dgv.MergeCells(startRow, columnIndex, i - 1, columnIndex,
                            currentValue?.ToString() ?? "", horizontalAlign, verticalAlign);
                    }

                    startRow = i;
                    currentValue = nextValue;
                }
            }

            // 处理最后一组
            if (dgv.Rows.Count - startRow > 1)
            {
                dgv.MergeCells(startRow, columnIndex, dgv.Rows.Count - 1, columnIndex,
                    currentValue?.ToString() ?? "", horizontalAlign, verticalAlign);
            }
        }

        /// <summary>
        /// 自动合并多列相同值 - 支持不同对齐方式
        /// </summary>
        /// <param name="dgv">DataGridView控件</param>
        /// <param name="columnSettings">列设置数组</param>
        public static void AutoMergeColumns(this MergeableDataGridView dgv,
            params ColumnMergeSetting[] columnSettings)
        {
            foreach (var setting in columnSettings)
            {
                dgv.AutoMergeColumn(setting.ColumnIndex, setting.HorizontalAlign, setting.VerticalAlign);
            }
        }

        /// <summary>
        /// 根据条件自动合并列
        /// </summary>
        /// <param name="dgv">DataGridView控件</param>
        /// <param name="columnIndex">列索引</param>
        /// <param name="mergeCondition">合并条件函数</param>
        /// <param name="horizontalAlign">水平对齐</param>
        /// <param name="verticalAlign">垂直对齐</param>
        public static void AutoMergeColumnWithCondition(this MergeableDataGridView dgv, int columnIndex,
            Func<object, object, bool> mergeCondition,
            StringAlignment horizontalAlign = StringAlignment.Center,
            StringAlignment verticalAlign = StringAlignment.Center)
        {
            if (dgv.Rows.Count == 0) return;

            int startRow = 0;
            object currentValue = dgv[columnIndex, 0].Value;

            for (int i = 1; i < dgv.Rows.Count; i++)
            {
                object nextValue = dgv[columnIndex, i].Value;

                if (!mergeCondition(currentValue, nextValue))
                {
                    // 条件不满足，合并前面的连续值
                    if (i - startRow > 1)
                    {
                        dgv.MergeCells(startRow, columnIndex, i - 1, columnIndex,
                            currentValue?.ToString() ?? "", horizontalAlign, verticalAlign);
                    }

                    startRow = i;
                    currentValue = nextValue;
                }
            }

            // 处理最后一组
            if (dgv.Rows.Count - startRow > 1)
            {
                dgv.MergeCells(startRow, columnIndex, dgv.Rows.Count - 1, columnIndex,
                    currentValue?.ToString() ?? "", horizontalAlign, verticalAlign);
            }
        }
    }

    /// <summary>
    /// 列合并设置
    /// </summary>
    public class ColumnMergeSetting
    {
        public int ColumnIndex { get; set; }
        public StringAlignment HorizontalAlign { get; set; } = StringAlignment.Center;
        public StringAlignment VerticalAlign { get; set; } = StringAlignment.Center;

        public ColumnMergeSetting(int columnIndex,
            StringAlignment horizontalAlign = StringAlignment.Center,
            StringAlignment verticalAlign = StringAlignment.Center)
        {
            ColumnIndex = columnIndex;
            HorizontalAlign = horizontalAlign;
            VerticalAlign = verticalAlign;
        }
    }
}
