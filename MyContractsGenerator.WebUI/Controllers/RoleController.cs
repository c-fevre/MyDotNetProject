using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesServices;
using MyContractsGenerator.WebUI.Mapping;
using MyContractsGenerator.WebUI.Models.NotificationModels;
using MyContractsGenerator.WebUI.Models.RoleModels;

namespace MyContractsGenerator.WebUI.Controllers
{
    [Authorize]
    public class RoleController : BaseController
    {
        /// <summary>
        ///     The role service
        /// </summary>
        private readonly IRoleService roleService;

        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        /// <summary>
        ///     GET Index
        /// </summary>
        [HttpGet]
        public ActionResult Index()
        {
            RoleMainModel model = new RoleMainModel();
            this.PopulateRoleMainModel(model);

            //display a notification if an administrator has been deleted
            if (this.TempData["DeleteRoleId"] == null || (int) this.TempData["DeleteRoleId"] <= 0)
            {
                return this.View(model);
            }

            role deleteRole = this.roleService.GetById((int) this.TempData["DeleteRoleId"]);
            NotificationModel notificationModel = new NotificationModel
            {
                Title =
                    string.Format(Resources.Role_RemoveNotification_Message, deleteRole.label)
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
                role defaultSelectedRole = this.roleService.GetAllActive().FirstOrDefault();
                return defaultSelectedRole == null
                    ? this.RedirectToAction("Index")
                    : this.RedirectToAction("Edit", new { defaultSelectedRole.id });
            }

            RoleMainModel model = new RoleMainModel();
            this.PopulateRoleMainModel(model);

            role dbRole = this.roleService.GetById(id);
            model.EditedRole = RoleMap.MapItem(dbRole);

            //display a notification if an administrator has been deleted
            if (this.TempData["RoleCreated"] != null && (bool) this.TempData["RoleCreated"])
            {
                this.PushNotification(model,
                                      string.Format(Resources.Role_Added, $"{dbRole.label}"),
                                      "success");
            }

            return this.View(model);
        }

        /// <summary>
        ///     POST Edit
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoleMainModel model)
        {
            Requires.ArgumentNotNull(model, "model");
            if (!this.ModelState.IsValid)
            {
                return this.ErrorOnEdit(model);
            }

            role existingRole = this.roleService.GetById(model.EditedRole.Id);
            if (existingRole == null)
            {
                return this.ErrorOnEdit(model);
            }

            //Verify if the label is already used
            if (this.roleService.IsThisLabelAlreadyExists(model.EditedRole.Label, model.EditedRole.Id))
            {
                this.ModelState.AddModelError("EditedRole.Label",
                                              Resources.Role_ErrorIncorrectLabelAlreadyUsed);
                return this.ErrorOnEdit(model);
            }

            if (!this.ModelState.IsValid)
            {
                return this.ErrorOnEdit(model);
            }

            existingRole.label = model.EditedRole.Label;

            this.roleService.UpdateRole(existingRole);

            this.PopulateRoleMainModel(model);

            this.PushNotification(model,
                                  string.Format(Resources.Role_Edited, $"{existingRole.label}"), "success");

            return this.View(model);
        }

        /// <summary>
        ///     Create
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            RoleMainModel model = new RoleMainModel();
            this.PopulateRoleMainModel(model);
            model.EditedRole = new RoleModel();

            //TODO Multilingue
            // model.EditedRole.Role = false;
            // model.EditedRole.ApplicationLangageId = Constants.StaticIdDefaultApplicationLangage;

            return this.View(model);
        }

        /// <summary>
        ///     POST Create
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoleMainModel model)
        {
            Requires.ArgumentNotNull(model, "model");

            if (!this.ModelState.IsValid)
            {
                return this.ErrorOnEdit(model);
            }

            //Verify if the email is already used
            if (this.roleService.IsThisLabelAlreadyExists(model.EditedRole.Label))
            {
                this.ModelState.AddModelError("EditedRole.Label",
                                              Resources.Role_ErrorIncorrectLabelAlreadyUsed);
                return this.ErrorOnEdit(model);
            }

            role newRole = new role
            {
                label = model.EditedRole.Label
            };

            // TODO Multilingue
            //newRole.isadministrator = model.EditedRole.Role;
            //newRole.applicationlanguage =
            //this.applicationLangageService.GetById(model.EditedRole.ApplicationLangageId);

            role dbCollab = this.roleService.AddRole(newRole);

            this.PopulateRoleMainModel(model);

            this.TempData["RoleCreated"] = true;

            return this.RedirectToAction("Edit", "Role", new { dbCollab.id });
        }

        /// <summary>
        ///     Remove the role
        /// </summary>
        /// <param name="id">role to remove</param>
        [HttpGet]
        public ActionResult Remove(int id)
        {
            Requires.ArgumentGreaterThanZero(id, "role id");

            this.roleService.DeleteRole(id);

            this.TempData["DeleteRoleId"] = id;

            return this.RedirectToAction("Index");
        }

        /// <summary>
        ///     Populate RoleMainModel
        /// </summary>
        private void PopulateRoleMainModel(RoleMainModel model)
        {
            //Populate the active roles
            IList<role> roles = this.roleService.GetAllActive();
            model.Roles = RoleMap.MapItems(roles);

            //populate the available application langages
            //TODO Multilingue
            //model.AvailableApplicationLangage = new List<SelectListItem>();
            /*IList<applicationlanguage> applicationLangages = this.applicationLangageService.GetAll();
            foreach (var applicationLangage in applicationLangages)
            {
                model.AvailableApplicationLangage.Add(new SelectListItem
                {
                    Text = applicationLangage.label,
                    Value = applicationLangage.id.ToString()
                });
            }*/
        }

        /// <summary>
        ///     Repopulate the model and return to the edit view when a validation error occurs
        /// </summary>
        private ActionResult ErrorOnEdit(RoleMainModel model)
        {
            this.PopulateRoleMainModel(model);
            return this.View(model);
        }

        /// <summary>
        ///     Custom push notification (partial view)
        /// </summary>
        private void PushNotification(RoleMainModel model, string message, string type)
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