using System.Drawing.Drawing2D;
using Timer = System.Windows.Forms.Timer;

namespace AppAPF
{
    public partial class Form1 : Form
    {
        // 机器人参数  
        private PointF robotPosition;        // 机器人位置  
        private PointF targetPosition;       // 目标位置  
        private List<Obstacle> obstacles;    // 障碍物列表  
        private List<PointF> path;           // 路径点  

        // APF参数 - 修改这些参数以解决问题  
        private float kAtt = 0.5f;           // 吸引力系数(减小)  
        private float kRep = 500.0f;         // 排斥力系数(增大)  
        private float obstacleRadius = 40f;  // 障碍物半径  
        private float influenceRange = 150f; // 障碍物影响范围(增大)  
        private float stepSize = 3.0f;       // 步长(减小)  

        // 动画参数  
        private Timer timerAnimation;        // 动画计时器  
        private int currentPathIndex = 0;    // 当前路径索引  
        private bool isPathPlanning = false; // 路径规划状态  
        private bool isAnimating = false;    // 动画状态  

        public Form1()
        {
            InitializeComponent();
            // 初始化双缓冲以减少闪烁
            this.DoubleBuffered = true;

            // 初始化场景
            InitializeScene();

            // 设置动画计时器
            timerAnimation = new Timer();
            timerAnimation.Interval = 50;
            timerAnimation.Tick += timerAnimation_Tick;
        }
        // 初始化场景
        private void InitializeScene()
        {
            // 设置起点、终点和障碍物
            robotPosition = new PointF(100, 200);
            targetPosition = new PointF(700, 300);

            obstacles = new List<Obstacle>
            {
                new Obstacle { Position = new PointF(300, 250), Radius = obstacleRadius },
                new Obstacle { Position = new PointF(400, 150), Radius = obstacleRadius },
                new Obstacle { Position = new PointF(500, 350), Radius = obstacleRadius },
                new Obstacle { Position = new PointF(450, 250), Radius = obstacleRadius }
            };

            path = new List<PointF>();
        }

        // 计算APF路径
        private void CalculatePath()
        {
            path.Clear();
            path.Add(robotPosition);

            PointF currentPos = robotPosition;
            int maxIterations = 1000; // 防止无限循环
            int iteration = 0;

            while (Distance(currentPos, targetPosition) > 10f && iteration < maxIterations)
            {
                // 计算当前位置受到的合力
                PointF force = CalculateTotalForce(currentPos);

                // 归一化力向量并应用步长
                float magnitude = (float)Math.Sqrt(force.X * force.X + force.Y * force.Y);
                if (magnitude > 0)
                {
                    force.X = force.X / magnitude * stepSize;
                    force.Y = force.Y / magnitude * stepSize;
                }

                // 计算下一个位置
                PointF nextPos = new PointF(
                    currentPos.X + force.X,
                    currentPos.Y + force.Y
                );

                // 添加到路径
                path.Add(nextPos);
                currentPos = nextPos;
                iteration++;
            }

            // 添加终点
            if (Distance(currentPos, targetPosition) <= 10f)
            {
                path.Add(targetPosition);
            }
        }

        // 计算合力
        private PointF CalculateTotalForce(PointF position)
        {
            // 计算吸引力  
            PointF attractiveForce = CalculateAttractiveForce(position);

            // 计算各障碍物产生的排斥力  
            PointF repulsiveForce = new PointF(0, 0);
            bool nearObstacle = false;

            foreach (var obstacle in obstacles)
            {
                PointF force = CalculateRepulsiveForce(position, obstacle);
                repulsiveForce.X += force.X;
                repulsiveForce.Y += force.Y;

                // 检查是否接近障碍物  
                float distance = Distance(position, obstacle.Position) - obstacle.Radius;
                if (distance < influenceRange * 0.5f)
                {
                    nearObstacle = true;
                }
            }

            // 合力 = 吸引力 + 排斥力  
            // 如果接近障碍物，减弱吸引力的影响，以确保安全避障  
            if (nearObstacle)
            {
                attractiveForce.X *= 0.3f;
                attractiveForce.Y *= 0.3f;
            }

            return new PointF(
                attractiveForce.X + repulsiveForce.X,
                attractiveForce.Y + repulsiveForce.Y
            );
        }

        // 计算吸引力
        private PointF CalculateAttractiveForce(PointF position)
        {
            // F_att = -k_att * (q - q_goal)
            return new PointF(
                kAtt * (targetPosition.X - position.X),
                kAtt * (targetPosition.Y - position.Y)
            );
        }

        // 计算排斥力
        private PointF CalculateRepulsiveForce(PointF position, Obstacle obstacle)
        {
            // 计算到障碍物中心的距离  
            float distanceToCenter = Distance(position, obstacle.Position);

            // 计算到障碍物边缘的距离  
            float distanceToEdge = distanceToCenter - obstacle.Radius;

            // 如果距离超出影响范围，排斥力为0  
            if (distanceToEdge > influenceRange)
                return new PointF(0, 0);

            // 计算排斥力 - 修改公式，更强调近距离排斥  
            float repulsiveMagnitude;

            if (distanceToEdge <= 0) // 如果与障碍物重叠或接触  
            {
                repulsiveMagnitude = kRep * 10; // 给予非常大的排斥力  
            }
            else
            {
                repulsiveMagnitude = kRep * (float)Math.Pow(1.0f / distanceToEdge - 1.0f / influenceRange, 2);

                // 引入二次反比关系，使排斥力在接近障碍物时增长更快  
                repulsiveMagnitude *= (influenceRange / distanceToEdge);
            }

            // 计算方向（远离障碍物）  
            float dirX = position.X - obstacle.Position.X;
            float dirY = position.Y - obstacle.Position.Y;

            // 归一化方向  
            float norm = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            if (norm > 0)
            {
                dirX /= norm;
                dirY /= norm;
            }

            // 排斥力  
            return new PointF(
                repulsiveMagnitude * dirX,
                repulsiveMagnitude * dirY
            );
        }

        // 计算两点间距离
        private float Distance(PointF p1, PointF p2)
        {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        // 开始路径规划
        private void StartPathPlanning()
        {
            isPathPlanning = true;
            CalculatePath();
            Invalidate(); // 重绘窗口
        }

        // 开始动画
        private void StartAnimation()
        {
            if (path.Count <= 1)
                return;

            currentPathIndex = 0;
            isAnimating = true;
            timerAnimation.Start();
        }

        // 动画计时器回调
        private void timerAnimation_Tick(object sender, EventArgs e)
        {
            if (!isAnimating || currentPathIndex >= path.Count - 1)
            {
                timerAnimation.Stop();
                isAnimating = false;
                return;
            }

            // 更新机器人位置
            currentPathIndex++;
            robotPosition = path[currentPathIndex];
            Invalidate(); // 重绘窗口
        }

        // 窗体绘制
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 绘制路径
            if (path.Count > 1)
            {
                using (Pen pathPen = new Pen(Color.Blue, 2))
                {
                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        g.DrawLine(pathPen, path[i], path[i + 1]);
                    }
                }
            }

            // 绘制障碍物
            using (Brush obstacleBrush = new SolidBrush(Color.Red))
            {
                foreach (var obstacle in obstacles)
                {
                    g.FillEllipse(obstacleBrush,
                        obstacle.Position.X - obstacle.Radius,
                        obstacle.Position.Y - obstacle.Radius,
                        obstacle.Radius * 2,
                        obstacle.Radius * 2);
                }
            }

            // 绘制目标点
            using (Brush targetBrush = new SolidBrush(Color.Green))
            {
                g.FillEllipse(targetBrush, targetPosition.X - 10, targetPosition.Y - 10, 20, 20);
            }

            // 绘制机器人
            using (Brush robotBrush = new SolidBrush(Color.Blue))
            {
                g.FillEllipse(robotBrush, robotPosition.X - 15, robotPosition.Y - 15, 30, 30);
            }
        }

        // 点击事件 - 计算路径并开始动画
        private void btnStart_Click(object sender, EventArgs e)
        {
            StartPathPlanning();
            StartAnimation();
        }

        // 重置场景
        private void btnReset_Click(object sender, EventArgs e)
        {
            timerAnimation.Stop();
            isAnimating = false;
            isPathPlanning = false;

            InitializeScene();
            Invalidate();
        }

        // 鼠标点击事件 - 设置目标位置
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (!isAnimating && e.Button == MouseButtons.Left)
            {
                targetPosition = new PointF(e.X, e.Y);
                Invalidate();
            }
        }
    }

    // 障碍物类
    public class Obstacle
    {
        public PointF Position { get; set; }
        public float Radius { get; set; }
    }
}
