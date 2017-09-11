using System.Collections.Generic;
using MyContractsGenerator.WebUI.Models.BaseModels;
using MyContractsGenerator.WebUI.Models.CollaboratorModels;
using MyContractsGenerator.WebUI.Models.FormAnswerModels;
using MyContractsGenerator.WebUI.Models.RoleModels;

namespace MyContractsGenerator.WebUI.Models.FormModels
{
    public class AnswersModel : BaseModel
    {
        /// <summary>
        ///     Gets or sets the role.
        /// </summary>
        /// <value>
        ///     The role.
        /// </value>
        public RoleModel Role { get; set; }

        /// <summary>
        ///     Gets or sets the collaborator.
        /// </summary>
        /// <value>
        ///     The collaborator.
        /// </value>
        public CollaboratorModel Collaborator { get; set; }

        /// <summary>
        ///     Gets or sets the question answer.
        /// </summary>
        /// <value>
        ///     The question answer.
        /// </value>
        public IList<FormAnswerModel> FormAnswers { get; set; }
    }
}