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

    public class FanCustomCurveController : FanCurveController
    {        
        public FanCustomCurveController()
        {
            
        }

        public FanCustomCurveController(Thermistor thermistor)
            : base(thermistor)
        {
            
        }

        public void SetCurve(ControlCurve curve)
        {
            this.curve = curve;
        }

        public override CurvePoint[] GetDefaultPoints()
        {
            return new CurvePoint[] {
                new CurvePoint(0, 0),
                new CurvePoint(0, 0),
                new CurvePoint(0, 0),
                new CurvePoint(0, 0),
                new CurvePoint(0, 0)
            };
        }

        public override void AssignFrom(Sensor.Fan fan)
        {
            base.AssignFrom(fan);
            SetCurve(fan.GetControlCurve());
        }

        internal override void Apply(Sensor.BaseSensorDevice sensor)
        {
            base.Apply(sensor);
            ((Sensor.Fan)sensor).SetControlCurve(GetCurve());
        }

        public override byte GetFanModernControllerID()
        {
            return 0x0E;
        }
    }

    public class FanQuiteModeController : FanCurveController {
        public FanQuiteModeController()
        {
            
        }

        public FanQuiteModeController(Thermistor thermistor)
            : base(thermistor)
        {
            
        }

        public override CurvePoint[] GetDefaultPoints()
        {
            return new CurvePoint[] {
                new CurvePoint(0, 0),
                new CurvePoint(0, 0),
                new CurvePoint(0, 0),
                new CurvePoint(0, 0),
                new CurvePoint(0, 0)
            };
        }

        public override byte GetFanModernControllerID()
        {
            return 0x08;
        }
    }

    public class FanBalancedModeController : FanCurveController
    { 
        public FanBalancedModeController()
        {
            
        }

        public FanBalancedModeController(Thermistor thermistor)
            : base(thermistor)
        {
            
        }

        public override CurvePoint[] GetDefaultPoints()
        {
            return new CurvePoint[] {
                new CurvePoint(0, 0),
                new CurvePoint(0, 0),
                new CurvePoint(0, 0),
                new CurvePoint(0, 0),
                new CurvePoint(0, 0)
            };
        }
        public override byte GetFanModernControllerID()
        {
            return 0x0A;
        }
    }

    public class FanPerformanceModeController : FanCurveController
    {
        public FanPerformanceModeController()
        {
            
        }

        public FanPerformanceModeController(Thermistor thermistor)
            : base(thermistor)
        {
            
        }

        public override CurvePoint[] GetDefaultPoints()
        {
            return new CurvePoint[] {
                new CurvePoint(0, 0),
                new CurvePoint(0, 0),
                new CurvePoint(0, 0),
                new CurvePoint(0, 0),
                new CurvePoint(0, 0)
            };
        }
        public override byte GetFanModernControllerID()
        {
            return 0x0C;
        }
    }

}
