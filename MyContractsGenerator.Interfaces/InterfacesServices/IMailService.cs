using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyContractsGenerator.DAL;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.Interfaces.InterfacesServices
{
    public interface IMailService
    {
        /// <summary>
        ///     Send mail to new user with his id and password
        /// </summary>
        /// <param name="passwordOwnerUser"></param>
        /// <param name="clearPassword"></param>
        /// <param name="userDoingCreationId"></param>
        void SendNewUserEmail(administrator passwordOwnerUser, string clearPassword, administrator userDoingCreation);

        /// <summary>
        ///     SendResetPasswordEmail
        /// </summary>
        /// <param name="user">password owner</param>
        /// <param name="clearPassword">unhashed user password</param>
        void SendResetPasswordEmail(administrator user, string clearPassword);
        
        /// <summary>
        /// Sends the form to collaborator.
        /// </summary>
        /// <param name="collaborator">The collaborator.</param>
        /// <param name="formUrl">The form URL.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="tempPassword">The temporary password.</param>
        /// <param name="lastMailTime">The last mail time.</param>
        void SendFormToCollaborator(collaborator collaborator, string formUrl, int adminId, string tempPassword, DateTime lastMailTime);

        /// <summary>
        /// Sends the form result to administrator.
        /// </summary>
        /// <param name="dbFormAnswer">The database form answer.</param>
        /// <param name="answers">The answers.</param>
        void SendFormResultToAdministrator(form_answer dbFormAnswer, IList<answer> answers);

        /// <summary>
        /// Sends the password changed administrator.
        /// </summary>
        /// <param name="administrator">The administrator.</param>
        /// <param name="password">The password.</param>
        void SendPasswordChangedAdministrator(administrator administrator, string password);

        /// <summary>
        /// Sends the welcome new administrator.
        /// </summary>
        /// <param name="administrator">The administrator.</param>
        /// <param name="password">The password.</param>
        void SendWelcomeNewAdministrator(administrator administrator, string password);

        /// <summary>
        /// Sends the generated password administrator.
        /// </summary>
        /// <param name="dbAdmin">The database admin.</param>
        /// <param name="generatedPassword">The generated password.</param>
        void SendGeneratedPasswordAdministrator(administrator dbAdmin, string generatedPassword);
    }
}
