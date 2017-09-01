using System.Collections.Generic;
using MyContractsGenerator.DAL;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.Interfaces.InterfacesServices
{
    public interface IRoleService
    {
        /// <summary>
        ///     Gets roles by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        role GetById(int id);

        /// <summary>
        ///     Gets roles
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IList<role> GetAllActive();

        /// <summary>
        ///     delete logically the roles
        /// </summary>
        /// <param name="roleId"></param>
        void DeleteRole(int roleId);

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="roleToUpdate">The role to update.</param>
        void UpdateRole(role roleToUpdate);

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="roleToCreate">The role to create.</param>
        /// <returns></returns>
        role AddRole(role roleToCreate);

        /// <summary>
        /// Determines whether [is this label already exists] [the specified label].
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns>
        ///   <c>true</c> if [is this label already exists] [the specified label]; otherwise, <c>false</c>.
        /// </returns>
        bool IsThisLabelAlreadyExists(string label);

        /// <summary>
        /// Determines whether [is this label already exists] [the specified label].
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="currentId">The current id.</param>
        /// <returns>
        ///   <c>true</c> if [is this label already exists] [the specified label]; otherwise, <c>false</c>.
        /// </returns>
        bool IsThisLabelAlreadyExists(string label, int currentId);

        /// <summary>
        /// Affects to role.
        /// </summary>
        /// <param name="editedCollaboratorLinkedRolesIds">The edited collaborator linked roles ids.</param>
        /// <param name="editedCollaboratorId">The edited collaborator identifier.</param>
        void AffectToRole(IEnumerable<int> editedCollaboratorLinkedRolesIds, int editedCollaboratorId);
    }
}
