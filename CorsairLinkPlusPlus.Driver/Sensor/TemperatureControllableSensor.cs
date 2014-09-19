using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Driver.Node;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    interface TemperatureControllableSensor : IControllableSensor
    {
        void SetTemperatureSensor(IThermistor thermistor);
        void SetTemperature(double temperature);
        IThermistor GetTemperatureSensor();
    }
}
