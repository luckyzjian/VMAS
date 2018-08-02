using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;


namespace Detect
{
    public partial class resultDisplay : Form
    {
        public resultDisplay()
        {
            InitializeComponent();
        }

        private void resultDisplay_Load(object sender, EventArgs e)
        {
           
        }
        public void init_panel(string jcff, string jcbh, SYS.Model.BJCLXXB bjcl)
        {
            switch (jcff)
            {
                case "加载减速法":
                    jzjs_result_panel childjzjspanel=new jzjs_result_panel();
                    childjzjspanel.TopLevel=false;
                    childjzjspanel.Dock=System.Windows.Forms.DockStyle.Fill;
                    childjzjspanel.FormBorderStyle=FormBorderStyle.None;
                    this.panel_result.Controls.Clear();
                    this.panel_result.Controls.Add(childjzjspanel);
                    childjzjspanel.Show();
                    childjzjspanel.display_jzjs(jcbh,bjcl);
                    break;
                case "自由加速法":
                    zyjs_result_panel childzyjspanel = new zyjs_result_panel();
                    childzyjspanel.TopLevel = false;
                    childzyjspanel.Dock = System.Windows.Forms.DockStyle.Fill;
                    childzyjspanel.FormBorderStyle = FormBorderStyle.None;
                    this.panel_result.Controls.Clear();
                    this.panel_result.Controls.Add(childzyjspanel);
                    childzyjspanel.Show();
                    childzyjspanel.display_zyjs(jcbh, bjcl);
                    break;
                case "双怠速法":
                    sds_result_panel childsdspanel = new sds_result_panel();
                    childsdspanel.TopLevel = false;
                    childsdspanel.Dock = System.Windows.Forms.DockStyle.Fill;
                    childsdspanel.FormBorderStyle = FormBorderStyle.None;
                    this.panel_result.Controls.Clear();
                    this.panel_result.Controls.Add(childsdspanel);
                    childsdspanel.Show();
                    childsdspanel.display_sds(jcbh, bjcl);
                    break;
                case "简易瞬态工况法":
                    vmas_result_panel childvmaspanel = new vmas_result_panel();
                    childvmaspanel.TopLevel = false;
                    childvmaspanel.Dock = System.Windows.Forms.DockStyle.Fill;
                    childvmaspanel.FormBorderStyle = FormBorderStyle.None;
                    this.panel_result.Controls.Clear();
                    this.panel_result.Controls.Add(childvmaspanel);
                    childvmaspanel.Show();
                    childvmaspanel.display_vmas(jcbh, bjcl);
                    break;
                case "简易瞬态工况法after":
                    vmas_resultafter_panel childvmasafterpanel = new vmas_resultafter_panel();
                    childvmasafterpanel.TopLevel = false;
                    childvmasafterpanel.Dock = System.Windows.Forms.DockStyle.Fill;
                    childvmasafterpanel.FormBorderStyle = FormBorderStyle.None;
                    this.panel_result.Controls.Clear();
                    this.panel_result.Controls.Add(childvmasafterpanel);
                    childvmasafterpanel.Show();
                    childvmasafterpanel.display_vmas(jcbh, bjcl);
                    break;
                default:
                    break;
            }
        }
    }
}
