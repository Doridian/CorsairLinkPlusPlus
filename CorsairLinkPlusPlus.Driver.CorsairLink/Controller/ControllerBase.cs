using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller
{
    public abstract class ControllerBase : IController
    {
        internal virtual void Apply(Sensor.BaseSensorDevice sensor)
        {

        }

        internal virtual void Refresh(Sensor.BaseSensorDevice sensor)
        {

        }

        public string RegisteredName
        {
            get
            {
                return "CorsairLink." + this.GetType().Name;
            }
        }
    }
}
