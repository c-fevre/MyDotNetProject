using System.Collections.Generic;
using MyContractsGenerator.WebUI.Models.BaseModels;

namespace MyContractsGenerator.WebUI.Models.FormModels
{
    public class FormMainModel : BaseModel
    {
        public IList<FormMailingModel> RolesWithCollaborators { get; set; }
    }
}