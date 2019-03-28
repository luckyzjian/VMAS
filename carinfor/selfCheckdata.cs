using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ini;
using System.IO;

namespace carinfor
{
    public class CGJselfCheckdata
    {


        private double hvitualtime;

        public double Hvitualtime
        {
            get { return hvitualtime; }
            set { hvitualtime = value; }
        }
        private double hrealtime;

        public double Hrealtime
        {
            get { return hrealtime; }
            set { hrealtime = value; }
        }
        private double lvitualtime;

        public double Lvitualtime
        {
            get { return lvitualtime; }
            set { lvitualtime = value; }
        }
        private double lrealtime;

        public double Lrealtime
        {
            get { return lrealtime; }
            set { lrealtime = value; }
        }
        private double hpower;

        public double Hpower
        {
            get { return hpower; }
            set { hpower = value; }
        }
        private double lpower;

        public double Lpower
        {
            get { return lpower; }
            set { lpower = value; }
        }
        private string checckResult;

        public string ChecckResult
        {
            get { return checckResult; }
            set { checckResult = value; }
        }
        private string checkTimeStart;

        public string CheckTimeStart
        {
            get { return checkTimeStart; }
            set { checkTimeStart = value; }
        }
        private string checkTimeEnd;

        public string CheckTimeEnd
        {
            get { return checkTimeEnd; }
            set { checkTimeEnd = value; }
        }
        private string remark;

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public string Gxdl
        {
            get
            {
                return gxdl;
            }

            set
            {
                gxdl = value;
            }
        }

        public string Jzhxds
        {
            get
            {
                return jzhxds;
            }

            set
            {
                jzhxds = value;
            }
        }

        public string Cs1
        {
            get
            {
                return cs1;
            }

            set
            {
                cs1 = value;
            }
        }

        public string Pc1
        {
            get
            {
                return pc1;
            }

            set
            {
                pc1 = value;
            }
        }

        public string Jsgl1
        {
            get
            {
                return jsgl1;
            }

            set
            {
                jsgl1 = value;
            }
        }

        public string Cs2
        {
            get
            {
                return cs2;
            }

            set
            {
                cs2 = value;
            }
        }

        public string Pc2
        {
            get
            {
                return pc2;
            }

            set
            {
                pc2 = value;
            }
        }

        public string Jsgl2
        {
            get
            {
                return jsgl2;
            }

            set
            {
                jsgl2 = value;
            }
        }

        public string Zjjg
        {
            get
            {
                return zjjg;
            }

            set
            {
                zjjg = value;
            }
        }

        private string gxdl;
        private string jzhxds;
        private string cs1;
        private string pc1;
        private string jsgl1;
        private string cs2;
        private string pc2;
        private string jsgl2;
        private string zjjg;
        public string kssj1 { set; get; }
        public string kssj2 { set; get; }
        public string jssj1 { set; get; }
        public string jssj2 { set; get; }
    }
    public class cgjPLHPSelfcheck
    {
        private string speedQJ1;

        public string SpeedQJ1
        {
            get { return speedQJ1; }
            set { speedQJ1 = value; }
        }
        private double nameSpeed1;

        public double NameSpeed1
        {
            get { return nameSpeed1; }
            set { nameSpeed1 = value; }
        }
        private double plhp1;

        public double Plhp1
        {
            get { return plhp1; }
            set { plhp1 = value; }
        }
        private string speedQJ2;

        public string SpeedQJ2
        {
            get { return speedQJ2; }
            set { speedQJ2 = value; }
        }
        private double nameSpeed2;

        public double NameSpeed2
        {
            get { return nameSpeed2; }
            set { nameSpeed2 = value; }
        }
        private double plhp2;

        public double Plhp2
        {
            get { return plhp2; }
            set { plhp2 = value; }
        }
        private string speedQJ3;

        public string SpeedQJ3
        {
            get { return speedQJ3; }
            set { speedQJ3 = value; }
        }
        private double nameSpeed3;

        public double NameSpeed3
        {
            get { return nameSpeed3; }
            set { nameSpeed3 = value; }
        }
        private double plhp3;

        public double Plhp3
        {
            get { return plhp3; }
            set { plhp3 = value; }
        }
        private string speedQJ4;

        public string SpeedQJ4
        {
            get { return speedQJ4; }
            set { speedQJ4 = value; }
        }
        private double nameSpeed4;

        public double NameSpeed4
        {
            get { return nameSpeed4; }
            set { nameSpeed4 = value; }
        }
        private double plhp4;

        public double Plhp4
        {
            get { return plhp4; }
            set { plhp4 = value; }
        }
        private double maxSpeed;

        public double MaxSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = value; }
        }
        private string checckResult;

        public string ChecckResult
        {
            get { return checckResult; }
            set { checckResult = value; }
        }
        private string checkTimeStart;

        public string CheckTimeStart
        {
            get { return checkTimeStart; }
            set { checkTimeStart = value; }
        }
        private string checkTimeEnd;

        public string CheckTimeEnd
        {
            get { return checkTimeEnd; }
            set { checkTimeEnd = value; }
        }
        private string remark;

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        public string hxsj1 { set; get; }
        public string kssj1 { set; get; }
        public string jssj1 { set; get; }
        public string hxsj2 { set; get; }
        public string kssj2 { set; get; }
        public string jssj2 { set; get; }
        public string hxsj3 { set; get; }
        public string kssj3 { set; get; }
        public string jssj3 { set; get; }
        public string hxsj4 { set; get; }
        public string kssj4 { set; get; }
        public string jssj4 { set; get; }
    }
    public class leakcheck
    {
        private string tightnessResult;

        public string TightnessResult
        {
            get { return tightnessResult; }
            set { tightnessResult = value; }
        }

    }
    public class wqfxySelfcheck
    {
        private string tightnessResult;

        public string TightnessResult
        {
            get { return tightnessResult; }
            set { tightnessResult = value; }
        }
        private string checkTimeStart;

        public string CheckTimeStart
        {
            get { return checkTimeStart; }
            set { checkTimeStart = value; }
        }
        private string checkTimeEnd;

        public string CheckTimeEnd
        {
            get { return checkTimeEnd; }
            set { checkTimeEnd = value; }
        }
        private string remark;

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public string Yqll
        {
            get
            {
                return yqll;
            }

            set
            {
                yqll = value;
            }
        }

        public string Yqyq
        {
            get
            {
                return yqyq;
            }

            set
            {
                yqyq = value;
            }
        }

        public string Zjjg
        {
            get
            {
                return zjjg;
            }

            set
            {
                zjjg = value;
            }
        }

        private string yqll;
        private string yqyq;
        private string zjjg;
    }
    public class ydjSelfcheck
    {
        private string zeroResult;

        public string ZeroResult
        {
            get { return zeroResult; }
            set { zeroResult = value; }
        }
        private double nZero;

        public double NZero
        {
            get { return nZero; }
            set { nZero = value; }
        }
        private double labelValueN50;

        public double LabelValueN50
        {
            get { return labelValueN50; }
            set { labelValueN50 = value; }
        }
        private double labelValueN70;

        public double LabelValueN70
        {
            get { return labelValueN70; }
            set { labelValueN70 = value; }
        }
        private double labelValueN90;

        public double LabelValueN90
        {
            get { return labelValueN90; }
            set { labelValueN90 = value; }
        }
        private double N50;

        public double N501
        {
            get { return N50; }
            set { N50 = value; }
        }
        private double N70;

        public double N701
        {
            get { return N70; }
            set { N70 = value; }
        }
        private double N90;

        public double N901
        {
            get { return N90; }
            set { N90 = value; }
        }
        private double Error50;

        public double Error501
        {
            get { return Error50; }
            set { Error50 = value; }
        }
        private double Error70;

        public double Error701
        {
            get { return Error70; }
            set { Error70 = value; }
        }
        private double Error90;

        public double Error901
        {
            get { return Error90; }
            set { Error90 = value; }
        }
        private string CheckResult;

        public string CheckResult1
        {
            get { return CheckResult; }
            set { CheckResult = value; }
        }
        private string checkTimeStart;

        public string CheckTimeStart
        {
            get { return checkTimeStart; }
            set { checkTimeStart = value; }
        }
        private string checkTimeEnd;

        public string CheckTimeEnd
        {
            get { return checkTimeEnd; }
            set { checkTimeEnd = value; }
        }
        private string remark;

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public string Jcds
        {
            get
            {
                return jcds;
            }

            set
            {
                jcds = value;
            }
        }

        public string Zjjg
        {
            get
            {
                return zjjg;
            }

            set
            {
                zjjg = value;
            }
        }

        private string jcds;
        private string zjjg;
    }
    public class lljSelfcheck
    {
        private string txJc;

        public string TxJc
        {
            get { return txJc; }
            set { txJc = value; }
        }
        private string lljll;

        public string Lljll
        {
            get { return lljll; }
            set { lljll = value; }
        }
        private string lljo2;

        public string Lljo2
        {
            get { return lljo2; }
            set { lljo2 = value; }
        }
        private string checkResult;

        public string CheckResult
        {
            get { return checkResult; }
            set { checkResult = value; }
        }
        private string checkTimeStart;

        public string CheckTimeStart
        {
            get { return checkTimeStart; }
            set { checkTimeStart = value; }
        }
        private string checkTimeEnd;

        public string CheckTimeEnd
        {
            get { return checkTimeEnd; }
            set { checkTimeEnd = value; }
        }
        private string remark;

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        public double lljsjll { set; get; }
        public double lljmyll { set; get; }
        public double lljsjllwc { set; get; }
    }
    public class sdsqtfxySelfcheck
    {
        private string tightnessResult;

        public string TightnessResult
        {
            get { return tightnessResult; }
            set { tightnessResult = value; }
        }
        private string lFlowResult;

        public string LFlowResult
        {
            get { return lFlowResult; }
            set { lFlowResult = value; }
        }
        private double canliuHC;

        public double CanliuHC
        {
            get { return canliuHC; }
            set { canliuHC = value; }
        }
        private string checkResult;

        public string CheckResult
        {
            get { return checkResult; }
            set { checkResult = value; }
        }
        private string checkTimeStart;

        public string CheckTimeStart
        {
            get { return checkTimeStart; }
            set { checkTimeStart = value; }
        }
        private string checkTimeEnd;

        public string CheckTimeEnd
        {
            get { return checkTimeEnd; }
            set { checkTimeEnd = value; }
        }
        private string remark;

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public string Yqll
        {
            get
            {
                return yqll;
            }

            set
            {
                yqll = value;
            }
        }

        public string Yqyq
        {
            get
            {
                return yqyq;
            }

            set
            {
                yqyq = value;
            }
        }

        public string Zjjg
        {
            get
            {
                return zjjg;
            }

            set
            {
                zjjg = value;
            }
        }

        private string yqll;
        private string yqyq;
        private string zjjg;
    }
    public class selfCheckRecord
    {
        private string checkdatetime;

        public string Checkdatetime
        {
            get { return checkdatetime; }
            set { checkdatetime = value; }
        }
        private bool cgjcheckrecord;

        public bool Cgjcheckrecord
        {
            get { return cgjcheckrecord; }
            set { cgjcheckrecord = value; }
        }
        private bool fqycheckrecord;

        public bool Fqycheckrecord
        {
            get { return fqycheckrecord; }
            set { fqycheckrecord = value; }
        }
        private bool ydjcheckrecord;

        public bool Ydjcheckrecord
        {
            get { return ydjcheckrecord; }
            set { ydjcheckrecord = value; }
        }
        private bool lljcheckrecord;

        public bool Lljcheckrecord
        {
            get { return lljcheckrecord; }
            set { lljcheckrecord = value; }
        }
    }
    public class hjcsgyqSelfcheck
    {
        private double actualTemperature;

        public double ActualTemperature
        {
            get { return actualTemperature; }
            set { actualTemperature = value; }
        }
        private double temperature;

        public double Temperature
        {
            get { return temperature; }
            set { temperature = value; }
        }
        private double actualHumidity;

        public double ActualHumidity
        {
            get { return actualHumidity; }
            set { actualHumidity = value; }
        }
        private double humidity;

        public double Humidity
        {
            get { return humidity; }
            set { humidity = value; }
        }
        private double actualAirPressure;

        public double ActualAirPressure
        {
            get { return actualAirPressure; }
            set { actualAirPressure = value; }
        }
        private double airPressure;

        public double AirPressure
        {
            get { return airPressure; }
            set { airPressure = value; }
        }
        private string checkTimeStart;

        public string CheckTimeStart
        {
            get { return checkTimeStart; }
            set { checkTimeStart = value; }
        }
        private string checkTimeEnd;

        public string CheckTimeEnd
        {
            get { return checkTimeEnd; }
            set { checkTimeEnd = value; }
        }
        private string remark;

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        private bool tempok;

        public bool Tempok
        {
            get { return tempok; }
            set { tempok = value; }
        }
        private bool humiok;

        public bool Humiok
        {
            get { return humiok; }
            set { humiok = value; }
        }
        private bool airpok;

        public bool Airpok
        {
            get { return airpok; }
            set { airpok = value; }
        }

        public string Zjjg
        {
            get
            {
                return zjjg;
            }

            set
            {
                zjjg = value;
            }
        }

        private string zjjg;
    }
    public class fdjzsSelfCheck
    {
        private string dszs;
        private string zjjg;

        public string Dszs
        {
            get
            {
                return dszs;
            }

            set
            {
                dszs = value;
            }
        }

        public string Zjjg
        {
            get
            {
                return zjjg;
            }

            set
            {
                zjjg = value;
            }
        }
    }
    public class wqfxyAdjust
    {
        private string gasType;
        private double labelValueCO2;
        private double detectValueCO2;
        private double labelValueCO;
        private double detectValueCO;
        private double labelValueNO;
        private double detectValueNO;
        private double labelValueHC;
        private double detectValueHC;
        private double labelValueO2;
        private double detectValueO2;
        private double labelValuePEF;
        private double labelValueC3H8;
        private string adjustResult;
        private string adjustTimeStart;
        private string adjustTimeEnd;
        private string remark;


    }
    public class selfCheckState
    {
        private string chCmd;
        private string nType;
        private string sjxl;
        private string sbbh;

        public string ChCmd
        {
            get
            {
                return chCmd;
            }

            set
            {
                chCmd = value;
            }
        }

        public string NType
        {
            get
            {
                return nType;
            }

            set
            {
                nType = value;
            }
        }

        public string Sjxl
        {
            get
            {
                return sjxl;
            }

            set
            {
                sjxl = value;
            }
        }

        public string Sbbh
        {
            get
            {
                return sbbh;
            }

            set
            {
                sbbh = value;
            }
        }
    }

    public class selfCheckIni
    {
        public string startUpPath = AppDomain.CurrentDomain.BaseDirectory;
        public bool writeSelfCheckState(selfCheckState cgjcheckdata)
        {
            try
            {
                ini.INIIO.WritePrivateProfileString("自检数据", "chCmd", cgjcheckdata.ChCmd, "C:/jcdatatxt/checkstatedata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "nType", cgjcheckdata.NType, "C:/jcdatatxt/checkstatedata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "sjxl", cgjcheckdata.Sjxl, "C:/jcdatatxt/checkstatedata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "sbbh", cgjcheckdata.Sbbh, "C:/jcdatatxt/checkstatedata.ini");
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool writeCGJcheckIni(CGJselfCheckdata cgjcheckdata)
        {
            try
            {
                ini.INIIO.WritePrivateProfileString("自检数据", "gxdl", cgjcheckdata.Gxdl, "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "HVitualTime", cgjcheckdata.Hvitualtime.ToString("0.000"), "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "HrealTime", cgjcheckdata.Hrealtime.ToString("0.000"), "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "LvitualTime", cgjcheckdata.Lvitualtime.ToString("0.000"), "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "LrealTime", cgjcheckdata.Lrealtime.ToString("0.000"), "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "jzhxds", cgjcheckdata.Jzhxds, "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "Hpower", cgjcheckdata.Hpower.ToString(), "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "cs1", cgjcheckdata.Cs1, "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "pc1", cgjcheckdata.Pc1, "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "kssj1", cgjcheckdata.kssj1, "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "jssj1", cgjcheckdata.jssj1, "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "jsgl1", cgjcheckdata.Jsgl1, "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "Lpower", cgjcheckdata.Lpower.ToString(), "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "cs2", cgjcheckdata.Cs2, "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "pc2", cgjcheckdata.Pc2, "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "kssj2", cgjcheckdata.kssj2, "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "jssj2", cgjcheckdata.jssj2, "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "jsgl2", cgjcheckdata.Jsgl2, "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "checkResult", cgjcheckdata.ChecckResult, "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "checkTimeStart", cgjcheckdata.CheckTimeStart, "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "checkTimeEnd", cgjcheckdata.CheckTimeEnd, "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "remark", cgjcheckdata.Remark, "C:/jcdatatxt/CGJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "zjjg", cgjcheckdata.Zjjg, "C:/jcdatatxt/CGJcheckdata.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool writeCheckRecord(selfCheckRecord cgjcheckdata)
        {
            try
            {
                ini.INIIO.WritePrivateProfileString("自检结果", "时间", cgjcheckdata.Checkdatetime, startUpPath+"/detectConfig.ini");
                ini.INIIO.WritePrivateProfileString("自检结果", "测功机", cgjcheckdata.Cgjcheckrecord ? "Y" : "N", startUpPath+"/detectConfig.ini");
                ini.INIIO.WritePrivateProfileString("自检结果", "废气仪", cgjcheckdata.Fqycheckrecord ? "Y" : "N", startUpPath+"/detectConfig.ini");
                ini.INIIO.WritePrivateProfileString("自检结果", "烟度计", cgjcheckdata.Ydjcheckrecord ? "Y" : "N", startUpPath+"/detectConfig.ini");
                ini.INIIO.WritePrivateProfileString("自检结果", "流量计", cgjcheckdata.Lljcheckrecord ? "Y" : "N", startUpPath+"/detectConfig.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public selfCheckRecord getCheckRecord()
        {
            selfCheckRecord configinidata = new selfCheckRecord();
            //parasiticData configinidata = new parasiticData();
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            ini.INIIO.GetPrivateProfileString("自检结果", "时间", "", temp, 2048, startUpPath+"/detectConfig.ini");
            configinidata.Checkdatetime = temp.ToString();
            ini.INIIO.GetPrivateProfileString("自检结果", "测功机", "", temp, 2048, startUpPath+"/detectConfig.ini");
            configinidata.Cgjcheckrecord = (temp.ToString() == "Y");
            ini.INIIO.GetPrivateProfileString("自检结果", "废气仪", "", temp, 2048, startUpPath+"/detectConfig.ini");
            configinidata.Fqycheckrecord = (temp.ToString() == "Y");
            ini.INIIO.GetPrivateProfileString("自检结果", "烟度计", "", temp, 2048, startUpPath+"/detectConfig.ini");
            configinidata.Ydjcheckrecord = (temp.ToString() == "Y");
            ini.INIIO.GetPrivateProfileString("自检结果", "流量计", "", temp, 2048, startUpPath+"/detectConfig.ini");
            configinidata.Lljcheckrecord = (temp.ToString() == "Y");
            return configinidata;
        }
        public bool writeLLJcheckIni(lljSelfcheck cgjcheckdata)
        {
            try
            {
                ini.INIIO.WritePrivateProfileString("自检数据", "Lljsjll", cgjcheckdata.lljsjll.ToString(), "C:/jcdatatxt/LLJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "Lljmyll", cgjcheckdata.lljmyll.ToString(), "C:/jcdatatxt/LLJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "Lljsjllwc", cgjcheckdata.lljsjllwc.ToString(), "C:/jcdatatxt/LLJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "Lljll", cgjcheckdata.Lljll.ToString(), "C:/jcdatatxt/LLJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "Lljo2", cgjcheckdata.Lljo2.ToString(), "C:/jcdatatxt/LLJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "Lljtx", cgjcheckdata.TxJc.ToString(), "C:/jcdatatxt/LLJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "checkResult", cgjcheckdata.CheckResult, "C:/jcdatatxt/LLJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "checkTimeStart", cgjcheckdata.CheckTimeStart, "C:/jcdatatxt/LLJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "checkTimeEnd", cgjcheckdata.CheckTimeEnd, "C:/jcdatatxt/LLJcheckdata.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "remark", cgjcheckdata.Remark, "C:/jcdatatxt/LLJcheckdata.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool writecgjPLHPSelfcheck(cgjPLHPSelfcheck cgjcheckdata)
        {
            try
            {
                ini.INIIO.WritePrivateProfileString("自检数据", "speedQJ1", cgjcheckdata.SpeedQJ1, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "nameSpeed1", cgjcheckdata.NameSpeed1.ToString(), "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "plhp1", cgjcheckdata.Plhp1.ToString(), "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "speedQJ2", cgjcheckdata.SpeedQJ2, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "nameSpeed2", cgjcheckdata.NameSpeed2.ToString(), "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "plhp2", cgjcheckdata.Plhp2.ToString(), "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "speedQJ3", cgjcheckdata.SpeedQJ3, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "nameSpeed3", cgjcheckdata.NameSpeed3.ToString(), "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "plhp3", cgjcheckdata.Plhp3.ToString(), "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "speedQJ4", cgjcheckdata.SpeedQJ4, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "nameSpeed4", cgjcheckdata.NameSpeed4.ToString(), "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "plhp4", cgjcheckdata.Plhp4.ToString(), "C:/jcdatatxt/cgjPLHPSelfcheck.ini");

                ini.INIIO.WritePrivateProfileString("自检数据", "maxSpeed", cgjcheckdata.MaxSpeed.ToString(), "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "checkResult", cgjcheckdata.ChecckResult, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "checkTimeStart", cgjcheckdata.CheckTimeStart, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "checkTimeEnd", cgjcheckdata.CheckTimeEnd, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "remark", cgjcheckdata.Remark, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "hxsj1", cgjcheckdata.hxsj1, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "kssj1", cgjcheckdata.kssj1, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "jssj1", cgjcheckdata.jssj1, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "hxsj2", cgjcheckdata.hxsj2, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "kssj2", cgjcheckdata.kssj2, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "jssj2", cgjcheckdata.jssj2, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "hxsj3", cgjcheckdata.hxsj3, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "kssj3", cgjcheckdata.kssj3, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "jssj3", cgjcheckdata.jssj3, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "hxsj4", cgjcheckdata.hxsj4, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "kssj4", cgjcheckdata.kssj4, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "jssj4", cgjcheckdata.jssj4, "C:/jcdatatxt/cgjPLHPSelfcheck.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool writewqfxySelfcheck(wqfxySelfcheck cgjcheckdata)
        {
            try
            {
                ini.INIIO.WritePrivateProfileString("自检数据", "Yqll", cgjcheckdata.Yqll, "C:/jcdatatxt/wqfxySelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "Yqyq", cgjcheckdata.Yqyq, "C:/jcdatatxt/wqfxySelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "Zjjg", cgjcheckdata.Zjjg, "C:/jcdatatxt/wqfxySelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "tightnessResult", cgjcheckdata.TightnessResult, "C:/jcdatatxt/wqfxySelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "checkTimeStart", cgjcheckdata.CheckTimeStart, "C:/jcdatatxt/wqfxySelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "checkTimeEnd", cgjcheckdata.CheckTimeEnd, "C:/jcdatatxt/wqfxySelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "remark", cgjcheckdata.Remark, "C:/jcdatatxt/wqfxySelfcheck.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool writeLeakTestResult(leakcheck cgjcheckdata)
        {
            try
            {
                ini.INIIO.WritePrivateProfileString("自检数据", "tightnessResult", cgjcheckdata.TightnessResult, "C:/jcdatatxt/leakcheck.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool writeydjSelfcheck(ydjSelfcheck cgjcheckdata)
        {
            try
            {
                ini.INIIO.WritePrivateProfileString("自检数据", "zeroResult", cgjcheckdata.ZeroResult, "C:/jcdatatxt/ydjSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "labelValueN50", cgjcheckdata.LabelValueN50.ToString(), "C:/jcdatatxt/ydjSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "labelValueN70", cgjcheckdata.LabelValueN70.ToString(), "C:/jcdatatxt/ydjSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "N50", cgjcheckdata.N501.ToString(), "C:/jcdatatxt/ydjSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "N70", cgjcheckdata.N701.ToString(), "C:/jcdatatxt/ydjSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "Error50", cgjcheckdata.Error501.ToString(), "C:/jcdatatxt/ydjSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "Error70", cgjcheckdata.Error701.ToString(), "C:/jcdatatxt/ydjSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "NZero", cgjcheckdata.NZero.ToString(), "C:/jcdatatxt/ydjSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "LabelValueN90", cgjcheckdata.LabelValueN90.ToString(), "C:/jcdatatxt/ydjSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "N90", cgjcheckdata.N901.ToString(), "C:/jcdatatxt/ydjSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "Error90", cgjcheckdata.Error901.ToString(), "C:/jcdatatxt/ydjSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "CheckResult", cgjcheckdata.CheckResult1, "C:/jcdatatxt/ydjSelfcheck.ini");

                ini.INIIO.WritePrivateProfileString("自检数据", "checkTimeStart", cgjcheckdata.CheckTimeStart, "C:/jcdatatxt/ydjSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "checkTimeEnd", cgjcheckdata.CheckTimeEnd, "C:/jcdatatxt/ydjSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "remark", cgjcheckdata.Remark, "C:/jcdatatxt/ydjSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "jcds", cgjcheckdata.Jcds, "C:/jcdatatxt/ydjSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "zjjg", cgjcheckdata.Zjjg, "C:/jcdatatxt/ydjSelfcheck.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool writesdsqtfxySelfcheck(sdsqtfxySelfcheck cgjcheckdata)
        {
            try
            {
                ini.INIIO.WritePrivateProfileString("自检数据", "Yqll", cgjcheckdata.Yqll, "C:/jcdatatxt/sdsqtfxySelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "Yqyq", cgjcheckdata.Yqyq, "C:/jcdatatxt/sdsqtfxySelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "Zjjg", cgjcheckdata.Zjjg, "C:/jcdatatxt/sdsqtfxySelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "tightnessResult", cgjcheckdata.TightnessResult, "C:/jcdatatxt/sdsqtfxySelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "checkTimeStart", cgjcheckdata.CheckTimeStart, "C:/jcdatatxt/sdsqtfxySelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "checkTimeEnd", cgjcheckdata.CheckTimeEnd, "C:/jcdatatxt/sdsqtfxySelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "remark", cgjcheckdata.Remark, "C:/jcdatatxt/sdsqtfxySelfcheck.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool writedzhjSelfcheck(hjcsgyqSelfcheck cgjcheckdata)
        {
            try
            {
                ini.INIIO.WritePrivateProfileString("自检数据", "actualTemperature", cgjcheckdata.ActualTemperature.ToString("0.0"), "C:/jcdatatxt/hjcsgyqSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "temperature", cgjcheckdata.Temperature.ToString("0.0"), "C:/jcdatatxt/hjcsgyqSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "actualHumidity", cgjcheckdata.ActualHumidity.ToString("0.0"), "C:/jcdatatxt/hjcsgyqSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "humidity", cgjcheckdata.Humidity.ToString("0.0"), "C:/jcdatatxt/hjcsgyqSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "actualAirPressure", cgjcheckdata.ActualAirPressure.ToString("0.0"), "C:/jcdatatxt/hjcsgyqSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "airPressure", cgjcheckdata.AirPressure.ToString("0.0"), "C:/jcdatatxt/hjcsgyqSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "checkTimeStart", cgjcheckdata.CheckTimeStart, "C:/jcdatatxt/hjcsgyqSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "checkTimeEnd", cgjcheckdata.CheckTimeEnd, "C:/jcdatatxt/hjcsgyqSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "remark", cgjcheckdata.Remark, "C:/jcdatatxt/hjcsgyqSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "tempok", cgjcheckdata.Tempok?"Y":"N", "C:/jcdatatxt/hjcsgyqSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "humiok", cgjcheckdata.Humiok ? "Y" : "N", "C:/jcdatatxt/hjcsgyqSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "airpok", cgjcheckdata.Airpok ? "Y" : "N", "C:/jcdatatxt/hjcsgyqSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "zjjg", cgjcheckdata.Zjjg, "C:/jcdatatxt/hjcsgyqSelfcheck.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool writefdjzsSelfcheck(fdjzsSelfCheck cgjcheckdata)
        {
            try
            {
                ini.INIIO.WritePrivateProfileString("自检数据", "Dszs", cgjcheckdata.Dszs, "C:/jcdatatxt/fdjzsSelfcheck.ini");
                ini.INIIO.WritePrivateProfileString("自检数据", "Zjjg", cgjcheckdata.Zjjg, "C:/jcdatatxt/fdjzsSelfcheck.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
