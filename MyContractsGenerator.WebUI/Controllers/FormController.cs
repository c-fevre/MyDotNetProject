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
        /// The role service
        /// </summary>
        private readonly IRoleService roleService;

        /// <summary>
        /// The mail service
        /// </summary>
        private readonly IMailService mailService;

        public FormController(ICollaboratorService collaboratorService, IRoleService roleService, IMailService mailService)
        {
            this.collaboratorService = collaboratorService;
            this.roleService = roleService;
            this.mailService = mailService;
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

            collaborator collab = this.collaboratorService.GetById((int) this.TempData["MailedCollaboratorId"]);
            NotificationModel notificationModel = new NotificationModel
            {
                Title =
                    string.Format(Resources.Form_MailedSentToCollaborator, collab.firstname,
                                  collab.lastname)
            };
            model.Notifications.Add(notificationModel);

            return this.View(model);
        }

        [HttpGet]
        public ActionResult Send(int collaboratorId, string formUrl)
        {
            int adminId = Convert.ToInt32(this.User.Identity.GetUserId());
            collaborator mailTarget = this.collaboratorService.GetById(collaboratorId);

            this.mailService.SendFormToCollaborator(mailTarget, formUrl, adminId);
            this.TempData["MailedCollaboratorId"] = collaboratorId;

            return this.RedirectToAction("Index");
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

                    mailingModel.Collaborators.ForEach(collab =>
                    {
                        collab.FormUrl =
                            $"{GlobalAppSettings.ApplicationBaseUrl}{this.Url.Action("WhoAreYou", "CollaboratorForm", new { c = ShaHashPassword.GetSha256ResultString(collab.Email) })}";
                    });

                    model.RolesWithCollaborators.Add(mailingModel);
                });


            }
        }
    }
}