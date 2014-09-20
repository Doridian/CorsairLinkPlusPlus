using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Common.Registry;
using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Controller;
using CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node;
using CorsairLinkPlusPlus.Driver.CorsairLink.Registry;
using CorsairLinkPlusPlus.Driver.CorsairLink.USB;
using CorsairLinkPlusPlus.Driver.CorsairLink.Utility;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor.Internal
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
                lock (CorsairRootDevice.usbGlobalMutex)
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

            lock (CorsairRootDevice.usbGlobalMutex)
            {
                modernDevice.SetCurrentFan(id, true);
                modernDevice.WriteSingleByteRegister(0x12, fanData, true);
                cachedFanData = null;
            }
        }

        public override void Refresh(bool volatileOnly)
        {
            base.Refresh(volatileOnly);
            if (!volatileOnly)
                cachedFanData = null;
            ((ControllerBase)GetController()).Refresh(this);
        }

        protected override double GetValueInternal()
        {
            DisabledCheck();

            return modernDevice.GetCoolerRPM(id);
        }

        private bool FanDataBitSet(int bit)
        {
            bit = 1 << bit;
            return (GetFanData() & bit) == bit;
        }

        protected override bool IsPresentInternal()
        {
            return FanDataBitSet(7);
        }

        internal override bool IsPWMInternal()
        {
            return FanDataBitSet(0);
        }

        public void SetTemperatureSensor(IThermistor thermistor)
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

            lock (CorsairRootDevice.usbGlobalMutex)
            {
                modernDevice.SetCurrentFan(id, true);
                modernDevice.WriteRegister(0x15, BitConverter.GetBytes((short)(temperature * 256.0)), true);
            }
        }

        public void SetController(IController controller)
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

        public void SaveControllerData(IController controller)
        {
            DisabledCheck();

            if (!(controller is FanController))
                throw new ArgumentException();
            ((ControllerBase)GetController()).Apply(this);
        }

        public IController GetController()
        {
            DisabledCheck();

            if (controller == null)
            {
                controller = FanControllerRegistry.Get(this, (byte)(GetFanData() & 0x0E /*00001110*/));
            }
            return controller;
        }

        internal override void SetFixedRPM(double fixedRPM)
        {
            DisabledCheck();

            lock (CorsairRootDevice.usbGlobalMutex)
            {
                modernDevice.SetCurrentFan(id, true);
                modernDevice.WriteRegister(0x14, BitConverter.GetBytes((short)fixedRPM), true);
            }
        }

        internal override void SetFixedPercent(double percent)
        {
            DisabledCheck();

            byte percentB = (byte)(percent * 2.55);

            lock (CorsairRootDevice.usbGlobalMutex)
            {
                modernDevice.SetCurrentFan(id, true);
                modernDevice.WriteSingleByteRegister(0x13, percentB, true);
            }
        }

        internal override double GetFixedPercent()
        {
            DisabledCheck();

            byte percentB;

            lock (CorsairRootDevice.usbGlobalMutex)
            {
                modernDevice.SetCurrentFan(id);
                percentB = modernDevice.ReadSingleByteRegister(0x13);
            }

            return (short)(percentB / 2.55);
        }

        internal override double GetFixedRPM()
        {
            DisabledCheck();

            byte[] rpmB;

            lock (CorsairRootDevice.usbGlobalMutex)
            {
                modernDevice.SetCurrentFan(id);
                rpmB = modernDevice.ReadRegister(0x14, 2);
            }

            return BitConverter.ToInt16(rpmB, 0);
        }

        internal override ControlCurve<double, double> GetControlCurve()
        {
            DisabledCheck();

            byte[] tempTable, rpmTable;

            lock (CorsairRootDevice.usbGlobalMutex)
            {
                modernDevice.SetCurrentFan(id);
                tempTable = modernDevice.ReadRegister(0x1A, 10);
                rpmTable = modernDevice.ReadRegister(0x19, 10);
            }

            List<CurvePoint<double, double>> points = new List<CurvePoint<double, double>>();

            for (int i = 0; i < 10; i += 2)
                points.Add(new CurvePoint<double, double>(BitConverter.ToInt16(tempTable, i) / 256, BitConverter.ToInt16(rpmTable, i)));

            return new ControlCurve<double, double>(points);
        }

        internal override void SetControlCurve(ControlCurve<double, double> curve)
        {
            DisabledCheck();

            List<CurvePoint<double, double>> points = curve.GetPoints();
            if (points.Count != 5)
                throw new ArgumentException();

            byte[] tempTable = new byte[10];
            byte[] rpmTable = new byte[10];

            for (int i = 0; i < 10; i += 2)
            {
                CurvePoint<double, double> point = points[i / 2];
                Buffer.BlockCopy(BitConverter.GetBytes((short)(point.x * 256)), 0, tempTable, i, 2);
                Buffer.BlockCopy(BitConverter.GetBytes((short)point.y), 0, rpmTable, i, 2);
            }

            lock (CorsairRootDevice.usbGlobalMutex)
            {
                modernDevice.SetCurrentFan(id, true);
                modernDevice.WriteRegister(0x1A, tempTable, true);
                modernDevice.WriteRegister(0x19, rpmTable, true);
            }
        }

        public IThermistor GetTemperatureSensor()
        {
            DisabledCheck();

            int idx = (GetFanData() & 0x70 /* 01110000 */) >> 4;
            if (idx == 7)
                return null;
            return modernDevice.GetThermistor(idx);
        }

        internal override void SetMinimalRPM(double rpm)
        {
            DisabledCheck();

            lock (CorsairRootDevice.usbGlobalMutex)
            {
                modernDevice.SetCurrentFan(id, true);
                modernDevice.WriteRegister(0x18, BitConverter.GetBytes((short)rpm));
            }
        }

        internal override double GetMinimalRPM()
        {
            DisabledCheck();

            short minRPM;

            lock (CorsairRootDevice.usbGlobalMutex)
            {
                modernDevice.SetCurrentFan(id);
                minRPM = BitConverter.ToInt16(modernDevice.ReadRegister(0x18, 2), 0);
            }

            return minRPM;
        }

        public IEnumerable<RegisteredController> GetValidControllers()
        {
            return new RegisteredController[] {
                ControllerRegistry.Get("CorsairLink.FanDefaultController"),
                ControllerRegistry.Get("CorsairLink.FanBalancedModeController"),
                ControllerRegistry.Get("CorsairLink.FanQuiteModeController"),
                ControllerRegistry.Get("CorsairLink.FanPerformanceModeController"),
                ControllerRegistry.Get("CorsairLink.FanCustomCurveController"),
                ControllerRegistry.Get("CorsairLink.FanFixedRPMController"),
                ControllerRegistry.Get("CorsairLink.FanFixedPercentController"),
            };
        }
    }
}
