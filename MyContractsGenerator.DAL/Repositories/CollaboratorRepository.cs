using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesRepo;

namespace MyContractsGenerator.DAL.Repositories
{
    public class CollaboratorRepository : BaseRepository<collaborator>, ICollaboratorRepository
    {
        public CollaboratorRepository(MyContractsGeneratorEntities context) : base(context)
        {
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public new collaborator GetById(int id, int organizationId)
        {
            return this.Table
                       .Include(d => d.organization)
                       .Include(d => d.roles)
                       .Include(u => u.form_answer)
                       .Include(u => u.form_answer.Select(fa => fa.role))
                       .Include(c => c.form_answer.Select(fa => fa.answers))
                       .Include(c => c.form_answer.Select(fa => fa.form))
                       .Where(u => u.organization_id == organizationId)
                       .Where(u => u.organization.active)
                       .Where(u => u.active)
                       .SingleOrDefault(d => d.id == id);
        }

        /// <summary>
        /// Gets the by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public collaborator GetByEmail(string email, int organizationId)
        {
            return this.Table
                       .Include(d => d.organization)
                       .Include(d => d.roles)
                       .Include(u => u.form_answer)
                       .Include(u => u.form_answer.Select(fa => fa.role))
                       .Where(u => u.organization_id == organizationId)
                       .Where(u => u.organization.active)
                       .Where(u => u.email == email)
                       .SingleOrDefault(u => u.active);
        }

        public IEnumerable<collaborator> GetAllActive(int organizationId)
        {
            return this.Table
                       .Include(d => d.organization)
                       .Include(d => d.roles)
                       .Include(u => u.form_answer)
                       .Include(u => u.form_answer.Select(fa => fa.role))
                       .Where(u => u.organization_id == organizationId)
                       .Where(u => u.organization.active)
                       .Where(u => u.active);
        }
    }
}