using CorsairLinkPlusPlus.Driver.Link;
using HidLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.USB
{
    public class CorsairLinkUSBDevice
    {
        private readonly HidDevice hidDevice;
        protected int commandNo = 20;

        internal CorsairLinkUSBDevice(HidDevice hidDevice)
        {
            this.hidDevice = hidDevice;
        }

        protected virtual byte[] MakeCommand(byte opcode, byte channel, byte[] payload)
        {
            byte[] _command = new byte[65];
            _command[0] = 0;
            _command[2] = (byte)(commandNo++);
            if (commandNo > 255)
                commandNo = 20;
            _command[3] = (byte)(opcode | (channel << 4));
            if (payload != null)
            {
                _command[1] = (byte)(payload.Length + 2);
                Buffer.BlockCopy(payload, 0, _command, 4, payload.Length);
            }
            else
            {
                _command[1] = 2;
            }
            return _command;
        }

        public List<byte> GetUsedChannels()
        {
            byte[] cmdRes = SendCommand(0x4F, 0x00, null);
            List<byte> usedChannels = new List<byte>();
            for (byte i = 0; i < 8; i++)
            {
                if (cmdRes[i] != 0xFF)
                {
                    usedChannels.Add(i);
                }
            }
            return usedChannels;
        }

        public List<CorsairLinkDevice> GetSubDevices()
        {
            List<CorsairLinkDevice> ret = new List<CorsairLinkDevice>();

            foreach(byte channel in GetUsedChannels())
            {
                ret.Add(GetDeviceOnChannel(channel));
            }

            return ret;
        }

        public CorsairLinkDevice GetDeviceOnChannel(byte channel)
        {
            return CorsairLinkDevice.CreateNew(this, channel);
        }

        protected virtual byte[] ParseResponse(byte[] response)
        {
            byte[] _response = new byte[60];
            Buffer.BlockCopy(response, 3, _response, 0, _response.Length);
            return _response;
        }

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

    public class CorsairLinkUSBDeviceOld : CorsairLinkUSBDevice
    {
        internal CorsairLinkUSBDeviceOld(HidDevice hidDevice)
            : base(hidDevice)
        {

        }

        protected override byte[] MakeCommand(byte opcode, byte channel, byte[] payload)
        {
            byte[] _command = new byte[65];
            _command[0] = 0;
            _command[1] = (byte)(commandNo++);
            if (commandNo > 255)
                commandNo = 20;
            _command[2] = (byte)(opcode | (channel << 4));
            if (payload != null)
            {
                Buffer.BlockCopy(payload, 0, _command, 3, payload.Length);
            }
            return _command;
        }

        protected override byte[] ParseResponse(byte[] response)
        {
            byte[] _response = new byte[60];
            Buffer.BlockCopy(response, 2, _response, 0, _response.Length);
            return _response;
        }
    }
}
