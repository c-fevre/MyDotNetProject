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
    }
}
