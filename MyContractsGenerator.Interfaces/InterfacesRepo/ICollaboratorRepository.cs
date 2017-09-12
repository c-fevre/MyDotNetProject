using System.Collections.Generic;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.Interfaces.InterfacesRepo
{
    public interface ICollaboratorRepository : IBaseRepository<collaborator>
    {
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        new collaborator GetById(int id, int organizationId);

        /// <summary>
        /// Gets the by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        collaborator GetByEmail(string email, int organizationId);

        /// <summary>
        /// Gets all active.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        IEnumerable<collaborator> GetAllActive(int organizationId);
    }
}