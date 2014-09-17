using HidLibrary;

namespace CorsairLinkPlusPlus.Driver.USB
{
    public class CorsairLinkUSBDeviceBootloader : CorsairLinkUSBDeviceOld
    {
        internal CorsairLinkUSBDeviceBootloader(CorsairRootDevice root, HidDevice hidDevice) : base(root, hidDevice) { }

        public override string GetName()
        {
            return "Corsair Bootloader";
        }
    }
}