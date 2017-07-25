using Lykke.Job.BackgroundWorker.Contract;

namespace Lykke.Job.BackgroundWorker.Components
{
    public interface IWorkersFactory
    {
        IWorker GetWorker(WorkType workType, string contextJson);
    }
}