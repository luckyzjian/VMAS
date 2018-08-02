using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 动力性测量
{
    public partial class 修正曲线示意图 : Form
    {
        public int lx = 0;
        public 修正曲线示意图()
        {
            InitializeComponent();
        }
        private double[,] 水蒸气分压 =
        {
            { 0.3,0.2,0.2,0.1,0.1 },
            { 0.4,0.3,0.2,0.2,0.1} ,
            { 0.6,0.5,0.4,0.2,0.1} ,
            { 0.9,0.7,0.5,0.4,0.2} ,

            { 1.2,1.0,0.7,0.5,0.2} ,
            { 1.7,1.4,1.0,0.7,0.5} ,
            { 2.3,1.9,1.4,0.9,0.5} ,
            { 3.2,2.5,1.9,1.3,0.6} ,

            { 3.6,2.9,2.1,1.4,0.7} ,
            { 4.2,3.4,2.5,1.7,0.9} ,
            { 4.8,3.8,2.9,1.9,1.0} ,
            { 5.3,4.3,3.2,2.1,1.1} ,

            { 6.0,4.8,3.6,2.6,1.2} ,
            { 6.6,5.3,4.0,2.7,1.3} ,
            { 7.4,5.9,4.4,3.0,1.5} ,
            { 8.2,6.6,4.9,3.3,1.6} ,

            { 9.1,7.3,5.5,3.6,1.8} ,
            { 10.1,8.1,6.1,4.0,2.0} ,
            { 11.2,8.9,6.7,4.5,2.2} ,
            { 12.3,9.9,7.4,4.9,2.5} ,
        };
        private List<double> templist = new List<double> { -10, -5, 0, 5, 10, 15, 20, 25, 27, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48, 50 };
        private List<double> humilist = new List<double> { 100, 80, 60, 40, 20 };
        private int GetRow(double temp)
        {
            if (temp <= templist[0]) return 0;
            else if (temp >= templist[templist.Count - 1]) return templist.Count - 1;
            else
            {
                for (int i = 0; i < templist.Count - 1; i++)
                {
                    if ((temp - templist[i]) * (temp - templist[i + 1]) <= 0)
                    {
                        if (Math.Abs(temp - templist[i]) < Math.Abs(temp - templist[i + 1]))
                            return i;
                        else
                            return i + 1;
                    }
                }
                return templist.Count - 1;
            }
        }
        private int GetColumn(double humidity)
        {
            if (humidity >= humilist[0]) return 0;
            else if (humidity <= humilist[humilist.Count - 1]) return humilist.Count - 1;
            else
            {
                for (int i = 0; i < humilist.Count - 1; i++)
                {
                    if ((humidity - humilist[i]) * (humidity - humilist[i + 1]) <= 0)
                    {
                        if (Math.Abs(humidity - humilist[i]) < Math.Abs(humidity - humilist[i + 1]))
                            return i;
                        else
                            return i + 1;
                    }
                }
                return humilist.Count - 1;
            }
        }
        private double fa = 0, fm = 0.3;//大气因子

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                double sd = 0.3;
                double dqy = 99;
                double wd = 25;
                List<double> αdwdlist = new List<double>();
                List<double> wdlist = new List<double>();
                for (wd = -10; wd <= 50; wd = wd + 2)
                {
                    double αd;
                    if(lx==0) αd = caculateαd(wd, sd, dqy);
                    else αd = caculateαa(wd, sd, dqy);
                    wdlist.Add(wd);
                    αdwdlist.Add(αd);
                }
                chart1.Series[0].Points.Clear();
                chart1.ChartAreas[0].AxisX.Title = "温度/℃";
                chart1.ChartAreas[0].AxisX.Interval = 2;
                chart1.ChartAreas[0].AxisY.Interval = 0.02;
                chart1.ChartAreas[0].AxisY.Title = "修正值";
                chart1.ChartAreas[0].AxisY.Minimum = 0.8;
                chart1.ChartAreas[0].AxisY.Maximum = 1.2;
                string label = "";
                for (int i = 0; i < wdlist.Count; i++)
                {
                    System.Windows.Forms.DataVisualization.Charting.DataPoint jsg_point = null;
                    jsg_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(wdlist[i], αdwdlist[i]);
                    chart1.Series[0].Points.Add(jsg_point);
                    label += wdlist[i].ToString("0.0") + "℃—" + αdwdlist[i].ToString("0.00")+"\r\n";
                }
                chart1.Series[0].Name = label;
            }
        }

        /// <summary>
        /// 柴油机校正系数计算
        /// </summary>
        private double caculateαd(double wd, double sd, double dqy)
        {
            int rownumber = GetRow(wd);
            int columnnumber = GetColumn(sd);
            φPaw = 水蒸气分压[rownumber, columnnumber];
            pa = dqy - φPaw;
            fa = (99.0 / pa) * Math.Pow((wd + 273.15) / 298, 0.7);
            return  Math.Round(Math.Pow(fa, fm),3);
        }
        /// <summary>
        /// 汽油机校正系数计算
        /// </summary>
        private double pa = 0, φPaw = 0;

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                double sd = 0.3;
                double dqy = 99;
                double wd = 25;
                List<double> αdwdlist = new List<double>();
                List<double> wdlist = new List<double>();
                for (dqy = 80; dqy <= 110; dqy = dqy + 2)
                {

                    double αd;
                    if (lx == 0) αd = caculateαd(wd, sd, dqy);
                    else αd = caculateαa(wd, sd, dqy);
                    wdlist.Add(dqy);
                    αdwdlist.Add(αd);
                }
                chart1.Series[0].Points.Clear();
                chart1.ChartAreas[0].AxisX.Title = "大气压/kPa";
                chart1.ChartAreas[0].AxisX.Interval = 2;
                chart1.ChartAreas[0].AxisY.Interval = 0.02;
                chart1.ChartAreas[0].AxisY.Title = "修正值";
                chart1.ChartAreas[0].AxisY.Minimum = 0.8;
                chart1.ChartAreas[0].AxisY.Maximum = 1.4;
                string label = "";
                for (int i = 0; i < wdlist.Count; i++)
                {
                    System.Windows.Forms.DataVisualization.Charting.DataPoint jsg_point = null;
                    jsg_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(wdlist[i], αdwdlist[i]);
                    chart1.Series[0].Points.Add(jsg_point);
                    label += wdlist[i].ToString("0.0") + "kPa—" + αdwdlist[i].ToString("0.00") + "\r\n";
                }
                chart1.Series[0].Name = label;
            }
        }

        private void 修正曲线示意图_Load(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                double sd = 0.3;
                double dqy = 99;
                double wd = 25;
                List<double> αdwdlist = new List<double>();
                List<double> wdlist = new List<double>();
                for (sd = 0; sd <= 100; sd = sd + 4)
                {
                    double αd;
                    if (lx == 0) αd = caculateαd(wd, sd, dqy);
                    else αd = caculateαa(wd, sd, dqy);
                    wdlist.Add(sd);
                    αdwdlist.Add(αd);
                }
                chart1.Series[0].Points.Clear();
                chart1.ChartAreas[0].AxisX.Title = "湿度/%";
                chart1.ChartAreas[0].AxisX.Interval = 2;
                chart1.ChartAreas[0].AxisY.Interval = 0.02;
                chart1.ChartAreas[0].AxisY.Minimum = 0.8;
                chart1.ChartAreas[0].AxisY.Maximum = 1.2;
                chart1.ChartAreas[0].AxisY.Title = "修正值";
                string label = "";
                for (int i = 0; i < wdlist.Count; i++)
                {
                    System.Windows.Forms.DataVisualization.Charting.DataPoint jsg_point = null;
                    jsg_point = new System.Windows.Forms.DataVisualization.Charting.DataPoint(wdlist[i], αdwdlist[i]);
                    chart1.Series[0].Points.Add(jsg_point);
                    label += wdlist[i].ToString("0.0") + "%—" + αdwdlist[i].ToString("0.00") + "\r\n";
                }
                chart1.Series[0].Name = label;
            }

        }

        private double caculateαa(double wd, double sd, double dqy)
        {
            int rownumber = GetRow(wd);
            int columnnumber = GetColumn(sd);
            φPaw = 水蒸气分压[rownumber, columnnumber];
            pa = dqy - φPaw;
            return Math.Round(Math.Pow(99.0 / pa, 1.2) * Math.Pow((wd + 273.15) / 298, 0.6), 3);
        }
    }
}
