using CorsairLinkPlusPlus.Driver.Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public class CorsairFanFixedRPMController : CorsairControllerBase, CorsairFanController
    {
        private int rpm;

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
            ((CorsairFan)sensor).SetRPM(rpm);
        }

        internal override void Refresh(CorsairSensor sensor)
        {
 
        }

        public byte GetFanModernControllerID()
        {
            return 0x04;
        }
    }
}
