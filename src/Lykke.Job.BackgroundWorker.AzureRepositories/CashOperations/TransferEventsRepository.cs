using System;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables.Templates.Index;
using Lykke.Job.BackgroundWorker.Core.Domain.CashOperations;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Job.BackgroundWorker.AzureRepositories.CashOperations
{
    public class TransferEventEntity : TableEntity
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

            public static string GenerateRowKey(string id)
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

            public static string GenerateRowKey(string id)
            {
                return id;
            }
        }
    }

    public class TransferEventsRepository : ITransferEventsRepository
    {
        private readonly INoSQLTableStorage<TransferEventEntity> _tableStorage;
        private readonly INoSQLTableStorage<AzureIndex> _blockChainHashIndices;

        public TransferEventsRepository(INoSQLTableStorage<TransferEventEntity> tableStorage, INoSQLTableStorage<AzureIndex> blockChainHashIndices)
        {
            _tableStorage = tableStorage;
            _blockChainHashIndices = blockChainHashIndices;
        }

        public async Task UpdateBlockChainHashAsync(string clientId, string id, string blockChainHash)
        {
            var partitionKey = TransferEventEntity.ByClientId.GeneratePartitionKey(clientId);
            var rowKey = TransferEventEntity.ByClientId.GenerateRowKey(id);

            var item = await _tableStorage.GetDataAsync(partitionKey, rowKey);

            if (item.State == TransactionStates.SettledOffchain || item.State == TransactionStates.InProcessOffchain)
                return;

            item.BlockChainHash = blockChainHash;

            var multisigPartitionKey = TransferEventEntity.ByMultisig.GeneratePartitionKey(item.Multisig);
            var multisigRowKey = TransferEventEntity.ByMultisig.GenerateRowKey(id);

            var multisigItem = await _tableStorage.GetDataAsync(multisigPartitionKey, multisigRowKey);
            multisigItem.BlockChainHash = blockChainHash;
            multisigItem.State = TransactionStates.SettledOnchain;

            var indexEntity = AzureIndex.Create(blockChainHash, rowKey, partitionKey, rowKey);
            await _blockChainHashIndices.InsertOrReplaceAsync(indexEntity);

            await _tableStorage.InsertOrReplaceAsync(item);
            await _tableStorage.InsertOrReplaceAsync(multisigItem);
        }
    }
}