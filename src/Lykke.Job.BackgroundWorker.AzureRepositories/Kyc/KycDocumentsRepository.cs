using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Job.BackgroundWorker.Core.Domain.Kyc;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Job.BackgroundWorker.AzureRepositories.Kyc
{
    public class KycDocumentEntity : TableEntity, IKycDocument
    {
        public static string GeneratePartitionKey(string clientId)
        {
            return clientId;
        }

        public static string GenerateRowKey(string documentId)
        {
            return documentId;
        }


        public string ClientId => PartitionKey;
        public string DocumentId => RowKey;
        public string Type { get; set; }
        public string Mime { get; set; }
        public string KycComment { get; set; }
        public string State { get; set; }
        public string FileName { get; set; }
        public DateTime DateTime { get; set; }
        public string DocumentName { get; set; }

        public static KycDocumentEntity Create(IKycDocument src)
        {
            return new KycDocumentEntity
            {
                PartitionKey = GeneratePartitionKey(src.ClientId),
                RowKey = GenerateRowKey(src.DocumentId ?? Guid.NewGuid().ToString()),
                Type = src.Type,
                Mime = src.Mime,
                DateTime = src.DateTime,
                FileName = src.FileName,
                State = src.State,
                KycComment = src.KycComment,
                DocumentName = src.DocumentName
            };
        }

    }


    public class KycDocumentsRepository : IKycDocumentsRepository
    {
        private readonly INoSQLTableStorage<KycDocumentEntity> _tableStorage;

        public KycDocumentsRepository(INoSQLTableStorage<KycDocumentEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IKycDocument> AddAsync(IKycDocument kycDocument)
        {
            var newDocument = KycDocumentEntity.Create(kycDocument);
            await _tableStorage.InsertAsync(newDocument);
            return newDocument;
        }

        public async Task AddAsync(IEnumerable<IKycDocument> kycDocuments)
        {
            IEnumerable<KycDocumentEntity> newEntities = kycDocuments.Select(x => KycDocumentEntity.Create(x));

            await _tableStorage.InsertOrReplaceAsync(newEntities);
        }

        public async Task<IEnumerable<IKycDocument>> GetAsync(string clientId)
        {
            var partitionKey = KycDocumentEntity.GeneratePartitionKey(clientId);
            return await _tableStorage.GetDataAsync(partitionKey);
        }

        public async Task<IKycDocument> GetAsync(string clientId, string documentId)
        {
            return await _tableStorage.GetDataAsync(KycDocumentEntity.GeneratePartitionKey(clientId),
                KycDocumentEntity.GenerateRowKey(documentId));
        }

        public async Task<IEnumerable<IKycDocument>> GetKycSpiderDocuments(string clientId, bool showDeclined = false)
        {
            var partitionKey = KycDocumentEntity.GeneratePartitionKey(clientId);
            var docs = (await _tableStorage.GetDataAsync(partitionKey, doc => doc.Type.IsKycSpiderDocumentType())).ToList();

            var result = docs.Where(doc => showDeclined || doc.State != DocumentStates.Declined).ToList();

            return result;
        }

        public async Task<IEnumerable<IKycDocument>> GetOneEachTypeLatestAsync(string clientId, bool showDeclined = false)
        {
            var partitionKey = KycDocumentEntity.GeneratePartitionKey(clientId);
            var docs = (await _tableStorage.GetDataAsync(partitionKey)).ToList();

            var result = new List<IKycDocument>();
            var latestIdCard =
                docs.OrderByDescending(x => x.DateTime)
                    .FirstOrDefault(x => x.Type == KycDocumentTypeApi.IdCard.ToText() && (showDeclined || x.State != DocumentStates.Declined));
            if (latestIdCard != null)
                result.Add(latestIdCard);
            var latestSelfie =
                docs.OrderByDescending(x => x.DateTime)
                    .FirstOrDefault(x => x.Type == KycDocumentTypeApi.Selfie.ToText() && (showDeclined || x.State != DocumentStates.Declined));
            if (latestSelfie != null)
                result.Add(latestSelfie);
            var latestProofOfAddress =
                docs.OrderByDescending(x => x.DateTime)
                    .FirstOrDefault(x => x.Type == KycDocumentTypeApi.ProofOfAddress.ToText() && (showDeclined || x.State != DocumentStates.Declined));
            if (latestProofOfAddress != null)
                result.Add(latestProofOfAddress);

            return result;
        }

        public async Task<IKycDocument> DeleteAsync(string clientId, string documentId)
        {
            var partitionKey = KycDocumentEntity.GeneratePartitionKey(clientId);
            var rowKey = KycDocumentEntity.GenerateRowKey(documentId);
            return await _tableStorage.DeleteAsync(partitionKey, rowKey);
        }

        public async Task<IKycDocument> SetStateAndComment(string clientId, string documentId, string comment, string state)
        {
            return await _tableStorage.MergeAsync(KycDocumentEntity.GeneratePartitionKey(clientId),
                KycDocumentEntity.GenerateRowKey(documentId), entity =>
                {
                    entity.KycComment = comment;
                    entity.State = state;
                    return entity;
                });
        }

        public async Task SetLatestApproved(string clientId)
        {
            var docs = await GetOneEachTypeLatestAsync(clientId);

            foreach (var doc in docs)
            {
                await _tableStorage.MergeAsync(KycDocumentEntity.GeneratePartitionKey(clientId),
                    KycDocumentEntity.GenerateRowKey(doc.DocumentId), entity =>
                    {
                        entity.State = DocumentStates.Approved;
                        return entity;
                    });
            }
        }
    }

}