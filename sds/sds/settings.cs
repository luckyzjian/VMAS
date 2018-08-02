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
    public partial class settings : Form
    {
        carinfor.configIni configini = new carinfor.configIni();
        public settings()
        {
            InitializeComponent();
        }

        

        private void buttonSdsSave_Click(object sender, EventArgs e)
        {
            carinfor.SdsConfigInfdata sdsconfig = configini.getSdsConfigIni();
            sdsconfig.FlowTime = int.Parse(textBoxSdsFlowtime.Text);
            sdsconfig.Ndz = float.Parse(textBoxSdsNdz.Text);
            sdsconfig.Zscc = int.Parse(textBoxSdsZscc.Text);
            sdsconfig.Flowback = checkBoxSdsFlowback.Checked;
            sdsconfig.IfFqyTl = checkBoxSdsZero.Checked;
            sdsconfig.ConcentrationMonitor = checkBoxSdsNdjk.Checked;
            sdsconfig.RotateSpeedMonitor = checkBoxSdsZsjk.Checked;
            sdsconfig.Zsj = comboBoxSdsZsj.Text;
            sdsconfig.Zsjck = comboBoxSdsZsjCom.Text;
            if (configini.writeSdsConfigIni(sdsconfig))
                MessageBox.Show("保存成功.", "系统提示");
            else
                MessageBox.Show("数据输入有误,请检查.", "未成功保存");
        }

        private void settings_Load(object sender, EventArgs e)
        {
            carinfor.SdsConfigInfdata sdsconfig = configini.getSdsConfigIni();
            

            textBoxSdsFlowtime.Text = sdsconfig.FlowTime.ToString("0");
            textBoxSdsNdz.Text = sdsconfig.Ndz.ToString("0.0");
            textBoxSdsZscc.Text = sdsconfig.Zscc.ToString("0");
            checkBoxSdsFlowback.Checked = sdsconfig.Flowback;
            checkBoxSdsZero.Checked = sdsconfig.IfFqyTl;
            checkBoxSdsNdjk.Checked = sdsconfig.ConcentrationMonitor;
            checkBoxSdsZsjk.Checked = sdsconfig.RotateSpeedMonitor;
            comboBoxSdsZsj.Text = sdsconfig.Zsj;
            comboBoxSdsZsjCom.Text = sdsconfig.Zsjck;

        }
    }
}
