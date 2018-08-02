namespace Detect
{
    partial class Asm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label_msg = new System.Windows.Forms.Label();
            this.button_kztt = new System.Windows.Forms.Button();
            this.button_ss = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.panel_gl = new System.Windows.Forms.Panel();
            this.Msg_gl = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.panel_nl = new System.Windows.Forms.Panel();
            this.Msg_nl = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel_cs = new System.Windows.Forms.Panel();
            this.Msg_cs = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel_cp = new System.Windows.Forms.Panel();
            this.Msg_cp = new System.Windows.Forms.Label();
            this.panel_msg = new System.Windows.Forms.Panel();
            this.Msg_msg = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel_gl.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel_nl.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel_cs.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel_cp.SuspendLayout();
            this.panel_msg.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.chart1);
            this.panel1.Location = new System.Drawing.Point(2, 232);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1347, 382);
            this.panel1.TabIndex = 1;
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.Teal;
            chartArea1.CursorX.Interval = 100000D;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            legend2.Enabled = false;
            legend2.Name = "Legend2";
            this.chart1.Legends.Add(legend1);
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.LegendText = "速度上限";
            series1.Name = "seriesCeiling";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.LegendText = "速度";
            series2.Name = "seriesdata";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.LegendText = "速度下限";
            series3.Name = "seriesLower";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Series.Add(series3);
            this.chart1.Size = new System.Drawing.Size(1343, 378);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Controls.Add(this.button_kztt);
            this.panel2.Controls.Add(this.button_ss);
            this.panel2.Location = new System.Drawing.Point(1, 620);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1346, 108);
            this.panel2.TabIndex = 2;
            // 
            // panel7
            // 
            this.panel7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel7.Controls.Add(this.label4);
            this.panel7.Controls.Add(this.panel9);
            this.panel7.Location = new System.Drawing.Point(3, 39);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(338, 62);
            this.panel7.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label4.Location = new System.Drawing.Point(148, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "消息";
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.Black;
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel9.Controls.Add(this.label_msg);
            this.panel9.Location = new System.Drawing.Point(5, 24);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(331, 36);
            this.panel9.TabIndex = 1;
            // 
            // label_msg
            // 
            this.label_msg.AutoSize = true;
            this.label_msg.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Bold);
            this.label_msg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label_msg.Location = new System.Drawing.Point(128, 5);
            this.label_msg.Name = "label_msg";
            this.label_msg.Size = new System.Drawing.Size(70, 22);
            this.label_msg.TabIndex = 0;
            this.label_msg.Text = "Empty";
            // 
            // button_kztt
            // 
            this.button_kztt.Location = new System.Drawing.Point(1150, 39);
            this.button_kztt.Name = "button_kztt";
            this.button_kztt.Size = new System.Drawing.Size(75, 56);
            this.button_kztt.TabIndex = 2;
            this.button_kztt.Text = "台体控制";
            this.button_kztt.UseVisualStyleBackColor = true;
            this.button_kztt.Click += new System.EventHandler(this.button_kztt_Click);
            // 
            // button_ss
            // 
            this.button_ss.Location = new System.Drawing.Point(1251, 39);
            this.button_ss.Name = "button_ss";
            this.button_ss.Size = new System.Drawing.Size(75, 56);
            this.button_ss.TabIndex = 0;
            this.button_ss.Text = "开始检测";
            this.button_ss.UseVisualStyleBackColor = true;
            this.button_ss.Click += new System.EventHandler(this.button_ss_Click);
            // 
            // panel3
            // 
            this.panel3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.panel11);
            this.panel3.Controls.Add(this.panel8);
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Location = new System.Drawing.Point(1, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1346, 123);
            this.panel3.TabIndex = 3;
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.label8);
            this.panel11.Controls.Add(this.panel_gl);
            this.panel11.Location = new System.Drawing.Point(674, 3);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(335, 118);
            this.panel11.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 22F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label8.Location = new System.Drawing.Point(96, 5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(139, 30);
            this.label8.TabIndex = 0;
            this.label8.Text = "功率(KW)";
            // 
            // panel_gl
            // 
            this.panel_gl.BackColor = System.Drawing.Color.Black;
            this.panel_gl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_gl.Controls.Add(this.Msg_gl);
            this.panel_gl.Location = new System.Drawing.Point(4, 38);
            this.panel_gl.Name = "panel_gl";
            this.panel_gl.Size = new System.Drawing.Size(328, 78);
            this.panel_gl.TabIndex = 1;
            // 
            // Msg_gl
            // 
            this.Msg_gl.AutoSize = true;
            this.Msg_gl.Font = new System.Drawing.Font("宋体", 45F, System.Drawing.FontStyle.Bold);
            this.Msg_gl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.Msg_gl.Location = new System.Drawing.Point(103, 7);
            this.Msg_gl.Name = "Msg_gl";
            this.Msg_gl.Size = new System.Drawing.Size(118, 60);
            this.Msg_gl.TabIndex = 0;
            this.Msg_gl.Text = "0.0";
            this.Msg_gl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label5);
            this.panel8.Controls.Add(this.panel_nl);
            this.panel8.Location = new System.Drawing.Point(1010, 3);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(335, 118);
            this.panel8.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 22F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label5.Location = new System.Drawing.Point(104, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 30);
            this.label5.TabIndex = 0;
            this.label5.Text = "扭力(N)";
            // 
            // panel_nl
            // 
            this.panel_nl.BackColor = System.Drawing.Color.Black;
            this.panel_nl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_nl.Controls.Add(this.Msg_nl);
            this.panel_nl.Location = new System.Drawing.Point(4, 38);
            this.panel_nl.Name = "panel_nl";
            this.panel_nl.Size = new System.Drawing.Size(328, 78);
            this.panel_nl.TabIndex = 1;
            // 
            // Msg_nl
            // 
            this.Msg_nl.AutoSize = true;
            this.Msg_nl.Font = new System.Drawing.Font("宋体", 45F, System.Drawing.FontStyle.Bold);
            this.Msg_nl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.Msg_nl.Location = new System.Drawing.Point(103, 7);
            this.Msg_nl.Name = "Msg_nl";
            this.Msg_nl.Size = new System.Drawing.Size(118, 60);
            this.Msg_nl.TabIndex = 0;
            this.Msg_nl.Text = "0.0";
            this.Msg_nl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label2);
            this.panel6.Controls.Add(this.panel_cs);
            this.panel6.Location = new System.Drawing.Point(338, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(335, 118);
            this.panel6.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 22F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label2.Location = new System.Drawing.Point(82, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 30);
            this.label2.TabIndex = 0;
            this.label2.Text = "车速(KM/H)";
            // 
            // panel_cs
            // 
            this.panel_cs.BackColor = System.Drawing.Color.Black;
            this.panel_cs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_cs.Controls.Add(this.Msg_cs);
            this.panel_cs.Location = new System.Drawing.Point(4, 38);
            this.panel_cs.Name = "panel_cs";
            this.panel_cs.Size = new System.Drawing.Size(328, 78);
            this.panel_cs.TabIndex = 1;
            // 
            // Msg_cs
            // 
            this.Msg_cs.AutoSize = true;
            this.Msg_cs.Font = new System.Drawing.Font("宋体", 45F, System.Drawing.FontStyle.Bold);
            this.Msg_cs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.Msg_cs.Location = new System.Drawing.Point(103, 7);
            this.Msg_cs.Name = "Msg_cs";
            this.Msg_cs.Size = new System.Drawing.Size(118, 60);
            this.Msg_cs.TabIndex = 0;
            this.Msg_cs.Text = "0.0";
            this.Msg_cs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label1);
            this.panel5.Controls.Add(this.panel_cp);
            this.panel5.Location = new System.Drawing.Point(2, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(335, 118);
            this.panel5.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 22F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(120, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "车牌";
            // 
            // panel_cp
            // 
            this.panel_cp.BackColor = System.Drawing.Color.Black;
            this.panel_cp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_cp.Controls.Add(this.Msg_cp);
            this.panel_cp.Location = new System.Drawing.Point(4, 38);
            this.panel_cp.Name = "panel_cp";
            this.panel_cp.Size = new System.Drawing.Size(328, 78);
            this.panel_cp.TabIndex = 1;
            // 
            // Msg_cp
            // 
            this.Msg_cp.AutoSize = true;
            this.Msg_cp.Font = new System.Drawing.Font("宋体", 45F, System.Drawing.FontStyle.Bold);
            this.Msg_cp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.Msg_cp.Location = new System.Drawing.Point(162, 7);
            this.Msg_cp.Name = "Msg_cp";
            this.Msg_cp.Size = new System.Drawing.Size(0, 60);
            this.Msg_cp.TabIndex = 0;
            this.Msg_cp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel_msg
            // 
            this.panel_msg.BackColor = System.Drawing.Color.Black;
            this.panel_msg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_msg.Controls.Add(this.Msg_msg);
            this.panel_msg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_msg.Location = new System.Drawing.Point(0, 0);
            this.panel_msg.Name = "panel_msg";
            this.panel_msg.Size = new System.Drawing.Size(1336, 100);
            this.panel_msg.TabIndex = 4;
            // 
            // Msg_msg
            // 
            this.Msg_msg.AutoSize = true;
            this.Msg_msg.Font = new System.Drawing.Font("宋体", 64F, System.Drawing.FontStyle.Bold);
            this.Msg_msg.ForeColor = System.Drawing.Color.Red;
            this.Msg_msg.Location = new System.Drawing.Point(671, 5);
            this.Msg_msg.Name = "Msg_msg";
            this.Msg_msg.Size = new System.Drawing.Size(0, 86);
            this.Msg_msg.TabIndex = 0;
            this.Msg_msg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.Controls.Add(this.panel_msg);
            this.panel4.Location = new System.Drawing.Point(7, 126);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1336, 100);
            this.panel4.TabIndex = 5;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Asm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 730);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Asm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Asm（稳态工况法）";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Asm_FormClosing);
            this.Load += new System.EventHandler(this.Asm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel_gl.ResumeLayout(false);
            this.panel_gl.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel_nl.ResumeLayout(false);
            this.panel_nl.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel_cs.ResumeLayout(false);
            this.panel_cs.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel_cp.ResumeLayout(false);
            this.panel_cp.PerformLayout();
            this.panel_msg.ResumeLayout(false);
            this.panel_msg.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button_ss;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel_cp;
        private System.Windows.Forms.Label Msg_cp;
        private System.Windows.Forms.Panel panel_msg;
        private System.Windows.Forms.Label Msg_msg;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel_gl;
        private System.Windows.Forms.Label Msg_gl;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel_nl;
        private System.Windows.Forms.Label Msg_nl;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel_cs;
        private System.Windows.Forms.Label Msg_cs;
        private System.Windows.Forms.Button button_kztt;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label label_msg;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Timer timer1;
    }
}