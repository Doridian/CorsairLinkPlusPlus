using CorsairLinkPlusPlus.Driver.Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Node
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
