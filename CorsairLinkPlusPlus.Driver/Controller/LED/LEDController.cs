using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Driver.Sensor;

namespace CorsairLinkPlusPlus.Driver.Controller.LED
{
    public interface LEDController : IController
    {
        void AssignFrom(Sensor.LED led);
        byte GetLEDModernControllerID();
    }
}
