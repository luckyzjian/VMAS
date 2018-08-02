using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace carinfor
{
    public class xysjData
    {
        private string sd;

        public string Sd
        {
            get { return sd; }
            set { sd = value; }
        }
        private string startPower;

        public string StartPower
        {
            get { return startPower; }
            set { startPower = value; }
        }
        private string endPower;

        public string EndPower
        {
            get { return endPower; }
            set { endPower = value; }
        }
        private string startForce;

        public string StartForce
        {
            get { return startForce; }
            set { startForce = value; }
        }
        private string endForce;

        public string EndForce
        {
            get { return endForce; }
            set { endForce = value; }
        }
        private string xyTime;

        public string XyTime
        {
            get { return xyTime; }
            set { xyTime = value; }
        }
        private string wdTime;

        public string WdTime
        {
            get { return wdTime; }
            set { wdTime = value; }
        }
        private string bdjg;

        public string Bdjg
        {
            get { return bdjg; }
            set { bdjg = value; }
        }
    }
    public class xysjControl
    {
        public bool writeBzGlideIni(xysjData glidedata)
        {
            try
            {
                if (File.Exists("C:/jcdatatxt/xysjData.ini"))
                {
                    File.Delete("C:/jcdatatxt/xysjData.ini");
                }
                //configInfdata preConfigData = getConfigIni();
                ini.INIIO.WritePrivateProfileString("标定数据", "速度", glidedata.Sd, "C:/jcdatatxt/xysjData.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "初载荷", glidedata.StartPower, "C:/jcdatatxt/xysjData.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "初制动力", glidedata.StartForce, "C:/jcdatatxt/xysjData.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "末载荷", glidedata.EndPower, "C:/jcdatatxt/xysjData.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "末制动力", glidedata.EndForce, "C:/jcdatatxt/xysjData.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "响应时间", glidedata.XyTime, "C:/jcdatatxt/xysjData.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "稳定时间", glidedata.WdTime, "C:/jcdatatxt/xysjData.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "标定结果", glidedata.Bdjg, "C:/jcdatatxt/xysjData.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
