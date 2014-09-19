using CorsairLinkPlusPlus.Common.Controller;
using CorsairLinkPlusPlus.Common.Utility;
using CorsairLinkPlusPlus.Driver.CorsairLink.Sensor;
using CorsairLinkPlusPlus.Driver.CorsairLink.Utility;
using System;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Controller.Fan
{
    public abstract class FanCurveController : TemperatureDependantControllerBase, FanController, ICurveNumberController
    {
        public FanCurveController()
        {
            LoadDefaultCurve();
        }

        public FanCurveController(Thermistor thermistor) : base(thermistor)
        {
            LoadDefaultCurve();
        }

        public void LoadDefaultCurve()
        {
            curve = GetDefaultPoints().Copy();
        }

        public abstract ControlCurve<double, double> GetDefaultPoints();

        protected ControlCurve<double, double> curve;

        public ControlCurve<double, double> GetCurve()
        {
            return curve.Copy();
        }

        public virtual void SetCurve(ControlCurve<double, double> curve)
        {
            throw new InvalidOperationException();
        }

        public virtual void AssignFrom(Sensor.Fan fan)
        {
            SetThermistor(((TemperatureControllableSensor)fan).GetTemperatureSensor());
        }

        public abstract byte GetFanModernControllerID();
    }
}
