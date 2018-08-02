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
using System.Data.OleDb;


namespace 动力油耗
{
    public partial class dynamicTest : Form
    {
        private const string flxh = "fly_2000";
        private const string mqxh = "mql_8201";
        private const string nhxh = "nhty_1";
        WeightWCFServer.LZServicesClient lzserver = null;
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
        public delegate void wtcs(Control controlname, string text);
        public delegate void wtcc(Control controlname, bool ischecked);
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
        public List<byte> keylist = new List<byte>();
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
        private string yhpdjg;
        private string yhpdjgstring;
        private double yhXz = 0;
        private int xzyj = 0;
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

            if (carinfo.油耗车速方式 == 0)
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
            }
            else
            {
                v0 = carinfo.油耗检测工况速度;
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
                        if (equipdata.Cgjxh == "DCG-26L|DCG-26LD")
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
            if (checkBox牵引车满载总质量.Checked)
            {
                Y_Ft = carinfo.牵引车满载总质量 * g * f;
            }
            else
            {
                Y_Ft = carinfo.额定总质量 * g * f;
            }
            A = carinfo.汽车前轮距 * carinfo.汽车高度 * 0.000001;
            Y_Fw = 0.5 * Cd * A * p * (v0 / 3.6) * (v0 / 3.6);
            Y_FR = Y_Ft + Y_Fw;
            fc = 1.5 * f;
            Y_Ffc = carinfo.驱动轴空载质量 * g * fc;
            Y_Fc = Y_Ftc + Y_Ffc;
            Y_FTC = Y_FR - Y_Fc;
            Y_FTC = Math.Round(Y_FTC, 0);//台架加载阻力，四舍五入至整数位，单位为N
            if (Y_FTC < 0)
            {
                MessageBox.Show("油耗加载力计算结果为负值，请核对车辆信息是否准确完整", "错误");
            }
            else if (Y_FTC < 150)
            {
                dynconfigdata.FuelForceQj = 150;
                MessageBox.Show("油耗加载力计算结果较低，请核对车辆信息是否准确完整", "警告");
            }
            else if(dynconfigdata.DynStopUnstable)
            {
                dynconfigdata.FuelForceQj = FuelForceQj+(float)(Y_FTC * 0.03);
            }
            
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
        private string xnpdjgstring;
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
                dynconfigdata.DynForceQj = DynForceQj+(float)(X_FE * 0.03);
            }
            showTextboxText(textBoxD_FE, X_FE.ToString("0"));
            showTextboxText(textBoxD_Fe1, X_Fe.ToString("0"));
            showTextboxText(textBoxD_η, η.ToString("0.000"));
            showTextboxText(textBoxD_Pe, Pe.ToString("0"));
            //showTextboxText(textBoxD_nm, nm.ToString("0"));
            showTextboxText(textBoxD_Ftc, X_Ftc.ToString("0"));
            showTextboxText(textBoxD_Fc, X_Fc.ToString("0"));
            showTextboxText(textBoxD_Ff, X_Ff.ToString("0"));
            showTextboxText(textBoxD_Ft, X_Ft.ToString("0"));
            showTextboxText(textBoxD_αa, αd.ToString("0.000"));
            showTextboxText(textBoxD_Ve, Ve.ToString("0.0"));
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
            showTextboxText(textBoxD_FE, X_FM.ToString("0"));
            showTextboxText(textBoxD_Fe1, X_Fm.ToString("0"));
            showTextboxText(textBoxD_η, η.ToString("0.000"));
            showTextboxText(textBoxD_Pe, Mm.ToString("0"));
            showTextboxText(textBoxD_nm, nm.ToString("0"));
            showTextboxText(textBoxD_Ftc, X_Ftc.ToString("0"));
            showTextboxText(textBoxD_Fc, X_Fc.ToString("0"));
            showTextboxText(textBoxD_Ff, X_Ff.ToString("0"));
            showTextboxText(textBoxD_Ft, X_Ft.ToString("0"));
            showTextboxText(textBoxD_αa, αa.ToString("0.000"));
            showTextboxText(textBoxD_Ve, Vm.ToString("0.0"));
        }

        private void writeDieselResult(string mmsd, string mmnl, string mmgl)
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
            resultdata.判定结果 = (xnpdjgstring == "合格" ? 0 : 1);
            if (carinfo.动力检测类型 == 0)
                resultdata.等级评定结果 = 0;
            else if (carinfo.动力检测类型 == 1)
                resultdata.等级评定结果 = levelJudgeStep;
            else if (carinfo.动力检测类型 == 3)
                resultdata.等级评定结果 = 1;
            else
                resultdata.等级评定结果 = 2;
            resultdata.速度曲线 = mmsd;
            resultdata.扭力曲线 = mmnl;
            resultdata.功率曲线 = mmgl;
            showTextboxText(textBoxD_Vw, Vw.ToString("0.0"));
            showTextboxText(textBoxD_Ve, Ve.ToString("0.0"));
            showTextboxText(textBoxD_PD, xnpdjgstring);
            carini.writeDieselDynamicJson(resultdata,dynconfigdata.DynFileStyle);
        }
        private void writeGasolineResult(string mmsd, string mmnl, string mmgl)
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
            resultdata.判定结果 = (xnpdjgstring == "合格" ? 0 : 1);
            if (carinfo.动力检测类型 == 0)
                resultdata.等级评定结果 = 0;
            else if (carinfo.动力检测类型 == 1)
                resultdata.等级评定结果 = levelJudgeStep;
            else if (carinfo.动力检测类型 == 3)
                resultdata.等级评定结果 = 1;
            else
                resultdata.等级评定结果 = 2;
            resultdata.速度曲线 = mmsd;
            resultdata.扭力曲线 = mmnl;
            resultdata.功率曲线 = mmgl;
            showTextboxText(textBoxD_Vw, Vw.ToString("0.0"));
            showTextboxText(textBoxD_Ve, Vm.ToString("0.0"));
            showTextboxText(textBoxD_PD, xnpdjgstring);
            carini.writeGasolineDynamicJson(resultdata, dynconfigdata.DynFileStyle);
        }
        private void writeSpeedTestResult()
        {
            SpeedResult resultdata = new SpeedResult();
            resultdata.车速 = speedTestResult;
            resultdata.车速判定结果 = speedTestPdjg;
            carini.writeSpeedJson(resultdata, dynconfigdata.DynFileStyle);
        }
        private void writeQdzzlResult()
        {
            QdzzlResult resultdata = new QdzzlResult();
            resultdata.驱动轴空载质量 = qdzzz_scz;
            carini.writeQdzzlJson(resultdata, dynconfigdata.DynFileStyle);
        }
        private int yhsmallthanxzcount = 0;
        /// <summary>
        /// 写燃料消耗测量结果
        /// </summary>
        private void writeFuelResult(string mmsd,string mmnl,string mmyh)
        {
            fuelResult resultdata = new fuelResult();
            resultdata.检测速度 = v0;
            resultdata.台架加载阻力 = Y_FTC;
            resultdata.燃料总消耗量 = totalYh;
            resultdata.总行驶里程 = totalDistance;
            resultdata.百公里燃料消耗量 = yhPerHundred;
            resultdata.限值 = yhXz;
            resultdata.限值依据 = xzyj;
            resultdata.判定结果 = (yhpdjg == "合格" ? 0 : 1);
            resultdata.汽车滚动阻力 = Y_Ft;
            resultdata.空气阻力 = Y_Fw;
            resultdata.滚动阻力系数 = f;
            resultdata.迎风面积 = A;
            resultdata.空气阻力系数 = Cd;
            resultdata.台架运转阻力 = Y_Fc;
            resultdata.台架滚动阻力 = Y_Ffc;
            resultdata.台架滚动阻力系数 = fc;
            resultdata.台架内阻 = Y_Ftc;
            resultdata.速度曲线 = mmsd;
            resultdata.扭力曲线 = mmnl;
            resultdata.油耗曲线 = mmyh;
            
            carini.writeYhyResultJson(resultdata, dynconfigdata.DynFileStyle);
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
        public float FuelForceQj = 0;
        private void yhTest_Load(object sender, EventArgs e)
        {
            //aquaGauge1.Value = 65f;
            //aquaGaugeRev.Value = 4500;
            carinfo = carini.getYhCarIniXn();
            
            initCurve();
            equipdata = configini.getEquipConfigIni();
            dynconfigdata = configini.getDynConfigIni();
            DynForceQj = dynconfigdata.DynForceQj;
            FuelForceQj = dynconfigdata.FuelForceQj;
            initKeyList();//初始化按键值
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
                if (carinfo.是否检测油耗)
                {
                    if (carinfo.货车车身型式 == 2 && carinfo.汽车类型 == 1)
                    {
                        checkBox牵引车满载总质量.Enabled = true;
                    }
                    else
                    {
                        checkBox牵引车满载总质量.Enabled = false;
                    }
                    if (carinfo.额定总质量 < 3500)
                    {
                        MessageBox.Show("总质量小于3.5t，不能检测油耗", "提示");
                        carinfo.是否检测油耗 = false;
                    }
                }
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
        public void initKeyList()
        {
            if(dynconfigdata.DynYkKey!="")
            {
                string[] keyarray = dynconfigdata.DynYkKey.Split('|');
                for(int i=0;i<keyarray.Count();i++)
                {
                    string keystring = keyarray[i];
                    byte keyvalue = 0;
                    if(byte.TryParse(keystring,out keyvalue))
                        keylist.Add(keyvalue);
                }
                if(keylist.Count==0)
                {
                    keylist.Add(1);
                    keylist.Add(2);
                    keylist.Add(3);
                    keylist.Add(4);
                    keylist.Add(5);
                    keylist.Add(6);
                    keylist.Add(7);
                    keylist.Add(8);
                    keylist.Add(9);
                    keylist.Add(10);
                    keylist.Add(11);
                    keylist.Add(12);
                }
            }
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
            labelJCLSH.Text = carinfo.检测流水号;
            labelCLHP.Text = carinfo.车辆号牌;
            labelJCXZ.Text = carinfo.检测性质;
            switch(carinfo.动力检测类型)
            {
                case 0:
                    labelDLXPJBZ.Text = "二级";
                    break;
                case 1:
                    labelDLXPJBZ.Text = "自动等级评定";
                    break;
                case 2:
                    labelDLXPJBZ.Text = "二级";
                    break;
                case 3:
                    labelDLXPJBZ.Text = "一级";
                    break;
                default:break;
            }
            labelSFKC.Text = carinfo.汽车类型==0 ? "是" : "否";
            labelZZL.Text = carinfo.额定总质量.ToString("0");
            labelJGL.Text=carinfo.压燃式功率参数类型==1? "是" : "否";
            labelEDGL.Text = carinfo.压燃式额定功率.ToString("0.0");
            labelEDNJ.Text = carinfo.点燃式额定扭矩.ToString("0");
            labelEDZS.Text = carinfo.点燃式额定扭矩转速.ToString("0");
            labelEDYH.Text = carinfo.油耗限值.ToString("0.0");
            labelLTLX.Text = carinfo.轮胎类型==0?"子午线":"斜交";
            labelDMKD.Text = carinfo.子午胎轮胎断面宽度.ToString("0.00");
            labelQCGD.Text = carinfo.汽车高度.ToString("0");
            labelQLJ.Text = carinfo.汽车前轮距.ToString("0");
            labelQCCD.Text = carinfo.客车车长.ToString("0");
            labelKCDJ.Text = carinfo.客车等级==0?"高级":(carinfo.客车等级 == 1 ? "中级" :"普通");
            switch (carinfo.货车车身型式)
            {
                case 0:
                    labelHCCSXS.Text = "拦板车";
                    break;
                case 1:
                    labelHCCSXS.Text = "自卸车";
                    break;
                case 2:
                    labelHCCSXS.Text = "牵引车";
                    break;
                case 3:
                    labelHCCSXS.Text = "仓栅车";
                    break;
                case 4:
                    labelHCCSXS.Text = "厢式车";
                    break;
                case 5:
                    labelHCCSXS.Text = "罐车";
                    break;
                default: break;
            }
            if (carinfo.驱动轴质量方式 == 0)
            {
                labelQDZKZZL.Text = carinfo.驱动轴空载质量.ToString("0");
            }
            else
            {
                labelQDZKZZL.Text = "待称重";
                labelQDZKZZL.ForeColor = Color.Red;
            }
            labelQYCMZZL.Text = carinfo.牵引车满载总质量.ToString("0");
            labelYHXZ.Text = carinfo.油耗限值.ToString("0.0");
            labelYHCS.Text = carinfo.油耗检测工况速度.ToString("0.0");
            labelRLZL.Text = carinfo.燃料种类==0?"汽油":"柴油";
            labelQDZS.Text = carinfo.驱动轴数.ToString("0");
            checkBoxCS.Checked = carinfo.是否检测车速;
            checkBoxDYN.Checked = carinfo.是否检测动力性;
            checkBoxFUEL.Checked = carinfo.是否检测油耗;
            checkBoxQdzcz.Checked = (carinfo.驱动轴质量方式 == 1);
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

            curve3.BgColor = Color.Black;
            curve3.TextColor = Color.WhiteSmoke;
            curve3.SliceColor = Color.WhiteSmoke;
            curve3.AxisColor = Color.WhiteSmoke;
            curve3.AxisTextColor = Color.WhiteSmoke;
            curve3.BorderColor = Color.WhiteSmoke;
            curve3.SliceTextColor = Color.WhiteSmoke;
            curve3.Width = pictureBox油耗.Width;
            curve3.Height = pictureBox油耗.Height;
            curve3.Title = "油耗曲线";
            curve3.XAxisText = "时间(s)";
            curve3.YAxisText = "油耗(mL)";
            curve3.YSliceValue = 100;
            curve3.Keys = new string[] { "0", "10", "20", "30", "40", "50", "60", "70", "80", "90" };
            curve3.Values = new float[] { 0f };
            curve3.Fit();
            pictureBox油耗.Image = curve3.CreateImage();
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
            try
            {
                if (equipdata.useWeightWCF)
                {                    
                    lzserver = new WeightWCFServer.LZServicesClient("BasicHttpBinding_ILZServices",equipdata.WeightWCFaddress);
                    WeightWCFServer.BackDataTpye lzstatus=lzserver.GetTestStatus();
                }
            }
            catch(Exception er)
            {
                lzserver = null;
                MessageBox.Show("轴重WCF连接出现异常:" + er.Message);
                //throw (new Exception(er.Message));
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
        #region
        private bool qdzzzfinished = false;
        private int qdzzz_scz = 0;
        /// <summary>
        /// 驱动轴轴重测试
        /// </summary>
        private void QdzWeight_Exe()
        {
            try
            {
                qdzzz_scz = 0;
                WeightWCFServer.BackDataTpye lzstatus = lzserver.GetTestStatus();
                showTextboxText(textBoxLzts1, "读取轴重工位状态");
                showTextboxText(textBoxQdzStatus, lzstatus.Result);
                if (lzstatus.Code == 1)
                {
                    if (MessageBox.Show("有车辆正在检测，是否中止正在进行的检测？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        lzstatus = lzserver.SetTestStop();
                        Thread.Sleep(200);
                        showTextboxText(textBoxLzts1, "停止当前测量");
                        showTextboxText(textBoxQdzStatus, lzstatus.Result);
                    }
                    else
                        return;
                }
                if (lzstatus.Code == 2)
                {
                    lzstatus = lzserver.SetTestStop();
                    Thread.Sleep(200);
                    showTextboxText(textBoxLzts1, "停止当前测量");
                    showTextboxText(textBoxQdzStatus, lzstatus.Result);
                }
                if (lzstatus.Code == -1)
                {
                    lzstatus = lzserver.SetTestStop();
                    Thread.Sleep(200);
                    showTextboxText(textBoxLzts1, "停止当前测量");
                    showTextboxText(textBoxQdzStatus, lzstatus.Result);
                }
                Msg(labelTS, panelTS, "等待驱动轴称重完成", true);
                showLedSingleRow(2, "驱动轴称重中...");
                qdzzzfinished = true;
                WeightWCFServer.InputDataType lzcarinf = new WeightWCFServer.InputDataType();
                lzcarinf.CLHP = carinfo.车辆号牌;
                lzcarinf.ZS = carinfo.驱动轴数;
                lzstatus = lzserver.SetTestStart(lzcarinf);
                showTextboxText(textBoxLzts1, "开始轴重测量");
                showTextboxText(textBoxQdzStatus, lzstatus.Result);
                if (lzstatus.Code != 1)
                {
                    Msg(labelTS, panelTS, "轴重测量开启失败", true);
                    showLedSingleRow(2, "轴重测量开启失败");
                    return;
                }
                Thread.Sleep(200);
                lzstatus = lzserver.GetTestStatus();
                while (lzstatus.Code == 1)
                {
                    lzstatus = lzserver.GetTestStatus();
                    showTextboxText(textBoxLzts1, "读取轴重工位状态");
                    showTextboxText(textBoxQdzStatus, lzstatus.Result);
                    Thread.Sleep(1000);
                }
                int code = lzstatus.Code;
                if (code == 2)
                {
                    lzstatus = lzserver.GetTestResult();
                    showTextboxText(textBoxLzts1, "读取测量结果");
                    showTextboxText(textBoxQdzStatus, lzstatus.Result);
                    if (int.TryParse(lzstatus.Result, out qdzzz_scz))
                    {
                        qdzzz_scz = int.Parse(lzstatus.Result);
                        qdzzzfinished = true;
                    }
                    else
                    {
                        qdzzzfinished = false;
                        Msg(labelTS, panelTS, "轴重测量结果非数字", true);
                        showLedSingleRow(2, "轴重测量结果无效");
                    }
                }
                else if (code == -1)
                {
                    showTextboxText(textBoxLzts1, "测量失败");
                    showTextboxText(textBoxQdzStatus, lzstatus.Result);
                }
                else
                {
                    showTextboxText(textBoxLzts1, "测量失败");
                    showTextboxText(textBoxQdzStatus, lzstatus.Result);
                }
            }
            catch(Exception er)
            {
                MessageBox.Show("轴重测量过程发生异常:"+er.Message);
            }
        }
        #endregion

        private void Jc_Exe()
        {           
            try
            {
                #region 驱动轴称重
                if (checkBoxQdzcz.Checked&&(carinfo.是否检测动力性||carinfo.是否检测油耗))//判断是否需要称轴重,前提 是要检测动力性和油耗才需要驱动轴重量
                {
                    if (lzserver == null)
                    {
                        Msg(labelTS, panelTS, "轴重工位连接失败", true);
                        showLedSingleRow(2, "轴重工位连接失败");
                        return;
                    }
                    qdzzzfinished = false;
                    QdzWeight_Exe();
                    if (qdzzzfinished)//称重成功
                    {
                        Ref_Control_Checked(checkBoxQdzcz, false);
                        carinfo.驱动轴空载质量 = qdzzz_scz;
                        writeQdzzlResult();
                        Msg_Show(labelQDZKZZL, qdzzz_scz.ToString("0"), false);
                        Msg(labelTS, panelTS, "驱动轴重量="+ qdzzz_scz.ToString("0")+"kg", true);
                        showLedSingleRow(2, "驱动轴称重完毕　");
                    }
                    else//称重失败
                    {
                        Msg(labelTS, panelTS, "驱动轴重量称重失败", true);
                        showLedSingleRow(2, "驱动轴称重失败　");
                        return;
                    }
                }
                if (carinfo.是否检测油耗)//有了驱动轴重量后计算油耗加载力，如果加载力有问题，可以预先提示
                {
                    caculateFTC();
                    showYhCarInf();
                    if (Y_FTC < 0)
                    {
                        Msg(labelTS, panelTS, "油耗加载力计算异常",false);
                        showLedSingleRow(2, "油耗加载力异常　");
                        return;
                    }
                }
                #endregion
                #region 到位判定
                if (dynconfigdata.DynUseGddw)
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
                                    byte keyvalue = (byte)(igbt.keyandgd >> 4);
                                    if (keylist.Contains(keyvalue))
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
                Thread.Sleep(3000);
                #endregion
                if (checkBoxDYN.Checked||checkBoxFUEL.Checked||checkBoxCS.Checked)//判断是否有可检项
                {

                    if (checkBoxCS.Checked)
                    {
                        #region 检测车速
                        workState = 1;
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
                                        byte keyvalue = (byte)(igbt.keyandgd >> 4);
                                        if (keylist.Contains(keyvalue))
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
                            checkBoxCS.Checked = false;
                        }
                        else
                        {
                            speedTestPdjg = 1;
                            Msg(labelTS, panelTS, speedTestResult.ToString("0.0") + "km/h" + " 不合格", true);
                            showLed(addLength(speedTestResult.ToString("0.0") + "km/h", 10), "　　不合格　　　");
                        }
                        showTextboxText(textBoxS_data, speedTestResult.ToString("0.0"));
                        showTextboxText(textBoxS_PD, speedTestPdjg == 0 ? "合格" : "不合格");
                        SpeedResult result = new SpeedResult();
                        writeSpeedTestResult();
                        Thread.Sleep(2000);
                        isTestSpeedPerod = false;
                        #endregion
                    }
                    if (checkBoxDYN.Checked)
                    {
                        #region 检测动力性
                        speedlist.Clear();
                        forcelist.Clear();
                        powerlist.Clear();
                        workState = 2;
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
                                if (ydjEnvironment.WD == 0 && ydjEnvironment.SD == 0 && ydjEnvironment.DQY == 0)
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
                            if(Wd==0&&Sd==0&&Dqy==0)
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
                        showTextboxText(textBoxD_wd, Wd.ToString("0.0"));
                        showTextboxText(textBoxD_sd, Sd.ToString("0.0"));
                        showTextboxText(textBoxD_dqy, Dqy.ToString("0.0"));
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
                        else if (carinfo.动力检测类型 == 3)
                        {
                            showLed("开始一级评定　　", "　　　　　　　　");
                            Msg(labelTS, panelTS, "开始一级评定", true);
                            Label_Msg(labelTestType, "开始一级评定测试");
                            Thread.Sleep(2000);
                        }
                        else if (carinfo.动力检测类型 == 2)
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
                                if (equipdata.isIgbtContainGdyk)
                                {
                                    if (dynconfigdata.DynYkStyle == 0)
                                    {
                                        if (((igbt.keyandgd) & 0xf0) != 0x00)
                                        {
                                            byte keyvalue = (byte)(igbt.keyandgd >> 4);
                                            if (keylist.Contains(keyvalue))
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
                            Msg(labelTS, panelTS, "加速使车速超过Vm", true);
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
                                if (speednow > Vm + 1)//加速使速度超过Vm且保持要求的时间以上，再开始加载
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
                                    igbt.Set_Control_Force((float)(X_FM * carinfo.加载力比例));
                                    showLedSingleRow(2, "开始加载:" + addLength(X_FE.ToString("0"), 4));
                                    Thread.Sleep(900);
                                }
                            }
                            else
                            {
                                for (int i = 1; i <= jzsj; i++)//逐渐加载,以免对车速影响太大
                                {
                                    igbt.Set_Control_Force((float)((X_FM * carinfo.加载力比例) * i * 1.0 / (1.0 * jzsj)));
                                    Msg(labelTS, panelTS, "加载中:" + (jzsj - i) + "s", true);
                                    showLedSingleRow(2, "开始加载:" + addLength(forcenow.ToString("0"), 4));
                                    Thread.Sleep(1000);
                                }
                            }
                            igbt.Set_Control_Force((float)(X_FM * carinfo.加载力比例));
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
                                Thread.Sleep(80);
                                showLedSingleRow(1, "当前速度:" + addLength(speednow.ToString("0.0"), 4));
                                Msg(labelTS, panelTS, "Vw:" + speedStableList[0].ToString("0.0") + " V:" + speednow.ToString("0.0") + " " + (stableCountWait * 0.1).ToString("0.0") + "s", true);
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
                                if (speedStableList.Min() < Vm && speedStableList.Max() < Vm)
                                {
                                    Vw = speedStableList.Min();
                                    break;
                                }
                                if (stableCountWait >= dynconfigdata.DynMaxWaitTime * 10)//如果超过最长等待稳定时间,即停止测试,取当前速度为稳定车速
                                {
                                    Vw = speednow;
                                    break;
                                }
                                Thread.Sleep(80);
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
                                xnpdjgstring = "合格";
                                Msg(labelTS, panelTS, "Vw≥Vm" + " 合格", true);
                                showLedSingleRow(1, "　　　合格　　　");
                                //checkBoxDYN.Checked = false;
                                Ref_Control_Checked(checkBoxDYN, false);
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                pdjg = false;
                                xnpdjgstring = "不合格";
                                Msg(labelTS, panelTS, "Vw＜Vm" + " 不合格", true);
                                showLedSingleRow(1, "　　不合格　　　");
                                Thread.Sleep(1000);
                            }
                            showTextboxText(textBoxD_Vw, Vw.ToString("0.0"));
                            showTextboxText(textBoxD_Ve, Vm.ToString("0.0"));
                            showTextboxText(textBoxD_PD, xnpdjgstring);
                            Thread.Sleep(1000);
                            Thread.Sleep(1000);
                            if (carinfo.动力检测类型 == 1 && levelJudgeStep == 1 && !pdjg)
                            {
                                levelJudgeStep = 2;
                                goto StartPosition;
                            }
                            DataTable dyn_datatable = new DataTable();
                            DataRow dr = null;
                            string mmsd = "", mmnl = "", mmgl = "";
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
                                    mmsd += speedlist[i].ToString("0.0");
                                    mmnl += forcelist[i].ToString("0.0");
                                    mmgl += powerlist[i].ToString("0.0");
                                    dyn_datatable.Rows.Add(dr);
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show("生成过程数据时出错：" + er.Message);
                                return;
                            }
                            csvwriter.SaveCSV(dyn_datatable, "D:/dataseconds/" + carinfo.车辆号牌 + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "dyn.csv");
                            writeGasolineResult(mmsd,mmnl,mmgl);
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
                                    igbt.Set_Control_Force((float)(X_FE * carinfo.加载力比例));
                                    showLedSingleRow(2, "开始加载:" + addLength(X_FE.ToString("0"), 4));
                                    Thread.Sleep(900);
                                }
                            }
                            else
                            {
                                for (int i = 1; i <= jzsj; i++)//逐渐加载,以免对车速影响太大
                                {
                                    igbt.Set_Control_Force((float)(X_FE * carinfo.加载力比例 * i * 1.0 / (jzsj * 1.0)));
                                    Msg(labelTS, panelTS, "加载中:" + (jzsj - i) + "s", true);
                                    showLedSingleRow(2, "开始加载:" + addLength(forcenow.ToString("0"), 4));
                                    Thread.Sleep(1000);
                                }
                            }
                            igbt.Set_Control_Force((float)(X_FE * carinfo.加载力比例));
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
                                Msg(labelTS, panelTS, "Vw:" + speedStableList[0].ToString("0.0") + " V:" + speednow.ToString("0.0") + " " + (stableCountWait * 0.1).ToString("0.0") + "s", true);
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
                                if (stableCountWait >= dynconfigdata.DynMaxWaitTime * 10)//如果超过最长等待稳定时间,即停止测试,取当前速度为稳定车速
                                {
                                    Vw = speednow;
                                    break;
                                }
                                Thread.Sleep(80);
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
                                xnpdjgstring = "合格";
                                Msg(labelTS, panelTS, "Vw≥Ve" + " 合格", true);
                                showLedSingleRow(1, "　　　合格　　　");
                                //checkBoxDYN.Checked = false;
                                Ref_Control_Checked(checkBoxDYN, false);
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                pdjg = false;
                                xnpdjgstring = "不合格";
                                Msg(labelTS, panelTS, "Vw＜Ve" + " 不合格", true);
                                showLedSingleRow(1, "　　不合格　　　");
                                Thread.Sleep(1000);
                            }
                            showTextboxText(textBoxD_Vw, Vw.ToString("0.0"));
                            showTextboxText(textBoxD_Ve, Ve.ToString("0.0"));
                            showTextboxText(textBoxD_PD, xnpdjgstring);
                            Thread.Sleep(1000);
                            Thread.Sleep(1000);
                            if (carinfo.动力检测类型 == 1 && levelJudgeStep == 1 && !pdjg)
                            {
                                levelJudgeStep = 2;
                                goto StartPosition;
                            }
                            DataTable dyn_datatable = new DataTable();
                            DataRow dr = null;
                            string mmsd = "", mmnl = "", mmgl = "";
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
                                    mmsd+= speedlist[i].ToString("0.0");
                                    mmnl += forcelist[i].ToString("0.0");
                                    mmgl += powerlist[i].ToString("0.0");
                                    dyn_datatable.Rows.Add(dr);
                                }
                            }
                            catch (Exception er)
                            {
                                MessageBox.Show("生成过程数据时出错：" + er.Message);
                                return;
                            }
                            csvwriter.SaveCSV(dyn_datatable, "D:/dataseconds/" + carinfo.车辆号牌 + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv");
                            writeDieselResult(mmsd,mmnl,mmgl);                            
                            #endregion
                        }
                        #endregion
                    }
                    if (checkBoxFUEL.Checked)
                    {
                        #region 油耗检测
                        speedlist.Clear();
                        forcelist.Clear();
                        powerlist.Clear();
                        workState = 3;
                        if (Y_FTC < 100)
                        {
                            curve2.YSliceValue = 10;
                        }
                        else if (Y_FTC < 1000)
                        {
                            curve2.YSliceValue = 100;
                        }
                        else if (Y_FTC < 10000)
                        {
                            curve2.YSliceValue = 1000;
                        }
                        else
                        {
                            curve2.YSliceValue = 2000;
                        }
                        flGetAddDataFlag = 0;
                        stopReadData = true;
                        Msg(labelTS, panelTS, "准备油耗检测", true);
                        showLed(carinfo.车辆号牌, "准备油耗检测　　");
                        Thread.Sleep(3000);
                        if (speednow > 0.5)
                        {
                            Msg(labelTS, panelTS, "请减速准备测试", true);
                            showLed(carinfo.车辆号牌, "请减速准备测试　");
                        }
                        else
                        {
                            Msg(labelTS, panelTS, "检测即将开始", true);
                            showLed(carinfo.车辆号牌, "检测即将开始　　");
                        }
                        Thread.Sleep(2000);
                        #region 调零及空气测定
                        Msg(labelTS, panelTS, "开始调零", true);
                        showLed(carinfo.车辆号牌, "开始调零　　　　");
                        yhy.setAirAsZero();
                        Thread.Sleep(500);
                        yhy.zeroEquip();
                        Thread.Sleep(500);
                        int zeroCount = 0;
                        while (true)
                        {
                            Msg(labelTS, panelTS, "调零中.." + zeroCount.ToString(), true);
                            showLed(carinfo.车辆号牌, "调零中.." + zeroCount.ToString());
                            if (equipdata.YhyXh == "mql_8201")
                            {
                                if (yhy.getRealTimeStatus(out yhystatus))
                                {
                                    if (yhystatus.气体调零状态位 == 0)
                                        break;
                                }
                            }
                            else if (equipdata.YhyXh == "fly_2000")
                            {
                                if (yhy.getFlRealTimeDataUnAdd(0, out yhyflstatusanddata))
                                {
                                    if (!yhyflstatusanddata.st01.调零)
                                        break;
                                }
                            }
                            else if (equipdata.YhyXh == nhxh)
                            {
                                if (yhy.getNhStatus(out yhynhstatus2))
                                {
                                    if (!yhynhstatus2.调零成功)
                                        break;
                                    if (yhynhstatus2.调零失败)
                                    {
                                        stopReadData = true;
                                        showLedSingleRow(1, "　　检测中止　　");
                                        Thread.Sleep(100);
                                        showLedSingleRow(2, "调零失败　　　　");
                                        Msg(labelTS, panelTS, "调零失败", true);
                                        flGetAddDataFlag = 0;//停止累计取数
                                        if (!yhy.stopTest())
                                        { yhy.stopTest(); }
                                        Thread.Sleep(900);
                                        showCurve = false;//暂停图表的显示                    
                                        igbt.Exit_Control();
                                        JC_Status = false;
                                        jcStatus = false;
                                        try
                                        {
                                            Th_get_FqandLl.Abort();
                                        }
                                        catch
                                        { }
                                        yhy.stopAction();
                                        yhy.stopFlow();
                                        GKSJ = 0;
                                        gksj_count = 0;
                                        //RefreshUI();
                                        Ref_Control_Text(button1, "开始检测");
                                        return;
                                    }
                                }
                            }
                            Thread.Sleep(900);
                            zeroCount++;
                        }
                        Msg(labelTS, panelTS, "调零完毕", true);
                        showLed(carinfo.车辆号牌, "调零完毕　　　　");
                        Thread.Sleep(1000);
                        if (equipdata.YhyXh == "mql_8201")
                        {
                            Msg(labelTS, panelTS, "测定环境空气", true);
                            showLed(carinfo.车辆号牌, "测定环境空气　　");
                            Thread.Sleep(1000);
                            yhy.testEnviroAir();
                            Thread.Sleep(500);
                            int testCount = 0;
                            while (true)
                            {
                                Msg(labelTS, panelTS, "测定中.." + testCount.ToString(), true);
                                showLed(carinfo.车辆号牌, "测定中.." + testCount.ToString());
                                if (yhy.getRealTimeStatus(out yhystatus))
                                {
                                    if (yhystatus.环境空气测定状态位 == 0)
                                        break;
                                }
                                Thread.Sleep(900);
                                testCount++;
                            }
                            Msg(labelTS, panelTS, "测定完毕", true);
                            showLed(carinfo.车辆号牌, "测定完毕　　　　");
                            Thread.Sleep(1000);
                        }
                        else if (equipdata.YhyXh == nhxh)
                        {
                            Msg(labelTS, panelTS, "测定环境空气", true);
                            showLed(carinfo.车辆号牌, "测定环境空气　　");
                            Thread.Sleep(1000);
                            yhy.testEnviroAir();
                            Thread.Sleep(500);
                            int testCount = 0;
                            while (true)
                            {
                                Msg(labelTS, panelTS, "测定中.." + testCount.ToString(), true);
                                showLed(carinfo.车辆号牌, "测定中.." + testCount.ToString());
                                if (yhy.getNhStatus(out yhynhstatus))
                                {
                                    if (!yhynhstatus.正在测量环境CO2)
                                        break;
                                }
                                Thread.Sleep(900);
                                testCount++;
                            }
                            Msg(labelTS, panelTS, "测定完毕", true);
                            showLed(carinfo.车辆号牌, "测定完毕　　　　");
                            Thread.Sleep(1000);
                        }
                        Msg(labelTS, panelTS, "修正CO2", true);
                        showLed(carinfo.车辆号牌, "修正CO2　　");
                        yhy.amendCO2();
                        Thread.Sleep(1000);
                        #endregion

                        #region 
                        if (carinfo.燃料种类 == 0)//汽油
                            yhy.selectQy();
                        else
                            yhy.selectCy();
                        Thread.Sleep(500);
                        #endregion
                        if (speednow > 0.5)
                        {
                            Msg(labelTS, panelTS, "等待滚筒静止", true);
                            showLed(carinfo.车辆号牌, "等待滚筒静止　　");
                            while (speednow > 0.5)
                            {
                                Thread.Sleep(1000);
                            }
                        }
                        igbt.Set_ClearKey();
                        Msg(labelTS, panelTS, "固定采集管", true);
                        showLed(carinfo.车辆号牌, "固定采集管　　　");
                        if (equipdata.isIgbtContainGdyk)
                        {
                            igbt.Set_ClearKey();
                            Thread.Sleep(1000);
                            isZsStable = false;
                            while (!isZsStable)
                            {
                                if (dynconfigdata.DynYkStyle == 0)
                                {
                                    if (((igbt.keyandgd) & 0xf0) != 0x00)
                                    {
                                        byte keyvalue = (byte)(igbt.keyandgd >> 4);
                                        if (keylist.Contains(keyvalue))
                                            isZsStable=true;
                                        igbt.Set_ClearKey();
                                    }
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
                                Thread.Sleep(200);
                            }
                            /*
                            while ((igbt.keyandgd & 0xf0) == 0x00)
                                Thread.Sleep(500);
                            igbt.Set_ClearKey();*/
                        }
                        else
                        {
                            Thread.Sleep(1000);
                            MessageBox.Show("确认采集管已固定好?", "系统提示");
                        }
                        Msg(labelTS, panelTS, "固定完成", true);
                        showLed(carinfo.车辆号牌, "固定完成　　　　");
                        Thread.Sleep(1000);
                        Msg(labelTS, panelTS, "启动风机", true);
                        showLed(carinfo.车辆号牌, "启动风机　　　　");
                        if (radioButtonSF.Checked)
                            yhy.selectLowFlow();
                        else if (radioButtonMF.Checked)
                            yhy.selectMidFlow();//选择中流量
                        else if (radioButtonLF.Checked)
                            yhy.selectHighFlow();//选择中流量
                        else if (radioButtonXLF.Checked)
                            yhy.selectExtraHighFlow();//选择中流量
                        Thread.Sleep(500);
                        if (!yhy.startFlow())//启动风机
                        {
                            Thread.Sleep(200);
                            if (!yhy.startFlow())
                            {
                                Thread.Sleep(200);
                                yhy.startFlow();
                            }
                        }
                        Thread.Sleep(1000);
                        Msg(labelTS, panelTS, "加速至" + v0.ToString("00") + "km/h", true);
                        showLed("加速至" + v0.ToString("00") + "km/h", " " + speednow.ToString("00.0") + " km/h");
                        while (Math.Abs(speednow - v0) > dynconfigdata.FuelSdQj)
                        {
                            showLedSingleRow(2, " " + speednow.ToString("00.0") + " km/h");
                            Thread.Sleep(200);
                        }
                        for (int i = 0; i < 15; i++)
                        {
                            showLedSingleRow(2, " " + speednow.ToString("00.0") + " km/h");
                            Thread.Sleep(200);
                        }
                        if (!yhy.pumpAir()) yhy.pumpAir();//开始抽气
                        Msg(labelTS, panelTS, "开始加载", true);
                        showLed("开始加载　　　　", " " + speednow.ToString("00.0") + " km/h");
                        //igbt.Set_Control_Force(0);


                        if (igbt != null)
                        {
                            igbt.Set_Control_Force(0);
                            Thread.Sleep(200);
                            igbt.Start_Control_Force();
                        }
                        int jzsj = (int)(Y_FTC / dynconfigdata.FuelforceTime);
                        if (jzsj <= 1)
                        {
                            if (igbt != null)
                            {
                                igbt.Set_Control_Force((float)(Y_FTC * carinfo.加载力比例));
                                showLed("加载: " + forcenow.ToString("0000") + "N", " " + speednow.ToString("00.0") + " km/h");
                                Thread.Sleep(900);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < jzsj; i++)//逐渐加载,以免对车速影响太大
                            {
                                if (igbt != null)
                                {
                                    igbt.Set_Control_Force((float)(Y_FTC * carinfo.加载力比例 * i * 1.0 / (double)(jzsj)));
                                }

                                Msg(labelTS, panelTS, "加载中:" + (jzsj - i), true);
                                showLed("加载: " + forcenow.ToString("0000") + "N", " " + speednow.ToString("00.0") + " km/h");
                                Thread.Sleep(900);
                            }
                        }
                        igbt.Set_Control_Force((float)(Y_FTC * carinfo.加载力比例));
                        Thread.Sleep(900);
                        Msg(labelTS, panelTS, "加载完成", true);

                        StartPosition:
                        showLedSingleRow(1, "保持: " + v0.ToString("00") + "km/h");
                        yhsmallthanxzcount = 0;
                        int stableTime = 15;
                        while (stableTime >= 0)
                        {
                            showLedSingleRow(2, "v:" + speednow.ToString("00.0") + " " + stableTime.ToString("00") + "s");
                            if (Math.Abs(speednow - v0) > dynconfigdata.FuelSdQj)
                            {
                                Msg(labelTS, panelTS, "保持" + v0.ToString("00") + "km/h/" + speednow.ToString("0.0"), true);
                                Thread.Sleep(900);
                                stableTime = 15;
                                continue;
                            }
                            else
                            {
                                Msg(labelTS, panelTS, "保持" + v0.ToString("00") + "km/h(" + stableTime.ToString() + "s)", true);
                            }
                            Thread.Sleep(900);
                            stableTime--;
                        }

                        YHSTART:
                        if (!yhy.stopTest())
                        { yhy.stopTest(); }
                        yhsmallthanxzcount = 0;
                        distance = 0;
                        Msg(labelTS, panelTS, "开始测量", true);
                        showLedSingleRow(2, "开始测量");
                        pregksj = 0;
                        gksj = 0;
                        starttime = DateTime.Now;
                        showCurve = true;
                        yhy.setNHDelayTime(nhdelaytime);
                        Thread.Sleep(500);
                        while (!yhy.startTest(60, (int)(carinfo.额定总质量)))
                        {
                            Thread.Sleep(900);
                        }
                        
                        Thread.Sleep(500);
                        if (equipdata.YhyXh == "mql_8201")
                        {
                            yhy.getRealTimeTotalData(out yhydata_temp);
                            while (yhydata_temp.CHECKTIME >= 60)//如果
                            {
                                Thread.Sleep(900);
                                yhy.getRealTimeTotalData(out yhydata_temp);
                            }
                        }
                        else if (equipdata.YhyXh == "fly_2000")
                        {
                            yhy.getFlRealTimeDataUnAdd(flGetAddDataFlag, out yhyflstatusanddata_temp);
                            while (yhyflstatusanddata_temp.累加测试时间 >= 60)//如果
                            {
                                Thread.Sleep(900);
                                yhy.getFlRealTimeDataUnAdd(flGetAddDataFlag, out yhyflstatusanddata_temp);
                            }
                        }
                        else if (equipdata.YhyXh == nhxh)
                        {
                            yhy.GetNHYhyDat(out nhdata_temp);
                            while (nhdata_temp.time >= 60)//如果
                            {
                                Thread.Sleep(900);
                                yhy.GetNHYhyDat(out nhdata_temp);
                            }
                        }
                        Thread.Sleep(500);
                        JC_Status = true;
                        stopReadData = false;
                        flGetAddDataFlag = 1;//开始累计取数
                        
                        ccsj = 0;
                        forceccsj = 0;
                        int testTime = 60;
                        showLedSingleRow(1, "保持" + v0.ToString("00") + "km/h" + testTime.ToString("00") + "s");
                        while (testTime > 0)
                        {
                            Msg(labelTS, panelTS, "保持" + v0.ToString("00") + "km/h(" + testTime + "s)", true);
                            if (Math.Abs(speednow - v0) > dynconfigdata.FuelSdQj)
                            {
                                ccsj += 0.3;
                                if (ccsj > 3)
                                {
                                    ccsj = 0;
                                    stopReadData = true;
                                    showLedSingleRow(2, "速度超差重新计时");
                                    Msg(labelTS, panelTS, "速度超差", true);
                                    Thread.Sleep(400);
                                    flGetAddDataFlag = 0;//停止累计取数
                                                         //JC_Status = false;
                                    if (!yhy.stopTest())
                                    { yhy.stopTest(); }
                                    Thread.Sleep(900);
                                    for (int i = 5; i >= 0; i--)
                                    {
                                        Msg(labelTS, panelTS, "即将重新开始..." + i.ToString(), true);
                                        showLedSingleRow(2, "将重新开始测量　");
                                        Thread.Sleep(900);
                                    }
                                    if (dynconfigdata.DynReStartStyle == 0)
                                        goto YHSTART;
                                    else
                                        goto StartPosition;
                                    /*starttime = DateTime.Now;
                                    testTime = 60;
                                    while (!yhy.startTest(60))
                                    {
                                        Thread.Sleep(900);
                                    }
                                    stopReadData = false;
                                    distance = 0;
                                    while (yhydata.CHECKTIME == 60)
                                    {
                                        Thread.Sleep(900);
                                    }
                                    // JC_Status = true;
                                    continue;*/
                                }
                            }
                            else
                            {
                                ccsj = 0;
                            }

                            if (Math.Abs(forcenow - Y_FTC) > dynconfigdata.FuelForceQj)
                            {
                                forceccsj += 0.3;
                                if (forceccsj > 3)
                                {
                                    //Y_FTC = 0;
                                    forceccsj = 0;
                                    stopReadData = true;
                                    showLedSingleRow(2, "加载超差重新计时");
                                    Msg(labelTS, panelTS, "加载阻力超差", true);
                                    flGetAddDataFlag = 0;//停止累计取数
                                                         //JC_Status = false;
                                                         //Thread.Sleep(500);
                                    if (!yhy.stopTest())
                                    { yhy.stopTest(); }
                                    Thread.Sleep(900);
                                    for (int i = 5; i >= 0; i--)
                                    {
                                        Msg(labelTS, panelTS, "即将重新开始..." + i.ToString(), true);
                                        showLedSingleRow(2, "将重新开始测量　");
                                        Thread.Sleep(900);
                                    }
                                    if (dynconfigdata.DynReStartStyle == 0)
                                        goto YHSTART;
                                    else
                                        goto StartPosition;
                                    /*Msg(labelTS, panelTS, "重新计时", true);
                                    testTime = 60;
                                    while (!yhy.startTest(60))
                                    {
                                        Thread.Sleep(900);
                                    }
                                    distance = 0;
                                    stopReadData = false;
                                    while (yhydata.CHECKTIME == 60)
                                    {
                                        Thread.Sleep(900);
                                    }
                                    //JC_Status = true;
                                    continue;*/
                                }
                            }
                            else
                            {
                                forceccsj = 0;
                            }
                            if (dynconfigdata.DynFuelJk && testTime < 55)
                            {

                                if (equipdata.YhyXh == "mql_8201")
                                {
                                    if (yhydata.SSYH < dynconfigdata.DynFuelXz)
                                        yhsmallthanxzcount++;
                                }
                                else if (equipdata.YhyXh == "fly_2000")
                                {
                                    if (yhyflstatusanddata.yh_ssyh < dynconfigdata.DynFuelXz)
                                        yhsmallthanxzcount++;
                                }
                                else if (equipdata.YhyXh == nhxh)
                                {
                                    if (yhynhdata.ssyh < dynconfigdata.DynFuelXz)
                                        yhsmallthanxzcount++;
                                }
                                if (yhsmallthanxzcount >= 3)
                                {
                                    stopReadData = true;
                                    showLedSingleRow(1, "　　检测中止　　");
                                    Thread.Sleep(100);
                                    showLedSingleRow(2, "实时油耗值过低　");
                                    Msg(labelTS, panelTS, "实时油耗过低", true);
                                    flGetAddDataFlag = 0;//停止累计取数
                                    if (!yhy.stopTest())
                                    { yhy.stopTest(); }
                                    Thread.Sleep(900);
                                    showCurve = false;//暂停图表的显示                    
                                    igbt.Exit_Control();
                                    JC_Status = false;
                                    jcStatus = false;
                                    try
                                    {
                                        Th_get_FqandLl.Abort();
                                    }
                                    catch
                                    { }
                                    yhy.stopAction();
                                    yhy.stopFlow();
                                    GKSJ = 0;
                                    gksj_count = 0;
                                    //RefreshUI();
                                    Ref_Control_Text(button1, "开始检测");
                                    return;
                                }
                            }
                            if (equipdata.YhyXh == "mql_8201")
                            {
                                testTime = 60 - yhydata.CHECKTIME;
                            }
                            else if (equipdata.YhyXh == "fly_2000")
                            {
                                testTime = 60 - (int)yhyflstatusanddata.累加测试时间;
                            }
                            else if (equipdata.YhyXh == nhxh)
                            {
                                testTime = 60 + nhdelaytime - (int)yhynhdata.time;
                            }
                            showLed("保持" + v0.ToString("00") + "  " + testTime.ToString("00") + "s", "v:" + speednow.ToString("00.0") + " " + ccsj.ToString("0.0") + "s");
                            //Thread.Sleep(100);
                            //showLedSingleRow(2, );
                            Thread.Sleep(300);
                        }
                        if (equipdata.YhyXh == "mql_8201")
                        {
                            totalYh = yhydata.ZYH;
                        }
                        else if (equipdata.YhyXh == "fly_2000")
                        {
                            totalYh = yhyflstatusanddata.yh_ljyh;
                        }
                        else if (equipdata.YhyXh == nhxh)
                        {
                            totalYh = yhynhdata.ljyh;
                        }
                        totalDistance = distance;
                        showCurve = false;
                        stopReadData = true;//停止取数
                        Msg(labelTS, panelTS, "测量完毕", true);
                        showLedSingleRow(2, "测量完毕　　　　");
                        flGetAddDataFlag = 0;
                        JC_Status = false;
                        Thread.Sleep(1000);
                        Msg(labelXSJL, panelXSJL, totalDistance.ToString("0"), false);
                        yhPerHundred = Math.Round(totalYh * 100 / totalDistance, 1);
                        //yhXz = 0;
                        testTimes++;
                        if (yhPerHundred > yhXz)
                            yhpdjg = "不合格";
                        else
                        {
                            yhpdjg = "合格";
                        }
                        showLedSingleRow(1, "百公里油耗：  ");
                        showLedSingleRow(2, "   " + yhPerHundred.ToString("0.0") + "L");
                        Thread.Sleep(2000);
                        showTextboxText(textBoxF_BGLYH, yhPerHundred.ToString("0.0"));
                        showTextboxText(textBoxF_PD, yhpdjg);
                        if (yhpdjg == "不合格" && testTimes < dynconfigdata.FuelTestFjjs + 1)
                        {
                            Msg(labelTS, panelTS, "检测不合格", true);
                            showLedSingleRow(2, "检测不合格　　　");
                            Thread.Sleep(2000);
                            for (int i = 5; i >= 0; i--)
                            {
                                Msg(labelTS, panelTS, "即将开始第" + testTimes.ToString() + "次复检..." + i.ToString(), true);
                                showLedSingleRow(2, "将开始第" + testTimes.ToString() + "次复检");

                                Thread.Sleep(900);
                            }
                            if (dynconfigdata.DynReStartStyle == 0)
                                goto YHSTART;
                            else
                                goto StartPosition;

                        }
                        else
                        {

                            Msg(labelTS, panelTS, "检测" + yhpdjg + "，请减速", true);
                            if (yhpdjg == "合格")
                            {
                                showLedSingleRow(1, "　合格　　请减速");
                                Ref_Control_Checked(checkBoxFUEL, false);
                            }
                            else
                                showLedSingleRow(1, "　不合格　请减速");
                            Thread.Sleep(2000);
                        }
                        double result = yhPerHundred;
                        Thread.Sleep(1000);


                        Msg(labelTS, panelTS, "检测完毕，请拔掉取样管", true);
                        showLedSingleRow(2, "请拔掉取样管");

                        igbt.Exit_Control();
                        Thread.Sleep(2000);
                        for (int i = 30; i >= 0; i--)
                        {
                            Msg(labelTS, panelTS, "反吹中.." + i.ToString() + "s", true);
                            showLedSingleRow(2, "反吹中..." + i.ToString("00") + "s");
                            Thread.Sleep(900);
                        }
                        if (!yhy.stopAction())
                        { yhy.stopAction(); }
                        if (!yhy.stopFlow())
                        { yhy.stopFlow(); }
                        Thread.Sleep(1000);
                        DataTable dyn_datatable = new DataTable();
                        DataRow dr = null;
                        string mmsd="", mmnl = "", mmyh = "";
                        try
                        {
                            dyn_datatable.Columns.Add("速度");
                            dyn_datatable.Columns.Add("扭力");
                            dyn_datatable.Columns.Add("油耗");
                            for (int i = 0; i < speedlist.Count; i++)//将数据写入逐秒数据
                            {
                                dr = dyn_datatable.NewRow();
                                dr["速度"] = speedlist[i].ToString("0.0");
                                dr["扭力"] = forcelist[i].ToString("0");
                                dr["油耗"] = powerlist[i].ToString("0.0");
                                mmsd += speedlist[i].ToString("0.0") + ",";
                                mmnl += forcelist[i].ToString("0.0") + ",";
                                mmyh += powerlist[i].ToString("0.0") + ",";
                                dyn_datatable.Rows.Add(dr);
                            }
                        }
                        catch (Exception er)
                        {
                            MessageBox.Show("生成过程数据时出错：" + er.Message);
                            return;
                        }

                        Msg(labelTS, panelTS, "结果:" + yhPerHundred.ToString("0.0") + " 限值:" + yhXz.ToString("0.0") + " " + yhpdjg, true);
                        Thread.Sleep(3000);
                        csvwriter.SaveCSV(dyn_datatable, "D:/dataseconds/" + carinfo.车辆号牌 + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "Fuel.csv");
                        writeFuelResult(mmsd,mmnl,mmyh);
                        Thread.Sleep(1000);
                        #endregion
                    }
                    #region 结束 提示
                    Msg(labelTS, panelTS, "检测完成,请减速停车", true);
                    showLed("检测完成　　　　", "请减速停车　　　");
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
            showTextboxText(textBoxD_η, yhPerHundred.ToString("0.0"));
            showTextboxText(textBoxD_Pe, yhXz.ToString("0.0"));
            showTextboxText(textBoxD_nm, xnpdjgstring);
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
        Thread Th_get_Yhy = null;
        int GKSJ = 0;
        double ccsj = 0;
        double forceccsj = 0;

        private void checkBox牵引车满载总质量_CheckedChanged(object sender, EventArgs e)
        {
            caculateFTC();
            showYhCarInf();
        }

        private DataTable readFromMdb(string sqlstring, out string expmsg)
        {
            DataTable dt = new DataTable();
            expmsg = "";
            try
            {
                string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "/油耗车型库.mdb";
                OleDbConnection connection = new OleDbConnection(connectionString);
                connection.Open();
                OleDbCommand odCommand = connection.CreateCommand();
                OleDbTransaction Trans = connection.BeginTransaction();
                odCommand.Transaction = Trans;
                Trans.Rollback();
                try
                {
                    odCommand.CommandText = sqlstring;
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlstring, connection);
                    dataAdapter.Fill(dt);
                    dataAdapter.Dispose();
                }
                catch (Exception er)
                {
                    expmsg = er.Message;
                    Trans.Rollback();
                }
                connection.Close();
                return dt;
            }
            catch (Exception er)
            {
                expmsg = er.Message;
                return null;
            }
        }
        private bool getYhXz(out double xz, out int xzyj)
        {
            DataTable dt;
            xz = 0; xzyj = 0;
            string msg;
            string sqlstring = "select * from [车型] where CLXH='" + carinfo.车辆型号 + "' and FDJXH='" + carinfo.发动机型号 + "'";
            dt = readFromMdb(sqlstring, out msg);
            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    if (carinfo.汽车类型 == 0 && carinfo.客车等级 == 0)
                    {
                        xz = Math.Round(1.14 * double.Parse(dt.Rows[0]["YH60MZ"].ToString()), 2);
                        xzyj = 0;
                        return true;
                    }
                    else
                    {
                        xz = Math.Round(1.14 * double.Parse(dt.Rows[0]["YH50MZ"].ToString()), 2);
                        xzyj = 0;
                        return true;
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show("按车型和发动机型号找到匹配项，但数据格式出现异常：" + er.Message);

                }
            }
            sqlstring = "select * from [车型] where CLXH='" + carinfo.车辆型号 + "'";
            dt = readFromMdb(sqlstring, out msg);
            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    if (carinfo.汽车类型 == 0 && carinfo.客车等级 == 0)
                    {
                        xz = Math.Round(1.14 * double.Parse(dt.Rows[0]["YH60MZ"].ToString()), 2);
                        xzyj = 0;
                        return true;
                    }
                    else
                    {
                        xz = Math.Round(1.14 * double.Parse(dt.Rows[0]["YH50MZ"].ToString()), 2);
                        xzyj = 0;
                        return true;
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show("按车型找到匹配项，但数据格式出现异常：" + er.Message);
                }
            }
            double k = 1.0;
            if (carinfo.燃料种类 == 0) k = 1.15;
            if (carinfo.汽车类型 == 0)
            {
                sqlstring = "select * from [在用柴油客车参比值] where int(MINLENGTH)<" + carinfo.客车车长 + " and int(MAXLENGTH)>=" + carinfo.客车车长;
                dt = readFromMdb(sqlstring, out msg);
                if (dt != null && dt.Rows.Count > 0)
                {
                    try
                    {
                        if (carinfo.客车等级 == 0)
                        {
                            xz = Math.Round(k * double.Parse(dt.Rows[0]["HIGH"].ToString()), 2);
                            xzyj = 1;
                            return true;
                        }
                        else
                        {
                            xz = Math.Round(k * double.Parse(dt.Rows[0]["MIDANDLOW"].ToString()), 2);
                            xzyj = 1;
                            return true;
                        }
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show("在【在用柴油客车参比值】中找到匹配项，但数据格式出现异常：" + er.Message);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("在【在用柴油客车参比值】中未找到匹配项，客车长度：" + carinfo.客车车长.ToString());
                    return false;
                }
            }
            else
            {
                if (checkBox牵引车满载总质量.Checked)
                {
                    sqlstring = "select * from [在用柴油半挂车参比值] where int(MINZZL)<" + carinfo.牵引车满载总质量 + " and int(MAXZZL)>=" + carinfo.牵引车满载总质量;
                    dt = readFromMdb(sqlstring, out msg);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        try
                        {
                            xz = Math.Round(k * double.Parse(dt.Rows[0]["XZ"].ToString()), 2);
                            xzyj = 2;
                            return true;
                        }

                        catch (Exception er)
                        {
                            MessageBox.Show("在【在用柴油半挂车参比值】中找到匹配项，但数据格式出现异常：" + er.Message);
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("在【在用柴油半挂车参比值】中未找到匹配项，牵引车满载总质量：" + carinfo.牵引车满载总质量.ToString());
                        return false;
                    }
                }
                else
                {
                    sqlstring = "select * from [在用柴油货车参比值] where int(MINZZL)<" + carinfo.额定总质量 + " and int(MAXZZL)>=" + carinfo.额定总质量;
                    dt = readFromMdb(sqlstring, out msg);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        try
                        {
                            xz = Math.Round(k * double.Parse(dt.Rows[0]["XZ"].ToString()), 2);
                            xzyj = 1;
                            return true;
                        }
                        catch (Exception er)
                        {
                            MessageBox.Show("在【在用柴油货车参比值】中找到匹配项，但数据格式出现异常：" + er.Message);
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("在【在用柴油货车参比值】中未找到匹配项，额定总质量：" + carinfo.额定总质量.ToString());
                        return false;
                    }
                }
            }
        }
        private void showYhCarInf()
        {
            if (carinfo.油耗限值依据 == 0)
            {
                if (getYhXz(out yhXz, out xzyj))
                {
                    string xzyjstirng = "";
                    switch (xzyj)
                    {
                        case 0: xzyjstirng = "车型库匹配"; break;
                        case 1: xzyjstirng = "表C.1、C.2匹配"; break;
                        case 2: xzyjstirng = "表C.3匹配"; break;
                        default: break;
                    }
                    textBoxF_YHXZ.Text = yhXz.ToString("0.00") + "(" + xzyjstirng + ")";
                }
                else
                {
                    textBoxF_YHXZ.Text = "初始化限值失败";
                }
            }
            else
            {
                xzyj = 3;
                yhXz = carinfo.油耗限值;
                string xzyjstirng = "平台下发";
                textBoxF_YHXZ.Text = yhXz.ToString("0.00") + "(" + xzyjstirng + ")";
            }
            textBoxF_V0.Text = v0.ToString("0.0");
            textBoxF_FTC.Text = Y_FTC.ToString("0");
            textBoxF_Ff.Text = Y_Ft.ToString();
            textBoxF_FW.Text = Y_Fw.ToString();
            textBoxF_f.Text = f.ToString("0.000");
            textBoxF_A.Text = A.ToString("0");
            textBoxF_CD.Text = Cd.ToString("0.000");
            textBoxF_FC.Text= Y_Fc.ToString("0");
            textBoxF_Ffc.Text = Y_Ffc.ToString("0");
            textBoxF_fc1.Text = fc.ToString("0.000");
            textBoxF_Ftc1.Text = Y_Ftc.ToString("0");
        }
        public static List<float> speedlist = new List<float>();
        public static List<float> forcelist = new List<float>();
        public static List<float> powerlist = new List<float>();
        private DateTime starttime = DateTime.Now;
        private int gksj = 0;
        private int pregksj = 0;
        public static float speednow=0, forcenow=0, powernow=0;
        private bool showCurve = false;
        private int workState = 0;

        bool JC_Status = false;
        private int enginezs = 0;

        double speedTestResult = 0;
        int speedTestPdjg = 0;

        public static bool isReloadData = false;
        public static float curvescalevalue = 0;

        private void dynamicTest_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F5)
            {
                if(carinfo.驱动轴质量方式==1)
                {
                    qdzzz_scz = 1100;
                    writeQdzzlResult();
                }
                if(carinfo.是否检测车速)
                {
                    speedTestResult = 37.0;
                    speedTestPdjg = 0;
                    writeSpeedTestResult();
                }
                if (carinfo.是否检测动力性)
                {
                    if(carinfo.燃料种类==0)
                    {
                        speedTestResult = 0;
                        speedTestPdjg = 0;
                        η = 0.750;
                        Mm = 200;
                        Vm = 58.4;
                        nm = 3000;
                        Vw = 71.1;
                        X_FM = 2120;
                        X_Fm = 3225;
                        X_Ftc = 110;
                        X_Fc = 177;
                        X_Ff = 290;
                        X_Ft = 528;
                        αa = 1.126;
                        fc = 0.009;
                        Wd = 32.3;
                        Sd = 29.7;
                        Dqy = 95.6;
                        xnpdjgstring = "合格";
                        writeGasolineResult("25.3,26.3,27.3,", "200,300,400,", "55.3,56.3,57.3,");
                    }
                    else
                    {
                        speedTestResult = 0;
                        speedTestPdjg = 0;
                        η = 0.750;
                        Pe = 200;
                        Ve = 58.4;
                        Vw = 71.1;
                        X_Fe = 4977;
                        X_FE = 3184;
                        X_Ftc = 160;
                        X_Fc = 177;
                        X_Ff = 684;
                        X_Ft = 773;
                        αd = 1.031;
                        fc = 0.009;
                        Wd = 32.2;
                        Sd = 30.6;
                        Dqy = 95.7;
                        xnpdjgstring = "合格";
                        writeDieselResult("25.3,26.3,27.3,", "200,300,400,", "55.3,56.3,57.3,");
                    }
                }
                if(carinfo.是否检测油耗)
                {
                    v0 = 50;
                    Y_FTC = 395;
                    totalYh = 86;
                    totalDistance =833.0;
                    yhPerHundred = 10.3;
                    yhXz = 12.6;
                    xzyj = 0;
                    yhpdjg = "合格";
                    Y_Ft = 346;
                    Y_Fw = 237;
                    f = 0.006;
                    A = 2;
                    Cd = 0.900;
                    Y_Fc = 188;
                    Y_Ffc = 88;
                    fc = 0.009;
                    Y_Ftc = 100;
                    writeFuelResult("50.3,50.3,50.3,", "200,300,400,", "7.3,8.3,9.3,");
                }
            }
            if (e.KeyData == Keys.F6)
                button6.Visible = !button6.Visible;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (button6.Text == "WCF GO")
            {
                button6.Text = "WCF STOP";
                try
                {
                    if (TH_ST != null)
                        TH_ST.Abort();
                }
                catch
                { }
                TH_ST = new Thread(QdzWeight_Exe); //检测过程
                TH_ST.Start();
            }
            else
            {
                button6.Text = "WCF GO";
                try
                {
                    if (TH_ST != null)
                        TH_ST.Abort();
                }
                catch
                { }
            }
        }

        public static bool displayCurveScale = false;
        private int showCount = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            if(igbt!=null)
            {
                speednow = igbt.Speed;
                forcenow = (float)(igbt.Force/carinfo.加载力比例);
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
                if(workState==3)
                {
                    if (showform != null) showform.DriverShow_OnMyChange(5, distance.ToString("0.0"));
                    if (showform != null)
                    {
                        if (equipdata.YhyXh == "mql_8201")
                        {
                            showform.DriverShow_OnMyChange(6, yhydata.ZYH.ToString("0.0"));
                        }
                        else if (equipdata.YhyXh == "fly_2000")
                        {
                            showform.DriverShow_OnMyChange(6, yhyflstatusanddata.yh_ljyh.ToString("0.0"));
                        }
                        else if (equipdata.YhyXh == nhxh)
                        {
                            showform.DriverShow_OnMyChange(6, yhynhdata.ljyh.ToString("0.0"));
                        }
                    }
                }
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
                    if (workState == 2)
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
                    else if (workState == 3)
                    {
                        if (gksj < 900)
                        {
                            gksj = (int)(timesp.TotalMilliseconds / 100);
                            int timeafterpre = gksj - pregksj;//计算距离上次计算过去了几个时间间隔，每个时间间隔为100ms
                            pregksj = gksj;
                            if (jcStatus)
                                distance += speednow * timeafterpre * 1.0 / 3.6 * 0.1;

                            if (gksj % 10 == 0)
                            {
                                isReloadData = true;

                                speedlist.Add(speednow);
                                forcelist.Add(forcenow);
                                if (equipdata.YhyXh == "mql_8201")
                                {
                                    powerlist.Add((float)(yhydata.ZYH));
                                }
                                else if (equipdata.YhyXh == "fly_2000")
                                {
                                    powerlist.Add((float)(yhyflstatusanddata.yh_ljyh));
                                }
                                else if (equipdata.YhyXh == nhxh)
                                {
                                    powerlist.Add((float)(yhynhdata.ljyh));
                                }                                
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
                                curve3.Values = powerlist.ToArray();
                                curve3.Fit();
                                pictureBox油耗.Image = curve3.CreateImage();
                            }
                        }
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
            try
            {
                if (Th_get_Yhy != null) Th_get_Yhy.Abort();
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

        private byte flGetAddDataFlag = 0;//福立取数指令的累计标志，0：不累计 1：累计
        private int nhdelaytime = 0;
        yhrRealTimeStatus yhystatus = new yhrRealTimeStatus();
        fl_StatusAndData yhyflstatusanddata = new fl_StatusAndData();
        fl_StatusAndData yhyflstatusanddata_temp = new fl_StatusAndData();
        NH_status yhynhstatus = new NH_status();
        NH_status2 yhynhstatus2 = new NH_status2();
        NH_status yhynhstatus_temp = new NH_status();
        yhrRealTimeTotalData yhydata_temp = new yhrRealTimeTotalData();
        yhrRealTimeData yhydata2_temp = new yhrRealTimeData();
        yhrRealTimePfwsszl yhydata3_temp = new yhrRealTimePfwsszl();
        NH_fuleData nhdata_temp = new NH_fuleData();
        NH_standardData nhstandarddata_temp = new NH_standardData();
        NH_fuleData yhynhdata = new NH_fuleData();
        NH_standardData yhynhstandarddata = new NH_standardData();
        private bool stopReadData = false;
        public void YHY_Detect()
        {
            while (true)
            {
                try
                {
                    if (!stopReadData)
                    {
                        if (equipdata.YhyXh == "mql_8201")
                        {
                            if (yhy.getRealTimeTotalData(out yhydata_temp))
                            {
                                yhydata = yhydata_temp;
                                Msg(labelLL, panelLL, yhydata.BZLL.ToString(), false);
                                Msg(labelFBZLL, panelFBZLL, yhydata.FBZLL.ToString(), false);
                                Msg(labelXSJL, panelXSJL, distance.ToString("0"), false);
                                Msg(labelTIME, panelTIME, yhydata.CHECKTIME.ToString(), false);
                                Msg(labelSSYH, panelSSYH, yhydata.SSYH.ToString(), false);
                                Msg(labelLJYH, panelLJYH, yhydata.ZYH.ToString(), false);
                            }
                            else//如果没有读取油耗实时值，直接Continue继续读取
                            { continue; }
                            if (yhy.getRealTimeData(out yhydata2_temp))
                            {
                                yhydata2 = yhydata2_temp;
                                Msg(labelCO, panelCO, yhydata2.CO.ToString(), false);
                                Msg(labelCO2, panelCO2, yhydata2.CO2.ToString(), false);
                                Msg(labelHC, panelHC, yhydata2.HC.ToString(), false);
                                Msg(labelNO, panelNO, yhydata2.NO.ToString(), false);
                                Msg(labelTEMP, panelTEMP, yhydata2.HJWD.ToString(), false);
                                Msg(labelHUMIDITY, panelHUNIDITY, yhydata2.HJSD.ToString(), false);
                                Msg(labelAT, panelAT, yhydata2.HJYL.ToString(), false);
                                Msg(labelXSWD, panelXSWD, yhydata2.XSWD.ToString(), false);
                                Msg(labelXSYL, panelXSYL, yhydata2.XSYL.ToString(), false);
                            }
                            if (yhy.getRealTimePfwsszl(out yhydata3_temp))
                            {
                                yhydata3 = yhydata3_temp;
                                Msg(labelCOZL, panelCOZL, yhydata3.SSCO.ToString(), false);
                                Msg(labelCO2ZL, panelCO2ZL, yhydata3.SSCO2.ToString(), false);
                                Msg(labelHCZL, panelHCZL, yhydata3.SSHC.ToString(), false);
                            }
                        }
                        else if (equipdata.YhyXh == "fly_2000")
                        {
                            if (yhy.getFlRealTimeDataUnAdd(flGetAddDataFlag, out yhyflstatusanddata_temp))
                            {
                                yhyflstatusanddata = yhyflstatusanddata_temp;
                                Msg(labelLL, panelLL, yhyflstatusanddata.bzll.ToString(), false);
                                Msg(labelFBZLL, panelFBZLL, yhyflstatusanddata.fbzll.ToString(), false);
                                Msg(labelXSJL, panelXSJL, distance.ToString("0"), false);
                                Msg(labelTIME, panelTIME, yhyflstatusanddata.累加测试时间.ToString(), false);
                                Msg(labelSSYH, panelSSYH, yhyflstatusanddata.yh_ssyh.ToString(), false);
                                Msg(labelLJYH, panelLJYH, yhyflstatusanddata.yh_ljyh.ToString(), false);
                                Msg(labelCO, panelCO, yhyflstatusanddata.co_ssnd.ToString(), false);
                                Msg(labelCO2, panelCO2, yhyflstatusanddata.co2_ssnd.ToString(), false);
                                Msg(labelHC, panelHC, yhyflstatusanddata.hc_ssnd.ToString(), false);
                                Msg(labelNO, panelNO, yhyflstatusanddata.no_ssnd.ToString(), false);
                                Msg(labelTEMP, panelTEMP, yhyflstatusanddata.hjwd.ToString(), false);
                                Msg(labelHUMIDITY, panelHUNIDITY, yhyflstatusanddata.xdsd.ToString(), false);
                                Msg(labelAT, panelAT, yhyflstatusanddata.xsyl.ToString(), false);
                                Msg(labelXSWD, panelXSWD, yhyflstatusanddata.xswd.ToString(), false);
                                Msg(labelXSYL, panelXSYL, yhyflstatusanddata.xsyl.ToString(), false);
                                Msg(labelCOZL, panelCOZL, yhyflstatusanddata.co2_sszl.ToString(), false);
                                Msg(labelCO2ZL, panelCO2ZL, yhyflstatusanddata.co2_sszl.ToString(), false);
                                Msg(labelHCZL, panelHCZL, yhyflstatusanddata.hc_sszl.ToString(), false);

                            }
                            else
                            { continue; }
                        }
                        else if (equipdata.YhyXh == nhxh)
                        {
                            /*if (yhy.GetNHstandardDat(out nhstandarddata_temp))
                            {
                                yhynhstandarddata = nhstandarddata_temp;
                                Msg(labelCO, panelCO, yhynhstandarddata.co_ssnd.ToString(), false);
                                Msg(labelCO2, panelCO2, yhynhstandarddata.co2_ssnd.ToString(), false);
                                Msg(labelHC, panelHC, yhynhstandarddata.hc_ssnd.ToString(), false);
                                Msg(labelNO, panelNO,"--", false);
                                Msg(labelTEMP, panelTEMP, yhynhstandarddata.wd.ToString(), false);
                                Msg(labelHUMIDITY, panelHUNIDITY, "--", false);
                                Msg(labelAT, panelAT, yhynhstandarddata.dqy.ToString(), false);
                                Msg(labelLL, panelLL, yhynhstandarddata.bzll.ToString(), false);
                                Msg(labelFBZLL, panelFBZLL, yhynhstandarddata.fbzll.ToString(), false);
                            }
                            else//如果没有读取油耗实时值，直接Continue继续读取
                            { continue; }
                            Thread.Sleep(200);*/
                            if (yhy.GetNHYhyDat(out nhdata_temp))
                            {
                                yhynhdata = nhdata_temp;
                                Msg(labelXSJL, panelXSJL, distance.ToString("0"), false);
                                Msg(labelTIME, panelTIME, yhynhdata.time.ToString(), false);
                                Msg(labelSSYH, panelSSYH, yhynhdata.ssyh.ToString(), false);
                                Msg(labelLJYH, panelLJYH, yhynhdata.ljyh.ToString(), false);
                                Msg(labelXSWD, panelXSWD, yhynhdata.wd.ToString(), false);
                                Msg(labelXSYL, panelXSYL, yhynhdata.dqy.ToString(), false);
                                Msg(labelCOZL, panelCOZL, yhynhdata.cozl.ToString(), false);
                                Msg(labelCO2ZL, panelCO2ZL, yhynhdata.co2zl.ToString(), false);
                                Msg(labelHCZL, panelHCZL, yhynhdata.hczl.ToString(), false);
                            }
                        }
                    }
                    Thread.Sleep(400);
                }
                catch
                { }
            }
        }
        public void Fq_Detect()
        {
            while (true)
            {
                if (showCurve)
                {
                    if (dynconfigdata.Zsj == "烟度计")
                    {
                        if (flb_100 != null)
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
                    }
                    else if (dynconfigdata.Zsj == "废气仪")
                    {
                        if (fla_502 != null)
                        {
                            enginezs = (int)fla_502.GetData().ZS;
                        }
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
                    if (carinfo.是否检测油耗)
                    {
                        Exhaust.yhrRealTimeStatus status = new yhrRealTimeStatus();
                        if (yhy != null)
                        {
                            if (equipdata.YhyXh == "mql_8201")
                            {
                                int i = 0;
                                for (i = 0; i < 10; i++)
                                {
                                    if (yhy.getRealTimeStatus(out status))
                                        break;
                                }
                                if (i == 10)
                                {
                                    MessageBox.Show("获取油耗仪通讯失败,不能开始检测", "系统错误");
                                    return;
                                }
                                if (status.预热状态位 == 0x01)
                                {
                                    MessageBox.Show("油耗仪正在预热中,不能开始检测", "系统错误");
                                    return;
                                }
                            }
                            else if (equipdata.YhyXh == "fly_2000")
                            {
                                int i = 0;
                                for (i = 0; i < 10; i++)
                                {
                                    if (yhy.getFlRealTimeDataUnAdd(0, out yhyflstatusanddata))
                                        break;
                                }
                                if (i == 10)
                                {
                                    MessageBox.Show("获取油耗仪通讯失败,不能开始检测", "系统错误");
                                    return;
                                }
                                if (yhyflstatusanddata.st01.预热)
                                {
                                    MessageBox.Show("油耗仪正在预热中,不能开始检测", "系统错误");
                                    return;
                                }
                            }
                            else if (equipdata.YhyXh == nhxh)
                            {
                                int i = 0;
                                for (i = 0; i < 10; i++)
                                {
                                    if (yhy.getNhStatus(out yhynhstatus))
                                    {
                                        if (yhynhstatus.预热)
                                        {
                                            MessageBox.Show("油耗仪正在预热中,不能开始检测", "系统错误");
                                            return;
                                        }
                                        else
                                            break;
                                    }
                                }
                                if (i == 10)
                                {
                                    MessageBox.Show("获取油耗仪通讯失败,不能开始检测", "系统错误");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("油耗仪未打开,不能开始检测", "系统错误");
                            return;
                        }
                    }
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
                    workState = 0;
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
                    curve3.displayScale1 = false;
                    curve3.displayScale2 = false;
                    //开妈检测过程
                    TH_ST = new Thread(Jc_Exe); //检测过程
                    TH_ST.Start();
                    stopReadData = true;
                    flGetAddDataFlag = 0;
                    Th_get_FqandLl = new Thread(Fq_Detect);
                    Th_get_FqandLl.Start();
                    if (carinfo.是否检测油耗)
                    {
                        Th_get_Yhy = new Thread(YHY_Detect);
                        Th_get_Yhy.Start();

                    }
                    GKSJ = 0;
                    button1.Text = "停止检测";
                    jcStatus = true;
                    //button2.Enabled = false;
                }
                else
                {
                    showCurve = false;//暂停图表的显示 

                    igbt.Exit_Control();
                    JC_Status = false;
                    jcStatus = false;
                    stopReadData = true;
                    flGetAddDataFlag = 0;

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

                    if (carinfo.是否检测油耗)
                    {
                        try
                        {
                            Th_get_Yhy.Abort();
                        }
                        catch
                        { }
                    }
                    if (carinfo.是否检测油耗)
                    {
                        yhy.stopAction();
                        yhy.stopFlow();
                    }
                    GKSJ = 0;
                    gksj_count = 0;
                    workState = 0;
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

        /// <summary>
        /// 刷新控件的文字信息
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <param name="text">文字信息</param>
        public void Ref_Control_Text(Control control, string text)
        {
            BeginInvoke(new wtcs(ref_Control_Text), control, text);
        }

        public void ref_Control_Text(Control control, string text)
        {
            control.Text = text;
        }
        public void Ref_Control_Checked(Control control, bool ischecked)
        {
            BeginInvoke(new wtcc(ref_Control_Checkd), control, ischecked);
        }

        public void ref_Control_Checkd(Control control, bool ischecked)
        {
           ((CheckBox)control).Checked = ischecked;
        }


    }
}
