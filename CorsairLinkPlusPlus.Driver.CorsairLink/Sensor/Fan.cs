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
using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node;
using CorsairLinkPlusPlus.Driver.CorsairLink.Utility;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor
{
    public abstract class Fan : Cooler, IFan
    {
        protected bool? cachedPWM = null;

        internal Fan(BaseLinkDevice device, int id)
            : base(device, id)
        {

        }

        public bool PWM
        {
            get
            {
                if (cachedPWM == null)
                    cachedPWM = IsPWMInternal();
                return (bool)cachedPWM;
            }
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            if (!volatileOnly)
                cachedPWM = null;
        }

        internal abstract void SetFixedRPM(double fixedRPM);
        internal abstract void SetFixedPercent(double percent);
        internal abstract double GetFixedRPM();
        internal abstract double GetFixedPercent();
        internal abstract ControlCurve<double, double> GetControlCurve();
        internal abstract void SetControlCurve(ControlCurve<double, double> curve);
        internal abstract void SetMinimalRPM(double rpm);
        internal abstract double GetMinimalRPM();

        internal virtual bool IsPWMInternal()
        {
            return false;
        }

        public override SensorType SensorType
        {
            get
            {
                return SensorType.Fan;
            }
        }
    }
}
