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
using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Utility;
using System;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller.LED
{
    public class SingleColor : ControllerBase, ILEDController, IFixedColorController
    {
        private Color m_color;

        public SingleColor()
        {
            
        }

        public SingleColor(Color color)
        {
            m_color = color;
        }

        public byte GetLEDModernControllerID()
        {
            return 0x00;
        }

        public Color Value
        {
            get
            {
                return m_color;
            }
            set
            {
                m_color = value;
            }
        }

        internal override void Apply(Sensor.BaseSensorDevice sensor)
        {
            if (!(sensor is Sensor.LED))
                throw new ArgumentException();
            base.Apply(sensor);

            ((Sensor.LED)sensor).SetFixedRGBCycleColors(m_color.ToArray());
        }

        public void AssignFrom(Sensor.LED led)
        {
            byte[] colorData = led.GetFixedRGBCycleColors();
            m_color = new Color(colorData[0], colorData[1], colorData[2]);
        }
    }
}
