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
    /// <seealso cref="MyContractsGenerator.Interfaces.InterfacesServices.IOrganizationService" />
    public class OrganizationService : BaseService, IOrganizationService
    {
        /// <summary>
        ///     The organization repository
        /// </summary>
        private readonly IOrganizationRepository organizationRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrganizationService" /> class.
        /// </summary>
        /// <param name="organizationRepository">The organization repository.</param>
        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            this.organizationRepository = organizationRepository;
        }

        /// <summary>
        ///     Gets organization by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public organization GetById(int id)
        {
            return this.organizationRepository.GetById(id);
        }

        /// <summary>
        ///     delete logically the organization
        /// </summary>
        /// <param name="organizationId"></param>
        public void DeleteOrganization(int organizationId)
        {
            Requires.ArgumentGreaterThanZero(organizationId, "Organization Id");
            this.organizationRepository.Remove(organizationId);

            this.organizationRepository.SaveChanges();
        }

        /// <summary>
        ///     Adds the organization.
        /// </summary>
        /// <param name="organizationToCreate">The organization to create.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public organization AddOrganization(organization organizationToCreate)
        {
            Requires.ArgumentNotNull(organizationToCreate, "organizationToCreate");

            organization dbOrganization = this.organizationRepository.Add(organizationToCreate);
            this.organizationRepository.SaveChanges();

            return dbOrganization;
        }

        /// <summary>
        ///     Updates the organization.
        /// </summary>
        /// <param name="organizationToUpdate">The organization to update.</param>
        public void UpdateOrganization(organization organizationToUpdate)
        {
            var dbOrganization = this.organizationRepository.GetById(organizationToUpdate.id);
            if (dbOrganization == null)
            {
                return;
            }

            //TODO Bindings ?
            dbOrganization.label = organizationToUpdate.label;

            this.organizationRepository.Update(dbOrganization);
            this.organizationRepository.SaveChanges();
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<organization> GetAll()
        {
            return this.organizationRepository.GetAll().ToList();
        }

        /// <summary>
        ///     Adds the organizations.
        /// </summary>
        /// <param name="organizations">The organizations.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void AddOrganizations(IList<organization> organizations)
        {
            Requires.ArgumentNotNull(organizations, "organizations");

            organizations.ToList().ForEach(a => { this.organizationRepository.Add(a); });

            this.organizationRepository.SaveChanges();
        }

        public bool IsThisLabelAlreadyExists(string label, int id)
        {
            return this.organizationRepository.GetAll().Any(o => o.id != id && string.Equals(o.label, label, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Determines whether /[is this label already exists] [the specified label].
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns>
        /// <c>true</c> if [is this label already exists] [the specified label]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsThisLabelAlreadyExists(string label)
        {
            return this.organizationRepository.GetAll().Any(o => string.Equals(o.label, label, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}