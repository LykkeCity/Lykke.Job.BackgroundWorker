using System;
using System.Linq;
using System.Threading.Tasks;

using Common;

using Lykke.Job.BackgroundWorker.Contract.Contexts;

using Lykke.Service.ClientAccount.Client;
using Lykke.Service.Kyc.Abstractions.Domain.Verification;
using Lykke.Service.Kyc.Abstractions.Services;

namespace Lykke.Job.BackgroundWorker.Components.Workers
{
    public class SetPartnerAccountInfoWorker : IWorker
    {
        private SetPartnerAccountInfoWorkerContext _context;
        private readonly IClientAccountClient _clientAccountService;
        private readonly IKycStatusService _kycStatusService;
        private readonly IKycDocumentsService _kycDocumentsService;
        private readonly IKycProfileService _kycProfileService;

        public SetPartnerAccountInfoWorker(
            IClientAccountClient clientAccountService, 
            IKycStatusService kycStatusService,
            IKycDocumentsService kycDocumentsService,
            IKycProfileService kycProfileService)
        {
            _clientAccountService = clientAccountService;
            _kycStatusService = kycStatusService;
            _kycDocumentsService = kycDocumentsService;
            _kycProfileService = kycProfileService;
        }

        public async Task DoWork()
        {
            if (_context == null) {
                throw new InvalidOperationException("Context was not set.");
            }

            var accounts = await _clientAccountService.GetClientsByEmail(_context.Email);
            if (accounts == null || accounts.Count() <= 1) {
                return;
            }

            var clientKycStatuses = await _kycStatusService.GetKycStatusAsync(accounts.Select(x => x.Id));

            var passedKycClientId = clientKycStatuses.FirstOrDefault(x => x.Value == KycStatus.Ok).Key;
            if (string.IsNullOrEmpty(passedKycClientId)) {
                return;
            }

            var clientsExceptMain = clientKycStatuses.Keys.Except(new[] { passedKycClientId }).ToArray();

            await _kycDocumentsService.CopyDocumentsAsunc(passedKycClientId, clientsExceptMain);
            await _kycProfileService.CopyPersonalDataAsunc(passedKycClientId, clientsExceptMain);
            await _kycStatusService.CopyeKycStatusAsunc(passedKycClientId, clientsExceptMain);
        }

        public void SetContext(string contextJson)
        {
            _context = contextJson.DeserializeJson<SetPartnerAccountInfoWorkerContext>();

            if (_context == null) {
                throw new ArgumentException(nameof(contextJson));
            }
        }
    }
}
