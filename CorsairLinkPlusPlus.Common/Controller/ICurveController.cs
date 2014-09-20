using CorsairLinkPlusPlus.Common.Utility;

namespace CorsairLinkPlusPlus.Common.Controller
{
    public interface ICurveController<K, V> : IController
    {
        ControlCurve<K, V> Curve { get; set; }
    }
}
