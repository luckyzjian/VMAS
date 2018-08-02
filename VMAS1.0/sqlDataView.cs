using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SYS_DAL;
using SYS.Model;
//using Microsoft.Office.Core;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;
//using Word = Microsoft.Office.Interop.Word;

namespace VMAS1._0
{
    public partial class sqlDataView : Form
    {
        public string[] selectID = new string[1024];
        BJCLXX bjclxx = new BJCLXX();
        BJCLXXB bjcl = new BJCLXXB();
        VMASdal vmasdal = new VMASdal();
        JZJS jzjs = new JZJS();
        JZJSdal jzjsdal = new JZJSdal();
        SDSdal sdsdal = new SDSdal();
        Zyjsdal zyjsdal = new Zyjsdal();
        private string jcffSelected;
        public sqlDataView()
        {
            InitializeComponent();
        }

        private void sqlDataView_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“zJ_VMASDataSet.BJCLXXB”中。您可以根据需要移动或删除它。
            //this.bJCLXXBTableAdapter.Fill(this.zJ_VMASDataSet.BJCLXXB);
            init_jcx();
            dateStartDate.Text = DateTime.Now.Date.ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0];
            dateEndDate.Text = DateTime.Now.Date.ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0];
        }
        #region 初始化检测线选项
        private void init_jcx()
        {
            DataTable dt =mainPanelForm.jcxxx.getAllJiancexian();
            //DataTable dt = bjclxx.getAllCarWait();
            DataRow dr = null;
            if (dt != null)
            {
                comboBox_detectLine.Items.Add("所有检测线");
                foreach (DataRow dR in dt.Rows)
                {
                    comboBox_detectLine.Items.Add(dR["JCXMC"].ToString());
                }
                comboBox_detectLine.SelectedIndex = 0;
            }
            
        }
        #endregion
        /*
        #region 显示加载减速报表
        private void display_jzjs()
        {
            //this.BJCLXXBTableAdapter.Fill(this.HBXDataSet1.BJCLXXB);
            jzjs = jzjsdal.Get_JZJS(selectID[0]);
            reportViewer1.LocalReport.ReleaseSandboxAppDomain();
            reportViewer1.LocalReport.Dispose();
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "VMAS1._0.ReportJzjs.rdlc";
            SqlConnection myConn = new SqlConnection("Data Source=LUCKYZJIAN-PC\\SQLEXPRESS;Initial Catalog=ZJ_VMAS;User ID=sa;Password=123");
            DataSet myds = new DataSet();
            DataSet mydy = new DataSet();
            SqlCommand cmd = myConn.CreateCommand();
            SqlParameter[] spr ={
                                   new SqlParameter("@jcclph",bjcl.JCCLPH)
                               };
            cmd.CommandText = "select * from BJCLXXB where jcclph=@jcclph";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddRange(spr);
            SqlDataAdapter myda = new SqlDataAdapter(cmd);
            myConn.Open();
            myda.Fill(myds);
            myConn.Close();
            reportViewer1.Visible = true;
            try
            {
                ReportParameter[] rptpara =
            {
                new ReportParameter("rptParaB", "测试报表参数"),
                new ReportParameter("rptParaA",jzjs.JCBH),
                new ReportParameter("ReportParameter_sbhzh", "核准设备号"),
                new ReportParameter("ReportParameter_cgjxh","xc_13a"),
                new ReportParameter("ReportParameter_cgjscs", "成都新成汽车检测设备有限公司"),
                new ReportParameter("ReportParameter_btgxh","flb_100"),
                new ReportParameter("ReportParameter_btgscs", "广州市福立分析仪器有限公司"),
                new ReportParameter("ReportParameter_fdjzsjxh","flb_100"),
                new ReportParameter("ReportParameter_fdjzsjscs", "广州市福立分析仪器有限公司"),
                new ReportParameter("ReportParameter_hjzxh","flb_100"),
                new ReportParameter("ReportParameter_hjzscs", "广州市福立分析仪器有限公司"),
                new ReportParameter("ReportParameter_pcxh","IPC-810E"),
                new ReportParameter("ReportParameter_pcscs", "广州研祥"),
                new ReportParameter("ReportParameter_wd",jzjs.WD),
                new ReportParameter("ReportParameter_sd", jzjs.SD),
                new ReportParameter("ReportParameter_dqy",jzjs.DQY),
                new ReportParameter("ReportParameter_hk", jzjs.HK),
                new ReportParameter("ReportParameter_nk",jzjs.NK),
                new ReportParameter("ReportParameter_ek",jzjs.EK),
                new ReportParameter("ReportParameter_edgl",bjcl.FDJEDGL),
                new ReportParameter("ReportParameter_lbgl", jzjs.HP),
                new ReportParameter("ReportParameter_edzs",bjcl.FDJEDZS),
                new ReportParameter("ReportParameter_lbzs", jzjs.MAXLBZS),
                new ReportParameter("ReportParameter_hkxz", jzjs.GXXZ),
                new ReportParameter("ReportParameter_nkxz",jzjs.GXXZ),
                new ReportParameter("ReportParameter_ekxz", jzjs.GXXZ),
                new ReportParameter("ReportParameter_glxz",jzjs.GLXZ),
                new ReportParameter("ReportParameter_zsxz",bjcl.FDJEDZS),
                new ReportParameter("ReportParameter_ydjg",jzjs.PDJG),
                new ReportParameter("ReportParameter_gljg",jzjs.GLPD),
                new ReportParameter("ReportParameter_zsjg", jzjs.ZSPD),
                new ReportParameter("ReportParameter_cj", jzjs.CJ),
                new ReportParameter("ReportParameter_jcy","周建"),
                new ReportParameter("ReportParameter_shy", "王茂林"),
                new ReportParameter("ReportParameter_pzr","余姜锋"),
                new ReportParameter("ReportParameter_zcy","胡泽平")
            };
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.SetParameters(rptpara);
            }
            catch
            {
                throw;
            }
            ReportDataSource rds = new ReportDataSource("DataSet2", myds.Tables[0]);
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.RefreshReport();
        }
        #endregion
        #region 显示双怠速法报表
        private void display_sds()
        {
            SDS sdsbjcl = new SDS();
            sdsbjcl = sdsdal.Get_SDS(selectID[0]);
            reportViewer1.LocalReport.ReleaseSandboxAppDomain();
            reportViewer1.LocalReport.Dispose();
            //this.reportViewer1.LocalReport.ReportEmbeddedResource = "Detect.ReportVmas.rdlc";
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "VMAS1._0.ReportSds.rdlc";
            SqlConnection myConn = new SqlConnection("Data Source=LUCKYZJIAN-PC\\SQLEXPRESS;Initial Catalog=ZJ_VMAS;User ID=sa;Password=123");
            DataSet myds = new DataSet();
            DataSet mydy = new DataSet();
            SqlCommand cmd = myConn.CreateCommand();
            SqlParameter[] spr ={
                                   new SqlParameter("@jcclph",bjcl.JCCLPH)
                               };
            cmd.CommandText = "select * from BJCLXXB where jcclph=@jcclph";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddRange(spr);
            SqlDataAdapter myda = new SqlDataAdapter(cmd);
            myConn.Open();
            myda.Fill(myds);
            myConn.Close();
            reportViewer1.Visible = true;
            reportViewer1.LocalReport.DataSources.Clear();
            //reportViewer1.LocalReport.SetParameters(rptpara);
            ReportDataSource rds = new ReportDataSource("DataSet2", myds.Tables[0]);
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.RefreshReport();
        }
        #endregion
        #region 显示简易瞬态工况法报表
        private void display_vmas()
        {
            VMAS vmasbjcl = new VMAS();
            vmasbjcl = vmasdal.Get_VMAS(selectID[0]);
            reportViewer1.LocalReport.ReleaseSandboxAppDomain();
            reportViewer1.LocalReport.Dispose();
            //this.reportViewer1.LocalReport.ReportEmbeddedResource = "Detect.ReportVmas.rdlc";
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "VMAS1._0.ReportVmas.rdlc";
            SqlConnection myConn = new SqlConnection("Data Source=LUCKYZJIAN-PC\\SQLEXPRESS;Initial Catalog=ZJ_VMAS;User ID=sa;Password=123");
            DataSet myds = new DataSet();
            DataSet mydy = new DataSet();
            SqlCommand cmd = myConn.CreateCommand();
            SqlParameter[] spr ={
                                   new SqlParameter("@jcclph",bjcl.JCCLPH)
                               };
            cmd.CommandText = "select * from BJCLXXB where jcclph=@jcclph";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddRange(spr);
            SqlDataAdapter myda = new SqlDataAdapter(cmd);
            myConn.Open();
            myda.Fill(myds);
            myConn.Close();
            reportViewer1.Visible = true;
            
            reportViewer1.LocalReport.DataSources.Clear();
            //reportViewer1.LocalReport.SetParameters(rptpara);
            ReportDataSource rds = new ReportDataSource("DataSet2", myds.Tables[0]);
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.RefreshReport();
        }
        #endregion
        #region 显示自由加速法报表
        private void display_zyjs()
        {
            Zyjs_Btg zyjsbjcl = new Zyjs_Btg();
            zyjsbjcl = zyjsdal.Get_Zyjs(selectID[0]);
            reportViewer1.LocalReport.ReleaseSandboxAppDomain();
            reportViewer1.LocalReport.Dispose();
            //this.reportViewer1.LocalReport.ReportEmbeddedResource = "Detect.ReportVmas.rdlc";
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "VMAS1._0.ReportZyjs.rdlc";
            SqlConnection myConn = new SqlConnection("Data Source=LUCKYZJIAN-PC\\SQLEXPRESS;Initial Catalog=ZJ_VMAS;User ID=sa;Password=123");
            DataSet myds = new DataSet();
            DataSet mydy = new DataSet();
            SqlCommand cmd = myConn.CreateCommand();
            SqlParameter[] spr ={
                                   new SqlParameter("@jcclph",bjcl.JCCLPH)
                               };
            cmd.CommandText = "select * from BJCLXXB where jcclph=@jcclph";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddRange(spr);
            SqlDataAdapter myda = new SqlDataAdapter(cmd);
            myConn.Open();
            myda.Fill(myds);
            myConn.Close();
            reportViewer1.Visible = true;
            try
            {
                ReportParameter[] rptpara =
            {
                new ReportParameter("rptParaB", "沈阳大路机动车检测站"),
                new ReportParameter("ReportParameter_btgxh", "flb_100"),
                new ReportParameter("ReportParameter_wd",zyjsbjcl.WD),
                new ReportParameter("ReportParameter_sd", zyjsbjcl.SD),
                new ReportParameter("ReportParameter_dqy",zyjsbjcl.DQY),
                new ReportParameter("ReportParameter_one", zyjsbjcl.DYCCLZ),
                new ReportParameter("ReportParameter_two",zyjsbjcl.DECCLZ),
                new ReportParameter("ReportParameter_three",zyjsbjcl.DSCCLZ),
                new ReportParameter("ReportParameter_average",zyjsbjcl.PJZ),
                new ReportParameter("ReportParameter_xz",zyjsbjcl.BTGXZ),
                new ReportParameter("ReportParameter_dszs"," "),
                new ReportParameter("ReportParameter_cj",zyjsbjcl.PDJG),
                new ReportParameter("ReportParameter_jcy","周建"),
                new ReportParameter("ReportParameter_shy", "王茂林"),
                new ReportParameter("ReportParameter_pzr","余姜锋")
            };
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.SetParameters(rptpara);
            }
            catch
            {
                throw;
            }
            ReportDataSource rds = new ReportDataSource("DataSet2", myds.Tables[0]);
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.RefreshReport();
        }
        #endregion
         * */
        private void simpleButtonCheck_Click(object sender, EventArgs e)
        {
            string jcxSql="";
            string sdsjcffSql = "";
            string vmasjcffSql = "";
            string zyjsjcffSql = "";
            string jzjsjcffSql = "";
            string clphSql="";
            string startDate = dateStartDate.Text+" 0:0:0" ;
            string endDate = dateEndDate.Text + " 23:59:59";
            string jiancexianName = comboBox_detectLine.Text;
            DataTable dt_wait = new DataTable();
            DataTable dt = null;
            if (jiancexianName != "所有检测线")
            {
                jiancexianName = mainPanelForm.jcxxx.Get_jcxbh_by_jcxmc(jiancexianName);
            }
            else
            {
                jiancexianName = "%";
            }
            jcxSql = "jcbh like '%-" + jiancexianName + "-%'";
            if (textBoxPlateNumber.Text.Trim() != "")
            {
                clphSql = " and jcclph like '%" + textBoxPlateNumber.Text.Trim() + "%'";
            }
            sdsjcffSql = "select jcbh,jcclph,jcrq from SDS where " + jcxSql + clphSql + " and  convert(datetime,jcrq,20)>convert(datetime,'" + startDate + "',20)" + " and convert(datetime,jcrq,20)<convert(datetime,'" + endDate + "',20)";
            vmasjcffSql = "select jcbh,jcclph,jcrq from VMAS where " + jcxSql + clphSql + " and  convert(datetime,jcrq,20)>convert(datetime,'" + startDate + "',20)" + " and convert(datetime,jcrq,20)<convert(datetime,'" + endDate + "',20)";
            zyjsjcffSql = "select jcbh,jcclph,jcrq from zyjs_btg where " + jcxSql + clphSql + " and  convert(datetime,jcrq,20)>convert(datetime,'" + startDate + "',20)" + " and convert(datetime,jcrq,20)<convert(datetime,'" + endDate + "',20)";
            jzjsjcffSql = "select jcbh,jcclph,jcrq from JZJS where " + jcxSql + clphSql + " and  convert(datetime,jcrq,20)>convert(datetime,'" + startDate + "',20)" + " and convert(datetime,jcrq,20)<convert(datetime,'" + endDate + "',20)";
            if (comboBoxJcff.Text.Trim()== "双怠速法")
            {
                dt = mainPanelForm.jcxxx.requerydata(sdsjcffSql);
                dt_wait = new DataTable();
                dt_wait.Columns.Add("检测编号");
                dt_wait.Columns.Add("车牌号");
                dt_wait.Columns.Add("检测日期");
                dt_wait.Columns.Add("检测方法");
                DataRow dr = null;
                if (dt != null)
                {
                    foreach (DataRow dR in dt.Rows)
                    {
                        dr = dt_wait.NewRow();
                        dr["检测编号"] = dR["JCBH"].ToString();
                        dr["车牌号"] = dR["JCCLPH"].ToString();
                        dr["检测日期"] = dR["JCRQ"].ToString();
                        dr["检测方法"] = "双怠速法";
                        dt_wait.Rows.Add(dr);
                    }
                }
            }
            else if (comboBoxJcff.Text.Trim() == "简易瞬态工况法")
            {
                dt = mainPanelForm.jcxxx.requerydata(vmasjcffSql);
                dt_wait = new DataTable();
                dt_wait.Columns.Add("检测编号");
                dt_wait.Columns.Add("车牌号");
                dt_wait.Columns.Add("检测日期");
                dt_wait.Columns.Add("检测方法");
                DataRow dr = null;
                if (dt != null)
                {
                    foreach (DataRow dR in dt.Rows)
                    {
                        dr = dt_wait.NewRow();
                        dr["检测编号"] = dR["JCBH"].ToString();
                        dr["车牌号"] = dR["JCCLPH"].ToString();
                        dr["检测日期"] = dR["JCRQ"].ToString();
                        dr["检测方法"] = "简易瞬态工况法";
                        dt_wait.Rows.Add(dr);
                    }
                }
            }
            else if (comboBoxJcff.Text.Trim() == "加载减速法")
            {
                dt = mainPanelForm.jcxxx.requerydata(jzjsjcffSql);
                dt_wait = new DataTable();
                dt_wait.Columns.Add("检测编号");
                dt_wait.Columns.Add("车牌号");
                dt_wait.Columns.Add("检测日期");
                dt_wait.Columns.Add("检测方法");
                DataRow dr = null;
                if (dt != null)
                {
                    foreach (DataRow dR in dt.Rows)
                    {
                        dr = dt_wait.NewRow();
                        dr["检测编号"] = dR["JCBH"].ToString();
                        dr["车牌号"] = dR["JCCLPH"].ToString();
                        dr["检测日期"] = dR["JCRQ"].ToString();
                        dr["检测方法"] = "加载减速法";
                        dt_wait.Rows.Add(dr);
                    }
                }
            }
            else if (comboBoxJcff.Text.Trim() == "自由加速法")
            {
                dt = mainPanelForm.jcxxx.requerydata(zyjsjcffSql);
                dt_wait = new DataTable();
                dt_wait.Columns.Add("检测编号");
                dt_wait.Columns.Add("车牌号");
                dt_wait.Columns.Add("检测日期");
                dt_wait.Columns.Add("检测方法");
                DataRow dr = null;
                if (dt != null)
                {
                    foreach (DataRow dR in dt.Rows)
                    {
                        dr = dt_wait.NewRow();
                        dr["检测编号"] = dR["JCBH"].ToString();
                        dr["车牌号"] = dR["JCCLPH"].ToString();
                        dr["检测日期"] = dR["JCRQ"].ToString();
                        dr["检测方法"] = "自由加速法";
                        dt_wait.Rows.Add(dr);
                    }
                }
            }
            else
            {
                dt = mainPanelForm.jcxxx.requerydata(sdsjcffSql);
                dt_wait = new DataTable();
                dt_wait.Columns.Add("检测编号");
                dt_wait.Columns.Add("车牌号");
                dt_wait.Columns.Add("检测日期");
                dt_wait.Columns.Add("检测方法");
                DataRow dr = null;
                if (dt != null)
                {
                    foreach (DataRow dR in dt.Rows)
                    {
                        dr = dt_wait.NewRow();
                        dr["检测编号"] = dR["JCBH"].ToString();
                        dr["车牌号"] = dR["JCCLPH"].ToString();
                        dr["检测日期"] = dR["JCRQ"].ToString();
                        dr["检测方法"] = "双怠速法";
                        dt_wait.Rows.Add(dr);
                    }
                }
                dt = mainPanelForm.jcxxx.requerydata(vmasjcffSql);
                if (dt != null)
                {
                    foreach (DataRow dR in dt.Rows)
                    {
                        dr = dt_wait.NewRow();
                        dr["检测编号"] = dR["JCBH"].ToString();
                        dr["车牌号"] = dR["JCCLPH"].ToString();
                        dr["检测日期"] = dR["JCRQ"].ToString();
                        dr["检测方法"] = "简易瞬态工况法";
                        dt_wait.Rows.Add(dr);
                    }
                }
                dt = mainPanelForm.jcxxx.requerydata(jzjsjcffSql);
                if (dt != null)
                {
                    foreach (DataRow dR in dt.Rows)
                    {
                        dr = dt_wait.NewRow();
                        dr["检测编号"] = dR["JCBH"].ToString();
                        dr["车牌号"] = dR["JCCLPH"].ToString();
                        dr["检测日期"] = dR["JCRQ"].ToString();
                        dr["检测方法"] = "加载减速法";
                        dt_wait.Rows.Add(dr);
                    }
                }
                dt = mainPanelForm.jcxxx.requerydata(zyjsjcffSql);
                if (dt != null)
                {
                    foreach (DataRow dR in dt.Rows)
                    {
                        dr = dt_wait.NewRow();
                        dr["检测编号"] = dR["JCBH"].ToString();
                        dr["车牌号"] = dR["JCCLPH"].ToString();
                        dr["检测日期"] = dR["JCRQ"].ToString();
                        dr["检测方法"] = "自由加速法";
                        dt_wait.Rows.Add(dr);
                    }
                }
            }
            dataGridView1.DataSource = dt_wait;
        }
        #region 卸载面板中所有的reportview
        private void GetReportviewinF(Form temp)     //对panel进行遍历的函数
        {
            foreach (Control tempcon in temp.Controls)
            {
                if (tempcon is ReportViewer)
                    (tempcon as ReportViewer).LocalReport.ReleaseSandboxAppDomain();
                else if (tempcon is Panel)
                    this.GetReportviewinP((Panel)tempcon);
                else if (tempcon is GroupBox)
                    this.GetReportviewinG((GroupBox)tempcon);
                else if (tempcon is Form)
                    this.GetReportviewinF((Form)tempcon);
            }
        }
        private void GetReportviewinP(Panel temp)     //对panel进行遍历的函数
        {
            foreach (Control tempcon in temp.Controls)
            {
                if (tempcon is ReportViewer)
                    (tempcon as ReportViewer).LocalReport.ReleaseSandboxAppDomain();
                else if (tempcon is Panel)
                    this.GetReportviewinP((Panel)tempcon);
                else if (tempcon is GroupBox)
                    this.GetReportviewinG((GroupBox)tempcon);
                else if (tempcon is Form)
                    this.GetReportviewinF((Form)tempcon);
            }
        }

        private void GetReportviewinG(GroupBox temp)   //对GroupBox遍历
        {
            foreach (Control tempcon in temp.Controls)
            {
                if (tempcon is ReportViewer)
                    (tempcon as ReportViewer).LocalReport.ReleaseSandboxAppDomain();
                else if (tempcon is Panel)
                    this.GetReportviewinP((Panel)tempcon);
                else if (tempcon is GroupBox)
                    this.GetReportviewinG((GroupBox)tempcon);
                else if (tempcon is Form)
                    this.GetReportviewinF((Form)tempcon);

            }
        }
        #endregion
        private void sqlDataView_Closing(object sender, FormClosingEventArgs e)
        {
            GetReportviewinP(panel_result);//卸载reportview
        }
        
        private void dataGridView1_selectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (dataGridView1.SelectedRows.Count == 1)
                {
                    ToolStripMenuItem_Check.Enabled = true;
                    toolStripMenuItem_Print.Enabled = true;
                    selectID = new string[1024];
                    for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                    {
                        selectID[i] = dataGridView1.SelectedRows[i].Cells["检测编号"].Value.ToString();
                    }
                    bjcl = bjclxx.GetModel_by_jcclph(dataGridView1.SelectedRows[0].Cells["车牌号"].Value.ToString());
                    jcffSelected = dataGridView1.SelectedRows[0].Cells["检测方法"].Value.ToString();
                }
                else if (dataGridView1.SelectedRows.Count > 1)
                {
                    ToolStripMenuItem_Check.Enabled = false;
                    toolStripMenuItem_Print.Enabled = false;
                    selectID = new string[1024];
                    for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                    {
                        selectID[i] = dataGridView1.SelectedRows[i].Cells["检测编号"].Value.ToString();
                    }
                }
                else
                {
                    ToolStripMenuItem_Check.Enabled = false;
                    toolStripMenuItem_Print.Enabled = false;
                    selectID = new string[1024];
                    for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                    {
                        selectID[i] = dataGridView1.SelectedRows[i].Cells["检测编号"].Value.ToString();
                    }
                }
            }
            else
            {
                selectID = new string[1024];
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    selectID[i] = dataGridView1.SelectedRows[i].Cells["检测编号"].Value.ToString();
                }
            }
        }

        private void ToolStripMenuItem_Check_Click(object sender, EventArgs e)
        {
            if (jcffSelected == "简易瞬态工况法")
            {
                string Cllx="";
                //display_vmas();
                GetReportviewinP(panel_result);
                if (bjcl.HDZK <= 6 && int.Parse(bjcl.ZDZZL) <= 2500 && (bjcl.CLLX.IndexOf("客") > -1 || bjcl.CLLX.IndexOf("轿") > -1))     //第一类轻型汽车
                    Cllx = "第一类轻型汽车";
                else if (int.Parse(bjcl.ZDZZL) <= 3500)       //第二类轻型汽车
                    Cllx = "第二类轻型汽车";
                else
                    Cllx = "重型汽车";              //重型汽车
                switch (Cllx)
                {
                    case "第一类轻型汽车":
                        if (DateTime.Compare(Convert.ToDateTime(bjcl.CCRQ), Convert.ToDateTime("2000-07-01")) <= 0)    //2000年7月1日前生产的第一类轻型汽车
                        {
                            vmas_result_panel childvmaspanel = new vmas_result_panel();
                            childvmaspanel.TopLevel = false;
                            childvmaspanel.Dock = System.Windows.Forms.DockStyle.Fill;
                            childvmaspanel.FormBorderStyle = FormBorderStyle.None;
                            this.panel_result.Controls.Clear();
                            this.panel_result.Controls.Add(childvmaspanel);
                            childvmaspanel.Show();
                            childvmaspanel.display_vmas(selectID[0], bjcl);

                        }
                        else     //2000年7月1日起生产的第一类轻型汽车
                        {
                            vmas_resultafter_panel childvmasafterpanel = new vmas_resultafter_panel();
                            childvmasafterpanel.TopLevel = false;
                            childvmasafterpanel.Dock = System.Windows.Forms.DockStyle.Fill;
                            childvmasafterpanel.FormBorderStyle = FormBorderStyle.None;
                            this.panel_result.Controls.Clear();
                            this.panel_result.Controls.Add(childvmasafterpanel);
                            childvmasafterpanel.Show();
                            childvmasafterpanel.display_vmas(selectID[0], bjcl);
                        }
                        break;
                    case "第二类轻型汽车":
                        if (DateTime.Compare(Convert.ToDateTime(bjcl.CCRQ), Convert.ToDateTime("2001-10-01")) < 0)    //2001年10月1日前生产的第二类轻型汽车
                        {
                            vmas_result_panel childvmaspanel = new vmas_result_panel();
                            childvmaspanel.TopLevel = false;
                            childvmaspanel.Dock = System.Windows.Forms.DockStyle.Fill;
                            childvmaspanel.FormBorderStyle = FormBorderStyle.None;
                            this.panel_result.Controls.Clear();
                            this.panel_result.Controls.Add(childvmaspanel);
                            childvmaspanel.Show();
                            childvmaspanel.display_vmas(selectID[0], bjcl);

                        }
                        else
                        {
                            vmas_resultafter_panel childvmasafterpanel = new vmas_resultafter_panel();
                            childvmasafterpanel.TopLevel = false;
                            childvmasafterpanel.Dock = System.Windows.Forms.DockStyle.Fill;
                            childvmasafterpanel.FormBorderStyle = FormBorderStyle.None;
                            this.panel_result.Controls.Clear();
                            this.panel_result.Controls.Add(childvmasafterpanel);
                            childvmasafterpanel.Show();
                            childvmasafterpanel.display_vmas(selectID[0], bjcl);
                        }
                        break;
                    default: break;
                }
                
            }
            else if (jcffSelected == "加载减速法")
            {
                //display_jzjs();
                GetReportviewinP(panel_result);
                jzjs_result_panel childjzjspanel=new jzjs_result_panel();
                childjzjspanel.TopLevel=false;
                childjzjspanel.Dock=System.Windows.Forms.DockStyle.Fill;
                childjzjspanel.FormBorderStyle=FormBorderStyle.None;
                this.panel_result.Controls.Clear();
                this.panel_result.Controls.Add(childjzjspanel);
                childjzjspanel.Show();
                childjzjspanel.display_jzjs(selectID[0],bjcl);

            }
            else if (jcffSelected == "双怠速法")
            {
                //display_sds();
                GetReportviewinP(panel_result);
                sds_result_panel childsdspanel = new sds_result_panel();
                childsdspanel.TopLevel = false;
                childsdspanel.Dock = System.Windows.Forms.DockStyle.Fill;
                childsdspanel.FormBorderStyle = FormBorderStyle.None;
                this.panel_result.Controls.Clear();
                this.panel_result.Controls.Add(childsdspanel);
                childsdspanel.Show();
                childsdspanel.display_sds(selectID[0], bjcl);
            }
            else if (jcffSelected == "自由加速法")
            {
                //display_zyjs();
                GetReportviewinP(panel_result);//预先卸载
                zyjs_result_panel childzyjspanel = new zyjs_result_panel();
                childzyjspanel.TopLevel = false;
                childzyjspanel.Dock = System.Windows.Forms.DockStyle.Fill;
                childzyjspanel.FormBorderStyle = FormBorderStyle.None;
                this.panel_result.Controls.Clear();
                this.panel_result.Controls.Add(childzyjspanel);
                childzyjspanel.Show();
                childzyjspanel.display_zyjs(selectID[0], bjcl);
            }
        }

        private void toolStripMenuItem_Print_Click(object sender, EventArgs e)
        {
                //display_jzjs();
            if (jcffSelected == "双怠速法" || jcffSelected == "自由加速法")
            {
                MessageBox.Show("对不起，该方法没有详细数据可查看。", "系统提示");
            }
            else
            {
                GetReportviewinP(panel_result);
                dataPerSec childdatapanel = new dataPerSec();
                childdatapanel.TopLevel = false;
                childdatapanel.Dock = System.Windows.Forms.DockStyle.Fill;
                childdatapanel.FormBorderStyle = FormBorderStyle.None;
                this.panel_result.Controls.Clear();
                this.panel_result.Controls.Add(childdatapanel);
                childdatapanel.Show();
                childdatapanel.init_datagridview(jcffSelected);
                childdatapanel.init_data(selectID[0], jcffSelected);
            }
           }
    }
}
