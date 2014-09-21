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

using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor
{
    public abstract class BaseSensorDevice : CorsairLinkPlusPlus.Common.Sensor.BaseSensorDevice, ISensor
    {
        internal readonly BaseLinkDevice device;
        internal readonly int id;

        internal BaseSensorDevice(BaseLinkDevice device, int id)
            : base(device)
        {
            this.device = device;
            this.id = id;
        }

        public override string GetLocalDeviceID()
        {
            return "Sensor" + SensorType.GetName() + id;
        }

        public override DeviceType Type
        {
            get
            {
                return DeviceType.Sensor;
            }
        }

        public override string Name
        {
            get
            {
                return SensorType.GetName() + " " + id;
            }
        }
    }
}
