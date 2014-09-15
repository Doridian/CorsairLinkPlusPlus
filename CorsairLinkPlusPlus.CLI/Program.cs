using CorsairLinkPlusPlus.Driver;
using CorsairLinkPlusPlus.Driver.Node;
using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.USB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.CLI
{
    class Program
    {
        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2} ", b << 8);
            return hex.ToString();
        }

        static void PrintSensorsAndSubDevices(CorsairLinkDevice device, string prefix)
        {
            Console.Out.WriteLine(prefix + "+ " + device.GetDeviceName());

            foreach (CorsairSensor sensor in device.GetSensors())
            {
                Console.Out.WriteLine(prefix + "\t- " + sensor.GetName() + " = " + sensor.GetValue() + " " + sensor.GetUnit());
            }

            foreach(CorsairLinkDevice subDevice in device.GetSubDevices())
            {
                PrintSensorsAndSubDevices(subDevice, prefix + "\t");
            }
        }

        static void Main(string[] args)
        {
            CorsairLinkDeviceEnumerator enumerator = new CorsairLinkDeviceEnumerator();
            List<CorsairLinkUSBDevice> usbDevices = enumerator.GetDevices();

            foreach (CorsairLinkUSBDevice usbDevice in usbDevices)
            {
                Console.Out.WriteLine("+ " + usbDevice.GetName());
                foreach (CorsairLinkDevice device in usbDevice.GetSubDevices())
                {
                    PrintSensorsAndSubDevices(device, "\t");
                }
            }

            Console.In.ReadLine();
        }
    }
}
