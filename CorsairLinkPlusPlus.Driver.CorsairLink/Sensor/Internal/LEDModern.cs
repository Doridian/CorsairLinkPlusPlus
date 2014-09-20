using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Common.Registry;
using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Controller;
using CorsairLinkPlusPlus.Driver.CorsairLink.Controller.LED;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node;
using CorsairLinkPlusPlus.Driver.CorsairLink.Registry;
using CorsairLinkPlusPlus.Driver.CorsairLink.USB;
using CorsairLinkPlusPlus.Driver.CorsairLink.Utility;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor.Internal
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

            byte[] rgbInt;

            lock (CorsairRootDevice.usbGlobalMutex)
            {
                modernDevice.SetCurrentLED(id);
                rgbInt = modernDevice.ReadRegister(0x07, 3);
            }

            return rgbInt;
        }

        internal override void SetRGB(byte[] rgb)
        {
            DisabledCheck();

            byte[] reg = new byte[12];
            Buffer.BlockCopy(rgb, 0, reg, 0, rgb.Length);

            lock (CorsairRootDevice.usbGlobalMutex)
            {
                modernDevice.SetCurrentLED(id, true);
                modernDevice.WriteRegister(0x0B, reg);
            }

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

            lock (CorsairRootDevice.usbGlobalMutex)
            {
                modernDevice.SetCurrentLED(id);
                modernDevice.WriteRegister(0x08, BitConverter.GetBytes((short)(temperature * 256.0)));
            }
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
                SetLEDData((byte)(ledControllerID | 0x0B));

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

            lock (CorsairRootDevice.usbGlobalMutex)
            {
                modernDevice.SetCurrentLED(id, true);
                modernDevice.WriteSingleByteRegister(0x06, ledData);
                cachedLEDData = null;
            }
        }

        private byte GetLEDData()
        {
            DisabledCheck();

            if (cachedLEDData == null)
            {
                lock (CorsairRootDevice.usbGlobalMutex)
                {
                    modernDevice.SetCurrentLED(id);
                    cachedLEDData = modernDevice.ReadSingleByteRegister(0x06);
                }
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

            byte[] colors;

            lock (CorsairRootDevice.usbGlobalMutex)
            {
                modernDevice.SetCurrentLED(id);
                colors = modernDevice.ReadRegister(0x0B, 12);
            }

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

            lock (CorsairRootDevice.usbGlobalMutex)
            {
                modernDevice.SetCurrentLED(id, true);
                modernDevice.WriteRegister(0x0B, colors, true);
            }
        }

        internal override ControlCurve<double, Color> GetControlCurve()
        {
            byte[] tempTable, colorTable;

            lock (CorsairRootDevice.usbGlobalMutex)
            {
                modernDevice.SetCurrentLED(id);
                tempTable = modernDevice.ReadRegister(0x09, 6);
                colorTable = modernDevice.ReadRegister(0x0A, 9);
            }

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

            lock (CorsairRootDevice.usbGlobalMutex)
            {
                modernDevice.SetCurrentLED(id, true);
                modernDevice.WriteRegister(0x09, tempTable, true);
                modernDevice.WriteRegister(0x0A, colorTable, true);
            }
        }

        public IEnumerable<RegisteredController> GetValidControllers()
        {
            return new RegisteredController[] {
                ControllerRegistry.Get("CorsairLink.LEDSingleColorController"),
                ControllerRegistry.Get("CorsairLink.LEDTwoColorController"),
                ControllerRegistry.Get("CorsairLink.LEDFourColorController"),
                ControllerRegistry.Get("CorsairLink.LEDTemperatureController"),
            };
        }

        public IEnumerable<string> ValidControllerNames
        {
            get
            {
                return new string[] {
                    "CorsairLink.LEDSingleColorController",
                    "CorsairLink.LEDTwoColorController",
                    "CorsairLink.LEDFourColorController",
                    "CorsairLink.LEDTemperatureController",
                };
            }
        }
    }
}
