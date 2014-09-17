using HidLibrary;
using System;

namespace CorsairLinkPlusPlus.Driver.USB
{
	public abstract class CorsairLinkUSBDeviceNew : CorsairLinkUSBDevice
    {
        internal CorsairLinkUSBDeviceNew(CorsairRootDevice root, HidDevice hidDevice) : base(root, hidDevice) { }

        protected override byte[] ParseResponse(byte[] response)
        {
            byte[] _response = new byte[60];
            Buffer.BlockCopy(response, 3, _response, 0, _response.Length);
            return _response;
        }

        protected override byte[] MakeCommand(byte opcode, byte channel, byte[] payload)
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
                _command[1] = 2;
            return _command;
        }
    }
}