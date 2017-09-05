using System.Linq;
using System.Web.Mvc;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.Common.PasswordHelper;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesServices;
using MyContractsGenerator.WebUI.Mapping;
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
        /// The form answer service
        /// </summary>
        private readonly IFormService formService;

        /// <summary>
        /// The form answer service
        /// </summary>
        private readonly IFormAnswerService formAnswerService;

        /// <summary>
        /// The mail service
        /// </summary>
        private readonly IMailService mailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollaboratorFormController"/> class.
        /// </summary>
        /// <param name="collaboratorService">The collaborator service.</param>
        /// <param name="roleService">The role service.</param>
        /// <param name="formAnswerService">The form answer service.</param>
        /// <param name="mailService">The mail service.</param>
        public CollaboratorFormController(ICollaboratorService collaboratorService, IRoleService roleService,
                                          IFormService formService, IFormAnswerService formAnswerService,
                                          IMailService mailService)
        {
            this.collaboratorService = collaboratorService;
            this.roleService = roleService;
            this.formAnswerService = formAnswerService;
            this.formService = formService;
            this.mailService = mailService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult WhoAreYou(string c, string fa)
        {
            CollaboratorFormMainModel model = new CollaboratorFormMainModel { c = c, fa = fa };

            return this.View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Checkin(CollaboratorFormMainModel model)
        {
            // model validation
            Requires.ArgumentNotNull(model, "model");

            if (!this.ModelState.IsValid)
            {
                return this.View("WhoAreYou", model);
            }
            
            collaborator collaboratorToCheck = this.collaboratorService.GetByEmail(model.Email);

            if (collaboratorToCheck == null)
            {
                this.ModelState.AddModelError("Password",
                                              Resources.CollaboratorForm_Error);
                return this.View("WhoAreYou", model);
            }

            form_answer lastFormAnswer = collaboratorToCheck.form_answer.OrderBy(c => c.last_update).FirstOrDefault();

            if (lastFormAnswer != null &&
                model.fa == ShaHashPassword.GetSha256ResultString(lastFormAnswer.id.ToString()) &&
                model.c == ShaHashPassword.GetSha256ResultString(collaboratorToCheck.id.ToString()))
            {
                if (lastFormAnswer.password != ShaHashPassword.GetSha256ResultString(model.Password))
                {
                    this.ModelState.AddModelError("Password",
                                                  Resources.CollaboratorForm_Error);
                    return this.View("WhoAreYou", model);
                }

                // Added a checked test : TODO see if we keep this or not (not used for now)
                lastFormAnswer.@checked = true;
                this.formAnswerService.UpdateFormAnswer(lastFormAnswer);

                // TODO OK : Rediriger vers la page du formulaire, en passant dans le model le collaborateur
                // Gérer les réponse au formulaire
                // Afficher les réponses
                return this.RedirectToAction("YourForm", model);
            }
            else
            {
                this.ModelState.AddModelError("Password",
                                              Resources.CollaboratorForm_Error);
                return this.View("WhoAreYou", model);
            }

        }

        /// <summary>
        /// Yours the form.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult YourForm(CollaboratorFormMainModel model)
        {
            // model validation
            Requires.ArgumentNotNull(model, "model");

            if (!this.ModelState.IsValid)
            {
                return this.View("WhoAreYou", model);
            }

            collaborator collaboratorToCheck = this.collaboratorService.GetByEmail(model.Email);

            if (collaboratorToCheck == null)
            {
                this.ModelState.AddModelError("Password",
                                              Resources.CollaboratorForm_Error);
                return this.View("WhoAreYou", model);
            }

            form_answer lastFormAnswer = collaboratorToCheck.form_answer.OrderBy(c => c.last_update).FirstOrDefault();

            if (lastFormAnswer != null &&
                model.fa == ShaHashPassword.GetSha256ResultString(lastFormAnswer.id.ToString()) &&
                model.c == ShaHashPassword.GetSha256ResultString(collaboratorToCheck.id.ToString()))
            {
                this.PopulateFormMainModel(model, lastFormAnswer);
            }
            else
            {
                this.ModelState.AddModelError("Password",
                                              Resources.CollaboratorForm_Error);
                return this.View("WhoAreYou", model);
            }

            return this.View(model);
        }

        /// <summary>
        /// Populates the form main model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="lastFormAnswer">The last form answer.</param>
        private void PopulateFormMainModel(CollaboratorFormMainModel model, form_answer lastFormAnswer)
        {
            form dbForm = this.formService.GetById(lastFormAnswer.form_id);
            collaborator dbCollaborator = this.collaboratorService.GetByEmail(model.Email);

            model.Form = new CollaboratorFormModel
            {
                Label = dbForm.label,
                Id = dbForm.id,
                Collaborator = CollaboratorMap.MapItem(dbCollaborator),
                Roles = RoleMap.MapItems(dbCollaborator.roles),
                Questions = QuestionMap.MapItems(dbForm.questions)
            };
        }
    }
}