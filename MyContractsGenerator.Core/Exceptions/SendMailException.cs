using MyContractsGenerator.Common.I18N;
using System;

namespace MyContractsGenerator.Core.Exceptions
{

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class SendMailException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SendMailException"/> class.
        /// </summary>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        public SendMailException(Exception innerException, string email)
            : base(String.Format(Resources.SendMailExceptionMessage, email), innerException)
        {
        }
    }
}