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

        protected virtual byte[] MakeCommand(byte commandID, byte channelID, byte[] command)
        {
            byte[] _command = new byte[65];
            _command[0] = 0;
            _command[1] = (byte)(command.Length + 2);
            _command[2] = (byte)(commandNo++);
            if (commandNo > 255)
                commandNo = 20;
            _command[3] = (byte)(commandID | (channelID << 4));
            Buffer.BlockCopy(command, 0, _command, 4, command.Length);
            return _command;
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

        public byte[] SendCommand(byte commandID, byte channelID, byte[] command)
        {
            hidDevice.Write(MakeCommand(commandID, channelID, command), 500);
            return ReadResponse();
        }

        internal static CorsairLinkUSBDevice CreateFromHIDDevice(HidDevice hidDevice)
        {
            switch (hidDevice.Attributes.ProductId)
            {
                case 0x0C02: /* Commander PID */
                    return CorsairLinkUSBDeviceOld.CreateFromOldHIDDevice(hidDevice);
                case 0x0C04:
                    return new CorsairLinkUSBDevice(hidDevice);
                default:
                    return null;
            }
        }
    }

    public class CorsairLinkUSBDeviceOld : CorsairLinkUSBDevice
    {
        internal CorsairLinkUSBDeviceOld(HidDevice hidDevice)
            : base(hidDevice)
        {

        }

        protected override byte[] MakeCommand(byte commandID, byte channelID, byte[] command)
        {
            byte[] _command = new byte[65];
            _command[0] = 0;
            _command[1] = (byte)(commandNo++);
            if (commandNo > 255)
                commandNo = 20;
            _command[2] = (byte)(commandID | (channelID << 4));
            Buffer.BlockCopy(command, 0, _command, 3, command.Length);
            return _command;
        }

        protected override byte[] ParseResponse(byte[] response)
        {
            byte[] _response = new byte[60];
            Buffer.BlockCopy(response, 2, _response, 0, _response.Length);
            return _response;
        }

        internal static CorsairLinkUSBDeviceOld CreateFromOldHIDDevice(HidDevice hidDevice)
        {
            return new CorsairLinkUSBDeviceOld(hidDevice);
        }
    }
}
