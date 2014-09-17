using CorsairLinkPlusPlus.Driver.Node;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    interface TemperatureControllableSensor : ControllableSensor
    {
        void SetTemperatureSensor(Thermistor thermistor);
        void SetTemperature(double temperature);
        Thermistor GetTemperatureSensor();
    }
}
