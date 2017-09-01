using System.Collections.Generic;
using MyContractsGenerator.WebUI.Models.BaseModels;
using MyContractsGenerator.WebUI.Models.CollaboratorModels;
using MyContractsGenerator.WebUI.Models.RoleModels;

namespace MyContractsGenerator.WebUI.Models.FormModels
{
    public class FormMainModel : BaseModel
    {
        public IList<FormMailingModel> RolesWithCollaborators { get; set; }
        
    }
}