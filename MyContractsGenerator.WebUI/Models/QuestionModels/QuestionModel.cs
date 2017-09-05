using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyContractsGenerator.Core.Enum;
using MyContractsGenerator.WebUI.Models.BaseModels;

namespace MyContractsGenerator.WebUI.Models.QuestionModels
{
    public class QuestionModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public QuestionType.QuestionTypeEnum Type { get; set; }
    }
}