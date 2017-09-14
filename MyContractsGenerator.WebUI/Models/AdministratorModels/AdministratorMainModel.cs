using System.Collections.Generic;
using System.Web.Mvc;
using MyContractsGenerator.WebUI.Models.BaseModels;

namespace MyContractsGenerator.WebUI.Models.AdministratorModels
{
    public class AdministratorMainModel : BaseModel
    {
        /// <summary>
        ///     Gets or sets the administrators.
        /// </summary>
        /// <value>
        ///     The administrators.
        /// </value>
        public IList<AdministratorModel> Administrators { get; set; }

        /// <summary>
        ///     Gets or sets the edited administrator.
        /// </summary>
        /// <value>
        ///     The edited administrator.
        /// </value>
        public AdministratorModel EditedAdministrator { get; set; }

        /// <summary>
        /// Gets or sets the available organizations.
        /// </summary>
        /// <value>
        /// The available organizations.
        /// </value>
        public IList<SelectListItem> AvailableOrganizations { get; set; }
    }
}