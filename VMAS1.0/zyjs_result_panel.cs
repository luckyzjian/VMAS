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
    public partial class zyjs_result_panel : Form
    {
        SYS_DAL.Zyjsdal zyjsdal = new SYS_DAL.Zyjsdal();
        SYS_DAL.ZYJS_jcxxdal zyjs_jcxxdal = new ZYJS_jcxxdal();
        public zyjs_result_panel()
        {
            InitializeComponent();
        }

        private void zyjs_result_panel_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“ZJ_VMASDataSet.BJCLXXB”中。您可以根据需要移动或删除它。
            //this.BJCLXXBTableAdapter.Fill(this.ZJ_VMASDataSet.BJCLXXB);

        }
        public void display_zyjs(string zyjs_jcbh, SYS.Model.BJCLXXB bjcl)
        {
            SYS.Model.Zyjs_Btg zyjsbjcl = zyjsdal.Get_Zyjs(zyjs_jcbh);
            SYS.Model.ZYJS_JCXXB zyjs_jcxxb = zyjs_jcxxdal.Get_ZYJS_jcxx(zyjs_jcbh);
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
                    new ReportParameter("rptParaB", zyjs_jcxxb.JCZMC),
                    new ReportParameter("ReportParameter_btgxh", zyjs_jcxxb.CSYQXH),
                    new ReportParameter("ReportParameterjcrq",zyjsbjcl.JCRQ),
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
                    new ReportParameter("ReportParameter_jcy",zyjs_jcxxb.JCY),
                    new ReportParameter("ReportParameter_shy", zyjs_jcxxb.SHY),
                    new ReportParameter("ReportParameter_pzr",zyjs_jcxxb.PZR)
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

        private void zyjs_result_panelFormClosing(object sender, FormClosingEventArgs e)
        {
            reportViewer1.LocalReport.ReleaseSandboxAppDomain();
        }
    }
}
