using CorsairLinkPlusPlus.Driver.Node;
using CorsairLinkPlusPlus.Driver.Node.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Sensor.Internal
{
    public class PSUMainsEfficiencySensor : BasePowerSensor
    {
        private readonly PSUMainsPowerDevice powerDevice;

        internal PSUMainsEfficiencySensor(PSUMainsPowerDevice device)
            : base(device, 0)
        {
            powerDevice = device;
        }

        public override string GetSensorType()
        {
            return "Efficiency";
        }

        protected override double GetValueInternal()
        {
            return powerDevice.ReadEfficiency();
        }

        public override string GetUnit()
        {
            return "%";
        }
    }
}
