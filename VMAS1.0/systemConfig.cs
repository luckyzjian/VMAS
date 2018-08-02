using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using System.Data.Sql;

namespace VMAS1._0
{
    public partial class systemConfig : Form
    {
        public systemConfig()
        {
            InitializeComponent();
        }

        private void systemConfig_Load(object sender, EventArgs e)
        {
            string cgjCK = mainPanelForm.jcxxxb.DPCGJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0];
            string fqyCK = mainPanelForm.jcxxxb.FQFXYPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0];
            string ydjCK = mainPanelForm.jcxxxb.BTGYDJPZ.Split(new string[]{ "||" }, StringSplitOptions.RemoveEmptyEntries)[0];
            string lljCK = mainPanelForm.jcxxxb.LLJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0];
            comboBoxIgbt.Text = cgjCK;
            comboBoxFqy.Text = fqyCK;
            comboBoxYdj.Text = ydjCK;
            comboBoxLlj.Text = lljCK;
            try
            {
                foreach (string com in System.IO.Ports.SerialPort.GetPortNames()) //自动获取串行口名称
                {
                    comboBoxIgbt.Items.Add(com);
                    comboBoxFqy.Items.Add(com);
                    comboBoxLlj.Items.Add(com);
                    comboBoxYdj.Items.Add(com);
                }
               // DisplaySqlData();
            }
            catch
            {
                MessageBox.Show("没有找到串口，不能使用此软件", "提示");     //没有获取到COM报错
            }

        }

        private void simpleButtonSaveSerial_Click(object sender, EventArgs e)
        {
            string cgjCK = comboBoxIgbt.Text.Trim() + "||38400,n,8,1||" + mainPanelForm.jcxxxb.DPCGJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[2];
            string fqyCK = comboBoxFqy.Text.Trim() + "||9600,n,8,1||"+mainPanelForm.jcxxxb.FQFXYPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[2];
            string ydjCK = comboBoxYdj.Text.Trim() + "||9600,n,8,1||" + mainPanelForm.jcxxxb.BTGYDJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[2];
            string lljCK = comboBoxLlj.Text.Trim() + "||9600,n,8,1||" + mainPanelForm.jcxxxb.LLJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[2];
            mainPanelForm.jcxxx.updateJcxSerialConfig(mainPanelForm.jcxxxb.GYJSJIP, cgjCK, fqyCK, ydjCK, lljCK);
        }
        private void DisplaySqlData()
        {
            SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
            System.Data.DataTable Sqltable = instance.GetDataSources();
            foreach (System.Data.DataRow row in Sqltable.Rows)
            {
                foreach (System.Data.DataColumn col in Sqltable.Columns)
                {
                    comboBoxSqlName.Items.Add(row["ServerName"].ToString());
                }
            }
        }

        private void simpleButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            newUserLogin childCarLogin = new newUserLogin();
            childCarLogin.TopLevel = false;
            childCarLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            childCarLogin.FormBorderStyle = FormBorderStyle.None;
            this.panel_User.Controls.Clear();
            this.panel_User.Controls.Add(childCarLogin);
            childCarLogin.Show();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            userLogin childCarLogin = new userLogin();
            childCarLogin.TopLevel = false;
            childCarLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            childCarLogin.FormBorderStyle = FormBorderStyle.None;
            this.panel_User.Controls.Clear();
            this.panel_User.Controls.Add(childCarLogin);
            childCarLogin.Show();
        }
    }
}
