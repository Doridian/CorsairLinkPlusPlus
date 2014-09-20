using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;
using System;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan
{
    public class FanFixedRPMController : ControllerBase, FanController, IFixedNumberController
    {
        private double rpm;

        public FanFixedRPMController() { }

        public FanFixedRPMController(double rpm)
        {
            Value = rpm;
        }

        public double Value
        {
            get
            {
                return rpm;
            }
            set
            {
                this.rpm = value;
            }
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
            Value = fan.GetFixedRPM();
        }
    }
}
