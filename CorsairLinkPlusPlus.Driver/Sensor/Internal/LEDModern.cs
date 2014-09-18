using CorsairLinkPlusPlus.Driver.Controller;
using CorsairLinkPlusPlus.Driver.Controller.LED;
using CorsairLinkPlusPlus.Driver.Node;
using CorsairLinkPlusPlus.Driver.Registry;
using CorsairLinkPlusPlus.Driver.USB;
using System;

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
            GetController().Refresh(this);
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

            RootDevice.usbGlobalMutex.WaitOne();
            modernDevice.SetCurrentLED(id);
            modernDevice.WriteRegister(0x08, BitConverter.GetBytes((short)(temperature * 256.0)));
            RootDevice.usbGlobalMutex.ReleaseMutex();
        }

        public void SetController(ControllerBase controller)
        {
            DisabledCheck();

            if (!(controller is LEDController))
                throw new ArgumentException();

            LEDController ledController = (LEDController)controller;

            byte ledControllerID = ledController.GetLEDModernControllerID();
            if ((ledControllerID & 0x3F /* 00111111 */) != 0)
                throw new ArgumentException();

            byte ledData = GetLEDData();
            ledData &= 0x3F; //00111111
            ledData |= (byte)ledControllerID;
            SetLEDData(ledData);

            SaveControllerData(controller);
        }

        public ControllerBase GetController()
        {
            DisabledCheck();

            if (controller == null)
                controller = LEDControllerRegistry.Get(this, (byte)(GetLEDData() & 0xC0 /* 11000000 */));

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

        public Thermistor GetTemperatureSensor()
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
            if(colors.Length > 12)
                throw new ArgumentException();

            if (colors.Length < 12)
            {
                byte[] newColors = new byte[12];
                Buffer.BlockCopy(colors, 0, newColors, 0, colors.Length);
                colors = newColors;
            }

            DisabledCheck();

            RootDevice.usbGlobalMutex.WaitOne();
            modernDevice.SetCurrentLED(id);
            modernDevice.WriteRegister(0x0B, colors);
            RootDevice.usbGlobalMutex.ReleaseMutex();
        }
    }
}
