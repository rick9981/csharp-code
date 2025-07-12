using System.Data;
using System.Windows.Forms;

namespace AppDataGridBar
{
    public partial class Form1 : Form
    {
        private DataTable dataTable;
        public Form1()
        {
            InitializeComponent();
            InitializeDataGridView();
        }
        private void InitializeDataGridView()
        {
            // ��������Դ
            dataTable = new DataTable();
            dataTable.Columns.Add("��������", typeof(string));
            dataTable.Columns.Add("����", typeof(int)); // �洢0-100�Ľ���ֵ
            dataTable.Columns.Add("״̬", typeof(string));

            // ���ʾ������
            dataTable.Rows.Add("�ļ�����", 75, "������");
            dataTable.Rows.Add("����ͬ��", 45, "������");
            dataTable.Rows.Add("��������", 100, "�����");
            dataTable.Rows.Add("�ʼ�����", 30, "������");

            // ������Դ
            dataGridView1.DataSource = dataTable;

            // ����������
            dataGridView1.Columns["��������"].Width = 120;
            dataGridView1.Columns["����"].Width = 200;
            dataGridView1.Columns["״̬"].Width = 80;

            // �ؼ�����CellPainting�¼�
            dataGridView1.CellPainting += DataGridView1_CellPainting;

            // �Ż���ʾЧ��
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void DataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // ֻ��"����"�н����Զ������
            if (e.ColumnIndex == 1 && e.RowIndex >= 0) // �����е�����Ϊ1
            {
                // ��ȡ����ֵ
                if (int.TryParse(e.Value?.ToString(), out int progressValue))
                {
                    // ȷ������ֵ��0-100��Χ��
                    progressValue = Math.Max(0, Math.Min(100, progressValue));

                    // ���Ʊ���
                    e.PaintBackground(e.CellBounds, true);

                    // ������������������߾ࣩ
                    Rectangle progressRect = new Rectangle(
                        e.CellBounds.X + 2,
                        e.CellBounds.Y + 2,
                        e.CellBounds.Width - 4,
                        e.CellBounds.Height - 4
                    );

                    // ���ƽ�������������ɫ��
                    using (Brush backgroundBrush = new SolidBrush(Color.LightGray))
                    {
                        e.Graphics.FillRectangle(backgroundBrush, progressRect);
                    }

                    // ���������
                    int fillWidth = (int)(progressRect.Width * (progressValue / 100.0));

                    if (fillWidth > 0)
                    {
                        Rectangle fillRect = new Rectangle(
                            progressRect.X,
                            progressRect.Y,
                            fillWidth,
                            progressRect.Height
                        );

                        // ���ݽ���ֵѡ����ɫ
                        Color fillColor = GetProgressColor(progressValue);

                        using (Brush fillBrush = new SolidBrush(fillColor))
                        {
                            e.Graphics.FillRectangle(fillBrush, fillRect);
                        }
                    }

                    // �������֣����Ȱٷֱȣ�
                    string text = $"{progressValue}%";
                    using (Brush textBrush = new SolidBrush(Color.Black))
                    {
                        StringFormat sf = new StringFormat();
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;

                        e.Graphics.DrawString(text, e.CellStyle.Font, textBrush,
                            e.CellBounds, sf);
                    }

                    // ���Ʊ߿�
                    using (Pen borderPen = new Pen(Color.Gray))
                    {
                        e.Graphics.DrawRectangle(borderPen, progressRect);
                    }

                    // ���Ϊ�Ѵ�������Ĭ�ϻ���
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// ���ݽ���ֵ���ض�Ӧ��ɫ
        /// </summary>
        private Color GetProgressColor(int progress)
        {
            if (progress < 30)
                return Color.FromArgb(220, 53, 69); // ��ɫ
            else if (progress < 70)
                return Color.FromArgb(255, 193, 7);  // ��ɫ
            else if (progress < 100)
                return Color.FromArgb(40, 167, 69);  // ��ɫ
            else
                return Color.FromArgb(23, 162, 184); // ��ɫ����ɣ�
        }
    }
}
