using CorsairLinkPlusPlus.Driver.USB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Link
{
    public abstract class CorsairLinkDevice
    {
        internal static CorsairLinkDevice CreateNew(CorsairLinkUSBDevice usbDevice, byte channel, byte deviceType)
        {
            switch (deviceType << 8)
            {
                case 0x500:
                    return new CorsairLinkDeviceModern(usbDevice, channel);
                case 0x300:
                    return new CorsairLinkDeviceAFP(usbDevice, channel);
                case 0x100:
                    return new CorsairLinkDeviceAXPSU(usbDevice, channel);
                default:
                    return null;
            }
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
            switch (bytes)
            {
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

        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public virtual string GetDeviceName()
        {
            throw new NotImplementedException();
        }
        public virtual int GetFanCount()
        {
            return 0;
        }

        public virtual int GetTemperatureCount()
        {
            return 0;
        }

        public virtual int GetLEDCount()
        {
            return 0;
        }
    }
}
