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
            curve = new ControlCurve(GetDefaultPoints().ToList<CurvePoint>());
        }

        public abstract CurvePoint[] GetDefaultPoints();

        protected ControlCurve curve;

        public ControlCurve GetCurve()
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
