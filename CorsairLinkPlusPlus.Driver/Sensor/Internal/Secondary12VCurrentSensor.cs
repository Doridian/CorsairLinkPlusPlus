using CorsairLinkPlusPlus.Driver.Node;

namespace CorsairLinkPlusPlus.Driver.Sensor.Internal
{
    internal class Secondary12VCurrentSensor : CurrentSensor
    {
        private readonly string name;
        private readonly LinkDevicePSU psuDevice;

        internal Secondary12VCurrentSensor(LinkDevicePSU device, int id, string name)
            : base(device, id)
        {
            this.name = name;
            this.psuDevice = device;
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            psuDevice.Refresh(true);
        }

        protected override double GetValueInternal()
        {
            DisabledCheck();
            return psuDevice.GetSecondary12VCurrent(id);
        }

        public override string GetName()
        {
            return name + " Current";
        }
    }
}
