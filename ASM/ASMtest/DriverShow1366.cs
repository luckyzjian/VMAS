using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASMtest
{
    public partial class DriverShow1366 : Form
    {
        private bool asm5025 = false;
        private bool asm2540 = false;
        public delegate void wtlsb(Label Msgowner, string Msgstr, bool Update_DB);                //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                              //委托
        public DriverShow1366()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            pictureBoxTrue.Parent = panelTrack;
            Msg(labelSpeed, panelSpeed, "0.0", false);
            pictureBoxTrue.Location = new Point(83, 25);
            pictureBoxFault.Location = new Point(83, 25);
            pictureBoxFault.Visible = true;
            pictureBoxTrue.Visible = false;
            Msg(labelTs1, panelts1, ASM.ts1, false);
            Msg(labelts2, panelTs2, ASM.ts2, false);
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
            Msg(labelTs1, panelts1,ASM.ts1, false);
            Msg(labelts2, panelTs2, ASM.ts2, false);
            if (ASM.igbt != null)
            {
                float speedNow=ASM.igbt.Speed;
                Msg(labelSpeed, panelSpeed, speedNow.ToString("0.0"), false);
                if (ASM.asm5025)
                {
                    if (!asm5025)
                    {
                        asm5025 = true;
                        panelTrack.BackgroundImage = Image.FromFile(Application.StartupPath + "\\imgtrack\\datarack透明25.png");

                    }
                    int x = (int)((speedNow - 25) * 200 + 683);
                    if (x < 83) x = 83;
                    if (x > 1283) x = 1283;
                    pictureBoxFault.Location = new Point(x, 25);
                    pictureBoxTrue.Location = new Point(x, 25);
                    if (speedNow >= 23.5 && speedNow <= 26.5)
                    {
                        pictureBoxFault.Visible = false;
                        pictureBoxTrue.Visible = true;
                    }
                    else
                    {
                        pictureBoxTrue.Visible = false;
                        pictureBoxFault.Visible = true;
                    }
                }
                else
                {
                    asm5025 = false;
                }
                if (ASM.asm2540)
                {
                    if (!asm2540)
                    {
                        asm2540 = true;
                        panelTrack.BackgroundImage = Image.FromFile(Application.StartupPath + "\\imgtrack\\datarack透明40.png");
                    }
                    int x = (int)((speedNow - 40) * 200 + 683);
                    if (x < 83) x = 83;
                    if (x > 1283) x = 1283;
                    pictureBoxFault.Location = new Point(x, 25);
                    pictureBoxTrue.Location = new Point(x, 25);
                    if (speedNow >= 38.5 && speedNow <= 41.5)
                    {
                        pictureBoxFault.Visible = false;
                        pictureBoxTrue.Visible = true;
                    }
                    else
                    {
                        pictureBoxTrue.Visible = false;
                        pictureBoxFault.Visible = true;
                    }
                }
                else
                {
                    asm2540 = false;
                }
            }
            if (ASM.driverformmin) this.Close();
        }
    }
}
