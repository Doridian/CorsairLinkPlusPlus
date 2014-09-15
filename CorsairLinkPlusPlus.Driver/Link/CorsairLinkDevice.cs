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
            return usbDevice.ReadSingleByteRegister(register, channel);
        }

        protected byte[] ReadRegister(byte register, byte bytes)
        {
            return usbDevice.ReadRegister(register, channel, bytes);
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
