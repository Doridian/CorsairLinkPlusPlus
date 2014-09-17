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

        internal override double GetValueInternal()
        {
            DisabledCheck();

            return modernDevice.GetCoolerRPM(id);
        }

        internal override bool IsPresentInternal()
        {
            DisabledCheck();

            return true;
        }
    }
}
