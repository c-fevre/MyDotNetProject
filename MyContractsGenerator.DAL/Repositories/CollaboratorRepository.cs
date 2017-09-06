using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// Get an entity by identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <returns>
        /// the entity or null
        /// </returns>
        public new collaborator GetById(int id)
        {
            return this.Table
                .Include(d => d.roles)
                .Include(u => u.form_answer)
                .Include(c => c.form_answer.Select(fa => fa.answers))
                .Include(c => c.form_answer.Select(fa => fa.form))
                .Where(u => u.active)
                .SingleOrDefault(d => d.id == id);
        }

        /// <summary>
        /// Gets administrator by Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public collaborator GetByEmail(string email)
        {
            return this.Table
                       .Include(d => d.roles)
                       .Include(u => u.form_answer)
                       .Where(u => u.email == email)
                       .SingleOrDefault(u => u.active);
        }

        public IEnumerable<collaborator> GetAllActive()
        {
            return this.Table
                       //.Include(d => d.applicationlanguage)
                       .Include(d => d.roles)
                       .Include(u => u.form_answer)
                       .Where(u => u.active);
        }
    }
}
