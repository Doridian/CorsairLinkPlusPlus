using CorsairLinkPlusPlus.Driver.Sensor;
using System;

namespace CorsairLinkPlusPlus.Driver.Controller
{
    public class CorsairTemperatureDependantControllerBase : CorsairControllerBase
    {
        private CorsairThermistor thermistor;

        public CorsairTemperatureDependantControllerBase()
        {

        }

        public CorsairTemperatureDependantControllerBase(CorsairThermistor thermistor)
        {
            SetThermistor(thermistor);
        }

        public void SetThermistor(CorsairThermistor thermistor)
        {
            this.thermistor = thermistor;
        }

        internal virtual double GetTemperature()
        {
            thermistor.Refresh(true);
            return thermistor.GetValue();
        }

        internal override void Apply(CorsairSensor sensor)
        {
            if (!(sensor is CorsairTemperatureControllableSensor))
                throw new ArgumentException();
            ((CorsairTemperatureControllableSensor)sensor).SetTemperatureSensor(thermistor);
            Refresh(sensor);
        }

        internal override void Refresh(CorsairSensor sensor)
        {
            if (!(sensor is CorsairTemperatureControllableSensor))
                throw new ArgumentException();
            if (CorsairUtility.DoesThermistorNeedManualPush(sensor, thermistor))
                ((CorsairTemperatureControllableSensor)sensor).SetTemperature(GetTemperature());
        }
    }
}
