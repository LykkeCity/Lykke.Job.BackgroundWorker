using Autofac;
using Autofac.Extensions.DependencyInjection;
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
using System;

namespace Lykke.Job.BackgroundWorker.Modules
{
    public class JobModule : Module
    {
        private readonly IReloadingManager<BackgroundWorkerSettings> _settings;
        private readonly ILog _log;
        private readonly IReloadingManager<DbSettings> _dbSettings;
        private readonly IReloadingManager<KycServiceSettings> _kycSettings;

        public JobModule(IReloadingManager<AppSettings> settings, ILog log)
        {
            _settings = settings.Nested(x => x.BackgroundWorkerJob);
            _kycSettings = settings.Nested(x => x.KycServiceSettings);
            _dbSettings = _settings.Nested(x => x.Db);
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // NOTE: Do not register entire settings in container, pass necessary settings to services which requires them
            // ex:
            // builder.RegisterType<QuotesPublisher>()
            //  .As<IQuotesPublisher>()
            //  .WithParameter(TypedParameter.From(_settings.Rabbit.ConnectionString))

            builder.RegisterInstance(_settings.CurrentValue)
                .SingleInstance();

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance()
                .WithParameter(TypedParameter.From(TimeSpan.FromSeconds(30)));

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            // NOTE: You can implement your own poison queue notifier. See https://github.com/LykkeCity/JobTriggers/blob/master/readme.md
            // builder.Register<PoisionQueueNotifierImplementation>().As<IPoisionQueueNotifier>();

            builder.RegisterType<WorkersFactory>().As<IWorkersFactory>().SingleInstance();

            builder.RegisterType<SetPinWorker>();
            builder.RegisterType<SetAuthLogGeolocationWorker>();
            builder.RegisterType<SetPartnerAccountInfoWorker>();
            builder.RegisterType<CheckPersonWorker>();

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

            builder.RegisterLykkeServiceClient(_settings.CurrentValue.ClientAccountServiceUrl);
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