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
    public partial class sds_result_panel : Form
    {
        SYS_DAL.SDS_jcxxdal sds_jcxxdal = new SDS_jcxxdal();
        SYS_DAL.SDSdal sdsdal = new SYS_DAL.SDSdal();
        public sds_result_panel()
        {
            InitializeComponent();
        }

        private void sds_result_panel_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“ZJ_VMASDataSet.BJCLXXB”中。您可以根据需要移动或删除它。
            //this.BJCLXXBTableAdapter.Fill(this.ZJ_VMASDataSet.BJCLXXB);

        }
        public void display_sds(string sds_jcbh, SYS.Model.BJCLXXB bjcl)
        {
            SYS.Model.SDS_JCXXB sds_jcxxb = sds_jcxxdal.Get_SDS_jcxx(sds_jcbh);
            SYS.Model.SDS sdsbjcl = sdsdal.Get_SDS(sds_jcbh);
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
                    new ReportParameter("ReportParameter_jczmc", sds_jcxxb.JCZMC),
                    new ReportParameter("ReportParameter_jcczy", sds_jcxxb.JCCZY),
                    new ReportParameter("ReportParameter_jcjsy", sds_jcxxb.JCJSY),
                    new ReportParameter("ReportParameterjcrq", sdsbjcl.JCRQ),
                    new ReportParameter("ReportParameter_wd", sdsbjcl.WD),
                    new ReportParameter("ReportParameter_dqy", sdsbjcl.DQY),
                    new ReportParameter("ReportParameter_sd", sdsbjcl.SD),
                    new ReportParameter("ReportParameter_sbhzh",sds_jcxxb.SBRZBH),
                    new ReportParameter("ReportParameter_sbmc", sds_jcxxb.SBMC),
                    new ReportParameter("ReportParameter_sbxh", sds_jcxxb.SBXH),
                    new ReportParameter("ReportParameter_sbzzc",sds_jcxxb.SBZZC),
                    new ReportParameter("ReportParameter_kqxsjg",sdsbjcl.λ),
                    new ReportParameter("ReportParameter_dcojg",sdsbjcl.DSCO),
                    new ReportParameter("ReportParameter_dhcjg",sdsbjcl.DSHC),
                    new ReportParameter("ReportParameter_gcojg",sdsbjcl.GDSCO),
                    new ReportParameter("ReportParameter_ghcjg",sdsbjcl.GDSHC),
                    new ReportParameter("ReportParameter_kqxsxz",sdsbjcl.λXZ),
                    new ReportParameter("ReportParameter_dcoxz",sdsbjcl.DSCOXZ),
                    new ReportParameter("ReportParameter_dhcxz", sdsbjcl.DSHCXZ),
                    new ReportParameter("ReportParameter_gcoxz",sdsbjcl.GDSCOXZ),
                    new ReportParameter("ReportParameter_ghcxz",sdsbjcl.GDSHCXZ),
                    new ReportParameter("ReportParameter_gqxspd",sdsbjcl.λPD),
                    new ReportParameter("ReportParameter_dcopd",sdsbjcl.DSCOPD),
                    new ReportParameter("ReportParameter_dhcpd",sdsbjcl.DSHCPD),
                    new ReportParameter("ReportParameter_gcopd",sdsbjcl.GDSCOPD),
                    new ReportParameter("ReportParameter_ghcpd",sdsbjcl.GDSHCPD),
                    new ReportParameter("ReportParameter_cj",sdsbjcl.PDJG)
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

        private void sds_result_panelFormClosing(object sender, FormClosingEventArgs e)
        {
            reportViewer1.LocalReport.ReleaseSandboxAppDomain();
        }

        private void sds_result_panel_Leave(object sender, EventArgs e)
        {
            reportViewer1.LocalReport.ReleaseSandboxAppDomain();
        }
    }
}
