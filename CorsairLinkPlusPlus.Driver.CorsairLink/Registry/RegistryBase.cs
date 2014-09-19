using System;
using System.Collections.Generic;
using System.Reflection;

namespace CorsairLinkPlusPlus.Driver.CorsairLink.Registry
{
    public class RegistryBase<T>
    {
        protected RegistryBase() { }

        protected static T ConstructObjectForInspection(Type type)
        {
            return (T)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
        }

        protected static List<Type> GetSubtypesInNamespace(string namespaceName)
        {
            List<Type> types = new List<Type>();
            Assembly asm = Assembly.GetCallingAssembly();
            foreach(Type type in asm.GetExportedTypes())
                if (!type.IsInterface && !type.IsAbstract && type.Namespace == namespaceName && typeof(T).IsAssignableFrom(type))
                    types.Add(type);

            return types;
        }
    }
}
