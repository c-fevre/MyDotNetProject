using MyContractsGenerator.WebUI.Models.BaseModels;

namespace MyContractsGenerator.WebUI.Models.AnswerModels
{
    /// <summary>
    /// </summary>
    /// <seealso cref="MyContractsGenerator.WebUI.Models.BaseModels.BaseModel" />
    public class AnswerModel : BaseModel
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the form identifier.
        /// </summary>
        /// <value>
        ///     The form identifier.
        /// </value>
        public int FormAnswerId { get; set; }

        /// <summary>
        ///     Gets or sets the question identifier.
        /// </summary>
        /// <value>
        ///     The question identifier.
        /// </value>
        public int QuestionId { get; set; }

        /// <summary>
        ///     Gets or sets the answer value.
        /// </summary>
        /// <value>
        ///     The answer value.
        /// </value>
        public string AnswerValue { get; set; }
    }
}