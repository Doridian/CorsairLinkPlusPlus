using CorsairLinkPlusPlus.Driver.Node;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public interface CorsairTemperatureControllableSensor : CorsairControllableSensor
    {
        void SetTemperatureSensor(int idx);
        void SetTemperature(double temperature);
    }
}
