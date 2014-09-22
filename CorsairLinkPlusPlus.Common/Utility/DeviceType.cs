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
namespace CorsairLinkPlusPlus.Common.Utility
{
    public enum DeviceType
    {
        Root,
        Hub,
        Sensor,
        PSU,
        Cooler,
        VirtualHub
    }

    public static class DeviceTypeExtensions
    {
        public static string GetName(this DeviceType unit)
        {
            switch (unit)
            {
                case DeviceType.Root:
                    return "Root";
                case DeviceType.Hub:
                    return "Hub";
                case DeviceType.Sensor:
                    return "Sensor";
                case DeviceType.PSU:
                    return "PSU";
                case DeviceType.Cooler:
                    return "Cooler";
                case DeviceType.VirtualHub:
                    return "Virtual Hub";
            }
            return "N/A";
        }
    }
}
