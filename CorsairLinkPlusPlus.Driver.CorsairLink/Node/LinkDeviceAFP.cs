using CorsairLinkPlusPlus.Common;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node.Internal;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;
using CorsairLinkPlusPlus.Driver.CorsairLink.USB;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Node
{
    public class LinkDeviceAFP : BaseLinkDevice
    {
        internal LinkDeviceAFP(USB.BaseUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }

        public override string Name
        {
            get
            {
                return "Corsair AirFlow Pro";
            }
        }

        protected override List<IDevice> GetSubDevicesInternal()
        {
            List<IDevice> ret = base.GetSubDevicesInternal();

            for (int i = 0; i < 6; i++)
                ret.Add(new LinkAFPRAMStick(this, channel, i));

            return ret;
        }
    }
}
