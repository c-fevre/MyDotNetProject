﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesRepo;

namespace MyContractsGenerator.DAL.Repositories
{
    /// <summary>
    /// </summary>
    /// <seealso cref="administrator" />
    public class OrganizationRepository : BaseRepository<organization>, IOrganizationRepository
    {
        public OrganizationRepository(MyContractsGeneratorEntities context) : base(context)
        {
        }

        /// <summary>
        ///     Get an entity by identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <returns>
        ///     the entity or null
        /// </returns>
        organization IBaseRepository<organization>.GetById(int id)
        {
            return this.Table
                       .Include(d => d.administrators)
                       .Include(d => d.collaborators)
                       .Include(d => d.roles)
                       .Where(o => o.active)
                       .SingleOrDefault(d => d.id == id);
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<organization> GetAll()
        {
            return this.Table
                       .Include(d => d.administrators)
                       .Include(d => d.collaborators)
                       .Include(d => d.roles)
                       .Where(o => o.active);
        }
    }
}