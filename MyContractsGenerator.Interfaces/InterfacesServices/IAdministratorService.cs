﻿using System.Collections.Generic;
using MyContractsGenerator.Domain;

namespace MyContractsGenerator.Interfaces.InterfacesServices
{
    public interface IAdministratorService
    {
        /// <summary>
        /// Gets the by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        administrator GetByEmail(string email);

        /// <summary>
        /// Gets the administrator by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        administrator GetAdministratorById(int id);

        /// <summary>
        /// Gets the active administrators.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        IList<administrator> GetActiveAdministrators(int organizationId);

        /// <summary>
        /// Determines whether [is this email already exists] [the specified email].
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is this email already exists] [the specified email]; otherwise, <c>false</c>.
        /// </returns>
        bool IsThisEmailAlreadyExists(string email);

        /// <summary>
        /// Determines whether [is this email already exists] [the specified email].
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="administratorId">The administrator identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is this email already exists] [the specified email]; otherwise, <c>false</c>.
        /// </returns>
        bool IsThisEmailAlreadyExists(string email, int administratorId);

        /// <summary>
        ///     Reset administrator password and send the generated password by mail
        /// </summary>
        /// <param name="passwordOwneradministratorId"></param>
        /// <param name="organizationId">The organization identifier.</param>
        void ResetPassword(int passwordOwneradministratorId, int organizationId);

        /// <summary>
        ///     Updates the administrator.
        /// </summary>
        /// <param name="administratorToUpdate">The administrator to update.</param>
        /// <param name="organizationId">The organization identifier.</param>
        void Update(administrator administratorToUpdate, int organizationId);

        /// <summary>
        ///     Deletes the specified administrator identifier.
        /// </summary>
        /// <param name="administratorId">The administrator identifier.</param>
        /// <param name="organizationId">The organization identifier.</param>
        void Delete(int administratorId);

        /// <summary>
        /// Adds the specified administrator to create.
        /// </summary>
        /// <param name="administratorToCreate">The administrator to create.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns></returns>
        administrator Add(administrator administratorToCreate, int organizationId);
    }
}