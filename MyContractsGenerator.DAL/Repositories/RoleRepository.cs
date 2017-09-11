using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.DAL.Repositories
{
    public class RoleRepository : BaseRepository<role>, IRoleRepository
    {
        public RoleRepository(MyContractsGeneratorEntities context) : base(context)
        {
        }

        /// <summary>
        ///     Get an entity by identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <returns>
        ///     the entity or null
        /// </returns>
        public override role GetById(int id)
        {
            return this.Table
                       .Include(r => r.collaborators)
                       .Include(r => r.form_answer)
                       .Include(r => r.form_answer.Select(c => c.collaborator))
                       .Where(d => d.active)
                       .SingleOrDefault(d => d.id == id);
        }

        /// <summary>
        ///     Gets all active.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<role> GetAllActive()
        {
            return this.Table
                       .Include(r => r.collaborators)
                       .Include(r => r.form_answer)
                       .Include(r => r.form_answer.Select(c => c.collaborator))
                       .Where(d => d.active);
        }
    }
}