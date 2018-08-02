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

namespace Detect
{
    public partial class vmas_result_panel : Form
    {
        SYS_DAL.VMASdal vmasdal = new SYS_DAL.VMASdal();
        SYS_DAL.VMAS_jcxxdal vmas_jcxxdal = new VMAS_jcxxdal();
        public vmas_result_panel()
        {
            InitializeComponent();
        }

        private void vmas_result_panel_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“ZJ_VMASDataSet.BJCLXXB”中。您可以根据需要移动或删除它。
            //this.BJCLXXBTableAdapter.Fill(this.ZJ_VMASDataSet.BJCLXXB);

        }
        public void display_vmas(string vmas_jcbh, SYS.Model.BJCLXXB bjcl)
        {
            SYS.Model.VMAS vmasbjcl = vmasdal.Get_VMAS(vmas_jcbh);
            SYS.Model.VMAS_JCXXB vmas_jcxxb = vmas_jcxxdal.Get_VMAS_jcxx(vmas_jcbh);
            reportViewer1.LocalReport.ReleaseSandboxAppDomain();
            reportViewer1.LocalReport.Dispose();
            DataSet myds = new DataSet();
            DataSet mydy = new DataSet();
            SqlParameter[] spr ={
                                   new SqlParameter("@jcclph",bjcl.JCCLPH)
                               };
            string sql = "select * from BJCLXXB where jcclph=@jcclph";
            myds=DBHelperSQL.GetDataSet(sql, spr);
            reportViewer1.Visible = true;
            try
            {
                ReportParameter[] rptpara =
                {
                    new ReportParameter("ReportParameter_jczmc", vmas_jcxxb.JCZMC),
                    new ReportParameter("ReportParameter_jcczy", vmas_jcxxb.JCCZY),
                    new ReportParameter("ReportParameterjcrq", vmasbjcl.JCRQ),
                    new ReportParameter("ReportParameterjcrq", vmas_jcxxb.JCRQ),
                    new ReportParameter("ReportParameter_jcjsy", vmas_jcxxb.JCJSY),
                    new ReportParameter("ReportParameter_sbhzh", vmas_jcxxb.SBRZBH),
                    new ReportParameter("ReportParameter_sbmc", vmas_jcxxb.SBMC),
                    new ReportParameter("ReportParameter_sbxh", vmas_jcxxb.SBXH),
                    new ReportParameter("ReportParameter_sbzzc",vmas_jcxxb.SBZZC),
                    new ReportParameter("ReportParameter_dpcgj", vmas_jcxxb.DPCGJ),
                    new ReportParameter("ReportParameter_pqfxy", vmas_jcxxb.PQFXY),
                    new ReportParameter("ReportParameter_wd",vmasbjcl.WD),
                    new ReportParameter("ReportParameter_sd", vmasbjcl.SD),
                    new ReportParameter("ReportParameter_dqy",vmasbjcl.DQY),
                    new ReportParameter("ReportParameter_hcjg",vmasbjcl.HCZL),
                    new ReportParameter("ReportParameter_cojg",vmasbjcl.COZL),
                    new ReportParameter("ReportParameter_noxjg",vmasbjcl.NOXZL),
                    new ReportParameter("ReportParameter_coxz", vmasbjcl.COXZ),
                    new ReportParameter("ReportParameter_hcxz",vmasbjcl.HCXZ),
                    new ReportParameter("ReportParameter_noxxz", vmasbjcl.NOXXZ),
                    new ReportParameter("ReportParameter_hcpd",vmasbjcl.HCPD),
                    new ReportParameter("ReportParameter_copd",vmasbjcl.COPD),
                    new ReportParameter("ReportParameter_noxpd", vmasbjcl.NOXPD),
                    new ReportParameter("ReportParameter_cj",vmasbjcl.CJ)
                };
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.SetParameters(rptpara);
            }
            catch
            {
                throw;
            }
            //reportViewer1.LocalReport.SetParameters(rptpara);
            ReportDataSource rds = new ReportDataSource("DataSet2", myds.Tables[0]);
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.RefreshReport();
        }

        private void vmas_result_panelFormClosing(object sender, FormClosingEventArgs e)
        {
            reportViewer1.LocalReport.ReleaseSandboxAppDomain();
        }
    }
}
