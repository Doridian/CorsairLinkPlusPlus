#region LICENSE
/**
 * CorsairLinkPlusPlus
 * Copyright (c) 2014, Mark Dietzer & Simon Schick, All rights reserved.
 *
 * CorsairLinkPlusPlus is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 *
 * CorsairLinkPlusPlus is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with CorsairLinkPlusPlus.
 */
 #endregion

using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;
using CorsairLinkPlusPlus.Driver.CorsairLink.USB;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Node
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

        public override DeviceType Type
        {
            get
            {
                return DeviceType.Hub;
            }
        }

        public override string GetLocalDeviceID()
        {
            return "Channel" + channel;
        }

        internal readonly USB.BaseUSBDevice usbDevice;
        internal readonly byte channel;

        internal BaseLinkDevice(BaseLinkDevice linkDevice, byte channel)
            : base(linkDevice)
        {
            this.usbDevice = linkDevice.usbDevice;
            this.channel = channel;
        }

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

        internal void WriteSingleByteRegister(byte register, byte value, bool verify = false)
        {
            DisabledCheck();
            usbDevice.WriteSingleByteRegister(register, channel, value, verify);
        }

        internal void WriteRegister(byte register, byte[] bytes, bool verify = false)
        {
            DisabledCheck();
            usbDevice.WriteRegister(register, channel, bytes, verify);
        }
    }
}
