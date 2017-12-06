using System.Threading.Tasks;

namespace Lykke.Job.BackgroundWorker.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}