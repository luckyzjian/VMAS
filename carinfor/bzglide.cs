using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace carinfor
{
    public class bzglide
    {
        private string hxqj;

        public string Hxqj
        {
            get { return hxqj; }
            set { hxqj = value; }
        }
        private string ccdt;

        public string Ccdt
        {
            get { return ccdt; }
            set { ccdt = value; }
        }
        private string acdt;

        public string Acdt
        {
            get { return acdt; }
            set { acdt = value; }
        }
        private string wc;

        public string Wc
        {
            get { return wc; }
            set { wc = value; }
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
    public class bzglideControl
    {
        public bool writeBzGlideIni(bzglide glidedata)
        {
            try
            {
                if (File.Exists("C:/jcdatatxt/BzGlide.ini"))
                {
                    File.Delete("C:/jcdatatxt/BzGlide.ini");
                }
                //configInfdata preConfigData = getConfigIni();
                ini.INIIO.WritePrivateProfileString("标定数据", "滑行区间", glidedata.Hxqj, "C:/jcdatatxt/BzGlide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "CCDT", glidedata.Ccdt, "C:/jcdatatxt/BzGlide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "ACDT", glidedata.Acdt, "C:/jcdatatxt/BzGlide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "误差", glidedata.Wc, "C:/jcdatatxt/BzGlide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "备注说明", glidedata.Bzsm, "C:/jcdatatxt/BzGlide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "标定结果", glidedata.Bdjg, "C:/jcdatatxt/BzGlide.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
