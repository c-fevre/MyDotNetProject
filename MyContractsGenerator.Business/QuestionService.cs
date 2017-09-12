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
    /// <seealso cref="MyContractsGenerator.Interfaces.InterfacesServices.IQuestionService" />
    public class QuestionService : BaseService, IQuestionService
    {
        /// <summary>
        ///     The question repository
        /// </summary>
        private readonly IQuestionRepository questionRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="QuestionService" /> class.
        /// </summary>
        /// <param name="questionRepository">The question repository.</param>
        public QuestionService(IQuestionRepository questionRepository)
        {
            this.questionRepository = questionRepository;
        }

        /// <summary>
        ///     Gets question by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public question GetById(int id)
        {
            return this.questionRepository.GetById(id);
        }

        /// <summary>
        ///     delete logically the question
        /// </summary>
        /// <param name="questionId"></param>
        public void DeleteQuestion(int questionId)
        {
            Requires.ArgumentGreaterThanZero(questionId, "Form Answer Id");
            this.questionRepository.Remove(questionId);

            this.questionRepository.SaveChanges();
        }

        /// <summary>
        ///     Adds the question.
        /// </summary>
        /// <param name="questionToCreate">The question to create.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public question AddQuestion(question questionToCreate)
        {
            Requires.ArgumentNotNull(questionToCreate, "questionToCreate");

            question dbQuestion = this.questionRepository.Add(questionToCreate);
            this.questionRepository.SaveChanges();

            return dbQuestion;
        }

        /// <summary>
        ///     Updates the question.
        /// </summary>
        /// <param name="questionToUpdate">The question to update.</param>
        public void UpdateQuestion(question questionToUpdate)
        {
            var dbQuestion = this.questionRepository.GetById(questionToUpdate.id);
            if (dbQuestion == null)
            {
                return;
            }

            //TODO Bindings ?
            dbQuestion.label = questionToUpdate.label;
            dbQuestion.order = questionToUpdate.order;

            this.questionRepository.Update(dbQuestion);
            this.questionRepository.SaveChanges();
        }

        /// <summary>
        ///     Gets the questions by form identifier.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<question> GetQuestionsByFormId(int formId)
        {
            return this.questionRepository.GetAllByFormId(formId).ToList();
        }
    }
}