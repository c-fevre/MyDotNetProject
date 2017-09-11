using System.Collections.Generic;
using System.Linq;
using MyContractsGenerator.Domain;
using MyContractsGenerator.WebUI.Models.AnswerModels;

namespace MyContractsGenerator.WebUI.Mapping
{
    public static class AnswerMap
    {
        /// <summary>
        ///     Models to data map.
        /// </summary>
        /// <param name="questionsAnswers">The questions answers.</param>
        /// <returns></returns>
        public static IList<answer> ModelToEntitieaMap(IList<AnswerModel> questionsAnswers)
        {
            IList<answer> answers = new List<answer>();

            questionsAnswers.ToList().ForEach(qa =>
            {
                answer newAnswer = new answer
                {
                    form_answer_id = qa.FormAnswerId,
                    question_id = qa.QuestionId,
                    answer_value = qa.AnswerValue
                };
                answers.Add(newAnswer);
            });

            return answers;
        }

        #region Domain To Model

        /// <summary>
        ///     Transforms an User to an answerModel
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        internal static AnswerModel MapItem(answer answer)
        {
            if (answer == null)
            {
                return null;
            }

            AnswerModel answerModel = new AnswerModel
            {
                Id = answer.id,
                FormAnswerId = answer.form_answer_id,
                QuestionId = answer.question_id,
                AnswerValue = answer.answer_value
            };

            return answerModel;
        }

        /// <summary>
        ///     Maps the items.
        /// </summary>
        /// <param name="answers">The answers.</param>
        /// <returns></returns>
        internal static IList<AnswerModel> MapItems(IEnumerable<answer> answers)
        {
            var enumerable = answers as IList<answer> ?? answers.ToList();

            return !enumerable.Any() ? new List<AnswerModel>() : enumerable.Select(MapItem).ToList();
        }

        #endregion
    }
}