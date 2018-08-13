using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using carinfor;
using Exhaust;
using Dynamometer;
using System.Threading;
using SarchPMS.Business.Draw;
using System.IO;
using System.Diagnostics;


namespace 动力性测量
{
    public partial class dynamicTest : Form
    {
        CSVcontrol.CSVwriter csvwriter = new CSVcontrol.CSVwriter();
        CSVcontrol.csvReader csvreader = new CSVcontrol.csvReader();
        private yhControl yhy = null;
        private IGBT igbt=null;
        LedControl.BX5k1 ledcontrol = null;
        private yhcarInidata carinfo = new yhcarInidata();
        private carIni carini = new carIni();
        private equipmentConfigInfdata equipdata = new equipmentConfigInfdata();
        private configIni configini = new configIni();
        private DynConfigInfdata dynconfigdata = new DynConfigInfdata();
        public delegate void wtlsb(Label Msgowner, string Msgstr, bool Update_DB);                //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                              //委托
        public delegate void wtlm(Label Msgowner, string Msgstr);
        public delegate void wtTextboxText(TextBox Msgowner, string Msgstr);
        private Point panelDetailsData = new Point(2, 84);
        private Point panelDetailsInf = new Point(0, 0);
        private string UseFqy = "";
        Exhaust.Fla501 fla_501 = null;
        Exhaust.Fla502 fla_502 = null;
        Exhaust.FLB_100 flb_100 = null;
        Exhaust.Flv_1000 flv_1000 = null;
        Exhaust.VMT_2000 vmt_2000 = null;
        Exhaust.RPM5300 rpm5300 = null;
        Exhaust.XCE_100 xce_100 = null;
        bool isUseRotater = false;
        public dynamicTest()
        {
            InitializeComponent();
            this.Load += new EventHandler(DrawPoint_Load);
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }
        Font sfont;
        Bitmap bmpSpeed,bmpForce;
        Graphics grpSpeed,grpForce;
        Pen pen;
        private void DrawPoint_Load(object sender, EventArgs e)
        {
            bmpSpeed = new Bitmap(pictureBox速度.Width, pictureBox速度.Height);
            grpSpeed = Graphics.FromImage(bmpSpeed);
            grpSpeed.Clear(pictureBox速度.BackColor);
            bmpForce = new Bitmap(pictureBox扭力.Width, pictureBox扭力.Height);
            grpForce = Graphics.FromImage(bmpForce);
            grpForce.Clear(pictureBox扭力.BackColor);
            pen = new Pen(Color.Black, 1);
            sfont = new Font("宋体", 10.5f);
        }
        #region 汽车燃料消耗检测工况相关参数计算
        //汽车燃料消耗检测工况下的道路行驶阻力：FR=Ft+Fw        
        private double distance = 0;//记录行驶距离,m
        private double totalDistance = 0;
        private double totalYh = 0;
        private double yhPerHundred = 0;
        private string pdjgstring;
        private double yhXz = 0;
        private int testTimes = 0;//第几次检测
        private double Y_FR = 0, Y_Ft = 0, Y_Fw = 0;
        //汽车道路行驶的滚动阻力：Ft=G*g*f,G为受检车辆额定总质量（或牵引车单车满载总质量)
        //f:滚动阻力系数，汽车以50km/h,60km/h速度在水平面行驶的滚动阻力系数f
        //|-------------轮胎--------------|----------f
        //|子午胎------轮胎断面宽度<8.25in|---------0.007
        //|子午胎-----轮胎断面宽度>=8.25in|---------0.006
        //|------------斜交胎-------------|---------0.010
        private double g = 9.81, f = 0.007;
        //------------------------------------------|---------------------------------------
        //汽车道路行驶空气阻力：Fw=1/2*Cd*A*p*(v0/3.6)*(v0/3.6)
        //------------------------------------------|---------------------------------------
        //   Cd          营运客车                   |                  营运货车
        //   车长L     等速60km/h      等速50km/h   |    车身型式     额定总质量    等速50
        //------------------------------------------|---------------------------------------
        //                                          |      拦板车      G<10000        0.9
        //   L<=7000       0.6             0.65     |      自卸车       
        //                                          |      牵引车      G>=10000       1.1
        //------------------------------------------|---------------------------------------
        //7000<L<=9000     0.7             0.75     |      仓栅车                     1.4
        //------------------------------------------|---------------------------------------
        //                                          |      厢式车      G<10000        0.8
        //   L>7000        0.8             0.85     |               10000<=G<15000    0.9
        //                                          |      罐  车      G>=15000       1.0
        //------------------------------------------|---------------------------------------
        //A为迎风面积 A=B*H*10(-6)   B:汽车前轮距(mm) H：汽车高度(mm)
        private double Cd = 0, A = 0, p = 1.189, v0 = 50.0;
        //汽车台架运转阻力等于汽车台架滚动阻力和台架内阻之和：Fc=Ffc+Ftc
        //Ftc为台架内阻，可使用推荐值 ，也可以使用滑行法进行测定
        //----------------------|----------------------------|-----------------------------
        //        速度          |   二轴四滚筒台架内阻Ftc    |  三轴六滚筒台架内阻Ftc    
        //----------------------|----------------------------|-----------------------------
        //         50           |               100          |           130  
        //----------------------|----------------------------|-----------------------------
        //         60           |               110          |           140
        //----------------------|----------------------------|-----------------------------
        private double Y_Fc = 0, Y_Ffc = 0, Y_Ftc = 0;
        //Ffc=Gr*g*fc   fc=1.5*f  Gr为驱动轴空载质量
        private double Gr = 0, fc = 0;


        //台架加载阻力 FTC=Fr-Fc
        private double Y_FTC = 0;

        /// <summary>
        /// 计算油耗测试工况时台架加载阻力
        /// </summary>
        private void caculateFTC()
        {
            if (carinfo.汽车类型 == 0)//营运客车
            {
                if (carinfo.客车等级 == 0)//高级
                {
                    v0 = 60;
                }
                else
                {
                    v0 = 50;
                }
            }
            else//营运货车
            {
                v0 = 50;
            }
            if (carinfo.汽车类型 == 0)//营运客车
            {
                if (carinfo.客车车长 <= 7000)//高级
                {
                    if (v0 == 60)
                        Cd = 0.60;
                    else
                        Cd = 0.65;
                }
                else if (carinfo.客车车长 > 9000)
                {
                    if (v0 == 60)
                        Cd = 0.80;
                    else
                        Cd = 0.85;
                }
                else
                {
                    if (v0 == 60)
                        Cd = 0.70;
                    else
                        Cd = 0.75;
                }
            }
            else//营运货车
            {
                if (carinfo.货车车身型式 == 0 || carinfo.货车车身型式 == 1 || carinfo.货车车身型式 == 2)
                {
                    if (carinfo.额定总质量 < 10000)
                    {
                        Cd = 0.9;
                    }
                    else
                    {
                        Cd = 1.1;
                    }
                }
                else if (carinfo.货车车身型式 == 3)
                {
                    Cd = 1.4;
                }
                else if (carinfo.货车车身型式 == 4 || carinfo.货车车身型式 == 5)
                {
                    if (carinfo.额定总质量 < 10000)
                    {
                        Cd = 0.8;
                    }
                    else if (carinfo.额定总质量 >= 15000)
                    {
                        Cd = 1.0;
                    }
                    else
                    {
                        Cd = 0.9;
                    }
                }
                
            }
            if (equipdata.CgjNz == 0)//测功机内阻使用经验值
            {
                if (v0 == 60)
                {
                    if (carinfo.驱动轴数 == 1)
                    {
                        if (equipdata.Cgjxh== "DCG-26L|DCG-26LD")
                        {
                            Y_Ftc = 140;
                        }
                        else
                            Y_Ftc = 110;
                    }
                    else
                    {
                        Y_Ftc = 140;
                    }
                }
                else
                {
                    if (carinfo.驱动轴数 == 1)
                    {

                        if (equipdata.Cgjxh == "DCG-26L|DCG-26LD")
                        {
                            Y_Ftc = 130;
                        }
                        else
                            Y_Ftc = 100;
                    }
                    else
                    {
                        Y_Ftc = 130;
                    }
                }
            }
            else//使用反拖法测定的值
            {
                Y_Ftc = configini.getCgjNz(v0.ToString("0"));
            }
            if (carinfo.轮胎类型 == 0)
            {
                if (carinfo.子午胎轮胎断面宽度 < 8.25) f = 0.007;
                else f = 0.006;
            }
            else
            {
                f = 0.010;
            }
            Y_Ft = carinfo.额定总质量 * g * f;
            A = carinfo.汽车前轮距 * carinfo.汽车高度 * 0.000001;
            Y_Fw = 0.5 * Cd * A * p * (v0 / 3.6) * (v0 / 3.6);
            Y_FR = Y_Ft + Y_Fw;
            fc = 1.5 * f;
            Y_Ffc = carinfo.驱动轴空载质量 * g * fc;
            Y_Fc = Y_Ftc + Y_Ffc;
            Y_FTC = Y_FR - Y_Fc;
            Y_FTC = Math.Round(Y_FTC, 0);//台架加载阻力，四舍五入至整数位，单位为N
        }
        #endregion
        #region 性能动力测试相关参数计算
        //X_FE=X_Fe-X_Ftc-X_Fc-X_Ff-X_Ft
        //Fc=fc*Gr*g
        private double X_FE = 0,X_Fe=0, X_Ftc = 0, X_Fc = 0, X_Ff = 0, X_Ft = 0;//柴油车使用
        private double X_FM = 0, X_Fm = 0;//汽油车使用
        //X_Fe=(3600*η*Pe)/(αd*Ve)
        //Pe:发动机功率 kW
        //η:0.75
        //αd:压燃式发动机功率校正系数，发动机因子fm=0.3
        private double η = 0.75, Pe = 0, αd = 0, αa = 0,fm=0.3;
        //Ff=(3600*fp*Pe)/Ve
        //fp:Ve车速点，发动附件消耗功率系数，当发动机铭牌功率参数以额定功率表征时，fp=0.1，以车辆铭牌最大净功率表征时，fp取0
        private double fp = 0;
        //Ft=0.18*(Fe-Ff)
        /// <summary>
        /// 驱动轮轮边稳定车速
        /// </summary>
        private double Vw = 0;
        /// <summary>
        /// 额定功率车速Ve=0.86*Va
        /// </summary>
        private double Ve = 0;
        /// <summary>
        /// 全油门所挂挡位的最高稳定车速，单位为千米每小时（km/h）
        /// </summary>
        private double Va = 0;
        private double Wd = 0, Sd = 0, Dqy = 0;
        private double[,] 水蒸气分压 =
        { 
            { 0.3,0.2,0.2,0.1,0.1 },
            { 0.4,0.3,0.2,0.2,0.1} ,
            { 0.6,0.5,0.4,0.2,0.1} ,
            { 0.9,0.7,0.5,0.4,0.2} ,

            { 1.2,1.0,0.7,0.5,0.2} ,
            { 1.7,1.4,1.0,0.7,0.5} ,
            { 2.3,1.9,1.4,0.9,0.5} ,
            { 3.2,2.5,1.9,1.3,0.6} ,

            { 3.6,2.9,2.1,1.4,0.7} ,
            { 4.2,3.4,2.5,1.7,0.9} ,
            { 4.8,3.8,2.9,1.9,1.0} ,
            { 5.3,4.3,3.2,2.1,1.1} ,

            { 6.0,4.8,3.6,2.6,1.2} ,
            { 6.6,5.3,4.0,2.7,1.3} ,
            { 7.4,5.9,4.4,3.0,1.5} ,
            { 8.2,6.6,4.9,3.3,1.6} ,

            { 9.1,7.3,5.5,3.6,1.8} ,
            { 10.1,8.1,6.1,4.0,2.0} ,
            { 11.2,8.9,6.7,4.5,2.2} ,
            { 12.3,9.9,7.4,4.9,2.5} ,
        };
        private List<double> templist = new List<double>{-10,-5,0,5,10,15,20,25,27,30,32,34,36,38,40,42,44,46,48,50 };
        private List<double> humilist = new List<double> { 100, 80, 60, 40, 20 };
        private int GetRow(double temp)
        {
            if (temp <= templist[0]) return 0;
            else if (temp >= templist[templist.Count - 1]) return templist.Count - 1;
            else
            {
                for (int i = 0; i < templist.Count - 1; i++)
                {
                    if ((temp - templist[i]) * (temp - templist[i +1]) <= 0)
                    {
                        if (Math.Abs(temp - templist[i]) < Math.Abs(temp - templist[i + 1]))
                            return i;
                        else
                            return i + 1;
                    }
                }
                return templist.Count - 1;
            }
        }
        private int GetColumn(double humidity)
        {
            if (humidity >= humilist[0]) return 0;
            else if (humidity <= humilist[humilist.Count - 1]) return humilist.Count - 1;
            else
            {
                for (int i = 0; i < humilist.Count - 1; i++)
                {
                    if ((humidity - humilist[i]) * (humidity - humilist[i + 1]) <= 0)
                    {
                        if (Math.Abs(humidity - humilist[i]) < Math.Abs(humidity - humilist[i + 1]))
                            return i;
                        else
                            return i + 1;
                    }
                }
                return humilist.Count - 1;
            }
        }
        private double fa = 0;//大气因子
        /// <summary>
        /// 柴油机校正系数计算
        /// </summary>
        private void caculateαd()
        {
            int rownumber = GetRow(Wd);
            int columnnumber = GetColumn(Sd);
            φPaw = 水蒸气分压[rownumber, columnnumber];
            pa = Dqy - φPaw;
            fa =(99.0/pa) * Math.Pow((Wd + 273.15) / 298, 0.7);
            αd = Math.Pow(fa, fm);
        }
        /// <summary>
        /// 汽油机校正系数计算
        /// </summary>
        private double pa = 0, φPaw = 0;
        private void caculateαa()
        {
            int rownumber = GetRow(Wd);
            int columnnumber = GetColumn(Sd);
            φPaw = 水蒸气分压[rownumber, columnnumber];
            pa = Dqy - φPaw;
            αa = Math.Pow(99.0 / pa, 1.2) * Math.Pow((Wd+273.15) / 298, 0.6);
        }
        private void caculateCyFE()
        {
            if (carinfo.动力检测类型 == 0)
            {
                η = 0.75;
            }
            else if (carinfo.动力检测类型 == 1)
            {
                if (levelJudgeStep == 1)
                    η = 0.82;
                else
                    η = 0.75;
            }
            else if (carinfo.动力检测类型 == 3)
            {
                η = 0.82;
            }
            else if (carinfo.动力检测类型 == 2)
            {
                η = 0.75;
            }
            Pe = carinfo.压燃式额定功率;
            Gr = carinfo.驱动轴空载质量;
            caculateαd();
            X_Fe = (3600.0 * η * Pe) / (αd * Ve);
            if (equipdata.CgjNz == 0)//测功机内阻使用经验值
            {
                if (carinfo.燃料种类 == 0)
                {
                    if (carinfo.驱动轴数 == 1)
                    {
                        if (equipdata.Cgjxh == "DCG-26L|DCG-26LD")
                        {
                            X_Ftc = 140;
                        }
                        else
                            X_Ftc = 110;
                    }
                    else
                    {
                        X_Ftc = 140;
                    }
                }
                else
                {
                    if (carinfo.驱动轴数 == 1)
                    {
                        if (equipdata.Cgjxh == "DCG-26L|DCG-26LD")
                        {
                            X_Ftc = 160;
                        }
                        else
                            X_Ftc = 130;
                    }
                    else
                    {
                        X_Ftc = 160;
                    }
                }
            }
            else//使用反拖法测定的值
            {
                X_Ftc = configini.getCgjNz("80");
            }
            if (carinfo.轮胎类型 == 0)
            {
                f = 0.006;
            }
            else
            {
                f = 0.010;
            }
            if(Ve>=70)
                fc = 2.0 * f;
            else
                fc = 1.5 * f;
            X_Fc = fc * Gr * g;
            if (carinfo.压燃式功率参数类型 == 0)
                fp = 0.1;
            else
                fp = 0;
            X_Ff = 3600 * fp * Pe / Ve;
            X_Ft = 0.18 * (X_Fe - X_Ff);
            X_FE = X_Fe - X_Ftc - X_Fc - X_Ff - X_Ft;

            if (dynconfigdata.DynStopUnstable)
            {
                dynconfigdata.DynForceQj = DynForceQj+ (float)(X_FE*0.03);
            }
            showTextboxText(textBoxCy_Ve, Ve.ToString("0.0"));
            showTextboxText(textBoxCy_Pe, Pe.ToString("0"));
            showTextboxText(textBoxCy_η, η.ToString("0.000"));
            showTextboxText(textBoxCy_αd, αd.ToString("0.000"));
            showTextboxText(textBoxCy_Fe, X_Fe.ToString("0"));
            showTextboxText(textBoxCy_Ftc, X_Ftc.ToString("0"));
            showTextboxText(textBoxCy_f, f.ToString("0.000"));
            showTextboxText(textBoxCy_fc, fc.ToString("0.000"));
            showTextboxText(textBoxCy_Gr, Gr.ToString("0"));
            showTextboxText(textBoxCy_轮胎滚动阻力, X_Fc.ToString("0"));
            showTextboxText(textBoxCy_fp, fp.ToString("0.000"));
            showTextboxText(textBoxCy_Ff, X_Ff.ToString("0"));
            showTextboxText(textBoxCy_Ft, X_Ft.ToString("0"));
            showTextboxText(textBoxCy_加载力, X_FE.ToString("0"));
        }
        private void writeDieselResult()
        {
            DieselDynamicResult resultdata = new DieselDynamicResult();
            resultdata.检测类型 = carinfo.检测类型;
            resultdata.车速 = speedTestResult;
            resultdata.车速判定结果 = speedTestPdjg;
            resultdata.功率比值系数 = η;
            resultdata.额定功率 = Pe;
            resultdata.额定功率车速 = Ve;
            resultdata.稳定车速 = Vw;
            resultdata.台架滚动阻力 = X_Fe;
            resultdata.加载力 = X_FE;
            resultdata.测功机内阻 = X_Ftc;
            resultdata.轮胎滚动阻力 = X_Fc;
            resultdata.发动机附件阻力 = X_Ff;
            resultdata.车辆传动系允许阻力 = X_Ft;
            resultdata.功率校正系数 = αd;
            resultdata.台架滚动阻力系数 = fc;
            resultdata.温度 = Wd;
            resultdata.湿度 = Sd;
            resultdata.大气压 = Dqy;
            resultdata.判定结果 =(pdjgstring == "合格"?0:1);
            if (carinfo.动力检测类型 == 0)
                resultdata.等级评定结果 = 0;
            else if (carinfo.动力检测类型 == 1)
                resultdata.等级评定结果 = levelJudgeStep;
            else if (carinfo.动力检测类型 == 3)
                resultdata.等级评定结果 = 1;
            else
                resultdata.等级评定结果 = 2;
            showTextboxText(textBoxVa, Va.ToString("0.0"));
            showTextboxText(textBoxVw, Vw.ToString("0.0"));
            showTextboxText(textBoxVmOrVe, Ve.ToString("0.0"));
            showTextboxText(textBox判定, pdjgstring);
            carini.writeDieselDynamicResult(resultdata);
        }
        private double Vm = 0,nm=0;
        /// <summary>
        /// 发动机额定扭矩
        /// </summary>
        private double Mm = 0;
        private void caculateQyFE()
        {
            if (carinfo.动力检测类型 == 0)
            {
                η = 0.75;
            }
            else if (carinfo.动力检测类型 == 1)
            {
                if (levelJudgeStep == 1)
                    η = 0.82;
                else
                    η = 0.75;
            }
            else if (carinfo.动力检测类型 == 3)
            {
                η = 0.82;
            }
            else if (carinfo.动力检测类型 == 2)
            {
                η = 0.75;
            }
            Mm = carinfo.点燃式额定扭矩;
            if (carinfo.点燃式额定扭矩转速 > 4000)
                nm = 4000;
            else
                nm = carinfo.点燃式额定扭矩转速;
            Gr = carinfo.驱动轴空载质量;
            fm = 0.06;
            caculateαa();
            X_Fm = (0.377* η * Mm*nm) / (αa * Vm);
            if (equipdata.CgjNz == 0)//测功机内阻使用经验值
            {
                if (carinfo.燃料种类 == 0)
                {
                    if (carinfo.驱动轴数 == 1)
                    {
                        if (equipdata.Cgjxh == "DCG-26L|DCG-26LD")
                        {
                            X_Ftc = 140;
                        }
                        else
                            X_Ftc = 110;
                    }
                    else
                    {
                        X_Ftc = 140;
                    }
                }
                else
                {
                    if (carinfo.驱动轴数 == 1)
                    {
                        if (equipdata.Cgjxh == "DCG-26L|DCG-26LD")
                        {
                            X_Ftc = 160;
                        }
                        else
                            X_Ftc = 130;
                    }
                    else
                    {
                        X_Ftc = 160;
                    }
                }
            }
            else//使用反拖法测定的值
            {
                X_Ftc = configini.getCgjNz("50");
            }
            if (carinfo.轮胎类型 == 0)
            {
                f = 0.006;
            }
            else
            {
                f = 0.010;
            }
            if (Ve >= 70)
                fc = 2.0 * f;
            else
                fc = 1.5 * f;
            X_Fc = fc * Gr * g;
            X_Ff = 0.377 * fm * Mm*nm / Vm;
            X_Ft = 0.18 * (X_Fm - X_Ff);
            X_FM = X_Fm - X_Ftc - X_Fc - X_Ff - X_Ft;
            if (dynconfigdata.DynStopUnstable)
            {
                dynconfigdata.DynForceQj = DynForceQj + (float)(X_FM * 0.03);
            }
            showTextboxText(textBoxQy_Vm, Vm.ToString("0.0"));
            showTextboxText(textBoxQy_nm, nm.ToString("0"));
            showTextboxText(textBoxQy_η, η.ToString("0.000"));
            showTextboxText(textBoxQy_Mm, Mm.ToString("0"));
            showTextboxText(textBoxQy_αa, αa.ToString("0.000"));
            showTextboxText(textBoxQy_Fm, X_Fm.ToString("0"));
            showTextboxText(textBoxQy_Ftc, X_Ftc.ToString("0"));
            showTextboxText(textBoxQy_f, f.ToString("0.000"));
            showTextboxText(textBoxQy_fc, fc.ToString("0.000"));
            showTextboxText(textBoxQy_Gr, Gr.ToString("0"));
            showTextboxText(textBoxQy_轮胎滚动阻力, X_Fc.ToString("0"));
            showTextboxText(textBoxQy_发动机附件消耗扭矩系数, fm.ToString("0.000"));
            showTextboxText(textBoxQy_Ff, X_Ff.ToString("0"));
            showTextboxText(textBoxQy_Ft, X_Ft.ToString("0"));
            showTextboxText(textBoxQy_加载力, X_FM.ToString("0"));
        }

        private void writeGasolineResult()
        {
            GasolineDynamicResult resultdata = new GasolineDynamicResult();
            resultdata.检测类型 = carinfo.检测类型;
            resultdata.车速 = speedTestResult;
            resultdata.车速判定结果 = speedTestPdjg;
            resultdata.功率比值系数 = η;
            resultdata.发动机额定扭矩 = Mm;
            resultdata.额定扭矩车速 = Vm;
            resultdata.额定扭矩转速 = nm;
            resultdata.稳定车速 = Vw;
            resultdata.加载力 = X_FM;
            resultdata.发动机达标扭矩驱动力 = X_Fm;
            resultdata.测功机内阻 = X_Ftc;
            resultdata.轮胎滚动阻力 = X_Fc;
            resultdata.发动机附件阻力 = X_Ff;
            resultdata.车辆传动系允许阻力 = X_Ft;
            resultdata.功率校正系数 = αa;
            resultdata.台架滚动阻力系数 = fc;
            resultdata.温度 = Wd;
            resultdata.湿度 = Sd;
            resultdata.大气压 = Dqy;
            resultdata.判定结果 = (pdjgstring == "合格" ? 0 : 1);
            if (carinfo.动力检测类型 == 0)
                resultdata.等级评定结果 = 0;
            else if (carinfo.动力检测类型 == 1)
                resultdata.等级评定结果 = levelJudgeStep;
            else if (carinfo.动力检测类型 == 3)
                resultdata.等级评定结果 = 1;
            else
                resultdata.等级评定结果 = 2;
            showTextboxText(textBoxVa,"--");
            showTextboxText(textBoxVw, Vw.ToString("0.0"));
            showTextboxText(textBoxVmOrVe, Vm.ToString("0.0"));
            showTextboxText(textBox判定, pdjgstring);
            carini.writeGasolineDynamicResult(resultdata);
        }
        private void writeSpeedTestResult()
        {
            GasolineDynamicResult resultdata = new GasolineDynamicResult();
            resultdata.检测类型 = carinfo.检测类型;
            resultdata.车速 = speedTestResult;
            resultdata.车速判定结果 = speedTestPdjg;
            resultdata.功率比值系数 = 0;
            resultdata.发动机额定扭矩 = 0;
            resultdata.额定扭矩车速 = 0;
            resultdata.额定扭矩转速 = 0;
            resultdata.稳定车速 = 0;
            resultdata.加载力 = 0;
            resultdata.发动机达标扭矩驱动力 = 0;
            resultdata.测功机内阻 = 0;
            resultdata.轮胎滚动阻力 = 0;
            resultdata.发动机附件阻力 = 0;
            resultdata.车辆传动系允许阻力 = 0;
            resultdata.功率校正系数 = 0;
            resultdata.台架滚动阻力系数 = 0;
            resultdata.温度 = 0;
            resultdata.湿度 = 0;
            resultdata.大气压 = 0;
            resultdata.判定结果 =0;
            resultdata.等级评定结果 = 0;
            carini.writeGasolineDynamicResult(resultdata);
        }
        #endregion

        /// 设置在第几个屏幕上启动。
        /// </summary>
        /// <param name="screen">屏幕(从0开始)</param>
        /// <param name="form">要启动的程序。</param>
        public void FormStartScreen(int screen, Form form)
        {
            if (Screen.AllScreens.Length <= 1)
                return;
            if (Screen.AllScreens.Length < screen)
                return;
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new System.Drawing.Point(Screen.AllScreens[screen].Bounds.X, Screen.AllScreens[screen].Bounds.Y);
            form.WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// 默认在第1一个扩展屏幕上打开。
        /// </summary>
        /// <param name="form">要启动的程序。</param>
        public void FormStartScreen(Form form)
        {
            FormStartScreen(1, form);
        }
        DriverShow showform = null;
        private bool IsUseTpTemp = false;
        public static bool isTestSpeedPerod = false;
        public float DynForceQj = 0;
        private void resetStatus()
        {
            testStatus teststatus = new testStatus();
            teststatus.OutlookID = carinfo.检测流水号;
            teststatus.Clph = carinfo.车辆号牌;
            teststatus.Hpzl = carinfo.号牌种类;
            teststatus.Code = "0";
            teststatus.Message = "准备" ;
            carini.writeStatusData(teststatus);
        }
        private void yhTest_Load(object sender, EventArgs e)
        {
           
            //aquaGauge1.Value = 65f;
            //aquaGaugeRev.Value = 4500;
            carinfo = carini.getYhCarIni();
            resetStatus();
            initCurve();
            equipdata = configini.getEquipConfigIni();
            dynconfigdata = configini.getDynConfigIni();
            DynForceQj = dynconfigdata.DynForceQj;
            initEquipment();
            starttime = DateTime.Now;
            //showCurve = true;
            timer2.Start();
            if (equipdata.DisplayMethod == "扩展")
            {
                if (equipdata.DriverFbl == 0)
                {
                    showform = new DriverShow();
                    FormStartScreen(equipdata.DriverDisplay, showform);
                    showform.Show();
                }
                else
                {
                    showform = new DriverShow();
                    FormStartScreen(equipdata.DriverDisplay, showform);
                    showform.Show();
                }
            }
            if (carinfo.车辆号牌 == "")
            {
                //Msg(labelCPH, panelCPH, "-----", false);
                showCarInf();
                Msg(labelTS, panelTS, "无待检车辆", true);
                showLed("无待检车辆信息　", "　　　　　　　　");
            }
            else
            {
                //caculateFTC();
                //Msg(labelCPH, panelCPH, carinfo.车辆号牌.ToUpper(), false);
                showCarInf();
                Msg(labelTS, panelTS, "请驶上线准备", true);
                showLed(carinfo.车辆号牌, "请驶上线准备　");
            }
            if (equipdata.isTpTempInstrument)
            {
                if (File.Exists("C://jcdatatxt/环境数据.ini"))
                {
                    //string wd, sd, dqy;
                    StringBuilder temp = new StringBuilder();
                    temp.Length = 2048;
                    ini.INIIO.GetPrivateProfileString("环境数据", "wd", "", temp, 2048, "C:/jcdatatxt/环境数据.ini");
                    Wd = double.Parse(temp.ToString());
                    ini.INIIO.GetPrivateProfileString("环境数据", "sd", "", temp, 2048, "C:/jcdatatxt/环境数据.ini");
                    Sd = double.Parse(temp.ToString());
                    ini.INIIO.GetPrivateProfileString("环境数据", "dqy", "", temp, 2048, "C:/jcdatatxt/环境数据.ini");
                    Dqy = double.Parse(temp.ToString());
                    IsUseTpTemp = true;
                }
            }

            if (equipdata.TempInstrument=="模拟")
            {
                if (File.Exists(Application.StartupPath+"/环境数据.ini"))
                {
                    //string wd, sd, dqy;
                    StringBuilder temp = new StringBuilder();
                    temp.Length = 2048;
                    ini.INIIO.GetPrivateProfileString("环境数据", "wd", "", temp, 2048, Application.StartupPath + "/环境数据.ini");
                    Wd = double.Parse(temp.ToString());
                    ini.INIIO.GetPrivateProfileString("环境数据", "sd", "", temp, 2048, Application.StartupPath + "/环境数据.ini");
                    Sd = double.Parse(temp.ToString());
                    ini.INIIO.GetPrivateProfileString("环境数据", "dqy", "", temp, 2048, Application.StartupPath + "/环境数据.ini");
                    Dqy = double.Parse(temp.ToString());
                    IsUseTpTemp = true;
                }
            }
            if (equipdata.WorkAutomaticMode)
                button1_Click(sender, e);
        }
        public void showLedSingleRow(int lx, string ledstring)
        {
            if (ledcontrol != null)
            {
                if (lx == 1)
                    ledcontrol.writeLed(ledstring, 2, equipdata.Ledxh);
                else if (lx == 2)
                    ledcontrol.writeLed(ledstring, 5, equipdata.Ledxh);
            }
        }
        public void showLed(string ledstring1, string ledstring2)
        {
            if (ledcontrol != null)
            {
                ledcontrol.writeLed(ledstring1, 2, equipdata.Ledxh);
                Thread.Sleep(200);
                ledcontrol.writeLed(ledstring2, 5, equipdata.Ledxh);
            }
        }
        private void showCarInf()
        {
            if (carinfo.燃料种类 == 0)//点燃式
            {
                panel汽油.Visible = true;
                panel汽油.Location = panelDetailsInf;
                panel柴油.Visible = false;
                panelCy.Visible = false;
                panelQy.Visible = true;
                panelQy.Location = panelDetailsData;
                textBox车牌号.Text = carinfo.车辆号牌;
                textBox号牌种类.Text = carinfo.号牌种类;
                textBox额定扭矩.Text = carinfo.点燃式额定扭矩.ToString("0");
                textBox额定扭矩转速.Text =carinfo.点燃式额定扭矩转速.ToString("0");
                switch(carinfo.燃料种类)
                {
                    case 0:textBox燃料种类.Text = "汽油"; break;
                    case 1: textBox燃料种类.Text = "柴油"; break;
                    default: break;
                }
                textBox总质量.Text = carinfo.额定总质量.ToString();
                textBox驱动轴数.Text = carinfo.驱动轴数.ToString();
                textBox驱动轴空载质量.Text = carinfo.驱动轴空载质量.ToString();
                switch (carinfo.轮胎类型)
                {
                    case 0: textBox轮胎规格.Text = "子午线"; break;
                    case 1: textBox轮胎规格.Text = "斜交"; break;
                    default: break;
                }
                textBox车辆型号.Text = carinfo.车辆型号.ToString();
            }
            else
            {
                panel汽油.Visible = false;
                panel柴油.Visible = true;
                panel柴油.Location = panelDetailsInf;
                panelCy.Visible = true;
                panelQy.Visible = false;
                panelCy.Location = panelDetailsData;
                textBox货车车牌号.Text = carinfo.车辆号牌;
                textBox货车号牌种类.Text = carinfo.号牌种类;
                textBox货车额定功率.Text = carinfo.压燃式额定功率.ToString("0");
                switch (carinfo.压燃式功率参数类型)
                {
                    case 0: textBox货车功率参数.Text = "额定功率表示"; break;
                    case 1: textBox货车功率参数.Text = "最大净功率表征"; break;
                    default: break;
                }
                switch (carinfo.是否危险货物运输车辆)
                {
                    case 0: textBox危险货物运输车.Text = "否"; break;
                    case 1: textBox危险货物运输车.Text = "是"; break;
                    default: break;
                }
                switch (carinfo.燃料种类)
                {
                    case 0: textBox货车燃料种类.Text = "汽油"; break;
                    case 1: textBox货车燃料种类.Text = "柴油"; break;
                    default: break;
                }
                textBox货车总质量.Text = carinfo.额定总质量.ToString();
                textBox货车驱动轴数.Text = carinfo.驱动轴数.ToString();
                textBox货车驱动轴质量.Text = carinfo.驱动轴空载质量.ToString();

                switch (carinfo.轮胎类型)
                {
                    case 0: textBox货车轮胎规格.Text = "子午线"; break;
                    case 1: textBox货车轮胎规格.Text = "斜交"; break;
                    default: break;
                }
                textBox货车车辆型号.Text = carinfo.车辆型号.ToString();
            }
            if (carinfo.动力检测类型 == 0) labelTestType.Text = "达标检测";
            else if(carinfo.动力检测类型 == 1) labelTestType.Text = "等级评定";
            else if (carinfo.动力检测类型 == 3) labelTestType.Text = "一级等级评定";
            else if (carinfo.动力检测类型 == 2) labelTestType.Text = "二级等级评定";
        }
        private int levelJudgeStep = 1;
        private Curve2D curve1 = new Curve2D();
        private Curve2D curve2 = new Curve2D();
        private Curve2D curve3 = new Curve2D();
        /*double speedK = 1, forceK = 1, timeK = 1;
        private void initK(double speedMax, double forceMin, double forceMax,double time)
        {
            speedK = (pictureBox速度.Height - 20) * 1.0 / speedMax;
            forceK = (pictureBox扭力.Height - 20) * 1.0 / (forceMax-forceMin);
            timeK = (pictureBox速度.Width - 20) * 1.0 / time;
        }

        private void initSpeedCurve(List<double> listdata)
        {
            grpSpeed.Clear(pictureBox速度.BackColor);

        }*/
        private void initCurve()
        {
            curve1.BgColor = Color.Black;
            curve1.TextColor = Color.WhiteSmoke;
            curve1.SliceColor = Color.WhiteSmoke;
            curve1.AxisColor = Color.WhiteSmoke;
            curve1.AxisTextColor = Color.WhiteSmoke;
            curve1.BorderColor = Color.WhiteSmoke;
            curve1.SliceTextColor = Color.WhiteSmoke;
            curve1.Width = pictureBox速度.Width;
            curve1.Height = pictureBox速度.Height;
            curve1.Title = "速度曲线";
            curve1.XAxisText = "时间(s)";
            curve1.YAxisText = "速度(km/h)";
            curve1.Keys = new string[] { "0", "10", "20", "30", "40", "50", "60", "70", "80", "90" };
            curve1.Values = new float[] { 0f };
            curve1.CurveColors = new Color[] { Color.Yellow };
            curve1.Fit();
            pictureBox速度.Image = curve1.CreateImage();

            curve2.BgColor = Color.Black;
            curve2.TextColor = Color.WhiteSmoke;
            curve2.SliceColor = Color.WhiteSmoke;
            curve2.AxisColor = Color.WhiteSmoke;
            curve2.AxisTextColor = Color.WhiteSmoke;
            curve2.BorderColor = Color.WhiteSmoke;
            curve2.SliceTextColor = Color.WhiteSmoke;
            curve2.Width = pictureBox扭力.Width;
            curve2.Height = pictureBox扭力.Height;
            curve2.Title = "加载力曲线";
            curve2.XAxisText = "时间(s)";
            curve2.YAxisText = "加载力(N)";
            curve2.YSliceValue = 100;
            curve2.Keys = new string[] { "0", "10", "20", "30", "40", "50", "60", "70", "80", "90" };
            curve2.Values = new float[] { 0f };
            curve2.CurveColors = new Color[] { Color.Green };
            curve2.Fit();
            pictureBox扭力.Image = curve2.CreateImage();
            
        }
        public void initEquipment()
        {
            bool Init_flag = true;
            string init_message = "";
            try
            {
                if (equipdata.Ledifpz)
                {
                    try
                    {
                        ledcontrol = new LedControl.BX5k1();
                        ledcontrol.row1 = (byte)(equipdata.ledrow1);
                        ledcontrol.row2 = (byte)(equipdata.ledrow2);
                        if (ledcontrol.Init_Comm(equipdata.Ledck, equipdata.LedComstring, (byte)equipdata.LEDTJPH) == false)
                        {
                            ledcontrol = null;
                            Init_flag = false;
                            init_message += "LED屏串口打开失败.";
                        }
                    }
                    catch (Exception er)
                    {
                        ledcontrol = null;
                        Init_flag = false;
                        MessageBox.Show(er.ToString(), "出错啦");
                    }
                }

            }
            catch (Exception)
            {
            }
            try
            {
                if (equipdata.isYhyPz == true)
                {
                    switch (equipdata.YhyXh.ToLower())           //通过仪器型号选择初始化项目
                    {
                        case "mql_8201":
                            try
                            {
                                yhy = new Exhaust.yhControl(equipdata.YhyXh);
                                if (yhy.Init_Comm(equipdata.YhyCk, equipdata.YhjCkpz) == false)
                                {
                                    yhy = null;
                                    Init_flag = false;
                                    init_message = "油耗仪串口打开失败.";
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                yhy = null;
                                Init_flag = false;
                            }
                            break;
                        case "fly_2000":
                            try
                            {
                                yhy = new Exhaust.yhControl(equipdata.YhyXh);
                                if (yhy.Init_Comm(equipdata.YhyCk, equipdata.YhjCkpz) == false)
                                {
                                    yhy = null;
                                    Init_flag = false;
                                    init_message = "油耗仪串口打开失败.";
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                yhy = null;
                                Init_flag = false;
                            }
                            break;
                        case "nhty_1":
                            try
                            {
                                yhy = new Exhaust.yhControl(equipdata.YhyXh);
                                if (yhy.Init_Comm(equipdata.YhyCk, equipdata.YhjCkpz) == false)
                                {
                                    yhy = null;
                                    Init_flag = false;
                                    init_message = "油耗仪串口打开失败.";
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                yhy = null;
                                Init_flag = false;
                            }
                            break;
                    }
                }
            }
            catch (Exception)
            {

            }
            //这里只初始化了废气分析仪其他设备要继续初始化
            try
            {
                if (equipdata.Cgjifpz)
                {
                    
                        try
                        {
                            igbt = new Dynamometer.IGBT("BNTD",equipdata.isIgbtContainGdyk);
                            if (igbt.Init_Comm(equipdata.Cgjck, equipdata.cgjckpzz) == false)
                            {
                                igbt = null;
                                Init_flag = false;
                                init_message += "测功机串口打开失败.";
                            }
                        }
                        catch (Exception er)
                        {
                            igbt = null;
                            Init_flag = false;
                            MessageBox.Show(er.ToString(), "出错啦");
                        }
                }
            }
            catch (Exception)
            {
            }
            try
            {
                if (equipdata.Fqyifpz == true)
                {
                    switch (equipdata.Fqyxh.ToLower())           //通过仪器型号选择初始化项目
                    {
                        case "nha_503":
                            try
                            {
                                UseFqy = "fla_502";
                                fla_502 = new Exhaust.Fla502(equipdata.Fqyxh);
                                if (fla_502.Init_Comm(equipdata.Fqyck, equipdata.Fqyckpzz) == false)
                                {
                                    fla_502 = null;
                                    Init_flag = false;
                                    init_message = "废气仪串口打开失败.";
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                fla_502 = null;
                                Init_flag = false;
                            }
                            break;
                        case "nha_506":
                            try
                            {
                                UseFqy = "fla_502";
                                fla_502 = new Exhaust.Fla502(equipdata.Fqyxh);
                                if (fla_502.Init_Comm(equipdata.Fqyck, equipdata.Fqyckpzz) == false)
                                {
                                    fla_502 = null;
                                    Init_flag = false;
                                    init_message = "废气仪串口打开失败.";
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                fla_502 = null;
                                Init_flag = false;
                            }
                            break;
                        case "fla_502":
                            try
                            {
                                UseFqy = "fla_502";
                                fla_502 = new Exhaust.Fla502(equipdata.Fqyxh);
                                if (fla_502.Init_Comm(equipdata.Fqyck, equipdata.Fqyckpzz) == false)
                                {
                                    fla_502 = null;
                                    Init_flag = false;
                                    init_message = "废气仪串口打开失败.";
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                fla_502 = null;
                                Init_flag = false;
                            }
                            break;
                        case "mqw_50a":
                            try
                            {
                                UseFqy = "mqw_50a";
                                fla_502 = new Exhaust.Fla502(equipdata.Fqyxh);
                                if (fla_502.Init_Comm(equipdata.Fqyck, equipdata.Fqyckpzz) == false)
                                {
                                    fla_502 = null;
                                    Init_flag = false;
                                    init_message = "废气仪串口打开失败.";
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                fla_502 = null;
                                Init_flag = false;
                            }
                            break;
                        case "cdf5000":
                            try
                            {
                                UseFqy = "fla_502";
                                fla_502 = new Exhaust.Fla502(equipdata.Fqyxh);
                                fla_502.isNhSelfUse = equipdata.isFqyNhSelfUse;
                                if (fla_502.Init_Comm(equipdata.Fqyck, equipdata.Fqyckpzz) == false)
                                {
                                    fla_502 = null;
                                    Init_flag = false;
                                    init_message = "废气仪串口打开失败.";
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                fla_502 = null;
                                Init_flag = false;
                            }
                            break;
                        case "fla_501":
                            try
                            {
                                UseFqy = "fla_501";
                                fla_501 = new Exhaust.Fla501();
                                if (fla_501.Init_Comm(equipdata.Fqyck, equipdata.Fqyckpzz) == false)
                                {
                                    fla_501 = null;
                                    Init_flag = false;
                                    init_message = "废气仪串口打开失败.";
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show(er.ToString(), "出错啦");
                                fla_501 = null;
                                Init_flag = false;
                            }
                            break;
                    }
                }
            }
            catch (Exception)
            {

            }
            
            try
            {
                if (equipdata.Ydjifpz == true && equipdata.Ydjxh != "CDF5000")
                {
                    try
                    {
                        flb_100 = new Exhaust.FLB_100(equipdata.Ydjxh);
                        if (flb_100.Init_Comm(equipdata.Ydjck, "9600,N,8,1") == false)
                        {
                            flb_100 = null;
                            Init_flag = false;
                            init_message += "烟度计串口打开失败.";
                        }
                    }
                    catch (Exception er)
                    {
                        flb_100 = null;
                        Init_flag = false;
                        MessageBox.Show(er.ToString(), "出错啦");
                    }
                }
            }
            catch (Exception)
            {
            }
            try
            {
                if (equipdata.Lljifpz == true)
                {
                    try
                    {
                        flv_1000 = new Exhaust.Flv_1000();
                        if (flv_1000.Init_Comm(equipdata.Lljck, "9600,N,8,1") == false)
                        {
                            flv_1000 = null;
                            Init_flag = false;
                            init_message += "流量计串口打开失败.";

                        }
                    }
                    catch (Exception er)
                    {
                        flv_1000 = null;
                        Init_flag = false;
                        MessageBox.Show(er.ToString(), "出错啦");
                    }
                }
            }
            catch (Exception)
            {
                flv_1000 = null;
                Init_flag = false;
            }
            try
            {
                if (equipdata.TempInstrument == "XCE_100")
                {
                    try
                    {
                        xce_100 = new Exhaust.XCE_100("XCE_100");
                        if (xce_100.Init_Comm(equipdata.Xce100ck, equipdata.Xce100Comstring) == false)
                        {
                            xce_100 = null;
                            Init_flag = false;
                            init_message += "XCE100串口打开失败.";

                        }
                    }
                    catch (Exception er)
                    {
                        xce_100 = null;
                        Init_flag = false;
                        MessageBox.Show(er.ToString(), "XCE100串口打开出错啦");
                    }
                }
                else if (equipdata.TempInstrument == "DWSP_T5")
                {
                    try
                    {
                        xce_100 = new Exhaust.XCE_100("DWSP_T5");
                        if (xce_100.Init_Comm(equipdata.Xce100ck, equipdata.Xce100Comstring) == false)
                        {
                            xce_100 = null;
                            Init_flag = false;
                            init_message += "DWSP_T5串口打开失败.";

                        }
                    }
                    catch (Exception er)
                    {
                        xce_100 = null;
                        Init_flag = false;
                        MessageBox.Show(er.ToString(), "DWSP_T5串口打开出错啦");
                    }
                }
                else if (equipdata.TempInstrument == "FTH_2")
                {
                    try
                    {
                        xce_100 = new Exhaust.XCE_100("FTH_2");
                        if (xce_100.Init_Comm(equipdata.Xce100ck, equipdata.Xce100Comstring) == false)
                        {
                            xce_100 = null;
                            Init_flag = false;
                            init_message += "FTH_2串口打开失败.";

                        }
                    }
                    catch (Exception er)
                    {
                        xce_100 = null;
                        Init_flag = false;
                        MessageBox.Show(er.ToString(), "FTH_2串口打开出错啦");
                    }
                }
                else if (equipdata.TempInstrument == "RZ_1")
                {
                    try
                    {
                        xce_100 = new Exhaust.XCE_100("RZ_1");
                        if (xce_100.Init_Comm(equipdata.Xce100ck, equipdata.Xce100Comstring) == false)
                        {
                            xce_100 = null;
                            Init_flag = false;
                            init_message += "RZ_1串口打开失败.";

                        }
                    }
                    catch (Exception er)
                    {
                        xce_100 = null;
                        Init_flag = false;
                        MessageBox.Show(er.ToString(), "RZ_1串口打开出错啦");
                    }
                }
            }
            catch (Exception)
            {
                xce_100 = null;
                Init_flag = false;
            }
            try
            {
                if (dynconfigdata.Zsj.ToLower() == "vmt-2000" || dynconfigdata.Zsj.ToLower() == "vut-3000")
                {
                    try
                    {
                        vmt_2000 = new Exhaust.VMT_2000();
                        isUseRotater = true;
                        if (vmt_2000.Init_Comm(dynconfigdata.Zsjck, "19200,N,8,1") == false)
                        {
                            vmt_2000 = null;
                            Init_flag = false;
                            init_message += "转速计串口打开失败.";
                            isUseRotater = false;
                        }

                    }
                    catch (Exception er)
                    {
                        vmt_2000 = null;
                        Init_flag = false;
                        MessageBox.Show(er.ToString(), "出错啦");
                        isUseRotater = false;
                    }
                }
                else if (dynconfigdata.Zsj.ToLower() == "mqz-2" || dynconfigdata.Zsj.ToLower() == "mqz-3")
                {
                    MessageBox.Show("系统未提供该转速计功能，请重新配置", "系统提示");
                    isUseRotater = false;
                }
                else if (dynconfigdata.Zsj.ToLower() == "rpm5300")
                {
                    try
                    {
                        rpm5300 = new Exhaust.RPM5300();
                        isUseRotater = true;
                        if (rpm5300.Init_Comm(dynconfigdata.Zsjck, "9600,N,8,1") == false)
                        {
                            rpm5300 = null;
                            Init_flag = false;
                            init_message += "转速计串口打开失败.";
                            isUseRotater = false;
                        }

                    }
                    catch (Exception er)
                    {
                        rpm5300 = null;
                        Init_flag = false;
                        MessageBox.Show(er.ToString(), "出错啦");
                        isUseRotater = false;
                    }
                }
                else
                {
                    isUseRotater = false;
                }

            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 信息显示
        /// </summary>
        /// <param name="Msgowner">信息显示的Label控件</param>
        /// <param name="Msgfather">Label控件的父级Panel</param>
        /// <param name="Msgstr">要显示的信息</param>
        /// <param name="Update_DB">是不是要更新到检测状态</param>
        public void Msg(Label Msgowner, Panel Msgfather, string Msgstr, bool Update_DB)
        {
            if (Update_DB)
            {
                if (showform != null) showform.DriverShow_OnMyChange(0, Msgstr);
            }
            BeginInvoke(new wtlsb(Msg_Show), Msgowner, Msgstr, Update_DB);
            BeginInvoke(new wtlp(Msg_Position), Msgowner, Msgfather);
        }
        public void Label_Msg(Label Msgowner, string Msgstr)
        {
            BeginInvoke(new wtlm(Label_Show), Msgowner, Msgstr);
        }
        public void Label_Show(Label Msgowner, string Msgstr)
        {
            Msgowner.Text = Msgstr;
        }
        public void Msg_Show(Label Msgowner, string Msgstr, bool Update_DB)
        {
            Msgowner.Text = Msgstr;
        }

        public void Msg_Position(Label Msgowner, Panel Msgfather)
        {
            if (Msgowner.Width < Msgfather.Width)
                Msgowner.Location = new Point((Msgfather.Width - Msgowner.Width) / 2, Msgowner.Location.Y);
            else
                Msgowner.Location = new Point(0, Msgowner.Location.Y);
        }
        public void showTextboxText(TextBox textbox,string text)
        {
            BeginInvoke(new wtTextboxText(textbox_show), textbox, text);
        }
        public void textbox_show(TextBox Msgowner, string Msgstr)
        {
            Msgowner.Text = Msgstr;
        }
        bool jcStatus = false;
        int gksj_count = 0;

        private void button3_Click(object sender, EventArgs e)
        {
                if (igbt != null)
                    igbt.Lifter_Up();
        }
        private bool isZsStable = false;

        private void checkBox取车内仪表转速_CheckedChanged(object sender, EventArgs e)
        {
            button5.Visible = checkBox取车内仪表转速.Checked;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            isZsStable = true;
            button5.Visible = false;
            checkBox取车内仪表转速.Checked = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
                if (igbt != null)
                    igbt.Lifter_Down();
        }

        bool JC_Status = false;
        yhrRealTimeStatus yhystatus = new yhrRealTimeStatus();
        private int enginezs = 0;
        private string addLength(string a, int b)
        {
            while (a.Length < b)
            {
                a = " " + a;
            }
            return a;
        }
        public static float driveshowForceSliceValue = 100;

        private void button4_Click(object sender, EventArgs e)
        {
            if (igbt != null)
                igbt.Force_Zeroing();
        }
        double speedTestResult = 0;
        int speedTestPdjg = 0;

        private void button6_Click(object sender, EventArgs e)
        {
            //修正曲线示意图 xzform = new 修正曲线示意图();
            //xzform.lx = carinfo.燃料种类;
            // xzform.Show();
            if(File.Exists( Application.StartupPath+"/气象站示值修正.exe"))
                Process.Start(Application.StartupPath + "/气象站示值修正.exe");
        }

        private void Jc_Exe()
        {           
            try
            {

                if(dynconfigdata.DynUseGddw)
                {
                    Msg(labelTS, panelTS, "请车辆到位", true);
                    showLedSingleRow(2, "请车辆到位　　　");
                    int gddwcount = 0;
                    while(gddwcount<dynconfigdata.DynGddwTime*10)
                    {
                        Thread.Sleep(100);
                        byte gddata = 0x01;
                        switch (equipdata.CarGdChanel)
                        {
                            case 1: gddata = 0x01; break;
                            case 2: gddata = 0x02; break;
                            case 3: gddata = 0x04; break;
                            default: break;
                        }
                        if ((igbt.keyandgd & gddata) == 0x00)
                        {
                            gddwcount++;
                        }
                        else
                        {
                            gddwcount = 0;
                        }
                    }
                }
                if(dynconfigdata.DynUseYkdw)
                {
                    isZsStable = false;
                    while (!isZsStable)
                    {
                        if (equipdata.isIgbtContainGdyk)
                        {
                            if (dynconfigdata.DynYkStyle == 0)
                            {
                                if (((igbt.keyandgd) & 0xf0) != 0x00)
                                {
                                    isZsStable = true;
                                }
                                igbt.Set_ClearKey();
                            }
                            else
                            {
                                byte gddata = 0x01;
                                switch (dynconfigdata.DynWjyktd)
                                {
                                    case 1: gddata = 0x01; break;
                                    case 2: gddata = 0x02; break;
                                    case 3: gddata = 0x04; break;
                                    default: break;
                                }
                                if ((igbt.keyandgd & gddata) == 0x00)
                                {
                                    isZsStable = true;
                                }
                            }
                        }
                        Thread.Sleep(500);
                    }
                    isZsStable = false;
                }
                levelJudgeStep = 1;
                Msg(labelTS, panelTS, "举升器下降", true);
                showLedSingleRow(2, "举升器下降　　　");
                igbt.Lifter_Down(); //台体下降
                Thread.Sleep(500);
                igbt.Lifter_Down(); //台体下降
                Thread.Sleep(500);
                igbt.Lifter_Down(); //台体下降
                Thread.Sleep(3000);
                if (carinfo.检测类型==1)
                {
                    carini.changeStatusData("1", "开始速度检测");
                       isTestSpeedPerod = true;
                    Msg(labelTS, panelTS, "准备车速检测", true);
                    showLed(carinfo.车辆号牌, "准备车速检测");
                    Thread.Sleep(3000);
                    Msg(labelTS, panelTS, "加速至40km/h请按遥控", true);
                    showLed("请加速至40km/h", "　稳定后请按遥控");                    
                    if (equipdata.isIgbtContainGdyk)
                    {
                        igbt.Set_ClearKey();
                    }
                    Thread.Sleep(1000);
                    isZsStable = false;
                    while (!isZsStable)
                    {
                        if (equipdata.isIgbtContainGdyk)
                        {
                            if (dynconfigdata.DynYkStyle == 0)
                            {
                                if (((igbt.keyandgd) & 0xf0) != 0x00)
                                {
                                    isZsStable = true;
                                }
                                igbt.Set_ClearKey();
                            }
                            else
                            {
                                byte gddata = 0x01;
                                switch (dynconfigdata.DynWjyktd)
                                {
                                    case 1: gddata = 0x01; break;
                                    case 2: gddata = 0x02; break;
                                    case 3: gddata = 0x04; break;
                                    default: break;
                                }
                                if ((igbt.keyandgd & gddata) == 0x00)
                                {
                                    isZsStable = true;
                                }
                            }
                        }
                        Thread.Sleep(500);
                    }
                    isZsStable = false;
                    speedTestResult = Math.Round(speednow,1);
                    if (speedTestResult >= 32.8&&speedTestResult<=40.0)
                    {
                        speedTestPdjg = 0;
                        Msg(labelTS, panelTS, speedTestResult.ToString("0.0") + "km/h"+ " 合格", true);
                        showLed(addLength(speedTestResult.ToString("0.0")+"km/h",10), "　　　合格　　　");
                    }
                    else
                    {
                        speedTestPdjg = 1;
                        Msg(labelTS, panelTS, speedTestResult.ToString("0.0") + "km/h" + " 不合格", true);
                        showLed(addLength(speedTestResult.ToString("0.0") + "km/h", 10), "　　不合格　　　");
                    }
                    Thread.Sleep(2000);
                    isTestSpeedPerod = false;
                    Msg(labelTS, panelTS, "检测完成,请减速停车", true);
                    showLed("检测完成　　　　", "请减速停车　　　");
                    igbt.Exit_Control();
                    Thread.Sleep(3000);
                    writeSpeedTestResult();
                    while (speednow > 0.1)
                        Thread.Sleep(1000);
                    igbt.Lifter_Up();
                    carini.changeStatusData("2", "速度检测完成");
                    Thread.Sleep(5000);//等待举升升起后出车
                    Msg(labelTS, panelTS, "检测完毕", true);
                    showLed("　　检测完毕　　", "　　　　　　　　");
                    Thread.Sleep(2000);
                    showLed("　　欢迎参检　　", "　　　　　　　　");
                    this.Close();
                }
                else if (carinfo.检测类型 == 0 || carinfo.检测类型 == 2)
                {
                    if (carinfo.检测类型 == 2)
                    {
                        carini.changeStatusData("1", "开始速度检测");
                        isTestSpeedPerod = true;
                        Msg(labelTS, panelTS, "准备车速检测", true);
                        showLed(carinfo.车辆号牌, "准备车速检测");
                        Thread.Sleep(3000);
                        Msg(labelTS, panelTS, "加速至40km/h请按遥控", true);
                        showLed("请加速至40km/h", "　稳定后请按遥控");
                        if (equipdata.isIgbtContainGdyk)
                        {
                            igbt.Set_ClearKey();
                        }
                        Thread.Sleep(1000);
                        isZsStable = false;
                        while (!isZsStable)
                        {
                            if (equipdata.isIgbtContainGdyk)
                            {
                                if (dynconfigdata.DynYkStyle == 0)
                                {
                                    if (((igbt.keyandgd) & 0xf0) != 0x00)
                                    {
                                        isZsStable = true;
                                    }
                                    igbt.Set_ClearKey();
                                }
                                else
                                {
                                    byte gddata = 0x01;
                                    switch (dynconfigdata.DynWjyktd)
                                    {
                                        case 1: gddata = 0x01; break;
                                        case 2: gddata = 0x02; break;
                                        case 3: gddata = 0x04; break;
                                        default: break;
                                    }
                                    if ((igbt.keyandgd & gddata) == 0x00)
                                    {
                                        isZsStable = true;
                                    }
                                }
                            }
                            Thread.Sleep(500);
                        }
                        isZsStable = false;
                        speedTestResult = Math.Round(speednow, 1);
                        if (speedTestResult >= 32.8 && speedTestResult <= 40.0)
                        {
                            speedTestPdjg = 0;
                            Msg(labelTS, panelTS, speedTestResult.ToString("0.0") + "km/h" + " 合格", true);
                            showLed(addLength(speedTestResult.ToString("0.0") + "km/h", 10), "　　　合格　　　");
                        }
                        else
                        {
                            speedTestPdjg = 1;
                            Msg(labelTS, panelTS, speedTestResult.ToString("0.0") + "km/h" + " 不合格", true);
                            showLed(addLength(speedTestResult.ToString("0.0") + "km/h", 10), "　　不合格　　　");
                        }
                        Thread.Sleep(2000);
                        carini.changeStatusData("2", "速度检测完成");
                        isTestSpeedPerod = false;
                    }
                    carini.changeStatusData("3", "开始动力性检测");
                    Msg(labelTS, panelTS, "准备动力性测量", true);
                    showLed(carinfo.车辆号牌, "准备动力性测量");
                    Thread.Sleep(3000);
                    Msg(labelTS, panelTS, "测量即将开始", true);
                    showLed(carinfo.车辆号牌, "检测即将开始　　");
                    Thread.Sleep(1000);
                    #region 仪器准备
                    Msg(labelTS, panelTS, "测量环境参数", true);
                    showLed(carinfo.车辆号牌, "测量环境参数　　");
                    Exhaust.Fla502_data Environment = new Exhaust.Fla502_data();
                    Exhaust.Flb_100_smoke ydjEnvironment = new Exhaust.Flb_100_smoke();
                    Exhaust.yhrRealTimeData yhyEnvironment = new yhrRealTimeData();
                    try
                    {
                        if (IsUseTpTemp)
                        {
                            Wd = Wd;
                            Sd = Sd;
                            Dqy = Dqy;
                        }
                        else if (equipdata.TempInstrument == "烟度计")
                        {
                            flb_100.Set_Measure();
                            Thread.Sleep(1000);
                            if (equipdata.IsOldMqy200)
                                ydjEnvironment = flb_100.get_DirectData();
                            else
                                ydjEnvironment = flb_100.get_Data();
                            if(ydjEnvironment.WD==0&&ydjEnvironment.SD==0&&ydjEnvironment.DQY==0)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    if (equipdata.IsOldMqy200)
                                        ydjEnvironment = flb_100.get_DirectData();
                                    else
                                        ydjEnvironment = flb_100.get_Data();
                                    Thread.Sleep(200);
                                    if (ydjEnvironment.WD != 0 || ydjEnvironment.SD != 0 && ydjEnvironment.DQY != 0)
                                        break;
                                }
                            }
                            Wd = ydjEnvironment.WD;
                            Sd = ydjEnvironment.SD;
                            Dqy = ydjEnvironment.DQY;
                        }
                        else if (equipdata.TempInstrument == "油耗仪")
                        {
                            if (yhy.getTempData(out yhyEnvironment))
                            {
                                Wd = yhyEnvironment.HJWD;
                                Sd = yhyEnvironment.HJSD;
                                Dqy = yhyEnvironment.HJYL;
                            }
                            else if (yhy.getTempData(out yhyEnvironment))
                            {                                
                                Wd = yhyEnvironment.HJWD;
                                Sd = yhyEnvironment.HJSD;
                                Dqy = yhyEnvironment.HJYL;
                            }
                            else
                            {
                                Msg(labelTS, panelTS, "读取环境参数失败", false);
                                showLedSingleRow(2, "读取环境参数失败");
                                return;
                            }
                        }
                        else if (equipdata.TempInstrument == "废气仪")
                        {
                            if (equipdata.Fqyxh.ToLower() == "nha_503" || equipdata.Fqyxh.ToLower() == "fla_502" || equipdata.Fqyxh.ToLower() == "cdf5000")
                            {
                                Exhaust.Fla502_temp_data flaEnvironment = fla_502.Get_Temp();
                                Wd = flaEnvironment.TEMP;
                                Sd = flaEnvironment.HUMIDITY;
                                Dqy = flaEnvironment.AIRPRESSURE;
                            }
                            else
                            {
                                Environment = fla_502.GetData();
                                Wd = Environment.HJWD;
                                Sd = Environment.SD;
                                Dqy = Environment.HJYL;
                            }
                        }
                        else if (equipdata.TempInstrument == "XCE_100")
                        {
                            if (xce_100.readEnvironment())
                            {
                                Wd = xce_100.temp;
                                Sd = xce_100.humidity;
                                Dqy = xce_100.airpressure;
                            }
                        }
                        else if (equipdata.TempInstrument == "DWSP_T5")
                        {
                            if (xce_100.readEnvironment())
                            {
                                Wd = xce_100.temp;
                                Sd = xce_100.humidity;
                                Dqy = xce_100.airpressure;
                            }
                        }
                        else if (equipdata.TempInstrument == "FTH_2")
                        {
                            if (xce_100.readEnvironment())
                            {
                                Wd = xce_100.temp;
                                Sd = xce_100.humidity;
                                Dqy = xce_100.airpressure;
                            }
                        }
                        else if (equipdata.TempInstrument == "RZ_1")
                        {
                            if (xce_100.readEnvironment())
                            {
                                Wd = xce_100.temp;
                                Sd = xce_100.humidity;
                                Dqy = xce_100.airpressure;
                            }
                        }
                        if (Wd == 0 && Sd == 0 && Dqy == 0)
                        {
                            showLedSingleRow(2, "读取环境参数失败");
                            Msg(labelTS, panelTS, "读取环境参数失败", false);
                            return;
                        }
                    }
                    catch (Exception er)
                    {
                        showLedSingleRow(2, "读取环境参数异常");
                        Msg(labelTS, panelTS, "读取环境参数异常", false);
                        return;
                    }
                    showTextboxText(textBoxWd, Wd.ToString("0.0"));
                    showTextboxText(textBoxSd, Sd.ToString("0.0"));
                    showTextboxText(textBoxDqy, Dqy.ToString("0.0"));
                    //textBoxWd.Text = Wd.ToString("0.0");
                    //textBoxSd.Text = Sd.ToString("0.0");
                    //textBoxDqy.Text = Dqy.ToString("0.0");
                    Thread.Sleep(1000);
                    #endregion
                    StartPosition:
                    if (carinfo.动力检测类型 == 1)
                    {
                        if (levelJudgeStep == 1)
                        {
                            showLed("开始一级评定　　", "　　　　　　　　");
                            Msg(labelTS, panelTS, "开始一级评定", true);
                            Label_Msg(labelTestType, "开始一级评定测试");
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            showLed("开始二级评定　　", "　　　　　　　　");
                            Msg(labelTS, panelTS, "开始二级评定", true);
                            Label_Msg(labelTestType, "开始二级评定测试");
                            Thread.Sleep(2000);
                        }
                    }
                    else if(carinfo.动力检测类型==3)
                    {
                        showLed("开始一级评定　　", "　　　　　　　　");
                        Msg(labelTS, panelTS, "开始一级评定", true);
                        Label_Msg(labelTestType, "开始一级评定测试");
                        Thread.Sleep(2000);
                    }
                    else if(carinfo.动力检测类型==2)
                    {
                        showLed("开始二级评定　　", "　　　　　　　　");
                        Msg(labelTS, panelTS, "开始二级评定", true);
                        Label_Msg(labelTestType, "开始二级评定测试");
                        Thread.Sleep(2000);
                    }
                    showCurve = true;
                    if (carinfo.燃料种类 == 0)
                    {
                        #region 点燃式测量过程
                        showLed("检测开始　　　　", "第3档逐步加速");
                        Msg(labelTS, panelTS, "第3档逐步加速", true);
                        Thread.Sleep(2000);
                        Msg(labelTS, panelTS, "稳定转速至:" + carinfo.点燃式额定扭矩转速.ToString("0"), true);
                        Thread.Sleep(1000);
                        VmTest:
                        showLedSingleRow(1, "转速稳至:" + addLength(carinfo.点燃式额定扭矩转速.ToString("0"), 4));
                        if (equipdata.isIgbtContainGdyk)
                        {
                            igbt.Set_ClearKey();
                        }
                        Thread.Sleep(1000);
                        isZsStable = false;
                        int zsstabletime = 0;
                        igbt.Set_ClearKey();
                        Thread.Sleep(200);
                        while (!isZsStable)
                        {
                            Thread.Sleep(100);
                            Msg(labelTS, panelTS, "稳定:" + carinfo.点燃式额定扭矩转速.ToString("0") + "|" + enginezs.ToString("0"), true);
                            if (dynconfigdata.Zsj == "未配置")
                            {
                                showLedSingleRow(2, "转速稳定后按遥控");
                            }
                            else
                            {
                                showLedSingleRow(2, "当前转速:" + addLength(enginezs.ToString("0"), 4));
                            }
                            if (Math.Abs(enginezs - carinfo.点燃式额定扭矩转速) < dynconfigdata.cczs)
                            {
                                zsstabletime++;
                            }
                            else
                            {
                                zsstabletime = 0;
                            }
                            if (zsstabletime > dynconfigdata.stableTime * 10)
                                isZsStable = true;
                            if (equipdata.isIgbtContainGdyk&&dynconfigdata.DynYkqr)
                            {
                                if (dynconfigdata.DynYkStyle == 0)
                                {
                                    if (((igbt.keyandgd) & 0xf0) != 0x00)
                                    {
                                        isZsStable = true;
                                    }
                                    igbt.Set_ClearKey();
                                }
                                else
                                {
                                    byte gddata = 0x01;
                                    switch (dynconfigdata.DynWjyktd)
                                    {
                                        case 1: gddata = 0x01; break;
                                        case 2: gddata = 0x02; break;
                                        case 3: gddata = 0x04; break;
                                        default: break;
                                    }
                                    if ((igbt.keyandgd & gddata) == 0x00)
                                    {
                                        isZsStable = true;
                                    }
                                }
                            }
                        }
                        Vm = speednow;
                        if (Vm > 80)
                        {
                            Msg(labelTS, panelTS, "Vm大于80km/h,请降档重新测取", true);
                            showLed("Vm大于80km/h", "请降档重新测取　");
                            Thread.Sleep(2000);
                            goto VmTest;
                        }
                        Msg(labelTS, panelTS, "额定扭矩车速确定,Vm=" + Vm.ToString("0.0") + "km/h", true);
                        showLed("额定扭矩车速确定", "请加速　　　　　");
                        Thread.Sleep(1000);
                        Msg(labelTS, panelTS, "加速使车速超过Vm" , true);
                        curve1.scale1Color = Color.Red;
                        curve1.scale1Value = (float)Vm;
                        curve1.displayScale1 = true;
                        curvescalevalue = (float)Vm;
                        displayCurveScale = true;
                        Thread.Sleep(1000);
                        isZsStable = false;
                        int stableTime = 0;
                        float prespeed = 0;
                        while (!isZsStable)
                        {
                            Thread.Sleep(100);
                            Msg(labelTS, panelTS, "当前车速:" + speednow.ToString("0.0"), true);
                            if (speednow > Vm+1)//加速使速度超过Vm且保持要求的时间以上，再开始加载
                            {
                                stableTime++;
                            }
                            else
                            {
                                stableTime = 0;
                            }
                            if (stableTime > dynconfigdata.stableTime * 10)
                                isZsStable = true;

                            showLedSingleRow(2, "当前车速:" + addLength(speednow.ToString("0.0"), 4));
                        }
                        Thread.Sleep(5000);
                        caculateQyFE();//计算汽油车加载力
                        if (X_FM < 100)
                        {
                            curve2.YSliceValue = 10;
                        }
                        else if (X_FM < 1000)
                        {
                            curve2.YSliceValue = 100;
                        }
                        else if (X_FM < 10000)
                        {
                            curve2.YSliceValue = 1000;
                        }
                        else
                        {
                            curve2.YSliceValue = 2000;
                        }
                        driveshowForceSliceValue = curve2.YSliceValue;
                        Msg(labelTS, panelTS, "开始加载,加载力=" + X_FM.ToString("0") + "N", true);
                        igbt.Set_Control_Force(0);
                        Thread.Sleep(200);
                        igbt.Start_Control_Force();
                        int jzsj = (int)(X_FM / dynconfigdata.forceTime);
                        if (jzsj <= 1)
                        {
                            if (igbt != null)
                            {
                                igbt.Set_Control_Force((float)(X_FM* (carinfo.加载力比例 * dynconfigdata.DynNlxs)));
                                showLedSingleRow(2, "开始加载:" + addLength(X_FE.ToString("0"), 4));
                                Thread.Sleep(900);
                            }
                        }
                        else
                        {
                            for (int i = 1; i <= jzsj; i++)//逐渐加载,以免对车速影响太大
                            {
                                igbt.Set_Control_Force((float)((X_FM* (carinfo.加载力比例 * dynconfigdata.DynNlxs)) * i*1.0 / (1.0*jzsj)));
                                Msg(labelTS, panelTS, "加载中:" + (jzsj - i) + "s", true);
                                showLedSingleRow(2, "开始加载:" + addLength(forcenow.ToString("0"), 4));
                                Thread.Sleep(1000);
                            }
                        }
                        igbt.Set_Control_Force((float)(X_FM * (carinfo.加载力比例 * dynconfigdata.DynNlxs)));
                        Msg(labelTS, panelTS, "等待稳定3s", true);
                        Thread.Sleep(1000);

                        DateTime stablestarttime = DateTime.Now;
                        DateTime waitstarttime = DateTime.Now;
                        int forcestabletime = 3000;
                        int waittime = 0;
                        while (forcestabletime > 0)
                        {
                            //DateTime stablenowtime = DateTime.Now;
                            TimeSpan stabletimespan = DateTime.Now - stablestarttime;
                            TimeSpan waittimespan = DateTime.Now - waitstarttime;
                            waittime = (int)waittimespan.TotalMilliseconds;
                            forcestabletime = 3000 - (int)stabletimespan.TotalMilliseconds;
                            showLedSingleRow(2, "等待稳定:" + addLength(((double)forcestabletime / 1000.0).ToString("0.0"), 4));
                            Msg(labelTS, panelTS, "等待稳定" + ((double)forcestabletime / 1000.0).ToString("0.0") + "s" + "|" + forcenow.ToString("0") + "N", true);
                            if (Math.Abs(forcenow - X_FM) > dynconfigdata.DynForceQj)
                            {
                                stablestarttime = DateTime.Now;
                            }
                            if (waittime > dynconfigdata.DynUnstableTime*1000)//如果超过15秒加载力还未稳定，则终止检测
                            {
                                Thread.Sleep(100);
                                showLedSingleRow(1, "检测中止");
                                showLedSingleRow(2, "加载力超时未稳定");
                                Msg(labelTS, panelTS, "加载力超时未稳定，检测中止", false);
                                igbt.Exit_Control();
                                return;
                            }
                            Thread.Sleep(50);
                        }
                        /*
                        int forcestabletime = 30;
                        while (forcestabletime > 0)
                        {
                            showLedSingleRow(2, "等待稳定:" + addLength(((double)forcestabletime / 10.0).ToString("0.0"), 4));
                            Msg(labelTS, panelTS, "等待稳定" + ((double)forcestabletime / 10.0).ToString("0.0") + "s" + "|" + forcenow.ToString("0") + "N", true);
                            if (Math.Abs(forcenow - X_FM) < dynconfigdata.DynForceQj)
                            {
                                forcestabletime--;
                            }
                            else
                            {
                                forcestabletime = 30;
                            }
                            Thread.Sleep(100);
                        }*/

                        Msg(labelTS, panelTS, "加载完成,等待速度稳定", true);
                        showLed("加载完成　　　　", "等待速度稳定　　");
                        Thread.Sleep(1000);
                        List<double> speedStableList = new List<double>();
                        speedStableList.Add(speednow);
                        int stableCountWait = 0;
                        for (int i = 0; i < 29; i++)//先保存3s内的数据，共30个点的数据进行稳定判定
                        {
                            Thread.Sleep(80);
                            showLedSingleRow(1, "当前速度:" + addLength(speednow.ToString("0.0"), 4));
                            Msg(labelTS, panelTS, "Vw:" + speedStableList[0].ToString("0.0") + " V:" + speednow.ToString("0.0") + " " + (stableCountWait * 0.1).ToString("0.0") + "s", true);
                            speedStableList.Add(speednow);
                        }
                        while (true)
                        {
                            stableCountWait++;
                            Vw = speedStableList[0];//取过去3秒内的第一个数据作为Vw
                            showLedSingleRow(1, "当前速度:" + addLength(speednow.ToString("0.0"), 4));
                            Msg(labelTS, panelTS, "Vw:" + speedStableList[0].ToString("0.0") + " V:" + speednow.ToString("0.0") + " " + (stableCountWait * 0.1).ToString("0.0") + "s", true);
                            if (speedStableList.Min() - Vw >= -dynconfigdata.DynVwSdqj && speedStableList.Max() - Vw <= dynconfigdata.DynVwSdqj)
                            {
                                break;//如果3秒内的最小值和最大值与Vw的差值在设置范围内，则认为稳定，退出循环
                            }
                            else
                            {
                                speedStableList.Add(speednow);//添加当前数据至末尾
                                speedStableList.RemoveAt(0);//移除第一个数据
                            }
                            if (speedStableList.Min() < Vm && speedStableList.Max() < Vm)
                            {
                                Vw = speedStableList.Min();//如果3s内的数据都小于评断限值，则认为已经不合格，以免车辆被拖死，取最小值为Vw，退出循环
                                break;
                            }
                            if (stableCountWait >= dynconfigdata.DynMaxWaitTime*10)//如果超过最长等待稳定时间,即停止测试,取当前速度为稳定车速
                            {
                                Vw = speednow;//如果达到规定的等待时间，则取当前速度 为Vw，退出循环
                                break;
                            }
                            Thread.Sleep(40);
                        }

                        Msg(labelTS, panelTS, "Vw已测取,Vw=" + Vw.ToString("0.0") + "km/h", true);
                        showLed(" Vw测定完毕: ", " " + addLength(Vw.ToString("0.0"), 4) + "km/h");
                        showCurve = false;//在第一时间停止图表的显示
                        Thread.Sleep(2000);
                        igbt.Exit_Control();
                        bool pdjg = false;
                        if (Vw >= Vm)
                        {
                            pdjg = true;
                            pdjgstring = "合格";
                            Msg(labelTS, panelTS, "Vw≥Vm" + " 合格", true);
                            showLedSingleRow(1, "　　　合格　　　");
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            pdjg = false;
                            pdjgstring = "不合格";
                            Msg(labelTS, panelTS, "Vw＜Vm" + " 不合格", true);
                            showLedSingleRow(1, "　　不合格　　　");
                            Thread.Sleep(1000);
                        }
                        Thread.Sleep(1000);
                        Thread.Sleep(1000);
                        if (carinfo.动力检测类型 == 1 && levelJudgeStep == 1 && !pdjg)
                        {
                            levelJudgeStep = 2;
                            goto StartPosition;
                        }
                        Msg(labelTS, panelTS, "检测完成,请减速停车", true);
                        showLed("检测完成　　　　", "请减速停车　　　");
                        DataTable dyn_datatable = new DataTable();
                        DataRow dr = null;
                        try
                        {
                            dyn_datatable.Columns.Add("速度");
                            dyn_datatable.Columns.Add("扭力");
                            dyn_datatable.Columns.Add("功率");
                            dyn_datatable.Columns.Add("达标速度");
                            for (int i = 0; i < speedlist.Count; i++)//将数据写入逐秒数据
                            {
                                dr = dyn_datatable.NewRow();
                                dr["速度"] = speedlist[i].ToString("0.0");
                                dr["扭力"] = forcelist[i].ToString("0");
                                dr["功率"] = powerlist[i].ToString("0.0");
                                dr["达标速度"] = Vm.ToString("0.0");
                                dyn_datatable.Rows.Add(dr);
                            }
                        }
                        catch (Exception er)
                        {
                            MessageBox.Show("生成过程数据时出错：" + er.Message);
                            return;
                        }
                        csvwriter.SaveCSV(dyn_datatable, "D:/dataseconds/" + carinfo.车辆号牌+"_"+DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv");
                        writeGasolineResult();
                        carini.changeStatusData("4", "动力性检测完成");
                        Thread.Sleep(3000);
                        //System.IO.File.Delete(@"c:\jcdatatxt\carinfo.ini");
                        while (speednow > 0.1)
                            Thread.Sleep(1000);
                        igbt.Lifter_Up();
                        Thread.Sleep(5000);//等待举升升起后出车
                        Msg(labelTS, panelTS, "检测完毕", true);
                        showLed("　　检测完毕　　", "　　　　　　　　");
                        Thread.Sleep(2000);
                        showLed("　　欢迎参检　　", "　　　　　　　　");
                        this.Close();
                        #endregion
                    }
                    else
                    {
                        #region 压燃式测量过程

                        Msg(labelTS, panelTS, "直接挡逐步加速直至全油门", true);
                        showLed("检测开始　　　　", "加速直至油门全开");
                        Thread.Sleep(5000);
                        VaTest:
                        isZsStable = false;
                        int stableTime = 0;
                        float prespeed = 0;
                        igbt.Set_ClearKey();
                        Thread.Sleep(200);
                        while (!isZsStable)
                        {
                            Thread.Sleep(100);
                            Msg(labelTS, panelTS, "当前车速:" + speednow.ToString("0.0"), true);
                            if (carinfo.是否危险货物运输车辆 == 1 && speednow > 80)
                            {
                                showLed("车速不能大于80", "请降低档位　　　");
                                Msg(labelTS, panelTS, "危货车最高车速不能大于80km/h,请降低档位", true);
                                Thread.Sleep(1000);
                                stableTime = 0;
                            }
                            else if (speednow > 95)
                            {
                                showLed("车速不能大于95", "请降低档位　　　");
                                Msg(labelTS, panelTS, "最高车速不能大于95km/h,请降低档位", true);
                                Thread.Sleep(1000);
                                stableTime = 0;
                            }
                            else if (speednow > 50)//50km/h以上进行判断车速是否稳定
                            {
                                if (Math.Abs(prespeed - speednow) > dynconfigdata.DynSdqj)//速度变化 不能超过0.5km/h,默认保持3s即认为速度已稳定
                                {
                                    stableTime = 0;
                                    prespeed = speednow;
                                }
                                else
                                {
                                    stableTime++;
                                }
                            }
                            if (stableTime > dynconfigdata.stableTime * 10)
                                isZsStable = true;
                            if (equipdata.isIgbtContainGdyk && dynconfigdata.DynYkqr)
                            {
                                if (dynconfigdata.DynYkStyle == 0)
                                {
                                    if (((igbt.keyandgd) & 0xf0) != 0x00)
                                    {
                                        isZsStable = true;
                                    }
                                    igbt.Set_ClearKey();
                                }
                                else
                                {
                                    byte gddata = 0x01;
                                    switch (dynconfigdata.DynWjyktd)
                                    {
                                        case 1: gddata = 0x01; break;
                                        case 2: gddata = 0x02; break;
                                        case 3: gddata = 0x04; break;
                                        default: break;
                                    }
                                    if ((igbt.keyandgd & gddata) == 0x00)
                                    {
                                        isZsStable = true;
                                    }
                                }
                            }
                            showLedSingleRow(2, "当前车速:" + addLength(speednow.ToString("0.0"), 4));
                        }
                        Va = speednow;
                        Ve = Va * 0.86;
                        Msg(labelTS, panelTS, "额定功率车速确定,Ve=" + Ve.ToString("0.0") + "km/h", true);
                        showLed("额定功率车速确定", "" + addLength(Ve.ToString("0.0"), 5) + "km/h ");

                        curve1.scale1Color = Color.Red;
                        curve1.scale1Value = (float)Ve;
                        curve1.displayScale1 = true;
                        curvescalevalue = (float)Ve;
                        displayCurveScale = true;
                        Thread.Sleep(2000);
                        caculateCyFE();//计算汽油车加载力
                        if (X_FE < 100)
                        {
                            curve2.YSliceValue = 10;
                        }
                        else if (X_FE < 1000)
                        {
                            curve2.YSliceValue = 100;
                        }
                        else if (X_FE < 10000)
                        {
                            curve2.YSliceValue = 1000;
                        }
                        else
                        {
                            curve2.YSliceValue = 2000;
                        }
                        driveshowForceSliceValue = curve2.YSliceValue;
                        Msg(labelTS, panelTS, "开始加载,加载力=" + X_FE.ToString("0") + "N", true);
                        igbt.Set_Control_Force(0);
                        Thread.Sleep(200);
                        igbt.Start_Control_Force();
                        int jzsj = (int)(X_FE / dynconfigdata.forceTime);
                        if (jzsj <= 1)
                        {
                            if (igbt != null)
                            {
                                igbt.Set_Control_Force((float)(X_FE * (carinfo.加载力比例 * dynconfigdata.DynNlxs)));
                                showLedSingleRow(2, "开始加载:" + addLength(X_FE.ToString("0"), 4));
                                Thread.Sleep(900);
                            }
                        }
                        else
                        {
                            for (int i = 1; i <= jzsj; i++)//逐渐加载,以免对车速影响太大
                            {
                                igbt.Set_Control_Force((float)(X_FE * (carinfo.加载力比例 * dynconfigdata.DynNlxs) * i*1.0 / (jzsj*1.0)));
                                Msg(labelTS, panelTS, "加载中:" + (jzsj - i) + "s", true);
                                showLedSingleRow(2, "开始加载:" + addLength(forcenow.ToString("0"), 4));
                                Thread.Sleep(1000);
                            }
                        }
                        igbt.Set_Control_Force((float)(X_FE * (carinfo.加载力比例 * dynconfigdata.DynNlxs)));
                        Msg(labelTS, panelTS, "等待稳定3s", true);
                        Thread.Sleep(1000);

                        DateTime stablestarttime = DateTime.Now;
                        DateTime waitstarttime = DateTime.Now;
                        int forcestabletime = 3000;
                        int waittime = 0;
                        while (forcestabletime > 0)
                        {
                            //DateTime stablenowtime = DateTime.Now;
                            TimeSpan stabletimespan = DateTime.Now - stablestarttime;
                            TimeSpan waittimespan = DateTime.Now - waitstarttime;
                            waittime = (int)waittimespan.TotalMilliseconds;
                            forcestabletime = 3000 - (int)stabletimespan.TotalMilliseconds;
                            showLedSingleRow(2, "等待稳定:" + addLength(((double)forcestabletime / 1000.0).ToString("0.0"), 4));
                            Msg(labelTS, panelTS, "等待稳定" + ((double)forcestabletime / 1000.0).ToString("0.0") + "s" + "|" + forcenow.ToString("0") + "N", true);
                            if (Math.Abs(forcenow - X_FE) > dynconfigdata.DynForceQj)
                            {
                                stablestarttime = DateTime.Now;
                            }
                            if (waittime > dynconfigdata.DynUnstableTime * 1000)//如果超过15秒加载力还未稳定，则终止检测
                            {
                                Thread.Sleep(100);
                                showLedSingleRow(1, "检测中止");
                                showLedSingleRow(2, "加载力超时未稳定");
                                Msg(labelTS, panelTS, "加载力超时未稳定，检测中止", false);
                                igbt.Exit_Control();
                                return;
                            }
                            Thread.Sleep(50);
                        }

                        Msg(labelTS, panelTS, "加载完成,等待速度稳定", true);
                        showLed("加载完成　　　　", "等待速度稳定　　");
                        Thread.Sleep(1000);
                        List<double> speedStableList = new List<double>();
                        speedStableList.Add(speednow);
                        int stableCountWait = 0;
                        for (int i = 0; i < 29; i++)
                        {
                            stableCountWait++;
                            Thread.Sleep(80);
                            showLedSingleRow(1, "当前速度:" + addLength(speednow.ToString("0.0"), 4));
                            Msg(labelTS, panelTS, "Vw:" + speedStableList[0].ToString("0.0") + " V:" + speednow.ToString("0.0")+" "+(stableCountWait*0.1).ToString("0.0")+"s", true);
                            speedStableList.Add(speednow);
                        }
                        while (true)
                        {
                            stableCountWait++;
                            Vw = speedStableList[0];
                            showLedSingleRow(1, "当前速度:" + addLength(speednow.ToString("0.0"), 4));
                            Msg(labelTS, panelTS, "Vw:" + speedStableList[0].ToString("0.0") + " V:" + speednow.ToString("0.0") + " " + (stableCountWait * 0.1).ToString("0.0") + "s", true);
                            if (speedStableList.Min() - Vw >= -dynconfigdata.DynVwSdqj && speedStableList.Max() - Vw <= dynconfigdata.DynVwSdqj)
                            {
                                break;
                            }
                            else
                            {
                                speedStableList.Add(speednow);
                                speedStableList.RemoveAt(0);
                            }
                            if (speedStableList.Min() < Ve && speedStableList.Max() < Ve)
                            {
                                Vw = speedStableList.Min();
                                break;
                            }
                            if (stableCountWait >= dynconfigdata.DynMaxWaitTime*10)//如果超过最长等待稳定时间,即停止测试,取当前速度为稳定车速
                            {
                                Vw = speednow;
                                break;
                            }
                            Thread.Sleep(40);
                        }

                        Msg(labelTS, panelTS, "Vw已测取,Vw=" + Vw.ToString("0.0") + "km/h", true);
                        showLed(" Vw测定完毕: ", " " + addLength(Vw.ToString("0.0"), 4) + "km/h");
                        showCurve = false;//在第一时间停止图表的显示
                        Thread.Sleep(2000);
                        igbt.Exit_Control();
                        bool pdjg = false;
                        if (Vw >= Ve)
                        {
                            pdjg = true;
                            pdjgstring = "合格";
                            Msg(labelTS, panelTS, "Vw≥Ve" + " 合格", true);
                            showLedSingleRow(1, "　　　合格　　　");
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            pdjg = false;
                            pdjgstring = "不合格";
                            Msg(labelTS, panelTS, "Vw＜Ve" + " 不合格", true);
                            showLedSingleRow(1, "　　不合格　　　");
                            Thread.Sleep(1000);
                        }
                        Thread.Sleep(1000);
                        Thread.Sleep(1000);
                        if (carinfo.动力检测类型 == 1 && levelJudgeStep == 1 && !pdjg)
                        {
                            levelJudgeStep = 2;
                            goto StartPosition;
                        }
                        Msg(labelTS, panelTS, "检测完成,请减速停车", true);
                        showLed("检测完成　　　　", "请减速停车　　　");
                        DataTable dyn_datatable = new DataTable();
                        DataRow dr = null;
                        try
                        {
                            dyn_datatable.Columns.Add("速度");
                            dyn_datatable.Columns.Add("扭力");
                            dyn_datatable.Columns.Add("功率");
                            dyn_datatable.Columns.Add("达标速度");
                            for (int i = 0; i < speedlist.Count; i++)//将数据写入逐秒数据
                            {
                                dr = dyn_datatable.NewRow();
                                dr["速度"] = speedlist[i].ToString("0.0");
                                dr["扭力"] = forcelist[i].ToString("0");
                                dr["功率"] = powerlist[i].ToString("0.0");
                                dr["达标速度"] = Ve.ToString("0.0");
                                dyn_datatable.Rows.Add(dr);
                            }
                        }
                        catch (Exception er)
                        {
                            MessageBox.Show("生成过程数据时出错：" + er.Message);
                            return;
                        }
                        csvwriter.SaveCSV(dyn_datatable, "D:/dataseconds/" + carinfo.车辆号牌 + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv");
                        writeDieselResult();
                        carini.changeStatusData("4", "动力性检测完成");
                        Thread.Sleep(3000);
                        //System.IO.File.Delete(@"c:\jcdatatxt\carinfo.ini");
                        while (speednow > 0.1)
                            Thread.Sleep(1000);
                        igbt.Lifter_Up();
                        Thread.Sleep(5000);//等待举升升起后出车
                        Msg(labelTS, panelTS, "检测完毕", true);
                        showLed("　　检测完毕　　", "　　　　　　　　");
                        Thread.Sleep(2000);
                        showLed("　　欢迎参检　　", "　　　　　　　　");
                        this.Close();
                        #endregion
                    }
                }
            }
            catch
            { }

        //功率检测
        /*if (checkBox2.Checked == true)
        {
            try
            {
                if (carinfo.燃料种类 == 0)
                {
                    JC_Status = false;
                    Msg(labelTS, panelTS, "准备功率检测", false);
                    Thread.Sleep(3000);
                    igbt.Lifter_Down();
                    CX:;
                    Msg(labelTS, panelTS, "请使用直接档", false);
                    Thread.Sleep(1000);
                    Msg(labelCPH, panelCPH, "逐步加速", false);
                    Msg(labelTS, panelTS, "准备扫描转速", false);
                    Thread.Sleep(2000);

                    Msg(labelCPH, panelCPH, "目标转速:", false);
                    Msg(labelTS, panelTS, carinfo.点燃式额定扭矩转速.ToString(), false);
                    isZsStable = false;
                    int stableCount = 0;
                    for(int i=0;i<300;i++)
                    {
                        Thread.Sleep(100);
                        if (Math.Abs(enginezs - carinfo.点燃式额定扭矩转速) < 200)
                        {
                            stableCount++;
                            if (stableCount > 30)
                            {
                                Vm = igbt.Speed;
                                if(Vm>80)
                                {

                                }
                            }
                        }
                        else
                        {
                            stableCount = 0;
                        }
                    }

                    Msg(labelTS, panelTS, "扫描稳定车速", false);
                    Thread.Sleep(5000);
                    int DJS = 5;
                    for (int i = 0; i < 5; i++)
                    {
                        Msg(labelTS, panelTS, "采集稳定车速" + DJS.ToString(), false);
                        Thread.Sleep(1000);
                        DJS = 5 - i;
                    }

                    //取稳定车速
                    float CGspeedMax;
                    CGspeedMax = igbt.Speed;

                    if (carinfo.是否危险货物运输车辆 == 0)
                    {
                        if (CGspeedMax <= 95)
                        {
                            CGspeedMax = (float)(CGspeedMax * 0.86);
                        }
                        else
                        {
                            Msg(labelCPH, panelCPH, "车速过高", false);
                            Msg(labelTS, panelTS, "降档重新扫描", false);
                            goto CX;
                        }
                    }
                    else
                    {
                        if (CGspeedMax <= 80) //危险品车
                        {
                            CGspeedMax = (float)(CGspeedMax * 0.86);
                        }
                        else
                        {
                            Msg(labelCPH, panelCPH, "车速过高", false);
                            Msg(labelTS, panelTS, "降档重新扫描", false);
                            goto CX;
                        }
                    }


                    Msg(labelTS, panelTS, "稳定车速:" + CGspeedMax.ToString(), false);
                    Thread.Sleep(2000);
                    Msg(labelTS, panelTS, "开始恒力加载", false);
                    igbt.Set_Control_Force(0);
                    Thread.Sleep(200);
                    igbt.Start_Control_Force();
                    for (int i = 0; i < 5; i++)//逐渐加载,以免对车速影响太大
                    {
                        igbt.Set_Control_Force((float)(carinfo.CGforce * i / 4.0));
                        Msg(labelTS, panelTS, "加载中:" + (i + 1), false);
                        Thread.Sleep(2000);
                    }

                    Msg(labelTS, panelTS, "稳定车速采集", false);
                    int stableTime = 3;
                    while (stableTime >= 0)
                    {
                        if (Math.Abs(igbt.Force - carinfo.CGforce) > 20)
                        {
                            Msg(labelTS, panelTS, "扭力波动超±20", false);
                            Thread.Sleep(900);
                            stableTime = 3;
                            continue;
                        }
                        else
                        {
                            Msg(labelTS, panelTS, "稳定车速采集", false);
                        }
                        Thread.Sleep(900);
                        stableTime--;
                    }

                    Msg(labelTS, panelTS, "开始采集车速", false);
                    Thread.Sleep(500);
                    ccsj = 0;
                    int testTime = 5;
                    float UPspeed;

                    while (testTime > 0)
                    {
                        UPspeed = igbt.Speed;
                        Thread.Sleep(900);
                        Msg(labelTS, panelTS, "车速采集中", false);
                        if (Math.Abs(igbt.Speed - UPspeed) > 0.5)
                        {
                            ccsj += 1;
                            if (ccsj >= 3)
                            {
                                Msg(labelTS, panelTS, "速度超差", false);
                                Thread.Sleep(900);
                                Msg(labelTS, panelTS, "重新计时", false);
                                Thread.Sleep(900);
                                testTime = 5;
                                continue;
                            }
                        }
                        else
                        {
                            ccsj = 0;
                        }
                        testTime--;
                    }

                    CGspeedMax = igbt.Speed;

                    Msg(labelTS, panelTS, "测量完毕", false);
                    Thread.Sleep(1000);
                    igbt.Exit_Control();
                    Thread.Sleep(1000);

                    igbt.Lifter_Up();

                    Thread.Sleep(1000);
                    Msg(labelTS, panelTS, "上传测功结果", false);
                    Thread.Sleep(1000);
                    carini.writeCgjResultIni(carinfo.lsh, CGspeedMax.ToString());
                    Msg(labelTS, panelTS, "上传完毕", false);
                    Thread.Sleep(1000);
                    System.IO.File.Delete(@"c:\jcdatatxt\YHY.ini");
                    RefreshUI();
                    this.Close();
                }
            }
            catch
            { }
        }*/


        }
        private void showResult()
        {
            showTextboxText(textBoxVw, yhPerHundred.ToString("0.0"));
            showTextboxText(textBoxVmOrVe, yhXz.ToString("0.0"));
            showTextboxText(textBox判定, pdjgstring);
        }
        yhrRealTimeTotalData yhydata = new yhrRealTimeTotalData();
        yhrRealTimeData yhydata2 = new yhrRealTimeData();
        yhrRealTimePfwsszl yhydata3 = new yhrRealTimePfwsszl();
        

        private void RefreshUI()
        {
            Msg(labelTS, panelTS, "无待检车辆", true);            
        }

        Thread Th_get_FqandLl = null;
        Thread TH_ST = null;
        int GKSJ = 0;
        double ccsj = 0;


        public static List<float> speedlist = new List<float>();
        public static List<float> forcelist = new List<float>();
        public static List<float> powerlist = new List<float>();
        private DateTime starttime = DateTime.Now;
        private int gksj = 0;
        public static float speednow=0, forcenow=0, powernow=0;
        private bool showCurve = false;
        public static bool isReloadData = false;
        public static float curvescalevalue = 0;
        public static bool displayCurveScale = false;
        private int showCount = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            if(igbt!=null)
            {
                if (isTestSpeedPerod)
                {
                    speednow = igbt.Speed;
                }
                else
                {
                    speednow = (float)(igbt.Speed*dynconfigdata.DynSdxs);
                }
                forcenow = (float)(igbt.Force/ (carinfo.加载力比例 * dynconfigdata.DynNlxs));
                powernow = (float)((speednow / 3.6 * forcenow) / 1000);
                if (isTestSpeedPerod)
                {
                    Msg(labelSd, panelSd, "-.-", false);
                    aquaGauge1.Value = 0;
                }
                else
                {
                    Msg(labelSd, panelSd, speednow.ToString("0.0"), false);
                    if (speednow > 0) aquaGauge1.Value = speednow;
                    else aquaGauge1.Value = 0;
                }
                Msg(labelZs, panelZs, enginezs.ToString("0"), false);
                Msg(labelNl, panelNl, forcenow.ToString("0"), false);
                if (forcenow > 0) aquaGaugeRev.Value = forcenow;
                else aquaGauge3.Value = 0;
                aquaGaugeRev.Value = enginezs;
                if(JC_Status)
                {
                    panelSpeedOutRange.Size=new Size((int)(200*ccsj/3.0),36);
                }
            }
            if (showCount <= 5)
            {
                showCount = 0;
                if (isTestSpeedPerod)
                {
                    if (showform != null) showform.DriverShow_OnMyChange(1, "-.-");
                }
                else
                {
                    if (showform != null) showform.DriverShow_OnMyChange(1, speednow.ToString("0.0"));
                }
                if (showform != null) showform.DriverShow_OnMyChange(2, forcenow.ToString("0"));
                if (showform != null) showform.DriverShow_OnMyChange(3, powernow.ToString("0.0"));
                if (showform != null) showform.DriverShow_OnMyChange(4, enginezs.ToString("0"));
            }
            else
            {
                showCount++;
            }
            if (showCurve)
            {
                TimeSpan timesp = DateTime.Now - starttime;
                if ((int)(timesp.TotalMilliseconds / 100) > gksj)
                {

                    gksj = (int)(timesp.TotalMilliseconds / 100);
                    if (jcStatus)
                        distance += speednow / 3.6 * 0.1;

                    if (gksj % 10 == 0)
                    {
                        isReloadData = true;
                        speedlist.Add(speednow);
                        forcelist.Add(forcenow);
                        powerlist.Add(powernow);
                        /*
                            speedlist.Add((float)(Math.Sin((double)gksj/900.0*2*Math.PI)*50+50));
                            forcelist.Add((float)(Math.Cos((double)gksj / 900.0 * 2 * Math.PI)* 500+500));
                            powerlist.Add((float)(yhydata.ZYH));
                        */    
                        if (speedlist.Count > 90)
                        {
                            speedlist.RemoveAt(0);
                            forcelist.RemoveAt(0);
                            powerlist.RemoveAt(0);
                        }
                        isReloadData = false;
                        curve1.Values = speedlist.ToArray();
                        curve1.Fit();
                        pictureBox速度.Image = curve1.CreateImage();
                        curve2.Values = forcelist.ToArray();
                        curve2.Fit();
                        pictureBox扭力.Image = curve2.CreateImage();
                    }
                }
            }
        }

        private void yhTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (Th_get_FqandLl != null) Th_get_FqandLl.Abort();
            }
            catch
            { }
            try
            {
                if (TH_ST != null) TH_ST.Abort();
            }
            catch
            { }
            timer2.Stop();
            try
            {
                if (flb_100 != null)
                {
                    if (flb_100.ComPort_3.IsOpen)
                        flb_100.ComPort_3.Close();
                }
                if (fla_502 != null)
                {
                    if (fla_502.ComPort_1.IsOpen)
                        fla_502.ComPort_1.Close();
                }
                if (fla_501 != null)
                {
                    if (fla_501.ComPort_1.IsOpen)
                        fla_501.ComPort_1.Close();
                }
                if (flv_1000 != null)
                {
                    if (flv_1000.ComPort_1.IsOpen)
                        flv_1000.ComPort_1.Close();
                }
                if (ledcontrol != null)
                {
                    showLed("　　　　　　　　", "　　　　　　　　");
                    if (ledcontrol.ComPort_2.IsOpen)
                        ledcontrol.ComPort_2.Close();
                }
                if (vmt_2000 != null)
                {
                    if (vmt_2000.ComPort_1.IsOpen)
                        vmt_2000.ComPort_1.Close();
                }
                if (rpm5300 != null)
                {
                    rpm5300.closeEquipment();
                }
                if (yhy != null)
                {
                    if (yhy.ComPort_1.IsOpen)
                        yhy.ComPort_1.Close();
                }
                if (igbt != null)
                {
                    igbt.Exit_Control();
                    igbt.closeIgbt();
                }
                if (xce_100 != null)
                {
                    if (xce_100.ComPort_1.IsOpen)
                        xce_100.ComPort_1.Close();
                }
            }
            catch
            { }
        }

        public void Fq_Detect()
        {
            while (true)
            {
                if (showCurve)
                {
                    if (dynconfigdata.Zsj == "烟度计")
                    {
                        if (equipdata.IsOldMqy200)
                        {
                            enginezs = (int)(flb_100.get_DirectData().Zs);
                        }
                        else
                        {
                            enginezs = (int)(flb_100.get_Data().Zs);
                        }
                    }
                    else if (dynconfigdata.Zsj == "废气仪")
                    {
                        enginezs = (int)(fla_502.GetData().ZS);
                    }
                    else if (rpm5300 != null)
                    {
                        enginezs = (int)rpm5300.ZS;
                    }
                    else if (vmt_2000 != null)
                    {
                        if (vmt_2000.readRotateSpeed())
                            enginezs = vmt_2000.zs;
                    }
                }
                Thread.Sleep(30);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (jcStatus == false)
                {
                    panelSpeedOutRange.Size = new Size(10, 36);
                    if (igbt != null)
                    {
                        if (!igbt.isComSuccess)
                        {
                            MessageBox.Show("测功机通讯不正常,不能开始检测", "系统错误");
                            return;
                        }
                        igbt.Force_Zeroing();
                    }
                    else
                    {
                        MessageBox.Show("测功机未打开,不能开始检测", "系统错误");
                        return;
                    }

                    isTestSpeedPerod = false;
                    gksj_count = 0;
                    JC_Status = false;
                    //清空图表的显示
                    showCurve = false;
                    speedlist.Clear();
                    forcelist.Clear();
                    powerlist.Clear();
                    curve1.displayScale1 = false;
                    curve2.displayScale1 = false;
                    curve1.displayScale2 = false;
                    curve2.displayScale2 = false;
                    //开妈检测过程
                    TH_ST = new Thread(Jc_Exe); //检测过程
                    TH_ST.Start();
                    Th_get_FqandLl = new Thread(Fq_Detect);
                    Th_get_FqandLl.Start();
                    GKSJ = 0;
                    button1.Text = "停止检测";
                    jcStatus = true;
                    //button2.Enabled = false;
                }
                else
                {
                    showCurve = false;//暂停图表的显示 
                    resetStatus();//将状态位置0
                    igbt.Exit_Control();
                    JC_Status = false;
                    jcStatus = false;
                    
                    try
                    {
                        TH_ST.Abort();
                    }
                    catch
                    { }
                    try
                    {
                        Th_get_FqandLl.Abort();
                    }
                    catch
                    { }
                    GKSJ = 0;
                    gksj_count = 0;
                    //igbt.Lifter_Up(); //升起台体
                    //RefreshUI();
                    Msg(labelTS, panelTS, "检测被中止", true);
                    showLed("　　检测中止　　", "　　　　　　　　");
                    button1.Text = "开始检测";
                    //button2.Enabled = true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("操作失败，请检查仪器工作是否正常", "系统错误");
            }
        }


    }
}
