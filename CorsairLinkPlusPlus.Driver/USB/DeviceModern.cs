using HidLibrary;

namespace CorsairLinkPlusPlus.Driver.USB
{
    public class DeviceModern : DeviceNew
    {
        internal DeviceModern(RootDevice root, HidDevice hidDevice) : base(root, hidDevice) { }

        public override string GetName()
        {
            return GetDeviceOnChannel(0).GetName() + " USB";
        }
    }
}