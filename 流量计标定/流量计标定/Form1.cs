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

namespace 流量计标定
{
    public partial class Form1 : Form
    {
        configIni configini = new configIni();
        equipmentConfigInfdata configdata = new equipmentConfigInfdata();
        flowmeterIni flowini = new flowmeterIni();
        Flowmeterdata flowdata = new Flowmeterdata();
        FlowmeterConfig flowmeterdata = new FlowmeterConfig();
        flowmeterConfigIni flowmeterini = new flowmeterConfigIni();
        public Thread th_fqdetect = null;
        public Exhaust.Flv_1000 flv_1000 = new Exhaust.Flv_1000();
        public delegate void wtts(TextBox textedit, string content);
        public delegate void wtdtview(DataGridView datagridview, string title, int row_number, string message);
        public delegate void wtlsb(Label Msgowner, string Msgstr);      //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                     //委托
        public delegate void wtcs(Control controlname, string text);                    //委托
        private bool isSaved = false;
        public Form1()
        {
            InitializeComponent();
        }
        private void Init_form()
        {
            radioButtonSssj.Checked = true;
        }
        private void Init_thread()
        {
            th_fqdetect = new Thread(fq_detect);
            th_fqdetect.Start();
        }
        private void fq_detect()
        {
            try
            {
                while (true)
                {
                    if (flv_1000 != null)
                    {
                        if (radioButtonSssj.Checked == true)
                        {
                            flv_1000.Get_unstandardDat();
                            setlabeltext(textEditll, flv_1000.ll_unstandard_value.ToString("0.00"));
                            setlabeltext(textBoxO2, flv_1000.o2_unstandard_value.ToString("0.00"));
                            setlabeltext(textBoxQtwd, flv_1000.temp_unstandard_value.ToString("0.00"));
                            setlabeltext(textBoxQtyl, flv_1000.yali_unstandard_value.ToString("0.00"));
                            Thread.Sleep(500);
                        }
                        else
                        {
                            flv_1000.Get_standardDat();
                            setlabeltext(textEditll, flv_1000.ll_standard_value.ToString("0.00"));
                            setlabeltext(textBoxO2, flv_1000.o2_standard_value.ToString("0.00"));
                            setlabeltext(textBoxQtwd, flv_1000.temp_standard_value.ToString("0.00"));
                            setlabeltext(textBoxQtyl, flv_1000.yali_standard_value.ToString("0.00"));
                            Thread.Sleep(500);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        private void initEquipment()
        {
            bool Init_flag = true;
            string init_message = "";
            try
            {
                if (configdata.Lljifpz == true)
                {
                    try
                    {
                        flv_1000 = new Exhaust.Flv_1000(configdata.Lljxh);
                        flv_1000.isNhSelfUse = configdata.isLljNhSelfUse;
                        if (flv_1000.Init_Comm(configdata.Lljck, "9600,N,8,1") == false)
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

            }
        }
        private void initConfigInfo()
        {
            configdata = configini.getEquipConfigIni();
        }
        #region 信息显示
        public void setlabeltext(TextBox textedit, string content)
        {
            try
            {
                BeginInvoke(new wtts(set_labeltext), textedit, content);
            }
            catch
            { }
        }
        public void set_labeltext(TextBox textedit, string content)
        {
            textedit.Text = content;
        }
        public void datagridview_msg(DataGridView datagridview, string title, int row_number, string message)
        {
            BeginInvoke(new wtdtview(dt_msg), datagridview, title, row_number, message);
        }
        public void dt_msg(DataGridView datagridview, string title, int row_number, string message)
        {
            datagridview.Rows[row_number].Cells[title].Value = message;
        }
        #endregion
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

        private void Form1_Load(object sender, EventArgs e)
        {
            flowdata.Bdjg = "-2";
            initConfigInfo();
            initFlowmeterdata();
            initEquipment();
            Init_thread();
            init_datagrid();
            
            radioButtonSssj.Checked = true;
            radioButtonLLsingle.Checked = true;
            radioButtonHighO2.Checked = true;
        }
        private void init_datagrid()
        {
            dataGridView1.Rows.Add();
            datagridview_msg(dataGridView1, "项目", 0, "低标气检查");
            datagridview_msg(dataGridView1, "项目", 1, "高标气检查");
            datagridview_msg(dataGridView1, "标定结果", 0, "未完成");
            datagridview_msg(dataGridView1, "标定结果", 1, "未完成");
            //datagridview_msg(dataGridView1, "备注说明", 0, flowdata.Bzsm);
            //datagridview_msg(dataGridView1, "备注说明", 1, flowdata.Bzsm);
        }
        private void initFlowmeterdata()
        {
            flowmeterdata = flowmeterini.getFlowmeterConfigIni();
            textBoxbzO2.Text = flowmeterdata.JcqO2_high.ToString("0.00");
            textEditBiaoO2.Text = flowmeterdata.BdqO2.ToString("0.00");
            radioButtonHigh.Checked = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (flv_1000 != null)
            {
                if (configdata.Lljxh != "nhf_1")
                {
                    string bdzt = flv_1000.Xishi_O2(float.Parse(textEditBiaoO2.Text.Trim()));
                    Msg_label(label_msg, panel_msg, bdzt);
                }
                else
                {
                    if (radioButtonHighO2.Checked)
                    {
                        string bdzt = flv_1000.nhf_O2High_demarcate(float.Parse(textEditBiaoO2.Text.Trim()));
                        Msg_label(label_msg, panel_msg, bdzt);
                    }
                    else if (radioButtonMidO2.Checked)
                    {
                        string bdzt = flv_1000.nhf_O2Mid_demarcate(float.Parse(textEditBiaoO2.Text.Trim()));
                        Msg_label(label_msg, panel_msg, bdzt);
                    }
                    else if (radioButtonLowO2.Checked)
                    {
                        string bdzt = flv_1000.nhf_O2low_demarcate(float.Parse(textEditBiaoO2.Text.Trim()));
                        Msg_label(label_msg, panel_msg, bdzt);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            float o2bzz = 0f;
            float o2scz = 0f;
            float llbzz = 0f;
            float llscz = 0f;
            float o2wc = 0f, llwc = 0f;
            float o2xdwc = 0f, llxdwc = 0f;
            flowdata.Bzsm = "";
            if(dataGridView1.Rows[0].Cells["标定结果"].Value.ToString()=="未完成"||dataGridView1.Rows[1].Cells["标定结果"].Value.ToString() == "未完成")
            {
                MessageBox.Show("保存失败，请先完成标定！","系统提示");
                return;
            }
            flowdata.O2glcbz = float.Parse(dataGridView1.Rows[1].Cells["氧气标准值"].Value.ToString());
            flowdata.O2glcclz = float.Parse(dataGridView1.Rows[1].Cells["氧气实测值"].Value.ToString());
            flowdata.O2glcwc = float.Parse(dataGridView1.Rows[1].Cells["氧气误差"].Value.ToString());
            flowdata.O2dlcbz = float.Parse(dataGridView1.Rows[0].Cells["氧气标准值"].Value.ToString());
            flowdata.O2dlcclz = float.Parse(dataGridView1.Rows[0].Cells["氧气实测值"].Value.ToString());
            flowdata.O2dlcwc = float.Parse(dataGridView1.Rows[0].Cells["氧气误差"].Value.ToString());
            if (dataGridView1.Rows[0].Cells["标定结果"].Value.ToString() == "合格" && dataGridView1.Rows[1].Cells["标定结果"].Value.ToString() == "合格")
                flowdata.Bdjg = "0";
            else
            {
                
                if (dataGridView1.Rows[0].Cells["标定结果"].Value.ToString() == "不合格")
                    flowdata.Bzsm += "低标不合格";
                if (dataGridView1.Rows[1].Cells["标定结果"].Value.ToString() == "不合格")
                    flowdata.Bzsm += "高标不合格";
                flowdata.Bdjg = "-1";
            }
            if (flowini.writeanalysismeterIni(flowdata))
            {
                MessageBox.Show("数据保存成功,请退出标定", "系统提示");
                isSaved = true;
            }
            else
                MessageBox.Show("数据保存失败", "系统提示");

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSaved)
            {
                if (MessageBox.Show("还未保存结果，确认退出？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    //flowdata.O2glcbz = 0;
                    //flowdata.O2glcclz = 0;
                    //flowdata.O2glcwc = 0;
                    //flowdata.O2dlcbz = 0;
                    //flowdata.O2dlcclz = 0;
                    //flowdata.O2dlcwc = 0;
                    //flowdata.Bdjg = "-2";
                    //flowdata.Bzsm = " ";
                    //flowini.writeanalysismeterIni(flowdata);
                    if (th_fqdetect != null)
                    {
                        if (th_fqdetect.IsAlive)
                            th_fqdetect.Abort();
                    }

                    if (flv_1000 != null)
                    {
                        try
                        {
                            if (flv_1000.ComPort_1.IsOpen)
                                flv_1000.ComPort_1.Close();
                        }
                        catch
                        { }
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                if (th_fqdetect != null)
                {
                    if (th_fqdetect.IsAlive)
                        th_fqdetect.Abort();
                }

                if (flv_1000 != null)
                {
                    try
                    {
                        if (flv_1000.ComPort_1.IsOpen)
                            flv_1000.ComPort_1.Close();
                    }
                    catch
                    { }
                }
            }

            
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButtonLow_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLow.Checked == true)
            {
                textBoxbzO2.Text = flowmeterdata.JcqO2_low.ToString("0.00");

            }
            else
            {
                textBoxbzO2.Text = flowmeterdata.JcqO2_high.ToString("0.00");
            }
        }

        private void buttonSavedata_Click(object sender, EventArgs e)
        {
            float o2bzz = 0f;
            float o2scz = 0f;
            float o2wc = 0f;
            float o2xdwc = 0f;
            string bdjg = "";
            o2bzz = float.Parse(textBoxbzO2.Text.Trim());
            o2scz = float.Parse(textBoxO2.Text.Trim());
            o2wc = o2scz - o2bzz;
            if (Math.Abs(o2wc) <= 0.5)
                bdjg = "合格";
            else
                bdjg = "不合格";
            o2xdwc = Math.Abs(o2scz - o2bzz) / o2bzz;
            if (radioButtonHigh.Checked == true)
            {
                //datagridview_msg(dataGridView1, "项目", 1, "低标气检查");
                datagridview_msg(dataGridView1, "氧气标准值", 1, o2bzz.ToString("0.00"));
                datagridview_msg(dataGridView1, "氧气实测值", 1, o2scz.ToString("0.00"));
                datagridview_msg(dataGridView1, "氧气误差", 1, o2wc.ToString("0.000"));
                datagridview_msg(dataGridView1, "标定结果", 1, bdjg);
                datagridview_msg(dataGridView1, "备注说明", 1, "");
            }
            else
            {
                //datagridview_msg(dataGridView1, "项目", 1, "低标气检查");
                datagridview_msg(dataGridView1, "氧气标准值", 0, o2bzz.ToString("0.00"));
                datagridview_msg(dataGridView1, "氧气实测值", 0, o2scz.ToString("0.00"));
                datagridview_msg(dataGridView1, "氧气误差", 0, o2wc.ToString("0.000"));
                datagridview_msg(dataGridView1, "标定结果", 0, bdjg);
                datagridview_msg(dataGridView1, "备注说明", 0, "");
            }
            
        }

        private void buttonO2zero_Click(object sender, EventArgs e)
        {
            if (flv_1000 != null)
            {
                string bdzt = flv_1000.nhf_SetO2zero();
                Msg_label(label_msg, panel_msg, bdzt);
            }
        }

        private void buttonLLdemarcate_Click(object sender, EventArgs e)
        {
            if (flv_1000 != null)
            {
                if (configdata.Lljxh == "nhf_1")
                {
                    if (radioButtonHighLL.Checked)
                    {
                        string bdzt = flv_1000.nhf_LLhigh_demarcate(float.Parse(textBoxBiaoLL.Text.Trim()) *60f/28.3168f);
                        Msg_label(label_msg, panel_msg, bdzt);
                    }
                    else if (radioButtonLowLL.Checked)
                    {
                        string bdzt = flv_1000.nhf_LLlow_demarcate(float.Parse(textBoxBiaoLL.Text.Trim()) * 60f / 28.3168f);
                        Msg_label(label_msg, panel_msg, bdzt);
                    }
                    else if (radioButtonLLsingle.Checked)
                    {
                        string bdzt = flv_1000.nhf_LLsingle_demarcate(float.Parse(textBoxBiaoLL.Text.Trim()) * 60f / 28.3168f);
                        Msg_label(label_msg, panel_msg, bdzt);
                    }
                }
            }
        }

        private void buttonYldemarcate_Click(object sender, EventArgs e)
        {
            if (flv_1000 != null)
            {
                string bdzt = flv_1000.nhf_YLsingle_demarcate(float.Parse(textBoxYL.Text.Trim()));
                Msg_label(label_msg, panel_msg, bdzt);
            }
        }

        private void buttonTempdemarcate_Click(object sender, EventArgs e)
        {
            if (flv_1000 != null)
            {
                string bdzt = flv_1000.nhf_Tempsingle_demarcate(float.Parse(textBoxTemp.Text.Trim()));
                Msg_label(label_msg, panel_msg, bdzt);
            }
        }

        private void buttonTurnOnMotor_Click(object sender, EventArgs e)
        {
            if (flv_1000 != null)
            {
                string bdzt = flv_1000.nhf_TurnOnMotor();
                Msg_label(label_msg, panel_msg, bdzt);
            }
        }

        private void buttonTurnOffMotor_Click(object sender, EventArgs e)
        {
            if (flv_1000 != null)
            {
                string bdzt = flv_1000.nhf_TurnOffMotor();
                Msg_label(label_msg, panel_msg, bdzt);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认恢复到出厂设置？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (flv_1000 != null)
                {
                    string bdzt = flv_1000.reset_default();
                    Msg_label(label_msg, panel_msg, bdzt);
                }
            } 
        }
    }
}
