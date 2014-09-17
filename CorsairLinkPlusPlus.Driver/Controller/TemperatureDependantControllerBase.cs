using CorsairLinkPlusPlus.Driver.Sensor;
using System;

namespace CorsairLinkPlusPlus.Driver.Controller
{
    public class TemperatureDependantControllerBase : ControllerBase
    {
        private Thermistor thermistor;

        public TemperatureDependantControllerBase() { }

        public TemperatureDependantControllerBase(Thermistor thermistor)
        {
            SetThermistor(thermistor);
        }

        public void SetThermistor(Thermistor thermistor)
        {
            this.thermistor = thermistor;
        }

        internal virtual double GetTemperature()
        {
            thermistor.Refresh(true);
            return thermistor.GetValue();
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
