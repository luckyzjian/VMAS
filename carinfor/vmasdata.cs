using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace carinfor
{
    public class vmasdata
    {
        private string carID;

        public string CarID
        {
            get { return carID; }
            set { carID = value; }
        }
        private string sd;

        public string Sd
        {
            get { return sd; }
            set { sd = value; }
        }
        private string wd;

        public string Wd
        {
            get { return wd; }
            set { wd = value; }
        }
        private string dqy;

        public string Dqy
        {
            get { return dqy; }
            set { dqy = value; }
        }
        private string cozl;

        public string Cozl
        {
            get { return cozl; }
            set { cozl = value; }
        }
        private string hczl;

        public string Hczl
        {
            get { return hczl; }
            set { hczl = value; }
        }
        private string noxzl;

        public string Noxzl
        {
            get { return noxzl; }
            set { noxzl = value; }
        }
        private string ljcc;

        public string Ljcc
        {
            get { return ljcc; }
            set { ljcc = value; }
        }
        private string ambientO2;

        public string AmbientO2
        {
            get { return ambientO2; }
            set { ambientO2 = value; }
        }
        private string residualHC;

        public string ResidualHC
        {
            get { return residualHC; }
            set { residualHC = value; }
        }
        private string testTime;

        public string TestTime
        {
            get { return testTime; }
            set { testTime = value; }
        }
        private string airFlowAll;

        public string AirFlowAll
        {
            get { return airFlowAll; }
            set { airFlowAll = value; }
        }
        private string hcnox;

        public string Hcnox
        {
            get { return hcnox; }
            set { hcnox = value; }
        }
        private string co2;

        public string Co2
        {
            get { return co2; }
            set { co2 = value; }
        }
        private string power;

        public string Power
        {
            get { return power; }
            set { power = value; }
        }
        private string sjxslc;

        public string Sjxslc
        {
            get { return sjxslc; }
            set { sjxslc = value; }
        }
        private string starttime;

        public string Starttime
        {
            get { return starttime; }
            set { starttime = value; }
        }
        private string stopreason;

        public string Stopreason
        {
            get { return stopreason; }
            set { stopreason = value; }
        }
        private string lambda;
        public string LAMBDA
        {
            get { return lambda; }
            set { lambda = value; }
        }

    }
    public class vmasdataControl
    {
        public bool writeVmasData(vmasdata vmas_data)
        {
            try
            {
                if (File.Exists("C:/jcdatatxt/" + vmas_data.CarID + ".ini"))
                {
                    File.Delete("C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                }
                ini.INIIO.WritePrivateProfileString("检测结果", "车辆ID", vmas_data.CarID, "C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "相对湿度", vmas_data.Sd, "C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "环境温度", vmas_data.Wd, "C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "大气压力", vmas_data.Dqy, "C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值1", vmas_data.Cozl, "C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值2", vmas_data.Hczl, "C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值3", vmas_data.Noxzl, "C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "累计超差", vmas_data.Ljcc, "C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "AmbientO2", vmas_data.AmbientO2, "C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "ResidualHC", vmas_data.ResidualHC, "C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "TestTime", vmas_data.TestTime, "C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "AirFlowAll", vmas_data.AirFlowAll, "C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "实际行驶里程", vmas_data.Sjxslc, "C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "Power", vmas_data.Power, "C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "Starttime", vmas_data.Starttime, "C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "Stopreason", vmas_data.Stopreason, "C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "Hcnox", vmas_data.Hcnox, "C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "co2", vmas_data.Co2, "C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "lambda", vmas_data.LAMBDA, "C:/jcdatatxt/" + vmas_data.CarID + ".ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
