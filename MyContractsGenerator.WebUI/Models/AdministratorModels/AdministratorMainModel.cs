using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyContractsGenerator.WebUI.Models.BaseModels;
using MyContractsGenerator.WebUI.Models.RoleModels;

namespace MyContractsGenerator.WebUI.Models.AdministratorModels
{
    public class AdministratorMainModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the administrators.
        /// </summary>
        /// <value>
        /// The administrators.
        /// </value>
        public IList<AdministratorModel> Administrators { get; set; }

        /// <summary>
        /// Gets or sets the edited administrator.
        /// </summary>
        /// <value>
        /// The edited administrator.
        /// </value>
        public AdministratorModel EditedAdministrator { get; set; }
    }
}