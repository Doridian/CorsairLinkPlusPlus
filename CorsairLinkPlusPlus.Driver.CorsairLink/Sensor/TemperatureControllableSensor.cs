using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor
{
    interface TemperatureControllableSensor : IControllableSensor
    {
        void SetTemperatureSensor(IThermistor thermistor);
        void SetTemperature(double temperature);
        IThermistor GetTemperatureSensor();
    }
}
