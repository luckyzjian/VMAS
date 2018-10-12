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

namespace sds
{
    public partial class Sds : Form
    {
        carinfor.carInidata carbj = new carInidata();
        public static equipmentConfigInfdata equipconfig = new equipmentConfigInfdata();
        SdsConfigInfdata sdsconfig = new SdsConfigInfdata();
        public static VmasConfigInfdata vmasconfig = new VmasConfigInfdata();
        carIni carini = new carIni();
        configIni configini = new configIni();
        CSVcontrol.CSVwriter csvwriter = new CSVcontrol.CSVwriter();
        carinfor.sdsdata sds_data = new sdsdata();
        sdsdataControl sdsdatacontrol = new sdsdataControl();
        sdsDataSeconds sds_dataseconds = new sdsDataSeconds();
        sdsDataSecondsControl sds_datasecondscontrol = new sdsDataSecondsControl();
        statusconfigIni statusconfigini = new statusconfigIni();
        private bool monizs = false;
        private int monizt = 0;

        private string UseFqy = "";
        Exhaust.Fla501 fla_501 = null;
        Exhaust.Fla502 fla_502 = null;
        Exhaust.FLB_100 flb_100 = null;
        Exhaust.Flv_1000 flv_1000 = null;
        LedControl.BX5k1 ledcontrol = null;
        Dynamometer.IGBT igbt = null;
        Exhaust.VMT_2000 vmt_2000 = null;
        Exhaust.RPM5300 rpm5300 = null;
        Exhaust.XCE_100 xce_100 = null;
        Exhaust.Nhsjz nhsjz = null;
        thaxs thaxsdata = new thaxs();
        bool isUseRotater = false;

        private DateTime startTime, nowtime;

        public static int sxnb = 0;//时序类别，0：加速 1：功率扫描 2：100% 3：90% 4：80%
        private int cysx = 0;

        public float jcTime = 0f;
        private DateTime jcStarttime;
        public int JCSJ = 0; 

        public bool JC_Status = false;                                                          //检测状态

        private bool sdsIsFinished = false;
        private bool writeDataIsFinished = false;

        private float gongkuangTime = 0f;
        private int GKSJ = 0;
        private int effectiveGKSJ = 0;
        private int gdspregksj = 0;
        private int gdstestgksj = 0;
        private int dspregksj = 0;
        private int dstestgksj = 0;
        private int pregksj = 0;

        Thread TH_ST = null;                                                                    //检测线程
        public delegate void wtls(Label Msgowner, string Msgstr);                               //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                             //委托
        public delegate void wtlsb(Label Msgowner, string Msgstr, bool Update_DB);              //委托
        public delegate void wtcs(Control controlname, string text);                            //委托
        public delegate void wt_void();                                                         //委托
        public delegate void wtdtview(DataGridView datagridview, string title, int row_number, string message);
        public delegate void wl_led(LEDNumber.LEDNumber lednumber, string data);
        Thread Th_get_FqandLl = null;                                                           //废气检测线程
        Thread th_load = null;

        public int ZS_XZ = 3500;
        public float H_HC = 0;                                                                  //高怠速HC
        public float H_CO = 0;                                                                  //高怠速CO
        public float H_CO2;
        public float H_O2;
        public float H_ZS;
        public float L_CO2;
        public float L_O2;
        public float λ = 0;                                                                    //λ
        public float λ_value = 0;
        public float L_HC = 0;                                                                  //怠速HC
        public float L_CO = 0;                                                                  //怠速CO
        public float L_ZS = 0;
        public float H_HC_XZ = 0;                                                               //高怠速HC限值
        public float H_CO_XZ = 0;                                                               //高怠速CO限值
        public string λ_XZ = "1.00±0.03";                                                     //λ限值
        public float L_HC_XZ = 0;                                                               //怠速HC限值
        public float L_CO_XZ = 0;                                                               //怠速CO限值
        public string GDSHCPD = "不合格";                                                       //高怠速HC判定
        public string GDSCOPD = "不合格";                                                       //高怠速CO判定
        public string λPD = "不合格";                                                          //λ判定
        public string DSHCPD = "不合格";                                                        //怠速HC判定
        public string DSCOPD = "不合格";                                                        //怠速CO判定
        public string PD = "不合格";                                                            //判定结果
        public Exhaust.Fla502_data[] Exhaust_List_H = new Exhaust.Fla502_data[31];            //高怠速每秒废气结果
        public Exhaust.Fla502_data[] Exhaust_List_L = new Exhaust.Fla502_data[31];            //怠速每秒废气结果
        public Exhaust.Fla501_data[] Exhaust501_List_H = new Exhaust.Fla501_data[31];            //高怠速每秒废气结果
        public Exhaust.Fla501_data[] Exhaust501_List_L = new Exhaust.Fla501_data[31];            //怠速每秒废气结果
        DataTable Jccl = null;                                                                  //检测车辆信息
        public string Cllx = "";                                                                //车辆类型
        public string CLSYQK = "";                                                              //车辆使用情况
        public double WD = 0;                                                                  //温度
        public double SD = 0;                                                                  //相对湿度
        public double DQY = 0;                                                                //大气压

        public double[] temperatureEveryMonth = { -4, 1, 9, 17, 25, 29, 30, 28, 23, 16, 6, -2 };
        public static  float wd = 0f;
        public static  float sd = 0f;
        public static  float dqy = 0f;
        private float yw = 0f;
        public static float Zs = 0;
        private float yw_now = 0;
        public static  float hc_ld = 0;
        public static  float co_ld = 0;
        public static  float no_ld = 0;
        public static  float o2_ld = 0;
        public static  float co2_ld = 0;
        public static bool sds_status = false;
        public Exhaust.Fla502_data Vmas_Exhaust_Now = new Exhaust.Fla502_data();
        public Exhaust.Fla502_temp_data Vmas_Exhaust_tempNow = new Exhaust.Fla502_temp_data();
        public Exhaust.Fla501_data Vmas_Exhaust501_Now = new Exhaust.Fla501_data();
        private string[] Qcsxlist = new string[2048];
        private string[] Sxnblist = new string[2048];
        private int[] Cysxlist = new int[2048];
        private float[] Hclist = new float[2048];
        private float[] Nolist = new float[2048];
        private float[] Colist = new float[2048];
        private float[] O2list = new float[2048];
        private float[] Co2list = new float[2048];
        private float[] λlist = new float[2048];
        private float[] Zslist = new float[2048];
        private float[] Ywlist = new float[2048];
        private float[] Wdlist = new float[2048];
        private float[] Sdlist = new float[2048];
        private float[] Dqylist = new float[2048];
        #region 沈阳过程数据
        private string[] QcsxlistSY = new string[120];
        private string[] SxnblistSY = new string[120];
        private int[] CysxlistSY = new int[120];
        private float[] HclistSY = new float[120];
        private float[] NolistSY = new float[120];
        private float[] ColistSY = new float[120];
        private float[] O2listSY = new float[120];
        private float[] Co2listSY = new float[120];
        private float[] λlistSY = new float[120];
        private float[] ZslistSY = new float[120];
        private float[] YwlistSY = new float[120];
        private float[] WdlistSY = new float[120];
        private float[] SdlistSY = new float[120];
        private float[] DqylistSY = new float[120];
        #endregion
        public int isLowFlow = 0;
        public int[] zsrandom = { 0, 15, 85, 23, 65, 25, 54, 25, 14, 25, 56, 35, 15, 24, 85, 35, 95, 21, 23, 56, 32, 0, 14, 25, 52, 55, 55, 68, 52, 32, 12, 1, 25, 32, 62, 1, 52, 5, 26, 2, 2, 3, 6, 89, 5, 52, 0, 65, 38, 95, 25, 24, 17, 69, 25, 25, 3, 58, 65, 24, 21, 25, 35, 95, 65 };
        public static int gds = 2500;
        private bool chsy = false;
        public string jctime = "";
        DataTable dt = new DataTable();
        SYS.Model.jcxztCheck jcxzt = new SYS.Model.jcxztCheck();
        //public Exhaust.Fla502_data Vmas_Exhaust_ReviseNow = new Exhaust.Fla502_data();
        SYS_DAL.JCXXX jcxxx = new SYS_DAL.JCXXX();
        private bool isSongpin = true;
        private int jczt = 0;

        public static string ts1 = "川AV7M82";
        public static string ts2 = "双怠速法";

        public static string ts3 = "CO:-- HC:-- λ:-- CO2:-- O2:-- T:-- H:-- A:-- ";
        public static bool driverformmin = false;
        private bool isautostart = true;
        public static bool CanGetZs = true;
        public Sds()
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
        private void Sds_Load(object sender, EventArgs e)
        {
            initCarInfo();
            initConfigInfo();
            initDataResult();
            initEquipment();
            Init_Data();            //初始化数据
            Init_Limit();           //初始化限值
            Init_Show();            //初始化显示
            isSongpin = false;
            jcStarttime = DateTime.Now;
            timer3.Start();
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
                fla_502.coxs = carbj.SDS_CO;
                fla_502.hcxs = carbj.SDS_HC;
            }
            if (isautostart)
            {
                Thread.Sleep(3000);
                button_ss_Click(sender, e);
            }

            if (fla_502 != null)
            {
                fla_502.lockKeyboard();
                Thread.Sleep(100);
                fla_502.StopBlowback();
            }
            if(equipconfig.useJHSCREEN)
            {
                groupBox1.Visible = false;
                groupBox2.Visible = false;
            }
        }

        #region 初始化
        private void initCarInfo()
        {
            carbj = carini.getCarIni();
            ts1 = carbj.CarPH;
            if (carbj.ISMOTO)
            {
                ts2 = "双怠速法(摩)";
                this.Text= "双怠速法(摩)";
                panelMotoData.Visible = false;
            }
        }
        private void initConfigInfo()
        {
            equipconfig = configini.getEquipConfigIni();
            isautostart = equipconfig.WorkAutomaticMode;
            sdsconfig = configini.getSdsConfigIni();
            vmasconfig = configini.getVmasConfigIni();
            thaxsdata = configini.getthaxsConfigIni();
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
                        case "nha_506":
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
                                if (fla_502.Init_Comm(equipconfig.Fqyck,equipconfig.Fqyckpzz) == false)
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
                if (equipconfig.Ydjifpz == true&&equipconfig.Ydjxh!="CDF5000")
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
                if (equipconfig.Ledifpz)
                {
                    try
                    {
                        ledcontrol = new LedControl.BX5k1();
                        ledcontrol.row1 = (byte)equipconfig.ledrow1;
                        ledcontrol.row2 = (byte)equipconfig.ledrow2;
                        if (ledcontrol.Init_Comm(equipconfig.Ledck, equipconfig.LedComstring,(byte)equipconfig.LEDTJPH) == false)
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
                if (sdsconfig.Zsj.ToLower() == "vmt-2000" || sdsconfig.Zsj.ToLower() == "vut-3000")
                {
                    try
                    {
                        vmt_2000 = new Exhaust.VMT_2000();
                        isUseRotater = true;
                        if (vmt_2000.Init_Comm(sdsconfig.Zsjck, "19200,N,8,1") == false)
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
                else if (sdsconfig.Zsj.ToLower() == "mqz-2" || sdsconfig.Zsj.ToLower() == "mqz-3")
                {
                    MessageBox.Show("系统未提供该转速计功能，请重新配置", "系统提示");
                    isUseRotater = false;
                }
                else if (sdsconfig.Zsj.ToLower() == "rpm5300")
                {
                    try
                    {
                        rpm5300 = new Exhaust.RPM5300();
                        isUseRotater = true;
                        if (rpm5300.Init_Comm(sdsconfig.Zsjck, "9600,N,8,1") == false)
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
            dt.Columns.Add("项目");
            dt.Columns.Add("结果");
            DataRow dr = null;
            dr = dt.NewRow();
            dr["项目"] = "高怠速HC";
            dr["结果"] = "--";
            //dr["判定"] = "--";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "高怠速CO";
            dr["结果"] = "--";
            //dr["判定"] = "--";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "λ";
            dr["结果"] = "--";
            //dr["判定"] = "--";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "低怠速HC";
            dr["结果"] = "--";
            dt.Rows.Add(dr);
            //dr["判定"] = "--";
            dr = dt.NewRow();
            dr["项目"] = "低怠速CO";
            dr["结果"] = "--";
            dt.Rows.Add(dr);
            dataGridView1.DataSource = dt;
        }

        public void Init_Data()
        {
            try
            {
                Msg(label_cp, panel_cp, carbj.CarPH, false);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString(), "车辆数据初始化失败");
            }
        }

        public void Init_Limit()
        {
            try
            {
                if (carbj.ISMOTO)
                {
                        if (carbj.CarEdzs * 0.7 > 3500)
                            ZS_XZ = 3500;
                        else if (carbj.CarEdzs * 0.7 < 2500)
                            ZS_XZ = 2500;
                        else
                            ZS_XZ = (int)carbj.CarEdzs;
                    gds = 2500;
                }
                else if (carbj.CarZzl > 3500)
                {
                    ZS_XZ = 2520;
                    gds = 1800;
                }
                else
                {
                    ZS_XZ = 3500;
                    gds = 2500;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString(), "限值初始化失败");
            }
        }

        public void Init_Show()
        {
            try
            {
                // Ref_Control_Text(label_cp, Jccl.Rows[0]["JCCLPH"].ToString());
                Ref_Control_Text(label_st, "准备检测");
                Msg(label_msgcs, panel2, carbj.CarPH + " 请到位", true);
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed(carbj.CarPH,2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("请上检测线准备", 5, equipconfig.Ledxh);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString(), "车辆数据初始化失败");
            }
        }
        #endregion

        public delegate void wttextboxvalue(TextBox textbox, string value);
        public delegate void wtpanelvisible(Panel panel, bool visible_value);
        public void panel_visible(Panel panel, bool visible_value)
        {
            BeginInvoke(new wtpanelvisible(panel_show), panel, visible_value);
        }
        public void panel_show(Panel panel, bool visible_value)
        {
            panel.Visible = visible_value;
        }
        public void textbox_value(TextBox textbox, string value)
        {
            BeginInvoke(new wttextboxvalue(textboxValue), textbox, value);
        }
        public void textboxValue(TextBox textbox, string value)
        {
            textbox.Text = value;
        }
        private bool wsd_sure = false;
        private bool wsdValueIsRight = false;
        public void Jc_Exe()
        {

            int chaocha = 0;
            int zero_count = 0;
            DataRow dr = null;
            if (carbj.ISMOTO && sdsconfig.Zscc < 250)
                sdsconfig.Zscc = 250;
            int sdscc = sdsconfig.Zscc;
            monizt = 0;
            try
            {
                statusconfigini.writeGlStatusData(statusconfigIni.ENUM_GL_STATUS.STATUS_DAOWEI, "");
                if (sdsconfig.RotateSpeedMonitor== false)
                    sdscc = 4000;
                else
                    sdscc = sdsconfig.Zscc;
                /*
                switch (UseFqy)
                {
                    case "fla_502":
                        fla_502.Stop();
                        //CarWait.fla_502.Set_QigangShu(gangshu);
                        break;
                    case "fla_501":
                        fla_501.Stop();
                        break;
                    case "mqw_511":
                        fla_502.Stop();
                        break;
                }*/
                sdsIsFinished = false;
                //statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.ZIJIAN, JCSJ.ToString());
                Msg(Msg_msg, panel_msg, "测试即将开始,检查废气分析仪...", false);
                ts1 = "测试即将开始";
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("正在检查废气仪", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("......", 5, equipconfig.Ledxh);
                }
                Thread.Sleep(1000);
                switch (UseFqy)
                {
                    case "fla_502":
                        string fla_502_status = fla_502.Get_Struct();
                        if (fla_502_status != "仪器已经准备好")
                        {
                            Msg(Msg_msg, panel_msg, "废气仪" + fla_502_status + ",测试将结束", false);
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("废气仪不正常", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("测试结束", 5, equipconfig.Ledxh);
                            }
                            Ref_Control_Text(label_st, "检测停止");
                            ts1 = "废气仪" + fla_502_status;
                            BeginInvoke(new wt_void(Ref_Button));
                            JC_Status = false;
                            //BeginInvoke(new wt_void(Ref_Button));
                            sds_status = false;
                            Th_get_FqandLl.Abort();
                            return;
                        }
                        break;
                    case "fla_501":
                        break;
                    case "mqw_511":
                        break;
                }
                Thread.Sleep(1000);
                if (sdsconfig.IfFqyTl)
                {
                    switch (UseFqy)
                    {
                        case "fla_502":
                            fla_502.Zeroing();
                            //CarWait.fla_502.Set_QigangShu(gangshu);
                            break;
                        case "fla_501":
                            fla_501.SetZero();
                            break;
                        case "mqw_511":
                            break;
                    }
                    Thread.Sleep(500);
                    switch (UseFqy)
                    {
                        case "fla_502":
                            zero_count = 0;
                            if (equipconfig.Fqyxh.ToLower() == "nha_503")
                            {
                                while (fla_502.Zeroing() == false)//该处需要测试后定
                                {
                                    Thread.Sleep(900);
                                    Msg(Msg_msg, panel_msg, "废气仪调零中..." + zero_count.ToString() + "s", true);
                                    ts1 = "调零中..." + zero_count.ToString() + "s";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("废气仪调零中...", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed(zero_count.ToString() + "s", 5, equipconfig.Ledxh);
                                    }
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
                                    Msg(Msg_msg, panel_msg, "废气仪调零中..." + zero_count.ToString() + "s", true);
                                    ts1 = "调零中..." + zero_count.ToString() + "s";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("废气仪调零中...", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed(zero_count.ToString() + "s", 5, equipconfig.Ledxh);
                                    }
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
                                    Msg(Msg_msg, panel_msg, "废气仪调零中..." + zero_count.ToString() + "s", true);
                                    ts1 = "调零中..." + zero_count.ToString() + "s";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("废气仪调零中...", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed(zero_count.ToString() + "s", 5, equipconfig.Ledxh);
                                    }
                                    zero_count++;
                                    if (zero_count == 60)
                                        break;
                                }
                            }
                            break;
                        case "fla_501":
                            zero_count = 30;
                            while (zero_count > 0)
                            {
                                Thread.Sleep(900);
                                Msg(Msg_msg, panel_msg, "废气仪调零中..." + zero_count.ToString() + "s", true);
                                ts1 = "调零中..." + zero_count.ToString() + "s";
                                if (ledcontrol != null)
                                {
                                    ledcontrol.writeLed("废气仪调零中...", 2, equipconfig.Ledxh);
                                    Thread.Sleep(200);
                                    ledcontrol.writeLed(zero_count.ToString() + "s", 5, equipconfig.Ledxh);
                                }
                                zero_count--;
                            }
                            break;
                        case "mqw_511":
                            break;
                    }
                    if (sdsconfig.sdsNoReZero) sdsconfig.IfFqyTl = false;
                }
                ts1 = "读取环境参数...";
                Msg(Msg_msg, panel_msg, "读取环境参数...", false);             
                try//获取环境参数
                {
                    Thread.Sleep(1000);
                    if (IsUseTpTemp)
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
                    else if (equipconfig.TempInstrument == "烟度计"&&flb_100!=null)
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
                    }
                    else if (equipconfig.TempInstrument == "XCE_101")
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
                    }
                    else if (equipconfig.TempInstrument == "RZ_1")
                    {
                        if (xce_100.readEnvironment())
                        {
                            WD = xce_100.temp;
                            SD = xce_100.humidity;
                            DQY = xce_100.airpressure;
                        }
                    }
                    else if (equipconfig.TempInstrument == "模拟")
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
                    ts1 = WD.ToString("0.0") + "℃ " + SD.ToString("0.0") + "% " + DQY.ToString("0.0") + "kPa";
                    //ts2 = "低于限值,检测中止";
                    Msg(Msg_msg, panel_msg, WD.ToString("0.0") + "℃ " + SD.ToString("0.0") + "% " + DQY.ToString("0.0") + "kPa", false);
                    textbox_value(textBoxSDSD, SD.ToString("0.0"));
                    textbox_value(textBoxSDWD, WD.ToString("0.0"));
                    textbox_value(textBoxSDQY, DQY.ToString("0.0"));
                    wsd_sure = false;
                    if (sdsconfig.IsSureTemp)
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
                            ts1 = "请先校正废气仪温湿度";
                            Msg(Msg_msg, panel_msg, "请先校正废气仪温湿度", false);
                            BeginInvoke(new wt_void(Ref_Button));
                            Ref_Control_Text(label_st, "检测停止");
                            JC_Status = false;
                            //BeginInvoke(new wt_void(Ref_Button));
                            sds_status = false;
                            Th_get_FqandLl.Abort();
                            return;
                        }
                        WD = double.Parse(textBoxSDWD.Text);
                        SD = double.Parse(textBoxSDSD.Text);
                        DQY = double.Parse(textBoxSDQY.Text);
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception)
                {
                }
                if (sdsconfig.IfFqyTl == true)
                {
                    Thread.Sleep(500);
                    fla_502.Pump_Pipeair();
                    for (int i = 15; i >= 0; i--)
                    {
                        Msg(Msg_msg, panel_msg, "HC残留测定..." + i.ToString("0"), true);
                        ts1 = "HC测定..." + i.ToString("0");
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("HC残留测定...", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed(i.ToString() + "s", 5, equipconfig.Ledxh);
                        }
                        Thread.Sleep(900);
                    }
                    Thread.Sleep(500);
                    Exhaust.Fla502_data Environment = fla_502.GetData();
                    if (Environment.HC < 20)
                    {
                        Msg(Msg_msg, panel_msg, "HC残留量：" + Environment.HC.ToString("0")+" 达标", true);
                        ts1 = "HC残留量：" + Environment.HC.ToString("0");
                        ts2 = "达标";
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("HC残留量：" + Environment.HC.ToString("0"), 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("达标", 5, equipconfig.Ledxh);
                        }
                    }
                    else
                    {
                        Msg(Msg_msg, panel_msg, "HC残留量：" + Environment.HC.ToString("0") + " 未达标", true);
                        ts1 = "HC残留量：" + Environment.HC.ToString("0");
                        ts2 = "未达标，检测中止";
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("HC残留量：" + Environment.HC.ToString("0"), 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("未达标，检测中止", 5, equipconfig.Ledxh);
                        }
                        BeginInvoke(new wt_void(Ref_Button));
                        Ref_Control_Text(label_st, "检测停止");
                        JC_Status = false;
                        //BeginInvoke(new wt_void(Ref_Button));
                        sds_status = false;
                        Th_get_FqandLl.Abort();
                        return;
                    }
                    fla_502.StopBlowback();
                    Thread.Sleep(1000);
                    ts2 = "";
                }
                if (sdsconfig.IsTestYw)
                {
                    ts1 = "读取油温...";
                    Msg(Msg_msg, panel_msg, "读取油温...", false);
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("读取油温...", 2, equipconfig.Ledxh);
                    }
                    float ywnow = 0;
                    if (nhsjz != null && sdsconfig.Ywj == "南华附件")
                    {
                        if (nhsjz.readData())
                            ywnow = nhsjz.yw;
                        else if (nhsjz.readData())
                            ywnow = nhsjz.yw;
                    }
                    else
                    {
                        Exhaust.Fla502_data Environment = fla_502.GetData();
                        ywnow = Environment.YW;
                    }
                    Thread.Sleep(1000);
                    if (ywnow < 80)
                    {
                        ts1 = "油温: " + ywnow.ToString("0.0") + " ℃";
                        ts2 = "低于限值,检测中止";
                        Msg(Msg_msg, panel_msg, "油温:" + ywnow.ToString("0.0") + "℃" + "低于限值,检测中止", false);
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("油温低于限值",2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("检测中止", 5, equipconfig.Ledxh);
                        }
                        BeginInvoke(new wt_void(Ref_Button));
                        Ref_Control_Text(label_st, "检测停止");
                        JC_Status = false;
                        //BeginInvoke(new wt_void(Ref_Button));
                        sds_status = false;
                        Th_get_FqandLl.Abort();
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
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.ZIJIANJIESHU, JCSJ.ToString());
                Thread.Sleep(1000);
                switch (UseFqy)
                {
                    case "fla_502":
                        fla_502.Pump_Pipeair();
                        //CarWait.fla_502.Set_QigangShu(gangshu);
                        break;
                    case "mqw_511":
                        fla_502.Pump_Pipeair();
                        //CarWait.fla_502.Set_QigangShu(gangshu);
                        break;
                    case "fla_501":
                        fla_501.Start();
                        break;
                }
                Thread.Sleep(1000);
                
                sds_status = true;//开启废气仪
                Msg(Msg_msg, panel_msg, "检测即将开始，请启动车辆，安置好转速计", false);
                ts1 = "检测开始";
                ts2 = "请安置转速计";
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("启动车辆保持怠速", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("请安置好转速计", 5, equipconfig.Ledxh);
                }
                Thread.Sleep(3000);
                Msg(label_msgcs, panel2, "检测开始", false);
                
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("检测开始", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("加速至" + ZS_XZ + "转以上", 5, equipconfig.Ledxh);
                }
                Ref_Control_Text(label_st, "检测开始");
                
                Msg(Msg_msg, panel_msg, carbj.CarPH + "加速至" + ZS_XZ + "转以上", true);
                ts2 = "加速至" + ZS_XZ + "转以上";
                Thread.Sleep(2000);
                if (comboBoxZSJK.SelectedIndex > 0) monizs = true;
                else monizs = false;
                monizt = 1;
                while (Zs <= ZS_XZ&&sdsconfig.RotateSpeedMonitor)
                {
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("加速至" + ZS_XZ + "转以上",2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("  转速：" + (int)Zs + "转", 5, equipconfig.Ledxh);
                    }
                    Thread.Sleep(100);
                }
                //statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIASU, JCSJ.ToString());
                chaocha = 0;
                GKSJ = 0;
                effectiveGKSJ = 0;
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.DAOWEI, GKSJ.ToString());
                Thread.Sleep(2000);
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.CHATANTOU, GKSJ.ToString());
                startTime = DateTime.Now;
                JC_Status = true;//开始存储逐秒数据
                statusconfigini.writeNeuStatusData("StartTest", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sds_data.StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                statusconfigini.writeGlStatusData(statusconfigIni.ENUM_GL_STATUS.STATUS_STARTSAMPLE, "");
                sxnb = 0;//70%额定转速
                jczt = 0;
                cysx = 1;//采样时序设为1
                JC_Status = true;//开始存储逐秒数据
                while(effectiveGKSJ<1)
                {
                    Thread.Sleep(400);
                }
                int nowgksj = 0;
                int preseconds = carbj.ISMOTO ? 10 : 30;//准备时间
                if (carbj.ISMOTO)
                {
                    if (sdsconfig.TimerMode3500 == 0)
                    {
                        nowgksj = effectiveGKSJ - 1;
                        pregksj = nowgksj;//将目前时间记为开始时间
                        int timercount = 0;
                        while (timercount < preseconds)        //保持3500转30秒
                        {
                            nowgksj = effectiveGKSJ - 1;
                            if (nowgksj > pregksj)
                            {
                                pregksj = nowgksj;
                                if (Zslist[nowgksj] < ZS_XZ && sdsconfig.RotateSpeedMonitor)
                                {
                                    pregksj = nowgksj;
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + "转速超差", true);
                                    ts2 = "转速超差";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("　　转速超差　　", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed(" " + (int)Zs + "转：" + (10 - timercount).ToString() + "秒", 5, equipconfig.Ledxh);
                                    }

                                }
                                else
                                {
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + " 保持：" + (int)ZS_XZ + "转以上" + (10 - timercount).ToString() + " 秒", true);
                                    ts2 = " 保持" + (int)ZS_XZ + "转以上" + (10 - timercount).ToString() + " 秒";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("保持:" + (int)ZS_XZ + "转以上", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed(" " + (int)Zs + "转：" + (10 - timercount).ToString() + "秒", 5, equipconfig.Ledxh);
                                    }
                                    QcsxlistSY[timercount] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    SxnblistSY[timercount] = "0";
                                    CysxlistSY[timercount] = timercount + 1;
                                    HclistSY[timercount] = Hclist[nowgksj];
                                    NolistSY[timercount] = Nolist[nowgksj];
                                    ColistSY[timercount] = Colist[nowgksj];
                                    O2listSY[timercount] = O2list[nowgksj];
                                    Co2listSY[timercount] = Co2list[nowgksj];
                                    λlistSY[timercount] = λlist[nowgksj];
                                    ZslistSY[timercount] = Zslist[nowgksj];
                                    YwlistSY[timercount] = Ywlist[nowgksj];
                                    WdlistSY[timercount] = Wdlist[nowgksj];
                                    SdlistSY[timercount] = Sdlist[nowgksj];
                                    DqylistSY[timercount] = Dqylist[nowgksj];
                                    timercount++;
                                }
                            }
                            Thread.Sleep(400);
                        }
                    }
                    else if (sdsconfig.TimerMode3500 == 2)
                    {
                        nowgksj = effectiveGKSJ - 1;
                        pregksj = nowgksj;//将目前时间记为开始时间
                        int timercount = 0;
                        while (timercount < preseconds)        //保持3500转30秒
                        {
                            nowgksj = effectiveGKSJ - 1;
                            if (nowgksj > pregksj)
                            {
                                pregksj = nowgksj;
                                Msg(Msg_msg, panel_msg, carbj.CarPH + " 保持：" + (int)ZS_XZ + "转以上" + (10 - timercount).ToString() + " 秒", true);
                                ts2 = " 保持" + (int)ZS_XZ + "转以上" + (10 - timercount).ToString() + " 秒";
                                if (ledcontrol != null)
                                {
                                    ledcontrol.writeLed("保持:" + (int)ZS_XZ + "转以上", 2, equipconfig.Ledxh);
                                    Thread.Sleep(200);
                                    ledcontrol.writeLed(" " + (int)Zs + "转：" + (10 - timercount).ToString() + "秒", 5, equipconfig.Ledxh);
                                }
                                QcsxlistSY[timercount] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                SxnblistSY[timercount] = "0";
                                CysxlistSY[timercount] = timercount + 1;
                                HclistSY[timercount] = Hclist[nowgksj];
                                NolistSY[timercount] = Nolist[nowgksj];
                                ColistSY[timercount] = Colist[nowgksj];
                                O2listSY[timercount] = O2list[nowgksj];
                                Co2listSY[timercount] = Co2list[nowgksj];
                                λlistSY[timercount] = λlist[nowgksj];
                                ZslistSY[timercount] = Zslist[nowgksj];
                                YwlistSY[timercount] = Ywlist[nowgksj];
                                WdlistSY[timercount] = Wdlist[nowgksj];
                                SdlistSY[timercount] = Sdlist[nowgksj];
                                DqylistSY[timercount] = Dqylist[nowgksj];
                                timercount++;
                            }
                            Thread.Sleep(400);
                        }
                    }
                    else
                    {
                        nowgksj = effectiveGKSJ - 1;
                        pregksj = nowgksj;//将目前时间记为开始时间

                        int timercount = 0;
                        while (timercount < preseconds)        //保持3500转30秒
                        {
                            nowgksj = effectiveGKSJ - 1;
                            if (nowgksj > pregksj)
                            {
                                pregksj = nowgksj;
                                if (Zslist[nowgksj] < ZS_XZ && sdsconfig.RotateSpeedMonitor)
                                {
                                    timercount = 0;
                                    pregksj = nowgksj;
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + "转速超差，将重新计时", true);
                                    ts2 = "转速超差，将重新计时";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("转速超差重新计时", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed(" " + (int)Zs + "转：" + (10 - timercount).ToString() + "秒", 5, equipconfig.Ledxh);
                                    }
                                }
                                else
                                {
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + " 保持：" + (int)ZS_XZ + "转以上" + (10 - timercount).ToString() + " 秒", true);
                                    ts2 = " 保持" + (int)ZS_XZ + "转以上" + (10 - timercount).ToString() + " 秒";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("保持:" + (int)ZS_XZ + "转以上", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed(" " + (int)Zs + "转：" + (10 - timercount).ToString() + "秒", 5, equipconfig.Ledxh);
                                    }
                                    QcsxlistSY[timercount] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    SxnblistSY[timercount] = "0";
                                    CysxlistSY[timercount] = timercount + 1;
                                    HclistSY[timercount] = Hclist[nowgksj];
                                    NolistSY[timercount] = Nolist[nowgksj];
                                    ColistSY[timercount] = Colist[nowgksj];
                                    O2listSY[timercount] = O2list[nowgksj];
                                    Co2listSY[timercount] = Co2list[nowgksj];
                                    λlistSY[timercount] = λlist[nowgksj];
                                    ZslistSY[timercount] = Zslist[nowgksj];
                                    YwlistSY[timercount] = Ywlist[nowgksj];
                                    WdlistSY[timercount] = Wdlist[nowgksj];
                                    SdlistSY[timercount] = Sdlist[nowgksj];
                                    DqylistSY[timercount] = Dqylist[nowgksj];
                                    timercount++;
                                }
                            }
                            Thread.Sleep(400);
                            //nowgksj = effectiveGKSJ - 1;
                        }
                    }
                }
                else
                {
                    if (sdsconfig.TimerMode3500 == 0)
                    {
                        nowgksj = effectiveGKSJ - 1;
                        pregksj = nowgksj;//将目前时间记为开始时间
                        int timercount = 0;
                        while (timercount < preseconds)        //保持3500转30秒
                        {
                            nowgksj = effectiveGKSJ - 1;
                            if (nowgksj > pregksj)
                            {
                                pregksj = nowgksj;

                                if (Zslist[nowgksj] < ZS_XZ && sdsconfig.RotateSpeedMonitor)
                                {
                                    pregksj = nowgksj;
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + "转速超差", true);
                                    ts2 = "转速超差";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("　　转速超差　　", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed(" " + (int)Zs + "转：" + (30 - timercount).ToString() + "秒", 5, equipconfig.Ledxh);
                                    }

                                }
                                else
                                {
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + " 保持：" + (int)ZS_XZ + "转以上" + (30 - timercount).ToString() + " 秒", true);
                                    ts2 = " 保持" + (int)ZS_XZ + "转以上" + (30 - timercount).ToString() + " 秒";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("保持:" + (int)ZS_XZ + "转以上", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed(" " + (int)Zs + "转：" + (30 - timercount).ToString() + "秒", 5, equipconfig.Ledxh);
                                    }
                                    QcsxlistSY[timercount] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    SxnblistSY[timercount] = "0";
                                    CysxlistSY[timercount] = timercount + 1;
                                    HclistSY[timercount] = Hclist[nowgksj];
                                    NolistSY[timercount] = Nolist[nowgksj];
                                    ColistSY[timercount] = Colist[nowgksj];
                                    O2listSY[timercount] = O2list[nowgksj];
                                    Co2listSY[timercount] = Co2list[nowgksj];
                                    λlistSY[timercount] = λlist[nowgksj];
                                    ZslistSY[timercount] = Zslist[nowgksj];
                                    YwlistSY[timercount] = Ywlist[nowgksj];
                                    WdlistSY[timercount] = Wdlist[nowgksj];
                                    SdlistSY[timercount] = Sdlist[nowgksj];
                                    DqylistSY[timercount] = Dqylist[nowgksj];
                                    timercount++;
                                }
                            }
                            Thread.Sleep(400);
                        }
                    }
                    else if (sdsconfig.TimerMode3500 == 2)
                    {
                        nowgksj = effectiveGKSJ - 1;
                        pregksj = nowgksj;//将目前时间记为开始时间
                        int timercount = 0;
                        while (timercount < preseconds)        //保持3500转30秒
                        {
                            nowgksj = effectiveGKSJ - 1;
                            if (nowgksj > pregksj)
                            {
                                pregksj = nowgksj;
                                Msg(Msg_msg, panel_msg, carbj.CarPH + " 保持：" + (int)ZS_XZ + "转以上" + (30 - timercount).ToString() + " 秒", true);
                                ts2 = " 保持" + (int)ZS_XZ + "转以上" + (30 - timercount).ToString() + " 秒";
                                if (ledcontrol != null)
                                {
                                    ledcontrol.writeLed("保持:" + (int)ZS_XZ + "转以上", 2, equipconfig.Ledxh);
                                    Thread.Sleep(200);
                                    ledcontrol.writeLed(" " + (int)Zs + "转：" + (30 - timercount).ToString() + "秒", 5, equipconfig.Ledxh);
                                }
                                QcsxlistSY[timercount] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                SxnblistSY[timercount] = "0";
                                CysxlistSY[timercount] = timercount + 1;
                                HclistSY[timercount] = Hclist[nowgksj];
                                NolistSY[timercount] = Nolist[nowgksj];
                                ColistSY[timercount] = Colist[nowgksj];
                                O2listSY[timercount] = O2list[nowgksj];
                                Co2listSY[timercount] = Co2list[nowgksj];
                                λlistSY[timercount] = λlist[nowgksj];
                                ZslistSY[timercount] = Zslist[nowgksj];
                                YwlistSY[timercount] = Ywlist[nowgksj];
                                WdlistSY[timercount] = Wdlist[nowgksj];
                                SdlistSY[timercount] = Sdlist[nowgksj];
                                DqylistSY[timercount] = Dqylist[nowgksj];
                                timercount++;
                            }
                            Thread.Sleep(400);
                        }
                    }
                    else
                    {
                        nowgksj = effectiveGKSJ - 1;
                        pregksj = nowgksj;//将目前时间记为开始时间
                        int timercount = 0;
                        while (timercount < preseconds)        //保持3500转30秒
                        {
                            nowgksj = effectiveGKSJ - 1;
                            if (nowgksj > pregksj)
                            {
                                pregksj = nowgksj;
                                if (Zslist[nowgksj] < ZS_XZ && sdsconfig.RotateSpeedMonitor)
                                {
                                    timercount = 0;
                                    pregksj = nowgksj;
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + "转速超差，将重新计时", true);
                                    ts2 = "转速超差，将重新计时";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("转速超差重新计时", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed(" " + (int)Zs + "转：" + (30 - timercount).ToString() + "秒", 5, equipconfig.Ledxh);
                                    }
                                }
                                else
                                {
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + " 保持：" + (int)ZS_XZ + "转以上" + (30 - timercount).ToString() + " 秒", true);
                                    ts2 = " 保持" + (int)ZS_XZ + "转以上" + (30 - timercount).ToString() + " 秒";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("保持:" + (int)ZS_XZ + "转以上", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed(" " + (int)Zs + "转：" + (30 - timercount).ToString() + "秒", 5, equipconfig.Ledxh);
                                    }
                                    QcsxlistSY[timercount] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    SxnblistSY[timercount] = "0";
                                    CysxlistSY[timercount] = timercount + 1;
                                    HclistSY[timercount] = Hclist[nowgksj];
                                    NolistSY[timercount] = Nolist[nowgksj];
                                    ColistSY[timercount] = Colist[nowgksj];
                                    O2listSY[timercount] = O2list[nowgksj];
                                    Co2listSY[timercount] = Co2list[nowgksj];
                                    λlistSY[timercount] = λlist[nowgksj];
                                    ZslistSY[timercount] = Zslist[nowgksj];
                                    YwlistSY[timercount] = Ywlist[nowgksj];
                                    WdlistSY[timercount] = Wdlist[nowgksj];
                                    SdlistSY[timercount] = Sdlist[nowgksj];
                                    DqylistSY[timercount] = Dqylist[nowgksj];
                                    timercount++;
                                }
                            }
                            Thread.Sleep(400);
                            //nowgksj = effectiveGKSJ - 1;
                        }
                    }
                }
                Msg(Msg_msg, panel_msg, "请立即插入探头，深度不少于400mm", false);
                ts2 = "请插入探头";
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("请立即插入探头", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("深度大于400mm", 5);
                }
                Thread.Sleep(2000);
                chaocha = 0;
                if (sdsconfig.ConcentrationMonitor)
                {
                    switch (UseFqy)
                    {
                        case "fla_502":
                            while ((Vmas_Exhaust_Now.CO + Vmas_Exhaust_Now.CO2 < sdsconfig.Ndz)||Vmas_Exhaust_Now.O2>=10) ;
                            //CarWait.fla_502.Set_QigangShu(gangshu);
                            break;
                        case "mqw_511":
                            while ((Vmas_Exhaust_Now.CO + Vmas_Exhaust_Now.CO2 < sdsconfig.Ndz )|| Vmas_Exhaust_Now.O2 >= 10) ;
                            //CarWait.fla_502.Set_QigangShu(gangshu);
                            break;
                        case "fla_501":
                            while ((Vmas_Exhaust501_Now.CO + Vmas_Exhaust501_Now.CO2 < sdsconfig.Ndz) || Vmas_Exhaust501_Now.O2 >= 10) ;
                            break;
                    }
                }
                ts2 = "探头已插好";
                Msg(Msg_msg, panel_msg, "探头已经插好，即将开始高怠速测试", false);
                statusconfigini.writeGlStatusData(statusconfigIni.ENUM_GL_STATUS.STATUS_TANTOU, "");

                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("探头已经插好", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("将开始高怠速测试", 5, equipconfig.Ledxh);
                }
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIASU, GKSJ.ToString());
                statusconfigini.writeNeuStatusData("TestingHigh", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                Thread.Sleep(2000);
                jczt = 1;
                monizt = 2;
                //monizs = false;
                ts1 = "高怠速准备";
                ts2 = "调整至" + gds.ToString() + "r/min";
                Msg(label_msgcs, panel2, "测试高怠速 " + gds + "转", false);
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("高怠速测试",2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("请减速至" + gds + "转", 5, equipconfig.Ledxh);
                }
                Msg(Msg_msg, panel_msg, carbj.CarPH + "请减速至" + gds + "转", true);
                Thread.Sleep(1000);
                while (Math.Abs(Zs - gds) > sdscc)
                {
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("减速速至" + gds + "转", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("  转速：" + (int)Zs + "转", 5, equipconfig.Ledxh);
                    }
                    Thread.Sleep(100);
                }
                sxnb = 1;//高怠速准备
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIANCEZHONG, GKSJ.ToString());
                //gdspregksj = GKSJ;
                nowgksj = effectiveGKSJ - 1;
                pregksj = nowgksj;
                gdspregksj = nowgksj;//将目前时间记为开始时间
                int passTime = 0;
                while (passTime < 15)        //插探头
                {
                    if (sdsconfig.ConcentrationMonitor)
                    {
                        if (isLowFlow == -1)
                        {
                            Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到废气仪通讯故障，请重新检测", true);
                            ts2 = "废气仪通讯故障";
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("废气仪通讯故障", 5, equipconfig.Ledxh);
                            }
                            Ref_Control_Text(label_st, "检测停止");
                            BeginInvoke(new wt_void(Ref_Button));
                            JC_Status = false;
                            sds_status = false;
                            Th_get_FqandLl.Abort();
                            return;
                        }
                        else if (isLowFlow == -2)
                        {
                            Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到废气仪气路阻塞，请重新检测", true);
                            ts2 = "废气仪低流量";
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("废气仪气路阻塞", 5, equipconfig.Ledxh);
                            }
                            Ref_Control_Text(label_st, "检测停止");
                            BeginInvoke(new wt_void(Ref_Button));
                            JC_Status = false;
                            sds_status = false;
                            Th_get_FqandLl.Abort();
                            return;
                        }
                    }
                    nowgksj = effectiveGKSJ - 1;
                    if (nowgksj > pregksj)
                    {
                        pregksj = nowgksj;
                        if (Math.Abs(Zslist[nowgksj] - gds) > sdscc)
                        {
                            if (sdsconfig.TimerModeHP == 0)
                            {
                                //gdspregksj = nowgksj - passTime;
                                Msg(Msg_msg, panel_msg, carbj.CarPH + "转速超差", true);
                                ts2 = "转速超差";
                                if (ledcontrol != null)
                                {
                                    ledcontrol.writeLed("转速超差　　　　", 2, equipconfig.Ledxh);
                                    Thread.Sleep(200);
                                    ledcontrol.writeLed(" " + (int)Zs + "转：" + (15 - passTime).ToString() + "秒", 5, equipconfig.Ledxh);
                                }
                            }
                            else
                            {
                                passTime = 0;
                                gdspregksj = nowgksj;
                                Msg(Msg_msg, panel_msg, carbj.CarPH + "转速超差，将重新计时", true);
                                ts2 = "转速超差，将重新计时";
                                if (ledcontrol != null)
                                {
                                    ledcontrol.writeLed("转速超差重新计时", 2, equipconfig.Ledxh);
                                    Thread.Sleep(200);
                                    ledcontrol.writeLed(" " + (int)Zs + "转：" + (15 - passTime).ToString() + "秒", 5, equipconfig.Ledxh);
                                }
                            }
                        }
                        else
                        {
                            Msg(Msg_msg, panel_msg, carbj.CarPH + "保持" + gds + "转 " + (15 - passTime).ToString() + " 秒", true);
                            ts2 = "保持" + gds + "转 " + (15 - passTime).ToString() + " 秒";
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("保持转速:" + gds + "转", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed(" " + (int)Zs + "转：" + (15 - passTime).ToString() + "秒", 5, equipconfig.Ledxh);
                            }
                            QcsxlistSY[passTime + preseconds] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            SxnblistSY[passTime + preseconds] = "1";
                            CysxlistSY[passTime + preseconds] = passTime + 1;
                            HclistSY[passTime + preseconds] = Hclist[nowgksj];
                            NolistSY[passTime + preseconds] = Nolist[nowgksj];
                            ColistSY[passTime + preseconds] = Colist[nowgksj];
                            O2listSY[passTime + preseconds] = O2list[nowgksj];
                            Co2listSY[passTime + preseconds] = Co2list[nowgksj];
                            λlistSY[passTime + preseconds] = λlist[nowgksj];
                            ZslistSY[passTime + preseconds] = Zslist[nowgksj];
                            YwlistSY[passTime + preseconds] = Ywlist[nowgksj];
                            WdlistSY[passTime + preseconds] = Wdlist[nowgksj];
                            SdlistSY[passTime + preseconds] = Sdlist[nowgksj];
                            DqylistSY[passTime + preseconds] = Dqylist[nowgksj];
                            passTime++;
                        }
                    }
                    Thread.Sleep(400);
                    //passTime = nowgksj - gdspregksj;
                    //nowgksj = effectiveGKSJ - 1;
                }
                sxnb = 1;//高怠速测试
                ts1 = "高怠速测试";
                jczt = 2;
                monizt = 2;
                //monizs = false;
                nowgksj = effectiveGKSJ - 1;
                pregksj = nowgksj;
                gdstestgksj = nowgksj;
                passTime = 0;
                // Msg(Msg_msg, panel_msg, CarWait.bjcl.JCCLPH.ToString() + "" + (15 - i).ToString() + " 秒", true);
                while (passTime < 30)        //取值30秒
                {
                    if (sdsconfig.ConcentrationMonitor)
                    {
                        if (isLowFlow == -1)
                        {
                            Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到废气仪通讯故障，请重新检测", true);
                            ts2 = "废气仪通讯故障";
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("废气仪通讯故障", 5, equipconfig.Ledxh);
                            }
                            Ref_Control_Text(label_st, "检测停止");
                            BeginInvoke(new wt_void(Ref_Button));
                            JC_Status = false;
                            sds_status = false;
                            Th_get_FqandLl.Abort();
                            return;
                        }
                        else if (isLowFlow == -2)
                        {
                            Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到废气仪气路阻塞，请重新检测", true);
                            ts2 = "废气仪低流量";
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("废气仪气路阻塞", 5, equipconfig.Ledxh);
                            }
                            Ref_Control_Text(label_st, "检测停止");
                            BeginInvoke(new wt_void(Ref_Button));
                            JC_Status = false;
                            sds_status = false;
                            Th_get_FqandLl.Abort();
                            return;
                        }

                        switch (UseFqy)
                        {
                            case "fla_502":
                                if (Vmas_Exhaust_Now.CO + Vmas_Exhaust_Now.CO2 < sdsconfig.Ndz)
                                {
                                    ts2 = "浓度值过低";
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到探头脱落或气路阻塞，请重新检测", true);
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed("监测到尾气异常", 5, equipconfig.Ledxh);
                                    }
                                    Ref_Control_Text(label_st, "检测停止");
                                    BeginInvoke(new wt_void(Ref_Button));
                                    JC_Status = false;
                                    sds_status = false;
                                    Th_get_FqandLl.Abort();
                                    return;
                                }
                                if (Vmas_Exhaust_Now.λ == 0 && sdsconfig.sdsHighLambdaMonitor)
                                {
                                    ts2 = "λ值异常";
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到过量空气系数为0，请重新检测", true);
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed("过量空气系数为0", 5, equipconfig.Ledxh);
                                    }
                                    Ref_Control_Text(label_st, "检测停止");
                                    BeginInvoke(new wt_void(Ref_Button));
                                    JC_Status = false;
                                    sds_status = false;
                                    Th_get_FqandLl.Abort();
                                    return;
                                }
                                //CarWait.fla_502.Set_QigangShu(gangshu);
                                break;
                            case "fla_501":
                                if (Vmas_Exhaust501_Now.CO + Vmas_Exhaust501_Now.CO2 < sdsconfig.Ndz)
                                {
                                    ts2 = "浓度值过低";
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到到探头脱落或气路阻塞，请重新检测", true);
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed("监测到尾气异常", 5, equipconfig.Ledxh);
                                    }
                                    Ref_Control_Text(label_st, "检测停止");
                                    BeginInvoke(new wt_void(Ref_Button));
                                    JC_Status = false;
                                    sds_status = false;
                                    Th_get_FqandLl.Abort();
                                    return;
                                }
                                if (Vmas_Exhaust501_Now.λ == 0 && sdsconfig.sdsHighLambdaMonitor)
                                {
                                    ts2 = "λ值异常";
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到过量空气系数为0，请重新检测", true);
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed("过量空气系数为0", 5, equipconfig.Ledxh);
                                    }
                                    Ref_Control_Text(label_st, "检测停止");
                                    BeginInvoke(new wt_void(Ref_Button));
                                    JC_Status = false;
                                    sds_status = false;
                                    Th_get_FqandLl.Abort();
                                    return;
                                }
                                break;
                            case "mqw_511":
                                if (Vmas_Exhaust_Now.CO + Vmas_Exhaust_Now.CO2 < sdsconfig.Ndz)
                                {
                                    ts2 = "浓度值过低";
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到到探头脱落或气路阻塞，请重新检测", true);
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed("监测到尾气异常", 5, equipconfig.Ledxh);
                                    }
                                    Ref_Control_Text(label_st, "检测停止");
                                    BeginInvoke(new wt_void(Ref_Button));
                                    JC_Status = false;
                                    sds_status = false;
                                    Th_get_FqandLl.Abort();
                                    return;
                                }
                                if (Vmas_Exhaust_Now.λ == 0 && sdsconfig.sdsHighLambdaMonitor)
                                {
                                    ts2 = "λ值异常";
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到过量空气系数为0，请重新检测", true);
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed("过量空气系数为0", 5, equipconfig.Ledxh);
                                    }
                                    Ref_Control_Text(label_st, "检测停止");
                                    BeginInvoke(new wt_void(Ref_Button));
                                    JC_Status = false;
                                    sds_status = false;
                                    Th_get_FqandLl.Abort();
                                    return;
                                }
                                break;
                        }
                    }
                    nowgksj = effectiveGKSJ - 1;
                    if (nowgksj > pregksj)
                    {
                        pregksj = nowgksj;
                        if (Math.Abs(Zslist[nowgksj] - gds) > sdscc)
                        {
                            if (sdsconfig.TimerModeHT == 0)
                            {
                                //gdstestgksj = nowgksj - passTime;
                                Msg(Msg_msg, panel_msg, carbj.CarPH + "转速超差", true);
                                ts2 = "转速超差";
                                if (ledcontrol != null)
                                {
                                    ledcontrol.writeLed("转速超差　　　　", 2, equipconfig.Ledxh);
                                    Thread.Sleep(200);
                                    ledcontrol.writeLed(" " + (int)Zs + "转：" + (15 - passTime).ToString() + "秒", 5, equipconfig.Ledxh);
                                }
                            }
                            else
                            {
                                passTime = 0;
                                gdstestgksj = nowgksj;
                                Msg(Msg_msg, panel_msg, carbj.CarPH + "转速超差，将重新计时", true);
                                ts2 = "转速超差，将重新计时";
                                if (ledcontrol != null)
                                {
                                    ledcontrol.writeLed("转速超差重新计时", 2, equipconfig.Ledxh);
                                    Thread.Sleep(200);
                                    ledcontrol.writeLed(" " + (int)Zs + "转：" + (30 - passTime).ToString() + "秒", 5, equipconfig.Ledxh);
                                }
                            }
                        }
                        else
                        {
                            Msg(Msg_msg, panel_msg, "请保持：" + (30 - passTime).ToString() + " 秒   " + (vmasconfig.IfDisplayData ? ("HC:" + hc_ld.ToString() + " CO:" + co_ld.ToString("0.000") + (carbj.ISMOTO?"":(" λ:" + λ_value.ToString("0.000")))) : ""), true);
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("保持" + gds + "转：" + (int)Zs, 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("测试中:" + (30 - passTime).ToString() + "秒", 5, equipconfig.Ledxh);
                            }
                            ts2 = "保持" + gds + "转 " + (30 - passTime).ToString() + " 秒";
                            QcsxlistSY[passTime + preseconds + 15] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            SxnblistSY[passTime + preseconds + 15] = "2";
                            Sxnblist[nowgksj] = "2";//将该时刻时序变量置为2
                            CysxlistSY[passTime + preseconds + 15] = passTime + 1;
                            HclistSY[passTime + preseconds + 15] = Hclist[nowgksj];
                            NolistSY[passTime + preseconds + 15] = Nolist[nowgksj];
                            ColistSY[passTime + preseconds + 15] = Colist[nowgksj];
                            O2listSY[passTime + preseconds + 15] = O2list[nowgksj];
                            Co2listSY[passTime + preseconds + 15] = Co2list[nowgksj];
                            λlistSY[passTime + preseconds + 15] = λlist[nowgksj];
                            ZslistSY[passTime + preseconds + 15] = Zslist[nowgksj];
                            YwlistSY[passTime + preseconds + 15] = Ywlist[nowgksj];
                            WdlistSY[passTime + preseconds + 15] = Wdlist[nowgksj];
                            SdlistSY[passTime + preseconds + 15] = Sdlist[nowgksj];
                            DqylistSY[passTime + preseconds + 15] = Dqylist[nowgksj];
                            passTime++;
                        }
                    }
                    Thread.Sleep(400);
                    //passTime = nowgksj - gdstestgksj;
                    //nowgksj = effectiveGKSJ - 1;
                }
                float highHCsum = 0, highCOsum = 0, highLambadasum = 0, highCO2sum = 0, highO2sum = 0, highZSsum = 0;
                //Exhaust_List_H[30] = new Exhaust.Fla502_data();
                //Exhaust_List_L[30] = new Exhaust.Fla502_data();
                for (int i = preseconds + 15; i < preseconds + 45; i++)
                {
                    highHCsum += HclistSY[i];
                    highCOsum += ColistSY[i];
                    highLambadasum += λlistSY[i];
                    highCO2sum += Co2listSY[i];
                    highO2sum += O2listSY[i];
                    highZSsum += ZslistSY[i];//gb_add
                }
                H_HC = highHCsum / 30;
                H_CO = highCOsum / 30.0f;
                H_CO2 = highCO2sum / 30.0f;
                H_O2 = highO2sum / 30.0f;
                λ = highLambadasum / 30.0f;
                H_ZS = highZSsum / 30.0f;//gb_add
                jczt = 3;
                sxnb = 3;//怠速准备
                //monizs = false;
                monizt = 3;
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("HC:" + H_HC.ToString("0"), 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("CO:" + H_CO.ToString("0.0"), 5, equipconfig.Ledxh);
                    Thread.Sleep(2000);
                    ledcontrol.writeLed("过量空气系数:", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed(λ.ToString("0.00"), 5, equipconfig.Ledxh);
                    Thread.Sleep(2000);
                }
                Msg(Msg_msg, panel_msg, carbj.CarPH + "请降至怠速状态", true);
                ts1 = "低怠速准备";
                ts2 = "车辆保持怠速状态";
                Thread.Sleep(1000);
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIASU, GKSJ.ToString());
                Msg(label_msgcs, panel2, "测试怠速状态", false);
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("怠速测试", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("请降至怠速状态", 5, equipconfig.Ledxh);
                }
                if (sdsconfig.RotateSpeedMonitor)
                {
                    while (Zs > 1500 || Zs < 200)
                    {
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("请降至怠速状态", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("  转速：" + (int)Zs + "转", 5, equipconfig.Ledxh);
                        }
                        Thread.Sleep(100);
                    }
                }
                statusconfigini.writeNeuStatusData("TestingLow", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                nowgksj = effectiveGKSJ - 1;
                pregksj = nowgksj;
                dspregksj = nowgksj;
                passTime = 0;
                while (passTime < 15)        //插探头
                {
                    if (sdsconfig.ConcentrationMonitor)
                    {
                        if (isLowFlow == -1)
                        {
                            ts2 = "废气仪通讯故障";
                            Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到废气仪通讯故障，请重新检测", true);
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("废气仪通讯故障", 5, equipconfig.Ledxh);
                            }
                            Ref_Control_Text(label_st, "检测停止");
                            BeginInvoke(new wt_void(Ref_Button));
                            JC_Status = false;
                            sds_status = false;
                            Th_get_FqandLl.Abort();
                            return;
                        }
                        else if (isLowFlow == -2)
                        {
                            Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到废气仪气路阻塞，请重新检测", true);
                            ts2 = "废气仪低流量";
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("废气仪气路阻塞", 5, equipconfig.Ledxh);
                            }
                            Ref_Control_Text(label_st, "检测停止");
                            BeginInvoke(new wt_void(Ref_Button));
                            JC_Status = false;
                            sds_status = false;
                            Th_get_FqandLl.Abort();
                            return;
                        }
                    }

                    nowgksj = effectiveGKSJ - 1;
                    if (nowgksj > pregksj)
                    {
                        pregksj = nowgksj;
                        Msg(Msg_msg, panel_msg, "请保持怠速状态 " + (15 - passTime).ToString() + " 秒", true);
                        ts2 = "保持怠速状态 " + (15 - passTime).ToString() + " 秒";
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("保持怠速状态", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed((15 - passTime).ToString() + "秒", 5, equipconfig.Ledxh);
                        }
                        QcsxlistSY[passTime + preseconds + 45] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        SxnblistSY[passTime + preseconds + 45] = "3";
                        CysxlistSY[passTime + preseconds + 45] = passTime + 1;
                        HclistSY[passTime + preseconds + 45] = Hclist[nowgksj];
                        NolistSY[passTime + preseconds + 45] = Nolist[nowgksj];
                        ColistSY[passTime + preseconds + 45] = Colist[nowgksj];
                        O2listSY[passTime + preseconds + 45] = O2list[nowgksj];
                        Co2listSY[passTime + preseconds + 45] = Co2list[nowgksj];
                        λlistSY[passTime + preseconds + 45] = λlist[nowgksj];
                        ZslistSY[passTime + preseconds + 45] = Zslist[nowgksj];
                        YwlistSY[passTime + preseconds + 45] = Ywlist[nowgksj];
                        WdlistSY[passTime + preseconds + 45] = Wdlist[nowgksj];
                        SdlistSY[passTime + preseconds + 45] = Sdlist[nowgksj];
                        DqylistSY[passTime + preseconds + 45] = Dqylist[nowgksj];
                        passTime++;
                    }
                    Thread.Sleep(400);
                    //nowgksj = effectiveGKSJ - 1;
                }
                jczt = 4;
                sxnb = 3;//怠速测试
                ts1 = "低怠速测量";
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIANCEZHONG, GKSJ.ToString());
                nowgksj = effectiveGKSJ - 1;
                pregksj = nowgksj;
                dstestgksj = nowgksj;
                passTime = 0;
                while (passTime < 30)        //取值30秒
                {
                    if (sdsconfig.ConcentrationMonitor)
                    {
                        if (isLowFlow == -1)
                        {
                            Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到废气仪通讯故障，请重新检测", true);
                            ts2 = "废气仪通讯故障";
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("废气仪通讯故障", 5, equipconfig.Ledxh);
                            }
                            Ref_Control_Text(label_st, "检测停止");
                            BeginInvoke(new wt_void(Ref_Button));
                            JC_Status = false;
                            sds_status = false;
                            Th_get_FqandLl.Abort();
                            return;
                        }
                        else if (isLowFlow == -2)
                        {
                            Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到废气仪气路阻塞，请重新检测", true);
                            ts2 = "废气仪低流量";
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("废气仪气路阻塞", 5, equipconfig.Ledxh);
                            }
                            Ref_Control_Text(label_st, "检测停止");
                            BeginInvoke(new wt_void(Ref_Button));
                            JC_Status = false;
                            sds_status = false;
                            Th_get_FqandLl.Abort();
                            return;
                        }
                        switch (UseFqy)
                        {
                            case "fla_502":
                                if (Vmas_Exhaust_Now.CO + Vmas_Exhaust_Now.CO2 < sdsconfig.Ndz)
                                {
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到探头脱落或气路阻塞，请重新检测", true);
                                    ts2 = "浓度值过低";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed("监测到尾气异常", 5, equipconfig.Ledxh);
                                    }
                                    Ref_Control_Text(label_st, "检测停止");
                                    BeginInvoke(new wt_void(Ref_Button));
                                    JC_Status = false;
                                    sds_status = false;
                                    Th_get_FqandLl.Abort();
                                    return;
                                }
                                if (Vmas_Exhaust_Now.λ == 0&&sdsconfig.sdsLowLambdaMonitor)
                                {
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到过量空气系数为0，请重新检测", true);
                                    ts2 = "λ值异常";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed("过量空气系数为0", 5, equipconfig.Ledxh);
                                    }
                                    Ref_Control_Text(label_st, "检测停止");
                                    BeginInvoke(new wt_void(Ref_Button));
                                    JC_Status = false;
                                    sds_status = false;
                                    Th_get_FqandLl.Abort();
                                    return;
                                }
                                //CarWait.fla_502.Set_QigangShu(gangshu);
                                break;
                            case "fla_501":
                                if (Vmas_Exhaust501_Now.CO + Vmas_Exhaust501_Now.CO2 < sdsconfig.Ndz)
                                {
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到到探头脱落或气路阻塞，请重新检测", true);
                                    ts2 = "浓度值过低";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed("监测到尾气异常", 5, equipconfig.Ledxh);
                                    }
                                    Ref_Control_Text(label_st, "检测停止");
                                    BeginInvoke(new wt_void(Ref_Button));
                                    JC_Status = false;
                                    sds_status = false;
                                    Th_get_FqandLl.Abort();
                                    return;
                                }
                                if (Vmas_Exhaust501_Now.λ == 0 && sdsconfig.sdsLowLambdaMonitor)
                                {
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到过量空气系数为0，请重新检测", true);
                                    ts2 = "λ值异常";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed("过量空气系数为0", 5, equipconfig.Ledxh);
                                    }
                                    Ref_Control_Text(label_st, "检测停止");
                                    BeginInvoke(new wt_void(Ref_Button));
                                    JC_Status = false;
                                    sds_status = false;
                                    Th_get_FqandLl.Abort();
                                    return;
                                }
                                break;
                                
                            case "mqw_511":
                                if (Vmas_Exhaust_Now.CO + Vmas_Exhaust_Now.CO2 < sdsconfig.Ndz)
                                {
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到到探头脱落或气路阻塞，请重新检测", true);
                                    ts2 = "浓度值过低";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed("监测到尾气异常", 5, equipconfig.Ledxh);
                                    }
                                    Ref_Control_Text(label_st, "检测停止");
                                    BeginInvoke(new wt_void(Ref_Button));
                                    JC_Status = false;
                                    sds_status = false;
                                    Th_get_FqandLl.Abort();
                                    return;
                                }
                                if (Vmas_Exhaust_Now.λ == 0 && sdsconfig.sdsLowLambdaMonitor)
                                {
                                    Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到过量空气系数为0，请重新检测", true);
                                    ts2 = "λ值异常";
                                    if (ledcontrol != null)
                                    {
                                        ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                        Thread.Sleep(200);
                                        ledcontrol.writeLed("过量空气系数为0", 5, equipconfig.Ledxh);
                                    }
                                    Ref_Control_Text(label_st, "检测停止");
                                    BeginInvoke(new wt_void(Ref_Button));
                                    JC_Status = false;
                                    sds_status = false;
                                    Th_get_FqandLl.Abort();
                                    return;
                                }
                                break;
                        }
                    }
                    nowgksj = effectiveGKSJ - 1;
                    if (nowgksj > pregksj)
                    {
                        pregksj = nowgksj;
                        Msg(Msg_msg, panel_msg, (30 - passTime).ToString() + " 秒   " + (vmasconfig.IfDisplayData ? ("HC:" + hc_ld.ToString() + " CO:" + co_ld.ToString() + (carbj.ISMOTO ? "" : (" λ:" + λ_value.ToString("0.000")))) : ""), true);
                        ts2 = "保持怠速状态 " + (30 - passTime).ToString() + " 秒";
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("请保持怠速状态", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("测试中:" + (30 - passTime).ToString() + "秒", 5, equipconfig.Ledxh);
                        }
                        QcsxlistSY[passTime + preseconds + 60] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        SxnblistSY[passTime + preseconds + 60] = "4";
                        Sxnblist[nowgksj] = "4";//将该时刻时序变量置为4
                        CysxlistSY[passTime + preseconds + 60] = passTime + 1;
                        HclistSY[passTime + preseconds + 60] = Hclist[nowgksj];
                        NolistSY[passTime + preseconds + 60] = Nolist[nowgksj];
                        ColistSY[passTime + preseconds + 60] = Colist[nowgksj];
                        O2listSY[passTime + preseconds + 60] = O2list[nowgksj];
                        Co2listSY[passTime + preseconds + 60] = Co2list[nowgksj];
                        λlistSY[passTime + preseconds + 60] = λlist[nowgksj];
                        ZslistSY[passTime + preseconds + 60] = Zslist[nowgksj];
                        YwlistSY[passTime + preseconds + 60] = Ywlist[nowgksj];
                        WdlistSY[passTime + preseconds + 60] = Wdlist[nowgksj];
                        SdlistSY[passTime + preseconds + 60] = Sdlist[nowgksj];
                        DqylistSY[passTime + preseconds + 60] = Dqylist[nowgksj];
                        passTime++;
                    }
                    Thread.Sleep(400);
                    //nowgksj = effectiveGKSJ - 1;
                }
                //关泵
                sds_dataseconds.Gksj = GKSJ;//保存工况时间
                statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.GUOCHE, GKSJ.ToString());
                statusconfigini.writeGlStatusData(statusconfigIni.ENUM_GL_STATUS.STATUS_ENDSAMPLE, "");
                JC_Status = false;//停止工况时间
                Msg(Msg_msg, panel_msg, "数据计算中……", false);
                ts1 = "测量完毕";
                ts2 = "数据计算中...";
                float lowCOsum = 0, lowHCsum = 0, lowCO2sum = 0, lowO2sum = 0, lowZSsum = 0;
                for (int i = preseconds + 60; i < preseconds + 90; i++)
                {
                    lowHCsum += HclistSY[i];
                    lowCOsum += ColistSY[i];
                    lowCO2sum += Co2listSY[i];
                    lowO2sum += O2listSY[i];
                    lowZSsum += ZslistSY[i];//gb_add
                }
                L_HC = lowHCsum / 30;
                L_CO = lowCOsum / 30.0f;
                L_CO2 = lowCO2sum / 30.0f;
                L_O2 = lowO2sum / 30.0f;
                L_ZS = lowZSsum / 30.0f;//gb_add
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("HC:" + L_HC.ToString("0"), 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("CO:" + L_CO.ToString("0.0"), 5, equipconfig.Ledxh);
                    Thread.Sleep(2000);
                }
                timer_show.Stop();
                timer2.Stop();
                //statusconfigini.writeStatusData(statusconfigIni.EQUIPMENTSTATUS.JIANCEZHONG, JCSJ.ToString());
                Msg(Msg_msg, panel_msg, "测试完毕,请拨出探头.", false);
                Msg(label_msgcs, panel2, "测试完毕", false);
                ts2 = "请拨出探头";
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("测试完毕", 2,equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("请拔出探头", 5, equipconfig.Ledxh);
                }
                sds_dataseconds.CarID = carbj.CarID;
                DataTable sds_datatable = new DataTable();
                sds_datatable.Columns.Add("全程时序");
                sds_datatable.Columns.Add("时序类别");
                sds_datatable.Columns.Add("采样时序");
                sds_datatable.Columns.Add("HC");
                sds_datatable.Columns.Add("NO");
                sds_datatable.Columns.Add("CO");
                sds_datatable.Columns.Add("O2");
                sds_datatable.Columns.Add("CO2");
                sds_datatable.Columns.Add("过量空气系数");
                sds_datatable.Columns.Add("转速");
                sds_datatable.Columns.Add("油温");
                sds_datatable.Columns.Add("环境温度");
                sds_datatable.Columns.Add("相对湿度");
                sds_datatable.Columns.Add("大气压力");
                if (equipconfig.DATASECONDS_TYPE == "安徽")
                {
                    for (int i = preseconds; i < preseconds + 90; i++)//安微过程数据只要90s内的数据
                    {
                        dr = sds_datatable.NewRow();
                        dr["全程时序"] = QcsxlistSY[i];
                        dr["时序类别"] = SxnblistSY[i];
                        dr["采样时序"] = CysxlistSY[i];
                        dr["HC"] = HclistSY[i];
                        dr["NO"] = NolistSY[i];
                        dr["CO"] = ColistSY[i];
                        dr["O2"] = O2listSY[i];
                        dr["CO2"] = Co2listSY[i];
                        dr["过量空气系数"] = λlistSY[i];
                        dr["转速"] = ZslistSY[i];
                        dr["油温"] = YwlistSY[i];
                        dr["环境温度"] = WdlistSY[i];
                        dr["相对湿度"] = SdlistSY[i];
                        dr["大气压力"] = DqylistSY[i];
                        sds_datatable.Rows.Add(dr);
                    }
                }
                else if (equipconfig.DATASECONDS_TYPE == "安车通用联网")
                {
                    for (int i = 0; i < preseconds + 90; i++)//安微过程数据只要90s内的数据
                    {
                        dr = sds_datatable.NewRow();
                        dr["全程时序"] = QcsxlistSY[i];
                        dr["时序类别"] = SxnblistSY[i];
                        dr["采样时序"] = CysxlistSY[i];
                        dr["HC"] = HclistSY[i];
                        dr["NO"] = NolistSY[i];
                        dr["CO"] = ColistSY[i];
                        dr["O2"] = O2listSY[i];
                        dr["CO2"] = Co2listSY[i];
                        dr["过量空气系数"] = λlistSY[i];
                        dr["转速"] = ZslistSY[i];
                        dr["油温"] = YwlistSY[i];
                        dr["环境温度"] = WdlistSY[i];
                        dr["相对湿度"] = SdlistSY[i];
                        dr["大气压力"] = DqylistSY[i];
                        sds_datatable.Rows.Add(dr);
                    }
                }
                else
                {
                    for (int i = 0; i < sds_dataseconds.Gksj; i++)
                    {
                        dr = sds_datatable.NewRow();
                        dr["全程时序"] = Qcsxlist[i];
                        dr["时序类别"] = Sxnblist[i];
                        dr["采样时序"] = Cysxlist[i];
                        dr["HC"] = Hclist[i];
                        dr["NO"] = Nolist[i];
                        dr["CO"] = Colist[i];
                        dr["O2"] = O2list[i];
                        dr["CO2"] = Co2list[i];
                        dr["过量空气系数"] = λlist[i];
                        dr["转速"] = Zslist[i];
                        dr["油温"] = Ywlist[i];
                        dr["环境温度"] = Wdlist[i];
                        dr["相对湿度"] = Sdlist[i];
                        dr["大气压力"] = Dqylist[i];
                        sds_datatable.Rows.Add(dr);
                    }
                }
                sds_data.CarID = carbj.CarID;
                sds_data.Sd = SD.ToString("0.0");
                sds_data.Wd = WD.ToString("0.0");
                sds_data.Dqy = DQY.ToString("0.0");
                sds_data.λ_value = λ.ToString("0.00");
                sds_data.Hc_high = H_HC.ToString("0");
                sds_data.Co_high = H_CO.ToString("0.00");
                sds_data.Hc_low = L_HC.ToString("0");
                sds_data.Co_low = L_CO.ToString("0.00");
                sds_data.Co2_high = H_CO2.ToString("0.00");
                sds_data.O2_high = H_O2.ToString("0.00");
                sds_data.Co2_low = L_CO2.ToString("0.00");
                sds_data.O2_low = L_O2.ToString("0.00");
                sds_data.StopReason = "0";
                datagridview_msg(dataGridView1, "结果", 0, sds_data.Hc_high);
                datagridview_msg(dataGridView1, "结果", 1, sds_data.Co_high);
                datagridview_msg(dataGridView1, "结果", 2, sds_data.λ_value);
                datagridview_msg(dataGridView1, "结果", 3, sds_data.Hc_low);
                datagridview_msg(dataGridView1, "结果", 4, sds_data.Co_low);

                writeDataIsFinished = false;
                //th_load = new Thread(load_progress);
                //th_load.Start();
                csvwriter.SaveCSV(sds_datatable, "C:/jcdatatxt/" + carbj.CarID + ".csv");
                sdsdatacontrol.writeSdsData(sds_data);//写carID.ini文件
                writeDataIsFinished = true;
                ts2 = "请驶离检测线";
                Msg(label_msgcs, panel2, "测试完毕 ", true);
                Thread.Sleep(2000);
                driverformmin = true;

                if (sdsconfig.Flowback)
                {
                    switch (UseFqy)
                    {
                        case "fla_502":
                            fla_502.Stop();//停止废气分析析工作
                            fla_502.Blowback();
                            break;
                        case "mqw_511":
                            break;
                        case "fla_501":
                            //CarWait.fla_501.Stop();
                            //CarWait.fla_501.Clear();
                            break;
                    }
                    for (int i = sdsconfig.FlowTime; i >= 0; i--)
                    {
                        Thread.Sleep(1000);
                        Msg(Msg_msg, panel_msg, "清洗管路... " + i.ToString() + "秒", false);
                        ts2 = "清洗管路... " + i.ToString() + "秒";
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("请驶离检测线", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("清洗管路中：" + i.ToString() + "秒", 5, equipconfig.Ledxh);
                        }
                    }
                    switch (UseFqy)
                    {
                        case "fla_502":
                            fla_502.StopBlowback();//停止废气分析析工作
                            fla_502.StopBlowback();//停止废气分析析工作
                            break;
                        case "mqw_511":
                            fla_502.StopBlowback();//停止废气分析析工作
                            fla_502.StopBlowback();//停止废气分析析工作
                            break;
                        case "fla_501":
                            //CarWait.fla_501.Stop();
                            break;
                    }
                    Thread.Sleep(500);
                }
                JC_Status = false;
                BeginInvoke(new wt_void(Ref_Button));
                sds_status = false;
                //Th_get_FqandLl.Abort();
                sdsIsFinished = true;
                Thread.Sleep(1000);
                this.Close();
            }
            catch (Exception er)
            {
                ////MessageBox.Show(er.ToString(), "出错啦");
                //JC_Status = false;
                //sds_status = false;
                ////MessageBox.Show("在测试过程中程序异常退出.", "系统提示");
                //sds_data.CarID = carbj.CarID;
                //sds_data.Sd = "-1";
                //sds_data.Wd = "-1";
                //sds_data.Dqy = "-1";
                //sds_data.λ_value = "-1";
                //sds_data.Hc_high = "-1";
                //sds_data.Co_high = "-1";
                //sds_data.Hc_low = "-1";
                //sds_data.Co_low = "-1";
                //sdsdatacontrol.writeSdsData(sds_data);//写carID.ini文件
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
        public void Ref_Button()
        {
            button_ss.Text = "重新检测";
            JC_Status = false;
        }
        int iSeed = 10;
        Random ro = new Random(10);
        public static float λ_value_temp = 0;
        Random ran = new Random((int)(DateTime.Now.Ticks & 0xffffffffL) | (int)(DateTime.Now.Ticks >> 32));
        private void getRealData()
        {
            
            switch (UseFqy)
            {
                case "fla_502":
                    Vmas_Exhaust_Now = fla_502.GetData();
                    if (!monizs)
                    {
                        if (isUseRotater)
                        {
                            if (rpm5300 != null)
                            {
                                Zs = (float)rpm5300.ZS;
                            }
                            else if (vmt_2000 != null)
                            {
                                if (vmt_2000.readRotateSpeed())
                                    Zs = vmt_2000.zs;
                            }
                        }
                        else
                        {
                            Zs = Vmas_Exhaust_Now.ZS;
                        }
                    }
                    else
                    {


                        if (monizt == 1)
                        {
                            Zs = ZS_XZ + ran.Next(50, 150);
                        }
                        else if (monizt == 2)
                        {
                            Zs = gds + ran.Next(-99, 99);
                        }
                        else if (monizt == 3)
                        {
                            Zs = 780 + ran.Next(-99, 99);
                        }
                    }
                    //Vmas_Exhaust_tempNow = fla_502.Get_Temp();

                    co_ld = Vmas_Exhaust_Now.CO;
                    hc_ld = Vmas_Exhaust_Now.HC;
                    o2_ld = Vmas_Exhaust_Now.O2;
                    co2_ld = Vmas_Exhaust_Now.CO2;
                    no_ld = Vmas_Exhaust_Now.NO;
                    λ_value_temp = Vmas_Exhaust_Now.λ;
                    if (carbj.ISUSE)
                    {
                        if (λ_value_temp > 0.97 && λ_value_temp < 1.03)
                        { }
                        else if (λ_value_temp < 1.6f && λ_value_temp > 0.7f)
                            λ_value_temp = 1.0f + (DateTime.Now.Millisecond % 6 - 3) * 0.01f;
                    }
                    λ_value = λ_value_temp;
                    if (IsUseTpTemp)
                    {
                        wd = (float)WD;
                        sd = (float)SD;
                        dqy = (float)DQY;
                    }
                    else if (equipconfig.TempInstrument == "废气仪")
                    {
                        if (equipconfig.Fqyxh.ToLower() == "nha_503" || equipconfig.Fqyxh.ToLower() == "fla_502" || equipconfig.Fqyxh.ToLower() == "cdf5000")
                        {
                            Exhaust.Fla502_temp_data fla502_temp_data = fla_502.Get_Temp();
                            wd = fla502_temp_data.TEMP;
                            sd = fla502_temp_data.HUMIDITY;
                            dqy = fla502_temp_data.AIRPRESSURE;
                        }
                        else
                        {
                            wd = Vmas_Exhaust_Now.HJWD;
                            sd = Vmas_Exhaust_Now.SD;
                            dqy = Vmas_Exhaust_Now.HJYL;
                        }
                    }
                    else
                    {
                        wd = (float)WD;
                        sd = (float)SD;
                        dqy = (float)DQY;
                    }
                    if (nhsjz != null && sdsconfig.Ywj == "南华附件")
                    {
                        if (nhsjz.readData())
                            yw = nhsjz.yw;
                    }
                    else
                    {
                        yw = Vmas_Exhaust_Now.YW;
                    }
                    Thread.Sleep(50);
                    isLowFlow = fla_502.CheckIsLowFlow();
                    Thread.Sleep(50);
                    break;
                case "mqw_511":
                    Vmas_Exhaust_Now = fla_502.GetData();
                    if (!monizs)
                    {
                        if (isUseRotater)
                        {
                            if (rpm5300 != null)
                            {
                                Zs = (float)rpm5300.ZS;
                            }
                            else if (vmt_2000 != null)
                            {
                                if (vmt_2000.readRotateSpeed())
                                    Zs = vmt_2000.zs;
                            }
                        }
                        else
                        {
                            Zs = Vmas_Exhaust_Now.ZS;
                        }
                    }
                    else
                    {
                        if (monizt == 1)
                        {
                            Zs = ZS_XZ + ran.Next(50, 150);
                        }
                        else if (monizt == 2)
                        {
                            Zs = gds + ran.Next(-99, 99);
                        }
                        else if (monizt == 3)
                        {
                            Zs = 780 + ran.Next(-99, 99);
                        }
                    }
                    //Vmas_Exhaust_tempNow = fla_502.Get_Temp();

                    co_ld = Vmas_Exhaust_Now.CO;
                    hc_ld = Vmas_Exhaust_Now.HC;
                    o2_ld = Vmas_Exhaust_Now.O2;
                    co2_ld = Vmas_Exhaust_Now.CO2;
                    no_ld = Vmas_Exhaust_Now.NO;
                    λ_value_temp = Vmas_Exhaust_Now.λ;
                    if (carbj.ISUSE)
                    {
                        if (λ_value_temp > 0.97 && λ_value_temp < 1.03)
                        { }
                        else if (λ_value_temp < 1.6f && λ_value_temp > 0.7f)
                            λ_value_temp = 1.0f + (DateTime.Now.Millisecond % 6 - 3) * 0.01f;
                    }
                    λ_value = λ_value_temp;
                    if (IsUseTpTemp)
                    {
                        wd = (float)WD;
                        sd = (float)SD;
                        dqy = (float)DQY;
                    }
                    else
                    {
                        wd = (float)WD;
                        sd = (float)SD;
                        dqy = (float)DQY;
                    }
                    if (nhsjz != null && sdsconfig.Ywj == "南华附件")
                    {
                        if (nhsjz.readData())
                            yw = nhsjz.yw;
                    }
                    else
                    {
                        yw = Vmas_Exhaust_Now.YW;
                    }
                    break;
                case "fla_501":
                    Vmas_Exhaust501_Now = fla_501.Get_Data();
                    if (!monizs)
                    {
                        if (isUseRotater)
                        {
                            if (rpm5300 != null)
                            {
                                Zs = (float)rpm5300.ZS;
                            }
                            else if (vmt_2000 != null)
                            {
                                if (vmt_2000.readRotateSpeed())
                                    Zs = vmt_2000.zs;
                            }
                        }
                        else
                        {
                            Zs = Vmas_Exhaust501_Now.ZS;
                        }
                    }
                    else
                    {
                        if (monizt == 1)
                        {
                            Zs = ZS_XZ + ran.Next(50, 150);
                        }
                        else if (monizt == 2)
                        {
                            Zs = gds + ran.Next(-99, 99);
                        }
                        else if (monizt == 3)
                        {
                            Zs = 780 + ran.Next(-99, 99);
                        }
                    }

                    co_ld = Vmas_Exhaust501_Now.CO;
                    co2_ld = Vmas_Exhaust501_Now.CO2;
                    hc_ld = Vmas_Exhaust501_Now.HC;
                    o2_ld = Vmas_Exhaust501_Now.O2;
                    no_ld = Vmas_Exhaust501_Now.NO;
                    λ_value_temp = Vmas_Exhaust_Now.λ;
                    if (carbj.ISUSE)
                    {
                        if (λ_value_temp > 0.97 && λ_value_temp < 1.03)
                        { }
                        else if (λ_value_temp < 1.6f && λ_value_temp > 0.7f)
                            λ_value_temp = 1.0f + (DateTime.Now.Millisecond % 6 - 3) * 0.01f;
                    }
                    λ_value = λ_value_temp;
                    if (IsUseTpTemp)
                    {
                        wd = (float)WD;
                        sd = (float)SD;
                        dqy = (float)DQY;
                    }
                    else
                    {
                        wd = (float)WD;
                        sd = (float)SD;
                        dqy = (float)DQY;
                    }
                    if (nhsjz != null && sdsconfig.Ywj == "南华附件")
                    {
                        if (nhsjz.readData())
                            yw = nhsjz.yw;
                    }
                    else
                    {
                        yw = Vmas_Exhaust501_Now.YW;
                    }
                    //yw = Vmas_Exhaust501_Now.YW;
                    isLowFlow = 0;
                    break;
            }
        }
        private DateTime fq_pre_time = DateTime.Now;
        private DateTime fq_now_time = DateTime.Now;
        public void Fq_Detect()
        {
            while (true)
            {
                if (sds_status)
                {
                    if (equipconfig.DATASECONDS_TYPE == "安车通用联网")
                    {
                        fq_now_time = DateTime.Now;
                        if (DateTime.Compare(DateTime.Parse(fq_now_time.ToString("yyyy-MM-dd HH:mm:ss")), DateTime.Parse(fq_pre_time.ToString("yyyy-MM-dd HH:mm:ss"))) > 0)
                        {
                            fq_pre_time = fq_now_time;
                            getRealData();
                        }
                    }
                    else
                    {
                        getRealData();
                    }
                }
                Thread.Sleep(100);
            }
        }
        private void button_ss_Click(object sender, EventArgs e)
        {
            try
            {

                button1.Visible = false;
                monizt = 0;
                monizs = false;
                if (button_ss.Text != "停止检测")
                {
                    datagridview_msg(dataGridView1, "结果", 0, "--");
                    datagridview_msg(dataGridView1, "结果", 1, "--");
                    datagridview_msg(dataGridView1, "结果", 2, "--");
                    datagridview_msg(dataGridView1, "结果", 3, "--");
                    datagridview_msg(dataGridView1, "结果", 4, "--");
                    jctime = DateTime.Now.ToString();
                    TH_ST = new Thread(Jc_Exe);
                    TH_ST.Start();
                    timer2.Start();
                    Th_get_FqandLl = new Thread(Fq_Detect);
                    Th_get_FqandLl.Start();//启动废气分析仪
                    timer_show.Start();//启动定时器,实时显示转速值
                    JC_Status = false;
                    sds_status = false;
                    button_ss.Text = "停止检测";
                }
                else
                {
                    TH_ST.Abort();
                    JC_Status = false;
                    sds_status = false;
                    timer_show.Stop();
                    timer2.Stop();
                    Th_get_FqandLl.Abort();
                    switch (UseFqy)
                    {
                        case "fla_502":
                            fla_502.Stop();
                            break;
                        case "mqw_511":
                            fla_502.Stop();
                            break;
                        case "fla_501":
                            break;
                    }
                    Msg(Msg_msg, panel_msg, carbj.CarPH + " 检测已停止", true);
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("测试被终止",2 , equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("请移除探测头", 5, equipconfig.Ledxh);
                    }
                    ts2 = "检测被终止";
                    BeginInvoke(new wt_void(Ref_Button));
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
            if (Msgowner.Width < Msgfather.Width)
                Msgowner.Location = new Point((Msgfather.Width - Msgowner.Width) / 2, Msgowner.Location.Y);
            else
                Msgowner.Location = new Point(0, Msgowner.Location.Y);
        }
        public void led_display(LEDNumber.LEDNumber lednumber, string data)
        {
            BeginInvoke(new wl_led(led_show), lednumber, data);
        }
        public void led_show(LEDNumber.LEDNumber lednumber, string data)
        {
            lednumber.LEDText = data;
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

        private void timer_show_Tick(object sender, EventArgs e)
        {
            if (sds_status)
            {
                arcScaleComponentZs.Value = Zs;
                led_display(ledNumberWd, wd.ToString("0.0"));
                led_display(ledNumberSd, sd.ToString("0.0"));
                led_display(ledNumberDqy, dqy.ToString("0.0"));
                led_display(ledNumberZS2, Zs.ToString("0"));
                if (vmasconfig.IfDisplayData)
                {
                    led_display(ledNumberCO, co_ld.ToString("0.00"));
                    led_display(ledNumberCO2, co2_ld.ToString("0.00"));
                    led_display(ledNumberO2, o2_ld.ToString("0.00"));
                    led_display(ledNumberHC, hc_ld.ToString("0.00"));
                    led_display(ledNumberNO, no_ld.ToString("0.00"));
                    led_display(ledNumberλ, λ_value.ToString("0.00"));
                    ts3 = "CO:" + co_ld.ToString("0.00") + " HC:" + hc_ld.ToString("0") + " λ:" + λ_value.ToString("0.00") + " CO2:" + co2_ld.ToString("0.00") + " O2:" + o2_ld.ToString("0.00") + " T:" + wd.ToString("0.0") + " H:" + sd.ToString("0.0") + " A:" + dqy.ToString("0.0");
                }
                else
                {
                    led_display(ledNumberCO,"—");
                    led_display(ledNumberCO2, "—");
                    led_display(ledNumberO2, "—");
                    led_display(ledNumberHC, "—");
                    led_display(ledNumberNO, "—");
                    led_display(ledNumberλ, "—");
                    ts3 = "—";
                }

            }
            else
            {
                led_display(ledNumberDqy, "0");
                led_display(ledNumberZS2, "0");
                arcScaleComponentZs.Value = 0f;
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            int zero_count = 0;
            switch (UseFqy)
            {
                case "fla_502":
                    fla_502.Zeroing();
                    Thread.Sleep(500);
                    zero_count = 0;
                    while (fla_502.Get_Struct() == "调零中")
                    {
                        Thread.Sleep(900);
                        Msg(Msg_msg, panel_msg, "废气仪调零中..." + zero_count.ToString() + "s", true);
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("废气仪调零中...",2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed(zero_count.ToString() + "s", 5, equipconfig.Ledxh);
                        }
                        zero_count++;
                        if (zero_count == 50)
                            break;
                    }
                    Msg(label_msgcs, panel2, "废气仪调零完成", true);
                    break;
                case "fla_501":
                    zero_count = 30;
                    fla_501.SetZero();
                    Thread.Sleep(500);
                    while (zero_count > 0)
                    {
                        Thread.Sleep(900);
                        Msg(Msg_msg, panel_msg, "废气仪调零中..." + zero_count.ToString() + "s", true);
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("废气仪调零中...", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed(zero_count.ToString() + "s", 5, equipconfig.Ledxh);
                        }
                        zero_count--;
                    }
                    Msg(label_msgcs, panel2, "废气仪调零完成", true);
                    break;
            }
            Msg(Msg_msg, panel_msg, "废气仪调零完毕", true);
        }

        private void button_blowback_Click(object sender, EventArgs e)
        {
            switch (UseFqy)
            {
                case "fla_502":
                    fla_502.Blowback();
                    break;
                case "mqw_511":
                    fla_502.Pump_Pipeair();
                    break;
                case "fla_501":
                    break;
            }
        }

        private void button_stopblowback_Click(object sender, EventArgs e)
        {
            switch (UseFqy)
            {
                case "fla_502":
                    fla_502.StopBlowback();
                    break;
                case "mqw_511":
                    fla_502.Stop();
                    break;
                case "fla_501":
                    break;
            }
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_settings_Click(object sender, EventArgs e)
        {
            settings newSettings = new settings();
            newSettings.ShowDialog();
            initConfigInfo();
        }


        private DateTime gc_time = DateTime.Now;//用于标记过程数据全程时序，避免出现相同时间
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (JC_Status)//如果正在测试
            {
                nowtime = DateTime.Now;
                led_display(ledNumber_gksj, gongkuangTime.ToString("000.0"));
                //if(Convert.ToInt16(gongkuangTime * 10) / 10 != GKSJ)
                if (DateTime.Compare(DateTime.Parse(nowtime.ToString("yyyy-MM-dd HH:mm:ss")), DateTime.Parse(gc_time.ToString("yyyy-MM-dd HH:mm:ss"))) > 0)
                {
                    gc_time = nowtime;
                    if (GKSJ == 2048) GKSJ = 0;
                    Sxnblist[GKSJ] = sxnb.ToString("0");//时序类别
                    Qcsxlist[GKSJ] = nowtime.ToString("yyyy-MM-dd HH:mm:ss.fff");//全程时序
                    Hclist[GKSJ] = hc_ld;
                    Nolist[GKSJ] = no_ld;
                    Colist[GKSJ] = co_ld;
                    Co2list[GKSJ] = co2_ld;
                    O2list[GKSJ] = o2_ld;
                    λlist[GKSJ] = λ_value;
                    if(Zs-gds>100&&Zs-gds<sdsconfig.Zscc)
                        Zslist[GKSJ] = gds+(int)Zs%100;
                    else if(Zs - gds < -100 && Zs - gds > -sdsconfig.Zscc)
                        Zslist[GKSJ] = gds - (int)Zs % 100;
                    else
                        Zslist[GKSJ] = Zs;
                    
                    Ywlist[GKSJ] = yw;
                    Wdlist[GKSJ] = wd;
                    Sdlist[GKSJ] = sd;
                    Dqylist[GKSJ] = dqy;
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
                    if(int.Parse(Sxnblist[GKSJ])>0&&equipconfig.useJHJK)//在时序类别大于0(非准备阶段)判断
                    {
                        if(O2list[GKSJ]>=10)
                        {
                            TH_ST.Abort();
                            JC_Status = false;
                            sds_status = false;
                            Msg(Msg_msg, panel_msg, carbj.CarPH + "监测到氧气大于10%，检测中止", true);
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("氧气大于10%", 5, equipconfig.Ledxh);
                            }
                            ts1 = "监测到氧气大于10%";
                            ts2 = "检测被终止";
                            Th_get_FqandLl.Abort();
                            BeginInvoke(new wt_void(Ref_Button));
                        }
                        if(λlist[GKSJ]<equipconfig.JHLAMBDAMIN|| λlist[GKSJ]>equipconfig.JHLAMBDAMAX)
                        {
                            TH_ST.Abort();
                            JC_Status = false;
                            sds_status = false;
                            Msg(Msg_msg, panel_msg, carbj.CarPH + "λ值不正常，检测中止", true);
                            if (ledcontrol != null)
                            {
                                ledcontrol.writeLed("测试停止", 2, equipconfig.Ledxh);
                                Thread.Sleep(200);
                                ledcontrol.writeLed("λ值不正常", 5, equipconfig.Ledxh);
                            }
                            ts1 = "λ值不正常";
                            ts2 = "检测被终止";
                            Th_get_FqandLl.Abort();
                            BeginInvoke(new wt_void(Ref_Button));
                        }
                    }
                    GKSJ++;//工况时间加1
                    effectiveGKSJ = GKSJ;
                }
                TimeSpan timespan = nowtime - startTime;
                gongkuangTime = (float)timespan.TotalMilliseconds / 1000f;
            }
            else
            {
                GKSJ = 0;
                effectiveGKSJ = 0;
                gongkuangTime = 0f;
            }
        }

        private void Sds_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!sdsIsFinished)
            {
                if (MessageBox.Show("测试未完成，确认退出？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    
                    sds_data.CarID = carbj.CarID;
                    sds_data.Sd = "-1";
                    sds_data.Wd = "-1";
                    sds_data.Dqy = "-1";
                    sds_data.λ_value = "-1";
                    sds_data.Hc_high = "-1";
                    sds_data.Co_high = "-1";
                    sds_data.Hc_low = "-1";
                    sds_data.Co_low = "-1";
                    sds_data.Co2_high = "-1";
                    sds_data.O2_high = "-1";
                    sds_data.Co2_low = "-1";
                    sds_data.O2_low = "-1";
                    sds_data.StartTime = "";
                    sds_data.StopReason = "9";
                    sdsdatacontrol.writeSdsData(sds_data);//写carID.ini文件
                    if (Th_get_FqandLl != null) Th_get_FqandLl.Abort();
                    if (TH_ST != null) TH_ST.Abort();
                    timer_show.Stop();
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

                try
                {
                    if (Th_get_FqandLl != null) Th_get_FqandLl.Abort();
                }
                catch { }
                try
                {
                    if (TH_ST != null) TH_ST.Abort();
                }
                catch { }
                    timer_show.Stop();
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
                        if (rpm5300 != null)
                        {
                            rpm5300.closeEquipment();
                        }
                        if (vmt_2000 != null)
                        {
                            if (vmt_2000.ComPort_1.IsOpen)
                                vmt_2000.ComPort_1.Close();
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            monizs = true;
            button1.Visible = false;
        }

        private void Sds_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Control&&e.Shift && e.KeyCode == Keys.M)
            {
                //button1.Visible = true;
                comboBoxZSJK.Visible = !comboBoxZSJK.Visible;
            }
        }

        private void buttonQR_Click(object sender, EventArgs e)
        {
            wsdValueIsRight = true;
            wsd_sure = true;
        }

        private void buttonSDSR_Click(object sender, EventArgs e)
        {

            wsdValueIsRight = false;
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
