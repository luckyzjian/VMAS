using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using carinfor;
using Dynamometer;
using System.Threading;
using System.IO;


namespace QxzDemarcate
{
    public partial class Form1 : Form
    {
        configIni configini = new configIni();
        equipmentConfigInfdata equipconfig = new equipmentConfigInfdata();
        parasiticData parasiticdata = new parasiticData();
        selfCheckData selfcheckdata = new selfCheckData();
        selfCheckItem selfcheckitem = new selfCheckItem();
        selfCheckIni selfcheckini = new selfCheckIni();
        thaxs thaxsdata = new thaxs();
        private string UseFqy = "";
        Exhaust.Fla501 fla_501 = null;
        Exhaust.Fla502 fla_502 = null;
        Exhaust.FLB_100 flb_100 = null;
        Exhaust.Flv_1000 flv_1000 = null;
        Exhaust.VMT_2000 vmt_2000 = null;
        Exhaust.XCE_100 xce_100 = null;
        private Exhaust.yhControl yhy = null;
        bool isUseRotater = false;
        IGBT igbt = null;
        double wd, sd, dqy;
        double actwd, actsd, actdqy;
        DateTime kssj, jssj;
        bool isDermacated = false;
        bool isSaved = false;
        bool readingTemp = false;
        temperatureData tempdata = new temperatureData();
        public delegate void wtcs(Control controlname, string text);                                //委托
        public delegate void wtlsb(Label Msgowner, string Msgstr);                  //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                                 //委托
        public delegate void wttlsb(ToolStripLabel Msgowner, string Msgstr);                  //委托
        public Form1()
        {
            InitializeComponent();
        }
        private void initConfigInfo()
        {
            equipconfig = configini.getEquipConfigIni();
            selfcheckdata = configini.getSelfCheckDataIni();
            selfcheckitem = configini.getSelfCheckItemIni();
            parasiticdata = configini.getParasiticIni();
            thaxsdata = configini.getthaxsConfigIni();
        }
        public void Init()
        {
            initConfigInfo();
            initEquipment();
            timer1.Start();
            readingTemp = true;
        }
        private bool IsUseTpTemp = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
            kssj = DateTime.Now;
            if (equipconfig.isTpTempInstrument)
            {
                if (File.Exists("C://jcdatatxt/环境数据.ini"))
                {
                    string wds, sds, dqys;
                    StringBuilder temp = new StringBuilder();
                    temp.Length = 2048;
                    ini.INIIO.GetPrivateProfileString("环境数据", "wd", "", temp, 2048, "C:/jcdatatxt/环境数据.ini");
                    wds = temp.ToString();
                    ini.INIIO.GetPrivateProfileString("环境数据", "sd", "", temp, 2048, "C:/jcdatatxt/环境数据.ini");
                    sds = temp.ToString();
                    ini.INIIO.GetPrivateProfileString("环境数据", "dqy", "", temp, 2048, "C:/jcdatatxt/环境数据.ini");
                    dqys = temp.ToString();
                    textBoxWd.Text = wds;
                    textBoxSd.Text = sds;
                    textBoxDqy.Text = dqys;
                    wd = double.Parse(wds);
                    sd = double.Parse(sds);
                    dqy = double.Parse(dqys);
                    IsUseTpTemp = true;
                }
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
                if (equipconfig.Ydjifpz == true && equipconfig.Ydjxh != "CDF5000")
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            try//获取环境参数
            {
                if (readingTemp)
                {
                    Exhaust.Fla502_data Environment = new Exhaust.Fla502_data();
                    Exhaust.Flb_100_smoke ydjEnvironment = new Exhaust.Flb_100_smoke();
                    if (IsUseTpTemp)
                    {
                        wd = wd;
                        sd = sd;
                        dqy = dqy;
                    }
                    else if (equipconfig.TempInstrument == "烟度计"&&flb_100!=null)
                    {
                        ydjEnvironment = flb_100.get_Data();
                        wd = ydjEnvironment.WD;
                        sd = ydjEnvironment.SD;
                        dqy = ydjEnvironment.DQY;
                    }
                    else if (equipconfig.TempInstrument == "废气仪")
                    {
                        if (equipconfig.Fqyxh.ToLower() == "nha_503" || equipconfig.Fqyxh.ToLower() == "fla_502" || equipconfig.Fqyxh.ToLower() == "cdf5000")
                        {
                            Exhaust.Fla502_temp_data flaEnvironment = fla_502.Get_Temp();
                            wd = flaEnvironment.TEMP;
                            sd = flaEnvironment.HUMIDITY;
                            dqy = flaEnvironment.AIRPRESSURE;
                        }
                        else
                        {
                            Environment = fla_502.GetData();
                            wd = Environment.HJWD;
                            sd = Environment.SD;
                            dqy = Environment.HJYL;
                        }
                    }
                    else if (equipconfig.TempInstrument == "油耗仪")
                    {
                        Exhaust.yhrRealTimeData yhyEnvironment = new Exhaust.yhrRealTimeData();
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
                    else if (equipconfig.TempInstrument == "DWSP_T5")
                    {
                        if (xce_100.readEnvironment())
                        {
                            wd = xce_100.temp;
                            sd = xce_100.humidity;
                            dqy = xce_100.airpressure;
                        }
                    }
                    else if (equipconfig.TempInstrument == "FTH_2")
                    {
                        if (xce_100.readEnvironment())
                        {
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
                    }
                    actwd = thaxsdata.Tempxs * wd;
                    actsd = thaxsdata.Humixs * sd;
                    actdqy = thaxsdata.Airpxs * dqy;

                    Msg(labelWd, panelWd, actwd.ToString("0.0"));
                    Msg(labelSd, panelWd, actsd.ToString("0.0"));
                    Msg(labelDqy, panelDqy, actdqy.ToString("0.0"));
                }
            }
            catch (Exception)
            {
            }
        }
        #region 信息显示
        /// <summary>
        /// 信息显示
        /// </summary>
        /// <param name="Msgowner">信息显示的Label控件</param>
        /// <param name="Msgfather">Label控件的父级Panel</param>
        /// <param name="Msgstr">要显示的信息</param>
        /// <param name="Update_DB">是不是要更新到检测状态</param>
        public void Msg(Label Msgowner, Panel Msgfather, string Msgstr)
        {
            BeginInvoke(new wtlsb(Msg_Show), Msgowner, Msgstr);
            BeginInvoke(new wtlp(Msg_Position), Msgowner, Msgfather);
        }
        public void Msg_Toollabel(ToolStripLabel Msgowner, string Msgstr)
        {
            BeginInvoke(new wttlsb(Msg_toollabel), Msgowner, Msgstr);
        }
        public void Msg_toollabel(ToolStripLabel Msgowner, string Msgstr)
        {
            Msgowner.Text = Msgstr;
            //BeginInvoke(new wtlsb(Msg_Show), Msgowner, Msgstr, Update_DB);
            // BeginInvoke(new wtlp(Msg_Position), Msgowner, Msgfather);
        }

        public void Msg_Show(Label Msgowner, string Msgstr)
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

        private void button_qdJSS_Click(object sender, EventArgs e)
        {
            double wdxdwc, sdxdwc, dqyxdwc, wdjdwc, sdjdwc, dqyjdwc;
            bool pdjg=true;
            hjcsgyqSelfcheck hjcsdata = new hjcsgyqSelfcheck();
            hjcsdata.ActualAirPressure = double.Parse(textBoxDqy.Text);
            hjcsdata.AirPressure = actdqy;

            hjcsdata.ActualTemperature = double.Parse(textBoxWd.Text);
            hjcsdata.Temperature =actwd;
            hjcsdata.ActualHumidity = double.Parse(textBoxSd.Text);
            hjcsdata.Humidity = actsd;
            hjcsdata.CheckTimeStart = kssj.ToString("yyyy-MM-dd HH:mm:ss");
            hjcsdata.CheckTimeEnd = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            hjcsdata.Remark = "";

            tempdata.Dqybz = double.Parse(textBoxDqy.Text);
            tempdata.Dqyscz = actdqy;

            tempdata.Wdbz = double.Parse(textBoxWd.Text);
            tempdata.Wdscz = actwd;
            tempdata.Sdbz = double.Parse(textBoxSd.Text);
            tempdata.Sdscz = actsd;
            
            //selfcheckini.writedzhjSelfcheck(hjcsdata);
            wdjdwc = Math.Abs(hjcsdata.Temperature - hjcsdata.ActualTemperature);
            wdxdwc = Math.Round(wdjdwc / hjcsdata.ActualTemperature, 3);
            if (wdjdwc > 0.04 && wdjdwc > 1)
            {
                pdjg = false;
                LabelWdpd.Text = "×";
            }
            else
                LabelWdpd.Text = "√";
            tempdata.Wdwc = wdjdwc;
            sdjdwc = Math.Abs(hjcsdata.Humidity - hjcsdata.ActualHumidity);
            sdxdwc = Math.Round(sdjdwc / hjcsdata.ActualHumidity, 3);
            if (sdjdwc > 3 && sdxdwc > 0.05)
            {
                pdjg = false;
                LabelSdpd.Text = "×";
            }
            else
                LabelSdpd.Text = "√";
            tempdata.Sdwc = sdjdwc;
            dqyjdwc = Math.Abs(hjcsdata.AirPressure - hjcsdata.ActualAirPressure);
            dqyxdwc = Math.Round(dqyjdwc / hjcsdata.ActualAirPressure, 3);
            if (dqyjdwc > 1.5 && dqyxdwc > 0.03)
            {
                pdjg = false;
                LabelDqypd.Text = "×";
            }
            else
                LabelDqypd.Text = "√";
            tempdata.Dqywc=dqyjdwc;
            labelYqwd.Text = hjcsdata.Temperature.ToString("0.0");
            labelYqsd.Text = hjcsdata.Humidity.ToString("0.0");
            labelYqdqy.Text = hjcsdata.AirPressure.ToString("0.0");
            labelwdwc.Text = (wdxdwc * 100).ToString("0.0") + "%/" + wdjdwc.ToString("0.0");
            labelsdwc.Text = (sdxdwc * 100).ToString("0.0") + "%/" + sdjdwc.ToString("0.0");
            labeldqywc.Text = (dqyxdwc * 100).ToString("0.0") + "%/" + dqyjdwc.ToString("0.0");
            if (pdjg)
            {
                MessageBox.Show("检查合格");
                tempdata.Bdjg = "合格";
            }
            else
            {
                MessageBox.Show("检查不合格,请校准气象站");
                tempdata.Bdjg = "不合格";
            }
            tempdata.Bzsm = LabelWdpd.Text + ";" + LabelSdpd.Text + ";" + LabelDqypd.Text;
            isDermacated = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSaved)
            {
                if (MessageBox.Show("还未保存数据，确认退出吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    try
                    {
                        
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
                            if (yhy != null)
                            {
                                if (yhy.ComPort_1.IsOpen)
                                    yhy.ComPort_1.Close();
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
                        }
                        catch
                        { }
                    }
                    catch (Exception)
                    {
                        e.Cancel = true;
                    }
                }
                else
                { 
                }
            }
            else
            {
                try
                {
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
                        if (yhy != null)
                        {
                            if (yhy.ComPort_1.IsOpen)
                                yhy.ComPort_1.Close();
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
                    }
                    catch
                    { }
                }
                catch (Exception)
                {

                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (isDermacated == false)
            {
                MessageBox.Show("没有数据可进行保存", "提示");
                return;
            }
            else
            {
                temperatureIni tempini = new temperatureIni();
                tempini.writeanalysismeterIni(tempdata);
                MessageBox.Show("数据保存完毕", "提示");
                isSaved = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double realwd, realsd, realdqy, equipwd, equipsd, equipdqy;
            try
            {
                realwd = double.Parse(textBoxWd.Text);
                realsd = double.Parse(textBoxSd.Text);
                realdqy = double.Parse(textBoxDqy.Text);
            }
            catch
            {
                MessageBox.Show("输入格式有误");
                return;
            }
            string msg="";
            if (equipconfig.TempInstrument == "废气仪" && equipconfig.Fqyxh.ToLower() == "mqw_50a"&&fla_502!=null)
            {
                /*
                readingTemp = false;
                Thread.Sleep(500);
                if(fla_502.Demarcate_Temp((float)realwd))
                {
                    msg += "温度校正成功";
                }
                else
                {
                    msg += "温度校正失败";
                }
                Thread.Sleep(200);
                if (fla_502.Demarcate_Humidity((float)realsd))
                {
                    //thaxsdata.Humixs = realsd / sd;
                    msg += "湿度校正成功";
                }
                else
                {
                    msg += "湿度校正失败";
                }
                Thread.Sleep(200);
                if (fla_502.Demarcate_Airpressure((float)realdqy))
                {
                    //thaxsdata.Airpxs = realdqy / dqy;
                    msg += "大气压校正成功";
                }
                else
                {
                    msg += "大气压校正失败";
                }
                Thread.Sleep(500);
                readingTemp = true;
                //MessageBox.Show(msg, "提示");*/
                msg += "请使用上方鸣泉标准功能进行校准";

            }
            else
            {
                if (wd != 0)
                {
                    thaxsdata.Tempxs = realwd / wd;
                    msg += "温度校正成功";
                }
                else
                {
                    msg += "温度校正失败";
                }
                if (sd != 0)
                {
                    thaxsdata.Humixs = realsd / sd;
                    msg += "湿度校正成功";
                }
                else
                {
                    msg += "湿度校正失败";
                }
                if (dqy != 0)
                {
                    thaxsdata.Airpxs = realdqy / dqy;
                    msg += "大气压校正成功";
                }
                else
                {
                    msg += "大气压校正失败";
                }
                configini.writeThaxsConfigIni(thaxsdata);
            }
            MessageBox.Show(msg, "提示");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            thaxsdata.Tempxs = 1;
            thaxsdata.Humixs = 1;
            thaxsdata.Airpxs = 1; 
            configini.writeThaxsConfigIni(thaxsdata);
            MessageBox.Show("已恢复出厂设置", "提示");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (equipconfig.TempInstrument == "废气仪" && fla_502 != null)
            {
                double realwd, realsd, realdqy, equipwd, equipsd, equipdqy;
                if (equipconfig.Fqyxh.ToLower() == "mqw_50a"|| equipconfig.Fqyxh.ToLower() == "fla_502"|| equipconfig.Fqyxh.ToLower() == "nha_503")
                {
                    try
                    {
                        realwd = double.Parse(textBoxWd.Text);
                        //realsd = double.Parse(textBoxSd.Text);
                        //realdqy = double.Parse(textBoxDqy.Text);
                    }
                    catch
                    {
                        MessageBox.Show("输入格式有误");
                        return;
                    }
                    readingTemp = false;
                    Thread.Sleep(500);
                    if (fla_502.Demarcate_Temp((float)realwd))
                    {
                        Thread.Sleep(3000);
                        msg += "温度校正成功";
                    }
                    else
                    {
                        Thread.Sleep(3000);
                        msg += "温度校正成功";
                    }
                    Thread.Sleep(500);
                    readingTemp = true;
                    MessageBox.Show(msg, "提示");
                }
                
            }
            else if (equipconfig.TempInstrument == "烟度计" && flb_100 != null)// if(equipconfig.TempInstrument == "XCE_100"|| equipconfig.TempInstrument == "DWSP_T5" || equipconfig.TempInstrument == "FTH_2" || equipconfig.TempInstrument == "RZ_1")
            {
                double realwd, realsd, realdqy, equipwd, equipsd, equipdqy;
                if (equipconfig.Ydjxh.ToLower() == "mqy_200" )
                {
                    try
                    {
                        realwd = double.Parse(textBoxWd.Text);
                        //realsd = double.Parse(textBoxSd.Text);
                        //realdqy = double.Parse(textBoxDqy.Text);
                    }
                    catch
                    {
                        MessageBox.Show("输入格式有误");
                        return;
                    }
                    readingTemp = false;
                    Thread.Sleep(500);
                    if (flb_100.Set_DermacateWD((float)realwd))
                    {
                        Thread.Sleep(3000);
                        msg += "温度校正成功";
                    }
                    else
                    {
                        Thread.Sleep(3000);
                        msg += "温度校正成功";
                    }
                    Thread.Sleep(500);
                    readingTemp = true;
                    MessageBox.Show(msg, "提示");
                }
            }
            else// if(equipconfig.TempInstrument == "XCE_100"|| equipconfig.TempInstrument == "DWSP_T5" || equipconfig.TempInstrument == "FTH_2" || equipconfig.TempInstrument == "RZ_1")
            {
                double realwd, realsd, realdqy, equipwd, equipsd, equipdqy;
                try
                {
                    realwd = double.Parse(textBoxWd.Text);
                    realsd = double.Parse(textBoxSd.Text);
                    realdqy = double.Parse(textBoxDqy.Text);
                }
                catch
                {
                    MessageBox.Show("输入格式有误");
                    return;
                }
                if (wd != 0)
                {
                    thaxsdata.Tempxs = realwd / wd;
                    msg += "温度校正成功";
                }
                else
                {
                    msg += "温度校正失败";
                }                
                configini.writeThaxsConfigIni(thaxsdata);
                MessageBox.Show(msg, "提示");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (equipconfig.TempInstrument == "废气仪"&& fla_502 != null)
            {
                double realwd, realsd, realdqy, equipwd, equipsd, equipdqy;
                if (equipconfig.Fqyxh.ToLower() == "mqw_50a" || equipconfig.Fqyxh.ToLower() == "fla_502" || equipconfig.Fqyxh.ToLower() == "nha_503")
                {
                    try
                    {
                        //realwd = double.Parse(textBoxWd.Text);
                        realsd = double.Parse(textBoxSd.Text);
                        //realdqy = double.Parse(textBoxDqy.Text);
                    }
                    catch
                    {
                        MessageBox.Show("输入格式有误");
                        return;
                    }
                    readingTemp = false;
                    Thread.Sleep(500);
                    if (fla_502.Demarcate_Humidity((float)realsd))
                    {
                        //thaxsdata.Humixs = realsd / sd;
                        Thread.Sleep(3000);
                        msg += "湿度校正成功";
                    }
                    else
                    {
                        Thread.Sleep(3000);
                        msg += "湿度校正成功";
                    }
                    Thread.Sleep(500);
                    readingTemp = true;
                    MessageBox.Show(msg, "提示");
                }
            }
            else if (equipconfig.TempInstrument == "烟度计" && flb_100 != null)// if(equipconfig.TempInstrument == "XCE_100"|| equipconfig.TempInstrument == "DWSP_T5" || equipconfig.TempInstrument == "FTH_2" || equipconfig.TempInstrument == "RZ_1")
            {
                double realwd, realsd, realdqy, equipwd, equipsd, equipdqy;
                if (equipconfig.Ydjxh.ToLower() == "mqy_200")
                {
                    try
                    {
                        realsd = double.Parse(textBoxSd.Text);
                        //realsd = double.Parse(textBoxSd.Text);
                        //realdqy = double.Parse(textBoxDqy.Text);
                    }
                    catch
                    {
                        MessageBox.Show("输入格式有误");
                        return;
                    }
                    readingTemp = false;
                    Thread.Sleep(500);
                    if (flb_100.Set_DermacateSD((float)realsd))
                    {
                        Thread.Sleep(3000);
                        msg += "湿度校正成功";
                    }
                    else
                    {
                        Thread.Sleep(3000);
                        msg += "湿度校正成功";
                    }
                    Thread.Sleep(500);
                    readingTemp = true;
                    MessageBox.Show(msg, "提示");
                }
            }
            else// if(equipconfig.TempInstrument == "XCE_100"|| equipconfig.TempInstrument == "DWSP_T5" || equipconfig.TempInstrument == "FTH_2" || equipconfig.TempInstrument == "RZ_1")
            {
                double realwd, realsd, realdqy, equipwd, equipsd, equipdqy;
                try
                {
                    realwd = double.Parse(textBoxWd.Text);
                    realsd = double.Parse(textBoxSd.Text);
                    realdqy = double.Parse(textBoxDqy.Text);
                }
                catch
                {
                    MessageBox.Show("输入格式有误");
                    return;
                }
                if (sd != 0)
                {
                    thaxsdata.Humixs = realsd / sd;
                    msg += "湿度校正成功";
                }
                else
                {
                    msg += "湿度校正失败";
                }
                configini.writeThaxsConfigIni(thaxsdata);
                MessageBox.Show(msg, "提示");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (equipconfig.TempInstrument == "废气仪" && fla_502 != null)
            {
                double realwd, realsd, realdqy, equipwd, equipsd, equipdqy;
                if (equipconfig.Fqyxh.ToLower() == "mqw_50a" || equipconfig.Fqyxh.ToLower() == "fla_502" || equipconfig.Fqyxh.ToLower() == "nha_503")
                {
                    try
                    {
                        //realwd = double.Parse(textBoxWd.Text);
                        //realsd = double.Parse(textBoxSd.Text);
                        realdqy = double.Parse(textBoxDqy.Text);
                    }
                    catch
                    {
                        MessageBox.Show("输入格式有误");
                        return;
                    }
                    readingTemp = false;
                    Thread.Sleep(500);
                    if (fla_502.Demarcate_Airpressure((float)realdqy))
                    {
                        //thaxsdata.Airpxs = realdqy / dqy;
                        Thread.Sleep(3000);
                        msg += "大气压校正成功";
                    }
                    else
                    {
                        Thread.Sleep(3000);
                        msg += "大气压校正成功";
                    }
                    Thread.Sleep(500);
                    readingTemp = true;
                    MessageBox.Show(msg, "提示");
                }
            }
            else if (equipconfig.TempInstrument == "烟度计" && flb_100 != null)// if(equipconfig.TempInstrument == "XCE_100"|| equipconfig.TempInstrument == "DWSP_T5" || equipconfig.TempInstrument == "FTH_2" || equipconfig.TempInstrument == "RZ_1")
            {
                double realwd, realsd, realdqy, equipwd, equipsd, equipdqy;
                if (equipconfig.Ydjxh.ToLower() == "mqy_200")
                {
                    try
                    {
                        realdqy = double.Parse(textBoxDqy.Text);
                        //realsd = double.Parse(textBoxSd.Text);
                        //realdqy = double.Parse(textBoxDqy.Text);
                    }
                    catch
                    {
                        MessageBox.Show("输入格式有误");
                        return;
                    }
                    readingTemp = false;
                    Thread.Sleep(500);
                    if (flb_100.Set_DermacateDQY((float)realdqy))
                    {
                        Thread.Sleep(3000);
                        msg += "大气压校正成功";
                    }
                    else
                    {
                        Thread.Sleep(3000);
                        msg += "大气压校正成功";
                    }
                    Thread.Sleep(500);
                    readingTemp = true;
                    MessageBox.Show(msg, "提示");
                }
            }
            /*
            else if (equipconfig.TempInstrument == "XCE_100")// if(equipconfig.TempInstrument == "XCE_100"|| equipconfig.TempInstrument == "DWSP_T5" || equipconfig.TempInstrument == "FTH_2" || equipconfig.TempInstrument == "RZ_1")
            {

                double realwd, realsd, realdqy, equipwd, equipsd, equipdqy;
                try
                    {
                        realdqy = double.Parse(textBoxDqy.Text);
                        //realsd = double.Parse(textBoxSd.Text);
                        //realdqy = double.Parse(textBoxDqy.Text);
                    }
                    catch
                    {
                        MessageBox.Show("输入格式有误");
                        return;
                    }
                    readingTemp = false;
                    Thread.Sleep(500);
                    if (xce_100.demarcateHumidity((double)realdqy))
                    {
                        Thread.Sleep(3000);
                        msg += "大气压校正成功";
                    }
                    else
                    {
                        Thread.Sleep(3000);
                        msg += "大气压校正成功";
                    }
                    Thread.Sleep(500);
                    readingTemp = true;
                    MessageBox.Show(msg, "提示");
            }*/
            else// if(equipconfig.TempInstrument == "XCE_100"|| equipconfig.TempInstrument == "DWSP_T5" || equipconfig.TempInstrument == "FTH_2" || equipconfig.TempInstrument == "RZ_1")
            {
                double realwd, realsd, realdqy, equipwd, equipsd, equipdqy;
                try
                {
                    realwd = double.Parse(textBoxWd.Text);
                    realsd = double.Parse(textBoxSd.Text);
                    realdqy = double.Parse(textBoxDqy.Text);
                }
                catch
                {
                    MessageBox.Show("输入格式有误");
                    return;
                }                
                if (dqy != 0)
                {
                    thaxsdata.Airpxs = realdqy / dqy;
                    msg += "大气压校正成功";
                }
                else
                {
                    msg += "大气压校正失败";
                }
                configini.writeThaxsConfigIni(thaxsdata);
                MessageBox.Show(msg, "提示");
            }
        }
    }
}
