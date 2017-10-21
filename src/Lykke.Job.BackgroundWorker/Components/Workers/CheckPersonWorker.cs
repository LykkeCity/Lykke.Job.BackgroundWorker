using System;
using System.Linq;
using System.Threading.Tasks;

using Common;

using Lykke.Job.BackgroundWorker.Contract.Contexts;
using Lykke.Service.Kyc.Abstractions.Domain.Verification;
using Lykke.Service.Kyc.Abstractions.Services;

namespace Lykke.Job.BackgroundWorker.Components.Workers
{
    public class CheckPersonWorker: IWorker
    {
        private CheckPersonContext _context;
        private readonly IKycCheckPersonService _kycCheckPersonService;
        private readonly IKycStatusService _kycStatusService;


        public CheckPersonWorker(IKycCheckPersonService kycCheckPersonService, IKycStatusService kycStatusService)
        {
            _kycCheckPersonService = kycCheckPersonService;
            _kycStatusService = kycStatusService;
        }

        public async Task DoWork()
        {
            if (_context == null)
            {
                throw new InvalidOperationException("Context was not set.");
            }

            var clientId = _context.ClientId;

            var result = await _kycCheckPersonService.CheckPersonAsync(clientId);

            if (result.PersonProfiles == null || !result.PersonProfiles.Any()) // change status automatically from ReviewDone to Ok
            {
                if (await _kycStatusService.GetKycStatusAsync(clientId) == KycStatus.ReviewDone)
                {
                    await _kycStatusService.ChangeKycStatusAsync(clientId, KycStatus.Ok, "KycCheckPersonService");
                }
            }

            await _kycStatusService.SaveCheckPersonResultAsync(result);

        }

        public void SetContext(string contextJson)
        {
            _context = contextJson.DeserializeJson<CheckPersonContext>();
            if (_context == null)
            {
                throw new ArgumentException(nameof(contextJson));
            }
        }
    }
}
