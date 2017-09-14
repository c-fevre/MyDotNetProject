using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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

            string administratorsList = string.Empty;

            if (organization.administrators.Any())
            {
                organization.administrators.Where(a => a.active).ToList().ForEach(a =>
                  {
                      administratorsList += $"{a.firstname} {a.lastname} <\"{a.email}\"> ";
                  });
            }

            OrganizationModel organizationModel = new OrganizationModel
            {
                Id = organization.id,
                Label = organization.label,
                IsRemovable = !organization.administrators.Any(a => a.is_super_admin),
                AdministratorsList = administratorsList
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

        /// <summary>
        /// Maps the items to select list items.
        /// </summary>
        /// <param name="organizations">The organizations.</param>
        /// <returns></returns>
        public static IList<SelectListItem> MapItemsToSelectListItems(IList<organization> organizations)
        {
            var models = new List<SelectListItem>();

            foreach (var domain in organizations)
            {
                models.Add(new SelectListItem
                {
                    Value = domain.id.ToString(),
                    Text = domain.label
                });
            }

            return models;
        }
    }
}