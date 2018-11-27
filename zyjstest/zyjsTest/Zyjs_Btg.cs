using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using carinfor;
using System.IO;
using System.Diagnostics;

namespace zyjsTest
{
    public partial class Zyjs_Btg : Form
    {
        carinfor.carInidata carbj = new carInidata();
        public static equipmentConfigInfdata equipconfig = new equipmentConfigInfdata();
        BtgConfigInfdata btgconfig = new BtgConfigInfdata();
        statusconfigIni statusconfigini = new statusconfigIni();
        CSVcontrol.CSVwriter csvwriter = new CSVcontrol.CSVwriter();
        carIni carini = new carIni();
        configIni configini = new configIni();

        carinfor.zyjsBtgdata zyjs_data = new zyjsBtgdata();
        zyjsBtgdataControl zyjsdatacontrol = new zyjsBtgdataControl();

        public float jcTime = 0f;
        private DateTime jcStarttime;
        public int JCSJ = 0;

        private string UseFqy = "";
        Exhaust.Fla501 fla_501 = null;
        Exhaust.Fla502 fla_502 = null;
        Exhaust.FLB_100 flb_100 = null;
        Exhaust.Flv_1000 flv_1000 = null;
        Exhaust.VMT_2000 vmt_2000 = null;
        Exhaust.RPM5300 rpm5300 = null;
        Exhaust.XCE_100 xce_100 = null;
        Exhaust.Nhsjz nhsjz = null;
        private Exhaust.yhControl yhy = null;
        thaxs thaxsdata = new thaxs();
        bool isUseRotater = false;
        LedControl.BX5k1 ledcontrol = null;
        Dynamometer.IGBT igbt = null;
        private bool zyjsIsFinished = false;

        private float maxvalue = 0;
        private float maxywvalue = 0;
        private int maxzsvalue = 0;
        private bool caculateStart = false;

        private DateTime startTime, nowtime;
        public int GKSJ = 0, perGKSJ = 0;                                                                        //工况时间
        public float gongkuangTime = 0f;
        public bool SaveData_status = false;


        int cysx = 0;
        int sxnb = 1;
        bool isReadRealTime = true;
        public float[] Ywlist = new float[10240];
        public float[] wdlist = new float[10240];                                                //每秒光吸收系数数组
        public float[] sdlist = new float[10240];                                               //每秒发动机转速数数组
        public float[] dqylist = new float[10240];                                                  //每秒功率数组
        private string[] Qcsxlist = new string[20480];
        private string[] Sxnblist = new string[20480];
        public int[] Cysxlist = new int[10240];
        public float[] Zslist = new float[10240];                                               //每秒速度数组
        public float[] Klist = new float[10240];                                            //每秒速度数组
        public float[] Nslist = new float[10240];

        DataTable dt = new DataTable();

        public delegate void wt_void();                             //委托
        public delegate void wtls(Label Msgowner, string Msgstr);   //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather); //委托
        public delegate void wtlsb(Label Msgowner, string Msgstr, bool Update_DB);              //委托
        public delegate void wtcs(Control controlname, string text);                            //委托
        public delegate void wtdtview(DataGridView datagridview, string title, int row_number, string message);
        public bool JC_Status = false;                              //检测状态
        Thread TH_ST = null;                                        //检测线程
        Thread TH_YD = null;
        Thread Th_get_FqandLl = null;
        //Exhaust.Flb_100_smoke dycsmoke = null;
        //Exhaust.Flb_100_smoke decsmoke = null;
        //Exhaust.Flb_100_smoke dscsmoke = null;
        //Exhaust.Flb_100_smoke dsicsmoke = null;
        Exhaust.Flb_100_smoke smoke = new Exhaust.Flb_100_smoke();
        public double preclz1 = 0.0;
        public double preclz2 = 0.0;
        public double preclz3 = 0.0;
        public double dycclz = 0.0;
        public double decclz = 0.0;
        public double dscclz = 0.0;
        public double dycyw = 0.0;
        public double decyw = 0.0;
        public double dscyw = 0.0;
        public int dyczs = 0;
        public int deczs = 0;
        public int dsczs = 0;
        public double yw = 0.0;
        public float yw_now = 0;
        public double dsicclz = 0.0;
        public double pjz = 0.0;
        public double wd = 20;
        public double dqy = 101;
        public double sd = 20;
        public double[] temperatureEveryMonth = { -4, 1, 9, 17, 25, 29, 30, 28, 23, 16, 6, -2 };
        public double btgxz = 0.0;
        public string pdjg = "合格";
        public string Pqfs = "";                                    //选择其排气方式
        public bool zyjs_status = false;
        public int dszs = 750;
        public string jctime = "";
        SYS.Model.jcxztCheck jcxzt = new SYS.Model.jcxztCheck();
        SYS_DAL.JCXXX jcxxx = new SYS_DAL.JCXXX();
        private bool isSongpin = false;
        public static int dyzs = 0;
        string ledComString = "";
        public static string ts1 = "川AV7M82";
        public static string ts2 = "自由加速法";
        public static string data1 = "--";
        public static string data2 = "--";
        public static string data3 = "--";
        public static string data4 = "--";
        private bool isQxc = true;
        private int ddsj = 10;
        public static int ZS=0;
        private bool isautostart = true;
        public Zyjs_Btg()
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
        private bool IsUseVRM = false;
        private int VRMzt = 0;//0-怠速（800） 1-开始踩（800+(EDZS-800)*30%) 2-上升（800+(EDZS-800)*60%) 3-最大转速（EDZS) 4- 维持（EDZS) 5-维持（EDZS) 6-松油（800+(EDZS-800)*50%) 
        private bool IsProcessStarted(string processName)
        {

            Process[] temp = Process.GetProcessesByName(processName);

            if (temp.Length > 0) return true;

            else

                return false;

        }
        private void Zyjs_Btg_Load(object sender, EventArgs e)
        {
            IsUseVRM=IsProcessStarted("VRM");
            pictureBoxVRM.Visible = IsUseVRM;
            initCarInfo();
            initConfigInfo();
            initDataResult();
            initEquipment();
            Init_Data();            //初始化数据
            Init_Limit();           //初始化限值
            Init_Show();            //初始化显示
            isSongpin = false;
            if (btgconfig.RotateSpeedMonitor)
            {
                dyzs =(int)( carbj.CarEdzs);
            }
            else
                dyzs = 0;
            //断油转速 prepareform = new 断油转速();
            //prepareform.ShowDialog();
            /*
            if (carbj.CarBsxlx == "0")//自动档
            {
                dyzs = dyzs * 2 / 3;
            }
            */
            if (isQxc)
            {
                ddsj = 6;
                jhdszs = 2500;
            }
            else
            {
                jhdszs = 1800;
            }
            if (btgconfig.jhzsgcjk||equipconfig.useJHJK)
            {
                ddsj = 10;
                dyzs = jhdszs;
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
                    wd = double.Parse(temp.ToString());
                    ini.INIIO.GetPrivateProfileString("环境数据", "sd", "", temp, 2048, "C:/jcdatatxt/环境数据.ini");
                    sd = double.Parse(temp.ToString());
                    ini.INIIO.GetPrivateProfileString("环境数据", "dqy", "", temp, 2048, "C:/jcdatatxt/环境数据.ini");
                    dqy = double.Parse(temp.ToString());
                    IsUseTpTemp = true;
                }
            }

            if (carbj.ISUSE)
            {
                if(flb_100!=null)
                    flb_100.kxs = carbj.ZYJS_K;
            }
            if (isautostart)
            {
                Thread.Sleep(3000);
                button1_Click(sender, e);
            }
            if(equipconfig.useJHSCREEN)
            {
                groupBox1.Visible = false;
                groupBox2.Visible = false;
            }
        }

        #region 初始化
        private int jhdszs=2500;
        private void initCarInfo()
        {
            carbj = carini.getCarIni();
            isQxc = (carbj.CarZzl < 3500);
            
            ts1 = carbj.CarPH;
        }
        private void initConfigInfo()
        {
            equipconfig = configini.getEquipConfigIni();
            isautostart = equipconfig.WorkAutomaticMode;
            btgconfig = configini.getBtgConfigIni();
            thaxsdata = configini.getthaxsConfigIni();
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;

            ini.INIIO.GetPrivateProfileString("配置参数", "LED配置字", "", temp, 2048, @".\detectConfig.ini");
            ledComString = temp.ToString();
            //configdata = configini.getConfigIni();
        }
        public void initEquipment()
        {
            bool Init_flag = true;
            string init_message = "";
            
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
                                MessageBox.Show(er.ToString(), "出错啦");
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
                                MessageBox.Show(er.ToString(), "出错啦");
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
                                MessageBox.Show(er.ToString(), "出错啦");
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
            try
            {
                if (equipconfig.Lljifpz == true)
                {
                    try
                    {
                        flv_1000 = new Exhaust.Flv_1000();
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
                        MessageBox.Show(er.ToString(), "出错啦");
                    }
                }

            }
            catch (Exception)
            {
            }
            try
            {
                if (btgconfig.Zsj.ToLower() == "vmt-2000" || btgconfig.Zsj.ToLower() == "vut-3000")
                {
                    try
                    {
                        vmt_2000 = new Exhaust.VMT_2000();
                        isUseRotater = true;
                        if (vmt_2000.Init_Comm(btgconfig.Zsjck, "19200,N,8,1") == false)
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
                        MessageBox.Show(er.ToString(), "出错啦");
                        isUseRotater = false;
                    }
                }
                else if (btgconfig.Zsj.ToLower() == "mqz-2" || btgconfig.Zsj.ToLower() == "mqz-3")
                {
                    MessageBox.Show("系统未提供该转速计功能，请重新配置", "系统提示");
                    isUseRotater = false;
                }
                else if (btgconfig.Zsj.ToLower() == "rpm5300")
                {
                    try
                    {
                        rpm5300 = new Exhaust.RPM5300();
                        isUseRotater = true;
                        if (rpm5300.Init_Comm(btgconfig.Zsjck, "9600,N,8,1") == false)
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
                        MessageBox.Show(er.ToString(), "出错啦");
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
        private void initDataResult()
        {
            //dt.Clear();
            dt.Columns.Add("项目");
            dt.Columns.Add("结果");
            dt.Columns.Add("转速");
            DataRow dr = null;
            dr = dt.NewRow();
            dr["项目"] = "怠速转速";
            dr["结果"] = "--";
            dr["转速"] = "--";
            //dr["判定"] = "--";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "第一次";
            dr["结果"] = "--";
            dr["转速"] = "--";
            //dr["判定"] = "--";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "第二次";
            dr["结果"] = "--";
            dr["转速"] = "--";
            //dr["判定"] = "--";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "第三次";
            dr["结果"] = "--";
            dr["转速"] = "--";
            dt.Rows.Add(dr);
            dataGridView1.DataSource = dt;
        }
        public void Init_Data()
        {
            try
            {
                Msg(label_cp, panel3, carbj.CarPH, false);
                if (ledcontrol != null)
                {
                    if (equipconfig.Ledxh == "同济单排")
                    {
                        ledcontrol.writeLed(carbj.CarPH + "请上线", 2,equipconfig.Ledxh);
                    }
                    else
                    {
                        ledcontrol.writeLed(carbj.CarPH, 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("请上检测线准备", 5, equipconfig.Ledxh);
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString(), "车辆数据初始化失败");
            }
        }

        public void Init_Limit()
        {
        }

        public void Init_Show()
        {
            try
            {
                Ref_Control_Text(label_st, "准备检测");
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString(), "车辆数据初始化失败");
            }
        }

        #endregion

        public void Jc_Exe()
        {
            try
            {
                VRMzt = 0;
                statusconfigini.writeGlStatusData(statusconfigIni.ENUM_GL_STATUS.STATUS_DAOWEI, "");
                List<int> thistimezslist = new List<int>();
                int time_踩油门点 = 0;
                int maxzs = 0;
                int zs_到底点 = 0;
                int zs_松油点 = 0;
                int time_油门到底点 = 0;
                int time_断油点 = 0;
                int time_松油门点 = 0;
                Random ra = new Random();
                zyjsIsFinished = false;
                Msg(label_msg, panel_cp, "正在检查烟度计状态", true);
                ts1 = "检测即将开始";
                ts2 = "检测烟度计...";
                data1 = "--";
                data2 = "--";
                data3 = "--";
                data4 = "--";
                if (ledcontrol != null)
                {
                    if (equipconfig.Ledxh == "同济单排")
                    {
                        ledcontrol.writeLed("检查烟度计... ", 2, equipconfig.Ledxh);
                    }
                    else
                    {
                        ledcontrol.writeLed("检查烟度计... ", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("　　检测即将开始", 5, equipconfig.Ledxh);
                    }
                }
                Thread.Sleep(1000);
                for (int i = 0; i <= 10; i++)                        //检查烟度计状态
                {
                    if (equipconfig.Ydjxh != "nht_1"&&equipconfig.Ydjxh.ToLower()!="cdf5000")
                    {
                        string zt = flb_100.Get_Mode();
                        if (zt != "通讯故障")
                        {
                            Msg(label_msg, panel_cp, "烟度计工作正常", true);
                            break;
                        }
                        else if (i == 9)
                        {
                            Msg(label_msg, panel_cp, "烟度计无法正常连接，请检查后重新开始。", true);
                            ts1 = "检测终止";
                            ts2 = "烟度计通讯故障";
                            zyjs_status = false;
                            JC_Status = false;
                            this.BeginInvoke(new wt_void(Ref_Button));
                            if (ledcontrol != null)
                            {
                                if (equipconfig.Ledxh == "同济单排")
                                {
                                    ledcontrol.writeLed("烟度计未连接", 2, equipconfig.Ledxh);
                                }
                                else
                                {
                                    ledcontrol.writeLed("烟度计未连接　　", 2, equipconfig.Ledxh);
                                    Thread.Sleep(200);
                                    ledcontrol.writeLed("请检查后重新检测", 5, equipconfig.Ledxh);
                                }
                            }
                            return;
                        }
                    }
                    else
                    {
                        Msg(label_msg, panel_cp, "烟度计工作正常", true);
                        break;
                    }
                    Thread.Sleep(500);
                }
                
                if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                    fla_502.set_linearDem();
                else
                    flb_100.set_linearDem();//烟度计进行线性校正
                if (ledcontrol != null)
                {
                    if (equipconfig.Ledxh == "同济单排")
                    {
                        ledcontrol.writeLed("线性校正中...", 2, equipconfig.Ledxh);
                    }
                    else
                    {
                        ledcontrol.writeLed("线性校正中... ", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("......  　　", 5, equipconfig.Ledxh);
                    }
                }

                //ts1 = "检测终止";
                ts2 = "烟度计校正中...";
                Msg(label_msg, panel_cp, "烟度计正在进行线性校正。", false);
                Thread.Sleep(8000);//等待5s至线性校正结束
                // CarWait.igbt.Exit_Control();            //退出igbt的所有控制状态
                //CarWait.Flb_100.Set_Measure();
                Msg(label_msg, panel_cp, "校正完毕，请启动车辆保持怠速，安置好转速计", true);
                ts2 = "校正完毕";
                if (ledcontrol != null)
                {
                    if (equipconfig.Ledxh == "同济单排")
                    {
                        ledcontrol.writeLed("请安置好转速计", 2, equipconfig.Ledxh);
                    }
                    else
                    {
                        ledcontrol.writeLed("测试即将开始　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("请安置好转速计　", 5, equipconfig.Ledxh);
                    }
                }
                Thread.Sleep(3000);
                Thread.Sleep(1000);
                try//获取环境参数
                {
                    Thread.Sleep(1000);
                    //Exhaust.Fla502_data Environment = new Exhaust.Fla502_data();
                    Exhaust.Flb_100_smoke ydjEnvironment = new Exhaust.Flb_100_smoke();
                    Exhaust.yhrRealTimeData yhyEnvironment = new Exhaust.yhrRealTimeData();
                    if (IsUseTpTemp)
                    {
                        wd = wd;
                        sd = sd;
                        dqy = dqy;
                    }
                    else if (equipconfig.TempInstrument == "烟度计" && flb_100 != null)
                    {
                        if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                            fla_502.Set_Measure();
                        else
                            flb_100.Set_Measure();
                        Thread.Sleep(1000);
                        if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                            ydjEnvironment = fla_502.get_DirectData();
                        else
                        {
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
                        }
                        wd = ydjEnvironment.WD;
                        sd = ydjEnvironment.SD;
                        dqy = ydjEnvironment.DQY;
                    }
                    else if (equipconfig.TempInstrument == "废气仪")
                    {
                        if (equipconfig.Fqyxh.ToLower() == "nha_503" || equipconfig.Fqyxh.ToLower() == "fla_502" || equipconfig.Fqyxh.ToLower() == "cdf5000")
                        {
                            Exhaust.Fla502_temp_data Environment = fla_502.Get_Temp();
                            wd = Environment.TEMP;
                            sd = Environment.HUMIDITY;
                            dqy = Environment.AIRPRESSURE;
                        }
                        else
                        {
                            Exhaust.Fla502_data Environment = fla_502.GetData();
                            wd = Environment.HJWD;
                            sd = Environment.SD;
                            dqy = Environment.HJYL;
                        }
                    }
                    else if (equipconfig.TempInstrument == "油耗仪")
                    {
                        if (yhy.getTempData(out yhyEnvironment))
                        {
                            wd = yhyEnvironment.HJWD;
                            sd = yhyEnvironment.HJSD;
                            dqy = yhyEnvironment.HJYL;
                        }
                        else if (yhy.getTempData(out yhyEnvironment))
                        {
                            wd = yhyEnvironment.HJWD;
                            sd = yhyEnvironment.HJSD;
                            dqy = yhyEnvironment.HJYL;
                        }
                    }
                    else if (equipconfig.TempInstrument == "XCE_100")
                    {
                        if (xce_100.readEnvironment())
                        {
                            wd = xce_100.temp;
                            sd = xce_100.humidity;
                            dqy = xce_100.airpressure;
                        }
                    }
                    else if (equipconfig.TempInstrument == "XCE_101")
                    {
                        if (xce_100.readEnvironment())
                        {
                            wd = xce_100.temp;
                            sd = xce_100.humidity;
                            dqy = xce_100.airpressure;
                        }
                    }
                    else if (equipconfig.TempInstrument == "DWSP_T5" || equipconfig.TempInstrument == "FTH_2")
                    {
                        if (xce_100.readEnvironment())
                        {
                            wd = xce_100.temp;
                            sd = xce_100.humidity;
                            dqy = xce_100.airpressure;
                        }
                        else
                        {
                            xce_100.readEnvironment();
                            wd = xce_100.temp;
                            sd = xce_100.humidity;
                            dqy = xce_100.airpressure;
                        }
                    }
                    else if (equipconfig.TempInstrument == "RZ_1")
                    {
                        if (xce_100.readEnvironment())
                        {
                            wd = xce_100.temp;
                            sd = xce_100.humidity;
                            dqy = xce_100.airpressure;
                        }
                        else
                        {
                            xce_100.readEnvironment();
                            wd = xce_100.temp;
                            sd = xce_100.humidity;
                            dqy = xce_100.airpressure;
                        }
                    }
                    else if (equipconfig.TempInstrument == "模拟")
                    {
                        Random rd = new Random();
                        int month = DateTime.Now.Month;
                        int hour = DateTime.Now.Hour;
                        wd = temperatureEveryMonth[month - 1] - Math.Abs(hour - 12) * 0.6 + (double)(rd.Next(20) - 10) / 10.0;
                        sd = 50 + (double)(rd.Next(400) - 200) / 10.0;
                        dqy = 90 + (double)(rd.Next(20) - 10) / 10.0;
                    }
                    wd = thaxsdata.Tempxs * wd;
                    sd = thaxsdata.Humixs * sd;
                    dqy = thaxsdata.Airpxs * dqy;
                    ts1 = wd.ToString("0.0") + "℃ " + sd.ToString("0.0") + "% " + dqy.ToString("0.0") + "kPa";
                    //ts2 = "低于限值,检测中止";
                    Msg(label_msg, panel_cp, wd.ToString("0.0") + "℃ " + sd.ToString("0.0") + "% " + dqy.ToString("0.0") + "kPa", false);
                    Thread.Sleep(1000);
                }
                catch (Exception)
                {
                }
                if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                    fla_502.Set_Measure();
                else
                    flb_100.Set_Measure();
                Thread.Sleep(200);
                if (btgconfig.IsTestYw)
                {
                    ts1 = "读取油温...";
                    Msg(label_msg, panel_cp, "读取油温...", false);
                    if (ledcontrol != null)
                    {
                        if (equipconfig.Ledxh == "同济单排")
                        {
                            ledcontrol.writeLed("读取油温", 2, equipconfig.Ledxh);
                        }
                        else
                        {
                            ledcontrol.writeLed("读取油温...", 4, equipconfig.Ledxh);
                        }
                    }
                    Exhaust.Flb_100_smoke Environment = new Exhaust.Flb_100_smoke();
                    float ywnow = 0;
                    if (nhsjz != null && btgconfig.Ywj == "南华附件")
                    {
                        if (nhsjz.readData())
                            ywnow = nhsjz.yw;
                        else if (nhsjz.readData())
                            ywnow = nhsjz.yw;
                    }
                    else
                    {

                        if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                            Environment = fla_502.get_DirectData();
                        else
                            Environment = flb_100.get_DirectData();
                        ywnow = Environment.Yw;
                    }
                    Thread.Sleep(1000);
                    if (ywnow < 80)
                    {
                        ts1 = "油温: " + ywnow.ToString("0.0") + " ℃";
                        ts2 = "低于限值,检测中止";
                        Msg(label_msg, panel_cp, "油温:" + ywnow.ToString("0.0") + "℃" + "低于限值,检测中止", false);
                        if (ledcontrol != null)
                        {
                            if (equipconfig.Ledxh == "同济单排")
                            {
                                ledcontrol.writeLed("油温过低检测中止", 2, equipconfig.Ledxh);
                            }
                            else
                            {
                                ledcontrol.writeLed("油温低于限值　　", 2, equipconfig.Ledxh);
                                ledcontrol.writeLed("　　检测中止　　", 5, equipconfig.Ledxh);
                            }
                        }
                        JC_Status = false;
                        this.BeginInvoke(new wt_void(Ref_Button));
                        zyjs_status = false;
                        return;
                    }
                    else
                    {
                        ts1 = "油温: " + ywnow.ToString("0.0") + " ℃";
                        ts2 = "允许检测";
                        Msg(label_msg, panel_cp, "油温:" + ywnow.ToString("0.0") + "℃" + ",允许检测", false);

                    }
                    Thread.Sleep(1000);
                }
                if(btgconfig.Btgcfcs>0)
                {
                    isReadRealTime = true;
                    Msg(label_msg, panel_cp, "检测即将开始，请按提示进行吹拂", true);
                    zyjs_status = true;//烟度计开始获取数据
                    ts2 = "请按提示进行吹拂";
                    if (ledcontrol != null)
                    {
                        if (equipconfig.Ledxh == "同济单排")
                        {
                            ledcontrol.writeLed("请按提示进行操作", 2, equipconfig.Ledxh);
                        }
                        else
                        {
                            ledcontrol.writeLed("测试即将开始　　", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("请按提示进行操作", 5, equipconfig.Ledxh);
                        }
                    }
                    Thread.Sleep(2000);
                }
                for (int cfcs = 0; cfcs < btgconfig.Btgcfcs; cfcs++)
                {
                    Msg(label_msgcs, panel1, "第" + (cfcs + 1).ToString() + "次吹拂", false);
                    Msg(label_msg, panel_cp, carbj.CarPH + "请迅猛将油门踩到底", true);
                    ts1 = "第" + (cfcs + 1).ToString() + "次吹拂";
                    ts2 = "请1s内踩至断油转速";
                    VRMzt = 1;
                    if (ledcontrol != null)
                    {
                        if (equipconfig.Ledxh == "同济单排")
                        {
                            ledcontrol.writeLed("请油门踩到底", 2, equipconfig.Ledxh);
                        }
                        else
                        {
                            ledcontrol.writeLed("第" + (cfcs + 1).ToString() + "次吹拂", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("　　请油门踩到底", 5, equipconfig.Ledxh);
                        }
                    }
                    Thread.Sleep(2000);
                    while (ZS < dyzs)
                    {                        
                        Msg(label_msg, panel_cp, carbj.CarPH + "未达到断油转速，请保持油门踩到底", true);
                        Thread.Sleep(400);
                    }
                    if (!isQxc||equipconfig.useJHJK)
                    {
                        for (int i = 2; i > 0; i--)
                        {
                            Msg(label_msg, panel_cp, carbj.CarPH + "维持"+i.ToString()+"s", true);
                            ts2 = "维持" + i.ToString() + "s";
                            Thread.Sleep(1000);
                        }
                    }
                    Msg(label_msg, panel_cp, carbj.CarPH + "请松开踏板", true);
                    ts2 = "松开踏板";
                    if (ledcontrol != null)
                    {
                        if (equipconfig.Ledxh == "同济单排")
                        {
                            ledcontrol.writeLed("请松开踏板", 2, equipconfig.Ledxh);
                        }
                        else
                        {
                            ledcontrol.writeLed("第" + (cfcs + 1).ToString() + "次吹拂", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("　　　请松开踏板", 5, equipconfig.Ledxh);
                        }
                    }
                    Thread.Sleep(2000);
                    for (int i = ddsj; i >= 0; i--)
                    {
                        Msg(label_msg, panel_cp, carbj.CarPH + ",保持怠速：" + " " + i.ToString(), true);
                        ts2 = "怠速..." + " " + i.ToString();
                        if (ledcontrol != null)
                        {
                            if (equipconfig.Ledxh == "同济单排")
                            {
                                ledcontrol.writeLed("怠速:" + i.ToString() + "秒", 2, equipconfig.Ledxh);
                                Thread.Sleep(800);
                            }
                            else
                            {
                                ledcontrol.writeLed("第" + (cfcs + 1).ToString() + "次吹拂", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed(" 　 怠速:" + i.ToString() + "秒", 5, equipconfig.Ledxh);
                                Thread.Sleep(700);
                            }
                            //ledcontrol.writeLed(i.ToString() + "秒", 5);
                        }
                        else
                            Thread.Sleep(900);
                    }
                }
                //烟度计要求等待1秒后再发送数据
                if (ledcontrol != null)
                {
                    if (equipconfig.Ledxh == "同济单排")
                    {
                        ledcontrol.writeLed("请安置探头", 2, equipconfig.Ledxh);
                    }
                    else
                    {
                        ledcontrol.writeLed("测试即将开始　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("　　　请安置探头", 5, equipconfig.Ledxh);
                    }
                }
                if (btgconfig.BtgManualTantou)
                {
                    MessageBox.Show("确认探头是否已安好?", "系统提示");
                }
                else
                {
                    for (int i = 20; i >= 0; i--)
                    {
                        Msg(label_msg, panel_cp, "请安置好探头..." + i.ToString(), false);
                        ts2 = "请安置好探头..." + i.ToString();
                        Thread.Sleep(800);
                    }
                }
                Msg(label_msg, panel_cp, "探头已经安置完毕，即将开始测量。", false);
                statusconfigini.writeGlStatusData(statusconfigIni.ENUM_GL_STATUS.STATUS_TANTOU, "");
                if (ledcontrol != null)
                {
                    if (equipconfig.Ledxh == "同济单排")
                    {
                        ledcontrol.writeLed("即将开始测量", 2, equipconfig.Ledxh);
                    }
                    else
                    {
                        ledcontrol.writeLed("探头安置完毕　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("　　即将开始测量", 5, equipconfig.Ledxh);
                    }
                }
                DataTable btg_ahdatatable = new DataTable();
                btg_ahdatatable.Columns.Add("全程时序");
                btg_ahdatatable.Columns.Add("时序类别");
                btg_ahdatatable.Columns.Add("采样时序");
                btg_ahdatatable.Columns.Add("烟度值读数");
                btg_ahdatatable.Columns.Add("发动机转速");
                btg_ahdatatable.Columns.Add("油温");
                btg_ahdatatable.Columns.Add("环境温度");
                btg_ahdatatable.Columns.Add("大气压力");
                btg_ahdatatable.Columns.Add("相对湿度");
                DataRow drah = null;
                

                ts2 = "即将开始测量";
                cysx = 1;
                sxnb = 1;
                isReadRealTime = true;
                Thread.Sleep(500);
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.DAOWEI, GKSJ.ToString());
                Thread.Sleep(3000);
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.CHATANTOU, GKSJ.ToString());
                statusconfigini.writeNeuStatusData("StartTest", DateTime.Now.ToString());
                statusconfigini.writeGlStatusData(statusconfigIni.ENUM_GL_STATUS.STATUS_STARTSAMPLE, "");
                zyjs_data.StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                startTime = DateTime.Now;
                
                SaveData_status = true;
                isReadRealTime = false;
                if (btgconfig.btgclcs == 4)
                {
                    Thread.Sleep(500);
                    if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                        fla_502.clear_maxData();
                    else
                        flb_100.clear_maxData();
                    Thread.Sleep(100);
                    if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                        fla_502.Set_StableMeasure();
                    else
                        flb_100.Set_StableMeasure();
                    Thread.Sleep(100);
                    if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                        fla_502.Start_StableMeasure();
                    else
                        flb_100.Start_StableMeasure();
                    Thread.Sleep(100);
                    isReadRealTime = true;
                    maxvalue = 0;
                    maxzsvalue = 0;
                    maxywvalue = 0;
                    caculateStart = true;
                    sxnb = 1;
                    Msg(label_msgcs, panel1, "第" + sxnb.ToString() + "次测量", false);
                    Msg(label_msg, panel_cp, carbj.CarPH + "请迅猛将油门踩到底", true);
                    ts1 = "第" + sxnb.ToString() + "次测量";
                    ts2 = "请1s内踩至断油转速";
                    VRMzt = 1;
                    if (ledcontrol != null)
                    {
                        if (equipconfig.Ledxh == "同济单排")
                        {
                            ledcontrol.writeLed("请油门踩到底", 2, equipconfig.Ledxh);
                        }
                        else
                        {
                            ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("　　请油门踩到底", 5, equipconfig.Ledxh);
                        }
                    }
                    statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIASU, GKSJ.ToString());
                    Thread.Sleep(3000);

                    while (ZS < dyzs)
                    {
                        Msg(label_msg, panel_cp, carbj.CarPH + "未达到断油转速，请保持油门踩到底", true);
                        Thread.Sleep(400);
                    }
                    if (!isQxc || equipconfig.useJHJK)
                    {
                        for (int i = 2; i > 0; i--)
                        {
                            Msg(label_msg, panel_cp, carbj.CarPH + "维持" + i.ToString() + "s", true);
                            ts2 = "维持" + i.ToString() + "s";
                            Thread.Sleep(1000);
                        }
                    }
                    Msg(label_msg, panel_cp, carbj.CarPH + "请松开踏板", true);
                    ts2 = "松开踏板";
                    if (ledcontrol != null)
                    {
                        if (equipconfig.Ledxh == "同济单排")
                        {
                            ledcontrol.writeLed("请松开踏板", 2, equipconfig.Ledxh);
                        }
                        else
                        {
                            ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("　　　请松开踏板", 5, equipconfig.Ledxh);
                        }
                    }
                    statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIANCEZHONG, GKSJ.ToString());
                    if (equipconfig.useJHJK)
                    {
                        while (ZS >= 1000)
                        {
                            //Msg(label_msg, panel_cp, carbj.CarPH + "等待转速降至怠速转速", true);
                            Thread.Sleep(400);
                        }
                    }
                    Thread.Sleep(2000);
                    double preparedata = 0;
                    for (int i = ddsj; i >= 0; i--)
                    {
                        Msg(label_msg, panel_cp, carbj.CarPH + "保持怠速：" + " " + i.ToString(), true);
                        ts2 = "怠速..." + " " + i.ToString();
                        if (ledcontrol != null)
                        {
                            if (equipconfig.Ledxh == "同济单排")
                            {
                                ledcontrol.writeLed("怠速:" + i.ToString() + "秒", 2, equipconfig.Ledxh);
                                Thread.Sleep(800);
                            }
                            else
                            {
                                ledcontrol.writeLed("等待下次测量　　", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("　　　怠速:" + i.ToString() + "秒", 5, equipconfig.Ledxh);
                                Thread.Sleep(500);
                            }
                            //ledcontrol.writeLed(i.ToString() + "秒", 5);
                        }
                        else
                            Thread.Sleep(900);
                        if (i == 0)
                        {
                            isReadRealTime = false;
                            caculateStart = false;
                            Thread.Sleep(500);
                            if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                                fla_502.Stop_StableMeasure();
                            else
                                flb_100.Stop_StableMeasure();
                            Thread.Sleep(50);
                            //if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                            //    dycsmoke = fla_502.get_StableData(0.01f);
                            //else
                            //    dycsmoke = flb_100.get_StableData(0.01f);
                            //preparedata = dycsmoke.K;
                            //if (equipconfig.Ydjxh == "mqy_200" || equipconfig.Ydjxh == "flb_100" || equipconfig.Ydjxh.ToLower() == "cdf5000" || equipconfig.Ydjxh.ToLower() == "nht_1")
                                preparedata = maxvalue;
                            preclz1 = preparedata;
                            preclz2 = preparedata;
                            preclz3 = preparedata;
                            zyjs_data.PrepareData = preparedata.ToString("0.00");
                            drah = btg_ahdatatable.NewRow();
                            drah["全程时序"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            drah["时序类别"] = GKSJ.ToString("0");
                            drah["采样时序"] = GKSJ.ToString("0");
                            drah["烟度值读数"] = preclz1;
                            drah["发动机转速"] = maxzsvalue.ToString();
                            drah["油温"] = maxywvalue.ToString();
                            drah["环境温度"] = wd.ToString();
                            drah["大气压力"] = dqy.ToString();
                            drah["相对湿度"] = sd.ToString();
                            btg_ahdatatable.Rows.Add(drah);
                            maxvalue = 0; maxzsvalue = 0;
                            maxywvalue = 0;
                            isReadRealTime = true;
                        }
                    }
                    if(btgconfig.isYdjk)
                    {
                        if (preparedata < btgconfig.ydjk_value)
                        {
                            ts1 = "烟度值过低";
                            ts2 = "检测中止";
                            Msg(label_msg, panel_cp, "烟度值过低,检测中止,检查探头是否脱落", false);
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("烟度值过低　　　", 2, equipconfig.Ledxh);
                                ledcontrol.writeLed("　　检测中止　　", 5, equipconfig.Ledxh);
                            }
                            try
                            {
                                Th_get_FqandLl.Abort();
                            }
                            catch { }
                            zyjs_status = false;
                            JC_Status = false;
                            SaveData_status = false;
                            timer2.Stop();
                            this.BeginInvoke(new wt_void(Ref_Button));
                            return;
                        }
                    }
                    if (equipconfig.useJHJK)
                    {
                        thistimezslist.Clear();
                        for (int i = 0; i < GKSJ; i++)
                        {
                            if (Sxnblist[i] == sxnb.ToString())
                            {
                                thistimezslist.Add((int)Zslist[i]);
                            }
                        }
                        for (int i = 0; i < thistimezslist.Count; i++)
                        {
                            if (thistimezslist[i] >= 1000)
                            {
                                time_踩油门点 = i;
                                break;
                            }
                        }
                        maxzs = thistimezslist.Max();
                        time_断油点 = thistimezslist.IndexOf(maxzs);
                        zs_到底点 = (int)(maxzs * 0.67);
                        zs_松油点 = zs_到底点;
                        for (int i = 0; i <= time_断油点; i++)
                        {
                            if (thistimezslist[i] >= zs_到底点)
                            {
                                time_油门到底点 = i;
                                break;
                            }
                        }
                        for (int i = time_断油点; i < thistimezslist.Count; i++)
                        {
                            if (thistimezslist[i] <= zs_松油点)
                            {
                                time_松油门点 = i;
                                break;
                            }
                        }                       
                        
                        if (true)
                        {
                            if (time_油门到底点 - time_踩油门点 >= 3)
                            {
                                ts1 = "加油时间预警";
                                ts2 = "检测中止";
                                Msg(label_msg, panel_cp, "加油时间预警，超过3s,检测中止", false);
                                if (ledcontrol != null)
                                {
                                    ledcontrol.writeLed("加油时间预警　　", 2, equipconfig.Ledxh);
                                    ledcontrol.writeLed("　　检测中止　　", 5, equipconfig.Ledxh);
                                }
                                try
                                {
                                    Th_get_FqandLl.Abort();
                                }
                                catch { }
                                zyjs_status = false;
                                JC_Status = false;
                                SaveData_status = false;
                                timer2.Stop();
                                this.BeginInvoke(new wt_void(Ref_Button));
                                return;
                            }
                            if (time_松油门点 - time_油门到底点 < 2)
                            {
                                ts1 = "保持时间预警";
                                ts2 = "检测中止";
                                Msg(label_msg, panel_cp, "保持时间预警，少于2s,检测中止", false);
                                if (ledcontrol != null)
                                {
                                    ledcontrol.writeLed("保持时间预警　　", 2, equipconfig.Ledxh);
                                    ledcontrol.writeLed("　　检测中止　　", 5, equipconfig.Ledxh);
                                }
                                try
                                {
                                    Th_get_FqandLl.Abort();
                                }
                                catch { }
                                zyjs_status = false;
                                JC_Status = false;
                                SaveData_status = false;
                                timer2.Stop();
                                this.BeginInvoke(new wt_void(Ref_Button));
                                return;
                            }
                        }
                    }
                    sxnb++;
                    isReadRealTime = false;

                }
                else if (btgconfig.btgclcs == 6)
                {
                    sxnb = 1;
                    for (int clcs = 0; clcs < 3; clcs++)
                    {
                        Thread.Sleep(500);
                        if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                            fla_502.clear_maxData();
                        else
                            flb_100.clear_maxData();
                        Thread.Sleep(100);
                        if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                            fla_502.Set_StableMeasure();
                        else
                            flb_100.Set_StableMeasure();
                        Thread.Sleep(100);
                        if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                            fla_502.Start_StableMeasure();
                        else
                            flb_100.Start_StableMeasure();
                        Thread.Sleep(100);
                        isReadRealTime = true;
                        maxvalue = 0; maxzsvalue = 0;
                        maxywvalue = 0;
                        caculateStart = true;
                        Msg(label_msgcs, panel1, "第" + sxnb.ToString() + "次测量", false);
                        Msg(label_msg, panel_cp, carbj.CarPH + "请迅猛将油门踩到底", true);
                        ts1 = "第" + sxnb.ToString() + "次测量";
                        ts2 = "请1s内踩至断油转速";
                        VRMzt = 1;
                        if (ledcontrol != null)
                        {
                            if (equipconfig.Ledxh == "同济单排")
                            {
                                ledcontrol.writeLed("请油门踩到底", 2, equipconfig.Ledxh);
                            }
                            else
                            {
                                ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("　　请油门踩到底", 5, equipconfig.Ledxh);
                            }
                        }
                        statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIASU, GKSJ.ToString());
                        Thread.Sleep(3000);

                        while (ZS < dyzs)
                        {
                            Msg(label_msg, panel_cp, carbj.CarPH + "未达到断油转速，请保持油门踩到底", true);
                            Thread.Sleep(400);
                        }
                        if (!isQxc || equipconfig.useJHJK)
                        {
                            for (int i = 2; i > 0; i--)
                            {
                                Msg(label_msg, panel_cp, carbj.CarPH + "维持" + i.ToString() + "s", true);
                                ts2 = "维持" + i.ToString() + "s";
                                Thread.Sleep(1000);
                            }
                        }
                        Msg(label_msg, panel_cp, carbj.CarPH + "请松开踏板", true);
                        ts2 = "松开踏板";
                        if (ledcontrol != null)
                        {
                            if (equipconfig.Ledxh == "同济单排")
                            {
                                ledcontrol.writeLed("请松开踏板", 2, equipconfig.Ledxh);
                            }
                            else
                            {
                                ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("　　　请松开踏板", 5, equipconfig.Ledxh);
                            }
                        }
                        statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIANCEZHONG, GKSJ.ToString());
                        if (equipconfig.useJHJK)
                        {
                            while (ZS >= 1000)
                            {
                                //Msg(label_msg, panel_cp, carbj.CarPH + "等待转速降至怠速转速", true);
                                Thread.Sleep(400);
                            }
                        }
                        Thread.Sleep(2000);
                        double preparedata = 0;
                        for (int i = ddsj; i >= 0; i--)
                        {
                            Msg(label_msg, panel_cp, carbj.CarPH + "保持怠速：" + " " + i.ToString(), true);
                            ts2 = "怠速..." + " " + i.ToString();
                            if (ledcontrol != null)
                            {
                                if (equipconfig.Ledxh == "同济单排")
                                {
                                    ledcontrol.writeLed("怠速:" + i.ToString() + "秒", 2, equipconfig.Ledxh);
                                    Thread.Sleep(800);
                                }
                                else
                                {
                                    ledcontrol.writeLed("等待下次测量　　", 2, equipconfig.Ledxh);
                                    Thread.Sleep(200);
                                    ledcontrol.writeLed("　　　怠速:" + i.ToString() + "秒", 5, equipconfig.Ledxh);
                                    Thread.Sleep(500);
                                }
                                //ledcontrol.writeLed(i.ToString() + "秒", 5);
                            }
                            else
                                Thread.Sleep(900);
                            if (i == 0)
                            {
                                isReadRealTime = false;
                                caculateStart = false;
                                Thread.Sleep(500);
                                if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                                    fla_502.Stop_StableMeasure();
                                else
                                    flb_100.Stop_StableMeasure();
                                Thread.Sleep(50);
                                //if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                                //    dycsmoke = fla_502.get_StableData(0.01f);
                                //else
                                //    dycsmoke = flb_100.get_StableData(0.01f);
                                //preparedata = dycsmoke.K;
                                //if (equipconfig.Ydjxh == "mqy_200" || equipconfig.Ydjxh == "flb_100" || equipconfig.Ydjxh.ToLower() == "cdf5000" || equipconfig.Ydjxh.ToLower() == "nht_1")
                                    preparedata = maxvalue;
                                if (clcs == 0)
                                    preclz1 = preparedata;
                                else if (clcs == 1)
                                    preclz2 = preparedata;
                                else if (clcs == 2)
                                {
                                    preclz3 = preparedata;
                                    drah = btg_ahdatatable.NewRow();
                                    drah["全程时序"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    drah["时序类别"] = GKSJ.ToString("0");
                                    drah["采样时序"] = GKSJ.ToString("0");
                                    drah["烟度值读数"] = preclz3;
                                    drah["发动机转速"] = maxzsvalue.ToString();
                                    drah["油温"] = maxywvalue.ToString();
                                    drah["环境温度"] = wd.ToString();
                                    drah["大气压力"] = dqy.ToString();
                                    drah["相对湿度"] = sd.ToString();
                                    btg_ahdatatable.Rows.Add(drah);
                                }
                                maxvalue = 0; maxzsvalue = 0;
                                maxywvalue = 0;
                                zyjs_data.PrepareData = preparedata.ToString("0.00");
                                isReadRealTime = true;
                            }
                        }
                        if (btgconfig.isYdjk)
                        {
                            if (preparedata < btgconfig.ydjk_value)
                            {
                                ts1 = "烟度值过低";
                                ts2 = "检测中止";
                                Msg(label_msg, panel_cp, "烟度值过低,检测中止,检查探头是否脱落", false);
                                if (ledcontrol != null)
                                {
                                    ledcontrol.writeLed("烟度值过低　　　", 2, equipconfig.Ledxh);
                                    ledcontrol.writeLed("　　检测中止　　", 5, equipconfig.Ledxh);
                                }
                                try
                                {
                                    Th_get_FqandLl.Abort();
                                }
                                catch { }
                                zyjs_status = false;
                                JC_Status = false;
                                SaveData_status = false;
                                timer2.Stop();
                                this.BeginInvoke(new wt_void(Ref_Button));
                                return;
                            }
                        }
                        if (equipconfig.useJHJK)
                        {
                            thistimezslist.Clear();
                            for (int i = 0; i < GKSJ; i++)
                            {
                                if (Sxnblist[i] == sxnb.ToString())
                                {
                                    thistimezslist.Add((int)Zslist[i]);
                                }
                            }
                            for (int i = 0; i < thistimezslist.Count; i++)
                            {
                                if (thistimezslist[i] >= 1000)
                                {
                                    time_踩油门点 = i;
                                    break;
                                }
                            }
                            maxzs = thistimezslist.Max();
                            time_断油点 = thistimezslist.IndexOf(maxzs);
                            zs_到底点 = (int)(maxzs * 0.67);
                            zs_松油点 = zs_到底点;
                            for (int i = 0; i <= time_断油点; i++)
                            {
                                if (thistimezslist[i] >= zs_到底点)
                                {
                                    time_油门到底点 = i;
                                    break;
                                }
                            }
                            for (int i = time_断油点; i < thistimezslist.Count; i++)
                            {
                                if (thistimezslist[i] <= zs_松油点)
                                {
                                    time_松油门点 = i;
                                    break;
                                }
                            }

                            if (true)
                            {
                                if (time_油门到底点 - time_踩油门点 >= 3)
                                {
                                    ts1 = "加油时间预警";
                                    ts2 = "检测中止";
                                    Msg(label_msg, panel_cp, "加油时间预警，超过3s,检测中止", false);
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("加油时间预警　　", 2, equipconfig.Ledxh);
                                        ledcontrol.writeLed("　　检测中止　　", 5, equipconfig.Ledxh);
                                    }
                                    try
                                    {
                                        Th_get_FqandLl.Abort();
                                    }
                                    catch { }
                                    zyjs_status = false;
                                    JC_Status = false;
                                    SaveData_status = false;
                                    timer2.Stop();
                                    this.BeginInvoke(new wt_void(Ref_Button));
                                    return;
                                }
                                if (time_松油门点 - time_油门到底点 < 2)
                                {
                                    ts1 = "保持时间预警";
                                    ts2 = "检测中止";
                                    Msg(label_msg, panel_cp, "保持时间预警，少于2s,检测中止", false);
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("保持时间预警　　", 2, equipconfig.Ledxh);
                                        ledcontrol.writeLed("　　检测中止　　", 5, equipconfig.Ledxh);
                                    }
                                    try
                                    {
                                        Th_get_FqandLl.Abort();
                                    }
                                    catch { }
                                    zyjs_status = false;
                                    JC_Status = false;
                                    SaveData_status = false;
                                    timer2.Stop();
                                    this.BeginInvoke(new wt_void(Ref_Button));
                                    return;
                                }
                            }
                        }
                        sxnb++;
                        isReadRealTime = false;
                    }
                }
                else
                {
                    maxvalue = 0; maxzsvalue = 0;
                    maxywvalue = 0;
                    zyjs_data.PrepareData = "0.01";
                }
                Thread.Sleep(500);
                if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                    fla_502.clear_maxData();
                else
                    flb_100.clear_maxData();
                Thread.Sleep(100);
                if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                    fla_502.Set_StableMeasure();
                else
                    flb_100.Set_StableMeasure();
                Thread.Sleep(100);
                if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                    fla_502.Start_StableMeasure();
                else
                    flb_100.Start_StableMeasure();
                Thread.Sleep(100);
                isReadRealTime = true;
                caculateStart = true;
                Msg(label_msgcs, panel1, "第" + sxnb.ToString() + "次测量", false);
                Msg(label_msg, panel_cp, carbj.CarPH + "请迅猛将油门踩到底", true);
                ts1 = "第" + sxnb.ToString() + "次测量";
                ts2 = "请1s内踩至断油转速";
                VRMzt = 1;
                if (ledcontrol != null)
                {
                    if (equipconfig.Ledxh == "同济单排")
                    {
                        ledcontrol.writeLed("请油门踩到底", 2, equipconfig.Ledxh);
                    }
                    else
                    {
                        ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("　　请油门踩到底", 5, equipconfig.Ledxh);
                    }
                }
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIASU, GKSJ.ToString());
                statusconfigini.writeNeuStatusData("firstTest", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                Thread.Sleep(3000);                
                while (ZS < dyzs)
                {                    
                    Msg(label_msg, panel_cp, carbj.CarPH + "未达到断油转速，请保持油门踩到底", true);                    
                    Thread.Sleep(400);
                }
                if (!isQxc || equipconfig.useJHJK)
                {
                    for (int i = 2; i > 0; i--)
                    {
                        Msg(label_msg, panel_cp, carbj.CarPH + "维持" + i.ToString() + "s", true);
                        ts2 = "维持" + i.ToString() + "s";
                        Thread.Sleep(1000);
                    }
                }
                Msg(label_msg, panel_cp, carbj.CarPH + "请松开踏板", true);
                ts2 = "松开踏板";
                if (ledcontrol != null)
                {
                    if (equipconfig.Ledxh == "同济单排")
                    {
                        ledcontrol.writeLed("请松开踏板", 2, equipconfig.Ledxh);
                    }
                    else
                    {
                        ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("　　　请松开踏板", 5, equipconfig.Ledxh);
                    }
                }
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIANCEZHONG, GKSJ.ToString());
                if (equipconfig.useJHJK)
                {
                    while (ZS >= 1000)
                    {
                        //Msg(label_msg, panel_cp, carbj.CarPH + "等待转速降至怠速转速", true);
                        Thread.Sleep(400);
                    }
                }
                Thread.Sleep(2000);
                for (int i = ddsj; i >= 0; i--)
                {                    
                    Msg(label_msg, panel_cp, carbj.CarPH + "保持怠速：" + " " + i.ToString(), true);
                    ts2 = "怠速..." + " " + i.ToString();
                    if (ledcontrol != null)
                    {
                        if (equipconfig.Ledxh == "同济单排")
                        {
                            ledcontrol.writeLed("怠速:" + i.ToString() + "秒", 2, equipconfig.Ledxh);
                            Thread.Sleep(800);
                        }
                        else
                        {
                            ledcontrol.writeLed("等待下次测量　　", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("　　　怠速:" + i.ToString() + "秒", 5, equipconfig.Ledxh);

                            Thread.Sleep(500);
                        }
                        //ledcontrol.writeLed(i.ToString() + "秒", 5);
                    }
                    else
                        Thread.Sleep(900);
                    if (i == 0)
                    {
                        isReadRealTime = false;
                        caculateStart = false;
                        Thread.Sleep(500);
                        if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                            fla_502.Stop_StableMeasure();
                        else
                            flb_100.Stop_StableMeasure();
                        Thread.Sleep(50);
                        //if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                        //    dycsmoke = fla_502.get_StableData(0.01f);
                        //else
                        //    dycsmoke = flb_100.get_StableData(0.01f);
                        //dycclz = dycsmoke.K;
                        dyczs = maxzsvalue;
                        //if (equipconfig.Ydjxh == "mqy_200" || equipconfig.Ydjxh == "flb_100" || equipconfig.Ydjxh.ToLower() == "cdf5000" || equipconfig.Ydjxh.ToLower() == "nht_1")
                            dycclz = maxvalue;
                        if (btgconfig.btgDszsValue == 1)
                            dszs = maxzsvalue;
                        else
                            dszs = ZS;
                        maxvalue = 0; maxzsvalue = 0;
                        maxywvalue = 0;
                        //if (dycclz < 0.01) dycclz = 0.01;
                        dycyw = maxywvalue;
                        if (btgconfig.Zsj == "无")
                        {
                            dszs = 0;
                        }
                        /*else if (equipconfig.Ydjxh.ToLower() == "nht_1"&&btgconfig.Zsj=="烟度计")
                        {
                            dszs = 700 + DateTime.Now.Second;
                        }*/
                        else if(!btgconfig.RotateSpeedMonitor)
                        {
                            if (btgconfig.btgDszsValue == 1)
                                dszs = 3000 + DateTime.Now.Second;
                            else
                                dszs = 700 + DateTime.Now.Second;
                        }
                        datagridview_msg(dataGridView1, "结果", 1, dycclz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "转速", 1, dyczs.ToString("0"));
                        data1 = dycclz.ToString("0.00");
                        if (ledcontrol != null)
                        {
                            if (equipconfig.Ledxh == "同济单排")
                            {
                                ledcontrol.writeLed("K:"+data1, 2, equipconfig.Ledxh);
                            }
                            else
                            {
                                ledcontrol.writeLed("K:" + data1, 5, equipconfig.Ledxh);
                            }
                            Thread.Sleep(1000);//结果显示1s
                        }
                        drah = btg_ahdatatable.NewRow();
                        drah["全程时序"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        drah["时序类别"] = GKSJ.ToString("0");
                        drah["采样时序"] = GKSJ.ToString("0");
                        drah["烟度值读数"] = data1;
                        drah["发动机转速"] = dyczs.ToString();
                        drah["油温"] = dycyw.ToString();
                        drah["环境温度"] = wd.ToString();
                        drah["大气压力"] = dqy.ToString();
                        drah["相对湿度"] = sd.ToString();
                        btg_ahdatatable.Rows.Add(drah);
                        isReadRealTime = true;
                    }
                }
                if (btgconfig.isYdjk)
                {
                    if (dycclz < btgconfig.ydjk_value)
                    {
                        ts1 = "烟度值过低";
                        ts2 = "检测中止";
                        Msg(label_msg, panel_cp, "烟度值过低,检测中止,检查探头是否脱落", false);
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("烟度值过低　　　", 2, equipconfig.Ledxh);
                            ledcontrol.writeLed("　　检测中止　　", 5, equipconfig.Ledxh);
                        }
                        try
                        {
                            Th_get_FqandLl.Abort();
                        }
                        catch { }
                        zyjs_status = false;
                        JC_Status = false;
                        SaveData_status = false;
                        timer2.Stop();
                        this.BeginInvoke(new wt_void(Ref_Button));
                        return;
                    }
                }
                if (equipconfig.useJHJK)
                {
                    thistimezslist.Clear();
                    for (int i = 0; i < GKSJ; i++)
                    {
                        if (Sxnblist[i] == sxnb.ToString())
                        {
                            thistimezslist.Add((int)Zslist[i]);
                        }
                    }
                    for (int i = 0; i < thistimezslist.Count; i++)
                    {
                        if (thistimezslist[i] >= 1000)
                        {
                            time_踩油门点 = i;
                            break;
                        }
                    }
                    maxzs = thistimezslist.Max();
                    time_断油点 = thistimezslist.IndexOf(maxzs);
                    zs_到底点 = (int)(maxzs * 0.67);
                    zs_松油点 = zs_到底点;
                    for (int i = 0; i <= time_断油点; i++)
                    {
                        if (thistimezslist[i] >= zs_到底点)
                        {
                            time_油门到底点 = i;
                            break;
                        }
                    }
                    for (int i = time_断油点; i < thistimezslist.Count; i++)
                    {
                        if (thistimezslist[i] <= zs_松油点)
                        {
                            time_松油门点 = i;
                            break;
                        }
                    }
                    if (true)
                    {
                        if (time_油门到底点 - time_踩油门点 >= 3)
                        {
                            ts1 = "加油时间预警";
                            ts2 = "检测中止";
                            Msg(label_msg, panel_cp, "加油时间预警，超过3s,检测中止", false);
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("加油时间预警　　", 2, equipconfig.Ledxh);
                                ledcontrol.writeLed("　　检测中止　　", 5, equipconfig.Ledxh);
                            }
                            try
                            {
                                Th_get_FqandLl.Abort();
                            }
                            catch { }
                            zyjs_status = false;
                            JC_Status = false;
                            SaveData_status = false;
                            timer2.Stop();
                            this.BeginInvoke(new wt_void(Ref_Button));
                            return;
                        }
                        if (time_松油门点 - time_油门到底点 < 2)
                        {
                            ts1 = "保持时间预警";
                            ts2 = "检测中止";
                            Msg(label_msg, panel_cp, "保持时间预警，少于2s,检测中止", false);
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("保持时间预警　　", 2, equipconfig.Ledxh);
                                ledcontrol.writeLed("　　检测中止　　", 5, equipconfig.Ledxh);
                            }
                            try
                            {
                                Th_get_FqandLl.Abort();
                            }
                            catch { }
                            zyjs_status = false;
                            JC_Status = false;
                            SaveData_status = false;
                            timer2.Stop();
                            this.BeginInvoke(new wt_void(Ref_Button));
                            return;
                        }
                    }
                }
                sxnb++;
                isReadRealTime = false;
                Thread.Sleep(500);
                if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                    fla_502.clear_maxData();
                else
                    flb_100.clear_maxData();
                Thread.Sleep(100);
                if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                    fla_502.Set_StableMeasure();
                else
                    flb_100.Set_StableMeasure();
                Thread.Sleep(100);
                if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                    fla_502.Start_StableMeasure();
                else
                    flb_100.Start_StableMeasure();
                Thread.Sleep(100);
                //sxnb = 3;
                isReadRealTime = true;
                caculateStart = true;
                Msg(label_msgcs, panel1, "第" + sxnb.ToString() + "次测量", false);
                Msg(label_msg, panel_cp, carbj.CarPH + "请迅猛将油门踩到底", true);
                ts1 = "第" + sxnb.ToString() + "次测量";
                ts2 = "请1s内踩至断油转速";
                VRMzt = 1;
                if (ledcontrol != null)
                {
                    if (equipconfig.Ledxh == "同济单排")
                    {
                        ledcontrol.writeLed("请油门踩到底", 2, equipconfig.Ledxh);
                    }
                    else
                    {
                        ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("　　请油门踩到底", 5, equipconfig.Ledxh);
                    }
                }
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIASU, GKSJ.ToString());
                statusconfigini.writeNeuStatusData("secondTest", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                Thread.Sleep(3000);
                
                while (ZS < dyzs)
                {
                    Msg(label_msg, panel_cp, carbj.CarPH + "未达到断油转速，请保持油门踩到底", true);
                    
                    Thread.Sleep(400);
                }
                if (!isQxc || equipconfig.useJHJK)
                {
                    for (int i = 2; i > 0; i--)
                    {
                        Msg(label_msg, panel_cp, carbj.CarPH + "维持" + i.ToString() + "s", true);
                        ts2 = "维持" + i.ToString() + "s";
                        Thread.Sleep(1000);
                    }
                }
                Msg(label_msg, panel_cp, carbj.CarPH + "请松开踏板", true);
                ts2 = "松开踏板";
                if (ledcontrol != null)
                {
                    if (equipconfig.Ledxh == "同济单排")
                    {
                        ledcontrol.writeLed("请松开踏板", 2, equipconfig.Ledxh);
                    }
                    else
                    {
                        ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("　　　请松开踏板", 5, equipconfig.Ledxh);
                    }
                }
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIANCEZHONG, GKSJ.ToString());
                if (equipconfig.useJHJK)
                {
                    while (ZS >= 1000)
                    {
                        //Msg(label_msg, panel_cp, carbj.CarPH + "等待转速降至怠速转速", true);
                        Thread.Sleep(400);
                    }
                }
                Thread.Sleep(2000);
                for (int i = ddsj; i >= 0; i--)
                {                    
                    Msg(label_msg, panel_cp, carbj.CarPH + "保持怠速：" + " " + i.ToString(), true);
                    ts2 = "怠速..." + " " + i.ToString();
                    if (ledcontrol != null)
                    {
                        if (equipconfig.Ledxh == "同济单排")
                        {
                            ledcontrol.writeLed("怠速:" + i.ToString() + "秒", 2, equipconfig.Ledxh);
                            Thread.Sleep(800);
                        }
                        else
                        {
                            ledcontrol.writeLed("等待下次测量　　", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("　　　怠速:" + i.ToString() + "秒", 5, equipconfig.Ledxh);

                            Thread.Sleep(500);
                        }
                        //ledcontrol.writeLed(i.ToString() + "秒", 5);
                    }
                    else
                        Thread.Sleep(900);
                    if (i == 0)
                    {
                        isReadRealTime = false;
                        caculateStart = false;
                        Thread.Sleep(500);
                        if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                            fla_502.Stop_StableMeasure();
                        else
                            flb_100.Stop_StableMeasure();
                        Thread.Sleep(50);
                        //if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                        //    decsmoke = fla_502.get_StableData(0.01f);
                        //else
                        //    decsmoke = flb_100.get_StableData(0.01f);
                        //decclz = decsmoke.K;
                        deczs = maxzsvalue;
                        //if (equipconfig.Ydjxh == "mqy_200" || equipconfig.Ydjxh == "flb_100" || equipconfig.Ydjxh.ToLower() == "cdf5000" || equipconfig.Ydjxh.ToLower() == "nht_1")
                            decclz = maxvalue;
                        maxvalue = 0;
                        maxzsvalue = 0;
                        maxywvalue = 0;
                        //if (decclz < 0.01) decclz = 0.01;
                        decyw = maxywvalue;
                        datagridview_msg(dataGridView1, "结果", 2, decclz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "转速", 2, deczs.ToString("0"));
                        data2 = decclz.ToString("0.00");
                        if (ledcontrol != null)
                        {
                            if (equipconfig.Ledxh == "同济单排")
                            {
                                ledcontrol.writeLed("K:" + data2, 2, equipconfig.Ledxh);
                            }
                            else
                            {
                                ledcontrol.writeLed("K:" + data2, 5, equipconfig.Ledxh);
                            }
                            Thread.Sleep(1000);//结果显示1s
                        }
                        drah = btg_ahdatatable.NewRow();
                        drah["全程时序"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        drah["时序类别"] = GKSJ.ToString("0");
                        drah["采样时序"] = GKSJ.ToString("0");
                        drah["烟度值读数"] = data2;
                        drah["发动机转速"] = deczs.ToString();
                        drah["油温"] = decyw.ToString();
                        drah["环境温度"] = wd.ToString();
                        drah["大气压力"] = dqy.ToString();
                        drah["相对湿度"] = sd.ToString();
                        btg_ahdatatable.Rows.Add(drah);
                        isReadRealTime = true;
                    }
                }
                if (btgconfig.isYdjk)
                {
                    if (decclz < btgconfig.ydjk_value)
                    {
                        ts1 = "烟度值过低";
                        ts2 = "检测中止";
                        Msg(label_msg, panel_cp, "烟度值过低,检测中止,检查探头是否脱落", false);
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("烟度值过低　　　", 2, equipconfig.Ledxh);
                            ledcontrol.writeLed("　　检测中止　　", 5, equipconfig.Ledxh);
                        }
                        try
                        {
                            Th_get_FqandLl.Abort();
                        }
                        catch { }
                        zyjs_status = false;
                        JC_Status = false;
                        SaveData_status = false;
                        timer2.Stop();
                        this.BeginInvoke(new wt_void(Ref_Button));
                        return;
                    }
                }
                if (equipconfig.useJHJK)
                {
                    thistimezslist.Clear();
                    for (int i = 0; i < GKSJ; i++)
                    {
                        if (Sxnblist[i] == sxnb.ToString())
                        {
                            thistimezslist.Add((int)Zslist[i]);
                        }
                    }
                    for (int i = 0; i < thistimezslist.Count; i++)
                    {
                        if (thistimezslist[i] >= 1000)
                        {
                            time_踩油门点 = i;
                            break;
                        }
                    }
                    maxzs = thistimezslist.Max();
                    time_断油点 = thistimezslist.IndexOf(maxzs);
                    zs_到底点 = (int)(maxzs * 0.67);
                    zs_松油点 = zs_到底点;
                    for (int i = 0; i <= time_断油点; i++)
                    {
                        if (thistimezslist[i] >= zs_到底点)
                        {
                            time_油门到底点 = i;
                            break;
                        }
                    }
                    for (int i = time_断油点; i < thistimezslist.Count; i++)
                    {
                        if (thistimezslist[i] <= zs_松油点)
                        {
                            time_松油门点 = i;
                            break;
                        }
                    }

                    if (true)
                    {
                        if (time_油门到底点 - time_踩油门点 >= 3)
                        {
                            ts1 = "加油时间预警";
                            ts2 = "检测中止";
                            Msg(label_msg, panel_cp, "加油时间预警，超过3s,检测中止", false);
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("加油时间预警　　", 2, equipconfig.Ledxh);
                                ledcontrol.writeLed("　　检测中止　　", 5, equipconfig.Ledxh);
                            }
                            try
                            {
                                Th_get_FqandLl.Abort();
                            }
                            catch { }
                            zyjs_status = false;
                            JC_Status = false;
                            SaveData_status = false;
                            timer2.Stop();
                            this.BeginInvoke(new wt_void(Ref_Button));
                            return;
                        }
                        if (time_松油门点 - time_油门到底点 < 2)
                        {
                            ts1 = "保持时间预警";
                            ts2 = "检测中止";
                            Msg(label_msg, panel_cp, "保持时间预警，少于2s,检测中止", false);
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("保持时间预警　　", 2, equipconfig.Ledxh);
                                ledcontrol.writeLed("　　检测中止　　", 5, equipconfig.Ledxh);
                            }
                            try
                            {
                                Th_get_FqandLl.Abort();
                            }
                            catch { }
                            zyjs_status = false;
                            JC_Status = false;
                            SaveData_status = false;
                            timer2.Stop();
                            this.BeginInvoke(new wt_void(Ref_Button));
                            return;
                        }
                    }
                }
                sxnb++;
                isReadRealTime = false;
                Thread.Sleep(500);
                if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                    fla_502.clear_maxData();
                else
                    flb_100.clear_maxData();
                Thread.Sleep(100);
                if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                    fla_502.Set_StableMeasure();
                else
                    flb_100.Set_StableMeasure();
                Thread.Sleep(100);
                if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                    fla_502.Start_StableMeasure();
                else
                    flb_100.Start_StableMeasure();
                Thread.Sleep(100);
                isReadRealTime = true;
                //sxnb = 4;
                caculateStart = true;
                Msg(label_msgcs, panel1, "第" + sxnb.ToString() + "次测量", false);
                Msg(label_msg, panel_cp, carbj.CarPH + "请迅猛将油门踩到底", true);
                ts1 = "第" + sxnb.ToString() + "次测量";
                ts2 = "请1s内踩至断油转速";
                VRMzt = 1;
                if (ledcontrol != null)
                {
                    if (equipconfig.Ledxh == "同济单排")
                    {
                        ledcontrol.writeLed("请油门踩到底", 2, equipconfig.Ledxh);
                    }
                    else
                    {
                        ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("　　请油门踩到底", 5, equipconfig.Ledxh);
                    }
                }
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIASU, GKSJ.ToString());
                statusconfigini.writeNeuStatusData("thirdTest", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                Thread.Sleep(3000);
                
                while (ZS < dyzs)
                {
                    Msg(label_msg, panel_cp, carbj.CarPH + "未达到断油转速，请保持油门踩到底", true);

                    Thread.Sleep(400);
                }
                if (!isQxc || equipconfig.useJHJK)
                {
                    for (int i = 2; i > 0; i--)
                    {
                        Msg(label_msg, panel_cp, carbj.CarPH + "维持" + i.ToString() + "s", true);
                        ts2 = "维持" + i.ToString() + "s";
                        Thread.Sleep(1000);
                    }
                }
                Msg(label_msg, panel_cp, carbj.CarPH + "请松开踏板", true);
                ts2 = "松开踏板";
                if (ledcontrol != null)
                {
                    if (equipconfig.Ledxh == "同济单排")
                    {
                        ledcontrol.writeLed("请松开踏板", 2, equipconfig.Ledxh);
                    }
                    else
                    {
                        ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("　　　请松开踏板", 5, equipconfig.Ledxh);
                    }
                }
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIANCEZHONG, GKSJ.ToString());
                if (equipconfig.useJHJK)
                {
                    while (ZS >= 1000)
                    {
                        //Msg(label_msg, panel_cp, carbj.CarPH + "等待转速降至怠速转速", true);
                        Thread.Sleep(400);
                    }
                }
                Thread.Sleep(2000);
                for (int i = ddsj; i >= 0; i--)
                {                    
                    Msg(label_msg, panel_cp, carbj.CarPH + "保持怠速：" + " " + i.ToString(), true);
                    ts2 = "怠速..." + " " + i.ToString();
                    if (ledcontrol != null)
                    {
                        if (equipconfig.Ledxh == "同济单排")
                        {
                            ledcontrol.writeLed("怠速:" + i.ToString() + "秒", 2, equipconfig.Ledxh);
                            Thread.Sleep(800);
                        }
                        else
                        {
                            ledcontrol.writeLed("等待检测结束　　", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("　　　怠速:" + i.ToString() + "秒", 5, equipconfig.Ledxh);

                            Thread.Sleep(500);
                        }
                        //ledcontrol.writeLed(i.ToString() + "秒", 5);
                    }
                    else
                        Thread.Sleep(900);
                    if (i == 0)
                    {
                        isReadRealTime = false;
                        caculateStart = false;
                        Thread.Sleep(500);
                        if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                            fla_502.Stop_StableMeasure();
                        else
                            flb_100.Stop_StableMeasure();
                        Thread.Sleep(50);
                        //if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                        //    dscsmoke = fla_502.get_StableData(0.01f);
                        //else
                        //    dscsmoke = flb_100.get_StableData(0.01f);
                        //dscclz = dscsmoke.K;
                        dsczs = maxzsvalue;
                        //if (equipconfig.Ydjxh == "mqy_200" || equipconfig.Ydjxh == "flb_100" || equipconfig.Ydjxh.ToLower() == "cdf5000" || equipconfig.Ydjxh.ToLower() == "nht_1")
                            dscclz = maxvalue;
                        maxvalue = 0;
                        maxzsvalue = 0;
                        maxywvalue = 0;
                        //if (dscclz < 0.01) dscclz = 0.01;
                        dscyw = maxywvalue;
                        datagridview_msg(dataGridView1, "结果", 3, dscclz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "转速", 3, dyczs.ToString("0"));
                        data3 = dscclz.ToString("0.00");
                        if (ledcontrol != null)
                        {
                            if (equipconfig.Ledxh == "同济单排")
                            {
                                ledcontrol.writeLed("K:" + data3, 2, equipconfig.Ledxh);
                            }
                            else
                            {
                                ledcontrol.writeLed("K:" + data3, 5, equipconfig.Ledxh);
                            }
                            Thread.Sleep(1000);//结果显示1s
                        }

                        drah = btg_ahdatatable.NewRow();
                        drah["全程时序"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        drah["时序类别"] = GKSJ.ToString("0");
                        drah["采样时序"] = GKSJ.ToString("0");
                        drah["烟度值读数"] = data3;
                        drah["发动机转速"] = dsczs.ToString();
                        drah["油温"] = dscyw.ToString();
                        drah["环境温度"] = wd.ToString();
                        drah["大气压力"] = dqy.ToString();
                        drah["相对湿度"] = sd.ToString();
                        btg_ahdatatable.Rows.Add(drah);
                        isReadRealTime = true;
                    }
                }
                if (btgconfig.isYdjk)
                {
                    if (dscclz < btgconfig.ydjk_value)
                    {
                        ts1 = "烟度值过低";
                        ts2 = "检测中止";
                        Msg(label_msg, panel_cp, "烟度值过低,检测中止,检查探头是否脱落", false);
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("烟度值过低　　　", 2, equipconfig.Ledxh);
                            ledcontrol.writeLed("　　检测中止　　", 5, equipconfig.Ledxh);
                        }
                        try
                        {
                            Th_get_FqandLl.Abort();
                        }
                        catch { }
                        zyjs_status = false;
                        JC_Status = false;
                        SaveData_status = false;
                        timer2.Stop();
                        this.BeginInvoke(new wt_void(Ref_Button));
                        return;
                    }
                }
                if (equipconfig.useJHJK)
                {
                    thistimezslist.Clear();
                    for (int i = 0; i < GKSJ; i++)
                    {
                        if (Sxnblist[i] == sxnb.ToString())
                        {
                            thistimezslist.Add((int)Zslist[i]);
                        }
                    }
                    for (int i = 0; i < thistimezslist.Count; i++)
                    {
                        if (thistimezslist[i] >= 1000)
                        {
                            time_踩油门点 = i;
                            break;
                        }
                    }
                    maxzs = thistimezslist.Max();
                    time_断油点 = thistimezslist.IndexOf(maxzs);
                    zs_到底点 = (int)(maxzs * 0.67);
                    zs_松油点 = zs_到底点;
                    for (int i = 0; i <= time_断油点; i++)
                    {
                        if (thistimezslist[i] >= zs_到底点)
                        {
                            time_油门到底点 = i;
                            break;
                        }
                    }
                    for (int i = time_断油点; i < thistimezslist.Count; i++)
                    {
                        if (thistimezslist[i] <= zs_松油点)
                        {
                            time_松油门点 = i;
                            break;
                        }
                    }

                    if (true)
                    {
                        if (time_油门到底点 - time_踩油门点 >= 3)
                        {
                            ts1 = "加油时间预警";
                            ts2 = "检测中止";
                            Msg(label_msg, panel_cp, "加油时间预警，超过3s,检测中止", false);
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("加油时间预警　　", 2, equipconfig.Ledxh);
                                ledcontrol.writeLed("　　检测中止　　", 5, equipconfig.Ledxh);
                            }
                            try
                            {
                                Th_get_FqandLl.Abort();
                            }
                            catch { }
                            zyjs_status = false;
                            JC_Status = false;
                            SaveData_status = false;
                            timer2.Stop();
                            this.BeginInvoke(new wt_void(Ref_Button));
                            return;
                        }
                        if (time_松油门点 - time_油门到底点 < 2)
                        {
                            ts1 = "保持时间预警";
                            ts2 = "检测中止";
                            Msg(label_msg, panel_cp, "保持时间预警，少于2s,检测中止", false);
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("保持时间预警　　", 2, equipconfig.Ledxh);
                                ledcontrol.writeLed("　　检测中止　　", 5, equipconfig.Ledxh);
                            }
                            try
                            {
                                Th_get_FqandLl.Abort();
                            }
                            catch { }
                            zyjs_status = false;
                            JC_Status = false;
                            SaveData_status = false;
                            timer2.Stop();
                            this.BeginInvoke(new wt_void(Ref_Button));
                            return;
                        }
                    }
                }
                data4 = ((dycclz + decclz + dscclz) / 3.0).ToString("0.00");
                
                int gksjcount = GKSJ - 1;
                zyjs_status = false;
                SaveData_status = false;
                JC_Status = false;
                sxnb++;

                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.GUOCHE, GKSJ.ToString());
                statusconfigini.writeGlStatusData(statusconfigIni.ENUM_GL_STATUS.STATUS_ENDSAMPLE, "");
                if (ledcontrol != null)
                {
                    if (equipconfig.Ledxh == "同济单排")
                    {
                        ledcontrol.writeLed("平均值:" + data4, 2, equipconfig.Ledxh);
                    }
                    else
                    {
                        ledcontrol.writeLed("平均值:" + data4, 5, equipconfig.Ledxh);
                    }
                    Thread.Sleep(1000);//结果显示1s
                }
                //statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.GUOCHE, JCSJ.ToString());
                Thread.Sleep(2000);
                if (equipconfig.useJHJK)
                {
                    int useyears = DateTime.Now.Year - carbj.scrq.Year;
                    if (useyears >= 3)
                    {
                        if (dycclz<0.03&&decclz<0.03&&dscclz<0.03)
                        {
                            ts1 = "3年车K值均过低";
                            ts2 = "检测结果无效";
                            Msg(label_msg, panel_cp, "3年车K值均过低，检测结果无效", false);
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("3年车K值过低", 2, equipconfig.Ledxh);
                                ledcontrol.writeLed("　检测结果无效　", 5, equipconfig.Ledxh);
                            }
                            try
                            {
                                Th_get_FqandLl.Abort();
                            }
                            catch { }
                            zyjs_status = false;
                            JC_Status = false;
                            SaveData_status = false;
                            timer2.Stop();
                            this.BeginInvoke(new wt_void(Ref_Button));
                            return;
                        }
                    }
                }
                this.BeginInvoke(new wt_void(Ref_Button));
                Ref_Control_Text(label_st, "检测完成");
                Msg(label_msgcs, panel1, "检测完成", false);
                ts1 = "检测完成";
                ts2 = "移除探头驶离检测线";
                if (ledcontrol != null)
                {
                    if (equipconfig.Ledxh == "同济单排")
                    {
                        ledcontrol.writeLed("测试完成", 2, equipconfig.Ledxh);
                    }
                    else
                    {
                        ledcontrol.writeLed("测试完成　　　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("　　请移除探测头", 5, equipconfig.Ledxh);
                    }
                }
                Msg(label_msgcs, panel1, "检测完成", false);
                Msg(label_msg, panel_cp, carbj.CarPH + "测试完成,请移除探头后驶离检测线" , true);
                Thread.Sleep(1000);
                DataTable btg_datatable = new DataTable();
                btg_datatable.Columns.Add("全程时序");
                btg_datatable.Columns.Add("时序类别");
                btg_datatable.Columns.Add("采样时序");
                btg_datatable.Columns.Add("烟度值读数");
                btg_datatable.Columns.Add("光吸收系数K");
                btg_datatable.Columns.Add("SFN");
                btg_datatable.Columns.Add("发动机转速");
                btg_datatable.Columns.Add("转速");
                btg_datatable.Columns.Add("油温");
                btg_datatable.Columns.Add("环境温度");
                btg_datatable.Columns.Add("大气压力");
                btg_datatable.Columns.Add("相对湿度");
                DataRow dr = null;
                for (int i = 0; i < gksjcount; i++)
                {
                    dr = btg_datatable.NewRow();
                    dr["全程时序"] = Qcsxlist[i];
                    dr["时序类别"] = Sxnblist[i];
                    dr["采样时序"] = Cysxlist[i].ToString("0");
                    dr["烟度值读数"] = Klist[i].ToString("0.00");
                    dr["光吸收系数K"] = Klist[i].ToString("0.00");
                    dr["SFN"] = Nslist[i].ToString("0.00");
                    dr["发动机转速"] = Zslist[i].ToString("0");
                    dr["转速"] = Zslist[i].ToString("0");
                    dr["油温"] = Ywlist[i].ToString("0.0");
                    dr["环境温度"] = wdlist[i];
                    dr["大气压力"] = dqylist[i];
                    dr["相对湿度"] = sdlist[i];
                    btg_datatable.Rows.Add(dr);
                }
                
                zyjs_status = false;//停止获取烟度计数据
                JC_Status = false;
                SaveData_status = false;
                yw = (dycyw + decyw + dscyw) / 3f;
                double averagK = (dycclz + decclz + dscclz) / 3.0;
                zyjs_data.CarID = carbj.CarID;
                zyjs_data.Wd = wd.ToString("0.0");
                zyjs_data.Sd = sd.ToString("0.0");
                zyjs_data.Dqy = dqy.ToString("0.0");
                zyjs_data.prepareData1 = preclz1.ToString("0.00");
                zyjs_data.prepareData2 = preclz2.ToString("0.00");
                zyjs_data.prepareData3 = preclz3.ToString("0.00");
                zyjs_data.FirstData = dycclz.ToString("0.00");
                zyjs_data.SecondData = decclz.ToString("0.00");
                zyjs_data.ThirdData = dscclz.ToString("0.00");
                zyjs_data.Dszs = dszs.ToString("0");
                zyjs_data.Yw = yw.ToString("0.0");
                zyjs_data.SmokeAvg = averagK.ToString("0.00");
                zyjs_data.Rev1 = dyczs.ToString("0");
                zyjs_data.Rev2 = deczs.ToString("0");
                zyjs_data.Rev3 = dsczs.ToString("0");
                zyjs_data.StopReason = "0";
                csvwriter.SaveCSV(btg_datatable, "C:/jcdatatxt/" + carbj.CarID + ".csv");
                csvwriter.SaveCSV(btg_ahdatatable, "C:/jcdatatxt/" + carbj.CarID + "ah.csv");//写安徽联网需要的过程数据
                zyjsdatacontrol.writeJzjsData(zyjs_data);
                zyjsIsFinished = true;
                this.Close();
            }
            catch (Exception)
            {
                
            }
        }

        public void Ref_Button()
        {
            button_ss.Text = "重新检测";
            JC_Status = false;
            // button_ss.Enabled = false;
        }

        public void Ref_Control_label(string controlname, string text)
        {
            foreach (Label lab in this.Controls.Find(controlname, true))
            {
                lab.Text = text;
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
        /// <summary>
        /// 信息显示
        /// </summary>
        /// <param name="Msgowner">信息显示的Label控件</param>
        /// <param name="Msgfather">Label控件的父级Panel</param>
        /// <param name="Msgstr">要显示的信息</param>
        /// <param name="Update_DB">是不是要更新到检测状态</param>
        public void Msg(Label Msgowner, Panel Msgfather, string Msgstr, bool Update_DB)
        {
            try
            {
                BeginInvoke(new wtlsb(Msg_Show), Msgowner, Msgstr, Update_DB);
                BeginInvoke(new wtlp(Msg_Position), Msgowner, Msgfather);
            }
            catch (Exception)
            {
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (JC_Status == false)
                {
                    isReadRealTime = false;
                    datagridview_msg(dataGridView1, "结果", 0, "--");
                    datagridview_msg(dataGridView1, "结果", 1, "--");
                    datagridview_msg(dataGridView1, "结果", 2, "--");
                    datagridview_msg(dataGridView1, "结果", 3, "--");
                    jctime = DateTime.Now.ToString();
                    timer2.Start();
                    TH_ST = new Thread(Jc_Exe);
                    TH_ST.Start();
                    Th_get_FqandLl = new Thread(Fq_Detect);
                    Th_get_FqandLl.Start();
                    JC_Status = true;
                    zyjs_status = false;
                    SaveData_status = false;
                    button_ss.Text = "停止检测";
                }
                else
                {
                    caculateStart = false;
                    maxvalue = 0; maxzsvalue = 0;
                    maxywvalue = 0;
                    TH_ST.Abort();
                    try
                    {
                        Th_get_FqandLl.Abort();
                    }
                    catch
                    { }
                    //TH_YD.Abort();
                    zyjs_status = false;
                    //timer1.Stop();
                    JC_Status = false;
                    SaveData_status = false;
                    timer2.Stop();
                    Msg(label_msg, panel1, carbj.CarPH + " 检测已停止", true);

                    if (ledcontrol != null)
                    {
                        Thread.Sleep(500);
                        if (equipconfig.Ledxh == "同济单排")
                        {
                            ledcontrol.writeLed("测试被终止", 2, equipconfig.Ledxh);
                        }
                        else
                        {
                            ledcontrol.writeLed("测试被终止", 2, equipconfig.Ledxh);
                        }
                        //ledcontrol.writeLed("请移除探测头", 5);
                    }
                    ts2 = "测试被终止";
                    button_ss.Text = "重新检测";
                    isReadRealTime = false;
                }
            }
            catch (Exception)
            {
            }
        }

        private void Zyjs_Btg_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!zyjsIsFinished)
                {
                    if (MessageBox.Show("测试未完成，确认退出？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        if (ledcontrol != null)
                        {
                            if (equipconfig.Ledxh == "同济单排")
                            {
                                ledcontrol.writeLed("欢迎参检", 2, equipconfig.Ledxh);
                            }
                            else
                            {
                                ledcontrol.writeLed("小心驾驶安全第一", 2);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("　　欢迎参检　　", 5);
                            }
                        }
                        zyjs_data.CarID = carbj.CarID;
                        zyjs_data.Wd = "-1";
                        zyjs_data.Sd = "-1";
                        zyjs_data.Dqy = "-1";
                        zyjs_data.FirstData = "-1";
                        zyjs_data.SecondData = "-1";
                        zyjs_data.ThirdData = "-1";
                        zyjs_data.PrepareData = "-1";
                        zyjs_data.Dszs = "-1";
                        zyjs_data.Yw ="-1";
                        zyjs_data.StopReason = "9";
                        zyjs_data.StartTime = "";
                        zyjs_data.SmokeAvg = "-1";
                        zyjs_data.Rev1 = "-1";
                        zyjs_data.Rev2 = "-1";
                        zyjs_data.Rev3 = "-1";
                        zyjs_data.prepareData1 = "-1";
                        zyjs_data.prepareData2 = "-1";
                        zyjs_data.prepareData3 = "-1";

                        zyjsdatacontrol.writeJzjsData(zyjs_data);
                        if (TH_ST != null) TH_ST.Abort();
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
                        if (vmt_2000 != null)
                        {
                            if (vmt_2000.ComPort_1.IsOpen)
                                vmt_2000.ComPort_1.Close();
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
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else 
                {
                    if (ledcontrol != null)
                    {
                        if (equipconfig.Ledxh == "同济单排")
                        {
                            ledcontrol.writeLed("欢迎参检", 2, equipconfig.Ledxh);
                        }
                        else
                        {
                            ledcontrol.writeLed("小心驾驶安全第一", 2);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("　　欢迎参检　　", 5);
                        }
                    }
                    if (Th_get_FqandLl != null) Th_get_FqandLl.Abort();
                    if (TH_ST != null) TH_ST.Abort();
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
                        if (rpm5300 != null)
                        {
                            rpm5300.closeEquipment();
                        }
                        if(vmt_2000!=null)
                        {
                            if (vmt_2000.ComPort_1.IsOpen)
                                vmt_2000.ComPort_1.Close();
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
            }
            catch
            { }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {

            DateTime nowtime = DateTime.Now;
            TimeSpan timespan = nowtime - jcStarttime;
            jcTime = (float)timespan.TotalMilliseconds / 1000f;
            JCSJ = (int)jcTime;
        }

        private DateTime gc_time = DateTime.Now;//用于标记过程数据全程时序，避免出现相同时间
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (SaveData_status)//如果正在测试
            {
                nowtime = DateTime.Now;
                //if (Convert.ToInt16(gongkuangTime) != GKSJ)//100ms一次
                if (DateTime.Compare(DateTime.Parse(nowtime.ToString("yyyy-MM-dd HH:mm:ss")), DateTime.Parse(gc_time.ToString("yyyy-MM-dd HH:mm:ss"))) > 0)
                {
                    gc_time = nowtime;
                    if (equipconfig.DATASECONDS_TYPE == "安车通用联网")//如果为安车通用联网，在此取值，以保证子程序过程数据 与安车前置数据一致
                    {

                        if (IsUseVRM)
                        {
                            if (VRMzt == 0)
                                ZS = 750 + DateTime.Now.Millisecond % 200;
                            else if (VRMzt == 1)
                            {
                                ZS = (int)(800 + (carbj.CarEdzs - 800) * 0.3 + DateTime.Now.Millisecond % 100);
                                VRMzt = 2;
                            }
                            else if (VRMzt == 2)
                            {
                                ZS = (int)(800 + (carbj.CarEdzs - 800) * 0.7 + DateTime.Now.Millisecond % 100);
                                VRMzt = 3;
                            }
                            else if (VRMzt == 3)
                            {
                                ZS = (int)(800 + (carbj.CarEdzs - 800) * 1.1 + DateTime.Now.Millisecond % 100);
                                VRMzt = 4;
                            }
                            else if (VRMzt == 4)
                            {
                                ZS = (int)(carbj.CarEdzs * 1.2 + DateTime.Now.Millisecond % 100);
                                VRMzt = 5;
                            }
                            else if (VRMzt == 5)
                            {
                                ZS = (int)(carbj.CarEdzs * 1.1 + DateTime.Now.Millisecond % 100);
                                VRMzt = 6;
                            }
                            else if (VRMzt == 6)
                            {
                                ZS = (int)(800 + (carbj.CarEdzs - 800) * 0.5 + DateTime.Now.Millisecond % 100);
                                VRMzt = 0;
                            }
                        }
                        else if (isUseRotater)
                        {
                            if (rpm5300 != null)
                            {
                                ZS = (int)rpm5300.ZS;
                            }
                            else if (vmt_2000 != null)
                            {
                                if (vmt_2000.readRotateSpeed())
                                    ZS = vmt_2000.zs;
                            }
                        }
                        Msg(label_zstext, panel_zstext, ZS.ToString(), false);
                        arcScaleComponent3.Value = ZS;
                        if (isReadRealTime)
                        {
                            if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                            {
                                smoke = fla_502.get_DirectData(0.01f);
                            }
                            else
                            {
                                if (equipconfig.Ydjxh == "mqy_200" || equipconfig.Ydjxh == "flb_100" || equipconfig.Ydjxh.ToLower() == "nht_1")
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
                                else
                                {
                                    smoke = flb_100.get_StableData();
                                }
                            }
                            if (!isUseRotater&&!IsUseVRM)
                                ZS = (int)(smoke.Zs);

                            if (carbj.ISUSE)
                            {
                                Smoke = (float)(carbj.ZYJS_K * smoke.K);
                            }
                            else
                            {
                                Smoke = smoke.K;
                            }
                            Msg(labelK, panelK, Smoke.ToString("0.00"), false);

                            if (nhsjz != null && btgconfig.Ywj == "南华附件")
                            {
                                if (nhsjz.readData())
                                    yw_now = nhsjz.yw;
                            }
                            else
                            {
                                yw_now = smoke.Yw;
                            }

                            if (GKSJ == 10230) GKSJ = 0;
                            Sxnblist[GKSJ] = sxnb.ToString("0");//时序类别
                            Qcsxlist[GKSJ] = nowtime.ToString("yyyy-MM-dd HH:mm:ss.fff");//全程时序
                            Zslist[GKSJ] = ZS;
                            Klist[GKSJ] = Smoke;
                            Nslist[GKSJ] = smoke.Ns;
                            Ywlist[GKSJ] = yw_now;
                            wdlist[GKSJ] = (float)wd;
                            sdlist[GKSJ] = (float)sd;
                            dqylist[GKSJ] = (float)dqy;

                            if (caculateStart)
                            {
                                if (Klist[GKSJ] > maxvalue)
                                    maxvalue = Klist[GKSJ];
                                if (Zslist[GKSJ] > maxzsvalue)
                                    maxzsvalue = (int)(Zslist[GKSJ]);
                                if (Ywlist[GKSJ] > maxywvalue)
                                    maxywvalue = Ywlist[GKSJ];
                            }
                            //else
                            //{
                            //    Klist[GKSJ] = 0;
                            //    Nslist[GKSJ] =0;
                            //}
                            //Ywlist[GKSJ] = 0;
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
                            Cysxlist[GKSJ] = GKSJ;
                            GKSJ++;//工况时间加1
                        }

                    }
                    else
                    {
                        if (GKSJ == 10230) GKSJ = 0;
                        Sxnblist[GKSJ] = sxnb.ToString("0");//时序类别
                        Qcsxlist[GKSJ] = nowtime.ToString("yyyy-MM-dd HH:mm:ss.fff");//全程时序
                        Zslist[GKSJ] = ZS;
                        Klist[GKSJ] = Smoke;
                        Nslist[GKSJ] = smoke.Ns;
                        Ywlist[GKSJ] = yw_now;
                        wdlist[GKSJ] = (float)wd;
                        sdlist[GKSJ] = (float)sd;
                        dqylist[GKSJ] = (float)dqy;

                        if (caculateStart)
                        {
                            if (Klist[GKSJ] > maxvalue)
                                maxvalue = Klist[GKSJ];
                            if (Zslist[GKSJ] > maxzsvalue)
                                maxzsvalue = (int)(Zslist[GKSJ]);
                            if (Ywlist[GKSJ] > maxywvalue)
                                maxywvalue = Ywlist[GKSJ];
                        }
                        //else
                        //{
                        //    Klist[GKSJ] = 0;
                        //    Nslist[GKSJ] =0;
                        //}
                        //Ywlist[GKSJ] = 0;
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
                        Cysxlist[GKSJ] = GKSJ;
                        GKSJ++;//工况时间加1
                    }
                }
                TimeSpan timespan = nowtime - startTime;
                if(equipconfig.DATASECONDS_TYPE=="安徽")//安徽的过程数据，自由加速0.1s存一组数
                    gongkuangTime = (float)timespan.TotalMilliseconds / 100f;
                else
                    gongkuangTime = (float)timespan.TotalMilliseconds / 1000f;
            }
            else
            {
                if (equipconfig.DATASECONDS_TYPE == "安车通用联网")//如果为安车通用联网，在此取值，以保证在吹拂过程中也在取数
                {
                    if (isReadRealTime)
                    {
                        nowtime = DateTime.Now;
                        //if (Convert.ToInt16(gongkuangTime) != GKSJ)//100ms一次
                        if (DateTime.Compare(DateTime.Parse(nowtime.ToString("yyyy-MM-dd HH:mm:ss")), DateTime.Parse(gc_time.ToString("yyyy-MM-dd HH:mm:ss"))) > 0)
                        {
                            gc_time = nowtime;
                            if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                            {
                                smoke = fla_502.get_DirectData(0.01f);
                            }
                            else
                            {
                                if (equipconfig.Ydjxh == "mqy_200" || equipconfig.Ydjxh == "flb_100" || equipconfig.Ydjxh.ToLower() == "nht_1")
                                {
                                    if (equipconfig.IsOldMqy200)
                                    {
                                        smoke = flb_100.get_DirectData(0.01f);
                                    }
                                    else
                                    {
                                        smoke = flb_100.get_Data(0.01f);
                                    }
                                }
                                else
                                {
                                    smoke = flb_100.get_StableData(0.01f);
                                }
                            }
                            if (!isUseRotater)
                                ZS = (int)(smoke.Zs);

                            if (carbj.ISUSE)
                            {
                                Smoke = (float)(carbj.ZYJS_K * smoke.K);
                            }
                            else
                            {
                                Smoke = smoke.K;
                            }
                            Msg(labelK, panelK, Smoke.ToString("0.00"), false);
                        }
                        if (isUseRotater)
                        {
                            if (rpm5300 != null)
                            {
                                ZS = (int)rpm5300.ZS;
                            }
                            else if (vmt_2000 != null)
                            {
                                if (vmt_2000.readRotateSpeed())
                                    ZS = vmt_2000.zs;
                            }
                        }
                        if (nhsjz != null && btgconfig.Ywj == "南华附件")
                        {
                            if (nhsjz.readData())
                                yw_now = nhsjz.yw;
                        }
                        else
                        {
                            yw_now = smoke.Yw;
                        }
                        Msg(label_zstext, panel_zstext, ZS.ToString(), false);
                        arcScaleComponent3.Value = ZS;
                    }
                }
                GKSJ = 0;
                perGKSJ = 0;
                gongkuangTime = 0f;
            }
        }
        float Smoke = 0;
        public void Fq_Detect()
        {
            if (equipconfig.DATASECONDS_TYPE != "安车通用联网")
            {
                while (true)
                {
                    if (isReadRealTime)
                    {

                        if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                        {
                            smoke = fla_502.get_DirectData();
                        }
                        else
                        {
                            if (equipconfig.Ydjxh == "mqy_200" || equipconfig.Ydjxh == "flb_100" || equipconfig.Ydjxh.ToLower() == "nht_1")
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
                            else
                            {
                                smoke = flb_100.get_StableData();
                            }
                        }
                        if (!isUseRotater&&!IsUseVRM)
                            ZS = (int)(smoke.Zs);

                        if (carbj.ISUSE)
                        {
                            Smoke = (float)(carbj.ZYJS_K * smoke.K);
                        }
                        else
                        {
                            Smoke = smoke.K;
                        }
                        Msg(labelK, panelK, Smoke.ToString("0.00"), false);
                    }
                    if (IsUseVRM)
                    {
                        if (VRMzt == 0)
                            ZS = 750 + DateTime.Now.Millisecond % 200;
                        else if (VRMzt == 1)
                        {
                            ZS = (int)(800 + (carbj.CarEdzs - 800) * 0.3 + DateTime.Now.Millisecond % 100);
                            VRMzt = 2;
                        }
                        else if (VRMzt == 2)
                        {
                            ZS = (int)(800 + (carbj.CarEdzs - 800) * 0.7 + DateTime.Now.Millisecond % 100);
                            VRMzt = 3;
                        }
                        else if (VRMzt == 3)
                        {
                            ZS = (int)(800 + (carbj.CarEdzs - 800) * 1.1 + DateTime.Now.Millisecond % 100);
                            VRMzt = 4;
                        }
                        else if (VRMzt == 4)
                        {
                            ZS = (int)(carbj.CarEdzs * 1.2 + DateTime.Now.Millisecond % 100);
                            VRMzt = 5;
                        }
                        else if (VRMzt == 5)
                        {
                            ZS = (int)(carbj.CarEdzs * 1.1 + DateTime.Now.Millisecond % 100);
                            VRMzt = 6;
                        }
                        else if (VRMzt == 6)
                        {
                            ZS = (int)(800 + (carbj.CarEdzs - 800) * 0.5 + DateTime.Now.Millisecond % 100);
                            VRMzt = 0;
                        }
                    }
                    else if (isUseRotater)
                    {
                        if (rpm5300 != null)
                        {
                            ZS = (int)rpm5300.ZS;
                        }
                        else if (vmt_2000 != null)
                        {
                            if (vmt_2000.readRotateSpeed())
                                ZS = vmt_2000.zs;
                        }
                    }
                    if (nhsjz != null && btgconfig.Ywj == "南华附件")
                    {
                        if (nhsjz.readData())
                            yw_now = nhsjz.yw;
                    }
                    else
                    {
                        yw_now = smoke.Yw;
                    }
                    Msg(label_zstext, panel_zstext, ZS.ToString(), false);
                    arcScaleComponent3.Value = ZS;
                    Thread.Sleep(700);
                }
            }
        }

        private void button_settings_Click(object sender, EventArgs e)
        {
            settings newSettings = new settings();
            newSettings.ShowDialog();
            initConfigInfo(); 
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
