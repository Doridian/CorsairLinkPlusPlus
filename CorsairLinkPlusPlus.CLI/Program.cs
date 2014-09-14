using CorsairLinkPlusPlus.Driver;
using CorsairLinkPlusPlus.Driver.Link;
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
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        static void Main(string[] args)
        {
            CorsairLinkDeviceEnumerator enumerator = new CorsairLinkDeviceEnumerator();
            List<CorsairLinkUSBDevice> usbDevices = enumerator.GetDevices();
            CorsairLinkUSBDevice usbDevice = usbDevices[0];

            List<CorsairLinkDevice> devices = usbDevice.GetSubDevices();
            foreach (CorsairLinkDevice device in devices)
            {
                Console.Out.WriteLine("Found: " + device.GetDeviceName() + ", " + string.Format("{0:x2}", device.GetDeviceID()) + ", " + string.Format("{0:x4}", device.GetFirmwareVersion()) + ", " + device.GetFanCount());
            }

            Console.In.ReadLine();
        }
    }
}
