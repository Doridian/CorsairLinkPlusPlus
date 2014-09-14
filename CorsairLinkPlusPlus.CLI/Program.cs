using CorsairLinkPlusPlus.Driver;
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
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        static void Main(string[] args)
        {
            CorsairLinkDeviceEnumerator enumerator = new CorsairLinkDeviceEnumerator();
            List<CorsairLinkUSBDevice> devices = enumerator.GetDevices();
            CorsairLinkUSBDevice device = devices[0];

            //Console.Out.WriteLine(ByteArrayToString(device.MakeCommand(new byte[] {0x0A, 0x02, 0x08})));

            Console.Out.WriteLine(ByteArrayToString(device.SendCommand(0x4F, 0x00, new byte[] { })));
            //Console.Out.WriteLine(ByteArrayToString(device.SendCommand(new byte[] { 0x0B, 0x02, 0x08 })));
            //Console.Out.WriteLine(ByteArrayToString(device.SendCommand(new byte[] { 0x07, 0x00 })));

            Console.In.ReadLine();
        }
    }
}
