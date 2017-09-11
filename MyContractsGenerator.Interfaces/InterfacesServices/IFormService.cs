using System.Collections.Generic;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.Interfaces.InterfacesServices
{
    public interface IFormService
    {
        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        IList<form> GetAll();

        /// <summary>
        ///     Gets form by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        form GetById(int id);

        /// <summary>
        ///     delete logically the form
        /// </summary>
        /// <param name="formId"></param>
        void DeleteForm(int formId);

        /// <summary>
        ///     Updates the form.
        /// </summary>
        /// <param name="formToUpdate">The form to update.</param>
        void UpdateForm(form formToUpdate);

        /// <summary>
        ///     Adds the form.
        /// </summary>
        /// <param name="formToCreate">The form to create.</param>
        /// <returns></returns>
        form AddForm(form formToCreate);
    }
}