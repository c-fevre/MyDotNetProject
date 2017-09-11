using System.Collections.Generic;
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

                       //.Include(d => d.applicationlanguage)
                       .Where(d => d.active)
                       .SingleOrDefault(d => d.id == id);
        }

        /// <summary>
        ///     Gets administrator by Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public administrator GetByEmail(string email)
        {
            return this.Table

                       //.Include(d => d.applicationlanguage)
                       .Where(u => u.email == email)
                       .Where(d => d.active)
                       .SingleOrDefault(u => u.active);
        }

        /// <summary>
        ///     Gets all administrators order by lastName
        /// </summary>
        /// <returns></returns>
        public IList<administrator> GetActiveAdministrators()
        {
            return this.Table
                       .Where(u => u.active)
                       .OrderBy(u => u.email)
                       .ToList();
        }
    }
}