using System.Collections.Generic;
using System.Globalization;
using System.Web;
using MyContractsGenerator.WebUI.Models.NotificationModels;

namespace MyContractsGenerator.WebUI.Common
{
    /// <summary>
    ///     Classe qui fait l'accès typé à la session Web
    /// </summary>
    public static class AppSession
    {
        /// <summary>
        ///     List of userNotifications
        /// </summary>
        public static List<UserNotificationModel> UserNotifications
        {
            get { return GetWithDefault("UserNotifications", (List<UserNotificationModel>) null); }
            set { SetValue("UserNotifications", value); }
        }

        /// <summary>
        ///     user CultureInfo to define the resources to use
        /// </summary>
        public static CultureInfo UserCultureInfo
        {
            get { return GetWithDefault<CultureInfo>("userCultureInfo", null); }
            set { SetValue("userCultureInfo", value); }
        }

        private static T GetWithDefault<T>(string name, T defaultValue)
        {
            if (HttpContext.Current.Session?[name] == null)
            {
                return defaultValue;
            }

            return (T) HttpContext.Current.Session[name];
        }

        private static void SetValue(string name, object value)
        {
            HttpContext.Current.Session[name] = value;
        }
    }
}