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
    public class FormAnswerRepository : BaseRepository<form_answer>, IFormAnswerRepository
    {
        public FormAnswerRepository(MyContractsGeneratorEntities context) : base(context)
        {
        }

        /// <summary>
        /// Get an entity by identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <returns>
        /// the entity or null
        /// </returns>
        form_answer IBaseRepository<form_answer>.GetById(int id)
        {
            return this.Table
                .Include(d => d.collaborator)
                .Include(d => d.answers)
                .Include(d => d.form)
                .SingleOrDefault(d => d.id == id);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<form_answer> GetAll()
        {
            return this.Table
                .Include(d => d.collaborator)
                .Include(d => d.answers)
                .Include(d => d.form);
        }
    }
}
