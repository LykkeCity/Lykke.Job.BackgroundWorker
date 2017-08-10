using System;
using Autofac;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using Common.Log;
using Lykke.Job.BackgroundWorker.AzureRepositories.Clients;
using Lykke.Job.BackgroundWorker.AzureRepositories.EventLogs;
using Lykke.Job.BackgroundWorker.AzureRepositories.Kyc;
using Lykke.Job.BackgroundWorker.AzureRepositories.KycCheck;
using Lykke.Job.BackgroundWorker.Components;
using Lykke.Job.BackgroundWorker.Components.Workers;
using Lykke.Job.BackgroundWorker.Core;
using Lykke.Job.BackgroundWorker.Core.Domain.Clients;
using Lykke.Job.BackgroundWorker.Core.Domain.EventLogs;
using Lykke.Job.BackgroundWorker.Core.Domain.Kyc;
using Lykke.Job.BackgroundWorker.Core.Domain.KycCheckService;
using Lykke.Job.BackgroundWorker.Core.Services;
using Lykke.Job.BackgroundWorker.Core.Services.Geospatial;
using Lykke.Job.BackgroundWorker.Core.Services.PersonalData;
using Lykke.Job.BackgroundWorker.Services;
using Lykke.Job.BackgroundWorker.Services.Geospatial;
using Lykke.Job.BackgroundWorker.Services.PersonalData;
using Lykke.Service.OperationsRepository.Client;

namespace Lykke.Job.BackgroundWorker.Modules
{
    public class JobModule : Module
    {
        private readonly AppSettings.BackgroundWorkerSettings _settings;
        private readonly ILog _log;
        private readonly AppSettings.DbSettings _dbSettings;
        private readonly AppSettings.OperationsRepositoryClientSettings _repositorySettings;

        public JobModule(AppSettings settings, ILog log)
        {
            _settings = settings.BackgroundWorkerJob;
            _repositorySettings = settings.OperationsRepositoryClient;
            _dbSettings = _settings.Db;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_settings)
                .SingleInstance();

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
            builder.RegisterType<UpdateHashForOperationsWorker>();
            builder.RegisterType<SetPartnerAccountInfoWorker>();
            builder.RegisterType<CheckPersonWorker>()
                .WithParameter(TypedParameter.From(_settings.KycSpiderSettings));

            BindRepositories(builder);
            BindServices(builder);
        }

        private void BindServices(ContainerBuilder builder)
        {
            builder.RegisterType<SrvIpGeolocation>().As<ISrvIpGetLocation>().SingleInstance();
            
            builder.RegisterType<PersonalDataService>()
                .As<IPersonalDataService>()
                .WithParameter(TypedParameter.From(_settings.PersonalDataServiceSettings));

            builder.RegisterOperationsRepositoryClients(_repositorySettings.ServiceUrl, _log,
                _repositorySettings.RequestTimeout);
        }

        private void BindRepositories(ContainerBuilder builder)
        {
            builder.RegisterInstance<IClientAccountsRepository>(
            new ClientsRepository(
                new AzureTableStorage<ClientAccountEntity>(_dbSettings.ClientPersonalInfoConnString, "Traders", _log),
                new AzureTableStorage<ClientPartnerRelationEntity>(_dbSettings.ClientPersonalInfoConnString, "ClientPartnerRelations", _log),
                new AzureTableStorage<AzureIndex>(_dbSettings.ClientPersonalInfoConnString, "Traders", _log)));

            builder.RegisterInstance<IPersonalDataRepository>(
                new PersonalDataRepository(
                    new AzureTableStorage<PersonalDataEntity>(_dbSettings.ClientPersonalInfoConnString, "PersonalData", _log)));

            builder.RegisterInstance<IAuthorizationLogsRepository>(
                new AuthorizationLogsRepository(
                    new AzureTableStorage<AuthorizationLogRecordEntity>(_dbSettings.LogsConnString, "AuthLogs", _log)));

            builder.RegisterInstance<IKycDocumentsRepository>(
                new KycDocumentsRepository(
                    new AzureTableStorage<KycDocumentEntity>(_dbSettings.ClientPersonalInfoConnString, "KycDocuments", _log)));

            builder.RegisterInstance<IKycRepository>(
                new KycRepository(new AzureTableStorage<KycEntity>(_dbSettings.ClientPersonalInfoConnString, "KycStatuses", _log)));

            builder.RegisterInstance<IKycCheckPersonResultRepository>(
                new KycCheckPersonResultRepository(
                    new AzureTableStorage<KycCheckPersonResultEntity>(_dbSettings.ClientPersonalInfoConnString, "KycCheckPersonResults", _log)));

        }
    }
}