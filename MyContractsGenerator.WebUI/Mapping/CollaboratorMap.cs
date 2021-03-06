﻿using System.Collections.Generic;
using System.Linq;
using MyContractsGenerator.Domain;
using MyContractsGenerator.WebUI.Models.CollaboratorModels;

namespace MyContractsGenerator.WebUI.Mapping
{
    public static class CollaboratorMap
    {
        #region Domain To Model

        /// <summary>
        ///     Transforms an User to an CollaboratorModel
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

            return collaboratorModel;
        }

        /// <summary>
        ///     Transforms a IEnumerable<collaborators> to a IEnumerable<CollaboratorModel>
        /// </summary>
        /// <param name="collaborators"></param>
        /// <returns></returns>
        internal static IList<CollaboratorModel> MapItems(IEnumerable<collaborator> collaborators)
        {
            IList<CollaboratorModel> models = new List<CollaboratorModel>();

            if (collaborators.Any())
            {
                collaborators.ToList().ForEach(c => { models.Add(MapItem(c)); });
            }

            return models;
        }

        #endregion
    }
}