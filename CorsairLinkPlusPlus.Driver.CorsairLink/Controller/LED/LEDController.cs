using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller.LED
{
    public interface LEDController : IController
    {
        void AssignFrom(Sensor.LED led);
        byte GetLEDModernControllerID();
    }
}
