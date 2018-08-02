using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Detect
{
    public partial class Rylr : Form
    {
        SYS_DAL.Main_dal main_dal = new SYS_DAL.Main_dal();
        public Rylr()
        {
            InitializeComponent();
        }

        private void Rylr_Load(object sender, EventArgs e)
        {
            try
            {
                if (CarWait.UseMK == "BNTD")
                    btn_qxdq.Enabled = true;
                else
                    btn_qxdq.Enabled = false;
                try
                {
                    Exhaust.Fla502_temp_data Environment = new Exhaust.Fla502_temp_data();
                    Environment = CarWait.fla_502.Get_Temp();
                    textBox_wd.Text = Environment.TEMP.ToString("0.0");
                    textBox_sd.Text = Environment.HUMIDITY.ToString("0.0");
                    textBox_dqy.Text = Environment.AIRPRESSURE.ToString("0.0");
                }
                catch
                {
                    textBox_wd.Text = CarWait.wd;
                    textBox_sd.Text = CarWait.sd;
                    textBox_dqy.Text = CarWait.dqy;
                }
                List<string> lis = main_dal.Get_yg_list();
                comboBox_jsy.Items.AddRange(lis.ToArray());
                comboBox_czy.Items.AddRange(lis.ToArray());
                if (CarWait.Jcjsy != "" && CarWait.Jcczy != "")
                {
                    if(comboBox_jsy.Items.Contains(CarWait.Jcjsy))
                        comboBox_jsy.Text = CarWait.Jcjsy;
                    if(comboBox_czy.Items.Contains(CarWait.Jcczy))
                        comboBox_czy.Text = CarWait.Jcczy;
                }
                else
                {
                    if (comboBox_jsy.Items.Count >= 1)
                        comboBox_jsy.SelectedIndex = 1;
                    if (comboBox_czy.Items.Count >= 1)
                        comboBox_czy.SelectedIndex = 1;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("出错啦", er.ToString());
                return;
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                string errstr = "";
                foreach (Control ct in groupBox2.Controls)
                {
                    if (ct is ComboBox || ct is TextBox)
                    {
                        if (string.IsNullOrEmpty(ct.Text))
                        {
                            errorProvider1.SetError(ct, ct.Tag + "不能为空");
                            errstr += ct.Tag + " ";
                        }
                        else
                        {
                            try
                            {
                                float.Parse(ct.Text);
                            }
                            catch (Exception)
                            {
                                errorProvider1.SetError(ct, ct.Tag + "数据有误");
                                errstr += ct.Tag + " ";
                            }
                        }
                    }
                }

                foreach (Control ct in groupBox1.Controls)
                {
                    if (ct is ComboBox || ct is TextBox)
                    {
                        if (string.IsNullOrEmpty(ct.Text))
                        {
                            errorProvider1.SetError(ct, ct.Tag + "不能为空");
                            errstr += ct.Tag + " ";
                        }
                    }
                }

                if (errstr == "")
                {
                    CarWait.Jcjsy = comboBox_jsy.Text.Trim();
                    CarWait.Jcczy = comboBox_czy.Text.Trim();
                    ini.INIIO.WritePrivateProfileString("人员配置", "检测驾驶员", CarWait.Jcjsy, @".\Config.ini");
                    ini.INIIO.WritePrivateProfileString("人员配置", "检测操作员", CarWait.Jcczy, @".\Config.ini");
                    CarWait.wd = textBox_wd.Text.Trim();
                    CarWait.sd = textBox_sd.Text.Trim();
                    CarWait.dqy = textBox_dqy.Text.Trim();
                    this.Close();
                }
                else
                    MessageBox.Show("系统提示", "请选择或输入 " + errstr);
            }
            catch (Exception)
            {
            }
        }

        private void btn_qxdq_Click(object sender, EventArgs e)
        {
            try
            {
                Exhaust.Fla502_temp_data Environment = new Exhaust.Fla502_temp_data();
                Environment=CarWait.fla_502.Get_Temp();
                textBox_wd.Text = Environment.TEMP.ToString("0.0");
                textBox_sd.Text = Environment.HUMIDITY.ToString("0.0");
                textBox_dqy.Text = Environment.AIRPRESSURE.ToString("0.0");
            }
            catch (Exception)
            {
            }
        }

        private void textBox_wd_TextChanged(object sender, EventArgs e)
        {
            CarWait.Text_Verification(textBox_wd, "[0-9\\.]", 32, true);
        }

        private void textBox_sd_TextChanged(object sender, EventArgs e)
        {
            CarWait.Text_Verification(textBox_sd, "[0-9\\.]", 32, true);
        }

        private void textBox_dqy_TextChanged(object sender, EventArgs e)
        {
            CarWait.Text_Verification(textBox_dqy, "[0-9\\.]", 32, true);
        }
    }
}
