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
    public partial class LljDemarcate : Form
    {
        private string Usellj = "";
        public static Exhaust.Flv_1000 flv_1000 = null;
        public Thread th_lljcl = null;
        public Thread tl_bd = null;
        private bool Init_Flag = false;
        public delegate void wtlsb(Label Msgowner, string Msgstr);      //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                     //委托
        public delegate void wtcs(Control controlname, string text);                    //委托
        public LljDemarcate(VMAS1._0.demarcate parent)
        {
            InitializeComponent();
            this.MdiParent = parent;
        }

        private void LljDemarcate_Load(object sender, EventArgs e)
        {
            radioButtonBzll.Checked = true;
            Init_COM();
        }

        public void Init_COM()
        {
            try
            {
                if (mainPanelForm.jcxxxb.LLJPZ != null)
                    switch (mainPanelForm.jcxxxb.LLJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[2])           //通过仪器型号选择初始化项目
                    {
                        case "FLV_1000":
                            try
                            {
                                Usellj = "FLV_1000";
                                flv_1000 = new Exhaust.Flv_1000();
                                if (flv_1000.Init_Comm(mainPanelForm.jcxxxb.LLJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[0], mainPanelForm.jcxxxb.LLJPZ.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1]))
                                {
                                    //mqw_50A.Auto_Zeroing();       //设置自动调零
                                    Init_Flag = true;               //初始化串口成功
                                }
                                else
                                {
                                    flv_1000 = null;
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
                    }
            }
            catch (Exception)
            {
                MessageBox.Show("串口初始化失败,请检测相应设置", "出错啦");
                toolStrip1.Enabled = false;
            }
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            toolStripButton1.Enabled = false;
            th_lljcl = new Thread(lljcl);
            th_lljcl.Start();
        }
        private void lljcl()
        {
            try
            {
                if (flv_1000 != null)
                {
                    
                   
                    Msg_label(label_msg, panel_msg, "开始取数");
                    Thread.Sleep(1000);
                    while (true)
                    {
                        if (radioButtonBzll.Checked == true)
                        {
                            Thread.Sleep(500);
                            string lljzt = flv_1000.Get_standardDat();
                            if (lljzt == "获取成功")
                            {
                                Msg_label(label_bzll,panel_bzll,flv_1000.ll_standard_value.ToString("0.0"));
                                Msg_label(label_sjll, panel_sjll, "----");
                                Msg_label(label_xsyq, panel_xsyq, flv_1000.o2_standard_value.ToString("0.0"));
                                Msg_label(label_xsyl, panel_xsyl, flv_1000.yali_standard_value.ToString("0.0"));
                                Msg_label(label_xswd, panel_xswd, flv_1000.temp_standard_value.ToString("0.0"));
                            }
                            else
                            {
                                Msg_label(label_msg, panel_msg, lljzt);
                            }
                        }
                        else
                        {
                            Thread.Sleep(500);
                            string lljzt = flv_1000.Get_unstandardDat();
                            if (lljzt == "获取成功")
                            {
                                Msg_label(label_bzll, panel_bzll, "----");
                                Msg_label(label_sjll, panel_sjll, flv_1000.ll_unstandard_value.ToString("0.0"));
                                Msg_label(label_xsyq, panel_xsyq, flv_1000.o2_standard_value.ToString("0.0"));
                                Msg_label(label_xsyl, panel_xsyl, flv_1000.yali_standard_value.ToString("0.0"));
                                Msg_label(label_xswd, panel_xswd, flv_1000.temp_standard_value.ToString("0.0"));
                            }
                            else
                            {
                                Msg_label(label_msg, panel_msg, lljzt);
                            }
 
                        }
                        //Exhaust.Fla502_data ex_temp = fla_502.GetData();

                        //Msg(Msg_fq_1, panel_fq, "HC:" + ex_temp.HC + " CO:" + ex_temp.CO + " CO2:" + ex_temp.CO2 + " O2:" + ex_temp.O2, false);
                        //Msg(Msg_fq_2, panel_fq, "NO:" + ex_temp.NO + " λ:" + ex_temp.λ + " 转速:" + ex_temp.ZS + " 油温:" + ex_temp.YW, false);
                    }
                    //}
                }
                else
                {
                    Msg_label(label_msg, panel_msg, "出错,请检查流量计");
                }
            }
            catch (Exception)
            {
                Msg_label(label_msg, panel_msg, "出错,请检查流量计");
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

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    th_lljcl.Abort();
                }
                catch (Exception)
                {
                }
                toolStripButton1.Enabled = true;
                //Msg(label_llj_2, panel_llj, "", false);
                Msg_label(label_msg, panel_msg, "停止取数");
            }
            catch (Exception)
            {
            }
        }

        private void buttonO2jz_Click(object sender, EventArgs e)
        {
            try
            {
                Msg_label(label_msg, panel_msg, flv_1000.Xishi_O2(float.Parse(textEditXsy.Text.Trim())));
            }
            catch
            {
                MessageBox.Show("操作失败,请检测输入数据格式是否正确", "系统提示");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Msg_label(label_msg, panel_msg, flv_1000.airpressure_demarcate(float.Parse(textEditQtYl.Text.Trim())));
            }
            catch
            {
                MessageBox.Show("操作失败,请检测输入数据格式是否正确", "系统提示");
            }
        }

        private void buttonWdjz_Click(object sender, EventArgs e)
        {
            try
            {
                Msg_label(label_msg, panel_msg, flv_1000.temperature_demarcate(float.Parse(textEditQtwd.Text.Trim())));
            }
            catch
            {
                MessageBox.Show("操作失败,请检测输入数据格式是否正确", "系统提示");
            }
        }

        private void buttonCcsz_Click(object sender, EventArgs e)
        {
            try
            {
                Msg_label(label_msg, panel_msg, flv_1000.reset_default());
            }
            catch
            {
                MessageBox.Show("恢复出厂设置失败", "系统提示");
            }
        }
        /// <summary>
        /// 输入限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textEditXsy_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool IsContaintDot = this.textEditXsy.Text.Contains(".");
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
            {
                e.Handled = true;
            }
            else if (IsContaintDot && (e.KeyChar == 46))
            {
                e.Handled = true;
            }
        }
        private void textEditqtwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool IsContaintDot = this.textEditQtwd.Text.Contains(".");
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
            {
                e.Handled = true;
            }
            else if (IsContaintDot && (e.KeyChar == 46))
            {
                e.Handled = true;
            }
        }
        private void textEditqtyl_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool IsContaintDot = this.textEditQtYl.Text.Contains(".");
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
            {
                e.Handled = true;
            }
            else if (IsContaintDot && (e.KeyChar == 46))
            {
                e.Handled = true;
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lljdemarcate_formClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (th_lljcl != null)
                {
                    if (th_lljcl.IsAlive == true)
                        th_lljcl.Abort();
                }
                if (flv_1000 != null)
                {
                    if (flv_1000.ComPort_1.IsOpen)
                        flv_1000.ComPort_1.Close();
                }
                
            }
            catch (Exception)
            {
                //System.Environment.Exit(0);
            }
        }
        public void exit_form()
        {
            this.Close();
        }
    }
}
