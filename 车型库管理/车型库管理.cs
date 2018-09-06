using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using carinfor;

namespace 车型库管理
{
    public partial class 车型库管理 : Form
    {
        private Point panellocation = new Point(206, 3);
        private equipmentConfigInfdata equipdata = new equipmentConfigInfdata();
        private configIni configini = new configIni();
        public 车型库管理()
        {
            InitializeComponent();
        }

        private void 车型库管理_Load(object sender, EventArgs e)
        {
            reLoadTreeView();
            equipdata = configini.getEquipConfigIni();
        }
        private void reLoadTreeView()
        {
            treeView1.Nodes.Clear();
            DataTable dt = new DataTable();
            string expmsg;
            dt = readFromMdb("select distinct PC from [车型]", out expmsg);
            if (dt != null)
            {
                TreeNode root = new TreeNode();
                root.Text = "达标车型";
                treeView1.Nodes.Add(root);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TreeNode subNode = new TreeNode();
                    subNode.Text = "批次" + dt.Rows[i]["PC"].ToString();
                    root.Nodes.Add(subNode);
                }
                TreeNode searchnode = new TreeNode();
                searchnode.Text = "查询车型";
                treeView1.Nodes.Add(searchnode);
                TreeNode c1 = new TreeNode();
                c1.Text = "在用柴油客车参比值";
                treeView1.Nodes.Add(c1);
                TreeNode c2 = new TreeNode();
                c2.Text = "在用柴油货车参比值";
                treeView1.Nodes.Add(c2);
                TreeNode c3 = new TreeNode();
                c3.Text = "在用柴油半挂车汽车列车参比值";
                treeView1.Nodes.Add(c3);
                TreeNode c4 = new TreeNode();
                c4.Text = "燃油限值及加载力计算";
                treeView1.Nodes.Add(c4);

            }
        }
        private void showDataGridView(DataTable dt,DataGridView dtgrid,int lx)
        {

            dtgrid.DataSource = dt;
            dtgrid.RowHeadersVisible = false;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dtgrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    if (lx == 0)
                    {
                        dtgrid.Columns["CLXH"].HeaderText = "型号";
                        dtgrid.Columns["FDJXH"].HeaderText = "发动机型号";
                        dtgrid.Columns["QYMC"].HeaderText = "企业名称";
                        dtgrid.Columns["CPMC"].HeaderText = "产品名称";
                        dtgrid.Columns["WKCC"].HeaderText = "外廓尺寸";
                        dtgrid.Columns["HXCC"].HeaderText = "货箱尺寸";
                        dtgrid.Columns["ZBZL"].HeaderText = "整备质量";
                        dtgrid.Columns["ZZL"].HeaderText = "总质量";
                        dtgrid.Columns["QDXS"].HeaderText = "驱动型式";
                        dtgrid.Columns["LTGGXH"].HeaderText = "轮胎规格型号";
                        dtgrid.Columns["ZHYH"].HeaderText = "综合油耗";
                        dtgrid.Columns["YH50KZ"].HeaderText = "50km/h空载油耗";
                        dtgrid.Columns["YH50MZ"].HeaderText = "50km/h满载油耗";
                        dtgrid.Columns["YH60MZ"].HeaderText = "60km/h满载油耗";
                        dtgrid.Columns["PC"].HeaderText = "公布批次";
                        dtgrid.Columns["PZ"].HeaderText = "配置";
                    }
                    else if (lx == 1)
                    {
                        dtgrid.Columns["MINLENGTH"].HeaderText = "车长最小值";
                        dtgrid.Columns["MAXLENGTH"].HeaderText = "车长最大值";
                        dtgrid.Columns["HIGH"].HeaderText = "高级客车参比值";
                        dtgrid.Columns["MIDANDLOW"].HeaderText = "中级客车及低级客车参比值";
                    }
                    else if (lx == 2)
                    {
                        dtgrid.Columns["MINZZL"].HeaderText = "总质量最小值";
                        dtgrid.Columns["MAXZZL"].HeaderText = "总质量最大值";
                        dtgrid.Columns["XZ"].HeaderText = "参比值";
                    }
                    else if (lx == 3)
                    {
                        dtgrid.Columns["MINZZL"].HeaderText = "总质量最小值";
                        dtgrid.Columns["MAXZZL"].HeaderText = "总质量最大值";
                        dtgrid.Columns["XZ"].HeaderText = "参比值";
                    }
                }
            }
            dtgrid.Show();
        }
        private DataTable readFromMdb(string sqlstring,out string expmsg)
        {
            DataTable dt = new DataTable();
            expmsg = "";
            try
            {
                string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+Application.StartupPath+"/油耗车型库.mdb";
                OleDbConnection connection = new OleDbConnection(connectionString);
                connection.Open();
                OleDbCommand odCommand = connection.CreateCommand();
                OleDbTransaction Trans = connection.BeginTransaction();
                odCommand.Transaction = Trans;
                Trans.Rollback();
                try
                {
                    odCommand.CommandText = sqlstring;
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlstring, connection);
                    dataAdapter.Fill(dt);
                    dataAdapter.Dispose();
                }
                catch(Exception er)
                {
                    expmsg = er.Message;
                    Trans.Rollback();
                }
                connection.Close();
                return dt;
            }
            catch(Exception er)
            {
                expmsg = er.Message;
                MessageBox.Show("异常:" + expmsg);
                return null;
            }
        }
        private int deleteInMdb(string sqlstring, out string expmsg)
        {
            int deletedrows=0;
            expmsg = "";
            try
            {
                string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "/油耗车型库.mdb";
                OleDbConnection connection = new OleDbConnection(connectionString);
                connection.Open();
                OleDbCommand odCommand = connection.CreateCommand();
                OleDbTransaction Trans = connection.BeginTransaction();
                odCommand.Transaction = Trans;
                Trans.Rollback();
                try
                {
                    odCommand.CommandText = sqlstring;
                    deletedrows=odCommand.ExecuteNonQuery();
                    
                }
                catch (Exception er)
                {

                    expmsg = er.Message;
                    Trans.Rollback();
                }
                connection.Close();
                return deletedrows;
            }
            catch (Exception er)
            {
                expmsg = er.Message;
                return deletedrows;
            }
        }
        private int InsertToMdb(string sqlstring, out string expmsg)
        {
            int deletedrows = 0;
            expmsg = "";
            try
            {
                string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "/油耗车型库.mdb";
                OleDbConnection connection = new OleDbConnection(connectionString);
                connection.Open();
                OleDbCommand odCommand = connection.CreateCommand();
                OleDbTransaction Trans = connection.BeginTransaction();
                odCommand.Transaction = Trans;
                Trans.Rollback();
                try
                {
                    odCommand.CommandText = sqlstring;
                    deletedrows = odCommand.ExecuteNonQuery();

                }
                catch (Exception er)
                {

                    expmsg = er.Message;
                    Trans.Rollback();
                }
                connection.Close();
                return deletedrows;
            }
            catch (Exception er)
            {
                expmsg = er.Message;
                return deletedrows;
            }
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            DataTable dt = new DataTable();
            string expmsg;
            TreeNode selectedNode = treeView1.SelectedNode;
            if (selectedNode.Text == "查询车型")
            {
                panelAddXh.Visible = true;
                panelDataGrid.Visible = false;
                panelCaculate.Visible = false;
                panelAddXh.Location = panellocation;                
                showDataGridView(dt,dataGridView2,0);
            }
            else if (selectedNode.Text.Contains("批次"))
            {
                panelAddXh.Visible = false;
                panelDataGrid.Visible = true;
                panelCaculate.Visible = false;
                panelDataGrid.Location = panellocation;
                String PC = selectedNode.Text.Replace("批次", "");
                dt = readFromMdb("select * from [车型] where PC=" + PC, out expmsg);
                    showDataGridView(dt,dataGridView1,0);
            }
            else if (selectedNode.Text.Contains("客车"))
            {
                panelAddXh.Visible = false;
                panelDataGrid.Visible = true;
                panelCaculate.Visible = false;
                panelDataGrid.Location = panellocation;
                String PC = selectedNode.Text.Replace("批次", "");
                dt = readFromMdb("select * from [在用柴油客车参比值]", out expmsg);
                showDataGridView(dt, dataGridView1, 1);
            }
            else if (selectedNode.Text.Contains("货车"))
            {
                panelAddXh.Visible = false;
                panelDataGrid.Visible = true;
                panelCaculate.Visible = false;
                panelDataGrid.Location = panellocation;
                String PC = selectedNode.Text.Replace("批次", "");
                dt = readFromMdb("select * from [在用柴油货车参比值]", out expmsg);
                showDataGridView(dt, dataGridView1, 2);
            }
            else if (selectedNode.Text.Contains("半挂车"))
            {
                panelAddXh.Visible = false;
                panelDataGrid.Visible = true;
                panelCaculate.Visible = false;
                panelDataGrid.Location = panellocation;
                String PC = selectedNode.Text.Replace("批次", "");
                dt = readFromMdb("select * from [在用柴油半挂车参比值]", out expmsg);
                showDataGridView(dt, dataGridView1, 3);
            }
            else if (selectedNode.Text.Contains("计算"))
            {
                panelAddXh.Visible = false;
                panelDataGrid.Visible = false;
                panelCaculate.Visible = true;
                panelCaculate.Location = panellocation;
            }

        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reLoadTreeView();
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string expmsg;
            TreeNode selectedNode = treeView1.SelectedNode;
            string clxh = textBoxPpxh.Text;
            string fdjxh = textBoxFdjxh.Text;
            string pz = textBoxPZ.Text;
            if (clxh == "")
            {
                MessageBox.Show("必须输入车辆型号");
                return;
            }
            string sqlstring = "select * from [车型] where CLXH='" + clxh + "'";
            if(fdjxh!="")
                sqlstring+= " and FDJXH='" + fdjxh + "'";
            if (pz != "")
            {
                int pzint;
                if (int.TryParse(pz, out pzint))
                {
                    sqlstring += " and PZ=" + pz;
                }
                else
                {
                    MessageBox.Show("配置必须输入整数");
                    return;
                }
            }
            dt = readFromMdb(sqlstring,out expmsg);
            if (dt != null)
            {
                showDataGridView(dt,dataGridView2,0);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                int totaldeletedrows = 0;
                for (int i = 0; i < dataGridView2.SelectedRows.Count; i++)
                {
                    string expmsg;
                    string id = dataGridView2.SelectedRows[i].Cells["ID"].Value.ToString();
                    int deletedrows = deleteInMdb("delete from [车型] where ID=" + id, out expmsg);
                    totaldeletedrows += deletedrows;
                }
                MessageBox.Show("删除成功,共计删除"+totaldeletedrows+"行");
                buttonTest_Click(sender,e);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string clxh = textBoxPpxh.Text;
            string fdjxh = textBoxFdjxh.Text;
            string pz = textBoxPZ.Text;
            string qymc = textBoxQYMC.Text;
            string cpmc = "";
            string wkcc = textBoxLENGTH.Text + "" + textBoxWIDTH.Text + "" + textBoxHEIGHT.Text;
            string hxcc = "";
            string zbzl = "";
            string zzl = "";
            string qdxs = "";
            string ltggxh = textBoxLTGGXH.Text;
            string zhyh = "";
            string yh50kz = "";
            string yh50mz = textBoxYH50MZ.Text;
            string yh60mz = textBoxYH60MZ.Text;
            string pc = textBoxPC.Text;
            if (clxh == "")
            {
                MessageBox.Show("必须输入车辆型号");
                return;
            }

            int a;
            double b;
            if (!int.TryParse(pz, out a))
            {
                MessageBox.Show("配置必须输入整数");
                return;
            }
            if (!int.TryParse(pc, out a))
            {
                MessageBox.Show("批次必须输入整数");
                return;
            }
            if (!double.TryParse(yh50mz, out b))
            {
                MessageBox.Show("50km/h满载油耗必须输入有效数据");
                return;
            }
            if (!double.TryParse(yh60mz, out b))
            {
                MessageBox.Show("60km/h满载油耗必须输入有效数据");
                return;
            }
            string sqlstring = "insert into [车型] (CLXH,FDJXH,QYMC,WKCC,LTGGXH,YH50MZ,YH60MZ,PC,PZ) VALUES ('" 
                + clxh + "','"+fdjxh+"','" + qymc + "','" + wkcc + "','" + ltggxh + "','" + yh50mz + "','"
                + yh60mz + "'," + pc + "," + pz + ")";

            string expmsg;
            int deletedrows = InsertToMdb(sqlstring, out expmsg);
            MessageBox.Show("添加成功,共计添加" + deletedrows + "行");
            buttonTest_Click(sender, e);
        }

        yhcarInidata carinfo = new yhcarInidata();
        private void button1_Click(object sender, EventArgs e)
        {
            double a = 0;
            carinfo.检测流水号 = "";
            carinfo.检测类型 = 0;
            carinfo.车辆号牌 = textBoxPlate.Text;
            carinfo.号牌种类 = comboBoxPlateType.Text;
            carinfo.车辆型号 = textBoxCaculateXh.Text;
            carinfo.发动机型号 = textBoxCaculateFdjxh.Text;
            carinfo.燃料种类 = radioButtonCy.Checked ? 1 : 0;
            carinfo.加载力计算方式 = 1;
            if (textBoxEdzzl.Text == "")
            {
                MessageBox.Show("额定总质量不能为空", "提示");
                return;
            }
            else if (!double.TryParse(textBoxEdzzl.Text, out a))
            {
                MessageBox.Show("额定总质量非数字", "提示");
                return;
            }
            else
            {
                carinfo.额定总质量 = a;
            }
            if (textBoxQdkzzl.Text == "")
            {
                MessageBox.Show("驱动轴空载质量不能为空", "提示");
                return;
            }
            else if (!double.TryParse(textBoxQdkzzl.Text, out a))
            {
                MessageBox.Show("驱动轴空载质量非数字", "提示");
                return;
            }
            else
            {
                carinfo.驱动轴空载质量 = a;
            }
            if (carinfo.检测类型 == 0)
            {
                carinfo.汽车类型 = radioButtonKc.Checked ? 0 : 1;
                carinfo.油耗限值 = 0;
                carinfo.油耗检测加载力 = 0;
                carinfo.油耗检测工况速度 = 0;
                carinfo.驱动轴数 = int.Parse(comboBoxQdzs.Text);
                carinfo.轮胎类型 = comboBoxLtlx.SelectedIndex;
                if (carinfo.轮胎类型 == 0)
                {
                    if (textBoxZwtkd.Text == "")
                    {
                        MessageBox.Show("子午胎轮胎断面宽度不能为空", "提示");
                        return;
                    }
                    else if (!double.TryParse(textBoxZwtkd.Text, out a))
                    {
                        MessageBox.Show("子午胎轮胎断面宽度非数字", "提示");
                        return;
                    }
                    else
                    {
                        carinfo.子午胎轮胎断面宽度 = a;
                    }
                }
                else
                {
                    carinfo.子午胎轮胎断面宽度 = 0;
                }
                if (textBoxQlj.Text == "")
                {
                    MessageBox.Show("汽车前轮距不能为空", "提示");
                    return;
                }
                else if (!double.TryParse(textBoxQlj.Text, out a))
                {
                    MessageBox.Show("汽车前轮距非数字", "提示");
                    return;
                }
                else
                {
                    carinfo.汽车前轮距 = a;
                }
                if (textBoxQcgd.Text == "")
                {
                    MessageBox.Show("汽车高度不能为空", "提示");
                    return;
                }
                else if (!double.TryParse(textBoxQcgd.Text, out a))
                {
                    MessageBox.Show("汽车高度非数字", "提示");
                    return;
                }
                else
                {
                    carinfo.汽车高度 = a;
                }


                if (carinfo.汽车类型 == 0)
                {
                    carinfo.客车等级 = comboBoxKcdj.SelectedIndex;
                    if (carinfo.汽车类型 == 0)
                    {
                        if (textBoxKccc.Text == "")
                        {
                            MessageBox.Show("客车车长不能为空", "提示");
                            return;
                        }
                        else if (!double.TryParse(textBoxKccc.Text, out a))
                        {
                            MessageBox.Show("客车车长非数字", "提示");
                            return;
                        }
                        else
                        {
                            carinfo.客车车长 = a;
                        }
                    }
                    else
                    {
                        carinfo.客车车长 = 0;
                    }

                }
                else
                {
                    carinfo.货车车身型式 = comboBoxHccsxs.SelectedIndex;
                    if (carinfo.货车车身型式 == 2)
                    {
                        if (textBoxQycmzzzl.Text == "")
                        {
                            MessageBox.Show("牵引车满载总质量不能为空", "提示");
                            return;
                        }
                        else if (!double.TryParse(textBoxQycmzzzl.Text, out a))
                        {
                            MessageBox.Show("牵引车满载总质量非数字", "提示");
                            return;
                        }
                        else
                        {
                            carinfo.牵引车满载总质量 = a;
                        }
                    }
                    else
                    {
                        carinfo.牵引车满载总质量 = 0;
                    }
                }
            }
            else
            {

                carinfo.动力性检测加载力 = 0;
                carinfo.驱动轴数 = int.Parse(comboBoxQdzs.Text);
                carinfo.轮胎类型 = comboBoxLtlx.SelectedIndex;
                if (carinfo.燃料种类 == 0)
                {
                    if (textBoxEdnj.Text == "")
                    {
                        MessageBox.Show("额定扭矩不能为空", "提示");
                        return;
                    }
                    else if (!double.TryParse(textBoxEdnj.Text, out a))
                    {
                        MessageBox.Show("额定扭矩非数字", "提示");
                        return;
                    }
                    else
                    {
                        carinfo.点燃式额定扭矩 = a;
                    }
                    if (textBoxEdnjzs.Text == "")
                    {
                        MessageBox.Show("额定扭矩转速不能为空", "提示");
                        return;
                    }
                    else if (!double.TryParse(textBoxEdnjzs.Text, out a))
                    {
                        MessageBox.Show("额定扭矩转速非数字", "提示");
                        return;
                    }
                    else
                    {
                        carinfo.点燃式额定扭矩转速 = a;
                    }
                }
                else
                {
                    carinfo.是否危险货物运输车辆 = checkBoxIsDanger.Checked ? 1 : 0;
                    carinfo.压燃式功率参数类型 = comboBoxGlcslx.SelectedIndex;
                    if (textBoxEdgl.Text == "")
                    {
                        MessageBox.Show("额定功率不能为空", "提示");
                        return;
                    }
                    else if (!double.TryParse(textBoxEdgl.Text, out a))
                    {
                        MessageBox.Show("额定功率非数字", "提示");
                        return;
                    }
                    else
                    {
                        carinfo.压燃式额定功率 = a;
                    }
                }

            }
            double yhXz = 0;
            int xzyj = 0;
            if (getYhXz(out yhXz, out xzyj))
            {
                string xzyjstirng = "";
                switch(xzyj)
                {
                    case 0: xzyjstirng = "车型库匹配";break;
                    case 1: xzyjstirng = "表C.1、C.2匹配"; break;
                    case 2: xzyjstirng = "表C.3匹配"; break;
                    default:break;
                }
                textBoxXz.Text = yhXz.ToString("0.00") + "(" + xzyjstirng + ")";
            }
            else
            {
                textBoxXz.Text = "计算限值失败";
            }

        }
        private bool getYhXz(out double xz, out int xzyj)
        {
            DataTable dt;
            xz = 0; xzyj = 0;
            string msg;
            string sqlstring = "select * from [车型] where CLXH='" + carinfo.车辆型号 + "' and FDJXH='" + carinfo.发动机型号 + "'";
            dt = readFromMdb(sqlstring, out msg);
            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    if (carinfo.汽车类型 == 0 && carinfo.客车等级 == 0)
                    {
                        xz = Math.Round(1.14 * double.Parse(dt.Rows[0]["YH60MZ"].ToString()), 2);
                        xzyj = 0;
                        return true;
                    }
                    else
                    {
                        xz = Math.Round(1.14 * double.Parse(dt.Rows[0]["YH50MZ"].ToString()), 2);
                        xzyj = 0;
                        return true;
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show("按车型和发动机型号找到匹配项，但数据格式出现异常：" + er.Message);

                }
            }
            sqlstring = "select * from [车型] where CLXH='" + carinfo.车辆型号 + "'";
            dt = readFromMdb(sqlstring, out msg);
            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    if (carinfo.汽车类型 == 0 && carinfo.客车等级 == 0)
                    {
                        xz = Math.Round(1.14 * double.Parse(dt.Rows[0]["YH60MZ"].ToString()), 2);
                        xzyj = 0;
                        return true;
                    }
                    else
                    {
                        xz = Math.Round(1.14 * double.Parse(dt.Rows[0]["YH50MZ"].ToString()), 2);
                        xzyj = 0;
                        return true;
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show("按车型找到匹配项，但数据格式出现异常：" + er.Message);
                }
            }
            double k = 1.0;
            if (carinfo.燃料种类 == 0) k = 1.15;
            if (carinfo.汽车类型 == 0)
            {
                sqlstring = "select * from [在用柴油客车参比值] where int(MINLENGTH)<" + carinfo.客车车长 + " and int(MAXLENGTH)>=" + carinfo.客车车长;
                dt = readFromMdb(sqlstring, out msg);
                if (dt != null && dt.Rows.Count > 0)
                {
                    try
                    {
                        if (carinfo.客车等级 == 0)
                        {
                            xz = Math.Round(k * double.Parse(dt.Rows[0]["HIGH"].ToString()), 2);
                            xzyj = 1;
                            return true;
                        }
                        else
                        {
                            xz = Math.Round(k * double.Parse(dt.Rows[0]["MIDANDLOW"].ToString()), 2);
                            xzyj = 1;
                            return true;
                        }
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show("在【在用柴油客车参比值】中找到匹配项，但数据格式出现异常：" + er.Message);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("在【在用柴油客车参比值】中未找到匹配项，客车长度：" + carinfo.客车车长.ToString());
                    return false;
                }
            }
            else
            {
                if (checkBox牵引车满载总质量.Checked)
                {
                    sqlstring = "select * from [在用柴油半挂车参比值] where int(MINZZL)<" + carinfo.牵引车满载总质量 + " and int(MAXZZL)>=" + carinfo.牵引车满载总质量;
                    dt = readFromMdb(sqlstring, out msg);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        try
                        {
                            xz = Math.Round(k * double.Parse(dt.Rows[0]["XZ"].ToString()), 2);
                            xzyj = 2;
                            return true;
                        }

                        catch (Exception er)
                        {
                            MessageBox.Show("在【在用柴油半挂车参比值】中找到匹配项，但数据格式出现异常：" + er.Message);
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("在【在用柴油半挂车参比值】中未找到匹配项，牵引车满载总质量：" + carinfo.牵引车满载总质量.ToString());
                        return false;
                    }
                }
                else
                {
                    sqlstring = "select * from [在用柴油货车参比值] where int(MINZZL)<" + carinfo.额定总质量 + " and int(MAXZZL)>=" + carinfo.额定总质量;
                    dt = readFromMdb(sqlstring, out msg);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        try
                        {
                            xz = Math.Round(k * double.Parse(dt.Rows[0]["XZ"].ToString()), 2);
                            xzyj = 1;
                            return true;
                        }
                        catch (Exception er)
                        {
                            MessageBox.Show("在【在用柴油货车参比值】中找到匹配项，但数据格式出现异常：" + er.Message);
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("在【在用柴油货车参比值】中未找到匹配项，额定总质量：" + carinfo.额定总质量.ToString());
                        return false;
                    }
                }
            }
        }

        private void comboBoxHccsxs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxHccsxs.Text == "牵引车")
            {
                checkBox牵引车满载总质量.Enabled = true;
            }
            else
            {
                checkBox牵引车满载总质量.Enabled = false;
                checkBox牵引车满载总质量.Checked = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double a = 0;
            carinfo.检测流水号 = "";
            carinfo.检测类型 = 1;
            carinfo.车辆号牌 = textBoxPlate.Text;
            carinfo.号牌种类 = comboBoxPlateType.Text;
            carinfo.车辆型号 = textBoxCaculateXh.Text;
            carinfo.发动机型号 = textBoxCaculateFdjxh.Text;
            carinfo.燃料种类 = radioButtonCy.Checked ? 1 : 0;
            carinfo.加载力计算方式 = 1;
            if (textBoxEdzzl.Text == "")
            {
                MessageBox.Show("额定总质量不能为空", "提示");
                return;
            }
            else if (!double.TryParse(textBoxEdzzl.Text, out a))
            {
                MessageBox.Show("额定总质量非数字", "提示");
                return;
            }
            else
            {
                carinfo.额定总质量 = a;
            }
            if (textBoxQdkzzl.Text == "")
            {
                MessageBox.Show("驱动轴空载质量不能为空", "提示");
                return;
            }
            else if (!double.TryParse(textBoxQdkzzl.Text, out a))
            {
                MessageBox.Show("驱动轴空载质量非数字", "提示");
                return;
            }
            else
            {
                carinfo.驱动轴空载质量 = a;
            }
            if (carinfo.检测类型 == 0)
            {
                carinfo.汽车类型 = radioButtonKc.Checked ? 0 : 1;
                carinfo.油耗限值 = 0;
                carinfo.油耗检测加载力 = 0;
                carinfo.油耗检测工况速度 = 0;
                carinfo.驱动轴数 = int.Parse(comboBoxQdzs.Text);
                carinfo.轮胎类型 = comboBoxLtlx.SelectedIndex;
                if (carinfo.轮胎类型 == 0)
                {
                    if (textBoxZwtkd.Text == "")
                    {
                        MessageBox.Show("子午胎轮胎断面宽度不能为空", "提示");
                        return;
                    }
                    else if (!double.TryParse(textBoxZwtkd.Text, out a))
                    {
                        MessageBox.Show("子午胎轮胎断面宽度非数字", "提示");
                        return;
                    }
                    else
                    {
                        carinfo.子午胎轮胎断面宽度 = a;
                    }
                }
                else
                {
                    carinfo.子午胎轮胎断面宽度 = 0;
                }
                if (textBoxQlj.Text == "")
                {
                    MessageBox.Show("汽车前轮距不能为空", "提示");
                    return;
                }
                else if (!double.TryParse(textBoxQlj.Text, out a))
                {
                    MessageBox.Show("汽车前轮距非数字", "提示");
                    return;
                }
                else
                {
                    carinfo.汽车前轮距 = a;
                }
                if (textBoxQcgd.Text == "")
                {
                    MessageBox.Show("汽车高度不能为空", "提示");
                    return;
                }
                else if (!double.TryParse(textBoxQcgd.Text, out a))
                {
                    MessageBox.Show("汽车高度非数字", "提示");
                    return;
                }
                else
                {
                    carinfo.汽车高度 = a;
                }


                if (carinfo.汽车类型 == 0)
                {
                    carinfo.客车等级 = comboBoxKcdj.SelectedIndex;
                    if (carinfo.汽车类型 == 0)
                    {
                        if (textBoxKccc.Text == "")
                        {
                            MessageBox.Show("客车车长不能为空", "提示");
                            return;
                        }
                        else if (!double.TryParse(textBoxKccc.Text, out a))
                        {
                            MessageBox.Show("客车车长非数字", "提示");
                            return;
                        }
                        else
                        {
                            carinfo.客车车长 = a;
                        }
                    }
                    else
                    {
                        carinfo.客车车长 = 0;
                    }

                }
                else
                {
                    carinfo.货车车身型式 = comboBoxHccsxs.SelectedIndex;
                    if (carinfo.货车车身型式 == 2)
                    {
                        if (textBoxQycmzzzl.Text == "")
                        {
                            MessageBox.Show("牵引车满载总质量不能为空", "提示");
                            return;
                        }
                        else if (!double.TryParse(textBoxQycmzzzl.Text, out a))
                        {
                            MessageBox.Show("牵引车满载总质量非数字", "提示");
                            return;
                        }
                        else
                        {
                            carinfo.牵引车满载总质量 = a;
                        }
                    }
                    else
                    {
                        carinfo.牵引车满载总质量 = 0;
                    }
                }
            }
            else
            {

                carinfo.动力性检测加载力 = 0;
                carinfo.驱动轴数 = int.Parse(comboBoxQdzs.Text);
                carinfo.轮胎类型 = comboBoxLtlx.SelectedIndex;
                if (textBoxVeOrVm.Text == "")
                {
                    MessageBox.Show("达标车速不能为空", "提示");
                    return;
                }
                else if (!double.TryParse(textBoxVeOrVm.Text, out a))
                {
                    MessageBox.Show("达标车速非数字", "提示");
                    return;
                }
                else
                {
                    Vm = a;
                }
                if (textBoxWd.Text == "")
                {
                    MessageBox.Show("温度不能为空", "提示");
                    return;
                }
                else if (!double.TryParse(textBoxWd.Text, out a))
                {
                    MessageBox.Show("温度非数字", "提示");
                    return;
                }
                else
                {
                    Wd = a;
                }
                if (textBoxSd.Text == "")
                {
                    MessageBox.Show("湿度不能为空", "提示");
                    return;
                }
                else if (!double.TryParse(textBoxSd.Text, out a))
                {
                    MessageBox.Show("湿度非数字", "提示");
                    return;
                }
                else
                {
                    Sd = a;
                }
                if (textBoxDqy.Text == "")
                {
                    MessageBox.Show("大气压不能为空", "提示");
                    return;
                }
                else if (!double.TryParse(textBoxDqy.Text, out a))
                {
                    MessageBox.Show("大气压非数字", "提示");
                    return;
                }
                else
                {
                    Dqy = a;
                }
                if (carinfo.燃料种类 == 0)
                {
                    if (textBoxEdnj.Text == "")
                    {
                        MessageBox.Show("额定扭矩不能为空", "提示");
                        return;
                    }
                    else if (!double.TryParse(textBoxEdnj.Text, out a))
                    {
                        MessageBox.Show("额定扭矩非数字", "提示");
                        return;
                    }
                    else
                    {
                        carinfo.点燃式额定扭矩 = a;
                    }
                    if (textBoxEdnjzs.Text == "")
                    {
                        MessageBox.Show("额定扭矩转速不能为空", "提示");
                        return;
                    }
                    else if (!double.TryParse(textBoxEdnjzs.Text, out a))
                    {
                        MessageBox.Show("额定扭矩转速非数字", "提示");
                        return;
                    }
                    else
                    {
                        carinfo.点燃式额定扭矩转速 = a;
                    }

                    caculateQyFE();
                }
                else
                {
                    carinfo.是否危险货物运输车辆 = checkBoxIsDanger.Checked ? 1 : 0;
                    carinfo.压燃式功率参数类型 = comboBoxGlcslx.SelectedIndex;
                    if (textBoxEdgl.Text == "")
                    {
                        MessageBox.Show("额定功率不能为空", "提示");
                        return;
                    }
                    else if (!double.TryParse(textBoxEdgl.Text, out a))
                    {
                        MessageBox.Show("额定功率非数字", "提示");
                        return;
                    }
                    else
                    {
                        carinfo.压燃式额定功率 = a;
                    }
                    caculateCyFE();
                }

            }
            
        }

        #region 汽车燃料消耗检测工况相关参数计算
        //汽车燃料消耗检测工况下的道路行驶阻力：FR=Ft+Fw        
        private double distance = 0;//记录行驶距离,m
        private double totalDistance = 0;
        private double totalYh = 0;
        private double yhPerHundred = 0;
        private string pdjg;
        private double yhXz = 0;
        private int testTimes = 0;//第几次检测
        private double Y_FR = 0, Y_Ft = 0, Y_Fw = 0;
        //汽车道路行驶的滚动阻力：Ft=G*g*f,G为受检车辆额定总质量（或牵引车单车满载总质量)
        //f:滚动阻力系数，汽车以50km/h,60km/h速度在水平面行驶的滚动阻力系数f
        //|-------------轮胎--------------|----------f
        //|子午胎------轮胎断面宽度<8.25in|---------0.007
        //|子午胎-----轮胎断面宽度>=8.25in|---------0.006
        //|------------斜交胎-------------|---------0.010
        private double g = 9.81, f = 0.007;
        //------------------------------------------|---------------------------------------
        //汽车道路行驶空气阻力：Fw=1/2*Cd*A*p*(v0/3.6)*(v0/3.6)
        //------------------------------------------|---------------------------------------
        //   Cd          营运客车                   |                  营运货车
        //   车长L     等速60km/h      等速50km/h   |    车身型式     额定总质量    等速50
        //------------------------------------------|---------------------------------------
        //                                          |      拦板车      G<10000        0.9
        //   L<=7000       0.6             0.65     |      自卸车       
        //                                          |      牵引车      G>=10000       1.1
        //------------------------------------------|---------------------------------------
        //7000<L<=9000     0.7             0.75     |      仓栅车                     1.4
        //------------------------------------------|---------------------------------------
        //                                          |      厢式车      G<10000        0.8
        //   L>7000        0.8             0.85     |               10000<=G<15000    0.9
        //                                          |      罐  车      G>=15000       1.0
        //------------------------------------------|---------------------------------------
        //A为迎风面积 A=B*H*10(-6)   B:汽车前轮距(mm) H：汽车高度(mm)
        private double Cd = 0, A = 0, p = 1.189, v0 = 50.0;
        //汽车台架运转阻力等于汽车台架滚动阻力和台架内阻之和：Fc=Ffc+Ftc
        //Ftc为台架内阻，可使用推荐值 ，也可以使用滑行法进行测定
        //----------------------|----------------------------|-----------------------------
        //        速度          |   二轴四滚筒台架内阻Ftc    |  三轴六滚筒台架内阻Ftc    
        //----------------------|----------------------------|-----------------------------
        //         50           |               100          |           130  
        //----------------------|----------------------------|-----------------------------
        //         60           |               110          |           140
        //----------------------|----------------------------|-----------------------------
        private double Y_Fc = 0, Y_Ffc = 0, Y_Ftc = 0;
        //Ffc=Gr*g*fc   fc=1.5*f  Gr为驱动轴空载质量
        private double Gr = 0, fc = 0;


        //台架加载阻力 FTC=Fr-Fc
        private double Y_FTC = 0;

        /// <summary>
        /// 计算油耗测试工况时台架加载阻力
        /// </summary>
        private void caculateFTC()
        {
            if (carinfo.汽车类型 == 0)//营运客车
            {
                if (carinfo.客车等级 == 0)//高级
                {
                    v0 = 60;
                }
                else
                {
                    v0 = 50;
                }
            }
            else//营运货车
            {
                v0 = 50;
            }
            if (carinfo.汽车类型 == 0)//营运客车
            {
                if (carinfo.客车车长 <= 7000)//高级
                {
                    if (v0 == 60)
                        Cd = 0.60;
                    else
                        Cd = 0.65;
                }
                else if (carinfo.客车车长 > 9000)
                {
                    if (v0 == 60)
                        Cd = 0.80;
                    else
                        Cd = 0.85;
                }
                else
                {
                    if (v0 == 60)
                        Cd = 0.70;
                    else
                        Cd = 0.75;
                }
            }
            else//营运货车
            {
                if (carinfo.货车车身型式 == 0 || carinfo.货车车身型式 == 1 || carinfo.货车车身型式 == 2)
                {
                    if (carinfo.额定总质量 < 10000)
                    {
                        Cd = 0.9;
                    }
                    else
                    {
                        Cd = 1.1;
                    }
                }
                else if (carinfo.货车车身型式 == 3)
                {
                    Cd = 1.4;
                }
                else if (carinfo.货车车身型式 == 4 || carinfo.货车车身型式 == 5)
                {
                    if (carinfo.额定总质量 < 10000)
                    {
                        Cd = 0.8;
                    }
                    else if (carinfo.额定总质量 >= 15000)
                    {
                        Cd = 1.0;
                    }
                    else
                    {
                        Cd = 0.9;
                    }
                }

            }
            if (equipdata.CgjNz == 0)//测功机内阻使用经验值
            {
                if (v0 == 60)
                {
                    if (carinfo.驱动轴数 == 1)
                    {
                        if (equipdata.Cgjxh == "DCG-26L|DCG-26LD")
                        {
                            Y_Ftc = 140;
                        }
                        else
                            Y_Ftc = 110;
                    }
                    else
                    {
                        Y_Ftc = 140;
                    }
                }
                else
                {
                    if (carinfo.驱动轴数 == 1)
                    {

                        if (equipdata.Cgjxh == "DCG-26L|DCG-26LD")
                        {
                            Y_Ftc = 130;
                        }
                        else
                            Y_Ftc = 100;
                    }
                    else
                    {
                        Y_Ftc = 130;
                    }
                }
            }
            else//使用反拖法测定的值
            {
                Y_Ftc = configini.getCgjNz(v0.ToString("0"));
            }
            if (carinfo.轮胎类型 == 0)
            {
                if (carinfo.子午胎轮胎断面宽度 < 8.25) f = 0.007;
                else f = 0.006;
            }
            else
            {
                f = 0.010;
            }
            Y_Ft = carinfo.额定总质量 * g * f;
            A = carinfo.汽车前轮距 * carinfo.汽车高度 * 0.000001;
            Y_Fw = 0.5 * Cd * A * p * (v0 / 3.6) * (v0 / 3.6);
            Y_FR = Y_Ft + Y_Fw;
            fc = 1.5 * f;
            Y_Ffc = carinfo.驱动轴空载质量 * g * fc;
            Y_Fc = Y_Ftc + Y_Ffc;
            Y_FTC = Y_FR - Y_Fc;
            Y_FTC = Math.Round(Y_FTC, 0);//台架加载阻力，四舍五入至整数位，单位为N
        }
        #endregion
        #region 性能动力测试相关参数计算
        //X_FE=X_Fe-X_Ftc-X_Fc-X_Ff-X_Ft
        //Fc=fc*Gr*g
        private double X_FE = 0, X_Fe = 0, X_Ftc = 0, X_Fc = 0, X_Ff = 0, X_Ft = 0;//柴油车使用
        private double X_FM = 0, X_Fm = 0;//汽油车使用
        //X_Fe=(3600*η*Pe)/(αd*Ve)
        //Pe:发动机功率 kW
        //η:0.75
        //αd:压燃式发动机功率校正系数，发动机因子fm=0.3
        private double η = 0.75, Pe = 0, αd = 0, αa = 0, fm = 0.3;
        //Ff=(3600*fp*Pe)/Ve
        //fp:Ve车速点，发动附件消耗功率系数，当发动机铭牌功率参数以额定功率表征时，fp=0.1，以车辆铭牌最大净功率表征时，fp取0
        private double fp = 0;
        //Ft=0.18*(Fe-Ff)
        /// <summary>
        /// 驱动轮轮边稳定车速
        /// </summary>
        private double Vw = 0;
        /// <summary>
        /// 额定功率车速Ve=0.86*Va
        /// </summary>
        private double Ve = 0;
        /// <summary>
        /// 全油门所挂挡位的最高稳定车速，单位为千米每小时（km/h）
        /// </summary>
        private double Va = 0;
        private double Wd = 0, Sd = 0, Dqy = 0;
        private double[,] 水蒸气分压 =
        {
            { 0.3,0.2,0.2,0.1,0.1 },
            { 0.4,0.3,0.2,0.2,0.1} ,
            { 0.6,0.5,0.4,0.2,0.1} ,
            { 0.9,0.7,0.5,0.4,0.2} ,

            { 1.2,1.0,0.7,0.5,0.2} ,
            { 1.7,1.4,1.0,0.7,0.5} ,
            { 2.3,1.9,1.4,0.9,0.5} ,
            { 3.2,2.5,1.9,1.3,0.6} ,

            { 3.6,2.9,2.1,1.4,0.7} ,
            { 4.2,3.4,2.5,1.7,0.9} ,
            { 4.8,3.8,2.9,1.9,1.0} ,
            { 5.3,4.3,3.2,2.1,1.1} ,

            { 6.0,4.8,3.6,2.6,1.2} ,
            { 6.6,5.3,4.0,2.7,1.3} ,
            { 7.4,5.9,4.4,3.0,1.5} ,
            { 8.2,6.6,4.9,3.3,1.6} ,

            { 9.1,7.3,5.5,3.6,1.8} ,
            { 10.1,8.1,6.1,4.0,2.0} ,
            { 11.2,8.9,6.7,4.5,2.2} ,
            { 12.3,9.9,7.4,4.9,2.5} ,
        };
        private List<double> templist = new List<double> { -10, -5, 0, 5, 10, 15, 20, 25, 27, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48, 50 };
        private List<double> humilist = new List<double> { 100, 80, 60, 40, 20 };
        private int GetRow(double temp)
        {
            if (temp <= templist[0]) return 0;
            else if (temp >= templist[templist.Count - 1]) return templist.Count - 1;
            else
            {
                for (int i = 0; i < templist.Count - 1; i++)
                {
                    if ((temp - templist[i]) * (temp - templist[i + 1]) <= 0)
                    {
                        if (Math.Abs(temp - templist[i]) < Math.Abs(temp - templist[i + 1]))
                            return i;
                        else
                            return i + 1;
                    }
                }
                return templist.Count - 1;
            }
        }
        private int GetColumn(double humidity)
        {
            if (humidity <= humilist[0]) return 0;
            else if (humidity >= humilist[templist.Count - 1]) return humilist.Count - 1;
            else
            {
                for (int i = 0; i < humilist.Count - 1; i++)
                {
                    if ((humidity - humilist[i]) * (humidity - humilist[i + 1]) <= 0)
                    {
                        if (Math.Abs(humidity - humilist[i]) < Math.Abs(humidity - humilist[i + 1]))
                            return i;
                        else
                            return i + 1;
                    }
                }
                return humilist.Count - 1;
            }
        }
        private double fa = 0;//大气因子
        /// <summary>
        /// 柴油机校正系数计算
        /// </summary>
        private void caculateαd()
        {
            int rownumber = GetRow(Wd);
            int columnnumber = GetColumn(Sd);
            φPaw = 水蒸气分压[rownumber, columnnumber];
            pa = Dqy - φPaw;
            fa = (99.0 / pa) * Math.Pow((Wd + 273.15) / 298, 0.7);
            αd = Math.Pow(fa, fm);
        }
        /// <summary>
        /// 汽油机校正系数计算
        /// </summary>
        private double pa = 0, φPaw = 0;
        private void caculateαa()
        {
            int rownumber = GetRow(Wd);
            int columnnumber = GetColumn(Sd);
            φPaw = 水蒸气分压[rownumber, columnnumber];
            pa = Dqy - φPaw;
            αa = Math.Pow(99.0 / pa, 1.2) * Math.Pow((Wd + 273.15) / 298, 0.6);
        }
        private void caculateCyFE()
        {
            Ve = double.Parse( textBoxVeOrVm.Text);
            if (comboBoxDengji.SelectedIndex==0)
            {
                η = 0.82;
            }
            else
            {
                η = 0.75;
            }
            Pe = carinfo.压燃式额定功率;
            Gr = carinfo.驱动轴空载质量;
            caculateαd();
            X_Fe = (3600.0 * η * Pe) / (αd * Ve);
            if (equipdata.CgjNz == 0)//测功机内阻使用经验值
            {
                if (carinfo.燃料种类 == 0)
                {
                    if (carinfo.驱动轴数 == 1)
                    {
                        if (equipdata.Cgjxh == "DCG-26L|DCG-26LD")
                        {
                            X_Ftc = 140;
                        }
                        else
                            X_Ftc = 110;
                    }
                    else
                    {
                        X_Ftc = 140;
                    }
                }
                else
                {
                    if (carinfo.驱动轴数 == 1)
                    {
                        if (equipdata.Cgjxh == "DCG-26L|DCG-26LD")
                        {
                            X_Ftc = 160;
                        }
                        else
                            X_Ftc = 130;
                    }
                    else
                    {
                        X_Ftc = 160;
                    }
                }
            }
            else//使用反拖法测定的值
            {
                X_Ftc = configini.getCgjNz("80");
            }
            if (carinfo.轮胎类型 == 0)
            {
                f = 0.006;
            }
            else
            {
                f = 0.010;
            }
            if (Ve >= 70)
                fc = 2.0 * f;
            else
                fc = 1.5 * f;
            X_Fc = fc * Gr * g;
            if (carinfo.压燃式功率参数类型 == 0)
                fp = 0.1;
            else
                fp = 0;
            X_Ff = 3600 * fp * Pe / Ve;
            X_Ft = 0.18 * (X_Fe - X_Ff);
            X_FE = X_Fe - X_Ftc - X_Fc - X_Ff - X_Ft;
            textBox1.Clear();
            textBox1.AppendText("Ve="+Ve.ToString("0.0")+"\r\n");
            textBox1.AppendText("Pe=" + Pe.ToString("0") + "\r\n");
            textBox1.AppendText("η=" + η.ToString("0.000") + "\r\n");
            textBox1.AppendText("αd=" + αd.ToString("0.000") + "\r\n");
            textBox1.AppendText("Fe=" + X_Fe.ToString("0") + "\r\n");
            textBox1.AppendText("Ftc=" + X_Ftc.ToString("0") + "\r\n");
            textBox1.AppendText("f=" + f.ToString("0.000") + "\r\n");
            textBox1.AppendText("fc=" + fc.ToString("0.000") + "\r\n");
            textBox1.AppendText("Gr=" + Gr.ToString("0") + "\r\n");
            textBox1.AppendText("Fc=" + X_Fc.ToString("0") + "\r\n");
            textBox1.AppendText("fp=" + fp.ToString("0.000") + "\r\n");
            textBox1.AppendText("Ff=" + X_Ff.ToString("0") + "\r\n");
            textBox1.AppendText("Ft=" + X_Ft.ToString("0") + "\r\n");
            textBox1.AppendText("FE=" + X_FE.ToString("0") + "\r\n");
        }
        
        private double Vm = 0, nm = 0;
        /// <summary>
        /// 发动机额定扭矩
        /// </summary>
        private double Mm = 0;
        private void caculateQyFE()
        {
            Ve = double.Parse(textBoxVeOrVm.Text);
            if (comboBoxDengji.SelectedIndex == 0)
            {
                η = 0.82;
            }
            else
            {
                η = 0.75;
            }
            Mm = carinfo.点燃式额定扭矩;
            if (carinfo.点燃式额定扭矩转速 > 4000)
                nm = 4000;
            else
                nm = carinfo.点燃式额定扭矩转速;
            Gr = carinfo.驱动轴空载质量;
            fm = 0.06;
            caculateαa();
            X_Fm = (0.377 * η * Mm * nm) / (αa * Vm);
            if (equipdata.CgjNz == 0)//测功机内阻使用经验值
            {
                if (carinfo.燃料种类 == 0)
                {
                    if (carinfo.驱动轴数 == 1)
                    {
                        if (equipdata.Cgjxh == "DCG-26L|DCG-26LD")
                        {
                            X_Ftc = 140;
                        }
                        else
                            X_Ftc = 110;
                    }
                    else
                    {
                        X_Ftc = 140;
                    }
                }
                else
                {
                    if (carinfo.驱动轴数 == 1)
                    {
                        if (equipdata.Cgjxh == "DCG-26L|DCG-26LD")
                        {
                            X_Ftc = 160;
                        }
                        else
                            X_Ftc = 130;
                    }
                    else
                    {
                        X_Ftc = 160;
                    }
                }
            }
            else//使用反拖法测定的值
            {
                X_Ftc = configini.getCgjNz("50");
            }
            if (carinfo.轮胎类型 == 0)
            {
                f = 0.006;
            }
            else
            {
                f = 0.010;
            }
            if (Ve >= 70)
                fc = 2.0 * f;
            else
                fc = 1.5 * f;
            X_Fc = fc * Gr * g;
            X_Ff = 0.377 * fm * Mm * nm / Vm;
            X_Ft = 0.18 * (X_Fm - X_Ff);
            X_FM = X_Fm - X_Ftc - X_Fc - X_Ff - X_Ft;
            textBox1.Clear();
            textBox1.AppendText("Vm=" + Vm.ToString("0.0") + "\r\n");
            textBox1.AppendText("nm=" + nm.ToString("0") + "\r\n");
            textBox1.AppendText("η=" + η.ToString("0.000") + "\r\n");
            textBox1.AppendText("Mm=" + Mm.ToString("0") + "\r\n");
            textBox1.AppendText("αa=" + αa.ToString("0.000") + "\r\n");
            textBox1.AppendText("Fm=" + X_Fm.ToString("0") + "\r\n");
            textBox1.AppendText("Ftc=" + X_Ftc.ToString("0") + "\r\n");
            textBox1.AppendText("f=" + f.ToString("0.000") + "\r\n");
            textBox1.AppendText("fc=" + fc.ToString("0.000") + "\r\n");
            textBox1.AppendText("Gr=" + Gr.ToString("0") + "\r\n");
            textBox1.AppendText("Fc=" + X_Fc.ToString("0") + "\r\n");
            textBox1.AppendText("fm=" + fm.ToString("0.000") + "\r\n");
            textBox1.AppendText("Ff=" + X_Ff.ToString("0"+"\r\n"));
            textBox1.AppendText("Ft=" + X_Ft.ToString("0") + "\r\n");
            textBox1.AppendText("FM=" + X_FM.ToString("0") + "\r\n");
        }

        
        #endregion
    }
}
