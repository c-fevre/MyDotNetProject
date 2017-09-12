using System.Collections.Generic;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesRepo;

namespace MyContractsGenerator.DAL.Repositories
{
    public interface IOrganizationRepository : IBaseRepository<organization>
    {
        /// <summary>
        ///     Gets Administrator by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        new organization GetById(int id);

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<organization> GetAll();
    }
}