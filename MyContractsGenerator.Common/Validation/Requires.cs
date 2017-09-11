//------------------------------------------------------------------------------
// <summary>
//    APPLICATION : MyContractsGenerator
//    Author : Clement Fevre
//    Description : Classe Requires pour tester les paramètres en entrée des méthodes
// </summary>
//------------------------------------------------------------------------------
using System;

namespace MyContractsGenerator.Common.Validation
{
    public static class Requires
    {
        /// <summary>
        ///     Throws exception when Argument is null.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="value">The value.</param>
        /// <param name="name"> The name.</param>
        public static void ArgumentNotNull(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        ///     Throws exception when Argument is not greater than zero.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        public static void ArgumentGreaterThanZero(int value, string name)
        {
            if (value <= 0)
            {
                throw new ArgumentException(string.Format("Value : {0} ({1}) must be greater than Zero", value, name));
            }
        }

        /// <summary>
        ///     Throws exception when String argument is null or empty or white space.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="value">The value.</param>
        /// <param name="name"> The name.</param>
        public static void StringArgumentNotNullOrEmptyOrWhiteSpace(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}