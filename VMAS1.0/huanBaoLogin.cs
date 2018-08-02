using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VMAS1._0
{
    public partial class huanBaoLogin : Form
    {
        public huanBaoLogin()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text.Trim();
            string password = textBoxPassword.Text.Trim();
            mainPanelForm.huanBaoLogin = mainPanelForm.ygbdal.checkIsHuanBaoRight(username, password);
            if (mainPanelForm.huanBaoLogin)
            {
                this.Close();
            }
            else
            {
                if(mainPanelForm.ygbdal.checkIsRight(username, password))
                    MessageBox.Show("该用户权限不够，不能进入","登录失败！");
                else
                    MessageBox.Show("登录信息错误，请核实", "登录失败！");
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            mainPanelForm.huanBaoLogin = false;
            this.Close();
        }
    }
}
