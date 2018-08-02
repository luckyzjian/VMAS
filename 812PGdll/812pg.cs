using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace C812PGdll
{
    public class C812pg
    {
        public byte DO_hb = 0;
        public byte DO_lb = 0;
        public int PdshuziOut;
        public int InputInt;
        public const int ERR_NoError = 0;
        public const int ERR_BoardNoInit = 1;
        public const int ERR_InvalidBoardNumber = 2;
        public const int ERR_InitializedBoardNumber = 3;
        public const int ERR_BaseAddressError = 4;
        public const int ERR_BaseAddressConflict = 5;
        public const int ERR_DuplicateBoardSetting = 6;
        public const int ERR_DuplicateIrqSetting = 7;
        public const int ERR_PortError = 8;
        public const int ERR_ChannelError = 9;
        public const int ERR_InvalidADChannel = 10;
        public const int ERR_InvalidDAChannel = 11;
        public const int ERR_InvalidDIChannel = 12;
        public const int ERR_InvalidDOChannel = 13;
        public const int ERR_InvalidDIOChannel = 14;
        public const int ERR_InvalidIRQChannel = 15;
        public const int ERR_InvalidDMAChannel = 16;
        public const int ERR_InvalidChangeValue = 17;
        public const int ERR_InvalidTimerValue = 18;
        public const int ERR_InvalidTimerMode = 19;
        public const int ERR_InvalidCounterValue = 20;
        public const int ERR_InvalidCounterMode = 21;
        public const int ERR_InvalidADMode = 22;
        public const int ERR_InvalidMode = 23;
        public const int ERR_NotOutputPort = 24;
        public const int ERR_NotInputPort = 25;
        public const int ERR_AD_DMANotSet = 26;
        public const int ERR_AD_INTNotSet = 27;
        public const int ERR_AD_AquireTimeOut = 28;
        public const int ERR_AD_InvalidGain = 29;
        public const int ERR_INTNotSet = 30;
        public const int ERR_InvalidPortNumber = 31;
        public const int ERR_TotalErrorCount = 32;
        //-------------------- possible value of card_number --------------------
        public const int CARD_1 = 0;
        public const int CARD_2 = 1;
        public const int CARD_3 = 2;
        public const int CARD_4 = 3;
        public const int CARD_5 = 4;
        public const int CARD_6 = 5;
        public const int CARD_7 = 6;
        public const int CARD_8 = 7;

        //-------------------- for DI and DO --------------------
        public const int DI_LO_BYTE = 0;   //for port_no of DI (low  byte)
        public const int DI_HI_BYTE = 1;    //for port_no of DI (high byte)
        public const int DO_LO_BYTE = 0;    //for port_no of DO (low  byte)
        public const int DO_HI_BYTE = 1;   //for port_no of DO (high byte)

        //-------------------- for DA Interrupt status --------------------
        public const int DA_INT_STOP = 0;   //Interrupt completed
        public const int DA_INT_RUN = 1;     //Interrupt not completed

        //-------------------- for AD Interrupt status --------------------
        public const int AD_INT_STOP = 0;    //Interrupt completed
        public const int AD_INT_RUN = 1;     //Interrupt not completed

        //-------------------- for AD DMA status --------------------
        public const int AD_DMA_Stop = 0;    //DMA completed
        public const int AD_DMA_RUN = 1;     //DMA not completed


        //-------------------- possible ad_ch_no of AD --------------------
        public const int AD_CH_0 = 0;
        public const int AD_CH_1 = 1;
        public const int AD_CH_2 = 2;
        public const int AD_CH_3 = 3;
        public const int AD_CH_4 = 4;
        public const int AD_CH_5 = 5;
        public const int AD_CH_6 = 6;
        public const int AD_CH_7 = 7;
        public const int AD_CH_8 = 8;
        public const int AD_CH_9 = 9;
        public const int AD_CH_10 = 10;
        public const int AD_CH_11 = 11;
        public const int AD_CH_12 = 12;
        public const int AD_CH_13 = 13;
        public const int AD_CH_14 = 14;
        public const int AD_CH_15 = 15;

        //-------------------- possible da_ch_no of DA --------------------
        public const int DA_CH_1 = 0;
        public const int DA_CH_2 = 1;
        public const int DA_CH_3 = 2;
        public const int DA_CH_4 = 3;
        public const int DA_CH_5 = 4;
        public const int DA_CH_6 = 5;

        //-------------------- for IRQ channel number --------------------
        public const int IRQ3 = 3;
        public const int IRQ4 = 4;
        public const int IRQ5 = 5;
        public const int IRQ6 = 6;
        public const int IRQ7 = 7;
        public const int IRQ9 = 9;
        public const int IRQ10 = 10;
        public const int IRQ11 = 11;
        public const int IRQ12 = 12;
        public const int IRQ15 = 15;

        //-------------------- for DMA channel number --------------------
        public const int DMA_CH_1 = 1;
        public const int DMA_CH_3 = 3;

        //-------------------- AD mode (PCL-812PG) --------------------
        public const int A818_AD_MODE_0 = 0;   //External Trig, Software Polling
        public const int A818_AD_MODE_1 = 1;    //Software Trig, Software Polling
        public const int A818_AD_MODE_2 = 2;   //Timer    Trig, DMA Transfer
        public const int A818_AD_MODE_3 = 3;   //External Trig, DMA Transfer
        public const int A818_AD_MODE_4 = 4;   //External Trig, Soft/Int transfer
        public const int A818_AD_MODE_5 = 5;    //Soft     Trig, Soft/Int transfer
        public const int A818_AD_MODE_6 = 6;    //Timer    Trig, Soft/Int transfer
        public const int A818_AD_MODE_7 = 7;    //Not Used

        //-------------- Mode of Timer #0 (PCL-812PG) ---------------
        public const int TIMER_MODE0 = 0;   //Timer : Terminal Count
        public const int TIMER_MODE1 = 1;   //Timer : Programmer One-shot
        public const int TIMER_MODE2 = 2;   //Timer : Frq.  Generator
        public const int TIMER_MODE3 = 3;   //Timer : Square Wave Generator
        public const int TIMER_MODE4 = 4;   //Timer : Counter, Soft Trigger
        public const int TIMER_MODE5 = 5;  //Timer : Counter, Hard Trigger

        //-------------- Channel Input Mode --------------
        public const int SINGLE_ENDED = 0;
        public const int DIFFERENTIAL = 1;

        //-------------  PCL-818PG -------------------------
        public const int AD_GAIN_1 = 0;
        public const int AD_GAIN_2 = 1;
        public const int AD_GAIN_4 = 2;
        public const int AD_GAIN_8 = 3;
        public const int AD_GAIN_16 = 4;


        int Active;
        int[] status = new int[15];

        [DllImport("812PG.dll", EntryPoint = "W_812PG_Initial")]
        private static extern int W_812PG_Initial(int card_number, int base_address);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_Switch_Card_No")]
        private static extern int W_812PG_Switch_Card_No(int card_number);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_DI")]
        private static extern int W_812PG_DI(int port_number,ref byte di_data);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_DI_Channel")]
        private static extern int W_812PG_DI_Channel(int ch_no, int di_data);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_DO")]
        private static extern int W_812PG_DO(int port_number, byte do_data);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_DA")]
        private static extern int W_812PG_DA(int ch_no, int da_data);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_AD_Input_Mode")]
        private static extern int W_812PG_AD_Input_Mode(int ad_mode);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_AD_Set_Channel")]
        private static extern int W_812PG_AD_Set_Channel(int ch_no);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_AD_Set_Range")]
        private static extern int W_812PG_AD_Set_Range(int range_code);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_AD_Set_Gain")]
        private static extern int W_812PG_AD_Set_Gain(int ad_gain);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_AD_Set_Mode")]
        private static extern int W_812PG_AD_Set_Mode(int ad_mode);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_AD_Soft_Trig")]
        private static extern int W_812PG_AD_Soft_Trig();

        [DllImport("812PG.dll", EntryPoint = "W_812PG_AD_Aquire")]
        private static extern int W_812PG_AD_Aquire(ref int ad_data);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_CLR_IRQ")]
        private static extern int W_812PG_CLR_IRQ();

        [DllImport("812PG.dll", EntryPoint = "W_812PG_AD_DMA_Start")]
        private static extern int W_812PG_AD_DMA_Start(int ad_ch_no, int ad_gain, int dma_ch_no, int irq_ch_no, int Count, int ad_buffer, int C1, int C2);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_AD_DMA_Status")]
        private static extern int W_812PG_AD_DMA_Status(int status, int Count);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_AD_DMA_Stop")]
        private static extern int W_812PG_AD_DMA_Stop(int Count);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_AD_INT_Start")]
        private static extern int W_812PG_AD_INT_Start(int ad_ch_no, int ad_gain, int irq_ch_no, int Count, int ad_buffer, int C1, int C2);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_AD_INT_Status")]
        private static extern int W_812PG_AD_INT_Status(int status, int Count);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_AD_INT_Stop")]
        private static extern int W_812PG_AD_INT_Stop(int Count);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_AD_Timer")]
        private static extern int W_812PG_AD_Timer(int C1, int C2);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_TIMER_Start")]
        private static extern int W_812PG_TIMER_Start(int timer_mode, int c0);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_TIMER_Read")]
        private static extern int W_812PG_TIMER_Read(ref int counter_value);

        [DllImport("812PG.dll", EntryPoint = "W_812PG_TIMER_Stop")]
        private static extern int W_812PG_TIMER_Stop(int counter_value);

      
        /// <summary>
        /// 启动812卡
        /// </summary>
        /// <returns>void</returns>
        public int PopenDevice()
        {
            int a=W_812PG_Initial(CARD_1, 0x220);
            int b=W_812PG_Switch_Card_No(CARD_1);
            int c=W_812PG_AD_Set_Gain(0);
            int DO_lb = 0;
            int DO_hb = 0;
            Init_Device();
            return a;
        }

        /// <summary>
        /// 初始化812
        /// </summary>
        public void Init_Device()
        {
            int s = W_812PG_DO(0, 0);
            int ss = W_812PG_DO(1, 0);
        }

        /// <summary>
        /// 关闭812卡
        /// </summary>
        /// <returns>void</returns>
        public int PcloseDevice()
        {
            return 0;
        }

     

        /// <summary>
        ///获取模拟信号AD值 
        /// </summary>
        /// <param name="Chanel">通道号</param>
        /// <returns>返回AD值</returns>
        public int PaInput(int Chanel)
        {
            int A = 0;
            int a = W_812PG_AD_Set_Channel(Chanel);
            int b= W_812PG_AD_Soft_Trig();
            int c = W_812PG_AD_Aquire(ref A);
            return A;
        }

     
        /// <summary>
        /// 获取数字信号
        /// </summary>
        /// <returns>数字信号</returns>
        public int PdInput()
        {
            byte A = 0;
            int c=W_812PG_DI(DI_LO_BYTE, ref A);
            return A;
        }

  
        /// <summary>
        /// 模拟输出
        /// </summary>
        /// <param name="OutValue">模拟数</param>
        /// <param name="Flag">0</param>
        /// <returns>模拟值</returns>
        public int PdOutput(int OutValue, int Flag = 0)
        {
            if (Flag != 0)
            {
                return OutValue;
            }
            else
            {
                return OutValue;
            }
        }
       /// <summary>
        /// 模拟输出2
       /// </summary>
       /// <param name="OutValue">模拟数</param>
       /// <returns>模拟值</returns>
        public int PdOutput(int OutValue)
        {
            int Flag = 0;
            if (Flag != 0)
            {
                return OutValue;
            }
            else
            {
                return OutValue;
            }
        }

       
        /// <summary>
        /// 时钟数据读取（取车速的值）
        /// </summary>
        /// <returns>返回脉冲信号值</returns>
        public int PCounterRead()
        {
            int A = 0;
            W_812PG_TIMER_Read(ref A);
            return A;
        }

        public bool spflag = true;
        public long Pulsestart = 0;
        public float speedBefore;
        /// <summary>
        /// 获取车速
        /// </summary>
        /// <returns>车速</returns>
        public string getspeed(float time)
        {
            double speed = 0;
            long Pulse = 0;
            if (spflag)
            {
                Pulsestart = PCounterRead();
                spflag = false;
            }
            else
            {
                Pulse = PCounterRead();
                if (Pulsestart >= Pulse)
                {
                    speed = ((((((double)Pulsestart - (double)Pulse) / (double)600) * (double)370 * 3.14159) / (double)100000) * (double)3600);
                    Pulsestart = Pulse;
                    spflag = false;
                }
                else
                {
                    speed = ((((((double)Pulsestart + (double)65535 - (double)Pulse) / (double)600) * (double)370 * 3.14159) / (double)100000) * (double)3600);
                    Pulsestart = Pulse;
                    spflag = false;
                }
            }
           string strspeed= speed.ToString("0.0");
           return strspeed;
        }
   
        /// <summary>
        /// 启动时钟
        /// </summary>
        /// <param name="B">65535</param>
        /// <returns>脉冲</returns>
        public int PCounterStart(int B = 65535)
        {
            int S;
            int A;
            S = W_812PG_TIMER_Stop(B);
            S = W_812PG_TIMER_Start(TIMER_MODE4, B);
            A=PCounterRead();
            return A;
        }
       
        /// <summary>
        /// 启动时钟2
        /// </summary>
        /// <returns>脉冲</returns>
        public int PCounterStart()
        {
            int B = 65535;
            int S;
            int A;
            S = W_812PG_TIMER_Stop(B);
            S = W_812PG_TIMER_Start(TIMER_MODE4, B);
            A=PCounterRead();
            return A;
        }
  
   
        /// <summary>
        /// 获取光电信号
        /// </summary>
        /// <param name="CH">CH -通道号</param>
        /// <returns>返回是否到位</returns>
        public bool GDSwitch(int CH)
        {
            InputInt = PdInput();
            int exchange = ExchangeOn(CH);
            if ((InputInt & exchange) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int ExchangeOn(int A)
        {
            if (A > -1 && A < 16)
            {
                double a = Math.Pow(2, A);
                return (int)a;
            }
            else
            {
                return -1;
            }
        }


       
        //此方法需成对使用
        /// <summary>
        /// 控制继电器开关（启动或停止电机）
        /// </summary>
        /// <param name="Index"></param>
        public void OutPut812Pg(int Index)
        {
            int i, v, d;
            if (status[Index] == 1)
            {
                status[Index] = 0;
                if (Index < 8)
                {
                    d = 1;
                    for (i = 1; i <= Index; i++)
                    {
                        d = d * 2;
                    }
                    DO_lb = (byte)(DO_lb - d);
                    v = W_812PG_DO(0, DO_lb);
                    v = W_812PG_DO(1, DO_hb);
                }
                else
                {
                    d = 1;
                    for (i = 1; i <= Index - 8; i++)
                    {
                        d = d * 2;
                    }
                    DO_hb = (byte)(DO_hb - d);
                    v = W_812PG_DO(1, DO_hb);
                    v = W_812PG_DO(0, DO_lb);
                }
            }
            else
            {
                status[Index] = 1;
                if (Index < 8)
                {
                    d = 1;
                    for (i = 1; i <= Index; i++)
                    {
                        d = d * 2;
                    }
                    DO_lb = (byte)(DO_lb + d);
                    v = W_812PG_DO(0, DO_lb);
                    v = W_812PG_DO(1, DO_hb);
                }
                else
                {
                    d = 1;
                    for (i = 1; i <= Index - 8; i++)
                    {
                        d = d * 2;
                    }
                    DO_hb = (byte)(DO_hb + d);
                    v = W_812PG_DO(1, DO_hb);
                    v = W_812PG_DO(0, DO_lb);
                }
            }
        }


        /// <summary>
        /// 获取传感器的值
        /// </summary>
        /// <param name="r">通道号</param>
        /// <returns>经过一次平滑滤波后的传感器值</returns>
        public double pCalAverage(int r)
        {
            double d = 0.00;
            int S;
            double[] A = new double[400];
            double dblTemp;
            int i;
            int J;
            int C;
            C = A.Length;
            for (i = 0; i <= C - 1; i++)
            {
                S = PaInput(r);
                if (i == 0)
                {
                    A[i] = S;
                }
                else
                {
                    if (A[i - 1] < S)
                    {
                        A[i] = A[i - 1];
                        A[i - 1] = S;
                        for (J = i - 1; J >= 1; J--)
                        {
                            if (A[J] > A[J - 1])
                            {
                                dblTemp = A[J];
                                A[J] = A[J - 1];
                                A[J - 1] = dblTemp;
                            }
                            else
                            {
                                A[i] = S;
                            }
                        }
                    }
                }
            }
            int lngCount;
            dblTemp = 0;
            lngCount = C - 99;
            if (C > 40)
            {
                for (i = 50; i <= C - 50; i++)
                {
                    if ((A[i] == A[50]) | (A[i] == A[C - 50]))
                    {
                        lngCount = lngCount - 1;
                    }
                    else
                    {
                        dblTemp = dblTemp + A[i];
                    }
                }
                if (lngCount == 0)
                {
                    d = A[A.Length / 2];
                }
                else
                {
                    d = dblTemp / lngCount;
                }
            }
            else
            {
                d = 0.00;
            }
            return d;
        }

        /// <summary>
        /// 812PG模拟PWM信号控制涡流机加载，频率50HZ
        /// </summary>
        /// <param name="Multiple">系数，越大表示高电平时间越长，涡流机加载功率越大，不能大于2</param>
        /// <param name="time">加载时间</param>
        /// <param name="CH">连接涡流机的信号通道</param>
        public void PWM_Signal(double Multiple,float time,int CH)
        {
            int max = (int)time / 20;                       //计算循环次数
            int H_Time = (int)Math.Round(10 * Multiple);    //计算高电平时间四舍五入
            int L_Time = 20 - H_Time;
            for (int i = 0; i < max; i++)
            {
                W_812PG_DA(CH, 4095);
                Thread.Sleep(H_Time);
                W_812PG_DA(CH, 0);
                Thread.Sleep(L_Time);
            }
        }

        /// <summary>
        /// 关闭测功机加载
        /// </summary>
        /// <param name="CH">连接涡流机的信号通道</param>
        public void PWM_OFF_JZ(int CH)
        {
            W_812PG_DA(CH, 0);
        }
    }
}



