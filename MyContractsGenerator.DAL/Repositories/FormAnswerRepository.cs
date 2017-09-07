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
    public class FormAnswerRepository : BaseRepository<form_answer>, IFormAnswerRepository
    {
        public FormAnswerRepository(MyContractsGeneratorEntities context) : base(context)
        {
        }

        /// <summary>
        ///     Get an entity by identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <returns>
        ///     the entity or null
        /// </returns>
        form_answer IBaseRepository<form_answer>.GetById(int id)
        {
            return this.Table
                       .Include(d => d.role)
                       .Include(d => d.collaborator)
                       .Include(d => d.answers)
                       .Include(d => d.form)
                       .SingleOrDefault(d => d.id == id);
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<form_answer> GetAll()
        {
            return this.Table
                       .Include(d => d.role)
                       .Include(d => d.collaborator)
                       .Include(d => d.answers)
                       .Include(d => d.form);
        }

        /// <summary>
        ///     Gets all for collaborator.
        /// </summary>
        /// <param name="collaboratorId">The collaborator identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<form_answer> GetAllForCollaboratorAndRole(int collaboratorId, int roleId)
        {
            return this.Table
                       .Include(d => d.role)
                       .Include(fa => fa.form)
                       .Include(fa => fa.answers)
                       .Include(fa => fa.answers.Select(a => a.question))
                       .Include(fa => fa.answers.Select(a => a.question).Select(q => q.question_type))
                       .Where(fa => fa.collaborator_id.Equals(collaboratorId) && fa.role.id.Equals(roleId));
        }
    }
}
