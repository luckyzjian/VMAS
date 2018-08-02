using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace lugdowm
{
    public partial class load_display : Form
    {
        public load_display()
        {
            InitializeComponent();
        }

        public void progress_show()
        {
            for (int i = 0; i <= 10; i++)
            {
                progressBar_load.Value = i*10;
                
                Thread.Sleep(300);
            }
        }

    }
}
