using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.WebUI.Common;
using MyContractsGenerator.WebUI.Models.BaseModels;
using MyContractsGenerator.WebUI.Models.NotificationModels;

namespace MyContractsGenerator.WebUI.Controllers
{
    public class BaseController : Controller
    {
        private int? currentUserIdCache;

        protected int CurrentUserId
        {
            get
            {
                if (this.currentUserIdCache == null)
                {
                    this.currentUserIdCache = int.Parse(this.User.Identity.GetUserId());
                }
                return this.currentUserIdCache.Value;
            }
        }

        #region OnActionExecuted

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var res = filterContext.Result;
            if (res is ViewResult)
            {
                var model = ((ViewResult) res).Model;
                if (model is BaseModel)
                {
                    var baseModel = (BaseModel) model;

                    if (AppSession.UserNotifications != null)
                    {
                        baseModel.UserNotifications = AppSession.UserNotifications;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        ///     Add notification to DataTemp, this technique allows to keep in memory notification through redirections
        /// </summary>
        /// <param name="notificationToAdd"></param>
        protected void AddNotification(NotificationModel notificationToAdd)
        {
            //initialization
            if (this.TempData["Notifications"] == null)
            {
                this.TempData["Notifications"] = new List<NotificationModel>();
            }

            //Add the notification
            ((List<NotificationModel>) this.TempData["Notifications"]).Add(notificationToAdd);
        }

        /// <summary>
        ///     Add user notification to session
        /// </summary>
        /// <param name="userNotificationToAdd"></param>
        protected void AddUserNotification(UserNotificationModel userNotificationToAdd)
        {
            //initialization
            if (this.Session["UserNotifications"] == null)
            {
                this.Session["UserNotifications"] = new List<UserNotificationModel>();
            }

            //Add the notification
            ((List<UserNotificationModel>) this.Session["UserNotifications"]).Add(userNotificationToAdd);
        }

        /// <summary>
        ///     Add notification to DataTemp, this technique allows to keep in memory notification through redirections
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <param name="delay"></param>
        protected void AddNotification(string message, string type, int delay)
        {
            Requires.StringArgumentNotNullOrEmptyOrWhiteSpace(message, "message");

            NotificationModel notificationModel = new NotificationModel
            {
                Title = message,
                Type = type,
                Delay = delay
            };
            this.AddNotification(notificationModel);
        }

        /// <summary>
        ///     Add notification to DataTemp, this technique allows to keep in memory notification through redirections
        /// </summary>
        /// <param name="message">message of the notification</param>
        /// <param name="type">type of the notification</param>
        protected void AddNotification(string message, string type)
        {
            Requires.StringArgumentNotNullOrEmptyOrWhiteSpace(message, "message");

            NotificationModel notificationModel = new NotificationModel
            {
                Title = message,
                Type = type
            };
            this.AddNotification(notificationModel);
        }

        /// <summary>
        ///     Add notification to DataTemp, this technique allows to keep in memory notification through redirections
        /// </summary>
        /// <param name="message"></param>
        protected void AddNotification(string message)
        {
            Requires.StringArgumentNotNullOrEmptyOrWhiteSpace(message, "message");

            NotificationModel notificationModel = new NotificationModel
            {
                Title = message
            };
            this.AddNotification(notificationModel);
        }

        /// <summary>
        ///     Add notification to DataTemp, this technique allows to keep in memory notification through redirections
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <param name="delay"></param>
        protected void AddUserNotification(string message, string type, int delay, int id, int idUser)
        {
            Requires.StringArgumentNotNullOrEmptyOrWhiteSpace(message, "message");

            UserNotificationModel userNotificationModel = new UserNotificationModel
            {
                Title = message,
                Type = type,
                Delay = delay,
                Id = id,
                IdUser = idUser
            };
            this.AddUserNotification(userNotificationModel);
        }

        /// <summary>
        ///     populate baseModel with notification. Notification will be displayed in this file scripts/Customs/custom-shared.js.
        /// </summary>
        /// <param name="model"></param>
        protected void PopulateModelWithNotifications(BaseModel model)
        {
            if (this.TempData["Notifications"] == null)
            {
                model.Notifications = new List<NotificationModel>();
            }
            else
            {
                model.Notifications = (List<NotificationModel>) this.TempData["Notifications"];
            }
        }
    }
}