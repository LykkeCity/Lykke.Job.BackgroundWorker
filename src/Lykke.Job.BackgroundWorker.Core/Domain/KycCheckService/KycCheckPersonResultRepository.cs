using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Job.BackgroundWorker.Core.Domain.KycCheckService
{
    public enum MatchingLegalCategoryType { Sanction, Pep, Int_Pep, Ch_Pep, Pep_Rca, Ctrl_Org, Crime, Blacklist, Information, Reputation }

    public interface IKycCheckPersonProfile
    {
        string Name { get; set; }
        List<string> DatesOfBirth { get; set; }
        List<string> Citizenships { get; set; }
        List<string> Residences { get; set; }
        List<string> MatchingLegalCategories { get; set; }
    }

    public interface IKycCheckPersonResult
    {
        List<IKycCheckPersonProfile> PersonProfiles { get; set; }
        string Id { get; set; }
        long VerificationId { get; set; }
    }

    public interface IKycCheckPersonResultRepository
    {
        void SaveAsync(IKycCheckPersonResult obj);
        Task<IKycCheckPersonResult> GetAsync(string id);
    }
}