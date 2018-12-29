using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace Exhaust
{
    public class Flb_100_smoke
    {
        public Flb_100_smoke()
        {
            wd = 0;
            sd = 0;
            dqy = 0;
            Qw = 0;
            Ns = 0;
            N = 0;
            Zs = 0;
            Yw = 0;
            K = 0;
            no = 0;
            glyl = 0;
            qswd = 0;
            yqwd = 0;
        }
        private bool _success;

        public bool Success
        {
            get { return _success; }
            set { _success = value; }
        }
        private float _k;
        private float _ns;
        private float _n;
        private float _zs;
        private float _yw;
        private float _qw;

        /// <summary>
        /// 气温
        /// </summary>
        public float Qw
        {
          get { return _qw; }
          set { _qw = value; }
        }
        

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
        public float K
        {
            get { return _k; }
            set { _k = value; }
        }

        /// <summary>
        /// 不透光N值
        /// </summary>
        public float Ns
        {
            get { return _ns; }
            set { _ns = value; }
        }
        /// <summary>
        /// 不透光N值
        /// </summary>
        public float N
        {
            get { return _n; }
            set { _n = value; }
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

        public float No
        {
            get
            {
                return no;
            }

            set
            {
                no = value;
            }
        }

        public float Glyl
        {
            get
            {
                return glyl;
            }

            set
            {
                glyl = value;
            }
        }

        public float Qswd
        {
            get
            {
                return qswd;
            }

            set
            {
                qswd = value;
            }
        }

        public float Yqwd
        {
            get
            {
                return yqwd;
            }

            set
            {
                yqwd = value;
            }
        }

        private float no;
        private float glyl;
        private float qswd;
        private float yqwd;
    }
    public class mqw_5101_status
    {
        public bool isVehicleGasPumping { set; get; }
        public bool isAirGasPumping { set; get; }
        public bool isFlowBacking { set; get; }
        public bool isDemarcateGasPumping { set; get; }
        public bool isTestGasPumping { set; get; }
        public bool isZeroGasPumping { set; get; }
        public bool isNOZeroing { set; get; }
        public bool isNODemarcating { set; get; }
        public bool isTemperatureDemarcating { set; get; }
        public bool isHumidityDemarcating { set; get; }
        public bool isAirPressureDemarcating { set; get; }
        public bool isOilTempDemarcating { set; get; }
        public bool isGuanLuPressureDemarcating { set; get; }
        public bool isTesting { set; get; }
        public bool isLeakTesting { set; get; }
        public bool isAutoFlowBacking { set; get; }
        public bool isBackGroundAirTesting { set; get; }
        public bool isEnvirAirTesting { set; get; }
        public bool isPrepare { set; get; }
        public bool isBusy { set; get; }
        public bool isLowFlowAlarm { set; get; }
        public bool isLeakTestSuccess { set; get; }
    }
    public class FLB_100
    {
        public bool isNhSelfUse = false;
        private string yqxh = "flb_100";
        const string yq_mqw5101 = "mqw_5101";
        public double kxs = 1;
        public FLB_100(string xh)
        {
            yqxh = xh.ToLower();
        }
        
        public FLB_100()
        { }

        public System.IO.Ports.SerialPort ComPort_3;
        byte[] Send_Buffer;                                     //发送缓冲区
        byte[] Read_Buffer;                                     //读取缓冲区
        bool Read_Flag = false;                                 //是否有数据可以读取

       

        byte ydj_id = 0x02;
        byte[] get_Status = new byte[4]{0x02,0x04,0x01,0xf9};
        byte[] get_MeasureDat =new byte[4]{0x02,0x04,0x02,0xf8} ;
        byte[] get_StableDat = new byte[4]{0x02,0x04,0x03,0xf7};
        byte[] get_AccelerateDat = new byte[4]{0x02,0x04,0x04,0xf6};
        byte[] apply_LinearJiaozheng = new byte[4]{0x02,0x04,0x05,0xf5};
        byte[] set_TestDirectly = new byte[4]{0x02,0x04,0x06,0xf4};
        byte[] set_TestStable =new byte[4]{0x02,0x04,0x07,0xf3} ;
        byte[] set_TestAccelerate = new byte[4]{0x02,0x04,0x08,0xf2};
        byte[] stop_Stabletest =new byte[4]{0x02,0x04,0x09,0xf1} ;
        byte[] start_Stabletest = new byte[4]{0x02,0x04,0x0a,0xf0};
        byte[] apply_AccelerateTrigger = new byte[4]{0x02,0x04,0x0b,0xef};
        byte[] apply_LatestAccelerateDat =new byte[4]{0x02,0x04,0x0e,0xec} ;
        byte[] apply_LatestAccelerateQuDat =new byte[4]{0x02,0x04,0x0f,0xeb} ;

        byte cmdSetTestMethod_MQ = 0xa0;
        byte cmdGetTestMethod_MQ = 0xa1;
        byte cmdDemarcate_MQ = 0xa2;
        byte cmdStartAccelerate_MQ = 0xa3;
        byte cmdStopAccelerate_MQ = 0xa4;
        byte cmdGetAccelerateStatus_MQ = 0xa5;
        byte cmdGetDirectData_MQ = 0xa6;
        byte cmdGetAccelerateData_MQ = 0xa7;
        byte cmdClearMaxData_MQ = 0xa8;
        byte cmdGetMaxData_MQ = 0xa9;
        byte cmdDetectorIsOk_MQ = 0xaa;
        byte cmdDemarcateTemp_MQ = 0xac;
        byte cmdDemarcateHumi_MQ = 0xad;
        byte cmdDemarcateAirP_MQ = 0xab;
        byte cmdGetData_MQ = 0xae;

        #region 南华NHT-1命令字
        byte cmdSetTestMethod_NH = 0xa0;
        byte cmdGetTestMethod_NH = 0xa1;
        byte cmdDemarcate_NH = 0xa4;
        byte cmdStartAccelerate_NH = 0xad;
        byte cmdStopAccelerate_NH = 0xb0;
        byte cmdGetAccelerateStatus_NH = 0xae;
        byte cmdGetDirectData_NH = 0xa5;
        byte cmdGetAccelerateData_NH = 0xb1;
        byte cmdClearMaxData_NH = 0xa7;
        byte cmdGetMaxData_NH = 0xa6;
        byte cmdDetectorIsOk_NH = 0xaf;
        #endregion
        #region 佛分FTY-100命令字
        byte cmdGetStatus_FF = 0x01;
        byte cmdGetData_FF = 0x02;
        byte cmdGetStableData_FF = 0x03;
        byte cmdGetAccelerateData_FF = 0x04;
        byte cmdLinearDemarcate_FF = 0x05;
        byte cmdTestScreen_FF = 0x06;
        byte cmdStableScreen_FF = 0x07;
        byte cmdAccelerateScreen_FF = 0x08;
        byte cmdStartOrStopStableTest_FF = 0x09;
        byte cmdTriggerOneAccelerate_FF = 0x0b;
        byte cmdClearAccelerateData_FF = 0x0c;
        byte cmdChangeAccelerateStyle_FF = 0x0d;
        byte cmdGetLatestAccelerateData = 0x0e;
        byte cmdGetYW_FF = 0x10;
        byte cmdGetK_FF = 0x15;
        #endregion

        #region mqw-5101
        byte[] cmdPumpVehicleGas_MQW = { 0x05, 0x10, 0x03, 0xe8 };
        byte[] cmdPumpAir_MQW = { 0x05, 0x11, 0x03, 0xe7 };
        byte[] cmdManualFlowBack_MQW = { 0x05, 0x12, 0x03, 0xe6 };
        byte[] cmdPumpDemarcateGas_MQW = { 0x05, 0x13, 0x03, 0xe5 };
        byte[] cmdPumpTestGas_MQW = { 0x05, 0x14, 0x03, 0xe4 };
        byte[] cmdPumpZeroGas_MQW = { 0x05, 0x15, 0x03, 0xe3 };
        byte[] cmdStopAction_MQW = { 0x05, 0x16, 0x03, 0xe2 };
        byte[] cmdAutoFlowBack_MQW = { 0x05, 0x17, 0x03, 0xe1 };
        byte[] cmdTestBackAir_MQW = { 0x05, 0x18, 0x03, 0xe0 };
        byte[] cmdTestEnvirAir_MQW = { 0x05, 0x19, 0x03, 0xdf };
        byte[] cmdLeakTest_MQW = { 0x05, 0x1a, 0x03, 0xde };
        byte[] cmdQuitPrepare_MQW = { 0x05, 0x1f, 0x03, 0xd9 };
        byte[] cmdZero_MQW = { 0x05, 0x21, 0x03, 0xd7 };
        byte[] cmdDemarcateNoSingle_MQW = { 0x05, 0x22, 0x05, 0x00,0x00,0x00 };//后三位分别 为浓度高字节，低字节，校验字节
        byte[] cmdDemarcateTemp_MQW = { 0x05, 0x24, 0x05, 0x00, 0x00, 0x00 };//后三位分别 为浓度高字节，低字节，校验字节
        byte[] cmdDemarcateHumidity_MQW = { 0x05, 0x25, 0x05, 0x00, 0x00, 0x00 };//后三位分别 为浓度高字节，低字节，
        byte[] cmdDemarcatePressure_MQW = { 0x05, 0x26, 0x05, 0x00, 0x00, 0x00 };//后三位分别 为浓度高字节，低字节，校验字节
        byte[] cmdDemarcateOilTemp_MQW = { 0x05, 0x27, 0x05, 0x00, 0x00, 0x00 };//后三位分别 为浓度高字节，低字节，校验字节
        byte[] cmdDemarcateGuanluPressure_MQW = { 0x05, 0x28, 0x05, 0x00, 0x00, 0x00 };//后三位分别 为浓度高字节，低字节，校验字节
        byte[] cmdGetRealData_MQW = { 0x05, 0x30, 0x03, 0xc8 };
        byte[] cmdGetRealStatus_MQW = { 0x05, 0x31, 0x03, 0xc7 };
        byte[] cmdGetRealDataAndStatus_MQW = { 0x05, 0x32, 0x03, 0xc6 };
        byte[] cmdGetBackGroundAirData_MQW = { 0x05, 0x33, 0x03, 0xc5 };
        byte[] cmdGetEnviAirData_MQW = { 0x05, 0x34, 0x03, 0xc4 };
        byte[] cmdSetZeroGas_MQW = { 0x05, 0x40, 0x04, 0x00,0x00 };//设置调零气体
        byte[] cmdSetZeroStyle_MQW = { 0x05, 0x41, 0x04, 0x00, 0x00 };//设置调零方式
        byte[] cmdLockKeyBoard_MQW = { 0x05, 0x54, 0x03, 0xa4 };//设置调零气体
        byte[] cmdUnLockKeyBoard_MQW = { 0x05, 0x55, 0x03, 0xa3 };//设置调零气体
        byte[] cmdResetFactorySettings_MQW = { 0x05, 0x58, 0x03, 0xa0 };//设置调零气体

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

            ComPort_3.Write(Content, 0, Content.Length);
            ini.INIIO.saveLogInf("[烟度计发送]:" + byteToHexStr(Content));
        }
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
            int count = ComPort_3.Read(Read_Buffer, 0, ComPort_3.BytesToRead);
            if (count > 0)
            {
                ini.INIIO.saveLogInf("[烟度计接收]:" + byteToHexStr(Read_Buffer.ToList(), 0, count));
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
        #region FTY-100的校验方式
        private byte getCS_MQ(byte[] content, int startindex, int count)
        {
            byte CS = 0;
            for (int i = startindex; i < count; i++)
                CS += content[i];
            CS = BitConverter.GetBytes(~(Convert.ToInt16(CS)) + 1)[0];
            return CS;
        }
        #endregion
        #region 鸣泉清除最大值
        public bool clear_maxData()
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {
                case "flb_100":
                    return true;
                    break;
                case "mqy_200":
                    byte[] Content_MQ = new byte[] { cmdClearMaxData_MQ, 0 };
                    Content_MQ[1] = getCS_MQ(Content_MQ, 1);
                    SendData(Content_MQ);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 2)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return false;
                    else if (Read_Buffer[0] == cmdClearMaxData_MQ)
                        return true;
                    else
                        return false;
                    break;
                case "nht_1":
                    byte[] Content_NH = new byte[] { cmdClearMaxData_NH, 0 };
                    Content_NH[1] = getCS_MQ(Content_NH, 1);
                    SendData(Content_NH);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 2)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15 && !isNhSelfUse)
                        return false;
                    else if (Read_Buffer[0] == cmdClearMaxData_NH)
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
        #region 取仪器测量方式
        /// <summary>
        /// 取仪器测量方式
        /// </summary>
        /// <returns>string （正在预热、正在实时测量、正在自由加速试验、其他）</returns>
        public string Get_Mode()
        {
            byte CS = 0;
            int i = 0;
            ReadData();
            switch (yqxh)
            {
                case "flb_100":
                    if (!ComPort_3.IsOpen)
                        return "通讯故障";
                    ComPort_3.DiscardInBuffer();
                    SendData(get_Status);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 7)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return "通讯故障";
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return "通讯故障";
                    else
                    {
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5]);
                        if (Read_Buffer[6] == (byte)((~CS) + 1))
                        {
                            if (Read_Buffer[3] != 0x00)
                            {
                                if ((Read_Buffer[3] & 0x01) == 0x01)
                                {
                                    return "仪器处于稳态测量状态";
                                }
                                else if ((Read_Buffer[3] & 0x02) == 0x02)
                                {
                                    return "仪器处于预热状态";
                                }
                                else if ((Read_Buffer[3] & 0x04) == 0x04)
                                {
                                    return "仪器处于直接测量状态";
                                }
                                else if ((Read_Buffer[3] & 0x08) == 0x08)
                                {
                                    return "仪器处于加速测量状态";
                                }
                                else if ((Read_Buffer[3] & 0x10) == 0x10)
                                {
                                    return "仪器处于菜单屏";
                                }
                                else if ((Read_Buffer[3] & 0x20) == 0x20)
                                {
                                    return "加速数据符合标准要求";
                                }
                                else if ((Read_Buffer[3] & 0x40) == 0x40)
                                {
                                    return "仪器处于稳态测量的开始状态";
                                }
                                else if ((Read_Buffer[3] & 0x80) == 0x80)
                                {
                                    return "仪器处于加速测量自动触发状态";
                                }
                                else
                                {
                                    return "通讯故障";
                                }

                            }
                            else if ((Read_Buffer[4] & 0x08) == 0x08)
                                return "加速测量进行中";
                            else
                                return "仪器处于待机状态";
                        }
                        else
                            return "通讯故障";

                    }
                    break;
                case "mqy_200":
                    if (!ComPort_3.IsOpen)
                        return "通讯故障";
                    ComPort_3.DiscardInBuffer();
                    byte[] Content_MQ = new byte[] {cmdGetTestMethod_MQ,0};
                    Content_MQ[1] = getCS_MQ(Content_MQ, 1);
                    SendData(Content_MQ);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead <3)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return "通讯故障";
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return "通讯故障";
                    else
                    {
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1]);
                        if (Read_Buffer[2] == (byte)((~CS) + 1))
                        {
                            switch (Read_Buffer[2])
                            {
                                case 0x00:
                                    return "仪器处于预热状态";
                                    break;
                                case 0x01:
                                    return "仪器处于直接测量状态";
                                    break;
                                case 0x02:
                                    return "仪器处于加速测量状态";
                                    break;
                                case 0xff:
                                    return "仪器处于其他操作方式";
                                    break;
                                default:
                                    return "仪器处于未知状态";
                                    break;

                            }
                        }
                        else
                            return "通讯故障";

                    }
                    break;
                case "nht_1":
                    if (!ComPort_3.IsOpen)
                        return "通讯故障";
                    ComPort_3.DiscardInBuffer();
                    byte[] Content_NH = new byte[] { cmdGetTestMethod_NH, 0 };
                    Content_NH[1] = getCS_MQ(Content_NH, 1);
                    SendData(Content_NH);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 3)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return "通讯故障";
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15&&!isNhSelfUse)
                        return "通讯故障";
                    else
                    {
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1]);
                        if (Read_Buffer[2] == (byte)((~CS) + 1))
                        {
                            switch (Read_Buffer[2])
                            {
                                case 0x00:
                                    return "仪器处于预热状态";
                                    break;
                                case 0x01:
                                    return "仪器处于直接测量状态";
                                    break;
                                case 0x02:
                                    return "仪器处于加速测量状态";
                                    break;
                                case 0xff:
                                    return "仪器处于其他操作方式";
                                    break;
                                default:
                                    return "仪器处于未知状态";
                                    break;

                            }
                        }
                        else
                            return "通讯故障";

                    }
                    break;
                default:
                    return "未提供该型号此操作";
                    break;
            }
            
        }
        #endregion
        #region 进入实时测量状态
        /// <summary>
        /// 进入直接测量状态
        /// </summary>
        /// <returns>bool</returns>
        public bool Set_DermacateWD(double wd)
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {
                
                case "mqy_200":
                    byte[] Content_MQ = new byte[] { cmdDemarcateTemp_MQ, 0x00, 0x00 };
                    Content_MQ[1] = (byte)((wd * 10) / 256);
                    Content_MQ[2] = (byte)((wd * 10) % 256);
                    SendData(Content_MQ);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 3)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return false;
                    else if (Read_Buffer[0] == cmdDemarcateTemp_MQ)
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
        #region 进入实时测量状态
        /// <summary>
        /// 进入直接测量状态
        /// </summary>
        /// <returns>bool</returns>
        public bool Set_DermacateSD(double sd)
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {

                case "mqy_200":
                    byte[] Content_MQ = new byte[] { cmdDemarcateHumi_MQ, 0x00, 0x00 };
                    Content_MQ[1] = (byte)((sd * 10) / 256);
                    Content_MQ[2] = (byte)((sd * 10) % 256);
                    SendData(Content_MQ);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 3)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return false;
                    else if (Read_Buffer[0] == cmdDemarcateHumi_MQ)
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
        #region 进入实时测量状态
        /// <summary>
        /// 进入直接测量状态
        /// </summary>
        /// <returns>bool</returns>
        public bool Set_DermacateDQY(double dqy)
        {
            int i = 0;
            ReadData();
            switch (yqxh)
            {

                case "mqy_200":
                    byte[] Content_MQ = new byte[] { cmdDemarcateAirP_MQ, 0x00, 0x00 };
                    Content_MQ[1] = (byte)((dqy * 100) / 256);
                    Content_MQ[2] = (byte)((dqy * 100) % 256);
                    SendData(Content_MQ);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 3)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return false;
                    else if (Read_Buffer[0] == cmdDemarcateAirP_MQ)
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
                case "flb_100":
                    byte[] Content = new byte[] {0x01};
                    SendData(set_TestDirectly);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead<4)                          //等待仪器返回
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
                    break;
                case "mqy_200":
                    byte[] Content_MQ = new byte[] {cmdSetTestMethod_MQ,0x01,0};
                    Content_MQ[2] = getCS_MQ(Content_MQ, 2);
                    SendData(Content_MQ);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead <2)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return false;
                    else if (Read_Buffer[0] == cmdSetTestMethod_MQ)
                        return true;
                    else
                        return false;
                    break;
                case "nht_1":
                    byte[] Content_NH = new byte[] { cmdSetTestMethod_NH, 0x01, 0 };
                    Content_NH[2] = getCS_MQ(Content_NH, 2);
                    SendData(Content_NH);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 2)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15 && !isNhSelfUse)
                        return false;
                    else if (Read_Buffer[0] == cmdSetTestMethod_NH)
                        return true;
                    else
                        return false;
                    break;
                case "fty_100":
                    byte[] Content_FF = new byte[] { 1, 2, cmdTestScreen_FF, 0 };
                    Content_FF[3] = getCS_MQ(Content_FF, 1, 3);
                    SendData(Content_FF);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 4)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                        {
                            return false;
                        }
                    }
                    ReadData();
                    if (Read_Buffer[2] == 0xff)
                    {
                        return false;
                    }
                    else
                        return true;
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
                case "flb_100":
                    byte[] Content = new byte[] { 0x01 };
                    SendData(set_TestDirectly);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
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
                    break;
                case "mqy_200":
                    byte[] Content_MQ = new byte[] { cmdSetTestMethod_MQ, 0x01, 0 };
                    Content_MQ[2] = getCS_MQ(Content_MQ, 2);
                    SendData(Content_MQ);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 2)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return false;
                    else if (Read_Buffer[0] == cmdSetTestMethod_MQ)
                        return true;
                    else
                        return false;
                    break;
                case "nht_1":
                    byte[] Content_NH = new byte[] { cmdSetTestMethod_NH, 0x01, 0 };
                    Content_NH[2] = getCS_MQ(Content_NH, 2);
                    SendData(Content_NH);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 2)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15 && !isNhSelfUse)
                        return false;
                    else if (Read_Buffer[0] == cmdSetTestMethod_NH)
                        return true;
                    else
                        return false;
                    break;
                case "fty_100":
                    byte[] Content_FF = new byte[] { 1, 2, cmdStableScreen_FF, 0 };
                    Content_FF[3] = getCS_MQ(Content_FF, 1, 3);
                    SendData(Content_FF);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 4)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                        {
                            return false;
                        }
                    }
                    ReadData();
                    if (Read_Buffer[2] == 0xff)
                    {
                        return false;
                    }
                    else
                        return true;
                    break;
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
            switch (yqxh)
            {
                case "flb_100":
                    /*
                    //string Measurement = Get_Mode();
                    ReadData();
                    //ComPort_3.ReceivedBytesThreshold = 4;
                    byte[] Content = new byte[] { 0x01 };
                    SendData(start_Stabletest);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
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
                        return false;*/
                    return true;

                    break;
                case "fty_100":
                    byte[] Content_FF = new byte[] { 1, 2, cmdStartOrStopStableTest_FF, 0 };
                    Content_FF[3] = getCS_MQ(Content_FF, 1, 3);
                    SendData(Content_FF);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 4)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                        {
                            return false;
                        }
                    }
                    ReadData();
                    if (Read_Buffer[2] == 0xff)
                    {
                        return false;
                    }
                    else
                        return true;
                    break;
                case "mqy_200":
                    return true;
                case "nht_1":
                    return true;
                default:
                    return false;
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
            switch (yqxh)
            {
                case "flb_100":
                    /*
                    //string Measurement = Get_Mode();
                    ReadData();
                    //ComPort_3.ReceivedBytesThreshold = 4;
                    byte[] Content = new byte[] { 0x01 };
                    SendData(stop_Stabletest);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(5);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                        return true;
                    else
                        return false;*/
                    return true;
                    break;
                case "fty_100":
                    byte[] Content_FF = new byte[] { 1, 2, cmdStartOrStopStableTest_FF, 0 };
                    Content_FF[3] = getCS_MQ(Content_FF, 1, 3);
                    SendData(Content_FF);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 4)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                        {
                            return false;
                        }
                    }
                    ReadData();
                    if (Read_Buffer[2] == 0xff)
                    {
                        return false;
                    }
                    else
                        return true;
                    break;
                case "mqy_200":
                    return true;
                case "nht_1":
                    return true;
                default:
                    return false;
            }
        }
        #endregion
        #region 开始加速测量
        /// <summary>
        /// 开始加速测量
        /// </summary>
        /// <returns>bool</returns>
        public bool Start_accelerateMeasure()
        {
            int i = 0;
            switch (yqxh)
            {
                case "flb_100":
                    return true;
                    break;
                case "fty_100":
                    byte[] Content_FF = new byte[] { 1, 2, cmdAccelerateScreen_FF, 0 };
                    Content_FF[3] = getCS_MQ(Content_FF, 3);
                    SendData(Content_FF);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 4)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                        {
                            return false;
                        }
                    }
                    ReadData();
                    if (Read_Buffer[2] == 0xff)
                    {
                        return false;
                    }
                    else
                        return true;
                    break;
                case "mqy_200":
                    return true;
                case "nht_1":
                    return true;
                default:
                    return false;
            }
        }
        #endregion
        #region 触发一次加速测量
        /// <summary>
        /// 触发一次加速测量
        /// </summary>
        /// <returns>bool</returns>
        public bool Start_triggerAccelerateMeasure()
        {
            int i = 0;
            switch (yqxh)
            {
                case "flb_100":
                    return true;
                    break;
                case "fty_100":
                    byte[] Content_FF = new byte[] { 1, 2, cmdTriggerOneAccelerate_FF, 0 };
                    Content_FF[3] = getCS_MQ(Content_FF, 1, 3);
                    SendData(Content_FF);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 4)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                        {
                            return false;
                        }
                    }
                    ReadData();
                    if (Read_Buffer[2] == 0xff)
                    {
                        return false;
                    }
                    else
                        return true;
                    break;
                case "mqy_200":
                    return true;
                case "nht_1":
                    return true;
                default:
                    return false;
            }
        }
        #endregion
        #region 切换加速方式
        /// <summary>
        /// 触发一次加速测量
        /// </summary>
        /// <returns>bool</returns>
        public bool Start_changeAccelerateStyle()
        {
            int i = 0;
            switch (yqxh)
            {
                case "flb_100":
                    return true;
                    break;
                case "fty_100":
                    byte[] Content_FF = new byte[] { 1, 2, cmdChangeAccelerateStyle_FF, 0 };
                    Content_FF[3] = getCS_MQ(Content_FF, 1, 3);
                    SendData(Content_FF);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 4)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                        {
                            return false;
                        }
                    }
                    ReadData();
                    if (Read_Buffer[2] == 0xff)
                    {
                        return false;
                    }
                    else
                        return true;
                    break;
                case "mqy_200":
                    return true;
                case "nht_1":
                    return true;
                default:
                    return false;
            }
        }
        #endregion
        Flb_100_smoke smoke = new Flb_100_smoke();
        #region 取仪器测量数据
        public Flb_100_smoke get_Data()
        {            
            int i = 0;
            byte CS = 0;
            ReadData();
            switch (yqxh)
            {
                case "flb_100":
                    //ComPort_3.ReceivedBytesThreshold = 13;
                    //byte[] Content = new byte[] {0x01};
                    SendData(get_MeasureDat);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 13)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(30);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return smoke;
                    else
                    {
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] + Read_Buffer[9] + Read_Buffer[10] + Read_Buffer[11]);
                        if (Read_Buffer[12] == (byte)((~CS) + 1))
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            smoke.K = (float)(Math.Round(kxs * (BitConverter.ToInt16(temp_byte, 0)/100f), 2));
                            
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            smoke.Ns = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            
                            smoke.Qw = Read_Buffer[7];
                            temp_byte[0] = Read_Buffer[9];
                            temp_byte[1] = Read_Buffer[8];
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[11];
                            temp_byte[1] = Read_Buffer[10];
                            smoke.Zs = BitConverter.ToInt16(temp_byte, 0);
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                case "mqy_200":
                    byte[] Content_MQ = new byte[] { cmdGetData_MQ, 0 };
                    Content_MQ[1] = getCS_MQ(Content_MQ, 1);
                    SendData(Content_MQ);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 16)                          //等待仪器返回
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
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] + Read_Buffer[9] + Read_Buffer[10] + Read_Buffer[11] + Read_Buffer[12] + Read_Buffer[13] + Read_Buffer[14]);
                        if (Read_Buffer[15] == (byte)((~CS) + 1))
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
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0) - 273;
                            temp_byte[0] = Read_Buffer[10];
                            temp_byte[1] = Read_Buffer[9];
                            smoke.DQY = BitConverter.ToInt16(temp_byte, 0) / 100f;
                            temp_byte[0] = Read_Buffer[12];
                            temp_byte[1] = (byte)(Read_Buffer[11]&0x7f);
                            smoke.WD = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            if ((Read_Buffer[11] & 0x80) == 0x80)
                                smoke.WD = -smoke.WD;
                            temp_byte[0] = Read_Buffer[14];
                            temp_byte[1] = Read_Buffer[13];
                            smoke.SD = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                case yq_mqw5101:
                    SendData(cmdGetRealData_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 26)                          //等待仪器返回
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
                        CS = getCS_MQ(Read_Buffer, 25);// (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] + Read_Buffer[9] + Read_Buffer[10] + Read_Buffer[11] + Read_Buffer[12] + Read_Buffer[13] + Read_Buffer[14]);
                        if (Read_Buffer[25] == CS)
                        {
                            smoke.No = (float)(((short)(Read_Buffer[3] << 8 | Read_Buffer[4])) * 1f);
                            smoke.WD = (float)(((short)(Read_Buffer[5] << 8 | Read_Buffer[6])) * 0.1f);// BitConverter.ToInt16(temp_byte, 0) / 10f;
                            smoke.SD = (float)(((short)(Read_Buffer[7] << 8 | Read_Buffer[8])) * 0.1f);
                            smoke.DQY = (float)(((short)(Read_Buffer[9] << 8 | Read_Buffer[10])) * 0.01f);
                            smoke.Yw = (float)(((short)(Read_Buffer[11] << 8 | Read_Buffer[12])) * 0.1f);
                            smoke.Glyl = (float)(((short)(Read_Buffer[13] << 8 | Read_Buffer[14])) * 0.1f);
                            smoke.Zs = (float)(((short)(Read_Buffer[15] << 8 | Read_Buffer[16])) * 1f);
                            smoke.N = (float)(((short)(Read_Buffer[17] << 8 | Read_Buffer[18])) * 0.1f);
                            smoke.Ns = (float)(((short)(Read_Buffer[19] << 8 | Read_Buffer[20])) * 0.1f);
                            smoke.K = (float)(((short)(Read_Buffer[21] << 8 | Read_Buffer[22])) * 0.01f);
                            smoke.Qswd = Read_Buffer[23];
                            smoke.Qswd = Read_Buffer[24];
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                case "nht_1":
                    byte[] Content_NH = new byte[] { cmdGetDirectData_NH, 0 };
                    Content_NH[1] = getCS_MQ(Content_NH, 1);
                    SendData(Content_NH);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 10)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(30);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15 && !isNhSelfUse)
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
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0) - 273;

                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;

                case "fty_100":
                    byte[] Content_FF = new byte[] { 1, 2, cmdGetData_FF, 0 };
                    Content_FF[3] = getCS_MQ(Content_FF, 1, 3);
                    SendData(Content_FF);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 10)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                        {
                            return smoke;
                        }
                    }
                    ReadData();
                    if (Read_Buffer[2] == cmdGetData_FF)
                    {
                        CS = (byte)(Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8]);
                        if (true)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            smoke.Ns = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            smoke.K = (float)(-Math.Log(1 - (double)smoke.Ns / 100.0) / 0.43);
                            smoke.WD = 20.6f;
                            smoke.Qw = (float)Read_Buffer[6] / 10;
                            temp_byte[0] = Read_Buffer[8];
                            temp_byte[1] = Read_Buffer[7];
                            smoke.Zs = BitConverter.ToInt16(temp_byte, 0);
                            smoke.DQY = 101.5f;
                            smoke.SD = 53.6f;
                            smoke.Success = true;
                            return smoke;
                        }
                        else
                        {
                            smoke.Success = false;
                            return smoke;
                        }
                    }
                    else
                    {
                        smoke.Success = false;
                        return smoke;
                    }
                    break;
                default:
                    return smoke;
                    break;
            }
        }
        public Flb_100_smoke get_Data(float minKvalue)
        {
            
            int i = 0;
            byte CS = 0;
            ReadData();
            switch (yqxh)
            {
                case "flb_100":
                    //ComPort_3.ReceivedBytesThreshold = 13;
                    //byte[] Content = new byte[] {0x01};
                    SendData(get_MeasureDat);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 13)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(30);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return smoke;
                    else
                    {
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] + Read_Buffer[9] + Read_Buffer[10] + Read_Buffer[11]);
                        if (Read_Buffer[12] == (byte)((~CS) + 1))
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            smoke.K = (float)(Math.Round(kxs * (BitConverter.ToInt16(temp_byte, 0) / 100f), 2));
                            if (smoke.K <= minKvalue) smoke.K = minKvalue;

                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            smoke.Ns = BitConverter.ToInt16(temp_byte, 0) / 10f;

                            smoke.Qw = Read_Buffer[7];
                            temp_byte[0] = Read_Buffer[9];
                            temp_byte[1] = Read_Buffer[8];
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[11];
                            temp_byte[1] = Read_Buffer[10];
                            smoke.Zs = BitConverter.ToInt16(temp_byte, 0);
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                case "mqy_200":
                    byte[] Content_MQ = new byte[] { cmdGetData_MQ, 0 };
                    Content_MQ[1] = getCS_MQ(Content_MQ, 1);
                    SendData(Content_MQ);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 16)                          //等待仪器返回
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
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] + Read_Buffer[9] + Read_Buffer[10] + Read_Buffer[11] + Read_Buffer[12] + Read_Buffer[13] + Read_Buffer[14]);
                        if (Read_Buffer[15] == (byte)((~CS) + 1))
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
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0) - 273;
                            temp_byte[0] = Read_Buffer[10];
                            temp_byte[1] = Read_Buffer[9];
                            smoke.DQY = BitConverter.ToInt16(temp_byte, 0) / 100f;
                            temp_byte[0] = Read_Buffer[12];
                            temp_byte[1] = (byte)(Read_Buffer[11] & 0x7f);
                            smoke.WD = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            if ((Read_Buffer[11] & 0x80) == 0x80)
                                smoke.WD = -smoke.WD;
                            temp_byte[0] = Read_Buffer[14];
                            temp_byte[1] = Read_Buffer[13];
                            smoke.SD = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                case yq_mqw5101:
                    SendData(cmdGetRealData_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 26)                          //等待仪器返回
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
                        CS = getCS_MQ(Read_Buffer, 25);// (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] + Read_Buffer[9] + Read_Buffer[10] + Read_Buffer[11] + Read_Buffer[12] + Read_Buffer[13] + Read_Buffer[14]);
                        if (Read_Buffer[25] == CS)
                        {
                            smoke.No = (float)(((short)(Read_Buffer[3] << 8 | Read_Buffer[4])) * 1f);
                            smoke.WD = (float)(((short)(Read_Buffer[5] << 8 | Read_Buffer[6])) * 0.1f);// BitConverter.ToInt16(temp_byte, 0) / 10f;
                            smoke.SD = (float)(((short)(Read_Buffer[7] << 8 | Read_Buffer[8])) * 0.1f);
                            smoke.DQY = (float)(((short)(Read_Buffer[9] << 8 | Read_Buffer[10])) * 0.01f);
                            smoke.Yw = (float)(((short)(Read_Buffer[11] << 8 | Read_Buffer[12])) * 0.1f);
                            smoke.Glyl = (float)(((short)(Read_Buffer[13] << 8 | Read_Buffer[14])) * 0.1f);
                            smoke.Zs = (float)(((short)(Read_Buffer[15] << 8 | Read_Buffer[16])) * 1f);
                            smoke.N = (float)(((short)(Read_Buffer[17] << 8 | Read_Buffer[18])) * 0.1f);
                            smoke.Ns = (float)(((short)(Read_Buffer[19] << 8 | Read_Buffer[20])) * 0.1f);
                            smoke.K = (float)(((short)(Read_Buffer[21] << 8 | Read_Buffer[22])) * 0.01f);
                            if (smoke.K <= minKvalue) smoke.K = minKvalue;
                            smoke.Qswd = Read_Buffer[23];
                            smoke.Qswd = Read_Buffer[24];
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                case "nht_1":
                    byte[] Content_NH = new byte[] { cmdGetDirectData_NH, 0 };
                    Content_NH[1] = getCS_MQ(Content_NH, 1);
                    SendData(Content_NH);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 10)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(30);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15 && !isNhSelfUse)
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
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0) - 273;

                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;

                case "fty_100":
                    byte[] Content_FF = new byte[] { 1, 2, cmdGetData_FF, 0 };
                    Content_FF[3] = getCS_MQ(Content_FF, 1, 3);
                    SendData(Content_FF);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 10)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                        {
                            return smoke;
                        }
                    }
                    ReadData();
                    if (Read_Buffer[2] == cmdGetData_FF)
                    {
                        CS = (byte)(Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8]);
                        if (true)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            smoke.Ns = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            smoke.K = (float)(-Math.Log(1 - (double)smoke.Ns / 100.0) / 0.43);
                            if (smoke.K <= minKvalue) smoke.K = minKvalue;
                            smoke.WD = 20.6f;
                            smoke.Qw = (float)Read_Buffer[6] / 10;
                            temp_byte[0] = Read_Buffer[8];
                            temp_byte[1] = Read_Buffer[7];
                            smoke.Zs = BitConverter.ToInt16(temp_byte, 0);
                            smoke.DQY = 101.5f;
                            smoke.SD = 53.6f;
                            smoke.Success = true;
                            return smoke;
                        }
                        else
                        {
                            smoke.Success = false;
                            return smoke;
                        }
                    }
                    else
                    {
                        smoke.Success = false;
                        return smoke;
                    }
                    break;
                default:
                    return smoke;
                    break;
            }
        }
        #endregion
        #region 取仪器测量数据
        public Flb_100_smoke get_DirectData()
        {
            
            int i = 0;
            byte CS=0;
            ReadData();
            switch (yqxh)
            {
                case "flb_100":
                    //ComPort_3.ReceivedBytesThreshold = 13;
                    //byte[] Content = new byte[] {0x01};
                    SendData(get_MeasureDat);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 13)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(30);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return smoke;
                    else
                    {
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] + Read_Buffer[9] + Read_Buffer[10] + Read_Buffer[11]);
                        if (Read_Buffer[12] == (byte)((~CS) + 1))
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            smoke.K = (float)(Math.Round(kxs * (BitConverter.ToInt16(temp_byte, 0) / 100f), 2));
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            smoke.Ns = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            smoke.Qw = Read_Buffer[7];
                            temp_byte[0] = Read_Buffer[9];
                            temp_byte[1] = Read_Buffer[8];
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[11];
                            temp_byte[1] = Read_Buffer[10];
                            smoke.Zs = BitConverter.ToInt16(temp_byte, 0);
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                case "mqy_200":
                    byte[] Content_MQ = new byte[] { cmdGetDirectData_MQ, 0 };
                    Content_MQ[1] = getCS_MQ(Content_MQ, 1);
                    SendData(Content_MQ);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 10)                          //等待仪器返回
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
                            smoke.Zs = BitConverter.ToInt16(temp_byte, 0) ;
                            temp_byte[0] = Read_Buffer[8];
                            temp_byte[1] = Read_Buffer[7];
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0) -273;                            
                            smoke.DQY =0f;                            
                            smoke.WD = 0f;                            
                            smoke.SD = 0f;
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                case yq_mqw5101:
                    SendData(cmdGetRealData_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 26)                          //等待仪器返回
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
                        CS = getCS_MQ(Read_Buffer, 25);// (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] + Read_Buffer[9] + Read_Buffer[10] + Read_Buffer[11] + Read_Buffer[12] + Read_Buffer[13] + Read_Buffer[14]);
                        if (Read_Buffer[25] == CS)
                        {
                            smoke.No = (float)(((short)(Read_Buffer[3] << 8 | Read_Buffer[4])) * 1f);
                            smoke.WD = (float)(((short)(Read_Buffer[5] << 8 | Read_Buffer[6])) * 0.1f);// BitConverter.ToInt16(temp_byte, 0) / 10f;
                            smoke.SD = (float)(((short)(Read_Buffer[7] << 8 | Read_Buffer[8])) * 0.1f);
                            smoke.DQY = (float)(((short)(Read_Buffer[9] << 8 | Read_Buffer[10])) * 0.01f);
                            smoke.Yw = (float)(((short)(Read_Buffer[11] << 8 | Read_Buffer[12])) * 0.1f);
                            smoke.Glyl = (float)(((short)(Read_Buffer[13] << 8 | Read_Buffer[14])) * 0.1f);
                            smoke.Zs = (float)(((short)(Read_Buffer[15] << 8 | Read_Buffer[16])) * 1f);
                            smoke.N = (float)(((short)(Read_Buffer[17] << 8 | Read_Buffer[18])) * 0.1f);
                            smoke.Ns = (float)(((short)(Read_Buffer[19] << 8 | Read_Buffer[20])) * 0.1f);
                            smoke.K = (float)(((short)(Read_Buffer[21] << 8 | Read_Buffer[22])) * 0.01f);
                            //if (smoke.K <= minKvalue) smoke.K = minKvalue;
                            smoke.Qswd = Read_Buffer[23];
                            smoke.Qswd = Read_Buffer[24];
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                case "nht_1":
                    byte[] Content_NH = new byte[] { cmdGetDirectData_NH, 0 };
                    Content_NH[1] = getCS_MQ(Content_NH, 1);
                    SendData(Content_NH);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 10)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(30);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15 && !isNhSelfUse)
                        return smoke;
                    else
                    {
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] );
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
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0) - 273;
                            
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
           
            int i = 0;
            byte CS = 0;
            ReadData();
            switch (yqxh)
            {
                case "flb_100":
                    //ComPort_3.ReceivedBytesThreshold = 13;
                    //byte[] Content = new byte[] {0x01};
                    SendData(get_MeasureDat);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 13)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(30);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return smoke;
                    else
                    {
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] + Read_Buffer[9] + Read_Buffer[10] + Read_Buffer[11]);
                        if (Read_Buffer[12] == (byte)((~CS) + 1))
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            smoke.K = (float)(Math.Round(kxs * (BitConverter.ToInt16(temp_byte, 0) / 100f), 2));
                            if (smoke.K <= minKvalue) smoke.K = minKvalue;
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            smoke.Ns = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            smoke.Qw = Read_Buffer[7];
                            temp_byte[0] = Read_Buffer[9];
                            temp_byte[1] = Read_Buffer[8];
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[11];
                            temp_byte[1] = Read_Buffer[10];
                            smoke.Zs = BitConverter.ToInt16(temp_byte, 0);
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                case "mqy_200":
                    byte[] Content_MQ = new byte[] { cmdGetDirectData_MQ, 0 };
                    Content_MQ[1] = getCS_MQ(Content_MQ, 1);
                    SendData(Content_MQ);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 10)                          //等待仪器返回
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
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0) - 273;
                            smoke.DQY = 0f;
                            smoke.WD = 0f;
                            smoke.SD = 0f;
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                case yq_mqw5101:
                    SendData(cmdGetRealData_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 26)                          //等待仪器返回
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
                        CS = getCS_MQ(Read_Buffer, 25);// (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] + Read_Buffer[9] + Read_Buffer[10] + Read_Buffer[11] + Read_Buffer[12] + Read_Buffer[13] + Read_Buffer[14]);
                        if (Read_Buffer[25] == CS)
                        {
                            smoke.No = (float)(((short)(Read_Buffer[3] << 8 | Read_Buffer[4])) * 1f);
                            smoke.WD = (float)(((short)(Read_Buffer[5] << 8 | Read_Buffer[6])) * 0.1f);// BitConverter.ToInt16(temp_byte, 0) / 10f;
                            smoke.SD = (float)(((short)(Read_Buffer[7] << 8 | Read_Buffer[8])) * 0.1f);
                            smoke.DQY = (float)(((short)(Read_Buffer[9] << 8 | Read_Buffer[10])) * 0.01f);
                            smoke.Yw = (float)(((short)(Read_Buffer[11] << 8 | Read_Buffer[12])) * 0.1f);
                            smoke.Glyl = (float)(((short)(Read_Buffer[13] << 8 | Read_Buffer[14])) * 0.1f);
                            smoke.Zs = (float)(((short)(Read_Buffer[15] << 8 | Read_Buffer[16])) * 1f);
                            smoke.N = (float)(((short)(Read_Buffer[17] << 8 | Read_Buffer[18])) * 0.1f);
                            smoke.Ns = (float)(((short)(Read_Buffer[19] << 8 | Read_Buffer[20])) * 0.1f);
                            smoke.K = (float)(((short)(Read_Buffer[21] << 8 | Read_Buffer[22])) * 0.01f);
                            if (smoke.K <= minKvalue) smoke.K = minKvalue;
                            smoke.Qswd = Read_Buffer[23];
                            smoke.Qswd = Read_Buffer[24];
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                case "nht_1":
                    byte[] Content_NH = new byte[] { cmdGetDirectData_NH, 0 };
                    Content_NH[1] = getCS_MQ(Content_NH, 1);
                    SendData(Content_NH);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 10)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(30);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15 && !isNhSelfUse)
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
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0) - 273;

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
        #region 取稳态值
        public Flb_100_smoke get_StableData()
        {
            //string Measurement = Get_Mode();
            
            int i = 0;
            byte CS = 0;
            ReadData();
            switch (yqxh)
            {
                case "flb_100":
                    //ComPort_3.ReceivedBytesThreshold =13;
                    //byte[] Content = new byte[] {0x01};
                    SendData(get_StableDat);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 12)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return smoke;
                    else
                    {
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] + Read_Buffer[9] + Read_Buffer[10]);
                        if (Read_Buffer[11] == (byte)((~CS) + 1))
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            smoke.K = BitConverter.ToInt16(temp_byte, 0) / 100f;
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            smoke.Ns = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            //smoke.Qw = Read_Buffer[7];
                            temp_byte[0] = Read_Buffer[10];
                            temp_byte[1] = Read_Buffer[9];
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[8];
                            temp_byte[1] = Read_Buffer[7];
                            smoke.Zs = BitConverter.ToInt16(temp_byte, 0);
                        }
                        return smoke;
                    }
                    break;
                case "fty_100":
                    byte[] Content_FF = new byte[] { 1, 2, cmdGetStableData_FF, 0 };
                    Content_FF[3] = getCS_MQ(Content_FF, 1, 3);
                    SendData(Content_FF);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 8)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                        {
                            return smoke;
                        }
                    }
                    ReadData();
                    if (Read_Buffer[2] == cmdGetStableData_FF)
                    {
                        CS = (byte)(Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6]);
                        if (true)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            smoke.Ns = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            smoke.K = (float)(-Math.Log(1 - (double)smoke.Ns / 100.0) / 0.43);
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            smoke.Zs = BitConverter.ToInt16(temp_byte, 0);
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    else
                        return smoke;
                    break;
                case "mqy_200":
                    byte[] Content_MQ = new byte[] { cmdGetMaxData_MQ, 0 };
                    Content_MQ[1] = getCS_MQ(Content_MQ, 1);
                    SendData(Content_MQ);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 8)                          //等待仪器返回
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
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6]);
                        if (Read_Buffer[7] == (byte)((~CS) + 1))
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
                            smoke.Zs = BitConverter.ToInt16(temp_byte, 0) ;
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                case yq_mqw5101:
                    SendData(cmdGetRealData_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 26)                          //等待仪器返回
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
                        CS = getCS_MQ(Read_Buffer, 25);// (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] + Read_Buffer[9] + Read_Buffer[10] + Read_Buffer[11] + Read_Buffer[12] + Read_Buffer[13] + Read_Buffer[14]);
                        if (Read_Buffer[25] == CS)
                        {
                            smoke.No = (float)(((short)(Read_Buffer[3] << 8 | Read_Buffer[4])) * 1f);
                            smoke.WD = (float)(((short)(Read_Buffer[5] << 8 | Read_Buffer[6])) * 0.1f);// BitConverter.ToInt16(temp_byte, 0) / 10f;
                            smoke.SD = (float)(((short)(Read_Buffer[7] << 8 | Read_Buffer[8])) * 0.1f);
                            smoke.DQY = (float)(((short)(Read_Buffer[9] << 8 | Read_Buffer[10])) * 0.01f);
                            smoke.Yw = (float)(((short)(Read_Buffer[11] << 8 | Read_Buffer[12])) * 0.1f);
                            smoke.Glyl = (float)(((short)(Read_Buffer[13] << 8 | Read_Buffer[14])) * 0.1f);
                            smoke.Zs = (float)(((short)(Read_Buffer[15] << 8 | Read_Buffer[16])) * 1f);
                            smoke.N = (float)(((short)(Read_Buffer[17] << 8 | Read_Buffer[18])) * 0.1f);
                            smoke.Ns = (float)(((short)(Read_Buffer[19] << 8 | Read_Buffer[20])) * 0.1f);
                            smoke.K = (float)(((short)(Read_Buffer[21] << 8 | Read_Buffer[22])) * 0.01f);
                            //if (smoke.K <= minKvalue) smoke.K = minKvalue;
                            smoke.Qswd = Read_Buffer[23];
                            smoke.Qswd = Read_Buffer[24];
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                case "nht_1":
                    byte[] Content_NH = new byte[] { cmdGetMaxData_NH, 0 };
                    Content_NH[1] = getCS_MQ(Content_NH, 1);
                    SendData(Content_NH);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 8)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(30);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15 && !isNhSelfUse)
                        return smoke;
                    else
                    {
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6]);
                        if (Read_Buffer[7] == (byte)((~CS) + 1))
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
            
            int i = 0;
            byte CS = 0;
            ReadData();
            switch (yqxh)
            {
                case "flb_100":
                    //ComPort_3.ReceivedBytesThreshold =13;
                    //byte[] Content = new byte[] {0x01};
                    SendData(get_StableDat);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 12)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return smoke;
                    else
                    {
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] + Read_Buffer[9] + Read_Buffer[10]);
                        if (Read_Buffer[11] == (byte)((~CS) + 1))
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            smoke.K = BitConverter.ToInt16(temp_byte, 0) / 100f;
                            if (smoke.K <= minKvalue) smoke.K = minKvalue;
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            smoke.Ns = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            //smoke.Qw = Read_Buffer[7];
                            temp_byte[0] = Read_Buffer[10];
                            temp_byte[1] = Read_Buffer[9];
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[8];
                            temp_byte[1] = Read_Buffer[7];
                            smoke.Zs = BitConverter.ToInt16(temp_byte, 0);
                        }
                        return smoke;
                    }
                    break;
                case "fty_100":
                    byte[] Content_FF = new byte[] { 1, 2, cmdGetStableData_FF, 0 };
                    Content_FF[3] = getCS_MQ(Content_FF, 1, 3);
                    SendData(Content_FF);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 8)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                        {
                            return smoke;
                        }
                    }
                    ReadData();
                    if (Read_Buffer[2] == cmdGetStableData_FF)
                    {
                        CS = (byte)(Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6]);
                        if (true)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            smoke.Ns = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            smoke.K = (float)(-Math.Log(1 - (double)smoke.Ns / 100.0) / 0.43);
                            if (smoke.K <= minKvalue) smoke.K = minKvalue;
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            smoke.Zs = BitConverter.ToInt16(temp_byte, 0);
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    else
                        return smoke;
                    break;
                case "mqy_200":
                    byte[] Content_MQ = new byte[] { cmdGetMaxData_MQ, 0 };
                    Content_MQ[1] = getCS_MQ(Content_MQ, 1);
                    SendData(Content_MQ);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 8)                          //等待仪器返回
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
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6]);
                        if (Read_Buffer[7] == (byte)((~CS) + 1))
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
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                case yq_mqw5101:
                    SendData(cmdGetRealData_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 26)                          //等待仪器返回
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
                        CS = getCS_MQ(Read_Buffer, 25);// (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] + Read_Buffer[9] + Read_Buffer[10] + Read_Buffer[11] + Read_Buffer[12] + Read_Buffer[13] + Read_Buffer[14]);
                        if (Read_Buffer[25] == CS)
                        {
                            smoke.No = (float)(((short)(Read_Buffer[3] << 8 | Read_Buffer[4])) * 1f);
                            smoke.WD = (float)(((short)(Read_Buffer[5] << 8 | Read_Buffer[6])) * 0.1f);// BitConverter.ToInt16(temp_byte, 0) / 10f;
                            smoke.SD = (float)(((short)(Read_Buffer[7] << 8 | Read_Buffer[8])) * 0.1f);
                            smoke.DQY = (float)(((short)(Read_Buffer[9] << 8 | Read_Buffer[10])) * 0.01f);
                            smoke.Yw = (float)(((short)(Read_Buffer[11] << 8 | Read_Buffer[12])) * 0.1f);
                            smoke.Glyl = (float)(((short)(Read_Buffer[13] << 8 | Read_Buffer[14])) * 0.1f);
                            smoke.Zs = (float)(((short)(Read_Buffer[15] << 8 | Read_Buffer[16])) * 1f);
                            smoke.N = (float)(((short)(Read_Buffer[17] << 8 | Read_Buffer[18])) * 0.1f);
                            smoke.Ns = (float)(((short)(Read_Buffer[19] << 8 | Read_Buffer[20])) * 0.1f);
                            smoke.K = (float)(((short)(Read_Buffer[21] << 8 | Read_Buffer[22])) * 0.01f);
                            if (smoke.K <= minKvalue) smoke.K = minKvalue;
                            smoke.Qswd = Read_Buffer[23];
                            smoke.Qswd = Read_Buffer[24];
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                case "nht_1":
                    byte[] Content_NH = new byte[] { cmdGetMaxData_NH, 0 };
                    Content_NH[1] = getCS_MQ(Content_NH, 1);
                    SendData(Content_NH);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 8)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(30);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15 && !isNhSelfUse)
                        return smoke;
                    else
                    {
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6]);
                        if (Read_Buffer[7] == (byte)((~CS) + 1))
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
                case "flb_100":
                    SendData(apply_LinearJiaozheng);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                case "mqy_200":
                    byte[] Content_MQ = new byte[] { cmdDemarcate_MQ,  0 };
                    Content_MQ[1] = getCS_MQ(Content_MQ, 1);
                    SendData(Content_MQ);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 2)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return false;
                    else if (Read_Buffer[0] == cmdDemarcate_MQ)
                        return true;
                    else
                        return false;
                    break;
                case "fty_100":
                    byte[] Content_FF = new byte[] { 1, 2, cmdLinearDemarcate_FF, 0 };
                    Content_FF[3] = getCS_MQ(Content_FF, 1, 3);
                    SendData(Content_FF);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 4)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                        {
                            return false;
                        }
                    }
                    ReadData();
                    if (Read_Buffer[2] == cmdLinearDemarcate_FF)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                case "nht_1":
                    byte[] Content_NH = new byte[] { cmdDemarcate_NH, 0 };
                    Content_NH[1] = getCS_MQ(Content_NH, 1);
                    SendData(Content_NH);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 2)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15 && !isNhSelfUse)
                        return false;
                    else if (Read_Buffer[0] == cmdDemarcate_NH)
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
        #region 取最近一次加速主要数据
        public Flb_100_smoke get_latestAccelerateData()
        {
            //string Measurement = Get_Mode();
            
            int i = 0;
            byte CS = 0;
            ReadData();
            switch (yqxh)
            {
                case "flb_100":
                    //ComPort_3.ReceivedBytesThreshold =13;
                    //byte[] Content = new byte[] {0x01};
                    SendData(get_StableDat);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 12)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return smoke;
                    else
                    {
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] + Read_Buffer[9] + Read_Buffer[10]);
                        if (Read_Buffer[11] == (byte)((~CS) + 1))
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            smoke.K = (float)(Math.Round(kxs * (BitConverter.ToInt16(temp_byte, 0) / 100f), 2));
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            smoke.Ns = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            //smoke.Qw = Read_Buffer[7];
                            temp_byte[0] = Read_Buffer[10];
                            temp_byte[1] = Read_Buffer[9];
                            smoke.Yw = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            temp_byte[0] = Read_Buffer[8];
                            temp_byte[1] = Read_Buffer[7];
                            smoke.Zs = BitConverter.ToInt16(temp_byte, 0);
                        }
                        return smoke;
                    }
                    break;
                case "fty_100":
                    byte[] Content_FF = new byte[] { 1, 2, cmdGetLatestAccelerateData, 0 };
                    Content_FF[3] = getCS_MQ(Content_FF, 1, 3);
                    SendData(Content_FF);
                    Thread.Sleep(50);
                    while (ComPort_3.BytesToRead < 8)
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                        {
                            return smoke;
                        }
                    }
                    ReadData();
                    if (Read_Buffer[2] == cmdGetLatestAccelerateData)
                    {
                        CS = (byte)(Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6]);
                        if (true)
                        {
                            byte[] temp_byte = new byte[2];
                            temp_byte[0] = Read_Buffer[4];
                            temp_byte[1] = Read_Buffer[3];
                            smoke.Ns = BitConverter.ToInt16(temp_byte, 0) / 10f;
                            smoke.K = (float)(-Math.Log(1 - (double)smoke.Ns / 100.0) / 0.43);
                            temp_byte[0] = Read_Buffer[6];
                            temp_byte[1] = Read_Buffer[5];
                            smoke.Zs = BitConverter.ToInt16(temp_byte, 0);
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    else
                        return smoke;
                    break;
                case "mqy_200":
                    byte[] Content_MQ = new byte[] { cmdGetMaxData_MQ, 0 };
                    Content_MQ[1] = getCS_MQ(Content_MQ, 1);
                    SendData(Content_MQ);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 8)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(30);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return smoke;
                    else
                    {
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6]);
                        if (Read_Buffer[7] == (byte)((~CS) + 1))
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
                            return smoke;
                        }
                        else
                            return smoke;
                    }
                    break;
                case "nht_1":
                    byte[] Content_NH = new byte[] { cmdGetMaxData_NH, 0 };
                    Content_NH[1] = getCS_MQ(Content_NH, 1);
                    SendData(Content_NH);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 8)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(30);
                        if (i == 100)
                            return smoke;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15 && !isNhSelfUse)
                        return smoke;
                    else
                    {
                        CS = (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6]);
                        if (Read_Buffer[7] == (byte)((~CS) + 1))
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

        #region mqw_5101仪器相关协议 
        public bool pumpVehicleGas()
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    SendData(cmdPumpVehicleGas_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        public bool pumpAir()
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    SendData(cmdPumpAir_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        public bool flowBack()
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    SendData(cmdManualFlowBack_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        public bool pumpDemarcateGas()
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    SendData(cmdPumpDemarcateGas_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        public bool pumpTestGas()
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    SendData(cmdPumpTestGas_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        public bool pumpZeroGas()
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    SendData(cmdPumpZeroGas_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        public bool stopAction()
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    SendData(cmdStopAction_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }

        public bool autoFlowBack()
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    SendData(cmdAutoFlowBack_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        public bool testBackGroundAir()
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    SendData(cmdTestBackAir_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        public bool testEnvironmentAir()
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    SendData(cmdTestEnvirAir_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        public bool leakTest()
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    SendData(cmdLeakTest_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        public bool quitPrepare()
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    SendData(cmdQuitPrepare_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        public bool zeroEquipment()
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    SendData(cmdZero_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        public bool demarcateNOsingle(double value)
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    byte[] Content_MQ = cmdDemarcateNoSingle_MQW;
                    Content_MQ[3] = (byte)((value) / 256);
                    Content_MQ[4] = (byte)((value) % 256);
                    Content_MQ[5] = getCS_MQ(Content_MQ, 5);
                    SendData(Content_MQ);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        public bool demarcateTemperature(double value)
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    byte[] Content_MQ = cmdDemarcateTemp_MQW;
                    Content_MQ[3] = (byte)((value*10) / 256);
                    Content_MQ[4] = (byte)((value*10) % 256);
                    Content_MQ[5] = getCS_MQ(Content_MQ, 5);
                    SendData(Content_MQ);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        public bool demarcateHumidity(double value)
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    byte[] Content_MQ = cmdDemarcateHumidity_MQW;
                    Content_MQ[3] = (byte)((value * 10) / 256);
                    Content_MQ[4] = (byte)((value * 10) % 256);
                    Content_MQ[5] = getCS_MQ(Content_MQ, 5);
                    SendData(Content_MQ);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        public bool demarcateAirpressure(double value)
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    byte[] Content_MQ = cmdDemarcatePressure_MQW;
                    Content_MQ[3] = (byte)((value * 10) / 256);
                    Content_MQ[4] = (byte)((value * 10) % 256);
                    Content_MQ[5] = getCS_MQ(Content_MQ, 5);
                    SendData(Content_MQ);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        public bool demarcateOilTemperature(double value)
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    byte[] Content_MQ = cmdDemarcateOilTemp_MQW;
                    Content_MQ[3] = (byte)((value * 10) / 256);
                    Content_MQ[4] = (byte)((value * 10) % 256);
                    Content_MQ[5] = getCS_MQ(Content_MQ, 5);
                    SendData(Content_MQ);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        public bool demarcateGuanluPressure(double value)
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    byte[] Content_MQ = cmdDemarcateGuanluPressure_MQW;
                    Content_MQ[3] = (byte)((value * 10) / 256);
                    Content_MQ[4] = (byte)((value * 10) % 256);
                    Content_MQ[5] = getCS_MQ(Content_MQ, 5);
                    SendData(Content_MQ);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        public bool get_MQW5101_Status( out mqw_5101_status status)
        {
            //string Measurement = Get_Mode();
            status = new mqw_5101_status();
            int i = 0;
            byte CS = 0;
            ReadData();
            switch (yqxh)
            {
                case yq_mqw5101:
                    SendData(cmdGetRealStatus_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 12)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(5);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return false;
                    else
                    {
                        CS = getCS_MQ(Read_Buffer, 11);// (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] + Read_Buffer[9] + Read_Buffer[10] + Read_Buffer[11] + Read_Buffer[12] + Read_Buffer[13] + Read_Buffer[14]);
                        if (Read_Buffer[11] == CS)
                        {
                            status.isVehicleGasPumping =((Read_Buffer[3]&0x01)==0x01);
                            status.isAirGasPumping = ((Read_Buffer[3] & 0x02) == 0x02);
                            status.isFlowBacking = ((Read_Buffer[3] & 0x04) == 0x04);
                            status.isDemarcateGasPumping = ((Read_Buffer[3] & 0x08) == 0x08);
                            status.isTestGasPumping = ((Read_Buffer[3] & 0x10) == 0x10);
                            status.isZeroGasPumping = ((Read_Buffer[3] & 0x20) == 0x20);

                            status.isNOZeroing = ((Read_Buffer[4] & 0x01) == 0x01);
                            status.isNODemarcating = ((Read_Buffer[4] & 0x02) == 0x02);
                            status.isTemperatureDemarcating = ((Read_Buffer[4] & 0x04) == 0x04);
                            status.isHumidityDemarcating = ((Read_Buffer[4] & 0x08) == 0x08);
                            status.isAirPressureDemarcating = ((Read_Buffer[4] & 0x10) == 0x10);
                            status.isOilTempDemarcating = ((Read_Buffer[4] & 0x20) == 0x20);
                            status.isGuanLuPressureDemarcating = ((Read_Buffer[4] & 0x40) == 0x40);

                            status.isTesting = ((Read_Buffer[5] & 0x01) == 0x01);
                            status.isLeakTesting = ((Read_Buffer[5] & 0x02) == 0x02);
                            status.isAutoFlowBacking = ((Read_Buffer[5] & 0x04) == 0x04);
                            status.isBackGroundAirTesting = ((Read_Buffer[5] & 0x08) == 0x08);
                            status.isEnvirAirTesting = ((Read_Buffer[5] & 0x10) == 0x10);

                            status.isPrepare = ((Read_Buffer[7] & 0x01) == 0x01);
                            status.isBusy = ((Read_Buffer[7] & 0x02) == 0x02);
                            status.isLowFlowAlarm = ((Read_Buffer[7] & 0x40) == 0x40);
                            status.isLeakTestSuccess = ((Read_Buffer[7] & 0x80) == 0x80);
                            return true;
                        }
                        else
                            return false;
                    }
                    break;
                default:
                    return false;
                    break;
            }
        }
        public bool get_MQW5101_DataAndStatus(out Flb_100_smoke smoke,out mqw_5101_status status)
        {
            //string Measurement = Get_Mode();
            smoke = new Flb_100_smoke();
            status = new mqw_5101_status();
            int i = 0;
            byte CS = 0;
            ReadData();
            switch (yqxh)
            {
                case yq_mqw5101:
                    SendData(cmdGetRealDataAndStatus_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 34)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(5);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return false;
                    else
                    {
                        CS = getCS_MQ(Read_Buffer, 33);// (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] + Read_Buffer[9] + Read_Buffer[10] + Read_Buffer[11] + Read_Buffer[12] + Read_Buffer[13] + Read_Buffer[14]);
                        if (Read_Buffer[33] == CS)
                        {
                            smoke.No = (float)(((short)(Read_Buffer[3] << 8 | Read_Buffer[4])) * 1f);
                            smoke.WD = (float)(((short)(Read_Buffer[5] << 8 | Read_Buffer[6])) * 0.1f);// BitConverter.ToInt16(temp_byte, 0) / 10f;
                            smoke.SD = (float)(((short)(Read_Buffer[7] << 8 | Read_Buffer[8])) * 0.1f);
                            smoke.DQY = (float)(((short)(Read_Buffer[9] << 8 | Read_Buffer[10])) * 0.01f);
                            smoke.Yw = (float)(((short)(Read_Buffer[11] << 8 | Read_Buffer[12])) * 0.1f);
                            smoke.Glyl = (float)(((short)(Read_Buffer[13] << 8 | Read_Buffer[14])) * 0.1f);
                            smoke.Zs = (float)(((short)(Read_Buffer[15] << 8 | Read_Buffer[16])) * 1f);
                            smoke.N = (float)(((short)(Read_Buffer[17] << 8 | Read_Buffer[18])) * 0.1f);
                            smoke.Ns = (float)(((short)(Read_Buffer[19] << 8 | Read_Buffer[20])) * 0.1f);
                            smoke.K = (float)(((short)(Read_Buffer[21] << 8 | Read_Buffer[22])) * 0.01f);
                            smoke.Qswd = Read_Buffer[23];
                            smoke.Qswd = Read_Buffer[24];

                            status.isVehicleGasPumping = ((Read_Buffer[25] & 0x01) == 0x01);
                            status.isAirGasPumping = ((Read_Buffer[25] & 0x02) == 0x02);
                            status.isFlowBacking = ((Read_Buffer[25] & 0x04) == 0x04);
                            status.isDemarcateGasPumping = ((Read_Buffer[25] & 0x08) == 0x08);
                            status.isTestGasPumping = ((Read_Buffer[25] & 0x10) == 0x10);
                            status.isZeroGasPumping = ((Read_Buffer[25] & 0x20) == 0x20);

                            status.isNOZeroing = ((Read_Buffer[26] & 0x01) == 0x01);
                            status.isNODemarcating = ((Read_Buffer[26] & 0x02) == 0x02);
                            status.isTemperatureDemarcating = ((Read_Buffer[26] & 0x04) == 0x04);
                            status.isHumidityDemarcating = ((Read_Buffer[26] & 0x08) == 0x08);
                            status.isAirPressureDemarcating = ((Read_Buffer[26] & 0x10) == 0x10);
                            status.isOilTempDemarcating = ((Read_Buffer[26] & 0x20) == 0x20);
                            status.isGuanLuPressureDemarcating = ((Read_Buffer[26] & 0x40) == 0x40);

                            status.isTesting = ((Read_Buffer[27] & 0x01) == 0x01);
                            status.isLeakTesting = ((Read_Buffer[27] & 0x02) == 0x02);
                            status.isAutoFlowBacking = ((Read_Buffer[27] & 0x04) == 0x04);
                            status.isBackGroundAirTesting = ((Read_Buffer[27] & 0x08) == 0x08);
                            status.isEnvirAirTesting = ((Read_Buffer[27] & 0x10) == 0x10);

                            status.isPrepare = ((Read_Buffer[29] & 0x01) == 0x01);
                            status.isBusy = ((Read_Buffer[29] & 0x02) == 0x02);
                            status.isLowFlowAlarm = ((Read_Buffer[29] & 0x40) == 0x40);
                            status.isLeakTestSuccess = ((Read_Buffer[29] & 0x80) == 0x80);
                            return true;
                        }
                        else
                            return false;
                    }
                    break;
                default:
                    return false;
                    break;
            }
        }

        public bool get_MQW5101_BackgroundData(out short NO)
        {
            NO = 0;
            int i = 0;
            byte CS = 0;
            ReadData();
            switch (yqxh)
            {
                case yq_mqw5101:
                    SendData(cmdGetBackGroundAirData_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 6)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(5);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return false;
                    else
                    {
                        CS = getCS_MQ(Read_Buffer, 5);// (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] + Read_Buffer[9] + Read_Buffer[10] + Read_Buffer[11] + Read_Buffer[12] + Read_Buffer[13] + Read_Buffer[14]);
                        if (Read_Buffer[5] == CS)
                        {
                            NO = (short)(Read_Buffer[3] << 8 | Read_Buffer[4]);                            
                            return true;
                        }
                        else
                            return false;
                    }
                    break;
                default:
                    return false;
                    break;
            }
        }

        public bool get_MQW5101_EvironmentData(out short NO,out float wd,out float sd,out float dqy,out float yw)
        {
            NO = 0;
            wd = 0;
            sd = 0;
            dqy = 0;
            yw = 0;
            int i = 0;
            byte CS = 0;
            ReadData();
            switch (yqxh)
            {
                case yq_mqw5101:
                    SendData(cmdGetEnviAirData_MQW);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 14)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(5);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x15)
                        return false;
                    else
                    {
                        CS = getCS_MQ(Read_Buffer, 13);// (byte)(Read_Buffer[0] + Read_Buffer[1] + Read_Buffer[2] + Read_Buffer[3] + Read_Buffer[4] + Read_Buffer[5] + Read_Buffer[6] + Read_Buffer[7] + Read_Buffer[8] + Read_Buffer[9] + Read_Buffer[10] + Read_Buffer[11] + Read_Buffer[12] + Read_Buffer[13] + Read_Buffer[14]);
                        if (Read_Buffer[13] == CS)
                        {
                            NO = (short)(Read_Buffer[3] << 8 | Read_Buffer[4]);
                            wd = (float)(((short)(Read_Buffer[5] << 8 | Read_Buffer[6])) * 0.1f);// BitConverter.ToInt16(temp_byte, 0) / 10f;
                            sd = (float)(((short)(Read_Buffer[7] << 8 | Read_Buffer[8])) * 0.1f);
                            dqy = (float)(((short)(Read_Buffer[9] << 8 | Read_Buffer[10])) * 0.01f);
                            yw = (float)(((short)(Read_Buffer[11] << 8 | Read_Buffer[12])) * 0.1f);
                            return true;
                        }
                        else
                            return false;
                    }
                    break;
                default:
                    return false;
                    break;
            }
        }
        /// <summary>
        /// 设置调零气体
        /// </summary>
        /// <param name="value">0-空气 1-氮气</param>
        /// <returns></returns>
        public bool setZeroGas(byte value)
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    byte[] Content_MQ = cmdSetZeroGas_MQW;
                    Content_MQ[3] = value;
                    Content_MQ[4] = getCS_MQ(Content_MQ, 4);
                    SendData(Content_MQ);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        /// <summary>
        /// 设置调零方式
        /// </summary>
        /// <param name="value">0-手动 1-自动</param>
        /// <returns></returns>
        public bool setZeroStyle(byte value)
        {
            ReadData();
            int i = 0;
            switch (yqxh)
            {
                case yq_mqw5101:
                    byte[] Content_MQ = cmdSetZeroStyle_MQW;
                    Content_MQ[3] = value;
                    Content_MQ[4] = getCS_MQ(Content_MQ, 4);
                    SendData(Content_MQ);
                    Thread.Sleep(30);
                    while (ComPort_3.BytesToRead < 4)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    if (Read_Buffer[0] == 0x06)
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                default:
                    return false;
                    break;
            }
        }
        #endregion

    }
}