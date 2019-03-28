using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using carinfor;
using System.Threading;

namespace 泄漏检查
{
    public partial class Form1 : Form
    {
        public string startUpPath = Application.StartupPath;
        configIni configini = new configIni();
        equipmentConfigInfdata equipconfig = new equipmentConfigInfdata();
        private string UseFqy = "";
        Exhaust.Fla501 fla_501 = null;
        Exhaust.Fla502 fla_502 = null;
        public Form1()
        {
            InitializeComponent();
        }
        private void initConfigInfo()
        {
            equipconfig = configini.getEquipConfigIni();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initConfigInfo();
            initEquipment();
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

        }
        private bool isRunning = false;
        private Thread th_wait;
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text=="开始检漏")
            {
                if (th_wait != null && th_wait.IsAlive)
                    th_wait.Abort();

                th_wait = new Thread(waitTestFinished);
                th_wait.Start();
                button1.Text = "停止检漏";
            }
            else
            {

                if (th_wait != null && th_wait.IsAlive)
                    th_wait.Abort();
                button1.Text = "开始检漏";
            }
        }
        #region 信息显示

        public delegate void wtcs(Control controlname, string text);                                //委托
        public delegate void wtlsb(Label Msgowner, string Msgstr);                  //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                                 //委托
        public delegate void wttlsb(ToolStripLabel Msgowner, string Msgstr);                  //委托
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
        private bool isJLsure = false;
        private void waitTestFinished()
        {
            if (fla_502 != null)
            {
                bool isfqycheckresult = true;
                DateTime starttime, endtime;
                starttime = DateTime.Now;
                string fqyzt = fla_502.Get_Struct();
                if (fqyzt.Contains("失败"))
                {
                    Msg(labelMsg, panelMsg, "废气仪通讯异常");
                    isRunning = false;
                    return;
                }
                else if (fqyzt.Contains("预热"))
                {
                    Msg(labelMsg, panelMsg, "废气仪正在预热，自检将中止");
                    isRunning = false;
                    return;
                }
                else
                {
                    Msg(labelMsg, panelMsg, "废气仪通讯正常");
                }
                Thread.Sleep(500);
                Msg(labelMsg, panelMsg, "堵住探头进气口后点击\"确定\"按钮进行检漏");
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
                        Msg(labelMsg, panelMsg, "检漏中..." + leaktest.ToString() + "s");
                        leaktest++;
                        Thread.Sleep(900);
                    }
                    if (leaktesting == 1)
                    {
                        Ref_Control_Text(LabelFQYJL, "√");
                        Msg(labelMsg, panelMsg, "检漏完毕");
                        //leaktesting = true;
                    }
                    else if (leaktesting == -1)
                    {
                        Ref_Control_Text(LabelFQYJL, "×");
                        Msg(labelMsg, panelMsg, "检漏完毕");
                        //leaktesting = true;
                    }
                    else
                    {
                        Ref_Control_Text(LabelFQYJL, "×");
                        Msg(labelMsg, panelMsg, "检漏失败");
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
                            Msg(labelMsg, panelMsg, "检漏完毕");
                            leaktesting = true;
                        }
                        else if (leakstring == "泄漏超标")
                        {
                            Ref_Control_Text(LabelFQYJL, "×");
                            Msg(labelMsg, panelMsg, "检漏完毕");
                            isfqycheckresult = false;
                            leaktesting = true;
                        }
                        else
                        {
                            Msg(labelMsg, panelMsg, "检漏中..." + leaktest.ToString() + "s");
                            leaktest++;
                            Thread.Sleep(900);
                        }
                    }
                }
                else if (equipconfig.Fqyxh.ToLower() == "cdf5000")
                {
                    int leaktest = 0;
                    bool leaktesting = false;
                    while (!leaktesting)
                    {
                        string leakstring = fla_502.Get_fla502leckStruct();
                        if (leakstring == "无泄漏")
                        {
                            Ref_Control_Text(LabelFQYJL, "√");
                            Msg(labelMsg, panelMsg, "检漏完毕");
                            leaktesting = true;
                        }
                        else if (leakstring == "泄漏超标")
                        {
                            Ref_Control_Text(LabelFQYJL, "×");
                            Msg(labelMsg, panelMsg, "检漏完毕");
                            isfqycheckresult = false;
                            leaktesting = true;
                        }
                        else
                        {
                            Msg(labelMsg, panelMsg, "检漏中..." + leaktest.ToString() + "s");
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
                            Msg(labelMsg, panelMsg, "检漏完毕");
                            leaktesting = true;
                        }
                        else if (leakstring == "泄漏超标")
                        {
                            Ref_Control_Text(LabelFQYJL, "×");
                            Msg(labelMsg, panelMsg, "检漏完毕");
                            isfqycheckresult = false;
                            leaktesting = true;
                        }
                        else
                        {
                            Msg(labelMsg, panelMsg, "检漏中..." + leaktest.ToString() + "s");
                            leaktest++;
                            Thread.Sleep(900);
                        }
                    }
                }
                Ref_Control_Text(button1, "开始检漏");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (LabelFQYJL.Text == "--")
            {
                MessageBox.Show("请先完成检漏");
                return;
            }
            else
            {
                leakcheck fqysefdata = new leakcheck();
                if(LabelFQYJL.Text== "√")
                    fqysefdata.TightnessResult = "1";
                else
                    fqysefdata.TightnessResult = "0";
                selfCheckIni selfcontrol = new selfCheckIni();
                selfcontrol.writeLeakTestResult(fqysefdata);
                MessageBox.Show("保存成功");
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Shift && e.Control && e.KeyCode == Keys.M)
            {
                LabelFQYJL.Text = "√";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (th_wait != null && th_wait.IsAlive)
                th_wait.Abort();
            if (fla_502 != null)
                fla_502.ComPort_1.Close();
            System.Environment.Exit(0);
        }
    }
}
