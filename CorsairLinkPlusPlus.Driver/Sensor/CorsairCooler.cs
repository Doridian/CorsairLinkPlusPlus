using CorsairLinkPlusPlus.Driver.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public class CorsairCooler : CorsairSensor
    {
        internal CorsairCooler(CorsairLinkDevice device, int id)
            : base(device, id)
        {

        }

        public override string GetSensorType()
        {
            return device.GetCoolerType(id);
        }

        public override double GetValue()
        {
            return device.GetCoolerRPM(id);
        }

        public override string GetUnit()
        {
            return "RPM";
        }
    }
}
