using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace lzTest
{
    public partial class settings : Form
    {
        carinfor.configIni configini = new carinfor.configIni();
        public settings()
        {
            InitializeComponent();
        }

        

        private void buttonBtgSave_Click(object sender, EventArgs e)
        {
            carinfor.BtgConfigInfdata btgconfig = configini.getBtgConfigIni();
            btgconfig.Dyzs = int.Parse(textBoxBtgDyzs.Text);
            btgconfig.RotateSpeedMonitor = checkBoxBtgZsjk.Checked;
            btgconfig.Zsj = comboBoxBtgZsj.Text;
            btgconfig.Zsjck = comboBoxBtgZsjCom.Text;

            if (configini.writeBtgConfigIni(btgconfig))
                MessageBox.Show("保存成功.", "系统提示");
            else
                MessageBox.Show("数据输入有误,请检查.", "未成功保存");
        }

        private void settings_Load(object sender, EventArgs e)
        {
            
            carinfor.BtgConfigInfdata btgconfig = configini.getBtgConfigIni();
            textBoxBtgDyzs.Text = btgconfig.Dyzs.ToString("0");
            checkBoxBtgZsjk.Checked = btgconfig.RotateSpeedMonitor;
            comboBoxBtgZsj.Text = btgconfig.Zsj;
            comboBoxBtgZsjCom.Text = btgconfig.Zsjck;
        }
    }
}
