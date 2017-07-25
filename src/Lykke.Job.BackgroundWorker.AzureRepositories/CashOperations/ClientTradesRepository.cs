using System;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Job.BackgroundWorker.Core.Domain.CashOperations;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Job.BackgroundWorker.AzureRepositories.CashOperations
{
    public class ClientTradeEntity : TableEntity
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

            public static string GenerateRowKey(string tradeId)
            {
                return tradeId;
            }
        }

        public static class ByMultisig
        {
            public static string GeneratePartitionKey(string multisig)
            {
                return multisig;
            }

            public static string GenerateRowKey(string tradeId)
            {
                return tradeId;
            }
        }

        public static class ByDt
        {
            public static string GeneratePartitionKey()
            {
                return "dt";
            }

            public static string GenerateRowKey(string tradeId)
            {
                return tradeId;
            }
        }
    }

    public class ClientTradesRepository : IClientTradesRepository
    {
        private readonly INoSQLTableStorage<ClientTradeEntity> _tableStorage;

        public ClientTradesRepository(INoSQLTableStorage<ClientTradeEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task UpdateBlockChainHashAsync(string clientId, string recordId, string hash)
        {
            var partitionKey = ClientTradeEntity.ByClientId.GeneratePartitionKey(clientId);
            var rowKey = ClientTradeEntity.ByClientId.GenerateRowKey(recordId);

            var clientIdRecord = await _tableStorage.GetDataAsync(partitionKey, rowKey);

            var multisigPartitionKey = ClientTradeEntity.ByMultisig.GeneratePartitionKey(clientIdRecord.Multisig);
            var multisigRowKey = ClientTradeEntity.ByMultisig.GenerateRowKey(recordId);

            var dtPartitionKey = ClientTradeEntity.ByDt.GeneratePartitionKey();
            var dtRowKey = ClientTradeEntity.ByDt.GenerateRowKey(recordId);


            await _tableStorage.MergeAsync(partitionKey, rowKey, entity =>
            {
                entity.BlockChainHash = hash;
                entity.State = TransactionStates.SettledOnchain;
                return entity;
            });

            await _tableStorage.MergeAsync(multisigPartitionKey, multisigRowKey, entity =>
            {
                entity.BlockChainHash = hash;
                entity.State = TransactionStates.SettledOnchain;
                return entity;
            });

            await _tableStorage.MergeAsync(dtPartitionKey, dtRowKey, entity =>
            {
                entity.BlockChainHash = hash;
                entity.State = TransactionStates.SettledOnchain;
                return entity;
            });
        }
    }
}