namespace dynSettings
{
    partial class dynSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dynSettings));
            this.label166 = new System.Windows.Forms.Label();
            this.textBoxDynNlxs = new System.Windows.Forms.TextBox();
            this.label144 = new System.Windows.Forms.Label();
            this.textBoxDynSdxs = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label166
            // 
            this.label166.AutoSize = true;
            this.label166.Font = new System.Drawing.Font("宋体", 10.5F);
            this.label166.Location = new System.Drawing.Point(16, 47);
            this.label166.Name = "label166";
            this.label166.Size = new System.Drawing.Size(77, 14);
            this.label166.TabIndex = 176;
            this.label166.Text = "加载力系数";
            // 
            // textBoxDynNlxs
            // 
            this.textBoxDynNlxs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxDynNlxs.Font = new System.Drawing.Font("宋体", 10.5F);
            this.textBoxDynNlxs.ForeColor = System.Drawing.Color.Blue;
            this.textBoxDynNlxs.Location = new System.Drawing.Point(112, 42);
            this.textBoxDynNlxs.Name = "textBoxDynNlxs";
            this.textBoxDynNlxs.Size = new System.Drawing.Size(88, 23);
            this.textBoxDynNlxs.TabIndex = 175;
            // 
            // label144
            // 
            this.label144.AutoSize = true;
            this.label144.Font = new System.Drawing.Font("宋体", 10.5F);
            this.label144.Location = new System.Drawing.Point(16, 17);
            this.label144.Name = "label144";
            this.label144.Size = new System.Drawing.Size(63, 14);
            this.label144.TabIndex = 174;
            this.label144.Text = "速度系数";
            // 
            // textBoxDynSdxs
            // 
            this.textBoxDynSdxs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxDynSdxs.Font = new System.Drawing.Font("宋体", 10.5F);
            this.textBoxDynSdxs.ForeColor = System.Drawing.Color.Blue;
            this.textBoxDynSdxs.Location = new System.Drawing.Point(112, 12);
            this.textBoxDynSdxs.Name = "textBoxDynSdxs";
            this.textBoxDynSdxs.Size = new System.Drawing.Size(88, 23);
            this.textBoxDynSdxs.TabIndex = 173;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 9F);
            this.button1.Location = new System.Drawing.Point(112, 104);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 32);
            this.button1.TabIndex = 177;
            this.button1.Text = "保存";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dynSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 148);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label166);
            this.Controls.Add(this.textBoxDynNlxs);
            this.Controls.Add(this.label144);
            this.Controls.Add(this.textBoxDynSdxs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "dynSettings";
            this.Text = "动力性及油耗检测系数设置";
            this.Load += new System.EventHandler(this.dynSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label166;
        private System.Windows.Forms.TextBox textBoxDynNlxs;
        private System.Windows.Forms.Label label144;
        private System.Windows.Forms.TextBox textBoxDynSdxs;
        private System.Windows.Forms.Button button1;
    }
}

