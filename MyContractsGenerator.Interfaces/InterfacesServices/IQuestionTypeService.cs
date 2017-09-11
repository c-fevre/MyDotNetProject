using System.Collections.Generic;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.Interfaces.InterfacesServices
{
    public interface IQuestionTypeService
    {
        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        IList<question_type> GetAll();

        /// <summary>
        ///     Gets question_type by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        question_type GetById(int id);

        /// <summary>
        ///     delete logically the question_type
        /// </summary>
        /// <param name="question_typeId"></param>
        void DeleteQuestionType(int question_typeId);

        /// <summary>
        ///     Updates the question_type.
        /// </summary>
        /// <param name="question_typeToUpdate">The question_type to update.</param>
        void UpdateQuestionType(question_type question_typeToUpdate);

        /// <summary>
        ///     Adds the question_type.
        /// </summary>
        /// <param name="question_typeToCreate">The question_type to create.</param>
        /// <returns></returns>
        question_type AddQuestionType(question_type question_typeToCreate);
    }
}