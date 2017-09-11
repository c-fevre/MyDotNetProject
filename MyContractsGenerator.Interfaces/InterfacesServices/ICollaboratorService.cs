using System.Collections.Generic;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.Interfaces.InterfacesServices
{
    public interface ICollaboratorService
    {
        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        IList<collaborator> GetAllActive();

        /// <summary>
        ///     Gets administrator by login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        collaborator GetByEmail(string email);

        /// <summary>
        ///     Gets administrator by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        collaborator GetById(int id);

        /// <summary>
        ///     Check if this email is already used be an active administrator
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true: this email is already used be an active administrator</returns>
        bool IsThisEmailAlreadyExists(string email);

        /// <summary>
        ///     Determines whether [is this email already exists] [the specified email address].
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="currentCollaboratorId">The identifier.</param>
        /// <returns>
        ///     <c>true</c> if [is this email already exists] [the specified email address]; otherwise, <c>false</c>.
        /// </returns>
        bool IsThisEmailAlreadyExists(string emailAddress, int currentCollaboratorId);

        /// <summary>
        ///     delete logically the user
        /// </summary>
        /// <param name="collaboratorId"></param>
        void DeleteCollaborator(int collaboratorId);

        /// <summary>
        ///     Updates the collaborator.
        /// </summary>
        /// <param name="collaboratorToUpdate">The collaborator to update.</param>
        void UpdateCollaborator(collaborator collaboratorToUpdate);

        /// <summary>
        ///     Adds the collaborator.
        /// </summary>
        /// <param name="collaboratorToCreate">The collaborator to create.</param>
        /// <returns></returns>
        collaborator AddCollaborator(collaborator collaboratorToCreate);
    }
}