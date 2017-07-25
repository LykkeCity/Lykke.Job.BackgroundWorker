using System.Threading.Tasks;

namespace Lykke.Job.BackgroundWorker.Core.Domain.CashOperations
{
    public interface IClientTradesRepository
    {
        Task UpdateBlockChainHashAsync(string clientId, string recordId, string hash);
    }
}