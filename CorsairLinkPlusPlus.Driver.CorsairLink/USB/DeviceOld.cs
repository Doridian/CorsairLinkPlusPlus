using HidLibrary;
using System;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.USB
{
	public abstract class DeviceOld : BaseUSBDevice
    {
        internal DeviceOld(CorsairRootDevice root, HidDevice hidDevice) : base(root, hidDevice) { }

        protected override byte[] MakeCommand(byte opcode, byte channel, byte[] payload)
        {
            byte[] command = new byte[65];
            command[0] = 0;
            command[1] = (byte)(commandNo++);
            if (commandNo > 255)
                commandNo = 20;
            command[2] = (byte)(opcode | (channel << 4));
            if (payload != null)
                Buffer.BlockCopy(payload, 0, command, 3, payload.Length);
            return command;
        }

        protected override byte[] ParseResponse(byte[] response)
        {
            byte[] newResponse = new byte[60];
            Buffer.BlockCopy(response, 2, newResponse, 0, newResponse.Length);
            return newResponse;
        }
    }
}