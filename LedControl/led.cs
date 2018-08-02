using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace LedControl
{
    public class led
    {
        public System.IO.Ports.SerialPort ComPort_2;
        private byte esc = 0x1b;
        private byte clr = 0x44;
        private byte over = 0x0d;
        private byte pinhao = 1;

        private byte speed_Set = 0;
        private byte upleft_cchh = 1;
        private byte upleft_ccjw = 2;
        private byte downleft_jwzd = 3;
        private byte upmiddle_ccjw = 4;
        private byte downmiddle_jwzd = 5;
        private byte down_zd = 6;
        private byte left32bit_jwzd = 7;
        private byte middle32bit_jwzd = 8;
        private byte zd32bit = 9;
        byte[] Read_Buffer;
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
                return false;
                //throw (new ApplicationException("串口初始化出错，请检查串口是否被占用或设备配置的字符串是否正确"));
            }
        }
        #endregion
        public void close_equipment()
        {
            try
            {
                ComPort_2.Close();
            }
            catch
            { }
        }
        #region 清除屏幕
        public void clearLed_ph()
        {
            byte[] content = { esc, 1, clr, over };
            ComPort_2.Write(content, 0, 4);
        }
        #endregion
        public void writeLed(string ledString, byte displayStyle)
        {
            byte[] ledStringArray = System.Text.Encoding.GetEncoding("GB2312").GetBytes(ledString);
            byte[] content = new byte[4 + ledStringArray.Length];
            content[0] = esc;
            content[1] = 1;
            content[2] = displayStyle;
            for (byte i = 3; i < 3 + ledStringArray.Length; i++)
            {
                content[i] = ledStringArray[i - 3];
            }
            content[3 + ledStringArray.Length] = over;
            ComPort_2.Write(content, 0, 4 + ledStringArray.Length);
        }
    }
}
