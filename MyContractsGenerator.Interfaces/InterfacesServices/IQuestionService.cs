using System.Collections.Generic;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.Interfaces.InterfacesServices
{
    public interface IQuestionService
    {
        /// <summary>
        ///     Gets question by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        question GetById(int id);

        /// <summary>
        ///     delete logically the question
        /// </summary>
        /// <param name="questionId"></param>
        void DeleteQuestion(int questionId);

        /// <summary>
        ///     Updates the question.
        /// </summary>
        /// <param name="questionToUpdate">The question to update.</param>
        void UpdateQuestion(question questionToUpdate);

        /// <summary>
        ///     Adds the question.
        /// </summary>
        /// <param name="questionToCreate">The question to create.</param>
        /// <returns></returns>
        question AddQuestion(question questionToCreate);

        /// <summary>
        ///     Gets the questions by form identifier.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <returns></returns>
        IEnumerable<question> GetQuestionsByFormId(int formId);
    }
}