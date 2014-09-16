using CorsairLinkPlusPlus.Driver.Sensor;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public class CorsairFanDefaultController : CorsairControllerBase, CorsairFanController
    {
        public CorsairFanDefaultController()
        {

        }

        internal override void Apply(CorsairSensor sensor)
        {

        }

        internal override void Refresh(CorsairSensor sensor)
        {
 
        }

        public byte GetFanModernControllerID()
        {
            return 0x06;
        }

        public void AssignFrom(CorsairFan fan)
        {

        }
    }
}
