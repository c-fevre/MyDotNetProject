using System.Collections.Generic;
using System.Linq;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.DAL.Repositories;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesRepo;
using MyContractsGenerator.Interfaces.InterfacesServices;

namespace MyContractsGenerator.Business
{
    /// <summary>
    /// </summary>
    /// <seealso cref="MyContractsGenerator.Business.BaseService" />
    /// <seealso cref="MyContractsGenerator.Interfaces.InterfacesServices.IFormService" />
    public class FormService : BaseService, IFormService
    {
        /// <summary>
        /// The form repository
        /// </summary>
        private readonly IFormRepository formRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormService"/> class.
        /// </summary>
        /// <param name="formRepository">The form repository.</param>
        public FormService(IFormRepository formRepository)
        {
            this.formRepository = formRepository;
        }
        
        /// <summary>
        /// Gets form by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public form GetById(int id)
        {
            return this.formRepository.GetById(id);
        }
        
        /// <summary>
        /// delete logically the form
        /// </summary>
        /// <param name="formId"></param>
        public void DeleteForm(int formId)
        {
            Requires.ArgumentGreaterThanZero(formId, "Form Answer Id");
            this.formRepository.Remove(formId);
            
            this.formRepository.SaveChanges();
        }

        /// <summary>
        /// Adds the form.
        /// </summary>
        /// <param name="formToCreate">The form to create.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public form AddForm(form formToCreate)
        {
            Requires.ArgumentNotNull(formToCreate, "formToCreate");

            form dbForm = this.formRepository.Add(formToCreate);
            this.formRepository.SaveChanges();

            return dbForm;
        }

        /// <summary>
        /// Updates the form.
        /// </summary>
        /// <param name="formToUpdate">The form to update.</param>
        public void UpdateForm(form formToUpdate)
        {
            var dbForm = this.formRepository.GetById(formToUpdate.id);
            if (dbForm == null)
            {
                return;
            }

            //TODO Bindings ?
            dbForm.label = formToUpdate.label;

            this.formRepository.Update(dbForm);
            this.formRepository.SaveChanges();
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<form> GetAll()
        {
            return this.formRepository.GetAll().ToList();
        }
    }
}
