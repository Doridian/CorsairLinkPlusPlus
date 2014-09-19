
namespace CorsairLinkPlusPlus.Driver.CorsairLink.Node.Internal
{
    public class LinkDevicePSUHXiNoRail : LinkDevicePSUHX
    {
        internal LinkDevicePSUHXiNoRail(USB.BaseUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }

        internal override string[] GetSecondary12VRailNames()
        {
            return new string[0];
        }

        internal override int GetPCIeRailCount()
        {
            return 0;
        }
    }
}
