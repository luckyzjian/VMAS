using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SYS;
using SYS_DAL;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;

namespace VMAS1._0
{
    public partial class jzjs_result_panel : Form
    {
        SYS_DAL.JZJSdal jzjsdal = new SYS_DAL.JZJSdal();
        SYS_DAL.JZJS_jcxxdal jzjs_jcxxdal = new JZJS_jcxxdal();
        public jzjs_result_panel()
        {
            InitializeComponent();
        }

        private void jzjs_result_panel_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“ZJ_VMASDataSet.BJCLXXB”中。您可以根据需要移动或删除它。
            //this.BJCLXXBTableAdapter.Fill(this.ZJ_VMASDataSet.BJCLXXB);

        }
        public void display_jzjs(string jzjs_jcbh,SYS.Model.BJCLXXB bjcl)
        {
            SYS.Model.JZJS jzjs =jzjsdal.Get_JZJS(jzjs_jcbh);
            SYS.Model.JZJS_JCXXB jzjs_jcxxb = jzjs_jcxxdal.Get_JZJS_jcxx(jzjs_jcbh);
            reportViewer1.LocalReport.ReleaseSandboxAppDomain();
            reportViewer1.LocalReport.Dispose();
            DataSet myds = new DataSet();
            DataSet mydy = new DataSet();
            SqlParameter[] spr ={
                                   new SqlParameter("@jcclph",bjcl.JCCLPH)
                               };
            string sql = "select * from BJCLXXB where jcclph=@jcclph";
            myds = DBHelperSQL.GetDataSet(sql, spr);
            reportViewer1.Visible = true;
            try
            {
                ReportParameter[] rptpara =
            {
                new ReportParameter("rptParaB", jzjs_jcxxb.JCZMC),
                new ReportParameter("rptParaA",jzjs.JCBH),
                new ReportParameter("ReportParameterjcrq",jzjs.JCRQ),
                new ReportParameter("ReportParameter_sbhzh", jzjs_jcxxb.SBHZH),
                new ReportParameter("ReportParameter_cgjxh",jzjs_jcxxb.CGJXH),
                new ReportParameter("ReportParameter_cgjscs", jzjs_jcxxb.CGJZZC),
                new ReportParameter("ReportParameter_btgxh",jzjs_jcxxb.YDJXH),
                new ReportParameter("ReportParameter_btgscs", jzjs_jcxxb.YDJZZC),
                new ReportParameter("ReportParameter_fdjzsjxh",jzjs_jcxxb.ZSJXH),
                new ReportParameter("ReportParameter_fdjzsjscs", jzjs_jcxxb.ZSJZZC),
                new ReportParameter("ReportParameter_hjzxh",jzjs_jcxxb.HJZXH),
                new ReportParameter("ReportParameter_hjzscs", jzjs_jcxxb.HJZZZC),
                new ReportParameter("ReportParameter_pcxh",jzjs_jcxxb.PCXH),
                new ReportParameter("ReportParameter_pcscs",jzjs_jcxxb.PCZZC),
                new ReportParameter("ReportParameter_wd",jzjs.WD),
                new ReportParameter("ReportParameter_sd", jzjs.SD),
                new ReportParameter("ReportParameter_dqy",jzjs.DQY),
                new ReportParameter("ReportParameter_hk", jzjs.HK),
                new ReportParameter("ReportParameter_nk",jzjs.NK),
                new ReportParameter("ReportParameter_ek",jzjs.EK),
                new ReportParameter("ReportParameter_edgl",bjcl.FDJEDGL),
                new ReportParameter("ReportParameter_lbgl", jzjs.MAXLBGL),
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
                new ReportParameter("ReportParameter_jcy",jzjs_jcxxb.JCY),
                new ReportParameter("ReportParameter_shy", jzjs_jcxxb.SHY),
                new ReportParameter("ReportParameter_pzr",jzjs_jcxxb.PZR),
                new ReportParameter("ReportParameter_zcy",jzjs_jcxxb.ZCY)
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
            //reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            //reportViewer1.ZoomMode = ZoomMode.Percent;
            //reportViewer1.ZoomPercent = 25;
            reportViewer1.RefreshReport();
        }

        private void jzjs_result_panelFormClosing(object sender, FormClosingEventArgs e)
        {
            reportViewer1.LocalReport.ReleaseSandboxAppDomain();
        }
    }
}
