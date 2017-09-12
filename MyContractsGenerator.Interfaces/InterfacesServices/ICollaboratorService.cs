using System.Collections.Generic;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.Interfaces.InterfacesServices
{
    public interface ICollaboratorService
    {
        /// <summary>
        /// Gets all active.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        IList<collaborator> GetAllActive(int organizationId);

        /// <summary>
        /// Gets the by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        collaborator GetByEmail(string email, int organizationId);

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        collaborator GetById(int id, int organizationId);

        /// <summary>
        /// Determines whether [is this email already exists] [the specified email].
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is this email already exists] [the specified email]; otherwise, <c>false</c>.
        /// </returns>
        bool IsThisEmailAlreadyExists(string email, int organizationId);

        /// <summary>
        /// Determines whether [is this email already exists] [the specified email address].
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="currentCollaboratorId">The current collaborator identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is this email already exists] [the specified email address]; otherwise, <c>false</c>.
        /// </returns>
        bool IsThisEmailAlreadyExists(string emailAddress, int currentCollaboratorId, int organizationId);

        /// <summary>
        /// Deletes the collaborator.
        /// </summary>
        /// <param name="collaboratorId">The collaborator identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        void DeleteCollaborator(int collaboratorId, int organizationId);

        /// <summary>
        ///     Updates the collaborator.
        /// </summary>
        /// <param name="collaboratorToUpdate">The collaborator to update.</param>
        void UpdateCollaborator(collaborator collaboratorToUpdate, int organizationId);

        /// <summary>
        ///     Adds the collaborator.
        /// </summary>
        /// <param name="collaboratorToCreate">The collaborator to create.</param>
        /// <returns></returns>
        collaborator AddCollaborator(collaborator collaboratorToCreate, int organizationId);
    }
}