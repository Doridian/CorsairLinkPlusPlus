using CorsairLinkPlusPlus.Driver.USB;

namespace CorsairLinkPlusPlus.Driver.Link
{
    public class CorsairLinkDevicePMBUS : CorsairLinkDevice
    {
        internal CorsairLinkDevicePMBUS(CorsairLinkUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }
        public override string GetDeviceName()
        {
            byte[] ret = ReadRegister(0x9A, 7);
            return System.Text.Encoding.UTF8.GetString(ret);
        }

        public override int GetFanCount()
        {
            return 1;
        }
    }
}
