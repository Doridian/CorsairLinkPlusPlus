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
using CorsairLinkPlusPlus.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace CorsairLinkPlusPlus.RESTAPI.Methods
{
    public abstract class BaseMethod
    {
        public IDevice Device;
        public Dictionary<string, object> Arguments;

        public static void Execute(string name, IDevice device, Dictionary<string, object> arguments)
        {
            Methods.BaseMethod method;
            switch (name)
            {
                case "Refresh":
                    method = new Methods.Refresh();
                    break;
                case "SetController":
                    method = new Methods.SetController();
                    break;
                default:
                    throw new ArgumentException("Invalid method");
            }
            method.Arguments = arguments;
            method.Device = device;
            if (!method.IsDeviceValid())
                throw new ArgumentException("Invalid method for device");
            method.Call();
        }

        public abstract void Call();

        public virtual bool IsDeviceValid()
        {
            return true;
        }

        protected T GetOptionalArgument<T>(string name, T defaultValue = default(T))
        {
            return GetArgument<T>(name, true, defaultValue);
        }

        protected T GetRequiredArgument<T>(string name)
        {
            return GetArgument<T>(name, false);
        }

        protected T GetArgument<T>(string name, bool optional = false, T defaultValue = default(T))
        {
            if (!Arguments.ContainsKey(name))
            {
                if (!optional)
                    throw new ArgumentNullException("Argument " + name + " is required");
                return defaultValue;
            }
            dynamic argValue = (dynamic)Arguments[name];
            if (argValue is JObject)
                argValue = JsonConvert.DeserializeObject<Dictionary<string, object>>(((JObject)argValue).ToString());
            return (T)argValue;
        }
    }
}
