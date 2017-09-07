using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Job.BackgroundWorker.Core.Domain.KycCheckService
{
    public interface IKycCheckPersonProfile
    {
        string Name { get; set; }
        List<string> DatesOfBirth { get; set; }
        List<string> Citizenships { get; set; }
        List<string> Residences { get; set; }
        List<string> MatchingLegalCategories { get; set; }
        string SpiderProfileId { get; set; }
    }

    public interface IKycCheckPersonResult
    {
        List<IKycCheckPersonProfile> PersonProfiles { get; set; }
        string Id { get; set; }
        long VerificationId { get; set; }
    }

    public interface IKycCheckPersonResultRepository
    {
        Task SaveAsync(IKycCheckPersonResult obj);
    }
}