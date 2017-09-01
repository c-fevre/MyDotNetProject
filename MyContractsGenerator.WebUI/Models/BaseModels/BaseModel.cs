using System.Collections.Generic;
using MyContractsGenerator.WebUI.Models.NotificationModels;

namespace MyContractsGenerator.WebUI.Models.BaseModels
{
    /// <summary>
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        ///     If a notification is add to this List, it will be displayed a the launching of the page by the _Layout
        /// </summary>
        public List<NotificationModel> Notifications { get; set; } = new List<NotificationModel>();

        public List<UserNotificationModel> UserNotifications { get; set; } = new List<UserNotificationModel>();
    }
}