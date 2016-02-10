using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PygmentSharp.Core
{
    public class AttributeLocator<TAttr> where TAttr:Attribute
    {
        private static readonly Lazy<IEnumerable<Type>> _types = new Lazy<IEnumerable<Type>>(GetTypes);
        protected IEnumerable<Type> Types => _types.Value;

        private static IEnumerable<Type> GetTypes()
        {
            return GetTypesWithAttribute<TAttr>().ToArray();
        }
        private static IEnumerable<Type> GetTypesWithAttribute<T>() where T : Attribute
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetTypes().Where(t => t.GetCustomAttributes<T>(true).Any());
        }
    }
}