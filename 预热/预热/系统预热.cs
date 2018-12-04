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
using BpqFl;

namespace 预热
{
    public partial class 系统预热 : Form
    {
        configIni configini = new configIni();
        equipmentConfigInfdata configdata = new equipmentConfigInfdata();
        yureconfigInfdata yuredata = new yureconfigInfdata();
        yureconfigIni yureini = new yureconfigIni();
        public delegate void wtpv(Button button, bool visible_value);   //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather); //委托
        public delegate void wtlsb(Label Msgowner, string Msgstr, bool Update_DB);              //委托
        public delegate void wtpc(Panel panel, Color color);
        Thread th_yure = null;
        Exhaust.Fla501 fla_501 = null;
        Exhaust.Fla502 fla_502 = null;
        Exhaust.FLB_100 flb_100 = null;
        Exhaust.Flv_1000 flv_1000 = null;
        Dynamometer.IGBT igbt = null;
        private string UseFqy = "";
        private bool yureIsFinished = false;
        private bool sure_value = false;
        private DateTime fqy_starttime, ydj_starttime, llj_starttime, cgj_starttime;
        private float fqytime = 0, ydjtime = 0, lljtime = 0, cgjtime = 0;
        private bool fqystart = false, ydjstart = false, lljstart = false, cgjstart = false;

        public string bpqXh;
        public string bpqMethod;
        public string comportBpx;
        public string comportBpxPz;
        public double bpqXs = 0.83;
        public bool jsIsUp = true;

        BpqFl.bpxcontrol bpq = null;

        public 系统预热()
        {
            InitializeComponent();
        }
        private void init_configdata()
        {
            yuredata = yureini.getConfigIni();
        }
        private void buttonStart_Click(object sender, EventArgs e)
        {
            th_yure = new Thread(yure_Exe);
            th_yure.Start();
            timer1.Start();
            
        }
        private void yure_Exe()
        {
            if (panel_js.BackColor != Color.LightCoral)
            {
                Msg(label_tishi, panel_tishi, "正在检查举升器...", false);
                setPanelColor(panel_js, Color.LightCoral);
                Thread.Sleep(1000);
                if (igbt != null)
                {
                    igbt.Lifter_Down();
                    Thread.Sleep(1000);
                    Msg(label_tishi, panel_tishi, "举升器下降请点右侧【确定】键", false);
                    setPanelVisible(button_qd, true);
                    while (!sure_value) ;
                    Msg(label_tishi, panel_tishi, "举升器下降正常", false);
                    setPanelVisible(button_qd, false);
                    sure_value = false;
                    igbt.Lifter_Up();
                    Thread.Sleep(1000);
                    Msg(label_tishi, panel_tishi, "举升器上升请点右侧【确定】键", false);
                    setPanelVisible(button_qd, true);
                    while (!sure_value) ;
                    setPanelVisible(button_qd, false);
                    sure_value = false;
                    Msg(label_tishi, panel_tishi, "举升器上升正常，将自动下降", false);
                    igbt.Lifter_Down();
                    Thread.Sleep(1000);
                    jsIsUp = false;
                }
                else
                {
                    Msg(label_jstime, panel_jstime, "非预热项", false);
                }
            }
            if (panel_fxy.BackColor != Color.LightCoral)
            {
                Msg(label_tishi, panel_tishi, "正在检查废气仪...", false);
                setPanelColor(panel_fxy, Color.LightCoral);
                Thread.Sleep(1000);
                switch (UseFqy)
                {
                    case "fla_502":
                        if (fla_502 != null)
                        {
                            if (fla_502.Get_Struct() == "仪器通讯失败")
                            {
                                Msg(label_tishi, panel_tishi, "废气仪通讯失败，预热终止", false);
                                setPanelColor(panel_fxy, Color.Red);
                                return;
                            }
                            else
                            {
                                Msg(label_tishi, panel_tishi, "废气仪已开启，开始计时", false);
                                fqy_starttime = DateTime.Now;
                                fqystart = true;
                            }
                        }
                        else
                        {
                            Msg(label_fqytime, panel_fqytime, "非预热项", false);
                        }
                        break;
                    case "cdf5000":
                        if (configdata.cd_fqy)
                        {
                            if (fla_502 != null)
                            {
                                if (fla_502.Get_Struct() == "仪器通讯失败")
                                {
                                    Msg(label_tishi, panel_tishi, "废气仪通讯失败，预热终止", false);
                                    setPanelColor(panel_fxy, Color.Red);
                                    return;
                                }
                                else
                                {
                                    Msg(label_tishi, panel_tishi, "废气仪已开启，开始计时", false);
                                    fqy_starttime = DateTime.Now;
                                    fqystart = true;
                                }
                            }
                            else
                            {
                                Msg(label_fqytime, panel_fqytime, "非预热项", false);
                            }
                        }
                        else
                        {
                            Msg(label_fqytime, panel_fqytime, "非预热项", false);
                        }
                        break;
                    case "mqw_50a":
                    case "mqw_50b":
                        if (fla_502 != null)
                        {
                            if (fla_502.Get_Struct() == "仪器通讯失败")
                            {
                                Msg(label_tishi, panel_tishi, "废气仪通讯失败，预热终止", false);
                                setPanelColor(panel_fxy, Color.Red);
                                return;
                            }
                            else
                            {
                                Msg(label_tishi, panel_tishi, "废气仪已开启，开始计时", false);
                                fqy_starttime = DateTime.Now;
                                fqystart = true;
                            }
                        }
                        else
                        {
                            Msg(label_fqytime, panel_fqytime, "非预热项", false);
                        }
                        break;
                    case "fla_501":
                        if (fla_501 != null)
                        {

                            Msg(label_tishi, panel_tishi, "废气仪已开启，开始计时", false);
                            fqy_starttime = DateTime.Now;
                            fqystart = true;
                        }
                        else
                        {
                            Msg(label_fqytime, panel_fqytime, "非预热项", false);
                        }
                        break;
                    default:
                        Msg(label_fqytime, panel_fqytime, "非预热项", false);
                        break;
                }
                Thread.Sleep(1000);
            }
            if (panel_ydj.BackColor != Color.LightCoral)
            {
                Msg(label_tishi, panel_tishi, "正在检查烟度计...", false);
                setPanelColor(panel_ydj, Color.LightCoral);
                Thread.Sleep(1000);                
                if(configdata.Ydjifpz)
                {
                    if (configdata.Ydjxh.ToLower() == "cdf5000")
                    {
                        if (fla_502 != null)
                        {                            
                            Msg(label_tishi, panel_tishi, "烟度计已开启，开始计时", false);
                            ydj_starttime = DateTime.Now;
                            ydjstart = true;
                        }
                        else
                        {
                            Msg(label_tishi, panel_tishi, "烟度计通讯失败，预热终止", false);
                            setPanelColor(panel_ydj, Color.Red);
                            return;
                        }
                    }
                    else if (flb_100 != null)
                    {
                        if (flb_100.Get_Mode() == "通讯故障")
                        {
                            Msg(label_tishi, panel_tishi, "烟度计通讯失败，预热终止", false);
                            setPanelColor(panel_ydj, Color.Red);
                            return;
                        }
                        else
                        {
                            Msg(label_tishi, panel_tishi, "烟度计已开启，开始计时", false);
                            ydj_starttime = DateTime.Now;
                            ydjstart = true;
                        }
                    }
                    else
                    {
                        Msg(label_tishi, panel_tishi, "烟度计通讯失败，预热终止", false);
                        setPanelColor(panel_ydj, Color.Red);
                        return;
                    }
                }
                else
                {
                    Msg(label_ydjtime, panel_ydjtime, "非预热项", false);
                }
                Thread.Sleep(1000);
            }
            if (panel_llj.BackColor != Color.LightCoral)
            {
                Msg(label_tishi, panel_tishi, "正在检查流量计...", false);
                setPanelColor(panel_llj, Color.LightCoral);
                Thread.Sleep(1000);
                if (flv_1000 != null)
                {
                    Msg(label_tishi, panel_tishi, "流量计已开启，开始计时", false);
                    llj_starttime = DateTime.Now;
                    lljstart = true;
                }
                else
                {
                    Msg(label_lljtime, panel_lljtime, "非预热项", false);
                }
                Thread.Sleep(1000);
            }
            if (panel_cgj.BackColor != Color.LightCoral)
            {
                Msg(label_tishi, panel_tishi, "正在检查测功机...", false);
                setPanelColor(panel_cgj, Color.LightCoral);
                Thread.Sleep(3000);
                if (igbt != null)
                {
                    Msg(label_tishi, panel_tishi, "电机即将开启，请注意安全", false);
                    if (configdata.BpqMethod == "串口")
                    {
                        bpq.setMotorFre(40 * configdata.BpqXs);
                        Thread.Sleep(20);
                        bpq.turnOnMotor();

                        igbt.TurnOnRelay((byte)configdata.BpqDy);
                    }
                    else
                    {
                        //igbt.TurnOffRelay((byte)configdata.BpqDy);
                        igbt.Motor_Open();
                        Thread.Sleep(500);
                        igbt.Motor_Open();
                    }
                    while (igbt.Speed < 39)
                        Thread.Sleep(1000);
                    cgj_starttime = DateTime.Now;
                    cgjstart = true;
                }
                else
                {
                    Msg(label_cgjtime, panel_cgjtime, "非预热项", false);
                }
                Thread.Sleep(1000);
            }
            Msg(label_tishi, panel_tishi, "正在预热", false);
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
        public void setPanelColor(Panel panel, Color color)
        {
            try
            {
                BeginInvoke(new wtpc(set_panelColor), panel, color);
            }
            catch
            { }
        }
        public void set_panelColor(Panel panel, Color color)
        {
            panel.BackColor = color;
        }
        public void setPanelVisible(Button button,bool visible_value)
        {
            try
            {
                BeginInvoke(new wtpv(set_panelVisible), button, visible_value);
            }
            catch
            { }
        }
        public void set_panelVisible(Button button, bool visible_value)
        {
            button.Visible = visible_value;
        }

        private void toolStripButtonMotorOff_Click(object sender, EventArgs e)
        {
            if (igbt != null)
                igbt.Motor_Close();
        }

        private void toolStripButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            Thread stopthread = new Thread(stopProcess);
            stopthread.Start();
        }
        private void stopProcess()
        {

            if (igbt != null)
            {
                igbt.Motor_Close();
                if (igbt.Speed > 1f)
                {
                    igbt.Set_Duty((float)(configdata.BrakePWM * 1.0 / 100.0));
                    igbt.Start_Control_Duty();
                    while (igbt.Speed > 0.5f) Thread.Sleep(200);
                    igbt.Exit_Control();
                }
            }
        }

        private void 系统预热_Load(object sender, EventArgs e)
        {
            setPanelVisible(button_qd, false);
            configdata = configini.getEquipConfigIni();
            initEquipment();
            init_configdata();
            if (igbt != null)
                igbt.Lifter_Up();
            
        }
        public void initEquipment()
        {
            bool Init_flag = true;
            string init_message = "";
            try
            {
                if (configdata.Fqyifpz == true)
                {
                    switch (configdata.Fqyxh.ToLower())           //通过仪器型号选择初始化项目
                    {
                        case "nha_503":
                            try
                            {
                                UseFqy = "fla_502";
                                fla_502 = new Exhaust.Fla502(configdata.Fqyxh);
                                fla_502.isNhSelfUse = configdata.isFqyNhSelfUse;
                                if (fla_502.Init_Comm(configdata.Fqyck, configdata.Fqyckpzz) == false)
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
                                fla_502 = new Exhaust.Fla502(configdata.Fqyxh);
                                if (fla_502.Init_Comm(configdata.Fqyck, configdata.Fqyckpzz) == false)
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
                                UseFqy = "cdf5000";
                                fla_502 = new Exhaust.Fla502(configdata.Fqyxh);
                                if (fla_502.Init_Comm(configdata.Fqyck, configdata.Fqyckpzz) == false)
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
                                fla_502 = new Exhaust.Fla502(configdata.Fqyxh);
                                if (fla_502.Init_Comm(configdata.Fqyck, configdata.Fqyckpzz) == false)
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
                                fla_502 = new Exhaust.Fla502(configdata.Fqyxh);
                                if (fla_502.Init_Comm(configdata.Fqyck, configdata.Fqyckpzz) == false)
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
                                fla_502 = new Exhaust.Fla502(configdata.Fqyxh);
                                if (fla_502.Init_Comm(configdata.Fqyck, configdata.Fqyckpzz) == false)
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
                                if (fla_501.Init_Comm(configdata.Fqyck, configdata.Fqyckpzz) == false)
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

            //这里只初始化了废气分析仪其他设备要继续初始化
            try
            {
                if (configdata.Cgjifpz)
                {
                    try
                    {
                        igbt = new Dynamometer.IGBT("BNTD",configdata.isIgbtContainGdyk);
                        if (igbt.Init_Comm(configdata.Cgjck, configdata.cgjckpzz) == false)
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
                if (configdata.Ydjifpz == true&&configdata.Ydjxh!="CDF5000")
                {
                    try
                    {
                        flb_100 = new Exhaust.FLB_100(configdata.Ydjxh);
                        flb_100.isNhSelfUse = configdata.isYdjNhSelfUse;
                        if (flb_100.Init_Comm(configdata.Ydjck, configdata.Ydjckpzz) == false)
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
                if (configdata.Lljifpz == true)
                {
                    try
                    {
                        flv_1000 = new Exhaust.Flv_1000();
                        flv_1000.isNhSelfUse = configdata.isLljNhSelfUse;
                        if (flv_1000.Init_Comm(configdata.Lljck, configdata.Lljckpzz) == false)
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
                if (configdata.BpqMethod == "串口")
                {
                    try
                    {
                        bpq = new bpxcontrol(configdata.BpqXh);
                        if (bpq.Init_Comm(configdata.BpqCom, configdata.BpqComPz) == false)
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
            
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (th_yure != null)
                    if (th_yure.IsAlive)
                    {
                        th_yure.Abort();
                        
                    }
                if (igbt != null)
                {
                   if (configdata.BpqMethod == "串口")
                    {
                        igbt.TurnOffRelay((byte)configdata.BpqDy);
                        bpq.turnOffMotor();
                    }
                    else
                    {
                        igbt.TurnOffRelay((byte)configdata.BpqDy);
                        igbt.Motor_Close();
                    }
                }
                Msg(label_tishi, panel_tishi, "已停止预热", false);
                timer1.Stop();
            }
            catch
            { }
        }

        private void 系统预热_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!yureIsFinished)
            {
                if (MessageBox.Show("预热未完成，确认退出？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    //try
                    //{
                    //    ini.INIIO.WritePrivateProfileString("PARAM", "Status", "0", "C:/jcdatatxt/Yure.ini");
                    //}
                    //catch
                    //{ }
                    try
                    {
                        igbt.Exit_Control();
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
                            if (flv_1000.ComPort_1.IsOpen)
                                flv_1000.ComPort_1.Close();
                        }
                        if (igbt != null)
                        {
                            igbt.closeIgbt();
                        }
                        if (bpq != null)
                        {
                            bpq.Close_Com();
                        }
                    }
                    catch
                    { }
                    try
                    {
                        if (th_yure != null)
                            th_yure.Abort();
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
                    ini.INIIO.WritePrivateProfileString("PARAM", "Status", "1", "C:/jcdatatxt/Yure.ini");
                }
                catch
                { }
                try
                {
                    igbt.Exit_Control();
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
                        if (flv_1000.ComPort_1.IsOpen)
                            flv_1000.ComPort_1.Close();
                    }
                    if (igbt != null)
                    {
                        igbt.closeIgbt();
                    }
                }
                catch
                { }
                try
                {
                    if (th_yure != null)
                        th_yure.Abort();
                }
                catch
                { }
            }

        }

        private void button_qd_Click(object sender, EventArgs e)
        {
            sure_value = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime nowtime = DateTime.Now;
            bool cgjisOver = true, fqyisOver = true, ydjIsover = true, lljisOver = true;
            if (fqystart)
            {
                TimeSpan timespan1 = nowtime - fqy_starttime;
                fqytime = (float)timespan1.TotalMilliseconds / 1000f;
                if (fqytime >= yuredata.Fqytime) fqystart = false;
                Msg(label_fqytime, panel_fqytime, ((int)fqytime / 60).ToString("0") + ":" + ((int)fqytime % 60).ToString("0"), false);
            }
            if (ydjstart)
            {
                TimeSpan timespan2 = nowtime - ydj_starttime;
                ydjtime = (float)timespan2.TotalMilliseconds / 1000f;
                if (ydjtime >=yuredata.Ydjtime) ydjstart = false;
                Msg(label_ydjtime, panel_ydjtime, ((int)ydjtime / 60).ToString("0") + ":" + ((int)ydjtime % 60).ToString("0"), false);
            }
            if (lljstart)
            {
                TimeSpan timespan3 = nowtime - llj_starttime;
                lljtime = (float)timespan3.TotalMilliseconds / 1000f;
                if (lljtime >= yuredata.Lljtime) lljstart = false;
                Msg(label_lljtime, panel_lljtime, ((int)lljtime / 60).ToString("0") + ":" + ((int)lljtime % 60).ToString("0"), false);
            }
            if (cgjstart)
            {
                TimeSpan timespan4 = nowtime - cgj_starttime;
                cgjtime = (float)timespan4.TotalMilliseconds / 1000f;
                if (cgjtime >= yuredata.Cgjtime)
                {
                    cgjstart = false;
                    if (configdata.BpqMethod == "串口")
                    {
                        igbt.TurnOffRelay((byte)configdata.BpqDy);
                        bpq.turnOffMotor();
                    }
                    else
                    {
                        igbt.TurnOffRelay((byte)configdata.BpqDy);
                        igbt.Motor_Close();
                        Thread.Sleep(500);
                        igbt.Motor_Close();
                    }
                }
                Msg(label_cgjtime, panel_cgjtime, ((int)cgjtime / 60).ToString("0") + ":" + ((int)cgjtime % 60).ToString("0"), false);
            }
            if (configdata.Cgjifpz == true && cgjtime < yuredata.Cgjtime)
                cgjisOver = false;
            if (configdata.Fqyifpz == true && fqytime < yuredata.Fqytime)
                fqyisOver = false;
            if (configdata.Ydjifpz == true && ydjtime < yuredata.Ydjtime)
                ydjIsover = false;
            if (configdata.Lljifpz == true && lljtime < yuredata.Lljtime)
                lljisOver = false;
            if (cgjisOver==true&&fqyisOver==true&&ydjIsover==true&&lljisOver==true)
            {
                if (!jsIsUp)
                {
                    Msg(label_tishi, panel_tishi, "预热完毕，等待测功机减速", false);
                    if (igbt != null)
                    {
                        if (igbt.Speed == 0)
                        {
                            igbt.Lifter_Up();
                            jsIsUp = true;
                        }
                    }
                }
                else
                {
                    yureIsFinished = true;
                    Msg(label_tishi, panel_tishi, "预热完毕，请退出", false);
                }
            }
            if (igbt != null)
            {
                Msg(Msg_cs, panel_cs, igbt.Speed.ToString("0.0"), false);
                Msg(Msg_nl, panel_nl, igbt.Force.ToString(), false);
                Msg(Msg_gl, panel_gl, igbt.Power.ToString("0.00"), false);
            }
            

        }

        private void toolStripButtonMotorOn_Click(object sender, EventArgs e)
        {
            if (igbt != null)
                igbt.Motor_Open();
        }

        private void toolStripButtonLiftUp_Click(object sender, EventArgs e)
        {
            if (igbt != null)
                igbt.Lifter_Up();
        }

        private void toolStripButtonLiftDown_Click(object sender, EventArgs e)
        {
            if (igbt != null)
                igbt.Lifter_Down();

        }
    }
}
