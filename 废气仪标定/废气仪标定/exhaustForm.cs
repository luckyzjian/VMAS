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

namespace 废气仪标定
{
    public partial class exhaustForm : Form
    {
        configIni configini = new configIni();
        equipmentConfigInfdata configdata = new equipmentConfigInfdata();
        private string UseFqy = "";
        public static Exhaust.Fla502 fla_502 = null;
        public static Exhaust.Fla501 fla_501 = null;
        public Thread th_fqdetect = null;
        public Thread tl_bd = null;
        public Thread tl_jc = null;
        public delegate void wtts(DevExpress.XtraEditors.TextEdit textedit, string content);
        private bool Init_Flag = false;
        public delegate void wtlsb(Label Msgowner, string Msgstr);      //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                     //委托
        public delegate void wtcs(Control controlname, string text);                    //委托
        analysismeterInidata analysisdata = new analysismeterInidata();
        analysismeterIni analysismeterini = new analysismeterIni();

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
        private void initEquipment()
        {
            bool Init_flag = true;
            string init_message = "";
            try
            {
                if (configdata.Fqyifpz == true)
                {
                    switch (configdata.Fqyxh.ToLower())           //通过仪器型号选择初始化项目
                    {
                        case "fla_502":
                            try
                            {
                                UseFqy = "fla_502";
                                fla_502 = new Exhaust.Fla502(UseFqy);
                                if (fla_502.Init_Comm(configdata.Fqyck, "9600,N,8,1") == false)
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
                                fla_502 = new Exhaust.Fla502(UseFqy);
                                if (fla_502.Init_Comm(configdata.Fqyck, "9600,N,8,1") == false)
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
                                if (fla_501.Init_Comm(configdata.Fqyck, "9600,N,8,1") == false)
                                {
                                    fla_501 = null;
                                    Init_flag = false;
                                    init_message = "废气仪串口打开失败.";
                                }
                                groupBox3.Enabled = false;//如果是fla_501,则不提供零气和空气的调零选择
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                fla_501 = null;
                                Init_flag = false;
                            }
                            break;
                        default:break;
                    }
                }
            }
            catch (Exception)
            {

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
            initEquipment();
            Init_thread();
        }
        private void Init_form()
        {
            radioButtonLowB.Checked = true;
            radioButtonHuanj.Checked = true;
        }
        private void Init_thread()
        {
            th_fqdetect = new Thread(fq_detect);
            th_fqdetect.Start();
        }
        private void Msg(float hc, float no, float co, float co2, float pef, float o2)
        {
            setlabeltext(textEditHc, hc.ToString("0"));
            setlabeltext(textEditNo, no.ToString("0"));
            setlabeltext(textEditCo, co.ToString("0.00"));
            setlabeltext(textEditCo2, co2.ToString("0.00"));
            setlabeltext(textEditPEF, pef.ToString("0.000"));
            setlabeltext(textEditO2, o2.ToString("0.00"));
        }
        private void fq_detect()
        {
            try
            {
                float pef = 0f;
                switch (UseFqy)
                {
                    case "fla_502":
                        {
                            if (fla_502 != null)
                            {
                                while (true)
                                {
                                    Thread.Sleep(500);
                                    Exhaust.Fla502_data ex_temp = fla_502.GetData();
                                    pef = fla_502.Getdata_PEF();
                                    Msg(ex_temp.HC, ex_temp.NO, ex_temp.CO, ex_temp.CO2, pef, ex_temp.O2);
                                    Thread.Sleep(100);
                                }
                            }
                        }
                        break;
                    case "fla_501":
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
                int zero_count = 0;
                toolStripButtonTl.Enabled = false;
                toolStripButtonXljc.Enabled = false;
                toolStripButtonBd.Enabled = false;
                toolStripButtonSelfDetect.Enabled = false;
                groupBox3.Enabled = false;
                groupBox4.Enabled = false;
                groupBox7.Enabled = false;
                switch (UseFqy)
                {
                    case "fla_502":
                        if (fla_502 != null)
                        {
                            fla_502.Zeroing();
                            zero_count = 0;
                            while (fla_502.Get_Struct() == "调零中")
                            {
                                Thread.Sleep(900);
                                Msg_label(label_msg, panel_msg, "废气仪调零中..." + zero_count.ToString() + "s");
                                zero_count++;
                                if (zero_count == 60)
                                    break;
                            }
                            break;
                            Msg_label(label_msg, panel_msg, "调零成功");
                            toolStripButtonTl.Enabled = true;
                            toolStripButtonXljc.Enabled = true;
                            toolStripButtonBd.Enabled = true;
                            groupBox3.Enabled = true;
                            groupBox4.Enabled = true;
                            groupBox7.Enabled = true;
                            toolStripButtonSelfDetect.Enabled = true;
                        }
                        else
                        {
                            Msg_label(label_msg, panel_msg, "调零失败,检查仪器状态");
                            toolStripButtonTl.Enabled = true;
                            toolStripButtonXljc.Enabled = true;
                            toolStripButtonBd.Enabled = true;
                            groupBox3.Enabled = true;
                            groupBox4.Enabled = true;
                            groupBox7.Enabled = true;
                            toolStripButtonSelfDetect.Enabled = true;
                        }
                        break;
                    case "fla_501":
                        if (fla_501 != null)
                        {
                            fla_501.SetZero();
                            zero_count = 30;
                            while (zero_count > 0)
                            {
                                Thread.Sleep(900);
                                Msg_label(label_msg, panel_msg, "废气仪调零中..." + zero_count.ToString() + "s");
                                zero_count--;
                            }
                            Msg_label(label_msg, panel_msg, "调零成功");
                            toolStripButtonTl.Enabled = true;
                            toolStripButtonXljc.Enabled = true;
                            toolStripButtonBd.Enabled = true;
                            groupBox3.Enabled = true;
                            groupBox4.Enabled = true;
                            groupBox7.Enabled = true;
                            toolStripButtonSelfDetect.Enabled = true;
                        }
                        else
                        {
                            Msg_label(label_msg, panel_msg, "调零失败,检查仪器状态");
                            toolStripButtonTl.Enabled = true;
                            toolStripButtonXljc.Enabled = true;
                            toolStripButtonBd.Enabled = true;
                            groupBox3.Enabled = true;
                            groupBox4.Enabled = true;
                            groupBox7.Enabled = true;
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
                groupBox3.Enabled = true;
                groupBox4.Enabled = true;
                groupBox7.Enabled = true;
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
                case "fla_502":
                    if(fla_502!=null)
                        fla_502.Leak_check();
                    break;
                case "fla_501":
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
            string bdzt = "";
            switch (UseFqy)
            {
                case "fla_502":
                    if (fla_502 != null)
                    {
                        fla_502.Set_standardGas(double.Parse(textEditBiaoC3h8.Text), double.Parse(textEditBiaoCo.Text), double.Parse(textEditBiaoCo2.Text), double.Parse(textEditBiaoO2.Text), float.Parse(textEditBiaoNo.Text));//不标定氧气
                        Msg_label(label_msg, panel_msg, "请确认打开标定气,压力调整到(0.05~0.1)MPa");
                        if (MessageBox.Show("确认打开标定气？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "标定被停止");
                            return; 
                        }
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
                    }
                    break;
                case "fla_501":
                    MessageBox.Show("该型号废气仪未提供该功能,请进行手动标定", "系统提示");
                    break;
            }
        }

        private void toolStripButtonPumpOn_Click(object sender, EventArgs e)
        {
            switch (UseFqy)
            {
                case "fla_502":
                    fla_502.Pump_Pipeair();
                    break;
                case "fla_501":
                    fla_501.openbang();
                    break;
            }
        }

        private void toolStripButtonPumpOff_Click(object sender, EventArgs e)
        {
            switch (UseFqy)
            {
                case "fla_502":
                    fla_502.StopBlowback();
                    break;
                case "fla_501":
                    fla_501.Closebang();
                    break;
            }

        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            try
            {
                if(tl_bd!=null)
                    if (tl_bd.IsAlive)
                        tl_bd.Abort();
                if (tl_jc != null)
                    if (tl_jc.IsAlive)
                        tl_jc.Abort();
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
            if (tl_jc != null)
                if (tl_jc.IsAlive)
                    tl_jc.Abort();
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Close();

        }
        public void exit_form()
        {
            this.Close();
        }
        #region 信息显示 
        public void setlabeltext(DevExpress.XtraEditors.TextEdit textedit, string content)
        {
            try
            {
                BeginInvoke(new wtts(set_labeltext), textedit, content);
            }
            catch
            { }
        }
        public void set_labeltext(DevExpress.XtraEditors.TextEdit textedit, string content)
        {
            textedit.Text = content;
        }
        #endregion

        private void radioButtonHuanj_CheckedChanged(object sender, EventArgs e)
        {
            switch (UseFqy)
            {
                case "fla_502":
                    if (fla_502 != null)
                    {
                        fla_502.setAirAsTl();
                    }
                    break;
            }
        }

        private void toolStripButtonSelfDetect_Click(object sender, EventArgs e)
        {
            Thread tl_jc = new Thread(fqjc);
            tl_jc.Start();
        }
        private void fqjc()
        {
            string bdzt = "";
            switch (UseFqy)
            {
                case "fla_502":
                    if (fla_502 != null)
                    {
                        fla_502.Set_standardGas(double.Parse(textEditBiaoC3h8.Text), double.Parse(textEditBiaoCo.Text), double.Parse(textEditBiaoCo2.Text), double.Parse(textEditBiaoO2.Text), float.Parse(textEditBiaoNo.Text));//不标定氧气
                        Msg_label(label_msg, panel_msg, "请确认打开标定气,压力调整到(0.05~0.1)MPa");
                        if (MessageBox.Show("确认打开标定气？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "标定被停止");
                            return;
                        }
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
                    }
                    break;
                case "fla_501":
                    MessageBox.Show("该型号废气仪未提供该功能,请进行手动标定", "系统提示");
                    break;
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            string gdjz="0";//0——高校准，1——低校准
            float cowc = 0f, co2wc = 0f, hcwc = 0f, nowc = 0f, o2wc = 0f ;
            float coxdwc = 0f, co2xdwc = 0f, hcxdwc = 0f, noxdwc = 0f,o2xdwc=0f;
            bool copd = true, co2pd = true, hcpd = true, nopd = true,o2pd=true;
            float o2bz=0f;
            float o2clz=0f;
            if (radioButtonLowB.Checked == true)
            {
                analysisdata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                analysisdata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());
                cowc = Math.Abs(analysisdata.Cobz - analysisdata.Coclz);
                coxdwc = cowc / analysisdata.Cobz;
                if (cowc <= 0.02 || coxdwc <= 0.03)
                    copd = true;
                else
                    copd = false;
                analysisdata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                analysisdata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());
                co2wc = Math.Abs(analysisdata.Co2bz - analysisdata.Co2clz);
                co2xdwc = cowc / analysisdata.Co2bz;
                if (co2wc <= 0.3 || co2xdwc <= 0.03)
                    co2pd = true;
                else
                    co2pd = false;
                analysisdata.Nobz = float.Parse(textEditBiaoNo.Text.Trim());
                analysisdata.Noclz = float.Parse(textEdit_nojg.Text.Trim());
                nowc = Math.Abs(analysisdata.Nobz - analysisdata.Noclz);
                noxdwc = nowc / analysisdata.Nobz;
                if (nowc <= 25 || noxdwc <= 0.04)
                    nopd = true;
                else
                    nopd = false;
                analysisdata.Hcbz = float.Parse(textEditBiaoC3h8.Text.Trim());
                analysisdata.Hcclz = float.Parse(textEdit_hcjg.Text.Trim());
                hcwc = Math.Abs(analysisdata.Hcbz - analysisdata.Hcclz);
                hcxdwc = hcwc / analysisdata.Hcbz;
                if (hcwc <= 4 || hcxdwc <= 0.03)
                    hcpd = true;
                else
                    hcpd = false;
                analysisdata.Jzds = 1;
                analysisdata.Gdjz = "1";
                analysisdata.Gdjz = "0";
                analysisdata.Bzsm = "";
                if (copd == true && co2pd == true && hcpd == true && nopd == true)
                    analysisdata.Bdjg = "0";
                else
                    analysisdata.Bdjg = "1";
                analysismeterini.writeanalysismeterIni(analysisdata);
            }
            else if (radioButtonHighB.Checked == true)
            {
                analysisdata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                analysisdata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());
                cowc = Math.Abs(analysisdata.Cobz - analysisdata.Coclz);
                coxdwc = cowc / analysisdata.Cobz;
                if (coxdwc <= 0.05)
                    copd = true;
                else
                    copd = false;
                analysisdata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                analysisdata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());
                co2wc = Math.Abs(analysisdata.Co2bz - analysisdata.Co2clz);
                co2xdwc = cowc / analysisdata.Co2bz;
                if (co2wc <= 0.3 || co2xdwc <= 0.05)
                    co2pd = true;
                else
                    co2pd = false;
                analysisdata.Nobz = float.Parse(textEditBiaoNo.Text.Trim());
                analysisdata.Noclz = float.Parse(textEdit_nojg.Text.Trim());
                nowc = Math.Abs(analysisdata.Nobz - analysisdata.Noclz);
                noxdwc = nowc / analysisdata.Nobz;
                if (noxdwc <= 0.08)
                    nopd = true;
                else
                    nopd = false;
                analysisdata.Hcbz = float.Parse(textEditBiaoC3h8.Text.Trim());
                analysisdata.Hcclz = float.Parse(textEdit_hcjg.Text.Trim());
                hcwc = Math.Abs(analysisdata.Hcbz - analysisdata.Hcclz);
                hcxdwc = hcwc / analysisdata.Hcbz;
                if (hcxdwc <= 0.05)
                    hcpd = true;
                else
                    hcpd = false;
                analysisdata.Jzds = 1;
                analysisdata.Gdjz = "0";
                analysisdata.Bzsm = "";
                if (copd == true && co2pd == true && hcpd == true && nopd == true)
                    analysisdata.Bdjg = "0";
                else
                    analysisdata.Bdjg = "1";
                analysismeterini.writeanalysismeterIni(analysisdata);
            }
            else
            {
                o2bz = float.Parse(textEditBiaoO2.Text.Trim());
                o2clz = float.Parse(textEdit_o2jg.Text.Trim());
            }
        }

    }
}
