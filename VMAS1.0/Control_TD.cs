using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Threading;
using System.StubHelpers;
using System.Windows.Forms.DataVisualization.Charting;
using System.Text.RegularExpressions;
using Dynamometer;

namespace Detect
{
    public partial class Control_TD : Form
    {

        public delegate void wtls(Label Msgowner, string Msgstr);                               //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                             //委托
        public delegate void wtcs(Control controlname, string text);                            //委托

        public Control_TD()
        {
            InitializeComponent();
        }

        private void Control_TD_Load(object sender, EventArgs e)
        {
            timer1.Interval = 100;
            timer1.Start();
        }
         
        #region 信息显示
        public void Msg(Label Msgowner, Panel Msgfather, string Msgstr)
        {
            BeginInvoke(new wtls(Msg_Show), Msgowner, Msgstr);
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

        public void Ref_Control_Text(Control control, string text)
        {
            BeginInvoke(new wtcs(ref_Control_Text), control, text);
        }

        public void ref_Control_Text(Control control, string text)
        {
            control.Text = text;
        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                Msg(Msg_cs, panel_cs, Daily.Main.igbt.Speed);
                Msg(Msg_nl, panel_nl, Daily.Main.igbt.Force);
                Msg(Msg_gl, panel_gl, Daily.Main.igbt.Power);
                Msg(Msg_jsd, panel_jsd, Daily.Main.igbt.Acceleration);
                Msg(Msg_xsjl, panel_xsjl, Daily.Main.igbt.Range);
                Msg(Msg_msg, panel_msg, Daily.Main.igbt.Msg);
            }
            catch (Exception)
            {
                
            }
        }

        private void button_jsss_Click(object sender, EventArgs e)
        {
            Daily.Main.igbt.Exit_Control();
            Daily.Main.igbt.Lifter_Up();
        }

        private void button_jsxj_Click(object sender, EventArgs e)
        {
            Daily.Main.igbt.Exit_Control();
            Daily.Main.igbt.Lifter_Down();
        }

        private void button_qddj_Click(object sender, EventArgs e)
        {
            Daily.Main.igbt.Exit_Control();
            Daily.Main.igbt.Motor_Open();
        }

        private void button_gbdj_Click(object sender, EventArgs e)
        {
            Daily.Main.igbt.Exit_Control();
            Daily.Main.igbt.Motor_Close();
        }

        private void button_njql_Click(object sender, EventArgs e)
        {
            Daily.Main.igbt.Exit_Control();
            Daily.Main.igbt.Force_Zeroing();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
                label_dw.Text = "KW";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
                label_dw.Text = "N";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
                label_dw.Text = "KM/H";
        }

        private void button_ks_Click(object sender, EventArgs e)
        {
            try
            {
                float.Parse(textBox_jzl.Text.Trim());
                if (radioButton1.Checked == true)
                {
                    Daily.Main.igbt.Exit_Control();
                    Daily.Main.igbt.Set_Control_Power(float.Parse(textBox_jzl.Text.Trim()));
                    Daily.Main.igbt.Start_Control_Power();
                }
                else if (radioButton2.Checked == true)
                {
                    Daily.Main.igbt.Exit_Control();
                    Daily.Main.igbt.Set_Control_Force(int.Parse(textBox_jzl.Text.Trim()));
                    Daily.Main.igbt.Start_Control_Force();
                }
                else if (radioButton3.Checked == true)
                {
                    Daily.Main.igbt.Exit_Control();
                    Daily.Main.igbt.Set_Speed(float.Parse(textBox_jzl.Text.Trim()));
                    Daily.Main.igbt.Start_Control_Speed();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("输入的加载量有误。", "系统提示");
            }
        }

        private void button_tz_Click(object sender, EventArgs e)
        {
            Daily.Main.igbt.Exit_Control();
        }

        private void button_qclc_Click(object sender, EventArgs e)
        {
            Daily.Main.igbt.Exit_Control();
            Daily.Main.igbt.Range_Zeroing();
        }

        private void textBox_jzl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox_jzl.Text.Length < 12)
            {
                bool IsDonetContraint = this.textBox_jzl.Text.Contains(".");
                if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
                {
                    e.Handled = true;
                }
                else if (IsDonetContraint && (e.KeyChar == 46))
                {
                    e.Handled = true;
                }
            }
            else
            {
                MessageBox.Show("你输入的数字位数过长，请重新输入", "系统提示");
                textBox_jzl.Text = "";
                return;
            }
        }
    }
}
