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
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;
using CorsairLinkPlusPlus.Driver.CorsairLink.Utility;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan
{
    public class QuiteMode : BaseCurve
    {
        public QuiteMode() { }

        public QuiteMode(Thermistor thermistor)
            : base(thermistor)
        {

        }

        public override ControlCurve<double, double> GetDefaultPoints()
        {
            return new ControlCurve<double, double>(
                new CurvePoint<double, double>(28, 900),
                new CurvePoint<double, double>(31, 1000),
                new CurvePoint<double, double>(34, 1100),
                new CurvePoint<double, double>(37, 1200),
                new CurvePoint<double, double>(40, 1300)
            );
        }

        public override byte GetFanModernControllerID()
        {
            return 0x08;
        }
    }
}
