using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Lykke.Service.PersonalData.Contract;
using Lykke.Service.PersonalData.Contract.Models;
using Lykke.Service.Kyc.Abstractions.Services;
using Lykke.Service.Kyc.Abstractions.Domain.Verification;
using static Lykke.Job.BackgroundWorker.Services.AppSettings;

namespace Lykke.Job.BackgroundWorker.Services.KycCheckService
{
    public class KycCheckService
    {
        private readonly IPersonalDataService _personalDataService;
        private readonly AppSettings.KycSpiderSettings _settings;
        private readonly IKycStatusService _kycStatusService;

        public KycCheckService(IPersonalDataService personalDataService,
            KycSpiderSettings settings,
            IKycStatusService kycStatusService
            )
        {
            _personalDataService = personalDataService;
            _settings = settings;
            _kycStatusService = kycStatusService;
        }

        public async Task CheckPerson(string clientId)
        {
            var personalData = await _personalDataService.GetAsync(clientId);
            PersonCheckData checkData = new PersonCheckData
            {
                residences = new string[] { },
                citizenships = new string[] { },
                datesOfBirth = new IncompleteDate[] { },
                customerId = personalData.Id,
                names = new[]
                {
                    new PersonName {firstName = personalData.FirstName, lastName = personalData.LastName}
                }
            };

            checkData.citizenships = new[] {
                personalData.CountryFromID
            };
            checkData.datesOfBirth = new[] {
                new IncompleteDate { year = personalData.DateOfBirth?.Year ?? -1, month = personalData.DateOfBirth?.Month ?? -1, day = personalData.DateOfBirth?.Day ?? -1 }
            };
            checkPerson request = new checkPerson(checkData);

            BasicHttpsBinding binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
            binding.MaxReceivedMessageSize = 1024 * 512;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            EndpointAddress epa = new EndpointAddress(_settings.EndpointUrl);
            ChannelFactory<accessChannel> factory = new ChannelFactory<accessChannel>(binding, epa);
            factory.Credentials.UserName.UserName = _settings.User;
            factory.Credentials.UserName.Password = _settings.Password;

            accessChannel channel = factory.CreateChannel();
            checkPersonResponse response = await channel.checkPersonAsync(request);

            KycCheckPersonResult result = FillCheckPersonResult(personalData, response);
            if (!isPersonSuspicious(result)) // change status automatically from ReviewDone to Ok
            {
                if (await _kycStatusService.GetKycStatusAsync(clientId) == KycStatus.ReviewDone)
                {
                    await _kycStatusService.ChangeKycStatusAsync(clientId, KycStatus.Ok, "KycCheckPersonService");
                }
            }
            await _kycStatusService.SaveCheckPersonResultAsync(result);
        }

        private bool isPersonSuspicious(KycCheckPersonResult result)
        {
            if (result.PersonProfiles == null || result.PersonProfiles.Count() == 0)
            {
                return false;
            }
            return true;
        }

        public KycCheckPersonResult FillCheckPersonResult(IPersonalData personalData, checkPersonResponse response)
        {
            KycCheckPersonResult resultObject = new KycCheckPersonResult
            {
                Id = personalData.Id,
                VerificationId = response.@return.verificationId
            };

            if (response.@return.personProfiles?.Length > 0)
            {
                List<IKycCheckPersonProfile> profs = new List<IKycCheckPersonProfile>();
                foreach (PersonProfile resultProfile in response.@return.personProfiles)
                {
                    IKycCheckPersonProfile p = new KycCheckPersonProfile();
                    p.Name = resultProfile.name;
                    p.Citizenships = resultProfile.citizenships.ToList();
                    p.Residences = resultProfile.residences.ToList();
                    p.MatchingLegalCategories = resultProfile.matchingLegalCategories.Select(_ => _.ToLower()).ToList();
                    p.SpiderProfileId = resultProfile.id;

                    List<String> datesOfBirth = new List<string>();
                    foreach (IncompleteDate d in resultProfile.datesOfBirth)
                    {
                        string dStr;
                        if (d.day > 0)
                        {
                            dStr = String.Format($"{d.year:00}{d.month:00}{d.day:00}");
                        }
                        else if (d.month > 0)
                        {
                            dStr = String.Format($"{d.year:00}{d.month:00}");
                        }
                        else
                        {
                            dStr = String.Format($"{d.year:00}");
                        }
                        datesOfBirth.Add(dStr);
                    }
                    p.DatesOfBirth = datesOfBirth.ToList();

                    profs.Add(p);
                }
                resultObject.PersonProfiles = profs.ToList();
            }

            return resultObject;

        }
    }

}