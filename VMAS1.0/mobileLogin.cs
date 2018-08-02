using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;

namespace VMAS1._0
{
    public partial class mobileLogin : Form
    {
        public mobileLogin()
        {
            InitializeComponent();
        }
        private int dataBaseStartValue = Base.sharedInstance().judgeIsStartDatabase();

        private string mobileDataBaseString = ConfigurationManager.AppSettings["mobileCharacter"].ToString();
         #region 读取数据库表中固定某一位置的数据
        /// <summary>
        /// 读取数据库表中固定某一位置的数据
        /// </summary>
        /// <param name="_databaseString">数据库连接串</param>
        /// <param name="_sqlString">sql查询语句</param>
        /// <param name="index">固定索引位置</param>
        /// <returns>数据库中某一字段的数据</returns>
        private ArrayList DatabaseQuery(string _databaseString, string _sqlString, int index)
        {
            ArrayList dataArray = new ArrayList();

            if (dataBaseStartValue == 0)
            {
                MessageBox.Show("数据库不存在，请安装SQL SERVER2000");
            }
            else if (dataBaseStartValue == 2)
            {
                MessageBox.Show("请运行数据库");
            }
            else if (dataBaseStartValue == 1)
            {
                try
                {
                    SqlConnection conn = new SqlConnection(_databaseString);
                    SqlCommand mysqlCommand = conn.CreateCommand();
                    mysqlCommand.CommandText = _sqlString;
                    conn.Open();
                    SqlDataReader reader = mysqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        dataArray.Add(reader[index].ToString());
                    }
                    conn.Close();
                }
                catch
                {
                    //dataArray = null;
                }
            }
            return dataArray;
        }
        #endregion

        #region 从电平及格式化编码表中读取数据
        /// <summary>
        /// 从电平及格式化编码表中读取数据
        /// </summary>
        /// <param name="_databaseString">数据库连接串</param>
        /// <param name="_projName">工程名</param>
        /// <param name="_testProj">测试项目</param>
        /// <returns>电平及格式化编码数据</returns>
        private string ReadDataFromLevelAndEncodeTable(string _databaseString, string _projName, string _testProj)
        {
            string levelString = "";
            string selectLevelEncodeString = "select VH,VL,CVA,CVB,encodeData from levelAndEncodeSet where projName = '" + _projName + "'and testProj = '" + _testProj + "'";
            if (dataBaseStartValue == 1)
            {
                try
                {
                    SqlConnection conn = new SqlConnection(_databaseString);
                    SqlCommand mysqlCommand = conn.CreateCommand();
                    mysqlCommand.CommandText = selectLevelEncodeString;
                    conn.Open();
                    SqlDataReader reader = mysqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        levelString = reader[0].ToString().Replace(" ", "") + ";" + reader[1].ToString().Replace(" ", "") + ";" + reader[2].ToString().Replace(" ", "") + ";" + reader[3].ToString().Replace(" ", "") + ";" + reader[4].ToString().Replace(" ", "");
                    }
                    conn.Close();
                }
                catch
                { 
                
                }
            }
            return levelString;
        }
        #endregion
        #region 向数据库中插入、更新、删除数据
        /// <summary>
        /// 向数据库中插入、更新、删除一行数据
        /// </summary>
        /// <param name="_databaseString">数据库连接串</param>
        /// <param name="_sqlString">要更新的sql语句</param>
        /// <returns>执行的行数，如果返回0则数据操作失败</returns>
        private int ExecuteSQLToDataBase(string _databaseString, string _sqlString)
        {
            int rowCount = 0;
            if (dataBaseStartValue == 1)
            {
                try
                {
                    SqlConnection conn = new SqlConnection(_databaseString);
                    SqlCommand mysqlCommand = conn.CreateCommand();
                    mysqlCommand.CommandText = _sqlString;
                    conn.Open();
                    rowCount = mysqlCommand.ExecuteNonQuery();
                    conn.Close();
                }
                catch { }
            }
            
            return rowCount;
        }
        #endregion
          private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string updateMobileSetString = "insert into mobileCharacter (车牌号码,车辆类型,车牌颜色,车辆型号,制造厂商,基准质量,最大总质量,车架号,发动机型号,发动机排量,发动机额定功率,发动机生产企业,气缸数,供油方式,变速箱类型,绿色环保标志,累计表读数,档位数,车辆登记日期,单车轴重,驱动方式,驱动轮气压,燃油规格,车主姓名,车主地址,车主电话,检测站编号,检测线编号) values ('" + textBoxPlateNumber.Text + "','" + comboBoxMobileType.Text + "','" + comboBoxMobileColor.Text + "','" +
                      textBoxMobileNumber.Text + "','" + textBoxMobileManufacture.Text + "','" + textBoxWeight.Text + "','" + textBoxMostWeight.Text + "','" + textBoxFarameNumber.Text + "','" + textBoxEngineNumber.Text + "','" + textBoxEnginePlacement.Text + "','" + textBoxEnginePower.Text + "','" + textBoxEngineManufacture.Text + "','" + comboBoxCylinderCount.Text + "','" + comboBoxFuelSupplyType.Text + "','" + comboBoxGearbox.Text + "','" + comboBoxGreenFlag.Text + "','"
                      +textBoxOdometer.Text+"','"+textBoxGearCount.Text+"','"+dateEditRegisterData.Text+"','"+textBoxAxleLoad.Text+"','"+comboBoxDriveStyle.Text+"','"+textBoxPneumaticWheel.Text+"','"+comboBoxFuelType.Text+"','"+textBoxOwnerName.Text+"','"+textBoxOwnerAdd.Text+"','"+textBoxOwnerTel.Text+"','"+textBoxStationNumber.Text+"','"+textBoxInspectorNumber.Text+"')";
            if (ExecuteSQLToDataBase(mobileDataBaseString, updateMobileSetString) < 1)
            {
                MessageBox.Show("保存车辆信息失败");
                return;
            }
            else
            {
                MessageBox.Show("车辆信息保存成功");
                return;
            }
        }

          private void simpleButtonCheckIsExist_Click(object sender, EventArgs e)
          {
              if (Base.sharedInstance().judgeMobileIsExist(textBoxPlateNumber.Text))
              {
                  string selectSQLString = "select 车辆类型,车牌颜色,车辆型号,制造厂商,基准质量,最大总质量,车架号,发动机型号,发动机排量,发动机额定功率,发动机生产企业,气缸数,供油方式,变速箱类型,绿色环保标志,累计表读数,档位数,车辆登记日期,单车轴重,驱动方式,驱动轮气压,燃油规格,车主姓名,车主地址,车主电话,检测站编号,检测线编号 from mobileCharacter where 车牌号码='" + textBoxPlateNumber.Text +"'";
                  SqlConnection conn = new SqlConnection(mobileDataBaseString);
                  SqlCommand mySqlCommand = conn.CreateCommand();
                  mySqlCommand.CommandText = selectSQLString;
                  conn.Open();
                  SqlDataReader reader = mySqlCommand.ExecuteReader();
                  while (reader.Read())
                  {
                      comboBoxMobileType.Text=reader[0].ToString().Replace(" ","");//车辆类型
                      comboBoxMobileColor.Text=reader[1].ToString().Replace(" ","");//车牌颜色
                      textBoxMobileNumber.Text=reader[2].ToString().Replace(" ","");//车辆型号
                      textBoxMobileManufacture.Text=reader[3].ToString().Replace(" ","");//制造厂商
                      textBoxWeight.Text=reader[4].ToString().Replace(" ","");//基准质量
                      textBoxMostWeight.Text=reader[5].ToString().Replace(" ","");//最大总质量
                      textBoxFarameNumber.Text=reader[6].ToString().Replace(" ","");//车架号
                      textBoxEngineNumber.Text=reader[7].ToString().Replace(" ","");//发动机型号
                      textBoxEnginePlacement.Text=reader[8].ToString().Replace(" ","");//发动机排量
                      textBoxEnginePower.Text = reader[9].ToString().Replace(" ", "");//发动机额定功率
                      textBoxEngineManufacture.Text = reader[10].ToString().Replace(" ", "");//发动机生产企业 
                      comboBoxCylinderCount.Text=reader[11].ToString().Replace(" ","");//气缸数
                      comboBoxFuelSupplyType.Text=reader[12].ToString().Replace(" ","");//供油方式
                      comboBoxGearbox.Text=reader[13].ToString().Replace(" ","");//变速箱类型
                      comboBoxGreenFlag.Text=reader[14].ToString().Replace(" ","");//绿色环保标志
                      textBoxOdometer.Text=reader[15].ToString().Replace(" ","");//累计表读数
                      textBoxGearCount.Text=reader[16].ToString().Replace(" ","");//档位数
                      dateEditRegisterData.Text=reader[17].ToString().Replace(" ","");//车辆登记日期
                      textBoxAxleLoad.Text=reader[18].ToString().Replace(" ","");//单车轴重
                      comboBoxDriveStyle.Text=reader[19].ToString().Replace(" ","");//驱动方式
                      textBoxPneumaticWheel.Text=reader[20].ToString().Replace(" ","");//驱动轮气压
                      comboBoxFuelType.Text=reader[21].ToString().Replace(" ","");//燃料类型
                      textBoxOwnerName.Text=reader[22].ToString().Replace(" ","");//车主姓名
                      textBoxOwnerAdd.Text=reader[23].ToString().Replace(" ","");//车主地址
                      textBoxOwnerTel.Text=reader[24].ToString().Replace(" ","");//车主电话
                      textBoxStationNumber.Text=reader[25].ToString().Replace(" ","");//检测站号
                      textBoxInspectorNumber.Text = reader[26].ToString().Replace(" ", "");//检测员号
                  }
                  conn.Close();
              }
              else
              {
                  MessageBox.Show("没有找到该车的相关信息");
              }
          }

          private void toolStripMenuItem2_Click(object sender, EventArgs e)
          {
              FormIg195Test formTest = new FormIg195Test();
              formTest.Show();
          }

          
    }

}
