﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyContractsGenerator.Common;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.Common.PasswordHelper;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesServices;
using MyContractsGenerator.WebUI.Mapping;
using MyContractsGenerator.WebUI.Models.FormModels;
using MyContractsGenerator.WebUI.Models.NotificationModels;
using WebGrease.Css.Extensions;

namespace MyContractsGenerator.WebUI.Controllers
{
    [Authorize]
    public class FormController : BaseController
    {
        /// <summary>
        ///     The collaborator service
        /// </summary>
        private readonly ICollaboratorService collaboratorService;

        /// <summary>
        /// The administrator service
        /// </summary>
        private readonly IAdministratorService administratorService;

        /// <summary>
        ///     The collaborator service
        /// </summary>
        private readonly IFormAnswerService formAnswerService;

        /// <summary>
        ///     The mail service
        /// </summary>
        private readonly IMailService mailService;

        /// <summary>
        ///     The question service
        /// </summary>
        private readonly IQuestionService questionService;

        /// <summary>
        ///     The role service
        /// </summary>
        private readonly IRoleService roleService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FormController" /> class.
        /// </summary>
        /// <param name="collaboratorService">The collaborator service.</param>
        /// <param name="roleService">The role service.</param>
        /// <param name="formAnswerService">The form answer service.</param>
        /// <param name="questionService">The question service.</param>
        /// <param name="mailService">The mail service.</param>
        /// <param name="administratorService"></param>
        public FormController(ICollaboratorService collaboratorService, IRoleService roleService,
                              IFormAnswerService formAnswerService, IQuestionService questionService,
                              IMailService mailService, IAdministratorService administratorService) : base(administratorService)
        {
            this.collaboratorService = collaboratorService;
            this.roleService = roleService;
            this.formAnswerService = formAnswerService;
            this.mailService = mailService;
            this.questionService = questionService;
            this.administratorService = administratorService;
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

            collaborator collab = this.collaboratorService.GetById((int) this.TempData["MailedCollaboratorId"], this.CurrentOrganizationId);
            NotificationModel notificationModel = new NotificationModel
            {
                Title =
                    string.Format(Resources.Form_MailedSentToCollaborator, collab.lastname,
                                  collab.firstname)
            };
            model.Notifications.Add(notificationModel);

            return this.View(model);
        }

        /// <summary>
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="collaboratorId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ReSend(int collaboratorId, int roleId)
        {
            collaborator mailTarget = this.collaboratorService.GetById(collaboratorId, this.CurrentOrganizationId);
            form_answer lastFormAnswer =
                mailTarget.form_answer.Where(fa => !fa.replied).FirstOrDefault(fa => fa.role.id.Equals(roleId));

            if (lastFormAnswer != null)
            {
                string formUrl =
                    $"{GlobalAppSettings.ApplicationBaseUrl}{this.Url.Action("WhoAreYou", "CollaboratorForm", new { c = ShaHashPassword.GetSha256ResultString(mailTarget.id.ToString()), fa = ShaHashPassword.GetSha256ResultString(lastFormAnswer.id.ToString()) })}";

                lastFormAnswer.password = PasswordGenerator.GeneratePassword(8, 4);
                this.mailService.SendFormToCollaborator(mailTarget, formUrl, this.CurrentOrganizationId, lastFormAnswer.password,
                                                        lastFormAnswer.last_collaborator_mail_time);

                lastFormAnswer.password = ShaHashPassword.GetSha256ResultString(lastFormAnswer.password);
                lastFormAnswer.last_update = DateTime.Now;
                lastFormAnswer.last_collaborator_mail_time = DateTime.Now;
                this.formAnswerService.UpdateFormAnswer(lastFormAnswer, this.CurrentOrganizationId);
            }

            this.TempData["MailedCollaboratorId"] = collaboratorId;

            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// </summary>
        /// <param name="collaboratorId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Send(int collaboratorId, int roleId)
        {
            administrator dbAdministrator = this.administratorService.GetAdministratorById(this.CurrentOrganizationId);

            collaborator mailTarget = this.collaboratorService.GetById(collaboratorId, this.CurrentOrganizationId);
            role linkedRole = this.roleService.GetById(roleId, this.CurrentOrganizationId);

            string tempPassword = PasswordGenerator.GeneratePassword(8, 4);

            // TODO Dynamic forms
            form_answer newFormAnswer = new form_answer
            {
                organization_id = dbAdministrator.organization_id,
                collaborator = mailTarget,
                role = linkedRole,
                last_update = DateTime.Now,
                last_collaborator_mail_time = DateTime.Now,
                replied = false,
                form_id = 1,
                password = ShaHashPassword.GetSha256ResultString(tempPassword)
            };

            form_answer dbFormAnswer = this.formAnswerService.AddFormAnswer(newFormAnswer, this.CurrentOrganizationId);

            string formUrl =
                $"{GlobalAppSettings.ApplicationBaseUrl}{this.Url.Action("WhoAreYou", "CollaboratorForm", new { fa = ShaHashPassword.GetSha256ResultString(dbFormAnswer.id.ToString()) })}";

            this.mailService.SendFormToCollaborator(mailTarget, formUrl, this.CurrentOrganizationId, tempPassword, DateTime.MinValue);

            this.TempData["MailedCollaboratorId"] = collaboratorId;

            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// </summary>
        /// <param name="collaboratorId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CollaboratorAnswers(int collaboratorId, int roleId)
        {
            AnswersModel model = new AnswersModel();
            this.PopulateAnswersModel(model, collaboratorId, roleId);

            return this.View(model);
        }

        /// <summary>
        ///     Populates the answers model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="collaboratorId">The collaborator identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        private void PopulateAnswersModel(AnswersModel model, int collaboratorId, int roleId)
        {
            collaborator collab = this.collaboratorService.GetById(collaboratorId, this.CurrentOrganizationId);
            role role = this.roleService.GetById(roleId, this.CurrentOrganizationId);
            IList<form_answer> collabFormAnswers = this.formAnswerService.GetAllForCollaboratorAndRole(collaboratorId,
                                                                                                       roleId, this.CurrentOrganizationId);

            model.Collaborator = CollaboratorMap.MapItem(collab);
            model.FormAnswers = FormAnswerMap.MapItems(collabFormAnswers);
            model.Role = RoleMap.MapItem(role);
        }

        /// <summary>
        ///     Populates the form main model.
        /// </summary>
        /// <param name="model">The model.</param>
        private void PopulateFormMainModel(FormMainModel model)
        {
            IList<role> roles = this.roleService.GetAllActive(this.CurrentOrganizationId);
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

                    mailingModel.Collaborators.ToList().ForEach(c =>
                    {
                        c.FormAnswers =
                            FormAnswerMap.MapItems(r.form_answer.Where(fa => fa.collaborator_id.Equals(c.Id)));
                    });

                    model.RolesWithCollaborators.Add(mailingModel);
                });
            }
        }
    }
}