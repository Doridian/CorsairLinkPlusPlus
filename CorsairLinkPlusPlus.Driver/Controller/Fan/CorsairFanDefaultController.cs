using CorsairLinkPlusPlus.Driver.Sensor;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public class CorsairFanDefaultController : CorsairTemperatureDependantControllerBase, CorsairFanController
    {
        public CorsairFanDefaultController()
        {

        }

        public CorsairFanDefaultController(CorsairThermistor thermistor)
            : base(thermistor)
        {

        }

        public byte GetFanModernControllerID()
        {
            return 0x06;
        }

        public virtual void AssignFrom(CorsairFan fan)
        {
            SetThermistor(((CorsairTemperatureControllableSensor)fan).GetTemperatureSensor());
        }
    }
}
