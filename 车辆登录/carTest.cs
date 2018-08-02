using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using carinfor;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace 车辆登录
{
    public partial class carTest : Form
    {
        public carIni carini = new carIni();
        Thread th_wait = null;
        public delegate void wtControlvalue(Control textbox, string value);

        public void SetControlText(Control textbox, string value)
        {
            BeginInvoke(new wtControlvalue(textboxValue), textbox, value);
        }
        public void textboxValue(Control textbox, string value)
        {
            textbox.Text = value;
        }
        public carTest()
        {
            InitializeComponent();
        }

        private void radioButtonFuelTest_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            if (buttonTest.Text == "开始检测"|| buttonTest.Text == "重新检测")
            {
                double a = 0;
                yhcarInidata carinidata = new yhcarInidata();
                carinidata.检测流水号 = "";
                carinidata.检测类型 = 0;
                carinidata.车辆号牌 = textBoxPlate.Text;
                carinidata.号牌种类 = comboBoxPlateType.Text;
                carinidata.车辆型号 = textBoxPpxh.Text;
                carinidata.发动机型号 = textBoxFdjxh.Text;
                carinidata.燃料种类 = radioButtonCy.Checked ? 1 : 0;
                carinidata.加载力计算方式 = 1;
                if (textBoxEdzzl.Text == "")
                {
                    MessageBox.Show("额定总质量不能为空", "提示");
                    return;
                }
                else if (!double.TryParse(textBoxEdzzl.Text, out a))
                {
                    MessageBox.Show("额定总质量非数字", "提示");
                    return;
                }
                else
                {
                    carinidata.额定总质量 = a;
                }
                if (textBoxQdkzzl.Text == "")
                {
                    MessageBox.Show("驱动轴空载质量不能为空", "提示");
                    return;
                }
                else if (!double.TryParse(textBoxQdkzzl.Text, out a))
                {
                    MessageBox.Show("驱动轴空载质量非数字", "提示");
                    return;
                }
                else
                {
                    carinidata.驱动轴空载质量 = a;
                }
                if (radioButtonFuelTest.Checked)
                {
                    carinidata.汽车类型 = radioButtonKc.Checked ? 0 : 1;
                    carinidata.油耗限值 = 0;
                    carinidata.油耗检测加载力 = 0;
                    carinidata.油耗检测工况速度 = 0;
                    carinidata.驱动轴数 = int.Parse(comboBoxQdzs.Text);
                    carinidata.轮胎类型 = comboBoxLtlx.SelectedIndex;
                    if (carinidata.轮胎类型 == 0)
                    {
                        if (textBoxZwtkd.Text == "")
                        {
                            MessageBox.Show("子午胎轮胎断面宽度不能为空", "提示");
                            return;
                        }
                        else if (!double.TryParse(textBoxZwtkd.Text, out a))
                        {
                            MessageBox.Show("子午胎轮胎断面宽度非数字", "提示");
                            return;
                        }
                        else
                        {
                            carinidata.子午胎轮胎断面宽度 = a;
                        }
                    }
                    else
                    {
                        carinidata.子午胎轮胎断面宽度 = 0;
                    }
                    if (textBoxQlj.Text == "")
                    {
                        MessageBox.Show("汽车前轮距不能为空", "提示");
                        return;
                    }
                    else if (!double.TryParse(textBoxQlj.Text, out a))
                    {
                        MessageBox.Show("汽车前轮距非数字", "提示");
                        return;
                    }
                    else
                    {
                        carinidata.汽车前轮距 = a;
                    }
                    if (textBoxQcgd.Text == "")
                    {
                        MessageBox.Show("汽车高度不能为空", "提示");
                        return;
                    }
                    else if (!double.TryParse(textBoxQcgd.Text, out a))
                    {
                        MessageBox.Show("汽车高度非数字", "提示");
                        return;
                    }
                    else
                    {
                        carinidata.汽车高度 = a;
                    }


                    if (carinidata.汽车类型 == 0)
                    {
                        carinidata.客车等级 = comboBoxKcdj.SelectedIndex;
                        if (carinidata.汽车类型 == 0)
                        {
                            if (textBoxKccc.Text == "")
                            {
                                MessageBox.Show("客车车长不能为空", "提示");
                                return;
                            }
                            else if (!double.TryParse(textBoxKccc.Text, out a))
                            {
                                MessageBox.Show("客车车长非数字", "提示");
                                return;
                            }
                            else
                            {
                                carinidata.客车车长 = a;
                            }
                        }
                        else
                        {
                            carinidata.客车车长 = 0;
                        }

                    }
                    else
                    {
                        carinidata.货车车身型式 = comboBoxHccsxs.SelectedIndex;
                        if (carinidata.货车车身型式 == 2)
                        {
                            if (textBoxQycmzzzl.Text == "")
                            {
                                MessageBox.Show("牵引车满载总质量不能为空", "提示");
                                return;
                            }
                            else if (!double.TryParse(textBoxQycmzzzl.Text, out a))
                            {
                                MessageBox.Show("牵引车满载总质量非数字", "提示");
                                return;
                            }
                            else
                            {
                                carinidata.牵引车满载总质量 = a;
                            }
                        }
                        else
                        {
                            carinidata.牵引车满载总质量 = 0;
                        }
                    }
                }
                else
                {

                    carinidata.动力性检测加载力 = 0;
                    carinidata.驱动轴数 = int.Parse(comboBoxQdzs.Text);
                    carinidata.轮胎类型 = comboBoxLtlx.SelectedIndex;
                    if (carinidata.燃料种类 == 0)
                    {
                        if (textBoxEdnj.Text == "")
                        {
                            MessageBox.Show("额定扭矩不能为空", "提示");
                            return;
                        }
                        else if (!double.TryParse(textBoxEdnj.Text, out a))
                        {
                            MessageBox.Show("额定扭矩非数字", "提示");
                            return;
                        }
                        else
                        {
                            carinidata.点燃式额定扭矩 = a;
                        }
                        if (textBoxEdnjzs.Text == "")
                        {
                            MessageBox.Show("额定扭矩转速不能为空", "提示");
                            return;
                        }
                        else if (!double.TryParse(textBoxEdnjzs.Text, out a))
                        {
                            MessageBox.Show("额定扭矩转速非数字", "提示");
                            return;
                        }
                        else
                        {
                            carinidata.点燃式额定扭矩转速 = a;
                        }
                    }
                    else
                    {
                        carinidata.是否危险货物运输车辆 = checkBoxIsDanger.Checked ? 1 : 0;
                        carinidata.压燃式功率参数类型 = comboBoxGlcslx.SelectedIndex;
                        if (textBoxEdgl.Text == "")
                        {
                            MessageBox.Show("额定功率不能为空", "提示");
                            return;
                        }
                        else if (!double.TryParse(textBoxEdgl.Text, out a))
                        {
                            MessageBox.Show("额定功率非数字", "提示");
                            return;
                        }
                        else
                        {
                            carinidata.压燃式额定功率 = a;
                        }
                    }
                }
                if (!carini.writeYhCarIni(carinidata))
                {
                    MessageBox.Show("发送车辆检测信息有误，不能开始检测");
                }
                else
                {
                    try
                    {
                        File.Delete("C://jcdatatxt/fuelResult.ini");
                        File.Delete("C://jcdatatxt/DynamicResult.ini");
                        if (radioButtonFuelTest.Checked)
                        {
                            Process.Start("D://环保检测子程序/碳平衡油耗仪.EXE", "auto");
                            exeName = "碳平衡油耗仪";
                        }
                        else
                        { 
                            Process.Start("D://环保检测子程序/动力性测量.EXE", "auto");
                            exeName = "动力性测量";
                        }
                        th_wait = new Thread(waitTestFinished);
                        th_wait.Start();
                        buttonTest.Text = "检测已开始";
                        //buttonTest.Enabled = false;
                    }
                    catch(Exception er)
                    {
                        MessageBox.Show("打开检测子程序出现异常：" + er.Message);
                    }
                }

            }
        }
        /// <summary>

        /// 此函数用于判断某一外部进程是否打开

        /// </summary>

        /// <param name="processName">参数为进程名</param>

        /// <returns>如果打开了，就返回true，没打开，就返回false</returns>

        private bool IsProcessStarted(string processName)
        {

            Process[] temp = Process.GetProcessesByName(processName);

            if (temp.Length > 0) return true;

            else

                return false;

        }
        private void waitTestFinished()
        {
            try
            {
                string newPath = "C://jcdatatxt/fuelResult.ini";
                string newCsvPath = "C://jcdatatxt/testProcessData.csv";
                switch (exeName)
                {
                    case "碳平衡油耗仪":
                        newPath = "C://jcdatatxt/fuelResult.ini";
                        break;
                    case "动力性测量":
                        newPath = "C://jcdatatxt/DynamicResult.ini";
                        break;
                    default:
                        break;
                }
                while (true)
                {
                    Thread.Sleep(500);
                    if (!IsProcessStarted(exeName))
                    {
                        if (File.Exists(newPath))//东软平台状态字处理
                        {
                            byte[] byData = new byte[500];
                            char[] charData = new char[1000];
                            FileStream fs = new FileStream(newPath, FileMode.Open);
                            Decoder d = Encoding.Default.GetDecoder();
                            switch (exeName)
                            {
                                case "碳平衡油耗仪":
                                    SetControlText(buttonTest, "重新检测");
                                    fs.Seek(0, SeekOrigin.Begin);
                                    fs.Read(byData, 0, 500); //byData传进来的字节数组,用以接受FileStream对象中的数据,第2个参数是字节数组中开始写入数据的位置,它通常是0,表示从数组的开端文件中向数组写数据,最后一个参数规定从文件读多少字符.
                                    d.GetChars(byData, 0, byData.Length, charData, 0);
                                    SetControlText(richTextBox1,new string(charData));
                                    fs.Close();
                                    break;
                                case "动力性测量":
                                    SetControlText(buttonTest, "重新检测");
                                    fs.Seek(0, SeekOrigin.Begin);
                                    fs.Read(byData, 0, 500); //byData传进来的字节数组,用以接受FileStream对象中的数据,第2个参数是字节数组中开始写入数据的位置,它通常是0,表示从数组的开端文件中向数组写数据,最后一个参数规定从文件读多少字符.
                                    d.GetChars(byData, 0, byData.Length, charData, 0);
                                    SetControlText(richTextBox1, new string(charData));
                                    fs.Close();
                                    break;
                                default:
                                    break;
                            }
                            return;
                        }
                        else
                        {
                            SetControlText(buttonTest, "重新检测");
                            SetControlText(richTextBox1, "检测结束，无检测结果");
                        }
                    }
                    
                }
            }
            catch
            { }
        }
        public string exeName = "";
        private void carTest_Load(object sender, EventArgs e)
        {
            radioButtonFuelTest.Checked = true;
            radioButtonQy.Checked = true;
            radioButtonKc.Checked = true;
            foreach(Control childcombobox in panel3.Controls)
            {
                if(childcombobox is ComboBox)
                    ((ComboBox)childcombobox).SelectedIndex = 0;
            }
        }
    }
}
