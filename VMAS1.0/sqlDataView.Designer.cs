namespace VMAS1._0
{
    partial class sqlDataView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(sqlDataView));
            this.bJCLXXBBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.zJ_VMASDataSet = new VMAS1._0.ZJ_VMASDataSet();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Wait_Car_menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_Check = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Print = new System.Windows.Forms.ToolStripMenuItem();
            this.panel4 = new System.Windows.Forms.Panel();
            this.simpleButtonCheck = new DevExpress.XtraEditors.SimpleButton();
            this.comboBox_detectLine = new System.Windows.Forms.ComboBox();
            this.label47 = new System.Windows.Forms.Label();
            this.dateEndDate = new DevExpress.XtraEditors.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.dateStartDate = new DevExpress.XtraEditors.DateEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxPlateNumber = new System.Windows.Forms.TextBox();
            this.label38 = new System.Windows.Forms.Label();
            this.comboBoxJcff = new System.Windows.Forms.ComboBox();
            this.label52 = new System.Windows.Forms.Label();
            this.bJCLXXBTableAdapter = new VMAS1._0.ZJ_VMASDataSetTableAdapters.BJCLXXBTableAdapter();
            this.panel_result = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.bJCLXXBBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zJ_VMASDataSet)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.Wait_Car_menu.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEndDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateStartDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateStartDate.Properties)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bJCLXXBBindingSource
            // 
            this.bJCLXXBBindingSource.DataMember = "BJCLXXB";
            this.bJCLXXBBindingSource.DataSource = this.zJ_VMASDataSet;
            // 
            // zJ_VMASDataSet
            // 
            this.zJ_VMASDataSet.DataSetName = "ZJ_VMASDataSet";
            this.zJ_VMASDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.CadetBlue;
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1068, 117);
            this.panel1.TabIndex = 1;
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
            this.label12.Text = "数据库查询";
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
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel2.Location = new System.Drawing.Point(0, 232);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(351, 427);
            this.panel2.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.Wait_Car_menu;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(351, 427);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_selectionChanged);
            // 
            // Wait_Car_menu
            // 
            this.Wait_Car_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Check,
            this.toolStripMenuItem_Print});
            this.Wait_Car_menu.Name = "Wait_Car_menu";
            this.Wait_Car_menu.Size = new System.Drawing.Size(173, 70);
            // 
            // ToolStripMenuItem_Check
            // 
            this.ToolStripMenuItem_Check.Name = "ToolStripMenuItem_Check";
            this.ToolStripMenuItem_Check.Size = new System.Drawing.Size(172, 22);
            this.ToolStripMenuItem_Check.Text = "查看该车报表";
            this.ToolStripMenuItem_Check.Click += new System.EventHandler(this.ToolStripMenuItem_Check_Click);
            // 
            // toolStripMenuItem_Print
            // 
            this.toolStripMenuItem_Print.Name = "toolStripMenuItem_Print";
            this.toolStripMenuItem_Print.Size = new System.Drawing.Size(172, 22);
            this.toolStripMenuItem_Print.Text = "查看该车详细数据";
            this.toolStripMenuItem_Print.Click += new System.EventHandler(this.toolStripMenuItem_Print_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel4.Controls.Add(this.simpleButtonCheck);
            this.panel4.Controls.Add(this.comboBox_detectLine);
            this.panel4.Controls.Add(this.label47);
            this.panel4.Controls.Add(this.dateEndDate);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.dateStartDate);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.textBoxPlateNumber);
            this.panel4.Controls.Add(this.label38);
            this.panel4.Controls.Add(this.comboBoxJcff);
            this.panel4.Controls.Add(this.label52);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 19);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(345, 90);
            this.panel4.TabIndex = 4;
            // 
            // simpleButtonCheck
            // 
            this.simpleButtonCheck.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.simpleButtonCheck.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.simpleButtonCheck.Location = new System.Drawing.Point(173, 59);
            this.simpleButtonCheck.Name = "simpleButtonCheck";
            this.simpleButtonCheck.Size = new System.Drawing.Size(164, 23);
            this.simpleButtonCheck.TabIndex = 102;
            this.simpleButtonCheck.Text = "查询";
            this.simpleButtonCheck.Click += new System.EventHandler(this.simpleButtonCheck_Click);
            // 
            // comboBox_detectLine
            // 
            this.comboBox_detectLine.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.comboBox_detectLine.AutoCompleteCustomSource.AddRange(new string[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6"});
            this.comboBox_detectLine.FormattingEnabled = true;
            this.comboBox_detectLine.Location = new System.Drawing.Point(64, 60);
            this.comboBox_detectLine.Name = "comboBox_detectLine";
            this.comboBox_detectLine.Size = new System.Drawing.Size(85, 25);
            this.comboBox_detectLine.TabIndex = 101;
            // 
            // label47
            // 
            this.label47.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(14, 63);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(47, 17);
            this.label47.TabIndex = 100;
            this.label47.Text = "检测线:";
            // 
            // dateEndDate
            // 
            this.dateEndDate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dateEndDate.EditValue = new System.DateTime(2013, 8, 5, 11, 19, 23, 955);
            this.dateEndDate.Location = new System.Drawing.Point(236, 33);
            this.dateEndDate.Name = "dateEndDate";
            this.dateEndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEndDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEndDate.Size = new System.Drawing.Size(101, 21);
            this.dateEndDate.TabIndex = 99;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(207, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 17);
            this.label1.TabIndex = 98;
            this.label1.Text = "至:";
            // 
            // dateStartDate
            // 
            this.dateStartDate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dateStartDate.EditValue = new System.DateTime(2013, 8, 5, 11, 19, 23, 955);
            this.dateStartDate.Location = new System.Drawing.Point(236, 6);
            this.dateStartDate.Name = "dateStartDate";
            this.dateStartDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateStartDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateStartDate.Size = new System.Drawing.Size(101, 21);
            this.dateStartDate.TabIndex = 97;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(171, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 17);
            this.label7.TabIndex = 96;
            this.label7.Text = "检测日期:";
            // 
            // textBoxPlateNumber
            // 
            this.textBoxPlateNumber.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.textBoxPlateNumber.Location = new System.Drawing.Point(64, 32);
            this.textBoxPlateNumber.Name = "textBoxPlateNumber";
            this.textBoxPlateNumber.Size = new System.Drawing.Size(85, 23);
            this.textBoxPlateNumber.TabIndex = 93;
            // 
            // label38
            // 
            this.label38.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(14, 36);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(47, 17);
            this.label38.TabIndex = 92;
            this.label38.Text = "车牌号:";
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
            this.comboBoxJcff.FormattingEnabled = true;
            this.comboBoxJcff.Items.AddRange(new object[] {
            "双怠速法",
            "简易瞬态工况法",
            "加载减速法",
            "自由加速法",
            "任意方法"});
            this.comboBoxJcff.Location = new System.Drawing.Point(64, 6);
            this.comboBoxJcff.Name = "comboBoxJcff";
            this.comboBoxJcff.Size = new System.Drawing.Size(85, 25);
            this.comboBoxJcff.TabIndex = 91;
            this.comboBoxJcff.Text = "任意方法";
            // 
            // label52
            // 
            this.label52.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(2, 9);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(59, 17);
            this.label52.TabIndex = 90;
            this.label52.Text = "检测方法:";
            // 
            // bJCLXXBTableAdapter
            // 
            this.bJCLXXBTableAdapter.ClearBeforeFill = true;
            // 
            // panel_result
            // 
            this.panel_result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_result.Location = new System.Drawing.Point(361, 124);
            this.panel_result.Name = "panel_result";
            this.panel_result.Size = new System.Drawing.Size(707, 535);
            this.panel_result.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(0, 117);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(351, 112);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询条件";
            // 
            // sqlDataView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 662);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel_result);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "sqlDataView";
            this.Text = "sqlDataView";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.sqlDataView_Closing);
            this.Load += new System.EventHandler(this.sqlDataView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bJCLXXBBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zJ_VMASDataSet)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.Wait_Car_menu.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEndDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateStartDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateStartDate.Properties)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox comboBoxJcff;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.TextBox textBoxPlateNumber;
        private System.Windows.Forms.Label label38;
        private DevExpress.XtraEditors.DateEdit dateEndDate;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.DateEdit dateStartDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox_detectLine;
        private System.Windows.Forms.Label label47;
        private DevExpress.XtraEditors.SimpleButton simpleButtonCheck;
        private System.Windows.Forms.ContextMenuStrip Wait_Car_menu;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Check;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Print;
        private ZJ_VMASDataSet zJ_VMASDataSet;
        private System.Windows.Forms.BindingSource bJCLXXBBindingSource;
        private ZJ_VMASDataSetTableAdapters.BJCLXXBTableAdapter bJCLXXBTableAdapter;
        private System.Windows.Forms.Panel panel_result;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}