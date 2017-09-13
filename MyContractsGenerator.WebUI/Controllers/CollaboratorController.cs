using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesServices;
using MyContractsGenerator.WebUI.Mapping;
using MyContractsGenerator.WebUI.Models.CollaboratorModels;
using MyContractsGenerator.WebUI.Models.NotificationModels;

namespace MyContractsGenerator.WebUI.Controllers
{
    [Authorize]
    public class CollaboratorController : BaseController
    {
        /// <summary>
        ///     The collaborator service
        /// </summary>
        private readonly ICollaboratorService collaboratorService;

        /// <summary>
        ///     The role service
        /// </summary>
        private readonly IRoleService roleService;

        /// <summary>
        /// The administrator service
        /// </summary>
        private readonly IAdministratorService administratorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollaboratorController"/> class.
        /// </summary>
        /// <param name="collaboratorService">The collaborator service.</param>
        /// <param name="roleService">The role service.</param>
        /// <param name="administratorService">The administrator service.</param>
        public CollaboratorController(ICollaboratorService collaboratorService, IRoleService roleService, 
                                           IAdministratorService administratorService) : base(administratorService)
        {
            this.administratorService = administratorService;
            this.collaboratorService = collaboratorService;
            this.roleService = roleService;
        }

        /// <summary>
        ///     GET Index
        /// </summary>
        [HttpGet]
        public ActionResult Index()
        {
            CollaboratorMainModel model = new CollaboratorMainModel();
            this.PopulateCollaboratorMainModel(model);

            //display a notification if an administrator has been deleted
            if (this.TempData["DeleteCollaboratorId"] == null || (int)this.TempData["DeleteCollaboratorId"] <= 0)
            {
                return this.View(model);
            }

            collaborator deleteCollaborator =
                this.collaboratorService.GetById((int)this.TempData["DeleteCollaboratorId"], this.CurrentOrganizationId);
            NotificationModel notificationModel = new NotificationModel
            {
                Title =
                    string.Format(Resources.Collaborator_RemoveNotification_Message, deleteCollaborator.lastname,
                                  deleteCollaborator.firstname)
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
                collaborator defaultSelectedCollaborator = this.collaboratorService.GetAllActive(this.CurrentOrganizationId).FirstOrDefault();
                return defaultSelectedCollaborator == null
                    ? this.RedirectToAction("Index")
                    : this.RedirectToAction("Edit", new { defaultSelectedCollaborator.id });
            }

            CollaboratorMainModel model = new CollaboratorMainModel();
            this.PopulateCollaboratorMainModel(model);

            collaborator collaborator = this.collaboratorService.GetById(id, this.CurrentOrganizationId);
            model.EditedCollaborator = CollaboratorMap.MapItem(collaborator);

            //display a notification if an administrator has been deleted
            if (this.TempData["CollaboratorCreated"] != null && (bool)this.TempData["CollaboratorCreated"])
            {
                this.PushNotification(model,
                                      string.Format(Resources.Collaborator_Added,
                                                    $"{collaborator.lastname} {collaborator.firstname}"),
                                      "success");
            }

            return this.View(model);
        }

        /// <summary>
        ///     POST Edit
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CollaboratorMainModel model)
        {
            Requires.ArgumentNotNull(model, "model");
            if (!this.ModelState.IsValid)
            {
                return this.ErrorOnEdit(model);
            }

            collaborator existingCollaborator = this.collaboratorService.GetById(model.EditedCollaborator.Id, this.CurrentOrganizationId);
            if (existingCollaborator == null)
            {
                return this.ErrorOnEdit(model);
            }

            //Verify if the email is already used
            if (this.collaboratorService.IsThisEmailAlreadyExists(model.EditedCollaborator.Email,
                                                                  model.EditedCollaborator.Id,
                                                                  this.CurrentOrganizationId))
            {
                this.ModelState.AddModelError("EditedCollaborator.Email",
                                              Resources.Administrator_ErrorIncorrectEmailAlreadyUsed);
                return this.ErrorOnEdit(model);
            }

            if (!this.ModelState.IsValid)
            {
                return this.ErrorOnEdit(model);
            }

            if (model.EditedCollaborator.LinkedRolesIds == null)
            {
                model.EditedCollaborator.LinkedRolesIds = new List<int>();
            }

            existingCollaborator.email = model.EditedCollaborator.Email;
            existingCollaborator.firstname = model.EditedCollaborator.FirstName;
            existingCollaborator.lastname = model.EditedCollaborator.LastName;

            this.collaboratorService.UpdateCollaborator(existingCollaborator, this.CurrentOrganizationId);

            this.roleService.AffectToRole(model.EditedCollaborator.LinkedRolesIds,
                                          model.EditedCollaborator.Id, this.CurrentOrganizationId);

            this.PopulateCollaboratorMainModel(model);

            this.PushNotification(model,
                                  string.Format(Resources.Collaborator_Edited,
                                                $"{existingCollaborator.lastname} {existingCollaborator.firstname}"),
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
            CollaboratorMainModel model = new CollaboratorMainModel();
            this.PopulateCollaboratorMainModel(model);
            model.EditedCollaborator = new CollaboratorModel();

            return this.View(model);
        }

        /// <summary>
        ///     POST Create
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CollaboratorMainModel model)
        {
            Requires.ArgumentNotNull(model, "model");

            if (!this.ModelState.IsValid)
            {
                return this.ErrorOnEdit(model);
            }

            //Verify if the email is already used
            if (this.collaboratorService.IsThisEmailAlreadyExists(model.EditedCollaborator.Email, this.CurrentOrganizationId))
            {
                this.ModelState.AddModelError("EditedCollaborator.Email",
                                              Resources.Administrator_ErrorIncorrectEmailAlreadyUsed);
                return this.ErrorOnEdit(model);
            }

            collaborator newCollaborator = new collaborator
            {
                email = model.EditedCollaborator.Email,
                firstname = model.EditedCollaborator.FirstName,
                lastname = model.EditedCollaborator.LastName
            };

            collaborator dbCollab = this.collaboratorService.AddCollaborator(newCollaborator, this.CurrentOrganizationId);

            this.roleService.AffectToRole(model.EditedCollaborator.LinkedRolesIds,
                                          dbCollab.id, this.CurrentOrganizationId);

            this.PopulateCollaboratorMainModel(model);

            this.TempData["CollaboratorCreated"] = true;

            return this.RedirectToAction("Edit", "Collaborator", new { dbCollab.id });
        }

        /// <summary>
        ///     Remove the collaborator
        /// </summary>
        /// <param name="id">collaborator to remove</param>
        [HttpGet]
        public ActionResult Remove(int id)
        {
            Requires.ArgumentGreaterThanZero(id, "collaborator id");
            this.collaboratorService.DeleteCollaborator(id, this.CurrentOrganizationId);

            this.TempData["DeleteCollaboratorId"] = id;

            return this.RedirectToAction("Index");
        }

        /// <summary>
        ///     Populate AdministratorMainModel
        /// </summary>
        private void PopulateCollaboratorMainModel(CollaboratorMainModel model)
        {
            //Populate the active collaborators
            IList<collaborator> collaborators = this.collaboratorService.GetAllActive(this.CurrentOrganizationId);
            IList<role> roles = this.roleService.GetAllActive(this.CurrentOrganizationId);

            model.Collaborators = CollaboratorMap.MapItems(collaborators);
            model.AvailableRoles = RoleMap.MapItemsToSelectListItems(roles);
        }

        /// <summary>
        ///     Repopulate the model and return to the edit view when a validation error occurs
        /// </summary>
        private ActionResult ErrorOnEdit(CollaboratorMainModel model)
        {
            this.PopulateCollaboratorMainModel(model);
            return this.View(model);
        }

        /// <summary>
        ///     Custom push notification (partial view)
        /// </summary>
        private void PushNotification(CollaboratorMainModel model, string message, string type)
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