using static AppFlowChart.FlowChartEditor;

namespace AppFlowChart
{
    public partial class Form1 : Form
    {
        private List<FlowChartNode> nodes;
        private List<Connection> connections;
        private FlowChartNode selectedNode;
        private FlowChartNode dragNode;
        private bool isDragging;
        private Point dragStartPoint;
        private bool isConnecting;
        private FlowChartNode connectStartNode;
        private Point mousePosition;
        private ConnectionDirection currentConnectionDirection;
        public Form1()
        {
            InitializeComponent();
            InitializeFlowChart();
        }

        private void InitializeFlowChart()
        {
            nodes = new List<FlowChartNode>();
            connections = new List<Connection>();
            selectedNode = null;
            isDragging = false;
            isConnecting = false;
            currentConnectionDirection = ConnectionDirection.Forward;
            UpdateDirectionButtons();
        }

        private void UpdateDirectionButtons()
        {
            // 重置所有按钮颜色
            btnForward.BackColor = Color.LightGray;
            btnBackward.BackColor = Color.LightGray;
            btnBoth.BackColor = Color.LightGray;
            btnNone.BackColor = Color.LightGray;

            // 高亮当前选择的按钮
            switch (currentConnectionDirection)
            {
                case ConnectionDirection.Forward:
                    btnForward.BackColor = Color.Orange;
                    break;
                case ConnectionDirection.Backward:
                    btnBackward.BackColor = Color.Orange;
                    break;
                case ConnectionDirection.Both:
                    btnBoth.BackColor = Color.Orange;
                    break;
                case ConnectionDirection.None:
                    btnNone.BackColor = Color.Orange;
                    break;
            }
        }

        private void DrawNode(Graphics g, FlowChartNode node)
        {
            Rectangle bounds = node.Bounds;

            // 绘制节点背景
            using (Brush brush = new SolidBrush(node.BackColor))
            {
                if (node.NodeType == NodeType.Rectangle)
                    g.FillRectangle(brush, bounds);
                else if (node.NodeType == NodeType.Ellipse)
                    g.FillEllipse(brush, bounds);
                else if (node.NodeType == NodeType.Diamond)
                    g.FillPolygon(brush, GetDiamondPoints(bounds));
            }

            // 绘制边框
            Color borderColor = node == selectedNode ? Color.Red : Color.Black;
            int borderWidth = node == selectedNode ? 3 : 2;

            using (Pen pen = new Pen(borderColor, borderWidth))
            {
                if (node.NodeType == NodeType.Rectangle)
                    g.DrawRectangle(pen, bounds);
                else if (node.NodeType == NodeType.Ellipse)
                    g.DrawEllipse(pen, bounds);
                else if (node.NodeType == NodeType.Diamond)
                    g.DrawPolygon(pen, GetDiamondPoints(bounds));
            }

            // 绘制文本
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                using (Brush textBrush = new SolidBrush(Color.Black))
                {
                    g.DrawString(node.Text, this.Font, textBrush, bounds, sf);
                }
            }
        }

        private Point[] GetDiamondPoints(Rectangle bounds)
        {
            int centerX = bounds.X + bounds.Width / 2;
            int centerY = bounds.Y + bounds.Height / 2;

            return new Point[]
            {
                new Point(centerX, bounds.Y),
                new Point(bounds.Right, centerY),
                new Point(centerX, bounds.Bottom),
                new Point(bounds.X, centerY)
            };
        }

        private void DrawConnection(Graphics g, Connection connection)
        {
            // 计算节点边缘的连接点，而不是中心点
            Point startPoint = GetNodeEdgePoint(connection.StartNode, connection.EndNode);
            Point endPoint = GetNodeEdgePoint(connection.EndNode, connection.StartNode);

            using (Pen pen = new Pen(Color.Black, 2))
            {
                g.DrawLine(pen, startPoint, endPoint);

                // 根据连接方向绘制箭头
                switch (connection.Direction)
                {
                    case ConnectionDirection.Forward:
                        DrawArrow(g, pen, startPoint, endPoint);
                        break;
                    case ConnectionDirection.Backward:
                        DrawArrow(g, pen, endPoint, startPoint);
                        break;
                    case ConnectionDirection.Both:
                        DrawArrow(g, pen, startPoint, endPoint);
                        DrawArrow(g, pen, endPoint, startPoint);
                        break;
                    case ConnectionDirection.None:
                        // 不绘制箭头
                        break;
                }
            }
        }

        // 计算节点边缘的连接点
        private Point GetNodeEdgePoint(FlowChartNode fromNode, FlowChartNode toNode)
        {
            Rectangle fromBounds = fromNode.Bounds;
            Rectangle toBounds = toNode.Bounds;

            // 计算两个节点中心点
            Point fromCenter = new Point(
                fromBounds.X + fromBounds.Width / 2,
                fromBounds.Y + fromBounds.Height / 2);

            Point toCenter = new Point(
                toBounds.X + toBounds.Width / 2,
                toBounds.Y + toBounds.Height / 2);

            // 计算方向向量
            double dx = toCenter.X - fromCenter.X;
            double dy = toCenter.Y - fromCenter.Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance == 0) return fromCenter;

            // 单位方向向量
            double unitX = dx / distance;
            double unitY = dy / distance;

            // 根据节点类型计算边缘点
            return GetNodeBoundaryPoint(fromNode, unitX, unitY);
        }

        // 根据节点类型计算边界点
        private Point GetNodeBoundaryPoint(FlowChartNode node, double directionX, double directionY)
        {
            Rectangle bounds = node.Bounds;
            Point center = new Point(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);

            switch (node.NodeType)
            {
                case NodeType.Rectangle:
                    return GetRectangleBoundaryPoint(bounds, center, directionX, directionY);

                case NodeType.Ellipse:
                    return GetEllipseBoundaryPoint(bounds, center, directionX, directionY);

                case NodeType.Diamond:
                    return GetDiamondBoundaryPoint(bounds, center, directionX, directionY);

                default:
                    return center;
            }
        }

        // 计算矩形边界点
        private Point GetRectangleBoundaryPoint(Rectangle bounds, Point center, double dirX, double dirY)
        {
            double halfWidth = bounds.Width / 2.0;
            double halfHeight = bounds.Height / 2.0;

            // 计算与矩形边界的交点
            double t = Math.Min(halfWidth / Math.Abs(dirX), halfHeight / Math.Abs(dirY));

            return new Point(
                (int)(center.X + dirX * t),
                (int)(center.Y + dirY * t)
            );
        }

        // 计算椭圆边界点
        private Point GetEllipseBoundaryPoint(Rectangle bounds, Point center, double dirX, double dirY)
        {
            // 获取目标点方向的极角
            double theta = Math.Atan2(dirY, dirX);
            double a = bounds.Width / 2.0;
            double b = bounds.Height / 2.0;
            double x = center.X + a * Math.Cos(theta);
            double y = center.Y + b * Math.Sin(theta);
            return new Point((int)x, (int)y);
        }

        // 计算菱形边界点
        private Point GetDiamondBoundaryPoint(Rectangle bounds, Point center, double dirX, double dirY)
        {
            double halfWidth = bounds.Width / 2.0;
            double halfHeight = bounds.Height / 2.0;

            // 根据方向角度计算交点
            double absX = Math.Abs(dirX);
            double absY = Math.Abs(dirY);

            // 菱形边界条件：|x/a| + |y/b| = 1
            double scale = 1.0 / (absX / halfWidth + absY / halfHeight);

            return new Point(
                (int)(center.X + dirX * scale),
                (int)(center.Y + dirY * scale)
            );
        }

        // 箭头绘制方法
        private void DrawArrow(Graphics g, Pen pen, Point start, Point end)
        {
            // 计算箭头方向
            double dx = end.X - start.X;
            double dy = end.Y - start.Y;
            double angle = Math.Atan2(dy, dx);

            int arrowLength = 15;  // 增加箭头长度
            double arrowAngle = Math.PI / 6;  // 30度角

            // 计算箭头的两个端点
            Point arrowPoint1 = new Point(
                (int)(end.X - arrowLength * Math.Cos(angle - arrowAngle)),
                (int)(end.Y - arrowLength * Math.Sin(angle - arrowAngle)));

            Point arrowPoint2 = new Point(
                (int)(end.X - arrowLength * Math.Cos(angle + arrowAngle)),
                (int)(end.Y - arrowLength * Math.Sin(angle + arrowAngle)));

            // 绘制箭头
            using (Pen arrowPen = new Pen(pen.Color, pen.Width))
            {
                g.DrawLine(arrowPen, end, arrowPoint1);
                g.DrawLine(arrowPen, end, arrowPoint2);

                // 可选：填充箭头（实心箭头）
                Point[] arrowPoints = { end, arrowPoint1, arrowPoint2 };
                using (Brush arrowBrush = new SolidBrush(pen.Color))
                {
                    g.FillPolygon(arrowBrush, arrowPoints);
                }
            }
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // 绘制连接线
            foreach (var connection in connections)
            {
                DrawConnection(g, connection);
            }

            // 绘制临时连接线
            if (isConnecting && connectStartNode != null)
            {
                using (Pen pen = new Pen(Color.Blue, 2))
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                    // 从节点边缘开始绘制临时连接线
                    Point startPoint = new Point(
                        connectStartNode.Bounds.X + connectStartNode.Bounds.Width / 2,
                        connectStartNode.Bounds.Y + connectStartNode.Bounds.Height / 2);

                    g.DrawLine(pen, startPoint, mousePosition);

                    // 在鼠标位置绘制临时箭头预览
                    if (currentConnectionDirection == ConnectionDirection.Forward ||
                        currentConnectionDirection == ConnectionDirection.Both)
                    {
                        using (Pen arrowPen = new Pen(Color.Blue, 2))
                        {
                            DrawArrow(g, arrowPen, startPoint, mousePosition);
                        }
                    }
                }
            }

            // 绘制节点
            foreach (var node in nodes)
            {
                DrawNode(g, node);
            }
        }

        

        private FlowChartNode GetNodeAt(Point location)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                if (nodes[i].Bounds.Contains(location))
                    return nodes[i];
            }
            return null;
        }

        private void btnAddRectangle_Click(object sender, EventArgs e)
        {
            AddNode(NodeType.Rectangle);
        }

        private void btnAddEllipse_Click(object sender, EventArgs e)
        {
            AddNode(NodeType.Ellipse);
        }

        private void btnAddDiamond_Click(object sender, EventArgs e)
        {
            AddNode(NodeType.Diamond);
        }

        private void AddNode(NodeType nodeType)
        {
            FlowChartNode newNode = new FlowChartNode
            {
                NodeType = nodeType,
                Bounds = new Rectangle(50 + nodes.Count * 20, 50 + nodes.Count * 20, 100, 60),
                Text = $"节点{nodes.Count + 1}",
                BackColor = GetNodeColor(nodeType)
            };

            nodes.Add(newNode);
            pnlMain.Invalidate();
        }

        private Color GetNodeColor(NodeType nodeType)
        {
            switch (nodeType)
            {
                case NodeType.Rectangle: return Color.LightBlue;
                case NodeType.Ellipse: return Color.LightGreen;
                case NodeType.Diamond: return Color.LightYellow;
                default: return Color.White;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (selectedNode != null)
            {
                isConnecting = true;
                connectStartNode = selectedNode;
                MessageBox.Show($"请点击目标节点以创建连接\n当前箭头方向: {GetDirectionDescription(currentConnectionDirection)}");
            }
            else
            {
                MessageBox.Show("请先选择一个节点");
            }
        }

        private string GetDirectionDescription(ConnectionDirection direction)
        {
            switch (direction)
            {
                case ConnectionDirection.Forward: return "正向 (→)";
                case ConnectionDirection.Backward: return "反向 (←)";
                case ConnectionDirection.Both: return "双向 (↔)";
                case ConnectionDirection.None: return "无箭头 (─)";
                default: return "未知";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedNode != null)
            {
                connections.RemoveAll(c => c.StartNode == selectedNode || c.EndNode == selectedNode);
                nodes.Remove(selectedNode);
                selectedNode = null;
                pnlMain.Invalidate();
            }
            else
            {
                MessageBox.Show("请先选择一个节点");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要清空所有内容吗？", "确认",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                nodes.Clear();
                connections.Clear();
                selectedNode = null;
                pnlMain.Invalidate();
            }
        }

        // 方向按钮事件处理
        private void btnForward_Click(object sender, EventArgs e)
        {
            currentConnectionDirection = ConnectionDirection.Forward;
            UpdateDirectionButtons();
        }

        private void btnBackward_Click(object sender, EventArgs e)
        {
            currentConnectionDirection = ConnectionDirection.Backward;
            UpdateDirectionButtons();
        }

        private void btnBoth_Click(object sender, EventArgs e)
        {
            currentConnectionDirection = ConnectionDirection.Both;
            UpdateDirectionButtons();
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            currentConnectionDirection = ConnectionDirection.None;
            UpdateDirectionButtons();
        }

        private void pnlMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FlowChartNode clickedNode = GetNodeAt(e.Location);

                if (isConnecting)
                {
                    if (clickedNode != null && clickedNode != connectStartNode)
                    {
                        // 创建带方向的连接
                        connections.Add(new Connection(connectStartNode, clickedNode, currentConnectionDirection));
                        isConnecting = false;
                        connectStartNode = null;
                        pnlMain.Invalidate();
                    }
                    else if (clickedNode == null)
                    {
                        isConnecting = false;
                        connectStartNode = null;
                        pnlMain.Invalidate();
                    }
                }
                else
                {
                    selectedNode = clickedNode;

                    if (selectedNode != null)
                    {
                        isDragging = true;
                        dragNode = selectedNode;
                        dragStartPoint = e.Location;
                    }

                    pnlMain.Invalidate();
                }
            }
        }

        private void pnlMain_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            dragNode = null;
        }

        private void pnlMain_MouseMove(object sender, MouseEventArgs e)
        {
            mousePosition = e.Location;

            if (isDragging && dragNode != null)
            {
                int deltaX = e.X - dragStartPoint.X;
                int deltaY = e.Y - dragStartPoint.Y;

                dragNode.Bounds = new Rectangle(
                    dragNode.Bounds.X + deltaX,
                    dragNode.Bounds.Y + deltaY,
                    dragNode.Bounds.Width,
                    dragNode.Bounds.Height);

                dragStartPoint = e.Location;
                pnlMain.Invalidate();
            }
            else if (isConnecting)
            {
                pnlMain.Invalidate();
            }
        }
    }
}
