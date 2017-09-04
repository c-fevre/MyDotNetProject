using System.Collections.Generic;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesRepo;

namespace MyContractsGenerator.DAL.Repositories
{
    public interface IQuestionTypeRepository : IBaseRepository<question_type>
    {
        /// <summary>
        ///     Gets Administrator by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        new question_type GetById(int id);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<question_type> GetAll();
    }
}
