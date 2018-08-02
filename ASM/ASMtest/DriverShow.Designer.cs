namespace ASMtest
{
    partial class DriverShow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DriverShow));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panelTS3 = new System.Windows.Forms.Panel();
            this.labelTS3 = new System.Windows.Forms.Label();
            this.panelTrack = new System.Windows.Forms.Panel();
            this.panelSpeed = new System.Windows.Forms.Panel();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.pictureBoxFault = new System.Windows.Forms.PictureBox();
            this.pictureBoxTrue = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelTs2 = new System.Windows.Forms.Panel();
            this.labelts2 = new System.Windows.Forms.Label();
            this.panelts1 = new System.Windows.Forms.Panel();
            this.labelTs1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panelTS3.SuspendLayout();
            this.panelTrack.SuspendLayout();
            this.panelSpeed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFault)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrue)).BeginInit();
            this.panel2.SuspendLayout();
            this.panelTs2.SuspendLayout();
            this.panelts1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1424, 874);
            this.panel1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.panelTrack);
            this.panel4.Controls.Add(this.panelTS3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 349);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1424, 525);
            this.panel4.TabIndex = 2;
            // 
            // panelTS3
            // 
            this.panelTS3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTS3.Controls.Add(this.labelTS3);
            this.panelTS3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTS3.Location = new System.Drawing.Point(0, 466);
            this.panelTS3.Name = "panelTS3";
            this.panelTS3.Size = new System.Drawing.Size(1422, 57);
            this.panelTS3.TabIndex = 2;
            // 
            // labelTS3
            // 
            this.labelTS3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelTS3.AutoSize = true;
            this.labelTS3.Font = new System.Drawing.Font("宋体", 30F, System.Drawing.FontStyle.Bold);
            this.labelTS3.ForeColor = System.Drawing.Color.Red;
            this.labelTS3.Location = new System.Drawing.Point(211, 8);
            this.labelTS3.Name = "labelTS3";
            this.labelTS3.Size = new System.Drawing.Size(983, 40);
            this.labelTS3.TabIndex = 1;
            this.labelTS3.Text = "CO:-- HC:-- NOx:-- CO2:-- O2:--T:-- H:-- A:-- ";
            this.labelTS3.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // panelTrack
            // 
            this.panelTrack.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panelTrack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelTrack.BackgroundImage")));
            this.panelTrack.Controls.Add(this.panelSpeed);
            this.panelTrack.Controls.Add(this.pictureBoxFault);
            this.panelTrack.Controls.Add(this.pictureBoxTrue);
            this.panelTrack.Location = new System.Drawing.Point(11, 9);
            this.panelTrack.Name = "panelTrack";
            this.panelTrack.Size = new System.Drawing.Size(1400, 369);
            this.panelTrack.TabIndex = 0;
            // 
            // panelSpeed
            // 
            this.panelSpeed.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panelSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSpeed.Controls.Add(this.labelSpeed);
            this.panelSpeed.Location = new System.Drawing.Point(520, 244);
            this.panelSpeed.Name = "panelSpeed";
            this.panelSpeed.Size = new System.Drawing.Size(386, 119);
            this.panelSpeed.TabIndex = 3;
            // 
            // labelSpeed
            // 
            this.labelSpeed.AutoSize = true;
            this.labelSpeed.Font = new System.Drawing.Font("宋体", 80F, System.Drawing.FontStyle.Bold);
            this.labelSpeed.ForeColor = System.Drawing.Color.Lime;
            this.labelSpeed.Location = new System.Drawing.Point(96, 6);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(210, 107);
            this.labelSpeed.TabIndex = 2;
            this.labelSpeed.Text = "0.0";
            // 
            // pictureBoxFault
            // 
            this.pictureBoxFault.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxFault.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxFault.Image")));
            this.pictureBoxFault.Location = new System.Drawing.Point(83, 25);
            this.pictureBoxFault.Name = "pictureBoxFault";
            this.pictureBoxFault.Size = new System.Drawing.Size(50, 150);
            this.pictureBoxFault.TabIndex = 2;
            this.pictureBoxFault.TabStop = false;
            // 
            // pictureBoxTrue
            // 
            this.pictureBoxTrue.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxTrue.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxTrue.Image")));
            this.pictureBoxTrue.Location = new System.Drawing.Point(83, 25);
            this.pictureBoxTrue.Name = "pictureBoxTrue";
            this.pictureBoxTrue.Size = new System.Drawing.Size(50, 150);
            this.pictureBoxTrue.TabIndex = 1;
            this.pictureBoxTrue.TabStop = false;
            this.pictureBoxTrue.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panelTs2);
            this.panel2.Controls.Add(this.panelts1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1424, 349);
            this.panel2.TabIndex = 0;
            // 
            // panelTs2
            // 
            this.panelTs2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTs2.Controls.Add(this.labelts2);
            this.panelTs2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTs2.Location = new System.Drawing.Point(0, 167);
            this.panelTs2.Name = "panelTs2";
            this.panelTs2.Size = new System.Drawing.Size(1422, 178);
            this.panelTs2.TabIndex = 1;
            // 
            // labelts2
            // 
            this.labelts2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelts2.AutoSize = true;
            this.labelts2.Font = new System.Drawing.Font("宋体", 120F, System.Drawing.FontStyle.Bold);
            this.labelts2.ForeColor = System.Drawing.Color.Red;
            this.labelts2.Location = new System.Drawing.Point(262, 6);
            this.labelts2.Name = "labelts2";
            this.labelts2.Size = new System.Drawing.Size(873, 161);
            this.labelts2.TabIndex = 1;
            this.labelts2.Text = "稳态工况法";
            this.labelts2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // panelts1
            // 
            this.panelts1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelts1.Controls.Add(this.labelTs1);
            this.panelts1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelts1.Location = new System.Drawing.Point(0, 0);
            this.panelts1.Name = "panelts1";
            this.panelts1.Size = new System.Drawing.Size(1422, 167);
            this.panelts1.TabIndex = 0;
            // 
            // labelTs1
            // 
            this.labelTs1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelTs1.AutoSize = true;
            this.labelTs1.Font = new System.Drawing.Font("宋体", 100F, System.Drawing.FontStyle.Bold);
            this.labelTs1.ForeColor = System.Drawing.Color.Red;
            this.labelTs1.Location = new System.Drawing.Point(411, 19);
            this.labelTs1.Name = "labelTs1";
            this.labelTs1.Size = new System.Drawing.Size(601, 134);
            this.labelTs1.TabIndex = 0;
            this.labelTs1.Text = "--------";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // DriverShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1424, 874);
            this.Controls.Add(this.panel1);
            this.Name = "DriverShow";
            this.Text = "司机助提示";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panelTS3.ResumeLayout(false);
            this.panelTS3.PerformLayout();
            this.panelTrack.ResumeLayout(false);
            this.panelSpeed.ResumeLayout(false);
            this.panelSpeed.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFault)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrue)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panelTs2.ResumeLayout(false);
            this.panelTs2.PerformLayout();
            this.panelts1.ResumeLayout(false);
            this.panelts1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panelTs2;
        private System.Windows.Forms.Panel panelts1;
        private System.Windows.Forms.Panel panelTrack;
        private System.Windows.Forms.PictureBox pictureBoxTrue;
        private System.Windows.Forms.Label labelts2;
        private System.Windows.Forms.Label labelTs1;
        private System.Windows.Forms.PictureBox pictureBoxFault;
        private System.Windows.Forms.Panel panelSpeed;
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panelTS3;
        private System.Windows.Forms.Label labelTS3;
    }
}

