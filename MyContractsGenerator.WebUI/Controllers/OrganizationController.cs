using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesServices;
using MyContractsGenerator.WebUI.Mapping;
using MyContractsGenerator.WebUI.Models.OrganizationModels;
using MyContractsGenerator.WebUI.Models.NotificationModels;

namespace MyContractsGenerator.WebUI.Controllers
{
    [Authorize]
    public class OrganizationController : BaseController
    {
        /// <summary>
        ///     The organization service
        /// </summary>
        private readonly IOrganizationService organizationService;

        /// <summary>
        /// The administrator service
        /// </summary>
        private readonly IAdministratorService administratorService;

        /// <summary>
        /// The current administrator identifier
        /// </summary>
        private readonly int currentAdministratorId;

        /// <summary>
        /// The current organization identifier
        /// </summary>
        private readonly int currentOrganizationId;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationController"/> class.
        /// </summary>
        /// <param name="organizationService">The organization service.</param>
        /// <param name="administratorService">The administrator service.</param>
        public OrganizationController(IOrganizationService organizationService, IAdministratorService administratorService)
        {
            this.organizationService = organizationService;
            this.administratorService = administratorService;

            this.currentAdministratorId = int.Parse(System.Web.HttpContext.Current.User.Identity.GetUserId());
            this.currentOrganizationId = administratorService.GetAdministratorById(this.currentAdministratorId).organization_id;
        }

        /// <summary>
        ///     GET Index
        /// </summary>
        [HttpGet]
        public ActionResult Index()
        {
            OrganizationMainModel model = new OrganizationMainModel();
            this.PopulateOrganizationMainModel(model);

            //display a notification if an administrator has been deleted
            if (this.TempData["DeleteOrganizationId"] == null || (int) this.TempData["DeleteOrganizationId"] <= 0)
            {
                return this.View(model);
            }

            organization deleteOrganization =
                this.organizationService.GetById((int) this.TempData["DeleteOrganizationId"]);
            NotificationModel notificationModel = new NotificationModel
            {
                Title =
                    string.Format(Resources.Organization_RemoveNotification_Message, deleteOrganization.label)
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
                organization defaultSelectedOrganization = this.organizationService.GetAll().FirstOrDefault();
                return defaultSelectedOrganization == null
                    ? this.RedirectToAction("Index")
                    : this.RedirectToAction("Edit", new { defaultSelectedOrganization.id });
            }

            OrganizationMainModel model = new OrganizationMainModel();
            this.PopulateOrganizationMainModel(model);

            organization organization = this.organizationService.GetById(id);
            model.EditedOrganization = OrganizationMap.MapItem(organization);

            //display a notification if an administrator has been deleted
            if (this.TempData["OrganizationCreated"] != null && (bool) this.TempData["OrganizationCreated"])
            {
                this.PushNotification(model,
                                      string.Format(Resources.Organization_Added, organization.label),
                                      "success");
            }

            return this.View(model);
        }

        /// <summary>
        ///     POST Edit
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrganizationMainModel model)
        {
            Requires.ArgumentNotNull(model, "model");
            if (!this.ModelState.IsValid)
            {
                return this.ErrorOnEdit(model);
            }

            organization existingOrganization = this.organizationService.GetById(model.EditedOrganization.Id);
            if (existingOrganization == null)
            {
                return this.ErrorOnEdit(model);
            }

            //Verify if the email is already used
            if (this.organizationService.IsThisLabelAlreadyExists(model.EditedOrganization.Label, model.EditedOrganization.Id))
            {
                this.ModelState.AddModelError("EditedOrganization.Email",
                                              Resources.Administrator_ErrorIncorrectEmailAlreadyUsed);
                return this.ErrorOnEdit(model);
            }

            if (!this.ModelState.IsValid)
            {
                return this.ErrorOnEdit(model);
            }

            /*if (model.EditedOrganization.LinkedRolesIds == null)
            {
                model.EditedOrganization.LinkedRolesIds = new List<int>();
            }*/

            existingOrganization.label = model.EditedOrganization.Label;

            this.organizationService.UpdateOrganization(existingOrganization);

            /*this.roleService.AffectToRole(model.EditedOrganization.LinkedRolesIds,
                                          model.EditedOrganization.Id, this.currentOrganizationId);*/

            this.PopulateOrganizationMainModel(model);

            this.PushNotification(model,
                                  string.Format(Resources.Organization_Edited, existingOrganization.label),
                                  "success");

            return this.View(model);
        }

        /// <summary>
        ///     Create
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            OrganizationMainModel model = new OrganizationMainModel();
            this.PopulateOrganizationMainModel(model);
            model.EditedOrganization = new OrganizationModel();

            return this.View(model);
        }

        /// <summary>
        ///     POST Create
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrganizationMainModel model)
        {
            Requires.ArgumentNotNull(model, "model");

            if (!this.ModelState.IsValid)
            {
                return this.ErrorOnEdit(model);
            }

            //Verify if the email is already used
            if (this.organizationService.IsThisLabelAlreadyExists(model.EditedOrganization.Label))
            {
                this.ModelState.AddModelError("EditedOrganization.Email",
                                              Resources.Administrator_ErrorIncorrectEmailAlreadyUsed);
                return this.ErrorOnEdit(model);
            }

            organization newOrganization = new organization
            {
                label = model.EditedOrganization.Label,
                active = true
            };

            organization dbOrga = this.organizationService.AddOrganization(newOrganization);

            /*this.roleService.AffectToRole(model.EditedOrganization.LinkedRolesIds,
                                          dbOrga.id);*/

            this.PopulateOrganizationMainModel(model);

            this.TempData["OrganizationCreated"] = true;

            return this.RedirectToAction("Edit", "Organization", new { dbOrga.id });
        }

        /// <summary>
        ///     Remove the organization
        /// </summary>
        /// <param name="id">organization to remove</param>
        [HttpGet]
        public ActionResult Remove(int id)
        {
            Requires.ArgumentGreaterThanZero(id, "organization id");
            this.organizationService.DeleteOrganization(id);

            this.TempData["DeleteOrganizationId"] = id;

            return this.RedirectToAction("Index");
        }

        /// <summary>
        ///     Populate AdministratorMainModel
        /// </summary>
        private void PopulateOrganizationMainModel(OrganizationMainModel model)
        {
            //Populate the active organizations
            IList<organization> organizations = this.organizationService.GetAll();
            //IList<role> roles = this.roleService.GetAllActive(this.currentOrganizationId);

            model.Organizations = OrganizationMap.MapItems(organizations);
            //model.AvailableRoles = RoleMap.MapItemsToSelectListItems(roles);
        }

        /// <summary>
        ///     Repopulate the model and return to the edit view when a validation error occurs
        /// </summary>
        private ActionResult ErrorOnEdit(OrganizationMainModel model)
        {
            this.PopulateOrganizationMainModel(model);
            return this.View(model);
        }

        /// <summary>
        ///     Custom push notification (partial view)
        /// </summary>
        private void PushNotification(OrganizationMainModel model, string message, string type)
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