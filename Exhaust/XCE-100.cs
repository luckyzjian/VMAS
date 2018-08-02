using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace Exhaust
{
    public class XCE_100
    {
        //private System.ComponentModel.IContainer components = new System.ComponentModel.Container();
        public System.IO.Ports.SerialPort ComPort_1;
        byte[] Send_Buffer;                                     //发送缓冲区
        byte[] Read_Buffer;                                     //读取缓冲区
        bool Read_Flag = false;                                 //是否有数据可以读取

        #region VMT-2000通讯协议

        public int zs=0;
        public float temp = 0f;
        public float humidity = 0f;
        public float airpressure = 0f;

        #endregion
        private string yqxh = "xce_100";
        public XCE_100()
        { }
        public XCE_100(string xh)
        {
            yqxh = xh.ToLower();
        }

        #region 串口VMT-2000初始化
        /// <summary>
        /// 串口废气初始化
        /// </summary>
        /// <param name="PortName">串口名字</param>
        /// <param name="LinkString">连接字符串 如9600,N,8,1</param>
        /// <returns>bool</returns>
        public bool Init_Comm(string PortName, string LinkString)
        {
            try
            {
                ComPort_1 = new SerialPort();
                if (ComPort_1.IsOpen)
                    ComPort_1.Close();
                ComPort_1.PortName = PortName;
                ComPort_1.BaudRate = int.Parse(LinkString.Split(',').GetValue(0).ToString());
                switch (LinkString.Split(',').GetValue(1).ToString())
                {
                    case "n":
                        ComPort_1.Parity = System.IO.Ports.Parity.None;
                        break;
                    case "N":
                        ComPort_1.Parity = System.IO.Ports.Parity.None;
                        break;
                    case "o":
                        ComPort_1.Parity = System.IO.Ports.Parity.Odd;
                        break;
                    case "O":
                        ComPort_1.Parity = System.IO.Ports.Parity.Odd;
                        break;
                    case "e":
                        ComPort_1.Parity = System.IO.Ports.Parity.Even;
                        break;
                    case "E":
                        ComPort_1.Parity = System.IO.Ports.Parity.Even;
                        break;
                    case "m":
                        ComPort_1.Parity = System.IO.Ports.Parity.Mark;
                        break;
                    case "M":
                        ComPort_1.Parity = System.IO.Ports.Parity.Mark;
                        break;
                    case "s":
                        ComPort_1.Parity = System.IO.Ports.Parity.Space;
                        break;
                    case "S":
                        ComPort_1.Parity = System.IO.Ports.Parity.Space;
                        break;
                    default:
                        ComPort_1.Parity = System.IO.Ports.Parity.None;
                        break;
                }
                ComPort_1.DataBits = int.Parse(LinkString.Split(',').GetValue(2).ToString());
                switch (LinkString.Split(',').GetValue(3).ToString())
                {
                    case "1":
                        ComPort_1.StopBits = System.IO.Ports.StopBits.One;
                        break;
                    case "2":
                        ComPort_1.StopBits = System.IO.Ports.StopBits.Two;
                        break;
                    default:
                        ComPort_1.StopBits = System.IO.Ports.StopBits.One;
                        break;
                }
                //ComPort_1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Ref_Readflag);
                ComPort_1.Open();
                if (ComPort_1.IsOpen)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                throw (new ApplicationException("串口初始化出错，请检查串口是否被占用或设备配置的字符串是否正确"));
            }
        }
        #endregion
        #region 确定是否有返回数据
        /// <summary>
        /// 确定是否有返回数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Ref_Readflag(object sender, SerialDataReceivedEventArgs e)
        {
            Read_Flag = true;
        }
        #endregion

        #region 接收数据
        /// <summary>
        /// 接收数据
        /// </summary>
        public int ReadData()
        {
            int bytesCount = 0;
            Read_Buffer = new byte[100];
            Read_Flag = false;
            bytesCount= ComPort_1.BytesToRead;
            ComPort_1.Read(Read_Buffer, 0, bytesCount);
            return bytesCount;
        }
        #endregion

        

        public bool setWorkMode(string workmode)
        {
            ReadData();
            int i = 0;
            byte CS = 0;
            byte[] Content = new byte[] { 0x4e, 0x54, 0x30, 0x31,0x53,0x04,0x01,0x03,0x00,0x00};
            if (workmode == "C") Content[8] = 0;
            else Content[8] = 1;
            for (i = 5; i < 8; i++)
                CS += Content[i];
            CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)))[0];
            Content[9] = CS;
            ComPort_1.Write(Content, 0, 10);        //发送开始测量命令
            Thread.Sleep(50);
            while (ComPort_1.BytesToRead < 10)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return false;
            }
            ReadData();
            if (Read_Buffer[7] == 0xff)
                return false;
            else
                return true;
        }
        public bool demarcateHumidity(double value)
        {
            ReadData();
            if (yqxh == "xce_100")
            {
                string valuestring = (value * 10).ToString("0000");
                ComPort_1.WriteLine("B");
                int len = valuestring.Length;
                int len1 = 4 - len;
                byte[] ledStringArray = System.Text.Encoding.Default.GetBytes(valuestring.Trim());
                for (int i=0;i<len1;i++)
                    ComPort_1.WriteLine("0");
                for (int i = 0; i < len; i++)
                    ComPort_1.Write(ledStringArray,i,1);
                ComPort_1.WriteLine("P");
                return true;
            }
            else
                return false;
        }
        public bool readEnvironment()
        {
            ReadData();
            int i = 0;
            byte CS = 0;
            if (yqxh == "xce_100")
            {
                ComPort_1.WriteLine("W");
                Thread.Sleep(50);
                while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                {
                    i++;
                    Thread.Sleep(10);
                    if (i == 100)
                        return false;
                }
                Thread.Sleep(200);
                int length = ReadData();
                if (Read_Buffer[length - 1] == 'E')
                {
                    try
                    {
                        string datastring = Encoding.Default.GetString(Read_Buffer, 0, length - 1);
                        int Hindex = 0, Aindex = 0;
                        Hindex = datastring.IndexOf('H');
                        Aindex = datastring.IndexOf('P');
                        temp = float.Parse(Encoding.Default.GetString(Read_Buffer, 1, Hindex - 1));
                        humidity = float.Parse(Encoding.Default.GetString(Read_Buffer, Hindex + 1, Aindex - 1 - Hindex));
                        airpressure = float.Parse(Encoding.Default.GetString(Read_Buffer, Aindex + 1, length - 2 - Aindex));
                        return true;//HC
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                    return false;
            }
            else if (yqxh == "dwsp_t5")
            {
                byte[] Content = new byte[] { 0x01, 0x04, 0x00,0x00,0x00,0x03,0xb0,0x0b};
                ComPort_1.Write(Content, 0, 8);        //发送开始测量命令
                while (ComPort_1.BytesToRead < 11)                          //等待仪器返回
                {
                    i++;
                    Thread.Sleep(10);
                    if (i == 100)
                        return false;
                }
                Thread.Sleep(200);
                int length = ReadData();
                if (Read_Buffer[0] == 0x01)
                {
                    try
                    {                        
                        temp = (float)(Read_Buffer[5]*256+Read_Buffer[6])/10.0f;
                        humidity = (float)(Read_Buffer[7] * 256 + Read_Buffer[8]) / 10.0f;
                        airpressure = (float)(Read_Buffer[3] * 256 + Read_Buffer[4]) / 100.0f;
                        return true;//HC
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                    return false;
            }
            else if (yqxh == "fth_2")
            {
                byte[] Content = new byte[] { 0x4e, 0x54, 0x33, 0x33, 0x53, 0x03, 0x01, 0x01,0xfa };
                ComPort_1.Write(Content, 0, 9);        //发送开始测量命令
                while (ComPort_1.BytesToRead < 19)                          //等待仪器返回
                {
                    i++;
                    Thread.Sleep(10);
                    if (i == 100)
                        return false;
                }
                Thread.Sleep(200);
                int length = ReadData();
                if (Read_Buffer[0] == 0x4e)
                {
                    try
                    {
                        temp = (float)(Read_Buffer[10] * 256 + Read_Buffer[11]) / 10.0f;
                        humidity = (float)(Read_Buffer[12] * 256 + Read_Buffer[13]) / 10.0f;
                        airpressure = (float)(Read_Buffer[14] * 256 + Read_Buffer[15]) / 10.0f;
                        return true;//HC
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                    return false;
            }
            else if (yqxh == "rz_1")
            {
                byte[] Content = new byte[] { 0x01, 0x04, 0x00, 0x00, 0x00, 0x04, 0xf1, 0xc9 };
                ComPort_1.Write(Content, 0,8);        //发送开始测量命令
                while (ComPort_1.BytesToRead < 13)                          //等待仪器返回
                {
                    i++;
                    Thread.Sleep(10);
                    if (i == 100)
                        return false;
                }
                Thread.Sleep(200);
                int length = ReadData();
                if (Read_Buffer[0] == 0x01)
                {
                    try
                    {
                        temp = (float)(Read_Buffer[3] * 256 + Read_Buffer[4])*0.5f / 100.0f;
                        humidity = (float)(Read_Buffer[5] * 256 + Read_Buffer[6])*2.0f / 100.0f;
                        airpressure = (float)(Read_Buffer[7] * 256 + Read_Buffer[8])*1.1f / 100.0f;
                        return true;//HC
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                    return false;
            }
            else
                return false;
        }
    }
}
