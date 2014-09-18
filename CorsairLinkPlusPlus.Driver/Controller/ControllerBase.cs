using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Driver.Sensor;

namespace CorsairLinkPlusPlus.Driver.Controller
{
    public abstract class ControllerBase : IController
    {
        internal virtual void Apply(Sensor.BaseSensorDevice sensor)
        {

        }

        internal virtual void Refresh(Sensor.BaseSensorDevice sensor)
        {

        }
    }
}
