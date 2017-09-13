using System;

namespace MyContractsGenerator.Core.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.ApplicationException" />
    [Serializable]
    public class BusinessException : ApplicationException
    {
        /// <summary>
        /// The message
        /// </summary>
        private readonly string message;

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message { get { return this.message; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException"/> class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public BusinessException(string message)
        {
            this.message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public BusinessException(string message, params string[] args)
        {
            this.message = string.Format(message, args);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public BusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="args">The arguments.</param>
        public BusinessException(string message, Exception innerException, params string[] args)
            : base(message, innerException)
        {
            this.message = string.Format(message, args);
        }
    }
}
