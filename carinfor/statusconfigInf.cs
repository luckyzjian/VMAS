using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace carinfor
{
    public class statusconfigInfdata
    {
        private string ntype;

        public string nType
        {
            get { return ntype; }
            set { ntype = value; }
        }
        private string sjxl;

        public string SJXL
        {
            get { return sjxl; }
            set { sjxl = value; }
        }


    }
    public class statusconfigIni
    {
        char[] statusString = { 'H', 'W', 'L', 'D', 'T', 'Z', 'I', 'A', 'U', 'P', 'F', 'C', 'V', 'L', 'E', 'J','K' };
        public enum EQUIPMENTSTATUS : byte { YURE = 0, KONGXIAN = 1, DAOWEI = 2, XIAJIANG = 3, JIANCEZHONG = 4, JIEZHUANSU = 5, CHATANTOU = 6, JIASU = 7, JUSHENG = 8, GUOCHE = 9, JIANCESHIBAI = 10, BIAODING = 11, ZIJIAN = 12, SUOZHI = 13, GUZHANG = 14, BIAODINGJIESHU = 15, ZIJIANJIESHU = 15 };
        string[] glStatusString = { "daowei", "tantou", "startsample", "endsample" };
        public enum ENUM_GL_STATUS { STATUS_DAOWEI,STATUS_TANTOU,STATUS_STARTSAMPLE,STATUS_ENDSAMPLE};
        public statusconfigInfdata getGlConfigIni()
        {
            int b = 0;
            statusconfigInfdata configinidata = new statusconfigInfdata();
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            ini.INIIO.GetPrivateProfileString("检测设备", "设备状态", "", temp, 2048, "C://jcdatatxt/glstatusConfig.ini");
            configinidata.nType = temp.ToString();
            ini.INIIO.GetPrivateProfileString("检测设备", "时间序列", "", temp, 2048, "C://jcdatatxt/glstatusConfig.ini");
            configinidata.SJXL = temp.ToString();
            return configinidata;
        }
        public bool writeGlStatusData(ENUM_GL_STATUS equipmentstatus, string sjxl)
        {
            try
            {
                if (File.Exists("C://jcdatatxt/glstatusConfig.ini"))
                {
                    File.Delete("C://jcdatatxt/glstatusConfig.ini");
                }
                ini.INIIO.WritePrivateProfileString("检测设备", "设备状态", glStatusString[(int)equipmentstatus], "C://jcdatatxt/glstatusConfig.ini");
                ini.INIIO.WritePrivateProfileString("检测设备", "时间序列", sjxl, "C://jcdatatxt/glstatusConfig.ini");
                return true;
            }
            catch
            {
                return false;
            }

        }
        public statusconfigInfdata getConfigIni()
        {
            int b = 0;
            statusconfigInfdata configinidata = new statusconfigInfdata();
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            ini.INIIO.GetPrivateProfileString("检测设备", "设备状态", "", temp, 2048, "C://jcdatatxt/statusConfig.ini");
            configinidata.nType = temp.ToString();
            ini.INIIO.GetPrivateProfileString("检测设备", "时间序列", "", temp, 2048, "C://jcdatatxt/statusConfig.ini");
            configinidata.SJXL = temp.ToString();
            return configinidata;
        }
        public bool writeStatusData(EQUIPMENTSTATUS equipmentstatus,string sjxl)
        {
            try
            {
                if (File.Exists("C://jcdatatxt/statusConfig.ini"))
                {
                    File.Delete("C://jcdatatxt/statusConfig.ini");
                }
                ini.INIIO.WritePrivateProfileString("检测设备", "设备状态",statusString[(int)equipmentstatus].ToString(), "C://jcdatatxt/statusConfig.ini");
                ini.INIIO.WritePrivateProfileString("检测设备", "时间序列", sjxl, "C://jcdatatxt/statusConfig.ini");
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool writeNeuStatusData(string equipmentstatus, string sjxl)
        {
            try
            {
                if (File.Exists("C://jcdatatxt/neustatusConfig.ini"))
                {
                    File.Delete("C://jcdatatxt/neustatusConfig.ini");
                }
                ini.INIIO.WritePrivateProfileString("检测设备", "设备状态", equipmentstatus, "C://jcdatatxt/neustatusConfig.ini");
                ini.INIIO.WritePrivateProfileString("检测设备", "时间序列", sjxl, "C://jcdatatxt/neustatusConfig.ini");
                return true;
            }
            catch
            {
                return false;
            }

        }

    }

}
