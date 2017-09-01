using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.DAL;
using MyContractsGenerator.DAL.Repositories;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesServices;

namespace MyContractsGenerator.Business
{
    public class RoleService : BaseService, IRoleService
    {
        /// <summary>
        /// The collaborator repository
        /// </summary>
        private readonly IRoleRepository roleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleService"/> class.
        /// </summary>
        /// <param name="roleRepository">The role repository.</param>
        public RoleService(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        /// <summary>
        /// Gets administrator by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public role GetById(int id)
        {
            return this.roleRepository.GetById(id);
        }

        public void DeleteRole(int roleId)
        {
            Requires.ArgumentGreaterThanZero(roleId, "Role Id");

            role dbRole = this.roleRepository.GetById(roleId);
            dbRole.active = false;
            this.roleRepository.SaveChanges();
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="roleToUpdate">The role to update.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void UpdateRole(role roleToUpdate)
        {
            var dbRole = this.roleRepository.GetById(roleToUpdate.id);
            if (dbRole == null)
            {
                return;
            }

            dbRole.label = roleToUpdate.label;
            dbRole.active = roleToUpdate.active;
            
            this.roleRepository.Update(dbRole);
            this.roleRepository.SaveChanges();
        }

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="roleToCreate">The role to create.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public role AddRole(role roleToCreate)
        {
            Requires.ArgumentNotNull(roleToCreate, "collaboratorToCreate");

            roleToCreate.active = true;

            role dbUser = this.roleRepository.Add(roleToCreate);
            this.roleRepository.SaveChanges();

            return dbUser;
        }

        /// <summary>
        /// Determines whether [is this label already exists] [the specified label].
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns>
        /// <c>true</c> if [is this label already exists] [the specified label]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool IsThisLabelAlreadyExists(string label)
        {
            return this.roleRepository.GetAllActive().Any(r => r.label == label);
        }

        /// <summary>
        /// Determines whether [is this label already exists] [the specified label].
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="currentLabel">The current label.</param>
        /// <returns>
        /// <c>true</c> if [is this label already exists] [the specified label]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool IsThisLabelAlreadyExists(string label, int currentId)
        {
            role result = this.roleRepository.GetAllActive().SingleOrDefault(r => r.label == label);

            if (result == null)
            {
                return false;
            }

            return !result.id.Equals(currentId);
        }

        /// <summary>
        /// Gets roles
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<role> GetAllActive()
        {
            return this.roleRepository.GetAllActive().ToList();
        }

        /// <summary>
        /// Affects to role.
        /// </summary>
        /// <param name="editedCollaboratorLinkedRolesIds">The edited collaborator linked roles ids.</param>
        /// <param name="editedCollaboratorId">The edited collaborator identifier.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void AffectToRole(IEnumerable<int> editedCollaboratorLinkedRolesIds, int editedCollaboratorId)
        {
            throw new NotImplementedException();
        }
    }
}
