﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Management;
using MaterialSkin;
using MaterialSkin.Controls;
using MetroFramework;
using MetroFramework.Forms;
using Microsoft.VisualBasic;
using System.IO;
using System.Threading;

namespace Cologne
{
    public partial class Form1 : MetroForm
    {
        private List<String> hardware = new List<String>();
        public Form1()
        {
            InitializeComponent();
        }

        private ManagementObject fetchHardwareProperties(string winClass)
        {
            ManagementObjectCollection cpu = new ManagementObjectSearcher("Select * from " + winClass).Get();

            foreach(ManagementObject x in cpu)
            {
                return x;
            }

            return null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ManagementObject cpu = fetchHardwareProperties("Win32_Processor");
            ManagementObject gpu = fetchHardwareProperties("Win32_VideoController");
            ManagementObject mobo = fetchHardwareProperties("Win32_BaseBoard");
            ManagementObject mem = fetchHardwareProperties("Win32_ComputerSystem"); 
             ManagementObject memory = fetchHardwareProperties("Win32_PhysicalMemory");
             ManagementObject os = fetchHardwareProperties("Win32_OperatingSystem");

            //ManagementObject hdd = fetchHardwareProperties("Win32_DiskDrive");

            hardware.Add(cpu["Name"].ToString());
            hardware.Add(gpu["Name"].ToString());
            hardware.Add(mobo["Product"].ToString());
            hardware.Add(Math.Round(Convert.ToDouble(mem["TotalPhysicalMemory"]) / 1024 / 1024 / 1024).ToString());

            label1.Text += " " + cpu["Name"].ToString();
            label2.Text += " " + gpu["Name"].ToString();
            label3.Text += " " + mobo["Product"].ToString();
            label5.Text = " " + hardware[3].ToString() + "GB " + memory["Manufacturer"] + " " + RamType;
            label6.Text = " " + os["Name"].ToString().Split('|')[0];

            new Thread(() =>
            {
                Change();
            }).Start();

            Temperature temp = new Temperature();

            decimal[] temps = temp.Update();

            cpu_temp.Text = temps[0].ToString();
            gpu_temp.Text = temps[1].ToString();
            mem_temp.Text = temps[2].ToString();
        }

        public void Change()
        {
            Appraise price = new Appraise(hardware);

            decimal call = price.Depreciate();

            decimal actual = Math.Round(price.actual, 2);
            decimal dep = Math.Round(call, 2);

            this.Invoke((MethodInvoker)delegate ()
            {
                if (dep < actual) label8.ForeColor = Color.Red;

                label7.ForeColor = Color.Green;

                label8.Text = "Depreciated Value: $" + dep.ToString();
                label7.Text = "Actual Value: $" + actual.ToString();
            });
        }

        public string RamType
        {
            get
            {
                int type = 0;

                ConnectionOptions connection = new ConnectionOptions();
                connection.Impersonation = ImpersonationLevel.Impersonate;
                ManagementScope scope = new ManagementScope("\\\\.\\root\\CIMV2", connection);
                scope.Connect();
                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_PhysicalMemory");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    type = Convert.ToInt32(queryObj["MemoryType"]);
                }

                return TypeString(type);
            }
        }

        private string TypeString(int type)
        {
            string outValue = string.Empty;

            switch (type)
            {
                case 0x0: outValue = "Unknown"; break;
                case 0x1: outValue = "DDR4"; break;
                case 0x2: outValue = "DRAM"; break;
                case 0x3: outValue = "Synchronous DRAM"; break;
                case 0x4: outValue = "Cache DRAM"; break;
                case 0x5: outValue = "EDO"; break;
                case 0x6: outValue = "EDRAM"; break;
                case 0x7: outValue = "VRAM"; break;
                case 0x8: outValue = "SRAM"; break;
                case 0x9: outValue = "RAM"; break;
                case 0xa: outValue = "ROM"; break;
                case 0xb: outValue = "Flash"; break;
                case 0xc: outValue = "EEPROM"; break;
                case 0xd: outValue = "FEPROM"; break;
                case 0xe: outValue = "EPROM"; break;
                case 0xf: outValue = "CDRAM"; break;
                case 0x10: outValue = "3DRAM"; break;
                case 0x11: outValue = "SDRAM"; break;
                case 0x12: outValue = "SGRAM"; break;
                case 0x13: outValue = "RDRAM"; break;
                case 0x14: outValue = "DDR"; break;
                case 0x15: outValue = "DDR2"; break;
                case 0x16: outValue = "DDR2 FB-DIMM"; break;
                case 0x17: outValue = "Undefined 23"; break;
                case 0x18: outValue = "DDR3"; break;
                case 0x19: outValue = "FBD2"; break;
                default: outValue = "Undefined"; break;
            }

            return outValue;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            using (Collect frm = new Collect())
            {
                frm.ShowDialog();
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                Change();
            }).Start();
        }
    }
}
