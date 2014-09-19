using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Driver.Sensor;
using System;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public class FanFixedPercentController : ControllerBase, FanController, IFixedNumberController
    {
        private double percent;

        public FanFixedPercentController() { }

        public FanFixedPercentController(double percent)
        {
            SetValue(percent);
        }

        public void SetValue(double percent)
        {
            this.percent = percent;
        }

        public double GetValue()
        {
            return percent;
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
            SetValue(fan.GetFixedPercent());
        }
    }
}
