using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyContractsGenerator.DAL;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesRepo;

namespace MyContractsGenerator.DAL.Repositories
{
    public interface IRoleRepository : IBaseRepository<role>
    {
        /// <summary>
        ///     Gets Administrator by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        new role GetById(int id);

        /// <summary>
        /// Gets all active.
        /// </summary>
        /// <returns></returns>
        IEnumerable<role> GetAllActive();
    }
}
