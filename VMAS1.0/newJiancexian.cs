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

namespace VMAS1._0
{
   
    public partial class newJiancexian : Form
    {
        public static SBXX sbxx = new SBXX();
        public static JCXXX jcxxx = new JCXXX();
        private JCXXXB newjcx = new JCXXXB();
        public newJiancexian()
        {
            InitializeComponent();
            init_sb();
        }
        #region 初始化检测线选项
        private void init_sb()
        {
            DataTable dt = sbxx.Get_sb_by_lx("底盘测功机");
            if (dt != null)
            {
                foreach (DataRow dR in dt.Rows)
                {
                    comboBoxCgj.Items.Add(dR["SBMC"].ToString());
                }
            }
            dt = sbxx.Get_sb_by_lx("废气分析仪");
            if (dt != null)
            {
                foreach (DataRow dR in dt.Rows)
                {
                    comboBoxFqfxy.Items.Add(dR["SBMC"].ToString());
                }
            }
            dt = sbxx.Get_sb_by_lx("不透光烟度计");
            if (dt != null)
            {
                foreach (DataRow dR in dt.Rows)
                {
                    comboBoxYdj.Items.Add(dR["SBMC"].ToString());
                }
            }
            dt = sbxx.Get_sb_by_lx("气体流量分析仪");
            if (dt != null)
            {
                foreach (DataRow dR in dt.Rows)
                {
                    comboBoxLlj.Items.Add(dR["SBMC"].ToString());
                }
            }
            dt = sbxx.Get_sb_by_lx("气体流量分析仪");
            if (dt != null)
            {
                foreach (DataRow dR in dt.Rows)
                {
                    comboBoxLlj.Items.Add(dR["SBMC"].ToString());
                }
            }
            dt = sbxx.Get_sb_by_lx("计算机控制系统");
            if (dt != null)
            {
                foreach (DataRow dR in dt.Rows)
                {
                    comboBoxPC.Items.Add(dR["SBMC"].ToString());
                }
            }
            dt = sbxx.Get_sb_by_lx("环境站");
            if (dt != null)
            {
                foreach (DataRow dR in dt.Rows)
                {
                    comboBoxHjz.Items.Add(dR["SBMC"].ToString());
                }
            }
            dt = sbxx.Get_sb_by_lx("转速计");
            if (dt != null)
            {
                foreach (DataRow dR in dt.Rows)
                {
                    comboBoxZsj.Items.Add(dR["SBMC"].ToString());
                }
            }
        }
        #endregion

        private void btn_ok_Click(object sender, EventArgs e)
        {
            newjcx.JCXBH = textBoxNewLineBh.Text;
            newjcx.JCXMC = textBoxJcxmc.Text;
            newjcx.GYJSJIP = textBoxJcxIP.Text;
            if (comboBoxCgj.Text == "" || comboBoxCgj.Text == "无")
            {
                newjcx.DPCGJBH = 0;
            }
            else
            {
                newjcx.DPCGJBH =sbxx.Get_sbbh_by_sbmc(comboBoxCgj.Text); 
            }
            if (comboBoxFqfxy.Text == "" || comboBoxFqfxy.Text == "无")
            {
                newjcx.FQFXYBH = 0;
            }
            else
            {
                newjcx.FQFXYBH = sbxx.Get_sbbh_by_sbmc(comboBoxFqfxy.Text);
            }
            if (comboBoxYdj.Text == "" || comboBoxYdj.Text == "无")
            {
                newjcx.BTGYDJBH = 0;
            }
            else
            {
                newjcx.BTGYDJBH = sbxx.Get_sbbh_by_sbmc(comboBoxYdj.Text);
            }
            if (comboBoxLlj.Text == "" || comboBoxLlj.Text == "无")
            {
                newjcx.LLJBH = 0;
            }
            else
            {
                newjcx.LLJBH = sbxx.Get_sbbh_by_sbmc(comboBoxLlj.Text);
            }
            if (comboBoxPC.Text == "" || comboBoxPC.Text == "无")
            {
                newjcx.PCBH = 0;
            }
            else
            {
                newjcx.PCBH = sbxx.Get_sbbh_by_sbmc(comboBoxPC.Text);
            }
            if (comboBoxHjz.Text == "" || comboBoxHjz.Text == "无")
            {
                newjcx.HJZBH = 0;
            }
            else
            {
                newjcx.HJZBH = sbxx.Get_sbbh_by_sbmc(comboBoxHjz.Text);
            }
            if (comboBoxZsj.Text == "" || comboBoxZsj.Text == "无")
            {
                newjcx.WYZSBBH = 0;
            }
            else
            {
                newjcx.WYZSBBH = sbxx.Get_sbbh_by_sbmc(comboBoxZsj.Text);
            }
            newjcx.RZBH = textBoxRzbh.Text.Trim();
            newjcx.ZZCS = textBoxZzcs.Text.Trim();
            newjcx.LJSYS = 0;
            newjcx.JCFFLX = 0;
            newjcx.QTLLFXYBH = 0;
            newjcx.LEDDPBH = 0;
            newjcx.XH = textBoxXh.Text;
            string cgjCK = comboBoxCgjCk.Text.Trim() + "||38400,n,8,1||BNTD";
            string fqyCK = comboBoxFqyCk.Text.Trim() + "||9600,n,8,1||FLA_502";
            string ydjCK = comboBoxYdjCk.Text.Trim() + "||9600,n,8,1||FLB_100";
            string lljCK = comboBoxLljCk.Text.Trim() + "||9600,n,8,1||FLV_1000";
            newjcx.DPCGJPZ = cgjCK;
            newjcx.FQFXYPZ = fqyCK;
            newjcx.BTGYDJPZ = ydjCK;
            newjcx.LLJPZ = lljCK;
            if(jcxxx.Have_ThisLine(newjcx.JCXBH,newjcx.JCXMC,newjcx.GYJSJIP))
            {
                if (MessageBox.Show("该条检测线已经存在,是否更新该检测线信息？(如果不更新,请检测新添加检测线的编号,名称或IP地址是否已经存在)", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    jcxxx.Update(newjcx);
                    MessageBox.Show("更新成功", "系统提示");
                }
            }
            else
            {
                jcxxx.AddLine(newjcx);
                MessageBox.Show("添加检测线成功", "系统提示");
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (textBoxNewLineBh.Text == "")
            {
                MessageBox.Show("请输入检测线编号", "系统提示");
            }
            else
            {
                init_jcxInfor(textBoxNewLineBh.Text.Trim());
            }
        }
        public void init_jcxInfor(string newjcxbh)
        {
            if (jcxxx.Have_ThisLine(newjcxbh, "", ""))
            {
                try
                {
                    JCXXXB newjcx = jcxxx.GetModelbyJcxbh(newjcxbh);
                    textBoxJcxIP.Text = newjcx.GYJSJIP;
                    textBoxJcxmc.Text = newjcx.JCXMC;
                    if (newjcx.DPCGJBH != 0)
                    {
                        int cgjbh = newjcx.DPCGJBH;
                        SYS.Model.SBXXB cgjsb = null;
                        cgjsb = sbxx.Get_sb_by_bh(cgjbh);
                        comboBoxCgj.Text = cgjsb.SBMC;
                    }
                    else
                    {
                        comboBoxCgj.Text = "无";
                    }
                    if (newjcx.FQFXYBH != 0)
                    {
                        int cgjbh = newjcx.FQFXYBH;
                        SYS.Model.SBXXB cgjsb = null;
                        cgjsb = sbxx.Get_sb_by_bh(cgjbh);
                        comboBoxFqfxy.Text = cgjsb.SBMC;
                    }
                    else
                    {
                        comboBoxFqfxy.Text = "无";
                    }
                    if (newjcx.BTGYDJBH != 0)
                    {
                        int cgjbh = newjcx.BTGYDJBH;
                        SYS.Model.SBXXB cgjsb = null;
                        cgjsb = sbxx.Get_sb_by_bh(cgjbh);
                        comboBoxYdj.Text = cgjsb.SBMC;
                    }
                    else
                    {
                        comboBoxYdj.Text = "无";
                    }
                    if (newjcx.LLJBH != 0)
                    {
                        int cgjbh = newjcx.LLJBH;
                        SYS.Model.SBXXB cgjsb = null;
                        cgjsb = sbxx.Get_sb_by_bh(cgjbh);
                        comboBoxLlj.Text = cgjsb.SBMC;
                    }
                    else
                    {
                        comboBoxLlj.Text = "无";
                    }
                    if (newjcx.PCBH != 0)
                    {
                        int cgjbh = newjcx.PCBH;
                        SYS.Model.SBXXB cgjsb = null;
                        cgjsb = sbxx.Get_sb_by_bh(cgjbh);
                        comboBoxPC.Text = cgjsb.SBMC;
                    }
                    else
                    {
                        comboBoxPC.Text = "无";
                    }
                    if (newjcx.HJZBH != 0)
                    {
                        int cgjbh = newjcx.HJZBH;
                        SYS.Model.SBXXB cgjsb = null;
                        cgjsb = sbxx.Get_sb_by_bh(cgjbh);
                        comboBoxHjz.Text = cgjsb.SBMC;
                    }
                    else
                    {
                        comboBoxHjz.Text = "无";
                    }
                    if (newjcx.WYZSBBH != 0)
                    {
                        int cgjbh = newjcx.WYZSBBH;
                        SYS.Model.SBXXB cgjsb = null;
                        cgjsb = sbxx.Get_sb_by_bh(cgjbh);
                        comboBoxZsj.Text = cgjsb.SBMC;
                    }
                    else
                    {
                        comboBoxZsj.Text = "无";
                    }
                    textBoxRzbh.Text = newjcx.RZBH;
                    textBoxZzcs.Text = newjcx.ZZCS;
                    textBoxXh.Text = newjcx.XH;
                    string cgjCK = newjcx.DPCGJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    string fqyCK = newjcx.FQFXYPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    string ydjCK = newjcx.BTGYDJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    string lljCK = newjcx.LLJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    comboBoxCgjCk.Text = cgjCK;
                    comboBoxFqyCk.Text = fqyCK;
                    comboBoxYdjCk.Text = ydjCK;
                    comboBoxLljCk.Text = lljCK;
                    MessageBox.Show("查询成功！", "系统提示");
                }
                catch
                {
                    MessageBox.Show("检测线读取出错", "系统提示");
                }
            }
            else
            {
                MessageBox.Show("不存在该检测线", "系统提示");
            }
 
        }

        
    }
    
}
