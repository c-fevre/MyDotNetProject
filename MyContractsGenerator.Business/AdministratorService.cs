using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyContractsGenerator.Common.PasswordHelper;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesRepo;
using MyContractsGenerator.Interfaces.InterfacesServices;

namespace MyContractsGenerator.Business
{
    public class AdministratorService : BaseService, IAdministratorService
    {
        private readonly IMailService mailService;
        private readonly IAdministratorRepository administratorRepository;

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
        ///     Get administrator by Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public administrator GetByEmail(string email)
        {
            return this.administratorRepository.GetByEmail(email);
        }

        /// <summary>
        ///     Gets Manipulator by login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public administrator GetAdministratorByLogin(string email, string password)
        {
            var administrator = this.administratorRepository.GetByEmail(email);
            if (administrator == null)
            {
                return null;
            }

            if (administrator.password != password)
            {
                return null;
            }

            return administrator;
        }

        /// <summary>
        ///     Gets all administrators
        /// </summary>
        /// <returns></returns>
        public IList<administrator> GetActiveAdministrators()
        {
            return this.administratorRepository.GetActiveAdministrators();
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
        ///     UpdateAdministrator
        /// </summary>
        /// <param name="administratorToUpdate"></param>
        /// <param name="administratorDoingUpdate"></param>
        public void Update(administrator administratorToUpdate)
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
            dbadministrator.organization_id = administratorToUpdate.organization_id;

            if (!string.IsNullOrEmpty(administratorToUpdate.password))
            {
                dbadministrator.password = administratorToUpdate.password;
            }

            this.administratorRepository.Update(dbadministrator);
            this.administratorRepository.SaveChanges();
        }

        /// <summary>
        ///     Addadministrator
        /// </summary>
        /// <param name="administratorToCreate"></param>
        /// <returns></returns>
        public administrator Add(administrator administratorToCreate)
        {
            Requires.ArgumentNotNull(administratorToCreate, "administratorToCreate");

            administratorToCreate.active = true;

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
        ///     Check if this email is already used by an active administrator
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true: this email is already used be an active administrator</returns>
        public bool IsThisEmailAlreadyExists(string email)
        {
            IList<administrator> administrators = this.administratorRepository.GetActiveAdministrators().Where(a => a.email.Equals(email)).ToList();
            return !(administrators == null || administrators.Count == 0);
        }

        /// Check if this email is already used by an active administrator, excepted the administrator passed by parameter
        /// </summary>
        /// <param name="email"></param>
        /// <param name="administratorId"></param>
        /// <returns>true: this email is already used be an active administrator, excepted the administrator passed by parameter</returns>
        public bool IsThisEmailAlreadyExists(string email, int administratorId)
        {
            IList<administrator> administrators =
                this.administratorRepository.GetActiveAdministrators().Where(a => a.email.Equals(email)).ToList();

            return administrators.Any(administrator => administrator.id != administratorId);
        }
        

        /// <summary>
        ///     Reset administrator password and send the generated password by mail
        /// </summary>
        /// <param name="passwordOwneradministratorId"></param>
        public void ResetPassword(int passwordOwneradministratorId)
        {
            //Requires
            Requires.ArgumentGreaterThanZero(passwordOwneradministratorId, "passwordOwneradministratorId");
            administrator administrator = this.administratorRepository.GetById(passwordOwneradministratorId);
            Requires.ArgumentNotNull(administrator, "administrator");

            //new password generation
            string clearPassword = PasswordGenerator.GeneratePassword(8, 4);
            administrator.password = ShaHashPassword.GetSha256ResultString(clearPassword);
            this.Update(administrator);

            //Send mail
            this.mailService.SendGeneratedPasswordAdministrator(administrator, clearPassword);
        }
    }
}
