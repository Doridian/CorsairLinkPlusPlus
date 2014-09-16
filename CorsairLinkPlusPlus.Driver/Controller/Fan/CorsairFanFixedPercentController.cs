using CorsairLinkPlusPlus.Driver.Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public class CorsairFanFixedPercentController : CorsairControllerBase, CorsairFanController
    {
        private int percent;

        public CorsairFanFixedPercentController(int percent)
        {
            SetPercent(percent);
        }

        public void SetPercent(int percent)
        {
            this.percent = percent;
        }

        internal override void Apply(CorsairSensor sensor)
        {
            if (!(sensor is CorsairFan))
                throw new ArgumentException();
            ((CorsairFan)sensor).SetPercent(percent);
        }

        internal override void Refresh(CorsairSensor sensor)
        {

        }

        public byte GetFanModernControllerID()
        {
            return 0x02;
        }
    }
}
