using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MyContractsGenerator.Interfaces.InterfacesRepo;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.DAL.Repositories
{
    /// <summary>
    /// </summary>
    /// <seealso cref="administrator" />
    public class QuestionRepository : BaseRepository<question>, IQuestionRepository
    {
        public QuestionRepository(MyContractsGeneratorEntities context) : base(context)
        {
        }

        /// <summary>
        /// Get an entity by identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <returns>
        /// the entity or null
        /// </returns>
        question IBaseRepository<question>.GetById(int id)
        {
            return this.Table
                .Include(d => d.forms)
                .Include(d => d.answers)
                .SingleOrDefault(d => d.id == id);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<question> GetAll()
        {
            return this.Table
                .Include(d => d.forms)
                .Include(d => d.answers);
        }
    }
}
