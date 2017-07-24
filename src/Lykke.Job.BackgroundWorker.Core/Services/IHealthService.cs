
namespace Lykke.Job.BackgroundWorker.Core.Services
{
    public interface IHealthService
    {
        string GetHealthViolationMessage();
        string GetHealthWarningMessage();
    }
}