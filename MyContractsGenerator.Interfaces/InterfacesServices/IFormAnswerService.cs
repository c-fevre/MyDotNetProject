using System.Collections.Generic;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.Interfaces.InterfacesServices
{
    public interface IFormAnswerService
    {
        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        IList<form_answer> GetAll();

        /// <summary>
        ///     Gets form_answer by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        form_answer GetById(int id);

        /// <summary>
        ///     delete logically the form_answer
        /// </summary>
        /// <param name="formAnswerId"></param>
        void DeleteFormAnswer(int formAnswerId);

        /// <summary>
        ///     Updates the form_answer.
        /// </summary>
        /// <param name="formAnswerToUpdate">The form_answer to update.</param>
        void UpdateFormAnswer(form_answer formAnswerToUpdate);

        /// <summary>
        ///     Adds the form_answer.
        /// </summary>
        /// <param name="formAnswerToCreate">The form_answer to create.</param>
        /// <returns></returns>
        form_answer AddFormAnswer(form_answer formAnswerToCreate);

        /// <summary>
        ///     Gets all for collaborator.
        /// </summary>
        /// <param name="collaboratorId">The collaborator identifier.</param>
        /// <returns></returns>
        IList<form_answer> GetAllForCollaboratorAndRole(int collaboratorId, int rolId);
    }
}