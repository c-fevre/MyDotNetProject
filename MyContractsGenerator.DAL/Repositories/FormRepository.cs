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
    public class FormRepository : BaseRepository<form>, IFormRepository
    {
        public FormRepository(MyContractsGeneratorEntities context) : base(context)
        {
        }

        /// <summary>
        /// Get an entity by identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <returns>
        /// the entity or null
        /// </returns>
        public new form GetById(int id)
        {
            return this.Table
                .Include(d => d.roles)
                .Include(d => d.questions)
                .Include(d => d.questions.Select(q => q.question_type))
                .Include(d => d.form_answer)
                .SingleOrDefault(d => d.id == id);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<form> GetAll()
        {
            return this.Table
                .Include(d => d.roles)
                .Include(d => d.questions)
                .Include(d => d.form_answer);
        }
    }
}
