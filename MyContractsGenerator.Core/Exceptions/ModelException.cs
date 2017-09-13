using System;
using System.Collections.Generic;
using System.Linq;

namespace MyContractsGenerator.Core.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.ApplicationException" />
    public class ModelException : ApplicationException
    {
        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public List<ErrorField> Errors { get; private set; }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message
        {
            get
            {
                return string.Join("\n", this.Errors.Select(e=> e.Message));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelException"/> class.
        /// </summary>
        /// <param name="errors">The errors.</param>
        public ModelException(List<ErrorField> errors)
        {
            this.Errors = errors;
        }

        /// <summary>
        /// 
        /// </summary>
        public class ErrorField
        {
            /// <summary>
            /// Champ en erreur
            /// </summary>
            public string Field { get; set; }

            /// <summary>
            /// Message d'erreur associé au champ
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="ErrorField"/> class.
            /// </summary>
            /// <param name="field">The field.</param>
            /// <param name="message">The message.</param>
            public ErrorField(string field, string message)
            {
                this.Field = field;
                this.Message = message;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ErrorField"/> class.
            /// </summary>
            /// <param name="field">The field.</param>
            /// <param name="message">The message.</param>
            /// <param name="args">The arguments.</param>
            public ErrorField(string field, string message, params string[] args)
            {
                this.Field = field;
                this.Message = string.Format(message, args);
            }
        }
    }
}
