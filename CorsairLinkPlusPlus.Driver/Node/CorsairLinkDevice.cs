using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.USB;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.Node
{
    public abstract class CorsairLinkDevice : CorsairBaseDevice
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
                    return CorsairLinkDevicePSU.CreateNew(usbDevice, channel);
                default:
                    return null;
            }
        }

        public virtual string GetUDID()
        {
            return usbDevice.GetUDID() + "/Channel" + channel;
        }

        internal readonly CorsairLinkUSBDevice usbDevice;
        internal readonly byte channel;

        internal CorsairLinkDevice(CorsairLinkUSBDevice usbDevice, byte channel)
        {
            this.usbDevice = usbDevice;
            this.channel = channel;
        }

        public virtual void Refresh(bool volatileOnly)
        {

        }

        public bool IsPresent()
        {
            return true;
        }

        internal byte ReadSingleByteRegister(byte register)
        {
            return usbDevice.ReadSingleByteRegister(register, channel);
        }

        internal byte[] ReadRegister(byte register, byte bytes)
        {
            return usbDevice.ReadRegister(register, channel, bytes);
        }

        internal void WriteSingleByteRegister(byte register, byte value)
        {
            usbDevice.WriteSingleByteRegister(register, channel, value);
        }

        internal void WriteRegister(byte register, byte[] bytes)
        {
            usbDevice.WriteRegister(register, channel, bytes);
        }

        public abstract string GetName();

        public virtual List<CorsairBaseDevice> GetSubDevices()
        {
            return new List<CorsairBaseDevice>(GetSensors());
        }

        public virtual List<CorsairSensor> GetSensors()
        {
            return new List<CorsairSensor>();
        }
    }
}
