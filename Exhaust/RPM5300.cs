using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace Exhaust
{
    public class RPM5300
    {
        public System.IO.Ports.SerialPort ComPort_2;
        public byte[] Send_Buffer;                                      //发送缓冲区
        public byte[] Read_Buffer;                                      //读取缓冲区
        public static string UseMK = "BNTD";                            //使用什么模块
        public string All_Content = "";                                   //没有解析过的返回数据
        public List<byte> All_Content_byte = new List<byte>();            //没有解析过的返回数据
        
        public string Msg = "";                                         //消息
        public string Msg_received = "";                                         //消息

        public double ZS = 0;

        public Thread readThread = null;
        static bool _continue = false;
        public bool msg_back = false;
        //以下为新成BNTD专用变量
        
        //public string Duty = "0.0";                                     //占空比(BNTD)

        bool Read_Flag = false;                                         //是否有数据可以读取
        private Thread Th_Lifter_Up = null;                             //举升上升线程
        public static Thread Th_Resolve = null;                         //解析线程
        public string Status = "time";                                  //IGBT状态 time——实时 Speed——恒速控制 Force——恒扭矩 Power——恒功率 Demarcate——标定 T——取环境参数(BNTD) F——取标定系数(BNTD) s——取速度PID控制参数(BNTD) f——取力PID控制参数(BNTD) S——取速度系数(BNTD) null——空闲
        public double sum = 0;

        #region BNTD通讯协议
        string ZJM_100_SetLeft = "ZSL";
        string ZJM_100_SetRight = "ZSR";
        string ZJM_100_ClearLeft = "ZCL";
        string ZJM_100_ClearRight = "ZCR";
        string ZJM_100_ClearKey = "ZCK";
        string ZJM_100_SetIO = "ZSO";
        string ZJM_100_SetMode = "ZSM";

        string GAB_100_RUN = "XSR";
        string GAB_100_STOP = "XSS";
        string GAB_100_RELAYON = "XSJ";
        string GAB_100_RELAYOFF = "XSD";
        string GAB_100_ClearKey = "XSK";

        private byte idleMode = 0x00;
        private byte testMode = 0x01;
        private byte SolidifyRom = 0x02;
        private byte ReadRom = 0x03;
        
        #endregion

        #region 构造函数
        public RPM5300()
        {
        }
        #endregion
        #region 关闭相应资源
        public void closeEquipment()
        {
            Stop_test();
            _continue = false;
            Thread.Sleep(100);
            try
            {
                readThread.Abort();
            }
            catch
            { }
            try
            {
                if (ComPort_2.IsOpen)
                    ComPort_2.Close();
            }
            catch
            { }
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
                readThread = new Thread(Resolve);
                //ComPort_2.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Read);
                //ComPort_2.ReceivedBytesThreshold =2;
                ComPort_2.Open();
                _continue = true;
                readThread.Start();

                //Th_Resolve = new Thread(Resolve);
                //Th_Resolve.Start();
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
                ComPort_2.Close();
                temp = true;
            }
            catch (Exception er)
            {
                throw er;
            }
            return temp;
        }
        #endregion

        #region 发送数据
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="Content">内容</param>
        public void SendData(byte[] Content)
        {
            try
            {
                byte[] Cmd_Temp = Content;
                int Sum = 0;
                foreach (byte b in Cmd_Temp)
                    Sum += b;
                Sum = Sum % 256;
                byte[] Cmd = new byte[Cmd_Temp.Length + 2];
                for (int i = 0; i < Cmd_Temp.Length; i++)
                    Cmd[i] = Cmd_Temp[i];
                Cmd[Cmd_Temp.Length] = Convert.ToByte(Sum);
                Cmd[Cmd_Temp.Length + 1] = 0x43;
                
                Send_Buffer = Cmd;
                //string sss = Encoding.Default.GetString(Send_Buffer);
                ComPort_2.Write(Send_Buffer, 0, Send_Buffer.Length);
            }
            catch (Exception)
            {
                //throw;
            }
        }
        #endregion

        #region 串口返回数据事件
        /// <summary>
        /// 当串口有返回数据事件时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Ref_Readflag(object sender, SerialDataReceivedEventArgs e)
        {
            ReadData();
            Resolve();
        }
        #endregion
        #region 接收数据
        /// <summary>
        /// 接收数据
        /// </summary>
        public bool ReadData()
        {
            try
            {
                //comportIsReading = true;
                if (ComPort_2.BytesToRead > 0)
                {
                    int read_buffer_length = ComPort_2.BytesToRead;
                    Read_Buffer = new byte[read_buffer_length];
                    ComPort_2.Read(Read_Buffer, 0, read_buffer_length);
                    List<byte> buffer = Read_Buffer.ToList();
                    All_Content_byte.AddRange(buffer.ToList());
                    //All_Content += Encoding.Default.GetString(buffer.ToArray());
                    return true;
                }
                else
                    return false;
                //comportIsReading = false;
            }
            catch (Exception)
            {
                ComPort_2.DiscardInBuffer();
                return false;
            }
        }
        #endregion

        #region 串口返回数据解析
        /// <summary>
        /// 串口返回数据解析
        /// </summary>
        private void Resolve()
        {
            while (_continue)
            {
                ReadData();
                if (All_Content_byte.Count >= 5)
                {
                    msg_back = false;
                    //float power = float.Parse(textBox_zsgl.Text);
                    try
                    {
                        if (All_Content_byte == null)
                        {
                            continue;
                        }
                        if (All_Content_byte.Count > 0)
                        {
                            int temp_start1 = 0;
                            int temp_start2 = 0;
                            //bool msg_back = false;
                            //temp_start1 = All_Content_byte.IndexOf(0x02);       //A
                            temp_start2 = All_Content_byte.IndexOf(0X0D);       //C
                            if (temp_start2<4)
                            {
                                All_Content_byte.RemoveRange(0, temp_start2+1);
                                continue;
                            }
                            if (temp_start2 == -1)
                            {
                                continue;
                            }
                            temp_start1 = temp_start2 - 4;
                            string sd = Encoding.Default.GetString(All_Content_byte.ToArray(), temp_start1, 4);
                            double zstemp = 0;
                            if (double.TryParse(sd, out zstemp))
                            {
                                ZS = zstemp;
                                All_Content_byte.RemoveRange(0, temp_start2 + 1);
                            }
                            else
                            {
                                All_Content_byte.RemoveRange(0, temp_start2 + 1); 
                            }                           
                        }
                        if (All_Content_byte.Count > 15)
                            All_Content_byte.RemoveRange(0, All_Content_byte.Count - 5);
                    }
                    catch (Exception)
                    {
                            All_Content_byte.Clear();
                    }
                }
                Thread.Sleep(60);
            }

        }
        #endregion

        #region 获取下位机返回到消息
        /// <summary>
        /// 获取下位机返回到消息
        /// </summary>
        /// <returns>string 消息 没消息的时候返回空字符串</returns>
        public string Get_Message()
        {
            string message = "";
            //message =  Read_Buffer
            return message;
        }
        #endregion

        #region 退出所有控制
        /// <summary>
        /// 退出所有控制
        /// </summary>
        public void Stop_test()
        {
            byte[] Cmd = null;
                    List<byte> temp_cmd = Encoding.Default.GetBytes(ZJM_100_SetMode).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0));
                    Cmd = temp_cmd.ToArray();
            SendData(Cmd);
        }
        #endregion

        
    }
}
