using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Common.Registry;
using CorsairLinkPlusPlus.Common.Sensor;
using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Controller;
using CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan;
using CorsairLinkPlusPlus.Driver.CorsairLink.Node;
using CorsairLinkPlusPlus.Driver.CorsairLink.Utility;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Sensor.Internal
{
    class FanPSU : Fan, IControllableSensor
    {
        private ControllerBase m_controller = null;

        internal FanPSU(LinkDevicePSU device, int id)
            : base(device, id)
        {

        }

        protected override double GetValueInternal()
        {
            DisabledCheck();

            return BitCodec.ToFloat(device.ReadRegister(0x90, 2), 0);
        }

        internal override void SetFixedPercent(double percent)
        {
            DisabledCheck();

            if (percent < 0 || percent > 100)
                throw new ArgumentException();
            device.WriteSingleByteRegister(0x3B, (byte)percent, true);
        }

        internal override double GetFixedPercent()
        {
            DisabledCheck();

            return device.ReadSingleByteRegister(0x3B);
        }

        public IController Controller
        {
            get
            {
                DisabledCheck();

                if (m_controller == null)
                {
                    switch (device.ReadSingleByteRegister(0xF0))
                    {
                        case 0:
                            m_controller = new FanDefaultController();
                            break;
                        case 1:
                            FanFixedPercentController newController = new FanFixedPercentController();
                            newController.AssignFrom(this);
                            m_controller = newController;
                            break;
                    }
                }

                return m_controller;
            }

            set
            {
                DisabledCheck();

                if (value is FanDefaultController)
                    device.WriteSingleByteRegister(0xF0, 0, true);
                else if (value is FanFixedPercentController)
                    device.WriteSingleByteRegister(0xF0, 1, true);
                else
                    throw new ArgumentException();

                m_controller = (ControllerBase)value;

                SaveControllerData(value);
            }
        }

        public void SaveControllerData(IController controller)
        {
            DisabledCheck();

            ((ControllerBase)controller).Apply(this);
        }

        internal override double GetFixedRPM()
        {
            throw new NotImplementedException();
        }

        internal override void SetFixedRPM(double rpm)
        {
            throw new NotImplementedException();
        }

        internal override ControlCurve<double, double> GetControlCurve()
        {
            throw new NotImplementedException();
        }

        internal override void SetControlCurve(ControlCurve<double, double> curve)
        {
            throw new NotImplementedException();
        }

        internal override void SetMinimalRPM(double rpm)
        {
            throw new NotImplementedException();
        }

        internal override double GetMinimalRPM()
        {
            DisabledCheck();

            return 0;
        }

        public IEnumerable<RegisteredController> GetValidControllers()
        {
            return new RegisteredController[] {
                ControllerRegistry.Get("CorsairLink.FanDefaultController"),
                ControllerRegistry.Get("CorsairLink.FanFixedPercentController"),
            };
        }

        public IEnumerable<string> ValidControllerNames
        {
            get
            {
                return new string[] {
                    "CorsairLink.FanDefaultController",
                    "CorsairLink.FanFixedPercentController",
                };
            }
        }
    }
}
