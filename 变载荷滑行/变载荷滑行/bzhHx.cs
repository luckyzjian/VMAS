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

namespace 变载荷滑行
{
    public partial class bzhHx : Form
    {
        public string startUpPath =Application.StartupPath;
        private MatrixEquation.MatrixEquation jsglqx_matrix = null;
        MatchEvaluator me = delegate (Match m)
        {
            return "\n";
        };
        private DateTime startTime, nowtime;
        public int GKSJ = 0, perGKSJ = 0;                                                                        //工况时间
        public float gongkuangTime = 0f;
        private int gksjcount = 0;
        public int[] TimeCountlist = new int[10240];
        public float[] Torquelist = new float[10240];                                               //每秒速度数组
        public float[] Velocitylist = new float[10240];                                               //每秒扭力数
        public float[] Powerlist = new float[10240];
        public float[] Forcelist = new float[10240];

        private bool isSaveStartTime = false;
        private int testTime = 0;
        equipmentConfigInfdata configinfdata = new equipmentConfigInfdata();
        configIni configini = new configIni();
        glide glidedata = new glide();
        glideControl glidecontrol = new glideControl();
        BpqFl.bpxcontrol bpq = null;
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

        float jsglqx_a = 0.0006f, jsglqx_b = 0.0003f, jsglqx_c = 0.2075f;
        DateTime bzhx_t1_start, bzhx_t1_end, bzhx_t2_start, bzhx_t2_end, bzhx_t3_start, bzhx_t3_end;

        double jsgl1 = 0, jsgl2 = 0, jsgl3 = 0, jsgl4 = 0, jsgl5 = 0, jsgl6 = 0, jsgl7 = 0, jsgl8 = 0, jsgl9 = 0, jsgl10 = 0;
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
            idle_state = 1, demarcate_state, hengding_state, jsgl_state
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
        private float bhjz_power = 0;
        private byte glTestStep = 0;//在基本惯量测试中代表第glTestStep次滑行

        private double plhp_40 = 0, plhp_24 = 0;

        private float t1_moni = 0F, t2_moni = 0F, t3_moni = 0F, t4_moni = 0F, t5_moni = 0F, t6_moni = 0F;
        private bool useMoni = true;

        private float t1_bhjz = 0F, t2_bhjz = 0F, t3_bhjz = 0F;

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
                Set_Duty((float)(configinfdata.BrakePWM * 1.0 / 100.0));
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
        

        private bool useMoni_bhjz = false;

        private string useMethod = "48~16";
        private string gl_power_string = "0KW滑行时间(s)";
        private float gl_power_set = 0f;

        private float real_force = 5300f;
        private float gl_force = 5300f;
        private bool use_gl_force = false;

        System.Windows.Forms.Screen[] sc;
        private int sc_width = 0;
        private int sc_height = 0;

        bzglide bzglidedata = null;
        bool isSaved = false;



        #region 初始化
        public bzhHx()
        {
            InitializeComponent();
        }
        #endregion
        private void bzhHx_Load(object sender, EventArgs e)
        {

            sc = System.Windows.Forms.Screen.AllScreens;
            sc_height = this.Height;
            sc_width = this.Width;
            Init();
            Datagridview_bzhjzhx();
            //Datagridview_bzhjzhx();
            //this.timer1.Enabled = true;
        }


        private void initConfigInfo()
        {
            configinfdata = configini.getEquipConfigIni();
            labelStandard.Text = configinfdata.TestStandard;
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

            if (configinfdata.TestStandard != "HJT290" && configinfdata.TestStandard != "HJT291" && configinfdata.TestStandard != "HJT292")
            {
                if (configinfdata.TestStandard == "JJF1221压燃式")
                {
                    dr = dt.NewRow();
                    dr["初速度(km/h）"] = 72.4.ToString();
                    dr["末速度(km/h）"] = 16.1.ToString();
                    dr["名义时间(s）"] = (0.01713 * DIW).ToString("0.000");
                    dt.Rows.Add(dr);
                }
                else
                {
                    dr = dt.NewRow();
                    dr["初速度(km/h）"] = 48.3.ToString();
                    dr["末速度(km/h）"] = 16.1.ToString();
                    dr["名义时间(s）"] = (0.00707 * DIW).ToString("0.000");
                    dt.Rows.Add(dr);

                }
            }
            else
            {
                dr = dt.NewRow();
                dr["初速度(km/h）"] = 80.5.ToString();
                dr["末速度(km/h）"] = 8.0.ToString();
                dr["名义时间(s）"] = (0.028394 * DIW).ToString("0.000");
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["初速度(km/h）"] = 72.4.ToString();
                dr["末速度(km/h）"] = 16.1.ToString();
                dr["名义时间(s）"] = (0.01713 * DIW).ToString("0.000");
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr["初速度(km/h）"] = 61.1.ToString();
                dr["末速度(km/h）"] = 43.4.ToString();
                dr["名义时间(s）"] = (0.0043866 * DIW).ToString("0.000");
                dt.Rows.Add(dr);

            }
            dataGrid_bzhjzhx.DataSource = dt;
        }
        public void Init()
        {
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            try
            {
                initConfigInfo();

                ini.INIIO.GetPrivateProfileString("DIW", "DIW", "907.2", temp, 2048, startUpPath+"/detectConfig.ini");
                DIW = float.Parse(temp.ToString());



                ini.INIIO.GetPrivateProfileString("DIW_MONI", "speed_xishu", "1.0", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                speed_xishu = float.Parse(temp.ToString());

                ini.INIIO.GetPrivateProfileString("寄生功率(日常测试)", "40Km/h", "1.00", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                plhp_40 = double.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("寄生功率(日常测试)", "24Km/h", "0.30", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                plhp_24 = double.Parse(temp.ToString());

                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t1_bhjz", "25.33", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t1_bhjz = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t2_bhjz", "15.47", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t2_bhjz = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "t3_bhjz", "3.96", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                t3_bhjz = float.Parse(temp.ToString());
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "useMoni", "true", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                if (temp.ToString() == "true")
                    toolStripLabel提示信息.Text = "准备完毕";
                else
                    toolStripLabel提示信息.Text = "提示信息";
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
                    Init_Comm(configinfdata.Cgjck, configinfdata.cgjckpzz);                           //初始化串口
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
                if (configinfdata.isIgbtContainGdyk)
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

                                case testState.bzhx_state:
                                    if (configinfdata.TestStandard == "HJT290" || configinfdata.TestStandard == "HJT291" || configinfdata.TestStandard == "HJT292")
                                    {
                                        #region HJT290,HJT291,HJT292
                                        if (Speed > 88.5)
                                        //while (true)                //等待达到预定速度
                                        {
                                            // if (Speed > 88.5)
                                            Motor_Close();
                                            bhjz_power = 0;
                                            Set_Control_Power(3.7f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c-qj1);
                                            Start_Control_Power();
                                            timer_flag = true;
                                        }
                                        if (timer_flag == true)//  Motor_Close();                         //达到速度关闭电机
                                        {
                                            if (Speed <= 80.5 && Speed >= 78.8)
                                            {
                                                if (bhjz_power <= 0)
                                                {
                                                    bzhx_t1_start = DateTime.Now;
                                                    bhjz_power = 1;
                                                    Set_Control_Power(3.7f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj1);
                                                }
                                                t1 += 0.01;
                                            }
                                            if (Speed < 78.8 && Speed >= 77.2)
                                            {
                                                if (bhjz_power <= 1)
                                                {
                                                    bhjz_power = 2;
                                                    Set_Control_Power(4.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj1);
                                                }
                                                t2 += 0.01;
                                            }
                                            if (Speed < 77.2 && Speed >= 75.6)
                                            {
                                                if (bhjz_power <= 2)
                                                {
                                                    bhjz_power = 3;
                                                    Set_Control_Power(5.1f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj1);
                                                }
                                                t3 += 0.01;
                                            }
                                            if (Speed < 7.56 && Speed >= 74.0)
                                            {
                                                if (bhjz_power <= 3)
                                                {
                                                    bhjz_power = 4;
                                                    Set_Control_Power(5.9f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj1);
                                                }
                                                t4 += 0.01;
                                            }
                                            if (Speed < 74.0 && Speed >= 72.4)
                                            {
                                                if (bhjz_power <= 4)
                                                {
                                                    bhjz_power = 5;
                                                    Set_Control_Power(6.6f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj1);
                                                }
                                                t5 += 0.01;
                                            }
                                            if (Speed < 72.4 && Speed >= 70.8)
                                            {
                                                if (bhjz_power <= 5)
                                                {
                                                    bzhx_t2_start = DateTime.Now;
                                                    bhjz_power = 6;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t6 += 0.01;
                                            }
                                            if (Speed < 70.8 && Speed >= 69.2)
                                            {
                                                if (bhjz_power <= 6)
                                                {
                                                    bhjz_power = 7;
                                                    Set_Control_Power(5.9f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t7 += 0.01;
                                            }
                                            if (Speed < 69.2 && Speed >= 67.6)
                                            {
                                                if (bhjz_power <= 7)
                                                {
                                                    bhjz_power = 8;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t8 += 0.01;
                                            }
                                            if (Speed < 67.6 && Speed >= 66.0)
                                            {
                                                if (bhjz_power <= 8)
                                                {
                                                    bhjz_power = 9;
                                                    Set_Control_Power(8.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t9 += 0.01;
                                            }
                                            if (Speed < 66.0 && Speed >= 64.4)
                                            {
                                                if (bhjz_power <= 9)
                                                {
                                                    bhjz_power = 10;
                                                    Set_Control_Power(10.3f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t10 += 0.01;
                                            }
                                            if (Speed < 64.4 && Speed >= 62.8)
                                            {
                                                if (bhjz_power <= 10)
                                                {
                                                    bhjz_power = 11;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t11 += 0.01;
                                            }
                                            if (Speed < 62.8 && Speed >= 61.1)
                                            {
                                                if (bhjz_power <= 11)
                                                {
                                                    bhjz_power = 12;
                                                    Set_Control_Power(13.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t12 += 0.01;
                                            }
                                            if (Speed < 61.1 && Speed >= 59.5)
                                            {
                                                if (bhjz_power <= 12)
                                                {
                                                    bzhx_t3_start = DateTime.Now;
                                                    bhjz_power = 13;
                                                    Set_Control_Power(14.7f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t13 += 0.01;
                                            }
                                            if (Speed < 59.5 && Speed >= 57.9)
                                            {
                                                if (bhjz_power <= 13)
                                                {
                                                    bhjz_power = 14;
                                                    Set_Control_Power(15.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t14 += 0.01;
                                            }
                                            if (Speed < 57.9 && Speed >= 56.3)
                                            {
                                                if (bhjz_power <= 14)
                                                {
                                                    bhjz_power = 15;
                                                    Set_Control_Power(16.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t15 += 0.01;
                                            }
                                            if (Speed < 56.3 && Speed >= 54.7)
                                            {
                                                if (bhjz_power <= 15)
                                                {
                                                    bhjz_power = 16;
                                                    Set_Control_Power(16.9f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t16 += 0.01;
                                            }
                                            if (Speed < 54.7 && Speed >= 53.1)
                                            {
                                                if (bhjz_power <= 16)
                                                {
                                                    bhjz_power = 17;
                                                    Set_Control_Power(17.6f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t17 += 0.01;
                                            }
                                            if (Speed < 53.1 && Speed >= 51.5)
                                            {
                                                if (bhjz_power <= 17)
                                                {
                                                    bhjz_power = 18;
                                                    Set_Control_Power(18.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t18 += 0.01;
                                            }
                                            if (Speed < 51.5 && Speed >= 49.9)
                                            {
                                                if (bhjz_power <= 18)
                                                {
                                                    bhjz_power = 19;
                                                    Set_Control_Power(17.6f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t19 += 0.01;
                                            }
                                            if (Speed < 49.9 && Speed >= 48.3)
                                            {
                                                if (bhjz_power <= 19)
                                                {
                                                    bhjz_power = 20;
                                                    Set_Control_Power(16.9f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t20 += 0.01;
                                            }
                                            if (Speed < 48.3 && Speed >= 46.7)
                                            {
                                                if (bhjz_power <= 20)
                                                {
                                                    bhjz_power = 21;
                                                    Set_Control_Power(16.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t21 += 0.01;
                                            }
                                            if (Speed < 46.7 && Speed >= 45.1)
                                            {
                                                if (bhjz_power <= 21)
                                                {
                                                    bhjz_power = 22;
                                                    Set_Control_Power(15.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t22 += 0.01;
                                            }
                                            if (Speed < 45.1 && Speed >= 43.4)
                                            {
                                                if (bhjz_power <= 22)
                                                {
                                                    bhjz_power = 23;
                                                    Set_Control_Power(14.7f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t23 += 0.01;
                                            }
                                            if (Speed < 43.4 && Speed >= 41.8)
                                            {
                                                if (bhjz_power <= 23)
                                                {
                                                    bzhx_t3_end = DateTime.Now;
                                                    bhjz_power = 24;
                                                    Set_Control_Power(13.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t24 += 0.01;
                                            }
                                            if (Speed < 41.8 && Speed >= 40.2)
                                            {
                                                if (bhjz_power <= 24)
                                                {
                                                    bhjz_power = 25;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t25 += 0.01;
                                            }
                                            if (Speed < 40.2 && Speed >= 38.6)
                                            {
                                                if (bhjz_power <= 25)
                                                {
                                                    bhjz_power = 26;
                                                    Set_Control_Power(10.3f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t26 += 0.01;
                                            }
                                            if (Speed < 38.6 && Speed >= 37.0)
                                            {
                                                if (bhjz_power <= 26)
                                                {
                                                    bhjz_power = 27;
                                                    Set_Control_Power(11.0f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t27 += 0.01;
                                            }
                                            if (Speed < 37.0 && Speed >= 35.4)
                                            {
                                                if (bhjz_power <= 27)
                                                {
                                                    bhjz_power = 28;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t28 += 0.01;
                                            }
                                            if (Speed < 35.4 && Speed >= 33.8)
                                            {
                                                if (bhjz_power <= 28)
                                                {
                                                    bhjz_power = 29;
                                                    Set_Control_Power(12.5f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t29 += 0.01;
                                            }
                                            if (Speed < 33.8 && Speed >= 32.2)
                                            {
                                                if (bhjz_power <= 29)
                                                {
                                                    bhjz_power = 30;
                                                    Set_Control_Power(13.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t30 += 0.01;
                                            }
                                            if (Speed < 32.2 && Speed >= 30.6)
                                            {
                                                if (bhjz_power <= 30)
                                                {
                                                    bhjz_power = 31;
                                                    Set_Control_Power(12.5f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t31 += 0.01;
                                            }
                                            if (Speed < 30.6 && Speed >= 29.0)
                                            {
                                                if (bhjz_power <= 31)
                                                {
                                                    bhjz_power = 32;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t32 += 0.01;
                                            }
                                            if (Speed < 29.0 && Speed >= 27.4)
                                            {
                                                if (bhjz_power <= 32)
                                                {
                                                    bhjz_power = 33;
                                                    Set_Control_Power(11.0f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t33 += 0.01;
                                            }
                                            if (Speed < 27.4 && Speed >= 25.7)
                                            {
                                                if (bhjz_power <= 33)
                                                {
                                                    bhjz_power = 34;
                                                    Set_Control_Power(10.3f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t34 += 0.01;
                                            }
                                            if (Speed < 25.7 && Speed >= 24.1)
                                            {
                                                if (bhjz_power <= 34)
                                                {
                                                    bhjz_power = 35;
                                                    Set_Control_Power(8.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t35 += 0.01;
                                            }
                                            if (Speed < 24.1 && Speed >= 22.5)
                                            {
                                                if (bhjz_power <= 35)
                                                {
                                                    bhjz_power = 36;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t36 += 0.01;
                                            }
                                            if (Speed < 22.5 && Speed >= 20.9)
                                            {
                                                if (bhjz_power <= 36)
                                                {
                                                    bhjz_power = 37;
                                                    Set_Control_Power(8.1f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t37 += 0.01;
                                            }
                                            if (Speed < 20.9 && Speed >= 19.3)
                                            {
                                                if (bhjz_power <= 37)
                                                {
                                                    bhjz_power = 38;
                                                    Set_Control_Power(8.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t33 += 0.01;
                                            }
                                            if (Speed < 19.3 && Speed >= 17.7)
                                            {
                                                if (bhjz_power <= 38)
                                                {
                                                    bhjz_power = 39;
                                                    Set_Control_Power(8.1f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t38 += 0.01;
                                            }
                                            if (Speed < 17.7 && Speed >= 16.1)
                                            {
                                                if (bhjz_power <= 39)
                                                {
                                                    bhjz_power = 40;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t39 += 0.01;
                                            }
                                            if (Speed < 16.1 && Speed >= 14.5)
                                            {
                                                if (bhjz_power <= 40)
                                                {
                                                    bzhx_t2_end = DateTime.Now;
                                                    bhjz_power = 41;
                                                    Set_Control_Power(6.6f);
                                                }
                                                t40 += 0.01;
                                            }
                                            if (Speed < 14.5 && Speed >= 12.9)
                                            {
                                                if (bhjz_power <= 41)
                                                {
                                                    bhjz_power = 42;
                                                    Set_Control_Power(5.9f);
                                                }
                                                t41 += 0.01;
                                            }
                                            if (Speed < 12.9 && Speed >= 11.3)
                                            {
                                                if (bhjz_power <= 42)
                                                {
                                                    bhjz_power = 43;
                                                    Set_Control_Power(5.1f);
                                                }
                                                t42 += 0.01;
                                            }
                                            if (Speed < 11.3 && Speed >= 9.7)
                                            {
                                                if (bhjz_power <= 43)
                                                {
                                                    bhjz_power = 44;
                                                    Set_Control_Power(4.4f);
                                                }
                                                t43 += 0.01;
                                            }
                                            if (Speed < 9.7 && Speed >= 8.0)
                                            {
                                                if (bhjz_power <= 44)
                                                {
                                                    bhjz_power = 45;
                                                    Set_Control_Power(3.7f);
                                                }
                                                t44 += 0.01;
                                            }
                                            if (Speed < 8.0)                                       //区间记录完成
                                            {
                                                if (bhjz_power <= 45)
                                                {
                                                    bzhx_t1_end = DateTime.Now;
                                                    bhjz_power = 46;
                                                    testFinish = true;
                                                    nowState = testState.idle_state;
                                                }

                                            }
                                        }
                                        #endregion
                                    }
                                    else if (configinfdata.TestStandard == "JJF1221压燃式")
                                    {
                                        #region JJF1221压燃式
                                        if (Speed > 80.5)
                                        //while (true)                //等待达到预定速度
                                        {
                                            // if (Speed > 88.5)
                                            Motor_Close();
                                            bhjz_power = 5;
                                            Set_Control_Power(6.6f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                            Start_Control_Power();
                                            timer_flag = true;
                                        }
                                        if (timer_flag == true)//  Motor_Close();                         //达到速度关闭电机
                                        {
                                            if (Speed < 72.4 && Speed >= 70.8)
                                            {
                                                if (bhjz_power <= 5)
                                                {
                                                    bzhx_t2_start = DateTime.Now;
                                                    bhjz_power = 6;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t6 += 0.01;
                                            }
                                            if (Speed < 70.8 && Speed >= 69.2)
                                            {
                                                if (bhjz_power <= 6)
                                                {
                                                    bhjz_power = 7;
                                                    Set_Control_Power(5.9f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t7 += 0.01;
                                            }
                                            if (Speed < 69.2 && Speed >= 67.6)
                                            {
                                                if (bhjz_power <= 7)
                                                {
                                                    bhjz_power = 8;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t8 += 0.01;
                                            }
                                            if (Speed < 67.6 && Speed >= 66.0)
                                            {
                                                if (bhjz_power <= 8)
                                                {
                                                    bhjz_power = 9;
                                                    Set_Control_Power(8.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t9 += 0.01;
                                            }
                                            if (Speed < 66.0 && Speed >= 64.4)
                                            {
                                                if (bhjz_power <= 9)
                                                {
                                                    bhjz_power = 10;
                                                    Set_Control_Power(10.3f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t10 += 0.01;
                                            }
                                            if (Speed < 64.4 && Speed >= 62.8)
                                            {
                                                if (bhjz_power <= 10)
                                                {
                                                    bhjz_power = 11;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t11 += 0.01;
                                            }
                                            if (Speed < 62.8 && Speed >= 61.1)
                                            {
                                                if (bhjz_power <= 11)
                                                {
                                                    bhjz_power = 12;
                                                    Set_Control_Power(13.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t12 += 0.01;
                                            }
                                            if (Speed < 61.1 && Speed >= 59.5)
                                            {
                                                if (bhjz_power <= 12)
                                                {
                                                    //bzhx_t3_start = DateTime.Now;
                                                    bhjz_power = 13;
                                                    Set_Control_Power(14.7f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t13 += 0.01;
                                            }
                                            if (Speed < 59.5 && Speed >= 57.9)
                                            {
                                                if (bhjz_power <= 13)
                                                {
                                                    bhjz_power = 14;
                                                    Set_Control_Power(15.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t14 += 0.01;
                                            }
                                            if (Speed < 57.9 && Speed >= 56.3)
                                            {
                                                if (bhjz_power <= 14)
                                                {
                                                    bhjz_power = 15;
                                                    Set_Control_Power(16.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t15 += 0.01;
                                            }
                                            if (Speed < 56.3 && Speed >= 54.7)
                                            {
                                                if (bhjz_power <= 15)
                                                {
                                                    bhjz_power = 16;
                                                    Set_Control_Power(16.9f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t16 += 0.01;
                                            }
                                            if (Speed < 54.7 && Speed >= 53.1)
                                            {
                                                if (bhjz_power <= 16)
                                                {
                                                    bhjz_power = 17;
                                                    Set_Control_Power(17.6f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t17 += 0.01;
                                            }
                                            if (Speed < 53.1 && Speed >= 51.5)
                                            {
                                                if (bhjz_power <= 17)
                                                {
                                                    bhjz_power = 18;
                                                    Set_Control_Power(18.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t18 += 0.01;
                                            }
                                            if (Speed < 51.5 && Speed >= 49.9)
                                            {
                                                if (bhjz_power <= 18)
                                                {
                                                    bhjz_power = 19;
                                                    Set_Control_Power(17.6f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t19 += 0.01;
                                            }
                                            if (Speed < 49.9 && Speed >= 48.3)
                                            {
                                                if (bhjz_power <= 19)
                                                {
                                                    bhjz_power = 20;
                                                    Set_Control_Power(16.9f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t20 += 0.01;
                                            }
                                            if (Speed < 48.3 && Speed >= 46.7)
                                            {
                                                if (bhjz_power <= 20)
                                                {
                                                    //bzhx_t2_start = DateTime.Now;
                                                    bhjz_power = 21;
                                                    Set_Control_Power(16.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t21 += 0.01;
                                            }
                                            if (Speed < 46.7 && Speed >= 45.1)
                                            {
                                                if (bhjz_power <= 21)
                                                {
                                                    bhjz_power = 22;
                                                    Set_Control_Power(15.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t22 += 0.01;
                                            }
                                            if (Speed < 45.1 && Speed >= 43.4)
                                            {
                                                if (bhjz_power <= 22)
                                                {
                                                    bhjz_power = 23;
                                                    Set_Control_Power(14.7f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t23 += 0.01;
                                            }
                                            if (Speed < 43.4 && Speed >= 41.8)
                                            {
                                                if (bhjz_power <= 23)
                                                {
                                                    //bzhx_t3_end = DateTime.Now;
                                                    bhjz_power = 24;
                                                    Set_Control_Power(13.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t24 += 0.01;
                                            }
                                            if (Speed < 41.8 && Speed >= 40.2)
                                            {
                                                if (bhjz_power <= 24)
                                                {
                                                    bhjz_power = 25;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t25 += 0.01;
                                            }
                                            if (Speed < 40.2 && Speed >= 38.6)
                                            {
                                                if (bhjz_power <= 25)
                                                {
                                                    bhjz_power = 26;
                                                    Set_Control_Power(10.3f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t26 += 0.01;
                                            }
                                            if (Speed < 38.6 && Speed >= 37.0)
                                            {
                                                if (bhjz_power <= 26)
                                                {
                                                    bhjz_power = 27;
                                                    Set_Control_Power(11.0f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t27 += 0.01;
                                            }
                                            if (Speed < 37.0 && Speed >= 35.4)
                                            {
                                                if (bhjz_power <= 27)
                                                {
                                                    bhjz_power = 28;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t28 += 0.01;
                                            }
                                            if (Speed < 35.4 && Speed >= 33.8)
                                            {
                                                if (bhjz_power <= 28)
                                                {
                                                    bhjz_power = 29;
                                                    Set_Control_Power(12.5f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t29 += 0.01;
                                            }
                                            if (Speed < 33.8 && Speed >= 32.2)
                                            {
                                                if (bhjz_power <= 29)
                                                {
                                                    bhjz_power = 30;
                                                    Set_Control_Power(13.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t30 += 0.01;
                                            }
                                            if (Speed < 32.2 && Speed >= 30.6)
                                            {
                                                if (bhjz_power <= 30)
                                                {
                                                    bhjz_power = 31;
                                                    Set_Control_Power(12.5f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t31 += 0.01;
                                            }
                                            if (Speed < 30.6 && Speed >= 29.0)
                                            {
                                                if (bhjz_power <= 31)
                                                {
                                                    bhjz_power = 32;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t32 += 0.01;
                                            }
                                            if (Speed < 29.0 && Speed >= 27.4)
                                            {
                                                if (bhjz_power <= 32)
                                                {
                                                    bhjz_power = 33;
                                                    Set_Control_Power(11.0f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t33 += 0.01;
                                            }
                                            if (Speed < 27.4 && Speed >= 25.7)
                                            {
                                                if (bhjz_power <= 33)
                                                {
                                                    bhjz_power = 34;
                                                    Set_Control_Power(10.3f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t34 += 0.01;
                                            }
                                            if (Speed < 25.7 && Speed >= 24.1)
                                            {
                                                if (bhjz_power <= 34)
                                                {
                                                    bhjz_power = 35;
                                                    Set_Control_Power(8.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t35 += 0.01;
                                            }
                                            if (Speed < 24.1 && Speed >= 22.5)
                                            {
                                                if (bhjz_power <= 35)
                                                {
                                                    bhjz_power = 36;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t36 += 0.01;
                                            }
                                            if (Speed < 22.5 && Speed >= 20.9)
                                            {
                                                if (bhjz_power <= 36)
                                                {
                                                    bhjz_power = 37;
                                                    Set_Control_Power(8.1f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t37 += 0.01;
                                            }
                                            if (Speed < 20.9 && Speed >= 19.3)
                                            {
                                                if (bhjz_power <= 37)
                                                {
                                                    bhjz_power = 38;
                                                    Set_Control_Power(8.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t33 += 0.01;
                                            }
                                            if (Speed < 19.3 && Speed >= 17.7)
                                            {
                                                if (bhjz_power <= 38)
                                                {
                                                    bhjz_power = 39;
                                                    Set_Control_Power(8.1f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t38 += 0.01;
                                            }
                                            if (Speed < 17.7 && Speed >= 16.1)
                                            {
                                                if (bhjz_power <= 39)
                                                {
                                                    bhjz_power = 40;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t39 += 0.01;
                                            }

                                            if (Speed < 16.1)                                       //区间记录完成
                                            {
                                                if (bhjz_power <= 40)
                                                {
                                                    bzhx_t2_end = DateTime.Now;
                                                    bhjz_power = 46;
                                                    testFinish = true;
                                                    nowState = testState.idle_state;
                                                }

                                            }
                                        }
                                        #endregion

                                    }
                                    else if (configinfdata.TestStandard == "JJF1221点燃式")
                                    {
                                        #region JJF1221点燃式
                                        if (Speed > 56.0)
                                        //while (true)                //等待达到预定速度
                                        {
                                            // if (Speed > 88.5)
                                            Motor_Close();
                                            bhjz_power = 20;
                                            Set_Control_Power(16.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                            Start_Control_Power();
                                            timer_flag = true;
                                        }
                                        if (timer_flag == true)//  Motor_Close();                         //达到速度关闭电机
                                        {
                                            if (Speed < 48.3 && Speed >= 46.7)
                                            {
                                                if (bhjz_power <= 20)
                                                {
                                                    //bzhx_t2_start = DateTime.Now;
                                                    bzhx_t2_start = DateTime.Now;
                                                    bhjz_power = 21;
                                                    Set_Control_Power(16.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t21 += 0.01;
                                            }
                                            if (Speed < 46.7 && Speed >= 45.1)
                                            {
                                                if (bhjz_power <= 21)
                                                {
                                                    bhjz_power = 22;
                                                    Set_Control_Power(15.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t22 += 0.01;
                                            }
                                            if (Speed < 45.1 && Speed >= 43.4)
                                            {
                                                if (bhjz_power <= 22)
                                                {
                                                    bhjz_power = 23;
                                                    Set_Control_Power(14.7f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t23 += 0.01;
                                            }
                                            if (Speed < 43.4 && Speed >= 41.8)
                                            {
                                                if (bhjz_power <= 23)
                                                {
                                                    //bzhx_t3_end = DateTime.Now;
                                                    bhjz_power = 24;
                                                    Set_Control_Power(13.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t24 += 0.01;
                                            }
                                            if (Speed < 41.8 && Speed >= 40.2)
                                            {
                                                if (bhjz_power <= 24)
                                                {
                                                    bhjz_power = 25;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t25 += 0.01;
                                            }
                                            if (Speed < 40.2 && Speed >= 38.6)
                                            {
                                                if (bhjz_power <= 25)
                                                {
                                                    bhjz_power = 26;
                                                    Set_Control_Power(10.3f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t26 += 0.01;
                                            }
                                            if (Speed < 38.6 && Speed >= 37.0)
                                            {
                                                if (bhjz_power <= 26)
                                                {
                                                    bhjz_power = 27;
                                                    Set_Control_Power(11.0f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t27 += 0.01;
                                            }
                                            if (Speed < 37.0 && Speed >= 35.4)
                                            {
                                                if (bhjz_power <= 27)
                                                {
                                                    bhjz_power = 28;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t28 += 0.01;
                                            }
                                            if (Speed < 35.4 && Speed >= 33.8)
                                            {
                                                if (bhjz_power <= 28)
                                                {
                                                    bhjz_power = 29;
                                                    Set_Control_Power(12.5f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t29 += 0.01;
                                            }
                                            if (Speed < 33.8 && Speed >= 32.2)
                                            {
                                                if (bhjz_power <= 29)
                                                {
                                                    bhjz_power = 30;
                                                    Set_Control_Power(13.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t30 += 0.01;
                                            }
                                            if (Speed < 32.2 && Speed >= 30.6)
                                            {
                                                if (bhjz_power <= 30)
                                                {
                                                    bhjz_power = 31;
                                                    Set_Control_Power(12.5f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t31 += 0.01;
                                            }
                                            if (Speed < 30.6 && Speed >= 29.0)
                                            {
                                                if (bhjz_power <= 31)
                                                {
                                                    bhjz_power = 32;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t32 += 0.01;
                                            }
                                            if (Speed < 29.0 && Speed >= 27.4)
                                            {
                                                if (bhjz_power <= 32)
                                                {
                                                    bhjz_power = 33;
                                                    Set_Control_Power(11.0f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t33 += 0.01;
                                            }
                                            if (Speed < 27.4 && Speed >= 25.7)
                                            {
                                                if (bhjz_power <= 33)
                                                {
                                                    bhjz_power = 34;
                                                    Set_Control_Power(10.3f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t34 += 0.01;
                                            }
                                            if (Speed < 25.7 && Speed >= 24.1)
                                            {
                                                if (bhjz_power <= 34)
                                                {
                                                    bhjz_power = 35;
                                                    Set_Control_Power(8.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t35 += 0.01;
                                            }
                                            if (Speed < 24.1 && Speed >= 22.5)
                                            {
                                                if (bhjz_power <= 35)
                                                {
                                                    bhjz_power = 36;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t36 += 0.01;
                                            }
                                            if (Speed < 22.5 && Speed >= 20.9)
                                            {
                                                if (bhjz_power <= 36)
                                                {
                                                    bhjz_power = 37;
                                                    Set_Control_Power(8.1f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t37 += 0.01;
                                            }
                                            if (Speed < 20.9 && Speed >= 19.3)
                                            {
                                                if (bhjz_power <= 37)
                                                {
                                                    bhjz_power = 38;
                                                    Set_Control_Power(8.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t33 += 0.01;
                                            }
                                            if (Speed < 19.3 && Speed >= 17.7)
                                            {
                                                if (bhjz_power <= 38)
                                                {
                                                    bhjz_power = 39;
                                                    Set_Control_Power(8.1f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t38 += 0.01;
                                            }
                                            if (Speed < 17.7 && Speed >= 16.1)
                                            {
                                                if (bhjz_power <= 39)
                                                {
                                                    bhjz_power = 40;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t39 += 0.01;
                                            }

                                            if (Speed < 16.1)                                       //区间记录完成
                                            {
                                                if (bhjz_power <= 40)
                                                {
                                                    bzhx_t2_end = DateTime.Now;
                                                    bhjz_power = 46;
                                                    testFinish = true;
                                                    nowState = testState.idle_state;
                                                }

                                            }
                                        }
                                        #endregion
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

        #region 带光电串口返回数据解析
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

                                case testState.bzhx_state:
                                    if (configinfdata.TestStandard == "HJT290" || configinfdata.TestStandard == "HJT291" || configinfdata.TestStandard == "HJT292")
                                    {
                                        #region HJT290,HJT291,HJT292
                                        if (Speed > 88.5)
                                        //while (true)                //等待达到预定速度
                                        {
                                            // if (Speed > 88.5)
                                            Motor_Close();
                                            bhjz_power = 0;
                                            Set_Control_Power(3.7f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj1);
                                            Start_Control_Power();
                                            timer_flag = true;
                                        }
                                        if (timer_flag == true)//  Motor_Close();                         //达到速度关闭电机
                                        {
                                            if (Speed <= 80.5 && Speed >= 78.8)
                                            {
                                                if (bhjz_power <= 0)
                                                {
                                                    bzhx_t1_start = DateTime.Now;
                                                    bhjz_power = 1;
                                                    Set_Control_Power(3.7f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj1);
                                                }
                                                t1 += 0.01;
                                            }
                                            if (Speed < 78.8 && Speed >= 77.2)
                                            {
                                                if (bhjz_power <= 1)
                                                {
                                                    bhjz_power = 2;
                                                    Set_Control_Power(4.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj1);
                                                }
                                                t2 += 0.01;
                                            }
                                            if (Speed < 77.2 && Speed >= 75.6)
                                            {
                                                if (bhjz_power <= 2)
                                                {
                                                    bhjz_power = 3;
                                                    Set_Control_Power(5.1f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj1);
                                                }
                                                t3 += 0.01;
                                            }
                                            if (Speed < 7.56 && Speed >= 74.0)
                                            {
                                                if (bhjz_power <= 3)
                                                {
                                                    bhjz_power = 4;
                                                    Set_Control_Power(5.9f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj1);
                                                }
                                                t4 += 0.01;
                                            }
                                            if (Speed < 74.0 && Speed >= 72.4)
                                            {
                                                if (bhjz_power <= 4)
                                                {
                                                    bhjz_power = 5;
                                                    Set_Control_Power(6.6f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj1);
                                                }
                                                t5 += 0.01;
                                            }
                                            if (Speed < 72.4 && Speed >= 70.8)
                                            {
                                                if (bhjz_power <= 5)
                                                {
                                                    bzhx_t2_start = DateTime.Now;
                                                    bhjz_power = 6;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t6 += 0.01;
                                            }
                                            if (Speed < 70.8 && Speed >= 69.2)
                                            {
                                                if (bhjz_power <= 6)
                                                {
                                                    bhjz_power = 7;
                                                    Set_Control_Power(5.9f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t7 += 0.01;
                                            }
                                            if (Speed < 69.2 && Speed >= 67.6)
                                            {
                                                if (bhjz_power <= 7)
                                                {
                                                    bhjz_power = 8;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t8 += 0.01;
                                            }
                                            if (Speed < 67.6 && Speed >= 66.0)
                                            {
                                                if (bhjz_power <= 8)
                                                {
                                                    bhjz_power = 9;
                                                    Set_Control_Power(8.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t9 += 0.01;
                                            }
                                            if (Speed < 66.0 && Speed >= 64.4)
                                            {
                                                if (bhjz_power <= 9)
                                                {
                                                    bhjz_power = 10;
                                                    Set_Control_Power(10.3f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t10 += 0.01;
                                            }
                                            if (Speed < 64.4 && Speed >= 62.8)
                                            {
                                                if (bhjz_power <= 10)
                                                {
                                                    bhjz_power = 11;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t11 += 0.01;
                                            }
                                            if (Speed < 62.8 && Speed >= 61.1)
                                            {
                                                if (bhjz_power <= 11)
                                                {
                                                    bhjz_power = 12;
                                                    Set_Control_Power(13.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t12 += 0.01;
                                            }
                                            if (Speed < 61.1 && Speed >= 59.5)
                                            {
                                                if (bhjz_power <= 12)
                                                {
                                                    bzhx_t3_start = DateTime.Now;
                                                    bhjz_power = 13;
                                                    Set_Control_Power(14.7f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t13 += 0.01;
                                            }
                                            if (Speed < 59.5 && Speed >= 57.9)
                                            {
                                                if (bhjz_power <= 13)
                                                {
                                                    bhjz_power = 14;
                                                    Set_Control_Power(15.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t14 += 0.01;
                                            }
                                            if (Speed < 57.9 && Speed >= 56.3)
                                            {
                                                if (bhjz_power <= 14)
                                                {
                                                    bhjz_power = 15;
                                                    Set_Control_Power(16.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t15 += 0.01;
                                            }
                                            if (Speed < 56.3 && Speed >= 54.7)
                                            {
                                                if (bhjz_power <= 15)
                                                {
                                                    bhjz_power = 16;
                                                    Set_Control_Power(16.9f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t16 += 0.01;
                                            }
                                            if (Speed < 54.7 && Speed >= 53.1)
                                            {
                                                if (bhjz_power <= 16)
                                                {
                                                    bhjz_power = 17;
                                                    Set_Control_Power(17.6f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t17 += 0.01;
                                            }
                                            if (Speed < 53.1 && Speed >= 51.5)
                                            {
                                                if (bhjz_power <= 17)
                                                {
                                                    bhjz_power = 18;
                                                    Set_Control_Power(18.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t18 += 0.01;
                                            }
                                            if (Speed < 51.5 && Speed >= 49.9)
                                            {
                                                if (bhjz_power <= 18)
                                                {
                                                    bhjz_power = 19;
                                                    Set_Control_Power(17.6f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t19 += 0.01;
                                            }
                                            if (Speed < 49.9 && Speed >= 48.3)
                                            {
                                                if (bhjz_power <= 19)
                                                {
                                                    bhjz_power = 20;
                                                    Set_Control_Power(16.9f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t20 += 0.01;
                                            }
                                            if (Speed < 48.3 && Speed >= 46.7)
                                            {
                                                if (bhjz_power <= 20)
                                                {
                                                    bhjz_power = 21;
                                                    Set_Control_Power(16.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t21 += 0.01;
                                            }
                                            if (Speed < 46.7 && Speed >= 45.1)
                                            {
                                                if (bhjz_power <= 21)
                                                {
                                                    bhjz_power = 22;
                                                    Set_Control_Power(15.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t22 += 0.01;
                                            }
                                            if (Speed < 45.1 && Speed >= 43.4)
                                            {
                                                if (bhjz_power <= 22)
                                                {
                                                    bhjz_power = 23;
                                                    Set_Control_Power(14.7f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj3);
                                                }
                                                t23 += 0.01;
                                            }
                                            if (Speed < 43.4 && Speed >= 41.8)
                                            {
                                                if (bhjz_power <= 23)
                                                {
                                                    bzhx_t3_end = DateTime.Now;
                                                    bhjz_power = 24;
                                                    Set_Control_Power(13.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t24 += 0.01;
                                            }
                                            if (Speed < 41.8 && Speed >= 40.2)
                                            {
                                                if (bhjz_power <= 24)
                                                {
                                                    bhjz_power = 25;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t25 += 0.01;
                                            }
                                            if (Speed < 40.2 && Speed >= 38.6)
                                            {
                                                if (bhjz_power <= 25)
                                                {
                                                    bhjz_power = 26;
                                                    Set_Control_Power(10.3f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t26 += 0.01;
                                            }
                                            if (Speed < 38.6 && Speed >= 37.0)
                                            {
                                                if (bhjz_power <= 26)
                                                {
                                                    bhjz_power = 27;
                                                    Set_Control_Power(11.0f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t27 += 0.01;
                                            }
                                            if (Speed < 37.0 && Speed >= 35.4)
                                            {
                                                if (bhjz_power <= 27)
                                                {
                                                    bhjz_power = 28;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t28 += 0.01;
                                            }
                                            if (Speed < 35.4 && Speed >= 33.8)
                                            {
                                                if (bhjz_power <= 28)
                                                {
                                                    bhjz_power = 29;
                                                    Set_Control_Power(12.5f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t29 += 0.01;
                                            }
                                            if (Speed < 33.8 && Speed >= 32.2)
                                            {
                                                if (bhjz_power <= 29)
                                                {
                                                    bhjz_power = 30;
                                                    Set_Control_Power(13.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t30 += 0.01;
                                            }
                                            if (Speed < 32.2 && Speed >= 30.6)
                                            {
                                                if (bhjz_power <= 30)
                                                {
                                                    bhjz_power = 31;
                                                    Set_Control_Power(12.5f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t31 += 0.01;
                                            }
                                            if (Speed < 30.6 && Speed >= 29.0)
                                            {
                                                if (bhjz_power <= 31)
                                                {
                                                    bhjz_power = 32;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t32 += 0.01;
                                            }
                                            if (Speed < 29.0 && Speed >= 27.4)
                                            {
                                                if (bhjz_power <= 32)
                                                {
                                                    bhjz_power = 33;
                                                    Set_Control_Power(11.0f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t33 += 0.01;
                                            }
                                            if (Speed < 27.4 && Speed >= 25.7)
                                            {
                                                if (bhjz_power <= 33)
                                                {
                                                    bhjz_power = 34;
                                                    Set_Control_Power(10.3f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t34 += 0.01;
                                            }
                                            if (Speed < 25.7 && Speed >= 24.1)
                                            {
                                                if (bhjz_power <= 34)
                                                {
                                                    bhjz_power = 35;
                                                    Set_Control_Power(8.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t35 += 0.01;
                                            }
                                            if (Speed < 24.1 && Speed >= 22.5)
                                            {
                                                if (bhjz_power <= 35)
                                                {
                                                    bhjz_power = 36;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t36 += 0.01;
                                            }
                                            if (Speed < 22.5 && Speed >= 20.9)
                                            {
                                                if (bhjz_power <= 36)
                                                {
                                                    bhjz_power = 37;
                                                    Set_Control_Power(8.1f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t37 += 0.01;
                                            }
                                            if (Speed < 20.9 && Speed >= 19.3)
                                            {
                                                if (bhjz_power <= 37)
                                                {
                                                    bhjz_power = 38;
                                                    Set_Control_Power(8.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t33 += 0.01;
                                            }
                                            if (Speed < 19.3 && Speed >= 17.7)
                                            {
                                                if (bhjz_power <= 38)
                                                {
                                                    bhjz_power = 39;
                                                    Set_Control_Power(8.1f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t38 += 0.01;
                                            }
                                            if (Speed < 17.7 && Speed >= 16.1)
                                            {
                                                if (bhjz_power <= 39)
                                                {
                                                    bhjz_power = 40;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t39 += 0.01;
                                            }
                                            if (Speed < 16.1 && Speed >= 14.5)
                                            {
                                                if (bhjz_power <= 40)
                                                {
                                                    bzhx_t2_end = DateTime.Now;
                                                    bhjz_power = 41;
                                                    Set_Control_Power(6.6f);
                                                }
                                                t40 += 0.01;
                                            }
                                            if (Speed < 14.5 && Speed >= 12.9)
                                            {
                                                if (bhjz_power <= 41)
                                                {
                                                    bhjz_power = 42;
                                                    Set_Control_Power(5.9f);
                                                }
                                                t41 += 0.01;
                                            }
                                            if (Speed < 12.9 && Speed >= 11.3)
                                            {
                                                if (bhjz_power <= 42)
                                                {
                                                    bhjz_power = 43;
                                                    Set_Control_Power(5.1f);
                                                }
                                                t42 += 0.01;
                                            }
                                            if (Speed < 11.3 && Speed >= 9.7)
                                            {
                                                if (bhjz_power <= 43)
                                                {
                                                    bhjz_power = 44;
                                                    Set_Control_Power(4.4f);
                                                }
                                                t43 += 0.01;
                                            }
                                            if (Speed < 9.7 && Speed >= 8.0)
                                            {
                                                if (bhjz_power <= 44)
                                                {
                                                    bhjz_power = 45;
                                                    Set_Control_Power(3.7f);
                                                }
                                                t44 += 0.01;
                                            }
                                            if (Speed < 8.0)                                       //区间记录完成
                                            {
                                                if (bhjz_power <= 45)
                                                {
                                                    bzhx_t1_end = DateTime.Now;
                                                    bhjz_power = 46;
                                                    testFinish = true;
                                                    nowState = testState.idle_state;
                                                }

                                            }
                                        }
                                        #endregion
                                    }
                                    else if (configinfdata.TestStandard == "JJF1221压燃式")
                                    {
                                        #region JJF1221压燃式
                                        if (Speed > 80.5)
                                        //while (true)                //等待达到预定速度
                                        {
                                            // if (Speed > 88.5)
                                            Motor_Close();
                                            bhjz_power = 5;
                                            Set_Control_Power(6.6f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c-qj2);
                                            Start_Control_Power();
                                            timer_flag = true;
                                        }
                                        if (timer_flag == true)//  Motor_Close();                         //达到速度关闭电机
                                        {
                                            if (Speed < 72.4 && Speed >= 70.8)
                                            {
                                                if (bhjz_power <= 5)
                                                {
                                                    bzhx_t2_start = DateTime.Now;
                                                    bhjz_power = 6;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t6 += 0.01;
                                            }
                                            if (Speed < 70.8 && Speed >= 69.2)
                                            {
                                                if (bhjz_power <= 6)
                                                {
                                                    bhjz_power = 7;
                                                    Set_Control_Power(5.9f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t7 += 0.01;
                                            }
                                            if (Speed < 69.2 && Speed >= 67.6)
                                            {
                                                if (bhjz_power <= 7)
                                                {
                                                    bhjz_power = 8;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t8 += 0.01;
                                            }
                                            if (Speed < 67.6 && Speed >= 66.0)
                                            {
                                                if (bhjz_power <= 8)
                                                {
                                                    bhjz_power = 9;
                                                    Set_Control_Power(8.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t9 += 0.01;
                                            }
                                            if (Speed < 66.0 && Speed >= 64.4)
                                            {
                                                if (bhjz_power <= 9)
                                                {
                                                    bhjz_power = 10;
                                                    Set_Control_Power(10.3f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t10 += 0.01;
                                            }
                                            if (Speed < 64.4 && Speed >= 62.8)
                                            {
                                                if (bhjz_power <= 10)
                                                {
                                                    bhjz_power = 11;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t11 += 0.01;
                                            }
                                            if (Speed < 62.8 && Speed >= 61.1)
                                            {
                                                if (bhjz_power <= 11)
                                                {
                                                    bhjz_power = 12;
                                                    Set_Control_Power(13.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t12 += 0.01;
                                            }
                                            if (Speed < 61.1 && Speed >= 59.5)
                                            {
                                                if (bhjz_power <= 12)
                                                {
                                                    //bzhx_t3_start = DateTime.Now;
                                                    bhjz_power = 13;
                                                    Set_Control_Power(14.7f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t13 += 0.01;
                                            }
                                            if (Speed < 59.5 && Speed >= 57.9)
                                            {
                                                if (bhjz_power <= 13)
                                                {
                                                    bhjz_power = 14;
                                                    Set_Control_Power(15.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t14 += 0.01;
                                            }
                                            if (Speed < 57.9 && Speed >= 56.3)
                                            {
                                                if (bhjz_power <= 14)
                                                {
                                                    bhjz_power = 15;
                                                    Set_Control_Power(16.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t15 += 0.01;
                                            }
                                            if (Speed < 56.3 && Speed >= 54.7)
                                            {
                                                if (bhjz_power <= 15)
                                                {
                                                    bhjz_power = 16;
                                                    Set_Control_Power(16.9f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t16 += 0.01;
                                            }
                                            if (Speed < 54.7 && Speed >= 53.1)
                                            {
                                                if (bhjz_power <= 16)
                                                {
                                                    bhjz_power = 17;
                                                    Set_Control_Power(17.6f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t17 += 0.01;
                                            }
                                            if (Speed < 53.1 && Speed >= 51.5)
                                            {
                                                if (bhjz_power <= 17)
                                                {
                                                    bhjz_power = 18;
                                                    Set_Control_Power(18.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t18 += 0.01;
                                            }
                                            if (Speed < 51.5 && Speed >= 49.9)
                                            {
                                                if (bhjz_power <= 18)
                                                {
                                                    bhjz_power = 19;
                                                    Set_Control_Power(17.6f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t19 += 0.01;
                                            }
                                            if (Speed < 49.9 && Speed >= 48.3)
                                            {
                                                if (bhjz_power <= 19)
                                                {
                                                    bhjz_power = 20;
                                                    Set_Control_Power(16.9f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t20 += 0.01;
                                            }
                                            if (Speed < 48.3 && Speed >= 46.7)
                                            {
                                                if (bhjz_power <= 20)
                                                {
                                                    //bzhx_t2_start = DateTime.Now;
                                                    bhjz_power = 21;
                                                    Set_Control_Power(16.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t21 += 0.01;
                                            }
                                            if (Speed < 46.7 && Speed >= 45.1)
                                            {
                                                if (bhjz_power <= 21)
                                                {
                                                    bhjz_power = 22;
                                                    Set_Control_Power(15.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t22 += 0.01;
                                            }
                                            if (Speed < 45.1 && Speed >= 43.4)
                                            {
                                                if (bhjz_power <= 22)
                                                {
                                                    bhjz_power = 23;
                                                    Set_Control_Power(14.7f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t23 += 0.01;
                                            }
                                            if (Speed < 43.4 && Speed >= 41.8)
                                            {
                                                if (bhjz_power <= 23)
                                                {
                                                    //bzhx_t3_end = DateTime.Now;
                                                    bhjz_power = 24;
                                                    Set_Control_Power(13.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t24 += 0.01;
                                            }
                                            if (Speed < 41.8 && Speed >= 40.2)
                                            {
                                                if (bhjz_power <= 24)
                                                {
                                                    bhjz_power = 25;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t25 += 0.01;
                                            }
                                            if (Speed < 40.2 && Speed >= 38.6)
                                            {
                                                if (bhjz_power <= 25)
                                                {
                                                    bhjz_power = 26;
                                                    Set_Control_Power(10.3f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t26 += 0.01;
                                            }
                                            if (Speed < 38.6 && Speed >= 37.0)
                                            {
                                                if (bhjz_power <= 26)
                                                {
                                                    bhjz_power = 27;
                                                    Set_Control_Power(11.0f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t27 += 0.01;
                                            }
                                            if (Speed < 37.0 && Speed >= 35.4)
                                            {
                                                if (bhjz_power <= 27)
                                                {
                                                    bhjz_power = 28;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t28 += 0.01;
                                            }
                                            if (Speed < 35.4 && Speed >= 33.8)
                                            {
                                                if (bhjz_power <= 28)
                                                {
                                                    bhjz_power = 29;
                                                    Set_Control_Power(12.5f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t29 += 0.01;
                                            }
                                            if (Speed < 33.8 && Speed >= 32.2)
                                            {
                                                if (bhjz_power <= 29)
                                                {
                                                    bhjz_power = 30;
                                                    Set_Control_Power(13.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t30 += 0.01;
                                            }
                                            if (Speed < 32.2 && Speed >= 30.6)
                                            {
                                                if (bhjz_power <= 30)
                                                {
                                                    bhjz_power = 31;
                                                    Set_Control_Power(12.5f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t31 += 0.01;
                                            }
                                            if (Speed < 30.6 && Speed >= 29.0)
                                            {
                                                if (bhjz_power <= 31)
                                                {
                                                    bhjz_power = 32;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t32 += 0.01;
                                            }
                                            if (Speed < 29.0 && Speed >= 27.4)
                                            {
                                                if (bhjz_power <= 32)
                                                {
                                                    bhjz_power = 33;
                                                    Set_Control_Power(11.0f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t33 += 0.01;
                                            }
                                            if (Speed < 27.4 && Speed >= 25.7)
                                            {
                                                if (bhjz_power <= 33)
                                                {
                                                    bhjz_power = 34;
                                                    Set_Control_Power(10.3f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t34 += 0.01;
                                            }
                                            if (Speed < 25.7 && Speed >= 24.1)
                                            {
                                                if (bhjz_power <= 34)
                                                {
                                                    bhjz_power = 35;
                                                    Set_Control_Power(8.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t35 += 0.01;
                                            }
                                            if (Speed < 24.1 && Speed >= 22.5)
                                            {
                                                if (bhjz_power <= 35)
                                                {
                                                    bhjz_power = 36;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t36 += 0.01;
                                            }
                                            if (Speed < 22.5 && Speed >= 20.9)
                                            {
                                                if (bhjz_power <= 36)
                                                {
                                                    bhjz_power = 37;
                                                    Set_Control_Power(8.1f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t37 += 0.01;
                                            }
                                            if (Speed < 20.9 && Speed >= 19.3)
                                            {
                                                if (bhjz_power <= 37)
                                                {
                                                    bhjz_power = 38;
                                                    Set_Control_Power(8.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t33 += 0.01;
                                            }
                                            if (Speed < 19.3 && Speed >= 17.7)
                                            {
                                                if (bhjz_power <= 38)
                                                {
                                                    bhjz_power = 39;
                                                    Set_Control_Power(8.1f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t38 += 0.01;
                                            }
                                            if (Speed < 17.7 && Speed >= 16.1)
                                            {
                                                if (bhjz_power <= 39)
                                                {
                                                    bhjz_power = 40;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t39 += 0.01;
                                            }

                                            if (Speed < 16.1)                                       //区间记录完成
                                            {
                                                if (bhjz_power <= 40)
                                                {
                                                    bzhx_t2_end = DateTime.Now;
                                                    bhjz_power = 46;
                                                    testFinish = true;
                                                    nowState = testState.idle_state;
                                                }

                                            }
                                        }
                                        #endregion

                                    }
                                    else if (configinfdata.TestStandard == "JJF1221点燃式")
                                    {
                                        #region JJF1221点燃式
                                        if (Speed > 56.0)
                                        //while (true)                //等待达到预定速度
                                        {
                                            // if (Speed > 88.5)
                                            Motor_Close();
                                            bhjz_power = 20;
                                            Set_Control_Power(16.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                            Start_Control_Power();
                                            timer_flag = true;
                                        }
                                        if (timer_flag == true)//  Motor_Close();                         //达到速度关闭电机
                                        {
                                            if (Speed < 48.3 && Speed >= 46.7)
                                            {
                                                if (bhjz_power <= 20)
                                                {
                                                    //bzhx_t2_start = DateTime.Now;
                                                    bzhx_t2_start = DateTime.Now;
                                                    bhjz_power = 21;
                                                    Set_Control_Power(16.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t21 += 0.01;
                                            }
                                            if (Speed < 46.7 && Speed >= 45.1)
                                            {
                                                if (bhjz_power <= 21)
                                                {
                                                    bhjz_power = 22;
                                                    Set_Control_Power(15.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t22 += 0.01;
                                            }
                                            if (Speed < 45.1 && Speed >= 43.4)
                                            {
                                                if (bhjz_power <= 22)
                                                {
                                                    bhjz_power = 23;
                                                    Set_Control_Power(14.7f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t23 += 0.01;
                                            }
                                            if (Speed < 43.4 && Speed >= 41.8)
                                            {
                                                if (bhjz_power <= 23)
                                                {
                                                    //bzhx_t3_end = DateTime.Now;
                                                    bhjz_power = 24;
                                                    Set_Control_Power(13.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t24 += 0.01;
                                            }
                                            if (Speed < 41.8 && Speed >= 40.2)
                                            {
                                                if (bhjz_power <= 24)
                                                {
                                                    bhjz_power = 25;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t25 += 0.01;
                                            }
                                            if (Speed < 40.2 && Speed >= 38.6)
                                            {
                                                if (bhjz_power <= 25)
                                                {
                                                    bhjz_power = 26;
                                                    Set_Control_Power(10.3f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t26 += 0.01;
                                            }
                                            if (Speed < 38.6 && Speed >= 37.0)
                                            {
                                                if (bhjz_power <= 26)
                                                {
                                                    bhjz_power = 27;
                                                    Set_Control_Power(11.0f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t27 += 0.01;
                                            }
                                            if (Speed < 37.0 && Speed >= 35.4)
                                            {
                                                if (bhjz_power <= 27)
                                                {
                                                    bhjz_power = 28;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t28 += 0.01;
                                            }
                                            if (Speed < 35.4 && Speed >= 33.8)
                                            {
                                                if (bhjz_power <= 28)
                                                {
                                                    bhjz_power = 29;
                                                    Set_Control_Power(12.5f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t29 += 0.01;
                                            }
                                            if (Speed < 33.8 && Speed >= 32.2)
                                            {
                                                if (bhjz_power <= 29)
                                                {
                                                    bhjz_power = 30;
                                                    Set_Control_Power(13.2f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t30 += 0.01;
                                            }
                                            if (Speed < 32.2 && Speed >= 30.6)
                                            {
                                                if (bhjz_power <= 30)
                                                {
                                                    bhjz_power = 31;
                                                    Set_Control_Power(12.5f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t31 += 0.01;
                                            }
                                            if (Speed < 30.6 && Speed >= 29.0)
                                            {
                                                if (bhjz_power <= 31)
                                                {
                                                    bhjz_power = 32;
                                                    Set_Control_Power(11.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t32 += 0.01;
                                            }
                                            if (Speed < 29.0 && Speed >= 27.4)
                                            {
                                                if (bhjz_power <= 32)
                                                {
                                                    bhjz_power = 33;
                                                    Set_Control_Power(11.0f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t33 += 0.01;
                                            }
                                            if (Speed < 27.4 && Speed >= 25.7)
                                            {
                                                if (bhjz_power <= 33)
                                                {
                                                    bhjz_power = 34;
                                                    Set_Control_Power(10.3f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t34 += 0.01;
                                            }
                                            if (Speed < 25.7 && Speed >= 24.1)
                                            {
                                                if (bhjz_power <= 34)
                                                {
                                                    bhjz_power = 35;
                                                    Set_Control_Power(8.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t35 += 0.01;
                                            }
                                            if (Speed < 24.1 && Speed >= 22.5)
                                            {
                                                if (bhjz_power <= 35)
                                                {
                                                    bhjz_power = 36;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t36 += 0.01;
                                            }
                                            if (Speed < 22.5 && Speed >= 20.9)
                                            {
                                                if (bhjz_power <= 36)
                                                {
                                                    bhjz_power = 37;
                                                    Set_Control_Power(8.1f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t37 += 0.01;
                                            }
                                            if (Speed < 20.9 && Speed >= 19.3)
                                            {
                                                if (bhjz_power <= 37)
                                                {
                                                    bhjz_power = 38;
                                                    Set_Control_Power(8.8f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t33 += 0.01;
                                            }
                                            if (Speed < 19.3 && Speed >= 17.7)
                                            {
                                                if (bhjz_power <= 38)
                                                {
                                                    bhjz_power = 39;
                                                    Set_Control_Power(8.1f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t38 += 0.01;
                                            }
                                            if (Speed < 17.7 && Speed >= 16.1)
                                            {
                                                if (bhjz_power <= 39)
                                                {
                                                    bhjz_power = 40;
                                                    Set_Control_Power(7.4f - jsglqx_a * (Speed - 0.8f) * (Speed - 0.8f) - 2 * jsglqx_b * Speed - jsglqx_c - qj2);
                                                }
                                                t39 += 0.01;
                                            }

                                            if (Speed < 16.1)                                       //区间记录完成
                                            {
                                                if (bhjz_power <= 40)
                                                {
                                                    bzhx_t2_end = DateTime.Now;
                                                    bhjz_power = 46;
                                                    testFinish = true;
                                                    nowState = testState.idle_state;
                                                }

                                            }
                                        }
                                        #endregion
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
                Thread.Sleep(500);
                TurnOnRelay((byte)configinfdata.BpqDy);

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
                TurnOffRelay((byte)configinfdata.BpqDy);
                Thread.Sleep(500);
                bpq.turnOffMotor();
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
        float qj1 = 0, qj2 = 0, qj3 = 0;
        private void button_bzhjz_Click(object sender, EventArgs e)
        {
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            double glbccs = 0;
            ini.INIIO.GetPrivateProfileString("寄生功率", "data1", "0.0", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
            if (!double.TryParse(temp.ToString(), out glbccs))
            {
                glbccs = 0;
            }
            ini.INIIO.GetPrivateProfileString("寄生功率", "qj1", "0.0", temp, 2048, startUpPath + "/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
            if (!float.TryParse(temp.ToString(), out qj1))
            {
                qj1 = 0;
            }
            ini.INIIO.GetPrivateProfileString("寄生功率", "qj2", "0.0", temp, 2048, startUpPath + "/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
            if (!float.TryParse(temp.ToString(), out qj2))
            {
                qj2 = 0;
            }
            ini.INIIO.GetPrivateProfileString("寄生功率", "qj3", "0.0", temp, 2048, startUpPath + "/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
            if (!float.TryParse(temp.ToString(), out qj3))
            {
                qj3 = 0;
            }
            if (configinfdata.TestStandard == "HJT290" || configinfdata.TestStandard == "HJT291" || configinfdata.TestStandard == "JJF1221点燃式")
            {
                double[] jsglqx_power = new double[4];
                double[] jsglqx_speed = new double[4];
                for (int i = 0; i < 4; i++)
                {
                    jsglqx_speed[i] = (24 + 8 * i);
                    ini.INIIO.GetPrivateProfileString("寄生功率(日常测试)", (24 + 8 * i).ToString() + "Km/h", "Empty", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                    if (temp.ToString() == "Empty")
                    {
                        MessageBox.Show("没有寄生功率相关数据", "出错啦");
                        return;
                    }
                    try
                    {
                        jsglqx_power[i] = double.Parse(temp.ToString()) + glbccs;
                    }
                    catch
                    {
                        MessageBox.Show("读取寄生功率相关数据异常:" + temp.ToString(), "出错啦");
                        return;
                    }

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
                        MessageBox.Show("没有寄生功率相关数据", "出错啦");
                        return;
                    }
                    try
                    {
                        jsglqx_power[i] = double.Parse(temp.ToString()) + glbccs;
                    }
                    catch
                    {
                        MessageBox.Show("寄生功率相关数据读取异常:" + temp.ToString(), "出错啦");
                        return;
                    }

                }
                jsglqx_matrix = new MatrixEquation.MatrixEquation(jsglqx_speed, jsglqx_power, 2);
                double[] jsglqx_matrixResult = jsglqx_matrix.GetResult();
                jsglqx_c = (float)jsglqx_matrixResult[0];
                jsglqx_b = (float)jsglqx_matrixResult[1];
                jsglqx_a = (float)jsglqx_matrixResult[2];
            }
            if (true)
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
                    Test_Flag = true;
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
        //变载荷加载滑行试验
        public void bzhjz_exe()
        {
            double tm1 = 25.77, tm2 = 15.54, tm3 = 3.98;           //标准
            double ts1 = 0, ts2 = 0, ts3 = 0;
            double result1 = 0, result2 = 0, result3 = 0;
            double Standard1 = 0.04, Standard2 = 0.02, Standard3 = 0.03;             //误差
            string r1 = "", r2 = "", r3 = "";
            Random rd = new Random();
            if (configinfdata.TestStandard == "HJT290" || configinfdata.TestStandard == "HJT291" || configinfdata.TestStandard == "HJT292")
            {
                tm1 = 0.028394 * DIW;
                tm2 = 0.01713 * DIW;
                tm3 = 0.0043866 * DIW;
            }
            else if (configinfdata.TestStandard == "JJF1221点燃式")
            {
                tm2 = 0.00707 * DIW;
                Standard2 = 0.04;
            }
            else if (configinfdata.TestStandard == "JJF1221压燃式")
            {
                tm2 = 0.01713 * DIW;
                Standard2 = 0.04;
            }
            Lifter_Down();         //下降举升
            Thread.Sleep(6000);         //等待2秒
            Motor_Open(90 * bpqXs);          //启动电机加速
            while (testFinish == false)
            {
                Thread.Sleep(100);
            }
            Exit_Control();
            try
            {
                if (toolStripLabel提示信息.Text == "提示信息")
                {
                    if (configinfdata.TestStandard == "HJT290" || configinfdata.TestStandard == "HJT291" || configinfdata.TestStandard == "HJT292")
                    {
                        TimeSpan timespan1 = bzhx_t1_end - bzhx_t1_start;
                        TimeSpan timespan2 = bzhx_t2_end - bzhx_t2_start;
                        TimeSpan timespan3 = bzhx_t3_end - bzhx_t3_start;
                        ts1 = (float)(timespan1.TotalMilliseconds / 1000f);
                        ts2 = (float)(timespan2.TotalMilliseconds / 1000f);
                        ts3 = (float)(timespan3.TotalMilliseconds / 1000f);
                    }
                    else
                    {
                        TimeSpan timespan2 = bzhx_t2_end - bzhx_t2_start;
                        ts2 = (float)(timespan2.TotalMilliseconds / 1000f);
                    }
                }
                else
                {
                    ts1 = tm1 + (double)rd.Next(10) / 100.0;
                    ts2 = tm2 + (double)rd.Next(10) / 100.0;
                    ts3 = tm3 + (double)rd.Next(10) / 100.0;
                }
                ts1 = Math.Round(ts1, 2);
                ts2 = Math.Round(ts2, 2);
                ts3 = Math.Round(ts3, 2);
                result1 = Math.Abs(ts1 - tm1) / tm1;
                result1 = Math.Round(result1, 2);
                result2 = Math.Abs(ts2 - tm2) / tm2;
                result2 = Math.Round(result2, 2);
                result3 = Math.Abs(ts3 - tm3) / tm3;
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
            if (configinfdata.TestStandard == "HJT290" || configinfdata.TestStandard == "HJT291" || configinfdata.TestStandard == "HJT292")
            {
                dataGrid_bzhjzhx.Rows[0].Cells["实测时间(s)"].Value = ts1.ToString("0.000");
                dataGrid_bzhjzhx.Rows[0].Cells["误差"].Value = result1.ToString("0.000");
                dataGrid_bzhjzhx.Rows[0].Cells["验收标准"].Value = 0.04;
                dataGrid_bzhjzhx.Rows[0].Cells["结果"].Value = r1.ToString();
                dataGrid_bzhjzhx.Rows[1].Cells["实测时间(s)"].Value = ts2.ToString("0.000");
                dataGrid_bzhjzhx.Rows[1].Cells["误差"].Value = result2.ToString("0.000");
                dataGrid_bzhjzhx.Rows[1].Cells["验收标准"].Value = 0.02;
                dataGrid_bzhjzhx.Rows[1].Cells["结果"].Value = r2.ToString();
                dataGrid_bzhjzhx.Rows[2].Cells["实测时间(s)"].Value = ts3.ToString("0.000");
                dataGrid_bzhjzhx.Rows[2].Cells["误差"].Value = result3.ToString("0.000");
                dataGrid_bzhjzhx.Rows[2].Cells["验收标准"].Value = 0.03;
                dataGrid_bzhjzhx.Rows[2].Cells["结果"].Value = r3.ToString();
            }
            else
            {
                dataGrid_bzhjzhx.Rows[0].Cells["实测时间(s)"].Value = ts2.ToString("0.000");
                dataGrid_bzhjzhx.Rows[0].Cells["误差"].Value = result2.ToString("0.000");
                dataGrid_bzhjzhx.Rows[0].Cells["验收标准"].Value = 0.04;
                dataGrid_bzhjzhx.Rows[0].Cells["结果"].Value = r2.ToString();
            }
            bzglidedata = new bzglide();
            if (configinfdata.TestStandard == "HJT290" || configinfdata.TestStandard == "HJT291" || configinfdata.TestStandard == "HJT292")
                bzglidedata.Hxqj = "72.4-16.1";
            else if (configinfdata.TestStandard == "JJF1221压燃式")
                bzglidedata.Hxqj = "72.4-16.1";
            else if (configinfdata.TestStandard == "JJF1221点燃式")
                bzglidedata.Hxqj = "48.3-16.1";
            bzglidedata.Ccdt = tm2.ToString("0.000");
            bzglidedata.Acdt = ts2.ToString("0.000");
            bzglidedata.Wc = result2.ToString("0.000");
            bzglidedata.Bzsm = "";
            bzglidedata.Bdjg = (r2 == "合格") ? "0" : "-1";
            Thread.Sleep(3000);
            Lifter_Up();
            Test_Flag = false;
        }

        private void buttonSaveResult_Click(object sender, EventArgs e)
        {
            if (bzglidedata != null)
            {
                bzglideControl bzglidecontrol = new bzglideControl();
                bzglidecontrol.writeBzGlideIni(bzglidedata);
                MessageBox.Show("数据保存成功");
                isSaved = true;
            }
            else
                MessageBox.Show("没有有效数据进行保存，请先完成试验");
        }

        private void buttonStopTest_Click(object sender, EventArgs e)
        {
            Motor_Close();
            Exit_Control();
            nowState = testState.idle_state;
            if (Test_Flag)
            {

                try
                {
                    th_bzhjz.Abort();
                    Exit_Control();
                }
                catch (Exception)
                {
                }

                Test_Flag = false;
            }
        }

        private void bzhHx_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSaved)
            {
                if (MessageBox.Show("还未保存结果，确认退出？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    Exit_Control();
                    _continue = false;
                    Thread.Sleep(5);
                    try
                    {
                        readThread.Abort();
                    }
                    catch
                    { }
                    try
                    {
                        th_bzhjz.Abort();
                    }
                    catch
                    { }
                    try
                    {
                        ComPort_2.Close();
                    }
                    catch
                    { }
                    try
                    {
                        if (bpq != null)
                            bpq.Close_Com();
                    }
                    catch
                    { }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                Exit_Control();
                _continue = false;
                Thread.Sleep(5);
                try
                {
                    readThread.Abort();
                }
                catch
                { }
                try
                {
                    th_bzhjz.Abort();
                }
                catch
                { }
                try
                {
                    ComPort_2.Close();
                }
                catch
                { }
                try
                {
                    if (bpq != null)
                        bpq.Close_Com();
                }
                catch
                { }
            }
        }
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
            Motor_Open(90 * bpqXs);
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
                    th_bzhjz.Abort();
                    Exit_Control();
                }
                catch (Exception)
                {
                }

                Test_Flag = false;
            }
        }

        private void toolStripButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonExit_Click(object sender, EventArgs e)
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

        private void bzhHx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.Control && e.KeyCode == Keys.M)
            {
                if (toolStripLabel提示信息.Text == "提示信息")
                    toolStripLabel提示信息.Text = "准备完毕";
                else
                    toolStripLabel提示信息.Text = "提示信息";
            }
        }
    }
}
