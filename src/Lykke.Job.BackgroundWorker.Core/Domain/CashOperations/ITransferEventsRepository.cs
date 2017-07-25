using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Job.BackgroundWorker.Core.Domain.CashOperations
{
    public interface ITransferEventsRepository
    {
        Task UpdateBlockChainHashAsync(string clientId, string id, string blockChainHash);
    }
}