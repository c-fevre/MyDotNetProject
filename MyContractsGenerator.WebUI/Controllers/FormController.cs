using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyContractsGenerator.Common;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.Common.PasswordHelper;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesServices;
using MyContractsGenerator.WebUI.Mapping;
using MyContractsGenerator.WebUI.Models.FormAnswerModels;
using MyContractsGenerator.WebUI.Models.FormModels;
using MyContractsGenerator.WebUI.Models.NotificationModels;
using WebGrease.Css.Extensions;

namespace MyContractsGenerator.WebUI.Controllers
{
    [Authorize]
    public class FormController : BaseController
    {
        /// <summary>
        /// The collaborator service
        /// </summary>
        private readonly ICollaboratorService collaboratorService;

        /// <summary>
        /// The collaborator service
        /// </summary>
        private readonly IFormAnswerService formAnswerService;

        /// <summary>
        /// The role service
        /// </summary>
        private readonly IRoleService roleService;

        /// <summary>
        /// The question service
        /// </summary>
        private readonly IQuestionService questionService;

        /// <summary>
        /// The mail service
        /// </summary>
        private readonly IMailService mailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormController"/> class.
        /// </summary>
        /// <param name="collaboratorService">The collaborator service.</param>
        /// <param name="roleService">The role service.</param>
        /// <param name="formAnswerService">The form answer service.</param>
        /// <param name="questionService">The question service.</param>
        /// <param name="mailService">The mail service.</param>
        public FormController(ICollaboratorService collaboratorService, IRoleService roleService, IFormAnswerService formAnswerService, IQuestionService questionService, IMailService mailService)
        {
            this.collaboratorService = collaboratorService;
            this.roleService = roleService;
            this.formAnswerService = formAnswerService;
            this.mailService = mailService;
            this.questionService = questionService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            FormMainModel model = new FormMainModel();
            this.PopulateFormMainModel(model);

            if (this.TempData["MailedCollaboratorId"] == null)
            {
                return this.View(model);
            }

            collaborator collab = this.collaboratorService.GetById((int)this.TempData["MailedCollaboratorId"]);
            NotificationModel notificationModel = new NotificationModel
            {
                Title =
                    string.Format(Resources.Form_MailedSentToCollaborator, collab.firstname,
                                  collab.lastname)
            };
            model.Notifications.Add(notificationModel);

            return this.View(model);
        }

        /// <summary>
        /// </summary>
        /// <param name="collaboratorId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ReSend(int collaboratorId)
        {
            int adminId = Convert.ToInt32(this.User.Identity.GetUserId());
            collaborator mailTarget = this.collaboratorService.GetById(collaboratorId);
            form_answer lastFormAnswer = mailTarget.form_answer.ToList().Where(fa => !fa.replied).OrderByDescending(fa => fa.last_update).FirstOrDefault();

            if (lastFormAnswer != null)
            {
                string formUrl =
                        $"{GlobalAppSettings.ApplicationBaseUrl}{this.Url.Action("WhoAreYou", "CollaboratorForm", new { c = ShaHashPassword.GetSha256ResultString(mailTarget.id.ToString()), fa = ShaHashPassword.GetSha256ResultString(lastFormAnswer.id.ToString()) })}";

                lastFormAnswer.password = RandomString(10);
                this.mailService.SendFormToCollaborator(mailTarget, formUrl, adminId, lastFormAnswer.password,lastFormAnswer.last_collaborator_mail_time);

                lastFormAnswer.password = ShaHashPassword.GetSha256ResultString(lastFormAnswer.password);
                lastFormAnswer.last_update = DateTime.Now;
                lastFormAnswer.last_collaborator_mail_time = DateTime.Now;
                this.formAnswerService.UpdateFormAnswer(lastFormAnswer);
            }

            this.TempData["MailedCollaboratorId"] = collaboratorId;

            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// </summary>
        /// <param name="collaboratorId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Send(int collaboratorId)
        {
            int adminId = Convert.ToInt32(this.User.Identity.GetUserId());
            collaborator mailTarget = this.collaboratorService.GetById(collaboratorId);
            string tempPassword = RandomString(10);

            // TODO Check information, Dynamic forms
            // TODO Link FormAnswer to Role AND collaborator not only collaborator
            form_answer newFormAnswer = new form_answer
            {
                admin_id = adminId,
                collaborator = mailTarget,
                last_update = DateTime.Now,
                last_collaborator_mail_time = DateTime.Now,
                replied = false,
                form_id = 1,
                password = ShaHashPassword.GetSha256ResultString(tempPassword)
            };

            form_answer dbFormAnswer = this.formAnswerService.AddFormAnswer(newFormAnswer);

            string formUrl =
                    $"{GlobalAppSettings.ApplicationBaseUrl}{this.Url.Action("WhoAreYou", "CollaboratorForm", new { c = ShaHashPassword.GetSha256ResultString(mailTarget.id.ToString()), fa = ShaHashPassword.GetSha256ResultString(dbFormAnswer.id.ToString()) })}";

            this.mailService.SendFormToCollaborator(mailTarget, formUrl, adminId, tempPassword, DateTime.MinValue);
            
            this.TempData["MailedCollaboratorId"] = collaboratorId;

            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// </summary>
        /// <param name="collaboratorId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CollaboratorAnswers(int collaboratorId)
        {
            AnswersModel model = new AnswersModel();
            this.PopulateAnswersModel(model, collaboratorId);

            return this.View(model);
        }

        /// <summary>
        /// Populates the answers model.
        /// </summary>
        /// <param name="model">The model.</param>
        private void PopulateAnswersModel(AnswersModel model, int collaboratorId)
        {
            collaborator collab = this.collaboratorService.GetById(collaboratorId);
            IList<form_answer> collabFormAnswers = this.formAnswerService.GetAllForCollaborator(collaboratorId);

            model.Collaborator = CollaboratorMap.MapItem(collab);
            model.FormAnswers = FormAnswerMap.MapItems(collabFormAnswers);
        }

        /// <summary>
        /// Populates the form main model.
        /// </summary>
        /// <param name="model">The model.</param>
        private void PopulateFormMainModel(FormMainModel model)
        {
            IList<role> roles = this.roleService.GetAllActive();
            model.RolesWithCollaborators = new List<FormMailingModel>();

            if (roles.Any())
            {
                roles.Where(r => r.collaborators.Any()).ForEach(r =>
                {
                    FormMailingModel mailingModel = new FormMailingModel
                    {
                        Role = RoleMap.MapItem(r),
                        Collaborators = CollaboratorMap.MapItems(r.collaborators)
                    };

                    model.RolesWithCollaborators.Add(mailingModel);
                });
            }
        }

        /// <summary>
        /// Randoms the string.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        private static string RandomString(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}