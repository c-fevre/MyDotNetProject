using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyContractsGenerator.Common;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.Common.PasswordHelper;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesServices;
using MyContractsGenerator.WebUI.Mapping;
using MyContractsGenerator.WebUI.Models.AdministratorModels;
using MyContractsGenerator.WebUI.Models.NotificationModels;

namespace MyContractsGenerator.WebUI.Controllers
{
    [Authorize]
    public class AdministratorController : BaseController
    {
        /// <summary>
        ///     The administrator service
        /// </summary>
        private readonly IAdministratorService administratorService;

        /// <summary>
        ///     The administrator service
        /// </summary>
        private readonly IMailService mailService;

        /// <summary>
        /// The current administrator identifier
        /// </summary>
        private readonly int currentAdministratorId;

        /// <summary>
        /// The current organization identifier
        /// </summary>
        private readonly int currentOrganizationId;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AdministratorController" /> class.
        /// </summary>
        /// <param name="administratorService">The administrator service.</param>
        /// <param name="mailService">The mail service.</param>
        public AdministratorController(IAdministratorService administratorService, IMailService mailService)
        {
            this.administratorService = administratorService;
            this.mailService = mailService;

            this.currentAdministratorId = int.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId());
            this.currentOrganizationId = administratorService.GetAdministratorById(this.currentAdministratorId).organization_id;
        }

        /// <summary>
        ///     GET Index
        /// </summary>
        [HttpGet]
        [Authorize(Roles = AppConstants.SuperAdminRoleLabel)]
        public ActionResult Index()
        {
            AdministratorMainModel model = new AdministratorMainModel();
            this.PopulateAdministratorMainModel(model);

            //display a notification if an administrator has been deleted
            if (this.TempData["DeleteAdministratorId"] == null || (int) this.TempData["DeleteAdministratorId"] <= 0)
            {
                return this.View(model);
            }

            administrator deleteAdministrator =
                this.administratorService.GetAdministratorById((int) this.TempData["DeleteAdministratorId"]);
            NotificationModel notificationModel = new NotificationModel
            {
                Title =
                    string.Format(Resources.Administrator_RemoveNotification_Message, deleteAdministrator.lastname,
                                  deleteAdministrator.firstname)
            };
            model.Notifications.Add(notificationModel);

            return this.View(model);
        }

        /// <summary>
        ///     GET Edit
        /// </summary>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            //select by default the element of the list
            if (id == 0)
            {
                administrator defaultSelectedAdministrator =
                    this.administratorService.GetAdministratorById(int.Parse(this.User.Identity.GetUserId()));
                return this.RedirectToAction("Edit", new { defaultSelectedAdministrator.id });
            }

            AdministratorMainModel model = new AdministratorMainModel();
            this.PopulateAdministratorMainModel(model);

            administrator dbAdmin;
            if (!this.User.IsInRole(AppConstants.SuperAdminRoleLabel))
            {
                dbAdmin = this.administratorService.GetAdministratorById(this.currentAdministratorId);
                model.EditedAdministrator = AdministratorMap.MapItem(dbAdmin);

                return this.View("MyProfile", model);
            }

            dbAdmin = this.administratorService.GetAdministratorById(id);
            model.EditedAdministrator = AdministratorMap.MapItem(dbAdmin);

            //display a notification if an administrator has been deleted
            if (this.TempData["AdministratorCreated"] != null && (bool) this.TempData["AdministratorCreated"])
            {
                this.PushNotification(model,
                                      string.Format(Resources.Administrator_Added,
                                                    $"{dbAdmin.firstname} {dbAdmin.lastname}"),
                                      "success");
            }
            else if (this.TempData["NewAdministratorPasswordGenerated"] != null)
            {
                administrator dbAdministrator =
                    this.administratorService.GetAdministratorById(
                        (int) this.TempData["NewAdministratorPasswordGenerated"]);
                this.PushNotification(model,
                                      string.Format(Resources.Administrator_NewPasswordGenerated,
                                                    $"{dbAdministrator.firstname} {dbAdministrator.lastname}"),
                                      "success");
            }

            return this.View("Edit", model);
        }

        /// <summary>
        ///     POST Edit
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyProfile(AdministratorMainModel model)
        {
            Requires.ArgumentNotNull(model, "model");
            if (!this.ModelState.IsValid)
            {
                return this.ErrorOnEdit(model);
            }

            administrator existingAdministrator =
                this.administratorService.GetAdministratorById(model.EditedAdministrator.Id);
            if (existingAdministrator == null)
            {
                return this.ErrorOnEdit(model);
            }

            //Verify if the email is already used
            if (this.administratorService.IsThisEmailAlreadyExists(model.EditedAdministrator.Email,
                                                                   model.EditedAdministrator.Id))
            {
                this.ModelState.AddModelError("EditedAdministrator.Email",
                                              Resources.Administrator_ErrorIncorrectEmailAlreadyUsed);
                return this.ErrorOnEdit(model);
            }

            existingAdministrator.email = model.EditedAdministrator.Email;
            existingAdministrator.firstname = model.EditedAdministrator.FirstName;
            existingAdministrator.lastname = model.EditedAdministrator.LastName;

            //Check infos
            ActionResult actionResult;
            if (this.CheckAdministratorInformations(model, existingAdministrator, out actionResult))
            {
                return actionResult;
            }

            this.administratorService.Update(existingAdministrator, this.currentOrganizationId);
            this.PopulateAdministratorMainModel(model);

            if (model.EditedAdministrator.NewPassword != null &&
                model.EditedAdministrator.NewPasswordConfirmation != null)
            {
                this.mailService.SendPasswordChangedAdministrator(existingAdministrator,
                                                                  model.EditedAdministrator.NewPassword);
                if (existingAdministrator.id == this.CurrentUserId)
                {
                    this.HttpContext.GetOwinContext().Authentication.SignOut(
                        DefaultAuthenticationTypes.ApplicationCookie);
                }
            }

            this.PushNotification(model,
                                  string.Format(Resources.Administrator_Edited,
                                                $"{existingAdministrator.firstname} {existingAdministrator.lastname}"),
                                  "success");

            return this.View("MyProfile", model);
        }

        /// <summary>
        ///     POST Edit
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdministratorMainModel model)
        {
            Requires.ArgumentNotNull(model, "model");
            if (!this.ModelState.IsValid)
            {
                return this.ErrorOnEdit(model);
            }

            administrator existingAdministrator =
                this.administratorService.GetAdministratorById(model.EditedAdministrator.Id);
            if (existingAdministrator == null)
            {
                return this.ErrorOnEdit(model);
            }

            //Verify if the email is already used
            if (this.administratorService.IsThisEmailAlreadyExists(model.EditedAdministrator.Email,
                                                                   model.EditedAdministrator.Id))
            {
                this.ModelState.AddModelError("EditedAdministrator.Email",
                                              Resources.Administrator_ErrorIncorrectEmailAlreadyUsed);
                return this.ErrorOnEdit(model);
            }

            existingAdministrator.email = model.EditedAdministrator.Email;
            existingAdministrator.firstname = model.EditedAdministrator.FirstName;
            existingAdministrator.lastname = model.EditedAdministrator.LastName;

            //Check infos
            ActionResult actionResult;
            if (this.CheckAdministratorInformations(model, existingAdministrator, out actionResult))
            {
                return actionResult;
            }

            this.administratorService.Update(existingAdministrator, this.currentOrganizationId);
            this.PopulateAdministratorMainModel(model);

            if (model.EditedAdministrator.NewPassword != null &&
                model.EditedAdministrator.NewPasswordConfirmation != null)
            {
                this.mailService.SendPasswordChangedAdministrator(existingAdministrator,
                                                                  model.EditedAdministrator.NewPassword);
                if (existingAdministrator.id == this.CurrentUserId)
                {
                    this.HttpContext.GetOwinContext().Authentication.SignOut(
                        DefaultAuthenticationTypes.ApplicationCookie);
                }
            }

            this.PushNotification(model,
                                  string.Format(Resources.Administrator_Edited,
                                                $"{existingAdministrator.firstname} {existingAdministrator.lastname}"),
                                  "success");

            return this.View(model);
        }

        /// <summary>
        ///     Checks the administrator informations.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="existingAdministrator">The existing administrator.</param>
        /// <param name="actionResult">The action result.</param>
        /// <returns></returns>
        private bool CheckAdministratorInformations(AdministratorMainModel model, administrator existingAdministrator,
                                                    out ActionResult actionResult)
        {
            actionResult = null;

            if (model.EditedAdministrator.NewPassword != null &&
                model.EditedAdministrator.NewPasswordConfirmation != null &&
                model.EditedAdministrator.NewPassword != model.EditedAdministrator.NewPasswordConfirmation)
            {
                this.ModelState.AddModelError("EditedAdministrator.NewPasswordConfirmation",
                                              Resources.Administrator_ErrorIncorrectPasswordConfirmation);
                {
                    actionResult = this.ErrorOnEdit(model);
                    return true;
                }
            }

            if (model.EditedAdministrator.CurrentPassword != null &&
                ShaHashPassword.GetSha256ResultString(model.EditedAdministrator.CurrentPassword) !=
                existingAdministrator.password)
            {
                model.EditedAdministrator.NewPassword = null;
                model.EditedAdministrator.NewPasswordConfirmation = null;

                this.ModelState.AddModelError("EditedAdministrator.CurrentPassword",
                                              Resources.Administrator_ErrorIncorrectPassword);
                {
                    actionResult = this.ErrorOnEdit(model);
                    return true;
                }
            }

            if (model.EditedAdministrator.NewPassword == null ||
                model.EditedAdministrator.NewPasswordConfirmation == null)
            {
                return false;
            }

            if (existingAdministrator.password ==
                ShaHashPassword.GetSha256ResultString(model.EditedAdministrator.NewPassword))
            {
                this.ModelState.AddModelError("EditedAdministrator.NewPasswordConfirmation",
                                              Resources.Administrator_ErrorOldAndNewPasswordAreTheSame);
                {
                    actionResult = this.ErrorOnEdit(model);
                    return true;
                }
            }

            existingAdministrator.password =
                ShaHashPassword.GetSha256ResultString(model.EditedAdministrator.NewPassword);

            return false;
        }

        /// <summary>
        ///     Create
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = AppConstants.SuperAdminRoleLabel)]
        public ActionResult Create()
        {
            AdministratorMainModel model = new AdministratorMainModel();
            this.PopulateAdministratorMainModel(model);
            model.EditedAdministrator = new AdministratorModel();

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppConstants.SuperAdminRoleLabel)]
        public ActionResult GenerateNewPassword(AdministratorMainModel model)
        {
            Requires.ArgumentNotNull(model, "model");
            Requires.ArgumentNotNull(model.EditedAdministrator.Id, "administratorId");

            this.administratorService.ResetPassword(model.EditedAdministrator.Id, this.currentOrganizationId);

            this.TempData["NewAdministratorPasswordGenerated"] = model.EditedAdministrator.Id;

            return this.RedirectToAction("Edit", new { model.EditedAdministrator.Id });
        }

        /// <summary>
        ///     POST Create
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdministratorMainModel model)
        {
            Requires.ArgumentNotNull(model, "model");

            if (!this.ModelState.IsValid)
            {
                return this.ErrorOnEdit(model);
            }

            //Verify if the email is already used
            if (this.administratorService.IsThisEmailAlreadyExists(model.EditedAdministrator.Email, this.currentOrganizationId))
            {
                this.ModelState.AddModelError("EditedAdministrator.Email",
                                              Resources.Administrator_ErrorIncorrectEmailAlreadyUsed);
                return this.ErrorOnEdit(model);
            }

            string password = PasswordGenerator.GeneratePassword(8, 4);
            administrator newAdministrator = new administrator
            {
                email = model.EditedAdministrator.Email,
                firstname = model.EditedAdministrator.FirstName,
                lastname = model.EditedAdministrator.LastName,
                password = ShaHashPassword.GetSha256ResultString(password),
                active = true,
                is_super_admin = false,
                organization_id = 0
            };

            administrator dbAdmin = this.administratorService.Add(newAdministrator, this.currentOrganizationId);

            this.PopulateAdministratorMainModel(model);

            this.mailService.SendWelcomeNewAdministrator(dbAdmin, password);

            this.TempData["AdministratorCreated"] = true;

            return this.RedirectToAction("Edit", "Administrator", new { dbAdmin.id });
        }

        /// <summary>
        ///     Remove the administrator
        /// </summary>
        /// <param name="id">administrator to remove</param>
        [HttpGet]
        [Authorize(Roles = AppConstants.SuperAdminRoleLabel)]
        public ActionResult Remove(int id)
        {
            Requires.ArgumentGreaterThanZero(id, "administrator id");
            this.administratorService.Delete(id);

            this.TempData["DeleteAdministratorId"] = id;

            return this.RedirectToAction("Index");
        }

        /// <summary>
        ///     Populate AdministratorMainModel
        /// </summary>
        private void PopulateAdministratorMainModel(AdministratorMainModel model)
        {
            //Populate the active administrators
            IList<administrator> administrators = this.administratorService.GetActiveAdministrators(this.currentOrganizationId);

            model.Administrators = AdministratorMap.MapItems(administrators);
        }

        /// <summary>
        ///     Repopulate the model and return to the edit view when a validation error occurs
        /// </summary>
        private ActionResult ErrorOnEdit(AdministratorMainModel model)
        {
            this.PopulateAdministratorMainModel(model);
            return this.View(model);
        }

        /// <summary>
        ///     Custom push notification (partial view)
        /// </summary>
        private void PushNotification(AdministratorMainModel model, string message, string type)
        {
            NotificationModel notificationModel = new NotificationModel
            {
                Title = message,
                Type = type,
                Delay = 10000
            };
            model.Notifications.Add(notificationModel);
        }
    }
}