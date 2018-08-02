using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace LED屏测试
{
    public partial class LED测试 : Form
    {
        public LED测试()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string com = comboBoxLEDck.Text;
            string compz = comboBoxLEDCOMSTRING.Text;
            string ledxh = comboBoxLEDXH.Text;
            string ledtext1 = textBox1.Text;
            string ledtext2 = textBox2.Text;
            string ledupnumber = comboBoxLEDROW1.Text;
            string leddownnumber = comboBoxLEDROW2.Text;
            LedControl.BX5k1 led = new LedControl.BX5k1();
            if (led.Init_Comm(com, compz,1))
            {
                led.writeLed(ledtext1, 2, ledxh);
                Thread.Sleep(100);
                led.writeLed(ledtext2, 5, ledxh);
                Thread.Sleep(100);
                led.ComPort_2.Close();
                MessageBox.Show("发送成功");
            }
            else
            {
                MessageBox.Show("串口打开失败");
            }
            
        }
    }
}
