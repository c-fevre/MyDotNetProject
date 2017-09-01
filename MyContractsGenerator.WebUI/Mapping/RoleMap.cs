using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyContractsGenerator.Domain;
using MyContractsGenerator.WebUI.Models.RoleModels;

namespace MyContractsGenerator.WebUI.Mapping
{
    public static class RoleMap
    {
        #region Domain To Model

        /// <summary>
        ///     Transforms an User to an AdministratorModel
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        internal static RoleModel MapItem(role role)
        {
            if (role == null)
            {
                return null;
            }

            RoleModel roleModel = new RoleModel
            {
                Id = role.id,
                Label = role.label,
                IsActive = role.active
            };

            return roleModel;
        }

        /// <summary>
        /// Maps the items to select list items.
        /// </summary>
        /// <param name="domains">The domains.</param>
        /// <returns></returns>
        internal static IList<SelectListItem> MapItemsToSelectListItems(IList<role> domains)
        {
            var models = new List<SelectListItem>();

            foreach (var domain in domains)
            {
                models.Add(new SelectListItem
                {
                    Value = domain.id.ToString(),
                    Text = domain.label
                });
            }

            return models;
        }
        /// <summary>
        ///     Transforms a IEnumerable<roles> to a IEnumerable<RoleModel>
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        internal static IList<RoleModel> MapItems(IEnumerable<role> roles)
        {
            var enumerable = roles as IList<role> ?? roles.ToList();

            return !enumerable.Any() ? new List<RoleModel>() : enumerable.Select(MapItem).ToList();
        }

        #endregion
    }
}