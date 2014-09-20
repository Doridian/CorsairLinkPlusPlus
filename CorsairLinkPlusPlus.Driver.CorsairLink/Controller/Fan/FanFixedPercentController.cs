using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;
using System;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan
{
    public class FanFixedPercentController : ControllerBase, FanController, IFixedNumberController
    {
        private double percent;

        public FanFixedPercentController() { }

        public FanFixedPercentController(double percent)
        {
            Value = percent;
        }

        public double Value
        {
            get
            {
                return percent;
            }
            set
            {
                this.percent = value;
            }
        }

        internal override void Apply(Sensor.BaseSensorDevice sensor)
        {
            if (!(sensor is Sensor.Fan))
                throw new ArgumentException();
            ((Sensor.Fan)sensor).SetFixedPercent(percent);
        }

        internal override void Refresh(Sensor.BaseSensorDevice sensor)
        {

        }

        public byte GetFanModernControllerID()
        {
            return 0x02;
        }

        public void AssignFrom(Sensor.Fan fan)
        {
            Value = fan.GetFixedPercent();
        }
    }
}
