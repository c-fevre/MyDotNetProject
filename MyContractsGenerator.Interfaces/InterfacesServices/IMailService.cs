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
        void SendFormToCollaborator(collaborator collaborator, string formUrl, int adminId, string tempPassword);
    }
}
