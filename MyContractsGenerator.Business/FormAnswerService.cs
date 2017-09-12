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
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public form_answer GetById(int id, int organizationId)
        {
            return this.formAnswerRepository.GetById(id, organizationId);
        }

        /// <summary>
        /// Deletes the form answer.
        /// </summary>
        /// <param name="formAnswerId">The form answer identifier.</param>
        public void DeleteFormAnswer(int formAnswerId)
        {
            Requires.ArgumentGreaterThanZero(formAnswerId, "Form Answer Id");
            this.formAnswerRepository.Remove(formAnswerId);

            this.formAnswerRepository.SaveChanges();
        }

        /// <summary>
        /// Adds the form answer.
        /// </summary>
        /// <param name="formAnswerToCreate">The form answer to create.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public form_answer AddFormAnswer(form_answer formAnswerToCreate, int organizationId)
        {
            Requires.ArgumentNotNull(formAnswerToCreate, "formAnswerToCreate");
            Requires.ArgumentNotNull(organizationId, "organizationId");

            formAnswerToCreate.collaborator_id = organizationId;

            form_answer dbFormAnswer = this.formAnswerRepository.Add(formAnswerToCreate);
            this.formAnswerRepository.SaveChanges();

            return dbFormAnswer;
        }

        /// <summary>
        /// Updates the form answer.
        /// </summary>
        /// <param name="formAnswerToUpdate">The form answer to update.</param>
        /// <param name="organizationId">The organization identifier.</param>
        public void UpdateFormAnswer(form_answer formAnswerToUpdate, int organizationId)
        {
            var dbFormAnswer = this.formAnswerRepository.GetById(formAnswerToUpdate.id, organizationId);
            if (dbFormAnswer == null)
            {
                return;
            }

            //TODO Bindings ?
            dbFormAnswer.last_update = DateTime.Now;
            dbFormAnswer.replied = formAnswerToUpdate.replied;
            dbFormAnswer.organization_id = organizationId;

            this.formAnswerRepository.Update(dbFormAnswer);
            this.formAnswerRepository.SaveChanges();
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="organizationId">The organizationId.</param>
        /// <returns></returns>
        public IList<form_answer> GetAll(int organizationId)
        {
            return this.formAnswerRepository.GetAll(organizationId).ToList();
        }

        /// <summary>
        /// Gets all for collaborator and role.
        /// </summary>
        /// <param name="collaboratorId">The collaborator identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public IList<form_answer> GetAllForCollaboratorAndRole(int collaboratorId, int roleId, int organizationId)
        {
            return this.formAnswerRepository.GetAllForCollaboratorAndRole(collaboratorId, roleId, organizationId).ToList();
        }
        
        /// <summary>
        /// Gets all active.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<form_answer> GetAllActive()
        {
            return this.formAnswerRepository.GetAll().Where(fa => !fa.replied).ToList();
        }
    }
}