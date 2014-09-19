using CorsairLinkPlusPlus.Common.Controller;
using System;
using System.Reflection;

namespace CorsairLinkPlusPlus.Common.Registry
{
    public class RegisteredController
    {
        private readonly string controllerName;
        private readonly ConstructorInfo ctor;
        private readonly Type type;

        internal RegisteredController(Type type, string name)
        {
            this.controllerName = name;
            this.ctor = type.GetConstructor(new Type[0]);
            this.type = type;
        }

        public Type[] GetImplementedInterfaces()
        {
            return type.GetInterfaces();
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
