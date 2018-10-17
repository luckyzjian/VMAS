using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace lugdowm
{
    public partial class DriverShow : Form
    {
        private double timerCount = 0;
        public delegate void wtlsb(Label Msgowner, string Msgstr, bool Update_DB);                //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                              //委托
        public DriverShow()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            if(Jzjs.equipconfig.useJHSCREEN)
            {
                panel5.Visible = false;
                panel6.Visible = false;
            }
            Msg(labelTs1, panelts1, Jzjs.ts1, false);
            Msg(labelts2, panelTs2, Jzjs.ts2, false);
            Init_Chart();
            chart1.Series["CS"].Points.AddXY(0, 0);
            chart2.Series["XSGL"].Points.AddXY(0, 0);
            timer1.Start();
            
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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control &&e.KeyCode == Keys.C)
            {
                this.Close();
            }
        }
        public void Init_Chart()
        {
            try
            {
                chart2.Series["XSGL"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;//吸收功率
                chart2.Series["GXSXS"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;//光吸收系数
                chart1.Series["FDJZS"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;//发动机转速
                chart1.Series["CS"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;//车速
                //chart1.ChartAreas["ChartArea1"].AxisX.Interval = 100000;
                chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 100;
                chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0;
                chart1.ChartAreas["ChartArea1"].AxisY2.Maximum = 5000;
                chart1.ChartAreas["ChartArea1"].AxisY2.Minimum = 0;
                chart1.ChartAreas["ChartArea1"].AxisY.Title = "速度(km/h)&转速(r/min)";

                chart2.ChartAreas["ChartArea1"].AxisY.Maximum = 100;
                chart2.ChartAreas["ChartArea1"].AxisY.Minimum = 0;
                chart2.ChartAreas["ChartArea1"].AxisY2.Maximum = 10;
                chart2.ChartAreas["ChartArea1"].AxisY2.Minimum = 0;
                chart2.ChartAreas["ChartArea1"].AxisY.Title = "功率(kW)&K值(/m)";
                //chart1.ChartAreas["ChartArea1"].AxisY.Interval = 10;
                chart1.ChartAreas["ChartArea1"].AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
                chart2.ChartAreas["ChartArea1"].AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
                //chart1.ChartAreas["ChartArea1"].AxisY2.Maximum = 10000;
                //chart1.ChartAreas["ChartArea1"].AxisY2.Minimum = 0;
                //chart1.ChartAreas["ChartArea1"].AxisY2.Title = "转速";
                //chart1.ChartAreas["ChartArea1"].AxisY2.Interval = 1000;
            }
            catch
            {
            }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Msg(labelTs1, panelts1,Jzjs.ts1, false);
            Msg(labelts2, panelTs2, Jzjs.ts2, false);
            Msg(labelZS, panelZS, Jzjs.ZS.ToString("0"), false);
            if (Jzjs.igbt != null)
            {
                float speedNow=Jzjs.igbt.Speed;
                float powerNow =Jzjs.igbt.Power;
                Msg(labelSpeed, panelSpeed, speedNow.ToString("0.0"), false);
                Msg(labelPower, panelPower, powerNow.ToString("0.0"), false);
                Msg(labelK, panelK, Jzjs.Smoke.ToString("0.00"), false);
                if (Jzjs.Jzjs_status)
                {
                    if (timerCount > 8)
                    {

                        chart1.Series["CS"].Points.AddY( speedNow);
                        chart1.Series["CS"].Points.RemoveAt(0);
                        if (!Jzjs.equipconfig.useJHSCREEN)
                        {
                            chart1.Series["FDJZS"].Points.AddY(Jzjs.ZS);
                            chart2.Series["XSGL"].Points.AddY(powerNow);
                            chart2.Series["GXSXS"].Points.AddY(Jzjs.Smoke);
                            chart1.Series["FDJZS"].Points.RemoveAt(0);
                            chart2.Series["XSGL"].Points.RemoveAt(0);
                            chart2.Series["GXSXS"].Points.RemoveAt(0);
                        }
                    }
                    else
                    {
                        chart1.Series["CS"].Points.AddY( speedNow);
                        if (!Jzjs.equipconfig.useJHSCREEN)
                        {
                            chart1.Series["FDJZS"].Points.AddY(Jzjs.ZS);
                            chart2.Series["XSGL"].Points.AddY(powerNow);
                            chart2.Series["GXSXS"].Points.AddY(Jzjs.Smoke);
                        }
                        timerCount = timerCount + 0.1;
                    }
                }
            }
            /*if (true)
            {
                if (true)
                {
                    if (timerCount > 8)
                    {
                        chart1.Series["CS"].Points.AddY(timerCount * 10);
                        chart1.Series["FDJZS"].Points.AddY(timerCount * 500);
                        chart2.Series["XSGL"].Points.AddY(timerCount * 10);
                        chart2.Series["GXSXS"].Points.AddY(timerCount);
                        chart1.Series["CS"].Points.RemoveAt(0);
                        chart1.Series["FDJZS"].Points.RemoveAt(0);
                        chart2.Series["XSGL"].Points.RemoveAt(0);
                        chart2.Series["GXSXS"].Points.RemoveAt(0);
                        timerCount=timerCount+0.1;
                        
                    }
                    else
                    {
                        chart1.Series["CS"].Points.AddY( timerCount*10);
                        chart1.Series["FDJZS"].Points.AddY( timerCount*500);
                        chart2.Series["XSGL"].Points.AddY( timerCount*10);
                        chart2.Series["GXSXS"].Points.AddY( timerCount);
                        timerCount = timerCount + 0.1;
                    }
                }
            }*/
        }
    }
}
