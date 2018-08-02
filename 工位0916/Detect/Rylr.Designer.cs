namespace Detect
{
    partial class Rylr
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBox_jsy = new System.Windows.Forms.ComboBox();
            this.comboBox_czy = new System.Windows.Forms.ComboBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_qxdq = new System.Windows.Forms.Button();
            this.textBox_dqy = new System.Windows.Forms.TextBox();
            this.textBox_sd = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_wd = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox_jsy
            // 
            this.comboBox_jsy.Font = new System.Drawing.Font("宋体", 12F);
            this.comboBox_jsy.FormattingEnabled = true;
            this.comboBox_jsy.Location = new System.Drawing.Point(85, 50);
            this.comboBox_jsy.Name = "comboBox_jsy";
            this.comboBox_jsy.Size = new System.Drawing.Size(107, 24);
            this.comboBox_jsy.TabIndex = 0;
            this.comboBox_jsy.Tag = "引车员";
            // 
            // comboBox_czy
            // 
            this.comboBox_czy.Font = new System.Drawing.Font("宋体", 12F);
            this.comboBox_czy.FormattingEnabled = true;
            this.comboBox_czy.Location = new System.Drawing.Point(85, 80);
            this.comboBox_czy.Name = "comboBox_czy";
            this.comboBox_czy.Size = new System.Drawing.Size(107, 24);
            this.comboBox_czy.TabIndex = 0;
            this.comboBox_czy.Tag = "操作员";
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(328, 156);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 23);
            this.btn_ok.TabIndex = 1;
            this.btn_ok.Text = "确定";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.Location = new System.Drawing.Point(7, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "引车员：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(7, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "操作员:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(47, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "请选择检测相关人员";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBox_jsy);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBox_czy);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(211, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 115);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "人员";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_qxdq);
            this.groupBox2.Controls.Add(this.textBox_dqy);
            this.groupBox2.Controls.Add(this.textBox_sd);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.textBox_wd);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(193, 174);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "气象";
            // 
            // btn_qxdq
            // 
            this.btn_qxdq.Location = new System.Drawing.Point(112, 144);
            this.btn_qxdq.Name = "btn_qxdq";
            this.btn_qxdq.Size = new System.Drawing.Size(75, 23);
            this.btn_qxdq.TabIndex = 6;
            this.btn_qxdq.Text = "读取";
            this.btn_qxdq.UseVisualStyleBackColor = true;
            this.btn_qxdq.Click += new System.EventHandler(this.btn_qxdq_Click);
            // 
            // textBox_dqy
            // 
            this.textBox_dqy.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox_dqy.Location = new System.Drawing.Point(84, 112);
            this.textBox_dqy.Name = "textBox_dqy";
            this.textBox_dqy.Size = new System.Drawing.Size(67, 26);
            this.textBox_dqy.TabIndex = 4;
            this.textBox_dqy.Tag = "大气压";
            this.textBox_dqy.TextChanged += new System.EventHandler(this.textBox_dqy_TextChanged);
            // 
            // textBox_sd
            // 
            this.textBox_sd.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox_sd.Location = new System.Drawing.Point(84, 80);
            this.textBox_sd.Name = "textBox_sd";
            this.textBox_sd.Size = new System.Drawing.Size(67, 26);
            this.textBox_sd.TabIndex = 4;
            this.textBox_sd.Tag = "湿度";
            this.textBox_sd.TextChanged += new System.EventHandler(this.textBox_sd_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 12F);
            this.label7.Location = new System.Drawing.Point(6, 115);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 16);
            this.label7.TabIndex = 2;
            this.label7.Text = "大气压：";
            // 
            // textBox_wd
            // 
            this.textBox_wd.Font = new System.Drawing.Font("宋体", 12F);
            this.textBox_wd.Location = new System.Drawing.Point(84, 49);
            this.textBox_wd.Name = "textBox_wd";
            this.textBox_wd.Size = new System.Drawing.Size(67, 26);
            this.textBox_wd.TabIndex = 4;
            this.textBox_wd.Tag = "温度";
            this.textBox_wd.TextChanged += new System.EventHandler(this.textBox_wd_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F);
            this.label6.Location = new System.Drawing.Point(22, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 16);
            this.label6.TabIndex = 2;
            this.label6.Text = "湿度：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(52, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "请录入气象数据";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 12F);
            this.label10.Location = new System.Drawing.Point(157, 115);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 16);
            this.label10.TabIndex = 2;
            this.label10.Text = "Kpa";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 12F);
            this.label9.Location = new System.Drawing.Point(157, 83);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(16, 16);
            this.label9.TabIndex = 2;
            this.label9.Text = "%";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 12F);
            this.label8.Location = new System.Drawing.Point(157, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 16);
            this.label8.TabIndex = 2;
            this.label8.Text = "℃";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F);
            this.label5.Location = new System.Drawing.Point(22, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 16);
            this.label5.TabIndex = 2;
            this.label5.Text = "温度：";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // Rylr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ClientSize = new System.Drawing.Size(421, 194);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Rylr";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rylr";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Rylr_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_jsy;
        private System.Windows.Forms.ComboBox comboBox_czy;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_dqy;
        private System.Windows.Forms.TextBox textBox_sd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_wd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_qxdq;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}