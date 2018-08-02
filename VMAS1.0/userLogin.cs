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
    public partial class userLogin : Form
    {
        public int Carwait_Scroll = 0;                                                  //待检车滚动条位置
        public bool ref_zt = true;                                                      //dataGrid_waitcar_SelectionChanged_1函数使能标记
        public string[] selectID = new string[1024];                                    //当前等待车辆选中的列表
        YGBdal ygbdal = new YGBdal();
        YGB ygb = new YGB();
        public userLogin()
        {
            InitializeComponent();
            ref_WaitCar();
        }

        public void ref_WaitCar()
        {
            try
            {
                DataTable dt_wait = new DataTable();
                dt_wait.Columns.Add("员工编号");
                dt_wait.Columns.Add("员工姓名");
                dt_wait.Columns.Add("用户名");
                dt_wait.Columns.Add("员工职位");
                dt_wait.Columns.Add("员工身份证号");
                dt_wait.Columns.Add("员工电话");
                dt_wait.Columns.Add("员工状态");
                DataTable dt = mainPanelForm.ygbdal.Get_AllUser();
                DataRow dr = null;
                if (dt != null)
                {
                    foreach (DataRow dR in dt.Rows)
                    {
                        dr = dt_wait.NewRow();
                        dr["员工编号"] = dR["YGBH"].ToString();
                        dr["员工姓名"] = dR["YGXM"].ToString();
                        dr["用户名"] = dR["User_Name"].ToString();
                        dr["员工身份证号"] = dR["YGSFZ"].ToString();
                        dr["员工电话"] = dR["DHHM"].ToString();
                        dr["员工状态"] = dR["YGZT"].ToString();
                        dr["员工职位"] = mainPanelForm.zwbdal.getZwmcbyZwbh(int.Parse(dR["YGZWBH"].ToString()));
                        dt_wait.Rows.Add(dr);
                    }
                }
                dataGrid_waitcar.DataSource = dt_wait;
                dataGrid_waitcar.Sort(dataGrid_waitcar.Columns["员工编号"], ListSortDirection.Ascending);
            }
            catch (Exception)
            {

            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }
        private void dataGrid_waitcar_Scroll(object sender, ScrollEventArgs e)
        {
            Carwait_Scroll = e.NewValue;
        }
        private void deleteSelectedCars(object sender, EventArgs e)
        {
            if (dataGrid_waitcar.SelectedRows.Count > 0)
            {
                for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                {
                    ygb = ygbdal.Get_ygxx_by_bh(dataGrid_waitcar.SelectedRows[i].Cells["员工编号"].Value.ToString());
                    ygbdal.deleteThisUser(ygb.YGBH);
                }
                ref_WaitCar();
            }
        }
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ref_WaitCar();
        }
        private void dataGrid_waitcar_SelectionChanged_1(object sender, EventArgs e)
        {
            if (ref_zt)
            {
                if (dataGrid_waitcar.SelectedRows.Count > 0)
                {
                    if (dataGrid_waitcar.SelectedRows.Count == 1)
                    {
                        //ToolStripMenuItem_fc.Enabled = true;
                        ToolStripMenuItemqx.Enabled = true;
                        selectID = new string[1024];
                        for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                        {
                            selectID[i] = dataGrid_waitcar.SelectedRows[i].Cells["员工编号"].Value.ToString();
                        }
                        ygb = ygbdal.Get_ygxx_by_bh(dataGrid_waitcar.SelectedRows[0].Cells["员工编号"].Value.ToString());
                        pictureBox1.ImageLocation = ".\\staffpic\\" + ygb.YGBH + ".jpg";
                    }
                    else if (dataGrid_waitcar.SelectedRows.Count > 1)
                    {
                        //ToolStripMenuItem_fc.Enabled = false;
                        ToolStripMenuItemqx.Enabled = true;
                        selectID = new string[1024];
                        for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                        {
                            selectID[i] = dataGrid_waitcar.SelectedRows[i].Cells["员工编号"].Value.ToString();
                        }
                    }
                    else
                    {
                        //ToolStripMenuItem_fc.Enabled = false;
                        selectID = new string[1024];
                        for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                        {
                            selectID[i] = dataGrid_waitcar.SelectedRows[i].Cells["员工编号"].Value.ToString();
                        }
                    }
                }
                else
                {
                    selectID = new string[1024];
                    for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                    {
                        selectID[i] = dataGrid_waitcar.SelectedRows[i].Cells["员工编号"].Value.ToString();
                    }
                }
            }
        }

    }
}
