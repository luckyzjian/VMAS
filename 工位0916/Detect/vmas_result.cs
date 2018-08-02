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
    public partial class vmas_result : Form
    {
        public vmas_result()
        {
            InitializeComponent();
        }

        private void vmas_result_Load(object sender, EventArgs e)
        {
            label_cojg.Text=VMAS.vmas.COZL;
            label_coxz.Text=VMAS.Limit_CO_BEBORE.ToString();
            label_copd.Text=VMAS.vmas.COPD;
            label_hcjg.Text=VMAS.vmas.HCZL;
            label_hcxz.Text=VMAS.Limit_HC_BEBORE.ToString();
            label_hcpd.Text=VMAS.vmas.HCPD;
            label_nojg.Text=VMAS.vmas.NOXZL;
            label_noxz.Text=VMAS.Limit_NO_BEBORE.ToString();
            label_nopd.Text=VMAS.vmas.NOXPD;
            label_cj.Text=VMAS.vmas.CJ;
        }
    }
}
