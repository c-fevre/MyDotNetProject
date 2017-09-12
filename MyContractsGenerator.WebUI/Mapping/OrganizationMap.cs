using System.Collections.Generic;
using System.Linq;
using MyContractsGenerator.Domain;
using MyContractsGenerator.WebUI.Models.OrganizationModels;

namespace MyContractsGenerator.WebUI.Mapping
{
    public static class OrganizationMap
    {
        #region Domain To Model

        /// <summary>
        ///     Transforms an User to an OrganizationModel
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        internal static OrganizationModel MapItem(organization organization)
        {
            if (organization == null)
            {
                return null;
            }

            OrganizationModel organizationModel = new OrganizationModel
            {
                Id = organization.id,
                Label = organization.label,
                IsRemovable = !organization.administrators.Any(a => a.is_super_admin)
            };

            return organizationModel;
        }

        /// <summary>
        ///     Transforms a IEnumerable<organizations> to a IEnumerable<OrganizationModel>
        /// </summary>
        /// <param name="organizations"></param>
        /// <returns></returns>
        internal static IList<OrganizationModel> MapItems(IEnumerable<organization> organizations)
        {
            IList<OrganizationModel> models = new List<OrganizationModel>();

            if (organizations.Any())
            {
                organizations.ToList().ForEach(c => { models.Add(MapItem(c)); });
            }

            return models;
        }

        #endregion
    }
}