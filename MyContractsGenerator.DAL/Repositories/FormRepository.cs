﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        ///     Get an entity by identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <returns>
        ///     the entity or null
        /// </returns>
        public new form GetById(int id)
        {
            return this.Table
                       .Include(d => d.roles)
                       .Include(d => d.form_answer)
                       .Include(d => d.form_answer.Select(fa => fa.role))
                       .Include(d => d.form_answer.Select(fa => fa.collaborator))
                       .Include(d => d.form_answer.Select(fa => fa.collaborator).Select(c => c.roles))
                       .Include(d => d.form_answer.Select(fa => fa.organization))
                       .Include(d => d.questions)
                       .Include(d => d.questions.Select(q => q.question_type))
                       .SingleOrDefault(d => d.id.Equals(id));
        }

        /// <summary>
        ///     Gets all.
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