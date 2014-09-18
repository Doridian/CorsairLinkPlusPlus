using CorsairLinkPlusPlus.Common.Controller;

namespace CorsairLinkPlusPlus.Common.Sensor
{
    public interface IControllableSensor : ISensor
    {
        void SetController(IController controller);
        void SaveControllerData(IController controller);
        IController GetController();
    }
}
