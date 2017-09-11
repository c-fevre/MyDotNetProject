using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.Common.I18N;
using Microsoft.Ajax.Utilities;
using MyContractsGenerator.Common;
using MyContractsGenerator.Common.PasswordHelper;
using MyContractsGenerator.Domain;
using MyContractsGenerator.WebUI.Models.administratorModels;
using MyContractsGenerator.Interfaces.InterfacesServices;
using MyContractsGenerator.WebUI.Common;

namespace MyContractsGenerator.WebUI.Controllers
{
    public class IdentityController : BaseController
    {
        private readonly IAdministratorService administratorService;

        public IdentityController(IAdministratorService administratorService)
        {
            this.administratorService = administratorService;
        }

        /// <summary>
        ///     Login Page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return this.View();
        }

        /// <summary>
        ///     Login Action
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            //Model validation
            if (email.IsNullOrWhiteSpace())
            {
                this.ModelState.AddModelError("Email",
                                              string.Format(Resources.Login_RequiredField, Resources.Login_Label));
            }
            if (password.IsNullOrWhiteSpace())
            {
                this.ModelState.AddModelError("Password",
                                              string.Format(Resources.Login_RequiredField, Resources.Login_Password));
            }
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }


            administrator connectedUser = this.administratorService.GetByEmail(email);
            if (connectedUser == null)
            {
                this.ModelState.AddModelError("Password", Resources.Login_InvalidUserNameOrPassword);
                return this.View();
            }
            if (connectedUser.password != ShaHashPassword.GetSha256ResultString(password))
            {
                this.ModelState.AddModelError("Password", Resources.Login_InvalidUserNameOrPassword);
                return this.View();
            }

            var claims =
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, connectedUser.id.ToString()),
                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                              "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
                    new Claim(ClaimTypes.Name, connectedUser.email),
                    connectedUser.is_super_admin ? new Claim(ClaimTypes.Role, AppConstants.SuperAdminRoleLabel) : new Claim(ClaimTypes.Role, AppConstants.AdminRoleLabel)
                };

            var ident = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            // TODO Multilingue
            //CultureInfo userCulture = CultureInfo.CreateSpecificCulture(connectedUser.applicationlanguage.culturename);
            //AppSession.UserCultureInfo = userCulture;

            this.HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, ident);


            return this.RedirectToAction("Index", "Role");
        }

        /// <summary>
        ///     Forgot Password Action
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string email)
        {
            AdministratorLoginModel model = new AdministratorLoginModel { Email = email };

            if (string.IsNullOrEmpty(email))
            {
                this.AddNotification(string.Format(Resources.Login_RequiredField, Resources.Email_Label), "danger");

                this.PopulateModelWithNotifications(model);
                return this.View("Login", model);
            }

            bool isThisMailExists = this.administratorService.IsThisEmailAlreadyExists(email);
            if (!isThisMailExists)
            {
                this.AddNotification(Resources.Login_UnknownLogin, "danger");

                this.PopulateModelWithNotifications(model);
                return this.View("Login", model);
            }

            administrator administrator = this.administratorService.GetByEmail(email);
            this.administratorService.ResetPassword(administrator.id);
            model.Email = administrator.email;

            this.AddNotification(string.Format(Resources.Login_PasswordChangeSuccess, administrator.email), "success");
            this.PopulateModelWithNotifications(model);

            return this.View("Login", model);
        }

        /// <summary>
        ///     Logoutaction
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            this.HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return this.RedirectToAction("Login");
        }
    }
}