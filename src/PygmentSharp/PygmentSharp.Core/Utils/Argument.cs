using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace PygmentSharp.Core.Utils
{
    public static class Argument
    {
        /// <summary>
        /// Validates a value is not null
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <see cref="arg"/> is null</exception>
        /// <param name="arg">The argument to validate</param>
        /// <param name="argName">The name of the argument</param>
        // ReSharper disable once UnusedParameter.Global
        [AssertionMethod]
        public static void EnsureNotNull([AssertionCondition(AssertionConditionType.IS_NOT_NULL)]object arg, [InvokerParameterName]string argName)
        {
            if (arg == null)
                throw new ArgumentNullException(argName);
        }

        /// <summary>
        /// Validates no elements in the given list are null
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if <see cref="arg"/> contains a null element</exception>
        /// <param name="arg">The list to validate</param>
        /// <param name="argName">The name of the argument</param>
        // ReSharper disable once UnusedParameter.Global
        public static void EnsureNoNullElements(IReadOnlyCollection<object>arg, [InvokerParameterName]string argName)
        {
            if (arg.Any(s => s == null)) throw new ArgumentException("Null list elements not permitted", argName);
        }
    }
}
