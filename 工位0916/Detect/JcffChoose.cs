using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Detect
{
    public partial class JcffChoose : Form
    {
        public string jcff_choose = "";
        public JcffChoose()
        {
            InitializeComponent();
        }
        public void JcffChoose_Load(object sender, EventArgs e)
        {
            //comboBoxJcff.style = 2;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (comboBoxJcff.Text.Trim() == "")
            {
                MessageBox.Show("请选择一种测试方法","系统提示");
            }
            else
            {
                jcff_choose = comboBoxJcff.Text;
                this.Close();
            }

        }

        private void JcffChoose_Load()
        {

        }
    }
}
