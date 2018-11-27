using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace LedControl
{
    public class BX5k1
    {
        //HtmlExtractor.Gb2312Encoding encoding = new HtmlExtractor.Gb2312Encoding();

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
        public byte row1 = 0;
        public byte row2 = 1;
        public byte[] CB_firstRowContent = { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
        public byte[] CB_secondRowContent = { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
        byte[] Read_Buffer;

        public byte Pinhao
        {
            get
            {
                return pinhao;
            }

            set
            {
                pinhao = value;
            }
        }
        #region 初始化串口
        /// <summary>
        /// 初始化串口
        /// </summary>
        /// <param name="PortName">串口名字</param>
        /// <param name="LinkString">连接字符串 如9600,N,8,1</param>
        /// <returns>bool</returns>
        public bool Init_Comm(string PortName, string LinkString,byte tjph)
        {
            try
            {
                Pinhao = tjph;
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
                ComPort_2 = null;

                throw (new ApplicationException("串口初始化出错，请检查串口是否被占用或设备配置的字符串是否正确"));
            }
        }
        #endregion
        #region 清除屏幕
        public void clearLed_ph()
        {
            led_clear(0, 0, 32, 32, 8);
            Thread.Sleep(50);
            led_clear(0, 32, 32, 32, 8);
            Thread.Sleep(50);
        }
        #endregion
        public void writeLed(string ledString, byte displayStyle)
        {
            switch (displayStyle)
            {
                case 1:
                    led_display(ledString, 0, 0, 32, 32, 0, 0, 0, 0, 1, 4, 10);
                    break;
                case 2:
                    led_display(ledString, 0, 0, 32, 32, 0, 0, 0, 0, 1, 4, 10);
                    break;
                case 3:
                    led_display(ledString, 0, 32, 32, 32, 0, 0, 0, 0, 1, 4, 10);
                    break;
                case 4:
                    led_display(ledString, 0, 0, 32, 32, 0, 0, 0, 0, 1, 4, 10);
                    break;
                case 5:
                    led_display(ledString, 0, 32, 32, 32, 0, 0, 0, 0, 1, 4, 10);
                    break;
                case 6:
                    led_display(ledString, 0, 32, 32, 32, 0, 0, 0, 0, 1, 4, 10);
                    break;
                case 7:
                    led_display(ledString, 0, 0, 32, 32, 0, 0, 0, 0, 3, 1, 4);
                    break;
                case 8:
                    led_display(ledString, 0, 32, 32, 32, 0, 0, 0, 0, 3, 1, 4);
                    break;
                default: break;
            }
        }
        public void writeLed(string ledString, byte displayStyle, string xh)
        {
            if (xh == "仰邦")
            {
                switch (displayStyle)
                {
                    case 1:
                        led_display(ledString, 0, 0, 32, 32, 0, 0, 0, 0, 1, 4, 10);
                        break;
                    case 2:
                        led_display(ledString, 0, 0, 32, 32, 0, 0, 0, 0, 1, 4, 10);
                        break;
                    case 3:
                        led_display(ledString, 0, 32, 32, 32, 0, 0, 0, 0, 1, 4, 10);
                        break;
                    case 4:
                        led_display(ledString, 0, 0, 32, 32, 0, 0, 0, 0, 1, 4, 10);
                        break;
                    case 5:
                        led_display(ledString, 0, 32, 32, 32, 0, 0, 0, 0, 1, 4, 10);
                        break;
                    case 6:
                        led_display(ledString, 0, 32, 32, 32, 0, 0, 0, 0, 1, 4, 10);
                        break;
                    case 7:
                        led_display(ledString, 0, 0, 32, 32, 0, 0, 0, 0, 3, 1, 4);
                        break;
                    case 8:
                        led_display(ledString, 0, 32, 32, 32, 0, 0, 0, 0, 3, 1, 4);
                        break;
                    default: break;
                }
            }
            else if (xh == "同济")
            {
                byte[] ledStringArray = System.Text.Encoding.GetEncoding("GB2312").GetBytes(ledString.Trim());
                byte[] content = new byte[4 + ledStringArray.Length];
                content[0] = esc;
                content[1] = Pinhao;
                if (displayStyle == 2)
                    displayStyle = 4;
                content[2] = displayStyle;
                for (byte i = 3; i < 3 + ledStringArray.Length; i++)
                {
                    content[i] = ledStringArray[i - 3];
                }
                content[3 + ledStringArray.Length] = over;
                for (int i = 0; i < content.Length; i++)
                {
                    ComPort_2.Write(content, i, 1);
                    Thread.Sleep(2);
                }
            }
            else if (xh == "安通")
            {
                byte[] ledStringArray = System.Text.Encoding.GetEncoding("GB2312").GetBytes(ledString.Trim());
                byte[] content = new byte[4 + ledStringArray.Length];
                if (displayStyle == 2)
                {
                    content[0] = 0x4c;
                    content[1] = 0x4c;
                    content[2] = 0x4c;
                }
                else
                {
                    content[0] = 0x49;
                    content[1] = 0x49;
                    content[2] = 0x49;
                }
                for (byte i = 3; i < 3 + ledStringArray.Length; i++)
                {
                    content[i] = ledStringArray[i - 3];
                }
                content[3 + ledStringArray.Length] = 0x02;
                for (int i = 0; i < content.Length; i++)
                {
                    ComPort_2.Write(content, i, 1);
                    Thread.Sleep(1);
                }
            }
            else if ( xh == "同济单排")
            {
                byte[] ledStringArray = System.Text.Encoding.GetEncoding("GB2312").GetBytes(ledString.Trim());
                byte[] content = new byte[4 + ledStringArray.Length];
                content[0] = esc;
                content[1] = 1;
                content[2] = displayStyle;
                for (byte i = 3; i < 3 + ledStringArray.Length; i++)
                {
                    content[i] = ledStringArray[i - 3];
                }
                content[3 + ledStringArray.Length] = over;
                for (int i = 0; i < content.Length; i++)
                {
                    ComPort_2.Write(content, i, 1);
                    Thread.Sleep(20);
                }
            }

            else if (xh == "大雷中屏")
            {
                byte[] ledStringArray = System.Text.Encoding.GetEncoding("GB2312").GetBytes(ledString.Trim());
                byte[] content = new byte[3 + ledStringArray.Length];
                content[0] = 0x00;
                if (displayStyle == 2)
                {
                    content[1] = row1;
                }
                else
                {
                    content[1] = row2;
                }
                 //   displayStyle = 4;
                //content[2] = displayStyle;
                for (byte i = 2; i < 2 + ledStringArray.Length; i++)
                {
                    content[i] = ledStringArray[i - 2];
                }
                content[2 + ledStringArray.Length] = 0x0d;
                for (int i = 0; i < content.Length; i++)
                {
                    ComPort_2.Write(content, i, 1);
                    Thread.Sleep(1);
                }
            }
            else if (xh == "荆州广佛")
            {
                byte[] ledStringArray = System.Text.Encoding.GetEncoding("GB2312").GetBytes(ledString.Trim());
                byte[] content = new byte[5 + ledStringArray.Length];
                content[0] = 0x00;
                content[1] = Pinhao;
                if (displayStyle == 2)
                {
                    content[2] = row1;
                }
                else
                {
                    content[2] = row2;
                }
                content[3] = 0x32;
                //   displayStyle = 4;
                //content[2] = displayStyle;
                for (byte i = 4; i < 4 + ledStringArray.Length; i++)
                {
                    content[i] = ledStringArray[i - 4];
                }
                content[4 + ledStringArray.Length] = 0x0d;
                for (int i = 0; i < content.Length; i++)
                {
                    ComPort_2.Write(content, i, 1);
                    Thread.Sleep(1);
                }
            }
            else if (xh == "华燕")
            {
                byte[] ledStringArray = System.Text.Encoding.GetEncoding("GB2312").GetBytes(ledString.Trim());
                byte[] ledStringAfterBq = new byte[20];
                if (displayStyle == 2)
                {
                    ledStringAfterBq[0] = row1;
                    ledStringAfterBq[1] = row1;
                }
                else if(displayStyle==5)
                {
                    ledStringAfterBq[0] = row2;
                    ledStringAfterBq[1] = row2;
                }
                ledStringAfterBq[2] = 0x10;
                ledStringAfterBq[3] = 0x00;
                if (ledStringArray.Length > 16)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        ledStringAfterBq[i+4] = ledStringArray[i];
                    }
                }
                else
                {
                    for (int i = 0; i < ledStringArray.Length; i++)
                    {
                        ledStringAfterBq[i+4] = ledStringArray[i];
                    }
                    for(int i=ledStringArray.Length;i<16;i++)
                    {
                        ledStringAfterBq[i+4] = 0x20;
                    }
                }
                ComPort_2.Write(ledStringAfterBq,0, ledStringAfterBq.Length);
            }
            else if (xh == "南华")
            {
                byte[] ledStringArray = System.Text.Encoding.GetEncoding("GB2312").GetBytes(ledString.Trim());
                byte[] content = new byte[5 + ledStringArray.Length];
                content[0] = 0;
                content[1] = 0x31;
                if (displayStyle == 2)
                    content[2] = 0x31;
                else if (displayStyle == 5)
                    content[2] = 0x32;
                content[3] = 0x32;
                for (byte i = 4; i < 4 + ledStringArray.Length; i++)
                {
                    content[i] = ledStringArray[i - 4];
                }
                content[4+ ledStringArray.Length] = 0x0d;
                ComPort_2.Write(content, 0, content.Length);

            }
            else if (xh == "成保")
            {
                byte[] ledStringArray = System.Text.Encoding.GetEncoding("GB2312").GetBytes(ledString.Trim());
                if (displayStyle == 2)//上排
                {
                    int len = ledStringArray.Length;
                    int ix = (12 - len) / 2;
                    ix = ix >= 0 ? ix : 0;
                    len = len <= 12 ? len : 12;
                    int iy = len + ix;
                    for (int i = 0; i < ix; i++)
                    {
                        CB_firstRowContent[i] = 0x20;
                    }
                    for(int i=ix;i<iy;i++)
                    {
                        CB_firstRowContent[i] = ledStringArray[i-ix];
                    }
                    for(int i=iy;i<12;i++)
                    {
                        CB_firstRowContent[i] = 0x20;
                    }
                }
                else//下排
                {
                    int len = ledStringArray.Length;
                    int ix = (12 - len) / 2;
                    ix = ix >= 0 ? ix : 0;
                    len = len <= 12 ? len : 12;
                    int iy = len + ix;
                    for (int i = 0; i < ix; i++)
                    {
                        CB_secondRowContent[i] = 0x20;
                    }
                    for (int i = ix; i < iy; i++)
                    {
                        CB_secondRowContent[i] = ledStringArray[i - ix];
                    }
                    for (int i = iy; i < 12; i++)
                    {
                        CB_secondRowContent[i] = 0x20;
                    }
                }
                byte[] content = new byte[26];
                content[0] = 0x26;
                for (byte i = 0; i <12; i++)
                {
                    content[i+1] = CB_firstRowContent[i];
                    content[i + 13] = CB_secondRowContent[i];
                }
                content[25] = 0x0d;
                for (int i = 0; i < content.Length; i++)
                {
                    ComPort_2.Write(content, i, 1);
                    Thread.Sleep(1);
                }
            }
        }
        public static byte[] ConvertStringToByteArray(string stringToConvert)
        {
            return (new UnicodeEncoding()).GetBytes(stringToConvert);
        }
        #region 清屏
        /// <summary>
        /// 清除x*y的屏显内容，x和y以像素点为单位
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void led_clear(byte x, byte y, byte w, byte h, byte length)
        {
            string clearstring = "";
            for (byte i = 0; i < length; i++)
                clearstring += "　";
            if (clearstring.Length >= 1)
                led_display(clearstring, x, y, w, h, 0, 0, 0, 0, 1, 0, 1);
        }
        #endregion
        #region 显示文字
        public void led_display(string str, byte x, byte y, byte w, byte h, int ziku, int size, int color, int xinhao, int displayStyle, int displaySpeed, int stayTime)
        {
            if (ComPort_2 != null)
            {
                List<byte> s = new List<byte>();
                byte[] ledStringArray = System.Text.Encoding.GetEncoding("GB2312").GetBytes(str);
                foreach (byte c in ledStringArray)
                    s.Add(c);
                if (xinhao == 0)
                {
                    byte length = 0;
                    byte[] c1 ={0xA5,0xA5,0xA5,0xA5,0x01,0x00,0x00,0x80,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,
	        0x51,0x06,0x00,0x00,0xA3,0x06,0x01,0x00,0x00,0x00,0x01,0x00,0x00,0x00,0x00,0x00,0x00,
	        0x00,0x10,0x00,0x20,0x00,0x01,0xff,0xff,0xff,0xff,0xff,0xff,0x01,0x02,0x02,0x01,0x00,
	        0x04,0x01,0x00,0x00,0x00,0x00,0x5c,0x46,0x45,0x30,0x30,0x30,0x5c,0x43,0x01};
                    length = (byte)s.Count;
                    c1[16] += (byte)(length + 45);
                    c1[21] += (byte)(length + 40);
                    c1[25] += (byte)(length + 36);
                    c1[28] = x;
                    c1[30] = y;
                    c1[32] = w;
                    c1[34] = h;
                    if (y > 0)
                        c1[36] = 1;
                    else
                        c1[36] = 0;
                    c1[45] = 2;
                    c1[46] = (byte)displayStyle;
                    c1[48] = (byte)displaySpeed;
                    c1[49] = (byte)stayTime;
                    c1[50] += (byte)(length + 9);

                    if (ziku == 0)
                    {
                        c1[56] = (byte)'O';
                    }
                    else
                    {
                        c1[56] = (byte)'E';
                    }
                    switch (size)
                    {
                        case 0:
                            c1[59] = (byte)'0';
                            break;
                        case 1:
                            c1[59] = (byte)'1';
                            break;
                        case 2:
                            c1[59] = (byte)'2';
                            break;
                        default:
                            c1[59] = (byte)'0';
                            break;
                    }
                    switch (color)
                    {
                        case 0:
                            c1[62] = (byte)'1';
                            break;
                        case 1:
                            c1[62] = (byte)'2';
                            break;
                        case 2:
                            c1[62] = (byte)'3';
                            break;
                        default:
                            c1[62] = (byte)'1';
                            break;
                    }
                    ComPort_2.Write(c1, 0, c1.Length);
                    for (int i = 0; i < length; i++)
                    {
                        if (s[i] == 0xa5)
                        {
                            s[i] = 0xa6;
                            s.Insert(1, 0x02);
                            length++;
                        }
                        else if (s[i] == 0xa6)
                        {
                            s.Insert(1, 0x01);
                            length++;
                        }
                        else if (s[i] == 0x5a)
                        {
                            s[i] = 0x5b;
                            s.Insert(1, 0x02);
                            length++;
                        }
                        else if (s[i] == 0x5b)
                        {
                            s.Insert(1, 0x01);
                            length++;
                        }
                    }
                    s.Add(0xff);
                    s.Add(0xff);
                    s.Add(0x5a);
                    byte[] s_Send = s.ToArray();
                    ComPort_2.Write(s_Send, 0, s_Send.Length);
                }
            }
        }
        #endregion
        public void closeLed()
        {
            try
            {
                if (ComPort_2.IsOpen)
                    ComPort_2.Close();
            }
            catch
                { }
        }
    }
}
