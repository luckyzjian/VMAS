using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace Exhaust
{
    public class Fla502_data
    {
        public Fla502_data()
        { 
            
            CO2 = 0;
            CO = 0;
            HC = 0;
            NO = 0;
            O2 = 0;
            SD = 0;
            YW = 0;
            HJWD = 0;
            ZS = 0;
            QLYL = 0;
            λ = 0;
            HJYL = 0;
        }
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
        private float _pef = 0.0f;
        private float _sjll = 0.0f;

        public float Sjll
        {
            get { return _sjll; }
            set { _sjll = value; }
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
        public float PEF
        {
            get { return _pef; }
            set { _pef = value; }
        }
    }
    public class Fla502_temp_data
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

    public class Fla502
    {
        public bool isNhSelfUse = false;
        private string yqxh = "fla_502";
        const string cdxh = "cdf5000";
        public double hcxs = 1, coxs = 1, noxs = 1;
        public Fla502()
        { }
        public Fla502(string xh)
        {
            yqxh = xh.ToLower();
        }
        

        //private System.ComponentModel.IContainer components = new System.ComponentModel.Container();
        public System.IO.Ports.SerialPort ComPort_1;
        byte[] Send_Buffer;                                     //发送缓冲区
        byte[] Read_Buffer;                                     //读取缓冲区
        bool Read_Flag = false;                                 //是否有数据可以读取

        #region fla_502通讯协议
        byte DID = 0x02;                                            //数据头
        byte NAK = 0x15;                                            //命令错误，发送数据长度错误或者校验和错误

        byte cmdGetDat = 0x01;
        byte cmdGetStatusOfnha504 = 0x26;
        byte cmdGetTemp = 0x02;
        byte cmdPumpAir = 0x03;
        byte cmdPumpPipeair = 0x04;
        //byte cmdPumpPipeairOfnha504 = 0x23;
        byte cmdClearPipe = 0x05;
        byte cmdStopClear = 0x06;
        byte cmdLeakCheck = 0x07;
        byte cmdZeroing = 0x08;
        byte cmdWrStandardAir = 0x09;
        byte cmdDemarcate = 0x10;
        byte cmdWrTestAir = 0x11;
        byte cmdAutoCheck = 0x12;
        byte cmdGetPEF = 0x13;
        byte cmdZeroIsAir = 0x15;
        byte cmdZeroIsZero = 0x16;
        byte cmdSetStrokeIs4 = 0x17;
        byte cmdSetStrokeIs2 = 0x18;
        byte cmdSetIgnitionIsSingle = 0x19;
        byte cmdSetIgnitionIsTwice = 0x20;
       
        byte cmdUnlockKeyboard = 0x23;
        byte cmdLockKeyboard = 0x24;
        byte cmdTurnonLCD = 0x25;
        byte cmdTurnoffLCD = 0x26;
        byte cmdDemarcateTemp = 0x27;
        byte cmdDemarcateOilTemp = 0x29;
        byte cmdDemarcateHumidity = 0x30;
        byte cmdDemarcateAirPres = 0x31;
        byte cmdOpenTestGas = 0x32;
        //byte cmdOpenTestGasOfnha504 = 0x20;
        byte cmdCloseTestGas = 0x33;
        //byte cmdCloseTestGasOfnha504 = 0x21;
        byte cmdStartTest = 0x34;
        byte cmdPrepareTest = 0x35;
        byte cmdOpenStandardGas = 0x36;
        byte cmdCloseStandardGas = 0x37;
        byte cmdOpenZeroGas = 0x38;
        byte cmdCloseZeroGas = 0x39;
        byte cmdSetCylinderCount = 0x40;
        byte cmdSetIsBus = 0x41;
        byte cmdSetIsBranch = 0x42;




        public float hc_density = 0.0f;
        public float co_density = 0.0f;
        public float co2_density = 0.0f;
        public float o2_density = 0.0f;
        public float no_density = 0.0f;
        public float pef_value = 0.0f;

        #endregion
        #region NHA-503/NHA-508
        byte cmdGetDat_nh503 = 01;
        byte cmdGetStatus_nh504 = 0x26;
        byte cmdGetTemp_nh503 = 2;
        byte cmdPumpAir_nh503 =3;
        byte cmdPumpPipeair_nh503 = 4;
        byte cmdPumpPipeair_nh504 = 0x23;
        byte cmdClearPipe_nh503 = 5;
        byte cmdStopClear_nh503 = 6;
        byte cmdLeakCheck_nh503 = 7;
        byte cmdZeroing_nh503 = 8;
        byte cmdWrStandardAir_nh503 = 9;
        byte cmdDemarcate_nh503 = 10;
        byte cmdWrTestAir_nh503 = 11;
        byte cmdAutoCheck_nh503 = 12;
        byte cmdGetPEF_nh503 = 13;
        byte cmdZeroIsAir_nh503 = 15;
        byte cmdZeroIsZero_nh503 = 16;
        byte cmdSetStrokeIs4_nh503 = 17;
        byte cmdSetStrokeIs2_nh503 = 18;
        byte cmdSetIgnitionIsSingle_nh503 = 19;
        byte cmdSetIgnitionIsTwice_nh503 = 20;

        byte cmdUnlockKeyboard_nh503 = 23;
        byte cmdLockKeyboard_nh503 = 24;
        byte cmdTurnonLCD_nh503 = 25;
        byte cmdTurnoffLCD_nh503 = 26;
        byte cmdDemarcateTemp_nh503 = 27;
        byte cmdDemarcateOilTemp_nh503 = 29;
        byte cmdDemarcateHumidity_nh503 = 30;
        byte cmdDemarcateAirPres_nh503 = 31;
        byte cmdOpenTestGas_nh503 = 32;
        byte cmdOpenTestGas_nha504 = 0x20;
        byte cmdCloseTestGas_nh503 = 33;
        byte cmdCloseTestGas_nha504 = 0x21;
        byte cmdStartTest_nh503 = 34;
        byte cmdPrepareTest_nh503 = 35;
        byte cmdOpenStandardGas_nh503 = 36;
        byte cmdCloseStandardGas_nh503 = 37;
        byte cmdOpenZeroGas_nh503 = 38;
        byte cmdCloseZeroGas_nh503 = 39;
        byte cmdSetCylinderCount_nh503 = 40;
        byte cmdSetIsBus_nh503 = 41;
        byte cmdSetIsBranch_nh503 = 42;
        #endregion
        #region NHA-506

        byte cmdGetData506 = 0x03;
        byte cmdPumpPipeair506 = 0x01;
        byte cmdStopAction506 = 0x02;
        byte cmdGetStatus506 = 0x14;

        #endregion
        #region CDF5000
        byte CDcmdGetDat = 0x01;
        byte CDcmdGetTemp = 0x02;
        byte CDcmdPumpAir = 0x03;
        byte CDcmdPumpPipeair = 0x04;
        byte CDcmdClearPipe = 0x05;
        byte CDcmdStopClear = 0x06;
        byte CDcmdLeakCheck = 0x07;
        byte CDcmdZeroing = 0x08;
        byte CDcmdWrStandardAir = 0x09;
        byte CDcmdDemarcate = 0x0A;
        byte CDcmdWrTestAir = 0x0B;
        byte CDcmdAutoCheck = 0x0C;
        byte CDcmdGetPEF = 0x0D;
        byte CDcmdClearNOFlag = 0x0e;
        byte CDcmdZeroIsAir = 0x0f;
        byte CDcmdZeroIsZero = 0x10;
        byte CDcmdSetStrokeIs4 = 0x11;
        byte CDcmdSetStrokeIs2 = 0x12;
        byte CDcmdSetIgnitionIsSingle = 0x13;
        byte CDcmdSetIgnitionIsTwice = 0x14;
        
        byte CDcmdOpenTestGas = 0x20;
        byte CDcmdCloseTestGas = 0x21;

        #region CDF5000柴油部分
        byte CDCYcmdSetTest = 0xa0;
        byte CDCYcmdDemarcate = 0xa4;
        byte CDCYcmdGetDirectData = 0xa5;
        byte CDCYcmdGetMaxData = 0xa6;
        byte CDCYcmdClearMaxData = 0xa7;
        #endregion
        #endregion
        #region MQW_50A通讯协议

        byte cmdGetDat_MQ = 0x60;
        byte cmdGetStatus_MQ = 0x61;
        byte cmdBackAir_MQ = 0x62;
        byte cmdHuanjinAir_MQ = 0x63;
        byte cmdGetRemainedHC_MQ = 0x64;
        byte cmdAutoFlowback_MQ = 0x65;
        byte cmdAutoLeakTest_MQ = 0x66;
        byte cmdZero_MQ = 0x67;
        byte cmdSetZeroGas_MQ = 0x68;
        byte cmdDemarcate_MQ = 0x69;
        byte cmdLockKeyboard_MQ = 0x6a;
        byte cmdUnlockKeyboard_MQ = 0x6b;
        byte cmdSetRl_MQ = 0x6c;
        byte cmdSetFireCount_MQ = 0x6d;
        byte cmdSetStrokeCount_MQ = 0x6e;
        byte cmdGetBackAirData_MQ = 0x6f;
        byte cmdGethjAirData_MQ = 0x70;
        byte cmdGetRemainedHCData_MQ = 0x71;
        byte cmdGlylDemarcate_MQ = 0x72;
        byte cmdYwDemarcate_MQ = 0x73;
        byte cmdSdDemarcate_MQ = 0x75;
        byte cmdSetTlStyle_MQ = 0x76;
        byte cmdDefault_MQ = 0x77;
        byte cmdStopAction_MQ = 0x78;
        byte cmdStartTest_MQ = 0x79;
        byte cmdStopTest_MQ = 0x7a;
        byte cmdPumpPipeair_MQ = 0x7b;
        byte cmdPumpAir_MQ = 0x7c;
        byte cmdPumpN2_MQ = 0x7d;
        byte cmdOpenDemarcateGas_MQ = 0x7e;
        byte cmdOpenTestGas_MQ = 0x7f;
        byte cmdFlowBack_MQ = 0x80;
        byte cmdYLdemarcate_MQ = 0x86;
        byte cmdWDdemarcate_MQ = 0x87;
        byte cmdTurnOnLCD_MQ = 0x88;
        byte cmdTurnOffLCD_MQ = 0x89;

        #endregion
        #region 佛分FASM-5000的通讯协议
        byte[] cmdFrameHead_FF = { 0x5a, 0x5a };
        byte cmdEquipAddress_FF = 0x01;
        byte cmdGetData_FF = 0x02;
        byte cmdGetPEF_FF = 0x03;
        byte cmdGetTestSignal_FF = 0x04;
        byte cmdGetStatus_FF = 0x05;
        byte cmdWriteDemacate_FF = 0x07;
        byte cmdStopPump_FF = 0x0a;
        byte cmdPumpPipeAir_FF = 0x0b;
        byte cmdPumpAir_FF = 0x0c;
        byte cmdPumpZeroAir_FF = 0x0d;
        byte cmdPumpTestGas_FF = 0x0e;
        byte cmdPumpDemarcateGas_FF = 0x0f;
        byte cmdBlowBack_FF = 0x10;
        byte cmdPumpLeakCheck_FF = 0x11;
        byte cmdSetZeroGas_FF = 0x12;
        byte cmdSetCarInf_FF = 0x13;
        byte cmdZeroing_FF = 0x15;
        byte cmdLeakCheck_FF = 0x17;
        byte cmdCantAnswer_FF = 0x19;
        byte cmdSuccessAnswer_FF = 0x1a;
        byte cmdFailAnswer_FF = 0x1b;
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
                int temp = 0;
                byte[] CS = new byte[DF.Length + 2];
                CS[0] = Cmd;
                //byte LB = Convert.ToByte(3 + Content.Length);
                for (int i = 0; i < DF.Length; i++)
                {
                    temp += DF[i];
                    CS[i + 1] = DF[i];
                }
                //temp +=  Cmd ;//计算出和校验位
                CS[DF.Length + 1] = Convert.ToByte(temp % 256);
                ComPort_1.Write(CS, 0, CS.Length);
                ini.INIIO.saveLogInf("[废气仪发送]:" + byteToHexStr(CS));
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
        public string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += " " + bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        public string byteToHexStr(List<byte> listbyte, int start, int end)
        {
            string returnStr = "";
            if (listbyte != null)
            {
                for (int i = start; i <= end; i++)
                {
                    returnStr += " " + listbyte[i].ToString("X2");
                }
            }
            return returnStr;
        }
        #region 接收数据
        /// <summary>
        /// 接收数据
        /// </summary>
        public void ReadData()
        {
            Read_Buffer = new byte[100];
            Read_Flag = false;
            int count = ComPort_1.Read(Read_Buffer, 0, ComPort_1.BytesToRead);
            if (count > 0)
            {
                ini.INIIO.saveLogInf("【废气仪接收】:" + byteToHexStr(Read_Buffer.ToList(), 0, count));
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
            int i = 0;
            string msg = "";
            byte Status = 0;
            switch (yqxh)
            {
                case "fla_502":

                    ReadData();
                    byte[] Content = new byte[] { cmdGetDat };
                    i = 0;

                    if (!ComPort_1.IsOpen)      //串口出错
                        return "仪器通讯失败";
                    ComPort_1.Write(Content, 0, 1);
                    //SendData(Cmd_GetStatus, Content);    //取废气分析仪状态
                    Thread.Sleep(50);
                    while (ComPort_1.BytesToRead < 21)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "仪器通讯失败";
                    }
                    ReadData();                     //读取返回的数据
                    //[06](HC)(CO)(CO2)(O2)(NO)(转速)(油温)(流量)[Status](lamda)[CS]
                    Status = Convert.ToByte(Read_Buffer[17] & 0xff);
                    switch (Status)
                    {
                        case 0x02: msg += "预热状态"; break;
                        case 0x04: msg += "检漏中"; break;
                        case 0x05: msg += "调零中"; break;
                        case 0x00: msg += "仪器已经准备好"; break;
                        default: break;
                    }
                    return msg;
                    break;
                case cdxh:

                    ReadData();
                    byte[] ContentCD = new byte[] { CDcmdGetDat };
                    i = 0;

                    if (!ComPort_1.IsOpen)      //串口出错
                        return "仪器通讯失败";
                    ComPort_1.Write(ContentCD, 0, 1);
                    //SendData(Cmd_GetStatus, Content);    //取废气分析仪状态
                    Thread.Sleep(50);
                    while (ComPort_1.BytesToRead < 19)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "仪器通讯失败";
                    }
                    ReadData();                     //读取返回的数据
                    //[06](HC)(CO)(CO2)(O2)(NO)(转速)(油温)(流量)[Status](lamda)[CS]
                    Status = Convert.ToByte(Read_Buffer[17] & 0xff);
                    switch (Status)
                    {
                        case 0x02: msg += "预热状态"; break;
                        case 0x04: msg += "检漏中"; break;
                        case 0x05: msg += "调零中"; break;
                        case 0x00: msg += "仪器已经准备好"; break;
                        default: break;
                    }
                    return msg;
                    break;
                case "mqw_50a":
                    ReadData();
                    i = 0;
                    byte[] Content_MQ = new byte[] { DID, cmdGetStatus_MQ, 0X03, 0 };
                    Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                    ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 11)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "仪器通讯失败";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x61)
                    {
                        Status = Convert.ToByte(Read_Buffer[3] & 0xff);
                        switch (Status)
                        {
                            case 0x00: msg += "仪器已经准备好"; break;
                            case 0x01: msg += "预热中"; break;
                            case 0x02: msg += "反吹中"; break;
                            case 0x04: msg += "背景空气测定中"; break;
                            case 0x08: msg += "环境空气测定中"; break;
                            case 0x10: msg += "泄漏检查进行中"; break;
                            case 0x20: msg += "HC残留测定中"; break;
                            case 0x40: msg += "调零中"; break;
                            case 0x80: msg += "气路低流量"; break;
                            default: break;
                        }
                        return msg;
                    }
                    else
                        return "仪器通讯失败";
                    break;

                case "mqw_511":
                    ReadData();
                    i = 0;
                    byte[] Content_MQ511 = new byte[] { 0x10 };
                    ComPort_1.Write(Content_MQ511, 0, 1);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "仪器通讯失败";
                    }
                    ReadData();
                    return "仪器已经准备好";
                    break;
                case "nha_503":
                    ReadData();
                    byte[] ContentNH = new byte[] { cmdGetDat_nh503 };
                    i = 0;

                    if (!ComPort_1.IsOpen)      //串口出错
                        return "仪器通讯失败";
                    ComPort_1.Write(ContentNH, 0, 1);
                    //SendData(Cmd_GetStatus, Content);    //取废气分析仪状态
                    Thread.Sleep(50);
                    while (ComPort_1.BytesToRead < 19)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "仪器通讯失败";
                    }
                    bool isnewnha = (ComPort_1.BytesToRead >= 21);
                    ReadData();                     //读取返回的数据
                    //[06](HC)(CO)(CO2)(O2)(NO)(转速)(油温)(流量)[Status](lamda)[CS]
                    Status = Convert.ToByte(Read_Buffer[17] & 0xff);
                    if (isnewnha)
                    {
                        switch (Status)
                        {
                            case 0x01: msg += "O2老化"; break;
                            case 0x02: msg += "NO老化"; break;
                            case 0x03: msg += "O2及NO老化"; break;
                            case 0x00: msg += "仪器已经准备好"; break;
                            default: msg += "仪器已经准备好"; break;
                        }
                    }
                    else
                    {
                        if ((Status & 0x03) == 0x00)
                        {
                            msg += "仪器已经准备好";
                        }
                        else
                        {
                            msg += "仪器已经准备好";
                        }
                    }
                    return msg;
                    break;
                case "nha_506":
                    ReadData();
                    byte[] ContentNH506 = new byte[] { cmdGetStatus506 };
                    i = 0;

                    if (!ComPort_1.IsOpen)      //串口出错
                        return "仪器通讯失败";
                    ComPort_1.Write(ContentNH506, 0, 1);
                    //SendData(Cmd_GetStatus, Content);    //取废气分析仪状态
                    Thread.Sleep(50);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "仪器通讯失败";
                    }
                    
                    ReadData();                     //读取返回的数据
                    //[06](HC)(CO)(CO2)(O2)(NO)(转速)(油温)(流量)[Status](lamda)[CS]
                    Status = Convert.ToByte(Read_Buffer[0] & 0xff);
                    switch(Status)
                    {
                        case 0x48:msg = "仪器正在预热";break;
                        case 0x5a: msg = "仪器正在调零"; break;
                        case 0x57: msg = "仪器已经准备好"; break;
                        default: msg = "状态字无效"; break;
                    }
                    return msg;
                    break;
                default:
                    return "未提供该型号相关操作";
                    break;
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
        #region 开始检测
        /// <summary>
        /// 开始检测
        /// </summary>
        /// <returns>string</returns>
        public string Start()
        {
            string Struct_Now = "";
            int i = 0;
            switch (yqxh)
            {
                case "fla_502":
                    ReadData();
                    //Struct_Now = Get_Struct();
                    i = 0;
                    byte[] Content = new byte[] { cmdStartTest };
                    if (true)
                    {
                        Stop();//先关闭测试
                        ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                        Thread.Sleep(10);
                        while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
                                return "仪器通讯失败";
                        }
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[0] == 0x06)
                            return "成功开始测量";
                        else
                            return "仪器通讯失败";
                    }
                    else
                        return Struct_Now;
                    break;
                case cdxh:
                    ReadData();
                    //Struct_Now = Get_Struct();
                    i = 0;
                    byte[] ContentCD = new byte[] { CDcmdPumpPipeair };
                    if (true)
                    {
                        Stop();//先关闭测试
                        ComPort_1.Write(ContentCD, 0, 1);        //发送开始测量命令
                        Thread.Sleep(10);
                        while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
                                return "仪器通讯失败";
                        }
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[0] == 0x06)
                            return "成功开始测量";
                        else
                            return "仪器通讯失败";
                    }
                    else
                        return Struct_Now;
                    break;
                case "mqw_50a":
                    ReadData();
                    //Struct_Now = Get_Struct();
                    //Zeroing();
                    i = 0;
                    byte[] Content_MQ = new byte[] { DID, cmdStartTest_MQ, 0X03, 0 };
                    Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                    if (true)
                    {
                        Stop();//先关闭测试
                        ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                        Thread.Sleep(10);
                        while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
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
                    break;
                case "mqw_511":
                    ReadData();
                    i = 0;
                    byte[] Content_MQ511 = new byte[] { 0x01 };
                    ComPort_1.Write(Content_MQ511, 0, 1);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "仪器通讯失败";
                    }
                    ReadData();
                    return "成功开始测量";
                    break;
                case "nha_503":
                    ReadData();
                    Struct_Now = Get_Struct();
                    i = 0;
                    byte[] naContent = new byte[] { cmdPumpPipeair_nh503 };
                    if (Struct_Now == "仪器已经准备好")
                    {
                        Stop();//先关闭测试
                        ComPort_1.Write(naContent, 0, 1);        //发送开始测量命令
                        Thread.Sleep(10);
                        while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
                                return "仪器通讯失败";
                        }
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[0] == 0x06)
                            return "成功开始测量";
                        else
                            return "成功开始测量";
                    }
                    else
                        return Struct_Now;
                    break;
                case "nha_506":return "成功开始测量";break;
                case "fasm_5000":
                    ReadData();
                    //Struct_Now = Get_Struct();
                    //Zeroing();
                    i = 0;
                    byte[] Content_FF = new byte[] { cmdEquipAddress_FF, cmdPumpPipeAir_FF, 0X00, 0x00, 0x00 };
                    Content_FF[4] = getCS_MQ(Content_FF, 4);
                    if (true)
                    {
                        Stop();//先关闭测试
                        ComPort_1.Write(cmdFrameHead_FF, 0, 2);        //发送开始测量命令
                        ComPort_1.Write(Content_FF, 0, 5);        //发送开始测量命令
                        Thread.Sleep(10);
                        while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
                                return "仪器通讯失败";
                        }
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x0b && Read_Buffer[4] == 0x00 && Read_Buffer[5] == 0x00 && Read_Buffer[6] == 0xf4)
                            return "成功开始测量";
                        else
                            return "仪器通讯失败";
                    }
                    else
                        return Struct_Now;
                    break;
                default:
                    return "未找到该型号相关操作";
                    break;
            }
        }
        #endregion

        #region 停止检测
        /// <summary>
        /// 停止检测
        /// </summary>
        /// <returns>string</returns>
        public bool Stop()
        {
            int i = 0;
            switch (yqxh)
            {
                case "fla_502":
                    ReadData();
                    i = 0;
                    byte[] Content = new byte[] { cmdPrepareTest };
                    ComPort_1.Write(Content, 0, 1);        //发送停止测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case cdxh:
                    ReadData();
                    i = 0;
                    byte[] ContentCD = new byte[] { CDcmdStopClear };
                    ComPort_1.Write(ContentCD, 0, 1);        //发送停止测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "mqw_50a":
                    ReadData();
                    i = 0;
                    byte[] Content_MQ = new byte[] { DID, cmdStopTest_MQ, 0X03, 0 };
                    Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                    ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x7a && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x7d)
                        return true;
                    else
                        return false;
                    break;
                case "mqw_511":
                    ReadData();
                    i = 0;
                    byte[] Content_MQ511 = new byte[] { 0x02 };
                    ComPort_1.Write(Content_MQ511, 0, 1);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();
                    return true;
                    break;
                case "nha_503":
                    ReadData();
                    i = 0;
                    byte[] naContent = new byte[] { cmdStopClear_nh503 };
                    ComPort_1.Write(naContent, 0, 1);        //发送停止测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "nha_506": return true; break;
                case "fasm_5000":
                    ReadData();
                    //Struct_Now = Get_Struct();
                    //Zeroing();
                    i = 0;
                    byte[] Content_FF = new byte[] { cmdEquipAddress_FF, cmdStopPump_FF, 0X00, 0x00, 0x00 };
                    Content_FF[4] = getCS_MQ(Content_FF, 4);
                    ComPort_1.Write(cmdFrameHead_FF, 0, 2);        //发送开始测量命令
                    ComPort_1.Write(Content_FF, 0, 5);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x0a && Read_Buffer[4] == 0x00 && Read_Buffer[5] == 0x00 && Read_Buffer[6] == 0xf5)
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
        #region 开始标定
        /// <summary>
        /// 开始检测
        /// </summary>
        /// <returns>string</returns>
        public string Demarcate()
        {
            int i = 0;
            switch (yqxh)
            {
                case "fla_502":
                    ReadData();
                    byte[] Content = new byte[] { cmdDemarcate };
                    ComPort_1.Write(Content, 0, 1);        //发送开始测量命令
                    Thread.Sleep(500);
                    if (ComPort_1.BytesToRead > 0)
                    {
                        Thread.Sleep(500);
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[0] == 0x05)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[2];
                            temp_byte[1] = Read_Buffer[1];
                            hc_density = BitConverter.ToInt16(temp_byte, 0);         //HC
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            co_density = BitConverter.ToInt16(temp_byte, 0) / 1000f; ;       //CO
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            co2_density = BitConverter.ToInt16(temp_byte, 0) / 100f;               //CO2
                            temp_byte[0] = Read_Buffer[8];
                            temp_byte[1] = Read_Buffer[7];
                            o2_density = BitConverter.ToInt16(temp_byte, 0) / 100f;               //O2
                            temp_byte[0] = Read_Buffer[10];
                            temp_byte[1] = Read_Buffer[9];
                            no_density = BitConverter.ToInt16(temp_byte, 0);          //NO
                            temp_byte[0] = Read_Buffer[12];
                            temp_byte[1] = Read_Buffer[11];
                            pef_value = BitConverter.ToInt16(temp_byte, 0) / 1000f;         //转速
                            return "标定中";

                        }
                        else if (Read_Buffer[0] == 0x00)
                            return "标定成功";
                        else if (Read_Buffer[0] == 0x01)
                            return "标定失败";
                        else
                            return "仪器通讯失败";
                    }
                    else
                        return "仪器通讯失败";
                    break;
                case cdxh:
                    ReadData();
                    byte[] ContentCD = new byte[] { CDcmdDemarcate };
                    ComPort_1.Write(ContentCD, 0, 1);        //发送开始测量命令
                    Thread.Sleep(500);
                    if (ComPort_1.BytesToRead > 0)
                    {
                        Thread.Sleep(500);
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[0] == 0x05)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[2];
                            temp_byte[1] = Read_Buffer[1];
                            hc_density = BitConverter.ToInt16(temp_byte, 0);         //HC
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            co_density = BitConverter.ToInt16(temp_byte, 0) / 1000f; ;       //CO
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            co2_density = BitConverter.ToInt16(temp_byte, 0) / 100f;               //CO2
                            temp_byte[0] = Read_Buffer[8];
                            temp_byte[1] = Read_Buffer[7];
                            o2_density = BitConverter.ToInt16(temp_byte, 0) / 100f;               //O2
                            temp_byte[0] = Read_Buffer[10];
                            temp_byte[1] = Read_Buffer[9];
                            no_density = BitConverter.ToInt16(temp_byte, 0);          //NO
                            temp_byte[0] = Read_Buffer[12];
                            temp_byte[1] = Read_Buffer[11];
                            pef_value = BitConverter.ToInt16(temp_byte, 0) / 1000f;         //转速
                            return "标定中";

                        }
                        else if (Read_Buffer[0] == 0x00)
                            return "标定成功";
                        else if (Read_Buffer[0] == 0x01)
                            return "标定失败";
                        else
                            return "仪器通讯失败";
                    }
                    else
                        return "仪器通讯失败";
                    break;
                case "nha_503":
                    ReadData();
                    byte[] naContent = new byte[] { cmdDemarcate_nh503 };
                    ComPort_1.Write(naContent, 0, 1);        //发送开始测量命令
                    ini.INIIO.saveLogInf("[废气仪发送]:" + byteToHexStr(naContent));
                    Thread.Sleep(500);
                    if (ComPort_1.BytesToRead > 0)
                    {
                        Thread.Sleep(500);
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[0] == 0x05)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[2];
                            temp_byte[1] = Read_Buffer[1];
                            hc_density = BitConverter.ToInt16(temp_byte, 0);         //HC
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            co_density = BitConverter.ToInt16(temp_byte, 0) / 100f; ;       //CO
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            co2_density = BitConverter.ToInt16(temp_byte, 0) / 100f;               //CO2
                            temp_byte[0] = Read_Buffer[8];
                            temp_byte[1] = Read_Buffer[7];
                            o2_density = BitConverter.ToInt16(temp_byte, 0) / 100f;               //O2
                            temp_byte[0] = Read_Buffer[10];
                            temp_byte[1] = Read_Buffer[9];
                            no_density = BitConverter.ToInt16(temp_byte, 0);          //NO
                            temp_byte[0] = Read_Buffer[12];
                            temp_byte[1] = Read_Buffer[11];
                            pef_value = BitConverter.ToInt16(temp_byte, 0) / 1000f;         //转速
                            return "标定中";

                        }
                        else if (Read_Buffer[0] == 0x00)
                            return "标定成功";
                        else if (Read_Buffer[0] == 0x01)
                            return "标定失败";
                        else
                            return "仪器通讯失败";
                    }
                    else
                        return "仪器通讯失败";
                    break;
                default:
                    return "未找到该型号相关操作";
                    break;
            }

            // else
            // return Struct_Now;
        }
        #endregion

        private Fla502_temp_data Fla502_tempdata = new Fla502_temp_data();
        #region 获取辅助数据
        /// <summary>
        /// byte cmdGetTemp = 0x02;
        /// </summary>
        /// <returns>Fla502_temp_data</returns>
        public Fla502_temp_data Get_Temp()
        {
            int i = 0;
            switch (yqxh)
            {
                case "fla_502":
                    ReadData();
                    byte[] Content = new byte[] { cmdGetTemp };
                    ComPort_1.Write(Content, 0, 1);//
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 8)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return Fla502_tempdata;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x6)
                    {
                        byte[] temp_byte = new byte[2];
                        temp_byte[0] = Read_Buffer[2];
                        temp_byte[1] = Read_Buffer[1];
                        Fla502_tempdata.TEMP = BitConverter.ToInt16(temp_byte, 0) / 10f;         //环境温度
                        temp_byte[0] = Read_Buffer[4];
                        temp_byte[1] = Read_Buffer[3];
                        Fla502_tempdata.AIRPRESSURE = BitConverter.ToInt16(temp_byte, 0) / 10f; ;       //大气压力
                        temp_byte[0] = Read_Buffer[6];
                        temp_byte[1] = Read_Buffer[5];
                        Fla502_tempdata.HUMIDITY = BitConverter.ToInt16(temp_byte, 0) / 10f;               //相对湿度


                    }
                    return Fla502_tempdata;
                    break;
                case cdxh:
                    ReadData();
                    Fla502_temp_data cd_tempdata = new Fla502_temp_data();
                    byte[] ContentCD = new byte[] { CDcmdGetTemp };
                    ComPort_1.Write(ContentCD, 0, 1);//
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 8)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return cd_tempdata;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x6)
                    {
                        byte[] temp_byte = new byte[2];
                        temp_byte[0] = Read_Buffer[2];
                        temp_byte[1] = Read_Buffer[1];
                        cd_tempdata.TEMP = BitConverter.ToInt16(temp_byte, 0) / 10f;         //环境温度
                        temp_byte[0] = Read_Buffer[4];
                        temp_byte[1] = Read_Buffer[3];
                        cd_tempdata.AIRPRESSURE = BitConverter.ToInt16(temp_byte, 0) / 10f; ;       //大气压力
                        temp_byte[0] = Read_Buffer[6];
                        temp_byte[1] = Read_Buffer[5];
                        cd_tempdata.HUMIDITY = BitConverter.ToInt16(temp_byte, 0) / 10f;               //相对湿度


                    }
                    return cd_tempdata;
                    break;
                case "nha_503":
                    ReadData();
                    Fla502_temp_data na503_tempdata = new Fla502_temp_data();
                    byte[] naContent = new byte[] { cmdGetTemp_nh503 };
                    ComPort_1.Write(naContent, 0, 1);//
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 8)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return na503_tempdata;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x6)
                    {
                        byte[] temp_byte = new byte[2];
                        temp_byte[0] = Read_Buffer[2];
                        temp_byte[1] = Read_Buffer[1];
                        na503_tempdata.TEMP = BitConverter.ToInt16(temp_byte, 0) / 10f;         //环境温度
                        temp_byte[0] = Read_Buffer[4];
                        temp_byte[1] = Read_Buffer[3];
                        na503_tempdata.AIRPRESSURE = BitConverter.ToInt16(temp_byte, 0) / 10f; ;       //大气压力
                        temp_byte[0] = Read_Buffer[6];
                        temp_byte[1] = Read_Buffer[5];
                        na503_tempdata.HUMIDITY = BitConverter.ToInt16(temp_byte, 0) / 10f;               //相对湿度


                    }
                    return na503_tempdata;
                    break;
                case "nha_506":
                    Fla502_temp_data na506_tempdata = new Fla502_temp_data();
                    na506_tempdata.TEMP = 0;
                    na506_tempdata.AIRPRESSURE = 0;
                    na506_tempdata.HUMIDITY = 0;
                    return na506_tempdata;
                    break;
                default:
                    ReadData();
                    Fla502_temp_data de503_tempdata = new Fla502_temp_data();
                    return de503_tempdata;
                    break;
            }
        }
        #endregion
        //   byte cmdPumpAir = 0x03;
        #region 抽空气
        /// <summary>
        /// 开泵，气样从环境气入口进入。
        /// </summary>
        /// <returns>bool</returns>
        public bool Pump_air()
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case "fla_502":
                    byte[] Content = new byte[] { cmdPumpAir };
                    ComPort_1.Write(Content, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case cdxh:
                    byte[] ContentCD = new byte[] { CDcmdPumpAir };
                    ComPort_1.Write(ContentCD, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "mqw_50a":
                    byte[] Content_MQ = new byte[] { DID, cmdPumpAir_MQ, 0X03, 0 };
                    Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                    ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x7b && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x7c)
                        return true;
                    else
                        return false;
                    break;
                case "nha_503":
                    byte[] naContent = new byte[] { cmdPumpAir_nh503 };
                    ComPort_1.Write(naContent, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "fasm_5000":
                    ReadData();
                    //Struct_Now = Get_Struct();
                    //Zeroing();
                    i = 0;
                    byte[] Content_FF = new byte[] { cmdEquipAddress_FF, cmdPumpAir_FF, 0X00, 0x00, 0x00 };
                    Content_FF[4] = getCS_MQ(Content_FF, 4);
                    ComPort_1.Write(cmdFrameHead_FF, 0, 2);        //发送开始测量命令
                    ComPort_1.Write(Content_FF, 0, 5);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x0c && Read_Buffer[4] == 0x00 && Read_Buffer[5] == 0x00 && Read_Buffer[6] == 0xf3)
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
        #region 抽管道气
        /// <summary>
        /// 开泵，气样从探头进入。
        /// </summary>
        /// <returns>bool</returns>
        public bool Pump_Pipeair()
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case "fla_502":
                    byte[] Content = new byte[] { cmdPumpPipeair };
                    ComPort_1.Write(Content, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case cdxh:
                    byte[] ContentCD = new byte[] { CDcmdPumpPipeair };
                    ComPort_1.Write(ContentCD, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "mqw_50a":
                    byte[] Content_MQ = new byte[] { DID, cmdPumpPipeair_MQ, 0X03, 0 };
                    Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                    ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x7c && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x7b)
                        return true;
                    else
                        return false;
                    break;
                case "mqw_511":
                    ReadData();
                    i = 0;
                    byte[] Content_MQ511 = new byte[] { 0x01 };
                    ComPort_1.Write(Content_MQ511, 0, 1);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();
                    return true;
                    break;
                case "nha_503":
                    byte[] naContent = new byte[] { cmdPumpPipeair_nh503 };
                    if (isNhSelfUse) naContent[0] = cmdPumpPipeair_nh504;
                    ComPort_1.Write(naContent, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return true;
                    break;
                case "nha_506":
                    byte[] naContent506 = new byte[] { cmdPumpPipeair506 };
                    ComPort_1.Write(naContent506, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return true;
                    break;
                case "fasm_5000":
                    ReadData();
                    //Struct_Now = Get_Struct();
                    //Zeroing();
                    i = 0;
                    byte[] Content_FF = new byte[] { cmdEquipAddress_FF, cmdPumpPipeAir_FF, 0X00, 0x00, 0x00 };
                    Content_FF[4] = getCS_MQ(Content_FF, 4);
                    ComPort_1.Write(cmdFrameHead_FF, 0, 2);        //发送开始测量命令
                    ComPort_1.Write(Content_FF, 0, 5);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x0b && Read_Buffer[4] == 0x00 && Read_Buffer[5] == 0x00 && Read_Buffer[6] == 0xf4)
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

        #region 标定温度
        /// <summary>
        /// 开泵，气样从探头进入。
        /// </summary>
        /// <returns>bool</returns>
        public bool Demarcate_Temp(float tempValue)
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case "fla_502":
                    byte[] Content = new byte[] { };
                    Content = new byte[2];
                    byte[] temp_byte = BitConverter.GetBytes((short)(tempValue*10));
                    Content[0] = temp_byte[1];
                    Content[1] = temp_byte[0];                    
                    SendData(cmdDemarcateTemp, Content);
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 2)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x00)
                        return true;
                    else
                        return false;
                    break;
                case "nha_503":
                    byte[] NhContent = new byte[] { };
                    NhContent = new byte[2];
                    byte[] nhtemp_byte = BitConverter.GetBytes((short)(tempValue * 10));
                    NhContent[0] = nhtemp_byte[1];
                    NhContent[1] = nhtemp_byte[0];
                    SendData(cmdDemarcateTemp_nh503, NhContent);
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 2)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x00)
                        return true;
                    else
                        return false;
                    break;
                case "mqw_50a":
                    byte[] Content_MQ = new byte[] { DID, 0x87, 0X05, 0x00, 0x00, 0 };
                    Content_MQ[3] = (byte)((tempValue * 10) / 256);
                    Content_MQ[4] = (byte)((tempValue * 10) % 256);
                    Content_MQ[5] = getCS_MQ(Content_MQ, 5);
                    ComPort_1.Write(Content_MQ, 0, 6);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x87 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0xf6)
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
        #region 标定温度
        /// <summary>
        /// 开泵，气样从探头进入。
        /// </summary>
        /// <returns>bool</returns>
        public bool Demarcate_Humidity(float tempValue)
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case "fla_502":
                    byte[] Content = new byte[] { };
                    Content = new byte[2];
                    byte[] temp_byte = BitConverter.GetBytes((short)(tempValue * 10));
                    Content[0] = temp_byte[1];
                    Content[1] = temp_byte[0];
                    SendData(cmdDemarcateHumidity, Content);
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 2)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x00)
                        return true;
                    else
                        return false;
                    break;
                case "nha_503":
                    byte[] NhContent = new byte[] { };
                    NhContent = new byte[2];
                    byte[] nhtemp_byte = BitConverter.GetBytes((short)(tempValue * 10));
                    NhContent[0] = nhtemp_byte[1];
                    NhContent[1] = nhtemp_byte[0];
                    SendData(cmdDemarcateHumidity_nh503, NhContent);
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 2)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x00)
                        return true;
                    else
                        return false;
                    break;
                case "mqw_50a":
                    byte[] Content_MQ = new byte[] { DID, 0x75, 0X05, 0x00, 0x00, 0 };
                    Content_MQ[3] = (byte)((tempValue * 10) / 256);
                    Content_MQ[4] = (byte)((tempValue * 10) % 256);
                    Content_MQ[5] = getCS_MQ(Content_MQ, 5);
                    ComPort_1.Write(Content_MQ, 0, 6);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x75 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x82)
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
        #region 标定温度
        /// <summary>
        /// 开泵，气样从探头进入。
        /// </summary>
        /// <returns>bool</returns>
        public bool Demarcate_Airpressure(float tempValue)
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case "fla_502":
                    byte[] Content = new byte[] { };
                    Content = new byte[2];
                    byte[] temp_byte = BitConverter.GetBytes((short)(tempValue * 10));
                    Content[0] = temp_byte[1];
                    Content[1] = temp_byte[0];
                    SendData(cmdDemarcateAirPres, Content);
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 2)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x00)
                        return true;
                    else
                        return false;
                    break;
                case "nha_503":
                    byte[] NhContent = new byte[] { };
                    NhContent = new byte[2];
                    byte[] nhtemp_byte = BitConverter.GetBytes((short)(tempValue * 10));
                    NhContent[0] = nhtemp_byte[1];
                    NhContent[1] = nhtemp_byte[0];
                    SendData(cmdDemarcateAirPres_nh503, NhContent);
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 2)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x00)
                        return true;
                    else
                        return false;
                    break;
                case "mqw_50a":
                    byte[] Content_MQ = new byte[] { DID, 0x86, 0X05, 0x00, 0x00, 0 };
                    Content_MQ[3] = (byte)((tempValue * 10) / 256);
                    Content_MQ[4] = (byte)((tempValue * 10) % 256);
                    Content_MQ[5] = getCS_MQ(Content_MQ, 5);
                    ComPort_1.Write(Content_MQ, 0, 6);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x86 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0xf7)
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

        #region 获取检漏状态
        /// <summary>
        /// 仪器是否可以进行测量
        /// </summary>
        /// <returns>string 仪器状态</returns>
        public string Get_leakTestStruct()
        {
            int i = 0;
            string msg = "";
            byte Status = 0;
            switch (yqxh)
            {
                case "fla_502":

                    ReadData();
                    byte[] Content = new byte[] { cmdGetDat };
                    i = 0;

                    if (!ComPort_1.IsOpen)      //串口出错
                        return "仪器通讯失败";
                    ComPort_1.Write(Content, 0, 1);
                    //SendData(Cmd_GetStatus, Content);    //取废气分析仪状态
                    Thread.Sleep(50);
                    while (ComPort_1.BytesToRead < 21)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "仪器通讯失败";
                    }
                    ReadData();                     //读取返回的数据
                    //[06](HC)(CO)(CO2)(O2)(NO)(转速)(油温)(流量)[Status](lamda)[CS]
                    Status = Convert.ToByte(Read_Buffer[17] & 0xff);
                    switch (Status)
                    {
                        case 0x02: msg += "预热状态"; break;
                        case 0x04: msg += "检漏中"; break;
                        case 0x05: msg += "调零中"; break;
                        case 0x00: msg += "仪器已经准备好"; break;
                        default: break;
                    }
                    return msg;
                    break;
                case cdxh:

                    ReadData();
                    byte[] ContentCD = new byte[] { CDcmdGetDat };
                    i = 0;

                    if (!ComPort_1.IsOpen)      //串口出错
                        return "仪器通讯失败";
                    ComPort_1.Write(ContentCD, 0, 1);
                    //SendData(Cmd_GetStatus, Content);    //取废气分析仪状态
                    Thread.Sleep(50);
                    while (ComPort_1.BytesToRead < 19)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "仪器通讯失败";
                    }
                    ReadData();                     //读取返回的数据
                    //[06](HC)(CO)(CO2)(O2)(NO)(转速)(油温)(流量)[Status](lamda)[CS]
                    Status = Convert.ToByte(Read_Buffer[17] & 0xff);
                    switch (Status)
                    {
                        case 0x02: msg += "预热状态"; break;
                        case 0x04: msg += "检漏中"; break;
                        case 0x05: msg += "调零中"; break;
                        case 0x00: msg += "仪器已经准备好"; break;
                        default: break;
                    }
                    return msg;
                    break;
                case "mqw_50a":
                    ReadData();
                    i = 0;
                    byte[] Content_MQ = new byte[] { DID, cmdGetStatus_MQ, 0X03, 0 };
                    Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                    ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 11)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "仪器通讯失败";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x61)
                    {
                        Status = Convert.ToByte(Read_Buffer[3] & 0xff);
                        if ((Status & 0x10) == 0x10)
                            msg = "检定中";
                        else
                        {
                            Status = Convert.ToByte(Read_Buffer[5] & 0xff);
                            if ((Status & 0x01) == 0x01)
                            {
                                msg = "泄漏超标";
                            }
                            else
                                msg = "无泄漏";
                        }
                        return msg;
                    }
                    else
                        return "仪器通讯失败";
                    break;
                case "nha_503":
                    ReadData();
                    byte[] nhContent = new byte[] { cmdLeakCheck_nh503 };
                    i = 0;

                    if (!ComPort_1.IsOpen)      //串口出错
                        return "仪器通讯失败";
                    ComPort_1.Write(nhContent, 0, 1);
                    //SendData(Cmd_GetStatus, Content);    //取废气分析仪状态
                    Thread.Sleep(50);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "仪器通讯失败";
                    }
                    ReadData();             
                    if (Read_Buffer[0] == 0x05)
                       msg = "检定中";
                    else if (Read_Buffer[0] == 0x01)
                    {
                       msg = "泄漏超标";
                    }
                    else
                       msg = "无泄漏";
                    return msg;
                    break;
                default:
                    return "未提供该型号相关操作";
                    break;
            }

        }
        public string Get_fla502leckStruct()
        {
            int i = 0;
            string msg = "";
            byte Status = 0;
            switch (yqxh)
            {
                case "fla_502":

                    ReadData();
                    byte[] Content = new byte[] { cmdGetDat };
                    i = 0;

                    if (!ComPort_1.IsOpen)      //串口出错
                        return "仪器通讯失败";
                    ComPort_1.Write(Content, 0, 1);
                    //SendData(Cmd_GetStatus, Content);    //取废气分析仪状态
                    Thread.Sleep(50);
                    while (ComPort_1.BytesToRead < 21)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "仪器通讯失败";
                    }
                    ReadData();                     //读取返回的数据

                    //[06](HC)(CO)(CO2)(O2)(NO)(转速)(油温)(流量)[Status](lamda)[CS]
                    Status = Convert.ToByte(Read_Buffer[17] & 0xff);
                    if (Status == 0x04)
                    {
                        msg = "检定中";
                    }
                    else
                    {
                        if(Read_Buffer[15]==0x88&&Read_Buffer[16]==0x88)
                        {
                            msg = "泄漏超标";
                        }
                        else
                        {
                            msg = "无泄漏";
                        }
                    }
                    return msg;
                    break;
                case cdxh:

                    ReadData();
                    byte[] ContentCD = new byte[] { CDcmdLeakCheck };
                    i = 0;

                    if (!ComPort_1.IsOpen)      //串口出错
                        return "仪器通讯失败";
                    ComPort_1.Write(ContentCD, 0, 1);
                    //SendData(Cmd_GetStatus, Content);    //取废气分析仪状态
                    Thread.Sleep(50);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return "仪器通讯失败";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x00)
                    {
                        msg = "无泄漏";
                    }
                    else if (Read_Buffer[0] == 0x01)
                    {
                        msg = "泄漏超标";
                    }
                    else if (Read_Buffer[0] == 0x05)
                    {
                        msg = "检定中";
                    }
                    return msg;
                    break;
                case "mqw_50a":
                    ReadData();
                    i = 0;
                    byte[] Content_MQ = new byte[] { DID, cmdGetStatus_MQ, 0X03, 0 };
                    Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                    ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 11)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "仪器通讯失败";
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x61)
                    {
                        Status = Convert.ToByte(Read_Buffer[3] & 0xff);
                        if ((Status & 0x10) == 0x10)
                            msg = "检定中";
                        else
                        {
                            Status = Convert.ToByte(Read_Buffer[5] & 0xff);
                            if ((Status & 0x01) == 0x01)
                            {
                                msg = "泄漏超标";
                            }
                            else
                                msg = "无泄漏";
                        }
                        return msg;
                    }
                    else
                        return "仪器通讯失败";
                    break;
                case "nha_503":
                    ReadData();
                    byte[] nhContent = new byte[] { cmdLeakCheck_nh503 };
                    i = 0;

                    if (!ComPort_1.IsOpen)      //串口出错
                        return "仪器通讯失败";
                    ComPort_1.Write(nhContent, 0, 1);
                    //SendData(Cmd_GetStatus, Content);    //取废气分析仪状态
                    Thread.Sleep(50);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return "仪器通讯失败";
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x05)
                        msg = "检定中";
                    else if (Read_Buffer[0] == 0x01)
                    {
                        msg = "泄漏超标";
                    }
                    else
                        msg = "无泄漏";
                    return msg;
                    break;
                default:
                    return "未提供该型号相关操作";
                    break;
            }

        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <returns>0:未返回数据 1：执行成功 -1：执行失败 -2：无法响应指令</returns>
        public int waitSuccessAnswer()
        {
            switch (yqxh)
            {
                case "fasm_5000":
                    int i = 0;
                    while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return 0;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x1a && Read_Buffer[4] == 0x00 && Read_Buffer[5] == 0x00 && Read_Buffer[6] == 0xe5)
                        return 1;
                    else if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x1b && Read_Buffer[4] == 0x00 && Read_Buffer[5] == 0x00 && Read_Buffer[6] == 0xe4)
                        return -1;
                    else if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x19 && Read_Buffer[4] == 0x00 && Read_Buffer[5] == 0x00 && Read_Buffer[6] == 0xe6)
                        return -2;
                    else
                        return -2;
                    break;
                default: return 1; break;
            }

        }
        #region 检漏
        /// <summary>
        /// 检漏。
        /// </summary>
        /// <returns>bool</returns>
        public bool Leak_check()
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case "fla_502":
                    byte[] Content = new byte[] { cmdLeakCheck };
                    ComPort_1.Write(Content, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case cdxh:
                    byte[] ContentCD = new byte[] { CDcmdLeakCheck };
                    ComPort_1.Write(ContentCD, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "nha_503":
                    ReadData();
                    byte[] nhContent = new byte[] { cmdLeakCheck_nh503 };
                    i = 0;

                    if (!ComPort_1.IsOpen)      //串口出错
                        return false;
                    ComPort_1.Write(nhContent, 0, 1);
                    //SendData(Cmd_GetStatus, Content);    //取废气分析仪状态
                    Thread.Sleep(50);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                    
                    return true;
                    break;
                case "mqw_50a":

                    byte[] Content_MQ = new byte[] { DID, cmdAutoLeakTest_MQ, 0X03, 0 };
                    Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                    ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x66 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x91)
                        return true;
                    else
                        return false;
                    break;
                case "fasm_5000":
                    ReadData();
                    //Struct_Now = Get_Struct();
                    //Zeroing();
                    i = 0;
                    byte[] Content_FF = new byte[] { cmdEquipAddress_FF, cmdLeakCheck_FF, 0X00, 0x00, 0x00 };
                    Content_FF[4] = getCS_MQ(Content_FF, 4);
                    ComPort_1.Write(cmdFrameHead_FF, 0, 2);        //发送开始测量命令
                    ComPort_1.Write(Content_FF, 0, 5);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x17 && Read_Buffer[4] == 0x00 && Read_Buffer[5] == 0x00 && Read_Buffer[6] == 0xe8)
                        return true;
                    else
                        return false;
                    break;
                default: return false; break;
            }
        }
        #endregion
        #region 输入标定气浓度
        /// <summary>
        /// 写入标定气深度，输入为零的表示不标定，有数值的表示要标定。
        /// </summary>
        /// <returns>bool</returns>
        public bool Set_standardGas(double C3H8_density, double co_density, double co2_density, double o2_density, double no_density)
        {
            ReadData();
            int i = 0;
            byte[] temp_byte = new byte[2];
            switch (yqxh)
            {
                case "fla_502":
                    byte[] Content = new byte[] { };

                    Content = new byte[10];
                    temp_byte = BitConverter.GetBytes((ushort)C3H8_density);
                    Content[0] = temp_byte[1];
                    Content[1] = temp_byte[0];
                    temp_byte = BitConverter.GetBytes((ushort)(co_density * 1000));
                    Content[2] = temp_byte[1];
                    Content[3] = temp_byte[0];
                    temp_byte = BitConverter.GetBytes((ushort)(co2_density * 100));
                    Content[4] = temp_byte[1];
                    Content[5] = temp_byte[0];
                    temp_byte = BitConverter.GetBytes((ushort)(o2_density * 100));
                    Content[6] = temp_byte[1];
                    Content[7] = temp_byte[0];
                    temp_byte = BitConverter.GetBytes((ushort)(no_density));
                    Content[8] = temp_byte[1];
                    Content[9] = temp_byte[0];
                    SendData(cmdWrStandardAir, Content);
                    //ComPort_1.Write(Content, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 2)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x00)
                        return true;
                    else
                        return false;
                    break;
                case cdxh:
                    byte[] ContentCD = new byte[] { };

                    Content = new byte[8];
                    temp_byte = BitConverter.GetBytes((ushort)C3H8_density);
                    Content[0] = temp_byte[1];
                    Content[1] = temp_byte[0];
                    temp_byte = BitConverter.GetBytes((ushort)(co_density * 100));
                    Content[2] = temp_byte[1];
                    Content[3] = temp_byte[0];
                    temp_byte = BitConverter.GetBytes((ushort)(co2_density * 100));
                    Content[4] = temp_byte[1];
                    Content[5] = temp_byte[0];
                    temp_byte = BitConverter.GetBytes((ushort)(no_density));
                    Content[6] = temp_byte[1];
                    Content[7] = temp_byte[0];
                    SendData(CDcmdWrStandardAir, ContentCD);
                    //ComPort_1.Write(Content, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 2)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x00)
                        return true;
                    else
                        return false;
                    break;
                case "mqw_50a":
                    byte tvm = 0x00;
                    if (co2_density > 0) tvm |= 0x01;
                    if (co_density > 0) tvm |= 0x02;
                    if (C3H8_density > 0) tvm |= 0x04;
                    if (no_density > 0) tvm |= 0x08;
                    if (o2_density > 0) tvm |= 0x10;

                    byte[] Content_MQ = new byte[] { DID, cmdDemarcate_MQ, 0x0e, tvm, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    temp_byte = BitConverter.GetBytes((ushort)(co2_density * 100));
                    Content_MQ[4] = temp_byte[0];
                    Content_MQ[5] = temp_byte[1];
                    temp_byte = BitConverter.GetBytes((ushort)(co_density * 100));
                    Content_MQ[6] = temp_byte[0];
                    Content_MQ[7] = temp_byte[1];
                    temp_byte = BitConverter.GetBytes((ushort)(C3H8_density * 100));
                    Content_MQ[8] = temp_byte[0];
                    Content_MQ[9] = temp_byte[1];
                    temp_byte = BitConverter.GetBytes((ushort)(no_density * 100));
                    Content_MQ[10] = temp_byte[0];
                    Content_MQ[11] = temp_byte[1];
                    temp_byte = BitConverter.GetBytes((ushort)(o2_density * 100));
                    Content_MQ[12] = temp_byte[0];
                    Content_MQ[13] = temp_byte[1];
                    Content_MQ[14] = getCS_MQ(Content_MQ, 14);
                    ComPort_1.Write(Content_MQ, 0, 15);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x69 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x8e)
                        return true;
                    else
                        return false;
                    break;
                case "nha_503":
                    byte[] naContent = new byte[] { };
                    Content = new byte[8];
                    temp_byte = BitConverter.GetBytes((ushort)C3H8_density);
                    Content[0] = temp_byte[1];
                    Content[1] = temp_byte[0];
                    temp_byte = BitConverter.GetBytes((ushort)(co_density * 100));
                    Content[2] = temp_byte[1];
                    Content[3] = temp_byte[0];
                    temp_byte = BitConverter.GetBytes((ushort)(co2_density * 100));
                    Content[4] = temp_byte[1];
                    Content[5] = temp_byte[0];
                    temp_byte = BitConverter.GetBytes((ushort)(no_density));
                    Content[6] = temp_byte[1];
                    Content[7] = temp_byte[0];
                    SendData(cmdWrStandardAir_nh503, Content);
                    //ComPort_1.Write(Content, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 2)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x00)
                        return true;
                    else
                        return false;
                    break;
                case "fasm_5000":
                    i = 0;
                    byte[] Content_FF = new byte[] { cmdEquipAddress_FF, cmdWriteDemacate_FF, 0X09, 0x00, 0x00 };
                    byte[] ContentDemarcate_FF = new byte[] { 0x00, 0x00, 0X00, 0x00, 0x00, 0x00, 0X00, 0x00, 0x00 };
                    Content_FF[4] = getCS_MQ(Content_FF, 4);
                    temp_byte = BitConverter.GetBytes((ushort)C3H8_density);
                    ContentDemarcate_FF[0] = temp_byte[0];
                    ContentDemarcate_FF[1] = temp_byte[1];
                    temp_byte = BitConverter.GetBytes((ushort)(co_density * 100));
                    ContentDemarcate_FF[2] = temp_byte[0];
                    ContentDemarcate_FF[3] = temp_byte[1];
                    temp_byte = BitConverter.GetBytes((ushort)(co2_density * 100));
                    ContentDemarcate_FF[4] = temp_byte[0];
                    ContentDemarcate_FF[5] = temp_byte[1];
                    temp_byte = BitConverter.GetBytes((ushort)(no_density * 100));
                    ContentDemarcate_FF[6] = temp_byte[0];
                    ContentDemarcate_FF[7] = temp_byte[1];
                    ContentDemarcate_FF[8] = getCS_MQ(ContentDemarcate_FF, 8);
                    ComPort_1.Write(cmdFrameHead_FF, 0, 2);        //发送开始测量命令
                    ComPort_1.Write(Content_FF, 0, 5);        //发送开始测量命令
                    ComPort_1.Write(ContentDemarcate_FF, 0, 9);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 16)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x07 && Read_Buffer[4] == 0x09 && Read_Buffer[5] == 0x00 && Read_Buffer[6] == 0xef)
                        return true;
                    else
                        return false;
                    break;
                default: return false; break;
            }
        }
        #endregion
        #region 输入气缸数
        /// <summary>
        /// 写入标定气深度，输入为零的表示不标定，有数值的表示要标定。
        /// </summary>
        /// <returns>bool</returns>
        public bool Set_QigangShu(byte gangshu)
        {

            ReadData();
            int i = 0;
            byte[] Content = new byte[1] { gangshu };
            SendData(cmdSetCylinderCount, Content);
            //ComPort_1.Write(Content, 0, 1);       //发送停止检查动作命令
            Thread.Sleep(10);
            while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
            {
                i++;
                Thread.Sleep(10);
                if (i == 100)
                    return false;
            }
            ReadData();                                 //读取返回的数据
            if (Read_Buffer[0] == 0x06)
                return true;
            else
                return false;
        }
        #endregion
        #region 标定气开
        /// <summary>
        /// 标定气开。
        /// </summary>
        /// <returns>bool</returns>
        public bool Open_standardGas()
        {
            ReadData();
            int i = 0;
            byte[] temp_byte = new byte[2];
            switch (yqxh)
            {
                case "fla_502":
                    byte[] Content = new byte[] { cmdOpenStandardGas };
                    ComPort_1.Write(Content, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "mqw_50a":
                    byte[] Content_MQ = new byte[] { DID, cmdOpenDemarcateGas_MQ, 0X03, 0 };
                    Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                    ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x7e && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x79)
                        return true;
                    else
                        return false;
                    break;
                case "nha_503":
                    
                    return true;
                    break;
                case "fasm_5000":
                    ReadData();
                    //Struct_Now = Get_Struct();
                    //Zeroing();
                    i = 0;
                    byte[] Content_FF = new byte[] { cmdEquipAddress_FF, cmdPumpDemarcateGas_FF, 0X00, 0x00, 0x00 };
                    Content_FF[4] = getCS_MQ(Content_FF, 4);
                    ComPort_1.Write(cmdFrameHead_FF, 0, 2);        //发送开始测量命令
                    ComPort_1.Write(Content_FF, 0, 5);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x0f && Read_Buffer[4] == 0x00 && Read_Buffer[5] == 0x00 && Read_Buffer[6] == 0xf0)
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
        #region 标定气关
        /// <summary>
        /// 标定气关。
        /// </summary>
        /// <returns>bool</returns>
        public bool Close_standardGas()
        {
            ReadData();
            int i = 0;
            byte[] temp_byte = new byte[2];
            switch (yqxh)
            {
                case "fla_502":
                    byte[] Content = new byte[] { cmdCloseStandardGas };
                    ComPort_1.Write(Content, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "mqw_50a":
                    byte[] Content_MQ = new byte[] { DID, cmdStopAction_MQ, 0X03, 0 };
                    Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                    ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x78 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x7f)
                        return true;
                    else
                        return false;
                    break;
                case "nha_503":
                    return true;
                    break;
                case "fasm_5000":
                    ReadData();
                    //Struct_Now = Get_Struct();
                    //Zeroing();
                    i = 0;
                    byte[] Content_FF = new byte[] { cmdEquipAddress_FF, cmdStopPump_FF, 0X00, 0x00, 0x00 };
                    Content_FF[4] = getCS_MQ(Content_FF, 4);
                    ComPort_1.Write(cmdFrameHead_FF, 0, 2);        //发送开始测量命令
                    ComPort_1.Write(Content_FF, 0, 5);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x0a && Read_Buffer[4] == 0x00 && Read_Buffer[5] == 0x00 && Read_Buffer[6] == 0xf5)
                        return true;
                    else
                        return false;
                    break;
                default: return false; break;
            }
        }
        #endregion

        #region 检查是否低流量
        /// <summary>
        /// 仪器是否可以进行测量
        /// </summary>
        /// <returns>string 仪器状态</returns>
        public int CheckIsLowFlow()
        {
            int i = 0;
            string msg = "";
            byte Status = 0;
            switch (yqxh)
            {
                case "fla_502":
                    byte[] Content = new byte[] { cmdGetDat };
                    ComPort_1.Write(Content, 0, 1);//
                    Thread.Sleep(500);
                    while (ComPort_1.BytesToRead < 21)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return -1;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x6)
                    {
                        byte[] temp_byte = new byte[2];                        
                        temp_byte[0] = Read_Buffer[16];
                        temp_byte[1] = Read_Buffer[15];
                        if (BitConverter.ToUInt16(temp_byte, 0) == 0x8888)
                            return -2;        //流量 
                        else if (BitConverter.ToUInt16(temp_byte, 0) == 0x0000)
                            return 0;        //流量 

                    }
                    return 0;
                    break;
                case cdxh:
                    byte[] ContentCD = new byte[] { CDcmdGetDat };
                    ComPort_1.Write(ContentCD, 0, 1);//
                    Thread.Sleep(500);
                    while (ComPort_1.BytesToRead < 19)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return -1;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x6)
                    {
                        byte[] temp_byte = new byte[2];
                        //if (BitConverter.ToUInt16(temp_byte, 0) == 0x8888)
                        //    return -2;        //流量 
                        //else if (BitConverter.ToUInt16(temp_byte, 0) == 0x0000)
                            return 0;        //流量 

                    }
                    return 0;
                    break;
                case "nha_503":
                    byte[] naContent = new byte[] { cmdGetDat_nh503 };
                    ComPort_1.Write(naContent, 0, 1);//
                    Thread.Sleep(500);
                    while (ComPort_1.BytesToRead < 19)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return -1;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x6)
                    {
                        byte[] temp_byte = new byte[2];                       
                        temp_byte[0] = Read_Buffer[16];
                        temp_byte[1] = Read_Buffer[15];
                        if (BitConverter.ToUInt16(temp_byte, 0) == 0x8888)
                            return -2;        //流量 
                        else if (BitConverter.ToUInt16(temp_byte, 0) == 0x0000)
                            return 0;        //流量 
                    }
                    return 0;
                    break;
                case "mqw_50a":
                    ReadData();
                    i = 0;
                    byte[] Content_MQ = new byte[] { DID, cmdGetStatus_MQ, 0X03, 0 };
                    Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                    ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 11)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return -1;//未通讯上
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x61)
                    {
                        Status = Convert.ToByte(Read_Buffer[3] & 0xff);
                        if ((Status & 0x80) == 0x80) return -2;//低流量
                        else return 0;
                    }
                    else
                        return 0;
                    break;
                default:
                    return 0;
                    break;
            }

        }
        #endregion
        #region 标定
        /// <summary>
        /// 标定气开。
        /// </summary>
        /// <returns>bool</returns>
        public string startDemarcate()
        {
            ReadData();
            int i = 0;
            byte[] temp_byte = new byte[2];
            switch (yqxh)
            {
                case "fla_502":
                    byte[] Content = new byte[] { cmdDemarcate };
                    ComPort_1.Write(Content, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 14)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                        {
                            if (ComPort_1.BytesToRead == 1)
                            {
                                ReadData();
                                if (Read_Buffer[0] == 0x00)
                                    return "标定成功";
                                else if (Read_Buffer[0] == 0x01)
                                    return "标定失败";
                            }
                            else
                            {
                                return "标定失败";
                            }
                        }
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x05)
                        return "正在标定中";
                    else
                        return "通讯异常";
                    break;
                case cdxh:
                    byte[] ContentCD = new byte[] { CDcmdDemarcate };
                    ComPort_1.Write(ContentCD, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 14)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                        {
                            if (ComPort_1.BytesToRead == 1)
                            {
                                ReadData();
                                if (Read_Buffer[0] == 0x00)
                                    return "标定成功";
                                else if (Read_Buffer[0] == 0x01)
                                    return "标定失败";
                            }
                            else
                            {
                                return "标定失败";
                            }
                        }
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x05)
                        return "正在标定中";
                    else
                        return "通讯异常";
                    break;
                case "nha_503":
                    byte[] naContent = new byte[] { cmdDemarcate_nh503 };
                    ComPort_1.Write(naContent, 0, 1);       //发送停止检查动作命令
                    ini.INIIO.saveLogInf("[废气仪发送]:" + byteToHexStr(naContent));
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 14)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                        {
                            if (ComPort_1.BytesToRead == 1)
                            {
                                ReadData();
                                if (Read_Buffer[0] == 0x00)
                                    return "标定成功";
                                else if (Read_Buffer[0] == 0x01)
                                    return "标定失败";
                            }
                            else
                            {
                                return "标定失败";
                            }
                        }
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x05)
                        return "正在标定中";
                    else
                        return "通讯异常";
                    break;
                default: return "未提示该型号该功能"; break;
            }
        }
        #endregion
        #region 0气开
        /// <summary>
        /// 0气开。
        /// </summary>
        /// <returns>bool</returns>
        public bool Open_zeroGas()
        {
            ReadData();
            int i = 0;
            byte[] temp_byte = new byte[2];
            switch (yqxh)
            {
                case "fla_502":
                    byte[] Content = new byte[] { cmdOpenZeroGas };
                    ComPort_1.Write(Content, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "mqw_50a":
                    byte[] Content_MQ = new byte[] { DID, cmdPumpAir_MQ, 0X03, 0 };
                    Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                    ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x7b && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x7c)
                        return true;
                    else
                        return false;
                    break;
                case "nha_503":
                    byte[] naContent = new byte[] { cmdOpenZeroGas_nh503 };
                    ComPort_1.Write(naContent, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "fasm_5000":
                    ReadData();
                    //Struct_Now = Get_Struct();
                    //Zeroing();
                    i = 0;
                    byte[] Content_FF = new byte[] { cmdEquipAddress_FF, cmdPumpZeroAir_FF, 0X00, 0x00, 0x00 };
                    Content_FF[4] = getCS_MQ(Content_FF, 4);
                    ComPort_1.Write(cmdFrameHead_FF, 0, 2);        //发送开始测量命令
                    ComPort_1.Write(Content_FF, 0, 5);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x0d && Read_Buffer[4] == 0x00 && Read_Buffer[5] == 0x00 && Read_Buffer[6] == 0xf2)
                        return true;
                    else
                        return false;
                    break;
                default: return false; break;
            }
        }
        #endregion
        #region 0气关
        /// <summary>
        /// 0气关。
        /// </summary>
        /// <returns>bool</returns>
        public bool Close_zeroGas()
        {
            ReadData();
            int i = 0;
            byte[] temp_byte = new byte[2];
            switch (yqxh)
            {
                case "fla_502":
                    byte[] Content = new byte[] { cmdCloseZeroGas };
                    ComPort_1.Write(Content, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "mqw_50a":
                    byte[] Content_MQ = new byte[] { DID, cmdStopAction_MQ, 0X03, 0 };
                    Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                    ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x78 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x7f)
                        return true;
                    else
                        return false;
                    break;
                case "nha_503":
                    byte[] naContent = new byte[] { cmdCloseZeroGas_nh503 };
                    ComPort_1.Write(naContent, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "fasm_5000":
                    ReadData();
                    //Struct_Now = Get_Struct();
                    //Zeroing();
                    i = 0;
                    byte[] Content_FF = new byte[] { cmdEquipAddress_FF, cmdStopPump_FF, 0X00, 0x00, 0x00 };
                    Content_FF[4] = getCS_MQ(Content_FF, 4);
                    ComPort_1.Write(cmdFrameHead_FF, 0, 2);        //发送开始测量命令
                    ComPort_1.Write(Content_FF, 0, 5);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x0a && Read_Buffer[4] == 0x00 && Read_Buffer[5] == 0x00 && Read_Buffer[6] == 0xf5)
                        return true;
                    else
                        return false;
                    break;
                default: return false; break;
            }
        }
        #endregion
        #region 检查气开
        /// <summary>
        /// 检查气开。
        /// </summary>
        /// <returns>bool</returns>
        public bool Open_testGas()
        {
            ReadData();
            int i = 0;
            byte[] temp_byte = new byte[2];
            switch (yqxh)
            {
                case "fla_502":
                    byte[] Content = new byte[] { cmdOpenTestGas };
                    ComPort_1.Write(Content, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case cdxh:
                    byte[] ContentCD = new byte[] { CDcmdOpenTestGas };
                    ComPort_1.Write(ContentCD, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "mqw_50a":
                    byte[] Content_MQ = new byte[] { DID, cmdOpenTestGas_MQ, 0X03, 0 };
                    Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                    ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x7f && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x78)
                        return true;
                    else
                        return false;
                    break;
                case "nha_503":
                    byte[] naContent = new byte[] { cmdOpenTestGas_nh503 };
                    if (isNhSelfUse)
                        naContent[0] = cmdOpenTestGas_nha504;
                    ComPort_1.Write(naContent, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "fasm_5000":
                    ReadData();
                    //Struct_Now = Get_Struct();
                    //Zeroing();
                    i = 0;
                    byte[] Content_FF = new byte[] { cmdEquipAddress_FF, cmdPumpTestGas_FF, 0X00, 0x00, 0x00 };
                    Content_FF[4] = getCS_MQ(Content_FF, 4);
                    ComPort_1.Write(cmdFrameHead_FF, 0, 2);        //发送开始测量命令
                    ComPort_1.Write(Content_FF, 0, 5);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x0e && Read_Buffer[4] == 0x00 && Read_Buffer[5] == 0x00 && Read_Buffer[6] == 0xf1)
                        return true;
                    else
                        return false;
                    break;
                default: return false; break;
            }
        }
        #endregion
        #region 检查气关
        /// <summary>
        /// 检查气关。
        /// </summary>
        /// <returns>bool</returns>
        public bool Close_testGas()
        {
            ReadData();
            int i = 0;
            byte[] temp_byte = new byte[2];
            switch (yqxh)
            {
                case "fla_502":
                    byte[] Content = new byte[] { cmdCloseTestGas };
                    ComPort_1.Write(Content, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case cdxh:
                    byte[] ContentCD = new byte[] { CDcmdCloseTestGas };
                    ComPort_1.Write(ContentCD, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "mqw_50a":
                    byte[] Content_MQ = new byte[] { DID, 0x78, 0X03, 0 };
                    Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                    ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x78 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x7f)
                        return true;
                    else
                        return false;
                    break;
                case "nha_503":
                    byte[] naContent = new byte[] { cmdCloseTestGas_nh503 };
                    if (isNhSelfUse)
                        naContent[0] = cmdCloseTestGas_nha504;
                    ComPort_1.Write(naContent, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "fasm_5000":
                    ReadData();
                    //Struct_Now = Get_Struct();
                    //Zeroing();
                    i = 0;
                    byte[] Content_FF = new byte[] { cmdEquipAddress_FF, cmdStopPump_FF, 0X00, 0x00, 0x00 };
                    Content_FF[4] = getCS_MQ(Content_FF, 4);
                    ComPort_1.Write(cmdFrameHead_FF, 0, 2);        //发送开始测量命令
                    ComPort_1.Write(Content_FF, 0, 5);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x0a && Read_Buffer[4] == 0x00 && Read_Buffer[5] == 0x00 && Read_Buffer[6] == 0xf5)
                        return true;
                    else
                        return false;
                    break;
                default: return false; break;
            }
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
            byte[] temp_byte = new byte[2];
            switch (yqxh)
            {
                case "fla_502":
                    byte[] Content = new byte[] { cmdPrepareTest };
                    ComPort_1.Write(Content, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case cdxh:
                    byte[] ContentCD = new byte[] { CDcmdStopClear };
                    ComPort_1.Write(ContentCD, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "nha_503":
                    byte[] naContent = new byte[] { cmdPrepareTest_nh503 };
                    ComPort_1.Write(naContent, 0, 1);       //发送停止检查动作命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                                 //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                default: return false; break;
            }
        }
        #endregion




        #region 反吹
        /// <summary>
        /// 反吹(反吹过程大概需要30秒 完成后须调用Stop_Check方法停止)
        /// </summary>
        /// <returns>bool</returns>
        public bool Blowback()
        {
            try
            {
                int i = 0;
                switch (yqxh)
                {
                    case "fla_502":
                        ReadData();
                        i = 0;
                        byte[] Content = new byte[] { cmdClearPipe };
                        ComPort_1.Write(Content, 0, 1);

                        //SendData(Cmd_Blowback, Content);        //发送反吹命令
                        Thread.Sleep(10);
                        i = 0;
                        while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 100)
                                return false;
                        }
                        ReadData();                         //读取数据
                        if (Read_Buffer[0] == 0x06)
                            return true;
                        else
                            return false;
                        break;
                    case cdxh:
                        ReadData();
                        i = 0;
                        byte[] ContentCD = new byte[] { CDcmdClearPipe };
                        ComPort_1.Write(ContentCD, 0, 1);

                        //SendData(Cmd_Blowback, Content);        //发送反吹命令
                        Thread.Sleep(10);
                        i = 0;
                        while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 100)
                                return false;
                        }
                        ReadData();                         //读取数据
                        if (Read_Buffer[0] == 0x06)
                            return true;
                        else
                            return false;
                        break;
                    case "mqw_50a":
                        ReadData();
                        i = 0;
                        byte[] Content_MQ = new byte[] { DID, cmdFlowBack_MQ, 0X03, 0 };
                        Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                        ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                        Thread.Sleep(10);
                        while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
                                return false;
                        }
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x80 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x77)
                            return true;
                        else
                            return false;
                        break;
                    case "mqw_511":
                        ReadData();
                        i = 0;
                        byte[] Content_MQ511 = new byte[] { 0x01 };
                        ComPort_1.Write(Content_MQ511, 0, 1);        //发送开始测量命令
                        Thread.Sleep(10);
                        while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
                                return false;
                        }
                        ReadData();
                        return true;
                        break;
                    case "nha_503":
                        ReadData();
                        i = 0;
                        byte[] naContent = new byte[] { cmdClearPipe_nh503 };
                        ComPort_1.Write(naContent, 0, 1);

                        //SendData(Cmd_Blowback, Content);        //发送反吹命令
                        Thread.Sleep(10);
                        i = 0;
                        while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 100)
                                return false;
                        }
                        ReadData();                         //读取数据
                        if (Read_Buffer[0] == 0x06)
                            return true;
                        else
                            return false;
                        break;
                    case "fasm_5000":
                        ReadData();
                        //Struct_Now = Get_Struct();
                        //Zeroing();
                        i = 0;
                        byte[] Content_FF = new byte[] { cmdEquipAddress_FF, cmdBlowBack_FF, 0X02, 0x00, 0x00, 0xff, 0x01 };
                        Content_FF[4] = getCS_MQ(Content_FF, 4);
                        ComPort_1.Write(cmdFrameHead_FF, 0, 2);        //发送开始测量命令
                        ComPort_1.Write(Content_FF, 0, 7);        //发送开始测量命令
                        Thread.Sleep(10);
                        while (ComPort_1.BytesToRead < 9)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
                                return false;
                        }
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x10 && Read_Buffer[4] == 0x00 && Read_Buffer[5] == 0x00 && Read_Buffer[6] == 0xed)
                            return true;
                        else
                            return false;
                        break;
                    case "nha_506": return true;break;
                    default:
                        return false;
                        break;
                }
            }
            catch
            { return false; }
            //}
            //else
            //    return false;
        }
        #endregion

        #region 停止反吹
        /// <summary>
        /// 反吹过程大概需要30秒 完成后须该方法停止)
        /// </summary>
        /// <returns>bool</returns>
        public bool StopBlowback()
        {
            try
            {
                int i = 0;
                switch (yqxh)
                {
                    case "fla_502":
                        ReadData();
                        i = 0;
                        byte[] Content = new byte[] { cmdStopClear };
                        ComPort_1.Write(Content, 0, 1);
                        Thread.Sleep(10);
                        i = 0;
                        while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 100)
                                return false;
                        }
                        ReadData();                         //读取数据
                        if (Read_Buffer[0] == 0x06)
                            return true;
                        else
                            return false;
                        break;
                    case cdxh:
                        ReadData();
                        i = 0;
                        byte[] ContentCD = new byte[] { CDcmdStopClear };
                        ComPort_1.Write(ContentCD, 0, 1);
                        Thread.Sleep(10);
                        i = 0;
                        while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 100)
                                return false;
                        }
                        ReadData();                         //读取数据
                        if (Read_Buffer[0] == 0x06)
                            return true;
                        else
                            return false;
                        break;
                    case "nha_503":
                        ReadData();
                        i = 0;
                        byte[] naContent = new byte[] { cmdStopClear_nh503 };
                        ComPort_1.Write(naContent, 0, 1);
                        Thread.Sleep(10);
                        i = 0;
                        while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 100)
                                return false;
                        }
                        ReadData();                         //读取数据
                        if (Read_Buffer[0] == 0x06)
                            return true;
                        else
                            return false;
                        break;
                    case "mqw_50a":
                        ReadData();
                        i = 0;
                        byte[] Content_MQ = new byte[] { DID, cmdStopAction_MQ, 0X03, 0 };
                        Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                        ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                        Thread.Sleep(10);
                        while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
                                return false;
                        }
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x78 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x7f)
                            return true;
                        else
                            return false;
                        break;
                    case "mqw_511":
                        ReadData();
                        i = 0;
                        byte[] Content_MQ511 = new byte[] { 0x02 };
                        ComPort_1.Write(Content_MQ511, 0, 1);        //发送开始测量命令
                        Thread.Sleep(10);
                        while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
                                return false;
                        }
                        ReadData();
                        return true;
                        break;
                    case "fasm_5000":
                        ReadData();
                        //Struct_Now = Get_Struct();
                        //Zeroing();
                        i = 0;
                        byte[] Content_FF = new byte[] { cmdEquipAddress_FF, cmdStopPump_FF, 0X00, 0x00, 0x00 };
                        Content_FF[4] = getCS_MQ(Content_FF, 4);
                        ComPort_1.Write(cmdFrameHead_FF, 0, 2);        //发送开始测量命令
                        ComPort_1.Write(Content_FF, 0, 5);        //发送开始测量命令
                        Thread.Sleep(10);
                        while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 200)
                                return false;
                        }
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x0a && Read_Buffer[4] == 0x00 && Read_Buffer[5] == 0x00 && Read_Buffer[6] == 0xf5)
                            return true;
                        else
                            return false;
                        break;
                    case "nha_506":
                        ReadData();
                        i = 0;
                        byte[] nha_506 = new byte[] { cmdStopAction506 };
                        ComPort_1.Write(nha_506, 0, 1);
                        Thread.Sleep(10);
                        i = 0;
                        while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(10);
                            if (i == 100)
                                return false;
                        }
                        ReadData();                         //读取数据
                        if (Read_Buffer[0] == 0x06)
                            return true;
                        else
                            return false;
                        break;
                    default:
                        return false;
                        break;
                }
            }
            catch
            {
                return false;
            }

        }
        #endregion
        #region 调零
        /// <summary>
        /// 调零(调零过程大概为30秒，可以通过Get_Struct方法检查是否调零成功)
        /// </summary>
        /// <returns>bool</returns>
        public bool Zeroing()
        {
            int i = 0;
            switch (yqxh)
            {
                case "fla_502":
                    ReadData();
                    i = 0;
                    byte[] Content = new byte[] { cmdZeroing };
                    ComPort_1.Write(Content, 0, 1);//发送调零命令
                    Thread.Sleep(10);
                    i = 0;
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                         //读取数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case cdxh:
                    ReadData();
                    i = 0;
                    byte[] ContentCD = new byte[] { CDcmdZeroing };
                    ComPort_1.Write(ContentCD, 0, 1);//发送调零命令
                    Thread.Sleep(10);
                    i = 0;
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                         //读取数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "nha_503":
                    ReadData();
                    i = 0;
                    byte[] naContent = new byte[] { cmdZeroing_nh503 };
                    ComPort_1.Write(naContent, 0, 1);//发送调零命令
                    Thread.Sleep(10);
                    i = 0;
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                         //读取数据
                    if (Read_Buffer[0] == 0x00 || Read_Buffer[0] == 0x01)//0x00和0x01表示调零完成，其他表示失败
                        return true;
                    else
                        return false;
                    break;
                case "mqw_50a":
                    ReadData();
                    i = 0;
                    byte[] Content_MQ = new byte[] { DID, cmdZero_MQ, 0X03, 0 };
                    Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                    ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x67 && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x90)
                        return true;
                    else
                        return false;
                    break;
                case "fasm_5000":
                    ReadData();
                    //Struct_Now = Get_Struct();
                    //Zeroing();
                    i = 0;
                    byte[] Content_FF = new byte[] { cmdEquipAddress_FF, cmdZeroing_FF, 0X00, 0x00, 0x00 };
                    Content_FF[4] = getCS_MQ(Content_FF, 4);
                    ComPort_1.Write(cmdFrameHead_FF, 0, 2);        //发送开始测量命令
                    ComPort_1.Write(Content_FF, 0, 5);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x15 && Read_Buffer[4] == 0x00 && Read_Buffer[5] == 0x00 && Read_Buffer[6] == 0xea)
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
        #region 设置调零气体为空气
        /// <summary>
        /// 
        /// </summary>
        /// <returns>bool</returns>
        public bool setAirAsTl()
        {
            int i = 0;
            switch (yqxh)
            {
                case "fla_502":
                    ReadData();
                    //int i = 0;
                    byte[] Content = new byte[] { 0x15 };
                    ComPort_1.Write(Content, 0, 1);//发送调零命令
                    Thread.Sleep(10);
                    i = 0;
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                         //读取数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case cdxh:
                    ReadData();
                    //int i = 0;
                    byte[] ContentCD = new byte[] { CDcmdZeroIsAir };
                    ComPort_1.Write(ContentCD, 0, 1);//发送调零命令
                    Thread.Sleep(10);
                    i = 0;
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                         //读取数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "nha_503":
                    ReadData();
                    //int i = 0;
                    byte[] naContent = new byte[] { 15 };
                    ComPort_1.Write(naContent, 0, 1);//发送调零命令
                    Thread.Sleep(10);
                    i = 0;
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                         //读取数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "fasm_5000":
                    ReadData();
                    //Struct_Now = Get_Struct();
                    //Zeroing();
                    i = 0;
                    byte[] Content_FF = new byte[] { cmdEquipAddress_FF, cmdSetZeroGas_FF, 0X02, 0x00, 0x00, 0x00, 0x00 };
                    Content_FF[4] = getCS_MQ(Content_FF, 4);
                    ComPort_1.Write(cmdFrameHead_FF, 0, 2);        //发送开始测量命令
                    ComPort_1.Write(Content_FF, 0, 7);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01)
                        return true;
                    else
                        return false;
                    break;
                default: return false; break;
            }
        }
        #endregion
        #region 设置调零气体为零气
        /// <summary>
        /// 
        /// </summary>
        /// <returns>bool</returns>
        public bool setZeroAsTl()
        {
            int i = 0;
            switch (yqxh)
            {
                case "fla_502":
                    //int i = 0;
                    byte[] Content = new byte[] { 0x16 };
                    ComPort_1.Write(Content, 0, 1);//发送调零命令
                    Thread.Sleep(10);
                    i = 0;
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                         //读取数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case cdxh:
                    ReadData();
                    //int i = 0;
                    byte[] ContentCD = new byte[] { CDcmdZeroIsZero };
                    ComPort_1.Write(ContentCD, 0, 1);//发送调零命令
                    Thread.Sleep(10);
                    i = 0;
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                         //读取数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "nha_503":
                    //int i = 0;
                    byte[] naContent = new byte[] { 16 };
                    ComPort_1.Write(naContent, 0, 1);//发送调零命令
                    Thread.Sleep(10);
                    i = 0;
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                         //读取数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "fasm_5000":
                    ReadData();
                    //Struct_Now = Get_Struct();
                    //Zeroing();
                    i = 0;
                    byte[] Content_FF = new byte[] { cmdEquipAddress_FF, cmdSetZeroGas_FF, 0X02, 0x00, 0xeb, 0x01, 0xff };
                    Content_FF[4] = getCS_MQ(Content_FF, 4);
                    ComPort_1.Write(cmdFrameHead_FF, 0, 2);        //发送开始测量命令
                    ComPort_1.Write(Content_FF, 0, 7);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 7)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01)
                        return true;
                    else
                        return false;
                    break;
                default: return false; break;
            }
        }
        #endregion
        private Fla502_data Fla502_data = new Fla502_data();
        #region 获取实时数据
        /// <summary>
        /// 获取实时数据 至少耗时500ms
        /// </summary>
        /// <returns>Fla502_data  如果值全为0则表明通讯失败</returns>
        public Fla502_data GetData()
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case "fla_502":
                    byte[] Content = new byte[] { cmdGetDat };
                    ComPort_1.Write(Content, 0, 1);//
                    Thread.Sleep(500);
                    while (ComPort_1.BytesToRead < 21)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return Fla502_data;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x6)
                    {
                        byte[] temp_byte = new byte[2];
                        temp_byte[0] = Read_Buffer[2];
                        temp_byte[1] = Read_Buffer[1];
                        Fla502_data.HC =(float)(Math.Round(hcxs*( BitConverter.ToInt16(temp_byte, 0)),0));         //HC
                        if (Fla502_data.HC <= 0) Fla502_data.HC = 1f;
                        temp_byte[0] = Read_Buffer[4];
                        temp_byte[1] = Read_Buffer[3];
                        Fla502_data.CO = (float)(Math.Round(coxs * (BitConverter.ToInt16(temp_byte, 0) / 1000f),3));       //CO
                        if (Fla502_data.CO <= 0) Fla502_data.CO = 0.01f;
                        temp_byte[0] = Read_Buffer[6];
                        temp_byte[1] = Read_Buffer[5];
                        Fla502_data.CO2 = BitConverter.ToInt16(temp_byte, 0) / 100f;               //CO2
                        temp_byte[0] = Read_Buffer[8];
                        temp_byte[1] = Read_Buffer[7];
                        Fla502_data.O2 = BitConverter.ToInt16(temp_byte, 0) / 100f;               //O2
                        temp_byte[0] = Read_Buffer[10];
                        temp_byte[1] = Read_Buffer[9];
                        Fla502_data.NO = (float)(Math.Round(noxs * (BitConverter.ToInt16(temp_byte, 0)), 0));          //NO
                        if (Fla502_data.NO <= 0) Fla502_data.NO = 1f;
                        temp_byte[0] = Read_Buffer[12];
                        temp_byte[1] = Read_Buffer[11];
                        Fla502_data.ZS = BitConverter.ToInt16(temp_byte, 0);         //转速
                        temp_byte[0] = Read_Buffer[14];
                        temp_byte[1] = Read_Buffer[13];
                        Fla502_data.YW = BitConverter.ToInt16(temp_byte, 0) / 10f;         //油温
                        temp_byte[0] = Read_Buffer[16];
                        temp_byte[1] = Read_Buffer[15];
                        if (BitConverter.ToUInt16(temp_byte, 0) == 0x8888)
                            Fla502_data.LL = "0";        //流量 
                        else if (BitConverter.ToUInt16(temp_byte, 0) == 0x0000)
                            Fla502_data.LL = "1";        //流量 
                        temp_byte[0] = Read_Buffer[19];
                        temp_byte[1] = Read_Buffer[18];
                        Fla502_data.λ = BitConverter.ToInt16(temp_byte, 0) / 100f;             

                    }
                    return Fla502_data;
                    break;
                case cdxh:
                    byte[] ContentCD = new byte[] { CDcmdGetDat };
                    ComPort_1.Write(ContentCD, 0, 1);//
                    Thread.Sleep(500);
                    while (ComPort_1.BytesToRead < 19)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return Fla502_data;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x6)
                    {
                        byte[] temp_byte = new byte[2];
                        temp_byte[0] = Read_Buffer[2];
                        temp_byte[1] = Read_Buffer[1];
                        Fla502_data.HC = (float)(Math.Round(hcxs * (BitConverter.ToInt16(temp_byte, 0)), 0));         //HC
                        if (Fla502_data.HC <= 0) Fla502_data.HC = 1f;
                        temp_byte[0] = Read_Buffer[4];
                        temp_byte[1] = Read_Buffer[3];
                        Fla502_data.CO = (float)(Math.Round(coxs * (BitConverter.ToInt16(temp_byte, 0) / 100f), 2));       //CO
                        if (Fla502_data.CO <= 0) Fla502_data.CO = 0.01f;
                        temp_byte[0] = Read_Buffer[6];
                        temp_byte[1] = Read_Buffer[5];
                        Fla502_data.CO2 = BitConverter.ToInt16(temp_byte, 0) / 100f;               //CO2
                        temp_byte[0] = Read_Buffer[8];
                        temp_byte[1] = Read_Buffer[7];
                        Fla502_data.O2 = BitConverter.ToInt16(temp_byte, 0) / 100f;               //O2
                        temp_byte[0] = Read_Buffer[10];
                        temp_byte[1] = Read_Buffer[9];
                        Fla502_data.NO = (float)(Math.Round(noxs * (BitConverter.ToInt16(temp_byte, 0)), 0));          //NO
                        if (Fla502_data.NO <= 0) Fla502_data.NO = 1f;
                        temp_byte[0] = Read_Buffer[12];
                        temp_byte[1] = Read_Buffer[11];
                        Fla502_data.ZS = BitConverter.ToInt16(temp_byte, 0);         //转速
                        temp_byte[0] = Read_Buffer[14];
                        temp_byte[1] = Read_Buffer[13];
                        Fla502_data.YW = BitConverter.ToInt16(temp_byte, 0);         //油温
                        //if (BitConverter.ToUInt16(temp_byte, 0) == 0x8888)
                        //    Fla502_data.LL = "0";        //流量 
                        //else if (BitConverter.ToUInt16(temp_byte, 0) == 0x0000)
                            Fla502_data.LL = "1";        //流量 
                        temp_byte[0] = Read_Buffer[16];
                        temp_byte[1] = Read_Buffer[15];
                        //Fla502_data.λ = BitConverter.ToInt16(temp_byte, 0) / 100f;
                        if ((Fla502_data.CO + Fla502_data.CO2) == 0)
                            Fla502_data.λ = 0;
                        else
                            Fla502_data.λ = (float)((Fla502_data.CO2 + Fla502_data.CO / 2f + Fla502_data.O2 + ((1.7261 / 4f * 3.5 / (3.5 + Fla502_data.CO / Fla502_data.CO2) - 0.0176 / 2f) * (Fla502_data.CO2 + Fla502_data.CO))) / ((1 + 1.7261 / 4f - 0.0176 / 2) * (Fla502_data.CO2 + Fla502_data.CO + 0.0006 * Fla502_data.HC)));
                        if (Fla502_data.λ < 0 || Fla502_data.λ > 100f)
                            Fla502_data.λ = 0;

                    }
                    return Fla502_data;
                    break;
                case "nha_503":
                    byte[] naContent = new byte[] { cmdGetDat_nh503 };
                    ComPort_1.Write(naContent, 0, 1);//
                    ini.INIIO.saveLogInf("【废气仪发送】:" + byteToHexStr(naContent));
                    Thread.Sleep(500);
                    while (ComPort_1.BytesToRead < 19)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return Fla502_data;
                    }
                    bool isnewnha = (ComPort_1.BytesToRead >= 21);
                    ReadData();
                    if (Read_Buffer[0] == 0x6)
                    {
                        byte[] temp_byte = new byte[2];
                        temp_byte[0] = Read_Buffer[2];
                        temp_byte[1] = Read_Buffer[1];
                        Fla502_data.HC = BitConverter.ToInt16(temp_byte, 0);         //HC
                        if (Fla502_data.HC <= 0) Fla502_data.HC = 1f;
                        temp_byte[0] = Read_Buffer[4];
                        temp_byte[1] = Read_Buffer[3];
                        Fla502_data.CO = BitConverter.ToInt16(temp_byte, 0) / 100f; ;       //CO
                        if (Fla502_data.CO <= 0) Fla502_data.CO = 0.01f;
                        temp_byte[0] = Read_Buffer[6];
                        temp_byte[1] = Read_Buffer[5];
                        Fla502_data.CO2 = BitConverter.ToInt16(temp_byte, 0) / 100f;               //CO2
                        temp_byte[0] = Read_Buffer[8];
                        temp_byte[1] = Read_Buffer[7];
                        Fla502_data.O2 = BitConverter.ToInt16(temp_byte, 0) / 100f;               //O2
                        temp_byte[0] = Read_Buffer[10];
                        temp_byte[1] = Read_Buffer[9];
                        Fla502_data.NO = BitConverter.ToInt16(temp_byte, 0);          //NO
                        if (Fla502_data.NO <= 0) Fla502_data.NO = 1f;
                        temp_byte[0] = Read_Buffer[12];
                        temp_byte[1] = Read_Buffer[11];
                        Fla502_data.ZS = BitConverter.ToInt16(temp_byte, 0);         //转速
                        temp_byte[0] = Read_Buffer[14];
                        temp_byte[1] = Read_Buffer[13];
                        Fla502_data.YW = BitConverter.ToInt16(temp_byte, 0) / 10f;         //油温
                        temp_byte[0] = Read_Buffer[16];
                        temp_byte[1] = Read_Buffer[15];
                        if (BitConverter.ToUInt16(temp_byte, 0) == 0x8888)
                            Fla502_data.LL = "0";        //流量 
                        else if (BitConverter.ToUInt16(temp_byte, 0) == 0x0000)
                            Fla502_data.LL = "1";        //流量 
                        if (isnewnha)
                        {
                            temp_byte[0] = Read_Buffer[19];
                            temp_byte[1] = Read_Buffer[18];
                            Fla502_data.λ = BitConverter.ToInt16(temp_byte, 0) / 100f;
                        }
                        else
                        {
                            Fla502_data.λ = 1.0f + (Fla502_data.CO2 - 7) * 0.003f;
                        }
                        //if ((Fla502_data.CO + Fla502_data.CO2) == 0)
                        //    Fla502_data.λ = 0;
                        //else
                        //    Fla502_data.λ = (float)((Fla502_data.CO2 + Fla502_data.CO / 2f + Fla502_data.O2 + ((1.7261 / 4f * 3.5 / (3.5 + Fla502_data.CO / Fla502_data.CO2) - 0.0176 / 2f) * (Fla502_data.CO2 + Fla502_data.CO))) / ((1 + 1.7261 / 4f - 0.0176 / 2) * (Fla502_data.CO2 + Fla502_data.CO + 0.0006 * Fla502_data.HC)));
                        //if (Fla502_data.λ < 0 || Fla502_data.λ > 100f)
                        //    Fla502_data.λ = 0;

                    }
                    return Fla502_data;
                    break;
                case "nha_506":
                    byte[] na506Content = new byte[] { cmdGetData506 };
                    ComPort_1.Write(na506Content, 0, 1);//
                    ini.INIIO.saveLogInf("【废气仪发送】:" + byteToHexStr(na506Content));
                    Thread.Sleep(500);
                    while (ComPort_1.BytesToRead < 18)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return Fla502_data;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x6)
                    {
                        byte[] temp_byte = new byte[2];
                        temp_byte[0] = Read_Buffer[2];
                        temp_byte[1] = Read_Buffer[1];
                        Fla502_data.HC = BitConverter.ToInt16(temp_byte, 0);         //HC
                        if (Fla502_data.HC <= 0) Fla502_data.HC = 1f;
                        temp_byte[0] = Read_Buffer[4];
                        temp_byte[1] = Read_Buffer[3];
                        Fla502_data.CO = BitConverter.ToInt16(temp_byte, 0) / 100f; ;       //CO
                        if (Fla502_data.CO <= 0) Fla502_data.CO = 0.01f;
                        temp_byte[0] = Read_Buffer[6];
                        temp_byte[1] = Read_Buffer[5];
                        Fla502_data.CO2 = BitConverter.ToInt16(temp_byte, 0) / 100f;               //CO2
                        temp_byte[0] = Read_Buffer[8];
                        temp_byte[1] = Read_Buffer[7];
                        Fla502_data.O2 = BitConverter.ToInt16(temp_byte, 0) / 100f;               //O2
                        temp_byte[0] = Read_Buffer[10];
                        temp_byte[1] = Read_Buffer[9];
                        Fla502_data.NO = BitConverter.ToInt16(temp_byte, 0);          //NO
                        if (Fla502_data.NO <= 0) Fla502_data.NO = 1f;
                        temp_byte[0] = Read_Buffer[12];
                        temp_byte[1] = Read_Buffer[11];
                        Fla502_data.ZS = BitConverter.ToInt16(temp_byte, 0);         //转速
                        temp_byte[0] = Read_Buffer[14];
                        temp_byte[1] = Read_Buffer[13];
                        Fla502_data.YW = BitConverter.ToInt16(temp_byte, 0) / 10f;         //油温
                        temp_byte[0] = Read_Buffer[16];
                        temp_byte[1] = Read_Buffer[15];
                            Fla502_data.λ = BitConverter.ToInt16(temp_byte, 0) / 100f;
                        //if ((Fla502_data.CO + Fla502_data.CO2) == 0)
                        //    Fla502_data.λ = 0;
                        //else
                        //    Fla502_data.λ = (float)((Fla502_data.CO2 + Fla502_data.CO / 2f + Fla502_data.O2 + ((1.7261 / 4f * 3.5 / (3.5 + Fla502_data.CO / Fla502_data.CO2) - 0.0176 / 2f) * (Fla502_data.CO2 + Fla502_data.CO))) / ((1 + 1.7261 / 4f - 0.0176 / 2) * (Fla502_data.CO2 + Fla502_data.CO + 0.0006 * Fla502_data.HC)));
                        //if (Fla502_data.λ < 0 || Fla502_data.λ > 100f)
                        //    Fla502_data.λ = 0;

                    }
                    return Fla502_data;
                    break;
                case "mqw_50a":
                    byte[] Content_MQ = new byte[] { DID, cmdGetDat_MQ, 0X03, 0 };
                    Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                    ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                    Thread.Sleep(500);
                    while (ComPort_1.BytesToRead < 28)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return Fla502_data;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x6 && Read_Buffer[1] == 0x60 && Read_Buffer[2] == 0x1B)
                    {
                        byte[] temp_byte = new byte[2];
                        temp_byte[0] = Read_Buffer[4];
                        temp_byte[1] = Read_Buffer[3];
                        Fla502_data.CO2 = BitConverter.ToInt16(temp_byte, 0) / 100f;         //二氧化碳
                        temp_byte[0] = Read_Buffer[6];
                        temp_byte[1] = Read_Buffer[5];
                        Fla502_data.CO = (float)(Math.Round(coxs * (BitConverter.ToInt16(temp_byte, 0) / 100f), 2));       //一氧化碳
                        if (Fla502_data.CO <= 0) Fla502_data.CO = 0.01f;
                        temp_byte[0] = Read_Buffer[8];
                        temp_byte[1] = Read_Buffer[7];
                        Fla502_data.HC = (float)(Math.Round(hcxs * (BitConverter.ToInt16(temp_byte, 0)), 0));               //碳氢
                        if (Fla502_data.HC <= 0) Fla502_data.HC = 1f;
                        temp_byte[0] = Read_Buffer[10];
                        temp_byte[1] = Read_Buffer[9];
                        Fla502_data.NO = (float)(Math.Round(noxs * (BitConverter.ToInt16(temp_byte, 0)), 0));               //一氧化氮
                        if (Fla502_data.NO <= 0) Fla502_data.NO = 1f;
                        temp_byte[0] = Read_Buffer[12];
                        temp_byte[1] = Read_Buffer[11];
                        Fla502_data.O2 = BitConverter.ToInt16(temp_byte, 0) / 100f;          //氧气
                        temp_byte[0] = Read_Buffer[14];
                        temp_byte[1] = Read_Buffer[13];
                        Fla502_data.SD = BitConverter.ToInt16(temp_byte, 0) / 10f;         //湿度
                        temp_byte[0] = Read_Buffer[16];
                        temp_byte[1] = Read_Buffer[15];
                        Fla502_data.YW = BitConverter.ToInt16(temp_byte, 0) / 10f;         //油温
                        temp_byte[0] = Read_Buffer[18];
                        temp_byte[1] = Read_Buffer[17];
                        Fla502_data.HJWD = BitConverter.ToInt16(temp_byte, 0) / 10f;       //环境温度
                        temp_byte[0] = Read_Buffer[20];
                        temp_byte[1] = Read_Buffer[19];
                        Fla502_data.ZS = BitConverter.ToInt16(temp_byte, 0);               //转速
                        temp_byte[0] = Read_Buffer[22];
                        temp_byte[1] = Read_Buffer[21];
                        Fla502_data.QLYL = BitConverter.ToInt16(temp_byte, 0) / 10f;         //油路压力 
                        temp_byte[0] = Read_Buffer[24];
                        temp_byte[1] = Read_Buffer[23];
                        Fla502_data.λ = BitConverter.ToInt16(temp_byte, 0) / 1000f;       //燃空比λ
                        temp_byte[0] = Read_Buffer[26];
                        temp_byte[1] = Read_Buffer[25];
                        Fla502_data.HJYL = BitConverter.ToInt16(temp_byte, 0) / 10f;       //环境压力 kpa
                    }
                    return Fla502_data;
                    break;
                case "fasm_5000":
                    ReadData();
                    //Struct_Now = Get_Struct();
                    //Zeroing();
                    i = 0;
                    byte[] Content_FF = new byte[] { cmdEquipAddress_FF, cmdGetData_FF, 0X00, 0x00, 0x00 };
                    Content_FF[4] = getCS_MQ(Content_FF, 4);
                    ComPort_1.Write(cmdFrameHead_FF, 0, 2);        //发送开始测量命令
                    ComPort_1.Write(Content_FF, 0, 5);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 32)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return Fla502_data;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x02)
                    {
                        byte[] temp_byte = new byte[2];
                        temp_byte[0] = Read_Buffer[7];
                        temp_byte[1] = Read_Buffer[8];
                        Fla502_data.HC = BitConverter.ToInt16(temp_byte, 0);         //二氧化碳
                        if (Fla502_data.HC <= 0) Fla502_data.HC = 1f;
                        temp_byte[0] = Read_Buffer[9];
                        temp_byte[1] = Read_Buffer[10];
                        Fla502_data.CO = BitConverter.ToInt16(temp_byte, 0) / 100f; ;       //一氧化碳
                        if (Fla502_data.CO <= 0) Fla502_data.CO = 0.01f;
                        temp_byte[0] = Read_Buffer[11];
                        temp_byte[1] = Read_Buffer[12];
                        Fla502_data.CO2 = BitConverter.ToInt16(temp_byte, 0) / 100f;               //碳氢
                        temp_byte[0] = Read_Buffer[13];
                        temp_byte[1] = Read_Buffer[14];
                        Fla502_data.O2 = BitConverter.ToInt16(temp_byte, 0) / 100f;               //一氧化氮
                        temp_byte[0] = Read_Buffer[15];
                        temp_byte[1] = Read_Buffer[16];
                        Fla502_data.NO = BitConverter.ToInt16(temp_byte, 0);          //氧气
                        if (Fla502_data.NO <= 0) Fla502_data.NO = 1f;
                        temp_byte[0] = Read_Buffer[17];
                        temp_byte[1] = Read_Buffer[18];
                        Fla502_data.λ = BitConverter.ToInt16(temp_byte, 0) / 100f;         //湿度
                        temp_byte[0] = Read_Buffer[19];
                        temp_byte[1] = Read_Buffer[20];
                        Fla502_data.ZS = BitConverter.ToInt16(temp_byte, 0);         //油温
                        temp_byte[0] = Read_Buffer[21];
                        temp_byte[1] = Read_Buffer[22];
                        Fla502_data.YW = BitConverter.ToInt16(temp_byte, 0) / 10f;       //环境温度
                        temp_byte[0] = Read_Buffer[23];
                        temp_byte[1] = Read_Buffer[24];
                        Fla502_data.Sjll = BitConverter.ToInt16(temp_byte, 0);               //转速
                        temp_byte[0] = Read_Buffer[25];
                        temp_byte[1] = Read_Buffer[26];
                        Fla502_data.HJYL = BitConverter.ToInt16(temp_byte, 0) / 10f;         //油路压力 
                        temp_byte[0] = Read_Buffer[27];
                        temp_byte[1] = Read_Buffer[28];
                        Fla502_data.HJWD = BitConverter.ToInt16(temp_byte, 0) / 10f;       //燃空比λ
                        temp_byte[0] = Read_Buffer[29];
                        temp_byte[1] = Read_Buffer[30];
                        Fla502_data.SD = BitConverter.ToInt16(temp_byte, 0) / 10f;       //环境压力 kpa
                    }
                    return Fla502_data;
                    break;

                case "mqw_511":
                    ReadData();
                    i = 0;
                    byte[] Content_MQ511 = new byte[] { 0x03 };
                    ComPort_1.Write(Content_MQ511, 0, 1);        //发送开始测量命令
                    Thread.Sleep(200);
                    while (ComPort_1.BytesToRead < 19)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return Fla502_data;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        byte[] temp_byte = new byte[2];
                        temp_byte[0] = Read_Buffer[2];
                        temp_byte[1] = Read_Buffer[1];
                        Fla502_data.HC = BitConverter.ToInt16(temp_byte, 0);         //二氧化碳
                        temp_byte[0] = Read_Buffer[4];
                        temp_byte[1] = Read_Buffer[3];
                        Fla502_data.CO = BitConverter.ToInt16(temp_byte, 0) / 100f; ;       //一氧化碳
                        temp_byte[0] = Read_Buffer[6];
                        temp_byte[1] = Read_Buffer[5];
                        Fla502_data.CO2 = BitConverter.ToInt16(temp_byte, 0) / 100f;               //碳氢
                        temp_byte[0] = Read_Buffer[8];
                        temp_byte[1] = Read_Buffer[7];
                        Fla502_data.O2 = BitConverter.ToInt16(temp_byte, 0) / 100f;               //一氧化氮
                        temp_byte[0] = Read_Buffer[10];
                        temp_byte[1] = Read_Buffer[9];
                        Fla502_data.NO = BitConverter.ToInt16(temp_byte, 0);          //氧气
                        temp_byte[0] = Read_Buffer[12];
                        temp_byte[1] = Read_Buffer[11];
                        Fla502_data.ZS = BitConverter.ToInt16(temp_byte, 0);         //湿度
                        temp_byte[0] = Read_Buffer[14];
                        temp_byte[1] = Read_Buffer[13];
                        Fla502_data.YW = BitConverter.ToInt16(temp_byte, 0);         //油温
                        temp_byte[0] = Read_Buffer[16];
                        temp_byte[1] = Read_Buffer[15];
                        Fla502_data.λ = BitConverter.ToInt16(temp_byte, 0) / 100f;       //环境温度                       
                    }
                    return Fla502_data;
                    break;
                default:
                    return Fla502_data;
                    break;
            }

        }
        #endregion


        public float Getdata_PEF()
        {
            ReadData();
            byte[] temp_byte = new byte[2];
            int i = 0;
            switch (yqxh)
            {
                case "fla_502":
                    byte[] Content = new byte[] { cmdGetPEF };
                    ComPort_1.Write(Content, 0, 1);//发送调零命令
                    Thread.Sleep(10);
                    i = 0;
                    while (ComPort_1.BytesToRead < 2)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return 0.0f;
                    }
                    ReadData();                         //读取数据

                    temp_byte[0] = Read_Buffer[1];
                    temp_byte[1] = Read_Buffer[0];
                    return (BitConverter.ToInt16(temp_byte, 0) / 1000f);         //HC
                    break;
                case cdxh:
                    byte[] ContentCD = new byte[] { CDcmdGetPEF };
                    ComPort_1.Write(ContentCD, 0, 1);//发送调零命令
                    Thread.Sleep(10);
                    i = 0;
                    while (ComPort_1.BytesToRead < 2)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return 0.0f;
                    }
                    ReadData();                         //读取数据

                    temp_byte[0] = Read_Buffer[1];
                    temp_byte[1] = Read_Buffer[0];
                    return (BitConverter.ToInt16(temp_byte, 0) / 1000f);         //HC
                    break;

                case "nha_503":
                    byte[] naContent = new byte[] { cmdGetPEF_nh503 };
                    ComPort_1.Write(naContent, 0, 1);//发送调零命令
                    Thread.Sleep(10);
                    i = 0;
                    while (ComPort_1.BytesToRead < 2)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return 0.0f;
                    }
                    ReadData();                         //读取数据
                    //byte[] temp_byte = new byte[2];
                    temp_byte[0] = Read_Buffer[1];
                    temp_byte[1] = Read_Buffer[0];
                    return (BitConverter.ToInt16(temp_byte, 0) / 1000f);         //HC
                    break;
                case "fasm_5000":
                    ReadData();
                    //Struct_Now = Get_Struct();
                    //Zeroing();
                    i = 0;
                    byte[] Content_FF = new byte[] { cmdEquipAddress_FF, cmdGetPEF_FF, 0X00, 0x00, 0x00 };
                    Content_FF[4] = getCS_MQ(Content_FF, 4);
                    ComPort_1.Write(cmdFrameHead_FF, 0, 2);        //发送开始测量命令
                    ComPort_1.Write(Content_FF, 0, 5);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 32)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return 0.530f;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0xa5 && Read_Buffer[1] == 0xa5 && Read_Buffer[2] == 0x01 && Read_Buffer[3] == 0x04)
                    {
                        //byte[] temp_byte = new byte[4];
                        temp_byte[0] = Read_Buffer[7];
                        temp_byte[1] = Read_Buffer[8];
                        temp_byte[2] = Read_Buffer[9];
                        temp_byte[3] = Read_Buffer[10];
                        return (BitConverter.ToSingle(temp_byte, 0));         //HC
                    }
                    return 0.530f;
                    break;
                default: return 0f; break;
            }

        }
        #region 锁键盘
        /// <summary>
        /// 锁键盘。
        /// </summary>
        /// <returns>bool</returns>
        public bool lockKeyboard()
        {
            ReadData();
            int i = 0;
            byte[] temp_byte = new byte[2];
            switch (yqxh)
            {
                case "fla_502":
                    ReadData();
                    i = 0;
                    byte[] Content = new byte[] { cmdLockKeyboard };
                    ComPort_1.Write(Content, 0, 1);

                    //SendData(Cmd_Blowback, Content);        //发送反吹命令
                    Thread.Sleep(10);
                    i = 0;
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                         //读取数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "mqw_50a":
                    byte[] Content_MQ = new byte[] { DID, cmdLockKeyboard_MQ, 0X03, 0 };
                    Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                    ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x6A && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x8D)
                        return true;
                    else
                        return false;
                    break;

                default: return false; break;
            }
        }
        #endregion
        #region 解锁键盘
        /// <summary>
        /// 锁键盘。
        /// </summary>
        /// <returns>bool</returns>
        public bool unlockKeyboard()
        {
            ReadData();
            int i = 0;
            byte[] temp_byte = new byte[2];
            switch (yqxh)
            {
                case "fla_502":
                    ReadData();
                    i = 0;
                    byte[] Content = new byte[] { cmdUnlockKeyboard };
                    ComPort_1.Write(Content, 0, 1);

                    //SendData(Cmd_Blowback, Content);        //发送反吹命令
                    Thread.Sleep(10);
                    i = 0;
                    while (ComPort_1.BytesToRead < 1)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                         //读取数据
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case "mqw_50a":
                    byte[] Content_MQ = new byte[] { DID, cmdUnlockKeyboard_MQ, 0X03, 0 };
                    Content_MQ[3] = getCS_MQ(Content_MQ, 3);
                    ComPort_1.Write(Content_MQ, 0, 4);        //发送开始测量命令
                    Thread.Sleep(10);
                    while (ComPort_1.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06 && Read_Buffer[1] == 0x6B && Read_Buffer[2] == 0x03 && Read_Buffer[3] == 0x90)
                        return true;
                    else
                        return false;
                    break;

                default: return false; break;
            }
        }
        #endregion

        #region 驰达柴油部分
        #region 进入实时测量状态
        /// <summary>
        /// 进入直接测量状态
        /// </summary>
        /// <returns>bool</returns>
        public bool Set_Measure()
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {
                
                case cdxh:
                    byte[] Content_MQ = new byte[] { CDCYcmdSetTest, 0x01, 0 };
                    Content_MQ[2] = getCS_MQ(Content_MQ, 2);
                    ComPort_1.Write(Content_MQ, 0, 3);
                    Thread.Sleep(50);
                    while (ComPort_1.BytesToRead < 2)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return false;
                    else if (Read_Buffer[0] == CDCYcmdSetTest)
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
        #region 线性校正
        public bool set_linearDem()
        {
            //string Measurement = Get_Mode();
            //get_AccData = 0;
            ReadData();
            //ComPort_3.ReceivedBytesThreshold = 4;
            int i = 0;
            //byte CS = 0;
            //byte[] Content = new byte[] {0x01};
            switch (yqxh)
            {
                case cdxh:
                    byte[] Content_MQ = new byte[] { CDCYcmdDemarcate, 0 };
                    Content_MQ[1] = getCS_MQ(Content_MQ, 1);
                    ComPort_1.Write(Content_MQ, 0, 2);
                    Thread.Sleep(50);
                    while (ComPort_1.BytesToRead < 2)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return false;
                    else if (Read_Buffer[0] == CDCYcmdDemarcate)
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
        
        public Flb_100_smoke get_DirectData()
        {
            Flb_100_smoke smoke = new Flb_100_smoke();
            smoke.K = 0;
            smoke.Ns = 0;
            smoke.Yw = 0;
            smoke.Zs = 0;
            smoke.Qw = 0;
            int i = 0;
            byte CS = 0;
            ReadData();
            switch (yqxh)
            {
                case cdxh:
                    byte[] Content_MQ = new byte[] { CDCYcmdGetDirectData, 0 };
                    Content_MQ[1] = getCS_MQ(Content_MQ, 1);
                    ComPort_1.Write(Content_MQ, 0, 2);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 10)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(5);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return smoke;
                    else
                    {
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8]);
                        if (Read_Buffer[9] == (byte)((~CS) + 1))
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[2];
                            temp_byte[1] = Read_Buffer[1];
                            smoke.Ns = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            smoke.K = BitConverter.ToInt16(temp_byte, 0) / 100f;
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            smoke.Zs = BitConverter.ToInt16(temp_byte, 0);
                            temp_byte[0] = Read_Buffer[8];
                            temp_byte[1] = Read_Buffer[7];
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0);
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                default:
                    return smoke;
                    break;
            }
        }
        public Flb_100_smoke get_DirectData(float minKvalue)
        {
            Flb_100_smoke smoke = new Flb_100_smoke();
            smoke.K = 0;
            smoke.Ns = 0;
            smoke.Yw = 0;
            smoke.Zs = 0;
            smoke.Qw = 0;
            int i = 0;
            byte CS = 0;
            ReadData();
            switch (yqxh)
            {
                case cdxh:
                    byte[] Content_MQ = new byte[] { CDCYcmdGetDirectData, 0 };
                    Content_MQ[1] = getCS_MQ(Content_MQ, 1);
                    ComPort_1.Write(Content_MQ, 0, 2);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 10)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(5);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return smoke;
                    else
                    {
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8]);
                        if (Read_Buffer[9] == (byte)((~CS) + 1))
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[2];
                            temp_byte[1] = Read_Buffer[1];
                            smoke.Ns = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            smoke.K = BitConverter.ToInt16(temp_byte, 0) / 100f;
                            if (smoke.K <= minKvalue) smoke.K = minKvalue;
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            smoke.Zs = BitConverter.ToInt16(temp_byte, 0);
                            temp_byte[0] = Read_Buffer[8];
                            temp_byte[1] = Read_Buffer[7];
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0);
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                default:
                    return smoke;
                    break;
            }
        }
        #region 取稳态值
        public Flb_100_smoke get_StableData()
        {
            //string Measurement = Get_Mode();
            Flb_100_smoke smoke = new Flb_100_smoke();
            smoke.K = 0.01f;
            smoke.Ns = 0;
            smoke.Yw = 0;
            smoke.Zs = 0;
            smoke.Qw = 0;
            int i = 0;
            byte CS = 0;
            ReadData();
            switch (yqxh)
            {
                case cdxh:
                    byte[] Content_MQ = new byte[] { CDCYcmdGetMaxData, 0 };
                    Content_MQ[1] = getCS_MQ(Content_MQ, 1);
                    ComPort_1.Write(Content_MQ, 0, 2);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 10)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(5);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return smoke;
                    else
                    {
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8]);
                        if (Read_Buffer[9] == (byte)((~CS) + 1))
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[2];
                            temp_byte[1] = Read_Buffer[1];
                            smoke.Ns = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            smoke.K = BitConverter.ToInt16(temp_byte, 0) / 100f;
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            smoke.Zs = BitConverter.ToInt16(temp_byte, 0);
                            temp_byte[0] = Read_Buffer[8];
                            temp_byte[1] = Read_Buffer[7];
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0);
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                default:
                    return smoke;
                    break;
            }
        }
        public Flb_100_smoke get_StableData(float minKvalue)
        {
            //string Measurement = Get_Mode();
            Flb_100_smoke smoke = new Flb_100_smoke();
            smoke.K = 0.01f;
            smoke.Ns = 0;
            smoke.Yw = 0;
            smoke.Zs = 0;
            smoke.Qw = 0;
            int i = 0;
            byte CS = 0;
            ReadData();
            switch (yqxh)
            {
                case cdxh:
                    byte[] Content_MQ = new byte[] { CDCYcmdGetMaxData, 0 };
                    Content_MQ[1] = getCS_MQ(Content_MQ, 1);
                    ComPort_1.Write(Content_MQ, 0, 2);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 10)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(5);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return smoke;
                    else
                    {
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8]);
                        if (Read_Buffer[9] == (byte)((~CS) + 1))
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[2];
                            temp_byte[1] = Read_Buffer[1];
                            smoke.Ns = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            smoke.K = BitConverter.ToInt16(temp_byte, 0) / 100f;
                            if (smoke.K <= minKvalue) smoke.K = minKvalue;
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            smoke.Zs = BitConverter.ToInt16(temp_byte, 0);
                            temp_byte[0] = Read_Buffer[8];
                            temp_byte[1] = Read_Buffer[7];
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0);
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                default:
                    return smoke;
                    break;
            }
        }
        #endregion
        #region 鸣泉清除最大值
        public bool clear_maxData()
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {
                case cdxh:
                    byte[] Content_MQ = new byte[] { CDCYcmdClearMaxData, 0 };
                    Content_MQ[1] = getCS_MQ(Content_MQ, 1);
                    ComPort_1.Write(Content_MQ, 0, 2);
                    Thread.Sleep(50);
                    while (ComPort_1.BytesToRead < 2)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return false;
                    else if (Read_Buffer[0] == CDCYcmdClearMaxData)
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

        #region 进入稳态测量状态
        /// <summary>
        /// 进入稳态测量状态
        /// </summary>
        /// <returns>bool</returns>
        public bool Set_StableMeasure()
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {
                case cdxh:
                        return true;
                default:
                    return false;
                    break;
            }

        }
        #endregion
        #region 开始稳态测量
        /// <summary>
        /// 开始稳态测量
        /// </summary>
        /// <returns>bool</returns>
        public bool Start_StableMeasure()
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {

                case cdxh:
                    byte[] Content_MQ = new byte[] { CDCYcmdSetTest, 0x01, 0 };
                    Content_MQ[2] = getCS_MQ(Content_MQ, 2);
                    ComPort_1.Write(Content_MQ, 0, 3);
                    Thread.Sleep(50);
                    while (ComPort_1.BytesToRead < 2)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return false;
                    else if (Read_Buffer[0] == CDCYcmdSetTest)
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
        #region 停止稳态测量
        /// <summary>
        /// 停止稳态测量
        /// </summary>
        /// <returns>bool</returns>
        public bool Stop_StableMeasure()
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {
                case cdxh:
                    return true;
                default:
                    return false;
                    break;
            }
        }
        #endregion
        #endregion
    }
}
