using HidLibrary;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.USB
{
    public class DeviceCommanderB : DeviceOld
    {
        internal DeviceCommanderB(CorsairRootDevice root, HidDevice hidDevice) : base(root, hidDevice) { }

        public override string GetName()
        {
            return "Corsair Commander (B)";
        }
    }
}