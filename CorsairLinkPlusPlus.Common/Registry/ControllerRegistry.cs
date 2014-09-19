using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.Common.Registry
{
    public static class ControllerRegistry
    {
        private static Dictionary<string, RegisteredController> controllers = new Dictionary<string, RegisteredController>();

        public static RegisteredController Get(string name, Type type = null)
        {
            if (controllers.ContainsKey(name))
                return controllers[name];
            if (type == null)
                throw new KeyNotFoundException();
            RegisteredController controller = new RegisteredController(type.GetConstructor(new Type[0]), name);
            controllers.Add(name, controller);
            return controller;
        }
    }
}
