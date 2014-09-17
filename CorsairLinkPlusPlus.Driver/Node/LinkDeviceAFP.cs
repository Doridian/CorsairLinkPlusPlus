using CorsairLinkPlusPlus.Driver.Node.Internal;
using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.USB;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.Node
{
    public class LinkDeviceAFP : BaseLinkDevice
    {
        internal LinkDeviceAFP(USB.BaseUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }

        public override string GetName()
        {
            return "Corsair AirFlow Pro";
        }

        internal override List<BaseDevice> GetSubDevicesInternal()
        {
            List<BaseDevice> ret = base.GetSubDevicesInternal();

            for (int i = 0; i < 6; i++)
                ret.Add(new LinkAFPRAMStick(this, channel, i));

            return ret;
        }
    }
}
