using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Lykke.Job.BackgroundWorker.AzureRepositories.KycCheck;
using Lykke.Job.BackgroundWorker.Core.Domain.KycCheckService;
using Lykke.Service.PersonalData.Contract;
using Lykke.Service.PersonalData.Contract.Models;

namespace Lykke.Job.BackgroundWorker.Services.KycCheckService
{
    public class KycCheckService
    {
        private readonly IPersonalDataService _personalDataService;
        private readonly IKycCheckPersonResultRepository _kycCheckPersonResultRepository;
        private readonly AppSettings.KycSpiderSettings _settings;

        public KycCheckService(IPersonalDataService personalDataService,
            IKycCheckPersonResultRepository kycCheckPersonResultRepository,
            AppSettings.KycSpiderSettings settings)
        {
            _personalDataService = personalDataService;
            _kycCheckPersonResultRepository = kycCheckPersonResultRepository;
            _settings = settings;
        }

        public async Task CheckPerson(string clientId)
        {
            var personalData = await _personalDataService.GetAsync(clientId);
            PersonCheckData checkData = new PersonCheckData
            {
                residences = new string[] {},
                citizenships = new string[] {},
                datesOfBirth = new IncompleteDate[] {},
                customerId = personalData.Id,
                names = new[]
                {
                    new PersonName {firstName = personalData.FirstName, lastName = personalData.LastName}
                }
            };

            checkData.citizenships = new [] {
                personalData.CountryFromID
            };
            checkData.datesOfBirth = new [] {
                new IncompleteDate { year = personalData.DateOfBirth?.Year ?? -1, month = personalData.DateOfBirth?.Month ?? -1, day = personalData.DateOfBirth?.Day ?? -1 }
            };
            checkPerson request = new checkPerson(checkData);

            BasicHttpsBinding binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            EndpointAddress epa = new EndpointAddress(_settings.EndpointUrl);
            ChannelFactory<accessChannel> factory = new ChannelFactory<accessChannel>(binding, epa);
            factory.Credentials.UserName.UserName = _settings.User;
            factory.Credentials.UserName.Password = _settings.Password;

            accessChannel channel = factory.CreateChannel();
            checkPersonResponse response = await channel.checkPersonAsync(request);

            await SaveCheckPersonResult(personalData, response);
        }

        public async Task SaveCheckPersonResult(IPersonalData personalData, checkPersonResponse response)
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

            await _kycCheckPersonResultRepository.SaveAsync(resultObject);

        }
    }

}