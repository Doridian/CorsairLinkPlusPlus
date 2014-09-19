using CorsairLinkPlusPlus.Common.Controller;
using System;
using System.Reflection;

namespace CorsairLinkPlusPlus.Common.Registry
{
    public class RegisteredController
    {
        private string controllerName;
        private ConstructorInfo ctor;
        private Type type;

        internal RegisteredController(Type type, string name)
        {
            this.controllerName = name;
            this.ctor = type.GetConstructor(new Type[0]);
            this.type = type;
        }

        public Type GetControllerType()
        {
            return type;
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
