using HidLibrary;
using System;

namespace CorsairLinkPlusPlus.Driver.USB
{
	public abstract class CorsairLinkUSBDeviceOld : CorsairLinkUSBDevice
    {
        internal CorsairLinkUSBDeviceOld(HidDevice hidDevice) : base(hidDevice) { }

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