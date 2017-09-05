using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyContractsGenerator.WebUI.Models.BaseModels;

namespace MyContractsGenerator.WebUI.Models.FormAnswerModels
{
    public class FormAnswerModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the last up date time.
        /// </summary>
        /// <value>
        /// The last up date time.
        /// </value>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FormAnswerModel"/> is replied.
        /// </summary>
        /// <value>
        ///   <c>true</c> if replied; otherwise, <c>false</c>.
        /// </value>
        public bool Replied { get; set; }
    }
}