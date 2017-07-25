namespace Lykke.Job.BackgroundWorker.Contract.Contexts
{
    public class CheckPersonContext
    {
        public string ClientId { get; set; }

        public CheckPersonContext(string id)
        {
            ClientId = id;
        }
    }

}