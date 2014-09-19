using CorsairLinkPlusPlus.Driver.CorsairLink.Node;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor.Internal
{
    class ThermistorPSU : Thermistor
    {
        internal ThermistorPSU(LinkDevicePSU device, int id)
            : base(device, id)
        {

        }

        protected override double GetValueInternal()
        {
            DisabledCheck();

            return BitCodec.ToFloat(device.ReadRegister(0x8E, 2), 0);
        }
    }
}
