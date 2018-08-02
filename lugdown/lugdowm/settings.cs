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
    public partial class settings : Form
    {
        carinfor.configIni configini = new carinfor.configIni();
        public settings()
        {
            InitializeComponent();
        }

       

        private void buttonLugdownSave_Click(object sender, EventArgs e)
        {
            carinfor.LugdownConfigInfdata lugdownconfig = configini.getLugdownConfigIni();
            lugdownconfig.MinSpeed = int.Parse(textBoxLugDownMinSpeed.Text);
            lugdownconfig.MaxSpeed = int.Parse(textBoxLugDownMaxSpeed.Text);
            lugdownconfig.Zsj = comboBoxLugdownZsj.Text;
            lugdownconfig.Zsjck = comboBoxLugdownZsjCom.Text;
            lugdownconfig.IfSureTemp = checkBoxLugdownSureTemp.Checked;
            lugdownconfig.Smpl = float.Parse(textBoxLugdownSmpl.Text);
            if (radioButtonLugdownHgl.Checked)
                lugdownconfig.Glsmms = "恒功率";
            else
                lugdownconfig.Glsmms = "恒速";
            if (configini.writeLugdownConfigIni(lugdownconfig))
                MessageBox.Show("保存成功.", "系统提示");
            else
                MessageBox.Show("数据输入有误,请检查.", "未成功保存");
        }

        private void settings_Load(object sender, EventArgs e)
        {
            carinfor.LugdownConfigInfdata lugdownconfig = configini.getLugdownConfigIni();

            
            textBoxLugDownMinSpeed.Text = lugdownconfig.MinSpeed.ToString("0");
            textBoxLugDownMaxSpeed.Text = lugdownconfig.MaxSpeed.ToString("0");
            comboBoxLugdownZsj.Text = lugdownconfig.Zsj;
            comboBoxLugdownZsjCom.Text = lugdownconfig.Zsjck;
            checkBoxLugdownSureTemp.Checked = lugdownconfig.IfSureTemp;
            if (lugdownconfig.Glsmms == "恒功率")
            {
                radioButtonLugdownHgl.Checked = true;
                radioButtonLugdownhs.Checked = false;
            }
            else
            {
                radioButtonLugdownHgl.Checked = false;
                radioButtonLugdownhs.Checked = true;
            }
            textBoxLugdownSmpl.Text = lugdownconfig.Smpl.ToString("0");

        }
    }
}
