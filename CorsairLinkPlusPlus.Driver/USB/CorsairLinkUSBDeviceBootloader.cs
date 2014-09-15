using HidLibrary;

namespace CorsairLinkPlusPlus.Driver.USB
{
    public class CorsairLinkUSBDeviceBootloader : CorsairLinkUSBDeviceOld
    {
        internal CorsairLinkUSBDeviceBootloader(HidDevice hidDevice) : base(hidDevice) { }

        public override string GetName()
        {
            return "Corsair Bootloader";
        }
    }
}