using HidSharp;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.USB
{
    public class DeviceModern : DeviceNew
    {
        internal DeviceModern(CorsairRootDevice root, HidDevice hidDevice) : base(root, hidDevice) { }

        public override string Name
        {
            get
            {
                return GetDeviceOnChannel(0).Name + " USB";
            }
        }
    }
}