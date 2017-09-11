using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesRepo;

namespace MyContractsGenerator.DAL.Repositories
{
    /// <summary>
    /// </summary>
    /// <seealso cref="administrator" />
    public class QuestionTypeRepository : BaseRepository<question_type>, IQuestionTypeRepository
    {
        public QuestionTypeRepository(MyContractsGeneratorEntities context) : base(context)
        {
        }

        /// <summary>
        ///     Get an entity by identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <returns>
        ///     the entity or null
        /// </returns>
        question_type IBaseRepository<question_type>.GetById(int id)
        {
            return this.Table
                       .Include(d => d.questions)
                       .SingleOrDefault(d => d.id == id);
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<question_type> GetAll()
        {
            return this.Table
                       .Include(d => d.questions);
        }
    }
}