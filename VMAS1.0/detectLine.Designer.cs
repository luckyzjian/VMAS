namespace VMAS1._0
{
    partial class detectLine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(detectLine));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel_newjiancexian = new System.Windows.Forms.Panel();
            this.panel_jiancexian = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Wait_Car_menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemqx = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGrid_waitcar = new System.Windows.Forms.DataGridView();
            this.查看ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel_jiancexian.SuspendLayout();
            this.Wait_Car_menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_waitcar)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.CadetBlue;
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(962, 117);
            this.panel1.TabIndex = 2;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 30F, System.Drawing.FontStyle.Bold);
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label12.Location = new System.Drawing.Point(151, 32);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(223, 52);
            this.label12.TabIndex = 3;
            this.label12.Text = "检测线设置";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(32, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 99);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // panel_newjiancexian
            // 
            this.panel_newjiancexian.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.panel_newjiancexian.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel_newjiancexian.Location = new System.Drawing.Point(0, 119);
            this.panel_newjiancexian.Name = "panel_newjiancexian";
            this.panel_newjiancexian.Size = new System.Drawing.Size(234, 449);
            this.panel_newjiancexian.TabIndex = 6;
            // 
            // panel_jiancexian
            // 
            this.panel_jiancexian.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_jiancexian.Controls.Add(this.dataGrid_waitcar);
            this.panel_jiancexian.Location = new System.Drawing.Point(237, 119);
            this.panel_jiancexian.Name = "panel_jiancexian";
            this.panel_jiancexian.Size = new System.Drawing.Size(722, 245);
            this.panel_jiancexian.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Location = new System.Drawing.Point(237, 370);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(722, 198);
            this.panel2.TabIndex = 8;
            // 
            // Wait_Car_menu
            // 
            this.Wait_Car_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemqx,
            this.刷新ToolStripMenuItem,
            this.查看ToolStripMenuItem});
            this.Wait_Car_menu.Name = "Wait_Car_menu";
            this.Wait_Car_menu.Size = new System.Drawing.Size(153, 92);
            this.Wait_Car_menu.Opening += new System.ComponentModel.CancelEventHandler(this.Wait_Car_menu_Opening);
            // 
            // ToolStripMenuItemqx
            // 
            this.ToolStripMenuItemqx.Name = "ToolStripMenuItemqx";
            this.ToolStripMenuItemqx.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItemqx.Text = "删除";
            this.ToolStripMenuItemqx.Click += new System.EventHandler(this.deleteSelectedCars);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // dataGrid_waitcar
            // 
            this.dataGrid_waitcar.AllowUserToAddRows = false;
            this.dataGrid_waitcar.AllowUserToDeleteRows = false;
            this.dataGrid_waitcar.AllowUserToOrderColumns = true;
            this.dataGrid_waitcar.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            this.dataGrid_waitcar.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGrid_waitcar.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGrid_waitcar.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGrid_waitcar.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGrid_waitcar.ContextMenuStrip = this.Wait_Car_menu;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGrid_waitcar.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGrid_waitcar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid_waitcar.GridColor = System.Drawing.Color.Gray;
            this.dataGrid_waitcar.Location = new System.Drawing.Point(0, 0);
            this.dataGrid_waitcar.Name = "dataGrid_waitcar";
            this.dataGrid_waitcar.ReadOnly = true;
            this.dataGrid_waitcar.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGrid_waitcar.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGrid_waitcar.RowHeadersVisible = false;
            this.dataGrid_waitcar.RowHeadersWidth = 80;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            this.dataGrid_waitcar.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGrid_waitcar.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGrid_waitcar.RowTemplate.DefaultCellStyle.Format = "D";
            this.dataGrid_waitcar.RowTemplate.DefaultCellStyle.NullValue = null;
            this.dataGrid_waitcar.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(255)))));
            this.dataGrid_waitcar.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGrid_waitcar.RowTemplate.Height = 30;
            this.dataGrid_waitcar.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid_waitcar.ShowCellErrors = false;
            this.dataGrid_waitcar.ShowRowErrors = false;
            this.dataGrid_waitcar.Size = new System.Drawing.Size(722, 245);
            this.dataGrid_waitcar.TabIndex = 29;
            this.dataGrid_waitcar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGrid_waitcar_Scroll);
            this.dataGrid_waitcar.SelectionChanged += new System.EventHandler(this.dataGrid_waitcar_SelectionChanged_1);
            // 
            // 查看ToolStripMenuItem
            // 
            this.查看ToolStripMenuItem.Name = "查看ToolStripMenuItem";
            this.查看ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.查看ToolStripMenuItem.Text = "查看";
            this.查看ToolStripMenuItem.Click += new System.EventHandler(this.查看ToolStripMenuItem_Click);
            // 
            // detectLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 573);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel_jiancexian);
            this.Controls.Add(this.panel_newjiancexian);
            this.Controls.Add(this.panel1);
            this.Name = "detectLine";
            this.Text = "detectLine";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.detectLine_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel_jiancexian.ResumeLayout(false);
            this.Wait_Car_menu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_waitcar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel_newjiancexian;
        private System.Windows.Forms.Panel panel_jiancexian;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ContextMenuStrip Wait_Car_menu;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemqx;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGrid_waitcar;
        private System.Windows.Forms.ToolStripMenuItem 查看ToolStripMenuItem;
    }
}