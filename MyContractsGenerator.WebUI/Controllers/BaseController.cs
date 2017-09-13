using System;
using System.Collections.Generic;
using System.Web.Mvc;
using log4net;
using Microsoft.AspNet.Identity;
using MyContractsGenerator.Business;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.Core.Exceptions;
using MyContractsGenerator.WebUI.Common;
using MyContractsGenerator.WebUI.Models.BaseModels;
using MyContractsGenerator.WebUI.Models.NotificationModels;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.Interfaces.InterfacesServices;

namespace MyContractsGenerator.WebUI.Controllers
{
    public class BaseController : Controller
    {
        private IAdministratorService administratorService;

        /// <summary>
        /// The current admin identifier cache
        /// </summary>
        private int currentAdminIdCache;

        /// <summary>
        /// The current organization identifier cache
        /// </summary>
        private int currentOrganizationIdCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="administratorService">The administrator service.</param>
        public BaseController(IAdministratorService administratorService)
        {
            this.administratorService = administratorService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        public BaseController( )
        {
        }

        /// <summary>
        /// Gets the current user identifier.
        /// </summary>
        /// <value>
        /// The current user identifier.
        /// </value>
        protected int CurrentUserId
        {
            get
            {
                if (this.currentAdminIdCache == 0)
                {
                    this.currentAdminIdCache = int.Parse(this.User.Identity.GetUserId());
                }
                return this.currentAdminIdCache;
            }
        }

        /// <summary>
        /// Gets the current oraganization identifier.
        /// </summary>
        /// <value>
        /// The current oraganization identifier.
        /// </value>
        protected int CurrentOrganizationId
        {
            get
            {
                if (this.currentOrganizationIdCache == 0 && this.administratorService != null)
                {
                    this.currentOrganizationIdCache =
                        this.administratorService.GetAdministratorById(int.Parse(this.User.Identity.GetUserId())).id;
                }
                else if(this.administratorService == null)
                {
                    throw new BusinessException("Administrator service not instancied by child controller");
                }
                return this.currentOrganizationIdCache;
            }
        }

        #region OnActionExecuted

        /// <summary>
        /// Méthode appelée après la méthode d'action.
        /// </summary>
        /// <param name="filterContext">Informations sur les requête et action actuelles.</param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var res = filterContext.Result;
            if (res is ViewResult)
            {
                var model = ((ViewResult)res).Model;
                if (model is BaseModel)
                {
                    var baseModel = (BaseModel)model;

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
            ((List<NotificationModel>)this.TempData["Notifications"]).Add(notificationToAdd);
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
            ((List<UserNotificationModel>)this.Session["UserNotifications"]).Add(userNotificationToAdd);
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
        /// Stores an object in the current user session.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="key">The key used to identify the object.</param>
        /// <param name="value">The object value.</param>
        protected void StoreInSession<T>(string key, T value)
        {
            this.Session[key] = value;
        }

        /// <summary>
        /// Gets an object that has previously been stored in user session.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="key">The key used to identify the object.</param>
        /// <returns>
        /// The returned object.
        /// </returns>
        protected T GetFromSession<T>(string key)
        {
            return (T)this.Session[key];
        }

        /// <summary>
        /// Removes an object from the user session.
        /// </summary>
        /// <param name="key">The key used to identify the object.</param>
        protected void RemoveFromSession(string key)
        {
            this.Session.Remove(key);
        }

        /// <summary>
        /// 
        /// </summary>
        public enum GraviteLog
        {
            /// <summary>
            /// The debug
            /// </summary>
            Debug,

            /// <summary>
            /// The information
            /// </summary>
            Info,

            /// <summary>
            /// The user error
            /// </summary>
            UserError,

            /// <summary>
            /// The system error
            /// </summary>
            SystemError
        }

        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Logs the specified gravite.
        /// </summary>
        /// <param name="gravite">The gravite.</param>
        /// <param name="UserId">The user identifier.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="userAgent">The user agent.</param>
        public void Log(GraviteLog gravite, string UserId, Exception ex, string userAgent)
        {
            if (!(ex is BusinessException || ex is ModelException))
            {
                this.Log(gravite, UserId, $"{ex.Message} - {ex.StackTrace}", userAgent);
            }
        }

        /// <summary>
        /// Logs the specified gravite.
        /// </summary>
        /// <param name="gravite">The gravite.</param>
        /// <param name="identifiantUtilisateur">The identifiant utilisateur.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="userAgent">The user agent.</param>
        /// <param name="groupe">The groupe.</param>
        /// <param name="correlationId">The correlation identifier.</param>
        public void Log(GraviteLog gravite, string identifiantUtilisateur, Exception ex, string userAgent,
            string groupe = null, Guid? correlationId = null)
        {
            this.Log(gravite, identifiantUtilisateur, ex.Message, userAgent, groupe, correlationId);
        }

        /// <summary>
        /// Logs the specified gravite.
        /// </summary>
        /// <param name="gravite">The gravite.</param>
        /// <param name="identifiantUtilisateur">The identifiant utilisateur.</param>
        /// <param name="message">The message.</param>
        /// <param name="userAgent">The user agent.</param>
        /// <param name="groupe">The groupe.</param>
        /// <param name="correlationId">The correlation identifier.</param>
        public void Log(GraviteLog gravite, string identifiantUtilisateur, string message, string userAgent,
    string groupe = null, Guid? correlationId = null)
        {
            string messageAndUserAgent = $"{userAgent} - {System.Web.HttpUtility.HtmlEncode(message)}";
            switch (gravite)
            {
                case GraviteLog.Debug:
                    if (log.IsDebugEnabled)
                    {
                        log.Debug(messageAndUserAgent);
                    }
                    break;

                case GraviteLog.Info:
                    if (log.IsInfoEnabled)
                    {
                        log.Info(messageAndUserAgent);
                    }
                    break;

                case GraviteLog.SystemError:
                    if (log.IsFatalEnabled)
                    {
                        log.Fatal(messageAndUserAgent);
                    }
                    break;

                case GraviteLog.UserError:
                    if (log.IsErrorEnabled)
                    {
                        log.Error(messageAndUserAgent);
                    }
                    break;
            }
        }

        /// <summary>
        /// Méthode générique de traitement d'une erreur
        /// </summary>
        /// <param name="filterContext">Contexte de l'exception à traiter</param>
        private void ErrorManager(ExceptionContext filterContext)
        {
            try
            {
                if (filterContext.Exception == null)
                {
                    return;
                }

                if (log.IsErrorEnabled)
                {
                    log.Error(
                        "Who: " + filterContext.HttpContext.User.Identity.Name + " - What: " +
                        filterContext.Exception.Message, filterContext.Exception);
                }


                var exception = filterContext.Exception;
                Type typeException = exception.GetType();

                if (typeof(ModelException).IsAssignableFrom(typeException))
                {
                    foreach (var error in ((ModelException)exception).Errors)
                    {
                        this.ModelState.AddModelError(error.Field, error.Message);
                    }
                    filterContext.ExceptionHandled = true;
                    var actionName = this.RouteData.GetRequiredString("action");
                    this.ActionInvoker.InvokeAction(filterContext.Controller.ControllerContext, actionName);
                }
                else if (typeof(BusinessException).IsAssignableFrom(typeException))
                {
                    this.Log(GraviteLog.UserError, this.User.Identity.Name, exception, this.Request.UserAgent);
                    this.ModelState.AddModelError(string.Empty, exception.Message);
                    filterContext.ExceptionHandled = true;
                    var actionName = this.RouteData.GetRequiredString("action");
                    this.ActionInvoker.InvokeAction(filterContext.Controller.ControllerContext, actionName);
                }
                else if (typeof(System.Threading.ThreadAbortException).IsAssignableFrom(typeException))
                {
                    filterContext.ExceptionHandled = true;
                    // Si le thread a été abandonné, c'est suite à un Response.redirect ou Server.transfer
                    // Aucune action n'est nécessaire pour traiter cette erreur
                }
                else if (typeof(System.Web.HttpRequestValidationException).IsAssignableFrom(typeException))
                {
                    filterContext.ExceptionHandled = true;
                    filterContext.Controller.TempData["HttpRequestValidationException"] =
                        Resources.HttpRequestValidationException;
                    // Il est nécessaire de conserver la donnée pour 2 requêtes (view + layout)
                    filterContext.Controller.TempData.Keep();
                }
                else
                {
                    this.Log(GraviteLog.SystemError, this.User.Identity.Name, exception, this.Request.UserAgent);
                    throw exception;
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                // L'appel à Server.transfer ou Response.Redirect provoque l'arrêt du thread courant, avec une levée d'exception
                // Cette exception n'est pas à traiter
            }
            catch (System.Web.HttpRequestValidationException exception)
            {
                this.Log(GraviteLog.SystemError, this.User.Identity.Name, exception, this.Request.UserAgent);
                throw;
            }
            catch (Exception exception)
            {
                this.Log(GraviteLog.SystemError, this.User.Identity.Name, exception, this.Request.UserAgent);
                // Si on passe ici c'est que la gestion d'erreur se passe mal. Faire une redirection vers une page d'erreur?
                throw;
            }
        }

        /// <summary>
        /// Called when an unhandled exception occurs in the action.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnException(ExceptionContext filterContext)
        {
            // filterContext.RequestContext.HttpContext.Request.Browser
            this.ErrorManager(filterContext);

            if (!filterContext.ExceptionHandled)
            {
                // Ajout de la redirection en réponse Ajax
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.ExceptionHandled = true;
                    filterContext.Result = new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new { Success = false, ErrorMessage = filterContext.Exception.Message }
                    };
                }
                else
                {
                    base.OnException(filterContext);
                }
            }
            else
            {
                var exception = filterContext.Exception;
                // Pas de redirection pour les erreurs traitées avec retour client
                if (!(exception is BusinessException) && !(exception is ModelException))
                {
                    // Redirection sur la page d'origine
                    if (filterContext.HttpContext.Request.UrlReferrer != null)
                    {
                        filterContext.HttpContext.Response.Redirect(filterContext.HttpContext.Request.UrlReferrer.ToString());
                    }
                }
            }
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
                model.Notifications = (List<NotificationModel>)this.TempData["Notifications"];
            }
        }
    }
}