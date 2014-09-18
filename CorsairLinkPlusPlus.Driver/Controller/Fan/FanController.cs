using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Driver.Sensor;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public interface FanController : IController
    {
        void AssignFrom(Sensor.Fan fan);
        byte GetFanModernControllerID();
    }
}
