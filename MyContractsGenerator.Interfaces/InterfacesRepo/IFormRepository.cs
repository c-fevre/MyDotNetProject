using System.Collections.Generic;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesRepo;

namespace MyContractsGenerator.DAL.Repositories
{
    public interface IFormRepository : IBaseRepository<form>
    {
        /// <summary>
        ///     Gets Administrator by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        new form GetById(int id);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<form> GetAll();
    }
}
