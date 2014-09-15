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
                hex.AppendFormat("{0:x2} ", b << 8);
            return hex.ToString();
        }

        static void Main(string[] args)
        {
            CorsairLinkDeviceEnumerator enumerator = new CorsairLinkDeviceEnumerator();
            List<CorsairLinkUSBDevice> usbDevices = enumerator.GetDevices();
            CorsairLinkUSBDevice usbDevice = usbDevices[0];

            /*Console.Out.WriteLine(ByteArrayToHexString(usbDevice.SendCommand(0x1F, 0, null)));
            Console.Out.WriteLine(ByteArrayToHexString(usbDevice.SendCommand(0x2F, 0, null)));
            Console.Out.WriteLine(ByteArrayToHexString(usbDevice.SendCommand(0x3F, 0, null)));
            Console.Out.WriteLine(ByteArrayToHexString(usbDevice.SendCommand(0x4F, 0, null)));
            Console.Out.WriteLine(ByteArrayToHexString(usbDevice.SendCommand(0x5F, 0, null)));
            Console.Out.WriteLine(ByteArrayToHexString(usbDevice.SendCommand(0x6F, 0, null)));
            Console.Out.WriteLine(ByteArrayToHexString(usbDevice.SendCommand(0x7F, 0, null)));
            Console.Out.WriteLine(ByteArrayToHexString(usbDevice.SendCommand(0x8F, 0, null)));
            Console.Out.WriteLine(ByteArrayToHexString(usbDevice.SendCommand(0x9F, 0, null)));
            Console.Out.WriteLine(ByteArrayToHexString(usbDevice.SendCommand(0xAF, 0, null)));
            Console.Out.WriteLine(ByteArrayToHexString(usbDevice.SendCommand(0xBF, 0, null)));*/

            List<CorsairLinkDevice> devices = usbDevice.GetSubDevices();
            foreach (CorsairLinkDevice device in devices)
            {
                Console.Out.WriteLine("Found: " + device.GetDeviceName() + ", " + device.GetFanCount());
            }

            Console.In.ReadLine();
        }
    }
}
