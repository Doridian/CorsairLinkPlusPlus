using CorsairLinkPlusPlus.Driver.Controller;
using CorsairLinkPlusPlus.Driver.Node;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public interface CorsairControllableSensor
    {
        void SetController(CorsairControllerBase controller);
    }
}
