using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace sds
{
    public partial class DriverShow1366 : Form
    {
        
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
            if(Sds.gds==2500)
                panelTrack.BackgroundImage = Image.FromFile(Application.StartupPath + "\\imgtrack\\datarack_sds2500.png");
            else
                panelTrack.BackgroundImage = Image.FromFile(Application.StartupPath + "\\imgtrack\\datarack_sds1800.png");
            pictureBoxTrue.Parent = panelTrack;
            Msg(labelSpeed, panelSpeed, "0.0", false);
            pictureBoxTrue.Location = new Point(74, 30);
            pictureBoxFault.Location = new Point(74, 30);
            pictureBoxFault.Visible = true;
            pictureBoxTrue.Visible = false;
            Msg(labelTs1, panelts1, Sds.ts1, false);
            Msg(labelts2, panelTs2, Sds.ts2, false);
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
            Msg(labelTs1, panelts1,Sds.ts1, false);
            Msg(labelts2, panelTs2, Sds.ts2, false);
            if (Sds.sds_status)
            {
                int zs=(int)Sds.Zs;
                Msg(labelSpeed, panelSpeed, Sds.Zs.ToString("0"), false);
                if (Sds.sxnb == 1 || Sds.sxnb == 2)
                {
                    if ((zs >= Sds.gds - 100) && (zs <= Sds.gds + 100))
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
                else if (Sds.sxnb == 0)
                {
                    if (zs >= 3500)
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
                    pictureBoxFault.Visible = false;
                    pictureBoxTrue.Visible = true;
                }
                if (zs >= 500 && zs <= 3500)
                {
                    pictureBoxTrue.Location = new Point(74 + (int)((zs - 500) * 0.4), 30);
                    pictureBoxFault.Location = new Point(74 + (int)((zs - 500) * 0.4), 30);
                }
                else if (zs < 500)
                {
                    pictureBoxTrue.Location = new Point(74 , 30);
                    pictureBoxFault.Location = new Point(74, 30);
                }
                else
                {
                    pictureBoxTrue.Location = new Point(1274, 30);
                    pictureBoxFault.Location = new Point(1274, 30);
                }
            }
            if (Sds.driverformmin) this.Close();
        }
    }
}
