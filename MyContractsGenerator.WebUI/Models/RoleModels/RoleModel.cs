using System.ComponentModel.DataAnnotations;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.WebUI.Models.BaseModels;

namespace MyContractsGenerator.WebUI.Models.RoleModels
{
    public class RoleModel : BaseModel
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the last name.
        /// </summary>
        /// <value>
        ///     The last name.
        /// </value>
        [Required(ErrorMessageResourceName = "Shared_RequiredField", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "Role_Label_Name", ResourceType = typeof(Resources))]
        [StringLength(40, ErrorMessageResourceName = "Shared_MessageError_MaxLength",
            ErrorMessageResourceType = typeof(Resources))]
        public string Label { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }
    }
}