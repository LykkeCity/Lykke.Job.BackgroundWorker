namespace Lykke.Job.BackgroundWorker.Contract.Contexts
{
    public class SetEtheriumContractContext
    {
        public SetEtheriumContractContext(string clientId)
        {
            ClientId = clientId;
        }

        public string ClientId { get; set; }
    }
}