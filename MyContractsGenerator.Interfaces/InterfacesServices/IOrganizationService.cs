using System.Collections.Generic;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.Interfaces.InterfacesServices
{
    public interface IOrganizationService
    {
        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        IList<organization> GetAll();

        /// <summary>
        ///     Gets organization by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        organization GetById(int id);

        /// <summary>
        ///     delete logically the organization
        /// </summary>
        /// <param name="organizationId"></param>
        void DeleteOrganization(int organizationId);

        /// <summary>
        ///     Updates the organization.
        /// </summary>
        /// <param name="organizationToUpdate">The organization to update.</param>
        void UpdateOrganization(organization organizationToUpdate);

        /// <summary>
        ///     Adds the organization.
        /// </summary>
        /// <param name="organizationToCreate">The organization to create.</param>
        /// <returns></returns>
        organization AddOrganization(organization organizationToCreate);

        /// <summary>
        /// Determines whether [is this label already exists] [the specified label].
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is this label already exists] [the specified label]; otherwise, <c>false</c>.
        /// </returns>
        bool IsThisLabelAlreadyExists(string label, int id);

        /// <summary>
        /// Determines whether /[is this label already exists] [the specified label].
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns>
        ///   <c>true</c> if [is this label already exists] [the specified label]; otherwise, <c>false</c>.
        /// </returns>
        bool IsThisLabelAlreadyExists(string label);
    }
}