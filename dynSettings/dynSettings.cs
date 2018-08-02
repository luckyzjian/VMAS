using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dynSettings
{
    public partial class dynSettings : Form
    {
        carinfor.configIni configini = new carinfor.configIni();
        public dynSettings()
        {
            InitializeComponent();
        }

        private void dynSettings_Load(object sender, EventArgs e)
        {
            carinfor.DynConfigInfdata dynconfig = configini.getDynConfigIni();
            textBoxDynSdxs.Text = dynconfig.DynSdxs.ToString("0.00");
            textBoxDynNlxs.Text = dynconfig.DynNlxs.ToString("0.00");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double sdxs = double.Parse(textBoxDynSdxs.Text);
                double nlxs = double.Parse(textBoxDynNlxs.Text);
                if(sdxs>3||sdxs<0.5)
                {
                    MessageBox.Show("速度系数必须在[0.5~3]区间内","错误");
                    return;
                }
                if (nlxs >2 || nlxs < 0.2)
                {
                    MessageBox.Show("速度系数必须在[0.2~2]区间内", "错误");
                    return;
                }
                configini.writeDynXsIni(sdxs, nlxs);
                MessageBox.Show("保存成功");
            }
            catch(Exception er)
            { MessageBox.Show("保存失败，数据格式有误"); }
        }
    }
}
