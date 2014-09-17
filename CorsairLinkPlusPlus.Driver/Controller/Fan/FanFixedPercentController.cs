using CorsairLinkPlusPlus.Driver.Sensor;
using System;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public class FanFixedPercentController : ControllerBase, FanController
    {
        private int percent;

        public FanFixedPercentController()
        {

        }
        
        public FanFixedPercentController(int percent)
        {
            SetPercent(percent);
        }

        public void SetPercent(int percent)
        {
            this.percent = percent;
        }

        public int GetPercent()
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
            SetPercent(fan.GetFixedPercent());
        }
    }
}
