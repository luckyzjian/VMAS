using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace VMAS1._0
{
    public partial class exhaustForm : Form
    {
        private string UseFqy = "";
        public static Exhaust.MQW_50A mqw_50A = null;                                   //名泉MQW_50A废气分析仪方法集
        public static Exhaust.Fla502 fla_502 = null;
        public static Exhaust.Fla501 fla_501 = null;
        public Thread th_fqdetect = null;
        public Thread tl_bd = null;
        private bool Init_Flag = false;
        public delegate void wtlsb(Label Msgowner, string Msgstr);      //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                     //委托
        public delegate void wtcs(Control controlname, string text);                    //委托
        public exhaustForm(VMAS1._0.demarcate parent)
        {
            InitializeComponent();
            this.MdiParent = parent;
        }

        private void low_biaoQiChecked(object sender, EventArgs e)
        {
            if (radioButtonLowB.Checked == true)
            {
                textEditBiaoC3h8.Text = "200";
                textEditBiaoCo.Text = "0.5";
                textEditBiaoCo2.Text = "3.6";
                textEditBiaoNo.Text = "300";
                textEditBiaoO2.Text = "0.0";
            }
        }

        private void highh_biaoChecked(object sender, EventArgs e)
        {
            if (radioButtonHighB.Checked == true)
            {
                textEditBiaoC3h8.Text = "3200";
                textEditBiaoCo.Text = "8.0";
                textEditBiaoCo2.Text = "12";
                textEditBiaoNo.Text = "3000";
                textEditBiaoO2.Text = "0.0";
            }
        }
        private void zeroO2checked(object sender, EventArgs e)
        {
            if (radioButtonBiaolq.Checked == true)
            {
                textEditBiaoC3h8.Text = "0";
                textEditBiaoCo.Text = "0";
                textEditBiaoCo2.Text = "0";
                textEditBiaoNo.Text = "0";
                textEditBiaoO2.Text = "20.9";
            }
        }
        private void FqBiaoding_Load(object sender, EventArgs e)
        {
            Init_form();
            Init_COM();
            Init_thread();
        }
        private void Init_form()
        {
            radioButtonLowB.Checked = true;
            radioButtonHuanj.Checked = true;
        }
        public void Init_COM()
        {
            try
            {
                if (mainPanelForm.jcxxxb.FQFXYPZ != null)
                    switch (mainPanelForm.jcxxxb.FQFXYPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[2])           //通过仪器型号选择初始化项目
                    {
                        case "MQW-50A":
                            try
                            {
                                UseFqy = "MQW-50A";
                                mqw_50A = new Exhaust.MQW_50A();
                                if (mqw_50A.Init_Comm(mainPanelForm.jcxxxb.FQFXYPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0], mainPanelForm.jcxxxb.FQFXYPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1]))
                                {
                                    //mqw_50A.Auto_Zeroing();       //设置自动调零
                                    Init_Flag = true;               //初始化串口成功
                                }
                                else
                                {
                                    mqw_50A = null;
                                    Init_Flag = false;
                                    toolStrip1.Enabled = false;
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                Init_Flag = false;
                                toolStrip1.Enabled = false;
                            }
                            break;
                        case "FLA_502":
                            try
                            {
                                UseFqy = "FLA_502";
                                fla_502 = new Exhaust.Fla502();
                                if (fla_502.Init_Comm(mainPanelForm.jcxxxb.FQFXYPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0], mainPanelForm.jcxxxb.FQFXYPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1]))
                                {
                                    //mqw_50A.Auto_Zeroing();       //设置自动调零
                                    Init_Flag = true;               //初始化串口成功
                                }
                                else
                                {
                                    fla_502 = null;
                                    Init_Flag = false;
                                    toolStrip1.Enabled = false;
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                toolStrip1.Enabled = false;
                            }
                            break;
                        case "FLA_501":
                            try
                            {
                                UseFqy = "FLA_501";
                                fla_501 = new Exhaust.Fla501();
                                if (fla_501.Init_Comm(mainPanelForm.jcxxxb.FQFXYPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0], mainPanelForm.jcxxxb.FQFXYPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1]))
                                {
                                    //mqw_50A.Auto_Zeroing();       //设置自动调零
                                    Init_Flag = true;               //初始化串口成功
                                }
                                else
                                {
                                    fla_501 = null;
                                    Init_Flag = false;
                                    toolStrip1.Enabled = false;
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                toolStrip1.Enabled = false;
                            }
                            break;
                    }
            }
            catch (Exception)
            {
                MessageBox.Show("串口初始化失败,请检测相应设置", "出错啦");
                toolStrip1.Enabled = false;
            }
        }
        private void Init_thread()
        {
            th_fqdetect = new Thread(fq_detect);
            th_fqdetect.Start();
        }
        private void Msg(float hc, float no, float co, float co2, float pef, float o2)
        {
            textEditHc.Text = hc.ToString("0");
            textEditNo.Text = no.ToString("0");
            textEditCo.Text = co.ToString("0.00");
            textEditCo2.Text = co2.ToString("0.00");
            textEditPEF.Text = pef.ToString("0.000");
            textEditO2.Text = o2.ToString("0.00");
        }
        private void fq_detect()
        {
            try
            {
                float pef=0f;
                switch (UseFqy)
                {
                    case "FLA_502":
                        {
                            if (fla_502 != null)
                            {
                                while (true)
                                {
                                    Thread.Sleep(500);
                                    Exhaust.Fla502_data ex_temp = fla_502.GetData();
                                    pef=fla_502.Getdata_PEF();
                                    Msg(ex_temp.HC,ex_temp.NO,ex_temp.CO,ex_temp.CO2,pef,ex_temp.O2);
                                    Thread.Sleep(100);
                                }
                            }
                        }
                        break;
                    case "FLA_501":
                        {
                            if (fla_501 != null)
                            {
                                while (true)
                                {
                                    Thread.Sleep(500);
                                    Exhaust.Fla501_data ex_temp = fla_501.Get_Data();
                                    pef = fla_501.Getdata_PEF();
                                    Msg(ex_temp.HC, ex_temp.NO, ex_temp.CO, ex_temp.CO2, pef, ex_temp.O2);
                                    Thread.Sleep(100);
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception)
            {
                
            }
        }

        private void toolStripButtonTl_Click(object sender, EventArgs e)
        {
            Thread tl_fq = new Thread(fqtl);
            tl_fq.Start();
        }

        private void fqtl()
        {
            try
            {
                toolStripButtonTl.Enabled = false;
                toolStripButtonXljc.Enabled = false;
                toolStripButtonBd.Enabled = false;
                toolStripButtonPumpOn.Enabled = false;
                toolStripButtonPumpOff.Enabled = false;
                toolStripButtonSelfDetect.Enabled = false;
                switch (UseFqy)
                {
                    case "FLA_502":
                        if (fla_502 != null)
                        {
                            if (radioButtonLingQ.Checked == true)
                                fla_502.setZeroAsTl();
                            else
                                fla_502.setAirAsTl();
                            fla_502.Zeroing();
                            for (int i = 30; i > 0; i--)
                            {
                                Msg_label(label_msg, panel_msg, "仪器调零 " + i.ToString("0"));
                                Thread.Sleep(1000);
                            }
                            while (true)
                            {
                                int i = 100;
                                if (i <= 1)
                                {
                                    Msg_label(label_msg, panel_msg, "调零失败,请手动操作");
                                    break;
                                }
                                if (fla_502.Get_Struct().IndexOf("准备好") > -1)
                                    break;
                                else
                                    i--;
                                Thread.Sleep(10);
                            }
                            Msg_label(label_msg, panel_msg, "调零成功");
                            toolStripButtonTl.Enabled = true;
                            toolStripButtonXljc.Enabled = true;
                            toolStripButtonBd.Enabled = true;
                            toolStripButtonPumpOn.Enabled = true;
                            toolStripButtonPumpOff.Enabled = true;
                            toolStripButtonSelfDetect.Enabled = true;
                        }
                        else
                        {
                            Msg_label(label_msg, panel_msg, "调零失败,检查仪器状态");
                            toolStripButtonTl.Enabled = true;
                            toolStripButtonXljc.Enabled = true;
                            toolStripButtonBd.Enabled = true;
                            toolStripButtonPumpOn.Enabled = true;
                            toolStripButtonPumpOff.Enabled = true;
                            toolStripButtonSelfDetect.Enabled = true;
                        }
                        break;
                    case "FLA_501":
                        if (fla_501 != null)
                        {
                            fla_501.SetZero();
                            for (int i = 30; i > 0; i--)
                            {
                                Msg_label(label_msg, panel_msg, "仪器调零 " + i);
                                Thread.Sleep(1000);
                            }
                            Msg_label(label_msg, panel_msg, "调零成功");
                            toolStripButtonTl.Enabled = true;
                            toolStripButtonXljc.Enabled = true;
                            toolStripButtonBd.Enabled = true;
                            toolStripButtonPumpOn.Enabled = true;
                            toolStripButtonPumpOff.Enabled = true;
                            toolStripButtonSelfDetect.Enabled = true;
                        }
                        else
                        {
                            Msg_label(label_msg, panel_msg, "调零失败,检查仪器状态");
                            toolStripButtonTl.Enabled = true;
                            toolStripButtonXljc.Enabled = true;
                            toolStripButtonBd.Enabled = true;
                            toolStripButtonPumpOn.Enabled = true;
                            toolStripButtonPumpOff.Enabled = true;
                            toolStripButtonSelfDetect.Enabled = true;
                        }
                        break;
                }

            }
            catch (Exception)
            {
                Msg_label(label_msg, panel_msg, "调零失败,检查仪器状态");
                toolStripButtonTl.Enabled = true;
                toolStripButtonXljc.Enabled = true;
                toolStripButtonBd.Enabled = true;
                toolStripButtonPumpOn.Enabled = true;
                toolStripButtonPumpOff.Enabled = true;
                toolStripButtonSelfDetect.Enabled = true;
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
        public void Msg_label(Label Msgowner, Panel Msgfather, string Msgstr)
        {
            BeginInvoke(new wtlsb(Msg_Show), Msgowner, Msgstr);
            BeginInvoke(new wtlp(Msg_Position), Msgowner, Msgfather);
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

        private void toolStripButtonXljc_Click(object sender, EventArgs e)
        {
            switch (UseFqy)
            {
                case "FLA_502":
                    fla_502.Leak_check();
                    break;
                case "FLA_501":
                    break;
            }
            
        }

        private void toolStripButtonBd_Click(object sender, EventArgs e)
        {
            Thread tl_bd = new Thread(fqbd);
            tl_bd.Start();
        }
        private void fqbd()
        {
            string bdzt="";
            switch (UseFqy)
            {
                case "FLA_502":
                    fla_502.Set_standardGas(double.Parse(textEditBiaoC3h8.Text), double.Parse(textEditBiaoCo.Text), double.Parse(textEditBiaoCo2.Text), double.Parse(textEditBiaoO2.Text), float.Parse(textEditBiaoNo.Text));//不标定氧气
                    Msg_label(label_msg, panel_msg, "开始标定");
                    while (true)
                    {
                        bdzt = fla_502.Demarcate();
                        if (bdzt == "标定中" || bdzt == "开始标定")
                            Msg_label(label_msg, panel_msg, "正在标定");
                        else
                            break;
                    }
                    Msg_label(label_msg, panel_msg, bdzt);
                    break;
                case "FLA_501":
                    MessageBox.Show("该型号废气仪未提供该功能,请进行手动标定", "系统提示");
                    break;
            }
        }

        private void toolStripButtonPumpOn_Click(object sender, EventArgs e)
        {
            switch (UseFqy)
            {
                case "FLA_502":
                    fla_502.Pump_Pipeair();
                    break;
                case "FLA_501":
                    fla_501.openbang();
                    break;
            }
        }

        private void toolStripButtonPumpOff_Click(object sender, EventArgs e)
        {
            switch (UseFqy)
            {
                case "FLA_502":
                    fla_502.StopBlowback();
                    break;
                case "FLA_501":
                    fla_501.Closebang();
                    break;
            }

        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (th_fqdetect.IsAlive)
                    th_fqdetect.Abort();
                if (tl_bd.IsAlive)
                    tl_bd.Abort();
            }
            catch
            { }
        }

        private void fqDemarcate_closing(object sender, FormClosingEventArgs e)
        {
            if (th_fqdetect != null)
            {
                if (th_fqdetect.IsAlive)
                    th_fqdetect.Abort();
            }
            if (tl_bd != null)
            {
                if (tl_bd.IsAlive)
                    tl_bd.Abort();
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
        }

        private void fqDemarcate_closing(object sender, ControlEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Close();

        }
        public void exit_form()
        {
            this.Close();
        }


    }
}
