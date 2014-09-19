using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Common.Registry;
using System.Collections;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Common.Sensor
{
    public interface IControllableSensor : ISensor
    {
        void SetController(IController controller);
        void SaveControllerData(IController controller);
        IController GetController();
        IEnumerable<RegisteredController> GetValidControllers();
    }
}
