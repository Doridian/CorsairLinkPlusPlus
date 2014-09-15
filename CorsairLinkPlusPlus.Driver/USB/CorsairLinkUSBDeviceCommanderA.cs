using HidLibrary;

namespace CorsairLinkPlusPlus.Driver.USB
{
    public class CorsairLinkUSBDeviceCommanderA : CorsairLinkUSBDeviceOld
    {
        internal CorsairLinkUSBDeviceCommanderA(HidDevice hidDevice) : base(hidDevice) { }

        public override string GetName()
        {
            return "Corsair Commander (A)";
        }
    }
}