
namespace CorsairLinkPlusPlus.Driver.Node.Internal
{
    public class LinkDevicePSUAX1200i : LinkDevicePSU
    {
        internal LinkDevicePSUAX1200i(USB.BaseUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }

        internal override int GetPCIeRailCount()
        {
            return 8;
        }
    }
}
