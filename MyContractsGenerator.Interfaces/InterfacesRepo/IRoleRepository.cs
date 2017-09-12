using System.Collections.Generic;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesRepo;

namespace MyContractsGenerator.DAL.Repositories
{
    public interface IRoleRepository : IBaseRepository<role>
    {
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        new role GetById(int id, int organizationId);

        /// <summary>
        /// Gets all active.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        IEnumerable<role> GetAllActive(int organizationId);
    }
}