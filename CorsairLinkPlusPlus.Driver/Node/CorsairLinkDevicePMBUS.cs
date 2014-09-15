using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.USB;
using System;

namespace CorsairLinkPlusPlus.Driver.Node
{
    public class CorsairLinkDevicePMBUS : CorsairLinkDevice
    {
        internal CorsairLinkDevicePMBUS(CorsairLinkUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }
        public override string GetName()
        {
            byte[] ret = ReadRegister(0x9A, 7);
            return System.Text.Encoding.UTF8.GetString(ret);
        }

        internal override double GetCoolerRPM(int id)
        {
            if (id != 0)
                return 0;
            return CorsairBitCodec.ToFloat(ReadRegister(0x90, 2), 0);
        }

        internal override double GetTemperatureDegC(int id)
        {
            if (id != 0)
                return 0;
            return CorsairBitCodec.ToFloat(ReadRegister(0x8E, 2), 0);
        }

        public override int GetCoolerCount()
        {
            return 1;
        }

        public override int GetTemperatureCount()
        {
            return 1;
        }
    }
}
