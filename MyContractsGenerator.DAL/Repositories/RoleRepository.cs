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
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public role GetById(int id, int organizationId)
        {
            return this.Table
                       .Include(r => r.collaborators)
                       .Include(r => r.form_answer)
                       .Include(r => r.form_answer.Select(c => c.collaborator))
                       .Where(r => r.organization_id.Equals(organizationId))
                       .SingleOrDefault(d => d.id == id);
        }

        /// <summary>
        /// Gets all active.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public IEnumerable<role> GetAllActive(int organizationId)
        {
            return this.Table
                       .Include(r => r.collaborators)
                       .Include(r => r.form_answer)
                       .Include(r => r.form_answer.Select(c => c.collaborator))
                       .Where(r => r.organization_id.Equals(organizationId))
                       .Where(d => d.active);
        }
    }
}