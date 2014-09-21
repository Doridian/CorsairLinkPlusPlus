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

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor.Internal
{
    internal class Secondary12VCurrentSensor : CurrentSensor
    {
        private readonly string name;
        private readonly LinkDevicePSU psuDevice;

        internal Secondary12VCurrentSensor(LinkDevicePSU device, int id, string name)
            : base(device, id)
        {
            this.name = name;
            this.psuDevice = device;
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            psuDevice.Refresh(true);
        }

        protected override double GetValueInternal()
        {
            DisabledCheck();
            return psuDevice.GetSecondary12VCurrent(id);
        }

        public override string Name
        {
            get
            {
                return name + " Current";
            }
        }
    }
}
