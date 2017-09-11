using System.ComponentModel.DataAnnotations;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.WebUI.Models.BaseModels;

namespace MyContractsGenerator.WebUI.Models.administratorModels
{
    public class AdministratorLoginModel : BaseModel
    {
        [Display(Name = "Email_Label", ResourceType = typeof(Resources))]
        [EmailAddress(ErrorMessageResourceName = "Collaborator_ErrorIncorrectEmail",
            ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Required(ErrorMessageResourceName = "Shared_RequiredField", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(100, ErrorMessageResourceName = "Shared_MessageError_MinAndMaxLength",
            ErrorMessageResourceType = typeof(Resources))]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "Shared_RequiredField", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(100, ErrorMessageResourceName = "Shared_MessageError_MinAndMaxLength",
            ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Login_Password", ResourceType = typeof(Resources))]
        public string Password { get; set; }
    }
}