using HidLibrary;

namespace CorsairLinkPlusPlus.Driver.USB
{
    public class DeviceBootloader : DeviceOld
    {
        internal DeviceBootloader(RootDevice root, HidDevice hidDevice) : base(root, hidDevice) { }

        public override string GetName()
        {
            return "Corsair Bootloader";
        }
    }
}