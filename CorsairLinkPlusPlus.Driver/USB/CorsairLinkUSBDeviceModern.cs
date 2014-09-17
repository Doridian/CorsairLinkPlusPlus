using HidLibrary;

namespace CorsairLinkPlusPlus.Driver.USB
{
    public class CorsairLinkUSBDeviceModern : CorsairLinkUSBDeviceNew
    {
        internal CorsairLinkUSBDeviceModern(CorsairRootDevice root, HidDevice hidDevice) : base(root, hidDevice) { }

        public override string GetName()
        {
            return GetDeviceOnChannel(0).GetName() + " USB";
        }
    }
}