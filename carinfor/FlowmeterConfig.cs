using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace carinfor
{
    public class FlowmeterConfig
    {
        private float bdqO2;

        public float BdqO2
        {
            get { return bdqO2; }
            set { bdqO2 = value; }
        }
        private float jcqO2_high;

        public float JcqO2_high
        {
            get { return jcqO2_high; }
            set { jcqO2_high = value; }
        }
        private float jcqO2_low;

        public float JcqO2_low
        {
            get { return jcqO2_low; }
            set { jcqO2_low = value; }
        }

    }
    public class flowmeterConfigIni
    {
        public FlowmeterConfig getFlowmeterConfigIni()
        {
            float a = 0;
            //int b = 0;
            FlowmeterConfig flowmeterdata = new FlowmeterConfig();
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            ini.INIIO.GetPrivateProfileString("流量计标定", "标定气O2", "", temp, 2048, @".\detectConfig.ini");
            if (float.TryParse(temp.ToString().Trim(), out a))
                flowmeterdata.BdqO2 = a;
            else
                flowmeterdata.BdqO2 = 16.0f;
            ini.INIIO.GetPrivateProfileString("废气仪标定", "高检查气", "", temp, 2048, @".\detectConfig.ini");
            if (float.TryParse(temp.ToString().Trim(), out a))
                flowmeterdata.JcqO2_high = a;
            else
                flowmeterdata.JcqO2_high = 19.0f;
            ini.INIIO.GetPrivateProfileString("废气仪标定", "低检查气", "", temp, 2048, @".\detectConfig.ini");
            if (float.TryParse(temp.ToString().Trim(), out a))
                flowmeterdata.JcqO2_low = a;
            else
                flowmeterdata.JcqO2_low = 4.5f;


            return flowmeterdata;
        }

    }
}

