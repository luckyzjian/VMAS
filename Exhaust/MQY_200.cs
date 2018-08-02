using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace Exhaust
{
    public class Smoke
    {
        private float _btg;
        private float _gxsxs;
        private float _zs;
        private float _yw;

        /// <summary>
        /// 转速
        /// </summary>
        public float Zs
        {
            get { return _zs; }
            set { _zs = value; }
        }
        
        /// <summary>
        /// 油温
        /// </summary>
        public float Yw
        {
            get { return _yw; }
            set { _yw = value; }
        }

        /// <summary>
        /// 光吸收系数
        /// </summary>
        public float Gxsxs
        {
            get { return _gxsxs; }
            set { _gxsxs = value; }
        }

        /// <summary>
        /// 不透光N值
        /// </summary>
        public float Btg
        {
            get { return _btg; }
            set { _btg = value; }
        }
    }

    public class MQY_200
    {
        public System.IO.Ports.SerialPort ComPort_3;
        byte[] Send_Buffer;                                     //发送缓冲区
        byte[] Read_Buffer;                                     //读取缓冲区
        bool Read_Flag = false;                                 //是否有数据可以读取

        #region MQY_200通讯协议
        byte Mode_Preheat = 0x00;                               //仪器正在预热（非法）
        byte Mode_Real = 0x01;                                  //仪器正在进行实时测量（合法）
        byte Mode_Free = 0x02;                                  //仪器正在进行自由加速测量（合法）
        byte Mode_Other = 0xFF;                                 //选择测量方式
        byte Mode_Choice_Measuring = 0xA0;                      //仪器正在进行其他方式（非法）
        byte Mode_Get_Measuring = 0xA1;                         //取测量方式
        byte Mode_Calibration = 0xA2;                           //校准（实时测试方式）                                   
        byte Mode_Start_Test = 0xA3;                            //开始或继续自由加速试验
        byte Mode_End_Test = 0xA4;                              //结束或中止自由加速试验（联网）
        byte Mode_Get_TestState = 0xA5;                         //取自由加速试验状态（联网）
        byte Mode_Get_RealData = 0xA6;                          //取实时测试数据
        byte Mode_Get_TestData = 0xA7;                          //取自由加速试验数据（联网）
        byte Mode_Maximum_clear = 0xA8;                         //清除最大值
        byte Mode_Get_MaxData = 0xA9;                           //取最大值数据
        byte Mode_Exhaust = 0xAA;         



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
                ComPort_3.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Ref_Readflag);
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
        public void SendData(byte Cmd, byte[] Content)
        {
            //使用 WaitHandle 等待异步调用 
            byte Cmd_Content = Cmd; //命令
            byte[] DF = Content;
            int temp = 0;
            for (int i = 0; i < DF.Length; i++)
                temp += DF[i];
            temp += Cmd;
            string tempstr1 = "";
            tempstr1 = Convert.ToString(~int.Parse(temp.ToString()) + 1, 16); //取反 +1
            string tempstr2 = tempstr1.Substring(tempstr1.Length - 2, 2);//低八
            byte[] temp_CS = BitConverter.GetBytes(Convert.ToInt32(tempstr2, 16));
            //temp_CS[0]    校验码
            Send_Buffer = new byte[2 + DF.Length];
            Send_Buffer[0] = Cmd_Content;                   //加命令           
            for (int i = 0; i < DF.Length; i++)
                Send_Buffer[i + 1] = DF[i];                   //加内容
            Send_Buffer[1 + DF.Length] = temp_CS[0];          //加校验码
            ComPort_3.Write(Send_Buffer, 0, Send_Buffer.Length);
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
            Read_Buffer = new byte[2048];
            if (Read_Flag)
            {
                Read_Flag = false;
                ComPort_3.Read(Read_Buffer, 0, 2048);
            }
        }
        #endregion

        #region 取仪器测量方式
        /// <summary>
        /// 取仪器测量方式
        /// </summary>
        /// <returns>string （正在预热、正在实时测量、正在自由加速试验、其他）</returns>
        public string Get_Mode()
        {
            string msg = "其他";
            byte[] Content = new byte[] { };
            int i = 0;
            if (!ComPort_3.IsOpen)
                return "其他";
            SendData(Mode_Get_Measuring, Content);
            Thread.Sleep(10);
            while (!Read_Flag)
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return "";
            }
            ReadData();
            if (Read_Buffer.Length > 0)
                switch (Read_Buffer[1])
                {
                    case 0x00:
                        msg = "正在预热";
                        break;
                    case 0x01:
                        msg = "正在实时测量";
                        break;
                    case 0x02:
                        msg = "正在自由加速试验";
                        break;
                    case 0xff:
                        msg = "其他";
                        break;
                }
            return msg;
        }
        #endregion

        #region 进入实时测量状态
        /// <summary>
        /// 进入实时测量状态
        /// </summary>
        /// <returns>bool</returns>
        public bool Set_Measure()
        {
            string Measurement = Get_Mode();
            int i = 0;
            byte[] Content = new byte[] {0x01};
            SendData(Mode_Choice_Measuring, Content);
                while (!Read_Flag)                          //等待仪器返回
                {
                    i++;
                    Thread.Sleep(10);
                    if (i == 100)
                        return false;
                }
                ReadData();
                if (Read_Buffer.Length > 0)
                {
                    if (Read_Buffer[0] == 0xA0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
        }
        #endregion

        #region 仪器校准
        /// <summary>
        /// 仪器校准
        /// </summary>
        /// <returns>bool</returns>
        public bool Calibration()
        {
            byte[] Content = new byte[] { };
            int i = 0;
            SendData(Mode_Calibration, Content);
            while (!Read_Flag)
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return false;
            }
            ReadData();
            if (Read_Buffer.Length > 0)
            {
                if (Read_Buffer[0] == 0xA2)
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 取仪器测量数据
        public Smoke Get_Data()
        {
            Smoke smoke = new Smoke();
            smoke.Btg = 0;
            smoke.Gxsxs = 0;
            smoke.Yw = 0;
            smoke.Zs = 0;
            try
            {
                int i = 0;
                byte[] Content = new byte[] { };
                    SendData(Mode_Get_RealData, Content);
                    while (!Read_Flag)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer.Length > 0)
                    {
                        if (Read_Buffer[0] == 0xA6)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[2];
                            temp_byte[1] = Read_Buffer[1];
                            smoke.Btg = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            smoke.Gxsxs = BitConverter.ToInt16(temp_byte, 0) / 100f;
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            smoke.Zs = BitConverter.ToInt16(temp_byte, 0);
                            temp_byte[0] = Read_Buffer[8];
                            temp_byte[1] = Read_Buffer[7];
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0)-273;
                            //smoke.Yw = (Convert.ToInt32(Read_Buffer[7]) * 256 + Convert.ToInt32(Read_Buffer[8])) - 273;
                        }

                        else if (Read_Buffer[0] == 0x15 || Read_Buffer[0] == 0xeb)
                        {
                            return smoke;
                        }
                        else
                        {
                            return smoke;
                        }
                    }
                    else
                    {
                        return smoke;
                    }         
            }
            catch (Exception)
            {
                return smoke;
            }
            return smoke;
        }
        #endregion

        #region 模拟实时数据
        /// <summary>
        /// 模拟实时数据
        /// </summary>
        /// <returns>Smoke</returns>
        public Smoke Get_Data_Imitate()
        {
            Smoke smoke = new Smoke();
            smoke.Btg = 0.3f;
            smoke.Gxsxs = 0.4f;
            smoke.Yw = 56;
            smoke.Zs = 3000;
            
            return smoke;
        }
        #endregion
    }
}