using CorsairLinkPlusPlus.Driver.Node;

namespace CorsairLinkPlusPlus.Driver.Sensor
{
    public abstract class CorsairCooler : CorsairSensor
    {
        internal CorsairCooler(CorsairLinkDevice device, int id)
            : base(device, id)
        {

        }

        public override string GetUnit()
        {
            return "RPM";
        }
    }
}
