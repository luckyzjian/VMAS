namespace Detect
{
    partial class carLogin
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel_Login = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_caiyou = new System.Windows.Forms.RadioButton();
            this.radioButton_qiyou = new System.Windows.Forms.RadioButton();
            this.comboBoxJcff = new System.Windows.Forms.ComboBox();
            this.label52 = new System.Windows.Forms.Label();
            this.simpleButtonLogin = new DevExpress.XtraEditors.SimpleButton();
            this.panel4.SuspendLayout();
            this.panel_Login.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.panel_Login);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(855, 241);
            this.panel4.TabIndex = 38;
            // 
            // panel_Login
            // 
            this.panel_Login.Controls.Add(this.groupBox2);
            this.panel_Login.Controls.Add(this.simpleButtonLogin);
            this.panel_Login.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Login.Location = new System.Drawing.Point(0, 0);
            this.panel_Login.Name = "panel_Login";
            this.panel_Login.Size = new System.Drawing.Size(853, 239);
            this.panel_Login.TabIndex = 110;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.comboBoxJcff);
            this.groupBox2.Controls.Add(this.label52);
            this.groupBox2.Location = new System.Drawing.Point(271, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(310, 160);
            this.groupBox2.TabIndex = 116;
            this.groupBox2.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_caiyou);
            this.groupBox1.Controls.Add(this.radioButton_qiyou);
            this.groupBox1.Location = new System.Drawing.Point(64, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(178, 85);
            this.groupBox1.TabIndex = 115;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "检测车辆类型";
            // 
            // radioButton_caiyou
            // 
            this.radioButton_caiyou.AutoSize = true;
            this.radioButton_caiyou.Location = new System.Drawing.Point(59, 50);
            this.radioButton_caiyou.Name = "radioButton_caiyou";
            this.radioButton_caiyou.Size = new System.Drawing.Size(62, 21);
            this.radioButton_caiyou.TabIndex = 1;
            this.radioButton_caiyou.TabStop = true;
            this.radioButton_caiyou.Text = "柴油车";
            this.radioButton_caiyou.UseVisualStyleBackColor = true;
            this.radioButton_caiyou.CheckedChanged += new System.EventHandler(this.caiyouCarSelected);
            // 
            // radioButton_qiyou
            // 
            this.radioButton_qiyou.AutoSize = true;
            this.radioButton_qiyou.Location = new System.Drawing.Point(59, 23);
            this.radioButton_qiyou.Name = "radioButton_qiyou";
            this.radioButton_qiyou.Size = new System.Drawing.Size(62, 21);
            this.radioButton_qiyou.TabIndex = 0;
            this.radioButton_qiyou.TabStop = true;
            this.radioButton_qiyou.Text = "汽油车";
            this.radioButton_qiyou.UseVisualStyleBackColor = true;
            this.radioButton_qiyou.CheckedChanged += new System.EventHandler(this.carStyleChanged);
            // 
            // comboBoxJcff
            // 
            this.comboBoxJcff.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.comboBoxJcff.AutoCompleteCustomSource.AddRange(new string[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6"});
            this.comboBoxJcff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxJcff.FormattingEnabled = true;
            this.comboBoxJcff.Location = new System.Drawing.Point(123, 113);
            this.comboBoxJcff.Name = "comboBoxJcff";
            this.comboBoxJcff.Size = new System.Drawing.Size(113, 25);
            this.comboBoxJcff.TabIndex = 89;
            // 
            // label52
            // 
            this.label52.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(61, 116);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(59, 17);
            this.label52.TabIndex = 32;
            this.label52.Text = "检测方法:";
            // 
            // simpleButtonLogin
            // 
            this.simpleButtonLogin.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.simpleButtonLogin.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.simpleButtonLogin.Location = new System.Drawing.Point(384, 176);
            this.simpleButtonLogin.Name = "simpleButtonLogin";
            this.simpleButtonLogin.Size = new System.Drawing.Size(124, 23);
            this.simpleButtonLogin.TabIndex = 114;
            this.simpleButtonLogin.Text = "进入注册";
            this.simpleButtonLogin.Click += new System.EventHandler(this.simpleButtonLogin_Click);
            // 
            // carLogin
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(855, 241);
            this.Controls.Add(this.panel4);
            this.Name = "carLogin";
            this.Text = "Form1";
            this.panel4.ResumeLayout(false);
            this.panel_Login.ResumeLayout(false);
            this.panel_Login.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox comboBoxJcff;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Panel panel_Login;
        private DevExpress.XtraEditors.SimpleButton simpleButtonLogin;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_caiyou;
        private System.Windows.Forms.RadioButton radioButton_qiyou;
    }
}