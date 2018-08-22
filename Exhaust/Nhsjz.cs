using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace Exhaust
{
    public class Nhsjz
    {
        //private System.ComponentModel.IContainer components = new System.ComponentModel.Container();
        public System.IO.Ports.SerialPort ComPort_1;
        byte[] Send_Buffer;                                     //发送缓冲区
        byte[] Read_Buffer;                                     //读取缓冲区
        bool Read_Flag = false;                                 //是否有数据可以读取

        #region VMT-2000通讯协议

        public int zs =0;
        public float wd =0, sd=0, dqy=0, yw = 0;

        #endregion

        #region 串口Nhsjz初始化
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

        

        public bool readData()
        {
            ReadData();
            int i = 0;
            byte CS = 0;
            byte[] Content = new byte[] { 0x02, 0x41, 0x03 };
            ComPort_1.Write(Content, 0, 3);        //发送开始测量命令
            Thread.Sleep(50);
            while (ComPort_1.BytesToRead < 39)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 20)
                    return false;
            }
            ReadData();
            if (Read_Buffer[0] == 0x02)
            {
                string wd_string = Encoding.Default.GetString(Read_Buffer, 12, 3);
                string sd_string = Encoding.Default.GetString(Read_Buffer, 16, 4);
                string dqy_string = Encoding.Default.GetString(Read_Buffer, 21, 4);
                string yw_string = Encoding.Default.GetString(Read_Buffer, 26,4);
                string zs_string = Encoding.Default.GetString(Read_Buffer, 31, 4);

                try
                {
                    float wd_temp = float.Parse(wd_string)*0.1f;
                    if (Read_Buffer[11] == 0x30) wd_temp = wd_temp - 30;
                    else wd_temp = 30 - wd_temp;
                    float sd_temp = float.Parse(sd_string) * 0.1f;
                    float dqy_temp = float.Parse(dqy_string) * 0.1f;
                    float yw_temp = float.Parse(yw_string) * 0.1f;
                    //if (Read_Buffer[26] == 0x30) yw_temp = yw_temp - 31;
                    //else 
                    yw_temp = yw_temp - 31;
                    int zs_temp = int.Parse(zs_string);
                    wd = wd_temp;
                    sd = sd_temp;
                    dqy = dqy_temp;
                    yw = yw_temp;
                    zs = zs_temp;
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
    }
}
