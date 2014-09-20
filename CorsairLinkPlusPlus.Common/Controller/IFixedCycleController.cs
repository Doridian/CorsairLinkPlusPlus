using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Common.Controller
{
    public interface IFixedCycleController<V> : IController
    {
        V[] Value { get; set; }
    }
}
