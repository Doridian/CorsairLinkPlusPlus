using HidLibrary;

namespace CorsairLinkPlusPlus.Driver.USB
{
    public class CorsairLinkUSBDeviceModern : CorsairLinkUSBDeviceNew
    {
        internal CorsairLinkUSBDeviceModern(HidDevice hidDevice) : base(hidDevice) { }

        public override string GetName()
        {
            return GetDeviceOnChannel(0).GetName() + " USB";
        }
    }
}