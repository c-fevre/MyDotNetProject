using System.Collections.Generic;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.Interfaces.InterfacesServices
{
    public interface IFormAnswerService
    {
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="organizationId">The organizationId.</param>
        /// <returns></returns>
        IList<form_answer> GetAll(int organizationId);

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        form_answer GetById(int id, int organizationId);

        /// <summary>
        /// Deletes the form answer.
        /// </summary>
        /// <param name="formAnswerId">The form answer identifier.</param>
        void DeleteFormAnswer(int formAnswerId);

        /// <summary>
        /// Updates the form answer.
        /// </summary>
        /// <param name="formAnswerToUpdate">The form answer to update.</param>
        /// <param name="organizationId">The organization identifier.</param>
        void UpdateFormAnswer(form_answer formAnswerToUpdate, int organizationId);

        /// <summary>
        /// Adds the form answer.
        /// </summary>
        /// <param name="formAnswerToCreate">The form answer to create.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        form_answer AddFormAnswer(form_answer formAnswerToCreate, int organizationId);

        /// <summary>
        /// Gets all for collaborator and role.
        /// </summary>
        /// <param name="collaboratorId">The collaborator identifier.</param>
        /// <param name="rolId">The rol identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        IList<form_answer> GetAllForCollaboratorAndRole(int collaboratorId, int rolId, int organizationId);

        /// <summary>
        /// Gets all active.
        /// </summary>
        IList<form_answer> GetAllActive();
    }
}