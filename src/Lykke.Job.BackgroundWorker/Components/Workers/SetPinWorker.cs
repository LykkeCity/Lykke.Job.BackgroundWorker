using System;
using System.Threading.Tasks;

using Common;

using Lykke.Job.BackgroundWorker.Contract.Contexts;
using Lykke.Service.ClientAccount.Client;

namespace Lykke.Job.BackgroundWorker.Components.Workers
{
    public class SetPinWorker: IWorker
    {
        private readonly IClientAccountClient _clientAccountsRepository;
        private SetPinContext _context;

        public SetPinWorker(IClientAccountClient clientAccountsRepository)
        {
            _clientAccountsRepository = clientAccountsRepository;
        }

        public async Task DoWork()
        {
            if (_context == null)
            {
                throw new InvalidOperationException("Context was not set.");
            }

            await _clientAccountsRepository.SetPIN(_context.ClientId, _context.Pin);
        }

        public void SetContext(string contextJson)
        {
            _context = contextJson.DeserializeJson<SetPinContext>();
            if (_context == null)
            {
                throw new ArgumentException(nameof(contextJson));
            }
        }
    }
}
