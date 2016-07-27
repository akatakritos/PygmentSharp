using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace PygmentSharp.Core.Utils
{
    internal static class Argument
    {
        /// <summary>
        /// Validates a value is not null
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="arg"/> is null</exception>
        /// <param name="arg">The argument to validate</param>
        /// <param name="argName">The name of the argument</param>
        // ReSharper disable once UnusedParameter.Global
        [AssertionMethod]
        public static void EnsureNotNull([AssertionCondition(AssertionConditionType.IS_NOT_NULL)]object arg, [InvokerParameterName]string argName)
        {
            if (arg == null)
                throw new ArgumentNullException(argName);
        }
    }
}
