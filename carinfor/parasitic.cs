using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace carinfor
{
    public class parasitic
    {
        private string sdqj;

        public string Sdqj
        {
            get { return sdqj; }
            set { sdqj = value; }
        }
        private string mysd;

        public string Mysd
        {
            get { return mysd; }
            set { mysd = value; }
        }
        private string hxsj;

        public string Hxsj
        {
            get { return hxsj; }
            set { hxsj = value; }
        }

        private string jsgl;

        public string Jsgl
        {
            get { return jsgl; }
            set { jsgl = value; }
        }
        private string ljhxsj;

        public string Ljhxsj
        {
            get { return ljhxsj; }
            set { ljhxsj = value; }
        }
        private string bz;

        public string Bz
        {
            get { return bz; }
            set { bz = value; }
        }
        private string bdjg;

        public string Bdjg
        {
            get { return bdjg; }
            set { bdjg = value; }
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

        public string Diw
        {
            get
            {
                return diw;
            }

            set
            {
                diw = value;
            }
        }

        private string starttime;
        private string acdt40;
        private string acdt24;
        private string plhp40;
        private string plhp24;
        private string diw;
        public string kssj { set; get; }
        public string jssj { set; get; }
    }
    public class parasiticControl
    {
        public bool writeParasiticIni(parasitic parasiticdata)
        {
            try
            {
                if (File.Exists("C:/jcdatatxt/Parasitic.ini"))
                {
                    File.Delete("C:/jcdatatxt/Parasitic.ini");
                }
                ini.INIIO.WritePrivateProfileString("标定数据", "速度区间", parasiticdata.Sdqj, "C:/jcdatatxt/Parasitic.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "名义速度", parasiticdata.Mysd, "C:/jcdatatxt/Parasitic.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "滑行时间", parasiticdata.Hxsj, "C:/jcdatatxt/Parasitic.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "寄生功率", parasiticdata.Jsgl, "C:/jcdatatxt/Parasitic.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "累计滑行时间", parasiticdata.Ljhxsj, "C:/jcdatatxt/Parasitic.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "备注说明", parasiticdata.Bz, "C:/jcdatatxt/Parasitic.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "标定结果", parasiticdata.Bdjg, "C:/jcdatatxt/Parasitic.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "Starttime", parasiticdata.Starttime, "C:/jcdatatxt/Parasitic.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "Acdt40", parasiticdata.Acdt40, "C:/jcdatatxt/Parasitic.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "Acdt24", parasiticdata.Acdt24, "C:/jcdatatxt/Parasitic.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "Plhp40", parasiticdata.Plhp40, "C:/jcdatatxt/Parasitic.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "Plhp24", parasiticdata.Plhp24, "C:/jcdatatxt/Parasitic.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "Diw", parasiticdata.Diw, "C:/jcdatatxt/Parasitic.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "kssj", parasiticdata.kssj, "C:/jcdatatxt/Parasitic.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "jssj", parasiticdata.jssj, "C:/jcdatatxt/Parasitic.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
