using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Management;
using ini;

namespace upanControl
{
    public class ucontrol
    {
        string _serialNumber;
        string _driveLetter;
        string _upan_infor;
        string[] _upan;
        string[] _upan_number;
        string[] _upan_zhidingnumber;
        public void init_number()
        {
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            ini.INIIO.GetPrivateProfileString("配件", "设备", "", temp, 2048, @".\Config.ini");
            this._upan_zhidingnumber = temp.ToString().Trim().Split(',');
        }
        public string search_upan()
        {
            string upan_find = "";
            this._upan_infor = "";
            init_number();
            foreach (var item in DriveInfo.GetDrives())
            {
                if (item.DriveType == DriveType.Removable)
                {
                    this._upan_infor += item.ToString();
                    //MessageBox.Show(item.Name + "是移动磁盘，但是不是u盘不确定，还可能是SD卡，TF卡，或是移动硬盘");
                }
            }
            if (this._upan_infor != null)
            {
                this._upan = this._upan_infor.Split('\\');
                this._upan_number = new string[this._upan.Count()];
                get_upanNumber();
                foreach (string item in this._upan_zhidingnumber)
                {
                    upan_find = get_upandirectory(item);
                    if (upan_find != "")
                        return upan_find;
                }
            }
            return upan_find;
        }

        public void get_upanNumber()
        {
            int i=0;
            foreach (string item in this._upan)
            {
                if (item!="")
                {
                    this._upan_number[i] = getSerialNumberFromDriveLetter(item);
                    i++;
                    //MessageBox.Show(item.Name + "是移动磁盘，但是不是u盘不确定，还可能是SD卡，TF卡，或是移动硬盘");
                }
            }
        }
        public string get_upandirectory(string upan_zhiding)
        {
            int i = 0;
            for (i = 0;  i < this._upan_number.Count()-1;i++)
            {
                if (this._upan_number[i] == upan_zhiding)
                    return this._upan[i];
            }
            return "";
        }
        public string getSerialNumberFromDriveLetter(string driveLetter)
        {
            this._driveLetter = driveLetter.ToUpper();

            if (!this._driveLetter.Contains(":"))
            {
                this._driveLetter += ":";
            }

            matchDriveLetterWithSerial();

            return this._serialNumber;
        }

        private void matchDriveLetterWithSerial()
        {

            string[] diskArray;
            string driveNumber;
            string driveLetter;

            ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDiskToPartition");
            foreach (ManagementObject dm in searcher1.Get())
            {
                diskArray = null;
                driveLetter = getValueInQuotes(dm["Dependent"].ToString());
                diskArray = getValueInQuotes(dm["Antecedent"].ToString()).Split(',');
                driveNumber = diskArray[0].Remove(0, 6).Trim();
                if (driveLetter == this._driveLetter)
                {
                    /* This is where we get the drive serial */
                    ManagementObjectSearcher disks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                    foreach (ManagementObject disk in disks.Get())
                    {

                        if (disk["Name"].ToString() == ("\\\\.\\PHYSICALDRIVE" + driveNumber) & disk["InterfaceType"].ToString() == "USB")
                        {
                            this._serialNumber = parseSerialFromDeviceID(disk["PNPDeviceID"].ToString());
                        }
                    }
                }
            }
        }

        private string parseSerialFromDeviceID(string deviceId)
        {
            string[] splitDeviceId = deviceId.Split('\\');
            string[] serialArray;
            string serial;
            int arrayLen = splitDeviceId.Length - 1;

            serialArray = splitDeviceId[arrayLen].Split('&');
            serial = serialArray[0];

            return serial;
        }

        private string getValueInQuotes(string inValue)
        {
            string parsedValue = "";

            int posFoundStart = 0;
            int posFoundEnd = 0;

            posFoundStart = inValue.IndexOf("\"");
            posFoundEnd = inValue.IndexOf("\"", posFoundStart + 1);

            parsedValue = inValue.Substring(posFoundStart + 1, (posFoundEnd - posFoundStart) - 1);

            return parsedValue;
        }

    }
}
