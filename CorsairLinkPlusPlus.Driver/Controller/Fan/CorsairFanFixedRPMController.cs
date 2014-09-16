using CorsairLinkPlusPlus.Driver.Sensor;
using System;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public class CorsairFanFixedRPMController : CorsairControllerBase, CorsairFanController
    {
        private int rpm;

        public CorsairFanFixedRPMController()
        {

        }

        public CorsairFanFixedRPMController(int rpm)
        {
            SetRPM(rpm);
        }

        public void SetRPM(int rpm)
        {
            this.rpm = rpm;
        }

        internal override void Apply(CorsairSensor sensor)
        {
            if (!(sensor is CorsairFan))
                throw new ArgumentException();
            ((CorsairFan)sensor).SetFixedRPM(rpm);
        }

        internal override void Refresh(CorsairSensor sensor)
        {
 
        }

        public byte GetFanModernControllerID()
        {
            return 0x04;
        }

        public void AssignFrom(CorsairFan fan)
        {
            SetRPM(fan.GetFixedRPM());
        }
    }
}
