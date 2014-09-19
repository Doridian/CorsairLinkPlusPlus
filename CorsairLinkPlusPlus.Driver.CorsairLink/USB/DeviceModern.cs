using HidLibrary;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.USB
{
    public class DeviceModern : DeviceNew
    {
        internal DeviceModern(CorsairRootDevice root, HidDevice hidDevice) : base(root, hidDevice) { }

        public override string GetName()
        {
            return GetDeviceOnChannel(0).GetName() + " USB";
        }
    }
}