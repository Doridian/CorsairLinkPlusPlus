using HidLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver
{
    public class CorsairLinkDevice
    {
        private readonly HidDevice hidDevice;
        protected int commandNo = 20;

        internal CorsairLinkDevice(HidDevice hidDevice)
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

        internal static CorsairLinkDevice CreateFromHIDDevice(HidDevice hidDevice)
        {
            switch (hidDevice.Attributes.ProductId)
            {
                case 0x0C02: /* Commander PID */
                    return CorsairLinkDeviceOldCommander.CreateFromOldHIDDevice(hidDevice);
                case 0x0C04:
                    return new CorsairLinkDevice(hidDevice);
                default:
                    return null;
            }
        }
    }

    public class CorsairLinkDeviceOldCommander : CorsairLinkDevice
    {
        internal CorsairLinkDeviceOldCommander(HidDevice hidDevice)
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

        internal static CorsairLinkDeviceOldCommander CreateFromOldHIDDevice(HidDevice hidDevice)
        {
            return new CorsairLinkDeviceOldCommander(hidDevice);
        }
    }

    public class DeviceEnumerator
    {
        public List<CorsairLinkDevice> GetDevices()
        {
            IEnumerable<HidDevice> hidDevices = HidDevices.Enumerate(0x1B1C /* Corsair VID */);

            List<CorsairLinkDevice> devices = new List<CorsairLinkDevice>();
            foreach (HidDevice hidDevice in hidDevices)
            {
                CorsairLinkDevice device = CorsairLinkDevice.CreateFromHIDDevice(hidDevice);
                if(device != null)
                    devices.Add(device);
            }
            return devices;
        }
    }
}
