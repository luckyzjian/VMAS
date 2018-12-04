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

namespace 废气仪标定
{

    public partial class Form1 : Form
    {
        public int GKSJ = 0, perGKSJ = 0;                                                                        //工况时间
        public float gongkuangTime = 0f;
        private int gksjcount = 0;
        private bool isdermarcateMq = false;
        public int[] TimeCountlist = new int[10240];
        public float[] HClist = new float[10240];                                               //每秒速度数组
        public float[] COlist = new float[10240];                                               //每秒扭力数
        public float[] CO2list = new float[10240];
        public float[] O2list = new float[10240];
        public float[] NOlist = new float[10240];                                               //每秒扭力数
        public float[] PEFlist = new float[10240];
        public string[] STATUSlist = new string[10240];
        private bool isSaveStartTime = false;
        CSVcontrol.CSVwriter csvwriter = new CSVcontrol.CSVwriter();
        private DateTime startTime, nowtime;
        private string status = "0";

        configIni configini = new configIni();
        equipmentConfigInfdata configdata = new equipmentConfigInfdata();
        fqyconfigIni fqyconfigini = new fqyconfigIni();
        fqyconfigInfdata fqyconfigdata = new fqyconfigInfdata();
        private string UseFqy = "";
        public static Exhaust.Fla502 fla_502 = null;
        public static Exhaust.Fla501 fla_501 = null;
        public Thread th_fqdetect = null;
        public Thread tl_bd = null;
        public Thread tl_jc = null;
        public Thread tl_fq = null;
        public delegate void wtts(TextBox textedit, string content);
        public delegate void wtdtview(DataGridView datagridview, string title, int row_number, string message);
        private bool Init_Flag = false;
        public delegate void wtlsb(Label Msgowner, string Msgstr);      //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                     //委托
        public delegate void wtcs(Control controlname, string text);                    //委托

        public delegate void wtcv(Control controlname, bool isvisible);                    //委托
        analysismeterInidata analysisdata = new analysismeterInidata();
        analysismeterIni analysismeterini = new analysismeterIni();
        demacatefqy demacatedata = new demacatefqy();
        demacatefqyIni demarcatefqyini = new demacatefqyIni();
        private string action = "准备状态";
        private bool isSaved = false;
        public Form1()
        {
            
            InitializeComponent();
            initfqyConfigInfo();
            String[] CmdArgs = System.Environment.GetCommandLineArgs();
            if (CmdArgs.Length <= 1)
            {
                radioButtonLowB.Checked = true;
            }
            else
            {
                if (CmdArgs[1] == "L")
                {
                    radioButtonLowB.Checked = true;
                    this.Text = "废气仪检查";
                    toolStripButtonBd.Visible = false;
                }
                else if (CmdArgs[1] == "H")
                {
                    radioButtonHighB.Checked = true;
                    this.Text = "废气仪检查";
                    toolStripButtonBd.Visible = false;
                }
                else if (CmdArgs[1] == "B")
                {
                    radioButtonHighB.Checked = true;
                    this.Text = "废气仪标定";
                    toolStripButtonSelfDetect.Visible = false;
                }
            }

        }
        private void Init_form()
        {
            //radioButtonLowB.Checked = true;
            radioButtonHuanj.Checked = true;
        }
        private void Init_thread()
        {
            th_fqdetect = new Thread(fq_detect);
            th_fqdetect.Start();
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
                        case "nha_503":
                            try
                            {
                                UseFqy = "nha_503";
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
                        case "nha_506":
                            try
                            {
                                UseFqy = "nha_506";
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
                        case "fasm_5000":
                            try
                            {
                                UseFqy = "fasm_5000";
                                fla_502 = new Exhaust.Fla502(UseFqy);
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
                                UseFqy = configdata.Fqyxh.ToLower();
                                fla_502 = new Exhaust.Fla502(UseFqy);
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
                                fla_502 = new Exhaust.Fla502(UseFqy);
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
                                if (fla_501.Init_Comm(configdata.Fqyck, configdata.Fqyckpzz) == false)
                                {
                                    fla_501 = null;
                                    Init_flag = false;
                                    init_message = "废气仪串口打开失败.";
                                }
                                groupBox3.Enabled = false;//如果是fla_501,则不提供零气和空气的调零选择
                                groupBox7.Enabled = false;
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                fla_501 = null;
                                Init_flag = false;
                            }
                            break;
                        default: break;
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
        private void initfqyConfigInfo()
        {
            fqyconfigdata = fqyconfigini.getfqyConfigIni();
        }
        private void init_wcchart()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            switch (UseFqy)
            {
                case "mqw_50a":
                case "mqw_50b":

                    if (radioButtonLowB.Checked == true)
                    {
                        //datagridview_msg(dataGridView1, "标定项目", 0, "低标气检查");
                        dt.Columns.Add("气体");
                        dt.Columns.Add("绝对值");
                        dt.Columns.Add("相对值");
                        dr = dt.NewRow();
                        dr["气体"] = "HC";
                        dr["绝对值"] = "4";
                        dr["相对值"] = "3%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO";
                        dr["绝对值"] = "0.02";
                        dr["相对值"] = "3%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO2";
                        dr["绝对值"] = "0.3";
                        dr["相对值"] = "3%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "NO";
                        dr["绝对值"] = "25";
                        dr["相对值"] = "4%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "O2";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dataGridView2.DataSource = dt;
                    }
                    else if (radioButtonHighB.Checked == true)
                    {
                        //DataTable dt = new DataTable();
                        //DataRow dr = null;
                        dt.Columns.Add("气体");
                        dt.Columns.Add("绝对值");
                        dt.Columns.Add("相对值");
                        dr = dt.NewRow();
                        dr["气体"] = "HC";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO2";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "NO";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "8%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "O2";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dataGridView2.DataSource = dt;
                    }
                    else
                    {
                        dt.Columns.Add("气体");
                        dt.Columns.Add("绝对值");
                        dt.Columns.Add("相对值");
                        dr = dt.NewRow();
                        dr["气体"] = "HC";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO2";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "NO";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "O2";
                        dr["绝对值"] = "0.1";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dataGridView2.DataSource = dt;
                    }
                    break;
                case "cdf5000":

                    if (radioButtonLowB.Checked == true)
                    {
                        //datagridview_msg(dataGridView1, "标定项目", 0, "低标气检查");
                        dt.Columns.Add("气体");
                        dt.Columns.Add("绝对值");
                        dt.Columns.Add("相对值");
                        dr = dt.NewRow();
                        dr["气体"] = "HC";
                        dr["绝对值"] = "4";
                        dr["相对值"] = "3%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO";
                        dr["绝对值"] = "0.02";
                        dr["相对值"] = "3%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO2";
                        dr["绝对值"] = "0.3";
                        dr["相对值"] = "3%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "NO";
                        dr["绝对值"] = "25";
                        dr["相对值"] = "4%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "O2";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dataGridView2.DataSource = dt;
                    }
                    else if (radioButtonHighB.Checked == true)
                    {
                        //DataTable dt = new DataTable();
                        //DataRow dr = null;
                        dt.Columns.Add("气体");
                        dt.Columns.Add("绝对值");
                        dt.Columns.Add("相对值");
                        dr = dt.NewRow();
                        dr["气体"] = "HC";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO2";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "NO";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "8%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "O2";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dataGridView2.DataSource = dt;
                    }
                    else
                    {
                        dt.Columns.Add("气体");
                        dt.Columns.Add("绝对值");
                        dt.Columns.Add("相对值");
                        dr = dt.NewRow();
                        dr["气体"] = "HC";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO2";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "NO";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "O2";
                        dr["绝对值"] = "0.1";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dataGridView2.DataSource = dt;
                    }
                    break;
                case "fla_502":

                    if (radioButtonLowB.Checked == true)
                    {
                        //datagridview_msg(dataGridView1, "标定项目", 0, "低标气检查");
                        dt.Columns.Add("气体");
                        dt.Columns.Add("绝对值");
                        dt.Columns.Add("相对值");
                        dr = dt.NewRow();
                        dr["气体"] = "HC";
                        dr["绝对值"] = "4";
                        dr["相对值"] = "3%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO";
                        dr["绝对值"] = "0.02";
                        dr["相对值"] = "3%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO2";
                        dr["绝对值"] = "0.3";
                        dr["相对值"] = "3%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "NO";
                        dr["绝对值"] = "25";
                        dr["相对值"] = "4%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "O2";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dataGridView2.DataSource = dt;
                    }
                    else if (radioButtonHighB.Checked == true)
                    {
                        //DataTable dt = new DataTable();
                        //DataRow dr = null;
                        dt.Columns.Add("气体");
                        dt.Columns.Add("绝对值");
                        dt.Columns.Add("相对值");
                        dr = dt.NewRow();
                        dr["气体"] = "HC";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO2";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "NO";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "8%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "O2";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dataGridView2.DataSource = dt;
                    }
                    else
                    {
                        dt.Columns.Add("气体");
                        dt.Columns.Add("绝对值");
                        dt.Columns.Add("相对值");
                        dr = dt.NewRow();
                        dr["气体"] = "HC";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO2";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "NO";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "O2";
                        dr["绝对值"] = "0.1";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dataGridView2.DataSource = dt;
                    }
                    break;
                case "fasm_5000":

                    if (radioButtonLowB.Checked == true)
                    {
                        //datagridview_msg(dataGridView1, "标定项目", 0, "低标气检查");
                        dt.Columns.Add("气体");
                        dt.Columns.Add("绝对值");
                        dt.Columns.Add("相对值");
                        dr = dt.NewRow();
                        dr["气体"] = "HC";
                        dr["绝对值"] = "4";
                        dr["相对值"] = "3%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO";
                        dr["绝对值"] = "0.02";
                        dr["相对值"] = "3%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO2";
                        dr["绝对值"] = "0.3";
                        dr["相对值"] = "3%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "NO";
                        dr["绝对值"] = "25";
                        dr["相对值"] = "4%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "O2";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dataGridView2.DataSource = dt;
                    }
                    else if (radioButtonHighB.Checked == true)
                    {
                        //DataTable dt = new DataTable();
                        //DataRow dr = null;
                        dt.Columns.Add("气体");
                        dt.Columns.Add("绝对值");
                        dt.Columns.Add("相对值");
                        dr = dt.NewRow();
                        dr["气体"] = "HC";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO2";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "NO";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "8%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "O2";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dataGridView2.DataSource = dt;
                    }
                    else
                    {
                        dt.Columns.Add("气体");
                        dt.Columns.Add("绝对值");
                        dt.Columns.Add("相对值");
                        dr = dt.NewRow();
                        dr["气体"] = "HC";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO2";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "NO";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "O2";
                        dr["绝对值"] = "0.1";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dataGridView2.DataSource = dt;
                    }
                    break;
                case "nha_503":

                    if (radioButtonLowB.Checked == true)
                    {
                        //datagridview_msg(dataGridView1, "标定项目", 0, "低标气检查");
                        dt.Columns.Add("气体");
                        dt.Columns.Add("绝对值");
                        dt.Columns.Add("相对值");
                        dr = dt.NewRow();
                        dr["气体"] = "HC";
                        dr["绝对值"] = "4";
                        dr["相对值"] = "3%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO";
                        dr["绝对值"] = "0.02";
                        dr["相对值"] = "3%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO2";
                        dr["绝对值"] = "0.3";
                        dr["相对值"] = "3%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "NO";
                        dr["绝对值"] = "25";
                        dr["相对值"] = "4%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "O2";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dataGridView2.DataSource = dt;
                    }
                    else if (radioButtonHighB.Checked == true)
                    {
                        //DataTable dt = new DataTable();
                        //DataRow dr = null;
                        dt.Columns.Add("气体");
                        dt.Columns.Add("绝对值");
                        dt.Columns.Add("相对值");
                        dr = dt.NewRow();
                        dr["气体"] = "HC";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO2";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "NO";
                        dr["绝对值"] = "N/A";
                        dr["相对值"] = "8%";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "O2";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dataGridView2.DataSource = dt;
                    }
                    else
                    {
                        dt.Columns.Add("气体");
                        dt.Columns.Add("绝对值");
                        dt.Columns.Add("相对值");
                        dr = dt.NewRow();
                        dr["气体"] = "HC";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "CO2";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "NO";
                        dr["绝对值"] = "--";
                        dr["相对值"] = "--";
                        dt.Rows.Add(dr);
                        dr = dt.NewRow();
                        dr["气体"] = "O2";
                        dr["绝对值"] = "0.1";
                        dr["相对值"] = "5%";
                        dt.Rows.Add(dr);
                        dataGridView2.DataSource = dt;
                    }
                    break;
                case "fla_501":
                    // DataTable dt = new DataTable();
                    // DataRow dr = null;
                    dt.Columns.Add("气体");
                    dt.Columns.Add("绝对值");
                    dt.Columns.Add("相对值");
                    dr = dt.NewRow();
                    dr["气体"] = "HC";
                    dr["绝对值"] = "12";
                    dr["相对值"] = "5%";
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["气体"] = "CO";
                    dr["绝对值"] = "0.06";
                    dr["相对值"] = "5%";
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["气体"] = "CO2";
                    dr["绝对值"] = "0.05";
                    dr["相对值"] = "5%";
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["气体"] = "NO";
                    dr["绝对值"] = "--";
                    dr["相对值"] = "--";
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["气体"] = "O2";
                    dr["绝对值"] = "0.1";
                    dr["相对值"] = "5%";
                    dt.Rows.Add(dr);
                    dataGridView2.DataSource = dt;
                    break;
                case "mqw_511":
                    // DataTable dt = new DataTable();
                    // DataRow dr = null;
                    dt.Columns.Add("气体");
                    dt.Columns.Add("绝对值");
                    dt.Columns.Add("相对值");
                    dr = dt.NewRow();
                    dr["气体"] = "HC";
                    dr["绝对值"] = "12";
                    dr["相对值"] = "5%";
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["气体"] = "CO";
                    dr["绝对值"] = "0.06";
                    dr["相对值"] = "5%";
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["气体"] = "CO2";
                    dr["绝对值"] = "0.05";
                    dr["相对值"] = "5%";
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["气体"] = "NO";
                    dr["绝对值"] = "--";
                    dr["相对值"] = "--";
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    dr["气体"] = "O2";
                    dr["绝对值"] = "0.1";
                    dr["相对值"] = "5%";
                    dt.Rows.Add(dr);
                    dataGridView2.DataSource = dt;
                    break;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            analysisdata.Bdjg = "-2";
            Init_form();
            initConfigInfo();
            initEquipment();
            switch (UseFqy)//初始化调零气体为环境气
            {
                case "fla_502":
                    if (fla_502 != null)
                    {
                        fla_502.setAirAsTl();
                    }
                    break;
                case "cdf5000":
                    if (fla_502 != null)
                    {
                        fla_502.setAirAsTl();
                    }
                    break;
                case "nha_503":
                    if (fla_502 != null)
                    {
                        fla_502.setAirAsTl();
                    }
                    break;
                case "fasm_5000":
                    if (fla_502 != null)
                    {
                        fla_502.setAirAsTl();
                    }
                    break;
            }
            Init_thread();
            init_wcchart();
            if (radioButtonLowB.Checked == true)
            {
                textEditBiaoC3h8.Text = fqyconfigdata.Hc_low.ToString("0.0");
                textEditBiaoCo.Text = fqyconfigdata.Co_low.ToString("0.000");
                textEditBiaoCo2.Text = fqyconfigdata.Co2_low.ToString("0.00");
                textEditBiaoNo.Text = fqyconfigdata.No_low.ToString("0");
                textEditBiaoO2.Text = fqyconfigdata.O2_low.ToString("0.0");
                init_wcchart();
            }
            //dataGridView1.Rows.Add();
        }
        private void Msg(float hc, float no, float co, float co2, float pef, float o2)
        {
            setlabeltext(textEditHc, hc.ToString("0"));
            setlabeltext(textEditNo, no.ToString("0"));
            setlabeltext(textEditCo, co.ToString("0.000"));
            setlabeltext(textEditCo2, co2.ToString("0.00"));
            setlabeltext(textEditPEF, pef.ToString("0.000"));
            setlabeltext(textEditO2, o2.ToString("0.00"));
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
        #region
        /// <summary>
        /// 刷新控件的文字信息
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <param name="text">文字信息</param>
        public void Ref_Control_visible(Control control, bool isvisible)
        {
            BeginInvoke(new wtcv(ref_Control_visible), control, isvisible);
        }

        public void ref_Control_visible(Control control, bool isvisible)
        {
            control.Visible = isvisible;
        }
        #endregion
        private void fq_detect()
        {
            try
            {
                float pef = 0f;
                if (fla_502 != null)
                    if (fla_502.Get_Struct() == "预热状态")
                    {
                        Msg_label(label_msg, panel_msg, "废气仪正在预热,请稍后再试");
                        return;
                    }
                while (true)
                {

                    Msg_label(label_tishi, panel_tishi, action);
                    if (action != "标定中" && action != "调零中")//如果在标定状态下则不显示
                    {

                        switch (UseFqy)
                        {
                            case "fla_502":
                                {
                                    if (fla_502 != null)
                                    {
                                        Thread.Sleep(500);
                                        Exhaust.Fla502_data ex_temp = fla_502.GetData();
                                        Thread.Sleep(100);
                                        pef = fla_502.Getdata_PEF();
                                        Msg(ex_temp.HC, ex_temp.NO, ex_temp.CO, ex_temp.CO2, pef, ex_temp.O2);
                                        Thread.Sleep(100);
                                    }
                                    else
                                        Thread.Sleep(500);
                                }
                                break;
                            case "cdf5000":
                                {
                                    if (fla_502 != null)
                                    {
                                        Thread.Sleep(500);
                                        Exhaust.Fla502_data ex_temp = fla_502.GetData();
                                        Thread.Sleep(100);
                                        pef = fla_502.Getdata_PEF();
                                        Msg(ex_temp.HC, ex_temp.NO, ex_temp.CO, ex_temp.CO2, pef, ex_temp.O2);
                                        Thread.Sleep(100);
                                    }
                                    else
                                        Thread.Sleep(500);
                                }
                                break;
                            case "nha_503":
                                {
                                    if (fla_502 != null)
                                    {
                                        Thread.Sleep(500);
                                        Exhaust.Fla502_data ex_temp = fla_502.GetData();
                                        Thread.Sleep(100);
                                        pef = fla_502.Getdata_PEF();
                                        Msg(ex_temp.HC, ex_temp.NO, ex_temp.CO, ex_temp.CO2, pef, ex_temp.O2);
                                        Thread.Sleep(100);
                                    }
                                    else
                                        Thread.Sleep(500);
                                }
                                break;
                            case "nha_506":
                                {
                                    if (fla_502 != null)
                                    {
                                        Thread.Sleep(500);
                                        Exhaust.Fla502_data ex_temp = fla_502.GetData();
                                        Thread.Sleep(100);
                                        pef = fla_502.Getdata_PEF();
                                        Msg(ex_temp.HC, ex_temp.NO, ex_temp.CO, ex_temp.CO2, pef, ex_temp.O2);
                                        Thread.Sleep(100);
                                    }
                                    else
                                        Thread.Sleep(500);
                                }
                                break;
                            case "mqw_50b":
                                {
                                    if (fla_502 != null)
                                    {
                                        Thread.Sleep(500);
                                        Exhaust.Fla502_data ex_temp = fla_502.GetData();
                                        Thread.Sleep(100);
                                        pef = fla_502.Getdata_PEF();
                                        Msg(ex_temp.HC, ex_temp.NO, ex_temp.CO, ex_temp.CO2, pef, ex_temp.O2);
                                        Thread.Sleep(100);
                                    }
                                    else
                                        Thread.Sleep(500);
                                }
                                break;
                            case "mqw_50a":
                                {
                                    if (fla_502 != null)
                                    {
                                        Thread.Sleep(500);
                                        Exhaust.Fla502_data ex_temp = fla_502.GetData();
                                        Thread.Sleep(100);
                                        pef = fqyconfigdata.PEF;
                                        Msg(ex_temp.HC, ex_temp.NO, ex_temp.CO, ex_temp.CO2, pef, ex_temp.O2);
                                        Thread.Sleep(100);
                                    }
                                    else
                                        Thread.Sleep(500);
                                }
                                break;
                            case "mqw_511":
                                {
                                    if (fla_502 != null)
                                    {
                                        Thread.Sleep(500);
                                        Exhaust.Fla502_data ex_temp = fla_502.GetData();
                                        Thread.Sleep(100);
                                        pef = fqyconfigdata.PEF;
                                        Msg(ex_temp.HC, ex_temp.NO, ex_temp.CO, ex_temp.CO2, pef, ex_temp.O2);
                                        Thread.Sleep(100);
                                    }
                                    else
                                        Thread.Sleep(500);
                                }
                                break;
                            case "fasm_5000":
                                {
                                    if (fla_502 != null)
                                    {
                                        Thread.Sleep(500);
                                        Exhaust.Fla502_data ex_temp = fla_502.GetData();
                                        Thread.Sleep(100);
                                        pef = fla_502.Getdata_PEF();
                                        Msg(ex_temp.HC, ex_temp.NO, ex_temp.CO, ex_temp.CO2, pef, ex_temp.O2);
                                        Thread.Sleep(100);
                                    }
                                    else
                                        Thread.Sleep(500);
                                }
                                break;
                            case "fla_501":
                                {
                                    if (fla_501 != null)
                                    {
                                        Thread.Sleep(500);
                                        Exhaust.Fla501_data ex_temp = fla_501.Get_Data();
                                        pef = fla_501.Getdata_PEF();
                                        Msg(ex_temp.HC, ex_temp.NO, ex_temp.CO, ex_temp.CO2, pef, ex_temp.O2);
                                        Thread.Sleep(100);
                                    }
                                    else
                                        Thread.Sleep(500);
                                }
                                break;
                            default: Thread.Sleep(500); break;
                        }
                    }
                    else if (action == "标定中")
                    {
                        switch (UseFqy)
                        {
                            case "fla_502":
                                if (fla_502 != null)
                                {
                                    Msg(fla_502.hc_density, fla_502.no_density, fla_502.co_density, fla_502.co2_density, fla_502.pef_value, fla_502.o2_density);
                                    Thread.Sleep(500);
                                }
                                break;
                            case "cdf5000":
                                if (fla_502 != null)
                                {
                                    Msg(fla_502.hc_density, fla_502.no_density, fla_502.co_density, fla_502.co2_density, fla_502.pef_value, fla_502.o2_density);
                                    Thread.Sleep(500);
                                }
                                break;
                            case "nha_503":
                                if (fla_502 != null)
                                {
                                    Msg(fla_502.hc_density, fla_502.no_density, fla_502.co_density, fla_502.co2_density, fla_502.pef_value, fla_502.o2_density);
                                    Thread.Sleep(500);
                                }
                                break;
                            case "fasm_5000":
                                if (fla_502 != null)
                                {
                                    Msg(fla_502.hc_density, fla_502.no_density, fla_502.co_density, fla_502.co2_density, fla_502.pef_value, fla_502.o2_density);
                                    Thread.Sleep(500);
                                }
                                break;
                            case "mqw_50a":
                            case "mqw_50b":
                                if (fla_502 != null)
                                {
                                    //Msg(fla_502.hc_density, fla_502.no_density, fla_502.co_density, fla_502.co2_density, fla_502.pef_value, fla_502.o2_density);
                                    Thread.Sleep(500);
                                }
                                break;
                            case "mqw_511":
                                if (fla_502 != null)
                                {
                                    //Msg(fla_502.hc_density, fla_502.no_density, fla_502.co_density, fla_502.co2_density, fla_502.pef_value, fla_502.o2_density);
                                    Thread.Sleep(500);
                                }
                                break;
                            case "fla_501":
                                if (fla_501 != null)
                                {
                                    Thread.Sleep(500);
                                    Exhaust.Fla501_data ex_temp = fla_501.Get_Data();
                                    pef = fla_501.Getdata_PEF();
                                    Msg(ex_temp.HC, ex_temp.NO, ex_temp.CO, ex_temp.CO2, pef, ex_temp.O2);
                                    Thread.Sleep(100);
                                }
                                else
                                    Thread.Sleep(500);
                                break;
                        }
                        Thread.Sleep(500);
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
                textEditBiaoC3h8.Text = fqyconfigdata.Hc_high.ToString("0.0");
                textEditBiaoCo.Text = fqyconfigdata.Co_high.ToString("0.000");
                textEditBiaoCo2.Text = fqyconfigdata.Co2_high.ToString("0.00");
                textEditBiaoNo.Text = fqyconfigdata.No_high.ToString("0");
                textEditBiaoO2.Text = fqyconfigdata.O2_high.ToString("0.0");
                init_wcchart();
            }
        }
        private void zeroO2checked(object sender, EventArgs e)
        {
            if (radioButtonBiaolq.Checked == true)
            {
                textEditBiaoC3h8.Text = fqyconfigdata.Hc_zero.ToString("0.0");
                textEditBiaoCo.Text = fqyconfigdata.Co_zero.ToString("0.000");
                textEditBiaoCo2.Text = fqyconfigdata.Co2_zero.ToString("0.00");
                textEditBiaoNo.Text = fqyconfigdata.No_zero.ToString("0");
                textEditBiaoO2.Text = fqyconfigdata.O2_zero.ToString("0.0");
                init_wcchart();
            }
        }
        private void low_biaoQiChecked(object sender, EventArgs e)
        {
            if (radioButtonLowB.Checked == true)
            {
                textEditBiaoC3h8.Text = fqyconfigdata.Hc_low.ToString("0.0");
                textEditBiaoCo.Text = fqyconfigdata.Co_low.ToString("0.000");
                textEditBiaoCo2.Text = fqyconfigdata.Co2_low.ToString("0.00");
                textEditBiaoNo.Text = fqyconfigdata.No_low.ToString("0");
                textEditBiaoO2.Text = fqyconfigdata.O2_low.ToString("0.0");
                init_wcchart();
            }
        }

        private void toolStripButtonTl_Click(object sender, EventArgs e)
        {
            if (action == "准备状态")
            {
                th_fqdetect.Suspend();
                tl_fq = new Thread(fqtl);
                tl_fq.Start();
            }
            else
            {
                MessageBox.Show("仪器正在" + action + ",请先关闭");
            }
        }
        private void fqtl()
        {
            action = "调零中";
            int zero_count = 0;
            string tlzd = "调零中";
            switch (UseFqy)
            {

                case "fla_502":
                    if (fla_502 != null)
                    {

                        fla_502.Zeroing();
                        zero_count = 0;
                        Thread.Sleep(1000);
                        while (tlzd == "调零中")
                        {
                            tlzd = fla_502.Get_Struct();
                            Thread.Sleep(900);
                            Msg_label(label_msg, panel_msg, "废气仪调零中..." + zero_count.ToString() + "s");
                            zero_count++;
                            if (zero_count == 60)
                                break;
                        }
                        Msg_label(label_msg, panel_msg, "调零成功");
                    }
                    else
                    {
                        Msg_label(label_msg, panel_msg, "调零失败,检查仪器状态");
                    }
                    break;
                case "cdf5000":
                    if (fla_502 != null)
                    {

                        fla_502.Zeroing();
                        zero_count = 0;
                        Thread.Sleep(1000);
                        while (zero_count<=40)
                        {
                            tlzd = fla_502.Get_Struct();
                            Thread.Sleep(900);
                            Msg_label(label_msg, panel_msg, "废气仪调零中..." + zero_count.ToString() + "s");
                            zero_count++;
                            if (zero_count == 60)
                                break;
                        }
                        Msg_label(label_msg, panel_msg, "调零成功");
                    }
                    else
                    {
                        Msg_label(label_msg, panel_msg, "调零失败,检查仪器状态");
                    }
                    break;
                case "nha_503":
                    if (fla_502 != null)
                    {

                        fla_502.Zeroing();
                        zero_count = 0;
                        Thread.Sleep(1000);
                        while (fla_502.Zeroing() == false)//该处需要测试后定
                        {
                            Thread.Sleep(900);
                            Msg_label(label_msg, panel_msg, "废气仪调零中..." + zero_count.ToString() + "s");
                            zero_count++;
                            if (zero_count == 60)
                                break;
                        }
                        
                        Msg_label(label_msg, panel_msg, "调零成功");
                    }
                    else
                    {
                        Msg_label(label_msg, panel_msg, "调零失败,检查仪器状态");
                    }
                    break;
                case "fasm_5000":
                    if (fla_502 != null)
                    {
                        fla_502.Zeroing();
                        zero_count = 0;
                        Thread.Sleep(1000);
                        while (tlzd == "调零中")
                        {
                            tlzd = fla_502.Get_Struct();
                            Thread.Sleep(900);
                            Msg_label(label_msg, panel_msg, "废气仪调零中..." + zero_count.ToString() + "s");
                            zero_count++;
                            if (zero_count == 60)
                                break;
                        }
                        Msg_label(label_msg, panel_msg, "调零成功");
                    }
                    else
                    {
                        Msg_label(label_msg, panel_msg, "调零失败,检查仪器状态");
                    }
                    break;
                case "mqw_50a":
                case "mqw_50b":
                    if (fla_502 != null)
                    {
                        fla_502.Zeroing();
                        zero_count = 0;
                        Thread.Sleep(1000);
                        while (tlzd == "调零中")
                        {
                            tlzd = fla_502.Get_Struct();
                            Thread.Sleep(900);
                            Msg_label(label_msg, panel_msg, "废气仪调零中..." + zero_count.ToString() + "s");
                            zero_count++;
                            if (zero_count == 60)
                                break;
                        }
                        Msg_label(label_msg, panel_msg, "调零成功");
                    }
                    else
                    {
                        Msg_label(label_msg, panel_msg, "调零失败,检查仪器状态");
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
                    }
                    else
                    {
                        Msg_label(label_msg, panel_msg, "调零失败,检查仪器状态");
                    }
                    break;
            }
            action = "准备状态";
            th_fqdetect.Resume();

        }

        private void toolStripButtonBd_Click(object sender, EventArgs e)
        {
            if (action == "准备状态")
            {
                Thread tl_bd = new Thread(fqbd);
                tl_bd.Start();
            }
            else
            {
                MessageBox.Show("仪器正在" + action + ",请先关闭");
            }
        }
        private void fqbd()
        {
            string bdzt = "";
            action = "标定中";
            float pef = 0f;
            switch (UseFqy)
            {
                case "fla_502":
                    if (fla_502 != null)
                    {
                        fla_502.Stop();
                        if (MessageBox.Show("确认标准气浓度填写的浓度值与标准气罐的标称值一致？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "标定被停止");
                            return;
                        }
                        fla_502.Set_standardGas(double.Parse(textEditBiaoC3h8.Text), double.Parse(textEditBiaoCo.Text), double.Parse(textEditBiaoCo2.Text), double.Parse(textEditBiaoO2.Text), float.Parse(textEditBiaoNo.Text));//不标定氧气
                        Msg_label(label_msg, panel_msg, "请确认打开标定气,压力调整到(0.05~0.1)MPa");
                        if (MessageBox.Show("确认打开标定气？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "标定被停止");
                            return;
                        }
                        Msg_label(label_msg, panel_msg, "开始标定");
                        bdzt = fla_502.Demarcate();//第一次发送时没有命令返回
                        Thread.Sleep(100);
                        while (true)
                        {
                            Thread.Sleep(100);
                            bdzt = fla_502.Demarcate();
                            if (bdzt == "标定中" || bdzt == "仪器通讯失败")
                            {
                                Msg_label(label_msg, panel_msg, "正在标定");
                            }
                            else
                                break;
                        }
                        Msg_label(label_msg, panel_msg, bdzt);
                        Thread.Sleep(1000);
                        Exhaust.Fla502_data resultdata = fla_502.GetData();
                        Thread.Sleep(100);
                        pef = fla_502.Getdata_PEF();
                        Msg(resultdata.HC, resultdata.NO, resultdata.CO, resultdata.CO2, pef, resultdata.O2);
                        setlabeltext(textEdit_Cojg, textEditCo.Text);
                        setlabeltext(textEdit_co2jg, textEditCo2.Text);
                        setlabeltext(textEdit_hcjg, textEditHc.Text);
                        setlabeltext(textEdit_nojg, textEditNo.Text);
                        setlabeltext(textEdit_pefjg, pef.ToString("0.000"));
                        setlabeltext(textEdit_o2jg, textEditO2.Text);
                        /*if (radioButtonLowB.Checked == true)
                        {

                            demacatedata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                            demacatedata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());
                            demacatedata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                            demacatedata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());

                            demacatedata.Nobz = float.Parse(textEditBiaoNo.Text.Trim());
                            demacatedata.Noclz = float.Parse(textEdit_nojg.Text.Trim());

                            demacatedata.Hcbz = float.Parse(textEditBiaoC3h8.Text.Trim()) * float.Parse(textEdit_pefjg.Text.Trim());
                            demacatedata.Hcclz = float.Parse(textEdit_hcjg.Text.Trim());

                            demacatedata.Jzds = 1;
                            demacatedata.Gdjz = "1";
                            demacatedata.Bzsm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());
                            demacatedata.Bdjg = "合格";
                            demarcatefqyini.writeanalysismeterIni(demacatedata);
                            MessageBox.Show("记录已保存");
                        }
                        else if (radioButtonHighB.Checked == true)
                        {
                            demacatedata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                            demacatedata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());

                            demacatedata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                            demacatedata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());

                            demacatedata.Nobz = float.Parse(textEditBiaoNo.Text.Trim());
                            demacatedata.Noclz = float.Parse(textEdit_nojg.Text.Trim());

                            demacatedata.Hcbz = float.Parse(textEditBiaoC3h8.Text.Trim()) * float.Parse(textEdit_pefjg.Text.Trim());
                            demacatedata.Hcclz = float.Parse(textEdit_hcjg.Text.Trim());

                            demacatedata.Jzds = 4;
                            demacatedata.Gdjz = "0";
                            demacatedata.Bzsm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());

                            demacatedata.Bdjg = "合格";

                            demarcatefqyini.writeanalysismeterIni(demacatedata);
                            MessageBox.Show("记录已保存");
                            //analysismeterini.writeanalysismeterIni(analysisdata);
                        }*/

                        if (jiancha_detect())
                            Msg_label(label_jcjg, panel_jcjg, "通过");
                        else
                            Msg_label(label_jcjg, panel_jcjg, "未通过");
                    }
                    break;
                case "cdf5000":
                    if (fla_502 != null)
                    {
                        fla_502.Stop();
                        if (MessageBox.Show("确认标准气浓度填写的浓度值与标准气罐的标称值一致？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "标定被停止");
                            return;
                        }
                        fla_502.Set_standardGas(double.Parse(textEditBiaoC3h8.Text), double.Parse(textEditBiaoCo.Text), double.Parse(textEditBiaoCo2.Text), double.Parse(textEditBiaoO2.Text), float.Parse(textEditBiaoNo.Text));//不标定氧气
                        Msg_label(label_msg, panel_msg, "请确认打开标定气,压力调整到(0.05~0.1)MPa");
                        if (MessageBox.Show("确认打开标定气？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "标定被停止");
                            return;
                        }
                        Msg_label(label_msg, panel_msg, "开始标定");
                        bdzt = fla_502.Demarcate();//第一次发送时没有命令返回
                        Thread.Sleep(100);
                        while (true)
                        {
                            Thread.Sleep(100);
                            bdzt = fla_502.Demarcate();
                            if (bdzt == "标定中" || bdzt == "仪器通讯失败")
                            {
                                Msg_label(label_msg, panel_msg, "正在标定");
                            }
                            else
                                break;
                        }
                        Msg_label(label_msg, panel_msg, bdzt);
                        Thread.Sleep(1000);
                        Exhaust.Fla502_data resultdata = fla_502.GetData();
                        Thread.Sleep(100);
                        float pefnow = fqyconfigdata.PEF;
                        Msg(resultdata.HC, resultdata.NO, resultdata.CO, resultdata.CO2, resultdata.PEF, resultdata.O2);
                        setlabeltext(textEdit_Cojg, textEditCo.Text);
                        setlabeltext(textEdit_co2jg, textEditCo2.Text);
                        setlabeltext(textEdit_hcjg, textEditHc.Text);
                        setlabeltext(textEdit_nojg, textEditNo.Text);
                        setlabeltext(textEdit_pefjg, textEditPEF.Text);
                        setlabeltext(textEdit_o2jg, textEditO2.Text);
                        /*
                        if (radioButtonLowB.Checked == true)
                        {

                            demacatedata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                            demacatedata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());
                            demacatedata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                            demacatedata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());

                            demacatedata.Nobz = float.Parse(textEditBiaoNo.Text.Trim());
                            demacatedata.Noclz = float.Parse(textEdit_nojg.Text.Trim());

                            demacatedata.Hcbz = float.Parse(textEditBiaoC3h8.Text.Trim()) * float.Parse(textEdit_pefjg.Text.Trim());
                            demacatedata.Hcclz = float.Parse(textEdit_hcjg.Text.Trim());

                            demacatedata.Jzds = 1;
                            demacatedata.Gdjz = "1";
                            demacatedata.Bzsm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());
                            demacatedata.Bdjg = "合格";
                            demarcatefqyini.writeanalysismeterIni(demacatedata);
                            MessageBox.Show("记录已保存");
                        }
                        else if (radioButtonHighB.Checked == true)
                        {
                            demacatedata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                            demacatedata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());

                            demacatedata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                            demacatedata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());

                            demacatedata.Nobz = float.Parse(textEditBiaoNo.Text.Trim());
                            demacatedata.Noclz = float.Parse(textEdit_nojg.Text.Trim());

                            demacatedata.Hcbz = float.Parse(textEditBiaoC3h8.Text.Trim()) * float.Parse(textEdit_pefjg.Text.Trim());
                            demacatedata.Hcclz = float.Parse(textEdit_hcjg.Text.Trim());

                            demacatedata.Jzds = 4;
                            demacatedata.Gdjz = "0";
                            demacatedata.Bzsm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());

                            demacatedata.Bdjg = "合格";

                            demarcatefqyini.writeanalysismeterIni(demacatedata);
                            MessageBox.Show("记录已保存");
                            //analysismeterini.writeanalysismeterIni(analysisdata);
                        }*/
                        if (jiancha_detect())
                            Msg_label(label_jcjg, panel_jcjg, "通过");
                        else
                            Msg_label(label_jcjg, panel_jcjg, "未通过");
                    }
                    break;
                case "nha_503":
                    if (fla_502 != null)
                    {
                        fla_502.Stop();
                        if (MessageBox.Show("确认标准气浓度填写的浓度值与标准气罐的标称值一致？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "标定被停止");
                            return;
                        }
                        fla_502.Set_standardGas(double.Parse(textEditBiaoC3h8.Text), double.Parse(textEditBiaoCo.Text), double.Parse(textEditBiaoCo2.Text), double.Parse(textEditBiaoO2.Text), float.Parse(textEditBiaoNo.Text));//不标定氧气
                        Msg_label(label_msg, panel_msg, "请确认打开标定气,压力调整到(0.05~0.1)MPa");
                        if (MessageBox.Show("确认打开标定气？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "标定被停止");
                            return;
                        }
                        Msg_label(label_msg, panel_msg, "开始标定");
                        bdzt = fla_502.Demarcate();//第一次发送时没有命令返回
                        Thread.Sleep(100);
                        while (true)
                        {
                            Thread.Sleep(100);
                            bdzt = fla_502.Demarcate();
                            if (bdzt == "标定中" || bdzt == "仪器通讯失败")
                            {
                                Msg_label(label_msg, panel_msg, "正在标定");
                            }
                            else
                                break;
                        }
                        Msg_label(label_msg, panel_msg, bdzt);
                        Thread.Sleep(1000);
                        Exhaust.Fla502_data resultdata = fla_502.GetData();
                        Thread.Sleep(100);
                        pef = fla_502.Getdata_PEF();
                        Msg(resultdata.HC, resultdata.NO, resultdata.CO, resultdata.CO2, pef, resultdata.O2);
                        setlabeltext(textEdit_Cojg, textEditCo.Text);
                        setlabeltext(textEdit_co2jg, textEditCo2.Text);
                        setlabeltext(textEdit_hcjg, textEditHc.Text);
                        setlabeltext(textEdit_nojg, textEditNo.Text);
                        setlabeltext(textEdit_pefjg, pef.ToString("0.000"));
                        setlabeltext(textEdit_o2jg, textEditO2.Text);
                        /*
                        if (radioButtonLowB.Checked == true)
                        {

                            demacatedata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                            demacatedata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());
                            demacatedata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                            demacatedata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());

                            demacatedata.Nobz = float.Parse(textEditBiaoNo.Text.Trim());
                            demacatedata.Noclz = float.Parse(textEdit_nojg.Text.Trim());

                            demacatedata.Hcbz = float.Parse(textEditBiaoC3h8.Text.Trim()) * float.Parse(textEdit_pefjg.Text.Trim());
                            demacatedata.Hcclz = float.Parse(textEdit_hcjg.Text.Trim());

                            demacatedata.Jzds = 1;
                            demacatedata.Gdjz = "1";
                            demacatedata.Bzsm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());
                            demacatedata.Bdjg = "合格";
                            demarcatefqyini.writeanalysismeterIni(demacatedata);
                            MessageBox.Show("记录已保存");
                        }
                        else if (radioButtonHighB.Checked == true)
                        {
                            demacatedata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                            demacatedata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());

                            demacatedata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                            demacatedata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());

                            demacatedata.Nobz = float.Parse(textEditBiaoNo.Text.Trim());
                            demacatedata.Noclz = float.Parse(textEdit_nojg.Text.Trim());

                            demacatedata.Hcbz = float.Parse(textEditBiaoC3h8.Text.Trim()) * float.Parse(textEdit_pefjg.Text.Trim());
                            demacatedata.Hcclz = float.Parse(textEdit_hcjg.Text.Trim());

                            demacatedata.Jzds = 4;
                            demacatedata.Gdjz = "0";
                            demacatedata.Bzsm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());

                            demacatedata.Bdjg = "合格";

                            demarcatefqyini.writeanalysismeterIni(demacatedata);
                            MessageBox.Show("记录已保存");
                            //analysismeterini.writeanalysismeterIni(analysisdata);
                        }*/
                        if (jiancha_detect())
                            Msg_label(label_jcjg, panel_jcjg, "通过");
                        else
                            Msg_label(label_jcjg, panel_jcjg, "未通过");
                    }
                    break;
                case "fasm_5000":
                    if (fla_502 != null)
                    {
                        fla_502.Stop();
                        if (MessageBox.Show("确认标准气浓度填写的浓度值与标准气罐的标称值一致？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "标定被停止");
                            return;
                        }
                        //fla_502.Set_standardGas(double.Parse(textEditBiaoC3h8.Text), double.Parse(textEditBiaoCo.Text), double.Parse(textEditBiaoCo2.Text), double.Parse(textEditBiaoO2.Text), float.Parse(textEditBiaoNo.Text));//不标定氧气
                        Msg_label(label_msg, panel_msg, "请确认打开标定气,压力调整到(0.05~0.1)MPa");
                        if (MessageBox.Show("确认打开标定气？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "标定被停止");
                            return;
                        }
                        fla_502.Open_standardGas();
                        Msg_label(label_msg, panel_msg, "开始标定,等待数据稳定后点击【进行标定】");

                        //bdzt = fla_502.Demarcate();//第一次发送时没有命令返回
                        Thread.Sleep(1000);

                        isdermarcateMq = false;
                        Ref_Control_visible(buttonDemarcateMQ, true);
                        while (!isdermarcateMq)
                        {
                            Thread.Sleep(500);
                            Exhaust.Fla502_data ex_temp = fla_502.GetData();
                            Thread.Sleep(100);
                            pef = fqyconfigdata.PEF;
                            Msg(ex_temp.HC, ex_temp.NO, ex_temp.CO, ex_temp.CO2, pef, ex_temp.O2);
                            Thread.Sleep(100);
                        }
                        if (fla_502.Set_standardGas(double.Parse(textEditBiaoC3h8.Text), double.Parse(textEditBiaoCo.Text), double.Parse(textEditBiaoCo2.Text), double.Parse(textEditBiaoO2.Text), float.Parse(textEditBiaoNo.Text)))//不标定氧气
                            Msg_label(label_msg, panel_msg, "标定成功");
                        else
                            Msg_label(label_msg, panel_msg, "标定失败");
                        Thread.Sleep(500);
                        fla_502.StopBlowback();
                        Ref_Control_visible(buttonDemarcateMQ, false);
                        Thread.Sleep(1000);
                        Exhaust.Fla502_data resultdata = fla_502.GetData();
                        Thread.Sleep(100);
                        pef= fqyconfigdata.PEF;
                        Msg(resultdata.HC, resultdata.NO, resultdata.CO, resultdata.CO2, pef, resultdata.O2);
                        
                        setlabeltext(textEdit_Cojg, textEditCo.Text);
                        setlabeltext(textEdit_co2jg, textEditCo2.Text);
                        setlabeltext(textEdit_hcjg, textEditHc.Text);
                        setlabeltext(textEdit_nojg, textEditNo.Text);
                        setlabeltext(textEdit_pefjg, pef.ToString("0.000"));
                        setlabeltext(textEdit_o2jg, textEditO2.Text);

                        if (jiancha_detect())
                            Msg_label(label_jcjg, panel_jcjg, "通过");
                        else
                            Msg_label(label_jcjg, panel_jcjg, "未通过");
                    }
                    break;
                case "mqw_50a":
                case "mqw_50b":
                    if (fla_502 != null)
                    {
                        fla_502.Stop();
                        if (MessageBox.Show("确认标准气浓度填写的浓度值与标准气罐的标称值一致？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "标定被停止");
                            return;
                        }
                        //fla_502.Set_standardGas(double.Parse(textEditBiaoC3h8.Text), double.Parse(textEditBiaoCo.Text), double.Parse(textEditBiaoCo2.Text), double.Parse(textEditBiaoO2.Text), float.Parse(textEditBiaoNo.Text));//不标定氧气
                        Msg_label(label_msg, panel_msg, "请确认打开标定气,压力调整到(0.05~0.1)MPa");
                        if (MessageBox.Show("确认打开标定气？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "标定被停止");
                            return;
                        }
                        fla_502.Open_standardGas();
                        Msg_label(label_msg, panel_msg, "开始标定,等待数据稳定后点击【进行标定】");

                        //bdzt = fla_502.Demarcate();//第一次发送时没有命令返回
                        Thread.Sleep(1000);

                        isdermarcateMq = false;
                        Ref_Control_visible(buttonDemarcateMQ, true);
                        while (!isdermarcateMq)
                        {
                            Thread.Sleep(500);
                            Exhaust.Fla502_data ex_temp = fla_502.GetData();
                            Thread.Sleep(100);
                            if (UseFqy == "mqw_50b")
                                pef = fla_502.Getdata_PEF();
                            else
                                pef = fqyconfigdata.PEF;
                            Msg(ex_temp.HC, ex_temp.NO, ex_temp.CO, ex_temp.CO2, pef, ex_temp.O2);
                            Thread.Sleep(100);
                        }
                        if (fla_502.Set_standardGas(double.Parse(textEditBiaoC3h8.Text), double.Parse(textEditBiaoCo.Text), double.Parse(textEditBiaoCo2.Text), double.Parse(textEditBiaoO2.Text), float.Parse(textEditBiaoNo.Text)))//不标定氧气
                            Msg_label(label_msg, panel_msg, "标定成功");
                        else
                            Msg_label(label_msg, panel_msg, "标定失败");
                        Thread.Sleep(500);
                        fla_502.StopBlowback();
                        Ref_Control_visible(buttonDemarcateMQ, false);
                        Thread.Sleep(1000);
                        Exhaust.Fla502_data resultdata = fla_502.GetData();
                        Thread.Sleep(100);
                        float pefnow = pef;
                        Msg(resultdata.HC, resultdata.NO, resultdata.CO, resultdata.CO2, pefnow, resultdata.O2);
                        setlabeltext(textEdit_Cojg, textEditCo.Text);
                        setlabeltext(textEdit_co2jg, textEditCo2.Text);
                        setlabeltext(textEdit_hcjg, textEditHc.Text);
                        setlabeltext(textEdit_nojg, textEditNo.Text);
                        setlabeltext(textEdit_pefjg, textEditPEF.Text);
                        setlabeltext(textEdit_o2jg, textEditO2.Text);
                        /*
                        if (radioButtonLowB.Checked == true)
                        {

                            demacatedata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                            demacatedata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());
                            demacatedata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                            demacatedata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());

                            demacatedata.Nobz = float.Parse(textEditBiaoNo.Text.Trim());
                            demacatedata.Noclz = float.Parse(textEdit_nojg.Text.Trim());

                            demacatedata.Hcbz = float.Parse(textEditBiaoC3h8.Text.Trim()) * float.Parse(textEdit_pefjg.Text.Trim());
                            demacatedata.Hcclz = float.Parse(textEdit_hcjg.Text.Trim());

                            demacatedata.Jzds = 1;
                            demacatedata.Gdjz = "1";
                            demacatedata.Bzsm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());
                            demacatedata.Bdjg = "合格";
                            demarcatefqyini.writeanalysismeterIni(demacatedata);
                            MessageBox.Show("记录已保存");
                        }
                        else if (radioButtonHighB.Checked == true)
                        {
                            demacatedata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                            demacatedata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());

                            demacatedata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                            demacatedata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());

                            demacatedata.Nobz = float.Parse(textEditBiaoNo.Text.Trim());
                            demacatedata.Noclz = float.Parse(textEdit_nojg.Text.Trim());

                            demacatedata.Hcbz = float.Parse(textEditBiaoC3h8.Text.Trim()) * float.Parse(textEdit_pefjg.Text.Trim());
                            demacatedata.Hcclz = float.Parse(textEdit_hcjg.Text.Trim());

                            demacatedata.Jzds = 4;
                            demacatedata.Gdjz = "0";
                            demacatedata.Bzsm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());

                            demacatedata.Bdjg = "合格";

                            demarcatefqyini.writeanalysismeterIni(demacatedata);
                            MessageBox.Show("记录已保存");
                            //analysismeterini.writeanalysismeterIni(analysisdata);
                        }
                        */

                        if (jiancha_detect())
                            Msg_label(label_jcjg, panel_jcjg, "通过");
                        else
                            Msg_label(label_jcjg, panel_jcjg, "未通过");
                    }
                    break;
                case "fla_501":
                    if (fla_501 != null)
                    {
                        MessageBox.Show("该型号废气仪未提供该功能,请进行手动标定", "系统提示");
                    }
                    break;
            }
            action = "准备状态";
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            try
            {
                Ref_Control_visible(buttonDemarcateMQ, false);
                isSaveStartTime = false;
                if (tl_bd != null)
                    if (tl_bd.IsAlive)
                    {
                        tl_bd.Abort();
                        Msg_label(label_msg, panel_msg, "标定被停止");
                    }
                if (tl_jc != null)
                    if (tl_jc.IsAlive)
                    {
                        tl_jc.Abort();
                        Msg_label(label_msg, panel_msg, "检查被停止");
                    }
                if (tl_fq != null)
                    if (tl_fq.IsAlive)
                    {
                        tl_fq.Abort();
                        Msg_label(label_msg, panel_msg, "调零被停止");
                        th_fqdetect.Resume();
                    }
                switch (UseFqy)
                {
                    case "fla_502":
                        if (fla_502 != null) fla_502.Stop();
                        action = "准备状态";
                        break;
                    case "cdf5000":
                        if (fla_502 != null) fla_502.Stop();
                        action = "准备状态";
                        break;
                    case "nha_503":
                        if (fla_502 != null) fla_502.Stop();
                        action = "准备状态";
                        break;
                    case "mqw_50a":
                    case "mqw_50b":
                        if (fla_502 != null) fla_502.Stop();
                        action = "准备状态";
                        break;
                    case "mqw_511":
                        if (fla_502 != null) fla_502.Stop();
                        action = "准备状态";
                        break;
                    case "fasm_5000":
                        if (fla_502 != null) fla_502.Stop();
                        action = "准备状态";
                        break;
                    case "fla_501":
                        action = "准备状态";
                        break;
                }

            }
            catch
            { }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSaved)
            {
                if (MessageBox.Show("还未保存结果，确认退出？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
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

        }

        private void checkBox_yskq_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_yskq.Checked == true)
            {
                if (fla_502 != null)
                    fla_502.Blowback();
                checkBox_bzq.Checked = false;
                checkBox_lq.Checked = false;
                checkBox_jcq.Checked = false;
                checkBox_pump.Checked = false;
                checkBox_hjq.Checked = false;
                action = "通压缩空气中";
            }
            else
            {
                fla_502.StopBlowback();
                action = "准备状态";
            }
        }

        private void checkBox_lq_Click(object sender, EventArgs e)
        {
            if (checkBox_lq.Checked == true)
            {
                if (fla_502 != null)
                    fla_502.Open_zeroGas();
                checkBox_bzq.Checked = false;
                checkBox_yskq.Checked = false;
                checkBox_jcq.Checked = false;
                checkBox_pump.Checked = false;
                checkBox_hjq.Checked = false;
                action = "通零气中";
            }
            else
            {
                fla_502.Close_zeroGas();
                action = "准备状态";
            }
        }

        private void checkBox_bzq_Click(object sender, EventArgs e)
        {
            if (checkBox_bzq.Checked == true)
            {
                if (fla_502 != null)
                    fla_502.Open_standardGas();
                checkBox_lq.Checked = false;
                checkBox_yskq.Checked = false;
                checkBox_jcq.Checked = false;
                checkBox_pump.Checked = false;
                checkBox_hjq.Checked = false;
                action = "通标准气中";
            }
            else
            {
                fla_502.Close_standardGas();
                action = "准备状态";
            }
        }

        private void checkBox_jcq_Click(object sender, EventArgs e)
        {
            if (checkBox_jcq.Checked == true)
            {
                if (fla_502 != null)
                    fla_502.Open_testGas();
                checkBox_lq.Checked = false;
                checkBox_yskq.Checked = false;
                checkBox_bzq.Checked = false;
                checkBox_pump.Checked = false;
                checkBox_hjq.Checked = false;
                action = "通检查气中";
            }
            else
            {
                fla_502.Close_testGas();
                action = "准备状态";
            }
        }

        private void checkBox_pump_Click(object sender, EventArgs e)
        {
            if (checkBox_pump.Checked == true)
            {
                if (fla_502 != null)
                    fla_502.Pump_air();
                checkBox_lq.Checked = false;
                checkBox_yskq.Checked = false;
                checkBox_jcq.Checked = false;
                checkBox_bzq.Checked = false;
                checkBox_hjq.Checked = false;
                action = "抽样气中";
            }
            else
            {
                fla_502.StopBlowback();
                action = "准备状态";
            }
        }

        private void checkBox_hjq_Click(object sender, EventArgs e)
        {
            if (checkBox_hjq.Checked == true)
            {
                if (fla_502 != null)
                    fla_502.Pump_air();
                checkBox_lq.Checked = false;
                checkBox_yskq.Checked = false;
                checkBox_jcq.Checked = false;
                checkBox_bzq.Checked = false;
                checkBox_pump.Checked = false;
                action = "通环境气中";
            }
            else
            {
                fla_502.StopBlowback();
                action = "准备状态";
            }
        }

        private void toolStripButtonSelfDetect_Click(object sender, EventArgs e)
        {

            if (action == "准备状态")
            {
                Thread tl_jc = new Thread(fqjc);
                tl_jc.Start();
                timer2.Start();
                isSaveStartTime = false;
            }
            else
            {
                MessageBox.Show("仪器正在" + action + ",请先关闭");
            }
        }
        private void fqjc()
        {
            string bdzt = "";
            int dzsj = 60;
            action = "检查中";
            switch (UseFqy)
            {
                case "fla_502":
                    if (fla_502 != null)
                    {
                        //fla_502.Set_standardGas(double.Parse(textEditBiaoC3h8.Text), double.Parse(textEditBiaoCo.Text), double.Parse(textEditBiaoCo2.Text), double.Parse(textEditBiaoO2.Text), float.Parse(textEditBiaoNo.Text));//不标定氧气
                        if (MessageBox.Show("确认标准气浓度填写的浓度值与标准气罐的标称值一致？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "检查被停止");
                            return;
                        }
                        Msg_label(label_msg, panel_msg, "请确认打开检查气气罐,压力调整到(0.05~0.1)MPa");
                        Thread.Sleep(500);
                        if (MessageBox.Show("确认打开检查气气罐？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "检查被停止");
                            return;
                        }
                        if (fla_502 != null)
                            fla_502.Open_testGas();
                        status = "3";
                        startTime = DateTime.Now;
                        isSaveStartTime = true;
                        Msg_label(label_msg, panel_msg, "开始通检查气，60秒后将自动读取数据判断");
                        Thread.Sleep(800);
                        while (dzsj >= 0)
                        {
                            Msg_label(label_msg, panel_msg, "通检查气中..." + dzsj.ToString("0" + "秒"));
                            Thread.Sleep(900);
                            dzsj--;
                        }
                        status = "4";
                        for (int i = 0; i < 50; i++)
                        {

                            setlabeltext(textEdit_Cojg, textEditCo.Text);
                            setlabeltext(textEdit_co2jg, textEditCo2.Text);
                            setlabeltext(textEdit_hcjg, textEditHc.Text);
                            setlabeltext(textEdit_nojg, textEditNo.Text);
                            setlabeltext(textEdit_pefjg, textEditPEF.Text);
                            setlabeltext(textEdit_o2jg, textEditO2.Text);
                            Thread.Sleep(100);
                        }
                        gksjcount = GKSJ;
                        isSaveStartTime = false;
                        switch (UseFqy)
                        {
                            case "fla_502":
                                if (fla_502 != null)
                                    fla_502.Close_testGas();
                                break;
                            case "fla_501":
                                break;
                        }
                        Msg_label(label_msg, panel_msg, "停止通检查气，检查完毕");
                        if (jiancha_detect())
                            Msg_label(label_jcjg, panel_jcjg, "通过");
                        else
                            Msg_label(label_jcjg, panel_jcjg, "未通过");

                    }
                    break;
                case "cdf5000":
                    if (fla_502 != null)
                    {
                        //fla_502.Set_standardGas(double.Parse(textEditBiaoC3h8.Text), double.Parse(textEditBiaoCo.Text), double.Parse(textEditBiaoCo2.Text), double.Parse(textEditBiaoO2.Text), float.Parse(textEditBiaoNo.Text));//不标定氧气
                        if (MessageBox.Show("确认标准气浓度填写的浓度值与标准气罐的标称值一致？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "检查被停止");
                            return;
                        }
                        Msg_label(label_msg, panel_msg, "请确认打开检查气气罐,压力调整到(0.05~0.1)MPa");
                        Thread.Sleep(500);
                        if (MessageBox.Show("确认打开检查气气罐？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "检查被停止");
                            return;
                        }
                        if (fla_502 != null)
                            fla_502.Open_testGas();
                        status = "3";
                        startTime = DateTime.Now;
                        isSaveStartTime = true;
                        Msg_label(label_msg, panel_msg, "开始通检查气，60秒后将自动读取数据判断");
                        Thread.Sleep(800);
                        while (dzsj >= 0)
                        {
                            Msg_label(label_msg, panel_msg, "通检查气中..." + dzsj.ToString("0" + "秒"));
                            Thread.Sleep(900);
                            dzsj--;
                        }
                        status = "4";
                        for (int i = 0; i < 50; i++)
                        {

                            setlabeltext(textEdit_Cojg, textEditCo.Text);
                            setlabeltext(textEdit_co2jg, textEditCo2.Text);
                            setlabeltext(textEdit_hcjg, textEditHc.Text);
                            setlabeltext(textEdit_nojg, textEditNo.Text);
                            setlabeltext(textEdit_pefjg, textEditPEF.Text);
                            setlabeltext(textEdit_o2jg, textEditO2.Text);
                            Thread.Sleep(100);
                        }
                        gksjcount = GKSJ;
                        isSaveStartTime = false;
                        switch (UseFqy)
                        {
                            case "fla_502":
                                if (fla_502 != null)
                                    fla_502.Close_testGas();
                                break;
                            case "cdf5000":
                                if (fla_502 != null)
                                    fla_502.Close_testGas();
                                break;
                            case "fla_501":
                                break;
                        }
                        Msg_label(label_msg, panel_msg, "停止通检查气，检查完毕");
                        if (jiancha_detect())
                            Msg_label(label_jcjg, panel_jcjg, "通过");
                        else
                            Msg_label(label_jcjg, panel_jcjg, "未通过");

                    }
                    break;
                case "nha_503":
                    if (fla_502 != null)
                    {
                        //fla_502.Set_standardGas(double.Parse(textEditBiaoC3h8.Text), double.Parse(textEditBiaoCo.Text), double.Parse(textEditBiaoCo2.Text), double.Parse(textEditBiaoO2.Text), float.Parse(textEditBiaoNo.Text));//不标定氧气
                        if (MessageBox.Show("确认标准气浓度填写的浓度值与标准气罐的标称值一致？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "检查被停止");
                            return;
                        }
                        Msg_label(label_msg, panel_msg, "请确认打开检查气气罐,压力调整到(0.05~0.1)MPa");
                        Thread.Sleep(500);
                        if (MessageBox.Show("确认打开检查气气罐？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "检查被停止");
                            return;
                        }
                        if (fla_502 != null)
                            fla_502.Open_testGas();
                        status = "3";
                        startTime = DateTime.Now;
                        isSaveStartTime = true;
                        Msg_label(label_msg, panel_msg, "开始通检查气，60秒后将自动读取数据判断");
                        Thread.Sleep(800);
                        while (dzsj >= 0)
                        {
                            Msg_label(label_msg, panel_msg, "通检查气中..." + dzsj.ToString("0" + "秒"));
                            Thread.Sleep(900);
                            dzsj--;
                        }
                        status = "4";
                        for (int i = 0; i < 50; i++)
                        {

                            setlabeltext(textEdit_Cojg, textEditCo.Text);
                            setlabeltext(textEdit_co2jg, textEditCo2.Text);
                            setlabeltext(textEdit_hcjg, textEditHc.Text);
                            setlabeltext(textEdit_nojg, textEditNo.Text);
                            setlabeltext(textEdit_pefjg, textEditPEF.Text);
                            setlabeltext(textEdit_o2jg, textEditO2.Text);
                            Thread.Sleep(100);
                        }
                        gksjcount = GKSJ;
                        isSaveStartTime = false;
                        if (fla_502 != null)
                            fla_502.Close_testGas();
                        Msg_label(label_msg, panel_msg, "停止通检查气，检查完毕");
                        if (jiancha_detect())
                            Msg_label(label_jcjg, panel_jcjg, "通过");
                        else
                            Msg_label(label_jcjg, panel_jcjg, "未通过");

                    }
                    break;
                case "fasm_5000":

                    if (fla_502 != null)
                    {
                        //fla_502.Set_standardGas(double.Parse(textEditBiaoC3h8.Text), double.Parse(textEditBiaoCo.Text), double.Parse(textEditBiaoCo2.Text), double.Parse(textEditBiaoO2.Text), float.Parse(textEditBiaoNo.Text));//不标定氧气
                        if (MessageBox.Show("确认标准气浓度填写的浓度值与标准气罐的标称值一致？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "检查被停止");
                            return;
                        }
                        Msg_label(label_msg, panel_msg, "请确认打开检查气气罐,压力调整到(0.05~0.1)MPa");
                        Thread.Sleep(500);
                        if (MessageBox.Show("确认打开检查气气罐？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "检查被停止");
                            return;
                        }
                        if (fla_502 != null)
                            fla_502.Open_testGas();
                        status = "3";
                        startTime = DateTime.Now;
                        isSaveStartTime = true;
                        dzsj = 60;
                        Msg_label(label_msg, panel_msg, "开始通检查气，60秒后将自动读取数据判断");
                        Thread.Sleep(800);
                        while (dzsj >= 0)
                        {
                            Msg_label(label_msg, panel_msg, "通检查气中..." + dzsj.ToString("0" + "秒"));
                            Thread.Sleep(900);
                            dzsj--;
                        }
                        status = "4";
                        for (int i = 0; i < 50; i++)
                        {

                            setlabeltext(textEdit_Cojg, textEditCo.Text);
                            setlabeltext(textEdit_co2jg, textEditCo2.Text);
                            setlabeltext(textEdit_hcjg, textEditHc.Text);
                            setlabeltext(textEdit_nojg, textEditNo.Text);
                            setlabeltext(textEdit_pefjg, textEditPEF.Text);
                            setlabeltext(textEdit_o2jg, textEditO2.Text);
                            Thread.Sleep(100);
                        }
                        gksjcount = GKSJ;
                        isSaveStartTime = false;

                        if (fla_502 != null)
                            fla_502.Close_testGas();
                        Msg_label(label_msg, panel_msg, "停止通检查气，检查完毕");
                        if (jiancha_detect())
                            Msg_label(label_jcjg, panel_jcjg, "通过");
                        else
                            Msg_label(label_jcjg, panel_jcjg, "未通过");

                    }
                    break;
                case "mqw_50a":
                case "mqw_50b":

                    if (fla_502 != null)
                    {
                        //fla_502.Set_standardGas(double.Parse(textEditBiaoC3h8.Text), double.Parse(textEditBiaoCo.Text), double.Parse(textEditBiaoCo2.Text), double.Parse(textEditBiaoO2.Text), float.Parse(textEditBiaoNo.Text));//不标定氧气
                        if (MessageBox.Show("确认标准气浓度填写的浓度值与标准气罐的标称值一致？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "检查被停止");
                            return;
                        }
                        Msg_label(label_msg, panel_msg, "请确认打开检查气气罐,压力调整到(0.05~0.1)MPa");
                        Thread.Sleep(500);
                        if (MessageBox.Show("确认打开检查气气罐？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "检查被停止");
                            return;
                        }
                        if (fla_502 != null)
                            fla_502.Open_testGas();
                        status = "3";
                        startTime = DateTime.Now;
                        isSaveStartTime = true;
                        dzsj = 60;
                        Msg_label(label_msg, panel_msg, "开始通检查气，60秒后将自动读取数据判断");
                        Thread.Sleep(800);
                        while (dzsj >= 0)
                        {
                            Msg_label(label_msg, panel_msg, "通检查气中..." + dzsj.ToString("0" + "秒"));
                            Thread.Sleep(900);
                            dzsj--;
                        }
                        status = "4";
                        for (int i = 0; i < 50; i++)
                        {

                            setlabeltext(textEdit_Cojg, textEditCo.Text);
                            setlabeltext(textEdit_co2jg, textEditCo2.Text);
                            setlabeltext(textEdit_hcjg, textEditHc.Text);
                            setlabeltext(textEdit_nojg, textEditNo.Text);
                            setlabeltext(textEdit_pefjg, textEditPEF.Text);
                            setlabeltext(textEdit_o2jg, textEditO2.Text);
                            Thread.Sleep(100);
                        }
                        gksjcount = GKSJ;
                        isSaveStartTime = false;

                        if (fla_502 != null)
                            fla_502.Close_testGas();
                        Msg_label(label_msg, panel_msg, "停止通检查气，检查完毕");
                        if (jiancha_detect())
                            Msg_label(label_jcjg, panel_jcjg, "通过");
                        else
                            Msg_label(label_jcjg, panel_jcjg, "未通过");

                    }
                    break;
                case "fla_501":
                    if (fla_501 != null)
                    {
                        if (MessageBox.Show("确认标准气浓度填写的浓度值与标准气罐的标称值一致？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "检查被停止");
                            return;
                        }
                        Msg_label(label_msg, panel_msg, "请确认打开检查气,压力调整到(0.05~0.1)MPa");
                        if (MessageBox.Show("确认打开检查气？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "检查被停止");
                            return;
                        }
                        Msg_label(label_msg, panel_msg, "开始通检查气，60秒后将自动读取数据判断");
                        Thread.Sleep(800);
                        while (dzsj >= 0)
                        {
                            Msg_label(label_msg, panel_msg, "通检查气中..." + dzsj.ToString("0" + "秒"));
                            Thread.Sleep(900);
                            dzsj--;
                        }
                        setlabeltext(textEdit_Cojg, textEditCo.Text);
                        setlabeltext(textEdit_co2jg, textEditCo2.Text);
                        setlabeltext(textEdit_hcjg, textEditHc.Text);
                        setlabeltext(textEdit_nojg, textEditNo.Text);
                        setlabeltext(textEdit_pefjg, textEditPEF.Text);
                        setlabeltext(textEdit_o2jg, textEditO2.Text);
                        Msg_label(label_msg, panel_msg, "停止通检查气，检查完毕");
                        if (jiancha_detect())
                            Msg_label(label_jcjg, panel_jcjg, "通过");
                        else
                            Msg_label(label_jcjg, panel_jcjg, "未通过");

                    }
                    //MessageBox.Show("该型号废气仪未提供该功能,请进行手动标定", "系统提示");
                    break;
                case "mqw_511":
                    if (fla_502 != null)
                    {
                        if (MessageBox.Show("确认标准气浓度填写的浓度值与标准气罐的标称值一致？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "检查被停止");
                            return;
                        }
                        Msg_label(label_msg, panel_msg, "请确认打开检查气,压力调整到(0.05~0.1)MPa");
                        if (MessageBox.Show("确认打开检查气？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            Msg_label(label_msg, panel_msg, "检查被停止");
                            return;
                        }
                        Msg_label(label_msg, panel_msg, "开始通检查气，60秒后将自动读取数据判断");
                        Thread.Sleep(800);
                        while (dzsj >= 0)
                        {
                            Msg_label(label_msg, panel_msg, "通检查气中..." + dzsj.ToString("0" + "秒"));
                            Thread.Sleep(900);
                            dzsj--;
                        }
                        setlabeltext(textEdit_Cojg, textEditCo.Text);
                        setlabeltext(textEdit_co2jg, textEditCo2.Text);
                        setlabeltext(textEdit_hcjg, textEditHc.Text);
                        setlabeltext(textEdit_nojg, textEditNo.Text);
                        setlabeltext(textEdit_pefjg, textEditPEF.Text);
                        setlabeltext(textEdit_o2jg, textEditO2.Text);
                        Msg_label(label_msg, panel_msg, "停止通检查气，检查完毕");
                        if (jiancha_detect())
                            Msg_label(label_jcjg, panel_jcjg, "通过");
                        else
                            Msg_label(label_jcjg, panel_jcjg, "未通过");

                    }
                    //MessageBox.Show("该型号废气仪未提供该功能,请进行手动标定", "系统提示");
                    break;
            }
            action = "准备状态";
        }
        private bool jiancha_detect()
        {
            string gdjz = "0";//0——高校准，1——低校准
            float cowc = 0f, co2wc = 0f, hcwc = 0f, nowc = 0f, o2wc = 0f;
            float coxdwc = 0f, co2xdwc = 0f, hcxdwc = 0f, noxdwc = 0f, o2xdwc = 0f;
            bool copd = true, co2pd = true, hcpd = true, nopd = true, o2pd = true;
            float o2bz = 0f;
            float o2clz = 0f;
            string wtgx = "";
            bool pdjg = false;
            switch (UseFqy)
            {
                case "fla_502":
                    if (radioButtonLowB.Checked == true)
                    {
                        analysisdata.Pef = textEdit_pefjg.Text;
                        analysisdata.c3h8 = textEditBiaoC3h8.Text;
                        analysisdata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                        analysisdata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());
                        cowc = Math.Abs(analysisdata.Cobz - analysisdata.Coclz);
                        coxdwc = cowc / analysisdata.Cobz;
                        if (cowc <= 0.02 || coxdwc <= 0.03)
                            copd = true;
                        else
                        {
                            copd = false;
                            wtgx += "CO.";
                        }
                        analysisdata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                        analysisdata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());
                        co2wc = Math.Abs(analysisdata.Co2bz - analysisdata.Co2clz);
                        co2xdwc = co2wc / analysisdata.Co2bz;
                        if (co2wc <= 0.3 || co2xdwc <= 0.03)
                            co2pd = true;
                        else
                        {
                            co2pd = false;
                            wtgx += "CO2.";
                        }
                        analysisdata.Nobz = (float)Math.Round(double.Parse(textEditBiaoNo.Text.Trim()), 0);
                        analysisdata.Noclz = (float)Math.Round(double.Parse(textEdit_nojg.Text.Trim()), 0);
                        nowc = Math.Abs(analysisdata.Nobz - analysisdata.Noclz);
                        noxdwc = nowc / analysisdata.Nobz;
                        if (nowc <= 25 || noxdwc <= 0.04)
                            nopd = true;
                        else
                        {
                            nopd = false;
                            wtgx += "NO.";
                        }
                        analysisdata.Hcbz = (float)Math.Round(double.Parse(textEditBiaoC3h8.Text.Trim()) * double.Parse(textEdit_pefjg.Text.Trim()), 0);
                        analysisdata.Hcclz = (float)Math.Round(double.Parse(textEdit_hcjg.Text.Trim()), 0);
                        hcwc = Math.Abs(analysisdata.Hcbz - analysisdata.Hcclz);
                        hcxdwc = hcwc / analysisdata.Hcbz;
                        if (hcwc <= 4 || hcxdwc <= 0.03)
                            hcpd = true;
                        else
                        {
                            hcpd = false;
                            wtgx += "HC.";
                        }
                        analysisdata.Jzds = 4;
                        analysisdata.Gdjz = "1";
                        analysisdata.Bzsm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());
                        analysisdata.Copc = (coxdwc * 100).ToString("0.0");
                        analysisdata.Co2pc = (co2xdwc * 100).ToString("0.0");
                        analysisdata.Hcpc = (hcxdwc * 100).ToString("0.0");
                        analysisdata.Nopc = (noxdwc * 100).ToString("0.0");
                        if (copd == true && co2pd == true && hcpd == true && nopd == true)
                            analysisdata.Bdjg = "合格";
                        else
                            analysisdata.Bdjg = "不合格";
                        //analysismeterini.writeanalysismeterIni(analysisdata);
                        datagridview_msg(dataGridView1, "标定项目", 0, "低标气检查");
                        datagridview_msg(dataGridView1, "CO2标值", 0, analysisdata.Co2bz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2实测值", 0, analysisdata.Co2clz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2误差", 0, co2wc.ToString("0.00")+"/"+(co2xdwc*100).ToString("0.0")+"%");
                        datagridview_msg(dataGridView1, "CO标值", 0, analysisdata.Cobz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO实测值", 0, analysisdata.Coclz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO误差", 0, cowc.ToString("0.000") + "/" + (coxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "NO标值", 0, analysisdata.Nobz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO实测值", 0, analysisdata.Noclz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO误差", 0, nowc.ToString("0") + "/" + (noxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "HC标值", 0, analysisdata.Hcbz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC实测值", 0, analysisdata.Hcclz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC误差", 0, hcwc.ToString("0") + "/" + (hcxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "O2标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2误差", 0, "非检查项");
                        if (analysisdata.Bdjg == "合格")
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "通过");
                            pdjg = true;
                        }
                        else
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "未通过");
                            pdjg = false;
                        }
                        datagridview_msg(dataGridView1, "备注说明", 0, wtgx);
                    }
                    else if (radioButtonHighB.Checked == true)
                    {
                        analysisdata.Pef = textEdit_pefjg.Text;
                        analysisdata.c3h8 = textEditBiaoC3h8.Text;
                        analysisdata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                        analysisdata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());
                        cowc = Math.Abs(analysisdata.Cobz - analysisdata.Coclz);
                        coxdwc = cowc / analysisdata.Cobz;
                        if (coxdwc <= 0.05)
                            copd = true;
                        else
                        {
                            copd = false;
                            wtgx += "CO.";
                        }
                        analysisdata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                        analysisdata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());
                        co2wc = Math.Abs(analysisdata.Co2bz - analysisdata.Co2clz);
                        co2xdwc = co2wc / analysisdata.Co2bz;
                        if (co2wc <= 0.3 || co2xdwc <= 0.05)
                            co2pd = true;
                        else
                        {
                            wtgx += "CO2.";
                            co2pd = false;
                        }
                        analysisdata.Nobz = (float)Math.Round(double.Parse(textEditBiaoNo.Text.Trim()), 0);
                        analysisdata.Noclz = (float)Math.Round(double.Parse(textEdit_nojg.Text.Trim()), 0);
                        nowc = Math.Abs(analysisdata.Nobz - analysisdata.Noclz);
                        noxdwc = nowc / analysisdata.Nobz;
                        if (noxdwc <= 0.08)
                            nopd = true;
                        else
                        {
                            wtgx += "NO.";
                            nopd = false;
                        }
                        analysisdata.Hcbz = (float)Math.Round(double.Parse(textEditBiaoC3h8.Text.Trim()) * double.Parse(textEdit_pefjg.Text.Trim()), 0);
                        analysisdata.Hcclz = (float)Math.Round(double.Parse(textEdit_hcjg.Text.Trim()), 0);
                        hcwc = Math.Abs(analysisdata.Hcbz - analysisdata.Hcclz);
                        hcxdwc = hcwc / analysisdata.Hcbz;
                        if (hcxdwc <= 0.05)
                            hcpd = true;
                        else
                        {

                            wtgx += "HC.";
                            hcpd = false;
                        }
                        analysisdata.Jzds = 4;
                        analysisdata.Gdjz = "0";
                        analysisdata.Bzsm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());

                        analysisdata.coabswc = (cowc).ToString("0.00");
                        analysisdata.co2abswc = (co2wc).ToString("0.00");
                        analysisdata.hcabswc = (hcwc).ToString("0");
                        analysisdata.noabswc = (nowc).ToString("0");
                        analysisdata.Copc = (coxdwc * 100).ToString("0.0");
                        analysisdata.Co2pc = (co2xdwc * 100).ToString("0.0");
                        analysisdata.Hcpc = (hcxdwc * 100).ToString("0.0");
                        analysisdata.Nopc = (noxdwc * 100).ToString("0.0");
                        if (copd == true && co2pd == true && hcpd == true && nopd == true)
                            analysisdata.Bdjg = "合格";
                        else
                            analysisdata.Bdjg = "不合格";
                        //analysismeterini.writeanalysismeterIni(analysisdata);
                        datagridview_msg(dataGridView1, "标定项目", 0, "高标气检查");
                        datagridview_msg(dataGridView1, "CO2标值", 0, analysisdata.Co2bz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2实测值", 0, analysisdata.Co2clz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2误差", 0,  (co2xdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "CO标值", 0, analysisdata.Cobz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO实测值", 0, analysisdata.Coclz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO误差", 0, (coxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "NO标值", 0, analysisdata.Nobz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO实测值", 0, analysisdata.Noclz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO误差", 0, (noxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "HC标值", 0, analysisdata.Hcbz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC实测值", 0, analysisdata.Hcclz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC误差", 0,  (hcxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "O2标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2误差", 0, "非检查项");
                        if (analysisdata.Bdjg == "合格")
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "通过");
                            pdjg = true;
                        }
                        else
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "未通过");
                            pdjg = false;
                        }
                        datagridview_msg(dataGridView1, "备注说明", 0, wtgx);
                        //analysismeterini.writeanalysismeterIni(analysisdata);
                    }
                    else
                    {
                        o2bz = float.Parse(textEditBiaoO2.Text.Trim());
                        o2clz = float.Parse(textEdit_o2jg.Text.Trim());
                        o2wc = Math.Abs(o2bz - o2clz);
                        o2xdwc = o2wc / o2bz;
                        datagridview_msg(dataGridView1, "标定项目", 0, "氧气检查");
                        datagridview_msg(dataGridView1, "CO2标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO2实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "NO标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "NO实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "HC标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "HC实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2标值", 0, o2bz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "O2实测值", 0, o2clz.ToString("0.00"));
                        if (o2wc < 0.1 || o2xdwc < 0.05)
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "通过");
                            pdjg = true;
                        }
                        else
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "未通过");
                            pdjg = false;
                        }
                        datagridview_msg(dataGridView1, "备注说明", 0, "");
                    }
                    break;
                case "cdf5000":
                    if (radioButtonLowB.Checked == true)
                    {
                        analysisdata.Pef = textEdit_pefjg.Text;
                        analysisdata.c3h8 = textEditBiaoC3h8.Text;
                        analysisdata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                        analysisdata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());
                        cowc = Math.Abs(analysisdata.Cobz - analysisdata.Coclz);
                        coxdwc = cowc / analysisdata.Cobz;
                        if (cowc <= 0.02 || coxdwc <= 0.03)
                            copd = true;
                        else
                        {
                            copd = false;
                            wtgx += "CO.";
                        }
                        analysisdata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                        analysisdata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());
                        co2wc = Math.Abs(analysisdata.Co2bz - analysisdata.Co2clz);
                        co2xdwc = co2wc / analysisdata.Co2bz;
                        if (co2wc <= 0.3 || co2xdwc <= 0.03)
                            co2pd = true;
                        else
                        {
                            co2pd = false;
                            wtgx += "CO2.";
                        }
                        analysisdata.Nobz = (float)Math.Round(double.Parse(textEditBiaoNo.Text.Trim()), 0);
                        analysisdata.Noclz = (float)Math.Round(double.Parse(textEdit_nojg.Text.Trim()), 0);
                        nowc = Math.Abs(analysisdata.Nobz - analysisdata.Noclz);
                        noxdwc = nowc / analysisdata.Nobz;
                        if (nowc <= 25 || noxdwc <= 0.04)
                            nopd = true;
                        else
                        {
                            nopd = false;
                            wtgx += "NO.";
                        }
                        analysisdata.Hcbz = (float)Math.Round(double.Parse(textEditBiaoC3h8.Text.Trim()) * double.Parse(textEdit_pefjg.Text.Trim()), 0);
                        analysisdata.Hcclz = (float)Math.Round(double.Parse(textEdit_hcjg.Text.Trim()), 0);
                        hcwc = Math.Abs(analysisdata.Hcbz - analysisdata.Hcclz);
                        hcxdwc = hcwc / analysisdata.Hcbz;
                        if (hcwc <= 4 || hcxdwc <= 0.03)
                            hcpd = true;
                        else
                        {
                            hcpd = false;
                            wtgx += "HC.";
                        }
                        analysisdata.Jzds = 4;
                        analysisdata.Gdjz = "1";
                        analysisdata.Bzsm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());
                        analysisdata.Copc = (coxdwc * 100).ToString("0.0");
                        analysisdata.Co2pc = (co2xdwc * 100).ToString("0.0");
                        analysisdata.Hcpc = (hcxdwc * 100).ToString("0.0");
                        analysisdata.Nopc = (noxdwc * 100).ToString("0.0");
                        if (copd == true && co2pd == true && hcpd == true && nopd == true)
                            analysisdata.Bdjg = "合格";
                        else
                            analysisdata.Bdjg = "不合格";
                        //analysismeterini.writeanalysismeterIni(analysisdata);
                        datagridview_msg(dataGridView1, "标定项目", 0, "低标气检查");
                        datagridview_msg(dataGridView1, "CO2标值", 0, analysisdata.Co2bz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2实测值", 0, analysisdata.Co2clz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2误差", 0, co2wc.ToString("0.00") + "/" + (co2xdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "CO标值", 0, analysisdata.Cobz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO实测值", 0, analysisdata.Coclz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO误差", 0, cowc.ToString("0.000") + "/" + (coxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "NO标值", 0, analysisdata.Nobz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO实测值", 0, analysisdata.Noclz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO误差", 0, nowc.ToString("0") + "/" + (noxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "HC标值", 0, analysisdata.Hcbz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC实测值", 0, analysisdata.Hcclz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC误差", 0, hcwc.ToString("0") + "/" + (hcxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "O2标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2误差", 0, "非检查项");
                        if (analysisdata.Bdjg == "合格")
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "通过");
                            pdjg = true;
                        }
                        else
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "未通过");
                            pdjg = false;
                        }
                        datagridview_msg(dataGridView1, "备注说明", 0, wtgx);
                    }
                    else if (radioButtonHighB.Checked == true)
                    {
                        analysisdata.Pef = textEdit_pefjg.Text;
                        analysisdata.c3h8 = textEditBiaoC3h8.Text;
                        analysisdata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                        analysisdata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());
                        cowc = Math.Abs(analysisdata.Cobz - analysisdata.Coclz);
                        coxdwc = cowc / analysisdata.Cobz;
                        if (coxdwc <= 0.05)
                            copd = true;
                        else
                        {
                            copd = false;
                            wtgx += "CO.";
                        }
                        analysisdata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                        analysisdata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());
                        co2wc = Math.Abs(analysisdata.Co2bz - analysisdata.Co2clz);
                        co2xdwc = co2wc / analysisdata.Co2bz;
                        if (co2wc <= 0.3 || co2xdwc <= 0.05)
                            co2pd = true;
                        else
                        {
                            wtgx += "CO2.";
                            co2pd = false;
                        }
                        analysisdata.Nobz = (float)Math.Round(double.Parse(textEditBiaoNo.Text.Trim()), 0);
                        analysisdata.Noclz = (float)Math.Round(double.Parse(textEdit_nojg.Text.Trim()), 0);
                        nowc = Math.Abs(analysisdata.Nobz - analysisdata.Noclz);
                        noxdwc = nowc / analysisdata.Nobz;
                        if (noxdwc <= 0.08)
                            nopd = true;
                        else
                        {
                            wtgx += "NO.";
                            nopd = false;
                        }
                        analysisdata.Hcbz = (float)Math.Round(double.Parse(textEditBiaoC3h8.Text.Trim()) * double.Parse(textEdit_pefjg.Text.Trim()), 0);
                        analysisdata.Hcclz = (float)Math.Round(double.Parse(textEdit_hcjg.Text.Trim()), 0);
                        hcwc = Math.Abs(analysisdata.Hcbz - analysisdata.Hcclz);
                        hcxdwc = hcwc / analysisdata.Hcbz;
                        if (hcxdwc <= 0.05)
                            hcpd = true;
                        else
                        {

                            wtgx += "HC.";
                            hcpd = false;
                        }
                        analysisdata.Jzds = 4;
                        analysisdata.Gdjz = "0";
                        analysisdata.Bzsm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());

                        analysisdata.coabswc = (cowc).ToString("0.00");
                        analysisdata.co2abswc = (co2wc).ToString("0.00");
                        analysisdata.hcabswc = (hcwc).ToString("0");
                        analysisdata.noabswc = (nowc).ToString("0");
                        analysisdata.Copc = (coxdwc * 100).ToString("0.0");
                        analysisdata.Co2pc = (co2xdwc * 100).ToString("0.0");
                        analysisdata.Hcpc = (hcxdwc * 100).ToString("0.0");
                        analysisdata.Nopc = (noxdwc * 100).ToString("0.0");
                        if (copd == true && co2pd == true && hcpd == true && nopd == true)
                            analysisdata.Bdjg = "合格";
                        else
                            analysisdata.Bdjg = "不合格";
                        //analysismeterini.writeanalysismeterIni(analysisdata);
                        datagridview_msg(dataGridView1, "标定项目", 0, "高标气检查");
                        datagridview_msg(dataGridView1, "CO2标值", 0, analysisdata.Co2bz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2实测值", 0, analysisdata.Co2clz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2误差", 0, (co2xdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "CO标值", 0, analysisdata.Cobz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO实测值", 0, analysisdata.Coclz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO误差", 0, (coxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "NO标值", 0, analysisdata.Nobz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO实测值", 0, analysisdata.Noclz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO误差", 0, (noxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "HC标值", 0, analysisdata.Hcbz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC实测值", 0, analysisdata.Hcclz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC误差", 0, (hcxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "O2标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2误差", 0, "非检查项");
                        if (analysisdata.Bdjg == "合格")
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "通过");
                            pdjg = true;
                        }
                        else
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "未通过");
                            pdjg = false;
                        }
                        datagridview_msg(dataGridView1, "备注说明", 0, wtgx);
                        //analysismeterini.writeanalysismeterIni(analysisdata);
                    }
                    else
                    {
                        o2bz = float.Parse(textEditBiaoO2.Text.Trim());
                        o2clz = float.Parse(textEdit_o2jg.Text.Trim());
                        o2wc = Math.Abs(o2bz - o2clz);
                        o2xdwc = o2wc / o2bz;
                        datagridview_msg(dataGridView1, "标定项目", 0, "氧气检查");
                        datagridview_msg(dataGridView1, "CO2标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO2实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "NO标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "NO实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "HC标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "HC实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2标值", 0, o2bz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "O2实测值", 0, o2clz.ToString("0.00"));
                        if (o2wc < 0.1 || o2xdwc < 0.05)
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "通过");
                            pdjg = true;
                        }
                        else
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "未通过");
                            pdjg = false;
                        }
                        datagridview_msg(dataGridView1, "备注说明", 0, "");
                    }
                    break;
                case "nha_503":
                    if (radioButtonLowB.Checked == true)
                    {
                        analysisdata.Pef = textEdit_pefjg.Text;
                        analysisdata.c3h8 = textEditBiaoC3h8.Text;
                        analysisdata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                        analysisdata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());
                        cowc = Math.Abs(analysisdata.Cobz - analysisdata.Coclz);
                        coxdwc = cowc / analysisdata.Cobz;
                        if (cowc <= 0.02 || coxdwc <= 0.03)
                            copd = true;
                        else
                        {
                            copd = false;
                            wtgx += "CO.";
                        }
                        analysisdata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                        analysisdata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());
                        co2wc = Math.Abs(analysisdata.Co2bz - analysisdata.Co2clz);
                        co2xdwc = co2wc / analysisdata.Co2bz;
                        if (co2wc <= 0.3 || co2xdwc <= 0.03)
                            co2pd = true;
                        else
                        {
                            co2pd = false;
                            wtgx += "CO2.";
                        }
                        analysisdata.Nobz = (float)Math.Round(double.Parse(textEditBiaoNo.Text.Trim()), 0);
                        analysisdata.Noclz = (float)Math.Round(double.Parse(textEdit_nojg.Text.Trim()), 0);
                        nowc = Math.Abs(analysisdata.Nobz - analysisdata.Noclz);
                        noxdwc = nowc / analysisdata.Nobz;
                        if (nowc <= 25 || noxdwc <= 0.04)
                            nopd = true;
                        else
                        {
                            nopd = false;
                            wtgx += "NO.";
                        }
                        analysisdata.Hcbz = (float)Math.Round(double.Parse(textEditBiaoC3h8.Text.Trim()) * double.Parse(textEdit_pefjg.Text.Trim()), 0);
                        analysisdata.Hcclz = (float)Math.Round(double.Parse(textEdit_hcjg.Text.Trim()), 0);
                        hcwc = Math.Abs(analysisdata.Hcbz - analysisdata.Hcclz);
                        hcxdwc = hcwc / analysisdata.Hcbz;
                        if (hcwc <= 4 || hcxdwc <= 0.03)
                            hcpd = true;
                        else
                        {
                            hcpd = false;
                            wtgx += "HC.";
                        }
                        analysisdata.Jzds = 4;
                        analysisdata.Gdjz = "1";
                        analysisdata.Bzsm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());

                        analysisdata.coabswc = (cowc).ToString("0.00");
                        analysisdata.co2abswc = (co2wc).ToString("0.00");
                        analysisdata.hcabswc = (hcwc).ToString("0");
                        analysisdata.noabswc = (nowc).ToString("0");
                        analysisdata.Copc = (coxdwc * 100).ToString("0.0");
                        analysisdata.Co2pc = (co2xdwc * 100).ToString("0.0");
                        analysisdata.Hcpc = (hcxdwc * 100).ToString("0.0");
                        analysisdata.Nopc = (noxdwc * 100).ToString("0.0");
                        if (copd == true && co2pd == true && hcpd == true && nopd == true)
                            analysisdata.Bdjg = "合格";
                        else
                            analysisdata.Bdjg = "不合格";
                        //analysismeterini.writeanalysismeterIni(analysisdata);
                        datagridview_msg(dataGridView1, "标定项目", 0, "低标气检查");
                        datagridview_msg(dataGridView1, "CO2标值", 0, analysisdata.Co2bz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2实测值", 0, analysisdata.Co2clz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2误差", 0, co2wc.ToString("0.00") + "/" + (co2xdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "CO标值", 0, analysisdata.Cobz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO实测值", 0, analysisdata.Coclz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO误差", 0, cowc.ToString("0.000") + "/" + (coxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "NO标值", 0, analysisdata.Nobz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO实测值", 0, analysisdata.Noclz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO误差", 0, nowc.ToString("0") + "/" + (noxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "HC标值", 0, analysisdata.Hcbz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC实测值", 0, analysisdata.Hcclz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC误差", 0, hcwc.ToString("0") + "/" + (hcxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "O2标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2误差", 0, "非检查项");
                        if (analysisdata.Bdjg == "合格")
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "通过");
                            pdjg = true;
                        }
                        else
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "未通过");
                            pdjg = false;
                        }
                        datagridview_msg(dataGridView1, "备注说明", 0, wtgx);
                    }
                    else if (radioButtonHighB.Checked == true)
                    {
                        analysisdata.Pef = textEdit_pefjg.Text;
                        analysisdata.c3h8 = textEditBiaoC3h8.Text;
                        analysisdata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                        analysisdata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());
                        cowc = Math.Abs(analysisdata.Cobz - analysisdata.Coclz);
                        coxdwc = cowc / analysisdata.Cobz;
                        if (coxdwc <= 0.05)
                            copd = true;
                        else
                        {
                            copd = false;
                            wtgx += "CO.";
                        }
                        analysisdata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                        analysisdata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());
                        co2wc = Math.Abs(analysisdata.Co2bz - analysisdata.Co2clz);
                        co2xdwc = co2wc / analysisdata.Co2bz;
                        if (co2wc <= 0.3 || co2xdwc <= 0.05)
                            co2pd = true;
                        else
                        {
                            wtgx += "CO2.";
                            co2pd = false;
                        }
                        analysisdata.Nobz = (float)Math.Round(double.Parse(textEditBiaoNo.Text.Trim()), 0);
                        analysisdata.Noclz = (float)Math.Round(double.Parse(textEdit_nojg.Text.Trim()), 0);
                        nowc = Math.Abs(analysisdata.Nobz - analysisdata.Noclz);
                        noxdwc = nowc / analysisdata.Nobz;
                        if (noxdwc <= 0.08)
                            nopd = true;
                        else
                        {
                            wtgx += "NO.";
                            nopd = false;
                        }
                        analysisdata.Hcbz = (float)Math.Round(double.Parse(textEditBiaoC3h8.Text.Trim()) * double.Parse(textEdit_pefjg.Text.Trim()), 0);
                        analysisdata.Hcclz = (float)Math.Round(double.Parse(textEdit_hcjg.Text.Trim()), 0);
                        hcwc = Math.Abs(analysisdata.Hcbz - analysisdata.Hcclz);
                        hcxdwc = hcwc / analysisdata.Hcbz;
                        if (hcxdwc <= 0.05)
                            hcpd = true;
                        else
                        {

                            wtgx += "HC.";
                            hcpd = false;
                        }
                        analysisdata.Jzds = 4;
                        analysisdata.Gdjz = "0";
                        analysisdata.Bzsm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());
                        analysisdata.coabswc = (cowc).ToString("0.00");
                        analysisdata.co2abswc = (co2wc).ToString("0.00");
                        analysisdata.hcabswc = (hcwc).ToString("0");
                        analysisdata.noabswc = (nowc).ToString("0");
                        analysisdata.Copc = (coxdwc * 100).ToString("0.0");
                        analysisdata.Co2pc = (co2xdwc * 100).ToString("0.0");
                        analysisdata.Hcpc = (hcxdwc * 100).ToString("0.0");
                        analysisdata.Nopc = (noxdwc * 100).ToString("0.0");
                        if (copd == true && co2pd == true && hcpd == true && nopd == true)
                            analysisdata.Bdjg = "合格";
                        else
                            analysisdata.Bdjg = "不合格";
                        //analysismeterini.writeanalysismeterIni(analysisdata);
                        datagridview_msg(dataGridView1, "标定项目", 0, "高标气检查");
                        datagridview_msg(dataGridView1, "CO2标值", 0, analysisdata.Co2bz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2实测值", 0, analysisdata.Co2clz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2误差", 0, (co2xdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "CO标值", 0, analysisdata.Cobz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO实测值", 0, analysisdata.Coclz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO误差", 0, (coxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "NO标值", 0, analysisdata.Nobz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO实测值", 0, analysisdata.Noclz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO误差", 0, (noxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "HC标值", 0, analysisdata.Hcbz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC实测值", 0, analysisdata.Hcclz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC误差", 0, (hcxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "O2标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2误差", 0, "非检查项");
                        if (analysisdata.Bdjg == "合格")
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "通过");
                            pdjg = true;
                        }
                        else
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "未通过");
                            pdjg = false;
                        }
                        datagridview_msg(dataGridView1, "备注说明", 0, wtgx);
                        //analysismeterini.writeanalysismeterIni(analysisdata);
                    }
                    else
                    {
                        o2bz = float.Parse(textEditBiaoO2.Text.Trim());
                        o2clz = float.Parse(textEdit_o2jg.Text.Trim());
                        o2wc = Math.Abs(o2bz - o2clz);
                        o2xdwc = o2wc / o2bz;
                        datagridview_msg(dataGridView1, "标定项目", 0, "氧气检查");
                        datagridview_msg(dataGridView1, "CO2标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO2实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "NO标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "NO实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "HC标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "HC实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2标值", 0, o2bz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "O2实测值", 0, o2clz.ToString("0.00"));
                        if (o2wc < 0.1 || o2xdwc < 0.05)
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "通过");
                            pdjg = true;
                        }
                        else
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "未通过");
                            pdjg = false;
                        }
                        datagridview_msg(dataGridView1, "备注说明", 0, "");
                    }
                    break;
                case "fasm_5000":
                    if (radioButtonLowB.Checked == true)
                    {
                        analysisdata.Pef = textEdit_pefjg.Text;
                        analysisdata.c3h8 = textEditBiaoC3h8.Text;
                        analysisdata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                        analysisdata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());
                        cowc = Math.Abs(analysisdata.Cobz - analysisdata.Coclz);
                        coxdwc = cowc / analysisdata.Cobz;
                        if (cowc <= 0.02 || coxdwc <= 0.03)
                            copd = true;
                        else
                        {
                            copd = false;
                            wtgx += "CO.";
                        }
                        analysisdata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                        analysisdata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());
                        co2wc = Math.Abs(analysisdata.Co2bz - analysisdata.Co2clz);
                        co2xdwc = co2wc / analysisdata.Co2bz;
                        if (co2wc <= 0.3 || co2xdwc <= 0.03)
                            co2pd = true;
                        else
                        {
                            co2pd = false;
                            wtgx += "CO2.";
                        }
                        analysisdata.Nobz = (float)Math.Round(double.Parse(textEditBiaoNo.Text.Trim()), 0);
                        analysisdata.Noclz = (float)Math.Round(double.Parse(textEdit_nojg.Text.Trim()), 0);
                        nowc = Math.Abs(analysisdata.Nobz - analysisdata.Noclz);
                        noxdwc = nowc / analysisdata.Nobz;
                        if (nowc <= 25 || noxdwc <= 0.04)
                            nopd = true;
                        else
                        {
                            nopd = false;
                            wtgx += "NO.";
                        }
                        analysisdata.Hcbz = (float)Math.Round(double.Parse(textEditBiaoC3h8.Text.Trim()) * double.Parse(textEdit_pefjg.Text.Trim()), 0);
                        analysisdata.Hcclz = (float)Math.Round(double.Parse(textEdit_hcjg.Text.Trim()), 0);
                        hcwc = Math.Abs(analysisdata.Hcbz - analysisdata.Hcclz);
                        hcxdwc = hcwc / analysisdata.Hcbz;
                        if (hcwc <= 4 || hcxdwc <= 0.03)
                            hcpd = true;
                        else
                        {
                            hcpd = false;
                            wtgx += "HC.";
                        }
                        analysisdata.Jzds = 4;
                        analysisdata.Gdjz = "1";
                        analysisdata.Bzsm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());
                        analysisdata.coabswc = (cowc).ToString("0.00");
                        analysisdata.co2abswc = (co2wc).ToString("0.00");
                        analysisdata.hcabswc = (hcwc).ToString("0");
                        analysisdata.noabswc = (nowc).ToString("0");
                        analysisdata.Copc = (coxdwc * 100).ToString("0.0");
                        analysisdata.Co2pc = (co2xdwc * 100).ToString("0.0");
                        analysisdata.Hcpc = (hcxdwc * 100).ToString("0.0");
                        analysisdata.Nopc = (noxdwc * 100).ToString("0.0");
                        if (copd == true && co2pd == true && hcpd == true && nopd == true)
                            analysisdata.Bdjg = "合格";
                        else
                            analysisdata.Bdjg = "不合格";
                        //analysismeterini.writeanalysismeterIni(analysisdata);
                        datagridview_msg(dataGridView1, "标定项目", 0, "低标气检查");
                        datagridview_msg(dataGridView1, "CO2标值", 0, analysisdata.Co2bz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2实测值", 0, analysisdata.Co2clz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2误差", 0, co2wc.ToString("0.00") + "/" + (co2xdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "CO标值", 0, analysisdata.Cobz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO实测值", 0, analysisdata.Coclz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO误差", 0, cowc.ToString("0.000") + "/" + (coxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "NO标值", 0, analysisdata.Nobz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO实测值", 0, analysisdata.Noclz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO误差", 0, nowc.ToString("0") + "/" + (noxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "HC标值", 0, analysisdata.Hcbz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC实测值", 0, analysisdata.Hcclz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC误差", 0, hcwc.ToString("0") + "/" + (hcxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "O2标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2误差", 0, "非检查项");
                        if (analysisdata.Bdjg == "合格")
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "通过");
                            pdjg = true;
                        }
                        else
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "未通过");
                            pdjg = false;
                        }
                        datagridview_msg(dataGridView1, "备注说明", 0, wtgx);
                    }
                    else if (radioButtonHighB.Checked == true)
                    {
                        analysisdata.Pef = textEdit_pefjg.Text;
                        analysisdata.c3h8 = textEditBiaoC3h8.Text;
                        analysisdata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                        analysisdata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());
                        cowc = Math.Abs(analysisdata.Cobz - analysisdata.Coclz);
                        coxdwc = cowc / analysisdata.Cobz;
                        if (coxdwc <= 0.05)
                            copd = true;
                        else
                        {

                            wtgx += "CO.";
                            copd = false;
                        }
                        analysisdata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                        analysisdata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());
                        co2wc = Math.Abs(analysisdata.Co2bz - analysisdata.Co2clz);
                        co2xdwc = co2wc / analysisdata.Co2bz;
                        if (co2wc <= 0.3 || co2xdwc <= 0.05)
                            co2pd = true;
                        else
                        {
                            wtgx += "CO2.";
                            co2pd = false;
                        }
                        analysisdata.Nobz = (float)Math.Round(double.Parse(textEditBiaoNo.Text.Trim()), 0);
                        analysisdata.Noclz = (float)Math.Round(double.Parse(textEdit_nojg.Text.Trim()), 0);
                        nowc = Math.Abs(analysisdata.Nobz - analysisdata.Noclz);
                        noxdwc = nowc / analysisdata.Nobz;
                        if (noxdwc <= 0.08)
                            nopd = true;
                        else
                        {
                            wtgx += "NOX.";
                            nopd = false;
                        }
                        analysisdata.Hcbz = (float)Math.Round(double.Parse(textEditBiaoC3h8.Text.Trim()) * double.Parse(textEdit_pefjg.Text.Trim()), 0);
                        analysisdata.Hcclz = (float)Math.Round(double.Parse(textEdit_hcjg.Text.Trim()), 0);
                        hcwc = Math.Abs(analysisdata.Hcbz - analysisdata.Hcclz);
                        hcxdwc = hcwc / analysisdata.Hcbz;
                        if (hcxdwc <= 0.05)
                            hcpd = true;
                        else
                        {

                            wtgx += "HC.";
                            hcpd = false;
                        }
                        analysisdata.Jzds = 4;
                        analysisdata.Gdjz = "0";
                        analysisdata.Bzsm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());
                        analysisdata.coabswc = (cowc).ToString("0.00");
                        analysisdata.co2abswc = (co2wc).ToString("0.00");
                        analysisdata.hcabswc = (hcwc).ToString("0");
                        analysisdata.noabswc = (nowc).ToString("0");
                        analysisdata.Copc = (coxdwc * 100).ToString("0.0");
                        analysisdata.Co2pc = (co2xdwc * 100).ToString("0.0");
                        analysisdata.Hcpc = (hcxdwc * 100).ToString("0.0");
                        analysisdata.Nopc = (noxdwc * 100).ToString("0.0");
                        if (copd == true && co2pd == true && hcpd == true && nopd == true)
                            analysisdata.Bdjg = "合格";
                        else
                            analysisdata.Bdjg = "不合格";
                        //analysismeterini.writeanalysismeterIni(analysisdata);
                        datagridview_msg(dataGridView1, "标定项目", 0, "高标气检查");
                        datagridview_msg(dataGridView1, "CO2标值", 0, analysisdata.Co2bz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2实测值", 0, analysisdata.Co2clz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2误差", 0, (co2xdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "CO标值", 0, analysisdata.Cobz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO实测值", 0, analysisdata.Coclz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO误差", 0, (coxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "NO标值", 0, analysisdata.Nobz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO实测值", 0, analysisdata.Noclz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO误差", 0, (noxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "HC标值", 0, analysisdata.Hcbz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC实测值", 0, analysisdata.Hcclz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC误差", 0, (hcxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "O2标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2误差", 0, "非检查项");
                        if (analysisdata.Bdjg == "合格")
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "通过");
                            pdjg = true;
                        }
                        else
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "未通过");
                            pdjg = false;
                        }
                        datagridview_msg(dataGridView1, "备注说明", 0, wtgx);
                        //analysismeterini.writeanalysismeterIni(analysisdata);
                    }
                    else
                    {
                        o2bz = float.Parse(textEditBiaoO2.Text.Trim());
                        o2clz = float.Parse(textEdit_o2jg.Text.Trim());
                        o2wc = Math.Abs(o2bz - o2clz);
                        o2xdwc = o2wc / o2bz;
                        datagridview_msg(dataGridView1, "标定项目", 0, "氧气检查");
                        datagridview_msg(dataGridView1, "CO2标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO2实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "NO标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "NO实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "HC标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "HC实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2标值", 0, o2bz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "O2实测值", 0, o2clz.ToString("0.00"));
                        if (o2wc < 0.1 || o2xdwc < 0.05)
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "通过");
                            pdjg = true;
                        }
                        else
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "未通过");
                            pdjg = false;
                        }
                        datagridview_msg(dataGridView1, "备注说明", 0, "");
                    }
                    break;
                case "mqw_50a":
                case "mqw_50b":
                    if (radioButtonLowB.Checked == true)
                    {
                        analysisdata.Pef = textEdit_pefjg.Text;
                        analysisdata.c3h8 = textEditBiaoC3h8.Text;
                        analysisdata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                        analysisdata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());
                        cowc = Math.Abs(analysisdata.Cobz - analysisdata.Coclz);
                        coxdwc = cowc / analysisdata.Cobz;
                        if (cowc <= 0.02 || coxdwc <= 0.03)
                            copd = true;
                        else
                        {
                            copd = false;
                            wtgx += "CO.";
                        }
                        analysisdata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                        analysisdata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());
                        co2wc = Math.Abs(analysisdata.Co2bz - analysisdata.Co2clz);
                        co2xdwc = co2wc / analysisdata.Co2bz;
                        if (co2wc <= 0.3 || co2xdwc <= 0.03)
                            co2pd = true;
                        else
                        {
                            co2pd = false;
                            wtgx += "CO2.";
                        }
                        analysisdata.Nobz = (float)Math.Round(double.Parse(textEditBiaoNo.Text.Trim()),0);
                        analysisdata.Noclz = (float)Math.Round(double.Parse(textEdit_nojg.Text.Trim()),0);
                        nowc = Math.Abs(analysisdata.Nobz - analysisdata.Noclz);
                        noxdwc = nowc / analysisdata.Nobz;
                        if (nowc <= 25 || noxdwc <= 0.04)
                            nopd = true;
                        else
                        {
                            nopd = false;
                            wtgx += "NO.";
                        }
                        analysisdata.Hcbz = (float)Math.Round(double.Parse(textEditBiaoC3h8.Text.Trim()) * double.Parse(textEdit_pefjg.Text.Trim()),0);
                        analysisdata.Hcclz = (float)Math.Round(double.Parse(textEdit_hcjg.Text.Trim()),0);
                        hcwc = Math.Abs(analysisdata.Hcbz - analysisdata.Hcclz);
                        hcxdwc = hcwc / analysisdata.Hcbz;
                        if (hcwc <= 4 || hcxdwc <= 0.03)
                            hcpd = true;
                        else
                        {
                            hcpd = false;
                            wtgx += "HC.";
                        }
                        analysisdata.Jzds = 4;
                        analysisdata.Gdjz = "1";
                        analysisdata.Bzsm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());
                        analysisdata.coabswc = (cowc).ToString("0.00");
                        analysisdata.co2abswc = (co2wc).ToString("0.00");
                        analysisdata.hcabswc = (hcwc).ToString("0");
                        analysisdata.noabswc = (nowc).ToString("0");
                        analysisdata.Copc = (coxdwc * 100).ToString("0.0");
                        analysisdata.Co2pc = (co2xdwc * 100).ToString("0.0");
                        analysisdata.Hcpc = (hcxdwc * 100).ToString("0.0");
                        analysisdata.Nopc = (noxdwc * 100).ToString("0.0");
                        if (copd == true && co2pd == true && hcpd == true && nopd == true)
                            analysisdata.Bdjg = "合格";
                        else
                            analysisdata.Bdjg = "不合格";
                        //analysismeterini.writeanalysismeterIni(analysisdata);
                        datagridview_msg(dataGridView1, "标定项目", 0, "低标气检查");
                        datagridview_msg(dataGridView1, "CO2标值", 0, analysisdata.Co2bz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2实测值", 0, analysisdata.Co2clz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2误差", 0, co2wc.ToString("0.00") + "/" + (co2xdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "CO标值", 0, analysisdata.Cobz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO实测值", 0, analysisdata.Coclz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO误差", 0, cowc.ToString("0.000") + "/" + (coxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "NO标值", 0, analysisdata.Nobz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO实测值", 0, analysisdata.Noclz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO误差", 0, nowc.ToString("0") + "/" + (noxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "HC标值", 0, analysisdata.Hcbz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC实测值", 0, analysisdata.Hcclz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC误差", 0, hcwc.ToString("0") + "/" + (hcxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "O2标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2误差", 0, "非检查项");
                        if (analysisdata.Bdjg == "合格")
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "通过");
                            pdjg = true;
                        }
                        else
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "未通过");
                            pdjg = false;
                        }
                        datagridview_msg(dataGridView1, "备注说明", 0, wtgx);
                    }
                    else if (radioButtonHighB.Checked == true)
                    {
                        analysisdata.Pef = textEdit_pefjg.Text;
                        analysisdata.c3h8 = textEditBiaoC3h8.Text;
                        analysisdata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                        analysisdata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());
                        cowc = Math.Abs(analysisdata.Cobz - analysisdata.Coclz);
                        coxdwc = cowc / analysisdata.Cobz;
                        if (coxdwc <= 0.05)
                            copd = true;
                        else
                        {

                            wtgx += "CO.";
                            copd = false;
                        }
                        analysisdata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                        analysisdata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());
                        co2wc = Math.Abs(analysisdata.Co2bz - analysisdata.Co2clz);
                        co2xdwc = co2wc / analysisdata.Co2bz;
                        if (co2wc <= 0.3 || co2xdwc <= 0.05)
                            co2pd = true;
                        else
                        {
                            wtgx += "CO2.";
                            co2pd = false;
                        }
                        analysisdata.Nobz = (float)Math.Round(double.Parse(textEditBiaoNo.Text.Trim()), 0);
                        analysisdata.Noclz = (float)Math.Round(double.Parse(textEdit_nojg.Text.Trim()), 0);
                        nowc = Math.Abs(analysisdata.Nobz - analysisdata.Noclz);
                        noxdwc = nowc / analysisdata.Nobz;
                        if (noxdwc <= 0.08)
                            nopd = true;
                        else
                        {
                            wtgx += "NOX.";
                            nopd = false;
                        }
                        analysisdata.Hcbz = (float)Math.Round(double.Parse(textEditBiaoC3h8.Text.Trim()) * double.Parse(textEdit_pefjg.Text.Trim()), 0);
                        analysisdata.Hcclz = (float)Math.Round(double.Parse(textEdit_hcjg.Text.Trim()), 0);
                        hcwc = Math.Abs(analysisdata.Hcbz - analysisdata.Hcclz);
                        hcxdwc = hcwc / analysisdata.Hcbz;
                        if (hcxdwc <= 0.05)
                            hcpd = true;
                        else
                        {

                            wtgx += "HC.";
                            hcpd = false;
                        }
                        analysisdata.Jzds = 4;
                        analysisdata.Gdjz = "0";
                        analysisdata.Bzsm = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());
                        analysisdata.coabswc = (cowc).ToString("0.00");
                        analysisdata.co2abswc = (co2wc).ToString("0.00");
                        analysisdata.hcabswc = (hcwc).ToString("0");
                        analysisdata.noabswc = (nowc).ToString("0");
                        analysisdata.Copc = (coxdwc * 100).ToString("0.0");
                        analysisdata.Co2pc = (co2xdwc * 100).ToString("0.0");
                        analysisdata.Hcpc = (hcxdwc * 100).ToString("0.0");
                        analysisdata.Nopc = (noxdwc * 100).ToString("0.0");
                        if (copd == true && co2pd == true && hcpd == true && nopd == true)
                            analysisdata.Bdjg = "合格";
                        else
                            analysisdata.Bdjg = "不合格";
                        datagridview_msg(dataGridView1, "标定项目", 0, "高标气检查");
                        datagridview_msg(dataGridView1, "CO2标值", 0, analysisdata.Co2bz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2实测值", 0, analysisdata.Co2clz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2误差", 0, (co2xdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "CO标值", 0, analysisdata.Cobz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO实测值", 0, analysisdata.Coclz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO误差", 0, (coxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "NO标值", 0, analysisdata.Nobz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO实测值", 0, analysisdata.Noclz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO误差", 0, (noxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "HC标值", 0, analysisdata.Hcbz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC实测值", 0, analysisdata.Hcclz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC误差", 0, (hcxdwc * 100).ToString("0.0") + "%");
                        datagridview_msg(dataGridView1, "O2标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2误差", 0, "非检查项");
                        if (analysisdata.Bdjg == "合格")
                        {
                            pdjg = true;
                            datagridview_msg(dataGridView1, "判定结果", 0, "通过");
                        }
                        else
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "未通过");
                            pdjg = false;
                        }
                        datagridview_msg(dataGridView1, "备注说明", 0, wtgx);
                        //analysismeterini.writeanalysismeterIni(analysisdata);
                    }
                    else
                    {
                        o2bz = float.Parse(textEditBiaoO2.Text.Trim());
                        o2clz = float.Parse(textEdit_o2jg.Text.Trim());
                        o2wc = Math.Abs(o2bz - o2clz);
                        o2xdwc = o2wc / o2bz;
                        datagridview_msg(dataGridView1, "标定项目", 0, "氧气检查");
                        datagridview_msg(dataGridView1, "CO2标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO2实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "NO标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "NO实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "HC标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "HC实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2标值", 0, o2bz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "O2实测值", 0, o2clz.ToString("0.00"));
                        if (o2wc < 0.1 || o2xdwc < 0.05)
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "通过");
                            pdjg = true;
                        }
                        else
                        {
                            datagridview_msg(dataGridView1, "判定结果", 0, "未通过");
                            pdjg = false;
                        }
                        datagridview_msg(dataGridView1, "备注说明", 0, "");
                    }
                    break;
                case "fla_501":
                    if (radioButtonLowB.Checked == true || radioButtonHighB.Checked == true)
                    {
                        analysisdata.Pef = textEdit_pefjg.Text;
                        analysisdata.c3h8 = textEditBiaoC3h8.Text;
                        analysisdata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                        analysisdata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());
                        cowc = Math.Abs(analysisdata.Cobz - analysisdata.Coclz);
                        coxdwc = cowc / analysisdata.Cobz;
                        if (cowc <= 0.06 || coxdwc <= 0.05)
                            copd = true;
                        else
                        {
                            copd = false;
                            wtgx += "CO.";
                        }
                        analysisdata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                        analysisdata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());
                        co2wc = Math.Abs(analysisdata.Co2bz - analysisdata.Co2clz);
                        co2xdwc = co2wc / analysisdata.Co2bz;
                        if (co2wc <= 0.5 || co2xdwc <= 0.05)
                            co2pd = true;
                        else
                        {
                            co2pd = false;
                            wtgx += "CO2.";
                        }
                        analysisdata.Nobz = (float)Math.Round(double.Parse(textEditBiaoNo.Text.Trim()), 0);
                        analysisdata.Noclz = (float)Math.Round(double.Parse(textEdit_nojg.Text.Trim()), 0);
                        nowc = Math.Abs(analysisdata.Nobz - analysisdata.Noclz);
                        noxdwc = nowc / analysisdata.Nobz;
                        nopd = true;
                        analysisdata.Hcbz = (float)Math.Round(double.Parse(textEditBiaoC3h8.Text.Trim()) * double.Parse(textEdit_pefjg.Text.Trim()), 0);
                        analysisdata.Hcclz = (float)Math.Round(double.Parse(textEdit_hcjg.Text.Trim()), 0);
                        hcwc = Math.Abs(analysisdata.Hcbz - analysisdata.Hcclz);
                        hcxdwc = hcwc / analysisdata.Hcbz;
                        if (hcwc <= 12 || hcxdwc <= 0.05)
                            hcpd = true;
                        else
                        {
                            hcpd = false;
                            wtgx += "HC.";
                        }
                        analysisdata.Jzds = 4;
                        analysisdata.Bzsm = startTime.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());
                        analysisdata.coabswc = (cowc).ToString("0.00");
                        analysisdata.co2abswc = (co2wc).ToString("0.00");
                        analysisdata.hcabswc = (hcwc).ToString("0");
                        analysisdata.noabswc = (nowc).ToString("0");
                        analysisdata.Copc = (coxdwc * 100).ToString("0.0");
                        analysisdata.Co2pc = (co2xdwc * 100).ToString("0.0");
                        analysisdata.Hcpc = (hcxdwc * 100).ToString("0.0");
                        analysisdata.Nopc = (noxdwc * 100).ToString("0.0");
                        if (copd == true && co2pd == true && hcpd == true && nopd == true)
                            analysisdata.Bdjg = "合格";
                        else
                            analysisdata.Bdjg = "不合格";
                        //analysismeterini.writeanalysismeterIni(analysisdata);
                        if (radioButtonHighB.Checked == true)
                        {
                            datagridview_msg(dataGridView1, "标定项目", 0, "高标气检查");
                            analysisdata.Gdjz = "0";
                        }
                        else
                        {
                            datagridview_msg(dataGridView1, "标定项目", 0, "低标气检查");
                            analysisdata.Gdjz = "1";
                        }
                        datagridview_msg(dataGridView1, "CO2标值", 0, analysisdata.Co2bz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2实测值", 0, analysisdata.Co2clz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO标值", 0, analysisdata.Cobz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO实测值", 0, analysisdata.Coclz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "NO标值", 0, analysisdata.Nobz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO实测值", 0, analysisdata.Noclz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC标值", 0, analysisdata.Hcbz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC实测值", 0, analysisdata.Hcclz.ToString("0"));
                        datagridview_msg(dataGridView1, "O2标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2实测值", 0, "非检查项");
                        if (analysisdata.Bdjg == "合格")
                        {
                            pdjg = true;
                            datagridview_msg(dataGridView1, "判定结果", 0, "通过");
                        }
                        else
                        {
                            pdjg = false;
                            datagridview_msg(dataGridView1, "判定结果", 0, "未通过");
                        }
                        datagridview_msg(dataGridView1, "备注说明", 0, wtgx);
                    }
                    else
                    {
                        o2bz = float.Parse(textEditBiaoO2.Text.Trim());
                        o2clz = float.Parse(textEdit_o2jg.Text.Trim());
                        o2wc = Math.Abs(o2bz - o2clz);
                        o2xdwc = o2wc / o2bz;
                        datagridview_msg(dataGridView1, "标定项目", 0, "氧气检查");
                        datagridview_msg(dataGridView1, "CO2标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO2实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "NO标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "NO实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "HC标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "HC实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2标值", 0, o2bz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "O2实测值", 0, o2clz.ToString("0.00"));
                        if (o2wc < 0.1 || o2xdwc < 0.05)
                        {
                            pdjg = true;
                            datagridview_msg(dataGridView1, "判定结果", 0, "通过");
                        }
                        else
                        {
                            pdjg = false;
                            datagridview_msg(dataGridView1, "判定结果", 0, "未通过");
                        }
                        datagridview_msg(dataGridView1, "备注说明", 0, "");
                    }
                    break;

                case "mqw_511":
                    if (radioButtonLowB.Checked == true || radioButtonHighB.Checked == true)
                    {
                        analysisdata.Pef = textEdit_pefjg.Text;
                        analysisdata.c3h8 = textEditBiaoC3h8.Text;
                        analysisdata.Cobz = float.Parse(textEditBiaoCo.Text.Trim());
                        analysisdata.Coclz = float.Parse(textEdit_Cojg.Text.Trim());
                        cowc = Math.Abs(analysisdata.Cobz - analysisdata.Coclz);
                        coxdwc = cowc / analysisdata.Cobz;
                        if (cowc <= 0.06 || coxdwc <= 0.05)
                            copd = true;
                        else
                        {
                            copd = false;
                            wtgx += "CO.";
                        }
                        analysisdata.Co2bz = float.Parse(textEditBiaoCo2.Text.Trim());
                        analysisdata.Co2clz = float.Parse(textEdit_co2jg.Text.Trim());
                        co2wc = Math.Abs(analysisdata.Co2bz - analysisdata.Co2clz);
                        co2xdwc = co2wc / analysisdata.Co2bz;
                        if (co2wc <= 0.5 || co2xdwc <= 0.05)
                            co2pd = true;
                        else
                        {
                            co2pd = false;
                            wtgx += "CO2.";
                        }
                        analysisdata.Nobz = (float)Math.Round(double.Parse(textEditBiaoNo.Text.Trim()), 0);
                        analysisdata.Noclz = (float)Math.Round(double.Parse(textEdit_nojg.Text.Trim()), 0);
                        nowc = Math.Abs(analysisdata.Nobz - analysisdata.Noclz);
                        noxdwc = nowc / analysisdata.Nobz;
                        nopd = true;
                        analysisdata.Hcbz = (float)Math.Round(double.Parse(textEditBiaoC3h8.Text.Trim()) * double.Parse(textEdit_pefjg.Text.Trim()), 0);
                        analysisdata.Hcclz = (float)Math.Round(double.Parse(textEdit_hcjg.Text.Trim()), 0);
                        hcwc = Math.Abs(analysisdata.Hcbz - analysisdata.Hcclz);
                        hcxdwc = hcwc / analysisdata.Hcbz;
                        if (hcwc <= 12 || hcxdwc <= 0.05)
                            hcpd = true;
                        else
                        {
                            hcpd = false;
                            wtgx += "HC.";
                        }
                        analysisdata.Jzds = 4;
                        analysisdata.Bzsm = startTime.ToString("yyyy-MM-dd HH:mm:ss") + "," + float.Parse(textEdit_pefjg.Text.Trim());
                        analysisdata.coabswc = (cowc).ToString("0.00");
                        analysisdata.co2abswc = (co2wc).ToString("0.00");
                        analysisdata.hcabswc = (hcwc).ToString("0");
                        analysisdata.noabswc = (nowc).ToString("0");
                        analysisdata.Copc = (coxdwc * 100).ToString("0.0");
                        analysisdata.Co2pc = (co2xdwc * 100).ToString("0.0");
                        analysisdata.Hcpc = (hcxdwc * 100).ToString("0.0");
                        analysisdata.Nopc = (noxdwc * 100).ToString("0.0");
                        if (copd == true && co2pd == true && hcpd == true && nopd == true)
                            analysisdata.Bdjg = "合格";
                        else
                            analysisdata.Bdjg = "不合格";
                        //analysismeterini.writeanalysismeterIni(analysisdata);
                        if (radioButtonHighB.Checked == true)
                        {
                            datagridview_msg(dataGridView1, "标定项目", 0, "高标气检查");
                            analysisdata.Gdjz = "0";
                        }
                        else
                        {
                            datagridview_msg(dataGridView1, "标定项目", 0, "低标气检查");
                            analysisdata.Gdjz = "1";
                        }
                        datagridview_msg(dataGridView1, "CO2标值", 0, analysisdata.Co2bz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO2实测值", 0, analysisdata.Co2clz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "CO标值", 0, analysisdata.Cobz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "CO实测值", 0, analysisdata.Coclz.ToString("0.000"));
                        datagridview_msg(dataGridView1, "NO标值", 0, analysisdata.Nobz.ToString("0"));
                        datagridview_msg(dataGridView1, "NO实测值", 0, analysisdata.Noclz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC标值", 0, analysisdata.Hcbz.ToString("0"));
                        datagridview_msg(dataGridView1, "HC实测值", 0, analysisdata.Hcclz.ToString("0"));
                        datagridview_msg(dataGridView1, "O2标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2实测值", 0, "非检查项");
                        if (analysisdata.Bdjg == "合格")
                        {
                            pdjg = true;
                            datagridview_msg(dataGridView1, "判定结果", 0, "通过");
                        }
                        else
                        {
                            pdjg = false;
                            datagridview_msg(dataGridView1, "判定结果", 0, "未通过");
                        }
                        datagridview_msg(dataGridView1, "备注说明", 0, wtgx);
                    }
                    else
                    {
                        o2bz = float.Parse(textEditBiaoO2.Text.Trim());
                        o2clz = float.Parse(textEdit_o2jg.Text.Trim());
                        o2wc = Math.Abs(o2bz - o2clz);
                        o2xdwc = o2wc / o2bz;
                        datagridview_msg(dataGridView1, "标定项目", 0, "氧气检查");
                        datagridview_msg(dataGridView1, "CO2标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO2实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "CO实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "NO标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "NO实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "HC标值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "HC实测值", 0, "非检查项");
                        datagridview_msg(dataGridView1, "O2标值", 0, o2bz.ToString("0.00"));
                        datagridview_msg(dataGridView1, "O2实测值", 0, o2clz.ToString("0.00"));
                        if (o2wc < 0.1 || o2xdwc < 0.05)
                        {
                            pdjg = true;
                            datagridview_msg(dataGridView1, "判定结果", 0, "通过");
                        }
                        else
                        {
                            pdjg = false;
                            datagridview_msg(dataGridView1, "判定结果", 0, "未通过");
                        }
                        datagridview_msg(dataGridView1, "备注说明", 0, "");
                    }
                    break;
            }
            return pdjg;
        }

        private void radioButtonHuanj_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonHuanj.Checked == true)
            {
                switch (UseFqy)
                {
                    case "fla_502":
                        if (fla_502 != null)
                        {
                            fla_502.setAirAsTl();
                        }
                        break;
                    case "cdf5000":
                        if (fla_502 != null)
                        {
                            fla_502.setAirAsTl();
                        }
                        break;
                    case "nha_503":
                        if (fla_502 != null)
                        {
                            fla_502.setAirAsTl();
                        }
                        break;
                    case "fasm_5000":
                        if (fla_502 != null)
                        {
                            fla_502.setAirAsTl();
                        }
                        break;
                }
            }
            else
            {
                switch (UseFqy)
                {
                    case "fla_502":
                        if (fla_502 != null)
                        {
                            fla_502.setZeroAsTl();
                        }
                        break;
                    case "cdf5000":
                        if (fla_502 != null)
                        {
                            fla_502.setZeroAsTl();
                        }
                        break;
                    case "nha_503":
                        if (fla_502 != null)
                        {
                            fla_502.setZeroAsTl();
                        }
                        break;
                    case "fasm_5000":
                        if (fla_502 != null)
                        {
                            fla_502.setZeroAsTl();
                        }
                        break;
                }
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (radioButtonBiaolq.Checked == true)
            {
                MessageBox.Show("氧气检查仅供日常检查用，必须用五点标准气标定");
            }
            else
            {
                if (analysisdata != null)
                {
                    DataTable btg_datatable = new DataTable();
                    btg_datatable.Columns.Add("TimeCount");
                    btg_datatable.Columns.Add("HC");
                    btg_datatable.Columns.Add("CO");
                    btg_datatable.Columns.Add("CO2");
                    btg_datatable.Columns.Add("NO");
                    btg_datatable.Columns.Add("O2");
                    btg_datatable.Columns.Add("PEF");
                    btg_datatable.Columns.Add("STATUS");
                    DataRow dr = null;
                    for (int i = 0; i < gksjcount; i++)
                    {
                        dr = btg_datatable.NewRow();
                        dr["TimeCount"] = TimeCountlist[i];
                        dr["HC"] = HClist[i].ToString("0");
                        dr["CO"] = COlist[i].ToString("0.000");
                        dr["CO2"] = CO2list[i].ToString("0.00");
                        dr["NO"] = NOlist[i].ToString("0");
                        dr["O2"] = O2list[i].ToString("0.00");
                        dr["PEF"] = PEFlist[i].ToString("0.000");
                        dr["STATUS"] = STATUSlist[i];
                        btg_datatable.Rows.Add(dr);
                    }

                    csvwriter.SaveCSV(btg_datatable, "C:/jcdatatxt/" + "AnalyzerCalCheck.csv");
                    analysisdata.Starttime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    analysismeterini.writeanalysismeterIni(analysisdata);
                    MessageBox.Show("数据保存成功");
                    isSaved = true;
                }
                else
                    MessageBox.Show("没有有效数据进行保存，请先进行标准气检查");
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData==Keys.F5)
            {
                if (jiancha_detect())
                    Msg_label(label_jcjg, panel_jcjg, "通过");
                else
                    Msg_label(label_jcjg, panel_jcjg, "未通过");
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (isSaveStartTime)//如果正在测试
            {
                nowtime = DateTime.Now;
                if ((int)(gongkuangTime) != GKSJ)//100ms一次
                {
                    if (GKSJ == 10230) GKSJ = 0;
                    TimeCountlist[GKSJ] = GKSJ;
                    CO2list[GKSJ] = float.Parse(textEditCo2.Text);
                    COlist[GKSJ] = float.Parse(textEditCo.Text);
                    HClist[GKSJ] = float.Parse(textEditHc.Text);
                    O2list[GKSJ] = float.Parse(textEditO2.Text);
                    NOlist[GKSJ] = float.Parse(textEditNo.Text);
                    PEFlist[GKSJ] = float.Parse(textEditPEF.Text);
                    STATUSlist[GKSJ] = status;
                    GKSJ++;//工况时间加1
                }
                TimeSpan timespan = nowtime - startTime;
                gongkuangTime = (float)timespan.TotalMilliseconds / 1000f;
            }
            else
            {
                GKSJ = 0;
                perGKSJ = 0;
                gongkuangTime = 0f;
            }
        }

        private void buttonDemarcateMQ_Click(object sender, EventArgs e)
        {
            isdermarcateMq = true;
        }

    }
}
