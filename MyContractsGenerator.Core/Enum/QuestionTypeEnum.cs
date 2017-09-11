using System.ComponentModel;

namespace MyContractsGenerator.Core.Enum
{
    /// <summary>
    /// </summary>
    public static class QuestionType
    {
        /// <summary>
        /// </summary>
        public enum QuestionTypeEnum
        {
            [Description("text")] Text,

            [Description("numeric")] Numeric,

            [Description("boolean")] Boolean,

            [Description("date")] Datetime
        }
    }
}