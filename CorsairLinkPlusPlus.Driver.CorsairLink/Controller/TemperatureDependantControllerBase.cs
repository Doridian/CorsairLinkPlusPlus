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
using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;
using System;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller
{
    public class TemperatureDependantControllerBase : ControllerBase, ITemperatureDependantController
    {
        private IThermistor thermistor;

        public TemperatureDependantControllerBase() { }

        public TemperatureDependantControllerBase(IThermistor thermistor)
        {
            SetThermistor(thermistor);
        }

        public void SetThermistor(IThermistor thermistor)
        {
            this.thermistor = thermistor;
        }

        public IThermistor GetThermistor()
        {
            return this.thermistor;
        }

        internal virtual double GetTemperature()
        {
            thermistor.Refresh(true);
            return thermistor.Value;
        }

        internal override void Apply(Sensor.BaseSensorDevice sensor)
        {
            if (!(sensor is TemperatureControllableSensor))
                throw new ArgumentException();
            ((TemperatureControllableSensor)sensor).SetTemperatureSensor(thermistor);
            Refresh(sensor);
        }

        internal override void Refresh(Sensor.BaseSensorDevice sensor)
        {
            if (!(sensor is TemperatureControllableSensor))
                throw new ArgumentException();
            if (Core.DoesThermistorNeedManualPush(sensor, thermistor))
                ((TemperatureControllableSensor)sensor).SetTemperature(GetTemperature());
        }
    }
}
