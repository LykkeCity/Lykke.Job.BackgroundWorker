using Lykke.Job.BackgroundWorker.Core.Settings.JobSettings;
using Lykke.Job.BackgroundWorker.Core.Settings.SlackNotifications;
using Lykke.Service.ClientAccount.Client;

namespace Lykke.Job.BackgroundWorker.Core.Settings
{
    public class AppSettings
    {
        public BackgroundWorkerSettings BackgroundWorkerJob { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
        public ClientAccountServiceClientSettings ClientAccountServiceClient { get; set; }
    }
}