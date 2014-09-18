using CorsairLinkPlusPlus.Driver.Node;

namespace CorsairLinkPlusPlus.Driver.Sensor.Internal
{
    class PumpModern : Pump
    {
        private readonly LinkDeviceModern modernDevice;

        internal PumpModern(LinkDeviceModern device, int id)
            : base(device, id)
        {
            modernDevice = device;
        }

        protected override double GetValueInternal()
        {
            DisabledCheck();

            return modernDevice.GetCoolerRPM(id);
        }
    }
}
