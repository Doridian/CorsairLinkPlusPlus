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
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;
using System;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller.LED
{
    public class CustomCurve : TemperatureDependantControllerBase, ILEDController, ICurveColorController
    {
        protected ControlCurve<double, Color> curve;

        public CustomCurve()
        {

        }

        public CustomCurve(ControlCurve<double, Color> colors)
        {
            Value = colors;
        }

        internal override void Apply(Sensor.BaseSensorDevice sensor)
        {
            if(!(sensor is Sensor.LED))
                throw new ArgumentException();
            base.Apply(sensor);

            ((Sensor.LED)sensor).SetControlCurve(curve);
        }

        public ControlCurve<double, Color> Value
        {
            get
            {
                return curve.Copy();
            }
            set
            {
                curve = value.Copy();
            }
        }

        public virtual void AssignFrom(Sensor.LED led)
        {
            SetThermistor(((TemperatureControllableSensor)led).GetTemperatureSensor());
            curve = led.GetControlCurve();
        }

        protected int GetNumColors()
        {
            return 3;
        }

        private Color[] CopyNumColorArray(Color[] src, Color[] dst)
        {
            int numColors = GetNumColors();
            if (src.Length != numColors || dst.Length != numColors)
                throw new ArgumentException();

            for (int i = 0; i < numColors; i++)
                dst[i] = new Color(src[i]);

            return dst;
        }

        public byte GetLEDModernControllerID()
        {
            return 0xC0;
        }
    }
}
