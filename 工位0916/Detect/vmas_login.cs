using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SYS_DAL;
using SYS.Model;

namespace Detect
{
    public partial class vmas_login : Form
    {
        public vmas_login()
        {
            InitializeComponent();
        }

        private void saveVmasLogin()
        {
            SYS.Model.BJCLXXB model = new SYS.Model.BJCLXXB();
            if (!check_IsRight(out model))
            {
                MessageBox.Show("带\"*\"项为必填项,请填写完整后再提交", "系统提示");
            }
            else
            {
                if (CarWait.bjclxx.Have_BjclInDaijian(model.JCCLPH))
                {
                    //MessageBox.Show("该车已经在待检序列中,是否要更新该车信息", "系统提示");
                    if (MessageBox.Show("该车已经在待检序列中,是否要更新该车信息？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        CarWait.bjclxx.Update(model);
                        MessageBox.Show("更新成功", "系统提示");
                    }
                }
                else
                {
                    if (CarWait.bjclxx.Savedate(model))
                    {
                        //ref_WaitCar();
                        MessageBox.Show("保存成功,检测编号为" + model.JCBH + ".", "系统提示");
                    }

                }
            }
        }
        #region 检查输入是否正确
        private bool check_IsRight(out SYS.Model.BJCLXXB model)
        {
            model = new BJCLXXB();
            if (textBoxPlateNumber.Text == "" || comboBoxMobileType.Text == "" || textBoxFarameNumber.Text == "" || textBoxWeight.Text == ""
                || textBoxMostWeight.Text == "" || comboBoxFuelType.Text == "" || comboBoxFuelSupplyType.Text == "" || comboBoxGreenFlag.Text == "" || dateEditRegisterData.Text == "")
            {

                return false;
            }
            else
            {
                DateTime dt = DateTime.Now;
                string nowtime = dt.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").Trim();
                model.JCBH = nowtime; //用注册的时间做为被检车辆的编号,检测完了再重新定义检测编号存储到相应的数据库中
                //model.JCBH = jczNumber + "-" + jcxxxb.JCXBH.ToString() + "-" + jcxxxb.LJSYS.ToString();
                model.PZLX = comboBoxMobileColor.Text;//牌照颜色
                model.JCCLPH = textBoxPlateNumber.Text;//牌照号码
                model.CLXHBH = textBoxMobileNumber.Text;//车辆型号
                model.JCCS = 0;//检测次数
                //model.LSH=;//
                model.CCRQ = Convert.ToDateTime(dateEditProductDate.Text.Replace('/', '-'));//生产日期
                model.DJRQ = Convert.ToDateTime(dateEditRegisterData.Text.Replace('/', '-'));//登记日期
                model.FDJH = textBoxEngineNumber.Text;//发动机号
                model.CJH = textBoxFarameNumber.Text;//车架号
                model.CZ = textBoxOwnerName.Text;//车主
                model.CZDH = textBoxOwnerTel.Text;//车主电话
                model.CZDZ = textBoxOwnerAdd.Text;//车主地址
                if (textBoxOdometer.Text != "")
                    model.LCBDS = Convert.ToInt32(textBoxOdometer.Text);//里程表读数
                else
                    model.LCBDS = 0;
                model.HBBZ = comboBoxGreenFlag.Text;//环保标志
                model.SYQK = comboBoxInUse.Text;//使用情况
                if (comboBox_detectLine.Text.Trim() == "" || comboBox_detectLine.Text.Trim() == "本线检测")
                {
                    model.JCBJ = CarWait.jcxxxb.JCXBH.ToString();//默认为本线检测
                }//检测线号
                else if (comboBox_detectLine.Text.Trim() == "优先")
                {
                    model.JCBJ = "-1";
                }

                model.JCZT = "请上测功机检测";//检测状态
                model.QRJCFF = comboBoxJcff.Text;//检测方法
                model.RYLX = comboBoxFuelType.Text;
                model.GYFF = comboBoxFuelSupplyType.Text;
                model.BSXLX = comboBoxGearbox.Text;
                model.CLLX = comboBoxMobileType.Text;
                model.ZBZL = textBoxWeight.Text;
                model.ZDZZL = textBoxMostWeight.Text;
                model.FDJPL = textBoxEnginePlacement.Text;
                model.FDJSCS = textBoxEngineManufacture.Text;
                model.JCZH = CarWait.jczNumber;
                model.DCZZ = textBoxAxleLoad.Text;
                model.FDJEDGL = textBoxEnginePower.Text;
                model.FDJEDZS = textBoxEngineSpeed.Text;
                if (textBoxPipeCount.Text != "")
                    model.PQGSL = Convert.ToInt32(textBoxPipeCount.Text);
                else
                    model.PQGSL = 0;
                model.PFBZ = "地方标准";
                model.ZZCS = textBoxMobileManufacture.Text;
                if (comboBoxCylinderCount.Text != "")
                    model.QGS = Convert.ToInt32(comboBoxCylinderCount.Text);
                else
                    model.QGS = 0;
                model.QDXS = comboBoxDriveStyle.Text;
                model.QDLQY = textBoxPneumaticWheel.Text;
                model.JCYH = textBoxInspectorNumber.Text;
                if (textBoxGearCount.Text != "")
                    model.DWS = Convert.ToInt32(textBoxGearCount.Text);
                else
                    model.DWS = 0;
                if (textBoxHdzk.Text != "")
                    model.HDZK = Convert.ToInt32(textBoxHdzk.Text);
                else
                    model.HDZK = 0;
                //model.JQXS = comboBoxJqxs.Text;
                return true;
            }

        }
        #endregion
    }
}
