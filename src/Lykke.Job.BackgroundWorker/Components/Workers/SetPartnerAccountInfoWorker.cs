using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Lykke.Job.BackgroundWorker.Contract.Contexts;
using Lykke.Job.BackgroundWorker.Core.Domain.Clients;
using Lykke.Job.BackgroundWorker.Core.Domain.Kyc;
using Lykke.Job.BackgroundWorker.Core.Services.PersonalData;

namespace Lykke.Job.BackgroundWorker.Components.Workers
{
    public class SetPartnerAccountInfoWorker : IWorker
    {
        private readonly IClientAccountsRepository _clientAccountRepository;
        private SetPartnerAccountInfoWorkerContext _context;
        private readonly IKycDocumentsRepository _kycDocumentsRepository;
        private readonly IKycRepository _kycRepository;
        private readonly IPersonalDataService _personalDataService;

        public SetPartnerAccountInfoWorker(IClientAccountsRepository clientAccountRepository,
            IPersonalDataService personalDataService, IKycRepository kycRepository,
            IKycDocumentsRepository kycDocumentsRepository)
        {
            _personalDataService = personalDataService;
            _kycRepository = kycRepository;
            _clientAccountRepository = clientAccountRepository;
            _kycDocumentsRepository = kycDocumentsRepository;
        }

        public async Task DoWork()
        {
            if (_context == null)
            {
                throw new Exception("context was not set");
            }

            string email = _context.Email;
            //accounts for the same email
            IEnumerable<IClientAccount> accounts = await _clientAccountRepository.GetByEmailAsync(email);

            if (accounts == null || accounts.Count() <= 1)
            {
                return;
            }

            var clientIds = accounts.Select(x => x.Id);
            IDictionary<string, KycStatus> clientKycStatuses = await _kycRepository.GetKycStatusAsync(clientIds);

            var passedKycClientId = clientKycStatuses.Where(x => x.Value == KycStatus.Ok).FirstOrDefault().Key;

            if (passedKycClientId != null)
            {
                //Copy personal data and kyc to the new account
                IEnumerable<IKycDocument> kycDocuments = await _kycDocumentsRepository.GetAsync(passedKycClientId);
                IFullPersonalData personalData = await _personalDataService.GetFullAsync(passedKycClientId);
                IEnumerable<string> clientsExceptMain = clientIds.Except(new string[] { passedKycClientId });
                IDictionary<string, IFullPersonalData> oldFullPersonalDataDict =
                    (await _personalDataService.GetFullAsync(clientsExceptMain)).ToDictionary(x => x.Id);

                foreach (string clientId in clientsExceptMain)
                {
                    IFullPersonalData oldPersonalData;
                    oldFullPersonalDataDict.TryGetValue(clientId, out oldPersonalData);
                    IFullPersonalData newPersonalData = new FullPersonalData()
                    {
                        Address = personalData.Address,
                        City = personalData.City,
                        ContactPhone = personalData.ContactPhone,
                        Country = personalData.Country,
                        Email = personalData.Email,
                        FirstName = personalData.FirstName,
                        FullName = personalData.LastName,
                        Id = clientId,
                        Zip = personalData.Zip,
                        LastName = personalData.LastName,
                        PasswordHint = oldPersonalData?.PasswordHint,
                        ReferralCode = oldPersonalData?.ReferralCode,
                        Regitered = DateTime.UtcNow
                    };

                    await _personalDataService.UpdateAsync(newPersonalData);

                    IEnumerable<IKycDocument> newKycDocuments = kycDocuments.Select(oldDocument =>
                    {
                        IKycDocument newKycDocument = new KycDocument()
                        {
                            ClientId = clientId,
                            DateTime = DateTime.UtcNow,
                            DocumentId = oldDocument.DocumentId,
                            FileName = oldDocument.FileName,
                            Mime = oldDocument.Mime,
                            State = oldDocument.State,
                            KycComment = oldDocument.KycComment,
                            Type = oldDocument.Type
                        };

                        return newKycDocument;
                    });

                    await _kycDocumentsRepository.AddAsync(newKycDocuments);
                }

                await _kycRepository.SetStatusAsync(clientsExceptMain, KycStatus.Ok);
            }
        }

        public void SetContext(string contextJson)
        {
            _context = contextJson.DeserializeJson<SetPartnerAccountInfoWorkerContext>();

            if (_context == null)
            {
                throw new ArgumentException(nameof(contextJson));
            }
        }
    }
}
