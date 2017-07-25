using System.Threading.Tasks;

namespace Lykke.Job.BackgroundWorker.Core.Domain.CashOperations
{
    public interface ICashOperationsRepository
    {
        Task UpdateBlockchainHashAsync(string clientId, string id, string hash);
    }
}