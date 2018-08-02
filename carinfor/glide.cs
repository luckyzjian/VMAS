using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace carinfor
{
    public class glide
    {
        private string hxqj;

        public string Hxqj
        {
            get { return hxqj; }
            set { hxqj = value; }
        }
        private string qjmysd;

        public string Qjmysd
        {
            get { return qjmysd; }
            set { qjmysd = value; }
        }
        private string plhp;

        public string Plhp
        {
            get { return plhp; }
            set { plhp = value; }
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
        private string jzsdgl;

        public string Jzsdgl
        {
            get { return jzsdgl; }
            set { jzsdgl = value; }
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

        private string gxdl;
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

        public string Acdt40
        {
            get
            {
                return acdt40;
            }

            set
            {
                acdt40 = value;
            }
        }

        public string Acdt24
        {
            get
            {
                return acdt24;
            }

            set
            {
                acdt24 = value;
            }
        }

        public string Ccdt40
        {
            get
            {
                return ccdt40;
            }

            set
            {
                ccdt40 = value;
            }
        }

        public string Ccdt24
        {
            get
            {
                return ccdt24;
            }

            set
            {
                ccdt24 = value;
            }
        }

        public string Ihp40
        {
            get
            {
                return ihp40;
            }

            set
            {
                ihp40 = value;
            }
        }

        public string Ihp24
        {
            get
            {
                return ihp24;
            }

            set
            {
                ihp24 = value;
            }
        }

        public string Result40
        {
            get
            {
                return result40;
            }

            set
            {
                result40 = value;
            }
        }

        public string Result24
        {
            get
            {
                return result24;
            }

            set
            {
                result24 = value;
            }
        }

        public string Starttime
        {
            get
            {
                return starttime;
            }

            set
            {
                starttime = value;
            }
        }

        public string Plhp40
        {
            get
            {
                return plhp40;
            }

            set
            {
                plhp40 = value;
            }
        }

        public string Plhp24
        {
            get
            {
                return plhp24;
            }

            set
            {
                plhp24 = value;
            }
        }

        public string NeuFinished
        {
            get
            {
                return neuFinished;
            }

            set
            {
                neuFinished = value;
            }
        }

        private string acdt40;
        private string acdt24;
        private string ccdt40;
        private string ccdt24;
        private string ihp40;
        private string ihp24;
        private string plhp40;
        private string plhp24;
        private string result40;
        private string result24;
        private string starttime;
        private string neuFinished;
    }
    public class glideControl
    {
        public bool writeGlideIni(glide glidedata)
        {
            try
            {
                if (File.Exists("C:/jcdatatxt/Glide.ini"))
                {
                    File.Delete("C:/jcdatatxt/Glide.ini");
                }
                //configInfdata preConfigData = getConfigIni();
                ini.INIIO.WritePrivateProfileString("标定数据", "滑行区间", glidedata.Hxqj, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "区间名义速度", glidedata.Qjmysd, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "PLHP", glidedata.Plhp, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "CCDT", glidedata.Ccdt, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "ACDT", glidedata.Acdt, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "GXDL", glidedata.Gxdl, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "误差", glidedata.Wc, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "加载设定功率", glidedata.Jzsdgl, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "备注说明", glidedata.Bzsm, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "标定结果", glidedata.Bdjg, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "ACDT40", glidedata.Acdt40, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "ACDT24", glidedata.Acdt24, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "CCDT40", glidedata.Ccdt40, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "CCDT24", glidedata.Ccdt24, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "PLHP40", glidedata.Plhp40, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "PLHP24", glidedata.Plhp24, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "IHP40", glidedata.Ihp40, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "IHP24", glidedata.Ihp24, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "RESULT40", glidedata.Result40, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "RESULT24", glidedata.Result24, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "STARTTIME", glidedata.Starttime, "C:/jcdatatxt/Glide.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "NeuFinished", glidedata.NeuFinished, "C:/jcdatatxt/Glide.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
