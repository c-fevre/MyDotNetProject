namespace MyContractsGenerator.WebUI.Models.NotificationModels
{
    /// <summary>
    ///     http://bootstrap-notify.remabledesigns.com/
    /// </summary>
    public class NotificationModel
    {
        /// <summary>
        ///     Duration of the notification
        /// </summary>
        public int Delay { get; set; } = 15000;

        public string Title { get; set; }

        /// <summary>
        ///     Color of the notification:
        ///     info -> blue
        ///     danger -> red
        ///     warning -> yellow
        ///     success -> green
        /// </summary>
        public string Type { get; set; } = "success";
    }
}