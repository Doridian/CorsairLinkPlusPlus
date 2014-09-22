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
using CorsairLinkPlusPlus.Driver.CorsairLink.Node;
using HidSharp;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using CorsairLinkPlusPlus.Common.Utility;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.USB
{
    public abstract class BaseUSBDevice : BaseDevice
    {
        private readonly HidDevice hidDevice;
        protected int commandNo = 20;

        internal BaseUSBDevice(CorsairRootDevice root, HidDevice hidDevice)
            : base(root)
        {
            this.hidDevice = hidDevice;
        }

        private static string FixUpDeviceID(string id)
        {
            char[] ret = id.ToCharArray();
            for (int i = 0; i < id.Length; i++ )
            {
                switch (ret[i])
                {
                    case '#':
                    case '?':
                    case '/':
                    case '\\':
                    case '{':
                    case '}':
                    case '&':
                        ret[i] = '_';
                        break;
                }
            }
            return new string(ret);
        }

        public override string GetLocalDeviceID()
        {
            return FixUpDeviceID(hidDevice.DevicePath);
        }

        public struct ChannelData
        {
            internal readonly byte channel;
            internal readonly byte deviceType;

            public ChannelData(byte channel, byte deviceType)
            {
                this.channel = channel;
                this.deviceType = deviceType;
            }
        }

        private volatile Dictionary<byte, BaseLinkDevice> subDevicesByChannel = null;

        public Dictionary<byte, byte> GetUsedChannels()
        {
            byte[] cmdRes = SendCommand(0x4F, 0x00, null);
            Dictionary<byte, byte> usedChannels = new Dictionary<byte, byte>();
            for (byte i = 0; i < 8; i++)
                if (cmdRes[i] != 0xFF)
                    usedChannels.Add(i, cmdRes[i]);

            return usedChannels;
        }

        protected override List<IDevice> GetSubDevicesInternal()
        {
            List<IDevice> ret = base.GetSubDevicesInternal();

            lock (subDeviceLock)
            {
                if (subDevicesByChannel == null)
                {
                    subDevicesByChannel = new Dictionary<byte, BaseLinkDevice>();
                    foreach (KeyValuePair<byte, byte> channel in GetUsedChannels())
                        subDevicesByChannel.Add(channel.Key, GetDeviceOnChannel(channel.Key, channel.Value));
                }

                ret.AddRange(subDevicesByChannel.Values);
            }

            return ret;
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);

            lock (subDeviceLock)
            {
                subDevicesByChannel = null;
            }
        }

        internal BaseLinkDevice GetDeviceOnChannel(byte channel)
        {
            if (subDevicesByChannel == null)
                GetSubDevicesInternal();
            return subDevicesByChannel[channel];
        }

        private BaseLinkDevice GetDeviceOnChannel(byte channel, byte deviceType)
        {
            return BaseLinkDevice.CreateNew(this, channel, deviceType);
        }

        internal virtual byte ReadSingleByteRegister(byte register, byte channel)
        {
            return SendCommand(0x07, channel, new byte[] { register })[0];
        }

        internal virtual byte[] ReadRegister(byte register, byte channel, byte bytes)
        {
            switch (bytes)
            {
                case 1:
                    return new byte[] { ReadSingleByteRegister(register, channel) };
                case 2:
                    byte[] rawRet2 = SendCommand(0x09, channel, new byte[] { register });
                    return new byte[] { rawRet2[0], rawRet2[1] };
                default:
                    byte[] rawRet = SendCommand(0x0B, channel, new byte[] { register, bytes });
                    byte[] ret = new byte[rawRet[0]];
                    Buffer.BlockCopy(rawRet, 1, ret, 0, ret.Length);
                    return ret;
            }
        }

        protected virtual byte[] ParseResponse(byte[] response)
        {
            byte[] newResponse = new byte[60];
            Buffer.BlockCopy(response, 3, newResponse, 0, newResponse.Length);
            return newResponse;
        }

        private bool SleepAfterFailure()
        {
            Thread.Sleep(100);
            return true;
        }

        internal virtual void WriteSingleByteRegister(byte register, byte channel, byte value, bool verify = false)
        {
            do
            {
                SendCommand(0x06, channel, new byte[] { register, value });
            } while (verify && ReadSingleByteRegister(register, channel) != value && SleepAfterFailure());
        }

        internal virtual void WriteRegister(byte register, byte channel, byte[] bytes, bool verify = false)
        {
            switch (bytes.Length)
            {
                case 1:
                    WriteSingleByteRegister(register, channel, bytes[0]);
                    break;
                case 2:
                    byte[] twoBytes = new byte[] { register, bytes[0], bytes[1] };
                    do
                    {
                        SendCommand(0x08, channel, twoBytes);
                    } while (verify && ReadRegister(register, channel, 2).SequenceEqual(bytes) && SleepAfterFailure());
                    break;
                default:
                    byte[] rawBytes = new byte[bytes.Length + 2];
                    rawBytes[0] = register;
                    rawBytes[1] = (byte)bytes.Length;
                    Buffer.BlockCopy(bytes, 0, rawBytes, 2, bytes.Length);
                    do
                    {
                        SendCommand(0x0A, channel, rawBytes);
                    } while (verify && ReadRegister(register, channel, rawBytes[1]).SequenceEqual(bytes) && SleepAfterFailure());
                    break;
            }
        }

        protected abstract byte[] MakeCommand(byte opcode, byte channel, byte[] response);

        public byte[] SendCommand(byte opcode, byte channel, byte[] command)
        {
            DisabledCheck();

            byte[] responseBytes;

            command = MakeCommand(opcode, channel, command);

            try
            {
                using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
                {
                    HidStream stream = hidDevice.Open();
                    stream.Write(command);
                    responseBytes = stream.Read();
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                this.Disable();
                throw e;
            }

            return ParseResponse(responseBytes);
        }

        public override DeviceType Type
        {
            get
            {
                return DeviceType.Hub;
            }
        }
    }
}
