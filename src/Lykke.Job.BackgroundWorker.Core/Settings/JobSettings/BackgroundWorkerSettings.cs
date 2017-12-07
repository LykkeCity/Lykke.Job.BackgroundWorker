using Lykke.Service.Kyc.Client;

namespace Lykke.Job.BackgroundWorker.Core.Settings.JobSettings
{
    public class BackgroundWorkerSettings
    {
        public DbSettings Db { get; set; }
        public string ClientAccountServiceUrl { get; set; }
    }
}