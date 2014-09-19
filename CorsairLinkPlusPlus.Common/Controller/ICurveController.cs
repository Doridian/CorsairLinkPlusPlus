using CorsairLinkPlusPlus.Common.Utility;

namespace CorsairLinkPlusPlus.Common.Controller
{
    public interface ICurveController<K, V> : IController
    {
        void SetCurve(ControlCurve<K, V> value);
        ControlCurve<K, V> GetCurve();
    }
}
