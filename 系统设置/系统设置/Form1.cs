using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using DevComponents.DotNetBar;

namespace 系统设置
{
    public partial class Form1 : Office2007Form
    {
        carinfor.configIni configini = new carinfor.configIni();
        private string diwmp;
        private string cgjlx;
        private string pef;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initConfigInfo();
        }
        private void initConfigInfo()
        {
            carinfor.equipmentConfigInfdata equipconfig = configini.getEquipConfigIni();
            carinfor.VmasConfigInfdata vmasconfig = configini.getVmasConfigIni();
            carinfor.AsmConfigInfdata asmconfig = configini.getAsmConfigIni();
            carinfor.SdsConfigInfdata sdsconfig = configini.getSdsConfigIni();
            carinfor.BtgConfigInfdata btgconfig = configini.getBtgConfigIni();
            carinfor.DynConfigInfdata dynconfig = configini.getDynConfigIni();
            carinfor.LugdownConfigInfdata lugdownconfig = configini.getLugdownConfigIni();
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            try
            {
                ini.INIIO.GetPrivateProfileString("DIW", "DIWLX", "轻型车", temp, 2048,Application.StartupPath+ @"\detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                if (temp.ToString() == "轻型车")
                {
                    comboBoxCgjlx.SelectedIndex = 0;
                }
                else
                {
                    comboBoxCgjlx.SelectedIndex = 1;
                }
                ini.INIIO.GetPrivateProfileString("DIW", "DIWMP", "907", temp, 2048, Application.StartupPath + @"\detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                diwmp = temp.ToString();
                ini.INIIO.GetPrivateProfileString("废气仪标定", "PEF", "907", temp, 2048, Application.StartupPath + @"\detectConfig.ini");          //读配置文件（段名，字段，默认值，保存的strbuilder，大小，路径）
                pef = temp.ToString();
                textBoxDIWMP.Text = diwmp;
                textBoxPEF.Text = pef;
            }
            catch
            { }
            comboBoxHJT.Text = equipconfig.TestStandard;
            textBoxVmasNdz.Text = vmasconfig.Ndz.ToString("0.0");
            textBoxVmasLljll.Text = vmasconfig.Lljll.ToString("0.0");
            textBoxVmasWqll.Text = vmasconfig.Wqll.ToString("0.0");
            textBoxVmasXsb.Text = vmasconfig.Xsb.ToString("0.0");
            textBoxVmasJzgl.Text = vmasconfig.Gljz.ToString("0.0");
            textBoxVmasLxcc.Text = vmasconfig.Lxcc.ToString("0.0");
            textBoxVmasLjcc.Text = vmasconfig.Ljcc.ToString("0.0");
            textBoxVmasDssj.Text = vmasconfig.Dssj.ToString("0");
            textBoxVmasNdccsj.Text = vmasconfig.ndccsj.ToString("0");
            checkBoxVmasSdjk.Checked = vmasconfig.SpeedMonitor;
            checkBoxVmasJzgljk.Checked = vmasconfig.PowerMonitor;
            checkBoxVmasNdjk.Checked = vmasconfig.ConcentrationMonitor;
            checkBoxVmasLljk.Checked = vmasconfig.FlowMonitorr;
            checkBoxVmasXsbjk.Checked = vmasconfig.ThinnerratioMonitor;
            checkBoxVmasHjyjk.Checked = vmasconfig.Huanjingo2Monitor;
            checkBoxVmasCyljk.Checked = vmasconfig.RemainedMonitor;
            checkBoxVmasFlowback.Checked = vmasconfig.Flowback;
            checkBoxVmasZero.Checked = vmasconfig.IfFqyTl;
            checkBoxVmasDisplayData.Checked = vmasconfig.IfDisplayData;
            checkBoxVmasSureTemp.Checked = vmasconfig.IfSureTemp;
            checkBoxVmasYw.Checked = vmasconfig.IsTestYw;
            checkBoxAutoRestart.Checked = vmasconfig.AutoRestart;
            radioButtonVmasAccelerateProcess.Checked = !vmasconfig.IsWholeProcessJK;
            radioButtonVmasWholeProcess.Checked = vmasconfig.IsWholeProcessJK;
            checkBoxVMAS_CO2ANDO2.Checked = vmasconfig.SdCo2AndO2Monitor;
            textBoxVmasFlowbackTime.Text = vmasconfig.FlowTime.ToString("0");
            radioButtonCjBeforeTl.Checked = vmasconfig.CjBeforeTl;
            radioButtonCjAfterTl.Checked = !vmasconfig.CjBeforeTl;

            textBoxAsmNdz.Text = asmconfig.Ndz.ToString("0.0");
            textBoxAsmGlwc.Text = asmconfig.Gljz.ToString("0.0");
            textBoxAsmCcsj.Text = asmconfig.Lxcc.ToString("0.0");
            textBoxAsmFlowtime.Text = asmconfig.FlowTime.ToString("0");
            checkBoxAsmSdjk.Checked = asmconfig.SpeedMonitor;
            checkBox05Sdjk.Checked = asmconfig.Speed05Monitor;
            checkBoxAsmGljk.Checked = asmconfig.PowerMonitor;
            checkBoxAsmNdjk.Checked = asmconfig.ConcentrationMonitor;
            checkBoxAsmClljk.Checked = asmconfig.RemainedMonitor;
            checkBoxAsmFlowback.Checked = asmconfig.Flowback;
            checkBoxAsmZero.Checked = asmconfig.IfFqyTl;
            checkBoxAsmDispData.Checked = asmconfig.IfDisplayData;
            checkBoxAsmSuretemp.Checked = asmconfig.IfSureTemp;
            checkBoxASMyw.Checked = asmconfig.IsTestYw;
            checkBoxKsgk.Checked = asmconfig.IsKsgkUsed;
            checkBoxAsmHalfXzKsgk.Checked = asmconfig.IsAsmHalfXzKsgk;

            textBoxLugDownMinSpeed.Text = lugdownconfig.MinSpeed.ToString("0");
            textBoxLugDownMaxSpeed.Text = lugdownconfig.MaxSpeed.ToString("0");
            comboBoxLugdownZsj.Text = lugdownconfig.Zsj;
            comboBoxLugdownZsjCom.Text = lugdownconfig.Zsjck;
            checkBoxLugdownSureTemp.Checked = lugdownconfig.IfSureTemp;
            checkBoxLugdownZsPd.Checked = lugdownconfig.isFdjzsJudge;
            checkBoxLugdownYw.Checked = lugdownconfig.IsTestYw;
            radioButtonLugMaxHpGlsm.Checked = (lugdownconfig.LugdownMaxHpStyle == 0);
            radioButtonLugMaxHpVelmaxhp.Checked = (lugdownconfig.LugdownMaxHpStyle == 1);
            if (lugdownconfig.Glsmms == "恒功率")
            {
                radioButtonLugdownHgl.Checked = true;
                radioButtonLugdownhs.Checked = false;
            }
            else
            {
                radioButtonLugdownHgl.Checked = false;
                radioButtonLugdownhs.Checked = true;
            }
            textBoxLugdownSmpl.Text = lugdownconfig.Smpl.ToString("0");
            checkBoxGSMAXPPD.Checked = lugdownconfig.gsMaxPPD;
            checkBoxGSKCBPD.Checked = lugdownconfig.gsKcbPD;
            checkBoxGSKHGPD.Checked = lugdownconfig.gsKhgPD;
            checkBoxLugdownJcNox.Checked = lugdownconfig.testNOx;

            textBoxSdsFlowtime.Text = sdsconfig.FlowTime.ToString("0");
            textBoxSdsNdz.Text = sdsconfig.Ndz.ToString("0.0");
            textBoxSdsZscc.Text = sdsconfig.Zscc.ToString("0");
            checkBoxSdsFlowback.Checked = sdsconfig.Flowback;
            checkBoxSdsZero.Checked = sdsconfig.IfFqyTl;
            checkBoxSdsNdjk.Checked = sdsconfig.ConcentrationMonitor;
            checkBoxSdsZsjk.Checked = sdsconfig.RotateSpeedMonitor;
            comboBoxSdsZsj.Text = sdsconfig.Zsj;
            comboBoxSdsZsjCom.Text = sdsconfig.Zsjck;
            checkBoxSdsYw.Checked = sdsconfig.IsTestYw;
            comboBox3500.SelectedIndex = sdsconfig.TimerMode3500;
            comboBoxHPrepare.SelectedIndex = sdsconfig.TimerModeHP;
            comboBoxHTest.SelectedIndex = sdsconfig.TimerModeHT;
            comboBoxLPrepare.SelectedIndex = sdsconfig.TimerModeLP;
            comboBoxLTest.SelectedIndex = sdsconfig.TimerModeLT;
            checkBoxSdsSureTemp.Checked = sdsconfig.IsSureTemp;

            textBoxBtgDyzs.Text = btgconfig.Dyzs.ToString("0");
            checkBoxBtgZsjk.Checked = btgconfig.RotateSpeedMonitor;
            comboBoxBtgZsj.Text = btgconfig.Zsj;
            comboBoxBtgZsjCom.Text = btgconfig.Zsjck;
            checkBoxBtgYw.Checked = btgconfig.IsTestYw;
            comboBoxBTGCFCS.Text = btgconfig.Btgcfcs.ToString();
            comboBoxBTGCLCS.Text = btgconfig.btgclcs.ToString();
            checkBoxBtgManualTantou.Checked = btgconfig.BtgManualTantou;
            checkBoxBTGJHGCJK.Checked = btgconfig.jhzsgcjk;
            if (btgconfig.btgDszsValue == 0) radioButtonBTGDSZS.Checked = true;
            else radioButtonBTGDYZS.Checked = true;


            comboBoxDynZsj.Text = dynconfig.Zsj;
            comboBoxDynZsjCk.Text = dynconfig.Zsjck;
            textBoxDynZs.Text = dynconfig.cczs.ToString("0");
            textBoxDynZsTime.Text = dynconfig.stableTime.ToString("0");
            textBoxDynForceTime.Text = dynconfig.forceTime.ToString("0");
            textBoxFuelForceTime.Text = dynconfig.FuelforceTime.ToString("0");
            textBoxFuelForceQj.Text = dynconfig.FuelForceQj.ToString("0");
            textBoxFuelSdQj.Text = dynconfig.FuelSdQj.ToString("0.0");
            textBoxDynForceQj.Text = dynconfig.DynForceQj.ToString("0");
            radioButtonDynZdyk.Checked = (dynconfig.DynYkStyle == 0);
            radioButtonDynWjyk.Checked= (dynconfig.DynYkStyle == 1);
            comboBoxDynYkTd.Text = dynconfig.DynWjyktd.ToString();
            radioButtonDynReStartForm15.Checked = (dynconfig.DynReStartStyle == 1);
            radioButtonDynReStartFrom60.Checked = (dynconfig.DynReStartStyle == 0);
            checkBoxDynGddw.Checked = dynconfig.DynUseGddw;
            checkBoxDynYkdw.Checked = dynconfig.DynUseYkdw;
            textBoxDynGddwTime.Text = dynconfig.DynGddwTime.ToString("0");
            checkBoxDynJkYh.Checked = dynconfig.DynFuelJk;
            textBoxDynYhxz.Text = dynconfig.DynFuelXz.ToString("0.00");
            textBoxFuelTestFjcs.Text = dynconfig.FuelTestFjjs.ToString("0");
            textBoxDynSdqj.Text = dynconfig.DynSdqj.ToString("0.0");
            textBoxDynMaxWaitTime.Text = dynconfig.DynMaxWaitTime.ToString("0");
            textBoxDynVwSdqj.Text = dynconfig.DynVwSdqj.ToString("0.0");
            textBoxDynYkKey.Text = dynconfig.DynYkKey;
            radioButtonDynFileJson.Checked = (dynconfig.DynFileStyle == 0);
            radioButtonDynFileIni.Checked = (dynconfig.DynFileStyle == 1);
            checkBoxDynStopUnstable.Checked= dynconfig.DynStopUnstable;
            textBoxDynUnstableTime.Text= dynconfig.DynUnstableTime.ToString();

            checkBoxDYNTL.Checked = dynconfig.DynTl;
            checkBoxDynBackTest.Checked = dynconfig.DynBackTest;
            checkBoxDynCO2Test.Checked = dynconfig.DynCO2Test;
            checkBoxDynFlowBack.Checked = dynconfig.DynFlowBack;
            checkBoxDynYkqr.Checked = dynconfig.DynYkqr;

            comboBoxBPQCOM.Text = equipconfig.BpqCom;
            comboBoxBPQDY.Text = equipconfig.BpqDy.ToString();
            comboBoxBPQFS.Text = equipconfig.BpqMethod;
            comboBoxBPQXH.Text = equipconfig.BpqXh;
            comboBoxFAN.Text = equipconfig.HeatFan.ToString();
            comboBoxEmergency.Text = equipconfig.EmergencyStop.ToString();
            



            if (equipconfig.Cgjifpz == false)
                comboBoxCgjxh.Text = "无";
            else
                comboBoxCgjxh.Text = equipconfig.Cgjxh;
            comboBoxCgjck.Text = equipconfig.Cgjck;
            if (equipconfig.Fqyifpz == false)
                comboBoxFqyxh.Text = "无";
            else
                comboBoxFqyxh.Text = equipconfig.Fqyxh;
            comboBoxFqyck.Text = equipconfig.Fqyck;
            if (equipconfig.Ydjifpz == false)
                comboBoxYdjxh.Text = "无";
            else
                comboBoxYdjxh.Text = equipconfig.Ydjxh;
            checkBoxOldMqy200.Checked = equipconfig.IsOldMqy200;
            comboBoxYdjck.Text = equipconfig.Ydjck;
            if (equipconfig.Lzydjifpz == false)
                comboBoxLZYDJXH.Text = "无";
            else
                comboBoxLZYDJXH.Text = equipconfig.Lzydjxh;
            comboBoxLZYDJCK.Text = equipconfig.Lzydjck;
            if (equipconfig.Lljifpz == false)
                comboBoxLljxh.Text = "无";
            else
                comboBoxLljxh.Text = equipconfig.Lljxh;
            comboBoxLljck.Text = equipconfig.Lljck;
            if (equipconfig.isYhyPz == false)
                comboBoxYhyxh.Text = "无";
            else
                comboBoxYhyxh.Text = equipconfig.YhyXh;
            comboBoxYhyck.Text = equipconfig.YhyCk;
            if (equipconfig.NOxifpz == false)
                comboBoxNOxXh.Text = "无";
            else
                comboBoxNOxXh.Text = equipconfig.NOxXh;
            comboBoxNOxCk.Text = equipconfig.NOxCk;
            if (equipconfig.Ledifpz == false)
                comboBoxLEDck.Text = "未配置";
            else
                comboBoxLEDck.Text = equipconfig.Ledck;
            comboBoxCOMXCE.Text = equipconfig.Xce100ck;
            comboBoxXCECOMSTRING.Text = equipconfig.Xce100Comstring;
            comboBoxLEDXH.Text = equipconfig.Ledxh;
            comboBoxLEDCOMSTRING.Text = equipconfig.LedComstring;
            comboBoxTemp.Text = equipconfig.TempInstrument;
            comboBoxCgjckpzz.Text = equipconfig.cgjckpzz;
            comboBoxFqyckppz.Text=equipconfig.Fqyckpzz ;
            comboBoxYdjckppz.Text=equipconfig.Ydjckpzz ;
            comboBoxLljckpzz.Text = equipconfig.Lljckpzz;
            comboBoxYhyckpz.Text = equipconfig.YhjCkpz;
            comboBoxNOxCkpz.Text = equipconfig.NOxCkpz;
            comboBoxLZCKPZZ.Text = equipconfig.Lzckpzz;
            comboBoxDriverScreen.SelectedIndex = equipconfig.DriverDisplay;
            comboBoxDisplayMethod.Text = equipconfig.DisplayMethod;
            comboBoxDriveFbl.SelectedIndex = equipconfig.DriverFbl;
            textBoxFqyxysj.Text = equipconfig.FqyDelayTime.ToString();
            textBoxLLjO2xysj.Text = equipconfig.LljDelayTime.ToString();
            textBoxLLjLlmyz.Text = equipconfig.Lljllmy.ToString();
            textBoxLugdownWdsj.Text = equipconfig.LugdownWdsj.ToString();
            comboBoxWorkMode.SelectedIndex = (equipconfig.WorkAutomaticMode ? 0 : 1);
            textBoxBrakePWM.Text = equipconfig.BrakePWM.ToString();
            textBoxYdjL.Text = equipconfig.YdjL.ToString();
            comboBoxCgjNz.SelectedIndex = equipconfig.CgjNz;
            comboBoxCarGd.Text = equipconfig.CarGdChanel.ToString();
            checkBoxGdyk.Checked = equipconfig.isIgbtContainGdyk;
            checkBoxIsFqyNhSelfUse.Checked = equipconfig.isFqyNhSelfUse;
            checkBoxIsYdjNhSelfUse.Checked = equipconfig.isYdjNhSelfUse;
            comboBoxLEDROW1.Text = equipconfig.ledrow1.ToString();
            comboBoxLEDROW2.Text = equipconfig.ledrow2.ToString();
            comboBoxLEDTJPH.Text = equipconfig.LEDTJPH.ToString();
            checkBoxDisplayJudge.Checked = equipconfig.displayJudge;
            textBoxLZYDJADD.Text = equipconfig.lzydjadd;


            checkBoxUseWeightWCF.Checked=equipconfig.useWeightWCF ;
            textBoxWeightWCF.Text=equipconfig.WeightWCFaddress;
            checkBoxJHJK.Checked = equipconfig.useJHJK;
            textBoxJHLBGLYJZ.Text=equipconfig.JHLBGLB.ToString("0");
            textBoxJHLAMBDAMIN.Text=equipconfig.JHLAMBDAMIN.ToString("0.00");
            textBoxJHLAMBDAMAX.Text=equipconfig.JHLAMBDAMAX.ToString("0.00");
            comboBoxDataSecondsType.Text = equipconfig.DATASECONDS_TYPE;
            /*comboBoxZZYXH.Text = equipconfig.ZzyXh;
            comboBoxZZYCK.Text = equipconfig.ZzyCk;
            comboBoxZZYCKPZ.Text = equipconfig.ZzyCkpzz;
            comboBoxZZSURESTYLE.SelectedIndex = equipconfig.ZzSureStyle;
            comboBoxZzGd.Text = equipconfig.ZzGdChanel.ToString();
            comboBoxLEFTZZCHANEL.Text = equipconfig.LeftZzChanel.ToString();
            comboBoxRIGHTZZCHANEL.Text = equipconfig.RightZzChanel.ToString();*/

            comboBoxTMQCK.Text = equipconfig.Tmqck;
            comboBoxTMQPZ.Text = equipconfig.Tmqckpz;
            comboBoxTMQXH.Text = equipconfig.Tmqxh;
            checkBoxTPWSD.Checked = equipconfig.isTpTempInstrument;
            checkBoxCD_FQY.Checked = equipconfig.cd_fqy;
            checkBoxCD_YDJ.Checked = equipconfig.cd_ydj;
        }

        private void buttonVmasSave_Click(object sender, EventArgs e)
        {
            try
            {
                carinfor.VmasConfigInfdata vmasconfig = configini.getVmasConfigIni();
                vmasconfig.Ndz = float.Parse(textBoxVmasNdz.Text);
                vmasconfig.Lljll = float.Parse(textBoxVmasLljll.Text);
                vmasconfig.Wqll = float.Parse(textBoxVmasWqll.Text);
                vmasconfig.Xsb = float.Parse(textBoxVmasXsb.Text);
                vmasconfig.Gljz = float.Parse(textBoxVmasJzgl.Text);
                vmasconfig.Lxcc = float.Parse(textBoxVmasLxcc.Text);
                vmasconfig.Ljcc = float.Parse(textBoxVmasLjcc.Text);
                vmasconfig.Dssj = int.Parse(textBoxVmasDssj.Text);
                vmasconfig.ndccsj = int.Parse(textBoxVmasNdccsj.Text);
                vmasconfig.FlowTime = int.Parse(textBoxVmasFlowbackTime.Text);
                vmasconfig.SpeedMonitor = checkBoxVmasSdjk.Checked;

                vmasconfig.PowerMonitor = checkBoxVmasJzgljk.Checked;
                vmasconfig.ConcentrationMonitor = checkBoxVmasNdjk.Checked;
                vmasconfig.SdCo2AndO2Monitor = checkBoxVMAS_CO2ANDO2.Checked;
                vmasconfig.FlowMonitorr = checkBoxVmasLljk.Checked;
                vmasconfig.ThinnerratioMonitor = checkBoxVmasXsbjk.Checked;
                vmasconfig.Huanjingo2Monitor = checkBoxVmasHjyjk.Checked;
                vmasconfig.RemainedMonitor = checkBoxVmasCyljk.Checked;
                vmasconfig.Flowback = checkBoxVmasFlowback.Checked;
                vmasconfig.IfFqyTl = checkBoxVmasZero.Checked;
                vmasconfig.IfDisplayData = checkBoxVmasDisplayData.Checked;
                vmasconfig.IfSureTemp = checkBoxVmasSureTemp.Checked;
                vmasconfig.IsTestYw = checkBoxVmasYw.Checked;
                vmasconfig.AutoRestart = checkBoxAutoRestart.Checked;
                vmasconfig.IsWholeProcessJK = radioButtonVmasWholeProcess.Checked;
                vmasconfig.CjBeforeTl = radioButtonCjBeforeTl.Checked;
                if (configini.writeVmasConfigIni(vmasconfig))
                    MessageBox.Show("保存成功.", "系统提示");
                else
                    MessageBox.Show("数据输入有误,请检查.", "未成功保存");
            }
            catch(Exception er)
            {
                MessageBox.Show("保存出错,异常信息:"+er.Message, "未成功保存");
            }
        }

        private void buttonAsmSave_Click(object sender, EventArgs e)
        {
            carinfor.AsmConfigInfdata asmconfig = configini.getAsmConfigIni();

            asmconfig.Ndz = float.Parse(textBoxAsmNdz.Text);
            asmconfig.Gljz = float.Parse(textBoxAsmGlwc.Text);
            asmconfig.Lxcc = float.Parse(textBoxAsmCcsj.Text);
            asmconfig.FlowTime = int.Parse(textBoxAsmFlowtime.Text);
            asmconfig.SpeedMonitor = checkBoxAsmSdjk.Checked;
            asmconfig.Speed05Monitor = checkBox05Sdjk.Checked;
            asmconfig.PowerMonitor = checkBoxAsmGljk.Checked;
            asmconfig.ConcentrationMonitor = checkBoxAsmNdjk.Checked;
            asmconfig.RemainedMonitor = checkBoxAsmClljk.Checked;
            asmconfig.Flowback = checkBoxAsmFlowback.Checked;
            asmconfig.IfFqyTl = checkBoxAsmZero.Checked;
            asmconfig.IfDisplayData = checkBoxAsmDispData.Checked;
            asmconfig.IfSureTemp = checkBoxAsmSuretemp.Checked;
            asmconfig.IsTestYw = checkBoxASMyw.Checked;
            asmconfig.IsKsgkUsed = checkBoxKsgk.Checked;
            asmconfig.IsAsmHalfXzKsgk = checkBoxAsmHalfXzKsgk.Checked;
            if (configini.writeAsmConfigIni(asmconfig))
                MessageBox.Show("保存成功.", "系统提示");
            else
                MessageBox.Show("数据输入有误,请检查.", "未成功保存");
        }

        private void buttonLugdownSave_Click(object sender, EventArgs e)
        {
            carinfor.LugdownConfigInfdata lugdownconfig = configini.getLugdownConfigIni();
            lugdownconfig.MinSpeed = int.Parse(textBoxLugDownMinSpeed.Text);
            lugdownconfig.MaxSpeed = float.Parse(textBoxLugDownMaxSpeed.Text);
            lugdownconfig.Smpl = int.Parse(textBoxLugdownSmpl.Text);
            lugdownconfig.IfSureTemp = checkBoxLugdownSureTemp.Checked;
            lugdownconfig.isFdjzsJudge = checkBoxLugdownZsPd.Checked;
            lugdownconfig.gsMaxPPD = checkBoxGSMAXPPD.Checked;
            lugdownconfig.gsKcbPD = checkBoxGSKCBPD.Checked;
            lugdownconfig.gsKhgPD = checkBoxGSKHGPD.Checked;
            lugdownconfig.Zsj = comboBoxLugdownZsj.Text;
            lugdownconfig.Zsjck = comboBoxLugdownZsjCom.Text;
            lugdownconfig.IsTestYw = checkBoxLugdownYw.Checked;
            lugdownconfig.Glsmms = (radioButtonLugdownHgl.Checked) ? "恒功率" : "恒速度";
            lugdownconfig.LugdownMaxHpStyle = (radioButtonLugMaxHpVelmaxhp.Checked) ? 1 : 0;
            lugdownconfig.testNOx = checkBoxLugdownJcNox.Checked;
            if (configini.writeLugdownConfigIni(lugdownconfig))
                MessageBox.Show("保存成功.", "系统提示");
            else
                MessageBox.Show("数据输入有误,请检查.", "未成功保存");
        }

        private void buttonSdsSave_Click(object sender, EventArgs e)
        {
            carinfor.SdsConfigInfdata sdsconfig = configini.getSdsConfigIni();
            sdsconfig.FlowTime = int.Parse(textBoxSdsFlowtime.Text);
            sdsconfig.Ndz = float.Parse(textBoxSdsNdz.Text);
            sdsconfig.Zscc = int.Parse(textBoxSdsZscc.Text);
            sdsconfig.Flowback = checkBoxSdsFlowback.Checked;
            sdsconfig.IfFqyTl = checkBoxSdsZero.Checked;
            sdsconfig.ConcentrationMonitor = checkBoxSdsNdjk.Checked;
            sdsconfig.RotateSpeedMonitor = checkBoxSdsZsjk.Checked;
            sdsconfig.Zsj = comboBoxSdsZsj.Text;
            sdsconfig.Zsjck = comboBoxSdsZsjCom.Text;
            sdsconfig.IsTestYw = checkBoxSdsYw.Checked;
            sdsconfig.IsSureTemp = checkBoxSdsSureTemp.Checked;
            sdsconfig.TimerMode3500 = comboBox3500.SelectedIndex;
            sdsconfig.TimerModeHP = comboBoxHPrepare.SelectedIndex;
            sdsconfig.TimerModeHT = comboBoxHTest.SelectedIndex;
            sdsconfig.TimerModeLP = comboBoxLPrepare.SelectedIndex;
            sdsconfig.TimerModeLT = comboBoxLTest.SelectedIndex;
            if (configini.writeSdsConfigIni(sdsconfig))
                MessageBox.Show("保存成功.", "系统提示");
            else
                MessageBox.Show("数据输入有误,请检查.", "未成功保存");
        }

        private void buttonBtgSave_Click(object sender, EventArgs e)
        {
            carinfor.BtgConfigInfdata btgconfig = configini.getBtgConfigIni();
            btgconfig.Dyzs = int.Parse(textBoxBtgDyzs.Text);
            btgconfig.RotateSpeedMonitor = checkBoxBtgZsjk.Checked;
            btgconfig.jhzsgcjk = checkBoxBTGJHGCJK.Checked;
            btgconfig.Zsj = comboBoxBtgZsj.Text;
            btgconfig.Zsjck = comboBoxBtgZsjCom.Text;
            btgconfig.IsTestYw = checkBoxBtgYw.Checked;
            btgconfig.Btgcfcs =int.Parse(comboBoxBTGCFCS.Text);
            btgconfig.btgclcs = int.Parse(comboBoxBTGCLCS.Text);
            btgconfig.BtgManualTantou = checkBoxBtgManualTantou.Checked;
            btgconfig.btgDszsValue = radioButtonBTGDSZS.Checked ? 0 : 1;
            if (configini.writeBtgConfigIni(btgconfig))
                MessageBox.Show("保存成功.", "系统提示");
            else
                MessageBox.Show("数据输入有误,请检查.", "未成功保存");
        }

        private void buttonSaveEquipment_Click(object sender, EventArgs e)
        {
            carinfor.equipmentConfigInfdata equipconfig = configini.getEquipConfigIni();
            if (comboBoxCgjxh.Text == "无")
                equipconfig.Cgjifpz = false;
            else
            {
                equipconfig.Cgjifpz = true;
                equipconfig.Cgjxh = comboBoxCgjxh.Text;
                equipconfig.Cgjck = comboBoxCgjck.Text;
            }
            if (comboBoxFqyxh.Text == "无")
                equipconfig.Fqyifpz = false;
            else
            {
                equipconfig.Fqyifpz = true;
                equipconfig.Fqyxh = comboBoxFqyxh.Text;
                equipconfig.Fqyck = comboBoxFqyck.Text;
            }
            if (comboBoxYdjxh.Text == "无")
                equipconfig.Ydjifpz = false;
            else
            {
                equipconfig.Ydjifpz = true;
                equipconfig.Ydjxh = comboBoxYdjxh.Text;
                equipconfig.Ydjck = comboBoxYdjck.Text;
            }
            if (comboBoxLZYDJXH.Text == "无")
                equipconfig.Lzydjifpz = false;
            else
            {
                equipconfig.Lzydjifpz = true;
                equipconfig.Lzydjxh = comboBoxLZYDJXH.Text;
                equipconfig.Lzydjck = comboBoxLZYDJCK.Text;
            }
            if (comboBoxLljxh.Text == "无")
                equipconfig.Lljifpz = false;
            else
            {
                equipconfig.Lljifpz = true;
                equipconfig.Lljxh = comboBoxLljxh.Text;
                equipconfig.Lljck = comboBoxLljck.Text;
            }
            if (comboBoxLEDck.Text == "未配置")
                equipconfig.Ledifpz = false;
            else
            {
                equipconfig.Ledifpz = true;
                equipconfig.Ledck = comboBoxLEDck.Text;
            }
            equipconfig.Xce100ck = comboBoxCOMXCE.Text;
            equipconfig.Xce100Comstring =comboBoxXCECOMSTRING.Text;
            equipconfig.Ledxh = comboBoxLEDXH.Text;
            equipconfig.LedComstring = comboBoxLEDCOMSTRING.Text;
            equipconfig.BpqXh = comboBoxBPQXH.Text;
            equipconfig.BpqCom = comboBoxBPQCOM.Text;
            equipconfig.BpqDy = int.Parse(comboBoxBPQDY.Text);
            equipconfig.BpqMethod = comboBoxBPQFS.Text;
            equipconfig.HeatFan = int.Parse(comboBoxFAN.Text);
            equipconfig.EmergencyStop = int.Parse(comboBoxEmergency.Text);
            equipconfig.Fqyckpzz = comboBoxFqyckppz.Text;
            equipconfig.Ydjckpzz = comboBoxYdjckppz.Text;
            equipconfig.Lljckpzz = comboBoxLljckpzz.Text;
            equipconfig.TempInstrument = comboBoxTemp.Text;
            equipconfig.DisplayMethod = comboBoxDisplayMethod.Text;
            equipconfig.DriverDisplay = comboBoxDriverScreen.SelectedIndex;
            equipconfig.FqyDelayTime = int.Parse(textBoxFqyxysj.Text);
            equipconfig.LljDelayTime = int.Parse(textBoxLLjO2xysj.Text);
            equipconfig.LugdownWdsj = int.Parse(textBoxLugdownWdsj.Text);
            equipconfig.TestStandard = comboBoxHJT.Text;
            try
            {
                if (comboBoxCgjlx.SelectedIndex == 0)
                {
                    ini.INIIO.WritePrivateProfileString("DIW", "DIWLX", "轻型车", @".\detectConfig.ini");
                }
                else
                {
                    ini.INIIO.WritePrivateProfileString("DIW", "DIWLX", "轻型车", @".\detectConfig.ini");
                }
                ini.INIIO.WritePrivateProfileString("DIW", "DIWMP", textBoxDIWMP.Text, @".\detectConfig.ini");
                ini.INIIO.WritePrivateProfileString("废气仪标定", "PEF", textBoxPEF.Text, @".\detectConfig.ini");
            }
            catch
            { }
            if (configini.writeEquipmentConfig(equipconfig))
                MessageBox.Show("保存成功.", "系统提示");
            else
                MessageBox.Show("数据输入有误,请检查.", "未成功保存");
        }

        private void comboBoxLugdownZsj_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBoxLugdownZsj.Text != "废气仪" && comboBoxLugdownZsj.Text != "烟度计")
            {
                comboBoxLugdownZsjCom.Enabled = true;
            }
            else
            {
                comboBoxLugdownZsjCom.Enabled = false;
            }
        }

        private void comboBoxSdsZsj_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBoxSdsZsj.Text != "废气仪" && comboBoxSdsZsj.Text != "烟度计")
            {
                comboBoxSdsZsjCom.Enabled = true;
            }
            else
            {
                comboBoxSdsZsjCom.Enabled = false;
            }
        }

        private void comboBoxBtgZsj_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBoxBtgZsj.Text != "废气仪" && comboBoxBtgZsj.Text != "烟度计")
            {
                comboBoxBtgZsjCom.Enabled = true;
            }
            else
            {
                comboBoxBtgZsjCom.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.Delete(@".\detectConfig.ini");
            File.Copy(@"D:\exhaustTest\detectConfigBk.ini",@".\detectConfig.ini");
            initConfigInfo();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.Control && e.KeyCode == Keys.M)
            {
                buttonAsmSave.Visible = !buttonAsmSave.Visible;
                buttonVmasSave.Visible = !buttonVmasSave.Visible;
                buttonSdsSave.Visible = !buttonSdsSave.Visible;
                buttonLugdownSave.Visible = !buttonLugdownSave.Visible;
                buttonBtgSave.Visible = !buttonBtgSave.Visible;
                button1.Visible = !button1.Visible;
                panelASMSETTINGS.Enabled = !panelASMSETTINGS.Enabled;
                panelSDSSETTINGS.Enabled = !panelSDSSETTINGS.Enabled;
                panelVMASSETTINGS.Enabled = !panelVMASSETTINGS.Enabled;
                panelJZJS.Enabled = !panelJZJS.Enabled;
                panelBTG.Enabled = !panelBTG.Enabled;
                panel29.Enabled = !panel29.Enabled;

                panelASM.Visible = !panelASM.Visible;
                panelVMAS.Visible = !panelVMAS.Visible;
                panelSDS.Visible = !panelSDS.Visible;
            }
        }

        private void buttonSaveEquipment_Click_1(object sender, EventArgs e)
        {
            carinfor.equipmentConfigInfdata equipconfig = configini.getEquipConfigIni();
            if (comboBoxCgjxh.Text == "无")
                equipconfig.Cgjifpz = false;
            else
            {
                equipconfig.Cgjifpz = true;
                equipconfig.Cgjxh = comboBoxCgjxh.Text;
                equipconfig.Cgjck = comboBoxCgjck.Text;
            }
            if (comboBoxFqyxh.Text == "无")
                equipconfig.Fqyifpz = false;
            else
            {
                equipconfig.Fqyifpz = true;
                equipconfig.Fqyxh = comboBoxFqyxh.Text;
                equipconfig.Fqyck = comboBoxFqyck.Text;
            }
            if (comboBoxYdjxh.Text == "无")
                equipconfig.Ydjifpz = false;
            else
            {
                equipconfig.Ydjifpz = true;
                equipconfig.Ydjxh = comboBoxYdjxh.Text;
                equipconfig.Ydjck = comboBoxYdjck.Text;
                equipconfig.IsOldMqy200 = checkBoxOldMqy200.Checked;
            }
            if (comboBoxLZYDJXH.Text == "无")
                equipconfig.Lzydjifpz = false;
            else
            {
                equipconfig.Lzydjifpz = true;
                equipconfig.Lzydjxh = comboBoxLZYDJXH.Text;
                equipconfig.Lzydjck = comboBoxLZYDJCK.Text;
                equipconfig.lzydjadd = textBoxLZYDJADD.Text;
            }
            if (comboBoxLljxh.Text == "无")
                equipconfig.Lljifpz = false;
            else
            {
                equipconfig.Lljifpz = true;
                equipconfig.Lljxh = comboBoxLljxh.Text;
                equipconfig.Lljck = comboBoxLljck.Text;
            }
            if (comboBoxYhyxh.Text == "无")
                equipconfig.isYhyPz = false;
            else
            {
                equipconfig.isYhyPz = true;
                equipconfig.YhyXh = comboBoxYhyxh.Text;
                equipconfig.YhyCk = comboBoxYhyck.Text;
                equipconfig.YhjCkpz = comboBoxYhyckpz.Text;
            }
            if (comboBoxLEDck.Text == "未配置")
                equipconfig.Ledifpz = false;
            else
            {
                equipconfig.Ledifpz = true;
                equipconfig.Ledck = comboBoxLEDck.Text;
            }
            if (comboBoxNOxXh.Text == "无")
                equipconfig.NOxifpz = false;
            else
            {
                equipconfig.NOxifpz = true;
                equipconfig.NOxXh = comboBoxNOxXh.Text;
                equipconfig.NOxCk = comboBoxNOxCk.Text;
            }
            equipconfig.LEDTJPH = int.Parse(comboBoxLEDTJPH.Text);
            equipconfig.ledrow1 = int.Parse(comboBoxLEDROW1.Text);
            equipconfig.ledrow2 = int.Parse(comboBoxLEDROW2.Text);
            equipconfig.BrakePWM = int.Parse(textBoxBrakePWM.Text);
            equipconfig.YdjL = int.Parse(textBoxYdjL.Text);
            equipconfig.Xce100ck = comboBoxCOMXCE.Text;
            equipconfig.Xce100Comstring = comboBoxXCECOMSTRING.Text;
            equipconfig.Ledxh = comboBoxLEDXH.Text;
            equipconfig.LedComstring = comboBoxLEDCOMSTRING.Text;
            equipconfig.BpqXh = comboBoxBPQXH.Text;
            equipconfig.BpqCom = comboBoxBPQCOM.Text;
            equipconfig.BpqDy = int.Parse(comboBoxBPQDY.Text);
            equipconfig.BpqMethod = comboBoxBPQFS.Text;
            equipconfig.HeatFan = int.Parse(comboBoxFAN.Text);
            equipconfig.EmergencyStop = int.Parse(comboBoxEmergency.Text);
            equipconfig.cgjckpzz = comboBoxCgjckpzz.Text;
            equipconfig.Fqyckpzz = comboBoxFqyckppz.Text;
            equipconfig.Ydjckpzz = comboBoxYdjckppz.Text;
            equipconfig.Lljckpzz = comboBoxLljckpzz.Text;
            equipconfig.NOxCkpz = comboBoxNOxCkpz.Text;
            equipconfig.Lzckpzz = comboBoxLZCKPZZ.Text;
            equipconfig.TempInstrument = comboBoxTemp.Text;
            equipconfig.DisplayMethod = comboBoxDisplayMethod.Text;
            equipconfig.DriverDisplay = comboBoxDriverScreen.SelectedIndex;
            equipconfig.displayJudge = checkBoxDisplayJudge.Checked;
            equipconfig.DriverFbl = comboBoxDriveFbl.SelectedIndex;
            equipconfig.FqyDelayTime = int.Parse(textBoxFqyxysj.Text);
            equipconfig.LljDelayTime = int.Parse(textBoxLLjO2xysj.Text);
            equipconfig.Lljllmy = double.Parse(textBoxLLjLlmyz.Text);
            equipconfig.LugdownWdsj = int.Parse(textBoxLugdownWdsj.Text);
            equipconfig.TestStandard = comboBoxHJT.Text;
            equipconfig.WorkAutomaticMode = (comboBoxWorkMode.SelectedIndex == 0);
            equipconfig.Tmqxh = comboBoxTMQXH.Text;
            equipconfig.Tmqck = comboBoxTMQCK.Text;
            equipconfig.Tmqckpz = comboBoxTMQPZ.Text;
            equipconfig.isTpTempInstrument = checkBoxTPWSD.Checked;
            equipconfig.CgjNz = comboBoxCgjNz.SelectedIndex;
            equipconfig.CarGdChanel = int.Parse(comboBoxCarGd.Text);
            equipconfig.isIgbtContainGdyk = checkBoxGdyk.Checked;
            equipconfig.isFqyNhSelfUse = checkBoxIsFqyNhSelfUse.Checked;
            equipconfig.isYdjNhSelfUse = checkBoxIsYdjNhSelfUse.Checked;
            equipconfig.useWeightWCF = checkBoxUseWeightWCF.Checked;
            equipconfig.WeightWCFaddress = textBoxWeightWCF.Text;
            equipconfig.useJHJK = checkBoxJHJK.Checked;
            equipconfig.JHLBGLB = double.Parse(textBoxJHLBGLYJZ.Text);
            equipconfig.JHLAMBDAMIN = double.Parse(textBoxJHLAMBDAMIN.Text);
            equipconfig.JHLAMBDAMAX = double.Parse(textBoxJHLAMBDAMAX.Text);
            equipconfig.DATASECONDS_TYPE = comboBoxDataSecondsType.Text;
            equipconfig.cd_fqy = checkBoxCD_FQY.Checked;
            equipconfig.cd_ydj = checkBoxCD_YDJ.Checked;
            //equipconfig.ZzyEnable = comboBoxZZYXH.Text == "未配置";
            //equipconfig.ZzyXh = comboBoxZZYXH.Text;
            //equipconfig.ZzyCk = comboBoxZZYCK.Text;
            //equipconfig.ZzyCkpzz = comboBoxZZYCKPZ.Text;
            //equipconfig.LeftZzChanel = int.Parse(comboBoxLEFTZZCHANEL.Text);
            //equipconfig.RightZzChanel = int.Parse(comboBoxRIGHTZZCHANEL.Text);
            //equipconfig.ZzGdChanel = int.Parse(comboBoxZzGd.Text);
            //equipconfig.ZzSureStyle = comboBoxZZSURESTYLE.SelectedIndex;
            try
            {
                if (comboBoxCgjlx.SelectedIndex == 0)
                {
                    ini.INIIO.WritePrivateProfileString("DIW", "DIWLX", "轻型车",Application.StartupPath+@"\detectConfig.ini");
                }
                else
                {
                    ini.INIIO.WritePrivateProfileString("DIW", "DIWLX", "重型车", Application.StartupPath + @"\detectConfig.ini");
                }
                ini.INIIO.WritePrivateProfileString("DIW", "DIWMP", textBoxDIWMP.Text, Application.StartupPath + @"\detectConfig.ini");
                ini.INIIO.WritePrivateProfileString("废气仪标定", "PEF", textBoxPEF.Text, Application.StartupPath + @"\detectConfig.ini");
            }
            catch
            { }

            if (configini.writeEquipmentConfig(equipconfig))
                MessageBox.Show("保存成功.", "系统提示");
            else
                MessageBox.Show("数据输入有误,请检查.", "未成功保存");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            carinfor.DynConfigInfdata btgconfig = configini.getDynConfigIni();
            try
            {
                btgconfig.Zsj = comboBoxDynZsj.Text;
                btgconfig.Zsjck = comboBoxDynZsjCk.Text;
                btgconfig.cczs = int.Parse(textBoxDynZs.Text);
                btgconfig.stableTime = int.Parse(textBoxDynZsTime.Text);
                btgconfig.forceTime = int.Parse(textBoxDynForceTime.Text);
                btgconfig.FuelforceTime = int.Parse(textBoxFuelForceTime.Text);
                btgconfig.FuelSdQj = float.Parse(textBoxFuelSdQj.Text);
                btgconfig.DynSdqj = float.Parse(textBoxDynSdqj.Text);
                btgconfig.DynVwSdqj= float.Parse(textBoxDynVwSdqj.Text);
                btgconfig.FuelForceQj = float.Parse(textBoxFuelForceQj.Text);
                btgconfig.DynForceQj = float.Parse(textBoxDynForceQj.Text);
                btgconfig.DynYkStyle = radioButtonDynZdyk.Checked ? 0 : 1;
                btgconfig.DynWjyktd = int.Parse(comboBoxDynYkTd.Text);
                btgconfig.DynReStartStyle = radioButtonDynReStartFrom60.Checked ? 0 : 1;
                btgconfig.DynUseGddw = checkBoxDynGddw.Checked;
                btgconfig.DynGddwTime = int.Parse(textBoxDynGddwTime.Text);
                btgconfig.DynMaxWaitTime = int.Parse(textBoxDynMaxWaitTime.Text);
                btgconfig.DynFuelJk = checkBoxDynJkYh.Checked;
                btgconfig.DynFuelXz = double.Parse(textBoxDynYhxz.Text);
                btgconfig.DynUseYkdw = checkBoxDynYkdw.Checked;
                btgconfig.FuelTestFjjs = int.Parse(textBoxFuelTestFjcs.Text);
                btgconfig.DynYkKey = textBoxDynYkKey.Text;
                btgconfig.DynFileStyle = radioButtonDynFileJson.Checked?0:1;
                btgconfig.DynStopUnstable = checkBoxDynStopUnstable.Checked;
                btgconfig.DynUnstableTime = int.Parse(textBoxDynUnstableTime.Text);
                btgconfig.DynTl = checkBoxDYNTL.Checked;
                btgconfig.DynBackTest = checkBoxDynBackTest.Checked;
                btgconfig.DynCO2Test = checkBoxDynCO2Test.Checked;
                btgconfig.DynFlowBack = checkBoxDynFlowBack.Checked;
                btgconfig.DynYkqr = checkBoxDynYkqr.Checked;
                if (configini.writeDynConfigIni(btgconfig))
                    MessageBox.Show("保存成功.", "系统提示");
                else
                    MessageBox.Show("数据输入有误,请检查.", "未成功保存");
            }
            catch
            {
                MessageBox.Show("数据输入有误,请检查.", "未成功保存");
            }
        }

        private void comboBoxDynZsj_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxDynZsj.Text != "废气仪" && comboBoxDynZsj.Text != "烟度计")
            {
                comboBoxDynZsjCk.Enabled = true;
            }
            else
            {
                comboBoxDynZsjCk.Enabled = false;
            }
        }
    }
}
