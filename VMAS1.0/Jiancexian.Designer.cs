namespace VMAS1._0
{
    partial class Jiancexian
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label41 = new System.Windows.Forms.Label();
            this.ButtonDeletCar = new DevExpress.XtraEditors.SimpleButton();
            this.dataGrid_waitcar = new System.Windows.Forms.DataGridView();
            this.Wait_Car_menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemqx = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_waitcar)).BeginInit();
            this.Wait_Car_menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.simpleButton1);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.ButtonDeletCar);
            this.panel2.Controls.Add(this.dataGrid_waitcar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(836, 470);
            this.panel2.TabIndex = 34;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.simpleButton1.Location = new System.Drawing.Point(565, 43);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(126, 23);
            this.simpleButton1.TabIndex = 30;
            this.simpleButton1.Text = "保      存";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.LightSkyBlue;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel6.Controls.Add(this.label41);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(834, 39);
            this.panel6.TabIndex = 29;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label41.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label41.ForeColor = System.Drawing.Color.Blue;
            this.label41.Location = new System.Drawing.Point(8, 6);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(114, 22);
            this.label41.TabIndex = 0;
            this.label41.Text = ">>检测线信息";
            // 
            // ButtonDeletCar
            // 
            this.ButtonDeletCar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonDeletCar.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.ButtonDeletCar.Location = new System.Drawing.Point(697, 43);
            this.ButtonDeletCar.Name = "ButtonDeletCar";
            this.ButtonDeletCar.Size = new System.Drawing.Size(126, 23);
            this.ButtonDeletCar.TabIndex = 26;
            this.ButtonDeletCar.Text = "删除选中线";
            this.ButtonDeletCar.Click += new System.EventHandler(this.deleteSelectedCars);
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
            this.dataGrid_waitcar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
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
            this.dataGrid_waitcar.GridColor = System.Drawing.Color.Gray;
            this.dataGrid_waitcar.Location = new System.Drawing.Point(-1, 72);
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
            this.dataGrid_waitcar.Size = new System.Drawing.Size(835, 396);
            this.dataGrid_waitcar.TabIndex = 28;
            this.dataGrid_waitcar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGrid_waitcar_Scroll);
            this.dataGrid_waitcar.SelectionChanged += new System.EventHandler(this.dataGrid_waitcar_SelectionChanged_1);
            // 
            // Wait_Car_menu
            // 
            this.Wait_Car_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemqx,
            this.刷新ToolStripMenuItem});
            this.Wait_Car_menu.Name = "Wait_Car_menu";
            this.Wait_Car_menu.Size = new System.Drawing.Size(101, 48);
            this.Wait_Car_menu.Opening += new System.ComponentModel.CancelEventHandler(this.Wait_Car_menu_Opening);
            // 
            // ToolStripMenuItemqx
            // 
            this.ToolStripMenuItemqx.Name = "ToolStripMenuItemqx";
            this.ToolStripMenuItemqx.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItemqx.Text = "删除";
            this.ToolStripMenuItemqx.Click += new System.EventHandler(this.deleteSelectedCars);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // Jiancexian
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 470);
            this.Controls.Add(this.panel2);
            this.Name = "Jiancexian";
            this.Text = "carWait";
            this.Load += new System.EventHandler(this.CarWait_Load);
            this.panel2.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_waitcar)).EndInit();
            this.Wait_Car_menu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label41;
        private DevExpress.XtraEditors.SimpleButton ButtonDeletCar;
        private System.Windows.Forms.DataGridView dataGrid_waitcar;
        private System.Windows.Forms.ContextMenuStrip Wait_Car_menu;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemqx;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}