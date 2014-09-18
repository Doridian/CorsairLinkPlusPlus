using CorsairLinkPlusPlus.Driver.Controller;
using CorsairLinkPlusPlus.Driver.Controller.Fan;
using CorsairLinkPlusPlus.Driver.Node;
using CorsairLinkPlusPlus.Driver.Utility;
using System;

namespace CorsairLinkPlusPlus.Driver.Sensor.Internal
{
    class FanPSU : Fan, ControllableSensor
    {
        private ControllerBase controller = null;

        internal FanPSU(LinkDevicePSU device, int id)
            : base(device, id)
        {

        }

        protected override double GetValueInternal()
        {
            DisabledCheck();

            return BitCodec.ToFloat(device.ReadRegister(0x90, 2), 0);
        }

        internal override void SetFixedPercent(int percent)
        {
            DisabledCheck();

            if (percent < 0 || percent > 100)
                throw new ArgumentException();
            device.WriteSingleByteRegister(0x3B, (byte)percent);
        }

        internal override int GetFixedPercent()
        {
            DisabledCheck();

            return device.ReadSingleByteRegister(0x3B);
        }

        public void SetController(ControllerBase controller)
        {
            DisabledCheck();

            if (controller is FanDefaultController)
                device.WriteSingleByteRegister(0xF0, 0);
            else if (controller is FanFixedPercentController)
                device.WriteSingleByteRegister(0xF0, 1);

            SaveControllerData(controller);
        }

        public void SaveControllerData(ControllerBase controller)
        {
            DisabledCheck();

            controller.Apply(this);
        }

        public ControllerBase GetController()
        {
            DisabledCheck();

            if (controller == null)
                switch (device.ReadSingleByteRegister(0xF0))
                {
                    case 0:
                        controller = new FanDefaultController();
                        break;
                    case 1:
                        FanFixedPercentController newController = new FanFixedPercentController();
                        newController.AssignFrom(this);
                        controller = newController;
                        break;
                }

            return controller;
        }

        internal override int GetFixedRPM()
        {
            throw new NotImplementedException();
        }

        internal override void SetFixedRPM(int rpm)
        {
            throw new NotImplementedException();
        }

        internal override ControlCurve<int> GetControlCurve()
        {
            throw new NotImplementedException();
        }

        internal override void SetControlCurve(ControlCurve<int> curve)
        {
            throw new NotImplementedException();
        }

        internal override void SetMinimalRPM(int rpm)
        {
            throw new NotImplementedException();
        }

        internal override int GetMinimalRPM()
        {
            DisabledCheck();

            return 0;
        }
    }
}
