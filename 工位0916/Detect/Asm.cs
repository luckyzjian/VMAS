using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Detect
{
    public partial class Asm : Form
    {
        SYS.Model.ASM_XZDB asm_xzdb = new SYS.Model.ASM_XZDB();                                 //ASM限值地标
        public delegate void wtcs(Control controlname, string text);                            //委托
        public delegate void wtds(double Data, string ChartName);                               //委托
        public delegate void wt_void();                                                         //委托
        public delegate void wtlsb(Label Msgowner,string Msgstr,bool Update_DB);                //委托
        public delegate void wtlp(Label Msgowner,Panel Msgfather);                              //委托
        //public delegate void wtds(double Data,string SeriesName);                               //委托
        public delegate void wtdd(double min, double max);                                      //委托
        public bool JC_Status = false;                                                          //检测状态
        public string JC_Circuit="5025";                                                        //检测流程
        Thread TH_ST = null;                                                                    //检测线程
        Thread TH_Speed_st = null;                                                              //速度检测线程
        public int Preset_Time = 10;                                                            //废气分析仪预置时间，默认10秒
        public float[] Speed_List5025 = new float[91];                                          //5025工况每秒速度
        public float[] Speed_List2540 = new float[91];                                          //2540工况每秒速度
        public double[] Pd ={0.61129,0.65716,0.70605,0.75813,0.81359,
                           0.87260,0.93537,1.0021,1.0730,1.1482,
                           1.2281,1.3129,1.4027,1.4979,1.5988,
                           1.7056,1.8185,1.9380,2.0644,2.1978,
                           2.3388,2.4877,2.6447,2.8104,2.9850,
                           3.1690,3.3629,3.5670,3.7818,4.0078,
                           4.2455};                                                             //0到30度的饱和蒸汽压
        public double WD = 26;                                                                  //温度
        public double SD = 75;                                                                  //相对湿度
        public double DQY = 100;                                                                //大气压
        public Exhaust.Exhaust_Data[] Asm_Exhaust_List5025 = new Exhaust.Exhaust_Data[95];      //5025工况每秒废气结果
        public Exhaust.Exhaust_Data[] Asm_Exhaust_Revise_List5025 = new Exhaust.Exhaust_Data[95];//5025工况每秒废气修正后结果
        public Exhaust.Exhaust_Data[] Asm_Exhaust_List2540 = new Exhaust.Exhaust_Data[95];      //2540工况每秒废气结果
        public Exhaust.Exhaust_Data[] Asm_Exhaust_Revise_List2540 = new Exhaust.Exhaust_Data[95];//2540工况每秒废气修正后结果
        DataTable Jccl=null;                                                                    //检测车辆信息
        SYS.Model.ASM asm = new SYS.Model.ASM();                                                //asm检测数据
        public string Cllx = "";                                                                //车辆类型
        public float Limit_HC_5025 = 1;                                                         //5025HC限值
        public float Limit_CO_5025 = 1;                                                         //5025CO限值
        public float Limit_NO_5025 = 1;                                                         //5025NO限值
        public float Limit_HC_2540 = 1;                                                         //2540HC限值
        public float Limit_CO_2540 = 1;                                                         //2540CO限值
        public float Limit_NO_2540 = 1;                                                         //2540NO限值
        public string HC_5025_JG = "";                                                          //5025HC结果
        public string CO_5025_JG = "";                                                          //5025CO结果
        public string NO_5025_JG = "";                                                          //5025NO结果
        public string HC_2540_JG = "";                                                          //2540HC结果
        public string CO_2540_JG = "";                                                          //2540CO结果
        public string NO_2540_JG = "";                                                          //2540NO结果
        public string HC_5025_PD = "";                                                          //5025HC判定
        public string CO_5025_PD = "";                                                          //5025CO判定
        public string NO_5025_PD = "";                                                          //5025NO判定
        public string HC_2540_PD = "";                                                          //2540HC判定
        public string CO_2540_PD = "";                                                          //2540CO判定
        public string NO_2540_PD = "";                                                          //2540NO判定
        public string ASM_PD = "";                                                              //ASM总结果
        public int GKSJ = 0;                                                                    //工况时间
        public int Jcjg_Status = 0;                                                             //检测结果状态 0为不合格、1为5025快速工况合格、2为5025工况工况合格、3为2540快速工况合格、4为2540工况合格、5为5025工况重新计时、6为2540工况重新计时、7为检测终止。
        
        public bool Speed_Jc_flag = false;                                                      //速度检查标记
        public bool Speed_flag = false;                                                         //快速工况速度超过限制标记
        public int Out_times = 0;                                                               //判断是不是超出3秒的变量
        public int Arise = 0;                                                                   //超出3秒的次数
        public float Set_Power = 0;                                                             //扭矩

        public Asm()
        {
            InitializeComponent();
        }

        private void Asm_Load(object sender, EventArgs e)
        {
            Init_Chart();
            Init_Chart5025();   //初始化图标
            Init_Data();        //初始化数据
            Init_Limit();       //初始化限值
            timer1.Interval = 100;
            timer1.Start();
            //Init_Meteorology(); //初始化气象数据
        }

        #region 初始化

        public void Init_Chart()
        {
            chart1.Series["seriesCeiling"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series["seriesLower"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series["seriesdata"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;

            chart1.Series["seriesCeiling"].Color = System.Drawing.Color.Red;
            chart1.Series["seriesCeiling"].BorderWidth = 4;
            chart1.Series["seriesLower"].Color = System.Drawing.Color.Red;
            chart1.Series["seriesLower"].BorderWidth = 4;
            chart1.Series["seriesdata"].Color = System.Drawing.Color.Green;
            chart1.Series["seriesdata"].BorderWidth = 2;
            chart1.ChartAreas["ChartArea1"].AxisX.Interval = 100000;
            //chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 100;
            //chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0;
            //chart1.ChartAreas["ChartArea1"].AxisY.Title = "光吸收系数&吸收功率";
            //chart1.ChartAreas["ChartArea1"].AxisY.Interval = 10;
            //chart1.ChartAreas["ChartArea1"].AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            //chart1.ChartAreas["ChartArea1"].AxisY2.Maximum = 10000;
            //chart1.ChartAreas["ChartArea1"].AxisY2.Minimum = 0;
            //chart1.ChartAreas["ChartArea1"].AxisY2.Title = "转速";
            //chart1.ChartAreas["ChartArea1"].AxisY2.Interval = 1000;
            //Clear_Chart();
        }

        public void Init_Meteorology()
        {
            try
            {
                if (asm.WD==null||asm.WD == ""|| asm.SD==null|| asm.SD == ""||asm.DQY==null || asm.DQY == "")     //没有手动录入气象数据要获取
                //获取气象数据并初始化
                {
                    asm.WD = "26";
                    asm.DQY = "101";
                    asm.SD = "70";
                }
                WD = double.Parse(asm.WD);
                SD = double.Parse(asm.SD);
                DQY = double.Parse(asm.DQY);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString(), "气象数据初始化失败");
            }
        }

        public void Init_Limit()
        {
            asm_xzdb = CarWait.gbdal.Get_ASM_XZDB();
            try
            {
                if (int.Parse(Jccl.Rows[0]["HDZK"].ToString()) <= 6 && int.Parse(Jccl.Rows[0]["ZDZZL"].ToString()) <= 2500 && (Jccl.Rows[0]["CLLX"].ToString().IndexOf("客") > -1 || Jccl.Rows[0]["CLLX"].ToString().IndexOf("轿") > -1))     //第一类轻型汽车
                    Cllx = "第一类轻型汽车";
                else if (int.Parse(Jccl.Rows[0]["ZDZZL"].ToString()) <= 3500)       //第二类轻型汽车
                    Cllx = "第二类轻型汽车";
                else
                    Cllx = "重型汽车";              //重型汽车
                switch (Cllx)
                {
                    case "第一类轻型汽车":
                        if (DateTime.Compare(Convert.ToDateTime(Jccl.Rows[0]["CCRQ"].ToString()), Convert.ToDateTime("2000-07-01")) < 0)    //2000年7月1日前生产的第一类轻型汽车
                        {
                            if (int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1020)
                            {
                                //Limit_HC_5025 = 230;
                                //Limit_CO_5025 = 2.2f;
                                //Limit_NO_5025 = 4200;
                                //Limit_HC_2540 = 230;
                                //Limit_CO_2540 = 2.9f;
                                //Limit_NO_2540 = 3900;
                                Limit_HC_5025 = float.Parse(asm_xzdb.q1020d1Date200007015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.q1020d1Date200007015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.q1020d1Date200007015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.q1020d1Date20000701q2540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.q1020d1Date20000701q2540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.q1020d1Date20000701q2540NO);
                            }
                            if (1020 <int.Parse(Jccl.Rows[0]["JZZL"].ToString())&& int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1250)
                            {
                                //Limit_HC_5025 = 150;
                                //Limit_CO_5025 = 1.8f;
                                //Limit_NO_5025 = 3400;
                                //Limit_HC_2540 = 190;
                                //Limit_CO_2540 = 2.4f;
                                //Limit_NO_2540 = 3200;
                                Limit_HC_5025 = float.Parse(asm_xzdb.q10201250d1Date200007015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.q10201250d1Date200007015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.q10201250d1Date200007015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.q10201250d1Date200007012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.q10201250d1Date200007012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.q10201250d1Date200007012540NO);
                            }
                            if (1250 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()) && int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1470)
                            {
                                //Limit_HC_5025 = 170;
                                //Limit_CO_5025 = 1.6f;
                                //Limit_NO_5025 = 3000;
                                //Limit_HC_2540 = 170;
                                //Limit_CO_2540 = 2.1f;
                                //Limit_NO_2540 = 2800;
                                Limit_HC_5025 = float.Parse(asm_xzdb.q12501470d1Date200007015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.q12501470d1Date200007015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.q12501470d1Date200007015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.q12501470d1Date200007012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.q12501470d1Date200007012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.q12501470d1Date200007012540NO);
                            }
                            if (1470 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()) && int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1700)
                            {
                                //Limit_HC_5025 = 160;
                                //Limit_CO_5025 = 1.5f;
                                //Limit_NO_5025 = 2650;
                                //Limit_HC_2540 = 150;
                                //Limit_CO_2540 = 1.9f;
                                //Limit_NO_2540 = 2500;
                                Limit_HC_5025 = float.Parse(asm_xzdb.q14701700d1Date200007015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.q14701700d1Date200007015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.q14701700d1Date200007015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.q14701700d1Date200007012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.q14701700d1Date200007012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.q14701700d1Date200007012540NO);
                            }
                            if (1700 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()) && int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1930)
                            {
                                //Limit_HC_5025 = 130;
                                //Limit_CO_5025 = 1.2f;
                                //Limit_NO_5025 = 2200;
                                //Limit_HC_2540 = 130;
                                //Limit_CO_2540 = 1.6f;
                                //Limit_NO_2540 = 2050;
                                Limit_HC_5025 = float.Parse(asm_xzdb.q17001930d1Date200007015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.q17001930d1Date200007015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.q17001930d1Date200007015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.q17001930d2Date200110012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.q17001930d2Date200110012540CO);
                                Limit_CO_2540 = float.Parse(asm_xzdb.q17001930d2Date200110012540NO);
                            }
                            if (1930 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()) && int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 2150)
                            {
                                //Limit_HC_5025 = 120;
                                //Limit_CO_5025 = 1.1f;
                                //Limit_NO_5025 = 2000;
                                //Limit_HC_2540 = 120;
                                //Limit_CO_2540 = 1.5f;
                                //Limit_NO_2540 = 1850;
                                Limit_HC_5025 = float.Parse(asm_xzdb.q19302150d1Date200007015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.q19302150d1Date200007015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.q19302150d1Date200007015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.q19302150d1Date200007012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.q19302150d1Date200007012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.q19302150d1Date200007012540NO);
                            }
                            if (2150 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()))
                            {
                                //Limit_HC_5025 = 110;
                                //Limit_CO_5025 = 1.1f;
                                //Limit_NO_5025 = 1700;
                                //Limit_HC_2540 = 110;
                                //Limit_CO_2540 = 1.3f;
                                //Limit_NO_2540 = 1600;
                                Limit_HC_5025 = float.Parse(asm_xzdb.q2150d1Date200007015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.q2150d1Date200007015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.q2150d1Date200007015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.q2150d1Date200007012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.q2150d1Date200007012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.q2150d1Date200007012540NO);
                            }
                        }
                        else     //2000年7月1日起生产的第一类轻型汽车
                        {
                            if (int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1020)
                            {
                                //Limit_HC_5025 = 230;
                                //Limit_CO_5025 = 1.3f;
                                //Limit_NO_5025 = 1850;
                                //Limit_HC_2540 = 230;
                                //Limit_CO_2540 = 1.5f;
                                //Limit_NO_2540 = 1700;
                                Limit_HC_5025 = float.Parse(asm_xzdb.s1020d1Date200007015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.s1020d1Date200007015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.s1020d1Date200007015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.s1020d1Date200007012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.s1020d1Date200007012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.s1020d1Date200007012540NO);
                            }
                            if (1020 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()) && int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1250)
                            {
                                //Limit_HC_5025 = 190;
                                //Limit_CO_5025 = 1.1f;
                                //Limit_NO_5025 = 1500;
                                //Limit_HC_2540 = 190;
                                //Limit_CO_2540 = 1.2f;
                                //Limit_NO_2540 = 1350;
                                Limit_HC_5025 = float.Parse(asm_xzdb.s10201250d1Date200007015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.s10201250d1Date200007015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.s10201250d1Date200007015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.s10201250d1Date200007012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.s10201250d1Date200007012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.s10201250d1Date200007012540NO);
                            }
                            if (1250 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()) && int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1470)
                            {
                                //Limit_HC_5025 = 170;
                                //Limit_CO_5025 = 1.0f;
                                //Limit_NO_5025 = 1300;
                                //Limit_HC_2540 = 170;
                                //Limit_CO_2540 = 1.1f;
                                //Limit_NO_2540 = 1200;
                                Limit_HC_5025 = float.Parse(asm_xzdb.s12501470d1Date200007015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.s12501470d1Date200007015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.s12501470d1Date200007015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.s12501470d1Date200007012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.s12501470d1Date200007012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.s12501470d1Date200007012540NO);
                            }
                            if (1470 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()) && int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1700)
                            {
                                //Limit_HC_5025 = 160;
                                //Limit_CO_5025 = 0.9f;
                                //Limit_NO_5025 = 1200;
                                //Limit_HC_2540 = 150;
                                //Limit_CO_2540 = 1.0f;
                                //Limit_NO_2540 = 1100;
                                Limit_HC_5025 = float.Parse(asm_xzdb.s14701700d1Date200007015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.s14701700d1Date200007015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.s14701700d1Date200007015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.s14701700d1Date200007012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.s14701700d1Date200007012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.s14701700d1Date200007012540NO);
                            }
                            if (1700 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()) && int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1930)
                            {
                                //Limit_HC_5025 = 130;
                                //Limit_CO_5025 = 0.8f;
                                //Limit_NO_5025 = 1000;
                                //Limit_HC_2540 = 130;
                                //Limit_CO_2540 = 0.8f;
                                //Limit_NO_2540 = 900;
                                Limit_HC_5025 = float.Parse(asm_xzdb.s17001930d1Date200007015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.s17001930d1Date200007015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.s17001930d1Date200007015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.s17001930d1Date200007012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.s17001930d1Date200007012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.s17001930d1Date200007012540NO);

                            }
                            if (1930 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()) && int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 2150)
                            {
                                //Limit_HC_5025 = 120;
                                //Limit_CO_5025 = 0.7f;
                                //Limit_NO_5025 = 900;
                                //Limit_HC_2540 = 120;
                                //Limit_CO_2540 = 0.8f;
                                //Limit_NO_2540 = 800;
                                Limit_HC_5025 = float.Parse(asm_xzdb.s19302150d1Date200007015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.s19302150d1Date200007015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.s19302150d1Date200007015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.s19302150d2Date200110012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.s19302150d2Date200110012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.s19302150d2Date200110012540NO);
                            }
                            if (2150 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()))
                            {
                                //Limit_HC_5025 = 110;
                                //Limit_CO_5025 = 0.6f;
                                //Limit_NO_5025 = 750;
                                //Limit_HC_2540 = 110;
                                //Limit_CO_2540 = 0.7f;
                                //Limit_NO_2540 = 700;
                                Limit_HC_5025 = float.Parse(asm_xzdb.s2150d1Date200007015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.s2150d1Date200007015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.s2150d1Date200007015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.s2150d1Date200007012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.s2150d1Date200007012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.s2150d1Date200007012540NO);
                            }
                        }
                        break;
                    case "第二类轻型汽车":
                        if (DateTime.Compare(Convert.ToDateTime(Jccl.Rows[0]["CCRQ"].ToString()), Convert.ToDateTime("2001-10-01")) < 0)    //2001年10月1日前生产的第二类轻型汽车
                        {
                            if (int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1020)
                            {
                                //Limit_HC_5025 = 230;
                                //Limit_CO_5025 = 2.2f;
                                //Limit_NO_5025 = 4200;
                                //Limit_HC_2540 = 230;
                                //Limit_CO_2540 = 2.9f;
                                //Limit_NO_2540 = 3900;
                                Limit_HC_5025 = float.Parse(asm_xzdb.q1020d2Date200110015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.q1020d2Date200110015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.q1020d2Date200110015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.q1020d2Date200110012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.q1020d2Date200110012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.q1020d2Date200110012540NO);
                            }
                            if (1020 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()) && int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1250)
                            {
                                //Limit_HC_5025 = 150;
                                //Limit_CO_5025 = 1.8f;
                                //Limit_NO_5025 = 3400;
                                //Limit_HC_2540 = 190;
                                //Limit_CO_2540 = 2.4f;
                                //Limit_NO_2540 = 3200;
                                Limit_HC_5025 = float.Parse(asm_xzdb.q10201250d2Date200110015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.q10201250d2Date200110015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.q10201250d2Date200110015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.q10201250d2Date200110012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.q10201250d2Date200110012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.q10201250d2Date200110012540NO);
                            }
                            if (1250 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()) && int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1470)
                            {
                                //Limit_HC_5025 = 170;
                                //Limit_CO_5025 = 1.6f;
                                //Limit_NO_5025 = 3000;
                                //Limit_HC_2540 = 170;
                                //Limit_CO_2540 = 2.1f;
                                //Limit_NO_2540 = 2800;
                                Limit_HC_5025 = float.Parse(asm_xzdb.q12501470d2Date200110015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.q12501470d2Date200110015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.q12501470d2Date200110015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.q12501470d2Date200110012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.q12501470d2Date200110012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.q12501470d2Date200110012540NO);
                            }
                            if (1470 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()) && int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1700)
                            {
                                //Limit_HC_5025 = 160;
                                //Limit_CO_5025 = 1.5f;
                                //Limit_NO_5025 = 2650;
                                //Limit_HC_2540 = 150;
                                //Limit_CO_2540 = 1.9f;
                                //Limit_NO_2540 = 2500;
                                Limit_HC_5025 = float.Parse(asm_xzdb.q14701700d2Date200110015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.q14701700d2Date200110015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.q14701700d2Date200110015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.q14701700d2Date200110012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.q14701700d2Date200110012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.q14701700d2Date200110012540NO);
                            }
                            if (1700 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()) && int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1930)
                            {
                                //Limit_HC_5025 = 130;
                                //Limit_CO_5025 = 1.2f;
                                //Limit_NO_5025 = 2200;
                                //Limit_HC_2540 = 130;
                                //Limit_CO_2540 = 1.6f;
                                //Limit_NO_2540 = 2050;
                                Limit_HC_5025 = float.Parse(asm_xzdb.q17001930d2Date200110015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.q17001930d2Date200110015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.q17001930d2Date200110015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.q17001930d2Date200110012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.q17001930d2Date200110012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.q17001930d2Date200110012540NO);
                            }
                            if (1930 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()) && int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 2150)
                            {
                                //Limit_HC_5025 = 120;
                                //Limit_CO_5025 = 1.1f;
                                //Limit_NO_5025 = 2000;
                                //Limit_HC_2540 = 120;
                                //Limit_CO_2540 = 1.5f;
                                //Limit_NO_2540 = 1850;
                                Limit_HC_5025 = float.Parse(asm_xzdb.q19302150d2Date200110015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.q19302150d2Date200110015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.q19302150d2Date200110015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.q19302150d2Date200110012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.q19302150d2Date200110012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.q19302150d2Date200110012540NO);
                            }
                            if (2150 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()))
                            {
                                //Limit_HC_5025 = 110;
                                //Limit_CO_5025 = 1.1f;
                                //Limit_NO_5025 = 1700;
                                //Limit_HC_2540 = 110;
                                //Limit_CO_2540 = 1.3f;
                                //Limit_NO_2540 = 1600;
                                Limit_HC_5025 = float.Parse(asm_xzdb.q2150d2Date200110015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.q2150d2Date200110015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.q2150d2Date200110015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.q2150d2Date200110012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.q2150d2Date200110012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.q2150d2Date200110012540NO);
                            }
                        }
                        else     //2001年10月1日起生产的第二类轻型汽车
                        {
                            if (int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1020)
                            {
                                //Limit_HC_5025 = 230;
                                //Limit_CO_5025 = 1.3f;
                                //Limit_NO_5025 = 1850;
                                //Limit_HC_2540 = 230;
                                //Limit_CO_2540 = 1.5f;
                                //Limit_NO_2540 = 1700;
                                Limit_HC_5025 = float.Parse(asm_xzdb.s1020d2Date200110015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.s1020d2Date200110015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.s1020d2Date200110015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.s1020d2Date200110012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.s1020d2Date200110012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.s1020d2Date200110012540NO);

                            }
                            if (1020 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()) && int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1250)
                            {
                                //Limit_HC_5025 = 190;
                                //Limit_CO_5025 = 1.1f;
                                //Limit_NO_5025 = 1500;
                                //Limit_HC_2540 = 190;
                                //Limit_CO_2540 = 1.2f;
                                //Limit_NO_2540 = 1350;
                                Limit_HC_5025 = float.Parse(asm_xzdb.s10201250d2Date200110015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.s10201250d2Date200110015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.s10201250d2Date200110015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.s10201250d2Date200110012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.s10201250d2Date200110012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.s10201250d2Date200110012540NO);
                            }
                            if (1250 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()) && int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1470)
                            {
                                //Limit_HC_5025 = 170;
                                //Limit_CO_5025 = 1.0f;
                                //Limit_NO_5025 = 1300;
                                //Limit_HC_2540 = 170;
                                //Limit_CO_2540 = 1.1f;
                                //Limit_NO_2540 = 1200;
                                Limit_HC_5025 = float.Parse(asm_xzdb.s12501470d2Date200110015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.s12501470d2Date200110015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.s12501470d2Date200110015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.s12501470d2Date200110012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.s12501470d2Date200110012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.s12501470d2Date200110012540NO);
                            }
                            if (1470 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()) && int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1700)
                            {
                                //Limit_HC_5025 = 160;
                                //Limit_CO_5025 = 0.9f;
                                //Limit_NO_5025 = 1200;
                                //Limit_HC_2540 = 150;
                                //Limit_CO_2540 = 1.0f;
                                //Limit_NO_2540 = 1100;
                                Limit_HC_5025 = float.Parse(asm_xzdb.s14701700d2Date200110015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.s14701700d2Date200110015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.s14701700d2Date200110015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.s14701700d2Date200110012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.s14701700d2Date200110012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.s14701700d2Date200110012540NO);

                            }
                            if (1700 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()) && int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 1930)
                            {
                                //Limit_HC_5025 = 130;
                                //Limit_CO_5025 = 0.8f;
                                //Limit_NO_5025 = 1000;
                                //Limit_HC_2540 = 130;
                                //Limit_CO_2540 = 0.8f;
                                //Limit_NO_2540 = 900;
                                Limit_HC_5025 = float.Parse(asm_xzdb.s17001930d2Date200110015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.s17001930d2Date200110015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.s17001930d2Date200110015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.s17001930d2Date200110012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.s17001930d2Date200110012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.s17001930d2Date200110012540NO);
                            }
                            if (1930 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()) && int.Parse(Jccl.Rows[0]["JZZL"].ToString()) <= 2150)
                            {
                                //Limit_HC_5025 = 120;
                                //Limit_CO_5025 = 0.7f;
                                //Limit_NO_5025 = 900;
                                //Limit_HC_2540 = 120;
                                //Limit_CO_2540 = 0.8f;
                                //Limit_NO_2540 = 800;
                                Limit_HC_5025 = float.Parse(asm_xzdb.s19302150d2Date200110015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.s19302150d2Date200110015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.s19302150d2Date200110015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.s19302150d2Date200110012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.s19302150d2Date200110012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.s19302150d2Date200110012540NO);
                            }
                            if (2150 < int.Parse(Jccl.Rows[0]["JZZL"].ToString()))
                            {
                                //Limit_HC_5025 = 110;
                                //Limit_CO_5025 = 0.6f;
                                //Limit_NO_5025 = 750;
                                //Limit_HC_2540 = 110;
                                //Limit_CO_2540 = 0.7f;
                                //Limit_NO_2540 = 700;
                                Limit_HC_5025 = float.Parse(asm_xzdb.s2150d2Date200110015025HC);
                                Limit_CO_5025 = float.Parse(asm_xzdb.s2150d2Date200110015025CO);
                                Limit_NO_5025 = float.Parse(asm_xzdb.s2150d2Date200110015025NO);
                                Limit_HC_2540 = float.Parse(asm_xzdb.s2150d2Date200110012540HC);
                                Limit_CO_2540 = float.Parse(asm_xzdb.s2150d2Date200110012540CO);
                                Limit_NO_2540 = float.Parse(asm_xzdb.s2150d2Date200110012540NO);
                            }
                        }
                        break;
                    case "重型汽车":
                        if (DateTime.Compare(Convert.ToDateTime(Jccl.Rows[0]["CCRQ"].ToString()), Convert.ToDateTime("2004-09-01")) < 0)    //2004年9月1日前生产的重型汽车
                        {
                            //Limit_HC_5025 = 260;
                            //Limit_CO_5025 = 2.5f;
                            //Limit_NO_5025 = 4800;
                            //Limit_HC_2540 = 260;
                            //Limit_CO_2540 = 3.3f;
                            //Limit_NO_2540 = 4500;
                            Limit_HC_5025 = float.Parse(asm_xzdb.zxDate20040901q5025HC);
                            Limit_CO_5025 = float.Parse(asm_xzdb.zxDate20040901q5025CO);
                            Limit_NO_5025 = float.Parse(asm_xzdb.zxDate20040901q5025NO);
                            Limit_HC_2540 = float.Parse(asm_xzdb.zxDate20040901q2540HC);
                            Limit_CO_2540 = float.Parse(asm_xzdb.zxDate20040901q2540CO);
                            Limit_NO_2540 = float.Parse(asm_xzdb.zxDate20040901q2540NO);
                        }
                        else    //2004年9月1日起生产的重型汽车
                        {
                            //Limit_HC_5025 = 260;
                            //Limit_CO_5025 = 1.3f;
                            //Limit_NO_5025 = 2300;
                            //Limit_HC_2540 = 260;
                            //Limit_CO_2540 = 1.5f;
                            //Limit_NO_2540 = 2100;
                            Limit_HC_5025 = float.Parse(asm_xzdb.zxDate200409015025HC);
                            Limit_CO_5025 = float.Parse(asm_xzdb.zxDate200409015025CO);
                            Limit_NO_5025 = float.Parse(asm_xzdb.zxDate200409015025NO);
                            Limit_HC_2540 = float.Parse(asm_xzdb.zxDate200409012540HC);
                            Limit_CO_2540 = float.Parse(asm_xzdb.zxDate200409012540CO);
                            Limit_NO_2540 = float.Parse(asm_xzdb.zxDate200409012540NO);
                        }
                        break;
                }
            }
            catch (Exception )
            {
                MessageBox.Show("初始化排放限值出错，请检查被检车辆信息是否正确！", "出错啦");
            }
        }

        public void Init_Data()
        {
            asm = CarWait.asmdal.Get_ASM(CarWait.bjcl.JCBH, CarWait.bjcl.JCCS);
            Jccl = CarWait.bjclxx.Get_Carxx(CarWait.bjcl.JCBH);
            Msg(Msg_cp, panel_cp, Jccl.Rows[0]["JCCLPH"].ToString(),false);
            Msg(Msg_msg, panel_msg, Jccl.Rows[0]["JCCLPH"].ToString() + "请上测功机检测",true);
        }

        public void Init_Chart5025()
        {
            Clear_Chart();
            Setlimit(23, 27);
            for (int i = 0; i < 31; i++)
            {
                Ref_Chart(23.5 + i * 0.1, "seriesCeiling");
                Ref_Chart(20.5 + i * 0.1, "seriesLower");
            }

            for (int i = 0; i < 500; i++)
            {
                Ref_Chart(26.5, "seriesCeiling");
                Ref_Chart(23.5, "seriesLower");
            }
            //Init_Chart2540();
        }

        public void Init_Chart2540()
        {
            Clear_Chart();
            Setlimit(38, 42);
            for (int i = 0; i <= 30; i++)
            {
                Ref_Chart(38.5+i*0.1, "seriesCeiling");
                Ref_Chart(35.5+i*0.1, "seriesLower");
            }

            for (int i = 0; i < 500; i++)
            {
                Ref_Chart(41.5, "seriesCeiling");
                Ref_Chart(38.5, "seriesLower");
            }
        }

        #endregion

        public void Ref_Chart(double Data, string SeriesName)
        {
            Invoke(new wtds(ref_chart_data),Data,SeriesName );
        }

        void ref_chart_data(double Data,string SeriesName)
        {
            try
            {
                chart1.Series[SeriesName].Points.AddY(Data);
            }
            catch (Exception)
            {
            }
        }

        public void Clear_Chart()
        {
            BeginInvoke(new wt_void(clear_chart_data));
        }

        void clear_chart_data()
        {
            try
            {
                foreach (System.Windows.Forms.DataVisualization.Charting.Series series in chart1.Series)
                    series.Points.Clear();
            }
            catch (Exception)
            {
            }
        }

        public void Setlimit(double min, double max)
        {
            this.BeginInvoke(new wtdd(setlimit), min, max);
        }

        private void setlimit(double min, double max)
        {
            try
            {
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = max;
                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = min;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Speed_Detect()
        {
            while (true)
            {
                Thread.Sleep(1000);
                if (Speed_Jc_flag)
                {
                    try
                    {
                        if (JC_Circuit == "5025")
                        {
                            if (CarWait.Speed > 26.5 || CarWait.Speed < 23.5)
                                Out_times += 1;
                            else
                                Out_times = 0;
                        }
                        else
                        {
                            if (CarWait.Speed > 41.5 || CarWait.Speed < 38.5)
                                Out_times += 1;
                            else
                                Out_times = 0;
                        }
                        if (Out_times >= 3)                    //检测将被终止或工况计时重新开始
                        {
                            Arise += 1;
                            if (Arise < 2)
                            {
                                Msg(Msg_msg, panel_msg, "速度第" + Arise.ToString() + "次超出范围", true);
                                if (JC_Circuit == "5025")
                                {
                                    Speed_Jc_flag = false;
                                    Jcjg_Status = 5;
                                    TH_ST.Abort();
                                    Thread.Sleep(1000);
                                    Msg(Msg_msg, panel_msg, JC_Circuit + "工况重新计时", true);
                                    TH_ST = new Thread(Jc_Exe);
                                    TH_ST.Start();
                                }
                                else
                                {
                                    Speed_Jc_flag = false;
                                    Jcjg_Status = 6;
                                    TH_ST.Abort();
                                    Thread.Sleep(1000);
                                    Msg(Msg_msg, panel_msg, JC_Circuit + "工况重新计时", true);
                                    TH_ST = new Thread(Jc_Exe);
                                    TH_ST.Start();
                                }
                                //for (int d = 0; d < i; d++) //由于工况重新计时新绘制一段限制曲线     
                                //{
                                //    Ref_Chart(26.5, "seriesCeiling");
                                //    Ref_Chart(23.5, "seriesLower");
                                //}
                                //i = 0;                        //工况计时重新开始
                            }
                            else
                            {
                                Msg(Msg_msg, panel_msg, "速度再次超值，检测结束", true);
                                Speed_Jc_flag = false;
                                //Jcjg_Status = 7;
                                Jcjg_Status = 0;
                                TH_ST.Abort();
                                Ref_Control_Text(button_ss, "重新开始");
                                JC_Status = false;
                                return;
                                //检测终止
                            }
                            Out_times = 0;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public void Jc_Exe()
        {
            //检测结果状态 0为不合格、1为5025快速工况合格、2为5025工况工况合格、3为2540快速工况合格、4为2540工况合格、5为5025工况重新计时、6为2540工况重新计时、7为检测终止。
            asm.ASM25HC_XZ = Limit_HC_5025.ToString() ;
            asm.ASM25CO_XZ = Limit_CO_5025.ToString();
            asm.ASM25NO_XZ = Limit_NO_5025.ToString();
            asm.ASM40HC_XZ = Limit_HC_2540.ToString();
            asm.ASM40CO_XZ = Limit_CO_2540.ToString();
            asm.ASM40NO_XZ = Limit_NO_2540.ToString();
            asm.ASM25HC_PD="";
            asm.ASM25CO_PD="";
            asm.ASM25NO_PD="";
            asm.ASM40HC_PD="";
            asm.ASM40CO_PD="";
            asm.ASM40NO_PD = "";
            asm.ASM_PD = "";
            switch(CarWait.fqfxy.SBMS.Split(new string[]{"||"},StringSplitOptions.RemoveEmptyEntries)[1])           //通过仪器型号选择测量流程
            {
                case "MQW-50A":
                    switch (Jcjg_Status)
                    {
                        case 0:                             //这里用switch 来确定哪款废气分析仪用什么流程
                            Jc_Exe5025();
                            break;
                        case 2:
                            Jc_Exe2540();
                            break;
                        case 5:
                            Jc_Exe5025();
                            break;
                        case 6:
                            Jc_Exe2540();
                            break;
                    }
                    break;
            }
        }

        #region 5025工况

        /// <summary>
        /// 5025工况
        /// </summary>
        public void Jc_Exe5025()
        {
            int temp_flag=0;                //计数临时变量
            try
            {
                Set_Power = 0;
                CarWait.igbt.Exit_Control();                        //IGBT退出所有控制
                CarWait.igbt.Set_Control_Power(Set_Power);                 //设置恒扭矩值
                CarWait.igbt.Start_Control_Power();                 //启动恒扭矩控制
                JC_Circuit = "5025";
                Msg(Msg_msg, panel_msg, JC_Circuit + "开始，请加速到25Km/h", true);
                float hc = 0;
                float co = 0;
                float no = 0;
                float co2 = 0;
                string asm_exh_co = "";
                string asm_exh_hc = "";
                string asm_exh_no = "";
                string asm_exh_co2 = "";
                if (Jcjg_Status != 5)
                    Arise = 0;
                //加速阶段
                while (true)
                {
                    if (CarWait.Speed > 10)
                        break;
                    Thread.Sleep(100);
                }
                for (int i = 0; i < 15; i++)
                {
                    Ref_Chart(CarWait.Speed, "seriesdata");
                    Thread.Sleep(100);
                }
                for (int i = 1; i <= 50; i++)               //t=0,工况开始
                {
                    //Speed = Get_Speed();
                    Ref_Chart(CarWait.Speed, "seriesdata");         //取速度，耗时10毫秒
                    //当车辆的加速度为1.475m/s^2时，以车辆输出功率的50%作为设定功率加载。
                    //CarWait.CGJ_Jzgl = 10;
                    Thread.Sleep(100);
                    if (i % 10 == 0)
                    {
                        Set_Power += 1;                                   //每秒增加0.5kw
                        CarWait.igbt.Set_Control_Power(Set_Power);          
                        //CarWait.igbt.Start_Control_Force();
                        GKSJ += 1;
                        Speed_List5025[GKSJ] = CarWait.Speed;
                        Msg(Msg_msg, panel_msg, "时间：" + GKSJ.ToString() + "S" + "  保持速度25Km/h", true);
                    }
                }
                //加速完成
                temp_flag = 0;
                while (true)                //这里是为了让速度稳定不计入工况时间
                {
                    if (temp_flag >= 10)
                        break;
                    if (Math.Abs(CarWait.Speed - 25) <= 1.5)
                        temp_flag++;
                    Thread.Sleep(100);
                }
                GKSJ = 5;
                Speed_Jc_flag = true;                       //开始速度检查
                GKSJ = 0;                                   //工况时间置为0
                Msg(Msg_msg, panel_msg, JC_Circuit + "工况时间：" + GKSJ.ToString() + "S", true);
                //CarWait.CGJ_Jzgl = 10;
                //CarWait.igbt.Set_Control_Power(CarWait.CGJ_Jzgl);         //江新IGBT恒功率加载
                //CarWait.igbt.Start_Control_Power();
                //Msg(Msg_msg, panel_msg, "工况时间：" + GKSJ.ToString() + "S", true);
                //CarWait.mqw_50A.Stop_Check();                   //停止废气分析仪活动   1
                //while (true)                                    //开始测量             1
                //{
                //    string tempmsg=CarWait.mqw_50A.Start();
                //    if (tempmsg == "成功开始测量")
                //        break;
                //    else
                //        if (MessageBox.Show(tempmsg + " 是否重试", "仪器没准备好", MessageBoxButtons.YesNo) == DialogResult.Yes)
                //            continue;
                //        else
                //            this.Close();
                //}
                for (int i = 1; i <= Preset_Time * 10; i++)      //t=5，废气分析仪预置响应时间10秒
                {
                    CarWait.CGJ_Jzgl = 10;
                    Ref_Chart(26.5, "seriesCeiling");
                    Ref_Chart(23.5, "seriesLower");
                    //Speed = Get_Speed();                        //取速度，耗时10毫秒
                    Ref_Chart(CarWait.Speed, "seriesdata");
                    Thread.Sleep(100);
                    if (i % 10 == 0)
                    {
                        GKSJ += 1;
                        Speed_List5025[GKSJ] = CarWait.Speed;
                        Msg(Msg_msg, panel_msg, "时间：" + GKSJ.ToString() + "S" + "  仪器预置时间", true);
                    }
                }
                GKSJ = 15;
                for (int i = 1; i <= 10; i++)                   //t=15,开始取值，持续10秒取值
                {
                    GKSJ += 1;
                    float speed_E = 0;
                    for (int j = 0; j < 10; j++)
                    {
                        CarWait.CGJ_Jzgl = 10;          //加载
                        //Speed = Get_Speed();      //取速度，耗时10毫秒
                        speed_E += CarWait.Speed;
                        Thread.Sleep(100);
                    }
                    Ref_Chart(26.5, "seriesCeiling");
                    Ref_Chart(23.5, "seriesLower");
                    Speed_List5025[GKSJ] = speed_E / 10;
                    Ref_Chart(speed_E / 10f, "seriesdata");
                    if (i >= 2)
                        if (Speed_List5025[16] - Speed_List5025[GKSJ] >= 0.5)
                            Speed_flag = true;
                    //取废气值
                    //Asm_Exhaust_List5025[GKSJ] = CarWait.mqw_50A.GetData();               1
                    Asm_Exhaust_List5025[GKSJ] = CarWait.mqw_50A.Getdata_MQ411();
                    Msg(Msg_msg, panel_msg, "HC:" + Asm_Exhaust_List5025[GKSJ].HC.ToString() + "  CO:" + Asm_Exhaust_List5025[GKSJ].CO.ToString() + "  NO:" + Asm_Exhaust_List5025[GKSJ].NO.ToString(), true);
                }
                GKSJ = 25;
                if (!Speed_flag)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        co += Asm_Exhaust_List5025[15 + i].CO;
                        co2 += Asm_Exhaust_List5025[15 + i].CO2;
                        hc += Asm_Exhaust_List5025[15 + i].HC;
                        no += Asm_Exhaust_List5025[15 + i].NO;
                        if (i == 10)
                        {
                            Asm_Exhaust_List5025[91] = new Exhaust.Exhaust_Data();
                            Asm_Exhaust_List5025[91].CO = (float)co / 10f;
                            Asm_Exhaust_List5025[91].NO = (float)no / 10f;
                            Asm_Exhaust_List5025[91].CO2 = (float)co2 / 10f;
                            Asm_Exhaust_List5025[91].HC = (float)hc / 10f;
                            Asm_Exhaust_List5025[91] = Revise(Asm_Exhaust_List5025[91]);
                            if (Asm_Exhaust_List5025[91].CO <= Limit_CO_5025 / 2)
                                CO_5025_PD = "合格";
                            else
                                CO_5025_PD = "不合格";
                            if (Asm_Exhaust_List5025[91].HC <= Limit_HC_5025 / 2)
                                HC_5025_PD = "合格";
                            else
                                HC_5025_PD = "不合格";
                            if (Asm_Exhaust_List5025[91].NO <= Limit_NO_5025 / 2)
                                NO_5025_PD = "合格";
                            else
                                NO_5025_PD = "不合格";
                            if (CO_5025_PD == "合格" && HC_5025_PD == "合格" && NO_5025_PD == "合格")
                            {
                                ASM_PD = "通过";
                                CO_5025_JG = Math.Round(Asm_Exhaust_List5025[91].CO, 2).ToString();
                                HC_5025_JG = Math.Round(Asm_Exhaust_List5025[91].HC, 0).ToString();
                                NO_5025_JG = Math.Round(Asm_Exhaust_List5025[91].NO, 0).ToString();
                                Jcjg_Status = 1;
                            }
                            else
                            {
                                ASM_PD = "未通过";
                                Jcjg_Status = 0;
                            }
                        }
                    }
                    Msg(Msg_msg, panel_msg, "5025快速工况:" + ASM_PD, true);
                    Thread.Sleep(1000);
                }
                //5025快速工况合格检测结束
                if (Jcjg_Status == 1)
                {
                    foreach (Exhaust.Exhaust_Data s in Asm_Exhaust_List5025)
                    {
                        if (s != null)
                        {
                            asm_exh_co += s.CO.ToString();
                            asm_exh_co += ",";
                            asm_exh_hc += s.HC.ToString();
                            asm_exh_hc += ",";
                            asm_exh_no += s.NO.ToString();
                            asm_exh_no += ",";
                            asm_exh_co2 += s.CO2.ToString();
                            asm_exh_co2 += ",";
                        }
                    }
                    asm.MMCO = asm_exh_co;
                    asm.MMHC = asm_exh_hc;
                    asm.MMNO = asm_exh_no;
                    asm.MMCO2 = asm_exh_co2;
                    asm.ASM25HC_JG = HC_5025_JG;
                    asm.ASM25CO_JG = CO_5025_JG;
                    asm.ASM25NO_JG = NO_5025_JG;
                    asm.ASM40HC_JG = "-";
                    asm.ASM40CO_JG = "-";
                    asm.ASM40NO_JG = "-";
                    asm.ASM25HC_PD = HC_5025_PD;
                    asm.ASM25CO_PD = CO_5025_PD;
                    asm.ASM25NO_PD = NO_5025_PD;
                    asm.ASM40HC_PD = "-";
                    asm.ASM40CO_PD = "-";
                    asm.ASM40NO_PD = "-";
                    asm.ASM_PD = ASM_PD;
                    asm.GKSJ = 25;
                    asm.JCCXSJ = 25;
                    CarWait.asmdal.Save_ASM(asm);
                    Msg(Msg_msg, panel_msg, Jccl.Rows[0]["JCCLPH"].ToString() + "  " + ASM_PD, true);
                    Thread.Sleep(2000);
                    //操作废气分析仪结束检测
                    //CarWait.mqw_50A.Stop();                     //停止检测            1
                    //CarWait.mqw_50A.Blowback();                 //反吹                  1
                    for (int i = 30; i > 0; i--)
                    {
                        Thread.Sleep(1000);
                        Msg(Msg_msg, panel_msg, "反吹 " + i.ToString(), true);
                    }
                    //CarWait.mqw_50A.Stop_Check();               //停止所有检查动作    1
                    while (true)
                    {
                        if (CarWait.Speed < 1)
                            break;
                        Thread.Sleep(100);
                    }
                    CarWait.igbt.Lifter_Up();
                    Msg(Msg_msg, panel_msg, "举升上升", false);
                    CarWait.bjcl.JCZT = "完成 ○";
                    CarWait.bjcl.JCBJ = "0";
                    CarWait.bjclxx.Update(CarWait.bjcl);
                }
                else
                {
                    //继续运行至90s
                    Msg(Msg_msg, panel_msg, "继续5025工况，保持25Km/h", true);
                    Jcjg_Status = 2;                                //先将状态置为5025工况合格
                    GKSJ = 25;
                    for (int i = 1; i <= 65; i++)                   //t=25,开始取值，持续65秒取值
                    {
                        GKSJ += 1;
                        float speed_E = 0;
                        for (int j = 0; j < 10; j++)
                        {
                            CarWait.CGJ_Jzgl = 10;          //加载
                            //Speed = Get_Speed();      //取速度，耗时10毫秒
                            speed_E += CarWait.Speed;
                            Thread.Sleep(100);
                        }
                        Ref_Chart(26.5, "seriesCeiling");
                        Ref_Chart(23.5, "seriesLower");
                        Speed_List5025[GKSJ] = speed_E / 10;
                        Ref_Chart(speed_E / 10, "seriesdata");
                        //取废气值
                        //Asm_Exhaust_List5025[GKSJ] = CarWait.mqw_50A.GetData();               1
                        Asm_Exhaust_List5025[GKSJ] = CarWait.mqw_50A.Getdata_MQ411();
                        Msg(Msg_msg, panel_msg, "HC:" + Asm_Exhaust_List5025[GKSJ].HC.ToString() + "  CO:" + Asm_Exhaust_List5025[GKSJ].CO.ToString() + "  NO:" + Asm_Exhaust_List5025[GKSJ].NO.ToString(), true);
                    }
                    GKSJ = 90;
                    for (int i = 15; i <= 80; i++)   //总共90个数据下面J提供10个下标所以这里是80
                    {
                        if (Jcjg_Status != 2)
                            break;
                        hc = 0;
                        co = 0;
                        no = 0;
                        co2 = 0;
                        for (int j = 1; j <= 10; j++)
                        {
                            hc += Asm_Exhaust_List5025[i + j].HC;
                            co += Asm_Exhaust_List5025[i + j].CO;
                            no += Asm_Exhaust_List5025[i + j].NO;
                            if (j == 9)
                            {
                                hc = hc / 10f;
                                co = co / 10f;
                                no = no / 10f;
                                if (hc <= Limit_HC_5025)
                                    HC_5025_PD = "合格";
                                else
                                    HC_5025_PD = "不合格";
                                if (co <= Limit_CO_5025)
                                    CO_5025_PD = "合格";
                                else
                                    CO_5025_PD = "不合格";
                                if (no <= Limit_NO_5025)
                                    NO_5025_PD = "合格";
                                else
                                    NO_5025_PD = "不合格";
                                if (CO_5025_PD == "合格" && HC_5025_PD == "合格" && NO_5025_PD == "合格")
                                {
                                    ASM_PD = "-";
                                    Jcjg_Status = 2;
                                }
                                else
                                {
                                    HC_5025_JG = Math.Round(hc, 0).ToString();
                                    CO_5025_JG = Math.Round(co, 2).ToString();
                                    NO_5025_JG = Math.Round(no, 0).ToString();
                                    asm.ASM40HC_JG = "-";
                                    asm.ASM40CO_JG = "-";
                                    asm.ASM40NO_JG = "-";
                                    asm.ASM40HC_PD = "-";
                                    asm.ASM40CO_PD = "-";
                                    asm.ASM40NO_PD = "-";
                                    ASM_PD = "未通过";
                                    Jcjg_Status = 0;
                                }
                            }
                        }
                    }
                    if (Jcjg_Status == 2)
                    {
                        int out_asm = 0;
                        hc = 0;
                        co = 0;
                        no = 0;
                        co2 = 0;
                        //先保存之前未修正5025合格数据
                        for (int i = 16; i <= 90; i++)
                        {
                            hc += Asm_Exhaust_List5025[i].HC;
                            co += Asm_Exhaust_List5025[i].CO;
                            no += Asm_Exhaust_List5025[i].NO;
                        }

                        HC_5025_JG = Math.Round((hc / 75f), 0).ToString();
                        CO_5025_JG = Math.Round((co / 75f), 2).ToString();
                        NO_5025_JG = Math.Round((no / 75f), 0).ToString();

                        for (int i = 16; i <= 90; i++)
                        {
                            Asm_Exhaust_Revise_List5025[i] = Revise(Asm_Exhaust_List5025[i]);   //气象修正
                        }

                        for (int i = 16; i <= 90; i++)
                        {
                            //Asm_Exhaust_Revise_List[i] = Revise(Asm_Exhaust_List[i]);   //气象修正
                            if (Asm_Exhaust_Revise_List5025[i].HC > 5 * Limit_HC_5025 || Asm_Exhaust_Revise_List5025[i].CO > 5 * Limit_CO_5025 || Asm_Exhaust_Revise_List5025[i].NO > 5 * Limit_NO_5025)
                                out_asm++;
                            else
                                out_asm = 0;
                            if (out_asm >= 9)
                            {
                                hc = 0;
                                co = 0;
                                no = 0;
                                co2 = 0;
                                //不合格，保存数据退出，这里属于废气经修正后连续十秒超出限值500%，数据怎么保存暂时不清楚
                                if (Asm_Exhaust_Revise_List5025[i].HC > 5 * Limit_HC_5025)
                                    HC_5025_PD = "不合格";
                                else
                                    HC_5025_PD = "合格";
                                if (Asm_Exhaust_Revise_List5025[i].CO > 5 * Limit_CO_5025)
                                    CO_5025_PD = "不合格";
                                else
                                    CO_5025_PD = "合格";
                                if (Asm_Exhaust_Revise_List5025[i].NO > 5 * Limit_NO_5025)
                                    NO_5025_PD = "不合格";
                                else
                                    NO_5025_PD = "合格";
                                ASM_PD = "未通过";
                                HC_5025_JG = Math.Round(Asm_Exhaust_Revise_List5025[i].HC, 0).ToString();
                                CO_5025_JG = Math.Round(Asm_Exhaust_Revise_List5025[i].CO, 2).ToString();
                                NO_5025_JG = Math.Round(Asm_Exhaust_Revise_List5025[i].NO, 0).ToString();
                                asm.ASM40HC_JG = "-";
                                asm.ASM40CO_JG = "-";
                                asm.ASM40NO_JG = "-";
                                asm.ASM40HC_PD = "-";
                                asm.ASM40CO_PD = "-";
                                asm.ASM40NO_PD = "-";
                                Jcjg_Status = 0;
                                break;
                            }
                        }
                        //5025工况最终合格保存数据
                        if (Jcjg_Status == 2)
                        {
                            foreach (Exhaust.Exhaust_Data s in Asm_Exhaust_List5025)
                            {
                                if (s != null)
                                {
                                    asm_exh_co += s.CO.ToString();
                                    asm_exh_co += ",";
                                    asm_exh_hc += s.HC.ToString();
                                    asm_exh_hc += ",";
                                    asm_exh_no += s.NO.ToString();
                                    asm_exh_no += ",";
                                    asm_exh_co2 += s.CO2.ToString();
                                    asm_exh_co2 += ",";
                                }
                            }
                            asm.MMCO = asm_exh_co;
                            asm.MMHC = asm_exh_hc;
                            asm.MMNO = asm_exh_no;
                            asm.MMCO2 = asm_exh_co2;
                            asm.ASM25HC_JG = HC_5025_JG;
                            asm.ASM25CO_JG = CO_5025_JG;
                            asm.ASM25NO_JG = NO_5025_JG;
                            asm.ASM40HC_JG = "-";
                            asm.ASM40CO_JG = "-";
                            asm.ASM40NO_JG = "-";
                            asm.ASM25HC_PD = HC_5025_PD;
                            asm.ASM25CO_PD = CO_5025_PD;
                            asm.ASM25NO_PD = NO_5025_PD;
                            asm.ASM40HC_PD = "-";
                            asm.ASM40CO_PD = "-";
                            asm.ASM40NO_PD = "-";
                            asm.ASM_PD = "-";
                            asm.GKSJ = GKSJ;
                            asm.JCCXSJ = 90;
                            CarWait.asmdal.Save_ASM(asm);
                            Speed_Jc_flag = false;
                            Msg(Msg_msg, panel_msg, Jccl.Rows[0]["JCCLPH"].ToString() + "  继续2540工况", true);
                            Init_Chart2540();
                            Jc_Exe2540();           //继续进行2540
                        }
                    }
                    else
                    //5025工况不合格保存数据退出检测
                    {
                        foreach (Exhaust.Exhaust_Data s in Asm_Exhaust_List5025)
                        {
                            if (s != null)
                            {
                                asm_exh_co += s.CO.ToString();
                                asm_exh_co += ",";
                                asm_exh_hc += s.HC.ToString();
                                asm_exh_hc += ",";
                                asm_exh_no += s.NO.ToString();
                                asm_exh_no += ",";
                                asm_exh_co2 += s.CO2.ToString();
                                asm_exh_co2 += ",";
                            }
                        }
                        asm.MMCO = asm_exh_co;
                        asm.MMHC = asm_exh_hc;
                        asm.MMNO = asm_exh_no;
                        asm.MMCO2 = asm_exh_co2;
                        asm.ASM25HC_JG = HC_5025_JG;
                        asm.ASM25CO_JG = CO_5025_JG;
                        asm.ASM25NO_JG = NO_5025_JG;
                        asm.ASM40HC_JG = "-";
                        asm.ASM40CO_JG = "-";
                        asm.ASM40NO_JG = "-";
                        asm.ASM25HC_PD = HC_5025_PD;
                        asm.ASM25CO_PD = CO_5025_PD;
                        asm.ASM25NO_PD = NO_5025_PD;
                        asm.ASM40HC_PD = "-";
                        asm.ASM40CO_PD = "-";
                        asm.ASM40NO_PD = "-";
                        asm.ASM_PD = ASM_PD;
                        asm.GKSJ = GKSJ;
                        asm.JCCXSJ = 90;
                        CarWait.asmdal.Save_ASM(asm);
                        Speed_Jc_flag = false;
                        Msg(Msg_msg, panel_msg, Jccl.Rows[0]["JCCLPH"].ToString() + " 5025工况:" + ASM_PD + "  检测结束", true);
                        Thread.Sleep(2000);
                        //操作废气分析仪结束检测
                        //CarWait.mqw_50A.Stop();                     //停止检测            1
                        //CarWait.mqw_50A.Blowback();                 //反吹                  1
                        for (int i = 30; i > 0; i--)
                        {
                            Thread.Sleep(1000);
                            Msg(Msg_msg, panel_msg, "反吹 " + i.ToString(), true);
                        }
                        //CarWait.mqw_50A.Stop_Check();               //停止所有检查动作          1
                        while (true)
                        {
                            if (CarWait.Speed < 1)
                                break;
                            Thread.Sleep(100);
                        }
                        CarWait.igbt.Lifter_Up();
                        Msg(Msg_msg, panel_msg, "举升上升", false);
                        CarWait.bjcl.JCZT = "完成 ×";
                        CarWait.bjcl.JCBJ = "0";
                        CarWait.bjclxx.Update(CarWait.bjcl);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region 2540工况
        public void Jc_Exe2540()
        {
            try
            {
                float hc = 0;
                float co = 0;
                float no = 0;
                float co2 = 0;
                string asm_exh_co = "";
                string asm_exh_hc = "";
                string asm_exh_no = "";
                string asm_exh_co2 = "";
                if (Jcjg_Status != 6)
                    Arise = 0;
                //等待速度稳定
                Msg(Msg_msg, panel_msg, Jccl.Rows[0]["JCCLPH"].ToString() + "加速至40Km/h", true);
                CarWait.igbt.Set_Control_Power(10);
                JC_Circuit = "2540";
                Speed_Jc_flag = false;
                int outspeed = 0;
                //当速度在允许范围内并保持1秒才开始2540工况
                while (true)
                {
                    //Speed = Get_Speed();    //取速度耗时10ms
                    Thread.Sleep(100);
                    if (CarWait.Speed <= 41.5 && CarWait.Speed >= 38.5)
                        outspeed++;
                    else
                        outspeed = 0;
                    if (outspeed >= 9)
                        break;
                }

                Speed_Jc_flag = true;   //开始速度检查
                GKSJ = 0;                                 //工况时间置为0
                Msg(Msg_msg, panel_msg, "2540工况时间：" + GKSJ.ToString() + "S", true);
                for (int i = 1; i <= 50; i++)            //t=0,工况开始
                {
                    //Speed = Get_Speed();
                    Ref_Chart(CarWait.Speed, "seriesdata");         //取速度，耗时10毫秒
                    //当车辆的加速度为1.475m/s^2时，以车辆输出功率的50%作为设定功率加载。
                    CarWait.CGJ_Jzgl = 10;
                    Thread.Sleep(100);
                    if (i % 10 == 0)
                    {
                        GKSJ += 1;
                        Speed_List2540[GKSJ] = CarWait.Speed;
                        Msg(Msg_msg, panel_msg, "时间：" + GKSJ.ToString() + "S" + "  保持40Km/h", true);
                    }
                }
                GKSJ = 5;
                for (int i = 1; i <= Preset_Time * 10; i++)      //t=5，废气分析仪预置响应时间10秒
                {
                    CarWait.CGJ_Jzgl = 10;
                    Ref_Chart(41.5, "seriesCeiling");
                    Ref_Chart(38.5, "seriesLower");
                    //Speed = Get_Speed();                        //取速度，耗时10毫秒
                    Ref_Chart(CarWait.Speed, "seriesdata");
                    Thread.Sleep(100);
                    if (i % 10 == 0)
                    {
                        GKSJ += 1;
                        Speed_List2540[GKSJ] = CarWait.Speed;
                        Msg(Msg_msg, panel_msg, "时间：" + GKSJ.ToString() + "S" + "  仪器预置时间", true);
                    }
                }
                GKSJ = 15;
                Msg(Msg_msg, panel_msg, "开始取值", true);
                for (int i = 1; i <= 10; i++)                   //t=15,开始取值，持续10秒取值
                {
                    float speed_E = 0;
                    for (int j = 0; j < 10; j++)
                    {
                        CarWait.CGJ_Jzgl = 10;          //加载
                        //Speed = Get_Speed();      //取速度，耗时10毫秒
                        speed_E += CarWait.Speed;
                        Thread.Sleep(100);
                    }
                    GKSJ += 1;
                    Ref_Chart(41.5, "seriesCeiling");
                    Ref_Chart(38.5, "seriesLower");
                    Speed_List2540[GKSJ] = speed_E / 10;
                    Ref_Chart(speed_E / 10, "seriesdata");
                    if (i >= 2)
                        if (Speed_List2540[16] - Speed_List2540[GKSJ] >= 0.5)
                            Speed_flag = true;
                    //取废气值
                    //Asm_Exhaust_List2540[GKSJ] = CarWait.mqw_50A.GetData();           1
                    Asm_Exhaust_List2540[GKSJ] = CarWait.mqw_50A.Getdata_MQ411();
                    Msg(Msg_msg, panel_msg, "HC:" + Asm_Exhaust_List2540[GKSJ].HC.ToString() + "  CO:" + Asm_Exhaust_List2540[GKSJ].CO.ToString() + "  NO:" + Asm_Exhaust_List2540[GKSJ].NO.ToString(), true);
                }
                if (!Speed_flag)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        co += Asm_Exhaust_List2540[15 + i].CO;
                        co2 += Asm_Exhaust_List2540[15 + i].CO2;
                        hc += Asm_Exhaust_List2540[15 + i].HC;
                        no += Asm_Exhaust_List2540[15 + i].NO;
                        if (i == 10)
                        {
                            Asm_Exhaust_List2540[91] = new Exhaust.Exhaust_Data();
                            Asm_Exhaust_List2540[91].CO = (float)co / 10f;
                            Asm_Exhaust_List2540[91].NO = (float)no / 10f;
                            Asm_Exhaust_List2540[91].CO2 = (float)co2 / 10f;
                            Asm_Exhaust_List2540[91].HC = (float)hc / 10f;
                            Asm_Exhaust_List2540[91] = Revise(Asm_Exhaust_List2540[91]);
                            if (Asm_Exhaust_List2540[91].CO <= Limit_CO_2540 / 2)
                                CO_2540_PD = "合格";
                            else
                                CO_2540_PD = "不合格";
                            if (Asm_Exhaust_List2540[91].HC <= Limit_HC_2540 / 2)
                                HC_2540_PD = "合格";
                            else
                                HC_2540_PD = "不合格";
                            if (Asm_Exhaust_List2540[91].NO <= Limit_NO_2540 / 2)
                                NO_2540_PD = "合格";
                            else
                                NO_2540_PD = "不合格";
                            if (CO_2540_PD == "合格" && HC_2540_PD == "合格" && NO_2540_PD == "合格")
                            {
                                ASM_PD = "通过";
                                CO_2540_JG = Math.Round(Asm_Exhaust_List2540[91].CO, 2).ToString();
                                HC_2540_JG = Math.Round(Asm_Exhaust_List2540[91].HC, 0).ToString();
                                NO_2540_JG = Math.Round(Asm_Exhaust_List2540[91].NO, 0).ToString();
                                Jcjg_Status = 3;
                            }
                            else
                            {
                                ASM_PD = "未通过";
                                Jcjg_Status = 0;
                            }
                        }
                    }
                    Msg(Msg_msg, panel_msg, " 2540快速工况:" + ASM_PD, true);
                }
                //2540快速工况合格
                if (Jcjg_Status == 3)
                {
                    foreach (Exhaust.Exhaust_Data s in Asm_Exhaust_List2540)
                    {
                        if (s != null)
                        {
                            asm_exh_co += s.CO.ToString();
                            asm_exh_co += ",";
                            asm_exh_hc += s.HC.ToString();
                            asm_exh_hc += ",";
                            asm_exh_no += s.NO.ToString();
                            asm_exh_no += ",";
                            asm_exh_co2 += s.CO2.ToString();
                            asm_exh_co2 += ",";
                        }
                    }
                    asm.MMCO += asm_exh_co;
                    asm.MMHC += asm_exh_hc;
                    asm.MMNO += asm_exh_no;
                    asm.MMCO2 += asm_exh_co2;
                    asm.ASM25HC_JG = HC_5025_JG;
                    asm.ASM25CO_JG = CO_5025_JG;
                    asm.ASM25NO_JG = NO_5025_JG;
                    asm.ASM40HC_JG = HC_2540_JG;
                    asm.ASM40CO_JG = CO_2540_JG;
                    asm.ASM40NO_JG = NO_2540_JG;
                    asm.ASM25HC_PD = HC_5025_PD;
                    asm.ASM25CO_PD = CO_5025_PD;
                    asm.ASM25NO_PD = NO_5025_PD;
                    asm.ASM40HC_PD = HC_2540_PD;
                    asm.ASM40CO_PD = CO_2540_PD;
                    asm.ASM40NO_PD = NO_2540_PD;
                    asm.ASM_PD = ASM_PD;
                    asm.GKSJ = 25;
                    asm.JCCXSJ = 115;
                    CarWait.asmdal.Save_ASM(asm);
                    Speed_Jc_flag = false;
                    Msg(Msg_msg, panel_msg, Jccl.Rows[0]["JCCLPH"].ToString() + " 领取检测报告", true);
                    Thread.Sleep(2000);
                    CarWait.igbt.Exit_Control();                //退出控制
                    //操作废气分析仪结束检测
                    //CarWait.mqw_50A.Stop();                     //停止检测
                    //CarWait.mqw_50A.Blowback();                 //反吹
                    for (int i = 30; i > 0; i--)
                    {
                        Thread.Sleep(1000);
                        Msg(Msg_msg, panel_msg, "反吹 " + i.ToString(), true);
                    }
                    //CarWait.mqw_50A.Stop_Check();               //停止所有检查动作
                    while (true)
                    {
                        if (CarWait.Speed < 1)
                            break;
                        Thread.Sleep(100);
                    }
                    CarWait.igbt.Lifter_Up();
                    Msg(Msg_msg, panel_msg, "举升上升", false);
                    CarWait.bjcl.JCZT = "完成 ○";
                    CarWait.bjcl.JCBJ = "0";
                    CarWait.bjclxx.Update(CarWait.bjcl);
                }
                //如果2540快速工况不合格
                else
                {
                    Msg(Msg_msg, panel_msg, "继续2540工况,保持40Km/h", true);
                    //继续运行至90s
                    Jcjg_Status = 4;                                //先将状态置为2540工况合格
                    GKSJ = 25;
                    for (int i = 1; i <= 65; i++)                   //t=25,开始取值，持续65秒取值
                    {
                        float speed_E = 0;
                        for (int j = 0; j < 10; j++)
                        {
                            CarWait.CGJ_Jzgl = 10;          //加载
                            //Speed = Get_Speed();      //取速度，耗时10毫秒
                            speed_E += CarWait.Speed;
                            Thread.Sleep(100);
                        }
                        GKSJ += 1;
                        Ref_Chart(41.5, "seriesCeiling");
                        Ref_Chart(38.5, "seriesLower");
                        Speed_List2540[GKSJ] = speed_E / 10;
                        Ref_Chart(speed_E / 10, "seriesdata");
                        //取废气值
                        Asm_Exhaust_List2540[GKSJ] = CarWait.mqw_50A.Getdata_MQ411();
                        Msg(Msg_msg, panel_msg, "HC:" + Asm_Exhaust_List2540[GKSJ].HC.ToString() + "  CO:" + Asm_Exhaust_List2540[GKSJ].CO.ToString() + "  NO:" + Asm_Exhaust_List2540[GKSJ].NO.ToString(), true);
                    }
                    GKSJ = 90;
                    for (int i = 15; i <= 80; i++)   //总共90个数据下面J提供10个下标所以这里是80
                    {
                        if (Jcjg_Status != 4)
                            break;
                        hc = 0;
                        co = 0;
                        no = 0;
                        co2 = 0;
                        for (int j = 1; j <= 10; j++)
                        {
                            hc += Asm_Exhaust_List2540[i + j].HC;
                            co += Asm_Exhaust_List2540[i + j].CO;
                            no += Asm_Exhaust_List2540[i + j].NO;
                            if (j == 9)
                            {
                                hc = hc / 10;
                                co = co / 10;
                                no = no / 10;
                                if (hc <= Limit_HC_2540)
                                    HC_2540_PD = "合格";
                                else
                                    HC_2540_PD = "不合格";
                                if (co <= Limit_CO_2540)
                                    CO_2540_PD = "合格";
                                else
                                    CO_2540_PD = "不合格";
                                if (no <= Limit_NO_2540)
                                    NO_2540_PD = "合格";
                                else
                                    NO_2540_PD = "不合格";
                                if (CO_2540_PD == "合格" && HC_2540_PD == "合格" && NO_2540_PD == "合格")
                                {
                                    ASM_PD = "通过";
                                    Jcjg_Status = 4;
                                }
                                else
                                {
                                    HC_2540_JG = Math.Round(hc, 0).ToString();
                                    CO_2540_JG = Math.Round(co, 2).ToString();
                                    NO_2540_JG = Math.Round(no, 0).ToString();
                                    //asm.ASM40HC_JG = "-";
                                    //asm.ASM40CO_JG = "-";
                                    //asm.ASM40NO_JG = "-";
                                    //asm.ASM40HC_PD = "-";
                                    //asm.ASM40CO_PD = "-";
                                    //asm.ASM40NO_PD = "-";
                                    ASM_PD = "未通过";
                                    Jcjg_Status = 0;
                                }
                            }
                        }
                    }
                    //2450工况合格继续进行气象修正判定
                    if (Jcjg_Status == 4)
                    {
                        int out_asm = 0;
                        hc = 0;
                        co = 0;
                        no = 0;
                        co2 = 0;
                        //先保存之前未修正2540合格数据
                        for (int i = 16; i <= 90; i++)
                        {
                            hc += Asm_Exhaust_List2540[i].HC;
                            co += Asm_Exhaust_List2540[i].CO;
                            no += Asm_Exhaust_List2540[i].NO;
                        }

                        HC_2540_JG = Math.Round((hc / 75f), 0).ToString();
                        CO_2540_JG = Math.Round((co / 75f), 2).ToString();
                        NO_2540_JG = Math.Round((no / 75f), 0).ToString();

                        for (int i = 16; i <= 90; i++)
                        {
                            Asm_Exhaust_Revise_List2540[i] = Revise(Asm_Exhaust_List2540[i]);   //气象修正
                        }

                        for (int i = 16; i <= 90; i++)
                        {
                            //Asm_Exhaust_Revise_List[i] = Revise(Asm_Exhaust_List[i]);   //气象修正
                            if (Asm_Exhaust_Revise_List2540[i].HC > 5 * Limit_HC_2540 || Asm_Exhaust_Revise_List2540[i].CO > 5 * Limit_CO_2540 || Asm_Exhaust_Revise_List2540[i].NO > 5 * Limit_NO_2540)
                                out_asm++;
                            else
                                out_asm = 0;
                            if (out_asm >= 9)
                            {
                                hc = 0;
                                co = 0;
                                no = 0;
                                co2 = 0;
                                //不合格，保存数据退出，这里属于废气经修正后连续十秒超出限值500%，数据怎么保存暂时不清楚
                                if (Asm_Exhaust_Revise_List2540[i].HC > 5 * Limit_HC_2540)
                                    HC_2540_PD = "不合格";
                                else
                                    HC_2540_PD = "合格";
                                if (Asm_Exhaust_Revise_List2540[i].CO > 5 * Limit_CO_2540)
                                    CO_2540_PD = "不合格";
                                else
                                    CO_2540_PD = "合格";
                                if (Asm_Exhaust_Revise_List2540[i].NO > 5 * Limit_NO_2540)
                                    NO_2540_PD = "不合格";
                                else
                                    NO_2540_PD = "合格";
                                ASM_PD = "未通过";
                                HC_2540_JG = Math.Round(Asm_Exhaust_Revise_List5025[i].HC, 0).ToString();
                                CO_2540_JG = Math.Round(Asm_Exhaust_Revise_List5025[i].CO, 2).ToString();
                                NO_2540_JG = Math.Round(Asm_Exhaust_Revise_List5025[i].NO, 0).ToString();
                                //asm.ASM40HC_JG = "-";
                                //asm.ASM40CO_JG = "-";
                                //asm.ASM40NO_JG = "-";
                                //asm.ASM40HC_PD = "-";
                                //asm.ASM40CO_PD = "-";
                                //asm.ASM40NO_PD = "-";
                                Jcjg_Status = 0;
                                break;
                            }
                        }
                        //2540工况最终合格保存数据
                        if (Jcjg_Status == 4)
                        {
                            foreach (Exhaust.Exhaust_Data s in Asm_Exhaust_List2540)
                            {
                                if (s != null)
                                {
                                    asm_exh_co += s.CO.ToString();
                                    asm_exh_co += ",";
                                    asm_exh_hc += s.HC.ToString();
                                    asm_exh_hc += ",";
                                    asm_exh_no += s.NO.ToString();
                                    asm_exh_no += ",";
                                    asm_exh_co2 += s.CO2.ToString();
                                    asm_exh_co2 += ",";
                                }
                            }
                            asm.MMCO += asm_exh_co;
                            asm.MMHC += asm_exh_hc;
                            asm.MMNO += asm_exh_no;
                            asm.MMCO2 += asm_exh_co2;
                            asm.ASM25HC_JG = HC_5025_JG;
                            asm.ASM25CO_JG = CO_5025_JG;
                            asm.ASM25NO_JG = NO_5025_JG;
                            asm.ASM40HC_JG = HC_2540_JG;
                            asm.ASM40CO_JG = CO_2540_JG;
                            asm.ASM40NO_JG = NO_2540_JG;
                            asm.ASM25HC_PD = HC_5025_PD;
                            asm.ASM25CO_PD = CO_5025_PD;
                            asm.ASM25NO_PD = NO_5025_PD;
                            asm.ASM40HC_PD = HC_2540_PD;
                            asm.ASM40CO_PD = CO_2540_PD;
                            asm.ASM40NO_PD = NO_2540_PD;
                            asm.ASM_PD = ASM_PD;
                            asm.GKSJ = GKSJ;
                            asm.JCCXSJ = 180;
                            CarWait.asmdal.Save_ASM(asm);
                            Speed_Jc_flag = false;
                            Msg(Msg_msg, panel_msg, Jccl.Rows[0]["JCCLPH"].ToString() + "检测结果：" + ASM_PD + " 结束", true);
                            Thread.Sleep(2000);
                            //检测完毕
                            CarWait.igbt.Exit_Control();                //退出控制
                            //操作废气分析仪结束检测
                            //CarWait.mqw_50A.Stop();                     //停止检测
                            //CarWait.mqw_50A.Blowback();                 //反吹
                            for (int i = 30; i > 0; i--)
                            {
                                Thread.Sleep(1000);
                                Msg(Msg_msg, panel_msg, "反吹 " + i.ToString(), true);
                            }
                            //CarWait.mqw_50A.Stop_Check();               //停止所有检查动作
                            while (true)
                            {
                                if (CarWait.Speed < 1)
                                    break;
                                Thread.Sleep(100);
                            }
                            CarWait.igbt.Lifter_Up();
                            Msg(Msg_msg, panel_msg, "举升上升", false);
                            CarWait.bjcl.JCZT = "完成 ○";
                            CarWait.bjcl.JCBJ = "0";
                            CarWait.bjclxx.Update(CarWait.bjcl);
                        }
                    }
                    //2540工况不合格保存数据退出检测
                    else
                    {
                        foreach (Exhaust.Exhaust_Data s in Asm_Exhaust_List5025)
                        {
                            if (s != null)
                            {
                                asm_exh_co += s.CO.ToString();
                                asm_exh_co += ",";
                                asm_exh_hc += s.HC.ToString();
                                asm_exh_hc += ",";
                                asm_exh_no += s.NO.ToString();
                                asm_exh_no += ",";
                                asm_exh_co2 += s.CO2.ToString();
                                asm_exh_co2 += ",";
                            }
                        }
                        asm.MMCO += asm_exh_co;
                        asm.MMHC += asm_exh_hc;
                        asm.MMNO += asm_exh_no;
                        asm.MMCO2 += asm_exh_co2;
                        asm.ASM25HC_JG = HC_5025_JG;
                        asm.ASM25CO_JG = CO_5025_JG;
                        asm.ASM25NO_JG = NO_5025_JG;
                        asm.ASM40HC_JG = HC_2540_JG;
                        asm.ASM40CO_JG = CO_2540_JG;
                        asm.ASM40NO_JG = NO_2540_JG;
                        asm.ASM25HC_PD = HC_5025_PD;
                        asm.ASM25CO_PD = CO_5025_PD;
                        asm.ASM25NO_PD = NO_5025_PD;
                        asm.ASM40HC_PD = HC_2540_PD;
                        asm.ASM40CO_PD = CO_2540_PD;
                        asm.ASM40NO_PD = NO_2540_PD;
                        asm.ASM_PD = ASM_PD;
                        asm.GKSJ = GKSJ;
                        asm.JCCXSJ = 180;
                        CarWait.asmdal.Save_ASM(asm);
                        Speed_Jc_flag = false;
                        Msg(Msg_msg, panel_msg, Jccl.Rows[0]["JCCLPH"].ToString() + "检测结果：" + ASM_PD + " 结束", true);
                        Thread.Sleep(2000);
                        CarWait.igbt.Exit_Control();                //退出控制
                        //操作废气分析仪结束检测
                        //CarWait.mqw_50A.Stop();                     //停止检测
                        //CarWait.mqw_50A.Blowback();                 //反吹
                        for (int i = 30; i > 0; i--)
                        {
                            Thread.Sleep(1000);
                            Msg(Msg_msg, panel_msg, "反吹 " + i.ToString(), true);
                        }
                        //CarWait.mqw_50A.Stop_Check();               //停止所有检查动作
                        while (true)
                        {
                            if (CarWait.Speed < 1)
                                break;
                            Thread.Sleep(100);
                        }
                        CarWait.igbt.Lifter_Up();
                        Msg(Msg_msg, panel_msg, "举升上升", false);
                        CarWait.bjcl.JCZT = "完成 ×";
                        CarWait.bjcl.JCBJ = "0";
                        CarWait.bjclxx.Update(CarWait.bjcl);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        /// <summary>
        /// 气象修正
        /// </summary>
        /// <param name="Exhaust_data"></param>
        /// <returns></returns>
        public Exhaust.Exhaust_Data Revise(Exhaust.Exhaust_Data Exhaust_data)
        {
            int wd=26;                                               //气象修正用的温度
            Exhaust.Exhaust_Data ex_data = new Exhaust.Exhaust_Data();
            if (Exhaust_data == null)
            {
                ex_data.CO2 = 0;
                ex_data.CO = 0;
                ex_data.HC = 0;
                ex_data.NO = 0;
                ex_data.O2 = 0;
                ex_data.SD = 0;
                ex_data.YW = 0;
                ex_data.HJWD = 0;
                ex_data.ZS = 0;
                ex_data.QLYL = 0;
                ex_data.λ = 0;
                ex_data.HJYL = 0;
                return ex_data;
            }
            if (Exhaust_data.CO2 == 0 && Exhaust_data.CO == 0)
            {
                ex_data.CO2 = 0;
                ex_data.CO = 0;
                ex_data.HC = 0;
                ex_data.NO = 0;
                ex_data.O2 = 0;
                ex_data.SD = 0;
                ex_data.YW = 0;
                ex_data.HJWD = 0;
                ex_data.ZS = 0;
                ex_data.QLYL = 0;
                ex_data.λ = 0;
                ex_data.HJYL = 0;
                return ex_data;
            }
            float X=(float)Exhaust_data.CO2/(Exhaust_data.CO2+Exhaust_data.CO);
            float a=1;
            switch(Jccl.Rows[0]["RLLX"].ToString())
            {
                case "汽油":
                    a=4.644f;
                    break;
                case "天然气":
                    a=6.64f;
                    break;
                case "汽油天然气混合":
                    a=6.64f;
                    break;
            }
            float CO2x=(float)(X/(a+1.88*X)*100);
            float DF=CO2x/Exhaust_data.CO2;             //计算稀释系数
            if (DF > 3)                                 //如果稀释系数
                DF = 3;
            wd=(int)Math.Round(decimal.Parse(asm.WD),0);
            if (wd > 30)
                wd = 30;
            float H = (float)(43.478 * float.Parse(asm.SD) * Pd[wd] / (float.Parse(asm.DQY) - Pd[wd] * float.Parse(asm.SD) / 100));
            float kH = (float)(1 / (1 - 0.0047 * (H - 75)));    //湿度校正系数
            ex_data.HC=(int)(Exhaust_data.HC*DF);
            ex_data.CO=(float)(Exhaust_data.CO*DF);
            ex_data.NO = (int)(Exhaust_data.NO * DF * kH);
            ex_data.CO2 = Exhaust_data.CO2;
            ex_data.O2 = Exhaust_data.O2;
            ex_data.SD = Exhaust_data.SD;
            ex_data.YW = Exhaust_data.YW;
            ex_data.HJWD = Exhaust_data.HJWD;
            ex_data.ZS = Exhaust_data.ZS;
            ex_data.QLYL = Exhaust_data.QLYL;
            ex_data.λ = Exhaust_data.λ;
            ex_data.HJYL = Exhaust_data.HJYL;
            return ex_data;
        }

        public float Get_Speed()
        {
            //if (JC_Circuit == "5025")
            //{
            //    Random rd = new Random();
            //    Thread.Sleep(10);
            //    //return rd.Next(235, 265) / 10;
            //    return rd.Next(235, 265) / 10f;
            //}
            //else
            //{
            //    Random rd = new Random();
            //    Thread.Sleep(10);
            //    //return rd.Next(235, 265) / 10;
            //    return rd.Next(385, 415) / 10f;
            //}
            return (float)Convert.ToDouble(CarWait.igbt.Time_Results.Substring(1, 6));
        }

        private void button_ss_Click(object sender, EventArgs e)
        {
            try
            {
                if (JC_Status == false)
                {
                    TH_ST = new Thread(Jc_Exe);
                    TH_Speed_st = new Thread(Speed_Detect);
                    TH_ST.Start();
                    TH_Speed_st.Start();
                    JC_Status = true;
                    button_ss.Text = "停止检测";
                    CarWait.igbt.Lifter_Down();     //举升下降
                }
                else
                {
                    TH_ST.Abort();
                    TH_Speed_st.Abort();
                    JC_Status = false;
                    //this.BeginInvoke(new wt(Ref_Control_label), this.label_msg.Name.ToString(), CarWait.bjcl.JCCLPH.ToString() + " 检测已停止");
                    button_ss.Text = "重新检测";
                }
            }
            catch (Exception)
            {
            }
        }

        private void Asm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                TH_ST.Abort();
                TH_Speed_st.Abort();
                CarWait.igbt.Lifter_Up();       //举升上升
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
        public void Msg(Label Msgowner, Panel Msgfather, string Msgstr,bool Update_DB)
        {
            BeginInvoke(new wtlsb(Msg_Show), Msgowner, Msgstr,Update_DB);
            BeginInvoke(new wtlp(Msg_Position), Msgowner, Msgfather);
        }

        public void Msg_Show(Label Msgowner, string Msgstr,bool Update_DB)
        {
            Msgowner.Text = Msgstr;
            if(Update_DB)
            {
                CarWait.bjcl.JCZT = Msgstr;
                CarWait.bjclxx.Update(CarWait.bjcl);
            }
        }

        public void Msg_Position(Label Msgowner,Panel Msgfather)
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
            BeginInvoke(new wtcs(ref_Control_Text),control,text);
        }

        public void ref_Control_Text(Control control, string text)
        {
            control.Text = text;
        }
        #endregion

        private void button_kztt_Click(object sender, EventArgs e)
        {
            Control_TD kztt = new Control_TD();
            kztt.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                switch (CarWait.UseMK)
                {
                    case "BNTD":
                        switch (CarWait.igbt.Status)
                        {
                            //case "Demarcate":
                            //    switch (comboBox_bdtd.Text)
                            //    {
                            //        case "1":
                            //            Msg(label_ad, panel_ad, igbt.Time_Vol_1, false);
                            //            break;
                            //        case "2":
                            //            Msg(label_ad, panel_ad, igbt.Time_Vol_2, false);
                            //            break;
                            //        case "3":
                            //            Msg(label_ad, panel_ad, igbt.Time_Vol_3, false);
                            //            break;
                            //    }
                            //    break;
                            default:
                                Msg(Msg_cs, panel_cs, CarWait.igbt.Speed.ToString(), false);
                                //Speed = (float)Convert.ToDouble(igbt.Speed);
                                Msg(Msg_nl, panel_nl, CarWait.igbt.Force.ToString(), false);
                                //Force = (float)Convert.ToDouble(igbt.Force);
                                Msg(Msg_gl, panel_gl, CarWait.igbt.Power.ToString(), false);
                                //Power = (float)Convert.ToDouble(igbt.Power);
                                //Msg(Msg_dl, panel_cp, CarWait.igbt.Duty, false);
                                //Duty = (float)Convert.ToDouble(igbt.Duty);
                                Msg(label_msg, panel9, CarWait.igbt.Msg, false);
                                break;
                        }
                        break;
                    case "IGBT":
                        switch (CarWait.igbt.Status)
                        {
                            //case "Demarcate":
                            //    Msg(label_ad, panel_ad, igbt.Speed, false);
                            //    break;
                            default:
                                if (CarWait.igbt.Time_Results.Length == 32)
                                {
                                    Msg(Msg_cs, panel_cs, CarWait.igbt.Speed.ToString(), false);
                                    //Speed = (float)Convert.ToDouble(igbt.Speed);
                                    Msg(Msg_nl, panel_nl, CarWait.igbt.Force.ToString(), false);
                                    //Force = (float)Convert.ToDouble(igbt.Force);
                                    Msg(Msg_gl, panel_gl, CarWait.igbt.Power.ToString(), false);
                                    //Power = (float)Convert.ToDouble(igbt.Power);
                                    //IGBT未返回电流
                                }
                                break;
                        }
                        break;
                }
                Msg(Msg_msg, panel_msg, CarWait.igbt.Msg, false);
            }
            catch (Exception)
            {

            }
        }


    }
}
