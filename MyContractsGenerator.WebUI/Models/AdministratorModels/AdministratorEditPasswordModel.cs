using System.ComponentModel.DataAnnotations;
using MyContractsGenerator.Common.I18N;

namespace MyContractsGenerator.WebUI.Models.administratorModels
{
    public class AdministratorEditPasswordModel
    {
        [Required]
        [Display(Name = "Login_Label", ResourceType = typeof(Resources))]
        public string Login { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "Shared_RequiredField", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "administrator_CurrentPassword", ResourceType = typeof(Resources))]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "Shared_RequiredField", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "administrator_NewPassword", ResourceType = typeof(Resources))]
        [StringLength(40, MinimumLength = 4, ErrorMessageResourceName = "Shared_MessageError_MinAndMaxLength",
            ErrorMessageResourceType = typeof(Resources))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "Shared_RequiredField", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "administrator_NewPassWordConfirmation", ResourceType = typeof(Resources))]
        [StringLength(40, MinimumLength = 4, ErrorMessageResourceName = "Shared_MessageError_MinAndMaxLength",
            ErrorMessageResourceType = typeof(Resources))]
        public string NewPasswordConfirmation { get; set; }
    }
}