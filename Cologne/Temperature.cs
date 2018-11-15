using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Management;
using System.Windows.Forms;
using OpenHardwareMonitor.Hardware;

namespace Cologne
{
    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }
}
    class Temperature
    {
        private ManagementObject fetchHardwareProperties(string winClass)
        {
            ManagementObjectCollection cpu = new ManagementObjectSearcher("Select * from " + winClass).Get();

            foreach (ManagementObject x in cpu)
            {
                return x;
            }

            return null;
        }

        public decimal[] Update()
        {
            decimal[] temps = new decimal[3];
            Cologne.UpdateVisitor updateVisitor = new Cologne.UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.CPUEnabled = true;
            computer.GPUEnabled = true;
            computer.RAMEnabled = true;
            computer.Accept(updateVisitor);
            for (int i = 0; i < computer.Hardware.Length; i++)
            {
                if (computer.Hardware[i].HardwareType == HardwareType.CPU)
                {
                    for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                    {
                        if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                        {
                            temps[0] = Convert.ToDecimal(computer.Hardware[i].Sensors[j].Value);
                        }
                    }
                }

                if(computer.Hardware[i].HardwareType == HardwareType.GpuNvidia)
                {
                    for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                    {
                        if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                        {
                            temps[1] = Convert.ToDecimal(computer.Hardware[i].Sensors[j].Value);
                        }
                    }
                }

                if (computer.Hardware[i].HardwareType == HardwareType.RAM)
                {
                    for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                    {
                        if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                        {
                            temps[2] = Convert.ToDecimal(computer.Hardware[i].Sensors[j].Value);
                        }
                    }
                }
        }
            computer.Close();

            return temps;

        //MessageBox.Show(gpu["CurrentTemperature"].ToString());
    }
}
