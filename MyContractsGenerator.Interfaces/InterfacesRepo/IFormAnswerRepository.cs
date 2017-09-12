using System.Collections.Generic;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesRepo;

namespace MyContractsGenerator.DAL.Repositories
{
    public interface IFormAnswerRepository : IBaseRepository<form_answer>
    {
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        new form_answer GetById(int id, int organizationId);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        IEnumerable<form_answer> GetAll(int organizationId);

        /// <summary>
        /// Gets all for collaborator and role.
        /// </summary>
        /// <param name="collaboratorId">The collaborator identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        IEnumerable<form_answer> GetAllForCollaboratorAndRole(int collaboratorId, int roleId, int organizationId);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<form_answer> GetAll();
    }
}