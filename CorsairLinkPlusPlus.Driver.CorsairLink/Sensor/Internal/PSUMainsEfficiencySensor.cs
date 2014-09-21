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

using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node.Internal;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor.Internal
{
    public class PSUMainsEfficiencySensor : BasePowerSensor
    {
        private readonly PSUMainsPowerDevice powerDevice;

        internal PSUMainsEfficiencySensor(PSUMainsPowerDevice device)
            : base(device, 0)
        {
            powerDevice = device;
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            powerDevice.Refresh(true);
        }

        public override SensorType SensorType
        {
            get
            {
                return SensorType.Efficiency;
            }
        }

        protected override double GetValueInternal()
        {
            return powerDevice.ReadEfficiency();
        }

        public override Unit Unit
        {
            get
            {
                return Unit.Percent;
            }
        }
    }
}
