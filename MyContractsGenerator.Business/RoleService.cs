﻿using System.Collections.Generic;
using System.Linq;
using MyContractsGenerator.Common.Validation;
using MyContractsGenerator.DAL.Repositories;
using MyContractsGenerator.Domain;
using MyContractsGenerator.Interfaces.InterfacesRepo;
using MyContractsGenerator.Interfaces.InterfacesServices;

namespace MyContractsGenerator.Business
{
    public class RoleService : BaseService, IRoleService
    {
        /// <summary>
        ///     The collaborator repository
        /// </summary>
        private readonly ICollaboratorRepository collaboratorRepository;

        /// <summary>
        ///     The collaborator repository
        /// </summary>
        private readonly IRoleRepository roleRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RoleService" /> class.
        /// </summary>
        /// <param name="roleRepository">The role repository.</param>
        /// <param name="collaboratorRepository">The collaborator repository.</param>
        public RoleService(IRoleRepository roleRepository, ICollaboratorRepository collaboratorRepository)
        {
            this.roleRepository = roleRepository;
            this.collaboratorRepository = collaboratorRepository;
        }

        /// <summary>
        /// delete logically the roles
        /// </summary>
        /// <param name="roleId"></param>
        public void DeleteRole(int roleId)
        {
            Requires.ArgumentGreaterThanZero(roleId, "Role Id");

            role dbRole = this.roleRepository.GetById(roleId);
            dbRole.active = false;

            //Desaffect all collaborators
            dbRole.collaborators.ToList().ForEach(c => { this.DesaffectToRole(dbRole.id, c.id); });

            this.roleRepository.SaveChanges();
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="roleToUpdate">The role to update.</param>
        /// <param name="organizationId"></param>
        public void UpdateRole(role roleToUpdate, int organizationId)
        {
            var dbRole = this.roleRepository.GetById(roleToUpdate.id);
            if (dbRole == null)
            {
                return;
            }

            dbRole.label = roleToUpdate.label;
            dbRole.active = roleToUpdate.active;
            dbRole.organization_id = organizationId;

            this.roleRepository.Update(dbRole);
            this.roleRepository.SaveChanges();
        }

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="roleToCreate">The role to create.</param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public role AddRole(role roleToCreate, int organizationId)
        {
            Requires.ArgumentNotNull(roleToCreate, "collaboratorToCreate");

            roleToCreate.active = true;
            roleToCreate.organization_id = organizationId;

            role dbUser = this.roleRepository.Add(roleToCreate);
            this.roleRepository.SaveChanges();

            return dbUser;
        }

        /// <summary>
        ///     Affects to questionnaire model.
        /// </summary>
        /// <param name="roleIdToAffect">The role identifier to affect.</param>
        /// <param name="editedCollaboratorId">The edited collaborator identifier.</param>
        private void AffectToRole(int roleIdToAffect, int editedCollaboratorId)
        {
            Requires.ArgumentGreaterThanZero(roleIdToAffect, "roleIdToAffect");
            Requires.ArgumentGreaterThanZero(editedCollaboratorId, "editedCollaboratorId");

            role roleToUpdate = this.roleRepository.GetById(roleIdToAffect);

            collaborator newAffectation = this.collaboratorRepository.GetById(editedCollaboratorId);
            if (newAffectation != null)
            {
                roleToUpdate.collaborators.Add(newAffectation);
            }

            this.roleRepository.Update(roleToUpdate);
            this.roleRepository.SaveChanges();
        }

        /// <summary>
        ///     Desaffects to questionnaire model.
        /// </summary>
        /// <param name="roleIdToDesaffect">The role identifier to desaffect.</param>
        /// <param name="editedCollaboratorId">The edited collaborator identifier.</param>
        private void DesaffectToRole(int roleIdToDesaffect, int editedCollaboratorId)
        {
            Requires.ArgumentGreaterThanZero(roleIdToDesaffect, "roleIdToDesaffect");

            collaborator collaboratorToUpdate =
                this.collaboratorRepository.GetById(editedCollaboratorId);

            //role roleToUpdate = this.roleRepository.GetById(roleIdToDesaffect);

            if (collaboratorToUpdate.roles.Any())
            {
                collaboratorToUpdate.roles =
                    collaboratorToUpdate.roles.Where(r => !r.id.Equals(roleIdToDesaffect)).ToList();
            }

            this.collaboratorRepository.Update(collaboratorToUpdate);
            this.collaboratorRepository.SaveChanges();
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public role GetById(int id, int organizationId)
        {
            return this.roleRepository.GetById(id, organizationId);
        }

        /// <summary>
        /// Gets all active.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        public IList<role> GetAllActive(int organizationId)
        {
            return this.roleRepository.GetAllActive(organizationId).ToList();
        }

        /// <summary>
        /// Determines whether [is this label already exists] [the specified label].
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="currentId">The current identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>
        /// <c>true</c> if [is this label already exists] [the specified label]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsThisLabelAlreadyExists(string label, int currentId, int organizationId)
        {
            return this.roleRepository.GetAllActive(organizationId).Where(r => r.id != currentId).Any(r => r.label == label);
        }

        /// <summary>
        /// Determines whether [is this label already exists] [the specified label].
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>
        /// <c>true</c> if [is this label already exists] [the specified label]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsThisLabelAlreadyExists(string label, int organizationId)
        {
            return this.roleRepository.GetAllActive(organizationId).Any(r => r.label == label);
        }

        /// <summary>
        /// Affects to role.
        /// </summary>
        /// <param name="editedCollaboratorLinkedRolesIds">The edited collaborator linked roles ids.</param>
        /// <param name="editedCollaboratorId">The edited collaborator identifier.</param>
        /// <param name="organizationId"></param>
        public void AffectToRole(IEnumerable<int> editedCollaboratorLinkedRolesIds, int editedCollaboratorId, int organizationId)
        {
            Requires.ArgumentGreaterThanZero(editedCollaboratorId, "editedCollaboratorId");
            Requires.ArgumentNotNull(editedCollaboratorLinkedRolesIds, "editedCollaboratorLinkedRolesIds");
            foreach (var roleId in editedCollaboratorLinkedRolesIds)
            {
                Requires.ArgumentGreaterThanZero(roleId, "roleId");
            }

            IList<role> roleIdsAlreadyAffected =
                this.GetAllActive(organizationId).Where(r => r.collaborators.Any(c => c.id.Equals(editedCollaboratorId))).ToList();

            var alreadyAffectedRolesIds = new HashSet<int>();
            foreach (var alreadyAffectedRole in roleIdsAlreadyAffected)
            {
                alreadyAffectedRolesIds.Add(alreadyAffectedRole.id);
            }

            //Role to desaffect
            HashSet<int> roleIdsToDesaffect =
                new HashSet<int>(alreadyAffectedRolesIds.Where(i => !editedCollaboratorLinkedRolesIds.Contains(i)));
            foreach (var roleIdToDesaffect in roleIdsToDesaffect)
            {
                this.DesaffectToRole(roleIdToDesaffect, editedCollaboratorId);
            }

            //Modalities to affect
            HashSet<int> roleIdsToAffect =
                new HashSet<int>(editedCollaboratorLinkedRolesIds.Where(i => !alreadyAffectedRolesIds.Contains(i)));
            foreach (var roleIdToAffect in roleIdsToAffect)
            {
                this.AffectToRole(roleIdToAffect, editedCollaboratorId);
            }
        }
    }
}