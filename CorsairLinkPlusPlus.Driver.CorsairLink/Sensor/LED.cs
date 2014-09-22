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
using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node;
using CorsairLinkPlusPlus.Driver.CorsairLink.Utility;
using System;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor
{
    public abstract class LED : BaseSensorDevice, ILED
    {
        byte[] rgbCache = null;

        internal LED(BaseLinkDevice device, int id)
            : base(device, id)
        {

        }

        protected override object GetValueInternal()
        {
            byte[] rgb = GetRGB();
            return new Color(rgb[0], rgb[1], rgb[2]);
        }

        internal abstract byte[] GetFixedRGBCycleColors();

        internal abstract void SetFixedRGBCycleColors(byte[] colors);

        internal abstract ControlCurve<double, Color> GetControlCurve();

        internal abstract void SetControlCurve(ControlCurve<double, Color> colors);

        public byte[] GetRGB()
        {
            if (rgbCache == null)
                rgbCache = GetRGBInternal();
            return rgbCache;
        }

        internal void SetColor(Color color)
        {
            SetRGB(new byte[] { color.R, color.G, color.B });
        }

        internal abstract void SetRGB(byte[] rgb);

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            rgbCache = null;
        }

        internal abstract byte[] GetRGBInternal();

        public override SensorType SensorType
        {
            get
            {
                return SensorType.LED;
            }
        }

        public override Unit Unit
        {
            get
            {
                return Unit.Color;
            }
        }
    }
}
