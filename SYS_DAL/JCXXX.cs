using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SYS_DAL;
using SYS.Model;
using System.Data.SqlClient;
using System.Data;
using SYS;

namespace SYS_DAL
{
    public partial class JCXXX
    {
        #region 用本机IP获取本检测线信息
        /// <summary>
        /// 用本机IP获取本检测线信息
        /// </summary>
        /// <param name="IP">本机IP</param>
        /// <returns>JCXXXB Model</returns>
        public jcxztCheck GetModel(string jcxh)
        {
            jcxztCheck model = new jcxztCheck();
            string sql = "select * from JCXZT where JCXH=@jcxh";
            SqlParameter[] spr ={
                                    new SqlParameter("@jcxh",jcxh)
                                };
            try
            {
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text, spr);
                if (dt.Rows.Count > 0)
                {

                    model.JCXH = dt.Rows[0]["JCXH"].ToString();
                    model.Jcxmc = dt.Rows[0]["JCXMC"].ToString();
                    model.Jcxzt = dt.Rows[0]["JCXZT"].ToString();
                    model.Jcxcl = dt.Rows[0]["JCXCL"].ToString();
                    model.Cszt = dt.Rows[0]["CSZT"].ToString();
                    model.Csff = dt.Rows[0]["CSFF"].ToString();
                    model.Sj1 = dt.Rows[0]["SJ1"].ToString();
                    model.Sj2 = dt.Rows[0]["SJ2"].ToString();
                    model.Sj3 = dt.Rows[0]["SJ3"].ToString();
                    model.Sj4 = dt.Rows[0]["SJ4"].ToString();
                    model.Sj5 = dt.Rows[0]["SJ5"].ToString();
                    model.Jsbz = dt.Rows[0]["JSBZ"].ToString();
                    
                }
                else
                    model.JCXH = "-2";       //当服务器上没有找到本线时，本线编号置为-2，以免为0
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }
        #endregion
        #region 修改检测线的累计试验数(IP,累计试验次数)
        /// <summary>
        /// 修改检测线的累计试验数
        /// </summary>
        public int updateJcxLjsycs(string IP, int ljsycs)
        {
            string sql = "update JCXXXB set LJSYS=" + "'" + ljsycs + "'" + " where GYJSJIP=" + "'" + IP + "'";
            int rows = DBHelperSQL.Execute(sql);
            return rows;

        }
        #endregion
        #region 修改检测线的状态(检测线号,新的状态)
        /// <summary>
        /// 修改检测线的状态
        /// </summary>
        public int updateJcxztByJcxh(string jcxh, string newjcxzt)
        {
            string sql = "update JCXZT set JCXZT=" + "'" + newjcxzt + "'" + " where JCXH=" + "'" + jcxh + "'";
            int rows = DBHelperSQL.Execute(sql);
            return rows;

        }
        #endregion
        #region 修改检测线的检测车辆(检测线号,新的车辆)
        /// <summary>
        /// 修改检测线的状态
        /// </summary>
        public int updateJcxcltByJcxh(string jcxh, string newjcxcl)
        {
            string sql = "update JCXZT set JCXCL=" + "'" + newjcxcl + "'" + " where JCXH=" + "'" + jcxh + "'";
            int rows = DBHelperSQL.Execute(sql);
            return rows;

        }
        #endregion
        #region 修改检测线的测试状态(检测线号,新的测试状态)
        /// <summary>
        /// 修改检测线的状态
        /// </summary>
        public int updateCsztByJcxh(string jcxh, string newcszt)
        {
            string sql = "update JCXZT set CSZT=" + "'" + newcszt + "'" + " where JCXH=" + "'" + jcxh + "'";
            int rows = DBHelperSQL.Execute(sql);
            return rows;

        }
        #endregion
        #region 修改检测线的测试方法(检测线号,新的测试方法)
        /// <summary>
        /// 修改检测线的状态
        /// </summary>
        public int updateCsffByJcxh(string jcxh, string newcsff)
        {
            string sql = "update JCXZT set CSFF=" + "'" + newcsff + "'" + " where JCXH=" + "'" + jcxh + "'";
            int rows = DBHelperSQL.Execute(sql);
            return rows;

        }
        #endregion
        #region 修改检测线的数据(检测线号,新的数据)
        /// <summary>
        /// 修改检测线的状态
        /// </summary>
        public int updateDataByJcxh(string jcxh, string newsj1,string newsj2,string newsj3,string newsj4,string newsj5)
        {
            string sql = "update JCXZT set SJ1=" + "'" + newsj1 + "'" + ",SJ2=" + "'" + newsj2 + "'" + ",SJ3=" + "'" + newsj3 + "'" + ",SJ4=" + "'" + newsj4 + "'" + ",SJ5=" + "'" + newsj5 + "'" + " where JCXH=" + "'" + jcxh + "'";
            int rows = DBHelperSQL.Execute(sql);
            return rows;

        }
        #endregion
        #region 修改检测线的结束标志(检测线号,结束标志)
        /// <summary>
        /// 修改检测线的状态
        /// </summary>
        public int updateJsbzByJcxh(string jcxh, string newjsbz)
        {
            string sql = "update JCXZT set JSBZ=" + "'" + newjsbz + "'" + " where JCXH=" + "'" + jcxh + "'";
            int rows = DBHelperSQL.Execute(sql);
            return rows;

        }
        #endregion
        #region 获取所有检测线的信息
        /// <summary>
        /// 获取所有检测线的信息
        /// </summary>
        public DataTable getAllJiancexian()
        {
            string sql = "select * from JCXZT";
            DataTable dt = null;
            try
            {
                dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                return dt;
            }
            catch
            {
                throw;
            }
        }
        #endregion
        #region 获取所有检测线的信息
        /// <summary>
        /// 获取所有检测线的信息
        /// </summary>
        public bool checkDatabase()
        {
            string sql = "select * from JCXZT";
            DataTable dt = null;
            try
            {
                dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                if (dt != null)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
                //throw;
            }
        }
        #endregion
        public DataTable requerydata(string sql)
        {
            DataTable dt = null;
            try
            {
                dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                return dt;
            }
            catch
            {
                throw;
            }
        }
    }
}


