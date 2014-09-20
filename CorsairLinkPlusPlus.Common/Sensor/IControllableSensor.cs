using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Common.Registry;
using System.Collections;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Common.Sensor
{
    public interface IControllableSensor : ISensor
    {
        void SaveControllerData(IController controller);
        IController Controller { get; set; }
        IEnumerable<RegisteredController> GetValidControllers();
        IEnumerable<string> ValidControllerNames { get; }
    }
}
