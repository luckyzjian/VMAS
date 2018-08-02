namespace Detect
{
    partial class resultDisplay
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
            this.BJCLXXBBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ZJ_VMASDataSet = new Detect.ZJ_VMASDataSet();
            this.BJCLXXBTableAdapter = new Detect.ZJ_VMASDataSetTableAdapters.BJCLXXBTableAdapter();
            this.panel_result = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.BJCLXXBBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZJ_VMASDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // BJCLXXBBindingSource
            // 
            this.BJCLXXBBindingSource.DataMember = "BJCLXXB";
            this.BJCLXXBBindingSource.DataSource = this.ZJ_VMASDataSet;
            // 
            // ZJ_VMASDataSet
            // 
            this.ZJ_VMASDataSet.DataSetName = "ZJ_VMASDataSet";
            this.ZJ_VMASDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // BJCLXXBTableAdapter
            // 
            this.BJCLXXBTableAdapter.ClearBeforeFill = true;
            // 
            // panel_result
            // 
            this.panel_result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_result.Location = new System.Drawing.Point(0, 0);
            this.panel_result.Name = "panel_result";
            this.panel_result.Size = new System.Drawing.Size(854, 760);
            this.panel_result.TabIndex = 0;
            // 
            // resultDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 760);
            this.Controls.Add(this.panel_result);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "resultDisplay";
            this.Text = "检测报告";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.resultDisplay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BJCLXXBBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZJ_VMASDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource BJCLXXBBindingSource;
        private ZJ_VMASDataSet ZJ_VMASDataSet;
        private ZJ_VMASDataSetTableAdapters.BJCLXXBTableAdapter BJCLXXBTableAdapter;
        private System.Windows.Forms.Panel panel_result;


    }
}