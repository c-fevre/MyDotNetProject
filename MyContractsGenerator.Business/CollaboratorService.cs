using System.Collections.Generic;
using System.Linq;
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
        ///     The collaborator repository
        /// </summary>
        private readonly ICollaboratorRepository collaboratorRepository;

        /// <summary>
        ///     The mail service
        /// </summary>
        private readonly IMailService mailService;

        /// <summary>
        ///     The role service
        /// </summary>
        private readonly IRoleService roleService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CollaboratorService" /> class.
        /// </summary>
        /// <param name="collaboratorRepository">The collaborator repository.</param>
        /// <param name="mailService">The mail service.</param>
        /// <param name="roleService">The role service.</param>
        public CollaboratorService(ICollaboratorRepository collaboratorRepository, IMailService mailService,
                                   IRoleService roleService)
        {
            this.collaboratorRepository = collaboratorRepository;
            this.mailService = mailService;
            this.roleService = roleService;
        }

        /// <summary>
        /// Gets the by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public collaborator GetByEmail(string email, int organizationId)
        {
            return this.collaboratorRepository.GetByEmail(email, organizationId);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public collaborator GetById(int id, int organizationId)
        {
            return this.collaboratorRepository.GetById(id, organizationId);
        }

        /// <summary>
        /// Determines whether [is this email already exists] [the specified email].
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>
        /// <c>true</c> if [is this email already exists] [the specified email]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsThisEmailAlreadyExists(string email, int organizationId)
        {
            return this.collaboratorRepository.GetByEmail(email, organizationId) != null;
        }

        /// <summary>
        /// Determines whether [is this email already exists] [the specified email].
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="currentCollaboratorId">The current collaborator identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is this email already exists] [the specified email]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsThisEmailAlreadyExists(string email, int currentCollaboratorId, int organizationId)
        {
            collaborator result = this.collaboratorRepository.GetByEmail(email, organizationId);

            if (result == null)
            {
                return false;
            }

            return !result.id.Equals(currentCollaboratorId);
        }

        /// <summary>
        /// Gets all active.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public IList<collaborator> GetAllActive(int organizationId)
        {
            return this.collaboratorRepository.GetAllActive(organizationId).ToList();
        }

        /// <summary>
        /// Deletes the collaborator.
        /// </summary>
        /// <param name="collaboratorId">The collaborator identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        public void DeleteCollaborator(int collaboratorId, int organizationId)
        {
            Requires.ArgumentGreaterThanZero(collaboratorId, "Collaborateur Id");

            collaborator dbCollaborator = this.collaboratorRepository.GetById(collaboratorId, organizationId);
            dbCollaborator.active = false;

            //Desaffect all collaborators
            IEnumerable<int> emptyRoleList = new List<int>();
            this.roleService.AffectToRole(emptyRoleList, dbCollaborator.id, organizationId);

            this.collaboratorRepository.SaveChanges();
        }

        /// <summary>
        /// Adds the collaborator.
        /// </summary>
        /// <param name="collaboratorToCreate">The collaborator to create.</param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public collaborator AddCollaborator(collaborator collaboratorToCreate, int organizationId)
        {
            Requires.ArgumentNotNull(collaboratorToCreate, "collaboratorToCreate");

            collaboratorToCreate.active = true;
            collaboratorToCreate.organization_id = organizationId;

            collaborator dbUser = this.collaboratorRepository.Add(collaboratorToCreate);
            this.collaboratorRepository.SaveChanges();

            return dbUser;
        }

        /// <summary>
        /// Updates the collaborator.
        /// </summary>
        /// <param name="collaboratorToUpdate">The collaborator to update.</param>
        /// <param name="organizationId"></param>
        public void UpdateCollaborator(collaborator collaboratorToUpdate, int organizationId)
        {
            var dbUser = this.collaboratorRepository.GetById(collaboratorToUpdate.id);
            if (dbUser == null)
            {
                return;
            }

            dbUser.email = collaboratorToUpdate.email;
            dbUser.firstname = collaboratorToUpdate.firstname;
            dbUser.lastname = collaboratorToUpdate.lastname;
            dbUser.organization_id = organizationId;

            this.collaboratorRepository.Update(dbUser);
            this.collaboratorRepository.SaveChanges();
        }
    }
}