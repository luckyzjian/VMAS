using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace Exhaust
{
    public class Fla501_data
    {

        private float _hc = 0.0f;
        private float _co = 0.0f;
        private float _no = 0.0f;
        private float _co2 = 0.0f;
        private float _o2 = 0.0f;
        private float _λ = 0.0f;
        private float _sd = 0.0f;
        private float _yw = 0.0f;
        private float _hjwd = 0.0f;
        private float _zs = 0.0f;
        private float _qlyl = 0.0f;
        private float _hjyl = 0.0f;
        private string _ll = "";
        private float _afr = 0.0f;

        public float AFR
        {
            get { return _afr; }
            set { _afr = value; }
        }


        /// <summary>
        /// 废气HC值
        /// </summary>
        public float HC
        {
            get { return _hc; }
            set { _hc = value; }
        }

        /// <summary>
        /// 废气CO值
        /// </summary>
        public float CO
        {
            get { return _co; }
            set { _co = value; }
        }

        /// <summary>
        /// 废气NO值
        /// </summary>
        public float NO
        {
            get { return _no; }
            set { _no = value; }
        }

        /// <summary>
        /// 废气CO2值
        /// </summary>
        public float CO2
        {
            get { return _co2; }
            set { _co2 = value; }
        }

        /// <summary>
        /// 废气O2值
        /// </summary>
        public float O2
        {
            get { return _o2; }
            set { _o2 = value; }
        }

        /// <summary>
        /// 废气λ值
        /// </summary>
        public float λ
        {
            get { return _λ; }
            set { _λ = value; }
        }

        /// <summary>
        /// 废气相对湿度值
        /// </summary>
        public float SD
        {
            get { return _sd; }
            set { _sd = value; }
        }

        /// <summary>
        /// 废气油温值
        /// </summary>
        public float YW
        {
            get { return _yw; }
            set { _yw = value; }
        }

        /// <summary>
        /// 废气环境温度值
        /// </summary>
        public float HJWD
        {
            get { return _hjwd; }
            set { _hjwd = value; }
        }

        /// <summary>
        /// 废气转速值
        /// </summary>
        public float ZS
        {
            get { return _zs; }
            set { _zs = value; }
        }

        /// <summary>
        /// 废气气路压力值
        /// </summary>
        public float QLYL
        {
            get { return _qlyl; }
            set { _qlyl = value; }
        }

        /// <summary>
        /// 废气环境压力值
        /// </summary>
        public float HJYL
        {
            get { return _hjyl; }
            set { _hjyl = value; }
        }
        /// <summary>
        /// 废气环境流量值
        /// </summary>
        public string LL
        {
            get { return _ll; }
            set { _ll = value; }
        }
    }
    public class Fla501_temp_data
    {

        private float _temp = 0.0f;
        private float _humidity = 0.0f;
        private float _airpressure = 0.0f;
        /// <summary>
        /// 环境温度值
        /// </summary>
        public float TEMP
        {
            get { return _temp; }
            set { _temp = value; }
        }

        /// <summary>
        /// 相对湿度
        /// </summary>
        public float HUMIDITY
        {
            get { return _humidity; }
            set { _humidity = value; }
        }

        /// <summary>
        /// 大气压力
        /// </summary>
        public float AIRPRESSURE
        {
            get { return _airpressure; }
            set { _airpressure = value; }
        }

    }

    public class Fla501
    {
        //private System.ComponentModel.IContainer components = new System.ComponentModel.Container();
        public System.IO.Ports.SerialPort ComPort_1;
        byte[] Send_Buffer;                                     //发送缓冲区
        byte[] Read_Buffer;                                     //读取缓冲区
        bool Read_Flag = false;                                 //是否有数据可以读取

        #region fla_502通讯协议
        byte DID = 0x02;                                            //数据头
        byte NAK = 0x15;                                            //命令错误，发送数据长度错误或者校验和错误

        byte cmd_GetPef = 0x18;
        byte cmd_Zero = 0x20;
        byte cmd_GetData = 0x40;
        byte cmd_SwitchOn = 0xa4;
        byte cmd_BangOn = 0xa5;
        byte cmd_SetMeasure = 0xa6;
        byte cmd_SetReady = 0xa7;
        byte cmd_ReadMemData = 0x80;
        byte cmd_Clear = 0xcc;

        public float hc_density = 0.0f;
        public float co_density = 0.0f;
        public float co2_density = 0.0f;
        public float o2_density = 0.0f;
        public float no_density = 0.0f;
        public float pef_value = 0.0f;

        #endregion

        #region 串口废气初始化
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

        #region 开始检测
        /// <summary>
        /// 开始检测
        /// </summary>
        /// <returns>string</returns>
        public bool Start()
        {
            ReadData();
            int i = 0;
            byte[] Content = new byte[] { 0x02,0x01,0xa6,0x57};
            ComPort_1.Write(Content, 0, 4);        //发送开始测量命令
            Thread.Sleep(50);
            while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return false;
            }
            ReadData();
            if (Read_Buffer[0] == 0x06)
                return true;
            else
                return false;
        }
        #endregion

        #region 停止检测
        /// <summary>
        /// 停止检测
        /// </summary>
        /// <returns>string</returns>
        public bool Stop()
        {
            ReadData();
            int i = 0;
            byte[] Content = new byte[] {0x02,0x01,0xa7,0x56 };
            ComPort_1.Write(Content, 0, 4);        //发送停止测量命令
            Thread.Sleep(50);
            while (ComPort_1.BytesToRead <4)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return false;
            }
            ReadData();                     //读取返回的数据
            if (Read_Buffer[0] == 0x06)
                return true;
            else
                return false;
        }
        #endregion
        #region 停止检测
        /// <summary>
        /// 清洗管道
        /// </summary>
        /// <returns>string</returns>
        public bool Clear()
        {
            ReadData();
            int i = 0;
            byte[] Content = new byte[] { 0x02, 0x01, 0xcc, 0x31 };
            ComPort_1.Write(Content, 0, 4);        //发送停止测量命令
            Thread.Sleep(50);
            while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return false;
            }
            ReadData();                     //读取返回的数据
            if (Read_Buffer[0] == 0x06)
                return true;
            else
                return false;
        }
        #endregion
        #region 开泵
        /// <summary>
        /// 停止检测
        /// </summary>
        /// <returns>string</returns>
        public bool openbang()
        {
            ReadData();
            int i = 0;
            byte[] Content = new byte[] { 0x02, 0x02, 0xa5, 0x01,0x56 };
            ComPort_1.Write(Content, 0, 5);        //发送停止测量命令
            Thread.Sleep(50);
            while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return false;
            }
            ReadData();                     //读取返回的数据
            if (Read_Buffer[0] == 0x06)
                return true;
            else
                return false;
        }
        #endregion
        #region 关泵
        /// <summary>
        /// 停止检测
        /// </summary>
        /// <returns>string</returns>
        public bool Closebang()
        {
            ReadData();
            int i = 0;
            byte[] Content = new byte[] { 0x02, 0x02, 0xa5, 0x00, 0x57 };
            ComPort_1.Write(Content, 0, 5);        //发送停止测量命令
            Thread.Sleep(50);
            while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return false;
            }
            ReadData();                     //读取返回的数据
            if (Read_Buffer[0] == 0x06)
                return true;
            else
                return false;
        }
        #endregion
        #region 开电磁阀
        /// <summary>
        /// 停止检测
        /// </summary>
        /// <returns>string</returns>
        public bool openSwitch()
        {
            ReadData();
            int i = 0;
            byte[] Content = new byte[] { 0x02, 0x02, 0xa4, 0x01, 0x57 };
            ComPort_1.Write(Content, 0, 5);        //发送停止测量命令
            Thread.Sleep(50);
            while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return false;
            }
            ReadData();                     //读取返回的数据
            if (Read_Buffer[0] == 0x06)
                return true;
            else
                return false;
        }
        #endregion
        #region 关电磁阀
        /// <summary>
        /// 停止检测
        /// </summary>
        /// <returns>string</returns>
        public bool CloseSwitch()
        {
            ReadData();
            int i = 0;
            byte[] Content = new byte[] { 0x02, 0x02, 0xa4, 0x00, 0x58 };
            ComPort_1.Write(Content, 0, 5);        //发送停止测量命令
            Thread.Sleep(50);
            while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return false;
            }
            ReadData();                     //读取返回的数据
            if (Read_Buffer[0] == 0x06)
                return true;
            else
                return false;
        }
        #endregion
        #region 获取数据
        /// <summary>
        /// byte cmdGetTemp = 0x02;
        /// </summary>
        /// <returns>Fla502_temp_data</returns>
        public Fla501_data Get_Data()
        {
            ReadData();
            Fla501_data Fla502_testdata = new Fla501_data();
            Fla502_testdata.CO = 0f;
            Fla502_testdata.O2 = 0f;
            Fla502_testdata.CO2 = 0f;
            Fla502_testdata.HC = 0f;
            Fla502_testdata.NO = 0f;
            Fla502_testdata.YW = 0f;
            int i = 0;
            byte[] Content = new byte[] { 0x02,0x01,0x40,0xbd };
            ComPort_1.Write(Content, 0, 4);//
            Thread.Sleep(50);
            while (ComPort_1.BytesToRead<23)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return Fla502_testdata;
            }
            ReadData();
            if (Read_Buffer[0] == 0x6)
            {
                byte[] temp_byte = new byte[2];
                temp_byte[0] = Read_Buffer[5];
                temp_byte[1] = Read_Buffer[4];
                Fla502_testdata.CO2= BitConverter.ToInt16(temp_byte, 0)/100f;         //环境温度
                temp_byte[0] = Read_Buffer[7];
                temp_byte[1] = Read_Buffer[6];
                Fla502_testdata.CO = BitConverter.ToInt16(temp_byte, 0) / 1000f;       //大气压力
                temp_byte[0] = Read_Buffer[9];
                temp_byte[1] = Read_Buffer[8];
                Fla502_testdata.HC = BitConverter.ToInt16(temp_byte, 0) ;               //相对湿度
                temp_byte[0] = Read_Buffer[11];
                temp_byte[1] = Read_Buffer[10];
                Fla502_testdata.O2 = BitConverter.ToInt16(temp_byte, 0)/100f;               //相对湿度
                temp_byte[0] = Read_Buffer[13];
                temp_byte[1] = Read_Buffer[12];
                Fla502_testdata.NO = BitConverter.ToInt16(temp_byte, 0);               //相对湿度
                temp_byte[0] = Read_Buffer[15];
                temp_byte[1] = Read_Buffer[14];
                Fla502_testdata.ZS = BitConverter.ToUInt16(temp_byte, 0);               //相对湿度
                temp_byte[0] = Read_Buffer[17];
                temp_byte[1] = Read_Buffer[16];
                Fla502_testdata.YW= BitConverter.ToInt16(temp_byte, 0)/10f;               //相对湿度
                temp_byte[0] = Read_Buffer[19];
                temp_byte[1] = Read_Buffer[18];
                Fla502_testdata.λ = BitConverter.ToInt16(temp_byte, 0)/100f;               //相对湿度
                temp_byte[0] = Read_Buffer[21];
                temp_byte[1] = Read_Buffer[20];
                Fla502_testdata.AFR = BitConverter.ToInt16(temp_byte, 0)/1000f;               //相对湿度
                

            }
            return Fla502_testdata;
        }
        #endregion
        #region 零点校正
        /// <summary>
        /// 停止检测
        /// </summary>
        /// <returns>string</returns>
        public bool SetZero()
        {
            ReadData();
            int i = 0;
            byte[] Content = new byte[] { 0x02, 0x01, 0x20, 0xdd};
            ComPort_1.Write(Content, 0, 4);        //发送停止测量命令
            Thread.Sleep(50);
            while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return false;
            }
            ReadData();                     //读取返回的数据
            if (Read_Buffer[0] == 0x06)
                return true;
            else
                return false;
        }
        #endregion
        #region 取PEF
        public float Getdata_PEF()
        {
            ReadData();
            int i = 0;
            byte[] Content = new byte[] { 0x02, 0x02, 0x18, 0x00,0xe4};
            ComPort_1.Write(Content, 0, 4);//发送调零命令
            Thread.Sleep(10);
            i = 0;
            while (ComPort_1.BytesToRead < 6)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return 0.0f;
            }
            ReadData();                         //读取数据
            byte[] temp_byte = new byte[2];
            temp_byte[0] = Read_Buffer[4];
            temp_byte[1] = Read_Buffer[3];
            return (BitConverter.ToInt16(temp_byte, 0) / 1000f);         //PEF

        }
        #endregion

    }
}
