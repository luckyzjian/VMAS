using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using carinfor;
using Dynamometer;
using SYS.Model;
using SYS_DAL;

namespace ASMtest
{
    public partial class ASM : Form
    {
        int testvalue = 0;
        carinfor.carInidata carbj = new carInidata();
        equipmentConfigInfdata equipconfig = new equipmentConfigInfdata();
        public static AsmConfigInfdata asmconfig = new AsmConfigInfdata();
        carIni carini = new carIni();
        configIni configini = new configIni();
        statusconfigIni statusconfigini = new statusconfigIni();
        asmdata asm_data = new asmdata();
        asmdataControl asmdatacontrol = new asmdataControl();
        vmasDataSeconds vmas_dataseconds = new vmasDataSeconds();
        vmasDataSecondsControl vmas_datasecondscontrol = new vmasDataSecondsControl();
        JCXXX jcxxx = new JCXXX();
        jcxztCheck jcxzt = new jcxztCheck();
        CSVcontrol.CSVwriter csvwriter = new CSVcontrol.CSVwriter();
        CSVcontrol.csvReader csvreader = new CSVcontrol.csvReader();
        upanControl.ucontrol u_control = new upanControl.ucontrol();
        string dogUse = "";
        DataTable dt = new DataTable();
        DataTable dt_zb = null;
        private string UseFqy = "";
        Exhaust.Fla501 fla_501 = null;
        Exhaust.Fla502 fla_502 = null;
        Exhaust.FLB_100 flb_100 = null;
        Exhaust.Flv_1000 flv_1000 = null;
        Exhaust.XCE_100 xce_100 = null;
        Exhaust.Nhsjz nhsjz = null;
        thaxs thaxsdata = new thaxs();
        //LedControl.led ledcontrol = null;
        speed_data speedNow = new speed_data();//提前显示前4秒的状态
        speed_data speedLimit = new speed_data();
        static public IGBT igbt = null;
        int gksj_count = 0;
        public CSVcontrol.CSVHelper vmas_csv = null;
        private bool isVertical = false;//是否为竖状
        private bool isUseData = false;

        private bool chujianIsFinished = false;
        private bool chujianIsOk = false;

        private bool ig195IsFinished = false;
        private bool writeDataIsFinished = false;
        private bool wsd_sure = false;
        private bool wsdValueIsRight = false;

        public delegate void wl_led(LEDNumber.LEDNumber lednumber, string data);
        public delegate void wtcs(Control controlname, string text);                            //委托
        public delegate void wtds(double Data, double time, string ChartName);                               //委托
        public delegate void wtds2(double Data, double time, string ChartName);                               //委托
        public delegate void wtds_clr(string ChartName);                               //委托
        public delegate void wt_void();                                                         //委托
        public delegate void wtlsb(Label Msgowner, string Msgstr, bool Update_DB);                //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                              //委托
        public delegate void wtlm(Label Msgowner, string Msgstr);
        public delegate void wtdtview(DataGridView datagridview, string title, int row_number, string message);
        public delegate void wtpanelvisible(Panel panel, bool visible_value);
        public delegate void wttextboxenable(TextBox textbox, bool visible_value);
        public delegate void wttextboxvalue(TextBox textbox, string value);

        //public delegate void wtds(double Data,string SeriesName);                               //委托
        public delegate void wtdd(double min, double max);                                      //委托
        public delegate void wtddy(double min, double max);                                      //委托
        public bool JC_Status = false;                                                          //检测状态
        public bool saveProcessData = false;
        public string JC_Circuit = "5025";                                                        //检测流程
        Thread Th_get_FqandLl = null;                                                           //废气检测线程
        Thread TH_ST = null;                                                                    //检测线程
        
        public int fqy_delayTime = 0;
        public int Preset_Time = 10;                                                            //废气分析仪预置时间，默认10秒
        public float[] Speed_listIg195 = new float[300];                                          //IG195的每秒速度
        //2540工况每秒速度
        public double[] Pd ={0.61129,0.65716,0.70605,0.75813,0.81359,
                           0.87260,0.93537,1.0021,1.0730,1.1482,
                           1.2281,1.3129,1.4027,1.4979,1.5988,
                           1.7056,1.8185,1.9380,2.0644,2.1978,
                           2.3388,2.4877,2.6447,2.8104,2.9850,
                           3.1690,3.3629,3.5670,3.7818,4.0078,
                           4.2455};                                                             //0到30度的饱和蒸汽压
        public double[] temperatureEveryMonth = { -4, 1, 9, 17, 25, 29, 30, 28, 23, 16, 6, -2 };
        public static double WD = 0;                                                                  //温度
        public static double SD = 0;                                                                  //相对湿度
        public static double DQY = 0;                                                                //大气压
        public float hjo2 = 20.8f;                                                               //环境O2浓度
        public float[] hjo2data = new float[10];
        public Exhaust.Fla502_data[] Vmas_Exhaust_ListIG195 = new Exhaust.Fla502_data[300];      //IG195工况每秒废气结果
        public Exhaust.Fla502_data[] Vmas_Exhaust_Revise_ListIG195 = new Exhaust.Fla502_data[300];//IG195工况每秒废气修正后结果
        public static Exhaust.Fla502_data Vmas_Exhaust_Now = new Exhaust.Fla502_data();
        public Exhaust.Fla502_data Vmas_Exhaust_ReviseNow = new Exhaust.Fla502_data();
        public Exhaust.Fla502_temp_data fla502_temp_data = new Exhaust.Fla502_temp_data();
        public string[] Vmas_qcsj = new string[300];//全程时序
        public string[] Vmas_cysx = new string[300];
        public string[] Vmas_sxnb = new string[300];
        public float[] Vmas_Exhaust_llList = new float[300];//流量计实际流量
        public float[] Vmas_Exhaust_xso2List = new float[300];//流量计稀释O2浓度
        public float[] Vmas_Exhaust_lljtemp = new float[300];//流量计温度
        public float[] Vmas_Exhaust_lljyl = new float[300];//流量计压力
        public float[] Vmas_Exhaust_lljbzll = new float[300];//流量计标准流量
        public float[] Vmas_Exhaust_fqsjll = new float[300];//废气实际流量
        public float[] Vmas_Exhaust_k = new float[300];//稀释比每秒数据
        public float[] Vmas_Exhaust_qlyl = new float[300];
        public bool[] Vmas_Exhaust_accelerate = new bool[300];//稀释比每秒数据
        public float[] Vmas_Exhaust_cozl = new float[300];//每秒CO质量
        public float[] Vmas_Exhaust_co2ld = new float[300];//标准车速
        public float[] Vmas_Exhaust_cold = new float[300];//实时车速
        public float[] Vmas_Exhaust_hcld = new float[300];//实时车速
        public float[] Vmas_Exhaust_nold = new float[300];//实时车速
        public float[] Vmas_Exhaust_o2ld = new float[300];//实时车速
        public float[] Vmas_Exhaust_lambda = new float[300];
        public float[] Vmas_bzcs = new float[300];//标准车速
        public float[] Vmas_sscs = new float[300];//实时车速
        public float[] Vmas_jzgl = new float[300];//加载功率
        public float[] Vmas_nl = new float[300];//加载功率
        public float[] Vmas_hjwd = new float[300];//环境温度
        public float[] Vmas_xdsd = new float[300];//相对湿度
        public float[] Vmas_dqyl = new float[300];//大气压力
        public float[] Vmas_sdxzxs = new float[300];//湿度修正系数
        public float[] Vmas_xsxzxs = new float[300];//稀释修正系数
        public float[] Vmas_fxyglyl = new float[300];//分析仪管路压力
        public float[] Vmas_Exhaust_hczl = new float[300];//每秒HC质量
        public float[] Vmas_Exhaust_nozl = new float[300];//每秒NO质量
        public float[] Vmas_Exhaust_zs = new float[300];//每秒NO质量
        public float[] Vmas_Exhaust_yw = new float[300];//每秒NO质量
        public float[] Vmas_Exhaust_jsgl = new float[300];//每秒NO质量
        public float[] vmas_zsgl = new float[300];
        public int[] Vmas_jczt=new  int[300];

        private double[] asm_Exhaust_rev5025 = new double[90];
        private double[] asm_Exhaust_lambda5025 = new double[90];
        private double[] asm_Exhaust_rev2540 = new double[90];
        private double[] asm_Exhaust_lambda2540 = new double[90];

        List<string> other_Vmas_qcsj=new List<string>();//全程时序
        List<string> other_Vmas_cysx = new List<string>();
        List<string> other_Vmas_sxnb = new List<string>();
        List<double> other_Vmas_Exhaust_co2ld = new List<double>();//标准车速
        List<double> other_Vmas_Exhaust_cold = new List<double>();//实时车速
        List<double> other_Vmas_Exhaust_hcld = new List<double>();//实时车速
        List<double> other_Vmas_Exhaust_nold = new List<double>();//实时车速
        List<double> other_Vmas_Exhaust_o2ld = new List<double>();//实时车速
        List<double> other_Vmas_Exhaust_lambda = new List<double>();
        List<double> other_Vmas_Exhaust_qlyl = new List<double>();//标准车速
        List<double> other_Vmas_bzcs = new List<double>();//标准车速
        List<double> other_Vmas_sscs = new List<double>();//实时车速
        List<double> other_Vmas_jzgl = new List<double>();//加载功率
        List<double> other_Vmas_nl = new List<double>();//加载功率
        List<double> other_Vmas_hjwd = new List<double>();//环境温度
        List<double> other_Vmas_xdsd = new List<double>();//相对湿度
        List<double> other_Vmas_dqyl = new List<double>();//大气压力
        List<double> other_Vmas_sdxzxs = new List<double>();//湿度修正系数
        List<double> other_Vmas_xsxzxs = new List<double>();//稀释修正系数
        List<double> other_Vmas_fxyglyl = new List<double>();//分析仪管路压力
        List<double> other_Vmas_Exhaust_zs = new List<double>();//每秒NO质量
        List<double> other_Vmas_Exhaust_yw = new List<double>();//每秒NO质量
        List<double> other_Vmas_Exhaust_jsgl = new List<double>();//每秒NO质量
        List<double> other_vmas_zsgl = new List<double>();
        List<double> other_Vmas_jczt = new List<double>();
        

        public int[] lowflowarray = new int[300];
        public int isLowFlow = 0;
        private float yw = 0;
        private bool jcStatus = false;
        private string djccyString = "";
        private string sjxlString = "";
        private string hcclzString = "";
        private string noclzString = "";
        private string coclzString = "";
        private string co2clzString = "";
        private string o2clzString = "";
        private string csString = "";
        private string zsString = "";
        private string xsxzxsString = "";
        private string sdxzxsString = "";
        private string jsglString = "";
        private string zsglString = "";
        private string hjwdString = "";
        private string dqylString = "";
        private string xdsdString = "";
        private string ywString = "";
        private string jcztString = "";

        private List<float> asm_Exhaust_co5025 = new List<float>();
        private List<float> asm_Exhaust_hc5025 = new List<float>();
        private List<float> asm_Exhaust_no5025 = new List<float>();
        private List<float> asm_Exhaust_co25025 = new List<float>();
        private List<float> asm_Exhaust_o25025 = new List<float>();
        private List<int> asm_Exhaust_gksj5025 = new List<int>();

        private List<float> asm_Exhaust_hc2540 = new List<float>();
        private List<float> asm_Exhaust_co2540 = new List<float>();
        private List<float> asm_Exhaust_no2540 = new List<float>();
        private List<float> asm_Exhaust_co22540 = new List<float>();
        private List<float> asm_Exhaust_o22540 = new List<float>();
        private List<int> asm_Exhaust_gksj2540 = new List<int>();


        private bool exhaustIsCb5025 = false;
        private bool exhaustIsCb2540 = false;
        private float exhaustCoCb = 0f;
        private float exhaustNoCb = 0f;
        private float exhaustHcCb = 0f;
        private float exhaustCo2Cb = 0f;
        private float exhaustO2Cb = 0f;

        private float jsgl_xsa = 0;
        private float jsgl_xsb = 0;
        private float jsgl_xsc = 0;

        public float cozl_zb = 0f;
        public float hczl_zb = 0f;
        public float nozl_zb = 0f;
        private int sxnb = 0;//时序类别，0：检测前准备  1：怠速准备  2：检测过程

        private DateTime startTime, nowtime;
        private float sdxzxs, xsxzxs;
        public string Cllx = ""; //车辆类型
        //老车标准
        public static float Limit_HC_BEBORE = 1;                                                         //5025HC限值
        public static float Limit_CO_BEBORE = 1;                                                         //5025CO限值
        public static float Limit_NO_BEBORE = 1;
        //新车标准
        public static float Limit_HCNOX_AFTER = 1;
        public static float Limit_CO_AFTER = 1;//5025NO限值
        public static bool accelerate_flag = false;
        //2540NO限值
        public string HC_IG195_JG = "";                                                          //5025HC结果
        public string CO_IG195_JG = "";                                                          //5025CO结果
        public string NOX_IG195_JG = "";                                                          //5025NO结果
        public string VASM_PD = "";                                                              //ASM总结果
        public int GKSJ = 0;                                                                    //工况时间
        public int Jcjg_Status = 0;                                                             //检测结果状态 0为不合格、1为5025快速工况合格、2为5025工况工况合格、3为2540快速工况合格、4为2540工况合格、5为5025工况重新计时、6为2540工况重新计时、7为检测终止。
        private float distanceInTheory = 1.013f;//循环理论行驶距离
        public bool Speed_Jc_flag = false;                                                      //速度检查标记
        public bool Speed_flag = false;                                                         //快速工况速度超过限制标记
        public float outTimeContinus = 0;                                                               //连续超差的时间
        public float outTimeTotal = 0;                                                                   //总共超差的时间
        public float Set_Power5025 = 0, Set_Power2540 = 0;                                                             //扭矩
        public static bool beforedate = true;
        public enum TEST_STATE { STATE_PREP,STATE_ACC5025,STATE_TEST5025,STATE_ACC2540,STATE_TEST2540,STATE_FINISH};
        public static TEST_STATE testState = TEST_STATE.STATE_PREP;

        public float gongkuangTime = 0f;
        public string gongkuangStartTime = "";
        public string gongkuangEndTime = "";

        public float jcTime = 0f;
        private DateTime jcStarttime;
        public int JCSJ = 0;
        public int preJCSJ = 0;

        private static float md_co = 101325 * 0.01f * 28 / 8.31f / 273.15f;//mg
        private static float md_hc = 101325 * 0.000001f * 86 / 8.31f / 273.15f;//mg
        private static float md_no = 101325 * 0.000001f * 30 / 8.31f / 273.15f;//mg

        public float co_zl = 0f;
        public float hc_zl = 0f;
        public float no_zl = 0f;

        public bool lxcc_flag = false;
        public bool llcc_flag = false;
        public bool power_flag = false;
        public string jctime = "";

        public static System.Windows.Forms.Screen[] sc;
        public static int sc_width = 0;
        public static int sc_height = 0;

        private bool isSongpin = false;

        private int rlzl = 0;//0为气油1为柴油 2为LPG 3为NG 4为双燃料
        private int jczt = 0;

        public static string ts1 = "川AV7M82";
        public static string ts2 = "5025工况";
        public static string ts3 = "";
        public static bool driverformmin = false;
        private bool isautostart = true;



        /// 设置在第几个屏幕上启动。
        /// </summary>
        /// <param name="screen">屏幕(从0开始)</param>
        /// <param name="form">要启动的程序。</param>
        public void FormStartScreen(int screen, Form form)
        {
            if (Screen.AllScreens.Length <= 1)
                return;
            if (Screen.AllScreens.Length < screen)
                return;
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new System.Drawing.Point(Screen.AllScreens[screen].Bounds.X, Screen.AllScreens[screen].Bounds.Y);
            form.WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// 默认在第1一个扩展屏幕上打开。
        /// </summary>
        /// <param name="form">要启动的程序。</param>
        public void FormStartScreen(Form form)
        {
            FormStartScreen(1, form);
        }

        public struct speed_data
        {
            public float speed_now_data;
            public bool isChangeD;
            //Color speed_now_color;
        };

        public ASM()
        {
            String[] CmdArgs = System.Environment.GetCommandLineArgs();
            if (CmdArgs.Length <= 1)
            {
                isautostart = true;
            }
            else
            {
                if (CmdArgs[0] == "auto")
                    isautostart = true;
            }
            InitializeComponent();
            this.SetStyle(
                  ControlStyles.UserPaint |
                  ControlStyles.AllPaintingInWmPaint |
                  ControlStyles.OptimizedDoubleBuffer |
                  ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Opaque, false);
            this.UpdateStyles();
        }
        private bool IsUseTpTemp = false;
        private void Vmas_Load(object sender, EventArgs e)
        {
            panel_chujian.Visible = false;
            initCarInfo();
            initConfigInfo();
            initEquipment();
            initJsglXs();
            initChujian();
            Init_Data();        //初始化数据
            Init_power();
            timer1.Interval = 100;
            timer1.Start();
            /*if (igbt != null)
            {
                igbt.Lifter_Down();
                Thread.Sleep(100);
                //igbt.Set_Carjzzl(carbj.CarJzzl);
            }*/
            jcStarttime = DateTime.Now;
            //timer3.Start();
            if (!isautostart)
            {
                prepareFoem prepareform = new prepareFoem();
                prepareform.ShowDialog();
            }
            //carbj.CarPH = "川AV7M82";
            ts1 = carbj.CarPH;
            ts2 = "稳态工况法";
            ts3 = "CO:-- HC:-- NOx:-- CO2:-- O2:--T:-- H:-- A:-- ";
            if (equipconfig.DisplayMethod == "扩展")
            {
                if (equipconfig.DriverFbl == 0)
                {
                    DriverShow showform = new DriverShow();
                    FormStartScreen(equipconfig.DriverDisplay, showform);
                    showform.Show();
                }
                else
                {
                    DriverShow1366 showform = new DriverShow1366();
                    FormStartScreen(equipconfig.DriverDisplay, showform);
                    showform.Show();
                }
            }
            if (equipconfig.isTpTempInstrument)
            {
                if (File.Exists("C://jcdatatxt/环境数据.ini"))
                {
                    //string wd, sd, dqy;
                    StringBuilder temp = new StringBuilder();
                    temp.Length = 2048;
                    ini.INIIO.GetPrivateProfileString("环境数据", "wd", "", temp, 2048, "C:/jcdatatxt/环境数据.ini");
                    WD = double.Parse(temp.ToString());
                    ini.INIIO.GetPrivateProfileString("环境数据", "sd", "", temp, 2048, "C:/jcdatatxt/环境数据.ini");
                    SD = double.Parse(temp.ToString());
                    ini.INIIO.GetPrivateProfileString("环境数据", "dqy", "", temp, 2048, "C:/jcdatatxt/环境数据.ini");
                    DQY = double.Parse(temp.ToString());
                    IsUseTpTemp = true;
                }
            }

            if (carbj.ISUSE)
            {
                fla_502.coxs = carbj.ASM_CO;
                fla_502.hcxs = carbj.ASM_HC;
                fla_502.noxs = carbj.ASM_NO;
            }
            if (isautostart)
            {
                Thread.Sleep(3000);
                button_ss_Click_1(sender, e);
            }
        }
        private void initJsglXs()
        {
            try
            {
                StringBuilder temp = new StringBuilder();
                temp.Length = 2048;
                ini.INIIO.GetPrivateProfileString("寄生功率系数", "a", "0.0006", temp, 2048, Application.StartupPath + @"\detectConfig.ini");
                jsgl_xsa = float.Parse(temp.ToString().Trim());
                ini.INIIO.GetPrivateProfileString("寄生功率系数", "b", "0.0003", temp, 2048, Application.StartupPath + @"\detectConfig.ini");
                jsgl_xsb = float.Parse(temp.ToString().Trim());
                ini.INIIO.GetPrivateProfileString("寄生功率系数", "c", "0", temp, 2048, Application.StartupPath + @"\detectConfig.ini");
                jsgl_xsc = float.Parse(temp.ToString().Trim());
                jsgl_xsc = 0;
            }
            catch
            {
                MessageBox.Show("寄生功率系数初始化失败", "错误");
                jsgl_xsa = 0;
                jsgl_xsb = 0;
                jsgl_xsc = 0;
            }
        }
        private void load_progress()
        {
            load_display new_loadProgress = new load_display();
            new_loadProgress.Show();
            Thread.Sleep(100);
            while (!writeDataIsFinished)
            {
                new_loadProgress.progress_show();
            }
            new_loadProgress.Close();

        }
        public void Init_sc()
        {
            sc = System.Windows.Forms.Screen.AllScreens;
            sc_height = this.Height;
            sc_width = this.Width;
        }
        public void initEquipment()
        {
            bool Init_flag = true;
            string init_message = "";
            try
            {
                if (equipconfig.Fqyifpz == true)
                {
                    switch (equipconfig.Fqyxh.ToLower())           //通过仪器型号选择初始化项目
                    {
                        case "fla_502":
                            try
                            {
                                UseFqy = "fla_502";
                                fla_502 = new Exhaust.Fla502(equipconfig.Fqyxh);
                                if (fla_502.Init_Comm(equipconfig.Fqyck, equipconfig.Fqyckpzz) == false)
                                {
                                    fla_502 = null;
                                    Init_flag = false;
                                    init_message = "废气仪串口打开失败.";
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                fla_502 = null;
                                Init_flag = false;
                            }
                            break;
                        case "mqw_50a":
                        case "mqw_50b":
                            try
                            {
                                UseFqy = "mqw_50a";
                                fla_502 = new Exhaust.Fla502(equipconfig.Fqyxh);
                                if (fla_502.Init_Comm(equipconfig.Fqyck, equipconfig.Fqyckpzz) == false)
                                {
                                    fla_502 = null;
                                    Init_flag = false;
                                    init_message = "废气仪串口打开失败.";
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                fla_502 = null;
                                Init_flag = false;
                            }
                            break;
                        case "fasm_5000":
                            try
                            {
                                UseFqy = "fasm_5000";
                                fla_502 = new Exhaust.Fla502(equipconfig.Fqyxh);
                                if (fla_502.Init_Comm(equipconfig.Fqyck, equipconfig.Fqyckpzz) == false)
                                {
                                    fla_502 = null;
                                    Init_flag = false;
                                    init_message = "废气仪串口打开失败.";
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                fla_502 = null;
                                Init_flag = false;
                            }
                            break;
                        case "nha_503":
                            try
                            {
                                UseFqy = "fla_502";
                                fla_502 = new Exhaust.Fla502(equipconfig.Fqyxh);
                                fla_502.isNhSelfUse = equipconfig.isFqyNhSelfUse;
                                if (fla_502.Init_Comm(equipconfig.Fqyck, equipconfig.Fqyckpzz) == false)
                                {
                                    fla_502 = null;
                                    Init_flag = false;
                                    init_message = "废气仪串口打开失败.";
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                fla_502 = null;
                                Init_flag = false;
                            }
                            break;
                        case "fla_501":
                            try
                            {
                                UseFqy = "fla_501";
                                fla_501 = new Exhaust.Fla501();
                                if (fla_501.Init_Comm(equipconfig.Fqyck, equipconfig.Fqyckpzz) == false)
                                {
                                    fla_501 = null;
                                    Init_flag = false;
                                    init_message = "废气仪串口打开失败.";
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                fla_501 = null;
                                Init_flag = false;
                            }
                            break;

                        case "cdf5000":
                            try
                            {
                                UseFqy = "fla_502";
                                fla_502 = new Exhaust.Fla502(equipconfig.Fqyxh);
                                fla_502.isNhSelfUse = equipconfig.isFqyNhSelfUse;
                                if (fla_502.Init_Comm(equipconfig.Fqyck, equipconfig.Fqyckpzz) == false)
                                {
                                    fla_502 = null;
                                    Init_flag = false;
                                    init_message = "废气仪串口打开失败.";
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                fla_502 = null;
                                Init_flag = false;
                            }
                            break;
                        case "mqw_511":
                            try
                            {
                                UseFqy = "mqw_511";
                                fla_502 = new Exhaust.Fla502(equipconfig.Fqyxh);
                                if (fla_502.Init_Comm(equipconfig.Fqyck, equipconfig.Fqyckpzz) == false)
                                {
                                    fla_502 = null;
                                    Init_flag = false;
                                    init_message = "废气仪串口打开失败.";
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                fla_502 = null;
                                Init_flag = false;
                            }
                            break;
                    }
                }
            }
            catch (Exception)
            {

            }
            try
            {
                if (equipconfig.Ydjifpz == true && equipconfig.Ydjxh != "CDF5000")
                {
                    try
                    {
                        flb_100 = new Exhaust.FLB_100(equipconfig.Ydjxh);
                        flb_100.isNhSelfUse = equipconfig.isYdjNhSelfUse;
                        if (flb_100.Init_Comm(equipconfig.Ydjck, equipconfig.Ydjckpzz) == false)
                        {
                            flb_100 = null;
                            Init_flag = false;
                            init_message += "烟度计串口打开失败.";
                        }
                    }
                    catch (Exception er)
                    {
                        flb_100 = null;
                        Init_flag = false;
                        MessageBox.Show(er.ToString(), "出错啦");
                    }
                }
            }
            catch (Exception)
            {
            }
            //这里只初始化了废气分析仪其他设备要继续初始化
            try
            {
                if (equipconfig.Cgjifpz)
                {
                    try
                    {
                        igbt = new Dynamometer.IGBT("BNTD",equipconfig.isIgbtContainGdyk);
                        if (igbt.Init_Comm(equipconfig.Cgjck, equipconfig.cgjckpzz) == false)
                        {
                            igbt = null;
                            Init_flag = false;
                            init_message += "测功机串口打开失败.";
                        }
                    }
                    catch (Exception er)
                    {
                        igbt = null;
                        Init_flag = false;
                        MessageBox.Show(er.ToString(), "出错啦");
                    }
                }
            }
            catch (Exception)
            {
            }
            try
            {
                if (equipconfig.TempInstrument == "XCE_100")
                {
                    try
                    {
                        xce_100 = new Exhaust.XCE_100("XCE_100");
                        if (xce_100.Init_Comm(equipconfig.Xce100ck, equipconfig.Xce100Comstring) == false)
                        {
                            xce_100 = null;
                            Init_flag = false;
                            init_message += "XCE100串口打开失败.";

                        }
                    }
                    catch (Exception er)
                    {
                        xce_100 = null;
                        Init_flag = false;
                        MessageBox.Show(er.ToString(), "XCE100串口打开出错啦");
                    }
                }
                if (equipconfig.TempInstrument == "XCE_101")
                {
                    try
                    {
                        xce_100 = new Exhaust.XCE_100("XCE_101");
                        if (xce_100.Init_Comm(equipconfig.Xce100ck, equipconfig.Xce100Comstring) == false)
                        {
                            xce_100 = null;
                            Init_flag = false;
                            init_message += "XCE101串口打开失败.";

                        }
                    }
                    catch (Exception er)
                    {
                        xce_100 = null;
                        Init_flag = false;
                        MessageBox.Show(er.ToString(), "XCE101串口打开出错啦");
                    }
                }
                else if (equipconfig.TempInstrument == "DWSP_T5")
                {
                    try
                    {
                        xce_100 = new Exhaust.XCE_100("DWSP_T5");
                        if (xce_100.Init_Comm(equipconfig.Xce100ck, equipconfig.Xce100Comstring) == false)
                        {
                            xce_100 = null;
                            Init_flag = false;
                            init_message += "DWSP_T5串口打开失败.";

                        }
                    }
                    catch (Exception er)
                    {
                        xce_100 = null;
                        Init_flag = false;
                        MessageBox.Show(er.ToString(), "DWSP_T5串口打开出错啦");
                    }
                }
                else if (equipconfig.TempInstrument == "FTH_2")
                {
                    try
                    {
                        xce_100 = new Exhaust.XCE_100("FTH_2");
                        if (xce_100.Init_Comm(equipconfig.Xce100ck, equipconfig.Xce100Comstring) == false)
                        {
                            xce_100 = null;
                            Init_flag = false;
                            init_message += "FTH_2串口打开失败.";

                        }
                    }
                    catch (Exception er)
                    {
                        xce_100 = null;
                        Init_flag = false;
                        MessageBox.Show(er.ToString(), "FTH_2串口打开出错啦");
                    }
                }
                else if (equipconfig.TempInstrument == "RZ_1")
                {
                    try
                    {
                        xce_100 = new Exhaust.XCE_100("RZ_1");
                        if (xce_100.Init_Comm(equipconfig.Xce100ck, equipconfig.Xce100Comstring) == false)
                        {
                            xce_100 = null;
                            Init_flag = false;
                            init_message += "RZ_1串口打开失败.";

                        }
                    }
                    catch (Exception er)
                    {
                        xce_100 = null;
                        Init_flag = false;
                        MessageBox.Show(er.ToString(), "RZ_1串口打开出错啦");
                    }
                }
            }
            catch (Exception)
            {
                xce_100 = null;
                Init_flag = false;
            }

            try
            {
                if (equipconfig.IsUseNhSjz)
                {
                    try
                    {
                        nhsjz = new Exhaust.Nhsjz();
                        if (nhsjz.Init_Comm(equipconfig.NhSjz_Com, equipconfig.NhSjz_ComString) == false)
                        {
                            nhsjz = null;
                            Init_flag = false;
                            init_message += "南华司机助串口打开失败.";
                        }
                    }
                    catch (Exception er)
                    {
                        nhsjz = null;
                        Init_flag = false;
                        MessageBox.Show("南华司机助串口" + equipconfig.NhSjz_Com + "打开失败:" + er.ToString(), "出错啦");
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void initChujian()
        {
            dt.Columns.Add("项目");
            dt.Columns.Add("结果");
            dt.Columns.Add("判定");
            DataRow dr = null;
            dr = dt.NewRow();
            dr["项目"] = "滚筒速度";
            dr["结果"] = "";
            dr["判定"] = "";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "环境温度";
            dr["结果"] = "";
            dr["判定"] = "";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "环境湿度";
            dr["结果"] = "";
            dr["判定"] = "";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "环境气压";
            dr["结果"] = "";
            dr["判定"] = "";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "环境背景残留气体";
            dr["结果"] = "";
            dr["判定"] = "";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "取样系统HC残留量";
            dr["结果"] = "";
            dr["判定"] = "";
            dt.Rows.Add(dr);
            dataGridView1.DataSource = dt;
        }
        private void initCarInfo()
        {
            carbj = carini.getCarIni();
        }
        private void initConfigInfo()
        {
            equipconfig = configini.getEquipConfigIni();
            isautostart = equipconfig.WorkAutomaticMode;
            asmconfig = configini.getAsmConfigIni();
            thaxsdata = configini.getthaxsConfigIni();
            fqy_delayTime = 8;
        }
        public void led_display(LEDNumber.LEDNumber lednumber, string data)
        {
            BeginInvoke(new wl_led(led_show), lednumber, data);
        }
        public void led_show(LEDNumber.LEDNumber lednumber, string data)
        {
            lednumber.LEDText = data;
        }
        #region 初始化


        public void px()
        {
            float temp_val = 0f;
            for (int j = 0; j < 9; j++)
            {
                for (int i = 0; i < 9 - j; i++)
                {
                    if (hjo2data[i] < hjo2data[i + 1])
                    {
                        temp_val = hjo2data[i + 1];
                        hjo2data[i + 1] = hjo2data[i];
                        hjo2data[i] = temp_val;
                    }
                }
            }
        }
        public void Init_power()
        {
            try
            {
                Set_Power5025 = (float)(carbj.CarJzzl) / 148f/asmconfig.Glcs;
                Set_Power2540 = (float)(carbj.CarJzzl) / 185f / asmconfig.Glcs;

            }
            catch
            {
            }
        }
        public void Init_Data()
        {
            Msg(label_cp, panel_cp, carbj.CarPH, false);
            Msg(label_message, panel_msg, "点击开始检测按钮,开始检测", false);
        }
        #endregion

        private double jsgl = 0;
        private double zsgl = 0;
        private double zgl = 0;
        private float nl = 0;
        private void timer2_Click(object sender, EventArgs e)
        {
            float co_average = 0;
            float hc_average = 0;
            float no_average = 0;
            float co2_average = 0;
            float o2_average = 0;
            float lambda_average = 0;
            float co2 = 0;
            float o2 = 0;
            
            nowtime = DateTime.Now;
            if (JC_Status)
            {
                co2 = Vmas_Exhaust_Now.CO2;
                //if (co2 < 6) co2 = (float)(6 + (DateTime.Now.Millisecond % 100)*0.01);

                jsgl = Math.Round(jsgl_xsa * igbt.Speed * igbt.Speed + jsgl_xsb * igbt.Speed + jsgl_xsc, 3);//寄生功率=
                if (jsgl < 0) jsgl = 0;
                zgl = Math.Round(igbt.Power * asmconfig.Glcs, 3);
                if (zgl < jsgl) zgl = jsgl;
                zsgl = Math.Round(zgl - jsgl, 3);
                if (zsgl < 0) zsgl = 0;
                nl = igbt.Force;
                if (nl < 0) nl = 0;

                if (saveProcessData)
                {
                    try
                    {
                        DateTime jcnowtime = DateTime.Now;
                        TimeSpan jctimespan = jcnowtime - jcStarttime;
                        jcTime = (float)jctimespan.TotalMilliseconds / 1000f;
                        JCSJ = (int)jcTime;
                        led_display(ledNumberTestTime, jcTime.ToString("0.0"));
                        if (JCSJ > preJCSJ)           //每1s记录一次信息
                        {
                            preJCSJ = JCSJ;
                            if (equipconfig.DATASECONDS_TYPE == "安徽")
                            {                                
                                if (testState == TEST_STATE.STATE_PREP||testState==TEST_STATE.STATE_ACC5025||testState==TEST_STATE.STATE_ACC2540)
                                {
                                    int other_sxnb = 0;
                                    if (testState == TEST_STATE.STATE_PREP) other_sxnb = 0;
                                    else if (testState == TEST_STATE.STATE_ACC5025) other_sxnb = 3;
                                    else if (testState == TEST_STATE.STATE_ACC2540) other_sxnb = 4;
                                    other_Vmas_qcsj.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));//全程时序
                                    other_Vmas_cysx.Add((JCSJ + 1).ToString());
                                    other_Vmas_sxnb.Add(other_sxnb.ToString());
                                    other_Vmas_sscs.Add(igbt.Speed);//实时速度
                                    other_Vmas_jzgl.Add(zgl);//加载功率
                                    other_Vmas_Exhaust_jsgl.Add(jsgl);//寄生功率
                                    other_vmas_zsgl.Add(zsgl);
                                    other_Vmas_jczt.Add( jczt);
                                    other_Vmas_nl.Add(Math.Round(nl, 0));//加载功率
                                    other_Vmas_Exhaust_qlyl.Add( Vmas_Exhaust_Now.QLYL);//气路压力
                                    other_Vmas_Exhaust_cold.Add( Vmas_Exhaust_Now.CO);//CO浓度
                                    other_Vmas_Exhaust_co2ld.Add(co2);//CO2浓度
                                    other_Vmas_Exhaust_hcld.Add( Vmas_Exhaust_Now.HC);//HC浓度
                                    other_Vmas_Exhaust_o2ld.Add(Vmas_Exhaust_Now.O2);//废气仪氧气浓度

                                    other_Vmas_hjwd.Add(WD);//温度
                                    other_Vmas_xdsd.Add(SD);//湿度
                                    other_Vmas_dqyl.Add(DQY);//大气压

                                    other_Vmas_sdxzxs.Add(caculateDf(co2, Vmas_Exhaust_Now.CO));//稀释修正系数
                                    other_Vmas_xsxzxs.Add( caculateKh(WD, SD, DQY));//湿度修正系数

                                    other_Vmas_Exhaust_zs.Add( Vmas_Exhaust_Now.ZS);//转速
                                    other_Vmas_Exhaust_yw.Add( yw);//油温
                                    other_Vmas_Exhaust_nold.Add( Vmas_Exhaust_Now.NO);//NO浓度
                                }
                            }
                            else
                            {
                                //djccyString = "";
                                sjxlString += jcnowtime.ToString("yyyy-MM-dd HH:mm:ss.fff") + ",";
                                hcclzString += Vmas_Exhaust_Now.HC.ToString() + ",";
                                noclzString += Vmas_Exhaust_Now.NO.ToString() + ",";
                                coclzString += Vmas_Exhaust_Now.CO.ToString() + ",";
                                co2clzString += co2.ToString() + ",";
                                o2clzString += Vmas_Exhaust_Now.O2.ToString() + ",";
                                csString += igbt.Speed.ToString("0.0") + ",";
                                zsString += Vmas_Exhaust_Now.ZS.ToString() + ",";
                                xsxzxsString += "1" + ",";
                                sdxzxsString += "1" + ",";
                                jsglString += jsgl.ToString("0.0") + ",";
                                zsglString += zgl.ToString("0.0") + ",";
                                hjwdString += WD.ToString("0.0") + ",";
                                dqylString += DQY.ToString("0.0") + ",";
                                xdsdString += SD.ToString("0.0") + ",";
                                ywString += yw.ToString() + ",";
                                jcztString += jczt.ToString() + ",";
                            }

                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
                if (testState==TEST_STATE.STATE_TEST5025||testState==TEST_STATE.STATE_TEST2540)
                {
                    try
                    {
                        bool chaocha = true;
                        speedNow.speed_now_data = igbt.Speed;
                        if (testState == TEST_STATE.STATE_TEST5025)
                        {
                            if (igbt.Speed < 15)
                                igbt.Set_Control_Power(0);
                            else if(igbt.Speed<23.5)
                                igbt.Set_Control_Power((float)(((igbt.Speed-15)*0.1)*Set_Power5025));
                            else
                                igbt.Set_Control_Power((float)(Set_Power5025));

                        }
                        if (testState == TEST_STATE.STATE_TEST2540)
                        {
                            if (igbt.Speed < 15)
                                igbt.Set_Control_Power(0);
                            else if (igbt.Speed < 38.5)
                                igbt.Set_Control_Power((float)(((igbt.Speed - 15) * 0.04) * Set_Power2540));
                            else
                                igbt.Set_Control_Power(Set_Power2540);
                        }
                        if (Convert.ToInt16(gongkuangTime * 10) / 10 != GKSJ)           //每1s记录一次信息
                        {
                            if (testState == TEST_STATE.STATE_TEST5025)
                            {
                                if (GKSJ < 90)
                                {
                                    asm_Exhaust_rev5025[GKSJ] = Vmas_Exhaust_Now.ZS;
                                    asm_Exhaust_lambda5025[GKSJ] = Vmas_Exhaust_Now.λ;
                                }
                            }
                            if (testState == TEST_STATE.STATE_TEST2540)
                            {
                                if (GKSJ < 90)
                                {
                                    asm_Exhaust_rev2540[GKSJ] = Vmas_Exhaust_Now.ZS;
                                    asm_Exhaust_lambda2540[GKSJ] = Vmas_Exhaust_Now.λ;
                                }
                            }
                            gksj_count = GKSJ;
                            lowflowarray[gksj_count] = isLowFlow;
                            Vmas_qcsj[gksj_count] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//全程时序
                            Vmas_cysx[gksj_count] = (GKSJ + 1).ToString();
                            Vmas_sxnb[gksj_count] = sxnb.ToString();
                            Vmas_sscs[gksj_count] = igbt.Speed;//实时速度
                            Vmas_jzgl[gksj_count] = (float)zgl;//加载功率
                            Vmas_Exhaust_jsgl[gksj_count] = (float)jsgl;//寄生功率
                            vmas_zsgl[gksj_count] = (float)zsgl;
                            Vmas_jczt[gksj_count] = jczt;
                            Vmas_nl[gksj_count] = (float)(Math.Round(nl,0));//加载功率
                            Vmas_Exhaust_ListIG195[gksj_count] = Vmas_Exhaust_Now;//尾气浓度值
                            Vmas_Exhaust_Revise_ListIG195[gksj_count] = Vmas_Exhaust_ReviseNow;//修正的尾气深度值
                            Vmas_Exhaust_qlyl[gksj_count] = Vmas_Exhaust_Now.QLYL;//气路压力
                            Vmas_Exhaust_cold[gksj_count] = Vmas_Exhaust_Now.CO;//CO浓度
                            Vmas_Exhaust_co2ld[gksj_count] = co2;//CO2浓度
                            Vmas_Exhaust_hcld[gksj_count] = Vmas_Exhaust_Now.HC;//HC浓度
                            Vmas_Exhaust_o2ld[gksj_count] = Vmas_Exhaust_Now.O2;//废气仪氧气浓度
                                                                                
                            Vmas_hjwd[gksj_count] = (float)WD;//温度
                            Vmas_xdsd[gksj_count] = (float)SD;//湿度
                            Vmas_dqyl[gksj_count] = (float)DQY;//大气压

                            Vmas_xsxzxs[gksj_count] = caculateDf(Vmas_Exhaust_co2ld[gksj_count], Vmas_Exhaust_cold[gksj_count]);//稀释修正系数
                            Vmas_sdxzxs[gksj_count] = caculateKh(Vmas_hjwd[gksj_count], Vmas_xdsd[gksj_count], Vmas_dqyl[gksj_count]);//湿度修正系数
                            
                            Vmas_Exhaust_zs[gksj_count] = Vmas_Exhaust_Now.ZS;//转速
                            Vmas_Exhaust_yw[gksj_count] = yw;//油温
                            Vmas_Exhaust_nold[gksj_count] = Vmas_Exhaust_Now.NO;//NO浓度
                            if (asmconfig.IfDisplayData)
                            {
                                led_display(ledNumberCO, Vmas_Exhaust_Now.CO.ToString("0.00"));
                                led_display(ledNumberCO2, Vmas_Exhaust_Now.CO2.ToString("0.00"));
                                led_display(ledNumberNO, Vmas_Exhaust_Now.NO.ToString("0"));
                                led_display(ledNumberHC, Vmas_Exhaust_Now.HC.ToString("0"));
                                led_display(ledNumberO2, Vmas_Exhaust_Now.O2.ToString("0.00"));
                                led_display(ledNumberLL, Vmas_Exhaust_Now.QLYL.ToString("0"));
                                led_display(ledNumberYw, Vmas_Exhaust_Now.YW.ToString("0.0"));

                                ts3 =  "CO:" + Vmas_Exhaust_Now.CO.ToString("0.00") + " HC:" + Vmas_Exhaust_Now.HC.ToString("0") + " NOx:" + Vmas_Exhaust_Now.NO.ToString("0.00") + " CO2:" + Vmas_Exhaust_Now.CO2.ToString("0.00") + " O2:" + Vmas_Exhaust_Now.O2.ToString("0.00") + " T:" + Vmas_hjwd[gksj_count].ToString("0.0") + " H:" + Vmas_xdsd[gksj_count].ToString("0.0") + " A:" + Vmas_dqyl[gksj_count].ToString("0.0");

                            }
                            GKSJ++;
                        }
                        if (GKSJ >= 25)
                        {
                            if (testState == TEST_STATE.STATE_TEST5025)
                            {
                                //下面取消掉过程中大于5位限值判断不合格的程序，因为安车新平台要根据过程点反算结果，由于标准没有说清楚这种情况结果取什么修
                                /*bool COexhaustCbFiveXzFlag = true;
                                bool HCexhaustCbFiveXzFlag = true;
                                bool NOexhaustCbFiveXzFlag = true;
                                for (int averageCount = gksj_count - 9; averageCount <= gksj_count; averageCount++)
                                {
                                    if (Vmas_Exhaust_cold[averageCount] <= 5 * carbj.Xz2)
                                        COexhaustCbFiveXzFlag = false;
                                    if (Vmas_Exhaust_hcld[averageCount] <= 5 * carbj.Xz1)
                                        HCexhaustCbFiveXzFlag = false;
                                    if (Vmas_Exhaust_nold[averageCount] <= 5 * carbj.Xz3)
                                        NOexhaustCbFiveXzFlag = false;
                                }
                                if (COexhaustCbFiveXzFlag || HCexhaustCbFiveXzFlag || NOexhaustCbFiveXzFlag)
                                {
                                    exhaustIsCb5025 = true;
                                    exhaustCoCb = Vmas_Exhaust_cold[gksj_count];
                                    exhaustHcCb = Vmas_Exhaust_hcld[gksj_count];
                                    exhaustNoCb = Vmas_Exhaust_nold[gksj_count];
                                    exhaustCo2Cb = Vmas_Exhaust_co2ld[gksj_count];
                                    exhaustO2Cb = Vmas_Exhaust_o2ld[gksj_count];
                                }*/
                                bool isEffective = true;//定义该点数据有效
                                if (asmconfig.Speed05Monitor)//如果设置判定0.5km/h（任意连续 10s 内第一秒至第十秒的车速变化相对于第一秒小于±0.5km/h，测试结果有效）
                                {
                                    for (int averageCount = gksj_count - 9; averageCount <= gksj_count; averageCount++)
                                    {
                                        if (Math.Abs(Vmas_sscs[gksj_count] - Vmas_sscs[gksj_count - 9]) >= 0.5)//如果有一个点超过了0.5，则认为无效
                                        {
                                            isEffective = false;
                                            break;
                                        }
                                    }
                                }
                                if (isEffective)//第10秒的车速相对于第1秒变化小于0.5km/h时数据有效
                                {
                                    for (int averageCount = gksj_count - 9; averageCount <= gksj_count; averageCount++)
                                    {
                                        co_average += Vmas_Exhaust_cold[averageCount]* Vmas_xsxzxs[averageCount];
                                        hc_average += Vmas_Exhaust_hcld[averageCount] * Vmas_xsxzxs[averageCount];
                                        no_average += Vmas_Exhaust_nold[averageCount] * Vmas_xsxzxs[averageCount] * Vmas_sdxzxs[averageCount];
                                        co2_average += Vmas_Exhaust_co2ld[averageCount];
                                        o2_average += Vmas_Exhaust_o2ld[averageCount];
                                    }
                                    co_average =(float)(Math.Round( co_average / 10.0f,2));
                                    hc_average = (float)(Math.Round(hc_average / 10.0f, 0));
                                    no_average = (float)(Math.Round(no_average / 10.0f, 0));
                                    co2_average = co2_average / 10.0f;
                                    o2_average = o2_average / 10.0f;
                                    asm_Exhaust_co5025.Add(co_average);
                                    asm_Exhaust_hc5025.Add(hc_average);
                                    asm_Exhaust_no5025.Add(no_average);
                                    asm_Exhaust_co25025.Add(co2_average);
                                    asm_Exhaust_o25025.Add(o2_average);
                                    asm_Exhaust_gksj5025.Add(gksj_count);
                                    
                                }
                            }
                            if (testState == TEST_STATE.STATE_TEST2540)
                            {
                                /*bool COexhaustCbFiveXzFlag = true;
                                bool HCexhaustCbFiveXzFlag = true;
                                bool NOexhaustCbFiveXzFlag = true;
                                for (int averageCount = gksj_count - 9; averageCount <= gksj_count; averageCount++)
                                {
                                    if (Vmas_Exhaust_cold[averageCount] <= 5 * carbj.Xz5)
                                        COexhaustCbFiveXzFlag = false;
                                    if (Vmas_Exhaust_hcld[averageCount] <= 5 * carbj.Xz4)
                                        HCexhaustCbFiveXzFlag = false;
                                    if (Vmas_Exhaust_nold[averageCount] <= 5 * carbj.Xz6)
                                        NOexhaustCbFiveXzFlag = false;
                                }
                                if (COexhaustCbFiveXzFlag || HCexhaustCbFiveXzFlag || NOexhaustCbFiveXzFlag)
                                {
                                    exhaustIsCb2540 = true;
                                    exhaustCoCb = Vmas_Exhaust_cold[gksj_count];
                                    exhaustHcCb = Vmas_Exhaust_hcld[gksj_count];
                                    exhaustNoCb = Vmas_Exhaust_nold[gksj_count];
                                    exhaustCo2Cb = Vmas_Exhaust_co2ld[gksj_count];
                                    exhaustO2Cb = Vmas_Exhaust_o2ld[gksj_count];
                                }*/
                                bool isEffective = true;//定义该点数据有效
                                if (asmconfig.Speed05Monitor)//如果设置判定0.5km/h（任意连续 10s 内第一秒至第十秒的车速变化相对于第一秒小于±0.5km/h，测试结果有效）
                                {
                                    for (int averageCount = gksj_count - 9; averageCount <= gksj_count; averageCount++)
                                    {
                                        if (Math.Abs(Vmas_sscs[gksj_count] - Vmas_sscs[gksj_count - 9]) >= 0.5)//如果有一个点超过了0.5，则认为无效
                                        {
                                            isEffective = false;
                                            break;
                                        }
                                    }
                                }
                                if (isEffective)//第10秒的车速相对于第1秒变化小于0.5km/h时数据有效
                                {
                                    for (int averageCount = gksj_count - 9; averageCount <= gksj_count; averageCount++)
                                    {
                                        co_average += Vmas_Exhaust_cold[averageCount] * Vmas_xsxzxs[averageCount];
                                        hc_average += Vmas_Exhaust_hcld[averageCount] * Vmas_xsxzxs[averageCount];
                                        no_average += Vmas_Exhaust_nold[averageCount] * Vmas_xsxzxs[averageCount] * Vmas_sdxzxs[averageCount];
                                        co2_average += Vmas_Exhaust_co2ld[averageCount];
                                        o2_average += Vmas_Exhaust_o2ld[averageCount];
                                    }
                                    co_average = co_average / 10.0f;
                                    hc_average = hc_average / 10.0f;
                                    no_average = no_average / 10.0f;
                                    co2_average = co2_average / 10.0f;
                                    o2_average = o2_average / 10.0f;
                                    asm_Exhaust_co2540.Add(co_average);
                                    asm_Exhaust_hc2540.Add(hc_average);
                                    asm_Exhaust_no2540.Add(no_average);
                                    asm_Exhaust_co22540.Add(co2_average);
                                    asm_Exhaust_o22540.Add(o2_average);
                                    asm_Exhaust_gksj2540.Add(gksj_count);
                                }
                            }

                        }
                        else
                        {
                            if (testState == TEST_STATE.STATE_TEST5025)
                            {
                                jczt = 1;
                            }
                            if (testState == TEST_STATE.STATE_TEST2540)
                            {
                                jczt = 3;
                            }
                        }
                        if (GKSJ >= 3 && asmconfig.ConcentrationMonitor == true)//如果浓度监控开启的话
                        {
                            if ((Vmas_Exhaust_cold[gksj_count] + Vmas_Exhaust_co2ld[gksj_count]) < asmconfig.Ndz)
                            {
                                if ((Vmas_Exhaust_cold[gksj_count - 1] + Vmas_Exhaust_co2ld[gksj_count - 1]) < asmconfig.Ndz)
                                {
                                    if ((Vmas_Exhaust_cold[gksj_count - 2] + Vmas_Exhaust_co2ld[gksj_count - 2]) < asmconfig.Ndz)
                                    {
                                        testState = TEST_STATE.STATE_PREP;
                                        ovalShapeNDZ.FillColor = Color.Red;
                                        outTimeContinus = 0f;
                                        outTimeTotal = 0f;
                                        TH_ST.Abort();
                                        Speed_Jc_flag = false;
                                        gongkuangTime = 0f;
                                        GKSJ = 0;
                                        fla_502.Stop();
                                        Msg(label_message, panel_msg, "[CO]+[CO2]小于规定值，请检查取样探头是否脱落", true);
                                        ts1 = "测试中止";
                                        ts2 = "[CO]+[CO2]小于规定值";
                                        button_ss.Text = "重新检测";
                                        JC_Status = false; saveProcessData = false;
                                         jcStatus = false;
                                        return;
                                    }
                                }
                            }
                        }
                        if (GKSJ >= 3)//如果浓度监控开启的话
                        {
                            if ((lowflowarray[gksj_count] == -2) && (lowflowarray[gksj_count - 1] == -2) && (lowflowarray[gksj_count - 2] == -2))
                            {
                                testState = TEST_STATE.STATE_PREP;
                                //ovalShapeNDZ.FillColor = Color.Red;
                                outTimeContinus = 0f;
                                outTimeTotal = 0f;
                                TH_ST.Abort();
                                Speed_Jc_flag = false;
                                gongkuangTime = 0f;
                                GKSJ = 0;
                                fla_502.Stop();
                                Msg(label_message, panel_msg, "废气仪气路低流量警告，检测终止", true);
                                ts1 = "测试中止";
                                ts2 = "废气仪气路低流量警告";
                                button_ss.Text = "重新检测";
                                JC_Status = false; saveProcessData = false;
                                jcStatus = false;
                                return;
                            }
                        }
                        if (gongkuangTime >= 90.0f)
                        {
                            ledNumber_gksj.LEDText = "90.0";
                        }
                        else
                        {
                            ledNumber_gksj.LEDText = gongkuangTime.ToString("00.0");
                        }
                        float thisTimeSpan = gongkuangTime;
                        TimeSpan timespan = nowtime - startTime;
                        gongkuangTime = (float)timespan.TotalMilliseconds / 1000f;
                        thisTimeSpan = gongkuangTime - thisTimeSpan;
                        if (testState==TEST_STATE.STATE_TEST5025)
                        {
                            if ((igbt.Speed <= 26.5) && (igbt.Speed >= 23.5))
                                chaocha = false;
                        }
                        else
                        {
                            if ((igbt.Speed <= 41.5) && (igbt.Speed >= 38.5))
                                chaocha = false;
                        }
                        if (GKSJ < 5 && chaocha == true)
                        {
                            startTime = nowtime;
                            gongkuangTime = 0;
                            GKSJ = 0;
                        }
                        else if (chaocha == true)
                        {
                            outTimeContinus += thisTimeSpan;
                            outTimeTotal += thisTimeSpan;
                            if (asmconfig.SpeedMonitor == true)
                            {
                                if (outTimeContinus >= asmconfig.Lxcc)
                                {
                                    ovalShapeTXZK.FillColor = Color.Red;
                                    outTimeContinus = 0f;
                                    outTimeTotal = 0f;
                                    TH_ST.Abort();
                                    Speed_Jc_flag = false;
                                    testState = TEST_STATE.STATE_PREP;
                                    gongkuangTime = 0f;
                                    GKSJ = 0;
                                    fla_502.Stop();
                                    Msg(label_message, panel_msg, "车速连续超差的时间超过" + asmconfig.Lxcc.ToString("0.0") + "，请调整后重新开始", true);
                                    button_ss.Text = "重新检测";
                                    ts1 = "测试中止";
                                    ts2 = "车速超差超过规定值";
                                    JC_Status = false; saveProcessData = false;
                                    jcStatus = false;
                                    return;
                                    //Thread.Sleep(1500);
                                }
                            }

                        }
                        else
                        {
                            outTimeContinus = 0;
                        }
                        led_display(ledNumber_lxcc, outTimeContinus.ToString("0.0"));
                    }
                    catch (Exception)
                    {
                    }
                }

                else
                {
                    gongkuangTime = 0f;
                    GKSJ = 0;
                    outTimeTotal = 0f;
                    outTimeContinus = 0f;
                    ledNumber_gksj.LEDText = gongkuangTime.ToString("00.0");
                }
            }

        }
        public void Fq_Detect()
        {
            while (JC_Status)
            {
                Vmas_Exhaust_Now = fla_502.GetData();
                Revise(Vmas_Exhaust_Now);
                Thread.Sleep(30);
                if (equipconfig.Fqyxh.ToLower() == "fla_502" || equipconfig.Fqyxh.ToLower() == "nha_503" || equipconfig.Fqyxh.ToLower() == "cdf5000")
                {
                    fla502_temp_data = fla_502.Get_Temp();
                    Thread.Sleep(30);
                }
                isLowFlow = fla_502.CheckIsLowFlow();
                if (nhsjz != null && asmconfig.Ywj == "南华附件")
                {
                    if (nhsjz.readData())
                        yw = nhsjz.yw;
                }
                else
                {
                    yw = Vmas_Exhaust_Now.YW;
                }
                if (equipconfig.TempInstrument == "废气仪")
                {
                    if (equipconfig.Fqyxh.ToLower() == "fla_502" || equipconfig.Fqyxh.ToLower() == "nha_503" || equipconfig.Fqyxh.ToLower() == "cdf5000")
                    {
                        WD = fla502_temp_data.TEMP;
                        SD = fla502_temp_data.HUMIDITY;
                        DQY = fla502_temp_data.AIRPRESSURE;
                    }
                    else
                    {
                        WD = Vmas_Exhaust_Now.HJWD;
                        SD = Vmas_Exhaust_Now.SD;
                        DQY = Vmas_Exhaust_Now.HJYL;
                    }
                }
                Thread.Sleep(50);
            }
        }
        public void Jc_Exe()
        {
            Ig195_exe();
        }


        #region 初检
        public void vmas_chujian()
        {
            wsd_sure = false;
            wsdValueIsRight = false;
            chujianIsFinished = false;
            chujianIsOk = false;
            float lljo2 = 0f;
            Exhaust.Fla502_data huanjiang_data = null;

            try//获取环境参数
            {
                ts2 = "检测测试环境中";
                Thread.Sleep(1000);
                if(IsUseTpTemp)
                {
                    WD = WD;
                    SD = SD;
                    DQY = DQY;
                }
                else if (equipconfig.TempInstrument == "废气仪")
                {
                    if (equipconfig.Fqyxh.ToLower() == "nha_503" || equipconfig.Fqyxh.ToLower() == "fla_502" || equipconfig.Fqyxh.ToLower() == "cdf5000")
                    {
                        Exhaust.Fla502_temp_data Environment = fla_502.Get_Temp();
                        WD = Environment.TEMP;
                        SD = Environment.HUMIDITY;
                        DQY = Environment.AIRPRESSURE;
                    }
                    else
                    {
                        Exhaust.Fla502_data Environment = fla_502.GetData();
                        WD = Environment.HJWD;
                        SD = Environment.SD;
                        DQY = Environment.HJYL;
                    }
                }
                else if (equipconfig.TempInstrument == "烟度计" && flb_100 != null)
                {
                    flb_100.Set_Measure();
                    Thread.Sleep(1000);
                    if (equipconfig.IsOldMqy200)
                    {
                        Exhaust.Flb_100_smoke smoke = flb_100.get_DirectData();
                        WD = smoke.WD;
                        SD = smoke.SD;
                        DQY = smoke.DQY;
                    }
                    else
                    {
                        Exhaust.Flb_100_smoke ydjEnvironment = flb_100.get_Data();
                        WD = ydjEnvironment.WD;
                        SD = ydjEnvironment.SD;
                        DQY = ydjEnvironment.DQY;
                    }
                }
                else if (equipconfig.TempInstrument == "XCE_100")
                {
                    if (xce_100.readEnvironment())
                    {
                        WD = xce_100.temp;
                        SD = xce_100.humidity;
                        DQY = xce_100.airpressure;
                    }
                    else
                    {
                        Msg(label_chujiantishi, panel_chujiantishi, "读取环境参数失败,不能进行检测", false);
                        ts2 = "读取环境参数失败";
                        return;
                    }
                }
                else if (equipconfig.TempInstrument == "XCE_101")
                {
                    if (xce_100.readEnvironment())
                    {
                        WD = xce_100.temp;
                        SD = xce_100.humidity;
                        DQY = xce_100.airpressure;
                    }
                    else
                    {
                        Msg(label_chujiantishi, panel_chujiantishi, "读取环境参数失败,不能进行检测", false);
                        ts2 = "读取环境参数失败";
                        return;
                    }
                }
                else if (equipconfig.TempInstrument == "DWSP_T5" || equipconfig.TempInstrument == "FTH_2")
                {
                    if (xce_100.readEnvironment())
                    {
                        WD = xce_100.temp;
                        SD = xce_100.humidity;
                        DQY = xce_100.airpressure;
                    }
                    else
                    {
                        Msg(label_chujiantishi, panel_chujiantishi, "读取环境参数失败,不能进行检测", false);
                        ts2 = "读取环境参数失败";
                        return;
                    }
                }
                else if (equipconfig.TempInstrument == "RZ_1")
                {
                    if (xce_100.readEnvironment())
                    {
                        WD = xce_100.temp;
                        SD = xce_100.humidity;
                        DQY = xce_100.airpressure;
                    }
                    else
                    {
                        Msg(label_chujiantishi, panel_chujiantishi, "读取环境参数失败,不能进行检测", false);
                        ts2 = "读取环境参数失败";
                        return;
                    }
                }
                else if(equipconfig.TempInstrument=="模拟")
                {
                    Random rd = new Random();
                    int month = DateTime.Now.Month;
                    int hour = DateTime.Now.Hour;
                    WD = temperatureEveryMonth[month - 1] - Math.Abs(hour - 12) * 0.6 + (double)(rd.Next(20) - 10) / 10.0;
                    SD = 50 + (double)(rd.Next(400) - 200) / 10.0;
                    DQY = 90 + (double)(rd.Next(20) - 10) / 10.0;
                }
                WD = thaxsdata.Tempxs * WD;
                SD = thaxsdata.Humixs * SD;
                DQY = thaxsdata.Airpxs * DQY;
                Msg(label_wd, panel_wd, WD.ToString("0.0"), false);
                Msg(label_sd, panel_sd, SD.ToString("0.0"), false);
                Msg(label_dqy, panel_dqy, DQY.ToString("0.0"), false);
            }
            catch (Exception)
            {
            }
            panel_visible(panel_chujian, true);//显示初检界面
            textbox_value(textBoxSDSD, SD.ToString("0.0"));
            textbox_value(textBoxSDWD, WD.ToString("0.0"));
            textbox_value(textBoxSDQY, DQY.ToString("0.0"));
            if (asmconfig.IfSureTemp)
            {
                panel_visible(panelWSD, true);
                while (wsd_sure == false)
                {
                    Thread.Sleep(500);
                }
                wsd_sure = false;
                panel_visible(panelWSD, false);
                if (wsdValueIsRight == false)
                {
                    Msg(label_chujiantishi, panel_chujiantishi, "请先校正废气仪温湿度", false);
                    return;
                }
                WD = double.Parse(textBoxSDWD.Text);
                SD = double.Parse(textBoxSDSD.Text);
                DQY = double.Parse(textBoxSDQY.Text);
            }
            datagridview_msg(dataGridView1, "结果", 0, igbt.Speed.ToString("0.0"));
            if (igbt.Speed > 0.5f)
            {
                datagridview_msg(dataGridView1, "判定", 0, "×");
                Msg(label_chujiantishi, panel_chujiantishi, "请等待滚筒停止再进行", false);
                return;
            }
            else
                datagridview_msg(dataGridView1, "判定", 0, "√");

            datagridview_msg(dataGridView1, "结果", 1, WD.ToString("0.0"));
            if (WD > 40)
            {
                Msg(label_chujiantishi, panel_chujiantishi, "环境温度过高,不能进行检测", false);
                ts2 = "环境温度过高";
                datagridview_msg(dataGridView1, "判定", 1, "×");
                return;
            }
            else
                datagridview_msg(dataGridView1, "判定", 1, "√");

            datagridview_msg(dataGridView1, "结果", 2, SD.ToString("0.0"));
            if (SD > 85)
            {
                Msg(label_chujiantishi, panel_chujiantishi, "环境湿度不合格,不能进行检测", false);
                ts2 = "环境湿度过高";
                datagridview_msg(dataGridView1, "判定", 2, "×");
                return;
            }
            else
                datagridview_msg(dataGridView1, "判定", 2, "√");

            datagridview_msg(dataGridView1, "结果", 3, DQY.ToString("0.0"));
            if (DQY > 120)
            {
                Msg(label_chujiantishi, panel_chujiantishi, "环境大气压不合格,不能进行检测", false);
                datagridview_msg(dataGridView1, "判定", 3, "×");
                return;
            }
            else
                datagridview_msg(dataGridView1, "判定", 3, "√");
            Thread.Sleep(200);

            if (asmconfig.RemainedMonitor == true)
            {
                Thread.Sleep(200);
                fla_502.Pump_air();
                for (int i = equipconfig.BackGroundTestTime; i >= 0; i--)
                {
                    ts2 = "背景环境测定..." + i.ToString("0");
                    datagridview_msg(dataGridView1, "结果", 4, "检测中..." + i.ToString("0") + "秒");
                    Thread.Sleep(1000);
                }

            }
            Thread.Sleep(500);
            huanjiang_data = fla_502.GetData();
            if (asmconfig.RemainedMonitor == true)
            {
                asm_data.AmbientCO = huanjiang_data.CO.ToString("0.00");
                asm_data.AmbientCO2 = huanjiang_data.CO2.ToString("0.00");
                asm_data.AmbientHC = huanjiang_data.HC.ToString("0");
                asm_data.AmbientO2 = huanjiang_data.O2.ToString("0.00");
                asm_data.AmbientNO = huanjiang_data.NO.ToString("0");
                asm_data.BackgroundCO = huanjiang_data.CO.ToString("0.00");
                asm_data.BackgroundCO2 = huanjiang_data.CO2.ToString("0.00");
                asm_data.BackgroundHC = huanjiang_data.HC.ToString("0");
                asm_data.BackgroundO2 = huanjiang_data.O2.ToString("0.00");
                asm_data.BackgroundNO = huanjiang_data.NO.ToString("0");
                datagridview_msg(dataGridView1, "结果", 4, "CO:" + huanjiang_data.CO + ",HC:" + huanjiang_data.HC + ",NO:" + huanjiang_data.NO);
                if (huanjiang_data.CO > 2 || huanjiang_data.HC > 7 || huanjiang_data.NO > 25)//2,7,25
                {
                    Msg(label_chujiantishi, panel_chujiantishi, "环境背景污染水平不合格,请检查", true);
                    datagridview_msg(dataGridView1, "判定", 4, "×");
                    return;
                }
                else
                {
                    datagridview_msg(dataGridView1, "判定", 4, "√");
                }
                fla_502.StopBlowback();
            }
            else
            {
                asm_data.AmbientCO = "0.00";
                asm_data.AmbientCO2 = "0.00";
                asm_data.AmbientHC = "0";
                asm_data.AmbientO2 = "20.68";
                asm_data.AmbientNO = "0";
                asm_data.BackgroundCO = "0.00";
                asm_data.BackgroundCO2 = "0.00";
                asm_data.BackgroundHC = "0";
                asm_data.BackgroundO2 = "20.68";
                asm_data.BackgroundNO = "0";
                datagridview_msg(dataGridView1, "结果", 4, "CO:0,HC:0,NO:0");
                datagridview_msg(dataGridView1, "判定", 4, "√");
            }


            if (asmconfig.RemainedMonitor == true)
            {
                Thread.Sleep(500);
                fla_502.Pump_Pipeair();
                for (int i = equipconfig.CanliHCTestTime; i >= 0; i--)
                {
                    ts2 = "HC残留测定..." + i.ToString("0");
                    datagridview_msg(dataGridView1, "结果", 5, "检测中..." + i.ToString("0") + "秒");
                    Thread.Sleep(1000);
                }

            }
            Thread.Sleep(500);
            huanjiang_data = fla_502.GetData();
            if (asmconfig.RemainedMonitor == true)
            {
                datagridview_msg(dataGridView1, "结果", 5, "HC:" + huanjiang_data.HC.ToString("0"));
                if (huanjiang_data.HC > 10)//2,7,25
                {
                    Msg(label_chujiantishi, panel_chujiantishi, "取样系统HC残留量超标，请反吹", true);
                    datagridview_msg(dataGridView1, "判定", 5, "×");
                    return;
                }
                else
                {
                    datagridview_msg(dataGridView1, "判定", 5, "√");
                }
                asm_data.ResidualHC = huanjiang_data.HC.ToString();
                fla_502.StopBlowback();
            }
            else
            {
                asm_data.ResidualHC = "0";
                datagridview_msg(dataGridView1, "结果", 5, "HC:0");
                datagridview_msg(dataGridView1, "判定", 5, "√");
            }

            Thread.Sleep(500);
            panel_visible(panel_chujian, false);
            chujianIsFinished = true;
            chujianIsOk = true;

        }
        #endregion
        #region 5025工况

        /// <summary>
        /// 5025工况
        /// </summary>
        public void Ig195_exe()
        {
            sxnb = 0;
            int zero_count = 0;
            ig195IsFinished = false;
            DataRow dr = null;
            int cysx = 0;
            bool fast5025isOK = false;
            bool asm5025isOK = false;
            bool fast2540isOK = false;
            bool asm2540isOK = false;
            testState = TEST_STATE.STATE_PREP;
            float fast5025hc = 0;
            float fast5025co = 0;
            float fast5025no = 0;
            float fast5025co2 = 0;
            float fast5025o2 = 0;
            asm_Exhaust_co5025.Clear();
            asm_Exhaust_hc5025.Clear();
            asm_Exhaust_no5025.Clear();
            asm_Exhaust_co25025.Clear();
            asm_Exhaust_o25025.Clear();
            asm_Exhaust_hc2540.Clear();
            asm_Exhaust_co2540.Clear();
            asm_Exhaust_no2540.Clear();
            asm_Exhaust_co22540.Clear();
            asm_Exhaust_o22540.Clear();
            exhaustIsCb5025 = false;
            exhaustIsCb2540 = false;

            other_Vmas_qcsj.Clear();
            other_Vmas_cysx.Clear();
            other_Vmas_sxnb.Clear();
            other_Vmas_sscs.Clear();
            other_Vmas_jzgl.Clear();
            other_Vmas_Exhaust_jsgl.Clear();
            other_vmas_zsgl.Clear();
            other_Vmas_jczt.Clear();
            other_Vmas_nl.Clear();
            other_Vmas_Exhaust_qlyl.Clear();
            other_Vmas_Exhaust_cold.Clear();
            other_Vmas_Exhaust_co2ld.Clear();
            other_Vmas_Exhaust_hcld.Clear();
            other_Vmas_Exhaust_o2ld.Clear();

            other_Vmas_hjwd.Clear();
            other_Vmas_xdsd.Clear();
            other_Vmas_dqyl.Clear();

            other_Vmas_sdxzxs.Clear();
            other_Vmas_xsxzxs.Clear();

            other_Vmas_Exhaust_zs.Clear();
            other_Vmas_Exhaust_yw.Clear();
            other_Vmas_Exhaust_nold.Clear();

            DataTable vmas_datatable = new DataTable();
            vmas_datatable.Columns.Add("全程时序");
            vmas_datatable.Columns.Add("时序类别");
            vmas_datatable.Columns.Add("采样时序");
            vmas_datatable.Columns.Add("HC实时值");
            vmas_datatable.Columns.Add("CO实时值");
            vmas_datatable.Columns.Add("CO2实时值");
            vmas_datatable.Columns.Add("NO实时值");
            vmas_datatable.Columns.Add("O2实时值");
            //vmas_datatable.Columns.Add("标准时速");
            vmas_datatable.Columns.Add("实时车速");
            vmas_datatable.Columns.Add("加载功率");
            vmas_datatable.Columns.Add("扭力");
            vmas_datatable.Columns.Add("检测状态");
            vmas_datatable.Columns.Add("环境温度");
            vmas_datatable.Columns.Add("相对湿度");
            vmas_datatable.Columns.Add("大气压力");
            vmas_datatable.Columns.Add("湿度修正系数");
            vmas_datatable.Columns.Add("稀释修正系数");
            vmas_datatable.Columns.Add("分析仪管路压力");
            vmas_datatable.Columns.Add("转速");
            vmas_datatable.Columns.Add("寄生功率");
            vmas_datatable.Columns.Add("指示功率");
            vmas_datatable.Columns.Add("油温");
            try
            {

                statusconfigini.writeGlStatusData(statusconfigIni.ENUM_GL_STATUS.STATUS_DAOWEI, "");
                Msg(label_message, panel_msg, "测试即将开始,检查废气仪状态", false);
                ts1 = "测试即将开始";
                ts2 = "稳态工况法";
                Thread.Sleep(1000);
                string fla_502_status = fla_502.Get_Struct();
                if (fla_502_status != "仪器已经准备好")
                {
                    Msg(label_message, panel_msg, "废气仪" + fla_502_status + ",测试将结束", true);
                    button_ss.Text = "重新检测";
                    JC_Status = false; saveProcessData = false;
                    return;
                }
                Thread.Sleep(1000);
                
                Msg(label_message, panel_msg, "测试即将开始,正在进行初检", false);
                ts1 = "进行初检中";
                vmas_chujian();
                if (chujianIsFinished == false)
                {
                    Msg(label_message, panel_msg, "初检不合格,测试将结束", true);
                    ts1 = "初检不合格";
                    button_ss.Text = "重新检测";
                    JC_Status = false; saveProcessData = false;
                    return;
                }
                if (asmconfig.IsTestYw)
                {
                    ts1 = "读取油温...";
                    Msg(label_message, panel_msg, "读取油温...", false);
                    Exhaust.Fla502_data Environment = fla_502.GetData();
                    Thread.Sleep(1000);
                    float ywnow = Environment.YW;
                    if (ywnow < 80)
                    {
                        ts1 = "油温: " + ywnow.ToString("0.0") + " ℃";
                        ts2 = "低于限值,检测中止";
                        Msg(label_message, panel_msg, "油温:" + ywnow.ToString("0.0") + "℃" + "低于限值,检测中止", false);
                        button_ss.Text = "重新检测";
                        JC_Status = false; saveProcessData = false;
                        return;
                    }
                    else
                    {
                        ts1 = "油温: " + ywnow.ToString("0.0") + " ℃";
                        ts2 = "允许检测";
                        Msg(label_message, panel_msg, "油温:" + ywnow.ToString("0.0") + "℃" + ",允许检测", false);

                    }
                    Thread.Sleep(1000);
                }
                if (asmconfig.IfFqyTl)
                {

                    Thread.Sleep(500);
                    fla_502.Zeroing();                 //反吹                  1
                    Thread.Sleep(500);
                    zero_count = 0;
                    if (equipconfig.Fqyxh.ToLower() == "nha_503")
                    {
                        while (fla_502.Zeroing() == false)//该处需要测试后定
                        {
                            Thread.Sleep(900);
                            Msg(label_message, panel_msg, "废气仪调零中..." + zero_count.ToString() + "s", true);
                            ts1 = "调零中..." + zero_count.ToString() + "s";
                            zero_count++;
                            if (zero_count == 60)
                                break;
                        }
                    }
                    else if (equipconfig.Fqyxh.ToLower() == "cdf5000")
                    {
                        while (zero_count <= 40)//该处需要测试后定
                        {
                            Thread.Sleep(900);
                            Msg(label_message, panel_msg, "废气仪调零中..." + zero_count.ToString() + "s", true);
                            ts2 = "调零中..." + zero_count.ToString() + "s";
                            zero_count++;
                            if (zero_count == 60)
                                break;
                        }
                    }
                    else if (equipconfig.Fqyxh.ToLower() != "mqw_511")
                    {
                        while (fla_502.Get_Struct() == "调零中")
                        {
                            Thread.Sleep(900);
                            Msg(label_message, panel_msg, "废气仪调零中..." + zero_count.ToString() + "s", true);
                            ts1 = "调零中..." + zero_count.ToString() + "s";
                            zero_count++;
                            if (zero_count == 60)
                                break;
                        }
                    }
                    if (asmconfig.asmNoReZero) asmconfig.IfFqyTl = false;
                }
                Thread.Sleep(500);
                //statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.ZIJIANJIESHU, JCSJ.ToString());
                fla_502.Pump_Pipeair();
                Msg(label_message, panel_msg, "测试仪器开始工作,请安置好检测探头", false);
                sxnb = 0;
                ts1 = "请安置探头";
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.CHATANTOU, JCSJ.ToString());
                statusconfigini.writeNeuStatusData("StartTest", JCSJ.ToString());//东软开始命令
                asm_data.StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                asm_data.Has5025Tested = "1";
                asm_data.Has2540Tested = "0";
                asm_data.Lambda2540 = "0";
                asm_data.Rev2540 = "0";
                jcStarttime = DateTime.Now;
                jczt = 0;
                JC_Status = true;
                if (equipconfig.DATASECONDS_TYPE == "安徽") saveProcessData = true;
                testState = TEST_STATE.STATE_PREP;
                Th_get_FqandLl.Start();
                Thread.Sleep(2000);
                while (Vmas_Exhaust_Now.CO + Vmas_Exhaust_Now.CO2 <= asmconfig.Ndz)
                    Thread.Sleep(500);
                Msg(label_message, panel_msg, "探头已插好,检测开始", false);
                statusconfigini.writeGlStatusData(statusconfigIni.ENUM_GL_STATUS.STATUS_TANTOU, "");
                //Msg(label_message, panel_msg, "检测开始,举升下降", false);
                //ts1 = "举升下降";
                //igbt.Lifter_Down();     //举升下降
                Thread.Sleep(1000);
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIASU, JCSJ.ToString());
                igbt.Set_Control_Power(0f);
                Thread.Sleep(100);
                igbt.Start_Control_Power();
                Msg(label_message, panel_msg, "检测开始，请加速至25km/h", true);
                ts1 = "测5025工况";
                ts2 = "加速到25km/h";
                sxnb = 3;
                
                Thread.Sleep(2000);
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIANCEZHONG, JCSJ.ToString());
                Thread.Sleep(2000);
                while (igbt.Speed < 23.5)
                {
                    if(igbt.Speed < 15)
                        igbt.Set_Control_Power(0);
                    else
                        igbt.Set_Control_Power((float)(((igbt.Speed - 15) * 0.1) * Set_Power5025));
                    Thread.Sleep(100);
                }
                Msg(label_message, panel_msg, "请保持25km/h±1.5km/h,ASM5025工况开始", true);
                saveProcessData = true;
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.DAOWEI, JCSJ.ToString());
                statusconfigini.writeNeuStatusData("Testing5025", JCSJ.ToString());//东软5025开始
                statusconfigini.writeGlStatusData(statusconfigIni.ENUM_GL_STATUS.STATUS_STARTSAMPLE, "");
                ts1 = "测5025工况";
                ts2 = "保持25±1.5km/h";
                jczt = 1;
                sxnb = 1;
                testState = TEST_STATE.STATE_TEST5025;
                startTime = DateTime.Now;
                asm_data.Kstg5025 = "0";
                asm_data.Kstg2540 = "0";
                while (GKSJ < 5)
                {
                    ts2 = "保持25km/h..." + (5 - GKSJ).ToString() + "s";
                    Thread.Sleep(200);//在5s秒稳定住
                }
                while (GKSJ < 25)
                {
                    ts2 = "快速取样..." + (25 - GKSJ).ToString() + "s";
                    Thread.Sleep(200);//在5s秒稳定住
                }
                bool isFastEffective = true;//判断快速工况第15秒到25秒的速度值有没有在0.5以内
                if (asmconfig.Speed05Monitor)
                {
                    for (int i = 11; i <= 24; i++)
                    {
                        if (Math.Abs(Vmas_sscs[i] - Vmas_sscs[15]) >= 0.5)
                            isFastEffective = false;
                    }
                }
                if ((asmconfig.Speed05Monitor&& isFastEffective) ||!asmconfig.Speed05Monitor)
                {                    
                    fast5025hc = 0;
                    fast5025co = 0;
                    fast5025no = 0;
                    fast5025co2 = 0;
                    fast5025o2 = 0;
                    for (int i = 0; i < 10; i++)
                    {
                        fast5025hc += Vmas_Exhaust_hcld[i + 15] * Vmas_xsxzxs[i + 15];
                        fast5025co += Vmas_Exhaust_cold[i + 15] * Vmas_xsxzxs[i + 15];
                        fast5025no += Vmas_Exhaust_nold[i + 15] * Vmas_xsxzxs[i + 15] * Vmas_sdxzxs[i+15];
                        fast5025co2 += Vmas_Exhaust_co2ld[i + 15];
                        fast5025o2 += Vmas_Exhaust_o2ld[i + 15];
                    }
                    fast5025hc /= 10f;
                    fast5025co /= 10f;
                    fast5025no /= 10f;
                    fast5025co2 /= 10f;
                    fast5025o2 /= 10f;
                    if ((asmconfig.IsAsmHalfXzKsgk&&(fast5025hc <= 0.5f * carbj.Xz1 && fast5025co <= 0.5f * carbj.Xz2 && fast5025no <= 0.5f * carbj.Xz3))||(!asmconfig.IsAsmHalfXzKsgk&&(fast5025hc <= carbj.Xz1 && fast5025co <= carbj.Xz2 && fast5025no <= carbj.Xz3)))
                    {
                        asm_data.Hc5025 = fast5025hc.ToString("0");
                        asm_data.Co5025 = fast5025co.ToString("0.00");
                        asm_data.No5025 = fast5025no.ToString("0");
                        //asm_data.Co25025 = fast5025co2.ToString("0.00");
                        //asm_data.O25025 = fast5025o2.ToString("0.00");
                        asm_data.Hc5025pd = "合格";
                        asm_data.Co5025pd = "合格";
                        asm_data.No5025pd = "合格";
                        asm_data.Hc2540 = "";
                        asm_data.Co2540 = "";
                        asm_data.No2540 = "";
                        //asm_data.Co22540 = "";
                        //asm_data.O22540 = "";
                        asm_data.Hc2540pd = "";
                        asm_data.Co2540pd = "";
                        asm_data.No2540pd = "";
                        asm_data.RESULT = "合格";
                        double rev5025 = 0, lambda5025 = 0;
                        int startseconds=(equipconfig.DATASECONDS_TYPE == "东软甘肃")?15:0;
                        for (int i = startseconds; i < 25; i++)//将数据写入逐秒数据
                        {
                            dr = vmas_datatable.NewRow();
                            dr["全程时序"] = Vmas_qcsj[i];
                            dr["时序类别"] = Vmas_sxnb[i];
                            dr["采样时序"] = Vmas_cysx[i];
                            dr["HC实时值"] = Vmas_Exhaust_hcld[i];
                            dr["CO实时值"] = Vmas_Exhaust_cold[i];
                            dr["CO2实时值"] = Vmas_Exhaust_co2ld[i];
                            dr["NO实时值"] =equipconfig.DATASECONDS_TYPE=="安车通用联网"? ((Vmas_Exhaust_nold[i] * Vmas_sdxzxs[i]).ToString("0")): (Vmas_Exhaust_nold[i] .ToString("0"));
                            dr["O2实时值"] = Vmas_Exhaust_o2ld[i];
                            //dr["标准时速"] = Vmas_bzcs[i];
                            dr["实时车速"] = Vmas_sscs[i].ToString("0.0");
                            dr["加载功率"] = Vmas_jzgl[i].ToString("0.00");
                            dr["扭力"] = Vmas_nl[i].ToString("0");
                            //dr["检测状态"] = Vmas_jczt[i];
                            if (i > 14) dr["检测状态"] = "2";
                            else dr["检测状态"] = "1";
                            dr["环境温度"] = Vmas_hjwd[i];
                            dr["相对湿度"] = Vmas_xdsd[i];
                            dr["大气压力"] = Vmas_dqyl[i];
                            dr["湿度修正系数"] = Vmas_sdxzxs[i].ToString("0.000");
                            dr["稀释修正系数"] = Vmas_xsxzxs[i].ToString("0.000");
                            dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                            dr["转速"] = Vmas_Exhaust_zs[i];
                            dr["油温"] = Vmas_Exhaust_yw[i];
                            dr["寄生功率"] = Vmas_Exhaust_jsgl[i].ToString("0.00");
                            dr["指示功率"] = vmas_zsgl[i].ToString("0.00");
                            vmas_datatable.Rows.Add(dr);
                            rev5025 += asm_Exhaust_rev5025[i];
                            lambda5025 += asm_Exhaust_lambda5025[i];
                        }
                        asm_data.Rev5025 = (rev5025 / 25).ToString("0");
                        asm_data.Lambda5025 = (lambda5025 / 25).ToString("0.00");
                        fast5025isOK = true;
                        asm_data.Kstg5025 = "1";
                        //statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIANCEZHONG, JCSJ.ToString());
                        testState = TEST_STATE.STATE_FINISH;
                        igbt.Set_Control_Power(0f);
                    }
                }
                if (!fast5025isOK)
                {
                    while (GKSJ < 90)
                    {
                        if (!exhaustIsCb5025)//
                        {
                            ts2 = "取样中..." + (90 - GKSJ).ToString() + "s";
                            Thread.Sleep(200);//在5s秒稳定住
                        }
                        else//如查有任意连续10秒超过限值的500%则检测不合格
                        {
                            asm_data.Hc5025 = exhaustHcCb.ToString("0");
                            asm_data.Co5025 = exhaustCoCb.ToString("0.00");
                            asm_data.No5025 = exhaustNoCb.ToString("0");
                            //asm_data.Co25025 = exhaustCo2Cb.ToString("0.00");
                            //asm_data.O25025 = exhaustO2Cb.ToString("0.00");
                            asm_data.Hc5025pd = "合格";
                            asm_data.Co5025pd = "合格";
                            asm_data.No5025pd = "合格";
                            if (exhaustHcCb > carbj.Xz1)
                                asm_data.Hc5025pd = "不合格";
                            if (exhaustCoCb > carbj.Xz2)
                                asm_data.Co5025pd = "不合格";
                            if (exhaustNoCb > carbj.Xz3)
                                asm_data.No5025pd = "不合格";
                            asm_data.Hc2540 = "";
                            asm_data.Co2540 = "";
                            asm_data.No2540 = "";
                            //asm_data.Co22540 = "";
                            //asm_data.O22540 = "";
                            asm_data.Hc2540pd = "";
                            asm_data.Co2540pd = "";
                            asm_data.No2540pd = "";
                            asm_data.RESULT = "不合格";
                            asm5025isOK = true;
                            double rev5025 = 0, lambda5025 = 0;
                            for (int i = 0; i < GKSJ; i++)//将数据写入逐秒数据
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i];
                                dr["时序类别"] = Vmas_sxnb[i];
                                dr["采样时序"] = Vmas_cysx[i];
                                dr["HC实时值"] = Vmas_Exhaust_hcld[i];
                                dr["CO实时值"] = Vmas_Exhaust_cold[i];
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i];
                                dr["NO实时值"] = equipconfig.DATASECONDS_TYPE == "安车通用联网" ? ((Vmas_Exhaust_nold[i] * Vmas_sdxzxs[i]).ToString("0")) : (Vmas_Exhaust_nold[i].ToString("0"));
                                dr["O2实时值"] = Vmas_Exhaust_o2ld[i];
                                //dr["标准时速"] = Vmas_bzcs[i];
                                dr["实时车速"] = Vmas_sscs[i].ToString("0.0");
                                dr["加载功率"] = Vmas_jzgl[i].ToString("0.00");
                                dr["扭力"] = Vmas_nl[i].ToString("0");
                                dr["环境温度"] = Vmas_hjwd[i];
                                dr["相对湿度"] = Vmas_xdsd[i];
                                dr["大气压力"] = Vmas_dqyl[i];
                                dr["湿度修正系数"] = Vmas_sdxzxs[i].ToString("0.000");
                                dr["稀释修正系数"] = Vmas_xsxzxs[i].ToString("0.000");
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                dr["转速"] = Vmas_Exhaust_zs[i];
                                dr["油温"] = Vmas_Exhaust_yw[i];
                                dr["寄生功率"] = Vmas_Exhaust_jsgl[i].ToString("0.00");
                                dr["指示功率"] = vmas_zsgl[i].ToString("0.00");
                                vmas_datatable.Rows.Add(dr);
                                rev5025 += asm_Exhaust_rev5025[i];
                                lambda5025 += asm_Exhaust_lambda5025[i];
                            }
                            asm_data.Rev5025 = (rev5025 / GKSJ).ToString("0");
                            asm_data.Lambda5025 = (lambda5025 / GKSJ).ToString("0.00");
                            break;
                        }
                    }
                    if (!exhaustIsCb5025)//如果90秒前没有污染物超过限值的500%
                    {
                        int effectiveCount = asm_Exhaust_hc5025.Count;
                        double rev5025 = 0, lambda5025 = 0;
                        if (effectiveCount == 0)
                        {
                            ts1 = "过程中无有效点数";
                            ts2 = "检测中止";
                            Msg(label_message, panel_msg, "过程中无有效点数,检测中止", false);
                            button_ss.Text = "重新检测";
                            JC_Status = false;
                            saveProcessData = false;
                            return;
                        }
                        int effctiveindex = 0;
                        for (int index = 0; index < effectiveCount; index++)
                        {
                            if (asm_Exhaust_hc5025[index] > carbj.Xz1 || asm_Exhaust_co5025[index] > carbj.Xz2 || asm_Exhaust_no5025[index] > carbj.Xz3)//如果5025快速工况10秒内的平均值低于等于限值50%则测试结束
                            {
                                effctiveindex = asm_Exhaust_gksj5025[index];
                                asm_data.Hc5025 = asm_Exhaust_hc5025[index].ToString("0");
                                asm_data.Co5025 = asm_Exhaust_co5025[index].ToString("0.00");
                                asm_data.No5025 = asm_Exhaust_no5025[index].ToString("0");
                                asm_data.Hc5025pd = "合格";
                                asm_data.Co5025pd = "合格";
                                asm_data.No5025pd = "合格";
                                if (asm_Exhaust_hc5025[index] > carbj.Xz1)
                                {
                                    asm_data.Hc5025pd = "不合格";
                                }
                                if (asm_Exhaust_co5025[index] > carbj.Xz2)
                                {
                                    asm_data.Co5025pd = "不合格";
                                }
                                if (asm_Exhaust_no5025[index] > carbj.Xz3)
                                {
                                    asm_data.No5025pd = "不合格";
                                }
                                asm_data.Hc2540 = "";
                                asm_data.Co2540 = "";
                                asm_data.No2540 = "";
                                //asm_data.Co22540 = "";
                                //asm_data.O22540 = "";
                                asm_data.Hc2540pd = "";
                                asm_data.Co2540pd = "";
                                asm_data.No2540pd = "";
                                asm_data.RESULT = "不合格";
                                asm5025isOK = true;
                                asm_data.Has2540Tested = "0";
                                break;
                            }
                        }
                        if (!asm5025isOK)
                        {
                            effctiveindex = asm_Exhaust_gksj5025[effectiveCount-1];
                            asm_data.Hc5025 = asm_Exhaust_hc5025[effectiveCount - 1].ToString("0");
                            asm_data.Co5025 = asm_Exhaust_co5025[effectiveCount - 1].ToString("0.00");
                            asm_data.No5025 = asm_Exhaust_no5025[effectiveCount - 1].ToString("0");
                            asm_data.Hc5025pd = "合格";
                            asm_data.Co5025pd = "合格";
                            asm_data.No5025pd = "合格";
                            asm_data.Hc2540 = "";
                            asm_data.Co2540 = "";
                            asm_data.No2540 = "";
                            //asm_data.Co22540 = "";
                            //asm_data.O22540 = "";
                            asm_data.Hc2540pd = "";
                            asm_data.Co2540pd = "";
                            asm_data.No2540pd = "";
                            asm_data.RESULT = "合格";
                        }
                        for (int i = 0; i < 90; i++)//将数据写入逐秒数据
                        {
                            dr = vmas_datatable.NewRow();
                            dr["全程时序"] = Vmas_qcsj[i];
                            dr["时序类别"] = Vmas_sxnb[i];
                            dr["采样时序"] = Vmas_cysx[i];
                            dr["HC实时值"] = Vmas_Exhaust_hcld[i];
                            dr["CO实时值"] = Vmas_Exhaust_cold[i];
                            dr["CO2实时值"] = Vmas_Exhaust_co2ld[i];
                            dr["NO实时值"] = equipconfig.DATASECONDS_TYPE == "安车通用联网" ? ((Vmas_Exhaust_nold[i] * Vmas_sdxzxs[i]).ToString("0")) : (Vmas_Exhaust_nold[i].ToString("0"));
                            dr["O2实时值"] = Vmas_Exhaust_o2ld[i];
                            //dr["标准时速"] = Vmas_bzcs[i];
                            dr["实时车速"] = Vmas_sscs[i].ToString("0.0");
                            dr["加载功率"] = Vmas_jzgl[i].ToString("0.00");
                            dr["扭力"] = Vmas_nl[i].ToString("0");
                            //dr["检测状态"] = Vmas_jczt[i];
                            if (i >= effctiveindex - 9 && i <= effctiveindex)
                                dr["检测状态"] = "2";
                            else
                                dr["检测状态"] = "1";
                            dr["环境温度"] = Vmas_hjwd[i];
                            dr["相对湿度"] = Vmas_xdsd[i];
                            dr["大气压力"] = Vmas_dqyl[i];
                            dr["湿度修正系数"] = Vmas_sdxzxs[i].ToString("0.000");
                            dr["稀释修正系数"] = Vmas_xsxzxs[i].ToString("0.000");
                            dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                            dr["转速"] = Vmas_Exhaust_zs[i];
                            dr["油温"] = Vmas_Exhaust_yw[i];
                            dr["寄生功率"] = Vmas_Exhaust_jsgl[i].ToString("0.00");
                            dr["指示功率"] = vmas_zsgl[i].ToString("0.00");
                            vmas_datatable.Rows.Add(dr);                            
                            rev5025 += asm_Exhaust_rev5025[i];
                            lambda5025 += asm_Exhaust_lambda5025[i];
                        }
                        asm_data.Rev5025 = (rev5025 / 90).ToString("0");
                        asm_data.Lambda5025 = (lambda5025 / 90).ToString("0.00");
                        
                        //asm_data.Co25025 = asm_Exhaust_co25025[i].ToString("0.00");
                        //asm_data.O25025 = asm_Exhaust_o25025[i].ToString("0.00");
                        
                            
                        }
                        
                    //statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIANCEZHONG, JCSJ.ToString());
                    testState = TEST_STATE.STATE_FINISH;
                    igbt.Set_Control_Power(0f);
                }
                statusconfigini.writeNeuStatusData("Finish5025", JCSJ.ToString());//5025结束
                if (!asmconfig.IsKsgkUsed||!fast5025isOK)
                {
                    if (!asm5025isOK)
                    {
                        testState = TEST_STATE.STATE_ACC2540;
                        asm_data.Has2540Tested = "1";//5025工况检测合格的情况下，检测2540工况，这里标定为2540工况已检
                        jczt = 0;
                        Msg(label_message, panel_msg, "请加速至40km/h", true);
                        ts1 = "测2540工况";
                        ts2 = "请加速至40km/h";
                        sxnb = 3;
                        Thread.Sleep(2000);
                        while (igbt.Speed < 38.5)
                        {
                            if (igbt.Speed < 15)
                                igbt.Set_Control_Power(0);
                            else
                                igbt.Set_Control_Power((float)(((igbt.Speed - 15) * 0.04) * Set_Power2540));
                            Thread.Sleep(100);
                        }
                        statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIANCEZHONG, JCSJ.ToString());
                        statusconfigini.writeNeuStatusData("Testing2540", DateTime.Now.ToString());
                        Msg(label_message, panel_msg, "请保持40km/h±1.5km/h,ASM2540工况开始", true);
                        ts2 = "保持40±1.5km/h";
                        sxnb = 2;
                        testState = TEST_STATE.STATE_ACC2540;
                        startTime = DateTime.Now;
                        while (GKSJ < 5)
                        {
                            ts2 = "保持40km/h..." + (5 - GKSJ).ToString() + "s";
                            Thread.Sleep(200);//在5s秒稳定住
                        }//在5s秒稳定住
                        while (GKSJ < 25)
                        {
                            ts2 = "快速取样中..." + (25 - GKSJ).ToString() + "s";
                            Thread.Sleep(200);//在5s秒稳定住
                        }
                        isFastEffective = true;//判断快速工况第10秒到
                        if (asmconfig.Speed05Monitor)
                        {
                            for (int i = 11; i <= 24; i++)
                            {
                                if (Math.Abs(Vmas_sscs[i] - Vmas_sscs[15]) >= 0.5)
                                    isFastEffective = false;
                            }
                        }
                        if ((asmconfig.Speed05Monitor&& isFastEffective) ||!asmconfig.Speed05Monitor)
                        {                           
                            fast5025hc = 0;
                            fast5025co = 0;
                            fast5025no = 0;
                            fast5025o2 = 0;
                            fast5025co2 = 0;
                            for (int i = 0; i < 10; i++)
                            {
                                fast5025hc += Vmas_Exhaust_hcld[i + 15] * Vmas_xsxzxs[i + 15];
                                fast5025co += Vmas_Exhaust_cold[i + 15] * Vmas_xsxzxs[i + 15];
                                fast5025no += Vmas_Exhaust_nold[i + 15] * Vmas_xsxzxs[i + 15]*Vmas_sdxzxs[i+15];
                                //fast5025hc += Vmas_Exhaust_hcld[i + 15];
                                //fast5025co += Vmas_Exhaust_cold[i + 15];
                                //fast5025no += Vmas_Exhaust_nold[i + 15];
                                fast5025co2 += Vmas_Exhaust_co2ld[i + 15];
                                fast5025o2 += Vmas_Exhaust_o2ld[i + 15];
                            }
                            fast5025hc /= 10f;
                            fast5025co /= 10f;
                            fast5025no /= 10f;
                            fast5025co2 /= 10f;
                            fast5025o2 /= 10f;

                            fast5025hc = (float)(Math.Round(fast5025hc, 0));
                            fast5025co = (float)(Math.Round(fast5025co, 2));
                            fast5025no = (float)(Math.Round(fast5025no, 0));
                            if ((asmconfig.IsAsmHalfXzKsgk&&(fast5025hc <= 0.5f * carbj.Xz4 && fast5025co <= 0.5f * carbj.Xz5 && fast5025no <= 0.5f * carbj.Xz6))||(!asmconfig.IsAsmHalfXzKsgk&&(fast5025hc <= carbj.Xz4 && fast5025co <= carbj.Xz5 && fast5025no <= carbj.Xz6)))//如果5025快速工况10秒内的平均值低于等于限值50%则测试结束
                            {
                                asm_data.Hc2540 = fast5025hc.ToString("0");
                                asm_data.Co2540 = fast5025co.ToString("0.00");
                                asm_data.No2540 = fast5025no.ToString("0");
                                //asm_data.Co22540 = fast5025co2.ToString("0.00");
                                //asm_data.O22540 = fast5025o2.ToString("0.00");
                                asm_data.Hc2540pd = "合格";
                                asm_data.Co2540pd = "合格";
                                asm_data.No2540pd = "合格";
                                asm_data.RESULT = "合格";
                                double rev2540 = 0, lambda2540 = 0;
                                int startseconds = (equipconfig.DATASECONDS_TYPE == "东软甘肃") ? 15 : 0;
                                for (int i = startseconds; i < 25; i++)//将数据写入逐秒数据
                                {
                                    dr = vmas_datatable.NewRow();
                                    dr["全程时序"] = Vmas_qcsj[i];
                                    dr["时序类别"] = Vmas_sxnb[i];
                                    dr["采样时序"] = (int.Parse(Vmas_cysx[i]) + 90).ToString();
                                    dr["HC实时值"] = Vmas_Exhaust_hcld[i];
                                    dr["CO实时值"] = Vmas_Exhaust_cold[i];
                                    dr["CO2实时值"] = Vmas_Exhaust_co2ld[i];
                                    dr["NO实时值"] = equipconfig.DATASECONDS_TYPE == "安车通用联网" ? ((Vmas_Exhaust_nold[i] * Vmas_sdxzxs[i]).ToString("0")) : (Vmas_Exhaust_nold[i].ToString("0"));
                                    dr["O2实时值"] = Vmas_Exhaust_o2ld[i];
                                    //dr["标准时速"] = Vmas_bzcs[i];
                                    dr["实时车速"] = Vmas_sscs[i].ToString("0.0");
                                    dr["加载功率"] = Vmas_jzgl[i].ToString("0.00");
                                    dr["扭力"] = Vmas_nl[i].ToString("0");
                                    //dr["检测状态"] = Vmas_jczt[i];
                                    if (i > 14) dr["检测状态"] = "4";
                                    else dr["检测状态"] = "3";
                                    dr["环境温度"] = Vmas_hjwd[i];
                                    dr["相对湿度"] = Vmas_xdsd[i];
                                    dr["大气压力"] = Vmas_dqyl[i];
                                    dr["湿度修正系数"] = Vmas_sdxzxs[i].ToString("0.000");
                                    dr["稀释修正系数"] = Vmas_xsxzxs[i].ToString("0.000");
                                    dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                    dr["转速"] = Vmas_Exhaust_zs[i];
                                    dr["油温"] = Vmas_Exhaust_yw[i];
                                    dr["寄生功率"] = Vmas_Exhaust_jsgl[i].ToString("0.00");
                                    dr["指示功率"] = vmas_zsgl[i].ToString("0.00");
                                    vmas_datatable.Rows.Add(dr);
                                    rev2540 += asm_Exhaust_rev2540[i];
                                    lambda2540 += asm_Exhaust_lambda2540[i];
                                }
                                asm_data.Rev2540 = (rev2540 / 25).ToString("0");
                                asm_data.Lambda2540 = (lambda2540 / 25).ToString("0.00");
                                fast2540isOK = true;
                                asm_data.Kstg2540 = "1";
                            }
                        }
                        if (!fast2540isOK)
                        {

                            while (GKSJ < 90)
                            {
                                if (!exhaustIsCb2540)//
                                {
                                    ts2 = "取样中..." + (90 - GKSJ).ToString() + "s";
                                    Thread.Sleep(200);//在5s秒稳定住
                                }
                                else//如查有任意连续10秒超过限值的500%则检测不合格
                                {
                                    asm_data.Hc2540 = exhaustHcCb.ToString("0");
                                    asm_data.Co2540 = exhaustCoCb.ToString("0.00");
                                    asm_data.No2540 = exhaustNoCb.ToString("0");
                                    //asm_data.Co22540 = exhaustCo2Cb.ToString("0.00");
                                    //asm_data.O22540 = exhaustO2Cb.ToString("0.00");
                                    asm_data.Hc2540pd = "合格";
                                    asm_data.Co2540pd = "合格";
                                    asm_data.No2540pd = "合格";
                                    if (exhaustHcCb > carbj.Xz4)
                                        asm_data.Hc2540pd = "不合格";
                                    if (exhaustCoCb > carbj.Xz5)
                                        asm_data.Co2540pd = "不合格";
                                    if (exhaustNoCb > carbj.Xz6)
                                        asm_data.No2540pd = "不合格";
                                    asm_data.RESULT = "不合格";
                                    asm2540isOK = true;
                                    double rev2540 = 0, lambda2540 = 0;
                                    for (int i = 0; i < GKSJ; i++)//将数据写入逐秒数据
                                    {
                                        dr = vmas_datatable.NewRow();
                                        dr["全程时序"] = Vmas_qcsj[i];
                                        dr["时序类别"] = Vmas_sxnb[i];
                                        dr["采样时序"] = (int.Parse(Vmas_cysx[i]) + 90).ToString();
                                        dr["HC实时值"] = Vmas_Exhaust_hcld[i];
                                        dr["CO实时值"] = Vmas_Exhaust_cold[i];
                                        dr["CO2实时值"] = Vmas_Exhaust_co2ld[i];
                                        dr["NO实时值"] = equipconfig.DATASECONDS_TYPE == "安车通用联网" ? ((Vmas_Exhaust_nold[i] * Vmas_sdxzxs[i]).ToString("0")) : (Vmas_Exhaust_nold[i].ToString("0"));
                                        dr["O2实时值"] = Vmas_Exhaust_o2ld[i];
                                        //dr["标准时速"] = Vmas_bzcs[i];
                                        dr["实时车速"] = Vmas_sscs[i].ToString("0.0");
                                        dr["加载功率"] = Vmas_jzgl[i].ToString("0.00");
                                        dr["扭力"] = Vmas_nl[i].ToString("0");
                                        dr["检测状态"] = Vmas_jczt[i];
                                        dr["环境温度"] = Vmas_hjwd[i];
                                        dr["相对湿度"] = Vmas_xdsd[i];
                                        dr["大气压力"] = Vmas_dqyl[i];
                                        dr["湿度修正系数"] = Vmas_sdxzxs[i].ToString("0.000");
                                        dr["稀释修正系数"] = Vmas_xsxzxs[i].ToString("0.000");
                                        dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                        dr["转速"] = Vmas_Exhaust_zs[i];
                                        dr["油温"] = Vmas_Exhaust_yw[i];
                                        dr["寄生功率"] = Vmas_Exhaust_jsgl[i].ToString("0.00");
                                        dr["指示功率"] = vmas_zsgl[i].ToString("0.00");
                                        vmas_datatable.Rows.Add(dr);
                                        rev2540 += asm_Exhaust_rev2540[i];
                                        lambda2540 += asm_Exhaust_lambda2540[i];
                                    }
                                    asm_data.Rev2540 = (rev2540 / GKSJ).ToString("0");
                                    asm_data.Lambda2540 = (lambda2540 / GKSJ).ToString("0.00");
                                    break;
                                }
                            }
                            if (!exhaustIsCb2540)
                            {
                                int effectiveCount = asm_Exhaust_hc2540.Count;
                                double rev2540 = 0, lambda2540 = 0;
                                if (effectiveCount == 0)
                                {
                                    ts1 = "过程中无有效点数";
                                    ts2 = "检测中止";
                                    Msg(label_message, panel_msg, "过程中无有效点数,检测中止", false);
                                    button_ss.Text = "重新检测";
                                    JC_Status = false;
                                    saveProcessData = false;
                                    return;
                                }
                                int effctiveindex = 0;

                                for (int index = 0; index < effectiveCount; index++)
                                {
                                    if (asm_Exhaust_hc2540[index] > carbj.Xz4 || asm_Exhaust_co2540[index] > carbj.Xz5 || asm_Exhaust_no2540[index] > carbj.Xz6)//如果5025快速工况10秒内的平均值低于等于限值50%则测试结束
                                    {
                                        effctiveindex = asm_Exhaust_gksj2540[index];
                                        asm_data.Hc2540 = asm_Exhaust_hc2540[index].ToString("0");
                                        asm_data.Co2540 = asm_Exhaust_co2540[index].ToString("0.00");
                                        asm_data.No2540 = asm_Exhaust_no2540[index].ToString("0");
                                        asm_data.Hc2540pd = "合格";
                                        asm_data.Co2540pd = "合格";
                                        asm_data.No2540pd = "合格";
                                        if (asm_Exhaust_hc2540[index] > carbj.Xz4)
                                            asm_data.Hc2540pd = "不合格";
                                        if (asm_Exhaust_co2540[index] > carbj.Xz5)
                                            asm_data.Co2540pd = "不合格";
                                        if (asm_Exhaust_no2540[index]> carbj.Xz6)
                                            asm_data.No2540pd = "不合格";
                                        asm_data.RESULT = "不合格";
                                        asm2540isOK = true;
                                        break;
                                    }
                                }
                                if (!asm2540isOK)
                                {
                                    effctiveindex = asm_Exhaust_gksj2540[effectiveCount - 1];
                                    asm_data.Hc2540 = asm_Exhaust_hc2540[effectiveCount - 1].ToString("0");
                                    asm_data.Co2540 = asm_Exhaust_co2540[effectiveCount - 1].ToString("0.00");
                                    asm_data.No2540 = asm_Exhaust_no2540[effectiveCount - 1].ToString("0");
                                    asm_data.Hc2540pd = "合格";
                                    asm_data.Co2540pd = "合格";
                                    asm_data.No2540pd = "合格";
                                    asm_data.RESULT = "合格";
                                }
                                for (int i = 0; i < 90; i++)//将数据写入逐秒数据
                                {
                                    dr = vmas_datatable.NewRow();
                                    dr["全程时序"] = Vmas_qcsj[i];
                                    dr["时序类别"] = Vmas_sxnb[i];
                                    dr["采样时序"] = (int.Parse(Vmas_cysx[i]) + 90).ToString();
                                    dr["HC实时值"] = Vmas_Exhaust_hcld[i];
                                    dr["CO实时值"] = Vmas_Exhaust_cold[i];
                                    dr["CO2实时值"] = Vmas_Exhaust_co2ld[i];
                                    dr["NO实时值"] = equipconfig.DATASECONDS_TYPE == "安车通用联网" ? ((Vmas_Exhaust_nold[i] * Vmas_sdxzxs[i]).ToString("0")) : (Vmas_Exhaust_nold[i].ToString("0"));
                                    dr["O2实时值"] = Vmas_Exhaust_o2ld[i];
                                    //dr["标准时速"] = Vmas_bzcs[i];
                                    dr["实时车速"] = Vmas_sscs[i].ToString("0.0");
                                    dr["加载功率"] = Vmas_jzgl[i].ToString("0.00");
                                    dr["扭力"] = Vmas_nl[i].ToString("0");
                                    //dr["检测状态"] = Vmas_jczt[i];
                                    if (i >= effctiveindex - 9 && i <= effctiveindex)
                                        dr["检测状态"] = "4";
                                    else
                                        dr["检测状态"] = "3";
                                    dr["环境温度"] = Vmas_hjwd[i];
                                    dr["相对湿度"] = Vmas_xdsd[i];
                                    dr["大气压力"] = Vmas_dqyl[i];
                                    dr["湿度修正系数"] = Vmas_sdxzxs[i].ToString("0.000");
                                    dr["稀释修正系数"] = Vmas_xsxzxs[i].ToString("0.000");
                                    dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                    dr["转速"] = Vmas_Exhaust_zs[i];
                                    dr["油温"] = Vmas_Exhaust_yw[i];
                                    dr["寄生功率"] = Vmas_Exhaust_jsgl[i].ToString("0.00");
                                    dr["指示功率"] = vmas_zsgl[i].ToString("0.00");
                                    vmas_datatable.Rows.Add(dr);                                    
                                    rev2540 += asm_Exhaust_rev2540[i];
                                    lambda2540 += asm_Exhaust_lambda5025[i];
                                    
                                }
                                asm_data.Rev2540 = (rev2540 / 90).ToString("0");
                                asm_data.Lambda2540 = (lambda2540 / 90).ToString("0.00");
                                                              
                                    
                                }

                        }
                        statusconfigini.writeNeuStatusData("Finish2540", JCSJ.ToString());//5025结束
                    }
                }
                for (int i = 0; i < other_Vmas_qcsj.Count; i++)//将准备阶段，加速阶段的值存入到列表中
                {
                    dr = vmas_datatable.NewRow();
                    dr["全程时序"] = other_Vmas_qcsj[i];
                    dr["时序类别"] = other_Vmas_sxnb[i];
                    dr["采样时序"] = other_Vmas_cysx;
                    dr["HC实时值"] = other_Vmas_Exhaust_hcld[i].ToString("0");
                    dr["CO实时值"] = other_Vmas_Exhaust_cold[i].ToString("0.00");
                    dr["CO2实时值"] = other_Vmas_Exhaust_co2ld[i].ToString("0.00");
                    dr["NO实时值"] = equipconfig.DATASECONDS_TYPE == "安车通用联网" ? ((other_Vmas_Exhaust_nold[i] * other_Vmas_sdxzxs[i]).ToString("0")) : (other_Vmas_Exhaust_nold[i].ToString("0"));
                    dr["O2实时值"] = other_Vmas_Exhaust_o2ld[i].ToString("0.00");
                    //dr["标准时速"] = Vmas_bzcs[i];
                    dr["实时车速"] = other_Vmas_sscs[i].ToString("0.0");
                    dr["加载功率"] = other_Vmas_jzgl[i].ToString("0.00");
                    dr["扭力"] = other_Vmas_nl[i].ToString("0");
                    dr["检测状态"] = "3";
                    dr["环境温度"] = other_Vmas_hjwd[i].ToString("0.0");
                    dr["相对湿度"] = other_Vmas_xdsd[i].ToString("0.0");
                    dr["大气压力"] = other_Vmas_dqyl[i].ToString("0.0");
                    dr["湿度修正系数"] = other_Vmas_sdxzxs[i].ToString("0.000");
                    dr["稀释修正系数"] = other_Vmas_xsxzxs[i].ToString("0.000");
                    dr["分析仪管路压力"] = other_Vmas_Exhaust_qlyl[i];
                    dr["转速"] = other_Vmas_Exhaust_zs[i];
                    dr["油温"] = other_Vmas_Exhaust_yw[i];
                    dr["寄生功率"] = other_Vmas_Exhaust_jsgl[i].ToString("0.00");
                    dr["指示功率"] = other_vmas_zsgl[i].ToString("0.00");
                    vmas_datatable.Rows.Add(dr);
                }
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.GUOCHE, JCSJ.ToString());
                statusconfigini.writeGlStatusData(statusconfigIni.ENUM_GL_STATUS.STATUS_ENDSAMPLE, "");
                JC_Status = false; saveProcessData = false;
                jcStatus = false;
                testState = TEST_STATE.STATE_FINISH;
                asm_data.CarID = carbj.CarID;
                asm_data.Sd = SD.ToString("0.0");
                asm_data.Wd = WD.ToString("0.0");
                asm_data.Dqy = DQY.ToString("0.0");
                asm_data.Power2540 = Set_Power2540.ToString("0.00");
                asm_data.Power5025 = Set_Power5025.ToString("0.00");
                asm_data.StopReason = "0";
                igbt.Set_Control_Power(0f);
                igbt.Exit_Control();
                Msg(label_message, panel_msg, "测试结束,请撤离探头,等举升升起后驶离测功机", false);
                ts1 = "测量结束";
                ts2 = "请撤离探头";
                Th_get_FqandLl.Abort();
                Thread.Sleep(1000);
                fla_502.StopBlowback();
                asm_data.DjccyString = JCSJ.ToString();
                asm_data.SjxlString = sjxlString.Remove(sjxlString.Length - 1);
                asm_data.HcclzString = hcclzString.Remove(hcclzString.Length - 1);
                asm_data.NoclzString = noclzString.Remove(noclzString.Length - 1);
                asm_data.CoclzString = coclzString.Remove(coclzString.Length - 1);
                asm_data.Co2clzString = co2clzString.Remove(co2clzString.Length - 1);
                asm_data.O2clzString = o2clzString.Remove(o2clzString.Length - 1);
                asm_data.CsString = csString.Remove(csString.Length - 1);
                asm_data.ZsString = zsString.Remove(zsString.Length - 1);
                asm_data.XsxzxsString = xsxzxsString.Remove(xsxzxsString.Length - 1);
                asm_data.SdxzxsString = sdxzxsString.Remove(sdxzxsString.Length - 1);
                asm_data.JsglString = jsglString.Remove(jsglString.Length - 1);
                asm_data.ZsglString = zsglString.Remove(zsglString.Length - 1);
                asm_data.HjwdString = hjwdString.Remove(hjwdString.Length - 1);
                asm_data.DqylString = dqylString.Remove(dqylString.Length - 1);
                asm_data.XdsdString = xdsdString.Remove(xdsdString.Length - 1);
                asm_data.YwString = ywString.Remove(ywString.Length - 1);
                asm_data.JcztString = jcztString.Remove(jcztString.Length - 1);
                ig195IsFinished = true;
                Msg(label_message, panel_msg, "测试完毕,举升升起后请驶离测功机", true);
                if(asmconfig.Flowback)
                {
                    Thread.Sleep(1000);
                    fla_502.Blowback();
                }
                while (igbt.Speed > 0.5f) Thread.Sleep(500);
                ts2 = "驶离测功机";
                igbt.Lifter_Up();
                Thread.Sleep(1000);
                csvwriter.SaveCSV(vmas_datatable, "C:/jcdatatxt/" + carbj.CarID + ".csv");
                asmdatacontrol.writeAsmData(asm_data);//写carID.ini文件
                Thread.Sleep(1000);
                driverformmin = true;
                if (asmconfig.Flowback)
                {
                    if (asmconfig.FlowTime > 0)
                    {                 
                        for (int i = asmconfig.FlowTime; i >= 0; i--)
                        {
                            Thread.Sleep(1000);
                            Msg(label_message, panel_msg, "反吹... " + i.ToString() + "s", true);
                            ts2 = "反吹... " + i.ToString() + "s";
                        }
                    }
                    fla_502.StopBlowback();//停止反吹
                }
                Thread.Sleep(1000);
                fla_502.StopBlowback();
                button_ss.Text = "重新检测";
                button_ss.Enabled = true;
                gongkuangTime = 0f;
                GKSJ = 0;
                Thread.Sleep(1000);
                this.Close();

            }
            catch (Exception)
            {
            }
        }
        #endregion
        /// <summary>
        /// 气象修正
        /// </summary>
        /// <param name="Exhaust_data"></param>
        /// <returns></returns>
        public float caculateDf(double co2, double co)
        {
            try
            {
                int wd = 25;                                  //气象修正用的温度
                double X = 1;
                if (co2 + co == 0) X = 1;
                else
                    X = co2 / (co2 + co);//
                double a = 1;
                if (carbj.CarRlzl == "3") a = 6.64f;//石油气
                else if (carbj.CarRlzl == "4") a = 5.39f;//天燃气
                else a = 4.644f;//汽油
                double CO2x = (float)(X / (a + 1.88 * X) * 100);
                double DF = 0;
                if (co2 == 0)
                    DF = 1;
                else
                {
                    DF = CO2x / co2;             //计算稀释系数
                    if (DF > 3)                                 //如果稀释系数
                        DF = 3;
                }
                return (float)DF;
            }
            catch
            {
                return 1;
            }
        }
        /// <summary>
        /// 气象修正
        /// </summary>
        /// <param name="Exhaust_data"></param>
        /// <returns></returns>
        public float caculateKh(double temp, double humidity, double airpressure)
        {
            try
            {
                int wd = (int)temp;
                if (wd > 30)
                    wd = 30;
                if (wd <= 0) wd = 0;
                double H = (double)(43.478 * humidity * Pd[wd] / (airpressure - Pd[wd] * humidity / 100));
                double kH = (double)(1 / (1 - 0.0047 * (H - 75)));    //湿度校正系数
                return (float)kH;
            }
            catch
            {
                return 1;
            }
        }
        /// <summary>
        /// 气象修正
        /// </summary>
        /// <param name="Exhaust_data"></param>
        /// <returns></returns>
        public void Revise(Exhaust.Fla502_data Exhaust_data)
        {
            try
            {
                int wd = 25;                                  //气象修正用的温度
                float X = 1;
                if (Exhaust_data.CO2 + Exhaust_data.CO == 0) X = 1;
                else
                    X = (float)Exhaust_data.CO2 / (Exhaust_data.CO2 + Exhaust_data.CO);//
                float a = 1;
                if (carbj.CarRlzl == "2") a = 6.64f;//石油气
                else if (carbj.CarRlzl == "3") a = 5.39f;//天燃气
                else a = 4.64f;//汽油
                float CO2x = (float)(X / (a + 1.88 * X) * 100);
                float DF = 0;
                if (Exhaust_data.CO2 == 0)
                    DF = 0;
                else
                {
                    DF = CO2x / Exhaust_data.CO2;             //计算稀释系数
                    if (DF > 3)                                 //如果稀释系数
                        DF = 3;
                }
                wd = (int)Exhaust_data.HJWD;
                if (wd > 30)
                    wd = 30;
                if (wd <= 0) wd = 0;
                float H = (float)(43.478 * Exhaust_data.SD * Pd[wd] / (Exhaust_data.HJYL - Pd[wd] * Exhaust_data.SD / 100));
                float kH = (float)(1 / (1 - 0.0047 * (H - 75)));    //湿度校正系数
                sdxzxs = kH;
                xsxzxs = DF;
                Vmas_Exhaust_ReviseNow.CO = Vmas_Exhaust_Now.CO * DF;
                Vmas_Exhaust_ReviseNow.HC = Vmas_Exhaust_Now.HC * DF;
                Vmas_Exhaust_ReviseNow.NO = Vmas_Exhaust_Now.NO * DF * kH;
            }
            catch
            { }
        }
        private void button_ss_Click(object sender, EventArgs e)
        {
            try
            {
                if (JC_Status == false)
                {
                    TH_ST = new Thread(Jc_Exe);
                    timer2.Start();
                    Th_get_FqandLl = new Thread(Fq_Detect);
                    Th_get_FqandLl.Start();
                    TH_ST.Start();
                    JC_Status = true;
                    button_ss.Text = "停止检测";
                    igbt.Lifter_Down();     //举升下降
                }
                else
                {
                    Th_get_FqandLl.Abort();
                    TH_ST.Abort();
                    testState = TEST_STATE.STATE_FINISH;
                    timer2.Stop();
                    JC_Status = false; saveProcessData = false;
                    button_ss.Text = "重新检测";
                    gongkuangTime = 0f;
                    GKSJ = 0;
                }
            }
            catch (Exception)
            {
            }
        }

        private void vmas_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!ig195IsFinished)
                {
                    if (MessageBox.Show("测试未完成，确认退出？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        if (ig195IsFinished == false)
                        {

                            asm_data.CarID = carbj.CarID;//车辆ID
                            asm_data.Wd = "-1";//温度
                            asm_data.Sd = "-1";//湿度
                            asm_data.Dqy = "-1";//大气压
                            asm_data.Hc5025 = "-1";
                            asm_data.Co5025 = "-1";
                            asm_data.No5025 = "-1";
                            //asm_data.Co25025 = "-1";
                            //asm_data.O25025 = "-1";
                            asm_data.Hc5025pd = "-1";
                            asm_data.Co5025pd = "-1";
                            asm_data.No5025pd = "-1";
                            asm_data.Hc2540 = "-1";
                            asm_data.Co2540 = "-1";
                            asm_data.No2540 = "-1";
                           // asm_data.Co22540 = "-1";
                            //asm_data.O22540 = "-1";
                            asm_data.Hc2540pd = "-1";
                            asm_data.Co2540pd = "-1";
                            asm_data.No2540pd = "-1";
                            asm_data.RESULT = "-1";
                            asm_data.StartTime = "-1";
                            asm_data.StopReason = "9";
                            asm_data.Rev5025 = "-1";
                            asm_data.Lambda5025 = "-1";
                            asm_data.Power5025 = "-1";
                            asm_data.Rev2540 = "-1";
                            asm_data.Lambda2540 = "-1";
                            asm_data.Power2540 = "-1";
                            asm_data.Has2540Tested = "0";
                            asm_data.Has5025Tested = "0";
                            asm_data.RESULT = "-1";
                            asmdatacontrol.writeAsmData(asm_data);
                        }
                        if (Th_get_FqandLl != null) Th_get_FqandLl.Abort();
                        if (TH_ST != null) TH_ST.Abort();
                        timer2.Stop();
                        try
                        {
                            if (flb_100 != null)
                            {
                                if (flb_100.ComPort_3.IsOpen)
                                    flb_100.ComPort_3.Close();
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
                            if (igbt != null)
                            {
                                igbt.closeIgbt();
                            }
                            if (xce_100 != null)
                            {
                                if (xce_100.ComPort_1.IsOpen)
                                    xce_100.ComPort_1.Close();
                            }
                            if (nhsjz != null)
                            {
                                if (nhsjz.ComPort_1.IsOpen)
                                    nhsjz.ComPort_1.Close();
                            }
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
                            if (Th_get_FqandLl != null) Th_get_FqandLl.Abort();
                            if (TH_ST != null) TH_ST.Abort();
                            timer2.Stop();
                            try
                            {
                                if (flb_100 != null)
                                {
                                    if (flb_100.ComPort_3.IsOpen)
                                        flb_100.ComPort_3.Close();
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

                                if (igbt != null)
                                {
                                    igbt.closeIgbt();
                                }
                                if (xce_100 != null)
                                {
                                    if (xce_100.ComPort_1.IsOpen)
                                        xce_100.ComPort_1.Close();
                        }
                        if (nhsjz != null)
                        {
                            if (nhsjz.ComPort_1.IsOpen)
                                nhsjz.ComPort_1.Close();
                        }
                    }
                            catch
                            { }

                }

            }
            catch (Exception)
            {
            }
        }

        #region 信息显示
        public void datagridview_msg(DataGridView datagridview, string title, int row_number, string message)
        {
            BeginInvoke(new wtdtview(dt_msg), datagridview, title, row_number, message);
        }
        public void dt_msg(DataGridView datagridview, string title, int row_number, string message)
        {
            datagridview.Rows[row_number].Cells[title].Value = message;
        }
        public void panel_visible(Panel panel, bool visible_value)
        {
            BeginInvoke(new wtpanelvisible(panel_show), panel, visible_value);
        }
        public void panel_show(Panel panel, bool visible_value)
        {
            panel.Visible = visible_value;
        }
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
        public void Label_Msg(Label Msgowner, string Msgstr)
        {
            BeginInvoke(new wtlm(Label_Show), Msgowner, Msgstr);
        }
        public void Label_Show(Label Msgowner, string Msgstr)
        {
            Msgowner.Text = Msgstr;
        }
        public void Msg_Show(Label Msgowner, string Msgstr, bool Update_DB)
        {
            Msgowner.Text = Msgstr;
        }

        public void Msg_Position(Label Msgowner, Panel Msgfather)
        {
            if (Msgowner.Width < Msgfather.Width)
                Msgowner.Location = new Point((Msgfather.Width - Msgowner.Width) / 2, Msgowner.Location.Y);
            else
                Msgowner.Location = new Point(0, Msgowner.Location.Y);
        }
        public void textbox_enable(TextBox textbox, bool value)
        {
            BeginInvoke(new wttextboxenable(textboxEnable), textbox, value);
        }
        public void textboxEnable(TextBox textbox, bool value)
        {
            textbox.Enabled = value;
        }
        public void textbox_value(TextBox textbox, string value)
        {
            BeginInvoke(new wttextboxvalue(textboxValue), textbox, value);
        }
        public void textboxValue(TextBox textbox, string value)
        {
            textbox.Text = value;
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                switch (igbt.Status)
                {
                    default:
                        if (testState==TEST_STATE.STATE_TEST5025)
                        {
                            linearScaleComponentSpeed25.Value = (igbt.Speed - 23.5f) * 100f / 3f;
                        }
                        else if (testState==TEST_STATE.STATE_TEST2540)
                        {
                            linearScaleComponent40.Value = (igbt.Speed - 38.5f) * 100f / 3f;
                        }
                        else
                        {
                            linearScaleComponentSpeed25.Value = 0f;
                            linearScaleComponent40.Value = 0f;
                        }
                        ledNumberForce.LEDText = igbt.Force.ToString("0");
                        ledNumberGl.LEDText = igbt.Power.ToString("0.00");
                        arcScaleComponentForce.Value = igbt.Force;
                        arcScaleComponentGl.Value = igbt.Power;
                        arcScaleComponentSpeed.Value = igbt.Speed;
                        labelComponentSpeed.Text = igbt.Speed.ToString("0.0");
                        label_msg.Text = igbt.Msg;
                        break;
                }

                // label_msg.Text = CarWait.igbt.Msg;
            }
            catch (Exception)
            {

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

        private void toolStripButtonForceClear_Click(object sender, EventArgs e)
        {
            try
            {
                igbt.Force_Zeroing();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString(), "系统提示");
            }
        }

        private void toolStripButtonLiftUp_Click(object sender, EventArgs e)
        {
            igbt.Lifter_Up();
        }

        private void toolStripButtonLiftDown_Click(object sender, EventArgs e)
        {
            igbt.Lifter_Down();
        }

        private void toolStripButtonMotorOn_Click(object sender, EventArgs e)
        {
            igbt.Motor_Open();
        }

        private void toolStripButtonMotorOff_Click(object sender, EventArgs e)
        {
            igbt.Motor_Close();
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

        private void toolStripButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButtonFc_Click(object sender, EventArgs e)
        {
            if (toolStripButtonFc.Text == "反吹")
            {
                toolStripButtonFc.Text = "停止反吹";
                fla_502.Blowback();
            }
            else
            {
                toolStripButtonFc.Text = "反吹";
                fla_502.StopBlowback();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            settings newSettings = new settings();
            newSettings.ShowDialog();
            initConfigInfo();
        }

        private void buttonBlowback_Click(object sender, EventArgs e)
        {
            fla_502.Blowback();
        }

        private void buttonStopBlowback_Click(object sender, EventArgs e)
        {
            fla_502.StopBlowback();
        }


        private void button_ss_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (jcStatus == false)
                {
                    isUseData = false;
                    djccyString = "";
                    sjxlString = "";
                    hcclzString = "";
                    noclzString = "";
                    coclzString = "";
                    co2clzString = "";
                    o2clzString = "";
                    csString = "";
                    zsString = "";
                    xsxzxsString = "";
                    sdxzxsString = "";
                    jsglString = "";
                    zsglString = "";
                    hjwdString = "";
                    dqylString = "";
                    xdsdString = "";
                    ywString = "";
                    jcztString = "";
                    ovalShapeTXZK.FillColor = Color.Lime;
                    ovalShapeLJCC.FillColor = Color.Lime;
                    ovalShapeNDZ.FillColor = Color.Lime;
                    ovalShapeDLL.FillColor = Color.Lime;
                    ovalShapeJZGL.FillColor = Color.Lime;
                    led_display(ledNumber_gksj, "000.0");
                    led_display(ledNumber_lxcc, "0.0");
                    led_display(ledNumberCO, "0.0");
                    led_display(ledNumberCO2, "0.0");
                    led_display(ledNumberNO, "0");
                    led_display(ledNumberHC, "0");
                    led_display(ledNumberO2, "0.0");
                    led_display(ledNumberLL, "0.0");
                    //textBoxSDSD.Enabled = false;
                    //textBoxSDWD.Enabled = false;
                    igbt.Force_Zeroing();
                    //toolStripButtonLiftUp.Enabled = false;
                    //toolStripButtonLiftDown.Enabled = false;
                    gksj_count = 0;
                    power_flag = false;
                    jctime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    TH_ST = new Thread(Jc_Exe);
                    JCSJ = 0;
                    preJCSJ = 0;
                    JC_Status = false; saveProcessData = false;
                    Th_get_FqandLl = new Thread(Fq_Detect);
                    TH_ST.Start();
                    button_ss.Text = "停止检测";
                    testState = TEST_STATE.STATE_FINISH;
                    GKSJ = 0;
                    jczt = 0;
                    timer2.Start();
                }
                else
                {
                    if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Left)
                    {
                        igbt.Exit_Control();
                        JC_Status = false; saveProcessData = false;
                        jcStatus = false;
                        //toolStripButtonLiftUp.Enabled = true;
                        //toolStripButtonLiftDown.Enabled = true;
                        Th_get_FqandLl.Abort();
                        TH_ST.Abort();
                        fla_502.Stop();
                        testState = TEST_STATE.STATE_FINISH;
                        timer2.Stop();
                        button_ss.Text = "重新检测";
                        ts2 = "检测被终止";
                        gongkuangTime = 0f;
                        JCSJ = 0;
                        preJCSJ = 0;
                        GKSJ = 0;
                        jczt = 0;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("操作失败，请检查仪器工作是否正常", "系统错误");
            }
        }

        private void button_ss_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (jcStatus == false)
                {
                    if (File.Exists(@"C:\jcdatatxt\asmco2.dll"))                    
                        asmconfig.IsFixCo2 = true;
                    label19.ForeColor = asmconfig.IsFixCo2 ? Color.White : Color.Red;
                    isUseData = false;
                    djccyString = "";
                    sjxlString = "";
                    hcclzString = "";
                    noclzString = "";
                    coclzString = "";
                    co2clzString = "";
                    o2clzString = "";
                    csString = "";
                    zsString = "";
                    xsxzxsString = "";
                    sdxzxsString = "";
                    jsglString = "";
                    zsglString = "";
                    hjwdString = "";
                    dqylString = "";
                    xdsdString = "";
                    ywString = "";
                    jcztString = "";
                    ovalShapeTXZK.FillColor = Color.Lime;
                    ovalShapeLJCC.FillColor = Color.Lime;
                    ovalShapeNDZ.FillColor = Color.Lime;
                    ovalShapeDLL.FillColor = Color.Lime;
                    ovalShapeJZGL.FillColor = Color.Lime;
                    led_display(ledNumber_gksj, "000.0");
                    led_display(ledNumber_lxcc, "0.0");
                    led_display(ledNumberCO, "0.0");
                    led_display(ledNumberCO2, "0.0");
                    led_display(ledNumberNO, "0");
                    led_display(ledNumberHC, "0");
                    led_display(ledNumberO2, "0.0");
                    led_display(ledNumberLL, "0.0");
                    //textBoxSDSD.Enabled = false;
                    //textBoxSDWD.Enabled = false;

                    if (igbt != null)
                    {
                        igbt.Force_Zeroing();
                        Thread.Sleep(500);
                        Msg(label_message, panel_msg, "检测开始,举升下降", false);
                        ts1 = "举升下降...";
                        igbt.Lifter_Down();     //举升下降
                        Thread.Sleep(5000);
                    }
                    //toolStripButtonLiftUp.Enabled = false;
                    //toolStripButtonLiftDown.Enabled = false;
                    gksj_count = 0;
                    power_flag = false;
                    jctime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    TH_ST = new Thread(Jc_Exe);
                    JCSJ = 0;
                    preJCSJ = 0;
                    JC_Status = false; saveProcessData = false;
                    Th_get_FqandLl = new Thread(Fq_Detect);
                    TH_ST.Start();
                    button_ss.Text = "停止检测";
                    testState = TEST_STATE.STATE_FINISH;
                    GKSJ = 0;
                    jczt = 0;
                    timer2.Start();
                }
                else
                {
                        igbt.Exit_Control();
                        JC_Status = false; saveProcessData = false;
                    jcStatus = false;
                        //toolStripButtonLiftUp.Enabled = true;
                        //toolStripButtonLiftDown.Enabled = true;
                        Th_get_FqandLl.Abort();
                        TH_ST.Abort();
                        fla_502.Stop();
                    testState = TEST_STATE.STATE_FINISH;
                    timer2.Stop();
                        button_ss.Text = "重新检测";
                        ts2 = "检测被终止";
                        gongkuangTime = 0f;
                        JCSJ = 0;
                        preJCSJ = 0;
                        GKSJ = 0;
                        jczt = 0;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("操作失败，请检查仪器工作是否正常", "系统错误");
            }
        }

        private void buttonSDSR_Click(object sender, EventArgs e)
        {
            wsdValueIsRight = false;
            wsd_sure = true;
        }

        private void buttonQR_Click(object sender, EventArgs e)
        {
            wsdValueIsRight = true;
            wsd_sure = true;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            DateTime nowtime = DateTime.Now;
            TimeSpan timespan = nowtime - jcStarttime;
            jcTime = (float)timespan.TotalMilliseconds / 1000f;
            JCSJ = (int)jcTime;
        }

    }
}
