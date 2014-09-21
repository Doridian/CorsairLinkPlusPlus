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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Common.Utility
{
    public enum Unit {
        DegreeCelsius,
        Ampere,
        Volt,
        Watt,
        RPM,
        Percent,
        Color,
        Unknown
    }

    public static class UnitExtensions
    {
        public static string GetPostfix(this Unit unit, bool longFormat = false)
        {
            switch (unit)
            {
                case Unit.DegreeCelsius:
                    return longFormat ? "Degrees Celsius" : "°C";
                case Unit.Ampere:
                    return longFormat ? "Ampere" : "A";
                case Unit.Volt:
                    return longFormat ? "Volt" : "V";
                case Unit.Watt:
                    return longFormat ? "Watt" : "W";
                case Unit.RPM:
                    return longFormat ? "Rounds per Minute" : "RPM";
                case Unit.Percent:
                    return longFormat ? "Percent" : "%";
                case Unit.Color:
                    return longFormat ? "Color" : "RGB";

            }
            return longFormat ? "Unknown" : "N/A";
        }
    }
}
