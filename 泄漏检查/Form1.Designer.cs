namespace 泄漏检查
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panelMsg = new System.Windows.Forms.Panel();
            this.labelMsg = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.LabelFQYJL = new System.Windows.Forms.Label();
            this.panelMsg.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMsg
            // 
            this.panelMsg.BackColor = System.Drawing.Color.Black;
            this.panelMsg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelMsg.Controls.Add(this.labelMsg);
            this.panelMsg.Location = new System.Drawing.Point(2, 3);
            this.panelMsg.Name = "panelMsg";
            this.panelMsg.Size = new System.Drawing.Size(479, 50);
            this.panelMsg.TabIndex = 34;
            // 
            // labelMsg
            // 
            this.labelMsg.AutoSize = true;
            this.labelMsg.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Bold);
            this.labelMsg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.labelMsg.Location = new System.Drawing.Point(54, 4);
            this.labelMsg.Name = "labelMsg";
            this.labelMsg.Size = new System.Drawing.Size(378, 33);
            this.labelMsg.TabIndex = 0;
            this.labelMsg.Text = "堵住探头后点击开始检漏";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 10.5F);
            this.button1.Location = new System.Drawing.Point(178, 72);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 29);
            this.button1.TabIndex = 35;
            this.button1.Text = "开始检漏";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("宋体", 10.5F);
            this.button2.Location = new System.Drawing.Point(178, 147);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 29);
            this.button2.TabIndex = 36;
            this.button2.Text = "保存结果";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("宋体", 10.5F);
            this.button3.Location = new System.Drawing.Point(178, 182);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(112, 29);
            this.button3.TabIndex = 37;
            this.button3.Text = "退出";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // LabelFQYJL
            // 
            this.LabelFQYJL.AutoSize = true;
            this.LabelFQYJL.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold);
            this.LabelFQYJL.Location = new System.Drawing.Point(221, 115);
            this.LabelFQYJL.Name = "LabelFQYJL";
            this.LabelFQYJL.Size = new System.Drawing.Size(26, 16);
            this.LabelFQYJL.TabIndex = 38;
            this.LabelFQYJL.Text = "--";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 259);
            this.Controls.Add(this.LabelFQYJL);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panelMsg);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "泄漏检查";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.panelMsg.ResumeLayout(false);
            this.panelMsg.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelMsg;
        private System.Windows.Forms.Label labelMsg;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        internal System.Windows.Forms.Label LabelFQYJL;
    }
}

