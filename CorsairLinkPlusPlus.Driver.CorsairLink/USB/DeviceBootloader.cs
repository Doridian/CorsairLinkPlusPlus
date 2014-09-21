using HidSharp;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.USB
{
    public class DeviceBootloader : DeviceOld
    {
        internal DeviceBootloader(CorsairRootDevice root, HidDevice hidDevice) : base(root, hidDevice) { }

        public override string Name
        {
            get
            {
                return "Corsair Bootloader";
            }
        }
    }
}