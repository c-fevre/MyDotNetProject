using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.WebUI.Models.BaseModels;

namespace MyContractsGenerator.WebUI.Models.CollaboratorFormModels
{
    public class CollaboratorFormMainModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the hashed mail.
        /// </summary>
        /// <value>
        /// The hashed mail.
        /// </value>
        public string HashedMail { get; set; }

        [Required(ErrorMessageResourceName = "Shared_RequiredField", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Collaborator_Email", ResourceType = typeof(Resources))]
        [EmailAddress(ErrorMessageResourceName = "Collaborator_ErrorIncorrectEmail",
    ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [StringLength(100, ErrorMessageResourceName = "Shared_MessageError_MinAndMaxLength",
    ErrorMessageResourceType = typeof(Resources))]
        public string Email { get; set; }
    }
}