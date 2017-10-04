/*

using Common.Log;
using Lykke.Service.Kyc.Abstractions.Domain.Verification;
using Lykke.Service.Kyc.Abstractions.Services;
using Lykke.Service.Kyc.Abstractions.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Lykke.Job.BackgroundWorker.Services.AppSettings;

namespace Lykke.Job.BackgroundWorker.Services.KycService
{
    public class KycStatusServiceProxy : LykkeServiceProxyBase, IKycStatusService
    {
        protected override string ServiceUri { get; }
        protected override string BaseUrl { get; }
        protected override string ApiKey { get; }
        protected override ILog Log { get; }


        public KycStatusServiceProxy(KycServiceSettings _kycServiceSettings, ILog log)
        {
            ServiceUri = _kycServiceSettings.ServiceUri;
            BaseUrl = "api/KycStatus";
            ApiKey = _kycServiceSettings.ApiKey;

            Log = log;
        }

        public async Task<IKycCheckPersonResult> GetCheckPersonResultAsync(string clientId)
        {
            return await GetDataAsync<KycCheckPersonResult>($"check/{clientId}");
        }

        public async Task<KycStatus> GetKycStatusAsync(string clientId)
        {
            return await GetDataAsync<KycStatus>($"id/{clientId}");
        }

        public async Task<bool> ChangeKycStatusAsync(string clientId, KycStatus kycStatus, string changer)
        {
            return await PostDataAsync<bool>($"id/{clientId}", KycChangeStatusData.Create(kycStatus, changer));
        }

        public async Task<IDictionary<KycStatus, IEnumerable<string>>> GetClientsByStatusAsync(params KycStatus[] statuses)
        {
            return await PostDataAsync<Dictionary<KycStatus, IEnumerable<string>>>($"status/clients", statuses);
        }

        public async Task<IDictionary<KycStatus, int>> GetCountsByStatusAsync(params KycStatus[] statuses)
        {
            return await PostDataAsync<Dictionary<KycStatus, int>>($"status/counts", statuses);
        }
    }
}


*/