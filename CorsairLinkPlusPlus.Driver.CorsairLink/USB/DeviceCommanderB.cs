using HidSharp;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.USB
{
    public class DeviceCommanderB : DeviceOld
    {
        internal DeviceCommanderB(CorsairRootDevice root, HidDevice hidDevice) : base(root, hidDevice) { }

        public override string Name
        {
            get
            {
                return "Corsair Commander (B)";
            }
        }
    }
}