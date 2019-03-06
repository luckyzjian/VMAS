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

namespace lzTest
{
    public partial class Lz_Btg : Form
    {
        carinfor.carInidata carbj = new carInidata();
        equipmentConfigInfdata equipconfig = new equipmentConfigInfdata();
        BtgConfigInfdata btgconfig = new BtgConfigInfdata();
        carIni carini = new carIni();
        configIni configini = new configIni();

        carinfor.zyjsBtgdata zyjs_data = new zyjsBtgdata();
        zyjsBtgdataControl zyjsdatacontrol = new zyjsBtgdataControl();

        private string UseFqy = "";
        Exhaust.Fla501 fla_501 = null;
        Exhaust.Fla502 fla_502 = null;
        Exhaust.FLB_100 flb_100 = null;
        Exhaust.YD_1 yd_1 = null;
        Exhaust.Flv_1000 flv_1000 = null;
        Exhaust.VMT_2000 vmt_2000 = null;
        bool isUseRotater = false;
        LedControl.BX5k1 ledcontrol = null;
        Dynamometer.IGBT igbt = null;
        private bool zyjsIsFinished = false;

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
        Exhaust.Flb_100_smoke dycsmoke = null;
        Exhaust.Flb_100_smoke decsmoke = null;
        Exhaust.Flb_100_smoke dscsmoke = null;
        Exhaust.Flb_100_smoke dsicsmoke = null;
        Exhaust.Flb_100_smoke smoke = null;
        public double prepareclz = 0.0;
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
        public double dsicclz = 0.0;
        public double pjz = 0.0;
        public double wd = 20;
        public double dqy = 101;
        public double sd = 20;
        public double btgxz = 0.0;
        public string pdjg = "合格";
        public string Pqfs = "";                                    //选择其排气方式
        public bool zyjs_status = false;
        public static float ZS = 0;
        public int dszs = 750;
        public string jctime = "";
        private bool isSongpin = false;
        public static int dyzs = 0;
        string ledComString = "";
        public static string ts1 = "川AV7M82";
        public static string ts2 = "滤纸式烟度法";
        public static string data1 = "--";
        public static string data2 = "--";
        public static string data3 = "--";
        public static string data4 = "--";
        private bool isautostart = false;
        public Lz_Btg()
        {
            String[] CmdArgs = System.Environment.GetCommandLineArgs();
            if (CmdArgs.Length <= 1)
            {
                isautostart = false;
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
        private void Zyjs_Btg_Load(object sender, EventArgs e)
        {
            initCarInfo();
            initConfigInfo();
            initDataResult();
            initEquipment();
            Init_Data();            //初始化数据
            Init_Limit();           //初始化限值
            Init_Show();            //初始化显示
            isSongpin = false;
            if (btgconfig.RotateSpeedMonitor)
                dyzs = btgconfig.Dyzs;
            else
                dyzs = 0;
            //断油转速 prepareform = new 断油转速();
            //prepareform.ShowDialog();
            if (carbj.CarBsxlx == "0")//自动档
            {
                dyzs = dyzs * 2 / 3;
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

            if (isautostart)
            {
                Thread.Sleep(3000);
                button1_Click(sender, e);
            }
        }

        #region 初始化
        private void initCarInfo()
        {
            carbj = carini.getCarIni();
            ts1 = carbj.CarPH;
        }
        private void initConfigInfo()
        {
            equipconfig = configini.getEquipConfigIni();
            isautostart = equipconfig.WorkAutomaticMode;
            btgconfig = configini.getBtgConfigIni();
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
                        igbt = new Dynamometer.IGBT("BNTD",equipconfig.isIgbtContainGdyk);
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
                if (equipconfig.Fqyifpz == true)
                {
                    switch (equipconfig.Fqyxh.ToLower())           //通过仪器型号选择初始化项目
                    {
                        case "fla_502":
                            try
                            {
                                UseFqy = "fla_502";
                                fla_502 = new Exhaust.Fla502(equipconfig.Fqyxh);
                                if (fla_502.Init_Comm(equipconfig.Fqyck, "9600,N,8,1") == false)
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
                                if (fla_502.Init_Comm(equipconfig.Fqyck, "9600,N,8,1") == false)
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
                                if (fla_502.Init_Comm(equipconfig.Fqyck, "9600,N,8,1") == false)
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
                                if (fla_501.Init_Comm(equipconfig.Fqyck, "9600,N,8,1") == false)
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
                if (equipconfig.Ydjifpz == true)
                {
                    try
                    {
                        flb_100 = new Exhaust.FLB_100(equipconfig.Ydjxh);
                        if (flb_100.Init_Comm(equipconfig.Ydjck, "9600,N,8,1") == false)
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
                if (equipconfig.Lzydjifpz == true)
                {
                    try
                    {
                        yd_1 = new Exhaust.YD_1(equipconfig.Lzydjxh,byte.Parse(equipconfig.lzydjadd));
                        if (yd_1.Init_Comm(equipconfig.Lzydjck, equipconfig.Lzckpzz) == false)
                        {
                            yd_1 = null;
                            Init_flag = false;
                            init_message += "滤纸烟度计串口打开失败.";
                        }
                    }
                    catch (Exception er)
                    {
                        yd_1 = null;
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
                        if (flv_1000.Init_Comm(equipconfig.Lljck, "9600,N,8,1") == false)
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
            
        }
        private void initDataResult()
        {
            //dt.Clear();
            dt.Columns.Add("项目");
            dt.Columns.Add("结果");
            DataRow dr = null;
            dr = dt.NewRow();
            dr["项目"] = "怠速转速";
            dr["结果"] = "--";
            //dr["判定"] = "--";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "第一次测量值";
            dr["结果"] = "--";
            //dr["判定"] = "--";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "第二次测量值";
            dr["结果"] = "--";
            //dr["判定"] = "--";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["项目"] = "第三次测量值";
            dr["结果"] = "--";
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
                    ledcontrol.writeLed(carbj.CarPH, 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("请上检测线准备", 5, equipconfig.Ledxh);
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
                    ledcontrol.writeLed("检查烟度计... ", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("　　检测即将开始", 5, equipconfig.Ledxh);
                }
                Thread.Sleep(1000);
                if (!yd_1.ResetTest())
                {
                    Msg(label_msg, panel_cp, "烟度计无法正常连接，请检查后重新开始。", true);
                    ts1 = "检测终止";
                    ts2 = "烟度计通讯故障";
                    zyjs_status = false;
                    JC_Status = false;
                    this.BeginInvoke(new wt_void(Ref_Button));
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("烟度计未连接　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("请检查后重新检测", 5, equipconfig.Ledxh);
                    }
                    return;
                }
                Msg(label_msg, panel_cp, "烟度计连接正常", true);
                ts2 = "烟度计连接正常";
                Thread.Sleep(3000);
                try//获取环境参数
                {
                    Thread.Sleep(1000);
                    //Exhaust.Fla502_data Environment = new Exhaust.Fla502_data();
                    Exhaust.Flb_100_smoke ydjEnvironment = new Exhaust.Flb_100_smoke();
                    if (equipconfig.TempInstrument == "烟度计")
                    {
                        ydjEnvironment = flb_100.get_Data();
                        wd = ydjEnvironment.WD;
                        sd = ydjEnvironment.SD;
                        dqy = ydjEnvironment.DQY;
                    }
                    else
                    {
                        if (equipconfig.Fqyxh.ToLower() == "nha_503")
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
                }
                catch (Exception)
                {
                }
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("测试即将开始　　", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("请按提示进行吹拂", 5, equipconfig.Ledxh);
                }

                zyjs_status = true;//烟度计开始获取数据

                Msg(label_msg, panel_cp, "检测即将开始，请按提示进行吹拂", true);
                zyjs_status = true;//烟度计开始获取数据
                ts2 = "请按提示进行吹拂";
                Thread.Sleep(2000);
                for (int cfcs = 0; cfcs < btgconfig.Btgcfcs; cfcs++)
                {
                    Msg(label_msgcs, panel1, "第" + (cfcs + 1).ToString() + "次吹拂", false);
                    Msg(label_msg, panel_cp, carbj.CarPH + "请迅猛将油门踩到底", true);
                    ts1 = "第" + (cfcs + 1).ToString() + "次吹拂";
                    ts2 = "请迅猛将油门踩到底";
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("第" + (cfcs + 1).ToString() + "次吹拂", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("　　请油门踩到底", 5, equipconfig.Ledxh);
                    }
                    Thread.Sleep(2000);
                    /*if (isUseRotater)
                    {
                        if (vmt_2000.readRotateSpeed())
                            ZS = vmt_2000.zs;
                    }
                    else if (flb_100 != null)
                    {
                        smoke = flb_100.get_Data();
                        ZS = smoke.Zs;
                    }*/
                    while (ZS < dyzs)
                    {
                        Msg(label_msg, panel_cp, carbj.CarPH + "未达到断油转速，请保持油门踩到底", true);

                        Thread.Sleep(400);
                    }
                    Msg(label_msg, panel_cp, carbj.CarPH + "请松开踏板", true);
                    ts2 = "松开踏板";
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("第" + (cfcs + 1).ToString() + "次吹拂", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("　　　请松开踏板", 5, equipconfig.Ledxh);
                    }
                    Thread.Sleep(2000);
                    for (int i = 10; i >= 0; i--)
                    {
                        /*if (isUseRotater)
                        {
                            if (vmt_2000.readRotateSpeed())
                                ZS = vmt_2000.zs;
                        }
                        else if (flb_100 != null)
                        {
                            smoke = flb_100.get_Data();
                            //smoke = flb_100.get_Data();
                            ZS = smoke.Zs;
                        }
                        arcScaleComponent3.Value = ZS;*/
                        Msg(label_msg, panel_cp, carbj.CarPH + ",保持怠速：" + " " + i.ToString(), true);
                        ts2 = "怠速..." + " " + i.ToString();
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("第" + (cfcs + 1).ToString() + "次吹拂", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed(" 　 怠速:" + i.ToString() + "秒", 5, equipconfig.Ledxh);
                            Thread.Sleep(700);
                            //ledcontrol.writeLed(i.ToString() + "秒", 5);
                        }
                        else
                            Thread.Sleep(900);
                    }
                }
                


                //烟度计要求等待1秒后再发送数据
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("测量即将开始　　", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("请安置探头和踏板", 5, equipconfig.Ledxh);
                }
                for (int i = 15; i >= 0; i--)
                {
                    Msg(label_msg, panel_cp, "请安置好探头和踏板..." + i.ToString(), false);
                    ts2 = "请安置探头和踏板..." + i.ToString();
                    Thread.Sleep(800);
                }
                //MessageBox.Show("确认探头是否已安好?", "系统提示");
                Msg(label_msg, panel_cp, "安置完毕，请根据提示进行操作。", false);
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("安置完毕　　　　", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("　　即将开始测量", 5, equipconfig.Ledxh);
                }
                ts2 = "请按提示进行操作";
                Thread.Sleep(2000);
                int sxnb = 0;
                if (btgconfig.btgclcs == 4)
                {
                    sxnb++;
                    Msg(label_msgcs, panel1, "第" + sxnb.ToString() + "次测量", false);
                    Msg(label_msg, panel_cp, carbj.CarPH + "请迅猛将油门踩到底", true);
                    ts1 = "第" + sxnb.ToString() + "次测量";
                    ts2 = "请迅猛将油门踩到底";
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("　　请油门踩到底", 5, equipconfig.Ledxh);
                    }
                    Thread.Sleep(2000);
                    while (ZS < dyzs)
                    {
                        Msg(label_msg, panel_cp, carbj.CarPH + "未达到断油转速，请保持油门踩到底", true);
                        Thread.Sleep(400);
                    }
                    Msg(label_msg, panel_cp, carbj.CarPH + "请松开踏板", true);
                    ts2 = "松开踏板";
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("　　　请松开踏板", 5, equipconfig.Ledxh);
                    }
                    //int waitTime = 0;
                    float tempdata = 0;
                    Exhaust.YD_1_smoke realsmoke = new Exhaust.YD_1_smoke();
                    for (int i = 0; i < 10; i++)
                    {
                        //dszs = (int)ZS;
                        Msg(label_msg, panel_cp, carbj.CarPH + "测量中，保持怠速：" + " " + (10 - i).ToString(), true);
                        ts2 = "测量中..." + " " + (10 - i).ToString();
                        
                        if (ledcontrol != null)
                        {
                            ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed(" 　测量中:" + (10 - i).ToString() + "秒", 5, equipconfig.Ledxh);
                            Thread.Sleep(500);
                            //ledcontrol.writeLed(i.ToString() + "秒", 5);
                        }
                        else
                            Thread.Sleep(900);
                    }
                    prepareclz = tempdata;
                    Msg(label_msg, panel_cp, "第" + sxnb.ToString() + "次测量完毕", true);
                    ts2 = "第" + sxnb.ToString() + "次测量完毕";
                    Thread.Sleep(1000);
                }
                else
                {
                    prepareclz = 0;
                }
                sxnb++;
                yd_1.startTest();
                Thread.Sleep(2000);                 
                Msg(label_msgcs, panel1, "第" + sxnb.ToString() + "次测量", false);
                Msg(label_msg, panel_cp, carbj.CarPH + "请迅猛将油门踩到底", true);
                ts1 = "第" + sxnb.ToString() + "次测量";
                ts2 = "请迅猛将油门踩到底";
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("　　请油门踩到底", 5, equipconfig.Ledxh);
                }
                Thread.Sleep(2000);
                while (ZS < dyzs)
                {
                    Msg(label_msg, panel_cp, carbj.CarPH + "未达到断油转速，请保持油门踩到底", true);
                    Thread.Sleep(400);
                }
                dyczs = (int)ZS;
                while (!yd_1.WaitFirstTestFinished())
                {
                    ts1 = "第" + sxnb.ToString() + "次测量";
                    ts2 = "测量中";
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("　　　测量中　　", 5, equipconfig.Ledxh);
                    }
                    Thread.Sleep(900);
                }
                Msg(label_msg, panel_cp, carbj.CarPH + "请松开踏板", true);
                ts2 = "松开踏板";
                    
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("　　　请松开踏板", 5, equipconfig.Ledxh);
                }
                Thread.Sleep(1000);
                for (int i = 0; i < 10; i++)
                {
                    //dszs = (int)ZS;
                    Msg(label_msg, panel_cp, carbj.CarPH + "测量中，保持怠速：" + " " + (10 - i).ToString(), true);
                    ts2 = "测量中..." + " " + (10 - i).ToString();
                    if(i==9)
                        dszs = (int)ZS;
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed(" 　测量中:" + (10 - i).ToString() + "秒", 5, equipconfig.Ledxh);
                        Thread.Sleep(500);
                        //ledcontrol.writeLed(i.ToString() + "秒", 5);
                    }
                    else
                        Thread.Sleep(900);
                }
                Msg(label_msg, panel_cp, "第" + sxnb.ToString() + "次测量完毕", true);
                ts2 = "第" + sxnb.ToString() + "次测量完毕";
                Thread.Sleep(1000);
                sxnb++;
                Msg(label_msgcs, panel1, "第" + sxnb.ToString() + "次测量", false);
                Msg(label_msg, panel_cp, carbj.CarPH + "请迅猛将油门踩到底", true);
                ts1 = "第" + sxnb.ToString() + "次测量";
                ts2 = "请迅猛将油门踩到底";
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("　　请油门踩到底", 5, equipconfig.Ledxh);
                }
                Thread.Sleep(2000);
                while (ZS < dyzs)
                {
                    Msg(label_msg, panel_cp, carbj.CarPH + "未达到断油转速，请保持油门踩到底", true);
                    Thread.Sleep(400);
                }
                deczs = (int)ZS;
                while (!yd_1.WaitFirstTestFinished())
                {
                    ts1 = "第" + sxnb.ToString() + "次测量";
                    ts2 = "测量中";
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("　　　测量中　　", 5, equipconfig.Ledxh);
                    }
                    Thread.Sleep(900);
                }
                Msg(label_msg, panel_cp, carbj.CarPH + "请松开踏板", true);
                ts2 = "松开踏板";
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("　　　请松开踏板", 5, equipconfig.Ledxh);
                }
                Thread.Sleep(1000);
                for (int i = 0; i < 10; i++)
                {
                    //dszs = (int)ZS;
                    Msg(label_msg, panel_cp, carbj.CarPH + "测量中，保持怠速：" + " " + (10 - i).ToString(), true);
                    ts2 = "测量中..." + " " + (10 - i).ToString();
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed(" 　测量中:" + (10 - i).ToString() + "秒", 5, equipconfig.Ledxh);
                        Thread.Sleep(500);
                        //ledcontrol.writeLed(i.ToString() + "秒", 5);
                    }
                    else
                        Thread.Sleep(900);
                }
                Msg(label_msg, panel_cp, "第" + sxnb.ToString() + "次测量完毕", true);
                ts2 = "第" + sxnb.ToString() + "次测量完毕";
                Thread.Sleep(1000);
                sxnb++;
                Msg(label_msgcs, panel1, "第" + sxnb.ToString() + "次测量", false);
                Msg(label_msg, panel_cp, carbj.CarPH + "请迅猛将油门踩到底", true);
                ts1 = "第" + sxnb.ToString() + "次测量";
                ts2 = "请迅猛将油门踩到底";
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("　　请油门踩到底", 5, equipconfig.Ledxh);
                }
                Thread.Sleep(2000);
                while (ZS < dyzs)
                {
                    Msg(label_msg, panel_cp, carbj.CarPH + "未达到断油转速，请保持油门踩到底", true);
                    Thread.Sleep(400);
                }
                dsczs = (int)ZS;
                while (!yd_1.WaitTestFinished())
                {
                    ts1 = "第" + sxnb.ToString() + "次测量";
                    ts2 = "测量中";
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("　　　测量中　　", 5, equipconfig.Ledxh);
                    }
                    Thread.Sleep(900);
                }
                Msg(label_msg, panel_cp, carbj.CarPH + "请松开踏板", true);
                ts2 = "松开踏板";
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("　　　请松开踏板", 5, equipconfig.Ledxh);
                }
                Thread.Sleep(1000);
                for (int i = 0; i < 10; i++)
                {
                    //dszs = (int)ZS;
                    Msg(label_msg, panel_cp, carbj.CarPH + "测量中，保持怠速：" + " " + (10 - i).ToString(), true);
                    ts2 = "测量中..." + " " + (10 - i).ToString();
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("第" + sxnb.ToString() + "次测量　　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed(" 　测量中:" + (10 - i).ToString() + "秒", 5, equipconfig.Ledxh);
                        Thread.Sleep(500);
                        //ledcontrol.writeLed(i.ToString() + "秒", 5);
                    }
                    else
                        Thread.Sleep(900);
                }
                Msg(label_msg, panel_cp, "第" + sxnb.ToString() + "次测量完毕", true);
                ts2 = "第" + sxnb.ToString() + "次测量完毕";
                Thread.Sleep(1000);
                Exhaust.YD_1_smoke lzsmoke = new Exhaust.YD_1_smoke();
                lzsmoke = yd_1.readResult();
                if (lzsmoke.Success)
                {
                    dycclz = lzsmoke.FirstData;
                    decclz = lzsmoke.SencondData;
                    dscclz = lzsmoke.ThirdData;
                    datagridview_msg(dataGridView1, "结果", 1, dycclz.ToString("0.0"));
                    datagridview_msg(dataGridView1, "结果", 2, decclz.ToString("0.0"));
                    datagridview_msg(dataGridView1, "结果", 3, dscclz.ToString("0.0"));
                    data1 = dycclz.ToString("0.0");
                    data2 = decclz.ToString("0.0");
                    data3 = dscclz.ToString("0.0");
                    data4 = ((dycclz + decclz + dscclz) / 3.0).ToString("0.00");
                }
                else
                {
                    Msg(label_msg, panel_cp, "读取结果失败，请检查后重新开始。", true);
                    ts1 = "读取结果失败";
                    ts2 = "请检查后重新开始";
                    zyjs_status = false;
                    JC_Status = false;
                    this.BeginInvoke(new wt_void(Ref_Button));
                    if (ledcontrol != null)
                    {
                        ledcontrol.writeLed("读取结果失败　　", 2, equipconfig.Ledxh);
                        Thread.Sleep(200);
                        ledcontrol.writeLed("请检查后重新检测", 5, equipconfig.Ledxh);
                    }
                    return;
                }
                JC_Status = false;
                this.BeginInvoke(new wt_void(Ref_Button));
                Ref_Control_Text(label_st, "检测完成");
                Msg(label_msgcs, panel1, "检测完成", false);
                ts1 = "检测完成";
                ts2 = "移除探头驶离检测线";
                if (ledcontrol != null)
                {
                    ledcontrol.writeLed("测试完成　　　　", 2, equipconfig.Ledxh);
                    Thread.Sleep(200);
                    ledcontrol.writeLed("　　请移除探测头", 5, equipconfig.Ledxh);
                }
                Msg(label_msgcs, panel1, "检测完成", false);
                Msg(label_msg, panel_cp, carbj.CarPH + "测试完成,请移除探头后驶离检测线" , true);
                Thread.Sleep(1000);
                zyjs_status = false;//停止获取烟度计数据
                JC_Status = false;
                yw = 0f;
                zyjs_data.CarID = carbj.CarID;
                zyjs_data.Wd = wd.ToString("0.0");
                zyjs_data.Sd = sd.ToString("0.0");
                zyjs_data.Dqy = dqy.ToString("0.0");
                zyjs_data.FirstData = dycclz.ToString("0.0");
                zyjs_data.SecondData = decclz.ToString("0.0");
                zyjs_data.ThirdData = dscclz.ToString("0.0");
                zyjs_data.Dszs = dszs.ToString("0");
                zyjs_data.Yw = yw.ToString("0.0");
                zyjs_data.Rev1 = dyczs.ToString("0");
                zyjs_data.Rev2 = deczs.ToString("0");
                zyjs_data.Rev3 = dsczs.ToString("0");
                zyjs_data.prepareData1 = "0";
                zyjs_data.prepareData2 = "0";
                zyjs_data.prepareData3 = "0";
                zyjs_data.PrepareData = prepareclz.ToString("0.0");
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

        Thread Th_get_FqandLl = null;
        Exhaust.RPM5300 rpm5300 = null;
        bool isReadRealTime = false;

        public void Fq_Detect()
        {
            while (true)
            {
                if (zyjs_status)
                {

                    if (equipconfig.Ydjxh == "mqy_200")
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
                    if (!isUseRotater)
                        ZS = (int)(smoke.Zs);
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
                Msg(label_zstext, panel_zstext, ZS.ToString(), false);
                arcScaleComponent3.Value = ZS;
                arcScaleComponent1.Value=ZS;
                Thread.Sleep(200);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (JC_Status == false)
                {
                    datagridview_msg(dataGridView1, "结果", 0, "--");
                    datagridview_msg(dataGridView1, "结果", 1, "--");
                    datagridview_msg(dataGridView1, "结果", 2, "--");
                    datagridview_msg(dataGridView1, "结果", 3, "--");
                    jctime = DateTime.Now.ToString();
                    TH_ST = new Thread(Jc_Exe);
                    TH_ST.Start();
                    Th_get_FqandLl = new Thread(Fq_Detect);
                    Th_get_FqandLl.Start();
                    JC_Status = true;
                    button_ss.Text = "停止检测";
                }
                else
                {
                    TH_ST.Abort();
                    Th_get_FqandLl.Abort();
                    //TH_YD.Abort();
                    zyjs_status = false;
                    //timer1.Stop();
                    JC_Status = false;
                    Msg(label_msg, panel1, carbj.CarPH + " 检测已停止", true);
                    ts2 = "检测被终止";
                    if (ledcontrol != null)
                    {
                        Thread.Sleep(500);
                        ledcontrol.writeLed("测试被终止", 2, equipconfig.Ledxh);
                        //ledcontrol.writeLed("请移除探测头", 5);
                    }

                    button_ss.Text = "重新检测";
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
                            ledcontrol.writeLed("小心驾驶安全第一", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("　　欢迎参检　　", 5, equipconfig.Ledxh);
                        }
                        if (Th_get_FqandLl != null) Th_get_FqandLl.Abort();
                        zyjs_data.CarID = carbj.CarID;
                        zyjs_data.Wd = "-1";
                        zyjs_data.Sd = "-1";
                        zyjs_data.Dqy = "-1";
                        zyjs_data.FirstData = "-1";
                        zyjs_data.SecondData = "-1";
                        zyjs_data.ThirdData = "-1";
                        zyjs_data.Dszs = "-1";
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
                        if (yd_1 != null)
                        {
                            if (yd_1.ComPort_3.IsOpen)
                                yd_1.ComPort_3.Close();
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
                            ledcontrol.writeLed("小心驾驶安全第一", 2, equipconfig.Ledxh);
                            Thread.Sleep(200);
                            ledcontrol.writeLed("　　欢迎参检　　", 5, equipconfig.Ledxh);
                    }
                    if (Th_get_FqandLl != null) Th_get_FqandLl.Abort();
                    if (TH_ST != null) TH_ST.Abort();
                        if (flb_100 != null)
                        {
                            if (flb_100.ComPort_3.IsOpen)
                                flb_100.ComPort_3.Close();
                        }
                        if (yd_1 != null)
                        {
                            if (yd_1.ComPort_3.IsOpen)
                                yd_1.ComPort_3.Close();
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
                }
            }
            catch
            { }
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
