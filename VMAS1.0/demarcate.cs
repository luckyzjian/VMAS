using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SYS.Model;
using SYS_DAL;

namespace VMAS1._0
{
    public partial class demarcate : Form
    {
        public static SBXXB fqfxy = new SBXXB();
        public static SBXXB btgydj = new SBXXB();
        public static SBXXB llj = new SBXXB();
        public static SBXX sbxx = new SBXX();
        LljDemarcate newExhaustlljForm = null;
        exhaustForm newExhaustfqyForm = null;
        public demarcate()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (this.Contains(newExhaustfqyForm))
            {
                newExhaustfqyForm.Show();
                return;
            }
            else
            {
                newExhaustfqyForm = new exhaustForm(this);
                newExhaustfqyForm.TopLevel = false;
                newExhaustfqyForm.Dock = System.Windows.Forms.DockStyle.Fill;
                newExhaustfqyForm.FormBorderStyle = FormBorderStyle.None;
                //this.panelMain.Controls.Clear();
                this.panelMain.Controls.Add(newExhaustfqyForm);
                newExhaustfqyForm.Show();
            }
        }
        #region 判断一个窗体是否已经存在，不存在就显示，存在就不显示
        /// <summary>
        /// 判断一个窗体是否已经存在，不存在就显示，存在就不显示
        /// </summary>
        /// <param name="formName">窗口类名</param>
        /// <returns></returns>
        public bool checkForm(string formName)
        {
            foreach (Form form in this.MdiChildren)
            {
                if (form.Name == formName)
                {
                    if (form.WindowState == FormWindowState.Minimized)
                    {
                        form.WindowState = FormWindowState.Normal;
                    }
                    form.Activate();
                    return true;
                }
            }
            return false;
        }
        #endregion

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (this.Contains(newExhaustlljForm))
            {
                newExhaustlljForm.Show();
                return;
            }
            else
            {
                newExhaustlljForm = new LljDemarcate(this);
                newExhaustlljForm.TopLevel = false;
                newExhaustlljForm.Dock = System.Windows.Forms.DockStyle.Fill;
                newExhaustlljForm.FormBorderStyle = FormBorderStyle.None;
                //this.panelMain.Controls.Clear();
                this.panelMain.Controls.Add(newExhaustlljForm);
                newExhaustlljForm.Show();
            }

        }

        private void demarcate_Load(object sender, EventArgs e)
        {
            Init_SB();
        }
        public void Init_SB()
        {
            fqfxy = sbxx.Get_sb_by_bh(mainPanelForm.jcxxxb.FQFXYBH);
            if (fqfxy.SBBH == 0)
            {
                toolStripMenuItemFqy.Enabled = false;
            }
            btgydj = sbxx.Get_sb_by_bh(mainPanelForm.jcxxxb.BTGYDJBH);
            if (btgydj.SBBH == 0)
            {
                ToolStripMenuItemYdj.Enabled = false;
            }
            llj = sbxx.Get_sb_by_bh(mainPanelForm.jcxxxb.LLJBH);
            if (llj.SBBH == 0)
            {
                toolStripMenuItemLlj.Enabled = false;
            }
        }

        private void demarcate_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Contains(newExhaustlljForm))
            {
                newExhaustlljForm.exit_form();
            }
            if (this.Contains(newExhaustfqyForm))
            {
                newExhaustfqyForm.exit_form();
            }
        }
        
    }
}
