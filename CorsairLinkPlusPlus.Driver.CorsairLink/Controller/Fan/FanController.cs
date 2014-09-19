using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan
{
    public interface FanController : IController
    {
        void AssignFrom(Sensor.Fan fan);
        byte GetFanModernControllerID();
    }
}
