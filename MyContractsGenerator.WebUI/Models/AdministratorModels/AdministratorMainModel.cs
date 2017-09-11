using System.Collections.Generic;
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
    }
}