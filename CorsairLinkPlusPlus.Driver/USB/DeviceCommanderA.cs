using HidLibrary;

namespace CorsairLinkPlusPlus.Driver.USB
{
    public class DeviceCommanderA : DeviceOld
    {
        internal DeviceCommanderA(RootDevice root, HidDevice hidDevice) : base(root, hidDevice) { }

        public override string GetName()
        {
            return "Corsair Commander (A)";
        }
    }
}