using CorsairLinkPlusPlus.Driver.Controller;
using CorsairLinkPlusPlus.Driver.Node;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public interface ControllableSensor
    {
        void SetController(ControllerBase controller);
        void SaveControllerData(ControllerBase controller);
        ControllerBase GetController();
    }
}
