using System.ComponentModel.DataAnnotations;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.WebUI.Models.BaseModels;

namespace MyContractsGenerator.WebUI.Models.administratorModels
{
    public class AdministratorLoginModel : BaseModel
    {
        [Display(Name = "Login_Label", ResourceType = typeof(Resources))]
        public string Login { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Login_Password", ResourceType = typeof(Resources))]
        public string Password { get; set; }
    }
}