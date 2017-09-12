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
    public class AdministratorRepository : BaseRepository<administrator>, IAdministratorRepository
    {
        public AdministratorRepository(MyContractsGeneratorEntities context) : base(context)
        {
        }

        /// <summary>
        ///     Get an entity by identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <returns>
        ///     the entity or null
        /// </returns>
        administrator IBaseRepository<administrator>.GetById(int id)
        {
            return this.Table
                       .Include(a => a.organization)
                       .Include(a => a.organization.collaborators)
                       .Include(a => a.organization.roles)
                       .Include(a => a.organization.form_answer)
                       //.Include(d => d.applicationlanguage)
                       .Where(d => d.active)
                       .SingleOrDefault(d => d.id == id);
        }

        /// <summary>
        /// Gets the by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public administrator GetByEmail(string email)
        {
            return this.Table
                       .Include(a => a.organization)
                       .Include(a => a.organization.collaborators)
                       .Include(a => a.organization.roles)
                       .Include(a => a.organization.form_answer)
                       //.Include(d => d.applicationlanguage)
                       .Where(u => u.email == email)
                       .Where(u => u.organization.active)
                       .Where(d => d.active)
                       .SingleOrDefault(u => u.active);
        }
        
        /// <summary>
        /// Gets the active administrators.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public IEnumerable<administrator> GetActiveAdministrators(int organizationId)
        {
            return this.Table
                       .Include(a => a.organization)
                       .Include(a => a.organization.collaborators)
                       .Include(a => a.organization.roles)
                       .Include(a => a.organization.form_answer)
                       .Where(u => u.organization_id == organizationId)
                       .Where(u => u.organization.active)
                       .Where(u => u.active)
                       .OrderBy(u => u.email)
                       .ToList();
        }

        public IEnumerable<administrator> GetActiveAdministrators()
        {
            return this.Table
                       .Include(a => a.organization)
                       .Include(a => a.organization.collaborators)
                       .Include(a => a.organization.roles)
                       .Include(a => a.organization.form_answer)
                       .Where(u => u.organization.active)
                       .Where(u => u.active)
                       .OrderBy(u => u.email)
                       .ToList();
        }
    }
}