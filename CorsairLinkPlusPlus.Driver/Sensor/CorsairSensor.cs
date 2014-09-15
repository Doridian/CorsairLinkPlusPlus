using CorsairLinkPlusPlus.Driver.Node;

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

        public virtual bool IsPresent()
        {
            return true;
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
