using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Job.BackgroundWorker.Core.Domain.EventLogs;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Job.BackgroundWorker.AzureRepositories.EventLogs
{
    public class AuthorizationLogRecordEntity : TableEntity, IAuthorizationLogRecord
    {
        public static string GeneratePartitionKey(string clientId)
        {
            return clientId;
        }

        public string ClientId { get; set; }
        public DateTime DateTime { get; set; }
        public string UserAgent { get; set; }
        public string Ip { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public static AuthorizationLogRecordEntity Create(IAuthorizationLogRecord src)
        {
            return new AuthorizationLogRecordEntity
            {
                PartitionKey = GeneratePartitionKey(src.ClientId),
                DateTime = src.DateTime,
                ClientId = src.ClientId,
                UserAgent = src.UserAgent,
                Ip = src.Ip
            };
        }
    }

    public class AuthorizationLogsRepository : IAuthorizationLogsRepository
    {
        private readonly INoSQLTableStorage<AuthorizationLogRecordEntity> _tableStorage;

        public AuthorizationLogsRepository(INoSQLTableStorage<AuthorizationLogRecordEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<string> AddRecordAsync(IAuthorizationLogRecord record)
        {
            var newEntity = AuthorizationLogRecordEntity.Create(record);

            var res = await _tableStorage.InsertAndGenerateRowKeyAsDateTimeAsync(newEntity, record.DateTime);

            return res.RowKey;
        }

        public async Task<IEnumerable<IAuthorizationLogRecord>> GetAsync(string clientId, DateTime @from, DateTime to)
        {
            var partitionKey = AuthorizationLogRecordEntity.GeneratePartitionKey(clientId);

            return
                await _tableStorage.WhereAsync(partitionKey, @from.Date, to.Date.AddDays(1), ToIntervalOption.ExcludeTo);
        }

        public async Task<IAuthorizationLogRecord> GetRecordAsync(string clientId, string recordId)
        {
            var record = await _tableStorage.GetDataAsync(AuthorizationLogRecordEntity.GeneratePartitionKey(clientId),
                recordId);

            return record;
        }

        public Task UpdateGeoDataAsync(string clientId, string recordId, string country, string city)
        {
            return _tableStorage.ReplaceAsync(AuthorizationLogRecordEntity.GeneratePartitionKey(clientId),
                recordId, entity =>
                {
                    entity.Country = country;
                    entity.City = city;
                    return entity;
                });
        }
    }

}