using CorsairLinkPlusPlus.Driver.Sensor;
using CorsairLinkPlusPlus.Driver.Utility;
using System.Linq;

namespace CorsairLinkPlusPlus.Driver.Controller.Fan
{
    public abstract class CorsairFanCurveController : CorsairTemperatureDependantControllerBase, CorsairFanController
    {
        public CorsairFanCurveController()
        {
            LoadDefaultCurve();
        }

        public CorsairFanCurveController(CorsairThermistor thermistor) : base(thermistor)
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

        public virtual void AssignFrom(CorsairFan fan)
        {
            SetThermistor(((CorsairTemperatureControllableSensor)fan).GetTemperatureSensor());
        }

        public abstract byte GetFanModernControllerID();
    }

    public class CorsairFanCustomCurveController : CorsairFanCurveController
    {        
        public CorsairFanCustomCurveController()
        {
            
        }

        public CorsairFanCustomCurveController(CorsairThermistor thermistor)
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

        public override void AssignFrom(CorsairFan fan)
        {
            base.AssignFrom(fan);
            SetCurve(fan.GetControlCurve());
        }

        internal override void Apply(CorsairSensor sensor)
        {
            base.Apply(sensor);
            ((CorsairFan)sensor).SetControlCurve(GetCurve());
        }

        public override byte GetFanModernControllerID()
        {
            return 0x0E;
        }
    }

    public class CorsairFanQuiteModeController : CorsairFanCurveController {
        public CorsairFanQuiteModeController()
        {
            
        }

        public CorsairFanQuiteModeController(CorsairThermistor thermistor)
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

    public class CorsairFanBalancedModeController : CorsairFanCurveController
    { 
        public CorsairFanBalancedModeController()
        {
            
        }

        public CorsairFanBalancedModeController(CorsairThermistor thermistor)
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

    public class CorsairFanPerformanceModeController : CorsairFanCurveController
    {
        public CorsairFanPerformanceModeController()
        {
            
        }

        public CorsairFanPerformanceModeController(CorsairThermistor thermistor)
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
