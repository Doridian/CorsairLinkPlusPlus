
namespace CorsairLinkPlusPlus.Common.Controller
{
    public interface IFixedValueController<T> : IController
    {
        T Value { get; set; }
    }
}
