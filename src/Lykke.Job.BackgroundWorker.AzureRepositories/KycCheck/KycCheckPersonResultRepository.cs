using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Common;
using Lykke.Job.BackgroundWorker.Core.Domain.KycCheckService;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace Lykke.Job.BackgroundWorker.AzureRepositories.KycCheck
{
    public class KycCheckPersonProfile : IKycCheckPersonProfile
    {
        public string Name { get; set; }
        public List<string> DatesOfBirth { get; set; }
        public List<string> Citizenships { get; set; }
        public List<string> Residences { get; set; }
        public List<string> MatchingLegalCategories { get; set; }
    }

    public class KycCheckPersonResult : TableEntity, IKycCheckPersonResult
    {
        public List<IKycCheckPersonProfile> PersonProfiles { get; set; }
        public string Id { get; set; }
        public long VerificationId { get; set; }
    }

    public class KycCheckPersonResultEntity : TableEntity
    {
        [JsonProperty("PersonProfiles")]
        public string PersonProfiles { get; set; }
        public long VerificationId { get; set; }
    }

    public class KycCheckPersonResultRepository : IKycCheckPersonResultRepository
    {
        private readonly INoSQLTableStorage<KycCheckPersonResultEntity> _tableStorage;

        public KycCheckPersonResultRepository(INoSQLTableStorage<KycCheckPersonResultEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task SaveAsync(IKycCheckPersonResult res)
        {
            KycCheckPersonResultEntity entity = new KycCheckPersonResultEntity
            {
                PartitionKey = res.Id,
                RowKey = (long.MaxValue - DateTime.UtcNow.Ticks).ToString(),
                VerificationId = res.VerificationId
            };

            if (res.PersonProfiles != null)
            {
                entity.PersonProfiles = res.PersonProfiles.ToJson();
            }

            await _tableStorage.InsertOrReplaceAsync(entity);
        }
    }
}