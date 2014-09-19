using HidLibrary;
using System;

namespace CorsairLinkPlusPlus.Driver.USB
{
	public abstract class DeviceNew : BaseUSBDevice
    {
        internal DeviceNew(RootDevice root, HidDevice hidDevice) : base(root, hidDevice) { }

        protected override byte[] ParseResponse(byte[] response)
        {
            byte[] newResponse = new byte[60];
            Buffer.BlockCopy(response, 3, newResponse, 0, newResponse.Length);
            return newResponse;
        }

        protected override byte[] MakeCommand(byte opcode, byte channel, byte[] payload)
        {
            byte[] command = new byte[65];
            command[0] = 0;
            command[2] = (byte)(commandNo++);
            if (commandNo > 255)
                commandNo = 20;
            command[3] = (byte)(opcode | (channel << 4));
            if (payload != null)
            {
                command[1] = (byte)(payload.Length + 2);
                Buffer.BlockCopy(payload, 0, command, 4, payload.Length);
            }
            else
                command[1] = 2;
            return command;

        }
    }
}