using CorsairLinkPlusPlus.Driver.Sensor;

namespace CorsairLinkPlusPlus.Driver.Controller.LED
{
    public interface CorsairLEDController
    {
        void AssignFrom(CorsairLED fan);
        byte GetLEDModernControllerID();
    }
}
