using System;
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
    /// <seealso cref="MyContractsGenerator.Interfaces.InterfacesServices.IFormAnswerService" />
    public class FormAnswerService : BaseService, IFormAnswerService
    {
        /// <summary>
        ///     The formAnswer repository
        /// </summary>
        private readonly IFormAnswerRepository formAnswerRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FormAnswerService" /> class.
        /// </summary>
        /// <param name="formAnswerRepository">The formAnswer repository.</param>
        public FormAnswerService(IFormAnswerRepository formAnswerRepository)
        {
            this.formAnswerRepository = formAnswerRepository;
        }

        /// <summary>
        ///     Gets formAnswer by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public form_answer GetById(int id)
        {
            return this.formAnswerRepository.GetById(id);
        }

        /// <summary>
        ///     delete logically the formAnswer
        /// </summary>
        /// <param name="formAnswerId"></param>
        public void DeleteFormAnswer(int formAnswerId)
        {
            Requires.ArgumentGreaterThanZero(formAnswerId, "Form Answer Id");
            this.formAnswerRepository.Remove(formAnswerId);

            this.formAnswerRepository.SaveChanges();
        }

        /// <summary>
        ///     Adds the formAnswer.
        /// </summary>
        /// <param name="formAnswerToCreate">The formAnswer to create.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public form_answer AddFormAnswer(form_answer formAnswerToCreate)
        {
            Requires.ArgumentNotNull(formAnswerToCreate, "formAnswerToCreate");

            form_answer dbFormAnswer = this.formAnswerRepository.Add(formAnswerToCreate);
            this.formAnswerRepository.SaveChanges();

            return dbFormAnswer;
        }

        /// <summary>
        ///     Updates the formAnswer.
        /// </summary>
        /// <param name="formAnswerToUpdate">The formAnswer to update.</param>
        public void UpdateFormAnswer(form_answer formAnswerToUpdate)
        {
            var dbFormAnswer = this.formAnswerRepository.GetById(formAnswerToUpdate.id);
            if (dbFormAnswer == null)
            {
                return;
            }

            //TODO Bindings ?
            dbFormAnswer.last_update = DateTime.Now;
            dbFormAnswer.replied = formAnswerToUpdate.replied;

            this.formAnswerRepository.Update(dbFormAnswer);
            this.formAnswerRepository.SaveChanges();
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<form_answer> GetAll()
        {
            return this.formAnswerRepository.GetAll().ToList();
        }

        /// <summary>
        ///     Gets all for collaborator and role.
        /// </summary>
        /// <param name="collaboratorId">The collaborator identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public IList<form_answer> GetAllForCollaboratorAndRole(int collaboratorId, int roleId)
        {
            return this.formAnswerRepository.GetAllForCollaboratorAndRole(collaboratorId, roleId).ToList();
        }
    }
}