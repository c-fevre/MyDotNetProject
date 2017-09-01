using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesRepo;
using MyContractsGenerator.Interfaces.InterfacesServices;

namespace MyContractsGenerator.Business
{
    /// <summary>
    /// </summary>
    /// <seealso cref="MyContractsGenerator.Business.BaseService" />
    /// <seealso cref="MyContractsGenerator.Interfaces.InterfacesServices.ICollaboratorService" />
    public class CollaboratorService : BaseService, ICollaboratorService
    {
        /// <summary>
        /// The collaborator repository
        /// </summary>
        private readonly ICollaboratorRepository collaboratorRepository;

        /// <summary>
        /// The mail service
        /// </summary>
        private readonly IMailService mailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollaboratorService"/> class.
        /// </summary>
        /// <param name="collaboratorRepository">The collaborator repository.</param>
        /// <param name="mailService">The mail service.</param>
        public CollaboratorService(ICollaboratorRepository collaboratorRepository, IMailService mailService)
        {
            this.collaboratorRepository = collaboratorRepository;
            this.mailService = mailService;
        }

        /// <summary>
        /// Gets administrator by login
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public collaborator GetByEmail(string email)
        {
            return this.collaboratorRepository.GetByEmail(email);
        }

        /// <summary>
        /// Gets administrator by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public collaborator GetById(int id)
        {
            return this.collaboratorRepository.GetById(id);
        }

        /// <summary>
        /// Check if this email is already used be an active administrator
        /// </summary>
        /// <param name="email"></param>
        /// <returns>
        /// true: this email is already used be an active administrator
        /// </returns>
        public bool IsThisEmailAlreadyExists(string email)
        {
            return this.collaboratorRepository.GetByEmail(email) != null;
        }

        /// <summary>
        /// Determines whether [is this email already exists] [the specified email address].
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <param name="currentCollaboratorId">The identifier.</param>
        /// <returns>
        /// <c>true</c> if [is this email already exists] [the specified email address]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool IsThisEmailAlreadyExists(string email, int currentCollaboratorId)
        {
            collaborator result = this.collaboratorRepository.GetByEmail(email);

            if (result == null)
            {
                return false;
            }

            return !result.id.Equals(currentCollaboratorId);
        }

        public IList<collaborator> GetAllActive()
        {
            return this.collaboratorRepository.GetAllActive().ToList();
        }

        /// <summary>
        /// delete logically the user
        /// </summary>
        /// <param name="collaboratorId"></param>
        public void DeleteCollaborator(int collaboratorId)
        {
            Requires.ArgumentGreaterThanZero(collaboratorId, "Collaborateur Id");

            collaborator dbCollaborator = this.collaboratorRepository.GetById(collaboratorId);
            dbCollaborator.active = false;
            this.collaboratorRepository.SaveChanges();
        }

        /// <summary>
        /// Adds the collaborator.
        /// </summary>
        /// <param name="collaboratorToCreate">The collaborator to create.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public collaborator AddCollaborator(collaborator collaboratorToCreate)
        {
            Requires.ArgumentNotNull(collaboratorToCreate, "collaboratorToCreate");

            collaboratorToCreate.active = true;

            collaborator dbUser = this.collaboratorRepository.Add(collaboratorToCreate);
            this.collaboratorRepository.SaveChanges();

            return dbUser;
        }

        /// <summary>
        /// Updates the collaborator.
        /// </summary>
        /// <param name="collaboratorToUpdate">The collaborator to update.</param>
        public void UpdateCollaborator(collaborator collaboratorToUpdate)
        {
            var dbUser = this.collaboratorRepository.GetById(collaboratorToUpdate.id);
            if (dbUser == null)
            {
                return;
            }

            dbUser.email = collaboratorToUpdate.email;
            dbUser.firstname = collaboratorToUpdate.firstname;
            dbUser.lastname = collaboratorToUpdate.lastname;

            // TODO Multilingue
            //dbUser.applicationlanguage = this.applicationLangageRepository.GetById(userToUpdate.applicationlanguage.id);

            this.collaboratorRepository.Update(dbUser);
            this.collaboratorRepository.SaveChanges();
        }
    }
}
