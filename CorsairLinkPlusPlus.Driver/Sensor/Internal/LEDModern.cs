using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Driver.Controller;
using CorsairLinkPlusPlus.Driver.Controller.LED;
using CorsairLinkPlusPlus.Driver.Node;
using CorsairLinkPlusPlus.Driver.Registry;
using CorsairLinkPlusPlus.Driver.USB;
using CorsairLinkPlusPlus.Driver.Utility;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.Sensor.Internal
{
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

            ((ControllerBase)GetController()).Refresh(this);
        }

        internal override byte[] GetRGBInternal()
        {
            DisabledCheck();

            RootDevice.usbGlobalMutex.WaitOne();
            modernDevice.SetCurrentLED(id);
            byte[] rgbInt = modernDevice.ReadRegister(0x07, 3);
            RootDevice.usbGlobalMutex.ReleaseMutex();

            return rgbInt;
        }

        internal override void SetRGB(byte[] rgb)
        {
            DisabledCheck();

            byte[] reg = new byte[12];
            Buffer.BlockCopy(rgb, 0, reg, 0, rgb.Length);

            RootDevice.usbGlobalMutex.WaitOne();
            modernDevice.SetCurrentLED(id);
            modernDevice.WriteRegister(0x0B, reg);
            RootDevice.usbGlobalMutex.ReleaseMutex();

            Refresh(true);
        }

        public void SetTemperatureSensor(IThermistor thermistor)
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

            RootDevice.usbGlobalMutex.WaitOne();
            modernDevice.SetCurrentLED(id);
            modernDevice.WriteRegister(0x08, BitConverter.GetBytes((short)(temperature * 256.0)));
            RootDevice.usbGlobalMutex.ReleaseMutex();
        }

        public void SetController(IController controller)
        {
            DisabledCheck();

            if (!(controller is LEDController))
                throw new ArgumentException();

            LEDController ledController = (LEDController)controller;

            byte ledControllerID = ledController.GetLEDModernControllerID();

            if ((ledControllerID & 0x3F /* 00111111 */) != 0)
                throw new ArgumentException();

            if (ledController is TemperatureDependantControllerBase)
            {
                byte ledData = GetLEDData();
                ledData &= 0x3F; //00111111
                ledData |= (byte)ledControllerID;
                SetLEDData(ledData);
            }
            else
            {
                SetLEDData((byte)(ledControllerID | 0x0B));
            }

            SaveControllerData(controller);
        }

        public IController GetController()
        {
            DisabledCheck();

            if (controller == null)
                controller = LEDControllerRegistry.Get(this, (byte)(GetLEDData() & 0xC0 /* 11000000 */));

            return controller;
        }

        public void SaveControllerData(IController controller)
        {
            DisabledCheck();

            if (!(controller is LEDController))
                throw new ArgumentException();

            ((ControllerBase)GetController()).Apply(this);
        }

        private void SetLEDData(byte ledData)
        {
            DisabledCheck();

            RootDevice.usbGlobalMutex.WaitOne();
            modernDevice.SetCurrentLED(id);
            modernDevice.WriteSingleByteRegister(0x06, ledData);
            cachedLEDData = null;
            RootDevice.usbGlobalMutex.ReleaseMutex();
        }

        private byte GetLEDData()
        {
            DisabledCheck();

            if (cachedLEDData == null)
            {
                RootDevice.usbGlobalMutex.WaitOne();
                modernDevice.SetCurrentLED(id);
                cachedLEDData = modernDevice.ReadSingleByteRegister(0x06);
                RootDevice.usbGlobalMutex.ReleaseMutex();
            }
            return (byte)cachedLEDData;
        }

        public IThermistor GetTemperatureSensor()
        {
            DisabledCheck();

            int idx = (GetLEDData() & 0x07 /* 00000111 */);
            if (idx == 7)
                return null;
            return modernDevice.GetThermistor(idx);
        }

        internal override byte[] GetFixedRGBCycleColors()
        {
            DisabledCheck();

            RootDevice.usbGlobalMutex.WaitOne();
            modernDevice.SetCurrentLED(id);
            byte[] colors = modernDevice.ReadRegister(0x0B, 12);
            RootDevice.usbGlobalMutex.ReleaseMutex();

            return colors;
        }

        internal override void SetFixedRGBCycleColors(byte[] colors)
        {
            DisabledCheck();

            if(colors.Length > 12)
                throw new ArgumentException();

            if (colors.Length < 12)
            {
                byte[] newColors = new byte[12];
                Buffer.BlockCopy(colors, 0, newColors, 0, colors.Length);
                colors = newColors;
            }

            RootDevice.usbGlobalMutex.WaitOne();
            modernDevice.SetCurrentLED(id);
            modernDevice.WriteRegister(0x0B, colors);
            RootDevice.usbGlobalMutex.ReleaseMutex();
        }

        internal override ControlCurve<double, Color> GetControlCurve()
        {
            byte[] tempTable, colorTable;

            RootDevice.usbGlobalMutex.WaitOne();
            modernDevice.SetCurrentLED(id);
            tempTable = modernDevice.ReadRegister(0x09, 6);
            colorTable = modernDevice.ReadRegister(0x0A, 9);
            RootDevice.usbGlobalMutex.ReleaseMutex();

            List<CurvePoint<double, Color>> points = new List<CurvePoint<double, Color>>();

            for (int i = 0; i < 3; i++)
            {
                int j = i * 3;
                points.Add(new CurvePoint<double, Color>(BitConverter.ToInt16(tempTable, i * 2) / 256.0, new Color(colorTable[j], colorTable[j + 1], colorTable[j + 2])));
            }

            return new ControlCurve<double, Color>(points);
        }

        internal override void SetControlCurve(ControlCurve<double, Color> curve)
        {
            List<CurvePoint<double, Color>> points = curve.GetPoints();
            if (points.Count != 3)
                throw new ArgumentException();

            byte[] tempTable = new byte[6];
            byte[] colorTable = new byte[9];

            for(int i = 0; i < 3; i++)
            {
                CurvePoint<double, Color> point = points[i];
                Buffer.BlockCopy(BitConverter.GetBytes((short)(point.x * 256)), 0, tempTable, i, 2);
                Buffer.BlockCopy(point.y.ToArray(), 0, colorTable, i, 2);
            }

            RootDevice.usbGlobalMutex.WaitOne();
            modernDevice.SetCurrentLED(id);
            modernDevice.WriteRegister(0x09, tempTable);
            modernDevice.WriteRegister(0x0A, colorTable);
            RootDevice.usbGlobalMutex.ReleaseMutex();
        }
    }
}
