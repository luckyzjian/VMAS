namespace 流量计标定
{
    partial class Form1
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
            this.panel_msg = new System.Windows.Forms.Panel();
            this.label_msg = new System.Windows.Forms.Label();
            this.textEditll = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxO2 = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxQtyl = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBoxQtwd = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.radioButtonBzsj = new System.Windows.Forms.RadioButton();
            this.radioButtonSssj = new System.Windows.Forms.RadioButton();
            this.textEditBiaoO2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxbzO2 = new System.Windows.Forms.TextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.项目 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.氧气标准值 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.氧气实测值 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.氧气误差 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.标定结果 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.备注说明 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.buttonSavedata = new System.Windows.Forms.Button();
            this.radioButtonLow = new System.Windows.Forms.RadioButton();
            this.radioButtonHigh = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonMidO2 = new System.Windows.Forms.RadioButton();
            this.radioButtonHighO2 = new System.Windows.Forms.RadioButton();
            this.radioButtonLowO2 = new System.Windows.Forms.RadioButton();
            this.buttonO2zero = new System.Windows.Forms.Button();
            this.textBoxBiaoLL = new System.Windows.Forms.TextBox();
            this.radioButtonLowLL = new System.Windows.Forms.RadioButton();
            this.buttonLLdemarcate = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButtonHighLL = new System.Windows.Forms.RadioButton();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.textBoxYL = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonYldemarcate = new System.Windows.Forms.Button();
            this.textBoxTemp = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonTempdemarcate = new System.Windows.Forms.Button();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.buttonTurnOffMotor = new System.Windows.Forms.Button();
            this.buttonTurnOnMotor = new System.Windows.Forms.Button();
            this.radioButtonLLsingle = new System.Windows.Forms.RadioButton();
            this.button4 = new System.Windows.Forms.Button();
            this.panel_msg.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox9.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_msg
            // 
            this.panel_msg.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel_msg.BackColor = System.Drawing.SystemColors.Desktop;
            this.panel_msg.Controls.Add(this.label_msg);
            this.panel_msg.Location = new System.Drawing.Point(2, 3);
            this.panel_msg.Name = "panel_msg";
            this.panel_msg.Size = new System.Drawing.Size(794, 45);
            this.panel_msg.TabIndex = 41;
            // 
            // label_msg
            // 
            this.label_msg.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label_msg.AutoSize = true;
            this.label_msg.Font = new System.Drawing.Font("黑体", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_msg.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label_msg.Location = new System.Drawing.Point(216, 6);
            this.label_msg.Name = "label_msg";
            this.label_msg.Size = new System.Drawing.Size(378, 33);
            this.label_msg.TabIndex = 0;
            this.label_msg.Text = "完成检查后点击保存数据";
            // 
            // textEditll
            // 
            this.textEditll.BackColor = System.Drawing.Color.Black;
            this.textEditll.Font = new System.Drawing.Font("宋体", 40F, System.Drawing.FontStyle.Bold);
            this.textEditll.ForeColor = System.Drawing.Color.Lime;
            this.textEditll.Location = new System.Drawing.Point(6, 24);
            this.textEditll.Name = "textEditll";
            this.textEditll.ReadOnly = true;
            this.textEditll.Size = new System.Drawing.Size(182, 68);
            this.textEditll.TabIndex = 42;
            this.textEditll.TabStop = false;
            this.textEditll.Text = "00.0";
            this.textEditll.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textEditll);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 15F);
            this.groupBox1.Location = new System.Drawing.Point(2, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(194, 103);
            this.groupBox1.TabIndex = 43;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "流量值";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxO2);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 15F);
            this.groupBox2.Location = new System.Drawing.Point(202, 54);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(194, 103);
            this.groupBox2.TabIndex = 44;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "O2浓度";
            // 
            // textBoxO2
            // 
            this.textBoxO2.BackColor = System.Drawing.Color.Black;
            this.textBoxO2.Font = new System.Drawing.Font("宋体", 40F, System.Drawing.FontStyle.Bold);
            this.textBoxO2.ForeColor = System.Drawing.Color.Lime;
            this.textBoxO2.Location = new System.Drawing.Point(6, 24);
            this.textBoxO2.Name = "textBoxO2";
            this.textBoxO2.ReadOnly = true;
            this.textBoxO2.Size = new System.Drawing.Size(182, 68);
            this.textBoxO2.TabIndex = 42;
            this.textBoxO2.TabStop = false;
            this.textBoxO2.Text = "00.0";
            this.textBoxO2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBoxQtyl);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 15F);
            this.groupBox3.Location = new System.Drawing.Point(402, 54);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(194, 103);
            this.groupBox3.TabIndex = 45;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "气体压力";
            // 
            // textBoxQtyl
            // 
            this.textBoxQtyl.BackColor = System.Drawing.Color.Black;
            this.textBoxQtyl.Font = new System.Drawing.Font("宋体", 40F, System.Drawing.FontStyle.Bold);
            this.textBoxQtyl.ForeColor = System.Drawing.Color.Lime;
            this.textBoxQtyl.Location = new System.Drawing.Point(6, 24);
            this.textBoxQtyl.Name = "textBoxQtyl";
            this.textBoxQtyl.ReadOnly = true;
            this.textBoxQtyl.Size = new System.Drawing.Size(182, 68);
            this.textBoxQtyl.TabIndex = 42;
            this.textBoxQtyl.TabStop = false;
            this.textBoxQtyl.Text = "00.0";
            this.textBoxQtyl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBoxQtwd);
            this.groupBox4.Font = new System.Drawing.Font("宋体", 15F);
            this.groupBox4.Location = new System.Drawing.Point(602, 54);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(194, 103);
            this.groupBox4.TabIndex = 45;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "气体温度";
            // 
            // textBoxQtwd
            // 
            this.textBoxQtwd.BackColor = System.Drawing.Color.Black;
            this.textBoxQtwd.Font = new System.Drawing.Font("宋体", 40F, System.Drawing.FontStyle.Bold);
            this.textBoxQtwd.ForeColor = System.Drawing.Color.Lime;
            this.textBoxQtwd.Location = new System.Drawing.Point(6, 24);
            this.textBoxQtwd.Name = "textBoxQtwd";
            this.textBoxQtwd.ReadOnly = true;
            this.textBoxQtwd.Size = new System.Drawing.Size(182, 68);
            this.textBoxQtwd.TabIndex = 42;
            this.textBoxQtwd.TabStop = false;
            this.textBoxQtwd.Text = "00.0";
            this.textBoxQtwd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.radioButtonBzsj);
            this.groupBox5.Controls.Add(this.radioButtonSssj);
            this.groupBox5.Location = new System.Drawing.Point(8, 164);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(110, 95);
            this.groupBox5.TabIndex = 46;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "数据选择";
            // 
            // radioButtonBzsj
            // 
            this.radioButtonBzsj.AutoSize = true;
            this.radioButtonBzsj.Font = new System.Drawing.Font("宋体", 12F);
            this.radioButtonBzsj.Location = new System.Drawing.Point(13, 56);
            this.radioButtonBzsj.Name = "radioButtonBzsj";
            this.radioButtonBzsj.Size = new System.Drawing.Size(90, 20);
            this.radioButtonBzsj.TabIndex = 1;
            this.radioButtonBzsj.TabStop = true;
            this.radioButtonBzsj.Text = "标准数据";
            this.radioButtonBzsj.UseVisualStyleBackColor = true;
            // 
            // radioButtonSssj
            // 
            this.radioButtonSssj.AutoSize = true;
            this.radioButtonSssj.Font = new System.Drawing.Font("宋体", 12F);
            this.radioButtonSssj.Location = new System.Drawing.Point(13, 27);
            this.radioButtonSssj.Name = "radioButtonSssj";
            this.radioButtonSssj.Size = new System.Drawing.Size(90, 20);
            this.radioButtonSssj.TabIndex = 0;
            this.radioButtonSssj.TabStop = true;
            this.radioButtonSssj.Text = "实时数据";
            this.radioButtonSssj.UseVisualStyleBackColor = true;
            // 
            // textEditBiaoO2
            // 
            this.textEditBiaoO2.BackColor = System.Drawing.Color.Black;
            this.textEditBiaoO2.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold);
            this.textEditBiaoO2.ForeColor = System.Drawing.Color.Aqua;
            this.textEditBiaoO2.Location = new System.Drawing.Point(120, 44);
            this.textEditBiaoO2.Name = "textEditBiaoO2";
            this.textEditBiaoO2.Size = new System.Drawing.Size(100, 35);
            this.textEditBiaoO2.TabIndex = 47;
            this.textEditBiaoO2.Text = "20.9";
            this.textEditBiaoO2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(235, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 34);
            this.button1.TabIndex = 1;
            this.button1.Text = "氧气校准";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(117, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 16);
            this.label2.TabIndex = 52;
            this.label2.Text = "氧气标准浓度(%)";
            // 
            // textBoxbzO2
            // 
            this.textBoxbzO2.BackColor = System.Drawing.Color.Black;
            this.textBoxbzO2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.textBoxbzO2.ForeColor = System.Drawing.Color.Aqua;
            this.textBoxbzO2.Location = new System.Drawing.Point(131, 46);
            this.textBoxbzO2.Name = "textBoxbzO2";
            this.textBoxbzO2.Size = new System.Drawing.Size(100, 30);
            this.textBoxbzO2.TabIndex = 51;
            this.textBoxbzO2.Text = "20.9";
            this.textBoxbzO2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.radioButtonLLsingle);
            this.groupBox7.Controls.Add(this.textBoxBiaoLL);
            this.groupBox7.Controls.Add(this.radioButtonLowLL);
            this.groupBox7.Controls.Add(this.buttonLLdemarcate);
            this.groupBox7.Controls.Add(this.label3);
            this.groupBox7.Controls.Add(this.radioButtonHighLL);
            this.groupBox7.Location = new System.Drawing.Point(468, 164);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(328, 95);
            this.groupBox7.TabIndex = 51;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "流量标定";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.dataGridView1);
            this.groupBox8.Location = new System.Drawing.Point(2, 356);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(680, 124);
            this.groupBox8.TabIndex = 52;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "数据";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.项目,
            this.氧气标准值,
            this.氧气实测值,
            this.氧气误差,
            this.标定结果,
            this.备注说明});
            this.dataGridView1.Location = new System.Drawing.Point(6, 20);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(668, 98);
            this.dataGridView1.TabIndex = 0;
            // 
            // 项目
            // 
            this.项目.HeaderText = "项目";
            this.项目.Name = "项目";
            // 
            // 氧气标准值
            // 
            this.氧气标准值.HeaderText = "氧气标准值";
            this.氧气标准值.Name = "氧气标准值";
            // 
            // 氧气实测值
            // 
            this.氧气实测值.HeaderText = "氧气实测值";
            this.氧气实测值.Name = "氧气实测值";
            // 
            // 氧气误差
            // 
            this.氧气误差.HeaderText = "氧气误差";
            this.氧气误差.Name = "氧气误差";
            // 
            // 标定结果
            // 
            this.标定结果.HeaderText = "标定结果";
            this.标定结果.Name = "标定结果";
            // 
            // 备注说明
            // 
            this.备注说明.HeaderText = "备注说明";
            this.备注说明.Name = "备注说明";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(688, 403);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 34);
            this.button2.TabIndex = 53;
            this.button2.Text = "保存数据";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(688, 439);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 34);
            this.button3.TabIndex = 54;
            this.button3.Text = "退出";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.textBoxbzO2);
            this.groupBox9.Controls.Add(this.label2);
            this.groupBox9.Controls.Add(this.buttonSavedata);
            this.groupBox9.Controls.Add(this.radioButtonLow);
            this.groupBox9.Controls.Add(this.radioButtonHigh);
            this.groupBox9.Location = new System.Drawing.Point(8, 265);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(378, 85);
            this.groupBox9.TabIndex = 55;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "检查项目";
            // 
            // buttonSavedata
            // 
            this.buttonSavedata.Location = new System.Drawing.Point(260, 29);
            this.buttonSavedata.Name = "buttonSavedata";
            this.buttonSavedata.Size = new System.Drawing.Size(100, 34);
            this.buttonSavedata.TabIndex = 49;
            this.buttonSavedata.Text = "保存";
            this.buttonSavedata.UseVisualStyleBackColor = true;
            this.buttonSavedata.Click += new System.EventHandler(this.buttonSavedata_Click);
            // 
            // radioButtonLow
            // 
            this.radioButtonLow.AutoSize = true;
            this.radioButtonLow.Font = new System.Drawing.Font("宋体", 12F);
            this.radioButtonLow.Location = new System.Drawing.Point(20, 51);
            this.radioButtonLow.Name = "radioButtonLow";
            this.radioButtonLow.Size = new System.Drawing.Size(74, 20);
            this.radioButtonLow.TabIndex = 1;
            this.radioButtonLow.TabStop = true;
            this.radioButtonLow.Text = "低量程";
            this.radioButtonLow.UseVisualStyleBackColor = true;
            this.radioButtonLow.CheckedChanged += new System.EventHandler(this.radioButtonLow_CheckedChanged);
            // 
            // radioButtonHigh
            // 
            this.radioButtonHigh.AutoSize = true;
            this.radioButtonHigh.Font = new System.Drawing.Font("宋体", 12F);
            this.radioButtonHigh.Location = new System.Drawing.Point(20, 23);
            this.radioButtonHigh.Name = "radioButtonHigh";
            this.radioButtonHigh.Size = new System.Drawing.Size(74, 20);
            this.radioButtonHigh.TabIndex = 0;
            this.radioButtonHigh.TabStop = true;
            this.radioButtonHigh.Text = "高量程";
            this.radioButtonHigh.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.buttonO2zero);
            this.groupBox6.Controls.Add(this.textEditBiaoO2);
            this.groupBox6.Controls.Add(this.radioButtonLowO2);
            this.groupBox6.Controls.Add(this.button1);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.radioButtonMidO2);
            this.groupBox6.Controls.Add(this.radioButtonHighO2);
            this.groupBox6.Location = new System.Drawing.Point(120, 164);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(342, 95);
            this.groupBox6.TabIndex = 56;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "氧气标定";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.Location = new System.Drawing.Point(106, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 16);
            this.label1.TabIndex = 52;
            this.label1.Text = "氧气标准浓度(%)";
            // 
            // radioButtonMidO2
            // 
            this.radioButtonMidO2.AutoSize = true;
            this.radioButtonMidO2.Font = new System.Drawing.Font("宋体", 12F);
            this.radioButtonMidO2.Location = new System.Drawing.Point(9, 42);
            this.radioButtonMidO2.Name = "radioButtonMidO2";
            this.radioButtonMidO2.Size = new System.Drawing.Size(90, 20);
            this.radioButtonMidO2.TabIndex = 1;
            this.radioButtonMidO2.TabStop = true;
            this.radioButtonMidO2.Text = "中点标定";
            this.radioButtonMidO2.UseVisualStyleBackColor = true;
            // 
            // radioButtonHighO2
            // 
            this.radioButtonHighO2.AutoSize = true;
            this.radioButtonHighO2.Font = new System.Drawing.Font("宋体", 12F);
            this.radioButtonHighO2.Location = new System.Drawing.Point(9, 18);
            this.radioButtonHighO2.Name = "radioButtonHighO2";
            this.radioButtonHighO2.Size = new System.Drawing.Size(90, 20);
            this.radioButtonHighO2.TabIndex = 0;
            this.radioButtonHighO2.TabStop = true;
            this.radioButtonHighO2.Text = "高点标定";
            this.radioButtonHighO2.UseVisualStyleBackColor = true;
            // 
            // radioButtonLowO2
            // 
            this.radioButtonLowO2.AutoSize = true;
            this.radioButtonLowO2.Font = new System.Drawing.Font("宋体", 12F);
            this.radioButtonLowO2.Location = new System.Drawing.Point(9, 66);
            this.radioButtonLowO2.Name = "radioButtonLowO2";
            this.radioButtonLowO2.Size = new System.Drawing.Size(90, 20);
            this.radioButtonLowO2.TabIndex = 53;
            this.radioButtonLowO2.TabStop = true;
            this.radioButtonLowO2.Text = "低点标定";
            this.radioButtonLowO2.UseVisualStyleBackColor = true;
            // 
            // buttonO2zero
            // 
            this.buttonO2zero.Location = new System.Drawing.Point(235, 52);
            this.buttonO2zero.Name = "buttonO2zero";
            this.buttonO2zero.Size = new System.Drawing.Size(100, 34);
            this.buttonO2zero.TabIndex = 54;
            this.buttonO2zero.Text = "调零";
            this.buttonO2zero.UseVisualStyleBackColor = true;
            this.buttonO2zero.Click += new System.EventHandler(this.buttonO2zero_Click);
            // 
            // textBoxBiaoLL
            // 
            this.textBoxBiaoLL.BackColor = System.Drawing.Color.Black;
            this.textBoxBiaoLL.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold);
            this.textBoxBiaoLL.ForeColor = System.Drawing.Color.Aqua;
            this.textBoxBiaoLL.Location = new System.Drawing.Point(114, 42);
            this.textBoxBiaoLL.Name = "textBoxBiaoLL";
            this.textBoxBiaoLL.Size = new System.Drawing.Size(100, 35);
            this.textBoxBiaoLL.TabIndex = 58;
            this.textBoxBiaoLL.Text = "95.0";
            this.textBoxBiaoLL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // radioButtonLowLL
            // 
            this.radioButtonLowLL.AutoSize = true;
            this.radioButtonLowLL.Font = new System.Drawing.Font("宋体", 12F);
            this.radioButtonLowLL.Location = new System.Drawing.Point(12, 41);
            this.radioButtonLowLL.Name = "radioButtonLowLL";
            this.radioButtonLowLL.Size = new System.Drawing.Size(90, 20);
            this.radioButtonLowLL.TabIndex = 60;
            this.radioButtonLowLL.TabStop = true;
            this.radioButtonLowLL.Text = "低点标定";
            this.radioButtonLowLL.UseVisualStyleBackColor = true;
            // 
            // buttonLLdemarcate
            // 
            this.buttonLLdemarcate.Location = new System.Drawing.Point(238, 11);
            this.buttonLLdemarcate.Name = "buttonLLdemarcate";
            this.buttonLLdemarcate.Size = new System.Drawing.Size(82, 34);
            this.buttonLLdemarcate.TabIndex = 56;
            this.buttonLLdemarcate.Text = "标定";
            this.buttonLLdemarcate.UseVisualStyleBackColor = true;
            this.buttonLLdemarcate.Click += new System.EventHandler(this.buttonLLdemarcate_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F);
            this.label3.Location = new System.Drawing.Point(109, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 16);
            this.label3.TabIndex = 59;
            this.label3.Text = "标准流量(L/s)";
            // 
            // radioButtonHighLL
            // 
            this.radioButtonHighLL.AutoSize = true;
            this.radioButtonHighLL.Font = new System.Drawing.Font("宋体", 12F);
            this.radioButtonHighLL.Location = new System.Drawing.Point(12, 16);
            this.radioButtonHighLL.Name = "radioButtonHighLL";
            this.radioButtonHighLL.Size = new System.Drawing.Size(90, 20);
            this.radioButtonHighLL.TabIndex = 55;
            this.radioButtonHighLL.TabStop = true;
            this.radioButtonHighLL.Text = "高点标定";
            this.radioButtonHighLL.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.buttonTempdemarcate);
            this.groupBox10.Controls.Add(this.textBoxTemp);
            this.groupBox10.Controls.Add(this.label5);
            this.groupBox10.Controls.Add(this.textBoxYL);
            this.groupBox10.Controls.Add(this.label4);
            this.groupBox10.Controls.Add(this.buttonYldemarcate);
            this.groupBox10.Location = new System.Drawing.Point(392, 265);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(290, 85);
            this.groupBox10.TabIndex = 57;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "环境参数标定";
            // 
            // textBoxYL
            // 
            this.textBoxYL.BackColor = System.Drawing.Color.Black;
            this.textBoxYL.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.textBoxYL.ForeColor = System.Drawing.Color.Aqua;
            this.textBoxYL.Location = new System.Drawing.Point(131, 17);
            this.textBoxYL.Name = "textBoxYL";
            this.textBoxYL.Size = new System.Drawing.Size(100, 30);
            this.textBoxYL.TabIndex = 51;
            this.textBoxYL.Text = "100.0";
            this.textBoxYL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F);
            this.label4.Location = new System.Drawing.Point(13, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 16);
            this.label4.TabIndex = 52;
            this.label4.Text = "气体压力(kPa)";
            // 
            // buttonYldemarcate
            // 
            this.buttonYldemarcate.Location = new System.Drawing.Point(237, 13);
            this.buttonYldemarcate.Name = "buttonYldemarcate";
            this.buttonYldemarcate.Size = new System.Drawing.Size(47, 34);
            this.buttonYldemarcate.TabIndex = 49;
            this.buttonYldemarcate.Text = "标定";
            this.buttonYldemarcate.UseVisualStyleBackColor = true;
            this.buttonYldemarcate.Click += new System.EventHandler(this.buttonYldemarcate_Click);
            // 
            // textBoxTemp
            // 
            this.textBoxTemp.BackColor = System.Drawing.Color.Black;
            this.textBoxTemp.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.textBoxTemp.ForeColor = System.Drawing.Color.Aqua;
            this.textBoxTemp.Location = new System.Drawing.Point(131, 49);
            this.textBoxTemp.Name = "textBoxTemp";
            this.textBoxTemp.Size = new System.Drawing.Size(100, 30);
            this.textBoxTemp.TabIndex = 53;
            this.textBoxTemp.Text = "20.0";
            this.textBoxTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F);
            this.label5.Location = new System.Drawing.Point(13, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 16);
            this.label5.TabIndex = 54;
            this.label5.Text = "温度(℃)";
            // 
            // buttonTempdemarcate
            // 
            this.buttonTempdemarcate.Location = new System.Drawing.Point(237, 46);
            this.buttonTempdemarcate.Name = "buttonTempdemarcate";
            this.buttonTempdemarcate.Size = new System.Drawing.Size(47, 34);
            this.buttonTempdemarcate.TabIndex = 55;
            this.buttonTempdemarcate.Text = "标定";
            this.buttonTempdemarcate.UseVisualStyleBackColor = true;
            this.buttonTempdemarcate.Click += new System.EventHandler(this.buttonTempdemarcate_Click);
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.buttonTurnOffMotor);
            this.groupBox11.Controls.Add(this.buttonTurnOnMotor);
            this.groupBox11.Location = new System.Drawing.Point(688, 265);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(110, 80);
            this.groupBox11.TabIndex = 58;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "电机控制";
            // 
            // buttonTurnOffMotor
            // 
            this.buttonTurnOffMotor.Location = new System.Drawing.Point(18, 49);
            this.buttonTurnOffMotor.Name = "buttonTurnOffMotor";
            this.buttonTurnOffMotor.Size = new System.Drawing.Size(69, 27);
            this.buttonTurnOffMotor.TabIndex = 57;
            this.buttonTurnOffMotor.Text = "停止电机";
            this.buttonTurnOffMotor.UseVisualStyleBackColor = true;
            this.buttonTurnOffMotor.Click += new System.EventHandler(this.buttonTurnOffMotor_Click);
            // 
            // buttonTurnOnMotor
            // 
            this.buttonTurnOnMotor.Location = new System.Drawing.Point(17, 17);
            this.buttonTurnOnMotor.Name = "buttonTurnOnMotor";
            this.buttonTurnOnMotor.Size = new System.Drawing.Size(70, 27);
            this.buttonTurnOnMotor.TabIndex = 56;
            this.buttonTurnOnMotor.Text = "开启电机";
            this.buttonTurnOnMotor.UseVisualStyleBackColor = true;
            this.buttonTurnOnMotor.Click += new System.EventHandler(this.buttonTurnOnMotor_Click);
            // 
            // radioButtonLLsingle
            // 
            this.radioButtonLLsingle.AutoSize = true;
            this.radioButtonLLsingle.Font = new System.Drawing.Font("宋体", 12F);
            this.radioButtonLLsingle.Location = new System.Drawing.Point(12, 66);
            this.radioButtonLLsingle.Name = "radioButtonLLsingle";
            this.radioButtonLLsingle.Size = new System.Drawing.Size(90, 20);
            this.radioButtonLLsingle.TabIndex = 61;
            this.radioButtonLLsingle.TabStop = true;
            this.radioButtonLLsingle.Text = "单点标定";
            this.radioButtonLLsingle.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(688, 366);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 34);
            this.button4.TabIndex = 59;
            this.button4.Text = "恢复出厂设置";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 486);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.groupBox11);
            this.Controls.Add(this.groupBox10);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel_msg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "流量计标定";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel_msg.ResumeLayout(false);
            this.panel_msg.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_msg;
        private System.Windows.Forms.Label label_msg;
        private System.Windows.Forms.TextBox textEditll;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxO2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxQtyl;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBoxQtwd;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton radioButtonBzsj;
        private System.Windows.Forms.RadioButton radioButtonSssj;
        private System.Windows.Forms.TextBox textEditBiaoO2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxbzO2;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.RadioButton radioButtonLow;
        private System.Windows.Forms.RadioButton radioButtonHigh;
        private System.Windows.Forms.DataGridViewTextBoxColumn 项目;
        private System.Windows.Forms.DataGridViewTextBoxColumn 氧气标准值;
        private System.Windows.Forms.DataGridViewTextBoxColumn 氧气实测值;
        private System.Windows.Forms.DataGridViewTextBoxColumn 氧气误差;
        private System.Windows.Forms.DataGridViewTextBoxColumn 标定结果;
        private System.Windows.Forms.DataGridViewTextBoxColumn 备注说明;
        private System.Windows.Forms.Button buttonSavedata;
        private System.Windows.Forms.TextBox textBoxBiaoLL;
        private System.Windows.Forms.RadioButton radioButtonLowLL;
        private System.Windows.Forms.Button buttonLLdemarcate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButtonHighLL;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button buttonO2zero;
        private System.Windows.Forms.RadioButton radioButtonLowO2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButtonMidO2;
        private System.Windows.Forms.RadioButton radioButtonHighO2;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.TextBox textBoxTemp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxYL;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonYldemarcate;
        private System.Windows.Forms.Button buttonTempdemarcate;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Button buttonTurnOffMotor;
        private System.Windows.Forms.Button buttonTurnOnMotor;
        private System.Windows.Forms.RadioButton radioButtonLLsingle;
        private System.Windows.Forms.Button button4;
    }
}

