using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ini;

namespace carinfor
{
    public class analysismeterInidata
    {
        private float co2bz;//车辆ID

        public float Co2bz
        {
            get { return co2bz; }
            set { co2bz = value; }
        }
        private float co2clz;

        public float Co2clz
        {
            get { return co2clz; }
            set { co2clz = value; }
        }
        private float cobz;

        public float Cobz
        {
            get { return cobz; }
            set { cobz = value; }
        }
        private float coclz;

        public float Coclz
        {
            get { return coclz; }
            set { coclz = value; }
        }
        private float hcbz;

        public float Hcbz
        {
            get { return hcbz; }
            set { hcbz = value; }
        }
        private float hcclz;

        public float Hcclz
        {
            get { return hcclz; }
            set { hcclz = value; }
        }
        private float nobz;

        public float Nobz
        {
            get { return nobz; }
            set { nobz = value; }
        }
        private float noclz;

        public float Noclz
        {
            get { return noclz; }
            set { noclz = value; }
        }
        private int jzds;

        public int Jzds
        {
            get { return jzds; }
            set { jzds = value; }
        }
        private string gdjz;

        public string Gdjz
        {
            get { return gdjz; }
            set { gdjz = value; }
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

        public string Hcpc
        {
            get
            {
                return hcpc;
            }

            set
            {
                hcpc = value;
            }
        }

        public string Copc
        {
            get
            {
                return copc;
            }

            set
            {
                copc = value;
            }
        }

        public string Co2pc
        {
            get
            {
                return co2pc;
            }

            set
            {
                co2pc = value;
            }
        }

        public string Nopc
        {
            get
            {
                return nopc;
            }

            set
            {
                nopc = value;
            }
        }

        private string hcpc;
        private string copc;
        private string co2pc;
        private string nopc;


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

        public string Pef
        {
            get
            {
                return pef;
            }

            set
            {
                pef = value;
            }
        }

        private string starttime;
        private string pef;
        public string c3h8 { set; get; }
        public string coabswc { set; get; }
        public string co2abswc { set; get; }
        public string hcabswc { set; get; }
        public string noabswc { set; get; }
    }
    public class analysismeterIni
    {
        public bool writeanalysismeterIni(analysismeterInidata analysismeterdata)
        {
            try
            {
                ini.INIIO.WritePrivateProfileString("标定数据", "CO2标值", analysismeterdata.Co2bz.ToString("0.00"), "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "CO2测量值", analysismeterdata.Co2clz.ToString("0.00"), "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "CO标值", analysismeterdata.Cobz.ToString("0.00"), "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "CO测量值", analysismeterdata.Coclz.ToString("0.00"), "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "HC标值", analysismeterdata.Hcbz.ToString("0.00"), "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "HC测量值", analysismeterdata.Hcclz.ToString("0.00"), "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "NO标值", analysismeterdata.Nobz.ToString("0.00"), "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "NO测量值", analysismeterdata.Noclz.ToString("0.00"), "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "CO偏差", analysismeterdata.Copc, "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "CO2偏差", analysismeterdata.Co2pc, "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "HC偏差", analysismeterdata.Hcpc, "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "NO偏差", analysismeterdata.Nopc, "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "校准点数", analysismeterdata.Jzds.ToString("0"), "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "高低校准", analysismeterdata.Gdjz, "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "备注说明", analysismeterdata.Bzsm, "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "标定结果", analysismeterdata.Bdjg, "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "Pef", analysismeterdata.Pef, "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "Starttime", analysismeterdata.Starttime, "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "c3h8", analysismeterdata.c3h8, "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "coabswc", analysismeterdata.coabswc, "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "co2abswc", analysismeterdata.co2abswc, "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "hcabswc", analysismeterdata.hcabswc, "C:/jcdatatxt/AnalysisMeter.ini");
                ini.INIIO.WritePrivateProfileString("标定数据", "noabswc", analysismeterdata.noabswc, "C:/jcdatatxt/AnalysisMeter.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
