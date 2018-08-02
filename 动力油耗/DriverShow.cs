using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SarchPMS.Business.Draw;

namespace 动力油耗
{
    public partial class DriverShow : Form
    {
        public delegate void wtlsb(Label Msgowner, string Msgstr, bool Update_DB);                //委托
        public void Msg_Show(Label Msgowner, string Msgstr, bool Update_DB)
        {
            Msgowner.Text = Msgstr;
        }
        public void DriverShow_OnMyChange(int lx, string str)
        {
            switch (lx)
            {
                case 0:

                    BeginInvoke(new wtlsb(Msg_Show), labelTS, str, false);
                    //labelTS.Text = str;
                    break;
                case 1:
                    BeginInvoke(new wtlsb(Msg_Show), labelSD, str, false);
                    //labelSD.Text = str;
                    break;
                case 2:
                    BeginInvoke(new wtlsb(Msg_Show), labelJZL, str, false);
                    //labelJZL.Text = str;
                    break;
                case 3:
                    BeginInvoke(new wtlsb(Msg_Show), labelJZGL, str, false);
                    //labelJZGL.Text = str;
                    break;
                case 4:
                    BeginInvoke(new wtlsb(Msg_Show), labelZs, str, false);
                    //labelYH.Text = str;
                    break;
                case 5:
                    BeginInvoke(new wtlsb(Msg_Show), labelXSLC, str, false);
                    //labelJZGL.Text = str;
                    break;
                case 6:
                    BeginInvoke(new wtlsb(Msg_Show), labelYH, str, false);
                    //labelYH.Text = str;
                    break;
            }
        }
        public DriverShow()
        {
            InitializeComponent();
        }
        private Curve2DMulti curve1 = new Curve2DMulti();

        private void DriverShow_Load(object sender, EventArgs e)
        {
            initCurve();
            timer1.Start();
        }
        private void initCurve()
        {
            curve1.BgColor = Color.Black;
            curve1.TextColor = Color.WhiteSmoke;
            curve1.SliceColor = Color.WhiteSmoke;
            curve1.AxisColor = Color.WhiteSmoke;
            curve1.AxisTextColor = Color.WhiteSmoke;
            curve1.BorderColor = Color.WhiteSmoke;
            curve1.SliceTextColor = Color.WhiteSmoke;
            curve1.Width = pictureBox1.Width;
            curve1.Height = pictureBox1.Height;
            curve1.Title = "速度&扭力曲线";
            curve1.XAxisText = "时间(s)";
            curve1.YAxisText = "速度(km/h)";
            curve1.YAxisText2 = "加载力(N)";
            curve1.Keys = new string[] { "0", "10", "20", "30", "40", "50", "60", "70", "80", "90" };
            curve1.Values = new float[] { 0f };
            curve1.CurveColors = new Color[] { Color.Yellow, Color.Green };
            curve1.YMaxValue = 100;
            curve1.YSliceValue = 10;
            curve1.YSliceValue2 = 100;
            curve1.Fit();
            pictureBox1.Image = curve1.CreateImage();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!dynamicTest.isReloadData)
            {
                curve1.YSliceValue2 = dynamicTest.driveshowForceSliceValue;
                curve1.scale1Value = dynamicTest.curvescalevalue;
                curve1.scale1Color = Color.Red;
                curve1.displayScale1 = dynamicTest.displayCurveScale;
                curve1.Values = dynamicTest.speedlist.ToArray();
                curve1.Values2 = dynamicTest.forcelist.ToArray();
                curve1.Fit();
                pictureBox1.Image = curve1.CreateImage();
            }
        }
    }
}
