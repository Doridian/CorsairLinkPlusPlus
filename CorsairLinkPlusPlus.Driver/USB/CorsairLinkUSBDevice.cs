using CorsairLinkPlusPlus.Driver.Link;
using HidLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.USB
{
    public abstract class CorsairLinkUSBDevice
    {
        private readonly HidDevice hidDevice;
        protected int commandNo = 20;

        internal CorsairLinkUSBDevice(HidDevice hidDevice)
        {
            this.hidDevice = hidDevice;
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

        public abstract string GetName();

        public Dictionary<byte, byte> GetUsedChannels()
        {
            byte[] cmdRes = SendCommand(0x4F, 0x00, null);
            Dictionary<byte, byte> usedChannels = new Dictionary<byte, byte>();
            for (byte i = 0; i < 8; i++)
            {
                if (cmdRes[i] != 0xFF)
                {
                    usedChannels.Add(i, cmdRes[i]);
                }
            }
            return usedChannels;
        }

        public List<CorsairLinkDevice> GetSubDevices()
        {
            List<CorsairLinkDevice> ret = new List<CorsairLinkDevice>();

            foreach (KeyValuePair<byte, byte> channel in GetUsedChannels())
            {
                ret.Add(GetDeviceOnChannel(channel.Key, channel.Value));
            }

            return ret;
        }

        internal CorsairLinkDevice GetDeviceOnChannel(byte channel)
        {
            return GetDeviceOnChannel(channel, GetUsedChannels()[channel]);
        }

        internal CorsairLinkDevice GetDeviceOnChannel(byte channel, byte deviceType)
        {
            return CorsairLinkDevice.CreateNew(this, channel, deviceType);
        }

        internal byte ReadSingleByteRegister(byte register, byte channel)
        {
            return SendCommand(0x07, channel, new byte[] { register })[0];
        }

        internal byte[] ReadRegister(byte register, byte channel, byte bytes)
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

        protected abstract byte[] ParseResponse(byte[] response);
        protected abstract byte[] MakeCommand(byte opcode, byte channel, byte[] response);

        protected byte[] ReadResponse()
        {
            HidDeviceData response = hidDevice.Read(500);
            return ParseResponse(response.Data);
        }

        public byte[] SendCommand(byte opcode, byte channel, byte[] command)
        {
            hidDevice.Write(MakeCommand(opcode, channel, command), 500);
            return ReadResponse();
        }
    }
}
