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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text.Trim();
            string password = textBoxPassword.Text.Trim();
            mainPanelForm.loginSuccess = mainPanelForm.ygbdal.checkIsRight(username, password);
            if (mainPanelForm.loginSuccess)
            {
                mainPanelForm.loginYg = mainPanelForm.ygbdal.getXXbyuserName(username);
                this.Close();
            }
            else
            {
                MessageBox.Show("请检查登录信息是否正确","登录失败！");
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            mainPanelForm.loginSuccess = false;
            this.Close();
        }
    }
}
