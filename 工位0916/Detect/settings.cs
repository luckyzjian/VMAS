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
    public partial class settings : Form
    {
        carinfor.configIni configini = new carinfor.configIni();
        public settings()
        {
            InitializeComponent();
        }

        

        private void buttonVmasSave_Click(object sender, EventArgs e)
        {
            carinfor.VmasConfigInfdata vmasconfig = configini.getVmasConfigIni();
            vmasconfig.Ndz = float.Parse(textBoxVmasNdz.Text);
            vmasconfig.Lljll = float.Parse(textBoxVmasLljll.Text);
            vmasconfig.Wqll = float.Parse(textBoxVmasWqll.Text);
            vmasconfig.Xsb = float.Parse(textBoxVmasXsb.Text);
            vmasconfig.Gljz = float.Parse(textBoxVmasJzgl.Text);
            vmasconfig.Lxcc = float.Parse(textBoxVmasJzgl.Text);
            vmasconfig.Ljcc = float.Parse(textBoxVmasLxcc.Text);
            vmasconfig.Dssj = int.Parse(textBoxVmasDssj.Text);
            vmasconfig.FlowTime = int.Parse(textBoxVmasFlowbackTime.Text);
            vmasconfig.SpeedMonitor = checkBoxVmasSdjk.Checked;

            vmasconfig.PowerMonitor = checkBoxVmasJzgljk.Checked;
            vmasconfig.ConcentrationMonitor = checkBoxVmasNdjk.Checked;
            vmasconfig.FlowMonitorr = checkBoxVmasLljk.Checked;
            vmasconfig.ThinnerratioMonitor = checkBoxVmasXsbjk.Checked;
            vmasconfig.Huanjingo2Monitor = checkBoxVmasHjyjk.Checked;
            vmasconfig.RemainedMonitor = checkBoxVmasCyljk.Checked;
            vmasconfig.Flowback = checkBoxVmasFlowback.Checked;
            vmasconfig.IfFqyTl = checkBoxVmasZero.Checked;
            vmasconfig.IfDisplayData = checkBoxVmasDisplayData.Checked;
            vmasconfig.IfSureTemp = checkBoxVmasSureTemp.Checked;
            if (configini.writeVmasConfigIni(vmasconfig))
                MessageBox.Show("保存成功.", "系统提示");
            else
                MessageBox.Show("数据输入有误,请检查.", "未成功保存");
        }

        private void settings_Load(object sender, EventArgs e)
        {
            carinfor.VmasConfigInfdata vmasconfig = configini.getVmasConfigIni();

            textBoxVmasNdz.Text = vmasconfig.Ndz.ToString("0.0");
            textBoxVmasLljll.Text = vmasconfig.Lljll.ToString("0.0");
            textBoxVmasWqll.Text = vmasconfig.Wqll.ToString("0.0");
            textBoxVmasXsb.Text = vmasconfig.Xsb.ToString("0.0");
            textBoxVmasJzgl.Text = vmasconfig.Gljz.ToString("0.0");
            textBoxVmasLxcc.Text = vmasconfig.Lxcc.ToString("0.0");
            textBoxVmasLjcc.Text = vmasconfig.Ljcc.ToString("0.0");
            textBoxVmasDssj.Text = vmasconfig.Dssj.ToString("0");
            checkBoxVmasSdjk.Checked = vmasconfig.SpeedMonitor;
            checkBoxVmasJzgljk.Checked = vmasconfig.PowerMonitor;
            checkBoxVmasNdjk.Checked = vmasconfig.ConcentrationMonitor;
            checkBoxVmasLljk.Checked = vmasconfig.FlowMonitorr;
            checkBoxVmasXsbjk.Checked = vmasconfig.ThinnerratioMonitor;
            checkBoxVmasHjyjk.Checked = vmasconfig.Huanjingo2Monitor;
            checkBoxVmasCyljk.Checked = vmasconfig.RemainedMonitor;
            checkBoxVmasFlowback.Checked = vmasconfig.Flowback;
            checkBoxVmasZero.Checked = vmasconfig.IfFqyTl;
            checkBoxVmasDisplayData.Checked = vmasconfig.IfDisplayData;
            checkBoxVmasSureTemp.Checked = vmasconfig.IfSureTemp;
            textBoxVmasFlowbackTime.Text = vmasconfig.FlowTime.ToString("0");


        }
    }
}
