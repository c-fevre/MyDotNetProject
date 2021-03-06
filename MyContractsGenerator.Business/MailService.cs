﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web.Configuration;
using MyContractsGenerator.Common;
using MyContractsGenerator.Common.I18N;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesRepo;
using MyContractsGenerator.Interfaces.InterfacesServices;
using SendGrid;

namespace MyContractsGenerator.Business
{
    public class MailService : IMailService
    {
        /// <summary>
        ///     The administrator repository
        /// </summary>
        private readonly IAdministratorRepository administratorRepository;

        /// <summary>
        ///     Constructor
        /// </summary>
        public MailService(IAdministratorRepository administratorRepository)
        {
            this.administratorRepository = administratorRepository;
        }

        /// <summary>
        ///     Sends the form to collaborator.
        /// </summary>
        /// <param name="collaborator">The collaborator.</param>
        /// <param name="formUrl">The form URL.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="tempPassword">The temporary password.</param>
        /// <param name="lastMailTime">The last mail time.</param>
        public void SendFormToCollaborator(collaborator collaborator, string formUrl, int adminId, string tempPassword,
                                           DateTime lastMailTime)
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

            if (lastMailTime.AddDays(+1) < DateTime.Now)
            {
                this.SendEmail(
                    new List<string> { collaborator.email },
                    Resources.Form_CollaboratorMailSubject,
                    string.Format(Resources.Form_CollaboratorMailBody, currentAdministrator.email, currentAdministrator.organization.label, formUrl, tempPassword));
            }

            //Restore previous values
            Thread.CurrentThread.CurrentUICulture = currentUICulture;
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }

        /// <summary>
        /// Sends the form result to administrator.
        /// </summary>
        /// <param name="formAnswer">The form answer.</param>
        /// <param name="answers">The answers.</param>
        public void SendFormResultToAdministrator(form_answer formAnswer, IList<answer> answers)
        {
            Requires.ArgumentNotNull(formAnswer, "dbFormAnswer");
            Requires.ArgumentNotNull(answers, "answers");

            //change temporarily the cultureInfo to send the mail in the default application web language
            CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(WebConfigurationManager.AppSettings["MailCulture"]);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(WebConfigurationManager.AppSettings["MailCulture"]);

            string answersString = "";
            answers.OrderBy(a => a.question.order).ToList().ForEach(
                a => { answersString += $"<b>{a.question.label}</b> : {a.answer_value}<br/>"; });

            string collaboratorIdentity = $"{formAnswer.collaborator.lastname} {formAnswer.collaborator.firstname}";

            this.SendEmail(
                formAnswer.organization.administrators.Select(a => a.email),
                Resources.Form_CollaboratorAnswerMailSubject,
                string.Format(Resources.Form_CollaboratorAnswerMailBody, formAnswer.organization.label, collaboratorIdentity, formAnswer.role.label,
                              answersString)
            );

            //Restore previous values
            Thread.CurrentThread.CurrentUICulture = currentUICulture;
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }

        /// <summary>
        ///     Sends the generated password administrator.
        /// </summary>
        /// <param name="administrator">The administrator.</param>
        /// <param name="generatedPassword">The generated password.</param>
        public void SendGeneratedPasswordAdministrator(administrator administrator, string generatedPassword)
        {
            Requires.ArgumentNotNull(administrator, "administrator");
            Requires.ArgumentNotNull(generatedPassword, "administrator generatedPassword");

            //change temporarily the cultureInfo to send the mail in the default application web language
            CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(WebConfigurationManager.AppSettings["MailCulture"]);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(WebConfigurationManager.AppSettings["MailCulture"]);

            string administratorIdentity = $"{administrator.lastname} {administrator.firstname}";

            this.SendEmail(
                new List<string> { administrator.email },
                Resources.Administrator_GeneratedAdministratorPasswordTitle,
                string.Format(Resources.Administrator_GeneratedAdministratorPasswordBody, administratorIdentity,
                              GlobalAppSettings.ApplicationBaseUrl, administrator.organization.label, administrator.email, generatedPassword)
            );

            //Restore previous values
            Thread.CurrentThread.CurrentUICulture = currentUICulture;
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }

        /// <summary>
        ///     Sends the password changed administrator.
        /// </summary>
        /// <param name="administrator">The administrator.</param>
        /// <param name="password">The password.</param>
        public void SendPasswordChangedAdministrator(administrator administrator, string password)
        {
            Requires.ArgumentNotNull(administrator, "administrator");
            Requires.ArgumentNotNull(password, "administrator password");

            //change temporarily the cultureInfo to send the mail in the default application web language
            CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(WebConfigurationManager.AppSettings["MailCulture"]);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(WebConfigurationManager.AppSettings["MailCulture"]);

            string administratorIdentity = $"{administrator.lastname} {administrator.firstname}";

            this.SendEmail(
                new List<string> { administrator.email },
                Resources.Administrator_NewAdministratorPasswordTitle,
                string.Format(Resources.Administrator_NewAdministratorPasswordBody, administratorIdentity,
                              GlobalAppSettings.ApplicationBaseUrl, administrator.organization.label, administrator.email)
            );

            //Restore previous values
            Thread.CurrentThread.CurrentUICulture = currentUICulture;
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }

        /// <summary>
        ///     Sends the welcome new administrator.
        /// </summary>
        /// <param name="administrator">The administrator.</param>
        /// <param name="password">The password.</param>
        public void SendWelcomeNewAdministrator(administrator administrator, string password)
        {
            Requires.ArgumentNotNull(administrator, "administrator");
            Requires.ArgumentNotNull(password, "password");

            //change temporarily the cultureInfo to send the mail in the default application web language
            CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(WebConfigurationManager.AppSettings["MailCulture"]);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(WebConfigurationManager.AppSettings["MailCulture"]);

            string administratorIdentity = $"{administrator.lastname} {administrator.firstname}";

            this.SendEmail(
                new List<string> { administrator.email },
                Resources.Administrator_WelcomeNewAdministratorTitle,
                string.Format(Resources.Administrator_WelcomeNewAdministratorBody, administratorIdentity, administrator.organization.label,
                              GlobalAppSettings.ApplicationBaseUrl, administrator.email, password)
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