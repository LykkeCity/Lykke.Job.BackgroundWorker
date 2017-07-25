using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Job.BackgroundWorker.Core.Domain.Kyc
{
    public enum KycStatus
    {
        NeedToFillData, Pending, ReviewDone, Ok, Rejected, RestrictedArea, Complicated
    }

    public interface IKycRepository
    {
        Task<IDictionary<string, KycStatus>> GetKycStatusAsync(IEnumerable<string> clientIds);
        Task<KycStatus> GetKycStatusAsync(string clientId);
        Task<Dictionary<string, KycStatus>> GetKycStatusesAsync();

        Task<IEnumerable<string>> GetClientsByStatus(KycStatus kycStatus);

        Task SetStatusAsync(string clientId, KycStatus status);
        Task SetStatusAsync(IEnumerable<string> clientIds, KycStatus status);
    }

    public static class KycExt
    {
        public static bool IsKycOkOrReviewDone(this KycStatus status)
        {
            return status == KycStatus.Ok || status == KycStatus.ReviewDone;
        }
    }
}