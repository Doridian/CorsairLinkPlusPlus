using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.USB;
using System;

namespace CorsairLinkPlusPlus.Driver.Node
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

        public override string GetName()
        {
            switch (GetDeviceID())
            {
                case 0x37:
                    return "Corsair H80";
                case 0x38:
                    return "Corsair Cooling Node";
                case 0x39:
                    return "Corsair Lighting Node";
                case 0x3A:
                    return "Corsair H100";
                case 0x3B:
                    return "Corsair H80i";
                case 0x3C:
                    return "Corsair H100i";
                case 0x3D:
                    return "Corsair Commander Mini";
                case 0x3E:
                    return "Corsair H110i";
                case 0x3F:
                    return "Corsair Hub";
                case 0x40:
                    return "Corsair H100iGT";
                case 0x41:
                    return "Corsair H110iGT";
            }
            return "Unknown Modern Device (0x" + string.Format("{0:x2}", GetDeviceID()) + ")";
        }

        class CorsairFanModern : CorsairFan
        {
            private readonly CorsairLinkDeviceModern modernDevice;
            protected byte? cachedFanData = null;

            internal CorsairFanModern(CorsairLinkDeviceModern device, int id)
                : base(device, id)
            {
                modernDevice = device;
            }

            private byte GetFanData()
            {
                if (cachedFanData == null)
                {
                    lock (modernDevice.usbDevice.usbLock)
                    {
                        modernDevice.SetCurrentFan(id);
                        cachedFanData = modernDevice.ReadSingleByteRegister(0x12);
                    }
                }

                return (byte)cachedFanData;
            }

            public override void Refresh()
            {
                base.Refresh();
                cachedFanData = null;
            }

            private bool FanDataBitSet(int bit)
            {
                bit = 1 << bit;
                return (GetFanData() & bit) == bit;
            }

            internal override bool IsPresentInternal()
            {
                return FanDataBitSet(7);
            }

            internal override bool IsPWMInternal()
            {
                return FanDataBitSet(0);
            }
        }

        public override CorsairCooler GetCooler(int id)
        {
            if (id < 0 || id >= GetCoolerCount())
                return null;
            switch (GetCoolerType(id))
            {
                case "Fan":
                    return new CorsairFanModern(this, id);
                case "Pump":
                    return new CorsairPump(this, id);
            }
            return null;
        }

        public override string GetCoolerType(int id)
        {
            int devID = GetDeviceID();
            if (devID == 0x3B || devID == 0x3C || devID == 0x3E || devID == 0x40 || devID == 0x41)
            {
                if (id == GetCoolerCount() - 1)
                    return "Pump";
            }
            return base.GetCoolerType(id);
        }

        public override int GetCoolerCount()
        {
            return ReadSingleByteRegister(0x11);
        }

        internal void SetCurrentFan(int id)
        {
            WriteSingleByteRegister(0x10, (byte)id);
        }

        internal override double GetCoolerRPM(int id)
        {
            byte[] ret;
            lock (usbDevice.usbLock)
            {
                SetCurrentFan(id);
                ret = ReadRegister(0x16, 2);
            }
            return BitConverter.ToInt16(ret, 0);
        }

        protected void SetCurrentTemp(int id)
        {
            WriteSingleByteRegister(0x0C, (byte)id);
        }

        internal override double GetTemperatureDegC(int id)
        {
            byte[] ret;
            lock (usbDevice.usbLock)
            {
                SetCurrentTemp(id);
                ret = ReadRegister(0x0E, 2);
            }
            return ((double)BitConverter.ToInt16(ret, 0)) / 256.0;
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
