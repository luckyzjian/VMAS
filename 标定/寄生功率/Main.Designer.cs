using System.Windows.Forms.DataVisualization.Charting;
namespace 汽油寄生功率
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label12 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.labelStandard = new System.Windows.Forms.Label();
            this.toolStripLabelMessage = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonForceClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonLiftUp = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLiftDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonMotorOn = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMotorOff = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonStopTest = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPrintScreen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonExit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel提示信息 = new System.Windows.Forms.ToolStripLabel();
            this.chart_jsg = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button_jsgqx = new System.Windows.Forms.Button();
            this.button_start_jsg = new System.Windows.Forms.Button();
            this.label_24jsg = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label_40jsg = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel_cs = new System.Windows.Forms.Panel();
            this.Msg_cs = new System.Windows.Forms.Label();
            this.panel11 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.panel_gl = new System.Windows.Forms.Panel();
            this.Msg_gl = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.panel_nl = new System.Windows.Forms.Panel();
            this.Msg_nl = new System.Windows.Forms.Label();
            this.dataGrid_jsgl = new System.Windows.Forms.DataGridView();
            this.buttonJsglPreResult = new System.Windows.Forms.Button();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.textBox_Jsglqx_c = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.textBox_Jsglqx_b = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.textBox_Jsglqx_a = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel7.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_jsg)).BeginInit();
            this.panel12.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel_cs.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel_gl.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel_nl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_jsgl)).BeginInit();
            this.groupBox13.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(26, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 99);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 30F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(355, 28);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(543, 52);
            this.label12.TabIndex = 1;
            this.label12.Text = "底盘测功机寄生功率滑行试验";
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.CadetBlue;
            this.panel7.Controls.Add(this.labelStandard);
            this.panel7.Controls.Add(this.label12);
            this.panel7.Controls.Add(this.pictureBox1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1238, 115);
            this.panel7.TabIndex = 18;
            this.panel7.Paint += new System.Windows.Forms.PaintEventHandler(this.panel7_Paint);
            // 
            // labelStandard
            // 
            this.labelStandard.AutoSize = true;
            this.labelStandard.Font = new System.Drawing.Font("微软雅黑", 25F, System.Drawing.FontStyle.Bold);
            this.labelStandard.Location = new System.Drawing.Point(963, 66);
            this.labelStandard.Name = "labelStandard";
            this.labelStandard.Size = new System.Drawing.Size(250, 45);
            this.labelStandard.TabIndex = 4;
            this.labelStandard.Text = "标准：HJT290";
            // 
            // toolStripLabelMessage
            // 
            this.toolStripLabelMessage.AutoSize = false;
            this.toolStripLabelMessage.BackColor = System.Drawing.Color.Gray;
            this.toolStripLabelMessage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripLabelMessage.ForeColor = System.Drawing.Color.White;
            this.toolStripLabelMessage.Name = "toolStripLabelMessage";
            this.toolStripLabelMessage.Size = new System.Drawing.Size(180, 22);
            this.toolStripLabelMessage.Text = "status message";
            this.toolStripLabelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 32);
            // 
            // toolStripButtonForceClear
            // 
            this.toolStripButtonForceClear.AutoToolTip = false;
            this.toolStripButtonForceClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonForceClear.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonForceClear.Image")));
            this.toolStripButtonForceClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonForceClear.Name = "toolStripButtonForceClear";
            this.toolStripButtonForceClear.Size = new System.Drawing.Size(73, 29);
            this.toolStripButtonForceClear.Text = "扭力清零";
            this.toolStripButtonForceClear.Click += new System.EventHandler(this.toolStripButtonForceClear_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 32);
            // 
            // toolStripButtonLiftUp
            // 
            this.toolStripButtonLiftUp.AutoToolTip = false;
            this.toolStripButtonLiftUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonLiftUp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLiftUp.Image")));
            this.toolStripButtonLiftUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLiftUp.Name = "toolStripButtonLiftUp";
            this.toolStripButtonLiftUp.Size = new System.Drawing.Size(58, 29);
            this.toolStripButtonLiftUp.Text = "举升升";
            this.toolStripButtonLiftUp.Click += new System.EventHandler(this.toolStripButtonLiftUp_Click);
            // 
            // toolStripButtonLiftDown
            // 
            this.toolStripButtonLiftDown.AutoToolTip = false;
            this.toolStripButtonLiftDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonLiftDown.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLiftDown.Image")));
            this.toolStripButtonLiftDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLiftDown.Name = "toolStripButtonLiftDown";
            this.toolStripButtonLiftDown.Size = new System.Drawing.Size(58, 29);
            this.toolStripButtonLiftDown.Text = "举升降";
            this.toolStripButtonLiftDown.Click += new System.EventHandler(this.toolStripButtonLiftDown_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 32);
            // 
            // toolStripButtonMotorOn
            // 
            this.toolStripButtonMotorOn.AutoToolTip = false;
            this.toolStripButtonMotorOn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonMotorOn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMotorOn.Image")));
            this.toolStripButtonMotorOn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMotorOn.Name = "toolStripButtonMotorOn";
            this.toolStripButtonMotorOn.Size = new System.Drawing.Size(73, 29);
            this.toolStripButtonMotorOn.Text = "启动电机";
            this.toolStripButtonMotorOn.Click += new System.EventHandler(this.toolStripButtonMotorOn_Click);
            // 
            // toolStripButtonMotorOff
            // 
            this.toolStripButtonMotorOff.AutoToolTip = false;
            this.toolStripButtonMotorOff.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonMotorOff.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMotorOff.Image")));
            this.toolStripButtonMotorOff.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMotorOff.Name = "toolStripButtonMotorOff";
            this.toolStripButtonMotorOff.Size = new System.Drawing.Size(73, 29);
            this.toolStripButtonMotorOff.Text = "停止电机";
            this.toolStripButtonMotorOff.Click += new System.EventHandler(this.toolStripButtonMotorOff_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 32);
            // 
            // toolStripButtonStopTest
            // 
            this.toolStripButtonStopTest.AutoToolTip = false;
            this.toolStripButtonStopTest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonStopTest.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStopTest.Image")));
            this.toolStripButtonStopTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStopTest.Name = "toolStripButtonStopTest";
            this.toolStripButtonStopTest.Size = new System.Drawing.Size(73, 29);
            this.toolStripButtonStopTest.Text = "停止测试";
            this.toolStripButtonStopTest.Click += new System.EventHandler(this.toolStripButtonStopTest_Click);
            // 
            // toolStripButtonPrintScreen
            // 
            this.toolStripButtonPrintScreen.AutoToolTip = false;
            this.toolStripButtonPrintScreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonPrintScreen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPrintScreen.Image")));
            this.toolStripButtonPrintScreen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPrintScreen.Name = "toolStripButtonPrintScreen";
            this.toolStripButtonPrintScreen.Size = new System.Drawing.Size(43, 29);
            this.toolStripButtonPrintScreen.Text = "截屏";
            this.toolStripButtonPrintScreen.Click += new System.EventHandler(this.toolStripButtonPrintScreen_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 32);
            // 
            // toolStripButtonExit
            // 
            this.toolStripButtonExit.AutoToolTip = false;
            this.toolStripButtonExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonExit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExit.Image")));
            this.toolStripButtonExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExit.Name = "toolStripButtonExit";
            this.toolStripButtonExit.Size = new System.Drawing.Size(43, 29);
            this.toolStripButtonExit.Text = "退出";
            this.toolStripButtonExit.Click += new System.EventHandler(this.toolStripButtonExit_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 32);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.Teal;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelMessage,
            this.toolStripSeparator1,
            this.toolStripButtonForceClear,
            this.toolStripSeparator2,
            this.toolStripButtonLiftUp,
            this.toolStripButtonLiftDown,
            this.toolStripSeparator3,
            this.toolStripButtonMotorOn,
            this.toolStripButtonMotorOff,
            this.toolStripSeparator5,
            this.toolStripButtonStopTest,
            this.toolStripButton1,
            this.toolStripButtonPrintScreen,
            this.toolStripSeparator4,
            this.toolStripLabel提示信息,
            this.toolStripSeparator6,
            this.toolStripButtonExit});
            this.toolStrip1.Location = new System.Drawing.Point(3, 599);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1229, 32);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 17;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel提示信息
            // 
            this.toolStripLabel提示信息.Name = "toolStripLabel提示信息";
            this.toolStripLabel提示信息.Size = new System.Drawing.Size(69, 29);
            this.toolStripLabel提示信息.Text = "提示信息";
            // 
            // chart_jsg
            // 
            this.chart_jsg.Anchor = System.Windows.Forms.AnchorStyles.Top;
            chartArea2.AxisX.Title = "名义速度（Km/h）";
            chartArea2.AxisY.Title = "寄生功率（Kw）";
            chartArea2.Name = "ChartArea1";
            this.chart_jsg.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart_jsg.Legends.Add(legend2);
            this.chart_jsg.Location = new System.Drawing.Point(750, 243);
            this.chart_jsg.Name = "chart_jsg";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.LabelForeColor = System.Drawing.Color.Red;
            series2.Legend = "Legend1";
            series2.LegendText = "吸收功率";
            series2.Name = "xsgl_qx";
            this.chart_jsg.Series.Add(series2);
            this.chart_jsg.Size = new System.Drawing.Size(482, 353);
            this.chart_jsg.TabIndex = 30;
            this.chart_jsg.Text = "chart1";
            // 
            // button_jsgqx
            // 
            this.button_jsgqx.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button_jsgqx.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button_jsgqx.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_jsgqx.Location = new System.Drawing.Point(497, 328);
            this.button_jsgqx.Name = "button_jsgqx";
            this.button_jsgqx.Size = new System.Drawing.Size(247, 40);
            this.button_jsgqx.TabIndex = 31;
            this.button_jsgqx.Text = "寄生功率曲线";
            this.button_jsgqx.UseVisualStyleBackColor = true;
            this.button_jsgqx.Click += new System.EventHandler(this.button_jsgqx_Click);
            // 
            // button_start_jsg
            // 
            this.button_start_jsg.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button_start_jsg.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_start_jsg.Location = new System.Drawing.Point(497, 285);
            this.button_start_jsg.Name = "button_start_jsg";
            this.button_start_jsg.Size = new System.Drawing.Size(247, 42);
            this.button_start_jsg.TabIndex = 31;
            this.button_start_jsg.Text = "开始测试";
            this.button_start_jsg.UseVisualStyleBackColor = true;
            this.button_start_jsg.Click += new System.EventHandler(this.button_start_jsg_Click);
            // 
            // label_24jsg
            // 
            this.label_24jsg.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label_24jsg.AutoSize = true;
            this.label_24jsg.Font = new System.Drawing.Font("宋体", 12F);
            this.label_24jsg.Location = new System.Drawing.Point(702, 440);
            this.label_24jsg.Name = "label_24jsg";
            this.label_24jsg.Size = new System.Drawing.Size(24, 16);
            this.label_24jsg.TabIndex = 30;
            this.label_24jsg.Text = "--";
            // 
            // label17
            // 
            this.label17.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(526, 436);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(153, 21);
            this.label17.TabIndex = 30;
            this.label17.Text = "25km/h的寄生功率:";
            // 
            // label_40jsg
            // 
            this.label_40jsg.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label_40jsg.AutoSize = true;
            this.label_40jsg.Font = new System.Drawing.Font("宋体", 12F);
            this.label_40jsg.Location = new System.Drawing.Point(702, 415);
            this.label_40jsg.Name = "label_40jsg";
            this.label_40jsg.Size = new System.Drawing.Size(24, 16);
            this.label_40jsg.TabIndex = 30;
            this.label_40jsg.Text = "--";
            // 
            // label16
            // 
            this.label16.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(526, 413);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(153, 21);
            this.label16.TabIndex = 30;
            this.label16.Text = "40km/h的寄生功率:";
            // 
            // timer2
            // 
            this.timer2.Interval = 80;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.Black;
            this.panel12.Controls.Add(this.panel6);
            this.panel12.Controls.Add(this.panel11);
            this.panel12.Controls.Add(this.panel8);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel12.Location = new System.Drawing.Point(0, 115);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(1238, 122);
            this.panel12.TabIndex = 36;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label2);
            this.panel6.Controls.Add(this.panel_cs);
            this.panel6.Location = new System.Drawing.Point(301, 6);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(205, 111);
            this.panel6.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(34, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 31);
            this.label2.TabIndex = 0;
            this.label2.Text = "车速(km/h)";
            // 
            // panel_cs
            // 
            this.panel_cs.BackColor = System.Drawing.Color.Black;
            this.panel_cs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_cs.Controls.Add(this.Msg_cs);
            this.panel_cs.Location = new System.Drawing.Point(4, 38);
            this.panel_cs.Name = "panel_cs";
            this.panel_cs.Size = new System.Drawing.Size(192, 64);
            this.panel_cs.TabIndex = 1;
            // 
            // Msg_cs
            // 
            this.Msg_cs.AutoSize = true;
            this.Msg_cs.Font = new System.Drawing.Font("宋体", 38F, System.Drawing.FontStyle.Bold);
            this.Msg_cs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.Msg_cs.Location = new System.Drawing.Point(48, 9);
            this.Msg_cs.Name = "Msg_cs";
            this.Msg_cs.Size = new System.Drawing.Size(103, 51);
            this.Msg_cs.TabIndex = 0;
            this.Msg_cs.Text = "0.0";
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.label8);
            this.panel11.Controls.Add(this.panel_gl);
            this.panel11.Location = new System.Drawing.Point(723, 6);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(205, 111);
            this.panel11.TabIndex = 11;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(51, 4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 31);
            this.label8.TabIndex = 0;
            this.label8.Text = "功率(kW)";
            // 
            // panel_gl
            // 
            this.panel_gl.BackColor = System.Drawing.Color.Black;
            this.panel_gl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_gl.Controls.Add(this.Msg_gl);
            this.panel_gl.Location = new System.Drawing.Point(4, 38);
            this.panel_gl.Name = "panel_gl";
            this.panel_gl.Size = new System.Drawing.Size(193, 64);
            this.panel_gl.TabIndex = 1;
            // 
            // Msg_gl
            // 
            this.Msg_gl.AutoSize = true;
            this.Msg_gl.Font = new System.Drawing.Font("宋体", 38F, System.Drawing.FontStyle.Bold);
            this.Msg_gl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.Msg_gl.Location = new System.Drawing.Point(48, 7);
            this.Msg_gl.Name = "Msg_gl";
            this.Msg_gl.Size = new System.Drawing.Size(103, 51);
            this.Msg_gl.TabIndex = 0;
            this.Msg_gl.Text = "0.0";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label5);
            this.panel8.Controls.Add(this.panel_nl);
            this.panel8.Location = new System.Drawing.Point(512, 6);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(205, 111);
            this.panel8.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(58, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 31);
            this.label5.TabIndex = 0;
            this.label5.Text = "扭力(N)";
            // 
            // panel_nl
            // 
            this.panel_nl.BackColor = System.Drawing.Color.Black;
            this.panel_nl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_nl.Controls.Add(this.Msg_nl);
            this.panel_nl.Location = new System.Drawing.Point(4, 38);
            this.panel_nl.Name = "panel_nl";
            this.panel_nl.Size = new System.Drawing.Size(193, 64);
            this.panel_nl.TabIndex = 1;
            // 
            // Msg_nl
            // 
            this.Msg_nl.AutoSize = true;
            this.Msg_nl.Font = new System.Drawing.Font("宋体", 38F, System.Drawing.FontStyle.Bold);
            this.Msg_nl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.Msg_nl.Location = new System.Drawing.Point(48, 9);
            this.Msg_nl.Name = "Msg_nl";
            this.Msg_nl.Size = new System.Drawing.Size(103, 51);
            this.Msg_nl.TabIndex = 0;
            this.Msg_nl.Text = "0.0";
            // 
            // dataGrid_jsgl
            // 
            this.dataGrid_jsgl.AllowUserToAddRows = false;
            this.dataGrid_jsgl.AllowUserToDeleteRows = false;
            this.dataGrid_jsgl.AllowUserToOrderColumns = true;
            this.dataGrid_jsgl.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            this.dataGrid_jsgl.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGrid_jsgl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dataGrid_jsgl.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGrid_jsgl.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGrid_jsgl.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 14F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGrid_jsgl.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGrid_jsgl.GridColor = System.Drawing.Color.Gray;
            this.dataGrid_jsgl.Location = new System.Drawing.Point(3, 243);
            this.dataGrid_jsgl.Name = "dataGrid_jsgl";
            this.dataGrid_jsgl.ReadOnly = true;
            this.dataGrid_jsgl.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGrid_jsgl.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dataGrid_jsgl.RowHeadersVisible = false;
            this.dataGrid_jsgl.RowHeadersWidth = 40;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            this.dataGrid_jsgl.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dataGrid_jsgl.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGrid_jsgl.RowTemplate.DefaultCellStyle.Format = "D";
            this.dataGrid_jsgl.RowTemplate.DefaultCellStyle.NullValue = null;
            this.dataGrid_jsgl.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            this.dataGrid_jsgl.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGrid_jsgl.RowTemplate.Height = 30;
            this.dataGrid_jsgl.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid_jsgl.ShowCellErrors = false;
            this.dataGrid_jsgl.ShowRowErrors = false;
            this.dataGrid_jsgl.Size = new System.Drawing.Size(485, 353);
            this.dataGrid_jsgl.TabIndex = 29;
            // 
            // buttonJsglPreResult
            // 
            this.buttonJsglPreResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonJsglPreResult.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonJsglPreResult.Location = new System.Drawing.Point(497, 243);
            this.buttonJsglPreResult.Name = "buttonJsglPreResult";
            this.buttonJsglPreResult.Size = new System.Drawing.Size(247, 41);
            this.buttonJsglPreResult.TabIndex = 38;
            this.buttonJsglPreResult.Text = "上次结果";
            this.buttonJsglPreResult.UseVisualStyleBackColor = true;
            this.buttonJsglPreResult.Click += new System.EventHandler(this.buttonJsglPreResult_Click);
            // 
            // groupBox13
            // 
            this.groupBox13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox13.Controls.Add(this.textBox_Jsglqx_c);
            this.groupBox13.Controls.Add(this.label27);
            this.groupBox13.Controls.Add(this.textBox_Jsglqx_b);
            this.groupBox13.Controls.Add(this.label26);
            this.groupBox13.Controls.Add(this.textBox_Jsglqx_a);
            this.groupBox13.Controls.Add(this.label25);
            this.groupBox13.Location = new System.Drawing.Point(497, 463);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(247, 133);
            this.groupBox13.TabIndex = 39;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "功率曲线拟合系数(a*v^2+2*b*v+c)";
            // 
            // textBox_Jsglqx_c
            // 
            this.textBox_Jsglqx_c.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_Jsglqx_c.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_Jsglqx_c.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_Jsglqx_c.ForeColor = System.Drawing.Color.Blue;
            this.textBox_Jsglqx_c.Location = new System.Drawing.Point(80, 90);
            this.textBox_Jsglqx_c.Name = "textBox_Jsglqx_c";
            this.textBox_Jsglqx_c.Size = new System.Drawing.Size(132, 29);
            this.textBox_Jsglqx_c.TabIndex = 40;
            // 
            // label27
            // 
            this.label27.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label27.Location = new System.Drawing.Point(50, 97);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(22, 21);
            this.label27.TabIndex = 39;
            this.label27.Text = "c:";
            // 
            // textBox_Jsglqx_b
            // 
            this.textBox_Jsglqx_b.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_Jsglqx_b.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_Jsglqx_b.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_Jsglqx_b.ForeColor = System.Drawing.Color.Blue;
            this.textBox_Jsglqx_b.Location = new System.Drawing.Point(80, 55);
            this.textBox_Jsglqx_b.Name = "textBox_Jsglqx_b";
            this.textBox_Jsglqx_b.Size = new System.Drawing.Size(132, 29);
            this.textBox_Jsglqx_b.TabIndex = 38;
            // 
            // label26
            // 
            this.label26.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label26.Location = new System.Drawing.Point(50, 63);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(24, 21);
            this.label26.TabIndex = 37;
            this.label26.Text = "b:";
            // 
            // textBox_Jsglqx_a
            // 
            this.textBox_Jsglqx_a.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_Jsglqx_a.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_Jsglqx_a.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_Jsglqx_a.ForeColor = System.Drawing.Color.Blue;
            this.textBox_Jsglqx_a.Location = new System.Drawing.Point(80, 20);
            this.textBox_Jsglqx_a.Name = "textBox_Jsglqx_a";
            this.textBox_Jsglqx_a.Size = new System.Drawing.Size(132, 29);
            this.textBox_Jsglqx_a.TabIndex = 36;
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label25.Location = new System.Drawing.Point(49, 27);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(23, 21);
            this.label25.TabIndex = 35;
            this.label25.Text = "a:";
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(497, 370);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(247, 42);
            this.button1.TabIndex = 40;
            this.button1.Text = "保存结果";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.AutoToolTip = false;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(73, 29);
            this.toolStripButton1.Text = "快速刹车";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1238, 633);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox13);
            this.Controls.Add(this.buttonJsglPreResult);
            this.Controls.Add(this.dataGrid_jsgl);
            this.Controls.Add(this.panel12);
            this.Controls.Add(this.chart_jsg);
            this.Controls.Add(this.button_jsgqx);
            this.Controls.Add(this.button_start_jsg);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.label_24jsg);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label_40jsg);
            this.Controls.Add(this.label16);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "汽油寄生功率";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_exit);
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_jsg)).EndInit();
            this.panel12.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel_cs.ResumeLayout(false);
            this.panel_cs.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel_gl.ResumeLayout(false);
            this.panel_gl.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel_nl.ResumeLayout(false);
            this.panel_nl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_jsgl)).EndInit();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.ToolStripLabel toolStripLabelMessage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonForceClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonLiftUp;
        private System.Windows.Forms.ToolStripButton toolStripButtonLiftDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonMotorOn;
        private System.Windows.Forms.ToolStripButton toolStripButtonMotorOff;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButtonStopTest;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrintScreen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButtonExit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_jsg;
        private System.Windows.Forms.Button button_jsgqx;
        private System.Windows.Forms.Button button_start_jsg;
        private System.Windows.Forms.Label label_24jsg;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label_40jsg;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel_cs;
        private System.Windows.Forms.Label Msg_cs;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel_gl;
        private System.Windows.Forms.Label Msg_gl;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel_nl;
        private System.Windows.Forms.Label Msg_nl;
        private System.Windows.Forms.DataGridView dataGrid_jsgl;
        private System.Windows.Forms.Label labelStandard;
        private System.Windows.Forms.Button buttonJsglPreResult;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.TextBox textBox_Jsglqx_c;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox textBox_Jsglqx_b;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox textBox_Jsglqx_a;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel提示信息;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}

