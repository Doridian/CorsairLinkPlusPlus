using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Driver.Sensor;
using System;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public class FanFixedRPMController : ControllerBase, FanController, IFixedNumberController
    {
        private double rpm;

        public FanFixedRPMController() { }

        public FanFixedRPMController(double rpm)
        {
            SetValue(rpm);
        }

        public void SetValue(double rpm)
        {
            this.rpm = rpm;
        }

        public double GetValue()
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
            SetValue(fan.GetFixedRPM());
        }
    }
}
