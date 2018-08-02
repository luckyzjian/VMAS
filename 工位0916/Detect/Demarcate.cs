using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;

namespace Detect
{
    public partial class Demarcate : Form
    {
        MatchEvaluator me = delegate(Match m)
        {
            return "\n";
        };
        public delegate void wtls(Label Msgowner, string Msgstr);                           //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                         //委托
        public bool demarcate_force = false;
        public DataTable dt_bd = null;                                                      //标定时用的表

        public Demarcate()
        {
            InitializeComponent();
        }

        private void button_demarcate_force_Click(object sender, EventArgs e)
        {
            string errorlist="";
            if(textBox_Weight.Text.Trim()=="")
                errorlist+="请输入法码质量 ";
            if(textBox_modulus.Text.Trim()=="")
                errorlist+="请输入力臂比 ";
            if (errorlist == "")
            {
                try
                {
                    float.Parse(textBox_Weight.Text.Trim());
                }
                catch (Exception)
                {
                    errorlist += "输入的法码质量不合法 ";
                }
                try
                {
                    float.Parse(textBox_modulus.Text.Trim());
                }
                catch (Exception)
                {
                    errorlist += "输入的力臂比不合法 ";
                }
            }
            if (errorlist == "")
            {
                CarWait.igbt.Demarcate_Force(comboBox_point.SelectedIndex, int.Parse((float.Parse(textBox_Weight.Text.Trim()) * float.Parse(textBox_modulus.Text.Trim()) * 9.8f).ToString()));
                if (comboBox_point.SelectedIndex<comboBox_point.Items.Count-1)
                    comboBox_point.SelectedIndex += 1;
            }
            else
                MessageBox.Show(errorlist, "出错啦");
        }

        private void button_demarcate_speed_Click(object sender, EventArgs e)
        {
            string errorlist = "";
            if (textBox_diameter.Text.Trim() == "")
                errorlist += "请输入直径 ";
            if (textBox_pulse.Text.Trim() == "")
                errorlist += "请输入脉冲数 ";
            if (errorlist == "")
            {
                try
                {
                    int.Parse(textBox_diameter.Text.Trim());
                }
                catch (Exception)
                {
                    errorlist += "输入的直径不合法 ";
                }
                try
                {
                    int.Parse(textBox_pulse.Text.Trim());
                }
                catch (Exception)
                {
                    errorlist += "输入的脉冲数不合法 ";
                }
            }
            if (errorlist == "")
            {
                //igbt.Exit_Demarcate_Force();
                //Thread.Sleep(500);
                //igbt.Exit_Control();
                //Thread.Sleep(500);
                switch (CarWait.UseMK)
                {
                    case "BNTD":
                        CarWait.igbt.Demarcate_Speed((float.Parse(textBox_diameter.Text.Trim()) / 10).ToString(), textBox_pulse.Text.Trim());
                        break;
                    case "IGBT":
                        CarWait.igbt.Demarcate_Speed(textBox_diameter.Text.Trim(), textBox_pulse.Text.Trim());
                        break;
                }
            }
            else
                MessageBox.Show(errorlist, "出错啦");
        }

        private void Demarcate_Load(object sender, EventArgs e)
        {
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            switch (CarWait.UseMK)
            {
                case "BNTD":
                    button_read_speed_dp.Enabled = true;
                    label3.Text = "电压值";
                    panel14.Enabled = true;
                    panel3.Enabled = false;
                    dt_bd = null;
                    dt_bd = new DataTable();
                    dt_bd.Columns.Add("电压值");
                    dt_bd.Columns.Add("标定点");
                    dataGridView_bd.DataSource = dt_bd;
                    comboBox_bdtd.SelectedIndex = 0;
                    ini.INIIO.GetPrivateProfileString("UseMK", "使用通道", "1", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                    switch (temp.ToString())
                    {
                        case "1":
                            radioButton_td1.Checked = true;
                            break;
                        case "2":
                            radioButton_td2.Checked = true;
                            break;
                        case "3":
                            radioButton_td3.Checked = true;
                            break;
                    }
                    ini.INIIO.GetPrivateProfileString("UseMK", "BNTD标定说明", "无", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                    richTextBox_czts.Text = Regex.Replace(temp.ToString(), @"\\n", me);
                    CarWait.igbt.Exit_Control();
                    Thread.Sleep(5);
                    CarWait.igbt.Start_Demarcate_Force();
                    break;
                case "IGBT":
                    ini.INIIO.GetPrivateProfileString("UseMK", "IGBT标定说明", "无", temp, 2048, @".\Config.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                    richTextBox_czts.Text = Regex.Replace(temp.ToString(), @"\\n", me);
                    panel14.Enabled = false;
                    panel3.Enabled = true;
                    button_read_speed_dp.Enabled = false;
                    CarWait.igbt.Exit_Control();
                    break;
            }
            comboBox_point.Text = "第一点";
            timer1.Interval = 100;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (demarcate_force)
                    Msg(label_ad, panel_ad, int.Parse(CarWait.igbt.Read_Buffer.ToString().Substring(3, 5)).ToString());
                else
                {
                    Msg(Msg_cs, panel_cs, CarWait.igbt.Speed.ToString());
                    Msg(Msg_nl, panel_nl, CarWait.igbt.Force.ToString());
                }
                Msg(Msg_msg, panel_msg, CarWait.igbt.Msg);
            }
            catch (Exception)
            {
            }
        }

        #region 信息显示
        public void Msg(Label Msgowner, Panel Msgfather, string Msgstr)
        {
            Invoke(new wtls(Msg_Show), Msgowner, Msgstr);
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
        #endregion

        private void button_start_demarcate_Click(object sender, EventArgs e)
        {
            CarWait.igbt.Exit_Control();
            CarWait.igbt.Start_Demarcate_Force();
            demarcate_force = true;
        }

        private void button_ttkz_Click(object sender, EventArgs e)
        {
            Control_TD kztt = new Control_TD();
            kztt.ShowDialog();
        }

        private void button_tcbd_Click(object sender, EventArgs e)
        {
            CarWait.igbt.Exit_Demarcate_Force();
            CarWait.igbt.Exit_Control();
            demarcate_force = false;
        }

        private void button_njql_Click(object sender, EventArgs e)
        {
            CarWait.igbt.Exit_Control();
            CarWait.igbt.Force_Zeroing();
        }

        private void comboBox_point_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_point.SelectedIndex == 0)
            {
                textBox_Weight.Enabled = false;
                textBox_modulus.Enabled = false;
            }
            else
            {
                textBox_Weight.Enabled = true;
                textBox_modulus.Enabled = true;
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<DataRow> drlist = new List<DataRow>();
                if (dataGridView_bd.SelectedRows.Count > 0)
                {
                    for (int i = 0; i < dataGridView_bd.SelectedRows.Count; i++)
                        foreach (DataRow dr in dt_bd.Rows)
                        {
                            if (dr["标定点"].ToString() == dataGridView_bd.SelectedRows[i].Cells["标定点"].Value.ToString())
                                drlist.Add(dr);
                        }
                    if (drlist.Count != 0)
                        foreach (DataRow drd in drlist)
                            dt_bd.Rows.Remove(drd);
                    dataGridView_bd.DataSource = dt_bd;
                }
                else
                {
                    MessageBox.Show("请选择要删除的点", "系统提示");
                }
            }
            catch (Exception)
            {
            }
        }

        private void button_read_speed_dp_Click(object sender, EventArgs e)
        {
            CarWait.igbt.Get_Speed_DiameterandPusle();
            Thread.Sleep(100);
            textBox_diameter.Text = (float.Parse(CarWait.igbt.Speed_Diameter) * 10).ToString();
            textBox_pulse.Text = CarWait.igbt.Speed_Pusle;
        }

        private void button_addbdd_Click(object sender, EventArgs e)
        {
            try
            {
                float.Parse(textBox_bdd.Text.Trim());
                if (comboBox_bdtd.Text == "")
                {
                    MessageBox.Show("请选择标定通道！", "系统提示");
                    return;
                }
                DataRow dr = dt_bd.NewRow();
                dr["电压值"] = label_ad.Text;
                dr["标定点"] = textBox_bdd.Text.Trim();
                dt_bd.Rows.Add(dr);
                dataGridView_bd.DataSource = dt_bd;
            }
            catch (Exception)
            {
                MessageBox.Show("标定点数据有误，请检查！", "系统提示");
            }
        }

        private void button_read_force_m_Click(object sender, EventArgs e)
        {
            try
            {
                CarWait.igbt.Get_Force_Modulus();
                Thread.Sleep(100);
                switch (comboBox_bdtd.Text)
                {
                    case "1":
                        textBox_xs.Text = CarWait.igbt.b0;
                        break;
                    case "2":
                        textBox_xs.Text = CarWait.igbt.b1;
                        break;
                    case "3":
                        textBox_xs.Text = CarWait.igbt.b2;
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        private void button_scxs_Click(object sender, EventArgs e)
        {
            try
            {
                double v = 0;
                double d = 0;
                for (int i = 0; i < dataGridView_bd.Rows.Count; i++)
                {
                    v += float.Parse(dataGridView_bd.Rows[i].Cells["电压值"].Value.ToString());
                    d += float.Parse(dataGridView_bd.Rows[i].Cells["标定点"].Value.ToString());
                }
                textBox_xs.Text = float.Parse((d / v).ToString()).ToString();
            }
            catch (Exception)
            {
                dt_bd.Clear();
                dataGridView_bd.DataSource = dt_bd;
                MessageBox.Show("标定点可能有误,请重新操作。", "系统提示");
            }
        }

        private void button_write_Click(object sender, EventArgs e)
        {
            try
            {
                float xs = float.Parse(textBox_xs.Text);
                int td = int.Parse(comboBox_bdtd.Text);
                CarWait.igbt.Solidify_Force_Modulus(td, xs);
            }
            catch (Exception)
            {
                MessageBox.Show("是不是没有生成系数？或者没有选择通道？请检查！", "系统提示");
            }
        }

        private void comboBox_bdtd_SelectedIndexChanged(object sender, EventArgs e)
        {
            dt_bd = null;
            dt_bd = new DataTable();
            dt_bd.Columns.Add("电压值");
            dt_bd.Columns.Add("标定点");
            dataGridView_bd.DataSource = dt_bd;
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            string Aisle = "通道1";
            switch (comboBox_bdtd.Text)
            {
                case "1":
                    Aisle = "通道1";
                    break;
                case "2":
                    Aisle = "通道2";
                    break;
                case "3":
                    Aisle = "通道3";
                    break;
            }
            ini.INIIO.GetPrivateProfileString("UseMK", Aisle, "0.0", temp, 2048, @".\Config.ini");    //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
            textBox_xs.Text = temp.ToString();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            CarWait.Text_Verification(textBox_bdd, "[0-9\\.]", 12, true);
        }

        private void radioButton_td_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_td1.Checked)
            {
                ini.INIIO.WritePrivateProfileString("UseMK", "使用通道", "1", @".\Config.ini");
                CarWait.igbt.Select_Channel(1);
            }
            else if (radioButton_td2.Checked)
            {
                ini.INIIO.WritePrivateProfileString("UseMK", "使用通道", "2", @".\Config.ini");
                CarWait.igbt.Select_Channel(2);
            }
            else if (radioButton_td3.Checked)
            {
                ini.INIIO.WritePrivateProfileString("UseMK", "使用通道", "3", @".\Config.ini");
                CarWait.igbt.Select_Channel(3);
            }
        }
    }
}