using System;
using System.Threading.Tasks;
using Common;
using Lykke.Job.BackgroundWorker.Contract.Contexts;
using Lykke.Job.BackgroundWorker.Core.Domain.Clients;

namespace Lykke.Job.BackgroundWorker.Components.Workers
{
    public class SetPinWorker: IWorker
    {
        private readonly IClientAccountsRepository _clientAccountsRepository;
        private SetPinContext _context;

        public SetPinWorker(IClientAccountsRepository clientAccountsRepository)
        {
            _clientAccountsRepository = clientAccountsRepository;
        }

        public async Task DoWork()
        {
            if (_context == null)
                throw new Exception("context was not set");

            await _clientAccountsRepository.SetPin(_context.ClientId, _context.Pin);
        }

        public void SetContext(string contextJson)
        {
            _context = contextJson.DeserializeJson<SetPinContext>();
            if (_context == null)
                throw new ArgumentException(nameof(contextJson));
        }
    }
}
