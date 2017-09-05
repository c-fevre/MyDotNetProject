using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyContractsGenerator.Domain;
using MyContractsGenerator.WebUI.Models.FormAnswerModels;

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
                Replied = formAnswer.replied
            };
            
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