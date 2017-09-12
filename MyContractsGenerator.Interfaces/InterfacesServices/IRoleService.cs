using System.Collections.Generic;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.Interfaces.InterfacesServices
{
    public interface IRoleService
    {
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        role GetById(int id, int organizationId);

        /// <summary>
        /// Gets all active.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        IList<role> GetAllActive(int organizationId);

        /// <summary>
        ///     delete logically the roles
        /// </summary>
        /// <param name="roleId"></param>
        void DeleteRole(int roleId);

        /// <summary>
        ///     Updates the role.
        /// </summary>
        /// <param name="roleToUpdate">The role to update.</param>
        void UpdateRole(role roleToUpdate, int organizationId);

        /// <summary>
        ///     Adds the role.
        /// </summary>
        /// <param name="roleToCreate">The role to create.</param>
        /// <returns></returns>
        role AddRole(role roleToCreate, int organizationId);

        /// <summary>
        /// Determines whether [is this label already exists] [the specified label].
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="currentId">The current identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is this label already exists] [the specified label]; otherwise, <c>false</c>.
        /// </returns>
        bool IsThisLabelAlreadyExists(string label, int currentId, int organizationId);

        /// <summary>
        /// Determines whether [is this label already exists] [the specified label].
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is this label already exists] [the specified label]; otherwise, <c>false</c>.
        /// </returns>
        bool IsThisLabelAlreadyExists(string label, int organizationId);

        /// <summary>
        ///     Affects to role.
        /// </summary>
        /// <param name="editedCollaboratorLinkedRolesIds">The edited collaborator linked roles ids.</param>
        /// <param name="editedCollaboratorId">The edited collaborator identifier.</param>
        void AffectToRole(IEnumerable<int> editedCollaboratorLinkedRolesIds, int editedCollaboratorId, int organizationId);
    }
}