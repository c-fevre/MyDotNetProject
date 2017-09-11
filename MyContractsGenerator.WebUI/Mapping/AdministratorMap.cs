using System.Collections.Generic;
using System.Linq;
using MyContractsGenerator.Domain;
using MyContractsGenerator.WebUI.Models.AdministratorModels;

namespace MyContractsGenerator.WebUI.Mapping
{
    public static class AdministratorMap
    {
        #region Domain To Model

        /// <summary>
        ///     Transforms an User to an AdministratorModel
        /// </summary>
        /// <param name="Administrator"></param>
        /// <returns></returns>
        internal static AdministratorModel MapItem(administrator administrator)
        {
            if (administrator == null)
            {
                return null;
            }

            AdministratorModel administratorModel = new AdministratorModel
            {
                Id = administrator.id,
                LastName = administrator.lastname,
                FirstName = administrator.firstname,
                Email = administrator.email,
                IsActive = administrator.active,
                OrganizationId = administrator.organization_id
            };

            return administratorModel;
        }

        /// <summary>
        ///     Transforms a IEnumerable<administrators> to a IEnumerable<AdministratorModel>
        /// </summary>
        /// <param name="administrators"></param>
        /// <returns></returns>
        internal static IList<AdministratorModel> MapItems(IEnumerable<administrator> administrators)
        {
            IList<AdministratorModel> models = new List<AdministratorModel>();

            if (administrators.Any())
            {
                administrators.ToList().ForEach(c => { models.Add(MapItem(c)); });
            }

            return models;
        }

        #endregion
    }
}