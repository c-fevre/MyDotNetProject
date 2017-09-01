using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using MyContractsGenerator.Common.PasswordHelper;
using MyContractsGenerator.Domain;
using MyContractsGenerator.WebUI.Models.CollaboratorModels;

namespace MyContractsGenerator.WebUI.Mapping
{
    public static class CollaboratorMap
    {
        #region Domain To Model

        /// <summary>
        ///     Transforms an User to an AdministratorModel
        /// </summary>
        /// <param name="collaborator"></param>
        /// <returns></returns>
        internal static CollaboratorModel MapItem(collaborator collaborator)
        {
            if (collaborator == null)
            {
                return null;
            }

            CollaboratorModel collaboratorModel = new CollaboratorModel
            {
                Id = collaborator.id,
                LastName = collaborator.lastname,
                FirstName = collaborator.firstname,
                Email = collaborator.email,
                IsActive = collaborator.active,
                LinkedRolesIds = RoleMap.MapItems(collaborator.roles).Select(r => r.Id).ToList()
            };

            //TODO Multilingue
            //administratorModel.Administrator = user.isadministrator;
            //administratorModel.ApplicationLangageId = user.applicationlanguage.id;

            return collaboratorModel;
        }

        /// <summary>
        ///     Transforms a IEnumerable<collaborators> to a IEnumerable<CollaboratorModel>
        /// </summary>
        /// <param name="collaborators"></param>
        /// <returns></returns>
        internal static IList<CollaboratorModel> MapItems(IEnumerable<collaborator> collaborators)
        {
            var enumerable = collaborators as IList<collaborator> ?? collaborators.ToList();

            return !enumerable.Any() ? new List<CollaboratorModel>() : enumerable.Select(MapItem).ToList();
        }

        #endregion
    }
}