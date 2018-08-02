using System.Windows.Forms.DataVisualization.Charting;
namespace 加载滑行
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
            this.buttonSaveData = new System.Windows.Forms.Button();
            this.button_jzks = new System.Windows.Forms.Button();
            this.dataGrid_jzhx = new System.Windows.Forms.DataGridView();
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
            this.textBoxYjzForce = new System.Windows.Forms.TextBox();
            this.checkBoxYjzForce = new System.Windows.Forms.CheckBox();
            this.comboBoxHxqj = new System.Windows.Forms.ComboBox();
            this.label29 = new System.Windows.Forms.Label();
            this.textBox_zsgl = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel7.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_jzhx)).BeginInit();
            this.panel12.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel_cs.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel_gl.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel_nl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
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
            this.label12.Location = new System.Drawing.Point(379, 38);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(463, 52);
            this.label12.TabIndex = 1;
            this.label12.Text = "底盘测功机加载滑行试验";
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
            this.panel7.Size = new System.Drawing.Size(1200, 115);
            this.panel7.TabIndex = 18;
            // 
            // labelStandard
            // 
            this.labelStandard.AutoSize = true;
            this.labelStandard.Font = new System.Drawing.Font("微软雅黑", 25F, System.Drawing.FontStyle.Bold);
            this.labelStandard.Location = new System.Drawing.Point(938, 66);
            this.labelStandard.Name = "labelStandard";
            this.labelStandard.Size = new System.Drawing.Size(250, 45);
            this.labelStandard.TabIndex = 2;
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
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // toolStripButtonForceClear
            // 
            this.toolStripButtonForceClear.AutoToolTip = false;
            this.toolStripButtonForceClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonForceClear.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonForceClear.Image")));
            this.toolStripButtonForceClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonForceClear.Name = "toolStripButtonForceClear";
            this.toolStripButtonForceClear.Size = new System.Drawing.Size(73, 30);
            this.toolStripButtonForceClear.Text = "扭力清零";
            this.toolStripButtonForceClear.Click += new System.EventHandler(this.toolStripButtonForceClear_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
            // 
            // toolStripButtonLiftUp
            // 
            this.toolStripButtonLiftUp.AutoToolTip = false;
            this.toolStripButtonLiftUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonLiftUp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLiftUp.Image")));
            this.toolStripButtonLiftUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLiftUp.Name = "toolStripButtonLiftUp";
            this.toolStripButtonLiftUp.Size = new System.Drawing.Size(58, 30);
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
            this.toolStripButtonLiftDown.Size = new System.Drawing.Size(58, 30);
            this.toolStripButtonLiftDown.Text = "举升降";
            this.toolStripButtonLiftDown.Click += new System.EventHandler(this.toolStripButtonLiftDown_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 33);
            // 
            // toolStripButtonMotorOn
            // 
            this.toolStripButtonMotorOn.AutoToolTip = false;
            this.toolStripButtonMotorOn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonMotorOn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMotorOn.Image")));
            this.toolStripButtonMotorOn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMotorOn.Name = "toolStripButtonMotorOn";
            this.toolStripButtonMotorOn.Size = new System.Drawing.Size(73, 30);
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
            this.toolStripButtonMotorOff.Size = new System.Drawing.Size(73, 30);
            this.toolStripButtonMotorOff.Text = "停止电机";
            this.toolStripButtonMotorOff.Click += new System.EventHandler(this.toolStripButtonMotorOff_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 33);
            // 
            // toolStripButtonStopTest
            // 
            this.toolStripButtonStopTest.AutoToolTip = false;
            this.toolStripButtonStopTest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonStopTest.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStopTest.Image")));
            this.toolStripButtonStopTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStopTest.Name = "toolStripButtonStopTest";
            this.toolStripButtonStopTest.Size = new System.Drawing.Size(73, 30);
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
            this.toolStripButtonPrintScreen.Size = new System.Drawing.Size(43, 30);
            this.toolStripButtonPrintScreen.Text = "截屏";
            this.toolStripButtonPrintScreen.Click += new System.EventHandler(this.toolStripButtonPrintScreen_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 33);
            // 
            // toolStripButtonExit
            // 
            this.toolStripButtonExit.AutoToolTip = false;
            this.toolStripButtonExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonExit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExit.Image")));
            this.toolStripButtonExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExit.Name = "toolStripButtonExit";
            this.toolStripButtonExit.Size = new System.Drawing.Size(43, 30);
            this.toolStripButtonExit.Text = "退出";
            this.toolStripButtonExit.Click += new System.EventHandler(this.toolStripButtonExit_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 33);
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
            this.toolStrip1.Location = new System.Drawing.Point(3, 667);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1191, 33);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 17;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel提示信息
            // 
            this.toolStripLabel提示信息.Name = "toolStripLabel提示信息";
            this.toolStripLabel提示信息.Size = new System.Drawing.Size(69, 30);
            this.toolStripLabel提示信息.Text = "提示信息";
            // 
            // buttonSaveData
            // 
            this.buttonSaveData.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonSaveData.Location = new System.Drawing.Point(463, 598);
            this.buttonSaveData.Name = "buttonSaveData";
            this.buttonSaveData.Size = new System.Drawing.Size(287, 40);
            this.buttonSaveData.TabIndex = 24;
            this.buttonSaveData.Text = "保存结果";
            this.buttonSaveData.UseVisualStyleBackColor = true;
            this.buttonSaveData.Click += new System.EventHandler(this.buttonSaveData_Click);
            // 
            // button_jzks
            // 
            this.button_jzks.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_jzks.Location = new System.Drawing.Point(463, 552);
            this.button_jzks.Name = "button_jzks";
            this.button_jzks.Size = new System.Drawing.Size(287, 40);
            this.button_jzks.TabIndex = 22;
            this.button_jzks.Text = "开始测试";
            this.button_jzks.UseVisualStyleBackColor = true;
            this.button_jzks.Click += new System.EventHandler(this.button_jzks_Click);
            // 
            // dataGrid_jzhx
            // 
            this.dataGrid_jzhx.AllowUserToAddRows = false;
            this.dataGrid_jzhx.AllowUserToDeleteRows = false;
            this.dataGrid_jzhx.AllowUserToOrderColumns = true;
            this.dataGrid_jzhx.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            this.dataGrid_jzhx.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGrid_jzhx.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGrid_jzhx.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 14F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGrid_jzhx.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGrid_jzhx.ColumnHeadersHeight = 55;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 14F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGrid_jzhx.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGrid_jzhx.GridColor = System.Drawing.Color.Gray;
            this.dataGrid_jzhx.Location = new System.Drawing.Point(3, 243);
            this.dataGrid_jzhx.Name = "dataGrid_jzhx";
            this.dataGrid_jzhx.ReadOnly = true;
            this.dataGrid_jzhx.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGrid_jzhx.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dataGrid_jzhx.RowHeadersVisible = false;
            this.dataGrid_jzhx.RowHeadersWidth = 40;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            this.dataGrid_jzhx.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dataGrid_jzhx.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGrid_jzhx.RowTemplate.DefaultCellStyle.Format = "D";
            this.dataGrid_jzhx.RowTemplate.DefaultCellStyle.NullValue = null;
            this.dataGrid_jzhx.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            this.dataGrid_jzhx.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGrid_jzhx.RowTemplate.Height = 30;
            this.dataGrid_jzhx.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid_jzhx.ShowCellErrors = false;
            this.dataGrid_jzhx.ShowRowErrors = false;
            this.dataGrid_jzhx.Size = new System.Drawing.Size(1191, 178);
            this.dataGrid_jzhx.TabIndex = 12;
            this.dataGrid_jzhx.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGrid_jzhx_CellContentClick);
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
            this.panel12.Size = new System.Drawing.Size(1200, 122);
            this.panel12.TabIndex = 25;
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
            // textBoxYjzForce
            // 
            this.textBoxYjzForce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxYjzForce.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxYjzForce.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxYjzForce.Location = new System.Drawing.Point(612, 440);
            this.textBoxYjzForce.Name = "textBoxYjzForce";
            this.textBoxYjzForce.Size = new System.Drawing.Size(121, 29);
            this.textBoxYjzForce.TabIndex = 34;
            // 
            // checkBoxYjzForce
            // 
            this.checkBoxYjzForce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxYjzForce.AutoSize = true;
            this.checkBoxYjzForce.Font = new System.Drawing.Font("宋体", 15F);
            this.checkBoxYjzForce.Location = new System.Drawing.Point(473, 444);
            this.checkBoxYjzForce.Name = "checkBoxYjzForce";
            this.checkBoxYjzForce.Size = new System.Drawing.Size(128, 24);
            this.checkBoxYjzForce.TabIndex = 33;
            this.checkBoxYjzForce.Text = "是否预加载";
            this.checkBoxYjzForce.UseVisualStyleBackColor = true;
            this.checkBoxYjzForce.CheckedChanged += new System.EventHandler(this.checkBoxYjzForce_CheckedChanged);
            // 
            // comboBoxHxqj
            // 
            this.comboBoxHxqj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxHxqj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxHxqj.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxHxqj.FormattingEnabled = true;
            this.comboBoxHxqj.Items.AddRange(new object[] {
            "64km/h~48km/h",
            "48km/h~32km/h",
            "33km/h~17km/h",
            "32km/h~16km/h"});
            this.comboBoxHxqj.Location = new System.Drawing.Point(587, 474);
            this.comboBoxHxqj.Name = "comboBoxHxqj";
            this.comboBoxHxqj.Size = new System.Drawing.Size(146, 28);
            this.comboBoxHxqj.TabIndex = 38;
            // 
            // label29
            // 
            this.label29.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label29.Font = new System.Drawing.Font("宋体", 15F);
            this.label29.Location = new System.Drawing.Point(468, 508);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(153, 23);
            this.label29.TabIndex = 37;
            this.label29.Text = "加载设定(THP):";
            // 
            // textBox_zsgl
            // 
            this.textBox_zsgl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_zsgl.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_zsgl.FormattingEnabled = true;
            this.textBox_zsgl.Items.AddRange(new object[] {
            "6",
            "12",
            "4",
            "8",
            "15"});
            this.textBox_zsgl.Location = new System.Drawing.Point(624, 506);
            this.textBox_zsgl.Name = "textBox_zsgl";
            this.textBox_zsgl.Size = new System.Drawing.Size(109, 28);
            this.textBox_zsgl.TabIndex = 36;
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label23.Font = new System.Drawing.Font("宋体", 15F);
            this.label23.Location = new System.Drawing.Point(469, 477);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(128, 23);
            this.label23.TabIndex = 35;
            this.label23.Text = "滑行区间：";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(9, 429);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(439, 149);
            this.dataGridView1.TabIndex = 39;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(9, 588);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(439, 57);
            this.label1.TabIndex = 40;
            this.label1.Text = "(说明：\r\n必须完成48~32，33~17两个区间的测试，保存的结果才有效)";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.AutoToolTip = false;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(73, 30);
            this.toolStripButton1.Text = "快速刹车";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1200, 702);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBoxYjzForce);
            this.Controls.Add(this.checkBoxYjzForce);
            this.Controls.Add(this.comboBoxHxqj);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.textBox_zsgl);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.panel12);
            this.Controls.Add(this.buttonSaveData);
            this.Controls.Add(this.button_jzks);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.dataGrid_jzhx);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "加载滑行";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_exit);
            this.Load += new System.EventHandler(this.Main_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_jzhx)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
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
        private System.Windows.Forms.Button button_jzks;
        private System.Windows.Forms.DataGridView dataGrid_jzhx;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button buttonSaveData;
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
        private System.Windows.Forms.Label labelStandard;
        private System.Windows.Forms.TextBox textBoxYjzForce;
        private System.Windows.Forms.CheckBox checkBoxYjzForce;
        private System.Windows.Forms.ComboBox comboBoxHxqj;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.ComboBox textBox_zsgl;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ToolStripLabel toolStripLabel提示信息;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}

