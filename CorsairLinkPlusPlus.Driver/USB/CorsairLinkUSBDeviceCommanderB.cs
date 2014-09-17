using HidLibrary;

namespace CorsairLinkPlusPlus.Driver.USB
{
    public class CorsairLinkUSBDeviceCommanderB : CorsairLinkUSBDeviceOld
    {
        internal CorsairLinkUSBDeviceCommanderB(CorsairRootDevice root, HidDevice hidDevice) : base(root, hidDevice) { }

        public override string GetName()
        {
            return "Corsair Commander (B)";
        }
    }
}