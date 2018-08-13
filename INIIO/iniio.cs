using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.IO;
using System.Data;
using System.Xml;


namespace ini
{
    public static class INIIO
    {
        /// <summary>
        /// 写配置文件
        /// </summary>
        /// <param name="strAppName">段名</param>
        /// <param name="strKeyName">字段名</param>
        /// <param name="strString">内容</param>
        /// <param name="strFileName">文件路径包括名字</param>
        /// <returns>bool</returns>
        [DllImport("Kernel32.dll")]
        public static extern bool WritePrivateProfileString(string strAppName, string strKeyName, string strString, string strFileName);//写配置文件（段名，字段，字段值，路径）

        /// <summary>
        /// 以字符串形式读配置文件
        /// </summary>
        /// <param name="strAppName">段名</param>
        /// <param name="strKeyName">字段名</param>
        /// <param name="strDefault">默认值</param>
        /// <param name="sbReturnString">StringBuilder</param>
        /// <param name="nSize">StringBuilder大小</param>
        /// <param name="strFileName">文件路径包括名字</param>
        /// <returns>int</returns>
        [DllImport("Kernel32.dll")]
        public static extern int GetPrivateProfileString(string strAppName, string strKeyName, string strDefault, StringBuilder sbReturnString, int nSize, string strFileName);//读配置文件 string（段名，字段，默认值，保存的strbuilder，大小，路径）
        
        /// <summary>
        /// 以int形式读配置文件
        /// </summary>
        /// <param name="strAppName">段名</param>
        /// <param name="strKeyName">字段名</param>
        /// <param name="nDefault">默认值</param>
        /// <param name="strFileName">文件路径包括名字</param>
        /// <returns>int</returns>
        [DllImport("Kernel32.dll")]
        public static extern int GetPrivateProfileInt(string strAppName, string strKeyName, int nDefault, string strFileName);//读配置文件 int（段名，字段，默认值，路径）
        public static string startpath = AppDomain.CurrentDomain.BaseDirectory;
        public static bool isSaveLog = false;
        #region 创建文件夹
        public static bool createDir(string strPath)
        {
            try
            {
                //strPath = @strPath.Trim().ToString();
                Directory.CreateDirectory(strPath);
                return true;
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.Write(exp.Message.ToString());
                return false;
            }
        }
        #endregion
        #region 于文件夹下创建子文件
        public static bool createFile(string strPath, string filename)
        {
            try
            {
                //strPath = @strPath.Trim().ToString();
                string newPath = Path.Combine(strPath, filename);
                if (!File.Exists(newPath))
                {
                    File.Create(newPath);
                }
                Directory.CreateDirectory(strPath);
                return true;
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.Write(exp.Message.ToString());
                return false;
            }
        }
        #endregion
        #region 删除文件夹下所有文件
        public static bool deleteDir(string strPath)
        {
            try
            {
                //strPath = @strPath.Trim().ToString();
                if (Directory.Exists(strPath))
                {
                    string[] strDirs = Directory.GetDirectories(strPath);
                    string[] strFiles = Directory.GetFiles(strPath);
                    foreach (string strFile in strFiles)
                    {
                        File.Delete(strFile);
                    }
                    foreach (string strdir in strDirs)
                    {
                        Directory.Delete(strdir, true);
                    }
                }
                return true;
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.Write(exp.Message.ToString());
                return false;
            }
        }
        #endregion
        public static bool saveLogInf(string inf)
        {
            if (isSaveLog)
            {
                string filepath = startpath + "\\Xclog\\" + DateTime.Now.ToString("yyMMdd");
                string pathname = filepath + "\\" + DateTime.Now.ToString("HH") + "report.log";
                if (createDir(filepath))
                {
                    StreamWriter log = new StreamWriter(pathname, true);
                    log.WriteLine("time:" + System.DateTime.Now.ToLongTimeString() + " content:" + inf);
                    log.Close();
                }
            }
            return true;
        }
        public static bool saveSocketLogInf(string inf)
        {
            if (isSaveLog)
            {
                string filepath = ".\\Socketlog\\" + DateTime.Now.ToString("yyMMdd");
                string pathname = filepath + "\\" + DateTime.Now.ToString("HH") + "report.log";
                if (createDir(filepath))
                {
                    StreamWriter log = new StreamWriter(pathname, true);
                    log.WriteLine("time:" + System.DateTime.Now.ToLongTimeString() + " content:" + inf);
                    log.Close();
                }
            }
            return true;
        }
        public static bool saveXmlInf(string inf, string filename)
        {
            if (isSaveLog)
            {
                string filepath = ".\\Xmllog\\" + DateTime.Now.ToString("yyMMdd");
                string pathname = filepath + "\\" + filename + ".xml";
                if (createDir(filepath))
                {
                    StreamWriter log = new StreamWriter(pathname, true);
                    log.WriteLine(inf);
                    log.Close();
                }
            }
            return true;
        }
        public static bool saveJsonResultToFile(string jsonstring, string filename)
        {
            string filepath = @"C:\jcdatatxt";
            string pathname = filepath + "\\" + filename + ".ini";
            if (createDir(filepath))
            {
                if (File.Exists(pathname)) File.Delete(pathname);
                StreamWriter log = new StreamWriter(pathname, true);
                log.WriteLine(jsonstring);
                log.Close();
            }
            return true;
        }

    }
}
