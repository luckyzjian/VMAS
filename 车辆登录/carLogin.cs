using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace 车辆登录
{
    public partial class carLogin : Form
    {
        public carLogin()
        {
            InitializeComponent();
        }

        private void 车辆检测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Control childpenal in panelMain.Controls)
            {
                panelMain.Controls.Remove(childpenal);
            }
            carTest childcartest = new carTest();
            childcartest.TopLevel = false;
            panelMain.Controls.Add(childcartest);
            childcartest.Location = new Point(panelMain.Width / 2 - childcartest.Width / 2, panelMain.Height/2-childcartest.Height/2);
            childcartest.Show();
        }

        private void carLogin_Load(object sender, EventArgs e)
        {

        }

        private void 参数设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("D://环保检测子程序/系统设置.EXE");
            }
            catch (Exception er)
            {
                MessageBox.Show("异常:" + er.Message);
            }
        }

        private void 车型库设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("D://环保检测子程序/车型库设置.EXE");
            }
            catch (Exception er)
            {
                MessageBox.Show("异常:" + er.Message);
            }
        }

        private void 滑行法测内阻ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("D://环保检测子程序/测功机内阻测试.EXE");
            }
            catch (Exception er)
            {
                MessageBox.Show("异常:" + er.Message);
            }
        }
    }
}
