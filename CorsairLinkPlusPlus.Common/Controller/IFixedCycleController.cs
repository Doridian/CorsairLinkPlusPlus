using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Common.Controller
{
    public interface IFixedCycleController<V> : IController
    {
        void SetCycle(V[] cycle);
        V[] GetCycle();
    }
}
