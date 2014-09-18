using CorsairLinkPlusPlus.Driver.Sensor;

namespace CorsairLinkPlusPlus.Driver.Controller.LED
{
    public interface LEDController
    {
        void AssignFrom(Sensor.LED led);
        byte GetLEDModernControllerID();
    }
}
