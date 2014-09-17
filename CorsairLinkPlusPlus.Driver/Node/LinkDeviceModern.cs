using CorsairLinkPlusPlus.Driver.Controller;
using CorsairLinkPlusPlus.Driver.Controller.Fan;
using CorsairLinkPlusPlus.Driver.Controller.LED;
using CorsairLinkPlusPlus.Driver.Registry;
using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.USB;
using System;
using System.Collections.Generic;
using CorsairLinkPlusPlus.Driver.Utility;

namespace CorsairLinkPlusPlus.Driver.Node
{
    public class LinkDeviceModern : BaseLinkDevice
    {
        internal LinkDeviceModern(USB.BaseUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }

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

        class PumpModern : Pump
        {
            private readonly LinkDeviceModern modernDevice;

            internal PumpModern(LinkDeviceModern device, int id)
                : base(device, id)
            {
                modernDevice = device;
            }

            internal override double GetValueInternal()
            {
                DisabledCheck();

                return modernDevice.GetCoolerRPM(id);
            }

            internal override bool IsPresentInternal()
            {
                DisabledCheck();

                return true;
            }
        }

        class FanModern : Fan, TemperatureControllableSensor
        {
            private readonly LinkDeviceModern modernDevice;
            protected byte? cachedFanData = null;
            private FanController controller = null;

            internal FanModern(LinkDeviceModern device, int id)
                : base(device, id)
            {
                modernDevice = device;
            }

            private byte GetFanData()
            {
                DisabledCheck();

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
            private void SetFanData(byte fanData)
            {
                DisabledCheck();

                lock (modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentFan(id);
                    modernDevice.WriteSingleByteRegister(0x12, fanData);
                    cachedFanData = null;
                }
            }

            public override void Refresh(bool volatileOnly)
            {
                base.Refresh(volatileOnly);
                if (!volatileOnly)
                    cachedFanData = null;
                GetController().Refresh(this);
            }

            internal override double GetValueInternal()
            {
                DisabledCheck();

                return modernDevice.GetCoolerRPM(id);
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

            public void SetTemperatureSensor(Thermistor thermistor)
            {
                DisabledCheck();

                int idx = Core.GetRelativeThermistorByte(this, thermistor);
                byte fanData = GetFanData();
                fanData &= 0x8F; //10001111
                fanData |= (byte)(idx << 4);
                SetFanData(fanData);
            }

            public void SetTemperature(double temperature)
            {
                DisabledCheck();

                lock (modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentFan(id);
                    modernDevice.WriteRegister(0x15, BitConverter.GetBytes((short)(temperature * 256.0)));
                }
            }

            public void SetController(ControllerBase controller)
            {
                DisabledCheck();

                if (!(controller is FanController))
                    throw new ArgumentException();

                FanController fanController = (FanController)controller;

                byte fanControllerID = fanController.GetFanModernControllerID();
                if ((fanControllerID & 0xF1) != 0)
                    throw new ArgumentException();

                byte fanData = GetFanData();
                fanData &= 0xF1; //11110001
                fanData |= (byte)fanControllerID;
                SetFanData(fanData);

                SaveControllerData(controller);
            }

            public void SaveControllerData(ControllerBase controller)
            {
                DisabledCheck();

                if (!(controller is FanController))
                    throw new ArgumentException();
                controller.Apply(this);
            }

            public ControllerBase GetController()
            {
                DisabledCheck();

                if(controller == null)
                {
                    controller = FanControllerRegistry.GetFanController(this, (byte)(GetFanData() & 0x0E /*00001110*/)); 
                }
                return (ControllerBase)controller;
            }

            public override void SetFixedRPM(int fixedRPM)
            {
                DisabledCheck();

                lock (modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentFan(id);
                    modernDevice.WriteRegister(0x14, BitConverter.GetBytes((short)fixedRPM));
                }
            }

            public override void SetFixedPercent(int percent)
            {
                DisabledCheck();

                byte percentB = (byte)(percent * 2.55);
                lock (modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentFan(id);
                    modernDevice.WriteSingleByteRegister(0x13, percentB);
                }
            }

            public override int GetFixedPercent()
            {
                DisabledCheck();

                byte percentB;
                lock (modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentFan(id);
                    percentB = modernDevice.ReadSingleByteRegister(0x13);
                }
                return (short)(percentB / 2.55);
            }

            public override int GetFixedRPM()
            {
                DisabledCheck();

                byte[] rpmB;
                lock (modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentFan(id);
                    rpmB = modernDevice.ReadRegister(0x14, 2);
                }
                return BitConverter.ToInt16(rpmB, 0);
            }

            public override ControlCurve GetControlCurve()
            {
                DisabledCheck();

                byte[] tempTable, rpmTable;
                lock(modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentFan(id);
                    tempTable = modernDevice.ReadRegister(0x1A, 10);
                    rpmTable = modernDevice.ReadRegister(0x19, 10);
                }

                List<CurvePoint> points = new List<CurvePoint>();

                for (int i = 0; i < 10; i += 2)
                    points.Add(new CurvePoint(BitConverter.ToInt16(tempTable, i) / 256, BitConverter.ToInt16(rpmTable, i)));

                return new ControlCurve(points);
            }

            public override void SetControlCurve(ControlCurve curve)
            {
                DisabledCheck();

                List<CurvePoint> points = curve.GetPoints();
                if (points.Count != 5)
                    throw new ArgumentException();

                byte[] tempTable = new byte[10];
                byte[] rpmTable = new byte[10];

                for (int i = 0; i < 10; i += 2)
                {
                    CurvePoint point = points[i / 2];
                    Buffer.BlockCopy(BitConverter.GetBytes((short)(point.x * 256)), 0, tempTable, i, 2);
                    Buffer.BlockCopy(BitConverter.GetBytes((short)point.y), 0, rpmTable, i, 2);
                }

                lock (modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentFan(id);
                    modernDevice.WriteRegister(0x1A, tempTable);
                    modernDevice.WriteRegister(0x19, rpmTable);
                }
            }

            public Thermistor GetTemperatureSensor()
            {
                DisabledCheck();

                int idx = (GetFanData() & 0x70 /* 01110000 */) >> 4;
                if (idx == 7)
                    return null;
                return modernDevice.GetThermistor(idx);
            }

            public override void SetMinimalRPM(int rpm)
            {
                DisabledCheck();

                lock (modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentFan(id);
                    modernDevice.WriteRegister(0x18, BitConverter.GetBytes((short)rpm));
                }
            }

            public override int GetMinimalRPM()
            {
                DisabledCheck();

                lock(modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentFan(id);
                    return BitConverter.ToInt16(modernDevice.ReadRegister(0x18, 2), 0);
                }
            }
        }

        class ThermistorModern : Thermistor
        {
            private readonly LinkDeviceModern modernDevice;

            internal ThermistorModern(LinkDeviceModern device, int id)
                : base(device, id)
            {
                modernDevice = device;
            }

            internal override double GetValueInternal()
            {
                DisabledCheck();

                byte[] ret;
                lock (modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentTemp(id);
                    ret = modernDevice.ReadRegister(0x0E, 2);
                }
                return ((double)BitConverter.ToInt16(ret, 0)) / 256.0;
            }
        }

        class LEDModern : LED, TemperatureControllableSensor
        {
            private readonly LinkDeviceModern modernDevice;
            private byte? cachedLEDData;
            private LEDController controller;

            internal LEDModern(LinkDeviceModern device, int id)
                : base(device, id)
            {
                modernDevice = device;
            }

            public override void Refresh(bool volatileOnly)
            {
                base.Refresh(volatileOnly);
                if (!volatileOnly)
                    cachedLEDData = null;
                GetController().Refresh(this);
            }

            internal override byte[] GetRGBInternal()
            {
                DisabledCheck();

                lock (modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentLED(id);
                    return modernDevice.ReadRegister(0x07, 3);
                }
            }

            public override void SetRGB(byte[] rgb)
            {
                DisabledCheck();

                byte[] reg = new byte[12];
                Buffer.BlockCopy(rgb, 0, reg, 0, rgb.Length);
                lock (modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentLED(id);
                    modernDevice.WriteRegister(0x0B, reg);
                }
                Refresh(true);
            }

            public void SetTemperatureSensor(Thermistor thermistor)
            {
                DisabledCheck();

                int idx = Core.GetRelativeThermistorByte(this, thermistor);
                byte ledData = GetLEDData();
                ledData &= 0xF8; //11111000
                ledData |= (byte)idx;
                SetLEDData(ledData);
            }

            public void SetTemperature(double temperature)
            {
                DisabledCheck();

                lock (modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentLED(id);
                    modernDevice.WriteRegister(0x08, BitConverter.GetBytes((short)(temperature * 256.0)));
                }
            }

            public void SetController(ControllerBase controller)
            {
                DisabledCheck();

                if (!(controller is LEDController))
                    throw new ArgumentException();

                LEDController ledController = (LEDController)controller;

                byte ledControllerID = ledController.GetLEDModernControllerID();
                if ((ledControllerID & 0xF8 /* 11111000 */) != 0)
                    throw new ArgumentException();

                byte ledData = GetLEDData();
                ledData &= 0xF8; //11111000
                ledData |= (byte)ledControllerID;
                SetLEDData(ledData);

                SaveControllerData(controller);
            }

            public ControllerBase GetController()
            {
                DisabledCheck();

                if (controller == null)
                {
                    controller = LEDControllerRegistry.GetLEDController(this, (byte)(GetLEDData() & 0x07 /* 00000111 */));
                }
                return (ControllerBase)controller;
            }

            public void SaveControllerData(ControllerBase controller)
            {
                DisabledCheck();

                if (!(controller is LEDController))
                    throw new ArgumentException();
                controller.Apply(this);
            }

            private void SetLEDData(byte ledData)
            {
                DisabledCheck();

                lock (modernDevice.usbDevice.usbLock)
                {
                    cachedLEDData = null;
                    modernDevice.SetCurrentLED(id);
                    modernDevice.WriteSingleByteRegister(0x06, ledData);
                }
            }

            private byte GetLEDData()
            {
                DisabledCheck();

                if (cachedLEDData == null)
                {
                    lock(modernDevice.usbDevice.usbLock)
                    {
                        modernDevice.SetCurrentLED(id);
                        cachedLEDData = modernDevice.ReadSingleByteRegister(0x06);
                    }
                }
                return (byte)cachedLEDData;
            }

            public Thermistor GetTemperatureSensor()
            {
                DisabledCheck();

                int idx = (GetLEDData() & 0x07 /* 00000111 */);
                if (idx == 7)
                    return null;
                return modernDevice.GetThermistor(idx);
            }
        }

        private volatile Dictionary<int, Cooler> coolerSensors = null;
        private volatile Dictionary<int, Thermistor> tempSensors = null;
        private volatile Dictionary<int, LED> ledSensors = null;

        private volatile int temperatureCount = -1;
        private volatile int coolerCount = -1;
        private volatile int ledCount = -1;
        private volatile byte deviceID = 0xFF;

        internal Thermistor GetThermistor(int idx)
        {
            DisabledCheck();

            if (tempSensors == null)
                GetSubDevicesInternal();
            return tempSensors[idx];
        }

        internal override List<BaseDevice> GetSubDevicesInternal()
        {
            List<BaseDevice> ret = base.GetSubDevicesInternal();

            lock (subDeviceLock)
            {
                // COOLERS
                if (coolerSensors == null)
                {
                    coolerSensors = new Dictionary<int, Cooler>();
                    int imax = GetCoolerCount();
                    for (int i = 0; i < imax; i++)
                        coolerSensors.Add(i, GetCooler(i));
                }

                ret.AddRange(coolerSensors.Values);

                // THERMISTORS
                if (tempSensors == null)
                {
                    tempSensors = new Dictionary<int, Thermistor>();
                    int imax = GetTemperatureCount();
                    for (int i = 0; i < imax; i++)
                        tempSensors.Add(i, new ThermistorModern(this, i));
                }

                ret.AddRange(tempSensors.Values);

                // LEDs
                if (ledSensors == null)
                {
                    ledSensors = new Dictionary<int, LED>();
                    int imax = GetLEDCount();
                    for (int i = 0; i < imax; i++)
                        ledSensors.Add(i, new LEDModern(this, i));
                }

                ret.AddRange(ledSensors.Values);
            }

            return ret;
        }

        private Cooler GetCooler(int id)
        {
            DisabledCheck();

            if (id < 0 || id >= GetCoolerCount())
                return null;
            int devID = GetDeviceID();
            if (devID == 0x3B || devID == 0x3C || devID == 0x3E || devID == 0x40 || devID == 0x41)
                if (id == GetCoolerCount() - 1)
                    return new PumpModern(this, id);
            return new FanModern(this, id);
        }

        private void SetCurrentFan(int id)
        {
            WriteSingleByteRegister(0x10, (byte)id);
        }

        internal double GetCoolerRPM(int id)
        {
            DisabledCheck();

            byte[] ret;
            lock (usbDevice.usbLock)
            {
                SetCurrentFan(id);
                ret = ReadRegister(0x16, 2);
            }
            return BitConverter.ToInt16(ret, 0);
        }

        internal void SetCurrentTemp(int id)
        {
            DisabledCheck();

            WriteSingleByteRegister(0x0C, (byte)id);
        }

        internal void SetCurrentLED(int id)
        {
            DisabledCheck();

            WriteSingleByteRegister(0x04, (byte)id);
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            if (!volatileOnly)
            {
                lock (subDeviceLock)
                {
                    temperatureCount = -1;
                    coolerCount = -1;
                    ledCount = -1;
                    tempSensors = null;
                    ledSensors = null;
                    coolerSensors = null;
                    deviceID = 0xFF;
                }
            }
        }

        internal int GetCoolerCount()
        {
            DisabledCheck();

            if (coolerCount < 0)
                coolerCount = ReadSingleByteRegister(0x11);
            return coolerCount;
        }

        internal int GetTemperatureCount()
        {
            DisabledCheck();

            if (temperatureCount < 0)
                temperatureCount = ReadSingleByteRegister(0x0D);
            return temperatureCount;
        }

        internal int GetLEDCount()
        {
            DisabledCheck();

            if (ledCount < 0)
                ledCount = ReadSingleByteRegister(0x05);
            return ledCount;
        }

        public byte GetDeviceID()
        {
            DisabledCheck();

            if (deviceID == 0xFF)
                deviceID = ReadSingleByteRegister(0x00);
            return deviceID;
        }

        public int GetFirmwareVersion()
        {
            DisabledCheck();

            return BitConverter.ToInt16(ReadRegister(0x01, 2), 0);
        }
    }
}
