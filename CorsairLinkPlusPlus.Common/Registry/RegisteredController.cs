using CorsairLinkPlusPlus.Common.Controller;
using System.Reflection;

namespace CorsairLinkPlusPlus.Common.Registry
{
    public class RegisteredController
    {
        private string controllerName;
        private ConstructorInfo ctor;

        internal RegisteredController(ConstructorInfo ctor, string name)
        {
            this.controllerName = name;
            this.ctor = ctor;
        }

        public IController New()
        {
            return (IController)ctor.Invoke(null);
        }

        public string GetName()
        {
            return controllerName;
        }
    }
}
