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
using System.Threading;
using BpqFl;
using ini;

namespace VMAS1._0
{
    public partial class mainPanelForm : Form
    {
        tool tool = new tool();
        public static bool loginSuccess = false;
        public static bool huanBaoLogin = false;
        public static string ip;  
        public static YGB loginYg = new YGB();
        public static JCXXXB jcxxxb=new JCXXXB();
        public static JCXXX jcxxx=new JCXXX();
        JCZXXB jczxxb=new JCZXXB();
        JCZdal jczxx = new JCZdal();
        public static YGBdal ygbdal = new YGBdal();
        private Thread Th_initSql =null;
        public static ZWBdal zwbdal = new ZWBdal();
        public static string sbstarttime = "";
        public static string sbendtime = "";
        public static string ygendtime = "";
        public static string ygstarttime = "";
        public mainPanelForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            //this.Hide();
        }

        private void buttonIg195Test_Click(object sender, EventArgs e)
        {
            
            //this.Hide();
        }

        private void buttonMainTest_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
            Main newMain = new Main();
            newMain.Exit_Control();
            newMain.ShowDialog();
            timer1.Start();
        }

        private void buttonIg195Test_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
            Detect.CarWait newCarWait=new Detect.CarWait();
            newCarWait.ShowDialog();
            timer1.Start();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
            demarcate newDemarcateForm = new demarcate();
            newDemarcateForm.ShowDialog();
            timer1.Start();
        }
        private void init_sql()
        {
            try
            {
                toolStripLabelSql.Text = "正在初始化数据库...";
                ip = tool.getIp();                          //获取本机ip
                jcxxxb = jcxxx.GetModel(ip);                //初始化检测线
                Login newlogin = new Login();
                newlogin.ShowDialog();
                if (loginSuccess)
                {
                    enable_panel();
                    labelUserName.Text = loginYg.User_Name;
                }
                else
                {
                    disabled_panel();
                    labelUserName.Text = "未登录";
                }
                if (jcxxxb.JCXBH != "-2")                     //在服务器找到了本机IP配置的检测线
                {
                    jczxxb = jczxx.Get_jczxx();
                    label_jcxbh.Text = jczxxb.JCZBH;
                    label_jczmc.Text = jczxxb.JCZMC;
                    this.Text = "沈阳大路机动车检测站："+jcxxxb.JCXMC;
                }
                else
                {
                    disabled_panel();                           //禁止相应操作
                    MessageBox.Show("本机" + ip + "在配置之前，无法作为检测线使用。", "系统提示");
                }
                toolStripLabelSql.Text = "数据库连接正常";
            }
            catch
            {
                toolStripLabelSql.Text = "数据库连接异常";
                disabled_panel();
                MessageBox.Show("数据库未连接成功，请检查配置文件是否正确", "系统提示");
            }
        }
        private void Mainform_Load(object sender, EventArgs e)
        {
            init_control();
            init_sql();
            init_ini();
            timer1.Enabled = true;
        }

        private void init_ini()
        {
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            try
            {
                ini.INIIO.GetPrivateProfileString("设备起止日期", "起始日期", "2014/2/10", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                sbstarttime = temp.ToString();
                ini.INIIO.GetPrivateProfileString("设备起止日期", "终止日期", "2014/2/16", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                sbendtime = temp.ToString();
                ini.INIIO.GetPrivateProfileString("员工起止日期", "起始日期", "2014/2/10", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                ygstarttime = temp.ToString();
                ini.INIIO.GetPrivateProfileString("员工起止日期", "终止日期", "2014/2/16", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                ygendtime = temp.ToString();
            }
            catch
            { }
        }
        private void init_control()
        {
            //simpleButton1.Parent = pictureBox2;
            //simpleButtonCheckIsExist.Parent = pictureBox2;
            //pictureBox3.Parent = pictureBox2;
            buttonMainTest.Parent=pictureBox1;
            this.toolTip1.SetToolTip(this.buttonMainTest, "底盘测功机");
            buttonSql.Parent = pictureBox1;
            this.toolTip1.SetToolTip(this.buttonSql, "报表查询");
            buttonStationSetting.Parent = pictureBox1;
            this.toolTip1.SetToolTip(this.buttonStationSetting, "检测线设置");
            buttonSystemExit.Parent = pictureBox1;
            this.toolTip1.SetToolTip(this.buttonSystemExit, "退出系统");
            buttonSystemSetting.Parent = pictureBox1;
            this.toolTip1.SetToolTip(this.buttonSystemSetting, "系统设置");
            buttonIg195Test.Parent = pictureBox1;
            this.toolTip1.SetToolTip(this.buttonIg195Test, "进入排放检测");
            button1.Parent = pictureBox1;
            this.toolTip1.SetToolTip(this.button1, "废气仪标定");
            buttonHuanbaoSetting.Parent = pictureBox1;
            this.toolTip1.SetToolTip(this.buttonHuanbaoSetting, "环保局设置");
            buttonFueltankcapDet.Parent = pictureBox1;
            this.toolTip1.SetToolTip(this.buttonFueltankcapDet, "泄漏检测");
            //= pictureBox2;
            //buttonMainTest.Visible = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now; //当前时间的实例;
            string nowtime = dt.ToString();
            labelTime.Text = nowtime;

        }

        private void Mainform_closing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
            this.Close();
        }
        private void disabled_panel()
        {
            buttonIg195Test.Enabled=false;
            button1.Enabled=false;
            buttonHuanbaoSetting.Enabled=false;
            buttonFueltankcapDet.Enabled=false;
            buttonStationSetting.Enabled=false;
            buttonSystemSetting.Enabled=false;
            buttonMainTest.Enabled=false;
            buttonSql.Enabled = false;
        }
        private void enable_panel()
        {
            buttonIg195Test.Enabled = true;
            button1.Enabled = true;
            buttonHuanbaoSetting.Enabled = true;
            buttonFueltankcapDet.Enabled = true;
            buttonStationSetting.Enabled = true;
            buttonSystemSetting.Enabled = true;
            buttonSql.Enabled = true;
            buttonMainTest.Enabled = true;
        }

        private void buttonSystemSetting_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            systemConfig newsystemConfig = new systemConfig();
            newsystemConfig.ShowDialog();
            timer1.Start();
        }

        private void simpleButtonCheckIsExist_Click(object sender, EventArgs e)
        {
            Login newlogin = new Login();
            newlogin.ShowDialog();
            //newlogin.ShowDialog();
            if (loginSuccess)
            {
                labelUserName.Text = loginYg.User_Name;
                enable_panel();
            }
            else
            {
                labelUserName.Text ="未登录";
                disabled_panel();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            loginSuccess = false;
            labelUserName.Text = "未登录";
            disabled_panel();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            sqlDataView newsqlDataView = new sqlDataView();
            newsqlDataView.ShowDialog();
            timer1.Start();
        }

        private void buttonSystemExit_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
            this.Close();
        }

        private void buttonStationSetting_Click(object sender, EventArgs e)
        {
            detectLine childzyjspanel = new detectLine();
            childzyjspanel.ShowDialog();
        }

        private void buttonHuanbaoSetting_Click(object sender, EventArgs e)
        {
            huanBaoLogin hbloginpanel = new huanBaoLogin();
            hbloginpanel.ShowDialog();
            if (huanBaoLogin)
            {
                huanBaoConfig childHuanbao = new huanBaoConfig();
                childHuanbao.ShowDialog();
            }
        }

        
    }
}
