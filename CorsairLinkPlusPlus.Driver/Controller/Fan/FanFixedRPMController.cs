using CorsairLinkPlusPlus.Driver.Sensor;
using System;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public class FanFixedRPMController : ControllerBase, FanController
    {
        private int rpm;

        public FanFixedRPMController() { }

        public FanFixedRPMController(int rpm)
        {
            SetRPM(rpm);
        }

        public void SetRPM(int rpm)
        {
            this.rpm = rpm;
        }
        public int GetRPM()
        {
            return rpm;
        }

        internal override void Apply(Sensor.BaseSensorDevice sensor)
        {
            if (!(sensor is Sensor.Fan))
                throw new ArgumentException();
            ((Sensor.Fan)sensor).SetFixedRPM(rpm);
        }

        internal override void Refresh(Sensor.BaseSensorDevice sensor)
        {
 
        }

        public byte GetFanModernControllerID()
        {
            return 0x04;
        }

        public void AssignFrom(Sensor.Fan fan)
        {
            SetRPM(fan.GetFixedRPM());
        }
    }
}
