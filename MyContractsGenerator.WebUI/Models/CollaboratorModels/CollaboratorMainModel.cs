using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyContractsGenerator.WebUI.Models.BaseModels;
using MyContractsGenerator.WebUI.Models.RoleModels;

namespace MyContractsGenerator.WebUI.Models.CollaboratorModels
{
    public class CollaboratorMainModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the collaborators.
        /// </summary>
        /// <value>
        /// The collaborators.
        /// </value>
        public IList<CollaboratorModel> Collaborators { get; set; }

        /// <summary>
        /// Gets or sets the edited collaborator.
        /// </summary>
        /// <value>
        /// The edited collaborator.
        /// </value>
        public CollaboratorModel EditedCollaborator { get; set; }

        /// <summary>
        /// Gets or sets the linked roles.
        /// </summary>
        /// <value>
        /// The linked roles.
        /// </value>
        public IEnumerable<SelectListItem> AvailableRoles { get; set; }

        // TODO Multilinguage
        //public List<SelectListItem> AvailableApplicationLangage { get; set; }
    }
}