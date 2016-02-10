using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PygmentSharp.Core.Extensions
{
    public static class TypeExtensions
    {
        public static T InstantiateAs<T>(this Type type)
        {
            return (T)Activator.CreateInstance(type);
        }

        public static bool HasAttribute<TAttr>(this Type type, Func<TAttr, bool> predicate) where TAttr:Attribute
        {
            return type.GetCustomAttributes<TAttr>()
                .Any(predicate);
        }
    }
}
