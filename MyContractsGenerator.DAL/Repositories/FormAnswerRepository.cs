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
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public new form_answer GetById(int id, int organizationId)
        {
            return this.Table
                       .Include(d => d.organization)
                       .Include(d => d.role)
                       .Include(d => d.collaborator)
                       .Include(d => d.answers)
                       .Include(d => d.form)
                       .Where(o => o .organization_id == organizationId)
                       .Where(u => u.organization.active)
                       .SingleOrDefault(d => d.id == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public IEnumerable<form_answer> GetAll(int organizationId)
        {
            return this.Table
                       .Include(d => d.organization)
                       .Include(d => d.role)
                       .Include(d => d.collaborator)
                       .Include(d => d.answers)
                       .Include(d => d.form)
                       .Where(o => o.organization_id == organizationId)
                       .Where(u => u.organization.active);
        }

        /// <summary>
        /// Gets all for collaborator and role.
        /// </summary>
        /// <param name="collaboratorId">The collaborator identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public IEnumerable<form_answer> GetAllForCollaboratorAndRole(int collaboratorId, int roleId, int organizationId)
        {
            return this.Table
                       .Include(d => d.organization)
                       .Include(d => d.role)
                       .Include(fa => fa.form)
                       .Include(fa => fa.answers)
                       .Include(fa => fa.answers.Select(a => a.question))
                       .Include(fa => fa.answers.Select(a => a.question).Select(q => q.question_type))
                       .Where(o => o.organization_id == organizationId)
                       .Where(u => u.organization.active)
                       .Where(fa => fa.collaborator_id.Equals(collaboratorId) && fa.role.id.Equals(roleId));
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<form_answer> GetAll()
        {
            return this.Table
                       .Include(d => d.organization)
                       .Include(d => d.role)
                       .Include(fa => fa.form)
                       .Include(fa => fa.answers)
                       .Include(fa => fa.answers.Select(a => a.question))
                       .Include(fa => fa.answers.Select(a => a.question).Select(q => q.question_type))
                       .Where(u => u.organization.active);
        }
    }
}