using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Job.BackgroundWorker.Core.Domain.Clients
{
    public interface IClientAccount
    {
        DateTime Registered { get; }
        string Id { get; }
        string Email { get; }                
    }

    public interface IClientAccountsRepository
    {
        Task<IEnumerable<string>> GetIdsAsync(string email);        
        Task SetPin(string clientId, string newPin);        
    }
}