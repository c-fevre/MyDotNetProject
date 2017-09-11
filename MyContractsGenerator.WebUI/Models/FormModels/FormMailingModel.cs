using System.Collections.Generic;
using MyContractsGenerator.WebUI.Models.BaseModels;
using MyContractsGenerator.WebUI.Models.CollaboratorModels;
using MyContractsGenerator.WebUI.Models.RoleModels;

namespace MyContractsGenerator.WebUI.Models.FormModels
{
    public class FormMailingModel : BaseModel
    {
        /// <summary>
        ///     Gets or sets the role.
        /// </summary>
        /// <value>
        ///     The role.
        /// </value>
        public RoleModel Role { get; set; }

        /// <summary>
        ///     Gets or sets the collaborators.
        /// </summary>
        /// <value>
        ///     The collaborators.
        /// </value>
        public IList<CollaboratorModel> Collaborators { get; set; }
    }
}