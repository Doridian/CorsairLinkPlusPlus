using CorsairLinkPlusPlus.Driver.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public class CorsairPump : CorsairCooler
    {
        internal CorsairPump(CorsairLinkDevice device, int id)
            : base(device, id)
        {

        }

        public override string GetSensorType()
        {
            return "Pump";
        }
    }
}
