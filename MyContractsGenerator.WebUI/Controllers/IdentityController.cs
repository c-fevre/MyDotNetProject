using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.WebUI.Common;
using MyContractsGenerator.WebUI.Models.NotificationModels;
using MyContractsGenerator.Common.I18N;
using Microsoft.Ajax.Utilities;
using MyContractsGenerator.Common.PasswordHelper;
using MyContractsGenerator.Domain;
using MyContractsGenerator.WebUI.Models.administratorModels;
using MyContractsGenerator.Interfaces.InterfacesServices;
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
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string login, string password)
        {
            //Model validation
            if (login.IsNullOrWhiteSpace())
            {
                this.ModelState.AddModelError("Login",
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
            

            administrator connectedUser = this.administratorService.GetByLogin(login, ShaHashPassword.GetSha256ResultString(password));
            if (connectedUser == null)
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
                    new Claim(ClaimTypes.Name, connectedUser.login)
                };

            var ident = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            // TODO Multilingue
            //CultureInfo userCulture = CultureInfo.CreateSpecificCulture(connectedUser.applicationlanguage.culturename);
            //AppSession.UserCultureInfo = userCulture;

            this.HttpContext.GetOwinContext()
                .Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, ident);
            

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
            if (!this.ModelState.IsValid)
            {
                return this.View("Login");
            }

            bool isThisMailExists = this.administratorService.IsThisEmailAlreadyExists(email);
            if (!isThisMailExists)
            {
                this.ModelState.AddModelError("Email", Resources.Login_UnknownLogin);
                return this.View("Login");
            }

            administrator administrator = this.administratorService.GetByEmail(email);
            this.administratorService.ResetPassword(administrator.id, administrator.id);

            AdministratorLoginModel model = new AdministratorLoginModel { Login = administrator.login };

            this.AddNotification(string.Format(Resources.Login_PasswordChangeSuccess, administrator.login, "success"));
            this.PopulateModelWithNotifications(model);

            return this.View("Login", model);
        }

        /// <summary>
        ///     This view is used by the mobile application, through a webview.
        /// </summary>
        /// <param name="userlogin">email of the edited user</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult EditPassword(string userlogin)
        {
            Requires.StringArgumentNotNullOrEmptyOrWhiteSpace(userlogin, "useremail");

            var model = new AdministratorEditPasswordModel { Login = userlogin };

            return this.View(model);
        }

        /// <summary>
        ///     EditPassword
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult EditPassword(AdministratorEditPasswordModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            administrator administrator = this.administratorService.GetByEmail(model.Login);

            //Validation
            if (administrator == null)
            {
                this.ModelState.AddModelError("Email",
                                              string.Format(Resources.administrator_ErrorMessage_UnknownUser,
                                                            model.Login));
            }

            if (ShaHashPassword.GetSha256ResultString(model.CurrentPassword) != administrator.password)
            {
                this.ModelState.AddModelError("CurrentPassword", Resources.administrator_ErrorIncorrectPassword);
            }

            if (model.NewPassword != model.NewPasswordConfirmation)
            {
                this.ModelState.AddModelError("NewPasswordConfirmation",
                                              Resources.administrator_ErrorIncorrectPasswordConfirmation);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            //Change password
            administrator.password = ShaHashPassword.GetSha256ResultString(model.NewPassword);
            this.administratorService.UpdateAdministrator(administrator);

            return this.View("EditPasswordSuccess");
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