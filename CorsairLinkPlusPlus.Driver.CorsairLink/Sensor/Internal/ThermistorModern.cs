using CorsairLinkPlusPlus.Driver.CorsairLink.Node;
using CorsairLinkPlusPlus.Driver.CorsairLink.USB;
using System;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor.Internal
{
    class ThermistorModern : Thermistor
    {
        private readonly LinkDeviceModern modernDevice;

        internal ThermistorModern(LinkDeviceModern device, int id)
            : base(device, id)
        {
            modernDevice = device;
        }

        protected override double GetValueInternal()
        {
            DisabledCheck();

            byte[] ret;
            using(var localMutexLock = CorsairRootDevice.usbGlobalMutex.GetLock())
            {
                modernDevice.SetCurrentTemp(id);
                ret = modernDevice.ReadRegister(0x0E, 2);
            }
            return ((double)BitConverter.ToInt16(ret, 0)) / 256.0;
        }
    }
}
