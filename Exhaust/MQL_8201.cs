using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using ini;

namespace Exhaust
{
    public class yhrRealTimeData
    {
        public double CO { set; get; }
        public double CO2 { set; get; }
        public double HC { set; get; }
        public double NO { set; get; }
        public double PEF { set; get; }
        public double QLYL { set; get; }
        public double HJWD { set; get; }
        public double HJYL { set; get; }
        public double HJSD { set; get; }
        public double BZLL { set; get; }
        public double FBZLL { set; get; }
        public double XSYL { set; get; }
        public double XSWD { set; get; }
    }
    public class yhrRealTimeStatus
    {
        public int 预热状态位 { set; get; }
        public int 反吹状态位 { set; get; }
        public int 背景空气测定状态位 { set; get; }
        public int 环境空气测定状态位 { set; get; }
        public int 泄露检查状态位 { set; get; }
        public int HC残留测定状态位 { set; get; }
        public int 气体调零状态位 { set; get; }
        public int 气路低流量警告 { set; get; }
        public int 等待调零位 { set; get; }
        public int 调零方式位 { set; get; }
        public int 系统忙状态位 { set; get; }
        public int 泄露检查结果位 { set; get; }
        public int 碳氢残留测定结果位 { set; get; }
        public int 气体调零结果位 { set; get; }
        public int 燃料类型 { set; get; }
        public int 流量调零标志位 { set; get; }
        public int 流量调零结果位 { set; get; }
        public int 流量压力状态位 { set; get; }
        public int 流量温度状态位 { set; get; }
        public string 流量状态 { set; get; }
        public int CO2修正状态 { set; get; }
    }
    public class yhrRealTimeYh
    {
        public int CHECKTIME { set; get; }
        public double SSYH { set; get; }
        public double ZYH { set; get; }
    }
    public class yhrRealTimePfwsszl
    {
        public double SSCO { set; get; }
        public double SSCO2 { set; get; }
        public double SSHC { set; get; }
        public double SSNO { set; get; }
    }
    public class yhrRealTimePfzzl
    {
        public int ZTIME { set; get; }
        public double ZCO { set; get; }
        public double ZCO2 { set; get; }
        public double ZHC { set; get; }
        public double ZNO { set; get; }
    }
    public class yhrRealTimeTotalData
    {
        public double CO { set; get; }
        public double CO2 { set; get; }
        public double HC { set; get; }
        public double BZLL { set; get; }
        public double FBZLL { set; get; }
        public double XSYL { set; get; }
        public double XSWD { set; get; }
        public double SSCO { set; get; }
        public double SSCO2 { set; get; }
        public double SSHC { set; get; }
        public int CHECKTIME { set; get; }
        public double SSYH { set; get; }
        public double ZYH { set; get; }
    }
    public struct FL_Status01
    {
        public bool 预热;
        public bool 调零;
        public bool 校准;
        public bool 读背景气深度;
        public bool HC校准无效;
        public bool CO校准无效;
        public bool CO2校准无效;
        public bool NO校准无效;
    }
    public struct FL_Status02
    {
        public bool NDIR通道信号低;
        public bool NO信号低;
        public bool O2信号低;
    }
    public struct FL_Status04
    {
        public bool 取样泵开;
        public bool 电磁阀SV1关;
        public bool 电磁阀SV2关;
        public bool 电磁阀SV3关;
        public bool 电磁阀SV4关;
    }
    public struct FL_Status05
    {
        public bool 变频器正转运行中;
        public bool 变频器反转运行中;
        public bool 变频器待机中;
        public bool 变频器故障中;
        public bool 变频器通讯故障;
    }
    public struct fl_StatusAndData
    {
        public byte mode;
        public byte fuleSelect;
        public double fueldensity;
        public double rollerdiameter;
        public FL_Status01 st01;
        public FL_Status02 st02;
        public byte st03;
        public byte st04;
        public byte st05;
        public byte st06;
        public UInt32 dn;
        public double co2_ssnd;
        public double co_ssnd;
        public double hc_ssnd;
        public double no_ssnd;
        public double o2_ssnd;
        public double fbzll;
        public double bzll;
        public double cs;
        public double hjwd;
        public double xdsd;
        public double xsyl;
        public double xswd;
        public double xsxdsd;
        public double lljyc;//流量计压差
        public double df;
        public double hcf;
        public double rpm;
        public double co2_sszl;
        public double co_sszl;
        public double hc_sszl;
        public double no_sszl;
        public double yh_ssyh;
        public double distance_ssjl;
        public double fdjzs_ssyh;
        public double co2_ljzl;
        public double co_ljzl;
        public double hc_ljzl;
        public double no_ljzl;
        public double yh_ljyh;
        public double distance_ljjl;
        public double yh_100km;
        public double yh_hour;
        public double 流量检测压力;
        public double 气室补偿压力;
        public double 红外检测器温度;
        public double 低流量阈值;
        public double 单位时间C质量;
        public double 累计C质量;
        public double 气体密度;
        public double 累加测试时间;

    }
    public struct NH_standardData
    {
        public double hc_ssnd;
        public double co_ssnd;
        public double co2_ssnd;
        public double dqy;
        public double wd;
        public double fbzll;
        public double bzll;
        public double pef;
    }
    public struct NH_status
    {
        public bool 预热;
        public bool 风机启动;
        public bool 流量正常;
        public bool 正在测量;
        public bool 正在测量环境CO2;
    }
    public struct NH_status2
    {
        public bool 正在预热;
        public bool 等待检漏;
        public bool 正在检漏;
        public bool 检漏成功;
        public bool 检测失败;
        public bool 等待调零;
        public bool 正在调零;
        public bool 调零成功;
        public bool 调零失败;
        public bool 正在反吹;
        public bool 正在开泵;
        public bool 正在校准;
        public bool 校准成功;
        public bool 校准失败;
        public bool 废气仪待机;
    }
    public struct NH_fuleData
    {
        public int time;
        public double hc;
        public double co;
        public double co2;
        public double dqy;
        public double wd;
        public double fbzll;
        public double bzll;
        public double hczl;
        public double cozl;
        public double co2zl;
        public double ssyh;
        public double ljyh;
    }
    public class yhControl
    {
        private string yqxh = "mql_8201";
        private const string flxh = "fly_2000";
        private const string mqxh = "mql_8201";
        private const string nhxh = "nhty_1";
        private byte fl_rllx = 0x00;//福立油耗仪的燃料选择类型，默认为汽油

        byte[] NH_GetStandardData = { 0x01, 0x03, 0x12, 0x58, 0x00, 0x0a };
        byte[] NH_GetStatus = { 0x01, 0x03, 0x12, 0x5D, 0x00, 0x01 };
        byte[] NH_TurnOnMotor = { 0x01, 0x06, 0x12, 0x5E, 0x00, 0x01,0x00,0x00 };//后两个数据风机频率，单位为0.1Hz,如设定值为36.5Hz,则发送016D,小流量20Hz,中流量35Hz,大流量 50Hz,特大流量 60Hz
        byte[] NH_TurnOffMotor = { 0x01, 0x06, 0x12, 0x5E, 0x00, 0x00,0x00,0x00 };
        byte[] NH_SetNdzz = { 0x01, 0x06, 0x12, 0x60, 0x00, 0x00,0x00,0x00,0x00,0x00,0x00,0x00 };//data1:0000-取状态 0005-反吹 0006-停泵 0008-调零 0009-写校准HC,CO,CO2数据 000a-校准HC,CO,CO2 000f-调零气体 为空气 0010-调零气体为零气 0023-开泵
                                                                                                //data2:校准HC浓度，单位1ppm
                                                                                                //data3:校准CO浓度，单位为0.01%
                                                                                                //data4:校准CO2浓度，单位为0.01%
        byte[] NH_StartTest = { 0x01, 0x06, 0x12, 0x64, 0x00, 0x01,0x00,0x00,0x00,0x00 };//启动油耗检测
                                                                                           //data1:0x0001-启动检测 0x000-关闭检测
                                                                                           //data2:测量时间，单位1s，如设定时间为60s，则发送003c
                                                                                           //data3:汽车总质量，单位1kg，如设定总质量为13650kg，则发送3552
        byte[] NH_StopTest = { 0x01, 0x06, 0x12, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };//关闭油耗检测
                                                                                               //data1:0x0001-启动检测 0x000-关闭检测
                                                                                               //data2:测量时间，单位1s，如设定时间为60s，则发送003c
                                                                                               //data3:汽车总质量，单位1kg，如设定总质量为13650kg，则发送3552
        byte[] NH_GetFuelData = { 0x01, 0x03, 0x12, 0x80, 0x00, 0x0d,0x00,0x00 };//后两个数据默认传0x0000
        byte[] NH_SetDelayTime = { 0x01, 0x06, 0x12, 0x68, 0x00, 0x01,0x00,0x00 };//后两个数据延时时间，单位0.1s
        byte[] NH_GetDelayTime = { 0x01, 0x06, 0x12, 0x68, 0x00, 0x00,0x00,0x00 };
        byte[] NH_SetFuleType = { 0x01, 0x06, 0x12, 0x67, 0x00, 0x01,0x00,0x00 };//后两个数据为燃料种类，汽油0001，柴油0000
        byte[] NH_GetFuleType = { 0x01, 0x06, 0x12, 0x67, 0x00, 0x00, 0x00, 0x00 };
        byte[] NH_DemarcateQtyl = { 0x01, 0x06, 0x12, 0x6b, 0x00, 0x00 };//后两个数据为大气压，单位为0.01kPa
        byte[] NH_DemarcateWd = { 0x01, 0x06, 0x12, 0x6c, 0x00, 0x00 };//后两个数据为大气压，单位为0.1℃
        byte[] NH_DemarcateLLHigh = { 0x01, 0x06, 0x12, 0x69, 0x00, 0x00 };//后两个数据为流量高量程，单位为0.1L/s
        byte[] NH_DemarcateLLLow = { 0x01, 0x06, 0x12, 0x6a, 0x00, 0x00 };//后两个数据为流量低量程，单位为0.1L/s
        byte[] NH_StartEnvCO2Test = { 0x01, 0x06, 0x12, 0x70, 0x00, 0x01 };//启动CO2测量
        byte[] NH_StopEnvCO2Test = { 0x01, 0x06, 0x12, 0x70, 0x00, 0x00 };//关闭CO2测量
        byte[] NH_GetEnvCO2 = { 0x01, 0x06, 0x12, 0x71, 0x00, 0x00,0x00,0x00 };//读取环境CO2

        byte[] asciiTable = { 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46 };

        public yhControl(string xh)
        {
            yqxh = xh.ToLower();
            INIIO.startpath = AppDomain.CurrentDomain.BaseDirectory;
            INIIO.saveLogInf("xh=" + yqxh);
        }
        public yhControl()
        {
            INIIO.startpath = AppDomain.CurrentDomain.BaseDirectory;
            INIIO.saveLogInf("xh=" + yqxh);
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
        //private System.ComponentModel.IContainer components = new System.ComponentModel.Container();
        public System.IO.Ports.SerialPort ComPort_1;
        byte[] Send_Buffer;                                     //发送缓冲区
        byte[] Read_Buffer;                                     //读取缓冲区
        bool Read_Flag = false;                                 //是否有数据可以读取

        #region mql_8201通讯协议

        byte nak = 0x15;//命令错误，发送数据长度错误或者校验和错误

        byte[] CMDgetRealTimeData = { 0x07, 0x00,0x03, 0xf6 };
        byte[] CMDgetRealTimeStatus = { 0x07, 0x01,0x03, 0xF5 };
        byte[] CMDgetRealTimeYh = { 0x07, 0x02, 0x03, 0xf4};
        byte[] CMDgetRealTimePfwsszl = { 0x07, 0x03, 0x3, 0xF3 };
        byte[] CMDgetRealTimePfwzzl = { 0x07, 0x04, 0x03, 0xf2 };
        byte[] CMDgetRealTimeTotalData = { 0x07, 0x70, 0x03, 0x86};//后两个数据为标定的压力值，单位为0.01kPa,如校准值为101.33kPa,则发送2795
        byte[] CMDsetAirAsZeroStyle = { 0x07, 0x60, 0x04, 0x00,0x00 };
        byte[] CMDsetZeroAsZeroStyle = { 0x07, 0x60, 0x04, 0x01 ,0x00};
        byte[] CMDsetQyAsRl = { 0x07, 0x64, 0x04, 0x00,0x00 };
        byte[] CMDsetCyAsRl = { 0x07, 0x64, 0x04, 0x01,0x00 };
        byte[] CMDzeroEquip = { 0x07, 0x23, 0x03, 0xd3 };
        byte[] CMDtestEnviroAir = { 0x07, 0x27, 0x03, 0xcf };
        byte[] CMDamendCO2 = { 0x07, 0x31, 0x04, 0x00,0x00 };
        byte[] CMDselectLowFlow = { 0x07, 0x0a, 0x03, 0xec };
        byte[] CMDselectMidFlow = { 0x07, 0x0b, 0x03, 0xeb };
        byte[] CMDselectHighFlow = { 0x07, 0x0c, 0x03, 0xea };
        byte[] CMDpumpAir = { 0x07, 0x10, 0x03, 0xe6 };
        byte[] CMDflowBack = { 0x07, 0x11, 0x03, 0xe5 };
        byte[] CMDstartTest = { 0x07, 0x20, 0x05, 0x00, 0x00,0x00 };
        byte[] CMDstopTest = { 0x07, 0x21, 0x03, 0xd5 };
        byte[] CMDstopAction = { 0x07, 0x16, 0x03, 0xe0 };
        byte[] CMDstartFlow = { 0x07, 0x06, 0x03, 0xf0 };
        byte[] CMDstopFlow = { 0x07, 0x07, 0x03, 0xef };
        byte[] CMDexitPrep = { 0x07, 0x09, 0x03, 0xed };

        #endregion
        #region fly_2000通讯协议

        //byte nak = 0x15;//命令错误，发送数据长度错误或者校验和错误

        byte[] CMD_FL_READSTATUSANDDATA = { 0x09, 0x01, 0x01,
            0x00,
            0x00,
            0x00,0x00,
            0x00,0x00,
            0x00 };
        byte[] CMD_FL_ZEROO2AIRSPAN = { 0x03, 0x01, 0x03, 0xF8 };
        byte[] CMD_FL_SPANNDIRBENCH = { 0x0f, 0x01, 0x04,
            0x00,
            0x00,
            0x00,0x00,
            0x00,0x00,
            0x00,0x00,
            0x00,0x00,
            0x00,0x00,
            0x00};
        byte[] CMD_FL_READPEF = { 0x03, 0x01, 0x5, 0xF6 };
        byte[] CMD_FL_READNDIRBENCHSPAN = { 0x03, 0x01, 0x6, 0xF5 };
        byte[] CMD_FL_RESETNDIRBENCHSPAN = { 0x05, 0x01, 0x07,
            0x00,
            0x00,
            0x00};//后两个数据为标定的压力值，单位为0.01kPa,如校准值为101.33kPa,则发送2795
        byte[] CMD_FL_PNEUMATICCONTROL = { 0x04, 0x01, 0x08, 0x00, 0x00 };
        byte[] CMD_FL_READNVRAM = { 0x07, 0x60, 0x04, 0x01, 0x00 };
        byte[] CMD_FL_SETFUELCONSUMESPAN = { 0x05, 0x01, 0x0a,
            0x00,
            0x00,
            0x00};
        byte[] CMD_FL_READFUELCONSUMESPAN ={ 0x03, 0x01, 0x0b,
            0x00};
        byte[] CMD_FL_RESETFUELCONSUMESPAN = { 0x03, 0x01, 0x0c,
            0x00};
        byte[] CMD_FL_READBACKGOURDGAS = { 0x04, 0x01, 0x0d,
            0x00,
            0x00};
        byte[] CMD_FL_RESETSYSTEM = { 0x03, 0x01, 0x0e,
            0xed};
        byte[] CMD_FL_READSTATUSDATA2 = { 0x09, 0x01, 0x11,
            0x00,
            0x00,
            0x00,0x00,
            0x00,0x00,
            0x00 };
        byte[] CMD_FL_AMBIENTPARAMETERCALIBRATION = { 0x0a, 0x01, 0x12,
            0x00,
            0x00,0x00,
            0x00,0x00,
            0x00,0x00,
            0x00 };
        byte[] CMD_FL_NACK = { 0x0f, 0x0f};
        byte[] CMD_FL_TURNONFAN = { 0X01, 0X06, 0X10, 0X00, 0X00, 0X01, 0X4C, 0XCA };
        byte[] CMD_FL_TURNOFFFAN = { 0X01, 0X06, 0X10, 0X00, 0X00, 0X05, 0X4D, 0X09 };

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
        public void SendData(byte[] Content)
        {
            try
            {
                ComPort_1.Write(Content, 0, Content.Length);
                ini.INIIO.saveLogInf("[油耗仪发送]:" + byteToHexStr(Content));
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
        #region 福立的校验方式
        private byte getCS_FL(byte[] content, int count)
        {
            byte CS = 0;
            for (int i = 0; i < count; i++)
                CS += content[i];
            CS = (byte)(~CS);
            return CS;
        }
        #endregion
        #region 发送数据-福立
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="Cmd">命令</param>
        /// <param name="Content">内容</param>
        public void SendDataFL(byte[] Content)
        {
            try
            {
                byte[] framehead = { 0x4e, 0x54, 0x30, 0x32, 0x53 };
                ComPort_1.Write(framehead, 0, framehead.Length);
                ComPort_1.Write(Content, 0, Content.Length);
                ini.INIIO.saveLogInf("[油耗仪发送]:" + byteToHexStr(Content));
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region 发送数据-南华
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
                    arraySend[i * 2 + 1] = asciiTable[CS[i] / 16];
                    arraySend[i * 2 + 2] = asciiTable[CS[i] % 16];
                }
                ComPort_1.Write(arraySend, 0, arraySend.Length);
                ini.INIIO.saveLogInf("[油耗仪发送]:" + byteToHexStr(arraySend));
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
            Read_Buffer = new byte[200];
            Read_Flag = false;
            int count=ComPort_1.Read(Read_Buffer, 0, ComPort_1.BytesToRead);
            if(count>0)
            {
                ini.INIIO.saveLogInf("[油耗仪接收]:" + byteToHexStr(Read_Buffer.ToList(),0,count));
            }
        }
        #endregion
        #region 设置南华延迟时间

        public bool setNHDelayTime(int delaytime)
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    return true;break;
                case flxh: return true; break;
                case nhxh:
                    byte[] NH_data = NH_SetDelayTime;
                    NH_data[6] = (byte)((ushort)delaytime >> 8);
                    NH_data[7] = (byte)((ushort)delaytime);
                    SendDataOfNHF(NH_data);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 21)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return false;
                    else
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
                    break;
            }

        }
        #endregion
        #region 设置零气调零

        public bool setZeroAsZero()
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    byte CS = getCS_MQ(CMDsetZeroAsZeroStyle, 4);
                    CMDsetZeroAsZeroStyle[4] = CS;
                    SendData(CMDsetZeroAsZeroStyle);
                    Thread.Sleep(30);
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
                    break;

                case flxh: return true; break;
                case nhxh:
                    byte[] NH_data = NH_SetNdzz;
                    NH_data[5] = 0x10;
                    SendDataOfNHF(NH_data);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return false;
                    else
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
                    break;
            }

        }
        #endregion

        #region 设置空气调零

        public bool setAirAsZero()
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    byte CS = getCS_MQ(CMDsetAirAsZeroStyle, 4);
                    CMDsetAirAsZeroStyle[4] = CS;
                    SendData(CMDsetAirAsZeroStyle);
                    Thread.Sleep(30);
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
                    break;

                case flxh: return true; break;
                case nhxh:
                    byte[] NH_data = NH_SetNdzz;
                    NH_data[5] = 0x0f;
                    SendDataOfNHF(NH_data);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return false;
                    else
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
                    break;
            }

        }
        #endregion
        #region 调零

        public bool zeroEquip()
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    SendData(CMDzeroEquip);
                    Thread.Sleep(30);
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
                    break;
                case flxh:
                    SendDataFL(CMD_FL_ZEROO2AIRSPAN);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 9)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[7] == 0x03)
                        return true;
                    else
                        return false;
                    break;
                case nhxh:
                    byte[] NH_data = NH_SetNdzz;
                    NH_data[5] = 0x08;
                    SendDataOfNHF(NH_data);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return false;
                    else
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
                    break;
            }

        }
        #endregion
        #region 环境空气测定

        public bool testEnviroAir()
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    SendData(CMDtestEnviroAir);
                    Thread.Sleep(30);
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
                    break;
                case nhxh:
                    byte[] NH_data = NH_StartEnvCO2Test;
                    SendDataOfNHF(NH_data);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return false;
                    else
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
                    break;
            }

        }
        #endregion
        #region 二氧化碳修正

        public bool amendCO2()
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    CMDamendCO2[4] = getCS_MQ(CMDamendCO2, 4);
                    SendData(CMDamendCO2);
                    Thread.Sleep(30);
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
                    break;
                case flxh:
                    CMD_FL_READBACKGOURDGAS[3] = 0x01;
                    CMD_FL_READBACKGOURDGAS[4] = getCS_FL(CMD_FL_READBACKGOURDGAS, 4);
                    SendDataFL(CMD_FL_READBACKGOURDGAS);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 15)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[7] == 0x0d)
                        return true;
                    else
                        return false;
                    break;
                case nhxh:return true;//南华不提供该命令
                default:
                    return false;
                    break;
            }

        }
        #endregion
        #region 选择汽油为燃料

        public bool selectQy()
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    byte CS = getCS_MQ(CMDsetQyAsRl, 4);
                    CMDsetQyAsRl[4] = CS;
                    SendData(CMDsetQyAsRl);
                    Thread.Sleep(30);
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
                    break;
                case flxh:
                    fl_rllx = 0x00;
                    return true;
                    break;
                case nhxh:
                    byte[] NH_data = NH_SetFuleType;
                    NH_data[7] = 0x01;
                    SendDataOfNHF(NH_data);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 21)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return false;
                    else
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
                    break;
            }

        }
        #endregion
        #region 选择柴油为燃料

        public bool selectCy()
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    byte CS = getCS_MQ(CMDsetCyAsRl, 4);
                    CMDsetCyAsRl[4] = CS;
                    SendData(CMDsetCyAsRl);
                    Thread.Sleep(30);
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
                    break;

                case flxh:
                    fl_rllx = 0x01;
                    return true;
                    break;
                case nhxh:
                    byte[] NH_data = NH_SetFuleType;
                    NH_data[7] = 0x00;
                    SendDataOfNHF(NH_data);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 21)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return false;
                    else
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
                    break;
            }

        }
        #endregion
        private ushort nh_flow = 350;//南华风机流量，小流量：200，中流量：350，大流量：500，特大流量：600
        #region 选择低流量档

        public bool selectLowFlow()
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    SendData(CMDselectLowFlow);
                    Thread.Sleep(30);
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
                    break;
                case flxh:
                    return true;
                    break;
                case nhxh:
                    nh_flow = 200;
                    return true;
                default:
                    return false;
                    break;
            }

        }
        #endregion
        #region 选择中流量档

        public bool selectMidFlow()
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    SendData(CMDselectMidFlow);
                    Thread.Sleep(30);
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
                    break;

                case flxh:
                    return true;
                    break;
                case nhxh:
                    nh_flow = 350;
                    return true;
                default:
                    return false;
                    break;
            }

        }
        #endregion
        #region 选择高流量档

        public bool selectHighFlow()
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    SendData(CMDselectHighFlow);
                    Thread.Sleep(30);
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
                    break;

                case flxh:
                    return true;
                    break;
                case nhxh:
                    nh_flow = 500;
                    return true;
                default:
                    return false;
                    break;
            }

        }
        #endregion
        #region 选择特大流量档

        public bool selectExtraHighFlow()
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    SendData(CMDselectHighFlow);
                    Thread.Sleep(30);
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
                    break;

                case flxh:
                    return true;
                    break;
                case nhxh:
                    nh_flow = 600;
                    return true;
                default:
                    return false;
                    break;
            }

        }
    #endregion
        #region 风机启动

        public bool startFlow()
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    SendData(CMDstartFlow);
                    Thread.Sleep(30);
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
                    break;

                case flxh:
                    SendData(CMD_FL_TURNONFAN);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead <8)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[1] == 0x06)
                        return true;
                    else
                        return false;
                    break;
                case nhxh:
                    byte[] NH_data = NH_TurnOnMotor;
                    NH_data[6] = (byte)(nh_flow>>8);
                    NH_data[7] = (byte)(nh_flow);
                    SendDataOfNHF(NH_data);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 21)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return false;
                    else
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
                    break;
            }

        }
        #endregion
        #region 退出预热

        public bool exitPrepare()
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    SendData(CMDexitPrep);
                    Thread.Sleep(30);
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
                    break;

                case flxh: return true; break;
                case nhxh:return true;break;
                default:
                    return false;
                    break;
            }

        }
        #endregion
        #region 开泵抽气

        public bool pumpAir()
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    SendData(CMDpumpAir);
                    Thread.Sleep(30);
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
                    break;

                case flxh:
                    CMD_FL_PNEUMATICCONTROL[3] = 0x01;//投入稀释后气体 
                    CMD_FL_PNEUMATICCONTROL[4] = getCS_FL(CMD_FL_PNEUMATICCONTROL, 4);
                    SendDataFL(CMD_FL_PNEUMATICCONTROL);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 10)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[7] == 0x08)
                        return true;
                    else
                        return false;
                    break;

                case nhxh:
                    byte[] NH_data = NH_SetNdzz;
                    NH_data[5] = 0x23;
                    SendDataOfNHF(NH_data);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return false;
                    else
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
                    break;
            }

        }
        #endregion
        #region 开始测量
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">设定时间</param>
        /// <returns></returns>
        public bool startTest(int time,int zzl)
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    CMDstartTest[3] = (byte)(time/256);
                    CMDstartTest[4] = (byte)(time %256);
                    byte CS = getCS_MQ(CMDstartTest, 5);
                    CMDstartTest[5] = CS;
                    SendData(CMDstartTest);
                    Thread.Sleep(30);
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
                    break;
                case flxh:
                    fl_StatusAndData fldata = new fl_StatusAndData();
                    if (getFlRealTimeDataUnAdd(0, out fldata))//发一次非累计取数让累计数据清零
                    {
                        return true;
                    }
                    else
                        return false;
                    break;

                case nhxh:
                    byte[] NH_data = NH_StartTest;
                    NH_data[6] = (byte)(time>>8);
                    NH_data[7] = (byte)(time);
                    NH_data[8] = (byte)(zzl >> 8);
                    NH_data[9] = (byte)(zzl);
                    SendDataOfNHF(NH_data);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return false;
                    else
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
                    break;
            }

        }
        #endregion
        #region 开始测量
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">设定时间</param>
        /// <returns></returns>
        public bool stopTest()
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    SendData(CMDstopTest);
                    Thread.Sleep(30);
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
                    break;

                case flxh:
                    fl_StatusAndData fldata = new fl_StatusAndData();
                    if (getFlRealTimeDataUnAdd(0, out fldata))//发一次非累计取数让累计数据清零
                    {
                        return true;
                    }
                    else
                        return false;
                    break;
                case nhxh:
                    byte[] NH_data = NH_StopTest;
                    SendDataOfNHF(NH_data);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return false;
                    else
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
                    break;
            }

        }
        #endregion
        #region 反吹

        public bool flowBack()
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    SendData(CMDflowBack);
                    Thread.Sleep(30);
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
                    break;
                case flxh:
                    CMD_FL_PNEUMATICCONTROL[3] = 0x01;//福立通过抽入稀释后气体来进行清洗管路 
                    CMD_FL_PNEUMATICCONTROL[4] = getCS_FL(CMD_FL_PNEUMATICCONTROL, 4);
                    SendDataFL(CMD_FL_PNEUMATICCONTROL);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 10)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[7] == 0x08)
                        return true;
                    else
                        return false;
                    break;

                case nhxh:
                    byte[] NH_data = NH_SetNdzz;
                    NH_data[5] = 0x05;
                    SendDataOfNHF(NH_data);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return false;
                    else
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
                    break;
            }

        }
        #endregion
        #region 停止动作

        public bool stopAction()
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    SendData(CMDstopAction);
                    Thread.Sleep(30);
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
                    break;
                case flxh:
                    CMD_FL_PNEUMATICCONTROL[3] = 0x00;//投入稀释后气体 
                    CMD_FL_PNEUMATICCONTROL[4] = getCS_FL(CMD_FL_PNEUMATICCONTROL, 4);
                    SendDataFL(CMD_FL_PNEUMATICCONTROL);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 10)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[7] == 0x08)
                        return true;
                    else
                        return false;
                    break;

                case nhxh:
                    byte[] NH_data = NH_SetNdzz;
                    NH_data[5] = 0x06;
                    SendDataOfNHF(NH_data);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return false;
                    else
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
                    break;
            }

        }
        #endregion
        #region 关闭风机

        public bool stopFlow()
        {
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    SendData(CMDstopFlow);
                    Thread.Sleep(30);
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
                    break;


                case flxh:
                    SendData(CMD_FL_TURNOFFFAN);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 8)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[1] == 0x06)
                        return true;
                    else
                        return false;
                    break;

                case nhxh:
                    byte[] NH_data = NH_TurnOffMotor;
                    SendDataOfNHF(NH_data);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 21)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return false;
                    else
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
                    break;
            }

        }
        #endregion
        #region 取标准状态所有数据值
        /// <summary>
        /// 取标准状态所有数据值
        /// </summary>
        /// <returns>string</returns>
        public bool GetNHstandardDat(out NH_standardData data)
        {
            data = new NH_standardData();

            ReadData();
            int i = 0;
            byte CS = 0;
            switch (yqxh)
            {
                case flxh:return true;break;
                case mqxh:return true;break;
                case nhxh:
                    SendDataOfNHF(NH_GetStandardData);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 45)                          //协议上有分歧，有PEF值返回时为44个字节，没有PEF返回时为39个字节
                    {
                        i++;
                        Thread.Sleep(20);
                        if (i == 250)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return false;
                    else
                    {
                        try
                        {
                            string datastring = Encoding.Default.GetString(Read_Buffer, 7, 4);
                            data.hc_ssnd = Convert.ToInt16(datastring, 16);
                            datastring = Encoding.Default.GetString(Read_Buffer, 11, 4);
                            data.co_ssnd = Convert.ToInt16(datastring, 16) * 0.01f;
                            datastring = Encoding.Default.GetString(Read_Buffer, 15, 4);
                            data.co2_ssnd = Convert.ToInt16(datastring, 16) * 0.01f;
                            datastring = Encoding.Default.GetString(Read_Buffer, 19, 4);
                            data.dqy = Convert.ToInt16(datastring, 16) *0.01;
                            datastring = Encoding.Default.GetString(Read_Buffer, 23, 4);
                            data.wd = Convert.ToInt16(datastring, 16) * 0.1;
                            datastring = Encoding.Default.GetString(Read_Buffer, 27, 4);
                            data.bzll = Convert.ToInt16(datastring, 16) * 0.1;
                            datastring = Encoding.Default.GetString(Read_Buffer, 31, 4);
                            data.fbzll = Convert.ToInt16(datastring, 16) * 0.1;
                            datastring = Encoding.Default.GetString(Read_Buffer, 35, 4);//暂时不取PEF值
                            data.pef = Convert.ToInt16(datastring, 16) * 0.001;
                            return true;//HC
                        }
                        catch
                        {
                            return false;

                        }
                    }
                    break;
                default:
                    return false;
                    break;
            }

            //return "仪器通讯失败";

        }
        #endregion
        #region 取南华油耗值
        /// <summary>
        /// 取标准状态所有数据值
        /// </summary>
        /// <returns>string</returns>
        public bool GetNHYhyDat(out NH_fuleData data)
        {
            data = new NH_fuleData();

            ReadData();
            int i = 0;
            byte CS = 0;
            switch (yqxh)
            {
                case flxh: return true; break;
                case mqxh: return true; break;
                case nhxh:
                    SendDataOfNHF(NH_GetFuelData);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 63)                          //协议上有分歧，有PEF值返回时为44个字节，没有PEF返回时为39个字节
                    {
                        i++;
                        Thread.Sleep(20);
                        if (i == 200)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[3] == 0x38)
                        return false;
                    else
                    {
                        try
                        {
                            string datastring = Encoding.Default.GetString(Read_Buffer, 7, 4);
                            data.time = Convert.ToInt16(datastring, 16);
                            datastring = Encoding.Default.GetString(Read_Buffer, 11, 4);
                            data.hc = Convert.ToInt16(datastring, 16);
                            datastring = Encoding.Default.GetString(Read_Buffer, 15, 4);
                            data.co = Convert.ToInt16(datastring, 16) * 0.01f;
                            datastring = Encoding.Default.GetString(Read_Buffer, 19, 4);
                            data.co2 = Convert.ToInt16(datastring, 16) * 0.01;
                            datastring = Encoding.Default.GetString(Read_Buffer, 23, 4);
                            data.dqy = Convert.ToInt16(datastring, 16) * 0.1;
                            datastring = Encoding.Default.GetString(Read_Buffer, 27, 4);
                            data.wd = Convert.ToInt16(datastring, 16)-30;
                            datastring = Encoding.Default.GetString(Read_Buffer, 31, 4);
                            data.fbzll = Convert.ToInt16(datastring, 16);
                            datastring = Encoding.Default.GetString(Read_Buffer, 35, 4);
                            data.bzll = Convert.ToInt16(datastring, 16);
                            datastring = Encoding.Default.GetString(Read_Buffer, 39, 4);
                            data.hczl = Convert.ToInt16(datastring, 16);
                            datastring = Encoding.Default.GetString(Read_Buffer, 43, 4);
                            data.cozl = Convert.ToInt16(datastring, 16);
                            datastring = Encoding.Default.GetString(Read_Buffer, 47, 4);
                            data.co2zl = Convert.ToInt16(datastring, 16);
                            datastring = Encoding.Default.GetString(Read_Buffer, 51, 4);
                            data.ssyh = Convert.ToInt16(datastring, 16)*0.01;
                            datastring = Encoding.Default.GetString(Read_Buffer, 55, 4);
                            data.ljyh = Convert.ToInt16(datastring, 16)*0.1;
                            //datastring = Encoding.Default.GetString(Read_Buffer, 35, 4);//暂时不取PEF值
                            //data.pef = Convert.ToInt16(datastring, 16) * 0.001;
                            return true;//HC
                        }
                        catch
                        {
                            return false;

                        }
                    }
                    break;
                default:
                    return false;
                    break;
            }

            //return "仪器通讯失败";

        }
        #endregion
        public bool getNhStatus(out NH_status data)
        {
            data = new NH_status();
            byte Status = 0;
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case mqxh:return true;break;
                case flxh:return true;break;
                case nhxh:
                    SendDataOfNHF(NH_GetStatus);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 15)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(2);
                        if (i == 100)
                            return false;
                    }
                    ReadData(); 
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
                        return false;
                    }
                    data.预热 = ((Status & 0x01) == 0x01);
                    data.风机启动 = ((Status & 0x02) == 0x02);
                    data.流量正常 = ((Status & 0x04) == 0x04);
                    data.正在测量 = ((Status & 0x08) == 0x08);
                    data.正在测量环境CO2 = ((Status & 0x10) == 0x10);
                    return true;
                    break;
                default:
                    return true;
                    break;
            }
        }
        #region 福立取数指令 
        /// <summary>
        /// 福立取数指令
        /// </summary>
        /// <param name="mode">0:不累加 1：累加</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool getFlRealTimeDataUnAdd(byte mode,out fl_StatusAndData data)
        {
            data = new fl_StatusAndData();
            try
            {
                byte Status = 0;
                ReadData();
                int i = 0;
                string msg = "";
                if (!ComPort_1.IsOpen)      //串口出错
                    return false;
                CMD_FL_READSTATUSANDDATA[3] = mode;//不累加
                CMD_FL_READSTATUSANDDATA[4] = fl_rllx;//燃料类型
                if (fl_rllx == 0)//汽油密度0.750
                {
                    CMD_FL_READSTATUSANDDATA[5] = 0x02;
                    CMD_FL_READSTATUSANDDATA[6] = 0xe4;
                }
                else//柴油密度0.838
                {
                    CMD_FL_READSTATUSANDDATA[5] = 0x03;
                    CMD_FL_READSTATUSANDDATA[6] = 0x46;
                }
                CMD_FL_READSTATUSANDDATA[7] = 0x08;
                CMD_FL_READSTATUSANDDATA[8] = 0x84;
                CMD_FL_READSTATUSANDDATA[9] = getCS_FL(CMD_FL_READSTATUSANDDATA, 9);
                SendDataFL(CMD_FL_READSTATUSANDDATA);
                Thread.Sleep(30);
                while (ComPort_1.BytesToRead < 113)                          //等待仪器返回
                {
                    i++;
                    Thread.Sleep(10);
                    if (i == 100)
                        return false;
                }
                ReadData();                     //读取返回的数据
                if (Read_Buffer[7] == 0x01)
                {
                    data.mode = Read_Buffer[8];
                    data.fuleSelect = Read_Buffer[9];
                    data.fueldensity = (double)((ushort)((Read_Buffer[10] << 8) | Read_Buffer[11]) * 0.001);
                    data.rollerdiameter = (double)((ushort)((Read_Buffer[12] << 8) | Read_Buffer[13]) * 0.1);
                    data.st01 = new FL_Status01();
                    data.st02 = new FL_Status02();
                    data.st01.预热 = ((Read_Buffer[14] & 0x01) == 0x01);
                    data.st01.调零 = ((Read_Buffer[14] & 0x02) == 0x02);
                    data.st01.校准 = ((Read_Buffer[14] & 0x04) == 0x04);
                    data.st01.读背景气深度 = ((Read_Buffer[14] & 0x08) == 0x08);
                    data.st01.HC校准无效 = ((Read_Buffer[14] & 0x10) == 0x10);
                    data.st01.CO校准无效 = ((Read_Buffer[14] & 0x20) == 0x20);
                    data.st01.CO2校准无效 = ((Read_Buffer[14] & 0x40) == 0x40);
                    data.st01.NO校准无效 = ((Read_Buffer[14] & 0x80) == 0x80);

                    data.st02.NDIR通道信号低 = ((Read_Buffer[15] & 0x01) == 0x01);
                    data.st02.NO信号低 = ((Read_Buffer[15] & 0x02) == 0x02);
                    data.st02.O2信号低 = ((Read_Buffer[15] & 0x04) == 0x04);

                    data.st03 = Read_Buffer[16];
                    data.st04 = Read_Buffer[17];
                    data.st05 = Read_Buffer[18];
                    data.st06 = Read_Buffer[19];

                    data.dn = (UInt32)(Read_Buffer[20] << 24 | Read_Buffer[21] << 16 | Read_Buffer[22] << 8 | Read_Buffer[23]);

                    data.co2_ssnd = (double)((Int16)((Read_Buffer[24] << 8) | Read_Buffer[25]) * 0.01);
                    data.co_ssnd = (double)((Int16)((Read_Buffer[26] << 8) | Read_Buffer[27]) * 0.001);
                    data.hc_ssnd = (double)((Int16)((Read_Buffer[28] << 8) | Read_Buffer[29]));
                    data.no_ssnd = (double)((Int16)((Read_Buffer[30] << 8) | Read_Buffer[31]));
                    data.o2_ssnd = (double)((Int16)((Read_Buffer[32] << 8) | Read_Buffer[33]) * 0.01);

                    data.fbzll = (double)((Int16)((Read_Buffer[34] << 8) | Read_Buffer[35]) * 0.001);
                    data.bzll = (double)((Int16)((Read_Buffer[36] << 8) | Read_Buffer[37]) * 0.001);
                    data.cs = (double)((Int16)((Read_Buffer[38] << 8) | Read_Buffer[39]) * 0.1);
                    data.hjwd = (double)((Int16)((Read_Buffer[40] << 8) | Read_Buffer[41]) * 0.01);
                    data.xdsd = (double)((Int16)((Read_Buffer[42] << 8) | Read_Buffer[43]) * 0.1);
                    data.xsyl = (double)((Int16)((Read_Buffer[44] << 8) | Read_Buffer[45]) * 0.01);
                    data.xswd = (double)((Int16)((Read_Buffer[46] << 8) | Read_Buffer[47]) * 0.01);
                    data.xsxdsd = (double)((Int16)((Read_Buffer[48] << 8) | Read_Buffer[49]) * 0.01);
                    data.lljyc = (double)((Int16)((Read_Buffer[50] << 8) | Read_Buffer[51]) * 0.001);

                    data.df = (double)((ushort)((Read_Buffer[52] << 8) | Read_Buffer[53]) * 0.01);
                    data.hcf = (double)((ushort)((Read_Buffer[54] << 8) | Read_Buffer[55]) * 0.001);
                    data.rpm = (double)((ushort)((Read_Buffer[56] << 8) | Read_Buffer[57]) * 1);
                    data.co2_sszl = (double)((ushort)((Read_Buffer[58] << 8) | Read_Buffer[59]) * 0.01);
                    data.co_sszl = (double)((ushort)((Read_Buffer[60] << 8) | Read_Buffer[61]) * 0.001);
                    data.hc_sszl = (double)((ushort)((Read_Buffer[62] << 8) | Read_Buffer[63]) * 0.001);
                    data.no_sszl = (double)((ushort)((Read_Buffer[64] << 8) | Read_Buffer[65]) * 0.001);
                    data.yh_ssyh = (double)((ushort)((Read_Buffer[66] << 8) | Read_Buffer[67]) * 0.001);
                    data.distance_ssjl = (double)((ushort)((Read_Buffer[68] << 8) | Read_Buffer[69]) * 0.0001);
                    data.fdjzs_ssyh = (double)((ushort)((Read_Buffer[70] << 8) | Read_Buffer[71]) * 1);

                    data.co2_ljzl = (double)(0.01 * (UInt32)(Read_Buffer[72] << 24 | Read_Buffer[73] << 16 | Read_Buffer[74] << 8 | Read_Buffer[75]));
                    data.co_ljzl = (double)(0.001 * (UInt32)(Read_Buffer[76] << 24 | Read_Buffer[77] << 16 | Read_Buffer[78] << 8 | Read_Buffer[79]));
                    data.hc_ljzl = (double)(0.001 * (UInt32)(Read_Buffer[80] << 24 | Read_Buffer[81] << 16 | Read_Buffer[82] << 8 | Read_Buffer[83]));
                    data.no_ljzl = (double)(0.001 * (UInt32)(Read_Buffer[84] << 24 | Read_Buffer[85] << 16 | Read_Buffer[86] << 8 | Read_Buffer[87]));

                    data.yh_ljyh = (double)((ushort)((Read_Buffer[88] << 8) | Read_Buffer[89]) * 0.1);//注意，此处单位和协议不一样，协议单位为L,这里改为mL，为了和鸣泉统一
                    data.distance_ljjl = (double)((ushort)((Read_Buffer[90] << 8) | Read_Buffer[91]) * 0.001);
                    data.yh_100km = (double)((ushort)((Read_Buffer[92] << 8) | Read_Buffer[93]) * 0.01);
                    data.yh_hour = (double)((ushort)((Read_Buffer[94] << 8) | Read_Buffer[95]) * 0.01);

                    data.流量检测压力 = (double)((Int16)((Read_Buffer[96] << 8) | Read_Buffer[97]) * 0.01);
                    data.气室补偿压力 = (double)((Int16)((Read_Buffer[98] << 8) | Read_Buffer[99]) * 0.01);
                    data.红外检测器温度 = (double)((Int16)((Read_Buffer[100] << 8) | Read_Buffer[101]) * 0.01);
                    data.低流量阈值 = (double)((Int16)((Read_Buffer[102] << 8) | Read_Buffer[103]) * 1);

                    data.单位时间C质量 = (double)((ushort)((Read_Buffer[104] << 8) | Read_Buffer[105]) * 0.001);
                    data.累计C质量 = (double)((ushort)((Read_Buffer[106] << 8) | Read_Buffer[107]) * 0.1);
                    data.气体密度 = (double)((ushort)((Read_Buffer[108] << 8) | Read_Buffer[109]) * 0.001);
                    data.累加测试时间 = (double)((ushort)((Read_Buffer[110] << 8) | Read_Buffer[111]) * 0.01);

                    return true;
                }
                else
                    return false;
            }
            catch(Exception er)
            {
                throw new Exception(er.Message);
            }
        }
        #endregion
    
    
        public double flljyh = 0;
        public double flljsj = 0;
        #region 福立取数指令 
        /// <summary>
        /// 福立取数指令
        /// </summary>
        /// <param name="mode">0:不累加 1：累加</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool getFlRealTimeDataMoni(byte mode, out fl_StatusAndData data)
        {
            data = new fl_StatusAndData();
            try
            {
                byte Status = 0;
                ReadData();
                int i = 0;
                string msg = "";
                if (!ComPort_1.IsOpen)      //串口出错
                    return false;
                CMD_FL_READSTATUSANDDATA[3] = mode;//不累加
                if(mode==0)
                {
                    flljyh = 0;
                    flljsj = 0;
                }
                CMD_FL_READSTATUSANDDATA[4] = fl_rllx;//燃料类型
                if (fl_rllx == 0)//汽油密度0.750
                {
                    CMD_FL_READSTATUSANDDATA[5] = 0x02;
                    CMD_FL_READSTATUSANDDATA[6] = 0xe4;
                }
                else//柴油密度0.838
                {
                    CMD_FL_READSTATUSANDDATA[5] = 0x03;
                    CMD_FL_READSTATUSANDDATA[6] = 0x46;
                }
                CMD_FL_READSTATUSANDDATA[7] = 0x08;
                CMD_FL_READSTATUSANDDATA[8] = 0x84;
                CMD_FL_READSTATUSANDDATA[9] = getCS_FL(CMD_FL_READSTATUSANDDATA, 9);
                SendDataFL(CMD_FL_READSTATUSANDDATA);
                Thread.Sleep(30);
                while (ComPort_1.BytesToRead < 113)                          //等待仪器返回
                {
                    i++;
                    Thread.Sleep(10);
                    if (i == 100)
                        return false;
                }
                ReadData();                     //读取返回的数据
                if (Read_Buffer[7] == 0x01)
                {
                    data.mode = Read_Buffer[8];
                    data.fuleSelect = Read_Buffer[9];
                    data.fueldensity = (double)((ushort)((Read_Buffer[10] << 8) | Read_Buffer[11]) * 0.001);
                    data.rollerdiameter = (double)((ushort)((Read_Buffer[12] << 8) | Read_Buffer[13]) * 0.1);
                    data.st01 = new FL_Status01();
                    data.st02 = new FL_Status02();
                    data.st01.预热 = ((Read_Buffer[14] & 0x01) == 0x01);
                    data.st01.调零 = ((Read_Buffer[14] & 0x02) == 0x02);
                    data.st01.校准 = ((Read_Buffer[14] & 0x04) == 0x04);
                    data.st01.读背景气深度 = ((Read_Buffer[14] & 0x08) == 0x08);
                    data.st01.HC校准无效 = ((Read_Buffer[14] & 0x10) == 0x10);
                    data.st01.CO校准无效 = ((Read_Buffer[14] & 0x20) == 0x20);
                    data.st01.CO2校准无效 = ((Read_Buffer[14] & 0x40) == 0x40);
                    data.st01.NO校准无效 = ((Read_Buffer[14] & 0x80) == 0x80);

                    data.st02.NDIR通道信号低 = ((Read_Buffer[15] & 0x01) == 0x01);
                    data.st02.NO信号低 = ((Read_Buffer[15] & 0x02) == 0x02);
                    data.st02.O2信号低 = ((Read_Buffer[15] & 0x04) == 0x04);

                    data.st03 = Read_Buffer[16];
                    data.st04 = Read_Buffer[17];
                    data.st05 = Read_Buffer[18];
                    data.st06 = Read_Buffer[19];

                    data.dn = (UInt32)(Read_Buffer[20] << 24 | Read_Buffer[21] << 16 | Read_Buffer[22] << 8 | Read_Buffer[23]);

                    data.co2_ssnd = (double)((Int16)((Read_Buffer[24] << 8) | Read_Buffer[25]) * 0.01);
                    data.co_ssnd = (double)((Int16)((Read_Buffer[26] << 8) | Read_Buffer[27]) * 0.001);
                    data.hc_ssnd = (double)((Int16)((Read_Buffer[28] << 8) | Read_Buffer[29]));
                    data.no_ssnd = (double)((Int16)((Read_Buffer[30] << 8) | Read_Buffer[31]));
                    data.o2_ssnd = (double)((Int16)((Read_Buffer[32] << 8) | Read_Buffer[33]) * 0.01);

                    data.fbzll = (double)((Int16)((Read_Buffer[34] << 8) | Read_Buffer[35]) * 0.001);
                    data.bzll = (double)((Int16)((Read_Buffer[36] << 8) | Read_Buffer[37]) * 0.001);
                    data.cs = (double)((Int16)((Read_Buffer[38] << 8) | Read_Buffer[39]) * 0.1);
                    data.hjwd = (double)((Int16)((Read_Buffer[40] << 8) | Read_Buffer[41]) * 0.01);
                    data.xdsd = (double)((Int16)((Read_Buffer[42] << 8) | Read_Buffer[43]) * 0.1);
                    data.xsyl = (double)((Int16)((Read_Buffer[44] << 8) | Read_Buffer[45]) * 0.01);
                    data.xswd = (double)((Int16)((Read_Buffer[46] << 8) | Read_Buffer[47]) * 0.01);
                    data.xsxdsd = (double)((Int16)((Read_Buffer[48] << 8) | Read_Buffer[49]) * 0.01);
                    data.lljyc = (double)((Int16)((Read_Buffer[50] << 8) | Read_Buffer[51]) * 0.001);

                    data.df = (double)((ushort)((Read_Buffer[52] << 8) | Read_Buffer[53]) * 0.01);
                    data.hcf = (double)((ushort)((Read_Buffer[54] << 8) | Read_Buffer[55]) * 0.001);
                    data.rpm = (double)((ushort)((Read_Buffer[56] << 8) | Read_Buffer[57]) * 1);
                    data.co2_sszl = (double)((ushort)((Read_Buffer[58] << 8) | Read_Buffer[59]) * 0.01);
                    data.co_sszl = (double)((ushort)((Read_Buffer[60] << 8) | Read_Buffer[61]) * 0.001);
                    data.hc_sszl = (double)((ushort)((Read_Buffer[62] << 8) | Read_Buffer[63]) * 0.001);
                    data.no_sszl = (double)((ushort)((Read_Buffer[64] << 8) | Read_Buffer[65]) * 0.001);
                    data.yh_ssyh = 1.015 + (double)(DateTime.Now.Millisecond * 0.001);// (double)((ushort)((Read_Buffer[66] << 8) | Read_Buffer[67]) * 0.001);
                    data.distance_ssjl = (double)((ushort)((Read_Buffer[68] << 8) | Read_Buffer[69]) * 0.0001);
                    data.fdjzs_ssyh = (double)((ushort)((Read_Buffer[70] << 8) | Read_Buffer[71]) * 1);

                    data.co2_ljzl = (double)(0.01 * (UInt32)(Read_Buffer[72] << 24 | Read_Buffer[73] << 16 | Read_Buffer[74] << 8 | Read_Buffer[75]));
                    data.co_ljzl = (double)(0.001 * (UInt32)(Read_Buffer[76] << 24 | Read_Buffer[77] << 16 | Read_Buffer[78] << 8 | Read_Buffer[79]));
                    data.hc_ljzl = (double)(0.001 * (UInt32)(Read_Buffer[80] << 24 | Read_Buffer[81] << 16 | Read_Buffer[82] << 8 | Read_Buffer[83]));
                    data.no_ljzl = (double)(0.001 * (UInt32)(Read_Buffer[84] << 24 | Read_Buffer[85] << 16 | Read_Buffer[86] << 8 | Read_Buffer[87]));

                    data.distance_ljjl = (double)((ushort)((Read_Buffer[90] << 8) | Read_Buffer[91]) * 0.001);
                    data.yh_100km = (double)((ushort)((Read_Buffer[92] << 8) | Read_Buffer[93]) * 0.01);
                    data.yh_hour = (double)((ushort)((Read_Buffer[94] << 8) | Read_Buffer[95]) * 0.01);

                    data.流量检测压力 = (double)((Int16)((Read_Buffer[96] << 8) | Read_Buffer[97]) * 0.01);
                    data.气室补偿压力 = (double)((Int16)((Read_Buffer[98] << 8) | Read_Buffer[99]) * 0.01);
                    data.红外检测器温度 = (double)((Int16)((Read_Buffer[100] << 8) | Read_Buffer[101]) * 0.01);
                    data.低流量阈值 = (double)((Int16)((Read_Buffer[102] << 8) | Read_Buffer[103]) * 1);

                    data.单位时间C质量 = (double)((ushort)((Read_Buffer[104] << 8) | Read_Buffer[105]) * 0.001);
                    data.累计C质量 = (double)((ushort)((Read_Buffer[106] << 8) | Read_Buffer[107]) * 0.1);
                    data.气体密度 = (double)((ushort)((Read_Buffer[108] << 8) | Read_Buffer[109]) * 0.001);
                    data.累加测试时间 = (double)((ushort)((Read_Buffer[110] << 8) | Read_Buffer[111]) * 0.01);
                    if (mode == 1)
                        flljyh += (data.累加测试时间 - flljsj) * data.yh_ssyh;
                    else
                        flljyh = 0;
                    data.yh_ljyh = flljyh;//(double)((ushort)((Read_Buffer[88] << 8) | Read_Buffer[89]) * 0.1);//注意，此处单位和协议不一样，协议单位为L,这里改为mL，为了和鸣泉统一

                    flljsj = data.累加测试时间;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }
        }
        #endregion
        #region 读取实时数据

        public bool getRealTimeData(out yhrRealTimeData data)
        {
            data = new yhrRealTimeData();
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    SendData(CMDgetRealTimeData);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 30)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(20);
                        if (i == 10)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                    {
                        data.CO = (ushort)(Read_Buffer[3] <<8| Read_Buffer[4]) * 0.001;
                        data.CO2 = (ushort)(Read_Buffer[5] << 8 | Read_Buffer[6]) * 0.01;
                        data.HC = (short)(Read_Buffer[7] << 8 | Read_Buffer[8]) * 1;
                        data.NO = (short)(Read_Buffer[9] << 8 | Read_Buffer[10]) * 1;
                        data.PEF = (ushort)(Read_Buffer[11] << 8 | Read_Buffer[12]) * 0.001;
                        data.QLYL = (ushort)(Read_Buffer[13] << 8 | Read_Buffer[14]) * 0.1;
                        data.HJWD = (short)(Read_Buffer[15] << 8 | Read_Buffer[16]) * 0.1;
                        data.HJYL = (ushort)(Read_Buffer[17] << 8 | Read_Buffer[18]) * 0.1;
                        data.HJSD = (ushort)(Read_Buffer[19] << 8 | Read_Buffer[20]) * 0.1;
                        data.BZLL = (ushort)(Read_Buffer[21] << 8 | Read_Buffer[22]) * 1;
                        data.FBZLL = (ushort)(Read_Buffer[23] << 8 | Read_Buffer[24]) *1;
                        data.XSYL = (ushort)(Read_Buffer[25] << 8 | Read_Buffer[26]) * 0.1;
                        data.XSWD = (short)(Read_Buffer[27] << 8 | Read_Buffer[28]) * 0.1;
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
        #region 动力性测试读取环境参数 用

        public bool getTempData(out yhrRealTimeData data)
        {
            data = new yhrRealTimeData();
            try
            {
                ReadData();
                int i = 0;
                string msg = "";
                if (!ComPort_1.IsOpen)      //串口出错
                    return false;
                switch (yqxh)
                {
                    case "mql_8201":
                        SendData(CMDgetRealTimeData);
                        Thread.Sleep(30);
                        while (ComPort_1.BytesToRead < 30)                          //等待仪器返回
                        {
                            i++;
                            Thread.Sleep(50);
                            if (i == 20)
                                return false;
                        }
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[0] == 0x06)
                        {
                            data.CO = (ushort)(Read_Buffer[3] << 8 | Read_Buffer[4]) * 0.001;
                            data.CO2 = (ushort)(Read_Buffer[5] << 8 | Read_Buffer[6]) * 0.01;
                            data.HC = (short)(Read_Buffer[7] << 8 | Read_Buffer[8]) * 1;
                            data.NO = (short)(Read_Buffer[9] << 8 | Read_Buffer[10]) * 1;
                            data.PEF = (ushort)(Read_Buffer[11] << 8 | Read_Buffer[12]) * 0.001;
                            data.QLYL = (ushort)(Read_Buffer[13] << 8 | Read_Buffer[14]) * 0.1;
                            data.HJWD = (short)(Read_Buffer[15] << 8 | Read_Buffer[16]) * 0.1;
                            data.HJYL = (ushort)(Read_Buffer[17] << 8 | Read_Buffer[18]) * 0.1;
                            data.HJSD = (ushort)(Read_Buffer[19] << 8 | Read_Buffer[20]) * 0.1;
                            data.BZLL = (ushort)(Read_Buffer[21] << 8 | Read_Buffer[22]) * 1;
                            data.FBZLL = (ushort)(Read_Buffer[23] << 8 | Read_Buffer[24]) * 1;
                            data.XSYL = (ushort)(Read_Buffer[25] << 8 | Read_Buffer[26]) * 0.1;
                            data.XSWD = (short)(Read_Buffer[27] << 8 | Read_Buffer[28]) * 0.1;
                            return true;
                        }
                        else
                            return false;
                        break;

                    case flxh:
                        fl_StatusAndData fldata = new fl_StatusAndData();
                        if (getFlRealTimeDataUnAdd(0, out fldata))
                        {
                            data.CO = fldata.co_ssnd;
                            data.CO2 = fldata.co2_ssnd;
                            data.HC = fldata.hc_ssnd;
                            data.NO = fldata.no_ssnd;
                            data.PEF = 0;
                            data.QLYL = fldata.流量检测压力;
                            data.HJWD = fldata.hjwd;
                            data.HJYL = fldata.xsyl;
                            data.HJSD = fldata.xdsd;
                            data.BZLL = fldata.bzll;
                            data.FBZLL = fldata.fbzll;
                            data.XSYL = fldata.xsyl;
                            data.XSWD = fldata.xswd;
                            return true;
                        }
                        else
                            return false;
                    case nhxh:
                        SendDataOfNHF(NH_GetFuelData);
                        Thread.Sleep(30);
                        while (ComPort_1.BytesToRead < 44)                          //协议上有分歧，有PEF值返回时为44个字节，没有PEF返回时为39个字节
                        {
                            i++;
                            Thread.Sleep(20);
                            if (i == 200)
                                return false;
                        }
                        ReadData();                     //读取返回的数据
                        if (Read_Buffer[3] == 0x38)
                            return false;
                        else
                        {
                            try
                            {
                                data.CO = 0;
                                data.CO2 = 0;
                                data.HC = 0;
                                data.NO = 0;
                                data.PEF = 0;
                                data.QLYL = 0;
                                string datastring = Encoding.Default.GetString(Read_Buffer, 23, 4);
                                data.HJYL = Convert.ToInt16(datastring, 16) * 0.1;
                                datastring = Encoding.Default.GetString(Read_Buffer, 27, 4);
                                data.HJWD = Convert.ToInt16(datastring, 16) - 30;
                                data.HJSD = 50;
                                data.BZLL = 0;
                                data.FBZLL = 0;
                                data.XSYL = 0;
                                data.XSWD = 0;
                                return true;//HC
                            }
                            catch
                            {
                                return false;

                            }
                        }
                        break;

                    default:
                        return false;
                        break;
                }
            }
            catch(Exception er)
            {
                throw new Exception(er.Message);
            }

        }
        #endregion

        #region 读取实时状态

        public bool getRealTimeStatus(out yhrRealTimeStatus data)
        {
            data = new yhrRealTimeStatus();
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    SendData(CMDgetRealTimeStatus);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 11)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                    {
                        data.预热状态位 = (Read_Buffer[3]&0x01);
                        data.反吹状态位 = (Read_Buffer[3] & 0x02);
                        data.背景空气测定状态位 = (Read_Buffer[3] & 0x04);
                        data.环境空气测定状态位 = (Read_Buffer[3] & 0x08);
                        data.泄露检查状态位 = (Read_Buffer[3] & 0x10);
                        data.HC残留测定状态位 = (Read_Buffer[3] & 0x20);
                        data.气体调零状态位 = (Read_Buffer[3] & 0x40);
                        data.气路低流量警告 = (Read_Buffer[3] & 0x80);

                        data.等待调零位 = (Read_Buffer[4] & 0x01);
                        data.调零方式位 = (Read_Buffer[4] & 0x02);
                        data.系统忙状态位 = (Read_Buffer[4] & 0x80);


                        data.泄露检查结果位 = (Read_Buffer[5] & 0x01);
                        data.碳氢残留测定结果位 = (Read_Buffer[5] & 0x02);
                        data.气体调零结果位 = (Read_Buffer[5] & 0x80);

                        data.燃料类型 = (Read_Buffer[7] & 0x40);


                        data.流量调零标志位 = (Read_Buffer[8] & 0x01);
                        data.流量调零结果位 = (Read_Buffer[8] & 0x02);
                        data.流量压力状态位 = (Read_Buffer[8] & 0x03);
                        data.流量温度状态位 = (Read_Buffer[8] & 0x04);

                        switch ((byte)(Read_Buffer[9]&0x03))
                        {
                            case 0x00:data.流量状态 = "低流量"; break;
                            case 0x01: data.流量状态 = "中流量"; break;
                            case 0x02: data.流量状态 = "高流量"; break;
                            case 0x03: data.流量状态 = "预留"; break;
                            default:break;

                        }
                        data.CO2修正状态 = (Read_Buffer[8] & 0x04);
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
        public bool getNhStatus(out NH_status2 data)
        {
            Int32 status;
            data = new NH_status2();
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                        return true;
                    break;
                case flxh:return true;
                    break;
                case nhxh:
                    byte[] NH_data = NH_SetNdzz;
                    NH_data[5] = 0x00;
                    SendDataOfNHF(NH_data);
                    Thread.Sleep(20);
                    while (ComPort_1.BytesToRead < 17)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();
                    string datastring = Encoding.Default.GetString(Read_Buffer, 9, 4);
                    status = Convert.ToInt32(datastring, 16);
                    
                    data.正在预热  = (status == 0x1000);
                    data.等待检漏 = (status == 0x2000);
                    data.正在检漏 = (status == 0x0100);
                    data.检漏成功 = (status == 0x0200);
                    data.检测失败 = (status == 0x0400);
                    data.等待调零 = (status == 0x4000);
                    data.正在调零 = (status == 0x0020);
                    data.调零成功 = (status == 0x0040);
                    data.调零失败 = (status == 0x0080);
                    data.正在反吹 = (status == 0x0800);
                    data.正在开泵 = (status == 0x0001);
                    data.正在校准 = (status == 0x0004);
                    data.校准成功 = (status == 0x0008);
                    data.校准失败 = (status == 0x0010);
                    data.废气仪待机 = (status == 0x0000);
                    return true;
                    break;
                default:
                    return false;
                    break;
            }

        }
        #endregion
        #region 读取实时油耗

        public bool getRealTimeYh(out yhrRealTimeYh data)
        {
            data = new yhrRealTimeYh();
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    SendData(CMDgetRealTimeYh);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 14)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                    {
                        data.CHECKTIME = (Read_Buffer[3] * 256 + Read_Buffer[4]) * 1;
                        data.SSYH = (Read_Buffer[5] * 256*256*256 + Read_Buffer[6]*256*256+Read_Buffer[7]*256+Read_Buffer[8]) * 0.001;
                        data.ZYH = (Read_Buffer[9] * 256 * 256 * 256 + Read_Buffer[10] * 256 * 256 + Read_Buffer[11] * 256 + Read_Buffer[12]) * 0.001;
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
        #region 读取排放物实时质量

        public bool getRealTimePfwsszl(out yhrRealTimePfwsszl data)
        {
            data = new yhrRealTimePfwsszl();
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    SendData(CMDgetRealTimePfwsszl);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 12)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(20);
                        if (i == 10)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                    {
                        data.SSCO = (ushort)(Read_Buffer[3]<<8| Read_Buffer[4]) * 0.1;
                        data.SSCO2 = (ushort)(Read_Buffer[5]<<8|Read_Buffer[6]) * 0.01;
                        data.SSHC = (short)(Read_Buffer[7]<<8| Read_Buffer[8]) * 1;
                        data.SSNO = (ushort)(Read_Buffer[9]<<8| Read_Buffer[10] ) * 0.01;
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
        #region 读取排放物总质量

        public bool getRealTimePfzzl(out yhrRealTimePfzzl data)
        {
            data = new yhrRealTimePfzzl();
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    SendData(CMDgetRealTimePfwsszl);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 22)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(10);
                        if (i == 100)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                    {
                        data.ZTIME = (ushort)(Read_Buffer[3] <<8| Read_Buffer[4]) * 11;
                        data.ZCO = (uint)(Read_Buffer[5]<<24| Read_Buffer[6]<<16| Read_Buffer[7] <<8| Read_Buffer[8]) * 0.1;
                        data.ZCO2 = (uint)(Read_Buffer[9]<<24| Read_Buffer[10] << 16 | Read_Buffer[11] << 8 | Read_Buffer[12]) * 0.01;
                        data.ZHC = (uint)(Read_Buffer[13] << 24 | Read_Buffer[14] << 16 | Read_Buffer[15] << 8 | Read_Buffer[16]) * 1;
                        data.ZNO = (uint)(Read_Buffer[17] << 24 | Read_Buffer[18] << 16 | Read_Buffer[19] << 8 | Read_Buffer[20]) * 0.01;
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
        #region 读取所有实时数据

        public bool getRealTimeTotalData(out yhrRealTimeTotalData data)
        {
            data = new yhrRealTimeTotalData();
            ReadData();
            int i = 0;
            string msg = "";
            if (!ComPort_1.IsOpen)      //串口出错
                return false;
            switch (yqxh)
            {
                case "mql_8201":
                    SendData(CMDgetRealTimeTotalData);
                    Thread.Sleep(30);
                    while (ComPort_1.BytesToRead < 34)                          //等待仪器返回
                    {
                        i++;
                        Thread.Sleep(20);
                        if (i == 50)
                            return false;
                    }
                    ReadData();                     //读取返回的数据
                    if (Read_Buffer[0] == 0x06)
                    {
                        data.CO = (ushort)(Read_Buffer[3]<<8|Read_Buffer[4]) * 0.001;
                        data.CO2 = (ushort)(Read_Buffer[5] << 8 | Read_Buffer[6]) * 0.01;
                        data.HC = (short)(Read_Buffer[7] << 8 | Read_Buffer[8]) * 1;
                        data.BZLL = (ushort)(Read_Buffer[9] << 8 | Read_Buffer[10]) * 1;
                        data.FBZLL = (ushort)(Read_Buffer[11] << 8 | Read_Buffer[12]) * 1;
                        data.XSYL = (ushort)(Read_Buffer[13] << 8 | Read_Buffer[14]) * 0.1;
                        data.XSWD = (ushort)(Read_Buffer[15] << 8 | Read_Buffer[16]) * 0.1;
                        data.SSCO = (ushort)(Read_Buffer[17] << 8 | Read_Buffer[18]) * 0.1;
                        data.SSCO2 = (ushort)(Read_Buffer[19] << 8 | Read_Buffer[20]) * 0.01;
                        data.SSHC = (ushort)(Read_Buffer[21] << 8 | Read_Buffer[22]) * 1;
                        data.CHECKTIME = (ushort)(Read_Buffer[23] << 8 | Read_Buffer[24]) * 1;
                        data.SSYH = (uint)(Read_Buffer[25] << 24 | Read_Buffer[26]<<16|Read_Buffer[27]<<8| Read_Buffer[28]) * 0.001;
                        data.ZYH = (uint)(Read_Buffer[29] << 24 | Read_Buffer[30] << 16 | Read_Buffer[31] << 8 | Read_Buffer[32]) * 0.001;
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
