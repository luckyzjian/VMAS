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

namespace 烟度计标定
{
    public partial class Form1 : Form
    {
        equipmentConfigInfdata equipconfig = new equipmentConfigInfdata();
        BtgConfigInfdata btgconfig = new BtgConfigInfdata();
        CSVcontrol.CSVwriter csvwriter = new CSVcontrol.CSVwriter();
        configIni configini = new configIni();
        Exhaust.FLB_100 flb_100 = null;
        Exhaust.Fla502 fla_502 = null;
        Exhaust.Flb_100_smoke smoke = null;

        DataTable dt_speed = null;
        Smokemeterdata smokedata = new Smokemeterdata();
        SmokemeterIni smokedataini = new SmokemeterIni();
        public delegate void wtlsb(Label Msgowner, string Msgstr, bool Update_DB);                //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                              //委托
        bool isDermacate=false;
        bool isSaved = false;
        private double ydjN = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.StartPosition =FormStartPosition.CenterScreen;
            radioButtonN.Checked = true;
            initConfigInfo();
            initEquipment();
            dt_speed = null;
            dt_speed = new DataTable();
            dt_speed.Columns.Add("开始时间");
            dt_speed.Columns.Add("标准值(%)");
            dt_speed.Columns.Add("实测值(%)");
            dt_speed.Columns.Add("误差(%)");
            dt_speed.Columns.Add("判定");
            dataGridView1.DataSource = dt_speed;
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

        private void initConfigInfo()
        {
            equipconfig = configini.getEquipConfigIni();
            btgconfig = configini.getBtgConfigIni();
            //configdata = configini.getConfigIni();
        }
        public void initEquipment()
        {
            bool Init_flag = true;
            string init_message = "";

            //这里只初始化了废气分析仪其他设备要继续初始化
            
            try
            {
                if (equipconfig.Ydjifpz == true)
                {
                    if (equipconfig.Ydjxh != "CDF5000")
                    {
                        try
                        {
                            flb_100 = new Exhaust.FLB_100(equipconfig.Ydjxh);
                            flb_100.isNhSelfUse = equipconfig.isYdjNhSelfUse;
                            if (flb_100.Init_Comm(equipconfig.Ydjck, "9600,N,8,1") == false)
                            {
                                flb_100 = null;
                                Init_flag = false;
                                init_message += "烟度计串口打开失败.";
                                Msg(label_msg, panel_msg, init_message, false);
                            }
                            else if (equipconfig.Ydjxh != "nht_1")
                            {
                                string ydjzt = flb_100.Get_Mode();
                                if (ydjzt.Contains("故障"))
                                {
                                    ydjzt = flb_100.Get_Mode();
                                    if (ydjzt.Contains("故障"))
                                    {
                                        init_message += "烟度计通讯异常";
                                        Msg(label_msg, panel_msg, init_message, false);
                                        return;
                                    }
                                    else if (ydjzt.Contains("预热"))
                                    {
                                        init_message += "烟度计正在预热";
                                        Msg(label_msg, panel_msg, init_message, false);
                                        return;
                                    }
                                }
                                else if (ydjzt.Contains("预热"))
                                {
                                    init_message += "烟度计正在预热";
                                    Msg(label_msg, panel_msg, init_message, false);
                                    return;
                                }
                                flb_100.Set_Measure();
                                Thread.Sleep(1000);
                                timer1.Start();
                            }
                            else
                            {
                                flb_100.Set_Measure();
                                Thread.Sleep(1000);
                                timer1.Start();
                            }
                        }
                        catch (Exception er)
                        {
                            flb_100 = null;
                            Init_flag = false;

                            MessageBox.Show(er.ToString(), "出错啦");
                        }
                    }
                    else if (fla_502 == null)
                    {
                        try
                        {
                            
                            fla_502 = new Exhaust.Fla502("cdf5000");
                            fla_502.isNhSelfUse = equipconfig.isFqyNhSelfUse;
                            if (fla_502.Init_Comm(equipconfig.Fqyck, equipconfig.Fqyckpzz) == false)
                            {
                                fla_502 = null;
                                Init_flag = false;
                                init_message = "废气仪串口打开失败.";
                                return;
                            }
                            fla_502.Set_Measure();
                            Thread.Sleep(1000);
                            timer1.Start();
                        }
                        catch (Exception er)
                        {
                            MessageBox.Show(er.ToString(), "出错啦");
                            fla_502 = null;
                            Init_flag = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                smoke = fla_502.get_DirectData(0.01f);
            else
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
            ydjN = Math.Round((1 - Math.Exp((double)(equipconfig.YdjL) * 1.0 * Math.Log(1 - smoke.Ns / 100.0) / 430.0)) * 100, 1);
            textBoxN.Text = ydjN.ToString("0.0");
            textEditNs.Text = smoke.Ns.ToString("0.0");
            textBoxK.Text = smoke.K.ToString("0.00");
            textBox1.Text = smoke.Zs.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (equipconfig.Ydjxh.ToLower() == "cdf5000")
                fla_502.set_linearDem();
            else
            {
                if (flb_100 != null)
                    flb_100.set_linearDem();
            }
        }

        private void buttonSavedata_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dr = dt_speed.NewRow();
                dr["开始时间"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                dr["标准值(%)"] = textBoxbzNs.Text;
                double scz = 0;
                if (radioButtonN.Checked) scz = double.Parse(textBoxN.Text.Trim());
                else scz = double.Parse(textEditNs.Text.Trim());
                dr["实测值(%)"] = scz.ToString();
                dr["误差(%)"] = (double.Parse(textBoxbzNs.Text.Trim()) - scz).ToString();
                dr["判定"] = (Math.Abs(double.Parse(textBoxbzNs.Text.Trim()) - scz) > 2.0) ? "不合格" : "合格";
                dt_speed.Rows.Add(dr);
                dataGridView1.DataSource = dt_speed;
                smokedata.Kbz = float.Parse(textBoxbzNs.Text);
                smokedata.Kscz = (float)scz;
                smokedata.Kabswc = smokedata.Kscz - smokedata.Kbz;
                smokedata.Krelwc = smokedata.Kabswc / smokedata.Kbz;
                smokedata.Bdjg = (Math.Abs(double.Parse(textBoxbzNs.Text.Trim()) - scz) > 2.0) ? "不合格" : "合格";
                smokedata.Bzsm = "";
                isDermacate = true;
            }
            catch
            {
                MessageBox.Show("输入烟度值格式有误，请检查！", "添加失败！");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                List<DataRow> drlist = new List<DataRow>();

                if (dataGridView1.SelectedRows.Count > 0)
                {
                    for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                        foreach (DataRow dr in dt_speed.Rows)
                        {
                            if (dr["标准值(%)"].ToString() == dataGridView1.SelectedRows[i].Cells["标准值(%)"].Value.ToString())
                                drlist.Add(dr);
                        }
                    if (drlist.Count != 0)
                        foreach (DataRow drd in drlist)
                        {
                            dt_speed.Rows.Remove(drd);
                            
                        }
                    dataGridView1.DataSource = dt_speed;
                }
                else
                {
                    MessageBox.Show("请选择要删除的点", "系统提示");
                }
            }
            catch (Exception)
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isDermacate)
            {
                csvwriter.SaveCSV(dt_speed, "C:/jcdatatxt/" + "SmokemeterCal.csv");
                smokedataini.writeanalysismeterIni(smokedata);
                isSaved = true;
                MessageBox.Show("结果保存成功,请退出标定。", "系统提示！");
            }
            else
            {
                MessageBox.Show("没有可保存的数据,请重新标定。", "系统提示！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSaved)
            {
                if (MessageBox.Show("还未保存数据，确认退出吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    try
                    {
                        if (flb_100 != null)
                        {
                            if (flb_100.ComPort_3.IsOpen)
                                flb_100.ComPort_3.Close();
                        }
                    }
                    catch
                    { }
                    try
                    {
                        if (fla_502 != null)
                        {
                            if (fla_502.ComPort_1.IsOpen)
                                fla_502.ComPort_1.Close();
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
                    if (flb_100 != null)
                    {
                        if (flb_100.ComPort_3.IsOpen)
                            flb_100.ComPort_3.Close();
                    }
                }
                catch
                { }
                try
                {
                    if (fla_502 != null)
                    {
                        if (fla_502.ComPort_1.IsOpen)
                            fla_502.ComPort_1.Close();
                    }
                }
                catch
                { }

            }
        }
    }
}
