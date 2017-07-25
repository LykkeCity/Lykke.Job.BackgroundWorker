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

        public async Task<IKycCheckPersonResult> GetAsync(string id)
        {
            KycCheckPersonResultEntity entity = await _tableStorage.GetTopRecordAsync(id);
            if (entity != null)
            {
                IKycCheckPersonResult result = new KycCheckPersonResult();
                if (entity.PersonProfiles != null)
                {
                    List<KycCheckPersonProfile> profiles = entity.PersonProfiles.DeserializeJson<List<KycCheckPersonProfile>>();
                    if (profiles != null)
                    {
                        result.PersonProfiles = new List<IKycCheckPersonProfile>();
                        result.PersonProfiles.AddRange(profiles);
                    }
                }
                return result;
            }
            return null;
        }

        public async void SaveAsync(IKycCheckPersonResult res)
        {
            KycCheckPersonResultEntity entity = new KycCheckPersonResultEntity();
            entity.PartitionKey = res.Id;
            entity.RowKey = (long.MaxValue - DateTime.UtcNow.Ticks).ToString();
            entity.VerificationId = res.VerificationId;
            if (res.PersonProfiles != null)
            {
                entity.PersonProfiles = res.PersonProfiles.ToJson();
            }
            await _tableStorage.InsertOrReplaceAsync(entity);
        }



    }
}