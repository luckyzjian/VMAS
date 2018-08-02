using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using SYS.Model;
using System.Windows.Forms;

namespace VMAS1._0
{
    public partial class newUserLogin : Form
    {
        OpenFileDialog picfile = null;
        bool staffpic = false;
        public newUserLogin()
        {
            InitializeComponent();
        }

        private void UserLogin_Load(object sender, EventArgs e)
        {
            init_zw();
        }
        private void init_zw()
        {
            DataTable zwb = mainPanelForm.zwbdal.Get_AllUser();
            DataRow dr = null;
            if (zwb != null)
            {
                foreach (DataRow dR in zwb.Rows)
                {
                    comboBoxYgzwbh.Items.Add(dR["ZWMC"].ToString());
                }
                comboBoxYgzwbh.SelectedIndex = 3;
            }
        }

        private void simpleButtonSave_Click(object sender, EventArgs e)
        {
            if(textBoxYgbh.Text==""||textBoxYgxm.Text==""||textBoxPassword.Text==""||textBoxYgsfz.Text==""||textBoxSecPassword.Text==""||
                textBoxUsername.Text==""||textBoxYgdh.Text=="")
            {
                MessageBox.Show("资料不完整，请填写完整", "系统提示");
                return;
            }
            if (mainPanelForm.ygbdal.checkUserNameIsAlive(textBoxUsername.Text.Trim()))
            {
                MessageBox.Show("用户名已经存在，请重新填写", "系统提示");
                return;
            }
            if (mainPanelForm.ygbdal.checkYgbhIsAlive(textBoxYgbh.Text.Trim()))
            {
                MessageBox.Show("员工编号已经存在，请重新填写", "系统提示");
                return;
            }
            if (textBoxPassword.Text!=textBoxSecPassword.Text)
            {
                MessageBox.Show("两次密码输入不一致，请重新输入", "系统提示");
                textBoxPassword.Text = "";
                textBoxSecPassword.Text = "";
                return;
            }
            YGB newyg = new YGB();
            newyg.YGBH = textBoxYgbh.Text.Trim();
            newyg.YGXM = textBoxYgxm.Text.Trim();
            newyg.YGSFZ = textBoxYgsfz.Text.Trim();
            newyg.YGZT = "正常";
            newyg.YGZWBH = mainPanelForm.zwbdal.getZwbhbyZwmc(comboBoxYgzwbh.Text.Trim());
            newyg.User_Name = textBoxUsername.Text.Trim();
            newyg.Password = textBoxPassword.Text.Trim();
            newyg.DHHM = textBoxYgdh.Text.Trim();
            if (mainPanelForm.ygbdal.AddNewUser(newyg))
            {
                if (staffpic == true)
                {
                    System.IO.File.Copy(picfile.FileName, ".\\staffpic\\"+newyg.YGBH+".jpg",true);
                }
                MessageBox.Show("员工添加成功，用户名：" + newyg.User_Name + ",密码：" + newyg.Password, "系统提示");

            }
            else
                MessageBox.Show("操作失败，员工" + newyg.User_Name + "添加过程出现异常", "系统提示");
            staffpic = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            picfile = new OpenFileDialog();
            picfile.Title = "请选择图片";
            picfile.InitialDirectory = ".\\staffpic";
            picfile.Filter = "JPG(*.JPG)|*.JPG|JPEG(*.JPEG)|*.JPEG";
            if (picfile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.ImageLocation = picfile.FileName;
                    staffpic = true;
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.ToString(), "出错啦");
                }
            }
        }
    }
}
