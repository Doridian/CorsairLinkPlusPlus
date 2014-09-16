using CorsairLinkPlusPlus.Driver.Controller;
using CorsairLinkPlusPlus.Driver.Controller.Fan;
using CorsairLinkPlusPlus.Driver.Registry;
using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.USB;
using CorsairLinkPlusPlus.Driver.Utility;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.Node
{
    public class CorsairLinkDeviceModern : CorsairLinkDevice
    {
        internal CorsairLinkDeviceModern(CorsairLinkUSBDevice usbDevice, byte channel) : base(usbDevice, channel) { }

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

        class CorsairPumpModern : CorsairPump
        {
            private readonly CorsairLinkDeviceModern modernDevice;

            internal CorsairPumpModern(CorsairLinkDeviceModern device, int id)
                : base(device, id)
            {
                modernDevice = device;
            }

            internal override double GetValueInternal()
            {
                return modernDevice.GetCoolerRPM(id);
            }

            internal override bool IsPresentInternal()
            {
                return true;
            }
        }

        class CorsairFanModern : CorsairFan, CorsairTemperatureControllableSensor
        {
            private readonly CorsairLinkDeviceModern modernDevice;
            protected byte? cachedFanData = null;
            private CorsairFanController controller = null;

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
            private void SetFanData(byte fanData)
            {
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

            public void SetTemperatureSensor(CorsairThermistor thermistor)
            {
                int idx = CorsairUtility.GetRelativeThermistorByte(this, thermistor);
                byte fanData = GetFanData();
                fanData &= 0x8F; //10001111
                fanData |= (byte)(idx << 4);
                SetFanData(fanData);
            }

            public void SetTemperature(double temperature)
            {
                lock (modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentFan(id);
                    modernDevice.WriteRegister(0x15, BitConverter.GetBytes((short)(temperature * 256.0)));
                }
            }

            public void SetController(CorsairControllerBase controller)
            {
                if (!(controller is CorsairFanController))
                    throw new ArgumentException();

                CorsairFanController fanController = (CorsairFanController)controller;

                byte fanControllerID = fanController.GetFanModernControllerID();
                if ((fanControllerID & 0xF1) != 0)
                    throw new ArgumentException();

                byte fanData = GetFanData();
                fanData &= 0xF1; //11110001
                fanData |= (byte)fanControllerID;
                SetFanData(fanData);

                SaveControllerData(controller);
            }

            public void SaveControllerData(CorsairControllerBase controller)
            {
                if (!(controller is CorsairFanController))
                    throw new ArgumentException();
                controller.Apply(this);
            }

            public CorsairControllerBase GetController()
            {
                if(controller == null)
                {
                    controller = CorsairFanControllerRegistry.GetFanController(this, (byte)(GetFanData() & 0x0E /*00001110*/)); 
                }
                return (CorsairControllerBase)controller;
            }

            public override void SetFixedRPM(int fixedRPM)
            {
                lock (modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentFan(id);
                    modernDevice.WriteRegister(0x14, BitConverter.GetBytes((short)fixedRPM));
                }
            }

            public override void SetFixedPercent(int percent)
            {
                byte percentB = (byte)(percent * 2.55);
                Console.Out.WriteLine(percentB);
                lock (modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentFan(id);
                    modernDevice.WriteSingleByteRegister(0x13, percentB);
                }
            }

            public override int GetFixedPercent()
            {
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
                byte[] tempTable, rpmTable;
                lock(modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentFan(id);
                    tempTable = modernDevice.ReadRegister(0x1A, 10);
                    rpmTable = modernDevice.ReadRegister(0x19, 10);
                }

                List<CurvePoint> points = new List<CurvePoint>();

                for (int i = 0; i < 10; i += 2)
                {
                    points.Add(new CurvePoint(BitConverter.ToInt16(tempTable, i) / 256, BitConverter.ToInt16(rpmTable, i)));
                }

                return new ControlCurve(points);
            }

            public override void SetControlCurve(ControlCurve curve)
            {
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

            public CorsairThermistor GetTemperatureSensor()
            {
                int idx = (GetFanData() & 0x70 /* 01110000 */) >> 4;
                if (idx == 7)
                    return null;
                return modernDevice.GetThermistor(idx);
            }
        }

        class CorsairThermistorModern : CorsairThermistor
        {
            private readonly CorsairLinkDeviceModern modernDevice;

            internal CorsairThermistorModern(CorsairLinkDeviceModern device, int id)
                : base(device, id)
            {
                modernDevice = device;
            }

            internal override double GetValueInternal()
            {
                byte[] ret;
                lock (modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentTemp(id);
                    ret = modernDevice.ReadRegister(0x0E, 2);
                }
                return ((double)BitConverter.ToInt16(ret, 0)) / 256.0;
            }
        }

        class CorsairLEDModern : CorsairLED, CorsairTemperatureControllableSensor
        {
            private readonly CorsairLinkDeviceModern modernDevice;

            internal CorsairLEDModern(CorsairLinkDeviceModern device, int id)
                : base(device, id)
            {
                modernDevice = device;
            }

            public override void Refresh(bool volatileOnly)
            {
                base.Refresh(volatileOnly);
                GetController().Refresh(this);
            }

            internal override byte[] GetRGBInternal()
            {
                lock (modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentLED(id);
                    return modernDevice.ReadRegister(0x07, 3);
                }
            }

            public override void SetRGB(byte[] rgb)
            {
                byte[] reg = new byte[12];
                Buffer.BlockCopy(rgb, 0, reg, 0, rgb.Length);
                lock (modernDevice.usbDevice.usbLock)
                {
                    modernDevice.SetCurrentLED(id);
                    modernDevice.WriteRegister(0x0B, reg);
                }
                Refresh(true);
            }

            public void SetTemperatureSensor(CorsairThermistor thermistor)
            {
                throw new NotImplementedException();
            }

            public void SetTemperature(double temperature)
            {
                throw new NotImplementedException();
            }

            public void SetController(CorsairControllerBase controller)
            {
                throw new NotImplementedException();
            }

            public CorsairControllerBase GetController()
            {
                throw new NotImplementedException();
            }

            public void SaveControllerData(CorsairControllerBase controller)
            {
                throw new NotImplementedException();
            }

            public CorsairThermistor GetTemperatureSensor()
            {
                throw new NotImplementedException();
            }
        }

        public override List<CorsairSensor> GetSensors()
        {
            List<CorsairSensor> ret = base.GetSensors();
            int imax = GetCoolerCount();
            for (int i = 0; i < imax; i++)
            {
                CorsairSensor sensor = GetCooler(i);
                if(sensor.IsPresent())
                    ret.Add(sensor);
            }
            imax = GetTemperatureCount();
            for (int i = 0; i < imax; i++)
            {
                CorsairSensor sensor = GetThermistor(i);
                if(sensor != null && sensor.IsPresent())
                    ret.Add(sensor);
            }
            imax = GetLEDCount();
            for (int i = 0; i < imax; i++)
            {
                CorsairSensor sensor = new CorsairLEDModern(this, i);
                if (sensor.IsPresent())
                    ret.Add(sensor);
            }
            return ret;
        }

        internal CorsairThermistor GetThermistor(int id)
        {
            CorsairThermistor sensor = new CorsairThermistorModern(this, id);
            if (sensor.IsPresent())
                return sensor;
            return null;
        }

        internal CorsairCooler GetCooler(int id)
        {
            if (id < 0 || id >= GetCoolerCount())
                return null;
            int devID = GetDeviceID();
            if (devID == 0x3B || devID == 0x3C || devID == 0x3E || devID == 0x40 || devID == 0x41)
            {
                if (id == GetCoolerCount() - 1)
                    return new CorsairPumpModern(this, id);
            }
            return new CorsairFanModern(this, id);
        }

        internal void SetCurrentFan(int id)
        {
            WriteSingleByteRegister(0x10, (byte)id);
        }

        internal double GetCoolerRPM(int id)
        {
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
            WriteSingleByteRegister(0x0C, (byte)id);
        }

        internal void SetCurrentLED(int id)
        {
            WriteSingleByteRegister(0x04, (byte)id);
        }

        public override void Refresh(bool volatileOnly)
        {
            if (!volatileOnly)
            {
                temperatureCount = -1;
                coolerCount = -1;
                ledCount = -1;
                deviceID = 0xFF;
            }
        }

        int temperatureCount = -1;
        int coolerCount = -1;
        int ledCount = -1;
        byte deviceID = 0xFF;

        internal int GetCoolerCount()
        {
            if (coolerCount < 0)
                coolerCount = ReadSingleByteRegister(0x11);
            return coolerCount;
        }

        internal int GetTemperatureCount()
        {
            if (temperatureCount < 0)
                temperatureCount = ReadSingleByteRegister(0x0D);
            return temperatureCount;
        }

        internal int GetLEDCount()
        {
            if (ledCount < 0)
                ledCount = ReadSingleByteRegister(0x05);
            return ledCount;
        }

        public byte GetDeviceID()
        {
            if (deviceID == 0xFF)
                deviceID = ReadSingleByteRegister(0x00);
            return deviceID;
        }

        public int GetFirmwareVersion()
        {
            return BitConverter.ToInt16(ReadRegister(0x01, 2), 0);
        }
    }
}
