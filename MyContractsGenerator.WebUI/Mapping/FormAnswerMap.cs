using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyContractsGenerator.Core.Enum;
using MyContractsGenerator.Domain;
using MyContractsGenerator.WebUI.Models.FormAnswerModels;
using MyContractsGenerator.WebUI.Models.QuestionModels;

namespace MyContractsGenerator.WebUI.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public static class FormAnswerMap
    {
        #region Domain To Model

        /// <summary>
        ///     Transforms an User to an FormAnswerModel
        /// </summary>
        /// <param name="formAnswer"></param>
        /// <returns></returns>
        private static FormAnswerModel MapItem(form_answer formAnswer)
        {
            if (formAnswer == null)
            {
                return null;
            }

            FormAnswerModel formAnswerModel = new FormAnswerModel
            {
                Id = formAnswer.id,
                LastUpdateTime = formAnswer.last_update,
                Replied = formAnswer.replied,
                QuestionsAnswers = new List<QuestionModel>(),
                FormLabel = formAnswer.form?.label ?? "",
                LastCollaboratorMailTime = formAnswer.last_collaborator_mail_time
        };

            if (formAnswer.answers == null || !formAnswer.answers.Any())
            {
                return formAnswerModel;
            }

            formAnswer.answers.ToList().ForEach(a =>
            {
                QuestionModel questionModel = new QuestionModel
                {
                    Id = a.question_id,
                    Label = a.question.label,
                    Order = a.question.order,
                    Type = EnumEx.GetValueFromDescription<QuestionType.QuestionTypeEnum>(a.question.question_type.label),
                    Value = a.answer_value
                };
                formAnswerModel.QuestionsAnswers.Add(questionModel);
            });

            
            return formAnswerModel;
        }

        /// <summary>
        ///     Transforms a IEnumerable<formAnswers> to a IEnumerable<FormAnswerModel>
        /// </summary>
        /// <param name="formAnswers"></param>
        /// <returns></returns>
        internal static IList<FormAnswerModel> MapItems(IEnumerable<form_answer> formAnswers)
        {
            IList<FormAnswerModel> models = new List<FormAnswerModel>();

            if (formAnswers.Any())
            {
                formAnswers.ToList().ForEach(c =>
                {
                    models.Add(MapItem(c));
                });
            }

            return models;
        }

        #endregion

    }
}