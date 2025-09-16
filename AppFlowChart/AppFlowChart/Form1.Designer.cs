namespace AppFlowChart
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel toolPanel;
        private CustomPanel pnlMain;
        private System.Windows.Forms.Button btnAddRectangle;
        private System.Windows.Forms.Button btnAddEllipse;
        private System.Windows.Forms.Button btnAddDiamond;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblInstructions;

        // 添加方向选择按钮
        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.Button btnBackward;
        private System.Windows.Forms.Button btnBoth;
        private System.Windows.Forms.Button btnNone;
        private System.Windows.Forms.Label lblDirection;


        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolPanel = new System.Windows.Forms.Panel();
            this.btnAddRectangle = new System.Windows.Forms.Button();
            this.btnAddEllipse = new System.Windows.Forms.Button();
            this.btnAddDiamond = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.pnlMain = new CustomPanel();

            // 方向选择控件
            this.lblDirection = new System.Windows.Forms.Label();
            this.btnForward = new System.Windows.Forms.Button();
            this.btnBackward = new System.Windows.Forms.Button();
            this.btnBoth = new System.Windows.Forms.Button();
            this.btnNone = new System.Windows.Forms.Button();

            this.toolPanel.SuspendLayout();
            this.SuspendLayout();

            // 
            // toolPanel - 调整高度以容纳更多控件
            // 
            this.toolPanel.BackColor = System.Drawing.Color.LightGray;
            this.toolPanel.Controls.Add(this.lblDirection);
            this.toolPanel.Controls.Add(this.btnNone);
            this.toolPanel.Controls.Add(this.btnBoth);
            this.toolPanel.Controls.Add(this.btnBackward);
            this.toolPanel.Controls.Add(this.btnForward);
            this.toolPanel.Controls.Add(this.lblInstructions);
            this.toolPanel.Controls.Add(this.btnClear);
            this.toolPanel.Controls.Add(this.btnDelete);
            this.toolPanel.Controls.Add(this.btnConnect);
            this.toolPanel.Controls.Add(this.btnAddDiamond);
            this.toolPanel.Controls.Add(this.btnAddEllipse);
            this.toolPanel.Controls.Add(this.btnAddRectangle);
            this.toolPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolPanel.Location = new System.Drawing.Point(0, 0);
            this.toolPanel.Name = "toolPanel";
            this.toolPanel.Size = new System.Drawing.Size(1200, 100); // 增加高度和宽度
            this.toolPanel.TabIndex = 0;

            // 
            // btnAddRectangle
            // 
            this.btnAddRectangle.BackColor = System.Drawing.Color.LightBlue;
            this.btnAddRectangle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddRectangle.Location = new System.Drawing.Point(12, 12);
            this.btnAddRectangle.Name = "btnAddRectangle";
            this.btnAddRectangle.Size = new System.Drawing.Size(80, 30);
            this.btnAddRectangle.TabIndex = 0;
            this.btnAddRectangle.Text = "添加矩形";
            this.btnAddRectangle.UseVisualStyleBackColor = false;
            this.btnAddRectangle.Click += new System.EventHandler(this.btnAddRectangle_Click);

            // 
            // btnAddEllipse
            // 
            this.btnAddEllipse.BackColor = System.Drawing.Color.LightGreen;
            this.btnAddEllipse.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddEllipse.Location = new System.Drawing.Point(98, 12);
            this.btnAddEllipse.Name = "btnAddEllipse";
            this.btnAddEllipse.Size = new System.Drawing.Size(80, 30);
            this.btnAddEllipse.TabIndex = 1;
            this.btnAddEllipse.Text = "添加椭圆";
            this.btnAddEllipse.UseVisualStyleBackColor = false;
            this.btnAddEllipse.Click += new System.EventHandler(this.btnAddEllipse_Click);

            // 
            // btnAddDiamond
            // 
            this.btnAddDiamond.BackColor = System.Drawing.Color.LightYellow;
            this.btnAddDiamond.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddDiamond.Location = new System.Drawing.Point(184, 12);
            this.btnAddDiamond.Name = "btnAddDiamond";
            this.btnAddDiamond.Size = new System.Drawing.Size(80, 30);
            this.btnAddDiamond.TabIndex = 2;
            this.btnAddDiamond.Text = "添加菱形";
            this.btnAddDiamond.UseVisualStyleBackColor = false;
            this.btnAddDiamond.Click += new System.EventHandler(this.btnAddDiamond_Click);

            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.Orange;
            this.btnConnect.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConnect.Location = new System.Drawing.Point(290, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(80, 30);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "连接节点";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);

            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.LightCoral;
            this.btnDelete.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDelete.Location = new System.Drawing.Point(376, 12);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 30);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "删除节点";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.LightPink;
            this.btnClear.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClear.Location = new System.Drawing.Point(462, 12);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 30);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "清空画布";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            // 
            // lblDirection
            // 
            this.lblDirection.AutoSize = true;
            this.lblDirection.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDirection.Location = new System.Drawing.Point(570, 10);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new System.Drawing.Size(68, 17);
            this.lblDirection.TabIndex = 6;
            this.lblDirection.Text = "箭头方向:";

            // 
            // btnForward
            // 
            this.btnForward.BackColor = System.Drawing.Color.Orange;
            this.btnForward.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnForward.Location = new System.Drawing.Point(570, 30);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(60, 25);
            this.btnForward.TabIndex = 7;
            this.btnForward.Text = "正向→";
            this.btnForward.UseVisualStyleBackColor = false;
            this.btnForward.Click += new System.EventHandler(this.btnForward_Click);

            // 
            // btnBackward
            // 
            this.btnBackward.BackColor = System.Drawing.Color.LightGray;
            this.btnBackward.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBackward.Location = new System.Drawing.Point(636, 30);
            this.btnBackward.Name = "btnBackward";
            this.btnBackward.Size = new System.Drawing.Size(60, 25);
            this.btnBackward.TabIndex = 8;
            this.btnBackward.Text = "反向←";
            this.btnBackward.UseVisualStyleBackColor = false;
            this.btnBackward.Click += new System.EventHandler(this.btnBackward_Click);

            // 
            // btnBoth
            // 
            this.btnBoth.BackColor = System.Drawing.Color.LightGray;
            this.btnBoth.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBoth.Location = new System.Drawing.Point(702, 30);
            this.btnBoth.Name = "btnBoth";
            this.btnBoth.Size = new System.Drawing.Size(60, 25);
            this.btnBoth.TabIndex = 9;
            this.btnBoth.Text = "双向↔";
            this.btnBoth.UseVisualStyleBackColor = false;
            this.btnBoth.Click += new System.EventHandler(this.btnBoth_Click);

            // 
            // btnNone
            // 
            this.btnNone.BackColor = System.Drawing.Color.LightGray;
            this.btnNone.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnNone.Location = new System.Drawing.Point(768, 30);
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(60, 25);
            this.btnNone.TabIndex = 10;
            this.btnNone.Text = "无箭头";
            this.btnNone.UseVisualStyleBackColor = false;
            this.btnNone.Click += new System.EventHandler(this.btnNone_Click);

            // 
            // lblInstructions
            // 
            this.lblInstructions.AutoSize = true;
            this.lblInstructions.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblInstructions.Location = new System.Drawing.Point(12, 70);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(800, 17);
            this.lblInstructions.TabIndex = 11;
            this.lblInstructions.Text = "操作说明：点击节点选中 | 拖拽移动节点 | 选择箭头方向后，选中起始节点点击连接按钮，再点击目标节点创建连接";

            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 100);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1200, 600);
            this.pnlMain.TabIndex = 1;
            this.pnlMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlMain_Paint);
            this.pnlMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlMain_MouseDown);
            this.pnlMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlMain_MouseMove);
            this.pnlMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlMain_MouseUp);

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.toolPanel);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "流程图编辑器 - 支持可选箭头方向";
            this.toolPanel.ResumeLayout(false);
            this.toolPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

    }
}
