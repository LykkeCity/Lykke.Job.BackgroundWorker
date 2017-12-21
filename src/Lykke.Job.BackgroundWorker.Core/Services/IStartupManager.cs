using System.Threading.Tasks;

namespace Lykke.Job.BackgroundWorker.Core.Services
{
    public interface IStartupManager
    {
        Task StartAsync();
    }
}