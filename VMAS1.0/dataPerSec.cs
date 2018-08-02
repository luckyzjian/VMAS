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
    public partial class dataPerSec : Form
    {
        public SYS_DAL.VMASdal vmasdal = new SYS_DAL.VMASdal();
        public SYS_DAL.JZJSdal jzjsdal = new JZJSdal();
        public SYS_DAL.SDSdal sdsdal = new SDSdal();
        public ZYJS_jcxxdal zyjsdal = new ZYJS_jcxxdal();
        public static DataTable dt = null;                                                          //全局表
        public dataPerSec()
        {
            InitializeComponent();
        }
        public void init_datagridview(string jcff)
        {
            dt = new DataTable();
            DataRow dr = null;
            if (jcff == "简易瞬态工况法")
            {
                dt.Columns.Add("工况时间（s）");
                dt.Columns.Add("车速(m/s)");
                dt.Columns.Add("CO浓度（%）");
                dt.Columns.Add("CO2浓度（%）");
                dt.Columns.Add("O2浓度（%）");
                dt.Columns.Add("HC浓度（10-6）");
                dt.Columns.Add("NO浓度（10-6）");
                dt.Columns.Add("稀释O2浓度（%）");
                dt.Columns.Add("流量计流量（L/s）");
                dt.Columns.Add("稀释比");
                dt.Columns.Add("尾气实际流量（L/s）");
                dt.Columns.Add("CO质量（mg）");
                dt.Columns.Add("HC质量（mg）");
                dt.Columns.Add("NO质量（mg）");
                dt.Columns.Add("流量计压力（kPa）");
                dt.Columns.Add("流量计温度（℃）");
            }
            else if (jcff == "加载减速法")
            {
                dt.Columns.Add("工况时间（s）");
                dt.Columns.Add("车速(m/s)");
                dt.Columns.Add("扭力（N）");
                dt.Columns.Add("功率（KW）");
                dt.Columns.Add("转速（转/分）");
                dt.Columns.Add("烟度值（m-1）");
 
            }
            dataGridView1.DataSource = dt;
        }
        public void init_data(string jcbh, string jcff)
        {
            if(jcff=="简易瞬态工况法")
            {

            VMAS vmasdata = vmasdal.Get_VMAS(jcbh);
            label_cp.Text = vmasdata.JCCLPH;
            label_bh.Text = vmasdata.JCBH;
            label_ff.Text = jcff;
            string[] vmas_cs = vmasdata.MMCS.Split(',');
            string[] vmas_o2 = vmasdata.MMO2.Split(',');
            string[] vmas_co = vmasdata.MMCO.Split(',');
            string[] vmas_co2 = vmasdata.MMCO2.Split(',');
            string[] vmas_hc = vmasdata.MMHC.Split(',');
            string[] vmas_no = vmasdata.MMNO.Split(',');
            string[] vmas_xisio2 = vmasdata.MMXSO2.Split(',');
            string[] vmas_lljll = vmasdata.MMBZLL.Split(',');
            string[] vmas_sjll = vmasdata.MMSJLL.Split(',');
            string[] vmas_xsb = vmasdata.MMXSXS.Split(',');
            string[] vmas_cozl = vmasdata.MMCOZL.Split(',');
            string[] vmas_hczl= vmasdata.MMHCZL.Split(',');
            string[] vmas_nozl = vmasdata.MMNOZL.Split(',');
            string[] vmas_lljyl = vmasdata.MMLLJYL.Split(',');
            string[] vmas_lljwd = vmasdata.MMLLJWD.Split(',');
            if (vmasdata.GKSJ != "")
            {
                for (int i = 0; i < int.Parse(vmasdata.GKSJ); i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["工况时间（s）"] = i.ToString();
                    if (vmas_cs.Length > i)
                        dr["车速(m/s)"] = vmas_cs[i];
                    if (vmas_o2.Length > i)
                        dr["O2浓度（%）"] = vmas_o2[i];
                    if (vmas_co.Length > i)
                        dr["CO浓度（%）"] = vmas_co[i];
                    if (vmas_co2.Length > i)
                        dr["CO2浓度（%）"] = vmas_co2[i];
                    if (vmas_hc.Length > i)
                        dr["HC浓度（10-6）"] = vmas_hc[i];
                    if (vmas_no.Length > i)
                        dr["NO浓度（10-6）"] = vmas_no[i];
                    if (vmas_xisio2.Length > i)
                        dr["稀释O2浓度（%）"] = vmas_xisio2[i];
                    if (vmas_lljll.Length > i)
                        dr["流量计流量（L/s）"] = vmas_lljll[i];
                    if (vmas_sjll.Length > i)
                        dr["尾气实际流量（L/s）"] = vmas_sjll[i];
                    if (vmas_xsb.Length > i)
                        dr["稀释比"] = vmas_xsb[i];
                    if (vmas_cozl.Length > i)
                        dr["CO质量（mg）"] = vmas_cozl[i];
                    if (vmas_nozl.Length > i)
                        dr["NO质量（mg）"] = vmas_nozl[i];
                    if (vmas_hczl.Length > i)
                        dr["HC质量（mg）"] = vmas_hczl[i];
                    if (vmas_lljwd.Length > i)
                        dr["流量计温度（℃）"] = vmas_lljwd[i];
                    if (vmas_lljyl.Length > i)
                        dr["流量计压力（kPa）"] = vmas_lljyl[i];
                    dt.Rows.Add(dr);
                    dataGridView1.DataSource = dt;

                }
            }
            
            }
            else if (jcff == "加载减速法")
            {
                JZJS jzjsdata = jzjsdal.Get_JZJS(jcbh);
                label_cp.Text = jzjsdata.JCCLPH;
                label_bh.Text = jzjsdata.JCBH;
                label_ff.Text = jcff;
                string[] jzjs_cs = jzjsdata.MMZGXSD.Split(',');
                string[] jzjs_zs = jzjsdata.MMFDJZS.Split(',');
                string[] jzjs_btg = jzjsdata.MMXGK.Split(',');
                string[] jzjs_nl = jzjsdata.MMZGZDL.Split(',');
                string[] jzjs_power = jzjsdata.MMCGJFH.Split(',');
                if (jzjsdata.GKSJ != null && jzjsdata.GKSJ!=0)
                {
                    for (int i = 0; i < jzjsdata.GKSJ; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["工况时间（s）"] = i.ToString();
                        if (jzjs_cs.Length > i)
                            dr["车速(m/s)"] = jzjs_cs[i];
                        if (jzjs_nl.Length > i)
                            dr["扭力（N）"] = jzjs_nl[i];
                        if (jzjs_power.Length > i)
                            dr["功率（KW）"] = jzjs_power[i];
                        if (jzjs_zs.Length > i)
                            dr["转速（转/分）"] = jzjs_zs[i];
                        if (jzjs_btg.Length > i)
                            dr["烟度值（m-1）"] = jzjs_btg[i];
                        dt.Rows.Add(dr);
                        dataGridView1.DataSource = dt;
                    }
                }

            }
        }
    }
}
