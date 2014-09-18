using CorsairLinkPlusPlus.Driver.Controller;
using CorsairLinkPlusPlus.Driver.Controller.Fan;
using CorsairLinkPlusPlus.Driver.Node;
using CorsairLinkPlusPlus.Driver.Registry;
using CorsairLinkPlusPlus.Driver.Utility;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.Sensor.Internal
{
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

            if (controller == null)
            {
                controller = FanControllerRegistry.Get(this, (byte)(GetFanData() & 0x0E /*00001110*/));
            }
            return (ControllerBase)controller;
        }

        internal override void SetFixedRPM(int fixedRPM)
        {
            DisabledCheck();

            lock (modernDevice.usbDevice.usbLock)
            {
                modernDevice.SetCurrentFan(id);
                modernDevice.WriteRegister(0x14, BitConverter.GetBytes((short)fixedRPM));
            }
        }

        internal override void SetFixedPercent(int percent)
        {
            DisabledCheck();

            byte percentB = (byte)(percent * 2.55);
            lock (modernDevice.usbDevice.usbLock)
            {
                modernDevice.SetCurrentFan(id);
                modernDevice.WriteSingleByteRegister(0x13, percentB);
            }
        }

        internal override int GetFixedPercent()
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

        internal override int GetFixedRPM()
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

        internal override ControlCurve GetControlCurve()
        {
            DisabledCheck();

            byte[] tempTable, rpmTable;
            lock (modernDevice.usbDevice.usbLock)
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

        internal override void SetControlCurve(ControlCurve curve)
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

        internal override void SetMinimalRPM(int rpm)
        {
            DisabledCheck();

            lock (modernDevice.usbDevice.usbLock)
            {
                modernDevice.SetCurrentFan(id);
                modernDevice.WriteRegister(0x18, BitConverter.GetBytes((short)rpm));
            }
        }

        internal override int GetMinimalRPM()
        {
            DisabledCheck();

            lock (modernDevice.usbDevice.usbLock)
            {
                modernDevice.SetCurrentFan(id);
                return BitConverter.ToInt16(modernDevice.ReadRegister(0x18, 2), 0);
            }
        }
    }
}
