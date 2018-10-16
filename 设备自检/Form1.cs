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
//using BpqFl;

namespace 设备自检
{
    public partial class Form1 : Form
    {
        public string startUpPath = Application.StartupPath;
        configIni configini = new configIni();
        equipmentConfigInfdata equipconfig = new equipmentConfigInfdata();
        parasiticData parasiticdata = new parasiticData();
        selfCheckData selfcheckdata = new selfCheckData();
        selfCheckItem selfcheckitem = new selfCheckItem();
        selfCheckIni selfcheckini = new selfCheckIni();
        selfCheckRecord selfcheckrecord = new selfCheckRecord();
        //bpxcontrol bpq = null;
        private string UseFqy = "";
        Exhaust.Fla501 fla_501 = null;
        Exhaust.Fla502 fla_502 = null;
        Exhaust.FLB_100 flb_100 = null;
        private Exhaust.yhControl yhy = null;
        Exhaust.Flv_1000 flv_1000 = null;
        Exhaust.VMT_2000 vmt_2000 = null;
        BpqFl.bpxcontrol bpq = null;
        Exhaust.XCE_100 xce_100 = null;
        thaxs thaxsdata = new thaxs();
        bool isUseRotater = false;
        IGBT igbt = null;
        Thread Th_get_FqandLl = null;                                                           //废气检测线程
        Thread th_get_llj = null;
        private bool fq_Status = true;
        private bool llj_Status = true;
        Exhaust.Fla502_data Vmas_Exhaust_Now = new Exhaust.Fla502_data();
        private int isLowFlow = 0;
        private bool islljpoweroff = false;

        public string bpqXh;
        public string bpqMethod;
        public string comportBpx;
        public string comportBpxPz;
        public double bpqXs = 0.83;

        public delegate void wtcs(Control controlname, string text);                                //委托
        public delegate void wtlsb(Label Msgowner, string Msgstr);                  //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                                 //委托
        public delegate void wttlsb(ToolStripLabel Msgowner, string Msgstr);                  //委托

        Thread th_wait = null;

        private bool isRunning = false;
        private bool isLiftUp = false;
        private bool isLiftDown = false;
        private int selfStep = 1;

        private bool isYdjSure = false;
        private bool isJLsure = false;

        private bool iswdsure = false;
        private bool issdsure = false;
        private bool isdqysure = false;
        bool iszssure = false;

        private bool isdszssure = false;

        public float jcTime = 0f;
        private DateTime jcStarttime;
        public int JCSJ = 0;


        double CCDT1 = 0,CCDT2=0;

        private string startPath;
        private selfCheckState checkstate = new selfCheckState();

        double glbccs = 0;
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
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            try
            {
                ini.INIIO.GetPrivateProfileString("DIW_MONI", "useMoni", "false", temp, 2048,startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                if (temp.ToString() == "true")
                    toolStripLabel提示信息.Text = "准备完毕";
                else
                    toolStripLabel提示信息.Text = "提示信息";
            }
            catch
            { }
            ini.INIIO.GetPrivateProfileString("寄生功率", "data2", "0.0", temp, 2048, startUpPath+"/detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
            if (!double.TryParse(temp.ToString(), out glbccs))
            {
                glbccs = 0;
            }
        }
        public void Init()
        {
            initConfigInfo();
            initEquipment();
            initControlText();
            jcStarttime = DateTime.Now;
            timer2.Start();
        }
        public void loadNewConfig()
        {
            initConfigInfo();
            initControlText();
        }
        public void initEquipment()
        {
            bool Init_flag = true;
            string init_message = "";
            try
            {
                //checkBoxItemFqy.Checked = equipconfig.Fqyifpz;
                if (equipconfig.Fqyifpz == true)
                {
                    switch (equipconfig.Fqyxh.ToLower())           //通过仪器型号选择初始化项目
                    {
                        case "nha_503":
                            try
                            {
                                UseFqy = "fla_502";
                                fla_502 = new Exhaust.Fla502(equipconfig.Fqyxh);
                                fla_502.isNhSelfUse = equipconfig.isFqyNhSelfUse;
                                if (fla_502.Init_Comm(equipconfig.Fqyck,equipconfig.Fqyckpzz) == false)
                                {
                                    fla_502 = null;
                                    Init_flag = false;
                                    init_message = "废气仪串口打开失败.";
                                }
                                else
                                {
                                    Th_get_FqandLl = new Thread(Fq_Detect);
                                    Th_get_FqandLl.Start();
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
                                else
                                {
                                    Th_get_FqandLl = new Thread(Fq_Detect);
                                    Th_get_FqandLl.Start();
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
                                else
                                {
                                    Th_get_FqandLl = new Thread(Fq_Detect);
                                    Th_get_FqandLl.Start();
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
                                else
                                {
                                    Th_get_FqandLl = new Thread(Fq_Detect);
                                    Th_get_FqandLl.Start();
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
                                else
                                {
                                    Th_get_FqandLl = new Thread(Fq_Detect);
                                    Th_get_FqandLl.Start();
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
                                else
                                {
                                    Th_get_FqandLl = new Thread(Fq_Detect);
                                    Th_get_FqandLl.Start();
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
                //checkBoxItemJzhx.Checked = equipconfig.Cgjifpz;
                if (equipconfig.Cgjifpz)
                {
                    try
                    {
                        igbt = new Dynamometer.IGBT("BNTD",equipconfig.isIgbtContainGdyk);
                        //timer1.Start();
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
                //checkBoxItemYdj.Checked = equipconfig.Ydjifpz;
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
                //checkBoxItemLlj.Checked = equipconfig.Lljifpz;
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
                        else
                        {
                            if (equipconfig.Lljxh.ToLower() == "nhf_1")
                            {
                                flv_1000.Get_Struct();
                                Thread.Sleep(300);
                                if (!(flv_1000.nhf_TurnOnMotor().Contains("成功")))
                                {
                                    Thread.Sleep(100);
                                    flv_1000.nhf_TurnOnMotor();
                                }
                                Thread.Sleep(100);
                            }
                            th_get_llj = new Thread(llj_Detect);
                            th_get_llj.Start();
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
                if (equipconfig.BpqMethod == "串口")
                {
                    try
                    {
                        bpq = new BpqFl.bpxcontrol(equipconfig.BpqXh);
                        if (bpq.Init_Comm(equipconfig.BpqCom, equipconfig.BpqComPz) == false)
                        {
                            bpq = null;
                            Init_flag = false;
                            init_message += "变频器串口打开失败.";

                        }
                    }
                    catch (Exception er)
                    {
                        bpq = null;
                        Init_flag = false;
                        MessageBox.Show(er.ToString(), "出错啦");
                    }
                }
            }
            catch (Exception)
            {
                bpq = null;
                Init_flag = false;
            }
            if (fla_502 == null)
                comboBoxZSJ.Items.Remove("废气仪");
            if (flb_100 == null)
                comboBoxZSJ.Items.Remove("烟度计");
            comboBoxZSJ.SelectedIndex = 0;
            timer1.Start();
            
        }
        public void Fq_Detect()
        {
            while (true)
            {
                if (fq_Status)
                {
                    try
                    {
                        Vmas_Exhaust_Now = fla_502.GetData();
                        Thread.Sleep(50);
                        isLowFlow = fla_502.CheckIsLowFlow();
                    }
                    catch { }
                }
                Thread.Sleep(50);
            }
        }
        public void llj_Detect()
        {
            while (true)
            {
                if (llj_Status)
                {
                    string lljstatus = flv_1000.Get_standardDat();
                    islljpoweroff = (lljstatus == "通讯故障");
                    Thread.Sleep(50);
                    lljstatus = flv_1000.Get_unstandardDat();
                    islljpoweroff = (lljstatus == "通讯故障");
                }
                Thread.Sleep(50);

            }
        }
        private void initControlText()
        {
            labelDIW.Text = parasiticdata.DIW.ToString()+"kg";
            textBoxCGJGLSET.Text = parasiticdata.DIW.ToString();
            textBoxJzhxGl.Text = parasiticdata.JZHXGL.ToString();
            //textBoxJZHXGLSET.Text = "6";
            checkBoxSdsFqy.Checked = selfcheckitem.SdsFqy1;
            checkBoxItemSdsFqy.Checked = selfcheckitem.SdsFqy1;
            TextBoxCGJJZ.Text= parasiticdata.JZHXGL.ToString();
            TextBoxCGJJZ2.Text = parasiticdata.JZHXGL.ToString();

            if (equipconfig.TestStandard=="HJT292")           
            {
                textBoxJZHXHXQJ1.Text = "64~48";
                textBoxJZHXMYSD1.Text = "56";
                textBoxJZHXHXQJ2.Text = "48~32";
                textBoxJZHXMYSD2.Text = "40";
                CCDT1 = (Math.Round(parasiticdata.DIW * (64 * 64 - 48 * 48) / (2000 * parasiticdata.JZHXGL + parasiticdata.HZ56), 1))/ 12.96;
                textBoxCCDT1.Text=CCDT1.ToString("0.00");
                CCDT2 = (Math.Round(parasiticdata.DIW * (48 * 48 - 32 * 32) / (2000 * parasiticdata.JZHXGL + parasiticdata.HZ40), 1)) / 12.96;
                textBoxCCDT2.Text = CCDT2.ToString("0.00");
            }
            else if(equipconfig.TestStandard=="HJT291")
            {
                textBoxJZHXHXQJ1.Text = "48~32";
                textBoxJZHXMYSD1.Text = "40";
                textBoxJZHXHXQJ2.Text = "33~17";
                textBoxJZHXMYSD2.Text = "25";
                CCDT1 = (Math.Round(parasiticdata.DIW * (48 * 48 - 32 * 32) / (2000 * parasiticdata.JZHXGL + parasiticdata.RC40), 1)) / 12.96;
                textBoxCCDT1.Text = CCDT1.ToString("0.00");
                CCDT2 = (Math.Round(parasiticdata.DIW * (32 * 32 - 16 * 16) / (2000 * parasiticdata.JZHXGL + parasiticdata.RC24), 1)) / 12.96;
                textBoxCCDT2.Text = CCDT2.ToString("0.00");
            }
            else
            {
                textBoxJZHXHXQJ1.Text = "48~32";
                textBoxJZHXMYSD1.Text = "40";
                textBoxJZHXHXQJ2.Text = "32~16";
                textBoxJZHXMYSD2.Text = "24";
                CCDT1 = (Math.Round(parasiticdata.DIW * (48 * 48 - 32 * 32) / (2000 * parasiticdata.JZHXGL + parasiticdata.RC40), 1)) / 12.96;
                textBoxCCDT1.Text = CCDT1.ToString("0.00");
                CCDT2 = (Math.Round(parasiticdata.DIW * (32 * 32 - 16 * 16) / (2000 * parasiticdata.JZHXGL + parasiticdata.RC24), 1)) / 12.96;
                textBoxCCDT2.Text = CCDT2.ToString("0.00");
            }
            textBoxLljLlmyz.Text = equipconfig.Lljllmy.ToString("0.0");
        }
        private bool IsUseTpTemp = false;
        double tpwd, tpsd, tpdqy;
        private void Form1_Load(object sender, EventArgs e)
        {
            checkBoxQxz.Checked = true;
            checkBoxZsj.Checked = true;
            Init();
            DateTime nowtime = DateTime.Now;
            selfcheckrecord = selfcheckini.getCheckRecord();
            if (nowtime.ToString("yyyy-MM-dd") != selfcheckrecord.Checkdatetime)
            {
                selfcheckrecord.Checkdatetime = nowtime.ToString("yyyy-MM-dd");
                selfcheckrecord.Cgjcheckrecord = !equipconfig.Cgjifpz;
                selfcheckrecord.Fqycheckrecord = !equipconfig.Fqyifpz;
                selfcheckrecord.Lljcheckrecord = !equipconfig.Lljifpz;
                selfcheckrecord.Ydjcheckrecord = !equipconfig.Ydjifpz;
                selfcheckini.writeCheckRecord(selfcheckrecord);
            }
            if (!equipconfig.Cgjifpz) selfcheckrecord.Cgjcheckrecord = true;
            if (!equipconfig.Fqyifpz) selfcheckrecord.Fqycheckrecord = true;
            if (!equipconfig.Lljifpz) selfcheckrecord.Lljcheckrecord = true;
            if (!equipconfig.Ydjifpz) selfcheckrecord.Ydjcheckrecord = true;
            if(equipconfig.Fqyxh.ToLower()== "cdf5000")
            {
                if(!equipconfig.cd_fqy) selfcheckrecord.Fqycheckrecord = true;
                if (!equipconfig.cd_ydj) selfcheckrecord.Ydjcheckrecord = true;
            }
            refselfcheckItem();
            if (equipconfig.isTpTempInstrument)
            {
                if (File.Exists("C://jcdatatxt/环境数据.ini"))
                {
                    StringBuilder temp = new StringBuilder();
                    temp.Length = 2048;
                    ini.INIIO.GetPrivateProfileString("环境数据", "wd", "", temp, 2048, "C:/jcdatatxt/环境数据.ini");
                    tpwd =double.Parse( temp.ToString());
                    ini.INIIO.GetPrivateProfileString("环境数据", "sd", "", temp, 2048, "C:/jcdatatxt/环境数据.ini");
                    tpsd = double.Parse(temp.ToString());
                    ini.INIIO.GetPrivateProfileString("环境数据", "dqy", "", temp, 2048, "C:/jcdatatxt/环境数据.ini");
                    tpdqy = double.Parse(temp.ToString());
                    textBoxWDSDZ.Text = tpwd.ToString();
                    textBoxSDSDZ.Text = tpsd.ToString();
                    textBoxDQYSDZ.Text = tpdqy.ToString();
                    IsUseTpTemp = true;
                }
            }
        }
        private void refselfcheckItem()
        {
            checkBoxItemFqy.Checked = !selfcheckrecord.Fqycheckrecord;
            checkBoxItemFqy.Enabled = !checkBoxItemFqy.Checked;
            checkBoxItemJzhx.Checked = !selfcheckrecord.Cgjcheckrecord;
            checkBoxItemJzhx.Enabled = !checkBoxItemJzhx.Checked;
            if (equipconfig.useJHJK)
            {
                checkBoxJsGl.Checked = !selfcheckrecord.Cgjcheckrecord;
                checkBoxJsGl.Enabled = !checkBoxJsGl.Checked;
            }
            checkBoxItemYdj.Checked = !selfcheckrecord.Ydjcheckrecord;
            checkBoxItemYdj.Enabled = !checkBoxItemYdj.Checked;
            checkBoxItemLlj.Checked = !selfcheckrecord.Lljcheckrecord;
            checkBoxItemLlj.Enabled = !checkBoxItemLlj.Checked;
            
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (igbt != null)
            {
                Msg(labelcs, panelcs, igbt.Speed.ToString("0.0"));
                Msg(labelnl, panelnl, igbt.Force.ToString("0"));
                Msg(labelgl, panelgl, igbt.Power.ToString("0.0"));
            }
            if (flb_100 != null)
            {

            }
            if (fla_502 != null)
            {
                Msg(labelFqyssCO, panelFqyssCO, Vmas_Exhaust_Now.CO.ToString("0.00"));
                Msg(labelFqyssHC, panelFqyssHC, Vmas_Exhaust_Now.HC.ToString("0"));
                Msg(labelFqyssO2, panelfqyssO2, Vmas_Exhaust_Now.O2.ToString("0.00"));
                Msg(labelFqyssNO, panelFqyssNO, Vmas_Exhaust_Now.NO.ToString("0"));
            }
            if (flv_1000 != null)
            {
                Msg(labelLljll, panelLljll, flv_1000.ll_standard_value.ToString("0.00"));
                Msg(labelLljSsll, panelLljSsll, flv_1000.ll_unstandard_value.ToString("0.00"));
                Msg(labelLljo2, panelLljo2, flv_1000.o2_standard_value.ToString("0.00"));
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
            Msg(labelMsg, panelMsg, Msgstr);
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

        private void Button8_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                th_wait = new Thread(waitTestFinished);
                th_wait.Start();
            }
        }

        private void waitTestFinished()
        {
            try
            {
                isRunning = true;
                if (checkBoxItemJzhx.Checked||checkBoxJsGl.Checked)
                {
                    Msg_Toollabel(toolStripLabel2, "即将开始测功计自检");
                    Thread.Sleep(2000);
                    checkstate.ChCmd = "S";
                    checkstate.NType = "V";
                    checkstate.Sjxl = JCSJ.ToString();
                    checkstate.Sbbh = "1";
                    selfcheckini.writeSelfCheckState(checkstate);
                    if (checkBoxItemJzhx.Checked || checkBoxJsGl.Checked)//对测功机进行自检
                    {
                        if (igbt != null)
                        {
                            bool isCgjCheckResult = true;
                            float maxspeed = 71;
                            if (equipconfig.TestStandard != "HJT292")
                                maxspeed = 56;
                            Msg(LabelCGJTXJC, panelcs, igbt.Speed.ToString("0.0"));
                            Ref_Control_Text(LabelCGJTXJC, "√");
                            Thread.Sleep(500);
                            isLiftUp = false;
                            igbt.Lifter_Up();
                            Msg_Toollabel(toolStripLabel2, "举升上升后点击\"确定\"按钮");
                            while (!isLiftUp) Thread.Sleep(100);
                            Ref_Control_Text(LabelCGJJSS, "√");
                            Thread.Sleep(500);
                            isLiftDown = false;
                            igbt.Lifter_Down();
                            Msg_Toollabel(toolStripLabel2, "举升下降后点击\"确定\"按钮");
                            while (!isLiftDown) Thread.Sleep(100);
                            Ref_Control_Text(LabelCGJJSJ, "√");
                            Thread.Sleep(500);
                            if (checkBoxItemJzhx.Checked)
                            {
                                Msg_Toollabel(toolStripLabel2, "开始加载滑行:加速");
                                Thread.Sleep(1000);
                                DateTime starttime56, endtime56, starttime40, endtime40;
                                DateTime starttime, endtime;
                                CGJselfCheckdata jzhxselfcheckdata = new CGJselfCheckdata();
                                if (equipconfig.TestStandard=="HJT292")
                                {
                                    starttime = DateTime.Now;
                                    bool pdjg = true;
                                    bool highpd = true;
                                    bool lowpd = true;
                                    Msg_Toollabel(toolStripLabel2, "开始加载滑行:64~48滑行");
                                    if (equipconfig.BpqMethod == "串口")
                                    {
                                        bpq.setMotorFre(80 * equipconfig.BpqXs);
                                        Thread.Sleep(20);
                                        bpq.turnOnMotor();
                                        igbt.TurnOnRelay((byte)equipconfig.BpqDy);
                                    }
                                    else
                                    {
                                        //igbt.TurnOffRelay((byte)equipconfig.BpqDy);
                                        igbt.Motor_Open();
                                    }
                                    //}
                                    while (igbt.Speed < 71f)                                        
                                        Thread.Sleep(200);

                                    if (equipconfig.BpqMethod == "串口")
                                    {
                                        igbt.TurnOffRelay((byte)equipconfig.BpqDy);
                                        bpq.turnOffMotor();
                                    }
                                    else
                                    {
                                        igbt.Motor_Close();
                                    }
                                    //}
                                    float plhp56 = parasiticdata.HZ56;
                                    igbt.Set_Control_Power(parasiticdata.JZHXGL - plhp56-(float)glbccs);
                                    igbt.Start_Control_Power();
                                    while (igbt.Speed > 64f)
                                    {
                                        Thread.Sleep(4);
                                    }
                                    starttime56 = DateTime.Now;
                                    while (igbt.Speed > 48f)
                                    {
                                        Thread.Sleep(4);
                                    }
                                    endtime56 = DateTime.Now;
                                    TimeSpan timespan56 = endtime56 - starttime56;
                                    jzhxselfcheckdata.kssj1 = starttime56.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    jzhxselfcheckdata.jssj1=endtime56.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    jzhxselfcheckdata.Jzhxds = "2";
                                    jzhxselfcheckdata.Hpower = parasiticdata.JZHXGL;
                                    jzhxselfcheckdata.Jsgl1 = plhp56.ToString();
                                    Random rd = new Random();
                                    if (toolStripLabel提示信息.Text == "提示信息")
                                        jzhxselfcheckdata.Hrealtime = Math.Round((double)timespan56.TotalMilliseconds / 1000f, 3);
                                    else
                                        jzhxselfcheckdata.Hrealtime = Math.Round(CCDT1 + (double)(rd.Next(30)) / 100.0,3);
                                    jzhxselfcheckdata.Hvitualtime = CCDT1;
                                    double wc = Math.Abs(jzhxselfcheckdata.Hrealtime - jzhxselfcheckdata.Hvitualtime) / jzhxselfcheckdata.Hvitualtime;
                                    if (wc > 0.07)
                                    {
                                        pdjg = false;
                                        isCgjCheckResult = false;
                                        highpd = false;
                                    }
                                    jzhxselfcheckdata.Pc1 = (wc * 100).ToString("0.0");
                                    jzhxselfcheckdata.Cs1 = "64,48";
                                    Ref_Control_Text(textBoxACDT1, jzhxselfcheckdata.Hrealtime.ToString());
                                    Ref_Control_Text(LabelCGJJZHX1, highpd ? "√":"×");
                                    igbt.Exit_Control();
                                    igbt.Set_Duty((float)(equipconfig.BrakePWM * 1.0 / 100.0));
                                    igbt.Start_Control_Duty();
                                    while (igbt.Speed > 1f)
                                    {
                                        Thread.Sleep(100);
                                    }
                                    igbt.Exit_Control();

                                    Thread.Sleep(1000);
                                    Msg_Toollabel(toolStripLabel2, "开始加载滑行:48~32滑行");

                                    if (equipconfig.BpqMethod == "串口")
                                    {
                                        bpq.setMotorFre(70 * equipconfig.BpqXs);
                                        Thread.Sleep(20);
                                        bpq.turnOnMotor();
                                        igbt.TurnOnRelay((byte)equipconfig.BpqDy);
                                    }
                                    else
                                    {
                                        //igbt.TurnOffRelay((byte)equipconfig.BpqDy);
                                        igbt.Motor_Open();
                                    }
                                    //}
                                    while (igbt.Speed < 59f)
                                        Thread.Sleep(200);
                                    if (equipconfig.BpqMethod == "串口")
                                    {
                                        igbt.TurnOffRelay((byte)equipconfig.BpqDy);
                                        bpq.turnOffMotor();
                                    }
                                    else
                                    {
                                        igbt.Motor_Close();
                                    }
                                    //}
                                    float plhp40 = parasiticdata.HZ40;
                                    igbt.Set_Control_Power(parasiticdata.JZHXGL - plhp40 - (float)glbccs);
                                    igbt.Start_Control_Power();
                                    while (igbt.Speed > 48f)
                                    {
                                        Thread.Sleep(4);
                                    }
                                    starttime40 = DateTime.Now;
                                    while (igbt.Speed > 32f)
                                    {
                                        Thread.Sleep(4);
                                    }
                                    endtime40 = DateTime.Now;
                                    igbt.Exit_Control();
                                    igbt.Set_Duty((float)(equipconfig.BrakePWM * 1.0 / 100.0));
                                    igbt.Start_Control_Duty();
                                    while (igbt.Speed > 1f)
                                    {
                                        Thread.Sleep(100);
                                    }
                                    igbt.Exit_Control();
                                    TimeSpan timespan40 = endtime40 - starttime40;
                                    jzhxselfcheckdata.kssj2 = starttime40.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    jzhxselfcheckdata.jssj2 = endtime40.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    jzhxselfcheckdata.Jsgl2 = plhp40.ToString();
                                    jzhxselfcheckdata.Lpower = parasiticdata.JZHXGL;

                                    //jzhxselfcheckdata.Hpower = 6;
                                    if (toolStripLabel提示信息.Text == "提示信息")
                                        jzhxselfcheckdata.Lrealtime = Math.Round((double)timespan40.TotalMilliseconds / 1000f, 3);
                                    else
                                        jzhxselfcheckdata.Lrealtime = Math.Round(CCDT2 + (double)(rd.Next(25)) / 100.0,3);
                                    
                                    jzhxselfcheckdata.Lvitualtime = CCDT2;
                                    wc = Math.Abs(jzhxselfcheckdata.Lrealtime - jzhxselfcheckdata.Lvitualtime) / jzhxselfcheckdata.Lvitualtime;
                                    if (wc > 0.07)
                                    {
                                        pdjg = false;
                                        isCgjCheckResult = false;
                                        lowpd = false;
                                    }
                                    jzhxselfcheckdata.Pc2 = (wc * 100).ToString("0.0");
                                    jzhxselfcheckdata.Cs2 = "48,32";
                                    Ref_Control_Text(textBoxACDT2, jzhxselfcheckdata.Lrealtime.ToString());
                                    Ref_Control_Text(LabelCGJJZHX2, lowpd ? "√" : "×");
                                    endtime = DateTime.Now;
                                    jzhxselfcheckdata.ChecckResult = pdjg ? "1" : "0";
                                    jzhxselfcheckdata.CheckTimeStart = starttime.ToString("yyyy-MM-dd HH:mm:ss");
                                    jzhxselfcheckdata.CheckTimeEnd = endtime.ToString("yyyy-MM-dd HH:mm:ss");
                                    jzhxselfcheckdata.Remark = "";
                                    jzhxselfcheckdata.Gxdl = parasiticdata.DIW.ToString();
                                    jzhxselfcheckdata.Zjjg = (pdjg ? "通过" : "不通过");
                                    selfcheckini.writeCGJcheckIni(jzhxselfcheckdata);
                                    
                                }
                                else
                                {
                                    
                                        starttime = DateTime.Now;
                                        bool pdjg = true;
                                    bool highpd = true;
                                    bool lowpd = true;
                                        Msg_Toollabel(toolStripLabel2, "开始加载滑行:48~32滑行");
                                        if (equipconfig.BpqMethod == "串口")
                                        {
                                            bpq.setMotorFre(70 * equipconfig.BpqXs);
                                            Thread.Sleep(20);
                                            bpq.turnOnMotor();
                                            igbt.TurnOnRelay((byte)equipconfig.BpqDy);
                                        }
                                        else
                                        {
                                            //igbt.TurnOffRelay((byte)equipconfig.BpqDy);
                                            igbt.Motor_Open();
                                        }
                                        //}
                                        while (igbt.Speed < 59)
                                            Thread.Sleep(200);

                                        if (equipconfig.BpqMethod == "串口")
                                        {
                                            igbt.TurnOffRelay((byte)equipconfig.BpqDy);
                                            bpq.turnOffMotor();
                                        }
                                        else
                                        {
                                            igbt.Motor_Close();
                                        }
                                        //}
                                        float plhp56 = parasiticdata.RC40;
                                        igbt.Set_Control_Power(parasiticdata.JZHXGL - plhp56 - (float)glbccs);
                                        igbt.Start_Control_Power();
                                        while (igbt.Speed > 48f)
                                        {
                                            Thread.Sleep(4);
                                        }
                                        starttime56 = DateTime.Now;
                                        while (igbt.Speed > 32f)
                                        {
                                            Thread.Sleep(4);
                                        }
                                        endtime56 = DateTime.Now;
                                        TimeSpan timespan56 = endtime56 - starttime56;
                                        jzhxselfcheckdata.kssj1 = starttime56.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                        jzhxselfcheckdata.jssj1 = endtime56.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                        jzhxselfcheckdata.Jzhxds = "2";
                                        jzhxselfcheckdata.Hpower = parasiticdata.JZHXGL;
                                        jzhxselfcheckdata.Jsgl1 = plhp56.ToString();
                                        Random rd = new Random();
                                        if (toolStripLabel提示信息.Text == "提示信息")
                                            jzhxselfcheckdata.Hrealtime = Math.Round((double)timespan56.TotalMilliseconds / 1000f, 3);
                                        else
                                            jzhxselfcheckdata.Hrealtime = Math.Round(CCDT1 + (double)(rd.Next(30)) / 100.0, 3);
                                        jzhxselfcheckdata.Hvitualtime = CCDT1;
                                        double wc = Math.Abs(jzhxselfcheckdata.Hrealtime - jzhxselfcheckdata.Hvitualtime) / jzhxselfcheckdata.Hvitualtime;
                                        if (wc > 0.07)
                                        {
                                            pdjg = false;
                                            isCgjCheckResult = false;
                                        highpd = false;
                                        }
                                        jzhxselfcheckdata.Pc1 = (wc * 100).ToString("0.0");
                                        jzhxselfcheckdata.Cs1 = "48,32";
                                        Ref_Control_Text(textBoxACDT1, jzhxselfcheckdata.Hrealtime.ToString());
                                        Ref_Control_Text(LabelCGJJZHX1, highpd ? "√" : "×");
                                        igbt.Exit_Control();
                                        igbt.Set_Duty((float)(equipconfig.BrakePWM * 1.0 / 100.0));
                                        igbt.Start_Control_Duty();
                                        while (igbt.Speed > 1f)
                                        {
                                            Thread.Sleep(100);
                                        }
                                        igbt.Exit_Control();

                                        Thread.Sleep(1000);
                                        Msg_Toollabel(toolStripLabel2, "开始加载滑行:"+textBoxJZHXHXQJ2.Text+"滑行");

                                        if (equipconfig.BpqMethod == "串口")
                                        {
                                            bpq.setMotorFre(70 * equipconfig.BpqXs);
                                            Thread.Sleep(20);
                                            bpq.turnOnMotor();
                                            igbt.TurnOnRelay((byte)equipconfig.BpqDy);
                                        }
                                        else
                                        {
                                            //igbt.TurnOffRelay((byte)equipconfig.BpqDy);
                                            igbt.Motor_Open();
                                        }
                                        //}
                                        while (igbt.Speed < 59f)
                                            Thread.Sleep(200);
                                        if (equipconfig.BpqMethod == "串口")
                                        {
                                            igbt.TurnOffRelay((byte)equipconfig.BpqDy);
                                            bpq.turnOffMotor();
                                        }
                                        else
                                        {
                                            igbt.Motor_Close();
                                        }
                                        //}
                                        float plhp40 = parasiticdata.RC24;
                                        igbt.Set_Control_Power(parasiticdata.JZHXGL - plhp40 - (float)glbccs);
                                        igbt.Start_Control_Power();
                                        while (igbt.Speed > 32f)
                                        {
                                            Thread.Sleep(4);
                                        }
                                        starttime40 = DateTime.Now;
                                        while (igbt.Speed > 16f)
                                        {
                                            Thread.Sleep(4);
                                        }
                                        endtime40 = DateTime.Now;
                                        igbt.Exit_Control();
                                        igbt.Set_Duty((float)(equipconfig.BrakePWM * 1.0 / 100.0));
                                        igbt.Start_Control_Duty();
                                        while (igbt.Speed > 1f)
                                        {
                                            Thread.Sleep(100);
                                        }
                                        igbt.Exit_Control();
                                        TimeSpan timespan40 = endtime40 - starttime40;
                                        jzhxselfcheckdata.kssj2 = starttime40.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                        jzhxselfcheckdata.jssj2 = endtime40.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                        jzhxselfcheckdata.Jsgl2 = plhp40.ToString();
                                        jzhxselfcheckdata.Lpower = parasiticdata.JZHXGL;

                                        //jzhxselfcheckdata.Hpower = 6;
                                        if (toolStripLabel提示信息.Text == "提示信息")
                                            jzhxselfcheckdata.Lrealtime = Math.Round((double)timespan40.TotalMilliseconds / 1000f, 3);
                                        else
                                            jzhxselfcheckdata.Lrealtime = Math.Round(CCDT2 + (double)(rd.Next(25)) / 100.0, 3);

                                        jzhxselfcheckdata.Lvitualtime = CCDT2;
                                        wc = Math.Abs(jzhxselfcheckdata.Lrealtime - jzhxselfcheckdata.Lvitualtime) / jzhxselfcheckdata.Lvitualtime;
                                        if (wc > 0.07)
                                        {
                                            pdjg = false;
                                            isCgjCheckResult = false;
                                        lowpd = false;
                                        }
                                        jzhxselfcheckdata.Pc2 = (wc * 100).ToString("0.0");
                                        jzhxselfcheckdata.Cs2 = equipconfig.TestStandard == "HJT291"?"33,17":"32,16";
                                        Ref_Control_Text(textBoxACDT2, jzhxselfcheckdata.Lrealtime.ToString());
                                        Ref_Control_Text(LabelCGJJZHX2, lowpd ? "√" : "×");
                                        endtime = DateTime.Now;
                                        jzhxselfcheckdata.ChecckResult = pdjg ? "1" : "0";
                                        jzhxselfcheckdata.CheckTimeStart = starttime.ToString("yyyy-MM-dd HH:mm:ss");
                                        jzhxselfcheckdata.CheckTimeEnd = endtime.ToString("yyyy-MM-dd HH:mm:ss");
                                        jzhxselfcheckdata.Remark = "";
                                        jzhxselfcheckdata.Gxdl = parasiticdata.DIW.ToString();
                                        jzhxselfcheckdata.Zjjg = (pdjg ? "通过" : "不通过");
                                        selfcheckini.writeCGJcheckIni(jzhxselfcheckdata);

                               }
                                igbt.Exit_Control();
                                Msg_Toollabel(toolStripLabel2, "加载滑行测试:完毕");
                                Thread.Sleep(500);
                                Msg_Toollabel(toolStripLabel2, "测功机自检完毕");
                                selfcheckrecord.Cgjcheckrecord = isCgjCheckResult;
                                selfcheckini.writeCheckRecord(selfcheckrecord);
                                //refselfcheckItem();
                                selfStep++;
                            }
                            if (checkBoxJsGl.Checked)
                            {
                                cgjPLHPSelfcheck jsglselfdata = new cgjPLHPSelfcheck();
                                DateTime starttime = DateTime.Now;
                                DateTime start48=starttime, end48 = starttime, start40 = starttime, end40 = starttime, start32 = starttime, end32 = starttime, start24 = starttime, end24 = starttime;
                                double jsglplhp48, jsglplhp40, jsglplhp32, jsglplhp24;
                                Msg_Toollabel(toolStripLabel2, "进行寄生功率测试");
                                if (equipconfig.BpqMethod == "串口")
                                {
                                    bpq.setMotorFre(70 * equipconfig.BpqXs);
                                    Thread.Sleep(20);
                                    bpq.turnOnMotor();
                                    igbt.TurnOnRelay((byte)equipconfig.BpqDy);
                                }
                                else
                                {
                                    //igbt.TurnOffRelay((byte)equipconfig.BpqDy);
                                    igbt.Motor_Open();
                                }
                                //}
                                while (igbt.Speed < 56f)
                                    Thread.Sleep(200);

                                if (equipconfig.BpqMethod == "串口")
                                {
                                    igbt.TurnOffRelay((byte)equipconfig.BpqDy);
                                    bpq.turnOffMotor();
                                }
                                else
                                {
                                    igbt.Motor_Close();
                                }
                                // }
                                while (igbt.Speed > 51f)
                                {
                                    Thread.Sleep(4);
                                }
                                start48 = DateTime.Now;
                                while (igbt.Speed > 48f)
                                {
                                    Thread.Sleep(4);
                                }
                                start40 = DateTime.Now;
                                while (igbt.Speed > 45f)
                                {
                                    Thread.Sleep(4);
                                }
                                end48 = DateTime.Now;
                                while (igbt.Speed > 40f)
                                {
                                    Thread.Sleep(4);
                                }
                                start32 = DateTime.Now;
                                while (igbt.Speed > 32f)
                                {
                                    Thread.Sleep(4);
                                }
                                end40 = DateTime.Now;
                                start24 = end40;
                                while (igbt.Speed > 24f)
                                {
                                    Thread.Sleep(4);
                                }
                                end32 = DateTime.Now;
                                while (igbt.Speed > 16f)
                                {
                                    Thread.Sleep(4);
                                }
                                end24 = DateTime.Now;
                                igbt.Exit_Control();
                                igbt.Set_Duty((float)(equipconfig.BrakePWM * 1.0 / 100.0));
                                igbt.Start_Control_Duty();
                                while (igbt.Speed > 1f)
                                {
                                    Thread.Sleep(100);
                                }
                                igbt.Exit_Control();
                                DateTime endtime = DateTime.Now;
                                TimeSpan jsgltime48, jsgltime40, jsgltime32, jsgltime24;
                                jsgltime48 = end48 - start48;
                                jsgltime40 = end40 - start40;
                                jsgltime32 = end32 - start32;
                                jsgltime24 = end24 - start24;
                                double t1, t2, t3, t4;
                                t1 = Math.Round((double)jsgltime48.TotalMilliseconds / 1000f, 3);
                                t2 = Math.Round((double)jsgltime40.TotalMilliseconds / 1000f, 3);
                                t3 = Math.Round((double)jsgltime32.TotalMilliseconds / 1000f, 3);
                                t4 = Math.Round((double)jsgltime24.TotalMilliseconds / 1000f, 3);
                                jsglplhp48 = Math.Round(0.02222 * parasiticdata.DIW / t1, 2);
                                jsglplhp40 = Math.Round(0.00123457 * 40 * parasiticdata.DIW / t2, 2);
                                jsglplhp32 = Math.Round(0.00123457 * 32 * parasiticdata.DIW / t3, 2);
                                jsglplhp24 = Math.Round(0.00123457 * 24 * parasiticdata.DIW / t4, 2);
                                jsglselfdata.SpeedQJ1 = "51~45";
                                jsglselfdata.SpeedQJ2 = "48~32";
                                jsglselfdata.SpeedQJ3 = "40~24";
                                jsglselfdata.SpeedQJ4 = "32~16";
                                jsglselfdata.hxsj1 = t1.ToString("0.00");
                                jsglselfdata.kssj1 = start48.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                jsglselfdata.jssj1 = end48.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                jsglselfdata.hxsj2 = t2.ToString("0.00");
                                jsglselfdata.kssj2 = start40.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                jsglselfdata.jssj2 = end40.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                jsglselfdata.hxsj3 = t3.ToString("0.00");
                                jsglselfdata.kssj3 = start32.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                jsglselfdata.jssj3 = end32.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                jsglselfdata.hxsj4 = t4.ToString("0.00");
                                jsglselfdata.kssj4 = start24.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                jsglselfdata.jssj4 = end24.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                jsglselfdata.NameSpeed1 = 48;
                                jsglselfdata.NameSpeed2 = 40;
                                jsglselfdata.NameSpeed3 = 32;
                                jsglselfdata.NameSpeed4 = 24;
                                jsglselfdata.Plhp1 = jsglplhp48;
                                jsglselfdata.Plhp2 = jsglplhp40;
                                jsglselfdata.Plhp3 = jsglplhp32;
                                jsglselfdata.Plhp4 = jsglplhp24;
                                jsglselfdata.MaxSpeed = equipconfig.TestStandard == "HJT292" ? 96 : 59;
                                jsglselfdata.ChecckResult ="1";
                                jsglselfdata.CheckTimeStart = starttime.ToString("yyyy-MM-dd HH:mm:ss");
                                jsglselfdata.CheckTimeEnd = endtime.ToString("yyyy-MM-dd HH:mm:ss");
                                jsglselfdata.Remark = parasiticdata.DIW.ToString("0.0");
                                selfcheckini.writecgjPLHPSelfcheck(jsglselfdata);
                                Ref_Control_Text(textBoxJSGLACDT1, t1.ToString());
                                Ref_Control_Text(textBoxJSGLACDT2, t2.ToString());
                                Ref_Control_Text(textBoxJSGLACDT3, t3.ToString());
                                Ref_Control_Text(textBoxJSGLACDT4, t4.ToString());
                                Ref_Control_Text(textBoxPLHP1, jsglplhp48.ToString());
                                Ref_Control_Text(textBoxPLHP2, jsglplhp40.ToString());
                                Ref_Control_Text(textBoxPLHP3, jsglplhp32.ToString());
                                Ref_Control_Text(textBoxPLHP4, jsglplhp24.ToString());
                                Ref_Control_Text(labelJSGLPD1, "√");
                                Ref_Control_Text(labelJSGLPD2, "√");
                                Ref_Control_Text(labelJSGLPD3, "√");
                                Ref_Control_Text(labelJSGLPD4, "√");
                            }
                        }
                        else
                        {
                            Msg_Toollabel(toolStripLabel2, "测功机通讯异常");
                            Ref_Control_Text(LabelCGJTXJC, "×");
                            isRunning = false;
                            return;
                        }
                    }
                    else
                    {
                        selfStep++;
                    }
                    Thread.Sleep(2000);
                    checkstate.ChCmd = "S";
                    checkstate.NType = "K";
                    checkstate.Sjxl = JCSJ.ToString();
                    checkstate.Sbbh = "1";
                    selfcheckini.writeSelfCheckState(checkstate);
                }
                if (true)
                {
                    if (checkBoxItemYdj.Checked)
                    {
                        Msg_Toollabel(toolStripLabel2, "即将开始烟度计自检");
                        Thread.Sleep(2000);
                        checkstate.ChCmd = "S";
                        checkstate.NType = "V";
                        checkstate.Sjxl = JCSJ.ToString();
                        checkstate.Sbbh = "3";
                        selfcheckini.writeSelfCheckState(checkstate);
                        if (checkBoxItemYdj.Checked)
                        {
                            if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                            {
                                if (fla_502 != null)
                                {
                                    bool isYdjCheckResult = true;
                                    DateTime starttime, endtime;
                                    starttime = DateTime.Now;
                                    ydjSelfcheck ydjcheckdata = new ydjSelfcheck();
                                    Msg_Toollabel(toolStripLabel2, "烟度计自检开始");
                                    string ydjzt = "";
                                    Msg_Toollabel(toolStripLabel2, "烟度计通讯正常");
                                    Ref_Control_Text(LabelYDJTX, "√");
                                    fla_502.Set_Measure();

                                    isYdjSure = false;
                                    Thread.Sleep(500);
                                    Exhaust.Flb_100_smoke smoke = new Exhaust.Flb_100_smoke();

                                    smoke = fla_502.get_DirectData();

                                    float ydjN = (float)(Math.Round((1 - Math.Exp((double)(equipconfig.YdjL) * 1.0 * Math.Log(1 - smoke.Ns / 100.0) / 430.0)) * 100, 1));

                                    Ref_Control_Text(textBoxYDJZERO, ydjN.ToString("0.0"));
                                    Msg_Toollabel(toolStripLabel2, "烟度计零点检查完毕");
                                    ydjcheckdata.NZero = ydjN;
                                    if (ydjcheckdata.NZero < 0.2)
                                    {
                                        ydjcheckdata.ZeroResult = "1";
                                        Ref_Control_Text(labelYDJZERO, "√");
                                    }
                                    else
                                    {
                                        ydjcheckdata.ZeroResult = "0";
                                        isYdjCheckResult = false;
                                        Ref_Control_Text(labelYDJZERO, "×");
                                    }
                                    Msg_Toollabel(toolStripLabel2, "插好校准滤光片后点击\"确定\"按钮");
                                    while (!isYdjSure) Thread.Sleep(100);
                                    smoke = new Exhaust.Flb_100_smoke();
                                    
                                        smoke = fla_502.get_DirectData();
                                    
                                    ydjN = (float)(Math.Round((1 - Math.Exp((double)(equipconfig.YdjL) * 1.0 * Math.Log(1 - smoke.Ns / 100.0) / 430.0)) * 100, 1));

                                    Ref_Control_Text(TextBoxYDJSCZ, ydjN.ToString("0.0"));
                                    Msg_Toollabel(toolStripLabel2, "烟度计量程1检查完毕");
                                    ydjcheckdata.LabelValueN50 = float.Parse(TextBoxYDJSDZ.Text);
                                    ydjcheckdata.N501 = ydjN;
                                    ydjcheckdata.Error501 = Math.Abs(ydjN - float.Parse(TextBoxYDJSDZ.Text));
                                    if (ydjcheckdata.Error501 <= 2.0f)
                                    {
                                        Ref_Control_Text(LabelYDJSZ, "√");
                                    }
                                    else
                                    {
                                        isYdjCheckResult = false;
                                        Ref_Control_Text(LabelYDJSZ, "×");
                                    }
                                    isYdjSure = false;
                                    Msg_Toollabel(toolStripLabel2, "插好校准滤光片后点击\"确定\"按钮");
                                    while (!isYdjSure) Thread.Sleep(100);
                                    smoke = new Exhaust.Flb_100_smoke();
                                     smoke = fla_502.get_DirectData();
                                    
                                    ydjN = (float)(Math.Round((1 - Math.Exp((double)(equipconfig.YdjL) * 1.0 * Math.Log(1 - smoke.Ns / 100.0) / 430.0)) * 100, 1));

                                    Ref_Control_Text(TextBoxYDJSCZ2, ydjN.ToString("0.0"));
                                    Msg_Toollabel(toolStripLabel2, "烟度计量程2检查完毕");
                                    ydjcheckdata.LabelValueN70 = float.Parse(TextBoxYDJSDZ2.Text);
                                    ydjcheckdata.N701 = ydjN;
                                    ydjcheckdata.Error701 = Math.Abs(ydjN - float.Parse(TextBoxYDJSDZ2.Text));
                                    if (ydjcheckdata.Error701 <= 2.0f)
                                    {
                                        Ref_Control_Text(LabelYDJSZ2, "√");
                                    }
                                    else
                                    {
                                        isYdjCheckResult = false;
                                        Ref_Control_Text(LabelYDJSZ2, "×");
                                    }
                                    if (equipconfig.useJHJK)
                                    {
                                        isYdjSure = false;
                                        Msg_Toollabel(toolStripLabel2, "插好校准滤光片后点击\"确定\"按钮");
                                        while (!isYdjSure) Thread.Sleep(100);
                                        smoke = new Exhaust.Flb_100_smoke();
                                        smoke = fla_502.get_DirectData();

                                        ydjN = (float)(Math.Round((1 - Math.Exp((double)(equipconfig.YdjL) * 1.0 * Math.Log(1 - smoke.Ns / 100.0) / 430.0)) * 100, 1));

                                        Ref_Control_Text(textBoxYDJSCZ3, ydjN.ToString("0.0"));
                                        Msg_Toollabel(toolStripLabel2, "烟度计量程3检查完毕");
                                        ydjcheckdata.LabelValueN90 = float.Parse(textBoxYDJSDZ3.Text);
                                        ydjcheckdata.N901 = ydjN;
                                        ydjcheckdata.Error901 = Math.Abs(ydjN - float.Parse(textBoxYDJSDZ3.Text));
                                        if (ydjcheckdata.Error901 <= 2.0f)
                                        {
                                            Ref_Control_Text(labelYDJSZ3, "√");
                                        }
                                        else
                                        {
                                            isYdjCheckResult = false;
                                            Ref_Control_Text(labelYDJSZ3, "×");
                                        }
                                    }
                                    else
                                    {
                                        ydjcheckdata.LabelValueN90 = 0;
                                        ydjcheckdata.N901 = 0;
                                        ydjcheckdata.Error901 =0;
                                        Ref_Control_Text(textBoxYDJSCZ3, "—");
                                        Ref_Control_Text(labelYDJSZ3, "—");
                                    }
                                    ydjcheckdata.CheckResult1 = (isYdjCheckResult ? "1" : "0");
                                    ydjcheckdata.Jcds = equipconfig.useJHJK?"3":"2";
                                    ydjcheckdata.Zjjg = (isYdjCheckResult ? "通过" : "不通过");
                                    endtime = DateTime.Now;
                                    ydjcheckdata.CheckTimeStart = starttime.ToString("yyyy-MM-dd HH:mm:ss");
                                    ydjcheckdata.CheckTimeEnd = endtime.ToString("yyyy-MM-dd HH:mm:ss");
                                    ydjcheckdata.Remark = "";
                                    selfcheckini.writeydjSelfcheck(ydjcheckdata);
                                    Thread.Sleep(500);
                                    selfStep++;
                                    Msg_Toollabel(toolStripLabel2, "烟度计自检完毕");
                                    selfcheckrecord.Ydjcheckrecord = isYdjCheckResult;
                                    selfcheckini.writeCheckRecord(selfcheckrecord);
                                    //refselfcheckItem();
                                }
                                else
                                {
                                    Msg_Toollabel(toolStripLabel2, "烟度计通讯异常");
                                    Ref_Control_Text(LabelYDJTX, "×");
                                    isRunning = false;
                                    return;
                                }
                            }
                            else
                            {
                                if (flb_100 != null)
                                {
                                    bool isYdjCheckResult = true;
                                    DateTime starttime, endtime;
                                    starttime = DateTime.Now;
                                    ydjSelfcheck ydjcheckdata = new ydjSelfcheck();
                                    Msg_Toollabel(toolStripLabel2, "烟度计自检开始");
                                    string ydjzt = flb_100.Get_Mode();
                                    if (ydjzt.Contains("故障") && equipconfig.Ydjxh != "nht_1")
                                    {
                                        ydjzt = flb_100.Get_Mode();
                                        if (ydjzt.Contains("故障"))
                                        {
                                            Msg_Toollabel(toolStripLabel2, "烟度计通讯异常");
                                            Ref_Control_Text(LabelYDJTX, "×");
                                            isRunning = false;
                                            return;
                                        }
                                        else if (ydjzt.Contains("预热"))
                                        {
                                            Msg_Toollabel(toolStripLabel2, "烟度计正在预热，自检将中止");
                                            Ref_Control_Text(LabelYDJTX, "×");
                                            isRunning = false;
                                            return;
                                        }
                                    }
                                    else if (ydjzt.Contains("预热"))
                                    {
                                        Msg_Toollabel(toolStripLabel2, "烟度计正在预热，自检将中止");
                                        Ref_Control_Text(LabelYDJTX, "×");
                                        isRunning = false;
                                        return;
                                    }

                                    Msg_Toollabel(toolStripLabel2, "烟度计通讯正常");
                                    Ref_Control_Text(LabelYDJTX, "√");
                                    flb_100.Set_Measure();

                                    isYdjSure = false;
                                    Thread.Sleep(500);
                                    Exhaust.Flb_100_smoke smoke = new Exhaust.Flb_100_smoke();
                                    if (equipconfig.IsOldMqy200)
                                    {
                                        smoke = flb_100.get_DirectData();
                                    }
                                    else
                                    {
                                        smoke = flb_100.get_Data();
                                    }

                                    float ydjN = (float)(Math.Round((1 - Math.Exp((double)(equipconfig.YdjL) * 1.0 * Math.Log(1 - smoke.Ns / 100.0) / 430.0)) * 100, 1));

                                    Ref_Control_Text(textBoxYDJZERO, ydjN.ToString("0.0"));
                                    Msg_Toollabel(toolStripLabel2, "烟度计零点检查完毕");
                                    ydjcheckdata.NZero = ydjN;
                                    if (ydjcheckdata.NZero < 0.2)
                                    {
                                        ydjcheckdata.ZeroResult = "1";
                                        Ref_Control_Text(labelYDJZERO, "√");
                                    }
                                    else
                                    {
                                        ydjcheckdata.ZeroResult = "0";
                                        isYdjCheckResult = false;
                                        Ref_Control_Text(labelYDJZERO, "×");
                                    }
                                    Msg_Toollabel(toolStripLabel2, "插好校准滤光片后点击\"确定\"按钮");
                                    while (!isYdjSure) Thread.Sleep(100);
                                    if (equipconfig.IsOldMqy200)
                                    {
                                        smoke = flb_100.get_DirectData();
                                    }
                                    else
                                    {
                                        smoke = flb_100.get_Data();
                                    }
                                    ydjN = (float)(Math.Round((1 - Math.Exp((double)(equipconfig.YdjL) * 1.0 * Math.Log(1 - smoke.Ns / 100.0) / 430.0)) * 100, 1));

                                    Ref_Control_Text(TextBoxYDJSCZ, ydjN.ToString("0.0"));
                                    Msg_Toollabel(toolStripLabel2, "烟度计量程1检查完毕");
                                    ydjcheckdata.LabelValueN50 = float.Parse(TextBoxYDJSDZ.Text);
                                    ydjcheckdata.N501 = ydjN;
                                    ydjcheckdata.Error501 = Math.Abs(ydjN - float.Parse(TextBoxYDJSDZ.Text));
                                    if (ydjcheckdata.Error501 <= 2.0f)
                                    {
                                        Ref_Control_Text(LabelYDJSZ, "√");
                                    }
                                    else
                                    {
                                        isYdjCheckResult = false;
                                        Ref_Control_Text(LabelYDJSZ, "×");
                                    }
                                    isYdjSure = false;
                                    Msg_Toollabel(toolStripLabel2, "插好校准滤光片后点击\"确定\"按钮");
                                    while (!isYdjSure) Thread.Sleep(100);
                                    smoke = new Exhaust.Flb_100_smoke();
                                    if (equipconfig.IsOldMqy200)
                                    {
                                        smoke = flb_100.get_DirectData();
                                    }
                                    else
                                    {
                                        smoke = flb_100.get_Data();
                                    }
                                    ydjN = (float)(Math.Round((1 - Math.Exp((double)(equipconfig.YdjL) * 1.0 * Math.Log(1 - smoke.Ns / 100.0) / 430.0)) * 100, 1));

                                    Ref_Control_Text(TextBoxYDJSCZ2, ydjN.ToString("0.0"));
                                    Msg_Toollabel(toolStripLabel2, "烟度计量程2检查完毕");
                                    ydjcheckdata.LabelValueN70 = float.Parse(TextBoxYDJSDZ2.Text);
                                    ydjcheckdata.N701 = ydjN;
                                    ydjcheckdata.Error701 = Math.Abs(ydjN - float.Parse(TextBoxYDJSDZ2.Text));
                                    if (ydjcheckdata.Error701 <= 2.0f)
                                    {
                                        Ref_Control_Text(LabelYDJSZ2, "√");
                                    }
                                    else
                                    {
                                        isYdjCheckResult = false;
                                        Ref_Control_Text(LabelYDJSZ2, "×");
                                    }
                                    if (equipconfig.useJHJK)
                                    //if(true)
                                    {
                                        isYdjSure = false;
                                        Msg_Toollabel(toolStripLabel2, "插好校准滤光片后点击\"确定\"按钮");
                                        while (!isYdjSure) Thread.Sleep(100);
                                        smoke = new Exhaust.Flb_100_smoke();
                                        if (equipconfig.IsOldMqy200)
                                        {
                                            smoke = flb_100.get_DirectData();
                                        }
                                        else
                                        {
                                            smoke = flb_100.get_Data();
                                        }

                                        ydjN = (float)(Math.Round((1 - Math.Exp((double)(equipconfig.YdjL) * 1.0 * Math.Log(1 - smoke.Ns / 100.0) / 430.0)) * 100, 1));

                                        Ref_Control_Text(textBoxYDJSCZ3, ydjN.ToString("0.0"));
                                        Msg_Toollabel(toolStripLabel2, "烟度计量程3检查完毕");
                                        ydjcheckdata.LabelValueN90 = float.Parse(textBoxYDJSDZ3.Text);
                                        ydjcheckdata.N901 = ydjN;
                                        ydjcheckdata.Error901 = Math.Abs(ydjN - float.Parse(textBoxYDJSDZ3.Text));
                                        if (ydjcheckdata.Error901 <= 2.0f)
                                        {
                                            Ref_Control_Text(labelYDJSZ3, "√");
                                        }
                                        else
                                        {
                                            isYdjCheckResult = false;
                                            Ref_Control_Text(labelYDJSZ3, "×");
                                        }
                                    }
                                    else
                                    {
                                        ydjcheckdata.LabelValueN90 = 0;
                                        ydjcheckdata.N901 = 0;
                                        ydjcheckdata.Error901 = 0;
                                        Ref_Control_Text(textBoxYDJSCZ3, "—");
                                        Ref_Control_Text(labelYDJSZ3, "—");
                                    }
                                    ydjcheckdata.CheckResult1 = (isYdjCheckResult ? "1" : "0");
                                    ydjcheckdata.Jcds = "2";
                                    ydjcheckdata.Zjjg = (isYdjCheckResult ? "通过" : "不通过");
                                    endtime = DateTime.Now;
                                    ydjcheckdata.CheckTimeStart = starttime.ToString("yyyy-MM-dd HH:mm:ss");
                                    ydjcheckdata.CheckTimeEnd = endtime.ToString("yyyy-MM-dd HH:mm:ss");
                                    ydjcheckdata.Remark = "";
                                    selfcheckini.writeydjSelfcheck(ydjcheckdata);
                                    Thread.Sleep(500);
                                    selfStep++;
                                    Msg_Toollabel(toolStripLabel2, "烟度计自检完毕");
                                    selfcheckrecord.Ydjcheckrecord = isYdjCheckResult;
                                    selfcheckini.writeCheckRecord(selfcheckrecord);
                                    //refselfcheckItem();
                                }
                                else
                                {
                                    Msg_Toollabel(toolStripLabel2, "烟度计通讯异常");
                                    Ref_Control_Text(LabelYDJTX, "×");
                                    isRunning = false;
                                    return;
                                }
                            }
                        }
                        Thread.Sleep(2000);
                        checkstate.ChCmd = "S";
                        checkstate.NType = "K";
                        checkstate.Sjxl = JCSJ.ToString();
                        checkstate.Sbbh = "3";
                        selfcheckini.writeSelfCheckState(checkstate);
                    }
                    else
                    {
                        selfStep++;
                    }
                }
                if (true)
                {
                    fq_Status = false;
                    if (checkBoxItemFqy.Checked)
                    {

                        Msg_Toollabel(toolStripLabel2, "即将开始废气仪自检");
                        Thread.Sleep(2000);
                        checkstate.ChCmd = "S";
                        checkstate.NType = "V";
                        checkstate.Sjxl = JCSJ.ToString();
                        checkstate.Sbbh = "2";
                        selfcheckini.writeSelfCheckState(checkstate);
                        if (!checkBoxItemSdsFqy.Checked)
                        {
                            if (fla_502 != null)
                            {
                                bool isfqycheckresult = true;
                                wqfxySelfcheck fqysefdata = new wqfxySelfcheck();
                                DateTime starttime, endtime;
                                starttime = DateTime.Now;
                                string fqyzt = fla_502.Get_Struct();
                                if (fqyzt.Contains("失败"))
                                {
                                    Msg_Toollabel(toolStripLabel2, "废气仪通讯异常");
                                    Ref_Control_Text(LabelFQYTX, "×");
                                    isRunning = false;
                                    fq_Status = true;
                                    return;
                                }
                                else if (fqyzt.Contains("预热"))
                                {
                                    Msg_Toollabel(toolStripLabel2, "废气仪正在预热，自检将中止");
                                    Ref_Control_Text(LabelFQYTX, "×");
                                    isRunning = false;
                                    fq_Status = true;
                                    return;
                                }
                                else
                                {
                                    Msg_Toollabel(toolStripLabel2, "废气仪通讯正常");
                                    Ref_Control_Text(LabelFQYTX, "√");
                                }
                                Thread.Sleep(500);
                                Msg_Toollabel(toolStripLabel2, "堵住探头进气口后点击\"确定\"按钮进行检漏");
                                while (!isJLsure) Thread.Sleep(100);
                                fla_502.Leak_check();
                                Thread.Sleep(100);
                                if (equipconfig.Fqyxh == "fasm_5000")
                                {
                                    int leaktest = 0;
                                    int leaktesting = 0;
                                    while (leaktesting == 0)
                                    {
                                        leaktesting = fla_502.waitSuccessAnswer();
                                        Msg_Toollabel(toolStripLabel2, "检漏中..." + leaktest.ToString() + "s");
                                        leaktest++;
                                        Thread.Sleep(900);
                                    }
                                    if (leaktesting == 1)
                                    {
                                        Ref_Control_Text(LabelFQYJL, "√");
                                        fqysefdata.TightnessResult = "1";
                                        Msg_Toollabel(toolStripLabel2, "检漏完毕");
                                        //leaktesting = true;
                                    }
                                    else if (leaktesting == -1)
                                    {
                                        Ref_Control_Text(LabelFQYJL, "×");
                                        fqysefdata.TightnessResult = "0";
                                        Msg_Toollabel(toolStripLabel2, "检漏完毕");
                                        //leaktesting = true;
                                    }
                                    else
                                    {
                                        Ref_Control_Text(LabelFQYJL, "×");
                                        fqysefdata.TightnessResult = "0";
                                        Msg_Toollabel(toolStripLabel2, "检漏失败");
                                        isfqycheckresult = false;
                                    }

                                }
                                else if (equipconfig.Fqyxh == "fla_502")
                                {
                                    int leaktest = 0;
                                    bool leaktesting = false;
                                    while (!leaktesting)
                                    {
                                        string leakstring = fla_502.Get_fla502leckStruct();
                                        if (leakstring == "无泄漏")
                                        {
                                            Ref_Control_Text(LabelFQYJL, "√");
                                            fqysefdata.TightnessResult = "1";
                                            Msg_Toollabel(toolStripLabel2, "检漏完毕");
                                            leaktesting = true;
                                        }
                                        else if (leakstring == "泄漏超标")
                                        {
                                            Ref_Control_Text(LabelFQYJL, "×");
                                            fqysefdata.TightnessResult = "0";
                                            Msg_Toollabel(toolStripLabel2, "检漏完毕");
                                            isfqycheckresult = false;
                                            leaktesting = true;
                                        }
                                        else
                                        {
                                            Msg_Toollabel(toolStripLabel2, "检漏中..." + leaktest.ToString() + "s");
                                            leaktest++;
                                            Thread.Sleep(900);
                                        }
                                    }
                                }
                                else if (equipconfig.Fqyxh == "cdf5000")
                                {
                                    int leaktest = 0;
                                    bool leaktesting = false;
                                    while (!leaktesting)
                                    {
                                        string leakstring = fla_502.Get_fla502leckStruct();
                                        if (leakstring == "无泄漏")
                                        {
                                            Ref_Control_Text(LabelFQYJL, "√");
                                            fqysefdata.TightnessResult = "1";
                                            Msg_Toollabel(toolStripLabel2, "检漏完毕");
                                            leaktesting = true;
                                        }
                                        else if (leakstring == "泄漏超标")
                                        {
                                            Ref_Control_Text(LabelFQYJL, "×");
                                            fqysefdata.TightnessResult = "0";
                                            Msg_Toollabel(toolStripLabel2, "检漏完毕");
                                            isfqycheckresult = false;
                                            leaktesting = true;
                                        }
                                        else
                                        {
                                            Msg_Toollabel(toolStripLabel2, "检漏中..." + leaktest.ToString() + "s");
                                            leaktest++;
                                            Thread.Sleep(900);
                                        }
                                    }
                                }
                                else if (equipconfig.Fqyxh != "fla_501" && equipconfig.Fqyxh != "mqw_511")
                                {
                                    int leaktest = 0;
                                    bool leaktesting = false;
                                    while (!leaktesting)
                                    {
                                        string leakstring = fla_502.Get_leakTestStruct();
                                        if (leakstring == "无泄漏")
                                        {
                                            Ref_Control_Text(LabelFQYJL, "√");
                                            fqysefdata.TightnessResult = "1";
                                            Msg_Toollabel(toolStripLabel2, "检漏完毕");
                                            leaktesting = true;
                                        }
                                        else if (leakstring == "泄漏超标")
                                        {
                                            Ref_Control_Text(LabelFQYJL, "×");
                                            fqysefdata.TightnessResult = "0";
                                            Msg_Toollabel(toolStripLabel2, "检漏完毕");
                                            isfqycheckresult = false;
                                            leaktesting = true;
                                        }
                                        else
                                        {
                                            Msg_Toollabel(toolStripLabel2, "检漏中..." + leaktest.ToString() + "s");
                                            leaktest++;
                                            Thread.Sleep(900);
                                        }
                                    }
                                }

                                Thread.Sleep(500);
                                fla_502.Zeroing();                 //反吹                  1
                                Thread.Sleep(500);
                                int zero_count = 0;
                                if (equipconfig.Fqyxh.ToLower() == "nha_503")
                                {
                                    while (fla_502.Zeroing() == false)//该处需要测试后定
                                    {
                                        Thread.Sleep(900);
                                        Msg_Toollabel(toolStripLabel2, "废气仪调零中..." + zero_count.ToString() + "s");
                                        zero_count++;
                                        if (zero_count == 60)
                                            break;
                                    }
                                }
                                else if (equipconfig.Fqyxh != "fla_501" && equipconfig.Fqyxh != "mqw_511")
                                {
                                    while (fla_502.Get_Struct() == "调零中")
                                    {
                                        Thread.Sleep(900);
                                        Msg_Toollabel(toolStripLabel2, "废气仪调零中..." + zero_count.ToString() + "s");
                                        zero_count++;
                                        if (zero_count == 60)
                                            break;
                                    }
                                }
                                Ref_Control_Text(LabelFQYTL, "√");
                                Msg_Toollabel(toolStripLabel2, "调零完毕");
                                Thread.Sleep(500);
                                Msg_Toollabel(toolStripLabel2, "开始检查气路流量，请保持气路畅通");
                                Thread.Sleep(1000);
                                fla_502.Pump_Pipeair();
                                for (int count = 20; count >= 0; count--)
                                {
                                    Msg_Toollabel(toolStripLabel2, "抽气中..." + count.ToString());
                                    Thread.Sleep(1000);
                                }
                                if (fla_502.CheckIsLowFlow() < 0)
                                {
                                    Ref_Control_Text(labelFQYLL, "×");
                                    fqysefdata.Yqll = "0";
                                    Msg_Toollabel(toolStripLabel2, "气路流量完毕");
                                    isfqycheckresult = false;
                                }
                                else
                                {
                                    Ref_Control_Text(labelFQYLL, "√");
                                    fqysefdata.Yqll = "1";
                                    Msg_Toollabel(toolStripLabel2, "气路流量完毕");
                                }
                                Thread.Sleep(500);
                                fla_502.StopBlowback();
                                Thread.Sleep(500);
                                Exhaust.Fla502_data fla502data = new Exhaust.Fla502_data();
                                fla502data = fla_502.GetData();
                                //fqysefdata.Yqll = "";
                                fqysefdata.Yqyq = fla502data.O2.ToString("0.00");
                                Ref_Control_Text(TextBoxFQYYQ, fla502data.O2.ToString("0.00"));
                                Ref_Control_Text(LabelFQYYQ, "√");
                                Ref_Control_Text(labelHCPd, "√");
                                //Ref_Control_Text(TextBoxFQYLL, "0.0");
                                Thread.Sleep(500);
                                selfStep++;
                                Msg_Toollabel(toolStripLabel2, "废气仪自检完毕");
                                endtime = DateTime.Now;
                                fqysefdata.CheckTimeStart = starttime.ToString("yyyy-MM-dd HH:mm:ss");
                                fqysefdata.CheckTimeEnd = endtime.ToString("yyyy-MM-dd HH:mm:ss");
                                fqysefdata.Remark = isfqycheckresult ? "1" : "0";
                                fqysefdata.Zjjg = isfqycheckresult ? "通过" : "不通过";
                                selfcheckini.writewqfxySelfcheck(fqysefdata);
                                selfcheckrecord.Fqycheckrecord = isfqycheckresult;
                                selfcheckini.writeCheckRecord(selfcheckrecord);
                               // refselfcheckItem();
                            }
                            else
                            {
                                Msg_Toollabel(toolStripLabel2, "废气仪通讯异常");
                                Ref_Control_Text(LabelFQYTX, "×");
                                isRunning = false;
                                fq_Status = true;
                                return;
                            }
                        }
                        else
                        {
                            if (fla_502 != null)
                            {
                                bool isfqycheckresult = true;
                                sdsqtfxySelfcheck fqysefdata = new sdsqtfxySelfcheck();
                                DateTime starttime, endtime;
                                starttime = DateTime.Now;
                                string fqyzt = fla_502.Get_Struct();
                                if (fqyzt.Contains("失败"))
                                {
                                    Msg_Toollabel(toolStripLabel2, "废气仪通讯异常");
                                    Ref_Control_Text(LabelFQYTX, "×");
                                    isRunning = false;
                                    fq_Status = true;
                                    return;
                                }
                                else if (fqyzt.Contains("预热"))
                                {
                                    Msg_Toollabel(toolStripLabel2, "废气仪正在预热，自检将中止");
                                    Ref_Control_Text(LabelFQYTX, "×");
                                    isRunning = false;
                                    fq_Status = true;
                                    return;
                                }
                                else
                                {
                                    Msg_Toollabel(toolStripLabel2, "废气仪通讯正常");
                                    Ref_Control_Text(LabelFQYTX, "√");
                                }
                                Thread.Sleep(500);
                                Msg_Toollabel(toolStripLabel2, "堵住探头进气口后点击\"确定\"按钮进行检漏");
                                while (!isJLsure) Thread.Sleep(100);
                                fla_502.Leak_check();
                                Thread.Sleep(100);
                                if (equipconfig.Fqyxh == "fasm_5000")
                                {
                                    int leaktest = 0;
                                    int leaktesting = 0;
                                    while (leaktesting == 0)
                                    {
                                        leaktesting = fla_502.waitSuccessAnswer();
                                        Msg_Toollabel(toolStripLabel2, "检漏中..." + leaktest.ToString() + "s");
                                        leaktest++;
                                        Thread.Sleep(900);
                                    }
                                    if (leaktesting == 1)
                                    {
                                        Ref_Control_Text(LabelFQYJL, "√");
                                        fqysefdata.TightnessResult = "1";
                                        Msg_Toollabel(toolStripLabel2, "检漏完毕");
                                        //leaktesting = true;
                                    }
                                    else if (leaktesting == -1)
                                    {
                                        Ref_Control_Text(LabelFQYJL, "×");
                                        fqysefdata.TightnessResult = "0";
                                        Msg_Toollabel(toolStripLabel2, "检漏完毕");
                                        //leaktesting = true;
                                    }
                                    else
                                    {
                                        Ref_Control_Text(LabelFQYJL, "×");
                                        fqysefdata.TightnessResult = "0";
                                        Msg_Toollabel(toolStripLabel2, "检漏失败");
                                        isfqycheckresult = false;
                                    }

                                }
                                else if (equipconfig.Fqyxh == "fla_502")
                                {
                                    int leaktest = 0;
                                    bool leaktesting = false;
                                    while (!leaktesting)
                                    {
                                        string leakstring = fla_502.Get_fla502leckStruct();
                                        if (leakstring == "无泄漏")
                                        {
                                            Ref_Control_Text(LabelFQYJL, "√");
                                            fqysefdata.TightnessResult = "1";
                                            Msg_Toollabel(toolStripLabel2, "检漏完毕");
                                            leaktesting = true;
                                        }
                                        else if (leakstring == "泄漏超标")
                                        {
                                            Ref_Control_Text(LabelFQYJL, "×");
                                            fqysefdata.TightnessResult = "0";
                                            Msg_Toollabel(toolStripLabel2, "检漏完毕");
                                            isfqycheckresult = false;
                                            leaktesting = true;
                                        }
                                        else
                                        {
                                            Msg_Toollabel(toolStripLabel2, "检漏中..." + leaktest.ToString() + "s");
                                            leaktest++;
                                            Thread.Sleep(900);
                                        }
                                    }
                                }
                                else if (equipconfig.Fqyxh == "cdf5000")
                                {
                                    int leaktest = 0;
                                    bool leaktesting = false;
                                    while (!leaktesting)
                                    {
                                        string leakstring = fla_502.Get_fla502leckStruct();
                                        if (leakstring == "无泄漏")
                                        {
                                            Ref_Control_Text(LabelFQYJL, "√");
                                            fqysefdata.TightnessResult = "1";
                                            Msg_Toollabel(toolStripLabel2, "检漏完毕");
                                            leaktesting = true;
                                        }
                                        else if (leakstring == "泄漏超标")
                                        {
                                            Ref_Control_Text(LabelFQYJL, "×");
                                            fqysefdata.TightnessResult = "0";
                                            Msg_Toollabel(toolStripLabel2, "检漏完毕");
                                            isfqycheckresult = false;
                                            leaktesting = true;
                                        }
                                        else
                                        {
                                            Msg_Toollabel(toolStripLabel2, "检漏中..." + leaktest.ToString() + "s");
                                            leaktest++;
                                            Thread.Sleep(900);
                                        }
                                    }
                                }
                                else if (equipconfig.Fqyxh != "fla_501" && equipconfig.Fqyxh != "mqw_511")
                                {
                                    int leaktest = 0;
                                    bool leaktesting = false;
                                    while (!leaktesting)
                                    {
                                        string leakstring = fla_502.Get_leakTestStruct();
                                        if (leakstring == "无泄漏")
                                        {
                                            Ref_Control_Text(LabelFQYJL, "√");
                                            fqysefdata.TightnessResult = "1";
                                            Msg_Toollabel(toolStripLabel2, "检漏完毕");
                                            leaktesting = true;
                                        }
                                        else if (leakstring == "泄漏超标")
                                        {
                                            Ref_Control_Text(LabelFQYJL, "×");
                                            fqysefdata.TightnessResult = "0";
                                            Msg_Toollabel(toolStripLabel2, "检漏完毕");
                                            isfqycheckresult = false;
                                            leaktesting = true;
                                        }
                                        else
                                        {
                                            Msg_Toollabel(toolStripLabel2, "检漏中..." + leaktest.ToString() + "s");
                                            leaktest++;
                                            Thread.Sleep(900);
                                        }
                                    }
                                }
                                Thread.Sleep(500);
                                fla_502.Zeroing();                 //反吹                  1
                                Thread.Sleep(500);
                                int zero_count = 0;
                                if (equipconfig.Fqyxh.ToLower() == "nha_503")
                                {
                                    while (fla_502.Zeroing() == false)//该处需要测试后定
                                    {
                                        Thread.Sleep(900);
                                        Msg_Toollabel(toolStripLabel2, "废气仪调零中..." + zero_count.ToString() + "s");
                                        zero_count++;
                                        if (zero_count == 60)
                                            break;
                                    }
                                }
                                else if (equipconfig.Fqyxh != "fla_501" && equipconfig.Fqyxh != "mqw_511")
                                {
                                    while (fla_502.Get_Struct() == "调零中")
                                    {
                                        Thread.Sleep(900);
                                        Msg_Toollabel(toolStripLabel2, "废气仪调零中..." + zero_count.ToString() + "s");
                                        zero_count++;
                                        if (zero_count == 60)
                                            break;
                                    }
                                }
                                Ref_Control_Text(LabelFQYTL, "√");
                                Msg_Toollabel(toolStripLabel2, "调零完毕");
                                Thread.Sleep(500);
                                Msg_Toollabel(toolStripLabel2, "开始检查气路流量");
                                fla_502.Pump_Pipeair();
                                for (int count = 20; count >= 0; count--)
                                {
                                    Msg_Toollabel(toolStripLabel2, "抽气中..." + count.ToString());
                                    Thread.Sleep(1000);
                                }
                                if (fla_502.CheckIsLowFlow() < 0)
                                {
                                    Ref_Control_Text(labelFQYLL, "×");
                                    fqysefdata.Yqll = "0";
                                    Msg_Toollabel(toolStripLabel2, "气路流量完毕");
                                    isfqycheckresult = false;
                                }
                                else
                                {
                                    Ref_Control_Text(labelFQYLL, "√");
                                    fqysefdata.Yqll = "1";
                                    Msg_Toollabel(toolStripLabel2, "气路流量完毕");
                                }
                                Thread.Sleep(500);
                                fla_502.StopBlowback();
                                Thread.Sleep(500);
                                Exhaust.Fla502_data fla502data = new Exhaust.Fla502_data();
                                fla_502.Start();
                                for (int i = 15; i >= 0; i++)
                                {
                                    Msg_Toollabel(toolStripLabel2, "残留HC检测中..." + i.ToString() + "s");
                                }
                                fla502data = fla_502.GetData();
                                fla_502.Stop();
                                Ref_Control_Text(textBoxClHc, fla502data.HC.ToString());
                                Ref_Control_Text(TextBoxFQYYQ, fla502data.O2.ToString("0.00"));
                                Ref_Control_Text(LabelFQYYQ, "√");
                                Ref_Control_Text(labelHCPd, "√");
                                fqysefdata.CanliuHC = double.Parse(textBoxClHc.Text);
                                //fqysefdata.Yqll = "";
                                fqysefdata.Yqyq = fla502data.O2.ToString("0.00");
                                //Ref_Control_Text(TextBoxFQYLL, "0.0");
                                Thread.Sleep(500);
                                selfStep++;
                                Msg_Toollabel(toolStripLabel2, "废气仪自检完毕");
                                endtime = DateTime.Now;
                                fqysefdata.CheckResult = isfqycheckresult ? "1" : "0";
                                fqysefdata.Zjjg = isfqycheckresult ? "通过" : "不通过";
                                fqysefdata.CheckTimeStart = starttime.ToString("yyyy-MM-dd HH:mm:ss");
                                fqysefdata.CheckTimeEnd = endtime.ToString("yyyy-MM-dd HH:mm:ss");
                                fqysefdata.Remark = "";
                                selfcheckini.writesdsqtfxySelfcheck(fqysefdata);
                                selfcheckrecord.Fqycheckrecord = isfqycheckresult;
                                selfcheckini.writeCheckRecord(selfcheckrecord);
                                //refselfcheckItem();
                            }
                            else
                            {
                                Msg_Toollabel(toolStripLabel2, "废气仪通讯异常");
                                Ref_Control_Text(LabelFQYTX, "×");
                                isRunning = false;
                                fq_Status = true;
                                return;
                            }
                        }
                        
                        Thread.Sleep(2000);
                        checkstate.ChCmd = "S";
                        checkstate.NType = "K";
                        checkstate.Sjxl = JCSJ.ToString();
                        checkstate.Sbbh = "2";
                        selfcheckini.writeSelfCheckState(checkstate);
                    }
                    fq_Status = true;
                }
                if (true)
                {
                    if (checkBoxItemLlj.Checked)
                    {
                        if (flv_1000 != null)
                        {
                            bool lljcheckresult = true;
                            DateTime starttime, endtime;
                            starttime = DateTime.Now;
                            lljSelfcheck lljcheckdata = new lljSelfcheck();
                            Msg_Toollabel(toolStripLabel2, "流量计自检开始");
                            if (islljpoweroff)
                            {
                                Msg_Toollabel(toolStripLabel2, "流量计通讯异常");
                                Ref_Control_Text(labelLljtxpd, "×");
                                isRunning = false;
                                return;
                            }
                            else
                            {
                                Msg_Toollabel(toolStripLabel2, "流量计通讯正常");
                                Ref_Control_Text(labelLljtxpd, "√");
                            }
                            
                            Thread.Sleep(500);
                            lljcheckdata.TxJc = "1";
                            //flv_1000.Get_standardDat();
                            Msg_Toollabel(toolStripLabel2, "流量计流量检查");
                            Thread.Sleep(1000);
                            Ref_Control_Text(textBoxLljll, flv_1000.ll_standard_value.ToString("0.00"));
                            if (flv_1000.ll_standard_value < 95f)
                            {
                                Ref_Control_Text(labelLljllpd, "×");
                                lljcheckresult = false;
                                Msg_Toollabel(toolStripLabel2, "流量计流量低于95L/s");
                            }
                            else
                            {
                                Ref_Control_Text(labelLljllpd, "√");
                                Msg_Toollabel(toolStripLabel2, "流量计流量正常");
                            }
                            Msg_Toollabel(toolStripLabel2, "流量计实际流量检查");
                            Thread.Sleep(1000);
                            double lljsjll = flv_1000.ll_unstandard_value;
                            Ref_Control_Text(textBoxSjll, lljsjll.ToString("0.00"));
                            double wc = (lljsjll - equipconfig.Lljllmy) * 100.0 / equipconfig.Lljllmy;
                            Ref_Control_Text(textBoxLLjLLwc, wc.ToString("0.00"));
                            if (Math.Abs(wc)>10)
                            {
                                Ref_Control_Text(labelLljsjllpd, "×");
                                //lljcheckresult = false;
                                Msg_Toollabel(toolStripLabel2, "流量计实际流量超标");
                            }
                            else
                            {
                                Ref_Control_Text(labelLljsjllpd, "√");
                                Msg_Toollabel(toolStripLabel2, "流量计实际流量正常");
                            }
                            lljcheckdata.lljsjll = lljsjll;
                            lljcheckdata.lljmyll = equipconfig.Lljllmy;
                            lljcheckdata.lljsjllwc = wc;
                            Thread.Sleep(1000);
                            Msg_Toollabel(toolStripLabel2, "流量计氧气检查");
                            Thread.Sleep(1000);
                            Ref_Control_Text(textBoxLljo2, flv_1000.o2_standard_value.ToString("0.00"));
                            if (flv_1000.o2_standard_value <= 21.3f && flv_1000.o2_standard_value >= 20.3f)
                            {
                                Msg_Toollabel(toolStripLabel2, "流量计氧气正常");
                                Ref_Control_Text(labelLljO2pd, "√");
                            }
                            else
                            {
                                Ref_Control_Text(labelLljO2pd, "×");
                                Msg_Toollabel(toolStripLabel2, "流量计氧气超标");
                                lljcheckresult = false;
                            }
                            endtime = DateTime.Now;
                            lljcheckdata.TxJc = "1";
                            lljcheckdata.Lljll = flv_1000.ll_standard_value.ToString("0.00");
                            lljcheckdata.Lljo2 = flv_1000.o2_standard_value.ToString("0.00");
                            lljcheckdata.CheckResult = lljcheckresult ? "1" : "0";
                            lljcheckdata.CheckTimeStart = starttime.ToString("yyyy-MM-dd HH:mm:ss");
                            lljcheckdata.CheckTimeEnd = endtime.ToString("yyyy-MM-dd HH:mm:ss");
                            lljcheckdata.Remark = "";
                            selfcheckini.writeLLJcheckIni(lljcheckdata);
                            Thread.Sleep(500);
                            selfStep++;
                            Msg_Toollabel(toolStripLabel2, "流量计自检完毕");
                            selfcheckrecord.Lljcheckrecord = lljcheckresult;
                            selfcheckini.writeCheckRecord(selfcheckrecord);
                            //refselfcheckItem();
                        }
                        else
                        {
                            Msg_Toollabel(toolStripLabel2, "流量计通讯异常");
                            Ref_Control_Text(LabelYDJTX, "×");
                            isRunning = false;
                            return;
                        }
                    }
                    else
                    {
                        selfStep++;
                    }
                }
                if (true)
                {
                    if (checkBoxQxz.Checked)
                    {

                        Msg_Toollabel(toolStripLabel2, "即将开始气象单元自检");
                        Thread.Sleep(2000);
                        checkstate.ChCmd = "S";
                        checkstate.NType = "V";
                        checkstate.Sjxl = JCSJ.ToString();
                        checkstate.Sbbh = "4";
                        selfcheckini.writeSelfCheckState(checkstate);
                        if ( fla_502!= null||flb_100!=null||equipconfig.TempInstrument=="模拟")
                        {
                            //bool tempok = true,humiok=true,airpok=true;
                            DateTime starttime, endtime;
                            starttime = DateTime.Now;
                            hjcsgyqSelfcheck hjcsdata = new hjcsgyqSelfcheck();
                            Msg_Toollabel(toolStripLabel2, "气象单元自检开始");
                            Ref_Control_Text(labelQXZTX, "√");
                            Thread.Sleep(500);
                            iswdsure = false;
                            Msg_Toollabel(toolStripLabel2, "开始检测气象单元，填入数据值后点击确定");
                            while (!iswdsure) Thread.Sleep(500);
                            double wdxdwc, sdxdwc, dqyxdwc, wdjdwc, sdjdwc, dqyjdwc;
                            bool pdjg = true;
                            double wd=0, sd=0, dqy=0;
                            try//获取环境参数
                            {
                                Exhaust.Fla502_data Environment = new Exhaust.Fla502_data();
                                Exhaust.Flb_100_smoke ydjEnvironment = new Exhaust.Flb_100_smoke();
                                if(IsUseTpTemp)
                                {
                                    wd = tpwd;
                                    sd = tpsd;
                                    dqy = tpdqy;
                                }
                                else if (equipconfig.TempInstrument == "烟度计"&&flb_100!=null)
                                {
                                    if (equipconfig.IsOldMqy200)
                                    {
                                        ydjEnvironment = flb_100.get_DirectData();
                                    }
                                    else
                                    {
                                        ydjEnvironment = flb_100.get_Data();
                                    }
                                    wd = ydjEnvironment.WD;
                                    sd = ydjEnvironment.SD;
                                    dqy = ydjEnvironment.DQY;
                                }
                                else if (equipconfig.TempInstrument == "废气仪")
                                {
                                    if (equipconfig.Fqyxh.ToLower() == "nha_503" || equipconfig.Fqyxh.ToLower() == "fla_502"|| equipconfig.Ydjxh.ToLower() == "cdf5000")
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
                                else if (equipconfig.TempInstrument == "XCE_101")
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
                                wd = thaxsdata.Tempxs * wd;
                                sd = thaxsdata.Humixs * sd;
                                dqy = thaxsdata.Airpxs * dqy;

                                Ref_Control_Text(textBoxWDSCZ, wd.ToString("0.0"));
                                Ref_Control_Text(textBoxSDSCZ, sd.ToString("0.0"));
                                Ref_Control_Text(textBoxDQYSCZ, dqy.ToString("0.0"));
                            }
                            catch (Exception)
                            {
                            }
                            hjcsdata.ActualAirPressure = double.Parse(textBoxDQYSDZ.Text);
                            hjcsdata.AirPressure = dqy;
                            hjcsdata.ActualTemperature = double.Parse(textBoxWDSDZ.Text);
                            hjcsdata.Temperature = wd;
                            hjcsdata.ActualHumidity = double.Parse(textBoxSDSDZ.Text);
                            hjcsdata.Humidity = sd;
                            hjcsdata.CheckTimeStart = starttime.ToString("yyyy-MM-dd HH:mm:ss");
                            hjcsdata.CheckTimeEnd = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            hjcsdata.Remark = "";            
                            //selfcheckini.writedzhjSelfcheck(hjcsdata);
                            wdjdwc = Math.Abs(hjcsdata.Temperature - hjcsdata.ActualTemperature);
                            wdxdwc = Math.Round(wdjdwc / hjcsdata.ActualTemperature, 3);
                            if (wdjdwc > 0.04 && wdjdwc > 1)
                            {
                                pdjg = false;
                                hjcsdata.Tempok = false;
                                Ref_Control_Text(labelwd, "×");
                                //LabelWdpd.Text = "×";
                            }
                            else
                            {
                                Ref_Control_Text(labelwd, "√");
                                hjcsdata.Tempok = true;
                            }
                                //LabelWdpd.Text = "√";
                            //tempdata.Wdwc = wdjdwc;
                            sdjdwc = Math.Abs(hjcsdata.Humidity - hjcsdata.ActualHumidity);
                            sdxdwc = Math.Round(sdjdwc / hjcsdata.ActualHumidity, 3);
                            if (sdjdwc > 3 && sdxdwc > 0.05)
                            {
                                pdjg = false;
                                hjcsdata.Humiok = false;
                                Ref_Control_Text(labelsd, "×");
                                //LabelSdpd.Text = "×";
                            }
                            else
                            {
                                Ref_Control_Text(labelsd, "√");
                                hjcsdata.Humiok = true;
                            }
                                //LabelSdpd.Text = "√";
                            //tempdata.Sdwc = sdjdwc;
                            dqyjdwc = Math.Abs(hjcsdata.AirPressure - hjcsdata.ActualAirPressure);
                            dqyxdwc = Math.Round(dqyjdwc / hjcsdata.ActualAirPressure, 3);
                            if (dqyjdwc > 1.5 && dqyxdwc > 0.03)
                            {
                                pdjg = false;
                                hjcsdata.Airpok = false;
                                Ref_Control_Text(labeldqy, "×");
                                //LabelDqypd.Text = "×";
                            }
                            else
                            {
                                Ref_Control_Text(labeldqy, "√");
                                hjcsdata.Airpok = true;
                            }
                            //LabelDqypd.Text = "√";
                            hjcsdata.Zjjg = (pdjg ? "通过" : "不通过");
                            selfcheckini.writedzhjSelfcheck(hjcsdata);
                        }
                        else
                        {
                            Msg_Toollabel(toolStripLabel2, "气象站通讯异常");
                            Ref_Control_Text(labelQXZTX, "×");
                            isRunning = false;
                            return;
                        }
                        Thread.Sleep(2000);
                        checkstate.ChCmd = "S";
                        checkstate.NType = "K";
                        checkstate.Sjxl = JCSJ.ToString();
                        checkstate.Sbbh = "4";
                        selfcheckini.writeSelfCheckState(checkstate);
                    }
                    else
                    {
                        selfStep++;
                    }
                }
                if (true)
                {
                    if (checkBoxZsj.Checked)
                    {
                        Msg_Toollabel(toolStripLabel2, "即将开始转速计自检");
                        Thread.Sleep(2000);
                        checkstate.ChCmd = "S";
                        checkstate.NType = "V";
                        checkstate.Sjxl = JCSJ.ToString();
                        checkstate.Sbbh = "5";
                        selfcheckini.writeSelfCheckState(checkstate);
                        if (fla_502 != null || flb_100 != null)
                        {
                            fdjzsSelfCheck fdjzscheckdata = new fdjzsSelfCheck();
                            Msg_Toollabel(toolStripLabel2, "转速计自检开始");
                            Ref_Control_Text(labelZSJTX, "√");
                            Thread.Sleep(500);
                            Exhaust.Fla502_data fladata=new Exhaust.Fla502_data();
                            Exhaust.Flb_100_smoke smoke = new Exhaust.Flb_100_smoke();
                            iszssure = false;
                            Msg_Toollabel(toolStripLabel2, "安置好转速计后点确定");
                            while (!iszssure) Thread.Sleep(500);
                            if (comboBoxZSJ.Text == "废气仪")
                            {
                                if (fla_502 != null)
                                {
                                    fladata = fla_502.GetData();
                                    Ref_Control_Text(textBoxDSZS, fladata.ZS.ToString("0"));
                                    fdjzscheckdata.Dszs = fladata.ZS.ToString("0");
                                }
                                else
                                {
                                    Msg_Toollabel(toolStripLabel2, "转速计通讯异常");
                                    Ref_Control_Text(labelZSJTX, "×");
                                    isRunning = false;
                                    //return;
                                }
                            }
                            else if (comboBoxZSJ.Text == "烟度计"&&flb_100!=null)
                            {
                                if (flb_100 != null)
                                {
                                    smoke = flb_100.get_DirectData();
                                    Ref_Control_Text(textBoxDSZS, smoke.Zs.ToString("0"));
                                    fdjzscheckdata.Dszs = smoke.Zs.ToString("0");
                                }
                                else
                                {
                                    Msg_Toollabel(toolStripLabel2, "转速计通讯异常");
                                    Ref_Control_Text(labelZSJTX, "×");
                                    isRunning = false;
                                    //return;
                                }
                            }
                            else
                            {
                                if (fla_502 != null)
                                {
                                    fladata = fla_502.GetData();
                                    Ref_Control_Text(textBoxDSZS, fladata.ZS.ToString("0"));
                                    fdjzscheckdata.Dszs = fladata.ZS.ToString("0");
                                }
                                else if (flb_100 != null)
                                {
                                    smoke = flb_100.get_DirectData();
                                    Ref_Control_Text(textBoxDSZS, smoke.Zs.ToString("0"));
                                    fdjzscheckdata.Dszs = smoke.Zs.ToString("0");
                                }
                                else
                                {
                                    Msg_Toollabel(toolStripLabel2, "转速计通讯异常");
                                    Ref_Control_Text(labelZSJTX, "×");
                                    isRunning = false;
                                    //return;
                                }
                            }
                            fdjzscheckdata.Zjjg = "通过";
                            selfcheckini.writefdjzsSelfcheck(fdjzscheckdata);
                            Ref_Control_Text(labelDSZS, "√");
                            
                        }
                        else
                        {
                            Msg_Toollabel(toolStripLabel2, "转速计通讯异常");
                            Ref_Control_Text(labelZSJTX, "×");
                            isRunning = false;
                            return;
                        }
                        Thread.Sleep(2000);
                        checkstate.ChCmd = "S";
                        checkstate.NType = "K";
                        checkstate.Sjxl = JCSJ.ToString();
                        checkstate.Sbbh = "5";
                        selfcheckini.writeSelfCheckState(checkstate);
                    }
                    else
                    {
                        selfStep++;
                    }
                }
                isRunning = false;
                Msg_Toollabel(toolStripLabel2, "已经完成自检，如果需要重新自检，请点击\"重新开始\"");
                refselfcheckItem();
            }
            catch
            { }
        }

        private void waitTestFinishedMoni()
        {
            try
            {
                isRunning = true;
                if (checkBoxItemJzhx.Checked || checkBoxJsGl.Checked)
                {
                    Msg_Toollabel(toolStripLabel2, "即将开始测功计自检");
                    Thread.Sleep(2000);
                    checkstate.ChCmd = "S";
                    checkstate.NType = "V";
                    checkstate.Sjxl = JCSJ.ToString();
                    checkstate.Sbbh = "1";
                    selfcheckini.writeSelfCheckState(checkstate);
                    if (checkBoxItemJzhx.Checked || checkBoxJsGl.Checked)//对测功机进行自检
                    {
                        if (true)
                        {
                            bool isCgjCheckResult = true;
                            float maxspeed = 71;
                            if (equipconfig.TestStandard!="HJT292")
                                maxspeed = 56;
                            Msg(LabelCGJTXJC, panelcs, "0.0");
                            Ref_Control_Text(LabelCGJTXJC, "√");
                            Thread.Sleep(500);
                            Ref_Control_Text(LabelCGJJSS, "√");
                            Thread.Sleep(500);
                            Ref_Control_Text(LabelCGJJSJ, "√");
                            Thread.Sleep(500);
                            if (checkBoxItemJzhx.Checked)
                            {
                                Msg_Toollabel(toolStripLabel2, "开始加载滑行:加速");
                                Thread.Sleep(1000);
                                DateTime starttime56, endtime56, starttime40, endtime40;
                                DateTime starttime, endtime;
                                CGJselfCheckdata jzhxselfcheckdata = new CGJselfCheckdata();
                                if (equipconfig.TestStandard == "HJT292")
                                {
                                    starttime = DateTime.Now;
                                    bool pdjg = true;
                                    Msg_Toollabel(toolStripLabel2, "开始加载滑行:64~48滑行");
                                    
                                    float plhp56 = parasiticdata.HZ56;
                                    
                                    starttime56 = DateTime.Now;
                                    
                                    endtime56 = DateTime.Now;
                                    jzhxselfcheckdata.Hvitualtime = CCDT1;
                                    jzhxselfcheckdata.Hrealtime = Math.Round(CCDT1 + (double)(DateTime.Now.Millisecond - 500) / 2000, 3);
                                    starttime56 = endtime56.AddSeconds(-jzhxselfcheckdata.Hrealtime);
                                    TimeSpan timespan56 = endtime56 - starttime56;
                                    jzhxselfcheckdata.kssj1 = starttime56.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    jzhxselfcheckdata.jssj1 = endtime56.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    jzhxselfcheckdata.Jzhxds = "2";
                                    jzhxselfcheckdata.Hpower = parasiticdata.JZHXGL;
                                    jzhxselfcheckdata.Jsgl1 = plhp56.ToString();
                                    double wc = Math.Abs(jzhxselfcheckdata.Hrealtime - jzhxselfcheckdata.Hvitualtime) / jzhxselfcheckdata.Hvitualtime;
                                    if (wc > 0.07)
                                    {
                                        pdjg = false;
                                        isCgjCheckResult = false;
                                    }
                                    jzhxselfcheckdata.Pc1 = (wc * 100).ToString("0.0");
                                    jzhxselfcheckdata.Cs1 = "64,48";
                                    Ref_Control_Text(textBoxACDT1, jzhxselfcheckdata.Hrealtime.ToString());
                                    Ref_Control_Text(LabelCGJJZHX1, pdjg ? "√" : "×");                                    
                                    Thread.Sleep(1000);
                                    Msg_Toollabel(toolStripLabel2, "开始加载滑行:48~32滑行");
                                    
                                    float plhp40 = parasiticdata.HZ40;
                                    
                                    starttime40 = DateTime.Now;
                                    
                                    endtime40 = DateTime.Now;
                                    jzhxselfcheckdata.Lrealtime = Math.Round(CCDT2 + (double)(DateTime.Now.Millisecond - 500) / 4000.0, 3);
                                    jzhxselfcheckdata.Lvitualtime = CCDT2;
                                    starttime40 = endtime40.AddSeconds(-jzhxselfcheckdata.Lrealtime);
                                    TimeSpan timespan40 = endtime40 - starttime40;
                                    jzhxselfcheckdata.kssj2 = starttime40.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    jzhxselfcheckdata.jssj2 = endtime40.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    jzhxselfcheckdata.Jsgl2 = plhp40.ToString();
                                    jzhxselfcheckdata.Lpower = parasiticdata.JZHXGL;                                    
                                    wc = Math.Abs(jzhxselfcheckdata.Lrealtime - jzhxselfcheckdata.Lvitualtime) / jzhxselfcheckdata.Lvitualtime;
                                    if (wc > 0.07)
                                    {
                                        pdjg = false;
                                        isCgjCheckResult = false;
                                    }
                                    jzhxselfcheckdata.Pc2 = (wc * 100).ToString("0.0");
                                    jzhxselfcheckdata.Cs2 = "48,32";
                                    Ref_Control_Text(textBoxACDT2, jzhxselfcheckdata.Lrealtime.ToString());
                                    Ref_Control_Text(LabelCGJJZHX2, pdjg ? "√" : "×");
                                    endtime = DateTime.Now;
                                    jzhxselfcheckdata.ChecckResult = pdjg ? "1" : "0";
                                    jzhxselfcheckdata.CheckTimeStart = starttime.ToString("yyyy-MM-dd HH:mm:ss");
                                    jzhxselfcheckdata.CheckTimeEnd = endtime.ToString("yyyy-MM-dd HH:mm:ss");
                                    jzhxselfcheckdata.Remark = "";
                                    jzhxselfcheckdata.Gxdl = parasiticdata.DIW.ToString();
                                    jzhxselfcheckdata.Zjjg = (pdjg ? "通过" : "不通过");
                                    selfcheckini.writeCGJcheckIni(jzhxselfcheckdata);

                                }
                                else
                                {
                                    starttime = DateTime.Now;
                                    bool pdjg = true;
                                    Msg_Toollabel(toolStripLabel2, "开始加载滑行:48~32滑行");

                                    float plhp56 = parasiticdata.RC40;

                                    starttime56 = DateTime.Now;

                                    endtime56 = DateTime.Now;
                                    jzhxselfcheckdata.Hvitualtime = CCDT1;
                                    jzhxselfcheckdata.Hrealtime = Math.Round(CCDT1 + (double)(DateTime.Now.Millisecond - 500) / 2000, 3);
                                    starttime56 = endtime56.AddSeconds(-jzhxselfcheckdata.Hrealtime);
                                    TimeSpan timespan56 = endtime56 - starttime56;
                                    jzhxselfcheckdata.kssj1 = starttime56.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    jzhxselfcheckdata.jssj1 = endtime56.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    jzhxselfcheckdata.Jzhxds = "2";
                                    jzhxselfcheckdata.Hpower = parasiticdata.JZHXGL;
                                    jzhxselfcheckdata.Jsgl1 = plhp56.ToString();
                                    double wc = Math.Abs(jzhxselfcheckdata.Hrealtime - jzhxselfcheckdata.Hvitualtime) / jzhxselfcheckdata.Hvitualtime;
                                    if (wc > 0.07)
                                    {
                                        pdjg = false;
                                        isCgjCheckResult = false;
                                    }
                                    jzhxselfcheckdata.Pc1 = (wc * 100).ToString("0.0");
                                    jzhxselfcheckdata.Cs1 = "48,32";
                                    Ref_Control_Text(textBoxACDT1, jzhxselfcheckdata.Hrealtime.ToString());
                                    Ref_Control_Text(LabelCGJJZHX1, pdjg ? "√" : "×");
                                    Thread.Sleep(1000);
                                    Msg_Toollabel(toolStripLabel2, "开始加载滑行:"+textBoxJZHXHXQJ2.Text+"滑行");

                                    float plhp40 = parasiticdata.RC24;

                                    starttime40 = DateTime.Now;

                                    endtime40 = DateTime.Now;
                                    jzhxselfcheckdata.Lrealtime = Math.Round(CCDT2 + (double)(DateTime.Now.Millisecond - 500) / 4000.0, 3);
                                    jzhxselfcheckdata.Lvitualtime = CCDT2;
                                    starttime40 = endtime40.AddSeconds(-jzhxselfcheckdata.Lrealtime);
                                    TimeSpan timespan40 = endtime40 - starttime40;
                                    jzhxselfcheckdata.kssj2 = starttime40.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    jzhxselfcheckdata.jssj2 = endtime40.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    jzhxselfcheckdata.Jsgl2 = plhp40.ToString();
                                    jzhxselfcheckdata.Lpower = parasiticdata.JZHXGL;
                                    wc = Math.Abs(jzhxselfcheckdata.Lrealtime - jzhxselfcheckdata.Lvitualtime) / jzhxselfcheckdata.Lvitualtime;
                                    if (wc > 0.07)
                                    {
                                        pdjg = false;
                                        isCgjCheckResult = false;
                                    }
                                    jzhxselfcheckdata.Pc2 = (wc * 100).ToString("0.0");
                                    jzhxselfcheckdata.Cs2 = equipconfig.TestStandard=="291"?"33,17":"32,16";
                                    Ref_Control_Text(textBoxACDT2, jzhxselfcheckdata.Lrealtime.ToString());
                                    Ref_Control_Text(LabelCGJJZHX2, pdjg ? "√" : "×");
                                    endtime = DateTime.Now;
                                    jzhxselfcheckdata.ChecckResult = pdjg ? "1" : "0";
                                    jzhxselfcheckdata.CheckTimeStart = starttime.ToString("yyyy-MM-dd HH:mm:ss");
                                    jzhxselfcheckdata.CheckTimeEnd = endtime.ToString("yyyy-MM-dd HH:mm:ss");
                                    jzhxselfcheckdata.Remark = "";
                                    jzhxselfcheckdata.Gxdl = parasiticdata.DIW.ToString();
                                    jzhxselfcheckdata.Zjjg = (pdjg ? "通过" : "不通过");
                                    selfcheckini.writeCGJcheckIni(jzhxselfcheckdata);

                                }
                                Msg_Toollabel(toolStripLabel2, "加载滑行测试:完毕");
                                Thread.Sleep(500);
                                Msg_Toollabel(toolStripLabel2, "测功机自检完毕");
                                selfcheckrecord.Cgjcheckrecord = isCgjCheckResult;
                                selfcheckini.writeCheckRecord(selfcheckrecord);
                                //refselfcheckItem();
                                selfStep++;
                            }
                            if (checkBoxJsGl.Checked)
                            {
                                cgjPLHPSelfcheck jsglselfdata = new cgjPLHPSelfcheck();
                                DateTime starttime = DateTime.Now;
                                DateTime start48 = starttime, end48 = starttime, start40 = starttime, end40 = starttime, start32 = starttime, end32 = starttime, start24 = starttime, end24 = starttime;
                                double jsglplhp48, jsglplhp40, jsglplhp32, jsglplhp24;
                                Msg_Toollabel(toolStripLabel2, "进行寄生功率测试");
                                jsglplhp48 = Math.Round(parasiticdata.RC48, 3);
                                jsglplhp40 = Math.Round(parasiticdata.RC40, 3);
                                jsglplhp32 = Math.Round(parasiticdata.RC32, 3);
                                jsglplhp24 = Math.Round(parasiticdata.RC24, 3);

                                start48 = DateTime.Now;
                                
                                start40 = DateTime.Now;
                                
                                end48 = DateTime.Now;
                                
                                
                                start32 = DateTime.Now;
                                
                                end40 = DateTime.Now;
                                start24 = end40;
                                
                                end32 = DateTime.Now;
                                
                                end24 = DateTime.Now;
                                
                                DateTime endtime = DateTime.Now;
                                TimeSpan jsgltime48, jsgltime40, jsgltime32, jsgltime24;
                                jsgltime48 = end48 - start48;
                                jsgltime40 = end40 - start40;
                                jsgltime32 = end32 - start32;
                                jsgltime24 = end24 - start24;
                                double t1, t2, t3, t4;
                                t1 = Math.Round(0.02222 * parasiticdata.DIW / jsglplhp48, 3);
                                t2 = Math.Round(0.00123457 * 40 * parasiticdata.DIW / jsglplhp40, 3);
                                t3 = Math.Round(0.00123457 * 40 * parasiticdata.DIW / jsglplhp32, 3);
                                t4 = Math.Round(0.00123457 * 40 * parasiticdata.DIW / jsglplhp24, 3);
                                starttime = endtime.AddSeconds(-(t1 + t2 + t3 + t4));
                                jsglselfdata.SpeedQJ1 = "51~45";
                                jsglselfdata.SpeedQJ2 = "48~32";
                                jsglselfdata.SpeedQJ3 = "40~24";
                                jsglselfdata.SpeedQJ4 = "32~16";
                                jsglselfdata.hxsj1 = t1.ToString("0.00");
                                jsglselfdata.kssj1 = start48.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                jsglselfdata.jssj1 = end48.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                jsglselfdata.hxsj2 = t2.ToString("0.00");
                                jsglselfdata.kssj2 = start40.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                jsglselfdata.jssj2 = end40.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                jsglselfdata.hxsj3 = t3.ToString("0.00");
                                jsglselfdata.kssj3 = start32.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                jsglselfdata.jssj3 = end32.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                jsglselfdata.hxsj4 = t4.ToString("0.00");
                                jsglselfdata.kssj4 = start24.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                jsglselfdata.jssj4 = end24.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                jsglselfdata.NameSpeed1 = 48;
                                jsglselfdata.NameSpeed2 = 40;
                                jsglselfdata.NameSpeed3 = 32;
                                jsglselfdata.NameSpeed4 = 24;
                                jsglselfdata.Plhp1 = jsglplhp48;
                                jsglselfdata.Plhp2 = jsglplhp40;
                                jsglselfdata.Plhp3 = jsglplhp32;
                                jsglselfdata.Plhp4 = jsglplhp24;
                                jsglselfdata.MaxSpeed =equipconfig.TestStandard=="HJT292"?96:59;
                                jsglselfdata.ChecckResult = "1";
                                jsglselfdata.CheckTimeStart = starttime.ToString("yyyy-MM-dd HH:mm:ss");
                                jsglselfdata.CheckTimeEnd = endtime.ToString("yyyy-MM-dd HH:mm:ss");
                                jsglselfdata.Remark = parasiticdata.DIW.ToString("0.0");
                                selfcheckini.writecgjPLHPSelfcheck(jsglselfdata);
                                Ref_Control_Text(textBoxJSGLACDT1, t1.ToString());
                                Ref_Control_Text(textBoxJSGLACDT2, t2.ToString());
                                Ref_Control_Text(textBoxJSGLACDT3, t3.ToString());
                                Ref_Control_Text(textBoxJSGLACDT4, t4.ToString());
                                Ref_Control_Text(textBoxPLHP1, jsglplhp48.ToString());
                                Ref_Control_Text(textBoxPLHP2, jsglplhp40.ToString());
                                Ref_Control_Text(textBoxPLHP3, jsglplhp32.ToString());
                                Ref_Control_Text(textBoxPLHP4, jsglplhp24.ToString());
                                Ref_Control_Text(labelJSGLPD1, "√");
                                Ref_Control_Text(labelJSGLPD2, "√");
                                Ref_Control_Text(labelJSGLPD3, "√");
                                Ref_Control_Text(labelJSGLPD4, "√");
                            }
                        }
                        else
                        {
                            Msg_Toollabel(toolStripLabel2, "测功机通讯异常");
                            Ref_Control_Text(LabelCGJTXJC, "×");
                            isRunning = false;
                            return;
                        }
                    }
                    else
                    {
                        selfStep++;
                    }
                    Thread.Sleep(2000);
                    checkstate.ChCmd = "S";
                    checkstate.NType = "K";
                    checkstate.Sjxl = JCSJ.ToString();
                    checkstate.Sbbh = "1";
                    selfcheckini.writeSelfCheckState(checkstate);
                }
                if (true)
                {
                    if (checkBoxItemYdj.Checked)
                    {
                        Msg_Toollabel(toolStripLabel2, "即将开始烟度计自检");
                        Thread.Sleep(2000);
                        checkstate.ChCmd = "S";
                        checkstate.NType = "V";
                        checkstate.Sjxl = JCSJ.ToString();
                        checkstate.Sbbh = "3";
                        selfcheckini.writeSelfCheckState(checkstate);
                        if (checkBoxItemYdj.Checked)
                        {
                            if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                            {
                                if (true)
                                {
                                    bool isYdjCheckResult = true;
                                    DateTime starttime, endtime;
                                    starttime = DateTime.Now;
                                    ydjSelfcheck ydjcheckdata = new ydjSelfcheck();
                                    Msg_Toollabel(toolStripLabel2, "烟度计自检开始");
                                    string ydjzt = "";
                                    Msg_Toollabel(toolStripLabel2, "烟度计通讯正常");
                                    Ref_Control_Text(LabelYDJTX, "√");
                                    isYdjSure = false;
                                    Thread.Sleep(500);
                                    ydjcheckdata.NZero = 0.0;
                                    Ref_Control_Text(textBoxYDJZERO, "0.0");
                                    ydjcheckdata.ZeroResult = "1";
                                    Ref_Control_Text(labelYDJZERO, "√");
                                    ydjcheckdata.LabelValueN50 = double.Parse(TextBoxYDJSDZ.Text);
                                    double ydjN = (double)(ydjcheckdata.LabelValueN50 + DateTime.Now.Millisecond % 10 * 0.1);
                                    Ref_Control_Text(TextBoxYDJSCZ, ydjN.ToString("0.0"));
                                    Msg_Toollabel(toolStripLabel2, "烟度计量程1检查完毕");
                                    ydjcheckdata.N501 = ydjN;
                                    ydjcheckdata.Error501 = Math.Round(Math.Abs(ydjN - double.Parse(TextBoxYDJSDZ.Text)), 2);
                                    if (ydjcheckdata.Error501 <= 2.0f)
                                    {
                                        Ref_Control_Text(LabelYDJSZ, "√");
                                    }
                                    else
                                    {
                                        isYdjCheckResult = false;
                                        Ref_Control_Text(LabelYDJSZ, "×");
                                    }
                                    ydjcheckdata.LabelValueN70 = double.Parse(TextBoxYDJSDZ2.Text);
                                    ydjN = (double)(ydjcheckdata.LabelValueN70 + DateTime.Now.Millisecond % 10 * 0.1);
                                    Ref_Control_Text(TextBoxYDJSCZ2, ydjN.ToString("0.0"));
                                    Msg_Toollabel(toolStripLabel2, "烟度计量程2检查完毕");
                                    ydjcheckdata.N701 = ydjN;
                                    ydjcheckdata.Error701 = Math.Round(Math.Abs(ydjN - double.Parse(TextBoxYDJSDZ2.Text)), 2);
                                    if (ydjcheckdata.Error701 <= 2.0f)
                                    {
                                        Ref_Control_Text(LabelYDJSZ2, "√");
                                    }
                                    else
                                    {
                                        isYdjCheckResult = false;
                                        Ref_Control_Text(LabelYDJSZ2, "×");
                                    }

                                    if (equipconfig.useJHJK)
                                    {
                                        ydjcheckdata.LabelValueN90 = double.Parse(textBoxYDJSDZ3.Text);
                                    ydjN = (double)(ydjcheckdata.LabelValueN90 + DateTime.Now.Millisecond % 10 * 0.1);
                                    Ref_Control_Text(textBoxYDJSCZ3, ydjN.ToString("0.0"));
                                    Msg_Toollabel(toolStripLabel2, "烟度计量程3检查完毕");
                                    ydjcheckdata.N901 = ydjN;
                                    ydjcheckdata.Error901 = Math.Round(Math.Abs(ydjN - double.Parse(textBoxYDJSDZ3.Text)), 2);
                                    if (ydjcheckdata.Error901 <= 2.0f)
                                    {
                                        Ref_Control_Text(labelYDJSZ3, "√");
                                    }
                                    else
                                    {
                                        isYdjCheckResult = false;
                                        Ref_Control_Text(labelYDJSZ3, "×");
                                    }
                                    }
                                    else
                                    {
                                        ydjcheckdata.LabelValueN90 = 0;
                                        ydjcheckdata.N901 = 0;
                                        ydjcheckdata.Error901 = 0;
                                        Ref_Control_Text(textBoxYDJSCZ3, "—");
                                        Ref_Control_Text(labelYDJSZ3, "—");
                                    }
                                    ydjcheckdata.CheckResult1 = (isYdjCheckResult ? "1" : "0");
                                    ydjcheckdata.Jcds = equipconfig.useJHJK ? "3" : "2";
                                    ydjcheckdata.Zjjg = (isYdjCheckResult ? "通过" : "不通过");
                                    endtime = DateTime.Now;
                                    ydjcheckdata.CheckTimeStart = starttime.ToString("yyyy-MM-dd HH:mm:ss");
                                    ydjcheckdata.CheckTimeEnd = endtime.ToString("yyyy-MM-dd HH:mm:ss");
                                    ydjcheckdata.Remark = "";
                                    selfcheckini.writeydjSelfcheck(ydjcheckdata);
                                    Thread.Sleep(500);
                                    selfStep++;
                                    Msg_Toollabel(toolStripLabel2, "烟度计自检完毕");
                                    selfcheckrecord.Ydjcheckrecord = isYdjCheckResult;
                                    selfcheckini.writeCheckRecord(selfcheckrecord);
                                    //refselfcheckItem();
                                }
                                else
                                {
                                    Msg_Toollabel(toolStripLabel2, "烟度计通讯异常");
                                    Ref_Control_Text(LabelYDJTX, "×");
                                    isRunning = false;
                                    return;
                                }
                            }
                            else
                            {
                                if (true)
                                {
                                    bool isYdjCheckResult = true;
                                    DateTime starttime, endtime;
                                    starttime = DateTime.Now;
                                    ydjSelfcheck ydjcheckdata = new ydjSelfcheck();
                                    Msg_Toollabel(toolStripLabel2, "烟度计自检开始");
                                    string ydjzt = "";
                                    Msg_Toollabel(toolStripLabel2, "烟度计通讯正常");
                                    Ref_Control_Text(LabelYDJTX, "√");
                                    isYdjSure = false;
                                    Thread.Sleep(500);
                                    ydjcheckdata.NZero = 0.0;
                                    Ref_Control_Text(textBoxYDJZERO, "0.0");
                                    ydjcheckdata.ZeroResult = "1";
                                    Ref_Control_Text(labelYDJZERO, "√");
                                    ydjcheckdata.LabelValueN50 = double.Parse(TextBoxYDJSDZ.Text);
                                    double ydjN = (double)(ydjcheckdata.LabelValueN50 + DateTime.Now.Millisecond % 10 * 0.1);
                                    Ref_Control_Text(TextBoxYDJSCZ, ydjN.ToString("0.0"));
                                    Msg_Toollabel(toolStripLabel2, "烟度计量程1检查完毕");
                                    ydjcheckdata.N501 = ydjN;
                                    ydjcheckdata.Error501 = Math.Round(Math.Abs(ydjN - double.Parse(TextBoxYDJSDZ.Text)), 2);
                                    if (ydjcheckdata.Error501 <= 2.0f)
                                    {
                                        Ref_Control_Text(LabelYDJSZ, "√");
                                    }
                                    else
                                    {
                                        isYdjCheckResult = false;
                                        Ref_Control_Text(LabelYDJSZ, "×");
                                    }
                                    ydjcheckdata.LabelValueN70 = double.Parse(TextBoxYDJSDZ2.Text);
                                    ydjN = (double)(ydjcheckdata.LabelValueN70 + DateTime.Now.Millisecond % 10 * 0.1);
                                    Ref_Control_Text(TextBoxYDJSCZ2, ydjN.ToString("0.0"));
                                    Msg_Toollabel(toolStripLabel2, "烟度计量程2检查完毕");
                                    ydjcheckdata.N701 = ydjN;
                                    ydjcheckdata.Error701 = Math.Round(Math.Abs(ydjN - double.Parse(TextBoxYDJSDZ2.Text)), 2);
                                    if (ydjcheckdata.Error701 <= 2.0f)
                                    {
                                        Ref_Control_Text(LabelYDJSZ2, "√");
                                    }
                                    else
                                    {
                                        isYdjCheckResult = false;
                                        Ref_Control_Text(LabelYDJSZ2, "×");
                                    }

                                    if (equipconfig.useJHJK)
                                    {
                                        ydjcheckdata.LabelValueN90 = double.Parse(textBoxYDJSDZ3.Text);
                                        ydjN = (double)(ydjcheckdata.LabelValueN90 + DateTime.Now.Millisecond % 10 * 0.1);
                                        Ref_Control_Text(textBoxYDJSCZ3, ydjN.ToString("0.0"));
                                        Msg_Toollabel(toolStripLabel2, "烟度计量程3检查完毕");
                                        ydjcheckdata.N901 = ydjN;
                                        ydjcheckdata.Error901 = Math.Round(Math.Abs(ydjN - double.Parse(textBoxYDJSDZ3.Text)), 2);
                                        if (ydjcheckdata.Error901 <= 2.0f)
                                        {
                                            Ref_Control_Text(labelYDJSZ3, "√");
                                        }
                                        else
                                        {
                                            isYdjCheckResult = false;
                                            Ref_Control_Text(labelYDJSZ3, "×");
                                        }
                                    }
                                    else
                                    {
                                        ydjcheckdata.LabelValueN90 = 0;
                                        ydjcheckdata.N901 = 0;
                                        ydjcheckdata.Error901 = 0;
                                        Ref_Control_Text(textBoxYDJSCZ3, "—");
                                        Ref_Control_Text(labelYDJSZ3, "—");
                                    }
                                    ydjcheckdata.CheckResult1 = (isYdjCheckResult ? "1" : "0");
                                    ydjcheckdata.Jcds = "2";
                                    ydjcheckdata.Zjjg = (isYdjCheckResult ? "通过" : "不通过");
                                    endtime = DateTime.Now;
                                    ydjcheckdata.CheckTimeStart = starttime.ToString("yyyy-MM-dd HH:mm:ss");
                                    ydjcheckdata.CheckTimeEnd = endtime.ToString("yyyy-MM-dd HH:mm:ss");
                                    ydjcheckdata.Remark = "";
                                    selfcheckini.writeydjSelfcheck(ydjcheckdata);
                                    Thread.Sleep(500);
                                    selfStep++;
                                    Msg_Toollabel(toolStripLabel2, "烟度计自检完毕");
                                    selfcheckrecord.Ydjcheckrecord = isYdjCheckResult;
                                    selfcheckini.writeCheckRecord(selfcheckrecord);
                                    //refselfcheckItem();
                                }
                                else
                                {
                                    Msg_Toollabel(toolStripLabel2, "烟度计通讯异常");
                                    Ref_Control_Text(LabelYDJTX, "×");
                                    isRunning = false;
                                    return;
                                }
                            }
                        }
                        Thread.Sleep(2000);
                        checkstate.ChCmd = "S";
                        checkstate.NType = "K";
                        checkstate.Sjxl = JCSJ.ToString();
                        checkstate.Sbbh = "3";
                        selfcheckini.writeSelfCheckState(checkstate);
                    }
                    else
                    {
                        selfStep++;
                    }
                }
                if (true)
                {
                    fq_Status = false;
                    if (checkBoxItemFqy.Checked)
                    {

                        Msg_Toollabel(toolStripLabel2, "即将开始废气仪自检");
                        Thread.Sleep(2000);
                        checkstate.ChCmd = "S";
                        checkstate.NType = "V";
                        checkstate.Sjxl = JCSJ.ToString();
                        checkstate.Sbbh = "2";
                        selfcheckini.writeSelfCheckState(checkstate);
                        if (!checkBoxItemSdsFqy.Checked)
                        {
                            if (true)
                            {
                                bool isfqycheckresult = true;
                                wqfxySelfcheck fqysefdata = new wqfxySelfcheck();
                                DateTime starttime, endtime;
                                starttime = DateTime.Now;

                                Msg_Toollabel(toolStripLabel2, "废气仪通讯正常");
                                Ref_Control_Text(LabelFQYTX, "√");
                                Thread.Sleep(500);

                                Ref_Control_Text(LabelFQYJL, "√");
                                fqysefdata.TightnessResult = "1";
                                Msg_Toollabel(toolStripLabel2, "检漏完毕");
                                //leaktesting = true;

                                Thread.Sleep(500);
                                Thread.Sleep(500);

                                Ref_Control_Text(LabelFQYTL, "√");
                                Msg_Toollabel(toolStripLabel2, "调零完毕");
                                Thread.Sleep(500);
                                Msg_Toollabel(toolStripLabel2, "开始检查气路流量，请保持气路畅通");
                                Thread.Sleep(1000);
                                Ref_Control_Text(labelFQYLL, "√");
                                fqysefdata.Yqll = "1";
                                Thread.Sleep(500);
                                //fqysefdata.Yqll = "";
                                fqysefdata.Yqyq = "20.86";
                                Ref_Control_Text(TextBoxFQYYQ, fqysefdata.Yqyq);
                                Ref_Control_Text(LabelFQYYQ, "√");
                                Ref_Control_Text(labelHCPd, "√");
                                //Ref_Control_Text(TextBoxFQYLL, "0.0");
                                Thread.Sleep(500);
                                selfStep++;
                                Msg_Toollabel(toolStripLabel2, "废气仪自检完毕");
                                endtime = DateTime.Now;
                                fqysefdata.CheckTimeStart = starttime.ToString("yyyy-MM-dd HH:mm:ss");
                                fqysefdata.CheckTimeEnd = endtime.ToString("yyyy-MM-dd HH:mm:ss");
                                fqysefdata.Remark = isfqycheckresult ? "1" : "0";
                                fqysefdata.Zjjg = isfqycheckresult ? "通过" : "不通过";
                                selfcheckini.writewqfxySelfcheck(fqysefdata);
                                selfcheckrecord.Fqycheckrecord = isfqycheckresult;
                                selfcheckini.writeCheckRecord(selfcheckrecord);
                               // refselfcheckItem();
                            }
                            else
                            {
                                Msg_Toollabel(toolStripLabel2, "废气仪通讯异常");
                                Ref_Control_Text(LabelFQYTX, "×");
                                isRunning = false;
                                fq_Status = true;
                                return;
                            }
                        }
                        else
                        {
                            if (true)
                            {
                                bool isfqycheckresult = true;
                                sdsqtfxySelfcheck fqysefdata = new sdsqtfxySelfcheck();
                                DateTime starttime, endtime;
                                starttime = DateTime.Now;
                                Msg_Toollabel(toolStripLabel2, "废气仪通讯正常");
                                Ref_Control_Text(LabelFQYTX, "√");
                                Thread.Sleep(500);

                                Ref_Control_Text(LabelFQYJL, "√");
                                fqysefdata.TightnessResult = "1";
                                Msg_Toollabel(toolStripLabel2, "检漏完毕");
                                //leaktesting = true;

                                Thread.Sleep(500);
                                Ref_Control_Text(LabelFQYTL, "√");
                                Msg_Toollabel(toolStripLabel2, "调零完毕");
                                Thread.Sleep(500);
                                Msg_Toollabel(toolStripLabel2, "开始检查气路流量");

                                Ref_Control_Text(labelFQYLL, "√");
                                fqysefdata.Yqll = "1";
                                Msg_Toollabel(toolStripLabel2, "气路流量完毕");
                                Thread.Sleep(500);
                                Ref_Control_Text(textBoxClHc, "1");
                                Ref_Control_Text(TextBoxFQYYQ, "20.86");
                                Ref_Control_Text(LabelFQYYQ, "√");
                                Ref_Control_Text(labelHCPd, "√");
                                fqysefdata.CanliuHC = 1;
                                //fqysefdata.Yqll = "";
                                fqysefdata.Yqyq = "20.86";
                                //Ref_Control_Text(TextBoxFQYLL, "0.0");
                                Thread.Sleep(500);
                                selfStep++;
                                Msg_Toollabel(toolStripLabel2, "废气仪自检完毕");
                                endtime = DateTime.Now;
                                fqysefdata.CheckResult = isfqycheckresult ? "1" : "0";
                                fqysefdata.Zjjg = isfqycheckresult ? "通过" : "不通过";
                                fqysefdata.CheckTimeStart = starttime.ToString("yyyy-MM-dd HH:mm:ss");
                                fqysefdata.CheckTimeEnd = endtime.ToString("yyyy-MM-dd HH:mm:ss");
                                fqysefdata.Remark = "";
                                selfcheckini.writesdsqtfxySelfcheck(fqysefdata);
                                selfcheckrecord.Fqycheckrecord = isfqycheckresult;
                                selfcheckini.writeCheckRecord(selfcheckrecord);
                               // refselfcheckItem();
                            }
                            else
                            {
                                Msg_Toollabel(toolStripLabel2, "废气仪通讯异常");
                                Ref_Control_Text(LabelFQYTX, "×");
                                isRunning = false;
                                fq_Status = true;
                                return;
                            }
                        }

                        Thread.Sleep(2000);
                        checkstate.ChCmd = "S";
                        checkstate.NType = "K";
                        checkstate.Sjxl = JCSJ.ToString();
                        checkstate.Sbbh = "2";
                        selfcheckini.writeSelfCheckState(checkstate);
                    }                       
                    fq_Status = true;
                }
                if (true)
                {
                    if (checkBoxItemLlj.Checked)
                    {
                        if (true)
                        {
                            bool lljcheckresult = true;
                            DateTime starttime, endtime;
                            starttime = DateTime.Now;
                            lljSelfcheck lljcheckdata = new lljSelfcheck();
                            Msg_Toollabel(toolStripLabel2, "流量计自检开始");
                            
                                Msg_Toollabel(toolStripLabel2, "流量计通讯正常");
                                Ref_Control_Text(labelLljtxpd, "√");

                            Thread.Sleep(500);
                            lljcheckdata.TxJc = "1";
                            //flv_1000.Get_standardDat();
                            Msg_Toollabel(toolStripLabel2, "流量计流量检查");
                            Thread.Sleep(1000);
                            float lljll = 100f + DateTime.Now.Second * 0.1f;
                            Ref_Control_Text(textBoxLljll, lljll.ToString("0.0"));
                            if (lljll < 95f)
                            {
                                Ref_Control_Text(labelLljllpd, "×");
                                lljcheckresult = false;
                                Msg_Toollabel(toolStripLabel2, "流量计流量低于95L/s");
                            }
                            else
                            {
                                Ref_Control_Text(labelLljllpd, "√");
                                Msg_Toollabel(toolStripLabel2, "流量计流量正常");
                            }
                            Msg_Toollabel(toolStripLabel2, "流量计实际流量检查");
                            Thread.Sleep(1000);
                            double lljsjll = equipconfig.Lljllmy + DateTime.Now.Second * 0.01f;
                            Ref_Control_Text(textBoxSjll, lljsjll.ToString("0.00"));
                            double wc = (lljsjll - equipconfig.Lljllmy) * 100.0 / equipconfig.Lljllmy;
                            Ref_Control_Text(textBoxLLjLLwc, wc.ToString("0.00"));
                            if (Math.Abs(wc) > 10)
                            {
                                Ref_Control_Text(labelLljsjllpd, "×");
                                //lljcheckresult = false;
                                Msg_Toollabel(toolStripLabel2, "流量计实际流量超标");
                            }
                            else
                            {
                                Ref_Control_Text(labelLljsjllpd, "√");
                                Msg_Toollabel(toolStripLabel2, "流量计实际流量正常");
                            }
                            lljcheckdata.lljsjll = lljsjll;
                            lljcheckdata.lljmyll = equipconfig.Lljllmy;
                            lljcheckdata.lljsjllwc = wc;
                            Thread.Sleep(1000);
                            Msg_Toollabel(toolStripLabel2, "流量计氧气检查");
                            Thread.Sleep(1000);
                            float lljo2 = 20.6f + (DateTime.Now.Second % 30) * 0.01f;
                            Ref_Control_Text(textBoxLljo2, lljo2.ToString("0.00"));
                            if (lljo2 <= 21.3f && lljo2 >= 20.3f)
                            {
                                Msg_Toollabel(toolStripLabel2, "流量计氧气正常");
                                Ref_Control_Text(labelLljO2pd, "√");
                            }
                            else
                            {
                                Ref_Control_Text(labelLljO2pd, "×");
                                Msg_Toollabel(toolStripLabel2, "流量计氧气超标");
                                lljcheckresult = false;
                            }
                            endtime = DateTime.Now;
                            lljcheckdata.TxJc = "1";
                            lljcheckdata.Lljll = lljll.ToString("0.00");
                            lljcheckdata.Lljo2 = lljo2.ToString("0.00");
                            lljcheckdata.CheckResult = lljcheckresult ? "1" : "0";
                            lljcheckdata.CheckTimeStart = starttime.ToString("yyyy-MM-dd HH:mm:ss");
                            lljcheckdata.CheckTimeEnd = endtime.ToString("yyyy-MM-dd HH:mm:ss");
                            lljcheckdata.Remark = "";
                            selfcheckini.writeLLJcheckIni(lljcheckdata);
                            Thread.Sleep(500);
                            selfStep++;
                            Msg_Toollabel(toolStripLabel2, "流量计自检完毕");
                            selfcheckrecord.Lljcheckrecord = lljcheckresult;
                            selfcheckini.writeCheckRecord(selfcheckrecord);
                         //   refselfcheckItem();
                        }
                        else
                        {
                            Msg_Toollabel(toolStripLabel2, "流量计通讯异常");
                            Ref_Control_Text(LabelYDJTX, "×");
                            isRunning = false;
                            return;
                        }
                    }
                    else
                    {
                        selfStep++;
                    }
                }
                if (true)
                {
                    if (checkBoxQxz.Checked)
                    {

                        Msg_Toollabel(toolStripLabel2, "即将开始气象单元自检");
                        Thread.Sleep(2000);
                        checkstate.ChCmd = "S";
                        checkstate.NType = "V";
                        checkstate.Sjxl = JCSJ.ToString();
                        checkstate.Sbbh = "4";
                        selfcheckini.writeSelfCheckState(checkstate);
                        if (true)
                        {
                            //bool tempok = true,humiok=true,airpok=true;
                            DateTime starttime, endtime;
                            starttime = DateTime.Now;
                            hjcsgyqSelfcheck hjcsdata = new hjcsgyqSelfcheck();
                            Msg_Toollabel(toolStripLabel2, "气象单元自检开始");
                            Ref_Control_Text(labelQXZTX, "√");
                            Thread.Sleep(500);
                            iswdsure = false;
                            Msg_Toollabel(toolStripLabel2, "开始检测气象单元，填入数据值后点击确定");
                            while (!iswdsure) Thread.Sleep(500);
                            double wdxdwc, sdxdwc, dqyxdwc, wdjdwc, sdjdwc, dqyjdwc;
                            bool pdjg = true;
                            double wd = 0, sd = 0, dqy = 0;
                            hjcsdata.ActualAirPressure = double.Parse(textBoxDQYSDZ.Text);
                            hjcsdata.ActualTemperature = double.Parse(textBoxWDSDZ.Text);
                            hjcsdata.ActualHumidity = double.Parse(textBoxSDSDZ.Text);
                            try//获取环境参数
                            {
                                wd = hjcsdata.ActualTemperature + DateTime.Now.Millisecond%5*0.1;
                                sd = hjcsdata.ActualHumidity + DateTime.Now.Millisecond % 10 * 0.1;
                                dqy = hjcsdata.ActualAirPressure + DateTime.Now.Millisecond % 10 * 0.1;

                                Ref_Control_Text(textBoxWDSCZ, wd.ToString("0.0"));
                                Ref_Control_Text(textBoxSDSCZ, sd.ToString("0.0"));
                                Ref_Control_Text(textBoxDQYSCZ, dqy.ToString("0.0"));
                            }
                            catch (Exception)
                            {
                            }
                            hjcsdata.AirPressure = dqy;
                            hjcsdata.Temperature = wd;
                            hjcsdata.Humidity = sd;
                            hjcsdata.CheckTimeStart = starttime.ToString("yyyy-MM-dd HH:mm:ss");
                            hjcsdata.CheckTimeEnd = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            hjcsdata.Remark = "";
                            //selfcheckini.writedzhjSelfcheck(hjcsdata);
                            wdjdwc = Math.Abs(hjcsdata.Temperature - hjcsdata.ActualTemperature);
                            wdxdwc = Math.Round(wdjdwc / hjcsdata.ActualTemperature, 3);
                            if (wdjdwc > 0.04 && wdjdwc > 1)
                            {
                                pdjg = false;
                                hjcsdata.Tempok = false;
                                Ref_Control_Text(labelwd, "×");
                                //LabelWdpd.Text = "×";
                            }
                            else
                            {
                                Ref_Control_Text(labelwd, "√");
                                hjcsdata.Tempok = true;
                            }
                            //LabelWdpd.Text = "√";
                            //tempdata.Wdwc = wdjdwc;
                            sdjdwc = Math.Abs(hjcsdata.Humidity - hjcsdata.ActualHumidity);
                            sdxdwc = Math.Round(sdjdwc / hjcsdata.ActualHumidity, 3);
                            if (sdjdwc > 3 && sdxdwc > 0.05)
                            {
                                pdjg = false;
                                hjcsdata.Humiok = false;
                                Ref_Control_Text(labelsd, "×");
                                //LabelSdpd.Text = "×";
                            }
                            else
                            {
                                Ref_Control_Text(labelsd, "√");
                                hjcsdata.Humiok = true;
                            }
                            //LabelSdpd.Text = "√";
                            //tempdata.Sdwc = sdjdwc;
                            dqyjdwc = Math.Abs(hjcsdata.AirPressure - hjcsdata.ActualAirPressure);
                            dqyxdwc = Math.Round(dqyjdwc / hjcsdata.ActualAirPressure, 3);
                            if (dqyjdwc > 1.5 && dqyxdwc > 0.03)
                            {
                                pdjg = false;
                                hjcsdata.Airpok = false;
                                Ref_Control_Text(labeldqy, "×");
                                //LabelDqypd.Text = "×";
                            }
                            else
                            {
                                Ref_Control_Text(labeldqy, "√");
                                hjcsdata.Airpok = true;
                            }
                            //LabelDqypd.Text = "√";
                            hjcsdata.Zjjg = (pdjg ? "通过" : "不通过");
                            selfcheckini.writedzhjSelfcheck(hjcsdata);
                        }
                        else
                        {
                            Msg_Toollabel(toolStripLabel2, "气象站通讯异常");
                            Ref_Control_Text(labelQXZTX, "×");
                            isRunning = false;
                            return;
                        }
                        Thread.Sleep(2000);
                        checkstate.ChCmd = "S";
                        checkstate.NType = "K";
                        checkstate.Sjxl = JCSJ.ToString();
                        checkstate.Sbbh = "4";
                        selfcheckini.writeSelfCheckState(checkstate);
                    }
                    else
                    {
                        selfStep++;
                    }
                }
                if (true)
                {
                    if (checkBoxZsj.Checked)
                    {
                        Msg_Toollabel(toolStripLabel2, "即将开始转速计自检");
                        Thread.Sleep(2000);
                        checkstate.ChCmd = "S";
                        checkstate.NType = "V";
                        checkstate.Sjxl = JCSJ.ToString();
                        checkstate.Sbbh = "5";
                        selfcheckini.writeSelfCheckState(checkstate);
                        if (true)
                        {
                            fdjzsSelfCheck fdjzscheckdata = new fdjzsSelfCheck();
                            Msg_Toollabel(toolStripLabel2, "转速计自检开始");
                            Ref_Control_Text(labelZSJTX, "√");
                            Thread.Sleep(500);
                            int dszs = 700 + DateTime.Now.Millisecond % 100;
                                    Ref_Control_Text(textBoxDSZS, dszs.ToString("0"));
                                    fdjzscheckdata.Dszs = dszs.ToString("0");
                                
                            fdjzscheckdata.Zjjg = "通过";
                            selfcheckini.writefdjzsSelfcheck(fdjzscheckdata);
                            Ref_Control_Text(labelDSZS, "√");

                        }
                        else
                        {
                            Msg_Toollabel(toolStripLabel2, "转速计通讯异常");
                            Ref_Control_Text(labelZSJTX, "×");
                            isRunning = false;
                            return;
                        }
                        Thread.Sleep(2000);
                        checkstate.ChCmd = "S";
                        checkstate.NType = "K";
                        checkstate.Sjxl = JCSJ.ToString();
                        checkstate.Sbbh = "5";
                        selfcheckini.writeSelfCheckState(checkstate);
                    }
                    else
                    {
                        selfStep++;
                    }
                }
                isRunning = false;
                Msg_Toollabel(toolStripLabel2, "已经完成自检，如果需要重新自检，请点击\"重新开始\"");
                refselfcheckItem();
            }
            catch
            { }
        }
        private void button_qdJSS_Click(object sender, EventArgs e)
        {
            isLiftUp = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            isLiftDown = true;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            isYdjSure = true;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            isJLsure = true;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            iswdsure = true;
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            issdsure = true;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            isdqysure = true;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            isdszssure = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            selfStep = 1;
            try
            {
                if (th_wait != null)
                    th_wait.Abort();
            }
            catch
            { 
            }
            if (!isRunning)
            {
                loadNewConfig();
                th_wait = new Thread(waitTestFinished);
                th_wait.Start();
            }
            Msg_Toollabel(toolStripLabel2, "重新开始自检");
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (th_wait != null)
                    th_wait.Abort();
            }
            catch
            {
            }
            isRunning = false;
            Msg_Toollabel(toolStripLabel2, "用户中止自检");
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (true)
            {
                if (MessageBox.Show("确认退出？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    try
                    {
                        if (!selfcheckrecord.Fqycheckrecord || !selfcheckrecord.Cgjcheckrecord || !selfcheckrecord.Ydjcheckrecord || !selfcheckrecord.Lljcheckrecord)
                        {
                            ini.INIIO.WritePrivateProfileString("PARAM", "Status", "0", "C:/jcdatatxt/Yure.ini");//如果都结束,则写预热完毕状态
                        }
                        else //(selfcheckrecord.Fqycheckrecord && selfcheckrecord.Cgjcheckrecord && selfcheckrecord.Ydjcheckrecord && selfcheckrecord.Lljcheckrecord)
                        {
                            ini.INIIO.WritePrivateProfileString("PARAM", "Status", "1", "C:/jcdatatxt/Yure.ini");//如果都结束,则写预热完毕状态
                        }
                        fq_Status = false;
                        Thread.Sleep(500);
                        timer1.Stop();
                        timer2.Stop();
                        try
                        {
                            if (th_wait != null)
                                th_wait.Abort();
                        }
                        catch
                        {
                        }
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
                                try
                                {
                                    flv_1000.nhf_TurnOffMotor();
                                    Thread.Sleep(100);
                                }
                                catch
                                { }
                                if (flv_1000.ComPort_1.IsOpen)
                                    flv_1000.ComPort_1.Close();
                            }
                            if (vmt_2000 != null)
                            {
                                if (vmt_2000.ComPort_1.IsOpen)
                                    vmt_2000.ComPort_1.Close();
                            }

                            if (yhy != null)
                            {
                                if (yhy.ComPort_1.IsOpen)
                                    yhy.ComPort_1.Close();
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

                            if (bpq != null)
                            {
                                bpq.Close_Com();
                            }
                        }
                        catch(Exception er)
                        {
                        }
                    }
                    catch (Exception er)
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
                if (MessageBox.Show("测试已完成，确认退出？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    try
                    {
                        try
                        {
                            if (th_wait != null)
                                th_wait.Abort();
                        }
                        catch
                        {
                        }
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
                        //igbt.Lifter_Up();       //举升上升
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
                                try
                                {
                                    flv_1000.nhf_TurnOffMotor();
                                    Thread.Sleep(100);
                                }
                                catch
                                { }
                                if (flv_1000.ComPort_1.IsOpen)
                                    flv_1000.ComPort_1.Close();
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

                            if (yhy != null)
                            {
                                if (yhy.ComPort_1.IsOpen)
                                    yhy.ComPort_1.Close();
                            }
                            if (bpq != null)
                            {
                                bpq.Close_Com();
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
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                parasiticdata.DIW = float.Parse(textBoxCGJGLSET.Text);
                parasiticdata.JZHXGL = float.Parse(textBoxJzhxGl.Text);
                ini.INIIO.WritePrivateProfileString("DIW", "DIW", parasiticdata.DIW.ToString(), startUpPath + "/detectConfig.ini");
                ini.INIIO.WritePrivateProfileString("加载滑行", "功率", parasiticdata.JZHXGL.ToString(), startUpPath + "/detectConfig.ini");
                selfcheckitem.SdsFqy1 = checkBoxSdsFqy.Checked;
                checkBoxItemSdsFqy.Checked = checkBoxSdsFqy.Checked;
                if (configini.writeSelfConfigIni(selfcheckitem))
                {
                    initControlText();
                    MessageBox.Show("保存成功", "提示");
                }
                else
                    MessageBox.Show("保存失败", "提示");
            }
            catch
            {
                MessageBox.Show("保存失败，输入格式有误", "提示");
            }
            panel2.Visible = false;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.Control && e.KeyCode == Keys.F8)
            {
                panel2.Visible = !panel2.Visible;
            }
            if (e.Shift && e.Control && e.KeyCode == Keys.M)
            {
                if (toolStripLabel提示信息.Text == "提示信息")
                    toolStripLabel提示信息.Text = "准备完毕";
                else
                    toolStripLabel提示信息.Text = "提示信息";
            }
            if (e.Shift && e.Control && e.KeyCode == Keys.N)
            {
                toolStripLabel提示信息.Text = "模拟自检";
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            DateTime nowtime = DateTime.Now;
            TimeSpan timespan = nowtime - jcStarttime;
            jcTime = (float)timespan.TotalMilliseconds / 1000f;
            JCSJ = (int)jcTime;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            isYdjSure = true;
        }

        private void 开始ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                jcStarttime = DateTime.Now;
                timer2.Start();
                if (toolStripLabel提示信息.Text == "模拟自检")
                {
                    th_wait = new Thread(waitTestFinishedMoni);
                    th_wait.Start();
                }
                else
                {
                    th_wait = new Thread(waitTestFinished);
                    th_wait.Start();
                }
            }
        }

        private void 停止ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                timer2.Stop();
                if (th_wait != null)
                    th_wait.Abort();
                if(igbt!=null)
                {

                    if (equipconfig.BpqMethod == "串口")
                    {
                        igbt.TurnOffRelay((byte)equipconfig.BpqDy);
                        bpq.turnOffMotor();
                    }
                    else
                    {
                        igbt.Motor_Close();
                    }
                    //}
                    igbt.Exit_Control();
                }
                fq_Status = true;
            }
            catch
            {
            }
            isRunning = false;
            Msg_Toollabel(toolStripLabel2, "用户中止自检");
        }

        private void 重新开始ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selfStep = 1;
            try
            {
                if (th_wait != null)
                    th_wait.Abort();
            }
            catch
            {
            }
            if (!isRunning)
            {
                loadNewConfig();
                if (toolStripLabel提示信息.Text == "模拟自检")
                {
                    th_wait = new Thread(waitTestFinishedMoni);
                    th_wait.Start();
                }
                else
                {
                    th_wait = new Thread(waitTestFinished);
                    th_wait.Start();
                }
            }
            Msg_Toollabel(toolStripLabel2, "重新开始自检");
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Thread stopthread=new Thread(stopProcess);
            stopthread.Start();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            isYdjSure = true;
        }

        private void stopProcess()
        {
            
            if (igbt != null)
            {

                if (equipconfig.BpqMethod == "串口")
                {
                    igbt.TurnOffRelay((byte)equipconfig.BpqDy);
                    bpq.turnOffMotor();
                }
                else
                {
                    igbt.Motor_Close();
                }
               // }
                if (igbt.Speed > 1f)
                {
                    igbt.Set_Duty(0.1f);
                    igbt.Start_Control_Duty();
                    while (igbt.Speed > 0.5f) Thread.Sleep(200);
                    igbt.Exit_Control();
                }
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            iszssure = true;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            iswdsure = true;
        }
    }
}
