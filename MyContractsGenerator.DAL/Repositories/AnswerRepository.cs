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
    public class AnswerRepository : BaseRepository<answer>, IAnswerRepository
    {
        public AnswerRepository(MyContractsGeneratorEntities context) : base(context)
        {
        }

        /// <summary>
        ///     Get an entity by identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <returns>
        ///     the entity or null
        /// </returns>
        answer IBaseRepository<answer>.GetById(int id)
        {
            return this.Table
                       .Include(d => d.question)
                       .Include(d => d.form_answer)
                       .SingleOrDefault(d => d.id == id);
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<answer> GetAll()
        {
            return this.Table
                       .Include(d => d.question)
                       .Include(d => d.form_answer);
        }
    }
}