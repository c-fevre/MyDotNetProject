using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyContractsGenerator.Business;
using MyContractsGenerator.WebUI.Models.BaseModels;
using MyContractsGenerator.WebUI.Models.CollaboratorModels;
using MyContractsGenerator.WebUI.Models.QuestionModels;
using MyContractsGenerator.WebUI.Models.RoleModels;

namespace MyContractsGenerator.WebUI.Models.CollaboratorFormModels
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MyContractsGenerator.WebUI.Models.BaseModels.BaseModel" />
    public class CollaboratorFormModel
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
        /// Gets or sets the collaborator.
        /// </summary>
        /// <value>
        /// The collaborator.
        /// </value>
        public CollaboratorModel Collaborator { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public RoleModel Role { get; set; }

        /// <summary>
        /// Gets or sets the questions.
        /// </summary>
        /// <value>
        /// The questions.
        /// </value>
        public IList<QuestionModel> Questions { get; set; }
    }
}