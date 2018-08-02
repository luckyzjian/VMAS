namespace 烟度计标定
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxbzNs = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSavedata = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.textBoxK = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel_msg = new System.Windows.Forms.Panel();
            this.label_msg = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBoxN = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textEditNs = new System.Windows.Forms.TextBox();
            this.radioButtonNs = new System.Windows.Forms.RadioButton();
            this.radioButtonN = new System.Windows.Forms.RadioButton();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel_msg.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.radioButtonNs);
            this.groupBox9.Controls.Add(this.radioButtonN);
            this.groupBox9.Controls.Add(this.button1);
            this.groupBox9.Controls.Add(this.textBoxbzNs);
            this.groupBox9.Controls.Add(this.label2);
            this.groupBox9.Controls.Add(this.buttonSavedata);
            this.groupBox9.Location = new System.Drawing.Point(12, 181);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(791, 86);
            this.groupBox9.TabIndex = 65;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "检查项目";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(167, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 34);
            this.button1.TabIndex = 53;
            this.button1.Text = "线性校正";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxbzNs
            // 
            this.textBoxbzNs.BackColor = System.Drawing.Color.Black;
            this.textBoxbzNs.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold);
            this.textBoxbzNs.ForeColor = System.Drawing.Color.Aqua;
            this.textBoxbzNs.Location = new System.Drawing.Point(327, 43);
            this.textBoxbzNs.Name = "textBoxbzNs";
            this.textBoxbzNs.Size = new System.Drawing.Size(100, 30);
            this.textBoxbzNs.TabIndex = 51;
            this.textBoxbzNs.Text = "45.5";
            this.textBoxbzNs.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(308, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 16);
            this.label2.TabIndex = 52;
            this.label2.Text = "滤光片不透光度(%)";
            // 
            // buttonSavedata
            // 
            this.buttonSavedata.Location = new System.Drawing.Point(496, 36);
            this.buttonSavedata.Name = "buttonSavedata";
            this.buttonSavedata.Size = new System.Drawing.Size(100, 34);
            this.buttonSavedata.TabIndex = 49;
            this.buttonSavedata.Text = "保存";
            this.buttonSavedata.UseVisualStyleBackColor = true;
            this.buttonSavedata.Click += new System.EventHandler(this.buttonSavedata_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(695, 355);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 34);
            this.button3.TabIndex = 64;
            this.button3.Text = "退出";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(695, 319);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 34);
            this.button2.TabIndex = 63;
            this.button2.Text = "保存数据";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.dataGridView1);
            this.groupBox8.Location = new System.Drawing.Point(9, 273);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(680, 124);
            this.groupBox8.TabIndex = 62;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "数据";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.Location = new System.Drawing.Point(6, 20);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(668, 98);
            this.dataGridView1.TabIndex = 0;
            // 
            // textBoxK
            // 
            this.textBoxK.BackColor = System.Drawing.Color.Black;
            this.textBoxK.Font = new System.Drawing.Font("宋体", 40F, System.Drawing.FontStyle.Bold);
            this.textBoxK.ForeColor = System.Drawing.Color.Lime;
            this.textBoxK.Location = new System.Drawing.Point(6, 24);
            this.textBoxK.Name = "textBoxK";
            this.textBoxK.ReadOnly = true;
            this.textBoxK.Size = new System.Drawing.Size(182, 68);
            this.textBoxK.TabIndex = 42;
            this.textBoxK.TabStop = false;
            this.textBoxK.Text = "00.0";
            this.textBoxK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxK);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 15F);
            this.groupBox1.Location = new System.Drawing.Point(12, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(194, 103);
            this.groupBox1.TabIndex = 57;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "光吸收系数(1/m)";
            // 
            // panel_msg
            // 
            this.panel_msg.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel_msg.BackColor = System.Drawing.SystemColors.Desktop;
            this.panel_msg.Controls.Add(this.label_msg);
            this.panel_msg.Location = new System.Drawing.Point(8, 10);
            this.panel_msg.Name = "panel_msg";
            this.panel_msg.Size = new System.Drawing.Size(795, 45);
            this.panel_msg.TabIndex = 56;
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
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 15F);
            this.groupBox3.Location = new System.Drawing.Point(601, 70);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(194, 103);
            this.groupBox3.TabIndex = 66;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "转速(r/min)";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Black;
            this.textBox1.Font = new System.Drawing.Font("宋体", 40F, System.Drawing.FontStyle.Bold);
            this.textBox1.ForeColor = System.Drawing.Color.Lime;
            this.textBox1.Location = new System.Drawing.Point(6, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(182, 68);
            this.textBox1.TabIndex = 42;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "00.0";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(695, 283);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 34);
            this.button4.TabIndex = 67;
            this.button4.Text = "删除";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBoxN);
            this.groupBox4.Font = new System.Drawing.Font("宋体", 15F);
            this.groupBox4.Location = new System.Drawing.Point(209, 70);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(194, 103);
            this.groupBox4.TabIndex = 70;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "N(%)";
            // 
            // textBoxN
            // 
            this.textBoxN.BackColor = System.Drawing.Color.Black;
            this.textBoxN.Font = new System.Drawing.Font("宋体", 40F, System.Drawing.FontStyle.Bold);
            this.textBoxN.ForeColor = System.Drawing.Color.Lime;
            this.textBoxN.Location = new System.Drawing.Point(6, 24);
            this.textBoxN.Name = "textBoxN";
            this.textBoxN.ReadOnly = true;
            this.textBoxN.Size = new System.Drawing.Size(182, 68);
            this.textBoxN.TabIndex = 42;
            this.textBoxN.TabStop = false;
            this.textBoxN.Text = "00.0";
            this.textBoxN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textEditNs);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 15F);
            this.groupBox2.Location = new System.Drawing.Point(405, 70);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(194, 103);
            this.groupBox2.TabIndex = 69;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ns(%)";
            // 
            // textEditNs
            // 
            this.textEditNs.BackColor = System.Drawing.Color.Black;
            this.textEditNs.Font = new System.Drawing.Font("宋体", 40F, System.Drawing.FontStyle.Bold);
            this.textEditNs.ForeColor = System.Drawing.Color.Lime;
            this.textEditNs.Location = new System.Drawing.Point(6, 24);
            this.textEditNs.Name = "textEditNs";
            this.textEditNs.ReadOnly = true;
            this.textEditNs.Size = new System.Drawing.Size(182, 68);
            this.textEditNs.TabIndex = 42;
            this.textEditNs.TabStop = false;
            this.textEditNs.Text = "00.0";
            this.textEditNs.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // radioButtonNs
            // 
            this.radioButtonNs.AutoSize = true;
            this.radioButtonNs.Location = new System.Drawing.Point(23, 52);
            this.radioButtonNs.Name = "radioButtonNs";
            this.radioButtonNs.Size = new System.Drawing.Size(35, 16);
            this.radioButtonNs.TabIndex = 57;
            this.radioButtonNs.TabStop = true;
            this.radioButtonNs.Text = "Ns";
            this.radioButtonNs.UseVisualStyleBackColor = true;
            // 
            // radioButtonN
            // 
            this.radioButtonN.AutoSize = true;
            this.radioButtonN.Location = new System.Drawing.Point(23, 30);
            this.radioButtonN.Name = "radioButtonN";
            this.radioButtonN.Size = new System.Drawing.Size(29, 16);
            this.radioButtonN.TabIndex = 56;
            this.radioButtonN.TabStop = true;
            this.radioButtonN.Text = "N";
            this.radioButtonN.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(811, 400);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel_msg);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "烟度计标定";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel_msg.ResumeLayout(false);
            this.panel_msg.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.TextBox textBoxbzNs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonSavedata;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBoxK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel_msg;
        private System.Windows.Forms.Label label_msg;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBoxN;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textEditNs;
        private System.Windows.Forms.RadioButton radioButtonNs;
        private System.Windows.Forms.RadioButton radioButtonN;
    }
}

