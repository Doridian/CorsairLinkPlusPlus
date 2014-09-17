using CorsairLinkPlusPlus.Driver.Node;

namespace CorsairLinkPlusPlus.Driver.Sensor.Internal
{
    class ThermistorPSU : Thermistor
    {
        internal ThermistorPSU(LinkDevicePSU device, int id)
            : base(device, id)
        {

        }

        internal override double GetValueInternal()
        {
            DisabledCheck();

            return BitCodec.ToFloat(device.ReadRegister(0x8E, 2), 0);
        }
    }
}
