using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Common;
using Lykke.Job.BackgroundWorker.Core.Domain.Kyc;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Job.BackgroundWorker.AzureRepositories.Kyc
{
    public class KycEntity : TableEntity
    {

        internal const KycStatus DefaultStatus = KycStatus.NeedToFillData;

        public static string GeneratePartitionKey(KycStatus kycStatus)
        {
            return kycStatus.ToString();
        }

        public static string GenerateRowKey(string clientId)
        {
            return clientId;
        }


        internal string GetClientId()
        {
            return RowKey;
        }

        internal KycStatus GetSatus()
        {
            try
            {
                return PartitionKey.ParseEnum<KycStatus>();
            }
            catch (Exception)
            {

                return DefaultStatus;
            }
        }

        public static KycEntity Create(string clientId, KycStatus status)
        {
            return new KycEntity
            {
                PartitionKey = GeneratePartitionKey(status),
                RowKey = GenerateRowKey(clientId)
            };
        }

    };


    public class KycRepository : IKycRepository
    {
        private readonly INoSQLTableStorage<KycEntity> _tableStorage;


        public KycRepository(INoSQLTableStorage<KycEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<KycStatus> GetKycStatusAsync(string clientId)
        {
            var rowKey = KycEntity.GenerateRowKey(clientId);
            var entity = (await _tableStorage.GetDataRowKeyOnlyAsync(rowKey)).FirstOrDefault();

            return entity?.GetSatus() ?? KycEntity.DefaultStatus;
        }

        public async Task<Dictionary<string, KycStatus>> GetKycStatusesAsync()
        {
            return (await _tableStorage.GetDataAsync()).ToDictionary(item => item.GetClientId(), entity => entity.GetSatus());
        }

        public async Task<IEnumerable<string>> GetClientsByStatus(KycStatus kycStatus)
        {
            var partitionKey = KycEntity.GeneratePartitionKey(kycStatus);
            return (await _tableStorage.GetDataAsync(partitionKey)).OrderBy(x => x.Timestamp)
                .Select(itm => itm.GetClientId());
        }

        public async Task SetStatusAsync(string clientId, KycStatus status)
        {
            var rowKey = KycEntity.GenerateRowKey(clientId);
            var entity = (await _tableStorage.GetDataRowKeyOnlyAsync(rowKey)).FirstOrDefault();

            if (entity != null)
                await _tableStorage.DeleteAsync(entity);

            if (status == KycEntity.DefaultStatus)
                return;

            entity = KycEntity.Create(clientId, status);
            await _tableStorage.InsertOrReplaceAsync(entity);
        }

        public async Task<IDictionary<string, KycStatus>> GetKycStatusAsync(IEnumerable<string> clientIds)
        {
            var rowKeys = clientIds.Select(x => KycEntity.GenerateRowKey(x));
            var entities = await _tableStorage.GetDataRowKeysOnlyAsync(rowKeys);

            return entities.ToDictionary(x => x.GetClientId(), y => y.GetSatus());
        }

        public async Task SetStatusAsync(IEnumerable<string> clientIds, KycStatus status)
        {
            var entities = clientIds.Select(clientId => KycEntity.Create(clientId, status));

            await _tableStorage.InsertOrReplaceAsync(entities);
        }
    }

}