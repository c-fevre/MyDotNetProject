using System.Collections.Generic;
using System.Linq;
using MyContractsGenerator.Core.Enum;
using MyContractsGenerator.Domain;
using MyContractsGenerator.WebUI.Models.QuestionModels;

namespace MyContractsGenerator.WebUI.Mapping
{
    public class QuestionMap
    {
        #region Domain To Model

        /// <summary>
        ///     Transforms an User to an QuestionModel
        /// </summary>
        /// <param name="dbQuestion"></param>
        /// <returns></returns>
        private static QuestionModel MapItem(question dbQuestion)
        {
            if (dbQuestion == null)
            {
                return null;
            }

            QuestionModel questionModel = new QuestionModel
            {
                Id = dbQuestion.id,
                Label = dbQuestion.label,
                Order = dbQuestion.order,
                Type = EnumEx.GetValueFromDescription<QuestionType.QuestionTypeEnum>(dbQuestion.question_type.label),
                Value = string.Empty
            };

            return questionModel;
        }

        /// <summary>
        ///     Transforms a IEnumerable<questions> to a IEnumerable<QuestionModel>
        /// </summary>
        /// <param name="questions"></param>
        /// <returns></returns>
        internal static IList<QuestionModel> MapItems(IEnumerable<question> questions)
        {
            IList<QuestionModel> models = new List<QuestionModel>();

            if (questions.Any())
            {
                questions.ToList().ForEach(c => { models.Add(MapItem(c)); });
            }

            return models;
        }

        #endregion
    }
}