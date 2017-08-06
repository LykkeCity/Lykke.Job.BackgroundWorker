using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Lykke.Job.BackgroundWorker.AzureRepositories.KycCheck;
using Lykke.Job.BackgroundWorker.Core;
using Lykke.Job.BackgroundWorker.Core.Domain.Clients;
using Lykke.Job.BackgroundWorker.Core.Domain.KycCheckService;
using Lykke.Service.PersonalData.Contract.Models;

namespace Lykke.Job.BackgroundWorker.Services.KycCheckService
{
    public class KycCheckService
    {
        public async void CheckPerson(IPersonalData personalData, IKycCheckPersonResultRepository kycCheckPersonResultRepository, AppSettings.KycSpiderSettings settings)
        {
            PersonCheckData checkData = new PersonCheckData();
            checkData.residences = new string[] { };
            checkData.citizenships = new string[] { };
            checkData.datesOfBirth = new IncompleteDate[] { };

            checkData.customerId = personalData.Id;
            checkData.names = new PersonName[] {
                new PersonName() { firstName = personalData.FirstName, lastName = personalData.LastName }
            };
            checkData.citizenships = new string[] {
                personalData.CountryFromID
            };
            checkData.datesOfBirth = new IncompleteDate[] {
                new IncompleteDate() { year = personalData.DateOfBirth.Value.Year, month = personalData.DateOfBirth.Value.Month, day = personalData.DateOfBirth.Value.Day }
            };
            checkPerson request = new checkPerson(checkData);
            
            BasicHttpsBinding binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            EndpointAddress epa = new EndpointAddress(settings.EndpointUrl);
            ChannelFactory<accessChannel> factory = new ChannelFactory<accessChannel>(binding, epa);
            factory.Credentials.UserName.UserName = settings.User;
            factory.Credentials.UserName.Password = settings.Password;

            accessChannel channel = factory.CreateChannel();
            checkPersonResponse response = await channel.checkPersonAsync(request);

            SaveCheckPersonResult(personalData, response, kycCheckPersonResultRepository);
        }

        public void SaveCheckPersonResult(IPersonalData personalData, checkPersonResponse response, IKycCheckPersonResultRepository kycCheckPersonResultRepository)
        {
            KycCheckPersonResult resultObject = new KycCheckPersonResult();
            resultObject.Id = personalData.Id;
            resultObject.VerificationId = response.@return.verificationId;

            if (response.@return.personProfiles.Length > 0)
            {
                List<IKycCheckPersonProfile> profs = new List<IKycCheckPersonProfile>();
                foreach (PersonProfile resultProfile in response.@return.personProfiles)
                {
                    IKycCheckPersonProfile p = new KycCheckPersonProfile();
                    p.Name = resultProfile.name;
                    p.Citizenships = resultProfile.citizenships.ToList();
                    p.Residences = resultProfile.residences.ToList();
                    p.MatchingLegalCategories = resultProfile.matchingLegalCategories.Select(_ => _.ToLower()).ToList();

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
            kycCheckPersonResultRepository.SaveAsync(resultObject);

        }
    }

}