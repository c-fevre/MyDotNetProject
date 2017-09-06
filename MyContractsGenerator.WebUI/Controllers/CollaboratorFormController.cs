using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Web.Mvc;
using MyContractsGenerator.Common;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.Common.PasswordHelper;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesServices;
using MyContractsGenerator.WebUI.Mapping;
using MyContractsGenerator.WebUI.Models.AnswerModels;
using MyContractsGenerator.WebUI.Models.CollaboratorFormModels;
using MyContractsGenerator.WebUI.Models.QuestionModels;

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
        /// The answer service
        /// </summary>
        private readonly IAnswerService answerService;

        /// <summary>
        /// The mail service
        /// </summary>
        private readonly IMailService mailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollaboratorFormController"/> class.
        /// </summary>
        /// <param name="collaboratorService">The collaborator service.</param>
        /// <param name="roleService">The role service.</param>
        /// <param name="formService">The form service.</param>
        /// <param name="formAnswerService">The form answer service.</param>
        /// <param name="answerService">The answer service.</param>
        public CollaboratorFormController(ICollaboratorService collaboratorService, IRoleService roleService,
                                          IFormService formService, IFormAnswerService formAnswerService, IAnswerService answerService,
                                          IMailService mailService)
        {
            this.collaboratorService = collaboratorService;
            this.roleService = roleService;
            this.formAnswerService = formAnswerService;
            this.formService = formService;
            this.answerService = answerService;
            this.mailService = mailService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult WhoAreYou(string c, string fa)
        {
            CollaboratorFormMainModel model = new CollaboratorFormMainModel { c = c, fa = fa };

            return this.View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Thanks()
        {

            return this.View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult AlreadyReplied()
        {
            return this.View();
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
                this.ModelState.AddModelError(string.Empty,
                                              Resources.CollaboratorForm_Error);
                return this.View("WhoAreYou", model);
            }

            form_answer lastFormAnswer = collaboratorToCheck.form_answer.OrderByDescending(c => c.last_update).FirstOrDefault(lfa => !lfa.replied);


            if (lastFormAnswer != null &&
                model.fa == ShaHashPassword.GetSha256ResultString(lastFormAnswer.id.ToString()) &&
                model.c == ShaHashPassword.GetSha256ResultString(collaboratorToCheck.id.ToString()))
            {
                model.Password = ShaHashPassword.GetSha256ResultString(model.Password);

                if (lastFormAnswer.password != model.Password)
                {
                    this.ModelState.AddModelError(string.Empty,
                                                  Resources.CollaboratorForm_Error);
                    return this.View("WhoAreYou", model);
                }
                
                return this.RedirectToAction("YourForm", model);
            }
            else
            {
                this.ModelState.AddModelError(string.Empty,
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
                this.ModelState.AddModelError(string.Empty,
                                              Resources.CollaboratorForm_Error);
                return this.View("WhoAreYou", model);
            }

            form_answer lastFormAnswer = collaboratorToCheck.form_answer.OrderByDescending(c => c.last_update).FirstOrDefault();

            if (lastFormAnswer != null &&
                model.fa == ShaHashPassword.GetSha256ResultString(lastFormAnswer.id.ToString()) &&
                model.c == ShaHashPassword.GetSha256ResultString(collaboratorToCheck.id.ToString()))
            {
                this.PopulateFormMainModel(model, lastFormAnswer);
            }
            else
            {
                this.ModelState.AddModelError(string.Empty,
                                              Resources.CollaboratorForm_Error);
                return this.View("WhoAreYou", model);
            }

            return this.View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CheckForm(FormCollection model)
        {
            if (model == null || !model.HasKeys())
            {
                throw new InvalidCastException("Error while deconding form model - Don't cheat please.");
            }

            string collaboratorHashedId;
            string formAnswerHashedId;
            string passwordHashed;
            int formId;
            IList<AnswerModel> questionsAnswers;
            ProcessCustomForm(model, out questionsAnswers, out collaboratorHashedId, out formAnswerHashedId, out passwordHashed, out formId);

            form dbForm = this.formService.GetById(formId);

            var dbFormAnswer = CheckFormDataIntegrity(dbForm, passwordHashed, formAnswerHashedId, collaboratorHashedId);

            if (dbFormAnswer.replied)
            {
                return this.RedirectToAction("AlreadyReplied");
            }

            // Set Checked Form Answer Id
            questionsAnswers.ToList().ForEach(qa =>
            {
                qa.FormAnswerId = dbFormAnswer.id;
            });

            // Add Answers
            IList<answer> answers = AnswerMap.ModelToEntitieaMap(questionsAnswers);
            this.answerService.AddAnswers(answers);

            // Close Form Answer
            dbFormAnswer.replied = true;
            dbFormAnswer.last_update = DateTime.Now;
            this.formAnswerService.UpdateFormAnswer(dbFormAnswer);

            //Mail Administrator
            this.mailService.SendFormResultToAdministrator(dbFormAnswer, answers);

            return this.RedirectToAction("Thanks");
        }

        /// <summary>
        /// Checks the form data integrity.
        /// </summary>
        /// <param name="dbForm">The database form.</param>
        /// <param name="passwordHashed">The password hashed.</param>
        /// <param name="formAnswerHashedId">The form answer hashed identifier.</param>
        /// <param name="collaboratorHashedId">The collaborator hashed identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidCastException">
        /// Error while deconding form model - Don't cheat please.
        /// or
        /// Error while deconding form model - Don't cheat please.
        /// or
        /// Error while deconding form model - Don't cheat please.
        /// </exception>
        private static form_answer CheckFormDataIntegrity(form dbForm, string passwordHashed, string formAnswerHashedId,
                                                          string collaboratorHashedId)
        {
            if (dbForm == null || !dbForm.form_answer.Any())
            {
                throw new InvalidCredentialException("Error while deconding form model - Don't cheat please.");
            }

            form_answer dbFormAnswer = dbForm.form_answer.OrderByDescending(fa => fa.last_update).First();

            if (dbFormAnswer == null || dbFormAnswer.password != passwordHashed)
            {
                throw new InvalidCredentialException("Error while deconding form model - Don't cheat please.");
            }

            collaborator dbCollaborator = dbFormAnswer.collaborator;

            if (formAnswerHashedId != ShaHashPassword.GetSha256ResultString(dbFormAnswer.id.ToString()) ||
                collaboratorHashedId != ShaHashPassword.GetSha256ResultString(dbCollaborator.id.ToString()))
            {
                throw new InvalidCredentialException("Error while deconding form model - Don't cheat please.");
            }

            return dbFormAnswer;
        }

        /// <summary>
        /// Processes the custom form.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="questionsAnswers">The questions answers.</param>
        /// <param name="collaboratorHashedId">The collaborator hashed identifier.</param>
        /// <param name="formAnswerHashedId">The form answer hashed identifier.</param>
        /// <param name="formId">The form identifier.</param>
        private static void ProcessCustomForm(FormCollection model, out IList<AnswerModel> questionsAnswers, out string collaboratorHashedId, 
                                                out string formAnswerHashedId, out string passwordHashed, out int formId)
        {
            questionsAnswers  = new List<AnswerModel>();
            collaboratorHashedId = string.Empty;
            formAnswerHashedId = string.Empty;
            passwordHashed = string.Empty;
            formId = 0;

            foreach (var k in model.AllKeys)
            {
                switch (k)
                {
                    case AppConstants.CollaboratorHashedIdIdentifier:
                        collaboratorHashedId = model[k];
                        break;
                    case AppConstants.FormAnswerHashedIdIdentifier:
                        formAnswerHashedId = model[k];
                        break;
                    case AppConstants.FormIdIdentifier:
                        formId = int.Parse(model[k]);
                        break;
                    case AppConstants.PasswordIdentifier:
                        passwordHashed = model[k];
                        break;
                }

                if (k.StartsWith(AppConstants.QuestionIdPrefix))
                {
                    AnswerModel questionAnswer = new AnswerModel
                    {
                        QuestionId = int.Parse(k.Replace(AppConstants.QuestionIdPrefix, string.Empty)),
                        AnswerValue = model[k]
                    };
                    questionsAnswers.Add(questionAnswer);
                }
            }
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
                Questions = QuestionMap.MapItems(dbForm.questions.OrderByDescending(q => q.order))
            };
        }
    }
}