using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace Dynamometer
{
    public class IGBT
    {
        public System.IO.Ports.SerialPort ComPort_2;
        public enum IGBT_STATUS { IDLE,FORCE,POWER,SPEED,CURRENT };
        public IGBT_STATUS igbt_status_now=IGBT_STATUS.IDLE;
        public byte[] Send_Buffer;                                      //发送缓冲区
        public byte[] Read_Buffer;                                      //读取缓冲区
        public static string UseMK = "BNTD";                            //使用什么模块
        public string All_Content = "";                                   //没有解析过的返回数据
        public List<byte> All_Content_byte = new List<byte>();            //没有解析过的返回数据
        public List<float> speed_list = new List<float>();
        public List<float> force_list = new List<float>();
        public string Time_Results = "";                                //实时返回数据
        public string Time_AD = "";                                     //实时AD值
        public string Time_PID_Out = "";                                //实时PID值输出
        public string Time_PID = "";                                    //实时PID值
        public float Speed = 0;                                                                     //车速
        public float Force = 0;                                                                     //扭矩
        public float Power = 0;                                                                     //功率
        public bool isComSuccess = true;
        public int noDataTimeCount = 0;
        public float Speed_single = 0;                                                                     //车速
        public float Force_single = 0;                                                                     //扭矩
        public float Power_single = 0;                                                                     //功率
        public double forcexs = 1;
        public float Duty = 0;                                                                      //占空比
        public float Control_Speed = 0f;                                                             //控制的速度
        public float Control_Force = 0f;                                                             //控制的扭力
        public float Control_Power = 0f;                                                             //控制的功率
        public string Speed_string = "0.0";                                    //实时速度
        public string Force_string = "0.0";                                    //实时扭矩
        public string Power_string = "0.0";                                    //实时功率
        public string Duty_string = "0.0";                                     //占空比(BNTD)
        public string Acceleration = "0.0";                             //实时加速度
        public string Range = "0.0";                                    //实时行驶距离
        public string Control = "";                                     //遥控控制状态
        public string Msg = "";                                         //消息
        public string Msg_received = "";                                         //消息

        public string inertnessString = "";
        public float inertness = 907;
        public string carRMstring = "";
        public short carRM = 0;
        public byte keyandgd = 0;

        public Thread readThread = null;
        static bool _continue = false;
        public bool msg_back = false;
        //以下为新成BNTD专用变量
        public string Time_Vol_1 = "0.0";                               //实时电压值1通道(BNTD)
        public string Time_Vol_2 = "0.0";                               //实时电压值2通道(BNTD)
        public string Time_Vol_3 = "0.0";                               //实时电压值3通道(BNTD)
        public double speed_double = 0.0;
        public double speed_sum = 0.0;
        public byte caculate_count = 0;
        public double force_sum = 0.0;
        public double force_double = 0.0;
        public double vol_1_double = 0.0;
        public double vol_2_double = 0.0;
        public double vol_3_double = 0.0;
        public string Temperature = "0.0";                              //温度(BNTD)
        public string Humidity = "0.0";                                 //湿度(BNTD)
        public string ATM = "0.0";                                      //大气压(BNTD)
        public double b0_double = 0.0;
        public double b1_double = 0.0;
        public double b2_double = 0.0;
        public string b0 = "0.0";                                       //标定系数b0 1通道(BNTD)
        public string c0 = "0.0";                                       //标定系数c0 1通道(BNTD)
        public string b1 = "0.0";                                       //标定系数b1 2通道(BNTD)
        public string c1 = "0.0";                                       //标定系数c1 2通道(BNTD)
        public string b2 = "0.0";                                       //标定系数b2 3通道(BNTD)
        public string c2 = "0.0";                                       //标定系数c2 3通道(BNTD)
        public string kp = "0.0";                                       //速度PID参数P(BNTD)
        public string ki = "0.0";                                       //速度PID参数I(BNTD)
        public string kd = "0.0";                                       //速度PID参数D(BNTD)
        public string kp_force = "0.0";                                 //力PID参数P(BNTD)
        public string ki_force = "0.0";                                 //力PID参数I(BNTD)
        public string kd_force = "0.0";                                 //力PID参数D(BNTD)
        public string Speed_Diameter = "0.0";                           //滚筒直径
        public string Speed_Pusle = "0.0";                              //脉冲数
                                                                        //public string Duty = "0.0";                                     //占空比(BNTD)

        bool Read_Flag = false;                                         //是否有数据可以读取
        private Thread Th_Lifter_Up = null;                             //举升上升线程
        public static Thread Th_Resolve = null;                         //解析线程
        public string Status = "time";                                  //IGBT状态 time——实时 Speed——恒速控制 Force——恒扭矩 Power——恒功率 Demarcate——标定 T——取环境参数(BNTD) F——取标定系数(BNTD) s——取速度PID控制参数(BNTD) f——取力PID控制参数(BNTD) S——取速度系数(BNTD) null——空闲
        public double sum = 0;

        #region IGBT通讯协议
        byte Head = 0x2A;                                                 //命令头“*”                      
        string Cmd_Set_Constant_Speed = "*CS";                            //设置恒速
        string Cmd_Set_Constant_Force = "*CF";                          //设置恒扭力
        string Cmd_Set_Power = "*CP";                                   //设定恒功率
        string Cmd_Set_Control = "*CC";                                 //设置PID控制值
        string Cmd_Set_Parasitic = "*CH";                               //设置寄生功
        string Cmd_Start_Speed = "*SA";                                 //启动恒速度控制
        string Cmd_Start_Force = "*SB";                                 //启动恒扭力控制
        string Cmd_Start_Power = "*SC";                                 //启动恒功率控制
        string Cmd_Stop_Control = "*SS";                                //停止控制
        string Cmd_Lifting_Up = "*LU";                                  //举升上升
        string Cmd_Lifting_Down = "*LD";                                //举升下降
        string Cmd_Demarcate = "*DA";                                   //进入标定状态
        string Cmd_Demarcate_Exit = "*DS";                              //退出标定状态
        string Cmd_Clear_Mileage = "*DC";                               //行驶里程清零
        string Cmd_Get_PID = "*DP";                                     //获取DIP控制参数
        string Cmd_Get_PID_Export = "*DV";                              //获取DIP输出值
        string Cmd_Demarcate_Speed = "*I";                              //标定速度
        string Cmd_Demarcate_Force = "*Y";                              //标定扭矩
        string Cmd_Clear_Force = "*YC";                                 //扭矩清零
        string Cmd_Start_IGBT = "*PT";                                  //开始IGBT输出
        string Cmd_Stop_IGBT = "*PS";                                   //停止IGBT输出
        string Cmd_Get_IGBT = "*PV";                                    //获取IGBT输出值
        string Cmd_Reverse_Motor = "*MO";                               //反拖电机开启
        string Cmd_Reverse_Motor_Stop = "*MF";                          //反拖电机关闭
        string Cmd_Start_Simulation = "*RT";                            //开始道路阻力模拟
        string Cmd_Stop_Simulation = "*RS";                             //停止道路阻力模拟
        string Cmd_Set_Quality = "*RV";                                 //设置基准质量
        #endregion

        #region BNTD通讯协议
        string BNTD_Cmd_Set_Speed = "XSS";                              //BNTD设置恒速度值(km/h)
        string BNTD_Cmd_Set_Carjzzl = "XSM";                              //BNTD设置汽车的基准质量
        string BNTD_Cmd_Set_CgjInertness = "XSN";                              //BNTD设置汽车的基准质量
        string BNTD_Cmd_Set_ClearKey = "XSK";                              //BNTD清除按键值
        //string BNTD_Cmd_Set_Carjzzl = "XSM";                              //BNTD设置汽车的基准质量
        string BNTD_Cmd_Set_Force = "XSF";                              //BNTD设置恒扭力值(N)
        string BNTD_Cmd_Set_Power = "XSP";                              //BNTD设置恒功率值(kw)
        string BNTD_Cmd_Set_Duty = "XSI";                               //设置IGBT的pwm波占空比
        string BNTD_Cmd_Set_Force_Aisle = "XSC";                        //设置力所用的通道号
        string BNTD_Cmd_Set_Clear_Force = "XSZ";                        //设置力通道清零
        string BNTD_Cmd_Set_Diameter = "XCG";                           //设置滚筒直径
        string BNTD_Cmd_Set_Pulse = "XCP";                              //设置滚筒直径
        string BNTD_Cmd_Set_Force_b0 = "XCA";                           //设置通道1力标定系数b[0]：force=b[0]*(ad_value-c[0])
        string BNTD_Cmd_Set_Force_c0 = "XCB";                           //设置通道1力标定系数c[0]：force=b[0]*(ad_value-c[0])
        string BNTD_Cmd_Set_Force_b1 = "XCC";                           //设置通道2力标定系数b[1]：force=b[1]*(ad_value-c[1])
        string BNTD_Cmd_Set_Force_c1 = "XCD";                           //设置通道2力标定系数c[1]：force=b[1]*(ad_value-c[1])
        string BNTD_Cmd_Set_Force_b2 = "XCE";                           //设置通道3力标定系数b[2]：force=b[2]*(ad_value-c[2])
        string BNTD_Cmd_Set_Force_c2 = "XCF";                           //设置通道3力标定系数c[2]：force=b[2]*(ad_value-c[2])
        string BNTD_Cmd_Set_Duty_kp = "XCH";                            //设置恒速控制PID控制比例系数kp
        string BNTD_Cmd_Set_Duty_ki = "XCI";                            //设置恒速控制PID控制比例系数ki
        string BNTD_Cmd_Set_Duty_kd = "XCJ";                            //设置恒速控制PID控制比例系数kd
        string BNTD_Cmd_Set_Duty_kp_force = "XCK";                      //设置力控制PID控制比例系数kp_force
        string BNTD_Cmd_Set_Duty_ki_force = "XCL";                      //设置力控制PID控制比例系数ki_force
        string BNTD_Cmd_Set_Duty_kd_force = "XCM";                      //设置力控制PID控制比例系数kd_force
        string BNTD_Cmd_Control_Start_Speed = "XBS";                    //启动恒速度控制
        string BNTD_Cmd_Control_Start_Force = "XBF";                    //启动恒扭力控制
        string BNTD_Cmd_Control_Start_Power = "XBP";                    //启动恒功率控制
        string BNTD_Cmd_Control_Start_Duty = "XBI";                     //启动输出设定pwm值
        string BNTD_Cmd_Control_Start_InertnessPower = "XBM";                     //启动有仿惯量情况下的恒功率
        string BNTD_Cmd_Control_Stop_Quit = "XBQ";                      //停止控制,pwm输出0下位机为空闲状态
        string BNTD_Cmd_Control_Motor_Start = "XEB";                    //启动电机
        string BNTD_Cmd_Control_Motor_Stop = "XES";                     //停止电机
        string BNTD_Cmd_Lifting_Up = "XJU";                             //停止控制,pwm输出0下位机为空闲状态
        string BNTD_Cmd_Lifting_Down = "XJD";                           //停止控制,pwm输出0下位机为空闲状态
        string BNTD_Cmd_Demarcate = "XDF";                              //进入标定状态
        string BNTD_Cmd_Demarcate_Exit = "XDS";                         //退出标定状态
        string BNTD_Cmd_Read_Inertness = "XRI";                         //读取测功机惯量及汽车基准质量
        string BNTD_Cmd_Read_Environment = "XRT";                       //读取环境参数
        string BNTD_Cmd_Read_Force_Modulus = "XRF";                     //读取力标定系数b,c
        string BNTD_Cmd_Read_Speed_PID = "XRs";                         //读取恒速PID系数kp、ki、kp
        string BNTD_Cmd_Read_Force_PID = "XRf";                         //读取恒力PID系数kp_force、ki_force、kp_force
        string BNTD_Cmd_Read_Speed_Modulus = "XRS";                     //读取速度系数
        string BNTD_Cmd_Solidify_Force_Modulus = "XMF";                //将通道1力标定系数固化到ROM中
        string BNTD_Cmd_Solidify_Speed_Duty_Modulus = "XMs";            //将恒速控制的PID系数固化到ROM中
        string BNTD_Cmd_Solidify_Force_Duty_Modulus = "XMf";            //将恒力控制的PID系数固化到ROM中
        string BNTD_Cmd_Solidify_Speed_Modulus = "XMS";                 //将速度的直径和脉冲数固化到ROM中
        string BNTD_Cmd_Solidify_Inertness = "XMI";                       //将惯量值固化到ROM中

        string BNTD_Cmd_Control_Relay_TurnOn = "XEY";                    //开继电器
        string BNTD_Cmd_Control_Relay_TurnOff = "XEN";                     //关继电器
        #endregion

        #region 构造函数
        private bool hasGdyk = false;
        //public IGBT(string MK)
        //{
        //    UseMK = MK;
        //}
        public IGBT(string MK,bool gk)
        {
            UseMK = MK;
            hasGdyk =gk;
        }
        #endregion
        #region 关闭相应资源
        public void closeIgbt()
        {
            Exit_Control();
            _continue = false;
            Thread.Sleep(5);
            readThread.Abort();
            if (ComPort_2.IsOpen)
                ComPort_2.Close();
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
                if (hasGdyk)
                    readThread = new Thread(ResolveGd);
                else
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
                switch (UseMK.ToUpper())
                {
                    case "IGBT":
                        Cmd[Cmd_Temp.Length + 1] = 0x0D;
                        break;
                    case "BNTD":
                        Cmd[Cmd_Temp.Length + 1] = 0x43;
                        break;
                    default:
                        Cmd[Cmd_Temp.Length + 1] = 0x43;
                        break;
                }
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
                if (All_Content_byte.Count > 16)
                {
                    noDataTimeCount = 0;
                    isComSuccess = true;
                    int start = 0;
                    int end = 0;
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
                            temp_start1 = All_Content_byte.IndexOf(0x41);       //A
                            temp_start2 = All_Content_byte.IndexOf(0x44);       //D
                            if ((temp_start2 < temp_start1) && (temp_start2 != -1))
                            {
                                start = temp_start2;
                                msg_back = true;
                                end = All_Content_byte.IndexOf(0x43);//如果是调试信息，则以C结尾为结尾标志
                            }
                            else if (temp_start1 != -1)
                            {
                                start = temp_start1;
                                if (All_Content_byte.Count >= start + 15)
                                    end = start + 15;
                                else
                                    continue;
                            }
                            if (start == -1)
                            {
                                //没有开始符抛弃所有返回数据
                                All_Content_byte.Clear();
                                continue;
                            }
                            //end = All_Content_byte.IndexOf(0x43);   //C
                            if (end == -1)
                                continue;
                            if (end <= start)
                            {
                                All_Content_byte.RemoveRange(0, start);
                                continue;
                            }
                            if (msg_back)                   //解析的是消息
                            {
                                try
                                {
                                    Msg_received = Encoding.Default.GetString(All_Content_byte.ToArray(), start + 1, end - start - 1);
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    continue;
                                }
                                catch (Exception)
                                {
                                }
                            }
                            string sd = Encoding.Default.GetString(All_Content_byte.ToArray(), start + 1, 2);
                            //keyandgd = All_Content_byte[start + 15];//光电及遥控信息
                            switch (Encoding.Default.GetString(All_Content_byte.ToArray(), start + 1, 2))
                            {
                                case "DF":
                                    //igbt_status_now = IGBT_STATUS.IDLE;
                                    vol_1_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    vol_2_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    vol_3_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11);
                                    Time_Vol_1 = vol_1_double.ToString("0.000");
                                    Time_Vol_2 = vol_2_double.ToString("0.000");
                                    Time_Vol_3 = vol_3_double.ToString("0.000");
                                    All_Content_byte.RemoveRange(0, end + 1);

                                    break;
                                case "BS":
                                    igbt_status_now = IGBT_STATUS.SPEED;
                                    Speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    Force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);

                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "BP":
                                    igbt_status_now = IGBT_STATUS.POWER;
                                    Speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    Force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "BF":
                                    igbt_status_now = IGBT_STATUS.FORCE;
                                    Speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    Force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "BI":
                                    igbt_status_now = IGBT_STATUS.CURRENT;
                                    Speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    Force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "II":
                                    igbt_status_now = IGBT_STATUS.IDLE;
                                    byte[] ds = new byte[] { 0xff, 0xff, 0xff, 0xff };
                                    float sdd = BitConverter.ToSingle(ds, 0);
                                    Speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    Force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RT":
                                   // igbt_status_now = IGBT_STATUS.IDLE;
                                    Temperature = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString("0.0");
                                    Humidity = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString("0.0");
                                    ATM = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "Rs":
                                   // igbt_status_now = IGBT_STATUS.IDLE;
                                    kp = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                    ki = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                    kd = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString();
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "Rf":
                                    //igbt_status_now = IGBT_STATUS.IDLE;
                                    kp_force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                    ki_force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                    kd_force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString();
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RS":
                                    //igbt_status_now = IGBT_STATUS.IDLE;
                                    Speed_Diameter = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                    Speed_Pusle = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RA":
                                    //igbt_status_now = IGBT_STATUS.IDLE;

                                    b0_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    b1_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11);
                                    b0 = b0_double.ToString();
                                    c0 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                    b1 = b1_double.ToString();
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RI":
                                    //igbt_status_now = IGBT_STATUS.IDLE;
                                    inertness = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    carRM = (short)(BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7));
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RB":
                                    //igbt_status_now = IGBT_STATUS.IDLE;
                                    b2_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    c1 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                    b2 = b2_double.ToString();
                                    c2 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString();
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    isGetForceXs = true;
                                    break;
                                default:
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                            }
                            if (0 <= Speed_single && Speed_single <= 200)
                                speed_list.Add(Speed_single);
                            if (-20000 <= Force_single && Force_single <= 20000)
                                force_list.Add(Force_single);
                            if (speed_list.Count > 20)
                                speed_list.RemoveAt(0);
                            if (force_list.Count > 20)
                                force_list.RemoveAt(0);
                            speed_sum = speed_list.Sum();
                            force_sum = force_list.Sum();
                            if (force_list.Count >= 20)
                            {
                                caculate_count++;
                                Speed = (float)speed_sum / 20f;
                                Force = (float)force_sum / 20f;
                                Power = Speed / 3.6f * Force / 1000f;
                                if (caculate_count == 10)
                                {
                                    Speed_string = Speed.ToString("0.00");
                                    Force_string = Force.ToString("0");
                                    caculate_count = 0;
                                }
                            }

                            // Power = (float)Convert.ToDouble(Power_string);
                            Duty = (float)Convert.ToDouble(Duty_string);

                        }
                        if (All_Content_byte.Count > 32)
                            All_Content_byte.RemoveRange(0, All_Content_byte.Count - 17);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            All_Content_byte.RemoveRange(0, end + 1);
                        }
                        catch (Exception)
                        {
                            All_Content_byte.Clear();
                        }
                    }
                }
                else
                {
                    noDataTimeCount++;
                    if (noDataTimeCount >= 100)//如果超过500ms依然没有一次数据返回，则认为通讯中断
                    {
                        isComSuccess = false;
                        noDataTimeCount = 0;
                    }
                }
                Thread.Sleep(4);
            }

        }
        #endregion
        #region 串口返回数据解析
        /// <summary>
        /// 串口返回数据解析
        /// </summary>
        private void ResolveGd()
        {
            while (_continue)
            {
                ReadData();
                if (All_Content_byte.Count > 17)
                {
                    noDataTimeCount = 0;
                    isComSuccess = true;
                    int start = 0;
                    int end = 0;
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
                            temp_start1 = All_Content_byte.IndexOf(0x41);       //A
                            temp_start2 = All_Content_byte.IndexOf(0x44);       //D
                            if ((temp_start2 < temp_start1) && (temp_start2 != -1))
                            {
                                start = temp_start2;
                                msg_back = true;
                                end = All_Content_byte.IndexOf(0x43);//如果是调试信息，则以C结尾为结尾标志
                            }
                            else if (temp_start1 != -1)
                            {
                                start = temp_start1;
                                if (All_Content_byte.Count >= start + 16)
                                    end = start + 16;
                                else
                                    continue;
                            }
                            if (start == -1)
                            {
                                //没有开始符抛弃所有返回数据
                                All_Content_byte.Clear();
                                continue;
                            }
                            //end = All_Content_byte.IndexOf(0x43);   //C
                            if (end == -1)
                                continue;
                            if (end <= start)
                            {
                                All_Content_byte.RemoveRange(0, start);
                                continue;
                            }
                            if (msg_back)                   //解析的是消息
                            {
                                try
                                {
                                    Msg_received = Encoding.Default.GetString(All_Content_byte.ToArray(), start + 1, end - start - 1);
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    continue;
                                }
                                catch (Exception)
                                {
                                }
                            }
                            string sd = Encoding.Default.GetString(All_Content_byte.ToArray(), start + 1, 2);

                            switch (Encoding.Default.GetString(All_Content_byte.ToArray(), start + 1, 2))
                            {
                                case "DF":
                                    vol_1_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    vol_2_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    vol_3_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11);
                                    Time_Vol_1 = vol_1_double.ToString("0.000");
                                    Time_Vol_2 = vol_2_double.ToString("0.000");
                                    Time_Vol_3 = vol_3_double.ToString("0.000");
                                    keyandgd = All_Content_byte[start + 15];//光电及遥控信息
                                    All_Content_byte.RemoveRange(0, end + 1);

                                    break;
                                case "BS":
                                    igbt_status_now = IGBT_STATUS.SPEED;
                                    Speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    Force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);

                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    keyandgd = All_Content_byte[start + 15];//光电及遥控信息
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "BP":
                                    igbt_status_now = IGBT_STATUS.POWER;
                                    Speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    Force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    keyandgd = All_Content_byte[start + 15];//光电及遥控信息
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "BF":
                                    igbt_status_now = IGBT_STATUS.FORCE;
                                    Speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    Force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    keyandgd = All_Content_byte[start + 15];//光电及遥控信息
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "BI":
                                    igbt_status_now = IGBT_STATUS.CURRENT;
                                    Speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    Force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    keyandgd = All_Content_byte[start + 15];//光电及遥控信息
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "II":
                                    igbt_status_now = IGBT_STATUS.IDLE;
                                    byte[] ds = new byte[] { 0xff, 0xff, 0xff, 0xff };
                                    float sdd = BitConverter.ToSingle(ds, 0);
                                    Speed_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    Force_single = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    Duty_string = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    Power_string = ((float.Parse(Speed_string) / 3.6 * float.Parse(Force_string)) / 1000).ToString("0.00");
                                    keyandgd = All_Content_byte[start + 15];//光电及遥控信息
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RT":
                                    Temperature = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString("0.0");
                                    Humidity = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString("0.0");
                                    ATM = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                    keyandgd = All_Content_byte[start + 15];//光电及遥控信息
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "Rs":
                                    kp = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                    ki = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                    kd = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString();
                                    keyandgd = All_Content_byte[start + 15];//光电及遥控信息
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "Rf":
                                    kp_force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                    ki_force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                    kd_force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString();
                                    keyandgd = All_Content_byte[start + 15];//光电及遥控信息
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RS":
                                    Speed_Diameter = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                    Speed_Pusle = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                    keyandgd = All_Content_byte[start + 15];//光电及遥控信息
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RA":

                                    b0_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    b1_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11);
                                    b0 = b0_double.ToString();
                                    c0 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                    b1 = b1_double.ToString();
                                    keyandgd = All_Content_byte[start + 15];//光电及遥控信息
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RI":
                                    inertness = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3);
                                    carRM = (short)(BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7));
                                    keyandgd = All_Content_byte[start + 15];//光电及遥控信息
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                case "RB":
                                    b2_double = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7);
                                    c1 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                    b2 = b2_double.ToString();
                                    c2 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString();
                                    keyandgd = All_Content_byte[start + 15];//光电及遥控信息
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                                default:
                                    All_Content_byte.RemoveRange(0, end + 1);
                                    break;
                            }
                            if (0 <= Speed_single && Speed_single <= 200)
                                speed_list.Add(Speed_single);
                            if (-20000 <= Force_single && Force_single <= 20000)
                                force_list.Add(Force_single);
                            if (speed_list.Count > 20)
                                speed_list.RemoveAt(0);
                            if (force_list.Count > 20)
                                force_list.RemoveAt(0);
                            speed_sum = speed_list.Sum();
                            force_sum = force_list.Sum();
                            if (force_list.Count >= 20)
                            {
                                caculate_count++;
                                Speed = (float)speed_sum / 20f;
                                Force = (float)force_sum / 20f;
                                Power = Speed / 3.6f * Force / 1000f;
                                if (caculate_count == 10)
                                {
                                    Speed_string = Speed.ToString("0.00");
                                    Force_string = Force.ToString("0");
                                    caculate_count = 0;
                                }
                            }
                            Duty = (float)Convert.ToDouble(Duty_string);

                        }
                        if (All_Content_byte.Count > 34)
                            All_Content_byte.RemoveRange(0, All_Content_byte.Count - 18);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            All_Content_byte.RemoveRange(0, end + 1);
                        }
                        catch (Exception)
                        {
                            All_Content_byte.Clear();
                        }
                    }
                }
                else
                {
                    noDataTimeCount++;
                    if (noDataTimeCount >= 100)//如果超过500ms依然没有一次数据返回，则认为通讯中断
                    {
                        isComSuccess = false;
                        noDataTimeCount = 0;
                    }
                }
                Thread.Sleep(1);
            }

        }
        #endregion
        /*#region 接收数据
        /// <summary>
        /// 接收数据
        /// </summary>
        public void ReadData()
        {
            try
            {
                Read_Buffer = new byte[ComPort_2.BytesToRead];
                ComPort_2.Read(Read_Buffer, 0, ComPort_2.BytesToRead);
                List<byte> buffer=Read_Buffer.ToList();
                switch (UseMK.ToUpper())
                {
                    case "IGBT":
                        while (true)
                        {
                            int end = buffer.IndexOf(13);
                            if (end == -1)
                                break;
                            else
                            {
                                buffer[end] = 38;
                                if (end > 0)
                                    buffer.RemoveAt(end - 1);
                                //else
                                //    buffer.RemoveAt(end);
                            }
                        }
                        break;
                    case "BNTD":
                        All_Content_byte.AddRange(buffer.ToList());
                        break;
                    default:
                        break;
                }
                All_Content += Encoding.Default.GetString(buffer.ToArray());
            }
            catch (Exception)
            {
                ComPort_2.DiscardInBuffer();
            }
        }
        #endregion

        #region 串口返回数据解析
        /// <summary>
        /// 串口返回数据解析
        /// </summary>
        private void Resolve()
        {
            int start = 0;
            int end = 0;
            switch (UseMK.ToUpper())
            {
                case "IGBT":
                        try
                        {
                            if (All_Content != "")
                            {
                                start = All_Content.IndexOf('*');
                                if ((start > All_Content.IndexOf('@') && All_Content.IndexOf('@') > -1) || start == -1)
                                    start = All_Content.IndexOf('@');
                                if ((start > All_Content.IndexOf('#') && All_Content.IndexOf('#') > -1) || start == -1)
                                    start = All_Content.IndexOf('#');
                                end = All_Content.IndexOf('&');
                                if (end == -1)
                                    return;
                                switch (Status)
                                {
                                    case "Demarcate":
                                        if (end - start == 7 || end - start == 8)
                                            Time_AD = All_Content.Substring(start + 2, end - start - 2);
                                        break;
                                    default:
                                        if (end - start == 32 || end - start == 33)
                                            Time_Results = All_Content.Substring(start, end - start);
                                        if (All_Content.Substring(start, 1) == "@")
                                            Msg = All_Content.Substring(start, end - start);
                                        if (All_Content.Substring(start, 1) == "#")
                                            Time_PID = All_Content.Substring(start, end - start);
                                        break;
                                }
                                if (end >= 0)
                                    All_Content = All_Content.Remove(0, end + 1);
                                else
                                    if (start >= 0)
                                        All_Content = All_Content.Remove(0, start + 1);
                                Thread.Sleep(10);
                            }
                        }
                        catch (Exception)
                        {
                            try
                            {
                                if (end >= 0)
                                    All_Content = All_Content.Remove(0, end + 1);
                                else
                                    if (start >= 0)
                                        All_Content = All_Content.Remove(0, start + 1);
                            }
                            catch (Exception)
                            {
                            }
                        }
                        if (Time_Results.Length == 32)
                        {
                            Speed = Time_Results.Substring(1, 6);
                            Force = Time_Results.Substring(7, 5);
                            Power = Time_Results.Substring(12, 6);
                            Acceleration = Time_Results.Substring(18, 6);
                            Range = Time_Results.Substring(24, 8);
                        }
                        Thread.Sleep(10);
                        if (end >= 0)
                            All_Content = All_Content.Remove(0, end + 1);
                        else
                            if (start >= 0)
                                All_Content = All_Content.Remove(0, start + 1);
                    break;
                case "BNTD":
                        try
                        {
                            if (All_Content_byte == null)
                            {
                                return ;
                            }
                            if (All_Content_byte.Count>0)
                            {
                                int temp_start1 = 0;
                                int temp_start2 = 0;
                                bool msg_back = false;
                                temp_start1 = All_Content_byte.IndexOf(0x41);       //A
                                temp_start2 = All_Content_byte.IndexOf(0x44);       //D
                                if (temp_start2 < temp_start1 && temp_start2 != -1)
                                {
                                    start = temp_start2;
                                    msg_back = true;
                                }
                                else
                                    start = temp_start1;
                                if (start == -1)
                                {
                                    //没有开始符抛弃所有返回数据
                                    All_Content_byte.Clear();
                                    return ;
                                }
                                end = All_Content_byte.IndexOf(0x43);   //C
                                if (end == -1)
                                    return ;
                                if (end <= start)
                                {
                                    All_Content_byte.RemoveRange(0,start+1);
                                    return;
                                }
                                if (msg_back)                   //解析的是消息
                                {
                                    try
                                    {
                                        Msg = Encoding.Default.GetString(All_Content_byte.ToArray(), start + 1, end - start - 1);
                                        All_Content_byte.RemoveRange(0, end + 1);
                                        return;
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                string sd = Encoding.Default.GetString(All_Content_byte.ToArray(), start + 1, 2);
                                switch (Encoding.Default.GetString(All_Content_byte.ToArray(), start + 1, 2))
                                {
                                    case "DF":
                                        Time_Vol_1 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                        Time_Vol_2 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                        Time_Vol_3 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString();
                                        All_Content_byte.RemoveRange(0, end+1);
                                        break;
                                    case "BS":
                                        Speed = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString("0.0");
                                        Force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString("0.0");
                                        Duty = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                        Power = ((float.Parse(Speed)/3.6 * float.Parse(Force))/1000).ToString("0.0");
                                        All_Content_byte.RemoveRange(0, end+1);
                                        break;
                                    case "BP":
                                        Speed = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString("0.0");
                                        Force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString("0.0");
                                        Duty = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                        Power = ((float.Parse(Speed) / 3.6 * float.Parse(Force)) / 1000).ToString("0.0");
                                        All_Content_byte.RemoveRange(0, end + 1);
                                        break;
                                    case "BF":
                                        Speed = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString("0.0");
                                        Force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString("0.0");
                                        Duty = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                        Power = ((float.Parse(Speed) / 3.6 * float.Parse(Force)) / 1000).ToString("0.0");
                                        All_Content_byte.RemoveRange(0, end + 1);
                                        break;
                                    case "BI":
                                        Speed = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString("0.0");
                                        Force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString("0.0");
                                        Duty = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                        Power = ((float.Parse(Speed) / 3.6 * float.Parse(Force)) / 1000).ToString("0.0");
                                        All_Content_byte.RemoveRange(0, end + 1);
                                        break;
                                    case "II":
                                        byte[] ds = new byte[] { 0xff, 0xff, 0xff, 0xff };
                                        float sdd = BitConverter.ToSingle(ds, 0);
                                        Speed = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString("0.0");
                                        Force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString("0.0");
                                        Duty = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                        Power = ((float.Parse(Speed) / 3.6 * float.Parse(Force)) / 1000).ToString("0.0");
                                        All_Content_byte.RemoveRange(0, end+1);
                                        break;
                                    case "RT":
                                        Temperature = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString("0.0");
                                        Humidity = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString("0.0");
                                        ATM = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                        All_Content_byte.RemoveRange(0, end+1);
                                        break;
                                    case "Rs":
                                        kp = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                        ki = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                        kd = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString();
                                        All_Content_byte.RemoveRange(0, end+1);
                                        break;
                                    case "Rf":
                                        kp_force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                        ki_force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                        kd_force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString();
                                        All_Content_byte.RemoveRange(0, end+1);
                                        break;
                                    case "RS":
                                        Speed_Diameter = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                        Speed_Pusle = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                        All_Content_byte.RemoveRange(0, end+1);
                                        break;
                                    case "RA":
                                        b0 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                        c0 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                        b1 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString();
                                        All_Content_byte.RemoveRange(0, end+1);
                                        break;
                                    case "RB":
                                        c1 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString();
                                        b2 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString();
                                        c2 = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString();
                                        All_Content_byte.RemoveRange(0, end+1);
                                        break;
                                    default:
                                        try
                                        {
                                            //Speed = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 3).ToString("0.0");
                                            //Force = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 7).ToString("0.0");
                                            //Duty = BitConverter.ToSingle(All_Content_byte.ToArray(), start + 11).ToString("0.0");
                                            //Power = ((float.Parse(Speed) / 3.6 * float.Parse(Force)) / 1000).ToString("0.0");
                                            //All_Content_byte.RemoveRange(0, end + 1);
                                        }
                                        catch (Exception)
                                        {
                                            All_Content_byte.RemoveRange(0, end + 1);
                                        }
                                        //if (end - start == 32 || end - start == 33)
                                        //    Time_Results = All_Content.Substring(start, end - start);
                                        //if (All_Content.Substring(start, 1) == "@")
                                        //    Msg = All_Content.Substring(start, end - start);
                                        //if (All_Content.Substring(start, 1) == "#")
                                        //    Time_PID = All_Content.Substring(start, end - start);
                                        //All_Content_byte.RemoveRange(start, end);
                                        All_Content_byte.RemoveRange(0, end + 1);
                                        break;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            try
                            {
                                All_Content_byte.RemoveRange(0, end+1);
                            }
                            catch (Exception)
                            {
                                All_Content_byte.Clear();
                            }
                        }
                    break;
                default:
                    break;
            }
        }
        #endregion*/

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
        public void Exit_Control()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    Cmd = Encoding.Default.GetBytes(Cmd_Stop_Control);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Control_Stop_Quit).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            Status = "Time";        //设置为默认 实时
            SendData(Cmd);
        }
        #endregion

        #region 设置恒速值
        /// <summary>
        /// 设置恒速值
        /// </summary>
        /// <param name="Speed">float 速度</param>
        public void Set_Speed(float Speed)
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    // *CS + 0070.0 + 校验码 ＋ CR
                    string temp_speed = Speed.ToString();
                    string speed = "";
                    if (temp_speed.IndexOf(".") == -1)
                        temp_speed += ".0";
                    for (int i = 0; i < 6 - temp_speed.Length; i++)
                        speed += "0";
                    speed += temp_speed;
                    Cmd = Encoding.Default.GetBytes(Cmd_Set_Constant_Speed + speed);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Speed).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(Speed));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion

        #region 设置测功机的基本惯量
        /// <summary>
        /// 设置测功机的基本惯量
        /// </summary>
        /// <param name="Speed">float 基本惯量</param>
        public void Set_CgjInertness(float jbgl)
        {
            byte[] Cmd = null;

            List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_CgjInertness).ToList();
            temp_cmd.AddRange(BitConverter.GetBytes(jbgl));
            Cmd = temp_cmd.ToArray();
            SendData(Cmd);
        }
        #endregion
        #region 设置汽车的基准质量
        /// <summary>
        /// 设置汽车的基准质量
        /// </summary>
        /// <param name="Speed">float 基准质量</param>
        public void Set_Carjzzl(float jzzl)
        {
            byte[] Cmd = null;

            List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Carjzzl).ToList();
            temp_cmd.AddRange(BitConverter.GetBytes(jzzl));
            Cmd = temp_cmd.ToArray();
            SendData(Cmd);
        }
        #endregion
        #region 清除按键
        /// <summary>
        /// 清除按键
        /// </summary>
        /// <param name="Speed">float 基本惯量</param>
        public void Set_ClearKey()
        {
            byte[] Cmd = null;

            List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_ClearKey).ToList();
            temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
            Cmd = temp_cmd.ToArray();
            SendData(Cmd);
        }
        #endregion

        #region 启动恒速控制
        /// <summary>
        /// 启动恒速控制
        /// </summary>
        public void Start_Control_Speed()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    Cmd = Encoding.Default.GetBytes(Cmd_Start_Speed);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Control_Start_Speed).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
            Status = "Speed";           //设置状态为恒速控制
        }
        #endregion

        #region 设置恒功率值
        /// <summary>
        /// 设置恒功率值
        /// </summary>
        /// <param name="Power">float 功率</param>
        public void Set_Control_Power(float Power)
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    //*CP + 0005.0 + 校验码 ＋ CR
                    string temp_power = Power.ToString();
                    string power = "";
                    if (temp_power.IndexOf(".") == -1)
                        temp_power += ".0";
                    for (int i = 0; i < 6 - temp_power.Length; i++)
                        power += "0";
                    power += temp_power;
                    Cmd = Encoding.Default.GetBytes(Cmd_Set_Power + power);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Power).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(Power));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion

        #region 启动功率控制
        /// <summary>
        /// 启动功率控制
        /// </summary>
        public void Start_Control_Power()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    Cmd = Encoding.Default.GetBytes(Cmd_Start_Power);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Control_Start_Power).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
            Status = "Power";       //设置状态为恒功率
        }
        #endregion

        #region 启动仿惯量模式下的恒功率控制
        /// <summary>
        /// 启动功率控制
        /// </summary>
        public void Start_Control_InertnessPower()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    Cmd = Encoding.Default.GetBytes(Cmd_Start_Power);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Control_Start_InertnessPower).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
            Status = "Power";       //设置状态为恒功率
        }
        #endregion

        #region 设置恒扭矩值
        /// <summary>
        /// 设置恒扭矩值
        /// </summary>
        /// <param name="Force">int 扭矩值</param>
        public void Set_Control_Force(float Force)
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    //*CF + 01000 + 校验码 ＋ CR
                    string temp_force = Force.ToString();
                    string force = "";
                    for (int i = 0; i < 5 - temp_force.Length; i++)
                        force += "0";
                    force += temp_force;
                    Cmd = Encoding.Default.GetBytes(Cmd_Set_Constant_Force + force);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Force).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(Force));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion

        #region 启动扭矩控制
        /// <summary>
        /// 启动扭矩控制
        /// </summary>
        public void Start_Control_Force()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    Cmd = Encoding.Default.GetBytes(Cmd_Start_Force);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Control_Start_Force).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
            Status = "Force";           //设置状态为恒扭矩控制
        }
        #endregion

        #region 进入扭矩标定状态
        /// <summary>
        /// 进入扭矩标定状态
        /// </summary>
        public void Start_Demarcate_Force()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    Cmd = Encoding.Default.GetBytes(Cmd_Demarcate);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Demarcate).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
            Status = "Demarcate";           //设置标定状态
        }
        #endregion

        #region 标定扭矩
        /// <summary>
        /// 标定扭矩
        /// </summary>
        /// <param name="Point">第几点</param>
        /// <param name="Force">扭矩力（N）</param>
        public void Demarcate_Force(int Point, int Force)
        {
            string temp_force = Force.ToString();
            string force = "";
            for (int i = 0; i < 5 - temp_force.Length; i++)
                force += "0";
            force += temp_force;
            byte[] Cmd = Encoding.Default.GetBytes(Cmd_Demarcate_Force + Point.ToString() + force);
            SendData(Cmd);
        }
        #endregion

        #region 退出扭矩标定状态
        /// <summary>
        /// 退出扭矩标定状态
        /// </summary>
        public void Exit_Demarcate_Force()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    Cmd = Encoding.Default.GetBytes(Cmd_Demarcate_Exit);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Demarcate_Exit).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
            Status = "null";           //设置标定状态
        }
        #endregion

        #region 扭矩清零
        /// <summary>
        /// 扭矩清零
        /// </summary>
        public void Force_Zeroing()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    Cmd = Encoding.Default.GetBytes(Cmd_Clear_Force);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Clear_Force).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion

        #region 里程清零
        /// <summary>
        /// 里程清零
        /// </summary>
        public void Range_Zeroing()
        {
            byte[] Cmd = Encoding.Default.GetBytes(Cmd_Clear_Mileage);
            SendData(Cmd);
        }
        #endregion

        #region 标定速度
        /// <summary>
        /// 标定速度(不用进入标定状态，在控制空闲时就可以调用标定)
        /// </summary>
        /// <param name="Diameter">string 滚筒直径</param>
        /// <param name="Pulse">string 脉冲数</param>
        public void Demarcate_Speed(string Diameter, string Pulse)
        {
            try
            {
                byte[] Cmd = null;
                switch (UseMK)
                {
                    case "IGBT":
                        string tempstr = Diameter.ToString();
                        string diameter = "";
                        for (int i = 0; i < 5 - tempstr.Length; i++)
                            diameter += "0";
                        diameter += tempstr;
                        tempstr = Pulse.ToString();
                        string pulse = "";
                        for (int i = 0; i < 5 - tempstr.Length; i++)
                            pulse += "0";
                        pulse += tempstr;
                        Cmd = Encoding.Default.GetBytes(Cmd_Demarcate_Speed + diameter + pulse);
                        SendData(Cmd);
                        break;
                    case "BNTD":
                        List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Diameter).ToList();
                        temp_cmd.AddRange(BitConverter.GetBytes(float.Parse(Diameter)));
                        Cmd = temp_cmd.ToArray();
                        SendData(Cmd);
                        Thread.Sleep(10);
                        temp_cmd = null;
                        temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Pulse).ToList();
                        temp_cmd.AddRange(BitConverter.GetBytes(float.Parse(Pulse)));
                        Cmd = temp_cmd.ToArray();
                        SendData(Cmd);
                        Thread.Sleep(10);
                        temp_cmd = null;
                        temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Solidify_Speed_Modulus).ToList();
                        temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                        Cmd = temp_cmd.ToArray();
                        SendData(Cmd);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
            //*I  + 00216 + 00600 ＋ 校验码 +  CR 

        }
        #endregion

        #region 举升上升
        /// <summary>
        /// 举升上升，当速度不为零时等待速度为零上升
        /// </summary>
        public void Lifter_Up()
        {
            byte[] Cmd = null;

            List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Lifting_Up).ToList();
            temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
            //while (float.Parse(Speed_string) > 1.0)                      //如果速度大于1是不能升起举升的
            //Thread.Sleep(10);
            Cmd = temp_cmd.ToArray();
            SendData(Cmd);
        }
        #endregion

        #region 举升下降
        /// <summary>
        /// 举升下降
        /// </summary>
        public void Lifter_Down()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    Cmd = Encoding.Default.GetBytes(Cmd_Lifting_Down);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Lifting_Down).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion

        #region 启动电机
        /// <summary>
        /// 启动电机
        /// </summary>
        public void Motor_Open()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    Cmd = Encoding.Default.GetBytes(Cmd_Reverse_Motor);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Control_Motor_Start).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion

        #region 关闭电机
        /// <summary>
        /// 关闭电机
        /// </summary>
        public void Motor_Close()
        {
            byte[] Cmd = null;
            switch (UseMK)
            {
                case "IGBT":
                    Cmd = Encoding.Default.GetBytes(Cmd_Reverse_Motor_Stop);
                    break;
                case "BNTD":
                    List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Control_Motor_Stop).ToList();
                    temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                    Cmd = temp_cmd.ToArray();
                    break;
                default:
                    break;
            }
            SendData(Cmd);
        }
        #endregion

        #region 读取环境参数 BNDT用
        /// <summary>
        /// 读取环境参数
        /// </summary>
        /// <returns>float[] 下标0为温度、1为湿度、2为大气压 如果3个数都是0则未成功</returns>
        public float[] Get_Environment()
        {
            float[] Environment = new float[] { 0, 0, 0 };
            try
            {
                List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Read_Environment).ToList();
                temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                byte[] Cmd = temp_cmd.ToArray();
                SendData(Cmd);
                Thread.Sleep(500);                              //发送完取环境参数命令后等待500ms 以便环境参数更新
                Environment[0] = float.Parse(Temperature);      //温度
                Environment[1] = float.Parse(Humidity);         //湿度
                Environment[2] = float.Parse(ATM);              //大气压
                return Environment;
            }
            catch (Exception)
            {
                return Environment;
            }
        }
        #endregion

        #region 读取力通道的系数 BNDT用
        /// <summary>
        /// 读取力通道的系数 BNDT用 是读取所有力通道的系数，取完之后保存在IGBT类中b0、c0……
        /// </summary>
        private bool isGetForceXs = false;
        public bool Get_Force_Modulus()
        {
            try
            {
                isGetForceXs = false;
                List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Read_Force_Modulus).ToList();
                temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                byte[] Cmd = temp_cmd.ToArray();
                SendData(Cmd);
                Thread.Sleep(500);
                return isGetForceXs;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 固化力通道标定系数 BNDT用
        /// <summary>
        /// 固化力通道标定系数 BNDT用
        /// </summary>
        /// <param name="Aisle">int 通道号(1/2/3)</param>
        /// <param name="Modulus">float 系数b</param>
        public void Solidify_Force_Modulus(int Aisle, float Modulus)
        {
            try
            {
                List<byte> temp_cmd = null;
                byte[] Cmd = null;
                if (Aisle < 0 && Aisle > 3)
                    return;
                //设置参数
                switch (Aisle)
                {
                    case 1:
                        temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Force_b0).ToList();
                        break;
                    case 2:
                        temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Force_b1).ToList();
                        break;
                    case 3:
                        temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Force_b2).ToList();
                        break;
                }
                temp_cmd.AddRange(BitConverter.GetBytes(Modulus));
                Cmd = temp_cmd.ToArray();
                SendData(Cmd);
                Thread.Sleep(10);
                temp_cmd = null;
                //固化参数
                temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Solidify_Force_Modulus).ToList();
                temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                Cmd = temp_cmd.ToArray();
                SendData(Cmd);
                Thread.Sleep(10);
                //选择使用固化了系数的通道
                Select_Channel(Aisle);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region 选择使用的力通道(1/2/3) BNDT用
        /// <summary>
        /// 选择使用的力通道(1/2/3) BNDT用
        /// </summary>
        /// <param name="Aisle">int 通道号(1/2/3)</param>
        public void Select_Channel(int Aisle)
        {
            try
            {
                List<byte> temp_cmd = null;
                byte[] Cmd = null;
                if (Aisle < 0 && Aisle > 3)
                    return;
                temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Force_Aisle).ToList();
                temp_cmd.AddRange(BitConverter.GetBytes(Aisle));
                Cmd = temp_cmd.ToArray();
                SendData(Cmd);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region 读取滚筒直径和脉冲数 BNDT用
        /// <summary>
        /// 读取滚筒直径和脉冲数 BNDT用
        /// </summary>
        public void Get_Speed_DiameterandPusle()
        {
            try
            {
                List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Read_Speed_Modulus).ToList();
                temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                byte[] Cmd = temp_cmd.ToArray();
                SendData(Cmd);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region 读取基本惯量和汽车基准质量
        /// <summary>
        /// 基本惯量和汽车基准质量
        /// </summary>
        public void Get_InertnessAndRM()
        {
            try
            {
                List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Read_Inertness).ToList();
                temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
                byte[] Cmd = temp_cmd.ToArray();
                SendData(Cmd);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region 固化测功机基本惯量参数
        public void Solidify_Inertness()
        {
            byte[] Cmd = null;
            List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Solidify_Inertness).ToList();
            temp_cmd.AddRange(BitConverter.GetBytes(0f));
            Cmd = temp_cmd.ToArray();
            SendData(Cmd);
            Thread.Sleep(10);
        }
        #endregion
        #region 打开继电器(1/2/3) BNDT用
        /// <summary>
        /// 选择使用的力通道(1/2/3) BNDT用
        /// </summary>
        /// <param name="Aisle">int 通道号(1/2/3)</param>
        public void TurnOnRelay(byte RelaySelected)
        {
            try
            {
                List<byte> temp_cmd = null;
                byte[] chanel_cmd = { 0, 0, 0, RelaySelected };
                byte[] Cmd = null;
                if (RelaySelected < 0 && RelaySelected > 3)
                    return;
                temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Control_Relay_TurnOn).ToList();
                temp_cmd.AddRange(chanel_cmd.ToList());
                Cmd = temp_cmd.ToArray();
                SendData(Cmd);
            }
            catch (Exception)
            {
            }
        }
        #endregion
        #region 关闭继电器(1/2/3) BNDT用
        /// <summary>
        /// 选择使用的力通道(1/2/3) BNDT用
        /// </summary>
        /// <param name="Aisle">int 通道号(1/2/3)</param>
        public void TurnOffRelay(byte RelaySelected)
        {
            try
            {
                List<byte> temp_cmd = null;
                byte[] chanel_cmd = { 0, 0, 0, RelaySelected };
                byte[] Cmd = null;
                if (RelaySelected < 0 && RelaySelected > 3)
                    return;
                temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Control_Relay_TurnOff).ToList();
                temp_cmd.AddRange(chanel_cmd.ToList());
                Cmd = temp_cmd.ToArray();
                SendData(Cmd);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region 设置恒IGBT值
        /// <summary>
        /// 设置恒速值
        /// </summary>
        /// <param name="Speed">float 速度</param>
        public void Set_Duty(float duty)
        {
            byte[] Cmd = null;
            List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Set_Duty).ToList();
            temp_cmd.AddRange(BitConverter.GetBytes(duty));
            Cmd = temp_cmd.ToArray();
            SendData(Cmd);
        }
        #endregion
        #region 启动恒IGBT控制
        /// <summary>
        /// 启动恒速控制
        /// </summary>
        public void Start_Control_Duty()
        {
            byte[] Cmd = null;
            List<byte> temp_cmd = Encoding.Default.GetBytes(BNTD_Cmd_Control_Start_Duty).ToList();
            temp_cmd.AddRange(BitConverter.GetBytes(0.0f));
            Cmd = temp_cmd.ToArray();
            SendData(Cmd);
            Status = "Duty";           //设置状态为恒速控制
        }
        #endregion
    }
}
