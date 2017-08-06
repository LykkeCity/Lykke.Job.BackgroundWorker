using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables.Templates.Index;
using Common.PasswordTools;
using Lykke.Job.BackgroundWorker.Core.Domain.Clients;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Job.BackgroundWorker.AzureRepositories.Clients
{
    public class ClientPartnerRelationEntity : TableEntity
    {
        public static string GeneratePartitionKey(string email)
        {
            return $"TraderPartnerRelation_{email}";
        }

        public static string GenerateRowKey(string partnerId)
        {
            return partnerId;
        }

        public DateTime Registered { get; set; }
        public string Id => RowKey;
        public string Email { get; set; }
        public string PartnerId { get; set; }
        public string ClientId { get; set; }
    }

    public class ClientAccountEntity : TableEntity, IClientAccount, IPasswordKeeping
    {
        public static string GeneratePartitionKey()
        {
            return "Trader";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

        public static IEqualityComparer<ClientAccountEntity> ComparerById { get; } = new EqualityComparerById();

        public DateTime Registered { get; set; }
        public string Id => RowKey;
        public string Email { get; set; }        
        public string Pin { get; set; }
        public string NotificationsId { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }        

        public bool IsReviewAccount { get; set; }

        private class EqualityComparerById : IEqualityComparer<ClientAccountEntity>
        {
            public bool Equals(ClientAccountEntity x, ClientAccountEntity y)
            {
                if (x == y)
                    return true;
                if (x == null || y == null)
                    return false;
                return x.Id == y.Id;
            }

            public int GetHashCode(ClientAccountEntity obj)
            {
                if (obj?.Id == null)
                    return 0;
                return obj.Id.GetHashCode();
            }
        }
    }


    public class ClientsRepository : IClientAccountsRepository
    {
        private readonly INoSQLTableStorage<ClientAccountEntity> _clientsTablestorage;
        private readonly INoSQLTableStorage<AzureIndex> _emailIndices;
        private readonly INoSQLTableStorage<ClientPartnerRelationEntity> _clientPartnerTablestorage;
        private const string IndexEmail = "IndexEmail";

        public ClientsRepository(INoSQLTableStorage<ClientAccountEntity> clientsTablestorage,
            INoSQLTableStorage<ClientPartnerRelationEntity> clientPartnerTablestorage,
            INoSQLTableStorage<AzureIndex> emailIndices)
        {
            _clientsTablestorage = clientsTablestorage;
            _emailIndices = emailIndices;
            _clientPartnerTablestorage = clientPartnerTablestorage;
        }

        public async Task<IEnumerable<string>> GetIdsAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            IEnumerable<ClientPartnerRelationEntity> relations =
                await _clientPartnerTablestorage.GetDataAsync(ClientPartnerRelationEntity.GeneratePartitionKey(email));
            IEnumerable<string> rowKeys = relations.Select(x => x.ClientId);

            return (await _clientsTablestorage.GetDataAsync(ClientAccountEntity.GeneratePartitionKey(), rowKeys))
                .Append(await _clientsTablestorage.GetDataAsync(_emailIndices, IndexEmail, GetEmailPartnerIndexRowKey(email, null)))
                .Except(new ClientAccountEntity[] { null }, ClientAccountEntity.ComparerById)
                .Distinct(ClientAccountEntity.ComparerById)
                .Select(a => a.Id)
                .ToArray();
        }

        public Task SetPin(string clientId, string newPin)
        {
            var partitionKey = ClientAccountEntity.GeneratePartitionKey();
            var rowKey = ClientAccountEntity.GenerateRowKey(clientId);

            return _clientsTablestorage.ReplaceAsync(partitionKey, rowKey, itm =>
            {
                itm.Pin = newPin;
                return itm;
            });
        }
        
        private string GetEmailPartnerIndexRowKey(string email, string partnerId)
        {
            string lowEmail = email.ToLower();
            return string.IsNullOrEmpty(partnerId) ? $"{lowEmail}" : $"{lowEmail}_{partnerId}";
        }
    }

}