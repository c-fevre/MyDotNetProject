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
    public class AdministratorRepository : BaseRepository<administrator>, IAdministratorRepository
    {
        public AdministratorRepository(MyContractsGeneratorEntities context) : base(context)
        {
        }
        
        /// <summary>
        /// Get an entity by identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <returns>
        /// the entity or null
        /// </returns>
        administrator IBaseRepository<administrator>.GetById(int id)
        {
            return this.Table
                //.Include(d => d.applicationlanguage)
                .SingleOrDefault(d => d.id == id);
        }

        /// <summary>
        /// Gets administrator by Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public administrator GetByEmail(string email)
        {
            return this.Table
                       //.Include(d => d.applicationlanguage)
                       .Where(u => u.email == email)
                       .SingleOrDefault(u => u.active);
        }

        /// <summary>
        /// Gets all administrators order by lastName
        /// </summary>
        /// <returns></returns>
        public IList<administrator> GetActiveAdministrators()
        {
            return this.Table
                       .Where(u => u.active)
                       .OrderBy(u => u.login)
                       .ToList();
        }

        /// <summary>
        /// Gets administrator by login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public administrator GetByLogin(string login)
        {
            return this.Table
                       .Where(u => u.login == login)
                       .SingleOrDefault(u => u.active);
        }
    }
}
