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
    public partial class Form1 : Form
    {
        public Form1(frmContainer parent)
        {
            InitializeComponent();
            toolStripComboBoxFont.SelectedIndex = 0;
            MdiParent = parent;
            //frmChild child = new frmChild(this);
            //child.Show();
        }

        private void boldToolStripButton_CheckedChanged(object sender, EventArgs e)
        {
            Font oldFont, newFont;
            bool checkstate = ((ToolStripButton)sender).Checked;
            oldFont = this.richTextBox1.SelectionFont;
            if (checkstate)
                newFont = new Font(oldFont, oldFont.Style |FontStyle.Bold);
            else
                newFont = new Font(oldFont, oldFont.Style &~ FontStyle.Bold);
            richTextBox1.SelectionFont = newFont;
            richTextBox1.Focus();
            toolStripButtonBold.CheckedChanged -= new EventHandler(boldToolStripButton_CheckedChanged);
            toolStripButtonBold.Checked = checkstate;
            toolStripButtonBold.CheckedChanged += new EventHandler(boldToolStripButton_CheckedChanged);
            toolStripStatusLabelBold.Enabled = checkstate;
        }

        private void toolStripButtonItalic_CheckedChanged(object sender, EventArgs e)
        {
            Font oldFont, newFont;
            bool checkstate = ((ToolStripButton)sender).Checked;
            oldFont = this.richTextBox1.SelectionFont;
            if (!checkstate)
                newFont = new Font(oldFont, oldFont.Style & ~FontStyle.Italic);
            else
                newFont = new Font(oldFont, oldFont.Style | FontStyle.Italic);
            richTextBox1.SelectionFont = newFont;
            richTextBox1.Focus();
            toolStripButtonItalic.CheckedChanged -= new EventHandler(toolStripButtonItalic_CheckedChanged);
            toolStripButtonItalic.Checked = checkstate;
            toolStripButtonItalic.CheckedChanged += new EventHandler(toolStripButtonItalic_CheckedChanged);
            toolStripStatusLabelItalic.Enabled = checkstate;
        }

        private void toolStripButtonUnderline_CheckedChanged(object sender, EventArgs e)
        {
            Font oldFont, newFont;
            bool checkstate = ((ToolStripButton)sender).Checked;
            oldFont = this.richTextBox1.SelectionFont;
            if (!checkstate)
                newFont = new Font(oldFont, oldFont.Style & ~FontStyle.Underline);
            else
                newFont = new Font(oldFont, oldFont.Style | FontStyle.Underline);
            richTextBox1.SelectionFont = newFont;
            richTextBox1.Focus();
            toolStripButtonUnderline.CheckedChanged -= new EventHandler(toolStripButtonUnderline_CheckedChanged);
            toolStripButtonUnderline.Checked = checkstate;
            toolStripButtonUnderline.CheckedChanged += new EventHandler(toolStripButtonUnderline_CheckedChanged);
            toolStripStatusLabelUnderline.Enabled = checkstate;
        }

        private void toolStripComboxFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = ((ToolStripComboBox)sender).SelectedItem.ToString();
            Font newFont = null;
            if (richTextBox1.SelectionFont == null)
                newFont = new Font(text, richTextBox1.Font.Size);
            else
                newFont = new Font(text, richTextBox1.SelectionFont.Size, richTextBox1.SelectionFont.Style);
            richTextBox1.SelectionFont = newFont;
            richTextBox1.Focus();
        }

        private void richtext_textChanged(object sender, EventArgs e)
        {
            toolStripStatusLabelText.Text = "总共有字符:" + richTextBox1.Text.Length + "个";
        }
    }
}

