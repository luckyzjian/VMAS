using System;
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

namespace 惯量标定
{
    public partial class Main : Form
    {
        MatchEvaluator me = delegate(Match m)
        {
            return "\n";
        };
        configInfdata configinfdata = new configInfdata();
        configIni configini = new configIni();
        BpqFl.bpxcontrol bpx = new BpqFl.bpxcontrol();
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

        double jsgl1 = 0, jsgl2 = 0, jsgl3 = 0, jsgl4 = 0, jsgl5 = 0, jsgl6 = 0, jsgl7 = 0, jsgl8 = 0, jsgl9 = 0, jsgl10 = 0;
        float speed_xishu = 1f;

        public string comportBpx;
        public string comportBpxPz;

        private string comportIgbt = "";
        private string comportIgbtPz = "";
        //说明文档
        public string File_Name1 = Application.StartupPath+ @"\Rem\寄生功率滑行测试.txt";
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
            idle_state = 1, demarcate_state, hengding_state, jsgl_state
            ,jzhx_state,gl_state,bzhx_state,temp_state,jzwc_state };
        private testState nowState = testState.idle_state;
        private testState preState = testState.idle_state;

        private bool timer_flag=false;//在惯量相关测试中用以标识速度已达最高点
        private bool testFinish = false;//用以判断惯量相关测试是否结束
        private double DIW1 = 0, DIW2 = 0, DIW3 = 0;
        private double t1 = 0, t2 = 0, t3 = 0, t4 = 0, t5 = 0, t6 = 0, t7 = 0, t8 = 0, t9 = 0, t10 = 0;
        private double t11 = 0, t12 = 0, t13 = 0, t14 = 0, t15 = 0, t16 = 0, t17 = 0, t18 = 0, t19 = 0, t20 = 0;
        private double t21 = 0, t22 = 0, t23 = 0, t24 = 0, t25 = 0, t26 = 0, t27 = 0, t28 = 0, t29 = 0, t30 = 0;
        private double t31 = 0, t32 = 0, t33 = 0, t34 = 0, t35 = 0, t36 = 0, t37 = 0, t38 = 0, t39 = 0;
        private double t40 = 0, t41 = 0, t42 = 0, t43 = 0, t44 = 0;
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
        

        public void Init()
        {
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            try
            {
                HJT290.Checked = true;
                radioButton_daily.Checked = true;
                string[] Com_Items = GetCommKeys();
                ini.INIIO.GetPrivateProfileString("DIW", "DIWLX", "轻型车", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                if (temp.ToString() == "轻型车")
                    radioButtonQx.Checked = true;
                else
                    radioButtonZx.Checked = true;

                ini.INIIO.GetPrivateProfileString("DIW", "DIW", "907.2", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                textBox_zdgl.Text = temp.ToString();
                DIW = float.Parse(temp.ToString());

                ini.INIIO.GetPrivateProfileString("PID", "KP_SPEED_DEFAULT", "0", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                kp_float_default = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KI_SPEED_DEFAULT", "0", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                ki_float_default = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KD_SPEED_DEFAULT", "0", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                kd_float_default = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KP_FORCE_DEFAULT", "0", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                kp_force_default = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KI_FORCE_DEFAULT", "0", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                ki_force_default = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KD_FORCE_DEFAULT", "0", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                kd_force_default = float.Parse(temp.ToString());

                ini.INIIO.GetPrivateProfileString("PID", "KP_SPEED_ZX", "0", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                kp_float_zx = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KI_SPEED_ZX", "0", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                ki_float_zx = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KD_SPEED_ZX", "0", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                kd_float_zx = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KP_FORCE_ZX", "0", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                kp_force_zx = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KI_FORCE_ZX", "0", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                ki_force_zx = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("PID", "KD_FORCE_ZX", "0", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                kd_force_zx = float.Parse(temp.ToString());

                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t1", "6.80", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t1_moni = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t2", "3.29", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t2_moni = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t3", "6.79", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t3_moni = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t4", "3.29", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t4_moni = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t5", "6.80", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t5_moni = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t6", "3.30", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t6_moni = float.Parse(temp.ToString());

                ini.INIIO.GetPrivateProfileString("DIW_MONI", "speed_xishu", "1.0", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                speed_xishu = float.Parse(temp.ToString());

                ini.INIIO.GetPrivateProfileString("DIW_MONI", "useMoni", "true", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                if (temp.ToString() == "true")
                    useMoni = true;
                else
                    useMoni = false;


                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t1_bhjz", "25.33", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t1_bhjz = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t2_bhjz", "15.47", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t2_bhjz = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t3_bhjz", "3.96", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t3_bhjz = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "useMoni_bhjz", "false", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                if (temp.ToString() == "true")
                    useMoni_bhjz = true;
                else
                    useMoni_bhjz = false;
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "real_force", "5300", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                real_force = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "gl_force", "5300", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                gl_force = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "use_gl_force", "5300", temp, 2048, @".\Config.ini"); 
                if (temp.ToString() == "true")
                    use_gl_force = true;
                else
                    use_gl_force = false;
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "useMethod", "48~16", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                if (temp.ToString() == "48~32")
                {
                    useMethod = "48~32";
                    textBox1.Text = "48";
                    textBox2.Text = "32";
                    gl_power_string = "6KW滑行时间(s)";
                    gl_power_set = 6f;
                }
                else
                {
                    useMethod = "48~16";
                    textBox1.Text = "48";
                    textBox2.Text = "16";
                    gl_power_string = "0KW滑行时间(s)";
                    gl_power_set = 0f;
                }
                ini.INIIO.GetPrivateProfileString("Tscs", "Tscs", "7", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                Tscs = int.Parse(temp.ToString());

                ini.INIIO.GetPrivateProfileString("Com", "ComportBp", "COM2", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                comportBpx = temp.ToString();
                ini.INIIO.GetPrivateProfileString("Com", "ComBpstr", "9600,n,8,1", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                comportBpxPz = temp.ToString();

                ini.INIIO.GetPrivateProfileString("UseMK", "MK", "BNTD", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                UseMK = temp.ToString();
                switch (UseMK)
                {
                    case "IGBT":
                        break;
                    case "BNTD":
                        label1.Text = "占空";
                        break;
                    default:
                        break;
                }
                try
                {
                    Init_Comm(configinfdata.Cgjck, "38400,n,8,1");                           //初始化串口
                }
                catch
                {
                    MessageBox.Show("底盘测试机串口打开失败.","系统提示");
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
                            if ((temp_start2 < temp_start1) &&( temp_start2 != -1))
                            {
                                start = temp_start2;
                                msg_back = true;
                                end = All_Content_byte.IndexOf(0x43);//如果是调试信息，则以C结尾为结尾标志
                            }
                            else if (temp_start1!=-1)
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
                                    Speed = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    Force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "BP":
                                    Speed = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    Force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "BF":
                                    Speed = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    Force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "BI":
                                    Speed = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    Force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "II":
                                    byte[] ds = new byte[] { 0xff, 0xff, 0xff, 0xff };
                                    float sdd = BitConverter.ToSingle(ds, 0);
                                    Speed = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    Force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
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
                                    forceChanelByte = All_Content_byte.ToArray().ElementAt(start+14);
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
                            
                            speed_list.Add(Speed);
                            force_list.Add(Force);
                            if (speed_list.Count > 40)
                                speed_list.RemoveAt(0);
                            if (force_list.Count > 40)
                                force_list.RemoveAt(0);
                            speed_sum = speed_list.Sum();
                            force_sum = force_list.Sum();
                            if (force_list.Count >=40)
                            {
                                caculate_count++;
                                if (caculate_count == 10)
                                {
                                    Speed_string = (speed_sum / 40f * speed_xishu).ToString("0.0");
                                    Force_string = (force_sum/40f).ToString("0");
                                    Msg(Msg_cs, panel_cs, Speed_string, false);
                                    if (th_speedDermarcate)
                                        Ref_textbox(textBoxRealSpeed, Speed_string);//如果在速度标定状态则在相应位置显示实测速度值
                                    if (isForceDemarcate)
                                        Msg(label_yzl, panel9, Force_string, false);//在标定力状态下显示力
                                    Speed = (float)Convert.ToDouble(Speed_string);
                                    Msg(Msg_nl, panel_nl, Force_string, false);
                                    Force = (float)Convert.ToDouble(Force_string);
                                    caculate_count = 0;
                                }
                            }
                            Msg(Msg_gl, panel_gl, Power_string, false);
                            Power = (float)Convert.ToDouble(Power_string);
                            Msg(Msg_dl, panel_cp, Duty_string, false);
                            Duty = (float)Convert.ToDouble(Duty_string);
                            Msg_toollabel(toolStripLabelMessage, panel_cs, Msg_received, false);
                            switch (nowState)
                            {
                                case testState.demarcate_state:
                                     Invoke(new MethodInvoker(delegate()
                                     {
                                         switch (comboBox_bdtd.Text)
                                         {
                                             case "1":
                                                 Msg(label_ad, panel_ad, Time_Vol_1, false);
                                                 //Msg(label_yzl, panel_yzl, (vol_1_double * b0_double).ToString("0"), false);
                                                 break;
                                             case "2":
                                                 Msg(label_ad, panel_ad, Time_Vol_2, false);
                                                 //Msg(label_yzl, panel_yzl, (vol_2_double * b1_double).ToString("0"), false);
                                                 break;
                                             case "3":
                                                 Msg(label_ad, panel_ad, Time_Vol_3, false);
                                                 //Msg(label_yzl, panel_yzl, (vol_3_double * b2_double).ToString("0"), false);
                                                 break;
                                         }
                        
                                     }));
                                     break;
                                case testState.hengding_state:

                                    break;
                                case testState.jsgl_state:
                                    if (radioButton_check.Checked == true)
                                    {
                                        if (Speed > 96)
                                        {
                                            Motor_Close();
                                            timer_flag = true;
                                        }
                                        if (timer_flag == true)
                                        {
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
                                            if (Speed < 12)
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

                                        }
                                        if (timer_flag == true)
                                        {
                                            if (Speed <= 51 && Speed >= 45)
                                            {
                                                t1 += 0.01;
                                                dataGrid_jsgl.Rows[0].Cells["滑行时间（s）"].Value = t1.ToString("0.00");
                                            }
                                            if (Speed <= 48 && Speed >= 32)
                                                {
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
                                            if (Speed < 16)
                                            {
                                                testFinish = true;
                                                nowState = testState.idle_state;
                                                Exit_Control();
                                            }

                                        }

                                    }
                                    break;
                                case testState.jzhx_state:
                                    if (radioButton_daily.Checked == true)
                                    {
                                        if (Speed > 53)
                                        {
                                            timer_flag = true;
                                            Motor_Close();
                                            Start_Control_Power();
                                        }
                                        if (timer_flag == true)
                                        {
                                            if (Speed <= 48 && Speed >= 32)
                                            {
                                                //Set_Control_Power(power);
                                                // Start_Control_Power(); 
                                                t1 += 0.01;
                                                dataGrid_jzhx.Rows[0].Cells["实测时间ACDT(s)"].Value = t1.ToString("0.00");
                                            }
                                            if (Speed < 32 && Speed >= 16)
                                            {
                                                //Set_Control_Power(power);
                                                t2 += 0.01;
                                                dataGrid_jzhx.Rows[1].Cells["实测时间ACDT(s)"].Value = t2.ToString("0.00");
                                            }
                                            if (Speed < 16)
                                            {//区间记录完成
                                                testFinish = true;
                                                Set_Control_Power(0f);                                   //设置恒速度值
                                                nowState = testState.idle_state;
                                                Exit_Control();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Speed > 53)
                                        {
                                            timer_flag = true;
                                            Motor_Close();
                                            Start_Control_Power();
                                        }
                                        if (timer_flag == true)
                                        {
                                            if (Speed <= 48 && Speed >= 32)
                                            {
                                                //Set_Control_Power(power);
                                                // Start_Control_Power(); 
                                                t3 += 0.01;
                                                dataGrid_jzhx.Rows[0].Cells["实测时间ACDT(s)"].Value = t3.ToString("0.00");
                                            }
                                            if (Speed < 32 && Speed >= 16)
                                            {
                                                //Set_Control_Power(power);
                                                t4+= 0.01;
                                                dataGrid_jzhx.Rows[1].Cells["实测时间ACDT(s)"].Value = t4.ToString("0.00");
                                            }
                                            if (Speed < 16)
                                            {//区间记录完成
                                                testFinish = true;
                                                nowState = testState.idle_state;
                                                Set_Control_Power(0f);  
                                                Exit_Control();
                                            }
                                        }
                                    }

                                    break;
                                case testState.gl_state:
                                    //t1 += 0.01;
                                    //Msg(label_time, panel_time, t1.ToString("0.00"), false);
                                    //BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString("0.0");
                                    if (glTestStep < 7)
                                    {
                                        if (Speed >= 60)
                                        {
                                            Motor_Close();
                                            if (timer_flag == false) glTestStep++;
                                            timer_flag = true;
                                            if (useMethod == "48~32")
                                                Start_Control_Power();
                                            else
                                            {
                                                if ((glTestStep - 1) % 2 == 1)
                                                    Start_Control_Power();
                                            }
                                        }
                                        if (timer_flag == true)
                                        {
                                            if (Speed >= 16.0 & Speed <= 48.0)
                                            {
                                                t1 += 0.01;
                                                if (useMoni == false)
                                                {
                                                    if ((glTestStep - 1) % 2 == 0)
                                                        dataGrid_gl.Rows[(glTestStep - 1) / 2].Cells[gl_power_string].Value = t1.ToString("0.00");
                                                    else
                                                        dataGrid_gl.Rows[(glTestStep - 1) / 2].Cells["13KW滑行时间(s)"].Value = t1.ToString("0.00");
                                                }
                                            }
                                            else if (Speed == 0)//待到滚筒速度停下来
                                            {
                                                timer_flag = false;
                                                Exit_Control();
                                                switch (glTestStep)
                                                {
                                                    case 1:
                                                        if (useMoni == true)
                                                            t2 = t1_moni;
                                                        else
                                                            t2 = t1;
                                                        Set_Control_Power(13f);
                                                        Motor_Open();
                                                        break;
                                                    case 2:
                                                        if (useMoni == true)
                                                            t3 = t2_moni;
                                                        else
                                                            t3 = t1;
                                                        Set_Control_Power(gl_power_set);
                                                        Motor_Open();
                                                        dataGrid_gl.Rows[0].Cells[gl_power_string].Value = t2.ToString("0.00");
                                                        dataGrid_gl.Rows[0].Cells["13KW滑行时间(s)"].Value = t3.ToString("0.00");
                                                        DIW1 = Math.Round(2000 * 13 * t2 * t3 / (158.025 * (t2 - t3)), 1);
                                                        dataGrid_gl.Rows[0].Cells["实测(kg)"].Value = DIW1.ToString("0.0");
                                                        //dataGrid_gl.Rows[0].Cells["误差"].Value = t3.ToString("0.00");
                                                        //dataGrid_gl.Rows[0].Cells["判定"].Value = t3.ToString("0.00");
                                                        break;
                                                    case 3:
                                                        if (useMoni == true)
                                                            t4 = t3_moni;
                                                        else
                                                            t4 = t1;
                                                        //t4 = t1;
                                                        Set_Control_Power(13f);
                                                        Motor_Open();
                                                        break;
                                                    case 4:
                                                        if (useMoni == true)
                                                            t5 = t4_moni;
                                                        else
                                                            t5 = t1;
                                                        Set_Control_Power(gl_power_set);
                                                        Motor_Open();
                                                        dataGrid_gl.Rows[1].Cells[gl_power_string].Value = t4.ToString("0.00");
                                                        dataGrid_gl.Rows[1].Cells["13KW滑行时间(s)"].Value = t5.ToString("0.00");
                                                        DIW2 = Math.Round(2000 * 13 * t4 * t5 / (158.025 * (t4 - t5)), 1);
                                                        dataGrid_gl.Rows[1].Cells["实测(kg)"].Value = DIW2.ToString("0.0");
                                                        break;
                                                    case 5:
                                                        if (useMoni == true)
                                                            t6 = t5_moni;
                                                        else
                                                            t6 = t1;
                                                        Set_Control_Power(13f);
                                                        Motor_Open();
                                                        break;
                                                    case 6:
                                                        if (useMoni == true)
                                                            t7 = t6_moni;
                                                        else
                                                            t7 = t1;
                                                        Set_Control_Power(0);
                                                        dataGrid_gl.Rows[2].Cells[gl_power_string].Value = t6.ToString("0.00");
                                                        dataGrid_gl.Rows[2].Cells["13KW滑行时间(s)"].Value = t7.ToString("0.00");
                                                        DIW3 = Math.Round(2000 * 13 * t6 * t7 / (158.025 * (t6 - t7)), 1);
                                                        dataGrid_gl.Rows[2].Cells["实测(kg)"].Value = DIW3.ToString("0.0");
                                                        //dataGrid_gl.Rows[3].Cells["实测(kg)"].Value = "平均：" + ((DIW3 + DIW1 + DIW2)).ToString("0.0");
                                                        nowState = testState.idle_state;
                                                        glTestStep = 0;
                                                        testFinish = true;
                                                        break;
                                                    default: break;
                                                }
                                                t1 = 0;
                                            }
                                        }
                                    }
                                    break;
                                case testState.bzhx_state:
                                    if (Speed > 88.5)
                                    //while (true)                //等待达到预定速度
                                    {
                                        // if (Speed > 88.5)
                                        Motor_Close();
                                        bhjz_power = 3.7f;
                                        Set_Control_Power(bhjz_power);
                                        Start_Control_Power();
                                        timer_flag = true;
                                    }
                                    if (timer_flag == true)//  Motor_Close();                         //达到速度关闭电机
                                    {
                                        if (Speed <= 80.5 && Speed >= 78.8)
                                        {
                                            if (bhjz_power != 3.7f)
                                            {
                                                bhjz_power = 3.7f;
                                                Set_Control_Power(3.7f);
                                            }
                                            t1 += 0.01;
                                        }
                                        if (Speed < 78.8 && Speed >= 77.2)
                                        {
                                            if (bhjz_power != 4.4f)
                                            {
                                                bhjz_power = 4.4f;
                                                Set_Control_Power(4.4f);
                                            }
                                            t2 += 0.01;
                                        }
                                        if (Speed < 77.2 && Speed >= 75.6)
                                        {
                                            if (bhjz_power != 5.1f)
                                            {
                                                bhjz_power = 5.1f;
                                                Set_Control_Power(5.1f);
                                            }
                                            t3 += 0.01;
                                        }
                                        if (Speed < 7.56 && Speed >= 74.0)
                                        {
                                            if (bhjz_power != 5.9f)
                                            {
                                                bhjz_power = 5.9f;
                                                Set_Control_Power(5.9f);
                                            }
                                            t4 += 0.01;
                                        }
                                        if (Speed < 74.0 && Speed >= 72.4)
                                        {
                                            if (bhjz_power != 6.6f)
                                            {
                                                bhjz_power = 6.6f;
                                                Set_Control_Power(6.6f);
                                            }
                                            t5 += 0.01;
                                        }
                                        if (Speed < 72.4 && Speed >= 70.8)
                                        {
                                            if (bhjz_power != 7.4f)
                                            {
                                                bhjz_power = 7.4f;
                                                Set_Control_Power(7.4f);
                                            }
                                            t6 += 0.01;
                                        }
                                        if (Speed < 70.8 && Speed >= 69.2)
                                        {
                                            if (bhjz_power != 5.9f)
                                            {
                                                bhjz_power = 5.9f;
                                                Set_Control_Power(5.9f);
                                            }
                                            t7 += 0.01;
                                        }
                                        if (Speed < 69.2 && Speed >= 67.6)
                                        {
                                            if (bhjz_power != 7.4f)
                                            {
                                                bhjz_power = 7.4f;
                                                Set_Control_Power(7.4f);
                                            }
                                            t8 += 0.01;
                                        }
                                        if (Speed < 67.6 && Speed >= 66.0)
                                        {
                                            if (bhjz_power != 8.8f)
                                            {
                                                bhjz_power = 8.8f;
                                                Set_Control_Power(8.8f);
                                            }
                                            t9 += 0.01;
                                        }
                                        if (Speed < 66.0 && Speed >= 64.4)
                                        {
                                            if (bhjz_power != 10.3f)
                                            {
                                                bhjz_power = 10.3f;
                                                Set_Control_Power(10.3f);
                                            }
                                            t10 += 0.01;
                                        }
                                        if (Speed < 64.4 && Speed >= 62.8)
                                        {
                                            if (bhjz_power != 11.8f)
                                            {
                                                bhjz_power = 11.8f;
                                                Set_Control_Power(11.8f);
                                            }
                                            t11 += 0.01;
                                        }
                                        if (Speed < 62.8 && Speed >= 61.1)
                                        {
                                            if (bhjz_power != 13.2f)
                                            {
                                                bhjz_power = 13.2f;
                                                Set_Control_Power(13.2f);
                                            }
                                            t12 += 0.01;
                                        }
                                        if (Speed < 61.1 && Speed >= 59.5)
                                        {
                                            if (bhjz_power != 14.7f)
                                            {
                                                bhjz_power = 14.7f;
                                                Set_Control_Power(14.7f);
                                            }
                                            t13 += 0.01;
                                        }
                                        if (Speed < 59.5 && Speed >= 57.9)
                                        {
                                            if (bhjz_power != 15.4f)
                                            {
                                                bhjz_power = 15.4f;
                                                Set_Control_Power(15.4f);
                                            }
                                            t14 += 0.01;
                                        }
                                        if (Speed < 57.9 && Speed >= 56.3)
                                        {
                                            if (bhjz_power != 16.2f)
                                            {
                                                bhjz_power = 16.2f;
                                                Set_Control_Power(16.2f);
                                            }
                                            t15 += 0.01;
                                        }
                                        if (Speed < 56.3 && Speed >= 54.7)
                                        {
                                            if (bhjz_power != 16.9f)
                                            {
                                                bhjz_power = 16.9f;
                                                Set_Control_Power(16.9f);
                                            }
                                            t16 += 0.01;
                                        }
                                        if (Speed < 54.7 && Speed >= 53.1)
                                        {
                                            if (bhjz_power != 17.6f)
                                            {
                                                bhjz_power = 17.6f;
                                                Set_Control_Power(17.6f);
                                            }
                                            t17 += 0.01;
                                        }
                                        if (Speed < 53.1 && Speed >= 51.5)
                                        {
                                            if (bhjz_power != 18.4f)
                                            {
                                                bhjz_power = 18.4f;
                                                Set_Control_Power(18.4f);
                                            }
                                            t18 += 0.01;
                                        }
                                        if (Speed < 51.5 && Speed >= 49.9)
                                        {
                                            if (bhjz_power != 17.6f)
                                            {
                                                bhjz_power = 17.6f;
                                                Set_Control_Power(17.6f);
                                            }
                                            t19 += 0.01;
                                        }
                                        if (Speed < 49.9 && Speed >= 48.3)
                                        {
                                            if (bhjz_power != 16.9f)
                                            {
                                                bhjz_power = 16.9f;
                                                Set_Control_Power(16.9f);
                                            }
                                            t20 += 0.01;
                                        }
                                        if (Speed < 48.3 && Speed >= 46.7)
                                        {
                                            if (bhjz_power != 16.2f)
                                            {
                                                bhjz_power = 16.2f;
                                                Set_Control_Power(16.2f);
                                            }
                                            t21 += 0.01;
                                        }
                                        if (Speed < 46.7 && Speed >= 45.1)
                                        {
                                            if (bhjz_power != 15.4f)
                                            {
                                                bhjz_power = 15.4f;
                                                Set_Control_Power(15.4f);
                                            }
                                            t22 += 0.01;
                                        }
                                        if (Speed < 45.1 && Speed >= 43.4)
                                        {
                                            if (bhjz_power != 14.7f)
                                            {
                                                bhjz_power = 14.7f;
                                                Set_Control_Power(14.7f);
                                            }
                                            t23 += 0.01;
                                        }
                                        if (Speed < 43.4 && Speed >= 41.8)
                                        {
                                            if (bhjz_power != 13.2f)
                                            {
                                                bhjz_power = 13.2f;
                                                Set_Control_Power(13.2f);
                                            }
                                            t24 += 0.01;
                                        }
                                        if (Speed < 41.8 && Speed >= 40.2)
                                        {
                                            if (bhjz_power != 11.8f)
                                            {
                                                bhjz_power = 11.8f;
                                                Set_Control_Power(11.8f);
                                            }
                                            t25 += 0.01;
                                        }
                                        if (Speed < 40.2 && Speed >= 38.6)
                                        {
                                            if (bhjz_power != 10.3f)
                                            {
                                                bhjz_power = 10.3f;
                                                Set_Control_Power(10.3f);
                                            }
                                            t26 += 0.01;
                                        }
                                        if (Speed < 38.6 && Speed >= 37.0)
                                        {
                                            if (bhjz_power != 11.0f)
                                            {
                                                bhjz_power = 11.0f;
                                                Set_Control_Power(11.0f);
                                            }
                                            t27 += 0.01;
                                        }
                                        if (Speed < 37.0 && Speed >= 35.4)
                                        {
                                            if (bhjz_power != 11.8f)
                                            {
                                                bhjz_power = 11.8f;
                                                Set_Control_Power(11.8f);
                                            }
                                            t28 += 0.01;
                                        }
                                        if (Speed < 35.4 && Speed >= 33.8)
                                        {
                                            if (bhjz_power != 12.5f)
                                            {
                                                bhjz_power = 12.5f;
                                                Set_Control_Power(12.5f);
                                            }
                                            t29 += 0.01;
                                        }
                                        if (Speed < 33.8 && Speed >= 32.2)
                                        {
                                            if (bhjz_power != 13.2f)
                                            {
                                                bhjz_power = 13.2f;
                                                Set_Control_Power(13.2f);
                                            }
                                            t30 += 0.01;
                                        }
                                        if (Speed < 32.2 && Speed >= 30.6)
                                        {
                                            if (bhjz_power != 12.5f)
                                            {
                                                bhjz_power = 12.5f;
                                                Set_Control_Power(12.5f);
                                            }
                                            t31 += 0.01;
                                        }
                                        if (Speed < 30.6 && Speed >= 29.0)
                                        {
                                            if (bhjz_power != 11.8f)
                                            {
                                                bhjz_power = 11.8f;
                                                Set_Control_Power(11.8f);
                                            }
                                            t32 += 0.01;
                                        }
                                        if (Speed < 29.0 && Speed >= 27.4)
                                        {
                                            if (bhjz_power != 11.0f)
                                            {
                                                bhjz_power = 11.0f;
                                                Set_Control_Power(11.0f);
                                            }
                                            t33 += 0.01;
                                        }
                                        if (Speed < 27.4 && Speed >= 25.7)
                                        {
                                            if (bhjz_power != 10.3f)
                                            {
                                                bhjz_power = 10.3f;
                                                Set_Control_Power(10.3f);
                                            }
                                            t34 += 0.01;
                                        }
                                        if (Speed < 25.7 && Speed >= 24.1)
                                        {
                                            if (bhjz_power != 8.8f)
                                            {
                                                bhjz_power = 8.8f;
                                                Set_Control_Power(8.8f);
                                            }
                                            t35 += 0.01;
                                        }
                                        if (Speed < 24.1 && Speed >= 22.5)
                                        {
                                            if (bhjz_power != 7.4f)
                                            {
                                                bhjz_power = 7.4f;
                                                Set_Control_Power(7.4f);
                                            }
                                            t36 += 0.01;
                                        }
                                        if (Speed < 22.5 && Speed >= 20.9)
                                        {
                                            if (bhjz_power != 8.1f)
                                            {
                                                bhjz_power = 8.1f;
                                                Set_Control_Power(8.1f);
                                            }
                                            t37 += 0.01;
                                        }
                                        if (Speed < 20.9 && Speed >= 19.3)
                                        {
                                            if (bhjz_power != 8.8f)
                                            {
                                                bhjz_power = 8.8f;
                                                Set_Control_Power(8.8f);
                                            }
                                            t33 += 0.01;
                                        }
                                        if (Speed < 19.3 && Speed >= 17.7)
                                        {
                                            if (bhjz_power != 8.1f)
                                            {
                                                bhjz_power = 8.1f;
                                                Set_Control_Power(8.1f);
                                            }
                                            t38 += 0.01;
                                        }
                                        if (Speed < 17.7 && Speed >= 16.1)
                                        {
                                            if (bhjz_power != 7.4f)
                                            {
                                                bhjz_power = 7.4f;
                                                Set_Control_Power(7.4f);
                                            }
                                            t39 += 0.01;
                                        }
                                        if (Speed < 16.1 && Speed >= 14.5)
                                        {
                                            if (bhjz_power != 6.6f)
                                            {
                                                bhjz_power = 6.6f;
                                                Set_Control_Power(6.6f);
                                            }
                                            t40 += 0.01;
                                        }
                                        if (Speed < 14.5 && Speed >= 12.9)
                                        {
                                            if (bhjz_power != 5.9f)
                                            {
                                                bhjz_power = 5.9f;
                                                Set_Control_Power(5.9f);
                                            }
                                            t41 += 0.01;
                                        }
                                        if (Speed < 12.9 && Speed >= 11.3)
                                        {
                                            if (bhjz_power != 5.1f)
                                            {
                                                bhjz_power = 5.1f;
                                                Set_Control_Power(5.1f);
                                            }
                                            t42 += 0.01;
                                        }
                                        if (Speed < 11.3 && Speed >= 9.7)
                                        {
                                            if (bhjz_power != 4.4f)
                                            {
                                                bhjz_power = 4.4f;
                                                Set_Control_Power(4.4f);
                                            }
                                            t43 += 0.01;
                                        }
                                        if (Speed < 9.7 && Speed >= 8.0)
                                        {
                                            if (bhjz_power != 3.7f)
                                            {
                                                bhjz_power = 3.7f;
                                                Set_Control_Power(3.7f);
                                            }
                                            t44 += 0.01;
                                        }
                                        if (Speed < 8.0)                                       //区间记录完成
                                        {
                                            bhjz_power=0f;
                                            testFinish = true;
                                            nowState = testState.idle_state;

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

        #region 设置

        private void button_savezdgl_Click(object sender, EventArgs e)
        {
            try
            {
                if ((ini.INIIO.WritePrivateProfileString("DIW", "DIW", textBox_zdgl.Text.Trim(), @".\Config.ini")))
                {
                    if(radioButtonQx.Checked==true)
                        ini.INIIO.WritePrivateProfileString("DIW", "DIWLX", "轻型车", @".\Config.ini");
                    else
                        ini.INIIO.WritePrivateProfileString("DIW", "DIWLX", "重型车", @".\Config.ini");
                    DIW = float.Parse(textBox_zdgl.Text.Trim());
                    MessageBox.Show("保存成功", "系统提示");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString(), "出错");
            }
        }

        
        #endregion

        #region 基本功能测试(Test Complete1)

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabPage_2_Enter(object sender, EventArgs e)
        {
            
            jbgl_exe();
            jbgl1_exe();
        }
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void jbgl_exe()
        {
            radioButton_gzms1.Checked = true;
            label72.Text = "km/h";
            if (radioButtonQx.Checked == true)
            {
                textBoxSpeedKp.Text = kp_float_default.ToString();
                textBoxSpeedKi.Text = ki_float_default.ToString();
                textBoxSpeedKd.Text = kd_float_default.ToString();
                textBoxForceKp.Text = kp_force_default.ToString();
                textBoxForceKi.Text = ki_force_default.ToString();
                textBoxForceKd.Text = kd_force_default.ToString();
            }
            else
            {
                textBoxSpeedKp.Text = kp_float_zx.ToString();
                textBoxSpeedKi.Text = ki_float_zx.ToString();
                textBoxSpeedKd.Text = kd_float_zx.ToString();
                textBoxForceKp.Text = kp_force_zx.ToString();
                textBoxForceKi.Text = ki_force_zx.ToString();
                textBoxForceKd.Text = kd_force_zx.ToString();
            }
        }
        public void jbgl1_exe()
        {
            try
            {
                Random random = new Random();
                for (int pointIndex = 0; pointIndex < 10; pointIndex++)
                {
                    chart_jbgl.Series["功率"].Points.AddY(random.Next(45, 45));
                    chart_jbgl.Series["速度"].Points.AddY(random.Next(35, 35));
                    chart_jbgl.Series["扭力"].Points.AddY(random.Next(25, 25));
                }
                try
                {
                    th_jbglcs.Abort();
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
                MessageBox.Show("初始化错误", "系统提示");
            }
        }
        private void radioButton_gzms1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_gzms1.Checked)
            {
                label72.Text = "km/h";
                radioButton_gzms2.Checked = false;
                radioButton_gzms4.Checked = false;
            }
        }

        private void radioButton_gzms2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_gzms2.Checked)
            {
                label72.Text = "N";
                radioButton_gzms1.Checked = false;
                radioButton_gzms4.Checked = false;
            }
        }


        private void radioButton_gzms4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_gzms4.Checked)
            {
                label72.Text = "Kw";
                radioButton_gzms1.Checked = false;
                radioButton_gzms2.Checked = false;
            }
        }
        //每隔一定时间需要执行的函数体,timer start后每隔一秒就会执行该函数
        //Control.CheckForIllegalCrossThreadCalls = false;
        //chart_jbgl.Series["速度"].ChartType = SeriesChartType.Spline;
        //chart_jbgl.Series["扭力"].ChartType = SeriesChartType.Spline;
        //chart_jbgl.Series["功率"].ChartType = SeriesChartType.Spline;
        private void Ref_Chart_th()
        {
            while (true)
            {
                try
                {
                    Ref_Chart(Speed, "速度");
                    Ref_Chart(Force, "扭力");
                    Ref_Chart(Power, "功率");
                    if (jbgl_time >= 50)
                    {
                        Ref_Clear("速度");
                        Ref_Clear("扭力");
                        Ref_Clear("功率");
                    }
                }
                catch (Exception)
                {
                    //MessageBox.Show("没有相关数据", "出错啦");
                    //return;
                }
                Thread.Sleep(100);
                jbgl_time ++;
                if (jbgl_time > 60000) jbgl_time = 50;
            }
        }

        //曲线暂停和曲线继续
        private void button_qxzt_Click(object sender, EventArgs e)
        {
            try
            {
                if (button_qxzt.Text == "曲线暂停")
                {
                    try
                    {
                        th_jbglcs.Abort();
                    }
                    catch (Exception)
                    {
                    }
                    button_qxzt.Text = "曲线继续";
                }
                else if (button_qxzt.Text == "曲线继续")
                {
                    th_jbglcs = new Thread(Ref_Chart_th);
                    th_jbglcs.Start();
                    button_qxzt.Text = "曲线暂停";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("退出系统，重新启动", "系统提示");
            }
        }
        public void ref_chart_data(float Data, string SeriesName)
        {
            try
            {
                chart_jbgl.Series[SeriesName].Points.AddY(Data);
            }
            catch (Exception)
            {

            }
        }
        public void Ref_Chart(float Data, string SeriesName)
        {
            Invoke(new wtfs(ref_chart_data), Data, SeriesName);
        }
        public void Ref_Clear(string SeriesName)
        {
            Invoke(new wtds_clr(ref_chart_clear), SeriesName);
        }
        public void Ref_textbox(TextBox textbox, string text)
        {
            Invoke(new wtdisplay_textbox(display_textbox), textbox, text);
        }
        void display_textbox(TextBox textbox, string text)
        {
            textbox.Text = text;
        }
        void ref_chart_clear(string SeriesName)
        {
            try
            {
                chart_jbgl.Series[SeriesName].Points.RemoveAt(0);
            }
            catch (Exception)
            {
            }
        }
        public void clear_chart_data()
        {
            try
            {
                foreach (System.Windows.Forms.DataVisualization.Charting.Series series in chart_jbgl.Series)
                    series.Points.Clear();
            }
            catch (Exception)
            {

            }
        }
        public void Clear_Chart()
        {
            BeginInvoke(new wt_void(clear_chart_data));
        }

        //开始发送参数
        private void button_csfs_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    float.Parse(textBox_cs.Text.ToString());
                }
                catch (Exception)
                {
                    throw;
                }
                th_jbglcs = new Thread(Ref_Chart_th);
                th_jbglcs.Start();
                Test_Flag = true;
                //确定恒速度
                chart_jbgl.Series["速度"].BorderWidth = 1;
                chart_jbgl.Series["扭力"].BorderWidth = 1;
                chart_jbgl.Series["功率"].BorderWidth = 1;
                if (radioButton_gzms1.Checked)
                {
                    chart_jbgl.Series["速度"].BorderWidth = 3;
                    Control_Speed = float.Parse(textBox_cs.Text.ToString());
                    // Exit_Control();                                             //IGBT退出所有控制
                     Set_Speed(Control_Speed);                                   //设置恒速度值
                     Start_Control_Speed();                                      //启动恒速度控制
                        
                }
                if (radioButton_gzms2.Checked)
                {
                    chart_jbgl.Series["扭力"].BorderWidth = 3;
                    Control_Force = float.Parse(textBox_cs.Text.ToString());
                    // Exit_Control();                                             //IGBT退出所有控制
                     Set_Control_Force(Control_Force);                                   //设置恒速度值
                     Start_Control_Force();                                      //启动恒速度控制
                }
                if (radioButton_gzms4.Checked)
                {
                    chart_jbgl.Series["功率"].BorderWidth = 3;
                    Control_Power = float.Parse(textBox_cs.Text.ToString());
                    // Exit_Control();                                             //IGBT退出所有控制
                     Set_Control_Power(Control_Power);                                   //设置恒速度值
                     Start_Control_Power();                                      //启动恒速度控制
                }
                nowState = testState.hengding_state;
            }
            catch (Exception)
            {
                MessageBox.Show("请输入参数", "系统提示");
            }
        }

        //清除曲线
        private void button_qcqx_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (System.Windows.Forms.DataVisualization.Charting.Series series in chart_jbgl.Series)        //清空
                {
                    series.Points.Clear();
                }
                button_csfs.Enabled = true;
                button_qxzt.Text = "曲线暂停";
                textBox_cs.Enabled = true;
                radioButton_gzms1.Enabled = true;
                radioButton_gzms2.Enabled = true;
                radioButton_gzms4.Enabled = true;
                textBox_cs.Text = "";
                jbgl_time = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("系统错误，请重新启动", "系统提示");
            }
        }
        #endregion

        #region 寄生功率测试(Test Complete1)


        private void tabPage_3_Enter(object sender, EventArgs e)
        {
            if (!Test_Flag)                               //判定是否有测试正在进行
            {
                //if (page != tabPage_3)
                //{
                    jsg_Init_Other();
               // }
                jsg_Init_Limit();
                jsg_Init_Chart();
            }
            page = tabPage_3;
        }

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
            if (radioButton_daily.Checked == true)
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
            }
            dataGrid_jsgl.DataSource = dt;

        }

        private void button_start_jsg_Click(object sender, EventArgs e)
        {
            if (!Test_Flag)                             //判断是不是有测试正在进行
            {
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
                th_jsgcs = new Thread(Jsgcs_exe);       //新建线程
                th_jsgcs.Start();                       //线程开始执行
                testFinish = false;                     //测试还未结束
                timer_flag = false;
                Test_Flag = true;                       //正在测试的标记置为true 表示有测试正在进行
                tabControl1.Enabled = false;            //设置不能在页面中切换
                button_start_jsg.Text = "重新开始";
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
            Thread.Sleep(2000);         //等待2秒
             Motor_Open();          //启动电机加速
            while (testFinish == false)
            {
                Thread.Sleep(100);         //等待2秒
            }
             Exit_Control();
           //计算
                if (radioButton_daily.Checked)
                {
                    //dataGrid_jsgl.Rows[0].Cells["滑行时间（s）"].Value = t1.ToString("0.00");
                    //dataGrid_jsgl.Rows[1].Cells["滑行时间（s）"].Value = t2.ToString("0.00");
                    //dataGrid_jsgl.Rows[2].Cells["滑行时间（s）"].Value = t3.ToString("0.00");
                    //dataGrid_jsgl.Rows[3].Cells["滑行时间（s）"].Value = t4.ToString("0.00");
                    jsgl1 = Math.Round(0.00123457 * 48 * DIW / t1, 2);
                    jsgl2 = Math.Round(0.00123457 * 40 * DIW / t2, 2);
                    jsgl3 = Math.Round(0.00123457 * 32 * DIW / t3, 2);
                    jsgl4 = Math.Round(0.00123457 * 24 * DIW / t4, 2);
                }
                else
                {
                    //dataGrid_jsgl.Rows[0].Cells["滑行时间（s）"].Value = t1.ToString("0.00");
                    //dataGrid_jsgl.Rows[1].Cells["滑行时间（s）"].Value = t2.ToString("0.00");
                    //dataGrid_jsgl.Rows[2].Cells["滑行时间（s）"].Value = t3.ToString("0.00");
                    //dataGrid_jsgl.Rows[3].Cells["滑行时间（s）"].Value = t4.ToString("0.00");
                    //dataGrid_jsgl.Rows[4].Cells["滑行时间（s）"].Value = t5.ToString("0.00");
                    //dataGrid_jsgl.Rows[5].Cells["滑行时间（s）"].Value = t6.ToString("0.00");
                    //dataGrid_jsgl.Rows[6].Cells["滑行时间（s）"].Value = t7.ToString("0.00");
                    //dataGrid_jsgl.Rows[7].Cells["滑行时间（s）"].Value = t8.ToString("0.00");
                    //dataGrid_jsgl.Rows[8].Cells["滑行时间（s）"].Value = t9.ToString("0.00");
                    //dataGrid_jsgl.Rows[9].Cells["滑行时间（s）"].Value = t10.ToString("0.00");
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
                }
                //显示和记录

                
                dataGrid_jsgl.Rows[0].Cells["寄生功率（Kw）"].Value = jsgl1.ToString("0.00");
                
                dataGrid_jsgl.Rows[1].Cells["寄生功率（Kw）"].Value = jsgl2.ToString("0.00");
                
                dataGrid_jsgl.Rows[2].Cells["寄生功率（Kw）"].Value = jsgl3.ToString("0.00");
                
                dataGrid_jsgl.Rows[3].Cells["寄生功率（Kw）"].Value = jsgl4.ToString("0.00");


                if (!radioButton_daily.Checked)
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

                }
                if (radioButton_daily.Checked)
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
                if (radioButton_daily.Checked)
                {
                    ini.INIIO.WritePrivateProfileString("寄生功率(日常测试)", "48Km/h", jsgl1.ToString("0.00"), @".\Config.ini");
                    ini.INIIO.WritePrivateProfileString("寄生功率(日常测试)", "40Km/h", jsgl2.ToString("0.00"), @".\Config.ini");
                    ini.INIIO.WritePrivateProfileString("寄生功率(日常测试)", "32Km/h", jsgl3.ToString("0.00"), @".\Config.ini");
                    ini.INIIO.WritePrivateProfileString("寄生功率(日常测试)", "24Km/h", jsgl4.ToString("0.00"), @".\Config.ini");
                }
                else
                {
                    ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "88Km/h", jsgl1.ToString("0.00"), @".\Config.ini");
                    ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "80Km/h", jsgl2.ToString("0.00"), @".\Config.ini");
                    ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "72Km/h", jsgl3.ToString("0.00"), @".\Config.ini");
                    ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "64Km/h", jsgl4.ToString("0.00"), @".\Config.ini");
                    ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "56Km/h", jsgl5.ToString("0.00"), @".\Config.ini");
                    ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "48Km/h", jsgl6.ToString("0.00"), @".\Config.ini");
                    ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "40Km/h", jsgl7.ToString("0.00"), @".\Config.ini");
                    ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "32Km/h", jsgl8.ToString("0.00"), @".\Config.ini");
                    ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "24Km/h", jsgl9.ToString("0.00"), @".\Config.ini");
                    ini.INIIO.WritePrivateProfileString("寄生功率(核准)", "16Km/h", jsgl10.ToString("0.00"), @".\Config.ini");
                }
                Thread.Sleep(3000);         //等待滚筒停止
                 Lifter_Up();           //测试完成举升上升
                this.BeginInvoke(new wtb(Set_tab_Enabled), true);       //设置能在页面中切换
                Test_Flag = false;          //重置正在测试的标记
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
                if (radioButton_daily.Checked)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        ini.INIIO.GetPrivateProfileString("寄生功率(日常测试)", (24 + 8 * i).ToString() + "Km/h", "Empty", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
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
                        ini.INIIO.GetPrivateProfileString("寄生功率(核准)", (16 + 8 * i).ToString() + "Km/h", "Empty", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
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

        #region 加载滑行试验(Test Complete1)
        private void tabPage_4_Enter(object sender, EventArgs e)
        {
            if (!Test_Flag)                               //判定是否有测试正在进行
            {
                if (page != tabPage_4)
                {
                    DataGridView_jzhx();
                }
            }
            page = tabPage_4;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void DataGridView_jzhx()
        {
            dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add("速度区间(km/h)");
            dt.Columns.Add("名义速度(km/h)");
            dt.Columns.Add("寄生功率(Kw)");
            dt.Columns.Add("计算时间CCDT(s)");
            dt.Columns.Add("实测时间ACDT(s)");
            dt.Columns.Add("误差");
            dt.Columns.Add("判定");
            if (radioButton_daily.Checked == true)
            {
                for (int i = 0; i < 2; i++)
                {
                    dr = dt.NewRow();
                    dr["速度区间（Km/h）"] = (48 - i * 16).ToString() + "-" + (48 - (i + 1) * 16).ToString();
                    dr["名义速度（Km/h）"] = (40 - i * 16).ToString();
                    dt.Rows.Add(dr);
                }
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    dr = dt.NewRow();
                    dr["速度区间（Km/h）"] = (48 - i * 16).ToString() + "-" + (48 - (i + 1) * 16).ToString();
                    dr["名义速度（Km/h）"] = (40 - i * 16).ToString();
                    dt.Rows.Add(dr);
                }
            }
            dataGrid_jzhx.DataSource = dt;
        }

        /// <summary>
        /// 输入限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_zsgl_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool IsContaintDot = this.textBox_zsgl.Text.Contains(".");
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
            {
                e.Handled = true;
            }
            else if (IsContaintDot && (e.KeyChar == 46))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 开始按钮
        /// </summary>
        private void button_jzks_Click(object sender, EventArgs e)
        {

            if (!Test_Flag)                                           //判断是不是有测试正在进行
            {
                th_jzhx = new Thread(Jzhx_exe);                       //新建线程
                th_jzhx.Start();                                      //线程开始执行
                testFinish = false;
                timer_flag = false;
                Test_Flag = true;                                     //正在测试的标记置为true 表示有测试正在进行
                tabControl1.Enabled = false;                          //设置不能在页面中切换
                button_jzks.Text = "重新开始";
                nowState = testState.jzhx_state;

            }
            else
                MessageBox.Show("有测试正在进行，无法开始测试", "系统提示");
        }

        private void button_JzhxText_Click(object sender, EventArgs e)
        {
            try
            {
                StreamReader sr = File.OpenText(File_Name2);
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
        /// 加载滑行测试
        /// </summary>
        /// 
        public void Jzhx_exe()
        {
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
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            double CCDT1 = 0, CCDT2 = 0, CCDT3 = 0, CCDT4 = 0;
            double PLHP1 = 0, PLHP2 = 0, PLHP3 = 0, PLHP4 = 0;
            double result1 = 0, result2 = 0, result3 = 0, result4 = 0;
            string r1 = "", r2 = "";
            System.Windows.Forms.DataVisualization.Charting.DataPoint jsg_point = null;
            string d = textBox_zsgl.Text;  //指示功率
            if (radioButton_daily.Checked)
            {
                 ini.INIIO.GetPrivateProfileString("寄生功率(日常测试)", "40Km/h", "Empty", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                 if (temp.ToString() == "Empty")
                {
                     MessageBox.Show("没有相关数据", "出错啦");
                     return;
                }
                 jsgl2 = double.Parse(temp.ToString());
                 
                 ini.INIIO.GetPrivateProfileString("寄生功率(日常测试)", "24Km/h", "Empty", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                 if (temp.ToString() == "Empty")
                 {
                     MessageBox.Show("没有相关数据", "出错啦");
                     return;
                 }
                 jsgl4= double.Parse(temp.ToString());
                 CCDT1 = (Math.Round(DIW * (48 * 48 - 32 * 32) / (2000 * (Convert.ToDouble(d) + jsgl2)), 1)) / 12.96;
                 CCDT2 = (Math.Round((DIW * (32 * 32 - 16 * 16)) / (2000 * (Convert.ToDouble(d) + jsgl4)), 1)) / 12.96;
                 dataGrid_jzhx.Rows[0].Cells["寄生功率(Kw)"].Value = jsgl2.ToString("0.00");
                 dataGrid_jzhx.Rows[0].Cells["计算时间CCDT(s)"].Value = CCDT1.ToString("0.00"); 
                 dataGrid_jzhx.Rows[1].Cells["寄生功率(Kw)"].Value = jsgl4.ToString("0.00");
                 dataGrid_jzhx.Rows[1].Cells["计算时间CCDT(s)"].Value = CCDT2.ToString("0.00");
             }
             else
            {
                ini.INIIO.GetPrivateProfileString("寄生功率(日常测试)", "40Km/h", "Empty", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                if (temp.ToString() == "Empty")
                {
                     MessageBox.Show("没有相关数据", "出错啦");
                    return;
                }
                jsgl2 = double.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("寄生功率(日常测试)", "24Km/h", "Empty", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                if (temp.ToString() == "Empty")
                {
                    MessageBox.Show("没有相关数据", "出错啦");
                    return;
                }
                jsgl4 = double.Parse(temp.ToString());
                dataGrid_jzhx.Rows[0].Cells["寄生功率(Kw)"].Value = jsgl2.ToString("0.00");
                dataGrid_jzhx.Rows[0].Cells["计算时间CCDT(s)"].Value = CCDT3.ToString("0.00");
                dataGrid_jzhx.Rows[1].Cells["寄生功率(Kw)"].Value = jsgl4.ToString("0.00");
                dataGrid_jzhx.Rows[1].Cells["计算时间CCDT(s)"].Value = CCDT4.ToString("0.00");
                CCDT3 = (Math.Round((DIW * (48 * 48 - 32 * 32)) / (2000 * (Convert.ToDouble(d) + jsgl2)), 1)) / 12.96;
                CCDT4 = (Math.Round((DIW * (32 * 32 - 16 * 16)) / (2000 * (Convert.ToDouble(d) + jsgl4)), 1)) / 12.96;
            }
            float power = float.Parse(textBox_zsgl.Text);
            Set_Control_Power(power);  
             Lifter_Down();         //下降举升
            Thread.Sleep(2000);         //等待2秒
             Motor_Open();          //启动电机加速
            while(testFinish == false)
            {
                Thread.Sleep(100);
            }
             Exit_Control();
            //testFinish = true;
            t1 = Math.Round(t1, 1);
            t2 = Math.Round(t2, 1);
            t3 = Math.Round(t3, 1);
            t4 = Math.Round(t4, 1);
            try
                {
                    if (radioButton_daily.Checked == true)
                    {
                        

                        result1 = Math.Round(Math.Abs(CCDT1 - t1) / CCDT1, 2);
                        result2 = Math.Round(Math.Abs(CCDT2 - t2) / CCDT2, 2);

                    }
                    else
                    {
                       

                        result3 = Math.Round(Math.Abs(t3 - CCDT3) / CCDT3, 2);
                        result4 = Math.Round(Math.Abs(t4 - CCDT4) / CCDT4, 2);

                    }
                    //误差可以扩大到10%
                    if (radioButton_daily.Checked == true)
                    {
                        if (result1 <= 0.1)
                        { r1 = "合格"; }
                        else
                        { r1 = "不合格"; }
                        if (result2 <= 0.1)
                        { r2 = "合格"; }
                        else
                        { r2 = "不合格"; }
                    }
                    else
                    {

                        if (result3 <= 0.1)
                        { r1 = "合格"; }
                        else
                        { r1 = "不合格"; }
                        if (result4 <= 0.1)
                        { r2 = "合格"; }
                        else
                        { r2 = "不合格"; }
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.ToString(), "出错啦");
                }

                //显示
                if (radioButton_daily.Checked == true)
                {
                    

                    dataGrid_jzhx.Rows[0].Cells["误差"].Value = result1.ToString("0.00");
                    dataGrid_jzhx.Rows[0].Cells["判定"].Value = r1.ToString();

                    
                    //dataGrid_jzhx.Rows[1].Cells["实测时间ACDT(s)"].Value = t2.ToString();
                    dataGrid_jzhx.Rows[1].Cells["误差"].Value = result2.ToString("0.00");
                    dataGrid_jzhx.Rows[1].Cells["判定"].Value = r2.ToString();
                }
                else
                {
                    
                    //dataGrid_jzhx.Rows[0].Cells["实测时间ACDT(s)"].Value = t3.ToString();
                    dataGrid_jzhx.Rows[0].Cells["误差"].Value = result3.ToString("0.00");
                    dataGrid_jzhx.Rows[0].Cells["判定"].Value = r1.ToString();

                    
                    //dataGrid_jzhx.Rows[1].Cells["实测时间ACDT(s)"].Value = t4.ToString();
                    dataGrid_jzhx.Rows[1].Cells["误差"].Value = result4.ToString("0.00");
                    dataGrid_jzhx.Rows[1].Cells["判定"].Value = r2.ToString();
                }
                Thread.Sleep(3000);         //等待滚筒停止
                 Lifter_Up();           //测试完成举升上升
                 Exit_Control();
                this.BeginInvoke(new wtb(Set_tab_Enabled), true);       //设置能在页面中切换
                Test_Flag = false;
                          //重置正在测试的标记 
                        
        }
        public void Locked()
        {
            //锁止程序
        }
        #endregion

        #region 惯量测试(Test Complete1)

        //初始化
        private void tabPage_5_Enter(object sender, EventArgs e)
        {
            if (!Test_Flag)                               //判定是否有测试正在进行
            {
                if (page != tabPage_5)
                {
                    dataGridView_gl();
                }
            }
            page = tabPage_5;
        }

        public void dataGridView_gl()
        {
            dt = new DataTable();
            glTestStep = 0;
            DataRow dr = null;
            //DataRow dr1 = null;
            //DataRow dr2 = null;
            dt.Columns.Add("次数");
            dt.Columns.Add("滑行区间(km/h)");
            dt.Columns.Add(gl_power_string);
            dt.Columns.Add("13KW滑行时间(s)");
            dt.Columns.Add("标称(kg)");
            dt.Columns.Add("实测(kg)");
            dt.Columns.Add("误差");
            dt.Columns.Add("判定");
            for (int i = 0; i < 3; i++)
            {
                dr = dt.NewRow();
                dr["标称(kg)"] = DIW.ToString();
                dr["滑行区间(km/h)"]=useMethod;
                dr["次数"]=Convert.ToString(i+1);
                dt.Rows.Add(dr);
            }
            dr = dt.NewRow();
            dr["实测(kg)"] = "平均：";
            dt.Rows.Add(dr);
            dataGrid_gl.DataSource = dt;
        }

        //开始测试
        private void button_lgks_Click(object sender, EventArgs e)
        {
            if (radioButton_check.Checked == true)
            {
                if (!Test_Flag)                             //判断是不是有测试正在进行
                {
                    t1 = 0f;
                    t2 = 0f;
                    t3 = 0f;
                    t4 = 0f;
                    t5 = 0f;
                    t6 = 0f;
                    t7= 0f;
                    t8 = 0f;
                    t9 = 0f;
                    t10 = 0f;
                    dataGridView_gl();
                    th_gl = new Thread(gl_exe);             //新建线程
                    th_gl.Start();                          //线程开始执行
                    testFinish = false;
                    nowState = testState.gl_state;
                    timer_flag = false;
                    Test_Flag = true;                       //正在测试的标记置为true 表示有测试正在进行
                    tabControl1.Enabled = false;            //设置不能在页面中切换
                    button_lgks.Text = "重新开始";
                }
                else
                    MessageBox.Show("有测试正在进行，无法开始测试", "系统提示");
            }
            else
            {
                MessageBox.Show("惯量测试是核准测试内容，请在“设置”页面的“测试类型设置”中选择“核准测试”", "系统提示");
            }
        }
        private void button_glText_Click(object sender, EventArgs e)
        {
            try
            {
                StreamReader sr = File.OpenText(File_Name3);
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

        //惯量测试
        public void gl_exe()
        {
            double limit_DIW_1 = DIW + 28;          //上限
            double limit_DIW_2 = DIW - 28;          //下限
            // double DIW1 = 0;
            double wc = 0;
            string r1 = "";
            //double t1 = 0;                 //正常结构状态下（48-32）km/h滑行时间
            //double t2 = 0;                 //拆去飞轮结构状态下（48-32）km/h滑行时间
            Lifter_Down();         //下降举升
            Thread.Sleep(2000);         //等待2秒
            
            Motor_Open();          //启动电机加速
            Set_Control_Power(gl_power_set);//第一次加载0N的力。
            if (use_gl_force == true)
            {
                Solidify_Force_Modulus(1, gl_force);
                Thread.Sleep(1000);
            }
            while (testFinish == false)
            {
                Thread.Sleep(100);
            }
            Exit_Control();
            if (use_gl_force == true)
                Solidify_Force_Modulus(1, real_force);
            //计算
            try
            {
                //DIWF = float.Parse(textBox__zdglf.Text);                       //得到厂家提供的飞轮的转动惯量
                DIW1 = (DIW1 + DIW2 + DIW3) / 3.0;
                wc = Math.Round((DIW1 - DIW) / DIW, 3);
                wc = Math.Abs(wc);
                if (DIW1 <= limit_DIW_1 && DIW1 >= limit_DIW_2 && wc <= 0.02)
                {
                    r1 = "合格";
                }
                else
                {
                    r1 = "不合格";
                }
                dataGrid_gl.Rows[3].Cells["实测(kg)"].Value = "平均："+DIW1.ToString("0.0");
                dataGrid_gl.Rows[3].Cells["误差"].Value = wc.ToString("0.000");
                dataGrid_gl.Rows[3].Cells["判定"].Value = r1;

            }
            catch (ArgumentOutOfRangeException e)
            {
                MessageBox.Show(e.Message);
            }
            //显示


            // Thread.Sleep(3000);         //等待滚筒停止
            Lifter_Up();           //测试完成举升上升
            this.BeginInvoke(new wtb(Set_tab_Enabled), true);       //设置能在页面中切换
            Test_Flag = false;          //重置正在测试的标记 
        }
        #endregion

        #region 变载荷加载滑行试验(Test Complete1)
        //初始化
        private void tabPage_6_Enter(object sender, EventArgs e)
        {
            if (!Test_Flag)                               //判定是否有测试正在进行
            {
                if (page != tabPage_6)
                {
                    Datagridview_bzhjzhx();
                }
            }
            page = tabPage_6;
        }

        public void Datagridview_bzhjzhx()
        {
            dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add("初速度(km/h)");
            dt.Columns.Add("末速度(km/h)");
            dt.Columns.Add("名义时间(s)");
            dt.Columns.Add("实测时间(s)");
            dt.Columns.Add("误差");
            dt.Columns.Add("验收标准");
            dt.Columns.Add("结果");
            dr = dt.NewRow();
            dr["初速度(km/h）"] = 80.5.ToString();
            dr["末速度(km/h）"] = 8.ToString();
            dr["名义时间(s）"] = 25.77.ToString();
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["初速度(km/h）"] = 72.4.ToString();
            dr["末速度(km/h）"] = 16.1.ToString();
            dr["名义时间(s）"] = 15.54.ToString();
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["初速度(km/h）"] = 61.1.ToString();
            dr["末速度(km/h）"] = 43.4.ToString();
            dr["名义时间(s）"] = 3.98.ToString();
            dt.Rows.Add(dr);
            dataGrid_bzhjzhx.DataSource = dt;
        }

        //开始测试按钮
        private void button_bzhjz_Click(object sender, EventArgs e)
        {
            if (radioButton_check.Checked == true)
            {
                if (!Test_Flag)                             //判断是不是有测试正在进行
                {
                    t1 = 0; t2 = 0; t3 = 0; t4 = 0; t5 = 0; t6 = 0; t7 = 0; t8 = 0; t9 = 0; t10 = 0;
                    t11 = 0; t12 = 0; t13 = 0; t14 = 0; t15 = 0; t16 = 0; t17 = 0; t18 = 0; t19 = 0; t20 = 0;
                    t21 = 0; t22 = 0; t23 = 0; t24 = 0; t25 = 0; t26 = 0; t27 = 0; t28 = 0; t29 = 0; t30 = 0;
                    t31 = 0; t32 = 0; t33 = 0; t34 = 0; t35 = 0; t36 = 0; t37 = 0; t38 = 0; t39 = 0; t40 = 0;
                    t41 = 0; t42 = 0; t43 = 0; t44 = 0;
                    th_bzhjz = new Thread(bzhjz_exe);       //新建线程
                    th_bzhjz.Start();                       //线程开始执行
                    timer_flag = false;
                    testFinish = false;
                    nowState = testState.bzhx_state;
                    Test_Flag = true;                       //正在测试的标记置为true 表示有测试正在进行
                    tabControl1.Enabled = false;            //设置不能在页面中切换
                    button_bzhjz.Text = "重新开始";
                }
                else
                    MessageBox.Show("有测试正在进行，无法开始测试", "系统提示");
            }
            else
            {
                MessageBox.Show("变载荷加载测试是核准测试内容，请在“设置”页面的“测试类型设置”中选择“核准测试”", "系统提示");
            }
        }

        private void button_bzhjzhxText_Click(object sender, EventArgs e)
        {
            try
            {
                StreamReader sr = File.OpenText(File_Name4);
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
        //变载荷加载滑行试验
        public void bzhjz_exe()
        {
            double tm1 = 25.77, tm2 = 15.54, tm3 = 3.98;           //标准
            double ts1 = 0, ts2 = 0, ts3 = 0;
            double result1 = 0, result2 = 0, result3 = 0;
            double Standard1 = 0.04, Standard2 = 0.02, Standard3 = 0.03;             //误差
            string r1 = "", r2 = "", r3 = "";
            // Exit_Control();        //退出所有控制
             Lifter_Down();         //下降举升
            Thread.Sleep(2000);         //等待2秒
             Motor_Open();          //启动电机加速
            while (testFinish == false)
            {
                Thread.Sleep(100);
            }
             Exit_Control();
            try
            {
                if (useMoni_bhjz == false)
                {
                    ts1 = t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 +
                       t11 + t12 + t13 + t14 + t15 + t16 + t17 + t18 + t19 + t20 +
                       t21 + t22 + t23 + t24 + t25 + t26 + t27 + t28 + t29 + t30 +
                       t31 + t32 + t33 + t34 + t35 + t36 + t37 + t38 + t39 +
                       t40 + t41 + t42 + t43 + t44;
                    ts2 = t6 + t7 + t8 + t9 + t10 +
                        t11 + t12 + t13 + t14 + t15 + t16 + t17 + t18 + t19 + t20 +
                        t21 + t22 + t23 + t24 + t25 + t26 + t27 + t28 + t29 + t30 +
                        t31 + t32 + t33 + t34 + t35 + t36 + t37 + t38;
                    ts3 = t13 + t14 + t15 + t16 + t17 + t18 + t19 + t20 +
                        t21 + t22;
                }
                else
                {
                    ts1 = t1_bhjz;
                    ts2 = t2_bhjz;
                    ts3 = t3_bhjz;
                }
                ts1 = Math.Round(ts1, 2) ;
                ts2 = Math.Round(ts2, 2) ;
                ts3 = Math.Round(ts3, 2) ;
                result1 = Math.Abs(ts1 - tm1) / ts1;
                result2 = Math.Abs(ts2 - tm2) / ts2;
                result3 = Math.Abs(ts3 - tm3) / ts3;

                result1 = Math.Round(result1, 2);
                result2 = Math.Round(result2, 2);
                result3 = Math.Round(result3, 2);

                if ((result1 <= Standard1))
                {
                    r1 = "合格";
                }
                else
                {
                    r1 = "不合格";
                }
                if ((result2 <= Standard2))
                {
                    r2 = "合格";
                }
                else
                {
                    r2 = "不合格";
                }
                if ((result3 <= Standard3))
                {
                    r3 = "合格";
                }
                else
                {
                    r3 = "不合格";
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
            //显示和记录

            dataGrid_bzhjzhx.Rows[0].Cells["实测时间(s)"].Value = ts1.ToString();
            dataGrid_bzhjzhx.Rows[0].Cells["误差"].Value = result1.ToString();
            dataGrid_bzhjzhx.Rows[0].Cells["验收标准"].Value = 0.04;
            dataGrid_bzhjzhx.Rows[0].Cells["结果"].Value = r1.ToString();


            dataGrid_bzhjzhx.Rows[1].Cells["实测时间(s)"].Value = ts2.ToString();
            dataGrid_bzhjzhx.Rows[1].Cells["误差"].Value = result2.ToString();
            dataGrid_bzhjzhx.Rows[1].Cells["验收标准"].Value = 0.02;
            dataGrid_bzhjzhx.Rows[1].Cells["结果"].Value = r2.ToString();


            dataGrid_bzhjzhx.Rows[2].Cells["实测时间(s)"].Value = ts3.ToString();
            dataGrid_bzhjzhx.Rows[2].Cells["误差"].Value = result3.ToString();
            dataGrid_bzhjzhx.Rows[2].Cells["验收标准"].Value = 0.03;
            dataGrid_bzhjzhx.Rows[2].Cells["结果"].Value = r3.ToString();



            //写文件
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "80.5Km/h", 3.7f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "78.8Km/h", 4.4f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "77.2Km/h", 5.1f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "75.6Km/h", 5.9f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "74.1Km/h", 6.6f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "72.4Km/h", 7.4f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "70.8Km/h", 5.9f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "69.2Km/h", 7.4f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "67.6Km/h", 8.8f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "66.1Km/h", 10.3f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "64.4Km/h", 11.8f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "62.8Km/h", 13.2f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "61.1Km/h", 14.7f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "59.5Km/h", 15.4f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "57.9Km/h", 16.2f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "56.3Km/h", 16.9f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "54.7Km/h", 17.6f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "53.1Km/h", 18.4f.ToString(), @".\Config.ini");

            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "51.5Km/h", 17.6f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "49.9Km/h", 16.9f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "48.3Km/h", 16.2f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "46.7Km/h", 15.4f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "45.1Km/h", 14.7f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "43.4Km/h", 13.2f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "41.8Km/h", 11.8f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "40.2Km/h", 10.3f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "38.6Km/h", 11.0f.ToString(), @".\Config.ini");

            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "37.1Km/h", 11.8f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "35.4Km/h", 12.5f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "33.8Km/h", 13.2f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "32.2Km/h", 12.5f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "30.6Km/h", 11.8f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "29.1Km/h", 11.0f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "27.4Km/h", 10.3f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "25.7Km/h", 8.8f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "24.1Km/h", 7.4f.ToString(), @".\Config.ini");

            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "22.5Km/h", 8.1f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "20.9Km/h", 8.8f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "19.3Km/h", 8.1f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "17.7Km/h", 7.4f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "16.1Km/h", 6.6f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "14.5Km/h", 5.9f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "12.9Km/h", 5.1f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "11.3Km/h", 4.4f.ToString(), @".\Config.ini");
            ini.INIIO.WritePrivateProfileString("变载荷加载(核准)", "9.7Km/h", 3.7f.ToString(), @".\Config.ini");

            //结束操作
            Thread.Sleep(3000);         //等待滚筒停止
             Lifter_Up();           //测试完成举升上升
            //tabControl1.Enabled = true; //设置能在页面中切换
            this.BeginInvoke(new wtb(Set_tab_Enabled), true);       //设置能在页面中切换
            Test_Flag = false;          //重置正在测试的标记
        }

        //显示曲线
        private void button_xsqx_Click(object sender, EventArgs e)
        {
            if (chart_bzhjz.Visible)
            {
                chart_bzhjz.Visible = false;
                button_xsqx.Text = "显示曲线";
            }
            else
            {
                StringBuilder temp = new StringBuilder();
                temp.Length = 2048;
                foreach (System.Windows.Forms.DataVisualization.Charting.Series series in chart_bzhjz.Series)        //清空
                {
                    series.Points.Clear();
                }

                System.Windows.Forms.DataVisualization.Charting.DataPoint bzhjz_point = null;
                //public static extern int GetPrivateProfileString(string strAppName, string strKeyName, string strDefault, StringBuilder sbReturnString, int nSize, string strFileName);//读配置文件 string（段名，字段，默认值，保存的strbuilder，大小，路径）
                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (80.5).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");  //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(80.5, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (78.8).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(78.8, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (77.2).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(77.2, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (75.6).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(75.6, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (74.1).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(74.1, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (72.4).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(72.4, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (70.8).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(70.8, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (69.2).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(69.2, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (67.6).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(67.6, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (66.1).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(66.1, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (64.4).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(64.4, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (62.8).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(62.8, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (61.1).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(61.1, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (59.5).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(59.5, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (57.9).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(57.9, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (56.3).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(56.3, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (54.7).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(54.7, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (53.1).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(53.1, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (51.5).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(51.5, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (49.9).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(49.9, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (48.3).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(48.3, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (46.7).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(46.7, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (45.1).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(45.1, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (43.4).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(43.4, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (41.8).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(41.8, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (40.2).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(40.2, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (38.6).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(38.6, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (37.1).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(37.1, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (35.4).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(35.4, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (33.8).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(33.8, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (32.2).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(32.2, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (30.6).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(30.6, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (29.1).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(29.1, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (27.4).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(27.4, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (25.7).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(25.7, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (24.1).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(24.1, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (22.5).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(22.5, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (20.9).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(20.9, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (19.3).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(19.3, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (17.7).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(17.7, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (16.1).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(16.1, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (14.5).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(14.5, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (12.9).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(11.8, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (11.3).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(12.9, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                ini.INIIO.GetPrivateProfileString("变载荷加载(核准)", (9.7).ToString() + "km/h", "Empty", temp, 2048, @".\Config.ini");
                bzhjz_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(9.7, temp.ToString());
                chart_bzhjz.Series["bzhjz_qx"].Points.Add(bzhjz_point);

                if (temp.ToString() == "Empty")
                {
                    MessageBox.Show("没有相关数据", "出错啦");
                    return;
                }
                chart_bzhjz.Visible = true;
                button_xsqx.Text = "隐藏曲线";
            }

        }
        #endregion()

        #region 加载误差测试(Test Complete1)
        /// <summary>
        /// 初始化
        /// </summary>
        private void tabPage_9_Enter(object sender, EventArgs e)
        {
            if (!Test_Flag)                               //判定是否有测试正在进行
            {
                if (page != tabPage_9)
                {
                    Jzwc_Init();
                }
            }
            page = tabPage_9;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Jzwc_Init()
        {
            dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add("编号");
            dt.Columns.Add("速度区间Km/h");
            dt.Columns.Add("名义速度Km/h");
            dt.Columns.Add("IHP(Kw)");
            dt.Columns.Add("PLHP(Kw)");
            dt.Columns.Add("CCDT(s)");
            dt.Columns.Add("ACDT(s)");
            dt.Columns.Add("误差");
            dt.Columns.Add("结果");
            for (int i = 0; i < 12; i++)
            {
                dr = dt.NewRow();
                dr["编号"] = (i + 1).ToString();
                dr["速度区间Km/h"] = "48 - 24";
                dr["名义速度Km/h"] = "36";
                dt.Rows.Add(dr);
            }
            dataGrid_jzwc.DataSource = dt;
        }

        /// <summary>
        /// 加载功率（IHP）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_IHP_TextChanged(object sender, EventArgs e)
        {
            Match m = Regex.Match(this.textBox_IHP.Text, pattern);
            if (!m.Success)
            {
                MessageBox.Show("请输入数字！", "警告!!!");
                textBox_IHP.Text = "";
                return;
            }
        }

        //测试按钮
        private void button_jzwc_Click(object sender, EventArgs e)
        {
            if (radioButton_check.Checked)
            {
                if (!Test_Flag)                                           //判断是不是有测试正在进行
                {
                    th_jzwc = new Thread(Jzwc_exe);                        //新建线程  
                    th_jzwc.Start();                                      //线程开始执行
                    testFinish = false;
                    nowState = testState.jzwc_state;
                    timer_flag = false;
                    Test_Flag = true;                       //正在测试的标记置为true 表示有测试正在进行
                    tabControl1.Enabled = false;                          //设置不能在页面中切换
                    button_jzwc.Text = "重新开始";
                }
                else
                    MessageBox.Show("有测试正在进行，无法开始测试", "系统提示");
            }
            else
            {
                MessageBox.Show("加载误差测试是核准测试内容，请在“设置”页面的“测试类型设置”中选择“核准测试”", "系统提示");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                StreamReader sr = File.OpenText(File_Name7);
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
        public void Jzwc_exe()
        {
            //初始化
            double t1 = 0, t2 = 0, t3 = 0, t4 = 0, t5 = 0, t6 = 0, t7 = 0, t8 = 0, t9 = 0, t10 = 0, t11 = 0, t12 = 0; ; ;                 //实际时间
            double CCDT1 = 0, CCDT2 = 0, CCDT3 = 0, CCDT4 = 0, CCDT5 = 0, CCDT6 = 0, CCDT7 = 0, CCDT8 = 0, CCDT9 = 0, CCDT10 = 0, CCDT11 = 0, CCDT12 = 0;              //计算时间 
            double PLHP1 = 0, PLHP2 = 0, PLHP3 = 0, PLHP4 = 0, PLHP5 = 0, PLHP6 = 0, PLHP7 = 0, PLHP8 = 0, PLHP9 = 0, PLHP10 = 0, PLHP11 = 0, PLHP12 = 0;              //寄生功率
            double result1 = 0, result2 = 0, result3 = 0, result4 = 0, result5 = 0, result6 = 0, result7 = 0, result8 = 0, result9 = 0, result10 = 0, result11 = 0, result12 = 0;            //误差
            string r1 = "";

            //启动仪器
             Lifter_Down();         //下降举升
            Thread.Sleep(2000);         //等待2秒
             Motor_Open();          //启动电机加速
             while (testFinish == false)//等待测试完成
             {
                 Thread.Sleep(100);
             }
             Exit_Control();

            while (true)                //等待达到预定速度
            {
                if (Speed > 56.3)
                    break;
            }
             Motor_Close();
            while (true)
            {
                if (dataGrid_jzwc.CurrentCell.RowIndex == 0)
                {
                    if (Speed <= 48 && Speed >= 24)
                    {
                         Set_Control_Power(float.Parse(textBox_IHP.Text));
                        t1 += 0.1;
                    }
                    if (Speed < 20)                                       //区间记录完成
                    {
                        break;
                    }
                     Start_Control_Power();                           //启动功率控制
                    Thread.Sleep(100);
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 1)
                {
                    if (Speed <= 48 && Speed >= 24)
                    {
                         Set_Control_Power(float.Parse(textBox_IHP.Text));
                        t2 += 0.1;
                    }
                    if (Speed < 20)                                       //区间记录完成
                    {
                        break;
                    }
                     Start_Control_Power();                           //启动功率控制
                    Thread.Sleep(100);
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 2)
                {
                    if (Speed <= 48 && Speed >= 24)
                    {
                         Set_Control_Power(float.Parse(textBox_IHP.Text));
                        t3 += 0.1;
                    }
                    if (Speed < 20)                                       //区间记录完成
                    {
                        break;
                    }
                     Start_Control_Power();                           //启动功率控制
                    Thread.Sleep(100);
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 3)
                {
                    if (Speed <= 48 && Speed >= 24)
                    {
                         Set_Control_Power(float.Parse(textBox_IHP.Text));
                        t4 += 0.1;
                    }
                    if (Speed < 20)                                       //区间记录完成
                    {
                        break;
                    }
                     Start_Control_Power();                           //启动功率控制
                    Thread.Sleep(100);
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 4)
                {
                    if (Speed <= 48 && Speed >= 24)
                    {
                         Set_Control_Power(float.Parse(textBox_IHP.Text));
                        t5 += 0.1;
                    }
                    if (Speed < 20)                                       //区间记录完成
                    {
                        break;
                    }
                     Start_Control_Power();                           //启动功率控制
                    Thread.Sleep(100);
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 5)
                {
                    if (Speed <= 48 && Speed >= 24)
                    {
                         Set_Control_Power(float.Parse(textBox_IHP.Text));
                        t6 += 0.1;
                    }
                    if (Speed < 20)                                       //区间记录完成
                    {
                        break;
                    }
                     Start_Control_Power();                           //启动功率控制
                    Thread.Sleep(100);
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 6)
                {
                    if (Speed <= 48 && Speed >= 24)
                    {
                         Set_Control_Power(float.Parse(textBox_IHP.Text));
                        t7 += 0.1;
                    }
                    if (Speed < 20)                                       //区间记录完成
                    {
                        break;
                    }
                     Start_Control_Power();                           //启动功率控制
                    Thread.Sleep(100);
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 7)
                {
                    if (Speed <= 48 && Speed >= 24)
                    {
                         Set_Control_Power(float.Parse(textBox_IHP.Text));
                        t8 += 0.1;
                    }
                    if (Speed < 20)                                       //区间记录完成
                    {
                        break;
                    }
                     Start_Control_Power();                           //启动功率控制
                    Thread.Sleep(100);
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 8)
                {
                    if (Speed <= 48 && Speed >= 24)
                    {
                         Set_Control_Power(float.Parse(textBox_IHP.Text));
                        t9 += 0.1;
                    }
                    if (Speed < 20)                                       //区间记录完成
                    {
                        break;
                    }
                     Start_Control_Power();                           //启动功率控制
                    Thread.Sleep(100);
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 9)
                {
                    if (Speed <= 48 && Speed >= 24)
                    {
                         Set_Control_Power(float.Parse(textBox_IHP.Text));
                        t10 += 0.1;
                    }
                    if (Speed < 20)                                       //区间记录完成
                    {
                        break;
                    }
                     Start_Control_Power();                           //启动功率控制
                    Thread.Sleep(100);
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 10)
                {
                    if (Speed <= 48 && Speed >= 24)
                    {
                         Set_Control_Power(float.Parse(textBox_IHP.Text));
                        t11 += 0.1;
                    }
                    if (Speed < 20)                                       //区间记录完成
                    {
                        break;
                    }
                     Start_Control_Power();                           //启动功率控制
                    Thread.Sleep(100);
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 11)
                {
                    if (Speed <= 48 && Speed >= 24)
                    {
                         Set_Control_Power(float.Parse(textBox_IHP.Text));
                        t12 += 0.1;
                    }
                    if (Speed < 20)                                       //区间记录完成
                    {
                        break;
                    }
                     Start_Control_Power();                           //启动功率控制
                    Thread.Sleep(100);
                }
            }
            PLHP1 = Math.Round(0.00061728 * 36 * DIW / t1, 1);
            PLHP2 = Math.Round(0.00061728 * 36 * DIW / t2, 1);
            PLHP3 = Math.Round(0.00061728 * 36 * DIW / t3, 1);
            PLHP4 = Math.Round(0.00061728 * 36 * DIW / t4, 1);
            PLHP5 = Math.Round(0.00061728 * 36 * DIW / t5, 1);
            PLHP6 = Math.Round(0.00061728 * 36 * DIW / t6, 1);
            PLHP7 = Math.Round(0.00061728 * 36 * DIW / t7, 1);
            PLHP8 = Math.Round(0.00061728 * 36 * DIW / t8, 1);
            PLHP9 = Math.Round(0.00061728 * 36 * DIW / t9, 1);
            PLHP10 = Math.Round(0.00061728 * 36 * DIW / t10, 1);
            PLHP11 = Math.Round(0.00061728 * 36 * DIW / t11, 1);
            PLHP12 = Math.Round(0.00061728 * 36 * DIW / t12, 1);
            //计算
            string f = textBox_IHP.Text.Trim();
            try
            {
                if (dataGrid_jzwc.CurrentCell.RowIndex == 0)
                {
                    CCDT1 = (Math.Round((DIW * 1728) / (2000 * (int.Parse(f) + PLHP1)), 1)) / 10; //计算时间
                    result1 = Math.Abs(CCDT1 - t1) / CCDT1;
                    result1 = Math.Round(result1, 2);
                    //条件
                    if (int.Parse(f) == 4)
                    {
                        if (result1 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 18)
                    {
                        if (result1 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 11)
                    {
                        if (result1 <= 0.02)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    dataGrid_jzwc.Rows[0].Cells["IHP(Kw)"].Value = textBox_IHP.Text.ToString();
                    dataGrid_jzwc.Rows[0].Cells["PLHP(Kw)"].Value = PLHP1.ToString();
                    dataGrid_jzwc.Rows[0].Cells["CCDT(s)"].Value = CCDT1.ToString();
                    dataGrid_jzwc.Rows[0].Cells["ACDT(s)"].Value = t1.ToString();
                    dataGrid_jzwc.Rows[0].Cells["误差"].Value = result1.ToString();
                    dataGrid_jzwc.Rows[0].Cells["结果"].Value = r1.ToString();
                }

                if (dataGrid_jzwc.CurrentCell.RowIndex == 1)
                {
                    CCDT2 = (Math.Round((DIW * 1728) / (2000 * (int.Parse(f) + PLHP2)), 1)) / 10; //计算时间
                    result2 = Math.Abs(CCDT2 - t2) / CCDT2;
                    result2 = Math.Round(result2, 2);
                    //条件
                    if (int.Parse(f) == 4)
                    {
                        if (result2 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 18)
                    {
                        if (result2 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 11)
                    {
                        if (result2 <= 0.02)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    dataGrid_jzwc.Rows[1].Cells["IHP(Kw)"].Value = textBox_IHP.Text.ToString();
                    dataGrid_jzwc.Rows[1].Cells["PLHP(Kw)"].Value = PLHP2.ToString();
                    dataGrid_jzwc.Rows[1].Cells["CCDT(s)"].Value = CCDT2.ToString();
                    dataGrid_jzwc.Rows[1].Cells["ACDT(s)"].Value = t2.ToString();
                    dataGrid_jzwc.Rows[1].Cells["误差"].Value = result2.ToString();
                    dataGrid_jzwc.Rows[1].Cells["结果"].Value = r1.ToString();
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 2)
                {
                    CCDT3 = (Math.Round((DIW * 1728) / (2000 * (int.Parse(f) + PLHP3)), 1)) / 10; //计算时间
                    result3 = Math.Abs(CCDT3 - t3) / CCDT3;
                    result3 = Math.Round(result3, 2);
                    //条件
                    if (int.Parse(f) == 4)
                    {
                        if (result3 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 18)
                    {
                        if (result3 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 11)
                    {
                        if (result3 <= 0.02)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    dataGrid_jzwc.Rows[2].Cells["IHP(Kw)"].Value = textBox_IHP.Text.ToString();
                    dataGrid_jzwc.Rows[2].Cells["PLHP(Kw)"].Value = PLHP3.ToString();
                    dataGrid_jzwc.Rows[2].Cells["CCDT(s)"].Value = CCDT3.ToString();
                    dataGrid_jzwc.Rows[2].Cells["ACDT(s)"].Value = t3.ToString();
                    dataGrid_jzwc.Rows[2].Cells["误差"].Value = result3.ToString();
                    dataGrid_jzwc.Rows[2].Cells["结果"].Value = r1.ToString();
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 3)
                {
                    CCDT4 = (Math.Round((DIW * 1728) / (2000 * (int.Parse(f) + PLHP4)), 1)) / 10; //计算时间
                    result4 = Math.Abs(CCDT4 - t4) / CCDT4;
                    result4 = Math.Round(result4, 2);
                    //条件
                    if (int.Parse(f) == 4)
                    {
                        if (result4 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 18)
                    {
                        if (result4 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 11)
                    {
                        if (result4 <= 0.02)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    dataGrid_jzwc.Rows[3].Cells["IHP(Kw)"].Value = textBox_IHP.Text.ToString();
                    dataGrid_jzwc.Rows[3].Cells["PLHP(Kw)"].Value = PLHP4.ToString();
                    dataGrid_jzwc.Rows[3].Cells["CCDT(s)"].Value = CCDT4.ToString();
                    dataGrid_jzwc.Rows[3].Cells["ACDT(s)"].Value = t4.ToString();
                    dataGrid_jzwc.Rows[3].Cells["误差"].Value = result4.ToString();
                    dataGrid_jzwc.Rows[3].Cells["结果"].Value = r1.ToString();
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 4)
                {
                    CCDT5 = (Math.Round((DIW * 1728) / (2000 * (int.Parse(f) + PLHP5)), 1)) / 10; //计算时间
                    result5 = Math.Abs(CCDT5 - t5) / CCDT5;
                    result5 = Math.Round(result5, 2);
                    //条件
                    if (int.Parse(f) == 4)
                    {
                        if (result5 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 18)
                    {
                        if (result5 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 11)
                    {
                        if (result5 <= 0.02)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    dataGrid_jzwc.Rows[4].Cells["IHP(Kw)"].Value = textBox_IHP.Text.ToString();
                    dataGrid_jzwc.Rows[4].Cells["PLHP(Kw)"].Value = PLHP5.ToString();
                    dataGrid_jzwc.Rows[4].Cells["CCDT(s)"].Value = CCDT5.ToString();
                    dataGrid_jzwc.Rows[4].Cells["ACDT(s)"].Value = t5.ToString();
                    dataGrid_jzwc.Rows[4].Cells["误差"].Value = result5.ToString();
                    dataGrid_jzwc.Rows[4].Cells["结果"].Value = r1.ToString();
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 5)
                {
                    CCDT6 = (Math.Round((DIW * 1728) / (2000 * (int.Parse(f) + PLHP6)), 1)) / 10; //计算时间
                    result6 = Math.Abs(CCDT6 - t6) / CCDT6;
                    result6 = Math.Round(result6, 2);
                    //条件
                    if (int.Parse(f) == 4)
                    {
                        if (result6 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 18)
                    {
                        if (result6 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 11)
                    {
                        if (result6 <= 0.02)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    dataGrid_jzwc.Rows[5].Cells["IHP(Kw)"].Value = textBox_IHP.Text.ToString();
                    dataGrid_jzwc.Rows[5].Cells["PLHP(Kw)"].Value = PLHP6.ToString();
                    dataGrid_jzwc.Rows[5].Cells["CCDT(s)"].Value = CCDT6.ToString();
                    dataGrid_jzwc.Rows[5].Cells["ACDT(s)"].Value = t6.ToString();
                    dataGrid_jzwc.Rows[5].Cells["误差"].Value = result6.ToString();
                    dataGrid_jzwc.Rows[5].Cells["结果"].Value = r1.ToString();
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 6)
                {
                    CCDT7 = (Math.Round((DIW * 1728) / (2000 * (int.Parse(f) + PLHP7)), 1)) / 10; //计算时间
                    result7 = Math.Abs(CCDT7 - t7) / CCDT7;
                    result7 = Math.Round(result7, 2);
                    //条件
                    if (int.Parse(f) == 4)
                    {
                        if (result7 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 18)
                    {
                        if (result7 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 11)
                    {
                        if (result7 <= 0.02)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    dataGrid_jzwc.Rows[6].Cells["IHP(Kw)"].Value = textBox_IHP.Text.ToString();
                    dataGrid_jzwc.Rows[6].Cells["PLHP(Kw)"].Value = PLHP7.ToString();
                    dataGrid_jzwc.Rows[6].Cells["CCDT(s)"].Value = CCDT7.ToString();
                    dataGrid_jzwc.Rows[6].Cells["ACDT(s)"].Value = t7.ToString();
                    dataGrid_jzwc.Rows[6].Cells["误差"].Value = result7.ToString();
                    dataGrid_jzwc.Rows[6].Cells["结果"].Value = r1.ToString();
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 7)
                {
                    CCDT8 = (Math.Round((DIW * 1728) / (2000 * (int.Parse(f) + PLHP8)), 1)) / 10; //计算时间
                    result8 = Math.Abs(CCDT8 - t8) / CCDT8;
                    result8 = Math.Round(result8, 2);
                    //条件
                    if (int.Parse(f) == 4)
                    {
                        if (result8 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 18)
                    {
                        if (result8 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 11)
                    {
                        if (result8 <= 0.02)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    dataGrid_jzwc.Rows[7].Cells["IHP(Kw)"].Value = textBox_IHP.Text.ToString();
                    dataGrid_jzwc.Rows[7].Cells["PLHP(Kw)"].Value = PLHP8.ToString();
                    dataGrid_jzwc.Rows[7].Cells["CCDT(s)"].Value = CCDT8.ToString();
                    dataGrid_jzwc.Rows[7].Cells["ACDT(s)"].Value = t8.ToString();
                    dataGrid_jzwc.Rows[7].Cells["误差"].Value = result8.ToString();
                    dataGrid_jzwc.Rows[7].Cells["结果"].Value = r1.ToString();
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 8)
                {
                    CCDT9 = (Math.Round((DIW * 1728) / (2000 * (int.Parse(f) + PLHP9)), 1)) / 10; //计算时间
                    result9 = Math.Abs(CCDT9 - t9) / CCDT9;
                    result9 = Math.Round(result9, 2);
                    //条件
                    if (int.Parse(f) == 4)
                    {
                        if (result9 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 18)
                    {
                        if (result9 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 11)
                    {
                        if (result9 <= 0.02)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    dataGrid_jzwc.Rows[8].Cells["IHP(Kw)"].Value = textBox_IHP.Text.ToString();
                    dataGrid_jzwc.Rows[8].Cells["PLHP(Kw)"].Value = PLHP9.ToString();
                    dataGrid_jzwc.Rows[8].Cells["CCDT(s)"].Value = CCDT9.ToString();
                    dataGrid_jzwc.Rows[8].Cells["ACDT(s)"].Value = t9.ToString();
                    dataGrid_jzwc.Rows[8].Cells["误差"].Value = result9.ToString();
                    dataGrid_jzwc.Rows[8].Cells["结果"].Value = r1.ToString();
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 9)
                {
                    CCDT10 = (Math.Round((DIW * 1728) / (2000 * (int.Parse(f) + PLHP10)), 1)) / 10; //计算时间
                    result10 = Math.Abs(CCDT10 - t10) / CCDT10;
                    result10 = Math.Round(result10, 2);
                    //条件
                    if (int.Parse(f) == 4)
                    {
                        if (result10 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 18)
                    {
                        if (result10 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 11)
                    {
                        if (result10 <= 0.02)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    dataGrid_jzwc.Rows[9].Cells["IHP(Kw)"].Value = textBox_IHP.Text.ToString();
                    dataGrid_jzwc.Rows[9].Cells["PLHP(Kw)"].Value = PLHP10.ToString();
                    dataGrid_jzwc.Rows[9].Cells["CCDT(s)"].Value = CCDT10.ToString();
                    dataGrid_jzwc.Rows[9].Cells["ACDT(s)"].Value = t10.ToString();
                    dataGrid_jzwc.Rows[9].Cells["误差"].Value = result10.ToString();
                    dataGrid_jzwc.Rows[9].Cells["结果"].Value = r1.ToString();
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 10)
                {
                    CCDT11 = (Math.Round((DIW * 1728) / (2000 * (int.Parse(f) + PLHP11)), 1)) / 10; //计算时间
                    result11 = Math.Abs(CCDT11 - t11) / CCDT11;
                    result11 = Math.Round(result11, 2);
                    //条件
                    if (int.Parse(f) == 4)
                    {
                        if (result11 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 18)
                    {
                        if (result11 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 11)
                    {
                        if (result11 <= 0.02)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    dataGrid_jzwc.Rows[10].Cells["IHP(Kw)"].Value = textBox_IHP.Text.ToString();
                    dataGrid_jzwc.Rows[10].Cells["PLHP(Kw)"].Value = PLHP11.ToString();
                    dataGrid_jzwc.Rows[10].Cells["CCDT(s)"].Value = CCDT11.ToString();
                    dataGrid_jzwc.Rows[10].Cells["ACDT(s)"].Value = t11.ToString();
                    dataGrid_jzwc.Rows[10].Cells["误差"].Value = result11.ToString();
                    dataGrid_jzwc.Rows[10].Cells["结果"].Value = r1.ToString();
                }
                if (dataGrid_jzwc.CurrentCell.RowIndex == 11)
                {
                    CCDT12 = (Math.Round((DIW * 1728) / (2000 * (int.Parse(f) + PLHP12)), 1)) / 10; //计算时间
                    result12 = Math.Abs(CCDT12 - t12) / CCDT12;
                    result12 = Math.Round(result12, 2);
                    //条件
                    if (int.Parse(f) == 4)
                    {
                        if (result12 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 18)
                    {
                        if (result12 <= 0.04)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    else if (int.Parse(f) == 11)
                    {
                        if (result12 <= 0.02)
                        {
                            r1 = "合格";
                        }
                        else
                        {
                            r1 = "不合格";
                        }
                    }
                    dataGrid_jzwc.Rows[11].Cells["IHP(Kw)"].Value = textBox_IHP.Text.ToString();
                    dataGrid_jzwc.Rows[11].Cells["PLHP(Kw)"].Value = PLHP12.ToString();
                    dataGrid_jzwc.Rows[11].Cells["CCDT(s)"].Value = CCDT12.ToString();
                    dataGrid_jzwc.Rows[11].Cells["ACDT(s)"].Value = t12.ToString();
                    dataGrid_jzwc.Rows[11].Cells["误差"].Value = result12.ToString();
                    dataGrid_jzwc.Rows[11].Cells["结果"].Value = r1.ToString();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "出错");
            }
            //等待电机停止
            Thread.Sleep(3000);         //等待滚筒停止
             Lifter_Up();           //测试完成举升上升
            //tabControl1.Enabled = true; //设置能在页面中切换
            this.BeginInvoke(new wtb(Set_tab_Enabled), true);
            Test_Flag = false;          //重置正在测试的标记 
        }
        #endregion

        
        #region 标定
        private void tabPage_11_Enter(object sender, EventArgs e)
        {
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            button_read_speed_dp.Enabled = true;
            label3.Text = "电压值";
            panel14.Enabled = true;
                    //panel13.Enabled = false;
            dt_bd = null;
            dt_bd = new DataTable();
            dt_bd.Columns.Add("电压值(V)");
            dt_bd.Columns.Add("标定点(N)");
            dt_bd.Columns.Add("实测重量(N)");
            dataGridView_bd.DataSource = dt_bd;
            chartForce.Series["force_qx"].Points.Clear();//清除曲线
            chartForce.Series["voltage_qx"].Points.Clear();//清除曲线
            comboBox_bdtd.SelectedIndex = 0;
            ini.INIIO.GetPrivateProfileString("UseMK", "使用通道", "1", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
            switch (temp.ToString())
            {
                        case "1":
                            comboBox_bdtd.SelectedIndex = 0;
                            break;
                        case "2":
                            comboBox_bdtd.SelectedIndex = 1;
                            break;
                        case "3":
                            comboBox_bdtd.SelectedIndex = 2;
                            break;
            }
            ini.INIIO.GetPrivateProfileString("UseMK", "BNTD标定说明", "无", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
            richTextBox_czts.Text = Regex.Replace(temp.ToString(), @"\\n", me);
            Exit_Control();
            Thread.Sleep(5);
            isForceDemarcate = true;
        }

       /*private void button_demarcate_force_Click(object sender, EventArgs e)
        {
            string errorlist = "";
            if (textBox_Weight.Text.Trim() == "")
                errorlist += "请输入法码质量 ";
            if (textBox_modulus.Text.Trim() == "")
                errorlist += "请输入力臂比 ";
            if (errorlist == "")
            {
                try
                {
                    float.Parse(textBox_Weight.Text.Trim());
                }
                catch (Exception)
                {
                    errorlist += "输入的法码质量不合法 ";
                }
                try
                {
                    float.Parse(textBox_modulus.Text.Trim());
                }
                catch (Exception)
                {
                    errorlist += "输入的力臂比不合法 ";
                }
            }
            if (errorlist == "")
            {
                 Demarcate_Force(comboBox_point.SelectedIndex, int.Parse((float.Parse(textBox_Weight.Text.Trim()) * float.Parse(textBox_modulus.Text.Trim()) * 9.8f).ToString()));
                if (comboBox_point.SelectedIndex < comboBox_point.Items.Count - 1)
                    comboBox_point.SelectedIndex += 1;
            }
            else
                MessageBox.Show(errorlist, "出错啦");
        }*/

        private void button_demarcate_speed_Click(object sender, EventArgs e)
        {
            
        }

        /*private void Demarcate_Load(object sender, EventArgs e)
        {
            comboBox_point.Text = "第一点";
            //timer1.Interval = 100;
        }*/

        private void tabPage_11_Leave(object sender, EventArgs e)
        {
            isForceDemarcate = false;
            Exit_Demarcate_Force();
            nowState = testState.idle_state;
        }
        #endregion

        #region 控制
        /// <summary>
        /// 设置是否可以在页面中切换
        /// </summary>
        /// <param name="tf">bool</param>
        public void Set_tab_Enabled(bool tf)
        {
            tabControl1.Enabled = tf;         //设置能在页面中切换
        }

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
                tabControl1.Enabled = true;            //设置可以在页面中切换
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

        private void button_qddj_Click(object sender, EventArgs e)
        {
             Motor_Open();
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
            g.CopyFromScreen(Location, new Point(this.Left, sc_height-this.Bottom), s);
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

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (textBox_cs.Text.Length < 4)
                {

                    bool IsContaintDot = this.textBox_cs.Text.Contains(".");
                    if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
                    {
                        e.Handled = true;
                    }
                    else if (IsContaintDot && (e.KeyChar == 46))
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    MessageBox.Show("为了测试准确，输入的数字请不要大于1000", "系统提示");
                    textBox_cs.Text = "";
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("为了测试准确，输入的数字请不要大于100", "系统提示");
                textBox_cs.Text = "";
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Speed = (float)Convert.ToDouble( Speed);
            //meter1.ChangeValue = Speed;
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

        private void buttonSbAisle_Click(object sender, EventArgs e)
        {
            try
            {
                Read_Force_Chanel();
                Thread.Sleep(200);
                if(forceChanelByte>0&&forceChanelByte<4)
                {
                    comboBox_bdtd.SelectedIndex=forceChanelByte-1;
                }
                else
                {
                    MessageBox.Show("读取设备通道出错或者该设备还未进行过力标定","系统提示");
                }
            }
            catch (Exception)
            {
            }
        }
        private void button_addbdd_Click(object sender, EventArgs e)
        {
            try 
	        {	        
		        float.Parse(textBox_bdd.Text.Trim());
                if (comboBox_bdtd.Text == "")
                {
                    MessageBox.Show("请选择标定通道！", "系统提示");
                    return;
                }
                DataRow dr=dt_bd.NewRow();
                dr["电压值(V)"] = label_ad.Text;
                dr["标定点(N)"] = textBox_bdd.Text.Trim();
                dr["实测重量(N)"] = label_yzl.Text;
                dt_bd.Rows.Add(dr);
                dataGridView_bd.DataSource = dt_bd;
                System.Windows.Forms.DataVisualization.Charting.DataPoint jsg_point = null;
                jsg_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(double.Parse(textBox_bdd.Text.Trim()), label_yzl.Text);
                chartForce.Series["force_qx"].Points.Add(jsg_point);
                jsg_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(double.Parse(textBox_bdd.Text.Trim()), label_ad.Text);
                chartForce.Series["voltage_qx"].Points.Add(jsg_point);
	        }
	        catch (Exception)
	        {
		        MessageBox.Show("标定点数据有误，请检查！","系统提示");
	        }
            
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            Text_Verification(textBox_bdd, "[0-9\\.]", 12, true);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            dt_bd = null;
            dt_bd = new DataTable();
            dt_bd.Columns.Add("电压值(V)");
            dt_bd.Columns.Add("标定点(N)");
            dt_bd.Columns.Add("实测重量(N)");
            dataGridView_bd.DataSource = dt_bd;
            chartForce.Series["force_qx"].Points.Clear();//清除曲线
            chartForce.Series["voltage_qx"].Points.Clear();//清除曲线
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            string Aisle = "通道1";
            switch (comboBox_bdtd.Text)
            {
                case "1":
                    Aisle = "通道1";
                    ini.INIIO.WritePrivateProfileString("UseMK", "使用通道", "1", @".\Config.ini");
                    Select_Channel(1);
                    break;
                case "2":
                    Aisle = "通道2";
                    ini.INIIO.WritePrivateProfileString("UseMK", "使用通道", "2", @".\Config.ini");
                    Select_Channel(2);
                    break;
                case "3":
                    Aisle = "通道3";
                     ini.INIIO.WritePrivateProfileString("UseMK", "使用通道", "3", @".\Config.ini");
                    Select_Channel(3);
                    break;
            }
            ini.INIIO.GetPrivateProfileString("UseMK", Aisle, "0.0", temp, 2048, @".\Config.ini");    //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
            textBox_xs.Text = temp.ToString();
            
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<DataRow> drlist = new List<DataRow>();
                if (dataGridView_bd.SelectedRows.Count > 0)
                {
                    for (int i = 0; i < dataGridView_bd.SelectedRows.Count; i++)
                        foreach (DataRow dr in dt_bd.Rows)
                        {
                            if (dr["标定点(N)"].ToString() == dataGridView_bd.SelectedRows[i].Cells["标定点(N)"].Value.ToString())
                                drlist.Add(dr); 
                        }
                    if (drlist.Count != 0)
                        foreach (DataRow drd in drlist)
                        {
                            
                            System.Windows.Forms.DataVisualization.Charting.DataPoint jsg_point = null;
                            jsg_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(double.Parse(drd["标定点(N)"].ToString()), drd["实测重量(N)"].ToString());
                            //chartForce.Series["force_qx"].Points.Remove(jsg_point);
                            foreach (System.Windows.Forms.DataVisualization.Charting.DataPoint da in chartForce.Series["force_qx"].Points)
                            {
                                if (da.XValue == jsg_point.XValue)
                                {
                                    chartForce.Series["force_qx"].Points.Remove(da);
                                    break;
                                }
                            }
                            jsg_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(double.Parse(drd["标定点(N)"].ToString()), drd["电压值(V)"].ToString());
                            //chartForce.Series["voltage_qx"].Points.Remove(jsg_point);
                            foreach (System.Windows.Forms.DataVisualization.Charting.DataPoint da in chartForce.Series["voltage_qx"].Points)
                            {
                                if (da.XValue == jsg_point.XValue)
                                {
                                    chartForce.Series["voltage_qx"].Points.Remove(da);
                                    break;
                                }
                            }
                            dt_bd.Rows.Remove(drd);
                        }
                    dataGridView_bd.DataSource = dt_bd;
                }
                else 
                {
                    MessageBox.Show("请选择要删除的点", "系统提示");
                }
            }
            catch (Exception)
            {
            }
        }

        private void button_scxs_Click(object sender, EventArgs e)
        {
            try
            {
                double v = 0;
                double d = 0;
                for (int i = 0; i < dataGridView_bd.Rows.Count; i++)
                {
                    v += float.Parse(dataGridView_bd.Rows[i].Cells["电压值(V)"].Value.ToString());
                    d += float.Parse(dataGridView_bd.Rows[i].Cells["标定点(N)"].Value.ToString());
                }
                textBox_xs.Text = float.Parse((d / v).ToString()).ToString();
            }
            catch (Exception)
            {
                dt_bd.Clear();
                dataGridView_bd.DataSource = dt_bd;
                MessageBox.Show("标定点可能有误,请重新操作。", "系统提示");
            }
        }

        private void button_write_Click(object sender, EventArgs e)
        {
            try
            {
                float xs=float.Parse(textBox_xs.Text);
                byte td= byte.Parse(comboBox_bdtd.Text);
                Solidify_Force_Modulus(td, xs);
                Solidify_Force_Chanel();
            }
            catch (Exception)
            {
                MessageBox.Show("是不是没有生成系数？或者没有选择通道？请检查！", "系统提示");
            }
        }

        private void button_read_force_m_Click(object sender, EventArgs e)
        {
            try
            {
                 Get_Force_Modulus();
                Thread.Sleep(100);
                switch (comboBox_bdtd.Text)
                {
                    case "1":
                        textBox_xs.Text = b0;
                        break;
                    case "2":
                        textBox_xs.Text = b1;
                        break;
                    case "3":
                        textBox_xs.Text = b2;
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        /*private void radioButton_td_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_td1.Checked)
            {
                ini.INIIO.WritePrivateProfileString("UseMK", "使用通道", "1", @".\Config.ini");
                 Select_Channel(1);
            }
            else if (radioButton_td2.Checked)
            {
                ini.INIIO.WritePrivateProfileString("UseMK", "使用通道", "2", @".\Config.ini");
                 Select_Channel(2);
            }
            else if (radioButton_td3.Checked)
            {
                ini.INIIO.WritePrivateProfileString("UseMK", "使用通道", "3", @".\Config.ini");
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
             Motor_Open();
        }

        private void toolStripButtonMotorOff_Click(object sender, EventArgs e)
        {
             Motor_Close();
        }

        private void toolStripButtonStopTest_Click(object sender, EventArgs e)
        {
            Motor_Close();
            Exit_Control();
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
                tabControl1.Enabled = true;            //设置可以在页面中切换
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
        public void Motor_Open()
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
        #endregion

        #region 关闭电机
        /// <summary>
        /// 关闭电机
        /// </summary>
        public void Motor_Close()
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
            try
            {
                Exit_Control();
            }
            catch
            { }
            _continue = false;
            Thread.Sleep(5);
            if(readThread!=null)
                readThread.Abort();
            if(ComPort_2!=null)
                ComPort_2.Close();
            
        }

        private void button_read_speed_dp_Click_1(object sender, EventArgs e)
        {
            Get_Speed_DiameterandPusle();
            Thread.Sleep(100);
            textBox_diameter.Text = (float.Parse(Speed_Diameter) * 10).ToString();
            textBox_pulse.Text = Speed_Pusle;
        }

        private void button_demarcate_speed_Click_1(object sender, EventArgs e)
        {
            string errorlist = "";
            if (textBox_diameter.Text.Trim() == "")
                errorlist += "请输入直径 ";
            if (textBox_pulse.Text.Trim() == "")
                errorlist += "请输入脉冲数 ";
            if (errorlist == "")
            {
                try
                {
                    int.Parse(textBox_diameter.Text.Trim());
                }
                catch (Exception)
                {
                    errorlist += "输入的直径不合法 ";
                }
                try
                {
                    int.Parse(textBox_pulse.Text.Trim());
                }
                catch (Exception)
                {
                    errorlist += "输入的脉冲数不合法 ";
                }
            }
            if (errorlist == "")
            {
                // Exit_Demarcate_Force();
                //Thread.Sleep(500);
                // Exit_Control();
                //Thread.Sleep(500);
                switch (UseMK)
                {
                    case "BNTD":
                        Demarcate_Speed((float.Parse(textBox_diameter.Text.Trim()) / 10).ToString(), textBox_pulse.Text.Trim());
                        break;
                    case "IGBT":
                        Demarcate_Speed(textBox_diameter.Text.Trim(), textBox_pulse.Text.Trim());
                        break;
                }
            }
            else
                MessageBox.Show(errorlist, "出错啦");
        }

        private void tabPage_speed_Enter(object sender, EventArgs e)
        {
            dt_speed = null;
            dt_speed = new DataTable();
            dt_speed.Columns.Add("标定速度点(Km/h)");
            dt_speed.Columns.Add("实测速度(Km/h)");
            dt_speed.Columns.Add("标准速度(Km/h)");
            dt_speed.Columns.Add("误差(Km/h)");
            dataGridViewSpeed.DataSource = dt_speed;
            chartSpeed.Series["speed_qx"].Points.Clear();//清除曲线
            chartSpeed.Series["speed_qx"].Points.Add(new System.Windows.Forms.DataVisualization.Charting.DataPoint (0,"0"));
        }

        private void buttonStartSpeed_Click(object sender, EventArgs e)
        {
            if (Test_Flag == false)
            {
                if (th_speedDermarcate == false)//如果未执行
                {
                    if (radioButtonKeyboard.Checked == true)
                    {
                        th_speedDermarcate = true;
                        buttonStartSpeed.Text = "停止";
                        groupBox12.Enabled = false;
                    }
                    else if (radioButtonSerial.Checked == true)
                    {
                        //串口控制变频器
                        th_speedDermarcate = true;
                        buttonStartSpeed.Text = "停止";
                        groupBox12.Enabled = false;
                    }
                    else if (radioButtonCar.Checked == true)
                    {
                        try
                        {
                            float control_speed = float.Parse(textBoxMubiaoSpeed.Text.Trim());
                            Set_Speed(control_speed/0.9925f);
                            Start_Control_Speed();//开启速度控制
                            th_speedDermarcate = true;
                            buttonStartSpeed.Text = "停止";
                            groupBox12.Enabled = false;
                        }
                        catch
                        {
                            MessageBox.Show("目标速度输入不合法.", "输入错误!");
                        }
                    }
                }
                else
                {
                    if (radioButtonKeyboard.Checked == true)
                    {
                        th_speedDermarcate = false;
                        buttonStartSpeed.Text = "执行";
                        groupBox12.Enabled = true;
                    }
                    else if (radioButtonSerial.Checked == true)
                    {
                        //串口控制变频器
                        th_speedDermarcate = false;
                        buttonStartSpeed.Text = "执行";
                        groupBox12.Enabled = true;
                    }
                    else if (radioButtonCar.Checked == true)
                    {
                        Exit_Control();
                        th_speedDermarcate = false;
                        buttonStartSpeed.Text = "执行";
                        groupBox12.Enabled = true;

                    }

                }
            }
            else 
            {
                MessageBox.Show("有测试正在进行，无法开始测试", "系统提示");
            }
        }

        private void tabPage_speed_Leave(object sender, EventArgs e)
        {
            if (radioButtonKeyboard.Checked == true)
            {
                th_speedDermarcate = false;
                buttonStartSpeed.Text = "执行";
                groupBox12.Enabled = true;
            }
            else if (radioButtonSerial.Checked == true)
            {
                //串口控制变频器
                th_speedDermarcate = false;
                buttonStartSpeed.Text = "执行";
                groupBox12.Enabled = true;
            }
            else if (radioButtonCar.Checked == true)
            {
                Exit_Control();
                th_speedDermarcate = false;
                buttonStartSpeed.Text = "执行";
                groupBox12.Enabled = true;

            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                List<DataRow> drlist = new List<DataRow>();
                if (dataGridView_bd.SelectedRows.Count > 0)
                {
                    for (int i = 0; i < dataGridViewSpeed.SelectedRows.Count; i++)
                        foreach (DataRow dr in dt_speed.Rows)
                        {
                            if (dr["标定速度点(Km/h)"].ToString() == dataGridView_bd.SelectedRows[i].Cells["标定速度点(Km/h)"].Value.ToString())
                                drlist.Add(dr);
                        }
                    if (drlist.Count != 0)
                        foreach (DataRow drd in drlist)
                        {
                            dt_speed.Rows.Remove(drd);
                            System.Windows.Forms.DataVisualization.Charting.DataPoint jsg_point = null;
                            jsg_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(double.Parse(drd["标准速度(Km/h)"].ToString()), drd["实测速度(Km/h)"].ToString());
                            //chartSpeed.Series["speed_qx"].Points.Remove(jsg_point);
                            foreach (System.Windows.Forms.DataVisualization.Charting.DataPoint da in chartSpeed.Series["speed_qx"].Points)
                            {
                                if (da.XValue == jsg_point.XValue)
                                {
                                    chartSpeed.Series["speed_qx"].Points.Remove(da);
                                    break;
                                }
                            }
                        }
                    dataGridView_bd.DataSource = dt_speed;
                }
                else
                {
                    MessageBox.Show("请选择要删除的点", "系统提示");
                }
            }
            catch (Exception)
            {
            }
        }

        private void buttonAddSpeed_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dr = dt_speed.NewRow();
                dr["标定速度点(Km/h)"] = textBoxMubiaoSpeed.Text.Trim();
                dr["实测速度(Km/h)"] = textBoxRealSpeed.Text.Trim();
                dr["标准速度(Km/h)"] = textBoxStandardSpeed.Text.Trim();
                dr["误差(Km/h)"] =( double.Parse(textBoxRealSpeed.Text.Trim()) - double.Parse(textBoxStandardSpeed.Text.Trim())).ToString("0.0");
                dt_speed.Rows.Add(dr);
                dataGridView_bd.DataSource = dt_speed;
                System.Windows.Forms.DataVisualization.Charting.DataPoint jsg_point = null;
                jsg_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(double.Parse(textBoxStandardSpeed.Text.Trim()), textBoxRealSpeed.Text);
                chartSpeed.Series["speed_qx"].Points.Add(jsg_point);
            }
            catch
            {
                MessageBox.Show("输入速度值格式有误，请检查！", "添加失败！");
            }
        }

        private void buttonStartForce_Click(object sender, EventArgs e)
        {
            if (Test_Flag == false)
            {
                if (nowState != testState.demarcate_state)//如果未执行
                {
                    nowState = testState.demarcate_state;
                    Start_Demarcate_Force();
                    buttonStartForce.Text = "退出标定状态";
                }
                else
                {
                    nowState = testState.idle_state;
                    Exit_Demarcate_Force();
                    buttonStartForce.Text = "进入标定状态";
                }
            }
            else
            {
                MessageBox.Show("有其他测试正在进行，不能进入标定状态。", "系统提示！");
            }
        }

        private void radioButtonQx_checked(object sender, EventArgs e)
        {
            if (radioButtonQx.Checked == true)
            {
                textBox_zdgl.Text = "907.2";
            }
        }

        private void radioButtonZx_check(object sender, EventArgs e)
        {
            if (radioButtonZx.Checked == true)
            {
                textBox_zdgl.Text = "1452";
            }
        }

        private void bpqSerialSelected(object sender, EventArgs e)
        {
            if (radioButtonSerial.Checked == true)
                bpx.Init_Comm(comportBpx, comportBpxPz);
            else
                bpx.Close_Com();
        }

        private void buttonReadSpeedPid_Click(object sender, EventArgs e)
        {
            Read_SpeedPid();
            Thread.Sleep(100);
            textBoxSpeedKi.Text = (float.Parse(ki)).ToString();
            textBoxSpeedKp.Text = (float.Parse(kp)).ToString();
            textBoxSpeedKd.Text = (float.Parse(kd)).ToString();
        }

        private void buttonReadForcePid_Click(object sender, EventArgs e)
        {
            Read_ForcePid();
            Thread.Sleep(100);
            textBoxForceKi.Text = (float.Parse(ki_force)).ToString();
            textBoxForceKp.Text = (float.Parse(kp_force)).ToString();
            textBoxForceKd.Text = (float.Parse(kd_force)).ToString();
        }

        private void buttonReWriteSpeedPid_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                   ki_float= float.Parse(textBoxSpeedKi.Text.ToString());
                   kp_float = float.Parse(textBoxSpeedKp.Text.ToString());
                   kd_float = float.Parse(textBoxSpeedKd.Text.ToString());
                }
                catch (Exception)
                {
                    MessageBox.Show("参数输入格式不正确,请检查!", "系统提示");
                    return;
                }
                Set_SpeedPid(kp_float,ki_float,kd_float);
                MessageBox.Show("修改成功!", "系统提示");
            }
            catch (Exception)
            {
                
            }
        }

        private void buttonReWriteForcePid_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    ki_force_float = float.Parse(textBoxForceKi.Text.ToString());
                    kp_force_float = float.Parse(textBoxForceKp.Text.ToString());
                    kd_force_float = float.Parse(textBoxForceKd.Text.ToString());
                }
                catch (Exception)
                {
                    MessageBox.Show("参数输入格式不正确,请检查!", "系统提示");
                    return;
                }
                Set_ForcePid(ki_force_float, kp_force_float, kd_force_float);
                MessageBox.Show("修改成功!", "系统提示");
            }
            catch (Exception)
            {

            }
        }

        private void buttonPIDdefault_Click(object sender, EventArgs e)
        {
            if (radioButtonQx.Checked == true)
            {
                textBoxSpeedKp.Text = kp_float_default.ToString();
                textBoxSpeedKi.Text = ki_float_default.ToString();
                textBoxSpeedKd.Text = kd_float_default.ToString();
                textBoxForceKp.Text = kp_force_default.ToString();
                textBoxForceKi.Text = ki_force_default.ToString();
                textBoxForceKd.Text = kd_force_default.ToString();
            }
            else
            {
                textBoxSpeedKp.Text = kp_float_zx.ToString();
                textBoxSpeedKi.Text = ki_float_zx.ToString();
                textBoxSpeedKd.Text = kd_float_zx.ToString();
                textBoxForceKp.Text = kp_force_zx.ToString();
                textBoxForceKi.Text = ki_force_zx.ToString();
                textBoxForceKd.Text = kd_force_zx.ToString();
            }
        }

        private void buttonRelay1_Click(object sender, EventArgs e)
        {
            if (buttonRelay1.Text == "开继电器1")
            {
                buttonRelay1.Text = "关继电器1";
                TurnOnRelay(1);
            }
            else
            {
                buttonRelay1.Text = "开继电器1";
                TurnOffRelay(1);
            }
        }

        private void buttonRelay2_Click(object sender, EventArgs e)
        {
            if (buttonRelay2.Text == "开继电器2")
            {
                buttonRelay2.Text = "关继电器2";
                TurnOnRelay(2);
            }
            else
            {
                buttonRelay2.Text = "开继电器2";
                TurnOffRelay(2);
            }

        }

        private void buttonRelay3_Click(object sender, EventArgs e)
        {
            if (buttonRelay3.Text == "开继电器3")
            {
                buttonRelay3.Text = "关继电器3";
                TurnOnRelay(3);
            }
            else
            {
                buttonRelay3.Text = "开继电器3";
                TurnOffRelay(3);
            }
        }



            

    }
}

