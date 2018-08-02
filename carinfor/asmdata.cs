using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace carinfor
{
    public class asmdata
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
        private string hc5025;

        public string Hc5025
        {
            get { return hc5025; }
            set { hc5025 = value; }
        }

        private string co5025;

        public string Co5025
        {
            get { return co5025; }
            set { co5025 = value; }
        }
        private string no5025;

        public string No5025
        {
            get { return no5025; }
            set { no5025 = value; }
        }
        private string hc2540;

        public string Hc2540
        {
            get { return hc2540; }
            set { hc2540 = value; }
        }
        private string co2540;

        public string Co2540
        {
            get { return co2540; }
            set { co2540 = value; }
        }
        private string no2540;

        public string No2540
        {
            get { return no2540; }
            set { no2540 = value; }
        }
        private string hc5025pd;

        public string Hc5025pd
        {
            get { return hc5025pd; }
            set { hc5025pd = value; }
        }

        private string co5025pd;

        public string Co5025pd
        {
            get { return co5025pd; }
            set { co5025pd = value; }
        }
        private string no5025pd;

        public string No5025pd
        {
            get { return no5025pd; }
            set { no5025pd = value; }
        }
        private string hc2540pd;

        public string Hc2540pd
        {
            get { return hc2540pd; }
            set { hc2540pd = value; }
        }
        private string co2540pd;

        public string Co2540pd
        {
            get { return co2540pd; }
            set { co2540pd = value; }
        }
        private string no2540pd;

        public string No2540pd
        {
            get { return no2540pd; }
            set { no2540pd = value; }
        }
        private string result;

        public string RESULT
        {
            get { return result; }
            set { result = value; }
        }

        private string djccyString;

        public string DjccyString
        {
            get { return djccyString; }
            set { djccyString = value; }
        }
        private string sjxlString;

        public string SjxlString
        {
            get { return sjxlString; }
            set { sjxlString = value; }
        }
        private string hcclzString;

        public string HcclzString
        {
            get { return hcclzString; }
            set { hcclzString = value; }
        }
        private string noclzString;

        public string NoclzString
        {
            get { return noclzString; }
            set { noclzString = value; }
        }
        private string coclzString;

        public string CoclzString
        {
            get { return coclzString; }
            set { coclzString = value; }
        }
        private string co2clzString;

        public string Co2clzString
        {
            get { return co2clzString; }
            set { co2clzString = value; }
        }
        private string o2clzString;

        public string O2clzString
        {
            get { return o2clzString; }
            set { o2clzString = value; }
        }
        private string csString;

        public string CsString
        {
            get { return csString; }
            set { csString = value; }
        }
        private string zsString;

        public string ZsString
        {
            get { return zsString; }
            set { zsString = value; }
        }
        private string xsxzxsString;

        public string XsxzxsString
        {
            get { return xsxzxsString; }
            set { xsxzxsString = value; }
        }
        private string sdxzxsString;

        public string SdxzxsString
        {
            get { return sdxzxsString; }
            set { sdxzxsString = value; }
        }
        private string jsglString;

        public string JsglString
        {
            get { return jsglString; }
            set { jsglString = value; }
        }
        private string zsglString;

        public string ZsglString
        {
            get { return zsglString; }
            set { zsglString = value; }
        }
        private string hjwdString;

        public string HjwdString
        {
            get { return hjwdString; }
            set { hjwdString = value; }
        }
        private string dqylString;

        public string DqylString
        {
            get { return dqylString; }
            set { dqylString = value; }
        }
        private string xdsdString;

        public string XdsdString
        {
            get { return xdsdString; }
            set { xdsdString = value; }
        }
        private string ywString;

        public string YwString
        {
            get { return ywString; }
            set { ywString = value; }
        }
        private string jcztString;

        public string JcztString
        {
            get { return jcztString; }
            set { jcztString = value; }
        }
        private string kstg5025;

        public string Kstg5025
        {
            get { return kstg5025; }
            set { kstg5025 = value; }
        }
        private string kstg2540;

        public string Kstg2540
        {
            get { return kstg2540; }
            set { kstg2540 = value; }
        }

        private string ambientCO;

        public string AmbientCO
        {
            get { return ambientCO; }
            set { ambientCO = value; }
        }
        private string ambientCO2;

        public string AmbientCO2
        {
            get { return ambientCO2; }
            set { ambientCO2 = value; }
        }
        private string ambientHC;

        public string AmbientHC
        {
            get { return ambientHC; }
            set { ambientHC = value; }
        }
        private string ambientNO;

        public string AmbientNO
        {
            get { return ambientNO; }
            set { ambientNO = value; }
        }
        private string ambientO2;

        public string AmbientO2
        {
            get { return ambientO2; }
            set { ambientO2 = value; }
        }
        private string backgroundCO;

        public string BackgroundCO
        {
            get { return backgroundCO; }
            set { backgroundCO = value; }
        }
        private string backgroundCO2;

        public string BackgroundCO2
        {
            get { return backgroundCO2; }
            set { backgroundCO2 = value; }
        }
        private string backgroundHC;

        public string BackgroundHC
        {
            get { return backgroundHC; }
            set { backgroundHC = value; }
        }
        private string backgroundNO;

        public string BackgroundNO
        {
            get { return backgroundNO; }
            set { backgroundNO = value; }
        }
        private string backgroundO2;

        public string BackgroundO2
        {
            get { return backgroundO2; }
            set { backgroundO2 = value; }
        }
        private string residualHC;

        public string ResidualHC
        {
            get { return residualHC; }
            set { residualHC = value; }
        }
        private string startTime;

        public string StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        private string has5025Tested;

        public string Has5025Tested
        {
            get { return has5025Tested; }
            set { has5025Tested = value; }
        }
        private string has2540Tested;

        public string Has2540Tested
        {
            get { return has2540Tested; }
            set { has2540Tested = value; }
        }
        private string stopReason;

        public string StopReason
        {
            get { return stopReason; }
            set { stopReason = value; }
        }

        private string power5025;

        public string Power5025
        {
            get { return power5025; }
            set { power5025 = value; }
        }
        private string rev5025;

        public string Rev5025
        {
            get { return rev5025; }
            set { rev5025 = value; }
        }
        private string lambda5025;

        public string Lambda5025
        {
            get { return lambda5025; }
            set { lambda5025 = value; }
        }
        private string power2540;

        public string Power2540
        {
            get { return power2540; }
            set { power2540 = value; }
        }
        private string rev2540;

        public string Rev2540
        {
            get { return rev2540; }
            set { rev2540 = value; }
        }
        private string lambda2540;

        public string Lambda2540
        {
            get { return lambda2540; }
            set { lambda2540 = value; }
        }
    }
    public class asmdataControl
    {
        public bool writeAsmData(asmdata asm_data)
        {
            try
            {
                if (File.Exists("C:/jcdatatxt/" + asm_data.CarID + ".ini"))
                {
                    File.Delete("C:/jcdatatxt/" + asm_data.CarID + ".ini");
                }
                ini.INIIO.WritePrivateProfileString("检测结果", "车辆ID", asm_data.CarID, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "相对湿度", asm_data.Sd, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "环境温度", asm_data.Wd, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "大气压力", asm_data.Dqy, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值1", asm_data.Hc5025, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值2", asm_data.Co5025, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值3", asm_data.No5025, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值4", asm_data.Hc2540, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值5", asm_data.Co2540, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值6", asm_data.No2540, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "判定1", asm_data.Hc5025pd, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "判定2", asm_data.Co5025pd, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "判定3", asm_data.No5025pd, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "判定4", asm_data.Hc2540pd, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "判定5", asm_data.Co2540pd, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "判定6", asm_data.No2540pd, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据1", asm_data.DjccyString, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据2", asm_data.SjxlString, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据3", asm_data.HcclzString, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据4", asm_data.NoclzString, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据5", asm_data.CoclzString, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据6", asm_data.Co2clzString, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据7", asm_data.O2clzString, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据8", asm_data.CsString, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据9", asm_data.ZsString, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据10", asm_data.XsxzxsString, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据11", asm_data.SdxzxsString, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据12", asm_data.JsglString, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据13", asm_data.ZsglString, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据14", asm_data.HjwdString, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据15", asm_data.DqylString, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据16", asm_data.XdsdString, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据17", asm_data.YwString, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据18", asm_data.JcztString, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据19", asm_data.Kstg5025, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数据20", asm_data.Kstg2540, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "裁决", asm_data.RESULT, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "环境CO", asm_data.AmbientCO, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "环境CO2", asm_data.AmbientCO2, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "环境HC", asm_data.AmbientHC, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "环境NO", asm_data.AmbientNO, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "环境O2", asm_data.AmbientO2, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "背景CO", asm_data.BackgroundCO, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "背景CO2", asm_data.BackgroundCO2, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "背景HC", asm_data.BackgroundHC, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "背景NO", asm_data.BackgroundNO, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "背景O2", asm_data.BackgroundO2, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "残留HC", asm_data.ResidualHC, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "5025加载功率", asm_data.Power5025, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "5025转速结果", asm_data.Rev5025, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "5025lambda值", asm_data.Lambda5025, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "2540加载功率", asm_data.Power2540, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "2540转速结果", asm_data.Rev2540, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "2540lambda值", asm_data.Lambda2540, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "5025工况", asm_data.Has5025Tested, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "2540工况", asm_data.Has2540Tested, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "中止原因", asm_data.StopReason, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "开始时间", asm_data.StartTime, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "裁决", asm_data.RESULT, "C:/jcdatatxt/" + asm_data.CarID + ".ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public asmdata readAsmData(string filePath)
        {
            asmdata asm_data = new asmdata();
            try
            {
                StringBuilder temp = new StringBuilder();
                temp.Length = 2048;
                if (File.Exists(filePath))
                {
                    ini.INIIO.GetPrivateProfileString("检测结果", "车辆ID", "", temp, 2048, filePath);
                    asm_data.CarID = temp.ToString();
                    ini.INIIO.GetPrivateProfileString("检测结果", "相对湿度", "", temp, 2048, filePath);
                    asm_data.Sd = temp.ToString();
                    ini.INIIO.GetPrivateProfileString("检测结果", "环境温度", "", temp, 2048, filePath);
                    asm_data.Wd = temp.ToString();
                    ini.INIIO.GetPrivateProfileString("检测结果", "大气压力", "", temp, 2048, filePath);
                    asm_data.Dqy = temp.ToString();
                    ini.INIIO.GetPrivateProfileString("检测结果", "数值1", "", temp, 2048, filePath);
                    asm_data.Hc5025 = temp.ToString();
                    ini.INIIO.GetPrivateProfileString("检测结果", "数值2", "", temp, 2048, filePath);
                    asm_data.Co5025 = temp.ToString();
                    ini.INIIO.GetPrivateProfileString("检测结果", "数值3", "", temp, 2048, filePath);
                    asm_data.No5025 = temp.ToString();
                    ini.INIIO.GetPrivateProfileString("检测结果", "数值4", "", temp, 2048, filePath);
                    asm_data.Hc2540 = temp.ToString();
                    ini.INIIO.GetPrivateProfileString("检测结果", "数值5", "", temp, 2048, filePath);
                    asm_data.Co2540 = temp.ToString();
                    ini.INIIO.GetPrivateProfileString("检测结果", "数值6", "", temp, 2048, filePath);
                    asm_data.No2540 = temp.ToString();
                    ini.INIIO.GetPrivateProfileString("检测结果", "判定1", "", temp, 2048, filePath);
                    asm_data.Hc5025pd = temp.ToString();
                    ini.INIIO.GetPrivateProfileString("检测结果", "判定2", "", temp, 2048, filePath);
                    asm_data.Co5025pd = temp.ToString();
                    ini.INIIO.GetPrivateProfileString("检测结果", "判定3", "", temp, 2048, filePath);
                    asm_data.No5025pd = temp.ToString();
                    ini.INIIO.GetPrivateProfileString("检测结果", "判定4", "", temp, 2048, filePath);
                    asm_data.Hc2540pd = temp.ToString();
                    ini.INIIO.GetPrivateProfileString("检测结果", "判定5", "", temp, 2048, filePath);
                    asm_data.Co2540pd = temp.ToString();
                    ini.INIIO.GetPrivateProfileString("检测结果", "判定6", "", temp, 2048, filePath);
                    asm_data.No2540pd = temp.ToString();
                    ini.INIIO.GetPrivateProfileString("检测结果", "裁决", "", temp, 2048, filePath);
                    asm_data.RESULT = temp.ToString();

                }
                else
                {
                    asm_data.CarID = "-1";
                }
                return asm_data;
            }
            catch
            {
                asm_data.CarID = "-1";
                return asm_data;
            }
        }
    }
}
