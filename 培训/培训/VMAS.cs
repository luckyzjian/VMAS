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

namespace 培训
{
    public partial class VMAS : Form
    {
        carinfor.carInidata carbj = new carInidata();
        VmasConfigInfdata vmasconfig = new VmasConfigInfdata();
        equipmentConfigInfdata equipconfig = new equipmentConfigInfdata();
        carIni carini = new carIni();
        configIni configini = new configIni();
        vmasdata vmas_data = new vmasdata();
        vmasdataControl vmasdatacontrol = new vmasdataControl();
        vmasDataSeconds vmas_dataseconds = new vmasDataSeconds();
        vmasDataSecondsControl vmas_datasecondscontrol = new vmasDataSecondsControl();
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
        speed_data speedNow = new speed_data();//提前显示前4秒的状态
        speed_data speedLimit =  new speed_data();
        IGBT igbt =null;
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
        public float[] Vmas_bzcs = new float[300];//标准车速
        public float[] Vmas_sscs = new float[300];//实时车速
        public float[] Vmas_jzgl = new float[300];//加载功率
        public float[] Vmas_hjwd = new float[300];//环境温度
        public float[] Vmas_xdsd = new float[300];//相对湿度
        public float[] Vmas_dqyl = new float[300];//大气压力
        public float[] Vmas_sdxzxs = new float[300];//湿度修正系数
        public float[] Vmas_xsxzxs = new float[300];//稀释修正系数
        public float[] Vmas_fxyglyl = new float[300];//分析仪管路压力
        public float[] Vmas_Exhaust_hczl = new float[300];//每秒HC质量
        public float[] Vmas_Exhaust_nozl = new float[300];//每秒NO质量

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
        public float cozl_zb = 0f;
        public float hczl_zb = 0f;
        public float nozl_zb = 0f;
        private int sxnb = 0;//时序类别，0：检测前准备  1：怠速准备  2：检测过程
       
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
        public float Set_Power = 0;                                                             //扭矩
        public static bool beforedate = true;
        public bool Ig195_status = false;
        public float  gongkuangTime = 0f;
        public string gongkuangStartTime = "";
        public string gongkuangEndTime = "";

        private static float md_co=101325*0.01f*28/8.31f/273.15f;//mg
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

        private bool isSongpin=false;

        private int rlzl = 0;//0为气油1为柴油 2为LPG 3为NG 4为双燃料
        public struct speed_data
        {
            public float speed_now_data;
            public bool isChangeD;
            //Color speed_now_color;
        };

        public VMAS()
        {
            InitializeComponent();
            this.SetStyle(
                  ControlStyles.UserPaint |
                  ControlStyles.AllPaintingInWmPaint |
                  ControlStyles.OptimizedDoubleBuffer |
                  ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Opaque, false);
            this.UpdateStyles();
        }
        
        private void Vmas_Load(object sender, EventArgs e)
        {
            pictureBox2.Parent = pictureBox1;
            pictureBox2.Location = new Point(96, 5938);
            panel_chujian.Visible = false;
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
            if(igbt!=null)
            {
                igbt.Lifter_Down();
            }
            isSongpin = false;
            
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
                csvwriter.SaveCSV(vmas_datatable, "C:/jcdatatxt/" + carbj.CarID + ".csv");
                vmasdatacontrol.writeVmasData(vmas_data);
                writeDataIsFinished = true;
            }
            catch
            { }
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
            
            //这里只初始化了废气分析仪其他设备要继续初始化
            try
            {
                if (equipconfig.Cgjifpz)
                {
                    try
                    {
                        igbt = new Dynamometer.IGBT("BNTD", equipconfig.isIgbtContainGdyk);
                        if (igbt.Init_Comm(equipconfig.Cgjck, "38400,N,8,1") == false)
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
        }
        private void initConfigInfo()
        {
            equipconfig = configini.getEquipConfigIni();
            vmasconfig = configini.getVmasConfigIni();
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
                accelerate_flag = true;
                speedNow.speed_now_data = 0f; 
            }
            else if (gongkuangtime < 15)
            {
                accelerate_flag = true;
                speedNow.speed_now_data = (gongkuangtime - 11) * 15 / 4f;
            }
            else if (gongkuangtime < 23)
            {
                accelerate_flag = true;
                speedNow.speed_now_data = 15f;
            }
            else if (gongkuangtime < 28)
            {
                accelerate_flag = false;
                speedNow.speed_now_data = 15 - (gongkuangtime - 23) * 15 / 5f;
            }
            else if (gongkuangtime < 49)
            {
                accelerate_flag = true;
                speedNow.speed_now_data = 0f;
            }
            else if (gongkuangtime < 54)
            {
                accelerate_flag = true;
                speedNow.speed_now_data = (gongkuangtime - 49) * 15f / 5f;
            }
            else if (gongkuangtime < 56)
            {
                accelerate_flag = false;
                speedNow.speed_now_data = 15f;
                speedNow.isChangeD = true;//变档中
            }
            else if (gongkuangtime < 61)
            {
                accelerate_flag = true;
                speedNow.speed_now_data = 15 + (gongkuangtime - 56) * 17f / 5f;
            }
            else if (gongkuangtime < 85)
            {
                accelerate_flag = true;
                speedNow.speed_now_data = 32f;
            }
            else if (gongkuangtime < 96)
            {
                accelerate_flag = false;
                speedNow.speed_now_data = 32 - (gongkuangtime - 85) * 32 / 11f;
            }
            else if (gongkuangtime < 117)
            {
                accelerate_flag = true;
                speedNow.speed_now_data = 0f;
            }
            else if (gongkuangtime < 122)
            {
                accelerate_flag = true;
                speedNow.speed_now_data = (gongkuangtime - 117) * 15 / 5f;
                //speedNow.isChangeD = true;//变档中
            }
            else if (gongkuangtime < 124)
            {
                accelerate_flag = false;
                speedNow.speed_now_data = 15f ;
                speedNow.isChangeD = true;//变档中
            }
            else if (gongkuangtime < 133)
            {
                accelerate_flag = true;
                speedNow.speed_now_data = 15f + (gongkuangtime - 124) * 20 / 9f;
            }
            else if (gongkuangtime < 135)
            {
                accelerate_flag = false;
                speedNow.speed_now_data = 35f;
                speedNow.isChangeD = true;//变档中
            }
            else if (gongkuangtime < 143)
            {
                accelerate_flag = true;
                power_flag = false;
                speedNow.speed_now_data = 35f+(gongkuangtime - 135) * 15 / 8f;
            }
            else if (gongkuangtime < 155)
            {
                accelerate_flag = true;
                power_flag = true;
                speedNow.speed_now_data = 50f;
            }
            else if (gongkuangtime < 163)
            {
                accelerate_flag = false;
                power_flag = false;
                speedNow.speed_now_data = 50 - (gongkuangtime - 155) * 15 / 8f;
            }
            else if (gongkuangtime < 176)
            {
                accelerate_flag = true;
                speedNow.speed_now_data = 35f;
            }
            else if (gongkuangtime < 178)
            {
                accelerate_flag = false;
                speedNow.speed_now_data = 35 - (gongkuangtime - 176) * 3 / 2f;
                speedNow.isChangeD = true;//变档中
            }
            else if (gongkuangtime < 188)
            {
                accelerate_flag = false;
                speedNow.speed_now_data = 32 - (gongkuangtime - 178) * 32 / 10f;
                //speedNow.isChangeD = true;//变档中
            }
            else
            {
                accelerate_flag = false;
                speedNow.speed_now_data = 0f;
            }
            //if (speedNow.speed_low_data < 0) speedNow.speed_low_data = 0;
            return speedNow;
        }
        private void timer2_Click(object sender, EventArgs e)
        {
            nowtime = DateTime.Now;
            if (Ig195_status)
            {
                try
                {
                    bool chaocha = true;
                    speedNow.speed_now_data = igbt.Speed;
                    speedLimit = speed_now(gongkuangTime);
                    if (gongkuangTime < 195.1)
                    {
                        if (gongkuangTime <= 8)
                        {
                            pictureBox2.Location = new Point((int)(96 + 34 * igbt.Speed / 3), (int)(5938 - 1105 * gongkuangTime /39));
                        }
                        else
                        {
                            pictureBox2.Location = new Point((int)(96 + 34 * igbt.Speed / 3), (int)(5938 - 1105 * gongkuangTime / 39));
                            pictureBox1.Location = new Point(1, (int)(-5454 + 1105 * (gongkuangTime - 8) / 39));
                        }
                    }

                    if (Convert.ToInt16(gongkuangTime*10)/10 !=GKSJ)           //每1s记录一次信息
                    {
                        gksj_count = GKSJ + vmasconfig.Dssj + 2;
                        
                        GKSJ++;
                    }

                    
                    if (gongkuangTime >= 195.0f)
                    {
                        ledNumber_gksj.LEDText = "195.0";
                    }
                    else
                    {
                        ledNumber_gksj.LEDText = gongkuangTime.ToString("000.0");
                    }
                    float thisTimeSpan = gongkuangTime;
                    TimeSpan timespan = nowtime - startTime;
                    gongkuangTime = (float)timespan.TotalMilliseconds / 1000f;
                    thisTimeSpan = gongkuangTime - thisTimeSpan;
                    if (igbt.Speed <= (speed_now(gongkuangTime).speed_now_data + 2.2f) && (igbt.Speed >= (speed_now(gongkuangTime ).speed_now_data - 2.2f)))
                        chaocha = false;
                    if (chaocha==true && speedLimit.isChangeD == false)
                    {
                        outTimeContinus += thisTimeSpan;
                        outTimeTotal += thisTimeSpan;
                        
                        

                    }
                    else
                    {
                        outTimeContinus = 0;
                    }
                    led_display(ledNumber_lxcc,outTimeContinus.ToString("0.0"));
                    led_display(ledNumber_ljcc,outTimeTotal.ToString("0.0"));
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
                Thread.Sleep(30);
            }
        }
        public void llj_Detect()
        {
            while (JC_Status)
            {
                flv_1000.Get_standardDat();
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
                Thread.Sleep(1000);
                Exhaust.Fla502_data Environment = fla_502.GetData();
                WD=Environment.HJWD;
                SD=Environment.SD;
                DQY=Environment.HJYL;
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
                datagridview_msg(dataGridView1, "判定", 1, "×");
                return;
            }
            else
                datagridview_msg(dataGridView1, "判定", 1, "√");
            
            datagridview_msg(dataGridView1, "结果", 2, SD.ToString("0.0"));
            if (SD >100)
            {
                Msg(label_chujiantishi, panel_chujiantishi, "环境湿度不合格,不能进行检测", false);
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
                    datagridview_msg(dataGridView1, "结果", 5, "检测中..." + i.ToString("0") + "秒");
                    Thread.Sleep(1000);
                }
               
            }
            Thread.Sleep(500);
            huanjiang_data = fla_502.GetData();
            if (vmasconfig.RemainedMonitor == true)
            {
                datagridview_msg(dataGridView1, "结果", 5, "CO:" + huanjiang_data.CO + ",HC:" + huanjiang_data.HC + ",NO:" + huanjiang_data.NO);
                if (huanjiang_data.CO > 2 || huanjiang_data.HC > 7 || huanjiang_data.NO > 25)//2,7,25
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
            
            Thread.Sleep(500);
            Vmas_qcsj[1] = DateTime.Now.ToString().Replace('/', '-');//全程时序
            Vmas_cysx[1] = "0";
            Vmas_sxnb[1] = "2";
            Vmas_bzcs[1] = 0f;//标准速度
            Vmas_sscs[1] = 0f;//实时速度
            Vmas_jzgl[1] = 0f;//加载功率
            Speed_listIg195[1] = 0f;//实时速度
            Vmas_Exhaust_ListIG195[1] = huanjiang_data;//尾气浓度值
            Vmas_Exhaust_Revise_ListIG195[1] = huanjiang_data;//修正的尾气深度值
            Vmas_Exhaust_llList[1] = flv_1000.ll_standard_value;//流量计标准流量值
            Vmas_Exhaust_xso2List[1] = flv_1000.o2_standard_value;//流量计稀释氧浓度
            Vmas_Exhaust_lljtemp[1] = flv_1000.temp_standard_value;//流量计温度
            Vmas_Exhaust_lljyl[1] = flv_1000.yali_standard_value;//流量计压力
            Vmas_Exhaust_qlyl[1] = huanjiang_data.QLYL;
            if (hjo2 > flv_1000.o2_standard_value && hjo2 > huanjiang_data.O2)//稀释比
            {
                Vmas_Exhaust_k[1] = (hjo2 - flv_1000.o2_standard_value) / (hjo2 - huanjiang_data.O2);
            }
            else
            {
                Vmas_Exhaust_k[1] = 0f;
            }
            Vmas_Exhaust_fqsjll[1] = Vmas_Exhaust_llList[1] * Vmas_Exhaust_k[1];//尾气实际流量
            Vmas_Exhaust_cold[1] = huanjiang_data.CO;//CO浓度
            Vmas_Exhaust_co2ld[1] = huanjiang_data.CO2;//CO2浓度
            Vmas_Exhaust_hcld[1] = huanjiang_data.HC;//HC浓度
            Vmas_Exhaust_nold[1] = huanjiang_data.NO;//NO浓度
            Vmas_Exhaust_o2ld[1] = huanjiang_data.O2;//废气仪氧气浓度
            Vmas_Exhaust_cozl[1] = Vmas_Exhaust_fqsjll[1] * huanjiang_data.CO * md_co;//CO质量mg
            Vmas_Exhaust_nozl[1] = Vmas_Exhaust_fqsjll[1] * huanjiang_data.NO * md_no;//NO质量mg
            Vmas_Exhaust_hczl[1] = Vmas_Exhaust_fqsjll[1] * huanjiang_data.HC * md_hc;//HC质量mg
            Vmas_hjwd[1] = huanjiang_data.HJWD;//温度
            Vmas_xdsd[1] = huanjiang_data.SD;//湿度
            Vmas_dqyl[1] = huanjiang_data.HJYL;//大气压
            Vmas_xsxzxs[1] = 0;//稀释修正系数
            Vmas_sdxzxs[1] = 0;//湿度修正系数
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
            int temp_flag=0;                //计数临时变量
            int zero_count = 0;
            float lljo2 = 0f;
            ig195IsFinished = false;
            DataRow dr = null;
            int cysx = 0;
            try
            {
                Msg(label_message, panel_msg, "检测开始,举升下降", false);
                igbt.Lifter_Down();     //举升下降
                Thread.Sleep(3000);
                Msg(label_message, panel_msg, "检测开始，进行空档怠速", true);
                sxnb = 1;
                Thread.Sleep(2000);
                int timeDs = vmasconfig.Dssj;
                while (timeDs >0)
                {
                    cysx = vmasconfig.Dssj - timeDs + 2;//从数组的第三个开始存，第一、二个存的是准备阶段的数
                    Msg(label_message, panel_msg, "空档怠速中..." + timeDs.ToString() + "s", false);
                    
                    Thread.Sleep(900);
                    timeDs--;
                }
                Msg(label_message, panel_msg, "测试开始，请根据工况运转循环图及语言提示进行驾驶", true);
                Thread.Sleep(1000);
                //Msg(label_tishi, panel_tishi, "开始", false);
                Thread.Sleep(100);
                Ig195_status = true;
                startTime = DateTime.Now;
                Msg(label_message, panel_msg, "空档怠速11s", false);
                //Msg(label_tishi, panel_tishi, "空档", false);
                while (GKSJ<11)
                {
                    if (GKSJ < 6)
                        Msg(label_message, panel_msg, "怠速中..." + (11 - GKSJ).ToString() + "s", false);
                    else
                    {
                        Msg(label_message, panel_msg, "怠速中..." + (11 - GKSJ).ToString() + "s," + "请变换档位至一档", false);
                        //Msg(label_tishi, panel_tishi, "挂一档", false);
                    }
                    Thread.Sleep(100);
                }
                Msg(label_message, panel_msg, "一档加速到15km/h保持", false);
                //Msg(label_tishi, panel_tishi, "一档加速", false);
                while (GKSJ <15) Thread.Sleep(100);
                Msg(label_message, panel_msg, "一档15km/h保持8s", false);
                //Msg(label_tishi, panel_tishi, "一档保持", false);
                Thread.Sleep(600);
                while (GKSJ <23)
                {
                    Msg(label_message, panel_msg, "一档15km/h保持中..." + (23 - GKSJ).ToString() + "s", false);
                    Thread.Sleep(100);
                }
                Msg(label_message, panel_msg, "一档减速到0km/h,离合器接合", false);
                //Msg(label_tishi, panel_tishi, "减速", false);
                while (GKSJ < 25) Thread.Sleep(100);
                Msg(label_message, panel_msg, "继续减速到0km/h,离合器脱离", false);
                //Msg(label_tishi, panel_tishi, "脱离离合器", false);
                while (GKSJ < 28) Thread.Sleep(100);
                Msg(label_message, panel_msg, "退档怠速21s", false);
                //Msg(label_tishi, panel_tishi, "退档", false);
                Thread.Sleep(600);
                while (GKSJ<49)
                {
                    if (GKSJ < 44)
                        Msg(label_message, panel_msg, "怠速中..." + (49 - GKSJ).ToString() + "s", false);
                    else
                    {
                        Msg(label_message, panel_msg, "怠速中..." + (49 - GKSJ).ToString() + "s," + "请变换档位至一档", false);
                        //Msg(label_tishi, panel_tishi, "挂一档", false);
                    }
                    Thread.Sleep(100);
                }
                Msg(label_message, panel_msg, "一档加速到15km/h变换为二档", false);
                //Msg(label_tishi, panel_tishi, "一档加速", false);
                while (GKSJ < 54) Thread.Sleep(100);
                Msg(label_message, panel_msg, "变换档位至二档，继续加速至32km/h", false);
                //Msg(label_tishi, panel_tishi, "挂二档加速", false);
                while (GKSJ<61) Thread.Sleep(100);
                Msg(label_message, panel_msg, "二档32km/h保持24s", false);
                //Msg(label_tishi, panel_tishi, "二档保持", false);
                Thread.Sleep(1000);
                while (GKSJ<85)
                {
                    Msg(label_message, panel_msg, "二档32km/h保持中..." + (85 - GKSJ).ToString() + "s", false);
                    Thread.Sleep(100);
                }
                Msg(label_message, panel_msg, "二档减速到0km/h,离合器接合", false);
               // Msg(label_tishi, panel_tishi, "减速", false);
                while (GKSJ < 93) Thread.Sleep(100);
                Msg(label_message, panel_msg, "继续减速到0km/h,离合器脱离", false);
                //Msg(label_tishi, panel_tishi, "脱离离合器", false);
                while (GKSJ < 96) Thread.Sleep(100);
                Msg(label_message, panel_msg, "退档怠速21s", false);
                //Msg(label_tishi, panel_tishi, "退档", false);
                Thread.Sleep(1000);
                //timeDs = 20;
                while (GKSJ<117)
                {
                    if (GKSJ < 112)
                        Msg(label_message, panel_msg, "怠速中..." + (117 - GKSJ).ToString() + "s", false);
                    else
                    {
                        Msg(label_message, panel_msg, "怠速中..." + (117 - GKSJ).ToString() + "s," + "请变换档位至一档", false);
                        //Msg(label_tishi, panel_tishi, "挂一档", false);
                    }
                    Thread.Sleep(100);
                    //timeDs--;
                }
                Msg(label_message, panel_msg, "一档加速到15km/h变换为二档", false);
                //Msg(label_tishi, panel_tishi, "一档加速", false);
                while (GKSJ < 122) Thread.Sleep(100);
                Msg(label_message, panel_msg, "变换档位至二档，继续加速至32km/h变换为三档", false);
               // Msg(label_tishi, panel_tishi, "挂二档加速", false);
                while (GKSJ < 133) Thread.Sleep(100);
                Msg(label_message, panel_msg, "变换档位至三档，继续加速至50km/h并保持", false);
                //Msg(label_tishi, panel_tishi, "挂三档加速", false);
                while (GKSJ < 143) Thread.Sleep(100);
                Msg(label_message, panel_msg, "三档50km/h保持12s,正在加载", false);
                igbt.Set_Control_Power(Set_Power);
                igbt.Start_Control_Power();                 //启动恒扭矩控制
                Thread.Sleep(1000);
                //timeDs = 11;
                while (GKSJ<155)
                {
                    Msg(label_message, panel_msg, "三档50km/h保持中..." + (155 - GKSJ).ToString() + "s" + ",加载功率:" + Set_Power.ToString("0.00") + "KW", false);
                    Thread.Sleep(100);
                    //timeDs--;
                }
                igbt.Exit_Control();                 //退出恒功率控制
                //igbt.Set_Control_Power(0f);
                Msg(label_message, panel_msg, "加载完毕,三档减速到35km/h并保持", false);
                //Msg(label_tishi, panel_tishi, "减速", false);
                while (GKSJ < 163) Thread.Sleep(100);
                Msg(label_message, panel_msg, "三档35km/h保持13s", false);
                //Msg(label_tishi, panel_tishi, "保持", false);
                Thread.Sleep(600);
                //timeDs = 12;
                while (GKSJ<176)
                {
                    Msg(label_message, panel_msg, "三档35km/h保持中..." + (176 - GKSJ).ToString() + "s", false);
                    //Msg(label_tishi, panel_tishi, "等待开始", true);
                    Thread.Sleep(100);
                    //timeDs--;
                }
                Msg(label_message, panel_msg, "请换档至二档，减速到0km/h,离合器接合", false);
                //Msg(label_tishi, panel_tishi, "挂二档减速", false);
                while (GKSJ < 185) Thread.Sleep(100);
                Msg(label_message, panel_msg, "继续减速到0km/h,离合器脱离", true);
                //Msg(label_tishi, panel_tishi, "脱离离合器", false);
                while (GKSJ < 188) Thread.Sleep(100);
                Msg(label_message, panel_msg, "退档怠速", false);
                //Msg(label_tishi, panel_tishi, "退档", false);
                Thread.Sleep(1000);
                //timeDs = 6;
                while (GKSJ < 195 )
                {
                    Msg(label_message, panel_msg, "怠速中..." + (195  - GKSJ).ToString() + "s", false);
                    Thread.Sleep(100);
                    //timeDs--;
                }
                while (GKSJ < 195 + fqy_delayTime)
                {
                    Msg(label_message, panel_msg, "等待废气仪采样完毕..." + (195 + fqy_delayTime - GKSJ).ToString() + "s", false);
                    Thread.Sleep(100);
                    //timeDs--;
                }
                vmas_data.Ljcc = outTimeTotal.ToString("0.0");//记录下累计超差时间
                Ig195_status = false;
                
                Msg(label_message, panel_msg, "工况测试结束,请开探头，车辆怠速200s后驶离测功机", false);
                igbt.Lifter_Up();
                Msg(label_message, panel_msg, "测试完毕,请驶离测功机 " , true);
                Thread.Sleep(1000);
                igbt.Lifter_Up();
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
                            vmasdatacontrol.writeVmasData(vmas_data);
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
                    if (MessageBox.Show("测试已完成，确认退出？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
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
                        ledNumberCs.LEDText = igbt.Speed.ToString("0.0");
                        ledNumberGl.LEDText = igbt.Power.ToString("0.00");
                        arcScaleComponentCs.Value = igbt.Speed;
                        arcScaleComponentGl.Value = igbt.Power;
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


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
        }



        private void button_ss_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (JC_Status == false)
                {
                    isUseData = false;
                    pictureBox1.Location = new Point(1, -5454);
                    pictureBox2.Location = new Point(96, 5938);
                    ovalShapeLXCC.FillColor = Color.Lime;
                    ovalShapeLJCC.FillColor = Color.Lime;
                    ovalShapeNDZ.FillColor = Color.Lime;
                    ovalShapeLLJLJ.FillColor = Color.Lime;
                    ovalShapeWQLL.FillColor = Color.Lime;
                    ovalShapeXSB.FillColor = Color.Lime;
                    ovalShapeJZGL.FillColor = Color.Lime;
                    led_display(ledNumber_gksj, "000.0");
                    led_display(ledNumber_ljcc, "0.0");
                    led_display(ledNumber_lxcc, "0.0");
                    led_display(ledNumberCO, "0.0");
                    led_display(ledNumberCO2, "0.0");
                    led_display(ledNumberNO, "0");
                    led_display(ledNumberHC, "0");
                    led_display(ledNumberO2, "0.0");
                    led_display(ledNumberLL, "0.0");
                    textBoxSDSD.Enabled = false;
                    textBoxSDWD.Enabled = false;
                    igbt.Force_Zeroing();
                    //toolStripButtonLiftUp.Enabled = false;
                    //toolStripButtonLiftDown.Enabled = false;
                    gksj_count = 0;
                    power_flag = false;
                    jctime = DateTime.Now.ToString();
                    TH_ST = new Thread(Jc_Exe);
                    timer2.Start();
                    JC_Status = true;
                    TH_ST.Start();
                    button_ss.Text = "停止检测";
                    //panel_before.Visible = false;
                    GKSJ = 0;

                }
                else
                {
                    if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Left)
                    {
                        JC_Status = false;
                        TH_ST.Abort();
                        Ig195_status = false;
                        timer2.Stop();
                        button_ss.Text = "重新检测";
                        gongkuangTime = 0f;
                        
                        //gongkuangTime = 0f;
                        GKSJ = 0;
                    }
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
}
