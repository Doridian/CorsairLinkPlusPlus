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

        public bool IsImplemented(Type _interface)
        {
            return _interface.IsAssignableFrom(type);
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
