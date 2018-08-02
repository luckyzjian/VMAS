using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace Exhaust
{
    public class YD_1_smoke
    {
        public YD_1_smoke()
        {
            success = false;
            firstData = 0;
            sencondData = 0;
            thirdData = 0;
            _averagerb = 0;
            wd = 0;
            sd = 0;
            dqy = 0;
        }
        private bool success;

        public bool Success
        {
            get { return success; }
            set { success = value; }
        }
        private float firstData;

        public float FirstData
        {
            get { return firstData; }
            set { firstData = value; }
        }
        private float sencondData;

        public float SencondData
        {
            get { return sencondData; }
            set { sencondData = value; }
        }
        private float thirdData;

        public float ThirdData
        {
            get { return thirdData; }
            set { thirdData = value; }
        }

        private float _averagerb;

        
        /// <summary>
        /// 光吸收系数
        /// </summary>
        public float AVERAGERB
        {
            get { return _averagerb; }
            set { _averagerb = value; }
        }

        private float wd;

        public float WD
        {
            get { return wd; }
            set { wd = value; }
        }
        private float sd;

        public float SD
        {
            get { return sd; }
            set { sd = value; }
        }
        private float dqy;

        public float DQY
        {
            get { return dqy; }
            set { dqy = value; }
        }
    }

    public class YD_1
    {
        private string yqxh = "yd_1";
        private const string ffxh = "fby_201";
        private byte yqadd = 0x01;
        public YD_1(string xh,byte add)
        {
            yqadd = add;
            yqxh = xh.ToLower();
        }
        public YD_1()
        { }

        public System.IO.Ports.SerialPort ComPort_3;
        byte[] Send_Buffer;                                     //发送缓冲区
        byte[] Read_Buffer;                                     //读取缓冲区
        bool Read_Flag = false;                                 //是否有数据可以读取

       

        

        #region 南华yd-1命令字
        byte cmdStartTest = 0x91;
        byte cmdTestFinished = 0x94;
        byte cmdGetData = 0x97;
        byte cmdReset = 0x9a;
        byte cmdAnswerSuccess = 0x06;
        byte cmdAnswerFail = 0x15;
        byte cmdFirstAndSecondFinished = 0x95;
        byte cmdAnswerReset = 0x98;
        #endregion




        #region 串口初始化
        /// <summary>
        /// 串口初始化
        /// </summary>
        /// <param name="PortName">串口名字</param>
        /// <param name="LinkString">连接字符串 如9600,N,8,1</param>
        /// <returns>bool</returns>
        public bool Init_Comm(string PortName, string LinkString)
        {
            try
            {
                ComPort_3 = new SerialPort();
                if (ComPort_3.IsOpen)
                    ComPort_3.Close();
                ComPort_3.PortName = PortName;
                ComPort_3.BaudRate = int.Parse(LinkString.Split(',').GetValue(0).ToString());
                switch (LinkString.Split(',').GetValue(1).ToString())
                {
                    case "n":
                        ComPort_3.Parity = System.IO.Ports.Parity.None;
                        break;
                    case "N":
                        ComPort_3.Parity = System.IO.Ports.Parity.None;
                        break;
                    case "o":
                        ComPort_3.Parity = System.IO.Ports.Parity.Odd;
                        break;
                    case "O":
                        ComPort_3.Parity = System.IO.Ports.Parity.Odd;
                        break;
                    case "e":
                        ComPort_3.Parity = System.IO.Ports.Parity.Even;
                        break;
                    case "E":
                        ComPort_3.Parity = System.IO.Ports.Parity.Even;
                        break;
                    case "m":
                        ComPort_3.Parity = System.IO.Ports.Parity.Mark;
                        break;
                    case "M":
                        ComPort_3.Parity = System.IO.Ports.Parity.Mark;
                        break;
                    case "s":
                        ComPort_3.Parity = System.IO.Ports.Parity.Space;
                        break;
                    case "S":
                        ComPort_3.Parity = System.IO.Ports.Parity.Space;
                        break;
                    default:
                        ComPort_3.Parity = System.IO.Ports.Parity.None;
                        break;
                }
                ComPort_3.DataBits = int.Parse(LinkString.Split(',').GetValue(2).ToString());
                switch (LinkString.Split(',').GetValue(3).ToString())
                {
                    case "1":
                        ComPort_3.StopBits = System.IO.Ports.StopBits.One;
                        break;
                    case "2":
                        ComPort_3.StopBits = System.IO.Ports.StopBits.Two;
                        break;
                    default:
                        ComPort_3.StopBits = System.IO.Ports.StopBits.One;
                        break;
                }
                //ComPort_3.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Ref_Readflag);
                //ComPort_3.ReceivedBytesThreshold = 4;
                ComPort_3.Open();
                if (ComPort_3.IsOpen)
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

        #region 发送数据
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="Cmd">命令</param>
        /// <param name="Content">内容</param>
        public void SendData(byte[] Content)
        {
            //使用 WaitHandle 等待异步调用 

            ComPort_3.Write(Content, 0, 4);
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
            ComPort_3.Read(Read_Buffer, 0, ComPort_3.BytesToRead);
        }
        #endregion
        #region 鸣泉的校验方式
        private byte getCS_MQ(byte[] content, int count)
        {
            byte CS = 0;
            for (int i = 0; i < count; i++)
                CS += content[i];
            CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
            return CS;
        }
        #endregion
        #region 进入自动测量状态
        public bool startTest()
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {
                case "yd_1":
                    byte[] Content_NH = new byte[] { cmdStartTest };                    
                    ComPort_3.Write(Content_NH, 0, 1);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 1)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == cmdAnswerFail)
                        return false;
                    else if (Read_Buffer[0] == cmdAnswerSuccess)
                        return true;
                    else
                        return false;
                    break;
                case ffxh:
                    byte CHECKSUM = 0;
                    byte[] Content_FF = new byte[] { yqadd };
                    Content_FF[0] = yqadd;
                    ComPort_3.Parity = System.IO.Ports.Parity.Mark;
                    ComPort_3.Write(Content_FF, 0,1);
                    Thread.Sleep(3);
                    CHECKSUM += Content_FF[0];
                    Content_FF[0] = cmdStartTest;
                    ComPort_3.Parity = System.IO.Ports.Parity.Space;
                    ComPort_3.Write(Content_FF, 0, 1);
                    Thread.Sleep(3);
                    CHECKSUM += Content_FF[0];
                    Content_FF[0] = 1;
                    ComPort_3.Parity = System.IO.Ports.Parity.Space;
                    ComPort_3.Write(Content_FF, 0, 1);
                    Thread.Sleep(3);
                    CHECKSUM += Content_FF[0];
                    Content_FF[0] = CHECKSUM;
                    ComPort_3.Parity = System.IO.Ports.Parity.Space;
                    ComPort_3.Write(Content_FF, 0, 1);
                    Thread.Sleep(3);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 1)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == cmdAnswerFail)
                        return false;
                    else if (Read_Buffer[1] == cmdAnswerSuccess)
                        return true;
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        #endregion
        #region 复位
        public bool ResetTest()
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {
                case "yd_1":
                    byte[] Content_NH = new byte[] { cmdReset };
                    ComPort_3.Write(Content_NH, 0, 1);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 1)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == cmdAnswerFail)
                        return false;
                    else if (Read_Buffer[0] == cmdAnswerSuccess)
                        return true;
                    else
                        return false;
                    break;

                case ffxh:
                    byte CHECKSUM = 0;
                    byte[] Content_FF = new byte[] { yqadd };
                    Content_FF[0] = yqadd;
                    ComPort_3.Parity = System.IO.Ports.Parity.Mark;
                    ComPort_3.Write(Content_FF, 0, 1);
                    Thread.Sleep(3);
                    CHECKSUM += Content_FF[0];
                    Content_FF[0] = cmdReset;
                    ComPort_3.Parity = System.IO.Ports.Parity.Space;
                    ComPort_3.Write(Content_FF, 0, 1);
                    Thread.Sleep(3);
                    CHECKSUM += Content_FF[0];
                    Content_FF[0] = 1;
                    ComPort_3.Parity = System.IO.Ports.Parity.Space;
                    ComPort_3.Write(Content_FF, 0, 1);
                    Thread.Sleep(3);
                    CHECKSUM += Content_FF[0];
                    Content_FF[0] = CHECKSUM;
                    ComPort_3.Parity = System.IO.Ports.Parity.Space;
                    ComPort_3.Write(Content_FF, 0, 1);
                    Thread.Sleep(3);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 1)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == cmdAnswerFail)
                        return false;
                    else if (Read_Buffer[1] == cmdAnswerSuccess)
                        return true;
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        #endregion
        #region 判断第一次测量是否完成
        public bool WaitFirstTestFinished()
        {
            int i = 0;
            switch (yqxh)
            {
                case "yd_1":
                    if (ComPort_3.BytesToRead >= 1)
                    {
                        ReadData();
                        if (Read_Buffer[0] == cmdFirstAndSecondFinished)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                    break;

                case ffxh:
                    if (ComPort_3.BytesToRead >= 1)
                    {
                        ReadData();
                        if (Read_Buffer[0] == cmdFirstAndSecondFinished)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                default:
                    return false;
                    break;
            }
        }
        #endregion
        #region 判断第三次测量是否完成
        public bool WaitTestFinished()
        {
            int i = 0;
            switch (yqxh)
            {
                case "yd_1":
                    if (ComPort_3.BytesToRead >= 1)
                    {
                        ReadData();
                        if (Read_Buffer[0] == cmdTestFinished)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                    break;
                case ffxh:
                    if (ComPort_3.BytesToRead >= 1)
                    {
                        ReadData();
                        if (Read_Buffer[0] == cmdFirstAndSecondFinished)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                default:
                    return false;
                    break;
            }
        }
        #endregion
        #region 读取测量结果
        public YD_1_smoke readResult()
        {
            int i = 0;
            YD_1_smoke smoke = new YD_1_smoke();
            ReadData();
            switch (yqxh)
            {
                case "yd_1":
                    byte[] Content_NH = new byte[] { cmdGetData };
                    ComPort_3.Write(Content_NH, 0, 1);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 20)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x01)
                    {
                        smoke.Success = true;
                        smoke.FirstData = float.Parse(Encoding.Default.GetString(Read_Buffer, 1, 4));
                        smoke.SencondData = float.Parse(Encoding.Default.GetString(Read_Buffer, 5, 4));
                        smoke.ThirdData = float.Parse(Encoding.Default.GetString(Read_Buffer, 9, 4));
                        smoke.AVERAGERB = float.Parse(Encoding.Default.GetString(Read_Buffer, 13, 4));
                        return smoke;
                    }
                    else
                        return smoke;
                    break;


                case ffxh:
                    byte CHECKSUM = 0;
                    byte[] Content_FF = new byte[] { yqadd };
                    Content_FF[0] = yqadd;
                    ComPort_3.Parity = System.IO.Ports.Parity.Mark;
                    ComPort_3.Write(Content_FF, 0, 1);
                    Thread.Sleep(3);
                    CHECKSUM += Content_FF[0];
                    Content_FF[0] = cmdGetData;
                    ComPort_3.Parity = System.IO.Ports.Parity.Space;
                    ComPort_3.Write(Content_FF, 0, 1);
                    Thread.Sleep(3);
                    CHECKSUM += Content_FF[0];
                    Content_FF[0] = 1;
                    ComPort_3.Parity = System.IO.Ports.Parity.Space;
                    ComPort_3.Write(Content_FF, 0, 1);
                    Thread.Sleep(3);
                    CHECKSUM += Content_FF[0];
                    Content_FF[0] = CHECKSUM;
                    ComPort_3.Parity = System.IO.Ports.Parity.Space;
                    ComPort_3.Write(Content_FF, 0, 1);
                    Thread.Sleep(3);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 20)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x01)
                    {
                        smoke.Success = true;
                        smoke.FirstData = float.Parse(Encoding.Default.GetString(Read_Buffer, 1, 4));
                        smoke.SencondData = float.Parse(Encoding.Default.GetString(Read_Buffer, 5, 4));
                        smoke.ThirdData = float.Parse(Encoding.Default.GetString(Read_Buffer, 9, 4));
                        smoke.AVERAGERB = float.Parse(Encoding.Default.GetString(Read_Buffer, 13, 4));
                        return smoke;
                    }
                    else
                        return smoke;
                    break;
                default:
                    return smoke;
                    break;
            }
        }
        #endregion
        
    }
}