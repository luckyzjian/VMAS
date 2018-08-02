namespace VMAS1._0
{
    partial class demarcate
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFqy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemLlj = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemYdj = new System.Windows.Forms.ToolStripMenuItem();
            this.panelMain = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(880, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFqy,
            this.toolStripMenuItemLlj,
            this.ToolStripMenuItemYdj});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(100, 21);
            this.toolStripMenuItem1.Text = "核准项目（&B）";
            // 
            // toolStripMenuItemFqy
            // 
            this.toolStripMenuItemFqy.Name = "toolStripMenuItemFqy";
            this.toolStripMenuItemFqy.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuItemFqy.Text = "废气标定";
            this.toolStripMenuItemFqy.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItemLlj
            // 
            this.toolStripMenuItemLlj.Name = "toolStripMenuItemLlj";
            this.toolStripMenuItemLlj.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuItemLlj.Text = "流量计标定";
            this.toolStripMenuItemLlj.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // ToolStripMenuItemYdj
            // 
            this.ToolStripMenuItemYdj.Name = "ToolStripMenuItemYdj";
            this.ToolStripMenuItemYdj.Size = new System.Drawing.Size(136, 22);
            this.ToolStripMenuItemYdj.Text = "烟度计标定";
            // 
            // panelMain
            // 
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 25);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(880, 476);
            this.panelMain.TabIndex = 15;
            // 
            // demarcate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 501);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "demarcate";
            this.Text = "设备核准";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.demarcate_FormClosing);
            this.Load += new System.EventHandler(this.demarcate_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFqy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLlj;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemYdj;
        private System.Windows.Forms.Panel panelMain;
    }
}