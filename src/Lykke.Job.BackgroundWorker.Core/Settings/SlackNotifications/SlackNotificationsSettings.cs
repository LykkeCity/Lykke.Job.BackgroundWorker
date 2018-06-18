namespace Lykke.Job.BackgroundWorker.Core.Settings.SlackNotifications
{
    public class SlackNotificationsSettings
    {
        public AzureQueuePublicationSettings AzureQueue { get; set; }

        public int ThrottlingLimitSeconds { get; set; }
    }
}