using System;
using Autofac;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using Common.Log;
using Lykke.Job.BackgroundWorker.AzureRepositories.CashOperations;
using Lykke.Job.BackgroundWorker.AzureRepositories.Clients;
using Lykke.Job.BackgroundWorker.AzureRepositories.EventLogs;
using Lykke.Job.BackgroundWorker.AzureRepositories.Kyc;
using Lykke.Job.BackgroundWorker.AzureRepositories.KycCheck;
using Lykke.Job.BackgroundWorker.Components;
using Lykke.Job.BackgroundWorker.Components.Workers;
using Lykke.Job.BackgroundWorker.Core.Domain.CashOperations;
using Lykke.Job.BackgroundWorker.Core.Domain.Clients;
using Lykke.Job.BackgroundWorker.Core.Domain.EventLogs;
using Lykke.Job.BackgroundWorker.Core.Domain.Kyc;
using Lykke.Job.BackgroundWorker.Core.Domain.KycCheckService;
using Lykke.Job.BackgroundWorker.Core.Services;
using Lykke.Job.BackgroundWorker.Core.Services.Geospatial;
using Lykke.Job.BackgroundWorker.Services;
using Lykke.Job.BackgroundWorker.Services.Geospatial;
using Lykke.Job.BackgroundWorker.Services.KycCheckService;
using Lykke.Service.PersonalData.Client;
using Lykke.Service.PersonalData.Contract;
using Lykke.SettingsReader;

namespace Lykke.Job.BackgroundWorker.Modules
{
    public class JobModule : Module
    {
        private readonly IReloadingManager<AppSettings.BackgroundWorkerSettings> _settings;
        private readonly ILog _log;
        private readonly IReloadingManager<AppSettings.DbSettings> _dbSettings;

        public JobModule(IReloadingManager<AppSettings.BackgroundWorkerSettings> settings, ILog log)
        {
            _settings = settings;
            _dbSettings = settings.Nested(x => x.Db);
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_settings.CurrentValue)
                .SingleInstance();

            builder.RegisterInstance(_settings.CurrentValue.KycSpiderSettings);

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance()
                .WithParameter(TypedParameter.From(TimeSpan.FromSeconds(30)));

            // NOTE: You can implement your own poison queue notifier. See https://github.com/LykkeCity/JobTriggers/blob/master/readme.md
            // builder.Register<PoisionQueueNotifierImplementation>().As<IPoisionQueueNotifier>();

            builder.RegisterType<WorkersFactory>().As<IWorkersFactory>().SingleInstance();

            builder.RegisterType<SetPinWorker>();
            builder.RegisterType<SetAuthLogGeolocationWorker>();
            builder.RegisterType<SetPartnerAccountInfoWorker>();

            builder.RegisterType<KycCheckService>();
            builder.RegisterType<CheckPersonWorker>();

            BindRepositories(builder);
            BindServices(builder);
        }

        private void BindServices(ContainerBuilder builder)
        {
            builder.RegisterType<SrvIpGeolocation>().As<ISrvIpGetLocation>().SingleInstance();

            builder.RegisterType<PersonalDataService>()
                .As<IPersonalDataService>()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.PersonalDataServiceSettings));
        }

        private void BindRepositories(ContainerBuilder builder)
        {
            builder.RegisterInstance<ICashOperationsRepository>(
                new CashOperationsRepository(
                    AzureTableStorage<CashInOutOperationEntity>.Create(
                        _dbSettings.ConnectionString(s => s.ClientPersonalInfoConnString), "OperationsCash", _log),
                    AzureTableStorage<AzureIndex>.Create(
                        _dbSettings.ConnectionString(s => s.ClientPersonalInfoConnString), "OperationsCash", _log)
                ));

            builder.RegisterInstance<IClientTradesRepository>(new ClientTradesRepository(
                AzureTableStorage<ClientTradeEntity>.Create(_dbSettings.ConnectionString(x => x.HTradesConnString),
                    "Trades", _log)));

            builder.RegisterInstance<ITransferEventsRepository>(
                new TransferEventsRepository(
                    AzureTableStorage<TransferEventEntity>.Create(
                        _dbSettings.ConnectionString(s => s.ClientPersonalInfoConnString), "Transfers", _log),
                    AzureTableStorage<AzureIndex>.Create(
                        _dbSettings.ConnectionString(s => s.ClientPersonalInfoConnString), "Transfers", _log)));

            builder.RegisterInstance<IClientAccountsRepository>(
                new ClientsRepository(
                    AzureTableStorage<ClientAccountEntity>.Create(
                        _dbSettings.ConnectionString(x => x.ClientPersonalInfoConnString), "Traders", _log),
                    AzureTableStorage<ClientPartnerRelationEntity>.Create(
                        _dbSettings.ConnectionString(s => s.ClientPersonalInfoConnString), "ClientPartnerRelations",
                        _log),
                    AzureTableStorage<AzureIndex>.Create(
                        _dbSettings.ConnectionString(s => s.ClientPersonalInfoConnString), "Traders", _log)));

            builder.RegisterInstance<IAuthorizationLogsRepository>(
                new AuthorizationLogsRepository(
                    AzureTableStorage<AuthorizationLogRecordEntity>.Create(
                        _dbSettings.ConnectionString(s => s.LogsConnString), "AuthLogs", _log)));

            builder.RegisterInstance<IKycDocumentsRepository>(
                new KycDocumentsRepository(
                    AzureTableStorage<KycDocumentEntity>.Create(
                        _dbSettings.ConnectionString(s => s.ClientPersonalInfoConnString), "KycDocuments", _log)));

            builder.RegisterInstance<IKycRepository>(new KycRepository(
                AzureTableStorage<KycEntity>.Create(_dbSettings.ConnectionString(s => s.ClientPersonalInfoConnString),
                    "KycStatuses", _log)));

            builder.RegisterInstance<IKycCheckPersonResultRepository>(new KycCheckPersonResultRepository(
                AzureTableStorage<KycCheckPersonResultEntity>.Create(
                    _dbSettings.ConnectionString(s => s.ClientPersonalInfoConnString), "KycCheckPersonResults", _log)));
        }
    }
}