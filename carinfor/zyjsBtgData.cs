using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace carinfor
{
    public class zyjsBtgdata
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
        private string dszs;

        public string Dszs
        {
            get { return dszs; }
            set { dszs = value; }
        }
        private string prepareData;
        public string PrepareData
        {
            get
            {
                return prepareData;
            }

            set
            {
                prepareData = value;
            }
        }
        private string firstData;

        public string FirstData
        {
            get { return firstData; }
            set { firstData = value; }
        }
        private string secondData;

        public string SecondData
        {
            get { return secondData; }
            set { secondData = value; }
        }
        private string thirdData;

        public string ThirdData
        {
            get { return thirdData; }
            set { thirdData = value; }
        }

        private string yw;
        public string Yw
        {
            get { return yw; }
            set { yw = value; }
        }
        private string smokeAvg;

        public string SmokeAvg
        {
            get { return smokeAvg; }
            set { smokeAvg = value; }
        }
        private string startTime;

        public string StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        private string stopReason;

        public string StopReason
        {
            get { return stopReason; }
            set { stopReason = value; }
        }
        public string Rev1 { set; get; }
        public string Rev2 { set; get; }
        public string Rev3 { set; get; }

    }
    public class zyjsBtgdataControl
    {
        public bool writeJzjsData(zyjsBtgdata jzjs_data)
        {
            
            try
            {
                if (File.Exists("C:/jcdatatxt/" + jzjs_data.CarID + ".ini"))
                {
                    File.Delete("C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                }
                ini.INIIO.WritePrivateProfileString("检测结果", "车辆ID", jzjs_data.CarID, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "相对湿度", jzjs_data.Sd, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "环境温度", jzjs_data.Wd, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "大气压力", jzjs_data.Dqy, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值1", jzjs_data.Dszs, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值2", jzjs_data.FirstData, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值3", jzjs_data.SecondData, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值4", jzjs_data.ThirdData, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值5", jzjs_data.Yw, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值6", jzjs_data.PrepareData, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "StartTime", jzjs_data.StartTime, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "StopReason", jzjs_data.StopReason, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "SmokeAvg", jzjs_data.SmokeAvg, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "Rev1", jzjs_data.Rev1, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "Rev2", jzjs_data.Rev2, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "Rev3", jzjs_data.Rev3, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
