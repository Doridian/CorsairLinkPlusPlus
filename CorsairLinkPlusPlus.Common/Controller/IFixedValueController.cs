
namespace CorsairLinkPlusPlus.Common.Controller
{
    public interface IFixedValueController<T> : IController
    {
        void SetValue(T value);
        T GetValue();
    }
}
