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

        static void Main(string[] args)
        {
            CorsairLinkDeviceEnumerator enumerator = new CorsairLinkDeviceEnumerator();
            List<CorsairLinkUSBDevice> usbDevices = enumerator.GetDevices();

            foreach (CorsairLinkUSBDevice usbDevice in usbDevices)
            {
                Console.Out.WriteLine(usbDevice.GetName());
                List<CorsairLinkDevice> devices = usbDevice.GetSubDevices();
                foreach (CorsairLinkDevice device in devices)
                {
                    Console.Out.WriteLine("\t" + device.GetDeviceName());
                    for (int i = 0; i < device.GetCoolerCount(); i++)
                    {
                        CorsairCooler cooler = device.GetCooler(i);
                        Console.Out.WriteLine("\t\t" + cooler.GetName() + " = " + cooler.GetValue() + " " + cooler.GetUnit());
                    }
                }
            }

            Console.In.ReadLine();
        }
    }
}
