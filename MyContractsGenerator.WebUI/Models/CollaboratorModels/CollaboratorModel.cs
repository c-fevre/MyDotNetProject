using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.WebUI.Models.BaseModels;
using MyContractsGenerator.WebUI.Models.RoleModels;

namespace MyContractsGenerator.WebUI.Models.CollaboratorModels
{
    public class CollaboratorModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [Required(ErrorMessageResourceName = "Shared_RequiredField", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Collaborator_LastName", ResourceType = typeof(Resources))]
        [StringLength(40, ErrorMessageResourceName = "Shared_MessageError_MaxLength",
            ErrorMessageResourceType = typeof(Resources))]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Required(ErrorMessageResourceName = "Shared_RequiredField", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Collaborator_FirstName", ResourceType = typeof(Resources))]
        [StringLength(40, ErrorMessageResourceName = "Shared_MessageError_MaxLength",
            ErrorMessageResourceType = typeof(Resources))]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Required(ErrorMessageResourceName = "Shared_RequiredField", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Collaborator_Email", ResourceType = typeof(Resources))]
        [EmailAddress(ErrorMessageResourceName = "Collaborator_ErrorIncorrectEmail",
            ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [StringLength(100, ErrorMessageResourceName = "Shared_MessageError_MinAndMaxLength",
            ErrorMessageResourceType = typeof(Resources))]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the linked roles.
        /// </summary>
        /// <value>
        /// The linked roles.
        /// </value>
        [Display(Name = "Collaborator_Role", ResourceType = typeof(Resources))]
        public IEnumerable<int> LinkedRolesIds { get; set; }

        /// <summary>
        /// Gets or sets the form URL.
        /// </summary>
        /// <value>
        /// The form URL.
        /// </value>
        public string FormUrl { get; set; }

    }
}