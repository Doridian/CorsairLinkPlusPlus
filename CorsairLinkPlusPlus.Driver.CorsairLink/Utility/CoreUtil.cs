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

using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;
using System.Text;

namespace CorsairLinkPlusPlus.Driver.CorsairLink
{
    public static class CoreUtil
    {
        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2} ", b);
            return hex.ToString();
        }

        internal static byte GetRelativeThermistorByte(Sensor.BaseSensorDevice sensor, IThermistor thermistor)
        {
            if (thermistor == null)
                return 0;
            if (!(thermistor is Thermistor))
                return 7;
            Thermistor thermistorCast = (Thermistor)thermistor;
            return (byte)((sensor.device == thermistorCast.device) ? thermistorCast.id : 7);
        }

        internal static bool DoesThermistorNeedManualPush(Sensor.BaseSensorDevice sensor, IThermistor thermistor)
        {
            return GetRelativeThermistorByte(sensor, thermistor) == 7;
        }
    }
}

