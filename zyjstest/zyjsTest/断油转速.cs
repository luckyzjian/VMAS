using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zyjsTest
{
    public partial class 断油转速 : Form
    {
        public 断油转速()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                
                Zyjs_Btg.dyzs = (int)(int.Parse(textBox1.Text)*2/3);
                this.Close();
            }
            else
            {
                MessageBox.Show("请先输入断油转速", "系统提示");
            }
        }
    }
}
