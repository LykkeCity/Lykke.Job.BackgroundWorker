using System;

using Autofac;

using AzureStorage.Tables;

using Common.Log;

using Lykke.Job.BackgroundWorker.AzureRepositories.EventLogs;
using Lykke.Job.BackgroundWorker.Components;
using Lykke.Job.BackgroundWorker.Components.Workers;
using Lykke.Job.BackgroundWorker.Core.Domain.EventLogs;
using Lykke.Job.BackgroundWorker.Core.Services;
using Lykke.Job.BackgroundWorker.Core.Services.Geospatial;
using Lykke.Job.BackgroundWorker.Core.Settings;
using Lykke.Job.BackgroundWorker.Core.Settings.JobSettings;
using Lykke.Job.BackgroundWorker.Services;
using Lykke.Job.BackgroundWorker.Services.Geospatial;
using Lykke.Job.LykkeJob.Services;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.Kyc.Abstractions.Services;
using Lykke.Service.Kyc.Client;
using Lykke.SettingsReader;

namespace Lykke.Job.BackgroundWorker.Modules
{
    public class JobModule : Module
    {
        private readonly IReloadingManager<ClientAccountServiceClientSettings> _caSettings;
        private readonly IReloadingManager<BackgroundWorkerSettings> _jobSettings;
        private readonly ILog _log;
        private readonly IReloadingManager<DbSettings> _dbSettings;
        private readonly IReloadingManager<KycServiceSettings> _kycSettings;

        public JobModule(IReloadingManager<AppSettings> settings, ILog log)
        {
            _caSettings = settings.Nested(x => x.ClientAccountServiceClient);
            _jobSettings = settings.Nested(x => x.BackgroundWorkerJob);
            _kycSettings = settings.Nested(x => x.KycServiceSettings);
            _dbSettings = _jobSettings.Nested(x => x.Db);
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // NOTE: Do not register entire settings in container, pass necessary settings to services which requires them
            // ex:
            // builder.RegisterType<QuotesPublisher>()
            //  .As<IQuotesPublisher>()
            //  .WithParameter(TypedParameter.From(_settings.Rabbit.ConnectionString))

            builder.RegisterInstance(_jobSettings.CurrentValue)
                .SingleInstance();

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            // NOTE: You can implement your own poison queue notifier. See https://github.com/LykkeCity/JobTriggers/blob/master/readme.md
            // builder.Register<PoisionQueueNotifierImplementation>().As<IPoisionQueueNotifier>();

            builder.RegisterType<WorkersFactory>().As<IWorkersFactory>().SingleInstance();

            builder.RegisterType<SetPinWorker>();
            builder.RegisterType<SetAuthLogGeolocationWorker>();
            
            BindRepositories(builder);
            BindServices(builder);
        }

        private void BindServices(ContainerBuilder builder)
        {
            builder.RegisterType<SrvIpGeolocation>().As<ISrvIpGetLocation>().SingleInstance();

            builder.RegisterInstance<KycServiceSettings>(_kycSettings.CurrentValue).SingleInstance();
            builder.RegisterType<KycStatusServiceClient>().As<IKycStatusService>().SingleInstance();
            builder.RegisterType<KycCheckPersonServiceClient>().As<IKycCheckPersonService>().SingleInstance();
            builder.RegisterType<KycDocumentsServiceClient>().As<IKycDocumentsService>().SingleInstance();
            builder.RegisterType<KycProfileServiceClient>().As<IKycProfileService>().SingleInstance();

            builder.RegisterLykkeServiceClient(_caSettings.CurrentValue.ServiceUrl);
        }

        private void BindRepositories(ContainerBuilder builder)
        {
            builder.RegisterInstance<IAuthorizationLogsRepository>(
                new AuthorizationLogsRepository(
                    AzureTableStorage<AuthorizationLogRecordEntity>.Create(
                        _dbSettings.ConnectionString(s => s.LogsConnString), "AuthLogs", _log)));
        }
    }
}