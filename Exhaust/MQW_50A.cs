using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace Exhaust
{
    public class Exhaust_Data
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
    }

    public class MQW_50A
    {
        //private System.ComponentModel.IContainer components = new System.ComponentModel.Container();
        public System.IO.Ports.SerialPort ComPort_1;
        byte[] Send_Buffer;                                     //发送缓冲区
        byte[] Read_Buffer;                                     //读取缓冲区
        bool Read_Flag = false;                                 //是否有数据可以读取

        #region 50A通讯协议
        byte DID = 0x02;                                            //数据头
        byte NAK = 0x15;                                            //命令错误，发送数据长度错误或者校验和错误

        byte Cmd_GetData = 0x60;                                    //取实时数据
        byte Cmd_GetStatus = 0x61;                                  //实时状态获取命令
        byte Cmd_Background = 0x62;                                 //背景空气测定命令
        byte Cmd_Milieu = 0x63;                                     //环境空气测定命令
        byte Cmd_Remain = 0x64;                                     //HC残留测量
        byte Cmd_Blowback_Auto = 0x65;                              //反吹（自动完成反吹）
        byte Cmd_Leak_Check = 0x66;                                 //泄露检查
        byte Cmd_Zeroing = 0x67;                                    //调零命令
        byte Cmd_Set_Cmd_Zeroing = 0x68;                            //设置调零气体命令
        byte Cmd_Demarcate = 0x69;                                  //尾气标定命令
        byte Cmd_Lock = 0x6A;                                       //锁定键盘操作命令
        byte Cmd_Unlock = 0x6B;                                     //解锁键盘操作命令
        byte Cmd_HC_Set = 0x6C;                                     //HC燃料设置命令
        byte Cmd_Ignition_Set = 0x6D;                               //设置点火数命令
        byte Cmd_Stroke_Set = 0x6E;                                 //设置冲程数命令
        byte Cmd_Getbackground = 0x6F;                              //背景空气数据获取
        byte Cmd_Getmilieu = 0x70;                                  //环境空气数据获取
        byte Cmd_HC_remain = 0x71;                                  //HC残留数据获取
        byte Cmd_Pressure_Demarcate = 0x72;                         //管路压力标定命令
        byte Cmd_Oil_Temperature_Demarcate = 0x73;                  //油温标定命令
        byte Cmd_Humidity_Demarcate = 0x75;                         //环境温度标定命令
        byte Cmd_Auto_Zeroing = 0x76;                               //自动调零设置命令
        byte Cmd_Restore_Factory_Settings = 0x77;                   //恢复出厂设置命令
        byte Cmd_Stop_Action_Check = 0x78;                          //停止当前动作检查命令
        byte Cmd_Start = 0x79;                                      //开始测量命令
        byte Cmd_Stop = 0x7A;                                       //停止测量命令
        byte Cmd_Pumping_Check = 0x7B;                              //抽气动作检查命令
        byte Cmd_Milieu_Pumping_Check = 0x7C;                       //抽环境空气动作检查命令
        byte Cmd_Nitrogen_Check = 0x7D;                             //通氮气动作检查命令
        byte Cmd_Standard_Check = 0x7E;                             //通校准气动作检查
        byte Cmd_Check_Check = 0x7F;                                //通检查气体检查命令
        byte Cmd_Blowback = 0x80;                                   //反吹命令
        byte Cmd_Milieu_Pressure_Demarcate = 0x86;                  //环境压力标定命令
        byte Cmd_Milieu_Temperature_Demarcate = 0x87;               //环境温度标定命令
        byte Cmd_Open_Screen = 0x88;                                //打开显示屏命令
        byte Cmd_Close_Screen = 0x89;                               //关闭显示屏命令
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
                ComPort_1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Ref_Readflag);
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

        #region 发送数据
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="Cmd">命令</param>
        /// <param name="Content">内容</param>
        public void SendData(byte Cmd, byte[] Content)
        {
            try
            {
                byte DID = 0x02;
                byte[] DF = Content;
                int temp = 0;
                byte LB = Convert.ToByte(3 + Content.Length);
                for (int i = 0; i < DF.Length; i++)
                    temp += DF[i];
                temp += DID + Cmd + LB;
                string tempstr1 = Convert.ToString(~temp + 1, 16);
                string tempstr2 = "";
                bool f_struct = false;
                for (int i = 0; i < tempstr1.Length; i++)
                {
                    if (tempstr1[i] != 'f')
                        f_struct = true;
                    if (f_struct)
                        tempstr2 += tempstr1[i];
                }
                byte[] temp_CS = BitConverter.GetBytes(Convert.ToInt32(tempstr2, 16));
                int count = 0;
                for (int i = 0; i < temp_CS.Length; i++)
                {
                    if (temp_CS[i] != 0)
                        count++;
                }
                byte[] CS = new byte[count];
                int flag = 0;
                for (int i = count - 1; i >= 0; i--)
                {
                    CS[flag] = temp_CS[i];
                    flag++;
                }
                Send_Buffer = new byte[3 + DF.Length + count];
                Send_Buffer[0] = DID;
                Send_Buffer[1] = Cmd;
                Send_Buffer[2] = LB;
                for (int i = 0; i < DF.Length; i++)
                    Send_Buffer[i + 3] = DF[i];
                for (int i = 0; i < CS.Length; i++)
                    Send_Buffer[i + 3 + DF.Length] = CS[i];
                ComPort_1.Write(Send_Buffer, 0, Send_Buffer.Length);
            }
            catch (Exception)
            {
                throw;
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
            Read_Buffer = new byte[2048];
            if (Read_Flag)
            {
                Read_Flag = false;
                ComPort_1.Read(Read_Buffer, 0, 2048);
            }
        }
        #endregion

        #region 仪器是否可以进行测量
        /// <summary>
        /// 仪器是否可以进行测量
        /// </summary>
        /// <returns>string 仪器状态</returns>
        public string Get_Struct()
        {
            ReadData();
            byte[] Content=new byte[]{};
            int i = 0;
            string msg="";
            if (!ComPort_1.IsOpen)      //串口出错
                return "串口出错";
            SendData(Cmd_GetStatus, Content);    //取废气分析仪状态
            Thread.Sleep(50);
            while (!Read_Flag)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return "仪器通讯失败";
            }
            ReadData();                     //读取返回的数据
            //第一字节数据
            if ((Convert.ToInt32(Read_Buffer[3]) & 128) == 128)
                msg += "气路低流量 ";
            if((Convert.ToInt32(Read_Buffer[3]) & 64) == 64)
                msg += "正在调零 ";
            if ((Convert.ToInt32(Read_Buffer[3]) & 32) == 32)
                msg += "正在测定HC残留 ";
            if ((Convert.ToInt32(Read_Buffer[3]) & 16) == 16)
                msg += "正在检查泄漏 ";
            if ((Convert.ToInt32(Read_Buffer[3]) & 8) == 8)
                msg += "正在测定环境空气 ";
            if ((Convert.ToInt32(Read_Buffer[3]) & 4) == 4)
                msg += "正在测定背景空气 ";
            if ((Convert.ToInt32(Read_Buffer[3]) & 2) == 2)
                msg += "正在反吹 ";
            if ((Convert.ToInt32(Read_Buffer[3]) & 1) == 1)
                msg += "正在预热 ";

            //第二字节数据
            if ((Convert.ToInt32(Read_Buffer[4]) & 128) == 128)
                msg += "系统正在动作 ";
            if ((Convert.ToInt32(Read_Buffer[4]) & 64) == 64)
                msg += "已经开泵 ";
            if ((Convert.ToInt32(Read_Buffer[4]) & 32) == 32)
                msg += "正在标定湿度 ";
            if ((Convert.ToInt32(Read_Buffer[4]) & 16) == 16)
                msg += "正在标定环境温度 ";
            if ((Convert.ToInt32(Read_Buffer[4]) & 8) == 8)
                msg += "正在标定油温 ";
            if ((Convert.ToInt32(Read_Buffer[4]) & 4) == 4)
                msg += "正在标定环境压力 ";
            //if ((Convert.ToInt32(Read_Buffer[1]) & 2) == 2)
            //    msg += "调零方式为通氮调零 ";
            //if ((Convert.ToInt32(Read_Buffer[4]) & 1) == 1)
            //    msg += "等待调零 ";

            //第三字节数据
            if ((Convert.ToInt32(Read_Buffer[5]) & 128) == 128)
                msg += "油温标定失败 ";
            if ((Convert.ToInt32(Read_Buffer[5]) & 64) == 64)
                msg += "环境压力标定失败 ";
            if ((Convert.ToInt32(Read_Buffer[5]) & 32) == 32)
                msg += "平台压力标定失败 ";
            if ((Convert.ToInt32(Read_Buffer[5]) & 16) == 16)
                msg += "环境湿度标定失败 ";
            if ((Convert.ToInt32(Read_Buffer[5]) & 8) == 8)
                msg += "环境温度标定失败 ";
            if ((Convert.ToInt32(Read_Buffer[5]) & 4) == 4)
                msg += "调零失败 ";
            if ((Convert.ToInt32(Read_Buffer[5]) & 2) == 2)
                msg += "HC残留超标 ";
            if ((Convert.ToInt32(Read_Buffer[5]) & 1) == 1)
                msg += "泄露超标 ";
            
            //第四字节数据
            //if ((Convert.ToInt32(Read_Buffer[3]) & 128) == 128)
            //    msg += "冲程数 ";
            //if ((Convert.ToInt32(Read_Buffer[3]) & 64) == 64)
            //    msg += "点火数 ";
            //if ((Convert.ToInt32(Read_Buffer[3]) & 32) == 32)
            //    msg += "预留 ";
            if ((Convert.ToInt32(Read_Buffer[6]) & 16) == 16)
                msg += "正在标定O2 ";
            if ((Convert.ToInt32(Read_Buffer[6]) & 8) == 8)
                msg += "正在标定NO ";
            if ((Convert.ToInt32(Read_Buffer[6]) & 4) == 4)
                msg += "正在标定HC ";
            if ((Convert.ToInt32(Read_Buffer[6]) & 2) == 2)
                msg += "正在标定CO ";
            if ((Convert.ToInt32(Read_Buffer[6]) & 1) == 1)
                msg += "正在标定CO2 ";

            //第五字节
            //if ((Convert.ToInt32(Read_Buffer[4]) & 128) == 128)
            //    msg += "正在预热 ";
            //if ((Convert.ToInt32(Read_Buffer[4]) & 64) == 64)
            //    msg += "正在反吹 ";
            //if ((Convert.ToInt32(Read_Buffer[4]) & 32) == 32)
            //    msg += "正在测定背景空气 ";
            if ((Convert.ToInt32(Read_Buffer[7]) & 16) == 16)
                msg += "02标定失败 ";
            if ((Convert.ToInt32(Read_Buffer[7]) & 8) == 8)
                msg += "NO标定失败 ";
            if ((Convert.ToInt32(Read_Buffer[7]) & 4) == 4)
                msg += "HC标定失败 ";
            if ((Convert.ToInt32(Read_Buffer[7]) & 2) == 2)
                msg += "CO标定失败 ";
            if ((Convert.ToInt32(Read_Buffer[7]) & 1) == 1)
                msg += "CO2标定失败 ";
            if (msg == "")
                msg = "仪器已经准备好";
            return msg;
        }
        #endregion

        #region 开始检测
        /// <summary>
        /// 开始检测
        /// </summary>
        /// <returns>string</returns>
        public string Start()
        {
            ReadData();
            string Struct_Now=Get_Struct();
            //Zeroing();
            int i = 0;
            byte[] Content = new byte[] { };
            if (Struct_Now == "仪器已经准备好")
            {
                SendData(Cmd_Start, Content);        //发送开始测量命令
                Thread.Sleep(10);
                while (!Read_Flag)                          //等待仪器返回
                {
                    i++;
                    Thread.Sleep(10);
                    if (i == 100)
                        return "仪器通讯失败";
                }
                ReadData();                     //读取返回的数据
                if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x79 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x7E)
                    return "成功开始测量";
                else
                    return "仪器通讯失败";
            }
            else
                return Struct_Now;
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
            byte[] Content = new byte[] { };
            SendData(Cmd_Stop, Content);        //发送停止测量命令
            Thread.Sleep(10);
            while (!Read_Flag)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return false;
            }
            ReadData();                     //读取返回的数据
            if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x7A && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x7D)
                return true;
            else
                return false;
        }
        #endregion

        #region 停止检查
        /// <summary>
        /// 停止检查 停止当前的动作检查操作，在每启动一次动作检查之前需要向仪器发送此命令停止当前的动作检查操作。
        /// </summary>
        /// <returns>bool</returns>
        public bool Stop_Check()
        {
            ReadData();
            int i = 0;
            byte[] Content = new byte[] { };
            SendData(Cmd_Stop_Action_Check, Content);        //发送停止检查动作命令
            Thread.Sleep(10);
            while (!Read_Flag)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return false;
            }
            ReadData();                                 //读取返回的数据
            if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x78 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x7F)
                return true;
            else
                return false;
        }
        #endregion

        #region 设置自动调零
        /// <summary>
        /// 设置自动调零
        /// </summary>
        /// <returns>Bool</returns>
        public bool Auto_Zeroing()
        {
            ReadData();
            int i = 0;
            byte[] Content = new byte[] {0x0};
            SendData(Cmd_Auto_Zeroing, Content);        //发送自动调零命令
            Thread.Sleep(10);
            while (!Read_Flag)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return false;
            }
            ReadData();                                 //读取返回数据
            if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x76 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x81)
                return true;
            else
                return false;
                
        }
        #endregion

        #region 自动反吹
        /// <summary>
        /// 反吹(自动反吹过程大概需要30秒)
        /// </summary>
        /// <returns>bool</returns>
        public bool Auto_Blowback()
        {
            ReadData();
            int i = 0;
            byte[] Content = new byte[] { };
            //SendData(Cmd_Stop_Action_Check, Content);        //发送停止检查动作命令
            //Thread.Sleep(10);
            //while (!Read_Flag)                          //等待仪器返回
            //{
            //    i++;
            //    Thread.Sleep(10);
            //    if (i == 100)
            //        return false  ;
            //}
            //ReadData();                                 //读取返回的数据
            //if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x78 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x7F)
            //{
            SendData(Cmd_Blowback_Auto, Content);        //发送反吹命令
            Thread.Sleep(10);
            i = 0;
            while (!Read_Flag)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return false;
            }
            ReadData();                         //读取数据
            if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x80 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x77)
                return true;
            else
                return false;
            //}
            //else
            //    return false;
        }
        #endregion

        #region 反吹
        /// <summary>
        /// 反吹(反吹过程大概需要30秒 完成后须调用Stop_Check方法停止)
        /// </summary>
        /// <returns>bool</returns>
        public bool Blowback()
        {
            ReadData();
            int i = 0;
            byte[] Content = new byte[] { };
            //SendData(Cmd_Stop_Action_Check, Content);        //发送停止检查动作命令
            //Thread.Sleep(10);
            //while (!Read_Flag)                          //等待仪器返回
            //{
            //    i++;
            //    Thread.Sleep(10);
            //    if (i == 100)
            //        return false  ;
            //}
            //ReadData();                                 //读取返回的数据
            //if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x78 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x7F)
            //{
            SendData(Cmd_Blowback, Content);        //发送反吹命令
                Thread.Sleep(10);
                i = 0;
                while (!Read_Flag)                          //等待仪器返回
                {
                    i++;
                    Thread.Sleep(10);
                    if (i == 100)
                        return false;
                }
                ReadData();                         //读取数据
                if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x80 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x77)
                    return true;
                else
                    return false;
            //}
            //else
            //    return false;
        }
        #endregion

        #region 调零
        /// <summary>
        /// 调零(调零过程大概为30秒，可以通过Get_Struct方法检查是否调零成功)
        /// </summary>
        /// <returns>bool</returns>
        public bool Zeroing()
        {
            ReadData();
            int i = 0;
            byte[] Content = new byte[] { };
            //SendData(Cmd_Stop_Action_Check, Content);        //发送停止检查动作命令
            //Thread.Sleep(10);
            //while (!Read_Flag)                          //等待仪器返回
            //{
            //    i++;
            //    Thread.Sleep(10);
            //    if (i == 100)
            //        return false;
            //}
            //ReadData();                                 //读取返回的数据
            //if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x78 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x7F)
            //{
                SendData(Cmd_Zeroing, Content);        //发送调零命令
                Thread.Sleep(10);
                i = 0;
                while (!Read_Flag)                          //等待仪器返回
                {
                    i++;
                    Thread.Sleep(10);
                    if (i == 100)
                        return false;
                }
                ReadData();                         //读取数据
                if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x67 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x90)
                    return true;
                else
                    return false;
            //}
            //else
            //    return false;
        }
        #endregion

        #region 获取实时数据
        /// <summary>
        /// 获取实时数据 至少耗时500ms
        /// </summary>
        /// <returns>Exhaust_Data  如果值全为0则表明通讯失败</returns>
        public Exhaust_Data GetData()
        {
            ReadData();
            Exhaust_Data exhaust_data = new Exhaust_Data();
            exhaust_data.CO2 = 0;
            exhaust_data.CO = 0;
            exhaust_data.HC = 0;
            exhaust_data.NO = 0;
            exhaust_data.O2 = 0;
            exhaust_data.SD = 0;
            exhaust_data.YW = 0;
            exhaust_data.HJWD =0;
            exhaust_data.ZS = 0;
            exhaust_data.QLYL =0;
            exhaust_data.λ = 0;
            exhaust_data.HJYL = 0;
            int i = 0;
            byte[] Content = new byte[] { };
            SendData(Cmd_GetData, Content);
            Thread.Sleep(500);
            while (!Read_Flag)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return exhaust_data;
            }
            ReadData();
            if (Read_Buffer[0] == 0x6 && Read_Buffer[1] == 0x60 && Read_Buffer[2] == 0x1B)
            {
                byte[] temp_byte=new byte[2];
                temp_byte[0] = Read_Buffer[4];
                temp_byte[1] = Read_Buffer[3];
                exhaust_data.CO2 = BitConverter.ToInt16(temp_byte, 0)/100f;         //二氧化碳
                temp_byte[0] = Read_Buffer[6];
                temp_byte[1] = Read_Buffer[5];
                exhaust_data.CO = BitConverter.ToInt16(temp_byte, 0) / 100f;;       //一氧化碳
                temp_byte[0] = Read_Buffer[8];
                temp_byte[1] = Read_Buffer[9];
                exhaust_data.HC = BitConverter.ToInt16(temp_byte, 0);               //碳氢
                temp_byte[0] = Read_Buffer[10];
                temp_byte[1] = Read_Buffer[9];
                exhaust_data.NO = BitConverter.ToInt16(temp_byte, 0);               //一氧化氮
                temp_byte[0] = Read_Buffer[12];
                temp_byte[1] = Read_Buffer[11];
                exhaust_data.O2 = BitConverter.ToInt16(temp_byte, 0)/100f;          //氧气
                temp_byte[0] = Read_Buffer[14];
                temp_byte[1] = Read_Buffer[13];
                exhaust_data.SD = BitConverter.ToInt16(temp_byte, 0) / 10f;         //湿度
                temp_byte[0] = Read_Buffer[16];
                temp_byte[1] = Read_Buffer[15];
                exhaust_data.YW = BitConverter.ToInt16(temp_byte, 0) / 10f;         //油温
                temp_byte[0] = Read_Buffer[18];
                temp_byte[1] = Read_Buffer[17];
                exhaust_data.HJWD = BitConverter.ToInt16(temp_byte, 0) / 10f;       //环境温度
                temp_byte[0] = Read_Buffer[20];
                temp_byte[1] = Read_Buffer[19];
                exhaust_data.ZS = BitConverter.ToInt16(temp_byte, 0);               //转速
                temp_byte[0] = Read_Buffer[22];
                temp_byte[1] = Read_Buffer[21];
                exhaust_data.QLYL = BitConverter.ToInt16(temp_byte, 0)/10f;         //油路压力 
                temp_byte[0] = Read_Buffer[24];
                temp_byte[1] = Read_Buffer[23];
                exhaust_data.λ = BitConverter.ToInt16(temp_byte, 0) / 1000f;       //燃空比λ
                temp_byte[0] = Read_Buffer[26];
                temp_byte[1] = Read_Buffer[25];
                exhaust_data.HJYL = BitConverter.ToInt16(temp_byte, 0) / 10f;       //环境压力 kpa
            }
            return exhaust_data;
        }
        #endregion
        


        public Exhaust_Data Getdata_MQ411()
        {
            Exhaust_Data exhaust_data = new Exhaust_Data();
            Random rd=new Random();
            exhaust_data.HC = 0;
            exhaust_data.CO = 0;
            exhaust_data.NO = 0; 
            exhaust_data.CO2 = 0;
            exhaust_data.λ = 0;
            exhaust_data.HC = rd.Next(15, 30);
            exhaust_data.CO = rd.Next(5, 10) / 10f;
            exhaust_data.NO = rd.Next(15, 30);
            exhaust_data.CO2 = rd.Next(5, 25) / 10f;
            exhaust_data.λ = rd.Next(94, 113) / 100f;
            return exhaust_data;
        }

        public double Getdata_MQ511()
        {
            Random rd=new Random();
            return rd.Next(1, 35) / 10;
        }

        public double Getdata_BTG()
        {
            //ComPort_1 = new System.IO.Ports.SerialPort(this.components);
            Random rd = new Random();
            return rd.Next(1, 35)/10.0;
            //ComPort_1.BaudRate = 4800;

        }
    }
}
