using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ini;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace carinfor
{
    public class yhcarInidata
    {
        public yhcarInidata()
        {
            检测流水号 = "";
            检测类型 = 0;
            动力检测类型 = 0;
            车辆号牌 = "";
            号牌种类 = "";
            车辆型号 = "";
            发动机型号 = "";
            油耗限值依据 = 0;
            油耗限值 = 0;
            油耗检测加载力 = 0;
            油耗检测工况速度 = 0;
            燃料种类 = 0;
            子午胎轮胎断面宽度 = 0;
            汽车前轮距 = 0;
            汽车高度 = 0;
            客车车长 = 0;
            客车等级 = 0;
            汽车类型 = 0;
            货车车身型式 = 0;
            额定总质量 = 0;
            是否危险货物运输车辆 = 0;
            动力性检测加载力 = 0;
            加载力计算方式 = 1;
            驱动轴数 = 1;
            轮胎类型 = 0;
            压燃式功率参数类型 = 0;
            压燃式额定功率 = 0;
            点燃式额定扭矩 = 0;
            驱动轴空载质量 = 0;
            牵引车满载总质量 = 0;
            加载力比例 = 1;
            驱动轴质量方式 = 0;
            油耗车速方式 = 0;
            是否检测动力性 = false;
            是否检测油耗 = false;
            是否检测车速 = false;
        }
        public string 检测流水号 { set; get; }
        /// <summary>
        ///调用动力性检测时用 0:只检测动力性 1：只检测车速 2：车速与动力性均检测
        /// </summary>
        public int 检测类型 { set; get; }
        public string 检测性质 { set; get; }

        /// <summary>
        /// 0-达标检测 1-自动进行等级评定 2-单做二级评定 3-单做一级评定
        /// </summary>
        public int 动力检测类型 { set; get; }
        public string 车辆号牌 { set; get; }
        public string 号牌种类 { set; get; }

        public string 车辆型号 { set; get; }
        public string 发动机型号 { set; get; }

        //油耗定义
        /// <summary>
        /// 0:子程序根据车型判断 1:由“油耗限值”指定
        /// </summary>
        public int 油耗限值依据 { set; get; }
        public double 油耗限值 { set; get; }
        public double 油耗检测加载力 { set; get; }
        public double 油耗检测工况速度 { set; get; }
        /// <summary>
        /// 0:汽油 1：柴油
        /// </summary>
        public int 燃料种类 { set; get; }
        /// <summary>
        /// 单位：in
        /// </summary>
        public double 子午胎轮胎断面宽度 { set; get; }
        /// <summary>
        /// 单位：mm
        /// </summary>
        public double 汽车前轮距 { set; get; }
        /// <summary>
        /// 单位：mm
        /// </summary>
        public double 汽车高度 { set; get; }
        public double 客车车长 { set; get; }
        /// <summary>
        /// 0:高级 1：中级 2：普通级
        /// </summary>
        public int 客车等级 { set; get; }
        /// <summary>
        /// 0:营运客车  1:营运货车
        /// </summary>
        public int 汽车类型 { set; get; }

        /// <summary>
        /// 0-拦板车 1-自卸车 2-牵引车 3-仓栅车 4-厢式车 5-罐车
        /// </summary>
        public int 货车车身型式 { set; get; }
        public double 额定总质量 { set; get; }
        //功率定义
        /// <summary>
        /// 0:否 1：是
        /// </summary>
        public int 是否危险货物运输车辆 { set; get; } //是否危险品

        public double 动力性检测加载力 { set; get; }
        /// <summary>
        /// 0:上层传入加载力值  1:根据车辆信息计算加载力
        /// </summary>
        public int 加载力计算方式 { set; get; }
        /// <summary>
        /// 1:单驱动轴 2;双驱动轴
        /// </summary>
        public int 驱动轴数 { set; get; }
        /// <summary>
        /// 0:子午线轮胎  1:斜交轮胎
        /// </summary>
        public int 轮胎类型 { set; get; }

        /// <summary>
        /// 0:发动机铭牌以额定功率表示 1:发动机铭牌以最大净功率表征
        /// </summary>
        public int 压燃式功率参数类型 { set; get; }
        /// <summary>
        /// 压燃式发动机需要
        /// </summary>
        public double 压燃式额定功率 { set; get; }
        /// <summary>
        /// 点燃式发动机需要
        /// </summary>
        public double 点燃式额定扭矩 { set; get; }
        /// <summary>
        /// 点燃式发动机需要,当额定扭矩转速为nm1~nm2时，取其均值
        /// </summary>
        public double 点燃式额定扭矩转速 { set; get; }

        public double 驱动轴空载质量 { set; get; }
        public double 牵引车满载总质量 { set; get; }
        public double 加载力比例 { set; get; }
        /// <summary>
        /// 驱动轴重量方式 0：文本文件传入 1：称重台称重
        /// </summary>
        public int 驱动轴质量方式 { set; get; }
        /// <summary>
        /// 油耗车速方式 0：子程序自动根据车辆信息确定 1：由文本文件传入
        /// </summary>
        public int 油耗车速方式 { set; get; }
        public bool 是否检测动力性 { set; get; }
        public bool 是否检测车速 { set; get; }
        public bool 是否检测油耗 { set; get; }
    }
    public class fuelResult
    {
        public fuelResult()
        {
            this.速度曲线 = "";
            this.扭力曲线 = "";
            this.功率曲线 = "";
            this.油耗曲线 = "";
        }

        public double 检测速度 { set; get; }
        public double 台架加载阻力 { set; get; }
        public double 燃料总消耗量 { set; get; }
        public double 总行驶里程 { set; get; }
        public double 百公里燃料消耗量 { set; get; }
        public double 限值 { set; get; }
        /// <summary>
        /// 0-列入交通运输主管部门公布的车辆 1-未列入交通运输主管部门公布的车辆 2-牵引车按牵引车满载总质量进行检测时
        /// </summary>
        public int 限值依据 { set; get; }
        /// <summary>
        /// 0-合格 1-不合格
        /// </summary>
        public int 判定结果 { set; get; }
        public double 汽车滚动阻力 { set; get; }
        public double 空气阻力 { set; get; }
        public double 滚动阻力系数 { set; get; }
        public double 迎风面积 { set; get; }
        public double 空气阻力系数 { set; get; }
        public double 台架运转阻力 { set; get; }
        public double 台架滚动阻力 { set; get; }
        public double 台架滚动阻力系数 { set; get; }
        public double 台架内阻 { set; get; }
        public string 速度曲线 { set; get; }
        public string 扭力曲线 { set; get; }
        public string 功率曲线 { set; get; }
        public string 油耗曲线 { set; get; }
    }
    public class DieselDynamicResult
    {
        public DieselDynamicResult()
        {
            this.速度曲线 = "";
            this.扭力曲线 = "";
            this.功率曲线 = "";
        }
        public int 检测类型 { set; get; }
        public double 车速 { set; get; }
        public int 车速判定结果 { set; get; }
        public double 功率比值系数 { set; get; }//η
        public double 额定功率 { set; get; }//Pe
        public double 额定功率车速 { set; get; }//Ve
        public double 稳定车速 { set; get; }//Vw
        public double 台架滚动阻力 { set; get; }//Fe
        public double 加载力 { set; get; }//FE
        public double 测功机内阻 { set; get; }//Ftc
        public double 轮胎滚动阻力 { set; get; }//Fc
        public double 发动机附件阻力 { set; get; }//Ff
        public double 车辆传动系允许阻力 { set; get; }//Ft
        public double 功率校正系数 { set; get; }//αd
        public double 台架滚动阻力系数 { set; get; }//fc
        public double 温度 { set; get; }
        public double 湿度 { set; get; }
        public double 大气压 { set; get; }
        public int 判定结果 { set; get; }
        /// <summary>
        /// 0-未评定 1-1级 2-2级
        /// </summary>
        public int 等级评定结果 { set; get; }
        public string 速度曲线 { set; get; }
        public string 扭力曲线 { set; get; }
        public string 功率曲线 { set; get; }
    }
    public class GasolineDynamicResult
    {
        public GasolineDynamicResult()
        {
            this.速度曲线 = "";
            this.扭力曲线 = "";
            this.功率曲线 = "";
        }
        public int 检测类型 { set; get; }
        public double 车速 { set; get; }
        public int 车速判定结果 { set; get; }
        public double 功率比值系数 { set; get; }//η
        public double 发动机额定扭矩 { set; get; }//Mm
        public double 额定扭矩车速 { set; get; }//Vm
        public double 额定扭矩转速 { set; get; }//nm
        public double 稳定车速 { set; get; }//Vw
        public double 加载力 { set; get; }//FM
        public double 发动机达标扭矩驱动力 { set; get; }//Fm
        public double 测功机内阻 { set; get; }//Ftc
        public double 轮胎滚动阻力 { set; get; }//Fc
        public double 发动机附件阻力 { set; get; }//Ff
        public double 车辆传动系允许阻力 { set; get; }//Ft
        public double 功率校正系数 { set; get; }//αa
        public double 台架滚动阻力系数 { set; get; }//fc
        public double 温度 { set; get; }
        public double 湿度 { set; get; }
        public double 大气压 { set; get; }
        /// <summary>
        /// 0-合格 1-不合格
        /// </summary>
        public int 判定结果 { set; get; }
        /// <summary>
        /// 0-未评定 1-1级 2-2级
        /// </summary>
        public int 等级评定结果 { set; get; }
        public string 速度曲线 { set; get; }
        public string 扭力曲线 { set; get; }
        public string 功率曲线 { set; get; }
    }
    public class SpeedResult
    {
        public double 车速 { set; get; }
        public int 车速判定结果 { set; get; }
    }
    public class QdzzlResult
    {
        public double 驱动轴空载质量 { set; get; }
    }
    public class carInidata
    {
        private string carID;//车辆ID

        public string CarID
        {
            get { return carID; }
            set { carID = value; }
        }
        private string carPH;//车辆牌号

        public string CarPH
        {
            get { return carPH; }
            set { carPH = value; }
        }
        public int jqfs { set; get; }
        private float carJzzl;//基准质量

        public float CarJzzl
        {
            get { return carJzzl; }
            set { carJzzl = value; }
        }
        private int carZzl;

        public int CarZzl
        {
            get { return carZzl; }
            set { carZzl = value; }
        }
        private string carRlzl;//燃料类型

        public string CarRlzl
        {
            get { return carRlzl; }
            set { carRlzl = value; }
        }
        private float carEdgl;//额定功率

        public float CarEdgl
        {
            get { return carEdgl; }
            set { carEdgl = value; }
        }
        private float carEdzs;//额定转速

        public float CarEdzs
        {
            get { return carEdzs; }
            set { carEdzs = value; }
        }
        private string carBsxlx;//变速箱类型

        public string CarBsxlx
        {
            get { return carBsxlx; }
            set { carBsxlx = value; }
        }
        private float carLxcc;//连续超差时限

        public float CarLxcc
        {
            get { return carLxcc; }
            set { carLxcc = value; }
        }
        private float carLjcc;//累计超差时限

        public float CarLjcc
        {
            get { return carLjcc; }
            set { carLjcc = value; }
        }
        private float carNdz;//浓度限值

        public float CarNdz
        {
            get { return carNdz; }
            set { carNdz = value; }
        }
        private string carCc;//冲程

        public string CarCc
        {
            get { return carCc; }
            set { carCc = value; }
        }
        private float xz1;//hc 5025

        public float Xz1
        {
            get { return xz1; }
            set { xz1 = value; }
        }
        private float xz2;//co 5025

        public float Xz2
        {
            get { return xz2; }
            set { xz2 = value; }
        }
        private float xz3;//no 5025

        public float Xz3
        {
            get { return xz3; }
            set { xz3 = value; }
        }
        private float xz4;//hc 2540

        public float Xz4
        {
            get { return xz4; }
            set { xz4 = value; }
        }
        private float xz5;//co 2540

        public float Xz5
        {
            get { return xz5; }
            set { xz5 = value; }
        }
        private float xz6;//no 2540

        public float Xz6
        {
            get { return xz6; }
            set { xz6 = value; }
        }

        public double ASM_HC { set; get; }
        public double ASM_CO { set; get; }
        public double ASM_NO { set; get; }
        public double VMAS_HC { set; get; }
        public double VMAS_CO { set; get; }
        public double VMAS_NO { set; get; }
        public double SDS_HC { set; get; }
        public double SDS_CO { set; get; }
        public double ZYJS_K { set; get; }
        public double JZJS_K { set; get; }
        public double JZJS_GL { set; get; }
        public bool ISUSE { set; get; }
        public bool ISMOTO { set; get; }
        public DateTime scrq { set; get; }
    }
    public class carIni
    {
        public carInidata getCarIni()
        {
            float a = 0;
            int b = 0;
            double c = 1;
            DateTime dtime;
            carInidata carinidata = new carInidata();
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            ini.INIIO.GetPrivateProfileString("检测信息", "车辆ID", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            carinidata.CarID = temp.ToString().Trim();
            ini.INIIO.GetPrivateProfileString("检测信息", "车辆牌照号", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            carinidata.CarPH = temp.ToString().Trim();
            ini.INIIO.GetPrivateProfileString("检测信息", "进气方式", "1", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.jqfs = b;
            else
                carinidata.jqfs = 1;
            ini.INIIO.GetPrivateProfileString("检测信息", "生产日期", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (DateTime.TryParse(temp.ToString().Trim(), out dtime))
                carinidata.scrq = dtime;
            else
                carinidata.scrq = DateTime.Now;
            ini.INIIO.GetPrivateProfileString("检测信息", "基准质量", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (float.TryParse(temp.ToString().Trim(), out a))
                carinidata.CarJzzl = a;
            else
                carinidata.CarJzzl = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "总质量", "4000", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.CarZzl = b;
            else
                carinidata.CarZzl = 4000;
            ini.INIIO.GetPrivateProfileString("检测信息", "燃料种类", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            carinidata.CarRlzl = temp.ToString().Trim();
            ini.INIIO.GetPrivateProfileString("检测信息", "额定功率", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (float.TryParse(temp.ToString().Trim(), out a))
                carinidata.CarEdgl = a;
            else
                carinidata.CarEdgl = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "额定转速", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (float.TryParse(temp.ToString().Trim(), out a))
                carinidata.CarEdzs = a;
            else
                carinidata.CarEdzs = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "变速箱", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            carinidata.CarBsxlx = temp.ToString().Trim();
            ini.INIIO.GetPrivateProfileString("检测信息", "连续超差", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (float.TryParse(temp.ToString().Trim(), out a))
                carinidata.CarLxcc = a;
            else
                carinidata.CarLxcc = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "累计超差", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (float.TryParse(temp.ToString().Trim(), out a))
                carinidata.CarLjcc = a;
            else
                carinidata.CarLjcc = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "浓度值", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (float.TryParse(temp.ToString().Trim(), out a))
                carinidata.CarNdz = a;
            else
                carinidata.CarNdz = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "冲程", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            carinidata.CarCc = temp.ToString().Trim();
            ini.INIIO.GetPrivateProfileString("检测信息", "限值1", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (float.TryParse(temp.ToString().Trim(), out a))
                carinidata.Xz1 = a;
            else
                carinidata.Xz1 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "限值2", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (float.TryParse(temp.ToString().Trim(), out a))
                carinidata.Xz2 = a;
            else
                carinidata.Xz2 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "限值3", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (float.TryParse(temp.ToString().Trim(), out a))
                carinidata.Xz3 = a;
            else
                carinidata.Xz3 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "限值4", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (float.TryParse(temp.ToString().Trim(), out a))
                carinidata.Xz4 = a;
            else
                carinidata.Xz4 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "限值5", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (float.TryParse(temp.ToString().Trim(), out a))
                carinidata.Xz5 = a;
            else
                carinidata.Xz5 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "限值6", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (float.TryParse(temp.ToString().Trim(), out a))
                carinidata.Xz6 = a;
            else
                carinidata.Xz6 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "ASM_HC", "1", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (Double.TryParse(temp.ToString().Trim(), out c))
                carinidata.ASM_HC = c;
            else
                carinidata.ASM_HC = 1;
            ini.INIIO.GetPrivateProfileString("检测信息", "ASM_CO", "1", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (Double.TryParse(temp.ToString().Trim(), out c))
                carinidata.ASM_CO = c;
            else
                carinidata.ASM_CO = 1;
            ini.INIIO.GetPrivateProfileString("检测信息", "ASM_NO", "1", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (Double.TryParse(temp.ToString().Trim(), out c))
                carinidata.ASM_NO = c;
            else
                carinidata.ASM_NO = 1;
            ini.INIIO.GetPrivateProfileString("检测信息", "VMAS_HC", "1", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (Double.TryParse(temp.ToString().Trim(), out c))
                carinidata.VMAS_HC = c;
            else
                carinidata.VMAS_HC = 1;
            ini.INIIO.GetPrivateProfileString("检测信息", "VMAS_CO", "1", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (Double.TryParse(temp.ToString().Trim(), out c))
                carinidata.VMAS_CO = c;
            else
                carinidata.VMAS_CO = 1;
            ini.INIIO.GetPrivateProfileString("检测信息", "VMAS_NO", "1", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (Double.TryParse(temp.ToString().Trim(), out c))
                carinidata.VMAS_NO = c;
            else
                carinidata.VMAS_NO = 1;
            ini.INIIO.GetPrivateProfileString("检测信息", "SDS_HC", "1", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (Double.TryParse(temp.ToString().Trim(), out c))
                carinidata.SDS_HC = c;
            else
                carinidata.SDS_HC = 1;
            ini.INIIO.GetPrivateProfileString("检测信息", "SDS_CO", "1", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (Double.TryParse(temp.ToString().Trim(), out c))
                carinidata.SDS_CO = c;
            else
                carinidata.SDS_CO = 1;
            ini.INIIO.GetPrivateProfileString("检测信息", "ZYJS_K", "1", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (Double.TryParse(temp.ToString().Trim(), out c))
                carinidata.ZYJS_K = c;
            else
                carinidata.ZYJS_K = 1;
            ini.INIIO.GetPrivateProfileString("检测信息", "JZJS_K", "1", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (Double.TryParse(temp.ToString().Trim(), out c))
            {
                if (c <= 1.5 && c >= 0.1)
                    carinidata.JZJS_K = c;
                else
                    carinidata.JZJS_K = 1;
            }
            ini.INIIO.GetPrivateProfileString("检测信息", "JZJS_GL", "1", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (Double.TryParse(temp.ToString().Trim(), out c))
            {
                if (c <= 3 && c >= 0.5)
                    carinidata.JZJS_GL = c;
                else
                    carinidata.JZJS_GL = 1;
            }
            else
                carinidata.JZJS_GL = 1;
            ini.INIIO.GetPrivateProfileString("检测信息", "ISUSE", "N", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            carinidata.ISUSE = (temp.ToString().Trim() == "Y");
            ini.INIIO.GetPrivateProfileString("检测信息", "ISMOTO", "N", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            carinidata.ISMOTO = (temp.ToString().Trim() == "Y");
            return carinidata;
        }
        public bool writeCarIni(carInidata carinidata)
        {
            try
            {
                //configInfdata preConfigData = getConfigIni();
                ini.INIIO.WritePrivateProfileString("检测信息", "车辆ID", carinidata.CarID, @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "车辆牌照号", carinidata.CarPH, @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "基准质量", carinidata.CarJzzl.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "燃料种类", carinidata.CarRlzl, @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "额定功率", carinidata.CarEdgl.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "额定转速", carinidata.CarEdzs.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "变速箱", carinidata.CarBsxlx, @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "连续超差", carinidata.CarLxcc.ToString("0.0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "累计超差", carinidata.CarLjcc.ToString("0.0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "浓度值", carinidata.CarNdz.ToString("0.0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "冲程", carinidata.CarCc, @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "限值1", carinidata.Xz1.ToString("0.000"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "限值2", carinidata.Xz2.ToString("0.000"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "限值3", carinidata.Xz3.ToString("0.000"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "限值4", carinidata.Xz4.ToString("0.000"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "限值5", carinidata.Xz5.ToString("0.000"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "限值6", carinidata.Xz6.ToString("0.000"), @"C:\jcdatatxt\carinfo.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool writeYhCarIni(yhcarInidata carinidata)
        {
            try
            {
                //configInfdata preConfigData = getConfigIni();
                ini.INIIO.WritePrivateProfileString("检测信息", "检测流水号", carinidata.检测流水号, @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "检测类型", carinidata.检测类型.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "检测性质", carinidata.检测性质, @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "动力检测类型", carinidata.动力检测类型.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "车辆号牌", carinidata.车辆号牌, @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "号牌种类", carinidata.号牌种类, @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "车辆型号", carinidata.车辆型号, @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "发动机型号", carinidata.发动机型号, @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "油耗限值依据", carinidata.油耗限值依据.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "油耗限值", carinidata.油耗限值.ToString("0.00"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "油耗检测加载力", carinidata.油耗检测加载力.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "油耗检测工况速度", carinidata.油耗检测工况速度.ToString("0.0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "燃料种类", carinidata.燃料种类.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "子午胎轮胎断面宽度", carinidata.子午胎轮胎断面宽度.ToString("0.000"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "汽车前轮距", carinidata.汽车前轮距.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "汽车高度", carinidata.汽车高度.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "客车车长", carinidata.客车车长.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "客车等级", carinidata.客车等级.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "汽车类型", carinidata.汽车类型.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "货车车身型式", carinidata.货车车身型式.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "额定总质量", carinidata.额定总质量.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "是否危险货物运输车辆", carinidata.是否危险货物运输车辆.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "动力性检测加载力", carinidata.动力性检测加载力.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "加载力计算方式", carinidata.加载力计算方式.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "驱动轴数", carinidata.驱动轴数.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "轮胎类型", carinidata.轮胎类型.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "压燃式功率参数类型", carinidata.压燃式功率参数类型.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "压燃式额定功率", carinidata.压燃式额定功率.ToString("0.0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "点燃式额定扭矩", carinidata.点燃式额定扭矩.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "点燃式额定扭矩转速", carinidata.点燃式额定扭矩转速.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "驱动轴空载质量", carinidata.驱动轴空载质量.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "牵引车满载总质量", carinidata.牵引车满载总质量.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "驱动轴质量方式", carinidata.驱动轴质量方式.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "油耗车速方式", carinidata.油耗车速方式.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "是否检测车速", carinidata.是否检测车速 ? "Y" : "N", @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "是否检测动力性", carinidata.是否检测动力性 ? "Y" : "N", @"C:\jcdatatxt\carinfo.ini");
                ini.INIIO.WritePrivateProfileString("检测信息", "是否检测油耗", carinidata.是否检测油耗 ? "Y" : "N", @"C:\jcdatatxt\carinfo.ini");
                //ini.INIIO.WritePrivateProfileString("检测信息", "加载力比例", carinidata.加载力比例.ToString("0"), @"C:\jcdatatxt\carinfo.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public yhcarInidata getYhCarIni()
        {
            double a = 0;
            int b = 0;
            yhcarInidata carinidata = new yhcarInidata();
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;

            ini.INIIO.GetPrivateProfileString("检测信息", "检测流水号", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            carinidata.检测流水号 = temp.ToString().Trim();
            ini.INIIO.GetPrivateProfileString("检测信息", "车辆型号", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            carinidata.车辆型号 = temp.ToString().Trim();
            ini.INIIO.GetPrivateProfileString("检测信息", "发动机型号", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            carinidata.发动机型号 = temp.ToString().Trim();

            ini.INIIO.GetPrivateProfileString("检测信息", "检测类型", "0", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.检测类型 = b;
            else
                carinidata.检测类型 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "检测性质", "等级评定", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            carinidata.检测性质 = temp.ToString().Trim();
            ini.INIIO.GetPrivateProfileString("检测信息", "动力检测类型", "0", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.动力检测类型 = b;
            else
                carinidata.动力检测类型 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "油耗限值依据", "0", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.油耗限值依据 = b;
            else
                carinidata.油耗限值依据 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "油耗限值", "30", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.油耗限值 = a;
            else
                carinidata.油耗限值 = 30;
            ini.INIIO.GetPrivateProfileString("检测信息", "车辆号牌", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            carinidata.车辆号牌 = temp.ToString().Trim();
            ini.INIIO.GetPrivateProfileString("检测信息", "号牌种类", "", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            carinidata.号牌种类 = temp.ToString().Trim();
            ini.INIIO.GetPrivateProfileString("检测信息", "油耗检测加载力", "200", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.油耗检测加载力 = a;
            else
                carinidata.油耗检测加载力 = 200;
            ini.INIIO.GetPrivateProfileString("检测信息", "油耗检测工况速度", "50", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.油耗检测工况速度 = b;
            else
                carinidata.油耗检测工况速度 = 50;
            ini.INIIO.GetPrivateProfileString("检测信息", "燃料种类", "0", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.燃料种类 = b;
            else
                carinidata.燃料种类 = 0;

            ini.INIIO.GetPrivateProfileString("检测信息", "子午胎轮胎断面宽度", "8.25", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.子午胎轮胎断面宽度 = a;
            else
                carinidata.子午胎轮胎断面宽度 = 200;
            ini.INIIO.GetPrivateProfileString("检测信息", "汽车前轮距", "1000", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.汽车前轮距 = a;
            else
                carinidata.汽车前轮距 = 200;
            ini.INIIO.GetPrivateProfileString("检测信息", "汽车高度", "1500", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.汽车高度 = a;
            else
                carinidata.汽车高度 = 1500;
            ini.INIIO.GetPrivateProfileString("检测信息", "客车车长", "5500", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.客车车长 = a;
            else
                carinidata.客车车长 = 5500;

            ini.INIIO.GetPrivateProfileString("检测信息", "客车等级", "0", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.客车等级 = b;
            else
                carinidata.客车等级 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "汽车类型", "0", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.汽车类型 = b;
            else
                carinidata.汽车类型 = 0;

            ini.INIIO.GetPrivateProfileString("检测信息", "货车车身型式", "0", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.货车车身型式 = b;
            else
                carinidata.货车车身型式 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "额定总质量", "3500", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.额定总质量 = a;
            else
                carinidata.额定总质量 = 3500;
            ini.INIIO.GetPrivateProfileString("检测信息", "是否危险货物运输车辆", "0", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.是否危险货物运输车辆 = b;
            else
                carinidata.是否危险货物运输车辆 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "动力性检测加载力", "200", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.动力性检测加载力 = a;
            else
                carinidata.动力性检测加载力 = 200;

            ini.INIIO.GetPrivateProfileString("检测信息", "加载力计算方式", "0", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.加载力计算方式 = b;
            else
                carinidata.加载力计算方式 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "驱动轴数", "1", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.驱动轴数 = b;
            else
                carinidata.驱动轴数 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "轮胎类型", "0", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.轮胎类型 = b;
            else
                carinidata.轮胎类型 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "压燃式功率参数类型", "0", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.压燃式功率参数类型 = b;
            else
                carinidata.压燃式功率参数类型 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "压燃式额定功率", "200", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.压燃式额定功率 = a;
            else
                carinidata.压燃式额定功率 = 200;
            ini.INIIO.GetPrivateProfileString("检测信息", "点燃式额定扭矩", "200", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.点燃式额定扭矩 = a;
            else
                carinidata.点燃式额定扭矩 = 200;
            ini.INIIO.GetPrivateProfileString("检测信息", "点燃式额定扭矩转速", "4000", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.点燃式额定扭矩转速 = a;
            else
                carinidata.点燃式额定扭矩转速 = 4000;
            ini.INIIO.GetPrivateProfileString("检测信息", "驱动轴空载质量", "1000", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.驱动轴空载质量 = a;
            else
                carinidata.驱动轴空载质量 = 1000;
            ini.INIIO.GetPrivateProfileString("检测信息", "牵引车满载总质量", "200", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.牵引车满载总质量 = a;
            else
                carinidata.牵引车满载总质量 = 200;
            ini.INIIO.GetPrivateProfileString("检测信息", "加载力比例", "1", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.加载力比例 = a;
            else
                carinidata.加载力比例 = 1;
            ini.INIIO.GetPrivateProfileString("检测信息", "驱动轴质量方式", "0", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.驱动轴质量方式 = b;
            else
                carinidata.驱动轴质量方式 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "油耗车速方式", "0", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.油耗车速方式 = b;
            else
                carinidata.油耗车速方式 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "是否检测车速", "N", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            carinidata.是否检测车速 = temp.ToString().Trim() == "Y";
            ini.INIIO.GetPrivateProfileString("检测信息", "是否检测动力性", "N", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            carinidata.是否检测动力性 = temp.ToString().Trim() == "Y";
            ini.INIIO.GetPrivateProfileString("检测信息", "是否检测油耗", "N", temp, 2048, @"C:\jcdatatxt\carinfo.ini");
            carinidata.是否检测油耗 = temp.ToString().Trim() == "Y";
            return carinidata;
        }
        public yhcarInidata getYhCarIniXn()
        {
            double a = 0;
            int b = 0;
            yhcarInidata carinidata = new yhcarInidata();
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;

            ini.INIIO.GetPrivateProfileString("检测信息", "检测流水号", "", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            carinidata.检测流水号 = temp.ToString().Trim();
            ini.INIIO.GetPrivateProfileString("检测信息", "车辆型号", "", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            carinidata.车辆型号 = temp.ToString().Trim();
            ini.INIIO.GetPrivateProfileString("检测信息", "发动机型号", "", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            carinidata.发动机型号 = temp.ToString().Trim();

            ini.INIIO.GetPrivateProfileString("检测信息", "检测类型", "0", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.检测类型 = b;
            else
                carinidata.检测类型 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "检测性质", "等级评定", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            carinidata.检测性质 = temp.ToString().Trim();
            ini.INIIO.GetPrivateProfileString("检测信息", "动力检测类型", "0", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.动力检测类型 = b;
            else
                carinidata.动力检测类型 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "油耗限值依据", "0", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.油耗限值依据 = b;
            else
                carinidata.油耗限值依据 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "油耗限值", "30", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.油耗限值 = a;
            else
                carinidata.油耗限值 = 30;
            ini.INIIO.GetPrivateProfileString("检测信息", "车辆号牌", "", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            carinidata.车辆号牌 = temp.ToString().Trim();
            ini.INIIO.GetPrivateProfileString("检测信息", "号牌种类", "", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            carinidata.号牌种类 = temp.ToString().Trim();
            ini.INIIO.GetPrivateProfileString("检测信息", "油耗检测加载力", "200", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.油耗检测加载力 = a;
            else
                carinidata.油耗检测加载力 = 200;
            ini.INIIO.GetPrivateProfileString("检测信息", "油耗检测工况速度", "50", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.油耗检测工况速度 = b;
            else
                carinidata.油耗检测工况速度 = 50;
            ini.INIIO.GetPrivateProfileString("检测信息", "燃料种类", "0", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.燃料种类 = b;
            else
                carinidata.燃料种类 = 0;

            ini.INIIO.GetPrivateProfileString("检测信息", "子午胎轮胎断面宽度", "8.25", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.子午胎轮胎断面宽度 = a;
            else
                carinidata.子午胎轮胎断面宽度 = 200;
            ini.INIIO.GetPrivateProfileString("检测信息", "汽车前轮距", "1000", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.汽车前轮距 = a;
            else
                carinidata.汽车前轮距 = 200;
            ini.INIIO.GetPrivateProfileString("检测信息", "汽车高度", "1500", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.汽车高度 = a;
            else
                carinidata.汽车高度 = 1500;
            ini.INIIO.GetPrivateProfileString("检测信息", "客车车长", "5500", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.客车车长 = a;
            else
                carinidata.客车车长 = 5500;

            ini.INIIO.GetPrivateProfileString("检测信息", "客车等级", "0", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.客车等级 = b;
            else
                carinidata.客车等级 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "汽车类型", "0", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.汽车类型 = b;
            else
                carinidata.汽车类型 = 0;

            ini.INIIO.GetPrivateProfileString("检测信息", "货车车身型式", "0", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.货车车身型式 = b;
            else
                carinidata.货车车身型式 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "额定总质量", "3500", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.额定总质量 = a;
            else
                carinidata.额定总质量 = 3500;
            ini.INIIO.GetPrivateProfileString("检测信息", "是否危险货物运输车辆", "0", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.是否危险货物运输车辆 = b;
            else
                carinidata.是否危险货物运输车辆 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "动力性检测加载力", "200", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.动力性检测加载力 = a;
            else
                carinidata.动力性检测加载力 = 200;

            ini.INIIO.GetPrivateProfileString("检测信息", "加载力计算方式", "0", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.加载力计算方式 = b;
            else
                carinidata.加载力计算方式 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "驱动轴数", "1", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.驱动轴数 = b;
            else
                carinidata.驱动轴数 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "轮胎类型", "0", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.轮胎类型 = b;
            else
                carinidata.轮胎类型 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "压燃式功率参数类型", "0", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.压燃式功率参数类型 = b;
            else
                carinidata.压燃式功率参数类型 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "压燃式额定功率", "200", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.压燃式额定功率 = a;
            else
                carinidata.压燃式额定功率 = 200;
            ini.INIIO.GetPrivateProfileString("检测信息", "点燃式额定扭矩", "200", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.点燃式额定扭矩 = a;
            else
                carinidata.点燃式额定扭矩 = 200;
            ini.INIIO.GetPrivateProfileString("检测信息", "点燃式额定扭矩转速", "4000", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.点燃式额定扭矩转速 = a;
            else
                carinidata.点燃式额定扭矩转速 = 4000;
            ini.INIIO.GetPrivateProfileString("检测信息", "驱动轴空载质量", "1000", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.驱动轴空载质量 = a;
            else
                carinidata.驱动轴空载质量 = 1000;
            ini.INIIO.GetPrivateProfileString("检测信息", "牵引车满载总质量", "200", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.牵引车满载总质量 = a;
            else
                carinidata.牵引车满载总质量 = 200;
            ini.INIIO.GetPrivateProfileString("检测信息", "加载力比例", "1", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (double.TryParse(temp.ToString().Trim(), out a))
                carinidata.加载力比例 = a;
            else
                carinidata.加载力比例 = 1;
            ini.INIIO.GetPrivateProfileString("检测信息", "驱动轴质量方式", "0", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.驱动轴质量方式 = b;
            else
                carinidata.驱动轴质量方式 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "油耗车速方式", "0", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            if (int.TryParse(temp.ToString().Trim(), out b))
                carinidata.油耗车速方式 = b;
            else
                carinidata.油耗车速方式 = 0;
            ini.INIIO.GetPrivateProfileString("检测信息", "是否检测车速", "N", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            carinidata.是否检测车速 = temp.ToString().Trim() == "Y";
            ini.INIIO.GetPrivateProfileString("检测信息", "是否检测动力性", "N", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            carinidata.是否检测动力性 = temp.ToString().Trim() == "Y";
            ini.INIIO.GetPrivateProfileString("检测信息", "是否检测油耗", "N", temp, 2048, @"C:\jcdatatxt\carinfoXn.ini");
            carinidata.是否检测油耗 = temp.ToString().Trim() == "Y";
            return carinidata;
        }
        public bool writeYhyResultIni(fuelResult resultdata)
        {
            try
            {
                //configInfdata preConfigData = getConfigIni();
                ini.INIIO.WritePrivateProfileString("检测结果", "检测速度", resultdata.检测速度.ToString("0.0"), @"C:\jcdatatxt\fuelResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "台架加载阻力", resultdata.台架加载阻力.ToString("0"), @"C:\jcdatatxt\fuelResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "燃料总消耗量", resultdata.燃料总消耗量.ToString("0.0"), @"C:\jcdatatxt\fuelResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "总行驶里程", resultdata.总行驶里程.ToString("0.0"), @"C:\jcdatatxt\fuelResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "百公里燃料消耗量", resultdata.百公里燃料消耗量.ToString("0.0"), @"C:\jcdatatxt\fuelResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "限值", resultdata.限值.ToString("0.0"), @"C:\jcdatatxt\fuelResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "限值依据", resultdata.限值依据.ToString("0"), @"C:\jcdatatxt\fuelResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "判定结果", resultdata.判定结果.ToString("0"), @"C:\jcdatatxt\fuelResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "汽车滚动阻力", resultdata.汽车滚动阻力.ToString("0"), @"C:\jcdatatxt\fuelResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "空气阻力", resultdata.空气阻力.ToString("0"), @"C:\jcdatatxt\fuelResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "滚动阻力系数", resultdata.滚动阻力系数.ToString("0.000"), @"C:\jcdatatxt\fuelResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "迎风面积", resultdata.迎风面积.ToString("0"), @"C:\jcdatatxt\fuelResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "空气阻力系数", resultdata.空气阻力系数.ToString("0.000"), @"C:\jcdatatxt\fuelResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "台架运转阻力", resultdata.台架运转阻力.ToString("0"), @"C:\jcdatatxt\fuelResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "台架滚动阻力", resultdata.台架滚动阻力.ToString("0"), @"C:\jcdatatxt\fuelResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "台架滚动阻力系数", resultdata.台架滚动阻力系数.ToString("0.000"), @"C:\jcdatatxt\fuelResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "台架内阻", resultdata.台架内阻.ToString("0"), @"C:\jcdatatxt\fuelResult.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool writeDieselDynamicResult(DieselDynamicResult resultdata)
        {
            try
            {
                ini.INIIO.WritePrivateProfileString("检测结果", "检测类型", resultdata.检测类型 .ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "车速", resultdata.车速.ToString("0.0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "车速判定结果", resultdata.车速判定结果.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "功率比值系数", resultdata.功率比值系数.ToString("0.000"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "额定功率", resultdata.额定功率.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "额定功率车速", resultdata.额定功率车速.ToString("0.0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "稳定车速", resultdata.稳定车速.ToString("0.0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "台架滚动阻力", resultdata.台架滚动阻力.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "加载力", resultdata.加载力.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "测功机内阻", resultdata.测功机内阻.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "轮胎滚动阻力", resultdata.轮胎滚动阻力.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "发动机附件阻力", resultdata.发动机附件阻力.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "车辆传动系允许阻力", resultdata.车辆传动系允许阻力.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "功率校正系数", resultdata.功率校正系数.ToString("0.000"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "台架滚动阻力系数", resultdata.台架滚动阻力系数.ToString("0.000"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "温度", resultdata.温度.ToString("0.0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "湿度", resultdata.湿度.ToString("0.0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "大气压", resultdata.大气压.ToString("0.0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "判定结果", resultdata.判定结果.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "等级评定结果", resultdata.等级评定结果.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool writeGasolineDynamicResult(GasolineDynamicResult resultdata)
        {
            try
            {
                ini.INIIO.WritePrivateProfileString("检测结果", "检测类型", resultdata.检测类型.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "车速", resultdata.车速.ToString("0.0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "车速判定结果", resultdata.车速判定结果.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "功率比值系数", resultdata.功率比值系数.ToString("0.000"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "发动机额定扭矩", resultdata.发动机额定扭矩.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "额定扭矩车速", resultdata.额定扭矩车速.ToString("0.0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "额定扭矩转速", resultdata.额定扭矩转速.ToString("0.0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "稳定车速", resultdata.稳定车速.ToString("0.0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "加载力", resultdata.加载力.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "发动机达标扭矩驱动力", resultdata.发动机达标扭矩驱动力.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "测功机内阻", resultdata.测功机内阻.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "轮胎滚动阻力", resultdata.轮胎滚动阻力.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "发动机附件阻力", resultdata.发动机附件阻力.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "车辆传动系允许阻力", resultdata.车辆传动系允许阻力.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "功率校正系数", resultdata.功率校正系数.ToString("0.000"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "台架滚动阻力系数", resultdata.台架滚动阻力系数.ToString("0.000"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "温度", resultdata.温度.ToString("0.0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "湿度", resultdata.湿度.ToString("0.0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "大气压", resultdata.大气压.ToString("0.0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "判定结果", resultdata.判定结果.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "等级评定结果", resultdata.等级评定结果.ToString("0"), @"C:\jcdatatxt\DynamicResult.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool writeSpeedResult(SpeedResult resultdata)
        {
            try
            {
                ini.INIIO.WritePrivateProfileString("检测结果", "车速", resultdata.车速.ToString("0.0"), @"C:\jcdatatxt\SpeedResult.ini");
                ini.INIIO.WritePrivateProfileString("检测结果", "车速判定结果", resultdata.车速判定结果.ToString("0"), @"C:\jcdatatxt\SpeedResult.ini");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool writeYhyResultJson(fuelResult resultdata,int lx)
        {
            try
            {
                if (lx == 0)
                {
                    string jsonstring = new JObject((JObject)JsonConvert.DeserializeObject(ObjectToJson(resultdata))).ToString();
                    ini.INIIO.saveLogInf("SEND:\r\nwriteYhyResultJson(" + jsonstring + ")");
                    ini.INIIO.saveJsonResultToFile(jsonstring, "fuelResult");
                }
                else
                {
                    Type t = resultdata.GetType();
                    PropertyInfo[] PropertyList = t.GetProperties();
                    foreach (PropertyInfo item in PropertyList)
                    {
                        string name = item.Name;
                        object value = item.GetValue(resultdata, null);
                        ini.INIIO.WritePrivateProfileString("检测结果", name, value.ToString(), @"C:\jcdatatxt\fuelResult.ini");

                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool writeYhyResultJsonIni(fuelResult resultdata)
        {
            try
            {
                Type t = resultdata.GetType();
                PropertyInfo[] PropertyList = t.GetProperties();
                foreach (PropertyInfo item in PropertyList)
                {
                    string name = item.Name;
                    object value = item.GetValue(resultdata, null);
                    ini.INIIO.WritePrivateProfileString("检测结果", name, value.ToString(), @"C:\jcdatatxt\fuelResult.ini");

                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool writeDieselDynamicJson(DieselDynamicResult resultdata,int lx)
        {
            try
            {
                if (lx == 0)
                {
                    string jsonstring = new JObject((JObject)JsonConvert.DeserializeObject(ObjectToJson(resultdata))).ToString();
                    ini.INIIO.saveLogInf("SEND:\r\nwriteDieselDynamicJson(" + jsonstring + ")");
                    ini.INIIO.saveJsonResultToFile(jsonstring, "DynamicResult");
                }
                else
                {
                    Type t = resultdata.GetType();
                    PropertyInfo[] PropertyList = t.GetProperties();
                    foreach (PropertyInfo item in PropertyList)
                    {
                        string name = item.Name;
                        object value = item.GetValue(resultdata, null);
                        ini.INIIO.WritePrivateProfileString("检测结果", name, value.ToString(), @"C:\jcdatatxt\DynamicResult.ini");

                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool writeDieselDynamicJsonIni(DieselDynamicResult resultdata)
        {
            try
            {
                Type t = resultdata.GetType();
                PropertyInfo[] PropertyList = t.GetProperties();
                foreach (PropertyInfo item in PropertyList)
                {
                    string name = item.Name;
                    object value = item.GetValue(resultdata, null);
                    ini.INIIO.WritePrivateProfileString("检测结果", name, value.ToString(), @"C:\jcdatatxt\DynamicResult.ini");

                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool writeGasolineDynamicJson(GasolineDynamicResult resultdata,int lx)
        {
            try
            {
                if (lx == 0)
                {
                    string jsonstring = new JObject((JObject)JsonConvert.DeserializeObject(ObjectToJson(resultdata))).ToString();
                    ini.INIIO.saveLogInf("SEND:\r\nwriteDieselDynamicJson(" + jsonstring + ")");
                    ini.INIIO.saveJsonResultToFile(jsonstring, "DynamicResult");
                }
                else
                {
                    Type t = resultdata.GetType();
                    PropertyInfo[] PropertyList = t.GetProperties();
                    foreach (PropertyInfo item in PropertyList)
                    {
                        string name = item.Name;
                        object value = item.GetValue(resultdata, null);
                        ini.INIIO.WritePrivateProfileString("检测结果", name, value.ToString(), @"C:\jcdatatxt\DynamicResult.ini");

                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool writeGasolineDynamicJsonIni(GasolineDynamicResult resultdata)
        {
            try
            {
                Type t = resultdata.GetType();
                PropertyInfo[] PropertyList = t.GetProperties();
                foreach (PropertyInfo item in PropertyList)
                {
                    string name = item.Name;
                    object value = item.GetValue(resultdata, null);
                    ini.INIIO.WritePrivateProfileString("检测结果", name, value.ToString(), @"C:\jcdatatxt\DynamicResult.ini");

                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool writeSpeedJson(SpeedResult resultdata,int lx)
        {
            try
            {
                if (lx == 0)
                {
                    string jsonstring = new JObject((JObject)JsonConvert.DeserializeObject(ObjectToJson(resultdata))).ToString();
                    ini.INIIO.saveLogInf("SEND:\r\nwriteSpeedJson(" + jsonstring + ")");
                    ini.INIIO.saveJsonResultToFile(jsonstring, "SpeedResult");
                }
                else
                {
                    Type t = resultdata.GetType();
                    PropertyInfo[] PropertyList = t.GetProperties();
                    foreach (PropertyInfo item in PropertyList)
                    {
                        string name = item.Name;
                        object value = item.GetValue(resultdata, null);
                        ini.INIIO.WritePrivateProfileString("检测结果", name, value.ToString(), @"C:\jcdatatxt\SpeedResult.ini");

                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool writeSpeedJsonIni(SpeedResult resultdata)
        {
            try
            {
                Type t = resultdata.GetType();
                PropertyInfo[] PropertyList = t.GetProperties();
                foreach (PropertyInfo item in PropertyList)
                {
                    string name = item.Name;
                    object value = item.GetValue(resultdata, null);
                    ini.INIIO.WritePrivateProfileString("检测结果", name, value.ToString(), @"C:\jcdatatxt\SpeedResult.ini");

                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool writeQdzzlJson(QdzzlResult resultdata,int lx)
        {
            try
            {
                if (lx == 0)
                {
                    string jsonstring = new JObject((JObject)JsonConvert.DeserializeObject(ObjectToJson(resultdata))).ToString();
                    ini.INIIO.saveLogInf("SEND:\r\nwriteQdzzlJson(" + jsonstring + ")");
                    ini.INIIO.saveJsonResultToFile(jsonstring, "QdzzlResult");
                }
                else
                {
                    Type t = resultdata.GetType();
                    PropertyInfo[] PropertyList = t.GetProperties();
                    foreach (PropertyInfo item in PropertyList)
                    {
                        string name = item.Name;
                        object value = item.GetValue(resultdata, null);
                        ini.INIIO.WritePrivateProfileString("检测结果", name, value.ToString(), @"C:\jcdatatxt\QdzzlResult.ini");

                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool writeQdzzlJsonIni(QdzzlResult resultdata)
        {
            try
            {
                Type t = resultdata.GetType();
                PropertyInfo[] PropertyList = t.GetProperties();
                foreach (PropertyInfo item in PropertyList)
                {
                    string name = item.Name;
                    object value = item.GetValue(resultdata, null);
                    ini.INIIO.WritePrivateProfileString("检测结果", name, value.ToString(), @"C:\jcdatatxt\QdzzlResult.ini");

                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string ObjectToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        // 从一个Json串生成对象信息
        public static object JsonToObject(string jsonString, object obj)
        {
            return JsonConvert.DeserializeObject(jsonString, obj.GetType());
        }
    }
}
