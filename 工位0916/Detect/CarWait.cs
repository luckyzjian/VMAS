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
using System.Management;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.IO.Ports;
using ini;
using LedControl;
//using word = Microsoft.Office.Interop.Word;//添加对WORD类库的引用

namespace Detect
{
    public partial class CarWait : Form
    {
        //public static string jczNumber = "01";
        public static JCZXXB jczxxb = new JCZXXB();
        public static JCZdal jczxx = new JCZdal();
        private bool disable_panel = true;
        private int selectedTestCar = 0;
        public delegate void wtlsb(Label Msgowner, string Msgstr, bool Update_DB);      //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                     //委托
        public delegate void wtcs(Control controlname, string text);                    //委托
        public string[] selectID = new string[1024];                                    //当前等待车辆选中的列表
        public bool ref_zt = true;                                                      //dataGrid_waitcar_SelectionChanged_1函数使能标记
        public static string ip;                                                        //本机IP                                      
        public static BJCLXXB bjcl = null;                                              //被检车辆信息Model
        public static JCXGXXB jcxg = null;                                              //检测相关数据
        DataTable dt_wait=null;                                                         //等待车辆列表
        public static BJCLXX bjclxx = new BJCLXX();                                     //被检车辆信息方法集dal
        public static JCXGXX jcxgxx = new JCXGXX();                                     //被检车辆信息方法集dal
        public static SBXX sbxx = new SBXX();                                           //设备信息方法集dal
        public static SYS_DAL.XXXXB xxxxbdal = new SYS_DAL.XXXXB();                     //消息信息方法集dal
        public static Main_dal maindal = new Main_dal();                                //Main窗体的方法集dal
        tool tool = new tool();                                                         //方法集
        public static JCXXX jcxxx = new JCXXX();                                                      //检测线方法集
        public static JCXXXB jcxxxb = new JCXXXB();                                     //检测线信息Model
        public static SYS.Model.SYSConfig sys = new SYS.Model.SYSConfig();              //系统配置Model
        public static SBXXB fqfxy = new SBXXB();                                        //本线所用废气分析仪信息
        public static SBXXB zsj = new SBXXB();
        public static SBXXB hjz = new SBXXB();
        public static SBXXB pc = new SBXXB();
        public static SBXXB btgydj = new SBXXB();                                       //本线所用不透光仪
        public static SBXXB llj = new SBXXB();
        public static SBXXB led = new SBXXB();
        DataTable Jccl_temp = null;                                                     //检测车辆所有信息datatable
        public static Zyjsdal zyjsdal = new Zyjsdal();                                  //自由加速方法集dal
        public static ASMdal asmdal = new ASMdal();                                     //asm方法集dal
        public static VMASdal vmasdal = new VMASdal();
        public static JZJSdal jzjsdal = new JZJSdal();                                  //jzjs方法集dal
        public static SDSdal sdsdal = new SDSdal();                                     //sds方法集dal
        public static Thread TH_CGJ = null;                                             //测功机线程
        public static float CGJ_Jzgl = 0;                                               //测功机加载功率
        public static bool CGJ_Th_st = true;                                            //测功机线程状态
        public static int CGJ_CH = 1;                                                   //测功机通道
        C812PGdll.C812pg _812pg = new C812PGdll.C812pg();                               //812通讯类
        //public static Exhaust.Exhaust_exe exhaust_exe = new Exhaust.Exhaust_exe();     //废气分析仪通讯类
        public static Exhaust.MQW_50A mqw_50A = null;                                   //名泉MQW_50A废气分析仪方法集
        public static Exhaust.Fla502 fla_502 = null;
        public static Exhaust.Fla501 fla_501 = null;
        public static Exhaust.Flv_1000 flv_1000 = null;
        public static Exhaust.MQY_200 mqy_200 = null;                                   //名泉MQY_300烟度计方法集
        public static Exhaust.FLB_100 Flb_100 = null;
        public static led ledcontrol = null; 
        public static GBDal gbdal = new GBDal();                                        //国标配置数据层
        public static SBXXB cgj = new SBXXB();                                          //本线所用废气分析仪信息
        public static Dynamometer.IGBT igbt = null;                                     //测功机控制模块通讯类
        public Thread th_fqsscl = null;                                                 //废气实时测量测试用线程
        public Thread th_lljcl = null;
        public Thread th_ydjsscl = null;
        public Thread th_load = null;

        public int Carwait_Scroll = 0;                                                  //待检车滚动条位置
        public string Online_Order = "";                                                //上线命令    
        public static String Customer = "无";                                           //客户端注释
        public Thread thread_TCP = null;                                                //上线线程
        public Thread thread_UPD = null;                                                //UDP监听线程
        public Thread thread_initpanel = null;                                                //UDP监听线程
        public static int Port = 8001;                                                  //默认上线端口
        public static int UDP_Port_Send = 9999;                                         //默认UDP发送端口
        public static int UDP_Port = 9998;                                              //默认UDP接收端口
        public static String Host = "192.168.1.2";                                      //默认主控端地址
        public TcpClient Tcp_Client;                                                    //TCP连接
        public TcpListener Lis;                                                         //TCP监听
        public UdpClient UDP_Client;                                                    //UDP监听
        public UdpClient UDP_Client_Send;                                               //UDP发送
        public NetworkStream Ns;                                                        //网络流
        public Socket socket;                                                           //套接字
        public static IPEndPoint ap;                                                    //IP……shishi
        public static string Errorlist="";                                              //错误串
        public static bool Init_Flag = false;                                           //系统初始化标记
        public static string Jcjsy = "";                                                //检测驾驶员
        public static string Jcczy = "";                                                //检测操作员
        public static string wd = "26";                                                 //温度
        public static string sd = "25";                                                 //湿度
        public static string dqy = "101";                                                 //大气压
        public static float Speed = 0;                                                  //车速
        public static float Force = 0;                                                  //扭矩
        public static float Power = 0;                                                  //功率
        public static float Duty = 0;                                                   //占空比
        public static string UseMK = "BNTD";                                            //使用什么模块
        public static string UseFqy = "FLA_502";                                        //使用什么型号的废气分析仪
        public static string UseLlj = "FLV_1000";
        public static bool yj_result = true;
        public static string yj_result_inf = "";
        public static bool ifChaoCha = true;
        public static bool ifLianxuChaoCha = true;
        public static float jzjs_lowSpeed = 60f;
        public static float jzjs_highSpeed = 80f;
        public static float sds_chaocha = 200f; 
        public static bool jzjs_iflbzs = false;
        public static bool jzjs_iflbgl = true;
        public static int flv_520_delaytime = 8;
        public bool init_panel_ok = false;

        public static string jcjsy = "";
        public static string jcczy = "";
        public static string shy = "";
        public static string pzr = "";

        public static System.Windows.Forms.Screen[] sc;
        public static int sc_width = 0;
        public static int sc_height = 0;
        public static bool hjo2_check = false;
        public static bool cywq_check = true;
        public CarWait()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }
        private void init_loginPanel()
        {
            carLogin childvmaspanel = new carLogin();
            childvmaspanel.TopLevel = false;
            childvmaspanel.Dock = System.Windows.Forms.DockStyle.Fill;
            childvmaspanel.FormBorderStyle = FormBorderStyle.None;
            this.panel_login.Controls.Clear();
            this.panel_login.Controls.Add(childvmaspanel);
            childvmaspanel.Show();
        }
        private void vmas_loginPanel(bool display_inf, string jcclph)
        {
            vmasLogin childvmaspanel = new vmasLogin();
            childvmaspanel.TopLevel = false;
            childvmaspanel.Dock = System.Windows.Forms.DockStyle.Fill;
            childvmaspanel.FormBorderStyle = FormBorderStyle.None;
            this.panel_login.Controls.Clear();
            this.panel_login.Controls.Add(childvmaspanel);
            childvmaspanel.Show();
            if (display_inf)
            {
                childvmaspanel.ref_LoginPanel(jcclph);
            }
        }
        private void jzjs_loginPanel(bool display_inf, string jcclph)
        {
            JzjsLogin childvmaspanel = new JzjsLogin();
            childvmaspanel.TopLevel = false;
            childvmaspanel.Dock = System.Windows.Forms.DockStyle.Fill;
            childvmaspanel.FormBorderStyle = FormBorderStyle.None;
            this.panel_login.Controls.Clear();
            this.panel_login.Controls.Add(childvmaspanel);
            childvmaspanel.Show();
            if (display_inf)
            {
                childvmaspanel.ref_LoginPanel(jcclph);
            }
        }
        private void sds_loginPanel(bool display_inf, string jcclph)
        {
            SdsLogin childvmaspanel = new SdsLogin();
            childvmaspanel.TopLevel = false;
            childvmaspanel.Dock = System.Windows.Forms.DockStyle.Fill;
            childvmaspanel.FormBorderStyle = FormBorderStyle.None;
            this.panel_login.Controls.Clear();
            this.panel_login.Controls.Add(childvmaspanel);
            childvmaspanel.Show();
            if (display_inf)
            {
                childvmaspanel.ref_LoginPanel(jcclph);
            }
        }
        private void zyjs_loginPanel(bool display_inf,string jcclph)
        {
            ZyjsLogin childvmaspanel = new ZyjsLogin();
            childvmaspanel.TopLevel = false;
            childvmaspanel.Dock = System.Windows.Forms.DockStyle.Fill;
            childvmaspanel.FormBorderStyle = FormBorderStyle.None;
            this.panel_login.Controls.Clear();
            this.panel_login.Controls.Add(childvmaspanel);
            childvmaspanel.Show();
            if (display_inf)
            {
                childvmaspanel.ref_LoginPanel(jcclph);
            }
        }
        private void CarWait_Load(object sender, EventArgs e)
        {
            //float a = 99 / 100f;
            Thread th_load = new Thread(load_progress);
            jczxxb = jczxx.Get_jczxx();
            Init_sc();
            th_load.Start();
            ip = tool.getIp();                          //获取本机ip
            //Init_CGJ();                               //初始化测功机
            jcxxxb = jcxxx.GetModel(ip);                //初始化检测线
            if (jcxxxb.JCXBH != "-2")                     //在服务器找到了本机IP配置的检测线
            {
                this.Text = jcxxxb.JCXMC;
                Init_test();
                Init_SB();                              //初始化设备
                Init_COM();                             //初始化串口
                
                //Init_Login();                           //初始化登记界面
                init_loginPanel();
                ref_WaitCar();
            }
            else
            {
                panel_sb_fq.Enabled = disable_panel;
                panel_sb_yd.Enabled = disable_panel;
                panel_sb_cg.Enabled = disable_panel;
                panel_sb_llj.Enabled = disable_panel;
                panel_kz.Enabled = disable_panel;
                panel2.Enabled = disable_panel;
                panel_login.Enabled = disable_panel;
                Errorlist += "本机" + ip+"未配置 ";
                MessageBox.Show("本机" + ip + "在配置之前，无法作为检测线使用。", "系统提示");
            }
            sys = maindal.Get_SYSConfig();
            timer1.Interval = 1000;
            timer1.Start();
            Online();
            init_panel_ok = true;//上线
        }
        public void Init_sc()
        {
            sc = System.Windows.Forms.Screen.AllScreens;
            sc_height = this.Height;
            sc_width = this.Width;
        }
        public void Init_test()
        {
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            try
            {
                INIIO.GetPrivateProfileString("TEST", "IfChaoCha", "true", temp, 2048, @".\Config.ini");
                if (temp.ToString() == "true")
                    ifChaoCha = true;
                else
                    ifChaoCha = false;
                INIIO.GetPrivateProfileString("TEST", "IfLianxuChaoCha", "true", temp, 2048, @".\Config.ini");
                if (temp.ToString() == "true")
                    ifLianxuChaoCha = true;
                else
                    ifLianxuChaoCha = false;
                INIIO.GetPrivateProfileString("TEST", "VMAS", "true", temp, 2048, @".\Config.ini");
                if (temp.ToString() == "true")
                    radioButtonVmas.Enabled = true;
                else
                    radioButtonVmas.Enabled = false;
                INIIO.GetPrivateProfileString("TEST", "JZJS", "true", temp, 2048, @".\Config.ini");
                if (temp.ToString() == "true")
                    radioButtonLugdown.Enabled = true;
                else
                    radioButtonLugdown.Enabled = false;
                INIIO.GetPrivateProfileString("TEST", "SDS", "true", temp, 2048, @".\Config.ini");
                if (temp.ToString() == "true")
                    radioButtonSds.Enabled = true;
                else
                    radioButtonSds.Enabled = false;
                INIIO.GetPrivateProfileString("TEST", "ZYJS", "true", temp, 2048, @".\Config.ini");
                if (temp.ToString() == "true")
                    radioButtonZyjs.Enabled = true;
                else
                    radioButtonZyjs.Enabled = false;
                INIIO.GetPrivateProfileString("TEST", "JZJS_LOWSPEED", "60", temp, 2048, @".\Config.ini");
                jzjs_lowSpeed = float.Parse(temp.ToString());
                INIIO.GetPrivateProfileString("TEST", "JZJS_HIGHSPEED", "80", temp, 2048, @".\Config.ini");
                jzjs_highSpeed = float.Parse(temp.ToString());
                INIIO.GetPrivateProfileString("TEST", "JZJS_IFLBZS", "true", temp, 2048, @".\Config.ini");
                if (temp.ToString() == "true")
                    jzjs_iflbzs = true;
                else
                    jzjs_iflbzs = false;

                INIIO.GetPrivateProfileString("TEST", "检查环境氧", "否", temp, 2048, @".\Config.ini");
                if (temp.ToString() == "是")
                    hjo2_check = true;
                else
                    hjo2_check = false;

                INIIO.GetPrivateProfileString("TEST", "检查残余尾气", "是", temp, 2048, @".\Config.ini");
                if (temp.ToString() == "是")
                    cywq_check = true;
                else
                    cywq_check = false;

                INIIO.GetPrivateProfileString("TEST", "是否屏蔽未配置项", "是", temp, 2048, @".\Config.ini");
                if (temp.ToString() == "是")
                    disable_panel = false;
                else
                    disable_panel = true;
                
                INIIO.GetPrivateProfileString("TEST", "JZJS_IFLBGL", "true", temp, 2048, @".\Config.ini");
                if (temp.ToString() == "true")
                    jzjs_iflbgl = true;
                else
                    jzjs_iflbgl = false;
                INIIO.GetPrivateProfileString("TEST", "Flv_502_delaytime", "8", temp, 2048, @".\Config.ini");
                flv_520_delaytime = int.Parse(temp.ToString());
                INIIO.GetPrivateProfileString("TEST", "SDS_CHAOCHA", "200", temp, 2048, @".\Config.ini");
                sds_chaocha = float.Parse(temp.ToString());
                INIIO.GetPrivateProfileString("人员配置", "检测驾驶员", " ", temp, 2048, @".\Config.ini");
                Jcjsy = temp.ToString();
                INIIO.GetPrivateProfileString("人员配置", "检测操作员", " ", temp, 2048, @".\Config.ini");
                Jcczy = temp.ToString();
                INIIO.GetPrivateProfileString("人员配置", "审核", " ", temp, 2048, @".\Config.ini");
                shy = temp.ToString();
                INIIO.GetPrivateProfileString("人员配置", "批准人", " ", temp, 2048, @".\Config.ini");
                pzr = temp.ToString();
            }
            catch
            {
                MessageBox.Show("读取测试配置信息出错", "系统提示");
                ifChaoCha = true;
                ifLianxuChaoCha = true;
                jzjs_lowSpeed = 60f;
                jzjs_highSpeed = 80f;
                jzjs_iflbzs = false;
                jzjs_iflbgl = true;
            }
        }
        public void load_progress()
        {
            //this.Enabled = false;
            load_display new_loadProgress = new load_display();
            new_loadProgress.Show();
            Thread.Sleep(100);
            new_loadProgress.progress_show();
            new_loadProgress.Close();
            //this.Enabled = true;
         //   this.TopLevel = true;
        }
        public void Init_SB()
        {
            hjz = sbxx.Get_sb_by_bh(jcxxxb.HJZBH);//将LEDDPBH项用做环境站
            pc = sbxx.Get_sb_by_bh(jcxxxb.PCBH);
            zsj = sbxx.Get_sb_by_bh(jcxxxb.WYZSBBH);
            fqfxy = sbxx.Get_sb_by_bh(jcxxxb.FQFXYBH);
            if (fqfxy.SBBH == 0)
            {
                Errorlist += "本线没有配置废气分析仪 ";
                Msg(Msg_FQ, panel_fq, "该检测线未配置该设备", false);
                panel_sb_fq.Enabled = disable_panel;
            }
            else
            {
                Msg(Msg_FQ, panel_fq, "该检测线有配置该设备", false);
            }
            cgj = sbxx.Get_sb_by_bh(jcxxxb.DPCGJBH);
            if (cgj.SBBH == 0)
            {
                Errorlist += "本线没有配置底盘测功机 ";
                Msg(Msg_cg, panel_cg, "该检测线未配置测功机", false);
                panel_sb_cg.Enabled = disable_panel;
                //button_control.Enabled = disable_panel;
                //Demarcate.Enabled = false;
            }
            else
                try
                {
                    UseMK = cgj.SBMS.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    Msg(Msg_cg, panel_cg, "该检测线有配置测功机", false);
                }
                catch (Exception)
                {
                    UseMK = "BNTD";
                }

            btgydj = sbxx.Get_sb_by_bh(jcxxxb.BTGYDJBH);
            if (btgydj.SBBH == 0)
            {
                Errorlist += "本线没有配置不透光 ";
                Msg(Msg_yd, panel_yd, "该检测线未配置烟度计", false);
                panel_sb_yd.Enabled = disable_panel;
            }
            else
            {
                Msg(Msg_yd, panel_yd, "该检测线有配置烟度计", false);
            }
            llj = sbxx.Get_sb_by_bh(jcxxxb.LLJBH);
            if (llj.SBBH == 0)
            {
                Errorlist += "本线没有配置流量计 ";
                Msg(Msg_llj, panel_llj, "该检测线未配置流量计", false);
                panel_sb_llj.Enabled = disable_panel;
            }
            else
            {
                Msg(Msg_llj, panel_llj, "该检测线有配置流量计", false);
            }
            led = sbxx.Get_sb_by_bh(jcxxxb.LEDDPBH);
        }

        public void Init_COM()
        {
            try
            {
                if (jcxxxb.FQFXYPZ != null&&jcxxxb.FQFXYBH!=0)
                    switch (jcxxxb.FQFXYPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[2])           //通过仪器型号选择初始化项目
                    {
                        case "MQW-50A":
                            try
                            {
                                UseFqy = "MQW-50A";
                                mqw_50A = new Exhaust.MQW_50A();
                                if (mqw_50A.Init_Comm(jcxxxb.FQFXYPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0], jcxxxb.FQFXYPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1]))
                                {
                                    //mqw_50A.Auto_Zeroing();       //设置自动调零
                                    Init_Flag = true;               //初始化串口成功
                                }
                                else
                                {
                                    mqw_50A = null;
                                    Init_Flag = false;
                                    panel_sb_fq.Enabled = disable_panel;
                                    panel_kz.Enabled = disable_panel;
                                    panel2.Enabled = disable_panel;
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                panel_sb_fq.Enabled = disable_panel;
                                panel_kz.Enabled = disable_panel;
                                panel2.Enabled = disable_panel;
                            }
                            break;
                        case "FLA_502":
                            try
                            {
                                UseFqy = "FLA_502";
                                fla_502 = new Exhaust.Fla502();
                                if (fla_502.Init_Comm(jcxxxb.FQFXYPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0], jcxxxb.FQFXYPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1]))
                                {
                                    //mqw_50A.Auto_Zeroing();       //设置自动调零
                                    Init_Flag = true;               //初始化串口成功
                                }
                                else
                                {
                                    fla_502 = null;
                                    Init_Flag = false;
                                    panel_sb_fq.Enabled = disable_panel;
                                    panel_kz.Enabled = disable_panel;
                                    panel2.Enabled = disable_panel;
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                panel_sb_fq.Enabled = disable_panel;
                                panel_kz.Enabled = disable_panel;
                                panel2.Enabled = disable_panel;
                            }
                            break;
                        case "FLA_501":
                            try
                            {
                                UseFqy = "FLA_501";
                                fla_501 = new Exhaust.Fla501();
                                if (fla_501.Init_Comm(jcxxxb.FQFXYPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0], jcxxxb.FQFXYPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1]))
                                {
                                    //mqw_50A.Auto_Zeroing();       //设置自动调零
                                    Init_Flag = true;               //初始化串口成功
                                }
                                else
                                {
                                    fla_501 = null;
                                    Init_Flag = false;
                                    panel_sb_fq.Enabled = disable_panel;
                                    panel_kz.Enabled = disable_panel;
                                    panel2.Enabled = disable_panel;
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                panel_sb_fq.Enabled = disable_panel;
                                panel_kz.Enabled = disable_panel;
                                panel2.Enabled = disable_panel;
                            }
                            break;
                    }
            }
            catch (Exception)
            {
                panel_sb_fq.Enabled = disable_panel;
                panel_kz.Enabled = disable_panel;
                panel2.Enabled = disable_panel;
            }

            //这里只初始化了废气分析仪其他设备要继续初始化
            try
            {
                if (jcxxxb.DPCGJPZ != null&&jcxxxb.DPCGJBH!=0)
                    switch (jcxxxb.DPCGJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[2])           //通过仪器型号选择初始化项目
                    {
                        case "IGBT":
                            try
                            {
                                igbt = new Dynamometer.IGBT("IGBT");
                                if (igbt.Init_Comm(jcxxxb.DPCGJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0], jcxxxb.BTGYDJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1]))
                                {
                                    //mqw_50A.Auto_Zeroing();       //设置自动调零
                                    Init_Flag = true;               //初始化串口成功
                                }
                                else
                                {
                                    panel_sb_cg.Enabled = disable_panel;
                                    //button_control.Enabled = disable_panel;
                                    //Demarcate.Enabled = false;
                                    igbt = null;
                                    Init_Flag = false;
                                }
                            }
                            catch (Exception er)
                            {
                                panel_sb_cg.Enabled = disable_panel;
                                //button_control.Enabled = disable_panel;
                                //Demarcate.Enabled = false;
                                MessageBox.Show(er.ToString(), "出错啦");
                            }
                            break;
                        case "BNTD":
                            try
                            {
                                igbt = new Dynamometer.IGBT("BNTD");
                                if (igbt.Init_Comm(jcxxxb.DPCGJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0], jcxxxb.DPCGJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1]))
                                {
                                    //mqw_50A.Auto_Zeroing();       //设置自动调零
                                    Init_Flag = true;               //初始化串口成功
                                }
                                else
                                {
                                    panel_sb_cg.Enabled = disable_panel;
                                    //button_control.Enabled = disable_panel;
                                    //Demarcate.Enabled = false;
                                    igbt = null;
                                    Init_Flag = false;
                                }
                            }
                            catch (Exception er)
                            {
                                panel_sb_cg.Enabled = disable_panel;
                                //button_control.Enabled = disable_panel;
                                //Demarcate.Enabled = false;
                                MessageBox.Show(er.ToString(), "出错啦");
                            }
                            break;
                    }
            }
            catch (Exception)
            {
                panel_sb_cg.Enabled = disable_panel;
                //button_control.Enabled = disable_panel;
                //Demarcate.Enabled = false;
            }
            try
            {
                if (jcxxxb.BTGYDJPZ != null&&jcxxxb.BTGYDJBH!=0)
                    switch (jcxxxb.BTGYDJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[2])           //通过仪器型号选择初始化项目
                    {
                        case "MQY-200":
                            try
                            {
                                mqy_200 = new Exhaust.MQY_200();
                                if (mqy_200.Init_Comm(jcxxxb.BTGYDJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0], jcxxxb.BTGYDJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1]))
                                {
                                    //mqw_50A.Auto_Zeroing();       //设置自动调零
                                    Init_Flag = true;               //初始化串口成功
                                }
                                else
                                {
                                    mqy_200 = null;
                                    Init_Flag = false;
                                    panel_sb_yd.Enabled = disable_panel;
                                    panel_kz.Enabled = disable_panel;
                                    panel2.Enabled = disable_panel;
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                panel_sb_yd.Enabled = disable_panel;
                                panel_kz.Enabled = disable_panel;
                                panel2.Enabled = disable_panel;
                            }
                            break;
                        case "FLB_100":
                            try
                            {
                                Flb_100 = new Exhaust.FLB_100();
                                if (Flb_100.Init_Comm(jcxxxb.BTGYDJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0], jcxxxb.BTGYDJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1]))
                                {
                                    //mqw_50A.Auto_Zeroing();       //设置自动调零
                                    Init_Flag = true;               //初始化串口成功
                                }
                                else
                                {
                                    Flb_100 = null;
                                    Init_Flag = false;
                                    panel_sb_yd.Enabled = disable_panel;
                                    panel_kz.Enabled = disable_panel;
                                    panel2.Enabled = disable_panel;
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                panel_sb_yd.Enabled = disable_panel;
                                panel_kz.Enabled = disable_panel;
                                panel2.Enabled = disable_panel;
                            }
                            break;
                    }
            }
            catch (Exception)
            {
                panel_sb_yd.Enabled = disable_panel;
                panel_kz.Enabled = disable_panel;
                panel2.Enabled = disable_panel;
            }
            try
            {
                if (jcxxxb.LLJPZ != null&&jcxxxb.LLJBH!=0)
                    switch (jcxxxb.LLJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[2])           //通过仪器型号选择初始化项目
                    {
                        case "FLV_1000":
                            try
                            {
                                flv_1000 = new Exhaust.Flv_1000();
                                if (flv_1000.Init_Comm(jcxxxb.LLJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0], jcxxxb.LLJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1]))
                                {
                                    //mqw_50A.Auto_Zeroing();       //设置自动调零
                                    Init_Flag = true;               //初始化串口成功
                                }
                                else
                                {
                                    flv_1000 = null;
                                    Init_Flag = false;
                                    panel_sb_llj.Enabled = disable_panel;
                                    panel_llj.Enabled = disable_panel;
                                    panel2.Enabled = disable_panel;
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                panel_sb_llj.Enabled = disable_panel;
                                panel_llj.Enabled = disable_panel;
                                panel2.Enabled = disable_panel;
                            }
                            break;
                    }
            }
            catch (Exception)
            {
                panel_sb_llj.Enabled = disable_panel;
                panel_llj.Enabled = disable_panel;
                panel2.Enabled = disable_panel;
            }
            try
            {
                if (jcxxxb.LEDPZ != null&&jcxxxb.LEDDPBH!=0)
                {
                    try
                    {
                        ledcontrol = new led();
                        if (ledcontrol.Init_Comm(jcxxxb.LEDPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0], jcxxxb.LEDPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1]))
                        {
                            //mqw_50A.Auto_Zeroing();       //设置自动调零
                            Init_Flag = true;               //初始化串口成功
                        }
                        else
                        {
                            ledcontrol = null;
                            Init_Flag = false;
                        }
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show(er.ToString(), "出错啦");
                    }
                }

            }
            catch (Exception)
            {
            }
        }
        public void ref_WaitCar()
        {
            try
            {
                dt_wait = new DataTable();
                dt_wait.Columns.Add("检测编号");
                dt_wait.Columns.Add("车牌号");
                dt_wait.Columns.Add("车主姓名");
                dt_wait.Columns.Add("检测方法");
                DataTable dt = bjclxx.getCarWait(jcxxxb.JCXBH);
                DataRow dr = null;
                if (dt != null)
                {
                    foreach (DataRow dR in dt.Rows)
                    {
                        dr = dt_wait.NewRow();
                        dr["检测编号"] = dR["JCBH"].ToString();
                        dr["车牌号"] = dR["JCCLPH"].ToString();
                        //dr["号牌种类"] = dR["PZLX"].ToString();
                        dr["车主姓名"] = dR["CZ"].ToString();
                        //dr["检测次数"] = dR["JCCS"].ToString();
                        dr["检测方法"] = dR["QRJCFF"].ToString();
                        /*if ("9999".Equals(dR["JCBJ"].ToString()))
                        {
                            dr["检测线"] = "不确定检测线";
                        }
                        if ("-1".Equals(dR["JCBJ"].ToString()))
                        {
                            dr["检测线"] = "优先";
                        }
                        if (jcxxxb.JCXBH.ToString().Equals(dR["JCBJ"].ToString()))
                        {
                            dr["检测线"] = "确定本线检测";
                        }
                        dr["状态"] = dR["JCZT"].ToString();*/
                        dt_wait.Rows.Add(dr);
                    }
                }
                ref_zt = false;
                dataGrid_waitcar.DataSource = dt_wait;
                dataGrid_waitcar.Columns["检测编号"].Visible = false;
                //dataGrid_waitcar.Rows[0].Selected = false;
                dataGrid_waitcar.FirstDisplayedScrollingRowIndex = Carwait_Scroll;
                dataGrid_waitcar.Sort(dataGrid_waitcar.Columns["检测编号"], ListSortDirection.Ascending);
                ref_zt = true;
                if (dataGrid_waitcar.SelectedRows.Count != 1)
                {
                    Msg(msg_jcclcp, panel_jcclcp, "请选择一辆车进入测试", false);
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("欢迎光临指导工作", 1);
                        ledcontrol.writeLed("小心驾驶安全第一", 5);
                    }
                }
                else
                {
                    Msg(msg_jcclcp, panel_jcclcp, dataGrid_waitcar.SelectedRows[0].Cells["车牌号"].Value.ToString() + "请上检测线", false);
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed(dataGrid_waitcar.SelectedRows[0].Cells["车牌号"].Value.ToString() + " " + dataGrid_waitcar.SelectedRows[0].Cells["车主姓名"].Value.ToString(), 1);
                        ledcontrol.writeLed("      请上检测线", 5);
                    }
                    bjcl = bjclxx.GetModel(dataGrid_waitcar.SelectedRows[0].Cells["检测编号"].Value.ToString());
                    switch (bjcl.QRJCFF)
                    {
                        case "自由加速法": radioButtonZyjs.Checked = true; break;
                        case "加载减速法": radioButtonLugdown.Checked = true; break;
                        case "简易瞬态工况法": radioButtonVmas.Checked = true; break;
                        case "双怠速法": radioButtonSds.Checked = true; break;
                        default:
                            {
                                MessageBox.Show("该车未注册测试方法,请在界面左上方选择测试方法.", "系统提示");
                                break;
                            }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        //public void Init_CGJ()
        //{
        //    CGJ_Jzgl = 0;
        //    CGJ_Th_st = true;
        //    CGJ_CH = 1;
        //    TH_CGJ = new Thread(CGJ_exe);
        //    TH_CGJ.Start();
        //}

        //public void CGJ_exe()
        //{
        //    _812pg.PWM_OFF_JZ(CGJ_CH);
        //    while (CGJ_Th_st)
        //    {
        //        if (CGJ_Jzgl < 0)
        //            CGJ_Jzgl = 0;
        //        //if(CGJ_Jzgl>  )                                   //测功机加载功率限值
        //        float Multiple=1;
        //        Multiple = float.Parse(Math.Round(CGJ_Jzgl / 25, 2).ToString());                            //假定测功机最大吸收功率为50KW,计算加载系数
        //        if (Multiple > 2)
        //            Multiple = 2;
        //        _812pg.PWM_Signal(Multiple, 100, CGJ_CH);
        //    }

        //    if (!CGJ_Th_st)
        //    {
        //        _812pg.PWM_OFF_JZ(CGJ_CH);
        //    }
        //}

        private void ToolStripMenuItem_fc_Click(object sender, EventArgs e)
        {
            Fc();
        }

        private void 取消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGrid_waitcar.SelectedRows.Count > 0)
            {
                for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                {
                    bjcl = bjclxx.GetModel(dataGrid_waitcar.SelectedRows[i].Cells["检测编号"].Value.ToString());
                    bjclxx.deleteThisCar(bjcl.JCCLPH);
                    //bjcl.JCBJ = "0";    
                    //bjclxx.Update(bjcl);
                }
                ref_WaitCar();
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (dataGrid_waitcar.SelectedRows.Count > 0)
            {
                for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                {
                    bjcl = bjclxx.GetModel(dataGrid_waitcar.SelectedRows[i].Cells["检测编号"].Value.ToString());
                    bjclxx.deleteThisCar(bjcl.JCCLPH);
                }
                ref_WaitCar();
            }
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ref_WaitCar();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ref_WaitCar();
        }

        private void 录入检测员ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ydRylr rylr = new ydRylr();
                rylr.ShowDialog();
                JCXGXXB xgxx = new JCXGXXB();
                if (dataGrid_waitcar.SelectedRows.Count > 0)
                {
                    for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                    {
                        xgxx = jcxgxx.Get_Xgxx(dataGrid_waitcar.SelectedRows[i].Cells["检测编号"].Value.ToString());
                        xgxx.YCY = Jcjsy;
                        xgxx.PFJCY = Jcczy;
                        jcxgxx.Save_Xgxx(xgxx);
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("系统提醒", er.ToString());
            }
        }

        private void dataGrid_waitcar_SelectionChanged_1(object sender, EventArgs e)
        {
            if (ref_zt)
            {
                if (dataGrid_waitcar.SelectedRows.Count > 0)
                {
                    if (dataGrid_waitcar.SelectedRows.Count == 1)
                    {
                        ToolStripMenuItem_fc.Enabled = true;
                        ToolStripMenuItemqx.Enabled = true;
                        selectID = new string[1024];
                        for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                        {
                            selectID[i] = dataGrid_waitcar.SelectedRows[i].Cells["检测编号"].Value.ToString();
                        }
                        Msg(msg_jcclcp, panel_jcclcp, dataGrid_waitcar.SelectedRows[0].Cells["车牌号"].Value.ToString() + "请上检测线", false);
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed(dataGrid_waitcar.SelectedRows[0].Cells["车牌号"].Value.ToString() + " " + dataGrid_waitcar.SelectedRows[0].Cells["车主姓名"].Value.ToString(), 1);
                            ledcontrol.writeLed("      请上检测线", 5);
                        }
                        bjcl = bjclxx.GetModel(dataGrid_waitcar.SelectedRows[0].Cells["检测编号"].Value.ToString());
                        switch (bjcl.QRJCFF)
                        {
                            case "自由加速法": radioButtonZyjs.Checked = true; break;
                            case "加载减速法": radioButtonLugdown.Checked = true; break;
                            case "简易瞬态工况法": radioButtonVmas.Checked = true; break;
                            case "双怠速法": radioButtonSds.Checked = true; break;
                            default:
                                {
                                    radioButtonZyjs.Checked = false;
                                    radioButtonLugdown.Checked = false;
                                    radioButtonVmas.Checked = false;
                                    radioButtonSds.Checked = false;
                                    break;
                                }
                        }
                    }
                    else if (dataGrid_waitcar.SelectedRows.Count > 1)
                    {
                        ToolStripMenuItem_fc.Enabled = false;
                        ToolStripMenuItemqx.Enabled = true;
                        selectID = new string[1024];
                        for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                        {
                            selectID[i] = dataGrid_waitcar.SelectedRows[i].Cells["检测编号"].Value.ToString();
                        }
                    }
                    else
                    {
                        ToolStripMenuItem_fc.Enabled = false;
                        selectID = new string[1024];
                        for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                        {
                            selectID[i] = dataGrid_waitcar.SelectedRows[i].Cells["检测编号"].Value.ToString();
                        }
                    }
                }
                else
                {
                    selectID = new string[1024];
                    for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                    {
                        selectID[i] = dataGrid_waitcar.SelectedRows[i].Cells["检测编号"].Value.ToString();
                    }
                }
            }
        }
        private void ref_carInfo()
        {
            try
            {
                if (dataGrid_waitcar.SelectedRows.Count == 1)
                {
                    Msg(msg_jcclcp, panel_jcclcp, dataGrid_waitcar.SelectedRows[0].Cells["车牌号"].Value.ToString() + "请上检测线", false);
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed(dataGrid_waitcar.SelectedRows[0].Cells["车牌号"].Value.ToString() + " " + dataGrid_waitcar.SelectedRows[0].Cells["车主姓名"].Value.ToString(), 1);
                        ledcontrol.writeLed("      请上检测线", 5);
                    }
                    bjcl = bjclxx.GetModel(dataGrid_waitcar.SelectedRows[0].Cells["检测编号"].Value.ToString());
                    switch (bjcl.QRJCFF)
                    {
                        case "自由加速法":
                            {
                                radioButtonZyjs.Checked = true;
                                zyjs_loginPanel(true, dataGrid_waitcar.SelectedRows[0].Cells["车牌号"].Value.ToString());
                                break;
                            }
                        case "加载减速法":
                            {
                                radioButtonLugdown.Checked = true;
                                jzjs_loginPanel(true, dataGrid_waitcar.SelectedRows[0].Cells["车牌号"].Value.ToString());
                                break;
                            }
                        case "简易瞬态工况法":
                            {
                                radioButtonVmas.Checked = true;
                                vmas_loginPanel(true, dataGrid_waitcar.SelectedRows[0].Cells["车牌号"].Value.ToString());
                                break;
                            }
                        case "双怠速法":
                            {
                                radioButtonSds.Checked = true;
                                sds_loginPanel(true, dataGrid_waitcar.SelectedRows[0].Cells["车牌号"].Value.ToString());
                                break;
                            }
                        default:
                            {
                                JcffChoose jcffchoose = new JcffChoose();
                                jcffchoose.ShowDialog();
                                bjclxx.updateJcFf(dataGrid_waitcar.SelectedRows[0].Cells["检测编号"].Value.ToString(), jcffchoose.jcff_choose);
                                ref_WaitCar();
                                ref_carInfo();
                                break;
                            }
                    }
                }
            }
            catch
            {
            }
        }
        #region 双击以显示该车信息
        private void SelectThisCar(object sender, DataGridViewCellEventArgs e)
        {
            ref_carInfo();
        }
        #endregion
        private void button2_Click(object sender, EventArgs e)
        {
            Fc();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            //TH_CGJ.Abort();
           // this.close
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                switch (UseMK)
                {
                    case "BNTD":
                        switch (igbt.Status)
                        {
                            default:
                                //Msg(Msg_cs, panel_cs, igbt.Speed, false);
                                Speed = (float)Convert.ToDouble(igbt.Speed);
                                //Msg(Msg_nl, panel_nl, igbt.Force, false);
                                Force = (float)Convert.ToDouble(igbt.Force);
                                //Msg(Msg_gl, panel_gl, igbt.Power, false);
                                Power = (float)Convert.ToDouble(igbt.Power);
                                //Msg(Msg_dl, panel_cp, igbt.Duty, false);
                                Duty = (float)Convert.ToDouble(igbt.Duty);
                                //Msg(Msg_msg, panel_msg, igbt.Msg, false);
                                break;
                        }
                        break;
                    case "IGBT":
                        switch (igbt.Status)
                        {
                            //case "Demarcate":
                            //    Msg(label_ad, panel_ad, igbt.Speed, false);
                            //    break;
                            default:
                                if (igbt.Time_Results.Length == 32)
                                {
                                    //Msg(Msg_cs, panel_cs, igbt.Speed, false);
                                    Speed = (float)Convert.ToDouble(igbt.Speed);
                                    //Msg(Msg_nl, panel_nl, igbt.Force, false);
                                    Force = (float)Convert.ToDouble(igbt.Force);
                                    //Msg(Msg_gl, panel_gl, igbt.Power, false);
                                    Power = (float)Convert.ToDouble(igbt.Power);
                                    //IGBT未返回电流
                                }
                                break;
                        }
                        break;
                }
                //Msg(Msg_cg, panel_cg, igbt.Msg, false);
            }
            catch (Exception)
            {

            }
            //ref_WaitCar();
            //ref_Select();
        }

        public void ref_Select()
        {
            ref_zt = false;
            for (int i = 0; i < dataGrid_waitcar.Rows.Count; i++)
            {
                foreach (string str in selectID)
                {
                    if(str!=null)
                        if (dataGrid_waitcar.Rows[i].Cells["检测编号"].Value.ToString() == str.Substring(0, str.Length - 1) && dataGrid_waitcar.Rows[i].Cells["检测次数"].Value.ToString() == str.Substring(str.Length - 1))
                        {
                            dataGrid_waitcar.Rows[i].Selected = true;
                        }
                }
            }
            ref_zt = true;
        }

        private void Fc()
        {
            try
            {
                if (dataGrid_waitcar.SelectedRows.Count == 1)
                {
                    //if (Init_Flag)
                    //{
                    Jccl_temp = null;
                    Jccl_temp = new DataTable();
                    bjcl = null;
                    bjcl = new BJCLXXB();
                    bjcl = bjclxx.GetModel(dataGrid_waitcar.SelectedRows[0].Cells["检测编号"].Value.ToString());
                    jcxg = jcxgxx.Get_Xgxx(dataGrid_waitcar.SelectedRows[0].Cells["检测编号"].Value.ToString());
                    bjcl.JCRQ = DateTime.Now;
                    bjclxx.updateJcrq(bjcl.JCBH, DateTime.Now.ToString());
                    jcxg.JCBH = bjcl.JCBH;
                    if (bjcl.JCBJ == "-1" || bjcl.JCBJ == "9999" || bjcl.JCBJ == jcxxxb.JCXBH)
                    {
                        if (radioButtonLugdown.Checked == true)
                            bjcl.QRJCFF = "加载减速法";
                        else if (radioButtonSds.Checked == true)
                            bjcl.QRJCFF = "双怠速法";
                        else if (radioButtonVmas.Checked == true)
                            bjcl.QRJCFF = "简易瞬态工况法";
                        else if (radioButtonZyjs.Checked == true)
                            bjcl.QRJCFF = "自由加速法";
                        else
                        {
                            MessageBox.Show("该车未注册测试方法,请先在界面左上方选择测试方法.", "系统提示!");
                            return;
                        }
                        bjclxx.updateJcFf(bjcl.JCBH, bjcl.QRJCFF);
                        switch (bjcl.QRJCFF)
                        {
                            case "自由加速法":
                                if (radioButtonZyjs.Enabled == false)
                                    MessageBox.Show("该线不支持该检测方法。", "系统提示");
                                else
                                {
                                    ydRylr jyjsrylr = new ydRylr();
                                    jyjsrylr.ShowDialog();
                                    jcxg.YCY = Jcjsy;
                                    jcxg.LSH = bjcl.LSH.ToString();
                                    jcxg.PFJCY = Jcczy;
                                    jcxg.JCXBH = jcxxxb.JCXBH.ToString();
                                    jcxg.JCSJ = DateTime.Now;
                                    jcxg.JCCS = 1;
                                    jcxgxx.Save_Xgxx(jcxg);
                                    Zyjs_Btg zyjs_btg = new Zyjs_Btg();
                                    timer1.Enabled = false;
                                    timer2.Enabled = false;
                                    zyjs_btg.ShowDialog();
                                    timer1.Enabled = true;
                                    timer2.Enabled = true;
                                }
                                break;
                            case "双怠速法":
                                if (radioButtonSds.Enabled == false)
                                    MessageBox.Show("该线不支持该检测方法。", "系统提示");
                                else
                                {
                                    ydRylr sdsrylr = new ydRylr();
                                    sdsrylr.ShowDialog();
                                    jcxg.YCY = Jcjsy;
                                    jcxg.LSH = bjcl.LSH.ToString();
                                    jcxg.PFJCY = Jcczy;
                                    jcxg.JCXBH = jcxxxb.JCXBH.ToString();
                                    jcxg.JCSJ = DateTime.Now;
                                    jcxg.JCCS = 1;
                                    jcxgxx.Save_Xgxx(jcxg);
                                    Sds sds = new Sds();
                                    timer1.Enabled = false;
                                    timer2.Enabled = false;
                                    sds.ShowDialog();
                                    timer1.Enabled = true;
                                    timer2.Enabled = true;
                                }
                                break;
                            case "稳态工况法":
                                if (radioButtonZyjs.Enabled == false)
                                    MessageBox.Show("该线不支持该检测方法。", "系统提示");
                                else
                                {
                                    Rylr asmrylr = new Rylr();
                                    asmrylr.ShowDialog();
                                    jcxg.YCY = Jcjsy;
                                    jcxg.LSH = bjcl.LSH.ToString();
                                    jcxg.PFJCY = Jcczy;
                                    jcxg.JCXBH = jcxxxb.JCXBH.ToString();
                                    jcxg.JCSJ = DateTime.Now;
                                    jcxg.JCCS = 1;
                                    jcxgxx.Save_Xgxx(jcxg);
                                    Asm asm = new Asm();
                                    timer1.Enabled = false;
                                    timer2.Enabled = false;
                                    asm.ShowDialog();
                                    timer1.Enabled = true;
                                    timer2.Enabled = true;
                                }
                                break;
                            case "加载减速法":
                                if (radioButtonLugdown.Enabled == false)
                                    MessageBox.Show("该线不支持该检测方法。", "系统提示");
                                else
                                {
                                    Rylr jzjsrylr = new Rylr();
                                    jzjsrylr.ShowDialog();
                                    jcxg.YCY = Jcjsy;
                                    jcxg.LSH = bjcl.LSH.ToString();
                                    jcxg.PFJCY = Jcczy;
                                    jcxg.JCXBH = jcxxxb.JCXBH.ToString();
                                    jcxg.JCSJ = DateTime.Now;
                                    jcxg.JCCS = 1;
                                    jcxgxx.Save_Xgxx(jcxg);
                                    Jzjs jzjs = new Jzjs();
                                    timer1.Enabled = false;
                                    timer2.Enabled = false;
                                    jzjs.ShowDialog();
                                    timer1.Enabled = true;
                                    timer2.Enabled = true;
                                }
                                break;
                            case "简易瞬态工况法":
                                if (radioButtonVmas.Enabled== false)
                                    MessageBox.Show("该线不支持该检测方法。", "系统提示");
                                else
                                {
                                    Rylr vmasrylr = new Rylr();
                                    vmasrylr.ShowDialog();
                                    jcxg.YCY = Jcjsy;
                                    jcxg.LSH = bjcl.LSH.ToString();
                                    jcxg.PFJCY = Jcczy;
                                    jcxg.JCXBH = jcxxxb.JCXBH.ToString();
                                    jcxg.JCSJ = DateTime.Now;
                                    jcxg.JCCS = 1;
                                    jcxgxx.Save_Xgxx(jcxg);
                                    VMAS vmas = new VMAS();
                                    timer1.Enabled = false;
                                    timer2.Enabled = false;
                                    vmas.ShowDialog();
                                    timer1.Enabled = true;
                                    timer2.Enabled = true;
                                }
                                break;
                            default: break;
                        }
                        ref_WaitCar();
                    }
                    else
                    {
                        MessageBox.Show("这辆车可能不能在本线检测！", "系统提示");
                    }
                }
                else
                {
                    MessageBox.Show("请选中一辆需要上线的车辆", "系统提示");
                }
            }
            catch (Exception)
            {
                
            }
        }

        private void CarWait_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                try
                {
                    igbt.closeIgbt();//关闭IGBT中的进程及串口,退出所有控制
                }
                catch
                { }
                try
                {
                    if (th_fqsscl != null)
                    {
                        if (th_fqsscl.IsAlive == true)
                            th_fqsscl.Abort();
                    }
                    if (th_lljcl != null)
                    {
                        if (th_lljcl.IsAlive == true)
                            th_lljcl.Abort();
                    }
                    if (th_ydjsscl != null)
                    {
                        if (th_ydjsscl.IsAlive == true)
                            th_ydjsscl.Abort();
                    }
                }
                catch
                { }
                if (ledcontrol != null)
                {
                    CarWait.ledcontrol.writeLed("欢迎光临", 1);
                    CarWait.ledcontrol.writeLed("        指导工作", 5);
                    //ledcontrol.clearLed_ph();
                }
                timer1.Stop();
                if (Flb_100 != null)
                {
                    if (Flb_100.ComPort_3.IsOpen)
                        Flb_100.ComPort_3.Close();
                }
                if (fla_502 != null)
                {
                    if (fla_502.ComPort_1.IsOpen)
                        fla_502.ComPort_1.Close();
                }
                if (fla_501 != null)
                {
                    if (fla_501.ComPort_1.IsOpen)
                        fla_501.ComPort_1.Close();
                }
                if (flv_1000 != null)
                {
                    if (flv_1000.ComPort_1.IsOpen)
                        flv_1000.ComPort_1.Close();
                }
                if (ledcontrol != null)
                {
                    if (ledcontrol.ComPort_2.IsOpen)
                        ledcontrol.ComPort_2.Close();
                }
                
                
                //System.Environment.Exit(0);
            }
            catch (Exception)
            {
                //System.Environment.Exit(0);
            }
        }

        private void dataGrid_waitcar_Scroll(object sender, ScrollEventArgs e)
        {
            Carwait_Scroll = e.NewValue;
        }

        private void Demarcate_Click(object sender, EventArgs e)
        {
            Detect.Demarcate demarcate = new Demarcate();
            demarcate.ShowDialog();
        }

        #region 网络通讯和硬件访问

        public void Online()
        {
            try
            {
                Online_Order += "西西TEST" + "||";
                this.Get_MypcInfo();
                thread_TCP = new Thread(new ThreadStart(this.Send_Online_Message));
                thread_TCP.Start();
                UDP_Client = new UdpClient(UDP_Port);
                ap = new IPEndPoint(IPAddress.Any, 0); 
                thread_UPD = new Thread(UDP_Trans);
                thread_UPD.Start();
            }
            catch (Exception)
            {
            }
            
        }

        public void Link_UDP()
        {
            try
            {
                this.UDP_Client_Send = new UdpClient();
                this.UDP_Client_Send.Connect(Host, UDP_Port_Send);
            }
            catch (Exception)
            {
            }
            
        }

        /// <summary>
        /// 发送UDP数据
        /// </summary>
        /// <param name="Ms"></param>
        public void Send_UDP_Data(MemoryStream Ms)  
        {

            //如果没连接上了
            if (!this.UDP_Client_Send.Client.Connected)
                Link_UDP();
            int Len = 0;
            byte[] bb = new byte[4096];
            Ms.Position = 0;
            while ((Len = Ms.Read(bb, 0, bb.Length)) > 0)
            {
                this.UDP_Client_Send.Send(bb, bb.Length);
                Thread.Sleep(1);
            }
            //发送结尾符
            //Thread.Sleep(500);
            this.UDP_Client_Send.Send(Encoding.Default.GetBytes("**End**"), Encoding.Default.GetBytes("**End**").Length);

        }

        public void UDP_Trans()
        {
            int Flag = 0;
            while (true)
            {
                using (MemoryStream Ms = new MemoryStream())
                {
                    Flag = 0;
                    try
                    {
                        while (Flag == 0)
                        {
                            byte[] bb = null; ;
                            if (Flag == 0)
                            {
                                bb = UDP_Client.Receive(ref ap);
                                Ms.Write(bb, 0, bb.Length);
                                Ms.Flush();
                            }
                            if (Encoding.Default.GetString(bb) == "**End**")
                            {
                                Flag = 1;
                                //如果一张图传送完毕，则将完整流转换成Image
                                Image img = Image.FromStream(Ms);
                                //将Image打入到PictureBox中
                                //this.img = img;
                                //this.BeginInvoke(new wt(this.Copy_Image));
                                //this.Copy_Image();
                            }
                        }
                    }
                    catch (Exception er)
                    { };
                }
            }
        }

        public void Send_Online_Message()
        {
            this.Tcp_Client = new TcpClient();
            //尝试连接
            while (true)    //一旦开始连接 知道连接成功后才退出连接单元
            {
                try
                {
                    this.Tcp_Client.Connect(Host, Port);
                }
                catch (Exception er)
                {
                    if (er.Message.ToString() == "在一个已经连接的套接字上做了一个连接请求。")
                        break;
                }

            }
            //如果连接上了
            if (this.Tcp_Client.Connected)
            {
                //得到套接字原型
                this.socket = this.Tcp_Client.Client;
                this.Ns = new NetworkStream(this.socket);
                //发送上线请求
                this.Ns.Write(Encoding.Default.GetBytes(this.Online_Order), 0, Encoding.Default.GetBytes(this.Online_Order).Length);
                this.Ns.Flush();
                //如果请求发出后套接字仍然处于连接状态
                //则单劈出一个线程，用于接收命令
                if (this.socket.Connected)
                {
                    thread_TCP = new Thread(Get_Order);
                    //thread_TCP.IsBackground = true;
                    thread_TCP.Start();
                }
            }
        }

        public void Get_Order()
        {
            while (true)
            {
                try
                {
                    byte[] bb = new byte[1024];
                    //接收命令
                    int Order_Len = this.Ns.Read(bb, 0, bb.Length);
                    //得到主控端发来的命令集合
                    String[] Order_Set = Encoding.Default.GetString(bb, 0, Order_Len).Split(new String[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    this.Order_Catcher(Order_Set);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                    this.socket.Close();
                    this.Ns.Close();
                    this.Tcp_Client.Close();
                    this.Send_Online_Message();
                    break;
                }
            }
        }

        /// <summary>
        /// 此方法用于判断命令结构
        /// 根据不同的命令调用不同的方法进行处理
        /// </summary>
        /// <param name="Order_Set"></param>
        public void Order_Catcher(String[] Order)
        {
            switch (Order[0])
            {
            }
        }

        /// <summary>
        /// 此方法通过Windows WMI 服务
        /// 进行计算机硬件软件信息的收集
        /// </summary>
        public void Get_MypcInfo()
        {
            //查询计算机名
            this.Online_Order += this.WMI_Searcher("SELECT * FROM Win32_ComputerSystem", "Caption") + "||";
            //查询操作系统
            this.Online_Order += this.WMI_Searcher("SELECT * FROM Win32_OperatingSystem", "Caption") + "||";
            //查询CPU
            this.Online_Order += this.WMI_Searcher("SELECT * FROM Win32_Processor", "Caption") + "||";
            //查询内存容量 - 单位: MB
            this.Online_Order += (int.Parse(this.WMI_Searcher("SELECT * FROM Win32_OperatingSystem", "TotalVisibleMemorySize")) / 1024) + " MB||";
            //查询备注
            this.Online_Order += Customer + "||";
        }

        /// <summary>
        /// 此方法根据指定语句通过WMI查询用户指定内容
        /// 并且返回
        /// </summary>
        /// <param name="QueryString"></param>
        /// <param name="Item_Name"></param>
        /// <returns></returns>
        public String WMI_Searcher(String QueryString, String Item_Name)
        {
            String Result = "";
            ManagementObjectSearcher MOS = new ManagementObjectSearcher(QueryString);
            ManagementObjectCollection MOC = MOS.Get();
            foreach (ManagementObject MOB in MOC)
            {
                Result = MOB[Item_Name].ToString();
                break;
            }
            MOC.Dispose();
            MOS.Dispose();
            return Result;
        }

        #endregion

        private void button_control_Click(object sender, EventArgs e)
        {
            Control_TD kztt = new Control_TD();
            kztt.ShowDialog();
        }

        #region 输入字符合法性验证
        /// <summary>
        /// 输入字符合法性验证
        /// </summary>
        /// <param name="Ct">TextBox类型控件 名称</param>
        /// <param name="Regex">string 正则表达式(请勿带长度限制)</param>
        /// <param name="extent">int 允许输入的长度</param>
        /// <param name="Correct">bool 是否修正</param>
        /// <returns>bool 有错返回true</returns>
        public static bool Text_Verification(TextBox Ct, string Regex, int extent, bool Correct)
        {
            bool flag = false;
            try
            {
                if (Ct is TextBox)
                {
                    if (Ct.Text.Length > 0)
                    {
                        for (int i = Ct.Text.Length - 1; i >= 0; i--)
                        {
                            // 利用正則表達式，其中 \u4E00-\u9fa5 表示中文
                            if (!System.Text.RegularExpressions.Regex.IsMatch(Ct.Text.Substring(i, 1), Regex) || Ct.Text.Length > extent)
                            {
                                if (Correct)
                                    Ct.Text = Ct.Text.Remove(i, 1);
                                flag = true;
                            }
                        }
                        Ct.SelectionStart = Ct.Text.Length;
                    }
                }
            }
            catch (Exception)
            {
            }
            return flag;
        }

        /// <summary>
        /// 输入字符合法性验证
        /// </summary>
        /// <param name="Ct">ComboBox类型控件 名称</param>
        /// <param name="Regex">string 正则表达式(请勿带长度限制)</param>
        /// <param name="extent">int 允许输入的长度</param>
        /// <param name="Correct">bool 是否修正</param>
        /// <returns>bool 有错返回true</returns>
        public static bool Text_Verification(ComboBox Ct, string Regex, int extent, bool Correct)
        {
            bool flag = false;
            try
            {
                if (Ct is ComboBox)
                {
                    if (Ct.Text.Length > 0)
                    {
                        for (int i = Ct.Text.Length - 1; i >= 0; i--)
                        {
                            // 利用正則表達式，其中 \u4E00-\u9fa5 表示中文
                            if (!System.Text.RegularExpressions.Regex.IsMatch(Ct.Text.Substring(i, 1), Regex) || Ct.Text.Length > extent)
                            {
                                if (Correct)
                                    Ct.Text = Ct.Text.Remove(i, 1);
                                flag = true;
                            }
                        }
                        Ct.SelectionStart = Ct.Text.Length;
                    }
                }
            }
            catch (Exception)
            {
            }
            return flag;
        }
        #endregion

        #region 信息显示
        /// <summary>
        /// 信息显示
        /// </summary>
        /// <param name="Msgowner">信息显示的Label控件</param>
        /// <param name="Msgfather">Label控件的父级Panel</param>
        /// <param name="Msgstr">要显示的信息</param>
        /// <param name="Update_DB">是不是要更新到检测状态</param>
        public void Msg(Label Msgowner, Panel Msgfather, string Msgstr, bool Update_DB)
        {
            BeginInvoke(new wtlsb(Msg_Show), Msgowner, Msgstr, Update_DB);
            BeginInvoke(new wtlp(Msg_Position), Msgowner, Msgfather);
        }

        public void Msg_Show(Label Msgowner, string Msgstr, bool Update_DB)
        {
            Msgowner.Text = Msgstr;
            /*if (Update_DB)
            {
                CarWait.bjcl.JCZT = Msgstr;
                CarWait.bjclxx.Update(CarWait.bjcl);
            }*/
        }

        public void Msg_Position(Label Msgowner, Panel Msgfather)
        {
            if (Msgowner.Width < Msgfather.Width)
                Msgowner.Location = new Point((Msgfather.Width - Msgowner.Width) / 2, Msgowner.Location.Y);
            else
                Msgowner.Location = new Point(0, Msgowner.Location.Y);
        }

        /// <summary>
        /// 刷新控件的文字信息
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <param name="text">文字信息</param>
        public void Ref_Control_Text(Control control, string text)
        {
            BeginInvoke(new wtcs(ref_Control_Text), control, text);
        }

        public void ref_Control_Text(Control control, string text)
        {
            control.Text = text;
        }
        #endregion

        private void btn_cxyqzt_Click(object sender, EventArgs e)
        {
            try
            {
                string fqzt="";
                switch (UseFqy)
                {
                    case "FLA_502":
                        {
                            if (fla_502 != null)
                            {
                                fqzt = fla_502.Get_Struct();
                                Msg(Msg_FQ, panel_fq, fqzt, false);
                                if (fqzt.IndexOf("准备好") > -1)
                                {
                                    //btn_tl.Enabled = true;
                                    //btn_fc.Enabled = true;
                                    //btn_tzjc.Enabled = true;
                                    //btn_ssclfq.Enabled = true;
                                    //btn_tcclfq.Enabled = true;
                                }
                                else
                                {
                                    //btn_tl.Enabled = false;
                                    //btn_fc.Enabled = false;
                                    //btn_tzjc.Enabled = false;
                                    //btn_ssclfq.Enabled = false;
                                    //btn_tcclfq.Enabled = false;
                                }
                            }
                            else
                                Msg(Msg_FQ, panel_fq, "仪器出错,检查配置、串口、状态", false);
 
                        }
                        break;
                    case "MQW_50A":
                        {
                            if (mqw_50A != null)
                            {
                                fqzt = mqw_50A.Get_Struct();
                                Msg(Msg_FQ, panel_fq, fqzt, false);
                                if (fqzt.IndexOf("准备好") > -1)
                                {
                                    //btn_tl.Enabled = true;
                                    //btn_fc.Enabled = true;
                                    //btn_tzjc.Enabled = true;
                                    //btn_ssclfq.Enabled = true;
                                    //btn_tcclfq.Enabled = true;
                                }
                                else
                                {
                                    //btn_tl.Enabled = false;
                                    //btn_fc.Enabled = false;
                                    //btn_tzjc.Enabled = false;
                                    //btn_ssclfq.Enabled = false;
                                    //btn_tcclfq.Enabled = false;
                                }
                            }
                            else
                                Msg(Msg_FQ, panel_fq, "仪器出错,检查配置、串口、状态", false);
                        }
                        break;
                    case "FLA_501":
                        {
                            if (fla_501 != null)
                            {
                                Msg(Msg_FQ, panel_fq, "该型号废气仪不提供此功能", false);
                            }
                            else
                                Msg(Msg_FQ, panel_fq, "仪器出错,检查配置、串口、状态", false);
                        }
                        break;
                }
               
            }
            catch (Exception)
            {
                Msg(Msg_FQ, panel_fq, "出错,请检查重试", false);
            }
        }

        private void btn_tl_Click(object sender, EventArgs e)
        {
            Thread tl_fq = new Thread(fqtl);
            tl_fq.Start();
        }

        private void fqtl()
        {
            try
            {
                btn_fc.Enabled = false;
                btn_tl.Enabled = false;
                btn_tzjc.Enabled = false;
                btn_ssclfq.Enabled = false;
                btn_tcclfq.Enabled = false;
                button_stopblow.Enabled = false;
                switch (UseFqy)
                {
                    case "FLA_502":
                        if (fla_502 != null)
                        {
                            fla_502.Zeroing();
                            for (int i = 30; i > 0; i--)
                            {
                                Msg(Msg_FQ, panel_fq, "仪器调零 " + i, false);
                                btn_tl.Text = "调零 " + i;
                                Thread.Sleep(1000);
                            }
                            btn_tl.Text = "调零";
                            while (true)
                            {
                                int i = 100;
                                if (i <= 1)
                                {
                                    Msg(Msg_FQ, panel_fq, "调零失败,请手动操作", false);
                                    break;
                                }
                                if (fla_502.Get_Struct().IndexOf("准备好") > -1)
                                    break;
                                else
                                    i--;
                                Thread.Sleep(10);
                            }
                            Msg(Msg_FQ, panel_fq, "调零成功", false);
                            btn_fc.Enabled = true;
                            btn_tl.Enabled = true;
                            btn_tzjc.Enabled = true;
                            btn_ssclfq.Enabled = true;
                            btn_tcclfq.Enabled = true;
                            button_stopblow.Enabled = true;
                        }
                        else
                        {
                            Msg(Msg_FQ, panel_fq, "调零失败,检查仪器状态", false);
                        }
                        break;
                    case "FLA_501":
                        if (fla_501 != null)
                        {
                            fla_501.SetZero();
                            for (int i = 30; i > 0; i--)
                            {
                                Msg(Msg_FQ, panel_fq, "仪器调零 " + i, false);
                                btn_tl.Text = "调零 " + i;
                                Thread.Sleep(1000);
                            }
                            btn_tl.Text = "调零";
                            Msg(Msg_FQ, panel_fq, "调零成功", false);
                            btn_fc.Enabled = true;
                            btn_tl.Enabled = true;
                            btn_tzjc.Enabled = true;
                            btn_ssclfq.Enabled = true;
                            btn_tcclfq.Enabled = true;
                            button_stopblow.Enabled = true;
                        }
                        else
                        {
                            Msg(Msg_FQ, panel_fq, "调零失败,检查仪器状态", false);
                        }
                        break;
                }
                                
            }
            catch (Exception)
            {
                Msg(Msg_FQ, panel_fq, "调零失败,检查仪器状态", false);
            }
        }

        private void btn_fc_Click(object sender, EventArgs e)
        {
            try
            {
                switch (UseFqy)
                {
                    case "FLA_502":
                        if (fla_502 != null)
                        {
                            fla_502.Blowback();
                            Msg(Msg_FQ, panel_fq, "正在反吹……", false);
                            btn_ssclfq.Enabled = false;
                            btn_tcclfq.Enabled = false;
                        }
                        else
                            Msg(Msg_FQ, panel_fq, "调零失败,检查仪器状态", false);
                        break;
                    case "FLA_501":
                        if (fla_501 != null)
                        {
                            fla_501.Clear();
                            Msg(Msg_FQ, panel_fq, "正在清洗管道……", false);
                            btn_ssclfq.Enabled = false;
                            btn_tcclfq.Enabled = false;
                        }
                        else
                            Msg(Msg_FQ, panel_fq, "调零失败,检查仪器状态", false);
                        break;
                }
            }
            catch (Exception)
            {
                Msg(Msg_FQ, panel_fq, "调零失败,检查仪器状态", false);
            }
        }
        private void button_stopblow_Click(object sender, EventArgs e)
        {
            try
            {
                switch(UseFqy)
                {
                    case "FLA_502":
                        if (fla_502 != null)
                        {
                            fla_502.StopBlowback();
                            Msg(Msg_FQ, panel_fq, "停止反吹", false);
                            btn_ssclfq.Enabled = true;
                            btn_tcclfq.Enabled = true;
                        }
                        else
                            Msg(Msg_FQ, panel_fq, "调零失败,检查仪器状态", false);
                        break;
                    case "FLA_501":
                        if (fla_501 != null)
                        {
                            fla_501.Stop();
                            Msg(Msg_FQ, panel_fq, "停止反吹", false);
                            btn_ssclfq.Enabled = true;
                            btn_tcclfq.Enabled = true;
                        }
                        else
                            Msg(Msg_FQ, panel_fq, "调零失败,检查仪器状态", false);
                        break;
                }
            }
            catch (Exception)
            {
                Msg(Msg_FQ, panel_fq, "调零失败,检查仪器状态", false);
            }

        }

        private void btn_tzjc_Click(object sender, EventArgs e)
        {
            try
            {
                switch (UseFqy)
                {
                    case "FLA_502":
                        if (fla_502 != null)
                        {
                            fla_502.Stop_Check();
                            btn_ssclfq.Enabled = true;
                            btn_tcclfq.Enabled = true;
                            Msg(Msg_FQ, panel_fq, "已经停止检查", false);
                        }
                        else
                            Msg(Msg_FQ, panel_fq, "调零失败,检查仪器状态", false);
                        break;
                    case "FLA_501":
                        if (fla_501 != null)
                        {
                            fla_501.Stop();
                            btn_ssclfq.Enabled = true;
                            btn_tcclfq.Enabled = true;
                            Msg(Msg_FQ, panel_fq, "已经停止检查", false);
                        }
                        else
                            Msg(Msg_FQ, panel_fq, "调零失败,检查仪器状态", false);
                        break;
                }
                
            }
            catch (Exception)
            {
                Msg(Msg_FQ, panel_fq, "调零失败,检查仪器状态", false);
            }
        }

        private void btn_ssclfq_Click(object sender, EventArgs e)
        {
            th_fqsscl = new Thread(fqsscl);
            th_fqsscl.Start();
        }

        private void fqsscl()
        {
            try
            {
                switch (UseFqy)
                {
                    case "FLA_502":
                        {
                            if (fla_502 != null)
                            {
                                string clzt = fla_502.Start();
                                Msg(Msg_FQ, panel_fq, clzt, false);
                                if (clzt != "成功开始测量")
                                    return;
                                else
                                {
                                    btn_tl.Enabled = false;
                                    btn_fc.Enabled = false;
                                    button_stopblow.Enabled = false;
                                    btn_tzjc.Enabled = false;
                                    btn_ssclfq.Enabled = false;
                                    Thread.Sleep(1000);
                                    Msg(Msg_FQ, panel_fq, "", false);
                                    while (true)
                                    {
                                        Thread.Sleep(500);
                                        Exhaust.Fla502_data ex_temp = fla_502.GetData();
                                        Msg(Msg_fq_1, panel_fq, "HC:" + ex_temp.HC + " CO:" + ex_temp.CO + " CO2:" + ex_temp.CO2 + " O2:" + ex_temp.O2 + "NO:" + ex_temp.NO + " λ:" + ex_temp.λ + " 转速:" + ex_temp.ZS + " 油温:" + ex_temp.YW, false);
                                        //Msg(Msg_fq_2, panel_fq, "NO:" + ex_temp.NO + " λ:" + ex_temp.λ + " 转速:" + ex_temp.ZS + " 油温:" + ex_temp.YW, false);
                                    }
                                }
                            }
                            else
                            {
                                Msg(Msg_FQ, panel_fq, "检测失败,检查仪器状态", false);
                            }
                        }
                        break;
                    case "FLA_501":
                        {
                            if (fla_501 != null)
                            {
                                bool clzt = fla_501.Start();
                                if (clzt == false)
                                {
                                    Msg(Msg_FQ, panel_fq, "未能成功启动测量", false);
                                    return;
                                }
                                else
                                {
                                    btn_tl.Enabled = false;
                                    btn_fc.Enabled = false;
                                    button_stopblow.Enabled = false;
                                    btn_tzjc.Enabled = false;
                                    btn_ssclfq.Enabled = false;
                                    Thread.Sleep(1000);
                                    Msg(Msg_FQ, panel_fq, "", false);
                                    while (true)
                                    {
                                        Thread.Sleep(500);
                                        Exhaust.Fla501_data ex_temp = fla_501.Get_Data();
                                        Msg(Msg_fq_1, panel_fq, "HC:" + ex_temp.HC + " CO:" + ex_temp.CO + " CO2:" + ex_temp.CO2 + " O2:" + ex_temp.O2 + "NO:" + ex_temp.NO + " λ:" + ex_temp.λ + " 转速:" + ex_temp.ZS + " 油温:" + ex_temp.YW, false);
                                        //Msg(Msg_fq_2, panel_fq, "NO:" + ex_temp.NO + " λ:" + ex_temp.λ + " 转速:" + ex_temp.ZS + " 油温:" + ex_temp.YW, false);
                                    }
                                }
                            }
                            else
                            {
                                Msg(Msg_FQ, panel_fq, "检测失败,检查仪器状态", false);
                            }
                        }
                        break;
                }
            }
            catch (Exception)
            {
                Msg(Msg_FQ, panel_fq, "检测失败,检查仪器状态", false);
            }
        }

        private void btn_tcclfq_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    th_fqsscl.Abort();
                }
                catch (Exception)
                {
                }
                switch (UseFqy)
                {
                    case "FLA_502":
                        if (fla_502 != null)
                            fla_502.Stop();
                        break;
                    case "FLA_501":
                        if (fla_501 != null)
                            fla_501.Stop();
                        break;
                }
                btn_tl.Enabled = true;
                btn_fc.Enabled = true;
                btn_tzjc.Enabled = true;
                btn_ssclfq.Enabled = true;
                button_stopblow.Enabled = true;
                Msg(Msg_fq_1, panel_fq, "", false);
                Msg(Msg_FQ, panel_fq, "已经停止测量", false);
            }
            catch (Exception)
            {
            }
        }

        private void btn_yqzt_Click(object sender, EventArgs e)
        {
            try
            {
                if(th_ydjsscl!=null)
                    th_ydjsscl.Abort();//停止测试
                string ydzt = "";
                Msg(label_yd_1, panel_yd, "", false);
                //Msg(label_yd_2, panel_yd, "", false);
                if (Flb_100 != null)
                {
                    ydzt = Flb_100.Get_Mode();

                    Msg(Msg_yd, panel_yd, ydzt, false);
                    //if (fqzt.IndexOf("准备好") > -1)
                    //{
                    //    btn_tl.Enabled = true;
                    //    btn_fc.Enabled = true;
                    //    btn_tzjc.Enabled = true;
                    //    btn_ssclfq.Enabled = true;
                    //    btn_tcclfq.Enabled = true;
                    //}
                    //else
                    //{
                    //    btn_tl.Enabled = false;
                    //    btn_fc.Enabled = false;
                    //    btn_tzjc.Enabled = false;
                    //    btn_ssclfq.Enabled = false;
                    //    btn_tcclfq.Enabled = false;
                    //}
                }
                else
                    Msg(Msg_yd, panel_yd, "仪器出错,检查配置、串口、状态", false);
            }
            catch (Exception)
            {
                Msg(Msg_yd, panel_yd, "出错,请检查重试", false);
            }
        }

        private void btn_jz_Click(object sender, EventArgs e)
        {
            try
            {
                Msg(label_yd_1, panel_yd, "", false);
                Thread.Sleep(10);
                //Msg(label_yd_2, panel_yd, "", false);
                if (Flb_100 != null)
                {
                    if (Flb_100.set_linearDem())
                    {
                        Msg(Msg_yd, panel_yd, "开始校正", true);
                        Thread.Sleep(3000);
                        Msg(Msg_yd, panel_yd, "校正完成", false);
                    }
                    else
                        Msg(Msg_yd, panel_yd, "失败,检查仪器是否处于实时测量", false);
                }
                else
                    Msg(Msg_yd, panel_yd, "仪器出错,检查配置、串口、状态", false);
            }
            catch (Exception)
            {
                Msg(label_yd_1, panel_yd, "", false);
                //Msg(label_yd_2, panel_yd, "", false);
                Msg(Msg_yd, panel_yd, "出错,请检查重试", false);
            }
        }

        private void btn_ssclyd_Click(object sender, EventArgs e)
        {
            th_ydjsscl = new Thread(ydjsscl);
            th_ydjsscl.Start();
        }
        private void ydjsscl()
        {
                if (Flb_100 != null)
                {
                    if (Flb_100.Set_Measure())
                    {
                        Msg(Msg_yd, panel_yd, "进入实时测量成功", false);
                        Thread.Sleep(1000);
                        Msg(Msg_yd, panel_yd, "", false);
                        while (true)
                        {
                            Thread.Sleep(500);
                            Exhaust.Flb_100_smoke ex_smoke = Flb_100.get_Data();
                            Msg(label_yd_1, panel_yd, "K:" + ex_smoke.K + " Ns:" + ex_smoke.Ns + "气温:" + ex_smoke.Qw + " 油温:" + ex_smoke.Yw + " 转速:" + ex_smoke.Zs, false);
                            //Msg(label_yd_2, panel_yd, "气温:" + ex_smoke.Qw + " 油温:" + ex_smoke.Yw + " 转速:" + ex_smoke.Zs, false);
                        }
                    }
                }
                else
                {
                    Msg(Msg_yd, panel_yd, "检测失败,检查仪器状态", false);
                    Msg(label_yd_1, panel_yd, "", false);
                    //Msg(label_yd_2, panel_yd, "", false);
                }
        }
        private void button_tzyd_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    th_ydjsscl.Abort();
                    Msg(label_yd_1, panel_yd, "", false);
                    //Msg(label_yd_2, panel_yd, "", false);
                    Msg(Msg_yd, panel_yd, "已经停止测量", false);
                }
                catch (Exception)
                {
                }
                //if (Flb_100 != null)
                    //Flb_100.Stop();
                
            }
            catch (Exception)
            {
            }

        }
        
        private void btn_jsss_Click(object sender, EventArgs e)
        {
            try
            {
                if (igbt != null)
                    igbt.Lifter_Up();
                else
                    Msg(Msg_cg, panel_cg, "仪器出错,检查配置、串口、状态", false);
            }
            catch (Exception)
            {
                Msg(Msg_cg, panel_cg, "出错,请检查重试", false);
            }
        }

        private void btn_jsxj_Click(object sender, EventArgs e)
        {
            try
            {
                if (igbt != null)
                    igbt.Lifter_Down();
                else
                    Msg(Msg_cg, panel_cg, "仪器出错,检查配置、串口、状态", false);
            }
            catch (Exception)
            {
                Msg(Msg_cg, panel_cg, "出错,请检查重试", false);
            }
        }

        private void btn_jt_Click(object sender, EventArgs e)
        {
            try
            {
                if (igbt != null)
                    igbt.Exit_Control();
                else
                    Msg(Msg_cg, panel_cg, "仪器出错,检查配置、串口、状态", false);
            }
            catch (Exception)
            {
                Msg(Msg_cg, panel_cg, "出错,请检查重试", false);
            }
        }

        private void dataGrid_waitcar_MouseEnter(object sender, EventArgs e)
        {
            ref_WaitCar();
            ref_Select();
        }

        private void Wait_Car_menu_Opening(object sender, CancelEventArgs e)
        {
            foreach (ToolStripMenuItem tsmi in Wait_Car_menu.Items)
                tsmi.Enabled = false;
            刷新ToolStripMenuItem.Enabled = true;
            if (dataGrid_waitcar.SelectedRows.Count > 0)
            {
                ToolStripMenuItemqx.Enabled = true;
                录入检测员ToolStripMenuItem.Enabled = true;
            }
            if (dataGrid_waitcar.SelectedRows.Count == 1)
            {
                switch (dataGrid_waitcar.SelectedRows[0].Cells["检测方法"].Value.ToString().Trim())
                { 
                    case "自由加速法":
                        if (mqy_200 != null)
                            ToolStripMenuItem_fc.Enabled = true;
                        break;
                    case "加载减速法":
                        if (mqy_200 != null && igbt != null)
                            ToolStripMenuItem_fc.Enabled = true;
                        break;
                    case "双怠速法":
                        if (fla_502 != null)
                            ToolStripMenuItem_fc.Enabled = true;
                        break;
                    case "稳态工况法":
                        if (fla_502 != null && igbt != null)
                            ToolStripMenuItem_fc.Enabled = true;
                        break;
                }
            }
        }

        private void buttonStopBlow_Click(object sender, EventArgs e)
        {
            try
            {
                if (fla_502 != null)
                {
                    fla_502.StopBlowback();
                    Msg(Msg_FQ, panel_fq, "停止反吹", false);
                    btn_ssclfq.Enabled = true;
                    btn_tcclfq.Enabled = true;
                }
                else
                    Msg(Msg_FQ, panel_fq, "停止失败,检查仪器状态", false);
            }
            catch (Exception)
            {
                Msg(Msg_FQ, panel_fq, "停止失败,检查仪器状态", false);
            }
        }

        private void button_lljzt_Click(object sender, EventArgs e)
        {
            try
            {
                string lljzt = "";
                switch (UseLlj)
                {
                    case "FLV_1000":
                        {
                            if (flv_1000 != null)
                            {
                                lljzt = flv_1000.Get_Struct();
                                if (lljzt == "流量计已经准备好" || lljzt == "仪器通讯失败" || lljzt == "串口出错")
                                {
                                    Msg(label_llj_3, panel_llj, "", false);
                                    //Msg(label_llj_2, panel_llj, "", false);
                                    Msg(Msg_llj, panel_llj, lljzt, false);
                                    //return;
                                }
                                else
                                {
                                    Msg(Msg_llj, panel_llj,"", false);
                                    string[] lljzt2 = lljzt.Split(new string[] { "。" }, StringSplitOptions.RemoveEmptyEntries);
                                    Msg(label_llj_3, panel_llj, lljzt2[0], false);
                                    //Msg(label_llj_2, panel_llj, lljzt2[1], false);
 
                                }
                            }
                            else
                                Msg(Msg_llj, panel_llj, "仪器出错,检查配置、串口、状态", false);

                        }
                        break;
                    
                }

            }
            catch (Exception)
            {
                Msg(Msg_llj, panel_llj, "出错,请检查重试", false);
            }
        }

        private void button_lljcl_Click(object sender, EventArgs e)
        {
            th_lljcl = new Thread(lljcl);
            th_lljcl.Start();
        }

        private void lljcl()
        {
            try
            {
                if (flv_1000 != null)
                {
                    string lljzt = flv_1000.Get_Struct();
                    /*if (lljzt != "流量计已经准备好")
                    {
                        Msg(Msg_llj, panel_llj, "", false);
                        string[] lljzt2 = lljzt.Split(new string[] { "。" }, StringSplitOptions.RemoveEmptyEntries);
                        Msg(label_llj_3, panel_llj, lljzt2[0], false);
                    }
                    else
                    {*/
                        button_lljzt.Enabled = false;
                        button_lljcl.Enabled = false;

                        Thread.Sleep(1000);
                        Msg(Msg_llj, panel_llj, "", false);
                        while (true)
                        {
                            Thread.Sleep(500);
                            if (flv_1000.Get_standardDat() == "获取成功")
                            {
                                //Msg(label_llj_2, panel_llj, "温度:" + flv_1000.temp_standard_value + "气压:" + flv_1000.yali_standard_value, false);
                                Msg(label_llj_3, panel_llj, " 流量:" + flv_1000.ll_standard_value + " O2:" + flv_1000.o2_standard_value + "温度:" + flv_1000.temp_standard_value + "气压:" + flv_1000.yali_standard_value, false);
                                //Msg(Msg_fq_2, panel_fq, "NO:" + ex_temp.NO + " λ:" + ex_temp.λ + " 转速:" + ex_temp.ZS + " 油温:" + ex_temp.YW, false);

                            }
                            else
                            {

                            }
                            //Exhaust.Fla502_data ex_temp = fla_502.GetData();

                            //Msg(Msg_fq_1, panel_fq, "HC:" + ex_temp.HC + " CO:" + ex_temp.CO + " CO2:" + ex_temp.CO2 + " O2:" + ex_temp.O2, false);
                            //Msg(Msg_fq_2, panel_fq, "NO:" + ex_temp.NO + " λ:" + ex_temp.λ + " 转速:" + ex_temp.ZS + " 油温:" + ex_temp.YW, false);
                        }
                    //}
                }
                else
                {
                    Msg(Msg_llj, panel_llj, "出错,请检查流量计", false);
                }
            }
            catch (Exception)
            {
                Msg(Msg_llj, panel_llj, "检测失败,检查仪器状态", false);
            }
        }

        private void button_lljstop_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    th_lljcl.Abort();
                }
                catch (Exception)
                {
                }
                button_lljzt.Enabled = true;
                button_lljcl.Enabled = true;
                //Msg(label_llj_2, panel_llj, "", false);
                Msg(label_llj_3, panel_llj, "", false);
                Msg(Msg_llj, panel_llj, "已经停止测量", false);
            }
            catch (Exception)
            {
            }
        }

        private void button_llj_tl_Click(object sender, EventArgs e)
        {
            switch (UseLlj)
            {
                case "FLV_1000":
                    {
                        

                    }
                    break;

            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }
        #region 保存车辆信息
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            /*SYS.Model.BJCLXXB model=new SYS.Model.BJCLXXB();
            if (!check_IsRight(out model))
            {
                MessageBox.Show("带\"*\"项为必填项,请填写完整后再提交", "系统提示");
            }
            else
            {
                if (bjclxx.Have_BjclInDaijian(model.JCCLPH))
                {
                    //MessageBox.Show("该车已经在待检序列中,是否要更新该车信息", "系统提示");
                    if (MessageBox.Show("该车已经在待检序列中,是否要更新该车信息？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        bjclxx.Update(model);
                        MessageBox.Show("更新成功", "系统提示");
                    }
                }
                else
               {
                if (bjclxx.Savedate(model))
                {
                    //ref_WaitCar();
                    MessageBox.Show("保存成功,检测编号为"+model.JCBH+".", "系统提示");
                }
 
               }
                ref_WaitCar();
                    
                       
            }*/
            init_loginPanel();
        }
        #endregion
        /*
        #region 检查输入是否正确
        private bool check_IsRight(out SYS.Model.BJCLXXB model)
        {
            model = new BJCLXXB();
            if (textBoxPlateNumber.Text == "" || comboBoxMobileType.Text == "" || textBoxFarameNumber.Text == "" || textBoxWeight.Text == ""
                || textBoxMostWeight.Text == "" || comboBoxFuelType.Text == "" || comboBoxFuelSupplyType.Text == "" || comboBoxGreenFlag.Text == "" || dateEditRegisterData.Text == "")
            {
                
                return false;
            }
            else
            {
                DateTime dt = DateTime.Now;
                string nowtime = dt.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").Trim();
                model.JCBH = nowtime; //用注册的时间做为被检车辆的编号,检测完了再重新定义检测编号存储到相应的数据库中
                //model.JCBH = jczNumber + "-" + jcxxxb.JCXBH.ToString() + "-" + jcxxxb.LJSYS.ToString();
                model.PZLX = comboBoxMobileColor.Text;//牌照颜色
                model.JCCLPH = textBoxPlateNumber.Text;//牌照号码
                model.CLXHBH = textBoxMobileNumber.Text;//车辆型号
                model.JCCS = 0;//检测次数
                //model.LSH=;//
                model.CCRQ = Convert.ToDateTime(dateEditProductDate.Text.Replace('/', '-'));//生产日期
                model.DJRQ = Convert.ToDateTime(dateEditRegisterData.Text.Replace('/', '-'));//登记日期
                model.FDJH = textBoxEngineNumber.Text;//发动机号
                model.CJH = textBoxFarameNumber.Text;//车架号
                model.CZ = textBoxOwnerName.Text;//车主
                model.CZDH = textBoxOwnerTel.Text;//车主电话
                model.CZDZ = textBoxOwnerAdd.Text;//车主地址
                if (textBoxOdometer.Text != "")
                    model.LCBDS = Convert.ToInt32(textBoxOdometer.Text);//里程表读数
                else
                    model.LCBDS = 0;
                model.HBBZ = comboBoxGreenFlag.Text;//环保标志
                model.SYQK = comboBoxInUse.Text;//使用情况
                if (comboBox_detectLine.Text.Trim() == "" || comboBox_detectLine.Text.Trim() == "本线检测")
                { 
                    model.JCBJ = jcxxxb.JCXBH.ToString();//默认为本线检测
                }//检测线号
                else if (comboBox_detectLine.Text.Trim() == "优先")
                {
                    model.JCBJ ="-1";
                }
                
                model.JCZT = "请上测功机检测";//检测状态
                model.QRJCFF = comboBoxJcff.Text;//检测方法
                model.RYLX = comboBoxFuelType.Text;
                model.GYFF = comboBoxFuelSupplyType.Text;
                model.BSXLX = comboBoxGearbox.Text;
                model.CLLX = comboBoxMobileType.Text;
                model.ZBZL = textBoxWeight.Text;
                model.ZDZZL = textBoxMostWeight.Text;
                model.FDJPL = textBoxEnginePlacement.Text;
                model.FDJSCS = textBoxEngineManufacture.Text;
                model.JCZH = jczNumber;
                model.DCZZ = textBoxAxleLoad.Text;
                model.FDJEDGL = textBoxEnginePower.Text;
                model.FDJEDZS = textBoxEngineSpeed.Text;
                if (textBoxPipeCount.Text != "")
                    model.PQGSL = Convert.ToInt32(textBoxPipeCount.Text);
                else
                    model.PQGSL = 0;
                model.PFBZ = "地方标准";
                model.ZZCS = textBoxMobileManufacture.Text;
                if (comboBoxCylinderCount.Text != "")
                    model.QGS = Convert.ToInt32(comboBoxCylinderCount.Text);
                else
                    model.QGS = 0;
                model.QDXS = comboBoxDriveStyle.Text;
                model.QDLQY = textBoxPneumaticWheel.Text;
                model.JCYH = textBoxInspectorNumber.Text;
                if (textBoxGearCount.Text != "")
                    model.DWS = Convert.ToInt32(textBoxGearCount.Text);
                else
                    model.DWS = 0;
                if (textBoxHdzk.Text != "")
                    model.HDZK = Convert.ToInt32(textBoxHdzk.Text);
                else
                    model.HDZK = 0;
                model.JQXS = comboBoxJqxs.Text;
                return true;
            }
 
        }
        #endregion
         
        private void simpleButtonCheckIsExist_Click(object sender, EventArgs e)
        {
            if(textBoxPlateNumber.Text=="")
            {
                MessageBox.Show("请输入车牌号再进行查询!","系统提示");
            }
            else
            {
                string jcclcp = textBoxPlateNumber.Text;
                if (ref_LoginPanel(jcclcp))
                {
                    MessageBox.Show("查询成功!", "系统提示");
                }
                else
                {
                    MessageBox.Show("初次检测车辆,请输入车辆信息.", "信息提示");
                }

            }
            

        }
         */
        /*
        #region 根据车牌号来显示登记信息
        private bool ref_LoginPanel(string jcclcp)
        {
            BJCLXXB model = new BJCLXXB();
            DataTable dt = bjclxx.Get_Carxx_byph(jcclcp);
            if (dt.Rows.Count > 0)
            {
                textBoxPlateNumber.Text = dt.Rows[0]["JCCLPH"].ToString();
                comboBoxMobileColor.Text = dt.Rows[0]["PZLX"].ToString();
                textBoxMobileNumber.Text = dt.Rows[0]["CLXHBH"].ToString();//车辆型号
                dateEditRegisterData.Text = dt.Rows[0]["DJRQ"].ToString().Split(new char[] { ' ' })[0];
                textBoxEngineNumber.Text = dt.Rows[0]["FDJH"].ToString(); //发动机号
                textBoxFarameNumber.Text = dt.Rows[0]["CJH"].ToString(); //车架号
                textBoxOwnerName.Text = dt.Rows[0]["CZ"].ToString(); //车主
                textBoxOwnerTel.Text = dt.Rows[0]["CZDH"].ToString(); //车主电话
                textBoxOwnerAdd.Text = dt.Rows[0]["CZDZ"].ToString(); //车主地址
                textBoxOdometer.Text = dt.Rows[0]["LCBDS"].ToString();
                comboBoxGreenFlag.Text = dt.Rows[0]["HBBZ"].ToString(); //环保标志
                comboBoxInUse.Text = dt.Rows[0]["SYQK"].ToString(); 
                comboBoxJcff.Text = dt.Rows[0]["QRJCFF"].ToString(); //检测方法
                comboBoxFuelType.Text = dt.Rows[0]["RYLX"].ToString();
                comboBoxFuelSupplyType.Text = dt.Rows[0]["GYFF"].ToString();
                comboBoxGearbox.Text = dt.Rows[0]["BSXLX"].ToString();
                comboBoxMobileType.Text = dt.Rows[0]["CLLX"].ToString();
                textBoxWeight.Text = dt.Rows[0]["ZBZL"].ToString();
                textBoxMostWeight.Text = dt.Rows[0]["ZDZZL"].ToString();
                textBoxEnginePlacement.Text = dt.Rows[0]["FDJPL"].ToString();
                textBoxEngineManufacture.Text = dt.Rows[0]["FDJSCS"].ToString();
                jczNumber = dt.Rows[0]["JCZH"].ToString();
                textBoxAxleLoad.Text = dt.Rows[0]["DCZZ"].ToString();
                textBoxEnginePower.Text = dt.Rows[0]["FDJEDGL"].ToString();
                textBoxEngineSpeed.Text = dt.Rows[0]["FDJEDZS"].ToString();
                textBoxPipeCount.Text = dt.Rows[0]["PQGSL"].ToString();
                textBoxEmissionStandard.Text = dt.Rows[0]["PFBZ"].ToString();
                textBoxMobileManufacture.Text = dt.Rows[0]["ZZCS"].ToString();
                comboBoxCylinderCount.Text = dt.Rows[0]["QGS"].ToString();
                comboBoxDriveStyle.Text = dt.Rows[0]["QDXS"].ToString();
                textBoxPneumaticWheel.Text = dt.Rows[0]["QDLQY"].ToString();
                textBoxInspectorNumber.Text = dt.Rows[0]["JCYH"].ToString();
                textBoxHdzk.Text=dt.Rows[0]["HDZK"].ToString();
                textBoxGearCount.Text = dt.Rows[0]["DWS"].ToString();
                comboBoxJqxs.Text = dt.Rows[0]["JQXS"].ToString();
                return true;
            }
            else
            {
                
                return false;
 
            }

                
        }
        #endregion
        */
        

        private void timer2_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now; //当前时间的实例;
            string nowtime = dt.ToString();
            textBoxNowTime.Text = (nowtime.Split(new char[] { ' ' }))[0]; //转为string类型 把值交给textBox1的Text属性; 
            textBoxToday.Text = (nowtime.Split(new char[] { ' ' }))[1];
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Fc();

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            string daijian_bjclpz=textBoxDaijianCar.Text.Trim();
            if (daijian_bjclpz == "")
            {
                MessageBox.Show("请输入车牌号.", "系统提示");
            }
            else
            {
                if (bjclxx.Have_BjclInDaijian(daijian_bjclpz))
                {
                    for (int i = 0; i < dataGrid_waitcar.Rows.Count; i++)
                    {

                        if (dataGrid_waitcar.Rows[i].Cells["车牌号"].Value.ToString().Trim() == daijian_bjclpz)
                        {
                            foreach(DataGridViewRow dr in dataGrid_waitcar.SelectedRows)
                            {
                                foreach(DataGridViewCell drcellselected in dr.Cells)
                                {
                                    drcellselected.Selected = false;

                                }
                            }
                             foreach (DataGridViewCell drcell in dataGrid_waitcar.Rows[i].Cells)
                            {
                                drcell.Selected = true;
                            }
                             dataGrid_waitcar.FirstDisplayedScrollingRowIndex = i; 
                            //dataGrid_waitcar.Visible=
                            return;    
                        }
                    }
                        

                }
                else
                {
                    MessageBox.Show(daijian_bjclpz+"未找到，请在左侧登记该车信息.", "系统提示");
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
        }
/*
        private void simpleButtonCheckIsExist_Click_1(object sender, EventArgs e)
        {
            if (textBoxPlateNumber.Text == "")
            {
                MessageBox.Show("请输入车牌号再进行查询!", "系统提示");
            }
            else
            {
                string jcclcp = textBoxPlateNumber.Text;
                if (ref_LoginPanel(jcclcp))
                {
                    MessageBox.Show("查询成功!", "系统提示");
                }
                else
                {
                    MessageBox.Show("初次检测车辆,请输入车辆信息.", "信息提示");
                }

            }

        }*/



    }
}
