using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace Exhaust
{
    public class VMT_2000
    {
        //private System.ComponentModel.IContainer components = new System.ComponentModel.Container();
        public System.IO.Ports.SerialPort ComPort_1;
        byte[] Send_Buffer;                                     //发送缓冲区
        byte[] Read_Buffer;                                     //读取缓冲区
        bool Read_Flag = false;                                 //是否有数据可以读取

        #region VMT-2000通讯协议

        public int zs=0;

        #endregion

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
        public void ReadData()
        {
            Read_Buffer = new byte[100];
            Read_Flag = false;
            ComPort_1.Read(Read_Buffer, 0, ComPort_1.BytesToRead);
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
        public bool readRotateSpeed()
        {
            ReadData();
            int i = 0;
            byte CS = 0;
            byte[] Content = new byte[] { 0x4e, 0x54, 0x30, 0xff, 0x53, 0x03, 0x01, 0x80, 0x7b };
            ComPort_1.Write(Content, 0, 9);        //发送开始测量命令
            Thread.Sleep(50);
            while (ComPort_1.BytesToRead < 13)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return false;
            }
            ReadData();
            if (Read_Buffer[7] == 0x80)
            {
                byte[] temp_byte = new byte[2];
                temp_byte[0] = Read_Buffer[9];
                temp_byte[1] = Read_Buffer[8];
                zs = BitConverter.ToInt16(temp_byte, 0);
                return true;//HC
            }
            else
                return false;
        }
    }
}
