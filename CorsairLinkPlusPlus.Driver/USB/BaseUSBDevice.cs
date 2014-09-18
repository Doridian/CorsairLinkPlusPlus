using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Driver.Node;
using HidLibrary;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.USB
{
    public abstract class BaseUSBDevice : BaseDevice
    {
        private readonly HidDevice hidDevice;
        protected int commandNo = 20;

        internal BaseUSBDevice(RootDevice root, HidDevice hidDevice) : base(root)
        {
            this.hidDevice = hidDevice;
        }

        public override string GetLocalDeviceID()
        {
            return hidDevice.DevicePath.Replace('\\', '_').Replace('/', '_');
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

        protected override List<BaseDevice> GetSubDevicesInternal()
        {
            List<BaseDevice> ret = base.GetSubDevicesInternal();

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

        internal virtual void WriteSingleByteRegister(byte register, byte channel, byte value)
        {
            SendCommand(0x06, channel, new byte[] { register, value });
        }

        internal virtual void WriteRegister(byte register, byte channel, byte[] bytes)
        {
            switch (bytes.Length)
            {
                case 1:
                    WriteSingleByteRegister(register, channel, bytes[0]);
                    break;
                case 2:
                    SendCommand(0x08, channel, new byte[] { register, bytes[0], bytes[1] });
                    break;
                default:
                    byte[] rawBytes = new byte[bytes.Length + 2];
                    rawBytes[0] = register;
                    rawBytes[1] = (byte)bytes.Length;
                    Buffer.BlockCopy(bytes, 0, rawBytes, 2, bytes.Length);
                    SendCommand(0x0A, channel, rawBytes);
                    break;
            }
        }

        protected abstract byte[] ParseResponse(byte[] response);

        protected abstract byte[] MakeCommand(byte opcode, byte channel, byte[] response);

        public byte[] SendCommand(byte opcode, byte channel, byte[] command)
        {
            DisabledCheck();

            HidDeviceData response;
            command = MakeCommand(opcode, channel, command);

            RootDevice.usbGlobalMutex.WaitOne();
            hidDevice.Write(command, 500);
            response = hidDevice.Read(500);
            RootDevice.usbGlobalMutex.ReleaseMutex();

            return ParseResponse(response.Data);
        }
    }
}
