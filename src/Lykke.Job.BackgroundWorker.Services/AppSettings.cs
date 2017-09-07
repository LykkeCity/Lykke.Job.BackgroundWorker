using Lykke.Service.PersonalData.Settings;

namespace Lykke.Job.BackgroundWorker.Services
{
    public class AppSettings
    {
        public BackgroundWorkerSettings BackgroundWorkerJob { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }

        public class BackgroundWorkerSettings
        {
            public DbSettings Db { get; set; }
            public KycSpiderSettings KycSpiderSettings { get; set; }
            public PersonalDataServiceSettings PersonalDataServiceSettings { get; set; }
        }

        public class DbSettings
        {
            public string LogsConnString { get; set; }
            public string ClientPersonalInfoConnString { get; set; }
            public string HTradesConnString { get; set; }
        }

        public class KycSpiderSettings
        {
            public string User { get; set; }
            public string Password { get; set; }
            public string EndpointUrl { get; set; }
        }

        public class SlackNotificationsSettings
        {
            public AzureQueueSettings AzureQueue { get; set; }

            public int ThrottlingLimitSeconds { get; set; }
        }

        public class AzureQueueSettings
        {
            public string ConnectionString { get; set; }

            public string QueueName { get; set; }
        }
    }
}