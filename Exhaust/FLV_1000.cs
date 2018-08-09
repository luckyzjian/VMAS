using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace Exhaust
{
    public class Flv_1000_data
    {

        private float xso2 = 0.0f;

        public float Xso2
        {
            get { return xso2; }
            set { xso2 = value; }
        }
        private float yl = 0.0f;

        public float Yl
        {
            get { return yl; }
            set { yl = value; }
        }
        private float temp = 0.0f;

        public float Temp
        {
            get { return temp; }
            set { temp = value; }
        }
        private float fbzll = 0.0f;

        public float Fbzll
        {
            get { return fbzll; }
            set { fbzll = value; }
        }
        private float bzll = 0.0f;

        public float Bzll
        {
            get { return bzll; }
            set { bzll = value; }
        }
        private float fbzo2 = 0.0f;

        public float Fbzo2
        {
            get { return fbzo2; }
            set { fbzo2 = value; }
        }
        private float fbzyl = 0.0f;

        public float Fbzyl
        {
            get { return fbzyl; }
            set { fbzyl = value; }
        }
        private float fbztemp = 0.0f;

        public float Fbztemp
        {
            get { return fbztemp; }
            set { fbztemp = value; }
        }
        private float bzo2 = 0.0f;

        public float Bzo2
        {
            get { return bzo2; }
            set { bzo2 = value; }
        }
        private float bzyl = 0.0f;

        public float Bzyl
        {
            get { return bzyl; }
            set { bzyl = value; }
        }
        private float bztemp = 0.0f;

        public float Bztemp
        {
            get { return bztemp; }
            set { bztemp = value; }
        }
        private byte status = 0;

        public byte Status
        {
            get { return status; }
            set { status = value; }
        }

    }


    public class Flv_1000
    {
        private string yqxh = "flv_1000";
        public bool isNhSelfUse = false;
        public Flv_1000(string xh)
        {
            yqxh = xh.ToLower();
        }
        public Flv_1000()
        { }
        //private System.ComponentModel.IContainer components = new System.ComponentModel.Container();
        public System.IO.Ports.SerialPort ComPort_1;
        byte[] Send_Buffer;                                     //发送缓冲区
        byte[] Read_Buffer;                                     //读取缓冲区
        bool Read_Flag = false;                                 //是否有数据可以读取

        #region flv_1000通讯协议

        byte nak = 0x15;//命令错误，发送数据长度错误或者校验和错误

        byte getXishiO2 = 0x81;
        byte getYali = 0x82;
        byte getTemp = 0x83;
        byte getUnstandardLl = 0x84;
        byte getStandardLl = 0x85;
        byte getUnstandardDat = 0x86;
        byte getStandardDat = 0x87;
        byte getStatus = 0x88;
        byte SetXishiO2Standard = 0xa0;
        byte xishiO2_low_demarcate = 0xa1;
        byte xishiO2_high_demarcate = 0xa2;
        byte yali_demarcate = 0xa3;
        byte temp_demarcate = 0xa4;
        byte Set_default_settings = 0xac;

        byte getXishiO2_MQ = 0x81;
        byte getYali_MQ = 0x82;
        byte getTemp_MQ = 0x83;
        byte getUnstandardLl_MQ = 0x84;
        byte getStandardLl_MQ = 0x85;
        byte getUnstandardDat_MQ = 0x86;
        byte getStandardDat_MQ = 0x87;
        byte getStatus_MQ = 0x88;
        byte SetXishiO2Standard_MQ = 0xa1;

        byte[] getNHStandardData = { 0x01, 0x03,0x12, 0x58,0x00, 0x05 };
        byte[] getNHStandardData_selfuse = { 0x37, 0x00, 0x00, 0x00 };
        byte[] getNHStatus = { 0x01, 0x03,0x12, 0x5D, 0x00, 0x01 };
        byte[] getNHStatus_selfuse = { 0x32, 0x00, 0x01, 0x00, 0x01, 0x00 };
        byte[] turnOnNHMotor = { 0x01, 0x06, 0x12, 0x5E, 0x00, 0x01 };
        byte[] turnOffNHMotor = { 0x01, 0x06, 0x12, 0x5E, 0x00, 0x00 };
        byte[] turnOnNHMotor_selfuse = { 0x38, 0x00, 0x02, 0x00, 0x01, 0x00,0x10,0x00 };
        byte[] turnOffNHMotor_selfuse = { 0x38, 0x00, 0x02, 0x00, 0x01, 0x00, 0x00,0x00 };
        byte[] setNHO2zero = { 0x01, 0x06, 0x12, 0x5F, 0x00, 0x01 };
        byte[] setNHQtylSingleDemarcate = { 0x01, 0x06, 0x12, 0x60, 0x00, 0x00 };//后两个数据为标定的压力值，单位为0.01kPa,如校准值为101.33kPa,则发送2795
        byte[] setNHQtylSingleDemarcate_selfuse = { 0x35, 0x00, 0x04, 0x00, 0x01, 0x00,0x02,0x00,0x00,0x00,0xff,0xff};//后两个数据为标定的压力值，单位为0.01kPa,如校准值为101.33kPa,则发送2795
        byte[] setNHTempSingleDemarcate = { 0x01, 0x06, 0x12, 0x61, 0x00, 0x00 };//后两个数据为标定的气体温度，单位为0.1°C,如校准值为10.4°C,则发送0068
        byte[] setNHFlowHighDemarcate = { 0x01, 0x06, 0x12, 0x62, 0x00, 0x00 };//后两个数据为标定的标准流量(须小于390CFM)，单位为CFM(立方英尺/分钟),如校准值为300CCFM,则发送012C
        byte[] setNHFlowLowDemarcate = { 0x01, 0x06, 0x12, 0x63, 0x00, 0x00 };//后两个数据为标定的标准流量(须小于高量程)，单位为CFM(立方英尺/分钟),如校准值为300CCFM,则发送012C
        byte[] setNHFlowSingleDemarcate = { 0x01, 0x06, 0x12, 0x64, 0x00, 0x00 };//后两个数据为标定的标准流量，单位为CFM(立方英尺/分钟),如校准值为300CCFM,则发送012C
        byte[] setNHO2HighDemarcate = { 0x01, 0x06, 0x12, 0x65, 0x00, 0x00 };//后两个数据为标定的O2浓度，单位为0.01%,如校准值为20.8%,则发送0820
        byte[] setNHO2MidDemarcate = { 0x01, 0x06, 0x12, 0x66, 0x00, 0x00};//后两个数据为标定的O2浓度，单位为0.01%,如校准值为20.8%,则发送0820
        byte[] setNHO2LowDemarcate = { 0x01, 0x06, 0x12, 0x67, 0x00, 0x00 };//后两个数据为标定的O2浓度，单位为0.01%,如校准值为20.8%,则发送0820
        byte[] setNHRestDefault = { 0x01, 0x06, 0x12, 0x6C, 0x00, 0x01 };

        byte[] asciiTable = { 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46 };
        public float xishiO2_density = 0.0f;
        public float o2_standard_value = 0.0f;
        public float yali_standard_value = 0.0f;
        public float temp_standard_value = 0.0f;
        public float ll_standard_value = 0.0f;
        public float o2_unstandard_value = 0.0f;
        public float yali_unstandard_value = 0.0f;
        public float temp_unstandard_value = 0.0f;
        public float ll_unstandard_value = 0.0f;
        public float temp_value = 0.0f;
        public float yali_value = 0.0f;

        #endregion

        #region 流量计串口初始化
        /// <summary>
        /// 流量计串口初始化
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
                //byte DID = 0x02;
                byte[] DF = Content;
                //int temp = 0;
                byte[] CS = new byte[DF.Length + 2];
                CS[0] = Cmd;
                //byte LB = Convert.ToByte(3 + Content.Length);
                CS[1] = DF[0];
                CS[2] = DF[1];
                CS[3] = 0;
                ComPort_1.Write(CS, 0, CS.Length);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region 发送数据
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="Cmd">命令</param>
        /// <param name="Content">内容</param>
        public void SendDataOfNHF(byte[] Content)
        {
            try
            {
                //byte DID = 0x02;
                byte[] DF = Content;
                //int temp = 0;
                byte[] CS = new byte[DF.Length + 1];
                for (int i = 0; i < DF.Length; i++)
                {
                    CS[i] = DF[i];
                }
                CS[DF.Length] = getCS_MQ(DF, DF.Length);
                byte[] arraySend = new byte[CS.Length * 2 + 3];
                arraySend[0] = 0x3a;
                arraySend[CS.Length * 2 + 1] = 0x0d;
                arraySend[CS.Length * 2 + 2] = 0x0a;
                for (int i = 0; i < CS.Length; i++)
                {
                    arraySend[i* 2 + 1] = asciiTable[CS[i] / 16];
                    arraySend[i* 2 + 2] = asciiTable[CS[i] % 16];
                }
                ComPort_1.Write(arraySend, 0, arraySend.Length);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region 南华自用流量计校验方式发送数据 
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="Cmd">命令</param>
        /// <param name="Content">内容</param>
        public void SendDataOfNHF2(byte[] Content)
        {
            try
            {
                //byte DID = 0x02;
                byte[] DF = Content;
                //int temp = 0;
                byte[] CS = new byte[DF.Length + 2];
                short CS2 = 0;
                for (int i = 0; i < DF.Length; i++)
                {
                    CS[i] = DF[i];
                    CS2 += CS[i];
                }
                CS[DF.Length] =(byte)(CS2);
                CS[DF.Length+1] = (byte)(CS2 >> 8);                
                ComPort_1.Write(CS, 0, CS.Length);
            }
            catch (Exception)
            {
                throw;
            }
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

        #region 仪器是否可以进行测量
        /// <summary>
        /// 仪器是否可以进行测量
        /// </summary>
        /// <returns>string 仪器状态</returns>
        public string Get_Struct()
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return "仪器通讯失败";
            switch (yqxh)
            {
                case "flv_1000":
                    byte[] Content = new byte[] { 0x02, 0x07 };
                    SendData(getStatus, Content);
                    //ComPort_1.Write(Content, 0, 1);
                    //SendData(Cmd_GetStatus, Content);    //取废气分析仪状态
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return "仪器通讯失败";
                    }
                    ReadData();                     //读取返回的数据
                    //[06](HC)(CO)(CO2)(O2)(NO)(转速)(油温)(流量)[Status](lamda)[CS]
                    Status = Convert.ToByte(Read_Buffer[3] & 0xff);
                    if (Status == 0x00)
                        msg = "流量计已经准备好";
                    else
                    {
                        if ((Status & 0x01) == 0x01)
                            msg += "流量处于超量程,";
                        else
                            msg += "流量测量正常,";
                        if ((Status & 0x04) == 0x04)
                            msg += "气体压力不正常。";
                        else
                            msg += "气体压力正常。";
                        if ((Status & 0x08) == 0x08)
                            msg += "气体温度不正常,";
                        else
                            msg += "气体温度正常,";
                        if ((Status & 0x10) == 0x10)
                            msg += "氧化锆正在预热。";
                        else
                            msg += "氧化锆预热完毕。";
                    }

                    return msg;
                    break;
                case "mql_100":
                    byte[] Content_MQ = new byte[] { 0x02, 0x88, 0x03, 0x73 };
                    //ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                    ComPort_1.Write(Content_MQ, 0, 4);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 8)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return "仪器通讯失败";
                    }
                    ReadData();                     //读取返回的数据
                    //[06](HC)(CO)(CO2)(O2)(NO)(转速)(油温)(流量)[Status](lamda)[CS]
                    Status = Convert.ToByte(Read_Buffer[3] & 0xff);
                    if (Status == 0x00)
                        msg = "流量计已经准备好";
                    else
                    {
                        if ((Status & 0x01) == 0x01)
                            msg += "流量处于超量程,";
                        else
                            msg += "流量测量正常,";
                        if ((Status & 0x04) == 0x04)
                            msg += "气体压力不正常。";
                        else
                            msg += "气体压力正常。";
                        if ((Status & 0x08) == 0x08)
                            msg += "气体温度不正常,";
                        else
                            msg += "气体温度正常,";
                        if ((Status & 0x10) == 0x10)
                            msg += "氧化锆正在预热。";
                        else
                            msg += "氧化锆预热完毕。";
                    }

                    return msg;
                    break;
                case "nhf_1":
                    if (isNhSelfUse)
                    {
                        SendDataOfNHF2(getNHStatus_selfuse);
                        Thread.Sleep(30);
                        while (ComPort_1.BytesToRead < 8)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(2);
                            if (i == 100)
                                return "仪器通讯失败";
                        }
                        ReadData();                     //读取返回的数据
                                                        //[06](HC)(CO)(CO2)(O2)(NO)(转速)(油温)(流量)[Status](lamda)[CS]
                        if (Read_Buffer[0]==0x32&& Read_Buffer[1] == 0x06&&Read_Buffer[2] == 0x01)
                            return "流量计已经准备好";
                        else
                            return "氧化锆正在预热";
                    }
                    else
                    {
                        SendDataOfNHF(getNHStatus);
                        Thread.Sleep(30);
                        while (ComPort_1.BytesToRead < 15)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(2);
                            if (i == 100)
                                return "仪器通讯失败";
                        }
                        ReadData();                     //读取返回的数据
                                                        //[06](HC)(CO)(CO2)(O2)(NO)(转速)(油温)(流量)[Status](lamda)[CS]
                        Status = Convert.ToByte(Read_Buffer[10] & 0xff);
                        if (Status >= 0x30 && Status <= 0x39)
                        {
                            Status = (byte)(Status - 0x30);
                        }
                        else if (Status >= 'A' && Status <= 'F')
                        {
                            Status = (byte)(Status - 'A' + 10);
                        }
                        else if (Status >= 'a' && Status <= 'f')
                        {
                            Status = (byte)(Status - 'a' + 10);
                        }
                        else
                        {
                            return "仪器通讯失败";
                        }
                        if ((Status & 0x04) == 0x00)
                            return "流量处于超量程";
                        else if ((Status & 0x01) == 0x01)
                            return "氧化锆正在预热";
                        else
                            return "流量计已经准备好";
                    }
                    break;
                default:
                    return "未提供该型号相关功能";
                    break;
            }

        }
        #endregion

        #region 取稀释氧浓度值
        /// <summary>
        /// 取稀释氧浓度值
        /// </summary>
        /// <returns>string</returns>
        public string Get_xishiO2()
        {
            ReadData();
            int i = 0;
            byte CS = 0;
            switch (yqxh)
            {
                case "flv_1000":
                    byte[] Content = new byte[] { 0x02, 0x7b };
                    //ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                    SendData(getXishiO2, Content);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 5; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[5] == CS)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            xishiO2_density = BitConverter.ToInt16(temp_byte, 0) / 100f;
                            return "获取成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                case "mql_100":
                    byte[] Content_MQ = new byte[] { 0x02, 0x81, 0x03, 0x7a };
                    //ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                    ComPort_1.Write(Content_MQ, 0, 4);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 6)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 5; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[5] == CS)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            xishiO2_density = BitConverter.ToInt16(temp_byte, 0) / 100f;
                            return "获取成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                default:
                    return "未提供该型号相关功能";
                    break;
            }
            // return "仪器通讯失败";

        }
        #endregion
        #region 取压力值
        /// <summary>
        /// 取压力值
        /// </summary>
        /// <returns>string</returns>
        public string Get_Yali()
        {
            ReadData();
            int i = 0;
            byte CS = 0;
            switch (yqxh)
            {
                case "flv_1000":
                    byte[] Content = new byte[] { 0x02, 0x7a };
                    //ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                    SendData(getYali, Content);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 6)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 5; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[5] == CS)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            yali_value = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            return "获取成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                case "mql_100":
                    byte[] Content_MQ = new byte[] { 0x02, 0x82, 0x03, 0x79 };
                    //ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                    ComPort_1.Write(Content_MQ, 0, 4);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 6)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 5; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[5] == CS)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            yali_value = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            return "获取成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                default:
                    return "未提供该型号相关功能";
                    break;
            }

            //  return "仪器通讯失败";

        }
        #endregion
        #region 取温度值
        /// <summary>
        /// 取温度值
        /// </summary>
        /// <returns>string</returns>
        public string Get_Temperature()
        {
            ReadData();
            int i = 0;
            byte CS = 0;
            switch (yqxh)
            {
                case "flv_1000":
                    byte[] Content = new byte[] { 0x02, 0x79 };
                    //ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                    SendData(getTemp, Content);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 6)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 5; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[5] == CS)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            temp_value = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            return "获取成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                case "mql_100":
                    byte[] Content_MQ = new byte[] { 0x02, 0x83, 0x03, 0x78 };
                    //ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                    ComPort_1.Write(Content_MQ, 0, 4);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 6)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 5; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[5] == CS)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            temp_value = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            return "获取成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                default:
                    return "未提供该型号相关功能";
                    break;
            }

            // return "仪器通讯失败";

        }
        #endregion

        #region 取非标准状态流量值
        /// <summary>
        /// 取非标准状态流量值
        /// </summary>
        /// <returns>string</returns>
        public string Get_unstandardLl()
        {
            ReadData();
            int i = 0;
            byte CS = 0;
            switch (yqxh)
            {
                case "flv_1000":
                    byte[] Content = new byte[] { 0x02, 0x78 };
                    //ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                    SendData(getUnstandardLl, Content);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 6)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 5; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[5] == CS)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            ll_unstandard_value = BitConverter.ToInt16(temp_byte, 0) / 100f;
                            return "获取成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                case "mql_100":
                    byte[] Content_MQ = new byte[] { 0x02, 0x84, 0x03, 0x77 };
                    //ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                    ComPort_1.Write(Content_MQ, 0, 4);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 6)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 5; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[5] == CS)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            ll_unstandard_value = BitConverter.ToInt16(temp_byte, 0) / 6f;
                            return "获取成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                default:
                    return "未提供该型号相关功能";
                    break;
            }

            // return "仪器通讯失败";

        }
        #endregion
        #region 取标准状态流量值
        /// <summary>
        /// 取标准状态流量值
        /// </summary>
        /// <returns>string</returns>
        public string Get_standardLl()
        {
            ReadData();
            int i = 0;
            byte CS = 0;
            switch (yqxh)
            {
                case "flv_1000":
                    byte[] Content = new byte[] { 0x02, 0x77 };
                    //ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                    SendData(getStandardLl, Content);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 6)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 5; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[5] == CS)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            ll_standard_value = BitConverter.ToInt16(temp_byte, 0) / 100f;
                            return "获取成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                case "mql_100":
                    byte[] Content_MQ = new byte[] { 0x02, 0x85, 0x03, 0x76 };
                    //ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                    ComPort_1.Write(Content_MQ, 0, 4);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 6)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 5; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[5] == CS)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            ll_standard_value = BitConverter.ToInt16(temp_byte, 0) / 6f;
                            return "获取成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                case "nhf_1":
                    if (isNhSelfUse)
                    {
                        SendDataOfNHF2(getNHStandardData_selfuse);
                        Thread.Sleep(30);
                        while (ComPort_1.BytesToRead < 58)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
                                return "通讯故障";
                        }
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[0] == 0x37)
                        { 
                            o2_standard_value =(float)(Read_Buffer[45]*256+ Read_Buffer[44]) * 0.01f;
                            temp_standard_value = (float)(Read_Buffer[47] * 256 + Read_Buffer[46]) * 0.1f;
                            yali_standard_value = (float)(Read_Buffer[49] * 256 + Read_Buffer[48]) * 0.01f;
                            ll_unstandard_value = (float)(Math.Round((Read_Buffer[51] * 256 + Read_Buffer[50]) * 28.3168f / 60f,2));
                            ll_standard_value = (float)(Math.Round((ll_unstandard_value* yali_standard_value/100.325*273.15/(temp_standard_value+273.15)),2));
                            return "获取成功";//HC
                        }
                        else
                            return "命令不正常";
                    }
                    else
                    {
                        
                            SendDataOfNHF(getNHStandardData);
                            Thread.Sleep(30);
                            while (ComPort_1.BytesToRead < 31)                          //等待仪器返回
                            {
                                i++;
                                Thread.Sleep(10);
                                if (i == 200)
                                    return "通讯故障";
                            }
                            ReadData();                     //读取返回的数据
                            if (Read_Buffer[3] == 0x38)
                                return "命令不正常";
                            else
                            {
                                try
                                {
                                    string datastring = Encoding.Default.GetString(Read_Buffer, 9, 4);
                                    o2_standard_value = Convert.ToInt16(datastring, 16) * 0.01f;
                                    datastring = Encoding.Default.GetString(Read_Buffer, 13, 4);
                                    temp_standard_value = Convert.ToInt16(datastring, 16) * 0.1f;
                                    datastring = Encoding.Default.GetString(Read_Buffer, 17, 4);
                                    yali_standard_value = Convert.ToInt16(datastring, 16) * 0.01f;
                                    datastring = Encoding.Default.GetString(Read_Buffer, 21, 4);
                                    ll_unstandard_value = Convert.ToInt16(datastring, 16) * 28.3168f / 60f;
                                    datastring = Encoding.Default.GetString(Read_Buffer, 25, 4);
                                    ll_standard_value = Convert.ToInt16(datastring, 16) * 28.3168f / 60f;
                                    return "获取成功";//HC
                                }
                                catch
                                {
                                    return "通讯故障";

                                }
                            }
                    }
                    break;
                default:
                    return "未提供该型号相关功能";
                    break;
            }

            //return "仪器通讯失败";

        }
        #endregion

        #region 取非标准状态所有数据值
        /// <summary>
        /// 取非标准状态所有数据值
        /// </summary>
        /// <returns>string</returns>
        public string Get_unstandardDat()
        {
            ReadData();
            int i = 0;
            byte CS = 0;
            switch (yqxh)
            {
                case "flv_1000":
                    byte[] Content = new byte[] { 0x02, 0x77 };
                    //ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                    SendData(getUnstandardDat, Content);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 12)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 3; i < 11; i++)
                            CS += Read_Buffer[i];
                        //CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[11] == CS)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            o2_unstandard_value = BitConverter.ToInt16(temp_byte, 0) / 100f;
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            yali_unstandard_value = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[8];
                            temp_byte[1] = Read_Buffer[7];
                            temp_unstandard_value = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[10];
                            temp_byte[1] = Read_Buffer[9];
                            ll_unstandard_value = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            return "获取成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                case "mql_100":
                    byte[] Content_MQ = new byte[] { 0x02, 0x86, 0x03, 0x75 };
                    //ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                    ComPort_1.Write(Content_MQ, 0, 4);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 14)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 13; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[13] == CS)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            o2_unstandard_value = BitConverter.ToInt16(temp_byte, 0) / 100f;
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            yali_unstandard_value = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[8];
                            temp_byte[1] = Read_Buffer[7];
                            temp_unstandard_value = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[10];
                            temp_byte[1] = Read_Buffer[9];
                            ll_unstandard_value = BitConverter.ToInt16(temp_byte, 0) / 6f;
                            temp_byte[0] = Read_Buffer[12];
                            temp_byte[1] = Read_Buffer[11];
                            ll_standard_value = BitConverter.ToInt16(temp_byte, 0) / 6f;
                            return "获取成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                case "nhf_1":
                    if (isNhSelfUse)
                    {
                        SendDataOfNHF2(getNHStandardData_selfuse);
                        Thread.Sleep(30);
                        while (ComPort_1.BytesToRead < 58)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
                                return "通讯故障";
                        }
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[0] == 0x37)
                        {
                            o2_unstandard_value = (float)(Read_Buffer[45] * 256 + Read_Buffer[44]) * 0.01f;
                            temp_unstandard_value = (float)(Read_Buffer[47] * 256 + Read_Buffer[46]) * 0.1f;
                            yali_unstandard_value = (float)(Read_Buffer[49] * 256 + Read_Buffer[48]) * 0.01f;
                            ll_unstandard_value = (float)(Math.Round((Read_Buffer[51] * 256 + Read_Buffer[50]) * 28.3168f / 60f, 2));
                            ll_standard_value = (float)(Math.Round((ll_unstandard_value * yali_standard_value / 100.325 * 273.15 / (temp_standard_value + 273.15)), 2));
                            return "获取成功";//HC
                        }
                        else
                            return "命令不正常";
                    }
                    else
                    {
                        SendDataOfNHF(getNHStandardData);
                        Thread.Sleep(30);
                        while (ComPort_1.BytesToRead < 31)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
                                return "通讯故障";
                        }
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[3] == 0x38)
                            return "命令不正常";
                        else
                        {
                            try
                            {
                                string datastring = Encoding.Default.GetString(Read_Buffer, 7, 4);
                                o2_unstandard_value = Convert.ToInt16(datastring, 16) * 0.01f;
                                datastring = Encoding.Default.GetString(Read_Buffer, 11, 4);
                                temp_unstandard_value = Convert.ToInt16(datastring, 16) * 0.1f;
                                datastring = Encoding.Default.GetString(Read_Buffer, 15, 4);
                                yali_unstandard_value = Convert.ToInt16(datastring, 16) * 0.01f;
                                datastring = Encoding.Default.GetString(Read_Buffer, 19, 4);
                                ll_unstandard_value = Convert.ToInt16(datastring, 16) * 28.3168f / 60f;
                                datastring = Encoding.Default.GetString(Read_Buffer, 23, 4);
                                ll_standard_value = Convert.ToInt16(datastring, 16) * 28.3168f / 60f;
                                return "获取成功";//HC
                            }
                            catch
                            {
                                return "通讯故障";

                            }
                        }
                    }
                    break;
                default:
                    return "未提供该型号相关功能";
                    break;
            }

            //return "仪器通讯失败";

        }
        #endregion
        #region 取标准状态所有数据值
        /// <summary>
        /// 取标准状态所有数据值
        /// </summary>
        /// <returns>string</returns>
        public string Get_standardDat()
        {
            ReadData();
            int i = 0;
            byte CS = 0;
            switch (yqxh)
            {
                case "flv_1000":
                    byte[] Content = new byte[] { 0x02, 0x76 };
                    //ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                    SendData(getStandardDat, Content);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 12)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 3; i < 11; i++)
                            CS += Read_Buffer[i];
                        //CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[11] == CS)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            o2_standard_value = BitConverter.ToInt16(temp_byte, 0) / 100f;
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            yali_standard_value = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[8];
                            temp_byte[1] = Read_Buffer[7];
                            temp_standard_value = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[10];
                            temp_byte[1] = Read_Buffer[9];
                            ll_standard_value = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            return "获取成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                case "mql_100":
                    byte[] Content_MQ = new byte[] { 0x02, 0x87, 0x03, 0x74 };
                    //ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                    ComPort_1.Write(Content_MQ, 0, 4);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 12)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 11; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[11] == CS)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            o2_standard_value = BitConverter.ToInt16(temp_byte, 0) / 100f;
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            yali_standard_value = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[8];
                            temp_byte[1] = Read_Buffer[7];
                            temp_standard_value = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[10];
                            temp_byte[1] = Read_Buffer[9];
                            ll_standard_value = (float)(Math.Round(BitConverter.ToInt16(temp_byte, 0) / 6f, 1));
                            return "获取成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                case "nhf_1":
                    if (isNhSelfUse)
                    {
                        SendDataOfNHF2(getNHStandardData_selfuse);
                        Thread.Sleep(30);
                        while (ComPort_1.BytesToRead < 58)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
                                return "通讯故障";
                        }
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[0] == 0x37)
                        {
                            o2_standard_value = (float)(Read_Buffer[45] * 256 + Read_Buffer[44]) * 0.01f;
                            temp_standard_value = (float)(Read_Buffer[47] * 256 + Read_Buffer[46]) * 0.1f;
                            yali_standard_value = (float)(Read_Buffer[49] * 256 + Read_Buffer[48]) * 0.01f;
                            ll_unstandard_value = (float)(Math.Round((Read_Buffer[51] * 256 + Read_Buffer[50]) * 28.3168f / 60f, 2));
                            ll_standard_value = (float)(Math.Round((ll_unstandard_value * yali_standard_value / 100.325 * 273.15 / (temp_standard_value + 273.15)), 2));
                            return "获取成功";//HC
                        }
                        else
                            return "命令不正常";
                    }
                    else
                    {
                        SendDataOfNHF(getNHStandardData);
                        Thread.Sleep(30);
                        while (ComPort_1.BytesToRead < 31)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
                                return "通讯故障";
                        }
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[3] == 0x38)
                            return "命令不正常";
                        else
                        {
                            try
                            {
                                string datastring = Encoding.Default.GetString(Read_Buffer, 7, 4);
                                o2_standard_value = Convert.ToInt16(datastring, 16) * 0.01f;
                                datastring = Encoding.Default.GetString(Read_Buffer, 11, 4);
                                temp_standard_value = Convert.ToInt16(datastring, 16) * 0.1f;
                                datastring = Encoding.Default.GetString(Read_Buffer, 15, 4);
                                yali_standard_value = Convert.ToInt16(datastring, 16) * 0.01f;
                                datastring = Encoding.Default.GetString(Read_Buffer, 19, 4);
                                ll_unstandard_value = Convert.ToInt16(datastring, 16) * 28.3168f / 60f;
                                datastring = Encoding.Default.GetString(Read_Buffer, 23, 4);
                                ll_standard_value = Convert.ToInt16(datastring, 16) * 28.3168f / 60f;
                                return "获取成功";//HC
                            }
                            catch
                            {
                                return "通讯故障";

                            }
                        }
                    }
                    break;
                default:
                    return "未提供该型号相关功能";
                    break;
            }

            //return "仪器通讯失败";

        }
        #endregion
        #region 稀释氧校准
        public string Xishi_O2(float o2_xishio2_density)
        {
            ReadData();
            int i = 0;
            byte CS = 0;
            switch (yqxh)
            {
                case "flv_1000":
                    byte tempbyte = 0;
                    //byte[] temp_byte = new byte[2];
                    byte[] Content = new byte[2];
                    Content = BitConverter.GetBytes((Int16)(o2_xishio2_density * 100));
                    tempbyte = Content[0];
                    Content[0] = Content[1];
                    Content[1] = tempbyte;
                    SendData(SetXishiO2Standard, Content);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 3; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[3] == CS)
                        {
                            return "氧化锆校准成功";//HC
                        }
                        else
                            return "校准失败，校验码错误";
                    }
                    break;
                case "mql_100":
                    byte[] Content_MQ = new byte[] { 0x02, 0xa1, 0x03, 0x5a };
                    //ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                    ComPort_1.Write(Content_MQ, 0, 4);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 3; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[3] == CS)
                        {

                            return "氧化锆校准成功";//HC
                        }
                        else
                            return "校准失败，校验码错误";
                    }
                    break;
                default:
                    return "未提供该型号相关功能";
                    break;
            }

            //return "仪器通讯失败";

        }
        #endregion
        #region 稀释氧低标点标定
        public string Xishi_O2low_demarcate(float o2_xishio2_density)
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case "flv_1000":
                    byte CS = 0;
                    byte tempbyte = 0;
                    //byte[] temp_byte = new byte[2];
                    byte[] Content = new byte[2];
                    Content = BitConverter.GetBytes((Int16)(o2_xishio2_density * 100));
                    tempbyte = Content[0];
                    Content[0] = Content[1];
                    Content[1] = tempbyte;
                    SendData(xishiO2_low_demarcate, Content);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 3; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[3] == CS)
                        {
                            return "低点标定成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                default: return "该仪器未提供该命令"; break;
            }
            //return "仪器通讯失败";

        }
        #endregion
        #region 稀释氧高标点标定
        public string Xishi_O2high_demarcate(float o2_xishio2_density)
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case "flv_1000":
                    byte CS = 0;
                    byte tempbyte = 0;
                    //byte[] temp_byte = new byte[2];
                    byte[] Content = new byte[2];
                    Content = BitConverter.GetBytes((Int16)(o2_xishio2_density * 100));
                    tempbyte = Content[0];
                    Content[0] = Content[1];
                    Content[1] = tempbyte;
                    SendData(xishiO2_high_demarcate, Content);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 3; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[3] == CS)
                        {
                            return "氧气高点标定成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                default: return "该仪器未提供该命令"; break;
            }

        }
        #endregion
        #region 气体压力标定
        public string airpressure_demarcate(float airpressure_value)
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case "flv_1000":
                    byte CS = 0;
                    byte tempbyte = 0;
                    //byte[] temp_byte = new byte[2];
                    byte[] Content = new byte[2];
                    Content = BitConverter.GetBytes((Int16)(airpressure_value * 10));
                    tempbyte = Content[0];
                    Content[0] = Content[1];
                    Content[1] = tempbyte;
                    SendData(yali_demarcate, Content);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 3; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[3] == CS)
                        {
                            return "压力标准成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                default: return "该仪器未提供该命令"; break;
            }
            //return "仪器通讯失败";

        }
        #endregion
        #region 温度标定
        public string temperature_demarcate(float temperature_value)
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case "flv_1000":
                    byte CS = 0;
                    byte tempbyte = 0;
                    //byte[] temp_byte = new byte[2];
                    byte[] Content = new byte[2];
                    Content = BitConverter.GetBytes((Int16)(temperature_value * 10));
                    tempbyte = Content[0];
                    Content[0] = Content[1];
                    Content[1] = tempbyte;
                    SendData(temp_demarcate, Content);
                    Thread.Sleep(100);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 3; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[3] == CS)
                        {
                            return "温度标定成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                default: return "该仪器未提供该命令"; break;
            }
            //return "仪器通讯失败";

        }
        #endregion
        #region 恢复出厂设置
        public string reset_default()
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case "flv_1000":
                    byte CS = 0;
                    //byte[] temp_byte = new byte[2];
                    byte[] Content = new byte[] { 0x02, 0x88 };
                    //Content = BitConverter.GetBytes((ushort)(temperature_value*10));
                    SendData(Set_default_settings, Content);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 3; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[3] == CS)
                        {
                            return "恢复出厂设置成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                case "nhf_1":
                    SendDataOfNHF(setNHRestDefault);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return "命令不正常";
                    else
                    {
                        if (Read_Buffer[12] == 0x31)
                            return "恢复成功";//HC
                        else
                            return "恢复失败";
                    }
                    break;
                default: return "该仪器未提供该命令"; break;
            }
            //return "仪器通讯失败";

        }
        #endregion


        #region 南华稀释氧低标点标定
        public string nhf_O2low_demarcate(float o2_xishio2_density)
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {
                case "nhf_1":
                    byte[] Content = setNHO2LowDemarcate;
                    int demarcatevalue = (int)(o2_xishio2_density * 100);
                    Content[4] = (byte)(demarcatevalue / 256);
                    Content[5] = (byte)(demarcatevalue % 256);
                    SendDataOfNHF(Content);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return "命令不正常";
                    else
                    {
                        if (Read_Buffer[12] == 0x31)
                            return "低点标定成功";//HC
                        else
                            return "低点标定失败";
                    }
                    break;
                default: return "该仪器未提供该命令"; break;
            }
            //return "仪器通讯失败";

        }
        #endregion
        #region 南华稀释氧低标点标定
        public string nhf_O2Mid_demarcate(float o2_xishio2_density)
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {
                case "nhf_1":
                    byte[] Content = setNHO2MidDemarcate;
                    int demarcatevalue = (int)(o2_xishio2_density * 100);
                    Content[4] = (byte)(demarcatevalue / 256);
                    Content[5] = (byte)(demarcatevalue % 256);
                    SendDataOfNHF(Content);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return "命令不正常";
                    else
                    {
                        if (Read_Buffer[12] == 0x31)
                            return "中点标定成功";//HC
                        else
                            return "中点标定失败";
                    }
                    break;
                default: return "该仪器未提供该命令"; break;
            }
            //return "仪器通讯失败";

        }
        #endregion
        #region 南华稀释氧低标点标定
        public string nhf_O2High_demarcate(float o2_xishio2_density)
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {
                case "nhf_1":
                    byte[] Content = setNHO2HighDemarcate;
                    int demarcatevalue = (int)(o2_xishio2_density * 100);
                    Content[4] = (byte)(demarcatevalue / 256);
                    Content[5] = (byte)(demarcatevalue % 256);
                    SendDataOfNHF(Content);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return "命令不正常";
                    else
                    {
                        if (Read_Buffer[12] == 0x31)
                            return "高点标定成功";//HC
                        else
                            return "高点标定失败";
                    }
                    break;
                default: return "该仪器未提供该命令"; break;
            }
            //return "仪器通讯失败";

        }
        #endregion

        #region 南华流量高标点标定
        public string nhf_LLhigh_demarcate(float llvalue)
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {
                case "nhf_1":
                    byte[] Content = setNHFlowHighDemarcate;
                    int demarcatevalue = (int)(llvalue);
                    Content[4] = (byte)(demarcatevalue / 256);
                    Content[5] = (byte)(demarcatevalue % 256);
                    SendDataOfNHF(Content);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return "命令不正常";
                    else
                    {
                        if (Read_Buffer[12] == 0x31)
                            return "高点标定成功";//HC
                        else
                            return "高点标定失败";
                    }
                    break;
                default: return "该仪器未提供该命令"; break;
            }
            //return "仪器通讯失败";

        }
        #endregion
        #region 南华流量低标点标定
        public string nhf_LLlow_demarcate(float llvalue)
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {
                case "nhf_1":
                    byte[] Content = setNHFlowLowDemarcate;
                    int demarcatevalue = (int)(llvalue);
                    Content[4] = (byte)(demarcatevalue / 256);
                    Content[5] = (byte)(demarcatevalue % 256);
                    SendDataOfNHF(Content);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return "命令不正常";
                    else
                    {
                        if (Read_Buffer[12] == 0x31)
                            return "低点标定成功";//HC
                        else
                            return "低点标定失败";
                    }
                    break;
                default: return "该仪器未提供该命令"; break;
            }
            //return "仪器通讯失败";

        }
        #endregion
        #region 南华流量单点标定
        public string nhf_LLsingle_demarcate(float llvalue)
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {
                case "nhf_1":
                    byte[] Content = setNHFlowSingleDemarcate;
                    int demarcatevalue = (int)(llvalue);
                    Content[4] = (byte)(demarcatevalue / 256);
                    Content[5] = (byte)(demarcatevalue % 256);
                    SendDataOfNHF(Content);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return "命令不正常";
                    else
                    {
                        if (Read_Buffer[12] == 0x31)
                            return "单点标定成功";//HC
                        else
                            return "单点标定失败";
                    }
                    break;
                default: return "该仪器未提供该命令"; break;
            }
            //return "仪器通讯失败";

        }
        #endregion
        #region 南华气体压力单点校准
        public string nhf_YLsingle_demarcate(float llvalue)
        {
            int i = 0;
            ReadData();
            byte CS = 0;
            switch (yqxh)
            {
                case "flv_1000":
                    byte tempbyte = 0;
                    //byte[] temp_byte = new byte[2];
                    byte[] Content = new byte[2];
                    Content = BitConverter.GetBytes((Int16)(llvalue * 10));
                    tempbyte = Content[0];
                    Content[0] = Content[1];
                    Content[1] = tempbyte;
                    SendData(yali_demarcate, Content);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 3; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[3] == CS)
                        {
                            return "压力标准成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                case "mql_100":
                    byte[] Content_MQ = new byte[] { 0x02, 0xab, 0x05, 0x00,0x00,0x00 };
                    int demarcatevaluemq = (int)(llvalue * 100);
                    Content_MQ[3] = (byte)(demarcatevaluemq / 256);
                    Content_MQ[4] = (byte)(demarcatevaluemq % 256);
                    Content_MQ[5] = getCS_MQ(Content_MQ, 5);
                    //ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                    ComPort_1.Write(Content_MQ, 0, 6);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 3; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[3] == CS)
                        {

                            return "校准成功";//HC
                        }
                        else
                            return "校准失败，校验码错误";
                    }
                    break;
                case "nhf_1":
                    byte[] ContentNH = setNHQtylSingleDemarcate;
                    int demarcatevalue = (int)(llvalue*100);
                    ContentNH[4] = (byte)(demarcatevalue / 256);
                    ContentNH[5] = (byte)(demarcatevalue % 256);
                    SendDataOfNHF(ContentNH);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return "命令不正常";
                    else
                    {
                        if (Read_Buffer[12] == 0x31)
                            return "单点标定成功";//HC
                        else
                            return "单点标定失败";
                    }
                    break;
                default: return "该仪器未提供该命令"; break;
            }
            //return "仪器通讯失败";

        }
        #endregion
        #region 南华温度单点校准
        public string nhf_Tempsingle_demarcate(float llvalue)
        {
            int i = 0;
            ReadData();
            byte CS = 0;
            switch (yqxh)
            {
                case "flv_1000":
                    byte tempbyte = 0;
                    //byte[] temp_byte = new byte[2];
                    byte[] Content = new byte[2];
                    Content = BitConverter.GetBytes((Int16)(llvalue * 10));
                    tempbyte = Content[0];
                    Content[0] = Content[1];
                    Content[1] = tempbyte;
                    SendData(temp_demarcate, Content);
                    Thread.Sleep(100);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 3; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[3] == CS)
                        {
                            return "温度标定成功";//HC
                        }
                        else
                            return "校验码错误";
                    }
                    break;
                case "mql_100":
                    byte[] Content_MQ = new byte[] { 0x02, 0xaa, 0x05, 0x00, 0x00, 0x00 };
                    int demarcatevaluemq = (int)(llvalue * 100);
                    Content_MQ[3] = (byte)(demarcatevaluemq / 256);
                    Content_MQ[4] = (byte)(demarcatevaluemq % 256);
                    Content_MQ[5] = getCS_MQ(Content_MQ, 5);
                    //ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                    ComPort_1.Write(Content_MQ, 0, 6);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x15)
                        return "命令不正常";
                    else
                    {
                        for (i = 0; i < 3; i++)
                            CS += Read_Buffer[i];
                        CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
                        if (Read_Buffer[3] == CS)
                        {

                            return "校准成功";//HC
                        }
                        else
                            return "校准失败，校验码错误";
                    }
                    break;
                case "nhf_1":
                    byte[] ContentNH = setNHTempSingleDemarcate;
                    int demarcatevalue = (int)(llvalue*0.1);
                    ContentNH[4] = (byte)(demarcatevalue / 256);
                    ContentNH[5] = (byte)(demarcatevalue % 256);
                    SendDataOfNHF(ContentNH);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return "命令不正常";
                    else
                    {
                        if (Read_Buffer[12] == 0x31)
                            return "单点标定成功";//HC
                        else
                            return "单点标定失败";
                    }
                    break;
                default: return "该仪器未提供该命令"; break;
            }
            //return "仪器通讯失败";

        }
        #endregion
        #region 南华稀释氧空气调零
        public string nhf_SetO2zero()
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {
                case "nhf_1":
                    SendDataOfNHF(setNHO2zero);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "通讯故障";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return "命令不正常";
                    else
                    {
                        if (Read_Buffer[12] == 0x31)
                            return "调零成功";//HC
                        else
                            return "调零失败";
                    }
                    break;
                default: return "该仪器未提供该命令"; break;
            }
            //return "仪器通讯失败";

        }
        #endregion
        #region 南华开启电机
        public string nhf_TurnOnMotor()
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {
                case "nhf_1":
                    if (isNhSelfUse)
                    {
                        SendDataOfNHF2(turnOnNHMotor_selfuse);
                        Thread.Sleep(20);
                        while (ComPort_1.BytesToRead < 6)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
                                return "通讯故障";
                        }
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[0] == 0x38)                           
                                return "开启电机成功";//HC
                            else
                                return "开启电机失败";
                    }
                    else
                    {
                        SendDataOfNHF(turnOnNHMotor);
                        Thread.Sleep(20);
                        while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
                                return "通讯故障";
                        }
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[3] == 0x38)
                            return "命令不正常";
                        else
                        {
                            if (Read_Buffer[12] == 0x31)
                                return "开启电机成功";//HC
                            else
                                return "开启电机失败";
                        }
                    }
                    break;
                default: return "该仪器未提供该命令"; break;
            }
            //return "仪器通讯失败";

        }
        #endregion
        #region 南华稀释氧空气调零
        public string nhf_TurnOffMotor()
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {
                case "nhf_1":
                    if (isNhSelfUse)
                    {
                        SendDataOfNHF2(turnOffNHMotor_selfuse);
                        Thread.Sleep(20);
                        while (ComPort_1.BytesToRead < 6)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
                                return "通讯故障";
                        }
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[0] == 0x38)
                            return "关闭电机成功";//HC
                        else
                            return "关闭电机失败";
                    }
                    else
                    {
                        SendDataOfNHF(turnOffNHMotor);
                        Thread.Sleep(20);
                        while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
                                return "通讯故障";
                        }
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[3] == 0x38)
                            return "命令不正常";
                        else
                        {
                            if (Read_Buffer[12] == 0x31)
                                return "关闭电机成功";//HC
                            else
                                return "关闭电机失败";
                        }
                    }
                    break;
                default: return "该仪器未提供该命令"; break;
            }
            //return "仪器通讯失败";

        }
        #endregion

    }
}
