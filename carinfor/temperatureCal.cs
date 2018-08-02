using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ini;

namespace carinfor
{
    public class temperatureData
    {
        private double wdbz;

        public double Wdbz
        {
            get { return wdbz; }
            set { wdbz = value; }
        }
        private double wdscz;

        public double Wdscz
        {
            get { return wdscz; }
            set { wdscz = value; }
        }
        private double sdbz;

        public double Sdbz
        {
            get { return sdbz; }
            set { sdbz = value; }
        }
        private double sdscz;

        public double Sdscz
        {
            get { return sdscz; }
            set { sdscz = value; }
        }
        private double dqybz;

        public double Dqybz
        {
            get { return dqybz; }
            set { dqybz = value; }
        }
        private double dqyscz;

        public double Dqyscz
        {
            get { return dqyscz; }
            set { dqyscz = value; }
        }
        private double wdwc;

        public double Wdwc
        {
            get { return wdwc; }
            set { wdwc = value; }
        }
        private double sdwc;

        public double Sdwc
        {
            get { return sdwc; }
            set { sdwc = value; }
        }
        private double dqywc;

        public double Dqywc
        {
            get { return dqywc; }
            set { dqywc = value; }
        }
        private string bzsm;

        public string Bzsm
        {
            get { return bzsm; }
            set { bzsm = value; }
        }
        private string bdjg;

        public string Bdjg
        {
            get { return bdjg; }
            set { bdjg = value; }
        }

    }
    public class temperatureIni
    {
        public bool writeanalysismeterIni(temperatureData analysismeterdata)
        {
            try
            {
                ini.INIIO.WritePrivateProfileString("标定数据", "温度标值", analysismeterdata.Wdbz.ToString("0.0"), "C:/jcdatatxt/temperatureCal.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "温度测量值", analysismeterdata.Wdscz.ToString("0.0"), "C:/jcdatatxt/temperatureCal.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "湿度标值", analysismeterdata.Sdbz.ToString("0.0"), "C:/jcdatatxt/temperatureCal.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "湿度测量值", analysismeterdata.Sdscz.ToString("0.0"), "C:/jcdatatxt/temperatureCal.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "大气压标值", analysismeterdata.Dqybz.ToString("0.0"), "C:/jcdatatxt/temperatureCal.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "大气压测量值", analysismeterdata.Dqyscz.ToString("0.0"), "C:/jcdatatxt/temperatureCal.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "温度误差", analysismeterdata.Wdwc.ToString("0.0"), "C:/jcdatatxt/temperatureCal.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "湿度误差", analysismeterdata.Sdwc.ToString("0.0"), "C:/jcdatatxt/temperatureCal.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "大气压误差", analysismeterdata.Dqywc.ToString("0.0"), "C:/jcdatatxt/temperatureCal.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "备注说明", analysismeterdata.Bzsm, "C:/jcdatatxt/temperatureCal.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "标定结果", analysismeterdata.Bdjg, "C:/jcdatatxt/temperatureCal.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
