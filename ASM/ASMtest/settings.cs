using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ASMtest
{
    public partial class settings : Form
    {
        carinfor.configIni configini = new carinfor.configIni();
        public settings()
        {
            InitializeComponent();
        }

       
        private void buttonAsmSave_Click(object sender, EventArgs e)
        {
            carinfor.AsmConfigInfdata asmconfig = configini.getAsmConfigIni();

            asmconfig.Ndz = float.Parse(textBoxAsmNdz.Text);
            asmconfig.Gljz = float.Parse(textBoxAsmGlwc.Text);
            asmconfig.Lxcc = float.Parse(textBoxAsmCcsj.Text);
            asmconfig.FlowTime = int.Parse(textBoxAsmFlowtime.Text);
            asmconfig.SpeedMonitor = checkBoxAsmSdjk.Checked;
            asmconfig.PowerMonitor = checkBoxAsmGljk.Checked;
            asmconfig.ConcentrationMonitor = checkBoxAsmNdjk.Checked;
            asmconfig.RemainedMonitor = checkBoxAsmClljk.Checked;
            asmconfig.Flowback = checkBoxAsmFlowback.Checked;
            asmconfig.IfFqyTl = checkBoxAsmZero.Checked;
            asmconfig.IfDisplayData = checkBoxAsmDispData.Checked;
            asmconfig.IfSureTemp = checkBoxAsmSuretemp.Checked;
            if (configini.writeAsmConfigIni(asmconfig))
                MessageBox.Show("保存成功.", "系统提示");
            else
                MessageBox.Show("数据输入有误,请检查.", "未成功保存");
        }

        private void settings_Load(object sender, EventArgs e)
        {
            carinfor.AsmConfigInfdata asmconfig = configini.getAsmConfigIni();

            textBoxAsmNdz.Text = asmconfig.Ndz.ToString("0.0");
            textBoxAsmGlwc.Text = asmconfig.Gljz.ToString("0.0");
            textBoxAsmCcsj.Text = asmconfig.Lxcc.ToString("0.0");
            textBoxAsmFlowtime.Text = asmconfig.FlowTime.ToString("0");
            checkBoxAsmSdjk.Checked = asmconfig.SpeedMonitor;
            checkBoxAsmGljk.Checked = asmconfig.PowerMonitor;
            checkBoxAsmNdjk.Checked = asmconfig.ConcentrationMonitor;
            checkBoxAsmClljk.Checked = asmconfig.RemainedMonitor;
            checkBoxAsmFlowback.Checked = asmconfig.Flowback;
            checkBoxAsmZero.Checked = asmconfig.IfFqyTl;
            checkBoxAsmDispData.Checked = asmconfig.IfDisplayData;
            checkBoxAsmSuretemp.Checked = asmconfig.IfSureTemp;

        }
    }
}
