using Common;

namespace Lykke.Job.BackgroundWorker.Contract
{
    public class BackgroundWorkMessage
    {
        public WorkType WorkType { get; set; }
        public string ContextJson { get; set; }
    }

    public class BackgroundWorkMessage<T> : BackgroundWorkMessage
    {
        public BackgroundWorkMessage(WorkType workType, T contextObj)
        {
            ContextJson = contextObj.ToJson();
            WorkType = workType;
        }
    }
}