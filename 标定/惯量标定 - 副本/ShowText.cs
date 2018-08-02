using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 惯量标定
{
    public partial class ShowText : Form
    {
        public ShowText()
        {
            InitializeComponent();
        }
        public event EventHandler RichTextBoxChange;
        public string RichTextValue
        {
            get 
            {
                return richTextBox1.Text;
            }
            set
            {
                richTextBox1.Text = value;
            }
        }
        public ShowText(string Content)
        {
            InitializeComponent();
            RichTextValue = Content;
        }
        private void ShowText_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (RichTextBoxChange != null)
            {
                RichTextBoxChange(this, e);
            }
        }
    }
}
