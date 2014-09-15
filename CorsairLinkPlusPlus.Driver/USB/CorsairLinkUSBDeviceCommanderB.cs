using HidLibrary;

namespace CorsairLinkPlusPlus.Driver.USB
{
    public class CorsairLinkUSBDeviceCommanderB : CorsairLinkUSBDeviceOld
    {
        internal CorsairLinkUSBDeviceCommanderB(HidDevice hidDevice) : base(hidDevice) { }

        public override string GetName()
        {
            return "Corsair Commander (B)";
        }
    }
}