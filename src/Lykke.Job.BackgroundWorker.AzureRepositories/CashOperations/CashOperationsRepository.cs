using System;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables.Templates.Index;
using Lykke.Job.BackgroundWorker.Core.Domain.CashOperations;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Job.BackgroundWorker.AzureRepositories.CashOperations
{
    public class CashInOutOperationEntity : TableEntity
    {
        public string Id => RowKey;
        public string BlockChainHash { get; set; }
        public string Multisig { get; set; }
        public string StateField { get; set; }
        public TransactionStates State
        {
            get
            {
                TransactionStates type = TransactionStates.InProcessOnchain;
                if (!string.IsNullOrEmpty(StateField))
                {
                    Enum.TryParse(StateField, out type);
                }
                return type;
            }
            set { StateField = value.ToString(); }
        }

        public static class ByClientId
        {
            public static string GeneratePartitionKey(string clientId)
            {
                return clientId;
            }

            internal static string GenerateRowKey(string id)
            {
                return id;
            }
        }

        public static class ByMultisig
        {
            public static string GeneratePartitionKey(string multisig)
            {
                return multisig;
            }

            internal static string GenerateRowKey(string id)
            {
                return id;
            }
        }
    }

    public class CashOperationsRepository : ICashOperationsRepository
    {
        private readonly INoSQLTableStorage<CashInOutOperationEntity> _tableStorage;
        private readonly INoSQLTableStorage<AzureIndex> _blockChainHashIndices;

        public CashOperationsRepository(INoSQLTableStorage<CashInOutOperationEntity> tableStorage, INoSQLTableStorage<AzureIndex> blockChainHashIndices)
        {
            _tableStorage = tableStorage;
            _blockChainHashIndices = blockChainHashIndices;
        }

        public async Task UpdateBlockchainHashAsync(string clientId, string id, string hash)
        {
            var partitionkey = CashInOutOperationEntity.ByClientId.GeneratePartitionKey(clientId);
            var rowKey = CashInOutOperationEntity.ByClientId.GenerateRowKey(id);

            var record = await _tableStorage.GetDataAsync(partitionkey, rowKey);

            var multisigPartitionkey = CashInOutOperationEntity.ByMultisig.GeneratePartitionKey(record.Multisig);
            var multisigRowKey = CashInOutOperationEntity.ByMultisig.GenerateRowKey(id);

            var indexEntity = AzureIndex.Create(hash, rowKey, partitionkey, rowKey);
            await _blockChainHashIndices.InsertOrReplaceAsync(indexEntity);

            await _tableStorage.MergeAsync(partitionkey, rowKey, entity =>
            {
                entity.BlockChainHash = hash;
                entity.State = TransactionStates.SettledOnchain;
                return entity;
            });

            await _tableStorage.MergeAsync(multisigPartitionkey, multisigRowKey, entity =>
            {
                entity.BlockChainHash = hash;
                entity.State = TransactionStates.SettledOnchain;
                return entity;
            });
        }
    }
}