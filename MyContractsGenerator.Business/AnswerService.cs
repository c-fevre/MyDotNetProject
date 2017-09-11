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
    /// <seealso cref="MyContractsGenerator.Interfaces.InterfacesServices.IAnswerService" />
    public class AnswerService : BaseService, IAnswerService
    {
        /// <summary>
        ///     The answer repository
        /// </summary>
        private readonly IAnswerRepository answerRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AnswerService" /> class.
        /// </summary>
        /// <param name="answerRepository">The answer repository.</param>
        public AnswerService(IAnswerRepository answerRepository)
        {
            this.answerRepository = answerRepository;
        }

        /// <summary>
        ///     Gets answer by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public answer GetById(int id)
        {
            return this.answerRepository.GetById(id);
        }

        /// <summary>
        ///     delete logically the answer
        /// </summary>
        /// <param name="answerId"></param>
        public void DeleteAnswer(int answerId)
        {
            Requires.ArgumentGreaterThanZero(answerId, "Answer Id");
            this.answerRepository.Remove(answerId);

            this.answerRepository.SaveChanges();
        }

        /// <summary>
        ///     Adds the answer.
        /// </summary>
        /// <param name="answerToCreate">The answer to create.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public answer AddAnswer(answer answerToCreate)
        {
            Requires.ArgumentNotNull(answerToCreate, "answerToCreate");

            answer dbAnswer = this.answerRepository.Add(answerToCreate);
            this.answerRepository.SaveChanges();

            return dbAnswer;
        }

        /// <summary>
        ///     Updates the answer.
        /// </summary>
        /// <param name="answerToUpdate">The answer to update.</param>
        public void UpdateAnswer(answer answerToUpdate)
        {
            var dbAnswer = this.answerRepository.GetById(answerToUpdate.id);
            if (dbAnswer == null)
            {
                return;
            }

            //TODO Bindings ?
            dbAnswer.answer_value = answerToUpdate.answer_value;

            this.answerRepository.Update(dbAnswer);
            this.answerRepository.SaveChanges();
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<answer> GetAll()
        {
            return this.answerRepository.GetAll().ToList();
        }

        /// <summary>
        ///     Adds the answers.
        /// </summary>
        /// <param name="answers">The answers.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void AddAnswers(IList<answer> answers)
        {
            Requires.ArgumentNotNull(answers, "answers");

            answers.ToList().ForEach(a => { this.answerRepository.Add(a); });

            this.answerRepository.SaveChanges();
        }
    }
}