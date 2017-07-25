using System;
using System.Threading.Tasks;
using Common;
using Lykke.Job.BackgroundWorker.Contract.Contexts;
using Lykke.Job.BackgroundWorker.Core;
using Lykke.Job.BackgroundWorker.Core.Domain.Clients;
using Lykke.Job.BackgroundWorker.Core.Domain.KycCheckService;
using Lykke.Job.BackgroundWorker.Services.KycCheckService;

namespace Lykke.Job.BackgroundWorker.Components.Workers
{
    public class CheckPersonWorker: IWorker
    {
        private CheckPersonContext _context;
        private readonly IKycCheckPersonResultRepository _kycCheckPersonResultRepository;
        private readonly IPersonalDataRepository _personalDataRepository;
        private readonly AppSettings.KycSpiderSettings _kycSpiderSettings;


        public CheckPersonWorker(
            IPersonalDataRepository personalDataRepository, 
            IKycCheckPersonResultRepository kycCheckPersonResultRepository, 
            AppSettings.KycSpiderSettings kycSpiderSettings)
        {
            _kycCheckPersonResultRepository = kycCheckPersonResultRepository;
            _personalDataRepository = personalDataRepository;
            _kycSpiderSettings = kycSpiderSettings;
        }

        public async Task DoWork()
        {
            if (_context == null)
                throw new Exception("context was not set");

            IPersonalData personalData = await _personalDataRepository.GetAsync(_context.ClientId);

            KycCheckService s = new KycCheckService();
            s.CheckPerson(personalData, _kycCheckPersonResultRepository, _kycSpiderSettings);
       }

        public void SetContext(string contextJson)
        {
            _context = contextJson.DeserializeJson<CheckPersonContext>();
            if (_context == null)
                throw new ArgumentException(nameof(contextJson));
        }
    }
}
