using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyContractsGenerator.Common;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.Common.PasswordHelper;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.Core.Exceptions;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesServices;
using MyContractsGenerator.WebUI.Mapping;
using MyContractsGenerator.WebUI.Models.AnswerModels;
using MyContractsGenerator.WebUI.Models.CollaboratorFormModels;

namespace MyContractsGenerator.WebUI.Controllers
{
    /// <summary>
    ///     TODO Check security ...
    /// </summary>
    /// <seealso cref="MyContractsGenerator.WebUI.Controllers.BaseController" />
    [Authorize]
    public class CollaboratorFormController : BaseController
    {
        /// <summary>
        ///     The answer service
        /// </summary>
        private readonly IAnswerService answerService;

        /// <summary>
        /// The organization service
        /// </summary>
        private readonly IOrganizationService organizationService;

        /// <summary>
        ///     The collaborator service
        /// </summary>
        private readonly ICollaboratorService collaboratorService;

        /// <summary>
        ///     The form answer service
        /// </summary>
        private readonly IFormAnswerService formAnswerService;

        /// <summary>
        ///     The form answer service
        /// </summary>
        private readonly IFormService formService;

        /// <summary>
        ///     The mail service
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
        /// <param name="administratorService">The administrator service.</param>
        /// <param name="organizationService">The organization service.</param>
        /// <param name="mailService">The mail service.</param>
        public CollaboratorFormController(ICollaboratorService collaboratorService, IRoleService roleService,
                                          IFormService formService, IFormAnswerService formAnswerService,
                                          IAnswerService answerService,
                                          IOrganizationService organizationService,
                                          IMailService mailService)
        {
            this.collaboratorService = collaboratorService;
            this.formAnswerService = formAnswerService;
            this.formService = formService;
            this.answerService = answerService;
            this.organizationService = organizationService;
            this.mailService = mailService;
        }

        /// <summary>
        /// Whoes the are you.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="fa">The fa.</param>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult WhoAreYou(string fa)
        {
            CollaboratorFormMainModel model = new CollaboratorFormMainModel { fa = fa };

            return this.View(model);
        }

        /// <summary>
        /// Thankses this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Thanks()
        {
            return this.View();
        }

        /// <summary>
        /// Alreadies the replied.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult AlreadyReplied()
        {
            return this.View();
        }

        /// <summary>
        /// Checkins the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Checkin(CollaboratorFormMainModel model)
        {
            // model validation
            Requires.ArgumentNotNull(model, "model");

            if (!this.ModelState.IsValid)
            {
                return this.View("WhoAreYou", model);
            }

            // TODO Revoir process
            form_answer lastFormAnswer = this.formAnswerService.GetAllActive().SingleOrDefault(
                                                                fa =>
                                                                    ShaHashPassword.GetSha256ResultString(
                                                                        fa.id.ToString()).Equals(
                                                                        model.fa));

            if (lastFormAnswer == null)
            {
                this.ModelState.AddModelError(string.Empty,
                                              Resources.CollaboratorForm_Error);
                return this.View("WhoAreYou", model);
            }

            if (model.fa == ShaHashPassword.GetSha256ResultString(lastFormAnswer.id.ToString()))
            {
                model.Password = ShaHashPassword.GetSha256ResultString(model.Password);

                if (lastFormAnswer.password == model.Password)
                {
                    return this.RedirectToAction("YourForm", model);
                }

                this.ModelState.AddModelError(string.Empty,
                                              Resources.CollaboratorForm_Error);
                return this.View("WhoAreYou", model);
            }

            this.ModelState.AddModelError(string.Empty,
                                          Resources.CollaboratorForm_Error);
            return this.View("WhoAreYou", model);
        }

        /// <summary>
        ///     Yours the form.
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

            form_answer lastFormAnswer = this.formAnswerService.GetAllActive().SingleOrDefault(
                                                                fa =>
                                                                    ShaHashPassword.GetSha256ResultString(
                                                                        fa.id.ToString()).Equals(
                                                                        model.fa));

            if (lastFormAnswer == null)
            {
                this.ModelState.AddModelError(string.Empty,
                                              Resources.CollaboratorForm_Error);
                return this.View("WhoAreYou", model);
            }

            if (model.fa == ShaHashPassword.GetSha256ResultString(lastFormAnswer.id.ToString()))
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
        [ValidateAntiForgeryToken]
        public ActionResult CheckForm(FormCollection model)
        {
            if (model == null || !model.HasKeys())
            {
                throw new InvalidCastException("Error while deconding form model - Don't cheat please.");
            }
            
            string formAnswerHashedId;
            string passwordHashed;
            int formId;
            IList<AnswerModel> questionsAnswers;
            ProcessCustomForm(model, out questionsAnswers, out formAnswerHashedId, out passwordHashed, out formId);

            form dbForm = this.formService.GetById(formId);

            var dbFormAnswer = CheckFormDataIntegrity(dbForm, passwordHashed, formAnswerHashedId);

            if (dbFormAnswer.replied)
            {
                return this.RedirectToAction("AlreadyReplied");
            }

            // Set Checked Form Answer Id
            questionsAnswers.ToList().ForEach(qa => { qa.FormAnswerId = dbFormAnswer.id; });

            // Add Answers
            IList<answer> answers = AnswerMap.ModelToEntitieaMap(questionsAnswers);
            this.answerService.AddAnswers(answers);

            // Close Form Answer
            dbFormAnswer.replied = true;
            dbFormAnswer.last_update = DateTime.Now;
            this.formAnswerService.UpdateFormAnswer(dbFormAnswer, dbFormAnswer.organization.id);

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
        /// <exception cref="InvalidCredentialException">
        /// Error while deconding form model - Don't cheat please.
        /// or
        /// Error while deconding form model - Don't cheat please.
        /// or
        /// Error while deconding form model - Don't cheat please.
        /// </exception>
        private form_answer CheckFormDataIntegrity(form dbForm, string passwordHashed, string formAnswerHashedId)
        {
            if (dbForm == null || !dbForm.form_answer.Any())
            {
                throw new BusinessException("Error while deconding form model - Don't cheat please.");
            }

            form_answer dbFormAnswer = dbForm.form_answer.Where(fa => !fa.replied)
                                             .SingleOrDefault(
                                                 fa =>
                                                     ShaHashPassword.GetSha256ResultString(fa.id.ToString()).Equals(
                                                         formAnswerHashedId));

            if (dbFormAnswer == null || dbFormAnswer.password != passwordHashed)
            {
                throw new BusinessException("Error while deconding form model - Don't cheat please.");
            }

            if (formAnswerHashedId != ShaHashPassword.GetSha256ResultString(dbFormAnswer.id.ToString()))
            {
                throw new BusinessException("Error while deconding form model - Don't cheat please.");
            }

            dbFormAnswer.organization.administrators = this.organizationService.GetById(dbFormAnswer.organization.id).administrators;

            return dbFormAnswer;
        }

        /// <summary>
        /// Processes the custom form.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="questionsAnswers">The questions answers.</param>
        /// <param name="formAnswerHashedId">The form answer hashed identifier.</param>
        /// <param name="passwordHashed">The password hashed.</param>
        /// <param name="formId">The form identifier.</param>
        private static void ProcessCustomForm(FormCollection model, out IList<AnswerModel> questionsAnswers,
                                              out string formAnswerHashedId, out string passwordHashed, out int formId)
        {
            questionsAnswers = new List<AnswerModel>();
            formAnswerHashedId = string.Empty;
            passwordHashed = string.Empty;
            formId = 0;

            foreach (var k in model.AllKeys)
            {
                switch (k)
                {
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

                if (!k.StartsWith(AppConstants.QuestionIdPrefix))
                {
                    continue;
                }

                AnswerModel questionAnswer = new AnswerModel
                {
                    QuestionId = int.Parse(k.Replace(AppConstants.QuestionIdPrefix, string.Empty)),
                    AnswerValue = model[k]
                };
                questionsAnswers.Add(questionAnswer);
            }
        }

        /// <summary>
        ///     Populates the form main model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="lastFormAnswer">The last form answer.</param>
        private void PopulateFormMainModel(CollaboratorFormMainModel model, form_answer lastFormAnswer)
        {
            form dbForm = this.formService.GetById(lastFormAnswer.form_id);
            collaborator dbCollaborator = this.collaboratorService.GetByEmail(model.Email, lastFormAnswer.organization_id);

            model.Form = new CollaboratorFormModel
            {
                Label = dbForm.label,
                Id = dbForm.id,
                Collaborator = CollaboratorMap.MapItem(dbCollaborator),
                Role = RoleMap.MapItem(lastFormAnswer.role),
                Questions = QuestionMap.MapItems(dbForm.questions.OrderByDescending(q => q.order))
            };
        }
    }
}