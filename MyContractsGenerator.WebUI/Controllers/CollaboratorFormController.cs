using System.Web.Mvc;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.Common.PasswordHelper;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.Interfaces.InterfacesServices;
using MyContractsGenerator.WebUI.Models.CollaboratorFormModels;

namespace MyContractsGenerator.WebUI.Controllers
{
    [Authorize]
    public class CollaboratorFormController : BaseController
    {
        /// <summary>
        /// The collaborator service
        /// </summary>
        private readonly ICollaboratorService collaboratorService;

        /// <summary>
        /// The role service
        /// </summary>
        private readonly IRoleService roleService;

        /// <summary>
        /// The mail service
        /// </summary>
        private readonly IMailService mailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollaboratorFormController"/> class.
        /// </summary>
        /// <param name="collaboratorService">The collaborator service.</param>
        /// <param name="roleService">The role service.</param>
        /// <param name="mailService">The mail service.</param>
        public CollaboratorFormController(ICollaboratorService collaboratorService, IRoleService roleService, IMailService mailService)
        {
            this.collaboratorService = collaboratorService;
            this.roleService = roleService;
            this.mailService = mailService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult WhoAreYou(string c)
        {
            CollaboratorFormMainModel model = new CollaboratorFormMainModel { HashedMail = c };

            return this.View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult EmailCheck(CollaboratorFormMainModel model)
        {
            // model validation
            Requires.ArgumentNotNull(model, "model");

            if (!this.ModelState.IsValid)
            {
                return this.View("WhoAreYou", model);
            }

            if (model.HashedMail == ShaHashPassword.GetSha256ResultString(model.Email))
            {
                // TODO OK : Rediriger vers la page du formulaire, en passant dans le model le collaborateur
                // Gérer les réponse au formulaire
                // Afficher les réponses
                return this.View("WhoAreYou", model);
            }
            else
            {
                this.ModelState.AddModelError("Email",
                                              Resources.CollaboratorForm_UnknownEmail);
                return this.View("WhoAreYou", model);
            }

        }

    }
}