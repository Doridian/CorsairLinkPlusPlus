using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Driver.Node;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    interface TemperatureControllableSensor : IControllableSensor
    {
        void SetTemperatureSensor(Thermistor thermistor);
        void SetTemperature(double temperature);
        Thermistor GetTemperatureSensor();
    }
}
