using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PygmentSharp.Core
{
    /// <summary>
    /// Finds classes with a given attribute
    /// </summary>
    /// <typeparam name="TAttr"></typeparam>
    public class AttributeLocator<TAttr> where TAttr:Attribute
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly Lazy<IEnumerable<Type>> _types = new Lazy<IEnumerable<Type>>(GetTypes);

        /// <summary>
        /// Gets all the types in the current assebmbly
        /// </summary>
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