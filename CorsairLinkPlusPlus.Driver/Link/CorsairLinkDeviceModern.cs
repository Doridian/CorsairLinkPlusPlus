using CorsairLinkPlusPlus.Driver.USB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorsairLinkPlusPlus.Driver.Link
{
    public class CorsairLinkDeviceModern : CorsairLinkDevice
    {
        internal CorsairLinkDeviceModern(CorsairLinkUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }

        public byte GetDeviceID()
        {
            return ReadSingleByteRegister(0x00);
        }

        public int GetFirmwareVersion()
        {
            return BitConverter.ToInt16(ReadRegister(0x01, 2), 0);
        }

        public override string GetDeviceName()
        {
            switch (GetDeviceID())
            {
                case 0x37:
                    return "H80";
                case 0x38:
                    return "Cooling Node";
                case 0x39:
                    return "Lighting Node";
                case 0x3A:
                    return "H100";
                case 0x3B:
                    return "H80i";
                case 0x3C:
                    return "H100i";
                case 0x3D:
                    return "Commander Mini";
            }
            return "Unknown Modern Device (0x" + string.Format("{0:x2}", GetDeviceID()) + ")";
        }
        public override int GetFanCount()
        {
            return ReadSingleByteRegister(0x11);
        }

        public override int GetTemperatureCount()
        {
            return ReadSingleByteRegister(0x0D);
        }

        public override int GetLEDCount()
        {
            return ReadSingleByteRegister(0x05);
        }
    }
}
