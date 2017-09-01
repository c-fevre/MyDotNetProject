using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyContractsGenerator.WebUI.Models.BaseModels;

namespace MyContractsGenerator.WebUI.Models.RoleModels
{
    public class RoleMainModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public IList<RoleModel> Roles { get; set; }

        /// <summary>
        /// Gets or sets the edited role.
        /// </summary>
        /// <value>
        /// The edited role.
        /// </value>
        public RoleModel EditedRole { get; set; }
    }
}