using CorsairLinkPlusPlus.Driver.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public abstract class CorsairSensor
    {
        protected readonly CorsairLinkDevice device;
        protected int id;

        internal CorsairSensor(CorsairLinkDevice device, int id)
        {
            this.device = device;
            this.id = id;
        }

        public virtual string GetName()
        {
            return GetSensorType() + " " + id;
        }

        public abstract string GetSensorType();

        public abstract double GetValue();

        public abstract string GetUnit();
    }
}
