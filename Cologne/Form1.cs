using System;
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

namespace Kryptos
{
    public partial class Form1 : MaterialForm
    {
        private List<String> hardware = new List<String>();
        public Form1()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
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
            //ManagementObject hdd = fetchHardwareProperties("Win32_DiskDrive");

            hardware.Add(cpu["Name"].ToString());
            hardware.Add(gpu["Name"].ToString());
            hardware.Add(mobo["Product"].ToString());
            hardware.Add(Math.Round(Convert.ToDouble(mem["TotalPhysicalMemory"]) / 1024 / 1024 / 1024).ToString());

            label1.Text += " " + cpu["Name"].ToString();
            label2.Text += " " + gpu["Name"].ToString();
            label3.Text += " " + mobo["Product"].ToString();

            Appraise price = new Appraise(hardware);

            decimal total = price.Estimate();

            label4.Text = "$" + total.ToString();

            //MessageBox.Show(price.diag);
        }
    }
}
