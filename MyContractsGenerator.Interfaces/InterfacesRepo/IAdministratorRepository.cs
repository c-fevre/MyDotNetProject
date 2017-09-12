using System.Collections.Generic;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.Interfaces.InterfacesRepo
{
    public interface IAdministratorRepository : IBaseRepository<administrator>
    {
        /// <summary>
        ///     Gets Administrator by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        new administrator GetById(int id);

        /// <summary>
        /// Gets the by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        administrator GetByEmail(string email);

        /// <summary>
        /// Gets the active administrators.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        IEnumerable<administrator> GetActiveAdministrators(int organizationId);
        /// <summary>
        /// Gets the active administrators.
        /// </summary>
        /// <returns></returns>
        IEnumerable<administrator> GetActiveAdministrators();
    }
}