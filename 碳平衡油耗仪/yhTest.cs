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
using System.Data.OleDb;


namespace 碳平衡油耗仪
{
    public partial class yhTest : Form
    {
        private const string flxh = "fly_2000";
        private const string mqxh = "mql_8201";
        private const string nhxh = "nhty_1";

        CSVcontrol.CSVwriter csvwriter = new CSVcontrol.CSVwriter();
        CSVcontrol.csvReader csvreader = new CSVcontrol.csvReader();
        private yhControl yhy = null;
        private IGBT igbt=null;
        LedControl.BX5k1 ledcontrol = null;
        private yhcarInidata carinfo = new yhcarInidata();
        private carIni carini = new carIni();
        private equipmentConfigInfdata equipdata = new equipmentConfigInfdata();
        private DynConfigInfdata dynconfigdata = new DynConfigInfdata();
        private configIni configini = new configIni();
        public delegate void wtlsb(Label Msgowner, string Msgstr, bool Update_DB);                //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                              //委托
        public delegate void wtlm(Label Msgowner, string Msgstr);
        public delegate void wtTextboxText(TextBox Msgowner, string Msgstr);
        public delegate void wtcs(Control controlname, string text);

        private Point panelDetailsInf = new Point(0, 0);
        public yhTest()
        {
            InitializeComponent();
        }
        #region 汽车燃料消耗检测工况相关参数计算
        //汽车燃料消耗检测工况下的道路行驶阻力：FR=Ft+Fw        
        private double distance = 0;//记录行驶距离,m
        private double totalDistance = 0;
        private double totalYh = 0;
        private double yhPerHundred = 0;
        private string pdjg;
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
                if (carinfo.货车车身型式==0|| carinfo.货车车身型式==1 || carinfo.货车车身型式==2)
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
                else if (carinfo.货车车身型式==3)
                {
                    Cd = 1.4;
                }
                else if (carinfo.货车车身型式==4 || carinfo.货车车身型式==5)
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
            if (Y_FTC < 0)
            {
                MessageBox.Show("油耗加载力计算结果为负值，请核对车辆信息是否准确完整", "错误");
            }
            else if (Y_FTC < 100)
            {
                dynconfigdata.FuelForceQj = 100;
                MessageBox.Show("油耗加载力计算结果较低，请核对车辆信息是否准确完整", "警告");
            }
            else if(Y_FTC<205)
            {
                Y_FTC = 205;
                dynconfigdata.FuelForceQj = 100;
            }
            else if (dynconfigdata.DynStopUnstable)
            {
                dynconfigdata.FuelForceQj = FuelForceQj + (float)(Y_FTC * 0.03);
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
            if (humidity <= humilist[0]) return 0;
            else if (humidity >= humilist[templist.Count - 1]) return humilist.Count - 1;
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
            int columnnumber = GetRow(Sd);
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
            int columnnumber = GetRow(Sd);
            φPaw = 水蒸气分压[rownumber, columnnumber];
            pa = Dqy - φPaw;
            αa = Math.Pow(99.0 / pa, 1.2) * Math.Pow((Wd+273.15) / 298, 0.6);
        }
        private void caculateCyFE()
        {
            Pe = carinfo.压燃式额定功率;
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
                            Y_Ftc = 140;
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
                            Y_Ftc = 160;
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
            X_Fc = fc * carinfo.驱动轴空载质量 * g;
            if (carinfo.压燃式功率参数类型 == 0)
                fp = 0.1;
            else
                fp = 0;
            X_Ff = 3600 * fp * Pe / Ve;
            X_Ft = 0.18 * (X_Fe - X_Ff);
            X_FE = X_Fe - X_Ftc - X_Fc - X_Ff - X_Ft;
        }
        private double Vm = 0,nm=0;
        /// <summary>
        /// 发动机额定扭矩
        /// </summary>
        private double Mm = 0;
        private void caculateQyFE()
        {
            Mm = carinfo.点燃式额定扭矩;
            nm = carinfo.点燃式额定扭矩转速;
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
                            Y_Ftc = 140;
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
                            Y_Ftc = 160;
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
            X_Fc = fc * carinfo.驱动轴空载质量 * g;
            if (carinfo.压燃式功率参数类型 == 0)
                fp = 0.1;
            else
                fp = 0;
            X_Ff = 0.377 * 0.06 * Mm*nm / Vm;
            X_Ft = 0.18 * (X_Fm - X_Ff);
            X_FM = X_Fm - X_Ftc - X_Fc - X_Ff - X_Ft;
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
        public float FuelForceQj = 0;
        private void yhTest_Load(object sender, EventArgs e)
        {
            //aquaGauge1.Value = 65f;
            //aquaGaugeRev.Value = 4500;
            carinfo = carini.getYhCarIni();
            
            initCurve();
            equipdata = configini.getEquipConfigIni();
            dynconfigdata = configini.getDynConfigIni();
            FuelForceQj = dynconfigdata.FuelForceQj;
            initEquipment();
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
                if (carinfo.货车车身型式 == 2 && carinfo.汽车类型 == 1)
                {
                    checkBox牵引车满载总质量.Enabled = true;
                }
                else
                {
                    checkBox牵引车满载总质量.Enabled = false;
                }
                caculateFTC();
                showCarInf();
                //Msg(labelCPH, panelCPH, carinfo.车辆号牌.ToUpper(), false);
                Msg(labelTS, panelTS, "请驶上线准备", true);
                showLed(carinfo.车辆号牌, "请驶上线准备　");
            }
            if (equipdata.WorkAutomaticMode)
                button1_Click(sender, e);
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
        private void showCarInf()
        {
            if (carinfo.汽车类型 == 0)
            {
                panel客车.Visible = true;
                panel货车.Visible = false;
                panel客车.Location = panelDetailsInf;
                textBox车牌号.Text = carinfo.车辆号牌;
                textBox号牌种类.Text = carinfo.号牌种类;
                switch (carinfo.客车等级)
                {
                    case 0:
                        textBox客车等级.Text = "高级";break;
                    case 1:
                        textBox客车等级.Text = "中级"; break;
                    case 2:
                        textBox客车等级.Text = "普通级"; break;
                    default:break;
                }
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
                    case 0: textBox轮胎规格.Text = "子午线";textBox轮胎断面宽度.Text = carinfo.子午胎轮胎断面宽度.ToString(); break;
                    case 1: textBox轮胎规格.Text = "斜交"; textBox轮胎断面宽度.Text = "--"; break;
                    default: break;
                }
                textBox前轮距.Text = carinfo.汽车前轮距.ToString();
                textBox汽车高度.Text = carinfo.汽车高度.ToString();
                textBox客车车长.Text = carinfo.客车车长.ToString();
                textBox车辆型号.Text = carinfo.车辆型号.ToString();
                textBox限值依据.Text = carinfo.油耗限值依据.ToString();
            }
            else
            {
                panel客车.Visible = false;
                panel货车.Visible = true;
                panel货车.Location = panelDetailsInf;
                textBox货车车牌号.Text = carinfo.车辆号牌;
                textBox货车号牌种类.Text = carinfo.号牌种类;
                switch (carinfo.货车车身型式)
                {
                    case 0:textBox货车车身型式.Text = "拦板车"; break;
                    case 1: textBox货车车身型式.Text = "自卸车"; break;
                    case 2: textBox货车车身型式.Text = "牵引车"; break;
                    case 3: textBox货车车身型式.Text = "仓栅车"; break;
                    case 4: textBox货车车身型式.Text = "厢式车"; break;
                    case 5: textBox货车车身型式.Text = "罐车"; break;
                    default:break;
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
                    case 0: textBox货车轮胎规格.Text = "子午线"; textBox货车轮胎宽度.Text = carinfo.子午胎轮胎断面宽度.ToString(); break;
                    case 1: textBox货车轮胎规格.Text = "斜交"; textBox货车轮胎宽度.Text = "--"; break;
                    default: break;
                }
                textBox货车前轮距.Text = carinfo.汽车前轮距.ToString();
                textBox货车汽车高度.Text = carinfo.汽车高度.ToString();
                textBox货车牵引车重量.Text = carinfo.牵引车满载总质量.ToString();
                textBox货车车辆型号.Text = carinfo.车辆型号.ToString();
                textBox货车限值.Text = carinfo.油耗限值依据.ToString();
            }
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
                    textBox参比值.Text = yhXz.ToString("0.00") + "(" + xzyjstirng + ")";
                }
                else
                {
                    textBox参比值.Text = "初始化限值失败";
                }
            }
            else
            {
                xzyj = 3;
                yhXz = carinfo.油耗限值;
                string xzyjstirng = "平台下发";
                textBox参比值.Text = yhXz.ToString("0.00") + "(" + xzyjstirng + ")";
            }
            textBoxV0.Text = v0.ToString("0.0");
            textBoxf.Text = f.ToString("0.000");
            textBoxFt.Text = Y_Ft.ToString("0");
            textBoxCd.Text = Cd.ToString("0.000");
            textBoxA.Text = A.ToString("0");
            textBoxFw.Text = Y_Fw.ToString("0");
            textBox道路行驶阻力.Text = Y_FR.ToString("0");
            textBoxfc.Text = fc.ToString("0.000");
            textBox台架滚动阻力.Text = Y_Ffc.ToString("0");
            textBox台架内阻.Text = Y_Ftc.ToString("0");
            textBox汽车台架运转阻力.Text = Y_Fc.ToString("0");
            textBox加载阻力.Text = Y_FTC.ToString("0");
        }
        private Curve2D curve1 = new Curve2D();
        private Curve2D curve2 = new Curve2D();
        private Curve2D curve3 = new Curve2D();
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
            curve2.Keys = new string[] { "0", "10", "20", "30", "40", "50", "60", "70", "80", "90" };
            curve2.Values = new float[] { 0f };
            curve2.CurveColors = new Color[] { Color.Green };
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
            //curve2.YSliceValue = 100;
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
                try
                {
                    if (equipdata.Ledifpz)
                    {
                        try
                        {
                            ledcontrol = new LedControl.BX5k1();
                            ledcontrol.row1 = (byte)(equipdata.ledrow1);
                            ledcontrol.row2 = (byte)(equipdata.ledrow2);
                            if (ledcontrol.Init_Comm(equipdata.Ledck, equipdata.LedComstring,(byte)equipdata.LEDTJPH) == false)
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
                        case nhxh:
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lx">1:上排 2:下排</param>
        /// <param name="ledstring2"></param>
        public void showLedSingleRow(int lx, string ledstring)
        {
            if (ledcontrol != null)
            {
                if(lx==1)
                    ledcontrol.writeLed(ledstring, 2, equipdata.Ledxh);
                else if(lx==2)
                    ledcontrol.writeLed(ledstring, 5, equipdata.Ledxh);
            }
        }
        public void showLed(string ledstring1,string ledstring2)
        {
            if (ledcontrol != null)
            {
                ledcontrol.writeLed(ledstring1, 2, equipdata.Ledxh);
                Thread.Sleep(200);
                ledcontrol.writeLed(ledstring2, 5, equipdata.Ledxh);
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
                if (igbt != null)
                    igbt.Lifter_Down();
        }

        bool JC_Status = false;
        yhrRealTimeStatus yhystatus = new yhrRealTimeStatus();
        fl_StatusAndData yhyflstatusanddata = new fl_StatusAndData();
        fl_StatusAndData yhyflstatusanddata_temp = new fl_StatusAndData();
        NH_status yhynhstatus = new NH_status();
        NH_status2 yhynhstatus2 = new NH_status2();
        NH_status yhynhstatus_temp = new NH_status();
        private int enginezs = 0;
        public static  float curvescalevalue = 0;
        public static bool displayCurveScale = false;
        private void button4_Click(object sender, EventArgs e)
        {
            curvescalevalue = 70;
            displayCurveScale = true;
            curve1.scale1Value = curvescalevalue;
            curve1.scale1Color = Color.Red;
            curve1.displayScale1 = displayCurveScale;
        }

        private void checkBox牵引车满载总质量_CheckedChanged(object sender, EventArgs e)
        {
            caculateFTC();
            showCarInf();
        }

        private bool isZsStable = false;
        private byte flGetAddDataFlag = 0;//福立取数指令的累计标志，0：不累计 1：累计
        private int nhdelaytime = 0;
        private void Jc_Exe()
        {           
            try
            {
                if (dynconfigdata.DynUseGddw)
                {
                    Msg(labelTS, panelTS, "请车辆到位", true);
                    showLedSingleRow(2, "请车辆到位　　　");
                    int gddwcount = 0;
                    while (gddwcount < dynconfigdata.DynGddwTime * 10)
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
                if (dynconfigdata.DynUseYkdw)
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
                panelSpeedOutRange.Size = new Size(10, 36);
                Msg(labelTS, panelTS, "举升器下降", true);
                showLedSingleRow(2, "举升器下降　　　");
                igbt.Lifter_Down(); //台体下降
                Thread.Sleep(500);
                igbt.Lifter_Down(); //台体下降
                Thread.Sleep(500);
                igbt.Lifter_Down(); //台体下降
                Thread.Sleep(3000);
                flGetAddDataFlag = 0;
                stopReadData = true;
                Msg(labelTS, panelTS, "准备油耗检测", true);
                showLed(carinfo.车辆号牌, "准备油耗检测　　");
                Thread.Sleep(3000);
                Msg(labelTS, panelTS, "检测即将开始", true);
                showLed(carinfo.车辆号牌, "检测即将开始　　");
                Thread.Sleep(1000);

                #region 调零及空气测定
                if (dynconfigdata.DynTl)
                {
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
                                    RefreshUI();
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
                }
                if (dynconfigdata.DynBackTest)
                {
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
                }
                if (dynconfigdata.DynCO2Test)
                {
                    Msg(labelTS, panelTS, "修正CO2", true);
                    showLed(carinfo.车辆号牌, "修正CO2　　");
                    yhy.amendCO2();
                    Thread.Sleep(1000);
                }
                #endregion
                
                #region 
                if (carinfo.燃料种类 == 0)//汽油
                    yhy.selectQy();
                else
                    yhy.selectCy();
                Thread.Sleep(500);
                #endregion

                igbt.Set_ClearKey();
                Msg(labelTS, panelTS, "固定采集管", true);
                showLed(carinfo.车辆号牌, "固定采集管　　　");
                if (equipdata.isIgbtContainGdyk)
                {
                    igbt.Set_ClearKey();
                    Thread.Sleep(1000);
                    while (true)
                    {
                        if (dynconfigdata.DynYkStyle == 0)
                        {
                            if (((igbt.keyandgd) & 0xf0) != 0x00)
                            {
                                igbt.Set_ClearKey();
                                break;
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
                                break;
                            }
                        }
                        Thread.Sleep(500);
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
                else if(radioButtonMF.Checked)
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
                Msg(labelTS, panelTS, "加速至"+v0.ToString("00")+"km/h", true);
                showLed("加速至" + v0.ToString("00") + "km/h"," "+speednow.ToString("00.0")+" km/h");
                while (Math.Abs(speednow- v0) > dynconfigdata.FuelSdQj)
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
                    while (igbt.igbt_status_now != IGBT.IGBT_STATUS.FORCE)
                    {
                        igbt.Start_Control_Force();
                        Thread.Sleep(200);
                    }
                }
                int jzsj = (int)(Y_FTC / dynconfigdata.FuelforceTime);
                if (jzsj <= 1)
                {
                    if (igbt != null)
                    {
                        igbt.Set_Control_Force((float)(Y_FTC * (carinfo.加载力比例 * dynconfigdata.DynNlxs)));
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
                            igbt.Set_Control_Force((float)(Y_FTC * (carinfo.加载力比例 * dynconfigdata.DynNlxs) * i*1.0 / (double)(jzsj)));
                        }

                        Msg(labelTS, panelTS, "加载中:" + (jzsj - i), true);
                        showLed("加载: " + forcenow.ToString("0000") + "N", " " + speednow.ToString("00.0") + " km/h");
                        Thread.Sleep(900);
                    }
                }
                igbt.Set_Control_Force((float)(Y_FTC * (carinfo.加载力比例 * dynconfigdata.DynNlxs)));
                Thread.Sleep(900);
                Msg(labelTS, panelTS, "加载完成", true);

                StartPosition:
                showLedSingleRow(1, "保持: " + v0.ToString("00") + "km/h");
                yhsmallthanxzcount = 0;
                int stableTime = 15;
                while(stableTime>=0)
                {
                    showLedSingleRow(2, "v:" + speednow.ToString("00.0") + " "+stableTime.ToString("00")+"s");
                    if (Math.Abs(speednow - v0) > dynconfigdata.FuelSdQj)
                    {
                        Msg(labelTS, panelTS, "保持" + v0.ToString("00") + "km/h/" + speednow.ToString("0.0"), true);
                        Thread.Sleep(900);
                        stableTime = 15;
                        continue;
                    }
                    else
                    {
                        Msg(labelTS, panelTS, "保持" +v0.ToString("00") + "km/h(" + stableTime.ToString() + "s)", true);
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
                while (!yhy.startTest(60,(int)(carinfo.额定总质量)))
                {
                    Thread.Sleep(900);
                }         
                
                Thread.Sleep(500);
                JC_Status = true;
                stopReadData = false;
                flGetAddDataFlag = 1;//开始累计取数
                if (equipdata.YhyXh == "mql_8201")
                {
                    while (yhydata.CHECKTIME >= 60)//如果
                    {
                        Thread.Sleep(900);
                    }
                }
                else if(equipdata.YhyXh == "fly_2000")
                {
                    while (yhyflstatusanddata.累加测试时间 >= 60)//如果
                    {
                        Thread.Sleep(900);
                    }
                }
                else if (equipdata.YhyXh == nhxh)
                {
                    while (yhynhdata.time >= 60)//如果
                    {
                        Thread.Sleep(900);
                    }
                }
                ccsj = 0;
                forceccsj = 0;
                int testTime = 60;
                showLedSingleRow(1, "保持"+v0.ToString("00")+"km/h"+testTime.ToString("00")+"s");
                DateTime pretime = DateTime.Now;
                DateTime nowtime = DateTime.Now;
                while (testTime > 0)
                {
                    nowtime = DateTime.Now;
                    TimeSpan ts = nowtime - pretime;
                    double tsmis = ts.TotalSeconds;
                    pretime = nowtime;
                    Msg(labelTS, panelTS, "保持" + v0.ToString("00") + "km/h(" + testTime + "s)", true);
                    if (Math.Abs(speednow - v0) > dynconfigdata.FuelSdQj)
                    {
                        ccsj += tsmis;
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
                        forceccsj += tsmis;
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
                    if(dynconfigdata.DynFuelJk&&testTime<55)
                    {
                        
                        if (equipdata.YhyXh == "mql_8201")
                        {
                            if (yhydata.SSYH < dynconfigdata.DynFuelXz)
                                yhsmallthanxzcount++;
                        }
                        else if (equipdata.YhyXh == "fly_2000")
                        {
                            if(yhyflstatusanddata.yh_ssyh<dynconfigdata.DynFuelXz)
                                yhsmallthanxzcount++;
                        }
                        else if (equipdata.YhyXh == nhxh)
                        {
                            if (yhynhdata.ssyh < dynconfigdata.DynFuelXz)
                                yhsmallthanxzcount++;
                        }
                        if (yhsmallthanxzcount>=3)
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
                            RefreshUI();
                            Ref_Control_Text(button1, "开始检测");
                            return;
                        }
                    }
                    if (equipdata.YhyXh == "mql_8201")
                    {
                        testTime = 60 - yhydata.CHECKTIME;
                    }
                    else if(equipdata.YhyXh=="fly_2000")
                    {
                        testTime = 60 - (int)yhyflstatusanddata.累加测试时间;
                    }
                    else if (equipdata.YhyXh == nhxh)
                    {
                        testTime = 60+nhdelaytime - (int)yhynhdata.time;
                    }
                    showLed( "保持" + v0.ToString("00") + "  " + testTime.ToString("00") + "s", "v:" + speednow.ToString("00.0") + " " + ccsj.ToString("0.0") + "s");
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
                yhPerHundred =Math.Round(totalYh*100 / totalDistance,1);
                //yhXz = 0;
                testTimes++;
                if (yhPerHundred > yhXz)
                    pdjg = "不合格";
                else
                    pdjg = "合格";
                showLedSingleRow(1, "百公里油耗：  ");
                showLedSingleRow(2, "   "+yhPerHundred.ToString("0.0")+"L");
                Thread.Sleep(2000);
                showTextboxText(textBox百公里油耗, yhPerHundred.ToString("0.0"));
                showTextboxText(textBox判定, pdjg);
                if (pdjg == "不合格" && testTimes < dynconfigdata.FuelTestFjjs+1)
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
                    
                    Msg(labelTS, panelTS, "检测"+pdjg+"，请减速", true);
                    if(pdjg=="合格")
                        showLedSingleRow(1, "　合格　　请减速");
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
                bool isLiftHasUp = false;
                if (dynconfigdata.DynFlowBack)
                {
                    for (int i = 30; i >= 0; i--)
                    {
                        Msg(labelTS, panelTS, "反吹中.." + i.ToString() + "s", true);
                        showLedSingleRow(2, "反吹中..." + i.ToString("00") + "s");
                        Thread.Sleep(900);
                        if (speednow < 0.1 && !isLiftHasUp)
                        {
                            igbt.Lifter_Up();
                            isLiftHasUp = true;
                        }
                    }
                }
                if (!yhy.stopAction())
                { yhy.stopAction(); }
                if (!yhy.stopFlow())
                { yhy.stopFlow(); }
                Thread.Sleep(1000);
                DataTable dyn_datatable = new DataTable();
                DataRow dr = null;
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
                        dyn_datatable.Rows.Add(dr);
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show("生成过程数据时出错：" + er.Message);
                    return;
                }

                Msg(labelTS, panelTS, "结果:" + yhPerHundred.ToString("0.0") + " 限值:"+yhXz.ToString("0.0") +" "+pdjg, true);
                Thread.Sleep(3000);
                csvwriter.SaveCSV(dyn_datatable, "D:/dataseconds/" + carinfo.车辆号牌 + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv");
                writeResult();                  
                Thread.Sleep(1000);
                    
                //System.IO.File.Delete(@"c:\jcdatatxt\carinfo.ini");
                while (speednow > 0.1)
                    Thread.Sleep(1000);
                
                igbt.Lifter_Up();
                Thread.Sleep(5000);//等待举升升起后出车
                Msg(labelTS, panelTS, "测量完毕", true);
                showLed("　　检测完毕　　", "　　　　　　　　");
                Thread.Sleep(2000);
                showLed("　　欢迎参检　　", "　　　　　　　　");
                // RefreshUI();
                this.Close();
            }
            catch
            {
                //this.Close();
            }

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
        private int yhsmallthanxzcount = 0;
        private void writeResult()
        {
            fuelResult resultdata = new fuelResult();
            resultdata.检测速度 = v0;
            resultdata.台架加载阻力 = Y_FTC;
            resultdata.燃料总消耗量 = totalYh;
            resultdata.总行驶里程 = totalDistance;
            resultdata.百公里燃料消耗量 = yhPerHundred;
            resultdata.限值 = yhXz;
            resultdata.限值依据 = xzyj;
            resultdata.判定结果 = (pdjg == "合格" ? 0 : 1);
            resultdata.汽车滚动阻力 = Y_Ft;
            resultdata.空气阻力 = Y_Fw;
            resultdata.滚动阻力系数 = f;
            resultdata.迎风面积 = A;
            resultdata.空气阻力系数 = Cd;
            resultdata.台架运转阻力 = Y_Fc;
            resultdata.台架滚动阻力 = Y_Ffc;
            resultdata.台架滚动阻力系数 = fc;
            resultdata.台架内阻 = Y_Ftc;
            carini.writeYhyResultIni(resultdata);
        }
        private void showResult()
        {
            showTextboxText(textBox百公里油耗, yhPerHundred.ToString("0.0"));
            showTextboxText(textBox参比值, yhXz.ToString("0.0"));
            showTextboxText(textBox判定, pdjg);
        }
        yhrRealTimeTotalData yhydata = new yhrRealTimeTotalData();
        yhrRealTimeData yhydata2 = new yhrRealTimeData();
        yhrRealTimePfwsszl yhydata3 = new yhrRealTimePfwsszl();
        NH_fuleData yhynhdata = new NH_fuleData();
        NH_standardData yhynhstandarddata = new NH_standardData();

        private void button5_Click(object sender, EventArgs e)
        {
            if (igbt != null)
                igbt.Force_Zeroing();
        }

        yhrRealTimeTotalData yhydata_temp = new yhrRealTimeTotalData();

        private void button6_Click(object sender, EventArgs e)
        {
            if (yhy != null)
                yhy.exitPrepare();
        }

        yhrRealTimeData yhydata2_temp = new yhrRealTimeData();
        yhrRealTimePfwsszl yhydata3_temp = new yhrRealTimePfwsszl();
        NH_fuleData nhdata_temp = new NH_fuleData();
        NH_standardData nhstandarddata_temp = new NH_standardData();
        private bool stopReadData = false;
        private void Fq_Detect()
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
                        else if(equipdata.YhyXh== nhxh)
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

        private void RefreshUI()
        {
            Msg(labelCO, panelCO, "0", false);
            Msg(labelCO2, panelCO2, "0", false);
            Msg(labelHC, panelHC, "0", false);
            Msg(labelLL, panelLL, "0", false);
            Msg(labelTIME, panelTIME, "0", false);
            Msg(labelSSYH, panelSSYH, "0", false);
            Msg(labelLJYH, panelLJYH, "0", false);
                 
            Msg(labelTEMP, panelTEMP, "0", false);
            Msg(labelHUMIDITY, panelHUNIDITY, "0", false);
            Msg(labelAT, panelAT, "0", false);
            
            
        }

        Thread Th_get_FqandLl = null;
        Thread TH_ST = null;
        int GKSJ = 0;
        double ccsj = 0;
        double forceccsj = 0;


        public static  List<float> speedlist = new List<float>();
        public static List<float> forcelist = new List<float>();
        public static List<float> powerlist = new List<float>();
        public static bool isReloadData = false;
        private DateTime starttime = DateTime.Now;
        private int gksj = 0;
        private int pregksj = 0;
        public static float speednow=0, forcenow=0, powernow=0;

        private bool showCurve = false;
        int showCount = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (igbt != null)
            {
                speednow = (float)(igbt.Speed * dynconfigdata.DynSdxs);
                //speednow = igbt.Speed;
                forcenow = (float)(igbt.Force / (carinfo.加载力比例*dynconfigdata.DynNlxs));
                powernow = (float)((speednow / 3.6 * forcenow) / 1000);
                Msg(labelSd, panelSd, speednow.ToString("0.0"),false);
                Msg(labelZs, panelZs, enginezs.ToString("0"), false);
                Msg(labelNl, panelNl, forcenow.ToString("0"), false);
                if (speednow > 0) aquaGauge1.Value = speednow;
                else aquaGauge1.Value = 0;
                if (forcenow > 0) aquaGaugeForce.Value = forcenow;
                else aquaGaugeForce.Value = 0;
                if (JC_Status)
                {

                    panelSpeedOutRange.Size = new Size((int)(200 * ccsj / 3.0), 36);
                }
            }
            if (showCount <= 2)
            {
                showCount = 0;
                if (showform != null) showform.DriverShow_OnMyChange(1, speednow.ToString("0.0"));
                if (showform != null) showform.DriverShow_OnMyChange(2, forcenow.ToString("0"));
                if (showform != null) showform.DriverShow_OnMyChange(3, distance.ToString("0.0"));
                if (showform != null)
                {
                    if (equipdata.YhyXh == "mql_8201")
                    {
                        showform.DriverShow_OnMyChange(4, yhydata.ZYH.ToString("0.0"));
                    }
                    else if (equipdata.YhyXh == "fly_2000")
                    {
                        showform.DriverShow_OnMyChange(4, yhyflstatusanddata.yh_ljyh.ToString("0.0"));
                    }
                    else if (equipdata.YhyXh == nhxh)
                    {
                        showform.DriverShow_OnMyChange(4, yhynhdata.ljyh.ToString("0.0"));
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

                    if (gksj < 900)
                    {
                        gksj = (int)(timesp.TotalMilliseconds / 100);
                        int timeafterpre = gksj - pregksj;//计算距离上次计算过去了几个时间间隔，每个时间间隔为100ms
                        pregksj = gksj;
                        if (jcStatus)
                            distance += speednow* timeafterpre * 1.0 / 3.6 * 0.1;

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
                            curve3.Values = powerlist.ToArray();
                            curve3.Fit();
                            pictureBox油耗.Image = curve3.CreateImage();
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
            timer2.Stop();
            try
            {
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
                if (ledcontrol != null)
                {
                    showLed("　　　　　　　　", "　　　　　　　　");
                    if (ledcontrol.ComPort_2.IsOpen)
                        ledcontrol.ComPort_2.Close();
                }
            }
            catch
            { }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (jcStatus == false)
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
                                if (yhy.getFlRealTimeDataUnAdd(0,out yhyflstatusanddata))
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
                    curve3.displayScale1 = false;
                    curve3.displayScale2 = false;
                    //开妈检测过程
                    TH_ST = new Thread(Jc_Exe); //检测过程
                    TH_ST.Start();
                    stopReadData = true;
                    flGetAddDataFlag = 0;
                    Th_get_FqandLl = new Thread(Fq_Detect); //碳平衡采集
                    Th_get_FqandLl.Start();
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
                        Th_get_FqandLl.Abort();
                    }
                    catch
                    { }
                    try
                    {
                        TH_ST.Abort();
                    }
                    catch
                    { }
                    yhy.stopAction();
                    yhy.stopFlow();
                    GKSJ = 0;
                    gksj_count = 0;  
                    
                    button1.Text = "开始检测";
                    //button2.Enabled = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("操作失败，发生异常:"+er.Message, "系统错误");
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

    }
}
