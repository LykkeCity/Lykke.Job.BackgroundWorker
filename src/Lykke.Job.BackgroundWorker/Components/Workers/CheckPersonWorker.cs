using System;
using System.Threading.Tasks;
using Common;
using Lykke.Job.BackgroundWorker.Contract.Contexts;
using Lykke.Job.BackgroundWorker.Services.KycCheckService;

namespace Lykke.Job.BackgroundWorker.Components.Workers
{
    public class CheckPersonWorker: IWorker
    {
        private CheckPersonContext _context;
        private readonly KycCheckService _kycCheckService;


        public CheckPersonWorker(KycCheckService kycCheckService)
        {
            _kycCheckService = kycCheckService;
        }

        public async Task DoWork()
        {
            if (_context == null)
                throw new Exception("context was not set");

            await _kycCheckService.CheckPerson(_context.ClientId);
       }

        public void SetContext(string contextJson)
        {
            _context = contextJson.DeserializeJson<CheckPersonContext>();
            if (_context == null)
                throw new ArgumentException(nameof(contextJson));
        }
    }
}
