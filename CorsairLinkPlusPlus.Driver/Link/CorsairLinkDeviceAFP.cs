using CorsairLinkPlusPlus.Driver.USB;

namespace CorsairLinkPlusPlus.Driver.Link
{
    public class CorsairLinkDeviceAFP : CorsairLinkDevice
    {
        internal CorsairLinkDeviceAFP(CorsairLinkUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }
        public override string GetDeviceName()
        {
            return "AirFlow Pro";
        }
    }
}
