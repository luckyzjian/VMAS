﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Detect
{
    public partial class JzjsLogin : Form
    {
        public JzjsLogin()
        {
            InitializeComponent();
        }
        public SYS_DAL.BJCLXX bjclxx = new SYS_DAL.BJCLXX();
        public SYS_DAL.JCXXX jcxxx = new SYS_DAL.JCXXX();
        #region 初始化检测线选项
        private void init_jcx()
        {
            DataTable dt = jcxxx.getAllJiancexian();
            //DataTable dt = bjclxx.getAllCarWait();
            DataRow dr = null;
            if (dt != null)
            {
                comboBox_detectLine.Items.Add("优先");
                foreach (DataRow dR in dt.Rows)
                {
                    comboBox_detectLine.Items.Add(dR["JCXBH"].ToString() + "——" + dR["JCXMC"].ToString());
                }
                comboBox_detectLine.SelectedIndex = 0;
            }

        }
        #endregion
        public void save_bjcl()
        {
            SYS.Model.BJCLXXB model = new SYS.Model.BJCLXXB();
            if (!check_IsRight(out model))
            {
                //MessageBox.Show("带\"*\"项为必填项,请填写完整后再提交", "系统提示");
            }
            else
            {
                if (bjclxx.Have_BjclInDaijian(model.JCCLPH))
                {
                    //MessageBox.Show("该车已经在待检序列中,是否要更新该车信息", "系统提示");
                    if (MessageBox.Show("该车已经在待检序列中,是否要更新该车信息？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        bjclxx.Update(model);
                        MessageBox.Show("更新成功", "系统提示");
                    }
                }
                else
                {
                    if (bjclxx.Savedate(model))
                    {
                        MessageBox.Show("保存成功,检测编号为" + model.JCBH + ".", "系统提示");
                    }

                }

            }
        }
        #region 检查输入是否正确
        private bool check_IsRight(out SYS.Model.BJCLXXB model)
        {
            model = new SYS.Model.BJCLXXB();
            if (textBoxPlateNumber.Text == "" || comboBoxMobileType.Text == "" || textBoxFarameNumber.Text == "" || textBoxMostWeight.Text == "" || dateEditRegisterData.Text == ""||textBoxEnginePower.Text==""||textBoxEngineSpeed.Text=="")
            {
                MessageBox.Show("车辆基本信息填写不完整，请完整填写", "系统提示");
                return false;
            }
            else if (dateEditProductDate.Text == "" || comboBoxInUse.Text == ""||comboBoxJqxs.Text=="")
            {
                MessageBox.Show("检测信息中带*号为必填项，请完整填写", "系统提示");
                return false;
            }
            else
            {
                model.JCCLPH = textBoxPlateNumber.Text;//牌照号码
                model.PZLX = comboBoxMobileColor.Text;//牌照颜色
                model.CLLX = comboBoxMobileType.Text;//车辆类型
                model.CLXHBH = textBoxMobileNumber.Text;//车辆型号
                model.ZZCS = textBoxMobileManufacture.Text;//汽车制造商
                model.ZBZL = "未录入";//基准质量
                model.ZDZZL = textBoxMostWeight.Text;//最大总质量
                model.CJH = textBoxFarameNumber.Text;//车架号
                model.FDJH = textBoxEngineNumber.Text;//发动机号
                model.FDJPL = textBoxEnginePlacement.Text;//发动机排量
                model.RYLX = "未录入";//燃料类型
                model.FDJEDZS = textBoxEngineSpeed.Text;//发动机额定转速
                if (comboBoxCylinderCount.Text != "")
                    model.QGS = Convert.ToInt32(comboBoxCylinderCount.Text);//气缸数
                else
                    model.QGS = 0;
                model.FDJEDGL = textBoxEnginePower.Text;//发动机额定功率
                model.GYFF = "未录入";//供油方式
                model.BSXLX = "未录入";//变速箱类型
                model.HBBZ = "未录入";//环保标志
                if (textBoxOdometer.Text != "")
                    model.LCBDS = Convert.ToInt32(textBoxOdometer.Text);//里程表读数
                else
                    model.LCBDS = 0;
                model.CCRQ = Convert.ToDateTime(dateEditProductDate.Text.Replace('/', '-'));//生产日期
                model.HDZK = 0;
                model.DJRQ = Convert.ToDateTime(dateEditRegisterData.Text.Replace('/', '-'));//登记日期
                model.FDJSCS = textBoxEngineManufacture.Text;//发动机制造商
                //车主信息
                model.CZ = textBoxOwnerName.Text;//车主
                model.CZDH = textBoxOwnerTel.Text;//车主电话
                model.CZDZ = textBoxOwnerAdd.Text;//车主地址
                //检测站信息
                model.JCZH = CarWait.jczxxb.JCZBH;//检测站编号
                DateTime dt = DateTime.Now;
                string nowtime = dt.GetDateTimeFormats('s')[0].ToString().Replace("-", "").Replace("T", "").Replace(" ", "").Replace(":", "").Trim();//登记编号
                model.JCBH = nowtime; //用注册的时间做为被检车辆的编号,检测完了再重新定义检测编号存储到相应的数据库中
                model.JCCS = 0;//检测次数
                model.SYQK = comboBoxInUse.Text;//使用情况
                if (comboBox_detectLine.Text.Trim() != "")//检测线号
                {
                    if (comboBox_detectLine.Text.Trim() == "优先")
                        model.JCBJ = "-1";
                    else
                        model.JCBJ = comboBox_detectLine.Text.Trim().Split(new string[] { "——" }, StringSplitOptions.RemoveEmptyEntries)[0];
                }//检测线号
                else
                {
                    MessageBox.Show("请选择检测线", "系统提示");
                    return false;
                }
                model.JCZT = "请上测功机检测";//检测状态
                model.QRJCFF = "加载减速法";//检测方法
                model.PQGSL = 0;
                model.PFBZ = "未录入";//催化转化器情况
                model.QDLQY = "未录入";//驱动轮气压
                model.JCYH = textBoxInspectorNumber.Text;//检测员工号
                model.QDXS = "";//驱动形式
                model.JQXS = comboBoxJqxs.Text;//进气形式
                model.DWS = 0;//档位数
                model.DCZZ = "";//单车轴重
                return true;
            }

        }
        #endregion

        private void Login_Load(object sender, EventArgs e)
        {
            groupBox_carbt.Enabled = false;
            groupBox_carxt.Enabled = false;
            groupBox_cz.Enabled = false;
            groupBox_engine.Enabled = false;
            groupBox_test.Enabled = false;
            init_jcx();
            textBoxStationNumber.Text = CarWait.jczxxb.JCZBH;
        }
        private void enable_group()
        {
            groupBox_carbt.Enabled = true;
            groupBox_carxt.Enabled = true;
            groupBox_cz.Enabled = true;
            groupBox_engine.Enabled = true;
            groupBox_test.Enabled = true;
        }

        private void simpleButtonCheckIsExist_Click(object sender, EventArgs e)
        {
            if (textBoxPlateNumber.Text == "")
            {
                MessageBox.Show("请输入车牌号再进行查询!", "系统提示");
            }
            else if (textBoxPlateNumber.Text == "新车")
            {
                MessageBox.Show("初次检测车辆,请输入车辆信息.", "信息提示");
                enable_group();
            }
            else
            {
                string jcclcp = textBoxPlateNumber.Text;
                if (ref_LoginPanel(jcclcp))
                {
                    MessageBox.Show("查询成功!", "系统提示");
                    enable_group();
                }
                else
                {
                    MessageBox.Show("初次检测车辆,请输入车辆信息.", "信息提示");
                    enable_group();
                }


            }
        }
        #region 根据车牌号来显示登记信息
        public bool ref_LoginPanel(string jcclcp)
        {

            SYS.Model.BJCLXXB model = new SYS.Model.BJCLXXB();
            DataTable dt = bjclxx.Get_Carxx_byph(jcclcp);
            if (dt.Rows.Count > 0)
            {
                textBoxPlateNumber.Text = dt.Rows[0]["JCCLPH"].ToString();//登记编号
                comboBoxMobileColor.Text = dt.Rows[0]["PZLX"].ToString();//牌照类型
                textBoxMobileNumber.Text = dt.Rows[0]["CLXHBH"].ToString();//车辆型号
                dateEditRegisterData.Text = dt.Rows[0]["DJRQ"].ToString().Split(new char[] { ' ' })[0];//车辆登记日期
                dateEditProductDate.Text = dt.Rows[0]["CCRQ"].ToString().Split(new char[] { ' ' })[0];//车辆登记日期
                textBoxEngineNumber.Text = dt.Rows[0]["FDJH"].ToString(); //发动机号
                textBoxFarameNumber.Text = dt.Rows[0]["CJH"].ToString(); //车架号
                textBoxOwnerName.Text = dt.Rows[0]["CZ"].ToString(); //车主
                textBoxOwnerTel.Text = dt.Rows[0]["CZDH"].ToString(); //车主电话
                textBoxOwnerAdd.Text = dt.Rows[0]["CZDZ"].ToString(); //车主地址
                textBoxOdometer.Text = dt.Rows[0]["LCBDS"].ToString();//里程表读数
                comboBoxInUse.Text = dt.Rows[0]["SYQK"].ToString();//使用情况
                comboBoxMobileType.Text = dt.Rows[0]["CLLX"].ToString();//车辆类型
                textBoxMostWeight.Text = dt.Rows[0]["ZDZZL"].ToString();//最大总质量
                textBoxEnginePlacement.Text = dt.Rows[0]["FDJPL"].ToString();//发动机排量
                textBoxEngineManufacture.Text = dt.Rows[0]["FDJSCS"].ToString();//发动机生产商
                textBoxStationNumber.Text = dt.Rows[0]["JCZH"].ToString();//检测站号
                textBoxEnginePower.Text = dt.Rows[0]["FDJEDGL"].ToString();//发动机额定功率
                textBoxEngineSpeed.Text = dt.Rows[0]["FDJEDZS"].ToString();//发动机额定转速
                textBoxMobileManufacture.Text = dt.Rows[0]["ZZCS"].ToString();//车辆制造厂商
                comboBoxCylinderCount.Text = dt.Rows[0]["QGS"].ToString();//气缸数
                textBoxInspectorNumber.Text = dt.Rows[0]["JCYH"].ToString();//检测员工号
                comboBoxJqxs.Text = dt.Rows[0]["JQXS"].ToString();//核定载客数
                return true;
            }
            else
            {

                return false;

            }


        }
        #endregion

        private void simpleButtonSave_Click(object sender, EventArgs e)
        {
            save_bjcl();
        }

        private void textBoxMost_keyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (textBoxMostWeight.Text.Length < 8)
                {

                    bool IsContaintDot = this.textBoxMostWeight.Text.Contains(".");
                    if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
                    {
                        e.Handled = true;
                    }
                    else if (IsContaintDot && (e.KeyChar == 46))
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    MessageBox.Show("请核对数据后再输出,注意输入格式", "系统提示");
                    textBoxMostWeight.Text = "";
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("请核对数据后再输出,注意输入格式", "系统提示");
                textBoxMostWeight.Text = "";
                return;
            }
        }
        private void textBoxOwnerTel_keyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (textBoxOwnerTel.Text.Length < 15)
                {

                    //bool IsContaintDot = this.textBoxEngineSpeed.Text.Contains(".");
                    if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 45) && (e.KeyChar != 8))
                    {
                        e.Handled = true;
                    }

                }
                else
                {
                    MessageBox.Show("请按照正确格式输入座机(xxxx-xxxxxxx)或手机（xxxxxxxxxxx）号", "系统提示");
                    textBoxOwnerTel.Text = "";
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("请按照正确格式输入座机(xxxx-xxxxxxx)或手机（xxxxxxxxxxx）号", "系统提示");
                textBoxOwnerTel.Text = "";
                return;
            }

        }
        
    }
}
