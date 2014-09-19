using HidLibrary;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.USB
{
    public class DeviceCommanderA : DeviceOld
    {
        internal DeviceCommanderA(CorsairRootDevice root, HidDevice hidDevice) : base(root, hidDevice) { }

        public override string GetName()
        {
            return "Corsair Commander (A)";
        }
    }
}