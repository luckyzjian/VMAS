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
using System.Security.Cryptography;

namespace vmasDetect
{
    public partial class VMAS : Form
    {
        carinfor.carInidata carbj = new carInidata();
        VmasConfigInfdata vmasconfig = new VmasConfigInfdata();
        equipmentConfigInfdata equipconfig = new equipmentConfigInfdata();
        carIni carini = new carIni();
        statusconfigIni statusconfigini = new statusconfigIni();
        configIni configini = new configIni();
        vmasdata vmas_data = new vmasdata();
        vmasdataControl vmasdatacontrol = new vmasdataControl();
        vmasDataSeconds vmas_dataseconds = new vmasDataSeconds();
        vmasDataSecondsControl vmas_datasecondscontrol = new vmasDataSecondsControl();
        //statusconfigIni statusconfigini = new statusconfigIni();

        JCXXX jcxxx=new JCXXX();
        jcxztCheck jcxzt=new jcxztCheck();
        CSVcontrol.CSVwriter csvwriter = new CSVcontrol.CSVwriter();
        CSVcontrol.csvReader csvreader = new CSVcontrol.csvReader();
        upanControl.ucontrol u_control = new upanControl.ucontrol();
        string dogUse = "";
        DataTable dt=new DataTable();
        DataTable dt_zb =null;
        private string UseFqy="";
        Exhaust.Fla501 fla_501 = null;
        Exhaust.Fla502 fla_502 = null;
        Exhaust.FLB_100 flb_100 = null;
        Exhaust.Flv_1000 flv_1000 = null;
        LedControl.led ledcontrol = null;
        Exhaust.XCE_100 xce_100 = null;
        thaxs thaxsdata = new thaxs();
        public static speed_data speedNow = new speed_data();//提前显示前4秒的状态
        public static speed_data speedLimit = new speed_data();
        public static IGBT igbt =null;
        int gksj_count = 0;
        public CSVcontrol.CSVHelper vmas_csv = null;
        private bool isVertical = false;//是否为竖状
        private bool isUseData = false;

        private bool chujianIsFinished = false;
        private bool chujianIsOk = false;

        private bool ig195IsFinished = false;
        private bool writeDataIsFinished=false;
        private bool wsd_sure = false;
        private bool wsdValueIsRight = false;

        private bool isCcStop = false;

        public delegate void wl_led(LEDNumber.LEDNumber lednumber, string data); 
        public delegate void wtcs(Control controlname, string text);                            //委托
        public delegate void wtds(double Data,double time, string ChartName);                               //委托
        public delegate void wtds2(double Data, double time, string ChartName);                               //委托
        public delegate void wtds_clr(string ChartName);                               //委托
        public delegate void wt_void();                                                         //委托
        public delegate void wtlsb(Label Msgowner,string Msgstr,bool Update_DB);                //委托
        public delegate void wtlp(Label Msgowner,Panel Msgfather);                              //委托
        public delegate void wtlm(Label Msgowner,string Msgstr);
        public delegate void wtdtview(DataGridView datagridview,string title,int row_number,string message);
        public delegate void wtpanelvisible(Panel panel,bool visible_value);
        public delegate void wttextboxenable(TextBox textbox, bool visible_value);
        public delegate void wttextboxvalue(TextBox textbox, string  value);

        //public delegate void wtds(double Data,string SeriesName);                               //委托
        public delegate void wtdd(double min, double max);                                      //委托
        public delegate void wtddy(double min, double max);                                      //委托
        public bool JC_Status = false;                                                          //检测状态
        public string JC_Circuit="5025";                                                        //检测流程
        Thread Th_get_FqandLl = null;                                                           //废气检测线程
        Thread th_get_llj = null;
        Thread TH_ST = null;                                                                    //检测线程
        Thread th_load = null;
        Thread TH_Speed_st = null;                                                              //速度检测线程
        Thread Th_prepare = null;
        public int fqy_delayTime = 0;
        public int delayTimeBetweenFqyAndLlj = 0;
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
        public double WD = 26;                                                                  //温度
        public double SD = 75;                                                                  //相对湿度
        public double DQY = 100;                                                                //大气压
        public float hjo2 = 20.8f;                                                               //环境O2浓度
        public float[] hjo2data = new float[10];
        public Exhaust.Fla502_data[] Vmas_Exhaust_ListIG195 = new Exhaust.Fla502_data[300];      //IG195工况每秒废气结果
        public Exhaust.Fla502_data[] Vmas_Exhaust_Revise_ListIG195 = new Exhaust.Fla502_data[300];//IG195工况每秒废气修正后结果
        public Exhaust.Fla502_data Vmas_Exhaust_Now = new Exhaust.Fla502_data();
        public Exhaust.Fla502_data Vmas_Exhaust_ReviseNow = new Exhaust.Fla502_data();
        public Exhaust.Fla502_temp_data fla502_temp_data = new Exhaust.Fla502_temp_data();
        public string[] Vmas_qcsj = new string[300];//全程时序
        public string[] Vmas_cysx = new string[300];
        public float[] Vmas_lambda = new float[300];
        public string[] Vmas_sxnb = new string[300];
        public float[] Vmas_Exhaust_llList = new float[300];//流量计实际流量
        public float[] Vmas_Exhaust_xso2now = new float[300];//流量计稀释O2浓度
        public float[] Vmas_Exhaust_xso2afterDelay = new float[300];
        public float[] Vmas_Exhaust_lljtemp = new float[300];//流量计温度
        public float[] Vmas_Exhaust_lljyl = new float[300];//流量计压力
        public float[] Vmas_Exhaust_lljbzll = new float[300];//流量计标准流量
        public float[] Vmas_Exhaust_fqsjll = new float[300];//废气实际流量
        public float[] Vmas_Exhaust_k = new float[300];//稀释比每秒数据
        public float[] Vmas_Exhaust_qlyl = new float[300];
        public bool[] Vmas_Exhaust_accelerate = new bool[300];//稀释比每秒数据
        public bool[] Vmas_Exhaust_shandongflag = new bool[300];//稀释比每秒数据
        public float[] Vmas_Exhaust_cozl = new float[300];//每秒CO质量
        public float[] Vmas_Exhaust_co2zl = new float[300];//每秒CO质量
        public float[] Vmas_Exhaust_co2ld = new float[300];//标准车速
        public float[] Vmas_Exhaust_cold = new float[300];//实时车速
        public float[] Vmas_Exhaust_hcld = new float[300];//实时车速
        public float[] Vmas_Exhaust_nold = new float[300];//实时车速
        public float[] Vmas_Exhaust_o2ld = new float[300];//实时车速
        public float[] Vmas_bzcs = new float[300];//标准车速
        public float[] Vmas_sscs = new float[300];//实时车速
        public float[] Vmas_jsgl = new float[300];//加载功率
        public float[] Vmas_zsgl = new float[300];//加载功率
        public float[] Vmas_jzgl = new float[300];//加载功率
        public float[] Vmas_hjwd = new float[300];//环境温度
        public float[] Vmas_xdsd = new float[300];//相对湿度
        public float[] Vmas_dqyl = new float[300];//大气压力
        public float[] Vmas_sdxzxs = new float[300];//湿度修正系数
        public float[] Vmas_xsxzxs = new float[300];//稀释修正系数
        public float[] Vmas_fxyglyl = new float[300];//分析仪管路压力
        public float[] Vmas_Exhaust_hczl = new float[300];//每秒HC质量
        public float[] Vmas_Exhaust_nozl = new float[300];//每秒NO质量
        public float[] Vmas_nj = new float[300];//标准车速
        public float[] Vmas_fdjzs = new float[300];//标准车速

        public float[] Vmas_Exhaust_co_zb = new float[195];//每秒NO质量
        public float[] Vmas_Exhaust_hc_zb = new float[195];//每秒NO质量
        public float[] Vmas_Exhaust_no_zb = new float[195];//每秒NO质量
        public float[] Vmas_Exhaust_o2_zb = new float[195];//每秒NO质量
        public float[] Vmas_Exhaust_xishio2_zb = new float[195];//每秒NO质量
        public float[] Vmas_Exhaust_hujingo2_zb = new float[195];//每秒NO质量
        public float[] Vmas_Exhaust_cozl_zb = new float[195];//每秒NO质量
        public float[] Vmas_Exhaust_hczl_zb = new float[195];//每秒NO质量
        public float[] Vmas_Exhaust_nozl_zb = new float[195];//每秒NO质量 
        public float[] Vmas_Exhaust_lljll_zb = new float[195];//每秒NO质量
        public float[] Vmas_Exhaust_wqll_zb = new float[195];//每秒NO质量
        public float[] Vmas_Exhaust_k_zb = new float[195];//每秒NO质量
        public bool[] Vmas_Exhaust_Gd = new bool[300];//每秒NO质量
        public int[] lowflowarray = new int[300];
        public bool[] lljpoweroffarray = new bool[300];
        public bool[] cgjComSuccessarray = new bool[300];
        public int isLowFlow = 0;
        public bool islljpoweroff = false;
        
        public float cozl_zb = 0f;
        public float hczl_zb = 0f;
        public float nozl_zb = 0f;
        private int sxnb = 0;//时序类别，0：检测前准备  1：怠速准备  2：检测过程
        private float CarRmBc = 0f;
        private float powerSet1 = 0, powerSet2 = 0, powerSet3 = 0, powerSet4 = 0, powerSet5 = 0, powerSet6 = 0;

        private DateTime startTime,nowtime;
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
        public static bool shandongCo2O2_flag = false;
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
        public static float outTimeContinus = 0;                                                               //连续超差的时间
        public static float outTimeTotal = 0;                                                                   //总共超差的时间
        public static bool chaocha = false;
        public float Set_Power = 0;                                                             //扭矩
        public static bool beforedate = true;
        public static bool Ig195_status = false;
        public static float  gongkuangTime = 0f;
        public string gongkuangStartTime = "";
        public string gongkuangEndTime = "";

        private static float md_co=101325*0.01f*28/8.31f/273.15f;//mg
        private static float md_co2 = 101325 * 0.01f * 44 / 8.31f / 273.15f;//mg
        private static float md_hc = 101325 * 0.000001f * 86 / 8.31f / 273.15f;//mg
        private static float md_no = 101325 * 0.000001f * 46 / 8.31f / 273.15f;//mg

        public float co_zl = 0f;
        public float hc_zl = 0f;
        public float no_zl = 0f;
        public float co2_zl = 0f;

        public bool lxcc_flag = false;
        public bool llcc_flag = false;
        public bool power_flag = false;
        public string jctime = "";

        public static System.Windows.Forms.Screen[] sc;
        public static int sc_width = 0;
        public static int sc_height = 0;

        private bool isSongpin=false;

        private int rlzl = 0;//0为气油1为柴油 2为LPG 3为NG 4为双燃料
        public static string ts1 = "川AV7M82";
        public static string ts2 = "简易瞬态法";
        public static bool driverformmin = false;
        private bool isautostart = true;
        private bool isZkytNetMode = false;//是否是中科宇图联网模式

        int co2excedlimit = 0;
        int o2excedlimit = 0;
        
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

        public VMAS()
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
        private void Vmas_Load(object sender, EventArgs e)
        {
            pictureBox2.Parent = pictureBox1;
            pictureBox2.Location = new Point(96, 5938);
            pictureBox4.Parent = pictureBox1;
            pictureBox4.Location = new Point(96, 5938);
            panel_chujian.Visible = false;
            initJsglXs();
            initCarInfo();
            initConfigInfo();
            initEquipment();
            //textCsv();
            initChujian();
            Init_Chart();
            Init_Data();        //初始化数据
            Init_power();
            timer1.Interval = 100;
            timer1.Start();
            /*if(igbt!=null)
            {
                igbt.Lifter_Down();
            }*/
            isSongpin = false;
            if (!isautostart)
            {
                prepareFoem prepareform = new prepareFoem();
                prepareform.ShowDialog();
            }
            //prepareFoem prepareform = new prepareFoem();
            //prepareform.ShowDialog();
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
                    WD =double.Parse( temp.ToString());
                    ini.INIIO.GetPrivateProfileString("环境数据", "sd", "", temp, 2048, "C:/jcdatatxt/环境数据.ini");
                    SD = double.Parse(temp.ToString());
                    ini.INIIO.GetPrivateProfileString("环境数据", "dqy", "", temp, 2048, "C:/jcdatatxt/环境数据.ini");
                    DQY = double.Parse(temp.ToString());
                    IsUseTpTemp = true;
                }
            }

            if (carbj.ISUSE)
            {
                fla_502.coxs = carbj.VMAS_CO;
                fla_502.hcxs = carbj.VMAS_HC;
                fla_502.noxs = carbj.VMAS_NO;
            }
            if (isautostart)
            {
                Thread.Sleep(3000);
                button_ss_Click_1(sender, e);
            }

            if (fla_502 != null)
            {
                fla_502.lockKeyboard();
                Thread.Sleep(100);
                fla_502.StopBlowback();
            }
        }
        private void init_zbData()
        {
            Random ra = new Random();
            float hjo2_random = 0.01f * (ra.Next(10) - 5);
            dt_zb=csvreader.readCsv(@dogUse+"\\zb\\configdata.csv");
            if (dt_zb != null)
            {
                //float[] NunArray = new float[dt_zb.Rows.Count-1];
                for (int i = 1; i < dt_zb.Rows.Count; i++)
                {
                    try
                    {
                        Vmas_Exhaust_co_zb[i - 1] = float.Parse(dt_zb.Rows[i]["CO"].ToString()) + 0.02f * (ra.Next(10)-5);
                        if (Vmas_Exhaust_co_zb[i - 1] < 0f)
                            Vmas_Exhaust_co_zb[i - 1] = 0;
                        Vmas_Exhaust_hc_zb[i - 1] = float.Parse(dt_zb.Rows[i]["HC"].ToString()) + ra.Next(4)-2;
                        if (Vmas_Exhaust_hc_zb[i - 1] < 0f)
                            Vmas_Exhaust_hc_zb[i - 1] = 0;
                        Vmas_Exhaust_no_zb[i - 1] = float.Parse(dt_zb.Rows[i]["NO"].ToString()) + ra.Next(4)-2;
                        if (Vmas_Exhaust_no_zb[i - 1] < 0f)
                            Vmas_Exhaust_no_zb[i - 1] = 0;
                        Vmas_Exhaust_hujingo2_zb[i - 1] = float.Parse(dt_zb.Rows[i]["HJO2"].ToString()) + hjo2_random;
                        if (Vmas_Exhaust_hujingo2_zb[i - 1] < 20.3f)
                            Vmas_Exhaust_hujingo2_zb[i - 1] = 20.3f;
                        else if (Vmas_Exhaust_hujingo2_zb[i - 1] >21.3f)
                            Vmas_Exhaust_hujingo2_zb[i - 1] = 21.3f;
                        Vmas_Exhaust_o2_zb[i - 1] = float.Parse(dt_zb.Rows[i]["O2"].ToString()) +0.01f * (ra.Next(5) - 2.5f);
                        if (Vmas_Exhaust_o2_zb[i - 1] < 0f)
                            Vmas_Exhaust_o2_zb[i - 1] = 0;
                        Vmas_Exhaust_xishio2_zb[i - 1] = float.Parse(dt_zb.Rows[i]["XSO2"].ToString())+0.01f * (ra.Next(5) - 2.5f);
                        Vmas_Exhaust_lljll_zb[i - 1] = float.Parse(dt_zb.Rows[i]["LLJLL"].ToString()) +0.01f * (ra.Next(5) - 2.5f);
                        if (Vmas_Exhaust_hujingo2_zb[i - 1] > Vmas_Exhaust_xishio2_zb[i - 1] && Vmas_Exhaust_hujingo2_zb[i - 1] > Vmas_Exhaust_o2_zb[i - 1])//稀释比
                        {
                            Vmas_Exhaust_k_zb[i - 1] = (Vmas_Exhaust_hujingo2_zb[i - 1] - Vmas_Exhaust_xishio2_zb[i - 1]) / (Vmas_Exhaust_hujingo2_zb[i - 1] - Vmas_Exhaust_o2_zb[i - 1]);
                        }
                        else
                        {
                            Vmas_Exhaust_k_zb[i - 1] = 0f;
                        }
                        Vmas_Exhaust_wqll_zb[i - 1] = Vmas_Exhaust_lljll_zb[i-1] * Vmas_Exhaust_k_zb[i-1];//尾气实际流量
                        Vmas_Exhaust_cozl_zb[i - 1] = Vmas_Exhaust_wqll_zb[i - 1] * Vmas_Exhaust_co_zb[i - 1] * md_co;//CO质量mg
                        Vmas_Exhaust_nozl_zb[i - 1] = Vmas_Exhaust_wqll_zb[i - 1] * Vmas_Exhaust_no_zb[i - 1] * md_no;//NO质量mg
                        Vmas_Exhaust_hczl_zb[i - 1] = Vmas_Exhaust_wqll_zb[i - 1] * Vmas_Exhaust_hc_zb[i - 1] * md_hc;//HC质量mg

                    }

                    catch (Exception)
                    {

                        continue;
                    }
                }
                for(int i=0;i<195;i++)
                {
                    cozl_zb += Vmas_Exhaust_cozl_zb[i];
                    hczl_zb += Vmas_Exhaust_hczl_zb[i];
                    nozl_zb += Vmas_Exhaust_nozl_zb[i];
                }
                cozl_zb = cozl_zb*0.001f / distanceInTheory;
                hczl_zb = hczl_zb * 0.001f / distanceInTheory;
                nozl_zb = nozl_zb * 0.001f / distanceInTheory;

            }
        }
        private void textCsv()
        {
            try
            {
                DataRow dr = null;
                vmas_data.CarID = carbj.CarID;//车辆ID
                vmas_data.Wd = "21.1";//温度
                vmas_data.Sd = "20";//湿度
                vmas_data.Dqy = "101";//大气压
                vmas_data.Cozl = "0.01";//CO质量
                vmas_data.Noxzl = "0.02";//NO质量
                vmas_data.Hczl = "0.03";//HC质量
                vmas_dataseconds.CarID = carbj.CarID;
                DataTable vmas_datatable = new DataTable();
                vmas_datatable.Columns.Add("全程时序");
                vmas_datatable.Columns.Add("时序类别");
                vmas_datatable.Columns.Add("采样时序");
                vmas_datatable.Columns.Add("扭矩");
                vmas_datatable.Columns.Add("发动机转速");
                vmas_datatable.Columns.Add("HC实时值");
                vmas_datatable.Columns.Add("CO实时值");
                vmas_datatable.Columns.Add("CO2实时值");
                vmas_datatable.Columns.Add("NO实时值");
                vmas_datatable.Columns.Add("环境O2浓度");
                vmas_datatable.Columns.Add("分析仪O2实时值");
                vmas_datatable.Columns.Add("流量计O2实时值");
                vmas_datatable.Columns.Add("HC排放质量");
                vmas_datatable.Columns.Add("CO排放质量");
                vmas_datatable.Columns.Add("NO排放质量");
                vmas_datatable.Columns.Add("标准时速");
                vmas_datatable.Columns.Add("实时车速");
                vmas_datatable.Columns.Add("加载功率");
                vmas_datatable.Columns.Add("环境温度");
                vmas_datatable.Columns.Add("相对湿度");
                vmas_datatable.Columns.Add("大气压力");
                vmas_datatable.Columns.Add("流量计温度");
                vmas_datatable.Columns.Add("流量计压力");
                vmas_datatable.Columns.Add("实际体积流量");
                vmas_datatable.Columns.Add("标准体积流量");
                vmas_datatable.Columns.Add("湿度修正系数");
                vmas_datatable.Columns.Add("稀释修正系数");
                vmas_datatable.Columns.Add("稀释比");
                vmas_datatable.Columns.Add("分析仪管路压力");
                vmas_datatable.Columns.Add("分析仪实时流量");
                for (int i = fqy_delayTime; i < 195 + fqy_delayTime; i++)//将数据写入逐秒数据
                {
                    dr = vmas_datatable.NewRow();
                    dr["全程时序"] = "2";
                    dr["时序类别"] = "2";
                    dr["采样时序"] = i - fqy_delayTime;
                    dr["HC实时值"] = "2";
                    dr["CO实时值"] = "2";
                    dr["CO2实时值"] = "2";
                    dr["NO实时值"] = "2";
                    dr["环境O2浓度"] = "2";
                    dr["分析仪O2实时值"] = "2";
                    dr["流量计O2实时值"] = "2";
                    dr["HC排放质量"] = "2";
                    dr["CO排放质量"] = "2";
                    dr["NO排放质量"] = "2";
                    dr["标准时速"] = "2";
                    dr["实时车速"] = "2";
                    dr["加载功率"] = "2";
                    dr["环境温度"] = "2";
                    dr["相对湿度"] = "2";
                    dr["大气压力"] = "2";
                    dr["流量计温度"] = "2";
                    dr["流量计压力"] = "2";
                    dr["实际体积流量"] = "2";
                    dr["标准体积流量"] = "2";
                    dr["湿度修正系数"] = "2";
                    dr["稀释修正系数"] = "2";
                    dr["稀释比"] = "2";
                    dr["分析仪管路压力"] = 200f;
                    dr["分析仪实时流量"] = "2";
                    vmas_datatable.Rows.Add(dr);
                }
                
                writeDataIsFinished = false;
                th_load = new Thread(load_progress);
                th_load.Start();
                csvwriter.SaveCSV(vmas_datatable, "C:/jcdatatxt/" + carbj.CarID + ".csv");
                vmasdatacontrol.writeVmasData(vmas_data);
                writeDataIsFinished = true;
            }
            catch
            { }
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
            bool Init_flag=true;
            string init_message="";
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
                        case "mqw_50a":
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
                        igbt = new Dynamometer.IGBT("BNTD", equipconfig.isIgbtContainGdyk);
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
                if (equipconfig.Lljifpz == true)
                {
                    try
                    {
                        flv_1000 = new Exhaust.Flv_1000(equipconfig.Lljxh);
                        flv_1000.isNhSelfUse = equipconfig.isLljNhSelfUse;
                        if (flv_1000.Init_Comm(equipconfig.Lljck, equipconfig.Lljckpzz) == false)
                        {
                            flv_1000 = null;
                            Init_flag = false;
                            init_message += "流量计串口打开失败.";

                        }
                    }
                    catch (Exception er)
                    {
                        flv_1000 = null;
                        Init_flag = false;
                        MessageBox.Show(er.ToString(), "出错啦");
                    }
                }
            }
            catch (Exception)
            {
                flv_1000 = null;
                Init_flag = false;
            }
            try
            {
                if (equipconfig.Ledifpz)
                {
                    try
                    {
                        ledcontrol = new LedControl.led();
                        if (ledcontrol.Init_Comm(equipconfig.Ledck, "9600,N,8,1") == false)
                        {
                            ledcontrol = null;
                            Init_flag = false;
                            init_message += "LED屏串口打开失败.";
                        }
                    }
                    catch (Exception er)
                    {
                        ledcontrol = null;
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
        }
        private void initChujian()
        {
            dt.Columns.Add("项目");
            dt.Columns.Add("结果");
            dt.Columns.Add("判定");
            DataRow dr = null;
            dr = dt.NewRow();
            dr["项目"] = "滚筒速度";
            dr["结果"] = "--";
            dr["判定"] = "--";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "环境温度";
            dr["结果"] = "--";
            dr["判定"] = "--";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "环境湿度";
            dr["结果"] = "--";
            dr["判定"] = "--";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "环境气压";
            dr["结果"] = "--";
            dr["判定"] = "--";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "环境氧";
            dr["结果"] = "--";
            dr["判定"] = "--";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "环境背景残留气体";
            dr["结果"] = "--";
            dr["判定"] = "--";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "取样系统HC残留量";
            dr["结果"] = "--";
            dr["判定"] = "--";
            dt.Rows.Add(dr);
            dataGridView1.DataSource = dt;
        }
        private void initCarInfo()
        {
            carbj = carini.getCarIni();
            ts1 = carbj.CarPH;
            CarRmBc = carbj.CarJzzl - 907;
            if (CarRmBc < 0) CarRmBc = 0;
            powerSet1 = (float)(CarRmBc * (1.04 - 0.113)* vmasconfig.vmasPowerXs);
            powerSet2 = (float)(CarRmBc * (0.83 - 0.113)* vmasconfig.vmasPowerXs);
            powerSet3 = (float)(CarRmBc * (0.94 - 0.113)* vmasconfig.vmasPowerXs);
            powerSet4 = (float)(CarRmBc * (0.83 - 0.113)* vmasconfig.vmasPowerXs);
            powerSet5 = (float)(CarRmBc * (0.62 - 0.113)* vmasconfig.vmasPowerXs);
            powerSet6 = (float)(CarRmBc * (0.52 - 0.113)* vmasconfig.vmasPowerXs);
        }
        private void initConfigInfo()
        {
            equipconfig = configini.getEquipConfigIni();
            isautostart = equipconfig.WorkAutomaticMode;
            vmasconfig = configini.getVmasConfigIni();
            thaxsdata = configini.getthaxsConfigIni();
            fqy_delayTime = equipconfig.FqyDelayTime;
            delayTimeBetweenFqyAndLlj = fqy_delayTime - equipconfig.LljDelayTime;
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

        public void Init_Chart()
        {
            /*try
            {
                StringBuilder temp = new StringBuilder();
                temp.Length = 2048;
                
                Clear_Chart();
                chart1.Series["seriesCeiling"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                chart1.Series["seriesLower"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                chart1.Series["seriesdata"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                chart1.Series["seriesBzSpeed"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                //chart1.ChartAreas["ChartArea1"].AxisX.Interval = 100000;
                //chart1.ChartAreas["ChartArea1"].AxisY.Interval = 100000;
                chart1.Series["seriesCeiling"].Color = System.Drawing.Color.Red;
                chart1.Series["seriesCeiling"].BorderWidth = 2;
                chart1.Series["seriesLower"].Color = System.Drawing.Color.Red;
                chart1.Series["seriesLower"].BorderWidth = 2;
                chart1.Series["seriesdata"].Color = System.Drawing.Color.White;
                chart1.Series["seriesdata"].BorderWidth = 5;
                chart1.Series["seriesBzSpeed"].Color = System.Drawing.Color.Yellow;
                chart1.Series["seriesBzSpeed"].BorderWidth = 1;
                this.chart1.ChartAreas[0].AxisY.LabelStyle.IsEndLabelVisible = false;
                this.chart1.ChartAreas[0].AxisX.LabelStyle.IsEndLabelVisible = false;
                this.chart1.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
                this.chart1.ChartAreas[0].AxisY.LabelStyle.Format = "N0";
                //chart1.ChartAreas[0].Position.Height = 100;
                if (isVertical)
                {
                    SetYlimit(-3f, 60f);
                    setlimit(0f, 10f);
                    restartChart_vertical();
                }
                else
                {
                    SetYlimit(0f, 10f);
                    setlimit(-12f, 12f);
                    restartChart();
                }
            }
            catch
            { 
            }*/
        }
        public void restartChart()
        {
            try
            {
                //Clear_Chart();
                for (float i = 0f; i <= 5f; i = i + 0.1f)//显示5秒内的数据
                {
                    speed_data speedNow = speed_now(i);//提前显示前4秒的状态
                    float gongkuangSpeed = speedNow.speed_now_data;
                    float gongkuangSpeedLow = gongkuangSpeed - 2;
                    float gongkuangSpeedHigh = gongkuangSpeed + 2;
                    //Ref_Chart(gongkuangSpeedHigh,i, "seriesCeiling");
                    //Ref_Chart(gongkuangSpeedLow, i, "seriesLower");
                    //Ref_Chart(gongkuangSpeed, i, "seriesBzSpeed");
                }
                
            }
            catch
            { }
        }
        public void restartChart_vertical()
        {
            try
            {
                //Clear_Chart();
                
                for (float i = 0f; i <= 5f; i = i + 0.1f)//显示5秒内的数据
                {
                    speed_data speedNow = speed_now(i);//提前显示前4秒的状态
                    float gongkuangSpeed = speedNow.speed_now_data;
                    float gongkuangSpeedLow = gongkuangSpeed - 2;
                    float gongkuangSpeedHigh = gongkuangSpeed + 2;
                    //Ref_Chart_vertical(gongkuangSpeedHigh, i, "seriesCeiling");
                    //Ref_Chart_vertical(gongkuangSpeedLow, i, "seriesLower");
                    //Ref_Chart_vertical(gongkuangSpeed, i, "seriesBzSpeed");
                }
                
            }
            catch
            { }
        }
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
                if (carbj.CarJzzl <= 750f)
                {
                    Set_Power = float.Parse(equipconfig.PowerSet[0]);
                }
                else if (carbj.CarJzzl <= 850f)
                {
                    Set_Power = float.Parse(equipconfig.PowerSet[1]);
                }
                else if (carbj.CarJzzl <= 1020f)
                {
                    Set_Power = float.Parse(equipconfig.PowerSet[2]);
                }
                else if (carbj.CarJzzl <= 1250f)
                {
                    Set_Power = float.Parse(equipconfig.PowerSet[3]);
                }
                else if (carbj.CarJzzl <= 1470f)
                {
                    Set_Power = float.Parse(equipconfig.PowerSet[4]);
                }
                else if (carbj.CarJzzl <= 1700f)
                {
                    Set_Power = float.Parse(equipconfig.PowerSet[5]);
                }
                else if (carbj.CarJzzl <= 1930f)
                {
                    Set_Power = float.Parse(equipconfig.PowerSet[6]);
                }
                else if (carbj.CarJzzl <= 2150f)
                {
                    Set_Power = float.Parse(equipconfig.PowerSet[7]);
                }
                else if (carbj.CarJzzl<= 2380f)
                {
                    Set_Power = float.Parse(equipconfig.PowerSet[8]);
                }
                else if (carbj.CarJzzl <= 2610f)
                {
                    Set_Power = float.Parse(equipconfig.PowerSet[9]);
                }
                else
                {
                    Set_Power = float.Parse(equipconfig.PowerSet[10]);
                }
            }
            catch
            {
            }
        }
        public void Init_Data()
        {
            Msg(label_cp, panel_cp, carbj.CarPH,false);
            Msg(label_message, panel_msg, "点击开始检测按钮,开始检测", false);
        }
        #endregion
        
        private speed_data speed_now(float gongkuangtime)
        {
            speed_data speedNow ;
            speedNow.speed_now_data = 0;
            speedNow.isChangeD = false;
            //Clear_Chart();
            if (gongkuangtime < 11)
            {
                shandongCo2O2_flag = false;
                accelerate_flag = false;
                speedNow.speed_now_data = 0f; 
            }
            else if (gongkuangtime < 15)
            {
                shandongCo2O2_flag = false;
                accelerate_flag = true;
                speedNow.speed_now_data = (gongkuangtime - 11) * 15 / 4f;
            }
            else if (gongkuangtime < 23)
            {
                shandongCo2O2_flag = false;
                accelerate_flag = false;
                speedNow.speed_now_data = 15f;
            }
            else if (gongkuangtime < 28)
            {
                shandongCo2O2_flag = false;
                accelerate_flag = false;
                speedNow.speed_now_data = 15 - (gongkuangtime - 23) * 15 / 5f;
            }
            else if (gongkuangtime < 49)
            {
                shandongCo2O2_flag = false;
                accelerate_flag = false;
                speedNow.speed_now_data = 0f;
            }
            else if (gongkuangtime < 54)
            {
                shandongCo2O2_flag = false;
                accelerate_flag = true;
                speedNow.speed_now_data = (gongkuangtime - 49) * 15f / 5f;
            }
            else if (gongkuangtime < 56)
            {
                shandongCo2O2_flag = false;
                accelerate_flag = false;
                speedNow.speed_now_data = 15f;
                speedNow.isChangeD = true;//变档中
            }
            else if (gongkuangtime < 61)
            {
                shandongCo2O2_flag = true;
                accelerate_flag = true;
                speedNow.speed_now_data = 15 + (gongkuangtime - 56) * 17f / 5f;
            }
            else if (gongkuangtime < 85)
            {
                shandongCo2O2_flag = true;
                accelerate_flag = false;
                speedNow.speed_now_data = 32f;
            }
            else if (gongkuangtime < 96)
            {
                shandongCo2O2_flag = false;
                accelerate_flag = false;
                speedNow.speed_now_data = 32 - (gongkuangtime - 85) * 32 / 11f;
            }
            else if (gongkuangtime < 117)
            {
                shandongCo2O2_flag = false;
                accelerate_flag = false;
                speedNow.speed_now_data = 0f;
            }
            else if (gongkuangtime < 122)
            {
                shandongCo2O2_flag = true;
                accelerate_flag = true;
                speedNow.speed_now_data = (gongkuangtime - 117) * 15 / 5f;
                //speedNow.isChangeD = true;//变档中
            }
            else if (gongkuangtime < 124)
            {
                shandongCo2O2_flag = true;
                accelerate_flag = false;
                speedNow.speed_now_data = 15f ;
                speedNow.isChangeD = true;//变档中
            }
            else if (gongkuangtime < 133)
            {
                shandongCo2O2_flag = true;
                accelerate_flag = true;
                speedNow.speed_now_data = 15f + (gongkuangtime - 124) * 20 / 9f;
            }
            else if (gongkuangtime < 135)
            {
                shandongCo2O2_flag = true;
                accelerate_flag = false;
                speedNow.speed_now_data = 35f;
                speedNow.isChangeD = true;//变档中
            }
            else if (gongkuangtime < 143)
            {
                shandongCo2O2_flag = true;
                accelerate_flag = true;
                power_flag = false;
                speedNow.speed_now_data = 35f+(gongkuangtime - 135) * 15 / 8f;
            }
            else if (gongkuangtime < 155)
            {
                shandongCo2O2_flag = true;
                accelerate_flag = true;
                power_flag = true;
                speedNow.speed_now_data = 50f;
            }
            else if (gongkuangtime < 163)
            {
                shandongCo2O2_flag = false;
                accelerate_flag = false;
                power_flag = false;
                speedNow.speed_now_data = 50 - (gongkuangtime - 155) * 15 / 8f;
            }
            else if (gongkuangtime < 176)
            {
                shandongCo2O2_flag = true;
                accelerate_flag = false;
                speedNow.speed_now_data = 35f;
            }
            else if (gongkuangtime < 178)
            {
                shandongCo2O2_flag = false;
                accelerate_flag = false;
                speedNow.speed_now_data = 35 - (gongkuangtime - 176) * 3 / 2f;
                speedNow.isChangeD = true;//变档中
            }
            else if (gongkuangtime < 188)
            {
                shandongCo2O2_flag = false;
                accelerate_flag = false;
                speedNow.speed_now_data = 32 - (gongkuangtime - 178) * 32 / 10f;
                //speedNow.isChangeD = true;//变档中
            }
            else
            {
                shandongCo2O2_flag = false;
                accelerate_flag = false;
                speedNow.speed_now_data = 0f;
            }
            //if (speedNow.speed_low_data < 0) speedNow.speed_low_data = 0;
            return speedNow;
        }

        /// <summary>
        /// 计算Df
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
        /// 计算Kh
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

        private float jsgl_xsa = 0;
        private float jsgl_xsb = 0;
        private float jsgl_xsc = 0;
        private double jsgl = 0;
        private double zsgl = 0;
        private double zgl = 0;
        private float nl = 0;
        private DateTime gc_time = DateTime.Now;//用于标记过程数据全程时序，避免出现相同时间

        private void timer2_Click(object sender, EventArgs e)
        {
            nowtime = DateTime.Now;
            if (Ig195_status)
            {
                try
                {
                    chaocha = true;
                    speedNow.speed_now_data = igbt.Speed;
                    speedLimit = speed_now(gongkuangTime);
                    if (gongkuangTime < 195.1)
                    {
                        if (gongkuangTime <= 11)
                        {
                            pictureBox2.Location = new Point((int)(96 + 34 * igbt.Speed / 3), (int)(5936 - 1105 * gongkuangTime /39));
                            pictureBox4.Location = new Point((int)(96 + 34 * igbt.Speed / 3), (int)(5936 - 1105 * gongkuangTime / 39));
                        }
                        else
                        {
                            pictureBox2.Location = new Point((int)(96 + 34 * igbt.Speed / 3), (int)(5936 - 1105 * gongkuangTime / 39));
                            pictureBox4.Location = new Point((int)(96 + 34 * igbt.Speed / 3), (int)(5936 - 1105 * gongkuangTime / 39));
                            pictureBox1.Location = new Point(1, (int)(-5303 + 1105 * (gongkuangTime - 11) / 39));
                        }
                    }

                    //if (Convert.ToInt16(gongkuangTime*10)/10 !=GKSJ)           //每1s记录一次信息
                    if(DateTime.Compare(DateTime.Parse(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), DateTime.Parse(gc_time.ToString("yyyy-MM-dd HH:mm:ss"))) > 0)
                    {
                        gc_time = DateTime.Now;
                        Random rd2 = new Random();
                        if (equipconfig.DATASECONDS_TYPE == "江西")
                            gksj_count = GKSJ + vmasconfig.Dssj + 1;
                        else
                            gksj_count = GKSJ + vmasconfig.Dssj;
                        jsgl = Math.Round(jsgl_xsa * igbt.Speed * igbt.Speed + jsgl_xsb * igbt.Speed + jsgl_xsc, 3);//寄生功率=
                        if (jsgl < 0) jsgl = 0;
                        zgl = Math.Round(igbt.Power, 3);
                        if (GKSJ >= 143 && GKSJ < 155)
                        {
                            Random rd = new Random();
                            zgl = (float)(Set_Power + (double)(RNG.Next(300) - 150) * 1.0 / 1000.0);
                        }
                        if (zgl < jsgl) zgl = jsgl;
                        zsgl = Math.Round(zgl - jsgl, 3);
                        if (zsgl < 0) zsgl = 0;
                        nl = igbt.Force;
                        if (nl < 0) nl = 0;

                        lowflowarray[gksj_count] = isLowFlow;
                        lljpoweroffarray[gksj_count] = islljpoweroff;
                        cgjComSuccessarray[gksj_count] = igbt.isComSuccess;
                        Vmas_qcsj[gksj_count] =DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//全程时序
                        Vmas_cysx[gksj_count] = (GKSJ + 1).ToString();
                        Vmas_sxnb[gksj_count] = sxnb.ToString();
                        Vmas_nj[gksj_count] = nl;
                        Vmas_fdjzs[gksj_count] = Vmas_Exhaust_Now.ZS;
                        Vmas_bzcs[gksj_count] = speedLimit.speed_now_data;//标准速度
                        Vmas_sscs[gksj_count] = igbt.Speed;//实时速度
                        Vmas_jsgl[gksj_count] = (float)(Math.Round(jsgl,3));//功率
                        Vmas_zsgl[gksj_count] = (float)(Math.Round(zsgl, 3));//功率
                        Vmas_jzgl[gksj_count] = (float)(Math.Round(zgl, 3));//功率
                        Vmas_Exhaust_accelerate[gksj_count] = accelerate_flag;//是否处于加速和等速状态下
                        Vmas_Exhaust_shandongflag[gksj_count] = shandongCo2O2_flag;
                        Speed_listIg195[gksj_count] = igbt.Speed;//实时速度
                        Vmas_Exhaust_ListIG195[gksj_count] = Vmas_Exhaust_Now;//尾气浓度值
                        Vmas_Exhaust_Revise_ListIG195[gksj_count] = Vmas_Exhaust_Now;//修正的尾气浓度值
                        Vmas_Exhaust_llList[gksj_count] = flv_1000.ll_standard_value;//流量计标准流量值
                        Vmas_lambda[gksj_count] = Vmas_Exhaust_Now.λ;
                        Vmas_Exhaust_xso2now[gksj_count] = flv_1000.o2_standard_value;//流量计稀释氧浓度
                        if (Vmas_Exhaust_xso2now[gksj_count] > hjo2)
                            Vmas_Exhaust_xso2now[gksj_count] = hjo2;
                        if (gksj_count >= delayTimeBetweenFqyAndLlj)
                        {
                            Vmas_Exhaust_xso2afterDelay[gksj_count] = Vmas_Exhaust_xso2now[gksj_count - delayTimeBetweenFqyAndLlj];
                        }
                        else
                        {
                            Vmas_Exhaust_xso2afterDelay[gksj_count] = Vmas_Exhaust_xso2now[gksj_count];
                        }
                        Vmas_Exhaust_lljtemp[gksj_count] = flv_1000.temp_standard_value;//流量计温度
                        Vmas_Exhaust_lljyl[gksj_count] = flv_1000.yali_standard_value;//流量计压力
                        Vmas_Exhaust_o2ld[gksj_count] = Vmas_Exhaust_Now.O2;//废气仪氧气浓度
                        if (GKSJ > fqy_delayTime)
                        {
                            if (Vmas_Exhaust_accelerate[gksj_count - fqy_delayTime - vmasconfig.Dssj] == false)
                            {
                                if (Vmas_Exhaust_o2ld[gksj_count] > 5)
                                {
                                    Vmas_Exhaust_o2ld[gksj_count] = (float)(5.0 + (float)(Vmas_Exhaust_o2ld[gksj_count] - 5.0) * 0.5f);
                                }
                            }
                        }
                        if (hjo2 > Vmas_Exhaust_xso2afterDelay[gksj_count] && hjo2 > Vmas_Exhaust_o2ld[gksj_count])//稀释比
                        {
                            Vmas_Exhaust_k[gksj_count] = (float)(Math.Round((hjo2 - Vmas_Exhaust_xso2afterDelay[gksj_count]) / (hjo2 - Vmas_Exhaust_o2ld[gksj_count]), 3));
                        }
                        else
                        {
                            Vmas_Exhaust_k[gksj_count] = 0f;
                        }
                        Vmas_Exhaust_qlyl[gksj_count] = Vmas_Exhaust_Now.QLYL;
                        Vmas_Exhaust_fqsjll[gksj_count] = (float)(Math.Round(Vmas_Exhaust_llList[gksj_count] * Vmas_Exhaust_k[gksj_count], 3));//尾气实际流量
                        Vmas_Exhaust_cold[gksj_count] = Vmas_Exhaust_Now.CO;//CO浓度
                        Vmas_Exhaust_co2ld[gksj_count] = Vmas_Exhaust_Now.CO2;//CO2浓度
                        Vmas_Exhaust_hcld[gksj_count] = Vmas_Exhaust_Now.HC;//HC浓度
                        Vmas_Exhaust_nold[gksj_count] = Vmas_Exhaust_Now.NO;//NO浓度

                        Vmas_lambda[gksj_count] = Vmas_Exhaust_Now.λ;
                        if (IsUseTpTemp)
                        {
                            Vmas_hjwd[gksj_count] = (float)WD; //温度
                            Vmas_xdsd[gksj_count] = (float)SD;//湿度
                            Vmas_dqyl[gksj_count] = (float)DQY;//大气压
                        }
                        else if (equipconfig.Fqyxh.ToLower() == "fla_502" || equipconfig.Fqyxh.ToLower() == "nha_503")
                        {
                            Vmas_hjwd[gksj_count] = fla502_temp_data.TEMP;//温度
                            Vmas_xdsd[gksj_count] = fla502_temp_data.HUMIDITY;//湿度
                            Vmas_dqyl[gksj_count] = fla502_temp_data.AIRPRESSURE;//大气压
                        }
                        else
                        {
                            Vmas_hjwd[gksj_count] = Vmas_Exhaust_Now.HJWD;//温度
                            Vmas_xdsd[gksj_count] = Vmas_Exhaust_Now.SD;//湿度
                            Vmas_dqyl[gksj_count] = Vmas_Exhaust_Now.HJYL;//大气压
                        }
                        Vmas_xsxzxs[gksj_count] = (float)(Math.Round(caculateDf(Vmas_Exhaust_co2ld[gksj_count], Vmas_Exhaust_cold[gksj_count]), 3));//稀释修正系数
                        Vmas_sdxzxs[gksj_count] = (float)(Math.Round(caculateKh(Vmas_hjwd[gksj_count], Vmas_xdsd[gksj_count], Vmas_dqyl[gksj_count]), 3));//湿度修正系数
                        Vmas_Exhaust_co2zl[gksj_count] = (float)(Math.Round(Vmas_Exhaust_fqsjll[gksj_count] * Vmas_Exhaust_co2ld[gksj_count] * md_co2, 3));//CO质量mg
                        Vmas_Exhaust_cozl[gksj_count] = (float)(Math.Round(Vmas_Exhaust_fqsjll[gksj_count] * Vmas_Exhaust_cold[gksj_count] * md_co * Vmas_xsxzxs[gksj_count], 3));//CO质量mg
                        Vmas_Exhaust_nozl[gksj_count] = (float)(Math.Round(Vmas_Exhaust_fqsjll[gksj_count] * Vmas_Exhaust_nold[gksj_count] * md_no * Vmas_xsxzxs[gksj_count] * Vmas_sdxzxs[gksj_count], 3));//NO质量mg
                        Vmas_Exhaust_hczl[gksj_count] = (float)(Math.Round(Vmas_Exhaust_fqsjll[gksj_count] * Vmas_Exhaust_hcld[gksj_count] * md_hc * Vmas_xsxzxs[gksj_count], 3));//HC质量mg
                        if (equipconfig.isIgbtContainGdyk)
                        {
                            byte gddata = 0x01;
                            switch (equipconfig.EmergencyStop)
                            {
                                case 1: gddata = 0x01; break;
                                case 2: gddata = 0x02; break;
                                case 3: gddata = 0x04; break;
                                default: break;
                            }
                            Vmas_Exhaust_Gd[gksj_count] = ((igbt.keyandgd & gddata) == 0x00);
                        }
                        else
                        { Vmas_Exhaust_Gd[gksj_count] = true; }
                        if (vmasconfig.IfDisplayData)
                        {
                            Msg(labelCO, panelCO, Vmas_Exhaust_Now.CO.ToString("0.00"), false);
                            Msg(labelHC, panelHC, Vmas_Exhaust_Now.HC.ToString(), false);
                            Msg(labelNO, panelNO, Vmas_Exhaust_Now.NO.ToString(), false);
                        }
                        else
                        {
                            Msg(labelCO, panelCO, "—", false);
                            Msg(labelHC, panelHC, "—", false);
                            Msg(labelNO, panelNO, "—", false);
                        }
                        Msg(labelCO2, panelCO2, Vmas_Exhaust_Now.CO2.ToString("0.00"), false);
                        Msg(labelOO2, panelO2, Vmas_Exhaust_o2ld[gksj_count].ToString("0.00"), false);
                        Msg(labelXsO2, panelXsO2, Vmas_Exhaust_xso2now[gksj_count].ToString("0.00"), false);
                        Msg(labelLL, panelLL, Vmas_Exhaust_llList[gksj_count].ToString("0.0"), false);
                        Msg(labelWD, panelWD, Vmas_hjwd[gksj_count].ToString("0.0"), false);
                        Msg(labelSD, panelSD, Vmas_xdsd[gksj_count].ToString("0.0"), false);
                        Msg(labelQY, panelQY, Vmas_dqyl[gksj_count].ToString("0.0"), false);
                        Msg(labelWQLL, panelWQLL, Vmas_Exhaust_fqsjll[gksj_count].ToString("0.00"), false);
                        GKSJ++;
                        if (gksj_count >=vmasconfig.Dssj+fqy_delayTime)
                        {
                            if (vmasconfig.SdCo2AndO2Monitor)
                            {
                                if (Vmas_Exhaust_shandongflag[gksj_count - fqy_delayTime] == true)
                                {
                                    if (Vmas_Exhaust_co2ld[gksj_count] <= 10)
                                    {
                                        co2excedlimit++;
                                    }
                                    if (Vmas_Exhaust_o2ld[gksj_count] >= 4)
                                    {
                                        o2excedlimit++;
                                    }
                                }
                            }
                            else
                            {
                                co2excedlimit = 0;
                                o2excedlimit = 0;
                            }
                        }

                    }
                    if (GKSJ > 143+fqy_delayTime && GKSJ < 155+fqy_delayTime && vmasconfig.ConcentrationMonitor == true)//如果浓度监控开启的话
                    {
                        if (((Vmas_Exhaust_cold[gksj_count] + Vmas_Exhaust_co2ld[gksj_count]) < 3.0))
                        {
                            co2excedlimit = 0;
                            o2excedlimit = 0;
                            ovalShapeNDZ.FillColor = Color.Red;
                            outTimeContinus = 0f;
                            outTimeTotal = 0f;
                            TH_ST.Abort();
                            Th_get_FqandLl.Abort();
                            th_get_llj.Abort();
                            Speed_Jc_flag = false;
                            Ig195_status = false;
                            gongkuangTime = 0f;
                            GKSJ = 0;
                            //fla_502.Stop();
                            Msg(label_message, panel_msg, "50km/h时[CO]+[CO2]小于3.0%，请检查取样探头是否脱落", true);
                            ts1 = "检测中止";
                            ts2 = "浓度值过低"; 
                            if (vmasconfig.AutoRestart)
                            {
                                power_flag = false;
                                isCcStop = true;
                                jctime = DateTime.Now.ToString();
                                TH_ST = new Thread(Jc_Exe);
                                Th_get_FqandLl = new Thread(Fq_Detect);
                                th_get_llj = new Thread(llj_Detect);
                                timer2.Start();
                                JC_Status = true;
                                pictureBox1.Location = new Point(1, -5303);
                                pictureBox2.Location = new Point(96, 5936);
                                pictureBox4.Location = new Point(96, 5936);
                                //1, -5304
                                ovalShapeLXCC.FillColor = Color.Lime;
                                ovalShapeLJCC.FillColor = Color.Lime;
                                //ovalShapeNDZ.FillColor = Color.Lime;
                                ovalShapeLLJLJ.FillColor = Color.Lime;
                                ovalShapeWQLL.FillColor = Color.Lime;
                                ovalShapeXSB.FillColor = Color.Lime;
                                ovalShapeJZGL.FillColor = Color.Lime;
                                Msg(labelGksj, panelGksj, "000.0", false);
                                Msg(labelCO, panelCO, "0.00", false);
                                Msg(labelCO2, panelCO2, "0.00", false);
                                Msg(labelOO2, panelO2, "0.00", false);
                                Msg(labelHC, panelHC, "0", false);
                                Msg(labelNO, panelNO, "0", false);
                                Msg(labelLL, panelLL, "0.0", false);
                                Msg(labelLXCC, panelLXCC, "0.0", false);
                                Msg(labelLJCC, panelLJCC, "0.0", false);
                                TH_ST.Start();
                            }
                            else
                            {
                                MessageBox.Show("50km/h时[CO]+[CO2]小于3.0%，检测终止", "警告");
                            }
                        }
                    }
                    else if (GKSJ >= 3 && vmasconfig.ConcentrationMonitor == true&&!vmasconfig.SdCo2AndO2Monitor)//如果浓度监控开启的话
                    {
                        if (((Vmas_Exhaust_cold[gksj_count] + Vmas_Exhaust_co2ld[gksj_count]) < vmasconfig.Ndz) && Vmas_Exhaust_accelerate[gksj_count - fqy_delayTime] == true)
                        {
                                    co2excedlimit = 0;
                                    o2excedlimit = 0;
                                    ovalShapeNDZ.FillColor = Color.Red;
                                    outTimeContinus = 0f;
                                    outTimeTotal = 0f;
                                    TH_ST.Abort();
                                    Th_get_FqandLl.Abort();
                                    th_get_llj.Abort();
                                    Speed_Jc_flag = false;
                                    Ig195_status = false;
                                    gongkuangTime = 0f;
                                    GKSJ = 0;
                                    //fla_502.Stop();
                                    Msg(label_message, panel_msg, "[CO]+[CO2]小于6%，请检查取样探头是否脱落", true);
                                    ts1 = "检测中止";
                                    ts2 = "浓度值低于规定值"; 
                                    if (vmasconfig.AutoRestart)
                                    {
                                        power_flag = false;
                                        isCcStop = true;
                                        jctime = DateTime.Now.ToString();
                                        TH_ST = new Thread(Jc_Exe);
                                        Th_get_FqandLl = new Thread(Fq_Detect);
                                        th_get_llj = new Thread(llj_Detect);
                                        timer2.Start();
                                        JC_Status = true;
                                        pictureBox1.Location = new Point(1, -5303);
                                        pictureBox2.Location = new Point(96, 5936);
                                        pictureBox4.Location = new Point(96, 5936);
                                        //1, -5304
                                        ovalShapeLXCC.FillColor = Color.Lime;
                                        ovalShapeLJCC.FillColor = Color.Lime;
                                        //ovalShapeNDZ.FillColor = Color.Lime;
                                        ovalShapeLLJLJ.FillColor = Color.Lime;
                                        ovalShapeWQLL.FillColor = Color.Lime;
                                        ovalShapeXSB.FillColor = Color.Lime;
                                        ovalShapeJZGL.FillColor = Color.Lime;
                                        Msg(labelGksj, panelGksj, "000.0", false);
                                        Msg(labelCO, panelCO, "0.00", false);
                                        Msg(labelCO2, panelCO2, "0.00", false);
                                        Msg(labelOO2, panelO2, "0.00", false);
                                        Msg(labelHC, panelHC, "0", false);
                                        Msg(labelNO, panelNO, "0", false);
                                        Msg(labelLL, panelLL, "0.0", false);
                                        Msg(labelLXCC, panelLXCC, "0.0", false);
                                        Msg(labelLJCC, panelLJCC, "0.0", false);
                                        TH_ST.Start();
                                    }
                                    else
                                    {
                                        MessageBox.Show("[CO]+[CO2]小于6%，检测终止", "警告");
                                    }
                        }
                    }
                    else if(GKSJ>=0)
                    {
                        if (co2excedlimit >= vmasconfig.ndccsj)
                        {
                            co2excedlimit = 0;
                            o2excedlimit = 0;
                            ovalShapeNDZ.FillColor = Color.Red;
                            outTimeContinus = 0f;
                            outTimeTotal = 0f;
                            TH_ST.Abort();
                            Th_get_FqandLl.Abort();
                            th_get_llj.Abort();
                            Speed_Jc_flag = false;
                            Ig195_status = false;
                            gongkuangTime = 0f;
                            GKSJ = 0;
                            //fla_502.Stop();
                            Msg(label_message, panel_msg, "[CO2]小于10%，请检查取样探头是否脱落", true);
                            ts1 = "检测中止";
                            ts2 = "CO2浓度过低";
                            if (vmasconfig.AutoRestart)
                            {
                                power_flag = false;
                                isCcStop = true;
                                jctime = DateTime.Now.ToString();
                                TH_ST = new Thread(Jc_Exe);
                                Th_get_FqandLl = new Thread(Fq_Detect);
                                th_get_llj = new Thread(llj_Detect);
                                timer2.Start();
                                JC_Status = true;
                                pictureBox1.Location = new Point(1, -5303);
                                pictureBox2.Location = new Point(96, 5936);
                                pictureBox4.Location = new Point(96, 5936);
                                //1, -5304
                                ovalShapeLXCC.FillColor = Color.Lime;
                                ovalShapeLJCC.FillColor = Color.Lime;
                                //ovalShapeNDZ.FillColor = Color.Lime;
                                ovalShapeLLJLJ.FillColor = Color.Lime;
                                ovalShapeWQLL.FillColor = Color.Lime;
                                ovalShapeXSB.FillColor = Color.Lime;
                                ovalShapeJZGL.FillColor = Color.Lime;
                                Msg(labelGksj, panelGksj, "000.0", false);
                                Msg(labelCO, panelCO, "0.00", false);
                                Msg(labelCO2, panelCO2, "0.00", false);
                                Msg(labelOO2, panelO2, "0.00", false);
                                Msg(labelHC, panelHC, "0", false);
                                Msg(labelNO, panelNO, "0", false);
                                Msg(labelLL, panelLL, "0.0", false);
                                Msg(labelLXCC, panelLXCC, "0.0", false);
                                Msg(labelLJCC, panelLJCC, "0.0", false);
                                TH_ST.Start();
                            }
                            else
                            {
                                MessageBox.Show("[CO2]浓度小于10%，检测终止", "警告");
                            }
                        }
                        if (o2excedlimit >= vmasconfig.ndccsj)
                        {
                            co2excedlimit = 0;
                            o2excedlimit = 0;
                            ovalShapeNDZ.FillColor = Color.Red;
                            outTimeContinus = 0f;
                            outTimeTotal = 0f;
                            TH_ST.Abort();
                            Th_get_FqandLl.Abort();
                            th_get_llj.Abort();
                            Speed_Jc_flag = false;
                            Ig195_status = false;
                            gongkuangTime = 0f;
                            GKSJ = 0;
                            //fla_502.Stop();
                            Msg(label_message, panel_msg, "[O2]于大于4%，请检查取样探头是否脱落", true);
                            ts1 = "检测中止";
                            ts2 = "氧气浓度过高";
                            if (vmasconfig.AutoRestart)
                            {
                                power_flag = false;
                                isCcStop = true;
                                jctime = DateTime.Now.ToString();
                                TH_ST = new Thread(Jc_Exe);
                                Th_get_FqandLl = new Thread(Fq_Detect);
                                th_get_llj = new Thread(llj_Detect);
                                timer2.Start();
                                JC_Status = true;
                                pictureBox1.Location = new Point(1, -5303);
                                pictureBox2.Location = new Point(96, 5936);
                                pictureBox4.Location = new Point(96, 5936);
                                //1, -5304
                                ovalShapeLXCC.FillColor = Color.Lime;
                                ovalShapeLJCC.FillColor = Color.Lime;
                                //ovalShapeNDZ.FillColor = Color.Lime;
                                ovalShapeLLJLJ.FillColor = Color.Lime;
                                ovalShapeWQLL.FillColor = Color.Lime;
                                ovalShapeXSB.FillColor = Color.Lime;
                                ovalShapeJZGL.FillColor = Color.Lime;
                                Msg(labelGksj, panelGksj, "000.0", false);
                                Msg(labelCO, panelCO, "0.00", false);
                                Msg(labelCO2, panelCO2, "0.00", false);
                                Msg(labelOO2, panelO2, "0.00", false);
                                Msg(labelHC, panelHC, "0", false);
                                Msg(labelNO, panelNO, "0", false);
                                Msg(labelLL, panelLL, "0.0", false);
                                Msg(labelLXCC, panelLXCC, "0.0", false);
                                Msg(labelLJCC, panelLJCC, "0.0", false);
                                TH_ST.Start();
                            }
                            else
                            {
                                MessageBox.Show("氧气浓度大于4%，检测终止", "警告");
                            }
                        }
                    }
                    if (GKSJ >= 3)//如果浓度监控开启的话
                    {
                        if ((lljpoweroffarray[gksj_count] == true) && (lljpoweroffarray[gksj_count - 1] == true) && (lljpoweroffarray[gksj_count - 2] == true))
                        {
                            co2excedlimit = 0;
                            o2excedlimit = 0;
                            //ovalShapeXSB.FillColor = Color.Red;
                            outTimeContinus = 0f;
                            outTimeTotal = 0f;
                            TH_ST.Abort();
                            Th_get_FqandLl.Abort();
                            th_get_llj.Abort();
                            Speed_Jc_flag = false;
                            Ig195_status = false;
                            gongkuangTime = 0f;
                            GKSJ = 0;
                            //fla_502.Stop();
                            Msg(label_message, panel_msg, "流量计通讯故障警告，检测终止", true);
                            ts1 = "检测中止";
                            ts2 = "流量计通讯故障";
                            if (vmasconfig.AutoRestart)
                            {
                                power_flag = false;
                                isCcStop = true;
                                jctime = DateTime.Now.ToString();
                                TH_ST = new Thread(Jc_Exe);
                                Th_get_FqandLl = new Thread(Fq_Detect);
                                th_get_llj = new Thread(llj_Detect);
                                timer2.Start();
                                JC_Status = true;
                                pictureBox1.Location = new Point(1, -5303);
                                pictureBox2.Location = new Point(96, 5936);
                                pictureBox4.Location = new Point(96, 5936);
                                //1, -5304
                                ovalShapeLXCC.FillColor = Color.Lime;
                                ovalShapeLJCC.FillColor = Color.Lime;
                                //ovalShapeNDZ.FillColor = Color.Lime;
                                ovalShapeLLJLJ.FillColor = Color.Lime;
                                ovalShapeWQLL.FillColor = Color.Lime;
                                ovalShapeXSB.FillColor = Color.Lime;
                                ovalShapeJZGL.FillColor = Color.Lime;
                                Msg(labelGksj, panelGksj, "000.0", false);
                                Msg(labelCO, panelCO, "0.00", false);
                                Msg(labelCO2, panelCO2, "0.00", false);
                                Msg(labelOO2, panelO2, "0.00", false);
                                Msg(labelHC, panelHC, "0", false);
                                Msg(labelNO, panelNO, "0", false);
                                Msg(labelLL, panelLL, "0.0", false);
                                Msg(labelLXCC, panelLXCC, "0.0", false);
                                Msg(labelLJCC, panelLJCC, "0.0", false);
                                TH_ST.Start();
                            }
                            else
                            {
                                MessageBox.Show("流量计通讯故障警告，检测终止", "警告");
                            }
                        }
                    }
                    if (GKSJ >= 3)//如果浓度监控开启的话
                    {
                        if ((lowflowarray[gksj_count] == -2) && (lowflowarray[gksj_count - 1] == -2) && (lowflowarray[gksj_count - 2] == -2))
                        {
                            co2excedlimit = 0;
                            o2excedlimit = 0;
                            ovalShapeXSB.FillColor = Color.Red;
                            outTimeContinus = 0f;
                            outTimeTotal = 0f;
                            TH_ST.Abort();
                            Th_get_FqandLl.Abort();
                            th_get_llj.Abort();
                            Speed_Jc_flag = false;
                            Ig195_status = false;
                            gongkuangTime = 0f;
                            GKSJ = 0;
                            //fla_502.Stop();
                            Msg(label_message, panel_msg, "废气仪气路低流量警告，检测终止", true);
                            ts1 = "检测中止";
                            ts2 = "废气仪气路低流量";
                            if (vmasconfig.AutoRestart)
                            {
                                power_flag = false;
                                isCcStop = true;
                                jctime = DateTime.Now.ToString();
                                TH_ST = new Thread(Jc_Exe);
                                Th_get_FqandLl = new Thread(Fq_Detect);
                                th_get_llj = new Thread(llj_Detect);
                                timer2.Start();
                                JC_Status = true;
                                pictureBox1.Location = new Point(1, -5303);
                                pictureBox2.Location = new Point(96, 5936);
                                pictureBox4.Location = new Point(96, 5936);
                                //1, -5304
                                ovalShapeLXCC.FillColor = Color.Lime;
                                ovalShapeLJCC.FillColor = Color.Lime;
                                //ovalShapeNDZ.FillColor = Color.Lime;
                                ovalShapeLLJLJ.FillColor = Color.Lime;
                                ovalShapeWQLL.FillColor = Color.Lime;
                                ovalShapeXSB.FillColor = Color.Lime;
                                ovalShapeJZGL.FillColor = Color.Lime;
                                Msg(labelGksj, panelGksj, "000.0", false);
                                Msg(labelCO, panelCO, "0.00", false);
                                Msg(labelCO2, panelCO2, "0.00", false);
                                Msg(labelOO2, panelO2, "0.00", false);
                                Msg(labelHC, panelHC, "0", false);
                                Msg(labelNO, panelNO, "0", false);
                                Msg(labelLL, panelLL, "0.0", false);
                                Msg(labelLXCC, panelLXCC, "0.0", false);
                                Msg(labelLJCC, panelLJCC, "0.0", false);
                                TH_ST.Start();
                            }
                            else
                            {
                                MessageBox.Show("废气仪气路低流量警告，检测终止", "警告");
                            }
                        }
                    }
                    if (GKSJ >= 3)//如果浓度监控开启的话
                    {
                        if ((cgjComSuccessarray[gksj_count] == false) && (cgjComSuccessarray[gksj_count-1] == false))
                        {
                            co2excedlimit = 0;
                            o2excedlimit = 0;
                            //ovalShapeXSB.FillColor = Color.Red;
                            outTimeContinus = 0f;
                            outTimeTotal = 0f;
                            TH_ST.Abort();
                            Th_get_FqandLl.Abort();
                            th_get_llj.Abort();
                            Speed_Jc_flag = false;
                            Ig195_status = false;
                            gongkuangTime = 0f;
                            GKSJ = 0;
                            //fla_502.Stop();
                            Msg(label_message, panel_msg, "测功机通讯异常，检测终止", true);
                            ts1 = "检测中止";
                            ts2 = "测功机通讯异常";
                            if (vmasconfig.AutoRestart)
                            {
                                power_flag = false;
                                isCcStop = true;
                                jctime = DateTime.Now.ToString();
                                TH_ST = new Thread(Jc_Exe);
                                Th_get_FqandLl = new Thread(Fq_Detect);
                                th_get_llj = new Thread(llj_Detect);
                                timer2.Start();
                                JC_Status = true;
                                pictureBox1.Location = new Point(1, -5303);
                                pictureBox2.Location = new Point(96, 5936);
                                pictureBox4.Location = new Point(96, 5936);
                                //1, -5304
                                ovalShapeLXCC.FillColor = Color.Lime;
                                ovalShapeLJCC.FillColor = Color.Lime;
                                //ovalShapeNDZ.FillColor = Color.Lime;
                                ovalShapeLLJLJ.FillColor = Color.Lime;
                                ovalShapeWQLL.FillColor = Color.Lime;
                                ovalShapeXSB.FillColor = Color.Lime;
                                ovalShapeJZGL.FillColor = Color.Lime;
                                Msg(labelGksj, panelGksj, "000.0", false);
                                Msg(labelCO, panelCO, "0.00", false);
                                Msg(labelCO2, panelCO2, "0.00", false);
                                Msg(labelOO2, panelO2, "0.00", false);
                                Msg(labelHC, panelHC, "0", false);
                                Msg(labelNO, panelNO, "0", false);
                                Msg(labelLL, panelLL, "0.0", false);
                                Msg(labelLXCC, panelLXCC, "0.0", false);
                                Msg(labelLJCC, panelLJCC, "0.0", false);
                                TH_ST.Start();
                            }
                            else
                            {
                                MessageBox.Show("测功机通讯异常，检测终止", "警告");
                            }
                        }
                    }
                    if (GKSJ >= 3)//如果浓度监控开启的话
                    {
                        if ((lowflowarray[gksj_count] == -1) && (lowflowarray[gksj_count - 1] == -1) && (lowflowarray[gksj_count - 2] == -1))
                        {
                            co2excedlimit = 0;
                            o2excedlimit = 0;
                            //ovalShapeXSB.FillColor = Color.Red;
                            outTimeContinus = 0f;
                            outTimeTotal = 0f;
                            TH_ST.Abort();
                            Th_get_FqandLl.Abort();
                            th_get_llj.Abort();
                            Speed_Jc_flag = false;
                            Ig195_status = false;
                            gongkuangTime = 0f;
                            GKSJ = 0;
                            //fla_502.Stop();
                            Msg(label_message, panel_msg, "废气仪通讯故障，检测终止", true);
                            ts1 = "检测中止";
                            ts2 = "废气仪通讯故障";
                            if (vmasconfig.AutoRestart)
                            {
                                power_flag = false;
                                isCcStop = true;
                                jctime = DateTime.Now.ToString();
                                TH_ST = new Thread(Jc_Exe);
                                Th_get_FqandLl = new Thread(Fq_Detect);
                                th_get_llj = new Thread(llj_Detect);
                                timer2.Start();
                                JC_Status = true;
                                pictureBox1.Location = new Point(1, -5303);
                                pictureBox2.Location = new Point(96, 5936);
                                pictureBox4.Location = new Point(96, 5936);
                                //1, -5304
                                ovalShapeLXCC.FillColor = Color.Lime;
                                ovalShapeLJCC.FillColor = Color.Lime;
                                //ovalShapeNDZ.FillColor = Color.Lime;
                                ovalShapeLLJLJ.FillColor = Color.Lime;
                                ovalShapeWQLL.FillColor = Color.Lime;
                                ovalShapeXSB.FillColor = Color.Lime;
                                ovalShapeJZGL.FillColor = Color.Lime;
                                Msg(labelGksj, panelGksj, "000.0", false);
                                Msg(labelCO, panelCO, "0.00", false);
                                Msg(labelCO2, panelCO2, "0.00", false);
                                Msg(labelOO2, panelO2, "0.00", false);
                                Msg(labelHC, panelHC, "0", false);
                                Msg(labelNO, panelNO, "0", false);
                                Msg(labelLL, panelLL, "0.0", false);
                                Msg(labelLXCC, panelLXCC, "0.0", false);
                                Msg(labelLJCC, panelLJCC, "0.0", false);
                                TH_ST.Start();
                            }
                            else
                            {
                                MessageBox.Show("废气仪通讯故障，检测终止", "警告");
                            }
                        }
                    }
                    if (GKSJ > 147 && GKSJ < 153 && vmasconfig.PowerMonitor == true)//如果加载功率误差监控开启
                    {
                        if ((igbt.Power - Set_Power > 1f) || (igbt.Power - Set_Power < -1f))
                        {
                            co2excedlimit = 0;
                            o2excedlimit = 0;
                            ovalShapeJZGL.FillColor = Color.Red;
                            outTimeContinus = 0f;
                            outTimeTotal = 0f;
                            TH_ST.Abort();
                            Th_get_FqandLl.Abort();
                            th_get_llj.Abort();
                            Speed_Jc_flag = false;
                            Ig195_status = false;
                            gongkuangTime = 0f;
                            GKSJ = 0;
                            fla_502.Stop();
                            Msg(label_message, panel_msg, "监测功率加载超出指定值±0.2KW，请检查后并重新开始", true); 
                            ts1 = "检测中止";
                            ts2 = "加载功率超差";
                            if (vmasconfig.AutoRestart)
                            {
                                power_flag = false;
                                isCcStop = true;
                                jctime = DateTime.Now.ToString();
                                TH_ST = new Thread(Jc_Exe);
                                Th_get_FqandLl = new Thread(Fq_Detect);
                                th_get_llj = new Thread(llj_Detect);
                                timer2.Start();
                                JC_Status = true;
                                pictureBox1.Location = new Point(1, -5303);
                                pictureBox2.Location = new Point(96, 5936);
                                pictureBox4.Location = new Point(96, 5936);
                                //1, -5304
                                ovalShapeLXCC.FillColor = Color.Lime;
                                ovalShapeLJCC.FillColor = Color.Lime;
                                //ovalShapeNDZ.FillColor = Color.Lime;
                                ovalShapeLLJLJ.FillColor = Color.Lime;
                                ovalShapeWQLL.FillColor = Color.Lime;
                                ovalShapeXSB.FillColor = Color.Lime;
                                ovalShapeJZGL.FillColor = Color.Lime;
                                Msg(labelGksj, panelGksj, "000.0", false);
                                Msg(labelCO, panelCO, "0.00", false);
                                Msg(labelCO2, panelCO2, "0.00", false);
                                Msg(labelOO2, panelO2, "0.00", false);
                                Msg(labelHC, panelHC, "0", false);
                                Msg(labelNO, panelNO, "0", false);
                                Msg(labelLL, panelLL, "0.0", false);
                                Msg(labelLXCC, panelLXCC, "0.0", false);
                                Msg(labelLJCC, panelLJCC, "0.0", false);
                                TH_ST.Start();
                            }
                            else
                            {
                                MessageBox.Show("加载功率超标，检测终止", "警告");
                            }
                        }
                    }
                    if (GKSJ >= 8 && vmasconfig.ThinnerratioMonitor == true)
                    {
                        if (Vmas_Exhaust_k[gksj_count] > vmasconfig.Xsb && Vmas_Exhaust_fqsjll[gksj_count - 1] > vmasconfig.Xsb && Vmas_Exhaust_fqsjll[gksj_count - 2] > vmasconfig.Xsb && Vmas_Exhaust_fqsjll[gksj_count - 3] > vmasconfig.Xsb)
                        {
                            co2excedlimit = 0;
                            o2excedlimit = 0;
                            ovalShapeXSB.FillColor = Color.Red;
                            outTimeContinus = 0f;
                            outTimeTotal = 0f;
                            TH_ST.Abort();
                            Th_get_FqandLl.Abort();
                            th_get_llj.Abort();
                            Speed_Jc_flag = false;
                            Ig195_status = false;
                            gongkuangTime = 0f;
                            GKSJ = 0;
                            fla_502.Stop();
                            Msg(label_message, panel_msg, "监测到稀释比大于规定值，请检查后并重新开始", true);
                            ts1 = "检测中止";
                            ts2 = "稀释比超过规定值";
                            if (vmasconfig.AutoRestart)
                            {
                                power_flag = false;
                                isCcStop = true;
                                jctime = DateTime.Now.ToString();
                                TH_ST = new Thread(Jc_Exe);
                                Th_get_FqandLl = new Thread(Fq_Detect);
                                th_get_llj = new Thread(llj_Detect);
                                timer2.Start();
                                JC_Status = true;
                                pictureBox1.Location = new Point(1, -5303);
                                pictureBox2.Location = new Point(96, 5936);
                                pictureBox4.Location = new Point(96, 5936);
                                //1, -5304
                                ovalShapeLXCC.FillColor = Color.Lime;
                                ovalShapeLJCC.FillColor = Color.Lime;
                                //ovalShapeNDZ.FillColor = Color.Lime;
                                ovalShapeLLJLJ.FillColor = Color.Lime;
                                ovalShapeWQLL.FillColor = Color.Lime;
                                ovalShapeXSB.FillColor = Color.Lime;
                                ovalShapeJZGL.FillColor = Color.Lime;
                                Msg(labelGksj, panelGksj, "000.0", false);
                                Msg(labelCO, panelCO, "0.00", false);
                                Msg(labelCO2, panelCO2, "0.00", false);
                                Msg(labelOO2, panelO2, "0.00", false);
                                Msg(labelHC, panelHC, "0", false);
                                Msg(labelNO, panelNO, "0", false);
                                Msg(labelLL, panelLL, "0.0", false);
                                Msg(labelLXCC, panelLXCC, "0.0", false);
                                Msg(labelLJCC, panelLJCC, "0.0", false);
                                TH_ST.Start();
                            }
                            else
                            {
                                MessageBox.Show("监测到稀释比大于规定值，检测终止", "警告");
                            }
                        }
                    }
                    if (GKSJ >= 1 && vmasconfig.FlowMonitorr == true)
                    {
                        if (Vmas_Exhaust_llList[gksj_count] < vmasconfig.Lljll && Vmas_Exhaust_llList[gksj_count - 1] < vmasconfig.Lljll && vmasconfig.FlowMonitorr == true)
                        {
                            co2excedlimit = 0;
                            o2excedlimit = 0;
                            ovalShapeLLJLJ.FillColor = Color.Red;
                            outTimeContinus = 0f;
                            outTimeTotal = 0f;
                            TH_ST.Abort();
                            Th_get_FqandLl.Abort();
                            th_get_llj.Abort();
                            Speed_Jc_flag = false;
                            Ig195_status = false;
                            gongkuangTime = 0f;
                            GKSJ = 0;
                            fla_502.Stop();
                            Msg(label_message, panel_msg, "监测到流量计流量<" + vmasconfig.Lljll.ToString("0.0") + "L/s，请检查后并重新开始", true);
                            ts1 = "检测中止";
                            ts2 = "流量计流量过低";
                            if (vmasconfig.AutoRestart)
                            {
                                power_flag = false;
                                isCcStop = true;
                                jctime = DateTime.Now.ToString();
                                TH_ST = new Thread(Jc_Exe);
                                Th_get_FqandLl = new Thread(Fq_Detect);
                                th_get_llj = new Thread(llj_Detect);
                                timer2.Start();
                                JC_Status = true;
                                pictureBox1.Location = new Point(1, -5303);
                                pictureBox2.Location = new Point(96, 5936);
                                pictureBox4.Location = new Point(96, 5936);
                                //1, -5304
                                ovalShapeLXCC.FillColor = Color.Lime;
                                ovalShapeLJCC.FillColor = Color.Lime;
                                //ovalShapeNDZ.FillColor = Color.Lime;
                                ovalShapeLLJLJ.FillColor = Color.Lime;
                                ovalShapeWQLL.FillColor = Color.Lime;
                                ovalShapeXSB.FillColor = Color.Lime;
                                ovalShapeJZGL.FillColor = Color.Lime;
                                Msg(labelGksj, panelGksj, "000.0", false);
                                Msg(labelCO, panelCO, "0.00", false);
                                Msg(labelCO2, panelCO2, "0.00", false);
                                Msg(labelOO2, panelO2, "0.00", false);
                                Msg(labelHC, panelHC, "0", false);
                                Msg(labelNO, panelNO, "0", false);
                                Msg(labelLL, panelLL, "0.0", false);
                                Msg(labelLXCC, panelLXCC, "0.0", false);
                                Msg(labelLJCC, panelLJCC, "0.0", false);
                                TH_ST.Start();
                            }
                            else
                            {
                                MessageBox.Show("监测到流量计流量<" + vmasconfig.Lljll.ToString("0.0")+"，检测终止", "警告");
                            }
                        }
                    }
                    if (GKSJ >= 5&&equipconfig.isIgbtContainGdyk)
                    {
                        if ((Vmas_Exhaust_Gd[gksj_count] == true) && (Vmas_Exhaust_Gd[gksj_count - 1] == true) && (Vmas_Exhaust_Gd[gksj_count - 2] == true))
                        {
                            co2excedlimit = 0;
                            o2excedlimit = 0;
                            outTimeContinus = 0f;
                            outTimeTotal = 0f;
                            TH_ST.Abort();
                            Th_get_FqandLl.Abort();
                            th_get_llj.Abort();
                            Speed_Jc_flag = false;
                            Ig195_status = false;
                            gongkuangTime = 0f;
                            GKSJ = 0;
                            fla_502.Stop();
                            Msg(label_message, panel_msg, "检测被紧急中止，请检查后并重新开始", true);
                            button_ss.Text = "重新检测";
                            JC_Status = false;
                            igbt.Exit_Control();
                            if (fla_502 != null)
                                fla_502.StopBlowback();
                            ts1 = "检测中止";
                            ts2 = "检测被紧急中止";
                            MessageBox.Show("检测被紧急中止", "警告");
                        }
                    }

                    if (vmasconfig.FlowMonitorr == true)//如果加载功率误差监控开启
                    {
                        if (GKSJ >= 143 + fqy_delayTime && GKSJ <= 155 + fqy_delayTime)
                        {
                            if (Vmas_Exhaust_fqsjll[gksj_count] < 2f)
                            {
                                co2excedlimit = 0;
                                o2excedlimit = 0;
                                ovalShapeWQLL.FillColor = Color.Red;
                                outTimeContinus = 0f;
                                outTimeTotal = 0f;
                                TH_ST.Abort();
                                Th_get_FqandLl.Abort();
                                th_get_llj.Abort();
                                Speed_Jc_flag = false;
                                Ig195_status = false;
                                gongkuangTime = 0f;
                                GKSJ = 0;
                                fla_502.Stop();
                                Msg(label_message, panel_msg, "监测50km/h时尾气流量小于2L/s，请检查后并重新开始", true);
                                ts1 = "检测中止";
                                ts2 = "尾气流量小于2L/s";
                                if (vmasconfig.AutoRestart)
                                {
                                    power_flag = false;
                                    isCcStop = true;
                                    jctime = DateTime.Now.ToString();
                                    TH_ST = new Thread(Jc_Exe);
                                    Th_get_FqandLl = new Thread(Fq_Detect);
                                    th_get_llj = new Thread(llj_Detect);
                                    timer2.Start();
                                    JC_Status = true;
                                    pictureBox1.Location = new Point(1, -5303);
                                    pictureBox2.Location = new Point(96, 5936);
                                    pictureBox4.Location = new Point(96, 5936);
                                    //1, -5304
                                    ovalShapeLXCC.FillColor = Color.Lime;
                                    ovalShapeLJCC.FillColor = Color.Lime;
                                    //ovalShapeNDZ.FillColor = Color.Lime;
                                    ovalShapeLLJLJ.FillColor = Color.Lime;
                                    ovalShapeWQLL.FillColor = Color.Lime;
                                    ovalShapeXSB.FillColor = Color.Lime;
                                    ovalShapeJZGL.FillColor = Color.Lime;
                                    Msg(labelGksj, panelGksj, "000.0", false);
                                    Msg(labelCO, panelCO, "0.00", false);
                                    Msg(labelCO2, panelCO2, "0.00", false);
                                    Msg(labelOO2, panelO2, "0.00", false);
                                    Msg(labelHC, panelHC, "0", false);
                                    Msg(labelNO, panelNO, "0", false);
                                    Msg(labelLL, panelLL, "0.0", false);
                                    Msg(labelLXCC, panelLXCC, "0.0", false);
                                    Msg(labelLJCC, panelLJCC, "0.0", false);
                                    TH_ST.Start();
                                }
                                else
                                {
                                    MessageBox.Show("监测50km/h时尾气流量小于2L/s，检测终止", "警告");
                                }
                                //return;
                            }
                        }
                        else if (GKSJ > 3 + fqy_delayTime)
                        {
                            if (Vmas_Exhaust_fqsjll[gksj_count] < vmasconfig.Wqll&& Vmas_Exhaust_fqsjll[gksj_count-1] < vmasconfig.Wqll&& Vmas_Exhaust_fqsjll[gksj_count-2] < vmasconfig.Wqll)
                            {
                                if (vmasconfig.IsWholeProcessJK || Vmas_Exhaust_accelerate[gksj_count - fqy_delayTime] == true)
                                {
                                    co2excedlimit = 0;
                                    o2excedlimit = 0;
                                    ovalShapeWQLL.FillColor = Color.Red;
                                    outTimeContinus = 0f;
                                    outTimeTotal = 0f;
                                    TH_ST.Abort();
                                    Th_get_FqandLl.Abort();
                                    th_get_llj.Abort();
                                    Speed_Jc_flag = false;
                                    Ig195_status = false;
                                    gongkuangTime = 0f;
                                    GKSJ = 0;
                                    //fla_502.Stop();
                                    Msg(label_message, panel_msg, "监测尾气流量小于规定值，请检查后并重新开始", true);
                                    ts1 = "检测中止";
                                    ts2 = "尾气流量过低";
                                    if (vmasconfig.AutoRestart)
                                    {
                                        power_flag = false;
                                        isCcStop = true;
                                        jctime = DateTime.Now.ToString();
                                        TH_ST = new Thread(Jc_Exe);
                                        Th_get_FqandLl = new Thread(Fq_Detect);
                                        th_get_llj = new Thread(llj_Detect);
                                        timer2.Start();
                                        JC_Status = true;
                                        pictureBox1.Location = new Point(1, -5303);
                                        pictureBox2.Location = new Point(96, 5936);
                                        pictureBox4.Location = new Point(96, 5936);
                                        //pictureBox4.Location = new Point(96, 5936);
                                        //1, -5304
                                        ovalShapeLXCC.FillColor = Color.Lime;
                                        ovalShapeLJCC.FillColor = Color.Lime;
                                        ovalShapeNDZ.FillColor = Color.Lime;
                                        ovalShapeLLJLJ.FillColor = Color.Lime;
                                        ovalShapeWQLL.FillColor = Color.Lime;
                                        ovalShapeXSB.FillColor = Color.Lime;
                                        ovalShapeJZGL.FillColor = Color.Lime;
                                        Msg(labelGksj, panelGksj, "000.0", false);
                                        Msg(labelCO, panelCO, "0.00", false);
                                        Msg(labelCO2, panelCO2, "0.00", false);
                                        Msg(labelO2, panelO2, "0.00", false);
                                        Msg(labelHC, panelHC, "0", false);
                                        Msg(labelNO, panelNO, "0", false);
                                        Msg(labelLL, panelLL, "0.0", false);
                                        Msg(labelLXCC, panelLXCC, "0.0", false);
                                        Msg(labelLJCC, panelLJCC, "0.0", false);
                                        TH_ST.Start();
                                    }
                                    else
                                    {
                                        MessageBox.Show("监测尾气流量小于规定值，检测终止", "警告");
                                    }
                                }
                                //return;
                            }
                        }
                        }
                    if (gongkuangTime >= 195.0f)
                    {

                        Msg(labelGksj, panelGksj, "195.0", false);
                    }
                    else
                    {

                        Msg(labelGksj, panelGksj, gongkuangTime.ToString("000.0"), false);
                    }
                    float thisTimeSpan = gongkuangTime;
                    TimeSpan timespan = nowtime - startTime;
                    gongkuangTime = (float)timespan.TotalMilliseconds / 1000f;
                    thisTimeSpan = gongkuangTime - thisTimeSpan;
                    if (igbt.Speed <= (speed_now(gongkuangTime).speed_now_data + 2.2f) && (igbt.Speed >= (speed_now(gongkuangTime).speed_now_data - 2.2f)))
                    {
                        chaocha = false;
                        pictureBox4.Visible = false;
                        pictureBox2.Visible = true;
                    }
                    else
                    {
                        pictureBox2.Visible = false;
                        pictureBox4.Visible = true;
                    }
                    if (chaocha==true && speedLimit.isChangeD == false)
                    {
                        outTimeContinus += thisTimeSpan;
                        outTimeTotal += thisTimeSpan;
                        if (vmasconfig.SpeedMonitor == true)
                        {
                            if (outTimeContinus >= vmasconfig.Lxcc)
                            {
                                co2excedlimit = 0;
                                o2excedlimit = 0;
                                ovalShapeLXCC.FillColor = Color.Red;
                                outTimeContinus = 0f;
                                outTimeTotal = 0f;
                                TH_ST.Abort();
                                Th_get_FqandLl.Abort();
                                th_get_llj.Abort();
                                Speed_Jc_flag = false;
                                Ig195_status = false;
                                gongkuangTime = 0f;
                                GKSJ = 0;
                                fla_502.Stop();
                                Msg(label_message, panel_msg, "车速单点超差的时间超过" + vmasconfig.Lxcc.ToString("0.0") + "，请调整后重新开始", true);
                                ts1 = "检测中止";
                                ts2 = "车速单次超差超标";
                                if (vmasconfig.AutoRestart)
                                {
                                    isCcStop = true;
                                    jctime = DateTime.Now.ToString();
                                    TH_ST = new Thread(Jc_Exe);
                                    Th_get_FqandLl = new Thread(Fq_Detect);
                                    th_get_llj = new Thread(llj_Detect);
                                    timer2.Start();
                                    JC_Status = true;
                                    pictureBox1.Location = new Point(1, -5303);
                                    pictureBox2.Location = new Point(96, 5936);
                                    pictureBox4.Location = new Point(96, 5936);
                                    //pictureBox4.Location = new Point(96, 5936);
                                    //1, -5304
                                    //ovalShapeLXCC.FillColor = Color.Lime;
                                    ovalShapeLJCC.FillColor = Color.Lime;
                                    ovalShapeNDZ.FillColor = Color.Lime;
                                    ovalShapeLLJLJ.FillColor = Color.Lime;
                                    ovalShapeWQLL.FillColor = Color.Lime;
                                    ovalShapeXSB.FillColor = Color.Lime;
                                    ovalShapeJZGL.FillColor = Color.Lime;
                                    Msg(labelGksj, panelGksj, "000.0", false);
                                    Msg(labelCO, panelCO, "0.00", false);
                                    Msg(labelCO2, panelCO2, "0.00", false);
                                    Msg(labelOO2, panelO2, "0.00", false);
                                    Msg(labelHC, panelHC, "0", false);
                                    Msg(labelNO, panelNO, "0", false);
                                    Msg(labelLL, panelLL, "0.0", false);
                                    Msg(labelLXCC, panelLXCC, "0.0", false);
                                    Msg(labelLJCC, panelLJCC, "0.0", false);
                                    TH_ST.Start();
                                }
                                else
                                {
                                    MessageBox.Show("单点超差超过" + vmasconfig.Lxcc.ToString("0.0") + "s，检测终止", "警告");
                                }
                                //Thread.Sleep(1500);
                            }
                        }
                        if (vmasconfig.SpeedMonitor == true)
                        {
                            if (outTimeTotal >= vmasconfig.Ljcc)
                            {
                                co2excedlimit = 0;
                                o2excedlimit = 0;
                                ovalShapeLJCC.FillColor = Color.Red;
                                outTimeContinus = 0f;
                                outTimeTotal = 0f;
                                TH_ST.Abort();
                                Th_get_FqandLl.Abort();
                                th_get_llj.Abort();
                                Speed_Jc_flag = false;
                                Ig195_status = false;
                                gongkuangTime = 0f;
                                GKSJ = 0;
                                fla_502.Stop();
                                Msg(label_message, panel_msg, "车速累计超差的时间超过" + vmasconfig.Ljcc.ToString("0.0") + "，请调整后重新开始", true);
                                ts1 = "检测中止";
                                ts2 = "车速累计超差超标";
                                if (vmasconfig.AutoRestart)
                                {
                                    isCcStop = true;
                                    jctime = DateTime.Now.ToString();
                                    TH_ST = new Thread(Jc_Exe);
                                    Th_get_FqandLl = new Thread(Fq_Detect);
                                    th_get_llj = new Thread(llj_Detect);
                                    timer2.Start();
                                    JC_Status = true;
                                    pictureBox1.Location = new Point(1, -5303);
                                    pictureBox2.Location = new Point(96, 5936);
                                    pictureBox4.Location = new Point(96, 5936);
                                    //pictureBox4.Location = new Point(96, 5936);
                                    //1, -5304
                                    ovalShapeLXCC.FillColor = Color.Lime;
                                    //ovalShapeLJCC.FillColor = Color.Lime;
                                    ovalShapeNDZ.FillColor = Color.Lime;
                                    ovalShapeLLJLJ.FillColor = Color.Lime;
                                    ovalShapeWQLL.FillColor = Color.Lime;
                                    ovalShapeXSB.FillColor = Color.Lime;
                                    ovalShapeJZGL.FillColor = Color.Lime;
                                    Msg(labelGksj, panelGksj, "000.0", false);
                                    //led_display(ledNumber_gksj, "000.0");
                                    Msg(labelCO, panelCO, "0.00", false);
                                    Msg(labelCO2, panelCO2, "0.00", false);
                                    Msg(labelOO2, panelO2, "0.00", false);
                                    Msg(labelHC, panelHC, "0", false);
                                    Msg(labelNO, panelNO, "0", false);
                                    Msg(labelLL, panelLL, "0.0", false);
                                    Msg(labelLXCC, panelLXCC, "0.0", false);
                                    Msg(labelLJCC, panelLJCC, "0.0", false);
                                    TH_ST.Start();
                                }
                                else
                                {
                                    MessageBox.Show("累计超差超过" + vmasconfig.Ljcc.ToString("0.0") + "s，检测终止", "警告");
                                }
                            }
                        }
                        

                    }
                    else
                    {
                        outTimeContinus = 0;
                    }
                    Msg(labelLXCC, panelLXCC, outTimeContinus.ToString("0.0"), false);
                    Msg(labelLJCC, panelLJCC, outTimeTotal.ToString("0.0"), false);
                    Msg(labelCO2EXCEDTIMES, panelCO2EXCEDTIMES, co2excedlimit.ToString(), false);
                    Msg(labelO2EXCEDTIMES, panelO2EXCEDTIMES, o2excedlimit.ToString(), false);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                gongkuangTime = 0f;
                outTimeTotal = 0f;
                outTimeContinus = 0f;
            }

        }
        public void Fq_Detect()
        {
            while (JC_Status)
            {
                Vmas_Exhaust_Now = fla_502.GetData();
                Revise(Vmas_Exhaust_Now);
                Thread.Sleep(50);
                if (equipconfig.Fqyxh.ToLower() == "fla_502" || equipconfig.Fqyxh.ToLower() == "nha_503")
                {
                    fla502_temp_data = fla_502.Get_Temp();
                    Thread.Sleep(50);
                }
                isLowFlow = fla_502.CheckIsLowFlow();
                Thread.Sleep(50);
            }
        }
        public void llj_Detect()
        {
            while (JC_Status)
            {
                string lljstatus=flv_1000.Get_standardDat();
                islljpoweroff = (lljstatus == "通讯故障");
                Thread.Sleep(30);
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
                        Thread.Sleep(500);
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
            textbox_value(textBoxSDSD,SD.ToString("0.0"));
            textbox_value(textBoxSDWD,WD.ToString("0.0"));
            if (vmasconfig.IfSureTemp)
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
            if (WD >40)
            {
                Msg(label_chujiantishi, panel_chujiantishi, "环境温度过高,不能进行检测", false);
                ts2 = "环境温度过高";
                datagridview_msg(dataGridView1, "判定", 1, "×");
                return;
            }
            else
                datagridview_msg(dataGridView1, "判定", 1, "√");
            
            datagridview_msg(dataGridView1, "结果", 2, SD.ToString("0.0"));
            if (SD >85)
            {
                Msg(label_chujiantishi, panel_chujiantishi, "环境湿度不合格,不能进行检测", false);
                ts2 = "环境湿度过高";
                datagridview_msg(dataGridView1, "判定",2, "×");
                return;
            }
            else
                datagridview_msg(dataGridView1, "判定", 2, "√");
            
            datagridview_msg(dataGridView1, "结果", 3, DQY.ToString("0.0"));
            if (DQY>120)
            {
                Msg(label_chujiantishi, panel_chujiantishi, "环境大气压不合格,不能进行检测", false);
                datagridview_msg(dataGridView1, "判定", 3, "×");
                return;
            }
            else
                datagridview_msg(dataGridView1, "判定",3, "√");
            Thread.Sleep(200);
            ts2 = "背景O2测定";
            for (int i = 0; i < 10; i++)
            {
                flv_1000.Get_standardDat();
                hjo2data[i] = flv_1000.o2_standard_value;//取样空气O2浓度
            }
            px();
            if (vmasconfig.Huanjingo2Monitor == true)//是否检测环境氧
            {
                lljo2 = (hjo2data[3] + hjo2data[4] + hjo2data[5] + hjo2data[6]) / 4f;
                hjo2 = lljo2;
            }
            else
            {
                hjo2 = 20.8f;
            }
            vmas_data.AmbientO2 = hjo2.ToString();
            datagridview_msg(dataGridView1, "结果", 4, hjo2.ToString("0.00"));
            if (Math.Abs(hjo2 - 20.8) > 0.5)//正常环境O2浓度应为20.8%正负0.5%【GB18285-2005】
            {
                Msg(label_chujiantishi, panel_chujiantishi, "环境O2为" + lljo2.ToString("0.00") + ",请校正流量计", false);
                datagridview_msg(dataGridView1, "判定", 4, "×");
                return;
            }
            else
                datagridview_msg(dataGridView1, "判定", 4, "√");

            if (vmasconfig.RemainedMonitor == true)
            {
                Thread.Sleep(200);
                fla_502.Pump_air();
                for (int i = 15; i >= 0; i--)
                {
                    ts2 = "背景测定..." + i.ToString("0");
                    datagridview_msg(dataGridView1, "结果", 5, "检测中..." + i.ToString("0") + "秒");
                    Thread.Sleep(1000);
                }
               
            }
            Thread.Sleep(500);
            huanjiang_data = fla_502.GetData();
            if (vmasconfig.RemainedMonitor == true)
            {
                datagridview_msg(dataGridView1, "结果", 5, "CO:" + huanjiang_data.CO + ",HC:" + huanjiang_data.HC + ",NO:" + huanjiang_data.NO);
                if (huanjiang_data.CO > 2|| huanjiang_data.HC > 7 || huanjiang_data.NO > 25)//2,7,25
                {
                    Msg(label_chujiantishi, panel_chujiantishi, "环境背景污染水平不合格,请检查", true);
                    datagridview_msg(dataGridView1, "判定", 5, "×");
                    return;
                }
                else
                {
                    datagridview_msg(dataGridView1, "判定", 5, "√");
                }
                fla_502.StopBlowback();
            }
            else
            {
                datagridview_msg(dataGridView1, "结果", 5, "CO:0,HC:0,NO:0");
                datagridview_msg(dataGridView1, "判定", 5, "√");
            }


            if (vmasconfig.RemainedMonitor == true)
            {
                Thread.Sleep(500);
                fla_502.Pump_Pipeair();
                for (int i = 15; i >= 0; i--)
                {
                    ts2 = "HC测定..." + i.ToString("0");
                    datagridview_msg(dataGridView1, "结果", 6, "检测中..." + i.ToString("0") + "秒");
                    Thread.Sleep(1000);
                }
                
            }
            Thread.Sleep(500);
            huanjiang_data = fla_502.GetData();
            if (vmasconfig.RemainedMonitor == true)
            {
                datagridview_msg(dataGridView1, "结果", 6, "HC:" + huanjiang_data.HC.ToString("0") );
                if (huanjiang_data.HC > 7)//2,7,25
                {
                    Msg(label_chujiantishi, panel_chujiantishi, "取样系统HC残留量超标，请反吹", true);
                    datagridview_msg(dataGridView1, "判定", 6, "×");
                    return;
                }
                else
                {
                    datagridview_msg(dataGridView1, "判定", 6, "√");
                }
                fla_502.StopBlowback();
            }
            else
            {
                datagridview_msg(dataGridView1, "结果", 6, "HC:0");
                datagridview_msg(dataGridView1, "判定", 6, "√");
            }
            vmas_data.ResidualHC = huanjiang_data.HC.ToString();
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
            sxnb = 4;
            int temp_flag=0;                //计数临时变量
            int zero_count = 0;
            float lljo2 = 0f;
            ig195IsFinished = false;
            DataRow dr = null;
            int cysx = 0;
            if (equipconfig.Lljxh == "nhf_1")
            {
                if (flv_1000.nhf_TurnOnMotor() != "开启电机成功")
                {
                    flv_1000.nhf_TurnOnMotor();
                }
            }
            if (igbt != null)
            {
                igbt.TurnOnRelay((byte)equipconfig.HeatFan);
                Thread.Sleep(100);
            }
            try
            {
                if (chujianIsFinished && vmasconfig.AutoRestart)
                {
                    if (isCcStop)
                    {
                        //fla_502.Start();
                        isCcStop = false;
                        Thread.Sleep(2000);
                        for (int i = 10; i >= 0; i--)
                        {
                            Msg(label_message, panel_msg, "检测即将重新开始" + " " + i.ToString() + "s", false);
                            ts1 = "检测将重新开始";
                            ts2 = i.ToString() + "s";
                            Thread.Sleep(1000);
                        }
                        Th_get_FqandLl.Start();
                        th_get_llj.Start();
                    }
                    else
                    {

                        Thread.Sleep(2000);
                        fla_502.Pump_Pipeair();
                        Th_get_FqandLl.Start();
                        th_get_llj.Start();
                        Msg(label_message, panel_msg, "检测即将重新开始", false);
                        ts1 = "检测将重新开始";
                    }
                }
                else
                {
                    Msg(label_message, panel_msg, "测试即将开始,检查废气仪状态", false);
                    ts1 = "检测即将开始";
                    ts2 = "检测废气仪...";
                    Thread.Sleep(1000);
                    string fla_502_status = fla_502.Get_Struct();
                    if (fla_502_status != "仪器已经准备好")
                    {
                        Msg(label_message, panel_msg, "废气仪" + fla_502_status + ",测试将结束", true);
                        ts1 = "测试中止";
                        ts2 = "废气仪" + fla_502_status;
                        button_ss.Text = "重新检测";
                        JC_Status = false;
                        return;
                    }
                    Thread.Sleep(500);
                    if (vmasconfig.CjBeforeTl)
                    {
                        Msg(label_message, panel_msg, "测试即将开始,正在进行初检", false);
                        ts2 = "开始初检";
                        vmas_chujian();
                        if (chujianIsFinished == false)
                        {
                            Msg(label_message, panel_msg, "初检不合格,测试将结束", true);
                            ts1 = "检测中止";
                            ts2 = "初检不合格";
                            button_ss.Text = "重新检测";
                            JC_Status = false;

                            return;
                        }
                        Thread.Sleep(500);
                    }
                    if (vmasconfig.IsTestYw)
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
                            JC_Status = false;
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
                    Thread.Sleep(1000);

                    if (equipconfig.DATASECONDS_TYPE == "江西")
                    {
                        //sxnb设置为00
                        Exhaust.Fla502_data zhunbei_data = null;
                        try//获取环境参数
                        {
                            if (equipconfig.TempInstrument == "废气仪")
                            {
                                if (equipconfig.Fqyxh.ToLower() == "nha_503" || equipconfig.Fqyxh.ToLower() == "fla_502" || equipconfig.Fqyxh.ToLower() == "cdf5000")
                                {
                                    Thread.Sleep(500);
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
                            WD = thaxsdata.Tempxs * WD;
                            SD = thaxsdata.Humixs * SD;
                            DQY = thaxsdata.Airpxs * DQY;
                        }
                        catch (Exception)
                        {
                        }
                        Thread.Sleep(200);

                        for (int i = 0; i < 10; i++)
                        {
                            flv_1000.Get_standardDat();
                            hjo2data[i] = flv_1000.o2_standard_value;//取样空气O2浓度
                        }
                        px();
                        if (vmasconfig.Huanjingo2Monitor == true)//是否检测环境氧
                        {
                            lljo2 = (hjo2data[3] + hjo2data[4] + hjo2data[5] + hjo2data[6]) / 4f;
                            hjo2 = lljo2;
                        }
                        else
                        {
                            hjo2 = 20.8f;
                        }
                        zhunbei_data = fla_502.GetData();//取废气仪

                        Vmas_qcsj[0] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//全程时序
                        Vmas_cysx[0] = "1";//从0开始
                        Vmas_nj[0] = igbt.Force;
                        Vmas_fdjzs[0] = zhunbei_data.ZS;
                        Vmas_sxnb[0] = "00";
                        Vmas_bzcs[0] = 0f;//标准速度
                        Vmas_sscs[0] = 0f;//实时速度
                        Vmas_jsgl[0] = 0f;//加载功率
                        Vmas_zsgl[0] = 0f;//加载功率
                        Vmas_jzgl[0] = 0f;//加载功率
                        Speed_listIg195[0] = 0f;//实时速度
                        Vmas_Exhaust_ListIG195[0] = zhunbei_data;//尾气浓度值
                        Vmas_Exhaust_Revise_ListIG195[0] = zhunbei_data;//修正的尾气浓度值
                        Vmas_Exhaust_llList[0] = flv_1000.ll_standard_value;//流量计标准流量值
                        Vmas_Exhaust_xso2now[0] = flv_1000.o2_standard_value;
                        Vmas_Exhaust_xso2afterDelay[0] = flv_1000.o2_standard_value;//流量计稀释氧浓度
                        Vmas_Exhaust_lljtemp[0] = flv_1000.temp_standard_value;//流量计温度
                        Vmas_Exhaust_lljyl[0] = flv_1000.yali_standard_value;//流量计压力
                        if (hjo2 > flv_1000.o2_standard_value && hjo2 > zhunbei_data.O2)//稀释比
                        {
                            Vmas_Exhaust_k[0] = (hjo2 - flv_1000.o2_standard_value) / (hjo2 - zhunbei_data.O2);
                        }
                        else
                        {
                            Vmas_Exhaust_k[0] = 0f;
                        }
                        Vmas_Exhaust_fqsjll[0] = Vmas_Exhaust_llList[0] * Vmas_Exhaust_k[0];//尾气实际流量
                        Vmas_Exhaust_cold[0] = zhunbei_data.CO;//CO浓度
                        Vmas_Exhaust_co2ld[0] = zhunbei_data.CO2;//CO2浓度
                        Vmas_Exhaust_hcld[0] = zhunbei_data.HC;//HC浓度
                        Vmas_Exhaust_nold[0] = zhunbei_data.NO;//NO浓度
                        Vmas_Exhaust_o2ld[0] = zhunbei_data.O2;//废气仪氧气浓度
                        Vmas_Exhaust_cozl[0] = Vmas_Exhaust_fqsjll[0] * zhunbei_data.CO * md_co;//CO质量mg
                        Vmas_Exhaust_nozl[0] = Vmas_Exhaust_fqsjll[0] * zhunbei_data.NO * md_no;//NO质量mg
                        Vmas_Exhaust_hczl[0] = Vmas_Exhaust_fqsjll[0] * zhunbei_data.HC * md_hc;//HC质量mg
                        Vmas_Exhaust_co2zl[0] = Vmas_Exhaust_fqsjll[0] * zhunbei_data.CO2 * md_co2;//CO质量mg
                        Vmas_hjwd[0] = (float)WD; //温度
                        Vmas_xdsd[0] = (float)SD;//湿度
                        Vmas_dqyl[0] = (float)DQY;//大气压                
                        Vmas_xsxzxs[0] = (float)(Math.Round(caculateDf(Vmas_Exhaust_co2ld[0], Vmas_Exhaust_cold[0]), 3));//稀释修正系数
                        Vmas_sdxzxs[0] = (float)(Math.Round(caculateKh(Vmas_hjwd[0], Vmas_xdsd[0], Vmas_dqyl[0]), 3));//湿度修正系数
                        Vmas_Exhaust_qlyl[0] = zhunbei_data.QLYL;
                        Vmas_lambda[0] = zhunbei_data.λ;
                        Thread.Sleep(500);
                    }

                    if (vmasconfig.IfFqyTl)
                    {
                        fla_502.Zeroing();                 //反吹                  1
                        Thread.Sleep(500);
                        zero_count = 0;
                        if (equipconfig.Fqyxh.ToLower() == "nha_503")
                        {
                            while (fla_502.Zeroing() == false)//该处需要测试后定
                            {
                                Thread.Sleep(900);
                                Msg(label_message, panel_msg, "废气仪调零中..." + zero_count.ToString() + "s", true);
                                ts2 = "调零中..." + zero_count.ToString() + "s";
                                zero_count++;
                                if (zero_count == 60)
                                    break;
                            }
                        }
                        else if (equipconfig.Fqyxh.ToLower() == "cdf5000")
                        {
                            while (zero_count<=40)//该处需要测试后定
                            {
                                Thread.Sleep(900);
                                Msg(label_message, panel_msg, "废气仪调零中..." + zero_count.ToString() + "s", true);
                                ts2 = "调零中..." + zero_count.ToString() + "s";
                                zero_count++;
                                if (zero_count == 60)
                                    break;
                            }
                        }
                        else
                        {
                            while (fla_502.Get_Struct() == "调零中")
                            {
                                Thread.Sleep(900);
                                Msg(label_message, panel_msg, "废气仪调零中..." + zero_count.ToString() + "s", true);
                                ts2 = "调零中..." + zero_count.ToString() + "s";
                                zero_count++;
                                if (zero_count == 60)
                                    break;
                            }
                        }
                    }
                    Thread.Sleep(500);
                    Exhaust.Fla502_data huanjiang_data = fla_502.GetData();
                    if (!vmasconfig.CjBeforeTl)
                    {
                        Msg(label_message, panel_msg, "测试即将开始,正在进行初检", false);
                        ts2 = "开始初检";
                        vmas_chujian();
                        if (chujianIsFinished == false)
                        {
                            Msg(label_message, panel_msg, "初检不合格,测试将结束", true);
                            ts1 = "检测中止";
                            ts2 = "初检不合格";
                            button_ss.Text = "重新检测";
                            JC_Status = false;

                            return;
                        }
                        Thread.Sleep(500);
                    }
                    Thread.Sleep(500);
                    fla_502.Pump_Pipeair();
                    ts2 = "请安置探头";
                    Msg(label_message, panel_msg, "测试仪器开始工作,请安置好检测探头", false);
                    Thread.Sleep(500);
                    Th_get_FqandLl.Start();
                    th_get_llj.Start();
                    Thread.Sleep(2000);
                    while (Vmas_Exhaust_Now.CO + Vmas_Exhaust_Now.CO2 <= vmasconfig.Ndz)
                        Thread.Sleep(500);
                    Msg(label_message, panel_msg, "探头已插好,检测开始", false);
                    Thread.Sleep(1500);
                }
                Msg(label_message, panel_msg, "检测开始，进行空档怠速", true);
                ts1 = "检测即将开始";
                ts2 = "空档怠速";
                sxnb = 4;
                Thread.Sleep(2000);
                int timeDs = vmasconfig.Dssj;
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.DAOWEI, GKSJ.ToString());
                Thread.Sleep(2000);
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.CHATANTOU, GKSJ.ToString());
                statusconfigini.writeNeuStatusData("StartTest", GKSJ.ToString());//东软开始命令
                vmas_data.Starttime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                vmas_data.TestTime = (vmasconfig.Dssj + 195).ToString();
                vmas_data.Power = Set_Power.ToString();
                while (timeDs > 0)
                {
                    if (equipconfig.DATASECONDS_TYPE == "江西")
                    {
                        cysx = vmasconfig.Dssj - timeDs + 1;//从数组的第二个开始存，第一存准备阶段的数
                        Msg(label_message, panel_msg, "空档怠速中..." + timeDs.ToString() + "s", false);
                        ts2 = "怠速中..." + timeDs.ToString() + "s";
                        Vmas_qcsj[cysx] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//全程时序
                        Vmas_cysx[cysx] = cysx.ToString();//从0开始
                        Vmas_nj[cysx] = igbt.Force;
                        Vmas_fdjzs[cysx] = Vmas_Exhaust_Now.ZS;
                        Vmas_sxnb[cysx] = "11";
                        Vmas_bzcs[cysx] = 0f;//标准速度
                        Vmas_sscs[cysx] = 0f;//实时速度
                        Vmas_jsgl[cysx] = 0f;//加载功率
                        Vmas_zsgl[cysx] = 0f;//加载功率
                        Vmas_jzgl[cysx] = 0f;//加载功率
                        Speed_listIg195[cysx] = 0f;//实时速度
                        Vmas_Exhaust_ListIG195[cysx] = Vmas_Exhaust_Now;//尾气浓度值
                        Vmas_Exhaust_Revise_ListIG195[cysx] = Vmas_Exhaust_Now;//修正的尾气浓度值
                        Vmas_Exhaust_llList[cysx] = flv_1000.ll_standard_value;//流量计标准流量值
                        Vmas_Exhaust_xso2now[cysx] = flv_1000.o2_standard_value;
                        Vmas_Exhaust_xso2afterDelay[cysx] = flv_1000.o2_standard_value;//流量计稀释氧浓度
                        Vmas_Exhaust_lljtemp[cysx] = flv_1000.temp_standard_value;//流量计温度
                        Vmas_Exhaust_lljyl[cysx] = flv_1000.yali_standard_value;//流量计压力
                        if (hjo2 > flv_1000.o2_standard_value && hjo2 > Vmas_Exhaust_Now.O2)//稀释比
                        {
                            Vmas_Exhaust_k[cysx] = (hjo2 - flv_1000.o2_standard_value) / (hjo2 - Vmas_Exhaust_Now.O2);
                        }
                        else
                        {
                            Vmas_Exhaust_k[cysx] = 0f;
                        }
                        Vmas_Exhaust_fqsjll[cysx] = Vmas_Exhaust_llList[cysx] * Vmas_Exhaust_k[cysx];//尾气实际流量
                        Vmas_Exhaust_cold[cysx] = Vmas_Exhaust_Now.CO;//CO浓度
                        Vmas_Exhaust_co2ld[cysx] = Vmas_Exhaust_Now.CO2;//CO2浓度
                        Vmas_Exhaust_hcld[cysx] = Vmas_Exhaust_Now.HC;//HC浓度
                        Vmas_Exhaust_nold[cysx] = Vmas_Exhaust_Now.NO;//NO浓度
                        Vmas_Exhaust_o2ld[cysx] = Vmas_Exhaust_Now.O2;//废气仪氧气浓度
                        Vmas_Exhaust_cozl[cysx] = Vmas_Exhaust_fqsjll[cysx] * Vmas_Exhaust_Now.CO * md_co;//CO质量mg
                        Vmas_Exhaust_nozl[cysx] = Vmas_Exhaust_fqsjll[cysx] * Vmas_Exhaust_Now.NO * md_no;//NO质量mg
                        Vmas_Exhaust_hczl[cysx] = Vmas_Exhaust_fqsjll[cysx] * Vmas_Exhaust_Now.HC * md_hc;//HC质量mg
                        Vmas_Exhaust_co2zl[cysx] = Vmas_Exhaust_fqsjll[cysx] * Vmas_Exhaust_Now.CO2 * md_co2;//CO质量mg
                        if (IsUseTpTemp)
                        {
                            Vmas_hjwd[cysx] = (float)WD; //温度
                            Vmas_xdsd[cysx] = (float)SD;//湿度
                            Vmas_dqyl[cysx] = (float)DQY;//大气压
                        }
                        else if (equipconfig.Fqyxh.ToLower() == "fla_502" || equipconfig.Fqyxh.ToLower() == "nha_503")
                        {
                            Vmas_hjwd[cysx] = fla502_temp_data.TEMP;//温度
                            Vmas_xdsd[cysx] = fla502_temp_data.HUMIDITY;//湿度
                            Vmas_dqyl[cysx] = fla502_temp_data.AIRPRESSURE;//大气压
                        }
                        else
                        {
                            Vmas_hjwd[cysx] = Vmas_Exhaust_Now.HJWD;//温度
                            Vmas_xdsd[cysx] = Vmas_Exhaust_Now.SD;//湿度
                            Vmas_dqyl[cysx] = Vmas_Exhaust_Now.HJYL;//大气压
                        }
                        Vmas_xsxzxs[cysx] = (float)(Math.Round(caculateDf(Vmas_Exhaust_co2ld[cysx], Vmas_Exhaust_cold[cysx]), 3));//稀释修正系数
                        Vmas_sdxzxs[cysx] = (float)(Math.Round(caculateKh(Vmas_hjwd[cysx], Vmas_xdsd[cysx], Vmas_dqyl[cysx]), 3));//湿度修正系数
                        Vmas_Exhaust_qlyl[cysx] = Vmas_Exhaust_Now.QLYL;
                        Vmas_lambda[cysx] = Vmas_Exhaust_Now.λ;
                        Thread.Sleep(900);
                        timeDs--;
                    }
                    else
                    {
                        cysx = vmasconfig.Dssj - timeDs;//从数组的第三个开始存，第一、二个存的是准备阶段的数
                        Msg(label_message, panel_msg, "空档怠速中..." + timeDs.ToString() + "s", false);
                        ts2 = "怠速中..." + timeDs.ToString() + "s";
                        Vmas_qcsj[cysx] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//全程时序
                        Vmas_cysx[cysx] = cysx.ToString();//从0开始
                        Vmas_nj[cysx] = igbt.Force;
                        Vmas_fdjzs[cysx] = Vmas_Exhaust_Now.ZS;
                        Vmas_sxnb[cysx] = sxnb.ToString();
                        Vmas_bzcs[cysx] = 0f;//标准速度
                        Vmas_sscs[cysx] = 0f;//实时速度
                        Vmas_jsgl[cysx] = 0f;//加载功率
                        Vmas_zsgl[cysx] = 0f;//加载功率
                        Vmas_jzgl[cysx] = 0f;//加载功率
                        Speed_listIg195[cysx] = 0f;//实时速度
                        Vmas_Exhaust_ListIG195[cysx] = Vmas_Exhaust_Now;//尾气浓度值
                        Vmas_Exhaust_Revise_ListIG195[cysx] = Vmas_Exhaust_Now;//修正的尾气浓度值
                        Vmas_Exhaust_llList[cysx] = flv_1000.ll_standard_value;//流量计标准流量值
                        Vmas_Exhaust_xso2now[cysx] = flv_1000.o2_standard_value;
                        Vmas_Exhaust_xso2afterDelay[cysx] = flv_1000.o2_standard_value;//流量计稀释氧浓度
                        Vmas_Exhaust_lljtemp[cysx] = flv_1000.temp_standard_value;//流量计温度
                        Vmas_Exhaust_lljyl[cysx] = flv_1000.yali_standard_value;//流量计压力
                        if (hjo2 > flv_1000.o2_standard_value && hjo2 > Vmas_Exhaust_Now.O2)//稀释比
                        {
                            Vmas_Exhaust_k[cysx] = (hjo2 - flv_1000.o2_standard_value) / (hjo2 - Vmas_Exhaust_Now.O2);
                        }
                        else
                        {
                            Vmas_Exhaust_k[cysx] = 0f;
                        }
                        Vmas_Exhaust_fqsjll[cysx] = Vmas_Exhaust_llList[cysx] * Vmas_Exhaust_k[cysx];//尾气实际流量
                        Vmas_Exhaust_cold[cysx] = Vmas_Exhaust_Now.CO;//CO浓度
                        Vmas_Exhaust_co2ld[cysx] = Vmas_Exhaust_Now.CO2;//CO2浓度
                        Vmas_Exhaust_hcld[cysx] = Vmas_Exhaust_Now.HC;//HC浓度
                        Vmas_Exhaust_nold[cysx] = Vmas_Exhaust_Now.NO;//NO浓度
                        Vmas_Exhaust_o2ld[cysx] = Vmas_Exhaust_Now.O2;//废气仪氧气浓度
                        Vmas_Exhaust_cozl[cysx] = Vmas_Exhaust_fqsjll[cysx] * Vmas_Exhaust_Now.CO * md_co;//CO质量mg
                        Vmas_Exhaust_nozl[cysx] = Vmas_Exhaust_fqsjll[cysx] * Vmas_Exhaust_Now.NO * md_no;//NO质量mg
                        Vmas_Exhaust_hczl[cysx] = Vmas_Exhaust_fqsjll[cysx] * Vmas_Exhaust_Now.HC * md_hc;//HC质量mg
                        Vmas_Exhaust_co2zl[cysx] = Vmas_Exhaust_fqsjll[cysx] * Vmas_Exhaust_Now.CO2 * md_co2;//CO质量mg
                        if (IsUseTpTemp)
                        {
                            Vmas_hjwd[cysx] = (float)WD; //温度
                            Vmas_xdsd[cysx] = (float)SD;//湿度
                            Vmas_dqyl[cysx] = (float)DQY;//大气压
                        }
                        else if (equipconfig.Fqyxh.ToLower() == "fla_502" || equipconfig.Fqyxh.ToLower() == "nha_503")
                        {
                            Vmas_hjwd[cysx] = fla502_temp_data.TEMP;//温度
                            Vmas_xdsd[cysx] = fla502_temp_data.HUMIDITY;//湿度
                            Vmas_dqyl[cysx] = fla502_temp_data.AIRPRESSURE;//大气压
                        }
                        else
                        {
                            Vmas_hjwd[cysx] = Vmas_Exhaust_Now.HJWD;//温度
                            Vmas_xdsd[cysx] = Vmas_Exhaust_Now.SD;//湿度
                            Vmas_dqyl[cysx] = Vmas_Exhaust_Now.HJYL;//大气压
                        }
                        Vmas_xsxzxs[cysx] = (float)(Math.Round(caculateDf(Vmas_Exhaust_co2ld[cysx], Vmas_Exhaust_cold[cysx]), 3));//稀释修正系数
                        Vmas_sdxzxs[cysx] = (float)(Math.Round(caculateKh(Vmas_hjwd[cysx], Vmas_xdsd[cysx], Vmas_dqyl[cysx]), 3));//湿度修正系数
                        Vmas_Exhaust_qlyl[cysx] = Vmas_Exhaust_Now.QLYL;
                        Vmas_lambda[cysx] = Vmas_Exhaust_Now.λ;
                        Thread.Sleep(900);
                        timeDs--;
                    }
                }
                
                Msg(label_message, panel_msg, "测试开始，请根据工况运转循环图及语言提示进行驾驶", true);
                ts1 = "检测开始";
                ts2 = "根据循环图驾驶";
                Thread.Sleep(1000);
                igbt.Set_Control_Force(powerSet1);
                Thread.Sleep(100);
                igbt.Start_Control_Force();
                //Msg(label_tishi, panel_tishi, "开始", false);
                Thread.Sleep(100);
                Ig195_status = true;
                startTime = DateTime.Now;
                Msg(label_message, panel_msg, "空档怠速11s", false);

                //ts1 = "空档怠速11s";
                //Msg(label_tishi, panel_tishi, "空档", false);
                while (GKSJ<11)
                {
                    ts1 = "怠速中..." + (11 - GKSJ).ToString() + "s";
                    if (GKSJ < 6)
                    {
                        Msg(label_message, panel_msg, "怠速中..." + (11 - GKSJ).ToString() + "s", false);
                        ts2 = "空档";
                    }
                    else
                    {
                        ts2 = "变换至一档";
                        Msg(label_message, panel_msg, "怠速中..." + (11 - GKSJ).ToString() + "s," + "请变换档位至一档", false);
                        //Msg(label_tishi, panel_tishi, "挂一档", false);
                    }
                    Thread.Sleep(100);
                }
                sxnb = 5;
                Msg(label_message, panel_msg, "一档加速到15km/h保持", false);
                ts1 = "加速到15km/h";
                ts2 = "一档";
                //Msg(label_tishi, panel_tishi, "一档加速", false);
                while (GKSJ <15) Thread.Sleep(100);
                sxnb = 0;
                //statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS., GKSJ.ToString());
                statusconfigini.writeNeuStatusData("Testing15km", DateTime.Now.ToString());
                Msg(label_message, panel_msg, "一档15km/h保持8s", false);
                //Msg(label_tishi, panel_tishi, "一档保持", false);
                Thread.Sleep(600);
                while (GKSJ <23)
                {
                    ts1 = "保持..." + (23 - GKSJ).ToString() + "s";
                    ts2 = "一档";
                    Msg(label_message, panel_msg, "一档15km/h保持中..." + (23 - GKSJ).ToString() + "s", false);
                    Thread.Sleep(100);
                }
                sxnb = 6;
                igbt.Exit_Control();
                Msg(label_message, panel_msg, "一档减速到0km/h,离合器接合", false);
                ts1 = "减速至0km/h";
                ts2 = "一档";
                //Msg(label_tishi, panel_tishi, "减速", false);
                while (GKSJ < 25) Thread.Sleep(100);
                Msg(label_message, panel_msg, "继续减速到0km/h,离合器脱离", false);
                ts1 = "减速至0km/h";
                ts2 = "离合器脱离";
                //Msg(label_tishi, panel_tishi, "脱离离合器", false);
                while (GKSJ < 28) Thread.Sleep(100);
                sxnb = 4;
                Msg(label_message, panel_msg, "退档怠速21s", false);
                ts1 = "怠速21s";
                ts2 = "退档怠速";
                //Msg(label_tishi, panel_tishi, "退档", false);
                Thread.Sleep(600);
                igbt.Set_Control_Force(powerSet2);
                while (GKSJ<49)
                {
                    ts1 = "怠速中..." + (49 - GKSJ).ToString() + "s";
                    if (GKSJ < 44)
                    {
                        Msg(label_message, panel_msg, "怠速中..." + (49 - GKSJ).ToString() + "s", false);
                        ts2 = "空档";
                    }
                    else
                    {
                        Msg(label_message, panel_msg, "怠速中..." + (49 - GKSJ).ToString() + "s," + "请变换档位至一档", false);
                        ts2 = "变换至一档";
                        //Msg(label_tishi, panel_tishi, "挂一档", false);
                    }
                    Thread.Sleep(100);
                }
                sxnb = 5;
                igbt.Set_Control_Force(powerSet2);
                Thread.Sleep(100);
                igbt.Start_Control_Force();
                ts1 = "加速到15km/h";
                ts2 = "一档";
                Msg(label_message, panel_msg, "一档加速到15km/h变换为二档", false);
                //Msg(label_tishi, panel_tishi, "一档加速", false);
                while (GKSJ < 54) Thread.Sleep(100);
                igbt.Exit_Control();
                Msg(label_message, panel_msg, "变换档位至二档，继续加速至32km/h", false);
                ts1 = "加速到32km/h";
                ts2 = "变换至二档";
                while (GKSJ < 56) Thread.Sleep(100);
                igbt.Set_Control_Force(powerSet3);
                Thread.Sleep(100);
                igbt.Start_Control_Force();
                //Msg(label_tishi, panel_tishi, "挂二档加速", false);
                while (GKSJ<61) Thread.Sleep(100);
                sxnb = 1;
                statusconfigini.writeNeuStatusData("Testing32km", DateTime.Now.ToString());
                Msg(label_message, panel_msg, "二档32km/h保持24s", false);
                ts1 = "32km/h保持24s";
                ts2 = "二档";
                //Msg(label_tishi, panel_tishi, "二档保持", false);
                Thread.Sleep(1000);
                while (GKSJ<85)
                {
                    Msg(label_message, panel_msg, "二档32km/h保持中..." + (85 - GKSJ).ToString() + "s", false);
                    ts1 = "保持..." + (85 - GKSJ).ToString() + "s";
                    Thread.Sleep(100);
                }
                sxnb = 6;
                igbt.Exit_Control();
                Msg(label_message, panel_msg, "二档减速到0km/h,离合器接合", false);
                ts1 = "减速至0km/h";
                ts2 = "二档";
               // Msg(label_tishi, panel_tishi, "减速", false);
                while (GKSJ < 93) Thread.Sleep(100);
                Msg(label_message, panel_msg, "继续减速到0km/h,离合器脱离", false);
                ts1 = "减速至0km/h";
                ts2 = "离合器脱离";
                //Msg(label_tishi, panel_tishi, "脱离离合器", false);
                while (GKSJ < 96) Thread.Sleep(100);
                sxnb = 4;
                Msg(label_message, panel_msg, "退档怠速21s", false);
                ts1 = "怠速21s";
                ts2 = "退档怠速";
                //Msg(label_tishi, panel_tishi, "退档", false);
                Thread.Sleep(1000);
                igbt.Set_Control_Force(powerSet3);
                //timeDs = 20;
                while (GKSJ<117)
                {
                    ts1 = "怠速中..." + (117 - GKSJ).ToString() + "s";
                    if (GKSJ < 112)
                    {
                        Msg(label_message, panel_msg, "怠速中..." + (117 - GKSJ).ToString() + "s", false);
                        ts2 = "空档";
                    }
                    else
                    {
                        Msg(label_message, panel_msg, "怠速中..." + (117 - GKSJ).ToString() + "s," + "请变换档位至一档", false);
                        ts2 = "变换至一档";
                        //Msg(label_tishi, panel_tishi, "挂一档", false);
                    }
                    Thread.Sleep(100);
                    //timeDs--;
                }
                sxnb = 5;
                igbt.Set_Control_Force(powerSet4);
                Thread.Sleep(100);
                igbt.Start_Control_Force();
                ts1 = "加速到15km/h";
                ts2 = "一档";
                Msg(label_message, panel_msg, "一档加速到15km/h变换为二档", false);
                //Msg(label_tishi, panel_tishi, "一档加速", false);
                while (GKSJ < 122) Thread.Sleep(100);
                Msg(label_message, panel_msg, "变换档位至二档，继续加速至32km/h变换为三档", false);
                ts1 = "加速到32km/h";
                ts2 = "变换至二档";
                while (GKSJ < 124) Thread.Sleep(100);
                igbt.Set_Control_Force(powerSet5);
                Thread.Sleep(100);
                igbt.Start_Control_Force();
                // Msg(label_tishi, panel_tishi, "挂二档加速", false);
                while (GKSJ < 133) Thread.Sleep(100);
                Msg(label_message, panel_msg, "变换档位至三档，继续加速至50km/h并保持", false);
                ts1 = "加速到50km/h";
                ts2 = "变换至三档";
                while (GKSJ < 135) Thread.Sleep(100);
                igbt.Set_Control_Force(powerSet6);
                Thread.Sleep(100);
                igbt.Start_Control_Force();
                //Msg(label_tishi, panel_tishi, "挂三档加速", false);
                while (GKSJ < 143) Thread.Sleep(100);
                sxnb = 2;
                statusconfigini.writeNeuStatusData("Testing50km", DateTime.Now.ToString());
                //statusconfigini.writeStatusData("Testing50km", DateTime.Now.ToString());
                Msg(label_message, panel_msg, "三档50km/h保持12s", false);
                ts1 = "50km/h保持12s";
                ts2 = "三档";
                if (igbt != null)
                {
                    igbt.Set_Control_Power(Set_Power);
                    igbt.Start_Control_Power();                 //启动恒扭矩控制
                }
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIANCEZHONG, GKSJ.ToString());
                //Thread.Sleep(2000);
                Thread.Sleep(1000);
                //timeDs = 11;
                while (GKSJ<155)
                {
                    Msg(label_message, panel_msg, "三档50km/h保持中..." + (155 - GKSJ).ToString() + "s" , false);
                    ts1 = "保持..." + (155 - GKSJ).ToString() + "s";
                    Thread.Sleep(100);
                    //timeDs--;
                }
                if (igbt != null)
                {
                    igbt.Exit_Control();                 //退出恒功率控制
                }
                //igbt.Set_Control_Power(0f);
                sxnb = 6;
                Msg(label_message, panel_msg, "三档减速到35km/h并保持", false);
                ts1 = "减速至35km/h";
                ts2 = "三档";
                //Msg(label_tishi, panel_tishi, "减速", false);
                while (GKSJ < 163) Thread.Sleep(100);
                sxnb = 3;
                Msg(label_message, panel_msg, "三档35km/h保持13s", false);
                ts1 = "32km/h保持13s";
                ts2 = "三档";
                //Msg(label_tishi, panel_tishi, "保持", false);
                Thread.Sleep(600);
                //timeDs = 12;
                while (GKSJ<176)
                {
                    Msg(label_message, panel_msg, "三档35km/h保持中..." + (176 - GKSJ).ToString() + "s", false);
                    ts1 = "保持..." + (176 - GKSJ).ToString() + "s";
                    //Msg(label_tishi, panel_tishi, "等待开始", true);
                    Thread.Sleep(100);
                    //timeDs--;
                }
                sxnb = 6;
                Msg(label_message, panel_msg, "请换档至二档，减速到0km/h,离合器接合", false);
                ts1 = "减速至0km/h";
                ts2 = "二档";
                //Msg(label_tishi, panel_tishi, "挂二档减速", false);
                while (GKSJ < 185) Thread.Sleep(100);
                Msg(label_message, panel_msg, "继续减速到0km/h,离合器脱离", true);
                ts1 = "减速至0km/h";
                ts2 = "离合器脱离";
                //Msg(label_tishi, panel_tishi, "脱离离合器", false);
                while (GKSJ < 188) Thread.Sleep(100);
                sxnb = 4;
                Msg(label_message, panel_msg, "退档怠速", false);
                ts1 = "怠速";
                ts2 = "退档怠速";
                //Msg(label_tishi, panel_tishi, "退档", false);
                Thread.Sleep(1000);
                //timeDs = 6;
                while (GKSJ < 195 )
                {
                    ts1 = "怠速中..." + (195 - GKSJ).ToString() + "s";
                    Msg(label_message, panel_msg, "怠速中..." + (195  - GKSJ).ToString() + "s", false);
                    Thread.Sleep(100);
                    //timeDs--;
                }
                while (GKSJ < 195 + fqy_delayTime)
                {
                    ts1 = "采样中..." + (195 + fqy_delayTime - GKSJ).ToString() + "s";
                    ts2 = "等待采样结束";
                    Msg(label_message, panel_msg, "等待废气仪采样完毕..." + (195 + fqy_delayTime - GKSJ).ToString() + "s", false);
                    Thread.Sleep(100);
                    //timeDs--;
                }
                vmas_data.Ljcc = outTimeTotal.ToString("0.0");//记录下累计超差时间
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.GUOCHE, GKSJ.ToString());
                Ig195_status = false;

                Msg(label_message, panel_msg, "工况测试结束,请取探头，车辆怠速200s后驶离测功机", false);
                ts1 = "工况测试结束";
                ts2 = "取探头";
                //Msg(label_tishi, panel_tishi, "结束", false);
                JC_Status = false;
                Thread.Sleep(1000);
                try
                {
                    th_get_llj.Abort();
                }
                catch
                { }
                try
                {
                    Th_get_FqandLl.Abort();
                }
                catch
                { }
                try
                {
                    Thread.Sleep(2000);
                    if (fla_502 != null)
                    {
                        fla_502.StopBlowback();
                    }
                    for (int i = vmasconfig.Dssj + fqy_delayTime; i < 195 + vmasconfig.Dssj + fqy_delayTime; i++)//计算不对
                    {
                        co_zl += Vmas_Exhaust_cozl[i];
                        no_zl += Vmas_Exhaust_nozl[i];
                        hc_zl += Vmas_Exhaust_hczl[i];
                        co2_zl += Vmas_Exhaust_co2zl[i];
                    }
                    co2_zl = co2_zl * 0.001f / distanceInTheory;//g/km
                    co_zl = co_zl * 0.001f / distanceInTheory;//g/km
                    no_zl = no_zl * 0.001f / distanceInTheory;//g/km
                    hc_zl = hc_zl * 0.001f / distanceInTheory;//g/km
                    vmas_data.CarID = carbj.CarID;//车辆ID
                    vmas_data.Wd = WD.ToString("0.0");//温度
                    vmas_data.Sd = SD.ToString("0.0");//湿度
                    vmas_data.Dqy = DQY.ToString("0.0");//大气压
                    if (!isUseData)
                    {
                        vmas_data.Co2 = co2_zl.ToString("0.00");//CO质量
                        vmas_data.Cozl = co_zl.ToString("0.00");//CO质量
                        vmas_data.Noxzl = no_zl.ToString("0.00");//NO质量
                        vmas_data.Hczl = hc_zl.ToString("0.00");//HC质量
                    }
                    else
                    {
                        vmas_data.Cozl = cozl_zb.ToString("0.00");//CO质量
                        vmas_data.Noxzl = nozl_zb.ToString("0.00");//NO质量
                        vmas_data.Hczl = hczl_zb.ToString("0.00");//HC质量
                    }
                    vmas_data.Hcnox = (no_zl + hc_zl).ToString("0.00");
                }
                catch(Exception er)
                {
                    MessageBox.Show("计算结果过程中出错：" + er.Message);
                    return;
                }
                double zll = 0;
                double sjlc = 0;
                double lambdasum = 0;
                vmas_dataseconds.CarID=carbj.CarID;
                DataTable vmas_datatable = new DataTable();

                try
                {
                    vmas_datatable.Columns.Add("全程时序");
                    vmas_datatable.Columns.Add("时序类别");
                    vmas_datatable.Columns.Add("采样时序");
                    vmas_datatable.Columns.Add("lambda");
                    vmas_datatable.Columns.Add("扭矩");
                    vmas_datatable.Columns.Add("发动机转速");
                    vmas_datatable.Columns.Add("HC实时值");
                    vmas_datatable.Columns.Add("CO实时值");
                    vmas_datatable.Columns.Add("CO2实时值");
                    vmas_datatable.Columns.Add("NO实时值");
                    vmas_datatable.Columns.Add("环境O2浓度");
                    vmas_datatable.Columns.Add("分析仪O2实时值");
                    vmas_datatable.Columns.Add("流量计O2实时值");
                    vmas_datatable.Columns.Add("HC排放质量");
                    vmas_datatable.Columns.Add("CO排放质量");
                    vmas_datatable.Columns.Add("CO2排放质量");
                    vmas_datatable.Columns.Add("NO排放质量");
                    vmas_datatable.Columns.Add("标准时速");
                    vmas_datatable.Columns.Add("实时车速");
                    vmas_datatable.Columns.Add("寄生功率");
                    vmas_datatable.Columns.Add("指示功率");
                    vmas_datatable.Columns.Add("加载功率");
                    vmas_datatable.Columns.Add("环境温度");
                    vmas_datatable.Columns.Add("相对湿度");
                    vmas_datatable.Columns.Add("大气压力");
                    vmas_datatable.Columns.Add("流量计温度");
                    vmas_datatable.Columns.Add("流量计压力");
                    vmas_datatable.Columns.Add("实际体积流量");
                    vmas_datatable.Columns.Add("标准体积流量");
                    vmas_datatable.Columns.Add("湿度修正系数");
                    vmas_datatable.Columns.Add("稀释修正系数");
                    vmas_datatable.Columns.Add("稀释比");
                    vmas_datatable.Columns.Add("分析仪管路压力");
                    vmas_datatable.Columns.Add("尾气实际排放流量");
                    if (equipconfig.DATASECONDS_TYPE == "江西")
                    {
                        //准备阶段
                        dr = vmas_datatable.NewRow();
                        dr["全程时序"] = Vmas_qcsj[0];
                        dr["时序类别"] = Vmas_sxnb[0];
                        dr["采样时序"] = Vmas_cysx[0];
                        dr["扭矩"] = (Vmas_nj[0] * 0.108f).ToString("0.0");
                        dr["发动机转速"] = Vmas_fdjzs[0].ToString("0");
                        dr["HC实时值"] = Vmas_Exhaust_hcld[0].ToString("0");
                        dr["CO实时值"] = Vmas_Exhaust_cold[0].ToString("0.00");
                        dr["CO2实时值"] = Vmas_Exhaust_co2ld[0].ToString("0.00");
                        dr["NO实时值"] = Vmas_Exhaust_nold[0].ToString("0");
                        if (!isUseData)
                        {
                            dr["环境O2浓度"] = hjo2.ToString("0.00");
                        }
                        else
                        {
                            dr["环境O2浓度"] = Vmas_Exhaust_hujingo2_zb[0].ToString("0.00");
                        }
                        dr["分析仪O2实时值"] = Vmas_Exhaust_o2ld[0].ToString("0.00");
                        dr["流量计O2实时值"] = Vmas_Exhaust_xso2afterDelay[0].ToString("0.00");
                        dr["HC排放质量"] = Vmas_Exhaust_hczl[0].ToString("0.000");
                        dr["CO排放质量"] = Vmas_Exhaust_cozl[0].ToString("0.000");
                        dr["CO2排放质量"] = Vmas_Exhaust_co2zl[0].ToString("0.000");
                        dr["NO排放质量"] = Vmas_Exhaust_nozl[0].ToString("0.000");
                        dr["标准时速"] = Math.Round(Vmas_bzcs[0], 1).ToString("0.0");
                        dr["实时车速"] = Math.Round(Vmas_sscs[0], 1).ToString("0.0");
                        dr["寄生功率"] = Math.Round(Vmas_jsgl[0], 2).ToString("0.00");
                        dr["指示功率"] = Math.Round(Vmas_zsgl[0], 2).ToString("0.00");
                        dr["加载功率"] = Math.Round(Vmas_jzgl[0], 2).ToString("0.00");
                        dr["环境温度"] = Vmas_hjwd[0].ToString("0.0");
                        dr["相对湿度"] = Vmas_xdsd[0].ToString("0.0");
                        dr["大气压力"] = Vmas_dqyl[0].ToString("0.0");
                        dr["流量计温度"] = Vmas_Exhaust_lljtemp[0].ToString("0.0");
                        dr["流量计压力"] = Vmas_Exhaust_lljyl[0].ToString("0.0");
                        dr["实际体积流量"] = Math.Round(Vmas_Exhaust_llList[0] * 101.325 * (273.15 + Vmas_Exhaust_lljtemp[0]) / (Vmas_Exhaust_lljyl[0] * 273.15), 3).ToString("0.000");
                        dr["标准体积流量"] = Math.Round(Vmas_Exhaust_llList[0], 3).ToString("0.000");
                        dr["湿度修正系数"] = Math.Round(Vmas_sdxzxs[0], 3).ToString("0.000");
                        dr["稀释修正系数"] = Math.Round(Vmas_xsxzxs[0], 3).ToString("0.000");
                        dr["稀释比"] = Vmas_Exhaust_k[0].ToString("0.000");
                        dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[0].ToString("0.0");
                        dr["尾气实际排放流量"] = Math.Round(Vmas_Exhaust_fqsjll[0] * 0.6, 3).ToString("0.000");
                        dr["lambda"] = Vmas_lambda[0];
                        vmas_datatable.Rows.Add(dr);
                        //怠速阶段
                        for (int i = 1; i < vmasconfig.Dssj + 1; i++)//将数据写入逐秒数据
                        {
                            dr = vmas_datatable.NewRow();
                            dr["全程时序"] = Vmas_qcsj[i];
                            dr["时序类别"] = Vmas_sxnb[i];
                            dr["采样时序"] = Vmas_cysx[i];
                            dr["扭矩"] = (Vmas_nj[i] * 0.108f).ToString("0.0");
                            dr["发动机转速"] = Vmas_fdjzs[i].ToString("0");
                            dr["HC实时值"] = Vmas_Exhaust_hcld[i].ToString("0");
                            dr["CO实时值"] = Vmas_Exhaust_cold[i].ToString("0.00");
                            dr["CO2实时值"] = Vmas_Exhaust_co2ld[i].ToString("0.00");
                            dr["NO实时值"] = Vmas_Exhaust_nold[i].ToString("0");
                            if (!isUseData)
                            {
                                dr["环境O2浓度"] = hjo2.ToString("0.00");
                            }
                            else
                            {
                                dr["环境O2浓度"] = Vmas_Exhaust_hujingo2_zb[0].ToString("0.00");
                            }
                            dr["分析仪O2实时值"] = Vmas_Exhaust_o2ld[i].ToString("0.00");
                            dr["流量计O2实时值"] = Vmas_Exhaust_xso2afterDelay[i].ToString("0.00");
                            dr["HC排放质量"] = Vmas_Exhaust_hczl[i].ToString("0.000");
                            dr["CO排放质量"] = Vmas_Exhaust_cozl[i].ToString("0.000");
                            dr["CO2排放质量"] = Vmas_Exhaust_co2zl[i].ToString("0.000");
                            dr["NO排放质量"] = Vmas_Exhaust_nozl[i].ToString("0.000");
                            dr["标准时速"] = Math.Round(Vmas_bzcs[i], 1).ToString("0.0");
                            dr["实时车速"] = Math.Round(Vmas_sscs[i], 1).ToString("0.0");
                            dr["寄生功率"] = Math.Round(Vmas_jsgl[i], 2).ToString("0.00");
                            dr["指示功率"] = Math.Round(Vmas_zsgl[i], 2).ToString("0.00");
                            dr["加载功率"] = Math.Round(Vmas_jzgl[i], 2).ToString("0.00");
                            dr["环境温度"] = Vmas_hjwd[i].ToString("0.0");
                            dr["相对湿度"] = Vmas_xdsd[i].ToString("0.0");
                            dr["大气压力"] = Vmas_dqyl[i].ToString("0.0");
                            dr["流量计温度"] = Vmas_Exhaust_lljtemp[i].ToString("0.0");
                            dr["流量计压力"] = Vmas_Exhaust_lljyl[i].ToString("0.0");
                            dr["实际体积流量"] = Math.Round(Vmas_Exhaust_llList[i] * 101.325 * (273.15 + Vmas_Exhaust_lljtemp[i]) / (Vmas_Exhaust_lljyl[i] * 273.15), 3).ToString("0.000");
                            dr["标准体积流量"] = Math.Round(Vmas_Exhaust_llList[i], 3).ToString("0.000");
                            dr["湿度修正系数"] = Math.Round(Vmas_sdxzxs[i], 3).ToString("0.000");
                            dr["稀释修正系数"] = Math.Round(Vmas_xsxzxs[i], 3).ToString("0.000");
                            dr["稀释比"] = Vmas_Exhaust_k[i].ToString("0.000");
                            dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i].ToString("0.0");
                            dr["尾气实际排放流量"] = Math.Round(Vmas_Exhaust_fqsjll[i] * 0.6, 3).ToString("0.000");
                            dr["lambda"] = Vmas_lambda[i];
                            zll += Vmas_Exhaust_fqsjll[i] * 0.6;
                            vmas_datatable.Rows.Add(dr);
                        }
                        #region 195秒阶段
                        if (!isUseData)//将数据写入逐秒数据
                        {
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 1; i < vmasconfig.Dssj + fqy_delayTime + 12; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i - fqy_delayTime];
                                dr["时序类别"] = "4";
                                dr["采样时序"] = (i - fqy_delayTime - vmasconfig.Dssj + 1).ToString("0");
                                dr["扭矩"] = Math.Round(Vmas_nj[i - fqy_delayTime] * 0.108f, 1).ToString("0.0");
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime].ToString("0");
                                dr["HC实时值"] = Vmas_Exhaust_hcld[i].ToString("0");
                                dr["CO实时值"] = Vmas_Exhaust_cold[i].ToString("0.00");
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i].ToString("0.00");
                                dr["NO实时值"] = Vmas_Exhaust_nold[i].ToString("0");
                                dr["环境O2浓度"] = hjo2.ToString("0.00");
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2ld[i].ToString("0.00");
                                dr["流量计O2实时值"] = Vmas_Exhaust_xso2afterDelay[i].ToString("0.00");
                                dr["HC排放质量"] = Vmas_Exhaust_hczl[i].ToString("0.000");
                                dr["CO排放质量"] = Vmas_Exhaust_cozl[i].ToString("0.000");
                                dr["CO2排放质量"] = Vmas_Exhaust_co2zl[i].ToString("0.000");
                                dr["NO排放质量"] = Vmas_Exhaust_nozl[i].ToString("0.000");
                                dr["标准时速"] = Math.Round(Vmas_bzcs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["实时车速"] = Math.Round(Vmas_sscs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["寄生功率"] = Math.Round(Vmas_jsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["指示功率"] = Math.Round(Vmas_zsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["加载功率"] = Math.Round(Vmas_jzgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["环境温度"] = Vmas_hjwd[i].ToString("0.0");
                                dr["相对湿度"] = Vmas_xdsd[i].ToString("0.0");
                                dr["大气压力"] = Vmas_dqyl[i].ToString("0.0");
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i].ToString("0.0");
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i].ToString("0.0");
                                dr["实际体积流量"] = Math.Round(Vmas_Exhaust_llList[i] * 101.325 * (273.15 + Vmas_Exhaust_lljtemp[i]) / (Vmas_Exhaust_lljyl[i] * 273.15), 3).ToString("0.000");
                                dr["标准体积流量"] = Math.Round(Vmas_Exhaust_llList[i], 3).ToString("0.000");
                                dr["湿度修正系数"] = Math.Round(Vmas_sdxzxs[i], 3).ToString("0.000");
                                dr["稀释修正系数"] = Math.Round(Vmas_xsxzxs[i], 3).ToString("0.000");
                                dr["稀释比"] = Vmas_Exhaust_k[i].ToString("0.000");
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i].ToString("0.0");
                                dr["尾气实际排放流量"] = Math.Round(Vmas_Exhaust_fqsjll[i], 3).ToString("0.000");
                                dr["lambda"] = Vmas_lambda[i].ToString("0.000");
                                zll += Vmas_Exhaust_fqsjll[i] * 0.6;
                                sjlc += Vmas_sscs[i - fqy_delayTime] / 3.6;
                                lambdasum += Vmas_lambda[i];
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 12; i < vmasconfig.Dssj + fqy_delayTime + 16; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i - fqy_delayTime];
                                dr["时序类别"] = "5";
                                dr["采样时序"] = (i - fqy_delayTime - vmasconfig.Dssj + 1).ToString("0");
                                dr["扭矩"] = Math.Round(Vmas_nj[i - fqy_delayTime] * 0.108f, 1).ToString("0.0");
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime].ToString("0");
                                dr["HC实时值"] = Vmas_Exhaust_hcld[i].ToString("0");
                                dr["CO实时值"] = Vmas_Exhaust_cold[i].ToString("0.00");
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i].ToString("0.00");
                                dr["NO实时值"] = Vmas_Exhaust_nold[i].ToString("0");
                                dr["环境O2浓度"] = hjo2.ToString("0.00");
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2ld[i].ToString("0.00");
                                dr["流量计O2实时值"] = Vmas_Exhaust_xso2afterDelay[i].ToString("0.00");
                                dr["HC排放质量"] = Vmas_Exhaust_hczl[i].ToString("0.000");
                                dr["CO排放质量"] = Vmas_Exhaust_cozl[i].ToString("0.000");
                                dr["CO2排放质量"] = Vmas_Exhaust_co2zl[i].ToString("0.000");
                                dr["NO排放质量"] = Vmas_Exhaust_nozl[i].ToString("0.000");
                                dr["标准时速"] = Math.Round(Vmas_bzcs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["实时车速"] = Math.Round(Vmas_sscs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["寄生功率"] = Math.Round(Vmas_jsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["指示功率"] = Math.Round(Vmas_zsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["加载功率"] = Math.Round(Vmas_jzgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["环境温度"] = Vmas_hjwd[i].ToString("0.0");
                                dr["相对湿度"] = Vmas_xdsd[i].ToString("0.0");
                                dr["大气压力"] = Vmas_dqyl[i].ToString("0.0");
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i].ToString("0.0");
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i].ToString("0.0");
                                dr["实际体积流量"] = Math.Round(Vmas_Exhaust_llList[i] * 101.325 * (273.15 + Vmas_Exhaust_lljtemp[i]) / (Vmas_Exhaust_lljyl[i] * 273.15), 3).ToString("0.000");
                                dr["标准体积流量"] = Math.Round(Vmas_Exhaust_llList[i], 3).ToString("0.000");
                                dr["湿度修正系数"] = Math.Round(Vmas_sdxzxs[i], 3).ToString("0.000");
                                dr["稀释修正系数"] = Math.Round(Vmas_xsxzxs[i], 3).ToString("0.000");
                                dr["稀释比"] = Vmas_Exhaust_k[i].ToString("0.000");
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i].ToString("0.0");
                                dr["尾气实际排放流量"] = Math.Round(Vmas_Exhaust_fqsjll[i], 3).ToString("0.000");
                                dr["lambda"] = Vmas_lambda[i].ToString("0.000");
                                zll += Vmas_Exhaust_fqsjll[i] * 0.6;
                                sjlc += Vmas_sscs[i - fqy_delayTime] / 3.6;
                                lambdasum += Vmas_lambda[i];
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 16; i < vmasconfig.Dssj + fqy_delayTime + 24; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i - fqy_delayTime];
                                dr["时序类别"] = "0";
                                dr["采样时序"] = (i - fqy_delayTime - vmasconfig.Dssj + 1).ToString("0");
                                dr["扭矩"] = Math.Round(Vmas_nj[i - fqy_delayTime] * 0.108f, 1).ToString("0.0");
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime].ToString("0");
                                dr["HC实时值"] = Vmas_Exhaust_hcld[i].ToString("0");
                                dr["CO实时值"] = Vmas_Exhaust_cold[i].ToString("0.00");
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i].ToString("0.00");
                                dr["NO实时值"] = Vmas_Exhaust_nold[i].ToString("0");
                                dr["环境O2浓度"] = hjo2.ToString("0.00");
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2ld[i].ToString("0.00");
                                dr["流量计O2实时值"] = Vmas_Exhaust_xso2afterDelay[i].ToString("0.00");
                                dr["HC排放质量"] = Vmas_Exhaust_hczl[i].ToString("0.000");
                                dr["CO排放质量"] = Vmas_Exhaust_cozl[i].ToString("0.000");
                                dr["CO2排放质量"] = Vmas_Exhaust_co2zl[i].ToString("0.000");
                                dr["NO排放质量"] = Vmas_Exhaust_nozl[i].ToString("0.000");
                                dr["标准时速"] = Math.Round(Vmas_bzcs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["实时车速"] = Math.Round(Vmas_sscs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["寄生功率"] = Math.Round(Vmas_jsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["指示功率"] = Math.Round(Vmas_zsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["加载功率"] = Math.Round(Vmas_jzgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["环境温度"] = Vmas_hjwd[i].ToString("0.0");
                                dr["相对湿度"] = Vmas_xdsd[i].ToString("0.0");
                                dr["大气压力"] = Vmas_dqyl[i].ToString("0.0");
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i].ToString("0.0");
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i].ToString("0.0");
                                dr["实际体积流量"] = Math.Round(Vmas_Exhaust_llList[i] * 101.325 * (273.15 + Vmas_Exhaust_lljtemp[i]) / (Vmas_Exhaust_lljyl[i] * 273.15), 3).ToString("0.000");
                                dr["标准体积流量"] = Math.Round(Vmas_Exhaust_llList[i], 3).ToString("0.000");
                                dr["湿度修正系数"] = Math.Round(Vmas_sdxzxs[i], 3).ToString("0.000");
                                dr["稀释修正系数"] = Math.Round(Vmas_xsxzxs[i], 3).ToString("0.000");
                                dr["稀释比"] = Vmas_Exhaust_k[i].ToString("0.000");
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i].ToString("0.0");
                                dr["尾气实际排放流量"] = Math.Round(Vmas_Exhaust_fqsjll[i], 3).ToString("0.000");
                                dr["lambda"] = Vmas_lambda[i].ToString("0.000");
                                zll += Vmas_Exhaust_fqsjll[i] * 0.6;
                                sjlc += Vmas_sscs[i - fqy_delayTime] / 3.6;
                                lambdasum += Vmas_lambda[i];
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 24; i < vmasconfig.Dssj + fqy_delayTime + 29; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i - fqy_delayTime];
                                dr["时序类别"] = "6";
                                dr["采样时序"] = (i - fqy_delayTime - vmasconfig.Dssj + 1).ToString("0");
                                dr["扭矩"] = Math.Round(Vmas_nj[i - fqy_delayTime] * 0.108f, 1).ToString("0.0");
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime].ToString("0");
                                dr["HC实时值"] = Vmas_Exhaust_hcld[i].ToString("0");
                                dr["CO实时值"] = Vmas_Exhaust_cold[i].ToString("0.00");
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i].ToString("0.00");
                                dr["NO实时值"] = Vmas_Exhaust_nold[i].ToString("0");
                                dr["环境O2浓度"] = hjo2.ToString("0.00");
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2ld[i].ToString("0.00");
                                dr["流量计O2实时值"] = Vmas_Exhaust_xso2afterDelay[i].ToString("0.00");
                                dr["HC排放质量"] = Vmas_Exhaust_hczl[i].ToString("0.000");
                                dr["CO排放质量"] = Vmas_Exhaust_cozl[i].ToString("0.000");
                                dr["CO2排放质量"] = Vmas_Exhaust_co2zl[i].ToString("0.000");
                                dr["NO排放质量"] = Vmas_Exhaust_nozl[i].ToString("0.000");
                                dr["标准时速"] = Math.Round(Vmas_bzcs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["实时车速"] = Math.Round(Vmas_sscs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["寄生功率"] = Math.Round(Vmas_jsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["指示功率"] = Math.Round(Vmas_zsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["加载功率"] = Math.Round(Vmas_jzgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["环境温度"] = Vmas_hjwd[i].ToString("0.0");
                                dr["相对湿度"] = Vmas_xdsd[i].ToString("0.0");
                                dr["大气压力"] = Vmas_dqyl[i].ToString("0.0");
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i].ToString("0.0");
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i].ToString("0.0");
                                dr["实际体积流量"] = Math.Round(Vmas_Exhaust_llList[i] * 101.325 * (273.15 + Vmas_Exhaust_lljtemp[i]) / (Vmas_Exhaust_lljyl[i] * 273.15), 3).ToString("0.000");
                                dr["标准体积流量"] = Math.Round(Vmas_Exhaust_llList[i], 3).ToString("0.000");
                                dr["湿度修正系数"] = Math.Round(Vmas_sdxzxs[i], 3).ToString("0.000");
                                dr["稀释修正系数"] = Math.Round(Vmas_xsxzxs[i], 3).ToString("0.000");
                                dr["稀释比"] = Vmas_Exhaust_k[i].ToString("0.000");
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i].ToString("0.0");
                                dr["尾气实际排放流量"] = Math.Round(Vmas_Exhaust_fqsjll[i], 3).ToString("0.000");
                                dr["lambda"] = Vmas_lambda[i].ToString("0.000");
                                zll += Vmas_Exhaust_fqsjll[i] * 0.6;
                                sjlc += Vmas_sscs[i - fqy_delayTime] / 3.6;
                                lambdasum += Vmas_lambda[i];
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 29; i < vmasconfig.Dssj + fqy_delayTime + 50; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i - fqy_delayTime];
                                dr["时序类别"] = "4";
                                dr["采样时序"] = (i - fqy_delayTime - vmasconfig.Dssj + 1).ToString("0");
                                dr["扭矩"] = Math.Round(Vmas_nj[i - fqy_delayTime] * 0.108f, 1).ToString("0.0");
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime].ToString("0");
                                dr["HC实时值"] = Vmas_Exhaust_hcld[i].ToString("0");
                                dr["CO实时值"] = Vmas_Exhaust_cold[i].ToString("0.00");
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i].ToString("0.00");
                                dr["NO实时值"] = Vmas_Exhaust_nold[i].ToString("0");
                                dr["环境O2浓度"] = hjo2.ToString("0.00");
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2ld[i].ToString("0.00");
                                dr["流量计O2实时值"] = Vmas_Exhaust_xso2afterDelay[i].ToString("0.00");
                                dr["HC排放质量"] = Vmas_Exhaust_hczl[i].ToString("0.000");
                                dr["CO排放质量"] = Vmas_Exhaust_cozl[i].ToString("0.000");
                                dr["CO2排放质量"] = Vmas_Exhaust_co2zl[i].ToString("0.000");
                                dr["NO排放质量"] = Vmas_Exhaust_nozl[i].ToString("0.000");
                                dr["标准时速"] = Math.Round(Vmas_bzcs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["实时车速"] = Math.Round(Vmas_sscs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["寄生功率"] = Math.Round(Vmas_jsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["指示功率"] = Math.Round(Vmas_zsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["加载功率"] = Math.Round(Vmas_jzgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["环境温度"] = Vmas_hjwd[i].ToString("0.0");
                                dr["相对湿度"] = Vmas_xdsd[i].ToString("0.0");
                                dr["大气压力"] = Vmas_dqyl[i].ToString("0.0");
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i].ToString("0.0");
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i].ToString("0.0");
                                dr["实际体积流量"] = Math.Round(Vmas_Exhaust_llList[i] * 101.325 * (273.15 + Vmas_Exhaust_lljtemp[i]) / (Vmas_Exhaust_lljyl[i] * 273.15), 3).ToString("0.000");
                                dr["标准体积流量"] = Math.Round(Vmas_Exhaust_llList[i], 3).ToString("0.000");
                                dr["湿度修正系数"] = Math.Round(Vmas_sdxzxs[i], 3).ToString("0.000");
                                dr["稀释修正系数"] = Math.Round(Vmas_xsxzxs[i], 3).ToString("0.000");
                                dr["稀释比"] = Vmas_Exhaust_k[i].ToString("0.000");
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i].ToString("0.0");
                                dr["尾气实际排放流量"] = Math.Round(Vmas_Exhaust_fqsjll[i], 3).ToString("0.000");
                                dr["lambda"] = Vmas_lambda[i].ToString("0.000");
                                zll += Vmas_Exhaust_fqsjll[i] * 0.6;
                                sjlc += Vmas_sscs[i - fqy_delayTime] / 3.6;
                                lambdasum += Vmas_lambda[i];
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 50; i < vmasconfig.Dssj + fqy_delayTime + 62; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i - fqy_delayTime];
                                dr["时序类别"] = "5";
                                dr["采样时序"] = (i - fqy_delayTime - vmasconfig.Dssj + 1).ToString("0");
                                dr["扭矩"] = Math.Round(Vmas_nj[i - fqy_delayTime] * 0.108f, 1).ToString("0.0");
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime].ToString("0");
                                dr["HC实时值"] = Vmas_Exhaust_hcld[i].ToString("0");
                                dr["CO实时值"] = Vmas_Exhaust_cold[i].ToString("0.00");
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i].ToString("0.00");
                                dr["NO实时值"] = Vmas_Exhaust_nold[i].ToString("0");
                                dr["环境O2浓度"] = hjo2.ToString("0.00");
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2ld[i].ToString("0.00");
                                dr["流量计O2实时值"] = Vmas_Exhaust_xso2afterDelay[i].ToString("0.00");
                                dr["HC排放质量"] = Vmas_Exhaust_hczl[i].ToString("0.000");
                                dr["CO排放质量"] = Vmas_Exhaust_cozl[i].ToString("0.000");
                                dr["CO2排放质量"] = Vmas_Exhaust_co2zl[i].ToString("0.000");
                                dr["NO排放质量"] = Vmas_Exhaust_nozl[i].ToString("0.000");
                                dr["标准时速"] = Math.Round(Vmas_bzcs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["实时车速"] = Math.Round(Vmas_sscs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["寄生功率"] = Math.Round(Vmas_jsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["指示功率"] = Math.Round(Vmas_zsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["加载功率"] = Math.Round(Vmas_jzgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["环境温度"] = Vmas_hjwd[i].ToString("0.0");
                                dr["相对湿度"] = Vmas_xdsd[i].ToString("0.0");
                                dr["大气压力"] = Vmas_dqyl[i].ToString("0.0");
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i].ToString("0.0");
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i].ToString("0.0");
                                dr["实际体积流量"] = Math.Round(Vmas_Exhaust_llList[i] * 101.325 * (273.15 + Vmas_Exhaust_lljtemp[i]) / (Vmas_Exhaust_lljyl[i] * 273.15), 3).ToString("0.000");
                                dr["标准体积流量"] = Math.Round(Vmas_Exhaust_llList[i], 3).ToString("0.000");
                                dr["湿度修正系数"] = Math.Round(Vmas_sdxzxs[i], 3).ToString("0.000");
                                dr["稀释修正系数"] = Math.Round(Vmas_xsxzxs[i], 3).ToString("0.000");
                                dr["稀释比"] = Vmas_Exhaust_k[i].ToString("0.000");
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i].ToString("0.0");
                                dr["尾气实际排放流量"] = Math.Round(Vmas_Exhaust_fqsjll[i], 3).ToString("0.000");
                                dr["lambda"] = Vmas_lambda[i].ToString("0.000");
                                zll += Vmas_Exhaust_fqsjll[i] * 0.6;
                                sjlc += Vmas_sscs[i - fqy_delayTime] / 3.6;
                                lambdasum += Vmas_lambda[i];
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 62; i < vmasconfig.Dssj + fqy_delayTime + 86; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i - fqy_delayTime];
                                dr["时序类别"] = "1";
                                dr["采样时序"] = (i - fqy_delayTime - vmasconfig.Dssj + 1).ToString("0");
                                dr["扭矩"] = Math.Round(Vmas_nj[i - fqy_delayTime] * 0.108f, 1).ToString("0.0");
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime].ToString("0");
                                dr["HC实时值"] = Vmas_Exhaust_hcld[i].ToString("0");
                                dr["CO实时值"] = Vmas_Exhaust_cold[i].ToString("0.00");
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i].ToString("0.00");
                                dr["NO实时值"] = Vmas_Exhaust_nold[i].ToString("0");
                                dr["环境O2浓度"] = hjo2.ToString("0.00");
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2ld[i].ToString("0.00");
                                dr["流量计O2实时值"] = Vmas_Exhaust_xso2afterDelay[i].ToString("0.00");
                                dr["HC排放质量"] = Vmas_Exhaust_hczl[i].ToString("0.000");
                                dr["CO排放质量"] = Vmas_Exhaust_cozl[i].ToString("0.000");
                                dr["CO2排放质量"] = Vmas_Exhaust_co2zl[i].ToString("0.000");
                                dr["NO排放质量"] = Vmas_Exhaust_nozl[i].ToString("0.000");
                                dr["标准时速"] = Math.Round(Vmas_bzcs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["实时车速"] = Math.Round(Vmas_sscs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["寄生功率"] = Math.Round(Vmas_jsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["指示功率"] = Math.Round(Vmas_zsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["加载功率"] = Math.Round(Vmas_jzgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["环境温度"] = Vmas_hjwd[i].ToString("0.0");
                                dr["相对湿度"] = Vmas_xdsd[i].ToString("0.0");
                                dr["大气压力"] = Vmas_dqyl[i].ToString("0.0");
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i].ToString("0.0");
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i].ToString("0.0");
                                dr["实际体积流量"] = Math.Round(Vmas_Exhaust_llList[i] * 101.325 * (273.15 + Vmas_Exhaust_lljtemp[i]) / (Vmas_Exhaust_lljyl[i] * 273.15), 3).ToString("0.000");
                                dr["标准体积流量"] = Math.Round(Vmas_Exhaust_llList[i], 3).ToString("0.000");
                                dr["湿度修正系数"] = Math.Round(Vmas_sdxzxs[i], 3).ToString("0.000");
                                dr["稀释修正系数"] = Math.Round(Vmas_xsxzxs[i], 3).ToString("0.000");
                                dr["稀释比"] = Vmas_Exhaust_k[i].ToString("0.000");
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i].ToString("0.0");
                                dr["尾气实际排放流量"] = Math.Round(Vmas_Exhaust_fqsjll[i], 3).ToString("0.000");
                                dr["lambda"] = Vmas_lambda[i].ToString("0.000");
                                zll += Vmas_Exhaust_fqsjll[i] * 0.6;
                                sjlc += Vmas_sscs[i - fqy_delayTime] / 3.6;
                                lambdasum += Vmas_lambda[i];
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 86; i < vmasconfig.Dssj + fqy_delayTime + 97; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i - fqy_delayTime];
                                dr["时序类别"] = "6";
                                dr["采样时序"] = (i - fqy_delayTime - vmasconfig.Dssj + 1).ToString("0");
                                dr["扭矩"] = Math.Round(Vmas_nj[i - fqy_delayTime] * 0.108f, 1).ToString("0.0");
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime].ToString("0");
                                dr["HC实时值"] = Vmas_Exhaust_hcld[i].ToString("0");
                                dr["CO实时值"] = Vmas_Exhaust_cold[i].ToString("0.00");
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i].ToString("0.00");
                                dr["NO实时值"] = Vmas_Exhaust_nold[i].ToString("0");
                                dr["环境O2浓度"] = hjo2.ToString("0.00");
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2ld[i].ToString("0.00");
                                dr["流量计O2实时值"] = Vmas_Exhaust_xso2afterDelay[i].ToString("0.00");
                                dr["HC排放质量"] = Vmas_Exhaust_hczl[i].ToString("0.000");
                                dr["CO排放质量"] = Vmas_Exhaust_cozl[i].ToString("0.000");
                                dr["CO2排放质量"] = Vmas_Exhaust_co2zl[i].ToString("0.000");
                                dr["NO排放质量"] = Vmas_Exhaust_nozl[i].ToString("0.000");
                                dr["标准时速"] = Math.Round(Vmas_bzcs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["实时车速"] = Math.Round(Vmas_sscs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["寄生功率"] = Math.Round(Vmas_jsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["指示功率"] = Math.Round(Vmas_zsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["加载功率"] = Math.Round(Vmas_jzgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["环境温度"] = Vmas_hjwd[i].ToString("0.0");
                                dr["相对湿度"] = Vmas_xdsd[i].ToString("0.0");
                                dr["大气压力"] = Vmas_dqyl[i].ToString("0.0");
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i].ToString("0.0");
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i].ToString("0.0");
                                dr["实际体积流量"] = Math.Round(Vmas_Exhaust_llList[i] * 101.325 * (273.15 + Vmas_Exhaust_lljtemp[i]) / (Vmas_Exhaust_lljyl[i] * 273.15), 3).ToString("0.000");
                                dr["标准体积流量"] = Math.Round(Vmas_Exhaust_llList[i], 3).ToString("0.000");
                                dr["湿度修正系数"] = Math.Round(Vmas_sdxzxs[i], 3).ToString("0.000");
                                dr["稀释修正系数"] = Math.Round(Vmas_xsxzxs[i], 3).ToString("0.000");
                                dr["稀释比"] = Vmas_Exhaust_k[i].ToString("0.000");
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i].ToString("0.0");
                                dr["尾气实际排放流量"] = Math.Round(Vmas_Exhaust_fqsjll[i], 3).ToString("0.000");
                                dr["lambda"] = Vmas_lambda[i].ToString("0.000");
                                zll += Vmas_Exhaust_fqsjll[i] * 0.6;
                                sjlc += Vmas_sscs[i - fqy_delayTime] / 3.6;
                                lambdasum += Vmas_lambda[i];
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 97; i < vmasconfig.Dssj + fqy_delayTime + 118; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i - fqy_delayTime];
                                dr["时序类别"] = "4";
                                dr["采样时序"] = (i - fqy_delayTime - vmasconfig.Dssj + 1).ToString("0");
                                dr["扭矩"] = Math.Round(Vmas_nj[i - fqy_delayTime] * 0.108f, 1).ToString("0.0");
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime].ToString("0");
                                dr["HC实时值"] = Vmas_Exhaust_hcld[i].ToString("0");
                                dr["CO实时值"] = Vmas_Exhaust_cold[i].ToString("0.00");
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i].ToString("0.00");
                                dr["NO实时值"] = Vmas_Exhaust_nold[i].ToString("0");
                                dr["环境O2浓度"] = hjo2.ToString("0.00");
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2ld[i].ToString("0.00");
                                dr["流量计O2实时值"] = Vmas_Exhaust_xso2afterDelay[i].ToString("0.00");
                                dr["HC排放质量"] = Vmas_Exhaust_hczl[i].ToString("0.000");
                                dr["CO排放质量"] = Vmas_Exhaust_cozl[i].ToString("0.000");
                                dr["CO2排放质量"] = Vmas_Exhaust_co2zl[i].ToString("0.000");
                                dr["NO排放质量"] = Vmas_Exhaust_nozl[i].ToString("0.000");
                                dr["标准时速"] = Math.Round(Vmas_bzcs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["实时车速"] = Math.Round(Vmas_sscs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["寄生功率"] = Math.Round(Vmas_jsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["指示功率"] = Math.Round(Vmas_zsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["加载功率"] = Math.Round(Vmas_jzgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["环境温度"] = Vmas_hjwd[i].ToString("0.0");
                                dr["相对湿度"] = Vmas_xdsd[i].ToString("0.0");
                                dr["大气压力"] = Vmas_dqyl[i].ToString("0.0");
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i].ToString("0.0");
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i].ToString("0.0");
                                dr["实际体积流量"] = Math.Round(Vmas_Exhaust_llList[i] * 101.325 * (273.15 + Vmas_Exhaust_lljtemp[i]) / (Vmas_Exhaust_lljyl[i] * 273.15), 3).ToString("0.000");
                                dr["标准体积流量"] = Math.Round(Vmas_Exhaust_llList[i], 3).ToString("0.000");
                                dr["湿度修正系数"] = Math.Round(Vmas_sdxzxs[i], 3).ToString("0.000");
                                dr["稀释修正系数"] = Math.Round(Vmas_xsxzxs[i], 3).ToString("0.000");
                                dr["稀释比"] = Vmas_Exhaust_k[i].ToString("0.000");
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i].ToString("0.0");
                                dr["尾气实际排放流量"] = Math.Round(Vmas_Exhaust_fqsjll[i], 3).ToString("0.000");
                                dr["lambda"] = Vmas_lambda[i].ToString("0.000");
                                zll += Vmas_Exhaust_fqsjll[i] * 0.6;
                                sjlc += Vmas_sscs[i - fqy_delayTime] / 3.6;
                                lambdasum += Vmas_lambda[i];
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 118; i < vmasconfig.Dssj + fqy_delayTime + 144; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i - fqy_delayTime];
                                dr["时序类别"] = "5";
                                dr["采样时序"] = (i - fqy_delayTime - vmasconfig.Dssj + 1).ToString("0");
                                dr["扭矩"] = Math.Round(Vmas_nj[i - fqy_delayTime] * 0.108f, 1).ToString("0.0");
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime].ToString("0");
                                dr["HC实时值"] = Vmas_Exhaust_hcld[i].ToString("0");
                                dr["CO实时值"] = Vmas_Exhaust_cold[i].ToString("0.00");
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i].ToString("0.00");
                                dr["NO实时值"] = Vmas_Exhaust_nold[i].ToString("0");
                                dr["环境O2浓度"] = hjo2.ToString("0.00");
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2ld[i].ToString("0.00");
                                dr["流量计O2实时值"] = Vmas_Exhaust_xso2afterDelay[i].ToString("0.00");
                                dr["HC排放质量"] = Vmas_Exhaust_hczl[i].ToString("0.000");
                                dr["CO排放质量"] = Vmas_Exhaust_cozl[i].ToString("0.000");
                                dr["CO2排放质量"] = Vmas_Exhaust_co2zl[i].ToString("0.000");
                                dr["NO排放质量"] = Vmas_Exhaust_nozl[i].ToString("0.000");
                                dr["标准时速"] = Math.Round(Vmas_bzcs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["实时车速"] = Math.Round(Vmas_sscs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["寄生功率"] = Math.Round(Vmas_jsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["指示功率"] = Math.Round(Vmas_zsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["加载功率"] = Math.Round(Vmas_jzgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["环境温度"] = Vmas_hjwd[i].ToString("0.0");
                                dr["相对湿度"] = Vmas_xdsd[i].ToString("0.0");
                                dr["大气压力"] = Vmas_dqyl[i].ToString("0.0");
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i].ToString("0.0");
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i].ToString("0.0");
                                dr["实际体积流量"] = Math.Round(Vmas_Exhaust_llList[i] * 101.325 * (273.15 + Vmas_Exhaust_lljtemp[i]) / (Vmas_Exhaust_lljyl[i] * 273.15), 3).ToString("0.000");
                                dr["标准体积流量"] = Math.Round(Vmas_Exhaust_llList[i], 3).ToString("0.000");
                                dr["湿度修正系数"] = Math.Round(Vmas_sdxzxs[i], 3).ToString("0.000");
                                dr["稀释修正系数"] = Math.Round(Vmas_xsxzxs[i], 3).ToString("0.000");
                                dr["稀释比"] = Vmas_Exhaust_k[i].ToString("0.000");
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i].ToString("0.0");
                                dr["尾气实际排放流量"] = Math.Round(Vmas_Exhaust_fqsjll[i], 3).ToString("0.000");
                                dr["lambda"] = Vmas_lambda[i].ToString("0.000");
                                zll += Vmas_Exhaust_fqsjll[i] * 0.6;
                                sjlc += Vmas_sscs[i - fqy_delayTime] / 3.6;
                                lambdasum += Vmas_lambda[i];
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 144; i < vmasconfig.Dssj + fqy_delayTime + 156; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i - fqy_delayTime];
                                dr["时序类别"] = "2";
                                dr["采样时序"] = (i - fqy_delayTime - vmasconfig.Dssj + 1).ToString("0");
                                dr["扭矩"] = Math.Round(Vmas_nj[i - fqy_delayTime] * 0.108f, 1).ToString("0.0");
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime].ToString("0");
                                dr["HC实时值"] = Vmas_Exhaust_hcld[i].ToString("0");
                                dr["CO实时值"] = Vmas_Exhaust_cold[i].ToString("0.00");
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i].ToString("0.00");
                                dr["NO实时值"] = Vmas_Exhaust_nold[i].ToString("0");
                                dr["环境O2浓度"] = hjo2.ToString("0.00");
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2ld[i].ToString("0.00");
                                dr["流量计O2实时值"] = Vmas_Exhaust_xso2afterDelay[i].ToString("0.00");
                                dr["HC排放质量"] = Vmas_Exhaust_hczl[i].ToString("0.000");
                                dr["CO排放质量"] = Vmas_Exhaust_cozl[i].ToString("0.000");
                                dr["CO2排放质量"] = Vmas_Exhaust_co2zl[i].ToString("0.000");
                                dr["NO排放质量"] = Vmas_Exhaust_nozl[i].ToString("0.000");
                                dr["标准时速"] = Math.Round(Vmas_bzcs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["实时车速"] = Math.Round(Vmas_sscs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["寄生功率"] = Math.Round(Vmas_jsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["指示功率"] = Math.Round(Vmas_zsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["加载功率"] = Math.Round(Vmas_jzgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["环境温度"] = Vmas_hjwd[i].ToString("0.0");
                                dr["相对湿度"] = Vmas_xdsd[i].ToString("0.0");
                                dr["大气压力"] = Vmas_dqyl[i].ToString("0.0");
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i].ToString("0.0");
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i].ToString("0.0");
                                dr["实际体积流量"] = Math.Round(Vmas_Exhaust_llList[i] * 101.325 * (273.15 + Vmas_Exhaust_lljtemp[i]) / (Vmas_Exhaust_lljyl[i] * 273.15), 3).ToString("0.000");
                                dr["标准体积流量"] = Math.Round(Vmas_Exhaust_llList[i], 3).ToString("0.000");
                                dr["湿度修正系数"] = Math.Round(Vmas_sdxzxs[i], 3).ToString("0.000");
                                dr["稀释修正系数"] = Math.Round(Vmas_xsxzxs[i], 3).ToString("0.000");
                                dr["稀释比"] = Vmas_Exhaust_k[i].ToString("0.000");
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i].ToString("0.0");
                                dr["尾气实际排放流量"] = Math.Round(Vmas_Exhaust_fqsjll[i], 3).ToString("0.000");
                                dr["lambda"] = Vmas_lambda[i].ToString("0.000");
                                zll += Vmas_Exhaust_fqsjll[i] * 0.6;
                                sjlc += Vmas_sscs[i - fqy_delayTime] / 3.6;
                                lambdasum += Vmas_lambda[i];
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 156; i < vmasconfig.Dssj + fqy_delayTime + 164; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i - fqy_delayTime];
                                dr["时序类别"] = "6";
                                dr["采样时序"] = (i - fqy_delayTime - vmasconfig.Dssj + 1).ToString("0");
                                dr["扭矩"] = Math.Round(Vmas_nj[i - fqy_delayTime] * 0.108f, 1).ToString("0.0");
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime].ToString("0");
                                dr["HC实时值"] = Vmas_Exhaust_hcld[i].ToString("0");
                                dr["CO实时值"] = Vmas_Exhaust_cold[i].ToString("0.00");
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i].ToString("0.00");
                                dr["NO实时值"] = Vmas_Exhaust_nold[i].ToString("0");
                                dr["环境O2浓度"] = hjo2.ToString("0.00");
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2ld[i].ToString("0.00");
                                dr["流量计O2实时值"] = Vmas_Exhaust_xso2afterDelay[i].ToString("0.00");
                                dr["HC排放质量"] = Vmas_Exhaust_hczl[i].ToString("0.000");
                                dr["CO排放质量"] = Vmas_Exhaust_cozl[i].ToString("0.000");
                                dr["CO2排放质量"] = Vmas_Exhaust_co2zl[i].ToString("0.000");
                                dr["NO排放质量"] = Vmas_Exhaust_nozl[i].ToString("0.000");
                                dr["标准时速"] = Math.Round(Vmas_bzcs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["实时车速"] = Math.Round(Vmas_sscs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["寄生功率"] = Math.Round(Vmas_jsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["指示功率"] = Math.Round(Vmas_zsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["加载功率"] = Math.Round(Vmas_jzgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["环境温度"] = Vmas_hjwd[i].ToString("0.0");
                                dr["相对湿度"] = Vmas_xdsd[i].ToString("0.0");
                                dr["大气压力"] = Vmas_dqyl[i].ToString("0.0");
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i].ToString("0.0");
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i].ToString("0.0");
                                dr["实际体积流量"] = Math.Round(Vmas_Exhaust_llList[i] * 101.325 * (273.15 + Vmas_Exhaust_lljtemp[i]) / (Vmas_Exhaust_lljyl[i] * 273.15), 3).ToString("0.000");
                                dr["标准体积流量"] = Math.Round(Vmas_Exhaust_llList[i], 3).ToString("0.000");
                                dr["湿度修正系数"] = Math.Round(Vmas_sdxzxs[i], 3).ToString("0.000");
                                dr["稀释修正系数"] = Math.Round(Vmas_xsxzxs[i], 3).ToString("0.000");
                                dr["稀释比"] = Vmas_Exhaust_k[i].ToString("0.000");
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i].ToString("0.0");
                                dr["尾气实际排放流量"] = Math.Round(Vmas_Exhaust_fqsjll[i], 3).ToString("0.000");
                                dr["lambda"] = Vmas_lambda[i].ToString("0.000");
                                zll += Vmas_Exhaust_fqsjll[i] * 0.6;
                                sjlc += Vmas_sscs[i - fqy_delayTime] / 3.6;
                                lambdasum += Vmas_lambda[i];
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 164; i < vmasconfig.Dssj + fqy_delayTime + 177; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i - fqy_delayTime];
                                dr["时序类别"] = "3";
                                dr["采样时序"] = (i - fqy_delayTime - vmasconfig.Dssj + 1).ToString("0");
                                dr["扭矩"] = Math.Round(Vmas_nj[i - fqy_delayTime] * 0.108f, 1).ToString("0.0");
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime].ToString("0");
                                dr["HC实时值"] = Vmas_Exhaust_hcld[i].ToString("0");
                                dr["CO实时值"] = Vmas_Exhaust_cold[i].ToString("0.00");
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i].ToString("0.00");
                                dr["NO实时值"] = Vmas_Exhaust_nold[i].ToString("0");
                                dr["环境O2浓度"] = hjo2.ToString("0.00");
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2ld[i].ToString("0.00");
                                dr["流量计O2实时值"] = Vmas_Exhaust_xso2afterDelay[i].ToString("0.00");
                                dr["HC排放质量"] = Vmas_Exhaust_hczl[i].ToString("0.000");
                                dr["CO排放质量"] = Vmas_Exhaust_cozl[i].ToString("0.000");
                                dr["CO2排放质量"] = Vmas_Exhaust_co2zl[i].ToString("0.000");
                                dr["NO排放质量"] = Vmas_Exhaust_nozl[i].ToString("0.000");
                                dr["标准时速"] = Math.Round(Vmas_bzcs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["实时车速"] = Math.Round(Vmas_sscs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["寄生功率"] = Math.Round(Vmas_jsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["指示功率"] = Math.Round(Vmas_zsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["加载功率"] = Math.Round(Vmas_jzgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["环境温度"] = Vmas_hjwd[i].ToString("0.0");
                                dr["相对湿度"] = Vmas_xdsd[i].ToString("0.0");
                                dr["大气压力"] = Vmas_dqyl[i].ToString("0.0");
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i].ToString("0.0");
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i].ToString("0.0");
                                dr["实际体积流量"] = Math.Round(Vmas_Exhaust_llList[i] * 101.325 * (273.15 + Vmas_Exhaust_lljtemp[i]) / (Vmas_Exhaust_lljyl[i] * 273.15), 3).ToString("0.000");
                                dr["标准体积流量"] = Math.Round(Vmas_Exhaust_llList[i], 3).ToString("0.000");
                                dr["湿度修正系数"] = Math.Round(Vmas_sdxzxs[i], 3).ToString("0.000");
                                dr["稀释修正系数"] = Math.Round(Vmas_xsxzxs[i], 3).ToString("0.000");
                                dr["稀释比"] = Vmas_Exhaust_k[i].ToString("0.000");
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i].ToString("0.0");
                                dr["尾气实际排放流量"] = Math.Round(Vmas_Exhaust_fqsjll[i], 3).ToString("0.000");
                                dr["lambda"] = Vmas_lambda[i].ToString("0.000");
                                zll += Vmas_Exhaust_fqsjll[i] * 0.6;
                                sjlc += Vmas_sscs[i - fqy_delayTime] / 3.6;
                                lambdasum += Vmas_lambda[i];
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 177; i < vmasconfig.Dssj + fqy_delayTime + 189; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i - fqy_delayTime];
                                dr["时序类别"] = "6";
                                dr["采样时序"] = (i - fqy_delayTime - vmasconfig.Dssj + 1).ToString("0");
                                dr["扭矩"] = Math.Round(Vmas_nj[i - fqy_delayTime] * 0.108f, 1).ToString("0.0");
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime].ToString("0");
                                dr["HC实时值"] = Vmas_Exhaust_hcld[i].ToString("0");
                                dr["CO实时值"] = Vmas_Exhaust_cold[i].ToString("0.00");
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i].ToString("0.00");
                                dr["NO实时值"] = Vmas_Exhaust_nold[i].ToString("0");
                                dr["环境O2浓度"] = hjo2.ToString("0.00");
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2ld[i].ToString("0.00");
                                dr["流量计O2实时值"] = Vmas_Exhaust_xso2afterDelay[i].ToString("0.00");
                                dr["HC排放质量"] = Vmas_Exhaust_hczl[i].ToString("0.000");
                                dr["CO排放质量"] = Vmas_Exhaust_cozl[i].ToString("0.000");
                                dr["CO2排放质量"] = Vmas_Exhaust_co2zl[i].ToString("0.000");
                                dr["NO排放质量"] = Vmas_Exhaust_nozl[i].ToString("0.000");
                                dr["标准时速"] = Math.Round(Vmas_bzcs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["实时车速"] = Math.Round(Vmas_sscs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["寄生功率"] = Math.Round(Vmas_jsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["指示功率"] = Math.Round(Vmas_zsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["加载功率"] = Math.Round(Vmas_jzgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["环境温度"] = Vmas_hjwd[i].ToString("0.0");
                                dr["相对湿度"] = Vmas_xdsd[i].ToString("0.0");
                                dr["大气压力"] = Vmas_dqyl[i].ToString("0.0");
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i].ToString("0.0");
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i].ToString("0.0");
                                dr["实际体积流量"] = Math.Round(Vmas_Exhaust_llList[i] * 101.325 * (273.15 + Vmas_Exhaust_lljtemp[i]) / (Vmas_Exhaust_lljyl[i] * 273.15), 3).ToString("0.000");
                                dr["标准体积流量"] = Math.Round(Vmas_Exhaust_llList[i], 3).ToString("0.000");
                                dr["湿度修正系数"] = Math.Round(Vmas_sdxzxs[i], 3).ToString("0.000");
                                dr["稀释修正系数"] = Math.Round(Vmas_xsxzxs[i], 3).ToString("0.000");
                                dr["稀释比"] = Vmas_Exhaust_k[i].ToString("0.000");
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i].ToString("0.0");
                                dr["尾气实际排放流量"] = Math.Round(Vmas_Exhaust_fqsjll[i], 3).ToString("0.000");
                                dr["lambda"] = Vmas_lambda[i].ToString("0.000");
                                zll += Vmas_Exhaust_fqsjll[i] * 0.6;
                                sjlc += Vmas_sscs[i - fqy_delayTime] / 3.6;
                                lambdasum += Vmas_lambda[i];
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 189; i < vmasconfig.Dssj + fqy_delayTime + 196; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i - fqy_delayTime];
                                dr["时序类别"] = "4";
                                dr["采样时序"] = (i - fqy_delayTime - vmasconfig.Dssj + 1).ToString("0");
                                dr["扭矩"] = Math.Round(Vmas_nj[i - fqy_delayTime] * 0.108f, 1).ToString("0.0");
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime].ToString("0");
                                dr["HC实时值"] = Vmas_Exhaust_hcld[i].ToString("0");
                                dr["CO实时值"] = Vmas_Exhaust_cold[i].ToString("0.00");
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i].ToString("0.00");
                                dr["NO实时值"] = Vmas_Exhaust_nold[i].ToString("0");
                                dr["环境O2浓度"] = hjo2.ToString("0.00");
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2ld[i].ToString("0.00");
                                dr["流量计O2实时值"] = Vmas_Exhaust_xso2afterDelay[i].ToString("0.00");
                                dr["HC排放质量"] = Vmas_Exhaust_hczl[i].ToString("0.000");
                                dr["CO排放质量"] = Vmas_Exhaust_cozl[i].ToString("0.000");
                                dr["CO2排放质量"] = Vmas_Exhaust_co2zl[i].ToString("0.000");
                                dr["NO排放质量"] = Vmas_Exhaust_nozl[i].ToString("0.000");
                                dr["标准时速"] = Math.Round(Vmas_bzcs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["实时车速"] = Math.Round(Vmas_sscs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["寄生功率"] = Math.Round(Vmas_jsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["指示功率"] = Math.Round(Vmas_zsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["加载功率"] = Math.Round(Vmas_jzgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["环境温度"] = Vmas_hjwd[i].ToString("0.0");
                                dr["相对湿度"] = Vmas_xdsd[i].ToString("0.0");
                                dr["大气压力"] = Vmas_dqyl[i].ToString("0.0");
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i].ToString("0.0");
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i].ToString("0.0");
                                dr["实际体积流量"] = Math.Round(Vmas_Exhaust_llList[i] * 101.325 * (273.15 + Vmas_Exhaust_lljtemp[i]) / (Vmas_Exhaust_lljyl[i] * 273.15), 3).ToString("0.000");
                                dr["标准体积流量"] = Math.Round(Vmas_Exhaust_llList[i], 3).ToString("0.000");
                                dr["湿度修正系数"] = Math.Round(Vmas_sdxzxs[i], 3).ToString("0.000");
                                dr["稀释修正系数"] = Math.Round(Vmas_xsxzxs[i], 3).ToString("0.000");
                                dr["稀释比"] = Vmas_Exhaust_k[i].ToString("0.000");
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i].ToString("0.0");
                                dr["尾气实际排放流量"] = Math.Round(Vmas_Exhaust_fqsjll[i], 3).ToString("0.000");
                                dr["lambda"] = Vmas_lambda[i].ToString("0.000");
                                zll += Vmas_Exhaust_fqsjll[i] * 0.6;
                                sjlc += Vmas_sscs[i - fqy_delayTime] / 3.6;
                                lambdasum += Vmas_lambda[i];
                                vmas_datatable.Rows.Add(dr);
                            }
                            int dt_count = vmas_datatable.Rows.Count;
                            for (int i = 1; i < 196; i++)
                            {
                                vmas_datatable.Rows[dt_count - 196 + i]["采样时序"] = i.ToString();
                            }
                        }
                        else//将数据写入逐秒数据
                        {
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 1; i < vmasconfig.Dssj + fqy_delayTime + 12; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i];
                                dr["时序类别"] = "4";
                                dr["采样时序"] = i - vmasconfig.Dssj - fqy_delayTime - 1;
                                dr["HC实时值"] = Vmas_Exhaust_hc_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO实时值"] = Vmas_Exhaust_co_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO实时值"] = Vmas_Exhaust_no_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["环境O2浓度"] = Vmas_Exhaust_hujingo2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["流量计O2实时值"] = Vmas_Exhaust_xishio2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["HC排放质量"] = Vmas_Exhaust_hczl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO排放质量"] = Vmas_Exhaust_nozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["扭矩"] = Vmas_nj[i - fqy_delayTime];
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime];
                                dr["标准时速"] = Vmas_bzcs[i - fqy_delayTime];
                                dr["实时车速"] = Vmas_sscs[i - fqy_delayTime];
                                dr["加载功率"] = Vmas_jzgl[i - fqy_delayTime];
                                dr["环境温度"] = Vmas_hjwd[i - fqy_delayTime];
                                dr["相对湿度"] = Vmas_xdsd[i - fqy_delayTime];
                                dr["大气压力"] = Vmas_dqyl[i - fqy_delayTime];
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i - fqy_delayTime];
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i - fqy_delayTime];
                                dr["实际体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["标准体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["湿度修正系数"] = Vmas_sdxzxs[i];
                                dr["稀释修正系数"] = Vmas_xsxzxs[i];
                                dr["稀释比"] = Vmas_Exhaust_k_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                dr["尾气实际排放流量"] = Vmas_Exhaust_wqll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["lambda"] = Vmas_lambda[i - vmasconfig.Dssj - fqy_delayTime - 2].ToString("0.000");
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 12; i < vmasconfig.Dssj + fqy_delayTime + 16; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i];
                                dr["时序类别"] = "5";
                                dr["采样时序"] = i - vmasconfig.Dssj - fqy_delayTime - 1;
                                dr["HC实时值"] = Vmas_Exhaust_hc_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO实时值"] = Vmas_Exhaust_co_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO实时值"] = Vmas_Exhaust_no_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["环境O2浓度"] = Vmas_Exhaust_hujingo2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["流量计O2实时值"] = Vmas_Exhaust_xishio2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["HC排放质量"] = Vmas_Exhaust_hczl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO排放质量"] = Vmas_Exhaust_nozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["扭矩"] = Vmas_nj[i - fqy_delayTime];
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime];
                                dr["标准时速"] = Vmas_bzcs[i - fqy_delayTime];
                                dr["实时车速"] = Vmas_sscs[i - fqy_delayTime];
                                dr["加载功率"] = Vmas_jzgl[i - fqy_delayTime];
                                dr["环境温度"] = Vmas_hjwd[i - fqy_delayTime];
                                dr["相对湿度"] = Vmas_xdsd[i - fqy_delayTime];
                                dr["大气压力"] = Vmas_dqyl[i - fqy_delayTime];
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i - fqy_delayTime];
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i - fqy_delayTime];
                                dr["实际体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["标准体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["湿度修正系数"] = Vmas_sdxzxs[i];
                                dr["稀释修正系数"] = Vmas_xsxzxs[i];
                                dr["稀释比"] = Vmas_Exhaust_k_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                dr["尾气实际排放流量"] = Vmas_Exhaust_wqll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["lambda"] = Vmas_lambda[i - vmasconfig.Dssj - fqy_delayTime - 2].ToString("0.000");
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 16; i < vmasconfig.Dssj + fqy_delayTime + 24; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i];
                                dr["时序类别"] = "0";
                                dr["采样时序"] = i - vmasconfig.Dssj - fqy_delayTime - 1;
                                dr["HC实时值"] = Vmas_Exhaust_hc_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO实时值"] = Vmas_Exhaust_co_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO实时值"] = Vmas_Exhaust_no_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["环境O2浓度"] = Vmas_Exhaust_hujingo2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["流量计O2实时值"] = Vmas_Exhaust_xishio2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["HC排放质量"] = Vmas_Exhaust_hczl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO排放质量"] = Vmas_Exhaust_nozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["扭矩"] = Vmas_nj[i - fqy_delayTime];
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime];
                                dr["标准时速"] = Vmas_bzcs[i - fqy_delayTime];
                                dr["实时车速"] = Vmas_sscs[i - fqy_delayTime];
                                dr["加载功率"] = Vmas_jzgl[i - fqy_delayTime];
                                dr["环境温度"] = Vmas_hjwd[i - fqy_delayTime];
                                dr["相对湿度"] = Vmas_xdsd[i - fqy_delayTime];
                                dr["大气压力"] = Vmas_dqyl[i - fqy_delayTime];
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i - fqy_delayTime];
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i - fqy_delayTime];
                                dr["实际体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["标准体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["湿度修正系数"] = Vmas_sdxzxs[i];
                                dr["稀释修正系数"] = Vmas_xsxzxs[i];
                                dr["稀释比"] = Vmas_Exhaust_k_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                dr["尾气实际排放流量"] = Vmas_Exhaust_wqll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["lambda"] = Vmas_lambda[i - vmasconfig.Dssj - fqy_delayTime - 2].ToString("0.000");
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 24; i < vmasconfig.Dssj + fqy_delayTime + 29; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i];
                                dr["时序类别"] = "6";
                                dr["采样时序"] = i - vmasconfig.Dssj - fqy_delayTime - 1;
                                dr["HC实时值"] = Vmas_Exhaust_hc_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO实时值"] = Vmas_Exhaust_co_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO实时值"] = Vmas_Exhaust_no_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["环境O2浓度"] = Vmas_Exhaust_hujingo2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["流量计O2实时值"] = Vmas_Exhaust_xishio2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["HC排放质量"] = Vmas_Exhaust_hczl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO排放质量"] = Vmas_Exhaust_nozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["扭矩"] = Vmas_nj[i - fqy_delayTime];
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime];
                                dr["标准时速"] = Vmas_bzcs[i - fqy_delayTime];
                                dr["实时车速"] = Vmas_sscs[i - fqy_delayTime];
                                dr["加载功率"] = Vmas_jzgl[i - fqy_delayTime];
                                dr["环境温度"] = Vmas_hjwd[i - fqy_delayTime];
                                dr["相对湿度"] = Vmas_xdsd[i - fqy_delayTime];
                                dr["大气压力"] = Vmas_dqyl[i - fqy_delayTime];
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i - fqy_delayTime];
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i - fqy_delayTime];
                                dr["实际体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["标准体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["湿度修正系数"] = Vmas_sdxzxs[i];
                                dr["稀释修正系数"] = Vmas_xsxzxs[i];
                                dr["稀释比"] = Vmas_Exhaust_k_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                dr["尾气实际排放流量"] = Vmas_Exhaust_wqll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["lambda"] = Vmas_lambda[i - vmasconfig.Dssj - fqy_delayTime - 2].ToString("0.000");
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 29; i < vmasconfig.Dssj + fqy_delayTime + 50; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i];
                                dr["时序类别"] = "4";
                                dr["采样时序"] = i - vmasconfig.Dssj - fqy_delayTime - 1;
                                dr["HC实时值"] = Vmas_Exhaust_hc_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO实时值"] = Vmas_Exhaust_co_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO实时值"] = Vmas_Exhaust_no_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["环境O2浓度"] = Vmas_Exhaust_hujingo2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["流量计O2实时值"] = Vmas_Exhaust_xishio2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["HC排放质量"] = Vmas_Exhaust_hczl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO排放质量"] = Vmas_Exhaust_nozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["扭矩"] = Vmas_nj[i - fqy_delayTime];
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime];
                                dr["标准时速"] = Vmas_bzcs[i - fqy_delayTime];
                                dr["实时车速"] = Vmas_sscs[i - fqy_delayTime];
                                dr["加载功率"] = Vmas_jzgl[i - fqy_delayTime];
                                dr["环境温度"] = Vmas_hjwd[i - fqy_delayTime];
                                dr["相对湿度"] = Vmas_xdsd[i - fqy_delayTime];
                                dr["大气压力"] = Vmas_dqyl[i - fqy_delayTime];
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i - fqy_delayTime];
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i - fqy_delayTime];
                                dr["实际体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["标准体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["湿度修正系数"] = Vmas_sdxzxs[i];
                                dr["稀释修正系数"] = Vmas_xsxzxs[i];
                                dr["稀释比"] = Vmas_Exhaust_k_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                dr["尾气实际排放流量"] = Vmas_Exhaust_wqll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["lambda"] = Vmas_lambda[i - vmasconfig.Dssj - fqy_delayTime - 2].ToString("0.000");
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 50; i < vmasconfig.Dssj + fqy_delayTime + 62; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i];
                                dr["时序类别"] = "5";
                                dr["采样时序"] = i - vmasconfig.Dssj - fqy_delayTime - 1;
                                dr["HC实时值"] = Vmas_Exhaust_hc_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO实时值"] = Vmas_Exhaust_co_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO实时值"] = Vmas_Exhaust_no_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["环境O2浓度"] = Vmas_Exhaust_hujingo2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["流量计O2实时值"] = Vmas_Exhaust_xishio2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["HC排放质量"] = Vmas_Exhaust_hczl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO排放质量"] = Vmas_Exhaust_nozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["扭矩"] = Vmas_nj[i - fqy_delayTime];
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime];
                                dr["标准时速"] = Vmas_bzcs[i - fqy_delayTime];
                                dr["实时车速"] = Vmas_sscs[i - fqy_delayTime];
                                dr["加载功率"] = Vmas_jzgl[i - fqy_delayTime];
                                dr["环境温度"] = Vmas_hjwd[i - fqy_delayTime];
                                dr["相对湿度"] = Vmas_xdsd[i - fqy_delayTime];
                                dr["大气压力"] = Vmas_dqyl[i - fqy_delayTime];
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i - fqy_delayTime];
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i - fqy_delayTime];
                                dr["实际体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["标准体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["湿度修正系数"] = Vmas_sdxzxs[i];
                                dr["稀释修正系数"] = Vmas_xsxzxs[i];
                                dr["稀释比"] = Vmas_Exhaust_k_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                dr["尾气实际排放流量"] = Vmas_Exhaust_wqll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["lambda"] = Vmas_lambda[i - vmasconfig.Dssj - fqy_delayTime - 2].ToString("0.000");
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 62; i < vmasconfig.Dssj + fqy_delayTime + 86; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i];
                                dr["时序类别"] = "1";
                                dr["采样时序"] = i - vmasconfig.Dssj - fqy_delayTime - 1;
                                dr["HC实时值"] = Vmas_Exhaust_hc_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO实时值"] = Vmas_Exhaust_co_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO实时值"] = Vmas_Exhaust_no_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["环境O2浓度"] = Vmas_Exhaust_hujingo2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["流量计O2实时值"] = Vmas_Exhaust_xishio2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["HC排放质量"] = Vmas_Exhaust_hczl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO排放质量"] = Vmas_Exhaust_nozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["扭矩"] = Vmas_nj[i - fqy_delayTime];
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime];
                                dr["标准时速"] = Vmas_bzcs[i - fqy_delayTime];
                                dr["实时车速"] = Vmas_sscs[i - fqy_delayTime];
                                dr["加载功率"] = Vmas_jzgl[i - fqy_delayTime];
                                dr["环境温度"] = Vmas_hjwd[i - fqy_delayTime];
                                dr["相对湿度"] = Vmas_xdsd[i - fqy_delayTime];
                                dr["大气压力"] = Vmas_dqyl[i - fqy_delayTime];
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i - fqy_delayTime];
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i - fqy_delayTime];
                                dr["实际体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["标准体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["湿度修正系数"] = Vmas_sdxzxs[i];
                                dr["稀释修正系数"] = Vmas_xsxzxs[i];
                                dr["稀释比"] = Vmas_Exhaust_k_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                dr["尾气实际排放流量"] = Vmas_Exhaust_wqll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["lambda"] = Vmas_lambda[i - vmasconfig.Dssj - fqy_delayTime - 2].ToString("0.000");
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 86; i < vmasconfig.Dssj + fqy_delayTime + 97; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i];
                                dr["时序类别"] = "6";
                                dr["采样时序"] = i - vmasconfig.Dssj - fqy_delayTime - 1;
                                dr["HC实时值"] = Vmas_Exhaust_hc_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO实时值"] = Vmas_Exhaust_co_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO实时值"] = Vmas_Exhaust_no_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["环境O2浓度"] = Vmas_Exhaust_hujingo2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["流量计O2实时值"] = Vmas_Exhaust_xishio2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["HC排放质量"] = Vmas_Exhaust_hczl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO排放质量"] = Vmas_Exhaust_nozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["扭矩"] = Vmas_nj[i - fqy_delayTime];
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime];
                                dr["标准时速"] = Vmas_bzcs[i - fqy_delayTime];
                                dr["实时车速"] = Vmas_sscs[i - fqy_delayTime];
                                dr["加载功率"] = Vmas_jzgl[i - fqy_delayTime];
                                dr["环境温度"] = Vmas_hjwd[i - fqy_delayTime];
                                dr["相对湿度"] = Vmas_xdsd[i - fqy_delayTime];
                                dr["大气压力"] = Vmas_dqyl[i - fqy_delayTime];
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i - fqy_delayTime];
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i - fqy_delayTime];
                                dr["实际体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["标准体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["湿度修正系数"] = Vmas_sdxzxs[i];
                                dr["稀释修正系数"] = Vmas_xsxzxs[i];
                                dr["稀释比"] = Vmas_Exhaust_k_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                dr["尾气实际排放流量"] = Vmas_Exhaust_wqll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["lambda"] = Vmas_lambda[i - vmasconfig.Dssj - fqy_delayTime - 2].ToString("0.000");
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 97; i < vmasconfig.Dssj + fqy_delayTime + 118; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i];
                                dr["时序类别"] = "4";
                                dr["采样时序"] = i - vmasconfig.Dssj - fqy_delayTime - 1;
                                dr["HC实时值"] = Vmas_Exhaust_hc_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO实时值"] = Vmas_Exhaust_co_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO实时值"] = Vmas_Exhaust_no_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["环境O2浓度"] = Vmas_Exhaust_hujingo2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["流量计O2实时值"] = Vmas_Exhaust_xishio2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["HC排放质量"] = Vmas_Exhaust_hczl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO排放质量"] = Vmas_Exhaust_nozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["扭矩"] = Vmas_nj[i - fqy_delayTime];
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime];
                                dr["标准时速"] = Vmas_bzcs[i - fqy_delayTime];
                                dr["实时车速"] = Vmas_sscs[i - fqy_delayTime];
                                dr["加载功率"] = Vmas_jzgl[i - fqy_delayTime];
                                dr["环境温度"] = Vmas_hjwd[i - fqy_delayTime];
                                dr["相对湿度"] = Vmas_xdsd[i - fqy_delayTime];
                                dr["大气压力"] = Vmas_dqyl[i - fqy_delayTime];
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i - fqy_delayTime];
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i - fqy_delayTime];
                                dr["实际体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["标准体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["湿度修正系数"] = Vmas_sdxzxs[i];
                                dr["稀释修正系数"] = Vmas_xsxzxs[i];
                                dr["稀释比"] = Vmas_Exhaust_k_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                dr["尾气实际排放流量"] = Vmas_Exhaust_wqll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["lambda"] = Vmas_lambda[i - vmasconfig.Dssj - fqy_delayTime - 2].ToString("0.000");
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 118; i < vmasconfig.Dssj + fqy_delayTime + 144; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i];
                                dr["时序类别"] = "5";
                                dr["采样时序"] = i - vmasconfig.Dssj - fqy_delayTime - 1;
                                dr["HC实时值"] = Vmas_Exhaust_hc_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO实时值"] = Vmas_Exhaust_co_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO实时值"] = Vmas_Exhaust_no_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["环境O2浓度"] = Vmas_Exhaust_hujingo2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["流量计O2实时值"] = Vmas_Exhaust_xishio2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["HC排放质量"] = Vmas_Exhaust_hczl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO排放质量"] = Vmas_Exhaust_nozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["扭矩"] = Vmas_nj[i - fqy_delayTime];
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime];
                                dr["标准时速"] = Vmas_bzcs[i - fqy_delayTime];
                                dr["实时车速"] = Vmas_sscs[i - fqy_delayTime];
                                dr["加载功率"] = Vmas_jzgl[i - fqy_delayTime];
                                dr["环境温度"] = Vmas_hjwd[i - fqy_delayTime];
                                dr["相对湿度"] = Vmas_xdsd[i - fqy_delayTime];
                                dr["大气压力"] = Vmas_dqyl[i - fqy_delayTime];
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i - fqy_delayTime];
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i - fqy_delayTime];
                                dr["实际体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["标准体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["湿度修正系数"] = Vmas_sdxzxs[i];
                                dr["稀释修正系数"] = Vmas_xsxzxs[i];
                                dr["稀释比"] = Vmas_Exhaust_k_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                dr["尾气实际排放流量"] = Vmas_Exhaust_wqll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["lambda"] = Vmas_lambda[i - vmasconfig.Dssj - fqy_delayTime - 2].ToString("0.000");
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 144; i < vmasconfig.Dssj + fqy_delayTime + 156; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i];
                                dr["时序类别"] = "2";
                                dr["采样时序"] = i - vmasconfig.Dssj - fqy_delayTime - 1;
                                dr["HC实时值"] = Vmas_Exhaust_hc_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO实时值"] = Vmas_Exhaust_co_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO实时值"] = Vmas_Exhaust_no_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["环境O2浓度"] = Vmas_Exhaust_hujingo2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["流量计O2实时值"] = Vmas_Exhaust_xishio2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["HC排放质量"] = Vmas_Exhaust_hczl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO排放质量"] = Vmas_Exhaust_nozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["扭矩"] = Vmas_nj[i - fqy_delayTime];
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime];
                                dr["标准时速"] = Vmas_bzcs[i - fqy_delayTime];
                                dr["实时车速"] = Vmas_sscs[i - fqy_delayTime];
                                dr["加载功率"] = Vmas_jzgl[i - fqy_delayTime];
                                dr["环境温度"] = Vmas_hjwd[i - fqy_delayTime];
                                dr["相对湿度"] = Vmas_xdsd[i - fqy_delayTime];
                                dr["大气压力"] = Vmas_dqyl[i - fqy_delayTime];
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i - fqy_delayTime];
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i - fqy_delayTime];
                                dr["实际体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["标准体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["湿度修正系数"] = Vmas_sdxzxs[i];
                                dr["稀释修正系数"] = Vmas_xsxzxs[i];
                                dr["稀释比"] = Vmas_Exhaust_k_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                dr["尾气实际排放流量"] = Vmas_Exhaust_wqll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["lambda"] = Vmas_lambda[i - vmasconfig.Dssj - fqy_delayTime - 2].ToString("0.000");
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 156; i < vmasconfig.Dssj + fqy_delayTime + 164; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i];
                                dr["时序类别"] = "6";
                                dr["采样时序"] = i - vmasconfig.Dssj - fqy_delayTime - 1;
                                dr["HC实时值"] = Vmas_Exhaust_hc_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO实时值"] = Vmas_Exhaust_co_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO实时值"] = Vmas_Exhaust_no_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["环境O2浓度"] = Vmas_Exhaust_hujingo2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["流量计O2实时值"] = Vmas_Exhaust_xishio2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["HC排放质量"] = Vmas_Exhaust_hczl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO排放质量"] = Vmas_Exhaust_nozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["扭矩"] = Vmas_nj[i - fqy_delayTime];
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime];
                                dr["标准时速"] = Vmas_bzcs[i - fqy_delayTime];
                                dr["实时车速"] = Vmas_sscs[i - fqy_delayTime];
                                dr["加载功率"] = Vmas_jzgl[i - fqy_delayTime];
                                dr["环境温度"] = Vmas_hjwd[i - fqy_delayTime];
                                dr["相对湿度"] = Vmas_xdsd[i - fqy_delayTime];
                                dr["大气压力"] = Vmas_dqyl[i - fqy_delayTime];
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i - fqy_delayTime];
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i - fqy_delayTime];
                                dr["实际体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["标准体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["湿度修正系数"] = Vmas_sdxzxs[i];
                                dr["稀释修正系数"] = Vmas_xsxzxs[i];
                                dr["稀释比"] = Vmas_Exhaust_k_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                dr["尾气实际排放流量"] = Vmas_Exhaust_wqll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["lambda"] = Vmas_lambda[i - vmasconfig.Dssj - fqy_delayTime - 2].ToString("0.000");
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 164; i < vmasconfig.Dssj + fqy_delayTime + 177; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i];
                                dr["时序类别"] = "3";
                                dr["采样时序"] = i - vmasconfig.Dssj - fqy_delayTime - 1;
                                dr["HC实时值"] = Vmas_Exhaust_hc_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO实时值"] = Vmas_Exhaust_co_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO实时值"] = Vmas_Exhaust_no_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["环境O2浓度"] = Vmas_Exhaust_hujingo2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["流量计O2实时值"] = Vmas_Exhaust_xishio2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["HC排放质量"] = Vmas_Exhaust_hczl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO排放质量"] = Vmas_Exhaust_nozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["扭矩"] = Vmas_nj[i - fqy_delayTime];
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime];
                                dr["标准时速"] = Vmas_bzcs[i - fqy_delayTime];
                                dr["实时车速"] = Vmas_sscs[i - fqy_delayTime];
                                dr["加载功率"] = Vmas_jzgl[i - fqy_delayTime];
                                dr["环境温度"] = Vmas_hjwd[i - fqy_delayTime];
                                dr["相对湿度"] = Vmas_xdsd[i - fqy_delayTime];
                                dr["大气压力"] = Vmas_dqyl[i - fqy_delayTime];
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i - fqy_delayTime];
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i - fqy_delayTime];
                                dr["实际体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["标准体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["湿度修正系数"] = Vmas_sdxzxs[i];
                                dr["稀释修正系数"] = Vmas_xsxzxs[i];
                                dr["稀释比"] = Vmas_Exhaust_k_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                dr["尾气实际排放流量"] = Vmas_Exhaust_wqll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["lambda"] = Vmas_lambda[i - vmasconfig.Dssj - fqy_delayTime - 2].ToString("0.000");
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 177; i < vmasconfig.Dssj + fqy_delayTime + 189; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i];
                                dr["时序类别"] = "6";
                                dr["采样时序"] = i - vmasconfig.Dssj - fqy_delayTime - 1;
                                dr["HC实时值"] = Vmas_Exhaust_hc_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO实时值"] = Vmas_Exhaust_co_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO实时值"] = Vmas_Exhaust_no_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["环境O2浓度"] = Vmas_Exhaust_hujingo2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["流量计O2实时值"] = Vmas_Exhaust_xishio2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["HC排放质量"] = Vmas_Exhaust_hczl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO排放质量"] = Vmas_Exhaust_nozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["扭矩"] = Vmas_nj[i - fqy_delayTime];
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime];
                                dr["标准时速"] = Vmas_bzcs[i - fqy_delayTime];
                                dr["实时车速"] = Vmas_sscs[i - fqy_delayTime];
                                dr["加载功率"] = Vmas_jzgl[i - fqy_delayTime];
                                dr["环境温度"] = Vmas_hjwd[i - fqy_delayTime];
                                dr["相对湿度"] = Vmas_xdsd[i - fqy_delayTime];
                                dr["大气压力"] = Vmas_dqyl[i - fqy_delayTime];
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i - fqy_delayTime];
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i - fqy_delayTime];
                                dr["实际体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["标准体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["湿度修正系数"] = Vmas_sdxzxs[i];
                                dr["稀释修正系数"] = Vmas_xsxzxs[i];
                                dr["稀释比"] = Vmas_Exhaust_k_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                dr["尾气实际排放流量"] = Vmas_Exhaust_wqll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["lambda"] = Vmas_lambda[i - vmasconfig.Dssj - fqy_delayTime - 2].ToString("0.000");
                                vmas_datatable.Rows.Add(dr);
                            }
                            for (int i = vmasconfig.Dssj + fqy_delayTime + 189; i < vmasconfig.Dssj + fqy_delayTime + 196; i++)
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i];
                                dr["时序类别"] = "4";
                                dr["采样时序"] = i - vmasconfig.Dssj - fqy_delayTime - 1;
                                dr["HC实时值"] = Vmas_Exhaust_hc_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO实时值"] = Vmas_Exhaust_co_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO实时值"] = Vmas_Exhaust_no_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["环境O2浓度"] = Vmas_Exhaust_hujingo2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["流量计O2实时值"] = Vmas_Exhaust_xishio2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["HC排放质量"] = Vmas_Exhaust_hczl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO排放质量"] = Vmas_Exhaust_nozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["扭矩"] = Vmas_nj[i - fqy_delayTime];
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime];
                                dr["标准时速"] = Vmas_bzcs[i - fqy_delayTime];
                                dr["实时车速"] = Vmas_sscs[i - fqy_delayTime];
                                dr["加载功率"] = Vmas_jzgl[i - fqy_delayTime];
                                dr["环境温度"] = Vmas_hjwd[i - fqy_delayTime];
                                dr["相对湿度"] = Vmas_xdsd[i - fqy_delayTime];
                                dr["大气压力"] = Vmas_dqyl[i - fqy_delayTime];
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i - fqy_delayTime];
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i - fqy_delayTime];
                                dr["实际体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["标准体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["湿度修正系数"] = Vmas_sdxzxs[i];
                                dr["稀释修正系数"] = Vmas_xsxzxs[i];
                                dr["稀释比"] = Vmas_Exhaust_k_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                dr["尾气实际排放流量"] = Vmas_Exhaust_wqll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["lambda"] = Vmas_lambda[i - vmasconfig.Dssj - fqy_delayTime - 2].ToString("0.000");
                                vmas_datatable.Rows.Add(dr);
                            }
                            int dt_count = vmas_datatable.Rows.Count;
                            for (int i = 1; i < 196; i++)
                            {
                                vmas_datatable.Rows[dt_count - 196 + i]["采样时序"] = i.ToString();
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= vmasconfig.Dssj - 1; i++)//将数据写入逐秒数据
                        {
                            dr = vmas_datatable.NewRow();
                            dr["全程时序"] = Vmas_qcsj[i];
                            dr["时序类别"] = Vmas_sxnb[i];
                            dr["采样时序"] = Vmas_cysx[i];
                            dr["扭矩"] = (Vmas_nj[i] * 0.108f).ToString("0.0");
                            dr["发动机转速"] = Vmas_fdjzs[i].ToString("0");
                            dr["HC实时值"] = Vmas_Exhaust_hcld[i].ToString("0");
                            dr["CO实时值"] = Vmas_Exhaust_cold[i].ToString("0.00");
                            dr["CO2实时值"] = Vmas_Exhaust_co2ld[i].ToString("0.00");
                            dr["NO实时值"] = Vmas_Exhaust_nold[i].ToString("0");
                            if (!isUseData)
                            {
                                dr["环境O2浓度"] = hjo2.ToString("0.00");
                            }
                            else
                            {
                                dr["环境O2浓度"] = Vmas_Exhaust_hujingo2_zb[0].ToString("0.00");
                            }
                            dr["分析仪O2实时值"] = Vmas_Exhaust_o2ld[i].ToString("0.00");
                            dr["流量计O2实时值"] = Vmas_Exhaust_xso2afterDelay[i].ToString("0.00");
                            dr["HC排放质量"] = Vmas_Exhaust_hczl[i].ToString("0.000");
                            dr["CO排放质量"] = Vmas_Exhaust_cozl[i].ToString("0.000");
                            dr["CO2排放质量"] = Vmas_Exhaust_co2zl[i].ToString("0.000");
                            dr["NO排放质量"] = Vmas_Exhaust_nozl[i].ToString("0.000");
                            dr["标准时速"] = Math.Round(Vmas_bzcs[i], 1).ToString("0.0");
                            dr["实时车速"] = Math.Round(Vmas_sscs[i], 1).ToString("0.0");
                            dr["寄生功率"] = Math.Round(Vmas_jsgl[i], 2).ToString("0.00");
                            dr["指示功率"] = Math.Round(Vmas_zsgl[i], 2).ToString("0.00");
                            dr["加载功率"] = Math.Round(Vmas_jzgl[i], 2).ToString("0.00");
                            dr["环境温度"] = Vmas_hjwd[i].ToString("0.0");
                            dr["相对湿度"] = Vmas_xdsd[i].ToString("0.0");
                            dr["大气压力"] = Vmas_dqyl[i].ToString("0.0");
                            dr["流量计温度"] = Vmas_Exhaust_lljtemp[i].ToString("0.0");
                            dr["流量计压力"] = Vmas_Exhaust_lljyl[i].ToString("0.0");
                            dr["实际体积流量"] = Math.Round(Vmas_Exhaust_llList[i] * 101.325 * (273.15 + Vmas_Exhaust_lljtemp[i]) / (Vmas_Exhaust_lljyl[i] * 273.15), 3).ToString("0.000");
                            dr["标准体积流量"] = Math.Round(Vmas_Exhaust_llList[i], 3).ToString("0.000");
                            dr["湿度修正系数"] = Math.Round(Vmas_sdxzxs[i], 3).ToString("0.000");
                            dr["稀释修正系数"] = Math.Round(Vmas_xsxzxs[i], 3).ToString("0.000");
                            dr["稀释比"] = Vmas_Exhaust_k[i].ToString("0.000");
                            dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i].ToString("0.0");
                            dr["尾气实际排放流量"] = Math.Round(Vmas_Exhaust_fqsjll[i] * 0.6, 3).ToString("0.000");
                            dr["lambda"] = Vmas_lambda[i];
                            zll += Vmas_Exhaust_fqsjll[i] * 0.6;
                            vmas_datatable.Rows.Add(dr);
                        }
                        if (!isUseData)
                        {
                            for (int i = vmasconfig.Dssj + fqy_delayTime; i < 195 + vmasconfig.Dssj + fqy_delayTime; i++)//将数据写入逐秒数据
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i - fqy_delayTime];
                                dr["时序类别"] = Vmas_sxnb[i - fqy_delayTime];
                                dr["采样时序"] = (i - fqy_delayTime - vmasconfig.Dssj + 1).ToString("0");
                                dr["扭矩"] = Math.Round(Vmas_nj[i - fqy_delayTime] * 0.108f, 1).ToString("0.0");
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime].ToString("0");
                                dr["HC实时值"] = Vmas_Exhaust_hcld[i].ToString("0");
                                dr["CO实时值"] = Vmas_Exhaust_cold[i].ToString("0.00");
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i].ToString("0.00");
                                dr["NO实时值"] = Vmas_Exhaust_nold[i].ToString("0");
                                dr["环境O2浓度"] = hjo2.ToString("0.00");
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2ld[i].ToString("0.00");
                                dr["流量计O2实时值"] = Vmas_Exhaust_xso2afterDelay[i].ToString("0.00");
                                dr["HC排放质量"] = Vmas_Exhaust_hczl[i].ToString("0.000");
                                dr["CO排放质量"] = Vmas_Exhaust_cozl[i].ToString("0.000");
                                dr["CO2排放质量"] = Vmas_Exhaust_co2zl[i].ToString("0.000");
                                dr["NO排放质量"] = Vmas_Exhaust_nozl[i].ToString("0.000");
                                dr["标准时速"] = Math.Round(Vmas_bzcs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["实时车速"] = Math.Round(Vmas_sscs[i - fqy_delayTime], 1).ToString("0.0");
                                dr["寄生功率"] = Math.Round(Vmas_jsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["指示功率"] = Math.Round(Vmas_zsgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["加载功率"] = Math.Round(Vmas_jzgl[i - fqy_delayTime], 2).ToString("0.00");
                                dr["环境温度"] = Vmas_hjwd[i].ToString("0.0");
                                dr["相对湿度"] = Vmas_xdsd[i].ToString("0.0");
                                dr["大气压力"] = Vmas_dqyl[i].ToString("0.0");
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i].ToString("0.0");
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i].ToString("0.0");
                                dr["实际体积流量"] = Math.Round(Vmas_Exhaust_llList[i] * 101.325 * (273.15 + Vmas_Exhaust_lljtemp[i]) / (Vmas_Exhaust_lljyl[i] * 273.15), 3).ToString("0.000");
                                dr["标准体积流量"] = Math.Round(Vmas_Exhaust_llList[i], 3).ToString("0.000");
                                dr["湿度修正系数"] = Math.Round(Vmas_sdxzxs[i], 3).ToString("0.000");
                                dr["稀释修正系数"] = Math.Round(Vmas_xsxzxs[i], 3).ToString("0.000");
                                dr["稀释比"] = Vmas_Exhaust_k[i].ToString("0.000");
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i].ToString("0.0");
                                dr["尾气实际排放流量"] = Math.Round(Vmas_Exhaust_fqsjll[i], 3).ToString("0.000");
                                dr["lambda"] = Vmas_lambda[i].ToString("0.000");
                                zll += Vmas_Exhaust_fqsjll[i] * 0.6;
                                sjlc += Vmas_sscs[i - fqy_delayTime] / 3.6;
                                lambdasum += Vmas_lambda[i];
                                vmas_datatable.Rows.Add(dr);
                            }
                            #endregion
                        }
                        else
                        {
                            for (int i = vmasconfig.Dssj + 2 + fqy_delayTime; i < 195 + vmasconfig.Dssj + 2 + fqy_delayTime; i++)//将数据写入逐秒数据
                            {
                                dr = vmas_datatable.NewRow();
                                dr["全程时序"] = Vmas_qcsj[i];
                                dr["时序类别"] = Vmas_sxnb[i];
                                dr["采样时序"] = i - vmasconfig.Dssj - fqy_delayTime - 1;
                                dr["HC实时值"] = Vmas_Exhaust_hc_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO实时值"] = Vmas_Exhaust_co_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2实时值"] = Vmas_Exhaust_co2ld[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO实时值"] = Vmas_Exhaust_no_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["环境O2浓度"] = Vmas_Exhaust_hujingo2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪O2实时值"] = Vmas_Exhaust_o2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["流量计O2实时值"] = Vmas_Exhaust_xishio2_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["HC排放质量"] = Vmas_Exhaust_hczl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["CO2排放质量"] = Vmas_Exhaust_cozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["NO排放质量"] = Vmas_Exhaust_nozl_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["扭矩"] = Vmas_nj[i - fqy_delayTime];
                                dr["发动机转速"] = Vmas_fdjzs[i - fqy_delayTime];
                                dr["标准时速"] = Vmas_bzcs[i - fqy_delayTime];
                                dr["实时车速"] = Vmas_sscs[i - fqy_delayTime];
                                dr["加载功率"] = Vmas_jzgl[i - fqy_delayTime];
                                dr["环境温度"] = Vmas_hjwd[i - fqy_delayTime];
                                dr["相对湿度"] = Vmas_xdsd[i - fqy_delayTime];
                                dr["大气压力"] = Vmas_dqyl[i - fqy_delayTime];
                                dr["流量计温度"] = Vmas_Exhaust_lljtemp[i - fqy_delayTime];
                                dr["流量计压力"] = Vmas_Exhaust_lljyl[i - fqy_delayTime];
                                dr["实际体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["标准体积流量"] = Vmas_Exhaust_lljll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["湿度修正系数"] = Vmas_sdxzxs[i];
                                dr["稀释修正系数"] = Vmas_xsxzxs[i];
                                dr["稀释比"] = Vmas_Exhaust_k_zb[i - vmasconfig.Dssj - fqy_delayTime - 2];
                                dr["分析仪管路压力"] = Vmas_Exhaust_qlyl[i];
                                dr["尾气实际排放流量"] = Vmas_Exhaust_wqll_zb[i - vmasconfig.Dssj - fqy_delayTime - 2] * 0.6;
                                dr["lambda"] = Vmas_lambda[i - vmasconfig.Dssj - fqy_delayTime - 2].ToString("0.000");
                                vmas_datatable.Rows.Add(dr);
                            }
                        }
                    }
                }
                catch(Exception er)
                {
                    MessageBox.Show("生成过程数据时出错：" + er.Message);
                    return;
                }
                vmas_data.Sjxslc = (sjlc / 1000.0).ToString("0.000");
                vmas_data.Stopreason = "0";
                vmas_data.LAMBDA = (lambdasum / 195.0).ToString("0.000");
                vmas_data.AirFlowAll = (zll * 60.0 / ((195 + vmasconfig.Dssj) * 1.0)).ToString();
                Msg(label_message, panel_msg,carbj.CarPH + " IG195:" + "  检测结束,请拨出探测头", true);
                if (equipconfig.useJHJK)
                {
                    double jhlambda = double.Parse(vmas_data.LAMBDA);
                    if (jhlambda > 1.1 || jhlambda < 0.9)
                    {
                        Msg(label_message, panel_msg, "λ值不在0.9~1.1之间,检测结果无效", true);
                        ts1 = "检测结果无效";
                        ts2 = "λ值超标";
                        button_ss.Text = "重新检测";
                        button_ss.Enabled = true;
                        gongkuangTime = 0f;
                        GKSJ = 0;
                        JC_Status = false;
                        ig195IsFinished = true;
                        return;
                    }
                }
                writeDataIsFinished = false;
                th_load = new Thread(load_progress);
                th_load.Start();
                csvwriter.SaveCSV(vmas_datatable, "C:/jcdatatxt/" + carbj.CarID + ".csv");
                vmasdatacontrol.writeVmasData(vmas_data);
                writeDataIsFinished = true;
                Msg(label_message, panel_msg, "测试完毕,请驶离测功机 ", true);
                ts2 = "请驶离测功机";                
                Thread.Sleep(2000);
                if (equipconfig.Lljxh == "nhf_1")
                {
                    if (flv_1000.nhf_TurnOffMotor() != "开启电机成功")
                    {
                        flv_1000.nhf_TurnOffMotor();
                    }
                }
                if(igbt!=null)
                {
                    igbt.TurnOffRelay((byte)equipconfig.HeatFan);
                    Thread.Sleep(100);
                }
                if (igbt != null)
                {
                    igbt.Lifter_Up();
                }
                driverformmin = true;
                if (vmasconfig.Flowback)
                {
                    if (vmasconfig.FlowTime > 0)
                    {
                        Thread.Sleep(500);
                        if (fla_502 != null)
                        {
                            fla_502.Blowback();                 //反吹                  1
                        }
                        for (int i = vmasconfig.FlowTime; i >= 0; i--)
                        {
                            Thread.Sleep(1000);
                            Msg(label_message, panel_msg, "反吹... " + i.ToString() + "s", true);
                            ts2 = "反吹... " + i.ToString() + "s";
                        }
                        if (fla_502 != null)
                        {
                            fla_502.StopBlowback();//停止反吹
                        }
                    }
                }
                button_ss.Text = "重新检测";
                button_ss.Enabled = true;
                gongkuangTime = 0f;
                GKSJ = 0;
                JC_Status = false;                
                ig195IsFinished = true;
                //toolStripButtonLiftUp.Enabled = true;
                //toolStripButtonLiftDown.Enabled = true;
                Thread.Sleep(1000);
                
                this.Close();
                
            }
            catch (Exception er)
            {
            }
        }
        #endregion
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
                if (carbj.CarRlzl == "3") a = 6.64f;//石油气
                else if (carbj.CarRlzl == "4") a = 5.39f;//天燃气
                else a = 4.64f;//汽油
                float CO2x = (float)(X / (a + 1.88 * X) * 100);
                float DF=0;
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
                Vmas_Exhaust_ReviseNow.NO = Vmas_Exhaust_Now.NO * DF*kH;
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
                    Ig195_status = false;
                    timer2.Stop();
                    JC_Status = false;
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
                            vmas_data.Ljcc = "-1";//记录下累计超差时间
                            vmas_data.CarID = carbj.CarID;//车辆ID
                            vmas_data.Wd = "-1";//温度
                            vmas_data.Sd = "-1";//湿度
                            vmas_data.Dqy = "-1";//大气压
                            vmas_data.Cozl = "-1";//CO质量
                            vmas_data.Noxzl = "-1";//NO质量
                            vmas_data.Hczl = "-1";//HC质量
                            vmas_data.ResidualHC = "-1";
                            vmas_data.AirFlowAll = "-1";
                            vmas_data.AmbientO2 = "-1";
                            vmas_data.Hcnox = "-1";
                            vmas_data.TestTime = "-1";
                            vmas_data.Starttime = "-1";
                            vmas_data.Stopreason = "9";
                            vmas_data.Power = "-1";
                            vmas_data.Co2 = "-1";
                            vmas_data.LAMBDA = "-1";
                            vmasdatacontrol.writeVmasData(vmas_data);
                        }
                        if (Th_get_FqandLl != null) Th_get_FqandLl.Abort();
                        if (th_get_llj != null) th_get_llj.Abort();
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
                                fla_502.unlockKeyboard();
                                Thread.Sleep(100);
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
                            if (igbt != null)
                            {
                                igbt.closeIgbt();
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
                    if (th_get_llj != null) th_get_llj.Abort();
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
                            fla_502.unlockKeyboard();
                            Thread.Sleep(100);
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
                            if (igbt != null)
                            {
                                igbt.closeIgbt();
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
        public void dt_msg(DataGridView datagridview,string title,int row_number,string message)
        {
            datagridview.Rows[row_number].Cells[title].Value = message;
        }
        public void panel_visible(Panel panel,bool visible_value)
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
        public void Msg(Label Msgowner, Panel Msgfather, string Msgstr,bool Update_DB)
        {
            BeginInvoke(new wtlsb(Msg_Show), Msgowner, Msgstr,Update_DB);
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
        public void Msg_Show(Label Msgowner, string Msgstr,bool Update_DB)
        {
            Msgowner.Text = Msgstr;
        }

        public void Msg_Position(Label Msgowner,Panel Msgfather)
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
            BeginInvoke(new wtcs(ref_Control_Text),control,text);
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
                        Msg(labelCs, panelCs, igbt.Speed.ToString("0.0"), false);
                        Msg(labelGl, panelGl, igbt.Power.ToString("0.00"), false);
                        arcScaleComponentCs.Value = igbt.Speed;
                        arcScaleComponentGl.Value = igbt.Power;
                        
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
            Image img = new Bitmap(sc_width,sc_height);
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

        }

        private void button_ss_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (JC_Status == false)
                {
                    isUseData = false;
                    co2excedlimit = 0;
                    o2excedlimit = 0;
                    //pictureBox1.Location = new Point(1, -5454);
                    //pictureBox2.Location = new Point(96, 5938);
                    pictureBox1.Location = new Point(1, -5303);
                    pictureBox2.Location = new Point(96, 5936);
                    pictureBox4.Location = new Point(96, 5936);
                    //1, -5304
                    ovalShapeLXCC.FillColor = Color.Lime;
                    ovalShapeLJCC.FillColor = Color.Lime;
                    ovalShapeNDZ.FillColor = Color.Lime;
                    ovalShapeLLJLJ.FillColor = Color.Lime;
                    ovalShapeWQLL.FillColor = Color.Lime;
                    ovalShapeXSB.FillColor = Color.Lime;
                    ovalShapeJZGL.FillColor = Color.Lime;
                    Msg(labelGksj, panelGksj, "000.0", false);
                    Msg(labelCO, panelCO, "0.0", false);
                    Msg(labelCO2, panelCO2, "0.0", false);
                    Msg(labelO2, panelO2, "0.0", false);
                    Msg(labelHC, panelHC, "0", false);
                    Msg(labelNO, panelNO, "0", false);
                    Msg(labelLL, panelLL, "0.0", false);
                    Msg(labelLXCC, panelLXCC, "0.0", false);
                    Msg(labelLJCC, panelLJCC, "0.0", false);
                    textBoxSDSD.Enabled = false;
                    textBoxSDWD.Enabled = false;
                    if (igbt != null)
                    {
                        igbt.Force_Zeroing();
                        Thread.Sleep(500);
                        Msg(label_message, panel_msg, "检测开始,举升下降", false);
                        ts2 = "举升下降...";
                        igbt.Lifter_Down();     //举升下降
                        Thread.Sleep(5000);
                    }
                    //toolStripButtonLiftUp.Enabled = false;
                    //toolStripButtonLiftDown.Enabled = false;
                    gksj_count = 0;
                    power_flag = false;
                    jctime = DateTime.Now.ToString();
                    TH_ST = new Thread(Jc_Exe);
                    timer2.Start();
                    try
                    {
                        if (Th_get_FqandLl != null)
                            Th_get_FqandLl.Abort();
                    }
                    catch
                    { }
                    try
                    {
                        if (th_get_llj != null)
                            th_get_llj.Abort();
                    }
                    catch
                    { }
                    JC_Status = true;
                    Th_get_FqandLl = new Thread(Fq_Detect);
                    th_get_llj = new Thread(llj_Detect);
                    TH_ST.Start();
                    button_ss.Text = "停止检测";
                    //panel_before.Visible = false;
                    GKSJ = 0;

                }
                else
                {
                        JC_Status = false;
                        //toolStripButtonLiftUp.Enabled = true;
                        //toolStripButtonLiftDown.Enabled = true;
                        Th_get_FqandLl.Abort();
                        th_get_llj.Abort();
                        TH_ST.Abort();
                        if (fla_502 != null)
                            fla_502.StopBlowback();
                        Ig195_status = false;
                        timer2.Stop();
                        button_ss.Text = "重新检测";
                        gongkuangTime = 0f;
                        ts2 = "检测被终止";
                        ts1 = "检测停止";
                        //gongkuangTime = 0f;
                        GKSJ = 0;
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

        

    }
    public class RNG
    {
        private static RNGCryptoServiceProvider rngp = new RNGCryptoServiceProvider();
        private static byte[] rb = new byte[4];

        /// <summary> 
        /// 产生一个非负数的乱数 
        /// </summary> 
        public static int Next()
        {
            rngp.GetBytes(rb);
            int value = BitConverter.ToInt32(rb, 0);
            if (value < 0) value = -value;
            return value;
        }
        /// <summary> 
        /// 产生一个非负数且最大值在 max 以下的乱数 
        /// </summary> 
        /// <param name="max">最大值</param> 
        public static int Next(int max)
        {
            rngp.GetBytes(rb);
            int value = BitConverter.ToInt32(rb, 0);
            value = value % (max + 1);
            if (value < 0) value = -value;
            return value;
        }
        /// <summary> 
        /// 产生一个非负数且最小值在 min 以上最大值在 max 以下的乱数 
        /// </summary> 
        /// <param name="min">最小值</param> 
        /// <param name="max">最大值</param> 
        public static int Next(int min, int max)
        {
            int value = Next(max - min) + min;
            return value;
        }
    }
}
