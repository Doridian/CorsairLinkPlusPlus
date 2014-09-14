using CorsairLinkPlusPlus.Driver.USB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Link
{
    public class CorsairLinkDevice
    {
        internal static CorsairLinkDevice CreateNew(CorsairLinkUSBDevice usbDevice, byte channel)
        {
            return new CorsairLinkDevice(usbDevice, channel);
        }

        private readonly CorsairLinkUSBDevice usbDevice;
        private readonly byte channel;

        internal CorsairLinkDevice(CorsairLinkUSBDevice usbDevice, byte channel)
        {
            this.usbDevice = usbDevice;
            this.channel = channel;
        }

        protected byte ReadSingleByteRegister(byte register)
        {
            return usbDevice.SendCommand(0x07, channel, new byte[] { register })[0];
        }

        protected byte[] ReadRegister(byte register, byte bytes)
        {
            switch(bytes) {
                case 1:
                    return new byte[] { ReadSingleByteRegister(register) };
                case 2:
                    byte[] rawRet2 = usbDevice.SendCommand(0x09, channel, new byte[] { register });
                    return new byte[] { rawRet2[0], rawRet2[1] };
                default:
                    byte[] rawRet = usbDevice.SendCommand(0x0B, channel, new byte[] { register, bytes });
                    byte[] ret = new byte[rawRet[0]];
                    Buffer.BlockCopy(rawRet, 1, ret, 0, ret.Length);
                    return ret;
            }
        }

        public int GetDeviceID()
        {
            return ReadSingleByteRegister(0x00);
        }

        public int GetFirmwareVersion()
        {
            return BitConverter.ToInt16(ReadRegister(0x01, 2), 0);
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public string GetDeviceName()
        {
            byte[] reg = ReadRegister(0x02, 8);
            string ret = System.Text.Encoding.ASCII.GetString(reg);
            if (string.IsNullOrEmpty(ret))
                return "N/A";
            return ByteArrayToHexString(reg);
        }

        public int GetFanCount()
        {
            return ReadSingleByteRegister(0x11);
        }

        public int GetTemperatureCount()
        {
            return ReadSingleByteRegister(0x0D);
        }

        public int GetLEDCount()
        {
            return ReadSingleByteRegister(0x05);
        }
    }
}
