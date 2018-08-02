using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace carinfor
{
    public class sdsdata
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
        private string λ;

        public string λ_value
        {
            get { return λ; }
            set { λ = value; }
        }
        private string co_low;

        public string Co_low
        {
            get { return co_low; }
            set { co_low = value; }
        }
        private string hc_low;

        public string Hc_low
        {
            get { return hc_low; }
            set { hc_low = value; }
        }
        private string co_high;

        public string Co_high
        {
            get { return co_high; }
            set { co_high = value; }
        }
        private string hc_high;

        public string Hc_high
        {
            get { return hc_high; }
            set { hc_high = value; }
        }        private string co2_high;

        public string Co2_high
        {
            get { return co2_high; }
            set { co2_high = value; }
        }
        private string o2_high;

        public string O2_high
        {
            get { return o2_high; }
            set { o2_high = value; }
        }
        private string co2_low;

        public string Co2_low
        {
            get { return co2_low; }
            set { co2_low = value; }
        }
        private string o2_low;

        public string O2_low
        {
            get { return o2_low; }
            set { o2_low = value; }
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
    }
    public class sdsdataControl
    {
        public bool writeSdsData(sdsdata sds_data)
        {

            try
            {
                if (File.Exists("C:/jcdatatxt/" + sds_data.CarID + ".ini"))
                {
                    File.Delete("C:/jcdatatxt/" + sds_data.CarID + ".ini");
                }
                ini.INIIO.WritePrivateProfileString("检测结果", "车辆ID", sds_data.CarID, "C:/jcdatatxt/" + sds_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "相对湿度", sds_data.Sd, "C:/jcdatatxt/" + sds_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "环境温度", sds_data.Wd, "C:/jcdatatxt/" + sds_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "大气压力", sds_data.Dqy, "C:/jcdatatxt/" + sds_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值1", sds_data.λ_value, "C:/jcdatatxt/" + sds_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值2", sds_data.Co_low, "C:/jcdatatxt/" + sds_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值3", sds_data.Hc_low, "C:/jcdatatxt/" + sds_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值4", sds_data.Co_high, "C:/jcdatatxt/" + sds_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值5", sds_data.Hc_high, "C:/jcdatatxt/" + sds_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "co2high", sds_data.Co2_high, "C:/jcdatatxt/" + sds_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "o2high", sds_data.O2_high, "C:/jcdatatxt/" + sds_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "co2low", sds_data.Co2_low, "C:/jcdatatxt/" + sds_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "o2low", sds_data.O2_low, "C:/jcdatatxt/" + sds_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "StartTime", sds_data.StartTime, "C:/jcdatatxt/" + sds_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "StopReason", sds_data.StopReason, "C:/jcdatatxt/" + sds_data.CarID + ".ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
