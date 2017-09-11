using System.Collections.Generic;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesRepo;

namespace MyContractsGenerator.DAL.Repositories
{
    public interface IAnswerRepository : IBaseRepository<answer>
    {
        /// <summary>
        ///     Gets Administrator by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        new answer GetById(int id);

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<answer> GetAll();
    }
}