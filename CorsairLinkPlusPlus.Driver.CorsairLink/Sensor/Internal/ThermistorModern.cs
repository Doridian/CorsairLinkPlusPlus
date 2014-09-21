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

using CorsairLinkPlusPlus.Driver.CorsairLink.Node;
using CorsairLinkPlusPlus.Driver.CorsairLink.USB;
using System;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor.Internal
{
    class ThermistorModern : Thermistor
    {
        private readonly LinkDeviceModern modernDevice;

        internal ThermistorModern(LinkDeviceModern device, int id)
            : base(device, id)
        {
            modernDevice = device;
        }

        protected override double GetValueInternal()
        {
            DisabledCheck();

            byte[] ret;
            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentTemp(id);
                ret = modernDevice.ReadRegister(0x0E, 2);
            }
            return ((double)BitConverter.ToInt16(ret, 0)) / 256.0;
        }
    }
}
