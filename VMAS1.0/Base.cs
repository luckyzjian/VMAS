using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.Win32;
using System.Configuration;
using System.Collections;

namespace VMAS1._0
{
    class Base
    {
        //创建单例静态变量
        private static Base _sharedBaseSingleton;

        /// <summary>
        /// 建立Base的单例模型
        /// </summary>
        /// <returns></returns>
        public static Base sharedInstance()
        {
            if (_sharedBaseSingleton == null)
                _sharedBaseSingleton = new Base();
            return _sharedBaseSingleton;
        }

        protected Base() { }

        #region 判断当前进程是否运行数据库
        /// <summary>
        ///1、 判断该系统是否安装有数据库
        ///2、判断SQL数据库是否打开
        /// </summary>
        /// <returns>
        /// 当返回值为0时，表示系统没有安装数据库
        /// 当返回值为2时，表示数据库没有运行
        /// 当返回值为1时，表示数据库运行正常
        /// </returns>
        public int judgeIsStartDatabase()
        {
            //判断是否安装数据库
            RegistryKey pregkey;
            pregkey = Registry.LocalMachine.OpenSubKey(@"software\microsoft\microsoft SQL Server", true);
            if (pregkey == null)
            {
                return 0;
            }
            //判断是否开启数据库
            Process[] processes = Process.GetProcesses();
            foreach (Process prc in processes)
            {
                if ("sqlservr" == prc.ProcessName)
                {
                    return 1;
                }
            }
            return 2;
        }
        #endregion

        #region 判断数据库中是否存在记录
        /// <summary>
        /// 判断数据库中是否存在记录
        /// </summary>
        /// <param name="_dataBaseString">数据库链接串</param>
        /// <param name="sqlString">sql查询语句</param>
        /// <returns>当返回值大于0时，数据库中存在记录；否则记录不存在</returns>
        public int judgeDataIsExist(string _dataBaseString,string sqlString)
        {
            try
            {
                SqlConnection conn = new SqlConnection(_dataBaseString);
                SqlCommand queryCommand = new SqlCommand(sqlString, conn);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    //查询数据库中有多少行
                    int number = (int)queryCommand.ExecuteScalar();
                    conn.Close();
                    return number;
                }
                
            }
            catch
            {

            }

            return -1;
        }
        #endregion
        #region 判断数据库中是否存在记录
        /// <summary>
        /// 判断数据库中是否存在该辆车
        /// </summary>
        /// <param name="_dataBaseString">数据库链接串</param>
        /// <param name="sqlString">sql查询语句</param>
        /// <returns>当返回值大于0时，数据库中存在记录；否则记录不存在</returns>
        public Boolean judgeMobileIsExist(string _mobilePlate)
        {
            string dataBasestring = ConfigurationSettings.AppSettings["mobileCharacter"].ToString();
            string queryString = "select count(*) from mobileCharacter where 车牌号码='" + _mobilePlate + "'";
            if (Base.sharedInstance().judgeDataIsExist(dataBasestring, queryString) > 0)
            {
                return true;
            }
            return false;
        }
        
    }
}
#endregion