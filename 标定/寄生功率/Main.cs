﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.StubHelpers;
using System.Windows.Forms.DataVisualization.Charting;
using System.Text.RegularExpressions;
using carinfor;
//using Detect;

namespace 汽油寄生功率
{
    public partial class Main : Form
    {

        public string startUpPath = Application.StartupPath;
        private MatrixEquation.MatrixEquation jsglqx_matrix = null;
        float jsglqx_a = 0.0006f, jsglqx_b = 0.0003f, jsglqx_c = 0.2075f;
        MatchEvaluator me = delegate(Match m)
        {
            return "\n";
        };
        private int testTime = 0;
        equipmentConfigInfdata configdata = new equipmentConfigInfdata();
        configIni configini = new configIni();
        parasitic parasiticdata = null;
        parasiticControl parasiticcontrol = new parasiticControl();
        BpqFl.bpxcontrol bpq = null;
        private DateTime startTime, nowtime;
        public int GKSJ = 0, perGKSJ = 0;                                                                        //工况时间
        public float gongkuangTime = 0f;
        private int gksjcount = 0;

        public List<float> Torquelist = new List<float>();
        public List<float> Velocitylist = new List<float>();
        public List<float> Powerlist = new List<float>();
        public List<float> Forcelist = new List<float>();
        //public int[] TimeCountlist = new int[10240];
       // public float[] Torquelist = new float[10240];                                               //每秒速度数组
       // public float[] Velocitylist = new float[10240];                                               //每秒扭力数
        //public float[] Powerlist = new float[10240];
        //public float[] Forcelist = new float[10240];
        private bool isSaveStartTime = false;
        CSVcontrol.CSVwriter csvwriter = new CSVcontrol.CSVwriter();

        public delegate void wtgiss(DataGridView Grid, int Row, string Cell, string Content);          //委托
        public delegate void wtb(bool tf);                                                          //委托
        public delegate void wt_void();                                                             //委托
        public delegate void wtcs(Control controlname, string text);                                //委托
        public delegate void wtlsb(Label Msgowner, string Msgstr, bool Update_DB);                  //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                                 //委托
        public delegate void wtdd(DataGridView datagrid, DataTable dt);                             //委托

        public delegate void wtfs(float Data, string ChartName);                                    //委托
        public delegate void wtds_clr(string ChartName);
        public delegate void wtdisplay_textbox(TextBox textbox, string text);
        public static float DIW = 907.2f;                                                           //DIW值
        public static float DIWF = 908f;                                                            //DIW飞轮等效惯量(由厂家提供)
        public Thread th_jsgcs = null;                                                              //寄生功率测试线程
        public Thread th_jzhx = null;                                                               //加载滑行测试线程
        public Thread th_gl = null;                                                                 //惯量测试线程
        public Thread th_bzhjz = null;                                                              //变载荷加载滑行测试线程
        public Thread th_xysj = null;                                                               //响应时间线程
        public Thread th_jzwc = null;                                                               //加载误差测试
        public Thread th_dglmn = null;                                                              //电惯量模拟误差测试
        public Thread th_jbglcs = null;                                                             //基本功能测试
        public Thread readThread = null;
        public bool th_speedDermarcate = false;
        public bool isForceDemarcate = false;
        public bool Test_Flag = false;                                                              //是否有测试正在进行
        public static int Tscs = 0;                                                                 //提示次数
        //public static  igbt = null;                                                 //测功机控制模块
        public bool comportIsReading = false;
        public int jbgl_time = 0;
        public TabPage page = null;                                                                 //当前激活的页   
        public float Speed = 0;                                                                     //车速
        public float Force = 0;                                                                     //扭矩
        public float Power = 0;                                                                     //功率
        public float Duty = 0;                                                                      //占空比
        public float Control_Speed = 0f;                                                             //控制的速度
        public float Control_Force = 0f;                                                             //控制的扭力
        public float Control_Power = 0f;                                                             //控制的功率
        public static DataTable dt = null;                                                          //全局表
        public DataTable dt_bd = null;                                                              //标定时用的表
        DataTable dt_speed = null;
        public string pattern = @"^[0-9]*$";                                                        //正则表达式

        double jsgl1 = 0, jsgl2 = 0, jsgl3 = 0, jsgl4 = 0, jsgl5 = 0, jsgl6 = 0, jsgl7 = 0, jsgl8 = 0, jsgl9 = 0, jsgl10 = 0, jsgl50 = 0;
        float speed_xishu = 1f;
        public string bpqXh;
        public string bpqMethod;
        public string comportBpx;
        public string comportBpxPz;
        public double bpqXs = 0.83;

        private string comportIgbt = "";
        private string comportIgbtPz = "";
        //说明文档
        public string File_Name1 = Application.StartupPath + @"\Rem\寄生功率滑行测试.txt";
        public string File_Name2 = Application.StartupPath + @"\Rem\加载滑行测试.txt";
        public string File_Name3 = Application.StartupPath + @"\Rem\转动惯量等效汽车质量（DIW）测试.txt";
        public string File_Name4 = Application.StartupPath + @"\Rem\变载荷加载滑行测试.txt";
        public string File_Name5 = Application.StartupPath + @"\Rem\响应时间测试.txt";
        public string File_Name6 = Application.StartupPath + @"\Rem\电惯量准确度测试.txt";
        public string File_Name7 = Application.StartupPath + @"\Rem\加载误差测试.txt";
        public string File_Name8 = Application.StartupPath + @"\Rem\设备整体要求.txt";
        public string s;

        #region IGBT通讯协议
        byte Head = 0x2A;
        //命令头“*”                      
        string Cmd_Set_Constant_Speed = "*CS";                            //设置恒速
        string Cmd_Set_Constant_Force = "*CF";                          //设置恒扭力
        string Cmd_Set_Power = "*CP";                                   //设定恒功率
        string Cmd_Set_Control = "*CC";                                 //设置PID控制值
        string Cmd_Set_Parasitic = "*CH";                               //设置寄生功
        string Cmd_Start_Speed = "*SA";                                 //启动恒速度控制
        string Cmd_Start_Force = "*SB";                                 //启动恒扭力控制
        string Cmd_Start_Power = "*SC";                                 //启动恒功率控制
        string Cmd_Stop_Control = "*SS";                                //停止控制
        string Cmd_Lifting_Up = "*LU";                                  //举升上升
        string Cmd_Lifting_Down = "*LD";                                //举升下降
        string Cmd_Demarcate = "*DA";                                   //进入标定状态
        string Cmd_Demarcate_Exit = "*DS";                              //退出标定状态
        string Cmd_Clear_Mileage = "*DC";                               //行驶里程清零
        string Cmd_Get_PID = "*DP";                                     //获取DIP控制参数
        string Cmd_Get_PID_Export = "*DV";                              //获取DIP输出值
        string Cmd_Demarcate_Speed = "*I";                              //标定速度
        string Cmd_Demarcate_Force = "*Y";                              //标定扭矩
        string Cmd_Clear_Force = "*YC";                                 //扭矩清零
        string Cmd_Start_IGBT = "*PT";                                  //开始IGBT输出
        string Cmd_Stop_IGBT = "*PS";                                   //停止IGBT输出
        string Cmd_Get_IGBT = "*PV";                                    //获取IGBT输出值
        string Cmd_Reverse_Motor = "*MO";                               //反拖电机开启
        string Cmd_Reverse_Motor_Stop = "*MF";                          //反拖电机关闭
        string Cmd_Start_Simulation = "*RT";                            //开始道路阻力模拟
        string Cmd_Stop_Simulation = "*RS";                             //停止道路阻力模拟
        string Cmd_Set_Quality = "*RV";                                 //设置基准质量
        #endregion

        #region BNTD通讯协议
        public byte[] Send_Buffer;                                      //发送缓冲区
        //public byte[] Read_Buffer;                                      //读取缓冲区
        string BNTD_Cmd_Set_Speed = "XSS";                              //BNTD设置恒速度值(km/h)
        string BNTD_Cmd_Set_Force = "XSF";                              //BNTD设置恒扭力值(N)
        string BNTD_Cmd_Set_Power = "XSP";                              //BNTD设置恒功率值(kw)
        string BNTD_Cmd_Set_Duty = "XSI";                               //设置IGBT的pwm波占空比
        string BNTD_Cmd_Set_Force_Aisle = "XSC";                        //设置力所用的通道号
        string BNTD_Cmd_Set_Clear_Force = "XSZ";                        //设置力通道清零
        string BNTD_Cmd_Set_Diameter = "XCG";                           //设置滚筒直径
        string BNTD_Cmd_Set_Pulse = "XCP";                              //设置滚筒直径
        string BNTD_Cmd_Set_Force_b0 = "XCA";                           //设置通道1力标定系数b[0]：force=b[0]*(ad_value-c[0])
        string BNTD_Cmd_Set_Force_c0 = "XCB";                           //设置通道1力标定系数c[0]：force=b[0]*(ad_value-c[0])
        string BNTD_Cmd_Set_Force_b1 = "XCC";                           //设置通道2力标定系数b[1]：force=b[1]*(ad_value-c[1])
        string BNTD_Cmd_Set_Force_c1 = "XCD";                           //设置通道2力标定系数c[1]：force=b[1]*(ad_value-c[1])
        string BNTD_Cmd_Set_Force_b2 = "XCE";                           //设置通道3力标定系数b[2]：force=b[2]*(ad_value-c[2])
        string BNTD_Cmd_Set_Force_c2 = "XCF";                           //设置通道3力标定系数c[2]：force=b[2]*(ad_value-c[2])
        string BNTD_Cmd_Set_Duty_kp = "XCH";                            //设置恒速控制PID控制比例系数kp
        string BNTD_Cmd_Set_Duty_ki = "XCI";                            //设置恒速控制PID控制比例系数ki
        string BNTD_Cmd_Set_Duty_kd = "XCJ";                            //设置恒速控制PID控制比例系数kd
        string BNTD_Cmd_Set_Duty_kp_force = "XCK";                      //设置力控制PID控制比例系数kp_force
        string BNTD_Cmd_Set_Duty_ki_force = "XCL";                      //设置力控制PID控制比例系数ki_force
        string BNTD_Cmd_Set_Duty_kd_force = "XCM";                      //设置力控制PID控制比例系数kd_force
        string BNTD_Cmd_Control_Start_Speed = "XBS";                    //启动恒速度控制
        string BNTD_Cmd_Control_Start_Force = "XBF";                    //启动恒扭力控制
        string BNTD_Cmd_Control_Start_Power = "XBP";                    //启动恒功率控制
        string BNTD_Cmd_Control_Start_Duty = "XBI";                     //启动输出设定pwm值
        string BNTD_Cmd_Control_Stop_Quit = "XBQ";                      //停止控制,pwm输出0下位机为空闲状态
        string BNTD_Cmd_Control_Motor_Start = "XEB";                    //启动电机
        string BNTD_Cmd_Control_Motor_Stop = "XES";                     //停止电机
        string BNTD_Cmd_Control_Relay_TurnOn = "XEY";                    //开继电器
        string BNTD_Cmd_Control_Relay_TurnOff = "XEN";                     //关继电器
        string BNTD_Cmd_Lifting_Up = "XJU";                             //停止控制,pwm输出0下位机为空闲状态
        string BNTD_Cmd_Lifting_Down = "XJD";                           //停止控制,pwm输出0下位机为空闲状态
        string BNTD_Cmd_Demarcate = "XDF";                              //进入标定状态
        string BNTD_Cmd_Demarcate_Exit = "XDS";                         //退出标定状态
        string BNTD_Cmd_Read_Environment = "XRT";                       //读取环境参数
        string BNTD_Cmd_Read_Force_Modulus = "XRF";                     //读取力标定系数b,c
        string BNTD_Cmd_Read_Speed_PID = "XRs";                         //读取恒速PID系数kp、ki、kp
        string BNTD_Cmd_Read_Force_PID = "XRf";                         //读取恒力PID系数kp_force、ki_force、kp_force
        string BNTD_Cmd_Read_Speed_Modulus = "XRS";                     //读取速度系数
        string BNTD_Cmd_Solidify_Force_Modulus = "XMF";                //将通道1力标定系数固化到ROM中
        string BNTD_Cmd_Solidify_Speed_Duty_Modulus = "XMs";            //将恒速控制的PID系数固化到ROM中
        string BNTD_Cmd_Solidify_Force_Duty_Modulus = "XMf";            //将恒力控制的PID系数固化到ROM中
        string BNTD_Cmd_Solidify_Speed_Modulus = "XMS";                 //将速度的直径和脉冲数固化到ROM中
        string BNTD_Cmd_Solidify_Force_Chanel = "XMC";                   //将选择的力通道1/2/3固化到ROM中
        string BNTD_Cmd_Read_Force_Chanel = "XRC";                       //将读取力通道1/2/3
        #endregion


        public System.IO.Ports.SerialPort ComPort_2;

        static bool _continue = false;

        //public byte[] Send_Buffer;                                      //发送缓冲区
        public byte[] Read_Buffer;                                      //读取缓冲区
        public static string UseMK = "BNTD";                            //使用什么模块
        //public string All_Content = "";                                   //没有解析过的返回数据
        public List<byte> All_Content_byte = new List<byte>();            //没有解析过的返回数据
        public List<float> speed_list = new List<float>();
        public List<float> force_list = new List<float>();
        public string Speed_string = "0.0";                                    //实时速度
        public string Force_string = "0.0";                                    //实时扭矩
        public string Power_string = "0.0";                                    //实时功率
        public string Duty_string = "0.0";                                     //占空比(BNTD)
        public string Msg_received = "";                                         //消息
        public bool msg_back = false;

        //以下为新成BNTD专用变量
        public string Time_Vol_1 = "0.0";                               //实时电压值1通道(BNTD)
        public string Time_Vol_2 = "0.0";                               //实时电压值2通道(BNTD)
        public string Time_Vol_3 = "0.0";                               //实时电压值3通道(BNTD)
        public string Temperature = "0.0";                              //温度(BNTD)
        public string Humidity = "0.0";                                 //湿度(BNTD)
        public string ATM = "0.0";                                      //大气压(BNTD)
        public double b0_double = 0.0;
        public double b1_double = 0.0;
        public double b2_double = 0.0;
        public double speed_double = 0.0;
        public double speed_sum = 0.0;
        public byte caculate_count = 0;
        public double force_sum = 0.0;
        public double force_double = 0.0;
        public float speed_single = 0;
        public float force_single = 0;
        public double vol_1_double = 0.0;
        public double vol_2_double = 0.0;
        public double vol_3_double = 0.0;
        public float ki_float = 0f;
        public float kp_float = 0f;
        public float kd_float = 0f;
        public float ki_force_float = 0f;
        public float kp_force_float = 0f;
        public float kd_force_float = 0f;
        public float ki_float_default = 0f;
        public float kp_float_default = 0f;
        public float kd_float_default = 0f;
        public float ki_force_default = 0f;
        public float kp_force_default = 0f;
        public float kd_force_default = 0f;
        public float ki_float_zx = 0f;
        public float kp_float_zx = 0f;
        public float kd_float_zx = 0f;
        public float ki_force_zx = 0f;
        public float kp_force_zx = 0f;
        public float kd_force_zx = 0f;
        public string b0 = "0.0";                                       //标定系数b0 1通道(BNTD)
        public string c0 = "0.0";                                       //标定系数c0 1通道(BNTD)
        public string b1 = "0.0";                                       //标定系数b1 2通道(BNTD)
        public string c1 = "0.0";                                       //标定系数c1 2通道(BNTD)
        public string b2 = "0.0";                                       //标定系数b2 3通道(BNTD)
        public string c2 = "0.0";                                       //标定系数c2 3通道(BNTD)
        public string kp = "0.0";                                       //速度PID参数P(BNTD)
        public string ki = "0.0";                                       //速度PID参数I(BNTD)
        public string kd = "0.0";                                       //速度PID参数D(BNTD)
        public string kp_force = "0.0";                                 //力PID参数P(BNTD)
        public string ki_force = "0.0";                                 //力PID参数I(BNTD)
        public string kd_force = "0.0";                                 //力PID参数D(BNTD)
        public string Speed_Diameter = "0.0";                           //滚筒直径
        public string Speed_Pusle = "0.0";                              //脉冲数
        public string forceChanel = "0.0";                              //力通道
        public byte forceChanelByte = 1;                                //力通道


        bool Read_Flag = false;                                         //是否有数据可以读取
        //private Thread Th_Lifter_Up = null;                             //举升上升线程
        public static Thread Th_Resolve = null;                         //解析线程
        public string Status = "time";                                  //IGBT状态 time——实时 Speed——恒速控制 Force——恒扭矩 Power——恒功率 Demarcate——标定 T——取环境参数(BNTD) F——取标定系数(BNTD) s——取速度PID控制参数(BNTD) f——取力PID控制参数(BNTD) S——取速度系数(BNTD) null——空闲
        public double sum = 0;
        enum testState
        {
            idle_state = 1, demarcate_state, hengding_state,
            jsgl_state
                , jzhx_state, gl_state, bzhx_state, temp_state, jzwc_state
        };
        private testState nowState = testState.idle_state;
        private testState preState = testState.idle_state;

        private bool timer_flag = false;//在惯量相关测试中用以标识速度已达最高点
        private bool testFinish = false;//用以判断惯量相关测试是否结束
        private double DIW1 = 0, DIW2 = 0, DIW3 = 0;
        private double t1 = 0, t2 = 0, t3 = 0, t4 = 0, t5 = 0, t6 = 0, t7 = 0, t8 = 0, t9 = 0, t10 = 0;
        private double t11 = 0, t12 = 0, t13 = 0, t14 = 0, t15 = 0, t16 = 0, t17 = 0, t18 = 0, t19 = 0, t20 = 0;
        private double t21 = 0, t22 = 0, t23 = 0, t24 = 0, t25 = 0, t26 = 0, t27 = 0, t28 = 0, t29 = 0, t30 = 0;
        private double t31 = 0, t32 = 0, t33 = 0, t34 = 0, t35 = 0, t36 = 0, t37 = 0, t38 = 0, t39 = 0;
        private double t40 = 0, t41 = 0, t42 = 0, t43 = 0, t44 = 0;
        private double ljsxTime = 0;
        private float bhjz_power = 0;
        private byte glTestStep = 0;//在基本惯量测试中代表第glTestStep次滑行

        private float t1_moni = 0F, t2_moni = 0F, t3_moni = 0F, t4_moni = 0F, t5_moni = 0F, t6_moni = 0F;
        private bool useMoni = true;

        private float t1_bhjz = 0F, t2_bhjz = 0F, t3_bhjz = 0F;
        private bool useMoni_bhjz = true;

        private string useMethod = "48~16";
        private string gl_power_string = "0KW滑行时间(s)";
        private float gl_power_set = 0f;

        private float real_force = 5300f;
        private float gl_force = 5300f;
        private bool use_gl_force = false;

        System.Windows.Forms.Screen[] sc;
        private int sc_width = 0;
        private int sc_height = 0;
        private bool isSaved = false;



        #region 初始化
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

            sc = System.Windows.Forms.Screen.AllScreens;
            sc_height = this.Height;
            sc_width = this.Width;
            Init();
            //this.timer1.Enabled = true;
        }

        private void initConfigInfo()
        {
            configdata = configini.getEquipConfigIni();
            labelStandard.Text = configdata.TestStandard;
            jsg_Init_Other();
            // }
            jsg_Init_Limit();
            jsg_Init_Chart();
        }

        public void Init()
        {
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            try
            {
                initConfigInfo();
                string[] Com_Items = GetCommKeys();
                ini.INIIO.GetPrivateProfileString("DIW", "DIWLX", "轻型车", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）

                ini.INIIO.GetPrivateProfileString("DIW", "DIW", "907.2", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                DIW = float.Parse(temp.ToString());

                ini.INIIO.GetPrivateProfileString("PID", "KP_SPEED_DEFAULT", "0", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                kp_float_default = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KI_SPEED_DEFAULT", "0", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                ki_float_default = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KD_SPEED_DEFAULT", "0", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                kd_float_default = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KP_FORCE_DEFAULT", "0", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                kp_force_default = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KI_FORCE_DEFAULT", "0", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                ki_force_default = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KD_FORCE_DEFAULT", "0", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                kd_force_default = float.Parse(temp.ToString());

                ini.INIIO.GetPrivateProfileString("PID", "KP_SPEED_ZX", "0", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                kp_float_zx = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KI_SPEED_ZX", "0", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                ki_float_zx = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KD_SPEED_ZX", "0", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                kd_float_zx = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KP_FORCE_ZX", "0", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                kp_force_zx = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KI_FORCE_ZX", "0", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                ki_force_zx = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KD_FORCE_ZX", "0", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                kd_force_zx = float.Parse(temp.ToString());

                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t1", "6.80", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t1_moni = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t2", "3.29", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t2_moni = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t3", "6.79", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t3_moni = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t4", "3.29", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t4_moni = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t5", "6.80", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t5_moni = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t6", "3.30", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t6_moni = float.Parse(temp.ToString());

                ini.INIIO.GetPrivateProfileString("DIW_MONI", "speed_xishu", "1.0", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                speed_xishu = float.Parse(temp.ToString());

                ini.INIIO.GetPrivateProfileString("DIW_MONI", "useMoni", "true", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                if (temp.ToString() == "true")
                    useMoni = true;
                else
                    useMoni = false;


                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t1_bhjz", "25.33", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t1_bhjz = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t2_bhjz", "15.47", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t2_bhjz = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t3_bhjz", "3.96", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t3_bhjz = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "useMoni_bhjz", "false", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                if (temp.ToString() == "true")
                    useMoni_bhjz = true;
                else
                    useMoni_bhjz = false;
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "real_force", "5300", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                real_force = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "gl_force", "5300", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                gl_force = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "use_gl_force", "5300", temp, 2048, startUpPath+"/detectConfig.ini");
                if (temp.ToString() == "true")
                    use_gl_force = true;
                else
                    use_gl_force = false;
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "useMethod", "48~16", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）

                ini.INIIO.GetPrivateProfileString("Tscs", "Tscs", "7", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                Tscs = int.Parse(temp.ToString());

                ini.INIIO.GetPrivateProfileString("配置参数", "变频器控制方式", "串口", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                bpqMethod = temp.ToString();
                ini.INIIO.GetPrivateProfileString("配置参数", "变频器系数", "0.83", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                try
                {
                    bpqXs = double.Parse(temp.ToString());
                }
                catch
                {
                    bpqXs = 0.83;
                }
                string bqpComInf;
                ini.INIIO.GetPrivateProfileString("配置参数", "变频器串口配置", "COM5/9600,N,8,1/AMB", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                bqpComInf = temp.ToString();
                comportBpx = bqpComInf.Split('/')[0];
                comportBpxPz = bqpComInf.Split('/')[1];
                bpqXh = bqpComInf.Split('/')[2];
                if (bpqMethod == "串口")
                {
                    bpq = new BpqFl.bpxcontrol(bpqXh);
                    try
                    {
                        bpq.Init_Comm(comportBpx, comportBpxPz);                           //初始化串口
                    }
                    catch
                    {
                        MessageBox.Show("变频器串口打开失败.串口：" + comportBpx + ",配置字：" + comportBpxPz, "系统提示");
                    }
                }

                ini.INIIO.GetPrivateProfileString("UseMK", "MK", "BNTD", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                UseMK = temp.ToString();


                try
                {
                    Init_Comm(configdata.Cgjck, configdata.cgjckpzz);                           //初始化串口
                }

                catch
                {
                    MessageBox.Show("底盘测试机串口打开失败.", "系统提示");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "初始化出错可能无法继续");
            }
        }

        #region 初始化串口
        /// <summary>
        /// 初始化串口
        /// </summary>
        /// <param name="PortName">串口名字</param>
        /// <param name="LinkString">连接字符串 如9600,N,8,1</param>
        /// <returns>bool</returns>
        public bool Init_Comm(string PortName, string LinkString)
        {
            try
            {
                ComPort_2 = new SerialPort();
                if (ComPort_2.IsOpen)
                    ComPort_2.Close();
                ComPort_2.PortName = PortName;
                ComPort_2.BaudRate = int.Parse(LinkString.Split(',').GetValue(0).ToString());
                switch (LinkString.Split(',').GetValue(1).ToString().ToUpper())
                {
                    case "N":
                        ComPort_2.Parity = System.IO.Ports.Parity.None;
                        break;
                    case "O":
                        ComPort_2.Parity = System.IO.Ports.Parity.Odd;
                        break;
                    case "E":
                        ComPort_2.Parity = System.IO.Ports.Parity.Even;
                        break;
                    case "M":
                        ComPort_2.Parity = System.IO.Ports.Parity.Mark;
                        break;
                    case "S":
                        ComPort_2.Parity = System.IO.Ports.Parity.Space;
                        break;
                    default:
                        ComPort_2.Parity = System.IO.Ports.Parity.None;
                        break;
                }
                ComPort_2.DataBits = int.Parse(LinkString.Split(',').GetValue(2).ToString());
                switch (LinkString.Split(',').GetValue(3).ToString())
                {
                    case "1":
                        ComPort_2.StopBits = System.IO.Ports.StopBits.One;
                        break;
                    case "2":
                        ComPort_2.StopBits = System.IO.Ports.StopBits.Two;
                        break;
                    default:
                        ComPort_2.StopBits = System.IO.Ports.StopBits.One;
                        break;
                }

                if (configdata.isIgbtContainGdyk)
                    readThread = new Thread(ResolveGd);
                else
                    readThread = new Thread(Resolve);

                //ComPort_2.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Read);
                //ComPort_2.ReceivedBytesThreshold =2;
                ComPort_2.Open();
                _continue = true;
                readThread.Start();


                //Th_Resolve = new Thread(Resolve);
                //Th_Resolve.Start();
                if (ComPort_2.IsOpen)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                throw (new ApplicationException("串口初始化出错，请检查串口是否被占用或设备配置的字符串是否正确"));
            }
        }
        #endregion

        #region 串口返回数据事件
        /// <summary>
        /// 当串口有返回数据事件时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Read(object sender, SerialDataReceivedEventArgs e)
        {
            //while (_continue)
            //{
            try
            {
                ReadData();
                Resolve();
            }
            catch (TimeoutException)
            { }
            //}

            //ReadData();
            //Resolve();
        }
        #endregion

        #region 接收数据
        /// <summary>
        /// 接收数据
        /// </summary>
        public bool ReadData()
        {
            try
            {
                //comportIsReading = true;
                if (ComPort_2.BytesToRead > 0)
                {
                    int read_buffer_length = ComPort_2.BytesToRead;
                    Read_Buffer = new byte[read_buffer_length];
                    ComPort_2.Read(Read_Buffer, 0, read_buffer_length);
                    List<byte> buffer = Read_Buffer.ToList();
                    All_Content_byte.AddRange(buffer.ToList());
                    //All_Content += Encoding.Default.GetString(buffer.ToArray());
                    return true;
                }
                else
                    return false;
                //comportIsReading = false;
            }
            catch (Exception)
            {
                ComPort_2.DiscardInBuffer();
                return false;
            }
        }
        #endregion
        int kssjok1 = 0, kssjok2 = 0, kssjok3 = 0, kssjok4 = 0, kssjok5 = 0, kssjok6= 0, kssjok7 = 0, kssjok8 = 0, kssjok9 = 0, kssjok10 = 0, kssjok11 = 0;
        int jssjok1 = 0, jssjok2 = 0, jssjok3 = 0, jssjok4 = 0, jssjok5 = 0, jssjok6 = 0, jssjok7 = 0, jssjok8 = 0, jssjok9 = 0, jssjok10 = 0, jssjok11 = 0;
        DateTime kssj1, kssj2, kssj3, kssj4, kssj5, kssj6, kssj7, kssj8, kssj9, kssj10, kssj11;
        DateTime jssj1, jssj2, jssj3, jssj4, jssj5, jssj6, jssj7, jssj8, jssj9, jssj10, jssj11;
        #region 串口返回数据解析
        /// <summary>
        /// 串口返回数据解析
        /// </summary>
        private void Resolve()
        {
            while (_continue)
            {
                //   if (!comportIsReading)
                //   {
                ReadData();
                if (All_Content_byte.Count > 16)
                {
                    int start = 0;
                    int end = 0;
                    msg_back = false;

                    try
                    {
                        if (All_Content_byte == null)
                        {
                            continue;
                        }
                        if (All_Content_byte.Count > 0)
                        {
                            int temp_start1 = 0;
                            int temp_start2 = 0;
                            //bool msg_back = false;
                            temp_start1 = All_Content_byte.IndexOf(0x41);       //A
                            temp_start2 = All_Content_byte.IndexOf(0x44);       //D
                            if ((temp_start2 < temp_start1) && (temp_start2 != -1))
                            {
                                start = temp_start2;
                                msg_back = true;
                                end = All_Content_byte.IndexOf(0x43);//如果是调试信息，则以C结尾为结尾标志
                            }
                            else if (temp_start1 != -1)
                            {
                                start = temp_start1;
                                if (All_Content_byte.Count >= start + 15)
                                    end = start + 15;
                                else
                                    continue;
                            }
                            if (start == -1)
                            {
                                //没有开始符抛弃所有返回数据
                                All_Content_byte.Clear();
                                continue;
                            }
                            //end = All_Content_byte.IndexOf(0x43);   //C
                            if (end == -1)
                                continue;
                            if (end <= start)
                            {
                                All_Content_byte.RemoveRange(0, start);
                                continue;
                            }
                            if (msg_back)                   //解析的是消息
                            {
                                try
                                {
                                    Msg_received = Encoding.Default.GetString(All_Content_byte.ToArray(), start + 1, end - start - 1);
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    continue;
                                }
                                catch (Exception)
                                {
                                }
                            }
                            string sd = Encoding.Default.GetString(All_Content_byte.ToArray(), start + 1, 2);

                            switch (Encoding.Default.GetString(All_Content_byte.ToArray(), start + 1, 2))
                            {
                                case "DF":
                                    vol_1_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    vol_2_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    vol_3_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11);
                                    Time_Vol_1 = vol_1_double.ToString("0.0000");
                                    Time_Vol_2 = vol_2_double.ToString("0.0000");
                                    Time_Vol_3 = vol_3_double.ToString("0.0000");
                                    All_Content_byte.RemoveRange(0, end + 1);

                                    break;
                                case "BS":
                                    speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);

                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "BP":
                                    speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "BF":
                                    speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "BI":
                                    speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "II":
                                    byte[] ds = new byte[] { 0xff, 0xff, 0xff, 0xff };
                                    float sdd = BitConverter.ToSingle(ds, 0);
                                    speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RT":
                                    Temperature = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString("0.0");
                                    Humidity = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString("0.0");
                                    ATM = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "Rs":
                                    kp = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                    ki = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                    kd = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString();
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "Rf":
                                    kp_force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                    ki_force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                    kd_force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString();
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RS":
                                    Speed_Diameter = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                    Speed_Pusle = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RC":
                                    forceChanelByte = All_Content_byte.ToArray().ElementAt(start + 14);
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RA":
                                    b0_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    b1_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11);
                                    b0 = b0_double.ToString();
                                    c0 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                    b1 = b1_double.ToString();
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RB":
                                    b2_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    c1 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                    b2 = b2_double.ToString();
                                    c2 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString();
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                default:
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                            }
                            if (0 <= speed_single && speed_single <= 200)
                                speed_list.Add(speed_single);
                            if (-20000 <= force_single && force_single <= 20000)
                                force_list.Add(force_single);
                            if (speed_list.Count > 20)
                                speed_list.RemoveAt(0);
                            if (force_list.Count > 20)
                                force_list.RemoveAt(0);
                            speed_sum = speed_list.Sum();
                            force_sum = force_list.Sum();
                            if (force_list.Count >= 20)
                            {
                                caculate_count++;
                                Speed = (float)speed_sum / 20f * speed_xishu;
                                Force = (float)force_sum / 20f;
                                if (caculate_count == 10)
                                {
                                    caculate_count = 0;
                                    Speed_string = Speed.ToString("0.0");
                                    Force_string = Force.ToString("0");
                                    Msg(Msg_cs, panel_cs, Speed_string, false);
                                    Msg(Msg_nl, panel_nl, Force_string, false);
                                }
                            }
                            Msg(Msg_gl, panel_gl, Power_string, false);
                            Power = (float)Convert.ToDouble(Power_string);
                            Duty = (float)Convert.ToDouble(Duty_string);
                            Msg_toollabel(toolStripLabelMessage, panel_cs, Msg_received, false);
                            switch (nowState)
                            {
                                case testState.demarcate_state:

                                    break;
                                case testState.jsgl_state:
                                    if (configdata.TestStandard != "HJT290" && configdata.TestStandard != "HJT291" && configdata.TestStandard != "JJF1221点燃式")
                                    {
                                        //t4++;
                                        if (Speed > 96)
                                        {
                                            Motor_Close();
                                            timer_flag = true;
                                            startTime = DateTime.Now;
                                            isSaveStartTime = true;
                                        }
                                        if (timer_flag == true)
                                        {
                                            if (Speed <= 92 && Speed >= 12)
                                                ljsxTime += 0.01;
                                            if (Speed <= 92 && Speed >= 84)
                                            {
                                                if (kssjok1 == 0)
                                                {
                                                    kssj1 = DateTime.Now;
                                                    kssjok1 = 1;
                                                }
                                                t1 += 0.01;
                                                dataGrid_jsgl.Rows[0].Cells["滑行时间（s）"].Value = t1.ToString("0.00");
                                            }
                                            if (Speed < 84 && Speed >= 76)
                                            {
                                                if (jssjok1 == 0)
                                                {
                                                    jssj1 = DateTime.Now;
                                                    jssjok1 = 1;
                                                }
                                                if (kssjok2 == 0)
                                                {
                                                    kssj2 = DateTime.Now;
                                                    kssjok2 = 1;
                                                }
                                                t2 += 0.01;
                                                dataGrid_jsgl.Rows[1].Cells["滑行时间（s）"].Value = t2.ToString("0.00");
                                            }
                                            if (Speed < 76 && Speed >= 68)
                                            {
                                                if (jssjok2 == 0)
                                                {
                                                    jssj2 = DateTime.Now;
                                                    jssjok2 = 1;
                                                }
                                                if (kssjok3 == 0)
                                                {
                                                    kssj3 = DateTime.Now;
                                                    kssjok3 = 1;
                                                }
                                                t3 += 0.01;
                                                dataGrid_jsgl.Rows[2].Cells["滑行时间（s）"].Value = t3.ToString("0.00");
                                            }
                                            if (Speed < 68 && Speed >= 60)
                                            {
                                                if (jssjok3 == 0)
                                                {
                                                    jssj3 = DateTime.Now;
                                                    jssjok3 = 1;
                                                }
                                                if (kssjok4 == 0)
                                                {
                                                    kssj4 = DateTime.Now;
                                                    kssjok4 = 1;
                                                }
                                                t4 += 0.01;
                                                dataGrid_jsgl.Rows[3].Cells["滑行时间（s）"].Value = t4.ToString("0.00");
                                            }
                                            if (Speed < 60 && Speed >= 52)
                                            {
                                                if (jssjok4 == 0)
                                                {
                                                    jssj4 = DateTime.Now;
                                                    jssjok4 = 1;
                                                }
                                                if (kssjok5 == 0)
                                                {
                                                    kssj5 = DateTime.Now;
                                                    kssjok5 = 1;
                                                }
                                                t5 += 0.01;
                                                dataGrid_jsgl.Rows[4].Cells["滑行时间（s）"].Value = t5.ToString("0.00");
                                            }
                                            if (Speed < 52 && Speed >= 44)
                                            {
                                                if (jssjok5 == 0)
                                                {
                                                    jssj5 = DateTime.Now;
                                                    jssjok5 = 1;
                                                }
                                                if (kssjok6 == 0)
                                                {
                                                    kssj6 = DateTime.Now;
                                                    kssjok6 = 1;
                                                }
                                                t6 += 0.01;
                                                dataGrid_jsgl.Rows[5].Cells["滑行时间（s）"].Value = t6.ToString("0.00");
                                            }
                                            if (Speed < 44 && Speed >= 36)
                                            {
                                                if (jssjok6 == 0)
                                                {
                                                    jssj6 = DateTime.Now;
                                                    jssjok6 = 1;
                                                }
                                                if (kssjok7 == 0)
                                                {
                                                    kssj7 = DateTime.Now;
                                                    kssjok7= 1;
                                                }
                                                t7 += 0.01;
                                                dataGrid_jsgl.Rows[6].Cells["滑行时间（s）"].Value = t7.ToString("0.00");
                                            }
                                            if (Speed < 36 && Speed >= 28)
                                            {
                                                if (jssjok7 == 0)
                                                {
                                                    jssj7 = DateTime.Now;
                                                    jssjok7 = 1;
                                                }
                                                if (kssjok8 == 0)
                                                {
                                                    kssj8 = DateTime.Now;
                                                    kssjok8 = 1;
                                                }
                                                t8 += 0.01;
                                                dataGrid_jsgl.Rows[7].Cells["滑行时间（s）"].Value = t8.ToString("0.00");
                                            }
                                            if (Speed < 28 && Speed >= 20)
                                            {
                                                if (jssjok8 == 0)
                                                {
                                                    jssj8 = DateTime.Now;
                                                    jssjok8 = 1;
                                                }
                                                if (kssjok9 == 0)
                                                {
                                                    kssj9 = DateTime.Now;
                                                    kssjok9 = 1;
                                                }
                                                t9 += 0.01;
                                                dataGrid_jsgl.Rows[8].Cells["滑行时间（s）"].Value = t9.ToString("0.00");
                                            }
                                            if (Speed < 20 && Speed >= 12)
                                            {
                                                if (jssjok9 == 0)
                                                {
                                                    jssj9 = DateTime.Now;
                                                    jssjok9 = 1;
                                                }
                                                if (kssjok10 == 0)
                                                {
                                                    kssj10 = DateTime.Now;
                                                    kssjok10 = 1;
                                                }
                                                t10 += 0.01;
                                                dataGrid_jsgl.Rows[9].Cells["滑行时间（s）"].Value = t10.ToString("0.00");
                                            }
                                            if (Speed <= 54 && Speed >= 46)
                                            {
                                                t11 += 0.01;
                                                dataGrid_jsgl.Rows[10].Cells["滑行时间（s）"].Value = t11.ToString("0.00");
                                            }
                                            if (Speed < 12)
                                            {
                                                if (jssjok10 == 0)
                                                {
                                                    jssj10 = DateTime.Now;
                                                    jssjok10 = 1;
                                                }
                                                testFinish = true;
                                                nowState = testState.idle_state;
                                                Exit_Control();
                                            }
                                        }

                                    }
                                    else if(configdata.TestStandard=="HJT291")
                                    {
                                        if (Speed > 56)
                                        {
                                            Motor_Close();
                                            timer_flag = true;
                                            startTime = DateTime.Now;
                                            isSaveStartTime = true;

                                        }
                                        if (timer_flag == true)
                                        {
                                            if (Speed <= 51 && Speed >= 16)
                                                ljsxTime += 0.01;
                                            if (Speed <= 51 && Speed >= 45)
                                            {
                                                if (kssjok1 == 0)
                                                {
                                                    kssj1 = DateTime.Now;
                                                    kssjok1 = 1;
                                                }
                                                t1 += 0.01;
                                                dataGrid_jsgl.Rows[0].Cells["滑行时间（s）"].Value = t1.ToString("0.00");
                                            }
                                            if (Speed < 45)
                                            {
                                                if (jssjok1 == 0)
                                                {
                                                    jssj1 = DateTime.Now;
                                                    jssjok1 = 1;
                                                }
                                            }
                                            if (Speed <= 48 && Speed >= 32)
                                            {                                                
                                                if (kssjok2 == 0)
                                                {
                                                    kssj2 = DateTime.Now;
                                                    kssjok2 = 1;
                                                }
                                                if (!isSaveStartTime)
                                                {
                                                    startTime = DateTime.Now;
                                                    isSaveStartTime = true;
                                                }
                                                t2 += 0.01;
                                                dataGrid_jsgl.Rows[1].Cells["滑行时间（s）"].Value = t2.ToString("0.00");
                                            }
                                            if(Speed<32)
                                            {
                                                if (jssjok2 == 0)
                                                {
                                                    jssj2 = DateTime.Now;
                                                    jssjok2 = 1;
                                                }
                                            }
                                            if (Speed <= 40 && Speed >= 24)
                                            {
                                                if (kssjok3 == 0)
                                                {
                                                    kssj3 = DateTime.Now;
                                                    kssjok3 = 1;
                                                }
                                                t3 += 0.01;
                                                dataGrid_jsgl.Rows[2].Cells["滑行时间（s）"].Value = t3.ToString("0.00");
                                            }
                                            if (Speed < 24)
                                            {
                                                if (jssjok3 == 0)
                                                {
                                                    jssj3 = DateTime.Now;
                                                    jssjok3 = 1;
                                                }
                                            }
                                            if (Speed <= 33 && Speed >= 17)
                                            {
                                                if (kssjok4 == 0)
                                                {
                                                    kssj4 = DateTime.Now;
                                                    kssjok4 = 1;
                                                }
                                                t4 += 0.01;
                                                dataGrid_jsgl.Rows[3].Cells["滑行时间（s）"].Value = t4.ToString("0.00");
                                            }
                                            if (Speed < 17)
                                            {
                                                if (jssjok4 == 0)
                                                {
                                                    jssj4 = DateTime.Now;
                                                    jssjok4 = 1;
                                                }
                                            }
                                            if (Speed <= 54 && Speed >= 46)
                                            {
                                                t11 += 0.01;
                                                dataGrid_jsgl.Rows[10].Cells["滑行时间（s）"].Value = t11.ToString("0.00");
                                            }
                                            if (Speed < 16)
                                            {
                                                testFinish = true;
                                                nowState = testState.idle_state;
                                                Exit_Control();
                                            }

                                        }

                                    }
                                    else
                                    {
                                        if (Speed > 56)
                                        {
                                            Motor_Close();
                                            timer_flag = true;
                                            startTime = DateTime.Now;
                                            isSaveStartTime = true;

                                        }
                                        if (timer_flag == true)
                                        {
                                            if (Speed <= 51 && Speed >= 16)
                                                ljsxTime += 0.01;
                                            if (Speed <= 51 && Speed >= 45)
                                            {
                                                if (kssjok1 == 0)
                                                {
                                                    kssj1 = DateTime.Now;
                                                    kssjok1 = 1;
                                                }
                                                t1 += 0.01;
                                                dataGrid_jsgl.Rows[0].Cells["滑行时间（s）"].Value = t1.ToString("0.00");
                                            }
                                            if (Speed < 45)
                                            {
                                                if (jssjok1 == 0)
                                                {
                                                    jssj1 = DateTime.Now;
                                                    jssjok1 = 1;
                                                }
                                            }
                                            if (Speed <= 48 && Speed >= 32)
                                            {
                                                if (kssjok2 == 0)
                                                {
                                                    kssj2 = DateTime.Now;
                                                    kssjok2 = 1;
                                                }
                                                if (!isSaveStartTime)
                                                {
                                                    startTime = DateTime.Now;
                                                    isSaveStartTime = true;
                                                }
                                                t2 += 0.01;
                                                dataGrid_jsgl.Rows[1].Cells["滑行时间（s）"].Value = t2.ToString("0.00");
                                            }
                                            if (Speed < 32)
                                            {
                                                if (jssjok2 == 0)
                                                {
                                                    jssj2 = DateTime.Now;
                                                    jssjok2 = 1;
                                                }
                                            }
                                            if (Speed <= 40 && Speed >= 24)
                                            {
                                                if (kssjok3 == 0)
                                                {
                                                    kssj3 = DateTime.Now;
                                                    kssjok3 = 1;
                                                }
                                                t3 += 0.01;
                                                dataGrid_jsgl.Rows[2].Cells["滑行时间（s）"].Value = t3.ToString("0.00");
                                            }
                                            if (Speed < 24)
                                            {
                                                if (jssjok3 == 0)
                                                {
                                                    jssj3 = DateTime.Now;
                                                    jssjok3 = 1;
                                                }
                                            }
                                            if (Speed <= 32 && Speed >= 16)
                                            {
                                                if (kssjok4 == 0)
                                                {
                                                    kssj4 = DateTime.Now;
                                                    kssjok4 = 1;
                                                }
                                                t4 += 0.01;
                                                dataGrid_jsgl.Rows[3].Cells["滑行时间（s）"].Value = t4.ToString("0.00");
                                            }
                                            if (Speed < 16)
                                            {
                                                if (jssjok4 == 0)
                                                {
                                                    jssj4 = DateTime.Now;
                                                    jssjok4 = 1;
                                                }
                                            }
                                            if (Speed <= 54 && Speed >= 46)
                                            {
                                                t11 += 0.01;
                                                dataGrid_jsgl.Rows[10].Cells["滑行时间（s）"].Value = t11.ToString("0.00");
                                            }
                                            if (Speed < 16)
                                            {
                                                testFinish = true;
                                                nowState = testState.idle_state;
                                                Exit_Control();
                                            }

                                        }

                                    }
                                    break;

                                default: break;

                            }
                        }
                        //if (All_Content_byte.Count > 160)
                        //All_Content_byte.RemoveRange(0, All_Content_byte.Count - 145);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            All_Content_byte.RemoveRange(0, end + 1);
                        }
                        catch (Exception)
                        {
                            All_Content_byte.Clear();
                        }
                    }
                }
                //Thread.Sleep(1);
            }

        }
        #endregion
        #region 串口返回数据解析
        /// <summary>
        /// 串口返回数据解析
        /// </summary>
        private void ResolveGd()
        {
            while (_continue)
            {
                //   if (!comportIsReading)
                //   {
                ReadData();
                if (All_Content_byte.Count > 17)
                {
                    int start = 0;
                    int end = 0;
                    msg_back = false;

                    try
                    {
                        if (All_Content_byte == null)
                        {
                            continue;
                        }
                        if (All_Content_byte.Count > 0)
                        {
                            int temp_start1 = 0;
                            int temp_start2 = 0;
                            //bool msg_back = false;
                            temp_start1 = All_Content_byte.IndexOf(0x41);       //A
                            temp_start2 = All_Content_byte.IndexOf(0x44);       //D
                            if ((temp_start2 < temp_start1) && (temp_start2 != -1))
                            {
                                start = temp_start2;
                                msg_back = true;
                                end = All_Content_byte.IndexOf(0x43);//如果是调试信息，则以C结尾为结尾标志
                            }
                            else if (temp_start1 != -1)
                            {
                                start = temp_start1;
                                if (All_Content_byte.Count >= start + 16)
                                    end = start + 16;
                                else
                                    continue;
                            }
                            if (start == -1)
                            {
                                //没有开始符抛弃所有返回数据
                                All_Content_byte.Clear();
                                continue;
                            }
                            //end = All_Content_byte.IndexOf(0x43);   //C
                            if (end == -1)
                                continue;
                            if (end <= start)
                            {
                                All_Content_byte.RemoveRange(0, start);
                                continue;
                            }
                            if (msg_back)                   //解析的是消息
                            {
                                try
                                {
                                    Msg_received = Encoding.Default.GetString(All_Content_byte.ToArray(), start + 1, end - start - 1);
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    continue;
                                }
                                catch (Exception)
                                {
                                }
                            }
                            string sd = Encoding.Default.GetString(All_Content_byte.ToArray(), start + 1, 2);

                            switch (Encoding.Default.GetString(All_Content_byte.ToArray(), start + 1, 2))
                            {
                                case "DF":
                                    vol_1_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    vol_2_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    vol_3_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11);
                                    Time_Vol_1 = vol_1_double.ToString("0.0000");
                                    Time_Vol_2 = vol_2_double.ToString("0.0000");
                                    Time_Vol_3 = vol_3_double.ToString("0.0000");
                                    All_Content_byte.RemoveRange(0, end + 1);

                                    break;
                                case "BS":
                                    speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);

                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "BP":
                                    speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "BF":
                                    speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "BI":
                                    speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "II":
                                    byte[] ds = new byte[] { 0xff, 0xff, 0xff, 0xff };
                                    float sdd = BitConverter.ToSingle(ds, 0);
                                    speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RT":
                                    Temperature = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString("0.0");
                                    Humidity = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString("0.0");
                                    ATM = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "Rs":
                                    kp = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                    ki = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                    kd = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString();
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "Rf":
                                    kp_force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                    ki_force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                    kd_force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString();
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RS":
                                    Speed_Diameter = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                    Speed_Pusle = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RC":
                                    forceChanelByte = All_Content_byte.ToArray().ElementAt(start + 14);
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RA":
                                    b0_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    b1_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11);
                                    b0 = b0_double.ToString();
                                    c0 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                    b1 = b1_double.ToString();
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RB":
                                    b2_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    c1 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                    b2 = b2_double.ToString();
                                    c2 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString();
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                default:
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                            }
                            if (0 <= speed_single && speed_single <= 200)
                                speed_list.Add(speed_single);
                            if (-20000 <= force_single && force_single <= 20000)
                                force_list.Add(force_single);
                            if (speed_list.Count > 20)
                                speed_list.RemoveAt(0);
                            if (force_list.Count > 20)
                                force_list.RemoveAt(0);
                            speed_sum = speed_list.Sum();
                            force_sum = force_list.Sum();
                            if (force_list.Count >= 20)
                            {
                                caculate_count++;
                                Speed = (float)speed_sum / 20f * speed_xishu;
                                Force = (float)force_sum / 20f;
                                if (caculate_count == 10)
                                {
                                    caculate_count = 0;
                                    Speed_string = Speed.ToString("0.0");
                                    Force_string = Force.ToString("0");
                                    Msg(Msg_cs, panel_cs, Speed_string, false);
                                    Msg(Msg_nl, panel_nl, Force_string, false);
                                }
                            }
                            Msg(Msg_gl, panel_gl, Power_string, false);
                            Power = (float)Convert.ToDouble(Power_string);
                            Duty = (float)Convert.ToDouble(Duty_string);
                            Msg_toollabel(toolStripLabelMessage, panel_cs, Msg_received, false);
                            switch (nowState)
                            {
                                case testState.demarcate_state:

                                    break;
                                case testState.jsgl_state:
                                    if (configdata.TestStandard != "HJT290" && configdata.TestStandard != "HJT291" && configdata.TestStandard != "JJF1221点燃式")
                                    {
                                        //t4++;
                                        if (Speed > 96)
                                        {
                                            Motor_Close();
                                            timer_flag = true;
                                            startTime = DateTime.Now;
                                            isSaveStartTime = true;
                                        }
                                        if (timer_flag == true)
                                        {
                                            if (Speed <= 92 && Speed >= 12)
                                                ljsxTime += 0.01;
                                            if (Speed <= 92 && Speed >= 84)
                                            {
                                                t1 += 0.01;
                                                dataGrid_jsgl.Rows[0].Cells["滑行时间（s）"].Value = t1.ToString("0.00");
                                            }
                                            if (Speed < 84 && Speed >= 76)
                                            {
                                                t2 += 0.01;
                                                dataGrid_jsgl.Rows[1].Cells["滑行时间（s）"].Value = t2.ToString("0.00");
                                            }
                                            if (Speed < 76 && Speed >= 68)
                                            {
                                                t3 += 0.01;
                                                dataGrid_jsgl.Rows[2].Cells["滑行时间（s）"].Value = t3.ToString("0.00");
                                            }
                                            if (Speed < 68 && Speed >= 60)
                                            {
                                                t4 += 0.01;
                                                dataGrid_jsgl.Rows[3].Cells["滑行时间（s）"].Value = t4.ToString("0.00");
                                            }
                                            if (Speed < 60 && Speed >= 52)
                                            {
                                                t5 += 0.01;
                                                dataGrid_jsgl.Rows[4].Cells["滑行时间（s）"].Value = t5.ToString("0.00");
                                            }
                                            if (Speed < 52 && Speed >= 44)
                                            {
                                                t6 += 0.01;
                                                dataGrid_jsgl.Rows[5].Cells["滑行时间（s）"].Value = t6.ToString("0.00");
                                            }
                                            if (Speed < 44 && Speed >= 36)
                                            {
                                                t7 += 0.01;
                                                dataGrid_jsgl.Rows[6].Cells["滑行时间（s）"].Value = t7.ToString("0.00");
                                            }
                                            if (Speed < 36 && Speed >= 28)
                                            {
                                                t8 += 0.01;
                                                dataGrid_jsgl.Rows[7].Cells["滑行时间（s）"].Value = t8.ToString("0.00");
                                            }
                                            if (Speed < 28 && Speed >= 20)
                                            {
                                                t9 += 0.01;
                                                dataGrid_jsgl.Rows[8].Cells["滑行时间（s）"].Value = t9.ToString("0.00");
                                            }
                                            if (Speed < 20 && Speed >= 12)
                                            {
                                                t10 += 0.01;
                                                dataGrid_jsgl.Rows[9].Cells["滑行时间（s）"].Value = t10.ToString("0.00");
                                            }
                                            if (Speed <= 54 && Speed >= 46)
                                            {
                                                t11 += 0.01;
                                                dataGrid_jsgl.Rows[10].Cells["滑行时间（s）"].Value = t11.ToString("0.00");
                                            }
                                            if (Speed < 12)
                                            {
                                                testFinish = true;
                                                nowState = testState.idle_state;
                                                Exit_Control();
                                            }
                                        }

                                    }
                                    else if (configdata.TestStandard == "HJT291")
                                    {
                                        if (Speed > 56)
                                        {
                                            Motor_Close();
                                            timer_flag = true;
                                            startTime = DateTime.Now;
                                            isSaveStartTime = true;

                                        }
                                        if (timer_flag == true)
                                        {
                                            if (Speed <= 51 && Speed >= 16)
                                                ljsxTime += 0.01;
                                            if (Speed <= 51 && Speed >= 45)
                                            {
                                                t1 += 0.01;
                                                dataGrid_jsgl.Rows[0].Cells["滑行时间（s）"].Value = t1.ToString("0.00");
                                            }
                                            if (Speed <= 48 && Speed >= 32)
                                            {
                                                if (!isSaveStartTime)
                                                {
                                                    startTime = DateTime.Now;
                                                    isSaveStartTime = true;
                                                }
                                                t2 += 0.01;
                                                dataGrid_jsgl.Rows[1].Cells["滑行时间（s）"].Value = t2.ToString("0.00");
                                            }
                                            if (Speed <= 40 && Speed >= 24)
                                            {
                                                t3 += 0.01;
                                                dataGrid_jsgl.Rows[2].Cells["滑行时间（s）"].Value = t3.ToString("0.00");
                                            }
                                            if (Speed <= 33 && Speed >= 17)
                                            {

                                                t4 += 0.01;
                                                dataGrid_jsgl.Rows[3].Cells["滑行时间（s）"].Value = t4.ToString("0.00");
                                            }
                                            if (Speed <= 54 && Speed >= 46)
                                            {
                                                t11 += 0.01;
                                                dataGrid_jsgl.Rows[10].Cells["滑行时间（s）"].Value = t11.ToString("0.00");
                                            }
                                            if (Speed < 16)
                                            {
                                                testFinish = true;
                                                nowState = testState.idle_state;
                                                Exit_Control();
                                            }

                                        }

                                    }
                                    else
                                    {
                                        if (Speed > 56)
                                        {
                                            Motor_Close();
                                            timer_flag = true;
                                            startTime = DateTime.Now;
                                            isSaveStartTime = true;

                                        }
                                        if (timer_flag == true)
                                        {
                                            if (Speed <= 51 && Speed >= 16)
                                                ljsxTime += 0.01;
                                            if (Speed <= 51 && Speed >= 45)
                                            {
                                                t1 += 0.01;
                                                dataGrid_jsgl.Rows[0].Cells["滑行时间（s）"].Value = t1.ToString("0.00");
                                            }
                                            if (Speed <= 48 && Speed >= 32)
                                            {
                                                if (!isSaveStartTime)
                                                {
                                                    startTime = DateTime.Now;
                                                    isSaveStartTime = true;
                                                }
                                                t2 += 0.01;
                                                dataGrid_jsgl.Rows[1].Cells["滑行时间（s）"].Value = t2.ToString("0.00");
                                            }
                                            if (Speed <= 40 && Speed >= 24)
                                            {
                                                t3 += 0.01;
                                                dataGrid_jsgl.Rows[2].Cells["滑行时间（s）"].Value = t3.ToString("0.00");
                                            }
                                            if (Speed <= 32 && Speed >= 16)
                                            {

                                                t4 += 0.01;
                                                dataGrid_jsgl.Rows[3].Cells["滑行时间（s）"].Value = t4.ToString("0.00");
                                            }
                                            if (Speed <= 54 && Speed >= 46)
                                            {
                                                t11 += 0.01;
                                                dataGrid_jsgl.Rows[10].Cells["滑行时间（s）"].Value = t11.ToString("0.00");
                                            }
                                            if (Speed < 16)
                                            {
                                                testFinish = true;
                                                nowState = testState.idle_state;
                                                Exit_Control();
                                            }

                                        }

                                    }
                                    break;

                                default: break;

                            }
                        }
                        //if (All_Content_byte.Count > 160)
                        //All_Content_byte.RemoveRange(0, All_Content_byte.Count - 145);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            All_Content_byte.RemoveRange(0, end + 1);
                        }
                        catch (Exception)
                        {
                            All_Content_byte.Clear();
                        }
                    }
                }
                //Thread.Sleep(1);
            }

        }
        #endregion
        public static string[] GetCommKeys()
        {
            string[] values = null;
            try
            {
                RegistryKey hklm = Registry.LocalMachine;
                RegistryKey hs = hklm.OpenSubKey("HARDWARE\\DEVICEMAP\\SERIALCOMM");
                if (hs != null)
                {
                    values = new string[hs.ValueCount];
                    for (int i = 0; i < hs.ValueCount; i++)
                    {
                        values[i] = hs.GetValue(hs.GetValueNames()[i]).ToString();
                    }
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
            return values;
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
        public void Msg_toollabel(ToolStripLabel Msgowner, Panel Msgfather, string Msgstr, bool Update_DB)
        {
            Msgowner.Text = Msgstr;
            //BeginInvoke(new wtlsb(Msg_Show), Msgowner, Msgstr, Update_DB);
            // BeginInvoke(new wtlp(Msg_Position), Msgowner, Msgfather);
        }

        public void Msg_Show(Label Msgowner, string Msgstr, bool Update_DB)
        {
            Msgowner.Text = Msgstr;
            if (Update_DB)
            {
                //CarWait.bjcl.JCZT = Msgstr;
                //CarWait.bjclxx.Update(CarWait.bjcl);
            }
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



        #region 寄生功率测试(Test Complete1)



        /// <summary>
        /// 初始化限值
        /// </summary>
        public void jsg_Init_Limit()
        {
        }

        /// <summary>
        /// 初始化图表
        /// </summary>
        public void jsg_Init_Chart()
        {
        }

        /// <summary>
        /// 初始化其他
        /// </summary>
        public void jsg_Init_Other()
        {
            dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add("速度区间（Km/h）");
            dt.Columns.Add("名义速度（Km/h）");
            dt.Columns.Add("滑行时间（s）");
            dt.Columns.Add("寄生功率（Kw）");
            if (configdata.TestStandard == "HJT290" || configdata.TestStandard == "JJF1221点燃式")
            {
                for (int i = 0; i < 4; i++)
                {
                    dr = dt.NewRow();
                    if (i == 0)
                    {
                        dr["速度区间（Km/h）"] = (51 - i * 8).ToString() + "-" + (53 - (i + 1) * 8).ToString();
                    }
                    else
                    {
                        dr["速度区间（Km/h）"] = (56 - i * 8).ToString() + "-" + (56 - (i + 2) * 8).ToString();
                    }
                    dr["名义速度（Km/h）"] = (48 - i * 8).ToString();
                    dt.Rows.Add(dr);
                }
                dr = dt.NewRow();
                dr["速度区间（Km/h）"] = "54-46";
                dr["名义速度（Km/h）"] = "50";
                dt.Rows.Add(dr);
            }
            else if (configdata.TestStandard == "HJT291")
            {
                
                
                dr = dt.NewRow();
                dr["速度区间（Km/h）"] = "51-45";
                dr["名义速度（Km/h）"] = "48";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["速度区间（Km/h）"] = "48-32";
                dr["名义速度（Km/h）"] = "40";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["速度区间（Km/h）"] = "40-24";
                dr["名义速度（Km/h）"] = "32";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["速度区间（Km/h）"] = "33-17";
                dr["名义速度（Km/h）"] = "25";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["速度区间（Km/h）"] = "54-46";
                dr["名义速度（Km/h）"] = "50";
                dt.Rows.Add(dr);
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    dr = dt.NewRow();
                    dr["速度区间（Km/h）"] = (92 - i * 8).ToString() + "-" + (92 - (i + 1) * 8).ToString();
                    dr["名义速度（Km/h）"] = (88 - i * 8).ToString();
                    dt.Rows.Add(dr);
                }
                dr = dt.NewRow();
                dr["速度区间（Km/h）"] = "54-46";
                dr["名义速度（Km/h）"] = "50";
                dt.Rows.Add(dr);
            }
            dataGrid_jsgl.DataSource = dt;

        }

        private void button_start_jsg_Click(object sender, EventArgs e)
        {
            if (!Test_Flag)                             //判断是不是有测试正在进行
            {
                kssjok1 = 0;
                kssjok2 = 0;
                kssjok3 = 0;
                kssjok4 = 0;
                kssjok5 = 0;
                kssjok6 = 0;
                kssjok7 = 0;
                kssjok8= 0;
                kssjok9 = 0;
                kssjok10 = 0;
                kssjok11 = 0;
                jssjok1 = 0;
                jssjok2 = 0;
                jssjok3 = 0;
                jssjok4 = 0;
                jssjok5 = 0;
                jssjok6 = 0;
                jssjok7 = 0;
                jssjok8 = 0;
                jssjok9 = 0;
                jssjok10 = 0;
                jssjok11 = 0;
                t1 = 0f;
                t2 = 0f;
                t3 = 0f;
                t4 = 0f;
                t5 = 0f;
                t6 = 0f;
                t7 = 0f;
                t8 = 0f;
                t9 = 0f;
                t10 = 0f;
                ljsxTime = 0;
                th_jsgcs = new Thread(Jsgcs_exe);       //新建线程
                th_jsgcs.Start();                       //线程开始执行
                testFinish = false;                     //测试还未结束
                timer_flag = false;
                Test_Flag = true;
                button_start_jsg.Text = "重新开始";
                isSaveStartTime = false;
                timer2.Start();
                nowState = testState.jsgl_state;
            }
            else
                MessageBox.Show("有测试正在进行，无法开始测试", "系统提示");
        }

        private void button_JsglText_Click(object sender, EventArgs e)
        {
            try
            {
                StreamReader sr = File.OpenText(File_Name1);
                s = sr.ReadToEnd();
                ShowText showtext = new ShowText(s);
                if (showtext.ShowDialog() == DialogResult.OK)
                {
                    s = showtext.RichTextValue;
                }
                sr.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("文档不存在或者读取地址不对，请检查", "系统提示");
            }
        }
        /// <summary>
        /// 寄生功率测试
        /// </summary>
        public void Jsgcs_exe()
        {

            jsgl1 = 0;
            jsgl2 = 0;
            jsgl3 = 0;
            jsgl4 = 0;
            jsgl5 = 0;
            jsgl6 = 0;
            jsgl7 = 0;
            jsgl8 = 0;
            jsgl9 = 0;
            jsgl10 = 0;
            Lifter_Down();         //下降举升
            Thread.Sleep(6000);
            if (configdata.TestStandard == "HJT290" || configdata.TestStandard == "HJT291" || configdata.TestStandard == "JJF1221点燃式")
            {

                Motor_Open(70 * bpqXs);
            }
            else
            {

                Motor_Open(96 * bpqXs);
            }
            while (testFinish == false)
            {
                Thread.Sleep(100);         //等待2秒
            }
            isSaveStartTime = false;
            timer2.Stop();
            Exit_Control();
            //计算
            if (configdata.TestStandard == "HJT290" ||  configdata.TestStandard == "JJF1221点燃式")
            {

                jsgl1 = Math.Round(0.02222 * DIW / t1, 2);
                jsgl2 = Math.Round(0.00123457 * 40 * DIW / t2, 2);
                jsgl3 = Math.Round(0.00123457 * 32 * DIW / t3, 2);
                jsgl4 = Math.Round(0.00123457 * 24 * DIW / t4, 2);
                jsgl50 = Math.Round(0.030864 * DIW / t11, 3);
            }
            else if ( configdata.TestStandard == "HJT291" )
            {

                jsgl1 = Math.Round(0.02222 * DIW / t1, 2);
                jsgl2 = Math.Round(0.00123457 * 40 * DIW / t2, 2);
                jsgl3 = Math.Round(0.00123457 * 32 * DIW / t3, 2);
                jsgl4 = Math.Round(0.00123457 * 25 * DIW / t4, 2);
                jsgl50 = Math.Round(0.030864 * DIW / t11, 3);
            }
            else
            {

                jsgl1 = Math.Round(0.00061728 * 88 * DIW / t1, 2);
                jsgl2 = Math.Round(0.00061728 * 80 * DIW / t2, 2);
                jsgl3 = Math.Round(0.00061728 * 72 * DIW / t3, 2);
                jsgl4 = Math.Round(0.00061728 * 64 * DIW / t4, 2);
                jsgl5 = Math.Round(0.00061728 * 56 * DIW / t5, 2);
                jsgl6 = Math.Round(0.00061728 * 48 * DIW / t6, 2);
                jsgl7 = Math.Round(0.00061728 * 40 * DIW / t7, 2);
                jsgl8 = Math.Round(0.00061728 * 32 * DIW / t8, 2);
                jsgl9 = Math.Round(0.00061728 * 24 * DIW / t9, 2);
                jsgl10 = Math.Round(0.00061728 * 16 * DIW / t10, 2);
                jsgl50 = Math.Round(0.030864 * DIW / t11, 3);
            }
            //显示和记录


            dataGrid_jsgl.Rows[0].Cells["寄生功率（Kw）"].Value = jsgl1.ToString("0.00");

            dataGrid_jsgl.Rows[1].Cells["寄生功率（Kw）"].Value = jsgl2.ToString("0.00");

            dataGrid_jsgl.Rows[2].Cells["寄生功率（Kw）"].Value = jsgl3.ToString("0.00");

            dataGrid_jsgl.Rows[3].Cells["寄生功率（Kw）"].Value = jsgl4.ToString("0.00");


            if (configdata.TestStandard != "HJT290" && configdata.TestStandard != "HJT291" && configdata.TestStandard != "JJF1221点燃式")
            {
                //dataGrid_jsgl.Rows[4].Cells["滑行时间（s）"].Value = t5.ToString("0.00");
                dataGrid_jsgl.Rows[4].Cells["寄生功率（Kw）"].Value = jsgl5.ToString("0.00");
                //dataGrid_jsgl.Rows[5].Cells["滑行时间（s）"].Value = t6.ToString("0.00");
                dataGrid_jsgl.Rows[5].Cells["寄生功率（Kw）"].Value = jsgl6.ToString("0.00");
                //dataGrid_jsgl.Rows[6].Cells["滑行时间（s）"].Value = t7.ToString("0.00");
                dataGrid_jsgl.Rows[6].Cells["寄生功率（Kw）"].Value = jsgl7.ToString("0.00");
                //dataGrid_jsgl.Rows[7].Cells["滑行时间（s）"].Value = t8.ToString("0.00");
                dataGrid_jsgl.Rows[7].Cells["寄生功率（Kw）"].Value = jsgl8.ToString("0.00");
                //dataGrid_jsgl.Rows[8].Cells["滑行时间（s）"].Value = t9.ToString("0.00");
                dataGrid_jsgl.Rows[8].Cells["寄生功率（Kw）"].Value = jsgl9.ToString("0.00");
                //dataGrid_jsgl.Rows[9].Cells["滑行时间（s）"].Value = t10.ToString("0.00");
                dataGrid_jsgl.Rows[9].Cells["寄生功率（Kw）"].Value = jsgl10.ToString("0.00");
                dataGrid_jsgl.Rows[10].Cells["寄生功率（Kw）"].Value = jsgl50.ToString("0.00");

            }
            else
            {
                dataGrid_jsgl.Rows[4].Cells["寄生功率（Kw）"].Value = jsgl50.ToString("0.00");
            }
            if (configdata.TestStandard == "HJT290" || configdata.TestStandard == "HJT291")
            {
                Ref_Control_Text(label_24jsg, jsgl4.ToString("0.00"));
                Ref_Control_Text(label_40jsg, jsgl2.ToString("0.00"));
            }
            else
            {
                Ref_Control_Text(label_24jsg, jsgl9.ToString("0.00"));
                Ref_Control_Text(label_40jsg, jsgl7.ToString("0.00"));
            }
            //写文件
            if (configdata.TestStandard == "HJT290" || configdata.TestStandard == "HJT291" || configdata.TestStandard == "JJF1221点燃式")
            {
                ini.INIIO.WritePrivateProfileString("寄生功率(日常测试)", "48Km/h", jsgl1.ToString("0.00"), startUpPath+"/detectConfig.ini");
                ini.INIIO.WritePrivateProfileString("寄生功率(日常测试)", "40Km/h", jsgl2.ToString("0.00"), startUpPath+"/detectConfig.ini");
                ini.INIIO.WritePrivateProfileString("寄生功率(日常测试)", "32Km/h", jsgl3.ToString("0.00"), startUpPath+"/detectConfig.ini");
                ini.INIIO.WritePrivateProfileString("寄生功率(日常测试)", "24Km/h", jsgl4.ToString("0.00"), startUpPath+"/detectConfig.ini");
                parasiticdata = new parasitic();
                parasiticdata.Sdqj = "51-45,48-32,40-24,32-16";
                if(configdata.TestStandard=="HJT291")
                    parasiticdata.Sdqj = "51-45,48-32,40-24,33-17";
                parasiticdata.Mysd = "48,40,32,24";
                parasiticdata.Acdt40 = t2.ToString("0.000");
                parasiticdata.Acdt24 = t4.ToString("0.000");
                parasiticdata.Plhp40 = jsgl2.ToString("0.000");
                parasiticdata.Plhp24 = jsgl4.ToString("0.000");
                parasiticdata.kssj=kssj1.ToString("yyyy-MM-dd HH:mm:ss.fff")+","+ kssj2.ToString("yyyy-MM-dd HH:mm:ss.fff") + ","+kssj3.ToString("yyyy-MM-dd HH:mm:ss.fff") + ","+kssj4.ToString("yyyy-MM-dd HH:mm:ss.fff");
                parasiticdata.jssj = jssj1.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + jssj2.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + jssj3.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + jssj4.ToString("yyyy-MM-dd HH:mm:ss.fff");
                parasiticdata.Diw = DIW.ToString("0.0");
                parasiticdata.Hxsj = t1.ToString("0.00") + "," + t2.ToString("0.00") + "," + t3.ToString("0.00") + "," + t4.ToString("0.00");
                parasiticdata.Jsgl = jsgl1.ToString("0.00") + "," + jsgl2.ToString("0.00") + "," + jsgl3.ToString("0.00") + "," + jsgl4.ToString("0.00");
                parasiticdata.Ljhxsj = ljsxTime.ToString("0.00");
                parasiticdata.Bz = startTime.ToString("yyyy-MM-dd HH:mm:ss") + "," + DIW.ToString();
                parasiticdata.Bdjg = "合格";
            }
            else
            {
                ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "88Km/h", jsgl1.ToString("0.00"), startUpPath+"/detectConfig.ini");
                ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "80Km/h", jsgl2.ToString("0.00"), startUpPath+"/detectConfig.ini");
                ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "72Km/h", jsgl3.ToString("0.00"), startUpPath+"/detectConfig.ini");
                ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "64Km/h", jsgl4.ToString("0.00"), startUpPath+"/detectConfig.ini");
                ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "56Km/h", jsgl5.ToString("0.00"), startUpPath+"/detectConfig.ini");
                ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "48Km/h", jsgl6.ToString("0.00"), startUpPath+"/detectConfig.ini");
                ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "40Km/h", jsgl7.ToString("0.00"), startUpPath+"/detectConfig.ini");
                ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "32Km/h", jsgl8.ToString("0.00"), startUpPath+"/detectConfig.ini");
                ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "24Km/h", jsgl9.ToString("0.00"), startUpPath+"/detectConfig.ini");
                ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "16Km/h", jsgl10.ToString("0.00"), startUpPath+"/detectConfig.ini");
                parasiticdata = new parasitic();
                parasiticdata.Sdqj = "92-84,84-76,76-68,68-60,60-52,52-44,44-36,36-28,28-20,20-12";
                parasiticdata.Mysd = "88,80,72,64,56,48,40,32,24,16";
                parasiticdata.Acdt40 = t7.ToString("0.000");
                parasiticdata.Acdt24 = t9.ToString("0.000");
                parasiticdata.Plhp40 = jsgl7.ToString("0.000");
                parasiticdata.Plhp24 = jsgl9.ToString("0.000");
                parasiticdata.Diw = DIW.ToString("0.0");
                parasiticdata.Hxsj = t1.ToString("0.00") + "," + t2.ToString("0.00") + "," + t3.ToString("0.00") + "," + t4.ToString("0.00") + "," + t5.ToString("0.00") + ","
                    + t6.ToString("0.00") + "," + t7.ToString("0.00") + "," + t8.ToString("0.00") + "," + t9.ToString("0.00") + "," + t10.ToString("0.00");
                parasiticdata.Jsgl = jsgl1.ToString("0.00") + "," + jsgl2.ToString("0.00") + "," + jsgl3.ToString("0.00") + "," + jsgl4.ToString("0.00")
                    + "," + jsgl5.ToString("0.00") + "," + jsgl6.ToString("0.00") + "," + jsgl7.ToString("0.00") + "," + jsgl8.ToString("0.00") + "," +
                    jsgl9.ToString("0.00") + "," + jsgl10.ToString("0.00");
                parasiticdata.kssj = kssj1.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + kssj2.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + kssj3.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + kssj4.ToString("yyyy-MM-dd HH:mm:ss.fff")+","+
                    kssj5.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + kssj6.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + kssj7.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + kssj8.ToString("yyyy-MM-dd HH:mm:ss.fff")+"," +
                    kssj9.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + kssj10.ToString("yyyy-MM-dd HH:mm:ss.fff") ;
                parasiticdata.jssj = jssj1.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + jssj2.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + jssj3.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + jssj4.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," +
                    jssj5.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + jssj6.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + jssj7.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + jssj8.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," +
                    jssj9.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + jssj10.ToString("yyyy-MM-dd HH:mm:ss.fff");
                parasiticdata.Ljhxsj = ljsxTime.ToString("0.00");
                parasiticdata.Bz = startTime.ToString("yyyy-MM-dd HH:mm:ss") + "," + DIW.ToString();
                parasiticdata.Bdjg = "合格";
            }
            Thread.Sleep(3000);         //等待滚筒停止
            Test_Flag = false;          //重置正在测试的标记
            DataTable btg_datatable = new DataTable();
            btg_datatable.Columns.Add("TimeCount");
            btg_datatable.Columns.Add("Torque");
            btg_datatable.Columns.Add("Velocity");
            btg_datatable.Columns.Add("Power");
            btg_datatable.Columns.Add("Force");
            DataRow dr = null;
            for (int i = 0; i < Torquelist.Count; i++)
            {
                dr = btg_datatable.NewRow();
                dr["TimeCount"] = i.ToString();
                dr["Torque"] = Torquelist[i].ToString("0.00");
                dr["Velocity"] = Velocitylist[i].ToString("0.00");
                dr["Power"] = Powerlist[i].ToString("0.00");
                dr["Force"] = Forcelist[i].ToString("0.00");
                btg_datatable.Rows.Add(dr);
            }

            csvwriter.SaveCSV(btg_datatable, "C:/jcdatatxt/" + "parasiticdata.csv");
            testTime++;
        }




        private void button_jsgqx_Click(object sender, EventArgs e)
        {
            if (chart_jsg.Visible)
            {
                chart_jsg.Visible = false;
                button_jsgqx.Text = "寄生功曲线";
            }
            else
            {
                StringBuilder temp = new StringBuilder();
                temp.Length = 2048;
                foreach (System.Windows.Forms.DataVisualization.Charting.Series series in chart_jsg.Series)        //清空
                {
                    series.Points.Clear();
                }

                System.Windows.Forms.DataVisualization.Charting.DataPoint jsg_point = null;
                if (configdata.TestStandard == "HJT290" || configdata.TestStandard == "HJT291" || configdata.TestStandard == "JJF1221点燃式")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        ini.INIIO.GetPrivateProfileString("寄生功率(日常测试)", (24 + 8 * i).ToString() + "Km/h", "Empty", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                        if (temp.ToString() == "Empty")
                        {
                            MessageBox.Show("没有相关数据", "出错啦");
                            return;
                        }
                        jsg_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(24 + 8 * i, temp.ToString());
                        chart_jsg.Series["xsgl_qx"].Points.Add(jsg_point);
                    }
                }
                else
                {
                    for (int i = 0; i < 10; i++)
                    {
                        ini.INIIO.GetPrivateProfileString("寄生功率(核准)", (16 + 8 * i).ToString() + "Km/h", "Empty", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                        if (temp.ToString() == "Empty")
                        {
                            MessageBox.Show("没有相关数据", "出错啦");
                            return;
                        }
                        jsg_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(16 + 8 * i, temp.ToString());
                        chart_jsg.Series["xsgl_qx"].Points.Add(jsg_point);
                    }
                }
                chart_jsg.Visible = true;
                button_jsgqx.Text = "隐藏曲线";
            }
        }
        #endregion




        #region 控制


        private void button_ldcs_Click(object sender, EventArgs e)
        {
            Motor_Close();
            Exit_Control();
            if (Test_Flag)
            {
                try
                {
                    try
                    {
                        th_jbglcs.Abort();
                    }
                    catch (Exception)
                    {
                    }
                    Exit_Control();
                }
                catch (Exception)
                {
                }
                try
                {
                    th_jsgcs.Abort();
                    Exit_Control();
                }
                catch (Exception)
                {
                }
                try
                {
                    th_jzhx.Abort();
                    Exit_Control();
                }
                catch (Exception)
                {
                }
                try
                {
                    th_gl.Abort();
                    Exit_Control();
                }
                catch (Exception)
                {
                }
                try
                {
                    th_bzhjz.Abort();
                    Exit_Control();
                }
                catch (Exception)
                {
                }
                try
                {
                    th_xysj.Abort();
                    Exit_Control();
                }
                catch (Exception)
                {
                }
                try
                {
                    th_jzwc.Abort();
                    Exit_Control();
                }
                catch (Exception)
                {
                }
                try
                {
                    th_dglmn.Abort();
                    Exit_Control();
                }
                catch (Exception)
                {
                }
                Test_Flag = false;
            }
        }

        private void button_cgjql_Click(object sender, EventArgs e)
        {
            try
            {
                Force_Zeroing();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString(), "系统提示");
            }
        }

        private void button_jsss_Click(object sender, EventArgs e)
        {
            Lifter_Up();
        }

        private void button_jsxj_Click(object sender, EventArgs e)
        {
            Lifter_Down();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            Thread stopthread = new Thread(stopProcess);
            stopthread.Start();
        }
        private void stopProcess()
        {

            Motor_Close();
            Thread.Sleep(500);
            if (Speed > 1f)
            {
                Set_Duty((float)(configdata.BrakePWM * 1.0 / 100.0));
                Start_Control_Duty();
                while (Speed > 0.5f) Thread.Sleep(200);
                Exit_Control();
            }
        }
        #region 启动恒IGBT控制
        /// <summary>
        /// 启动恒速控制
        /// </summary>
        public void Start_Control_Duty()
        {
            byte[] Cmd = null;
            List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Control_Start_Duty).ToList();
            temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
            Cmd = temp_cmd.ToArray();
            SendData(Cmd);
            Status = "Duty";           //设置状态为恒速控制
        }
        #endregion

        #region 设置恒IGBT值
        /// <summary>
        /// 设置恒速值
        /// </summary>
        /// <param name="Speed">float 速度</param>
        public void Set_Duty(float duty)
        {
            byte[] Cmd = null;
            List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Duty).ToList();
            temp_cmd.AddRange(BitConverter.GetBytes(duty));
            Cmd = temp_cmd.ToArray();
            SendData(Cmd);
        }
        #endregion

        private void button_qddj_Click(object sender, EventArgs e)
        {
            Motor_Open(70 * bpqXs);
        }

        private void button_gbdj_Click(object sender, EventArgs e)
        {
            Motor_Close();
        }

        private void button_tc_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 截图
        private void button_jqpm_Click(object sender, EventArgs e)
        {
            Image img = Get_Image();
            if (img != null)
            {
                SaveFileDialog save1 = new SaveFileDialog();
                save1.Title = "文件保存在";
                save1.InitialDirectory = "%USERPROFILE%\\My Documents\\";
                save1.Filter = "JPG(*.JPG)|*.JPG|JPEG(*.JPEG)|*.JPEG|BMP(*.BMP)|*.BMP";
                //save1.RestoreDirectory = true;
                save1.FileName = "未命名";
                FileStream writer = null;
                if (save1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        byte[] buff = null;
                        MemoryStream Ms = new MemoryStream();
                        //将图像打入流
                        img.Save(Ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        writer = new FileStream(save1.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                        buff = Ms.GetBuffer();
                        writer.Write(buff, 0, buff.Length);
                        writer.Close();
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show(er.ToString(), "出错啦");
                    }
                }
            }
            else
                MessageBox.Show("获取图片失败", "出错啦");
        }

        /// <summary>
        /// 截图
        /// </summary>
        /// <returns>Image</returns>
        public Image Get_Image()
        {
            //创建一个跟屏幕大小一样的Image
            //Image img = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);
            Image img = new Bitmap(sc_width, sc_height);
            //创建GDI+ 用来DRAW屏幕
            Graphics g = Graphics.FromImage(img);
            //创建 图形大小
            Size s = new System.Drawing.Size(sc_width, sc_height);
            //将屏幕打入到Image中
            //g.CopyFromScreen(new Point(0, 0), new Point(0, 0), Screen.AllScreens[0].Bounds.Size);
            g.CopyFromScreen(Location, new Point(this.Left, sc_height - this.Bottom), s);
            ////得到屏幕HDC句柄
            //IntPtr HDC = g.GetHdc();
            ////截图后释放该句柄
            //g.ReleaseHdc(HDC);
            return img;
        }
        #endregion
        #endregion


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                StreamReader sr = File.OpenText(File_Name8);
                s = sr.ReadToEnd();
                ShowText showtext = new ShowText(s);
                if (showtext.ShowDialog() == DialogResult.OK)
                {
                    s = showtext.RichTextValue;
                }
                sr.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("文档不存在或者读取地址不对，请检查", "系统提示");
            }
        }


        private void button4_Click_1(object sender, EventArgs e)
        {
            //timer3.Enabled = (timer3.Enabled) ? false : true;
            Exit_Control();
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

        private void button_read_speed_dp_Click(object sender, EventArgs e)
        {

        }



        /*private void radioButton_td_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_td1.Checked)
            {
                ini.INIIO.WritePrivateProfileString("UseMK", "使用通道", "1",startUpPath+"/detectConfig.ini");
                 Select_Channel(1);
            }
            else if (radioButton_td2.Checked)
            {
                ini.INIIO.WritePrivateProfileString("UseMK", "使用通道", "2",startUpPath+"/detectConfig.ini");
                 Select_Channel(2);
            }
            else if (radioButton_td3.Checked)
            {
                ini.INIIO.WritePrivateProfileString("UseMK", "使用通道", "3",startUpPath+"/detectConfig.ini");
                 Select_Channel(3);
            }
        }*/

        private void toolStripButtonForceClear_Click(object sender, EventArgs e)
        {
            try
            {
                Force_Zeroing();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString(), "系统提示");
            }
        }

        private void toolStripButtonLiftUp_Click(object sender, EventArgs e)
        {
            Lifter_Up();
        }

        private void toolStripButtonLiftDown_Click(object sender, EventArgs e)
        {
            Lifter_Down();
        }

        private void toolStripButtonMotorOn_Click(object sender, EventArgs e)
        {
            Motor_Open(70);
        }

        private void toolStripButtonMotorOff_Click(object sender, EventArgs e)
        {
            Motor_Close();
        }

        private void toolStripButtonStopTest_Click(object sender, EventArgs e)
        {
            Motor_Close();
            Exit_Control();
            isSaveStartTime = false;
            nowState = testState.idle_state;
            if (Test_Flag)
            {
                try
                {
                    try
                    {
                        th_jbglcs.Abort();
                    }
                    catch (Exception)
                    {
                    }
                    Exit_Control();
                }
                catch (Exception)
                {
                }
                try
                {
                    th_jsgcs.Abort();
                    Exit_Control();
                }
                catch (Exception)
                {
                }
                try
                {
                    th_jzhx.Abort();
                    Exit_Control();
                }
                catch (Exception)
                {
                }
                try
                {
                    th_gl.Abort();
                    Exit_Control();
                }
                catch (Exception)
                {
                }
                try
                {
                    th_bzhjz.Abort();
                    Exit_Control();
                }
                catch (Exception)
                {
                }
                try
                {
                    th_xysj.Abort();
                    Exit_Control();
                }
                catch (Exception)
                {
                }
                try
                {
                    th_jzwc.Abort();
                    Exit_Control();
                }
                catch (Exception)
                {
                }
                try
                {
                    th_dglmn.Abort();
                    Exit_Control();
                }
                catch (Exception)
                {
                }
                Test_Flag = false;
            }
        }

        private void toolStripButtonPrintScreen_Click(object sender, EventArgs e)
        {
            Image img = Get_Image();
            if (img != null)
            {
                SaveFileDialog save1 = new SaveFileDialog();
                save1.Title = "文件保存在";
                save1.InitialDirectory = "%USERPROFILE%\\My Documents\\";
                save1.Filter = "JPG(*.JPG)|*.JPG|JPEG(*.JPEG)|*.JPEG|BMP(*.BMP)|*.BMP";
                //save1.RestoreDirectory = true;
                save1.FileName = "未命名";
                FileStream writer = null;
                if (save1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        byte[] buff = null;
                        MemoryStream Ms = new MemoryStream();
                        //将图像打入流
                        img.Save(Ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        writer = new FileStream(save1.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                        buff = Ms.GetBuffer();
                        writer.Write(buff, 0, buff.Length);
                        writer.Close();
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show(er.ToString(), "出错啦");
                    }
                }
            }
            else
                MessageBox.Show("获取图片失败", "出错啦");
        }

        private void toolStripButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();//关闭面板
        }

        #region 关闭串口
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns>bool</returns>
        public bool Close_Com()
        {
            bool temp = false;
            try
            {
                ComPort_2.Close();
                temp = true;
            }
            catch (Exception er)
            {
                throw er;
            }
            return temp;
        }
        #endregion

        #region 发送数据
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="Content">内容</param>
        public void SendData(byte[] Content)
        {
            try
            {
                byte[] Cmd_Temp = Content;
                int Sum = 0;
                foreach (byte b in Cmd_Temp)
                    Sum += b;
                Sum = Sum % 256;
                byte[] Cmd = new byte[Cmd_Temp.Length + 2];
                for (int i = 0; i < Cmd_Temp.Length; i++)
                    Cmd[i] = Cmd_Temp[i];
                Cmd[Cmd_Temp.Length] = Convert.ToByte(Sum);
                switch (UseMK.ToUpper())
                {
                    case "IGBT":
                        Cmd[Cmd_Temp.Length + 1] = 0x0D;
                        break;
                    case "BNTD":
                        Cmd[Cmd_Temp.Length + 1] = 0x43;
                        break;
                    default:
                        Cmd[Cmd_Temp.Length + 1] = 0x43;
                        break;
                }
                Send_Buffer = Cmd;
                //string sss = Encoding.Default.GetString(Send_Buffer);
                ComPort_2.Write(Send_Buffer, 0, Send_Buffer.Length);
            }
            catch (Exception)
            {
                //throw;
            }
        }
        #endregion



        #region 获取下位机返回到消息
        /// <summary>
        /// 获取下位机返回到消息
        /// </summary>
        /// <returns>string 消息 没消息的时候返回空字符串</returns>
        public string Get_Message()
        {
            string message = "";
            //message =  Read_Buffer
            return message;
        }
        #endregion

        #region 退出所有控制
        /// <summary>
        /// 退出所有控制
        /// </summary>
        public void Exit_Control()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    Cmd = Encoding.Default.GetBytes(Cmd_Stop_Control);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Control_Stop_Quit).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            Status = "Time";        //设置为默认 实时
            SendData(Cmd);
        }
        #endregion

        #region 设置恒速值
        /// <summary>
        /// 设置恒速值
        /// </summary>
        /// <param name="Speed">float 速度</param>
        public void Set_Speed(float Speed)
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    // *CS + 0070.0 + 校验码 ＋ CR
                    string temp_speed = Speed.ToString();
                    string speed = "";
                    if (temp_speed.IndexOf(".") == -1)
                        temp_speed += ".0";
                    for (int i = 0; i < 6 - temp_speed.Length; i++)
                        speed += "0";
                    speed += temp_speed;
                    Cmd = Encoding.Default.GetBytes(Cmd_Set_Constant_Speed + speed);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Speed).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(Speed));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion

        #region 启动恒速控制
        /// <summary>
        /// 启动恒速控制
        /// </summary>
        public void Start_Control_Speed()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    Cmd = Encoding.Default.GetBytes(Cmd_Start_Speed);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Control_Start_Speed).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
            Status = "Speed";           //设置状态为恒速控制
        }
        #endregion

        #region 设置恒功率值
        /// <summary>
        /// 设置恒功率值
        /// </summary>
        /// <param name="Power">float 功率</param>
        public void Set_Control_Power(float Power)
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    //*CP + 0005.0 + 校验码 ＋ CR
                    string temp_power = Power.ToString();
                    string power = "";
                    if (temp_power.IndexOf(".") == -1)
                        temp_power += ".0";
                    for (int i = 0; i < 6 - temp_power.Length; i++)
                        power += "0";
                    power += temp_power;
                    Cmd = Encoding.Default.GetBytes(Cmd_Set_Power + power);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Power).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(Power));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion

        #region 启动功率控制
        /// <summary>
        /// 启动功率控制
        /// </summary>
        public void Start_Control_Power()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    Cmd = Encoding.Default.GetBytes(Cmd_Start_Power);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Control_Start_Power).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
            Status = "Power";       //设置状态为恒功率
        }
        #endregion

        #region 设置恒扭矩值
        /// <summary>
        /// 设置恒扭矩值
        /// </summary>
        /// <param name="Force">int 扭矩值</param>
        public void Set_Control_Force(float Force)
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    //*CF + 01000 + 校验码 ＋ CR
                    string temp_force = Force.ToString();
                    string force = "";
                    for (int i = 0; i < 5 - temp_force.Length; i++)
                        force += "0";
                    force += temp_force;
                    Cmd = Encoding.Default.GetBytes(Cmd_Set_Constant_Force + force);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Force).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(Force));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion

        #region 启动扭矩控制
        /// <summary>
        /// 启动扭矩控制
        /// </summary>
        public void Start_Control_Force()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    Cmd = Encoding.Default.GetBytes(Cmd_Start_Force);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Control_Start_Force).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
            Status = "Force";           //设置状态为恒扭矩控制
        }
        #endregion

        #region 进入扭矩标定状态
        /// <summary>
        /// 进入扭矩标定状态
        /// </summary>
        public void Start_Demarcate_Force()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    Cmd = Encoding.Default.GetBytes(Cmd_Demarcate);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Demarcate).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
            Status = "Demarcate";           //设置标定状态
        }
        #endregion

        #region 标定扭矩
        /// <summary>
        /// 标定扭矩
        /// </summary>
        /// <param name="Point">第几点</param>
        /// <param name="Force">扭矩力（N）</param>
        public void Demarcate_Force(int Point, int Force)
        {
            string temp_force = Force.ToString();
            string force = "";
            for (int i = 0; i < 5 - temp_force.Length; i++)
                force += "0";
            force += temp_force;
            byte[] Cmd = Encoding.Default.GetBytes(Cmd_Demarcate_Force + Point.ToString() + force);
            SendData(Cmd);
        }
        #endregion

        #region 退出扭矩标定状态
        /// <summary>
        /// 退出扭矩标定状态
        /// </summary>
        public void Exit_Demarcate_Force()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    Cmd = Encoding.Default.GetBytes(Cmd_Demarcate_Exit);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Demarcate_Exit).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
            Status = "null";           //设置标定状态
        }
        #endregion

        #region 扭矩清零
        /// <summary>
        /// 扭矩清零
        /// </summary>
        public void Force_Zeroing()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    Cmd = Encoding.Default.GetBytes(Cmd_Clear_Force);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Clear_Force).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion

        #region 里程清零
        /// <summary>
        /// 里程清零
        /// </summary>
        public void Range_Zeroing()
        {
            byte[] Cmd = Encoding.Default.GetBytes(Cmd_Clear_Mileage);
            SendData(Cmd);
        }
        #endregion

        #region 标定速度
        /// <summary>
        /// 标定速度(不用进入标定状态，在控制空闲时就可以调用标定)
        /// </summary>
        /// <param name="Diameter">string 滚筒直径</param>
        /// <param name="Pulse">string 脉冲数</param>
        public void Demarcate_Speed(string Diameter, string Pulse)
        {
            try
            {
                byte[] Cmd = null;
                switch (UseMK)
                {
                    case "IGBT":
                        string tempstr = Diameter.ToString();
                        string diameter = "";
                        for (int i = 0; i < 5 - tempstr.Length; i++)
                            diameter += "0";
                        diameter += tempstr;
                        tempstr = Pulse.ToString();
                        string pulse = "";
                        for (int i = 0; i < 5 - tempstr.Length; i++)
                            pulse += "0";
                        pulse += tempstr;
                        Cmd = Encoding.Default.GetBytes(Cmd_Demarcate_Speed + diameter + pulse);
                        SendData(Cmd);
                        break;
                    case "BNTD":
                        List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Diameter).ToList();
                        temp_cmd.AddRange(BitConverter.GetBytes(float.Parse(Diameter)));
                        Cmd = temp_cmd.ToArray();
                        SendData(Cmd);
                        Thread.Sleep(10);
                        temp_cmd = null;
                        temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Pulse).ToList();
                        temp_cmd.AddRange(BitConverter.GetBytes(float.Parse(Pulse)));
                        Cmd = temp_cmd.ToArray();
                        SendData(Cmd);
                        Thread.Sleep(10);
                        temp_cmd = null;
                        temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Solidify_Speed_Modulus).ToList();
                        temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                        Cmd = temp_cmd.ToArray();
                        SendData(Cmd);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
            //*I  + 00216 + 00600 ＋ 校验码 +  CR 

        }
        #endregion

        #region 举升上升
        /// <summary>
        /// 举升上升，当速度不为零时等待速度为零上升
        /// </summary>
        public void Lifter_Up()
        {
            byte[] Cmd = null;

            List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Lifting_Up).ToList();
            temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
            //while (float.Parse(Speed_string) > 1.0)                      //如果速度大于1是不能升起举升的
            //Thread.Sleep(10);
            Cmd = temp_cmd.ToArray();
            SendData(Cmd);
        }


        #endregion

        #region 举升下降
        /// <summary>
        /// 举升下降
        /// </summary>
        public void Lifter_Down()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    Cmd = Encoding.Default.GetBytes(Cmd_Lifting_Down);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Lifting_Down).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion

        #region 启动电机
        /// <summary>
        /// 启动电机
        /// </summary>
        public void Motor_Open(double bqpSpeed_set)
        {
            if (bpqMethod != "串口")
            {
                byte[] Cmd = null;
                switch (UseMK)
                {
                    case "IGBT":
                        Cmd = Encoding.Default.GetBytes(Cmd_Reverse_Motor);
                        break;
                    case "BNTD":
                        List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Control_Motor_Start).ToList();
                        temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                        Cmd = temp_cmd.ToArray();
                        break;
                    default:
                        break;
                }
                SendData(Cmd);
            }
            else
            {

                bpq.setMotorFre(bqpSpeed_set);
                Thread.Sleep(20);
                bpq.turnOnMotor();
                Thread.Sleep(10);
                TurnOnRelay((byte)configdata.BpqDy);

            }
        }
        #endregion

        #region 关闭电机
        /// <summary>
        /// 关闭电机
        /// </summary>
        public void Motor_Close()
        {
            if (bpqMethod != "串口")
            {
                byte[] Cmd = null;
                switch (UseMK)
                {
                    case "IGBT":
                        Cmd = Encoding.Default.GetBytes(Cmd_Reverse_Motor_Stop);
                        break;
                    case "BNTD":
                        List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Control_Motor_Stop).ToList();
                        temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                        Cmd = temp_cmd.ToArray();
                        break;
                    default:
                        break;
                }
                SendData(Cmd);
            }
            else
            {
                TurnOffRelay((byte)configdata.BpqDy);
                Thread.Sleep(10);
                bpq.turnOffMotor();
            }
        }
        #endregion

        #region 读取环境参数 BNDT用
        /// <summary>
        /// 读取环境参数
        /// </summary>
        /// <returns>float[] 下标0为温度、1为湿度、2为大气压 如果3个数都是0则未成功</returns>
        public void Get_Environment()
        {
            //float[] Environment = new float[]{ 0,0,0};
            //try
            //{
            List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Read_Environment).ToList();
            temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
            byte[] Cmd = temp_cmd.ToArray();
            SendData(Cmd);
            Thread.Sleep(500);                              //发送完取环境参数命令后等待500ms 以便环境参数更新
            //Environment[0] = float.Parse();      //温度
            //Environment[1] = float.Parse(Humidity);         //湿度
            //Environment[2] = float.Parse(ATM);              //大气压
            //return Environment;
            //}
            //catch (Exception)
            //{
            //    return Environment;
            //}
        }
        #endregion

        #region 读取力通道的系数 BNDT用
        /// <summary>
        /// 读取力通道的系数 BNDT用 是读取所有力通道的系数，取完之后保存在IGBT类中b0、c0……
        /// </summary>
        public void Get_Force_Modulus()
        {
            try
            {
                List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Read_Force_Modulus).ToList();
                temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                byte[] Cmd = temp_cmd.ToArray();
                SendData(Cmd);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region 固化力通道标定系数 BNDT用
        /// <summary>
        /// 固化力通道标定系数 BNDT用
        /// </summary>
        /// <param name="Aisle">int 通道号(1/2/3)</param>
        /// <param name="Modulus">float 系数b</param>
        public void Solidify_Force_Modulus(byte Aisle, float Modulus)
        {
            try
            {
                List<byte> temp_cmd = null;
                byte[] Cmd = null;
                if (Aisle < 0 && Aisle > 3)
                    return;
                //设置参数
                switch (Aisle)
                {
                    case 1:
                        temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Force_b0).ToList();
                        break;
                    case 2:
                        temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Force_b1).ToList();
                        break;
                    case 3:
                        temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Force_b2).ToList();
                        break;
                }
                temp_cmd.AddRange(BitConverter.GetBytes(Modulus));
                Cmd = temp_cmd.ToArray();
                SendData(Cmd);
                Thread.Sleep(10);
                temp_cmd = null;
                //固化参数
                temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Solidify_Force_Modulus).ToList();
                temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                Cmd = temp_cmd.ToArray();
                SendData(Cmd);
                Thread.Sleep(10);
                //选择使用固化了系数的通道
                Select_Channel(Aisle);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region 选择使用的力通道(1/2/3) BNDT用
        /// <summary>
        /// 选择使用的力通道(1/2/3) BNDT用
        /// </summary>
        /// <param name="Aisle">int 通道号(1/2/3)</param>
        public void Select_Channel(byte Aisle)
        {
            try
            {
                List<byte> temp_cmd = null;
                byte[] chanel_cmd = { 0, 0, 0, Aisle };
                byte[] Cmd = null;
                if (Aisle < 0 && Aisle > 3)
                    return;
                temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Force_Aisle).ToList();
                temp_cmd.AddRange(chanel_cmd.ToList());
                Cmd = temp_cmd.ToArray();
                SendData(Cmd);
            }
            catch (Exception)
            {
            }
        }
        #endregion
        #region 打开继电器(1/2/3) BNDT用
        /// <summary>
        /// 选择使用的力通道(1/2/3) BNDT用
        /// </summary>
        /// <param name="Aisle">int 通道号(1/2/3)</param>
        public void TurnOnRelay(byte RelaySelected)
        {
            try
            {
                List<byte> temp_cmd = null;
                byte[] chanel_cmd = { 0, 0, 0, RelaySelected };
                byte[] Cmd = null;
                if (RelaySelected < 0 && RelaySelected > 3)
                    return;
                temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Control_Relay_TurnOn).ToList();
                temp_cmd.AddRange(chanel_cmd.ToList());
                Cmd = temp_cmd.ToArray();
                SendData(Cmd);
            }
            catch (Exception)
            {
            }
        }
        #endregion
        #region 关闭继电器(1/2/3) BNDT用
        /// <summary>
        /// 选择使用的力通道(1/2/3) BNDT用
        /// </summary>
        /// <param name="Aisle">int 通道号(1/2/3)</param>
        public void TurnOffRelay(byte RelaySelected)
        {
            try
            {
                List<byte> temp_cmd = null;
                byte[] chanel_cmd = { 0, 0, 0, RelaySelected };
                byte[] Cmd = null;
                if (RelaySelected < 0 && RelaySelected > 3)
                    return;
                temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Control_Relay_TurnOff).ToList();
                temp_cmd.AddRange(chanel_cmd.ToList());
                Cmd = temp_cmd.ToArray();
                SendData(Cmd);
            }
            catch (Exception)
            {
            }
        }
        #endregion
        #region 固化力选择通道 BNDT用
        /// <summary>
        /// 固化力选择通道 BNDT用
        /// </summary>
        /// <param name="Aisle">int 通道号(1/2/3)</param>
        /// <param name="Modulus">float 系数b</param>
        public void Solidify_Force_Chanel()
        {
            try
            {
                List<byte> temp_cmd = null;
                byte[] Cmd = null;
                temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Solidify_Force_Chanel).ToList();
                temp_cmd.AddRange(BitConverter.GetBytes(0f));
                Cmd = temp_cmd.ToArray();
                SendData(Cmd);
                Thread.Sleep(10);
            }
            catch (Exception)
            {
            }
        }
        #endregion
        #region 读取力选择通道 BNDT用
        /// <summary>
        /// 读取力选择通道 BNDT用
        /// </summary>
        public void Read_Force_Chanel()
        {
            try
            {
                List<byte> temp_cmd = null;
                byte[] Cmd = null;
                temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Read_Force_Chanel).ToList();
                temp_cmd.AddRange(BitConverter.GetBytes(0f));
                Cmd = temp_cmd.ToArray();
                SendData(Cmd);
                Thread.Sleep(10);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region 读取滚筒直径和脉冲数 BNDT用
        /// <summary>
        /// 读取滚筒直径和脉冲数 BNDT用
        /// </summary>
        public void Get_Speed_DiameterandPusle()
        {
            try
            {
                List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Read_Speed_Modulus).ToList();
                temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                byte[] Cmd = temp_cmd.ToArray();
                SendData(Cmd);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region 设置恒速度PID参数
        public void Set_SpeedPid(float Kp, float Ki, float Kd)
        {
            Set_SpeedPid_Kp(Kp);
            Set_SpeedPid_Ki(Ki);
            Set_SpeedPid_Kd(Kd);
        }
        #endregion

        #region 设置恒速度PID参数Kp
        /// <summary>
        /// 设置恒扭矩值
        /// </summary>
        /// <param name="Force">float 扭矩值</param>
        public void Set_SpeedPid_Kp(float Kp)
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Duty_kp).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(Kp));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion
        #region 设置恒速度PID参数Ki
        /// <summary>
        /// 设置恒扭矩值
        /// </summary>
        /// <param name="Force">float 扭矩值</param>
        public void Set_SpeedPid_Ki(float Ki)
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Duty_ki).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(Ki));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion
        #region 设置恒速度PID参数Kd
        /// <summary>
        /// 设置恒扭矩值
        /// </summary>
        /// <param name="Force">float 扭矩值</param>
        public void Set_SpeedPid_Kd(float Kd)
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Duty_kd).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(Kd));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion
        #region 读取恒速度PID参数
        public void Read_SpeedPid()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Read_Speed_PID).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion
        #region 设置恒力PID参数
        public void Set_ForcePid(float Kp, float Ki, float Kd)
        {
            Set_ForcePid_Kp(Kp);
            Set_ForcePid_Ki(Ki);
            Set_ForcePid_Kd(Kd);
        }
        #endregion
        #region 设置恒力及恒功率PID参数Kp
        /// <summary>
        /// 设置恒扭矩值
        /// </summary>
        /// <param name="Force">float 扭矩值</param>
        public void Set_ForcePid_Kp(float Kp)
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Duty_kp_force).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(Kp));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion
        #region 设置恒力及恒功率PID参数Ki
        /// <summary>
        /// 设置恒扭矩值
        /// </summary>
        /// <param name="Force">float 扭矩值</param>
        public void Set_ForcePid_Ki(float Ki)
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Duty_ki_force).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(Ki));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion
        #region 设置恒力及恒功率PID参数Kd
        /// <summary>
        /// 设置恒扭矩值
        /// </summary>
        /// <param name="Force">float 扭矩值</param>
        public void Set_ForcePid_Kd(float Kd)
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Duty_kd_force).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(Kd));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion
        #region 读取恒力PID参数
        public void Read_ForcePid()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Read_Force_PID).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion
        private void Main_exit(object sender, FormClosingEventArgs e)
        {
            if (!isSaved)
            {
                if (MessageBox.Show("还未保存数据，确认退出吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    try
                    {
                        Exit_Control();
                    }
                    catch
                    { }
                    _continue = false;
                    Thread.Sleep(5);
                    if (readThread != null)
                        readThread.Abort();
                    if (ComPort_2 != null)
                        ComPort_2.Close();
                    if (bpq != null)
                    {
                        if (bpq.ComPort_2.IsOpen)
                            bpq.ComPort_2.Close();
                    }
                    //parasiticdata = new parasitic();
                    //parasiticdata.Sdqj = "-1";
                    //parasiticdata.Mysd = "-1";
                    //parasiticdata.Hxsj = "-1";
                    //parasiticdata.Ljhxsj = "-1";
                    //parasiticdata.Bz = "-1";
                    //parasiticdata.Bdjg = "-2";
                    //parasiticcontrol.writeParasiticIni(parasiticdata);
                }
                else
                {
                    // 点击取消不保存，并不关闭窗口
                    e.Cancel = true;
                }
            }
            else
            {
                try
                {
                    Exit_Control();
                }
                catch
                { }
                _continue = false;
                Thread.Sleep(5);
                if (readThread != null)
                    readThread.Abort();
                if (ComPort_2 != null)
                    ComPort_2.Close();
                if (bpq != null)
                {
                    if (bpq.ComPort_2.IsOpen)
                        bpq.ComPort_2.Close();
                }

            }

        }



        private void timer2_Tick(object sender, EventArgs e)
        {
            if (isSaveStartTime)//如果正在测试
            {
                nowtime = DateTime.Now;
                if ((int)(gongkuangTime * 10) != GKSJ)//100ms一次
                {
                    Velocitylist.Add( Speed);
                    Torquelist.Add(Force);
                    Powerlist.Add(Power);
                    Forcelist.Add(Force);
                    GKSJ++;//工况时间加1
                }
                TimeSpan timespan = nowtime - startTime;
                gongkuangTime = (float)timespan.TotalMilliseconds / 1000f;
            }
            else
            {
                GKSJ = 0;
                perGKSJ = 0;
                gongkuangTime = 0f;
            }
        }

        private void buttonJsglPreResult_Click(object sender, EventArgs e)
        {
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            foreach (System.Windows.Forms.DataVisualization.Charting.Series series in chart_jsg.Series)        //清空
            {
                series.Points.Clear();
            }

            System.Windows.Forms.DataVisualization.Charting.DataPoint jsg_point = null;
            if (configdata.TestStandard == "HJT290" || configdata.TestStandard == "HJT291"||configdata.TestStandard== "JJF1221点燃式")
            {
                double[] jsglqx_power = new double[4];
                double[] jsglqx_speed = new double[4];
                for (int i = 0; i < 4; i++)
                {
                    jsglqx_speed[i] = (24 + 8 * i);
                    ini.INIIO.GetPrivateProfileString("寄生功率(日常测试)", (24 + 8 * i).ToString() + "Km/h", "Empty", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                    if (temp.ToString() == "Empty")
                    {
                        MessageBox.Show("没有相关数据", "出错啦");
                        return;
                    }
                    try
                    {
                        jsglqx_power[i] = double.Parse(temp.ToString());
                    }
                    catch
                    {
                        MessageBox.Show("读取数据异常:" + temp.ToString(), "出错啦");
                        return;
                    }
                    if (jsglqx_power[i] > 100)
                    {
                        MessageBox.Show("读取数据异常:" + temp.ToString(), "出错啦");
                        return;
                    }
                    dataGrid_jsgl.Rows[3 - i].Cells["寄生功率（Kw）"].Value = jsglqx_power[i].ToString("0.00");
                    jsg_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(24 + 8 * i, temp.ToString());
                    chart_jsg.Series["xsgl_qx"].Points.Add(jsg_point);

                }
                jsglqx_matrix = new MatrixEquation.MatrixEquation(jsglqx_speed, jsglqx_power, 2);
                double[] jsglqx_matrixResult = jsglqx_matrix.GetResult();
                jsglqx_c = (float)jsglqx_matrixResult[0];
                jsglqx_b = (float)jsglqx_matrixResult[1];
                jsglqx_a = (float)jsglqx_matrixResult[2];
            }
            else
            {
                double[] jsglqx_power = new double[10];
                double[] jsglqx_speed = new double[10];
                for (int i = 0; i < 10; i++)
                {
                    jsglqx_speed[i] = 16 + 8 * i;
                    ini.INIIO.GetPrivateProfileString("寄生功率(核准)", (16 + 8 * i).ToString() + "Km/h", "Empty", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                    if (temp.ToString() == "Empty")
                    {
                        MessageBox.Show("没有相关数据", "出错啦");
                        return;
                    }
                    try
                    {
                        jsglqx_power[i] = double.Parse(temp.ToString());
                    }
                    catch
                    {
                        MessageBox.Show("读取数据异常:" + temp.ToString(), "出错啦");
                        return;
                    }
                    if (jsglqx_power[i] > 100)
                    {
                        MessageBox.Show("读取数据异常:" + temp.ToString(), "出错啦");
                        return;
                    }
                    dataGrid_jsgl.Rows[9 - i].Cells["寄生功率（Kw）"].Value = jsglqx_power[i].ToString("0.00");
                    jsg_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(16 + 8 * i, temp.ToString());
                    chart_jsg.Series["xsgl_qx"].Points.Add(jsg_point);

                }
                jsglqx_matrix = new MatrixEquation.MatrixEquation(jsglqx_speed, jsglqx_power, 2);
                double[] jsglqx_matrixResult = jsglqx_matrix.GetResult();
                jsglqx_c = (float)jsglqx_matrixResult[0];
                jsglqx_b = (float)jsglqx_matrixResult[1];
                jsglqx_a = (float)jsglqx_matrixResult[2];
            }
            textBox_Jsglqx_a.Text = jsglqx_a.ToString("0.0000");
            textBox_Jsglqx_b.Text = jsglqx_b.ToString("0.0000");
            textBox_Jsglqx_c.Text = jsglqx_c.ToString("0.0000");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (parasiticdata != null)
            {
                parasiticdata.Starttime = startTime.ToString("yyyy-MM-dd HH:mm:ss");
                parasiticcontrol.writeParasiticIni(parasiticdata);
                isSaved = true;
                MessageBox.Show("保存成功");
            }
            else
            {
                MessageBox.Show("没有有效数据用于保存,请先完成试验");
            }
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}

