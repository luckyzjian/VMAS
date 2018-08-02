namespace lugdowm
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
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.label56 = new System.Windows.Forms.Label();
            this.radioButtonLugdownHgl = new System.Windows.Forms.RadioButton();
            this.radioButtonLugdownhs = new System.Windows.Forms.RadioButton();
            this.checkBoxLugdownSureTemp = new System.Windows.Forms.CheckBox();
            this.buttonLugdownSave = new System.Windows.Forms.Button();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.comboBoxLugdownZsjCom = new System.Windows.Forms.ComboBox();
            this.label44 = new System.Windows.Forms.Label();
            this.comboBoxLugdownZsj = new System.Windows.Forms.ComboBox();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.label47 = new System.Windows.Forms.Label();
            this.textBoxLugdownSmpl = new System.Windows.Forms.TextBox();
            this.label55 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.textBoxLugDownMinSpeed = new System.Windows.Forms.TextBox();
            this.label54 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.textBoxLugDownMaxSpeed = new System.Windows.Forms.TextBox();
            this.label52 = new System.Windows.Forms.Label();
            this.groupBox19.SuspendLayout();
            this.groupBox21.SuspendLayout();
            this.groupBox20.SuspendLayout();
            this.groupBox22.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.groupBox21);
            this.groupBox19.Controls.Add(this.buttonLugdownSave);
            this.groupBox19.Controls.Add(this.groupBox20);
            this.groupBox19.Controls.Add(this.groupBox22);
            this.groupBox19.Location = new System.Drawing.Point(12, 12);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Size = new System.Drawing.Size(263, 265);
            this.groupBox19.TabIndex = 65;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = "加载减速法";
            // 
            // groupBox21
            // 
            this.groupBox21.Controls.Add(this.label56);
            this.groupBox21.Controls.Add(this.radioButtonLugdownHgl);
            this.groupBox21.Controls.Add(this.radioButtonLugdownhs);
            this.groupBox21.Controls.Add(this.checkBoxLugdownSureTemp);
            this.groupBox21.Location = new System.Drawing.Point(15, 113);
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.Size = new System.Drawing.Size(227, 65);
            this.groupBox21.TabIndex = 61;
            this.groupBox21.TabStop = false;
            this.groupBox21.Text = "检测过程设置";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Font = new System.Drawing.Font("宋体", 9F);
            this.label56.Location = new System.Drawing.Point(6, 46);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(77, 12);
            this.label56.TabIndex = 63;
            this.label56.Text = "功率扫描模式";
            // 
            // radioButtonLugdownHgl
            // 
            this.radioButtonLugdownHgl.AutoSize = true;
            this.radioButtonLugdownHgl.Location = new System.Drawing.Point(152, 44);
            this.radioButtonLugdownHgl.Name = "radioButtonLugdownHgl";
            this.radioButtonLugdownHgl.Size = new System.Drawing.Size(59, 16);
            this.radioButtonLugdownHgl.TabIndex = 62;
            this.radioButtonLugdownHgl.TabStop = true;
            this.radioButtonLugdownHgl.Text = "恒功率";
            this.radioButtonLugdownHgl.UseVisualStyleBackColor = true;
            // 
            // radioButtonLugdownhs
            // 
            this.radioButtonLugdownhs.AutoSize = true;
            this.radioButtonLugdownhs.Location = new System.Drawing.Point(95, 44);
            this.radioButtonLugdownhs.Name = "radioButtonLugdownhs";
            this.radioButtonLugdownhs.Size = new System.Drawing.Size(47, 16);
            this.radioButtonLugdownhs.TabIndex = 61;
            this.radioButtonLugdownhs.TabStop = true;
            this.radioButtonLugdownhs.Text = "恒速";
            this.radioButtonLugdownhs.UseVisualStyleBackColor = true;
            // 
            // checkBoxLugdownSureTemp
            // 
            this.checkBoxLugdownSureTemp.AutoSize = true;
            this.checkBoxLugdownSureTemp.Font = new System.Drawing.Font("宋体", 9F);
            this.checkBoxLugdownSureTemp.Location = new System.Drawing.Point(53, 20);
            this.checkBoxLugdownSureTemp.Name = "checkBoxLugdownSureTemp";
            this.checkBoxLugdownSureTemp.Size = new System.Drawing.Size(108, 16);
            this.checkBoxLugdownSureTemp.TabIndex = 60;
            this.checkBoxLugdownSureTemp.Text = "是否确认温湿度";
            this.checkBoxLugdownSureTemp.UseVisualStyleBackColor = true;
            // 
            // buttonLugdownSave
            // 
            this.buttonLugdownSave.Font = new System.Drawing.Font("宋体", 9F);
            this.buttonLugdownSave.Location = new System.Drawing.Point(87, 231);
            this.buttonLugdownSave.Name = "buttonLugdownSave";
            this.buttonLugdownSave.Size = new System.Drawing.Size(86, 27);
            this.buttonLugdownSave.TabIndex = 59;
            this.buttonLugdownSave.Text = "保存";
            this.buttonLugdownSave.UseVisualStyleBackColor = true;
            this.buttonLugdownSave.Click += new System.EventHandler(this.buttonLugdownSave_Click);
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.comboBoxLugdownZsjCom);
            this.groupBox20.Controls.Add(this.label44);
            this.groupBox20.Controls.Add(this.comboBoxLugdownZsj);
            this.groupBox20.Location = new System.Drawing.Point(15, 181);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(227, 47);
            this.groupBox20.TabIndex = 58;
            this.groupBox20.TabStop = false;
            this.groupBox20.Text = "转速计选择";
            // 
            // comboBoxLugdownZsjCom
            // 
            this.comboBoxLugdownZsjCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLugdownZsjCom.Enabled = false;
            this.comboBoxLugdownZsjCom.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxLugdownZsjCom.ForeColor = System.Drawing.Color.Blue;
            this.comboBoxLugdownZsjCom.FormattingEnabled = true;
            this.comboBoxLugdownZsjCom.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6"});
            this.comboBoxLugdownZsjCom.Location = new System.Drawing.Point(147, 17);
            this.comboBoxLugdownZsjCom.Name = "comboBoxLugdownZsjCom";
            this.comboBoxLugdownZsjCom.Size = new System.Drawing.Size(74, 25);
            this.comboBoxLugdownZsjCom.TabIndex = 40;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Font = new System.Drawing.Font("宋体", 9F);
            this.label44.Location = new System.Drawing.Point(11, 25);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(41, 12);
            this.label44.TabIndex = 38;
            this.label44.Text = "转速计";
            // 
            // comboBoxLugdownZsj
            // 
            this.comboBoxLugdownZsj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLugdownZsj.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxLugdownZsj.ForeColor = System.Drawing.Color.Blue;
            this.comboBoxLugdownZsj.FormattingEnabled = true;
            this.comboBoxLugdownZsj.Items.AddRange(new object[] {
            "废气仪",
            "烟度计",
            "VMT-2000",
            "VUT-3000",
            "MQZ-2",
            "MQZ-3"});
            this.comboBoxLugdownZsj.Location = new System.Drawing.Point(60, 17);
            this.comboBoxLugdownZsj.Name = "comboBoxLugdownZsj";
            this.comboBoxLugdownZsj.Size = new System.Drawing.Size(79, 25);
            this.comboBoxLugdownZsj.TabIndex = 37;
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.label47);
            this.groupBox22.Controls.Add(this.textBoxLugdownSmpl);
            this.groupBox22.Controls.Add(this.label55);
            this.groupBox22.Controls.Add(this.label53);
            this.groupBox22.Controls.Add(this.textBoxLugDownMinSpeed);
            this.groupBox22.Controls.Add(this.label54);
            this.groupBox22.Controls.Add(this.label49);
            this.groupBox22.Controls.Add(this.textBoxLugDownMaxSpeed);
            this.groupBox22.Controls.Add(this.label52);
            this.groupBox22.Location = new System.Drawing.Point(15, 17);
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.Size = new System.Drawing.Size(227, 93);
            this.groupBox22.TabIndex = 55;
            this.groupBox22.TabStop = false;
            this.groupBox22.Text = "参数设置";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Font = new System.Drawing.Font("宋体", 9F);
            this.label47.Location = new System.Drawing.Point(176, 71);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(23, 12);
            this.label47.TabIndex = 59;
            this.label47.Text = "N/s";
            // 
            // textBoxLugdownSmpl
            // 
            this.textBoxLugdownSmpl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLugdownSmpl.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxLugdownSmpl.ForeColor = System.Drawing.Color.Blue;
            this.textBoxLugdownSmpl.Location = new System.Drawing.Point(113, 65);
            this.textBoxLugdownSmpl.Name = "textBoxLugdownSmpl";
            this.textBoxLugdownSmpl.Size = new System.Drawing.Size(56, 23);
            this.textBoxLugdownSmpl.TabIndex = 57;
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Font = new System.Drawing.Font("宋体", 9F);
            this.label55.Location = new System.Drawing.Point(15, 71);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(53, 12);
            this.label55.TabIndex = 58;
            this.label55.Text = "扫描频率";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Font = new System.Drawing.Font("宋体", 9F);
            this.label53.Location = new System.Drawing.Point(176, 22);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(29, 12);
            this.label53.TabIndex = 56;
            this.label53.Text = "km/h";
            // 
            // textBoxLugDownMinSpeed
            // 
            this.textBoxLugDownMinSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLugDownMinSpeed.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxLugDownMinSpeed.ForeColor = System.Drawing.Color.Blue;
            this.textBoxLugDownMinSpeed.Location = new System.Drawing.Point(113, 16);
            this.textBoxLugDownMinSpeed.Name = "textBoxLugDownMinSpeed";
            this.textBoxLugDownMinSpeed.Size = new System.Drawing.Size(56, 23);
            this.textBoxLugDownMinSpeed.TabIndex = 54;
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Font = new System.Drawing.Font("宋体", 9F);
            this.label54.Location = new System.Drawing.Point(15, 20);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(77, 12);
            this.label54.TabIndex = 55;
            this.label54.Text = "扫描最低速度";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Font = new System.Drawing.Font("宋体", 9F);
            this.label49.Location = new System.Drawing.Point(176, 48);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(29, 12);
            this.label49.TabIndex = 53;
            this.label49.Text = "km/h";
            // 
            // textBoxLugDownMaxSpeed
            // 
            this.textBoxLugDownMaxSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLugDownMaxSpeed.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxLugDownMaxSpeed.ForeColor = System.Drawing.Color.Blue;
            this.textBoxLugDownMaxSpeed.Location = new System.Drawing.Point(113, 40);
            this.textBoxLugDownMaxSpeed.Name = "textBoxLugDownMaxSpeed";
            this.textBoxLugDownMaxSpeed.Size = new System.Drawing.Size(56, 23);
            this.textBoxLugDownMaxSpeed.TabIndex = 51;
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Font = new System.Drawing.Font("宋体", 9F);
            this.label52.Location = new System.Drawing.Point(15, 46);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(77, 12);
            this.label52.TabIndex = 52;
            this.label52.Text = "扫描最大速度";
            // 
            // settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 351);
            this.Controls.Add(this.groupBox19);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "settings";
            this.Text = "系统设置";
            this.Load += new System.EventHandler(this.settings_Load);
            this.groupBox19.ResumeLayout(false);
            this.groupBox21.ResumeLayout(false);
            this.groupBox21.PerformLayout();
            this.groupBox20.ResumeLayout(false);
            this.groupBox20.PerformLayout();
            this.groupBox22.ResumeLayout(false);
            this.groupBox22.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox19;
        private System.Windows.Forms.GroupBox groupBox21;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.RadioButton radioButtonLugdownHgl;
        private System.Windows.Forms.RadioButton radioButtonLugdownhs;
        private System.Windows.Forms.CheckBox checkBoxLugdownSureTemp;
        private System.Windows.Forms.Button buttonLugdownSave;
        private System.Windows.Forms.GroupBox groupBox20;
        private System.Windows.Forms.ComboBox comboBoxLugdownZsjCom;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.ComboBox comboBoxLugdownZsj;
        private System.Windows.Forms.GroupBox groupBox22;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.TextBox textBoxLugdownSmpl;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.TextBox textBoxLugDownMinSpeed;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.TextBox textBoxLugDownMaxSpeed;
        private System.Windows.Forms.Label label52;


    }
}