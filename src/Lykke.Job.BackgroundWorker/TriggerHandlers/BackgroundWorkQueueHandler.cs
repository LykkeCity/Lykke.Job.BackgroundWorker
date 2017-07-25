using System;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Job.BackgroundWorker.Components;
using Lykke.Job.BackgroundWorker.Contract;
using Lykke.JobTriggers.Triggers.Attributes;

namespace Lykke.Job.BackgroundWorker.TriggerHandlers
{
    public class BackgroundWorkQueueHandler
    {
        private readonly IWorkersFactory _workersFactory;
        private readonly ILog _log;

        public BackgroundWorkQueueHandler(IWorkersFactory workersFactory, ILog log)
        {
            _workersFactory = workersFactory;
            _log = log;
        }

        [QueueTrigger("background-worker", 200)]
        public async Task ProcessInMessage(BackgroundWorkMessage msg)
        {
            try
            {
                await _log.WriteInfoAsync("BackgroundWorkQueueHandler", "ProcessInMessage", msg.ToJson(), "Handling message");
                var worker = _workersFactory.GetWorker(msg.WorkType, msg.ContextJson);
                await worker.DoWork();
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync("BackgroundWorkQueueHandler", "ProcessInMessage", msg.ToJson(), ex);
            }
        }
    }
}