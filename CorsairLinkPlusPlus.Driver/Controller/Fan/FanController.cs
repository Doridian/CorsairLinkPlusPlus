using CorsairLinkPlusPlus.Driver.Sensor;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public interface FanController
    {
        void AssignFrom(Sensor.Fan fan);
        byte GetFanModernControllerID();
    }
}
