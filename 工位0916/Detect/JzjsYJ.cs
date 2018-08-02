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
    public partial class JzjsYJ : Form
    {
        public JzjsYJ()
        {
            InitializeComponent();
            //treeView1.CheckBoxes = true;
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            CarWait.yj_result = true;
            CarWait.yj_result_inf = "";
            PrintTreeViewNode(treeView1.Nodes);
            this.Close();
        }
        public void PrintTreeViewNode(TreeNodeCollection node)
        {
            foreach (TreeNode n in node)
            {
                if (n.Checked == true)
                {
                    CarWait.yj_result_inf += n.Text + ";";//用";"分隔项目
                    CarWait.yj_result = false;
                }
                PrintTreeViewNode(n.Nodes);
            }
        }
        private void button_cancel_Click(object sender, EventArgs e)
        {
            CarWait.yj_result = false;
            CarWait.yj_result_inf = "取消检测";
            this.Close();
        }

        private void jzjs_yj_Load(object sender, EventArgs e)
        {
            init_yjItems();
        }

        private void init_yjItems()
        {
            treeView1.CheckBoxes = true;
            comboBox1.SelectedIndex = 0;
            buttonToZyjs.Enabled = false;
        }

        private void comboBox1_selectChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != 0)
                buttonToZyjs.Enabled = true;
            else
                buttonToZyjs.Enabled = false;
        }

        private void buttonToZyjs_Click(object sender, EventArgs e)
        {
            CarWait.yj_result = false;
            CarWait.yj_result_inf = "切换为自由加速法";
            this.Close();
        }

    }
}
