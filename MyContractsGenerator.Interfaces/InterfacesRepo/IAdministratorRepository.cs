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
        ///     Gets administrator by Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        administrator GetByEmail(string email);

        /// <summary>
        ///     Gets all administrators order by lastName
        /// </summary>
        /// <returns></returns>
        IList<administrator> GetActiveAdministrators();
    }
}