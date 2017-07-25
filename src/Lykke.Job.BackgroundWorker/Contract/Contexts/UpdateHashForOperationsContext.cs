namespace Lykke.Job.BackgroundWorker.Contract.Contexts
{
    public class UpdateHashForOperationsContext
    {
        public UpdateHashForOperationsContext(string cmdType, string contextData, string hash)
        {
            ContextData = contextData;
            CmdType = cmdType;
            Hash = hash;
        }

        public string ContextData { get; set; }
        public string Hash { get; set; }
        public string CmdType { get; set; }
    }
}