using CorsairLinkPlusPlus.Driver.Sensor;

namespace CorsairLinkPlusPlus.Driver.Controller
{
    public class CorsairTemperatureDependantControllerBase : CorsairControllerBase
    {
        private CorsairThermistor thermistor;

        public CorsairTemperatureDependantControllerBase(CorsairThermistor thermistor)
        {
            SetThermistor(thermistor);
        }

        public void SetThermistor(CorsairThermistor thermistor)
        {
            this.thermistor = thermistor;
        }

        public virtual double GetTemperature()
        {
            thermistor.Refresh(true);
            return thermistor.GetValue();
        }

        public void Apply(CorsairTemperatureControllableSensor sensor)
        {
            sensor.SetTemperatureSensor(CorsairUtility.GetRelativeThermistorByte(sensor, thermistor));
            Refresh(sensor);
        }

        internal void Refresh(CorsairTemperatureControllableSensor sensor)
        {
            if (CorsairUtility.DoesThermistorNeedManualPush(sensor, thermistor))
                sensor.SetTemperature(GetTemperature());
        }
    }
}
