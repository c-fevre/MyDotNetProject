using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContractsGenerator.Core.Enum
{
    /// <summary>
    /// 
    /// </summary>
    public static class QuestionType
    {
        /// <summary>
        /// 
        /// </summary>
        public enum QuestionTypeEnum
        {
            [Description("text")]
            Text,

            [Description("numeric")]
            Numeric,

            [Description("boolean")]
            Boolean,

            [Description("date")]
            Datetime
        }
      }
}
