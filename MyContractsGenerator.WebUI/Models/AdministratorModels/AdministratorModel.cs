using System.ComponentModel.DataAnnotations;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.WebUI.Models.BaseModels;

namespace MyContractsGenerator.WebUI.Models.AdministratorModels
{
    public class AdministratorModel : BaseModel
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the organization identifier.
        /// </summary>
        /// <value>
        ///     The organization identifier.
        /// </value>
        public int OrganizationId { get; set; }

        /// <summary>
        ///     Gets or sets the last name.
        /// </summary>
        /// <value>
        ///     The last name.
        /// </value>
        [Required(ErrorMessageResourceName = "Shared_RequiredField", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Administrator_LastName", ResourceType = typeof(Resources))]
        [StringLength(40, ErrorMessageResourceName = "Shared_MessageError_MaxLength",
            ErrorMessageResourceType = typeof(Resources))]
        public string LastName { get; set; }

        /// <summary>
        ///     Gets or sets the first name.
        /// </summary>
        /// <value>
        ///     The first name.
        /// </value>
        [Required(ErrorMessageResourceName = "Shared_RequiredField", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Administrator_FirstName", ResourceType = typeof(Resources))]
        [StringLength(40, ErrorMessageResourceName = "Shared_MessageError_MaxLength",
            ErrorMessageResourceType = typeof(Resources))]
        public string FirstName { get; set; }

        /// <summary>
        ///     Gets or sets the email.
        /// </summary>
        /// <value>
        ///     The email.
        /// </value>
        [Required(ErrorMessageResourceName = "Shared_RequiredField", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Administrator_Email", ResourceType = typeof(Resources))]
        [EmailAddress(ErrorMessageResourceName = "Administrator_ErrorIncorrectEmail",
            ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [StringLength(100, ErrorMessageResourceName = "Shared_MessageError_MinAndMaxLength",
            ErrorMessageResourceType = typeof(Resources))]
        public string Email { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        ///     Gets or sets the current password.
        /// </summary>
        /// <value>
        ///     The current password.
        /// </value>
        [Display(Name = "Administrator_CurrentPassword", ResourceType = typeof(Resources))]
        public string CurrentPassword { get; set; }

        /// <summary>
        ///     Gets or sets the reset password.
        /// </summary>
        /// <value>
        ///     The reset password.
        /// </value>
        [Display(Name = "Administrator_NewPassword", ResourceType = typeof(Resources))]
        [StringLength(40, MinimumLength = 4, ErrorMessageResourceName = "Shared_MessageError_MinAndMaxLength",
            ErrorMessageResourceType = typeof(Resources))]
        public string NewPassword { get; set; }

        /// <summary>
        ///     Gets or sets the reset password confirmation.
        /// </summary>
        /// <value>
        ///     The reset password confirmation.
        /// </value>
        [Display(Name = "Administrator_NewPasswordConfirmation", ResourceType = typeof(Resources))]
        [StringLength(40, MinimumLength = 4, ErrorMessageResourceName = "Shared_MessageError_MinAndMaxLength",
            ErrorMessageResourceType = typeof(Resources))]
        public string NewPasswordConfirmation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is removable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is removable; otherwise, <c>false</c>.
        /// </value>
        public bool IsRemovable{ get; set; }
    }
}