#region LICENSE
/**
 * CorsairLinkPlusPlus
 * Copyright (c) 2014, Mark Dietzer & Simon Schick, All rights reserved.
 *
 * CorsairLinkPlusPlus is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 *
 * CorsairLinkPlusPlus is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with CorsairLinkPlusPlus.
 */
 #endregion

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
            RegisteredController controller = new RegisteredController(type, name);
            controllers.Add(name, controller);
            return controller;
        }
    }
}
