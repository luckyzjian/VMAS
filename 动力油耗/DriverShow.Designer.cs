namespace 动力油耗
{
    partial class DriverShow
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
            this.labelSD = new System.Windows.Forms.Label();
            this.labelJZL = new System.Windows.Forms.Label();
            this.labelJZGL = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panelSD = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelTS = new System.Windows.Forms.Label();
            this.panelJZGL = new System.Windows.Forms.Panel();
            this.panelTS = new System.Windows.Forms.Panel();
            this.panelJZL = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelZs = new System.Windows.Forms.Label();
            this.panelLED = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panelYH = new System.Windows.Forms.Panel();
            this.labelYH = new System.Windows.Forms.Label();
            this.panelXSLC = new System.Windows.Forms.Panel();
            this.labelXSLC = new System.Windows.Forms.Label();
            this.panelSD.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelJZGL.SuspendLayout();
            this.panelTS.SuspendLayout();
            this.panelJZL.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelLED.SuspendLayout();
            this.panelYH.SuspendLayout();
            this.panelXSLC.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelSD
            // 
            this.labelSD.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelSD.Font = new System.Drawing.Font("DS-Digital", 80F);
            this.labelSD.ForeColor = System.Drawing.Color.Lime;
            this.labelSD.Location = new System.Drawing.Point(4, 0);
            this.labelSD.Name = "labelSD";
            this.labelSD.Size = new System.Drawing.Size(286, 100);
            this.labelSD.TabIndex = 0;
            this.labelSD.Text = "0.0";
            this.labelSD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelJZL
            // 
            this.labelJZL.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelJZL.Font = new System.Drawing.Font("DS-Digital", 40F);
            this.labelJZL.ForeColor = System.Drawing.Color.Lime;
            this.labelJZL.Location = new System.Drawing.Point(3, 3);
            this.labelJZL.Name = "labelJZL";
            this.labelJZL.Size = new System.Drawing.Size(287, 53);
            this.labelJZL.TabIndex = 0;
            this.labelJZL.Text = "0";
            this.labelJZL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelJZGL
            // 
            this.labelJZGL.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelJZGL.Font = new System.Drawing.Font("DS-Digital", 40F);
            this.labelJZGL.ForeColor = System.Drawing.Color.Lime;
            this.labelJZGL.Location = new System.Drawing.Point(4, 5);
            this.labelJZGL.Name = "labelJZGL";
            this.labelJZGL.Size = new System.Drawing.Size(286, 53);
            this.labelJZGL.TabIndex = 0;
            this.labelJZGL.Text = "0.0";
            this.labelJZGL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 20F);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(91, 341);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 27);
            this.label6.TabIndex = 107;
            this.label6.Text = "功率(kW)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 20F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(90, 142);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(135, 27);
            this.label5.TabIndex = 106;
            this.label5.Text = "加载力(N)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 20F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(74, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 27);
            this.label4.TabIndex = 105;
            this.label4.Text = "速度(km/h)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelSD
            // 
            this.panelSD.BackColor = System.Drawing.Color.Black;
            this.panelSD.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelSD.Controls.Add(this.labelSD);
            this.panelSD.Location = new System.Drawing.Point(2, 34);
            this.panelSD.Name = "panelSD";
            this.panelSD.Size = new System.Drawing.Size(297, 104);
            this.panelSD.TabIndex = 101;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(300, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1047, 634);
            this.pictureBox1.TabIndex = 100;
            this.pictureBox1.TabStop = false;
            // 
            // labelTS
            // 
            this.labelTS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTS.Font = new System.Drawing.Font("宋体", 80F, System.Drawing.FontStyle.Bold);
            this.labelTS.ForeColor = System.Drawing.Color.White;
            this.labelTS.Location = new System.Drawing.Point(3, 9);
            this.labelTS.Name = "labelTS";
            this.labelTS.Size = new System.Drawing.Size(1334, 106);
            this.labelTS.TabIndex = 1;
            this.labelTS.Text = "请上线准备";
            this.labelTS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelJZGL
            // 
            this.panelJZGL.BackColor = System.Drawing.Color.Black;
            this.panelJZGL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelJZGL.Controls.Add(this.labelJZGL);
            this.panelJZGL.Location = new System.Drawing.Point(2, 372);
            this.panelJZGL.Name = "panelJZGL";
            this.panelJZGL.Size = new System.Drawing.Size(297, 65);
            this.panelJZGL.TabIndex = 103;
            // 
            // panelTS
            // 
            this.panelTS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.panelTS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTS.Controls.Add(this.labelTS);
            this.panelTS.Location = new System.Drawing.Point(3, 3);
            this.panelTS.Name = "panelTS";
            this.panelTS.Size = new System.Drawing.Size(1342, 117);
            this.panelTS.TabIndex = 3;
            // 
            // panelJZL
            // 
            this.panelJZL.BackColor = System.Drawing.Color.Black;
            this.panelJZL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelJZL.Controls.Add(this.labelJZL);
            this.panelJZL.Location = new System.Drawing.Point(2, 172);
            this.panelJZL.Name = "panelJZL";
            this.panelJZL.Size = new System.Drawing.Size(297, 65);
            this.panelJZL.TabIndex = 102;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.panelYH);
            this.panel1.Controls.Add(this.panelXSLC);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.panelJZGL);
            this.panel1.Controls.Add(this.panelJZL);
            this.panel1.Controls.Add(this.panelSD);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 128);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1350, 643);
            this.panel1.TabIndex = 103;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 20F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(72, 241);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 27);
            this.label1.TabIndex = 109;
            this.label1.Text = "转速(r/min)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.labelZs);
            this.panel2.Location = new System.Drawing.Point(2, 271);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(297, 65);
            this.panel2.TabIndex = 108;
            // 
            // labelZs
            // 
            this.labelZs.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelZs.Font = new System.Drawing.Font("DS-Digital", 40F);
            this.labelZs.ForeColor = System.Drawing.Color.Lime;
            this.labelZs.Location = new System.Drawing.Point(4, 4);
            this.labelZs.Name = "labelZs";
            this.labelZs.Size = new System.Drawing.Size(286, 53);
            this.labelZs.TabIndex = 0;
            this.labelZs.Text = "0";
            this.labelZs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelLED
            // 
            this.panelLED.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panelLED.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLED.Controls.Add(this.panelTS);
            this.panelLED.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLED.Location = new System.Drawing.Point(0, 0);
            this.panelLED.Name = "panelLED";
            this.panelLED.Size = new System.Drawing.Size(1350, 128);
            this.panelLED.TabIndex = 102;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 20F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(69, 440);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 27);
            this.label2.TabIndex = 116;
            this.label2.Text = "行驶里程(m)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 20F);
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(90, 539);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(122, 27);
            this.label7.TabIndex = 115;
            this.label7.Text = "油耗(ml)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelYH
            // 
            this.panelYH.BackColor = System.Drawing.Color.Black;
            this.panelYH.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelYH.Controls.Add(this.labelYH);
            this.panelYH.Location = new System.Drawing.Point(2, 570);
            this.panelYH.Name = "panelYH";
            this.panelYH.Size = new System.Drawing.Size(297, 65);
            this.panelYH.TabIndex = 114;
            // 
            // labelYH
            // 
            this.labelYH.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelYH.Font = new System.Drawing.Font("DS-Digital", 40F);
            this.labelYH.ForeColor = System.Drawing.Color.Lime;
            this.labelYH.Location = new System.Drawing.Point(4, 4);
            this.labelYH.Name = "labelYH";
            this.labelYH.Size = new System.Drawing.Size(286, 53);
            this.labelYH.TabIndex = 0;
            this.labelYH.Text = "0.0";
            this.labelYH.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelXSLC
            // 
            this.panelXSLC.BackColor = System.Drawing.Color.Black;
            this.panelXSLC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelXSLC.Controls.Add(this.labelXSLC);
            this.panelXSLC.Location = new System.Drawing.Point(2, 470);
            this.panelXSLC.Name = "panelXSLC";
            this.panelXSLC.Size = new System.Drawing.Size(297, 65);
            this.panelXSLC.TabIndex = 113;
            // 
            // labelXSLC
            // 
            this.labelXSLC.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelXSLC.Font = new System.Drawing.Font("DS-Digital", 40F);
            this.labelXSLC.ForeColor = System.Drawing.Color.Lime;
            this.labelXSLC.Location = new System.Drawing.Point(3, 4);
            this.labelXSLC.Name = "labelXSLC";
            this.labelXSLC.Size = new System.Drawing.Size(287, 53);
            this.labelXSLC.TabIndex = 0;
            this.labelXSLC.Text = "0.0";
            this.labelXSLC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DriverShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 771);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelLED);
            this.Name = "DriverShow";
            this.Text = "DriverShow";
            this.Load += new System.EventHandler(this.DriverShow_Load);
            this.panelSD.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelJZGL.ResumeLayout(false);
            this.panelTS.ResumeLayout(false);
            this.panelJZL.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panelLED.ResumeLayout(false);
            this.panelYH.ResumeLayout(false);
            this.panelXSLC.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelSD;
        private System.Windows.Forms.Label labelJZL;
        private System.Windows.Forms.Label labelJZGL;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panelSD;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelTS;
        private System.Windows.Forms.Panel panelJZGL;
        private System.Windows.Forms.Panel panelTS;
        private System.Windows.Forms.Panel panelJZL;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelLED;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelZs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panelYH;
        private System.Windows.Forms.Label labelYH;
        private System.Windows.Forms.Panel panelXSLC;
        private System.Windows.Forms.Label labelXSLC;
    }
}