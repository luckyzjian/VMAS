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
/*
 * 2017-10-13 luckyzjian 加载减速加入甘肃地标过程判定内容：1、功率不合格，检测结束，REV100,HK,NK,EK均返回空；2、HK,NK,EK任意一个不合格，检测结束，判定为不合格，未检测的返回空；3、HK,NK,EK任意一个值小于限值的90%，检测结束，判定为合格，未检测的返回为空
 */

namespace lugdowm
{
    public partial class Jzjs : Form
    {
        const string yq_mqw5101 = "mqw_5101";
        carinfor.carInidata carbj = new carInidata();
        equipmentConfigInfdata equipconfig = new equipmentConfigInfdata();
        LugdownConfigInfdata lugdownconfig = new LugdownConfigInfdata();
        carIni carini = new carIni();
        configIni configini = new configIni();
        CSVcontrol.CSVwriter csvwriter = new CSVcontrol.CSVwriter();
        CSVcontrol.csvReader csvreader = new CSVcontrol.csvReader();
        carinfor.jzjsdata jzjs_data = new jzjsdata();
        jzjsdataControl jzjsdatacontrol = new jzjsdataControl();
        jzjsDataSeconds jzjs_dataseconds = new jzjsDataSeconds();
        jzjsDataSecondsControl jzjs_datasecondscontrol = new jzjsDataSecondsControl();
        statusconfigIni statusconfigini = new statusconfigIni();
        upanControl.ucontrol u_control = new upanControl.ucontrol();
        DataTable dt_zb = null;
        private int data_zb_count = 0;
        DataTable dt = new DataTable();
        private string UseFqy = "";
        Exhaust.Fla501 fla_501 = null;
        Exhaust.Fla502 fla_502 = null;
        Exhaust.FLB_100 flb_100 = null;
        Exhaust.Flv_1000 flv_1000 = null;
        Exhaust.VMT_2000 vmt_2000 = null;
        Exhaust.RPM5300 rpm5300 = null;
        Exhaust.XCE_100 xce_100 = null;
        Exhaust.FLB_100 notester = null;
        Exhaust.Nhsjz nhsjz = null;
        private Exhaust.yhControl yhy = null;
        thaxs thaxsdata = new thaxs();
        bool isUseRotater = false;
        LedControl.BX5k1 ledcontrol = null;
        public static IGBT igbt = null;
        public CSVcontrol.CSVHelper jzjs_csv = null;

        private bool chujianIsFinished = false;
        private bool chujianIsOk = false;

        private bool igjzjsIsFinished = false;
        private bool writeDataIsFinished = false;
        private bool wsd_sure = false;
        private bool wsdValueIsRight = false;

        private DateTime startTime, nowtime;
        private bool isUsedata = false;
        private int sxnb = 0;//时序类别，0：加速 1：功率扫描 2：100% 3：90% 4：80%
        private int cysx = 0;

        public static float MaxRPM = 0;
        public static bool MaxRpm_sure = false;
        Thread Th_get_FqandLl = null;                                                           //烟度检测线程
        public double WD = 20;                                                                      //温度
        public double SD = 20;                                                                      //相对湿度
        public double DQY = 101;                                                                    //大气压
        DataTable Jccl = null;                                                                      //检测车辆信息
        public static bool YJ_Status = true;                                                        //预检通过状态
        public float Speed = 0;                                                                     //速度
        public float Force = 0;                                                                     //力
        public float Power = 0;                                                                     //功率
        public static float ZS = 700;                                                                        //转速
        private float yw_now = 0;
        public static float Smoke = 0;                                                                     //烟度
        public static float No = 0;
        public float GL = 0;                                                                          //功率
        public float MaxP = 0;                                                                      //最大轮边功率
        public float MaxZ = 0;                                                                      //最大轮边功率时转速
        public float VelMaxHP = 0;                                                                  //计算的真实的最大轮边功率时的线鼓转速（速度）
        public float VelMaxHP_real = 0;                                                             //测到的最大轮边功率时的线鼓速度
        public string MAXLBGL = "";
        public string MAXLBZS = "";
        Thread TH_ST = null;                                                                        //检测线程
        Thread Th_show = null;                                                                      //信息显示线程
        Thread th_load = null;
        public bool JC_Status = false;                                                              //检测状态
        public string Jc_Process = "";                                                              //检测过程状态
        public float[] Speedlist = new float[1024];                                               //每秒速度数组
        public float[] Forcelist = new float[1024];                                               //每秒扭力数组
        public float[] GXXSlist = new float[1024];                                                //每秒光吸收系数数组
        public float[] btglist = new float[1024];                                               //每秒扭力数组
        public float[] wdlist = new float[1024];                                                //每秒光吸收系数数组
        public float[] sdlist = new float[1024];                                               //每秒发动机转速数数组
        public float[] dqylist = new float[1024];                                                  //每秒功率数组
        public float[] ywlist = new float[1024];
        public float[] FDJZSlist = new float[1024];
        public float[] DCFlist = new float[1024];                                                //每秒发动机转速数数组
        public float[] JSGLlist = new float[1024];                                                  //寄生功率
        public float[] ZSGLlist = new float[1024];                                                  //指示功率
        public float[] ZGLlist = new float[1024];                                                  //总功率=寄生功率+指示功率
        public string[] Qcsxlist = new string[1024];
        public string[] Sxnblist = new string[1024];
        public int[] Cysxlist = new int[1024];
        public float[] Speedlist_zb = new float[1024];                                               //每秒速度数组
        public float[] Forcelist_zb = new float[1024];                                               //每秒扭力数组
        public float[] GXXSlist_zb = new float[1024];                                                //每秒光吸收系数数组
        public float[] FDJZSlist_zb = new float[1024];                                               //每秒发动机转速数数组
        public float[] GLlist_zb = new float[1024];                                                  //每秒功率数组
        public string[] Qcsxlist_zb = new string[1024];
        public string[] Sxnblist_zb = new string[1024];
        public int[] Cysxlist_zb = new int[1024];
        public int[] opnolist = new int[1024];
        public int[] opcodelist = new int[1024];
        public int[] dynnlist = new int[1024];
        public int[] Nolist = new int[1024];
        int opno = 1;
        int opcode = 11;
        public float dcf = 0;
        public string HV = "-";                                                                     //100%VelMaxHP发动机转速
        public string NV = "-";                                                                     //90%VelMaxHP发动机转速
        public string EV = "-";                                                                     //80%VelMaxHP发动机转速
        public string HP = "-";                                                                     //100%VelMaxHP轮边功率
        public string NP = "-";                                                                     //90%VelMaxHP轮边功率
        public string EP = "-";                                                                     //80%VelMaxHP轮边功率
        public string HK = "-";                                                                     //100%VelMaxHP光吸收系数
        public string NK = "-";                                                                     //90%VelMaxHP光吸收系数
        public string EK = "-";                                                                     //80%VelMaxHP光吸收系数
        public string HNo = "-";                                                                     //100%VelMaxHP光吸收系数
        public string NNo = "-";                                                                     //90%VelMaxHP光吸收系数
        public string ENo = "-";                                                                     //80%VelMaxHP光吸收系数

        public string MMZGXSD = "-";                                                                //每秒转鼓线速度
        public string MMCGJFH = "-";                                                                //每秒测功机负荷
        public string MMGX = "-";                                                                   //每秒光吸收系数
        public string MMZGZDL = "-";                                                                //每秒转鼓制动力
        public float GXXZ = 0;                                                                      //光吸收系数限值
        public float GLXZ = 0;                                                                      //功率限值
        public string GLPD = "未判定";                                                              //功率判定结果
        public string PDJG = "未判定";                                                               //判定结果
        public string ZSPD = "未判定";                                                              //转速判定结果
        public int GKSJ = 0;                                                                        //工况时间
        public int JCCXSJ = 0;                                                                      //检测持续时间
        public string MMFDJZS = "-";                                                                //每秒发动机转速
        public bool FDJGLCZ = false;                                                                //发动机功率超过测功机限值状态
        public string CJ = "-";
        public delegate void wt();                                                                  //委托
        public delegate void wtfffff(float XSGL, float GXSXS, float FDJZS, float CS, float NL);//委托
        public delegate void wtds_clr();
        public delegate void wtlsb(Label Msgowner, string Msgstr, bool Update_DB);                   //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                                 //委托
        public delegate void wtcs(Control controlname, string text);                                //委托
        public delegate void wtpanelvisible(Panel panel, bool visible_value);                       //委托
        public delegate void wtbuttonvisible(Button button, bool visible_value);                       //委托
        public delegate void wtdtview(DataGridView datagridview, string title, int row_number, string message);//委托
        public delegate void wl_led(LEDNumber.LEDNumber lednumber, string data);
        public delegate void wttextboxenable(TextBox textbox, bool visible_value);
        public delegate void wttextboxvalue(TextBox textbox, string value);
        public string Cllx = "";                                                                    //车辆类型
        public string PD100 = "未判定";                                                             //VelMaxHP单项判定
        public string PD90 = "未判定";                                                              //VelMaxHP单项判定
        public string PD80 = "未判定";                                                              //VelMaxHP单项判定
        public Exhaust.Flb_100_smoke smoke = new Exhaust.Flb_100_smoke();                                                         //MQY-200烟度计数据
        public Exhaust.Flb_100_smoke nosmoke = new Exhaust.Flb_100_smoke();
        public Exhaust.mqw_5101_status realstatus = new Exhaust.mqw_5101_status();
        public float Control_Speed = 0;                                                             //控制的速度
        public float gongkuangTime = 0f;
        public int PregongkuangTime = 0;
        public static bool Jzjs_status = false;
        private bool fq_getdata = false;
        public float fa = 1f;
        public float fm = 1.2f;
        public string jctime = "";
        private string dogUse = "";
        private bool isSongpin = false;
        private int gksj_zb = 0, gksj0_zb = 0, gksj1_zb = 0, gksj2_zb = 0, gksj3_zb = 0, gksj4_zb = 0;
        public static string ts1 = "川AV7M82";
        public static string ts2 = "加载减速法";
        public static bool driverformmin = false;
        private int displaycount = 0;
        private string ledComString = "";
        private bool isautostart = true;
        public Jzjs()
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
        }
        /// <summary>
        /// 计算fa
        /// </summary>
        /// <param name="wd">温度：摄氏度</param>
        /// <param name="sd">湿度：%</param>
        /// <param name="dqy">大气压：kPa</param>
        /// <param name="jqfs">进气方式 0：自然吸气和机械增压的 1：涡轮增压</param>
        /// <returns></returns>
        private double init_xzxs(double wd, double sd, double dqy, int jqfs)
        {
            if (dqy == 0)
                return 1;
            else
            {
                double fa = 1;
                if (jqfs == 0)
                    fa = (99.0 / dqy) * (Math.Pow(((wd + 273) / 298.0), 0.7));
                else
                    fa = (Math.Pow((99.0 / dqy), 0.7)) * (Math.Pow(((wd + 273) / 298.0), 1.5));
                double fm = 1.2;
                double xs = Math.Pow(fa, fm);

                return xs;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Po">实测功率</param>
        /// <param name="wd">温度：摄氏度</param>
        /// <param name="sd">湿度：%</param>
        /// <param name="dqy">大气压：kPa</param>
        /// <param name="jqfs">进气方式 0：自然吸气和机械增压的 1：涡轮增压</param>
        /// <returns>计算修正功率</returns>
        private double caculate_Pc(double Po, double wd, double sd, double dqy, int jqfs)
        {
            double Pc = 0;
            double fa = init_xzxs(wd, sd, dqy, jqfs);
            double fm = 1.2;
            Pc = Po * Math.Pow(fa, fm);
            return Pc;
        }
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
        private void Jzjs_Load(object sender, EventArgs e)
        {
            //byte keyvalue = (byte)(0x1f >> 4);
            //double Pc = caculate_Pc(45.0, 0.5, 50, 101, 0);
            //flv_1000 = new Exhaust.Flv_1000();
            panel_chujian.Visible = false;
            button1.Visible = false;
            initJsglXs();
            initCarInfo();
            initConfigInfo();
            initEquipment();
            //ledcontrol.writeLed("白日依山尽", 2,equipconfig.Ledxh);
            initChujian();
            Init_Data();                                //初始化数据
            Init_Chart();                               //初始化图标
            Init_Timer();                               //初始化Timer
            isSongpin = false;
            /*if (igbt != null)
            {
                igbt.Lifter_Down();
            }*/
            if (!isautostart)
            {
                prepareFoem prepareform = new prepareFoem();
                prepareform.ShowDialog();
            }
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
                if (flb_100 != null)
                    flb_100.kxs = carbj.JZJS_K;
                if (notester != null)
                    notester.kxs = carbj.JZJS_K;
                igbt.forcexs = carbj.JZJS_GL;
            }
            if (isautostart)
            {
                Thread.Sleep(3000);
                button_ss_Click_1(sender, e);
            }
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
                                MessageBox.Show("废气仪串口" + equipconfig.Fqyck + "打开失败:" + er.ToString(), "出错啦");
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
                                MessageBox.Show("废气仪串口" + equipconfig.Fqyck + "打开失败:" + er.ToString(), "出错啦");
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
                                MessageBox.Show("废气仪串口" + equipconfig.Fqyck + "打开失败:" + er.ToString(), "出错啦");
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
                                MessageBox.Show("废气仪串口" + equipconfig.Fqyck + "打开失败:" + er.ToString(), "出错啦");
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
                                MessageBox.Show("废气仪串口" + equipconfig.Fqyck + "打开失败:" + er.ToString(), "出错啦");
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
                                MessageBox.Show("废气仪串口" + equipconfig.Fqyck + "打开失败:" + er.ToString(), "出错啦");
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
                if (equipconfig.isYhyPz == true)
                {
                    switch (equipconfig.YhyXh.ToLower())           //通过仪器型号选择初始化项目
                    {
                        case "mql_8201":
                            try
                            {
                                yhy = new Exhaust.yhControl(equipconfig.YhyXh);
                                if (yhy.Init_Comm(equipconfig.YhyCk, equipconfig.YhjCkpz) == false)
                                {
                                    yhy = null;
                                    Init_flag = false;
                                    init_message = "油耗仪串口打开失败.";
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show("油耗仪串口" + equipconfig.YhyCk + "打开失败:" + er.ToString(), "出错啦");
                                yhy = null;
                                Init_flag = false;
                            }
                            break;
                        case "fly_2000":
                            try
                            {
                                yhy = new Exhaust.yhControl(equipconfig.YhyXh);
                                if (yhy.Init_Comm(equipconfig.YhyCk, equipconfig.YhjCkpz) == false)
                                {
                                    yhy = null;
                                    Init_flag = false;
                                    init_message = "油耗仪串口打开失败.";
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show("油耗仪串口" + equipconfig.YhyCk + "打开失败:" + er.ToString(), "出错啦"); ;
                                yhy = null;
                                Init_flag = false;
                            }
                            break;
                        case "nhty_1":
                            try
                            {
                                yhy = new Exhaust.yhControl(equipconfig.YhyXh);
                                if (yhy.Init_Comm(equipconfig.YhyCk, equipconfig.YhjCkpz) == false)
                                {
                                    yhy = null;
                                    Init_flag = false;
                                    init_message = "油耗仪串口打开失败.";
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show("油耗仪串口" + equipconfig.YhyCk + "打开失败:" + er.ToString(), "出错啦");
                                yhy = null;
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
                        MessageBox.Show("测功机串口" + equipconfig.Cgjck + "打开失败:" + er.ToString(), "出错啦");
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
                        MessageBox.Show("烟度计串口" + equipconfig.Ydjck + "打开失败:" + er.ToString(), "出错啦");
                    }
                }
            }
            catch (Exception)
            {
            }
            try
            {
                if (lugdownconfig.testNOx)//是否检测NO
                {
                    if (equipconfig.Ydjxh != yq_mqw5101)//烟度计型号是否是MQW5101,如果是，则不用再单独打开NO测量仪
                    {
                        if (equipconfig.NOxifpz)//是否配置了NO测量仪
                        {
                            try
                            {
                                notester = new Exhaust.FLB_100(equipconfig.NOxXh);
                                //flv_1000.Get_standardDat();
                                if (notester.Init_Comm(equipconfig.NOxCk, equipconfig.NOxCkpz) == false)
                                {
                                    notester = null;
                                    Init_flag = false;
                                    init_message += "串口打开失败.";

                                }
                            }
                            catch (Exception er)
                            {
                                notester = null;
                                Init_flag = false;
                                MessageBox.Show("NOx测量仪" + equipconfig.NOxXh + "串口" + equipconfig.NOxCk + "打开失败:" + er.ToString(), "警告");
                            }
                        }
                        else
                        {
                            MessageBox.Show("未配置检测柴油车NO仪器，请检查相关配置", "警告");
                        }
                    }
                }
            }
            catch (Exception)
            {
                notester = null;
                Init_flag = false;
            }
            try
            {
                if (equipconfig.Lljifpz == true)
                {
                    try
                    {
                        flv_1000 = new Exhaust.Flv_1000(equipconfig.Lljxh);
                        //flv_1000.Get_standardDat();
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
                        MessageBox.Show("流量计串口" + equipconfig.Lljck + "打开失败:" + er.ToString(), "出错啦");
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
                        ledcontrol = new LedControl.BX5k1();
                        ledcontrol.row1 = (byte)equipconfig.ledrow1;
                        ledcontrol.row2 = (byte)equipconfig.ledrow2;
                        if (ledcontrol.Init_Comm(equipconfig.Ledck, equipconfig.LedComstring, (byte)equipconfig.LEDTJPH) == false)
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
                        MessageBox.Show("LED屏串口" + equipconfig.Ledck + "打开失败:" + er.ToString(), "出错啦");
                    }
                }

            }
            catch (Exception)
            {
            }
            try
            {
                if (lugdownconfig.Zsj.ToLower() == "vmt-2000" || lugdownconfig.Zsj.ToLower() == "vut-3000")
                {
                    try
                    {
                        vmt_2000 = new Exhaust.VMT_2000();
                        isUseRotater = true;
                        if (vmt_2000.Init_Comm(lugdownconfig.Zsjck, "19200,N,8,1") == false)
                        {
                            vmt_2000 = null;
                            Init_flag = false;
                            init_message += "转速计串口打开失败.";
                            isUseRotater = false;
                        }

                    }
                    catch (Exception er)
                    {
                        vmt_2000 = null;
                        Init_flag = false;
                        MessageBox.Show("转速计串口" + lugdownconfig.Zsjck + "打开失败:" + er.ToString(), "出错啦");
                        isUseRotater = false;
                    }
                }
                else if (lugdownconfig.Zsj.ToLower() == "mqz-2" || lugdownconfig.Zsj.ToLower() == "mqz-3")
                {
                    MessageBox.Show("系统未提供该转速计功能，请重新配置", "系统提示");
                    isUseRotater = false;
                }
                else if (lugdownconfig.Zsj.ToLower() == "rpm5300")
                {
                    try
                    {
                        rpm5300 = new Exhaust.RPM5300();
                        isUseRotater = true;
                        if (rpm5300.Init_Comm(lugdownconfig.Zsjck, "9600,N,8,1") == false)
                        {
                            rpm5300 = null;
                            Init_flag = false;
                            init_message += "转速计串口打开失败.";
                            isUseRotater = false;
                        }

                    }
                    catch (Exception er)
                    {
                        rpm5300 = null;
                        Init_flag = false;
                        MessageBox.Show("转速计串口" + lugdownconfig.Zsjck + "打开失败:" + er.ToString(), "出错啦");
                        isUseRotater = false;
                    }
                }
                else
                {
                    isUseRotater = false;
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
                        MessageBox.Show("气象站串口" + equipconfig.Xce100ck + "打开失败:" + er.ToString(), "出错啦");
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
                        MessageBox.Show("气象站串口" + equipconfig.Xce100ck + "打开失败:" + er.ToString(), "出错啦");
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
                        MessageBox.Show("气象站串口" + equipconfig.Xce100ck + "打开失败:" + er.ToString(), "出错啦");
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
                        MessageBox.Show("气象站串口" + equipconfig.Xce100ck + "打开失败:" + er.ToString(), "出错啦");
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
            dataGridView1.DataSource = dt;
        }
        private void initCarInfo()
        {
            carbj = carini.getCarIni();
            GLXZ = carbj.CarEdgl * 0.5f;
            ts1 = carbj.CarPH;
        }
        private void initConfigInfo()
        {
            equipconfig = configini.getEquipConfigIni();
            isautostart = equipconfig.WorkAutomaticMode;
            lugdownconfig = configini.getLugdownConfigIni();
            thaxsdata = configini.getthaxsConfigIni();
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;

            ini.INIIO.GetPrivateProfileString("配置参数", "LED配置字", "", temp, 2048, @".\detectConfig.ini");
            ledComString = temp.ToString();
        }
        private void init_zbData()
        {
            Random ra = new Random();
            dt_zb = csvreader.readCsv(@dogUse + "\\zb\\jzjs_configdata.csv");
            if (dt_zb != null)
            {
                data_zb_count = dt_zb.Rows.Count - 1;//一共有多少数
                gksj_zb = data_zb_count;
                //float[] NunArray = new float[dt_zb.Rows.Count-1];
                for (int i = 1; i < dt_zb.Rows.Count; i++)
                {
                    try
                    {
                        Speedlist_zb[i - 1] = (float)Math.Round(double.Parse(dt_zb.Rows[i]["车速"].ToString()) + 0.02 * (ra.Next(10) - 5), 2);
                        if (Speedlist_zb[i - 1] <= 0)
                            Speedlist_zb[i - 1] = 0;//每秒速度数组
                        //Forcelist_zb[i - 1] = float.Parse(dt_zb.Rows[i]["车速"].ToString()) + 0.02f * (ra.Next(10) - 5);                                               //每秒扭力数组
                        GXXSlist_zb[i - 1] = (float)Math.Round(double.Parse(dt_zb.Rows[i]["光吸收系数K"].ToString()) + 0.03 * (ra.Next(10) - 5), 2);
                        if (GXXSlist_zb[i - 1] <= 0)
                            GXXSlist_zb[i - 1] = 0;//每秒光吸收系数数组
                        //FDJZSlist_zb[i - 1] = float.Parse(dt_zb.Rows[i]["车速"].ToString()) + 0.02f * (ra.Next(10) - 5);                                              //每秒发动机转速数数组
                        GLlist_zb[i - 1] = (float)Math.Round(double.Parse(dt_zb.Rows[i]["功率"].ToString()) + 0.02 * (ra.Next(10) - 5), 1);
                        Qcsxlist_zb[i - 1] = dt_zb.Rows[i]["全程时序"].ToString();
                        Sxnblist_zb[i - 1] = dt_zb.Rows[i]["时序类别"].ToString();
                        if (Sxnblist_zb[i - 1] == "0") gksj0_zb++;
                        else if (Sxnblist_zb[i - 1] == "1") gksj1_zb++;
                        else if (Sxnblist_zb[i - 1] == "2") gksj2_zb++;
                        else if (Sxnblist_zb[i - 1] == "3") gksj3_zb++;
                        else if (Sxnblist_zb[i - 1] == "4") gksj4_zb++;
                        Cysxlist_zb[i - 1] = int.Parse(dt_zb.Rows[i]["采样时序"].ToString());
                    }

                    catch (Exception)
                    {

                        continue;
                    }
                }

            }
        }

        #region 初始化
        public void Init_Timer()
        {
            timer_show.Interval = 100;
            Th_show = new Thread(Datashow);
            Th_show.Start();
        }

        public void Init_Data()
        {
            Msg(Msg_msg, panel_msg, carbj.CarPH + "请上测功机检测", true);
            Msg(Msg_cp, panel_cp, carbj.CarPH, true);
            if (ledcontrol != null)
            {
                ledcontrol.writeLed(carbj.CarPH, 2, equipconfig.Ledxh);
                Thread.Sleep(200);
                ledcontrol.writeLed("请上线准备", 5, equipconfig.Ledxh);
            }
        }

        public void Init_Chart()
        {
            try
            {
                chart1.Series["XSGL"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;//吸收功率
                chart1.Series["GXSXS"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;//光吸收系数
                chart1.Series["FDJZS"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;//发动机转速
                chart1.Series["CS"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;//车速
                chart1.Series["NL"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;//车速
                //chart1.ChartAreas["ChartArea1"].AxisX.Interval = 100000;
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 100;
                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0;
                chart1.ChartAreas["ChartArea1"].AxisY.Title = "光吸收系数&吸收功率&速度";
                chart1.ChartAreas["ChartArea1"].AxisY.Interval = 5;
                chart1.ChartAreas["ChartArea1"].AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
                chart1.ChartAreas["ChartArea1"].AxisY2.Maximum = 10000;
                chart1.ChartAreas["ChartArea1"].AxisY2.Minimum = 0;
                chart1.ChartAreas["ChartArea1"].AxisY2.Title = "转速&扭力";
                chart1.ChartAreas["ChartArea1"].AxisY2.Interval = 1000;

                //ref_chart_data(100/(1-1), 10.0f, 50,50.0f);
                //byte[] temparray={0x5c,0x6a,0xff,0x8f};
                //float tempforce = BitConverter.ToSingle(temparray,0);
                //float a = 10;
                //float b = 10f;

                ref_chart_data(0, 0, 0, 0f, 0f);
            }
            catch
            {
            }

        }
        #endregion

        public void Ref_Chart(float XSGL, float GXSXS, float FDJZS, float CS, float NL)
        {
            try
            {
                Invoke(new wtfffff(ref_chart_data), XSGL, GXSXS, FDJZS, CS, NL);
            }
            catch (Exception)
            {

            }
        }

        void ref_chart_data(float XSGL, float GXSXS, float FDJZS, float CS, float NL)
        {
            chart1.Series["XSGL"].Points.AddY(XSGL);
            chart1.Series["GXSXS"].Points.AddY(GXSXS);
            chart1.Series["FDJZS"].Points.AddY(FDJZS);
            chart1.Series["CS"].Points.AddY(CS);
            chart1.Series["NL"].Points.AddY(NL);
        }
        public void Ref_Clear()
        {
            Invoke(new wtds_clr(ref_chart_clear));
        }
        void ref_chart_clear()
        {
            try
            {
                //chart1.Series[SeriesName].Points.RemoveAt(0);
                chart1.Series["XSGL"].Points.RemoveAt(0);
                chart1.Series["GXSXS"].Points.RemoveAt(0);
                chart1.Series["FDJZS"].Points.RemoveAt(0);
                chart1.Series["CS"].Points.RemoveAt(0);
                chart1.Series["NL"].Points.RemoveAt(0);
            }
            catch (Exception)
            {
            }
        }

        public void Clear_Chart()
        {
            BeginInvoke(new wt(clear_chart_data));
            //Ref_Chart(0, 0, 0);
        }

        void clear_chart_data()
        {
            foreach (System.Windows.Forms.DataVisualization.Charting.Series series in chart1.Series)
            {
                series.Points.Clear();
            }
        }

        #region 模拟数据
        public float Get_XSGL()
        {
            Random rd = new Random();
            Thread.Sleep(10);
            //return rd.Next(235, 265) / 10;
            return rd.Next(10, 999) / 10f;
        }

        public float Get_GXSXS()
        {
            Random rd = new Random();
            Thread.Sleep(10);
            //return rd.Next(235, 265) / 10;
            return rd.Next(10, 200) / 10f;
        }

        public float Get_FDJZS()
        {
            Random rd = new Random();
            Thread.Sleep(10);
            //return rd.Next(235, 265) / 10;
            return rd.Next(5000, 10000);
        }

        public float Get_Speed()
        {
            Random rd = new Random();
            Thread.Sleep(10);
            //return rd.Next(235, 265) / 10;
            return rd.Next(690, 710) / 10f;
        }

        public float Get_Force()
        {
            Random rd = new Random();
            //Thread.Sleep(10);
            //return rd.Next(235, 265) / 10;
            return rd.Next(1000, 2000);
        }
        #endregion

        private void button_ss_Click(object sender, EventArgs e)
        {
            try
            {
                if (JC_Status == false)
                {
                    TH_ST = new Thread(Jc_Exe);
                    TH_ST.Start();
                    Th_get_FqandLl = new Thread(Fq_Detect);
                    Th_get_FqandLl.Start();
                    timer_show.Start();
                    JC_Status = true;
                    Jzjs_status = false; fq_getdata = false;
                    //button_yj.Enabled = false;
                    button_ss.Text = "停止检测";
                }
                else
                {
                    TH_ST.Abort();
                    Th_get_FqandLl.Abort();
                    Jzjs_status = false; fq_getdata = false;
                    timer_show.Stop();
                    JC_Status = false;
                    button_ss.Text = "重新检测";
                }

            }
            catch (Exception)
            {
            }
        }
        #region 初检
        public void jzjs_chujian()
        {
            wsd_sure = false;
            chujianIsFinished = false;
            chujianIsOk = false;
            float lljo2 = 0f;
            panel_visible(panel_chujian, true);//显示初检界面
            try//获取环境参数
            {
                Thread.Sleep(1000);
                //Exhaust.Fla502_data Environment = new Exhaust.Fla502_data();
                Exhaust.Flb_100_smoke ydjEnvironment = new Exhaust.Flb_100_smoke();
                Exhaust.yhrRealTimeData yhyEnvironment = new Exhaust.yhrRealTimeData();
                if (IsUseTpTemp)
                {
                    WD = WD;
                    SD = SD;
                    DQY = DQY;
                }
                else if (equipconfig.TempInstrument == "烟度计" && flb_100 != null)
                {
                    flb_100.Set_Measure();
                    Thread.Sleep(1000);
                    if (equipconfig.IsOldMqy200)
                        ydjEnvironment = flb_100.get_DirectData();
                    else
                        ydjEnvironment = flb_100.get_Data();
                    if (ydjEnvironment.WD == 0 && ydjEnvironment.SD == 0 && ydjEnvironment.DQY == 0)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (equipconfig.IsOldMqy200)
                                ydjEnvironment = flb_100.get_DirectData();
                            else
                                ydjEnvironment = flb_100.get_Data();
                            Thread.Sleep(200);
                            if (ydjEnvironment.WD != 0 || ydjEnvironment.SD != 0 && ydjEnvironment.DQY != 0)
                                break;
                        }
                    }
                    WD = ydjEnvironment.WD;
                    SD = ydjEnvironment.SD;
                    DQY = ydjEnvironment.DQY;
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
                else if (equipconfig.TempInstrument == "油耗仪")
                {
                    if (yhy.getTempData(out yhyEnvironment))
                    {
                        WD = yhyEnvironment.HJWD;
                        SD = yhyEnvironment.HJSD;
                        DQY = yhyEnvironment.HJYL;
                    }
                    else if (yhy.getTempData(out yhyEnvironment))
                    {
                        WD = yhyEnvironment.HJWD;
                        SD = yhyEnvironment.HJSD;
                        DQY = yhyEnvironment.HJYL;
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
                        xce_100.readEnvironment();
                        WD = xce_100.temp;
                        SD = xce_100.humidity;
                        DQY = xce_100.airpressure;
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
                        xce_100.readEnvironment();
                        WD = xce_100.temp;
                        SD = xce_100.humidity;
                        DQY = xce_100.airpressure;
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
            textbox_value(textBoxSDSD, SD.ToString("0.0"));
            textbox_value(textBoxSDWD, WD.ToString("0.0"));
            if (lugdownconfig.IfSureTemp)
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
            if (igbt.Speed > 0.5)
            {
                datagridview_msg(dataGridView1, "判定", 0, "×");
                Msg(label_chujiantishi, panel_chujiantishi, "请等待滚筒停止再进行", false);
                return;
            }
            else
                datagridview_msg(dataGridView1, "判定", 0, "√");

            datagridview_msg(dataGridView1, "结果", 1, WD.ToString("0.0"));
            if (WD > 35)
            {
                Msg(label_chujiantishi, panel_chujiantishi, "环境温度不合格,不能进行检测", false);
                datagridview_msg(dataGridView1, "判定", 1, "×");
                return;
            }
            else
                datagridview_msg(dataGridView1, "判定", 1, "√");
            datagridview_msg(dataGridView1, "结果", 2, SD.ToString("0.0"));
            if (SD > 100)
            {
                Msg(label_chujiantishi, panel_chujiantishi, "环境湿度不合格,不能进行检测", false);
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
            Thread.Sleep(100);
            panel_visible(panel_chujian, false);
            chujianIsFinished = true;
            chujianIsOk = true;

        }
        #endregion
        private float glxzxs = 1f;
        public void Jc_Exe()
        {
            opno = 1;
            opcode = 11;
            int flag = 0;
            float Last_Speed = 0;
            float Modulus = 0;
            float velMaxHpRation = 1f;
            int Speed_Count = 0;
            float zdcs = 0f;
            DataRow dr = null;
            try
            {

                statusconfigini.writeGlStatusData(statusconfigIni.ENUM_GL_STATUS.STATUS_DAOWEI,"");
                #region 初检
                Clear_Chart();
                igjzjsIsFinished = false;
                MaxRpm_sure = false;
                Msg(Msg_msg, panel_msg, "测试开始，正在进行初检", true);
                ts1 = "测试开始";
                ts2 = "正在进行初检...";
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("测试即将开始　　", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("　　正在进行初检", 5, equipconfig.Ledxh);
                }
                jzjs_chujian();
                if (chujianIsFinished == false)
                {
                    Msg(Msg_msg, panel_msg, "初检不合格，请检查后重新开始。", true);
                    ts2 = "初检不合格";
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("测试中止　　　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("　　　初检不合格", 5, equipconfig.Ledxh);
                    }
                    JC_Status = false;
                    button_ss.Text = "重新检测";
                    Th_get_FqandLl.Abort();
                    Jzjs_status = false; fq_getdata = false;
                    return;
                }
                Msg(Msg_msg, panel_msg, "初检合格，正在检查烟度计状态", true);
                Thread.Sleep(1000);
                if (lugdownconfig.IsTestYw)
                {
                    ts1 = "读取油温...";
                    Msg(Msg_msg, panel_msg, "读取油温...", false);
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("读取油温...", 4, equipconfig.Ledxh);
                    }
                    Exhaust.Flb_100_smoke Environment = new Exhaust.Flb_100_smoke();
                    if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                        Environment = fla_502.get_DirectData();
                    else
                        Environment = flb_100.get_DirectData();
                    Thread.Sleep(1000);
                    float ywnow = Environment.Yw;
                    if (ywnow < 80)
                    {
                        ts1 = "油温: " + ywnow.ToString("0.0") + " ℃";
                        ts2 = "低于限值,检测中止";
                        Msg(Msg_msg, panel_msg, "油温:" + ywnow.ToString("0.0") + "℃" + "低于限值,检测中止", false);
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("油温低于限值", 4, equipconfig.Ledxh);
                            ledcontrol.writeLed("检测中止", 5, equipconfig.Ledxh);
                        }
                        JC_Status = false;
                        button_ss.Text = "重新检测";
                        Th_get_FqandLl.Abort();
                        Jzjs_status = false; fq_getdata = false;
                        return;
                    }
                    else
                    {
                        ts1 = "油温: " + ywnow.ToString("0.0") + " ℃";
                        ts2 = "允许检测";
                        Msg(Msg_msg, panel_msg, "油温:" + ywnow.ToString("0.0") + "℃" + ",允许检测", false);

                    }
                    Thread.Sleep(1000);
                }
                #endregion

                #region 检查仪器状态
                Thread.Sleep(1000);
                Jc_Process = "VelMaxHP";        //该过程为确定VelMaxHP
                for (int i = 0; i <= 10; i++)                        //检查烟度计状态
                {
                    if (equipconfig.Ydjxh != "nht_1" && equipconfig.Ydjxh.ToLower() != "cdf5000" && equipconfig.Ydjxh != yq_mqw5101)
                    {
                        string zt = flb_100.Get_Mode();
                        if (zt != "通讯故障")
                        {
                            Msg(Msg_msg, panel_msg, "烟度计工作正常", true);
                            break;
                        }
                        else if (i == 9)
                        {
                            ts2 = "通讯故障";
                            Msg(Msg_msg, panel_msg, "烟度计无法正常连接，请检查后重新开始。", true);
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("测试中止　　　　", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("　烟度计通讯故障", 5, equipconfig.Ledxh);
                            }
                            JC_Status = false;
                            Th_get_FqandLl.Abort();
                            button_ss.Text = "重新检测";
                            return;
                        }
                    }
                    else
                    {
                        Msg(Msg_msg, panel_msg, "烟度计工作正常", true);
                        break;
                    }
                    Thread.Sleep(500);
                }

                if (equipconfig.Ydjxh == yq_mqw5101)//如果烟度计型号是mqw5101
                {
                    Exhaust.mqw_5101_status status = new Exhaust.mqw_5101_status();
                    if (flb_100.get_MQW5101_Status(out status))
                    {
                        if (status.isPrepare)
                        {
                            ts2 = "仪器预热中";
                            Msg(Msg_msg, panel_msg, "仪器预热中，预热完毕后重新开始。", true);
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("测试中止　　　　", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("　　　仪器预热中", 5, equipconfig.Ledxh);
                            }
                            JC_Status = false;
                            Th_get_FqandLl.Abort();
                            button_ss.Text = "重新检测";
                            return;
                        }
                        if (status.isNOZeroing)
                        {
                            ts2 = "仪器调零中";
                            Msg(Msg_msg, panel_msg, "仪器调零中，调零完毕后重新开始。", true);
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("测试中止　　　　", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("　　　仪器调零中", 5, equipconfig.Ledxh);
                            }
                            JC_Status = false;
                            Th_get_FqandLl.Abort();
                            button_ss.Text = "重新检测";
                            return;
                        }
                        flb_100.stopAction();
                    }

                }
                if (notester != null)
                {
                    Exhaust.mqw_5101_status status = new Exhaust.mqw_5101_status();
                    if (notester.get_MQW5101_Status(out status))
                    {
                        if (status.isPrepare)
                        {
                            ts2 = "仪器预热中";
                            Msg(Msg_msg, panel_msg, "仪器预热中，预热完毕后重新开始。", true);
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("测试中止　　　　", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("　　　仪器预热中", 5, equipconfig.Ledxh);
                            }
                            JC_Status = false;
                            Th_get_FqandLl.Abort();
                            button_ss.Text = "重新检测";
                            return;
                        }
                        if (status.isNOZeroing)
                        {
                            ts2 = "仪器调零中";
                            Msg(Msg_msg, panel_msg, "仪器调零中，调零完毕后重新开始。", true);
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("测试中止　　　　", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("　　　仪器调零中", 5, equipconfig.Ledxh);
                            }
                            JC_Status = false;
                            Th_get_FqandLl.Abort();
                            button_ss.Text = "重新检测";
                            return;
                        }
                        notester.stopAction();
                    }
                }
                #endregion
               
                #region 烟度计线性校正中
                ts2 = "校正烟度计...";
                if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                {
                    fla_502.set_linearDem();
                    Msg(Msg_msg, panel_msg, "烟度计正在进行线性校正...", false);
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("线性校正中... ", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("......  　　", 5, equipconfig.Ledxh);
                    }
                    Thread.Sleep(8000);//等待5s至线性校正结束
                }
                else if (equipconfig.Ydjxh != yq_mqw5101)
                {
                    flb_100.set_linearDem();//烟度计进行线性校正                
                    Msg(Msg_msg, panel_msg, "烟度计正在进行线性校正...", false);
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("线性校正中... ", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("......  　　", 5, equipconfig.Ledxh);
                    }
                    Thread.Sleep(8000);//等待5s至线性校正结束
                }
                #endregion
                
                #region 仪器调零
                if (equipconfig.Ydjxh == yq_mqw5101)
                {
                    if (!flb_100.zeroEquipment())
                    {
                        ts2 = "启动调零失败";
                        Msg(Msg_msg, panel_msg, "启动调零失败。", true);
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("测试中止　　　　", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("　　启动调零失败", 5, equipconfig.Ledxh);
                        }
                        JC_Status = false;
                        Th_get_FqandLl.Abort();
                        button_ss.Text = "重新检测";
                        return;
                    }
                    Exhaust.mqw_5101_status status = new Exhaust.mqw_5101_status();
                    status.isNOZeroing = true;
                    int zero_count = 0;
                    while (status.isNOZeroing)//等待调零结束
                    {
                        Thread.Sleep(900);
                        flb_100.get_MQW5101_Status(out status);
                        Msg(Msg_msg, panel_msg, "NO调零中..." + zero_count.ToString() + "s", true);
                        ts2 = "NO调零中..." + zero_count.ToString() + "s";
                        zero_count++;
                        if (zero_count == 60)
                            break;
                    }
                    Msg(Msg_msg, panel_msg, "NO调零完毕", true);
                    ts2 = "NO调零完毕";
                    Thread.Sleep(1000);
                }
                if (notester != null)
                {
                    if (!notester.zeroEquipment())
                    {
                        ts2 = "启动调零失败";
                        Msg(Msg_msg, panel_msg, "启动调零失败。", true);
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("测试中止　　　　", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("　　启动调零失败", 5, equipconfig.Ledxh);
                        }
                        JC_Status = false;
                        Th_get_FqandLl.Abort();
                        button_ss.Text = "重新检测";
                        return;
                    }
                    Exhaust.mqw_5101_status status = new Exhaust.mqw_5101_status();
                    status.isNOZeroing = true;
                    int zero_count = 0;
                    while (status.isNOZeroing)//等待调零结束
                    {
                        Thread.Sleep(900);
                        notester.get_MQW5101_Status(out status);
                        Msg(Msg_msg, panel_msg, "NO调零中..." + zero_count.ToString() + "s", true);
                        ts2 = "NO调零中..." + zero_count.ToString() + "s";
                        zero_count++;
                        if (zero_count == 60)
                            break;
                    }
                    Msg(Msg_msg, panel_msg, "NO调零完毕", true);
                    ts2 = "NO调零完毕";
                    Thread.Sleep(1000);
                }
                #endregion
                
                #region 仪器开始测量
                if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                    fla_502.Set_Measure();
                else if (equipconfig.Ydjxh != yq_mqw5101)
                    flb_100.Set_Measure();
                if (equipconfig.Ydjxh == yq_mqw5101)
                {
                    if (!flb_100.pumpVehicleGas())
                        if (!flb_100.pumpVehicleGas())
                            flb_100.pumpVehicleGas();
                }
                if (notester != null)
                {
                    if (!notester.pumpVehicleGas())
                        if (!notester.pumpVehicleGas())
                            notester.pumpVehicleGas();
                }
                #endregion
                
                #region 测功机准备
                //Thread.Sleep(1000);                     //烟度计要求等待1秒后再发送数据
                igbt.Exit_Control();            //退出igbt的所有控制状态
                //igbt.Lifter_Down();//举升下降
                //Msg(Msg_msg, panel_msg, "举升下降", false);
                //ts2 = "举升下降";
                Thread.Sleep(1000);
                #endregion
                
                #region 安置探头
                Msg(Msg_msg, panel_msg, "请安置不透光烟度计尾气探头", false);
                ts2 = "请安置探头";
                Thread.Sleep(1000);
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("测试即将开始　　", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("　请安置好转速计", 5, equipconfig.Ledxh);
                }
                if (lugdownconfig.IfSureTemp)
                {
                    MessageBox.Show("确认探头是否已安好?", "系统提示");
                }
                else
                {
                    for (int i = 15; i >= 0; i--)
                    {
                        Msg(Msg_msg, panel_msg, "请安置好探头..." + i.ToString(), false);
                        Thread.Sleep(800);
                    }
                }
                statusconfigini.writeGlStatusData(statusconfigIni.ENUM_GL_STATUS.STATUS_TANTOU, "");
                #endregion

                #region 开始检测，提示回事
                fq_getdata = true;
                ts1 = "检测开始";
                ts2 = "请加速至70km/h左右";
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("　　检测开始　　", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("加速至70km/h ", 5, equipconfig.Ledxh);
                }
                //MessageBox.Show("确认烟度计探头是否已安好?", "系统提示");

                sxnb = 0;//时序类别设为0
                cysx = 1;//采样时序设为1
                Thread.Sleep(2000);
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("　速度　　功率　", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("　　　　　　　　", 5, equipconfig.Ledxh);
                    Thread.Sleep(200);
                }

                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.DAOWEI, GKSJ.ToString());
                Thread.Sleep(2000);
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.CHATANTOU, GKSJ.ToString());
                startTime = DateTime.Now;
                Jzjs_status = true;
                #endregion

                if (!isUsedata)
                {
                    #region 加速阶段
                    Msg(Msg_msg, panel_msg, "测试开始，请用合适档位全油加速至70km/h以上", true);
                    statusconfigini.writeNeuStatusData("StartTest", DateTime.Now.ToString());
                    statusconfigini.writeGlStatusData(statusconfigIni.ENUM_GL_STATUS.STATUS_STARTSAMPLE, "");
                    jzjs_data.StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    while (igbt.Speed < lugdownconfig.MinSpeed)
                    {
                        Thread.Sleep(500);
                    }
                    if (lugdownconfig.IfSureTemp)
                    {
                        setButtonVisible(button1, true);
                        Msg(Msg_msg, panel_msg, "待转速稳定后点击[确定最大转速]按钮开始功率扫描", true);
                        while (MaxRpm_sure == false)
                        {
                            Thread.Sleep(100);
                        }
                        setButtonVisible(button1, false);
                    }
                    else
                    {
                        int stableTime = 0;
                        float prespeed = 0;
                        while (stableTime < 30)
                        {
                            Msg(Msg_msg, panel_msg, "测试开始，请用合适档位全油加速至70km/h以上", true);
                            if (Math.Abs(prespeed - igbt.Speed) > 1f)
                            {
                                stableTime = 0;
                                prespeed = igbt.Speed;
                            }
                            else
                            {
                                stableTime++;
                            }
                            Thread.Sleep(100);
                        }
                    }
                    #endregion

                    #region 判断转速
                    MaxRpm_sure = false;
                    MaxRPM = ZS;//确定最大转速
                    if (Math.Abs(MaxRPM - carbj.CarEdzs) > 500)
                    {
                        Random rd = new Random();
                        MaxRPM = carbj.CarEdzs + DateTime.Now.Second * 10 - 300;
                        if (MaxRPM < 0) MaxRPM = 0;
                    }
                    jzjs_data.Velmaxhpzs = MaxRPM.ToString();
                    jzjs_data.Lbzs = MaxRPM.ToString();
                    led_display(ledNumber_ZDZS, MaxRPM.ToString("0"));
                    #endregion
                    
                    #region 判断速度
                    velMaxHpRation = MaxRPM / carbj.CarEdzs;
                    VelMaxHP_real = igbt.Speed;
                    jzjs_data.Velmaxhp = VelMaxHP_real.ToString();
                    zdcs = VelMaxHP_real;
                    Control_Speed = igbt.Speed; //计算VelMaxPH
                    if (Control_Speed < lugdownconfig.MinSpeed || Control_Speed > lugdownconfig.MaxSpeed)
                    {
                        Msg(Msg_msg, panel_msg, "最大速度不正常，请调整档位重新开始。", true);
                        ts1 = "检测终止";
                        ts2 = "最大速度不正常";
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("　　检测终止　　", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("　最大速度不正常", 5, equipconfig.Ledxh);
                        }
                        JC_Status = false;
                        button_ss.Text = "重新检测";
                        Th_get_FqandLl.Abort();
                        Jzjs_status = false;
                        fq_getdata = false;
                        return;
                    }
                    #endregion
                    
                    #region 功率扫描
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("　开始功率扫描　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                    }
                    opno = 2;
                    opcode = 12;
                    Msg(Msg_msg, panel_msg, "最大转速已确定，开始功率扫描，保持油门全开", true);
                    ts1 = "开始功率扫描";
                    ts2 = "保持油门全开";
                    Thread.Sleep(1000);
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("　速度　　功率　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                    }
                    if (lugdownconfig.Glsmms == "恒速度")
                    {
                        #region 恒速度功率扫描
                        igbt.Set_Speed(Control_Speed);
                        igbt.Start_Control_Speed();
                        Msg(Msg_msg, panel_msg, "开始功率扫描，等待速度稳定", true);
                        while (JC_Status)                           //等待速度稳定2秒
                        {
                            if (Math.Abs(igbt.Speed - Control_Speed) <= 1)
                                flag++;
                            else
                                flag = 0;
                            if (flag >= 20)
                                break;
                            Thread.Sleep(100);
                        }
                        flag = 0;
                        MaxP = 0f;
                        sxnb = 1;//功率扫描开始
                        while (JC_Status)               //功率扫描
                        {
                            if (lugdownconfig.isYdjk_glsm)
                            {
                                if (Smoke < lugdownconfig.ydjk_cl_value)
                                {
                                    JC_Status = false;
                                    button_ss.Text = "重新检测";
                                    Th_get_FqandLl.Abort();
                                    Jzjs_status = false; fq_getdata = false;
                                    Thread.Sleep(500);
                                    Msg(Msg_msg, panel_msg, "烟度值过低,检测中止,检测探头是否脱落。", true);
                                    ts1 = "烟度值过低";
                                    ts2 = "检测中止";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("　　检测终止　　", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed("检测探头是否脱落", 5, equipconfig.Ledxh);
                                    }
                                    igbt.Exit_Control();

                                    return;
                                }
                            }
                            Msg(Msg_msg, panel_msg, "正在进行功率扫描，当前速度：" + igbt.Speed.ToString("0.0") + "km/h", true);
                            ts2 = "速度:" + igbt.Speed.ToString("0.0") + "功率:" + nowpower.ToString("0.0");
                            GL = (float)nowpower;
                            flag++;
                            Thread.Sleep(100);
                            if (flag % 8 == 0)         //每隔一秒速度减少0.5km/h
                            {
                                Control_Speed -= 0.5f;
                                igbt.Set_Speed(Control_Speed);
                            }
                            while (Math.Abs(igbt.Speed - Control_Speed) > lugdownconfig.Sdwdqj)
                            {
                                Thread.Sleep(100);
                            }
                            if (igbt.Speed < (zdcs - 1f))
                            {
                                if (MaxP < GL)
                                {
                                    MaxP = GL;
                                    VelMaxHP_real = igbt.Speed;//真实VelMaxHP
                                    MaxZ = ZS;
                                }
                                else
                                {
                                    if ((VelMaxHP_real - igbt.Speed) > VelMaxHP_real * 0.2f)                         //实际VelMaxHP,功率扫描完成
                                        break;
                                }
                            }
                            if (igbt.Speed <= 40)
                                break;
                        }
                        #endregion
                    }
                    else
                    {
                        #region 恒功率功率扫描
                        Control_Speed = 0f;
                        igbt.Set_Control_Force(Control_Speed);
                        igbt.Start_Control_Force();
                        Msg(Msg_msg, panel_msg, "开始功率扫描，保持油门全开", true);
                        MaxP = 0f;
                        sxnb = 1;//功率扫描开始
                        while (JC_Status)               //功率扫描
                        {
                            if (lugdownconfig.isYdjk_glsm)
                            {
                                if (Smoke < lugdownconfig.ydjk_cl_value)
                                {
                                    JC_Status = false;
                                    button_ss.Text = "重新检测";
                                    Th_get_FqandLl.Abort();
                                    Jzjs_status = false; fq_getdata = false;
                                    Thread.Sleep(500);
                                    Msg(Msg_msg, panel_msg, "烟度值过低,检测中止,检测探头是否脱落。", true);
                                    ts1 = "烟度值过低";
                                    ts2 = "检测中止";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("　　检测终止　　", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed("检测探头是否脱落", 5, equipconfig.Ledxh);
                                    }
                                    igbt.Exit_Control();

                                    return;
                                }
                            }
                            Msg(Msg_msg, panel_msg, "正在进行功率扫描，当前速度：" + igbt.Speed.ToString("0.0") + "km/h", true);
                            ts2 = "速度:" + igbt.Speed.ToString("0.0") + "功率:" + nowpower.ToString("0.0");
                            GL = (float)nowpower;
                            flag++;
                            Thread.Sleep(100);
                            if (flag % 5 == 0)         //每隔一秒扭力增加lugdownconfig.Smpl
                            {
                                Control_Speed += lugdownconfig.Smpl / 2f;
                                igbt.Set_Control_Force(Control_Speed);
                            }
                            if (MaxP < GL)
                            {
                                MaxP = GL;
                                VelMaxHP_real = igbt.Speed;//真实VelMaxHP
                                MaxZ = ZS;
                            }
                            else
                            {
                                if ((VelMaxHP_real - igbt.Speed) > VelMaxHP_real * 0.2f)                         //实际VelMaxHP,功率扫描完成
                                    break;
                            }
                            if (igbt.Speed <= 40)
                                break;
                        }
                        #endregion
                    }
                    #endregion

                    #region 核对最大轮边功率
                    igbt.Exit_Control();
                    igbt.Exit_Control();
                    VelMaxHP_real = 0;
                    MaxP = 0;
                    for (int timecount = 0; timecount <= GKSJ - 1; timecount++)
                    {
                        if (ZGLlist[timecount] > MaxP)
                        {
                            MaxP = ZGLlist[timecount];
                            VelMaxHP_real = Speedlist[timecount];
                        }
                    }
                    opno = 3;
                    opcode = 13;
                    glxzxs = dcf;
                    VelMaxHP = VelMaxHP_real;//实际velMaxHp
                    jzjs_data.RealVelmaxhp = VelMaxHP_real.ToString();
                    jzjs_data.actualmaxhp = MaxP.ToString("0.0");
                    jzjs_data.glxzxs = glxzxs.ToString("0.000");
                    if (lugdownconfig.LugdownMaxHpStyle == 0)
                    {
                        jzjs_data.Lbgl = (MaxP * glxzxs).ToString("0.0");
                        led_display(ledNumberLBGL, jzjs_data.Lbgl);
                        led_display(ledNumberDCF, jzjs_data.glxzxs);
                        if (equipconfig.useJHJK)//金华监控到轮边功率大于额定功率的80%，即限值的160%中断检测
                        {
                            if (double.Parse(jzjs_data.Lbgl) > carbj.Xz1 * 0.02 * equipconfig.JHLBGLB)
                            {
                                JC_Status = false;
                                button_ss.Text = "重新检测";
                                Th_get_FqandLl.Abort();
                                Jzjs_status = false; fq_getdata = false;
                                Thread.Sleep(500);
                                Msg(Msg_msg, panel_msg, "轮边功率异常预警，检测中止。", true);
                                ts1 = "轮边功率异常预警";
                                ts2 = "检测中止";
                                if (ledcontrol != null)
                                {
                                    ledcontrol.writeLed("　　检测终止　　", 2, equipconfig.Ledxh);
                                    Thread.Sleep(200);
                                    ledcontrol.writeLed("轮边功率异常预警", 5, equipconfig.Ledxh);
                                }
                                igbt.Exit_Control();
                                return;
                            }
                        }
                    }
                    Msg(Msg_msg, panel_msg, "功率扫描完成，最大轮边功率:" + jzjs_data.Lbgl + "kW,VelMaxHP为" + VelMaxHP_real.ToString("0.0") + "km/h", true);
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("　功率扫描结束　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                    }
                    ts1 = "功率扫描结束";
                    ts2 = "HP:" + jzjs_data.Lbgl + "VelHP:" + VelMaxHP_real.ToString("0.0");
                    led_display(ledNumber_JSVEL, (VelMaxHP_real * velMaxHpRation).ToString("0.0"));
                    led_display(ledNumber_SJVEL, VelMaxHP.ToString("0.0"));
                    //led_display(ledNumberLBGL, HP);
                    statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIANCEZHONG, GKSJ.ToString());
                    Thread.Sleep(1000);//显示时间
                    if (equipconfig.useJHJK && carbj.CarZzl >= 3500)//金华重型车最大车速如果不在60-100之间则中止检测
                    {
                        if (VelMaxHP_real < 60 || VelMaxHP_real > 100)
                        {
                            JC_Status = false;
                            button_ss.Text = "重新检测";
                            Th_get_FqandLl.Abort();
                            Jzjs_status = false; fq_getdata = false;
                            Thread.Sleep(500);
                            Msg(Msg_msg, panel_msg, "最高车速不正常，检测中止。", true);
                            ts1 = "最高车速不正常";
                            ts2 = "检测中止";
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("　　检测终止　　", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("最高车速不正常", 5, equipconfig.Ledxh);
                            }
                            igbt.Exit_Control();
                            return;
                        }
                    }
                    if (lugdownconfig.gsMaxPPD)
                    {
                        jzjs_data.Lbgl = (MaxP * glxzxs).ToString("0.0");
                        if (double.Parse(jzjs_data.Lbgl) < carbj.Xz1)
                        {
                            ts1 = "轮边功率不合格";
                            ts2 = "测试结束";
                            HK = "";
                            NK = "";
                            EK = "";
                            jzjs_data.Rev100 = "";
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("功率过低测试结束", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                            }
                            Msg(Msg_msg, panel_msg, "最大轮边功率不合格，测试结束", true);
                            Thread.Sleep(2000);
                            goto Finish;
                        }
                    }
                    #endregion

                    if (equipconfig.DATASECONDS_TYPE == "江西" || equipconfig.DATASECONDS_TYPE == "云南保山")
                        sxnb = 2;

                    #region 加载测试
                    ts1 = "加载测试开始";
                    ts2 = "保持油门全开";
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("　加载测试开始　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                    }
                    Msg(Msg_msg, panel_msg, "扫描完毕，开始加载测试，保持油门全开", true);
                    Thread.Sleep(1000);
                    opno = 4;
                    opcode = 14;
                    if (lugdownconfig.Glsmms == "恒功率")
                    {
                        igbt.Set_Control_Force(0f);
                        Thread.Sleep(200);
                    }
                    Jc_Process = "VelMaxHP100%";
                    flag = 0;
                    while (JC_Status)
                    {
                        ts1 = Jc_Process + "测试";
                        float temp_speed = 0;
                        float temp_force = 0;
                        float temp_gl = 0;
                        float temp_zs = 0;
                        float temp_gxxs = 0;
                        float temp_no = 0;
                        switch (Jc_Process)
                        {
                            case "VelMaxHP100%":
                                statusconfigini.writeNeuStatusData("K100Testing", DateTime.Now.ToString());
                                Modulus = 1;
                                if (equipconfig.DATASECONDS_TYPE != "江西"&&equipconfig.DATASECONDS_TYPE!="云南保山")
                                    sxnb = 2;
                                opno = 5;
                                opcode = 21;
                                break;
                            case "VelMaxHP90%":
                                statusconfigini.writeNeuStatusData("K90Testing", DateTime.Now.ToString());
                                Modulus = 0.9f;
                                if (equipconfig.DATASECONDS_TYPE == "江西" || equipconfig.DATASECONDS_TYPE == "云南保山")
                                    sxnb = 4;
                                else
                                    sxnb = 3;
                                opno = 9;
                                opcode = 31;
                                break;
                            case "VelMaxHP80%":
                                statusconfigini.writeNeuStatusData("K80Testing", DateTime.Now.ToString());
                                Modulus = 0.8f;
                                if (equipconfig.DATASECONDS_TYPE == "江西" || equipconfig.DATASECONDS_TYPE == "云南保山")
                                    sxnb = 5;
                                else
                                    sxnb = 4;
                                opno = 13;
                                opcode = 41;
                                break;
                        }
                        igbt.Set_Speed(VelMaxHP_real * Modulus);
                        Thread.Sleep(200);
                        igbt.Start_Control_Speed();

                        if ((equipconfig.DATASECONDS_TYPE == "江西"|| equipconfig.DATASECONDS_TYPE == "云南保山") && Jc_Process == "VelMaxHP100%")
                        {
                            while (igbt.Speed < VelMaxHP_real - 1)
                            {
                                if (ledcontrol != null)
                                    ledcontrol.writeLed("恢复100%VelMaxHP", 2, equipconfig.Ledxh);
                                Msg(Msg_msg, panel_msg, "恢复100%VelMaxHP过程", true);
                                Thread.Sleep(200);
                            }
                            sxnb = 3;
                        }

                        Msg(Msg_msg, panel_msg, "正在进行加载减速测试，将在" + Math.Round(VelMaxHP_real * Modulus, 2).ToString("0.0") + "km/h时取值", true);
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("将在" + addLength(Math.Round(VelMaxHP_real * Modulus, 2).ToString("0.0"), 4) + "处取值 ", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                        }
                        ts2 = "将在" + Math.Round(VelMaxHP_real * Modulus, 2).ToString("0.0") + "km/h取值";
                        Speed_Count = 0;
                        while (JC_Status)
                        {
                            switch (Jc_Process)
                            {
                                case "VelMaxHP100%":
                                    opno = 6;
                                    opcode = 22;
                                    break;
                                case "VelMaxHP90%":
                                    opno = 10;
                                    opcode = 32;
                                    break;
                                case "VelMaxHP80%":
                                    opno = 14;
                                    opcode = 42;
                                    break;
                            }
                            Thread.Sleep(100);
                            if (Math.Abs(igbt.Speed - VelMaxHP_real * Modulus) < lugdownconfig.Sdwdqj)            //如果离目标速度小于1时
                            {
                                Speed_Count++;
                            }
                            else
                            {
                                //Speed_Count = 0;//如果大于1时，则表示未稳定
                                continue;
                            }
                            if (Speed_Count >= 30)
                                break;
                        }
                        if (Speed_Count >= 30)
                        {
                            //取值
                            switch (Jc_Process)
                            {
                                case "VelMaxHP100%":
                                    opno = 7;
                                    opcode = 23;
                                    break;
                                case "VelMaxHP90%":
                                    opno = 11;
                                    opcode = 33;
                                    break;
                                case "VelMaxHP80%":
                                    opno = 15;
                                    opcode = 43;
                                    break;
                            }
                            for (int i = 3; i > 0; i--)
                            {
                                Msg(Msg_msg, panel_msg, "将在" + i.ToString() + "s后取值", false);
                                ts2 = "将在" + i.ToString() + "s后取值";
                                Thread.Sleep(900);
                            }
                            Speed_Count = 0;
                            temp_force = 0;
                            temp_gl = 0;
                            temp_gxxs = 0;
                            temp_speed = 0;
                            temp_zs = 0;
                            temp_no = 0;
                            for (int i = 1; i <= 5; i++)//取5s内的数据，以10HZ的采样率
                            {
                                Msg(Msg_msg, panel_msg, "正在取值" + "(" + Jc_Process + ") " + i.ToString(), false);
                                ts2 = "正在取值..." + (6 - i).ToString() + "s";
                                if (ledcontrol != null)
                                {
                                    ledcontrol.writeLed("正在取值..." + (6 - i).ToString() + "s ", 2, equipconfig.Ledxh);
                                }
                                temp_speed += igbt.Speed;
                                temp_force += igbt.Force;
                                temp_gl += (float)nowpower;
                                temp_gxxs += Smoke;
                                temp_zs += ZS;
                                temp_no += No;
                                Thread.Sleep(900);
                            }
                            switch (Jc_Process)
                            {
                                case "VelMaxHP100%":
                                    opno = 8;
                                    opcode = 24;
                                    Speed_Count = 0;
                                    double glsum = 0;

                                    HP = (temp_gl / 5f).ToString("0.00");
                                    HK = (temp_gxxs / 5f).ToString("0.00");
                                    HV = (temp_zs / 5f).ToString("0");
                                    HNo = (temp_no / 5f).ToString("0");
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("测试结果:" + addLength(HK, 4) + " ", 2, equipconfig.Ledxh);
                                    }
                                    led_display(ledNumberGX_H, HK);
                                    jzjs_data.Rev100 = HV;
                                    if (lugdownconfig.LugdownMaxHpStyle == 0)
                                    {
                                        Msg(Msg_msg, panel_msg, "速度段：" + Jc_Process + "测试完成，测试结果：" + HK, true);
                                        ts2 = "结果：" + HK;
                                    }
                                    else
                                    {
                                        for (int timecount = GKSJ - 5; timecount <= GKSJ - 1; timecount++)
                                        {
                                            glsum += ZGLlist[timecount];
                                        }
                                        double glxz = glsum * glxzxs / 5.0;
                                        jzjs_data.Lbgl = glxz.ToString("0.0");
                                        led_display(ledNumberLBGL, jzjs_data.Lbgl);
                                        led_display(ledNumberDCF, jzjs_data.glxzxs);
                                        Msg(Msg_msg, panel_msg, "速度段：" + Jc_Process + "测试完成，修正功率:" + jzjs_data.Lbgl + "kW,K:" + HK, true);
                                        ts2 = "功率:" + jzjs_data.Lbgl + " K:" + HK;
                                        if (equipconfig.useJHJK)//金华监控到轮边功率大于额定功率的80%，即限值的160%中断检测
                                        {
                                            if (double.Parse(jzjs_data.Lbgl) > carbj.Xz1 * 0.02 * equipconfig.JHLBGLB)
                                            {
                                                JC_Status = false;
                                                button_ss.Text = "重新检测";
                                                Th_get_FqandLl.Abort();
                                                Jzjs_status = false; fq_getdata = false;
                                                Thread.Sleep(500);
                                                Msg(Msg_msg, panel_msg, "轮边功率异常预警，检测中止。", true);
                                                ts1 = "轮边功率异常预警";
                                                ts2 = "检测中止";
                                                if (ledcontrol != null)
                                                {
                                                    ledcontrol.writeLed("　　检测终止　　", 2, equipconfig.Ledxh);
                                                    Thread.Sleep(200);
                                                    ledcontrol.writeLed("轮边功率异常预警", 5, equipconfig.Ledxh);
                                                }
                                                igbt.Exit_Control();
                                                return;
                                            }
                                        }
                                    }
                                    /*if (double.Parse(HK)<0.01)
                                    {
                                        JC_Status = false;
                                        button_ss.Text = "重新检测";
                                        Th_get_FqandLl.Abort();
                                        Jzjs_status = false;
                                        Thread.Sleep(500);
                                        Msg(Msg_msg, panel_msg, "烟度值为0，检测中止。", true);
                                        ts1 = "烟度值为0";
                                        ts2 = "检测中止";
                                        if (ledcontrol != null)
                                        {
                                            ledcontrol.writeLed("　　检测终止　　", 2);
                                            Thread.Sleep(200);
                                            ledcontrol.writeLed("检测探头是否脱落", 5);
                                        }
                                        
                                        return;
                                    }*/
                                    Thread.Sleep(1000);
                                    Jc_Process = "VelMaxHP90%";
                                    if (lugdownconfig.gsKcbPD)
                                    {
                                        if (double.Parse(HK) > carbj.Xz2)
                                        {
                                            ts1 = "光吸收系数超标";
                                            ts2 = "测试结束";
                                            NK = "";
                                            EK = "";
                                            if (ledcontrol != null)
                                            {
                                                ledcontrol.writeLed("k超标  检测结束", 2, equipconfig.Ledxh);
                                                Thread.Sleep(200);
                                            }
                                            Msg(Msg_msg, panel_msg, "光吸收系数超标，测试结束", true);
                                            Thread.Sleep(2000);
                                            goto Finish;
                                        }
                                    }
                                    if (lugdownconfig.gsKhgPD)
                                    {
                                        if (double.Parse(HK) <= Math.Round(carbj.Xz2 * 0.9, 2))
                                        {
                                            ts1 = "k小于限值90%";
                                            ts2 = "测试结束";
                                            NK = "";
                                            EK = "";
                                            if (ledcontrol != null)
                                            {
                                                ledcontrol.writeLed("k合格  检测结束", 2, equipconfig.Ledxh);
                                                Thread.Sleep(200);
                                            }
                                            Msg(Msg_msg, panel_msg, "k小于限值90%，测试结束", true);
                                            Thread.Sleep(2000);
                                            goto Finish;
                                        }
                                    }
                                    if (equipconfig.useJHJK)
                                    {
                                        if (double.Parse(HK) == 0)
                                        {
                                            JC_Status = false;
                                            button_ss.Text = "重新检测";
                                            Th_get_FqandLl.Abort();
                                            Jzjs_status = false; fq_getdata = false;
                                            Thread.Sleep(500);
                                            Msg(Msg_msg, panel_msg, "烟度值为0，检测中止。", true);
                                            ts1 = "烟度值为0";
                                            ts2 = "检测中止";
                                            if (ledcontrol != null)
                                            {
                                                ledcontrol.writeLed("　　检测终止　　", 2, equipconfig.Ledxh);
                                                Thread.Sleep(200);
                                                ledcontrol.writeLed("检测探头是否脱落", 5, equipconfig.Ledxh);
                                            }
                                            igbt.Exit_Control();

                                            return;
                                        }
                                    }
                                    if (lugdownconfig.isYdjk_cl)
                                    {
                                        if (double.Parse(HK) < lugdownconfig.ydjk_cl_value)
                                        {
                                            JC_Status = false;
                                            button_ss.Text = "重新检测";
                                            Th_get_FqandLl.Abort();
                                            Jzjs_status = false; fq_getdata = false;
                                            Thread.Sleep(500);
                                            Msg(Msg_msg, panel_msg, "烟度值过低,检测中止,检查探头是否脱落", true);
                                            ts1 = "烟度值过低";
                                            ts2 = "检测中止";
                                            if (ledcontrol != null)
                                            {
                                                ledcontrol.writeLed("　　检测终止　　", 2, equipconfig.Ledxh);
                                                Thread.Sleep(200);
                                                ledcontrol.writeLed("检测探头是否脱落", 5, equipconfig.Ledxh);
                                            }
                                            igbt.Exit_Control();

                                            return;
                                        }
                                    }
                                    if (lugdownconfig.LugdownGljk)
                                    {
                                        if (double.Parse(HP)*glxzxs<double.Parse(jzjs_data.Lbgl)*lugdownconfig.Lugdown_Gljk_value*0.01)
                                        {
                                            JC_Status = false;
                                            button_ss.Text = "重新检测";
                                            Th_get_FqandLl.Abort();
                                            Jzjs_status = false; fq_getdata = false;
                                            Thread.Sleep(500);
                                            Msg(Msg_msg, panel_msg, "加载功率过低,检测中止", true);
                                            ts1 = "加载功率过低";
                                            ts2 = "检测中止";
                                            if (ledcontrol != null)
                                            {
                                                ledcontrol.writeLed("　　检测中止　　", 2, equipconfig.Ledxh);
                                                Thread.Sleep(200);
                                                ledcontrol.writeLed("加载功率低于限值", 5, equipconfig.Ledxh);
                                            }
                                            igbt.Exit_Control();

                                            return;
                                        }
                                    }
                                    break;
                                case "VelMaxHP90%":

                                    opno = 12;
                                    opcode = 34;
                                    Speed_Count = 0;
                                    NV = (temp_zs / 5f).ToString("0");
                                    NP = (temp_gl / 5f).ToString("0.00");
                                    NK = (temp_gxxs / 5f).ToString("0.00");
                                    NNo = (temp_no / 5f).ToString("0");
                                    Msg(Msg_msg, panel_msg, "速度段：" + Jc_Process + "测试完成，测试结果：" + NK, true);
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("测试结果:" + addLength(NK, 4) + " ", 2, equipconfig.Ledxh);
                                    }
                                    ts2 = "结果：" + NK;
                                    led_display(ledNumberGX_N, NK);
                                    /*if (double.Parse(NK) < 0.01)
                                    {
                                        JC_Status = false;
                                        button_ss.Text = "重新检测";
                                        Th_get_FqandLl.Abort();
                                        Jzjs_status = false;
                                        Thread.Sleep(500);
                                        Msg(Msg_msg, panel_msg, "烟度值为0，检测中止。", true);
                                        ts1 = "烟度值为0";
                                        ts2 = "检测中止";
                                        if (ledcontrol != null)
                                        {
                                            ledcontrol.writeLed("　　检测终止　　", 2);
                                            Thread.Sleep(200);
                                            ledcontrol.writeLed("检测探头是否脱落", 5);
                                        }

                                        return;
                                    }*/
                                    Thread.Sleep(1000);
                                    Jc_Process = "VelMaxHP80%";
                                    if (lugdownconfig.gsKcbPD)
                                    {
                                        if (double.Parse(NK) > carbj.Xz2)
                                        {
                                            ts1 = "光吸收系数超标";
                                            ts2 = "测试结束";
                                            EK = "0";
                                            if (ledcontrol != null)
                                            {
                                                ledcontrol.writeLed("k超标  检测结束", 2, equipconfig.Ledxh);
                                                Thread.Sleep(200);
                                            }
                                            Msg(Msg_msg, panel_msg, "光吸收系数超标，测试结束", true);
                                            Thread.Sleep(2000);
                                            goto Finish;
                                        }
                                    }
                                    if (lugdownconfig.gsKhgPD)
                                    {
                                        if (double.Parse(NK) <= Math.Round(carbj.Xz2 * 0.9, 2))
                                        {
                                            ts1 = "k小于限值90%";
                                            ts2 = "测试结束";
                                            EK = "";
                                            if (ledcontrol != null)
                                            {
                                                ledcontrol.writeLed("k合格  检测结束", 2, equipconfig.Ledxh);
                                                Thread.Sleep(200);
                                            }
                                            Msg(Msg_msg, panel_msg, "k小于限值90%，测试结束", true);
                                            Thread.Sleep(2000);
                                            goto Finish;
                                        }
                                    }
                                    if (equipconfig.useJHJK)
                                    {
                                        if (double.Parse(NK) == 0)
                                        {
                                            JC_Status = false;
                                            button_ss.Text = "重新检测";
                                            Th_get_FqandLl.Abort();
                                            Jzjs_status = false; fq_getdata = false;
                                            Thread.Sleep(500);
                                            Msg(Msg_msg, panel_msg, "烟度值为0，检测中止。", true);
                                            ts1 = "烟度值为0";
                                            ts2 = "检测中止";
                                            if (ledcontrol != null)
                                            {
                                                ledcontrol.writeLed("　　检测终止　　", 2, equipconfig.Ledxh);
                                                Thread.Sleep(200);
                                                ledcontrol.writeLed("检测探头是否脱落", 5, equipconfig.Ledxh);
                                            }
                                            igbt.Exit_Control();

                                            return;
                                        }
                                    }
                                    if (lugdownconfig.isYdjk_cl)
                                    {
                                        if (double.Parse(NK) < lugdownconfig.ydjk_cl_value)
                                        {
                                            JC_Status = false;
                                            button_ss.Text = "重新检测";
                                            Th_get_FqandLl.Abort();
                                            Jzjs_status = false; fq_getdata = false;
                                            Thread.Sleep(500);
                                            Msg(Msg_msg, panel_msg, "烟度值过低,检测中止,检查探头是否脱落", true);
                                            ts1 = "烟度值过低";
                                            ts2 = "检测中止";
                                            if (ledcontrol != null)
                                            {
                                                ledcontrol.writeLed("　　检测终止　　", 2, equipconfig.Ledxh);
                                                Thread.Sleep(200);
                                                ledcontrol.writeLed("检测探头是否脱落", 5, equipconfig.Ledxh);
                                            }
                                            igbt.Exit_Control();

                                            return;
                                        }
                                    }
                                    if (lugdownconfig.LugdownGljk)
                                    {
                                        if (double.Parse(NP) * glxzxs < double.Parse(jzjs_data.Lbgl) * lugdownconfig.Lugdown_Gljk_value * 0.01)
                                        {
                                            JC_Status = false;
                                            button_ss.Text = "重新检测";
                                            Th_get_FqandLl.Abort();
                                            Jzjs_status = false; fq_getdata = false;
                                            Thread.Sleep(500);
                                            Msg(Msg_msg, panel_msg, "加载功率过低，检测中止。", true);
                                            ts1 = "加载功率过低";
                                            ts2 = "检测中止";
                                            if (ledcontrol != null)
                                            {
                                                ledcontrol.writeLed("　　检测中止　　", 2, equipconfig.Ledxh);
                                                Thread.Sleep(200);
                                                ledcontrol.writeLed("加载功率低于限值", 5, equipconfig.Ledxh);
                                            }
                                            igbt.Exit_Control();

                                            return;
                                        }
                                    }
                                    break;
                                case "VelMaxHP80%":

                                    opno = 16;
                                    opcode = 44;
                                    Speed_Count = 0;
                                    EV = (temp_zs / 5f).ToString("0");
                                    EP = (temp_gl / 5f).ToString("0.00");
                                    EK = (temp_gxxs / 5f).ToString("0.00");
                                    ENo = (temp_no / 5f).ToString("0");
                                    Msg(Msg_msg, panel_msg, "速度段：" + Jc_Process + "测试完成，测试结果：" + EK, true);
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("测试结果:" + addLength(EK, 4) + " ", 2, equipconfig.Ledxh);
                                    }
                                    ts2 = "结果：" + EK;
                                    led_display(ledNumberGS_E, EK);
                                    if (equipconfig.useJHJK)
                                    {
                                        if (double.Parse(EK) == 0)
                                        {
                                            JC_Status = false;
                                            button_ss.Text = "重新检测";
                                            Th_get_FqandLl.Abort();
                                            Jzjs_status = false; fq_getdata = false;
                                            Thread.Sleep(500);
                                            Msg(Msg_msg, panel_msg, "烟度值为0，检测中止。", true);
                                            ts1 = "烟度值为0";
                                            ts2 = "检测中止";
                                            if (ledcontrol != null)
                                            {
                                                ledcontrol.writeLed("　　检测终止　　", 2, equipconfig.Ledxh);
                                                Thread.Sleep(200);
                                                ledcontrol.writeLed("检测探头是否脱落", 5, equipconfig.Ledxh);
                                            }
                                            igbt.Exit_Control();

                                            return;
                                        }
                                    }
                                    if (lugdownconfig.isYdjk_cl)
                                    {
                                        if (double.Parse(EK) < lugdownconfig.ydjk_cl_value)
                                        {
                                            JC_Status = false;
                                            button_ss.Text = "重新检测";
                                            Th_get_FqandLl.Abort();
                                            Jzjs_status = false; fq_getdata = false;
                                            Thread.Sleep(500);
                                            Msg(Msg_msg, panel_msg, "烟度值过低，检测中止。", true);
                                            ts1 = "烟度值过低";
                                            ts2 = "检测中止";
                                            if (ledcontrol != null)
                                            {
                                                ledcontrol.writeLed("　　检测终止　　", 2, equipconfig.Ledxh);
                                                Thread.Sleep(200);
                                                ledcontrol.writeLed("检测探头是否脱落", 5, equipconfig.Ledxh);
                                            }
                                            igbt.Exit_Control();

                                            return;
                                        }
                                    }
                                    if (double.Parse(EK) < 0.01 && double.Parse(NK) < 0.01 && double.Parse(HK) < 0.01)
                                    {
                                        JC_Status = false;
                                        button_ss.Text = "重新检测";
                                        Th_get_FqandLl.Abort();
                                        Jzjs_status = false; fq_getdata = false;
                                        Thread.Sleep(500);
                                        Msg(Msg_msg, panel_msg, "烟度值为0，检测中止。", true);
                                        ts1 = "烟度值为0";
                                        ts2 = "检测中止";
                                        if (ledcontrol != null)
                                        {
                                            ledcontrol.writeLed("　　检测终止　　", 2, equipconfig.Ledxh);
                                            Thread.Sleep(200);
                                            ledcontrol.writeLed("检测探头是否脱落", 5, equipconfig.Ledxh);
                                        }

                                        return;
                                    }

                                    
                                    if (lugdownconfig.LugdownGljk)
                                    {
                                        if (double.Parse(EP) * glxzxs < double.Parse(jzjs_data.Lbgl) * lugdownconfig.Lugdown_Gljk_value * 0.01)
                                        {
                                            JC_Status = false;
                                            button_ss.Text = "重新检测";
                                            Th_get_FqandLl.Abort();
                                            Jzjs_status = false; fq_getdata = false;
                                            Thread.Sleep(500);
                                            Msg(Msg_msg, panel_msg, "加载功率过低，检测中止。", true);
                                            ts1 = "加载功率过低";
                                            ts2 = "检测中止";
                                            if (ledcontrol != null)
                                            {
                                                ledcontrol.writeLed("　　检测中止　　", 2, equipconfig.Ledxh);
                                                Thread.Sleep(200);
                                                ledcontrol.writeLed("加载功率低于限值", 5, equipconfig.Ledxh);
                                            }
                                            igbt.Exit_Control();

                                            return;
                                        }
                                    }
                                    Thread.Sleep(1000);
                                    Jc_Process = "结束";
                                    break;
                            }
                            if (Jc_Process == "结束")
                                break;
                        }
                    }
                    #endregion
                    Finish:
                    #region 结束并保存数据
                    statusconfigini.writeNeuStatusData("FinishTest", DateTime.Now.ToString());
                    Msg(Msg_msg, panel_msg, "加载减速测试完毕。", true);
                    ts1 = "测试完毕";
                    ts2 = "松开节气门换至空档";
                    jzjs_dataseconds.Gksj = GKSJ;//记录总的工况时间
                    statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.GUOCHE, GKSJ.ToString());
                    statusconfigini.writeGlStatusData(statusconfigIni.ENUM_GL_STATUS.STATUS_ENDSAMPLE, "");
                    Jzjs_status = false; fq_getdata = false;
                    timer_show.Stop();//停止计时
                    Thread.Sleep(1000);
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("加载减速测试完毕", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("松节气门换至空档", 5, equipconfig.Ledxh);
                    }
                    DataTable jzjs_datatable = new DataTable();
                    jzjs_datatable.Columns.Add("全程时序");
                    jzjs_datatable.Columns.Add("时序类别");
                    jzjs_datatable.Columns.Add("采样时序");
                    jzjs_datatable.Columns.Add("车速");
                    //jzjs_datatable.Columns.Add("转速");
                    jzjs_datatable.Columns.Add("寄生功率");
                    jzjs_datatable.Columns.Add("指示功率");
                    jzjs_datatable.Columns.Add("功率");
                    jzjs_datatable.Columns.Add("扭力");
                    jzjs_datatable.Columns.Add("光吸收系数K");
                    jzjs_datatable.Columns.Add("不透光度");
                    jzjs_datatable.Columns.Add("环境温度");
                    jzjs_datatable.Columns.Add("大气压力");
                    jzjs_datatable.Columns.Add("相对湿度");
                    jzjs_datatable.Columns.Add("油温");
                    jzjs_datatable.Columns.Add("转速");
                    jzjs_datatable.Columns.Add("DCF");
                    jzjs_datatable.Columns.Add("OPNO");
                    jzjs_datatable.Columns.Add("OPCODE");
                    jzjs_datatable.Columns.Add("DYNN");
                    jzjs_datatable.Columns.Add("NO");
                    if (equipconfig.DATASECONDS_TYPE == "江西" || equipconfig.DATASECONDS_TYPE == "云南保山"||equipconfig.DATASECONDS_TYPE== "安徽")
                    {
                        for (int i = 10; i < jzjs_dataseconds.Gksj; i++)//从第10秒开始取过程 数据，以避免金华判断转速时，第一秒的转速为0
                        {
                            if (int.Parse(Sxnblist[i]) > 0)
                            {
                                dr = jzjs_datatable.NewRow();
                                dr["全程时序"] = Qcsxlist[i];
                                dr["时序类别"] = (int.Parse(Sxnblist[i]) - 1).ToString();
                                dr["采样时序"] = Cysxlist[i];
                                dr["车速"] = Speedlist[i];
                                //dr["转速"] = FDJZSlist[i].ToString();
                                dr["寄生功率"] = JSGLlist[i];
                                dr["指示功率"] = ZSGLlist[i];
                                dr["功率"] = ZGLlist[i];
                                dr["扭力"] = Forcelist[i];
                                dr["光吸收系数K"] = GXXSlist[i];
                                dr["不透光度"] = btglist[i];
                                dr["环境温度"] = wdlist[i];
                                dr["大气压力"] = dqylist[i];
                                dr["相对湿度"] = sdlist[i];
                                dr["油温"] = ywlist[i];
                                dr["转速"] = FDJZSlist[i];
                                dr["DCF"] = DCFlist[i];
                                dr["OPNO"] = opnolist[i];
                                dr["OPCODE"] = opcodelist[i];
                                dr["DYNN"] = dynnlist[i];
                                dr["NO"] = Nolist[i];
                                jzjs_datatable.Rows.Add(dr);
                                if (equipconfig.useJHJK)
                                {
                                    if (FDJZSlist[i] == 0)
                                    {
                                        JC_Status = false;
                                        button_ss.Text = "重新检测";
                                        Th_get_FqandLl.Abort();
                                        Jzjs_status = false; fq_getdata = false;
                                        Thread.Sleep(500);
                                        Msg(Msg_msg, panel_msg, "过程程有转速为0，检测中止。", true);
                                        ts1 = "过程程有转速为0";
                                        ts2 = "检测中止";
                                        if (ledcontrol != null)
                                        {
                                            ledcontrol.writeLed("　　检测终止　　", 2, equipconfig.Ledxh);
                                            Thread.Sleep(200);
                                            ledcontrol.writeLed("过程程有转速为0", 5, equipconfig.Ledxh);
                                        }
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 10; i < jzjs_dataseconds.Gksj; i++)//从第10秒开始取过程 数据，以避免金华判断转速时，第一秒的转速为0
                        {
                            dr = jzjs_datatable.NewRow();
                            dr["全程时序"] = Qcsxlist[i];
                            dr["时序类别"] = Sxnblist[i];
                            dr["采样时序"] = Cysxlist[i];
                            dr["车速"] = Speedlist[i];
                            //dr["转速"] = FDJZSlist[i].ToString();
                            dr["寄生功率"] = JSGLlist[i];
                            dr["指示功率"] = ZSGLlist[i];
                            dr["功率"] = ZGLlist[i];
                            dr["扭力"] = Forcelist[i];
                            dr["光吸收系数K"] = GXXSlist[i];
                            dr["不透光度"] = btglist[i];
                            dr["环境温度"] = wdlist[i];
                            dr["大气压力"] = dqylist[i];
                            dr["相对湿度"] = sdlist[i];
                            dr["油温"] = ywlist[i];
                            dr["转速"] = FDJZSlist[i];
                            dr["DCF"] = DCFlist[i];
                            dr["OPNO"] = opnolist[i];
                            dr["OPCODE"] = opcodelist[i];
                            dr["DYNN"] = dynnlist[i];
                            dr["NO"] = Nolist[i];
                            jzjs_datatable.Rows.Add(dr);
                            if (equipconfig.useJHJK)
                            {
                                if (FDJZSlist[i] == 0)
                                {
                                    JC_Status = false;
                                    button_ss.Text = "重新检测";
                                    Th_get_FqandLl.Abort();
                                    Jzjs_status = false; fq_getdata = false;
                                    Thread.Sleep(500);
                                    Msg(Msg_msg, panel_msg, "过程程有转速为0，检测中止。", true);
                                    ts1 = "过程程有转速为0";
                                    ts2 = "检测中止";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("　　检测终止　　", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed("过程程有转速为0", 5, equipconfig.Ledxh);
                                    }
                                    return;
                                }
                            }
                        }
                    }
                    jzjs_data.CarID = carbj.CarID;
                    jzjs_data.Sd = SD.ToString("0.0");
                    jzjs_data.Wd = WD.ToString("0.0");
                    jzjs_data.Dqy = DQY.ToString("0.0");
                    jzjs_data.Gxsxs_100 = HK;
                    jzjs_data.Gxsxs_90 = NK;
                    jzjs_data.Gxsxs_80 = EK;
                    jzjs_data.hno = HNo;
                    jzjs_data.nno = NNo;
                    jzjs_data.eno = ENo;
                    // jzjs_data.Lbgl = MaxP.ToString("0.0");
                    jzjs_data.StopReason = "0";
                    JC_Status = false;//停止测试，工况时间清零
                    Th_get_FqandLl.Abort();//停止烟度采样
                    igbt.Exit_Control();
                    Thread.Sleep(1000);
                    #endregion

                    #region NO测量仪反吹
                    if (equipconfig.Ydjxh.ToLower() == yq_mqw5101)
                    {
                        flb_100.stopAction();
                        Thread.Sleep(500);
                        flb_100.autoFlowBack();//自动反吹，时间30s
                    }
                    if (notester != null)
                    {
                        notester.stopAction();
                        Thread.Sleep(500);
                        notester.autoFlowBack();//自动反吹，时间30s
                    }
                    #endregion
                    #region 等待车辆静止，举升升起
                    for (int i = 30; i > 0; i--)
                    {
                        Msg(Msg_msg, panel_msg, "请松开节气门并换至空档，不要使用制动 " + i.ToString("00") + " 秒", true);
                        Thread.Sleep(1000);
                    }
                    timer_show.Stop();
                    while (true)
                    {
                        if (igbt.Speed < 1)
                            break;
                        Thread.Sleep(100);
                    }
                    Msg(Msg_msg, panel_msg, "检测完成,举升上升，请驶离测功机", false);
                    ts2 = "请驶离测功机";
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("　　检测完成　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("请车辆驶离测功机", 5, equipconfig.Ledxh);
                    }
                    igbt.Lifter_Up();           //检测完成举升上升
                    button_ss.Text = "重新检测";
                    JC_Status = false;
                    writeDataIsFinished = false;
                    //th_load = new Thread(load_progress);
                    //th_load.Start();
                    csvwriter.SaveCSV(jzjs_datatable, "C:/jcdatatxt/" + carbj.CarID + ".csv");
                    jzjsdatacontrol.writeJzjsData(jzjs_data);//写carID.ini文件
                    writeDataIsFinished = true;
                    igjzjsIsFinished = true;
                    #endregion
                    this.Close();//自动关闭
                }
                else
                {
                    #region 其他
                    Msg(Msg_msg, panel_msg, "测试开始，请用合适档位全油加速至70km/h左右", true);
                    Thread.Sleep(5000);//等待5秒
                    Msg(Msg_msg, panel_msg, "待转速稳定后点击[确定最大转速]按钮开始功率扫描", true);
                    Thread.Sleep(5000);//等待5秒
                    MaxRpm_sure = false;
                    //MaxRPM = ZS;//确定最大转速
                    //Msg(label_MaxRpm, panel_MaxRpm, MaxRPM.ToString("0"), true);
                    //velMaxHpRation = MaxRPM / carbj.CarEdzs;
                    //VelMaxHP_real = Speed;
                    //zdcs = VelMaxHP_real;
                    //Control_Speed = igbt.Speed; //计算VelMaxPH
                    //if (Control_Speed < 50f || Control_Speed > 90f)
                    //{
                    //    Msg(Msg_msg, panel_msg, "最大转速不正常，请调整档位重新开始。", true);
                    //    JC_Status = false;
                    //    button_ss.Text = "重新检测";
                    //    Th_get_FqandLl.Abort();
                    //    Jzjs_status = false;
                    //    return;
                    //}
                    //Control_Speed = 82f;

                    Msg(Msg_msg, panel_msg, "最大转速已确定，开始功率扫描，保持油门全开", true);
                    Thread.Sleep(1000);
                    Msg(Msg_msg, panel_msg, "开始功率扫描，等待速度稳定", true);
                    while (GKSJ <= gksj0_zb) Thread.Sleep(800);
                    flag = 0;
                    MaxP = 0f;
                    sxnb = 1;//功率扫描开始
                    while (GKSJ <= gksj0_zb + gksj1_zb)               //功率扫描
                    {
                        Msg(Msg_msg, panel_msg, "正在进行功率扫描，当前速度：" + igbt.Speed.ToString("0.0") + "km/h", true);
                        Thread.Sleep(800);
                        if (GLlist_zb[GKSJ] > MaxP)
                        {
                            MaxP = GLlist_zb[GKSJ];
                            VelMaxHP_real = Speedlist_zb[GKSJ];
                        }
                    }
                    VelMaxHP = VelMaxHP_real;//实际velMaxHp
                    HP = MaxP.ToString("0.0");//最大轮边功率
                    Msg(Msg_msg, panel_msg, "功率扫描完成，最大轮边功率:" + HP + "kW,VelMaxHP为" + VelMaxHP_real.ToString("0.0") + "km/h", true);
                    led_display(ledNumber_JSVEL, (VelMaxHP_real * velMaxHpRation).ToString("0.0"));
                    led_display(ledNumber_SJVEL, VelMaxHP.ToString("0.0"));
                    Thread.Sleep(1000);//显示时间
                    Msg(Msg_msg, panel_msg, "最大轮边功率扫描完毕，开始加载测试", true);
                    Thread.Sleep(1000);
                    Jc_Process = "VelMaxHP100%";
                    flag = 0;
                    while (JC_Status)
                    {
                        float temp_speed = 0;
                        float temp_force = 0;
                        float temp_gl = 0;
                        float temp_zs = 0;
                        float temp_gxxs = 0;
                        switch (Jc_Process)
                        {
                            case "VelMaxHP100%":
                                Modulus = 1;
                                sxnb = 2;
                                break;
                            case "VelMaxHP90%":
                                Modulus = 0.9f;
                                sxnb = 3;
                                break;
                            case "VelMaxHP80%":
                                Modulus = 0.8f;
                                sxnb = 4;
                                break;
                        }
                        //igbt.Set_Speed(VelMaxHP_real * Modulus);
                        Msg(Msg_msg, panel_msg, "正在进行加载减速测试，将在" + Math.Round(VelMaxHP_real * Modulus, 2).ToString("0.0") + "km/h时取值", true);
                        Thread.Sleep(1000);
                        switch (Jc_Process)
                        {
                            case "VelMaxHP100%":
                                while (GKSJ <= gksj0_zb + gksj1_zb + gksj2_zb - 5) Thread.Sleep(800);
                                temp_gxxs = 0f;
                                for (int i = 1; i <= 5; i++)//取5s内的数据，以10HZ的采样率
                                {
                                    Msg(Msg_msg, panel_msg, "正在取值" + "(" + Jc_Process + ") " + GXXSlist_zb[gksj0_zb + gksj1_zb + gksj2_zb - 6 + i].ToString("0.00"), false);
                                    temp_gxxs += GXXSlist_zb[gksj0_zb + gksj1_zb + gksj2_zb - 6 + i];
                                    Thread.Sleep(1000);
                                }
                                //Speed_Count = 0;
                                HK = (temp_gxxs / 5f).ToString("0.00");
                                Msg(Msg_msg, panel_msg, "速度段：" + Jc_Process + "测试完成，测试结果：" + HK, true);
                                Thread.Sleep(1000);
                                Jc_Process = "VelMaxHP90%";
                                break;
                            case "VelMaxHP90%":
                                while (GKSJ <= gksj0_zb + gksj1_zb + gksj2_zb + gksj3_zb - 5) Thread.Sleep(800);
                                temp_gxxs = 0f;
                                for (int i = 1; i <= 5; i++)//取5s内的数据，以10HZ的采样率
                                {
                                    Msg(Msg_msg, panel_msg, "正在取值" + "(" + Jc_Process + ") " + GXXSlist_zb[gksj0_zb + gksj1_zb + gksj2_zb + gksj3_zb - 6 + i].ToString("0.00"), false);
                                    temp_gxxs += GXXSlist_zb[gksj0_zb + gksj1_zb + gksj2_zb + gksj3_zb - 6 + i];
                                    Thread.Sleep(1000);
                                }
                                //Speed_Count = 0;
                                NK = (temp_gxxs / 5f).ToString("0.00");
                                Msg(Msg_msg, panel_msg, "速度段：" + Jc_Process + "测试完成，测试结果：" + HK, true);
                                Thread.Sleep(1000);
                                Jc_Process = "VelMaxHP80%";
                                break;
                            case "VelMaxHP80%":
                                while (GKSJ <= gksj0_zb + gksj1_zb + gksj2_zb + gksj3_zb + gksj4_zb - 5) Thread.Sleep(800);
                                temp_gxxs = 0f;
                                for (int i = 1; i <= 5; i++)//取5s内的数据，以10HZ的采样率
                                {
                                    Msg(Msg_msg, panel_msg, "正在取值" + "(" + Jc_Process + ") " + GXXSlist_zb[gksj0_zb + gksj1_zb + gksj2_zb + gksj3_zb + gksj4_zb - 6 + i].ToString("0.00"), false);
                                    temp_gxxs += GXXSlist_zb[gksj0_zb + gksj1_zb + gksj2_zb + gksj3_zb + gksj4_zb - 6 + i];
                                    Thread.Sleep(1000);
                                }
                                //Speed_Count = 0;
                                EK = (temp_gxxs / 5f).ToString("0.00");
                                Msg(Msg_msg, panel_msg, "速度段：" + Jc_Process + "测试完成，测试结果：" + HK, true);
                                Thread.Sleep(1000);
                                Jc_Process = "结束";
                                break;
                        }
                        if (Jc_Process == "结束")
                            break;
                    }
                    Msg(Msg_msg, panel_msg, "加载减速测试完毕。", true);
                    jzjs_dataseconds.Gksj = GKSJ;//记录总的工况时间
                    statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.GUOCHE, GKSJ.ToString());
                    Jzjs_status = false; fq_getdata = false;
                    timer_show.Stop();//停止计时
                    Thread.Sleep(1000);
                    DataTable jzjs_datatable = new DataTable();
                    jzjs_datatable.Columns.Add("全程时序");
                    jzjs_datatable.Columns.Add("时序类别");
                    jzjs_datatable.Columns.Add("采样时序");
                    jzjs_datatable.Columns.Add("车速");
                    jzjs_datatable.Columns.Add("功率");
                    jzjs_datatable.Columns.Add("光吸收系数K");
                    for (int i = 0; i < gksj_zb; i++)
                    {
                        dr = jzjs_datatable.NewRow();
                        dr["全程时序"] = Qcsxlist[i];
                        dr["时序类别"] = Sxnblist_zb[i];
                        dr["采样时序"] = Cysxlist_zb[i];
                        dr["车速"] = Speedlist_zb[i];
                        dr["功率"] = GLlist_zb[i];
                        dr["光吸收系数K"] = GXXSlist_zb[i];
                        jzjs_datatable.Rows.Add(dr);
                    }
                    jzjs_data.CarID = carbj.CarID;
                    jzjs_data.Sd = SD.ToString("0.0");
                    jzjs_data.Wd = WD.ToString("0.0");
                    jzjs_data.Dqy = DQY.ToString("0.0");
                    jzjs_data.Gxsxs_100 = HK;
                    jzjs_data.Gxsxs_90 = NK;
                    jzjs_data.Gxsxs_80 = EK;
                    jzjs_data.Lbgl = MaxP.ToString("0.0");
                    JC_Status = false;//停止测试，工况时间清零
                    Th_get_FqandLl.Abort();//停止烟度采样
                    igbt.Exit_Control();
                    for (int i = 15; i > 0; i--)
                    {
                        Msg(Msg_msg, panel_msg, "请松开节气门并换至空档，不要使用制动 " + i.ToString("00") + " 秒", true);
                        Thread.Sleep(1000);
                    }
                    timer_show.Stop();
                    while (true)
                    {
                        if (igbt.Speed < 1)
                            break;
                        Thread.Sleep(100);
                    }
                    Msg(Msg_msg, panel_msg, "检测完成,举升上升，请驶离测功机", false);
                    igbt.Lifter_Up();           //检测完成举升上升
                    button_ss.Text = "重新检测";
                    JC_Status = false;
                    writeDataIsFinished = false;
                    //th_load = new Thread(load_progress);
                    //th_load.Start();
                    csvwriter.SaveCSV(jzjs_datatable, "C:/jcdatatxt/" + carbj.CarID + ".csv");
                    jzjsdatacontrol.writeJzjsData(jzjs_data);//写carID.ini文件
                    writeDataIsFinished = true;
                    igjzjsIsFinished = true;
                    this.Close();
                    #endregion
                }
            }
            catch (Exception er)
            {
                ini.INIIO.saveLogInf("[jc_exe发生异常]:" + er.Message);
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
        #region 信息显示
        public void datagridview_msg(DataGridView datagridview, string title, int row_number, string message)
        {
            BeginInvoke(new wtdtview(dt_msg), datagridview, title, row_number, message);
        }
        public void dt_msg(DataGridView datagridview, string title, int row_number, string message)
        {
            datagridview.Rows[row_number].Cells[title].Value = message;
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

        public void Msg_Show(Label Msgowner, string Msgstr, bool Update_DB)
        {
            Msgowner.Text = Msgstr;
        }

        public void Msg_Position(Label Msgowner, Panel Msgfather)
        {
            // Msgowner.Font = new System.Drawing.Font("微软雅黑", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            if (Msgowner.Width < Msgfather.Width)
            {
                Msgowner.Location = new Point((Msgfather.Width - Msgowner.Width) / 2, Msgowner.Location.Y);
            }
            else
            {
                Msgowner.Location = new Point(0, Msgowner.Location.Y);
                //Msgowner.Font = new System.Drawing.Font("微软雅黑", 30F * Msgfather.Width/Msgowner.Width , System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            }
        }
        public void led_display(LEDNumber.LEDNumber lednumber, string data)
        {
            BeginInvoke(new wl_led(led_show), lednumber, data);
        }
        public void led_show(LEDNumber.LEDNumber lednumber, string data)
        {
            lednumber.LEDText = data;
        }
        public void setButtonVisible(Button button, bool value)
        {
            BeginInvoke(new wtbuttonvisible(set_buttonvisible), button, value);
        }
        public void set_buttonvisible(Button button, bool value)
        {
            button.Visible = value;
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

        public void panel_visible(Panel panel, bool visible_value)
        {
            BeginInvoke(new wtpanelvisible(panel_show), panel, visible_value);
        }
        public void panel_show(Panel panel, bool visible_value)
        {
            panel.Visible = visible_value;
        }
        #endregion

        public void Datashow()
        {
            while (true)
            {
                if (carbj.ISUSE)
                {
                    nowpower = carbj.JZJS_GL * igbt.Power;
                }
                else
                    nowpower = igbt.Power;
                Thread.Sleep(80);
                try
                {
                    switch (igbt.Status)
                    {
                        default:
                            Speed = (float)Convert.ToDouble(igbt.Speed);
                            led_display(ledNumberCS, igbt.Speed.ToString("0.0"));
                            //arcScaleComponentCS.Value = igbt.Speed;
                            led_display(ledNumberGL, nowpower.ToString("0.0"));
                            //arcScaleComponentGL.Value = igbt.Power;

                            break;
                    }

                }
                catch (Exception)
                {
                    //MessageBox.Show("速度错了");
                }
            }
        }
        public void Fq_Detect()
        {
            while (true)
            {
                if (fq_getdata)
                {
                    try
                    {
                        if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                            smoke = fla_502.get_DirectData();
                        else if (equipconfig.Ydjxh != yq_mqw5101)
                        {
                            if (equipconfig.IsOldMqy200)
                            {
                                smoke = flb_100.get_DirectData();
                            }
                            else
                            {
                                smoke = flb_100.get_Data();
                            }
                        }
                        else if (equipconfig.Ydjxh == yq_mqw5101)
                        {
                            if (!flb_100.get_MQW5101_DataAndStatus(out smoke, out realstatus))
                            {
                                ini.INIIO.saveLogInf("[!!!!!!取烟度计实时数据失败]");
                            }
                            else
                            {
                                No = smoke.No;
                            }
                        }
                        if (notester != null)
                        {
                            if (!notester.get_MQW5101_DataAndStatus(out nosmoke, out realstatus))
                            {
                                ini.INIIO.saveLogInf("[!!!!!!取烟度计实时数据失败]!!!!!!!");
                            }
                            else
                            {
                                No = nosmoke.No;
                            }
                        }
                        if (isUseRotater)
                        {
                            if (rpm5300 != null)
                                ZS = (float)rpm5300.ZS;
                            else if (vmt_2000 != null)
                            {
                                if (vmt_2000.readRotateSpeed())
                                    ZS = vmt_2000.zs;
                            }
                        }
                        else
                        {
                            ZS = smoke.Zs;
                        }
                        if(nhsjz!=null&&lugdownconfig.Ywj=="南华附件")
                        {
                            if (nhsjz.readData())
                                yw_now = nhsjz.yw;
                        }
                        else
                        {
                            yw_now = smoke.Yw;
                        }
                        if (carbj.ISUSE)
                        {
                            Smoke = (float)(carbj.JZJS_K * smoke.K);
                        }
                        else
                        {
                            Smoke = smoke.K;
                        }
                        led_display(ledNumber_GXXS, Smoke.ToString("0.0"));
                        led_display(ledNumberNO, No.ToString("0"));
                        //arcScaleComponentGXXS.Value = Smoke;
                        led_display(ledNumberZS, ZS.ToString("0.0"));
                        double realwd = WD, realsd = SD, realdqy = DQY;
                        if (IsUseTpTemp)
                        {
                            realwd = WD;
                            realsd = SD;
                            realdqy = DQY;
                        }
                        else if (equipconfig.TempInstrument == "烟度计" && flb_100 != null)
                        {
                            realwd = smoke.WD;
                            realsd = smoke.SD;
                            realdqy = smoke.DQY;
                        }
                        else if (equipconfig.TempInstrument == "废气仪")
                        {
                            if (equipconfig.Fqyxh.ToLower() == "nha_503" || equipconfig.Fqyxh.ToLower() == "fla_502" || equipconfig.Fqyxh.ToLower() == "cdf5000")
                            {
                                Exhaust.Fla502_temp_data Environment = fla_502.Get_Temp();
                                realwd = Environment.TEMP;
                                realsd = Environment.HUMIDITY;
                                realdqy = Environment.AIRPRESSURE;
                            }
                            else
                            {
                                Exhaust.Fla502_data Environment = fla_502.GetData();
                                realwd = Environment.HJWD;
                                realsd = Environment.SD;
                                realdqy = Environment.HJYL;
                            }
                        }
                        else if (equipconfig.TempInstrument == "油耗仪")
                        {
                            Exhaust.yhrRealTimeData yhyEnvironment = new Exhaust.yhrRealTimeData();
                            if (yhy.getTempData(out yhyEnvironment))
                            {
                                realwd = yhyEnvironment.HJWD;
                                realsd = yhyEnvironment.HJSD;
                                realdqy = yhyEnvironment.HJYL;
                            }
                            else if (yhy.getTempData(out yhyEnvironment))
                            {
                                realwd = yhyEnvironment.HJWD;
                                realsd = yhyEnvironment.HJSD;
                                realdqy = yhyEnvironment.HJYL;
                            }
                        }
                        else if (equipconfig.TempInstrument == "XCE_100")
                        {
                            if (xce_100.readEnvironment())
                            {
                                realwd = xce_100.temp;
                                realsd = xce_100.humidity;
                                realdqy = xce_100.airpressure;
                            }
                        }
                        else if (equipconfig.TempInstrument == "DWSP_T5" || equipconfig.TempInstrument == "FTH_2")
                        {
                            if (xce_100.readEnvironment())
                            {
                                realwd = xce_100.temp;
                                realsd = xce_100.humidity;
                                realdqy = xce_100.airpressure;
                            }
                            else
                            {
                                xce_100.readEnvironment();
                                realwd = xce_100.temp;
                                realsd = xce_100.humidity;
                                realdqy = xce_100.airpressure;
                            }
                        }
                        else if (equipconfig.TempInstrument == "RZ_1")
                        {
                            if (xce_100.readEnvironment())
                            {
                                realwd = xce_100.temp;
                                realsd = xce_100.humidity;
                                realdqy = xce_100.airpressure;
                            }
                            else
                            {
                                xce_100.readEnvironment();
                                realwd = xce_100.temp;
                                realsd = xce_100.humidity;
                                realdqy = xce_100.airpressure;
                            }
                        }
                        realwd = thaxsdata.Tempxs * realwd;
                        realsd = thaxsdata.Humixs * realsd;
                        realdqy = thaxsdata.Airpxs * realdqy;
                        led_display(ledNumberWd, realwd.ToString("0.0"));
                        led_display(ledNumberSd, realsd.ToString("0.0"));
                        led_display(ledNumberDqy, realdqy.ToString("0.0"));
                    }
                    catch (Exception er)
                    {
                        ini.INIIO.saveLogInf("[fq_detect发生异常]:" + er.Message);
                    }
                    // arcScaleComponentZS.Value = ZS;
                }
                Thread.Sleep(30);
            }
        }
        private string addLength(string a, int b)
        {
            while (a.Length < b)
            {
                a = " " + a;
            }
            return a;
        }
        private float jsgl_xsa = 0;
        private float jsgl_xsb = 0;
        private float jsgl_xsc = 0;
        private double jsgl = 0;
        private double zsgl = 0;
        private double zgl = 0;
        private float nl = 0;
        private double nowpower = 0;
        private DateTime gc_time = DateTime.Now;//用于标记过程数据全程时序，避免出现相同时间

        private void timer_show_Tick(object sender, EventArgs e)
        {
            if (Jzjs_status)//如果正在测试
            {
                try
                {
                    displaycount++;
                    if (displaycount == 4)
                    {
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed(addLength(igbt.Speed.ToString("0.0"), 4) + " " + addLength(nowpower.ToString("0.0"), 5) + " ", 5, equipconfig.Ledxh);
                        }
                        displaycount = 0;
                    }
                    nowtime = DateTime.Now;
                    if ((int)(gongkuangTime * 10) > PregongkuangTime)
                    {
                        for (int count = PregongkuangTime; count < (int)(gongkuangTime * 10); count++)
                        {
                            ref_chart_data((float)nowpower, Smoke, ZS / 100f, igbt.Speed, igbt.Force / 100.0f);
                            if (PregongkuangTime > 1400)
                                Ref_Clear();
                            led_display(ledNumber_gksj, gongkuangTime.ToString("000.0"));
                        }
                        PregongkuangTime = (int)(gongkuangTime * 10);

                    }
                    //if (Convert.ToInt16(gongkuangTime * 10) / 10 != GKSJ)
                    if(DateTime.Compare(DateTime.Parse(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), DateTime.Parse(gc_time.ToString("yyyy-MM-dd HH:mm:ss"))) > 0)
                    {
                        gc_time = System.DateTime.Now;
                        if (GKSJ == 1023) GKSJ = 0;
                        jsgl = Math.Round(jsgl_xsa * igbt.Speed * igbt.Speed + jsgl_xsb * igbt.Speed + jsgl_xsc, 3);//寄生功率=
                        if (jsgl < 0) jsgl = 0;
                        zsgl = Math.Round(nowpower, 3);
                        if (zsgl < 0) zsgl = 0;
                        zgl = Math.Round(jsgl + zsgl, 3);
                        nl = igbt.Force;
                        if (nl < 0) nl = 0;
                        Sxnblist[GKSJ] = sxnb.ToString("0");//时序类别
                        Qcsxlist[GKSJ] = nowtime.ToString("yyyy-MM-dd HH:mm:ss.fff");//全程时序
                        Speedlist[GKSJ] = igbt.Speed;//速度
                        if (GKSJ < 5 && ZS == 0)//如果前5秒钟转速为0，则给一个转速，因为金华监管平台要求过程 数据中的转速不能为0
                            FDJZSlist[GKSJ] = 800;
                        else
                            FDJZSlist[GKSJ] = ZS;//转速
                        JSGLlist[GKSJ] = (float)jsgl;
                        ZSGLlist[GKSJ] = (float)zsgl;

                        ZGLlist[GKSJ] = (float)zgl;//功率
                        GXXSlist[GKSJ] = Smoke;//烟度值
                        Forcelist[GKSJ] = nl;//力
                        btglist[GKSJ] = smoke.Ns;
                        wdlist[GKSJ] = (float)WD;
                        sdlist[GKSJ] = (float)SD;
                        dqylist[GKSJ] = (float)DQY;
                        dcf = (float)init_xzxs(WD, SD, DQY, carbj.jqfs);
                        DCFlist[GKSJ] = dcf;
                        ywlist[GKSJ] = yw_now;
                        opnolist[GKSJ] = opno;
                        opcodelist[GKSJ] = opcode;
                        dynnlist[GKSJ] = (int)(igbt.Force);
                        Nolist[GKSJ] = (int)No;
                        if (GKSJ >= 1)
                        {
                            if (Sxnblist[GKSJ] != Sxnblist[GKSJ - 1])//时序类别改变，表示进行新的阶段，采样时序需要重新计时
                            {
                                cysx = 1;
                            }
                            else
                            {
                                cysx++;
                            }
                        }
                        Cysxlist[GKSJ] = cysx;
                        ini.INIIO.saveLogInf("[" + GKSJ.ToString() + "]:功率=" + ZGLlist[GKSJ].ToString("0.0")+ "kW:速度=" + Speedlist[GKSJ].ToString("0.0") + "km/h,扭力="+ dynnlist[GKSJ].ToString("0")+"N,Dcf=" + DCFlist[GKSJ].ToString("0.000"));
                        GKSJ++;//工况时间加1
                    }
                    TimeSpan timespan = nowtime - startTime;
                    gongkuangTime = (float)timespan.TotalMilliseconds / 1000f;
                }
                catch (Exception er)
                {
                    ini.INIIO.saveLogInf("[timer_show发生异常]:" + er.Message);
                }
            }
            else
            {
                GKSJ = 0;
                gongkuangTime = 0f;
                PregongkuangTime = 0;
            }


        }

        private void Jzjs_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!igjzjsIsFinished)
            {
                if (MessageBox.Show("测试未完成，确认退出？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    try
                    {

                        timer_show.Stop();
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("小心驾驶安全第一", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("　　欢迎参检　　", 5, equipconfig.Ledxh);
                        }
                        if (!igjzjsIsFinished)
                        {
                            jzjs_data.CarID = carbj.CarID;
                            jzjs_data.Sd = "-1";
                            jzjs_data.Wd = "-1";
                            jzjs_data.Dqy = "-1";
                            jzjs_data.Gxsxs_100 = "-1";
                            jzjs_data.Gxsxs_90 = "-1";
                            jzjs_data.Gxsxs_80 = "-1";
                            jzjs_data.Lbgl = "-1";
                            jzjs_data.Rev100 = "-1";
                            jzjs_data.StartTime = "-1";
                            jzjs_data.StopReason = "9";
                            jzjsdatacontrol.writeJzjsData(jzjs_data);//写carID.ini文件
                        }
                        if (Th_show != null) Th_show.Abort();
                        if (TH_ST != null) TH_ST.Abort();
                        if (Th_get_FqandLl != null) Th_get_FqandLl.Abort();
                        if (th_load != null) th_load.Abort();
                        //igbt.Lifter_Up();       //举升上升
                        try
                        {
                            if (flb_100 != null)
                            {
                                if (flb_100.ComPort_3.IsOpen)
                                    flb_100.ComPort_3.Close();
                            }
                            if (notester != null)
                            {
                                if (notester.ComPort_3.IsOpen)
                                    notester.ComPort_3.Close();
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
                            if (vmt_2000 != null)
                            {
                                if (vmt_2000.ComPort_1.IsOpen)
                                    vmt_2000.ComPort_1.Close();
                            }
                            if (igbt != null)
                            {
                                igbt.closeIgbt();
                            }
                            if (rpm5300 != null)
                            {
                                rpm5300.closeEquipment();
                            }
                            if (xce_100 != null)
                            {
                                if (xce_100.ComPort_1.IsOpen)
                                    xce_100.ComPort_1.Close();
                            }
                            if (yhy != null)
                            {
                                if (yhy.ComPort_1.IsOpen)
                                    yhy.ComPort_1.Close();
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
                    catch (Exception)
                    {

                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {

                try
                {
                    timer_show.Stop();
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("小心驾驶安全第一", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("　　欢迎参检　　", 5, equipconfig.Ledxh);
                    }
                    if (Th_show != null) Th_show.Abort();
                    if (TH_ST != null) TH_ST.Abort();
                    if (Th_get_FqandLl != null) Th_get_FqandLl.Abort();
                    if (th_load != null) th_load.Abort();
                    //igbt.Lifter_Up();       //举升上升
                    try
                    {
                        if (flb_100 != null)
                        {
                            if (flb_100.ComPort_3.IsOpen)
                                flb_100.ComPort_3.Close();
                        }
                        if (notester != null)
                        {
                            if (notester.ComPort_3.IsOpen)
                                notester.ComPort_3.Close();
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
                        if (vmt_2000 != null)
                        {
                            if (vmt_2000.ComPort_1.IsOpen)
                                vmt_2000.ComPort_1.Close();
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
                        if (rpm5300 != null)
                        {
                            rpm5300.closeEquipment();
                        }
                        if (yhy != null)
                        {
                            if (yhy.ComPort_1.IsOpen)
                                yhy.ComPort_1.Close();
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
                catch (Exception)
                {

                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            MaxRpm_sure = true;
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

        private void toolStripButtonStopTest_Click(object sender, EventArgs e)
        {

        }

        private void button_ss_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (JC_Status == false)
                {
                    if (dt_zb != null)
                        isUsedata = true;
                    else
                        isUsedata = false;
                    if (igbt != null)
                    {
                        igbt.Force_Zeroing();
                        Thread.Sleep(500);
                        Msg(Msg_msg, panel_msg, "检测开始,举升下降", false);
                        ts1 = "举升下降...";
                        igbt.Lifter_Down();     //举升下降
                        Thread.Sleep(5000);
                    }
                    jctime = DateTime.Now.ToString();
                    TH_ST = new Thread(Jc_Exe);
                    TH_ST.Start();
                    Th_get_FqandLl = new Thread(Fq_Detect);
                    Th_get_FqandLl.Start();
                    timer_show.Start();
                    JC_Status = true;
                    Jzjs_status = false; fq_getdata = false;
                    //button_yj.Enabled = false;
                    button_ss.Text = "停止检测";
                }
                else
                {
                    igbt.Exit_Control();
                    TH_ST.Abort();
                    Th_get_FqandLl.Abort();
                    Jzjs_status = false; fq_getdata = false;
                    timer_show.Stop();
                    JC_Status = false;
                    if (ledcontrol != null)
                    {
                        Thread.Sleep(500);
                        ledcontrol.writeLed("测试被终止", 2, equipconfig.Ledxh);

                        //ledcontrol.writeLed("请移除探测头", 5);
                    }
                    ts2 = "测试被终止";
                    button_ss.Text = "重新检测";
                }
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            settings newSettings = new settings();
            newSettings.ShowDialog();
            initConfigInfo();
        }

        private void button_ss_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (JC_Status == false)
                {
                    if (dt_zb != null)
                        isUsedata = true;
                    else
                        isUsedata = false;
                    igbt.Force_Zeroing();
                    jctime = DateTime.Now.ToString();
                    TH_ST = new Thread(Jc_Exe);
                    TH_ST.Start();
                    Th_get_FqandLl = new Thread(Fq_Detect);
                    Th_get_FqandLl.Start();
                    timer_show.Start();
                    JC_Status = true;
                    Jzjs_status = false;
                    fq_getdata = false;
                    //button_yj.Enabled = false;
                    button_ss.Text = "停止检测";
                }
                else
                {
                    igbt.Exit_Control();
                    TH_ST.Abort();
                    Th_get_FqandLl.Abort();
                    Jzjs_status = false;
                    fq_getdata = false;
                    timer_show.Stop();
                    JC_Status = false;
                    if (ledcontrol != null)
                    {
                        Thread.Sleep(500);
                        ledcontrol.writeLed("测试被终止", 2, equipconfig.Ledxh);

                        //ledcontrol.writeLed("请移除探测头", 5);
                    }
                    ts2 = "测试被终止";
                    button_ss.Text = "重新检测";
                }
            }
            catch (Exception)
            {
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                fla_502.set_linearDem();
            else if (equipconfig.Ydjxh.ToLower() != yq_mqw5101)
                flb_100.set_linearDem();
            else if (equipconfig.Ydjxh.ToLower() == yq_mqw5101)
                flb_100.zeroEquipment();
            else if (notester != null)
                notester.zeroEquipment();
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