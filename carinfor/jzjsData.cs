using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace carinfor
{
    public class jzjsdata
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
        private string gxsxs_100;

        public string Gxsxs_100
        {
            get { return gxsxs_100; }
            set { gxsxs_100 = value; }
        }
        private string gxsxs_90;

        public string Gxsxs_90
        {
            get { return gxsxs_90; }
            set { gxsxs_90 = value; }
        }
        private string gxsxs_80;

        public string Gxsxs_80
        {
            get { return gxsxs_80; }
            set { gxsxs_80 = value; }
        }
        private string lbgl;
        /// <summary>
        /// 修正最大轮边功率
        /// </summary>
        public string Lbgl
        {
            get { return lbgl; }
            set { lbgl = value; }
        }
        private string lbzs;

        public string Lbzs
        {
            get { return lbzs; }
            set { lbzs = value; }
        }
        private string velmaxhp;
        /// <summary>
        /// 实际MAXHP时滚筒线速度
        /// </summary>
        public string Velmaxhp
        {
            get { return velmaxhp; }
            set { velmaxhp = value; }
        }
        private string velmaxhpzs;
        /// <summary>
        /// 轮边功率转速
        /// </summary>
        public string Velmaxhpzs
        {
            get { return velmaxhpzs; }
            set { velmaxhpzs = value; }
        }
        private string realVelmaxhp;
        /// <summary>
        /// 计算MAXHP时滚筒线速度
        /// </summary>
        public string RealVelmaxhp
        {
            get { return realVelmaxhp; }
            set { realVelmaxhp = value; }
        }
        private string rev100;

        public string Rev100
        {
            get { return rev100; }
            set { rev100 = value; }
        }
        private string startTime;

        public string StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        /// <summary>
        /// 功率修正系数
        /// </summary>
        public string glxzxs { set; get; }
        /// <summary>
        /// 实测最大轮边功率
        /// </summary>
        public string actualmaxhp { set; get; }
        private string stopReason;

        public string StopReason
        {
            get { return stopReason; }
            set { stopReason = value; }
        }
        public string hno { set; get; }
        public string nno { set; get; }
        public string eno { set; get; }
    }
    public class jzjsdataControl
    {
        public bool writeJzjsData(jzjsdata jzjs_data)
        {
            
            try
            {
                if (File.Exists("C:/jcdatatxt/" + jzjs_data.CarID + ".ini"))
                {
                    File.Delete("C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                }
                ini.INIIO.WritePrivateProfileString("检测结果", "车辆ID", jzjs_data.CarID, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "相对湿度", jzjs_data.Sd, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "环境温度", jzjs_data.Wd, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "大气压力", jzjs_data.Dqy, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值1", jzjs_data.Gxsxs_100, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值2", jzjs_data.Gxsxs_90, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值3", jzjs_data.Gxsxs_80, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值4", jzjs_data.Lbgl, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "数值5", jzjs_data.Lbzs, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "actualmaxhp", jzjs_data.actualmaxhp, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "VELMAXHP", jzjs_data.Velmaxhp, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "REALVELMAXHP", jzjs_data.RealVelmaxhp, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "VELMAXHPZS", jzjs_data.Velmaxhpzs, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "GLXZXS", jzjs_data.glxzxs, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "Rev100", jzjs_data.Rev100, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "StartTime", jzjs_data.StartTime, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "StopReason", jzjs_data.StopReason, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "hno", jzjs_data.hno, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "nno", jzjs_data.nno, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "eno", jzjs_data.eno, "C:/jcdatatxt/" + jzjs_data.CarID + ".ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
