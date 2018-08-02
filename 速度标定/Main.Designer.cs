using System.Windows.Forms.DataVisualization.Charting;
namespace 速度标定
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label12 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
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
            this.dataGridViewSpeed = new System.Windows.Forms.DataGridView();
            this.chartSpeed = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.radioButtonCar = new System.Windows.Forms.RadioButton();
            this.radioButtonSerial = new System.Windows.Forms.RadioButton();
            this.radioButtonKeyboard = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_read_speed_dp = new System.Windows.Forms.Button();
            this.button_demarcate_speed = new System.Windows.Forms.Button();
            this.textBox_pulse = new System.Windows.Forms.TextBox();
            this.textBox_diameter = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label44 = new System.Windows.Forms.Label();
            this.buttonStartSpeed = new System.Windows.Forms.Button();
            this.label40 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.textBoxStandardSpeed = new System.Windows.Forms.TextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.textBoxRealSpeed = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.buttonAddSpeed = new System.Windows.Forms.Button();
            this.textBoxMubiaoSpeed = new System.Windows.Forms.TextBox();
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
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel7.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartSpeed)).BeginInit();
            this.groupBox12.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel_cs.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel_gl.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel_nl.SuspendLayout();
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
            this.label12.Location = new System.Drawing.Point(300, 36);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(383, 52);
            this.label12.TabIndex = 1;
            this.label12.Text = "底盘测功机速度标定";
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.CadetBlue;
            this.panel7.Controls.Add(this.label12);
            this.panel7.Controls.Add(this.pictureBox1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(986, 115);
            this.panel7.TabIndex = 18;
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
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 30);
            // 
            // toolStripButtonForceClear
            // 
            this.toolStripButtonForceClear.AutoToolTip = false;
            this.toolStripButtonForceClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonForceClear.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonForceClear.Image")));
            this.toolStripButtonForceClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonForceClear.Name = "toolStripButtonForceClear";
            this.toolStripButtonForceClear.Size = new System.Drawing.Size(73, 27);
            this.toolStripButtonForceClear.Text = "扭力清零";
            this.toolStripButtonForceClear.Click += new System.EventHandler(this.toolStripButtonForceClear_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 30);
            // 
            // toolStripButtonLiftUp
            // 
            this.toolStripButtonLiftUp.AutoToolTip = false;
            this.toolStripButtonLiftUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonLiftUp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLiftUp.Image")));
            this.toolStripButtonLiftUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLiftUp.Name = "toolStripButtonLiftUp";
            this.toolStripButtonLiftUp.Size = new System.Drawing.Size(58, 27);
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
            this.toolStripButtonLiftDown.Size = new System.Drawing.Size(58, 27);
            this.toolStripButtonLiftDown.Text = "举升降";
            this.toolStripButtonLiftDown.Click += new System.EventHandler(this.toolStripButtonLiftDown_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 30);
            // 
            // toolStripButtonMotorOn
            // 
            this.toolStripButtonMotorOn.AutoToolTip = false;
            this.toolStripButtonMotorOn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonMotorOn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMotorOn.Image")));
            this.toolStripButtonMotorOn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMotorOn.Name = "toolStripButtonMotorOn";
            this.toolStripButtonMotorOn.Size = new System.Drawing.Size(73, 27);
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
            this.toolStripButtonMotorOff.Size = new System.Drawing.Size(73, 27);
            this.toolStripButtonMotorOff.Text = "停止电机";
            this.toolStripButtonMotorOff.Click += new System.EventHandler(this.toolStripButtonMotorOff_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 30);
            // 
            // toolStripButtonStopTest
            // 
            this.toolStripButtonStopTest.AutoToolTip = false;
            this.toolStripButtonStopTest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonStopTest.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStopTest.Image")));
            this.toolStripButtonStopTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStopTest.Name = "toolStripButtonStopTest";
            this.toolStripButtonStopTest.Size = new System.Drawing.Size(73, 27);
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
            this.toolStripButtonPrintScreen.Size = new System.Drawing.Size(43, 27);
            this.toolStripButtonPrintScreen.Text = "截屏";
            this.toolStripButtonPrintScreen.Click += new System.EventHandler(this.toolStripButtonPrintScreen_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 30);
            // 
            // toolStripButtonExit
            // 
            this.toolStripButtonExit.AutoToolTip = false;
            this.toolStripButtonExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonExit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExit.Image")));
            this.toolStripButtonExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExit.Name = "toolStripButtonExit";
            this.toolStripButtonExit.Size = new System.Drawing.Size(43, 27);
            this.toolStripButtonExit.Text = "退出";
            this.toolStripButtonExit.Click += new System.EventHandler(this.toolStripButtonExit_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 30);
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
            this.toolStripButtonExit,
            this.toolStripSeparator6});
            this.toolStrip1.Location = new System.Drawing.Point(3, 670);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(977, 30);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 17;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // dataGridViewSpeed
            // 
            this.dataGridViewSpeed.AllowUserToAddRows = false;
            this.dataGridViewSpeed.AllowUserToDeleteRows = false;
            this.dataGridViewSpeed.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            this.dataGridViewSpeed.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewSpeed.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSpeed.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewSpeed.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewSpeed.ColumnHeadersHeight = 46;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 14F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewSpeed.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewSpeed.GridColor = System.Drawing.Color.Gray;
            this.dataGridViewSpeed.Location = new System.Drawing.Point(512, 394);
            this.dataGridViewSpeed.Name = "dataGridViewSpeed";
            this.dataGridViewSpeed.ReadOnly = true;
            this.dataGridViewSpeed.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewSpeed.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridViewSpeed.RowHeadersVisible = false;
            this.dataGridViewSpeed.RowHeadersWidth = 40;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            this.dataGridViewSpeed.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridViewSpeed.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGridViewSpeed.RowTemplate.DefaultCellStyle.Format = "D";
            this.dataGridViewSpeed.RowTemplate.DefaultCellStyle.NullValue = null;
            this.dataGridViewSpeed.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            this.dataGridViewSpeed.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewSpeed.RowTemplate.Height = 30;
            this.dataGridViewSpeed.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewSpeed.ShowCellErrors = false;
            this.dataGridViewSpeed.ShowRowErrors = false;
            this.dataGridViewSpeed.Size = new System.Drawing.Size(468, 273);
            this.dataGridViewSpeed.TabIndex = 31;
            // 
            // chartSpeed
            // 
            chartArea2.AxisX.Maximum = 100D;
            chartArea2.AxisX.Minimum = 0D;
            chartArea2.AxisX.Title = "标准速度（Km/h）";
            chartArea2.AxisY.Title = "实测速度（Km/h）";
            chartArea2.Name = "ChartArea1";
            this.chartSpeed.ChartAreas.Add(chartArea2);
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend2.Name = "Legend1";
            this.chartSpeed.Legends.Add(legend2);
            this.chartSpeed.Location = new System.Drawing.Point(8, 394);
            this.chartSpeed.Name = "chartSpeed";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series2.LabelForeColor = System.Drawing.Color.Red;
            series2.Legend = "Legend1";
            series2.LegendText = "拟合曲线";
            series2.Name = "speed_qx";
            this.chartSpeed.Series.Add(series2);
            this.chartSpeed.Size = new System.Drawing.Size(492, 273);
            this.chartSpeed.TabIndex = 31;
            this.chartSpeed.Text = "chart1";
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.radioButtonCar);
            this.groupBox12.Controls.Add(this.radioButtonSerial);
            this.groupBox12.Controls.Add(this.radioButtonKeyboard);
            this.groupBox12.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox12.Location = new System.Drawing.Point(3, 243);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(180, 145);
            this.groupBox12.TabIndex = 36;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "速度控制方式";
            // 
            // radioButtonCar
            // 
            this.radioButtonCar.AutoSize = true;
            this.radioButtonCar.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButtonCar.Location = new System.Drawing.Point(23, 109);
            this.radioButtonCar.Name = "radioButtonCar";
            this.radioButtonCar.Size = new System.Drawing.Size(117, 24);
            this.radioButtonCar.TabIndex = 1;
            this.radioButtonCar.Text = "汽车恒速模式";
            this.radioButtonCar.UseVisualStyleBackColor = true;
            // 
            // radioButtonSerial
            // 
            this.radioButtonSerial.AutoSize = true;
            this.radioButtonSerial.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButtonSerial.Location = new System.Drawing.Point(23, 80);
            this.radioButtonSerial.Name = "radioButtonSerial";
            this.radioButtonSerial.Size = new System.Drawing.Size(132, 24);
            this.radioButtonSerial.TabIndex = 0;
            this.radioButtonSerial.Text = "串口控制变频器";
            this.radioButtonSerial.UseVisualStyleBackColor = true;
            // 
            // radioButtonKeyboard
            // 
            this.radioButtonKeyboard.AutoSize = true;
            this.radioButtonKeyboard.Checked = true;
            this.radioButtonKeyboard.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButtonKeyboard.Location = new System.Drawing.Point(23, 48);
            this.radioButtonKeyboard.Name = "radioButtonKeyboard";
            this.radioButtonKeyboard.Size = new System.Drawing.Size(132, 24);
            this.radioButtonKeyboard.TabIndex = 0;
            this.radioButtonKeyboard.TabStop = true;
            this.radioButtonKeyboard.Text = "键盘控制变频器";
            this.radioButtonKeyboard.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_read_speed_dp);
            this.groupBox2.Controls.Add(this.button_demarcate_speed);
            this.groupBox2.Controls.Add(this.textBox_pulse);
            this.groupBox2.Controls.Add(this.textBox_diameter);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(588, 243);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(386, 145);
            this.groupBox2.TabIndex = 40;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "车速标定";
            // 
            // button_read_speed_dp
            // 
            this.button_read_speed_dp.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_read_speed_dp.Location = new System.Drawing.Point(93, 78);
            this.button_read_speed_dp.Name = "button_read_speed_dp";
            this.button_read_speed_dp.Size = new System.Drawing.Size(101, 36);
            this.button_read_speed_dp.TabIndex = 11;
            this.button_read_speed_dp.Text = "读取参数";
            this.button_read_speed_dp.UseVisualStyleBackColor = true;
            this.button_read_speed_dp.Click += new System.EventHandler(this.button_read_speed_dp_Click_1);
            // 
            // button_demarcate_speed
            // 
            this.button_demarcate_speed.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_demarcate_speed.Location = new System.Drawing.Point(204, 78);
            this.button_demarcate_speed.Name = "button_demarcate_speed";
            this.button_demarcate_speed.Size = new System.Drawing.Size(96, 36);
            this.button_demarcate_speed.TabIndex = 0;
            this.button_demarcate_speed.Text = "标定速度";
            this.button_demarcate_speed.UseVisualStyleBackColor = true;
            this.button_demarcate_speed.Click += new System.EventHandler(this.button_demarcate_speed_Click_1);
            // 
            // textBox_pulse
            // 
            this.textBox_pulse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.textBox_pulse.Location = new System.Drawing.Point(300, 31);
            this.textBox_pulse.Name = "textBox_pulse";
            this.textBox_pulse.Size = new System.Drawing.Size(76, 27);
            this.textBox_pulse.TabIndex = 7;
            // 
            // textBox_diameter
            // 
            this.textBox_diameter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.textBox_diameter.Location = new System.Drawing.Point(143, 31);
            this.textBox_diameter.Name = "textBox_diameter";
            this.textBox_diameter.Size = new System.Drawing.Size(80, 27);
            this.textBox_diameter.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(224, 33);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 21);
            this.label10.TabIndex = 9;
            this.label10.Text = "脉冲数：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(9, 33);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(130, 21);
            this.label11.TabIndex = 10;
            this.label11.Text = "滚筒直径(mm)：";
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.button1);
            this.groupBox15.Controls.Add(this.label44);
            this.groupBox15.Controls.Add(this.buttonStartSpeed);
            this.groupBox15.Controls.Add(this.label40);
            this.groupBox15.Controls.Add(this.label41);
            this.groupBox15.Controls.Add(this.textBoxStandardSpeed);
            this.groupBox15.Controls.Add(this.label37);
            this.groupBox15.Controls.Add(this.label39);
            this.groupBox15.Controls.Add(this.textBoxRealSpeed);
            this.groupBox15.Controls.Add(this.label14);
            this.groupBox15.Controls.Add(this.label13);
            this.groupBox15.Controls.Add(this.buttonAddSpeed);
            this.groupBox15.Controls.Add(this.textBoxMubiaoSpeed);
            this.groupBox15.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox15.Location = new System.Drawing.Point(194, 243);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(379, 145);
            this.groupBox15.TabIndex = 37;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "标定内容";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(272, 99);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 33);
            this.button1.TabIndex = 24;
            this.button1.Text = "保存结果";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonSaveResult_Click);
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label44.Location = new System.Drawing.Point(75, 23);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(139, 20);
            this.label44.TabIndex = 23;
            this.label44.Text = "(待速度稳定后添加)";
            // 
            // buttonStartSpeed
            // 
            this.buttonStartSpeed.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonStartSpeed.Location = new System.Drawing.Point(272, 23);
            this.buttonStartSpeed.Name = "buttonStartSpeed";
            this.buttonStartSpeed.Size = new System.Drawing.Size(94, 35);
            this.buttonStartSpeed.TabIndex = 22;
            this.buttonStartSpeed.Text = "执行";
            this.buttonStartSpeed.UseVisualStyleBackColor = true;
            this.buttonStartSpeed.Click += new System.EventHandler(this.buttonStartSpeed_Click);
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label40.Location = new System.Drawing.Point(208, 106);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(52, 21);
            this.label40.TabIndex = 21;
            this.label40.Text = "Km/h";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label41.Location = new System.Drawing.Point(11, 104);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(84, 20);
            this.label41.TabIndex = 20;
            this.label41.Text = "标准速度：";
            // 
            // textBoxStandardSpeed
            // 
            this.textBoxStandardSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxStandardSpeed.Font = new System.Drawing.Font("宋体", 12F);
            this.textBoxStandardSpeed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.textBoxStandardSpeed.Location = new System.Drawing.Point(99, 101);
            this.textBoxStandardSpeed.Name = "textBoxStandardSpeed";
            this.textBoxStandardSpeed.Size = new System.Drawing.Size(100, 26);
            this.textBoxStandardSpeed.TabIndex = 19;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label37.Location = new System.Drawing.Point(208, 78);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(52, 21);
            this.label37.TabIndex = 18;
            this.label37.Text = "Km/h";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label39.Location = new System.Drawing.Point(11, 76);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(84, 20);
            this.label39.TabIndex = 17;
            this.label39.Text = "实测速度：";
            // 
            // textBoxRealSpeed
            // 
            this.textBoxRealSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxRealSpeed.Font = new System.Drawing.Font("宋体", 12F);
            this.textBoxRealSpeed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.textBoxRealSpeed.Location = new System.Drawing.Point(99, 73);
            this.textBoxRealSpeed.Name = "textBoxRealSpeed";
            this.textBoxRealSpeed.Size = new System.Drawing.Size(100, 26);
            this.textBoxRealSpeed.TabIndex = 16;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(208, 46);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 21);
            this.label14.TabIndex = 15;
            this.label14.Text = "Km/h";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(11, 50);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(84, 20);
            this.label13.TabIndex = 13;
            this.label13.Text = "目标速度：";
            // 
            // buttonAddSpeed
            // 
            this.buttonAddSpeed.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonAddSpeed.Location = new System.Drawing.Point(272, 62);
            this.buttonAddSpeed.Name = "buttonAddSpeed";
            this.buttonAddSpeed.Size = new System.Drawing.Size(94, 33);
            this.buttonAddSpeed.TabIndex = 14;
            this.buttonAddSpeed.Text = "添加";
            this.buttonAddSpeed.UseVisualStyleBackColor = true;
            this.buttonAddSpeed.Click += new System.EventHandler(this.buttonAddSpeed_Click);
            // 
            // textBoxMubiaoSpeed
            // 
            this.textBoxMubiaoSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxMubiaoSpeed.Font = new System.Drawing.Font("宋体", 12F);
            this.textBoxMubiaoSpeed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.textBoxMubiaoSpeed.Location = new System.Drawing.Point(99, 46);
            this.textBoxMubiaoSpeed.Name = "textBoxMubiaoSpeed";
            this.textBoxMubiaoSpeed.Size = new System.Drawing.Size(100, 26);
            this.textBoxMubiaoSpeed.TabIndex = 12;
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
            this.panel12.Size = new System.Drawing.Size(986, 122);
            this.panel12.TabIndex = 41;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label2);
            this.panel6.Controls.Add(this.panel_cs);
            this.panel6.Location = new System.Drawing.Point(177, 6);
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
            this.panel11.Location = new System.Drawing.Point(599, 6);
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
            this.panel8.Location = new System.Drawing.Point(388, 6);
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
            // toolStripButton1
            // 
            this.toolStripButton1.AutoToolTip = false;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(73, 27);
            this.toolStripButton1.Text = "快速刹车";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ClientSize = new System.Drawing.Size(986, 702);
            this.Controls.Add(this.chartSpeed);
            this.Controls.Add(this.dataGridViewSpeed);
            this.Controls.Add(this.groupBox12);
            this.Controls.Add(this.panel12);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.groupBox15);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "速度标定";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_exit);
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartSpeed)).EndInit();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
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
            this.ResumeLayout(false);

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
        private System.Windows.Forms.DataGridView dataGridViewSpeed;
        private Chart chartSpeed;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.RadioButton radioButtonCar;
        private System.Windows.Forms.RadioButton radioButtonSerial;
        private System.Windows.Forms.RadioButton radioButtonKeyboard;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_read_speed_dp;
        private System.Windows.Forms.Button button_demarcate_speed;
        private System.Windows.Forms.TextBox textBox_pulse;
        private System.Windows.Forms.TextBox textBox_diameter;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Button buttonStartSpeed;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.TextBox textBoxStandardSpeed;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.TextBox textBoxRealSpeed;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button buttonAddSpeed;
        private System.Windows.Forms.TextBox textBoxMubiaoSpeed;
        private System.Windows.Forms.Button button1;
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
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}

