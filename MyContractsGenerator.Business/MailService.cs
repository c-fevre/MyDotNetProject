using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;
using SendGrid;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesRepo;
using MyContractsGenerator.Interfaces.InterfacesServices;

namespace MyContractsGenerator.Business
{
    public class MailService : IMailService
    {
        /// <summary>
        /// The administrator repository
        /// </summary>
        private IAdministratorRepository administratorRepository;

        /// <summary>
        ///     Constructor
        /// </summary>     
        public MailService(IAdministratorRepository administratorRepository)
        {
            this.administratorRepository = administratorRepository;
        }

        /// <summary>
        ///     Send mail to new user with his id and password
        /// </summary>
        /// <param name="passwordOwnerUser"></param>
        /// <param name="clearPassword"></param>
        /// <param name="userDoingCreationId"></param>
        public void SendNewUserEmail(administrator passwordOwnerUser, string clearPassword, administrator userDoingCreation)
        {
            Requires.ArgumentNotNull(passwordOwnerUser, "user");
            Requires.StringArgumentNotNullOrEmptyOrWhiteSpace(passwordOwnerUser.email, "user.emailaddress");
            Requires.ArgumentNotNull(userDoingCreation, "userDoingCreation");

            //change temporarily the cultureInfo to send the mail in the default application web language
            CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(WebConfigurationManager.AppSettings["MailCulture"]);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(WebConfigurationManager.AppSettings["MailCulture"]);

            this.SendEmail(
                new List<string> { passwordOwnerUser.email },
                Resources.User_PasswordCreate_MailSubject,
                string.Format(
                    Resources.User_PasswordCreate_MailBody,
                    WebConfigurationManager.AppSettings["ServerWebUiUrl"],
                    DateTime.Now.ToShortDateString(),
                    userDoingCreation.login,
                    passwordOwnerUser.email,
                    clearPassword
                )
            );

            //Restore previous values
            Thread.CurrentThread.CurrentUICulture = currentUICulture;
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }

        /// <summary>
        ///     SendResetPasswordEmail
        /// </summary>
        /// <param name="administrator">password owner</param>
        /// <param name="clearPassword">unhashed user password</param>
        public void SendResetPasswordEmail(administrator administrator, string clearPassword)
        {
            Requires.ArgumentNotNull(administrator, "user");
            Requires.StringArgumentNotNullOrEmptyOrWhiteSpace(administrator.email, "user.emailaddress");

            //change temporarily the cultureInfo to send the mail in the default application web language
            CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(WebConfigurationManager.AppSettings["MailCulture"]);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(WebConfigurationManager.AppSettings["MailCulture"]);

            this.SendEmail(
                new List<string> { administrator.email },
                Resources.User_PasswordChange_MailSubject,
                string.Format(Resources.User_PasswordChange_MailBody, clearPassword)
            );

            //Restore previous values
            Thread.CurrentThread.CurrentUICulture = currentUICulture;
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }

        public void SendFormToCollaborator(collaborator collaborator, string formUrl, int adminId)
        {
            Requires.ArgumentNotNull(collaborator, "collaborator");
            Requires.StringArgumentNotNullOrEmptyOrWhiteSpace(formUrl, "formUrl");
            Requires.StringArgumentNotNullOrEmptyOrWhiteSpace(collaborator.email, "collaborator.emailAdress");

            //change temporarily the cultureInfo to send the mail in the default application web language
            CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(WebConfigurationManager.AppSettings["MailCulture"]);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(WebConfigurationManager.AppSettings["MailCulture"]);

            administrator currentAdministrator = this.administratorRepository.GetById(adminId);

            this.SendEmail(
                new List<string> { collaborator.email },
                Resources.Form_CollaboratorMailSubject,
                string.Format(Resources.Form_CollaboratorMailBody, currentAdministrator.email, formUrl)
            );

            //Restore previous values
            Thread.CurrentThread.CurrentUICulture = currentUICulture;
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }

        /// <summary>
        ///     Sends generic mail
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="senderMail"></param>
        /// <param name="senderName"></param>
        /// <param name="subject"></param>
        /// <param name="mailBody"></param>
        protected void SendEmail(IEnumerable<string> recipients, string subject, string mailBody)
        {
            Requires.ArgumentNotNull(recipients, "recipients");
            if (!recipients.Any())
            {
                throw new ArgumentException("recipients can't be empty");
            }

            // Create the email object first, then add the properties.
            SendGridMessage email = new SendGridMessage();
            foreach (var recipient in recipients)
            {
                email.AddTo(recipient);
            }

            email.From = new MailAddress(WebConfigurationManager.AppSettings["SendGridSenderMail"],
                                         WebConfigurationManager.AppSettings["SendGridSenderName"]);
            email.Subject = subject;
            email.Html = mailBody;

            // Create a Web transport, using API Key
            var transportWeb = new Web(WebConfigurationManager.AppSettings["SendGridApiKey"]);

            // Send the email.
            transportWeb.DeliverAsync(email);
        }
    }
}
