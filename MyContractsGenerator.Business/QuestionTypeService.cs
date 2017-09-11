using System.Collections.Generic;
using System.Linq;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.DAL.Repositories;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesServices;

namespace MyContractsGenerator.Business
{
    /// <summary>
    /// </summary>
    /// <seealso cref="MyContractsGenerator.Business.BaseService" />
    /// <seealso cref="MyContractsGenerator.Interfaces.InterfacesServices.IQuestionTypeService" />
    public class QuestionTypeService : BaseService, IQuestionTypeService
    {
        /// <summary>
        ///     The questionType repository
        /// </summary>
        private readonly IQuestionTypeRepository questionTypeRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="QuestionTypeService" /> class.
        /// </summary>
        /// <param name="questionTypeRepository">The questionType repository.</param>
        public QuestionTypeService(IQuestionTypeRepository questionTypeRepository)
        {
            this.questionTypeRepository = questionTypeRepository;
        }

        /// <summary>
        ///     Gets questionType by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public question_type GetById(int id)
        {
            return this.questionTypeRepository.GetById(id);
        }

        /// <summary>
        ///     delete logically the questionType
        /// </summary>
        /// <param name="questionTypeId"></param>
        public void DeleteQuestionType(int questionTypeId)
        {
            Requires.ArgumentGreaterThanZero(questionTypeId, "QuestionType Id");
            this.questionTypeRepository.Remove(questionTypeId);

            this.questionTypeRepository.SaveChanges();
        }

        /// <summary>
        ///     Adds the questionType.
        /// </summary>
        /// <param name="questionTypeToCreate">The questionType to create.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public question_type AddQuestionType(question_type questionTypeToCreate)
        {
            Requires.ArgumentNotNull(questionTypeToCreate, "questionTypeToCreate");

            question_type dbQuestionType = this.questionTypeRepository.Add(questionTypeToCreate);
            this.questionTypeRepository.SaveChanges();

            return dbQuestionType;
        }

        /// <summary>
        ///     Updates the questionType.
        /// </summary>
        /// <param name="questionTypeToUpdate">The questionType to update.</param>
        public void UpdateQuestionType(question_type questionTypeToUpdate)
        {
            var dbQuestionType = this.questionTypeRepository.GetById(questionTypeToUpdate.id);
            if (dbQuestionType == null)
            {
                return;
            }

            //TODO Bindings ?
            dbQuestionType.label = questionTypeToUpdate.label;

            this.questionTypeRepository.Update(dbQuestionType);
            this.questionTypeRepository.SaveChanges();
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<question_type> GetAll()
        {
            return this.questionTypeRepository.GetAll().ToList();
        }
    }
}