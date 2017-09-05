using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.WebUI.Models.BaseModels;

namespace MyContractsGenerator.WebUI.Models.CollaboratorFormModels
{
    public class CollaboratorFormMainModel
    {
        /// <summary>
        /// Gets or sets the hashed mail.
        /// </summary>
        /// <value>
        /// The hashed mail.
        /// </value>
        public string c { get; set; }

        /// <summary>
        /// Gets or sets the fa.
        /// </summary>
        /// <value>
        /// The fa.
        /// </value>
        public string fa { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Display(Name = "Login_Password", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "Shared_RequiredField", ErrorMessageResourceType = typeof(Resources))]
        public string Password { get; set; }

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
        /// Gets or sets the form.
        /// </summary>
        /// <value>
        /// The form.
        /// </value>
        public CollaboratorFormModel Form { get; set; }
    }
}