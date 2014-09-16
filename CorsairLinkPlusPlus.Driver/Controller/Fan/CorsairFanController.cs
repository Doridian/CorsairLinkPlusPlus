using CorsairLinkPlusPlus.Driver.Sensor;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public interface CorsairFanController
    {
        void AssignFrom(CorsairFan fan);
        byte GetFanModernControllerID();
    }
}
