using System;
using System.Collections.Generic;
using System.Linq;
using MyContractsGenerator.Common.PasswordHelper;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesRepo;
using MyContractsGenerator.Interfaces.InterfacesServices;

namespace MyContractsGenerator.Business
{
    public class AdministratorService : BaseService, IAdministratorService
    {
        private readonly IAdministratorRepository administratorRepository;
        private readonly IMailService mailService;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="administratorRepo"></param>
        public AdministratorService(IAdministratorRepository administratorRepo, IMailService mailService)
        {
            this.administratorRepository = administratorRepo;
            this.mailService = mailService;
        }

        /// <summary>
        /// Gets the by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public administrator GetByEmail(string email)
        {
            return this.administratorRepository.GetByEmail(email);
        }
        
        /// <summary>
        ///     Gets all administrators
        /// </summary>
        /// <returns></returns>
        public IList<administrator> GetActiveAdministrators(int organizationId)
        {
            return this.administratorRepository.GetActiveAdministrators(organizationId).ToList();
        }

        /// <summary>
        ///     Gets administrator by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public administrator GetAdministratorById(int id)
        {
            Requires.ArgumentGreaterThanZero(id, "id");
            return this.administratorRepository.GetById(id);
        }

        /// <summary>
        /// Updates the administrator.
        /// </summary>
        /// <param name="administratorToUpdate">The administrator to update.</param>
        /// <param name="organizationId">The organization identifier.</param>
        public void Update(administrator administratorToUpdate, int organizationId)
        {
            Requires.ArgumentNotNull(administratorToUpdate, "administratorToUpdate");

            var dbadministrator = this.administratorRepository.GetById(administratorToUpdate.id);
            if (dbadministrator == null)
            {
                return;
            }

            dbadministrator.email = administratorToUpdate.email;
            dbadministrator.firstname = administratorToUpdate.firstname;
            dbadministrator.lastname = administratorToUpdate.lastname;
            dbadministrator.active = administratorToUpdate.active;
            dbadministrator.organization_id = organizationId;

            if (!string.IsNullOrEmpty(administratorToUpdate.password))
            {
                dbadministrator.password = administratorToUpdate.password;
            }

            this.administratorRepository.Update(dbadministrator);
            this.administratorRepository.SaveChanges();
        }

        /// <summary>
        /// Adds the specified administrator to create.
        /// </summary>
        /// <param name="administratorToCreate">The administrator to create.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public administrator Add(administrator administratorToCreate, int organizationId)
        {
            Requires.ArgumentNotNull(administratorToCreate, "administratorToCreate");

            administratorToCreate.active = true;
            administratorToCreate.organization_id = organizationId;

            administrator dbadministrator = this.administratorRepository.Add(administratorToCreate);
            this.administratorRepository.SaveChanges();

            return dbadministrator;
        }

        /// <summary>
        ///     DeleteAdministrator
        /// </summary>
        /// <param name="administratorId"></param>
        public void Delete(int administratorId)
        {
            Requires.ArgumentGreaterThanZero(administratorId, "administratorid");

            administrator dbadministrator = this.administratorRepository.GetById(administratorId);
            dbadministrator.active = false;
            this.administratorRepository.SaveChanges();
        }

        /// <summary>
        /// Determines whether [is this email already exists] [the specified email].
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>
        /// <c>true</c> if [is this email already exists] [the specified email]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsThisEmailAlreadyExists(string email)
        {
            IList<administrator> administrators =
                this.administratorRepository.GetActiveAdministrators().Where(a => a.email.Equals(email)).ToList();
            return !(administrators == null || administrators.Count == 0);
        }

        /// <summary>
        /// Determines whether [is this email already exists] [the specified email].
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="administratorId">The administrator identifier.</param>
        /// <returns>
        /// <c>true</c> if [is this email already exists] [the specified email]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsThisEmailAlreadyExists(string email, int administratorId)
        {
            IList<administrator> administrators =
                this.administratorRepository.GetActiveAdministrators().Where(a => a.email.Equals(email)).ToList();

            return administrators.Any(administrator => administrator.id != administratorId);
        }

        /// <summary>
        /// Reset administrator password and send the generated password by mail
        /// </summary>
        /// <param name="passwordOwneradministratorId"></param>
        /// <param name="organizationId">The organization identifier.</param>
        public void ResetPassword(int passwordOwneradministratorId, int organizationId)
        {
            //Requires
            Requires.ArgumentGreaterThanZero(passwordOwneradministratorId, "passwordOwneradministratorId");
            administrator administrator = this.administratorRepository.GetById(passwordOwneradministratorId);
            Requires.ArgumentNotNull(administrator, "administrator");

            //new password generation
            string clearPassword = PasswordGenerator.GeneratePassword(8, 4);
            administrator.password = ShaHashPassword.GetSha256ResultString(clearPassword);
            this.Update(administrator, organizationId);

            //Send mail
            this.mailService.SendGeneratedPasswordAdministrator(administrator, clearPassword);
        }
    }
}