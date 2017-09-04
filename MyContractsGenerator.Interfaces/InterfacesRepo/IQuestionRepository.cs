using System.Collections.Generic;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesRepo;

namespace MyContractsGenerator.DAL.Repositories
{
    public interface IQuestionRepository : IBaseRepository<question>
    {
        /// <summary>
        ///     Gets Administrator by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        new question GetById(int id);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<question> GetAll();
    }
}
