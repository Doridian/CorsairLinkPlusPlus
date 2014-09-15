using CorsairLinkPlusPlus.Driver.USB;
using System;

namespace CorsairLinkPlusPlus.Driver.Link
{
    public abstract class CorsairLinkDevice
    {
        internal static CorsairLinkDevice CreateNew(CorsairLinkUSBDevice usbDevice, byte channel, byte deviceType)
        {
            switch (deviceType)
            {
                case 0x5:
                    return new CorsairLinkDeviceModern(usbDevice, channel);
                case 0x3:
                    return new CorsairLinkDeviceAFP(usbDevice, channel);
                case 0x1:
                    return new CorsairLinkDevicePMBUS(usbDevice, channel);
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

        protected void WriteSingleByteRegister(byte register, byte value)
        {
            usbDevice.WriteSingleByteRegister(register, channel, value);
        }

        protected void WriteRegister(byte register, byte[] bytes)
        {
            usbDevice.WriteRegister(register, channel, bytes);
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
