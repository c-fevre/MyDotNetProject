using System.Collections.Generic;
using MyContractsGenerator.DAL;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.Interfaces.InterfacesServices
{
    public interface IAdministratorService
    {
        /// <summary>
        ///     Gets administrator by login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        administrator GetByLogin(string email, string password);

        /// <summary>
        ///     Gets administrator by login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        administrator GetByEmail(string email);

        /// <summary>
        ///     Gets administrator by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        administrator GetAdministratorById(int id);

        /// <summary>
        ///     Gets all administrators
        /// </summary>
        /// <returns></returns>
        IList<administrator> GetActiveAdministrators();

        /// <summary>
        ///     delete logically the administrator
        /// </summary>
        /// <param name="administratorId"></param>
        void DeleteAdministrator(int administratorId);

        /// <summary>
        ///     Check if this email is already used be an active administrator
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true: this email is already used be an active administrator</returns>
        bool IsThisEmailAlreadyExists(string email);

        /// Check if this email is already used by an active administrator, excepted the administrator passed by parameter
        /// </summary>
        /// <param name="email"></param>
        /// <param name="administratorId"></param>
        /// <returns>true: this email is already used be an active administrator, excepted the administrator passed by parameter</returns>
        bool IsThisEmailAlreadyExists(string email, int administratorId);

        /// <summary>
        ///     Gets administrator by login/pwd
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        administrator GetAdministratorByLogin(string email, string password);

        /// <summary>
        ///     Reset administrator password and send the generated password by mail
        /// </summary>
        /// <param name="passwordOwneradministratorId"></param>
        /// <param name="administratorDoingUpdateId"></param>
        void ResetPassword(int passwordOwneradministratorId, int administratorDoingUpdateId);

        /// <summary>
        /// Updates the administrator.
        /// </summary>
        /// <param name="administratorToUpdate">The administrator to update.</param>
        void UpdateAdministrator(administrator administratorToUpdate);

    }
}
