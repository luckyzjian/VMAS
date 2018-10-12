using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zyjsTest
{
    public partial class DriverShow : Form
    {
        private bool asm5025 = false;
        private bool asm2540 = false;
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

            Msg(labelTs1, panelts1, Zyjs_Btg.ts1, false);
            Msg(labelts2, panelTs2, Zyjs_Btg.ts2, false);
            if(Zyjs_Btg.equipconfig.useJHSCREEN)
            {
                panel9.Visible = false;
                panel10.Visible = false;
                panel11.Visible = false;
                panel12.Visible = false;

                paneldata1.Visible = false;
                paneldata2.Visible = false;
                paneldata3.Visible = false;
                paneldata4.Visible = false;
            }
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            Msg(labelTs1, panelts1, Zyjs_Btg.ts1, false);
            Msg(labelts2, panelTs2, Zyjs_Btg.ts2, false);
            Msg(labelSpeed, panelSpeed, Zyjs_Btg.ZS.ToString("0"), false);
            Msg(labeldata1, paneldata1, Zyjs_Btg.data1, false);
            Msg(labeldata2, paneldata2, Zyjs_Btg.data2, false);
            Msg(labeldata3, paneldata3, Zyjs_Btg.data3, false);
            Msg(labeldata4, paneldata4, Zyjs_Btg.data4, false);            
        }
    }
}
