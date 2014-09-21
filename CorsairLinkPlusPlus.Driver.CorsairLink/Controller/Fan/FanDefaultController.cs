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

using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan
{
    public class FanDefaultController : TemperatureDependantControllerBase, FanController
    {
        public FanDefaultController() { }

        public FanDefaultController(Thermistor thermistor)
            : base(thermistor)
        {

        }

        internal override void Apply(Sensor.BaseSensorDevice sensor)
        {
            if(sensor is TemperatureControllableSensor)
                base.Apply(sensor);
        }

        internal override void Refresh(Sensor.BaseSensorDevice sensor)
        {
            if (sensor is TemperatureControllableSensor)
                base.Refresh(sensor);
        }

        public byte GetFanModernControllerID()
        {
            return 0x06;
        }

        public virtual void AssignFrom(Sensor.Fan fan)
        {
            if (fan is TemperatureControllableSensor)
                SetThermistor(((TemperatureControllableSensor)fan).GetTemperatureSensor());
        }
    }
}
