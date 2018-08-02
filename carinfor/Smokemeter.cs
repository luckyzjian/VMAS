using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ini;

namespace carinfor
{
    public class Smokemeterdata
    {
        private float kbz;

        public float Kbz
        {
            get { return kbz; }
            set { kbz = value; }
        }
        private float kscz;

        public float Kscz
        {
            get { return kscz; }
            set { kscz = value; }
        }
        private float kabswc;

        public float Kabswc
        {
            get { return kabswc; }
            set { kabswc = value; }
        }
        private float krelwc;

        public float Krelwc
        {
            get { return krelwc; }
            set { krelwc = value; }
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
    public class SmokemeterIni
    {
        public bool writeanalysismeterIni(Smokemeterdata flowmeterdata)
        {
            try
            {
                ini.INIIO.WritePrivateProfileString("标定数据", "K标定值", flowmeterdata.Kbz.ToString("0.00"), "C:/jcdatatxt/Smokemeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "K实测值", flowmeterdata.Kscz.ToString("0.00"), "C:/jcdatatxt/Smokemeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "K绝对误差", flowmeterdata.Kabswc.ToString("0.00"), "C:/jcdatatxt/Smokemeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "K相对误差", flowmeterdata.Krelwc.ToString("0.00"), "C:/jcdatatxt/Smokemeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "备注说明", flowmeterdata.Bzsm, "C:/jcdatatxt/Smokemeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "标定结果", flowmeterdata.Bdjg, "C:/jcdatatxt/Smokemeter.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
