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

using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;
using System;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan
{
    public class FanFixedPercentController : ControllerBase, FanController, IFixedNumberController
    {
        private double percent;

        public FanFixedPercentController() { }

        public FanFixedPercentController(double percent)
        {
            Value = percent;
        }

        public double Value
        {
            get
            {
                return percent;
            }
            set
            {
                this.percent = value;
            }
        }

        internal override void Apply(Sensor.BaseSensorDevice sensor)
        {
            if (!(sensor is Sensor.Fan))
                throw new ArgumentException();
            ((Sensor.Fan)sensor).SetFixedPercent(percent);
        }

        internal override void Refresh(Sensor.BaseSensorDevice sensor)
        {

        }

        public byte GetFanModernControllerID()
        {
            return 0x02;
        }

        public void AssignFrom(Sensor.Fan fan)
        {
            Value = fan.GetFixedPercent();
        }
    }
}
