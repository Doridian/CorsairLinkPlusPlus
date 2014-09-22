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

using HidSharp;
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
    }
}