using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using System.Data.Sql;
using SYS.Model;
using SYS_DAL;

namespace VMAS1._0
{
    public partial class huanBaoConfig : Form
    {
        DataTable dt_wait = null;
        public static YGB ygb = new YGB();
        public static YGBdal ygbdal = new YGBdal();
        public static JCXXX jcxxx = new JCXXX();
        public static JCXXXB jcxxxb = new JCXXXB();   
        public huanBaoConfig()
        {
            InitializeComponent();
        }

        private void systemConfig_Load(object sender, EventArgs e)
        {
            init_sb();
            init_jz();
            
        }
        public void init_sb()
        {
            int i = 0;
            dt_wait = new DataTable();
            dt_wait.Columns.Add("序号");
            dt_wait.Columns.Add("名称");
            dt_wait.Columns.Add("认证编号");
            dt_wait.Columns.Add("许可证起止日期");
            DataTable dt = jcxxx.getAllJiancexian();
            //DataTable dt = bjclxx.getAllCarWait();
            DataRow dr = null;
            if (dt != null)
            {
                foreach (DataRow dR in dt.Rows)
                {
                    i++;
                    dr = dt_wait.NewRow();
                    dr["序号"] = i.ToString();
                    dr["名称"] = dR["JCXMC"].ToString();
                    dr["认证编号"] = dR["RZBH"].ToString();
                    dr["许可证起止日期"] = mainPanelForm.sbstarttime + "-" + mainPanelForm.sbendtime;

                    dt_wait.Rows.Add(dr);
                }
            }
            dt = ygbdal.Get_AllUser();
            if (dt != null)
            {
                foreach (DataRow dR in dt.Rows)
                {
                    i++;
                    dr = dt_wait.NewRow();
                    dr["序号"] = i.ToString();
                    dr["名称"] = dR["YGXM"].ToString();
                    dr["认证编号"] = dR["YGSFZ"].ToString();
                    dr["许可证起止日期"] = mainPanelForm.ygstarttime + "-" + mainPanelForm.ygendtime;

                    dt_wait.Rows.Add(dr);
                }
            }
            dataGridView1.DataSource = dt_wait;
 
        }
        public void init_jz()
        {
            int i = 0;
            dt_wait = new DataTable();
            dt_wait.Columns.Add("序号");
            dt_wait.Columns.Add("名称");
            dt_wait.Columns.Add("认证编号");
            dt_wait.Columns.Add("许可证起止日期");
            
            dataGridView2.DataSource = dt_wait;

        }

        
    }
}
