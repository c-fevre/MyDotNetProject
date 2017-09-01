using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyContractsGenerator.WebUI.Models.NotificationModels
{
    public class UserNotificationModel : NotificationModel
    {
        public int Id { get; set; }

        public int IdUser { get; set; }
    }
}