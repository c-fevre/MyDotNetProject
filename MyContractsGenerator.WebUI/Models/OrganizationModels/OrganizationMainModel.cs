using System.Collections.Generic;
using MyContractsGenerator.WebUI.Models.BaseModels;

namespace MyContractsGenerator.WebUI.Models.OrganizationModels
{
    public class OrganizationMainModel : BaseModel
    {
        /// <summary>
        ///     Gets or sets the organizations.
        /// </summary>
        /// <value>
        ///     The organizations.
        /// </value>
        public IList<OrganizationModel> Organizations { get; set; }

        /// <summary>
        ///     Gets or sets the edited organization.
        /// </summary>
        /// <value>
        ///     The edited organization.
        /// </value>
        public OrganizationModel EditedOrganization { get; set; }
    }
}