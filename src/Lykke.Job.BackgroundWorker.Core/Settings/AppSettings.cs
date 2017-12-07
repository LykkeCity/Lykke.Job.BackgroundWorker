using Lykke.Job.BackgroundWorker.Core.Settings.JobSettings;
using Lykke.Job.BackgroundWorker.Core.Settings.SlackNotifications;
using Lykke.Service.Kyc.Client;

namespace Lykke.Job.BackgroundWorker.Core.Settings
{
    public class AppSettings
    {
        public BackgroundWorkerSettings BackgroundWorkerJob { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
        public KycServiceSettings KycServiceSettings { get; set; }
    }
}