﻿using System.Collections.Generic;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesRepo;

namespace MyContractsGenerator.DAL.Repositories
{
    public interface IFormAnswerRepository : IBaseRepository<form_answer>
    {
        /// <summary>
        ///     Gets Administrator by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        new form_answer GetById(int id);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<form_answer> GetAll();

        /// <summary>
        /// Gets all for collaborator and role.
        /// </summary>
        /// <param name="collaboratorId">The collaborator identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        IEnumerable<form_answer> GetAllForCollaboratorAndRole(int collaboratorId, int roleId);
    }
}
