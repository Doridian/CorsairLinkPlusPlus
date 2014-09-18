using CorsairLinkPlusPlus.Driver.Sensor;
using System.Linq;
using CorsairLinkPlusPlus.Driver.Utility;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public abstract class FanCurveController : TemperatureDependantControllerBase, FanController
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
            curve = new ControlCurve<int>(GetDefaultPoints().ToList());
        }

        public abstract CurvePoint<int>[] GetDefaultPoints();

        protected ControlCurve<int> curve;

        public ControlCurve<int> GetCurve()
        {
            return curve.Copy();
        }

        public virtual void AssignFrom(Sensor.Fan fan)
        {
            SetThermistor(((TemperatureControllableSensor)fan).GetTemperatureSensor());
        }

        public abstract byte GetFanModernControllerID();
    }
}
