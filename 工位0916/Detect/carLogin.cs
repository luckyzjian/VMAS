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

namespace Detect
{
    public partial class carLogin : Form
    {
        public static BJCLXX bjclxx = new BJCLXX();                                     //被检车辆信息方法集dal
        public static JCXXXB jcxxxb = new JCXXXB();
        JCXXX jcxxx = new JCXXX(); 
        public carLogin()
        {
            InitializeComponent();
           
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void simpleButtonCheckIsExist_Click(object sender, EventArgs e)
        {
        }
        
        private void simpleButtonLogin_Click(object sender, EventArgs e)
        {
            if (comboBoxJcff.Text == "")
                MessageBox.Show("请选择车辆检测方法", "系统提示");
            else if (comboBoxJcff.Text == "简易瞬态工况法")
            {
                vmasLogin childvmaspanel = new vmasLogin();
                childvmaspanel.TopLevel = false;
                childvmaspanel.Dock = System.Windows.Forms.DockStyle.Fill;
                childvmaspanel.FormBorderStyle = FormBorderStyle.None;
                this.panel_Login.Controls.Clear();
                this.panel_Login.Controls.Add(childvmaspanel);
                childvmaspanel.Show();
            }
            else if (comboBoxJcff.Text == "双怠速法")
            {
                SdsLogin childvmaspanel = new SdsLogin();
                childvmaspanel.TopLevel = false;
                childvmaspanel.Dock = System.Windows.Forms.DockStyle.Fill;
                childvmaspanel.FormBorderStyle = FormBorderStyle.None;
                this.panel_Login.Controls.Clear();
                this.panel_Login.Controls.Add(childvmaspanel);
                childvmaspanel.Show();
            }
            else if (comboBoxJcff.Text == "自由加速法")
            {
                ZyjsLogin childvmaspanel = new ZyjsLogin();
                childvmaspanel.TopLevel = false;
                childvmaspanel.Dock = System.Windows.Forms.DockStyle.Fill;
                childvmaspanel.FormBorderStyle = FormBorderStyle.None;
                this.panel_Login.Controls.Clear();
                this.panel_Login.Controls.Add(childvmaspanel);
                childvmaspanel.Show();
            }
            else if (comboBoxJcff.Text == "加载减速法")
            {
                JzjsLogin childvmaspanel = new JzjsLogin();
                childvmaspanel.TopLevel = false;
                childvmaspanel.Dock = System.Windows.Forms.DockStyle.Fill;
                childvmaspanel.FormBorderStyle = FormBorderStyle.None;
                this.panel_Login.Controls.Clear();
                this.panel_Login.Controls.Add(childvmaspanel);
                childvmaspanel.Show();
            }
        }

        private void carStyleChanged(object sender, EventArgs e)
        {
            if (radioButton_qiyou.Checked == true)
            {
                comboBoxJcff.Items.Clear();
                comboBoxJcff.Items.Add("双怠速法");
                comboBoxJcff.Items.Add("简易瞬态工况法");
            }
        }

        private void caiyouCarSelected(object sender, EventArgs e)
        {
            if (radioButton_caiyou.Checked == true)
            {
                comboBoxJcff.Items.Clear();
                comboBoxJcff.Items.Add("自由加速法");
                comboBoxJcff.Items.Add("加载减速法");
            }
        }
    }
}
