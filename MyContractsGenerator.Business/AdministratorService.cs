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
        // TODO Multilingue
        //private readonly IApplicationLangageRepository applicationLangageRepository;
        private readonly IMailService mailService;
        private readonly IAdministratorRepository administratorRepository;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="administratorRepo"></param>
        public AdministratorService(IAdministratorRepository administratorRepo, IMailService mailService)
        {
            this.administratorRepository = administratorRepo;
            // TODO Multilingue
            //this.applicationLangageRepository = applicationLangageRepo;
            this.mailService = mailService;
        }

        /// <summary>
        ///     Gets administrator by login
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public administrator GetByLogin(string login, string password)
        {
            var administrator = this.administratorRepository.GetByLogin(login);
            if (administrator == null)
            {
                return null;
            }

            return administrator.password != password ? null : administrator;
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
        public void UpdateAdministrator(administrator administratorToUpdate)
        {
            Requires.ArgumentNotNull(administratorToUpdate, "administratorToUpdate");

            var dbadministrator = this.administratorRepository.GetById(administratorToUpdate.id);
            if (dbadministrator == null)
            {
                return;
            }

            dbadministrator.email = administratorToUpdate.email;
            dbadministrator.login = administratorToUpdate.login;
            // TODO Multilingue
            // dbadministrator.applicationlanguage = this.applicationLangageRepository.GetById(administratorToUpdate.applicationlanguage.id);
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
        public administrator Addadministrator(administrator administratorToCreate)
        {
            Requires.ArgumentNotNull(administratorToCreate, "administratorToUpdate");
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
        public void DeleteAdministrator(int administratorId)
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
        /// <param name="administratorDoingUpdateId"></param>
        public void ResetPassword(int passwordOwneradministratorId, int administratorDoingUpdateId)
        {
            //Requires
            Requires.ArgumentGreaterThanZero(passwordOwneradministratorId, "passwordOwneradministratorId");
            Requires.ArgumentGreaterThanZero(administratorDoingUpdateId, "administratorDoingUpdateId");
            administrator administrator = this.administratorRepository.GetById(passwordOwneradministratorId);
            Requires.ArgumentNotNull(administrator, "administrator");

            //new password generation
            string clearPassword = PasswordGenerator.GeneratePassword(8, 4);
            administrator.password = ShaHashPassword.GetSha256ResultString(clearPassword);
            this.UpdateAdministrator(administrator);

            //Send mail
            this.mailService.SendResetPasswordEmail(administrator, clearPassword);
        }

    }
}
