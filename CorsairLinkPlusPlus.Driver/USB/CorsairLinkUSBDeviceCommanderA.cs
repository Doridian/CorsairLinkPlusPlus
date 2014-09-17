using HidLibrary;

namespace CorsairLinkPlusPlus.Driver.USB
{
    public class CorsairLinkUSBDeviceCommanderA : CorsairLinkUSBDeviceOld
    {
        internal CorsairLinkUSBDeviceCommanderA(CorsairRootDevice root, HidDevice hidDevice) : base(root, hidDevice) { }

        public override string GetName()
        {
            return "Corsair Commander (A)";
        }
    }
}