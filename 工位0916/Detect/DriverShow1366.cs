using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace vmasDetect
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
            panelData.Parent = panel3;
            pictureBoxTrack.Location = new Point(0, 0);
            pictureBoxRight.Parent = pictureBoxTrack;
            pictureBoxWrong.Parent = pictureBoxTrack;
            pictureBoxRight.Location = new Point(28, 622);
            pictureBoxWrong.Location = new Point(28, 622);
            pictureBoxRight.Visible = true;
            pictureBoxWrong.Visible = false;            
            Msg(labelTs1, panelTs1, VMAS.ts1, false);
            Msg(labelts2, panelTs2, VMAS.ts2, false);
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
            Msg(labelTs1, panelTs1, VMAS.ts1, false);
            Msg(labelts2, panelTs2, VMAS.ts2, false);
            if (VMAS.igbt != null)
            {
                Msg(labelSpeed, panelSpeed, VMAS.igbt.Speed.ToString("0.0"), false);
                if (VMAS.Ig195_status)
                {
                    try
                    {
                        if (VMAS.gongkuangTime >= 195.0f)
                        {
                            Msg(labelGksj, panelGksj, "195.0", false);
                        }
                        else
                        {
                            Msg(labelGksj, panelGksj, VMAS.gongkuangTime.ToString("000.0"), false);
                        }
                        if (VMAS.gongkuangTime < 195.1)
                        {
                            pictureBoxRight.Visible = !VMAS.chaocha;
                            pictureBoxWrong.Visible = VMAS.chaocha;
                            if (VMAS.gongkuangTime <= 22)
                            {
                                pictureBoxRight.Location = new Point((int)(28 + 1105 * VMAS.gongkuangTime / 39), (int)(622 - +34 * VMAS.igbt.Speed / 3));
                                pictureBoxWrong.Location = new Point((int)(28 + 1105 * VMAS.gongkuangTime / 39), (int)(622 - +34 * VMAS.igbt.Speed / 3));
                                pictureBoxTrack.Location = new Point(0, 0);
                            }
                            else
                            {
                                pictureBoxRight.Location = new Point((int)(28 + 1105 * VMAS.gongkuangTime / 39), (int)(622 - +34 * VMAS.igbt.Speed / 3));
                                pictureBoxWrong.Location = new Point((int)(28 + 1105 * VMAS.gongkuangTime / 39), (int)(622 - +34 * VMAS.igbt.Speed / 3));
                                pictureBoxTrack.Location = new Point((int)(-1105 * (VMAS.gongkuangTime-22) / 39), 0);
                            }
                        }
                        Msg(labelLxcc, panelLxcc, VMAS.outTimeContinus.ToString("0.0"), false);
                        Msg(labelLjcc, panelLjcc, VMAS.outTimeTotal.ToString("0.0"), false);
                    }
                    catch
                    { }
                }
            }
            if (VMAS.driverformmin) this.Close();
        }
    }
}
