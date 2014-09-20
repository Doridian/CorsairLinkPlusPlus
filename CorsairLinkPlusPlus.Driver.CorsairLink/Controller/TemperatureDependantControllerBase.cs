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
