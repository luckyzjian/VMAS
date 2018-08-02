using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SYS.Model;

namespace SYS_DAL
{
    public class Main_dal
    {
        #region 测试数据库连接状态
        /// <summary>
        /// 测试数据库连接状态
        /// </summary>
        /// <returns>bool</returns>
        public bool DB_Link_Test()
        {
            if (DBHelperSQL.DB_Link_Test())
                return true;
            else
                return false;
        }
        #endregion

      
    }
}
