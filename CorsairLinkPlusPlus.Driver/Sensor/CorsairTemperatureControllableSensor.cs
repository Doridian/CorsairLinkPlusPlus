using CorsairLinkPlusPlus.Driver.Node;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public interface CorsairTemperatureControllableSensor : CorsairControllableSensor
    {
        void SetTemperatureSensor(CorsairThermistor thermistor);
        void SetTemperature(double temperature);
        CorsairThermistor GetTemperatureSensor();
    }
}
