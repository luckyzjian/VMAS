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
using System.Threading;
using System.Management;
using System.Net.Sockets;
using System.IO;
using System.Net;

namespace VMAS1._0
{
    public partial class Jiancexian : Form
    {
        public delegate void wtlsb(Label Msgowner, string Msgstr, bool Update_DB);      //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                     //委托
        public delegate void wtcs(Control controlname, string text);                    //委托
        public string[] selectID = new string[1024];                                    //当前等待车辆选中的列表
        public bool ref_zt = true;                                                      //dataGrid_waitcar_SelectionChanged_1函数使能标记
        public static string ip;                                                        //本机IP                                      
        public static BJCLXXB bjcl = null;                                              //被检车辆信息Model
        public static JCXGXXB jcxg = null;                                              //检测相关数据
        DataTable dt_wait = null;                                                         //等待车辆列表
        public static BJCLXX bjclxx = new BJCLXX();                                     //被检车辆信息方法集dal
        public static JCXGXX jcxgxx = new JCXGXX();                                     //被检车辆信息方法集dal
        public static SBXX sbxx = new SBXX();                                           //设备信息方法集dal
        public static SYS_DAL.XXXXB xxxxbdal = new SYS_DAL.XXXXB();                     //消息信息方法集dal
        public static Main_dal maindal = new Main_dal();                                //Main窗体的方法集dal
        tool tool = new tool();                                                         //方法集
        JCXXX jcxxx = new JCXXX();                                                      //检测线方法集
        public static JCXXXB jcxxxb = new JCXXXB();                                     //检测线信息Model
        public static SYS.Model.SYSConfig sys = new SYS.Model.SYSConfig();              //系统配置Model
        public static SBXXB fqfxy = new SBXXB();                                        //本线所用废气分析仪信息
        public static SBXXB btgydj = new SBXXB();                                       //本线所用不透光仪
        public static SBXXB llj = new SBXXB();
        DataTable Jccl_temp = null;                                                     //检测车辆所有信息datatable
        public static Zyjsdal zyjsdal = new Zyjsdal();                                  //自由加速方法集dal
        public static ASMdal asmdal = new ASMdal();                                     //asm方法集dal
        public static JZJSdal jzjsdal = new JZJSdal();                                  //jzjs方法集dal
        public static SDSdal sdsdal = new SDSdal();                                     //sds方法集dal
        public static Thread TH_CGJ = null;                                             //测功机线程
        public static float CGJ_Jzgl = 0;                                               //测功机加载功率
        public static bool CGJ_Th_st = true;                                            //测功机线程状态
        public static int CGJ_CH = 1;                                                   //测功机通道
        public int Carwait_Scroll = 0;                                                  //待检车滚动条位置
        public static string Errorlist = "";                                              //错误串

       // public int Carwait_Scroll = 0;                                                  //待检车滚动条位置
        public string Online_Order = "";                                                //上线命令    
        public static String Customer = "无";                                           //客户端注释
        public Thread thread_TCP = null;                                                //上线线程
        public Thread thread_UPD = null;                                                //UDP监听线程
        public static int Port = 8001;                                                  //默认上线端口
        public static int UDP_Port_Send = 9999;                                         //默认UDP发送端口
        public static int UDP_Port = 9998;                                              //默认UDP接收端口
        public static String Host = "192.168.1.2";                                      //默认主控端地址
        public TcpClient Tcp_Client;                                                    //TCP连接
        public TcpListener Lis;                                                         //TCP监听
        public UdpClient UDP_Client;                                                    //UDP监听
        public UdpClient UDP_Client_Send;                                               //UDP发送
        public NetworkStream Ns;                                                        //网络流
        public Socket socket;                                                           //套接字
        public static IPEndPoint ap;                                                    //IP……shishi
        //public static string Errorlist = "";                                              //错误串
        public static bool Init_Flag = false;                                           //系统初始化标记
        public static string Jcjsy = "";                                                //检测驾驶员
        public static string Jcczy = "";                                                //检测操作员
        public static string wd = "26";                                                 //温度
        public static string sd = "75";                                                 //湿度
        public static string dqy = "1";                                                 //大气压
        public static float Speed = 0;                                                  //车速
        public static float Force = 0;                                                  //扭矩
        public static float Power = 0;                                                  //功率
        public static float Duty = 0;                                                   //占空比
        public static string UseMK = "BNTD";                                            //使用什么模块
        public static string UseFqy = "FLA_502";                                        //使用什么型号的废气分析仪
        public static string UseLlj = "FLV_1000";
        public Jiancexian()
        {
            InitializeComponent();
        }
        private void CarWait_Load(object sender, EventArgs e)
        {
            ref_WaitCar();
        }
        private void Wait_Car_menu_Opening(object sender, CancelEventArgs e)
        {
            foreach (ToolStripMenuItem tsmi in Wait_Car_menu.Items)
                tsmi.Enabled = false;
            刷新ToolStripMenuItem.Enabled = true;
            if (dataGrid_waitcar.SelectedRows.Count > 0)
            {
                ToolStripMenuItemqx.Enabled = true;
            }
            
        }
        private void dataGrid_waitcar_MouseEnter(object sender, EventArgs e)
        {
            ref_WaitCar();
            ref_Select();
        }
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ref_WaitCar();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ref_WaitCar();
        }
        public void ref_WaitCar()
        {
            try
            {
                dt_wait = new DataTable();
                dt_wait.Columns.Add("检测线编号");
                dt_wait.Columns.Add("检测线IP地址");
                dt_wait.Columns.Add("检测线名称");
                dt_wait.Columns.Add("测功机名称");
                dt_wait.Columns.Add("测功机厂家");
                dt_wait.Columns.Add("废气仪名称");
                dt_wait.Columns.Add("废气仪厂家");
                dt_wait.Columns.Add("烟度计名称");
                dt_wait.Columns.Add("烟度计厂家");
                dt_wait.Columns.Add("流量计名称");
                dt_wait.Columns.Add("流量计厂家");
                dt_wait.Columns.Add("检测线安装公司");
                dt_wait.Columns.Add("认证编号");
                DataTable dt = jcxxx.getAllJiancexian();
                //DataTable dt = bjclxx.getAllCarWait();
                DataRow dr = null;
                if (dt != null)
                {
                    foreach (DataRow dR in dt.Rows)
                    {
                        dr = dt_wait.NewRow();
                        dr["检测线编号"] = dR["JCXBH"].ToString();
                        dr["检测线名称"] = dR["JCXMC"].ToString();
                        dr["检测线IP地址"] = dR["GYJSJIP"].ToString();
                        if (dR["DPCGJBH"].ToString() != "0")
                        {
                            int cgjbh = 0;
                            SYS.Model.SBXXB cgjsb = null;
                            if (int.TryParse(dR["DPCGJBH"].ToString(), out cgjbh))
                                cgjsb = sbxx.Get_sb_by_bh(cgjbh);
                            dr["测功机名称"] = cgjsb.SBMC;
                            dr["测功机厂家"] = cgjsb.SCCJ;
                        }
                        else
                        {
                            dr["测功机名称"] = "未配置";
                            dr["测功机厂家"] = "未配置";
                        }
                        if (dR["FQFXYBH"].ToString() != "0")
                        {
                            int cgjbh = 0;
                            SYS.Model.SBXXB cgjsb = null;
                            if (int.TryParse(dR["FQFXYBH"].ToString(), out cgjbh))
                                cgjsb = sbxx.Get_sb_by_bh(cgjbh);
                            dr["废气仪名称"] = cgjsb.SBMC;
                            dr["废气仪厂家"] = cgjsb.SCCJ;
                        }
                        else
                        {
                            dr["废气仪名称"] = "未配置";
                            dr["废气仪厂家"] = "未配置";
                        }
                        if (dR["BTGYDJBH"].ToString() != "0")
                        {
                            int cgjbh = 0;
                            SYS.Model.SBXXB cgjsb = null;
                            if (int.TryParse(dR["BTGYDJBH"].ToString(), out cgjbh))
                                cgjsb = sbxx.Get_sb_by_bh(cgjbh);
                            dr["烟度计名称"] = cgjsb.SBMC;
                            dr["烟度计厂家"] = cgjsb.SCCJ;
                        }
                        else
                        {
                            dr["烟度计名称"] = "未配置";
                            dr["烟度计厂家"] = "未配置";
                        }
                        if (dR["LLJBH"].ToString() != "0")
                        {
                            int cgjbh = 0;
                            SYS.Model.SBXXB cgjsb = null;
                            if (int.TryParse(dR["LLJBH"].ToString(), out cgjbh))
                                cgjsb = sbxx.Get_sb_by_bh(cgjbh);
                            dr["流量计名称"] = cgjsb.SBMC;
                            dr["流量计厂家"] = cgjsb.SCCJ;
                        }
                        else
                        {
                            dr["流量计名称"] = "未配置";
                            dr["流量计厂家"] = "未配置";
                        }
                        dr["检测线安装公司"] = dR["ZZCS"].ToString();
                        dr["认证编号"] = dR["RZBH"].ToString();
                        dt_wait.Rows.Add(dr);
                    }
                }
                ref_zt = false;
                dataGrid_waitcar.DataSource = dt_wait;
                dataGrid_waitcar.FirstDisplayedScrollingRowIndex = Carwait_Scroll;
                dataGrid_waitcar.Sort(dataGrid_waitcar.Columns["检测线编号"], ListSortDirection.Ascending);
                ref_zt = true;
            }
            catch (Exception)
            {

            }
        }
        public void ref_Select()
        {
            ref_zt = false;
            for (int i = 0; i < dataGrid_waitcar.Rows.Count; i++)
            {
                foreach (string str in selectID)
                {
                    if (str != null)
                        if (dataGrid_waitcar.Rows[i].Cells["检测线编号"].Value.ToString() == str.Substring(0, str.Length - 1) && dataGrid_waitcar.Rows[i].Cells["检测次数"].Value.ToString() == str.Substring(str.Length - 1))
                        {
                            dataGrid_waitcar.Rows[i].Selected = true;
                        }
                }
            }
            ref_zt = true;
        }

        private void dataGrid_waitcar_Scroll(object sender, ScrollEventArgs e)
        {
            Carwait_Scroll = e.NewValue;
        }

        private void dataGrid_waitcar_SelectionChanged_1(object sender, EventArgs e)
        {
            if (ref_zt)
            {
                if (dataGrid_waitcar.SelectedRows.Count > 0)
                {
                    if (dataGrid_waitcar.SelectedRows.Count == 1)
                    {
                        //ToolStripMenuItem_fc.Enabled = true;
                        ToolStripMenuItemqx.Enabled = true;
                        selectID = new string[1024];
                        for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                        {
                            selectID[i] = dataGrid_waitcar.SelectedRows[i].Cells["检测线编号"].Value.ToString();
                        }
                    }
                    else if (dataGrid_waitcar.SelectedRows.Count > 1)
                    {
                        //ToolStripMenuItem_fc.Enabled = false;
                        ToolStripMenuItemqx.Enabled = true;
                        selectID = new string[1024];
                        for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                        {
                            selectID[i] = dataGrid_waitcar.SelectedRows[i].Cells["检测线编号"].Value.ToString();
                        }
                    }
                    else
                    {
                        //ToolStripMenuItem_fc.Enabled = false;
                        selectID = new string[1024];
                        for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                        {
                            selectID[i] = dataGrid_waitcar.SelectedRows[i].Cells["检测线编号"].Value.ToString();
                        }
                    }
                }
                else
                {
                    selectID = new string[1024];
                    for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                    {
                        selectID[i] = dataGrid_waitcar.SelectedRows[i].Cells["检测线编号"].Value.ToString();
                    }
                }
            }
        }

        private void deleteSelectedCars(object sender, EventArgs e)
        {
            if (dataGrid_waitcar.SelectedRows.Count > 0)
            {
                for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                {
                    jcxxxb = jcxxx.GetModelbyJcxbh(dataGrid_waitcar.SelectedRows[i].Cells["检测线编号"].Value.ToString());
                    jcxxx.deleteThisLine(jcxxxb.JCXBH);
                }
                ref_WaitCar();
            }
        }
        #region 输入字符合法性验证
        /// <summary>
        /// 输入字符合法性验证
        /// </summary>
        /// <param name="Ct">TextBox类型控件 名称</param>
        /// <param name="Regex">string 正则表达式(请勿带长度限制)</param>
        /// <param name="extent">int 允许输入的长度</param>
        /// <param name="Correct">bool 是否修正</param>
        /// <returns>bool 有错返回true</returns>
        public static bool Text_Verification(TextBox Ct, string Regex, int extent, bool Correct)
        {
            bool flag = false;
            try
            {
                if (Ct is TextBox)
                {
                    if (Ct.Text.Length > 0)
                    {
                        for (int i = Ct.Text.Length - 1; i >= 0; i--)
                        {
                            // 利用正則表達式，其中 \u4E00-\u9fa5 表示中文
                            if (!System.Text.RegularExpressions.Regex.IsMatch(Ct.Text.Substring(i, 1), Regex) || Ct.Text.Length > extent)
                            {
                                if (Correct)
                                    Ct.Text = Ct.Text.Remove(i, 1);
                                flag = true;
                            }
                        }
                        Ct.SelectionStart = Ct.Text.Length;
                    }
                }
            }
            catch (Exception)
            {
            }
            return flag;
        }

        /// <summary>
        /// 输入字符合法性验证
        /// </summary>
        /// <param name="Ct">ComboBox类型控件 名称</param>
        /// <param name="Regex">string 正则表达式(请勿带长度限制)</param>
        /// <param name="extent">int 允许输入的长度</param>
        /// <param name="Correct">bool 是否修正</param>
        /// <returns>bool 有错返回true</returns>
        public static bool Text_Verification(ComboBox Ct, string Regex, int extent, bool Correct)
        {
            bool flag = false;
            try
            {
                if (Ct is ComboBox)
                {
                    if (Ct.Text.Length > 0)
                    {
                        for (int i = Ct.Text.Length - 1; i >= 0; i--)
                        {
                            // 利用正則表達式，其中 \u4E00-\u9fa5 表示中文
                            if (!System.Text.RegularExpressions.Regex.IsMatch(Ct.Text.Substring(i, 1), Regex) || Ct.Text.Length > extent)
                            {
                                if (Correct)
                                    Ct.Text = Ct.Text.Remove(i, 1);
                                flag = true;
                            }
                        }
                        Ct.SelectionStart = Ct.Text.Length;
                    }
                }
            }
            catch (Exception)
            {
            }
            return flag;
        }
        #endregion


    }
}
