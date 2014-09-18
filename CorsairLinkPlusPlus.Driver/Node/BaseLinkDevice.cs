using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.USB;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.Node
{
    public abstract class BaseLinkDevice : BaseDevice
    {
        internal static BaseLinkDevice CreateNew(USB.BaseUSBDevice usbDevice, byte channel, byte deviceType)
        {
            switch (deviceType)
            {
                case 0x5:
                    return new LinkDeviceModern(usbDevice, channel);
                case 0x3:
                    return new LinkDeviceAFP(usbDevice, channel);
                case 0x1:
                    return LinkDevicePSU.CreateNew(usbDevice, channel);
                default:
                    return null;
            }
        }

        public override string GetLocalDeviceID()
        {
            return "Channel" + channel;
        }

        internal readonly USB.BaseUSBDevice usbDevice;
        internal readonly byte channel;

        internal BaseLinkDevice(USB.BaseUSBDevice usbDevice, byte channel) : base(usbDevice)
        {
            this.usbDevice = usbDevice;
            this.channel = channel;
        }

        internal byte ReadSingleByteRegister(byte register)
        {
            DisabledCheck();
            return usbDevice.ReadSingleByteRegister(register, channel);
        }

        internal byte[] ReadRegister(byte register, byte bytes)
        {
            DisabledCheck();
            return usbDevice.ReadRegister(register, channel, bytes);
        }

        internal void WriteSingleByteRegister(byte register, byte value)
        {
            DisabledCheck();
            usbDevice.WriteSingleByteRegister(register, channel, value);
        }

        internal void WriteRegister(byte register, byte[] bytes)
        {
            DisabledCheck();
            usbDevice.WriteRegister(register, channel, bytes);
        }
    }
}
