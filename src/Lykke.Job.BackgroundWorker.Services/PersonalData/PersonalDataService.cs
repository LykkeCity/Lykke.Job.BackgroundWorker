using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Flurl.Http;
using Lykke.Job.BackgroundWorker.Core;
using Lykke.Job.BackgroundWorker.Core.Domain.Clients;
using Lykke.Job.BackgroundWorker.Core.Services.PersonalData;

namespace Lykke.Job.BackgroundWorker.Services.PersonalData
{
    public class PersonalDataService : IPersonalDataService
    {
        private readonly AppSettings.PersonalDataServiceSettings _settings;
        private readonly ILog _log;

        public PersonalDataService(AppSettings.PersonalDataServiceSettings settings, ILog log)
        {
            _settings = settings;
            _log = log;
        }

        public async Task<IFullPersonalData> GetFullAsync(string id)
        {
            return await GetDataAsync<FullPersonalData>($"full/{id}");
        }

        public async Task<IEnumerable<IFullPersonalData>> GetFullAsync(IEnumerable<string> id)
        {
            return await PostDataAsync<FullPersonalData[]>(id.ToArray(), "full/list");
        }

        public Task UpdateAsync(IPersonalData personalData)
        {
            return PutDataAsync(personalData, "");
        }

        #region Helpers

        private IFlurlClient GetClient(string action)
        {
            return $"{_settings.ServiceUri}/api/PersonalData/{action}"
                .WithHeader("api-key", _settings.ApiKey);
        }

        private async Task<TResponse> GetDataAsync<TResponse>(string action)
        {
            try
            {
                return await GetClient(action)
                    .GetJsonAsync<TResponse>();
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(PersonalDataService), action, "GET", ex);
                throw;
            }
        }

        private async Task<TResponse> PostDataAsync<TResponse>(object request, string action)
        {
            try
            {
                return await GetClient(action)
                    .PostJsonAsync(request)
                    .ReceiveJson<TResponse>();
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(PersonalDataService), action, request.ToJson(), ex);
                throw;
            }
        }

        private async Task PutDataAsync(object request, string action)
        {
            try
            {
                await GetClient(action)
                    .PutJsonAsync(request);
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(PersonalDataService), action, request.ToJson(), ex);
                throw;
            }
        }


        #endregion

    }
}