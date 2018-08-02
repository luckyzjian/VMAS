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
    public partial class frmContainer : Form
    {
        public frmContainer()
        {
            InitializeComponent();
            Form1 newForm = new Form1(this);
            newForm.Show();
        }
    }
}
