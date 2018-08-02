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
    public partial class vmas_result_after : Form
    {
        public vmas_result_after()
        {
            InitializeComponent();
        }

        private void vmas_result_after_Load(object sender, EventArgs e)
        {
            label_cojg.Text = VMAS.vmas.COZL;
            label_coxz.Text = VMAS.Limit_CO_AFTER.ToString();
            label_copd.Text = VMAS.vmas.COPD;
            label_nojg.Text = VMAS.vmas.HCZL + "+" + VMAS.vmas.NOXZL;
            label_noxz.Text = VMAS.Limit_HCNOX_AFTER.ToString();
            label_nopd.Text = VMAS.vmas.NOXPD;
            label_cj.Text = VMAS.vmas.CJ;
        }
    }
}
