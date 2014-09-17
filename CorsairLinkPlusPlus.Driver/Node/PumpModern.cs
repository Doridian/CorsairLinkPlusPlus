using CorsairLinkPlusPlus.Driver.Sensor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Node
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
