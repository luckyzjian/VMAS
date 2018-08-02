using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace vmasDetect
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new VMAS());
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString(), "系统消息");
            }
        }
    }
}
