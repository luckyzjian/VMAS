namespace zyjsTest
{
    partial class settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(settings));
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.buttonBtgSave = new System.Windows.Forms.Button();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.comboBoxBtgZsjCom = new System.Windows.Forms.ComboBox();
            this.label40 = new System.Windows.Forms.Label();
            this.comboBoxBtgZsj = new System.Windows.Forms.ComboBox();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.checkBoxBtgZsjk = new System.Windows.Forms.CheckBox();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.label45 = new System.Windows.Forms.Label();
            this.textBoxBtgDyzs = new System.Windows.Forms.TextBox();
            this.label50 = new System.Windows.Forms.Label();
            this.groupBox15.SuspendLayout();
            this.groupBox16.SuspendLayout();
            this.groupBox17.SuspendLayout();
            this.groupBox18.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.buttonBtgSave);
            this.groupBox15.Controls.Add(this.groupBox16);
            this.groupBox15.Controls.Add(this.groupBox17);
            this.groupBox15.Controls.Add(this.groupBox18);
            this.groupBox15.Location = new System.Drawing.Point(12, 12);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(263, 218);
            this.groupBox15.TabIndex = 64;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "自由加速不透光法";
            // 
            // buttonBtgSave
            // 
            this.buttonBtgSave.Font = new System.Drawing.Font("宋体", 9F);
            this.buttonBtgSave.Location = new System.Drawing.Point(89, 185);
            this.buttonBtgSave.Name = "buttonBtgSave";
            this.buttonBtgSave.Size = new System.Drawing.Size(86, 27);
            this.buttonBtgSave.TabIndex = 59;
            this.buttonBtgSave.Text = "保存";
            this.buttonBtgSave.UseVisualStyleBackColor = true;
            this.buttonBtgSave.Click += new System.EventHandler(this.buttonBtgSave_Click);
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.comboBoxBtgZsjCom);
            this.groupBox16.Controls.Add(this.label40);
            this.groupBox16.Controls.Add(this.comboBoxBtgZsj);
            this.groupBox16.Location = new System.Drawing.Point(15, 132);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(227, 47);
            this.groupBox16.TabIndex = 58;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "转速计选择";
            // 
            // comboBoxBtgZsjCom
            // 
            this.comboBoxBtgZsjCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBtgZsjCom.Enabled = false;
            this.comboBoxBtgZsjCom.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxBtgZsjCom.ForeColor = System.Drawing.Color.Blue;
            this.comboBoxBtgZsjCom.FormattingEnabled = true;
            this.comboBoxBtgZsjCom.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6"});
            this.comboBoxBtgZsjCom.Location = new System.Drawing.Point(147, 16);
            this.comboBoxBtgZsjCom.Name = "comboBoxBtgZsjCom";
            this.comboBoxBtgZsjCom.Size = new System.Drawing.Size(74, 25);
            this.comboBoxBtgZsjCom.TabIndex = 40;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("宋体", 9F);
            this.label40.Location = new System.Drawing.Point(12, 25);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(41, 12);
            this.label40.TabIndex = 38;
            this.label40.Text = "转速计";
            // 
            // comboBoxBtgZsj
            // 
            this.comboBoxBtgZsj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBtgZsj.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxBtgZsj.ForeColor = System.Drawing.Color.Blue;
            this.comboBoxBtgZsj.FormattingEnabled = true;
            this.comboBoxBtgZsj.Items.AddRange(new object[] {
            "废气仪",
            "烟度计",
            "VMT-2000",
            "VUT-3000",
            "MQZ-2",
            "MQZ-3"});
            this.comboBoxBtgZsj.Location = new System.Drawing.Point(62, 17);
            this.comboBoxBtgZsj.Name = "comboBoxBtgZsj";
            this.comboBoxBtgZsj.Size = new System.Drawing.Size(79, 25);
            this.comboBoxBtgZsj.TabIndex = 37;
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.checkBoxBtgZsjk);
            this.groupBox17.Location = new System.Drawing.Point(15, 20);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(227, 50);
            this.groupBox17.TabIndex = 57;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "监控项目设置";
            // 
            // checkBoxBtgZsjk
            // 
            this.checkBoxBtgZsjk.AutoSize = true;
            this.checkBoxBtgZsjk.Font = new System.Drawing.Font("宋体", 9F);
            this.checkBoxBtgZsjk.Location = new System.Drawing.Point(53, 20);
            this.checkBoxBtgZsjk.Name = "checkBoxBtgZsjk";
            this.checkBoxBtgZsjk.Size = new System.Drawing.Size(72, 16);
            this.checkBoxBtgZsjk.TabIndex = 37;
            this.checkBoxBtgZsjk.Text = "转速监控";
            this.checkBoxBtgZsjk.UseVisualStyleBackColor = true;
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.label45);
            this.groupBox18.Controls.Add(this.textBoxBtgDyzs);
            this.groupBox18.Controls.Add(this.label50);
            this.groupBox18.Location = new System.Drawing.Point(15, 76);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Size = new System.Drawing.Size(227, 50);
            this.groupBox18.TabIndex = 55;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "参数设置";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Font = new System.Drawing.Font("宋体", 9F);
            this.label45.Location = new System.Drawing.Point(167, 22);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(35, 12);
            this.label45.TabIndex = 50;
            this.label45.Text = "r/min";
            // 
            // textBoxBtgDyzs
            // 
            this.textBoxBtgDyzs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBtgDyzs.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxBtgDyzs.ForeColor = System.Drawing.Color.Blue;
            this.textBoxBtgDyzs.Location = new System.Drawing.Point(104, 16);
            this.textBoxBtgDyzs.Name = "textBoxBtgDyzs";
            this.textBoxBtgDyzs.Size = new System.Drawing.Size(56, 23);
            this.textBoxBtgDyzs.TabIndex = 11;
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Font = new System.Drawing.Font("宋体", 9F);
            this.label50.Location = new System.Drawing.Point(6, 20);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(53, 12);
            this.label50.TabIndex = 12;
            this.label50.Text = "断油转速";
            // 
            // settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 351);
            this.Controls.Add(this.groupBox15);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统设置";
            this.Load += new System.EventHandler(this.settings_Load);
            this.groupBox15.ResumeLayout(false);
            this.groupBox16.ResumeLayout(false);
            this.groupBox16.PerformLayout();
            this.groupBox17.ResumeLayout(false);
            this.groupBox17.PerformLayout();
            this.groupBox18.ResumeLayout(false);
            this.groupBox18.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.Button buttonBtgSave;
        private System.Windows.Forms.GroupBox groupBox16;
        private System.Windows.Forms.ComboBox comboBoxBtgZsjCom;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.ComboBox comboBoxBtgZsj;
        private System.Windows.Forms.GroupBox groupBox17;
        private System.Windows.Forms.CheckBox checkBoxBtgZsjk;
        private System.Windows.Forms.GroupBox groupBox18;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.TextBox textBoxBtgDyzs;
        private System.Windows.Forms.Label label50;

    }
}