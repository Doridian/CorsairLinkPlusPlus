using CorsairLinkPlusPlus.Driver.Sensor;
using System;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public class CorsairFanFixedPercentController : CorsairControllerBase, CorsairFanController
    {
        private int percent;

        public CorsairFanFixedPercentController()
        {

        }
        
        public CorsairFanFixedPercentController(int percent)
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

        internal override void Apply(CorsairSensor sensor)
        {
            if (!(sensor is CorsairFan))
                throw new ArgumentException();
            ((CorsairFan)sensor).SetFixedPercent(percent);
        }

        internal override void Refresh(CorsairSensor sensor)
        {

        }

        public byte GetFanModernControllerID()
        {
            return 0x02;
        }

        public void AssignFrom(CorsairFan fan)
        {
            SetPercent(fan.GetFixedPercent());
        }
    }
}
