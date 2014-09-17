using CorsairLinkPlusPlus.Driver.Node;
using System;

namespace CorsairLinkPlusPlus.Driver.Sensor.Internal
{
    class ThermistorModern : Thermistor
    {
        private readonly LinkDeviceModern modernDevice;

        internal ThermistorModern(LinkDeviceModern device, int id)
            : base(device, id)
        {
            modernDevice = device;
        }

        internal override double GetValueInternal()
        {
            DisabledCheck();

            byte[] ret;
            lock (modernDevice.usbDevice.usbLock)
            {
                modernDevice.SetCurrentTemp(id);
                ret = modernDevice.ReadRegister(0x0E, 2);
            }
            return ((double)BitConverter.ToInt16(ret, 0)) / 256.0;
        }
    }
}
