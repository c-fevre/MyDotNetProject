using System.Collections.Generic;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.Interfaces.InterfacesServices
{
    public interface IAnswerService
    {
        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        IList<answer> GetAll();

        /// <summary>
        ///     Gets answer by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        answer GetById(int id);

        /// <summary>
        ///     delete logically the answer
        /// </summary>
        /// <param name="answerId"></param>
        void DeleteAnswer(int answerId);

        /// <summary>
        ///     Updates the answer.
        /// </summary>
        /// <param name="answerToUpdate">The answer to update.</param>
        void UpdateAnswer(answer answerToUpdate);

        /// <summary>
        ///     Adds the answer.
        /// </summary>
        /// <param name="answerToCreate">The answer to create.</param>
        /// <returns></returns>
        answer AddAnswer(answer answerToCreate);

        /// <summary>
        ///     Adds the answers.
        /// </summary>
        /// <param name="answers">The answers.</param>
        void AddAnswers(IList<answer> answers);
    }
}