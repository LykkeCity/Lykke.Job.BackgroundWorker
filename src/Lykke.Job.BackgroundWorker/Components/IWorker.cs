using System.Threading.Tasks;

namespace Lykke.Job.BackgroundWorker.Components
{
    public interface IWorker
    {
        Task DoWork();
        void SetContext(string contextJson);
    }
}