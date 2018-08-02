namespace Detect
{
    partial class YJ
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
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.checkBox_sf = new System.Windows.Forms.CheckBox();
            this.checkBox_aq = new System.Windows.Forms.CheckBox();
            this.checkBox_sq = new System.Windows.Forms.CheckBox();
            this.checkBox_xl = new System.Windows.Forms.CheckBox();
            this.panel_cp = new System.Windows.Forms.Panel();
            this.label_cp = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel_cp.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(267, 326);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 0;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(360, 326);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // checkBox_sf
            // 
            this.checkBox_sf.AutoSize = true;
            this.checkBox_sf.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_sf.Location = new System.Drawing.Point(267, 84);
            this.checkBox_sf.Name = "checkBox_sf";
            this.checkBox_sf.Size = new System.Drawing.Size(115, 16);
            this.checkBox_sf.TabIndex = 2;
            this.checkBox_sf.Text = "车辆身份不一致";
            this.checkBox_sf.UseVisualStyleBackColor = true;
            // 
            // checkBox_aq
            // 
            this.checkBox_aq.AutoSize = true;
            this.checkBox_aq.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_aq.Location = new System.Drawing.Point(267, 106);
            this.checkBox_aq.Name = "checkBox_aq";
            this.checkBox_aq.Size = new System.Drawing.Size(245, 16);
            this.checkBox_aq.TabIndex = 2;
            this.checkBox_aq.Text = "车辆存在机械故障可能会影响本次检测";
            this.checkBox_aq.UseVisualStyleBackColor = true;
            // 
            // checkBox_sq
            // 
            this.checkBox_sq.AutoSize = true;
            this.checkBox_sq.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_sq.Location = new System.Drawing.Point(267, 128);
            this.checkBox_sq.Name = "checkBox_sq";
            this.checkBox_sq.Size = new System.Drawing.Size(161, 16);
            this.checkBox_sq.TabIndex = 3;
            this.checkBox_sq.Text = "车辆为全时4轮驱动车辆";
            this.checkBox_sq.UseVisualStyleBackColor = true;
            // 
            // checkBox_xl
            // 
            this.checkBox_xl.AutoSize = true;
            this.checkBox_xl.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_xl.Location = new System.Drawing.Point(267, 150);
            this.checkBox_xl.Name = "checkBox_xl";
            this.checkBox_xl.Size = new System.Drawing.Size(180, 16);
            this.checkBox_xl.TabIndex = 4;
            this.checkBox_xl.Text = "车辆排汽系统存在明显泄漏";
            this.checkBox_xl.UseVisualStyleBackColor = true;
            // 
            // panel_cp
            // 
            this.panel_cp.BackColor = System.Drawing.Color.Black;
            this.panel_cp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_cp.Controls.Add(this.label_cp);
            this.panel_cp.Location = new System.Drawing.Point(1, 11);
            this.panel_cp.Name = "panel_cp";
            this.panel_cp.Size = new System.Drawing.Size(686, 58);
            this.panel_cp.TabIndex = 5;
            // 
            // label_cp
            // 
            this.label_cp.AutoSize = true;
            this.label_cp.Font = new System.Drawing.Font("微软雅黑", 24F);
            this.label_cp.ForeColor = System.Drawing.Color.White;
            this.label_cp.Location = new System.Drawing.Point(104, 7);
            this.label_cp.Name = "label_cp";
            this.label_cp.Size = new System.Drawing.Size(466, 41);
            this.label_cp.TabIndex = 1;
            this.label_cp.Text = "请工作人员对车辆进行以下检查";
            this.label_cp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(3, 17);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(530, 125);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "1.检查底盘测功机周围环境，将可能妨碍检测的物体清除，检查轮胎是否需要干燥、清洁。\r\n\r\n2.如果是前驱动车辆，则应使用拉车带、塞块等装置将车辆固定并施加非驱动轮" +
                "驻车制动器，\r\n避免检测过程中车辆的意外移动。如果不是前驱动车辆，则将塞块置于非驱动轮下固定车辆。\r\n\r\n3.若为双排气管,用双取样探头分别插入两排气管。\r\n\r" +
                "\n4.检查发动机转速计是否己安装好。\r\n";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(73, 172);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(536, 145);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "工作人员操作指南";
            // 
            // YJ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 361);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel_cp);
            this.Controls.Add(this.checkBox_xl);
            this.Controls.Add(this.checkBox_sq);
            this.Controls.Add(this.checkBox_aq);
            this.Controls.Add(this.checkBox_sf);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "YJ";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "车辆检查";
            this.TopMost = true;
            this.panel_cp.ResumeLayout(false);
            this.panel_cp.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.CheckBox checkBox_sf;
        private System.Windows.Forms.CheckBox checkBox_aq;
        private System.Windows.Forms.CheckBox checkBox_sq;
        private System.Windows.Forms.CheckBox checkBox_xl;
        private System.Windows.Forms.Panel panel_cp;
        private System.Windows.Forms.Label label_cp;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}