using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace BpqFl
{
    public class bpxcontrol
    {
        public System.IO.Ports.SerialPort ComPort_2;
        byte[] Read_Buffer;
        public static string bpqXh = "AMB";
        #region AMB - NEW/G7通讯协议

        byte cmdRun = 0x02;
        byte cmdStop = 0x01;
        byte cmdDirect = 0x10;
        byte cmdReverse = 0x20;
        byte cmdDirectRun = 0x12;
        byte cmdReverseRun = 0x22;
        #endregion

        #region 构造函数
        public bpxcontrol(string XH)
        {
            bpqXh = XH;
        }
        #endregion

        #region 初始化串口
        /// <summary>
        /// 初始化串口
        /// </summary>
        /// <param name="PortName">串口名字</param>
        /// <param name="LinkString">连接字符串 如9600,N,8,1</param>
        /// <returns>bool</returns>
        public bool Init_Comm(string PortName, string LinkString)
        {
            try
            {
                ComPort_2 = new SerialPort();
                if (ComPort_2.IsOpen)
                    ComPort_2.Close();
                ComPort_2.PortName = PortName;
                ComPort_2.BaudRate = int.Parse(LinkString.Split(',').GetValue(0).ToString());
                switch (LinkString.Split(',').GetValue(1).ToString().ToUpper())
                {
                    case "N":
                        ComPort_2.Parity = System.IO.Ports.Parity.None;
                        break;
                    case "O":
                        ComPort_2.Parity = System.IO.Ports.Parity.Odd;
                        break;
                    case "E":
                        ComPort_2.Parity = System.IO.Ports.Parity.Even;
                        break;
                    case "M":
                        ComPort_2.Parity = System.IO.Ports.Parity.Mark;
                        break;
                    case "S":
                        ComPort_2.Parity = System.IO.Ports.Parity.Space;
                        break;
                    default:
                        ComPort_2.Parity = System.IO.Ports.Parity.None;
                        break;
                }
                ComPort_2.DataBits = int.Parse(LinkString.Split(',').GetValue(2).ToString());
                switch (LinkString.Split(',').GetValue(3).ToString())
                {
                    case "1":
                        ComPort_2.StopBits = System.IO.Ports.StopBits.One;
                        break;
                    case "2":
                        ComPort_2.StopBits = System.IO.Ports.StopBits.Two;
                        break;
                    default:
                        ComPort_2.StopBits = System.IO.Ports.StopBits.One;
                        break;
                }
                ComPort_2.Open();
                if (ComPort_2.IsOpen)
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
        #region 接收数据
        /// <summary>
        /// 接收数据
        /// </summary>
        public void ReadData()
        {
            Read_Buffer = new byte[100];
            ComPort_2.Read(Read_Buffer, 0, ComPort_2.BytesToRead);
        }
        #endregion
        public UInt16 crc_cal_val(byte[] data_value, byte data_length)
        {
            int i;
            int data_index = data_length - 1;
            UInt16 crc_value = 0xffff;
            while ((data_length--) > 0)
            {
                crc_value ^= data_value[data_index - data_length];
                for (i = 0; i < 8; i++)
                {
                    if ((crc_value & 0x0001) > 0)
                        crc_value = Convert.ToUInt16((crc_value >> 1) ^ 0xa001);
                    else
                        crc_value = Convert.ToUInt16(crc_value >> 1);
                }
            }
            return (crc_value);
        }
        public int writebianpin_fre(UInt16 add, UInt16 data)
        {
            switch (bpqXh)
            {
                case "AMB":
                    Thread.Sleep(10);
                    byte[] sendbuf = { 0x01, 0x06, Convert.ToByte(add >> 8), Convert.ToByte(add & 0xff), Convert.ToByte(data >> 8), Convert.ToByte(data & 0xff), 0, 0 };
                    UInt16 crcvalue = 0;
                    byte[] receive_buf;
                    crcvalue = crc_cal_val(sendbuf, 6);
                    sendbuf[6] = Convert.ToByte(crcvalue & 0x00ff);
                    sendbuf[7] = Convert.ToByte(crcvalue >> 8);
                    Thread.Sleep(20);
                    ComPort_2.Write(sendbuf, 0, sendbuf.Length);
                    Thread.Sleep(10);
                    break;
                case "AMB100":
                    Thread.Sleep(10);
                    byte[] sendbufAMB100 = { 0x01, 0x06, Convert.ToByte(add >> 8), Convert.ToByte(add & 0xff), Convert.ToByte(data >> 8), Convert.ToByte(data & 0xff), 0, 0 };
                    UInt16 crcvalueAMB100 = 0;
                    byte[] receive_bufAMB100;
                    crcvalueAMB100 = crc_cal_val(sendbufAMB100, 6);
                    sendbufAMB100[6] = Convert.ToByte(crcvalueAMB100 & 0x00ff);
                    sendbufAMB100[7] = Convert.ToByte(crcvalueAMB100 >> 8);
                    Thread.Sleep(20);
                    ComPort_2.Write(sendbufAMB100, 0, sendbufAMB100.Length);
                    Thread.Sleep(10);
                    break;
                case "SB60":
                    Thread.Sleep(10);
                    byte[] sendbufSB = { 0x01, 0X10, 0x32, 0X00, 0X00, 0X02, 0X04, 0X00, 0X37, Convert.ToByte(data >> 8), Convert.ToByte(data & 0xff), 0, 0 };
                    UInt16 crcvalueSB = 0;
                    byte[] receive_bufSB;
                    crcvalue = crc_cal_val(sendbufSB, 11);
                    sendbufSB[11] = Convert.ToByte(crcvalue & 0x00ff);
                    sendbufSB[12] = Convert.ToByte(crcvalue >> 8);
                    Thread.Sleep(20);
                    ComPort_2.Write(sendbufSB, 0, sendbufSB.Length);
                    Thread.Sleep(10);
                    break;
                case "X11":
                    Thread.Sleep(10);
                    byte[] sendbufX11 = { 0x01, 0X10, 0x00, 0X00, 0X00, 0X02, 0X04, 0X00, 0X01, Convert.ToByte(data >> 8), Convert.ToByte(data & 0xff), 0, 0 };
                    UInt16 crcvalueX11 = 0;
                    byte[] receive_bufX11;
                    crcvalueX11 = crc_cal_val(sendbufX11, 11);
                    sendbufX11[11] = Convert.ToByte(crcvalueX11 & 0x00ff);
                    sendbufX11[12] = Convert.ToByte(crcvalueX11 >> 8);
                    Thread.Sleep(20);
                    ComPort_2.Write(sendbufX11, 0, sendbufX11.Length);
                    Thread.Sleep(10);
                    break;
                default: break;
            }
            return 1;
        }
        public bool turnOnMotor()
        {
            switch (bpqXh)
            {
                case "AMB":
                    writebianpin_fre(0x2000, 0x0012);
                    break;
                case "AMB100":
                    writebianpin_fre(0x1000, 0x0001);
                    break;
                case "SB60":
                    turnOffMotor();
                    Thread.Sleep(10);
                    byte[] sendbufSB = { 0x01, 0X10, 0x32, 0X00, 0X00, 0X01, 0X02, 0X00, 0X07, 0XF4, 0X51 };
                    ComPort_2.Write(sendbufSB, 0, sendbufSB.Length);
                    Thread.Sleep(10);
                    break;
                    break;
                default: break;
            }
            return true;
        }
        public bool turnOffMotor()
        {
            switch (bpqXh)
            {
                case "AMB":
                    writebianpin_fre(0x2000, 0x0001);
                    break;
                case "AMB100":
                    writebianpin_fre(0x1000, 0x0006);
                    break;
                case "SB60":
                    Thread.Sleep(10);
                    byte[] sendbufSB = { 0x01, 0X10, 0x32, 0X00, 0X00, 0X01, 0X02, 0X00, 0X06, 0X35, 0X91 };
                    ComPort_2.Write(sendbufSB, 0, sendbufSB.Length);
                    Thread.Sleep(10);
                    break;
                case "X11":
                    Thread.Sleep(10);
                    byte[] sendbufX11 = { 0x01, 0X10, 0x00, 0X00, 0X00, 0X01, 0X02, 0X00, 0X00, 0XA6, 0X50 };

                    ComPort_2.Write(sendbufX11, 0, sendbufX11.Length);
                    Thread.Sleep(10);
                    break;
                default: break;
            }
            return true;
        }
        public bool setMotorFre(double frequency)
        {

            switch (bpqXh)
            {
                case "AMB":
                    UInt16 fre = (UInt16)(frequency * 10);
                    writebianpin_fre(0x2001, fre);
                    break;
                case "AMB100":
                    UInt16 freAMB100 = (UInt16)(frequency * 100);
                    writebianpin_fre(0x2000, freAMB100);
                    break;
                case "SB60":
                    UInt16 freSB = (UInt16)(frequency * 100);
                    writebianpin_fre(0x2001, freSB);
                    break;
                case "X11"://30000为设定频率的100%
                    UInt16 freX11 = (UInt16)(frequency * 30000 / 80);
                    if (freX11 > 30000) freX11 = 30000;
                    writebianpin_fre(0x2001, freX11);
                    break;
                default: break;
            }
            return true;
        }
        public bool resetMotor()
        {
            switch (bpqXh)
            {
                case "AMB":
                    writebianpin_fre(0x2002, 0x0002);

                    break;
                default: break;
            }
            return true;
        }
        #region 关闭串口
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns>bool</returns>
        public bool Close_Com()
        {
            bool temp = false;
            try
            {
                if (ComPort_2.IsOpen)
                {
                    ComPort_2.Close();
                    temp = true;
                }
            }
            catch (Exception er)
            {
                throw er;
            }
            return temp;
        }
        #endregion
    }
}
