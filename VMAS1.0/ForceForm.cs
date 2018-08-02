using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VMAS1._0
{
    public partial class ForceForm : Form
    {
        public ForceForm(VMAS1._0.demarcate parent)
        {
            InitializeComponent();
            this.MdiParent = parent;
        }
    }
}
